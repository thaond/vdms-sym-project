using System;
using System.Linq;
using System.Web.UI.WebControls;
using VDMS.II.Entity;
using VDMS.II.Linq;
using VDMS.Helper;

public partial class Part_Inventory_SpecialIE : BasePage
{
	public string PageKey;
	public void NewPageKey()
	{
		PageKey = Guid.NewGuid().ToString();
		_PageKey.Value = PageKey;
		SaveState("PageKey", PageKey);

		odsPartList.DeleteParameters["key"].DefaultValue = PageKey;
		odsPartList.SelectParameters["key"].DefaultValue = PageKey;
		odsExportPart.DeleteParameters["key"].DefaultValue = PageKey;
		odsExportPart.SelectParameters["key"].DefaultValue = PageKey;
	}

	public void LoadPageKey()
	{
		PageKey = (string)LoadState("PageKey");
		odsPartList.DeleteParameters["key"].DefaultValue = PageKey;
		odsPartList.SelectParameters["key"].DefaultValue = PageKey;
		odsExportPart.DeleteParameters["key"].DefaultValue = PageKey;
		odsExportPart.SelectParameters["key"].DefaultValue = PageKey;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		InitInfoMsgControl(msg);
		InitErrMsgControl(msg);

		if (!IsPostBack)
		{
			NewPageKey();
			gvImp.PageSize = int.Parse(ddlRows.Text);
			gvExp.PageSize = int.Parse(ddlRows2.Text);
		}
		else
		{
			LoadPageKey();
		}
	}

	protected void ddlPRT_SelectedIndexChanged(object sender, EventArgs e)
	{
		phNG.Visible = ddlPRT.SelectedIndex == 1;
	}

	protected void cvNG_ServerValidate(object source, ServerValidateEventArgs args)
	{
		var db = DCFactory.GetDataContext<PartDataContext>();
		args.IsValid = db.NGFormHeaders.SingleOrDefault(p => p.NotGoodNumber == args.Value && p.DealerCode == UserHelper.DealerCode) != null;
	}

	protected void cmdAddRow_Click(object sender, EventArgs e)
	{
		if ((sender as WebControl).ID == "cmdAddRow")
		{
			SpecialIEDAO.Append(int.Parse(ddlRowCount.Text), SpecialIEType.Import, this.PageKey);
			this.gvImp.DataBind();
		}
		else       //"cmdAddRow2"
		{
			SpecialIEDAO.Append(int.Parse(ddlRowCount2.Text), SpecialIEType.Export, this.PageKey);
			this.gvExp.DataBind();
		}
	}

	protected void ddlWarehouse_OnSelectedIndexChanged(object sender, EventArgs e)
	{
		cmdSearch.Attributes["onclick"] = string.Format("javascript:showSearch(this, {0})", ddlWarehouse.SelectedValue);
        cmdSearch2.Attributes["onclick"] = string.Format("javascript:showSearch2(this, {0})", ddlWarehouseE.SelectedValue);
	}

	protected void ddlRows_SelectedIndexChanged(object sender, EventArgs e)
	{
		if ((sender as WebControl).ID == "ddlRows")
		{
			gvImp.PageSize = int.Parse(ddlRows.Text);
			gvImp.DataBind();
		}
		else
		{
			gvExp.PageSize = int.Parse(ddlRows2.Text);
			gvExp.DataBind();
		}
	}

	protected void UpdatePartInfo(object sender, EventArgs e)
	{
		TextBox tbPartNo = (TextBox)(sender as WebControl).Parent.FindControl("txtPartNo");
		TextBox tbPartQty = (TextBox)(sender as WebControl).Parent.FindControl("txtQty");
		TextBox tbUnitPrice = (TextBox)(sender as WebControl).Parent.FindControl("txtUnitPrice");
		TextBox tbPartRemark = (TextBox)(sender as WebControl).Parent.FindControl("txtRemark");
		DropDownList ddlVendor = (DropDownList)(sender as WebControl).Parent.FindControl("ddlVendor");
		DropDownList ddlPartType = (DropDownList)(sender as WebControl).Parent.FindControl("ddlPartType");
		HiddenField hdPartKey = (sender as WebControl).Parent.FindControl("hdPartKey") as HiddenField;

		SpecialIEType type = ((sender as WebControl).NamingContainer.NamingContainer.ID == "gvExp") ? SpecialIEType.Export : SpecialIEType.Import;

		SpecialIEDAO.UpdatePart(this.PageKey, type, new Guid(hdPartKey.Value), tbPartNo.Text.Trim(), /*ddlPartType.SelectedValue*/ null, ((ddlVendor == null) || string.IsNullOrEmpty(ddlVendor.SelectedValue)) ? (long?)null : long.Parse(ddlVendor.SelectedValue), (tbPartQty.Text.Trim() != "") ? int.Parse(tbPartQty.Text) : 0, (tbUnitPrice.Text.Trim() != "") ? int.Parse(tbUnitPrice.Text) : 0, tbPartRemark.Text.Trim());
	}

	protected void gv1_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			DropDownList ddlVendor = (DropDownList)e.Row.FindControl("ddlVendor");
			try
			{
				ddlVendor.SelectedValue = (e.Row.DataItem as SpecialIE).VendorId.ToString();
				e.Row.CssClass = ((e.Row.DataItem as SpecialIE).State == SIE_ItemState.NotFound) ? "errLine" : "";
			}
			catch { }
		}
	}

	protected void btnImport_Click(object sender, EventArgs e)
	{
		if (!Page.IsValid) return;
		if (SpecialIEDAO.GetImportParts(this.PageKey).Count <= 0)
		{
			gvImp.DataBind();
		}
		else
		{
			try
			{
				string vch = SpecialIEDAO.DoImport(this.PageKey, long.Parse(ddlWarehouse.SelectedValue));
				SpecialIEDAO.ClearImportPart(this.PageKey, SIE_ItemState.Imported);
				gvImp.DataBind();
				if (gvImp.Rows.Count == 0)
				{
					AddInfoMsg(Resources.Message.ActionSucessful);
					AddInfoMsg(Resources.Constants.VoucherNo + " : " + vch);
					NewPageKey();

					if (ddlPRT.SelectedIndex == 1)
					{
						var db = DCFactory.GetDataContext<PartDataContext>();
						var id = db.NGFormHeaders.SingleOrDefault(p => p.NotGoodNumber == txtNG.Text && p.DealerCode == UserHelper.DealerCode).NGFormHeaderId;
						litPopupJS.Text = string.Format(@"<script type=""text/javascript"">$(document).ready(function() {{tb_show(""Update Not Good data"", ""NotGoodView.aspx?id={0}&TB_iframe=true&height=320&width=600"");}});</script>", id);
					}
				}
				else AddErrorMsg(Resources.Message.SomeItemHasError);
			}
			catch (Exception ex) { AddErrorMsg(ex.Message); }
		}
	}

	protected void btnExport_Click(object sender, EventArgs e)
	{
		if (SpecialIEDAO.GetExportParts(this.PageKey).Count <= 0)
		{
			gvExp.DataBind();
		}
		else
		{
			try
			{
                string vch = SpecialIEDAO.DoExport(this.PageKey, long.Parse(ddlWarehouseE.SelectedValue));
				SpecialIEDAO.ClearExportPart(this.PageKey, SIE_ItemState.Exported);
				gvExp.DataBind();
				if (gvExp.Rows.Count == 0)
				{
					AddInfoMsg(Resources.Message.ActionSucessful);
					AddInfoMsg(Resources.Constants.VoucherNo + " : " + vch);
					NewPageKey();
				}
				else AddErrorMsg(Resources.Message.SomeItemHasError);
			}
			catch (Exception ex) { AddErrorMsg(ex.Message); }
		}
	}

	protected void btnPartInserted_Click(object sender, EventArgs e)
	{
		if (tabCont.ActiveTab.ID == "tabImp")
		{
			gvImp.DataBind();
			udpImport.Update();
		}
		else
		{
			gvExp.DataBind();
			udpExport.Update();
		}
	}
}
