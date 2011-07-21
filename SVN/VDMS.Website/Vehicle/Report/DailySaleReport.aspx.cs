using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using Resources;
using VDMS.Common.Web;
using VDMS.Common.Web.GridViewHepler;
using VDMS.Data.DAL2;
using VDMS.Helper;
using VDMS.I.Vehicle;
using VDMS.I.Report;

public partial class Vehicle_Report_DailySaleReport : BasePage
{
    DateTime monthNeedToLock;

    private Collection<SaleReportError> errorCode = new Collection<SaleReportError>();

    protected void AddError(SaleReportError error)
    {
        if (errorCode.Contains(error)) return;
        errorCode.Add(error);
    }
    private void ShowError()
    {
        bllError.Items.Clear();
        foreach (SaleReportError error in errorCode)
        {
            switch (error)
            {
                case SaleReportError.OK: break;
                case SaleReportError.PrevMonthNotLocked: bllError.Items.Add(string.Format(Reports.PrevMonthNotlocked, monthNeedToLock.ToString("MMMM"))); break;
                default: break;
            }

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) txtFromDate.Text = DateTime.Now.ToShortDateString();
        GridViewHelper helper = new GridViewHelper(this.gvReport, true);
        
        helper.RegisterSummary("TonDau", SummaryOperation.Sum);
        helper.RegisterSummary("Nhap", SummaryOperation.Sum);
        helper.RegisterSummary("Xuat", SummaryOperation.Sum);
        helper.RegisterSummary("TonCuoi", SummaryOperation.Sum);
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        ShowError();
    }

    protected void cmdCreate_Click(object sender, EventArgs e)
    {
        DateTime fromDate;
        DateTime.TryParse(txtFromDate.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out fromDate);

        CheckReportDate(fromDate);

        //if (ddlItem.SelectedIndex == 0) gvReport.DataSource = InventoryDao.SalesReport(fromDate, txtDealer.Text, UserHelper.DatabaseCode);
        //else gvReport.DataSource = InventoryDao.SalesReport(fromDate, ddlItem.SelectedValue, txtDealer.Text, UserHelper.DatabaseCode);
        gvReport.DataSource = IOSaleReport.DoReport(fromDate, ddlItem.SelectedValue, txtDealer.Text, ddlArea.SelectedValue, UserHelper.DatabaseCode);
        gvReport.DataBind();

        cmdExcel.Enabled = gvReport.Rows.Count > 0;
    }

    private void CheckReportDate(DateTime dtFrom)
    {
        // must check all dealer to valid
        //if (dtFrom > DateTime.MinValue)
        //{
        //    monthNeedToLock = (dtFrom.Month == 1) ? new DateTime(dtFrom.Year - 1, 12, 1) : new DateTime(dtFrom.Year, dtFrom.Month - 1, 1);
        //    if (!InventoryHelper.IsInventoryLock(monthNeedToLock, UserHelper.DealerCode))
        //        AddError(SaleReportError.PrevMonthNotLocked);
        //}
    }

    protected void ddlItem_DataBound(object sender, EventArgs e)
    {
        //foreach (ListItem item in ((DropDownList)sender).Items)
        //{
        //    if (!string.IsNullOrEmpty(item.Value))
        //        item.Text = string.Format("{1}({0})", item.Text, item.Value);
        //}
    }
    protected void cmdExcel_Click(object sender, EventArgs e)
    {
        GridView2Excel.Export(gvReport, this.Page, "SalesReport.xls");
    }

    int index = 0;
    protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //index++;
            e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();// index.ToString();
            //DataRowView row = e.Row.DataItem as DataRowView;
            //e.Row.Cells[2].Text = DealerHelper.GetName((string)row["DealerCode"]);
        }
    }
    protected void gvReport_DataBound(object sender, EventArgs e)
    {
        //gvReport.Rows[gvReport.Rows.Count - 1].CssClass = "SumLine";
    }
}
