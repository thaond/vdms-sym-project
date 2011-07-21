using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Users : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			FillUsersGrid();
			FillRolesList();
			CheckDefaultUsers();
		}
	}

	private void FillUsersGrid()
	{

		gridUsers.DataSource = Membership.GetAllUsers();
		gridUsers.DataBind();
	}

	private void FillRolesList()
	{
		lbAllRoles.Items.Clear();
		String[] roles = Roles.GetAllRoles();
		foreach (String role in roles)
		{
			lbAllRoles.Items.Add(role);
		}
	}

	private void CheckDefaultUsers()
	{
		lnkCreateDefaultUsers.Visible = Membership.GetUser("root") == null && Membership.GetUser("user") == null;
	}

	protected void gridUsers_RowCommand(Object sender, GridViewCommandEventArgs e)
	{
		switch (e.CommandName)
		{
			case "EditProfile":
				EditProfile(e.CommandArgument.ToString());
				break;
			case "DeleteUser":
				DeleteUser(e.CommandArgument.ToString());
				break;
			case "EditRoles":
				EditRoles(e.CommandArgument.ToString());
				break;
		}
	}

	private void EditProfile(String userName)
	{
		//ProfileCommon userProfile = Profile.GetProfile(userName);

		//tbFirstName.Text = userProfile.FirstName;
		//tbLastName.Text = userProfile.LastName;
		//tbHomePage.Text = userProfile.HomePage;

		//lblProfileHeader.Text = String.Format("Edit profile for user \"{0}\":", userProfile.UserName);
		//hfName.Value = userName;

		//multiView.ActiveViewIndex = 1;
	}

	private void DeleteUser(String userName)
	{
		Membership.DeleteUser(userName);
		FillUsersGrid();
		CheckDefaultUsers();
	}

	private void EditRoles(String userName)
	{
		hfName.Value = userName;
		String[] roles = Roles.GetAllRoles();
		cblRoles.Items.Clear();
		ListItem item;
		foreach (String role in roles)
		{
			item = new ListItem(role);
			item.Selected = Roles.IsUserInRole(userName, role);
			cblRoles.Items.Add(item);
		}

		lblRolesHeader.Text = String.Format("Edit roles for user \"{0}\":", userName);
		multiView.ActiveViewIndex = 2;
	}

	protected void btSave_Click(Object sender, EventArgs e)
	{
		if (Page.IsValid)
		{
			SaveProfile(hfName.Value);
			multiView.ActiveViewIndex = 0;
		}
	}

	private void SaveProfile(String userName)
	{
		//ProfileCommon userProfile = Profile.GetProfile(userName);

		//userProfile.FirstName = tbFirstName.Text;
		//userProfile.LastName = tbLastName.Text;
		//userProfile.HomePage = tbHomePage.Text;

		//userProfile.Save();
	}

	protected void btCancel_Click(Object sender, EventArgs e)
	{
		multiView.ActiveViewIndex = 0;
	}

	protected void btSaveUserRoles_Click(Object sender, EventArgs e)
	{
		SaveUserRoles(hfName.Value);
		multiView.ActiveViewIndex = 0;
	}

	private void SaveUserRoles(String userName)
	{
		foreach (ListItem item in cblRoles.Items)
		{
			if (item.Selected)
			{
				if (!Roles.IsUserInRole(userName, item.Text))
				{
					Roles.AddUserToRole(userName, item.Text);
				}
			}
			else if (Roles.IsUserInRole(userName, item.Text))
			{
				Roles.RemoveUserFromRole(userName, item.Text);
			}
		}
	}

	protected void lnkCreateDefaultUsers_Click(object sender, EventArgs e)
	{
		CreateDefaultUsers();
		FillUsersGrid();
		FillRolesList();
	}

	private void CreateDefaultUsers()
	{
		String login1 = "admin";

        String pwd1 = "admin";

		String email1 = "root@example.com";

		String role = "administrators";

		try
		{

			Membership.CreateUser(login1, pwd1, email1);
			if (!Roles.RoleExists(role))
				Roles.CreateRole(role);
			Roles.AddUsersToRole(new String[] { login1 }, role);

			//Membership.CreateUser(login2, pwd2, email2);
			//if (!Roles.RoleExists(role))
			//    Roles.CreateRole(role);
			//Roles.AddUsersToRole(new String[] { login2 }, role2);

			//lbCreationStatus.Text = String.Format(
			//  "<br />User {0} identified by password {1} and user {2} identified by password {3} were created successfully.",
			//  login1, pwd1, login2, pwd2);

			lnkCreateDefaultUsers.Visible = false;
		}
		catch
		{
			//lbCreationStatus.Text = String.Format(
			//  "<br />Error occured: " + ex.Message + " Check if user {0} with password {1} and user {2} with password {3} already exist.",
			//  login1, pwd1, login2, pwd2);
		}
	}

	protected void lnkAddUser_Click(Object sender, EventArgs e)
	{
		multiView.ActiveViewIndex = 3;
	}

	protected void lnkAddNewRole_Click(Object sender, EventArgs e)
	{
		if (Page.IsValid)
		{
			CreateRole();
		}
	}

	private void CreateRole()
	{
		Roles.CreateRole(tbNewRoleName.Text);
		FillRolesList();
		tbNewRoleName.Text = String.Empty;
	}

	protected void btAddUser_Click(Object sender, EventArgs e)
	{
		if (Page.IsValid)
		{
			AddUser();
			FillUsersGrid();
			multiView.ActiveViewIndex = 0;
		}
	}

	private void AddUser()
	{
		try
		{
			Membership.CreateUser(tbLogin.Text, tbPassword.Text, tbEmail.Text);
		}
		catch (Exception exception)
		{
			lblMessage.Text = String.Format("Error has occured: {0}", exception.Message);
		}
	}
}
