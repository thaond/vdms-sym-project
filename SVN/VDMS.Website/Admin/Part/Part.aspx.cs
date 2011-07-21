using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using VDMS.II.Entity;
using VDMS.II.PartManagement;
using VDMS.Helper;
using System.Data;
using VDMS.II.BasicData;

public partial class Admin_Part_Part : BasePage
{
	public string CategoryType { get; set; }

	protected void LoadCategory()
	{
		ddlCategory.CategoryType = rblType.SelectedValue;
		ddlCategory.DataBind();
		bAddNew.Enabled = rblType.SelectedValue == "A";
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		InitErrMsgControl(msg);
		InitInfoMsgControl(msg);

		CategoryType = rblType.SelectedValue;
		ddlCategory.CategoryType = this.CategoryType;
	}

	protected void rbl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		LoadCategory();
	}

	// update new entered value
	protected void ddlCat_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			DropDownList drop = (DropDownList)sender;
			long aId, catId;
			long.TryParse(drop.Attributes["AID"], out aId);
			long.TryParse(drop.SelectedValue, out catId);

			Accessory acs = AccessoryDAO.GetAccessory(aId);
			if ((catId > 0) && (acs != null) && (acs.PartInfos.Count == 1))
			{
				acs.PartInfos[0].CategoryId = catId;
			}
		}
		catch (Exception ex) { AddErrorMsg(ex.Message); }
	}

	protected void ddlAccType_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			DropDownList drop = (DropDownList)sender;
			long aId;
			long.TryParse(drop.Attributes["AID"], out aId);

			Accessory acs = AccessoryDAO.GetAccessory(aId);
			if (acs != null)
			{
				acs.AccessoryTypeCode = drop.SelectedValue;
			}
		}
		catch (Exception ex) { AddErrorMsg(ex.Message); }
	}

	protected void FavoriteIndexChanged(object sender, EventArgs e)
	{
		try
		{
			DropDownList drop = (DropDownList)sender;
			long fId;
			long.TryParse(drop.Attributes["FavID"], out fId);
			string partCode = drop.Attributes["PC"];

			Favorite fav = PartDAO.GetFavorite(fId);

			if (string.IsNullOrEmpty(drop.SelectedValue))
			{
				PartDAO.DeleteFavorite(fav);
			}
			else
			{
				if (fav == null && UserHelper.IsDealer && !string.IsNullOrEmpty(partCode))
				{
                    fav = new Favorite() { DealerCode = UserHelper.DealerCode, Type = drop.Attributes["Type"], PartCode = partCode, PartType = drop.Attributes["PT"] };
                    PartDAO.PartDC.Favorites.InsertOnSubmit(fav);
				}
				if (fav != null) fav.Rank = int.Parse(drop.SelectedValue);
			}
		}
		catch (Exception ex) { AddErrorMsg(ex.Message); }
	}
	protected void txtSafetyQuantity_OnTextChanged(object sender, EventArgs e)
	{
		try
		{
			TextBox txt = (TextBox)sender;
			long piId, wId;

			long.TryParse(txt.Attributes["WHID"], out wId);
			long.TryParse(txt.Attributes["PIID"], out piId);


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
		catch (Exception ex) { AddErrorMsg(ex.Message); }
	}
	protected void txtPartName_OnTextChanged(object sender, EventArgs e)
	{
		try
		{
			TextBox txt = (TextBox)sender;
			long aId;
			long.TryParse(txt.Attributes["AID"], out aId);
			string name = txt.Text.Trim();

			Accessory acs = AccessoryDAO.GetAccessory(aId);
			if (acs != null)
			{
				if (txt.Attributes["Target"] == "E") acs.EnglishName = txt.Text.Trim();
				else acs.VietnamName = txt.Text.Trim();
			}
		}
		catch (Exception ex) { AddErrorMsg(ex.Message); }
	}
	protected void txtUnitPrice_OnTextChanged(object sender, EventArgs e)
	{
		try
		{
			TextBox txt = (TextBox)sender;
			long pId;
			long.TryParse(txt.Attributes["PIID"], out pId);

			PartInfo pi = PartInfoDAO.GetPartInfo(pId);
			if ((pi != null) && !string.IsNullOrEmpty(txt.Text))
			{
				pi.Price = int.Parse(txt.Text);
			}
		}
		catch (Exception ex) { AddErrorMsg(ex.Message); }
	}

	// MultiView selector
	protected void SelectMode(object sender, EventArgs e)
	{
		MultiView mv = (MultiView)sender;
		mv.ActiveViewIndex = (rblMode.SelectedValue == "E") ? 0 : 1;
	}
	protected void SelectModeAndA(object sender, EventArgs e)
	{
		MultiView mv = (MultiView)sender;
		mv.ActiveViewIndex = ((rblMode.SelectedValue == "E") && (rblType.SelectedValue == "A")) ? 0 : 1;
	}
	protected void SelectModeAndP(object sender, EventArgs e)
	{
		MultiView mv = (MultiView)sender;
		mv.ActiveViewIndex = ((rblMode.SelectedValue == "E") && (rblType.SelectedValue == "P")) ? 0 : 1;
	}

	protected bool IsForAccessory()
	{
		return true;// :)) rblType.SelectedValue == PartType.Accessory;
	}
	protected void EnableEditFav(object sender, EventArgs e)
	{
		DropDownList drop = (DropDownList)sender;
		drop.Enabled = !string.IsNullOrEmpty(drop.SelectedValue);
	}

	/// //////////////////////////////////////////////////////////////////////////////////////////////////
	#region Eval methods

	protected int lastRowsCount = 1;
	long lastInfoId = 0;

	protected void GetInformation(long partInfoId)
	{
		if (lastInfoId != partInfoId)
		{
			// count warehouse to span row
			int rows = new PartInfoDAO().CountSafetiesForSetting(UserHelper.DealerCode);
			lastRowsCount = (rows == 0) ? 1 : rows;
			lastInfoId = partInfoId;
		}
	}
	public int GetSafetyRows(long partInfoId)
	{
		GetInformation(partInfoId);
		return lastRowsCount;
	}
	public List<PartSafetySetting> GetPartSafetyList(long partInfoId)
	{
		List<PartSafetySetting> list = new PartInfoDAO().FindSafetiesForSetting(partInfoId, UserHelper.DealerCode);

		return list;
	}

	public object EvalCatID(object category)
	{
		return (category == null) ? 0 : ((Category)category).CategoryId;
	}
	public object EvalCatName(object category, object tiptopCat)
	{
		Category cat;
		if ((category == null) && (tiptopCat != null))
		{
			cat = TipTopCategoryDAO.GetCategory(tiptopCat.ToString());
		}
		else
		{
			cat = (Category)category;
		}
		return (cat != null) ? cat.Name : "";
	}
	public object EvalFav(object fav)
	{
		return ((fav == null) || (int)fav == 0) ? "" : fav;
	}

	//public int GetFavoriteRank(object partCode, string type)
	//{
	//    int res = PartDAO.GetFavoriteRank(partCode.ToString(), UserHelper.DealerCode, type);

	//    return res;
	//}
	//public string EvalFavoriteRank(object rank)
	//{
	//    return ((rank!=null)&& (rank.ToString() != "0")) ? rank.ToString() : null;
	//}
	//public string EvalEngLish(object partCode)
	//{
	//    GetInformation(partCode.ToString());
	//    return engName;
	//}
	//public string EvalVietnamese(object partCode)
	//{
	//    GetInformation(partCode.ToString());
	//    return vietName;
	//}

	#endregion

	/// //////////////////////////////////////////////////////////////////////////////////////////////////


	protected void bQuery_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(lvParts.DataSourceID)) lvParts.DataSourceID = "odsParts";
		lvParts.DataBind();
	}

	protected void btnSave_OnLoad(object sender, EventArgs e)
	{
		(sender as WebControl).Visible = rblMode.SelectedValue == "E";
	}

	protected void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			PartInfoDAO.SaveChanged();
			lvParts.DataBind();
			AddInfoMsg(Resources.Message.ActionSucessful);
		}
		catch (Exception ex) { AddErrorMsg(ex.Message); }
	}
}
