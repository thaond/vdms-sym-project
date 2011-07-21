using VDMS.Data.IDAL.Interface;

namespace VDMS.Data.DAL.NHibernateDAL
{
    public class NHibernateDaoFactory : IDaoFactory
    {
        public IDao<T, IdT> GetDao<T, IdT>() where T : IDomainObject<IdT>, new()
        {
            return new Dao<T, IdT>();
        }
        public class Dao<T, IdT> : AbstractNHibernateDao<T, IdT> where T : IDomainObject<IdT>, new() { }
    }
}
