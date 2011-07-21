using System;

namespace VDMS.Core.Domain
{
	#region Orderheader

	/// <summary>
	/// Orderheader object for NHibernate mapped table 'SALE_ORDERHEADER'.
	/// </summary>
	[Serializable]
	public class Orderheader : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected DateTime _createddate;
		protected string _createdby;
		protected DateTime _lastediteddate;
		protected string _lasteditedby;
		protected string _ordernumber;
        protected DateTime _orderdate;
        protected DateTime _dealer_orderdate;
		protected decimal _ordertimes;
		protected DateTime _shippingdate;
		protected string _shippingto;
		protected int _status;
		protected string _dealercode;
        protected long _subtotal;
        protected long _bonusAmount;
		protected long _taxamt;
		protected long _freight;
		protected string _dealercomment;
		protected string _vmepcomment;
		protected int _referenceorderid;
		protected string _areacode;
		protected string _databasecode;
		protected int _deliveredstatus;
		protected string _secondaryshippingto;
		protected string _secondaryshippingcode;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Orderheader() { }

		public Orderheader(DateTime createddate, string createdby, DateTime lastediteddate, string lasteditedby, string ordernumber, DateTime orderdate, decimal ordertimes, DateTime shippingdate, string shippingto, int status, string dealercode, long subtotal, long taxamt, long freight, string dealercomment, string vmepcomment, int referenceorderid, string areacode, string databasecode, int deliveredstatus, string secondaryshippingto, string secondaryshippingcode)
		{
			this._createddate = createddate;
			this._createdby = createdby;
			this._lastediteddate = lastediteddate;
			this._lasteditedby = lasteditedby;
			this._ordernumber = ordernumber;
			this._orderdate = orderdate;
			this._ordertimes = ordertimes;
			this._shippingdate = shippingdate;
			this._shippingto = shippingto;
			this._status = status;
			this._dealercode = dealercode;
			this._subtotal = subtotal;
			this._taxamt = taxamt;
			this._freight = freight;
			this._dealercomment = dealercomment;
			this._vmepcomment = vmepcomment;
			this._referenceorderid = referenceorderid;
			this._areacode = areacode;
			this._databasecode = databasecode;
			this._deliveredstatus = deliveredstatus;
			this._secondaryshippingto = secondaryshippingto;
			this._secondaryshippingcode = secondaryshippingcode;
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

        public DateTime Orderdate
        {
            get { return _orderdate; }
            set { _orderdate = value; }
        }
        public DateTime DealerOrderdate
        {
            get { return _dealer_orderdate; }
            set { _dealer_orderdate = value; }
        }

		public decimal Ordertimes
		{
			get { return _ordertimes; }
			set { _ordertimes = value; }
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

		public int Status
		{
			get { return _status; }
			set { _status = value; }
		}
        public long BonusAmount
        {
            get { return _bonusAmount; }
            set { _bonusAmount = value; }
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

		public long Subtotal
		{
			get { return _subtotal; }
			set { _subtotal = value; }
		}

		public long Taxamt
		{
			get { return _taxamt; }
			set { _taxamt = value; }
		}

		public long Freight
		{
			get { return _freight; }
			set { _freight = value; }
		}

		public string Dealercomment
		{
			get { return _dealercomment; }
			set
			{
				if (value != null && value.Length > 4000)
					throw new ArgumentOutOfRangeException("Invalid value for Dealercomment", value, value.ToString());
				_dealercomment = value;
			}
		}

		public string Vmepcomment
		{
			get { return _vmepcomment; }
			set
			{
				if (value != null && value.Length > 4000)
					throw new ArgumentOutOfRangeException("Invalid value for Vmepcomment", value, value.ToString());
				_vmepcomment = value;
			}
		}

		public int Referenceorderid
		{
			get { return _referenceorderid; }
			set { _referenceorderid = value; }
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

		public int Deliveredstatus
		{
			get { return _deliveredstatus; }
			set { _deliveredstatus = value; }
		}

		public string Secondaryshippingto
		{
			get { return _secondaryshippingto; }
			set
			{
				if (value != null && value.Length > 512)
					throw new ArgumentOutOfRangeException("Invalid value for Secondaryshippingto", value, value.ToString());
				_secondaryshippingto = value;
			}
		}

		public string Secondaryshippingcode
		{
			get { return _secondaryshippingcode; }
			set
			{
				if (value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Secondaryshippingcode", value, value.ToString());
				_secondaryshippingcode = value;
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
			if (!(obj is Orderheader))
				throw new InvalidCastException("This object is not of type Orderheader");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Orderheader)obj).Id);
					break;
				case "Createddate":
					relativeValue = this.Createddate.CompareTo(((Orderheader)obj).Createddate);
					break;
				case "Createdby":
					relativeValue = this.Createdby.CompareTo(((Orderheader)obj).Createdby);
					break;
				case "Lastediteddate":
					relativeValue = this.Lastediteddate.CompareTo(((Orderheader)obj).Lastediteddate);
					break;
				case "Lasteditedby":
					relativeValue = this.Lasteditedby.CompareTo(((Orderheader)obj).Lasteditedby);
					break;
				case "Ordernumber":
					relativeValue = (this.Ordernumber != null) ? this.Ordernumber.CompareTo(((Orderheader)obj).Ordernumber) : -1;
					break;
				case "Orderdate":
					relativeValue = this.Orderdate.CompareTo(((Orderheader)obj).Orderdate);
					break;
                case "DealerOrderdate":
                    relativeValue = this.DealerOrderdate.CompareTo(((Orderheader)obj).DealerOrderdate);
					break;
				case "Ordertimes":
					relativeValue = this.Ordertimes.CompareTo(((Orderheader)obj).Ordertimes);
					break;
				case "Shippingdate":
					relativeValue = this.Shippingdate.CompareTo(((Orderheader)obj).Shippingdate);
					break;
				case "Shippingto":
					relativeValue = (this.Shippingto != null) ? this.Shippingto.CompareTo(((Orderheader)obj).Shippingto) : -1;
					break;
				case "Status":
					relativeValue = this.Status.CompareTo(((Orderheader)obj).Status);
					break;
				case "Dealercode":
					relativeValue = this.Dealercode.CompareTo(((Orderheader)obj).Dealercode);
					break;
                case "Subtotal":
                    relativeValue = this.Subtotal.CompareTo(((Orderheader)obj).Subtotal);
                    break;
                case "BonusAmount":
                    relativeValue = this.Subtotal.CompareTo(((Orderheader)obj).BonusAmount);
                    break;
                case "Taxamt":
					relativeValue = this.Taxamt.CompareTo(((Orderheader)obj).Taxamt);
					break;
				case "Freight":
					relativeValue = this.Freight.CompareTo(((Orderheader)obj).Freight);
					break;
				case "Dealercomment":
					relativeValue = (this.Dealercomment != null) ? this.Dealercomment.CompareTo(((Orderheader)obj).Dealercomment) : -1;
					break;
				case "Vmepcomment":
					relativeValue = (this.Vmepcomment != null) ? this.Vmepcomment.CompareTo(((Orderheader)obj).Vmepcomment) : -1;
					break;
				case "Referenceorderid":
					relativeValue = this.Referenceorderid.CompareTo(((Orderheader)obj).Referenceorderid);
					break;
				case "Areacode":
					relativeValue = this.Areacode.CompareTo(((Orderheader)obj).Areacode);
					break;
				case "Databasecode":
					relativeValue = (this.Databasecode != null) ? this.Databasecode.CompareTo(((Orderheader)obj).Databasecode) : -1;
					break;
				case "Deliveredstatus":
					relativeValue = this.Deliveredstatus.CompareTo(((Orderheader)obj).Deliveredstatus);
					break;
				case "Secondaryshippingto":
					relativeValue = (this.Secondaryshippingto != null) ? this.Secondaryshippingto.CompareTo(((Orderheader)obj).Secondaryshippingto) : -1;
					break;
				case "Secondaryshippingcode":
					relativeValue = (this.Secondaryshippingcode != null) ? this.Secondaryshippingcode.CompareTo(((Orderheader)obj).Secondaryshippingcode) : -1;
					break;
				default:
					goto case "Id";
			}
			if (Orderheader.SortDirection == SortDirection.Ascending)
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