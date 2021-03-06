using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Helper;

namespace VDMS.I.Vehicle
{
	public enum EditOrderActions : int
	{
		Undefined = -1,
		EditNewOrder = 0,
		EditOldOrder = 1,
		FinishEdit = 2,
		ViewOnly = 3,
		ViewWithCommands = 4,
	}

	[Serializable]
	public class ItemOrder : Orderdetail
	{
		public ItemOrder() : base() { }
		public ItemOrder(Orderdetail od, int? index)
			: base(od.Createddate, od.Createdby, od.Lastediteddate, od.Lasteditedby, od.Orderqty, od.Unitprice, od.Unitpricediscount, od.Orderpriority, od.Item, od.Orderheader, od.Specialoffer)
		{
			if (index != null) this.Index = (int)index;
		}
		public ItemOrder(Orderdetail od) : this(od, null) { }

		#region Public Properties

		public int Index { get; set; }

		public string ItemColor
		{
			get
			{
				if (this.Item == null) return null;
				return this.Item.Colorname;
			}
		}

		public long Price
		{
			get { return this.Orderqty * this.Unitprice; }
		}

		public string ItemCode
		{
			get
			{
				if (this.Item == null) return null;
				return this.Item.Id;
			}
		}

		public string ItemDescription
		{
			get
			{
				if (_item == null) return null;
				return string.Format("{1}({0})", _item.Itemname, _item.Id);
			}
		}

		public bool NotOnSale { get; set; }
		public bool OnSale { get { return !this.NotOnSale; } }

		public Orderdetail Base
		{
			get
			{
				return new Orderdetail(this.Createddate, this.Createdby, this.Lastediteddate, this.Lasteditedby, this.Orderqty, this.Unitprice, this.Unitpricediscount, this.Orderpriority, this.Item, this.Orderheader, this.Specialoffer);
			}
		}
		#endregion
	}

	/// <summary>
	/// Last: 30/11/2007 (11:03) dungnt
	/// </summary>
	public class Order
	{
		#region OrderHeader

		/// <summary>
		/// Create new order header
		/// </summary>
		/// <returns></returns>
		public static Orderheader CreateNewOrderHeaderDefault(DateTime orderDate)
		{
			Orderheader res = new Orderheader();
			DateTime dtNow = DateTime.Now;

			res.Createddate = dtNow;
			res.Createdby = UserHelper.Username;
			res.Lastediteddate = dtNow;
			res.Lasteditedby = UserHelper.Username;
			res.Orderdate = orderDate;

			res.Ordertimes = Order.GetOrderNumberByDate(orderDate, UserHelper.DealerCode);
			res.Shippingdate = dtNow;
			res.Status = (int)OrderStatus.Draft;

			res.Dealercode = UserHelper.DealerCode;
			res.Areacode = UserHelper.AreaCode;
			res.Databasecode = UserHelper.DatabaseCode;
			res.Deliveredstatus = (int)DeliveredOrderStatus.NotDeliveredAll;

			return res;
		}

		public static Orderheader SendOrderHearder(Orderheader oh)
		{
			IDao<Orderheader, long> dao = DaoFactory.GetDao<Orderheader, long>();
			oh.Status = (int)OrderStatus.Sent;
			return dao.SaveOrUpdate(oh);
		}

		public static Orderheader DelOrderHeader(Orderheader oh)
		{
			IDao<Orderheader, long> dao = DaoFactory.GetDao<Orderheader, long>();
			oh.Status = (int)OrderStatus.Deleted;
			return dao.SaveOrUpdate(oh);
		}

		public static DateTime FixOrderDate(DateTime orderDate)
		{
			return orderDate.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
		}
		#endregion

		#region OrderDetail

		public static List<ItemOrder> GetOrderItemsByOrderID(long orderId)
		{
			IDao<Orderdetail, long> daood = DaoFactory.GetDao<Orderdetail, long>();
			List<ItemOrder> res = new List<ItemOrder>();
			int i = 0;
			daood.SetCriteria(new ICriterion[] { Expression.Eq("Orderheader.Id", orderId) });
			foreach (Orderdetail item in daood.GetAll())
			{
				i++;
				res.Add(new ItemOrder(item, i));
			}

			return res;
		}

		public static Orderdetail CreateNewOrderDetailDefault()
		{
			Orderdetail res = new Orderdetail();
			res.Createddate = res.Lastediteddate = DateTime.Now;
			res.Createdby = res.Lasteditedby = UserHelper.Username;
			res.Unitpricediscount = 0;
			return res;
		}

		public static void DeleteOrderItems(long orderId, ref IDao<Orderdetail, long> daood)
		{
			daood.SetCriteria(new ICriterion[] { Expression.Eq("Orderheader.Id", orderId) });
			foreach (Orderdetail item in daood.GetAll())
			{
				daood.Delete(item);
			}
		}

		#endregion

		#region Item

		public static bool IsExistItem(string Id)
		{
			return Order.GetItemById(Id) != null;
		}

		public static Item GetItemById(string Id)
		{
			IDao<Item, string> dao = DaoFactory.GetDao<Item, string>();
			return dao.GetById(Id, false);
		}
		#endregion

		#region Helper Utils

		public static string GetOrderStatusString(int os)
		{
			string res = string.Empty;
			switch (os)
			{
				case (int)OrderStatus.Draft:
					res = Resources.OrderStatus.Draft;
					break;
				case (int)OrderStatus.Sent:
					res = Resources.OrderStatus.Sent;
					break;
				case (int)OrderStatus.Confirmed:
					res = Resources.OrderStatus.Confirmed;
					break;
				case (int)OrderStatus.Deleted:
					res = Resources.OrderStatus.Deleted;
					break;
				case (int)OrderStatus.Approved:
					res = Resources.OrderStatus.Approved;
					break;
				default:
					break;
			}
			return res;
		}

		public static int GetOrderNumberByDate(DateTime orderDate)
		{
			return Order.GetOrderNumberByDate(orderDate, UserHelper.DealerCode);
		}

		public static int GetOrderNumberByDate(DateTime dt, string dealerCode)
		{
			ISession sess = NHibernateSessionManager.Instance.GetSession();
			object res = sess.CreateCriteria(typeof(Orderheader)).Add(Expression.Eq("Dealercode", dealerCode))
														   .Add(Expression.Between("Orderdate", dt.Date, dt.Date.AddDays(1).AddSeconds(-1)))
														   .Add(Expression.Not(Expression.Eq("Status", (int)OrderStatus.Deleted)))
					   .SetProjection(Projections.Max("Ordertimes"))
					   .UniqueResult();
			return (res == null) ? 1 : 1 + (int)(decimal)res;
			//IDao<Orderheader, long> dao = DaoFactory.GetDao<Orderheader, long>();
			//dao.SetCriteria(new ICriterion[] { Expression.Eq("Dealercode", UserHelper.DealerCode), Expression.Between("Orderdate", dt.Date, dt.Date.AddDays(1)) });
			//return dao.GetCount() + 1;
		}

		#endregion
	}
}
