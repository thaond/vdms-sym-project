using System;
using System.Linq;
using System.Web.UI.WebControls;
using VDMS.II.Entity;
using VDMS.II.Linq;
using VDMS.II.PartManagement.Order;

public partial class Part_Inventory_Order : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			txtFromDate.Text = DateTime.Now.AddDays(-7).ToShortDateString();
			txtToDate.Text = DateTime.Now.ToShortDateString();
		}
	}

	protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			var h = (VDMS.WebService.Entity.OrderInfo)e.Row.DataItem;
			((HyperLink)e.Row.FindControl("h1")).NavigateUrl = string.Format("OrderEdit.aspx?id={0}&action=edit", h.OrderHeaderId);
			((HyperLink)e.Row.FindControl("h3")).NavigateUrl = string.Format("OrderView.aspx?id={0}&TB_iframe=true&height=320&width=420", h.OrderHeaderId);
			var canEdit = false;
			if (h.Status == OrderStatus.OrderOpen || (h.Status == OrderStatus.OrderSent && h.TipTopProcessed != "Y")) canEdit = true;
			if (!canEdit)
			{
				e.Row.Cells[0].Controls[0].Visible = false;
				e.Row.Cells[0].Controls[1].Visible = false;
				e.Row.Cells[0].Controls[2].Visible = false;
				e.Row.Cells[0].Controls[3].Visible = false;
				e.Row.Cells[7].Controls.Clear();
			}
			if (h.Status != OrderStatus.OrderOpen) e.Row.Cells[7].Controls.Clear();
			if (h.OrderSource != "V")
			{
				e.Row.Cells[0].Controls[2].Visible = false;
				e.Row.Cells[0].Controls[3].Visible = false;
			}
		}
	}

	protected void ddlDealer_SelectedIndexChanged(object sender, EventArgs e)
	{
		ddlWH.DealerCode = ddlDealer.SelectedValue;
		ddlWH.DataBind();
	}

	protected void ddlDealer_DataBound(object sender, EventArgs e)
	{
		ddlWH.DealerCode = ddlDealer.SelectedValue;
		ddlWH.DataBind();
	}

	protected void cmdQuery_Click(object sender, EventArgs e)
	{
		gv.DataBind();
	}
}
