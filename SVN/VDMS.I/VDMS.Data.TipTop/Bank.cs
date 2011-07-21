using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace VDMS.Data.TipTop
{
    public class Bank
    {
        public static bool Test = false;
        public static DataTable GetBankList(string dCode)
        {
            if (Test)
            {
                DataTable t = new DataTable();
                t.Columns.Add("DEALER_CODE");
                t.Columns.Add("DEALER_NAME");
                t.Columns.Add("BANK_CODE");
                t.Columns.Add("BANK_NAME");
                var r = t.NewRow();
                r["DEALER_CODE"] = "NGM001A"; r["DEALER_NAME"] = "NGM001A"; r["BANK_CODE"] = "VCB"; r["BANK_NAME"] = "VietCom"; t.Rows.Add(r);
                r = t.NewRow(); 
                r["DEALER_CODE"] = "NGM001A"; r["DEALER_NAME"] = "NGM001A"; r["BANK_CODE"] = "ACB"; r["BANK_NAME"] = "DongA"; t.Rows.Add(r);

                return t;
            }

            var db = DatabaseFactory.CreateDatabase();
            string sqlCommand = "select distinct BANK_CODE, BANK_NAME, BANK_NAME || ' (' || BANK_CODE || ')' as FULL_BANK_NAME from VIEW_BANK_FOR_DEALER";
            if (!string.IsNullOrEmpty(dCode)) sqlCommand = sqlCommand + string.Format(" where DEALER_CODE = '{0}'", dCode);

            var dbCommand = db.GetSqlStringCommand(sqlCommand);
            var dt = db.ExecuteDataSet(dbCommand).Tables[0];
            return dt;
        }
        public static DataTable GetBankAccList(string dCode, string bankCode)
        {
            if (Test)
            {
                var rd = new Random();
                DataTable t = new DataTable();
                t.Columns.Add("ACCOUNT_CODE");
                t.Columns.Add("ACCOUNT_HOLDER");
                var r = t.NewRow();
                r["ACCOUNT_CODE"] = rd.Next(23423523,43435234).ToString(); r["ACCOUNT_HOLDER"] = "VCB1"; t.Rows.Add(r);
                r = t.NewRow();
                r["ACCOUNT_CODE"] = rd.Next(43423523, 93435234).ToString(); r["ACCOUNT_HOLDER"] = "ACB1"; t.Rows.Add(r);

                return t;
            }
            
            var db = DatabaseFactory.CreateDatabase();
            string sqlCommand = string.Format("select distinct ACCOUNT_CODE, '' as ACCOUNT_HOLDER from VIEW_BANK_FOR_DEALER where BANK_CODE = '{0}'", bankCode);
            if (!string.IsNullOrEmpty(dCode)) sqlCommand = sqlCommand + string.Format(" and DEALER_CODE = '{0}'", dCode);

            var dbCommand = db.GetSqlStringCommand(sqlCommand);
            var dt = db.ExecuteDataSet(dbCommand).Tables[0];
            return dt;
        }
    }
}
