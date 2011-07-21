using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace VDMS.Data.DAL2
{
	public class OrderDao
	{
		/// <summary>
		/// Get orders sent to sales
		/// </summary>
		/// <param name="FromDate"></param>
		/// <param name="ToDate"></param>
		/// <param name="DealerCode"></param>
		/// <param name="AreaCode"></param>
		/// <param name="DatabaseCode"></param>
		/// <param name="Status"></param>
		/// <param name="PageIndex">Base index is 0</param>
		/// <param name="PageSize"></param>
		/// <returns></returns>
		public static DataSet GetOrderByStatus(DateTime FromDate, DateTime ToDate, string DealerCode, string AreaCode, string OrderNumber, string DatabaseCode, string Status, int PageIndex, int PageSize)
		{
			StringBuilder query = new StringBuilder();
			query.Append("select * from VIEW_SYM_ORDERDETAIL A inner join\n");
			query.Append("(select orderid, RowIndex from\n");
			query.Append("(select orderid, rownum as RowIndex from\n");
			query.AppendFormat("(select distinct orderid, ORDERDATE from VIEW_SYM_ORDERDETAIL where ORDERDATE between :p_FROMDATE and :p_TODATE and DatabaseCode='{0}'", DatabaseCode);
			if (!string.IsNullOrEmpty(Status)) query.Append(BuildStatusQueryCondition(Status)); //query.AppendFormat(" and STATUS =  {0}", Status);
			if (!string.IsNullOrEmpty(DealerCode)) query.Append(" AND DEALERCODE like :p_DEALERCODE");
			if (!string.IsNullOrEmpty(AreaCode)) query.Append(" AND AREACODE = :p_AREACODE");
			if (!string.IsNullOrEmpty(OrderNumber)) query.Append(" AND ORDERNUMBER like :p_ORDERNUMBER");
			query.Append(" order by ORDERDATE desc) X ) Y\n");
			query.Append("where RowIndex between :low and :high) B ");
			query.AppendFormat("on A.OrderId = B.OrderId and A.DatabaseCode='{0}' order by A.ORDERDATE, A.OrderId desc", DatabaseCode);

			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(query.ToString());
			db.AddInParameter(dbCommand, ":p_FROMDATE", DbType.DateTime, FromDate);
			db.AddInParameter(dbCommand, ":p_TODATE", DbType.DateTime, ToDate.Date.AddDays(1));
			if (!string.IsNullOrEmpty(DealerCode)) db.AddInParameter(dbCommand, ":p_DEALERCODE", DbType.AnsiString, "%" + DealerCode + "%");
			if (!string.IsNullOrEmpty(AreaCode)) db.AddInParameter(dbCommand, ":p_AREACODE", DbType.AnsiString, AreaCode);
			if (!string.IsNullOrEmpty(OrderNumber)) db.AddInParameter(dbCommand, ":p_ORDERNUMBER", DbType.AnsiString, "%" + OrderNumber + "%");
			db.AddInParameter(dbCommand, ":low", DbType.Int64, PageIndex * PageSize + 1);
			db.AddInParameter(dbCommand, ":high", DbType.Int64, PageSize * (PageIndex + 1));
			return db.ExecuteDataSet(dbCommand);
		}

		static string BuildStatusQueryCondition(string Status)
		{
			StringBuilder builder = new StringBuilder();
			builder.Append(" and (");
			foreach (string s in Status.Split(';'))
			{
				builder.AppendFormat("STATUS = {0} OR ", s);
			}
			builder.Remove(builder.Length - 4, 4);
			builder.Append(")");
			return builder.ToString();
		}

		public static decimal GetOrderCountByStatus(DateTime FromDate, DateTime ToDate, string DealerCode, string AreaCode, string OrderNumber, string DatabaseCode, string Status)
		{
			StringBuilder query = new StringBuilder();
			query.AppendFormat("select count(orderid) from SALE_ORDERHEADER where ORDERDATE between :p_FROMDATE and :p_TODATE and DatabaseCode='{0}'", DatabaseCode);
			if (!string.IsNullOrEmpty(Status)) query.Append(BuildStatusQueryCondition(Status)); //query.AppendFormat(" and STATUS = {0}", Status);
			if (!string.IsNullOrEmpty(DealerCode)) query.Append(" AND DEALERCODE like :p_DEALERCODE");
			if (!string.IsNullOrEmpty(AreaCode)) query.Append(" AND AREACODE = :p_AREACODE");
			if (!string.IsNullOrEmpty(OrderNumber)) query.Append(" AND ORDERNUMBER like :p_ORDERNUMBER");

			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(query.ToString());
			db.AddInParameter(dbCommand, ":p_FROMDATE", DbType.DateTime, FromDate);
			db.AddInParameter(dbCommand, ":p_TODATE", DbType.DateTime, ToDate.Date.AddDays(1));
			if (!string.IsNullOrEmpty(DealerCode)) db.AddInParameter(dbCommand, ":p_DEALERCODE", DbType.AnsiString, "%" + DealerCode + "%");
			if (!string.IsNullOrEmpty(AreaCode)) db.AddInParameter(dbCommand, ":p_AREACODE", DbType.AnsiString, AreaCode);
			if (!string.IsNullOrEmpty(OrderNumber)) db.AddInParameter(dbCommand, ":p_ORDERNUMBER", DbType.AnsiString, "%" + OrderNumber + "%");

			return (decimal)db.ExecuteScalar(dbCommand);
		}

		public static DataSet GetOrderDetail(long OrderId)
		{
			string sql = "select * from VIEW_SYM_ORDERDETAIL where ORDERID = :p_ORDERID";
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(sql);
			db.AddInParameter(dbCommand, ":p_ORDERID", DbType.Int64, OrderId);
			return db.ExecuteDataSet(dbCommand);
		}
	}
}
