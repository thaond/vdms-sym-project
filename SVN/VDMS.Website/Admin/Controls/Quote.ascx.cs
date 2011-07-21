using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.Linq;
using System.ComponentModel;

public partial class Admin_Controls_Quote : System.Web.UI.UserControl
{
	public long MessageId { get; set; }

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack && MessageId != 0) Refresh();
	}

	public void Refresh()
	{
		if (MessageId != 0)
		{
			var db = DCFactory.GetDataContext<BasicDataContext>();
			var msg = db.Messages.SingleOrDefault(p => p.MessageId == MessageId);
			if (msg.ParentId != 0)
			{
			}
			lb1.Text = msg.CreatedBy;
			l2.Text = msg.Body;
		}
		else this.Visible = false;
	}
}
