using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using VDMS.Helper;
using VDMS.II.Security;

public partial class Admin_Security_EditUser : BasePage
{
	string userName = null;
	string UserName
	{
		get
		{
			if (userName == null)
			{
				userName = this.Request.QueryString["UserName"];
				if (userName == null) userName = string.Empty;
			}
			return userName;
		}
	}
	string app = null;
	string App
	{
		get
		{
			if (app == null)
			{
				app = Request.QueryString["app"];
				if (app == null) Response.Redirect("ManageUsers.aspx");
			}
			return app;
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!this.IsPostBack)
		{
			// check permission
			if (!App.Equals(UserHelper.DealerCode, StringComparison.OrdinalIgnoreCase) && !UserHelper.IsSysAdmin)
			{
				mvMain.ActiveViewIndex = 1;
				AddErrorMsg(Message.AccessDenied);
			}

			mvMain.ActiveViewIndex = 0;
			InitErrMsgControl(vwCrError);

			if (!string.IsNullOrEmpty(UserName))
			{
				//MembershipUser user = Membership.GetUser(userName);
				MembershipUser user = MembershipDAO.GetUser(App, UserName);
				if (user != null)
				{
					txtUserName.Text = user.UserName;
					txtEmail.Text = user.Email;
					lblRegistered.Text = user.CreationDate.ToString("d");
					lblLastLogin.Text = user.LastLoginDate.ToString("d");
					lblLastActivity.Text = user.LastActivityDate.ToString("d");
					chkOnlineNow.Checked = user.IsOnline;
					chkApproved.Checked = user.IsApproved;
					chkLockedOut.Checked = user.IsLockedOut;
					chkLockedOut.Enabled = user.IsLockedOut;
					txtUserName.ReadOnly = true;
					//txtEmail.ReadOnly = true;
					mvProperties.ActiveViewIndex = 1;

					_UserProfile.Username = UserName;
					_UserProfile.DealerCode = App;
					_UserProfile.LoadProfile();
				}
				else
				{
					mvMain.ActiveViewIndex = 1;
					AddErrorMsg((string)GetLocalResourceObject("_InvalidInputData"));
					return;
				}
				btnResetPassword.OnClientClick = CreateConfirmScript((string)GetLocalResourceObject("_ResetPasswordQuestion"));
			}
			else
			{
				pnResetPass.Visible = false;
				_UserProfile.DealerCode = App;
			}
			BindRoles();
		}
	}

	private void BindRoles()
	{
		//VDMS.II.Entity.Role d;d.
		// fill the CheckBoxList with all the available roles, and then select
		// those that the user belongs to
		chklRoles.DataSource = RoleDAO.GetRoles(App);
		chklRoles.DataBind();
		if (!string.IsNullOrEmpty(UserName))
			foreach (string role in RoleDAO.GetRolesForUser(App, UserName))
			{
				ListItem item = chklRoles.Items.FindByText(role);
				if (item != null) item.Selected = true;
			}
	}

	void UpdateRoles(string aName, string uName)
	{
		// first remove the user from all roles...
		string[] currRoles = RoleDAO.GetRolesForUser(aName, uName);
		if (currRoles.Length > 0)
			RoleDAO.RemoveUserFromRoles(aName, uName, currRoles);
		// and then add the user to the selected roles
		List<string> newRoles = new List<string>();
		foreach (ListItem item in chklRoles.Items)
		{
			if (item.Selected)
				newRoles.Add(item.Text);
		}
		if (newRoles.Count != 0) RoleDAO.AddUserToRoles(aName, uName, newRoles.ToArray());
	}

	protected void btnUpdate_Click(object sender, EventArgs e)
	{
		if (Page.IsValid)
		{
			try
			{
				if (!string.IsNullOrEmpty(UserName))
				{
					var user = MembershipDAO.GetUser(App, UserName);
					if (!chkLockedOut.Checked)
					{
						user.UnlockUser();
						chkLockedOut.Enabled = false;
					}
					user.IsApproved = chkApproved.Checked;
					user.Email = txtEmail.Text;
					MembershipDAO.UpdateUser(App, user);

					SaveProfile(userName);
					UpdateRoles(App, UserName);
				}
				else
				{
					MembershipCreateStatus status;
					var newUser = MembershipDAO.AddNewUser(App, txtUserName.Text.Trim().ToUpper(), txtPassword.Text.Trim(), txtEmail.Text.Trim(), out status);
					if (newUser == null) lblSaveOk.Text = GetErrorMessage(status);
					else
					{
						SaveProfile(newUser.UserName);
						UpdateRoles(App, newUser.UserName);
						lblSaveOk.Text = Message.ActionSucessful;
						btnUpdate.Enabled = false;
					}
				}
				lblSaveOk.Visible = true;
			}
			catch (Exception exc)
			{
				lblSaveOk.Text = string.Format(Message.GeneralError, exc.Message);
			}
		}
	}

	void SaveProfile(string Username)
	{
		_UserProfile.SaveProfile(Username);
	}

	string GetErrorMessage(MembershipCreateStatus status)
	{
		switch (status)
		{
			case MembershipCreateStatus.DuplicateUserName:
				return CreateUserMsg.DuplicateUserName;

			case MembershipCreateStatus.DuplicateEmail:
				return CreateUserMsg.DuplicateEmail;

			case MembershipCreateStatus.InvalidPassword:
				return CreateUserMsg.InvalidPassword;

			case MembershipCreateStatus.InvalidEmail:
				return CreateUserMsg.InvalidEmail;

			case MembershipCreateStatus.InvalidAnswer:
				return CreateUserMsg.InvalidAnswer;

			case MembershipCreateStatus.InvalidQuestion:
				return CreateUserMsg.InvalidQuestion;

			case MembershipCreateStatus.InvalidUserName:
				return CreateUserMsg.InvalidUserName;

			case MembershipCreateStatus.ProviderError:
				return CreateUserMsg.ProviderError;

			case MembershipCreateStatus.UserRejected:
				return CreateUserMsg.UserRejected;

			default:
				return CreateUserMsg.Unknown;
		}
	}

	protected void btnResetPassword_Click(object sender, EventArgs e)
	{
		MembershipUser user = MembershipDAO.GetUser(App, UserName);
		string newPass = user.ResetPassword();
		if (user.ChangePassword(newPass, txtNewPassword.Text)) newPass = txtNewPassword.Text;
		lbResetPassResult.Text = string.Format((string)GetLocalResourceObject("_NewPassword"), newPass);
	}
}
