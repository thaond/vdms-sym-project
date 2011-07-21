using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.Common.Utils;

public partial class MPart_Report_CustomerReport : BasePage
{
    decimal totalQuantity, totalAmount;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string EvalSatus(string status)
    {
        var res = "";
        if (!string.IsNullOrEmpty(status))
        {
            var item = ddlStatus.Items.FindByValue(status);
            if (item != null) res = item.Text;
        }
        return res;
    }
    public IList<SaleItem> EvalDetail(object cusId)
    {
        long wid;
        long.TryParse(ddlWarehouse.SelectedValue, out wid);

        IList<SaleItem> res = CustomerSaleReport.GetCustomerSaleData(txtFromDate.Text.Trim(), txtToDate.Text.Trim(), null, null, txtVchFrom.Text.Trim(),
                                    txtVchTo.Text.Trim(), txtMVchFrom.Text.Trim(), txtMVchTo.Text.Trim(), txtPartCode.Text.Trim(), null, ddlStatus.SelectedValue, (long)cusId, wid, UserHelper.DealerCode);
        if (res != null)
        {
            totalQuantity = res.Where(i => i.No == 0).Sum(i => i.Quantity);
            totalAmount = res.Where(i => i.No == 0).Sum(i => i.SubAmount);
        }
        return res;
    }
    protected void lv_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        Literal litQuantity = (Literal)e.Item.FindControl("litQuantity");
        Literal litAmount = (Literal)e.Item.FindControl("litAmount");

        litQuantity.Text = totalQuantity.ToString("N0");
        litAmount.Text = totalAmount.ToString("N0");
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(lv.DataSourceID)) lv.DataSourceID = odsCus.ID;
        else lv.DataBind();
    }
}
