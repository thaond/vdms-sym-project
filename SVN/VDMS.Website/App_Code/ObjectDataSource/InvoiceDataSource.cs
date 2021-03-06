using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web;
using NHibernate;
using NHibernate.Expression;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Helper;
using VDMS.Common.Utils;

namespace VDMS.I.ObjectDataSource
{
	public class InvoiceDataSource
	{
		public InvoiceDataSource() { }
		private int count = 0;
		//public int SelectCount(int maximumRows, int startRowIndex)
		public int SelectCount(int maximumRows, int startRowIndex, string EngineNumber, string NumberPlate, string InvoiceNumber, string fromDate, string toDate, string AreaCode, string DealerCode, string IdentifyNumber, string Fullname, string Address, string Precinct, string DistrictId, string ProvinceId)
		{
			return count;
		}
		public int SelectCount(int maximumRows, int startRowIndex, string EngineNumber, string NumberPlate, string InvoiceNumber, string fromDate, string toDate, string AreaCode, string DealerCode, string IdentifyNumber, string Fullname, string Address, string Precinct, string DistrictId, string ProvinceId, string Model)
		{
			return count;
		}
		

		private List<T> TableToList<T>(DataTable tbl, int column)
		{
			List<T> list = new List<T>();
			foreach (DataRow row in tbl.Rows)
			{
				list.Add((T)row[column]);
			}
			return list;
		}
		private List<T> MergeList<T>(List<T> list, List<T> add)
		{
			foreach (T item in add)
			{
				list.Add(item);
			}
			return list;
		}

		public IList<Invoice> Select(int maximumRows, int startRowIndex, string EngineNumber, string NumberPlate, string InvoiceNumber, string fromDate, string toDate, string AreaCode, string DealerCode, string IdentifyNumber, string Fullname, string Address, string Precinct, string DistrictId, string ProvinceId)
		{
			return Select(maximumRows, startRowIndex, EngineNumber, NumberPlate, InvoiceNumber, fromDate, toDate, AreaCode, DealerCode, IdentifyNumber, Fullname, Address, Precinct, DistrictId, ProvinceId, null);
		}
		public IList<Invoice> Select(int maximumRows, int startRowIndex, string EngineNumber, string NumberPlate, string InvoiceNumber, string fromDate, string toDate, string AreaCode, string DealerCode, string IdentifyNumber, string Fullname, string Address, string Precinct, string DistrictId, string ProvinceId, string Model)
		{
			ISession sess = NHibernateSessionManager.Instance.GetSession();

			ICriteria icrIv = sess.CreateCriteria(typeof(Invoice));
            if (!string.IsNullOrEmpty(EngineNumber)) icrIv.Add(Expression.InsensitiveLike("Enginenumber", "%" + EngineNumber.Trim() + "%"));
            if (!string.IsNullOrEmpty(InvoiceNumber)) icrIv.Add(Expression.InsensitiveLike("Invoicenumber", "%" + InvoiceNumber.Trim() + "%"));

			if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
			{
                DateTime dtfromDate = DataFormat.DateFromString(fromDate), 
                         dttoDate = DataFormat.DateFromString(toDate);
				icrIv.Add(Expression.Between("Createddate", dtfromDate.Date, dttoDate.Date.AddDays(1)));
			}

            if (!string.IsNullOrEmpty(NumberPlate)) icrIv.CreateCriteria("Sellitem", "si").Add(Expression.InsensitiveLike("Numberplate", "%" + NumberPlate + "%"));

			ICriteria icrII = icrIv.CreateCriteria("Iteminstance", "ii");

			if (!string.IsNullOrEmpty(Model))
			{
				icrII.Add(Expression.InsensitiveLike("Itemtype", "%" + Model.Trim() + "%"));
			}

			if (!string.IsNullOrEmpty(DealerCode))
			{
				//icrII.Add(Expression.Like("Dealercode", "%" + DealerCode + "%"));
				icrII.Add(Expression.Eq("Dealercode", DealerCode));
			}
			else
			{
				List<string> dealers;
				if (!string.IsNullOrEmpty(AreaCode))
				{
					dealers = TableToList<string>(VDMS.Data.TipTop.Dealer.GetListDealer(AreaCode).Tables[0], 0);
					icrII.Add(Expression.In("Dealercode", dealers));
				}
				else
				{
					icrII.Add(Expression.Eq("Databasecode", UserHelper.DatabaseCode));
				}
			}

			ICriteria icrCus = icrIv.CreateCriteria("Customer", "cc");
			if (!string.IsNullOrEmpty(IdentifyNumber)) icrCus.Add(Expression.InsensitiveLike("Identifynumber", "%" + IdentifyNumber + "%"));
            if (!string.IsNullOrEmpty(Fullname)) icrCus.Add(Expression.InsensitiveLike("Fullname", "%" + Fullname + "%"));
            if (!string.IsNullOrEmpty(Address)) icrCus.Add(Expression.InsensitiveLike("Address", "%" + Address + "%"));
            if (!string.IsNullOrEmpty(Precinct)) icrCus.Add(Expression.InsensitiveLike("Precinct", "%" + Precinct + "%"));
            if (!string.IsNullOrEmpty(DistrictId)) icrCus.Add(Expression.InsensitiveLike("Districtid", "%" + DistrictId + "%"));
            if (!string.IsNullOrEmpty(ProvinceId)) icrCus.Add(Expression.InsensitiveLike("Provinceid", "%" + ProvinceId + "%"));

			if ((startRowIndex >= 0) && (maximumRows >= 0))
			{
				// Get count
				count = icrIv.List<Invoice>().Count;
				HttpContext.Current.Items["rowCount"] = count;

				icrIv.SetFirstResult(startRowIndex);
				icrIv.SetMaxResults(maximumRows);
			}

			icrIv.AddOrder(Order.Desc("Createddate"));
			return icrIv.List<Invoice>();
		}
		
	}
}
