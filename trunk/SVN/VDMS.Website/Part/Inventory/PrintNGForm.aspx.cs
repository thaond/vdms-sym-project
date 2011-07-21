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


public partial class Part_PrintNGForm : BasePage
{
    public long HeaderId
    {
        get { long id; long.TryParse(Request.QueryString["id"], out id); return id; }
    }
    NGFormHeader header = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (HeaderId > 0)
        {
            crptViewer.ReportSource = LoadForm(HeaderId);
            //if ((header == null) || (header.Status != ST_Status.New))
            lnkBack.NavigateUrl = "~/Part/inventory/NotGood.aspx";
            //else
            //    lnkBack.NavigateUrl = string.Format("~/Part/inventory/StockTransfer.aspx?id={0}", HeaderId);
        }
    }

    private object LoadForm(long HeaderId)
    {
        ReportDocument rptDoc = ReportFactory.GetReport();
        var dc = DCFactory.GetDataContext<PartDataContext>();
        var sv = DCFactory.GetDataContext<ServiceDataContext>();

        header = NotGoodDAO.GetNGHeader(HeaderId);
        if (header != null)
        {
            rptDoc.Load(HttpContext.Current.Server.MapPath(@"~/Part/Report/Crystal/NGSheet.rpt"));

            var data = from d in header.NGFormDetails
                       join p in dc.Parts.Where(p => p.DatabaseCode == header.Dealer.DatabaseCode) on d.PartCode equals p.PartCode
                       select new NGFormDetail(d)
                       {
                           PartName = p.VietnamName,
                           Broken = sv.Brokens.FirstOrDefault(b => b.BrokenCode == d.BrokenCode),
                       };
            rptDoc.SetDataSource(data);

            string dealer = header.Dealer.DealerName;

            rptDoc.SetParameterValue("NGDate", header.CreatedDate);
            rptDoc.SetParameterValue("Voucher", string.IsNullOrEmpty(header.NotGoodNumber) ? "" : header.NotGoodNumber);
            rptDoc.SetParameterValue("Dealer", dealer);
        }

        return rptDoc;
    }
}
