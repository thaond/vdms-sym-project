
namespace VDMS.Data.IDAL.Interface
{
	public interface IDaoFactory
    {
        IDao<T, IdT> GetDao<T, IdT>() where T : IDomainObject<IdT>, new();
    }
}
