using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.I.Vehicle;
using VDMS.I.Entity;
using Resources;
using VDMS.Common.Web;
using VDMS.Common.Utils;

public partial class Vehicle_Fin_QueryPayment : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnExport.Enabled = false;

        if (!IsPostBack)
        {
            txtFrom.Text = DataFormat.DateOfFirstDayInMonth(DateTime.Now).ToShortDateString();
            txtTo.Text = DateTime.Now.ToShortDateString();
        }
    }

    protected void btnFind_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(gvPayment.DataSourceID)) gvPayment.DataSourceID = odsIP.ID;
        else gvPayment.DataBind();
    }
    protected void gvPayment_DataBound(object sender, EventArgs e)
    {
        btnExport.Enabled = gvPayment.Rows.Count > 0;
    }
    protected string EvalStatus(object status)
    {
        if (status == null) return "";
        var i = ddlStatus.Items.FindByValue(status.ToString());
        return (i != null) ? i.Text : "";
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        gvPayment.AllowPaging = false;
        odsIP.EnablePaging = false;
        gvPayment.GridLines = GridLines.Both;

        if (string.IsNullOrEmpty(gvPayment.DataSourceID))
        {
            gvPayment.DataSourceID = odsIP.ID;
            gvPayment.DataBind();
        }

        gvPayment.Columns[gvPayment.Columns.Count - 1].Visible = false;

        GridView2Excel.Export(gvPayment, "paymentData.xls");
    }
    protected void rvDate_Load(object sender, EventArgs e)
    {
        RangeValidator rv = (RangeValidator)sender;
        rv.MaximumValue = DateTime.Now.ToShortDateString();
        rv.MinimumValue = new DateTime(2007, 1, 1).ToShortDateString();
    }
    protected void gvPayment_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        e.Cancel = !Page.IsValid;
    }
    protected void cvOrderNum_ServerValidate(object source, ServerValidateEventArgs args)
    {
        var o = OrderDAO.GetOrder(args.Value.Trim().ToUpper());
        args.IsValid = o != null && o.DealerCode == gvPayment.Rows[gvPayment.EditIndex].Cells[0].Text;
    }
}
