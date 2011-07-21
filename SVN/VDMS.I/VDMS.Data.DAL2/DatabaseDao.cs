using System;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace VDMS.Data.DAL2
{
	public class DatabaseDao
	{
		public static void ResetTableAndSequence(string TableName)
		{
			string SequenceName = string.Concat("seq_", TableName.Split('_')[1]);
			if (SequenceName.EndsWith("s") && !SequenceName.EndsWith("is") && SequenceName.IndexOf("In", StringComparison.Ordinal) == -1) SequenceName = SequenceName.Remove(SequenceName.Length - 1);

			string sql = string.Concat("delete from ", TableName);
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(sql);
			db.ExecuteNonQuery(dbCommand);

			sql = string.Concat("drop SEQUENCE ", SequenceName);
			dbCommand = db.GetSqlStringCommand(sql);
			db.ExecuteNonQuery(dbCommand);

			sql = string.Concat("CREATE SEQUENCE ", SequenceName, " MINVALUE 1 MAXVALUE 999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20");
			dbCommand = db.GetSqlStringCommand(sql);
			db.ExecuteNonQuery(dbCommand);
		}

		public static void ResetSequence(string TableName)
		{
			string SequenceName = string.Concat("seq_", TableName.Split('_')[1]);
			if (SequenceName.EndsWith("s") && !SequenceName.EndsWith("is") && SequenceName.IndexOf("In", StringComparison.Ordinal) == -1) SequenceName = SequenceName.Remove(SequenceName.Length - 1);

			string sql;
			switch (TableName)
			{
				case "app_RolesInTasks":
					sql = string.Format("select max(Id) from {0}", TableName);
					break;
				case "sale_OrderHeader":
					sql = string.Format("select max(OrderId) from {0}", TableName);
					break;
				case "sale_ShippingHeader":
					sql = string.Format("select max(ShippingId) from {0}", TableName);
					break;
				case "sale_TransHis":
					sql = string.Format("select max(TRANSACTIONID) from {0}", TableName);
					break;
				case "sym_Articles":
					sql = string.Format("select max(ARTICLEID) from {0}", TableName);
					break;
				case "app_Tasks":
					sql = string.Format("select max(TASKID) from {0}", TableName);
					break;
				default:
					sql = string.Format("select max({1}Id) from {0}", TableName, TableName.Split('_')[1]);
					break;
			}
			//string sql = string.Format("select max({1}Id) from {0}", TableName, TableName.Split('_')[1]);
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(sql);
			decimal count = 0;
			try
			{
				count = (decimal)db.ExecuteScalar(dbCommand);
			}
			catch { }

			try
			{
				sql = string.Concat("drop SEQUENCE ", SequenceName);
				dbCommand = db.GetSqlStringCommand(sql);
				db.ExecuteNonQuery(dbCommand);
			}
			catch
			{
			}

			sql = string.Concat("CREATE SEQUENCE ", SequenceName, " MINVALUE 1 MAXVALUE 999999999999999999999999999 START WITH ", count + 1, " INCREMENT BY 1 CACHE 20");
			dbCommand = db.GetSqlStringCommand(sql);
			db.ExecuteNonQuery(dbCommand);
		}

		public static void ResetSequence2(string TableName)
		{
			string SequenceName = string.Concat("v2_seq_", TableName.Substring(5).Replace("_", string.Empty));

			string sql = string.Format("select max({1}_Id) from {0}", TableName, TableName.Substring(5));
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(sql);
			decimal count = 0;
			try
			{
				count = (decimal)db.ExecuteScalar(dbCommand);
			}
			catch { }

			try
			{
				sql = string.Concat("drop SEQUENCE ", SequenceName);
				dbCommand = db.GetSqlStringCommand(sql);
				db.ExecuteNonQuery(dbCommand);
			}
			catch
			{
			}

			sql = string.Concat("CREATE SEQUENCE ", SequenceName, " MINVALUE 1 MAXVALUE 999999999999999999999999999 START WITH ", count + 1, " INCREMENT BY 1 CACHE 20");
			dbCommand = db.GetSqlStringCommand(sql);
			db.ExecuteNonQuery(dbCommand);
		}
	}
}
