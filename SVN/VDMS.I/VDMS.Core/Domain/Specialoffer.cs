using System;

namespace VDMS.Core.Domain
{
	#region Specialoffer

	/// <summary>
	/// Specialoffer object for NHibernate mapped table 'SALE_SPECIALOFFER'.
	/// </summary>
	[Serializable]
	public class Specialoffer : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected DateTime _createddate;
		protected string _createdby;
		protected DateTime _lastediteddate;
		protected string _lasteditedby;
		protected string _description;
		protected long _discountpct;
		protected DateTime _startdate;
		protected DateTime _enddate;
		protected decimal _minqty;
		protected decimal _maxqty;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Specialoffer() { }

		public Specialoffer(DateTime createddate, string createdby, DateTime lastediteddate, string lasteditedby, string description, long discountpct, DateTime startdate, DateTime enddate, decimal minqty, decimal maxqty)
		{
			this._createddate = createddate;
			this._createdby = createdby;
			this._lastediteddate = lastediteddate;
			this._lasteditedby = lasteditedby;
			this._description = description;
			this._discountpct = discountpct;
			this._startdate = startdate;
			this._enddate = enddate;
			this._minqty = minqty;
			this._maxqty = maxqty;
		}

		#endregion

		#region Public Properties

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

		public DateTime Lastediteddate
		{
			get { return _lastediteddate; }
			set { _lastediteddate = value; }
		}

		public string Lasteditedby
		{
			get { return _lasteditedby; }
			set
			{
				if (value != null && value.Length > 512)
					throw new ArgumentOutOfRangeException("Invalid value for Lasteditedby", value, value.ToString());
				_lasteditedby = value;
			}
		}

		public string Description
		{
			get { return _description; }
			set
			{
				if (value != null && value.Length > 512)
					throw new ArgumentOutOfRangeException("Invalid value for Description", value, value.ToString());
				_description = value;
			}
		}

		public long Discountpct
		{
			get { return _discountpct; }
			set { _discountpct = value; }
		}

		public DateTime Startdate
		{
			get { return _startdate; }
			set { _startdate = value; }
		}

		public DateTime Enddate
		{
			get { return _enddate; }
			set { _enddate = value; }
		}

		public decimal Minqty
		{
			get { return _minqty; }
			set { _minqty = value; }
		}

		public decimal Maxqty
		{
			get { return _maxqty; }
			set { _maxqty = value; }
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
			if (!(obj is Specialoffer))
				throw new InvalidCastException("This object is not of type Specialoffer");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Specialoffer)obj).Id);
					break;
				case "Createddate":
					relativeValue = this.Createddate.CompareTo(((Specialoffer)obj).Createddate);
					break;
				case "Createdby":
					relativeValue = this.Createdby.CompareTo(((Specialoffer)obj).Createdby);
					break;
				case "Lastediteddate":
					relativeValue = this.Lastediteddate.CompareTo(((Specialoffer)obj).Lastediteddate);
					break;
				case "Lasteditedby":
					relativeValue = this.Lasteditedby.CompareTo(((Specialoffer)obj).Lasteditedby);
					break;
				case "Description":
					relativeValue = this.Description.CompareTo(((Specialoffer)obj).Description);
					break;
				case "Discountpct":
					relativeValue = this.Discountpct.CompareTo(((Specialoffer)obj).Discountpct);
					break;
				case "Startdate":
					relativeValue = this.Startdate.CompareTo(((Specialoffer)obj).Startdate);
					break;
				case "Enddate":
					relativeValue = this.Enddate.CompareTo(((Specialoffer)obj).Enddate);
					break;
				case "Minqty":
					relativeValue = this.Minqty.CompareTo(((Specialoffer)obj).Minqty);
					break;
				default:
					goto case "Id";
			}
			if (Specialoffer.SortDirection == SortDirection.Ascending)
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
