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

public partial class Sales_Report_Default3 : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			txtFromDate.Text = DateTimeHelper.FirstDayInMonth.ToShortDateString();
			txtToDate.Text = DateTime.Now.ToShortDateString();
            //LoadAllBranch();
		}
		else Search();
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
		//if (InventoryHelper.IsInventoryLock(DateTime.Now.AddMonths(-1), UserHelper.DealerCode))
		//    ShowMessage(Resources.Reports.InventoryIsLocked, true);
		//else Search();
	}

	private void Search()
	{
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

        //ReportDocument rdReceiptDetail = ReportFactory.GetReport();
        //rdReceiptDetail.Load(Server.MapPath(@"~/Report/ReceiptDetail.rpt"));
        //rdReceiptDetail.SetDataSource(InventoryDao.ReportSellingDailyDebtOnly(frDate, toDate, UserHelper.DealerCode, ddlBranch.SelectedValue, UserHelper.DatabaseCode));
        //String TitleReport = Resources.Constants.Store + " ";
        //TitleReport += ddlBranch.SelectedItem.Text + ". " + Resources.Constants.FromDate + " " + frDate.ToShortDateString() + " " + Resources.Constants.ToDate + " " + toDate.ToShortDateString();
        //rdReceiptDetail.SetParameterValue("Title", TitleReport);
        //rdReceiptDetail.SetParameterValue("PageTitleReceiptDetail", Resources.Reports.PageTitleReceiptDetail);
        //rdReceiptDetail.SetParameterValue("tblAddress", Resources.Reports.tblAddress);
        //rdReceiptDetail.SetParameterValue("tblColor", Resources.Reports.tblColor);
        //rdReceiptDetail.SetParameterValue("tblCustomerName", Resources.Reports.tblCustomerName);
        //rdReceiptDetail.SetParameterValue("tblDept", Resources.Reports.tblDept);
        //rdReceiptDetail.SetParameterValue("tblEngineNo", Resources.Reports.tblEngineNo);
        //rdReceiptDetail.SetParameterValue("tblModel", Resources.Reports.tblModel);
        //rdReceiptDetail.SetParameterValue("tblNo", Resources.Reports.tblNo);
        //rdReceiptDetail.SetParameterValue("tblPaid", Resources.Reports.tblPaid);
        //rdReceiptDetail.SetParameterValue("tblPrice", Resources.Reports.tblPrice);
        //rdReceiptDetail.SetParameterValue("titleSum", Resources.Reports.titleSum);

        //if (!rdReceiptDetail.Rows.Count.Equals(0))
        //{
        //    CrystalReportViewer1.DisplayGroupTree = false; CrystalReportViewer1.HasCrystalLogo = false;
        //    CrystalReportViewer1.ReportSource = rdReceiptDetail;
        //    CrystalReportViewer1.DataBind();
        //    CrystalReportViewer1.Visible = true;
        //    plDateFromTo.Visible = false;
        //}
        //else
        //{
        //    plDateFromTo.Visible = true;
        //    CrystalReportViewer1.Visible = false;
        //    ShowMessage(Resources.Reports.NotFound, true);
        //}
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

	private void ShowMessage(string mesg, bool isError)
	{
		lblErr.Visible = true;
		lblErr.ControlStyle.ForeColor = (isError) ? Color.Red : Color.RoyalBlue;
		lblErr.Text = mesg;
	}
    protected void ods_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["DealerCode"] = UserHelper.DealerCode;
        e.InputParameters["DatabaseCode"] = UserHelper.DatabaseCode;
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        gv.AllowPaging = false;
        gv.DataBind();
        GridView2Excel.Export(gv, "ReceiptDetail.xls");
        gv.AllowPaging = true;
    }
}
