using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.PartManagement.Order;
using VDMS.Common.Utils;

public partial class Part_Report_RemainingOrderReport : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        pnOrderInfo.Visible = false;
    }
    protected void gv_DataBound(object sender, EventArgs e)
    {
        var row = gv.FooterRow;
        if (row != null)
        {
            var sum = RemainingOrderReport.SumRemainOrders(txtDealerCode.Text.Trim(), txtIssueNo.Text.Trim(), txtOrderNo.Text.Trim(), txtOrderFrom.Text, txtOrderTo.Text, txtIssueFrom.Text, txtIssueTo.Text);
            row.Cells[3].Text = Resources.Constants.Total;
            row.Cells[4].Text = sum.OrderQuantity.ToString("N0");
            row.Cells[4].CssClass = "number";
            row.Cells[5].Text = sum.QuotationQuantity.ToString("N0");
            row.Cells[5].CssClass = "number";
            row.Cells[6].Text = sum.DeliveryQuantity.ToString("N0");
            row.Cells[6].CssClass = "number";
            row.Cells[7].Text = (sum.QuotationQuantity - sum.DeliveryQuantity).ToString("N0");
            row.Cells[7].CssClass = "number";
            row.Cells[9].Text = sum.UnitPrice.ToString("N0");
            row.Cells[9].CssClass = "number";
        }
    }

    public void ClearOrderInfo()
    {
        litAddress.Text = "";
        litOrder.Text = "";
        litODate.Text = "";
        litDelivery.Text = "";
    }

    protected void btnDoReport_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(gv.DataSourceID))
            gv.DataSourceID = odsOrderRemain.ID;
        else
            gv.DataBind();

        // order info
        var o = RemainingOrderReport.GetRemainOrderInfo(txtDealerCode.Text.Trim(), txtIssueNo.Text.Trim(), txtOrderNo.Text.Trim(), txtOrderFrom.Text, txtOrderTo.Text, txtIssueFrom.Text, txtIssueTo.Text);
        ClearOrderInfo();
        pnOrderInfo.Visible = o != null;
        if (o != null)
        {
            bool hasReceive = o.ReceiveHeaders.Count() > 0;
            litAddress.Text = string.Format("{0} - {1}: {2}", o.Warehouse.Code, o.Dealer.DealerCode, o.Warehouse.Address);
            litOrder.Text = o.TipTopNumber;
            litODate.Text = o.OrderDate.ToShortDateString();
            var s = o.ReceiveHeaders.Select(h => h.IssueNumber + "(" + h.ReceiveDate.ToShortDateString() + ")").ToArray();

            litDelivery.Text = hasReceive? o.ReceiveHeaders.Select(h => h.IssueNumber + " ("+ h.ReceiveDate.ToShortDateString() + ")").ToArray().ToString(",   ") : "";
        }
    }
}

