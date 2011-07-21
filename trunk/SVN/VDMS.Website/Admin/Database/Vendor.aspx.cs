using System;
using System.Data.Linq;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.II.Entity;

public partial class Admin_Database_Vendor : BasePage
{
	long EditVDId;

	protected void Page_Load(object sender, EventArgs e)
	{
		base.InitErrMsgControl(divRight);
		if (IsPostBack)
		{
			EditVDId = (long)LoadState("EditVDId");
		}
		else
		{
			SaveState("EditVDId", EditVDId);
		}
		cmdCreateNew.Visible = UserHelper.IsDealer;
	}

	protected void LoadVD()
	{
		ResetControl(divRight);
		divRight.Visible = true;

		Vendor vd = VendorDAO.GetVendor(EditVDId);
		if (vd != null)
		{
			txtCode.Text = vd.Code;
			txtName.Text = vd.Name;
			ci.LoadInfo(vd.Contact);
		}
	}

	protected bool EvalCanDelete(object TransactionHistories)
	{
		EntitySet<TransactionHistory> THs = (EntitySet<TransactionHistory>)TransactionHistories;
		return ((THs != null) && (THs.Count > 0)) ? false : true;
	}

	protected void cmdCreateNew_Click(object sender, EventArgs e)
	{
		ResetControl(divRight);
		EditVDId = 0;
		base.SaveState("EditVDId", EditVDId);
		divRight.Visible = true;
	}

	protected void gv_SelectedIndexChanged(object sender, EventArgs e)
	{
		GridView gv = (GridView)sender;
		long.TryParse(gv.SelectedDataKey.Value.ToString(), out EditVDId);
		SaveState("EditVDId", EditVDId);
		LoadVD();
	}

	protected void gv_RowDeleted(object sender, GridViewDeletedEventArgs e)
	{
		Response.Redirect(Request.Url.ToString());
	}
	
	protected void cmdSave_Click(object sender, EventArgs e)
	{
		Vendor vd = VendorDAO.GetVendor(txtCode.Text.Trim(), UserHelper.DealerCode);
		if ((vd != null) && (vd.VendorId != EditVDId))
		{
			base.AddErrorMsg(string.Format(Resources.Message.ItemAlreadyExist, txtCode.Text.Trim()));
		}
		else
		{
			VendorDAO.CreateOrUpdate(EditVDId, txtCode.Text, txtName.Text, UserHelper.DealerCode, ci.GetInfo());
			gv.DataBind();
			ResetControl(divRight);
			Response.Redirect(Request.Url.ToString());
		}
	}
}
