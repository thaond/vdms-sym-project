using System;

namespace VDMS.Core.Domain
{
	#region Itemfavorite

	/// <summary>
	/// Itemfavorite object for NHibernate mapped table 'DATA_ITEMFAVORITE'.
	/// </summary>
	public class Itemfavorite : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected string _dealercode;
		protected Item _item;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Itemfavorite() { }

		public Itemfavorite(string dealercode, Item item)
		{
			this._dealercode = dealercode;
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
			if (!(obj is Itemfavorite))
				throw new InvalidCastException("This object is not of type Itemfavorite");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Itemfavorite)obj).Id);
					break;
				case "Dealercode":
					relativeValue = this.Dealercode.CompareTo(((Itemfavorite)obj).Dealercode);
					break;
				default:
					goto case "Id";
			}
			if (Itemfavorite.SortDirection == SortDirection.Ascending)
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
