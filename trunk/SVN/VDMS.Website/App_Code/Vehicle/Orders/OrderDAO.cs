using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.Helper;
using VDMS.II.Linq;
using VDMS.I.Linq;
using VDMS.I.Entity;
using System.ComponentModel;
using VDMS.Bonus.Entity;

namespace VDMS.I.Vehicle
{
    [DataObject]
    public class OrderDAO
    {
        void LogMessage(string s)
        {
            FileHelper.WriteLineToFileText("SelectOrder.log", string.Concat(DateTime.Now, ": ", s), true);
        }

        int count = 0;
        public int SelectCount(string sFromDate, string sToDate, string DealerCode, string AreaCode, string StatusCode, string orderNumber)
        {
            return count;
        }

        public IQueryable Select(int maximumRows, int startRowIndex, string sFromDate, string sToDate, string DealerCode, string AreaCode, string StatusCode, string orderNumber)
        {
            bool test = ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("test")) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("local")));

            if (test) LogMessage(string.Format("Start select order by {0}: sFromDate = {1}, sToDate = {2}, DealerCode = {3}, AreaCode = {4}, StatusCode = {5}", UserHelper.Username, sFromDate, sToDate, DealerCode, AreaCode, StatusCode));

            DateTime fromDate = UserHelper.ParseDate(sFromDate);
            DateTime toDate = UserHelper.ParseDate(sToDate);

            var db = DCFactory.GetDataContext<VehicleDataContext>();
            var oh = db.OrderHeaders.OrderBy(p => p.OrderDate)
                .Where(h => h.DatabaseCode == UserHelper.DatabaseCode);// && h.OrderDetails.Count() > 0);
            if (test) LogMessage(string.Format("All: {0}", oh.Count()));

            if (!string.IsNullOrEmpty(DealerCode)) oh = oh.Where(p => p.DealerCode == DealerCode);
            if (test) LogMessage(string.Format("Dealer: {0}", oh.Count()));

            if (!string.IsNullOrEmpty(AreaCode)) oh = oh.Where(p => p.AreaCode == AreaCode);
            if (test) LogMessage(string.Format("Area: {0}", oh.Count()));

            if (!string.IsNullOrEmpty(orderNumber)) oh = oh.Where(p => p.OrderNumber.Contains(orderNumber));
            if (test) LogMessage(string.Format("OrderNumber: {0}", oh.Count()));

            if (!string.IsNullOrEmpty(StatusCode))
            {
                if (StatusCode.Length == 1) oh = oh.Where(p => p.Status == int.Parse(StatusCode));
                else oh = oh.Where(p => p.Status == 1 || p.Status == 2 || p.Status == 4);
            }
            if (test) LogMessage(string.Format("Status: {0}", oh.Count()));

            if (fromDate != DateTime.MinValue) oh = oh.Where(p => p.OrderDate >= fromDate);
            if (toDate != DateTime.MinValue) oh = oh.Where(p => p.OrderDate < toDate.AddDays(1));
            if (test) LogMessage(string.Format("Date: {0}", oh.Count()));

            count = oh.Count();
            oh = oh.Skip(startRowIndex).Take(maximumRows);
            if (test) LogMessage(string.Format("Result: {0}", oh.Count()));

            var query = from h in oh
                        select new
                        {
                            h.OrderHeaderId,
                            h.DealerCode,
                            h.AreaCode,
                            h.Status,
                            h.OrderDate,
                            h.OrderTimes,
                            h.ShippingTo,
                            h.SecondaryShippingTo,
                            Items = from d in db.OrderDetails
                                    join i in db.Items on d.ItemCode equals i.ItemCode
                                    where d.OrderHeaderId == h.OrderHeaderId
                                    select new
                                    {
                                        d.ItemCode,
                                        i.ColorName,
                                        d.OrderQty,
                                        d.UnitPrice,
                                        Amount = d.OrderQty * d.UnitPrice,
                                        d.OrderPriority
                                    },
                            SubTotal = h.OrderDetails.Sum(d => d.OrderQty * d.UnitPrice),
                            TotalQuantity = h.OrderDetails.Sum(d => d.OrderQty),
                            h.OrderNumber,
                            h.DealerComment
                        };

            if (test) LogMessage("Done!");
            return query;
        }


        #region payment/bonus

        public static VehicleDataContext DC { get { return DCFactory.GetDataContext<VehicleDataContext>(); } }
        public static VDMS.Bonus.Linq.BonusDataContext BDC { get { return DCFactory.GetDataContext<VDMS.Bonus.Linq.BonusDataContext>(); } }

        int _CountOrderBonus;
        public int CountOrderBonus(DateTime from, DateTime to, string bonusStatus, string dealer)
        {
            return _CountOrderBonus;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable<VDMS.I.Entity.OrderHeader> SelectOrderBonus(int maximumRows, int startRowIndex, DateTime from, DateTime to, string bonusStatus, string dealer)
        {
            var q = DC.OrderHeaders.Where(p=>p.DatabaseCode == UserHelper.DatabaseCode);
            // sale man chi quan ly vung cua minh(admin - ADM: theo DB)
            if (UserHelper.Profile.Position == VDMS.II.Entity.PositionType.Employee && !UserHelper.IsSysAdmin)
                q = q.Where(p => p.AreaCode == UserHelper.AreaCode);

            if (!string.IsNullOrEmpty(bonusStatus)) q = q.Where(p => p.BonusStatus == bonusStatus);
            if (!string.IsNullOrEmpty(dealer)) q = q.Where(p => p.ShippingTo == dealer);
            if (from > DateTime.MinValue) q = q.Where(p => p.OrderDate >= from.Date);
            if (to > DateTime.MinValue) q = q.Where(p => p.OrderDate < to.Date.AddDays(1));
            _CountOrderBonus = q.Count();

            return q.Skip(startRowIndex).Take(maximumRows);
        }

        public static void ChangeBonus(long oid, long amount, bool confirmed, string comment)
        {
            var o = DC.OrderHeaders.SingleOrDefault(p => p.OrderHeaderId == oid);
            if (o != null && VDMS.I.Vehicle.OrderStatusAct.CanChangeBonusStatus(o.Status))
            {
                o.VMEPComment = comment;
                string transComment = string.IsNullOrEmpty(comment) ? o.OrderNumber : comment;

                var bConfirmed = o.BonusStatus == OrderBonusStatus.Confirmed;
                if (bConfirmed != confirmed) // thay doi trang thai
                {
                    if (BonusCloser.IsLock(o.ShippingTo, o.OrderDate)) throw new Exception(string.Format("Bonus for {0} has been locked", o.ShippingTo));

                    string type = confirmed ? BonusTransactionType.ConfirmOrderBonus : BonusTransactionType.UnConfirmOrderBonus;
                    var actAmount = (confirmed) ? -amount : o.BonusAmount;
                    //VDMS.II.BonusSystem.BonusPlans.MakeBonusTrans(BDC, type, o.OrderDate, o.ShippingTo, o.OrderNumber, actAmount, null, null, transComment);

                    o.BonusStatus = confirmed ? OrderBonusStatus.Confirmed : OrderBonusStatus.New;
                }
                else if (o.BonusAmount != amount && confirmed) // ko doi trang thai nhung doi so $
                {
                    var actAmount = o.BonusAmount - amount;
                    //VDMS.II.BonusSystem.BonusPlans.MakeBonusTrans(BDC, BonusTransactionType.ConfirmOrderBonus, o.OrderDate, o.ShippingTo, o.OrderNumber, actAmount, null, null, transComment);
                }
                o.BonusAmount = amount;

                DC.SubmitChanges();
                BDC.SubmitChanges();
            }
        }

        public static VDMS.I.Entity.OrderHeader GetOrder(long oid)
        {
            return DC.OrderHeaders.SingleOrDefault(p => p.OrderHeaderId == oid);
        }
        public static VDMS.I.Entity.OrderHeader GetOrder(string oNum)
        {
            return DC.OrderHeaders.SingleOrDefault(p => p.OrderNumber == oNum);
        }
        //public static IQueryable<OrderHeader> GetOrders(int maximumRows, int startRowIndex, string orderNum, string dCode, DateTime from, DateTime to, string status)
        //{
        //    var q = DC.OrderHeaders.AsQueryable();
        //    if (!string.IsNullOrEmpty(orderNum)) q = q.Where(p => p.OrderNumber == orderNum);
        //    if (!string.IsNullOrEmpty(dCode)) q = q.Where(p => p.DealerCode == dCode);
        //    if (from > DateTime.MinValue) q = q.Where(p => p.SaleOrderPayments.Where(pm => pm.PaymentDate >= from.Date).Count() > 0);
        //    if (to > DateTime.MinValue) q = q.Where(p => p.SaleOrderPayments.Where(pm => pm.PaymentDate < to.Date.AddDays(1)).Count() > 0);
        //    return q;
        //}

        #endregion

        #region statics

        public static void SysInit()
        {
            var dc = new VehicleDataContext();
            try
            {
                var l = dc.OrderHeaders.Where(h => h.OrderDetails.Count() == 0 && !string.IsNullOrEmpty(h.OrderNumber));
                var p = dc.SaleOrderPayments.Where(h => h.OrderHeader.OrderDetails.Count() == 0 && !string.IsNullOrEmpty(h.OrderHeader.OrderNumber));
                dc.SaleOrderPayments.DeleteAllOnSubmit(p);
                dc.OrderHeaders.DeleteAllOnSubmit(l);
                dc.SubmitChanges();
            }
            catch { }
            finally
            {
                dc.Dispose();
            }
        }

        public static bool CanWriteToInterface(long oid)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            IOrderHeader ioh = dc.IOrderHeaders.SingleOrDefault(h => h.OrderHeaderId == oid);
            bool ok = ioh == null;
            if (!ok) ok = ioh.TiptopProcess == "N";

            return ok;
        }
        public static void LockIOrder(long oid)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            IOrderHeader ioh = dc.IOrderHeaders.SingleOrDefault(h => h.OrderHeaderId == oid);
            ioh.TiptopProcess = "Y";
            dc.SubmitChanges();
        }
        public static void UnLockIOrder(long oid)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            IOrderHeader ioh = dc.IOrderHeaders.SingleOrDefault(h => h.OrderHeaderId == oid);
            ioh.TiptopProcess = "N";
            dc.SubmitChanges();
        }
        #endregion
    }
}