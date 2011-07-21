using System;
using System.Web;
using System.Web.UI.WebControls;
using VDMS.I.Vehicle;
using VDMS.Helper;

public partial class Sales_Inventory_Orderform : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string s = HttpContext.Current.Request.Url.DnsSafeHost.ToLower();
		if (!IsPostBack)
		{
			txtFromDate.Text = DateTimeHelper.FirstDayInMonth.ToShortDateString();
			txtToDate.Text = DateTime.Now.ToShortDateString();

			if (InventoryHelper.IsInventoryLock(DateTime.Now, UserHelper.DealerCode, UserHelper.BranchCode) ||
				InventoryHelper.IsInventoryLock(DateTime.Now, UserHelper.DealerCode, 0))
			{
				phInventoryLock.Visible = true;
				btnAdd.Enabled = false;
			}
		}
	}

	protected string EvalOrderStatus(Object objStatus)
	{
		return Order.GetOrderStatusString(int.Parse(objStatus.ToString()));
	}

	protected string EvalAddress(object add1, object add2)
	{
		if (!string.IsNullOrEmpty((string)add2)) return (string)add2;
		return VDMS.Data.TipTop.Dealer.GetAddressByBranchCode((string)add1);
	}

	protected void grdListOrder_DataBound(object sender, EventArgs e)
	{
		if (grdListOrder.TopPagerRow == null) return;
		Literal litPageInfo = grdListOrder.TopPagerRow.FindControl("litPageInfo") as Literal;
		if (litPageInfo != null) litPageInfo.Text = string.Format(Resources.Message.PagingInfo, grdListOrder.PageIndex + 1, grdListOrder.PageCount, HttpContext.Current.Items["rowCount"]);
	}

	protected void btnSearch_Click(object sender, EventArgs e)
	{
		grdListOrder.PageIndex = 0;
		grdListOrder.DataBind();
	}

	protected void ddlAddress_DataBound(object sender, EventArgs e)
	{
		if ((UserHelper.BranchCode != UserHelper.DealerCode) && !UserHelper.IsAdmin)
		{
			ddlAddress.SelectedValue = UserHelper.BranchCode;
			ddlAddress.Enabled = false;
		}
	}
}
