using VDMS.Core.Domain;

namespace VDMS.I.Report.DataObject
{
	public class RptOrderItem
	{
		public string ItemType { get; set; }
		public string ItemName { get; set; }
		public string Color { get; set; }
		public string No { get; set; }
		public string ShipTo { get; set; }
		public string Quantity { get; set; }
		public int iQuantity { get; set; }
		public int UnitPrice { get; set; }
		public int TotalAmount { get; set; }

		public RptOrderItem(Orderdetail item, int no, int div)
		{
			this.ItemName = item.Item.Itemname;
			this.ItemType = item.Item.Itemtype;
			this.Color = item.Item.Colorname;
			this.iQuantity = (int)item.Orderqty;
			this.Quantity = this.iQuantity.ToString();
			this.UnitPrice = (int)(item.Unitprice / div);
			this.TotalAmount = (int)(this.iQuantity * this.UnitPrice);
			this.No = no.ToString().PadLeft(2, '0');
		}

		public RptOrderItem(Orderdetail item, int no)
			: this(item, no, 1000)
		{
		}
	}
}