using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using VDMS.Common.Utils;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.PartManagement;
using VDMS.II.PartManagement.Order;
using VDMS.II.Linq;
using VDMS.I.Linq;


public partial class Part_PrintRewardForm : BasePage
{
    public long HeaderId
    {
        get { long id; long.TryParse(Request.QueryString["h"], out id); return id; }
    }
    public string Status
    {
        get { return Request.QueryString["s"]; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (HeaderId > 0)
        {
            LoadForm(HeaderId, Status);
            ////if ((header == null) || (header.Status != ST_Status.New))
            //lnkBack.NavigateUrl = "~/Part/inventory/NotGood.aspx";
            ////else
            ////    lnkBack.NavigateUrl = string.Format("~/Part/inventory/StockTransfer.aspx?id={0}", HeaderId);
        }
    }

    private void LoadForm(long HeaderId, string status)
    {
        var header = NotGoodDAO.GetLackingData(HeaderId, status);
        if (header != null)
        {
            ReportDocument rptDoc = ReportFactory.GetReport();
            rptDoc.Load(HttpContext.Current.Server.MapPath(@"~/Part/Report/Crystal/RewardForm.rpt"));

            rptDoc.SetDataSource(header.Items);

            rptDoc.SetParameterValue("NGNo", header.NotGoodNumber == null ? "" : header.NotGoodNumber);
            rptDoc.SetParameterValue("IssueNo", (header.IssueNumber == null) ? "" : header.IssueNumber);
            rptDoc.SetParameterValue("RewardNo", (header.RewardNumber == null) ? "" : header.RewardNumber);
            crptViewer.ReportSource = rptDoc;
        }
    }
}
