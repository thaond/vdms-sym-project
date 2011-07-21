using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Web;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;
using VDMS.Helper;
using VDMS.I.Service;

namespace VDMS.I.ObjectDataSource
{
	public class ExchangePartHeaderDataSource
	{
		public ExchangePartHeaderDataSource()
		{
		}
		private int count = 0;

		#region ntDung
		// return maximum record
		public int SelectCount(int maximumRows, int startRowIndex, string fromDate, string toDate, string status)
		{
			return count;
		}

		[DataObjectMethodAttribute(DataObjectMethodType.Select)]
		public List<Exchangepartheader> Select(int maximumRows, int startRowIndex, string fromDate, string toDate, string status, string dealerCode)
		{
			List<Exchangepartheader> list = new List<Exchangepartheader>();
			IDao<Exchangepartheader, long> dao = DaoFactory.GetDao<Exchangepartheader, long>();

			DateTime dtfromDate, dttoDate;
			DateTime.TryParse(fromDate, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dtfromDate);
			DateTime.TryParse(toDate, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dttoDate);
			if (!string.IsNullOrEmpty(status))
			{
				dao.SetCriteria(new ICriterion[] { Expression.Between("Exchangeddate", dtfromDate.Date, dttoDate.Date), Expression.Eq("Status", int.Parse(status)), Expression.Eq("Dealercode", dealerCode) });
			}
			else
			{
				dao.SetCriteria(new ICriterion[] { Expression.Between("Exchangeddate", dtfromDate.Date, dttoDate.Date), Expression.Eq("Dealercode", dealerCode), Expression.Ge("Status", 0) });
			}
			if (maximumRows != 0)
			{
				list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
				count = dao.ItemCount;
				if (startRowIndex >= count)
				{
					list = dao.GetPaged(0, maximumRows);
					count = dao.ItemCount;
				}
			}
			else
			{
				list = dao.GetAll();
			}
			HttpContext.Current.Items["rowCount"] = count;
			return list;
		}
		//public static List<Exchangepartheader> Select(DateTime fromDate, DateTime toDate, int status)
		//{
		//    List<Exchangepartheader> list = new List<Exchangepartheader>();
		//    IDao<Exchangepartheader, long> dao = DaoFactory.GetDao<Exchangepartheader, long>();

		//    dao.SetCriteria(new ICriterion[] { Expression.Between("Exchangeddate", fromDate.Date, toDate.Date), Expression.Eq("Status", status) });

		//    list = dao.GetAll();
		//    //HttpContext.Current.Items["rowCount"] = list.Count;
		//    return list;
		//}
		public List<Exchangepartheader> Select(DateTime fromDate, DateTime toDate, int status, string dealerCode)
		{
			return this.Select(int.MaxValue, 0, fromDate.ToShortDateString(), toDate.ToShortDateString(), status.ToString(), dealerCode);
		}
		//public static List<Exchangepartheader> Select(DateTime fromDate, DateTime toDate)
		//{
		//    List<Exchangepartheader> list = new List<Exchangepartheader>();
		//    IDao<Exchangepartheader, long> dao = DaoFactory.GetDao<Exchangepartheader, long>();

		//    dao.SetCriteria(new ICriterion[] { Expression.Between("Exchangeddate", fromDate.Date, toDate.Date) });

		//    list = dao.GetAll();
		//    return list;
		//}
		public static List<Exchangepartheader> Select(DateTime fromDate, DateTime toDate, int status, bool byArea)
		{
			List<Exchangepartheader> list = new List<Exchangepartheader>();
			IDao<Exchangepartheader, long> dao = DaoFactory.GetDao<Exchangepartheader, long>();

			// nmchi fix: area
			ArrayList crit = new ArrayList();
			List<string> areas;
			if (byArea)
			{
				areas = AreaHelper.Area;
				if ((areas != null) && (areas.Count > 0)) crit.Add(Expression.In("Areacode", areas));
				if (UserHelper.IsDealer) crit.Add(Expression.Eq("Dealercode", UserHelper.DealerCode));
			}
			crit.Add(Expression.Between("Exchangeddate", fromDate.Date, toDate.Date));
			if ((int)ExchangeVoucherStatus.All != status)
			{
				crit.Add(Expression.Eq("Status", status));
			}
			else
			{
				crit.Add(Expression.Ge("Status", 0));   // ko lay cac temp voucher
			}

			//dao.SetCriteria(new ICriterion[] { Expression.Between("Exchangeddate", fromDate.Date, toDate.Date) });
			dao.SetCriteria((ICriterion[])crit.ToArray(typeof(ICriterion)));
			// end: area

			list = dao.GetAll();
			return list;
		}
		#endregion

		#region nmChi
		public int SelectCount(int maximumRows, int startRowIndex, string fromDate, string toDate, int status, string dealerCode)
		{
			return count;
		}
		public IList<Exchangepartheader> Select(int maximumRows, int startRowIndex, string fromDate, string toDate, int status, string dealerCode)
		{
			DateTime dtfromDate, dttoDate;
			ArrayList crit = new ArrayList();
			IList<Exchangepartheader> list;
			IDao<Exchangepartheader, long> dao = DaoFactory.GetDao<Exchangepartheader, long>();

			crit.Add(Expression.Not(Expression.Eq("Status", (int)ExchangeVoucherStatus.New)));
			if (DateTime.TryParse(fromDate, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dtfromDate))
				crit.Add(Expression.Ge("Exchangeddate", dtfromDate));
			if (DateTime.TryParse(toDate, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dttoDate))
				crit.Add(Expression.Le("Exchangeddate", dttoDate));
			if (status > 0) crit.Add(Expression.Eq("Status", status));
			else crit.Add(Expression.Ge("Status", 0));

			if (!string.IsNullOrEmpty(dealerCode)) crit.Add(Expression.Eq("Dealercode", dealerCode.Trim().ToUpper()));

			try
			{
				dao.SetCriteria((ICriterion[])crit.ToArray(typeof(ICriterion)));
				list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
				count = dao.ItemCount;
				HttpContext.Current.Items["ConfirmationExchangeRowCount"] = count;
			}
			catch { return null; }
			return list;
		}

		#endregion
	}

}
