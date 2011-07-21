using System;
using System.Web.UI;
using System.Web;

namespace VDMS.UI
{
	public class OrderInfor : UserControl
	{
		#region _orderQuery

		private string _oqDateFrom, _oqDateTo, _oqDealer, _oqOrderNumber;
		private int _oqAreaIndex, _oqStatusIndex;

		public string OQ_DateFrom
		{
			get { return Server.UrlEncode(_oqDateFrom); }
			set { _oqDateFrom = value; }
		}
		public string OQ_DateTo
		{
			get { return Server.UrlEncode(_oqDateTo); }
			set { _oqDateTo = value; }
		}
		public string OQ_Dealer
		{
			get { return Server.UrlEncode(_oqDealer); }
			set { _oqDealer = value; }
		}
		public string OQ_OrderNumber
		{
			get { return Server.UrlEncode(_oqOrderNumber); }
			set { _oqOrderNumber = value; }
		}
		public int OQ_AreaIndex
		{
			get { return _oqAreaIndex; }
			set { _oqAreaIndex = value; }
		}
		public int OQ_StatusIndex
		{
			get { return _oqStatusIndex; }
			set { _oqStatusIndex = value; }
		}

		#endregion

		public decimal _OrderId = 0;
		public string _DealerCode = string.Empty;
		public string _DealerName = string.Empty;
		public DateTime _OrderDate = DateTime.Now;
		public decimal _OrderTimes = 1;
		public string _ShippingTo = string.Empty;
		public string _Comment = string.Empty;
		public string _OrderNumber = string.Empty;
		public string _Status = string.Empty;
		public string _SecondaryAddress = string.Empty;
		public string _print = string.Empty;


	}
}