using System;

namespace VDMS.Core.Domain
{
	#region Batchinvoicedetail

	/// <summary>
	/// Batchinvoicedetail object for NHibernate mapped table 'SALE_BATCHINVOICEDETAIL'.
	/// </summary>
	public class Batchinvoicedetail : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected string _enginenumber;
		protected DateTime _createddate;
		protected string _createdby;
		protected Iteminstance _iteminstance;
		protected Batchinvoiceheader _batchinvoiceheader;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Batchinvoicedetail() { }

		public Batchinvoicedetail(string enginenumber, DateTime createddate, string createdby, Iteminstance iteminstance, Batchinvoiceheader batchinvoiceheader)
		{
			this._enginenumber = enginenumber;
			this._createddate = createddate;
			this._createdby = createdby;
			this._iteminstance = iteminstance;
			this._batchinvoiceheader = batchinvoiceheader;
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

		public Iteminstance Iteminstance
		{
			get { return _iteminstance; }
			set { _iteminstance = value; }
		}

		public Batchinvoiceheader Batchinvoiceheader
		{
			get { return _batchinvoiceheader; }
			set { _batchinvoiceheader = value; }
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
			if (!(obj is Batchinvoicedetail))
				throw new InvalidCastException("This object is not of type Batchinvoicedetail");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Batchinvoicedetail)obj).Id);
					break;
				case "Enginenumber":
					relativeValue = this.Enginenumber.CompareTo(((Batchinvoicedetail)obj).Enginenumber);
					break;
				case "Createddate":
					relativeValue = this.Createddate.CompareTo(((Batchinvoicedetail)obj).Createddate);
					break;
				case "Createdby":
					relativeValue = this.Createdby.CompareTo(((Batchinvoicedetail)obj).Createdby);
					break;
				default:
					goto case "Id";
			}
			if (Batchinvoicedetail.SortDirection == SortDirection.Ascending)
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