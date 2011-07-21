using System;

namespace VDMS.Core.Domain
{
	#region Iteminstance

	/// <summary>
	/// Iteminstance object for NHibernate mapped table 'DATA_ITEMINSTANCE'.
	/// </summary>
	public class Iteminstance : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected string _dealercode;
		protected string _enginenumber;
		protected string _itemtype;
		protected string _color;
		protected DateTime _importeddate;
		protected int _status;
		protected DateTime _madedate;
		protected string _vmepinvoice;
		protected string _comments;
		protected string _branchcode;
		protected DateTime _createddate;
		protected DateTime _releaseddate;
		protected string _databasecode;
		protected Item _item;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Iteminstance() { }

		public Iteminstance(string dealercode, string enginenumber, string itemtype, string color, DateTime importeddate, int status, DateTime madedate, string vmepinvoice, string comments, string branchcode, DateTime createddate, DateTime releaseddate, string databasecode, Item item)
		{
			this._dealercode = dealercode;
			this._enginenumber = enginenumber;
			this._itemtype = itemtype;
			this._color = color;
			this._importeddate = importeddate;
			this._status = status;
			this._madedate = madedate;
			this._vmepinvoice = vmepinvoice;
			this._comments = comments;
			this._branchcode = branchcode;
			this._createddate = createddate;
			this._releaseddate = releaseddate;
			this._databasecode = databasecode;
			this._item = item;
		}

		#endregion

		#region Public Properties

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
				if (value != null && value.Length > 60)
					throw new ArgumentOutOfRangeException("Invalid value for Color", value, value.ToString());
				_color = value;
			}
		}

		public DateTime Importeddate
		{
			get { return _importeddate; }
			set { _importeddate = value; }
		}

		public int Status
		{
			get { return _status; }
			set { _status = value; }
		}

		public DateTime Madedate
		{
			get { return _madedate; }
			set { _madedate = value; }
		}

		public string Vmepinvoice
		{
			get { return _vmepinvoice; }
			set
			{
				if (value != null && value.Length > 256)
					throw new ArgumentOutOfRangeException("Invalid value for Vmepinvoice", value, value.ToString());
				_vmepinvoice = value;
			}
		}

		public string Comments
		{
			get { return _comments; }
			set
			{
				if (value != null && value.Length > 4000)
					throw new ArgumentOutOfRangeException("Invalid value for Comments", value, value.ToString());
				_comments = value;
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

		public DateTime Releaseddate
		{
			get { return _releaseddate; }
			set { _releaseddate = value; }
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

		public Item Item
		{
			get { return _item; }
			set { _item = value; }
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
			if (!(obj is Iteminstance))
				throw new InvalidCastException("This object is not of type Iteminstance");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Iteminstance)obj).Id);
					break;
				case "Dealercode":
					relativeValue = this.Dealercode.CompareTo(((Iteminstance)obj).Dealercode);
					break;
				case "Enginenumber":
					relativeValue = this.Enginenumber.CompareTo(((Iteminstance)obj).Enginenumber);
					break;
				case "Itemtype":
					relativeValue = this.Itemtype.CompareTo(((Iteminstance)obj).Itemtype);
					break;
				case "Color":
					relativeValue = this.Color.CompareTo(((Iteminstance)obj).Color);
					break;
				case "Importeddate":
					relativeValue = this.Importeddate.CompareTo(((Iteminstance)obj).Importeddate);
					break;
				case "Status":
					relativeValue = this.Status.CompareTo(((Iteminstance)obj).Status);
					break;
				case "Madedate":
					relativeValue = (this.Madedate != null) ? this.Madedate.CompareTo(((Iteminstance)obj).Madedate) : -1;
					break;
				case "Vmepinvoice":
					relativeValue = (this.Vmepinvoice != null) ? this.Vmepinvoice.CompareTo(((Iteminstance)obj).Vmepinvoice) : -1;
					break;
				case "Comments":
					relativeValue = (this.Comments != null) ? this.Comments.CompareTo(((Iteminstance)obj).Comments) : -1;
					break;
				case "Branchcode":
					relativeValue = this.Branchcode.CompareTo(((Iteminstance)obj).Branchcode);
					break;
				case "Createddate":
					relativeValue = this.Createddate.CompareTo(((Iteminstance)obj).Createddate);
					break;
				case "Releaseddate":
					relativeValue = this.Releaseddate.CompareTo(((Iteminstance)obj).Releaseddate);
					break;
				case "Databasecode":
					relativeValue = this.Databasecode.CompareTo(((Iteminstance)obj).Databasecode);
					break;
				default:
					goto case "Id";
			}
			if (Iteminstance.SortDirection == SortDirection.Ascending)
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