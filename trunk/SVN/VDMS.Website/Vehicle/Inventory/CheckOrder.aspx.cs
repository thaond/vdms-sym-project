using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.I.Vehicle;

public partial class Sales_Inventory_CheckOrder : BasePage
{
	//string OrderNumberTemp = "";

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			txtFromDate.Text = DateTimeHelper.FirstDayInMonth.ToShortDateString();
			txtToDate.Text = DateTime.Now.ToShortDateString();
			ddlStatus.SelectedValue = ((int)DeliveredOrderStatus.NotDeliveredAll).ToString();

			if (UserHelper.IsDealer)
			{
				//ddlDealer.SelectedValue = UserHelper.DealerCode;
                txtDealerCode.Text = UserHelper.DealerCode;
			}
            ddlArea.Enabled = txtDealerCode.Enabled = !UserHelper.IsDealer;
		}
		rvFromDate = DateValid(rvFromDate);
		rvToDate = DateValid(rvToDate);
	}

	protected void btnSearch_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(lv.DataSourceID)) lv.DataSourceID = "odsOrders";
		else lv.DataBind();
	}

	protected string GetOrderStatus(int StatusIndex)
	{
		switch (StatusIndex)
		{
			//case 0: return ddlStatus.Items[0].Text;
			case 0: return ddlStatus.Items[1].Text;
			case 1: return ddlStatus.Items[1].Text;
			case 2: return ddlStatus.Items[2].Text;
			default:
				{
					return ""; //ErrLabel
				}
		}
	}
	protected string EvalLackColor(object order, object imp)
	{
		if ((imp == null) || (imp == DBNull.Value)) return "red";
		if ((order == null) || (order == DBNull.Value)) return "inherit";
		return (int)order - (int)imp == 0 ? "inherit" : "red";
	}
	protected int EvalLack(object order, object imp)
	{
		int s = ((imp == null) || (imp == DBNull.Value)) ? 0 : (int)imp;
		int o = ((order == null) || (order == DBNull.Value)) ? 0 : (int)order;
		return o - s;
	}
	protected string ReturnShortDate(string dt)
	{
		DateTime objdt = new DateTime();
		if (!dt.Equals(""))
		{
			objdt = DateTime.Parse(dt);
		}
		return objdt.ToShortDateString();
	}

	private RangeValidator DateValid(RangeValidator rvDate)
	{
		rvDate.MinimumValue = DateTime.MinValue.ToShortDateString();
		rvDate.MaximumValue = DateTime.MaxValue.ToShortDateString();
		return rvDate;
	}
	protected void lnkViewShipping_DataBinding(object sender, EventArgs e)
	{
		LinkButton link = (LinkButton)sender;
		link.OnClientClick = string.Format("window.open('ShippingIssues.aspx?on={0}','ShippingIssues{1}',''); return false;", link.Text, link.Text);
	}
	protected void ddlDealer_SelectedIndexChanged(object sender, EventArgs e)
	{
        ddlWarehouse.DealerCode = txtDealerCode.Text.Trim().ToUpper();// ddlDealer.SelectedValue;
		ddlWarehouse.DataBind();
	}
}
