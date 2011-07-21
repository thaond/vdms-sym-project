using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Expression;
using VDMS.Data.IDAL.Interface;

namespace VDMS.Data.DAL.NHibernateDAL
{
	public abstract class AbstractNHibernateDao<T, IdT> : IDao<T, IdT> where T : IDomainObject<IdT>, new()
	{
		/// <summary>
		/// Loads an instance of type TypeOfListItem from the DB based on its ID.
		/// </summary>
		public T GetById(IdT id, bool shouldLock)
		{
			try
			{
				T entity;

				if (shouldLock)
				{
					entity = (T)NHibernateSession.Load(persitentType, id, LockMode.Upgrade);
				}
				else
				{
					entity = (T)NHibernateSession.Load(persitentType, id);
				}

				return entity;
			}
			catch
			{
				return default(T);
			}
		}

		public IList GetByQuery(string sql, Hashtable listPar)
		{
			IQuery q = NHibernateSession.CreateQuery(sql);
			if (listPar != null)
				foreach (string key in listPar.Keys)
					q.SetParameter(key, listPar[key]);

			return q.List() as IList;
		}

		public List<T> GetBySQLQuery(string sql)
		{
			try
			{
				return NHibernateSession.CreateSQLQuery(sql).List<T>() as List<T>;
			}
			catch
			{
				return new List<T>();
			}
		}

		private Order[] orders = null;
		public void SetOrder(Order[] orders)
		{
			this.orders = orders;
		}

		private ICriterion[] criterions = null;
		public void SetCriteria(ICriterion[] criterions)
		{
			this.criterions = criterions;
		}

		/// <summary>
		/// Loads every instance of the requested type using the 
		/// supplied <see cref="ICriterion" /> and <see cref="Order" />.
		/// </summary>
		public List<T> GetAll()
		{
			ICriteria criteria = NHibernateSession.CreateCriteria(persitentType);

			if (criterions != null)
				foreach (ICriterion criterium in criterions)
					criteria.Add(criterium);

			if (orders != null)
				foreach (Order order in orders)
					criteria.AddOrder(order);

			return criteria.List<T>() as List<T>;
		}

		private int itemCount = 0;
		public int ItemCount
		{
			get
			{
				return itemCount;
			}
		}

		public List<T> GetPaged(int Page, int PageSize)
		{
			ICriteria criteriaList = NHibernateSession.CreateCriteria(persitentType);
			ICriteria criteriaCount = NHibernateSession.CreateCriteria(persitentType);

			if (criterions != null)
				foreach (ICriterion criterium in criterions)
				{
					criteriaList.Add(criterium);
					criteriaCount.Add(criterium);
				}

			if (orders != null)
				foreach (Order order in orders)
				{
					criteriaList.AddOrder(order);
					criteriaCount.AddOrder(order);
				}

			criteriaCount.SetProjection(Projections.RowCount());
			itemCount = (int)criteriaCount.UniqueResult();

			criteriaList.SetFirstResult(Page * PageSize);
			criteriaList.SetMaxResults(PageSize);
			IList<T> ilist = criteriaList.List<T>();
			return ilist as List<T>;
		}

		public int GetCount()
		{
			ICriteria criteriaCount = NHibernateSession.CreateCriteria(persitentType);
			if (criterions != null)
				foreach (ICriterion criterium in criterions)
					criteriaCount.Add(criterium);

			criteriaCount.SetProjection(Projections.RowCount());
			return (int)criteriaCount.UniqueResult();
		}

		public List<T> GetByExample(T exampleInstance, params string[] propertiesToExclude)
		{
			ICriteria criteria = NHibernateSession.CreateCriteria(persitentType);
			Example example = Example.Create(exampleInstance);

			foreach (string propertyToExclude in propertiesToExclude)
			{
				example.ExcludeProperty(propertyToExclude);
			}

			criteria.Add(example);

			return criteria.List<T>() as List<T>;
		}

		/// <summary>
		/// Looks for a single instance using the example provided.
		/// </summary>
		/// <exception cref="NonUniqueResultException" />
		public T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude)
		{
			List<T> foundList = GetByExample(exampleInstance, propertiesToExclude);

			if (foundList.Count > 1)
			{
				throw new NonUniqueResultException(foundList.Count);
			}

			if (foundList.Count > 0)
			{
				return foundList[0];
			}
			else
			{
				return default(T);
			}
		}

		/// <summary>
		/// For entities that have assigned ID's, you must explicitly call Save to add a new one.
		/// See http://www.hibernate.org/hib_docs/reference/en/html/mapping.html#mapping-declaration-id-assigned.
		/// </summary>
		public T Save(T entity)
		{
			NHibernateSession.Save(entity);
			return entity;
		}

		/// <summary>
		/// For entities with automatatically generated IDs, such as identity, SaveOrUpdate may 
		/// be called when saving a new entity.  SaveOrUpdate can also be called to update any 
		/// entity, even if its ID is assigned.
		/// </summary>
		public T SaveOrUpdate(T entity)
		{
			NHibernateSession.SaveOrUpdate(entity);
			return entity;
		}
        public T SaveOrUpdateCopy(T entity)
        {
            NHibernateSession.SaveOrUpdateCopy(entity);
            return entity;
        }
		public void Delete(T entity)
		{
			T existEntity = default(T);
			existEntity = (T)NHibernateSession.Get(persitentType, entity.Id);
			if (existEntity == null) NHibernateSession.Delete(entity);
			else NHibernateSession.Delete(existEntity);
		}

		public void Delete(IdT Id)
		{
			T entity = default(T);
			entity = (T)NHibernateSession.Get(persitentType, Id);
			if (entity == null)
			{
				entity = new T();
				entity.Id = Id;
			}
			NHibernateSession.Delete(entity);
		}

		public void Delete(string query)
		{
			NHibernateSession.Delete(query);
		}

		/// <summary>
		/// Exposes the ISession used within the DAO.
		/// </summary>
		private ISession NHibernateSession
		{
			get
			{
				return NHibernateSessionManager.Instance.GetSession();
			}
		}

		private Type persitentType = typeof(T);
	}
}
