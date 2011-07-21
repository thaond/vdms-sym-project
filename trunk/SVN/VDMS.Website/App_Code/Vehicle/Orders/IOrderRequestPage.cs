using System;

namespace VDMS.I.Vehicle
{
	[Serializable]
	public class OrderQueryInformation
	{
		public string OQ_DateFrom;
		public string OQ_DateTo;
		public string OQ_Dealer;
		public string OQ_OrderNumber;
		public string OQ_Area;
		public string OQ_Status;
	}

	public interface IOrderRequestPage
	{
		OrderQueryInformation OrderQueryInfo
		{
			get;
			set;
		}
	}
}