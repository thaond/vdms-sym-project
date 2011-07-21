using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.Security;
using VDMS.II.Entity;
using VDMS.Helper;

public partial class Admin_Security_TaskSetting : BasePopup
{
	string url
	{
		get
		{
			return Request.QueryString["url"];
		}
	}

	string app
	{
		get
		{
			return Request.QueryString["app"];
		}
	}

	string role
	{
		get
		{
			return Request.QueryString["role"];
		}
	}

	Task AddedTask = null;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
			table1.Visible = gv1.Columns[gv1.Columns.Count - 1].Visible = UserHelper.IsSysAdmin;
	}

	protected void ibAddTask_Click(object sender, ImageClickEventArgs e)
	{
		AddedTask = PermissionDAO.CreateOrUpdateTask(tb2.Text, tb1.Text, url);
		PermissionDAO.SaveChanged();

		gv1.DataBind();
		//udpRoleTask.Update();
	}

	protected void imbDeleteTask_Click(object sender, ImageClickEventArgs e)
	{
		GridViewRow row = (sender as WebControl).NamingContainer as GridViewRow;
		Label txtCmd = row.FindControl("lbCommandName") as Label;
		PermissionDAO.DeleteTask(url, txtCmd.Text);
		PermissionDAO.SaveChanged();
		//BindTaskList(tvRole.SelectedValue);
		//udpRoleTask.Update();
	}

	protected void cmdRefreshTask_Click(object sender, EventArgs e)
	{
		//BindTaskList(tvRole.SelectedValue);
		//udpRoleTask.Update();
	}

	protected void gv1_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			Task task = e.Row.DataItem as Task;
			CheckBox chb = e.Row.FindControl("chkAllow") as CheckBox;
			chb.Checked = !Security.IsDeniedTask(url, app, role, task.CommandName);
		}
	}

	protected void cmdSaveTask_Click(object sender, EventArgs e)
	{
		PermissionDAO.DeleteAllRoleTask(app, role, url);
		foreach (GridViewRow row in gv1.Rows)
		{
			CheckBox chb = row.FindControl("chkAllow") as CheckBox;
			if (chb.Checked) PermissionDAO.CreateOrUpdateRoleTask(app, role, url, chb.ToolTip);
		}
		PermissionDAO.SaveChanged();
        ClosePopup("updated()");
	}
}
