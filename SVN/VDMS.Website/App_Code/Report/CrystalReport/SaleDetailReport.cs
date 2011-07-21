using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using VDMS.Common.Utils;
using VDMS.II.Linq;
using VDMS.II.Entity;
using VDMS.II.BasicData;
using VDMS.Helper;
using Resources;

public class AnmSalesDetail
{
    public int UnitPrice { get; set; }
    public string PartCode { get; set; }
    public int PercentDiscount { get; set; }
    public int OrderQuantity { get; set; }
    public long SalesHeaderId { get; set; }
}

public class SaleItem
{
    public int No { get; set; }
    public int iNo { get; set; }

    public DateTime InputDate { get; set; }
    public string PartName { get; set; }
    public string CustName { get; set; }
    public string EnglishName { get; set; }
    public string VietnameseName { get; set; }
    public string PartCode { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal Quantity { get; set; }

    public long SaleHeaderId { get; set; }
    public long CustId { get; set; }
    public string HeadStatus { get; set; }
    public string HeadVoucher { get; set; }
    public string HeadManualVoucher { get; set; }

    public SaleItem()
    {
    }
    public SaleItem(AnmSalesDetail sd)
    {
        this.UnitPrice = sd.UnitPrice;
        this.PartCode = sd.PartCode;
        this.Discount = sd.PercentDiscount;
        this.Quantity = sd.OrderQuantity;
        this.SaleHeaderId = sd.SalesHeaderId;

        this.SubAmount = (this.UnitPrice * this.Quantity);
        this.SubAmount = this.SubAmount - (this.SubAmount * this.Discount / 100);
    }
    public SaleItem(SalesDetail sd)
    {
        this.UnitPrice = sd.UnitPrice;
        this.PartCode = sd.PartCode;
        this.Discount = sd.PercentDiscount;
        this.Quantity = sd.OrderQuantity;
        this.SaleHeaderId = sd.SalesHeaderId;

        this.SubAmount = (this.UnitPrice * this.Quantity);
        this.SubAmount = this.SubAmount - (this.SubAmount * this.Discount / 100);

        if (sd.SalesHeaderId > 0)
        {
            this.CustId = sd.SalesHeader.CustomerId == null ? 0 : (long)sd.SalesHeader.CustomerId;
            this.CustName = (sd.SalesHeader.CustomerId == null) ? sd.SalesHeader.CustomerName : sd.SalesHeader.Customer.Name;
            this.HeadStatus = sd.SalesHeader.Status;
            this.HeadVoucher = sd.SalesHeader.SalesOrderNumber;
            this.HeadManualVoucher = sd.SalesHeader.ManualVoucherNumber;
        }
    }
}

public class SaleDetailReport
{
    public static ReportDocument CreateReportDoc(string fromDate, string toDate, string warehouseId, string dealerCode, string cusId)
    {
        string dealer = "", warehouse = "", saleman = "", address = "", dbCode, customer = "";
        long wId, cId;
        long.TryParse(warehouseId, out wId);
        long.TryParse(cusId, out cId);

        decimal total, srsAmount = 0;
        DateTime dtFrom = DataFormat.DateFromString(fromDate);
        DateTime dtTo = DataFormat.DateFromString(toDate);
        if (dtTo == DateTime.MinValue) dtTo = DateTime.Now;

        if (!string.IsNullOrEmpty(warehouseId))
        {
            Warehouse wh = WarehouseDAO.GetWarehouse(wId);
            if (wh == null) return null;
            warehouse = wh.Code;
            dealer = wh.Dealer.DealerName;
            address = wh.Address;
            dbCode = wh.Dealer.DatabaseCode;
        }
        else
        {
            Dealer d = DealerDAO.GetDealerByCode(dealerCode);
            if (d == null) return null;
            dbCode = d.DatabaseCode;
            dealer = d.DealerName;
            //warehouse = Constants.All;
            if (d.Contact != null) address = d.Contact.Address;
        }

        if (!string.IsNullOrEmpty(cusId))
        {
            Customer c = CustomerDAO.GetCustomer(cId);
            if (c != null) customer = c.ReportInfo;
        }

        if (string.IsNullOrEmpty(address)) address = "";

        ReportDocument rptDoc = ReportFactory.GetReport();
        rptDoc.Load(HttpContext.Current.Server.MapPath(@"~/Part/Report/Crystal/SaleDetailReport.rpt"));


        IList<SaleItem> data = GetReportSource(dtFrom, dtTo, wId, cId, dealerCode, dbCode, out total);
        rptDoc.SetDataSource(data);
        saleman = string.IsNullOrEmpty(UserHelper.Fullname) ? " " : UserHelper.Fullname;

        rptDoc.SetParameterValue("FromDate", dtFrom.ToShortDateString());
        rptDoc.SetParameterValue("ToDate", dtTo.ToShortDateString());
        rptDoc.SetParameterValue("Dealer", dealer);
        rptDoc.SetParameterValue("Warehouse", warehouse);
        rptDoc.SetParameterValue("SaleMan", saleman);
        rptDoc.SetParameterValue("Customer", customer);
        rptDoc.SetParameterValue("SRSAmount", srsAmount);
        rptDoc.SetParameterValue("Address", address);
        rptDoc.SetParameterValue("Total", total + srsAmount);

        return rptDoc;
    }

    public static IList<SaleItem> GetReportSource(DateTime dtFrom, DateTime dtTo, long warehouseId, long cusId, string dealerCode, string dbCode, out decimal total)
    {
        return GetReportSource(dtFrom, dtTo, warehouseId, cusId, dealerCode, dbCode, null, null, null, null, null, null, out total);
    }

    public static Func<string, int, int, int, SalesDetail> makeSalesDetail =
    (c, d, u, q) => new SalesDetail { PartCode = c, PercentDiscount = d, UnitPrice = u, OrderQuantity = q };

    public static IList<SaleItem> GetReportSource(DateTime dtFrom, DateTime dtTo, long warehouseId, long cusId, string dealerCode, string dbCode, string status, string partCode, string vchFrom, string vchTo, string mvchFrom, string mvchTo, out decimal total)
    {
        PartDataContext dc = DCFactory.GetDataContext<PartDataContext>();
        var query = dc.SalesDetails
                        .Where(sd => sd.SalesHeader.SalesDate >= dtFrom && sd.SalesHeader.SalesDate < dtTo.AddDays(1));

        if (!string.IsNullOrEmpty(status)) query = query.Where(sd => sd.SalesHeader.Status == status);
        if (!string.IsNullOrEmpty(partCode)) query = query.Where(sd => sd.PartCode.Contains(partCode));
        if (!string.IsNullOrEmpty(vchTo)) query = query.Where(sd => sd.SalesHeader.SalesOrderNumber.CompareTo(vchTo) <= 0);
        if (!string.IsNullOrEmpty(vchFrom)) query = query.Where(sd => sd.SalesHeader.SalesOrderNumber.CompareTo(vchFrom) >= 0);

        if (warehouseId > 0)
        {
            query = query.Where(sd => sd.SalesHeader.WarehouseId == warehouseId);
        }
        else
        {
            query = query.Where(sd => sd.SalesHeader.DealerCode == dealerCode);
        }

        if (cusId > 0)
        {
            query = query.Where(sd => sd.SalesHeader.CustomerId == cusId);
        }

        var res = query.Where(sd => sd.PartInfo.PartType == PartType.Part)
                        .GroupBy(sd => new { sd.PartCode, sd.UnitPrice, sd.PercentDiscount }, (key, g) => new AnmSalesDetail()
                        {
                            PartCode = key.PartCode,
                            PercentDiscount = key.PercentDiscount,
                            UnitPrice = key.UnitPrice,
                            OrderQuantity = g.Sum(i => i.OrderQuantity),
                        })
                        .Join(dc.Parts.Where(p => p.DatabaseCode == dbCode), sd => sd.PartCode, p => p.PartCode,
                                (sd, p) => new SaleItem(sd) { PartName = p.VietnamName })
                        //.OrderBy(i => i.PartCode)
                        .ToList();
        res.AddRange(query
                        .Where(sd => sd.PartInfo.PartType == PartType.Accessory)
                        .GroupBy(sd => new { sd.PartCode, sd.UnitPrice, sd.PercentDiscount }, (key, g) => new AnmSalesDetail()
                        {
                            PartCode = key.PartCode,
                            PercentDiscount = key.PercentDiscount,
                            UnitPrice = key.UnitPrice,
                            OrderQuantity = g.Sum(i => i.OrderQuantity),
                        })
                        .Join(dc.Accessories.Where(a => a.DealerCode == dealerCode), sd => sd.PartCode, a => a.AccessoryCode,
                            (sd, a) => new SaleItem(sd) { PartName = a.VietnamName })
                        //.OrderBy(i => i.PartCode)
                        .ToList()
                     );
        //SalesDetail
        total = res.Sum(si => si.SubAmount);
        totalAmount = (long)total;
        return res;
    }

    static long totalAmount;

    public static long TotalAmount { get { return totalAmount; } }
}
