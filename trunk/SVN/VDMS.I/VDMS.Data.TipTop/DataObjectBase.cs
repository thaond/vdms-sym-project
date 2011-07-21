using System.Data;
using System.Data.Common;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace VDMS.Data.TipTop
{
    public class DataObjectBase
    {
        public static DataSet ExecuteSql(string sql)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            return db.ExecuteDataSet(dbCommand);
        }

        public static T ExecuteScalarSql<T>(string sql)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            object res = db.ExecuteScalar(dbCommand);
            return (T)res;
        }

        internal static string BuildViewName(string viewname)
        {
            var DatabaseCode = (string)HttpContext.Current.Session["CurrentUser.DatabaseCode"];
            return string.Format("view_{0}_{1}", DatabaseCode, viewname).ToLower();
        }
        #region Auto
        internal static string BuildViewName2(string viewname,string databasecode)
        {
            //var DatabaseCode = (string)HttpContext.Current.Session["CurrentUser.DatabaseCode2"];
            return string.Format("view_{0}_{1}", databasecode, viewname).ToLower();
        }
        #endregion

        internal static string BuildViewName(string viewname, int Version)
        {
            var s = BuildViewName(viewname);
            if (Version == 2) s = s.Replace("_dnf_", "_dnp_").Replace("_htf_", "_htp_");
            return s;
        }

        internal static string BuildViewName(string viewname, bool revert)
        {
            if (!revert) return BuildViewName(viewname);
            var DatabaseCode = (string)HttpContext.Current.Session["CurrentUser.DatabaseCode"];
            if (string.IsNullOrEmpty(DatabaseCode)) DatabaseCode = "dnf"; // test only
            return string.Format("view_{0}_{1}", DatabaseCode.ToLower() == "dnf" ? "htf" : "dnf", viewname);
        }

        //protected static string DealerCode
        //{
        //    get
        //    {
        //        string DealerCode = (string)HttpContext.Current.Profile.GetPropertyValue("DealerCode");
        //        if (HttpContext.Current.User.IsInRole("Administrator"))
        //        {
        //            string AdminDealerCode = (string)HttpContext.Current.Session["AdminDealerCode"];
        //            if (AdminDealerCode == null) return string.Empty;
        //            return AdminDealerCode;
        //        }
        //        if (string.IsNullOrEmpty(DealerCode)) DealerCode = "BN002A"; // test only
        //        return DealerCode;
        //    }
        //}
    }
}
