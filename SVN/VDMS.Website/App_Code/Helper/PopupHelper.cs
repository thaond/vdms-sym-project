using System.Collections.Generic;
using System.Web;
using VDMS.Core.Domain;
using VDMS.I.Entity;
using VDMS.I.Service;
using Customer = VDMS.Core.Domain.Customer;

namespace VDMS.Helper
{
	public class PopupHelper
	{
		public static string SelectEngineSessionString { get { return "SelectEngineSession{0}"; } }
		public static string SelectModelSessionString { get { return "SelectModelSession{0}"; } }
		public static string SelectBrokenSessionString { get { return "SelectBrokenSession{0}"; } }
		public static string SelectCusSessionString { get { return "SelectCusSession{0}"; } }
		public static string SelectPCVSparesSessionString { get { return "SelectPCVSparesSession{0}"; } }
		public static string SelectSRSSparesSessionString { get { return "SelectSRSSparesSession{0}"; } }

		public static ItemInstance GetSelectEngineSession(string key)
		{
			return HttpContext.Current.Session[string.Format(PopupHelper.SelectEngineSessionString, key)] as ItemInstance;
		}
		public static void SetSelectEngineSession(string key, ItemInstance val)
		{
			HttpContext.Current.Session[string.Format(PopupHelper.SelectEngineSessionString, key)] = val;
		}

		public static string GetSelectModelSession(string key)
		{
			return HttpContext.Current.Session[string.Format(PopupHelper.SelectModelSessionString, key)].ToString();
		}
		public static void SetSelectModelSession(string key, string val)
		{
			HttpContext.Current.Session[string.Format(PopupHelper.SelectModelSessionString, key)] = val;
		}

		public static string GetSelectBrokenSession(string key)
		{
			return HttpContext.Current.Session[string.Format(PopupHelper.SelectBrokenSessionString, key)].ToString();
		}
		public static void SetSelectBrokenSession(string key, string val)
		{
			HttpContext.Current.Session[string.Format(PopupHelper.SelectBrokenSessionString, key)] = val;
		}

		public static Customer GetSelectCusSession(string key)
		{
			return HttpContext.Current.Session[string.Format(PopupHelper.SelectCusSessionString, key)] as Customer;
		}
		public static void SetSelectCusSession(string key, Customer val)
		{
			HttpContext.Current.Session[string.Format(PopupHelper.SelectCusSessionString, key)] = val;
		}

		public static List<WarrantySpare> GetSelectPCVSparesSession(string key)
		{
			return HttpContext.Current.Session[string.Format(PopupHelper.SelectPCVSparesSessionString, key)] as List<WarrantySpare>;
		}
		public static void SetSelectPCVSparesSession(string key, List<WarrantySpare> val)
		{
			HttpContext.Current.Session[string.Format(PopupHelper.SelectPCVSparesSessionString, key)] = val;
		}
		public static List<WarrantySpare> GetSelectSRSSparesSession(string key)
		{
			return HttpContext.Current.Session[string.Format(PopupHelper.SelectSRSSparesSessionString, key)] as List<WarrantySpare>;
		}
		public static void SetSelectSRSSparesSession(string key, List<WarrantySpare> val)
		{
			HttpContext.Current.Session[string.Format(PopupHelper.SelectSRSSparesSessionString, key)] = val;
		}
	}
}