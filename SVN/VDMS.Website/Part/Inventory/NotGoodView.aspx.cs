using System;
using System.Linq;
using VDMS.Helper;
using VDMS.II.Linq;
using VDMS.II.Entity;
using System.Web.UI.WebControls;

public partial class Part_Inventory_NotGoodView : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			if (UserHelper.IsVietnamLanguage) gv1.Columns[1].Visible = false;
			else gv1.Columns[2].Visible = false;

			var db = DCFactory.GetDataContext<PartDataContext>();
			var id = 0;
			int.TryParse(Request.QueryString["id"], out id);
			var ng = db.NGFormHeaders.SingleOrDefault(p => p.NGFormHeaderId == id);
			if (ng == null) Response.End();
			lblNG.Text = ng.NotGoodNumber;

			if (ng.NGType == NGType.Normal)
			{
				gv1.Columns[5].Visible = false;
			}
			else
			{
				gv1.Columns[4].Visible = false;
			}
		}
	}

	protected void bSend_Click(object sender, EventArgs e)
	{
		var db = DCFactory.GetDataContext<PartDataContext>();
		foreach (GridViewRow item in gv1.Rows)
		{
			var id = (long)gv1.DataKeys[item.DataItemIndex].Value;
			var nd = db.NGFormDetails.Single(p => p.NGFormDetailId == id);
			var q = 0;
			int.TryParse(((TextBox)item.FindControl("t1")).Text, out q);
			nd.ProblemAgainQuantity = q;
			nd.Passed = ((CheckBox)item.FindControl("c1")).Checked;
			if (!string.IsNullOrEmpty(((TextBox)item.FindControl("t2")).Text)) nd.TransactionComment = ((TextBox)item.FindControl("t2")).Text;
			db.SubmitChanges();
		}
		lblSaveOk.Visible = true;
		DisableButton();
	}
}
