using System;
using CrystalDecisions.CrystalReports.Engine;
using VDMS.I.Report.DataObject;

public partial class Vehicle_Report_PrintOrder : BasePage
{
    string oid;
    private ReportDocument rpt;
    protected void Page_Load(object sender, EventArgs e)
    {
        oid = Request.QueryString["oid"];
        MultiView1.ActiveViewIndex = LoadReport(oid) ? 1 : 0;
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        if (rpt != null)
        {
            rpt.Close();
            rpt.Dispose();
            GC.Collect();
        }
    }
    protected bool LoadReport(string orderId)
    {
        rptViewer.DisplayGroupTree = false;
        rptViewer.HasCrystalLogo = false;
        rpt = ReportDataSource.BuildReportDocumentForOrder(orderId);
        if (rpt != null)
        {
            rptViewer.ReportSource = rpt;
            rptViewer.DataBind();
        }
        return rpt != null;
    }
}
