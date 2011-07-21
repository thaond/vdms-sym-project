using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using VDMS.Common.Web.GridViewHepler;
using VDMS.Data.DAL2;
using VDMS.Data.TipTop;
using VDMS.Helper;
using VDMS.I.Vehicle;
using VDMS.UI;

public partial class Sales_Inventory_Business : BasePage
{
	//int PageIndex = 0;
	//int RowCount = 0;
	//int PageSize = 10;

	//int PageCount
	//{
	//    get
	//    {
	//        if (RowCount % PageSize == 0) return RowCount / PageSize;
	//        return RowCount / PageSize + 1;
	//    }
	//}

	//protected void Page_Init(object sender, EventArgs e)
	//{
	//    this.Page.RegisterRequiresControlState(this);
	//}

	//protected override void LoadControlState(object savedState)
	//{
	//    object[] ctlState = (object[])savedState;
	//    base.LoadControlState(ctlState[0]);
	//    this.PageIndex = (int)ctlState[1];
	//    this.RowCount = (int)ctlState[2];
	//}

	//protected override object SaveControlState()
	//{
	//    object[] ctlState = new object[3];
	//    ctlState[0] = base.SaveControlState();
	//    ctlState[1] = this.PageIndex;
	//    ctlState[2] = this.RowCount;
	//    return ctlState;
	//}

	//const string GroupName = "ORDERID+SHIPPINGTO+ORDERDATE+ORDERTIMES+SHIPPINGTO+DEALERCOMMENT+ORDERNUMBER+STATUS+SECONDARYSHIPPINGTO";

	protected void Page_LoadComplete(object sender, EventArgs e)
	{
		if (Page.PreviousPage != null)
		{
			OrderQueryInformation orderQI = (Page.PreviousPage as IOrderRequestPage).OrderQueryInfo;
			txtFromDate.Text = orderQI.OQ_DateFrom;
			txtDealer.Text = orderQI.OQ_Dealer;
			txtOrderNumber.Text = orderQI.OQ_OrderNumber;
			txtToDate.Text = orderQI.OQ_DateTo;

			//DropDownList dArea = (DropDownList)WebTools.FindControlById("ddlArea", Page);
			//DropDownList dStatus = (DropDownList)WebTools.FindControlById("ddlStatus", Page);
			ddlArea.SelectedValue = orderQI.OQ_Area;
			ddlStatus.SelectedValue = orderQI.OQ_Status;

			btnSubmit_Click(sender, e);
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			txtFromDate.Text = DateTimeHelper.FirstDayInMonth.ToShortDateString();
			txtToDate.Text = DateTime.Now.ToShortDateString();
		}
		//GridViewHelper helper = new GridViewHelper(this.gvMain);
		//helper.RegisterGroup(GroupName.Split('+'), true, true);
		//helper.RegisterSummary("ORDERQTY", SummaryOperation.Sum, GroupName);
		//helper.RegisterSummary("ITEMTOTALPRICE", SummaryOperation.Sum, GroupName);
		//helper.GroupHeader += new GroupEvent(helper_GroupHeader);
		//helper.GroupSummary += new GroupEvent(helper_GroupSummary);
	}

	//void helper_GroupSummary(string groupName, object[] values, GridViewRow row)
	//{
	//    row.CssClass = "Summary";
	//    row.Cells[0].Text = Message.Total;
	//    row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
	//}

	//void helper_GroupHeader(string groupName, object[] values, GridViewRow row)
	//{
	//    if (groupName == GroupName)
	//    {
	//        try
	//        {
	//            OrderInfor obj = null;
	//            obj = LoadControl("OrderInfor.ascx") as OrderInfor;
	//            obj._OrderId = (decimal)values[0];
	//            obj._DealerCode = (string)values[1];
	//            obj._OrderDate = (DateTime)values[2];
	//            obj._OrderTimes = (decimal)values[3];
	//            obj._ShippingTo = Dealer.GetAddressByBranchCode((string)values[4]);
	//            obj._Comment = values[5] as string;
	//            obj._OrderNumber = values[6] as string;
	//            obj._SecondaryAddress = values[8] as string;
	//            obj._DealerName = DealerHelper.GetName((string)values[1]);
	//            obj._print = OrderForm.PrintOrder;

	//            obj.OQ_DateFrom = txtFromDate.Text;
	//            obj.OQ_DateTo = txtToDate.Text;
	//            obj.OQ_Dealer = txtDealer.Text;
	//            obj.OQ_OrderNumber = txtOrderNumber.Text;
	//            obj.OQ_AreaIndex = ddlArea.SelectedIndex;
	//            obj.OQ_StatusIndex = ddlStatus.SelectedIndex;

	//            switch ((int)(decimal)values[7])
	//            {
	//                case 1:
	//                    obj._Status = Resources.OrderStatus.Sent;
	//                    break;
	//                case 2:
	//                    obj._Status = Resources.OrderStatus.Confirmed;
	//                    break;
	//                case 4:
	//                    obj._Status = Resources.OrderStatus.Approved;
	//                    break;
	//            }
	//            StringBuilder result = new StringBuilder(1024);
	//            obj.RenderControl(new HtmlTextWriter(new StringWriter(result)));
	//            row.Cells[0].Text = result.ToString();
	//            row.CssClass = "group1";
	//        }
	//        catch { }
	//    }
	//}

	protected string GetStatus(int status)
	{
		switch (status)
		{
			case 1:
				return Resources.OrderStatus.Sent;
			case 2:
				return Resources.OrderStatus.Confirmed;
			case 4:
				return Resources.OrderStatus.Approved;
		}
		return string.Empty;
	}

	//void BindData(bool Reset)
	//{
	//    if (Reset) PageIndex = 0;
	//    DateTime fromDate, toDate;
	//    DateTime.TryParse(txtFromDate.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out fromDate);
	//    DateTime.TryParse(txtToDate.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out toDate);

	//    gvMain.DataSource = OrderDao.GetOrderByStatus(fromDate, toDate, txtDealer.Text, ddlArea.SelectedValue, txtOrderNumber.Text, UserHelper.DatabaseCode, ddlStatus.SelectedValue, PageIndex, PageSize);
	//    RowCount = (int)OrderDao.GetOrderCountByStatus(fromDate, toDate, txtDealer.Text, ddlArea.SelectedValue, txtOrderNumber.Text, UserHelper.DatabaseCode, ddlStatus.SelectedValue);
	//    gvMain.DataBind();
	//}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		//BindData(true);
		lv.DataBind();
	}

	//protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
	//{
	//    PageIndex = e.NewPageIndex;
	//    BindData(false);
	//}

	//protected void cmdPage_Click(object sender, EventArgs e)
	//{
	//    Button button = (Button)sender;
	//    switch (button.CommandArgument)
	//    {
	//        case "First":
	//            PageIndex = 0;
	//            BindData(false);
	//            break;
	//        case "Prev":
	//            if (PageIndex > 0) PageIndex--;
	//            BindData(false);
	//            break;
	//        case "Next":
	//            if (PageIndex < PageCount - 1) PageIndex++;
	//            BindData(false);
	//            break;
	//        case "Last":
	//            PageIndex = PageCount - 1;
	//            BindData(false);
	//            break;
	//    }
	//}

	protected void ddlArea_DataBound(object sender, EventArgs e)
	{
		foreach (ListItem item in ddlArea.Items)
			item.Text = string.Format("{0}({1})", item.Value, item.Text);
	}
}
