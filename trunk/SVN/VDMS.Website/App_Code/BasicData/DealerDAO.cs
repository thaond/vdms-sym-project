using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.Linq;
using System.Web;
using System;

namespace VDMS.II.BasicData
{
   
    public class DealerDAO : SessionDealerDAO<DealerData>
    {
        public int GetCount()
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            return db.Dealers.Where(p => p.DealerCode != "/").Count();
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable FindAll(int maximumRows, int startRowIndex)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            return db.Dealers.Where(p => p.ParentCode != null).OrderBy(p => p.DealerCode).Skip(startRowIndex).Take(maximumRows);
        }

        int _countByCodes = 0;
        public int CountByCodes(string dealerCode, string areacode, string dbCode)
        {
            return _countByCodes;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable FindByCodes(int maximumRows, int startRowIndex, string dealerCode, string areacode, string dbCode)
        {
            if (dealerCode == null) dealerCode = "";
            if (areacode == null) areacode = "";
            if (dbCode == null) dbCode = "";

            var db = DCFactory.GetDataContext<PartDataContext>();
            var dealers = db.Dealers.Where(p => p.AreaCode.Contains(areacode.ToUpper()) && p.DatabaseCode.Contains(dbCode.ToUpper()) && (p.DealerCode.Contains(dealerCode.ToUpper()) /*|| dealerCode.ToUpper().Contains(p.DealerCode)*/) && p.ParentCode != null)
                .OrderBy(p => p.DealerCode).AsQueryable();
            _countByCodes = dealers.Count();
            if (maximumRows > 0)
                dealers = dealers.Skip(startRowIndex).Take(maximumRows);
            return dealers;
        }


        int _countByCode = 0;
        public int CountByCode(string dealerCode, string autoinstockpart, string autoinstockvehicle)
        {
            return _countByCode;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable FindByCode(int maximumRows, int startRowIndex, string dealerCode, string autoinstockpart, string autoinstockvehicle)
        {
            if (dealerCode == null) dealerCode = "";
            var db = DCFactory.GetDataContext<PartDataContext>();
            var dealers = from i in db.Dealers
                          where i.DealerCode.Contains(dealerCode.ToUpper()) && i.ParentCode != null
                          select i;
            if (!string.IsNullOrEmpty(autoinstockpart))
                dealers = dealers.Where(d => d.AutoInStockPartStatus == Convert.ToBoolean(autoinstockpart));
            if(!string.IsNullOrEmpty(autoinstockvehicle))
                dealers = dealers.Where(d => d.AutoInStockVehicleStatus == Convert.ToBoolean(autoinstockvehicle));
            _countByCode = dealers.Count();
            return dealers.OrderBy(p => p.DealerCode).Skip(startRowIndex).Take(maximumRows);
        }

        int _countByCodeWithDB = 0;
        public int CountByCodeWithDB(string dealerCode, string dbCode)
        {
            return _countByCodeWithDB;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<Dealer> SearchDealerByCodeWithDB(string dealerCode, string dbCode, int maximumRows, int startRowIndex)
        {
            if (dealerCode == null) dealerCode = "";
            if (dbCode == null) dbCode = "";

            var db = DCFactory.GetDataContext<PartDataContext>();
            var dealers = db.Dealers.OrderBy(p => p.DealerCode).Where(p => p.DealerCode != "/" && p.DealerCode.Contains(dealerCode) && p.DatabaseCode.Contains(dbCode));
            _countByCodeWithDB = dealers.Count();

            if ((startRowIndex >= 0) && (maximumRows > 0)) dealers = dealers.Skip(startRowIndex).Take(maximumRows);
            return dealers.ToList();
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public void Delete(string DealerCode)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            var d = db.Dealers.SingleOrDefault(p => p.DealerCode == DealerCode);
            //var d = GetDealerByCode(DealerCode);
            if (d != null)
            {
                try
                {
                    db.Categories.DeleteAllOnSubmit(db.Categories.Where(p => p.DealerCode == DealerCode));
                    db.Favorites.DeleteAllOnSubmit(db.Favorites.Where(p => p.DealerCode == DealerCode));
                    var c = db.Contacts.SingleOrDefault(p => p.ContactId == d.ContactId);
                    db.Warehouses.DeleteAllOnSubmit(db.Warehouses.Where(p => p.DealerCode == d.DealerCode));
                    db.Dealers.DeleteOnSubmit(d);
                    if (c != null) db.Contacts.DeleteOnSubmit(c);
                    db.SubmitChanges();

                    MembershipHelper.RemoveMembershipProvider(DealerCode);
                    MembershipHelper.RemoveRoleProvider(DealerCode);

                    DealerHelper.Init();
                }
                catch { }
            }
        }

        public static List<Dealer> GetTopDealers()
        {
            //var db = DCFactory.GetDataContext<PartDataContext>();
            //return db.Dealers.Where(d => d.ParentCode == null).ToList();
            return DealerHelper.Dealers.Where(d => d.ParentCode == null).ToList();
        }

        public static Dealer GetDealerByCode(string DealerCode)
        {
            if (DealerCode == string.Empty) DealerCode = "/";
            //var db = DCFactory.GetDataContext<PartDataContext>();
            //return db.Dealers.SingleOrDefault(d => d.DealerCode == DealerCode);
            return DealerHelper.Dealers.SingleOrDefault(d => d.DealerCode == DealerCode);
        }

        public static Dealer GetTopDealer(string dealerCode)
        {
            var d = DealerDAO.GetDealerByCode(dealerCode);
            while ((d.ParentCode != "/") && (d.DealerCode != "/"))
                d = DealerDAO.GetDealerByCode(d.ParentCode);
            return d;
        }

        public static List<Dealer> GetAllChildDealer(string dealerCode)
        {
            var pDealer = DealerDAO.GetDealerByCode(dealerCode);
            if (pDealer == null) return new List<Dealer>();
            else return pDealer.Dealers.ToList();
        }
    }
    [DataObject(true)]
    public class DealerData
    {
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
    }
    public class SessionDealerDAO<T> where T: DealerData
    {
        protected static string key = "DealerSelected";

        public static List<T> Dealer
        {
            get
            {   
                List<T> dealer = HttpContext.Current.Session[key] as List<T>;
                if (dealer == null)
                {
                    dealer = new List<T>();
                    HttpContext.Current.Session[key] = dealer;
                }
                return dealer;

            }
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IEnumerable<T> FindAll()
        {
            return Dealer;
        }
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void SecsionDealerDelete(string dealercode)
        {
            T dd = Dealer.SingleOrDefault(d => d.DealerCode == dealercode);
            if (dd != null) Dealer.Remove(dd);
            HttpContext.Current.Session[key] = Dealer;
        }

    }
}
