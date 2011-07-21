using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.PartManagement.Order;
using VDMS.II.PartManagement;

public partial class Part_Inventory_InStock : BasePage
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

	//long ReceiveId;
	//long NGFormId;
	//bool NGConfirmed = false;
	//List<NGFormDetail> list;
	protected void Page_Load(object sender, EventArgs e)
	{
		//try
		//{
		//    ReceiveId = (long)LoadState("ReceiveId");
		//}
		//catch { }
		if (!Page.IsPostBack)
		{
			if (OrderId == 0) Response.Redirect("Receive.aspx");
			if (InventoryDAO.IsInventoryLock(UserHelper.WarehouseId, DateTime.Now.Year, DateTime.Now.Month))
			{
				lblInventoryClose.Visible = true;
				bSave.Enabled = false;
			}
			OrderInfo1.OrderId = OrderId;
			var oh = OrderDAO.GetOrderHeader(OrderId);
			if (oh == null || oh.DealerCode != UserHelper.DealerCode) Response.Redirect("Receive.aspx");

			Literal litTotal = (Literal)lv.FindControl("litTotal");
			if (litTotal != null) litTotal.Text = string.Format("{0:C0}", oh.Amount);

			//if (oh.AlreadyInStock)
			//{
			//    var rh = OrderDAO.GetReceiveHeaderByOrderHeader(OrderId);
			//    ReceiveId = rh.ReceiveHeaderId;

			//    SaveState("ReceiveId", ReceiveId);
			//    //tbIN.Text = rh.IssueNumber;
			//    bSave.Enabled = false;

			//    if (oh.Status != OrderStatus.OrderClosedNormal)
			//    {
			//        var ngh = NotGoodDAO.GetHeaderByReceiveId(rh.ReceiveHeaderId);
			//        if (ngh != null)
			//        {
			//            NGConfirmed = ngh.Status == NGStatus.Confirmed;
			//            NGFormId = ngh.NGFormHeaderId;
			//            list = NotGoodDAO.FindAllDetail(NGFormId);
			//        }
			//        else
			//        {
			//            lv.Enabled = false;
			//            lblNGNotCreate.Visible = true;
			//        }
			//    }
			//    ods1.SelectMethod = "GetReceiveDetail";
			//}

			bSave.OnClientClick = SaveConfirmQuestion;
			//bSave.Enabled = OrderDAO.GetNewReceiveCount(OrderId) > 0;
		}
	}

	protected void lv_DataBound(object sender, EventArgs e)
	{
		var canSave = false;
		foreach (ListViewDataItem row in lv.Items)
		{
			var ngnumber = ((Label)row.FindControl("litNGNumber")).Text;
			var lvItems = (ListView)row.FindControl("lvItems");
			if (!string.IsNullOrEmpty(ngnumber))
			{
				foreach (var row1 in lvItems.Items)
				{
					((TextBox)row1.FindControl("t2")).Enabled = false;
					((TextBox)row1.FindControl("t3")).Enabled = false;
					((TextBox)row1.FindControl("t4")).Enabled = false;
					((TextBox)row1.FindControl("t5")).Enabled = false;
				}
			}
			if (lvItems.Enabled) canSave = true;
		}
		if (bSave.Enabled) bSave.Enabled = canSave;
	}

	//List<ReceiveDetail> rd;
	//List<OrderDAO.ImportLackInfo> ld;
	protected void bSave_Click(object sender, EventArgs e)
	{
		//if (!CheckQuantity()) return;
		if (!GetData()) return;
		if (!receiveOk)
		{
			Session["Receive_Header"] = rHeader;
			Session["Receive_Detail"] = rDetail;
			Session["Receive_OrderId"] = OrderId;
			Response.Redirect("NotGoodAuto.aspx?id=" + OrderId.ToString());
		}
		else
		{
			var oh = OrderDAO.GetOrderHeader(OrderId);
			OrderDAO.SaveReceive(rHeader, rDetail, oh.ToLocation);
            //OrderDAO.UpdateOrderDelivery(OrderId); //trigger used
            OrderDAO.CloseOrder(OrderId);
			lblSaveOk.Visible = true;
			DisableButton();
			//if (receiveNG) Response.Redirect("NotGoodAuto.aspx?id=" + OrderId.ToString());
			//Page.ClientScript.RegisterClientScriptBlock(typeof(Part_Inventory_InStock), @"window.location(""default2.html?keepThis=true&amp;TB_iframe=true&amp;height=388&amp;width=444"" title=""How to Build Better Credit"" class=""thickbox"");", "", true);
			var id = NotGoodDAO.GetIdByNGNumber(rHeader[0].NotGoodNumber);
			if (id != 0) litPopupJS.Text = string.Format(@"<script type=""text/javascript"">$(document).ready(function() {{tb_show(""Update Not Good data"", ""NotGoodView.aspx?id={0}&TB_iframe=true&height=320&width=600"");}});</script>", id);
		}
	}

	//bool CheckQuantity()
	//{
	//    var r = true;
	//    foreach (ListViewDataItem row in lv.Items)
	//    {
	//        var g = ((TextBox)row.FindControl("t1")).Text;
	//        var b = ((TextBox)row.FindControl("t2")).Text;
	//        var w = ((TextBox)row.FindControl("t3")).Text;
	//        var l = ((TextBox)row.FindControl("t4")).Text;
	//        var q = ((TextBox)row.FindControl("t0")).Text;
	//        var c = ((TextBox)row.FindControl("t5")).Text;
	//        if (int.Parse(q) != int.Parse(g) + int.Parse(b) + int.Parse(w) + int.Parse(l) || (int.Parse(q) != int.Parse(g) && string.IsNullOrEmpty(c)))
	//        {
	//            ((HtmlTableRow)row.FindControl("tr")).Attributes["class"] = "error";
	//            r = false;
	//        }
	//        else ((HtmlTableRow)row.FindControl("tr")).Attributes["class"] = row.DataItemIndex % 2 == 0 ? "event" : "old";
	//    }
	//    return r;
	//}

	//void GetLackData()
	//{
	//    ld = new List<OrderDAO.ImportLackInfo>();
	//    foreach (ListViewDataItem row in lv.Items)
	//    {
	//        if (((TextBox)row.FindControl("t4")).Enabled == true)
	//        {
	//            var l = int.Parse(((TextBox)row.FindControl("t4")).Text);
	//            var c = ((TextBox)row.FindControl("t5")).Text;

	//            var tr = (HtmlTableRow)row.FindControl("tr");
	//            ld.Add(new OrderDAO.ImportLackInfo
	//            {
	//                ReceiveDetailId = int.Parse(tr.Attributes["title"]),
	//                CurrentLack = l,
	//                Comment = c
	//            });
	//        }
	//    }
	//}

	List<ReceiveHeader> rHeader = new List<ReceiveHeader>();
	List<ReceiveDetail> rDetail = new List<ReceiveDetail>();
	bool receiveOk = true;
	bool receiveNG = false;
	bool GetData()
	{
		var r = true;
		var WarehouseId = OrderDAO.GetOrderHeader(OrderId).ToLocation;
		foreach (var row1 in lv.Items)
		{
			var issue = ((Label)row1.FindControl("litIssue")).Text;
			var ngnumber = ((Label)row1.FindControl("litNGNumber")).Text;
			if (!string.IsNullOrEmpty(ngnumber)) receiveNG = true;
			var lvItems = (ListView)row1.FindControl("lvItems");
			if (lvItems.Enabled == false) continue;
			var hId = (long)lv.DataKeys[row1.DataItemIndex].Value;
			if (hId == 0)
			{
				rHeader.Add(new ReceiveHeader
				{
					ReceiveHeaderId = hId,
					IssueNumber = issue,
					NotGoodNumber = ngnumber,
					OrderHeaderId = OrderId,
					DealerCode = UserHelper.DealerCode,
					WarehouseId = WarehouseId,
					ReceiveDate = DateTime.Now,
					IsLocked = false,
					IsAutomatic = false,
					HasUndo = false,
					HasNGForm = false
				});
			}
			foreach (var row in lvItems.Items)
			{
				var s = int.Parse(((Literal)row.FindControl("litSQ")).Text);
				var q = int.Parse(((Literal)row.FindControl("litQQ")).Text);
				var g = int.Parse(((TextBox)row.FindControl("t1")).Text.Length == 0 ? "0" : ((TextBox)row.FindControl("t1")).Text);
				var b = int.Parse(((TextBox)row.FindControl("t2")).Text.Length == 0 ? "0" : ((TextBox)row.FindControl("t2")).Text);
				var w = int.Parse(((TextBox)row.FindControl("t3")).Text.Length == 0 ? "0" : ((TextBox)row.FindControl("t3")).Text);
				var l = int.Parse(((TextBox)row.FindControl("t4")).Text.Length == 0 ? "0" : ((TextBox)row.FindControl("t4")).Text);
				var c = ((TextBox)row.FindControl("t5")).Text;

				if (s != g + b + w + l || (s != g && string.IsNullOrEmpty(c)))
				{
					((HtmlTableRow)row.FindControl("tr")).Attributes["class"] = "error";
					r = false;
				}
				else ((HtmlTableRow)row.FindControl("tr")).Attributes["class"] = row.DataItemIndex % 2 == 0 ? "event" : "old";
				if (s != g) receiveOk = false;

				var tr = (HtmlTableRow)row.FindControl("tr");
				var dId = (long)lvItems.DataKeys[row.DataItemIndex].Value;
				var detail = new ReceiveDetail
				{
					ReceiveDetailId = dId,
					GoodQuantity = g,
					BrokenQuantity = b,
					WrongQuantity = w,
					LackQuantity = l,
					DealerComment = c,
					OrderHeaderId = OrderId,
					PartCode = tr.Cells[0].InnerText.Trim(),
					ShippingQuantity = s,
					ReceiveHeader = hId == 0 ? rHeader[rHeader.Count - 1] : null,
					IssueNumber = issue,
					EnglishName = tr.Cells[1].InnerText.Trim(),
					VietnamName = ((Literal)row.FindControl("litVN")).Text,
					UnitPrice = int.Parse(tr.Cells[4].InnerText.Trim())
				};
				if (rowChanged.Contains(dId) || dId == 0) rDetail.Add(detail);
			}
		}
		return r && rHeader.Count > 0;
	}

	List<long> rowChanged = new List<long>();
	protected void t_TextChanged(object sender, EventArgs e)
	{
		var row = (ListViewDataItem)((TextBox)sender).NamingContainer;
		var lv = (ListView)row.NamingContainer;
		var key = (long)lv.DataKeys[row.DataItemIndex].Value;
		if (key != 0 && !rowChanged.Contains(key)) rowChanged.Add(key);
	}

	protected bool CanEdit(long ReceiveHeaderId)
	{
		return (long)Eval("ReceiveHeaderId") == 0;
		//if (ngh != null)
		//{
		//    NGConfirmed = ngh.Status == NGStatus.Confirmed;
		//    NGFormId = ngh.NGFormHeaderId;
		//    list = NotGoodDAO.FindAllDetail(NGFormId);
		//}
		//else
		//{
		//    lv.Enabled = false;
		//    lblNGNotCreate.Visible = true;
		//}
	}
}
