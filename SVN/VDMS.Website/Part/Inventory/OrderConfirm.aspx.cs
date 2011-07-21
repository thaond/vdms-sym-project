using System;
using VDMS.II.PartManagement.Order;
using VDMS;
using VDMS.Helper;

public partial class Part_Inventory_ConfirmOrder : BasePage
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
		if (!Page.IsPostBack)
		{
			if (VDMSSetting.CurrentSetting.OrderDateControl.Contains(UserHelper.DatabaseCode))
			{
				if ((UserHelper.Dealer.OrderDateControl & (2 << (int)DateTime.Now.DayOfWeek)) != 0)
				{
					bSend.Enabled = false;
					lblOrderDateSet.Visible = true;
				}
			}

			if (VDMSSetting.CurrentSetting.CheckOrderPartNotDuplicateBeforeConfirmWhenSend)
			{
				if (!VDMSSetting.CurrentSetting.ApplyCheckPartInSubOrder)
				{
					var order = OrderDAO.GetOrderHeader(OrderId);
					if (order.OrderType != "N") return;
				}
				if (OrderDAO.GetOrderDuplicate(OrderId) > 0)
				{
					bSend.Enabled = false;
					lblOrderDuplicate.Visible = true;
					hlEdit.Visible = true;
					hlEdit.NavigateUrl = string.Format("javascript:self.parent.checkduplicate({0});", OrderId);
				}
			}
		}
	}

	protected void bSend_Click(object sender, EventArgs e)
	{
		OrderDAO.SendOrder(OrderId, false);
		lblSaveOk.Visible = true;
		DisableButton();
		bBack.OnClientClick = "javascript:self.parent.updated(true);";
	}
}
