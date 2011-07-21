using System.Linq;
using VDMS.II.Linq;
using VDMS.II.Entity;
using VDMS.Helper;
using VDMS.II.BasicData;

namespace VDMS.II.Security
{
    public class ProfileDAO
    {
        /// <summary>
        /// Get list of employy which below the current Position
        /// </summary>
        /// <param name="Position"></param>
        /// <param name="Dept"></param>
        /// <param name="DatabaseCode"></param>
        /// <param name="AreaCode"></param>
        /// <returns></returns>
        public static IQueryable<string> GetEmployee(string Position, string Dept, string DatabaseCode)
        {
            var db = DCFactory.GetDataContext<SecurityDataContext>();
            var query = db.UserProfiles.Where(p => p.Dept == Dept && p.DatabaseCode == DatabaseCode);
            switch (Position)
            {
                case "M":
                    query = query.Where(p => p.Position == "E");
                    break;
                case "S":
                    query = query.Where(p => p.Position == "E" || p.Position == "M");
                    break;
                default:
                    return null;
            }
            return query.Select(p => p.UserName);
        }

        public static IQueryable<string> GetUsernameByProfile(string Position, string Dept, string DatabaseCode, string AreaCode)
        {
            var db = DCFactory.GetDataContext<SecurityDataContext>();
            return db.UserProfiles.Where(p => p.Position == Position && p.Dept == Dept && p.DatabaseCode == DatabaseCode && p.AreaCode == AreaCode).Select(p => p.UserName.ToUpper());
        }

        public static IQueryable<string> GetUsernameByProfile(string Position, string Dept)
        {
            var db = DCFactory.GetDataContext<SecurityDataContext>();
            return db.UserProfiles.Where(p => p.Position == Position && p.Dept == Dept).Select(p => p.UserName.ToUpper());
        }

        public static IQueryable<string> GetUsernameByProfile(string Position, string Dept, int NGLevel)
        {
            var db = DCFactory.GetDataContext<SecurityDataContext>();
            if (!string.IsNullOrEmpty(Position)) return db.UserProfiles.Where(p => p.Position == Position && p.Dept == Dept && p.NGLevel == NGLevel).Select(p => p.UserName.ToUpper());
            return db.UserProfiles.Where(p => p.Dept == Dept && p.NGLevel == NGLevel).Select(p => p.UserName.ToUpper());
        }

        public static void Delete(string Username, string DealerCode)
        {
            var db = DCFactory.GetDataContext<SecurityDataContext>();
            var pf = db.UserProfiles.SingleOrDefault(p => p.UserName.ToUpper() == Username.ToUpper() && p.DealerCode == DealerCode);
            if (pf != null)
            {
                db.UserProfiles.DeleteOnSubmit(pf);
                db.SubmitChanges();
            }
        }

        public static UserProfile GetProfile(string Username, string DealerCode)
        {
            var db = DCFactory.GetDataContext<SecurityDataContext>();
            return db.UserProfiles.SingleOrDefault(p => p.UserName.ToUpper() == Username.ToUpper() && p.DealerCode == DealerCode);
        }

        public static UserProfile CreateDefaultProfile()
        {
            var d = new UserProfile
            {
                UserName = UserHelper.Username,
                DealerCode = UserHelper.DealerCode
            };
            if (UserHelper.IsDealer)
            {
                var vw = WarehouseDAO.GetWarehouses(UserHelper.DealerCode, WarehouseType.Vehicle);
                var pw = WarehouseDAO.GetWarehouses(UserHelper.DealerCode, WarehouseType.Part);
                d.WarehouseId = UserHelper.Dealer.DefaultWarehouseId > 0 ? UserHelper.Dealer.DefaultWarehouseId : (pw.Count > 0 ? (long?)pw[0].WarehouseId : null);
                d.VWarehouseId = UserHelper.Dealer.DefaultVWarehouseId > 0 ? UserHelper.Dealer.DefaultVWarehouseId : (vw.Count > 0 ? (long?)vw[0].WarehouseId : null);
            }
            else
            {
                d.DatabaseCode = "DNF";
                d.AreaCode = "HC";
                d.NGLevel = 0;
                d.Dept = "SP";
                d.Position = "E";
            }
            //var db = new SecurityDataContext();
            //db.UserProfiles.InsertOnSubmit(d);
            //db.SubmitChanges();
            //db.Dispose();
            return d;
        }
    }
}