using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using VDMS.II.Linq;
using VDMS.II.Security;

public partial class _default : BasePage
{
	protected void Page_Load(object sender, EventArgs args)
	{
		if (!Page.IsPostBack)
		{   
			if (UserHelper.IsDealer)
			{
				// dealer only send and receive from VMEP => hide from and to column
				//mb2.Columns[1].Visible = false;

				if (!string.IsNullOrEmpty(Request.QueryString["id"])) Response.Redirect("default.aspx");
			}
			else
			{
				if (!string.IsNullOrEmpty(Request.QueryString["id"]))
				{
					var p = ProfileDAO.GetProfile(Request.QueryString["id"], "/");
					if (p.Position[0] >= UserHelper.Profile.Position[0]) Response.Redirect("default.aspx");
				}
				if (UserHelper.Profile.Position != "E")
				{
					t4.Visible = true;
					var p = UserHelper.Profile;
					ddlU.DataSource = ProfileDAO.GetEmployee(p.Position, p.Dept, p.DatabaseCode);
					ddlU.DataBind();
				}

				//divLeft.Attributes.Remove("style");
			}
		}
	}

	protected void bGo_Click(object sender, EventArgs e)
	{
		Response.Redirect("default.aspx?id=" + ddlU.SelectedValue);
	}

	protected void bt1_Click(object sender, EventArgs e)
	{
		mb1.DataBind();
	}

	protected void cmdSend_Click(object sender, EventArgs e)
	{
		File obj = MessageDAO.CreateFile(fuPM.PostedFile);

		var MessageId = MessageDAO.SaveMessage(txtMessage.Text.Trim(), MessageFlag.PrivateMassage, null, new File[] { obj },false);

		if (UserHelper.IsDealer)
		{
			var list = ProfileDAO.GetUsernameByProfile(rblP.SelectedValue, rblD.SelectedValue, UserHelper.DatabaseCode, UserHelper.AreaCode);
			foreach (var item in list)
			{
				MessageDAO.SendMessage(UserHelper.DealerCode, item, MessageId);
			}
		}
		else
		{
            foreach (GridViewRow row in dl.Rows)
            {
                MessageDAO.SendMessage(UserHelper.Username, row.Cells[1].Text, MessageId);
            }
		}

		ActionOk.Visible = true;
		txtMessage.Text = string.Empty;
		mb1.DataBind();
		mb2.DataBind();
	}

	protected void mb1_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			Admin_Controls_Quote q = (Admin_Controls_Quote)e.Row.FindControl("Quote1");
			MessageDAO.MessageDetail obj = (MessageDAO.MessageDetail)e.Row.DataItem;
			var db = DCFactory.GetDataContext<BasicDataContext>();
			var msgId = db.Messages.Single(p => p.MessageId == obj.MessageId).ParentId;
			if (msgId != null)
			{
				q.MessageId = msgId.Value;
				q.Refresh();
			}
			else q.Visible = false;
		}
	}

    private List<TempFile> AllFile = null;
    private List<TempFile> GetAllFile()
    {
        if (AllFile == null)
        {
            var db = DCFactory.GetDataContext<BasicDataContext>();
            AllFile = (from file in db.Files
                       select new TempFile
                                  {
                                      FileId = file.FileId,
                                      FileName = file.FileName,
                                      MessageId = file.MessageId
                                  }).ToList();
        }
        return AllFile;
    }
    protected List<TempFile> GetAttachment(long MessageId)
	{
		return GetAllFile().Where(p => p.MessageId == MessageId).ToList();
	}

	protected int GetOverSpan(DateTime d, int sp)
	{
		return (DateTime.Now - d).Days - sp;
	}
   protected class TempFile
   {
       public long MessageId { get; set; }
       public long FileId { get; set; }
       public string FileName { get; set; }
   }
    /// <summary>
    /// mvbinh: Checking file type from file name.
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
   protected bool isImage(string filename)
   {
       string[] s = { "png", "jpg", "jpeg", "bmp" };
       string[] t = filename.Split('.');
       if (t.Count() == 1)
           return false;
       return s.Contains(t.Last().ToLower());
   }


   protected void Refresh_Click(object sender, EventArgs args)
   {
       //  update the grids contents
       dl.DataBind();
   }
}

