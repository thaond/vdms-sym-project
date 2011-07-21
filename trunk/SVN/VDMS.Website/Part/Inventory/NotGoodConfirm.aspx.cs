using System;
using VDMS.II.PartManagement;

public partial class Part_Inventory_NotGoodConfirm : BasePage
{
	int OrderId
	{
		get
		{
			int id = 0;
			int.TryParse(Request.QueryString["id"], out id);
			return id;
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{

	}

	protected void bSend_Click(object sender, EventArgs e)
	{
		NotGoodDAO.SendForm(OrderId);
		lblSaveOk.Visible = true;
		DisableButton();
		bBack.OnClientClick = "javascript:self.parent.updated(true);";
	}
}
