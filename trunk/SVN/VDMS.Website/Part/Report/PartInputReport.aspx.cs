﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.Common.Utils;
using VDMS.II.Report;
using VDMS.Common.Web;

public partial class Part_Report_PartInputReport : BasePage
{
    string currentDealer
    {
        get { return (string)ViewState["currentDealer"]; }
        set { ViewState["currentDealer"] = value; }
    }

    protected void BindWarehouse()
    {
        if (ddlDealer.Items.Count == 0) ddlDealer.DataBind();
        ddlWarehouse.DealerCode = UserHelper.IsDealer ? UserHelper.DealerCode : ddlDealer.SelectedValue;
        ddlWarehouse.DataBind();
    }

    new protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
        ddlDealer.RootDealer = UserHelper.DealerCode;
        ddlDealer.RemoveRootItem = UserHelper.DealerCode == "/";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindWarehouse();
            txtFromDate.Text = DataFormat.DateOfFirstDayInMonth(DateTime.Now).ToShortDateString();
            txtToDate.Text = DateTime.Now.ToShortDateString();
            currentDealer = ddlDealer.SelectedValue;
        }
        else
        {
            if (currentDealer == ddlDealer.SelectedValue)
            {
                //crViewer.ReportSource = PartInputReport.CreateReportDoc(txtFromDate.Text, txtToDate.Text, ddlWarehouse.SelectedValue);
            }
            //crViewer.Visible = currentDealer == ddlDealer.SelectedValue;
        }
    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        //crViewer.DataBind();
    }
    protected void Unnamed3_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindWarehouse();
        currentDealer = ddlDealer.SelectedValue;
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        //GridView1.DataSourceID = ods.ID;
        GridView1.AllowPaging = false;
        GridView1.DataBind();
        GridView2Excel.Export(GridView1, string.Format("IMPrpt.{0}-{1}.{2}.{3}.xls", txtFromDate.Text, txtToDate.Text, ddlDealer.SelectedValue, ddlWarehouse.SelectedValue));
        GridView1.AllowPaging = true;
    }
}
