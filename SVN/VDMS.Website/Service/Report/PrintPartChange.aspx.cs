using System;
using CrystalDecisions.CrystalReports.Engine;
using VDMS.I.Report.DataObject;

public partial class Service_Report_PrintPartChange : BasePage
{
	string pcvn;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			rptViewer.DisplayGroupTree = false;
			rptViewer.HasCrystalLogo = false;
		}

		pcvn = Request.QueryString["pcvn"];
		MultiView1.ActiveViewIndex = LoadReport(pcvn) ? 1 : 0;
	}
	protected bool LoadReport(string changeNo)
	{
		if (string.IsNullOrEmpty(changeNo)) return false;

		ReportDocument rpt = ReportDataSource.BuildReportDocumentForPartChange(changeNo);
		if (rpt != null)
		{
			rptViewer.ReportSource = rpt;
			rptViewer.DataBind();
		}
		return rpt != null;
	}
}
