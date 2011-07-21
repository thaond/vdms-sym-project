using System;
using System.Linq;
using System.Globalization;
using System.Web;
using System.Web.Security;
using VDMS.II.BasicData;
using VDMS.Provider;
using VDMS.II.Linq;
using VDMS.II.Entity;
using VDMS.II.Security;

namespace VDMS.Helper
{
    public class UserHelper
    {
        public static Guid UserId
        {
            get
            {
                return (Guid)Membership.GetUser().ProviderUserKey;
            }
        }

        public static string Username
        {
            get
            {
                if (HttpContext.Current == null) return "System";
                string name = HttpContext.Current.User.Identity.Name;
                if (string.IsNullOrEmpty(name)) name = "Anonymous";
                return name.ToUpper();
            }
        }

        public static bool IsInRole(string role)
        {
            return HttpContext.Current.User.IsInRole(role);
        }

        public static bool IsDealer
        {
            get
            {
                return UserHelper.DealerCode != "/";
            }
        }

        public static bool IsAdmin
        {
            get
            {
                return IsInRole("Administrators");
            }
        }

        public static bool IsVMEPService
        {
            get
            {
                return VDMSProvider.OrgCode == "/" && UserHelper.IsInRole("Service");
            }
        }

        public static bool IsSysAdmin
        {
            get
            {
                return VDMSProvider.OrgCode == "/" && UserHelper.IsInRole("Administrators");
            }
        }

        public static void SetSessionDealerCode(string dealerCode)
        {
            try
            {
                var s = dealerCode.Trim().ToUpper();
                if (string.IsNullOrEmpty(s)) s = "/";

                var db = DCFactory.GetDataContext<PartDataContext>();
                HttpContext.Current.Session["CurrentUser.DealerCode"] = s;
                //HttpContext.Current.Session["AdminWarehouseId"] = db.Warehouses.First(p => p.DealerCode == s && p.Type == "P").WarehouseId;
                HttpContext.Current.Session["CurrentUser.Dealer"] = db.Dealers.Single(p => p.DealerCode == dealerCode);
                HttpContext.Current.Session["CurrentUser.DatabaseCode"] = Dealer.DatabaseCode;
            }
            catch
            {
                HttpContext.Current.Session["CurrentUser.DealerCode"] = null;
            }
        }

        public static void SetSessionWarehouseId(long WarehouseId)
        {
            HttpContext.Current.Session["WarehouseId"] = WarehouseId;
        }
        public static void SetSessionVehicleWarehouseId(long WarehouseId)
        {
            HttpContext.Current.Session["CurrentUser.VehicleBranch"] = null;
            HttpContext.Current.Session["VehicleWarehouseId"] = WarehouseId;
        }

        public static string FullBranchCode
        {
            get { return string.Format("{0}-{1}", DealerCode, BranchCode); }
        }

        public static string BranchCode
        {
            get
            {
                var d = (Warehouse)HttpContext.Current.Session["CurrentUser.VehicleBranch"];
                if (d == null)
                {
                    long wid = VehicleWarehouseId;
                    if (wid > 0) d = (Warehouse)HttpContext.Current.Session["CurrentUser.VehicleBranch"];
                }
                return (d != null) ? d.Code : "";
            }
        }

        public static string DealerCode
        {
            get
            {
                if (IsSysAdmin)
                {
                    object code = HttpContext.Current.Session["CurrentUser.DealerCode"];
                    return (code == null) ? "/" : code.ToString();
                }
                return VDMSProvider.OrgCode;
            }
        }

        public static string ApplicationName
        {
            get
            {
                string name = VDMSProvider.OrgCode;
                return (string.IsNullOrEmpty(name)) ? "/" : name;
            }
        }

        public static Dealer Dealer
        {
            get
            {
                var d = (Dealer)HttpContext.Current.Session["CurrentUser.Dealer"];
                if (d == null) d = DealerDAO.GetDealerByCode(DealerCode);
                if (d == null) d = new VDMS.II.Entity.Dealer() { DealerName = "VMEP" };
                HttpContext.Current.Session["CurrentUser.Dealer"] = d;
                return d;
            }
        }

        public static UserProfile Profile
        {
            get
            {
                var d = (UserProfile)HttpContext.Current.Session["CurrentUser.Profile"];
                if (d == null)
                {
                    using (var db = new SecurityDataContext())
                    {
                        d = db.UserProfiles.FirstOrDefault(p => p.UserName.ToUpper() == Username.ToUpper() && p.DealerCode.ToUpper() == DealerCode.ToUpper());
                        if (d == null)
                        {
                            HttpContext.Current.Session["CurrentUser.Profile"] = null;
                            d = ProfileDAO.CreateDefaultProfile();
                            db.UserProfiles.InsertOnSubmit(d);
                            db.SubmitChanges();
                        }
                        else
                            HttpContext.Current.Session["CurrentUser.Profile"] = d;
                        //d.PropertyChanging += new System.ComponentModel.PropertyChangingEventHandler(d_PropertyChanging);
                    }
                }
                return d;
            }
        }

        static void d_PropertyChanging(object sender, System.ComponentModel.PropertyChangingEventArgs e)
        {
        }

        public static long WarehouseId
        {
            get
            {
                //if (IsSysAdmin)
                //{
                //    try
                //    {
                //        return (long)HttpContext.Current.Session["AdminWarehouseId"];
                //    }
                //    catch
                //    {
                //        return 0;
                //    }
                //}
                if (!IsAdmin || HttpContext.Current.Session["WarehouseId"] == null) return ProfileWarehouseId;
                return (long)HttpContext.Current.Session["WarehouseId"];
            }
        }
        public static long VehicleWarehouseId
        {
            get
            {
                var d = (Warehouse)HttpContext.Current.Session["CurrentUser.VehicleBranch"];
                if (d == null)
                {
                    object wid = null;
                    if (IsSysAdmin || IsAdmin)
                    {
                        wid = HttpContext.Current.Session["VehicleWarehouseId"];
                    }
                    if (wid == null) wid = Profile.VWarehouseId;
                    if (wid != null) d = WarehouseDAO.GetWarehouse((long)wid, WarehouseType.Vehicle);

                    HttpContext.Current.Session["CurrentUser.VehicleBranch"] = d;
                    return (wid == null) ? 0 : (long)wid;
                }
                else return d.WarehouseId;
            }
        }

        public static long ProfileWarehouseId
        {
            get
            {
                if (Profile == null) return 0;
                return Profile.WarehouseId.HasValue ? Profile.WarehouseId.Value : 0;
            }
        }
        public static string ProfileVBranch
        {
            get
            {
                var brch = (string)HttpContext.Current.Session["CurrentUser.ProfileVehicleBranch"];
                if (brch == null)
                {
                    brch = "";
                    if ((Profile != null) && Profile.VWarehouseId.HasValue)
                    {
                        Warehouse w = WarehouseDAO.GetWarehouse((long)Profile.VWarehouseId, WarehouseType.Vehicle);
                        if (w != null) brch = w.Code;
                    }
                    HttpContext.Current.Session["CurrentUser.ProfileVehicleBranch"] = brch;
                }
                return brch;
            }
        }

        public static string DealerName
        {
            get
            {
                return Dealer.DealerName;
            }
        }

        public static string DatabaseCode
        {
            get
            {
                //if (IsDealer && !IsAdmin) return Dealer.DatabaseCode;
                //ProfileCommon profile = (ProfileCommon)HttpContext.Current.Profile;
                //if (string.IsNullOrEmpty(profile.DatabaseCode)) return "DNF";
                //return profile.DatabaseCode;
                //if (IsSysAdmin) return string.Empty;
                if (!IsDealer) return Profile.DatabaseCode;
                return Dealer.DatabaseCode;
            }
        }

        public static string AreaCode
        {
            get
            {
                //ProfileCommon profile = (ProfileCommon)HttpContext.Current.Profile;
                //if (string.IsNullOrEmpty(profile.AreaCode)) return "AC";
                //return (HttpContext.Current.Profile as ProfileCommon).AreaCode;
                if (IsDealer) return Dealer.AreaCode;
                return Profile.AreaCode;
            }
        }

        public static string Fullname
        {
            get
            {
                //return (HttpContext.Current.Profile as ProfileCommon).Fullname;
                return Profile.FullName;
            }
        }

        public static string Language
        {
            get
            {
                return VDMSProvider.Language;
            }
        }

        public static bool IsVietnamLanguage
        {
            get
            {
                return VDMSProvider.Language == "vi-VN";
            }
        }

        public static DateTime ParseDate(string InputDate)
        {
            DateTime d1 = DateTime.MinValue;
            DateTime.TryParse(InputDate, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out d1);
            return d1;
        }

        public static DateTime ParseDate(string InputDate, bool IncludeTime)
        {
            DateTime d1;
            DateTime.TryParse(InputDate, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out d1);
            if (IncludeTime) d1 = d1.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
            return d1;
        }

        public static string BuildFullBranch(string bCode)
        {
            return BuildFullBranch(DealerCode, bCode);
        }
        public static string BuildFullBranch(string dCode, string bCode)
        {
            return string.Format("{0}-{1}", dCode, bCode);
        }
        public static string ExtractBranch(string fullBrach)
        {
            string[] bs = fullBrach.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            return bs.Length > 1 ? bs[1] : bs[0];
        }
    }
}