using System;

namespace VDMS.Core.Domain
{
	#region Inventorylock

	/// <summary>
	/// Inventorylock object for NHibernate mapped table 'SALE_INVENTORYLOCK'.
	/// </summary>
	public class Inventorylock : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected int _month;
		protected int _year;
		protected bool _islocked;
		protected string _dealercode;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Inventorylock() { }

		public Inventorylock(int month, int year, bool islocked, string dealercode)
		{
			this._month = month;
			this._year = year;
			this._islocked = islocked;
			this._dealercode = dealercode;
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

		public bool Islocked
		{
			get { return _islocked; }
			set { _islocked = value; }
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
			if (!(obj is Inventorylock))
				throw new InvalidCastException("This object is not of type Inventorylock");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Inventorylock)obj).Id);
					break;
				case "Month":
					relativeValue = this.Month.CompareTo(((Inventorylock)obj).Month);
					break;
				case "Year":
					relativeValue = this.Year.CompareTo(((Inventorylock)obj).Year);
					break;
				case "Islocked":
					relativeValue = this.Islocked.CompareTo(((Inventorylock)obj).Islocked);
					break;
				case "Dealercode":
					relativeValue = this.Dealercode.CompareTo(((Inventorylock)obj).Dealercode);
					break;
				default:
					goto case "Id";
			}
			if (Inventorylock.SortDirection == SortDirection.Ascending)
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
