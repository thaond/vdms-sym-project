using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace VDMS.Data.DAL2
{
	public class ItemDao
	{
		public static DataTable GetListItemType(string DatabaseCode)
		{
			string sql = string.Format("select distinct SUBSTR(ITEMCODE, 1, INSTR(ITEMCODE, '-') - 1) as ItemType from DATA_ITEM where DatabaseCode like '%{0}%' Order by ItemType", DatabaseCode);
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(sql);
			return db.ExecuteDataSet(dbCommand).Tables[0];
		}

		public static DataTable GetListColorOfItem(string ItemType, string DatabaseCode)
		{
			string sql = string.Format("select SUBSTR(ITEMCODE, INSTR(ITEMCODE, '-') + 1, LENGTH(ITEMCODE) - INSTR(ITEMCODE, '-')) as Color from DATA_ITEM where AreaCode like '%{1}%' AND SUBSTR(ITEMCODE, 1, INSTR(ITEMCODE, '-') - 1)='{0}' Order by Color", ItemType, DatabaseCode);
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(sql);
			return db.ExecuteDataSet(dbCommand).Tables[0];
		}
	}
}
