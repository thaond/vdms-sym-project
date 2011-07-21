using System.Collections.Generic;
using System.Web;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;
using VDMS.I.Vehicle;

/// <summary>
/// Summary description for TransHisDataSource
/// </summary>
namespace VDMS.I.ObjectDataSource
{
	public class TransHisDataSource
	{
		public TransHisDataSource() { }

		private int count = 0;
		public int SelectCount(int maximumRows, int startRowIndex, string EngineNumber)
		{
			return count;
		}
		public List<TransHis> Select(int maximumRows, int startRowIndex, string EngineNumber)
		{
			List<TransHis> res = new List<TransHis>();
			IDao<TransHis, long> dao = DaoFactory.GetDao<TransHis, long>();
			//dao.SetCriteria(con);            
			res = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
			count = dao.ItemCount;
			if (startRowIndex >= count)
			{
				res = dao.GetPaged(0, maximumRows);
				count = dao.ItemCount;
			}
			HttpContext.Current.Items["rowCount"] = count;
			return res;
		}

		public List<TransHis> Select(string EngineNumber)
		{
			List<TransHis> res = new List<TransHis>();

			List<Iteminstance> listII = ItemInstanceHelper.GetIteminstanceByEngNo(EngineNumber, Helper.UserHelper.DealerCode);
			//List<Iteminstance> listII = ItemInstanceHelper.GetIteminstanceByEngNo(EngineNumber);
			if (listII.Count > 0)
			{
				IDao<TransHis, long> dao = DaoFactory.GetDao<TransHis, long>();
				dao.SetCriteria(new ICriterion[] { Expression.In("Iteminstance", listII) });
				dao.SetOrder(new NHibernate.Expression.Order[] { NHibernate.Expression.Order.Desc("Modifieddate") });
				res = dao.GetAll();
			}
			return res;
		}
	}
}