using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Part_Inventory_SearchPartUI : System.Web.UI.UserControl
{
	public string SearchOption { get; set; }

	protected void Page_Load(object sender, EventArgs e)
	{
		h1.Attributes.Add("onclick", string.Format("javascript:showSearch(this, '{0}')", SearchOption));
	}

	protected void cmdUpdateModel_Click(object sender, EventArgs e)
	{
		ddl1.Items.Clear();
		ddl1.Items.Add(new ListItem());
		ddl1.DataBind();
	}

	protected void ddl3_DataBound(object sender, EventArgs e)
	{
		foreach (ListItem item in ddl1.Items)
		{
			if (!string.IsNullOrEmpty(item.Text))
			{
				item.Text = string.Format("{0} ({1})", item.Value, item.Text);
			}
		}
	}
}
