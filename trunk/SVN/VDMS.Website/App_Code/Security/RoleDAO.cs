using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.Linq;
using VDMS.Provider;

namespace VDMS.II.Security
{
    public class RoleDAO
    {
        /// <summary>
        /// Count roles in application.
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public static int GetRolesCount(string appName)
        {
            if (string.IsNullOrEmpty(appName)) appName = "/";
            var sdc = DCFactory.GetDataContext<SecurityDataContext>();
            return sdc.Roles.Count(r => r.ApplicationName == appName);
        }

        /// <summary>
        /// Get all role in an application
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public static List<Role> GetRoles(string appName)
        {
            if (string.IsNullOrEmpty(appName)) appName = "/";
            var sdc = DCFactory.GetDataContext<SecurityDataContext>();
            var roles = from role in sdc.Roles where role.ApplicationName == appName select role;
            return roles.ToList();
        }

        /// <summary>
        /// Get top level role.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static Role GetRootRoles(string appName)
        {
            if (string.IsNullOrEmpty(appName)) appName = "/";
            var sdc = DCFactory.GetDataContext<SecurityDataContext>();
            return sdc.Roles.SingleOrDefault(r => (r.ParentRoleIndex == null) && (r.ApplicationName == appName));
        }

        /// <summary>
        /// Save changed to database.
        /// </summary>
        public static void SaveChanged()
        {
            DCFactory.GetDataContext<SecurityDataContext>().SubmitChanges();
        }

        /// <summary>
        /// Get role by name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public static Role GetRole(string name, string app)
        {
            if (string.IsNullOrEmpty(app)) app = "/";
            var sdc = DCFactory.GetDataContext<SecurityDataContext>();
            return sdc.Roles.SingleOrDefault(r => (r.RoleName == name) && (r.ApplicationName == app));
        }

        /// <summary>
        /// Get role by id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static Role GetRole(decimal roleId, string app)
        {
            if (string.IsNullOrEmpty(app)) app = "/";
            var sdc = DCFactory.GetDataContext<SecurityDataContext>();
            return sdc.Roles.SingleOrDefault(r => (r.RoleIndex == roleId) && (r.ApplicationName == app));
        }
        public static Role GetRole(decimal roleId)
        {
            var sdc = DCFactory.GetDataContext<SecurityDataContext>();
            return sdc.Roles.SingleOrDefault(r => (r.RoleIndex == roleId) && (r.ApplicationName == UserHelper.DealerCode));
        }

        /// <summary>
        /// Create new role and set parent role. Also save changed.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="name"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public static Role CreateRole(string app, string name, long parentID)
        {
            Role child = RoleDAO.GetRole(name, app);
            string tmpName = DateTime.Now.Ticks.ToString();

            if (child == null)
            {
                RoleDAO.AddNewRole(app, tmpName);
                child = RoleDAO.GetRole(tmpName, app);
            }

            if (child != null)
            {
                if (parentID > 0) child.ParentRoleIndex = parentID;
                child.RoleName = name;
                RoleDAO.SaveChanged();
            }
            child = RoleDAO.GetRole(name, app);

            try
            {
                // remove temp if other act failed
                RoleDAO.DeleteRole(app, tmpName);
            }
            catch { }

            return child;
        }

        /// <summary>
        /// Create administrators role for a dealer. Also save changed.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static Role CreateAdminRole(string app)
        {
            return RoleDAO.CreateRole(app, "Administrators", 0);
        }

        /// <summary>
        /// Add an user to specified Role of a dealer. Using provider.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="role"></param>
        /// <param name="user"></param>
        public static void AddUserToRole(string app, string role, string user)
        {
            RoleDAO.AddUsersToRoles(app, new string[] { user }, new string[] { role });
        }

        public static void AddUserToRoles(string app, string user, string[] roles)
        {
            RoleDAO.AddUsersToRoles(app, new string[] { user }, roles);
        }

        public static void AddUsersToRoles(string app, string[] userNames, string[] roleNames)
        {
            var prv = VDMSRoleProvider.GetProvider(app);
            if (prv != null) prv.AddUsersToRoles(userNames, roleNames);
            else throw new Exception("Invalid Organization Code!");
        }

        public static void RemoveUserFromRole(string app, string userName, string roleName)
        {
            RoleDAO.RemoveUsersFromRoles(app, new string[] { userName }, new string[] { roleName });
        }

        public static void RemoveUserFromRoles(string app, string userName, string[] roleNames)
        {
            RoleDAO.RemoveUsersFromRoles(app, new string[] { userName }, roleNames);
        }

        public static void RemoveUsersFromRoles(string app, string[] userNames, string[] roleNames)
        {
            var prv = VDMSRoleProvider.GetProvider(app);
            if (prv != null) prv.RemoveUsersFromRoles(userNames, roleNames);
            else throw new Exception("Invalid Organization Code!");
        }

        /// <summary>
        /// Delete an empty role(has no child role) even it has one or more users.
        /// Throw an exception if role does not empty.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool DeleteRole(string app, string name)
        {
            var role = GetRole(name, app);

            if (role != null)
            {
                var sdc = DCFactory.GetDataContext<SecurityDataContext>();
                if (!role.Roles.HasLoadedOrAssignedValues) role.Roles.Load();
                if (role.Roles.Count > 0) throw new Exception("Deleting role does not empty!");
                sdc.Roles.DeleteOnSubmit(role);
                sdc.SubmitChanges();
                //VDMSRoleProvider.GetProvider(app).DeleteRole(name, false);
            }
            return role != null;
        }

        /// <summary>
        /// Add new role use provider. 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="name"></param>
        public static void AddNewRole(string app, string name)
        {
            var prv = VDMSRoleProvider.GetProvider(app);
            if (prv != null) prv.CreateRole(name);
            else throw new Exception("Invalid Organization Code!");
        }

        /// <summary>
        /// Get users in role. Use provider.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static string[] GetUsersInRole(string app, string role)
        {
            var prv = VDMSRoleProvider.GetProvider(app);
            if (prv != null) return prv.GetUsersInRole(role);
            else throw new Exception("Invalid Organization Code!");
        }

        public static bool RoleExists(string app, string role)
        {
            var prv = VDMSRoleProvider.GetProvider(app);
            if (prv != null) return prv.RoleExists(role);
            else throw new Exception("Invalid Organization Code!");
        }

        public static string[] GetRolesForUser(string app, string userName)
        {
            var prv = VDMSRoleProvider.GetProvider(app);
            if (prv != null) return prv.GetRolesForUser(userName);
            else throw new Exception("Invalid Organization Code!");
        }
    }
}