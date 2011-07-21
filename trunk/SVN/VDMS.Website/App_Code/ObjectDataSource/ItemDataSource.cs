using System.Collections.Generic;
using System.Data;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL2;
using VDMS.Data.IDAL.Interface;
using VDMS.Helper;
using VDMS.I.Vehicle;

namespace VDMS.I.ObjectDataSource
{
	public class ItemDataSource
	{
		/// <summary>
		/// Return list of selling motorbike
		/// </summary>
		/// <returns></returns>
		public List<Item> GetListItem()
		{
			IDao<Item, string> dao = DaoFactory.GetDao<Item, string>();
			dao.SetOrder(new NHibernate.Expression.Order[] { NHibernate.Expression.Order.Asc("Id") });

			dao.SetCriteria(new ICriterion[] { ItemHepler.GetAvailableItemExpression(true), Expression.Like("DatabaseCode", UserHelper.DatabaseCode, MatchMode.Anywhere) });
			return dao.GetAll();
		}

		public DataTable GetListItemType()
		{
			return ItemDao.GetListItemType(UserHelper.DatabaseCode);
		}

		public Item GetOneItem()
		{
			List<Item> list = GetListItem();
			return (Item)list[0];
		}

		public static bool IsExistItem(string Id)
		{
            return IsExistItem(Id, true);
		}

        public static bool IsExistItem(string Id, bool NeedAvailable)
        {
            IDao<Item, string> dao = DaoFactory.GetDao<Item, string>();
            if (NeedAvailable)
            {
                //dao.SetCriteria(new ICriterion[] { Expression.Eq("Id", Id), Expression.Like("Available", true), Expression.Like("DatabaseCode", UserHelper.DatabaseCode, MatchMode.Anywhere) });
                dao.SetCriteria(new ICriterion[] { Expression.Eq("Id", Id), ItemHepler.GetAvailableItemExpression(true), Expression.Like("DatabaseCode", UserHelper.DatabaseCode, MatchMode.Anywhere) });
            }
            else
            {
                dao.SetCriteria(new ICriterion[] { Expression.Eq("Id", Id), Expression.Like("DatabaseCode", UserHelper.DatabaseCode, MatchMode.Anywhere) });
            }
            return dao.GetCount() > 0;
        }
	}
}
