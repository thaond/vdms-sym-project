using System;

namespace VDMS.Core.Domain
{
	#region Returnitem

	/// <summary>
	/// Returnitem object for NHibernate mapped table 'SALE_RETURNITEM'.
	/// </summary>
	public class Returnitem : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected string _returnreason;
		protected int _status;
		protected string _returnnumber;
		protected string _vmepcomment;
        protected string _dealcode;
        protected string _branchcode;
        protected DateTime _releasedate;
        protected DateTime _confirmdate;
		protected Iteminstance _iteminstance;
        protected static String _sortExpression = "Confirmdate";
		protected static SortDirection _sortDirection = SortDirection.Descending;


		#endregion

		#region Constructors

		public Returnitem() { }

        public Returnitem(string returnreason, int status, string returnnumber, string vmepcomment, string dealercode, string branchcode, DateTime releasedate, DateTime confirmdate, Iteminstance iteminstance)
		{
			this._returnreason = returnreason;
			this._status = status;
			this._returnnumber = returnnumber;
			this._vmepcomment = vmepcomment;
			this._iteminstance = iteminstance;
            this._branchcode = branchcode;
            this._dealcode = dealercode;
            this._releasedate = releasedate;
		}

		#endregion

		#region Public Properties

		public string Returnreason
		{
			get { return _returnreason; }
			set
			{
				if (value != null && value.Length > 4000)
					throw new ArgumentOutOfRangeException("Invalid value for Returnreason", value, value.ToString());
				_returnreason = value;
			}
		}

		public int Status
		{
			get { return _status; }
			set { _status = value; }
		}

		public string Returnnumber
		{
			get { return _returnnumber; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Returnnumber", value, value.ToString());
				_returnnumber = value;
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


        public string Dealercode
        {
            get { return _dealcode; }
            set
            {
                if (value != null && value.Length > 30)
                    throw new ArgumentOutOfRangeException("Invalid value for DealerCode", value, value.ToString());
                _dealcode = value;
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

        public DateTime Confirmdate
        {
            get { return _confirmdate; }
            set
            {   
                _confirmdate = value;
            }
        }

        public DateTime Releasedate
        {
            get { return _releasedate; }
            set
            {
                _releasedate = value;
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
			if (!(obj is Returnitem))
				throw new InvalidCastException("This object is not of type Returnitem");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Returnitem)obj).Id);
					break;
				case "Returnreason":
					relativeValue = this.Returnreason.CompareTo(((Returnitem)obj).Returnreason);
					break;
				case "Status":
					relativeValue = this.Status.CompareTo(((Returnitem)obj).Status);
					break;
				case "Returnnumber":
					relativeValue = (this.Returnnumber != null) ? this.Returnnumber.CompareTo(((Returnitem)obj).Returnnumber) : -1;
					break;
				case "Vmepcomment":
					relativeValue = (this.Vmepcomment != null) ? this.Vmepcomment.CompareTo(((Returnitem)obj).Vmepcomment) : -1;
					break;
                case "Dealercode":
                    relativeValue = this.Dealercode.CompareTo(((Returnitem)obj).Dealercode);
                    break;
                case "Branchcode":
                    relativeValue = this.Branchcode.CompareTo(((Returnitem)obj).Branchcode);
                    break;
                case "Confirmdate":
                    relativeValue = (this.Confirmdate != null) ? this.Confirmdate.CompareTo(((Returnitem)obj).Confirmdate) : -1;
                    break;
                case "Releasedate":
                    relativeValue = (this.Releasedate != null) ? this.Releasedate.CompareTo(((Returnitem)obj).Releasedate) : -1;
                    break;
				default:
					goto case "Id";
			}
			if (Returnitem.SortDirection == SortDirection.Ascending)
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
