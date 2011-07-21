using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Part_Inventory_UndoConfirm : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{

	}

	protected void cmdQuery_Click(object sender, EventArgs e)
	{
		gv.DataBind();
	}
}
