using System.Collections;
using System.Collections.Generic;
using NHibernate.Expression;

namespace VDMS.Data.IDAL.Interface
{
	public interface IDao<T, IdT>
	{
		T GetById(IdT id, bool shouldLock);
		List<T> GetAll();

		#region Paging data
		List<T> GetPaged(int Page, int PageSize);
		int ItemCount { get; }
		#endregion

		List<T> GetBySQLQuery(string sql);
		IList GetByQuery(string sql, Hashtable listPar);

		#region Order and Criteria
		void SetOrder(Order[] orders);
		void SetCriteria(ICriterion[] criterions);
		#endregion

		//List<T> GetByExample(T exampleInstance, params string[] propertiesToExclude);
		//T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude);
		int GetCount();


		#region Insert, Delete, Update

		T Save(T entity);
		T SaveOrUpdate(T entity);
        T SaveOrUpdateCopy(T entity);
		void Delete(T entity);
		void Delete(IdT Id);
		void Delete(string query);

		#endregion

		//void CommitChanges();
	}
}
