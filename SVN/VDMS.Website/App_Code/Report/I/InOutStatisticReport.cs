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
    public class TransactionComparer : IEqualityComparer<SaleTransHistory>
    {
        public bool Equals(SaleTransHistory x, SaleTransHistory y)
        {
            return x.ItemInstanceId == y.ItemInstanceId &&
                   x.ItemInstance.DealerCode == y.ItemInstance.DealerCode &&
                   x.TransactionType == y.TransactionType &&
                   x.TransactionDate == y.TransactionDate;
        }

        public int GetHashCode(SaleTransHistory t)
        {
            return t.GetHashCode();
        }


    }

    public class SummaryInOutStatisticItem
    {
        public string ItemType { get; set; }
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
        public string ItemType { get; set; }
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

            var res = sd.GroupBy(p => new { p.ItemType, p.OrderNumber, p.ShippingHeader.DealerCode },
                (g, rs) => new
                {
                    DealerCode = g.DealerCode,
                    //DealerName = VDMS.Helper.DealerHelper.GetNameI(g.DealerCode),
                    ItemType = g.ItemType,
                    //OrderDate = DC.OrderHeaders.Where(o => o.OrderNumber == g.OrderNumber).FirstOrDefault().OrderDate,
                    OrderNumber = g.OrderNumber,
                    Out = rs.Where(d => d.ItemInstance.Status == (int)ItemStatus.Sold && d.ItemInstance.ReleasedDate >= from.Date && d.ItemInstance.ReleasedDate < to.Date.AddDays(1)).Count(),
                    In = rs.Count()
                }).OrderBy(d => d.OrderNumber).AsQueryable();

            _DetailInOutReportCount = res.Count();
            if (maximumRows > 0) res = res.Skip(startRowIndex).Take(maximumRows);

            var ret = res.Join(DC.OrderHeaders, s => s.OrderNumber, o => o.OrderNumber, (s, o) => new DetailInOutStatisticReport
            {
                DealerCode = s.DealerCode,
                DealerName = VDMS.Helper.DealerHelper.GetNameI(s.DealerCode),
                In = s.In,
                ItemType = s.ItemType,
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

        private static List<int> ListStatusVehicle = new List<int>
                                                  {
                                                      (int) ItemStatus.Imported,
                                                      (int) ItemStatus.Sold,
                                                      (int) ItemStatus.Return
                                                  };
        public static IQueryable<SaleTransHistory> GetInOutTransactions(string area, DateTime dfrom, DateTime dto)
        {
            var dl = DC.Dealers.Where(d => d.DatabaseCode == UserHelper.DatabaseCode).Select(d => new { d.AreaCode, d.DealerCode });
            var trans = DC.SaleTransHistories.Where(t => t.TransactionDate >= dfrom &&
                                                         t.TransactionDate <= dto &&
                                                         (t.TransactionType == (int)ItemStatus.Imported ||
                //t.TransactionType == (int)ItemStatus.Moved ||
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
                        (p.ItemInstance.Status != 0 && p.ItemInstance.ImportedDate >= from.Date ||
                        (p.Status == (int)ItemStatus.Sold && p.ItemInstance.ReleasedDate >= from.Date)));

            if (to > DateTime.MinValue)
                q = q.Where(p => p.ItemInstance.Status != 0 &&
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
        
        public static List<SummaryInOutStatisticItem> GetInOutStatisticItems(string area, DateTime from, DateTime to)
        {
            //var q = GetShippingQuery(area, from, to);
            //var q = GetInOutTransactions(area, from, to);
            //return q.GroupBy(p => p.ItemInstance.Item.ItemType, (g, res) => new SummaryInOutStatisticItem() { ItemType = g }).OrderBy(p => p.ItemType);
            var dfrom = from;
            var dto = to;
            var sd = (from i in DC.ItemInstances
                      where
                            (i.ImportedDate >= dfrom || i.ReleasedDate >= dfrom) &&
                          ListStatusVehicle.Contains(i.Status)
                          &&
                          (from dl in DC.Dealers
                           where
                               dl.DatabaseCode == UserHelper.DatabaseCode &&
                               (string.IsNullOrEmpty(area) || dl.AreaCode == area)
                           select dl.DealerCode
                          ).Contains(i.DealerCode)
                      select new MiniIteminstance
                      {
                          ItemInstanceId = i.ItemInstanceId,
                          ItemType = i.ItemType,
                          ItemCode = i.ItemCode,
                          DealerCode = i.DealerCode,
                          Status = i.Status,
                          ImportedDate = i.ImportedDate,
                          ReleasedDate = i.ReleasedDate
                      }).ToList();
            sd = sd.Where(i => (i.ImportedDate >= dfrom && i.ImportedDate <= dto) || (i.ReleasedDate <= dto && i.ReleasedDate >= dfrom)).Distinct().ToList();
            var items = sd.GroupBy(p => p.ItemType, (g, ress) => new SummaryInOutStatisticItem() { ItemType = g }).OrderBy(p => p.ItemType);
            return items.ToList();
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
                case (int)ItemStatus.Moved: return true;
                case (int)ItemStatus.Return: return false;
                case (int)ItemStatus.Sold: return false;
                default: return false;
            };
        }
        public List<SummaryInOutStatisticReport> GetInOutStatisticItemsData(int maximumRows, int startRowIndex, string area, DateTime from, DateTime to)
        {
            var dfrom = from;
            var dto = to;
            var sd = (from i in DC.ItemInstances
                      where
                            (i.ImportedDate >= dfrom || i.ReleasedDate >= dfrom) &&
                          ListStatusVehicle.Contains(i.Status)
                          &&
                          (from dl in DC.Dealers
                           where
                               dl.DatabaseCode == UserHelper.DatabaseCode &&
                               (string.IsNullOrEmpty(area) || dl.AreaCode == area)
                           select dl.DealerCode
                          ).Contains(i.DealerCode)
                      select new MiniIteminstance
                      {
                          ItemInstanceId = i.ItemInstanceId,
                          ItemType = i.ItemType,
                          ItemCode = i.ItemCode,
                          DealerCode = i.DealerCode,
                          Status = i.Status,
                          ImportedDate = i.ImportedDate,
                          ReleasedDate = i.ReleasedDate
                      }).ToList();
            sd = sd.Where(i => (i.ImportedDate >= dfrom && i.ImportedDate <= dto) || (i.ReleasedDate <= dto && i.ReleasedDate >= dfrom)).Distinct().ToList();
            var items = sd.GroupBy(p => p.ItemType, (g, ress) => new SummaryInOutStatisticItem() { ItemType = g }).OrderBy(p => p.ItemType);
            var ds = sd.GroupBy(p => p.DealerCode).Select(p => p.Key).ToList();

            _SummaryInOutReportCount = ds.Count;
            if (maximumRows > 0) ds = ds.Skip(startRowIndex).Take(maximumRows).ToList();

            

            List<SummaryInOutStatisticReport> res = new List<SummaryInOutStatisticReport>();
            foreach (var p in ds)
            {
                var row = new SummaryInOutStatisticReport()
                {
                    Items = sd.Where(d => d.DealerCode == p)
                              .GroupBy(d => new { d.ItemType },
                    (g, rs) => new SummaryInOutStatisticItem
                    {
                        ItemType = g.ItemType,
                        In = rs.Where(s => s.ItemType == g.ItemType && s.ImportedDate >= dfrom && s.ImportedDate <= dto ).Count(),   
                        Out = rs.Where(s => s.ItemType == g.ItemType && !IsImportedItem(s.Status) && s.ReleasedDate >= dfrom && s.ReleasedDate <= dto).Count(),
                    }).ToList(),
                    DealerCode = p,
                    DealerName = DealerHelper.GetNameI(p),
                };
                row.Items.AddRange(items.Where(i => !row.Items.Exists(i2 => i2.ItemType == i.ItemType)).OrderBy(v => v.ItemType));
                row.Items = row.Items.OrderBy(i => i.ItemType).ToList();
                row.TotalIn = row.Items.Where(i => i.In.HasValue).Sum(i => i.In.Value);
                row.TotalOut = row.Items.Where(i => i.Out.HasValue).Sum(i => i.Out.Value);
                res.Add(row);
            }
            
            return res;
        }
       
    }
    public class MiniIteminstance
    {
        public long ItemInstanceId { get; set; }
        public string ItemType { get; set; }
        public string ItemCode { get; set; }
        public string DealerCode { get; set; }
        public int Status { get; set; }
        public DateTime ImportedDate { get; set; }
        public DateTime ReleasedDate { get; set; }
    }
}