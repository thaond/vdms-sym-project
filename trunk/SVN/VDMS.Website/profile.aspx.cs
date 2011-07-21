using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.Linq;
using VDMS.Helper;

public partial class profile : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			tb1.Text = UserHelper.Username;
			tb2.Text = UserHelper.Profile.FullName;
		}
	}

	protected void b1_Click(object sender, EventArgs e)
	{
		var db = DCFactory.GetDataContext<SecurityDataContext>();
		var up = db.UserProfiles.SingleOrDefault(p => p.UserName == UserHelper.Username && p.DealerCode == UserHelper.DealerCode);
		if (up == null) return;
		up.FullName = tb2.Text;
		db.SubmitChanges();
		HttpContext.Current.Session["CurrentUser.Profile"] = null;
		lblSaveOk.Visible = true;
	}
}
