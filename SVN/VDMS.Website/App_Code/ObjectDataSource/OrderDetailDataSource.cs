using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL2;
using VDMS.Data.IDAL.Interface;
using VDMS.Helper;

namespace VDMS.I.ObjectDataSource
{
	/// <summary>
	/// This data source allow query the order detail, include header.
	/// It also paging according to OrderHeaderId
	/// </summary>
	public class OrderDetailDataSource
	{
		public static int SelectCount(int maximumRows, int startRowIndex, string sFromDate, string sToDate, string DealerCode, string AreaCode, string StatusCode)
		{
			DateTime fromDate, toDate;
			DateTime.TryParse(sFromDate, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out fromDate);
			DateTime.TryParse(sToDate, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out toDate);
			decimal count = OrderDao.GetOrderCountByStatus(fromDate, toDate, DealerCode, AreaCode, string.Empty, UserHelper.DatabaseCode, StatusCode);
			return (int)count;
		}

		public static DataSet Select(int maximumRows, int startRowIndex, string sFromDate, string sToDate, string DealerCode, string AreaCode, string StatusCode)
		{
			DateTime fromDate, toDate;
			DateTime.TryParse(sFromDate, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out fromDate);
			DateTime.TryParse(sToDate, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out toDate);

			return OrderDao.GetOrderByStatus(fromDate, toDate, DealerCode, AreaCode, string.Empty, UserHelper.DatabaseCode, StatusCode, startRowIndex, maximumRows);
		}

		public static List<Orderdetail> GetItemsByHeaderId(string id)
		{
			long orderId;
			long.TryParse(id, out orderId);

			IDao<Orderdetail, long> dao = DaoFactory.GetDao<Orderdetail, long>();
			dao.SetCriteria(new ICriterion[] { Expression.Eq("Orderheader.Id", orderId) });
			return dao.GetAll();
		}
	}
}