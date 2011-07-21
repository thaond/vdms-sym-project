using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;

namespace VDMS.Data.DAL2
{
	public class ReturnItemDao
	{
		public static decimal GetReturnItemCount(string DatabaseCode, string DealerCode, string enginenumber,DateTime tbfrom,DateTime tbto, string status)
		{
			StringBuilder query = new StringBuilder();
			query.Append("select count(*)\n");
			query.Append("from DATA_ITEMINSTANCE A inner join SALE_RETURNITEM B on A.iteminstanceid = B.iteminstanceid\n");
			query.AppendFormat("where DATABASECODE='{0}'", DatabaseCode);
			if (!string.IsNullOrEmpty(DealerCode)) query.AppendFormat(" and A.DEALERCODE='{0}'", DealerCode);
            if(!string.IsNullOrEmpty(enginenumber)) query.AppendFormat(" and A.ENGINENUMBER='{0}'",enginenumber);

            if (tbfrom > default(DateTime))
            {
                string _from = tbfrom.ToString("ddMMyyyy");
                query.AppendFormat(" and B.RELEASEDATE >= to_date('{0}','ddmmyyyy')", _from);
            }
            if (tbto > default(DateTime))
            {
                string _to = tbto.AddDays(1).ToString("ddMMyyyy");
                query.AppendFormat(" and B.RELEASEDATE < to_date('{0}','ddmmyyyy')",_to);
            }
			if (!string.IsNullOrEmpty(status)) query.Append(BuildStatusQueryCondition(status));
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(query.ToString());
			return (decimal)db.ExecuteScalar(dbCommand);
		}

		public static DataSet GetReturnItem(string DatabaseCode, string DealerCode,string enginenumber,DateTime tbfrom,DateTime tbto, string status, int PageIndex, int PageSize)
		{
			StringBuilder query = new StringBuilder();
			query.Append("select * from\n");
			query.Append("(select rownum as RowIndex, A.ITEMINSTANCEID, A.ITEMCODE, A.DEALERCODE, A.ENGINENUMBER, A.ITEMTYPE, A.COLOR, A.IMPORTEDDATE, A.VMEPINVOICE, A.BRANCHCODE,\n");
            query.Append("A.DATABASECODE, B.RETURNITEMID, B.RETURNREASON ,B.Status, B.returnnumber, B.vmepcomment, B.RELEASEDATE\n");
			query.Append("from DATA_ITEMINSTANCE A inner join SALE_RETURNITEM B on A.iteminstanceid = B.iteminstanceid\n");
			query.AppendFormat("where DATABASECODE='{0}'", DatabaseCode);
			if (!string.IsNullOrEmpty(DealerCode)) query.AppendFormat(" and A.DEALERCODE='{0}'", DealerCode);
            if(!string.IsNullOrEmpty(enginenumber)) query.AppendFormat(" and A.ENGINENUMBER='{0}'",enginenumber);

            if (tbfrom > default(DateTime))
            {
                string _from = tbfrom.ToString("ddMMyyyy");
                query.AppendFormat(" and B.RELEASEDATE >= to_date('{0}','ddmmyyyy')", _from);
            }
            if (tbto > default(DateTime))
            {
                string _to = tbto.AddDays(1).ToString("ddMMyyyy");
                query.AppendFormat(" and B.RELEASEDATE < to_date('{0}','ddmmyyyy')",_to);
            }
			if (!string.IsNullOrEmpty(status)) query.Append(BuildStatusQueryCondition(status));
			query.Append(") where RowIndex between :low and :high");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(query.ToString());
			db.AddInParameter(dbCommand, ":low", DbType.Int64, PageIndex/PageSize * PageSize  + 1);
			db.AddInParameter(dbCommand, ":high", DbType.Int64, PageSize * (PageIndex + 1));
			return db.ExecuteDataSet(dbCommand);
		}

		static string BuildStatusQueryCondition(string Status)
		{
			StringBuilder builder = new StringBuilder();
			builder.Append(" and (");
			foreach (string s in Status.Split(';'))
			{
                builder.AppendFormat("B.STATUS = {0} OR ", s);
			}
			builder.Remove(builder.Length - 4, 4);
			builder.Append(")");
			return builder.ToString();
		}
	}
}
