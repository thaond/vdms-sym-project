using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.Entity;
using VDMS.Common.Utils;
using System.Data.Linq;
using VDMS.II.PartManagement.Order;
using VDMS.Common.Web;
using VDMS.II.BasicData;
using VDMS.Helper;

public partial class MPart_Report_RemainingOrderDetailReport : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (UserHelper.IsDealer //&& !UserHelper.IsSysAdmin
            )
        {
            txtDealerCode.Text = UserHelper.DealerCode;
            ddlArea.SelectedValue = UserHelper.AreaCode;
            ddlArea.Enabled = false;
            txtDealerCode.Enabled = false;
        }
        else
        {
            ddlArea.Enabled = true;
            txtDealerCode.Enabled = true;
        }
    }
    protected object EvalOrderHeaders(string dCode)
    {
        return OrderDAO.GetOrderRemain(ddlArea.SelectedValue, txtOrderNo.Text.Trim(), txtOrderFrom.Text.Trim(), txtOrderTo.Text.Trim(), txtIssueNo.Text.Trim(), txtIssueFrom.Text.Trim(), txtIssueTo.Text.Trim(), dCode);
    }

    protected string EvalDelivery(object deliveries)
    {
        if (deliveries == null) return "";
        var list = (EntitySet<ReceiveHeader>)deliveries;
        return list.Count() > 0 ? list.Select(h => h.IssueNumber + " (" + h.ReceiveDate.ToShortDateString() + ")").ToArray().ToString(",   ") : "";
    }
    protected void btnDoReport_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(lv.DataSourceID)) lv.DataSourceID = ods.ID;
        else lv.DataBind();
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        ListView lvExcel = (ListView)Page.LoadControl("~/Controls/ExcelTemplate/RemainOrderDetailReport.ascx").Controls[0];
        lvExcel.Attributes["txtDealerCode"] = txtDealerCode.Text.Trim();
        lvExcel.Attributes["txtIssueFrom"] = txtIssueFrom.Text.Trim();
        lvExcel.Attributes["txtIssueNo"] = txtIssueNo.Text.Trim();
        lvExcel.Attributes["txtIssueTo"] = txtIssueTo.Text.Trim();
        lvExcel.Attributes["txtOrderFrom"] = txtOrderFrom.Text.Trim();
        lvExcel.Attributes["txtOrderNo"] = txtOrderNo.Text.Trim();
        lvExcel.Attributes["txtOrderTo"] = txtOrderTo.Text.Trim();
        lvExcel.Attributes["ddlArea"] = ddlArea.SelectedValue;
        var odao = new OrderDAO();
        //lvExcel.DataSource = odao.FindByCodes(-1, -1, txtDealerCode.Text.Trim(), ddlArea.SelectedValue, "");
        lvExcel.DataSource = odao.GetDealerHasOrderRemain(-1, -1, ddlArea.SelectedValue, txtOrderNo.Text.Trim(), txtOrderFrom.Text.Trim(), txtOrderTo.Text.Trim(),
            txtIssueNo.Text.Trim(), txtIssueFrom.Text.Trim(), txtIssueTo.Text.Trim(), txtDealerCode.Text.Trim());
        if (odao.CountDealerHasOrderRemain(null, null, null, null, null, null, null, null) > 0)
        {
            lvExcel.DataBind();
            GridView2Excel.Export(lvExcel, "RemainOrderDetailReport.xls");
        }
    }

    protected void lv_DataBound(object sender, EventArgs e)
    {
        if (lv.Items.Count > 0)
        {
            lblNoResult.Visible = false;

            Literal orderQuantityLiteral = (Literal)lv.FindControl("Literal22");
            Literal quotationQuantityLiteral = (Literal)lv.FindControl("Literal23");
            Literal deliveryQuantityLiteral = (Literal)lv.FindControl("Literal24");
            Literal remainQuantityLiteral = (Literal)lv.FindControl("Literal25");
            Literal unitPriceLiteral = (Literal)lv.FindControl("Literal26");
            Literal remainAmountLiteral = (Literal)lv.FindControl("Literal27");

            orderQuantityLiteral.Text = OrderDAO.TotalOrderSummary.OrderQuantity.ToString("N0");
            quotationQuantityLiteral.Text = OrderDAO.TotalOrderSummary.QuotationQuantity.ToString("N0");
            deliveryQuantityLiteral.Text = OrderDAO.TotalOrderSummary.DelivaryQuantity.ToString("N0");
            remainQuantityLiteral.Text = OrderDAO.TotalOrderSummary.RemainQuantity.ToString("N0");
            unitPriceLiteral.Text = OrderDAO.TotalOrderSummary.UnitPrice.ToString("N0");
            remainAmountLiteral.Text = OrderDAO.TotalOrderSummary.RemainAmount.ToString("N0");
        }
        else
        {
            lblNoResult.Visible = true;
        }
    }
    protected string StringToShortDateTime(string s)
    {
        if (string.IsNullOrEmpty(s))
            return s;
        try
        {
            return (DateTime.Parse(s)).ToShortDateString();
        }
        catch (Exception)
        {
            return s;
        }
    }
}
