using System;
using System.Collections;

namespace VDMS.Core.Domain
{
	#region Invoice

	/// <summary>
	/// Invoice object for NHibernate mapped table 'SALE_INVOICE'.
	/// </summary>
	public class Invoice : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected string _enginenumber;
        protected string _dealercode;
        protected string _branchcode;
        protected DateTime _createddate;
        protected DateTime _selldate;
        protected string _createdby;
		protected string _invoicenumber;
		protected Customer _customer;
		protected Iteminstance _iteminstance;
		protected Sellitem _sellitem;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Invoice() { }

        public Invoice(string enginenumber, string dealercode, string branchcode, DateTime createddate, DateTime selldate, string createdby, string invoicenumber, Customer customer, Iteminstance iteminstance, Sellitem sellitem)
		{
			this._enginenumber = enginenumber;
            this._dealercode = dealercode;
            this._branchcode = branchcode;
			this._createddate = createddate;
			this._createdby = createdby;
			this._invoicenumber = invoicenumber;
			this._customer = customer;
			this._iteminstance = iteminstance;
			this._sellitem = sellitem;
            this._selldate = selldate;
		}

		#endregion

		#region Public Properties

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
        public DateTime Selldate
        {
            get { return _selldate; }
            set { _selldate = value; }
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

		public string Invoicenumber
		{
			get { return _invoicenumber; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Invoicenumber", value, value.ToString());
				_invoicenumber = value;
			}
		}

		public Customer Customer
		{
			get { return _customer; }
			set { _customer = value; }
		}

		public Iteminstance Iteminstance
		{
			get { return _iteminstance; }
			set { _iteminstance = value; }
		}

		public Sellitem Sellitem
		{
			get { return _sellitem; }
			set { _sellitem = value; }
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
			if (!(obj is Invoice))
				throw new InvalidCastException("This object is not of type Invoice");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Invoice)obj).Id);
					break;
				case "Enginenumber":
					relativeValue = this.Enginenumber.CompareTo(((Invoice)obj).Enginenumber);
					break;
                case "Dealercode":
                    relativeValue = this.Dealercode.CompareTo(((Invoice)obj).Dealercode);
                    break;
                case "Branchcode":
                    relativeValue = this.Branchcode.CompareTo(((Invoice)obj).Branchcode);
                    break;
                case "Createddate":
					relativeValue = this.Createddate.CompareTo(((Invoice)obj).Createddate);
                    break;
                case "Selldate":
                    relativeValue = this.Selldate.CompareTo(((Invoice)obj).Selldate);
					break;
				case "Createdby":
					relativeValue = this.Createdby.CompareTo(((Invoice)obj).Createdby);
					break;
				case "Invoicenumber":
					relativeValue = this.Invoicenumber.CompareTo(((Invoice)obj).Invoicenumber);
					break;
				default:
					goto case "Id";
			}
			if (Invoice.SortDirection == SortDirection.Ascending)
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