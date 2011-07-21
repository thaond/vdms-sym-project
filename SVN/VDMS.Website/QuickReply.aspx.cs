using System;
using System.Linq;
using System.Web.UI;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using VDMS.II.Linq;

public partial class QuickReply : BasePage
{
	int MessageBoxId
	{
		get
		{
			int id;
			int.TryParse(Request.QueryString["id"], out id);
			if (id == 0) Response.Redirect("~/");
			return id;
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			var db = DCFactory.GetDataContext<BasicDataContext>();
			Quote1.MessageId = db.MessageBoxes.Single(p => p.MessageBoxId == MessageBoxId).MessageId;
			SaveState("ParentId", Quote1.MessageId);
		}
	}

	protected void b1_Click(object sender, EventArgs e)
	{
		long ParentId = (long)LoadState("ParentId");
		long MessageId = MessageDAO.SaveMessage(tb1.Text, MessageFlag.IsReplyFlag, ParentId, null,false);

		var db = DCFactory.GetDataContext<BasicDataContext>();
		var mb = db.MessageBoxes.SingleOrDefault(p => p.MessageBoxId == MessageBoxId);
		MessageDAO.SendMessage(UserHelper.IsDealer ? UserHelper.DealerCode : UserHelper.Username, mb.FromUser, MessageId);
		mb.Flag = MessageFlag.AnswerFlag;
		db.SubmitChanges();

		//  register the script to close the popup
		Page.ClientScript.RegisterStartupScript(typeof(QuickReply), "closeThickBox", "self.parent.updated();", true);
	}
}
