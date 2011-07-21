using System;

namespace VDMS.Core.Domain
{
	#region Servicedetail

	/// <summary>
	/// Servicedetail object for NHibernate mapped table 'SER_SERVICEDETAIL'.
	/// </summary>
    [Serializable]
	public class Servicedetail : DomainObject<long>, IComparable
	{
		#region Member Variables

		protected string _partcode;
		protected string _partname;
		protected int _partqty;
		protected long _unitprice;
		protected string _serialnumber;
		protected Serviceheader _serviceheader;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Servicedetail() { }

		public Servicedetail(string partcode, string partname, int partqty, long unitprice, string serialnumber, Serviceheader serviceheader)
		{
			this._partcode = partcode;
			this._partname = partname;
			this._partqty = partqty;
			this._unitprice = unitprice;
			this._serialnumber = serialnumber;
			this._serviceheader = serviceheader;
		}

		#endregion

		#region Public Properties

		public string Partcode
		{
			get { return _partcode; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Partcode", value, value.ToString());
				_partcode = value;
			}
		}

		public string Partname
		{
			get { return _partname; }
			set
			{
				if (value != null && value.Length > 256)
					throw new ArgumentOutOfRangeException("Invalid value for Partname", value, value.ToString());
				_partname = value;
			}
		}

		public int Partqty
		{
			get { return _partqty; }
			set { _partqty = value; }
		}

		public long Unitprice
		{
			get { return _unitprice; }
			set { _unitprice = value; }
		}

		public string Serialnumber
		{
			get { return _serialnumber; }
			set
			{
				if (value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Serialnumber", value, value.ToString());
				_serialnumber = value;
			}
		}

		public Serviceheader Serviceheader
		{
			get { return _serviceheader; }
			set { _serviceheader = value; }
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
			if (!(obj is Servicedetail))
				throw new InvalidCastException("This object is not of type Servicedetail");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Servicedetail)obj).Id);
					break;
				case "Partcode":
					relativeValue = this.Partcode.CompareTo(((Servicedetail)obj).Partcode);
					break;
				case "Partname":
					relativeValue = (this.Partname != null) ? this.Partname.CompareTo(((Servicedetail)obj).Partname) : -1;
					break;
				case "Partqty":
					relativeValue = this.Partqty.CompareTo(((Servicedetail)obj).Partqty);
					break;
				case "Unitprice":
					relativeValue = this.Unitprice.CompareTo(((Servicedetail)obj).Unitprice);
					break;
				case "Serialnumber":
					relativeValue = (this.Serialnumber != null) ? this.Serialnumber.CompareTo(((Servicedetail)obj).Serialnumber) : -1;
					break;
				default:
					goto case "Id";
			}
			if (Servicedetail.SortDirection == SortDirection.Ascending)
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
