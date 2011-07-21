using System;

namespace VDMS.Core.Domain
{
	#region Orderdetail

	/// <summary>
	/// Orderdetail object for NHibernate mapped table 'SALE_ORDERDETAIL'.
	/// </summary>
	[Serializable]
	public class Orderdetail : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected DateTime _createddate;
		protected string _createdby;
		protected DateTime _lastediteddate;
		protected string _lasteditedby;
		protected int _orderqty;
		protected long _unitprice;
		protected long _unitpricediscount;
		protected int _orderpriority;
		protected Item _item;
		protected Orderheader _orderheader;
		protected Specialoffer _specialoffer;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Orderdetail() { }

		public Orderdetail(DateTime createddate, string createdby, DateTime lastediteddate, string lasteditedby, int orderqty, long unitprice, long unitpricediscount, int orderpriority, Item item, Orderheader orderheader, Specialoffer specialoffer)
		{
			this._createddate = createddate;
			this._createdby = createdby;
			this._lastediteddate = lastediteddate;
			this._lasteditedby = lasteditedby;
			this._orderqty = orderqty;
			this._unitprice = unitprice;
			this._unitpricediscount = unitpricediscount;
			this._orderpriority = orderpriority;
			this._item = item;
			this._orderheader = orderheader;
			this._specialoffer = specialoffer;
		}

		#endregion

		#region Public Properties

		public DateTime Createddate
		{
			get { return _createddate; }
			set { _createddate = value; }
		}

		public string Createdby
		{
			get { return _createdby; }
			set
			{
				if (value != null && value.Length > 512)
					throw new ArgumentOutOfRangeException("Invalid value for Createdby", value, value.ToString());
				_createdby = value;
			}
		}

		public DateTime Lastediteddate
		{
			get { return _lastediteddate; }
			set { _lastediteddate = value; }
		}

		public string Lasteditedby
		{
			get { return _lasteditedby; }
			set
			{
				if (value != null && value.Length > 512)
					throw new ArgumentOutOfRangeException("Invalid value for Lasteditedby", value, value.ToString());
				_lasteditedby = value;
			}
		}

		public int Orderqty
		{
			get { return _orderqty; }
			set { _orderqty = value; }
		}

		public long Unitprice
		{
			get { return _unitprice; }
			set { _unitprice = value; }
		}

		public long Unitpricediscount
		{
			get { return _unitpricediscount; }
			set { _unitpricediscount = value; }
		}

		public int Orderpriority
		{
			get { return _orderpriority; }
			set { _orderpriority = value; }
		}

		public Item Item
		{
			get { return _item; }
			set { _item = value; }
		}

		public Orderheader Orderheader
		{
			get { return _orderheader; }
			set { _orderheader = value; }
		}

		public Specialoffer Specialoffer
		{
			get { return _specialoffer; }
			set { _specialoffer = value; }
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
			if (!(obj is Orderdetail))
				throw new InvalidCastException("This object is not of type Orderdetail");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Orderdetail)obj).Id);
					break;
				case "Createddate":
					relativeValue = this.Createddate.CompareTo(((Orderdetail)obj).Createddate);
					break;
				case "Createdby":
					relativeValue = this.Createdby.CompareTo(((Orderdetail)obj).Createdby);
					break;
				case "Lasteditedby":
					relativeValue = this.Lasteditedby.CompareTo(((Orderdetail)obj).Lasteditedby);
					break;
				case "Orderqty":
					relativeValue = this.Orderqty.CompareTo(((Orderdetail)obj).Orderqty);
					break;
				case "Unitprice":
					relativeValue = this.Unitprice.CompareTo(((Orderdetail)obj).Unitprice);
					break;
				case "Unitpricediscount":
					relativeValue = this.Unitpricediscount.CompareTo(((Orderdetail)obj).Unitpricediscount);
					break;
				case "Orderpriority":
					relativeValue = this.Orderpriority.CompareTo(((Orderdetail)obj).Orderpriority);
					break;
				default:
					goto case "Id";
			}
			if (Orderdetail.SortDirection == SortDirection.Ascending)
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
