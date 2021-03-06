using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;
using VDMS.Data.TipTop;
using VDMS.Helper;
using VDMS.I.Entity;
using VDMS.I.Linq;
using VDMS.I.Vehicle;
using VDMS.II.Linq;
using Item = VDMS.Core.Domain.Item;

//using VDMS.I.Entity;
//using VDMS.I.Entity;

namespace VDMS.I.ObjectDataSource
{
    public class ItemInstanceDataSource
    {
        #region ntDung
        private int inStockCount = 0;
        public int CountInStock(int maximumRows, int startRowIndex, string engineNumber, string ModelCode, string ColorCode, string StorePlace, string VouchersStatus)
        {
            return inStockCount;
        }
        public IQueryable<VDMS.I.Entity.ItemInstanceEx> SelectInStock(int maximumRows, int startRowIndex, string engineNumber, string ModelCode, string ColorCode, string StorePlace, string VouchersStatus)
        {
            var sdc = VDMS.II.Linq.DCFactory.GetDataContext<VDMS.I.Linq.VehicleDataContext>();
            var query = sdc.ItemInstances.Where(i => i.DealerCode == UserHelper.DealerCode)
                                         .Where(i => ItemHepler.GetInstockItemStatus().Contains(i.Status));
            if (!string.IsNullOrEmpty(engineNumber)) query = query.Where(i => i.EngineNumber.Contains(engineNumber.Trim().ToUpper()));
            if (!string.IsNullOrEmpty(ModelCode)) query = query.Where(i => i.ItemType.Contains(ModelCode.Trim().ToUpper()));
            if (!string.IsNullOrEmpty(ColorCode)) query = query.Where(i => i.Color.Contains(ColorCode.Trim().ToUpper()));
            if (!string.IsNullOrEmpty(StorePlace))
            {
                query = query.Where(i => i.BranchCode == StorePlace);
                //&& i.Status != (int)ItemStatus.Moved);
            }

            //var spList = sdc.ShippingDetails.Where(i => i.ProductInstanceId > 0);
            //if (!string.IsNullOrEmpty(VouchersStatus) && (VouchersStatus != "2"))
            //{
            //    spList = spList.Where(s => s.VoucherStatus == int.Parse(VouchersStatus));
            //}

            var res = query
                        //.Join(spList, i => i.EngineNumber, s => s.EngineNumber, (i, s) 
                        //=> new VDMS.I.Entity.ItemInstanceEx(i) { HasVoucher = s.VoucherStatus == 1 });
                        .Select(i => new VDMS.I.Entity.ItemInstanceEx(i)).ToList(); // => ko join nua tranh' trung`
            var ress = res.AsQueryable<ItemInstanceEx>();
            if (!string.IsNullOrEmpty(VouchersStatus) && (VouchersStatus != "2"))
            {
                ress = ress.Where(i => i.VoucherStatus == int.Parse(VouchersStatus));
            }
            
            inStockCount = ress.Count();
            if ((startRowIndex >= 0) && (maximumRows > 0)) 
                ress = ress.Skip(startRowIndex).Take(maximumRows);
            return ress;
        }
        //public List<Iteminstance> Select(int maximumRows, int startRowIndex, string engineNumber, string ModelCode, string ColorCode, string StorePlace, string VouchersStatus)
        //{
        //    List<Iteminstance> list = new List<Iteminstance>();
        //    IDao<Iteminstance, long> dao = DaoFactory.GetDao<Iteminstance, long>();

        //    //DataIteminstance d;d.
        //    engineNumber = (string.IsNullOrEmpty(engineNumber)) ? "%" : "%" + engineNumber.Trim().ToUpper() + "%";
        //    ModelCode = (string.IsNullOrEmpty(ModelCode)) ? "%" : "%" + ModelCode.Trim().ToUpper() + "%";
        //    ColorCode = (string.IsNullOrEmpty(ColorCode)) ? "%" : "%" + ColorCode.Trim().ToUpper() + "%";
        //    StorePlace = (string.IsNullOrEmpty(StorePlace) || StorePlace == "0") ? "%" : "%" + StorePlace + "%";

        //    if (StorePlace != "%")
        //    {
        //        dao.SetCriteria(new ICriterion[] { Expression.Like("Enginenumber", engineNumber), Expression.In("Status", ItemHepler.GetInstockItemStatus()), Expression.Like("Itemtype", ModelCode), Expression.Like("Color", ColorCode), Expression.Like("Branchcode", StorePlace) });
        //    }
        //    else
        //    {
        //        // Lay tat ca cac BranchCode của DealerCode hien tai
        //        //AddressDataSource aDs = new AddressDataSource();
        //        IList<VDMS.II.Entity.Warehouse> dsA = AddressDataSource.GetListBranches();
        //        List<string> lBranchCode = new List<string>();
        //        foreach (VDMS.II.Entity.Warehouse w in dsA)
        //        {
        //            lBranchCode.Add(w.Code);
        //        }
        //        dao.SetCriteria(new ICriterion[] { Expression.Like("Enginenumber", engineNumber), Expression.In("Status", ItemHepler.GetInstockItemStatus()), Expression.Like("Itemtype", ModelCode), Expression.Like("Color", ColorCode), Expression.In("Branchcode", lBranchCode) });
        //    }

        //    // neu tim theo voucher thi list shipdetail dua theo list iis roi add shipdetail.iis vao list
        //    // VouchersStatus == 2 => all status
        //    if (!string.IsNullOrEmpty(VouchersStatus) && VouchersStatus != "2")
        //    {
        //        list = dao.GetAll();
        //        List<Shippingdetail> listSD = new List<Shippingdetail>();
        //        IDao<Shippingdetail, long> daoSD = DaoFactory.GetDao<Shippingdetail, long>();
        //        switch (VouchersStatus)
        //        {
        //            case "0":
        //                daoSD.SetCriteria(new ICriterion[] { Expression.In("PRODUCTINSTANCE", list), Expression.Eq("Voucherstatus", false) });
        //                break;
        //            case "1":
        //                daoSD.SetCriteria(new ICriterion[] { Expression.In("PRODUCTINSTANCE", list), Expression.Eq("Voucherstatus", true) });
        //                break;
        //            default:
        //                break;
        //        }

        //        list.Clear();

        //        listSD = daoSD.GetPaged(startRowIndex / maximumRows, maximumRows);
        //        count1 = daoSD.ItemCount;
        //        if (startRowIndex >= count1)
        //        {
        //            listSD = daoSD.GetPaged(0, maximumRows);
        //            count1 = daoSD.ItemCount;
        //        }
        //        if (listSD.Count > 0)
        //        {
        //            foreach (Shippingdetail sd in listSD)
        //            {
        //                list.Add(sd.PRODUCTINSTANCE);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
        //        count1 = dao.ItemCount;
        //        if (startRowIndex >= count1)
        //        {
        //            list = dao.GetPaged(0, maximumRows);
        //            count1 = dao.ItemCount;
        //        }
        //    }
        //    HttpContext.Current.Items["rowCount"] = count1;
        //    return list;
        //}
        #endregion

        #region nmChi
        private int count = 0;

        public int SelectCount(int maximumRows, int startRowIndex, string engineNumberLike)
        {
            return count;
        }

        public int SelectCount(int maximumRows, int startRowIndex, string engineNumberLike, string dealerCode)
        {
            return count;
        }

        private string GetRestDatabaseCode(string databaseCode)
        {
            return (databaseCode.ToUpper() == "DNF") ? "HTF" : "DNF";
        }

        public List<ItemInstance> SelectItem(int maximumRows, int startRowIndex, string engineNumberLike, string dealerCode, int[] status)
        {
            //IDao<Iteminstance, long> dao = DaoFactory.GetDao<Iteminstance, long>();
            //List<Iteminstance> list = null;

            //// Criteria
            //List<ICriterion> crit = new List<ICriterion>();
            //crit.Add(Expression.InsensitiveLike("Enginenumber", engineNumberLike));
            //if (!string.IsNullOrEmpty(dealerCode)) crit.Add(Expression.Eq("Dealercode", dealerCode));
            //if (status != null)
            //{
            //    crit.Add(Expression.In("Status", status));
            //}
            //dao.SetCriteria(crit.ToArray());

            //dao.SetOrder(new NHibernate.Expression.Order[] { NHibernate.Expression.Order.Asc("Enginenumber") });
            //list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
            //count = dao.ItemCount;

            //// if user change pageindex after change "select conditions" without press "check" button
            //// so we select with pageindex=0
            //if (startRowIndex >= count)
            //{
            //    list = dao.GetPaged(0, maximumRows);
            //    count = dao.ItemCount;
            //}
            //HttpContext.Current.Items["listgvSelectEngineRowCount"] = count;

            //return list;
            var db = DCFactory.GetDataContext<VehicleDataContext>();
            //if (status != null)
            //{
            //    var query = (from i in db.ItemInstances
            //                 where i.EngineNumber.Contains(engineNumberLike)
            //                       && (string.IsNullOrEmpty(dealerCode) || i.DealerCode.Equals(dealerCode))
            //                       && (status.Contains(i.Status))
            //                 orderby i.EngineNumber
            //                 select i);
            //    count = query.Count();
            //    HttpContext.Current.Items["listgvSelectEngineRowCount"] = count;
            //    return query.Skip(startRowIndex).Take(maximumRows).ToList();
            //}
            if (engineNumberLike.ToCharArray().Last() == '%')
                engineNumberLike = engineNumberLike.Substring(0, engineNumberLike.Length - 1);
            var query2 = (from i in db.ItemInstances
                         where i.EngineNumber.Contains(engineNumberLike)
                         && (string.IsNullOrEmpty(dealerCode) ? true : i.DealerCode.Equals(dealerCode))
                         orderby i.EngineNumber
                         select i);
            count = query2.Count();
            HttpContext.Current.Items["listgvSelectEngineRowCount"] = count;
            return query2.Skip(startRowIndex).Take(maximumRows).ToList();
        

    }

        // select item for sale
        public List<ItemInstance> SelectForSale(int maximumRows, int startRowIndex, string engineNumberLike)
        {
            //IDao<Iteminstance, long> dao = DaoFactory.GetDao<Iteminstance, long>();
            //List<Iteminstance> list = null;
            //engineNumberLike = string.IsNullOrEmpty(engineNumberLike) ? "%" : "%" + engineNumberLike + "%";

            //dao.SetCriteria(new ICriterion[] { 
            //    Expression.Eq("Dealercode", UserHelper.DealerCode), 
            //    Expression.Eq("Branchcode", UserHelper.BranchCode), 
            //    Expression.InsensitiveLike("Enginenumber", engineNumberLike), 
            //    Expression.In("Status", ItemHepler.GetInstockItemStatus()) 
            //});
            //dao.SetOrder(new NHibernate.Expression.Order[] { NHibernate.Expression.Order.Asc("Enginenumber") });
            //list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
            //count = dao.ItemCount;

            //// if user change pageindex after change "select conditions" without press "check" button
            //// so we select with pageindex=0
            //if (startRowIndex >= count)
            //{
            //    list = dao.GetPaged(0, maximumRows);
            //    count = dao.ItemCount;
            //}
            //HttpContext.Current.Items["listgvSelectItemRowCount"] = count;

            //return list;
            var db = DCFactory.GetDataContext<VehicleDataContext>();
            var query = from ii in db.ItemInstances
                        where ii.EngineNumber.Contains(engineNumberLike)
                              && ii.DealerCode.Equals(UserHelper.DealerCode)
                              && ii.BranchCode.Equals(UserHelper.BranchCode)
                              && ItemHepler.GetInstockItemStatus().Contains(ii.Status)
                        orderby ii.EngineNumber
                        select ii;
            count = query.Count();
            HttpContext.Current.Items["listgvSelectItemRowCount"] = count;
            return query.Skip(startRowIndex).Take(maximumRows).ToList();

        }
        public List<ItemInstance> SelectAll(int maximumRows, int startRowIndex, string engineNumberLike)
        {
            //IDao<Iteminstance, long> dao = DaoFactory.GetDao<Iteminstance, long>();
            //List<Iteminstance> list = null;
            //engineNumberLike = string.IsNullOrEmpty(engineNumberLike) ? "%" : "%" + engineNumberLike + "%";

            //dao.SetCriteria(new ICriterion[] { Expression.Eq("Dealercode", UserHelper.DealerCode), 
            //                                   Expression.Eq("Branchcode", UserHelper.BranchCode),
            //                                   Expression.InsensitiveLike("Enginenumber", engineNumberLike) });
            //dao.SetOrder(new NHibernate.Expression.Order[] { NHibernate.Expression.Order.Asc("Enginenumber") });
            //list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
            //count = dao.ItemCount;

            //// if user change pageindex after change "select conditions" without press "check" button
            //// so we select with pageindex=0
            //if (startRowIndex >= count)
            //{
            //    list = dao.GetPaged(0, maximumRows);
            //    count = dao.ItemCount;
            //}
            //HttpContext.Current.Items["listgvSelectItemRowCount"] = count;

            //return list;
            var db = DCFactory.GetDataContext<VehicleDataContext>();
            var query = from ii in db.ItemInstances
                        where ii.EngineNumber.Contains(engineNumberLike)
                              && ii.DealerCode.Equals(UserHelper.DealerCode)
                              && ii.BranchCode.Equals(UserHelper.BranchCode)
                        orderby ii.EngineNumber
                        select ii;
            count = query.Count();
            HttpContext.Current.Items["listgvSelectItemRowCount"] = count;
            return query.Skip(startRowIndex).Take(maximumRows).ToList();
        }

        //public static List<int> GetItemStatusForSale()
        //{
        //    List<int> list = new List<int>();
        //    list.Add((int)ItemStatus.AdmitTemporarily);
        //    list.Add((int)ItemStatus.Imported);
        //    list.Add((int)ItemStatus.Moved);
        //    list.Add((int)ItemStatus.Redundant);

        //    return list;
        //}

        // select sold items and tiptop items for service
        private List<ItemInstance> SelectFromTipTop(int maximumRows, int startRowIndex, string engineNumberLike, string dealerCode, string databaseCode1, string databaseCode2)
        {
            int page = (maximumRows == 0) ? 0 : startRowIndex / maximumRows;
            string databaseCode = databaseCode1;

            // get data from tiptop
            DataSet ds = Motorbike.GetItemInfo(engineNumberLike, databaseCode, dealerCode, page, maximumRows);
            if ((ds == null) || (ds.Tables[0].Rows.Count == 0))
            {
                databaseCode = databaseCode2;
                ds = Motorbike.GetItemInfo(engineNumberLike, databaseCode, dealerCode, page, maximumRows);
            }

            // count items
            count = (int)Motorbike.GetItemInfoCount(engineNumberLike, databaseCode, dealerCode);
            HttpContext.Current.Items["listgvSelectEngineRowCount"] = count;

            // re pagging if startRow is wrong 
            if (startRowIndex >= count && count > 0)
            {
                ds = Motorbike.GetItemInfo(engineNumberLike, databaseCode, dealerCode, 0, maximumRows);
            }

            // build return list
            List<ItemInstance> list = new List<ItemInstance>();
            ItemInstance item;
            var row = ds.Tables[0].Rows;
            int count_ds = row.Count - 1 ;
            while(count_ds >= 0)
            {
                var moreinfo = Motorbike.GetMoreItemInfo(row[count_ds]["EngineNo"].ToString(), row[count_ds]["DatabaseCode"].ToString());
                if (!string.IsNullOrEmpty(dealerCode))
                {
                    if (moreinfo.Tables[0].Rows[0]["DealerCode"] != dealerCode)
                    {
                        count_ds--;
                        continue;
                    }
                }
                item = new ItemInstance();
                if (moreinfo.Tables[0].Rows.Count > 0)
                {
                    item.DealerCode = moreinfo.Tables[0].Rows[0]["DealerCode"].ToString();
                    item.BranchCode = moreinfo.Tables[0].Rows[0]["DealerCode"].ToString();
                    item.MadeDate = (DateTime)moreinfo.Tables[0].Rows[0]["OutStockDate"]; ;
                    item.ImportedDate = (DateTime)moreinfo.Tables[0].Rows[0]["OutStockDate"];
                }
                
                item.Item = new Entity.Item();
                item.Item.ItemCode = row[count_ds]["ItemCode"].ToString();
                item.Item.DatabaseCode = databaseCode;
                item.Item.ColorCode = row[count_ds]["ColorCode"].ToString();
                item.Item.ColorName = row[count_ds]["ColorCode"].ToString();
                item.Item.ItemType = row[count_ds]["ItemCode"].ToString();

                item.ItemInstanceId = 0;
                item.DatabaseCode = row[count_ds]["DatabaseCode"].ToString();
                item.ItemType = item.Item.ItemType;
                item.Color = row[count_ds]["ColorCode"].ToString();

                item.EngineNumber = row[count_ds]["EngineNo"].ToString();
                item.ReleasedDate = DateTime.MinValue;

                list.Add(item);
                count_ds--;
            }
            return list;
        }
        public List<ItemInstance> SelectFromVDMS(int maximumRows, int startRowIndex, string engineNumberLike, string dealerCode)
        {
            //return SelectItem(maximumRows, startRowIndex, engineNumberLike, new int[] { (int)ItemStatus.Sold });
            return SelectItem(maximumRows, startRowIndex, engineNumberLike, dealerCode, null);    // ko quan tam den status
        }
        public IList<ItemInstance> SelectForSRS(int maximumRows, int startRowIndex, string engineNumberLike, string dealerCode)
        {
            List<ItemInstance> list = null;
            try
            {
                list = SelectFromVDMS(maximumRows, startRowIndex, engineNumberLike, dealerCode);
                if ((list == null) || (list.Count == 0))
                    list = SelectFromTipTop(maximumRows, startRowIndex, engineNumberLike, dealerCode, UserHelper.DatabaseCode, GetRestDatabaseCode(UserHelper.DatabaseCode));
            }
            catch { };
            return list;
        }
        public IList<ItemInstance> Select(int maximumRows, int startRowIndex, string engineNumberLike, string dealerCode)
        {
            //string enNum = (string.IsNullOrEmpty(engineNumberLike)) ? "%" : engineNumberLike.Trim().ToUpper() + "%";
            return SelectForSRS(maximumRows, startRowIndex, engineNumberLike, dealerCode);
        }

        // return empty when engineNumber is null or empty
        public IList<ItemInstance> SelectNot(int maximumRows, int startRowIndex, string engineNumberLike, string dealerCode)
        {
            if(string.IsNullOrEmpty(engineNumberLike)) return new List<ItemInstance>();
            //string enNum = (string.IsNullOrEmpty(engineNumberLike)) ? "%" : engineNumberLike.Trim().ToUpper() + "%";
            return SelectForSRS(maximumRows, startRowIndex, engineNumberLike, dealerCode);
        }

        public static ItemInstance GetByEngineNumber(string engineNum)
        {
            var db = DCFactory.GetDataContext<VehicleDataContext>();
            return db.ItemInstances.SingleOrDefault(p => p.EngineNumber.Equals(engineNum.Trim()));
        }

        //public bool HasChildRow(string Id)
        //{
        //    if (Id.Trim() == "") return false;
        //    IDao<Iteminstance, long> daoIIS = DaoFactory.GetDao<Iteminstance, long>();
        //    IDao<Itemfavorite, long> daoIF = DaoFactory.GetDao<Itemfavorite, long>();
        //    IDao<Orderdetail, long> daoOD = DaoFactory.GetDao<Orderdetail, long>();
        //    //Itemfavorite item = new Itemfavorite();

        //    daoIIS.SetCriteria(new ICriterion[] { Expression.Eq("Itemcode", Id) });
        //    daoIF.SetCriteria(new ICriterion[] { Expression.Eq("Item.Id", Id) });
        //    daoOD.SetCriteria(new ICriterion[] { Expression.Eq("Item.Id", Id) });

        //    return (daoIIS.GetAll().Count > 0) || (daoIF.GetAll().Count > 0) || (daoOD.GetAll().Count > 0);
        //}
        //public void Delete(string Id)
        //{
        //    if (HasChildRow(Id)) return;
        //    IDao<Item, string> dao = DaoFactory.GetDao<Item, string>();
        //    Item item;
        //    try
        //    {
        //        item = dao.GetById(Id, true);
        //        dao.Delete(item);
        //    }
        //    catch { }
        //}
        //public bool Update(string Id, bool Available, string Colorcode, string Colorname, string Itemname, string DatabaseCode, string price)
        //{
        //    IDao<Item, string> dao = DaoFactory.GetDao<Item, string>();
        //    Item item = dao.GetById(Id, true);
        //    long Price;
        //    if (item == null) return false;
        //    long.TryParse(price, out Price);

        //    item.Price = Price;
        //    item.Available = Available;
        //    if (Colorcode != null) item.Colorcode = Colorcode.Trim();
        //    if (Colorname != null) item.Colorname = Colorname.Trim();
        //    if (Itemname != null) item.Itemname = Itemname.Trim();
        //    if (DatabaseCode != null) item.DatabaseCode = DatabaseCode.Trim();
        //    try
        //    {
        //        return (dao.SaveOrUpdate(item) != null);
        //    }
        //    catch { return false; }
        //}
        //public bool Update(string Id, bool Available, string Itemname, string Price)
        //{
        //    return Update(Id, Available, null, null, Itemname, null, Price);
        //}
        //public bool Update(IOrderedDictionary keys, IOrderedDictionary oldValues, IOrderedDictionary newValues)
        //{
        //    IDao<Item, string> dao = DaoFactory.GetDao<Item, string>();
        //    Item item = dao.GetById(keys[0].ToString(), true);
        //    if (item == null) return false;
        //    if (newValues.Contains("Available")) item.Available = Convert.ToBoolean(newValues["Available"].ToString());
        //    if (newValues.Contains("Colorcode")) item.Colorcode = (newValues["Colorcode"].ToString());
        //    if (newValues.Contains("Colorname")) item.Colorcode = (newValues["Colorname"].ToString());
        //    if (newValues.Contains("Id")) item.Colorcode = (newValues["Id"].ToString());
        //    if (newValues.Contains("Itemname")) item.Itemname = (newValues["Itemname"].ToString());
        //    if (newValues.Contains("Areacode")) item.DatabaseCode = (newValues["Areacode"].ToString());
        //    try
        //    {
        //        return (dao.SaveOrUpdate(item) != null);
        //    }
        //    catch { return false; }
        //}

        public ItemInstanceDataSource()
        {
        }
        #endregion
    }

    public class MoreInfoEngineNo
    {
        public string Dealercode { get; set; }
        public DateTime? OutStockDate { get; set; }
    }
}
