using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;

namespace VDMS.Data.TipTop
{
    public class Shipping : DataObjectBase
    {
        public static string GetShippingAddress(string ShippingNumber, string DealerCode)
        {
            DataRow row = GetShippingHeader(ShippingNumber, DealerCode);
            return (row == null) ? string.Empty : (string)row["ShippingAddress"];
        }
        #region Auto
        public static DataTable GetShippingHeader2_auto(string ShippingNumber, string DealerCode, string databasecode)
        {
            if (ShippingNumber == "LOCALTEST")
            {
                DataTable tbl = new DataTable();
                tbl.Columns.Add("ShippingNumber");
                tbl.Columns.Add("ShippingDate", typeof(DateTime));
                tbl.Columns.Add("DealerCode");
                tbl.Columns.Add("BranchCode");
                tbl.Columns.Add("ShippingTo");
                tbl.Columns.Add("ShippingAddress");
                DataRow row = tbl.NewRow();
                row["ShippingNumber"] = "LOCALTEST";
                row["ShippingDate"] = VDMS.Common.Utils.DataFormat.DateFromString("1/9/2009");
                row["DealerCode"] = "KH001A";
                row["BranchCode"] = "KH001A";
                row["ShippingTo"] = "KH001A";
                row["ShippingAddress"] = "KH001A-HOA-980008";
                tbl.Rows.Add(row);
                return tbl;
            }

            Database db = DatabaseFactory.CreateDatabase();

            string sqlCommand = string.Format(
                "select oga01 as ShippingNumber, oga02 as ShippingDate, oga03 as DealerCode, oga04 as BranchCode, oga04 as ShippingTo, " +
                    "CASE " +
                    "WHEN oga044 IS NULL THEN " +
                        "(SELECT concat(concat(concat(concat(occ241,' / '),occ242),' / '),occ243) " +
                        "FROM {1} WHERE occ01 = :DealerCode) " +
                    "WHEN oga044 = 'MISC' THEN " +
                        "(SELECT concat(concat(concat(concat(oap041,' / '),oap042),' / '),oap043) " +
                        "FROM {2} WHERE oap01 = oga01) " +
                    "ELSE " +
                        "(SELECT concat(concat(concat(ocd02, ' ( '), concat(concat(concat(concat(ocd221,' / '),ocd222),' / '),ocd223)), ' )') " +
                        "FROM {3} WHERE ocd01 = :DealerCode AND ocd02 = oga044) " +
                    "END  \"ShippingAddress\" " +
                "from {0} where oga00=1 and ogaconf = 'Y' and oga01=:ShippingNumber and oga03=:DealerCode",
                BuildViewName2("oga_file",databasecode),
                BuildViewName2("occ_file",databasecode),
                BuildViewName2("oap_file",databasecode),
                BuildViewName2("ocd_file",databasecode));

            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);

            db.AddInParameter(dbCommand, ":ShippingNumber", DbType.AnsiString, ShippingNumber);
            db.AddInParameter(dbCommand, ":DealerCode", DbType.AnsiString, DealerCode);

            DataSet ds = db.ExecuteDataSet(dbCommand);

            if (ds.Tables[0].Rows.Count > 0) return ds.Tables[0];
            return null;
        }
        public static DataRow GetShippingHeader_auto(string ShippingNumber, string DealerCode,string databasecode)
        {
            var tbl = GetShippingHeader2_auto(ShippingNumber, DealerCode,databasecode);
            return (tbl != null) && (tbl.Rows.Count > 0) ? tbl.Rows[0] : null;
        }
        #endregion
        public static DataTable GetShippingHeader2(string ShippingNumber, string DealerCode)
        {
            if (ShippingNumber == "LOCALTEST")
            {
                DataTable tbl = new DataTable();
                tbl.Columns.Add("ShippingNumber");
                tbl.Columns.Add("ShippingDate", typeof(DateTime));
                tbl.Columns.Add("DealerCode");
                tbl.Columns.Add("BranchCode");
                tbl.Columns.Add("ShippingTo");
                tbl.Columns.Add("ShippingAddress");
                DataRow row = tbl.NewRow();
                row["ShippingNumber"] = "LOCALTEST";
                row["ShippingDate"] = VDMS.Common.Utils.DataFormat.DateFromString("1/9/2009");
                row["DealerCode"] = "KH001A";
                row["BranchCode"] = "KH001A";
                row["ShippingTo"] = "KH001A";
                row["ShippingAddress"] = "KH001A-HOA-980008";
                tbl.Rows.Add(row);
                return tbl;
            }

            Database db = DatabaseFactory.CreateDatabase();

            string sqlCommand = string.Format(
                "select oga01 as ShippingNumber, oga02 as ShippingDate, oga03 as DealerCode, oga04 as BranchCode, oga04 as ShippingTo, " +
                    "CASE " +
                    "WHEN oga044 IS NULL THEN " +
                        "(SELECT concat(concat(concat(concat(occ241,' / '),occ242),' / '),occ243) " +
                        "FROM {1} WHERE occ01 = :DealerCode) " +
                    "WHEN oga044 = 'MISC' THEN " +
                        "(SELECT concat(concat(concat(concat(oap041,' / '),oap042),' / '),oap043) " +
                        "FROM {2} WHERE oap01 = oga01) " +
                    "ELSE " +
                        "(SELECT concat(concat(concat(ocd02, ' ( '), concat(concat(concat(concat(ocd221,' / '),ocd222),' / '),ocd223)), ' )') " +
                        "FROM {3} WHERE ocd01 = :DealerCode AND ocd02 = oga044) " +
                    "END  \"ShippingAddress\" " +
                "from {0} where oga00=1 and ogaconf = 'Y' and oga01=:ShippingNumber and oga03=:DealerCode",
                BuildViewName("oga_file"),
                BuildViewName("occ_file"),
                BuildViewName("oap_file"),
                BuildViewName("ocd_file"));

            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);

            db.AddInParameter(dbCommand, ":ShippingNumber", DbType.AnsiString, ShippingNumber);
            db.AddInParameter(dbCommand, ":DealerCode", DbType.AnsiString, DealerCode);

            DataSet ds = db.ExecuteDataSet(dbCommand);

            if (ds.Tables[0].Rows.Count > 0) return ds.Tables[0];
            return null;
        }
        public static DataRow GetShippingHeader(string ShippingNumber, string DealerCode)
        {
            var tbl = GetShippingHeader2(ShippingNumber, DealerCode);
            return (tbl != null) && (tbl.Rows.Count > 0) ? tbl.Rows[0] : null;
        }

        public static string BuildShippingHeadersQuery(string OrderNumber, string DealerCode)
        {
            return string.Format(@"
                    select rownum as rowindex, oga01 as ShippingNumber, oga02 as ShippingDate, oga03 as DealerCode, oga04 as BranchCode, oga04 as ShippingTo, 
                        CASE 
                            WHEN oga044 IS NULL THEN 
                            (SELECT concat(concat(concat(concat(occ241,' / '),occ242),' / '),occ243)
                            FROM {1} WHERE occ01 = oga03) 
                        WHEN oga044 = 'MISC' THEN 
                            (SELECT concat(concat(concat(concat(oap041,' / '),oap042),' / '),oap043) 
                            FROM {2} WHERE oap01 = oga01)
                        ELSE 
                            (SELECT concat(concat(concat(ocd02, ' ( '), concat(concat(concat(concat(ocd221,' / '),ocd222),' / '),ocd223)), ' )') 
                            FROM {3} WHERE ocd01 = oga03 AND ocd02 = oga044) 
                        END ""ShippingAddress"" 
                    from {0} where oga00=1 and ogaconf = 'Y' and ogapost = 'Y' and oga03 like '{6}' and
                    exists (select ogb01 from {4} where ogb31 = '{5}' and ogb01 = oga01)",
                    BuildViewName("oga_file"),
                    BuildViewName("occ_file"),
                    BuildViewName("oap_file"),
                    BuildViewName("ocd_file"),
                    BuildViewName("ogb_file"),
                    OrderNumber,
                    DealerCode
                    );
        }
        public static int CountShippingHeaders(string OrderNumber, string DealerCode)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = string.Format("select count(*) from ({0})", BuildShippingHeadersQuery(OrderNumber, DealerCode));
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            object res = db.ExecuteScalar(dbCommand);
            return (int)(decimal)res;
        }
        public static DataTable GetShippingHeaders(string OrderNumber, string DealerCode)
        {
            return GetShippingHeaders(OrderNumber, DealerCode, -1, -1);
        }
        public static DataTable GetShippingHeaders(string OrderNumber, string DealerCode, int start, int max)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sqlCommand = BuildShippingHeadersQuery(OrderNumber, DealerCode);

            if ((start >= 0) && (max > 0))
            {
                sqlCommand = string.Format("select * from ({0}) where (rowindex between {1} and {2})", sqlCommand, start + 1, start + max);
            }

            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds.Tables[0];
        }

        public static DataSet GetShippingHeader(string DealerCode)
        {
            Database db = DatabaseFactory.CreateDatabase();

            string sqlCommand = string.Format(
                "select oga01 as ShippingNumber, oga02 as ShippingDate, oga03 as DealerCode, oga04 as BranchCode, oga04 as ShippingTo, " +
                    "CASE " +
                    "WHEN oga044 IS NULL THEN " +
                        "(SELECT concat(concat(concat(concat(occ241,' / '),occ242),' / '),occ243) " +
                        "FROM {1} WHERE occ01 = :DealerCode) " +
                    "WHEN oga044 = 'MISC' THEN " +
                        "(SELECT concat(concat(concat(concat(oap041,' / '),oap042),' / '),oap043) " +
                        "FROM {2} WHERE oap01 = oga01) " +
                    "ELSE " +
                        "(SELECT concat(concat(concat(ocd02, ' ( '), concat(concat(concat(concat(ocd221,' / '),ocd222),' / '),ocd223)), ' )') " +
                        "FROM {3} WHERE ocd01 = 'AG001A' AND ocd02 = oga044) " +
                    "END  \"ShippingAddress\" " +
                "from {0} where oga00=1 AND oga03=:DealerCode",
                BuildViewName("oga_file"),
                BuildViewName("occ_file"),
                BuildViewName("oap_file"),
                BuildViewName("ocd_file"));

            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);

            db.AddInParameter(dbCommand, ":DealerCode", DbType.AnsiString, DealerCode);

            return db.ExecuteDataSet(dbCommand);
        }

        public static DataSet GetShippingDetail(string ShippingNumber)
        {
            DataSet ds;

            #region test

            if (ShippingNumber == "LOCALTEST")
            {
                ds = new DataSet();
                DataTable tbl = new DataTable();
                tbl.Columns.Add("EngineNumber");
                tbl.Columns.Add("ItemModel");
                tbl.Columns.Add("ItemCode");
                tbl.Columns.Add("ItemType");
                tbl.Columns.Add("OrderNumber");
                tbl.Columns.Add("MadeDate", typeof(DateTime));
                tbl.Columns.Add("ItemName");
                tbl.Columns.Add("Price", typeof(int));
                tbl.Columns.Add("Color");
                tbl.Columns.Add("ColorName");
                tbl.Columns.Add("OrderNo");
                tbl.Columns.Add("Status");
                tbl.Columns.Add("Exception");

                DataRow row = tbl.NewRow();
                row["EngineNumber"] = "VMVD7B-H006863";
                row["ItemModel"] = "VTC-CG-RT";
                row["ItemCode"] = "VTC-CG-RT";
                row["ItemType"] = "VTC";
                row["OrderNumber"] = "HOA-980008";
                row["MadeDate"] = VDMS.Common.Utils.DataFormat.DateFromString("1/9/2008");
                row["Status"] = 1;
                row["ItemName"] = "HOA-VMVTBB";
                row["OrderNo"] = "HOA-980008";
                row["Price"] = 980008;
                row["Color"] = "HOA-980008";
                row["ColorName"] = "HOA-980008";
                row["Exception"] = "";
                tbl.Rows.Add(row);
                ds.Tables.Add(tbl);
                return ds;
            }
            #endregion

            Database db = DatabaseFactory.CreateDatabase();

            //--VIEW_DNF_TC_SPM_FILE B : doan thay the lay tu Motorbike.GetItemInfo
            // OrderNo la LineNo
            string sqlCommand = string.Format(@"select A.*, B.OutStockDate from
               (
	            select ogb04 as ItemCode, 
                ta_ogbb020 as Model, 
                ogb06 as ItemName, 
                ogbb02 as EngineNumber, 
                ogb03 as OrderNo, 
                ogb13 as Price, 
                ta_ogbb030 as ColorCode, 
                tc_col030 as ColorName, 
                ogb31 as TipTopOrderNumber, 
                0 as Status, '' as Exception,'' as BranchCode

		        from {1} inner join {0} on {0}.ogb01 = {1}.ogbb01 and {0}.ogb03 = {1}.ogbb012
			    left join {2} on {1}.ta_ogbb030 = {2}.tc_col010
		        where ogb01=:ShippingNumber
               ) A, 
                (
                  select engineno, max(OutStockDate) as OutStockDate from
                    (
                    select ogbb.ogbb02 as EngineNo, oga02 as OutStockDate 
                    from {3} oga, {1} ogbb
                    where oga.oga01=ogbb.ogbb01 
                    )
                  group by EngineNo
                ) B
               where A.EngineNumber = B.EngineNo(+)",
                   BuildViewName("ogb_file"),
                   BuildViewName("ogbb_file"),
                   BuildViewName("tc_col_file"),
                   BuildViewName("OGA_FILE"));

            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);

            db.AddInParameter(dbCommand, ":ShippingNumber", DbType.AnsiString, ShippingNumber);
            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }
        public static DataSet GetShippingDetail(string ShippingNumber, string OrderNumber)
        {
            Database db = DatabaseFactory.CreateDatabase();

            //--VIEW_DNF_TC_SPM_FILE B : doan thay the lay tu Motorbike.GetItemInfo
            string sqlCommand = string.Format(@"select A.*, B.OutStockDate as MadeDate from
               (
	            select ogb04 as ItemCode, ogb04 as ItemModel, ta_ogbb020 as ItemType, ogb06 as ItemName, ogbb02 as EngineNumber, ogb03 as OrderNo, ogb13 as Price, ta_ogbb030 as Color, tc_col030 as ColorName, ogb31 as OrderNumber, 0 as Status, '' as Exception
		        from {1} inner join {0} on {0}.ogb01 = {1}.ogbb01 and {0}.ogb03 = {1}.ogbb012
			    left join {2} on {1}.ta_ogbb030 = {2}.tc_col010
		        where ogb01=:ShippingNumber and ogb31 = :OrderNumber
               ) A, 
                (
                  select engineno, max(OutStockDate) as OutStockDate from
                    (
                    select ogbb.ogbb02 as EngineNo, oga02 as OutStockDate 
                    from {3} oga, {1} ogbb
                    where oga.oga01=ogbb.ogbb01 
                    )
                  group by EngineNo
                ) B
               where A.EngineNumber = B.EngineNo(+)",
                   BuildViewName("ogb_file"),
                   BuildViewName("ogbb_file"),
                   BuildViewName("tc_col_file"),
                   BuildViewName("OGA_FILE"));

            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);

            db.AddInParameter(dbCommand, ":ShippingNumber", DbType.AnsiString, ShippingNumber);
            db.AddInParameter(dbCommand, ":OrderNumber", DbType.AnsiString, OrderNumber);

            DataSet ds = db.ExecuteDataSet(dbCommand);
            //DataRow row = ds.Tables[0].NewRow();
            //row["EngineNumber"] = "VMM9BD-D029916";
            //row["ItemModel"] = "M9R-WD";
            //row["ItemCode"] = "M9R-WD";
            //row["ItemType"] = "M9R";
            //row["status"] = 1;
            //row["OrderNumber"] = "HOA-6A0050";
            //ds.Tables[0].Rows.Add(row);

            return ds;
        }

        public static int GetNumberOfShippingItem(string OrderNumber, string ItemCode)
        {
            Database db = DatabaseFactory.CreateDatabase();

            string sqlCommand = string.Format("select SUM(ogb12) from {0} where ogb31=:OrderNumber AND ogb04=:ItemCode", BuildViewName("ogb_file"));
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);

            db.AddInParameter(dbCommand, ":OrderNumber", DbType.AnsiString, OrderNumber);
            db.AddInParameter(dbCommand, ":ItemCode", DbType.AnsiString, ItemCode);

            try
            {
                return int.Parse(db.ExecuteScalar(dbCommand).ToString());
            }
            catch
            {
                return 0;
            }
        }

        public static DataTable GetInfoOfIssue(string issuenumber, string databasecode)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sqlCommnad = string.Format("select car_number, driver_name,driver_tel,exec_name,exec_tel,vmep1_name,vmep1_tel,vmep2_name,vmep2_tel,issue_memo from view_sale_deli_info where issue_no = '{0}' and database_code = '{1}'", issuenumber, databasecode);
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommnad);
            DataSet ds = db.ExecuteDataSet(dbCommand);
            return ds.Tables[0];
        }

    }
}
