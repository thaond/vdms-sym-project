using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using VDMS.II.Linq;
using VDMS.II.Security;
using VDMS.WebService.Entity;
using VDMS.Common.Utils;

namespace VDMS.II.PartManagement.Order
{
    public class OrderDAO
    {
        public class OrderRemain
        {
            public IEnumerable<OrderHeader> OrderHeaders { get; set; }
            public string DealerCodeAndName { get; set; }
            public OrderSummary DealerSummary { get; set; }
        }



        #region Remain

        public static bool OrderCanHasRemain(string status, DateTime? deliDate, DateTime? payDate)
        {
            return (status == OrderStatus.OrderVoid || !deliDate.HasValue) && payDate.HasValue && status != OrderStatus.VoidClose;
        }

        public static IQueryable<OrderHeader> GetOrderRemain(string aCode, string orderNo, string oFrom, string oTo, string issueNo, string iFrom, string iTo, string dCode)
        {
            DateTime orderFrom = DataFormat.DateFromString(oFrom);
            DateTime orderTo = DataFormat.DateFromString(oTo);
            DateTime issueFrom = DataFormat.DateFromString(iFrom);
            DateTime issueTo = DataFormat.DateFromString(iTo);
            if (dCode == null) dCode = ""; dCode = dCode.Trim().ToUpper();
            if (orderNo == null) orderNo = ""; orderNo = orderNo.ToUpper();
            if (issueNo == null) issueNo = ""; issueNo = issueNo.ToUpper();

            var dc = DCFactory.GetDataContext<PartDataContext>();
            var query = dc.OrderHeaders.Where(o => o.Dealer.DatabaseCode == UserHelper.DatabaseCode)
                // sua dong nay phai sua ca OrderCanHasRemain
                .Where(o => (o.Status == OrderStatus.OrderVoid || !o.DeliveryDate.HasValue) && o.PaymentDate.HasValue && o.Status != OrderStatus.VoidClose)
                .Where(o => o.TipTopNumber.Contains(orderNo) && o.ToDealer.Contains(dCode));
            if (orderFrom > DateTime.MinValue) query = query.Where(o => o.OrderDate >= orderFrom);
            if (orderTo > DateTime.MinValue) query = query.Where(o => o.OrderDate < orderTo.AddDays(1).Date);
            if (issueFrom > DateTime.MinValue) query = query.Where(o => o.ShippingDate >= issueFrom);
            if (issueTo > DateTime.MinValue) query = query.Where(o => o.ShippingDate < issueTo.AddDays(1).Date);
            if (!string.IsNullOrEmpty(issueNo)) query = query.Where(o => o.ReceiveHeaders.Any(r => r.IssueNumber.Contains(issueNo)));
            if (!string.IsNullOrEmpty(issueNo)) query = query.Where(o => o.Dealer.AreaCode == aCode);

            return query;
        }

        int _DealerHasOrderRemainCount;
        public int CountDealerHasOrderRemain(string aCode, string orderNo, string oFrom, string oTo, string issueNo, string iFrom, string iTo, string dCode)
        {
            return _DealerHasOrderRemainCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<OrderRemain> GetDealerHasOrderRemain(int maximumRows, int startRowIndex, string aCode, string orderNo, string oFrom, string oTo, string issueNo, string iFrom, string iTo, string dCode)
        {
            var query = GetOrderRemain(aCode, orderNo, oFrom, oTo, issueNo, iFrom, iTo, dCode);

            var res = query.GroupBy(p => new { p.Dealer.DealerCode, p.Dealer.DealerName }, (g, lst) => new OrderRemain()
            {
                DealerCodeAndName = g.DealerCode + " - " + g.DealerName,
                OrderHeaders = lst,
            });

            _DealerHasOrderRemainCount = res.Count();

            var res2 = res.ToList();

            TotalOrderSummary = new OrderSummary
            {
                OrderQuantity = res2.Sum(o => o.OrderHeaders.Sum(h => h.OrderDetails.Sum(d => d.OrderQuantity))),
                QuotationQuantity = res2.Sum(o => o.OrderHeaders.Sum(h => h.OrderDetails.Sum(d => d.QuotationQuantity))),
                DelivaryQuantity = res2.Sum(o => o.OrderHeaders.Sum(h => h.OrderDetails.Sum(d => d.DelivaryQuantity)).GetValueOrDefault(0)),
                RemainAmount = res2.Sum(o => o.OrderHeaders.Sum(h => h.OrderDetails.Sum(d => d.RemainAmount))),
                RemainQuantity = res2.Sum(o => o.OrderHeaders.Sum(h => h.OrderDetails.Sum(d => d.RemainQuantity))),
                UnitPrice = res2.Sum(o => o.OrderHeaders.Sum(h => h.OrderDetails.Sum(d => d.UnitPrice))),
            };

            if (maximumRows > 0 && startRowIndex >= 0)
            {
                res = res.Skip(startRowIndex).Take(maximumRows);
            }

            res2 = res.ToList();

            foreach (var r in res2)
            {
                r.OrderHeaders = r.OrderHeaders.ToList();
                foreach (var header in r.OrderHeaders)
                {
                    header.OrderHeaderSummary = new OrderSummary
                    {
                        OrderQuantity = header.OrderDetails.Sum(d => d.OrderQuantity),
                        QuotationQuantity = header.OrderDetails.Sum(d => d.QuotationQuantity),
                        DelivaryQuantity = header.OrderDetails.Sum(d => d.DelivaryQuantity.GetValueOrDefault(0)),
                        RemainQuantity = header.OrderDetails.Sum(d => d.RemainQuantity),
                        UnitPrice = header.OrderDetails.Sum(d => d.UnitPrice),
                        RemainAmount = header.OrderDetails.Sum(d => d.RemainAmount),
                    };
                }
                r.DealerSummary = new OrderSummary
                {
                    OrderQuantity = r.OrderHeaders.Sum(h => h.OrderHeaderSummary.OrderQuantity),
                    QuotationQuantity = r.OrderHeaders.Sum(h => h.OrderHeaderSummary.QuotationQuantity),
                    DelivaryQuantity = r.OrderHeaders.Sum(h => h.OrderHeaderSummary.DelivaryQuantity),
                    RemainQuantity = r.OrderHeaders.Sum(h => h.OrderHeaderSummary.RemainQuantity),
                    UnitPrice = r.OrderHeaders.Sum(h => h.OrderHeaderSummary.UnitPrice),
                    RemainAmount = r.OrderHeaders.Sum(h => h.OrderHeaderSummary.RemainAmount)
                };
            }

            return res2;
        }

        public static OrderSummary TotalOrderSummary { get; set; }


        #endregion

        #region Get List

        int count = 0;
        public int GetCount(string fromDate, string toDate, string dealerCode, string warehouseCode, string orderNumber, string status, string orderType)
        {
            return count;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<OrderInfo> FindAll(string fromDate, string toDate, string dealerCode, string warehouseCode, string orderNumber, string status, string orderType, int maximumRows, int startRowIndex)
        {
            DateTime d1 = UserHelper.ParseDate(fromDate);
            DateTime d2 = UserHelper.ParseDate(toDate);
            long wId = (string.IsNullOrEmpty(warehouseCode)) ? 0 : long.Parse(warehouseCode);
            return FindAll(d1, d2, UserHelper.DealerCode, dealerCode, wId, orderNumber, status, orderType, maximumRows, startRowIndex);
        }
        public List<OrderInfo> FindAll(DateTime d1, DateTime d2, string dealerCode, string toDealer, long warehouseId, string orderNumber, string status, string orderType, int maximumRows, int startRowIndex)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            var query = db.OrderHeaders.Where(p => p.ToDealer == toDealer && p.OrderDate > d1 && p.OrderDate < d2.AddDays(1));
            if (!string.IsNullOrEmpty(dealerCode)) query = query.Where(p => p.DealerCode == dealerCode);
            if (warehouseId > 0) query = query.Where(p => p.ToLocation == warehouseId);
            if (!string.IsNullOrEmpty(orderNumber)) query = query.Where(p => p.TipTopNumber == orderNumber);
            if (!string.IsNullOrEmpty(status)) query = query.Where(p => p.Status == status);
            if (!string.IsNullOrEmpty(orderType)) query = query.Where(p => p.OrderType == orderType);
            var result = from h in query
                         join w in db.ActiveWarehouses on h.ToLocation equals w.WarehouseId
                         select new OrderInfo
                         {
                             OrderDate = h.OrderDate,
                             Address = w.Address,
                             TipTopNumber = h.TipTopNumber,
                             ConfirmDate = h.ConfirmDate,
                             Status = h.Status,
                             CreatedBy = h.CreatedBy,
                             OrderHeaderId = h.OrderHeaderId,
                             TipTopProcessed = h.TipTopProcessed,
                             OrderSource = h.OrderSource
                         };
            count = result.Count();
            return result.OrderByDescending(p => p.OrderDate).Skip(startRowIndex).Take(maximumRows).ToList();
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public void Delete(long OrderHeaderId)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            var h = db.OrderHeaders.SingleOrDefault(p => p.OrderHeaderId == OrderHeaderId);
            if (h.TipTopProcessed == "Y") return;
            db.OrderDetails.DeleteAllOnSubmit(db.OrderDetails.Where(p => p.OrderHeaderId == OrderHeaderId));
            db.OrderHeaders.DeleteOnSubmit(h);
            db.SubmitChanges();

            DeleteInterface(OrderHeaderId);
        }

        public static IQueryable<OrderHeader> FindSubOrders(long id)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            var query = db.OrderHeaders.Where(p => p.ReferenceId == id);
            return query;
        }
        #endregion

        #region Send Order

        static void DeleteInterface(long OrderId)
        {
            var idb = DCFactory.GetDataContext<PartDataContext>();
            idb.IOrderDetails.DeleteAllOnSubmit(idb.IOrderDetails.Where(p => p.VDMSOrderId == OrderId));
            idb.IOrderHeaders.DeleteAllOnSubmit(idb.IOrderHeaders.Where(p => p.VDMSOrderId == OrderId));
            idb.SubmitChanges();
        }

        /// <summary>
        /// Send order, and save to interface
        /// </summary>
        /// <param name="OrderId">The order Id</param>
        /// <param name="Resent">true if order already sent</param>
        public static void SendOrder(long OrderId, bool Resent)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            var oh = db.OrderHeaders.SingleOrDefault(p => p.OrderHeaderId == OrderId);
            if (oh == null || (oh.Status != OrderStatus.OrderOpen && !Resent)) return;

            var idb = DCFactory.GetDataContext<PartDataContext>();
            if (Resent) DeleteInterface(OrderId);

            var ioh = new IOrderHeader
            {
                VDMSOrderId = oh.OrderHeaderId,
                DealerCode = oh.DealerCode,
                DeliveryCode = oh.Warehouse.Code,
                InvoiceCode = oh.DealerCode,
                OrderDate = new DateTime(oh.OrderDate.Year, oh.OrderDate.Month, oh.OrderDate.Day),
                OrderType = oh.OrderType == "N" ? "M" : "S",
                DatabaseCode = UserHelper.DatabaseCode.ToUpper().Replace("HTF", "HTP").Replace("DNF", "DNP"),
                Flag = "NE",
                TipTopProcess = "N"
            };
            foreach (var item in oh.OrderDetails)
            {
                var iod = new IOrderDetail
                {
                    LineNumber = item.LineNumber,
                    PartCode = item.PartCode,
                    OrderQuantity = item.OrderQuantity,
                    Price = 0,
                    TC_VDQ10 = "V",
                    IOrderHeader = ioh,
                    // mvbinh(19/01/2010) 
                    OriginalQty = item.OriginalQty,
                    Quo_Status = item.Quo_Status
                };
            }
            idb.IOrderHeaders.InsertOnSubmit(ioh);
            idb.SubmitChanges();

            oh.Status = OrderStatus.OrderSent;
            oh.OrderDate = DateTime.Now;
            db.SubmitChanges();
        }

        /// <summary>
        /// mvbinh:
        /// This funtion update orderquantity on orderdetail and iorderdetail. Throw "rollback" when occrur error.
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="linenumber"></param>
        /// <param name="partcode"></param>
        /// <param name="orderquantity"></param>
        public static void UpdateOrder(long OrderId, int linenumber, string partcode, int orderquantity)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            OrderDetail od = db.OrderDetails.SingleOrDefault(p => p.OrderHeaderId == OrderId && p.LineNumber == linenumber && p.PartCode == partcode);
            IOrderDetail iod = db.IOrderDetails.SingleOrDefault(p => p.VDMSOrderId == OrderId && p.LineNumber == linenumber && p.PartCode == partcode);
            if (od != null && iod != null)
            {
                if (orderquantity > od.DelivaryQuantity && orderquantity >= 0)
                {
                    System.Data.Common.DbTransaction transaction;
                    db.Connection.Open();
                    transaction = db.Connection.BeginTransaction();
                    db.Transaction = transaction;
                    try
                    {
                        // update vdms
                        od.Quo_Status = PartOrderQuoStatus.OrderEditByDealer;
                        od.OrderQuantity = orderquantity;

                        // update interface
                        db.IOrderDetails.DeleteOnSubmit(iod);
                        iod.Quo_Status = PartOrderQuoStatus.OrderEditByDealer;
                        iod.OrderQuantity = orderquantity;
                        db.IOrderDetails.InsertOnSubmit(iod);

                        db.SubmitChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
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
        }
        #endregion

        #region Order Notice

        public int CountPaymentLate = 0;
        public int GetCountPaymentLate()
        {
            return CountPaymentLate;
        }
        public IQueryable PaymentLate(int maximumRows, int startRowIndex)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            var ps = VDMSSetting.CurrentSetting.PaymentSpan;
            var query = from h in db.OrderHeaders
                        where h.QuotationDate != h.OrderDate && h.QuotationDate < DateTime.Now.AddDays(-ps) &&
                              h.DealerCode == UserHelper.DealerCode &&
                              !h.PaymentDate.HasValue &&
                              h.Status != "VD"
                        select new
                        {
                            h.OrderHeaderId,
                            h.TipTopNumber,
                            h.OrderDate,
                            h.QuotationDate,
                            PaymentSpan = ps,
                            h.Amount
                        };
            CountPaymentLate = query.Count();
            return query.Skip(startRowIndex).Take(maximumRows);
        }

        public int CountReceiveLate = 0;
        public int GetCountReceiveLate()
        {
            return CountReceiveLate;
        }
        public IQueryable ReceiveLate(int maximumRows, int startRowIndex)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            var ss = VDMSSetting.CurrentSetting.ShippingSpan;
            var query = (from s in db.IShippings
                         join h in db.OrderHeaders on s.VDMSOrderId equals h.OrderHeaderId
                         where s.ShippingDate < DateTime.Now.AddDays(-ss)
                                 && h.DealerCode == UserHelper.DealerCode
                                 && !
                                     (from t0 in db.ReceiveHeaders
                                      select new
                                      {
                                          t0.IssueNumber
                                      }).Contains(new { s.IssueNumber })
                         select new
                         {
                             s.IssueNumber,
                             s.ShippingDate,
                             s.VDMSOrderId,
                             ShippingSpan = ss
                         });
            query = query.Distinct();
            CountReceiveLate = query.Count();
            return query.Skip(startRowIndex).Take(maximumRows).Distinct(); // Loi Devart chang ??

            //return (from s in db.IShippings
            //        join h in db.OrderHeaders on s.VDMSOrderId equals h.OrderHeaderId
            //        where s.ShippingDate < DateTime.Now.AddDays(-ss)
            //                && h.DealerCode == UserHelper.DealerCode
            //                && !
            //                    (from t0 in db.ReceiveHeaders
            //                     select new
            //                     {
            //                         t0.IssueNumber
            //                     }).Contains(new { s.IssueNumber })
            //        select new
            //        {
            //            s.IssueNumber,
            //            s.ShippingDate,
            //            s.VDMSOrderId,
            //            ShippingSpan = ss
            //        }).Distinct();

            //return from h in db.OrderHeaders
            //       where h.ShippingDate != h.OrderDate && h.ShippingDate < DateTime.Now.AddDays(-ss) && h.DealerCode == UserHelper.DealerCode
            //       select new
            //       {
            //           h.TipTopNumber,
            //           h.ShippingDate,
            //           ShippingSpan = ss
            //       };
        }
        #endregion

        #region Order Detail

        int detailCount = 0;
        public int GetDetailCount(long OrderHeaderId)
        {
            return detailCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable GetAllDetail(long OrderHeaderId, int maximumRows, int startRowIndex)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            var query = from h in db.OrderDetails
                        join p in db.Parts on h.PartCode equals p.PartCode
                        where h.OrderHeaderId == OrderHeaderId && p.DatabaseCode == UserHelper.DatabaseCode
                        select new
                        {
                            h.LineNumber,
                            h.PartCode,
                            p.EnglishName,
                            p.VietnamName,
                            h.OrderQuantity,
                            h.QuotationQuantity,
                            h.UnitPrice,
                            h.Status,
                            h.ModifyFlag,
                            Amount = h.QuotationQuantity * h.UnitPrice
                        };
            detailCount = query.Count();
            return query.OrderBy(p => p.LineNumber).Skip(startRowIndex).Take(maximumRows);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable GetAllDetail(long OrderHeaderId)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            var query = from h in db.OrderDetails
                        join p in db.Parts on h.PartCode equals p.PartCode
                        where h.OrderHeaderId == OrderHeaderId && p.DatabaseCode == UserHelper.DatabaseCode
                        select new
                        {
                            h.LineNumber,
                            h.PartCode,
                            p.EnglishName,
                            p.VietnamName,
                            h.OrderQuantity,
                            h.QuotationQuantity,
                            GoodQuantity = h.QuotationQuantity,
                            BrokenQuantity = 0,
                            WrongQuantity = 0,
                            LackQuantity = 0,
                            ReceiveDetailId = 0,
                            DealerComment = string.Empty,
                            h.UnitPrice,
                            h.Status,
                            h.ModifyFlag,
                            Amount = h.QuotationQuantity * h.UnitPrice
                        };
            return query.OrderBy(p => p.LineNumber);
        }
        #endregion

        #region Order Info

        public static OrderHeader GetOrderHeader(long OrderHeaderId)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            return db.OrderHeaders.SingleOrDefault(p => p.OrderHeaderId == OrderHeaderId);
        }

        /// <summary>
        /// Check if dealer already has a sent order that contains the part which not shipped
        /// </summary>
        /// <param name="OrderHeaderId">The Id of new order, be compared with sent order</param>
        /// <returns>true if has, false if not</returns>
        public static long GetOrderDuplicate(long OrderHeaderId)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            var query = (from c in
                             (
                                 (from d0 in db.OrderDetails
                                  where d0.OrderHeaderId == OrderHeaderId
                                  select new
                                  {
                                      d0
                                  }))
                         join d in
                             (
                                 (from d0 in db.OrderDetails
                                  join h in db.OrderHeaders on d0.OrderHeaderId equals h.OrderHeaderId
                                  where h.ToLocation == UserHelper.WarehouseId && h.Status == OrderStatus.OrderSent
                                  select new
                                  {
                                      d0
                                  })) on c.d0.PartCode equals d.d0.PartCode
                         select new
                         {
                             d.d0.OrderHeaderId
                         }).FirstOrDefault();
            if (query == null) return 0;
            return query.OrderHeaderId;
        }

        public static List<string> GetOrderDuplicatePartCode(long OrderHeaderId)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            var query = (from c in
                             (
                                 (from d0 in db.OrderDetails
                                  where d0.OrderHeaderId == OrderHeaderId
                                  select new
                                  {
                                      d0
                                  }))
                         join d in
                             (
                                 (from d0 in db.OrderDetails
                                  join h in db.OrderHeaders on d0.OrderHeaderId equals h.OrderHeaderId
                                  where h.ToLocation == UserHelper.WarehouseId && h.Status == OrderStatus.OrderSent
                                  select new
                                  {
                                      d0
                                  })) on c.d0.PartCode equals d.d0.PartCode
                         select d.d0.PartCode);
            return query.ToList();
        }
        #endregion

        #region Receive Order

        //public static void SaveReceive(List<ReceiveHeader> rh, int OrderHeaderId)
        //{
        //    var db = DCFactory.GetDataContext<PartDataContext>();
        //    db.ReceiveHeaders.InsertAllOnSubmit(rh);
        //    db.SubmitChanges();
        //}

        //public static void ModifyReceive(List<ReceiveDetail> rd)
        //{
        //    var db = DCFactory.GetDataContext<PartDataContext>();
        //    rd.ForEach(p =>
        //    {
        //        int quantity = p.GoodQuantity;
        //        if (p.ReceiveDetailId != 0)
        //        {
        //            var old = db.ReceiveDetails.Single(q => q.ReceiveDetailId == p.ReceiveDetailId);
        //            old.GoodQuantity = p.GoodQuantity;
        //            old.LackQuantity = p.LackQuantity;
        //            old.WrongQuantity = p.WrongQuantity;
        //            old.BrokenQuantity = p.BrokenQuantity;
        //            old.DealerComment = p.DealerComment;
        //            db.SubmitChanges();
        //            quantity -= old.GoodQuantity;
        //        }
        //        if (quantity != 0)
        //            PartDAO.StockAdjust(p.PartCode, "P", UserHelper.DealerCode, UserHelper.WarehouseId, null, DateTime.Now, InventoryAction.NormalImport, 0, quantity, p.DealerComment, string.Empty, null);
        //    });
        //}

        /// <summary>
        /// Close the current order, mark the status
        /// </summary>
        /// <param name="OrderHeaderId">The order Id</param>
        public static void CloseOrder(long OrderHeaderId)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            Dictionary<string, int> order = new Dictionary<string, int>();
            var partOrderList = db.OrderDetails.Where(p => p.OrderHeaderId == OrderHeaderId).ToList();
            var partReceiveList = db.ReceiveDetails.Where(p => p.OrderHeaderId == OrderHeaderId).ToList();
            foreach (var item in partOrderList)
            {
                order.Add(item.PartCode, item.QuotationQuantity);
            }

            foreach (var item in partReceiveList)
            {
                if (order.ContainsKey(item.PartCode))
                    order[item.PartCode] -= item.ShippingQuantity;
            }
            foreach (var item in partOrderList)
            {
                if (order[item.PartCode] == 0) item.Status = "C";
                else if (partReceiveList.Find(p => p.PartCode == item.PartCode) != null) item.Status = "P";
            }
            db.SubmitChanges();
            if (order.Count(p => p.Value > 0) > 0) return; // some part has not ship
            var oh = db.OrderHeaders.Single(p => p.OrderHeaderId == OrderHeaderId);
            oh.AlreadyInStock = true;
            var query = (from ngh in db.NGFormHeaders
                         join rh in db.ReceiveHeaders on ngh.ReceiveHeaderId equals rh.ReceiveHeaderId
                         where rh.OrderHeaderId == OrderHeaderId
                         select ngh.NGFormHeaderId).Count();
            oh.Status = query == 0 ? OrderStatus.OrderClosedNormal : OrderStatus.OrderClosedAbnormal;
            db.SubmitChanges();
        }

        public static void UpdateOrderDelivery(long OrderHeaderId)
        {
            //no need this, trigger was used
            var dc = DCFactory.GetDataContext<PartDataContext>();
            UpdateOrderDelivery(dc, OrderHeaderId);
            dc.SubmitChanges();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="OrderHeaderId"></param>
        public static void UpdateOrderDelivery(PartDataContext dc, long OrderHeaderId)
        {
            var oh = dc.OrderHeaders.SingleOrDefault(o => o.OrderHeaderId == OrderHeaderId);
            if (oh != null)
            {
                foreach (var d in oh.OrderDetails)
                {
                    d.DelivaryQuantity = dc.ReceiveDetails
                                           .Where(rd => rd.OrderHeaderId == OrderHeaderId && rd.PartCode == d.PartCode)
                                           .Sum(rd => rd.ShippingQuantity);
                }
            }
        }

        static NGFormDetail CreateNGItem(string PartCode, int Quantity, string Status, string Comment, NGFormHeader h)
        {
            return new NGFormDetail
            {
                DealerComment = Comment,
                NGFormHeader = h,
                PartCode = PartCode,
                PartStatus = Status,
                RequestQuantity = Quantity
            };
        }

        /// <summary>
        /// Save receive and create NG form if neccesary
        /// </summary>
        /// <param name="rh">The list of receive header. Tip-Top can send multi-receive at one</param>
        /// <param name="rd">The list of receive detail, contains the list of part of all receive</param>
        /// <param name="WarehouseId">The warehouse of receive</param>
        public static void SaveReceive(List<ReceiveHeader> rh, List<ReceiveDetail> rd, long WarehouseId)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();

            // already recevied then return
            if (db.ReceiveHeaders.Count(p => rh.Select(q => q.IssueNumber).Contains(p.IssueNumber)) > 0) return;


            // create NG form
            rh.ForEach(p =>
            {
                if (rd.Count(q => q.IssueNumber == p.IssueNumber && q.ShippingQuantity != q.GoodQuantity) > 0)
                {
                    //var count = db.NGFormHeaders.Count(q => q.NotGoodNumber.Contains("NG-" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString()));
                    var h = new NGFormHeader
                    {
                        CreatedDate = DateTime.Now,
                        Status = NGStatus.Sent,
                        DealerCode = UserHelper.DealerCode,
                        //ReceiveHeaderId = p.ReceiveHeaderId,
                        ReceiveHeader = p,
                        ApproveLevel = 0,
                        NGType = NGType.Normal,
                        NotGoodNumber = "NG-" + p.IssueNumber
                    };
                    var list = new List<NGFormDetail>();
                    foreach (var item in rd.Where(q => q.IssueNumber == p.IssueNumber && q.GoodQuantity != q.ShippingQuantity))
                    {
                        if (item.BrokenQuantity != 0)
                            list.Add(CreateNGItem(item.PartCode, item.BrokenQuantity, PartNGType.Broken, item.DealerComment, h));
                        if (item.WrongQuantity != 0)
                            list.Add(CreateNGItem(item.PartCode, item.WrongQuantity, PartNGType.Wrong, item.DealerComment, h));
                        if (item.LackQuantity != 0)
                            list.Add(CreateNGItem(item.PartCode, item.LackQuantity, PartNGType.Lack, item.DealerComment, h));
                    }

                    //db.NGFormHeaders.InsertOnSubmit(h);
                    //db.SubmitChanges();

                    //db.ReceiveHeaders.Single(q => q.ReceiveHeaderId == p.ReceiveHeaderId).NotGoodNumber = h.NotGoodNumber;
                    //db.SubmitChanges();
                    p.NotGoodNumber = h.NotGoodNumber;

                    MessageDAO.SendNGAlert(h.NotGoodNumber, h.DealerCode);
                }
            });

            // save receive
            db.ReceiveHeaders.InsertAllOnSubmit(rh);
            db.SubmitChanges();

            // update current inventory, inventory day
            rd.ForEach(p =>
            {
                PartDAO.StockAdjust(p.PartCode, "P", UserHelper.DealerCode, WarehouseId, null, DateTime.Now, InventoryAction.NormalImport, p.UnitPrice * p.GoodQuantity, p.GoodQuantity, p.DealerComment, string.Empty, null);
            });
            db.SubmitChanges();
        }

        /// <summary>
        /// Do the auto receive
        /// </summary>
        /// <param name="IssueNumber">The issue number</param>
        public static void AutoReceive(string IssueNumber)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            var rd = new List<ReceiveDetail>();
            var list = db.IShippings.Where(p => p.IssueNumber == IssueNumber).ToList();
            var oh = db.OrderHeaders.Single(p => p.OrderHeaderId == list[0].VDMSOrderId);

            // collect data
            var rh = new ReceiveHeader
            {
                DealerCode = oh.ToDealer,
                HasUndo = false,
                IsAutomatic = true,
                IsLocked = false,
                IssueNumber = IssueNumber,
                OrderHeaderId = oh.OrderHeaderId,
                ReceiveDate = DateTime.Now,
                WarehouseId = oh.ToLocation
            };

            foreach (var item in list)
            {
                if (item.ShippingQuantity != 0)
                {
                    var od = db.OrderDetails.FirstOrDefault(p => p.OrderHeaderId == oh.OrderHeaderId && p.PartCode == item.PartCode);
                    if (od == null) continue;

                    var rcvd = new ReceiveDetail
                    {
                        BrokenQuantity = 0,
                        DealerComment = string.Empty,
                        GoodQuantity = item.ShippingQuantity,
                        LackQuantity = 0,
                        OrderHeaderId = oh.OrderHeaderId,
                        OrderQuantity = od.OrderQuantity,
                        PartCode = item.PartCode,
                        ShippingQuantity = item.ShippingQuantity,
                        WrongQuantity = 0,
                        ReceiveHeader = rh
                    };
                    rd.Add(rcvd);
                }
            }

            db.ReceiveHeaders.InsertOnSubmit(rh);
            db.SubmitChanges();

            // update current inventory, inventory day
            rd.ForEach(p =>
            {
                PartDAO.StockAdjust(p.PartCode, "P", oh.ToDealer, oh.ToLocation, null, DateTime.Now, InventoryAction.NormalImport, p.UnitPrice * p.GoodQuantity, p.GoodQuantity, p.DealerComment, string.Empty, null);
            });
            db.SubmitChanges();

            //// update has in stock flag
            //oh.AlreadyInStock = true;
            //oh.AutoInStockDate = DateTime.Now;
            //oh.Status = OrderStatus.OrderClosedNormal;
            //db.SubmitChanges();
        }

        /// <summary>
        /// Undo the auto receive
        /// </summary>
        /// <param name="IssueNumber"></param>
        public void UndoAutoReceive(string IssueNumber)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            var rh = db.ReceiveHeaders.Single(p => p.IssueNumber == IssueNumber);
            var list = db.ReceiveDetails.Where(p => p.ReceiveHeaderId == rh.ReceiveHeaderId).ToList();

            // undo receive
            list.ForEach(p =>
            {
                PartDAO.StockAdjust(p.PartCode, "P", rh.DealerCode, rh.WarehouseId, null, DateTime.Now, InventoryAction.UndoAutoImport, p.GoodQuantity * p.UnitPrice, -p.GoodQuantity, p.DealerComment, string.Empty, null);
            });
            db.SubmitChanges();

            // update has in stock flag
            var shipping = db.IShippings.Where(p => p.IssueNumber == IssueNumber).ToList();
            shipping.ForEach(p => p.ShippingDate = DateTime.Now);
            db.SubmitChanges();

            // delete all auto receive
            db.ReceiveDetails.DeleteAllOnSubmit(list);
            db.ReceiveHeaders.DeleteOnSubmit(rh);
            db.SubmitChanges();
        }

        //public static void ModifyLack(List<ImportLackInfo> rd, int OrderHeaderId)
        //{
        //    var db = DCFactory.GetDataContext<PartDataContext>();
        //    rd.ForEach(p =>
        //        {
        //            var old = db.ReceiveDetails.Single(q => q.ReceiveDetailId == p.ReceiveDetailId);
        //            var quantity = old.LackQuantity - p.CurrentLack;
        //            old.GoodQuantity += quantity;
        //            old.LackQuantity = p.CurrentLack;
        //            db.SubmitChanges();

        //            PartDAO.StockAdjust(old.PartCode, "P", UserHelper.DealerCode, UserHelper.WarehouseId, null, DateTime.Now, InventoryAction.NormalImport, 0, quantity, p.Comment, string.Empty, null);
        //        });
        //    var oh = db.OrderHeaders.Single(p => p.OrderHeaderId == OrderHeaderId);
        //    oh.Status = OrderStatus.OrderClosedAbnormal;
        //    db.SubmitChanges();
        //}
        #endregion

        #region Over Span

        /// <summary>
        /// Do the auto in stock
        /// </summary>
        public static void AutoInStock()
        {
            var ds = VDMS.II.Data.TipTop.Order.GetOrderOverInStock(DateTime.Now.AddHours(-VDMSSetting.CurrentSetting.AutoInstockSpan));

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                var o = item.Field<string>(3);
                AutoReceive(o);
            }
        }

        /// <summary>
        /// Send message to sales man
        /// </summary>
        public static void OverQuotation()
        {
            //var ds = VDMS.II.Data.TipTop.Order.GetOrderOverQuotation(DateTime.Now.AddHours(-VDMSSetting.CurrentSetting.AutoInstockSpan));
            if (VDMSSetting.CurrentSetting.QuotationSpan == 0) return;
            var db = DCFactory.GetDataContext<PartDataContext>();
            var d = DateTime.Now.AddDays(-VDMSSetting.CurrentSetting.QuotationSpan);
            var query = db.OrderHeaders.Where(p => p.QuotationDate.HasValue == false && p.OrderDate < d && p.SentWarningOverQuotation == false);

            foreach (var item in query)
            {
                item.SentWarningOverQuotation = true;
                db.SubmitChanges();

                var s = string.Format(MessageDAO.OverQuotationMessage, item.OrderHeaderId);
                if (string.IsNullOrEmpty(s)) continue;
                var MessageId = MessageDAO.SaveMessage(s, MessageFlag.SystemMessage, null, null, false);

                var list = ProfileDAO.GetUsernameByProfile("E", "SP"); // Employee of Spare Part
                foreach (var item1 in list)
                {
                    MessageDAO.SendMessage(item1, MessageId); // send message as system
                }
            }
        }

        /// <summary>
        /// Send message to sales man and email to admin
        /// </summary>
        public static void OverShipping()
        {
            if (VDMSSetting.CurrentSetting.ShippingSpan == 0) return;
            var db = DCFactory.GetDataContext<PartDataContext>();
            var d = DateTime.Now.AddDays(-VDMSSetting.CurrentSetting.ShippingSpan);
            var query = db.OrderHeaders.Where(p => p.ShippingDate.HasValue == false && p.PaymentDate < d && p.SentWarningOverShipping == false);

            foreach (var item in query)
            {
                item.SentWarningOverShipping = true;
                db.SubmitChanges();

                var s = string.Format(MessageDAO.OverShippingMessage, item.OrderHeaderId);
                if (string.IsNullOrEmpty(s)) continue;
                var MessageId = MessageDAO.SaveMessage(s, MessageFlag.SystemMessage, null, null, false);

                var list = ProfileDAO.GetUsernameByProfile("M", "SP"); // Manager of Spare Part
                foreach (var item1 in list)
                {
                    MessageDAO.SendMessage(item1, MessageId);
                }

                EmailHelper.SendMail("admin@vmep.com.vn", VDMSSetting.CurrentSetting.OverShippingEmail, "VDMS System Email", string.Format(MessageDAO.OverShippingMail, item.OrderHeaderId));
            }

        }
        #endregion

        #region Receive Info

        public static ReceiveHeader GetReceiveHeaderByOrderHeader(long OrderHeaderId)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            return db.ReceiveHeaders.SingleOrDefault(p => p.OrderHeaderId == OrderHeaderId);
        }

        //public static ReceiveHeader GetReceiveHeader(long ReceiveHeaderId)
        //{
        //    var db = DCFactory.GetDataContext<PartDataContext>();
        //    return db.ReceiveHeaders.SingleOrDefault(p => p.OrderHeaderId == ReceiveHeaderId);
        //}


        //        [DataObjectMethod(DataObjectMethodType.Select)]
        //        public IQueryable GetReceiveDetail(long OrderHeaderId)
        //        {
        //            var db = DCFactory.GetDataContext<PartDataContext>();
        //            var query = from o in db.OrderDetails
        //                        join h in db.ReceiveDetails on new { o.OrderHeaderId, o.PartCode } equals new { h.OrderHeaderId, h.PartCode }
        //                        join p in db.Parts on h.PartCode equals p.PartCode
        //                        where h.OrderHeaderId == OrderHeaderId && p.DatabaseCode == UserHelper.DatabaseCode
        //                        select new
        //                        {
        //                            h.PartCode,
        //                            p.EnglishName,
        //                            p.VietnamName,
        //                            o.OrderQuantity,
        //#warning change from Quotation to ShippingQuantity
        //                            QuotationQuantity = h.ShippingQuantity,
        //                            h.GoodQuantity,
        //                            h.BrokenQuantity,
        //                            h.WrongQuantity,
        //                            h.LackQuantity,
        //                            o.UnitPrice,
        //                            o.LineNumber,
        //                            h.DealerComment,
        //                            h.ReceiveDetailId,
        //                            Amount = o.QuotationQuantity * o.UnitPrice
        //                        };
        //            return query.OrderBy(p => p.LineNumber);
        //        }

        public static int GetNewReceiveCount(long OrderHeaderId)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            var t1 = (from s in db.IShippings
                      where s.VDMSOrderId == OrderHeaderId
                      select s.IssueNumber).ToList().Distinct().Count();
            var t2 = (from h in db.ReceiveHeaders
                      where h.OrderHeaderId == OrderHeaderId
                      select h.IssueNumber).Count();
            return t1 - t2;
        }

        /// <summary>
        /// Get all the shipping data from the interface, and join with order of dealer
        /// </summary>
        /// <param name="OrderHeaderId">The order Id</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<ReceiveHeaderInfo> GetShipping(long OrderHeaderId)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            var oh = OrderDAO.GetOrderHeader(OrderHeaderId);
            if (oh == null) return null;

            // get shipping data from interface
            // see the detail of IShipping member
            var shipping = (from s in db.IShippings
                            join p in db.Parts on s.PartCode equals p.PartCode
                            where s.VDMSOrderId == OrderHeaderId && p.DatabaseCode == oh.Dealer.DatabaseCode
                            select new ReceiveInfo
                            {
                                IssueNumber = s.IssueNumber,
                                PartCode = s.PartCode,
                                ShippingQuantity = s.ShippingQuantity,
                                ShippingDate = s.ShippingDate,
                                LineNumber = s.LineNumber,
                                EnglishName = p.EnglishName,
                                VietnamName = p.VietnamName,
                                OrderQuantity = 0,
                                QuotationQuantity = 0,
                                NotGoodNumber = s.NotGoodNumber
                            }).ToList();

            // get last receive info
            var order = db.OrderDetails.Where(p => p.OrderHeaderId == OrderHeaderId).ToList();
            var receive = (from h in db.ReceiveHeaders
                           join d in db.ReceiveDetails on h.ReceiveHeaderId equals d.ReceiveHeaderId
                           where h.OrderHeaderId == OrderHeaderId
                           select new { h, d }).ToList();

            // join shipping and receive to get all information
            shipping.ForEach(p =>
            {
                var od = order.Single(q => q.PartCode == p.PartCode);
                p.OrderQuantity = od.OrderQuantity;
                p.QuotationQuantity = od.QuotationQuantity;
                p.UnitPrice = od.UnitPrice;
                p.Amount = p.ShippingQuantity * p.UnitPrice;

                var rc = receive.SingleOrDefault(q => q.d.PartCode == p.PartCode && q.h.IssueNumber == p.IssueNumber);
                if (rc != null)
                {
                    p.GoodQuantity = rc.d.GoodQuantity;
                    p.BrokenQuantity = rc.d.BrokenQuantity;
                    p.WrongQuantity = rc.d.WrongQuantity;
                    p.LackQuantity = rc.d.LackQuantity;
                    p.ReceiveDetailId = rc.d.ReceiveDetailId;
                    p.ReceiveHeaderId = rc.h.ReceiveHeaderId;
                    p.DealerComment = rc.d.DealerComment;
                }
                else p.GoodQuantity = p.ShippingQuantity;
            });

            // group by issue number
            var issue = (from l in shipping
                         select new { l.IssueNumber, l.ShippingDate, l.ReceiveHeaderId, l.NotGoodNumber }).Distinct();
            return (from s in issue
                    select new ReceiveHeaderInfo()
                    {
                        IssueNumber = s.IssueNumber,
                        NotGoodNumber = s.NotGoodNumber,
                        ShippingDate = s.ShippingDate,
                        ReceiveHeaderId = s.ReceiveHeaderId,
                        Items = shipping.Where(p => p.IssueNumber == s.IssueNumber).OrderBy(p => p.LineNumber).ToList()
                    }).ToList();
        }

        #endregion

        #region Auto Import Info

        int autoImportCount = 0;
        public int GetAutoImportCount(string fromDate, string toDate, string dealerCode, string orderNumber)
        {
            return autoImportCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable FindAllAutoImport(string fromDate, string toDate, string dealerCode, string orderNumber, int maximumRows, int startRowIndex)
        {
            DateTime d1 = UserHelper.ParseDate(fromDate);
            DateTime d2 = UserHelper.ParseDate(toDate);
            var db = DCFactory.GetDataContext<PartDataContext>();
            var query = from o in db.OrderHeaders
                        join r in db.ReceiveHeaders on o.OrderHeaderId equals r.OrderHeaderId
                        where r.IsAutomatic == true && r.IsLocked == false
                        select new
                        {
                            o.TipTopNumber,
                            o.OrderDate,
                            o.DealerCode,
                            o.ShippingDate,
                            r.ReceiveDate,
                            r.ReceiveHeaderId,
                            r.IssueNumber
                        };
            if (d1 != DateTime.MinValue) query = query.Where(p => p.ReceiveDate > d1);
            if (d2 != DateTime.MinValue) query = query.Where(p => p.ReceiveDate < d1.AddDays(1));
            if (!string.IsNullOrEmpty(orderNumber)) query = query.Where(p => p.TipTopNumber.Contains(orderNumber));
            if (!string.IsNullOrEmpty(dealerCode)) query = query.Where(p => p.DealerCode == dealerCode);
            count = query.Count();
            return query.OrderByDescending(p => p.OrderDate).Skip(startRowIndex).Take(maximumRows);
        }

        #endregion
    }
}