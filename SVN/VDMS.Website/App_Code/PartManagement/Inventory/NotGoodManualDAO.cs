using System.ComponentModel;
using System.Linq;
using VDMS.Helper;
using VDMS.I.Linq;
using VDMS.II.Linq;

namespace VDMS.II.PartManagement
{
	[DataObject(true)]
	public class NGManual : PartData
	{
		public long NGFormDetailId { get; set; }
		public string BrokenCode { get; set; }
	}

	public class NotGoodManualDAO : SessionPartDAO<NGManual>
	{
		public static void Init()
		{
			key = "NotGoodManual_List";
		}

		public static void LoadFromDB(long OrderId)
		{
			var list = Parts;
			list.Clear();
			var db = DCFactory.GetDataContext<PartDataContext>();
			var query = from d in db.NGFormDetails
						join p in db.Parts on d.PartCode equals p.PartCode
						where d.NGFormHeaderId == OrderId && p.DatabaseCode == UserHelper.DatabaseCode
						select new { d, p };
			foreach (var item in query)
			{
				list.Add(new NGManual
				{
					PartCode = item.d.PartCode,
					Quantity = item.d.RequestQuantity,
					BrokenCode = item.d.BrokenCode,
					PartName = item.p.EnglishName,
					Comment = item.d.DealerComment
				});
			}
		}
	}

	public class Broken
	{
		[DataObjectMethod(DataObjectMethodType.Select)]
		public IQueryable FindAll()
		{
			var db = DCFactory.GetDataContext<ServiceDataContext>();
			return db.Brokens.OrderBy(p => p.BrokenCode).Select(p => new { p.BrokenCode, p.BrokenName });
		}
	}
}