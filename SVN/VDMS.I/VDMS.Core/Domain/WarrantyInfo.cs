using System;

namespace VDMS.Core.Domain
{
	#region Warrantyinfo

	/// <summary>
	/// Warrantyinfo object for NHibernate mapped table 'SER_WARRANTYINFO'.
	/// </summary>
	public class Warrantyinfo : DomainObject<string>, IComparable
	{
		#region Member Variables

		protected DateTime _purchasedate;
		protected int _kmcount;
		protected string _itemcode;
        protected string _createByDealer;
        protected string _status;
		protected string _color;
		protected string _selldealercode;
		protected string _databasecode;
		protected DateTime _createdate;
		protected Customer _customer;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Warrantyinfo() { }

        public Warrantyinfo(DateTime purchasedate, int kmcount, string itemcode, string color, string selldealercode, string databasecode, DateTime createdate, Customer customer, string createByDealer, string status)
		{
			this._purchasedate = purchasedate;
			this._kmcount = kmcount;
			this._itemcode = itemcode;
			this._color = color;
			this._selldealercode = selldealercode;
			this._databasecode = databasecode;
			this._createdate = createdate;
            this._customer = customer;
            this._createByDealer = createByDealer;
            this._status = status;
		}

		#endregion

		#region Public Properties

		public DateTime Purchasedate
		{
			get { return _purchasedate; }
			set { _purchasedate = value; }
		}

		public int Kmcount
		{
			get { return _kmcount; }
			set { _kmcount = value; }
		}

		public string Itemcode
		{
			get { return _itemcode; }
			set
			{
				if (value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Itemcode", value, value.ToString());
				_itemcode = value;
			}
		}

		public string Color
		{
			get { return _color; }
			set
			{
				if (value != null && value.Length > 120)
					throw new ArgumentOutOfRangeException("Invalid value for Color", value, value.ToString());
				_color = value;
			}
		}
        public string CreateByDealer
        {
            get { return _createByDealer; }
            set
            {
                if (value != null && value.Length > 30)
                    throw new ArgumentOutOfRangeException("Invalid value for CreateByDealer", value, value.ToString());
                _createByDealer = value;
            }
        }
        public string Status
        {
            get { return _status; }
            set
            {
                if (value != null && value.Length > 3)
                    throw new ArgumentOutOfRangeException("Invalid value for Status", value, value.ToString());
                _status = value;
            }
        }
		public string Selldealercode
		{
			get { return _selldealercode; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Selldealercode", value, value.ToString());
				_selldealercode = value;
			}
		}

		public string Databasecode
		{
			get { return _databasecode; }
			set
			{
				if (value != null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for Databasecode", value, value.ToString());
				_databasecode = value;
			}
		}

		public DateTime Createdate
		{
			get { return _createdate; }
			set { _createdate = value; }
		}

		public Customer Customer
		{
			get { return _customer; }
			set { _customer = value; }
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
			if (!(obj is Warrantyinfo))
				throw new InvalidCastException("This object is not of type Warrantyinfo");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Warrantyinfo)obj).Id);
					break;
				case "Purchasedate":
					relativeValue = this.Purchasedate.CompareTo(((Warrantyinfo)obj).Purchasedate);
					break;
				case "Kmcount":
					relativeValue = this.Kmcount.CompareTo(((Warrantyinfo)obj).Kmcount);
					break;
				case "Itemcode":
					relativeValue = this.Itemcode.CompareTo(((Warrantyinfo)obj).Itemcode);
					break;
				case "Color":
					relativeValue = this.Color.CompareTo(((Warrantyinfo)obj).Color);
					break;
                case "CreateByDealer":
                    relativeValue = this.CreateByDealer.CompareTo(((Warrantyinfo)obj).CreateByDealer);
					break;
				case "Status":
                    relativeValue = this.Status.CompareTo(((Warrantyinfo)obj).Status);
					break;
				case "Selldealercode":
					relativeValue = (this.Selldealercode != null) ? this.Selldealercode.CompareTo(((Warrantyinfo)obj).Selldealercode) : -1;
					break;
				case "Databasecode":
					relativeValue = this.Databasecode.CompareTo(((Warrantyinfo)obj).Databasecode);
					break;
				case "Createdate":
					relativeValue = (this.Createdate != null) ? this.Createdate.CompareTo(((Warrantyinfo)obj).Createdate) : -1;
					break;
				default:
					goto case "Id";
			}
			if (Warrantyinfo.SortDirection == SortDirection.Ascending)
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
