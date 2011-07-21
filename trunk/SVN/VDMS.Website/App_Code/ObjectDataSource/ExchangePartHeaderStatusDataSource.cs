using System.Collections.Generic;
using VDMS.I.Service;

namespace VDMS.I.ObjectDataSource
{
	/// <summary>
	/// Object
	/// </summary>
	public class ExchangePartHeaderStatusObject
	{
		public ExchangePartHeaderStatusObject()
		{
		}
		public ExchangePartHeaderStatusObject(int iValue)
		{
			_value = iValue;
		}

		private int _value;
		public int Value
		{
			set
			{
				_value = value;
			}
			get
			{
				return _value;
			}
		}
		public string ValueString
		{
			get
			{
				string res = ServiceTools.GetNativeExchangeStatusName(_value);
				return res;
			}
		}
	}

	/// <summary>
	/// Source
	/// </summary>
	public class ExchangePartHeaderStatusDataSource
	{
		public ExchangePartHeaderStatusDataSource()
		{
		}
		public List<ExchangePartHeaderStatusObject> Select()
		{
			List<ExchangePartHeaderStatusObject> list = new List<ExchangePartHeaderStatusObject>();
			list.Add(new ExchangePartHeaderStatusObject((int)ExchangeVoucherStatus.New));
			list.Add(new ExchangePartHeaderStatusObject((int)ExchangeVoucherStatus.Sent));
			list.Add(new ExchangePartHeaderStatusObject((int)ExchangeVoucherStatus.Canceled));
			return list;
		}

		public List<ExchangePartHeaderStatusObject> SelectForServiceRpt()
		{
			List<ExchangePartHeaderStatusObject> list = new List<ExchangePartHeaderStatusObject>();
			list.Add(new ExchangePartHeaderStatusObject((int)ExchangeVoucherStatus.All));
			list.Add(new ExchangePartHeaderStatusObject((int)ExchangeVoucherStatus.New));
			list.Add(new ExchangePartHeaderStatusObject((int)ExchangeVoucherStatus.Sent));
			list.Add(new ExchangePartHeaderStatusObject((int)ExchangeVoucherStatus.Reject));
			list.Add(new ExchangePartHeaderStatusObject((int)ExchangeVoucherStatus.Approved));
			return list;
		}

	}
}