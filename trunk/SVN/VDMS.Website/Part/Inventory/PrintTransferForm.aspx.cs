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

public partial class Part_PrintTransferForm : BasePage
{
	public long HeaderId
	{
		get { long id; long.TryParse(Request.QueryString["id"], out id); return id; }
	}
	TransferHeader header = null;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (HeaderId > 0)
		{
			crptViewer.ReportSource = LoadTransferForm(HeaderId);
			if ((header == null) || (header.Status != ST_Status.New))
				lnkBack.NavigateUrl = "~/Part/inventory/StockTransfer.aspx";
			else
				lnkBack.NavigateUrl = string.Format("~/Part/inventory/StockTransfer.aspx?id={0}", HeaderId);
		}
	}

	private object LoadTransferForm(long HeaderId)
	{
		ReportDocument rptDoc = ReportFactory.GetReport();

		header = PartTransferDAO.GetTransferHeader(HeaderId);
		if (header != null)
		{
			rptDoc.Load(HttpContext.Current.Server.MapPath(@"~/Part/Report/Crystal/StockTransferReport.rpt"));

			IList<PartTransfer> data = header.TransferDetails.Select(td => new PartTransfer(td)).ToList();
			rptDoc.SetDataSource(data);

			string comment = header.TransferComment;
			if (comment == null) comment = "";

			string dealer = header.Dealer.DealerName;
			string saleman = string.IsNullOrEmpty(UserHelper.Fullname) ? UserHelper.Username : UserHelper.Fullname;
            string sheet = string.IsNullOrEmpty(header.VoucherNumber) ? "" : header.VoucherNumber;

			rptDoc.SetParameterValue("TransferDate", header.TransferDate);
			rptDoc.SetParameterValue("Comment", comment);
			rptDoc.SetParameterValue("FromWarehouse", header.Warehouse.Address);
			rptDoc.SetParameterValue("ToWarehouse", header.Warehouse1.Address);
			rptDoc.SetParameterValue("SheetNo", sheet);
			rptDoc.SetParameterValue("Dealer", dealer);
			rptDoc.SetParameterValue("SaleMan", saleman);
		}

		return rptDoc;
	}
}
