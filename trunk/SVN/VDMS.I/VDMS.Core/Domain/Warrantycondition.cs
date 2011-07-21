using System;

namespace VDMS.Core.Domain
{
	#region Warrantycondition

	/// <summary>
	/// Warrantycondition object for NHibernate mapped table 'DATA_WARRANTYCONDITION'.
	/// </summary>
    [Serializable]
	public class Warrantycondition : DomainObject<decimal>, IComparable
	{
		#region Member Variables

		protected string _partcode;
		protected string _partnamevn;
		protected string _partnameen;
		protected string _motorcode;
		protected long _warrantytime;
		protected decimal _warrantylength;
		protected decimal _labour;
		protected string _manpower;
		protected decimal _unitprice;
		protected static String _sortExpression = "Id";
		protected static SortDirection _sortDirection = SortDirection.Ascending;

		#endregion

		#region Constructors

		public Warrantycondition() { }

		public Warrantycondition(string partcode, string partnamevn, string partnameen, string motorcode, long warrantytime, decimal warrantylength, decimal labour, string manpower, decimal unitprice)
		{
			this._partcode = partcode;
			this._partnamevn = partnamevn;
			this._partnameen = partnameen;
			this._motorcode = motorcode;
			this._warrantytime = warrantytime;
			this._warrantylength = warrantylength;
			this._labour = labour;
			this._manpower = manpower;
			this._unitprice = unitprice;
		}

		#endregion

		#region Public Properties

		public string Partcode
		{
			get { return _partcode; }
			set
			{
				if (value != null && value.Length > 35)
					throw new ArgumentOutOfRangeException("Invalid value for Partcode", value, value.ToString());
				_partcode = value;
			}
		}

		public string Partnamevn
		{
			get { return _partnamevn; }
			set
			{
				if (value != null && value.Length > 512)
					throw new ArgumentOutOfRangeException("Invalid value for Partnamevn", value, value.ToString());
				_partnamevn = value;
			}
		}

		public string Partnameen
		{
			get { return _partnameen; }
			set
			{
				if (value != null && value.Length > 512)
					throw new ArgumentOutOfRangeException("Invalid value for Partnameen", value, value.ToString());
				_partnameen = value;
			}
		}

		public string Motorcode
		{
			get { return _motorcode; }
			set
			{
				if (value != null && value.Length > 12)
					throw new ArgumentOutOfRangeException("Invalid value for Motorcode", value, value.ToString());
				_motorcode = value;
			}
		}

		public long Warrantytime
		{
			get { return _warrantytime; }
			set { _warrantytime = value; }
		}

		public decimal Warrantylength
		{
			get { return _warrantylength; }
			set { _warrantylength = value; }
		}

		public decimal Labour
		{
			get { return _labour; }
			set { _labour = value; }
		}

		public string Manpower
		{
			get { return _manpower; }
			set
			{
				if (value != null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for Manpower", value, value.ToString());
				_manpower = value;
			}
		}

		public decimal Unitprice
		{
			get { return _unitprice; }
			set { _unitprice = value; }
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
			if (!(obj is Warrantycondition))
				throw new InvalidCastException("This object is not of type Warrantycondition");

			int relativeValue;
			switch (SortExpression)
			{
				case "Id":
					relativeValue = this.Id.CompareTo(((Warrantycondition)obj).Id);
					break;
				case "Partcode":
					relativeValue = this.Partcode.CompareTo(((Warrantycondition)obj).Partcode);
					break;
				case "Partnamevn":
					relativeValue = this.Partnamevn.CompareTo(((Warrantycondition)obj).Partnamevn);
					break;
				case "Partnameen":
					relativeValue = this.Partnameen.CompareTo(((Warrantycondition)obj).Partnameen);
					break;
				case "Motorcode":
					relativeValue = (this.Motorcode != null) ? this.Motorcode.CompareTo(((Warrantycondition)obj).Motorcode) : -1;
					break;
				case "Warrantytime":
					relativeValue = this.Warrantytime.CompareTo(((Warrantycondition)obj).Warrantytime);
					break;
				case "Warrantylength":
					relativeValue = this.Warrantylength.CompareTo(((Warrantycondition)obj).Warrantylength);
					break;
				case "Labour":
					relativeValue = this.Labour.CompareTo(((Warrantycondition)obj).Labour);
					break;
				case "Manpower":
					relativeValue = (this.Manpower != null) ? this.Manpower.CompareTo(((Warrantycondition)obj).Manpower) : -1;
					break;
				case "Unitprice":
					relativeValue = this.Unitprice.CompareTo(((Warrantycondition)obj).Unitprice);
					break;
				default:
					goto case "Id";
			}
			if (Warrantycondition.SortDirection == SortDirection.Ascending)
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