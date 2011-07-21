using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using VDMS.I.Service;
using VDMS.Common.Web;
using VDMS.Helper;

public partial class Service_Popup_SelectSpares : BasePopup
{
	SelectSparesInfo Info { get; set; }
	#region Control state

	protected override object SaveControlState()
	{
		object[] ctlState = new object[2];
		ctlState[0] = base.SaveControlState();
		ctlState[1] = Info;
		return ctlState;
	}
	protected override void LoadControlState(object state)
	{
		if (state != null)
		{
			object[] ctlState = (object[])state;
			base.LoadControlState(ctlState[0]);
			Info = (SelectSparesInfo)ctlState[1];
		}
	}
	protected void Page_Init(object sender, EventArgs e)
	{
		Page.RegisterRequiresControlState(this);
	}

	#endregion

	protected void RefreshStatistic()
	{
		litSelectingPCVSparesCount.Text = Info.SelectingPCVSpares.Count.ToString();
		litSelectingSRSSparesCount.Text = Info.SelectingSRSSpares.Count.ToString();
	}

	public string EvalPartName(object nameEn, object nameVn)
	{
		return (string)((UserHelper.Language.Equals("vi-VN")) ? nameVn : nameEn);
	}

	protected void InitForm()
	{
		gvSelectSpare.PageSize = SrsSetting.selectSparePageSize;
		Info = new SelectSparesInfo();
	}

	// add/remove new spare to srs/pcv selecting list
	protected void AddSelectingSpare(int qtySRS, int qtyPCV, string litSpareNumber, string lbSpareNameVN, string lbSpareNameEN, string litModel, long warrTime, decimal warrLen, decimal labour, string hdManPower, decimal price, decimal newUnitPrice)
	{
		if (qtySRS > 0)
		{
			WarrantySpare item = new WarrantySpare(qtySRS, litSpareNumber, lbSpareNameVN, lbSpareNameEN, litModel, warrTime, warrLen, labour, hdManPower, price, newUnitPrice);
			AddSelectingSpareToList(Info.SelectingSRSSpares, item);
		}
		if (qtyPCV > 0)
		{
			WarrantySpare item = new WarrantySpare(qtyPCV, litSpareNumber, lbSpareNameVN, lbSpareNameEN, litModel, warrTime, warrLen, labour, hdManPower, price, newUnitPrice);
			AddSelectingSpareToList(Info.SelectingPCVSpares, item);
		}
	}
	protected void AddSelectingSpareToList(List<WarrantySpare> list, WarrantySpare item)
	{
		WarrantySpare oldItem = list.SingleOrDefault(w => w.Partcode.Equals(item.Partcode, StringComparison.OrdinalIgnoreCase));
		if (oldItem != null)
		{
			if (item.Quantity == 0) list.Remove(oldItem);
			else
			{
				oldItem.Quantity = item.Quantity;
				oldItem.Unitprice = item.Unitprice;
				oldItem.NewUnitPrice = item.NewUnitPrice;
			}
		}
		else if (item.Quantity > 0)
		{
			list.Add(item);
		}
	}
	protected void SelectSpareOnGridRow(GridViewRow row)
	{
		TextBox txtSRSQty = WebTools.FindControlById("txtAddSRSSpareQuantity", row) as TextBox;
		TextBox txtPCVQty = WebTools.FindControlById("txtAddPCVSpareQuantity", row) as TextBox;
		TextBox txtPrice = WebTools.FindControlById("txtAddSpareUnitPrice", row) as TextBox;
		Literal litOriPrice = WebTools.FindControlById("litOriginalSpareUnitPrice", row) as Literal;
		Label lbWarrTime = WebTools.FindControlById("lbWarrantyTime", row) as Label;
		Label lbWarrLength = WebTools.FindControlById("lbWarrantyLength", row) as Label;
		Label lbSpareNameEN = WebTools.FindControlById("lbSpareNameEN", row) as Label;
		Label lbSpareNameVN = WebTools.FindControlById("lbSpareNameVN", row) as Label;
		Literal litModel = WebTools.FindControlById("litModel", row) as Literal;
		Literal litSpareNumber = WebTools.FindControlById("litSelectedSpareNumber", row) as Literal;
		HiddenField hdLabour = WebTools.FindControlById("hdLabour", row) as HiddenField;
		HiddenField hdManPower = WebTools.FindControlById("hdManPower", row) as HiddenField;

		int qtySRS, qtyPCV;
		long warrTime;
		decimal warrLen, labour, price, newPrice;
		int.TryParse(txtSRSQty.Text, out qtySRS);
		int.TryParse(txtPCVQty.Text, out qtyPCV);
		long.TryParse(lbWarrTime.Text, out warrTime);
		decimal.TryParse(lbWarrLength.Text, out warrLen);
		decimal.TryParse(hdLabour.Value, out labour);
		decimal.TryParse(txtPrice.Text, out newPrice);
		decimal.TryParse(litOriPrice.Text, out price);
		//// update for empty data
		//txtPrice.Text = price.ToString();
		//txtQty.Text = qty.ToString();

		AddSelectingSpare(qtySRS, qtyPCV, litSpareNumber.Text, lbSpareNameVN.Text, lbSpareNameEN.Text, litModel.Text, warrTime, warrLen, labour, hdManPower.Value, price, newPrice);
	}

	protected void Page_PreRender(object sender, EventArgs e)
	{
		RefreshStatistic();
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			InitForm();
		}
	}
	protected void gvSelectxxx_OnRowCommand(object sender, GridViewCommandEventArgs e)
	{
		GridView gv = (GridView)sender;
		if ((e.CommandName == "Page") && (((e.CommandArgument == "Next") && ((gv.PageIndex + 1) == gv.PageCount)) || ((e.CommandArgument == "Prev") && (gv.PageIndex == 0))))
		{
			gv.DataBind();
		}

		udpSelectSpare.Update();
	}
	protected void gvSelectxxx_PreRender(object sender, EventArgs e)
	{
		GridView gv = (GridView)sender;
		if (gv.TopPagerRow == null) return;
		Literal litPageInfo = gv.TopPagerRow.FindControl("lit" + gv.ID + "PageInfo") as Literal;
		if (litPageInfo != null) litPageInfo.Text = string.Format(Message.PagingInfo, gv.PageIndex + 1, gv.PageCount, HttpContext.Current.Items["list" + gv.ID + "RowCount"]);
	}

	// not used
	protected void btnAddSpare_Click(object sender, EventArgs e)
	{
		long unitPrice = 0; int quantity = 0; string spareNumber = "", spareName = "";
		GridViewRow gvr = (GridViewRow)((Control)sender).NamingContainer;

		// unit price
		TextBox tbP = gvr.FindControl("txtAddSpareUnitPrice") as TextBox;
		if (tbP != null) long.TryParse(tbP.Text, out unitPrice);
		// quantity
		TextBox tbQ = gvr.FindControl("txtAddSpareQuantity") as TextBox;
		if (tbQ != null) int.TryParse(tbQ.Text, out quantity);
		// spare name
		Label lb = gvr.FindControl((UserHelper.Language.Equals("vi-VN", StringComparison.OrdinalIgnoreCase)) ? "lbSpareNameVN" : "lbSpareNameEN") as Label;
		if (lb != null) spareName = lb.Text;
		// spare number
		Literal lit = gvr.FindControl("litSelectedSpareNumber") as Literal;
		if (lb != null) spareNumber = lit.Text;

		//this.AddSRSItem(spareNumber, spareName, quantity, unitPrice);
	}
	protected void btnSelSpareOK_Click(object sender, EventArgs e)
	{
		//BindSRSItems();
		//udpSRSItems.Update();
	}
	protected void btnSelectSpare_Click(object sender, EventArgs e)
	{
		//Button btn = sender as Button;
		//GridViewRow gvr = gvSpareList.FooterRow;
		//Warrantycondition warr = WarrantyContent.GetWarrantyCondition(btn.CommandArgument);
		//if ((warr != null) && (gvr != null))
		//{
		//    TextBox txt = (WebTools.FindControlById("txtNewSpareName", gvr) as TextBox);
		//    if (txt != null) txt.Text = (UserHelper.Language.Equals("vi-VN")) ? warr.Partnamevn : warr.Partnameen;
		//    txt = (WebTools.FindControlById("txtNewSpareNumber", gvr) as TextBox);
		//    if (txt != null) txt.Text = warr.Partcode;
		//    txt = (WebTools.FindControlById("txtNewSparePrice", gvr) as TextBox);
		//    if (txt != null) txt.Text = warr.Unitprice.ToString();
		//}
		//BindSRSItems();
		//udpSRSItems.Update();
	}

	protected void gvSelectSpare_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			// update selected item
			Literal litSpareNumber = WebTools.FindControlById("litSelectedSpareNumber", e.Row) as Literal;
			Literal litSparePrice = WebTools.FindControlById("litOriginalSpareUnitPrice", e.Row) as Literal;
			TextBox txtPrice = WebTools.FindControlById("txtAddSpareUnitPrice", e.Row) as TextBox;
			TextBox txtSRSQty = WebTools.FindControlById("txtAddSRSSpareQuantity", e.Row) as TextBox;
			TextBox txtPCVQty = WebTools.FindControlById("txtAddPCVSpareQuantity", e.Row) as TextBox;

			WarrantySpare item = Info.SelectingSRSSpares.SingleOrDefault(w => w.Partcode.Equals(litSpareNumber.Text, StringComparison.OrdinalIgnoreCase));
			txtSRSQty.Text = (item != null) ? item.Quantity.ToString() : "0";
			txtPrice.Text = (item != null) ? item.Unitprice.ToString() : litSparePrice.Text;

			item = Info.SelectingPCVSpares.SingleOrDefault(w => w.Partcode.Equals(litSpareNumber.Text, StringComparison.OrdinalIgnoreCase));
			txtPCVQty.Text = (item != null) ? item.Quantity.ToString() : "0";

			// hilight action
			txtPCVQty.Attributes.Add("onchange", string.Format("HilightRow('{0}', '{1}', '{2}')", e.Row.ClientID, txtPCVQty.ClientID, txtSRSQty.ClientID));
			txtSRSQty.Attributes.Add("onchange", string.Format("HilightRow('{0}', '{1}', '{2}')", e.Row.ClientID, txtSRSQty.ClientID, txtPCVQty.ClientID));
			if ((txtPCVQty.Text != "0") || (txtSRSQty.Text != "0")) e.Row.CssClass = "readOnlyRow";
		}
	}
	protected void btnSearchSpare_Click(object sender, EventArgs e)
	{
		gvSelectSpare.DataSourceID = "odsSelectSpare";
		gvSelectSpare.DataBind();
	}
	protected void btnSelectSpare_DataBinding(object sender, EventArgs e)
	{
		Button btn = sender as Button;
		GridViewRow gvr = (GridViewRow)((Control)sender).NamingContainer;
		string name = "", price = "";
		if ((gvr != null))
		{
			Label lb = (WebTools.FindControlById((UserHelper.Language.Equals("vi-VN")) ? "lbSpareNameVN" : "lbSpareNameEN", gvr) as Label);
			if (lb != null) name = lb.Text;
			TextBox txt = (WebTools.FindControlById("txtAddSpareUnitPrice", gvr) as TextBox);
			if (txt != null) price = txt.Text;
			//btn.OnClientClick = string.Format("onSelectedSpare('{0}', '{1}', '{2}');", btn.CommandArgument, name.Replace("'", "\'"), price);
		}
	}
	//qty:OnTextChanged
	protected void UpdateSpareOnSelecting(object sender, EventArgs e)
	{
		GridViewRow row = ((sender as WebControl).NamingContainer as GridViewRow);
		SelectSpareOnGridRow(row);
	}
	protected void btnUpdateSelectingSpares_Click(object sender, EventArgs e)
	{
		foreach (GridViewRow row in gvSelectSpare.Rows)
		{
			SelectSpareOnGridRow(row);
		}
	}

	// select and close
	protected void btnAddSRSSpares_Click(object sender, EventArgs e)
	{
		PopupHelper.SetSelectSRSSparesSession(this.Key, Info.SelectingSRSSpares);
		ClosePopup("sparesSelected()");
	}
	protected void btnAddPCVSpares_Click(object sender, EventArgs e)
	{
		PopupHelper.SetSelectPCVSparesSession(this.Key, Info.SelectingPCVSpares);
		ClosePopup("sparesSelected()");
	}
	protected void btnAddSpares_Click(object sender, EventArgs e)
	{
		btnAddPCVSpares_Click(sender, e);
		btnAddSRSSpares_Click(sender, e);
	}

	protected void btnClearSelectingSRSSpares_Click(object sender, EventArgs e)
	{
		Info.SelectingSRSSpares.Clear();
	}
	protected void btnClearSelectingPCVSpares_Click(object sender, EventArgs e)
	{
		Info.SelectingPCVSpares.Clear();
	}
}
