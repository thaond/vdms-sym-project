using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.Core.Domain;
using VDMS.Core.Data;
using VDMS.Data.IDAL.Interface;
using NHibernate.Expression;
using System.Collections;
using VDMS.Common.Utils;
using VDMS.Common.Web;
using VDMS.Common.Web.Validator;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.DAL2;
using VDMS.Data.TipTop;
using VDMS.Helper;
using VDMS.I.Entity;
using VDMS.I.Linq;
using VDMS.I.ObjectDataSource;
using VDMS.I.Vehicle;
using VDMS.II.Linq;
using Item = VDMS.I.Entity.Item;
using ShippingDetail = Resources.ShippingDetail;
using ShippingHeader = VDMS.Core.Domain.ShippingHeader;
using System.Threading;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Data;
namespace VDMS.I.Vehicle
{
    public class AutoReceiveVehicle
    {
        #region BaseFuntion
        public static List<VDMS.I.Entity.ShippingDetail> Select(Int64 shipID)
        {
            using (var db = new VehicleDataContext())
            {
                return db.ShippingDetails.Where(p => p.ShippingId == shipID).ToList();
            }
        }
        public static ShippingHeader VDMS_ShippingHeader(string shipNumber)
        {
            IDao<ShippingHeader, long> dao = DaoFactory.GetDao<ShippingHeader, long>();
            dao.SetCriteria(new ICriterion[] { 
            Expression.Eq("Shippingnumber", shipNumber)});
            IList list = dao.GetAll();
            return ((list != null) && (list.Count > 0)) ? (ShippingHeader)list[0] : null;
        }
        /// <summary>
        /// Can chinh
        /// </summary>
        /// <param name="shipNum"></param>
        /// <param name="onTipTop"></param>
        /// <returns></returns>
        public static ShippingHeader GetShippingHeader(string shipNum, out bool onTipTop)
        {
            string shipNumber = shipNum.ToUpper().Trim();

            // get shipheader from VDMS
            ShippingHeader SH = VDMS_ShippingHeader(shipNumber);
            VDMS.I.Entity.IShippingHeader drSH = TipTop_ShippingHeader(shipNumber);

            if (SH == null)  // not found in VDMS
            {
                onTipTop = drSH != null;
                if (!onTipTop) return null;
            }
            else    // already exist in VDMS
            {
                onTipTop = false;
            }
            // after all: save infos and return
            return SH;
        }

        public static VDMS.I.Entity.IShippingHeader TipTop_ShippingHeader(string shipNumber)
        {
            //return Shipping.GetShippingHeader(shipNumber, UserHelper.DealerCode);
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            return dc.IShippingHeaders.SingleOrDefault(h => h.IssueNumber == shipNumber);
        }
        public static bool IsOrderConfirmed(string OrderNumber)
        {
            IDao<Orderheader, long> dao = DaoFactory.GetDao<Orderheader, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Ordernumber", OrderNumber), Expression.Eq("Status", (int)VDMS.I.Vehicle.OrderStatus.Confirmed) });
            return dao.GetCount() == 1;
        }
        /// <summary>
        /// Func for AutoVehicleinstock
        /// </summary>
        /// <returns></returns>
        public static IList AugoGetOrder()
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            List<string> autodealerlist = DealerHelper.AutoVehicleDealerlist();
            var query = from v in dc.OrderHeaders
                        where v.DeliveredStatus == 1 && v.Status == (int)VDMS.I.Vehicle.OrderStatus.Confirmed && autodealerlist.Contains(v.DealerCode)
                        select v;
            return query.ToList();
        }
        /// <summary>
        /// /// Func for AutoVehicleinstock
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public static IList AugoGetIss(string orderNumber)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            var list = dc.IShippingDetails.Where(h => h.TipTopOrderNumber == orderNumber)
                         .GroupBy(d => d.IssueNumber).Join(dc.IShippingHeaders, d => d.Key, h => h.IssueNumber, (d, h) => h);
            return list.ToList();
        }

        public static List<VDMS.I.Entity.IShippingDetail> TipTop_ShippingDetail(string shipNumber)
        {
            //string oNumber = txtOrderNumber.Text.Trim().ToUpper();
            //return (string.IsNullOrEmpty(oNumber)) ? Shipping.GetShippingDetail(shipNumber).Tables[0] : Shipping.GetShippingDetail(shipNumber, oNumber).Tables[0];

            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            List<VDMS.I.Entity.IShippingDetail> res = dc.IShippingDetails.Where(d => d.IssueNumber == shipNumber).ToList();
            return res;
        }

        public static bool isExsitDataItemInstance(string enginenumber)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            if (dc.ItemInstances.Where(d => d.EngineNumber == enginenumber).Count() > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region ActiveFuntion

        public static void RunAuto()
        {
            //Get Order need receive
            var ao = AugoGetOrder();
            int tao = ao.Count - 1;
            while (tao >= 0)
            {
                OrderHeader oh = (OrderHeader)ao[tao];
                var ish = AugoGetIss(oh.OrderNumber);
                int tc = ish.Count - 1;
                while (tc >= 0)
                {
                    bool ioc = true;
                    bool ontiptop = false;

                    IShippingHeader isd = (IShippingHeader)ish[tc];

                    if (isd.ShipDate >= DealerHelper.GetAutoVehicleStartDate(isd.DealerCode) && isd.ShipDate.AddHours(DealerHelper.GetAutoVehicleDealerSpan(isd.DealerCode)) < DateTime.Now)
                        ioc = false;

                    var tt = TipTop_ShippingDetail(isd.IssueNumber);

                    int ttc = tt.Count - 1;

                    while (ttc >= 0)
                    {
                        ioc &= IsOrderConfirmed(tt[ttc].TipTopOrderNumber);
                        ShippingHeader SH = GetShippingHeader(tt[ttc].IssueNumber, out ontiptop);
                        if (SH != null)
                        {
                            //List<VDMS.I.Entity.ShippingDetail> items = Select(SH.Id);
                            //items = items.Where(i => i.Status == (int)ImportItemStatus.Imported || i.Status == (int)ImportItemStatus.AdmitTemporarily).ToList();
                            //if (items.Count() > 0)
                            //{
                            //    ioc = false;
                            //    break;
                            //}
                        }
                        ttc--;
                    }
                    if (!ioc && !ontiptop)
                    {
                        //Its not automatic

                    }
                    else
                    {
                        RunInDebug(oh.OrderNumber);
                        //Its automatic
                    }
                    tc--;
                }
                tao--;
            }
        }

        public static void RunInDebug(string ordernumber)
        {
            var ish = AugoGetIss(ordernumber);
            foreach (var s in ish)
            {
                IShippingHeader isd = (IShippingHeader)s;
                var tt = TipTop_ShippingDetail(isd.IssueNumber);

                bool ioc = true;
                bool ontiptop = false;
                foreach (var t in tt)
                {
                    ioc &= IsOrderConfirmed(t.TipTopOrderNumber);
                    ShippingHeader SH = GetShippingHeader(t.IssueNumber, out ontiptop);
                    if (SH != null)
                    {
                        List<VDMS.I.Entity.ShippingDetail> items = Select(SH.Id);
                        items = items.Where(i => i.Status == (int)ImportItemStatus.Imported || i.Status == (int)ImportItemStatus.AdmitTemporarily).ToList();
                        if (items.Count() > 0)
                        {
                            ioc = false;
                            break;
                        }
                    }
                }
                VDMS.I.Entity.IShippingHeader drSH = TipTop_ShippingHeader(isd.IssueNumber);
                var tth = Shipping.GetShippingHeader_auto(drSH.IssueNumber, drSH.DealerCode, DealerHelper.GetDatabaseCode(drSH.DealerCode));
                if (tth == null)
                {
                    break;
                }
                else
                {
                    ioc = false;
                }
                if (!ioc && !ontiptop)
                {
                    //Its not automatic

                }
                else
                {
                    if (drSH.ShipDate >= DealerHelper.GetAutoVehicleStartDate(drSH.DealerCode) && drSH.ShipDate.AddHours(DealerHelper.GetAutoVehicleDealerSpan(drSH.DealerCode)) <= DateTime.Now)
                    {
                        if (SaveUpdate(drSH.BranchCode, drSH.IssueNumber, drSH.IssueNumber, DealerHelper.GetAreaCode(drSH.DealerCode), drSH.ShipDate, drSH.DealerCode))
                            LogMessage("IssueNumber: " + drSH.IssueNumber + ", branchcode: " + drSH.BranchCode + ", DealerCode:" + drSH.DealerCode + ", Shipdate:" + drSH.ShipDate);//Log
                    }
                }
            }
        }

        public static bool SaveUpdate(string branchcode, string issueNumber, string shipnumber, string areacode, DateTime baseImportDate, string dealercode)
        {

            string exception, itemCode, engineNumber, itemType, color, orderNumber, shipTo;
            Collection<ImportErrorCode> errorCode = new Collection<ImportErrorCode>();
            Int32 status;
            bool hasVoucher;
            ImportItemStatus IisStatus;
            ItemInstance IInst = null;
            ItemStatus hisItemStatus;
            DateTime madeDate, impDate, itemImpDate;
            VDMS.I.Entity.ShippingDetail SD;
            long Price;

            Item item = null;
            long shipID;

            using (var db = new VehicleDataContext())
            {
                System.Data.Common.DbTransaction transaction;
                db.Connection.Open();
                transaction = db.Connection.BeginTransaction();
                db.Transaction = transaction;
                try
                {
                    impDate = CalculateInitImportDate(baseImportDate);
                    if (impDate < baseImportDate)
                    {
                        AddError(ImportErrorCode.ImportDateLessThanBaseDate, errorCode);
                        return false;
                    }
                    var tt = TipTop_ShippingDetail(issueNumber);
                    foreach (var m in tt)
                    {
                        exception = m.Exception;
                        status = 1;
                        hasVoucher = IsOrderConfirmed(m.TipTopOrderNumber);
                        itemCode = m.ItemCode;
                        engineNumber = m.EngineNumber;
                        itemType = m.Model;
                        if (m.Status == (int)ImportItemStatus.Imported || m.Status == (int)ImportItemStatus.AdmitTemporarily)
                        {
                            continue;
                        }

                        if (ItemInstanceHelper.EngineNumberExist(engineNumber))
                        {
                            LogError("IssueNumber: " + issueNumber + ", branchcode: " + branchcode + ", DealerCode:" + dealercode + ", Shipdate:" + baseImportDate + ", EngineNumber in dataItemInstance:" + engineNumber);//Log
                            continue;
                            //throw new Exception( engineNumber +  " avaliable in ItemInstance");
                            //return false;
                        }
                        if (isExsitDataItemInstance(engineNumber))
                            return false;

                        color = m.ColorCode + " (" + m.ColorName + ")";
                        orderNumber = m.TipTopOrderNumber;
                        DateTime.TryParse(m.OutStockDate.ToString(),
                                          Thread.CurrentThread.CurrentCulture,
                                          DateTimeStyles.AllowWhiteSpaces, out madeDate);
                        long.TryParse(m.Price.ToString(), out Price);

                        // custom imported date for item
                        string impDateString = CalculateInitImportDate(baseImportDate).ToShortDateString();
                        if (string.IsNullOrEmpty(impDateString)) impDateString = DateTime.Now.ToShortDateString();

                        if (
                            !DateTime.TryParse(impDateString, Thread.CurrentThread.CurrentCulture,
                                               DateTimeStyles.AllowWhiteSpaces, out itemImpDate))
                        {
                            AddError(ImportErrorCode.InvalidImportDate, errorCode);
                        }

                        if (itemImpDate < baseImportDate) AddError(ImportErrorCode.ImportDateLessThanBaseDate, errorCode);
                        if (itemImpDate > DateTime.Now) AddError(ImportErrorCode.ImportDateTooLate, errorCode);
                        if (InventoryHelper.IsInventoryLock(itemImpDate, dealercode, branchcode))
                            AddError(ImportErrorCode.ImportDateLocked, errorCode);
                        if (errorCode.Count > 0)
                        {
                            return false;
                        }

                        //branchCode = ((Label)row.FindControl("lblBranchCode")).Text;

                        // day du cac thu roi thi khong chap nhan exception
                        if ((hasVoucher) && (status == 1)) exception = string.Empty;

                        IInst = null;
                        // get n' check item in table DATA_ITEM
                        item = CommonDAO.GetItemByCode(db, itemCode);
                        if (item == null)
                        {
                            AddError(ImportErrorCode.ItemNotExist, errorCode);
                            return false;
                        }
                        


                    }
                    shipTo = branchcode;

                    // save Shipping header
                    VDMS.I.Entity.ShippingHeader SH = CommonDAO.SaveOrUpdateShippingHeader(db, areacode, shipnumber,
                                                                                           shipTo,
                                                                                           impDate, dealercode,
                                                                                           (tt.Count +
                                                                                            tt.Count),
                                                                                           dealercode);
                    db.SubmitChanges();

                    if (SH == null)
                    {
                        return false;
                    }
                    shipID = db.ShippingHeaders.SingleOrDefault(p => p.ShippingNumber == shipnumber).ShippingId;


                    #region save shipping

                    foreach (var m in tt)
                    {
                        exception = m.Exception;
                        status = 1;
                        hasVoucher = IsOrderConfirmed(m.TipTopOrderNumber);
                        itemCode = m.ItemCode;
                        engineNumber = m.EngineNumber;
                        itemType = m.Model;

                        color = m.ColorCode + " (" + m.ColorName + ")";
                        orderNumber = m.TipTopOrderNumber;
                        DateTime.TryParse(m.OutStockDate.ToString(),
                                          Thread.CurrentThread.CurrentCulture,
                                          DateTimeStyles.AllowWhiteSpaces, out madeDate);
                        long.TryParse(m.Price.ToString(), out Price);


                        if (m.Status == (int)ImportItemStatus.Imported || m.Status == (int)ImportItemStatus.AdmitTemporarily)
                        {
                            continue;
                        }

                        if (ItemInstanceHelper.EngineNumberExist(engineNumber))
                        {
                            LogError("IssueNumber: " + issueNumber + ", branchcode: " + branchcode + ", DealerCode:" + dealercode + ", Shipdate:" + baseImportDate + ", EngineNumber in dataItemInstance:" + engineNumber);//Log
                            continue;
                            //throw new Exception( engineNumber +  " avaliable in ItemInstance");
                            //return false;
                        }
                        
                        
                        // custom imported date for item
                        string impDateString = impDate.ToShortDateString();
                        if (
                            !DateTime.TryParse(impDateString, Thread.CurrentThread.CurrentCulture,
                                               DateTimeStyles.AllowWhiteSpaces, out itemImpDate))
                        {
                            AddError(ImportErrorCode.InvalidImportDate, errorCode);
                        }

                        if (itemImpDate < baseImportDate) AddError(ImportErrorCode.ImportDateLessThanBaseDate, errorCode);
                        if (itemImpDate > DateTime.Now) AddError(ImportErrorCode.ImportDateTooLate, errorCode);
                        if (InventoryHelper.IsInventoryLock(itemImpDate, dealercode, branchcode))
                            AddError(ImportErrorCode.ImportDateLocked, errorCode);
                        if (errorCode.Count > 0)
                        {
                            return false;
                        }

                        

                        if ((hasVoucher) && (status == 1)) exception = string.Empty;
                        item = CommonDAO.GetItemByCode(db, itemCode);
                        if (status > 0) // nhap xe hoac tam nhap => save iteminstance n' transHistory
                        {
                            // clear instance for old shipping info
                            //foreach (var sd in CommonDAO.GetShippingDetails(db, engineNumber))
                            //{
                            //    sd.ProductInstanceId = null;
                            //}
                            // save ItemInstance of shipping
                            switch (status)
                            {
                                case 0:
                                    IisStatus = ImportItemStatus.NotArrived;
                                    break;
                                case 1:
                                    IisStatus = ImportItemStatus.Imported;
                                    break;
                                case 2:
                                    IisStatus = ImportItemStatus.AdmitTemporarily;
                                    break;
                                default:
                                    IisStatus = ImportItemStatus.NotArrived;
                                    break;
                            }
                            //db.SubmitChanges();
                            IInst = CommonDAO.SaveOrUpdateItemInstance(db, dealercode, branchcode,
                                                                       engineNumber,
                                                                       shipnumber, orderNumber, itemType, item,
                                                                       itemImpDate,
                                                                       color, (int)IisStatus, madeDate,
                                                                       DealerHelper.GetDatabaseCode(dealercode));
                            db.SubmitChanges();

                            if (IInst == null)
                            {
                                return false;
                            }

                            // save transaction history. <actualCost> is temporary equal to "zero"
                            switch (status)
                            {
                                case 0:
                                    hisItemStatus = ItemStatus.Lacked;
                                    break;
                                case 1:
                                    hisItemStatus = ItemStatus.Imported;
                                    break;
                                case 2:
                                    hisItemStatus = ItemStatus.AdmitTemporarily;
                                    break;
                                default:
                                    hisItemStatus = ItemStatus.AdmitTemporarily;
                                    break;
                            }
                            var t = CommonDAO.SaveTransHist(db, IInst, itemImpDate, hisItemStatus, Price,
                                                            dealercode, branchcode);

                            // save to Inventory of Day
                            var ttt = InventoryHelper.SaveInventoryDay(db, itemCode, itemImpDate, 1, (int)IisStatus,
                                                                      dealercode, branchcode);

                        }

                        // save shipping detail info
                        SD = CommonDAO.SaveOrUpdateShippingDetail(db, shipID, item, engineNumber, status,
                                                                  hasVoucher,
                                                                  exception, IInst, itemType, color,
                                                                  dealercode,
                                                                  orderNumber);
                        //db.SubmitChanges();

                    }


                    #endregion

                    #region Update order delivered status
                    List<String> listOrderNumber = new List<string>();
                    foreach (var m in tt)
                    {
                        listOrderNumber.Add((m.TipTopOrderNumber));
                    }

                    List<VDMS.I.Entity.ShippingDetail> loh = db.ShippingDetails.Where(p => listOrderNumber.Contains(p.OrderNumber)).ToList();

                    foreach (var m in tt)
                    {
                        orderNumber = m.TipTopOrderNumber;
                        var list = db.OrderHeaders.FirstOrDefault(p => p.OrderNumber == orderNumber); //oDao.GetAll();)
                        //list.CanUndoAutoReceive = true;
                        if (list != null)
                        {
                            DataSet ds = InventoryDao.CheckOrderDetail(list.OrderHeaderId);
                            int Orderstatus = (int)DeliveredOrderStatus.DeliveredAll;
                            int Orderqty, OrderShipped;
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                item = CommonDAO.GetItemByCode(db, dr["itemcode"].ToString());
                                Orderqty = int.Parse(dr["orderqty"].ToString());

                                OrderShipped =
                                    loh.Count(
                                        p =>
                                        p.OrderNumber == orderNumber && p.ItemCode == item.ItemCode &&
                                        p.Status == (int)ItemStatus.Imported);

                                if ((Orderqty - OrderShipped) != 0)
                                {
                                    Orderstatus = (int)DeliveredOrderStatus.NotDeliveredAll;
                                }
                            }
                            list.DeliveredStatus = Orderstatus;
                            //  (Orderstatus.Equals((int)DeliveredOrderStatus.NotDeliveredAll)) ? (int)DeliveredOrderStatus.NotDeliveredAll : (int)DeliveredOrderStatus.DeliveredAll;
                        }
                    }
                    db.SubmitChanges();
                    transaction.Commit();
                    //trans.Complete();

                    #endregion
                    
                    return true;
                }
                catch(Exception e)
                {
                    LogError("IssueNumber: " + issueNumber + ", branchcode: " + branchcode + ", DealerCode:" + dealercode + ", Shipdate:" + baseImportDate + ", System Error:" + e.Message );//Log
                    transaction.Rollback();
                    return false;
                }
                finally
                {
                    if (db.Connection != null)
                    {
                        db.Connection.Close();
                    }
                }
            }
        }

        #endregion
        #region ExtFuntion

        public static DateTime CalculateInitImportDate(DateTime baseImpDate)
        {
            DateTime result;
            int month = baseImpDate.Month, year = baseImpDate.Year;

            if ((month == DateTime.Now.Month) && (year == DateTime.Now.Year))
            {
                result = DateTime.Now;
            }
            else
            {
                if (month == 12)
                {
                    month = 0; year++;
                }
                result = new DateTime(year, month + 1, 1).AddDays(-1);
            }
            return result;
        }
        public static void AddError(ImportErrorCode error, Collection<ImportErrorCode> errorCode)
        {
            if ((error != ImportErrorCode.Ok) && (!errorCode.Contains(error)))
            {
                errorCode.Add(error);
            }
        }
        #endregion

        public static void LogMessage(string s)
        {
            FileHelper.WriteLineToFileText("autoReceiveVehicle.log", string.Concat(DateTime.Now, ": ", s), true);
        }
        public static void LogError(string s)
        {
            FileHelper.WriteLineToFileText("autoReceiveVehicle_error.log", string.Concat(DateTime.Now, ": ", s), true);
        }
    }
}
