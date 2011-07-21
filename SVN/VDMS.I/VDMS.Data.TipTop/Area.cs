using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace VDMS.Data.TipTop
{
	public class Area : DataObjectBase
	{
        //public static string GetArea(string DealerCode)
        //{
        //    if (string.IsNullOrEmpty(DealerCode) || (DealerCode == "/")) return string.Empty;
        //    Database db = DatabaseFactory.CreateDatabase();
        //    string sqlCommand = string.Format("select ta_occ010 from {0} where occ01 = '{1}'", BuildViewName("occ_file"), DealerCode);
        //    DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
        //    return db.ExecuteScalar(dbCommand).ToString();
        //}

		public static DataSet GetListArea()
		{
			Database db = DatabaseFactory.CreateDatabase();
			string sqlCommand = string.Format("select tc_inw010 as AreaCode, tc_inw020 as AreaName, tc_inw030 as CountryCode from {0} where tc_inwacti = 'Y'", BuildViewName("tc_inw_file"));
			DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
			return db.ExecuteDataSet(dbCommand);
		}

		public static DataSet GetListArea(string DatabaseCode)
		{
			Database db = DatabaseFactory.CreateDatabase();
			string sqlCommand = string.Format("select tc_inw010 as AreaCode, tc_inw020 as AreaName, tc_inw030 as CountryCode from view_{0}_tc_inw_file where tc_inwacti = 'Y'", DatabaseCode);
			DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
			return db.ExecuteDataSet(dbCommand);
		}

		public static DataSet GetListProvince()
		{
			Database db = DatabaseFactory.CreateDatabase();
			string sqlCommand = @"select CONCAT(tc_inv010,' (HTF)' ) as ProviceCode, CONCAT(tc_inv020,' (HTF)' ) as ProviceName from view_htf_tc_inv_file where tc_invacti = 'Y'
            union
            select CONCAT(tc_inv010,' (DNF)') as ProviceCode, CONCAT(tc_inv020,' (DNF)') as ProviceName from view_dnf_tc_inv_file where tc_invacti = 'Y'
            ";
			DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
			return db.ExecuteDataSet(dbCommand);
		}

		public static string GetProvinceNameByProvinceCode(string ProvinceCode)
		{
			if (string.IsNullOrEmpty(ProvinceCode)) return string.Empty;

			string res = string.Empty;
			Database db = DatabaseFactory.CreateDatabase();
			string sqlCommand = string.Format(@"select ProviceName from
            (select CONCAT(tc_inv010,' (HTF)' )  as ProviceCode, tc_inv020 as ProviceName from view_htf_tc_inv_file where tc_invacti = 'Y'
            union
            select CONCAT(tc_inv010,' (DNF)') as ProviceCode, tc_inv020 as ProviceName from view_dnf_tc_inv_file where tc_invacti = 'Y') A
            where A.ProviceCode = '{0}'
            ", ProvinceCode);
			DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
			try
			{
				return (string)db.ExecuteScalar(dbCommand);
			}
			catch
			{
				return string.Empty;
			}
		}
	}
}
