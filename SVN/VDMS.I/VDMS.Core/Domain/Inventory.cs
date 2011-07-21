using System;

namespace VDMS.Core.Domain
{
	#region Saleinventory

	/// <summary>
	/// Saleinventory object for NHibernate mapped table 'SALE_INVENTORY'.
	/// </summary>
	public class Saleinventory : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected int _month;
		protected int _year;
		protected int _quantity;
		protected string _dealercode;
		protected string _branchcode;
		protected Item _item;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Saleinventory() { }

		public Saleinventory(int month, int year, int quantity, string dealercode, string branchcode, Item item)
		{
			this._month = month;
			this._year = year;
			this._quantity = quantity;
			this._dealercode = dealercode;
			this._branchcode = branchcode;
			this._item = item;
		}

		#endregion

		#region Public Properties

		public int Month
		{
			get { return _month; }
			set { _month = value; }
		}

		public int Year
		{
			get { return _year; }
			set { _year = value; }
		}

		public int Quantity
		{
			get { return _quantity; }
			set { _quantity = value; }
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
			if (!(obj is Saleinventory))
				throw new InvalidCastException("This object is not of type Saleinventory");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Saleinventory)obj).Id);
					break;
				case "Month":
					relativeValue = this.Month.CompareTo(((Saleinventory)obj).Month);
					break;
				case "Year":
					relativeValue = this.Year.CompareTo(((Saleinventory)obj).Year);
					break;
				case "Quantity":
					relativeValue = this.Quantity.CompareTo(((Saleinventory)obj).Quantity);
					break;
				case "Dealercode":
					relativeValue = this.Dealercode.CompareTo(((Saleinventory)obj).Dealercode);
					break;
				case "Branchcode":
					relativeValue = this.Branchcode.CompareTo(((Saleinventory)obj).Branchcode);
					break;
				default:
					goto case "Id";
			}
			if (Saleinventory.SortDirection == SortDirection.Ascending)
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
