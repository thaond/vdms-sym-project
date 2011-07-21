using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.I.Linq;
using VDMS.II.Linq;
using VDMS.Helper;
using VDMS.I.Entity;
using VDMS.I.Vehicle;

namespace VDMS.I.Report
{
    public class SummaryInOutStatisticItem
    {
        public string ItemCode { get; set; }
        public int? In { get; set; }
        public int? Out { get; set; }
    }
    public class SummaryInOutStatisticReport
    {
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public List<SummaryInOutStatisticItem> Items { get; set; }
        public int TotalIn { get; set; }
        public int TotalOut { get; set; }
    }
    public class DetailInOutStatisticReport
    {
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string ItemCode { get; set; }
        public int In { get; set; }
        public int Out { get; set; }
    }

    [DataObject]
    public class InOutStatisticReport
    {
        public static VehicleDataContext DC
        {
            get
            {
                return DCFactory.GetDataContext<VehicleDataContext>();
            }
        }

        int _DetailInOutReportCount;
        public int CountDetailInOutReport(string area, DateTime from, DateTime to)
        {
            return _DetailInOutReportCount;
        }
        public IQueryable<DetailInOutStatisticReport> GetDetailInOutReport(int maximumRows, int startRowIndex, string area, DateTime from, DateTime to)
        {
            var sd = GetShippingQuery(area, from, to);

            var res = sd.GroupBy(p => new { p.ItemCode, p.OrderNumber, p.ShippingHeader.DealerCode },
                (g, rs) => new
                {
                    DealerCode = g.DealerCode,
                    //DealerName = VDMS.Helper.DealerHelper.GetNameI(g.DealerCode),
                    ItemCode = g.ItemCode,
                    //OrderDate = DC.OrderHeaders.Where(o => o.OrderNumber == g.OrderNumber).FirstOrDefault().OrderDate,
                    OrderNumber = g.OrderNumber,
                    In = rs.Count(),
                    Out = rs.Where(d => d.Status == (int)ItemStatus.Sold).Count(),
                }).OrderBy(d => d.OrderNumber).AsQueryable();

            _DetailInOutReportCount = res.Count();
            if (maximumRows > 0) res = res.Skip(startRowIndex).Take(maximumRows);

            var ret = res.Join(DC.OrderHeaders, s => s.OrderNumber, o => o.OrderNumber, (s, o) => new DetailInOutStatisticReport
            {
                DealerCode = s.DealerCode,
                DealerName = VDMS.Helper.DealerHelper.GetNameI(s.DealerCode),
                In = s.In,
                ItemCode = s.ItemCode,
                OrderDate = o.OrderDate,
                OrderNumber = s.OrderNumber,
                Out = s.Out,
            });
            return ret;
        }
        public IQueryable<DetailInOutStatisticReport> GetDetailInOutReport(string area, DateTime from, DateTime to)
        {
            return GetDetailInOutReport(-1, -1, area, from, to);
        }

        public static IQueryable<ShippingDetail> GetShippingQuery(string area, DateTime from, DateTime to)
        {
            var q = DC.ShippingDetails.Where(s => s.ProductInstanceId != null);
            var dl = DC.Dealers.Where(d => d.DatabaseCode == UserHelper.DatabaseCode);

            //Original------------------------------------------
            ////if (from > DateTime.MinValue) 
            //q = q.Where(p =>
            //    p.ItemInstance.ImportedDate >= from.Date
            //    || (p.Status == (int)ItemStatus.Sold && p.ItemInstance.ReleasedDate >= from.Date)
            //    );
            //if (to > DateTime.MinValue) q = q.Where(p => p.ItemInstance.ImportedDate < to.Date.AddDays(1)
            //    || (p.Status == (int)ItemStatus.Sold && p.ItemInstance.ReleasedDate < to.Date.AddDays(1)));

            //if (!string.IsNullOrEmpty(area)) dl = dl.Where(d => d.AreaCode == area);
            //----------------------------------------------------

            q = q.Where(p =>
                        (p.ItemInstance.ImportedDate >= from.Date ||
                        (p.Status == (int)ItemStatus.Sold && p.ItemInstance.ReleasedDate >= from.Date)));

            if (to > DateTime.MinValue)
                q = q.Where(p =>
                        (p.ItemInstance.ImportedDate < to.Date.AddDays(1) ||
                        (p.Status == (int)ItemStatus.Sold && p.ItemInstance.ReleasedDate < to.Date.AddDays(1)))
                );

            if (!string.IsNullOrEmpty(area)) dl = dl.Where(d => d.AreaCode == area);

            //q = q.Join(dl, s => s.ShippingHeader.DealerCode, d => d.DealerCode,
            //    (s, d) => s);

            var dlCode = dl.Select(d => d.DealerCode).ToList();

            q = q.Where(p => dlCode.Contains(p.ShippingHeader.DealerCode));

            return q;
        }

        public static IQueryable<SummaryInOutStatisticItem> GetInOutStatisticItems(string area, DateTime from, DateTime to)
        {
            var q = GetShippingQuery(area, from, to);
            return q.GroupBy(p => p.Item.ItemType, (g, res) => new SummaryInOutStatisticItem() { ItemCode = g }).OrderBy(p => p.ItemCode);
        }

        int _SummaryInOutReportCount;
        public int CountInOutStatisticItemsData(string area, DateTime from, DateTime to)
        {
            return _SummaryInOutReportCount;
        }
        public List<SummaryInOutStatisticReport> GetInOutStatisticItemsData(string area, DateTime from, DateTime to)
        {
            return GetInOutStatisticItemsData(-1, -1, area, from, to);
        }
        public List<SummaryInOutStatisticReport> GetInOutStatisticItemsData(int maximumRows, int startRowIndex, string area, DateTime from, DateTime to)
        {
            var items = GetInOutStatisticItems(area, from, to).ToList();
            var sd = GetShippingQuery(area, from, to);
            var ds = sd.GroupBy(p => p.ShippingHeader.DealerCode).Select(p => p.Key).ToList();

            _SummaryInOutReportCount = ds.Count;
            if (maximumRows > 0) ds = ds.Skip(startRowIndex).Take(maximumRows).ToList();


            List<SummaryInOutStatisticReport> res = new List<SummaryInOutStatisticReport>();
            ds.ForEach(p =>
            {
                var row = new SummaryInOutStatisticReport()
                {
                    Items = sd.Where(d => d.ShippingHeader.DealerCode == p)
                              .GroupBy(d => new { d.Item.ItemType },
                    (g, rs) => new SummaryInOutStatisticItem
                    {
                        ItemCode = g.ItemType,
                        In = rs.Count(),
                        Out = rs.Where(s => s.ItemInstance.Status == (int)ItemStatus.Sold).Count(),
                    }).ToList(),
                    DealerCode = p,
                    DealerName = DealerHelper.GetNameI(p),
                };
                row.Items.AddRange(items.Where(i => !row.Items.Exists(i2 => i.ItemCode == i2.ItemCode)));
                row.Items = row.Items.OrderBy(i => i.ItemCode).ToList();
                row.TotalIn = row.Items.Where(i => i.In.HasValue).Sum(i => i.In.Value);
                row.TotalOut = row.Items.Where(i => i.Out.HasValue).Sum(i => i.Out.Value);
                res.Add(row);
            });

            return res;
        }
    }
}