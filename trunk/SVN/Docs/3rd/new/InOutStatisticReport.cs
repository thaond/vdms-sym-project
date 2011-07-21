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

        public static IQueryable<SaleTransHistory> GetInOutTransactions(string area, DateTime from, DateTime to)
        {
            var dl = DC.Dealers.Where(d => d.DatabaseCode == UserHelper.DatabaseCode).Select(d => new { d.AreaCode, d.DealerCode });
            var trans = DC.SaleTransHistories.Where(t => t.TransactionDate >= from && 
                                                         t.TransactionDate < to && 
                                                         (t.TransactionType == (int)ItemStatus.Imported ||
                                                         t.TransactionType == (int)ItemStatus.Moved ||
                                                         t.TransactionType == (int)ItemStatus.Sold ||
                                                         t.TransactionType == (int)ItemStatus.Return));

            if (!String.IsNullOrEmpty(area)) dl = dl.Where(d => d.AreaCode == area);
            var dealers = dl.Select(d => d.DealerCode).ToList();
            trans = trans.Where(t => dealers.Contains(t.ItemInstance.DealerCode));

            return trans;
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
            //var q = GetShippingQuery(area, from, to);
            var q = GetInOutTransactions(area, from, to);
            return q.GroupBy(p => p.ItemInstance.ItemType, (g, res) => new SummaryInOutStatisticItem() { ItemCode = g }).OrderBy(p => p.ItemCode);
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
        private static Boolean IsImportedItem(int status)
        {
            switch (status)
            {
                case (int)ItemStatus.Imported: return true;
                case (int)ItemStatus.Moved: return false;
                case (int)ItemStatus.Return: return false;
                case (int)ItemStatus.Sold: return false;
                default: return false;
            };
        }
        public List<SummaryInOutStatisticReport> GetInOutStatisticItemsData(int maximumRows, int startRowIndex, string area, DateTime from, DateTime to)
        {
            //var items = GetInOutStatisticItems(area, from, to).ToList();
            //var sd = GetShippingQuery(area, from, to);
            var sd = GetInOutTransactions(area, from, to).ToList();

            var items = sd.GroupBy(p => p.ItemInstance.ItemType, (g, ress) => new SummaryInOutStatisticItem() { ItemCode = g }).OrderBy(p => p.ItemCode);
            var ds = sd.GroupBy(p => p.ItemInstance.DealerCode).Select(p => p.Key).ToList();

            _SummaryInOutReportCount = ds.Count;
            if (maximumRows > 0) ds = ds.Skip(startRowIndex).Take(maximumRows).ToList();


            List<SummaryInOutStatisticReport> res = new List<SummaryInOutStatisticReport>();
            foreach (var p in ds)
            {
                var row = new SummaryInOutStatisticReport()
                {
                    Items = sd.Where(d => d.ItemInstance.DealerCode == p)
                              .GroupBy(d => new { d.ItemInstance.ItemType },
                    (g, rs) => new SummaryInOutStatisticItem
                    {
                        ItemCode = g.ItemType,
                        In = rs.Where(t => IsImportedItem(t.TransactionType)).Count(),
                        Out = rs.Where(s => !IsImportedItem(s.TransactionType)).Count(),
                    }).ToList(),
                    DealerCode = p,
                    DealerName = DealerHelper.GetNameI(p),
                };
                row.Items.AddRange(items.Where(i => !row.Items.Exists(i2 => i2.ItemCode == i.ItemCode)));
                row.Items = row.Items.OrderBy(i => i.ItemCode).ToList();
                row.TotalIn = row.Items.Where(i => i.In.HasValue).Sum(i => i.In.Value);
                row.TotalOut = row.Items.Where(i => i.Out.HasValue).Sum(i => i.Out.Value);
                res.Add(row);
            }

            return res;
        }
    }
}