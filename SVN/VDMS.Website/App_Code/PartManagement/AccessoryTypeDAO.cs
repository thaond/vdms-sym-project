using System.Linq;
using VDMS.II.Entity;
using VDMS.II.Linq;

namespace VDMS.II.PartManagement
{
	public class AccessoryTypeDAO
	{
		public static PartDataContext DC
		{
			get
			{
				return DCFactory.GetDataContext<PartDataContext>();
			}
		}

		public static void Init()
		{
			if (!AccessoryTypeDAO.IsAccessoryTypeExist("YM")) AccessoryTypeDAO.CreateNew("YM", "Yamaha");
			if (!AccessoryTypeDAO.IsAccessoryTypeExist("HD")) AccessoryTypeDAO.CreateNew("HD", "Honda");
			if (!AccessoryTypeDAO.IsAccessoryTypeExist("CN")) AccessoryTypeDAO.CreateNew("CN", "China");
			if (!AccessoryTypeDAO.IsAccessoryTypeExist("OT")) AccessoryTypeDAO.CreateNew("OT", "Other");
			if (!AccessoryTypeDAO.IsAccessoryTypeExist("SY")) AccessoryTypeDAO.CreateNew("SY", "SYM");
		}

		public static AccessoryType GetAccessory(string code)
		{
			return DC.AccessoryTypes.Single(a => a.AccessoryTypeCode == code);
		}

		public static bool IsAccessoryTypeExist(string code)
		{
			return AccessoryTypeDAO.GetAccessory(code) != null;
		}

		public static AccessoryType CreateNew(string code, string name)
		{
			AccessoryType acs = null;
			if (!AccessoryTypeDAO.IsAccessoryTypeExist(code))
			{
				acs = new AccessoryType()
				{
					AccessoryTypeCode = code,
					AccessoryTypeName = name,
				};
				DC.AccessoryTypes.InsertOnSubmit(acs);
				DC.SubmitChanges();
			}
			return acs;
		}
	}
}