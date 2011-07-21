using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.I.Linq;
using VDMS.II.Linq;
using VDMS.Helper;
using VDMS.I.Entity;

namespace VDMS.I.Report
{
    public class SummaryOrderStatisticItem
    {
        public string ItemCode { get; set; }
        public int? Quantity { get; set; }
    }
    public class SummaryOrderStatisticReport
    {
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public List<SummaryOrderStatisticItem> Items { get; set; }
        public int TotalItems { get; set; }
    }
    public class DetailOrderStatisticReport
    {
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string ItemCode { get; set; }
        public int Quantity { get; set; }
    }

    [DataObject]
    public class OrderStatisticReport
    {
        public static VehicleDataContext DC
        {
            get
            {
                return DCFactory.GetDataContext<VehicleDataContext>();
            }
        }

        int _DetailOrderReportCount;
        public int CountDetailOrderReport(string area, DateTime from, DateTime to)
        {
            return _DetailOrderReportCount;
        }
        public IQueryable<DetailOrderStatisticReport> GetDetailOrderReport(int maximumRows, int startRowIndex, string area, DateTime from, DateTime to)
        {
            var q = GetQuery(area, from, to);

            var res = q.GroupBy(p => new { p.ItemCode, p.OrderHeader.OrderNumber, p.OrderHeader.OrderDate, p.OrderHeader.DealerCode },
                (g, od) => new DetailOrderStatisticReport()
                {
                    DealerCode = g.DealerCode,
                    DealerName = DealerHelper.GetNameI(g.DealerCode),
                    ItemCode = g.ItemCode,
                    OrderDate = g.OrderDate,
                    OrderNumber = g.OrderNumber,
                    Quantity = od.Sum(d => d.OrderQty),
                }).OrderBy(d => d.OrderNumber).OrderBy(d => d.OrderDate).AsQueryable();

            _DetailOrderReportCount = res.Count();
            if (maximumRows > 0) res = res.Skip(startRowIndex).Take(maximumRows);

            return res;
        }
        public IQueryable<DetailOrderStatisticReport> GetDetailOrderReport(string area, DateTime from, DateTime to)
        {
            return GetDetailOrderReport(-1, -1, area, from, to);
        }

        public static IQueryable<OrderDetail> GetQuery(string area, DateTime from, DateTime to)
        {
            if (area == null) area = "";
            var q =
                DC.OrderDetails.Where(
                    p =>
                    p.OrderHeader.Status != (int) VDMS.I.Vehicle.OrderStatus.Deleted &&
                    p.OrderHeader.DatabaseCode == UserHelper.DatabaseCode && p.OrderHeader.OrderNumber != null &&
                    p.OrderHeader.AreaCode.Contains(area));
            if (from > DateTime.MinValue) q = q.Where(p => p.OrderHeader.OrderDate >= from.Date);
            if (to > DateTime.MinValue) q = q.Where(p => p.OrderHeader.OrderDate < to.Date.AddDays(1));

            return q;
        }
        public static IQueryable<SummaryOrderStatisticItem> GetOrderStatisticItems(string area, DateTime from, DateTime to)
        {
            var q = GetQuery(area, from, to);
            return q.GroupBy(p => p.Item.ItemType, (g, res) => new SummaryOrderStatisticItem() { ItemCode = g }).OrderBy(p => p.ItemCode);
        }

        int _SummaryOrderReportCount;
        public int CountOrderStatisticItemsData(string area, DateTime from, DateTime to)
        {
            return _SummaryOrderReportCount;
        }
        public List<SummaryOrderStatisticReport> GetOrderStatisticItemsData(string area, DateTime from, DateTime to)
        {
            return GetOrderStatisticItemsData(-1, -1, area, from, to);
        }
        public List<SummaryOrderStatisticReport> GetOrderStatisticItemsData(int maximumRows, int startRowIndex, string area, DateTime from, DateTime to)
        {
            DateTime dfrom = from;
            DateTime dto = to;
            if (area == null) area = "";
            var q =
                from qq in DC.OrderDetails
                join o in DC.OrderHeaders on qq.OrderHeaderId equals o.OrderHeaderId
                join i in DC.Items on qq.ItemCode equals i.ItemCode
                where o.Status != (int) VDMS.I.Vehicle.OrderStatus.Deleted &&
                      o.DatabaseCode == UserHelper.DatabaseCode && o.OrderNumber != null &&
                      o.AreaCode.Contains(area)
                      &&(dfrom <= DateTime.MinValue ||o.OrderDate >=dfrom.Date )
                      && (dto <= DateTime.MinValue || o.OrderDate <dto.Date.AddDays(1))
                select new {qq, o, i};


            //var q = GetQuery(area, from, to);
            var items = GetOrderStatisticItems(area, dfrom, dto).ToList();
            var ds = q.GroupBy(p => p.o.DealerCode).Select(p => p.Key).ToList();
            _SummaryOrderReportCount = ds.Count;
            if (maximumRows > 0) ds = ds.Skip(startRowIndex).Take(maximumRows).ToList();
            var qqq = (from qq in q
                      where ds.Contains(qq.o.DealerCode)
                      select new
                                 {
                                     qq.o.DealerCode,
                                     qq.i.ItemType,
                                     qq.i.ItemCode,
                                     qq.qq.OrderQty,
                                     DealerName = DealerHelper.GetNameI(qq.o.DealerCode)
                                 }).ToList();
            List<SummaryOrderStatisticReport> res = new List<SummaryOrderStatisticReport>();

            ds.ForEach(p =>
                {
                    var row = new SummaryOrderStatisticReport()
                    {
                        Items = qqq.Where(d => d.DealerCode == p).GroupBy(d => d.ItemType,
                        (g, rs) => new SummaryOrderStatisticItem { ItemCode = g, Quantity = rs.Sum(od => od.OrderQty) }).ToList(),
                        DealerCode = p,
                        DealerName = DealerHelper.GetNameI(p),
                    };
                    row.Items.AddRange(items.Where(i => !row.Items.Exists(i2 => i.ItemCode == i2.ItemCode)));
                    row.Items = row.Items.OrderBy(i => i.ItemCode).ToList();
                    row.TotalItems = row.Items.Where(i => i.Quantity.HasValue).Sum(i => i.Quantity.Value);
                    res.Add(row);
                });

            return res;
        }
    }
}