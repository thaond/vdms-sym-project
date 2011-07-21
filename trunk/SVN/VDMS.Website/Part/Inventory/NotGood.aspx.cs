using System;
using System.Web.UI.WebControls;
using VDMS.II.Entity;

public partial class Part_Inventory_NotGood : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{

	}

	protected void b_Click(object sender, EventArgs e)
	{
		gv.DataBind();
	}

	protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			var obj = e.Row.DataItem as NGFormHeader;
			((HyperLink)e.Row.FindControl("h1")).NavigateUrl = string.Concat("NotGoodEdit.aspx?id=", obj.NGFormHeaderId);
			if (obj.NGType != "S") ((HyperLink)e.Row.FindControl("h1")).NavigateUrl = "#";
			((HyperLink)e.Row.FindControl("h3")).NavigateUrl = string.Format("NotGoodView.aspx?id={0}&TB_iframe=true&height=320&width=600", obj.NGFormHeaderId);
			if (obj.Status != OrderStatus.OrderOpen)
			{
				e.Row.Cells[0].Controls[0].Visible = false;
				e.Row.Cells[0].Controls[1].Visible = false;
				e.Row.Cells[7].Controls.Clear();
			}
			if (obj.Status == OrderStatus.OrderConfirmed)
			{
				e.Row.Cells[0].Controls[2].Visible = false;
				e.Row.Cells[0].Controls[3].Visible = false;
			}
			if (obj.NGType == NGType.Normal)
			{
				e.Row.Cells[0].Controls[2].Visible = false;
				e.Row.Cells[0].Controls[3].Visible = false;
			}
		}
	}
}
