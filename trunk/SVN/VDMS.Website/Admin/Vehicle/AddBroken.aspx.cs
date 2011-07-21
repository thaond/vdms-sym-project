using System;
using VDMS.Helper;
using Resources;
using VDMS.I.ObjectDataSource;

public partial class Admin_Database_AddBroken : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		lblLastUpdate.Text = DateTime.Now.ToShortDateString();
        lblEditBy.Text = UserHelper.Username;
		lblError.Visible = false;
	}

	private void Back()
	{
		Response.Redirect("Broken.aspx");
	}

	protected void btnCancel_Click(object sender, EventArgs e)
	{
		Back();
	}

	protected void btnSave_Click(object sender, EventArgs e)
	{
		BrokenDatasource broken = new BrokenDatasource();
        if (broken.Insert(txtBrokenCode.Text.Trim(), txtBrokenName.Text.Trim(), UserHelper.Username)) Back();
        else
        {
            lblError.Text = Message.AddBroken_BrokenCodeExist;
            lblError.Visible = true;
        }
	}
}
