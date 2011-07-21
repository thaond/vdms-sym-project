using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class MP_Popup : System.Web.UI.MasterPage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			//ServiceReference sr = new ServiceReference("~/Part/Inventory/FavouritePart.asmx");
			//ScriptManager1.Services.Add(sr);
		}
	}

	[WebMethod]
	public void MarkPart01(string PartNo, bool Marked)
	{
		//if (Marked) FavouritePartDAO.Mark(PartNo);
		//else FavouritePartDAO.UnMark(PartNo);
	}
}
