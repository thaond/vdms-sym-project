using System;
using System.Web.UI.WebControls;
using VDMS.Common.Utils;
using VDMS.Common.Web;

public partial class Part_Inventory_InShortReason : BasePage
{
	void BindDealerList()
	{
		ddlDealers.DatabaseCode = ddlRegion.SelectedValue;
		ddlDealers.DataBind();
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			BindDealerList();
			txtFromDate.Text = DataFormat.DateOfFirstDayInMonth(DateTime.Now).ToShortDateString();
			txtToDate.Text = DateTime.Now.ToShortDateString();
		}
	}

	protected void cmdQuery_Click(object sender, EventArgs e)
	{
		gv.DataBind();
	}

	protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
	{
		BindDealerList();
	}

	protected void cmd2Excel_Click(object sender, EventArgs e)
	{
		bool paged = gv.AllowPaging;
		GridLines gl = gv.GridLines;
		gv.AllowPaging = false;
		gv.GridLines = GridLines.Both;

		string dtFrom = DataFormat.DateFromString(txtFromDate.Text).ToString("yyyy_MM_dd");
		string dtTo = DataFormat.DateFromString(txtToDate.Text).ToString("yyyy_MM_dd");
		string dl = string.IsNullOrEmpty(ddlDealers.SelectedValue) ? "All" : ddlDealers.SelectedValue;
		string fileName = string.Format("ShortSupplier.{0}.{1}-{2}.xls", dl, dtFrom, dtTo);

		cmdQuery_Click(sender, e);
		if (gv.Rows.Count > 0)
		{
			GridView2Excel.Export(gv, fileName);
		}

		gv.GridLines = gl;
		gv.AllowPaging = paged;
	}
}
