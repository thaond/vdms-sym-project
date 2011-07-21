using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using VDMS.II.Entity;
using VDMS.II.PartManagement.Order;

public partial class Part_Inventory_NotGoodAuto : BasePage
{
	int OrderId
	{
		get
		{
			int id = 0;
			int.TryParse(Request.QueryString["id"], out id);
			return id;
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			if (Session["Receive_OrderId"] == null || (int)Session["Receive_OrderId"] != OrderId)
				Response.Redirect("~/default.aspx");
			var list = (List<ReceiveDetail>)Session["Receive_Detail"];
			var query = from i in (list.Where(p => p.ShippingQuantity != p.GoodQuantity).Select(p => new { p.IssueNumber }).Distinct())
						select new
						{
							i.IssueNumber,
							Items = list.Where(p => p.IssueNumber == i.IssueNumber && (p.BrokenQuantity != 0 || p.LackQuantity != 0 || p.WrongQuantity != 0))
						};
			lv.DataSource = query;
			lv.DataBind();
			//Page.ClientScript.RegisterStartupScript(typeof(Part_Inventory_NotGoodAuto), "unload", "queryUnload = true;", true);
		}
	}

	protected void b1_Click(object sender, EventArgs e)
	{
		var oh = OrderDAO.GetOrderHeader(OrderId);
		OrderDAO.SaveReceive((List<ReceiveHeader>)Session["Receive_Header"], (List<ReceiveDetail>)Session["Receive_Detail"], oh.ToLocation);
		OrderDAO.CloseOrder(OrderId);
		lblSaveOk.Visible = true;
		DisableButton();
	}
}
