using System;
using System.Linq;
using VDMS.II.Entity;
using VDMS.II.Linq;

namespace VDMS.II.PartManagement
{
	public class AccessoryDAO
	{
		public static PartDataContext DC
		{
			get
			{
				return DCFactory.GetDataContext<PartDataContext>();
			}
		}

		public static Accessory GetAccessory(string code, string dealerCode)
		{
			return DC.Accessories.SingleOrDefault(a => a.DealerCode == dealerCode && a.AccessoryCode == code);
		}

		public static Accessory GetAccessory(long accessoryId)
		{
			return DC.Accessories.SingleOrDefault(a => a.AccessoryId == accessoryId);
		}

		public static bool IsAccessoryExist(string code, string dealerCode)
		{
			return AccessoryDAO.GetAccessory(code, dealerCode) != null;
		}

		public static Accessory SaveOrUpdate(string code, string dealerCode, string eName, string vName, string acsType)
		{
			Accessory res = AccessoryDAO.GetAccessory(code, dealerCode);
			if (res == null)
			{
				res = new Accessory() { DealerCode = dealerCode, };
				DC.Accessories.InsertOnSubmit(res);
			}

			res.AccessoryCode = code;
			res.AccessoryTypeCode = acsType;
			res.EnglishName = eName;
			res.VietnamName = vName;

			DC.SubmitChanges();
			return res;
		}

		public static Accessory CreateNew(string code, string ename, string vname, string dealerCode, string type, long catId)
		{
			Accessory acs = null;
			if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(dealerCode) || (catId <= 0)) throw new InvalidOperationException("Wrong paramaters");

			if (!AccessoryDAO.IsAccessoryExist(code, dealerCode))
			{
				acs = new Accessory
				{
					AccessoryCode = code,
					EnglishName = ename,
					VietnamName = vname,
					DealerCode = dealerCode,
					AccessoryTypeCode = type,
				};
				PartInfo pi = new PartInfo
				{
					Accessory = acs,
					DealerCode = dealerCode,
					PartType = "A",
					PartCode = acs.AccessoryCode,
					CategoryId = catId,
				};

				DC.PartInfos.InsertOnSubmit(pi);
				DC.SubmitChanges();
			}
			return acs;
		}
	}
}