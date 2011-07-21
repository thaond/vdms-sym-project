using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Data.TipTop;
using VDMS.Helper;
using VDMS.I.Vehicle;

public partial class Sales_Inventory_Check : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			txtFromDate.Text = DateTimeHelper.FirstDayInMonth.ToShortDateString();
			txtToDate.Text = DateTime.Now.ToShortDateString();
			ddlStatus.SelectedValue = ((int)DeliveredOrderStatus.NotDeliveredAll).ToString();
		}
		lbErr.Visible = false;
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
			default: return string.Empty; //ErrLabel
		}
	}

	private void getArea()
	{
		ddlArea.DataSource = Area.GetListArea();
		ddlArea.DataTextField = "AreaName";
		ddlArea.DataValueField = "AreaCode";
		ddlArea.DataBind();
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
}
