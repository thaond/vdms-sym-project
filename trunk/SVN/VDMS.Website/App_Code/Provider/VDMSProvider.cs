using System.Data;
using System.Web;
using System.Web.Security;
using System.Xml;
using VDMS.II.Common.Utils;
using System.Configuration;

namespace VDMS.Provider
{
    public class VDMSProvider
    {
        #region Constants

        public const string DefaultMemberShipProvider = "AspNetOracleMembershipProvider";
        public const string MemberShipProviderNameFormat = DefaultMemberShipProvider + ".{0}";

        public const string DefaultRoleProvider = "AspNetOracleRoleProvider";
        public const string RoleProviderNameFormat = DefaultRoleProvider + ".{0}";

        public const string DefaultSiteMapProvider = "OracleSiteMapProvider";
        public const string SiteMapProviderNameFormat = DefaultSiteMapProvider + "{0}";

        public const string DefaultProfileProvider = "AspNetOracleProfileProvider";
        public const string ProfileProviderNameFormat = DefaultProfileProvider + ".{0}";

        #endregion

        #region Properties

        public static string OrgCode
        {
            get
            {
                if (HttpContext.Current.Session == null) return "/";
                string s = (string)HttpContext.Current.Session["VDMSSecurityProvider.OrganizationCode"];
                if (HttpContext.Current.Session[HttpContext.Current.Session.SessionID] == null)
                {
                    HttpContext.Current.Session.Abandon();
                    FormsAuthentication.SignOut();
                    HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl + "?ReturnUrl=" + HttpContext.Current.Request.Url.PathAndQuery);
                }
                if (string.IsNullOrEmpty(s)) return "/";
                return s;
            }
            set
            {
                string val = value.Trim().ToUpper();
                if (string.IsNullOrEmpty(val)) val = "/";
                HttpContext.Current.Session["VDMSSecurityProvider.OrganizationCode"] = val;
                HttpContext.Current.Session["CurrentUser.DealerCode"] = val;
                HttpContext.Current.Session[HttpContext.Current.Session.SessionID] = val;
            }
        }

        public static string Language
        {
            get
            {
                object sess = (HttpContext.Current.Session != null) ? HttpContext.Current.Session["VDMSSecurityProvider.Language"] : null;
                return (sess == null) ? string.Empty : sess.ToString();
            }
            set
            {
                HttpContext.Current.Session["VDMSSecurityProvider.Language"] = value.Trim();
            }
        }

        public static bool IsDebugEnabled
        {
            get
            {
				//ConfigFileManager webConfig = VDMSProvider.GetWebConfigFileManager("wc");
				//webConfig.CurrentNodePath = "/wc:configuration/wc:system.web/wc:compilation";
				//return bool.Parse(webConfig.GetAttribute("debug"));
				return ConfigurationManager.AppSettings["debug"] == "true";
            }
        }

        public static int? CrystalJobsLimit
        {
            get
            {
                var jl = ConfigurationManager.AppSettings["CrystalJobsLimit"];
                return string.IsNullOrEmpty(jl) ? (int?)null : int.Parse(jl);
            }
        }

        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["VDMS"].ConnectionString;
            }
        }

        #endregion

        #region Statics methods

		//protected static ConfigFileManager GetWebConfigFileManager(string nsAlias)
		//{
		//    var webConfig = new ConfigFileManager(ConfigFileType.WebConfig);
		//    webConfig.AddNamespace(nsAlias, "http://schemas.microsoft.com/.NetConfiguration/v2.0");
		//    webConfig.CurrentNSPrefix = nsAlias;
		//    return webConfig;
		//}


		//public static void AddMemberShipProvider(string appName)
		//{
		//    var webConfig = VDMSProvider.GetWebConfigFileManager("wc");
		//    webConfig.CurrentNodePath = "/wc:configuration/wc:system.web/wc:membership/wc:providers";

		//    var prvdName = string.Format(VDMSProvider.MemberShipProviderNameFormat, appName);
		//    var prvd = webConfig.GetElement("wc:add", "name", prvdName);
		//    if (prvd == null)
		//    {
		//        var defaultProvider = webConfig.GetElement("wc:add", "name", VDMSProvider.DefaultMemberShipProvider);
		//        prvd = (XmlElement)defaultProvider.Clone();
		//        prvd.SetAttribute("name", prvdName);
		//        prvd.SetAttribute("applicationName", appName);
		//        webConfig.AddElement(prvd);
		//        webConfig.SaveConfigDoc();
		//    }
		//}

        public static string GetMemberShipProviderName(string appName)
        {
            appName = appName.Replace("/", "");
            return (string.IsNullOrEmpty(appName)) ? VDMSProvider.DefaultMemberShipProvider : string.Format(VDMSProvider.MemberShipProviderNameFormat, appName);
        }

        public static string GetRoleProviderName(string appName)
        {
            appName = appName.Replace("/", "");
            return (string.IsNullOrEmpty(appName)) ? VDMSProvider.DefaultRoleProvider : string.Format(VDMSProvider.RoleProviderNameFormat, appName);
        }

        public static string GetProfileProviderName(string appName)
        {
            appName = appName.Replace("/", "");
            return (string.IsNullOrEmpty(appName)) ? VDMSProvider.DefaultProfileProvider : string.Format(VDMSProvider.ProfileProviderNameFormat, appName);
        }

        public static string GetSiteMapProviderName(string appName)
        {
            appName = appName.Replace("/", "");
            return (string.IsNullOrEmpty(appName)) ? VDMSProvider.DefaultSiteMapProvider : string.Format(VDMSProvider.SiteMapProviderNameFormat, appName);
        }

		//public static DataTable GetFakeDataList(string[] Field)
		//{
		//    DataTable tbl = new DataTable();
		//    foreach (string col in Field)
		//    {
		//        tbl.Columns.Add(col);
		//    }
		//    for (int i = 0; i < 15; i++)
		//    {
		//        DataRow row = tbl.NewRow();
		//        foreach (string col in Field)
		//        {
		//            row[col] = col + tbl.Rows.Count.ToString();
		//        }
		//        tbl.Rows.Add(row);
		//    }

		//    return tbl;
		//}

		//public static DataTable GetFakeDataList(string[] Field, string[] samples)
		//{
		//    DataTable tbl = new DataTable();
		//    foreach (string col in Field)
		//    {
		//        tbl.Columns.Add(col);
		//    }
		//    for (int i = 0; i < 15; i++)
		//    {
		//        DataRow row = tbl.NewRow();
		//        for (int j = 0; j < Field.Length; j++)
		//        {
		//            row[Field[j]] = string.Format(samples[j], tbl.Rows.Count.ToString());
		//        }
		//        tbl.Rows.Add(row);
		//    }

		//    return tbl;
		//}

        #endregion
    }
}
