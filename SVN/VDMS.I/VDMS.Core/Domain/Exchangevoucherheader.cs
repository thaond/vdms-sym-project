using System;

namespace VDMS.Core.Domain
{
	#region Exchangevoucherheader

	/// <summary>
	/// Exchangevoucherheader object for NHibernate mapped table 'SER_EXCHANGEVOUCHERHEADER'.
	/// </summary>
	[Serializable]
	public class Exchangevoucherheader : DomainObject<string>, IComparable
	{
		#region Member Variables

		protected DateTime _createddate;
		protected DateTime _lastprocesseddate;
		protected string _dealercode;
		protected int _status;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Exchangevoucherheader() { }

		public Exchangevoucherheader(DateTime createddate, DateTime lastprocesseddate, string dealercode, int status)
		{
			this._createddate = createddate;
			this._lastprocesseddate = lastprocesseddate;
			this._dealercode = dealercode;
			this._status = status;
		}

		#endregion

		#region Public Properties

		public DateTime Createddate
		{
			get { return _createddate; }
			set { _createddate = value; }
		}

		public DateTime Lastprocesseddate
		{
			get { return _lastprocesseddate; }
			set { _lastprocesseddate = value; }
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

		public int Status
		{
			get { return _status; }
			set { _status = value; }
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
			if (!(obj is Exchangevoucherheader))
				throw new InvalidCastException("This object is not of type Exchangevoucherheader");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Exchangevoucherheader)obj).Id);
					break;
				case "Createddate":
					relativeValue = this.Createddate.CompareTo(((Exchangevoucherheader)obj).Createddate);
					break;
				case "Lastprocesseddate":
					relativeValue = this.Lastprocesseddate.CompareTo(((Exchangevoucherheader)obj).Lastprocesseddate);
					break;
				case "Dealercode":
					relativeValue = (this.Dealercode != null) ? this.Dealercode.CompareTo(((Exchangevoucherheader)obj).Dealercode) : -1;
					break;
				case "Status":
					relativeValue = this.Status.CompareTo(((Exchangevoucherheader)obj).Status);
					break;
				default:
					goto case "Id";
			}
			if (Exchangevoucherheader.SortDirection == SortDirection.Ascending)
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
