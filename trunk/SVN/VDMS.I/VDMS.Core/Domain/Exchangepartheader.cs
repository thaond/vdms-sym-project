using System;

namespace VDMS.Core.Domain
{
	#region Exchangepartheader

	/// <summary>
	/// Exchangepartheader object for NHibernate mapped table 'SER_EXCHANGEPARTHEADER'.
	/// </summary>
	[Serializable]
	public class Exchangepartheader : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected string _enginenumber;
		protected DateTime _damageddate;
		protected long _kmcount;
		protected DateTime _exchangeddate;
        protected DateTime _lastprocesseddate;
		protected string _dealercode;
		protected string _areacode;
		protected long _feeamount;
		protected int _status;
		protected int _road;
		protected int _weather;
		protected int _speed;
		protected int _usage;
		protected string _engine;
		protected string _frame;
		protected string _electric;
		protected string _damaged;
		protected string _reason;
		protected string _comments;
		protected string _vouchernumber;
		protected string _framenumber;
		protected DateTime _purchasedate;
		protected DateTime _exportdate;
		protected string _model;
		protected decimal _proposefeeamount;
		protected Customer _customer;
		protected Serviceheader _serviceheader;
		protected Exchangevoucherheader _exchangevoucherheader;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Exchangepartheader() { }

        public Exchangepartheader(string enginenumber, DateTime damageddate, long kmcount, DateTime exchangeddate, string dealercode, string areacode, long feeamount, int status, int road, int weather, int speed, int usage, string engine, string frame, string electric, string damaged, string reason, string comments, string vouchernumber, string framenumber, DateTime purchasedate, DateTime exportdate, string model, decimal proposefeeamount, Customer customer, Serviceheader serviceheader, Exchangevoucherheader exchangevoucherheader, DateTime lastprocesseddate)
		{
			this._enginenumber = enginenumber;
			this._damageddate = damageddate;
			this._kmcount = kmcount;
			this._exchangeddate = exchangeddate;
			this._dealercode = dealercode;
			this._areacode = areacode;
			this._feeamount = feeamount;
			this._status = status;
			this._road = road;
			this._weather = weather;
			this._speed = speed;
			this._usage = usage;
			this._engine = engine;
			this._frame = frame;
			this._electric = electric;
			this._damaged = damaged;
			this._reason = reason;
			this._comments = comments;
			this._vouchernumber = vouchernumber;
			this._framenumber = framenumber;
			this._purchasedate = purchasedate;
			this._exportdate = exportdate;
			this._model = model;
			this._proposefeeamount = proposefeeamount;
			this._customer = customer;
			this._serviceheader = serviceheader;
			this._exchangevoucherheader = exchangevoucherheader;
            this._lastprocesseddate = lastprocesseddate;
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
        
		public DateTime Damageddate
		{
			get { return _damageddate; }
			set { _damageddate = value; }
		}
        public DateTime Lastprocesseddate
		{
            get { return _lastprocesseddate; }
            set { _lastprocesseddate = value; }
		}
		public long Kmcount
		{
			get { return _kmcount; }
			set { _kmcount = value; }
		}

		public DateTime Exchangeddate
		{
			get { return _exchangeddate; }
			set { _exchangeddate = value; }
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

		public string Areacode
		{
			get { return _areacode; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Areacode", value, value.ToString());
				_areacode = value;
			}
		}

		public long Feeamount
		{
			get { return _feeamount; }
			set { _feeamount = value; }
		}

		public int Status
		{
			get { return _status; }
			set { _status = value; }
		}

		public int Road
		{
			get { return _road; }
			set { _road = value; }
		}

		public int Weather
		{
			get { return _weather; }
			set { _weather = value; }
		}

		public int Speed
		{
			get { return _speed; }
			set { _speed = value; }
		}

		public int Usage
		{
			get { return _usage; }
			set { _usage = value; }
		}

		public string Engine
		{
			get { return _engine; }
			set
			{
				if (value != null && value.Length > 512)
					throw new ArgumentOutOfRangeException("Invalid value for Engine", value, value.ToString());
				_engine = value;
			}
		}

		public string Frame
		{
			get { return _frame; }
			set
			{
				if (value != null && value.Length > 512)
					throw new ArgumentOutOfRangeException("Invalid value for Frame", value, value.ToString());
				_frame = value;
			}
		}

		public string Electric
		{
			get { return _electric; }
			set
			{
				if (value != null && value.Length > 512)
					throw new ArgumentOutOfRangeException("Invalid value for Electric", value, value.ToString());
				_electric = value;
			}
		}

		public string Damaged
		{
			get { return _damaged; }
			set
			{
				if (value != null && value.Length > 512)
					_damaged = (value != null) ? value.Substring(0, 512) : value;
				_damaged = value;
			}
		}

		public string Reason
		{
			get { return _reason; }
			set
			{
				if (value != null && value.Length > 512)
					_reason = (value != null) ? value.Substring(0, 512) : value;
				_reason = value;
			}
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

		public string Vouchernumber
		{
			get { return _vouchernumber; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Vouchernumber", value, value.ToString());
				_vouchernumber = value;
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

		public DateTime Exportdate
		{
			get { return _exportdate; }
			set { _exportdate = value; }
		}

		public string Model
		{
			get { return _model; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Model", value, value.ToString());
				_model = value;
			}
		}

		public decimal Proposefeeamount
		{
			get { return _proposefeeamount; }
			set { _proposefeeamount = value; }
		}

		public Customer Customer
		{
			get { return _customer; }
			set { _customer = value; }
		}

		public Serviceheader Serviceheader
		{
			get { return _serviceheader; }
			set { _serviceheader = value; }
		}

		public Exchangevoucherheader Exchangevoucherheader
		{
			get { return _exchangevoucherheader; }
			set { _exchangevoucherheader = value; }
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
			if (!(obj is Exchangepartheader))
				throw new InvalidCastException("This object is not of type Exchangepartheader");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Exchangepartheader)obj).Id);
					break;
				case "Enginenumber":
					relativeValue = this.Enginenumber.CompareTo(((Exchangepartheader)obj).Enginenumber);
					break;
				case "Damageddate":
					relativeValue = this.Damageddate.CompareTo(((Exchangepartheader)obj).Damageddate);
					break;
				case "Kmcount":
					relativeValue = this.Kmcount.CompareTo(((Exchangepartheader)obj).Kmcount);
					break;
                case "Lastprocesseddate":
                    relativeValue = this.Lastprocesseddate.CompareTo(((Exchangepartheader)obj).Lastprocesseddate);
					break;
				case "Exchangeddate":
					relativeValue = this.Exchangeddate.CompareTo(((Exchangepartheader)obj).Exchangeddate);
					break;
				case "Dealercode":
					relativeValue = this.Dealercode.CompareTo(((Exchangepartheader)obj).Dealercode);
					break;
				case "Areacode":
					relativeValue = this.Areacode.CompareTo(((Exchangepartheader)obj).Areacode);
					break;
				case "Feeamount":
					relativeValue = this.Feeamount.CompareTo(((Exchangepartheader)obj).Feeamount);
					break;
				case "Status":
					relativeValue = this.Status.CompareTo(((Exchangepartheader)obj).Status);
					break;
				case "Road":
					relativeValue = this.Road.CompareTo(((Exchangepartheader)obj).Road);
					break;
				case "Weather":
					relativeValue = this.Weather.CompareTo(((Exchangepartheader)obj).Weather);
					break;
				case "Speed":
					relativeValue = this.Speed.CompareTo(((Exchangepartheader)obj).Speed);
					break;
				case "Usage":
					relativeValue = this.Usage.CompareTo(((Exchangepartheader)obj).Usage);
					break;
				case "Engine":
					relativeValue = (this.Engine != null) ? this.Engine.CompareTo(((Exchangepartheader)obj).Engine) : -1;
					break;
				case "Frame":
					relativeValue = (this.Frame != null) ? this.Frame.CompareTo(((Exchangepartheader)obj).Frame) : -1;
					break;
				case "Electric":
					relativeValue = (this.Electric != null) ? this.Electric.CompareTo(((Exchangepartheader)obj).Electric) : -1;
					break;
				case "Damaged":
					relativeValue = this.Damaged.CompareTo(((Exchangepartheader)obj).Damaged);
					break;
				case "Reason":
					relativeValue = this.Reason.CompareTo(((Exchangepartheader)obj).Reason);
					break;
				case "Comments":
					relativeValue = (this.Comments != null) ? this.Comments.CompareTo(((Exchangepartheader)obj).Comments) : -1;
					break;
				case "Vouchernumber":
					relativeValue = this.Vouchernumber.CompareTo(((Exchangepartheader)obj).Vouchernumber);
					break;
				case "Framenumber":
					relativeValue = (this.Framenumber != null) ? this.Framenumber.CompareTo(((Exchangepartheader)obj).Framenumber) : -1;
					break;
				case "Purchasedate":
					relativeValue = this.Purchasedate.CompareTo(((Exchangepartheader)obj).Purchasedate);
					break;
				case "Exportdate":
					relativeValue = (this.Exportdate != null) ? this.Exportdate.CompareTo(((Exchangepartheader)obj).Exportdate) : -1;
					break;
				case "Model":
					relativeValue = (this.Model != null) ? this.Model.CompareTo(((Exchangepartheader)obj).Model) : -1;
					break;
				case "Proposefeeamount":
					relativeValue = this.Proposefeeamount.CompareTo(((Exchangepartheader)obj).Proposefeeamount);
					break;
				default:
					goto case "Id";
			}
			if (Exchangepartheader.SortDirection == SortDirection.Ascending)
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
