using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;

/// <summary>
/// Summary description for SaleRejectHelper
/// </summary>
namespace VDMS.I.Vehicle
{
	public class SaleRejectHelper
	{
		public SaleRejectHelper() { }

		//public static DataTable GetReturnItemForConfirm(string DearlerCode, string Status)
		//{            
		//    DataTable res = new DataTable();
		//    res = new ReturnItemDao().GetReturnItemForConfirm(DearlerCode, int.Parse(Status)).Tables[0];
		//    //res = new ReturnItemDao().GetReturnItemForConfirm(DearlerCode, (int)ReturnItemStatus.Proposed).Tables[0];
		//    res.Columns.Add("STT", typeof(int));
		//    if (res.Rows.Count > 0)
		//    {
		//        for (int i = 1; i <= res.Rows.Count; i++)
		//        {
		//            res.Rows[i - 1]["STT"] = i;
		//        }
		//    }
		//    return res;                        
		//}
		public static Returnitem UpdateReturnitem(Returnitem rt)
		{
			IDao<Returnitem, long> dao = DaoFactory.GetDao<Returnitem, long>();
			return dao.SaveOrUpdate(rt);
		}
		public static Returnitem GetReturnitemById(long Id)
		{
			IDao<Returnitem, long> dao = DaoFactory.GetDao<Returnitem, long>();
			return dao.GetById(Id, false);
		}
	}
}
