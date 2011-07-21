using System;

namespace VDMS.Core.Domain
{
	#region Article

	/// <summary>
	/// Article object for NHibernate mapped table 'SYM_ARTICLES'.
	/// </summary>
	[Serializable]
	public class Event : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected DateTime _eventdate;
		protected string _eventname;
		protected string _location;
		protected string _databasecode;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Event() { }

		public Event(DateTime eventdate, string eventname, string location)
		{
			this._eventdate = eventdate;
			this._eventname = eventname;
			this._location = location;
		}

		#endregion

		#region Public Properties

		public DateTime Eventdate
		{
			get { return _eventdate; }
			set { _eventdate = value; }
		}

		public string Eventname
		{
			get { return _eventname; }
			set
			{
				if (value != null && value.Length > 150)
					throw new ArgumentOutOfRangeException("Invalid value for Createdby", value, value.ToString());
				_eventname = value;
			}
		}

		public string Location
		{
			get { return _location; }
			set
			{
				if (value != null && value.Length > 750)
					throw new ArgumentOutOfRangeException("Invalid value for Body", value, value.ToString());
				_location = value;
			}
		}

		public string DatabaseCode
		{
			get { return _databasecode; }
			set
			{
				if (value != null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for Body", value, value.ToString());
				_databasecode = value;
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
			if (!(obj is Event))
				throw new InvalidCastException("This object is not of type Article");

			int relativeValue = 0;
			switch (SortExpression)
			{
				//case "Id":
				//    relativeValue = this.Id.CompareTo(((Article)obj).Id);
				//    break;
				case "Eventdate":
					relativeValue = this.Eventdate.CompareTo(((Event)obj).Eventdate);
					break;
				case "Eventname":
					relativeValue = this.Eventname.CompareTo(((Event)obj).Eventname);
					break;
				case "Location":
					relativeValue = this.Location.CompareTo(((Event)obj).Location);
					break;
				//default:
				//    goto case "Id";
			}
			//if (Article.SortDirection == SortDirection.Ascending)
			//    relativeValue *= -1;
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
