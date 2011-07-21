using System;
using System.ComponentModel;
using System.Linq;
using VDMS.Common.Utils;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using VDMS.II.Linq;

namespace VDMS.II.PartManagement.Sales
{
    [DataObject(true)]
    public class PartSales : PartData
    {
        public int? Stock { get; set; }
        public int ReservedStock { get; set; }
        public int AvailableStock { get { return Stock.GetValueOrDefault(0) - ReservedStock + Quantity; } }
        public long SalesDetailId { get; set; }
        public int Discount { get; set; }
        public int Amount { get { return Quantity * UnitPrice / 100 * (100 - Discount); } }
        public int DiscountAmount { get { return Quantity * UnitPrice / 100 * (Discount); } }
    }

    public class PartSalesDAO : SessionPartDAO<PartSales>
    {
        public static void Init()
        {
            key = "PartSales_List";
        }

        /// <summary>
        /// Using when load from excel file
        /// </summary>
        public static void GetInfoId()
        {
            Parts.ForEach(p => GetInfoId(p));
            Parts.RemoveAll(p => p.PartInfoId == 0);
        }

        public static void GetInfoId(PartSales p)
        {
            //var db = DCFactory.GetDataContext<PartDataContext>();
            if (string.IsNullOrEmpty(p.PartType)) p.PartType = "P";
            var db = new PartDataContext();
            var i = db.PartInfos.SingleOrDefault(q => q.PartCode == p.PartCode && q.PartType == p.PartType && q.DealerCode == UserHelper.DealerCode);
            if (i != null)
            {
                p.PartInfoId = i.PartInfoId;
                var s = db.PartSafeties.SingleOrDefault(q => q.PartInfoId == p.PartInfoId && q.WarehouseId == UserHelper.WarehouseId);
                if (s != null) p.Stock = s.CurrentStock;
            }
            else return;
            if (p.PartType == "A")
            {
                var a = db.Accessories.SingleOrDefault(q => q.AccessoryCode == p.PartCode && q.DealerCode == UserHelper.DealerCode);
                if (a != null) p.PartName = UserHelper.IsVietnamLanguage ? a.VietnamName : a.EnglishName;
            }
            else
            {
                var a = db.Parts.SingleOrDefault(q => q.PartCode == p.PartCode && q.DatabaseCode == UserHelper.DatabaseCode);
                if (a != null) p.PartName = UserHelper.IsVietnamLanguage ? a.VietnamName : a.EnglishName;
            }
            try
            {
                db.SubmitChanges();//??????????????????????????????????????????
            }
            catch { }
            db.Dispose();
        }

        ///// <summary>
        ///// Get memory parts price (parts to be sale)
        ///// </summary>
        //public static void GetPrice()
        //{
        //    Parts.ForEach(p =>
        //        {
        //            if (!p.AlreadyGetPrice)
        //            {
        //                p.AlreadyGetPrice = true;
        //                if (p.PartInfoId == 0) return;
        //                if (p.PartType == "P")
        //                    p.UnitPrice = (int)VDMS.Data.TipTop.Part.GetPartPrice(p.PartCode);
        //                else
        //                {
        //                    var db = DCFactory.GetDataContext<PartDataContext>();
        //                    var price = db.PartInfos.Single(u => u.PartInfoId == p.PartInfoId).Price;
        //                    if (price.HasValue) p.UnitPrice = price.Value;
        //                }
        //            }
        //        });
        //}

        /// <summary>
        /// Load sale form from database to memory
        /// </summary>
        /// <param name="OrderId"></param>
        public static void LoadFromDB(long OrderId)
        {
            var list = Parts;
            list.Clear();
            var db = DCFactory.GetDataContext<PartDataContext>();
            var query = from d in db.SalesDetails
                        join p in db.PartInfos on d.PartInfoId equals p.PartInfoId
                        join i in db.PartSafeties on p.PartInfoId equals i.PartInfoId
                        where d.SalesHeaderId == OrderId && i.WarehouseId == UserHelper.WarehouseId
                        select new { d, p, i };
            foreach (var item in query)
            {
                list.Add(new PartSales
                {
                    PartCode = item.d.PartCode,
                    PartType = item.d.PartType,
                    Quantity = item.d.OrderQuantity,
                    UnitPrice = item.d.UnitPrice,
                    PartName = item.d.PartName,
                    Stock = item.i.CurrentStock,
                    ReservedStock = db.SalesDetails.Where(d => d.SalesHeader.Status == OrderStatus.OrderOpen &&
                                                               d.SalesHeader.DealerCode == UserHelper.DealerCode &&
                                                               d.SalesHeader.WarehouseId == UserHelper.WarehouseId &&
                                                               d.PartCode == item.d.PartCode).Sum(d => d.OrderQuantity),
                    Discount = item.d.PercentDiscount,
                    PartInfoId = item.p.PartInfoId,
                    AlreadyGetPrice = true,
                    SalesDetailId = item.d.SalesDetailId
                });
            }
        }

        /// <summary>
        /// Get data for Part sale detail report
        /// </summary>
        /// <param name="dealerCode"></param>
        /// <param name="partType"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public object GetPartSaleDetailReport(string dealerCode, string partType, string fromDate, string toDate)
        {
            DateTime dtFrom = DataFormat.DateFromString(fromDate), dtTo = DataFormat.DateFromString(toDate);
            var db = DCFactory.GetDataContext<PartDataContext>();
            //var query = from h in db.SalesHeaders
            //            join d in db.SalesDetails on h.SalesHeaderId equals d.SalesHeaderId
            //            join i in db.PartInfos.Where(p=>p.PartType==partType)
            //            on d.PartCode equals i.PartCode
            //            where i.PartType == partType
            //            select new { h, d, i };
            //if (!string.IsNullOrEmpty(dealerCode)) query = query.Where(p => p.h.DealerCode == dealerCode);
            //if (dtFrom != DateTime.MinValue) query = query.Where(p => p.h.SalesDate >= dtFrom);
            //if (dtTo != DateTime.MinValue) query = query.Where(p => p.h.SalesDate <= dtTo);
            //var list = from sd in query
            //           group sd by sd.d.PartCode into g
            //           select new
            //           {
            //               PartCode = g.Key,
            //               OrderQuantity = g.Sum(sd => sd.d.OrderQuantity),
            //               Price = g.Sum(sd => sd.d.OrderQuantity * sd.d.UnitPrice)
            //           };
            var saleHs = db.SalesHeaders.AsQueryable();
            if (!string.IsNullOrEmpty(dealerCode)) saleHs = saleHs.Where(sd => sd.DealerCode == dealerCode);
            if (dtFrom != DateTime.MinValue) saleHs = saleHs.Where(sd => sd.SalesDate >= dtFrom);
            if (dtTo != DateTime.MinValue) saleHs = saleHs.Where(sd => sd.SalesDate <= dtTo);

            // get part sale detail
            var query = db.SalesDetails.Where(sd => sd.PartType == partType)
                        .Join(saleHs, sd => sd.SalesHeaderId, sh => sh.SalesHeaderId,
                                (sd, sh) => new
                                {
                                    PartInfoId = sd.PartInfoId,
                                    Quantity = sd.OrderQuantity,
                                    UnitPrice = sd.UnitPrice,
                                })
                        .GroupBy(sd => sd.PartInfoId).Select(g => new
                        {
                            PartInfoId = g.Key,
                            Quantity = g.Sum(sd => sd.Quantity),
                            Price = g.Sum(sd => sd.Quantity * sd.UnitPrice)
                        });

            // mix with partInfo to get part code
            var list = query.Join(db.PartInfos, sd => sd.PartInfoId, pi => pi.PartInfoId,
                                (sd, pi) => new
                                {
                                    PartCode = pi.PartCode,
                                    Quantity = sd.Quantity,
                                    Price = sd.Price,
                                });

            return list;
        }

        /// <summary>
        /// check stock quantity before sale out. Return true for invalid data
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="WarehouseId"></param>
        /// <returns></returns>
        public static bool CheckSaleStockQuantity(long OrderId, long WarehouseId)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            var items = (from sd in db.SalesDetails
                        join ps in db.PartSafeties on sd.PartInfoId equals ps.PartInfoId
                        where ps.WarehouseId == WarehouseId &&
                              sd.SalesHeaderId == OrderId
                        //sd.OrderQuantity > (ps.CurrentStock - db.SalesDetails.Where(d => d.SalesHeader.Status == OrderStatus.OrderOpen &&
                        //                                                                 d.SalesHeader.WarehouseId == WarehouseId &&
                        //                                                                 d.SalesHeader.SalesHeaderId != OrderId &&
                        //                                                                 d.PartCode == sd.PartCode).Sum(d => d.OrderQuantity))
                        select new
                        {
                            sd.OrderQuantity,
                            ps.CurrentStock,
                            ReservedStock = db.SalesDetails.Where(d => d.SalesHeader.Status == OrderStatus.OrderOpen &&
                                                                       d.SalesHeader.WarehouseId == WarehouseId &&
                                                                       d.SalesHeaderId != sd.SalesHeaderId &&
                                                                       d.PartCode == sd.PartCode).Sum(d => d.OrderQuantity)
                        }).ToList();
            items = items.Where(i => i.OrderQuantity > (i.CurrentStock - i.ReservedStock)).ToList();
            return items.Count() > 0;
        }

        /// <summary>
        /// Do sale out saved sale form
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="SalesDate"></param>
        /// <returns></returns>
        public static string SalesOut(long OrderId, DateTime SalesDate)
        {
            if (OrderId <= 0) return null;

            var db = DCFactory.GetDataContext<PartDataContext>();
            var oh = db.SalesHeaders.Single(p => p.SalesHeaderId == OrderId);
            if (oh.Status == OrderStatus.OrderClosedNormal) return "";

            // change sale form status
            oh.Status = OrderStatus.OrderClosedNormal;
            oh.SalesDate = SalesDate;
            var s = oh.SalesOrderNumber;

            // update stock quantity
            var query = db.SalesDetails.Where(p => p.SalesHeaderId == OrderId);
            foreach (var item in query)
            {
                PartDAO.StockAdjust(item.PartCode, item.PartType, UserHelper.DealerCode, UserHelper.WarehouseId, null, SalesDate, InventoryAction.Sales, item.LineTotal, -item.OrderQuantity, string.Empty, oh.SalesOrderNumber, null);
            }

            // submit change to database
            PartDAO.PartDC.SubmitChanges();
            return s;
        }

        /// <summary>
        /// genarate SaleNumber (temporary number, actual number will be set when update to database)
        /// </summary>
        /// <param name="sh"></param>
        /// <returns></returns>
        public static string GenSaleNumber(SalesHeader sh)
        {
            // valid warehouse?
            Warehouse wh = WarehouseDAO.GetWarehouse(sh.WarehouseId);
            if (wh == null) return null;

            return string.Format("SL-{0}-{1}-{2}-", sh.DealerCode, sh.WarehouseId, sh.ModifiedDate.ToString("yyMMdd"));
        }

        /// <summary>
        /// get sale form by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SalesHeader GetSalesHeader(long id)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            return db.SalesHeaders.SingleOrDefault(h => h.SalesHeaderId == id);
        }
    }
}