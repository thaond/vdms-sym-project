using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.Entity;

public partial class Admin_Controls_Warehouse : System.Web.UI.UserControl
{
	public Warehouse Warehouse { get; private set; }

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack && Warehouse != null)
		{
			tb1.Text = Warehouse.Code;
			//tb2.Text = Warehouse.Name;
			tb3.Text = Warehouse.Address;
		}
	}

	public void GetInfo()
	{
		Warehouse = new Warehouse()
		{
			Code = tb1.Text.Trim(),
			//Name = tb2.Text.Trim(),
			Address = tb3.Text.Trim()
		};
	}
}
