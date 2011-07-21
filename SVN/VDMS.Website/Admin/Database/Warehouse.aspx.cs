using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.BasicData;
using VDMS.II.Entity;

public partial class Admin_Database_Warehouse : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        InitErrMsgControl(Msg);
    }

    protected void cmdCreateNew_Click(object sender, EventArgs e)
    {
        ResetControl(divRight);
        gv.EditIndex = -1;  // => ko check valid tren grid khi create new
        divRight.Visible = true;
    }

    protected void cmdSave_Click(object sender, EventArgs e)
    {
        wh1.GetInfo();
        Warehouse w = wh1.Warehouse;
        try
        {
            WarehouseDAO.Create(0, w.Code, ddlDealer.SelectedValue, w.Address, false);
            gv.DataBind();
        }
        catch (Exception ex)
        {
            base.AddErrorMsg(ex.Message);
        }
    }

    protected void gv_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Control lnkb = e.Row.FindControl("lnkbDelete");
            if (lnkb != null)
            {
                Warehouse wh = (Warehouse)e.Row.DataItem;
                lnkb.Visible = wh.OrderHeaders.Count == 0 && wh.Inventories.Count == 0 && wh.PartSafeties.Count == 0 && wh.ReceiveHeaders.Count == 0;
            }
        }
    }
}
