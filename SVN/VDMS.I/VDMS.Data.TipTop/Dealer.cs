using System.Collections.Generic;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace VDMS.Data.TipTop
{
	public class Dealer : DataObjectBase
	{
		/// <summary>
		/// Return the list of DealerCode
		/// </summary>
		/// <param name="DealerCode"></param>
		/// <returns></returns>
		public static DataSet GetListBranchOfDealer(string DealerCode)
		{
			var db = DatabaseFactory.CreateDatabase();
			var sqlCommand = string.Format("select occ01 as BranchCode, occ18 as BranchName, occ231 as Address, occ241 as BillAddress from {0} where occ21 = 'VN' and occacti = 'Y' and occ34 = :DealerCode", BuildViewName("occ_file"));
			var dbCommand = db.GetSqlStringCommand(sqlCommand);
			db.AddInParameter(dbCommand, ":DealerCode", DbType.AnsiString, DealerCode);
			var ds = db.ExecuteDataSet(dbCommand);

			//if (ds.Tables[0].Rows.Count == 0)
			//{
			//    var db1 = DatabaseFactory.CreateDatabase();
			//    sqlCommand = string.Format("select occ01 as BranchCode, occ18 as BranchName, occ231 as Address, occ241 as BillAddress from {0} where occ21 = 'VN' and occacti = 'Y' and occ34 = :DealerCode", BuildViewName("occ_file"));
			//    var dbCommand1 = db1.GetSqlStringCommand(sqlCommand);
			//    db1.AddInParameter(dbCommand1, ":DealerCode", DbType.AnsiString, DealerCode);
			//    ds = db1.ExecuteDataSet(dbCommand1);
			//}

			return ds;
		}

		public static DataSet GetListSecondaryAddressOfDealer(string DealerCode)
		{
			var db = DatabaseFactory.CreateDatabase();
			var sqlCommand = string.Format("select ocd02 as Code, concat(concat(concat(ocd02, ' ( '), concat(concat(concat(concat(concat(ocd221,' / '),ocd222),' / '),' / '),ocd223)), ' )') as Name from {0} where ocd01 = :DealerCode", BuildViewName("ocd_file"));
			var dbCommand = db.GetSqlStringCommand(sqlCommand);
			db.AddInParameter(dbCommand, ":DealerCode", DbType.AnsiString, DealerCode);
			return db.ExecuteDataSet(dbCommand);
		}

		/// <summary>
		/// Get the address by the branch code
		/// </summary>
		/// <param name="BranchCode"></param>
		/// <returns></returns>
		public static string GetAddressByBranchCode(string BranchCode)
		{
			var db = DatabaseFactory.CreateDatabase();
			var sqlCommand = string.Format("select occ241 as BillAddress from {0} where occ21 = 'VN' and occacti = 'Y' and occ01 = :BranchCode", BuildViewName("occ_file"));
			var dbCommand = db.GetSqlStringCommand(sqlCommand);
			db.AddInParameter(dbCommand, ":BranchCode", DbType.AnsiString, BranchCode);
			return (string)db.ExecuteScalar(dbCommand);
		}

		public static DataSet GetListDealer()
		{
			//var db = DatabaseFactory.CreateDatabase();
			//var sqlCommand = "select occ01 as BranchCode, occ18 as BranchName, occ231 as Address, occ34 from VIEW_DNF_OCC_FILE where occ21 = 'VN' and occacti = 'Y' and occ34 <> ' ' and not occ34 is null order by occ01";
			//var dbCommand = db.GetSqlStringCommand(sqlCommand);
			//return db.ExecuteDataSet(dbCommand);
			return GetListDealerALL();
		}

		public static DataSet GetListDealerALL()
		{
			var db = DatabaseFactory.CreateDatabase();
			var sqlCommand = "SELECT DISTINCT * From (" +
								"select occ01 as BranchCode, occ18 as BranchName, occ231 as Address, occ34  from VIEW_HTF_OCC_FILE where occ21 = 'VN' and (lpad(occ01,1) = 'N' and length(occ01) > 6) or (occ01 like 'MISC%') " +
								"UNION ALL " +
								"select occ01 as BranchCode, occ18 as BranchName, occ231 as Address, occ34  from VIEW_DNF_OCC_FILE where occ21 = 'VN' and (length(occ01) <= 6) or (occ01 like 'MISC%')) " +
								"ORDER BY BranchCode";
			var dbCommand = db.GetSqlStringCommand(sqlCommand);
			return db.ExecuteDataSet(dbCommand);
		}

		public static DataSet GetDealer(string DealerCode)
		{
			var db = DatabaseFactory.CreateDatabase();
			var sqlCommand = "SELECT DISTINCT * From (" +
								"select occ01 as BranchCode, occ18 as BranchName, occ231 as Address, occ34, occ03 as DealerType, 'HTF' as DBCode, ta_occ010 as AreaCode from VIEW_HTF_OCC_FILE where occ21 = 'VN' and (lpad(occ01,1) = 'N' and length(occ01) > 6) or (occ01 like 'MISC%') " +
								"UNION ALL " +
								"select occ01 as BranchCode, occ18 as BranchName, occ231 as Address, occ34, occ03 as DealerType, 'DNF' as DBCode, ta_occ010 as AreaCode from VIEW_DNF_OCC_FILE where occ21 = 'VN' and (length(occ01) <= 6) or (occ01 like 'MISC%')) " +
								"WHERE BranchCode = :BranchCode";
			var dbCommand = db.GetSqlStringCommand(sqlCommand);
			db.AddInParameter(dbCommand, ":BranchCode", DbType.AnsiString, DealerCode);
			return db.ExecuteDataSet(dbCommand);
		}

		/// <summary>
		/// Added by dungnt 26/12/2007 (14h00)
		/// </summary>
		/// <param name="sAreaCode"></param>
		/// <returns></returns>
		public static DataSet GetListDealer(string sAreaCode)
		{
			var db = DatabaseFactory.CreateDatabase();
			var sqlCommand = "select occ01 as BranchCode, occ18 as BranchName, occ231 as Address, occ34 from VIEW_DNF_OCC_FILE where occ21 = 'VN' and ta_occ010 = :AreaCode and occacti = 'Y' and occ34 <> ' ' and not occ34 is null order by occ01";
			var dbCommand = db.GetSqlStringCommand(sqlCommand);
			db.AddInParameter(dbCommand, ":AreaCode", DbType.AnsiString, sAreaCode);
			return db.ExecuteDataSet(dbCommand);
		}

		public static DataSet GetListDealer(List<string> sAreaCode)
		{
			var db = DatabaseFactory.CreateDatabase();
			var sqlCommand = "select occ01 as BranchCode, occ18 as BranchName, occ231 as Address, occ34 from VIEW_DNF_OCC_FILE where occ21 = 'VN' and ta_occ010 IN (";
			foreach (string area in sAreaCode)
			{
				sqlCommand += "'" + area + "',";
			}
			sqlCommand += "'qwerty123') and occacti = 'Y' and occ34 <> ' ' and not occ34 is null order by occ01";
			var dbCommand = db.GetSqlStringCommand(sqlCommand);
			return db.ExecuteDataSet(dbCommand);
		}

		public static DataSet GetListDealerByDatabase(string databaseCode)
		{
			var db = DatabaseFactory.CreateDatabase();
			var sqlCommand =
				(databaseCode == "HTF") ? "select occ01 as BranchCode, occ18 as BranchName, occ231 as Address, occ34  from VIEW_HTF_OCC_FILE where occ21 = 'VN' and (lpad(occ01,1) = 'N' and length(occ01) > 6) or (occ01 like 'MISC%') order by occ01" :
										  "select occ01 as BranchCode, occ18 as BranchName, occ231 as Address, occ34  from VIEW_DNF_OCC_FILE where occ21 = 'VN' and (length(occ01) <= 6) or (occ01 like 'MISC%') order by occ01";
			var dbCommand = db.GetSqlStringCommand(sqlCommand);
			return db.ExecuteDataSet(dbCommand);
		}

		public static bool IsDealerCodeExist(string DealerCode)
		{
			var db = DatabaseFactory.CreateDatabase();
			var sqlCommand = string.Format("select count(*) from {0} where occ21 = 'VN' and occacti = 'Y' and occ34 = :DealerCode", BuildViewName("occ_file"));
			var dbCommand = db.GetSqlStringCommand(sqlCommand);
			db.AddInParameter(dbCommand, ":DealerCode", DbType.AnsiString, DealerCode);
			return (decimal)db.ExecuteScalar(dbCommand) > 0;
		}

		public static bool IsBranchCodeExist(string DealerCode)
		{
			var db = DatabaseFactory.CreateDatabase();
			var sqlCommand = string.Format("select count(*) from {0} where occ21 = 'VN' and occacti = 'Y' and (occ34 = ' ' or occ34 is NULL) and occ01 = :DealerCode", BuildViewName("occ_file"));
			var dbCommand = db.GetSqlStringCommand(sqlCommand);
			db.AddInParameter(dbCommand, ":DealerCode", DbType.AnsiString, DealerCode);
			return (decimal)db.ExecuteScalar(dbCommand) > 0;
		}

        public static DataSet GetListAddress(string DealerCode)
        {
            return GetListAddress(DealerCode, "P");
        }
		public static DataSet GetListAddress(string DealerCode, string type)
		{
            int version = type == "P" ? 2 : 1;

			var db = DatabaseFactory.CreateDatabase();
			var sqlCommand = string.Format("select ocd02 as Code, ocd221 as Address from {0} where ocd01 = :DealerCode", BuildViewName("ocd_file", version));
			sqlCommand += " union all ";
            //sqlCommand += string.Format("select occ01 as Code, occ241 as Address from {0} where occ34 = :DealerCode", BuildViewName("occ_file", version));
            sqlCommand += string.Format("select occ01 as Code, occ241 as Address from {0} where occ01 = :DealerCode", BuildViewName("occ_file", version));

			var dbCommand = db.GetSqlStringCommand(sqlCommand);
			db.AddInParameter(dbCommand, ":DealerCode", DbType.AnsiString, DealerCode);
			return db.ExecuteDataSet(dbCommand);
		}
	}
}
