using System;

namespace VDMS.Core.Domain
{
	#region Exchangepartdetail

	/// <summary>
	/// Exchangepartdetail object for NHibernate mapped table 'SER_EXCHANGEPARTDETAIL'.
	/// </summary>
	[Serializable]
	public class Exchangepartdetail : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected string _partcodeo;
		protected string _partcodem;
		protected string _serialnumber;
		protected long _unitpricem;
		protected long _totalfeeo;
		protected long _totalfeem;
		protected long _unitpriceo;
        protected long _subTotalO;
        protected long _subTotalM;
        protected long _partAmountO;
        protected long _partAmountM;
        
        protected int _partqtyo;
		protected int _partqtym;
		protected string _vmepcomment;
		protected Broken _broken;
		protected Exchangepartheader _exchangepartheader;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Exchangepartdetail() { }

		public Exchangepartdetail(string partcodeo, string partcodem, string serialnumber, long unitpricem, long totalfeeo, long totalfeem, long unitpriceo, int partqtyo, int partqtym, string vmepcomment, Broken broken, Exchangepartheader exchangepartheader)
		{
			this._partcodeo = partcodeo;
			this._partcodem = partcodem;
			this._serialnumber = serialnumber;
			this._unitpricem = unitpricem;
			this._totalfeeo = totalfeeo;
			this._totalfeem = totalfeem;
			this._unitpriceo = unitpriceo;
			this._partqtyo = partqtyo;
			this._partqtym = partqtym;
			this._vmepcomment = vmepcomment;
			this._broken = broken;
			this._exchangepartheader = exchangepartheader;
		}

		#endregion

		#region Public Properties

		public string Partcodeo
		{
			get { return _partcodeo; }
			set
			{
				if (value != null && value.Length > 35)
					throw new ArgumentOutOfRangeException("Invalid value for Partcodeo", value, value.ToString());
				_partcodeo = value;
			}
		}

		public string Partcodem
		{
			get { return _partcodem; }
			set
			{
				if (value != null && value.Length > 35)
					throw new ArgumentOutOfRangeException("Invalid value for Partcodem", value, value.ToString());
				_partcodem = value;
			}
		}

		public string Serialnumber
		{
			get { return _serialnumber; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Serialnumber", value, value.ToString());
				_serialnumber = value;
			}
		}

        public long PartAmountM
        {
            get { return _partAmountM; }
            set { _partAmountM = value; }
        }
        public long PartAmountO
        {
            get { return _partAmountO; }
            set { _partAmountO = value; }
        }
        public long SubTotalO
        {
            get { return _subTotalO; }
            set { _subTotalO = value; }
        }
        public long SubTotalM
        {
            get { return _subTotalM; }
            set { _subTotalM = value; }
        }

        public long Unitpricem
		{
			get { return _unitpricem; }
			set { _unitpricem = value; }
		}

		public long Totalfeeo
		{
			get { return _totalfeeo; }
			set { _totalfeeo = value; }
		}

		public long Totalfeem
		{
			get { return _totalfeem; }
			set { _totalfeem = value; }
		}

		public long Unitpriceo
		{
			get { return _unitpriceo; }
			set { _unitpriceo = value; }
		}

		public int Partqtyo
		{
			get { return _partqtyo; }
			set { _partqtyo = value; }
		}

		public int Partqtym
		{
			get { return _partqtym; }
			set { _partqtym = value; }
		}

		public string Vmepcomment
		{
			get { return _vmepcomment; }
			set
			{
				if (value != null && value.Length > 2048)
					throw new ArgumentOutOfRangeException("Invalid value for Vmepcomment", value, value.ToString());
				_vmepcomment = value;
			}
		}

		public Broken Broken
		{
			get { return _broken; }
			set { _broken = value; }
		}

		public Exchangepartheader Exchangepartheader
		{
			get { return _exchangepartheader; }
			set { _exchangepartheader = value; }
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
			if (!(obj is Exchangepartdetail))
				throw new InvalidCastException("This object is not of type Exchangepartdetail");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Exchangepartdetail)obj).Id);
					break;
				case "Partcodeo":
					relativeValue = this.Partcodeo.CompareTo(((Exchangepartdetail)obj).Partcodeo);
					break;
				case "Partcodem":
					relativeValue = this.Partcodem.CompareTo(((Exchangepartdetail)obj).Partcodem);
					break;
				case "Serialnumber":
					relativeValue = this.Serialnumber.CompareTo(((Exchangepartdetail)obj).Serialnumber);
					break;
                    
                case "SubTotalO":
                    relativeValue = this.SubTotalO.CompareTo(((Exchangepartdetail)obj).SubTotalO);
                    break;
                case "PartAmountM":
                    relativeValue = this.PartAmountM.CompareTo(((Exchangepartdetail)obj).PartAmountM);
                    break;
                case "PartAmountO":
                    relativeValue = this.PartAmountO.CompareTo(((Exchangepartdetail)obj).PartAmountO);
                    break;
                case "SubTotalM":
                    relativeValue = this.SubTotalM.CompareTo(((Exchangepartdetail)obj).SubTotalM);
                    break;
				case "Unitpricem":
					relativeValue = this.Unitpricem.CompareTo(((Exchangepartdetail)obj).Unitpricem);
					break;
				case "Totalfeeo":
					relativeValue = this.Totalfeeo.CompareTo(((Exchangepartdetail)obj).Totalfeeo);
					break;
				case "Totalfeem":
					relativeValue = this.Totalfeem.CompareTo(((Exchangepartdetail)obj).Totalfeem);
					break;
				case "Unitpriceo":
					relativeValue = this.Unitpriceo.CompareTo(((Exchangepartdetail)obj).Unitpriceo);
					break;
				case "Partqtyo":
					relativeValue = this.Partqtyo.CompareTo(((Exchangepartdetail)obj).Partqtyo);
					break;
				case "Partqtym":
					relativeValue = this.Partqtym.CompareTo(((Exchangepartdetail)obj).Partqtym);
					break;
				case "Vmepcomment":
					relativeValue = (this.Vmepcomment != null) ? this.Vmepcomment.CompareTo(((Exchangepartdetail)obj).Vmepcomment) : -1;
					break;
				default:
					goto case "Id";
			}
			if (Exchangepartdetail.SortDirection == SortDirection.Ascending)
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
