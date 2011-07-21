using System;
using System.Web;
using System.Web.UI.WebControls;
using VDMS.Helper;

public partial class Admin_Database_Agency : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			txtFromDate.Text = DateTimeHelper.FirstDayInMonth.ToShortDateString();
			txtToDate.Text = DateTime.Now.ToShortDateString();
		}
	}

	protected void btnTest_Click(object sender, EventArgs e)
	{
		grvMaster.PageIndex = 0;
		grvMaster.DataBind();
	}

	protected void grvMaster_DataBound(object sender, EventArgs e)
	{
		try
		{
			Literal litPageInfo = grvMaster.TopPagerRow.FindControl("litPageInfo") as Literal;
			if (litPageInfo != null) litPageInfo.Text = string.Format(Resources.Message.PagingInfo, grvMaster.PageIndex + 1, grvMaster.PageCount, HttpContext.Current.Items["rowCount"]);
		}
		catch { }
	}

	protected void grvMaster_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			e.Row.Cells[0].Text = (grvMaster.PageSize * grvMaster.PageIndex + e.Row.RowIndex + 1).ToString();
		}
	}
}
