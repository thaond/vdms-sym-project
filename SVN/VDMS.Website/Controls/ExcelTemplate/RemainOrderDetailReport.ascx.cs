using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.PartManagement.Order;
using VDMS.II.Entity;
using System.Data.Linq;
using VDMS.Common.Utils;

public partial class Controls_ExcelTemplate_RemainOrderDetailReport : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected object EvalOrderHeaders(string dCode)
    {
        return OrderDAO.GetOrderRemain(lvExcel.Attributes["ddlArea"], lvExcel.Attributes["txtOrderNo"], lvExcel.Attributes["txtOrderFrom"],
            lvExcel.Attributes["txtOrderTo"], lvExcel.Attributes["txtIssueNo"],
            lvExcel.Attributes["txtIssueFrom"], lvExcel.Attributes["txtIssueTo"], dCode);
    }

    protected string EvalDelivery(object deliveries)
    {
        if (deliveries == null) return "";
        var list = (EntitySet<ReceiveHeader>)deliveries;
        return list.Count() > 0 ? list.Select(h => h.IssueNumber + " (" + h.ReceiveDate.ToShortDateString() + ")").ToArray().ToString(",   ") : "";
    }

    protected void lvExcel_DataBound(object sender, EventArgs e)
    {
        Literal orderQuantityLiteral = (Literal)lvExcel.FindControl("Literal22");
        Literal quotationQuantityLiteral = (Literal)lvExcel.FindControl("Literal23");
        Literal deliveryQuantityLiteral = (Literal)lvExcel.FindControl("Literal24");
        Literal remainQuantityLiteral = (Literal)lvExcel.FindControl("Literal25");
        Literal unitPriceLiteral = (Literal)lvExcel.FindControl("Literal26");
        Literal remainAmountLiteral = (Literal)lvExcel.FindControl("Literal27");

        orderQuantityLiteral.Text = OrderDAO.TotalOrderSummary.OrderQuantity.ToString("N0");
        quotationQuantityLiteral.Text = OrderDAO.TotalOrderSummary.QuotationQuantity.ToString("N0");
        deliveryQuantityLiteral.Text = OrderDAO.TotalOrderSummary.DelivaryQuantity.ToString("N0");
        remainQuantityLiteral.Text = OrderDAO.TotalOrderSummary.RemainQuantity.ToString("N0");
        unitPriceLiteral.Text = OrderDAO.TotalOrderSummary.UnitPrice.ToString("N0");
        remainAmountLiteral.Text = OrderDAO.TotalOrderSummary.RemainAmount.ToString("N0");
    }
}
