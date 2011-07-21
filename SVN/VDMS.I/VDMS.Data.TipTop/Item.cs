using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace VDMS.Data.TipTop
{
    public class Motorbike : DataObjectBase
    {
        /// <summary>
        /// Get all color available from Tip-Top
        /// </summary>
        /// <returns>A dataset contains all colors</returns>
        public static DataSet GetAllColor()
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sqlCommand = string.Format("select distinct tc_col010 as ColorCode, tc_col030 as ColorName from {0}", BuildViewName("tc_col_file"));
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            return db.ExecuteDataSet(dbCommand);
        }

        /// <summary>
        /// Get all motorbikes available from Tip-Top
        /// </summary>
        /// <returns>A dataset contains all motorbikes</returns>
        public static DataSet GetAllAvailable()
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sqlCommand = string.Format("select tc_ine010 as ItemCode, tc_ine170 as ItemName, tc_ine020 as ColorCode, tc_col030 as ColorName, 0 as Price from {0}, {1} where tc_ine020 = tc_col010(+)", BuildViewName("tc_ine_file"), BuildViewName("tc_col_file"));
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            return db.ExecuteDataSet(dbCommand);
        }

        // for: split tc_spm_file to two table TC_VDA_FILE (new) and TC_VDB_FILE (old)
        public static decimal CheckVehicleCode(string engineNo, string itemCode, string dataSource)
        {
            bool isNew = dataSource.Trim().Equals("TC_VDA_FILE", StringComparison.OrdinalIgnoreCase);
            string codeField = (isNew) ? "TC_VDA04" : "TC_VDB04";
            string engNField = (isNew) ? "TC_VDA01" : "TC_VDB01";

            Database db = DatabaseFactory.CreateDatabase();
            string sqlCommand = string.Format("select count(*) from {0} where {1} = :NEW_ENGNO and {2} = :OLD_ITEMCODE",
                                        BuildViewName(dataSource),
                                        engNField, codeField);
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            db.AddInParameter(dbCommand, ":NEW_ENGNO", DbType.AnsiString, engineNo);
            db.AddInParameter(dbCommand, ":OLD_ITEMCODE", DbType.AnsiString, itemCode);
            return (decimal)db.ExecuteScalar(dbCommand);
        }
        public static decimal CheckVehicleCode(string engineNo, string itemCode)
        {
            decimal res = CheckVehicleCode(engineNo, itemCode, "TC_VDA_FILE");
            return (res == 0) ? CheckVehicleCode(engineNo, itemCode, "TC_VDB_FILE") : res;
        }

        public static int CheckExchangeEngineNo(string NewEngineNo, string OldEngineNo, string OldItemCode)
        {
            if (CheckVehicleCode(NewEngineNo, OldItemCode) == 0) return -1;

            string IssueNo = GetIssueNo(OldEngineNo);

            Database db1 = DatabaseFactory.CreateDatabase();
            string sqlCommand1 = string.Format("select count(*) from {0} where ogbb01 = :IssueNo and ogbb02 = :OLD_ENGNO", BuildViewName("ogbb_file"));
            DbCommand dbCommand1 = db1.GetSqlStringCommand(sqlCommand1);
            db1.AddInParameter(dbCommand1, ":IssueNo", DbType.AnsiString, IssueNo);
            db1.AddInParameter(dbCommand1, ":OLD_ENGNO", DbType.AnsiString, OldEngineNo);
            if ((decimal)db1.ExecuteScalar(dbCommand1) != 0) return -2;

            Database db2 = DatabaseFactory.CreateDatabase();
            string sqlCommand2 = string.Format("select count(*) from {0}, {1} where oga01 = :IssueNo and ogapost = 'Y' and oga01 = ogbb01 and ogbb02 = :NEW_ENGNO  and  ta_ogbb060 = 'Y'", BuildViewName("oga_file"), BuildViewName("ogbb_file"));
            DbCommand dbCommand2 = db2.GetSqlStringCommand(sqlCommand2);
            db2.AddInParameter(dbCommand2, ":IssueNo", DbType.AnsiString, IssueNo);
            db2.AddInParameter(dbCommand2, ":NEW_ENGNO", DbType.AnsiString, NewEngineNo);
            if ((decimal)db2.ExecuteScalar(dbCommand2) != 1) return -3;

            return 0;
        }

        static string GetIssueNo(string EngineNo)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sqlCommand = "SELECT SHIPPINGNUMBER FROM SALE_SHIPPINGHEADER WHERE SHIPPINGID = (SELECT SHIPPINGID FROM SALE_SHIPPINGDETAIL WHERE ENGINENUMBER = :ENGNO)";
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            db.AddInParameter(dbCommand, ":ENGNO", DbType.AnsiString, EngineNo);
            return (string)db.ExecuteScalar(dbCommand);
        }

        public static DataSet GetEnginePrefix(string modelLike)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sqlCommand = @"
                SELECT TC_MOC010 as  Model, TC_MOC100 as Prefix FROM
	                (
	                SELECT * FROM 
		                (SELECT TC_MOC010, TC_MOC100 FROM view_HTF_TC_MOC_FILE 
		                WHERE TC_MOC010 IN (SELECT TC_INE170 FROM view_HTF_TC_INE_FILE WHERE TC_INE050 = 1 AND TC_INE060 <> 1)		
		                GROUP BY TC_MOC010, TC_MOC100 ORDER BY TC_MOC010)
	                UNION ALL
	                SELECT * FROM 
		                (SELECT TC_MOC010, TC_MOC100 FROM view_DNF_TC_MOC_FILE 
		                WHERE TC_MOC010 IN (SELECT TC_INE170 FROM view_DNF_TC_INE_FILE WHERE TC_INE050 = 1 AND TC_INE060 <> 1)		
			                AND CONCAT(TC_MOC010, TC_MOC100) NOT IN 
				                (SELECT CONCAT(TC_MOC010, TC_MOC100) FROM view_HTF_TC_MOC_FILE 				  	  	  			   		
				                WHERE TC_MOC010 IN (SELECT TC_INE170 FROM view_HTF_TC_INE_FILE WHERE TC_INE050 = 1 AND TC_INE060 <> 1))
                        GROUP BY TC_MOC010, TC_MOC100 ORDER BY TC_MOC010)
	                )
                WHERE UPPER(TC_MOC010) like UPPER(:modelLike)
                ORDER BY TC_MOC010, TC_MOC100";
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            db.AddInParameter(dbCommand, ":modelLike", DbType.AnsiString, modelLike);
            return db.ExecuteDataSet(dbCommand);
        }

        #region Get item Info for SRS

        //        protected static string ItemsQueryString  = @"select * from
        //                (
        //                select b.*, rownum as rowindex from
        //                (
        //                  select engineno, max(OutStockDate) as OutStockDate from
        //                    (
        //                    select '{0}' as DatabaseCode, ogbb.ogbb02 as EngineNo, oga03 as DealerCode, ta_ogbb020 as ItemCode, ta_ogbb030 as ColorCode, oga02 as OutStockDate 
        //                    from VIEW_{0}_OGA_FILE oga, VIEW_{0}_OGBB_FILE ogbb 
        //                    where oga.oga01=ogbb.ogbb01 and ogbb.ogbb02 like :ENGNO
        //                    )
        //                  group by EngineNo order by engineno
        //                ) A, 
        //                (
        //                  select '{0}' as DatabaseCode, ogbb.ogbb02 as EngineNo, oga03 as DealerCode, ta_ogbb020 as ItemCode, ta_ogbb030 as ColorCode, oga02 as OutStockDate 
        //                  from VIEW_{0}_OGA_FILE oga, VIEW_{0}_OGBB_FILE ogbb 
        //                  where oga.oga01=ogbb.ogbb01 and ogbb.ogbb02 like :ENGNO order by engineno
        //                ) B where a.engineno = b.engineno and a.OutStockDate = b.OutStockDate
        //                )";

//        protected static string ItemsQueryString = @"
//            select * from 
//            (
//                select TC_VD{1}01 AS EngineNo, 
//                       TC_VD{1}02 AS FrameNo, 
//                       TC_VD{1}03 AS ItemModel, 
//                       TC_VD{1}04 AS ItemCode, 
//                       TC_VD{1}05 AS ColorCode, 
//                       TC_VD{1}06 AS InstockDate, 
//                       DealerCode,
//                       --to_date('01/01/0001', 'dd/mm/yyyy') AS OutStockDate,
//                       OutStockDate,
//                       '{0}' AS DATABASECODE,
//                       rownum as rowindex
//                from VIEW_{0}_TC_VD{1}_FILE A
//                join VIEW_{0}_ITEM_SALEINFO B
//                on B.EngineNo = A.TC_VD{1}01
//                where A.TC_VD{1}01 like :ENGNO and DealerCode like :DCODE
//            )";
        protected static string ItemsQueryString = @"
            select * from 
            (
                select TC_VD{1}01 AS EngineNo, 
                       TC_VD{1}02 AS FrameNo, 
                       TC_VD{1}03 AS ItemModel, 
                       TC_VD{1}04 AS ItemCode, 
                       TC_VD{1}05 AS ColorCode, 
                       TC_VD{1}06 AS InstockDate, 
                       --DealerCode,
                       --to_date('01/01/0001', 'dd/mm/yyyy') AS OutStockDate,
                       --OutStockDate,
                       '{0}' AS DATABASECODE,
                       rownum as rowindex
                from VIEW_{0}_TC_VD{1}_FILE
                where TC_VD{1}01 like :ENGNO
            )";
        protected static string MoreInfoFromTipTop = @"
        select DealerCode,OutStockDate
        from VIEW_{0}_ITEM_SALEINFO
        where Engineno = '{1}'
        ";
//        protected static string ItemsQueryString = @"
//                   select TC_VD{1}01 AS EngineNo, 
//                       TC_VD{1}02 AS FrameNo, 
//                       TC_VD{1}03 AS ItemModel, 
//                       TC_VD{1}04 AS ItemCode, 
//                       TC_VD{1}05 AS ColorCode, 
//                       TC_VD{1}06 AS InstockDate, 
//                       DealerCode,
//                       --to_date('01/01/0001', 'dd/mm/yyyy') AS OutStockDate,
//                       OutStockDate,
//                       '{0}' AS DATABASECODE,
//                       rownum as rowindex from (select * from VIEW_{0}_TC_VD{1}_FILE where TC_VD{1}01 like :ENGNO),(select * from VIEW_{0}_ITEM_SALEINFO where EngineNo like :ENGNO and DealerCode like :DCODE) 
//                       where TC_VD{1}01 = EngineNo and (rownum between :low and :high)";
        protected static DataSet GetItemInfo(string EngineNo, string DatabaseCode, string region, string dCode, int PageIndex, int PageSize)
        {
            //EngineNo = '%' + EngineNo.Trim() + '%';   // encode out from here

            Database db = DatabaseFactory.CreateDatabase();
            string sqlCommand = string.Format(ItemsQueryString + " where rowindex between :low and :high", DatabaseCode, region);
            //HttpContext.Current.Session["TestSearchEngine"] = HttpContext.Current.Session["TestSearchEngine"].ToString() + sqlCommand;

            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);

            db.AddInParameter(dbCommand, ":ENGNO", DbType.AnsiString, EngineNo);
            //db.AddInParameter(dbCommand, ":DCODE", DbType.AnsiString, string.IsNullOrEmpty(dCode) ? "%" : dCode);
            db.AddInParameter(dbCommand, ":low", DbType.Int64, PageIndex * PageSize + 1);
            db.AddInParameter(dbCommand, ":high", DbType.Int64, PageSize * (PageIndex + 1));
            return db.ExecuteDataSet(dbCommand);
        }
        public static DataSet GetMoreItemInfo(string EngineNo, string DatabaseCode)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sqlCommand = string.Format(MoreInfoFromTipTop, DatabaseCode, EngineNo);

            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            return db.ExecuteDataSet(dbCommand);
        }

        // from first upgrade: Motorbike.GetItemInfo() does not return DealerCode and OutStockDate
        // use GetItemSaleInfo() to colect these info
        public static DataSet GetItemInfo(string EngineNo, string DatabaseCode, string dCode, int PageIndex, int PageSize)
        {
            DataSet res = GetItemInfo(EngineNo, DatabaseCode, "A", dCode, PageIndex, PageSize);
            //HttpContext.Current.Session["TestSearchEngine"] = HttpContext.Current.Session["TestSearchEngine"].ToString() + string.Format("\n Return: {0}\n", res.Tables[0].Rows.Count.ToString());
            if ((res == null) || (res.Tables[0].Rows.Count <= 0))
            {
                res = GetItemInfo(EngineNo, DatabaseCode, "B", dCode, PageIndex, PageSize);
                //HttpContext.Current.Session["TestSearchEngine"] = HttpContext.Current.Session["TestSearchEngine"].ToString() + string.Format("\n Return: {0}\n", res.Tables[0].Rows.Count.ToString());
            }

            return res;
        }

        public static decimal GetItemInfoCount(string EngineNo, string DatabaseCode, string dCode)
        {
            decimal itemCount = GetItemInfoCount(EngineNo, DatabaseCode, "A", dCode);
            if (itemCount <= 0)
            {
                itemCount = GetItemInfoCount(EngineNo, DatabaseCode, "B", dCode);
            }

            return itemCount;
        }
        public static decimal GetItemInfoCount(string EngineNo, string DatabaseCode, string region, string dCode)
        {
            //EngineNo = '%' + EngineNo.Trim() + '%';   // encode out from here
            Database db = DatabaseFactory.CreateDatabase();
            string sqlCommand = string.Format(@"select count(*) from (" + ItemsQueryString + ")", DatabaseCode, region);
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            db.AddInParameter(dbCommand, ":ENGNO", DbType.AnsiString, EngineNo);
            //db.AddInParameter(dbCommand, ":DCODE", DbType.AnsiString, string.IsNullOrEmpty(dCode) ? "%" : dCode);
            return (decimal)db.ExecuteScalar(dbCommand);
        }

        public static DataRow GetItemSaleInfo(string engineNo, string dbCode)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                string sqlCommand = string.Format(@"select * from VIEW_{0}_ITEM_SALEINFO where EngineNo = :ENGNO", dbCode);
                DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
                db.AddInParameter(dbCommand, ":ENGNO", DbType.AnsiString, engineNo);
                DataSet ds = db.ExecuteDataSet(dbCommand);
                return (ds.Tables[0].Rows.Count > 0) ? ds.Tables[0].Rows[0] : null;
            }
            catch { return null; }
        }

        #endregion

        #region Get item info in SPM

        /// <summary>
        /// Search in ALL_SPM, exactly matching
        /// </summary>
        /// <param name="EngineNo"></param>
        /// <returns></returns>
        public static DataTable GetItemInfoAtAll(string EngineNo)
        {
            string sql = string.Format(
                @"select * from VIEW_ALL_TC_SPM_FILE 
                where EngineNumber = '{0}'"
                , EngineNo);
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            DataSet ds = db.ExecuteDataSet(dbCommand);
            return ds.Tables[0];
        }

        #endregion
    }
}
