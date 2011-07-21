using System;
using System.Web.UI;
using VDMS.Helper;
using VDMS.Provider;
using VDMS.II.Security;

public partial class MasterPage : System.Web.UI.MasterPage
{
	protected override void OnPreRender(EventArgs e)
	{
        //phVirtualDealer.Visible = UserHelper.IsSysAdmin && VDMSProvider.IsDebugEnabled;
        phVirtualDealer.Visible = UserHelper.IsAdmin || UserHelper.ApplicationName == "/";
		txtVirtualDealerCode.Text = UserHelper.DealerCode;
        WW.Visible = UserHelper.ProfileWarehouseId != UserHelper.WarehouseId;
        WW2.Visible = UserHelper.ProfileVBranch != UserHelper.BranchCode;
        try
        {
            if (UserHelper.IsAdmin || (UserHelper.ApplicationName == "/"))
            {
                dlVWarehouse.SelectedValue = UserHelper.VehicleWarehouseId.ToString();
                dlWarehouse.SelectedValue = UserHelper.WarehouseId.ToString();
            }
        }
        catch { }
	}

	protected override void OnLoad(EventArgs e)
	{
        
        // Edit by mvbinh 
		//Page.ClientScript.RegisterStartupScript(typeof(MasterPage), "MENU", @"$(document).ready(function(){$(""ul.sf-menu"").superfish({pathClass: 'current'});});", true);
        if ((bool)System.Web.HttpContext.Current.Session["IsDeniedURLWithProvider"]) Response.Redirect("~/AccessDeny.aspx");
        Page.ClientScript.RegisterArrayDeclaration("SysMsg", string.Format("'{0}', '{1}', '{2}', '{3}', '{4}'", Resources.Question.DeleteData, Resources.Question.SaveData, Resources.Message.NotEnoughtPermission, Resources.Question.UnloadBrowser, Resources.Question.DoAction));
		if (!Page.IsPostBack)
		{   
			if (smds.Provider.CurrentNode != null) Page.Title = smds.Provider.CurrentNode.Title;
			litTitle.Text = Page.Title;
			Page.Title = "VDMS - " + Page.Title;
		}
	}

	protected void imbUpdateVirtualDealerCode_Click(object sender, ImageClickEventArgs e)
	{
		UserHelper.SetSessionDealerCode(txtVirtualDealerCode.Text);
        dlVWarehouse.DataBind();
        dlWarehouse.DataBind();
        UserHelper.SetSessionVehicleWarehouseId(dlVWarehouse.SelectedWarehouseId);
        UserHelper.SetSessionWarehouseId(dlWarehouse.SelectedWarehouseId);
		Response.Redirect(Request.Url.AbsolutePath);
	}

	protected void ibUW_Click(object sender, ImageClickEventArgs e)
	{
        UserHelper.SetSessionVehicleWarehouseId(dlVWarehouse.SelectedWarehouseId);
        UserHelper.SetSessionWarehouseId(dlWarehouse.SelectedWarehouseId);
		Response.Redirect(Request.Url.AbsolutePath);
	}

	protected override void OnInit(EventArgs e)
	{
		ScriptManager1.EnableScriptLocalization = true;
		ScriptManager1.EnableScriptGlobalization = true;
        string ssmp = string.Concat(VDMSProvider.Language, "SiteMap", UserHelper.IsDealer ? ".dealer" : string.Empty);
        smds.SiteMapProvider = smp.SiteMapProvider = ssmp;
		base.OnInit(e);
	}

	protected void ls_LoggedOut(object sender, EventArgs e)
	{   
		Session.Clear();
	}
}
