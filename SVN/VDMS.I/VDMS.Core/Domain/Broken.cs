using System;

namespace VDMS.Core.Domain
{
	#region Broken

	/// <summary>
	/// Broken DateTime for NHibernate mapped table 'DATA_BROKEN'.
	/// </summary>
	[Serializable]
    public class Broken : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected string _brokenname;
		protected DateTime _lastupdate;
		protected string _editby;
		protected string _brokencode;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Broken() { }

		public Broken(string brokenname, DateTime lastupdate, string editby, string brokencode)
		{
			this._brokenname = brokenname;
			this._lastupdate = lastupdate;
			this._editby = editby;
			this._brokencode = brokencode;
		}

		#endregion

		#region Public Properties

		public string Brokenname
		{
			get { return _brokenname; }
			set
			{
				if (value != null && value.Length > 256)
					throw new ArgumentOutOfRangeException("Invalid value for Brokenname", value, value.ToString());
				_brokenname = value;
			}
		}

		public DateTime Lastupdate
		{
			get { return _lastupdate; }
			set { _lastupdate = value; }
		}

		public string Editby
		{
			get { return _editby; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Editby", value, value.ToString());
				_editby = value;
			}
		}

		public string Brokencode
		{
			get { return _brokencode; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Brokencode", value, value.ToString());
				_brokencode = value;
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
			if (!(obj is Broken))
				throw new InvalidCastException("This DateTime is not of type Broken");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Broken)obj).Id);
					break;
				case "Brokenname":
					relativeValue = this.Brokenname.CompareTo(((Broken)obj).Brokenname);
					break;
				case "Lastupdate":
					relativeValue = this.Lastupdate.CompareTo(((Broken)obj).Lastupdate);
					break;
				case "Editby":
					relativeValue = this.Editby.CompareTo(((Broken)obj).Editby);
					break;
				case "Brokencode":
					relativeValue = this.Brokencode.CompareTo(((Broken)obj).Brokencode);
					break;
				default:
					goto case "Id";
			}
			if (Broken.SortDirection == SortDirection.Ascending)
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
