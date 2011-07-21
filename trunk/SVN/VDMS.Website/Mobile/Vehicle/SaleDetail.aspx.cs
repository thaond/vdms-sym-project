using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using VDMS.Common.Utils;
using VDMS.Common.Web;
using VDMS.Data.DAL2;
using VDMS.Helper;

public partial class MVehicle_Report_SaleDetail : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtFromDate.Text = DateTimeHelper.FirstDayInMonth.ToShortDateString();
            txtToDate.Text = DateTime.Now.ToShortDateString();
            //LoadAllBranch();
        }
        else
        {
            Search();
        }
        rvFromDate = DateValid(rvFromDate);
        rvToDate = DateValid(rvToDate);
    }
    private RangeValidator DateValid(RangeValidator rvDate)
    {
        rvDate.MinimumValue = DateTime.MinValue.ToShortDateString();
        rvDate.MaximumValue = DateTime.MaxValue.ToShortDateString();
        return rvDate;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        //if (InventoryHelper.IsInventoryLock(DateTime.Now, UserHelper.DealerCode))
        //{
        //    ShowMessage(Resources.Reports.InventoryIsLocked, true);
        //}
        //else Search();
        //liDateTitle.Text = Resources.Constants.FromDate + " " + txtFromDate.Text.Trim() + " " + Resources.Constants.ToDate + " " + txtToDate.Text.Trim();
    }
    private bool checkValidDateSearchCondition()
    {
        CultureInfo cultInfo = Thread.CurrentThread.CurrentCulture;
        DateTime dtFrom, dtTo;
        DateTime.TryParse(txtFromDate.Text, cultInfo, DateTimeStyles.AllowWhiteSpaces, out dtFrom);
        DateTime.TryParse(txtToDate.Text, cultInfo, DateTimeStyles.AllowWhiteSpaces, out dtTo);
        if (dtFrom > dtTo)
        {
            return false;
        }
        else return true;
    }

    private void Search()
    {
        //if (!InventoryHelper.IsInventoryLock(DateTime.Now, UserHelper.DealerCode))
        //{
        //    return;
        //}
        //if (ddlBranch.Items.Count == 0)
        //{
        //    ShowMessage(Resources.Reports.BranchLoadFail, true);
        //    return;
        //}
        //string fdate = txtFromDate.Text.Trim();
        //string tdate = txtToDate.Text.Trim();
        //DateTime frDate, toDate;
        //if (checkValidDateSearchCondition())
        //{
        //    DateTime.TryParse(fdate, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out frDate);
        //    DateTime.TryParse(tdate, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out toDate);
        //    toDate = toDate.AddHours(23); toDate = toDate.AddMinutes(59); toDate = toDate.AddSeconds(59);
        //}
        //else
        //{
        //    ShowMessage(Resources.Message.DateConditionErr, true);
        //    return;
        //}

        //ReportDocument rdSellingDaily = ReportFactory.GetReport();
        //rdSellingDaily.Load(Server.MapPath(@"~/Report/SellingDaily2.rpt"));
        //rdSellingDaily.SetDataSource(InventoryDao.ReportSellingDaily(frDate, toDate, UserHelper.DealerCode, ddlBranch.SelectedValue, UserHelper.DatabaseCode));
        //String TitleReport = Resources.Constants.Store + " ";
        //TitleReport += ddlBranch.SelectedItem.Text + ". " + Resources.Constants.FromDate + " " + frDate.ToShortDateString() + " " + Resources.Constants.ToDate + " " + toDate.ToShortDateString();
        //rdSellingDaily.SetParameterValue("Title", TitleReport);
        //rdSellingDaily.SetParameterValue("PageTitleSellingDaily", Resources.Reports.PageTitleSellingDaily);
        //rdSellingDaily.SetParameterValue("tblAddress", Resources.Reports.tblAddress);
        //rdSellingDaily.SetParameterValue("tblColor", Resources.Reports.tblColor);
        //rdSellingDaily.SetParameterValue("tblCustomerName", Resources.Reports.tblCustomerName);
        //rdSellingDaily.SetParameterValue("tblDept", Resources.Reports.tblDept);
        //rdSellingDaily.SetParameterValue("tblEngineNo", Resources.Reports.tblEngineNo);
        //rdSellingDaily.SetParameterValue("tblModel", Resources.Reports.tblModel);
        //rdSellingDaily.SetParameterValue("tblSellDate", Resources.Reports.tblSellDate);
        //rdSellingDaily.SetParameterValue("tblNo", Resources.Reports.tblNo);
        //rdSellingDaily.SetParameterValue("tblPaid", Resources.Reports.tblPaid);
        //rdSellingDaily.SetParameterValue("tblPrice", Resources.Reports.tblPrice);
        //rdSellingDaily.SetParameterValue("titleSum", Resources.Reports.titleSum);

        //if (rdSellingDaily.Rows.Count > 0)
        //{
        //    CrystalReportViewer1.DisplayGroupTree = false; CrystalReportViewer1.HasCrystalLogo = false;
        //    CrystalReportViewer1.ReportSource = rdSellingDaily;
        //    CrystalReportViewer1.DataBind();
        //    CrystalReportViewer1.Visible = true;
        //    plDateFromTo.Visible = false;
        //}
        //else
        //{
        //    CrystalReportViewer1.Visible = false;
        //    ShowMessage(Resources.Reports.NotFound, true);
        //    plDateFromTo.Visible = true;
        //}
    }
    private void ShowMessage(string mesg, bool isError)
    {
        lblErr.Visible = true;
        lblErr.ControlStyle.ForeColor = (isError) ? Color.Red : Color.RoyalBlue;
        lblErr.Text = mesg;
    }
    protected void gv_DataBinding(object sender, EventArgs e)
    {
    }
    protected void ods_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //ods.SelectParameters["DatabaseCode"].Type = TypeCode.String;
        e.InputParameters["DatabaseCode"] = UserHelper.DatabaseCode;
        e.InputParameters["DealerCode"] = UserHelper.DealerCode;

    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        gv.AllowPaging = false;
        gv.DataBind();
        GridView2Excel.Export(gv, "SaleDetail.xls");
        gv.AllowPaging = true;
    }
}