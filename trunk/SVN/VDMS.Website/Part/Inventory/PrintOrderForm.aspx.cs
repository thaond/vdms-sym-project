using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using VDMS.Common.Utils;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.PartManagement;
using VDMS.II.PartManagement.Order;
using VDMS.II.Linq;

class PartOrderItem
{
    public string PartCode { get; set; }
    public string PartName { get; set; }
    public string ItemType { get; set; }
    public string Color { get; set; }
    public string Note { get; set; }
    public int Quantity { get; set; }
    public int UnitPrice { get; set; }
    public int Amount { get { return Quantity * UnitPrice; } }

    public PartOrderItem(OrderDetail od)
    {
        this.PartCode = od.PartCode;
        this.Quantity = od.OrderQuantity;
        this.UnitPrice = od.UnitPrice;
    }
}

public partial class Part_PrintOrderForm : BasePage
{
	public long HeaderId
	{
		get { long id; long.TryParse(Request.QueryString["id"], out id); return id; }
	}
	OrderHeader header = null;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (HeaderId > 0)
		{
			crptViewer.ReportSource = LoadForm(HeaderId);
            //if ((header == null) || (header.Status != ST_Status.New))
				lnkBack.NavigateUrl = "~/Part/inventory/Order.aspx";
            //else
            //    lnkBack.NavigateUrl = string.Format("~/Part/inventory/StockTransfer.aspx?id={0}", HeaderId);
		}
	}

    private object LoadForm(long HeaderId)
	{
		ReportDocument rptDoc = ReportFactory.GetReport();
        var dc = DCFactory.GetDataContext<PartDataContext>();

		header = OrderDAO.GetOrderHeader(HeaderId);
		if (header != null)
		{
            rptDoc.Load(HttpContext.Current.Server.MapPath(@"~/Part/Report/Crystal/VMEPOrderSheet.rpt"));

            IList<PartOrderItem> data = header.OrderDetails
                            .Join(dc.Parts.Where(p => p.DatabaseCode == header.Dealer.DatabaseCode),
                                    od => od.PartCode, p => p.PartCode,
                                    (od, p) => new PartOrderItem(od) { PartName = p.VietnamName })
                            .ToList();
			rptDoc.SetDataSource(data);

			string dealer = header.Dealer.DealerName;

			rptDoc.SetParameterValue("OrderDate", header.OrderDate);
			rptDoc.SetParameterValue("Address", header.Warehouse.Address);
			rptDoc.SetParameterValue("Dealer", dealer);
		}

		return rptDoc;
	}
}
