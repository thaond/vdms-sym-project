using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Common.Utils;
using VDMS.Common.Web;

public partial class Part_Report_ShippingSpan : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDateFrom.Text = DataFormat.DateOfFirstDayInMonth(DateTime.Now).ToShortDateString();
            txtDateTo.Text = DateTime.Now.ToShortDateString();
        }
    }
    protected void odsShipping_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {

    }

    public string EvalOverDays(object shspan, object odate, object shdate, object paydate)
    {
        DateTime payDate = (DateTime)paydate;
        DateTime? shDate = (DateTime?)shdate;
        DateTime oDate = (DateTime)odate;
        DateTime startDate = ((shDate == null) || (shDate == oDate)) ? DateTime.Now : shDate.Value;
        int shSpan;
        int.TryParse(shspan.ToString(), out shSpan);

        return (startDate.Subtract(payDate).Days + shSpan).ToString();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GridView1.DataSourceID = odsShipping.ID;
        GridView1.DataBind();
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        bool paged = GridView1.AllowPaging;
        GridLines gl = GridView1.GridLines;
        GridView1.AllowPaging = false;
        odsShipping.EnablePaging = false;
        GridView1.GridLines = GridLines.Both;

        string dtFrom = DataFormat.DateFromString(txtDateFrom.Text).ToString("yyyy_MM_dd");
        string dtTo = DataFormat.DateFromString(txtDateTo.Text).ToString("yyyy_MM_dd");
        string dl = string.IsNullOrEmpty(ddlDBCode.SelectedValue) ? "All" : ddlDBCode.SelectedValue;
        string fileName = string.Format("OverShip.{0}.{1}-{2}.xls", dl, dtFrom, dtTo);

        btnSearch_Click(sender, e);
        if (GridView1.Rows.Count > 0)
        {
            GridView2Excel.Export(GridView1, fileName);
        }

        GridView1.GridLines = gl;
        GridView1.AllowPaging = paged;
        odsShipping.EnablePaging = paged;
    }
}
