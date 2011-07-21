using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.PartManagement;

public partial class Admin_Part_PartEdit : BasePage
{
	public VDMS.II.Entity.PartInfo EditingPart = null;
	public List<PartSafetySetting> EditingPartSafeties = null;
	public long PartInfoId
	{
		get { long res; long.TryParse(Request.QueryString["id"], out res); return res; }
	}

	public bool IsCreateNew
	{
		get { return Request.QueryString["type"] == "C"; }
	}

	protected void LoadPartInfo(long partInfoId)
	{
		EditingPart = PartInfoDAO.GetPartInfo(partInfoId);
        Favorite fSale = PartDAO.GetFavorite(EditingPart.PartCode, EditingPart.PartType, EditingPart.DealerCode, "S");
        Favorite fOrder = PartDAO.GetFavorite(EditingPart.PartCode, EditingPart.PartType, EditingPart.DealerCode, "O");

        if (fSale != null)
        {
            ddlSaleRank.BindingSelectedValue = fSale.Rank.ToString();
            ddlSaleRank.DataBind();
        }
        if (fOrder != null)
        {
            ddlOrderRank.BindingSelectedValue = fOrder.Rank.ToString();
            ddlOrderRank.DataBind();
        }

		if (EditingPart != null)
		{
			tbEnName.Text = EditingPart.Accessory.EnglishName;
			tbVnName.Text = EditingPart.Accessory.VietnamName;
			tbPartCode.Text = EditingPart.PartCode;
			tbPrice.Text = EditingPart.Price.ToString();
			ddlCategory.BindingSelectedValue = EditingPart.CategoryId.ToString();
			ddlAccType.BindingSelectedValue = EditingPart.Accessory.AccessoryTypeCode;
			ddlCategory.DataBind();
			ddlAccType.DataBind();

			odsPartSafeties.SelectParameters["partInfoId"].DefaultValue = PartInfoId.ToString();
			odsPartSafeties.SelectParameters["dealerCode"].DefaultValue = UserHelper.DealerCode;
			gvSafeties.DataSourceID = odsPartSafeties.ID;
			gvSafeties.DataBind();
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		InitErrMsgControl(msg);
		InitInfoMsgControl(msg);

		if (!Page.IsPostBack)
		{
			ddlOrderRank.Enabled = false;

			if (PartInfoId > 0)
			{
				LoadPartInfo(this.PartInfoId);
			}

			if (this.IsCreateNew)
			{
				//odsPartSafeties.SelectParameters["partInfoId"].DefaultValue = EditingPart.PartInfoId.ToString();
				odsPartSafeties.SelectParameters["dealerCode"].DefaultValue = UserHelper.DealerCode;
				gvSafeties.DataSourceID = odsPartSafeties.ID;
				gvSafeties.DataBind();
			}

			if ((Session["SaveAccessory"] != null) && (Session["SaveAccessory"].ToString() == "ok"))
			{
				AddInfoMsg(Resources.Message.ActionSucessful);
				Session.Remove("SaveAccessory");
			}
		}
	}

	protected void ProcessFavorite(Favorite fav, bool used, string type, string partCode,string partType, string rank)
	{
		if (used)
		{
			var db = PartInfoDAO.PartDC;
            if (fav == null)
            {
                fav = new Favorite { DealerCode = UserHelper.DealerCode, Type = type, PartCode = partCode, PartType = partType };
                db.Favorites.InsertOnSubmit(fav);
            }
			int fRank;
			int.TryParse(rank, out fRank);
			fav.Rank = fRank;
		}
		else
		{
			PartDAO.PendingDeleteFavorite(fav);
		}
	}

	protected void ProcessSafety(long piId)
	{
		long wId;

		foreach (GridViewRow row in gvSafeties.Rows)
		{
			TextBox txt = (TextBox)row.FindControl("txtSafetyQuantity");
			long.TryParse(txt.Attributes["WHID"], out wId);

			PartSafety ps = PartInfoDAO.GetPartSafety(piId, wId);
			if (ps == null && (wId > 0) && (piId > 0))
			{
				ps = new PartSafety() { PartInfoId = piId, WarehouseId = wId };
				PartInfoDAO.PartDC.PartSafeties.InsertOnSubmit(ps);
			}
			if (ps != null)
			{
				int qty;
				int.TryParse(txt.Text, out qty);
				ps.SafetyQuantity = qty;
			}
		}
	}

	protected void bSave_Click(object sender, EventArgs e)
	{
		int price;
		int.TryParse(tbPrice.Text, out price);
		string partCode = tbPartCode.Text.Trim().ToUpper();

		if (PartDAO.IsPartCodeValidGlobal(partCode))//(VDMS.Data.TipTop.Part.IsPartExist(partCode, "ALL"))
		{
			AddErrorMsg(lbExistInTipTop.Text);
			return;
		}

		try
		{
			EditingPart = PartInfoDAO.SaveOrUpdate((string.IsNullOrEmpty(ddlCategory.SelectedValue)) ? null : (long?)long.Parse(ddlCategory.SelectedValue), UserHelper.DealerCode, partCode, PartType.Accessory, price);
			var acs = AccessoryDAO.SaveOrUpdate(partCode, UserHelper.DealerCode, tbEnName.Text.Trim(), tbVnName.Text.Trim(), ddlAccType.SelectedValue);

			if (EditingPart.Accessory == null)
			{
				EditingPart.AccessoryId = acs.AccessoryId;
			}

			// check Favorite
            var fSale = PartDAO.GetFavorite(partCode, EditingPart.PartType, UserHelper.DealerCode, "S");
            var fOrder = PartDAO.GetFavorite(partCode, EditingPart.PartType, UserHelper.DealerCode, "O");
            ProcessFavorite(fSale, ddlSaleRank.SelectedIndex > 0, "S", EditingPart.PartCode, EditingPart.PartType, ddlSaleRank.SelectedValue);
            ProcessFavorite(fOrder, ddlOrderRank.SelectedIndex > 0, "O", EditingPart.PartCode, EditingPart.PartType, ddlOrderRank.SelectedValue);

			// save safety stock
			ProcessSafety(EditingPart.PartInfoId);

			PartInfoDAO.SaveChanged();

			Session["SaveAccessory"] = "ok";
			string url = Request.Url.ToString();
			Response.Redirect(url.Remove(url.IndexOf('?')) + "?id=" + EditingPart.PartInfoId.ToString());
		}
		catch (Exception ex) { AddErrorMsg(ex.Message); }
	}
}
