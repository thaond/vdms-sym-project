using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace VDMS.Data.DAL2
{
	public enum LockInventoryType : int
	{
		Locked = 0,
		Unlock = 1,
		NotFound = 2,
	}

    [DataObject]
	public class InventoryDao
	{

		public static bool DoInventory(int Month, int Year, string DealerCode)
		{
			string sql = "inventory.report_doinventory";
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetStoredProcCommand(sql);
			db.AddInParameter(dbCommand, "p_month", DbType.Int32, Month);
			db.AddInParameter(dbCommand, "p_year", DbType.Int32, Year);
			db.AddInParameter(dbCommand, "p_dealercode", DbType.AnsiString, DealerCode);
			db.AddOutParameter(dbCommand, "p_result", DbType.Int32, 4);
			db.ExecuteNonQuery(dbCommand);

			int result = (int)db.GetParameterValue(dbCommand, "p_result");
			return result == 0;
		}

		public static bool UnlockInventory(int Month, int Year, string DealerCode)
		{
			string sql = "inventory.INVENTORY_UNLOCK";
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetStoredProcCommand(sql);
			db.AddInParameter(dbCommand, "p_month", DbType.Int32, Month);
			db.AddInParameter(dbCommand, "p_year", DbType.Int32, Year);
			db.AddInParameter(dbCommand, "p_dealercode", DbType.AnsiString, DealerCode);
			db.AddOutParameter(dbCommand, "p_result", DbType.Int32, 4);
			db.ExecuteNonQuery(dbCommand);

			int result = (int)db.GetParameterValue(dbCommand, "p_result");

			return result == 0;
		}

		public static bool OpenLockedInventory(string DealerCode)
		{
			string sql = "inventory.UNLOCK_INVENTORY";
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetStoredProcCommand(sql);
			db.AddInParameter(dbCommand, "p_dealercode", DbType.AnsiString, DealerCode);
			db.AddOutParameter(dbCommand, "p_result", DbType.Int32, 4);
			db.ExecuteNonQuery(dbCommand);

			return (int)db.GetParameterValue(dbCommand, "p_result") == 0;
		}

        //public static int IsInventoryLock(int Month, int Year, string DealerCode)
        //{
        //    string sql = "inventory.IS_INVENTORY_LOCK";
        //    Database db = DatabaseFactory.CreateDatabase();
        //    DbCommand dbCommand = db.GetStoredProcCommand(sql);
        //    db.AddInParameter(dbCommand, "p_month", DbType.Int32, Month);
        //    db.AddInParameter(dbCommand, "p_year", DbType.Int32, Year);
        //    db.AddInParameter(dbCommand, "p_dealercode", DbType.AnsiString, DealerCode);
        //    db.AddOutParameter(dbCommand, "p_result", DbType.Int32, 4);
        //    db.ExecuteNonQuery(dbCommand);

        //    return (int)db.GetParameterValue(dbCommand, "p_result");
        //}

		public static DataSet SalesReport(DateTime ReportDate, string DealerCode, string DatabaseCode)
		{
			DealerCode = '%' + DealerCode.ToUpper() + '%';
			string sql = "inventory.REPORT_SALES";
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetStoredProcCommand(sql, ReportDate, DealerCode, DatabaseCode, null);

			return db.ExecuteDataSet(dbCommand);
		}

		public static DataSet SalesReport(DateTime ReportDate, string ItemCode, string DealerCode, string DatabaseCode)
		{
			DealerCode = '%' + DealerCode.ToUpper() + '%';
			string sql = "inventory.REPORT_SALES_BY_ITEM";
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetStoredProcCommand(sql, ReportDate, ItemCode, DealerCode, DatabaseCode, null);

			return db.ExecuteDataSet(dbCommand);
		}

		public static DataSet CheckOrderDetail(long orderheaderid)
		{
			string sql = "REPORT.Check_Order";
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetStoredProcCommand(sql, orderheaderid, null);

			DataSet ds = db.ExecuteDataSet(dbCommand);
			return ds;
		}

		public static DataSet RestOfMoney(string IdentifyNumber, DateTime d1, string userDealercode)
		{
			string sql = "REPORT.search_rest_money_cus"; string sql_allCustomer = "REPORT.search_rest_money";
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = (IdentifyNumber.Trim().Length == 0) ? db.GetStoredProcCommand(sql_allCustomer, d1, userDealercode, null) : db.GetStoredProcCommand(sql, d1, IdentifyNumber, userDealercode, null);

			DataSet ds = db.ExecuteDataSet(dbCommand);
			return ds;
		}

		public static DataSet ReportDaily(DateTime d1, DateTime d2, string DealerCode, string BranchCode, string BranchName)
		{
			string sql = "inventory.REPORT_DAILY";
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetStoredProcCommand(sql, d1, d2, DealerCode, BranchCode, BranchName, null);

			return db.ExecuteDataSet(dbCommand);
		}

        [DataObjectMethod(DataObjectMethodType.Select)]
		public static DataTable ReportSellingDaily(DateTime FromDate, DateTime ToDate, string DealerCode, string BranchCode, string DatabaseCode)
		{
			string sql = "REPORT.REPORT_SELLING_DAILY";
            if (string.IsNullOrEmpty(BranchCode)) BranchCode = "%";
            if (string.IsNullOrEmpty(DealerCode) || DealerCode == "/") DealerCode = "%";
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetStoredProcCommand(sql, FromDate, ToDate, DealerCode, BranchCode, DatabaseCode, null);

			DataSet ds = db.ExecuteDataSet(dbCommand);
			return ds.Tables[0];
		}

        [DataObjectMethod(DataObjectMethodType.Select)]
		public static DataTable ReportSellingDailyDebtOnly(DateTime FromDate, DateTime ToDate, string DealerCode, string BranchCode, string DatabaseCode)
		{
			string sql = "REPORT.report_selling_daily_debtonly";
            if (string.IsNullOrEmpty(BranchCode)) BranchCode = "%";

            Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetStoredProcCommand(sql, FromDate, ToDate, DealerCode, BranchCode, DatabaseCode, null);

			DataSet ds = db.ExecuteDataSet(dbCommand);
			return ds.Tables[0];
		}

		#region Sale Report

		public static DataRow SaleDailyDetail(string DealerCode, string ItemCode, DateTime ReportDate)
		{
			return new DataSet().Tables[0].Rows[0];
		}
		#endregion
	}
}
