using System.Reflection;
using VDMS.Data.IDAL.Interface;

namespace VDMS.Core.Data
{
	public class DaoFactory
	{
		public static IDao<T, IdT> GetDao<T, IdT>() where T : IDomainObject<IdT>, new()
		{
            IDaoFactory DaoFac = (IDaoFactory)Assembly.Load(@"VDMS.Data.DAL").CreateInstance(@"VDMS.Data.DAL.NHibernateDAL.NHibernateDaoFactory");
            return DaoFac.GetDao<T, IdT>();
		}
	}
}
