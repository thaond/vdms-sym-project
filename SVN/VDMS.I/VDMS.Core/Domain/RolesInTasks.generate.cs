using System;

namespace VDMS.Core.Domain
{
	#region RolesInTasks

	/// <summary>
	/// RolesInTasks object for NHibernate mapped table 'APP_ROLESINTASKS'.
	/// </summary>
	public class RolesInTasks : DomainObject<int>, IComparable
	{
		#region Member Variables

		protected int _taskid;
		protected string _roleid;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public RolesInTasks() { }

		public RolesInTasks(int taskid, string roleid)
		{
			this._taskid = taskid;
			this._roleid = roleid;
		}

		#endregion

		#region Public Properties

		public int Taskid
		{
			get { return _taskid; }
			set { _taskid = value; }
		}

		public string Roleid
		{
			get { return _roleid; }
			set
			{
				if (value != null && value.Length > 36)
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
			if (!(obj is RolesInTasks))
				throw new InvalidCastException("This object is not of type RolesInTasks");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((RolesInTasks)obj).Id);
					break;
				case "Taskid":
					relativeValue = this.Taskid.CompareTo(((RolesInTasks)obj).Taskid);
					break;
				case "Roleid":
					relativeValue = (this.Roleid != null) ? this.Roleid.CompareTo(((RolesInTasks)obj).Roleid) : -1;
					break;
				default:
					goto case "Id";
			}
			if (RolesInTasks.SortDirection == SortDirection.Ascending)
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
