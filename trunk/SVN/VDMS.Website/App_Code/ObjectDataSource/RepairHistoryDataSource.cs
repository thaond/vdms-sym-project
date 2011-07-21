using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using NHibernate;
using NHibernate.Expression;
using VDMS.I.Service;
using VDMS.Common.Data;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.IDAL.Interface;
using VDMS.Helper;


namespace VDMS.I.ObjectDataSource.RepairHistory
{
	#region declare objects
	public class RepairListHeader : TreeListObject
	{
		int no;
		long repairFee, serviceHeaderId;
		private string damaged, repair, dealer, dealerName;
		private DateTime repairDate;
		private SpareList spares;

		public int No
		{
			get { return no; }
			set { no = value; }
		}
		public DateTime RepairDate
		{
			get { return repairDate; }
			set { repairDate = value; }
		}
		public string Damaged
		{
			get { return damaged; }
			set { damaged = (value != null) ? value.Trim() : null; }
		}
		public string Repair
		{
			get { return repair; }
			set { repair = (value != null) ? value.Trim() : null; }
		}
		public string Dealer
		{
			get { return dealer; }
			set { dealer = (value != null) ? value.Trim() : null; }
		}
		public string DealerName
		{
			get { return dealerName; }
			set { dealerName = (value != null) ? value.Trim() : null; }
		}
		public long RepairFee
		{
			get { return repairFee; }
			set { repairFee = value; }
		}
		public long ServiceHeaderId
		{
			get { return serviceHeaderId; }
			set { serviceHeaderId = value; }
		}

		public SpareList Spares
		{
			get { return spares; }
			set { spares = value; }
		}

		public RepairListHeader()
		{
			Spares = new SpareList();
		}
		public RepairListHeader(string _damaged, string _repair, DateTime repairDate)
			: this()
		{
			this.Damaged = _damaged;
			this.Repair = _repair;
			this.RepairDate = repairDate;
		}
	}
	public class SpareList : TreeListObjects
	{
		public SpareList()
			: base()
		{
		}
		public SpareList(string sheetNum)
			: this()
		{
			//this.SheetVoucherNumber = sheetNum;
		}
	}
	public class SpareInfo : TreeListObject
	{
		private string spareNumber;
		bool isWarranty;
		private string sheetNumber;

		public string SheetVoucherNumber// store serviceSheet or voucher number
		{
			get { return sheetNumber; }
			set { sheetNumber = (value != null) ? value.Trim().ToUpper() : null; }
		}
		public bool IsWarranty
		{
			get { return isWarranty; }
			set { isWarranty = value; }
		}
		public string SpareNumber
		{
			get { return spareNumber; }
			set { spareNumber = value; }
		}
		public SpareInfo() { }
		public SpareInfo(string spareNum, string sheetNo)
		{
			this.SpareNumber = spareNum;
			this.SheetVoucherNumber = sheetNo;
		}
	}

	#endregion

	public class RepairHistoryDataSource
	{
		private static List<int> voucherStatusCanDelete = null;
		private string engineNumber;
		public string EngineNumber
		{
			get { return engineNumber; }
			set { engineNumber = value; }
		}

		public static DataTable RepairListTableSchema
		{
			get
			{
				DataTable tbl = new DataTable();
				tbl.Columns.Add("RepairDate");
				tbl.Columns.Add("Dealer");
				tbl.Columns.Add("DealerName");
				tbl.Columns.Add("Damaged");
				tbl.Columns.Add("Repair");
				tbl.Columns.Add("RepairFee");
				tbl.Columns.Add("SheetVoucherNumber");
				tbl.Columns.Add("SpareNumber");
				tbl.Columns.Add("IsWarranty");
				tbl.Columns.Add("ServiceHeaderId");

				return tbl;
			}
		}

		// chua phan trang
		public TreeListObjects Select(int maximumRows, int startRowIndex)
		{
			// int page = startRowIndex / maximumRows;

			TreeListObjects data = new TreeListObjects();
			RepairListHeader header;
			SpareInfo spare;


			Exchangepartheader exchH;
			ArrayList listED;
			IList listSD;
			ArrayList listSH = GetServiceHeaders(startRowIndex, maximumRows);

			for (int i = 0; i < listSH.Count; i++)
			{
				Serviceheader serH = (Serviceheader)listSH[i];
				// header section
				header = new RepairListHeader();
				header.No = startRowIndex + i + 1;
				header.Repair = serH.Repairresult;
				header.Damaged = serH.Damaged;
				header.RepairDate = serH.Servicedate;
				header.Dealer = serH.Dealercode;
				header.DealerName = DealerHelper.GetName(serH.Dealercode);
				header.RepairFee = serH.Feeamount;
				header.ServiceHeaderId = serH.Id;

				// repair section
				listSD = GetServiceDetails(serH.Id);
				foreach (Servicedetail serD in listSD)
				{
					spare = new SpareInfo(serD.Partcode, serH.Servicesheetnumber);
					spare.IsWarranty = false;
					header.Spares.Items.Add(spare);
				}

				// exchange section
				exchH = GetExchangeHeader(serH.Id);
				if (exchH != null)
				{
					listED = GetExchangeDetails(exchH.Id);
					foreach (Exchangepartdetail exchD in listED)
					{
						spare = new SpareInfo(exchD.Partcodem, exchH.Vouchernumber);
						spare.IsWarranty = true; ;
						header.Spares.Items.Add(spare);
					}
				}

				// add to top list 
				data.Items.Add(header);
			}
			return data;
		}

		public ArrayList GetExchangeDetails(long exchangeId)
		{
			NHibernate.ISession session = NHibernateSessionManager.Instance.GetSession();
			ArrayList list = (ArrayList)session.CreateCriteria(typeof(Exchangepartdetail)).Add(Expression.Eq("Exchangepartheader.Id", exchangeId)).List();
			/////////////////////////////////////////////////////////
			//Exchangepartdetail ed;
			//if (list.Count == 0)
			//{
			//    ed = new Exchangepartdetail();
			//    ed.Partcodem = exchangeId.ToString() + ": Partcodem 1";
			//    list.Add(ed);
			//    ed = new Exchangepartdetail();
			//    ed.Partcodem = exchangeId.ToString() + ": Partcodem 2";
			//    list.Add(ed);
			//}
			/////////////////////////////////////////////////////////
			return list;
		}
		public Exchangepartheader GetExchangeHeader(long sheetId)
		{
			NHibernate.ISession session = NHibernateSessionManager.Instance.GetSession();
			ArrayList list = (ArrayList)session.CreateCriteria(typeof(Exchangepartheader))
								.Add(Expression.Eq("Serviceheader.Id", sheetId))
								.Add(Expression.Ge("Status", 0))
								.List();
			/////////////////////////////////////////////////////////
			//Exchangepartheader eh;
			//if (list.Count == 0)
			//{
			//    eh = new Exchangepartheader();
			//    eh.Vouchernumber = sheetId.ToString() + ": Vouchernumber 1";
			//    list.Add(eh);
			//}
			/////////////////////////////////////////////////////////
			return (list.Count == 1) ? (Exchangepartheader)list[0] : null;
		}
		public IList GetServiceDetails(long sheetId)
		{
			NHibernate.ISession session = NHibernateSessionManager.Instance.GetSession();
			IList obj = session.CreateCriteria(typeof(Servicedetail))
							.Add(Expression.Eq("Serviceheader.Id", sheetId)).List();

			/////////////////////////////////////////////////////////
			//Servicedetail sd;
			//if (obj.Count == 0)
			//{
			//    sd = new Servicedetail(); sd.Partcode = sheetId.ToString() + ": Partcode1";
			//    sd.Serviceheader = new Serviceheader();
			//    obj.Add(sd);
			//    sd = new Servicedetail(); sd.Partcode = sheetId.ToString() + ": Partcode2";
			//    sd.Serviceheader = new Serviceheader();
			//    obj.Add(sd);
			//}
			/////////////////////////////////////////////////////////

			return obj;
		}
		public ArrayList GetServiceHeaders(int start, int max)
		{
			if (string.IsNullOrEmpty(EngineNumber)) return null;

			NHibernate.ISession session = NHibernateSessionManager.Instance.GetSession();

			// searching conditions
			ICriteria crit = session.CreateCriteria(typeof(Serviceheader));
			crit.Add(Expression.Eq("Enginenumber", EngineNumber));
			crit.Add(Expression.Ge("Status", 0));

			//crit.SetFirstResult(start);
			//crit.SetMaxResults(max);
			crit.AddOrder(Order.Desc("Servicedate"));

			ArrayList list = (ArrayList)crit.List();

			return list;
		}
		public static Serviceheader GetServiceHeader(string sheetNo)
		{
			IDao<Serviceheader, long> dao = DaoFactory.GetDao<Serviceheader, long>();
			dao.SetCriteria(new ICriterion[] { Expression.Ge("Status", 0), Expression.Eq("Servicesheetnumber", sheetNo.Trim().ToUpper()) });
			IList list = dao.GetAll();
			if (list.Count != 1) return null;
			return (Serviceheader)list[0];
		}
		public static void DeleteSRS(long sheetId)
		{
			if (CanDeleteSRS(sheetId))
			{
				using (TransactionBlock tran = new TransactionBlock())
				{
					try
					{
						IDao<Serviceheader, long> daos = DaoFactory.GetDao<Serviceheader, long>();
						Serviceheader sh = daos.GetById(sheetId, true);

						if ((sh != null)
							//&& (sh.Status >= 0)
						   )
						{
							IDao<Servicedetail, long> daosd = DaoFactory.GetDao<Servicedetail, long>();
							IDao<Exchangepartheader, long> daox = DaoFactory.GetDao<Exchangepartheader, long>();
							IDao<Exchangepartdetail, long> daoxd = DaoFactory.GetDao<Exchangepartdetail, long>();

							// delete exchange part
							daox.SetCriteria(new ICriterion[] { //Expression.Ge("Status", 0), 
                                                Expression.Eq("Serviceheader.Id", sheetId) });
							foreach (Exchangepartheader item in daox.GetAll())
							{
								daoxd.SetCriteria(new ICriterion[] { Expression.Eq("Exchangepartheader.Id", item.Id) });
								foreach (Exchangepartdetail xDetail in daoxd.GetAll())
								{
									daoxd.Delete(xDetail);
								}
								daox.Delete(item);
							}

							// delete service sheet
							daosd.SetCriteria(new ICriterion[] { Expression.Eq("Serviceheader.Id", sheetId) });
							foreach (Servicedetail item in daosd.GetAll())
							{
								daosd.Delete(item);
							}
							daos.Delete(sh);
						}
						tran.IsValid = true;
					}
					catch
					{
						tran.IsValid = false;
					}
				}
			}
		}
		public static bool CanDeleteSRS(long sheetId)
		{
			bool result = true;
			IDao<Exchangepartheader, long> daox = DaoFactory.GetDao<Exchangepartheader, long>();

			// get exchange part header that not allow delete
			daox.SetCriteria(new ICriterion[] { Expression.Ge("Status", 0), Expression.Eq("Serviceheader.Id", sheetId), Expression.Not(Expression.In("Status", GetExchangepartHeaderStatusToDelete())) });
			List<Exchangepartheader> list = daox.GetAll();
			result = result && (list.Count <= 0);

			return result;
		}

		public static List<int> GetExchangepartHeaderStatusToDelete()
		{
			if (voucherStatusCanDelete == null)
			{
				voucherStatusCanDelete = new List<int>();
				voucherStatusCanDelete.Add((int)ExchangeVoucherStatus.Reject);
				voucherStatusCanDelete.Add((int)ExchangeVoucherStatus.New);
				voucherStatusCanDelete.Add((int)ExchangeVoucherStatus.Canceled);
			}

			return voucherStatusCanDelete;
		}
		public static bool ExchangePartHeaderCanDelete(int status)
		{
			foreach (int item in GetExchangepartHeaderStatusToDelete())
			{
				if (item == status) return true;
			}
			return false;
		}

		public RepairHistoryDataSource(string EngineNum)
		{
			this.EngineNumber = EngineNum;
		}

	}
}