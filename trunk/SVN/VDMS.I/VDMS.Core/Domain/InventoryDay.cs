using System;

namespace VDMS.Core.Domain
{
	#region Inventoryday

	/// <summary>
	/// Inventoryday object for NHibernate mapped table 'SALE_INVENTORYDAY'.
	/// </summary>
	public class Inventoryday : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected int _quantity;
		protected int _actiontype;
		protected string _dealercode;
		protected string _branchcode;
		protected long _actionday;
		protected Item _item;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Inventoryday() { }

		public Inventoryday(int quantity, int actiontype, string dealercode, string branchcode, long actionday, Item item)
		{
			this._quantity = quantity;
			this._actiontype = actiontype;
			this._dealercode = dealercode;
			this._branchcode = branchcode;
			this._actionday = actionday;
			this._item = item;
		}

		#endregion

		#region Public Properties

		public int Quantity
		{
			get { return _quantity; }
			set { _quantity = value; }
		}

		public int Actiontype
		{
			get { return _actiontype; }
			set { _actiontype = value; }
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

		public long Actionday
		{
			get { return _actionday; }
			set { _actionday = value; }
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
			if (!(obj is Inventoryday))
				throw new InvalidCastException("This object is not of type Inventoryday");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Inventoryday)obj).Id);
					break;
				case "Quantity":
					relativeValue = this.Quantity.CompareTo(((Inventoryday)obj).Quantity);
					break;
				case "Actiontype":
					relativeValue = this.Actiontype.CompareTo(((Inventoryday)obj).Actiontype);
					break;
				case "Dealercode":
					relativeValue = this.Dealercode.CompareTo(((Inventoryday)obj).Dealercode);
					break;
				case "Branchcode":
					relativeValue = this.Branchcode.CompareTo(((Inventoryday)obj).Branchcode);
					break;
				case "Actionday":
					relativeValue = this.Actionday.CompareTo(((Inventoryday)obj).Actionday);
					break;
				default:
					goto case "Id";
			}
			if (Inventoryday.SortDirection == SortDirection.Ascending)
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
