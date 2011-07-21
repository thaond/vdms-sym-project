using System;

namespace VDMS.Core.Domain
{
	#region Shippingheader

	/// <summary>
	/// Shippingheader object for NHibernate mapped table 'SALE_SHIPPINGHEADER'.
	/// </summary>
	public class ShippingHeader : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected DateTime _createddate;
		protected string _createdby;
		protected string _dealercode;
		protected string _shippingnumber;
		protected DateTime _shippingdate;
		protected string _shippingto;
		protected int _itemcount;
		protected string _areacode;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public ShippingHeader() { }

		public ShippingHeader(DateTime createddate, string createdby, string dealercode, string shippingnumber, DateTime shippingdate, string shippingto, int itemcount, string areacode)
		{
			this._createddate = createddate;
			this._createdby = createdby;
			this._dealercode = dealercode;
			this._shippingnumber = shippingnumber;
			this._shippingdate = shippingdate;
			this._shippingto = shippingto;
			this._itemcount = itemcount;
			this._areacode = areacode;
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

		public string Dealercode
		{
			get { return _dealercode; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Dealercode", value, value.ToString());
				_dealercode = value;
			}
		}

		public string Shippingnumber
		{
			get { return _shippingnumber; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Shippingnumber", value, value.ToString());
				_shippingnumber = value;
			}
		}

		public DateTime Shippingdate
		{
			get { return _shippingdate; }
			set { _shippingdate = value; }
		}

		public string Shippingto
		{
			get { return _shippingto; }
			set
			{
				if (value != null && value.Length > 512)
					throw new ArgumentOutOfRangeException("Invalid value for Shippingto", value, value.ToString());
				_shippingto = value;
			}
		}

		public int Itemcount
		{
			get { return _itemcount; }
			set { _itemcount = value; }
		}

		public string Areacode
		{
			get { return _areacode; }
			set
			{
				if (value != null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for Areacode", value, value.ToString());
				_areacode = value;
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
			if (!(obj is ShippingHeader))
				throw new InvalidCastException("This object is not of type Shippingheader");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((ShippingHeader)obj).Id);
					break;
				case "Createddate":
					relativeValue = this.Createddate.CompareTo(((ShippingHeader)obj).Createddate);
					break;
				case "Createdby":
					relativeValue = this.Createdby.CompareTo(((ShippingHeader)obj).Createdby);
					break;
				case "Dealercode":
					relativeValue = this.Dealercode.CompareTo(((ShippingHeader)obj).Dealercode);
					break;
				case "Shippingnumber":
					relativeValue = this.Shippingnumber.CompareTo(((ShippingHeader)obj).Shippingnumber);
					break;
				case "Shippingdate":
					relativeValue = this.Shippingdate.CompareTo(((ShippingHeader)obj).Shippingdate);
					break;
				case "Shippingto":
					relativeValue = this.Shippingto.CompareTo(((ShippingHeader)obj).Shippingto);
					break;
				case "Itemcount":
					relativeValue = this.Itemcount.CompareTo(((ShippingHeader)obj).Itemcount);
					break;
				case "Areacode":
					relativeValue = this.Areacode.CompareTo(((ShippingHeader)obj).Areacode);
					break;
				default:
					goto case "Id";
			}
			if (ShippingHeader.SortDirection == SortDirection.Ascending)
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
