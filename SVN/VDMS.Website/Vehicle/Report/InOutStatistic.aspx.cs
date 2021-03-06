﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.I.Report;
using VDMS.Common.Utils;
using System.Web.UI.HtmlControls;
using VDMS.Common.Web;

public partial class Vehicle_Report_InOutStatistic : BasePage
{
    List<SummaryInOutStatisticItem> _Items = null;
    public List<SummaryInOutStatisticItem> ItemList
    {
        get
        {
            if (_Items == null) _Items = InOutStatisticReport.GetInOutStatisticItems(ddlArea.SelectedValue, DataFormat.DateFromString(txtFrom.Text), DataFormat.DateFromString(txtTo.Text));
            return _Items;
        }
    }

    public int EvalItemsCount()
    {
        return ItemList.Count() + 3;
    }
    public object EvalItems()
    {
        return ItemList;
    }

    GridView BindDetail(bool paging)
    {
        gv.AllowPaging = paging;
        odsDetail.EnablePaging = paging;
        lv.DataSourceID = null;
        lvExcel.DataSourceID = null;
        
        lv.Visible = lvExcel.Visible = false;
        gv.Visible = true;

        gv.GridLines = paging ? GridLines.None : GridLines.Both;

        if (string.IsNullOrEmpty(gv.DataSourceID))
        {
            gv.DataSourceID = odsDetail.ID;
            if (!paging) gv.DataBind();
        }
        else gv.DataBind();

        return gv;
    }
    ListView BindSummary(bool paging)
    {
        odsSummary.EnablePaging = paging;
        gv.DataSourceID = null;

        lv.Visible = paging;
        lvExcel.Visible = !paging;
        gv.Visible = false;

        var ll = (paging) ? lv : lvExcel;
        if (string.IsNullOrEmpty(ll.DataSourceID))
        {
            ll.DataSourceID = odsSummary.ID;
            if (!paging) ll.DataBind();
        }
        else ll.DataBind();

        return ll;
    }

    protected void tdPager_Load(object sender, EventArgs e)
    {
        (sender as HtmlTableCell).ColSpan = EvalItemsCount();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFrom.Text = DataFormat.DateOfFirstDayInMonth(DateTime.Now).ToShortDateString();
            txtTo.Text = DateTime.Now.ToShortDateString();
        }
    }
    protected void lvH_Load(object sender, EventArgs e)
    {
        var lv = (ListView)sender;
        lv.DataSource = EvalItems();
        lv.DataBind();
    }


    protected void btnFind_Click(object sender, EventArgs e)
    {
        if (rblType.SelectedIndex == 0) BindDetail(true); else BindSummary(true);
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        if (rblType.SelectedIndex == 0)
        {
            var gv = BindDetail(false);
            if (gv.Rows.Count > 0) GridView2Excel.Export(gv, "OrderStatistic.xls");
        }
        else
        {
            BindSummary(false);
            GridView2Excel.Export(lvExcel, "OrderStatistic.xls");
        }
    }
    protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Text = VDMS.Helper.DealerHelper.GetNameI(e.Row.Cells[0].Text);
        }
    }
}
