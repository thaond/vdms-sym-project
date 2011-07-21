using System;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using System.Web.UI.WebControls;

public partial class Admin_Database_ArticleEdit : BasePage
{
	int MessageId
	{
		get
		{
			int id = 0;
			int.TryParse(Request.QueryString["id"], out id);
			return id;
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			var m = MessageDAO.GetById(MessageId);
			if (m != null)
			{
				txtBody.Text = m.Body;
				phFileAttachment.Visible = false;
                cbhotnews.Checked = (m.Type == MessageType.HotMesssage) ? true : false;
                foreach (ListItem item in ddlDatabase.Items)
                {
                    if (item.Value == m.Flag)
                    {
                        item.Selected = true;
                        break;
                    }
                }
			}
		}
	}

	protected void btnUpdate_Click(object sender, EventArgs e)
	{
        if (Page.IsValid)
        {
            if (MessageId == 0)
            {
                File[] files = new File[3];
                files[0] = MessageDAO.CreateFile(fu1.PostedFile);
                files[1] = MessageDAO.CreateFile(fu2.PostedFile);
                files[2] = MessageDAO.CreateFile(fu3.PostedFile);

                MessageDAO.SaveMessage(txtBody.Text, ddlDatabase.SelectedValue, null, files, cbhotnews.Checked);
            }
            else MessageDAO.UpdateBody(MessageId, txtBody.Text, cbhotnews.Checked, ddlDatabase.SelectedValue);
            lblSaveOk.Visible = true;
            btnUpdate.Enabled = false;
        }
	}
}
