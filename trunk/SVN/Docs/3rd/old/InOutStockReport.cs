using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using VDMS.Common.Utils;
using VDMS.II.Linq;
using VDMS.II.Entity;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.II.PartManagement;

namespace VDMS.II.Report
{
    public class PartInOutStock
    {
        public int No { get; set; }
        public string PartCode { get; set; }
        public long PartInfoId { get; set; }
        public string PartName { get; set; }
        public decimal Begin { get; set; }
        public decimal BeginMonth { get; set; }
        public decimal BeginDay { get; set; }
        public Inventory BeginInventory { get; set; }
        public decimal In { get; set; }
        public decimal Out { get; set; }
        public decimal Stock { get; set; }
        public decimal InAmount { get; set; }
        public decimal OutAmount { get; set; }
    }
    public class InOutStockReport
    {
        public static ReportDocument CreateReportDoc(string partType, string fromDate, string toDate, string warehouseId, string dealerCode)
        {
            string dealer = "", warehouse = "", saleman = "";
            long wId;
            long.TryParse(warehouseId, out wId);
            DateTime dtFrom = DataFormat.DateFromString(fromDate);
            DateTime dtTo = DataFormat.DateFromString(toDate);
            if (dtTo == DateTime.MinValue) dtTo = DateTime.MaxValue;

            ReportDocument rptDoc = ReportFactory.GetReport();
            rptDoc.Load(HttpContext.Current.Server.MapPath(@"~/Part/Report/Crystal/InOutStockReport.rpt"));

            IList<PartInOutStock> data = InOutStockReport.GetReportSource(partType, dtFrom, dtTo, (wId == 0) ? null : (long?)wId, dealerCode);

            rptDoc.SetDataSource(data);

            Dealer d = DealerDAO.GetDealerByCode(dealerCode);
            if (d != null)
            {
                dealer = d.DealerName;
            }

            Warehouse wh = WarehouseDAO.GetWarehouse(wId);
            if (wh != null)
            {
                warehouse = wh.Address;
                dealer = wh.Dealer.DealerName;
            }

            saleman = string.IsNullOrEmpty(UserHelper.Fullname) ? UserHelper.Username : UserHelper.Fullname;

            rptDoc.SetParameterValue("FromDate", dtFrom.ToShortDateString());
            rptDoc.SetParameterValue("ToDate", dtTo.ToShortDateString());
            rptDoc.SetParameterValue("Dealer", dealer);
            rptDoc.SetParameterValue("Warehouse", warehouse);
            rptDoc.SetParameterValue("SaleMan", saleman);

            return rptDoc;
        }
        //public static ReportDocument CreateReportDoc_old(string partType, string fromDate, string toDate, string warehouseId, string dealerCode)
        //{
        //    string dealer = "", warehouse = "", saleman = "";
        //    long wId;
        //    long.TryParse(warehouseId, out wId);
        //    DateTime dtFrom = DataFormat.DateFromString(fromDate);
        //    DateTime dtTo = DataFormat.DateFromString(toDate);
        //    if (dtTo == DateTime.MinValue) dtTo = DateTime.MaxValue;

        //    ReportDocument rptDoc = ReportFactory.GetReport();
        //    rptDoc.Load(HttpContext.Current.Server.MapPath(@"~/Part/Report/Crystal/InOutStockReport.rpt"));

        //    IList<PartInOutStock> data = InOutStockReport.GetReportSource_old(partType, dtFrom, dtTo, (wId == 0) ? null : (long?)wId, dealerCode);

        //    rptDoc.SetDataSource(data);

        //    Warehouse wh = WarehouseDAO.GetWarehouse(wId);
        //    if (wh != null)
        //    {
        //        //warehouse = wh.Name;
        //        dealer = wh.Dealer.DealerName;
        //    }
        //    saleman = string.IsNullOrEmpty(UserHelper.Fullname) ? " " : UserHelper.Fullname;

        //    rptDoc.SetParameterValue("FromDate", dtFrom.ToShortDateString());
        //    rptDoc.SetParameterValue("ToDate", dtTo.ToShortDateString());
        //    rptDoc.SetParameterValue("Dealer", dealer);
        //    rptDoc.SetParameterValue("Warehouse", warehouse);
        //    rptDoc.SetParameterValue("SaleMan", saleman);

        //    return rptDoc;
        //}

        //public static IList<PartInOutStock> GetReportSource_old(string partType, DateTime dtFrom, DateTime dtTo, long? warehouseId, string dealerCode)
        //{
        //    PartDataContext dc = DCFactory.GetDataContext<PartDataContext>();

        //    Dealer d = DealerDAO.GetDealerByCode(dealerCode);
        //    if ((d == null) || d.DealerCode == "/") return new List<PartInOutStock>();

        //    if (warehouseId != null)
        //    {
        //        Warehouse wh = WarehouseDAO.GetWarehouse((long)warehouseId);
        //        if ((wh == null) || (wh.DealerCode != d.DealerCode)) return new List<PartInOutStock>();
        //    }

        //    //var query = from pif in dc.PartInfos
        //    //            join th in dc.TransactionHistories on pif.PartInfoId equals th.PartInfoId into id_join
        //    //            from p in id_join.DefaultIfEmpty()
        //    //            where p.TransactionDate >= dtFrom && p.TransactionDate <= dtTo && p.WarehouseId == warehouseId
        //    //            group p by p.PartInfoId into g
        //    //            select new PartInOutStock()
        //    //            {
        //    //                PartInfoId = g.Key,
        //    //                BeginInventory = dc.Inventories.SingleOrDefault(piv => piv.PartInfoId == g.Key && piv.Month == month && piv.Year == year),
        //    //                In = g.Where(th => th.Quantity > 0).Sum(th => th.Quantity),
        //    //                Out = g.Where(th => th.Quantity < 0).Sum(th => th.Quantity),
        //    //                InAmount = g.Where(th => th.Quantity > 0).Sum(th => th.ActualCost),
        //    //                OutAmount = g.Where(th => th.Quantity < 0).Sum(th => th.ActualCost),
        //    //            };
        //    var query = dc.PartInfos.Where(pi => pi.PartType == partType);
        //    List<PartInOutStock> list = null;
        //    if (partType == PartType.Part)
        //    {
        //        list = query.Join(dc.Parts.Where(p => p.DatabaseCode == d.DatabaseCode), pi => pi.PartCode, p => p.PartCode,
        //            (pi, p) => new PartInOutStock()
        //            {
        //                PartInfoId = pi.PartInfoId,
        //                PartCode = p.PartCode,
        //                PartName = p.VietnamName,
        //            }).ToList();
        //    }
        //    else
        //    {
        //        list = query.Select(pi => new PartInOutStock()
        //        {
        //            PartInfoId = pi.PartInfoId,
        //            PartCode = pi.PartCode,
        //            PartName = pi.Accessory.VietnamName,
        //        }).ToList();
        //    }

        //    DateTime dtBegin = (dtFrom > DateTime.MinValue) ? new DateTime(dtFrom.Year, dtFrom.Month - 1, 1) : dtFrom;
        //    int i = 1;
        //    list.ForEach(p =>
        //    {
        //        if (warehouseId == null) p.BeginInventory = InventoryDAO.GetPartInventory(p.PartInfoId, dealerCode, dtBegin.Year, dtBegin.Month);
        //        else p.BeginInventory = InventoryDAO.GetPartInventory(p.PartInfoId, (long)warehouseId, dtBegin.Year, dtBegin.Month);

        //        if (p.BeginInventory != null) p.Begin = p.BeginInventory.Quantity;

        //        p.Begin += TransactionDAO.Summarization(p.PartInfoId, dtBegin, dtFrom);
        //        p.In = TransactionDAO.CountIn(p.PartInfoId, warehouseId, dealerCode, dtFrom, dtTo);
        //        p.Out = TransactionDAO.CountOut(p.PartInfoId, warehouseId, dealerCode, dtFrom, dtTo);
        //        p.InAmount = TransactionDAO.SumInAmount(p.PartInfoId, warehouseId, dealerCode, dtFrom, dtTo);
        //        p.OutAmount = TransactionDAO.SumOutAmount(p.PartInfoId, warehouseId, dealerCode, dtFrom, dtTo);
        //        p.Stock = p.Begin + p.In - p.Out;
        //        p.No = i++;
        //    });
        //    return list;
        //}

        public static IList<PartInOutStock> GetReportSource(string partType, DateTime dtFrom, DateTime dtTo, long? warehouseId, string dealerCode)
        {
            List<PartInOutStock> list = null;
            PartDataContext dc = DCFactory.GetDataContext<PartDataContext>();

            DateTime dtBegin = (dtFrom > DateTime.MinValue) ? new DateTime(dtFrom.Year, dtFrom.Month, 1).AddMonths(-1) : dtFrom;

            Dealer d = DealerDAO.GetDealerByCode(dealerCode);
            if ((d == null) || d.DealerCode == "/") return new List<PartInOutStock>();

            if (warehouseId != null)
            {
                #region By warehouse

                Warehouse wh = WarehouseDAO.GetWarehouse((long)warehouseId);
                if ((wh == null) || (wh.DealerCode != d.DealerCode)) return new List<PartInOutStock>();

                var query = dc.PartInfos.Where(pi => pi.PartType == partType && pi.DealerCode == dealerCode)
                                        .Where(pi => pi.PartSafeties.Where(ps => ps.WarehouseId == warehouseId).Count() > 0);

                if (partType == PartType.Part)
                {
                    list = query.Join(dc.Parts.Where(p => p.DatabaseCode == d.DatabaseCode), pi => pi.PartCode, p => p.PartCode,
                        (pi, p) => new PartInOutStock()
                        {
                            PartInfoId = pi.PartInfoId,
                            PartCode = p.PartCode,
                            PartName = p.VietnamName,
                            BeginMonth = pi.Inventories.SingleOrDefault(i => i.WarehouseId == (long)warehouseId && i.Year == dtBegin.Year && i.Month == dtBegin.Month).Quantity,
                            BeginDay = pi.TransactionHistories
                                         .Where(th => th.TransactionDate >= DataFormat.DateOfFirstDayInMonth(dtFrom) && th.TransactionDate < dtFrom)
                                         .Where(th => th.WarehouseId == (long)warehouseId)
                                         .Sum(th => th.Quantity),
                            In = pi.TransactionHistories
                                         .Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo)
                                         .Where(th => th.WarehouseId == (long)warehouseId)
                                         .Where(th => th.Quantity > 0)
                                         .Sum(th => th.Quantity),
                            Out = pi.TransactionHistories
                                        .Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo)
                                        .Where(th => th.WarehouseId == (long)warehouseId)
                                        .Where(th => th.Quantity < 0)
                                        .Sum(th => th.Quantity) * -1,
                            InAmount = pi.TransactionHistories
                                        .Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo)
                                        .Where(th => th.WarehouseId == (long)warehouseId)
                                        .Where(th => th.Quantity > 0)
                                        .Sum(th => th.ActualCost),
                            OutAmount = pi.TransactionHistories
                                        .Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo)
                                        .Where(th => th.WarehouseId == (long)warehouseId)
                                        .Where(th => th.Quantity < 0)
                                        .Sum(th => th.ActualCost),
                        })
                        .OrderBy(p => p.PartCode)
                        .ToList();
                }
                else
                {
                    list = query.Select(pi => new PartInOutStock()
                    {
                        PartInfoId = pi.PartInfoId,
                        PartCode = pi.PartCode,
                        PartName = pi.Accessory.VietnamName,
                        BeginMonth = pi.Inventories.SingleOrDefault(i => i.WarehouseId == (long)warehouseId && i.Year == dtBegin.Year && i.Month == dtBegin.Month).Quantity,
                        BeginDay = pi.TransactionHistories
                                     .Where(th => th.TransactionDate >= DataFormat.DateOfFirstDayInMonth(dtBegin) && th.TransactionDate < dtFrom)
                                     .Where(th => th.WarehouseId == (long)warehouseId)
                                     .Sum(th => th.Quantity),
                        In = pi.TransactionHistories
                                     .Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo)
                                     .Where(th => th.WarehouseId == (long)warehouseId)
                                     .Where(th => th.Quantity > 0)
                                     .Sum(th => th.Quantity),
                        Out = pi.TransactionHistories
                                    .Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo)
                                    .Where(th => th.WarehouseId == (long)warehouseId)
                                    .Where(th => th.Quantity < 0)
                                    .Sum(th => th.Quantity) * -1,
                        InAmount = pi.TransactionHistories
                                    .Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo)
                                    .Where(th => th.WarehouseId == (long)warehouseId)
                                    .Where(th => th.Quantity > 0)
                                    .Sum(th => th.ActualCost),
                        OutAmount = pi.TransactionHistories
                                    .Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo)
                                    .Where(th => th.WarehouseId == (long)warehouseId)
                                    .Where(th => th.Quantity < 0)
                                    .Sum(th => th.ActualCost),
                    })
                    .OrderBy(p => p.PartCode)
                    .ToList();
                }

                #endregion
            }
            else
            {
                #region By dealer

                var query = dc.PartInfos.Where(pi => pi.PartType == partType);

                if (partType == PartType.Part)
                {
                    list = query.Join(dc.Parts.Where(p => p.DatabaseCode == d.DatabaseCode), pi => pi.PartCode, p => p.PartCode,
                        (pi, p) => new PartInOutStock()
                        {
                            PartInfoId = pi.PartInfoId,
                            PartCode = p.PartCode,
                            PartName = p.VietnamName,
                            BeginMonth = pi.Inventories.SingleOrDefault(i => i.DealerCode == dealerCode && i.WarehouseId == null && i.Year == dtBegin.Year && i.Month == dtBegin.Month).Quantity,
                            BeginDay = pi.TransactionHistories
                                         .Where(th => th.TransactionDate >= DataFormat.DateOfFirstDayInMonth(dtBegin) && th.TransactionDate < dtFrom)
                                         .Where(th => th.DealerCode == dealerCode)
                                         .Sum(th => th.Quantity),
                            In = pi.TransactionHistories
                                         .Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo)
                                         .Where(th => th.DealerCode == dealerCode)
                                         .Where(th => th.Quantity > 0)
                                         .Sum(th => th.Quantity),
                            Out = pi.TransactionHistories
                                        .Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo)
                                         .Where(th => th.DealerCode == dealerCode)
                                        .Where(th => th.Quantity < 0)
                                        .Sum(th => th.Quantity) * -1,
                            InAmount = pi.TransactionHistories
                                        .Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo)
                                         .Where(th => th.DealerCode == dealerCode)
                                        .Where(th => th.Quantity > 0)
                                        .Sum(th => th.ActualCost),
                            OutAmount = pi.TransactionHistories
                                        .Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo)
                                         .Where(th => th.DealerCode == dealerCode)
                                        .Where(th => th.Quantity < 0)
                                        .Sum(th => th.ActualCost),
                        }).OrderBy(p => p.PartCode).ToList();
                }
                else
                {
                    list = query.Select(pi => new PartInOutStock()
                    {
                        PartInfoId = pi.PartInfoId,
                        PartCode = pi.PartCode,
                        PartName = pi.Accessory.VietnamName,
                        BeginMonth = pi.Inventories.SingleOrDefault(i => i.DealerCode == dealerCode && i.WarehouseId == null && i.Year == dtBegin.Year && i.Month == dtBegin.Month).Quantity,
                        BeginDay = pi.TransactionHistories
                                     .Where(th => th.TransactionDate >= DataFormat.DateOfFirstDayInMonth(dtBegin) && th.TransactionDate < dtFrom)
                                     .Where(th => th.DealerCode == dealerCode)
                                     .Sum(th => th.Quantity),
                        In = pi.TransactionHistories
                                     .Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo)
                                     .Where(th => th.DealerCode == dealerCode)
                                     .Where(th => th.Quantity > 0)
                                     .Sum(th => th.Quantity),
                        Out = pi.TransactionHistories
                                    .Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo)
                                     .Where(th => th.DealerCode == dealerCode)
                                    .Where(th => th.Quantity < 0)
                                    .Sum(th => th.Quantity) * -1,
                        InAmount = pi.TransactionHistories
                                    .Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo)
                                     .Where(th => th.DealerCode == dealerCode)
                                    .Where(th => th.Quantity > 0)
                                    .Sum(th => th.ActualCost),
                        OutAmount = pi.TransactionHistories
                                    .Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo)
                                     .Where(th => th.DealerCode == dealerCode)
                                    .Where(th => th.Quantity < 0)
                                    .Sum(th => th.ActualCost),
                    }).OrderBy(p => p.PartCode).ToList();
                }

                #endregion
            }

            int index = 1;
            list.ForEach(p =>
            {
                p.Begin = p.BeginMonth + p.BeginDay;
                p.Stock = p.Begin + p.In - p.Out;
                p.No = index++;
            });
            list = list.Where(p => p.Stock != 0).ToList();
            return list;
        }
    }
}