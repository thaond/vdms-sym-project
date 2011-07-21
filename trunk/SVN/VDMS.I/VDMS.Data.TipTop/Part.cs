﻿using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace VDMS.Data.TipTop
{
    public class Part : DataObjectBase
    {
        //public static DataTable GetPartList(string PartCode, string PartName, string EngineNumber, string Model, string databaseCode, int RowIndex, int PageSize)
        //{
        //    return Part.GetPartList(PartCode, PartName, EngineNumber, Model, null, databaseCode, DateTime.MinValue, DateTime.MaxValue, RowIndex, PageSize);
        //}

        /// <summary>
        /// Get the part list from the Tip-Top
        /// </summary>
        /// <param name="PartCode"></param>
        /// <param name="PartName"></param>
        /// <param name="EngineNumber"></param>
        /// <param name="Model"></param>
        /// <param name="category"></param>
        /// <param name="databaseCode"></param>
        /// <param name="createFrom"></param>
        /// <param name="createTo"></param>
        /// <param name="RowIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        //        public static DataTable GetPartList(string PartCode, string PartName, string EngineNumber, string Model, string category, string databaseCode, DateTime createFrom, DateTime createTo, int RowIndex, int PageSize)
        //        {
        //            //DateTime dtFrom = DataFormat.DateFromString(createFrom);
        //            //DateTime dtTo = DataFormat.DateFromString(createTo);
        //            //if (dtTo == DateTime.MinValue) dtTo = DateTime.MaxValue;
        //            // TO DO: add created date cond

        //#warning check category and date cond

        //            if (!string.IsNullOrEmpty(EngineNumber))
        //            {
        //                var ds = Motorbike.GetItemInfo(EngineNumber, databaseCode, 0, 1);
        //                if (ds.Tables[0].Rows.Count != 0)
        //                {
        //                    Model = (string)ds.Tables[0].Rows[0]["ItemCode"];
        //                }
        //            }

        //            Database db = DatabaseFactory.CreateDatabase();
        //            StringBuilder query = new StringBuilder();
        //            if (string.IsNullOrEmpty(Model)) query.AppendFormat("select * from (SELECT ima01 as PartCode, ima02 as EnglishName, ta_ima030 as VietnamName, ima105 as Category, 'N/A' as CurrentStock, 0 as PartInfoId, rownum as RowIndex FROM view_{0}_ima_file where ima130=1", databaseCode);
        //            else
        //            {
        //                query.AppendFormat("select * from (SELECT ima01 as PartCode, ima02 as EnglishName, ta_ima030 as VietnamName, ima105 as Category, 'N/A' as CurrentStock, 0 as PartInfoId, rownum as RowIndex FROM view_{0}_ima_file ima\n", databaseCode);
        //                query.AppendFormat("left outer join VIEW_{0}_tc_vdn_file tc_vdn on IMA.IMA01 = TC_VDN.CHILD where ima130=1", databaseCode);
        //            }
        //            if (!string.IsNullOrEmpty(PartCode)) query.Append(" AND ima01 like :p_code");
        //            if (!string.IsNullOrEmpty(category)) query.Append(" AND ima105 like :p_cat");
        //            if (!string.IsNullOrEmpty(PartName)) query.Append(" AND (ima02 like :p_name or ta_ima030 like :p_name)");
        //            if (!string.IsNullOrEmpty(Model)) query.Append(" AND parent = :p_model");
        //            query.Append(") A where RowIndex between :low and :high");
        //            DbCommand dbCommand = db.GetSqlStringCommand(query.ToString().ToLower().Replace("_dnf_", "_dnp_").Replace("_htf_", "_htp_"));
        //            db.AddInParameter(dbCommand, ":low", DbType.Int64, RowIndex + 1);
        //            db.AddInParameter(dbCommand, ":high", DbType.Int64, RowIndex + PageSize);
        //            if (!string.IsNullOrEmpty(PartCode)) db.AddInParameter(dbCommand, ":p_code", DbType.AnsiString, "%" + PartCode + "%");
        //            if (!string.IsNullOrEmpty(PartName)) db.AddInParameter(dbCommand, ":p_name", DbType.String, "%" + PartName + "%");
        //            if (!string.IsNullOrEmpty(Model)) db.AddInParameter(dbCommand, ":p_model", DbType.String, Model);
        //            if (!string.IsNullOrEmpty(category)) db.AddInParameter(dbCommand, ":p_cat", DbType.String, category);

        //            DataSet res = db.ExecuteDataSet(dbCommand);
        //            return res.Tables[0];
        //        }

        //public static decimal GetPartListCount(string PartCode, string PartName, string EngineNumber, string Model, string databaseCode)
        //{
        //    return GetPartListCount(PartCode, PartName, EngineNumber, Model, null, databaseCode, DateTime.MinValue, DateTime.MaxValue);
        //}
        /// <summary>
        /// Get Part Count
        /// </summary>
        /// <param name="PartCode"></param>
        /// <param name="PartName"></param>
        /// <param name="EngineNumber"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        //        public static decimal GetPartListCount(string PartCode, string PartName, string EngineNumber, string Model, string category, string databaseCode, DateTime createFrom, DateTime createTo)
        //        {
        //            //DateTime dtFrom = DataFormat.DateFromString(createFrom);
        //            //DateTime dtTo = DataFormat.DateFromString(createTo);
        //            //if (dtTo == DateTime.MinValue) dtTo = DateTime.MaxValue;
        //            // TO DO: add created date cond

        //#warning check category cond
        //            databaseCode = databaseCode.ToLower().Replace("f", "p");
        //            Database db = DatabaseFactory.CreateDatabase();
        //            StringBuilder query = new StringBuilder();
        //            if (string.IsNullOrEmpty(Model)) query.Append(string.Format("select count(*) from view_{0}_ima_file where 1=1", databaseCode));
        //            else
        //            {
        //                query.AppendFormat("SELECT count(*) FROM view_{0}_ima_file ima\n", databaseCode);
        //                query.AppendFormat("left outer join view_{0}_tc_vdn_file tc_vdn on IMA.IMA01 = TC_VDN.CHILD where ima130=1", databaseCode);
        //            }

        //            if (!string.IsNullOrEmpty(PartCode)) query.Append(" AND ima01 like :p_code");
        //            if (!string.IsNullOrEmpty(category)) query.Append(" AND ima105 like :p_cat");
        //            if (!string.IsNullOrEmpty(PartName)) query.Append(" AND (ima02 like :p_name or ta_ima030 like :p_name)");
        //            if (!string.IsNullOrEmpty(Model)) query.Append(" AND parent = :p_model");
        //            DbCommand dbCommand = db.GetSqlStringCommand(query.ToString());
        //            if (!string.IsNullOrEmpty(PartCode)) db.AddInParameter(dbCommand, ":p_code", DbType.AnsiString, "%" + PartCode + "%");
        //            if (!string.IsNullOrEmpty(PartName)) db.AddInParameter(dbCommand, ":p_name", DbType.String, "%" + PartName + "%");
        //            if (!string.IsNullOrEmpty(Model)) db.AddInParameter(dbCommand, ":p_model", DbType.String, Model);
        //            if (!string.IsNullOrEmpty(category)) db.AddInParameter(dbCommand, ":p_cat", DbType.String, category);


        //            decimal res = (decimal)db.ExecuteScalar(dbCommand);

        //            return res;
        //        }

        public static bool IsPartExist(string PartCode, string databaseCode, bool checkStatus)
        {
            databaseCode = databaseCode.ToLower().Replace("f", "p");
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder query = new StringBuilder();
            // Change request 21/7/2010 : Them cot STATUS trong view
            query.Append(string.Format("select count(*) from view_{0}_ima_file where {1} = :p_code", databaseCode, databaseCode.ToLower() == "all" ? "Part_Code" : "ima01"));
            if (checkStatus)
                query.Append(string.Format(" and {0} <> 'N'", databaseCode.ToLower() == "all" ? "Status" : "ta_ima070"));
            DbCommand dbCommand = db.GetSqlStringCommand(query.ToString());
            db.AddInParameter(dbCommand, ":p_code", DbType.AnsiString, PartCode);
            return (decimal)db.ExecuteScalar(dbCommand) >= (decimal)1;
        }

        public static DataSet GetModelList(string databaseCode)
        {
            databaseCode = databaseCode.ToLower().Replace("f", "p");
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder query = new StringBuilder();
			// mr.Giang bat phai doi lai nhu cu 01/12/2009 11h:10 AM
            query.Append(string.Format("select distinct parent as model, descript from view_{0}_tc_vdn_file where unitcode='MD' order by parent", databaseCode));
            //query.Append(string.Format("select distinct model, descript from view_{0}_tc_vdn_file where unitcode='MD' order by model", databaseCode));
            DbCommand dbCommand = db.GetSqlStringCommand(query.ToString());
            return db.ExecuteDataSet(dbCommand);
        }

        public static DataSet GetModelList(string dbCode, string partCode, string engineNo)
        {
            string databaseCode = dbCode.ToLower().Replace("f", "p");
            Database db = DatabaseFactory.CreateDatabase();
            if (!string.IsNullOrEmpty(engineNo))
            {
                try
                {
                    var tbl = Motorbike.GetItemInfoAtAll(engineNo);
                    if (tbl.Rows.Count != 0)
                    {
                        var Model = (string)tbl.Rows[0]["model"];
						// mr.Giang bat phai doi lai nhu cu 01/12/2009 11h:10 AM
                        return db.ExecuteDataSet(db.GetSqlStringCommand(string.Format("select distinct parent as model, descript from view_{0}_tc_vdn_file where unitcode='MD' and parent like '{1}' order by parent", databaseCode, Model)));
                        //return db.ExecuteDataSet(db.GetSqlStringCommand(string.Format("select distinct model, descript from view_{0}_tc_vdn_file where unitcode='MD' and model like '{1}' order by model", databaseCode, Model)));
                    }
                }
                catch { }
            }

            partCode = "%" + partCode + "%";
			// mr.Giang bat phai doi lai nhu cu 01/12/2009 11h:10 AM
            var query = string.Format("select distinct parent as model, descript from view_{0}_tc_vdn_file where unitcode='MD' and child like :p_code order by parent", databaseCode);
            //var query = string.Format("select distinct model, descript from view_{0}_tc_vdn_file where unitcode='MD' and child like :p_code order by model", databaseCode);
            DbCommand dbCommand = db.GetSqlStringCommand(query);
            db.AddInParameter(dbCommand, ":p_code", DbType.AnsiString, partCode);
            return db.ExecuteDataSet(dbCommand);
        }

        public static decimal GetPartPrice(string PartCode)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder query = new StringBuilder();
            query.Append("SELECT DISTINCT a.xmf07\n");
            query.AppendFormat("FROM {0} a , (SELECT xmf03, MAX(xmf05) as LAST_DATE FROM {0}\n", BuildViewName("xmf_file", 2));
            query.Append("WHERE xmf07 is not null GROUP BY xmf03) temp WHERE a.xmf03=temp.xmf03\n");
            query.Append("AND a.xmf01='DSP' AND a.xmf02='VND' AND a.xmf05=temp.LAST_DATE AND a.xmf03=:p_code");
            DbCommand dbCommand = db.GetSqlStringCommand(query.ToString());
            db.AddInParameter(dbCommand, ":p_code", DbType.AnsiString, PartCode);
            try
            {
                return (decimal)db.ExecuteScalar(dbCommand);
            }
            catch
            {
                return 0;
            }
        }

        public static int SyncNewVDMSIParts(int defLabour)
        {
            Database db = DatabaseFactory.CreateDatabase();
            
            DbCommand dbCommand = db.GetStoredProcCommand("Part.AddNewPartsI");
            db.AddParameter(dbCommand, "retval", DbType.Int32, 0, ParameterDirection.ReturnValue, true, 0, 0, String.Empty, DataRowVersion.Current, Convert.DBNull);
            db.AddParameter(dbCommand, "p_defLabour", DbType.Int32, 0, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Current, defLabour);
            
            db.ExecuteNonQuery(dbCommand);
            int rows = (int)db.GetParameterValue(dbCommand, "retval");

            return rows;
        }
        public static int SyncVDMSIPartsPrice()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("Part.UpdatePartsPriceI");
            db.AddParameter(dbCommand, "retval", DbType.Int32, 0, ParameterDirection.ReturnValue, true, 0, 0, String.Empty, DataRowVersion.Current, Convert.DBNull);

            db.ExecuteNonQuery(dbCommand);
            int rows = (int)db.GetParameterValue(dbCommand, "retval");

            return rows;
        }
    }
}
