using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using VDMS.Common.Utils;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.PartManagement.Sales;

public class SaleReportItem : SalesDetail
{
    public int Quantity { get; set; }
    public int Discount { get; set; }
    public double DiscountAmount { get; set; }
    public double SubAmount { get; set; }

    public SaleReportItem(SalesDetail sd)
    {
        this.PartCode = sd.PartCode;
        this.PartName = sd.PartName;
        this.Quantity = sd.OrderQuantity;
        this.UnitPrice = sd.UnitPrice;
        this.Discount = sd.PercentDiscount;
        this.SubAmount = this.UnitPrice * this.Quantity;
        this.DiscountAmount = this.SubAmount * this.Discount / 100;
        this.SubAmount -= this.DiscountAmount;
    }
}

public partial class Part_PrintSaleForm : BasePage
{
    public long SalesOrderId
    {
        get
        {
            long id;
            long.TryParse(Request.QueryString["id"], out id);
            return id;
        }
    }
    SalesHeader saleHeader = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SalesOrderId > 0)
        {
            crptViewer.ReportSource = LoadSalesOrder(SalesOrderId);
            if ((saleHeader == null) || (saleHeader.Status != OrderStatus.OrderOpen))
                lnkBack.NavigateUrl = "~/Part/Sale.aspx";
            else
                lnkBack.NavigateUrl = string.Format("~/Part/Sale.aspx?id={0}", SalesOrderId);
        }
    }

    private object LoadSalesOrder(long SalesOrderId)
    {
        ReportDocument rptDoc = ReportFactory.GetReport();

        saleHeader = PartSalesDAO.GetSalesHeader(SalesOrderId);
        if (saleHeader != null)
        {
            rptDoc.Load(HttpContext.Current.Server.MapPath(@"~/Part/Report/Crystal/OrderSheet.rpt"));

            IList<SaleReportItem> data = GetReportSource(saleHeader);
            rptDoc.SetDataSource(data);

            string cus = "", vat = "", add = "", phone = "";
            if (saleHeader.CustomerName != null) cus = saleHeader.CustomerName;
            if (saleHeader.Customer != null)
            {
                cus = saleHeader.Customer.Name;
                if (saleHeader.Customer.VATCode != null) vat = saleHeader.Customer.VATCode;
                if (saleHeader.Customer.Contact != null)
                {
                    if (saleHeader.Customer.Contact.Address != null) add = saleHeader.Customer.Contact.Address;
                    phone = saleHeader.Customer.AnyPhone;
                }
            }

            string dealer = saleHeader.Dealer.DealerName;
            string saleman = string.IsNullOrEmpty(UserHelper.Fullname) ? UserHelper.Username : UserHelper.Fullname;
            saleman = saleHeader.SalesPerson;
            string sheet = saleHeader.SalesOrderNumber;

            rptDoc.SetParameterValue("Customer", cus);
            rptDoc.SetParameterValue("SheetNo", sheet);
            rptDoc.SetParameterValue("Dealer", dealer);
            rptDoc.SetParameterValue("SaleMan", saleman);
            rptDoc.SetParameterValue("VAT", vat);
            rptDoc.SetParameterValue("Address", add);
            rptDoc.SetParameterValue("Phone", phone);
            rptDoc.SetParameterValue("Note", string.IsNullOrEmpty(saleHeader.SalesComment) ? "" : saleHeader.SalesComment);
        }

        return rptDoc;
    }

    private IList<SaleReportItem> GetReportSource(SalesHeader sh)
    {
        return sh.SalesDetails.Select(sd => new SaleReportItem(sd)).ToList();
    }
}
