using System;

namespace VDMS.Core.Domain
{
	#region Task

	/// <summary>
	/// Task object for NHibernate mapped table 'APP_TASKS'.
	/// </summary>
	public class Task : DomainObject<int>, IComparable
	{
		#region Member Variables

		protected string _pathid;
		protected string _taskname;
		protected string _commandname;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Task() { }

		public Task(string pathid, string taskname, string commandname)
		{
			this._pathid = pathid;
			this._taskname = taskname;
			this._commandname = commandname;
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

		public string Taskname
		{
			get { return _taskname; }
			set
			{
				if (value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Taskname", value, value.ToString());
				_taskname = value;
			}
		}

		public string Commandname
		{
			get { return _commandname; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Commandname", value, value.ToString());
				_commandname = value;
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
			if (!(obj is Task))
				throw new InvalidCastException("This object is not of type Task");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Task)obj).Id);
					break;
				case "Pathid":
					relativeValue = this.Pathid.CompareTo(((Task)obj).Pathid);
					break;
				case "Taskname":
					relativeValue = this.Taskname.CompareTo(((Task)obj).Taskname);
					break;
				case "Commandname":
					relativeValue = (this.Commandname != null) ? this.Commandname.CompareTo(((Task)obj).Commandname) : -1;
					break;
				default:
					goto case "Id";
			}
			if (Task.SortDirection == SortDirection.Ascending)
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
