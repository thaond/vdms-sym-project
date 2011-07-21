using System;

namespace VDMS.Core.Domain
{
	#region Transhis

	/// <summary>
	/// Transhi object for NHibernate mapped table 'SALE_TRANSHIS'.
	/// </summary>
	public class TransHis : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected long _referenceorderid;
		protected int _transactiontype;
		protected DateTime _transactiondate;
		protected decimal _actualcost;
		protected DateTime _modifieddate;
		protected string _frombranch;
		protected string _tobranch;
		protected string _modifiedby;
		protected string _oldengineno;
		protected Iteminstance _iteminstance;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public TransHis() { }

		public TransHis(long referenceorderid, int transactiontype, DateTime transactiondate, long actualcost, DateTime modifieddate, string frombranch, string tobranch, string modifiedby, string oldengineno, Iteminstance iteminstance)
		{
			this._referenceorderid = referenceorderid;
			this._transactiontype = transactiontype;
			this._transactiondate = transactiondate;
			this._actualcost = actualcost;
			this._modifieddate = modifieddate;
			this._frombranch = frombranch;
			this._tobranch = tobranch;
			this._modifiedby = modifiedby;
			this._oldengineno = oldengineno;
			this._iteminstance = iteminstance;
		}

		#endregion

		#region Public Properties

		public long Referenceorderid
		{
			get { return _referenceorderid; }
			set { _referenceorderid = value; }
		}

		public int Transactiontype
		{
			get { return _transactiontype; }
			set { _transactiontype = value; }
		}

		public DateTime Transactiondate
		{
			get { return _transactiondate; }
			set { _transactiondate = value; }
		}

		public decimal Actualcost
		{
			get { return _actualcost; }
			set { _actualcost = value; }
		}

		public DateTime Modifieddate
		{
			get { return _modifieddate; }
			set { _modifieddate = value; }
		}

		public string Frombranch
		{
			get { return _frombranch; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Frombranch", value, value.ToString());
				_frombranch = value;
			}
		}

		public string Tobranch
		{
			get { return _tobranch; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Tobranch", value, value.ToString());
				_tobranch = value;
			}
		}

		public string Modifiedby
		{
			get { return _modifiedby; }
			set
			{
				if (value != null && value.Length > 512)
					throw new ArgumentOutOfRangeException("Invalid value for Modifiedby", value, value.ToString());
				_modifiedby = value;
			}
		}

		public string Oldengineno
		{
			get { return _oldengineno; }
			set
			{
				if (value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for Oldengineno", value, value.ToString());
				_oldengineno = value;
			}
		}

		public Iteminstance Iteminstance
		{
			get { return _iteminstance; }
			set { _iteminstance = value; }
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
			if (!(obj is TransHis))
				throw new InvalidCastException("This object is not of type Transhi");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((TransHis)obj).Id);
					break;
				case "Referenceorderid":
					relativeValue = this.Referenceorderid.CompareTo(((TransHis)obj).Referenceorderid);
					break;
				case "Transactiontype":
					relativeValue = this.Transactiontype.CompareTo(((TransHis)obj).Transactiontype);
					break;
				case "Transactiondate":
					relativeValue = this.Transactiondate.CompareTo(((TransHis)obj).Transactiondate);
					break;
				case "Actualcost":
					relativeValue = this.Actualcost.CompareTo(((TransHis)obj).Actualcost);
					break;
				case "Modifieddate":
					relativeValue = this.Modifieddate.CompareTo(((TransHis)obj).Modifieddate);
					break;
				case "Frombranch":
					relativeValue = (this.Frombranch != null) ? this.Frombranch.CompareTo(((TransHis)obj).Frombranch) : -1;
					break;
				case "Tobranch":
					relativeValue = (this.Tobranch != null) ? this.Tobranch.CompareTo(((TransHis)obj).Tobranch) : -1;
					break;
				case "Modifiedby":
					relativeValue = this.Modifiedby.CompareTo(((TransHis)obj).Modifiedby);
					break;
				case "Oldengineno":
					relativeValue = (this.Oldengineno != null) ? this.Oldengineno.CompareTo(((TransHis)obj).Oldengineno) : -1;
					break;
				default:
					goto case "Id";
			}
			if (TransHis.SortDirection == SortDirection.Ascending)
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
