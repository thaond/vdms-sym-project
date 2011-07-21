
namespace VDMS.Data.IDAL.Interface
{
	public interface IDomainObject<IdT>
	{
		IdT Id
		{
			get;
			set;
		}
	}
}
