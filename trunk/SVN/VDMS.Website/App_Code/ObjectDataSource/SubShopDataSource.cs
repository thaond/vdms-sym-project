using System.Collections.Generic;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;
using VDMS.Helper;

namespace VDMS.I.ObjectDataSource
{
	public class SubShopDataSource
	{
		private int count = 0;
		public int SelectCount(int maximumRows, int startRowIndex)
		{
			return count;
		}

		public IList<Subshop> Select(int maximumRows, int startRowIndex)
		{
			IDao<Subshop, long> subShopDao = DaoFactory.GetDao<Subshop, long>();
			subShopDao.SetCriteria(new ICriterion[] { Expression.Eq("Dealercode", UserHelper.DealerCode) });
			subShopDao.SetOrder(new Order[] { Order.Asc("Code") });
			List<Subshop> list = subShopDao.GetPaged(startRowIndex / maximumRows, maximumRows);
			count = subShopDao.ItemCount;
			return list;
		}

		public static void Delete(int Id)
		{
			IDao<Batchinvoiceheader, long> invoiceDao = DaoFactory.GetDao<Batchinvoiceheader, long>();
			Subshop shop = new Subshop();
			shop.Id = Id;
			invoiceDao.SetCriteria(new ICriterion[] { Expression.Eq("Subshop", shop) });
			if (invoiceDao.GetCount() != 0) return;
			IDao<Subshop, long> subShopDao = DaoFactory.GetDao<Subshop, long>();
			subShopDao.Delete(Id);
		}
	}
}