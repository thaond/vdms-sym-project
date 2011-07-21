using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using VDMS.Common.Utils;
using VDMS.Common.Web;

public partial class Part_Report_PartOrderServiceSummary : BasePage
{
    void LoadDealers()
    {
        ddlDealers.DatabaseCode = ddlRegion.SelectedValue;
        ddlDealers.DataBind();
    }
    protected string EvalRate(object divBy, object div)
    {
        var a = Convert.ToInt32(div);
        var b = Convert.ToInt32(divBy);
        if (a == 0) return string.Format("{0}/{1}", b, a);
        else return ((double)b / a * 100).ToString("N2") + "%";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDealers();
            txtFromDate.Text = DataFormat.DateOfFirstDayInMonth(DateTime.Now).ToShortDateString();
            txtToDate.Text = DateTime.Now.ToShortDateString();
        }
    }
    protected void cmdQuery_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(GridView1.DataSourceID)) GridView1.DataSourceID = "odsRateRpt";
        GridView1.DataBind();
    }

    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        long qParts = 0, oParts = 0, qPieces = 0, oPieces = 0;
        GridView grid = (GridView)sender;
        foreach (GridViewRow row in grid.Rows)
        {
            qParts += long.Parse(row.Cells[1].Text);
            oParts += long.Parse(row.Cells[2].Text);
            qPieces += long.Parse(row.Cells[4].Text);
            oPieces += long.Parse(row.Cells[5].Text);
        }
        grid.FooterRow.Cells[0].Text = Constants.Total;

        grid.FooterRow.Cells[1].Text = qParts.ToString();
        grid.FooterRow.Cells[2].Text = oParts.ToString();
        grid.FooterRow.Cells[3].Text = EvalRate(qParts, oParts);
        grid.FooterRow.Cells[4].Text = qPieces.ToString();
        grid.FooterRow.Cells[5].Text = oPieces.ToString();
        grid.FooterRow.Cells[6].Text = EvalRate(qPieces, oPieces);
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        bool paged = GridView1.AllowPaging;
        GridLines gl = GridView1.GridLines;
        GridView1.AllowPaging = false;
        odsRateRpt.EnablePaging = false;
        GridView1.GridLines = GridLines.Both;

        string dtFrom = DataFormat.DateFromString(txtFromDate.Text).ToString("yyyy_MM_dd");
        string dtTo = DataFormat.DateFromString(txtToDate.Text).ToString("yyyy_MM_dd");
        string dl = string.IsNullOrEmpty(ddlDealers.SelectedValue) ? "All" : ddlDealers.SelectedValue;
        string fileName = string.Format("PSV.{3}.{0}.{1}-{2}.xls", dl, dtFrom, dtTo, ddlRegion.SelectedValue);

        cmdQuery_Click(sender, e);
        if (GridView1.Rows.Count > 0)
        {
            GridView2Excel.Export(GridView1, fileName);
        }

        GridView1.GridLines = gl;
        GridView1.AllowPaging = paged;
        odsRateRpt.EnablePaging = paged;
    }
    protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDealers();
    }
}
