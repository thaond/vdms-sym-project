using System;

namespace VDMS.Core.Domain
{
	#region RolesInPaths

	/// <summary>
	/// RolesInPaths object for NHibernate mapped table 'APP_ROLESINPATHS'.
	/// </summary>
	public class RolesInPaths : DomainObject<int>, IComparable
	{
		#region Member Variables

		protected string _pathid;
		protected string _roleid;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public RolesInPaths() { }

		public RolesInPaths(string pathid, string roleid)
		{
			this._pathid = pathid;
			this._roleid = roleid;
		}

		#endregion

		#region Public Properties

		public string Pathid
		{
			get { return _pathid; }
			set
			{
				if (value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Pathid", value, value.ToString());
				_pathid = value;
			}
		}

		public string Roleid
		{
			get { return _roleid; }
			set
			{
				if (value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Roleid", value, value.ToString());
				_roleid = value;
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
			if (!(obj is RolesInPaths))
				throw new InvalidCastException("This object is not of type RolesInPaths");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((RolesInPaths)obj).Id);
					break;
				case "Pathid":
					relativeValue = this.Pathid.CompareTo(((RolesInPaths)obj).Pathid);
					break;
				case "Roleid":
					relativeValue = (this.Roleid != null) ? this.Roleid.CompareTo(((RolesInPaths)obj).Roleid) : -1;
					break;
				default:
					goto case "Id";
			}
			if (RolesInPaths.SortDirection == SortDirection.Ascending)
				relativeValue *= -1;
			return relativeValue;
		}
		#endregion

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}

	#endregion
}
