using System;
using System.Collections;
using System.Data;
using NHibernate;
using NHibernate.Expression;
using VDMS.Common.Data;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.IDAL.Interface;

namespace VDMS.I.ObjectDataSource.RepairList
{
	#region declare objects
	public class RepairListHeader : TreeListObject
	{
		private int no;
		private string engineNumber, custName;
		private DateTime repairDate;
		private ServiceRecordsList serviceRecordSheet;
		private ExchangeSparesList exchangeSpares;

		public int No
		{
			get { return no; }
			set { no = value; }
		}
		public string EngineNumber
		{
			get { return engineNumber; }
			set { engineNumber = value; }
		}
		public string CustomerName
		{
			get { return custName; }
			set { custName = value; }
		}
		public DateTime RepairDate
		{
			get { return repairDate; }
			set { repairDate = value; }
		}

		public ServiceRecordsList ServiceRecordSheet
		{
			get { return serviceRecordSheet; }
			set { serviceRecordSheet = value; }
		}
		public ExchangeSparesList ExchangeSpares
		{
			get { return exchangeSpares; }
			set { exchangeSpares = value; }
		}

		public RepairListHeader()
		{
			ServiceRecordSheet = new ServiceRecordsList();
			ExchangeSpares = new ExchangeSparesList();
		}
		public RepairListHeader(string engineNum, string custName, DateTime repairDate)
			: this()
		{
			this.EngineNumber = engineNum;
			this.CustomerName = custName;
			this.RepairDate = repairDate;
		}
	}

	public class RepairSpare : TreeListObject
	{
		private string spareNumber;
		private int quantity;
		private long unitPrice, spareAmount;

		public string SpareNumber
		{
			get { return spareNumber; }
			set { spareNumber = value; }
		}
		public int Quantity
		{
			get { return quantity; }
			set { quantity = value; }
		}
		public long UnitPrice
		{
			get { return unitPrice; }
			set { unitPrice = value; }
		}
		public long RepairSpareAmount
		{
			get { return spareAmount; }
			set { spareAmount = value; }
		}
		public RepairSpare() { }
		public RepairSpare(string spareNum, int quantity, long unitPrice)
		{
			this.Quantity = quantity;
			this.SpareNumber = spareNum;
			this.UnitPrice = unitPrice;
			this.RepairSpareAmount = Quantity * UnitPrice;
		}
	}
	public class ExchangeSpare : TreeListObject
	{
		private string spareNumber;
		private int quantity;
		private long unitPrice, spareAmount, feeAmount;

		public string SpareNumber
		{
			get { return spareNumber; }
			set { spareNumber = value; }
		}
		public int Quantity
		{
			get { return quantity; }
			set { quantity = value; }
		}
		public long UnitPrice
		{
			get { return unitPrice; }
			set { unitPrice = value; }
		}
		public long WarrantySpareAmount
		{
			get { return spareAmount; }
			set { spareAmount = value; }
		}
		public long WarrantyFeeAmount
		{
			get { return feeAmount; }
			set { feeAmount = value; }
		}

		public ExchangeSpare() { }
		public ExchangeSpare(string spareNum, int quantity, long unitPrice, long fee)
		{
			this.Quantity = quantity;
			this.SpareNumber = spareNum;
			this.UnitPrice = unitPrice;
			this.WarrantySpareAmount = Quantity * UnitPrice;
			this.WarrantyFeeAmount = fee;
		}
	}

	public class ExchangeSparePartsList : TreeListObjects
	{
		private int partNum;

		public int PartNum
		{
			get { return partNum; }
			set { partNum = value; }
		}

		public ExchangeSparePartsList()
			: base()
		{
		}
		public ExchangeSparePartsList(int num)
			: this()
		{
			PartNum = num;
		}
	}
	public class ServiceRecordsList : TreeListObjects
	{
		private string sheetNumber;
		private long feeAmount;

		public string ServiceSheetNumber
		{
			get { return sheetNumber; }
			set { sheetNumber = value; }
		}
		public long RepairFeeAmount
		{
			get { return feeAmount; }
			set { feeAmount = value; }
		}

		public ServiceRecordsList()
			: base()
		{
		}
		public ServiceRecordsList(string sheetNum)
			: this()
		{

		}
	}
	public class ExchangeSparesList : TreeListObjects
	{
		private string voucherNumber;

		public string ExchangeVoucherNumber
		{
			get { return voucherNumber; }
			set { voucherNumber = value; }
		}

		public ExchangeSparesList()
			: base()
		{
		}
		public ExchangeSparesList(string voucherNumber)
			: this()
		{

		}
	}
	#endregion

	public class RepairListDataSource
	{
		private int count = 0, pageSize, firstItemIndex;
		long totalSpareAmount, totalRepairFee, totalWarrantyAmount, totalWarrantyFee;
		private DateTime repairFrom, repairTo, buyFrom, buyTo;
		private string serNumFrom, serNumTo, custName, engineNum, branchCode;

		public long TotalSpareAmount
		{
			get { return totalSpareAmount; }
			set { totalSpareAmount = value; }
		}
		public long TotalRepairFee
		{
			get { return totalRepairFee; }
			set { totalRepairFee = value; }
		}
		public long TotalWarrantyAmount
		{
			get { return totalWarrantyAmount; }
			set { totalWarrantyAmount = value; }
		}
		public long TotalWarrantyFee
		{
			get { return totalWarrantyFee; }
			set { totalWarrantyFee = value; }
		}
		public int ItemCount
		{
			get { return count; }
			set { count = value; }
		}
		public int FirstItemIndex
		{
			get { return firstItemIndex; }
			set { firstItemIndex = value; }
		}
		public int PageSize
		{
			get { return pageSize; }
			set { pageSize = value; }
		}
		public int PageCount
		{
			get { return (pageSize == 0) ? 0 : (ItemCount / PageSize) + (((ItemCount % PageSize) > 0) ? 1 : 0); }
		}
		public int PageIndex
		{
			get { return (pageSize == 0) ? 0 : FirstItemIndex / PageSize; }
		}
		public DateTime RepairFromDate
		{
			get { return repairFrom; }
			set { repairFrom = value; }
		}
		public DateTime RepairToDate
		{
			get { return repairTo; }
			set { repairTo = value; }
		}
		public DateTime BuyFromDate
		{
			get { return buyFrom; }
			set { buyFrom = value; }
		}
		public DateTime BuyToDate
		{
			get { return buyTo; }
			set { buyTo = value; }
		}
		public string ServiceNumFrom
		{
			get { return serNumFrom; }
			set { serNumFrom = (value != null) ? value.Trim().ToUpper() : null; }
		}
		public string ServiceNumTo
		{
			get { return serNumTo; }
			set { serNumTo = (value != null) ? value.Trim().ToUpper() : null; }
		}
		public string CustomerName
		{
			get { return custName; }
			set { custName = (value != null) ? value.Trim() : null; }
		}
		public string EngineNum
		{
			get { return engineNum; }
			set { engineNum = (value != null) ? value.Trim().ToUpper() : null; }
		}
		public string BranchCode
		{
			get { return branchCode; }
			set { branchCode = (value != null) ? value.Trim().ToUpper() : null; }
		}

		private void CalculateSummary(RepairListHeader obj)
		{
			TotalRepairFee += obj.ServiceRecordSheet.RepairFeeAmount;
			foreach (RepairSpare spare in obj.ServiceRecordSheet.Items)
			{

				TotalSpareAmount += spare.Quantity * spare.UnitPrice;
			}
			foreach (ExchangeSpare spare in obj.ExchangeSpares.Items)
			{
				TotalWarrantyFee += spare.WarrantyFeeAmount;
				TotalWarrantyAmount += spare.Quantity * spare.UnitPrice;
			}
		}

		public static DataTable RepairListTableSchema
		{
			get
			{
				DataTable tbl = new DataTable();
				tbl.Columns.Add("No");
				tbl.Columns.Add("RepairDate");
				tbl.Columns.Add("EngineNumber");
				tbl.Columns.Add("CustomerName");
				tbl.Columns.Add("ServiceSheetNumber");
				tbl.Columns.Add("ExchangeVoucherNumber");
				tbl.Columns.Add("SpareNumber");
				tbl.Columns.Add("Quantity");
				tbl.Columns.Add("UnitPrice");
				tbl.Columns.Add("WarrantySpareAmount");
				tbl.Columns.Add("WarrantyFeeAmount");
				tbl.Columns.Add("RepairSpareAmount");
				tbl.Columns.Add("RepairFeeAmount");
				return tbl;
			}
		}
		public int SelectCount(int maximumRows, int startRowIndex)
		{
			return count;
		}
		public TreeListObjects Select(int maximumRows, int startRowIndex)
		{
			// int page = startRowIndex / maximumRows;

			TreeListObjects data = new TreeListObjects(false); // do not bind exchange row if not exist
			RepairListHeader header = new RepairListHeader();
			RepairSpare rSpare;
			ExchangeSpare exSpare;

			Exchangepartheader exchH;
			ArrayList listED;
			ArrayList listSD;
			ArrayList listSH = GetServiceHeaders(branchCode);

			ItemCount = (listSH == null) ? 0 : listSH.Count;
			PageSize = maximumRows;
			FirstItemIndex = startRowIndex;

			int lastItem = FirstItemIndex + PageSize;
			if (lastItem > ItemCount) lastItem = ItemCount;

			if ((listSH == null) || (listSH.Count == 0)) return null;
			for (int i = 0; i < ItemCount; i++)
			{
				Serviceheader serH = (Serviceheader)listSH[i];
				// header section
				header = new RepairListHeader();
				header.No = i + 1;
				header.CustomerName = serH.Customer.Fullname;
				header.EngineNumber = serH.Enginenumber;
				header.RepairDate = serH.Servicedate;

				// repair section
				header.ServiceRecordSheet.BindOrder = 1;
				header.ServiceRecordSheet.ServiceSheetNumber = serH.Servicesheetnumber;
				header.ServiceRecordSheet.RepairFeeAmount = serH.Feeamount;
				listSD = GetServiceDetails(serH.Id);
				foreach (Servicedetail serD in listSD)
				{
					rSpare = new RepairSpare(serD.Partcode, serD.Partqty, serD.Unitprice);
					header.ServiceRecordSheet.Items.Add(rSpare);
				}

				// exchange section
				header.ExchangeSpares.BindOrder = 2;
				exchH = GetExchangeHeader(serH.Id);
				if (exchH != null)
				{
					header.ExchangeSpares.ExchangeVoucherNumber = exchH.Vouchernumber;
					listED = GetExchangeDetails(exchH.Id);
					foreach (Exchangepartdetail exchD in listED)
					{
						exSpare = new ExchangeSpare(exchD.Partcodem, exchD.Partqtym, exchD.Unitpricem, exchD.Totalfeem);
						header.ExchangeSpares.Items.Add(exSpare);
					}
				}

				CalculateSummary(header);
				// add to top list 
				if ((i >= FirstItemIndex) && (i < lastItem)) data.Items.Add(header);
			}

			return data;
		}

		public ArrayList GetExchangeDetails(long exchangeId)
		{
			return GetExchangeSpares(exchangeId);
		}
		public ArrayList GetServiceDetails(long sheetId)
		{
			NHibernate.ISession session = NHibernateSessionManager.Instance.GetSession();
			object obj = session.CreateCriteria(typeof(Servicedetail))
							.Add(Expression.Eq("Serviceheader.Id", sheetId)).List();
			return (ArrayList)obj;
		}

		public Exchangepartheader GetExchangeHeader(long sheetId)
		{
			try
			{
				NHibernate.ISession session = NHibernateSessionManager.Instance.GetSession();
				ArrayList list = (ArrayList)session.CreateCriteria(typeof(Exchangepartheader))
                                    .Add(Expression.Eq("Serviceheader.Id", sheetId))
                                    //.Add(Expression.Ge("Status",0))
                                    .List();
				return (list.Count == 1) ? (Exchangepartheader)list[0] : null;
			}
			catch
			{
				return null;
			}
		}
		public ArrayList GetServiceHeaders(string branchCode) //int start, int max, 
		{
			NHibernate.ISession session = NHibernateSessionManager.Instance.GetSession();
			IList custs = null;

			if (!string.IsNullOrEmpty(CustomerName))
			{
				custs = session.CreateCriteria(typeof(Customer)).Add(Expression.InsensitiveLike("Fullname", "%" + CustomerName + "%")).List();
			}

			// searching conditions
			ICriteria crit = session.CreateCriteria(typeof(Serviceheader));
            crit.Add(Expression.Ge("Status", 0));
			if (!string.IsNullOrEmpty(BranchCode)) crit.Add(Expression.Eq("Branchcode", BranchCode));
			if ((RepairFromDate > DateTime.MinValue)) crit.Add(Expression.Ge("Servicedate", RepairFromDate));
			if ((RepairToDate > DateTime.MinValue)) crit.Add(Expression.Le("Servicedate", RepairToDate));
			if ((BuyFromDate > DateTime.MinValue)) crit.Add(Expression.Ge("Purchasedate", BuyFromDate));
			if ((BuyToDate > DateTime.MinValue)) crit.Add(Expression.Le("Purchasedate", BuyToDate));
			if (!string.IsNullOrEmpty(ServiceNumFrom)) crit.Add(Expression.Ge("Servicesheetnumber", ServiceNumFrom));
			if (!string.IsNullOrEmpty(ServiceNumTo)) crit.Add(Expression.Le("Servicesheetnumber", ServiceNumTo));
			if (!string.IsNullOrEmpty(EngineNum)) crit.Add(Expression.Eq("Enginenumber", EngineNum));
			if (custs != null) crit.Add(Expression.In("Customer", custs));

			//count = crit.
			//crit.SetFirstResult(start);
			//crit.SetMaxResults(max);
			crit.AddOrder(Order.Desc("Servicedate")).AddOrder(Order.Desc("Servicesheetnumber"));

			ArrayList list = (ArrayList)crit.List();
			return list;
		}
		public static Serviceheader GetServiceHeader(string sheetNo)
		{
			IDao<Serviceheader, long> dao = DaoFactory.GetDao<Serviceheader, long>();
			dao.SetCriteria(new ICriterion[] {Expression.Ge("Status", 0), Expression.Eq("Servicesheetnumber", sheetNo.Trim().ToUpper()) });
			IList list = dao.GetAll();
			if (list.Count != 1) return null;
			return (Serviceheader)list[0];
		}
		public static ArrayList GetExchangeSpares(long exchangeId)
		{
			NHibernate.ISession session = NHibernateSessionManager.Instance.GetSession();
			return (ArrayList)session.CreateCriteria(typeof(Exchangepartdetail))
                    .Add(Expression.Eq("Exchangepartheader.Id", exchangeId))
                    .List();
		}
		public static Exchangepartheader GetExchangeHeader(string exchangeNo)
		{
			IDao<Exchangepartheader, long> dao = DaoFactory.GetDao<Exchangepartheader, long>();
			dao.SetCriteria(new ICriterion[] {//Expression.Ge("Status",0), 
                    Expression.Eq("Vouchernumber", exchangeNo.Trim().ToUpper()) });
			IList list = dao.GetAll();
			if (list.Count != 1) return null;
			return (Exchangepartheader)list[0];
		}
		public static Exchangepartheader GetExchangeHeaderFromServiceSheet(long sheetId)
		{
			IDao<Exchangepartheader, long> dao = DaoFactory.GetDao<Exchangepartheader, long>();
			dao.SetCriteria(new ICriterion[] {Expression.Ge("Status",0), Expression.Eq("Serviceheader.Id", sheetId) });
			IList list = dao.GetAll();
			if (list.Count != 1) return null;
			return (Exchangepartheader)list[0];
		}
		public RepairListDataSource()
		{
			RepairFromDate = DateTime.MinValue;
			RepairToDate = DateTime.MinValue;
			BuyFromDate = DateTime.MinValue;
			BuyToDate = DateTime.MinValue;
			TotalRepairFee = 0;
			TotalSpareAmount = 0;
			TotalWarrantyAmount = 0;
			TotalWarrantyFee = 0;
		}

	}
}