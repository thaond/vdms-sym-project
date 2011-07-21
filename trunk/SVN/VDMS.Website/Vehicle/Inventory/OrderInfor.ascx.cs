using System;
using System.Web.UI.WebControls;

public partial class Vehicle_Inventory_OrderInfor : VDMS.UI.OrderInfor
{
	protected void CreateProcessOrderLink(LinkButton lnkb)
	{
		lnkb.PostBackUrl = string.Format("ProcessOrder.aspx?OrderId={0}", _OrderId);
		lnkb.Text = string.Format("<span class=\"processOrderLink\">{0}({1})</span> | <span class=\"processOrderLink\">{2}</span> | <span class=\"processOrderLink\">{3}</span> ({4})",
								  _DealerName, _DealerCode, ((DateTime)_OrderDate).ToShortDateString(), _OrderTimes, _Status);
	}

	public void Page_Load(object sender, EventArgs e)
	{

	}
}
