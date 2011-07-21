using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using VDMS.II.Entity;
using VDMS.II.Linq;

namespace VDMS.II.Security
{
    public class PermissionDAO
    {
        public static void SaveChanged()
        {
            DCFactory.GetDataContext<SecurityDataContext>().SubmitChanges();
        }

        // role in task 
        public static void DeleteAllRoleTask(string app, string role, string url)
        {
            var dc = DCFactory.GetDataContext<SecurityDataContext>();
            dc.RoleInTasks.DeleteAllOnSubmit(GetRoleTasks(app, role, url));
        }

        public static List<RoleInTask> GetRoleTasks(string app, string role, string url)
        {
            if (string.IsNullOrEmpty(app)) app = "/";
            var dc = DCFactory.GetDataContext<SecurityDataContext>();
            return dc.RoleInTasks.Where(r => r.ApplicationName.ToUpper() == app.Trim().ToUpper() &&
                                             r.RoleName.ToUpper() == role.Trim().ToUpper() &&
                                             r.Task.SiteMap.Url.ToUpper() == url.Trim().ToUpper()).ToList();
        }

        public static RoleInTask GetRoleTask(string app, string role, string url, string cmd)
        {
            if (string.IsNullOrEmpty(app)) app = "/";
            var task = GetTask(url, cmd);
            if (task == null) return null;

            if (!task.RoleInTasks.HasLoadedOrAssignedValues) task.RoleInTasks.Load();
            return task.RoleInTasks.SingleOrDefault(r => r.ApplicationName.ToUpper() == app.Trim().ToUpper() &&
                                                       r.RoleName.ToUpper() == role.Trim().ToUpper());
        }

        public static RoleInTask CreateOrUpdateRoleTask(string app, string role, string url, string cmd)
        {
            bool exist = true;
            if (string.IsNullOrEmpty(app)) app = "/";

            var dc = DCFactory.GetDataContext<SecurityDataContext>();
            var task = GetTask(url, cmd);
            if (task == null)
            {
                throw new Exception(string.Format("Task for {0} does not exist!", cmd));
            }

            var rt = GetRoleTask(app, role, url, cmd);
            if (rt == null)
            {
                rt = new RoleInTask();
                exist = false;
            }
            rt.TaskId = task.TaskId;
            rt.ApplicationName = app.Trim();
            rt.RoleName = role;
            if (!exist) dc.RoleInTasks.InsertOnSubmit(rt);

            return rt;
        }

        // task
        public static List<Task> GetTasks(string url)
        {
            if (url == null) url = "";
            var dc = DCFactory.GetDataContext<SecurityDataContext>();
            return dc.Tasks.Where(t => t.SiteMap.Url.ToLower() == url.ToLower()).ToList();
        }

        public static Task GetTask(string url, string cmd)
        {
            if (url == null) url = "";
            if (cmd == null) cmd = "";
            var dc = DCFactory.GetDataContext<SecurityDataContext>();
            return dc.Tasks.SingleOrDefault(t => t.SiteMap.Url.ToLower() == url.ToLower() && t.CommandName.ToLower() == cmd.ToLower());
        }

        public static bool DeleteTask(string url, string cmd)
        {
            var item = GetTask(url, cmd);
            if (item == null) return false;

            var dc = DCFactory.GetDataContext<SecurityDataContext>();
            if (item.RoleInTasks.Count > 0)
            {
                dc.RoleInTasks.DeleteAllOnSubmit(item.RoleInTasks);
            }
            dc.Tasks.DeleteOnSubmit(item);
            return true;
        }

        public static void DeleteTask(long TaskId)
        {
            var dc = DCFactory.GetDataContext<SecurityDataContext>();
            var task = dc.Tasks.SingleOrDefault(t => t.TaskId == TaskId);
            if (task == null) return;
            dc.RoleInTasks.DeleteAllOnSubmit(task.RoleInTasks);
            dc.Tasks.DeleteOnSubmit(task);
            dc.SubmitChanges();
        }

        public static Task CreateOrUpdateTask(string cmd, string taskName, string url)
        {
            var path = GetSitePath(url);
            if (path == null) throw new Exception("Sitemap path does not exist!");

            var task = GetTask(url, cmd);

            if (task == null)
            {
                var dc = DCFactory.GetDataContext<SecurityDataContext>();
                task = new Task();
                task.PathId = path.PathId;
                dc.Tasks.InsertOnSubmit(task);
            }
            task.CommandName = cmd.Trim();
            task.TaskName = taskName.Trim();

            return task;
        }

        // role in path
        public static void DeleteAllRolePath(string app, string role)
        {
            var dc = DCFactory.GetDataContext<SecurityDataContext>();
            dc.RoleInPaths.DeleteAllOnSubmit(GetRolePaths(app, role));
        }

        public static List<RoleInPath> GetRolePaths(string app, string role)
        {
            if (string.IsNullOrEmpty(app)) app = "/";
            var dc = DCFactory.GetDataContext<SecurityDataContext>();
            return dc.RoleInPaths.Where(r => r.ApplicationName.ToUpper() == app.Trim().ToUpper() &&
                                           r.RoleName.ToUpper() == role.Trim().ToUpper()).ToList();
        }

        public static RoleInPath GetRolePath(string app, string role, string url)
        {
            if (string.IsNullOrEmpty(app)) app = "/";

            var path = GetSitePath(url);
            if (path == null) return null;
            if (!path.RoleInPaths.HasLoadedOrAssignedValues) path.RoleInPaths.Load();
            return path.RoleInPaths.SingleOrDefault(r => r.ApplicationName.ToUpper() == app.Trim().ToUpper() &&
                                                       r.RoleName.ToUpper() == role.Trim().ToUpper());
        }

        public static bool IsAnyRoleInPath(string app, string[] roles, string url)
        {
            if (string.IsNullOrEmpty(app)) app = "/";

            var path = GetSitePath(url);
            if (path == null) return false;

            for (int i = 0; i < roles.Length; i++)
            {
                roles[i] = roles[i].ToLower();
            }

            if (!path.RoleInPaths.HasLoadedOrAssignedValues) path.RoleInPaths.Load();
            return path.RoleInPaths.Where(r => roles.Contains(r.RoleName.ToLower()) && r.ApplicationName.ToUpper() == app.Trim().ToUpper()).Count() > 0;
        }

        public static RoleInPath CreateOrUpdateRolePath(string app, string role, string url)
        {
            if (string.IsNullOrEmpty(app)) app = "/";

            var dc = DCFactory.GetDataContext<SecurityDataContext>();
            var path = GetSitePath(url);
            if (path == null)
            {
                throw new Exception("Sitemap path does not exist!");
            }

            var rp = GetRolePath(app, role, url);
            if (rp == null) rp = new RoleInPath();
            rp.ApplicationName = app.Trim();
            rp.RoleName = role;
            rp.PathId = path.PathId;
            //if (!exist) 
            dc.RoleInPaths.InsertOnSubmit(rp);

            return rp;
        }

        public static void SaveRolePath(string app, string role, TreeNode node)
        {
            if ((!string.IsNullOrEmpty(node.Value)) && (node.Checked)) CreateOrUpdateRolePath(app, role, node.Value);
            foreach (TreeNode item in node.ChildNodes)
            {
                SaveRolePath(app, role, item);
            }
        }

        // site map
        public static void SaveSitePath(TreeNode node)
        {
            if ((!string.IsNullOrEmpty(node.Value))) CreateOrUpdateSitePath(node.Value);
            foreach (TreeNode item in node.ChildNodes)
            {
                SaveSitePath(item);
            }
        }

        public static void CreateOrUpdateSitePath(string p)
        {
            bool exist = true;
            var dc = DCFactory.GetDataContext<SecurityDataContext>();
            var path = GetSitePath(p);

            if (path == null)
            {
                path = new SiteMap();
                exist = false;
            }
            path.Url = p.Trim();

            if (!exist) dc.SiteMaps.InsertOnSubmit(path);
        }

        public static VDMS.II.Entity.SiteMap GetSitePath(string url)
        {
            var dc = DCFactory.GetDataContext<SecurityDataContext>();
            return dc.SiteMaps.Where(p => p.Url.ToUpper() == url.Trim().ToUpper()).First();
        }
    }
}