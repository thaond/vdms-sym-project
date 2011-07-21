using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.Linq;
using VDMS.II.PartManagement;
using VDMS.II.BasicData;
using System.Data.Linq;

public partial class Admin_Part_Customer : BasePage
{
    long EditCustId;

    protected void Page_Load(object sender, EventArgs e)
    {
        base.InitErrMsgControl(divRight);
        if (IsPostBack)
        {
            EditCustId = (long)LoadState("EditCustId");
        }
        else
        {
            SaveState("EditCustId", EditCustId);
        }
        cmdCreateNew.Visible = UserHelper.IsDealer;
    }

    protected void LoadCus()
    {
        ResetControl(divRight);
        divRight.Visible = true;

        Customer cus = CustomerDAO.GetCustomer(EditCustId);
        if (cus != null)
        {
            txtCode.Text = cus.Code;
            txtName.Text = cus.Name;
            txtVAT.Text = cus.VATCode;
            ci.LoadInfo(cus.Contact);
        }
    }

    protected bool EvalCanDelete(object SalesHeaders)
    {
        EntitySet<SalesHeader> SHs = (EntitySet<SalesHeader>)SalesHeaders;
        return ((SHs != null) && (SHs.Count > 0)) ? false : true;
    }

    protected void cmdCreateNew_Click(object sender, EventArgs e)
    {
        ResetControl(divRight);
        EditCustId = 0;
        base.SaveState("EditCustId", EditCustId);
        divRight.Visible = true;
    }

    protected void cmdSave_Click(object sender, EventArgs e)
    {
        Customer cus = CustomerDAO.GetCustomer(txtCode.Text.Trim(), UserHelper.DealerCode);
        if ((cus != null) && (cus.CustomerId != EditCustId))
        {
            base.AddErrorMsg(string.Format(Resources.Message.ItemAlreadyExist, txtCode.Text.Trim()));
        }
        else
        {
            CustomerDAO.CreateOrUpdate(EditCustId, txtCode.Text, txtName.Text, txtVAT.Text, UserHelper.DealerCode, ci.GetInfo());
            gv.DataBind();
            divRight.Visible = false;
            Response.Redirect(Request.Url.ToString());
        }
    }

    protected void gv_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        long.TryParse(gv.SelectedDataKey.Value.ToString(), out EditCustId);
        SaveState("EditCustId", EditCustId);
        LoadCus();
    }
    protected void gv_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }
}
