using System;
using System.Linq;
using System.Web.UI;
using VDMS.II.Linq;

public partial class Part_Inventory_OrderInfo : System.Web.UI.UserControl
{
	public int OrderId { get; set; }
	public string RedirectURLWhenOrderNull { get; set; }
	public string OrderQueryString { get; set; }

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			int id;
			if (!string.IsNullOrEmpty(OrderQueryString))
			{
				int.TryParse(Request.QueryString[OrderQueryString], out id);
				if (id == 0) Response.Redirect(RedirectURLWhenOrderNull);
				OrderId = id;
			}
			var db = DCFactory.GetDataContext<PartDataContext>();
			var oh = db.OrderHeaders.SingleOrDefault(p => p.OrderHeaderId == OrderId);
			if (oh == null) Response.Redirect(RedirectURLWhenOrderNull);
			lblDealer.Text = oh.Dealer.DealerName;
			lblWarehouse.Text = oh.Warehouse.Address;
			lblOrderDate.Text = oh.OrderDate.ToShortDateString();
		}
	}
}
