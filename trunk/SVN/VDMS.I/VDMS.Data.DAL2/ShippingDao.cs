using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace VDMS.Data.DAL2
{
	public class ShippingDao
	{
		public DataSet GetShippingDetail(DateTime fromDate, DateTime toDate, int status)
		{
			string sql = "select * from SALE_SHIPPINGHEADER sh join SALE_SHIPPINGDETAIL sd on sd.SHIPPINGID = sh.shippingid join DATA_ITEM it on sd.ITEMCODE = it.ITEMCODE where sh.CREATEDDATE between :p_FROMDATE and :p_TODATE ";
			string strWhere = " and (\"EXCEPTION\" <> null and TRIM(\"EXCEPTION\") <> '') ";

			switch (status)
			{
				case 1:
					break;
				case 2:
					strWhere = strWhere + " and (VMEPRESPONSE = null or TRIM(VMEPRESPONSE) = '') ";
					break;
				case 3:
					strWhere = strWhere + " and (VMEPRESPONSE <> null and TRIM(VMEPRESPONSE) <> '') ";
					break;
				default:
					break;
			}

			strWhere = string.Empty;
			sql = sql + strWhere;

			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(sql);
			db.AddInParameter(dbCommand, ":p_FROMDATE", DbType.DateTime, fromDate.Date);
			db.AddInParameter(dbCommand, ":p_TODATE", DbType.DateTime, toDate.Date.AddDays(1));
			return db.ExecuteDataSet(dbCommand);
		}
	}
}
