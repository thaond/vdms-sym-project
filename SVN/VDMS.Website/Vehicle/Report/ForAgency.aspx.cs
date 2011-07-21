using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using Resources;
using VDMS.Common.Utils;
using VDMS.Helper;
using VDMS.I.Report;
using VDMS.I.Vehicle;

public partial class Vehicle_Report_ForAgency : BasePage
{
	DateTime monthNeedToLock;

	private Collection<ForAgencyError> errorCode = new Collection<ForAgencyError>();

	protected void AddError(ForAgencyError error)
	{
		if (errorCode.Contains(error)) return;
		errorCode.Add(error);
	}
	private void ShowError()
	{
		bllError.Items.Clear();
		foreach (ForAgencyError error in errorCode)
		{
			switch (error)
			{
				case ForAgencyError.OK: break;
				case ForAgencyError.PrevMonthNotLocked: bllError.Items.Add(string.Format(Reports.PrevMonthNotlocked, monthNeedToLock.ToString("MMMM"))); break;
				default: break;
			}

		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		errorCode.Clear();

		if (!Page.IsPostBack)
		{
			txtFromDate.Text = DateTimeHelper.FirstDayInMonth.ToShortDateString();
			txtToDate.Text = DateTime.Now.ToShortDateString();
		}
		else { Search(); }
		rvFromDate = DateValid(rvFromDate);
		rvToDate = DateValid(rvToDate);
	}

	protected void Page_PreRender(object sender, EventArgs e)
	{
		ShowError();
	}

	private RangeValidator DateValid(RangeValidator rvDate)
	{
		rvDate.MinimumValue = DateTime.MinValue.ToShortDateString();
		rvDate.MaximumValue = DateTime.MaxValue.ToShortDateString();
		return rvDate;
	}

	private void Search()
	{
		string fdate = txtFromDate.Text.Trim();
		string tdate = txtToDate.Text.Trim();
		DateTime frDate, toDate;

		if (checkValidDateSearchCondition())
		{
			DateTime.TryParse(fdate, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out frDate);
			DateTime.TryParse(tdate, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out toDate);
			toDate = toDate.AddHours(23); toDate = toDate.AddMinutes(59); toDate = toDate.AddSeconds(59);
		}
		else
		{
			ShowMessage(Resources.Message.DateConditionErr, true);
			return;
		}

		DataSet ds = ReportHelper.BuildReportDaily(frDate, toDate, UserHelper.DealerCode);
		if (ds != null)
		{
			ReportDocument rdForAgency = ReportFactory.GetReport();
			rdForAgency.Load(Server.MapPath(@"~/Report/IERItemsReport.rpt"));

            rdForAgency.SetDataSource(ds.Tables[0]);
			rdForAgency.SetParameterValue("FromDate", frDate.ToShortDateString());
			rdForAgency.SetParameterValue("ToDate", toDate.ToShortDateString());
			rdForAgency.SetParameterValue("IERPageTitle", Resources.Reports.IERPageTitle);
			rdForAgency.SetParameterValue("FromDateLabel", Resources.Constants.FromDate);
			rdForAgency.SetParameterValue("ToDateLabel", Resources.Constants.ToDate);
			rdForAgency.SetParameterValue("BranchLabel", Resources.Reports.BranchLabel);
			rdForAgency.SetParameterValue("Model", Resources.Reports.Model);
			rdForAgency.SetParameterValue("Color", Resources.Reports.Color);
			rdForAgency.SetParameterValue("SubTotal", Resources.Reports.SubTotal);
			rdForAgency.SetParameterValue("Total", Resources.Reports.Total);
			rdForAgency.SetParameterValue("AllTotal", Resources.Reports.AllTotal);
			rdForAgency.SetParameterValue("BeginingInventory", Resources.Reports.BeginingInventory);
			rdForAgency.SetParameterValue("EndingInventory", Resources.Reports.EndingInventory);
			rdForAgency.SetParameterValue("Order", Resources.Reports.Order);
			rdForAgency.SetParameterValue("Import", Resources.Reports.Import);
			rdForAgency.SetParameterValue("Issue", Resources.Reports.Issue);

			if (rdForAgency.Rows.Count > 0)
			{
				plDateFromTo.Visible = false;
				CrystalReportViewer1.DisplayGroupTree = false; CrystalReportViewer1.HasCrystalLogo = false;
				CrystalReportViewer1.ReportSource = rdForAgency;
				CrystalReportViewer1.DataBind();
				CrystalReportViewer1.Visible = true;
			}
			else
			{
				plDateFromTo.Visible = true;
				CrystalReportViewer1.Visible = false;
				ShowMessage(Resources.Reports.NotFound, true);
			}
		}
		else
		{
			plDateFromTo.Visible = true;
			CrystalReportViewer1.Visible = false;
			ShowMessage(Resources.Reports.NotFound, true);
		}
	}
	private bool checkValidDateSearchCondition()
	{
		CultureInfo cultInfo = Thread.CurrentThread.CurrentCulture;
		DateTime dtFrom, dtTo;

		DateTime.TryParse(txtFromDate.Text, cultInfo, DateTimeStyles.AllowWhiteSpaces, out dtFrom);
		DateTime.TryParse(txtToDate.Text, cultInfo, DateTimeStyles.AllowWhiteSpaces, out dtTo);

		if (dtFrom > DateTime.MinValue)
		{
			monthNeedToLock = (dtFrom.Month == 1) ? new DateTime(dtFrom.Year - 1, 12, 1) : new DateTime(dtFrom.Year, dtFrom.Month - 1, 1);
            if (!InventoryHelper.IsInventoryLock(monthNeedToLock, UserHelper.DealerCode,UserHelper.BranchCode))
				AddError(ForAgencyError.PrevMonthNotLocked);
		}
		if (dtFrom > dtTo)
		{
			return false;
		}
		else return true;
	}
	protected void btnSearch_Click(object sender, EventArgs e)
	{
		//if (InventoryHelper.IsInventoryLock(DateTime.Now.AddMonths(-1), UserHelper.DealerCode))
		//    ShowMessage(Resources.Reports.InventoryIsLocked, true);
		//else Search();
	}
	private void ShowMessage(string mesg, bool isError)
	{
		lblErr.Visible = true;
		lblErr.Text = mesg;
	}
}
