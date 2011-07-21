using System;

namespace VDMS.Core.Domain
{
	#region Serviceheader

	/// <summary>
	/// Serviceheader object for NHibernate mapped table 'SER_SERVICEHEADER'.
	/// </summary>
	[Serializable]
	public class Serviceheader : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected string _enginenumber;
		protected DateTime _servicedate;
		protected int _servicetype;
		protected string _damaged;
		protected string _repairresult;
		protected long _kmcount;
		protected string _comments;
		protected string _servicesheetnumber;
		protected long _feeamount;
		protected int _status;
		protected long _totalamount;
		protected string _numberplate;
		protected string _framenumber;
		protected DateTime _purchasedate;
		protected string _itemtype;
		protected string _colorcode;
		protected string _dealercode;
		protected string _branchcode;
		protected string _createby;
		protected Customer _customer;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Serviceheader() { }

		public Serviceheader(string enginenumber, DateTime servicedate, int servicetype, string damaged, string repairresult, long kmcount, string comments, string servicesheetnumber, long feeamount, long totalamount, string numberplate, string framenumber, DateTime purchasedate, string itemtype, string colorcode, string dealercode, string branchcode, string createby, Customer customer, int status)
		{
			this._enginenumber = enginenumber;
			this._servicedate = servicedate;
			this._servicetype = servicetype;
			this._damaged = damaged;
			this._repairresult = repairresult;
			this._kmcount = kmcount;
			this._comments = comments;
			this._servicesheetnumber = servicesheetnumber;
			this._feeamount = feeamount;
			this._status = status;
			this._totalamount = totalamount;
			this._numberplate = numberplate;
			this._framenumber = framenumber;
			this._purchasedate = purchasedate;
			this._itemtype = itemtype;
			this._colorcode = colorcode;
			this._dealercode = dealercode;
			this._branchcode = branchcode;
			this._createby = createby;
			this._customer = customer;
		}

		#endregion

		#region Public Properties

		public string Enginenumber
		{
			get { return _enginenumber; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Enginenumber", value, value.ToString());
				_enginenumber = value;
			}
		}

		public int Status
		{
			get { return _status; }
			set { _status = value; }
		}

		public DateTime Servicedate
		{
			get { return _servicedate; }
			set { _servicedate = value; }
		}

		public int Servicetype
		{
			get { return _servicetype; }
			set { _servicetype = value; }
		}

		public string Damaged
		{
			get { return _damaged; }
			set
			{
				if (value != null && value.Length > 1000)
					_damaged = (value != null) ? value.Substring(0, 1000) : value;
				_damaged = value;
			}
		}
		public string Createby
		{
			get { return _createby; }
			set
			{
				if (value != null && value.Length > 30)
					_createby = (value != null) ? value.Substring(0, 30) : value;
				_createby = value;
			}
		}

		public string Repairresult
		{
			get { return _repairresult; }
			set
			{
				if (value != null && value.Length > 1000)
					_repairresult = (value != null) ? value.Substring(0, 1000) : value;
				_repairresult = value;
			}
		}

		public long Kmcount
		{
			get { return _kmcount; }
			set { _kmcount = value; }
		}

		public string Comments
		{
			get { return _comments; }
			set
			{
				if (value != null && value.Length > 1000)
					_comments = (value != null) ? value.Substring(0, 1000) : value;
				_comments = value;
			}
		}

		public string Servicesheetnumber
		{
			get { return _servicesheetnumber; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Servicesheetnumber", value, value.ToString());
				_servicesheetnumber = value;
			}
		}

		public long Feeamount
		{
			get { return _feeamount; }
			set { _feeamount = value; }
		}

		public long Totalamount
		{
			get { return _totalamount; }
			set { _totalamount = value; }
		}

		public string Numberplate
		{
			get { return _numberplate; }
			set
			{
				if (value != null && value.Length > 15)
					throw new ArgumentOutOfRangeException("Invalid value for Numberplate", value, value.ToString());
				_numberplate = value;
			}
		}

		public string Framenumber
		{
			get { return _framenumber; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Framenumber", value, value.ToString());
				_framenumber = value;
			}
		}

		public DateTime Purchasedate
		{
			get { return _purchasedate; }
			set { _purchasedate = value; }
		}

		public string Itemtype
		{
			get { return _itemtype; }
			set
			{
				if (value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Itemtype", value, value.ToString());
				_itemtype = value;
			}
		}

		public string Colorcode
		{
			get { return _colorcode; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Colorcode", value, value.ToString());
				_colorcode = value;
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
			if (!(obj is Serviceheader))
				throw new InvalidCastException("This object is not of type Serviceheader");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Serviceheader)obj).Id);
					break;
				case "Enginenumber":
					relativeValue = this.Enginenumber.CompareTo(((Serviceheader)obj).Enginenumber);
					break;
				case "Servicedate":
					relativeValue = this.Servicedate.CompareTo(((Serviceheader)obj).Servicedate);
					break;
				case "Servicetype":
					relativeValue = this.Servicetype.CompareTo(((Serviceheader)obj).Servicetype);
					break;
				case "Damaged":
					relativeValue = this.Damaged.CompareTo(((Serviceheader)obj).Damaged);
					break;
				case "Repairresult":
					relativeValue = (this.Repairresult != null) ? this.Repairresult.CompareTo(((Serviceheader)obj).Repairresult) : -1;
					break;
				case "Kmcount":
					relativeValue = this.Kmcount.CompareTo(((Serviceheader)obj).Kmcount);
					break;
				case "Comments":
					relativeValue = (this.Comments != null) ? this.Comments.CompareTo(((Serviceheader)obj).Comments) : -1;
					break;
				case "Createby":
					relativeValue = (this.Comments != null) ? this.Comments.CompareTo(((Serviceheader)obj).Createby) : -1;
					break;
				case "Servicesheetnumber":
					relativeValue = this.Servicesheetnumber.CompareTo(((Serviceheader)obj).Servicesheetnumber);
					break;
				case "Feeamount":
					relativeValue = this.Feeamount.CompareTo(((Serviceheader)obj).Feeamount);
					break;
				case "Totalamount":
					relativeValue = this.Totalamount.CompareTo(((Serviceheader)obj).Totalamount);
					break;
				case "Numberplate":
					relativeValue = (this.Numberplate != null) ? this.Numberplate.CompareTo(((Serviceheader)obj).Numberplate) : -1;
					break;
				case "Framenumber":
					relativeValue = (this.Framenumber != null) ? this.Framenumber.CompareTo(((Serviceheader)obj).Framenumber) : -1;
					break;
				case "Status":
					relativeValue = this.Status.CompareTo(((Serviceheader)obj).Status);
					break;
				case "Purchasedate":
					relativeValue = this.Purchasedate.CompareTo(((Serviceheader)obj).Purchasedate);
					break;
				case "Itemtype":
					relativeValue = this.Itemtype.CompareTo(((Serviceheader)obj).Itemtype);
					break;
				case "Colorcode":
					relativeValue = this.Colorcode.CompareTo(((Serviceheader)obj).Colorcode);
					break;
				case "Dealercode":
					relativeValue = this.Dealercode.CompareTo(((Serviceheader)obj).Dealercode);
					break;
				case "Branchcode":
					relativeValue = this.Branchcode.CompareTo(((Serviceheader)obj).Branchcode);
					break;
				default:
					goto case "Id";
			}
			if (Serviceheader.SortDirection == SortDirection.Ascending)
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
