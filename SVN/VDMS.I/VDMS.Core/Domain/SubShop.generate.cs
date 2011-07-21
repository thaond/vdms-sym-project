using System;

namespace VDMS.Core.Domain
{
	#region Subshop

	/// <summary>
	/// Subshop object for NHibernate mapped table 'DATA_SUBSHOP'.
	/// </summary>
	[Serializable]
	public class Subshop : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected string _dealercode;
		protected string _name;
		protected string _code;
		protected bool _status;
		protected string _address;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Subshop() { }

		public Subshop(string dealercode, string name, string code, bool status, string address)
		{
			this._dealercode = dealercode;
			this._name = name;
			this._code = code;
			this._status = status;
			this._address = address;
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

		public string Name
		{
			get { return _name; }
			set
			{
				if (value != null && value.Length > 512)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string Code
		{
			get { return _code; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Code", value, value.ToString());
				_code = value;
			}
		}

		public bool Status
		{
			get { return _status; }
			set { _status = value; }
		}

		public string Address
		{
			get { return _address; }
			set
			{
				if (value != null && value.Length > 1024)
					throw new ArgumentOutOfRangeException("Invalid value for Address", value, value.ToString());
				_address = value;
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
			if (!(obj is Subshop))
				throw new InvalidCastException("This object is not of type Subshop");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Subshop)obj).Id);
					break;
				case "Dealercode":
					relativeValue = this.Dealercode.CompareTo(((Subshop)obj).Dealercode);
					break;
				case "Name":
					relativeValue = this.Name.CompareTo(((Subshop)obj).Name);
					break;
				case "Code":
					relativeValue = this.Code.CompareTo(((Subshop)obj).Code);
					break;
				case "Status":
					relativeValue = this.Status.CompareTo(((Subshop)obj).Status);
					break;
				case "Address":
					relativeValue = this.Address.CompareTo(((Subshop)obj).Address);
					break;
				default:
					goto case "Id";
			}
			if (Subshop.SortDirection == SortDirection.Ascending)
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
