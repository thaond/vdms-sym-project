using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Provider;
using VDMS.II.Report;
using VDMS.Helper;
using VDMS.II.PartManagement.Order;
using System.Collections;
using VDMS.Common.Utils;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using VDMS.Common.Web;

public partial class MPart_Report_SaleDetailReport : BasePage
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
                //crViewer.ReportSource = SaleDetailReport.CreateReportDoc(txtFromDate.Text, txtToDate.Text, ddlWarehouse.SelectedValue, ddlDealer.SelectedValue, ddlCustomer.SelectedValue);
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
        Dealer d = DealerDAO.GetDealerByCode(ddlDealer.SelectedValue);
        DateTime dtFrom = DataFormat.DateFromString(txtFromDate.Text);
        DateTime dtTo = DataFormat.DateFromString(txtToDate.Text);
        string xx = string.IsNullOrEmpty(ddlWarehouse.SelectedValue) ? "All" : ddlWarehouse.SelectedValue;
        string fileName = string.Format("SaleDetail.{0}.{1}.{2}-{3}.xls", d.DealerCode, xx, dtFrom.ToString("yyyy_MM_dd"), dtTo.ToString("yyyy_MM_dd"));

        //decimal total;
        //long wid, cid;
        //long.TryParse(ddlWarehouse.SelectedValue, out wid);
        //long.TryParse(ddlCustomer.SelectedValue, out cid);

        //GridView1.DataSource = SaleDetailReport.GetReportSource(dtFrom, dtTo, wid, cid, d.DealerCode, d.DatabaseCode, out total);
        //GridView1.DataBind();
        //if (GridView1.Rows.Count > 0)
        //{
        GridView1.AllowPaging = false;
        GridView1.DataBind();
        GridView2Excel.Export(GridView1, fileName);
        GridView1.AllowPaging = true;
        //}
    }
    protected void ObjectDataSource1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Dealer d = DealerDAO.GetDealerByCode(ddlDealer.SelectedValue);
        e.InputParameters["dealerCode"] = d.DealerCode;
        e.InputParameters["dbCode"] = d.DatabaseCode;
    }
    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        if (GridView1.Rows.Count > 0)
            GridView1.FooterRow.Cells[5].Text = SaleDetailReport.TotalAmount.ToString("C0");
    }
}
