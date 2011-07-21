using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.I.Vehicle;
using VDMS.I.Entity;
using VDMS.I.Linq;

public partial class Vehicle_Sale_SearchVehicle : BasePopup
{
    string lotkey;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lotkey = Request.Params["sessionkey"];
            SaleVehicleHelper.InitSale(lotkey);
            ddlWarehouse.SelectedValue = VDMS.Helper.UserHelper.BranchCode;
            UpdateGridView();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (gv.DataSourceID == null) gv.DataSourceID = odsVehicles.ID;
        else gv.DataBind();
    }

    private void GetListVehicles()
    {
        foreach (GridViewRow row in gv.Rows)
        {
            if (((CheckBox)row.Cells[0].Controls[1]).Checked)
            {
                SaleVehicleHelper.AddVehicle(row.Cells[1].Text);
            }
            else
            {
                SaleVehicleHelper.RemoveVehicle(row.Cells[1].Text);
            }
        }
    }

    protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GetListVehicles();
    }

    private void UpdateGridView()
    {
        foreach (GridViewRow row in gv.Rows)
        {
            var engineNo = row.Cells[1].Text;
            if (SaleVehicleHelper.GetVehicle(engineNo) != null)
                ((CheckBox)row.Cells[0].Controls[1]).Checked = true;
        }
    }

    protected void gv_PreRender(object sender, EventArgs e)
    {
        UpdateGridView();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        GetListVehicles();
        SaleVehicleHelper.SaveSession();
        ClosePopup("update()");
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        SaleVehicleHelper.ClearSession();
    }
    protected void ddlModel_DataBound(object sender, EventArgs e)
    {
    }

}
