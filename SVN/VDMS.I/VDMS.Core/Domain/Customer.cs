using System;

namespace VDMS.Core.Domain
{
	#region Customer

	/// <summary>
	/// Customer object for NHibernate mapped table 'SYM_CUSTOMER'.
	/// </summary>
	[Serializable]
	public class Customer : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected string _identifynumber;
		protected string _fullname;
		protected bool _gender;
		protected DateTime _birthdate;
		protected string _address;
		protected string _provinceid;
		protected string _districtid;
		protected int _jobtypeid;
		protected string _email;
		protected string _tel;
		protected string _mobile;
		protected int _customertype;
		protected string _customerdescription;
		protected string _precinct;
		protected int _priority;
		protected string _dealercode;
		protected bool _forservice;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Customer() { }

		public Customer(string identifynumber, string fullname, bool gender, DateTime birthdate, string address, string provinceid, string districtid, int jobtypeid, string email, string tel, string mobile, int customertype, string customerdescription, string precinct, int priority, string dealercode, bool forservice)
		{
			this._identifynumber = identifynumber;
			this._fullname = fullname;
			this._gender = gender;
			this._birthdate = birthdate;
			this._address = address;
			this._provinceid = provinceid;
			this._districtid = districtid;
			this._jobtypeid = jobtypeid;
			this._email = email;
			this._tel = tel;
			this._mobile = mobile;
			this._customertype = customertype;
			this._customerdescription = customerdescription;
			this._precinct = precinct;
			this._priority = priority;
			this._dealercode = dealercode;
			this._forservice = forservice;
		}

		#endregion

		#region Public Properties

		public string Identifynumber
		{
			get { return _identifynumber; }
			set
			{
				if (value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for Identifynumber", value, value.ToString());
				_identifynumber = value;
			}
		}

		public string Fullname
		{
			get { return _fullname; }
			set
			{
				if (value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Fullname", value, value.ToString());
				_fullname = value;
			}
		}

		public bool Gender
		{
			get { return _gender; }
			set { _gender = value; }
		}

		public DateTime Birthdate
		{
			get { return _birthdate; }
			set { _birthdate = value; }
		}

		public string Address
		{
			get { return _address; }
			set
			{
				if (value != null && value.Length > 265)
					throw new ArgumentOutOfRangeException("Invalid value for Address", value, value.ToString());
				_address = value;
			}
		}

		public string Provinceid
		{
			get { return _provinceid; }
			set
			{
				if (value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for Provinceid", value, value.ToString());
				_provinceid = value;
			}
		}

		public string Districtid
		{
			get { return _districtid; }
			set
			{
				if (value != null && value.Length > 256)
					throw new ArgumentOutOfRangeException("Invalid value for Districtid", value, value.ToString());
				_districtid = value;
			}
		}

		public int Jobtypeid
		{
			get { return _jobtypeid; }
			set { _jobtypeid = value; }
		}

		public string Email
		{
			get { return _email; }
			set
			{
				if (value != null && value.Length > 256)
					throw new ArgumentOutOfRangeException("Invalid value for Email", value, value.ToString());
				_email = value;
			}
		}

		public string Tel
		{
			get { return _tel; }
			set
			{
				if (value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for Tel", value, value.ToString());
				_tel = value;
			}
		}

		public string Mobile
		{
			get { return _mobile; }
			set
			{
				if (value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for Mobile", value, value.ToString());
				_mobile = value;
			}
		}

		public int Customertype
		{
			get { return _customertype; }
			set { _customertype = value; }
		}

		public string Customerdescription
		{
			get { return _customerdescription; }
			set
			{
				if (value != null && value.Length > 4000)
					throw new ArgumentOutOfRangeException("Invalid value for Customerdescription", value, value.ToString());
				_customerdescription = value;
			}
		}

		public string Precinct
		{
			get { return _precinct; }
			set
			{
				if (value != null && value.Length > 256)
					throw new ArgumentOutOfRangeException("Invalid value for Precinct", value, value.ToString());
				_precinct = value;
			}
		}

		public int Priority
		{
			get { return _priority; }
			set { _priority = value; }
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

		public bool Forservice
		{
			get { return _forservice; }
			set { _forservice = value; }
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
			if (!(obj is Customer))
				throw new InvalidCastException("This object is not of type Customer");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Customer)obj).Id);
					break;
				case "Identifynumber":
					relativeValue = (this.Identifynumber != null) ? this.Identifynumber.CompareTo(((Customer)obj).Identifynumber) : -1;
					break;
				case "Fullname":
					relativeValue = this.Fullname.CompareTo(((Customer)obj).Fullname);
					break;
				case "Gender":
					relativeValue = this.Gender.CompareTo(((Customer)obj).Gender);
					break;
				case "Birthdate":
					relativeValue = (this.Birthdate != null) ? this.Birthdate.CompareTo(((Customer)obj).Birthdate) : -1;
					break;
				case "Address":
					relativeValue = (this.Address != null) ? this.Address.CompareTo(((Customer)obj).Address) : -1;
					break;
				case "Provinceid":
					relativeValue = (this.Provinceid != null) ? this.Provinceid.CompareTo(((Customer)obj).Provinceid) : -1;
					break;
				case "Districtid":
					relativeValue = (this.Districtid != null) ? this.Districtid.CompareTo(((Customer)obj).Districtid) : -1;
					break;
				case "Jobtypeid":
					relativeValue = this.Jobtypeid.CompareTo(((Customer)obj).Jobtypeid);
					break;
				case "Email":
					relativeValue = (this.Email != null) ? this.Email.CompareTo(((Customer)obj).Email) : -1;
					break;
				case "Tel":
					relativeValue = (this.Tel != null) ? this.Tel.CompareTo(((Customer)obj).Tel) : -1;
					break;
				case "Mobile":
					relativeValue = (this.Mobile != null) ? this.Mobile.CompareTo(((Customer)obj).Mobile) : -1;
					break;
				case "Customertype":
					relativeValue = this.Customertype.CompareTo(((Customer)obj).Customertype);
					break;
				case "Customerdescription":
					relativeValue = (this.Customerdescription != null) ? this.Customerdescription.CompareTo(((Customer)obj).Customerdescription) : -1;
					break;
				case "Precinct":
					relativeValue = (this.Precinct != null) ? this.Precinct.CompareTo(((Customer)obj).Precinct) : -1;
					break;
				case "Priority":
					relativeValue = this.Priority.CompareTo(((Customer)obj).Priority);
					break;
				case "Dealercode":
					relativeValue = this.Dealercode.CompareTo(((Customer)obj).Dealercode);
					break;
				case "Forservice":
					relativeValue = this.Forservice.CompareTo(((Customer)obj).Forservice);
					break;
				default:
					goto case "Id";
			}
			if (Customer.SortDirection == SortDirection.Ascending)
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
