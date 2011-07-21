using System;

namespace VDMS.Core.Domain
{
	#region Payment

	/// <summary>
	/// Payment object for NHibernate mapped table 'SALE_PAYMENT'.
	/// </summary>
	public class Payment : DomainObject<decimal>, IComparable
	{
		#region Member Variables

		protected DateTime _paymentdate;
		protected decimal _amount;
		protected int _status;
		protected DateTime _transferdate;
		protected string _commentpayment;
		protected string _bankaccount;
		protected Sellitem _sellitem;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Payment() { }

		public Payment(DateTime paymentdate, decimal amount, int status, DateTime transferdate, string commentpayment, string bankaccount, Sellitem sellitem)
		{
			this._paymentdate = paymentdate;
			this._amount = amount;
			this._status = status;
			this._transferdate = transferdate;
			this._commentpayment = commentpayment;
			this._bankaccount = bankaccount;
			this._sellitem = sellitem;
		}

		#endregion

		#region Public Properties

		public DateTime Paymentdate
		{
			get { return _paymentdate; }
			set { _paymentdate = value; }
		}

		public decimal Amount
		{
			get { return _amount; }
			set { _amount = value; }
		}

		public int Status
		{
			get { return _status; }
			set { _status = value; }
		}

		public DateTime Transferdate
		{
			get { return _transferdate; }
			set { _transferdate = value; }
		}

		public string Commentpayment
		{
			get { return _commentpayment; }
			set
			{
				if (value != null && value.Length > 1024)
					throw new ArgumentOutOfRangeException("Invalid value for Commentpayment", value, value.ToString());
				_commentpayment = value;
			}
		}

		public string Bankaccount
		{
			get { return _bankaccount; }
			set
			{
				if (value != null && value.Length > 13)
					throw new ArgumentOutOfRangeException("Invalid value for Bankaccount", value, value.ToString());
				_bankaccount = value;
			}
		}

		public Sellitem Sellitem
		{
			get { return _sellitem; }
			set { _sellitem = value; }
		}

		public static String SortExpression
		{
			get { return _sortExpression; }
			set { _sortExpression = value; }
		}

		public static SortDirection SortDirection
		{
			get { return _sortDirection; }
			set { _sortDirection = value; }
		}
		#endregion

		#region IComparable Methods
		public int CompareTo(object obj)
		{
			if (!(obj is Payment))
				throw new InvalidCastException("This object is not of type Payment");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Payment)obj).Id);
					break;
				case "Paymentdate":
					relativeValue = this.Paymentdate.CompareTo(((Payment)obj).Paymentdate);
					break;
				case "Amount":
					relativeValue = this.Amount.CompareTo(((Payment)obj).Amount);
					break;
				case "Status":
					relativeValue = this.Status.CompareTo(((Payment)obj).Status);
					break;
				case "Transferdate":
					relativeValue = (this.Transferdate != null) ? this.Transferdate.CompareTo(((Payment)obj).Transferdate) : -1;
					break;
				case "Commentpayment":
					relativeValue = (this.Commentpayment != null) ? this.Commentpayment.CompareTo(((Payment)obj).Commentpayment) : -1;
					break;
				case "Bankaccount":
					relativeValue = (this.Bankaccount != null) ? this.Bankaccount.CompareTo(((Payment)obj).Bankaccount) : -1;
					break;
				default:
					goto case "Id";
			}
			if (Payment.SortDirection == SortDirection.Ascending)
				relativeValue *= -1;
			return relativeValue;
		}
		#endregion

		public override int GetHashCode()
		{
			return id.GetHashCode();
		}
	}

	#endregion
}