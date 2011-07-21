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
            lotkey = Request.Params["key"];
            SessionVehicleDAO<VehicleInfo>.Init(lotkey);
            UpdateGridView();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        gv.DataBind();
    }

    private void GetListVehicles()
    {
        foreach (GridViewRow row in gv.Rows)
        {
            if (((CheckBox)row.Cells[0].Controls[1]).Checked)
            {
                SessionVehicleDAO<VehicleInfo>.AddVehicle(row.Cells[1].Text);
            }
            else
            {
                SessionVehicleDAO<VehicleInfo>.RemoveVehicle(row.Cells[1].Text);
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
            if (SessionVehicleDAO<VehicleInfo>.GetVehicle(engineNo) != null)
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
        SessionVehicleDAO<VehicleInfo>.SaveSession();
        Page.ClientScript.RegisterStartupScript(typeof(Vehicle_Sale_SearchVehicle), "closeThickBox", "self.parent.updated();", true);
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        SessionVehicleDAO<VehicleInfo>.ClearSession();
    }
}
