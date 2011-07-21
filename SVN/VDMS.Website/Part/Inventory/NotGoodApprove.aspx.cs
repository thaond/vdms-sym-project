using System;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.II.PartManagement;

public partial class Part_Inventory_NotGoodApprove : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (UserHelper.Profile.NGLevel == 1) ddlD.EnabledSaperateByArea = true;
	}

	protected void b1_Click(object sender, EventArgs e)
	{
		var item = (ListViewDataItem)(((Button)sender).NamingContainer);
		var id = long.Parse(((Button)sender).CommandArgument);
		var lv1 = (ListView)item.FindControl("lv1");
		var total = 0;
		foreach (ListViewDataItem row in lv1.Items)
		{
			var q = int.Parse(((TextBox)row.FindControl("t1")).Text);
			string comment = string.Empty;
			switch (UserHelper.Profile.NGLevel)
			{
				case 1:
					comment = ((TextBox)row.FindControl("t2")).Text;
					break;
				case 2:
					comment = ((TextBox)row.FindControl("t3")).Text;
					break;
				case 3:
					comment = ((TextBox)row.FindControl("t4")).Text;
					break;
				default:
					break;
			}

			NotGoodDAO.ApproveDetail((long)lv1.DataKeys[row.DataItemIndex].Value, q, comment);
			total += q;
		}
		NotGoodDAO.ApproveHeader(id, total);
		lv.DataBind();
	}

	protected void b2_Click(object sender, EventArgs e)
	{
		var id = long.Parse(((Button)sender).CommandArgument);
		NotGoodDAO.RejectHeader(id);
		lv.DataBind();
	}

	protected void b_Click(object sender, EventArgs e)
	{
		lv.DataBind();
	}
}
