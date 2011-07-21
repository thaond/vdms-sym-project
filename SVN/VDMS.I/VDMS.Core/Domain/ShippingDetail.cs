using System;

namespace VDMS.Core.Domain
{
	#region Shippingdetail

	/// <summary>
	/// Shippingdetail object for NHibernate mapped table 'SALE_SHIPPINGDETAIL'.
	/// </summary>
	public class Shippingdetail : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected int _status;
		protected string _ordernumber;
		protected bool _voucherstatus;
		protected string _exception;
		protected string _enginenumber;
		protected string _itemtype;
		protected string _color;
		protected string _branchcode;
		protected string _vmepresponse;
		protected DateTime _vmepresponsedate;
		protected Item _item;
		protected ShippingHeader _shippingheader;
		protected Iteminstance _productinstance;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Shippingdetail() { }

		public Shippingdetail(int status, string ordernumber, bool voucherstatus, string exception, string enginenumber, string itemtype, string color, string branchcode, string vmepresponse, DateTime vmepresponsedate, Item item, ShippingHeader shippingheader, Iteminstance productinstance)
		{
			this._status = status;
			this._ordernumber = ordernumber;
			this._voucherstatus = voucherstatus;
			this._exception = exception;
			this._enginenumber = enginenumber;
			this._itemtype = itemtype;
			this._color = color;
			this._branchcode = branchcode;
			this._vmepresponse = vmepresponse;
			this._vmepresponsedate = vmepresponsedate;
			this._item = item;
			this._shippingheader = shippingheader;
			this._productinstance = productinstance;
		}

		#endregion

		#region Public Properties

		public int Status
		{
			get { return _status; }
			set { _status = value; }
		}

		public string Ordernumber
		{
			get { return _ordernumber; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Ordernumber", value, value.ToString());
				_ordernumber = value;
			}
		}

		public bool Voucherstatus
		{
			get { return _voucherstatus; }
			set { _voucherstatus = value; }
		}

		public string Exception
		{
			get { return _exception; }
			set
			{
				if (value != null && value.Length > 4000)
					throw new ArgumentOutOfRangeException("Invalid value for Exception", value, value.ToString());
				_exception = value;
			}
		}

		public string Enginenumber
		{
			get { return _enginenumber; }
			set
			{
				if (value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for Enginenumber", value, value.ToString());
				_enginenumber = value;
			}
		}

		public string Itemtype
		{
			get { return _itemtype; }
			set
			{
				if (value != null && value.Length > 512)
					throw new ArgumentOutOfRangeException("Invalid value for Itemtype", value, value.ToString());
				_itemtype = value;
			}
		}

		public string Color
		{
			get { return _color; }
			set
			{
				if (value != null && value.Length > 40)
					throw new ArgumentOutOfRangeException("Invalid value for Color", value, value.ToString());
				_color = value;
			}
		}

		public string Branchcode
		{
			get { return _branchcode; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Branchcode", value, value.ToString());
				_branchcode = value;
			}
		}

		public string Vmepresponse
		{
			get { return _vmepresponse; }
			set
			{
				if (value != null && value.Length > 4000)
					throw new ArgumentOutOfRangeException("Invalid value for Vmepresponse", value, value.ToString());
				_vmepresponse = value;
			}
		}

		public DateTime Vmepresponsedate
		{
			get { return _vmepresponsedate; }
			set { _vmepresponsedate = value; }
		}

		public Item Item
		{
			get { return _item; }
			set { _item = value; }
		}

		public ShippingHeader Shippingheader
		{
			get { return _shippingheader; }
			set { _shippingheader = value; }
		}

		public Iteminstance PRODUCTINSTANCE
		{
			get { return _productinstance; }
			set { _productinstance = value; }
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
			if (!(obj is Shippingdetail))
				throw new InvalidCastException("This object is not of type Shippingdetail");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Shippingdetail)obj).Id);
					break;
				case "Status":
					relativeValue = this.Status.CompareTo(((Shippingdetail)obj).Status);
					break;
				case "Ordernumber":
					relativeValue = this.Ordernumber.CompareTo(((Shippingdetail)obj).Ordernumber);
					break;
				case "Voucherstatus":
					relativeValue = this.Voucherstatus.CompareTo(((Shippingdetail)obj).Voucherstatus);
					break;
				case "Exception":
					relativeValue = (this.Exception != null) ? this.Exception.CompareTo(((Shippingdetail)obj).Exception) : -1;
					break;
				case "Enginenumber":
					relativeValue = this.Enginenumber.CompareTo(((Shippingdetail)obj).Enginenumber);
					break;
				case "Itemtype":
					relativeValue = this.Itemtype.CompareTo(((Shippingdetail)obj).Itemtype);
					break;
				case "Color":
					relativeValue = this.Color.CompareTo(((Shippingdetail)obj).Color);
					break;
				case "Branchcode":
					relativeValue = this.Branchcode.CompareTo(((Shippingdetail)obj).Branchcode);
					break;
				case "Vmepresponse":
					relativeValue = (this.Vmepresponse != null) ? this.Vmepresponse.CompareTo(((Shippingdetail)obj).Vmepresponse) : -1;
					break;
				case "Vmepresponsedate":
					relativeValue = (this.Vmepresponsedate != null) ? this.Vmepresponsedate.CompareTo(((Shippingdetail)obj).Vmepresponsedate) : -1;
					break;
				default:
					goto case "Id";
			}
			if (Shippingdetail.SortDirection == SortDirection.Ascending)
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
