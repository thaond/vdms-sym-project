using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.II.Linq;
using VDMS.Common.Utils;
using VDMS.II.Entity;
using VDMS.II.BasicData;

/// <summary>
/// Summary description for CustomerSaleReport
/// </summary>
public class CustomerSaleReport
{
    public CustomerSaleReport()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    //public static IList<SaleItem> GetCustomerSaleData2(string saleFrom, string saleTo, string ordFrom, string ordTo,
    //                                    string vchFrom, string vchTo, string mvchFrom, string mvchTo,
    //                                    string partCode, string partType, string status, long cusId, long wid)
    //{
    //    DateTime dtsFrom = DataFormat.DateFromString(saleFrom);
    //    DateTime dtsTo = DataFormat.DateFromString(saleTo);
    //    if (dtsTo == DateTime.MinValue) dtsTo = DateTime.Now;
    //    DateTime dtoFrom = DataFormat.DateFromString(ordFrom);
    //    DateTime dtoTo = DataFormat.DateFromString(ordTo);
    //    if (dtoTo == DateTime.MinValue) dtoTo = DateTime.Now;
    //    Warehouse wh = WarehouseDAO.GetWarehouse(wid);
    //    if (wh == null) return null;

    //    var dc = DCFactory.GetDataContext<PartDataContext>();
    //    var parts = dc.Parts.Where(p => p.DatabaseCode == wh.Dealer.DatabaseCode);

    //}
    public static IList<SaleItem> GetCustomerSaleData(string saleFrom, string saleTo, string ordFrom, string ordTo,
                                        string vchFrom, string vchTo, string mvchFrom, string mvchTo,
                                        string partCode, string partType, string status, long cusId, long wid, string dCode)
    {
        DateTime dtsFrom = DataFormat.DateFromString(saleFrom);
        DateTime dtsTo = DataFormat.DateFromString(saleTo);
        if (dtsTo == DateTime.MinValue) dtsTo = DateTime.Now;
        DateTime dtoFrom = DataFormat.DateFromString(ordFrom);
        DateTime dtoTo = DataFormat.DateFromString(ordTo);
        if (dtoTo == DateTime.MinValue) dtoTo = DateTime.Now;
        Dealer dealer = DealerDAO.GetDealerByCode(dCode);
        if (dealer == null) return null;

        var dc = DCFactory.GetDataContext<PartDataContext>();
        var parts = dc.Parts.Where(p => p.DatabaseCode == dealer.DatabaseCode);

        var query = dc.SalesDetails.AsQueryable();

        if (!string.IsNullOrEmpty(saleFrom) || !string.IsNullOrEmpty(saleTo)) query = query.Where(d => d.SalesHeader.SalesDate >= dtsFrom && d.SalesHeader.SalesDate < dtsTo.AddDays(1));
        if (!string.IsNullOrEmpty(ordFrom) || !string.IsNullOrEmpty(ordTo)) query = query.Where(d => d.SalesHeader.OrderDate >= dtoFrom && d.SalesHeader.OrderDate < dtoTo.AddDays(1));

        if (wid > 0) query = query.Where(d => d.SalesHeader.WarehouseId == wid);
        else query = query.Where(d => d.SalesHeader.DealerCode == dCode);

        if (cusId > 0) query = query.Where(d => d.SalesHeader.CustomerId == cusId);
        else if (cusId == -1) query = query.Where(d => d.SalesHeader.CustomerId == null);

        if (!string.IsNullOrEmpty(status)) query = query.Where(d => d.SalesHeader.Status == status);
        if (!string.IsNullOrEmpty(vchFrom)) query = query.Where(d => d.SalesHeader.SalesOrderNumber.CompareTo(vchFrom) >= 0);
        if (!string.IsNullOrEmpty(vchTo)) query = query.Where(d => d.SalesHeader.SalesOrderNumber.CompareTo(vchTo) <= 0);
        if (!string.IsNullOrEmpty(mvchFrom)) query = query.Where(d => d.SalesHeader.SalesOrderNumber.CompareTo(mvchFrom) >= 0);
        if (!string.IsNullOrEmpty(mvchTo)) query = query.Where(d => d.SalesHeader.SalesOrderNumber.CompareTo(mvchTo) <= 0);
        if (!string.IsNullOrEmpty(partCode)) query = query.Where(d => d.PartCode == partCode);
        if (!string.IsNullOrEmpty(partType)) query = query.Where(d => d.PartType == partType);

        var res = query.Where(d => d.PartType == PartType.Part)
                    .Join(parts, d => d.PartCode, p => p.PartCode, (d, p) => new SaleItem(d)
                        {
                            EnglishName = p.EnglishName,
                            VietnameseName = p.VietnamName,
                        }).ToList();
        res.AddRange(query.Where(d => d.PartType == PartType.Accessory)
                    .Select(d => new SaleItem(d)
                        {
                            EnglishName = d.PartInfo.Accessory.EnglishName,
                            VietnameseName = d.PartInfo.Accessory.VietnamName,
                        }).ToList());

        int index = 0, tIndex = 0;
        List<SaleItem> headi = new List<SaleItem>();
        long oldHeadId = 0;
        res = res.OrderBy(p => p.SaleHeaderId).ToList();
        foreach (var item in res)
        {
            if (item.SaleHeaderId != oldHeadId)
            {
                index = 0;
                headi.Add(new SaleItem
                {
                    No = 0,
                    HeadStatus = item.HeadStatus,
                    SaleHeaderId = item.SaleHeaderId,
                    iNo = tIndex + headi.Count,
                    HeadVoucher = item.HeadVoucher,
                    HeadManualVoucher = item.HeadManualVoucher,
                    EnglishName = (item.CustId == 0) ? string.Format("({0})", item.CustName) : "",
                    Quantity = res.Where(i => i.SaleHeaderId == item.SaleHeaderId).Sum(i => i.Quantity),
                    SubAmount = res.Where(i => i.SaleHeaderId == item.SaleHeaderId).Sum(i => i.SubAmount),
                });
                oldHeadId = item.SaleHeaderId;
            }

            //else
            {
                item.HeadManualVoucher = item.HeadStatus = item.HeadVoucher = "";
            }
            item.No = ++index;
            tIndex++;
        }

        foreach (var item in headi)
        {
            res.Insert(item.iNo, item);
        }
        return res;
    }
}
