using System.Web;
using VDMS.Helper;
using VDMS.II.Security;
using System.Collections.Generic;
using System.Linq;
namespace VDMS.Provider
{
    public class VDMSSiteMapProvider : XmlSiteMapProvider
    {
        //public List<UrlUser> lu = new List<UrlUser>();
        public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
        {
            if (node.Description.ToLower() == "invisible") return false;

            if (node.Key.ToLower() == "/default.aspx") return true;
            if (UserHelper.IsSysAdmin || string.IsNullOrEmpty(node.Url)) return true;


            List<UrlUser> lu = (List<UrlUser>)HttpContext.Current.Session["IsAccessibleToUser"];
            if (lu == null)
            {
                lu = new List<UrlUser>();
                bool res = !Security.IsDeniedURL(node.Url, UserHelper.DealerCode);
                lu.Add(new UrlUser { res = res, Url = node.Url, DealerCode = UserHelper.DealerCode });
                HttpContext.Current.Session["IsAccessibleToUser"] = lu;
                return res;
            }
            else
            {
                var q = lu.Where(l => l.Url == node.Url && l.DealerCode == UserHelper.DealerCode);
                UrlUser uu = new UrlUser();
                if (q.Count() > 0)
                    uu = q.First();
                else
                    uu = null;
                if (uu != null)
                    return uu.res;
                else
                {
                    bool res = !Security.IsDeniedURL(node.Url, UserHelper.DealerCode);
                    lu.Add(new UrlUser { res = res, Url = node.Url, DealerCode = UserHelper.DealerCode });
                    HttpContext.Current.Session["IsAccessibleToUser"] = lu;
                    return res;
                }   
            }
        }
    }
    public class UrlUser
    {
        public string Url { get; set; }
        public string DealerCode { get; set; }
        public bool res { get; set; }
    }
}