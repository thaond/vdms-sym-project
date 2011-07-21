using System;
using System.Web;

namespace VDMS.Core.Domain
{
	#region Item

	/// <summary>
	/// Item object for NHibernate mapped table 'DATA_ITEM'.
	/// </summary>
	[Serializable]
    public partial class Item : DomainObject<string>, IComparable
	{
		#region Member Variables

		protected string _itemname;
		protected string _colorcode;
		protected string _colorname;
        protected bool _forhtf;
        protected bool _fordnf;
		protected string _itemtype;
		protected string _databasecode;
		protected long _htfprice;
		protected long _dnfprice;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Item() { }

        public Item(string itemname, string colorcode, string colorname, bool forhtf, bool fordnf, string itemtype, string databasecode, long htfprice, long dnfprice)
		{
			this._itemname = itemname;
			this._colorcode = colorcode;
			this._colorname = colorname;
            this._fordnf = fordnf;
            this._forhtf = forhtf;
            this._itemtype = itemtype;
			this._databasecode = databasecode;
			this._htfprice = htfprice;
			this._dnfprice = dnfprice;
		}

		#endregion

		#region Public Properties

		public string Itemname
		{
			get { return _itemname; }
			set
			{
				if (value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Itemname", value, value.ToString());
				_itemname = value;
			}
		}

		public string Colorcode
		{
			get { return _colorcode; }
			set
			{
				if (value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for Colorcode", value, value.ToString());
				_colorcode = value;
			}
		}

		public string Colorname
		{
			get { return _colorname; }
			set
			{
				if (value != null && value.Length > 60)
					throw new ArgumentOutOfRangeException("Invalid value for Colorname", value, value.ToString());
				_colorname = value;
			}
		}

        public bool Forhtf
        {
            get { return _forhtf; }
            set { _forhtf = value; }
        }

		public bool Fordnf
        {
            get { return _fordnf; }
            set { _fordnf = value; }
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

		public string DatabaseCode
		{
			get { return _databasecode; }
			set
			{
				if (value != null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for Databasecode", value, value.ToString());
				_databasecode = value;
			}
		}

		public long Htfprice
		{
			get { return _htfprice; }
			set { _htfprice = value; }
		}

		public long Dnfprice
		{
			get { return _dnfprice; }
			set { _dnfprice = value; }
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
			if (!(obj is Item))
				throw new InvalidCastException("This object is not of type Item");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Item)obj).Id);
					break;
				case "Itemname":
					relativeValue = this.Itemname.CompareTo(((Item)obj).Itemname);
					break;
				case "Colorcode":
					relativeValue = this.Colorcode.CompareTo(((Item)obj).Colorcode);
					break;
				case "Colorname":
					relativeValue = this.Colorname.CompareTo(((Item)obj).Colorname);
					break;
                case "Forhtf":
                    relativeValue = this.Forhtf.CompareTo(((Item)obj).Forhtf);
                    break;
                case "Fordnf":
                    relativeValue = this.Fordnf.CompareTo(((Item)obj).Fordnf);
                    break;
				case "Itemtype":
					relativeValue = (this.Itemtype != null) ? this.Itemtype.CompareTo(((Item)obj).Itemtype) : -1;
					break;
				case "Databasecode":
					relativeValue = (this.DatabaseCode != null) ? this.DatabaseCode.CompareTo(((Item)obj).DatabaseCode) : -1;
					break;
				case "Htfprice":
					relativeValue = this.Htfprice.CompareTo(((Item)obj).Htfprice);
					break;
				case "Dnfprice":
					relativeValue = this.Dnfprice.CompareTo(((Item)obj).Dnfprice);
					break;
				default:
					goto case "Id";
			}
			if (Item.SortDirection == SortDirection.Ascending)
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
