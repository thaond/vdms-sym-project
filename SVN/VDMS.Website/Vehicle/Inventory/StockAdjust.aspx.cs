using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.I.Vehicle;
using VDMS.Common.Utils;
using System.Drawing;

public partial class Vehicle_Inventory_StockAdjust : BasePage
{
    string adjKey = "stockadjust_key";

    public string PageKey;
    public void NewPageKey()
    {
        PageKey = Guid.NewGuid().ToString();
        SaveState("PageKey", PageKey);
    }
    public void LoadPageKey()
    {
        PageKey = (string)LoadState("PageKey");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            rvAdjustDate.MaximumValue = DateTime.MaxValue.ToShortDateString();
            rvAdjustDate.MinimumValue = DateTime.MinValue.ToShortDateString();
            ddlFromBranch.Type = VDMS.II.Entity.WarehouseType.Vehicle;
            ddlToBranch.Type = VDMS.II.Entity.WarehouseType.Vehicle;
            ddlFromBranch.DealerCode = UserHelper.DealerCode;
            ddlToBranch.DealerCode = UserHelper.DealerCode;
            if (!string.IsNullOrEmpty(UserHelper.BranchCode))
                ddlFromBranch.SelectedValue = UserHelper.BranchCode;
            txtAdjustDate.Text = DateTime.Now.ToShortDateString();
            NewPageKey();
            lnkSearchVehicles.NavigateUrl = string.Format("../Popup/SearchVehicle.aspx?key={0}&sessionkey={1}&TB_iframe=true&height=480&width=600", PageKey, adjKey);
        }
        else
        {
            LoadPageKey();
        }
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        gv.DataBind();
    }

    protected void ShowMessage(string message, bool isError)
    {
        lblMessage.Visible = true;
        lblMessage.Text = message;
        lblMessage.ForeColor = isError ? Color.Red : Color.RoyalBlue;
    }

    protected void btnCommit_Click(object sender, EventArgs e)
    {
        StockAdjustHelper.BindCommonData(ddlFromBranch.SelectedValue, ddlToBranch.SelectedValue, DataFormat.DateFromString(txtAdjustDate.Text));
        var msg = StockAdjustHelper.CheckAllAdjustingVehicles();
        if (msg != Resources.Message.ActionSucessful)
        {
            ShowMessage(msg, true);
            return;
        }
        StockAdjustHelper.AdjustAllVehicles();
        ShowMessage(Resources.Message.ActionSucessful, false);
        StockAdjustHelper.ClearSession();
        gv.DataBind();

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        StockAdjustHelper.ClearSession();
        gv.DataBind();
    }
    protected void gv_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        StockAdjustHelper.RemoveVehicle(e.NewSelectedIndex);
        StockAdjustHelper.SaveSession();
        e.Cancel = true;
        gv.DataBind();
    }
    protected void btnAddVehicle_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtEngineNo.Text))
        {
            var engineNo = txtEngineNo.Text.Trim().ToUpper();
            if (VehicleDAO.IsVehicleExisted(engineNo, ddlFromBranch.SelectedValue, ItemStatus.Imported))
            {
                StockAdjustHelper.AddVehicle(engineNo);
                StockAdjustHelper.SaveSession();
                gv.DataBind();
            }
            else
            {
                ShowMessage(Resources.Message.NoVehicle, true);
            }
        }
    }
}
