using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Security;
using VDMS.Provider;

namespace VDMS.II.Security
{
    public class MembershipDAO
    {
        private int count;
        static string InvalidAppMsg = "Invalid Organization Code! Declaration maybe missing in webconfig.";

        public static MembershipUser AddNewUser(string app, string username, string password, string email, out MembershipCreateStatus createStatus)
        {
            createStatus = MembershipCreateStatus.ProviderError;
            MembershipProvider prv = VDMSMembershipProvider.GetProvider(app);
            if (prv != null)
            {
                return prv.CreateUser(username, password, email, "Quest", "VDMSII", true, Guid.NewGuid(), out createStatus);
            }
            else throw new Exception(InvalidAppMsg);
        }

        public static MembershipCreateStatus AddNewUser(string app, string username, string password, string email)
        {
            MembershipCreateStatus status;
            MembershipDAO.AddNewUser(app, username, password, email, out status);
            return status;
        }

        public static MembershipUser GetUser(string app, string username)
        {
            if (app == null) app = string.Empty;
            MembershipProvider prv = VDMSMembershipProvider.GetProvider(app);
            if (prv == null) return null;
            return prv.GetUser(username, false);
        }

        public static void UpdateUser(string app, MembershipUser user)
        {
            MembershipProvider prv = VDMSMembershipProvider.GetProvider(app);
            if (prv != null)
            {
                prv.UpdateUser(user);
            }
            else throw new Exception(InvalidAppMsg);
        }

        /// <summary>
        /// Checking App existed
        /// Return false if not esxit
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static bool CheckingApp(string app)
        {
            if (!string.IsNullOrEmpty(app))
            {
                MembershipProvider prv = VDMSMembershipProvider.GetProvider(app);
                if (prv == null) return false;
                else return true;
            }
            return false;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<MembershipUser> FindAll(int maximumRows, int startRowIndex, string app, string filter)
        {
            if (CheckingApp(app))
            {
                if (app == null) app = string.Empty;
                if (filter == null) filter = string.Empty;
                filter = "%" + filter + "%";
                MembershipProvider prv = VDMSMembershipProvider.GetProvider(app);
                //if (prv == null) throw new Exception(InvalidAppMsg);

                // cho nay ko page dc, luc nao cung return full list
                //MembershipUserCollection users = prv.FindUsersByName(filter, startRowIndex / maximumRows, maximumRows, out count);
                MembershipUserCollection users = prv.FindUsersByName(filter, 0, int.MaxValue, out count);
                List<MembershipUser> res = users.Cast<MembershipUser>().OrderBy(p => p.UserName).ToList();
                return res.Skip(startRowIndex).Take(maximumRows);
            }
            else
                return null;
        }

        public int GetCount(string app, string filter)
        {
            return count;
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public void Delete(string app, string UserName)
        {
            if (app == null) app = string.Empty;
            MembershipProvider prv = VDMSMembershipProvider.GetProvider(app);
            prv.DeleteUser(UserName, true);
        }
    }
}