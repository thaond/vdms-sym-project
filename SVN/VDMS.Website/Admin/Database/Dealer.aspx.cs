using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using VDMS.II.Entity;
using VDMS.II.Security;
using VDMS.Provider;

public partial class Admin_Security_Dealer : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{

	}

	protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			LinkButton b = (LinkButton)e.Row.Cells[0].Controls[0];
			b.OnClientClick = string.Format("javascript:location.href='DealerEdit.aspx?code={0}'; return false;", ((Dealer)e.Row.DataItem).DealerCode);
			HyperLink b1 = (HyperLink)e.Row.Cells[5].Controls[0];
			b1.Visible = !IsAccountAdminExist(((Dealer)e.Row.DataItem).DealerCode);
		}
	}

	bool IsAccountAdminExist(string app)
	{
		MembershipProvider prv = VDMSMembershipProvider.GetProvider(app);
		if (prv == null) return false;
		return prv.GetUser("admin", false) != null;
	}

    protected void btnFind_Click(object sender, EventArgs e)
    {
        gv.DataBind();
    }
}
