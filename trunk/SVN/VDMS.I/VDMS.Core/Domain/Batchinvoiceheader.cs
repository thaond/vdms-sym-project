using System;

namespace VDMS.Core.Domain
{
	#region Batchinvoiceheader

	/// <summary>
	/// Batchinvoiceheader object for NHibernate mapped table 'SALE_BATCHINVOICEHEADER'.
	/// </summary>
	public class Batchinvoiceheader : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected string _dealercode;
        protected string _branchcode;
        protected DateTime _createddate;
		protected string _createdby;
		protected string _batchinvoicenumber;
		protected Sellitem _sellitem;
		protected Subshop _subshop;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Batchinvoiceheader() { }

        public Batchinvoiceheader(string dealercode, string branchcode, DateTime createddate, string createdby, string batchinvoicenumber, Sellitem sellitem, Subshop subshop)
		{
			this._dealercode = dealercode;
            this._branchcode = branchcode;
            this._createddate = createddate;
			this._createdby = createdby;
			this._batchinvoicenumber = batchinvoicenumber;
			this._sellitem = sellitem;
			this._subshop = subshop;
		}

		#endregion

		#region Public Properties

		public string Dealercode
		{
			get { return _dealercode; }
			set
			{
				if (value != null && value.Length > 20)
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

		public string Batchinvoicenumber
		{
			get { return _batchinvoicenumber; }
			set
			{
				if (value != null && value.Length > 60)
					throw new ArgumentOutOfRangeException("Invalid value for Batchinvoicenumber", value, value.ToString());
				_batchinvoicenumber = value;
			}
		}

		public Sellitem Sellitem
		{
			get { return _sellitem; }
			set { _sellitem = value; }
		}

		public Subshop Subshop
		{
			get { return _subshop; }
			set { _subshop = value; }
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
			if (!(obj is Batchinvoiceheader))
				throw new InvalidCastException("This object is not of type Batchinvoiceheader");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Batchinvoiceheader)obj).Id);
					break;
				case "Dealercode":
					relativeValue = this.Dealercode.CompareTo(((Batchinvoiceheader)obj).Dealercode);
					break;
                case "Branchcode":
                    relativeValue = this.Branchcode.CompareTo(((Invoice)obj).Branchcode);
                    break;
                case "Createddate":
					relativeValue = this.Createddate.CompareTo(((Batchinvoiceheader)obj).Createddate);
					break;
				case "Createdby":
					relativeValue = this.Createdby.CompareTo(((Batchinvoiceheader)obj).Createdby);
					break;
				case "Batchinvoicenumber":
					relativeValue = this.Batchinvoicenumber.CompareTo(((Batchinvoiceheader)obj).Batchinvoicenumber);
					break;
				default:
					goto case "Id";
			}
			if (Batchinvoiceheader.SortDirection == SortDirection.Ascending)
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