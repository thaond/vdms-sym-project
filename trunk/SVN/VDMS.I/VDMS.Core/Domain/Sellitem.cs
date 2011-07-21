using System;

namespace VDMS.Core.Domain
{
	#region Sellitem

	/// <summary>
	/// Sellitem object for NHibernate mapped table 'SALE_SELLITEM'.
	/// </summary>
	public class Sellitem : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected decimal _pricebeforetax;
		protected int _taxamt;
		protected int _paymenttype;
		protected string _numberplate;
		protected string _selltype;
		protected DateTime _paymentdate;
		protected DateTime _numplaterecdate;
		protected string _commentsellitem;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Sellitem() { }

		public Sellitem(decimal pricebeforetax, int taxamt, int paymenttype, string numberplate, string selltype, DateTime paymentdate, DateTime numplaterecdate, string commentsellitem)
		{
			this._pricebeforetax = pricebeforetax;
			this._taxamt = taxamt;
			this._paymenttype = paymenttype;
			this._numberplate = numberplate;
			this._selltype = selltype;
			this._paymentdate = paymentdate;
			this._numplaterecdate = numplaterecdate;
			this._commentsellitem = commentsellitem;
		}

		#endregion

		#region Public Properties

		public decimal Pricebeforetax
		{
			get { return _pricebeforetax; }
			set { _pricebeforetax = value; }
		}

		public int Taxamt
		{
			get { return _taxamt; }
			set { _taxamt = value; }
		}

		public int Paymenttype
		{
			get { return _paymenttype; }
			set { _paymenttype = value; }
		}

		public string Numberplate
		{
			get { return _numberplate; }
			set
			{
				if (value != null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for Numberplate", value, value.ToString());
				_numberplate = value;
			}
		}

		public string Selltype
		{
			get { return _selltype; }
			set
			{
				if (value != null && value.Length > 512)
					throw new ArgumentOutOfRangeException("Invalid value for Selltype", value, value.ToString());
				_selltype = value;
			}
		}

		public DateTime Paymentdate
		{
			get { return _paymentdate; }
			set { _paymentdate = value; }
		}

		public DateTime Numplaterecdate
		{
			get { return _numplaterecdate; }
			set { _numplaterecdate = value; }
		}

		public string Commentsellitem
		{
			get { return _commentsellitem; }
			set
			{
				if (value != null && value.Length > 512)
					throw new ArgumentOutOfRangeException("Invalid value for Commentsellitem", value, value.ToString());
				_commentsellitem = value;
			}
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
			if (!(obj is Sellitem))
				throw new InvalidCastException("This object is not of type Sellitem");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Sellitem)obj).Id);
					break;
				case "Pricebeforetax":
					relativeValue = this.Pricebeforetax.CompareTo(((Sellitem)obj).Pricebeforetax);
					break;
				case "Taxamt":
					relativeValue = this.Taxamt.CompareTo(((Sellitem)obj).Taxamt);
					break;
				case "Paymenttype":
					relativeValue = this.Paymenttype.CompareTo(((Sellitem)obj).Paymenttype);
					break;
				case "Numberplate":
					relativeValue = (this.Numberplate != null) ? this.Numberplate.CompareTo(((Sellitem)obj).Numberplate) : -1;
					break;
				case "Selltype":
					relativeValue = (this.Selltype != null) ? this.Selltype.CompareTo(((Sellitem)obj).Selltype) : -1;
					break;
				case "Paymentdate":
					relativeValue = (this.Paymentdate != null) ? this.Paymentdate.CompareTo(((Sellitem)obj).Paymentdate) : -1;
					break;
				case "Numplaterecdate":
					relativeValue = (this.Numplaterecdate != null) ? this.Numplaterecdate.CompareTo(((Sellitem)obj).Numplaterecdate) : -1;
					break;
				case "Commentsellitem":
					relativeValue = (this.Commentsellitem != null) ? this.Commentsellitem.CompareTo(((Sellitem)obj).Commentsellitem) : -1;
					break;
				default:
					goto case "Id";
			}
			if (Sellitem.SortDirection == SortDirection.Ascending)
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
