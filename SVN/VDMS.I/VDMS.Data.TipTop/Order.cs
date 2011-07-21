using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace VDMS.II.Data.TipTop
{
	public class Order : VDMS.Data.TipTop.DataObjectBase
	{
		public static DataSet GetOrderOverInStock(DateTime d)
		{
			Database db = DatabaseFactory.CreateDatabase();
			StringBuilder query = new StringBuilder();
			//query.Append("select h.ORDER_HEADER_ID, h.DEALER_CODE, h.ORDER_DATE from V2_P_ORDER_aHEADER h inner join V2_P_DEALER d on\n");
			//query.Append("h.DEALER_CODE = d.DEALER_CODE and h.SHIPPING_DATE is not null\n");
			//query.Append("and h.AUTO_IN_STOCK_DATE < :p_date - d.RECEIVE_SPAN and h.Already_In_Stock = 0");
            query.Append("select distinct h.Order_Header_Id, d.Dealer_Code, d.AUTOINSTOCK_PART_SPAN as Receive_Span, s.TC_VDR03 from\n");
            query.Append("TC_VDR_FILE s inner join V2_P_ORDER_HEADER h on s.TC_VDR01 = h.Order_Header_Id\n");
            query.Append("inner join V2_P_DEALER d on h.To_Dealer = d.Dealer_Code\n");
            query.Append("where s.TC_VDR03 not in (select ISSUE_NUMBER from V2_P_RECEIVE_HEADER)\n");
            query.Append(" and d.AUTOINSTOCK_PART_STATUS = 1 and s.TC_VDR06 < :p_date - 1/24 * d.AUTOINSTOCK_PART_SPAN ");
            DbCommand dbCommand = db.GetSqlStringCommand(query.ToString());
            db.AddInParameter(dbCommand, ":p_date", DbType.DateTime, d);
            return db.ExecuteDataSet(dbCommand);
		}
	}
}

namespace VDMS.Data.TipTop
{
	public class Order : DataObjectBase
	{
		/// <summary>
		/// Get order detail from Tip-Top
		/// </summary>
		/// <param name="OrderNumber"></param>
		/// <returns></returns>
		public static DataSet GetOrderDetail(string OrderNumber)
		{
			Database db = DatabaseFactory.CreateDatabase();
			string sqlCommand = string.Format("select oeb04 as ItemCode, oeb12 as Quantity, oeb13 as Price, ta_oeb020 as Priority from {0} where oeb01 = :OrderNumber", BuildViewName("oeb_file"));
			DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
			db.AddInParameter(dbCommand, ":OrderNumber", DbType.AnsiString, OrderNumber);
			return db.ExecuteDataSet(dbCommand);
		}

		public static bool IsConfirmedOrderExist(string OrderNumber, string DealerCode, bool CheckIssue)
		{
			Database db = DatabaseFactory.CreateDatabase();
			string sqlCommand = string.Format("select count(*) from {0} where oea01 = :OrderNumber and oeaconf='Y' and OEA03 = :DealerCode and not exists (Select * from {1} where ogb31=:OrderNumber)", BuildViewName("oea_file"), BuildViewName("ogb_file"));
			if (!CheckIssue) sqlCommand = string.Format("select count(*) from {0} where oea01 = :OrderNumber and oeaconf='Y' and OEA03 = :DealerCode", BuildViewName("oea_file"));

			DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
			db.AddInParameter(dbCommand, ":OrderNumber", DbType.AnsiString, OrderNumber);
			db.AddInParameter(dbCommand, ":DealerCode", DbType.AnsiString, DealerCode);
			return (decimal)db.ExecuteScalar(dbCommand) > 0;
		}

		public static DateTime GetOrderDate(string OrderNumber)
		{
			Database db = DatabaseFactory.CreateDatabase();
			string sqlCommand = string.Format("select oea02 from {0} where oea01 = :OrderNumber", BuildViewName("oea_file"));
			DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
			db.AddInParameter(dbCommand, ":OrderNumber", DbType.AnsiString, OrderNumber);
			try
			{
				return (DateTime)db.ExecuteScalar(dbCommand);
			}
			catch
			{
				return DateTime.Now;
			}
		}
	}
}
