using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.II.Linq;
using VDMS.Helper;

namespace VDMS.II.Security
{
    public class Security
    {
        public static bool IsDeniedURL(string url, string app, string role)
        {
            return PermissionDAO.GetRolePath(app, role, url) == null;
        }

        public static bool IsDeniedURL(string url, string app)
        {
            return IsDeniedURLWithProvider(url, app, null);
        }
        public static bool IsDeniedURLWithProvider(string url, string app, SiteMapProvider smProvider)
        {
            if (url.ToLower().EndsWith("accessdeny.aspx") || url.ToLower().EndsWith("changepassword.aspx") || url.ToLower().EndsWith("default.aspx") || url.ToLower().EndsWith("profile.aspx")) return false;
            //if (UserHelper.Username == "admin" || UserHelper.IsAdmin) return false;
            if (UserHelper.IsSysAdmin) return false;
            if ((smProvider != null) && (smProvider.CurrentNode == null)) return true;

            //foreach (string role in RoleDAO.GetRolesForUser(app, UserHelper.Username))
            //    if (IsDeniedURL(url, app, role)) return true;
            return !PermissionDAO.IsAnyRoleInPath(app, RoleDAO.GetRolesForUser(app, UserHelper.Username), url);
        }

        public static bool IsDeniedTask(string url, string app, string role, string cmd)
        {
            // ko co task => ok
            VDMS.II.Entity.Task task = PermissionDAO.GetTask(url, cmd);
            if (task == null) return false;
            var rt = PermissionDAO.GetRoleTask(app, role, url, cmd);
            return rt == null;
        }

        public static bool IsDeniedTask(string url, string cmd)
        {
            if (UserHelper.IsSysAdmin) return false;
            var app = UserHelper.ApplicationName;

            bool res = true;
            string[] roles = RoleDAO.GetRolesForUser(app, UserHelper.Username);
            foreach (string role in roles)
                if (!IsDeniedTask(url, app, role, cmd)) { res = false; break; }

            return res;
        }
    }
}