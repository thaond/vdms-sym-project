using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.Security;
using VDMS.II.Entity;
using System.Web.Security;

public partial class Admin_Database_CreateDealerAdmin : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			txtDealer.Text = Request.QueryString["d"];
		}
	}

	protected void cmdCreate_Click(object sender, EventArgs e)
	{
		string userName = "admin";
		string email = "email@abc.com";
		bool done = false;

		if (MembershipDAO.AddNewUser(txtDealer.Text, userName, txtPassword.Text, email) == MembershipCreateStatus.Success)
		{
			Role adminRole = RoleDAO.GetRootRoles(txtDealer.Text);
			if (adminRole == null) adminRole = RoleDAO.CreateAdminRole(txtDealer.Text);
			if (adminRole != null)
			{
				RoleDAO.AddUserToRole(txtDealer.Text, adminRole.RoleName, userName);
				ActionOk.Visible = true;
				//phCreateAdmin.Visible = false;
				//gv.DataBind();
				done = true;
			}
		}

		ActionFaild.Visible = !done;
		cmdCancel.OnClientClick = "javascript:self.parent.updated(true);";
	}
}
