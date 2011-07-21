using System;
using VDMS.Core.Domain;

namespace VDMS.I.Vehicle
{
	public enum DataObjectState
	{
		Added,
		Deleted,
		Modified,
		Detached,
		Unchanged
	}

	[Serializable]
	public class OrderDetailEx : Orderdetail
	{
		public OrderDetailEx()
			: base()
		{
			ObjectState = DataObjectState.Unchanged;
		}

		public OrderDetailEx(Orderdetail oBase)
			: base(oBase.Createddate, oBase.Createdby, oBase.Lastediteddate, oBase.Lasteditedby, oBase.Orderqty, oBase.Unitprice, oBase.Unitpricediscount, oBase.Orderpriority, oBase.Item, oBase.Orderheader, oBase.Specialoffer)
		{
			Id = oBase.Id;
			ObjectState = DataObjectState.Unchanged;
		}

		public OrderDetailEx(Orderdetail oBase, DataObjectState objectState)
			: base(oBase.Createddate, oBase.Createdby, oBase.Lastediteddate, oBase.Lasteditedby, oBase.Orderqty, oBase.Unitprice, oBase.Unitpricediscount, oBase.Orderpriority, oBase.Item, oBase.Orderheader, oBase.Specialoffer)
		{
			Id = oBase.Id;
			ObjectState = objectState;
		}

		public string ItemColor
		{
			get
			{
				if (_item == null) return null;
				return base._item.Colorname;
			}
		}

		public string ItemCode
		{
			get
			{
				if (_item == null) return null;
				return base._item.Id;
			}
		}

		public long SubTotal
		{
			get
			{
				return (long)(_unitprice * _orderqty);
			}
		}

		int _index = 0;
		public int Index
		{
			get { return _index; }
			set { _index = value; }
		}

		protected DataObjectState _objectState;
		public DataObjectState ObjectState
		{
			get { return _objectState; }
			set { _objectState = value; }
		}

		public Orderdetail Base
		{
			get
			{
				Orderdetail newOD = new Orderdetail();
				newOD.Id = Id;
				newOD.Createddate = Createddate;
				newOD.Createdby = Createdby;
				newOD.Lastediteddate = Lastediteddate;
				newOD.Lasteditedby = Lasteditedby;
				newOD.Orderqty = Orderqty;
				newOD.Unitprice = Unitprice;
				newOD.Unitpricediscount = Unitpricediscount;
				newOD.Orderpriority = Orderpriority;
				newOD.Item = Item;
				newOD.Orderheader = Orderheader;
				newOD.Specialoffer = Specialoffer;
				return newOD;
			}
		}
	}
}
