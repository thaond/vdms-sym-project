using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.II.Entity;
using VDMS.Common.Utils;

namespace VDMS.II.PartManagement.Order
{
    public class OrderRemainItem
    {
        public OrderRemainItem()
        {
        }
        public OrderRemainItem(OrderRemainItem refObj)
        {
            this.Delivery = refObj.Delivery;
            this.LineNumber = refObj.LineNumber;
            this.Note = refObj.Note;
            this.OrderQuantity = refObj.OrderQuantity;
            this.PartNameEn = refObj.PartNameEn;
            this.PartNameVn = refObj.PartNameVn;
            this.PackUnit = refObj.PackUnit;
            this.PartCode = refObj.PartCode;
            this.QuotationQuantity = refObj.QuotationQuantity;
            this.UnitPrice = refObj.UnitPrice;
        }

        public int? Delivery { get; set; }

        public int? LineNumber { get; set; }
        public int OrderQuantity { get; set; }
        public int QuotationQuantity { get; set; }
        public long UnitPrice { get; set; }
        public string PartCode { get; set; }
        public string Note { get; set; }
        public string PartNameVn { get; set; }
        public string PartNameEn { get; set; }
        public string PackUnit { get; set; }

        public string PartName { get { return VDMS.Helper.UserHelper.Language == "en-US" ? PartNameEn : PartNameVn; } }
        public int DeliveryQuantity { get { return Delivery.HasValue ? Delivery.Value : 0; } }
        public int RemainQuantity { get { return OrderQuantity - DeliveryQuantity; } }
        public long RemainAmount { get { return UnitPrice * RemainQuantity; } }
    }

    [System.ComponentModel.DataObject]
    public class RemainingOrderReport
    {
        public static IQueryable<OrderDetail> GetBaseQuery(VDMS.II.Linq.PartDataContext dc, string dCode, string issueNo, DateTime oFrom, DateTime oTo, DateTime iFrom, DateTime iTo)
        {
            var query = dc.OrderDetails.Where(p => p.OrderHeader.Dealer.DatabaseCode == VDMS.Helper.UserHelper.DatabaseCode);

            if (!string.IsNullOrEmpty(dCode))
            {
                query = query.Where(p => p.OrderHeader.DealerCode == dCode);
            }
            if (!string.IsNullOrEmpty(issueNo))
            {
                query = query.Where(p => p.OrderHeader.ReceiveHeaders.Any(h => h.IssueNumber == issueNo));
            }
            if (oFrom > DateTime.MinValue)
            {
                query = query.Where(p => p.OrderHeader.OrderDate >= oFrom);
            }
            if (oTo > DateTime.MinValue)
            {
                query = query.Where(p => p.OrderHeader.OrderDate < oTo.AddDays(1).Date);
            }
            if (iFrom > DateTime.MinValue)
            {
                query = query.Where(p => p.OrderHeader.ReceiveHeaders.Any(h => h.ReceiveDate >= iFrom));
            }
            if (iTo > DateTime.MinValue)
            {
                query = query.Where(p => p.OrderHeader.ReceiveHeaders.Any(h => h.ReceiveDate < iTo.AddDays(1).Date));
            }

            return query;
        }

        public static OrderRemainItem SumRemainOrders(string dCode, string issueNo, string orderNo, string oFrom, string oTo, string iFrom, string iTo)
        {
            var dc = VDMS.II.Linq.DCFactory.GetDataContext<VDMS.II.Linq.PartDataContext>();
            var oF = DataFormat.DateFromString(oFrom);
            var oT = DataFormat.DateFromString(oTo);
            var iF = DataFormat.DateFromString(iFrom);
            var iT = DataFormat.DateFromString(iTo);
            var q = GetBaseQuery(dc, dCode, issueNo, oF, oT, iF, iT);
            if (!string.IsNullOrEmpty(orderNo))
            {
                q = q.Where(p => p.OrderHeader.TipTopNumber == orderNo);
            }

            var res = new OrderRemainItem()
            {
                OrderQuantity = q.Sum(p => p.OrderQuantity),
                QuotationQuantity = q.Sum(p => p.QuotationQuantity),
                Delivery = q.Sum(p => p.DelivaryQuantity),
                UnitPrice = q.Sum(p => (long)p.UnitPrice * (p.QuotationQuantity - (p.DelivaryQuantity == null ? 0 : p.DelivaryQuantity.Value))),
            };

            return res;
        }
        public static OrderHeader GetRemainOrderInfo(string dCode, string issueNo, string orderNo, string oFrom, string oTo, string iFrom, string iTo)
        {
            var dc = VDMS.II.Linq.DCFactory.GetDataContext<VDMS.II.Linq.PartDataContext>();
            var oF = DataFormat.DateFromString(oFrom);
            var oT = DataFormat.DateFromString(oTo);
            var iF = DataFormat.DateFromString(iFrom);
            var iT = DataFormat.DateFromString(iTo);
            var q = GetBaseQuery(dc, dCode, issueNo, oF, oT, iF, iT);
            if (!string.IsNullOrEmpty(orderNo))
            {
                q = q.Where(p => p.OrderHeader.TipTopNumber == orderNo);
            }

            var orders = q.GroupBy(d=> d.OrderHeader, (h, g) => h);
            if(orders.Count()!=1) return null;

            var res = orders.ToList()[0];

            return res;
        }

        int _RemainingOrderCount;
        public int CountRemainingOrder(string dCode, string issueNo, string orderNo, DateTime oFrom, DateTime oTo, DateTime iFrom, DateTime iTo)
        {
            return _RemainingOrderCount;
        }

        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
        public IQueryable<OrderRemainItem> GetRemainingOrder(string dCode, string issueNo, string orderNo, DateTime oFrom, DateTime oTo, DateTime iFrom, DateTime iTo)
        {
            var dc = VDMS.II.Linq.DCFactory.GetDataContext<VDMS.II.Linq.PartDataContext>();
            var query = GetBaseQuery(dc, dCode, issueNo, oFrom, oTo, iFrom, iTo);

            var pi = from p in dc.Parts.Where(p => p.DatabaseCode == VDMS.Helper.UserHelper.DatabaseCode)
                     join s in dc.PartSpecifications on p.PartCode equals s.PartCode into pt_join
                     from pt in pt_join.DefaultIfEmpty()
                     select new
                      {
                          p.EnglishName,
                          p.VietnamName,
                          pt.PackUnit,
                          p.PartCode,
                      };

            // co order number => chi tren 1 order ko can group
            if (!string.IsNullOrEmpty(orderNo))
            {
                query = query.Where(p => p.OrderHeader.TipTopNumber == orderNo);
                _RemainingOrderCount = query.Count();

                return query.Join(pi, d => d.PartCode, p => p.PartCode, (d, p) => new OrderRemainItem()
                {
                    PartCode = d.PartCode,
                    UnitPrice = d.UnitPrice,
                    Note = d.Note,
                    LineNumber = d.LineNumber,
                    OrderQuantity = d.OrderQuantity,
                    QuotationQuantity = d.QuotationQuantity,
                    Delivery = d.DelivaryQuantity,
                    PartNameEn = p.EnglishName,
                    PartNameVn = p.VietnamName,
                    PackUnit = p.PackUnit,
                }).OrderBy(p => p.LineNumber);
            }

            // all order => less info
            _RemainingOrderCount = query.Count();
            var res = query
                .Join(pi, d => d.PartCode, p => p.PartCode, (d, p) => new OrderRemainItem()
                {
                    PartCode = d.PartCode,
                    UnitPrice = d.UnitPrice,
                    OrderQuantity = d.OrderQuantity,
                    QuotationQuantity = d.QuotationQuantity,
                    Delivery = d.DelivaryQuantity,

                    PartNameEn = p.EnglishName,
                    PartNameVn = p.VietnamName,
                    PackUnit = p.PackUnit,
                })
            .GroupBy(p => new { p.PartCode, p.UnitPrice, p.PartNameEn, p.PartNameVn, p.PackUnit },
                (group, list) => new OrderRemainItem()
            {
                PartCode = group.PartCode,
                UnitPrice = group.UnitPrice,
                PartNameVn = group.PartNameVn,
                PartNameEn = group.PartNameEn,
                PackUnit = group.PackUnit,
                OrderQuantity = list.Sum(d => d.OrderQuantity),
                QuotationQuantity = list.Sum(d => d.QuotationQuantity),
                Delivery = list.Sum(d => d.Delivery),
            })
            .OrderBy(p => p.PartCode)
            ;

            return res;
        }
    }
}