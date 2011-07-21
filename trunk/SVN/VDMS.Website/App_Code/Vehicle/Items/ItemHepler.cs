using System;
using System.Linq;
using Resources;
using VDMS.Core.Domain;
using System.Collections;
using VDMS.Data.IDAL.Interface;
using VDMS.Core.Data;
using NHibernate.Expression;
using System.Collections.Generic;
using VDMS.Helper;
using VDMS.I.Linq;

namespace VDMS.I.Vehicle
{
	public class ItemHepler
	{
		/// <summary>
		/// Get item from ItemInstance table, by Engine Number.
		/// If EngineNumber not found in ItemInstance, or DealerCode in ItemInstance != DealerCode of current user return null
		/// </summary>
		/// <param name="EngineNo"></param>
		/// <returns></returns>
		public static Item GetItemByEngineNo(string EngineNo)
		{
			return null;
		}

		#region NMChi added
        public static Hashtable GetImportedDate(IList<VDMS.I.Entity.ShippingDetail> shippingList)
		{
			Hashtable data = new Hashtable();
			if ((shippingList == null) || shippingList.Count == 0) return data;

			List<string> listEngines = new List<string>();
            foreach (VDMS.I.Entity.ShippingDetail item in shippingList)
			{
				listEngines.Add(item.EngineNumber);
			}

            //IDao<Iteminstance, long> IISdao = DaoFactory.GetDao<Iteminstance, long>();
            //IISdao.SetCriteria(new ICriterion[] { Expression.In("Enginenumber", listEngines) });
            //IList listIIS = IISdao.GetAll();

            //foreach (Iteminstance item in listIIS)
            //{
            //    data.Add(item.Enginenumber, item.Importeddate);
            //}
            using( var db = new VehicleDataContext() )
            {
                var query = from ii in db.ItemInstances
                            where
                                listEngines.Contains(ii.EngineNumber)
                            select ii;
                foreach (var itemInstance in query)
                {
                    data.Add(itemInstance.EngineNumber, itemInstance.ImportedDate);
                }
                return data;
            }
			
		}
		public static ICriterion GetAvailableItemExpression(bool isAvailable)
		{
			if (UserHelper.DatabaseCode.Equals("DNF", StringComparison.OrdinalIgnoreCase))
			{
				return Expression.Eq("Fordnf", isAvailable);
			}
			else
			{
				return Expression.Eq("Forhtf", isAvailable);
			}
		}
		public static void MakeItemAavailable(Item item, bool available)
		{
			if (UserHelper.DatabaseCode.Equals("DNF", StringComparison.OrdinalIgnoreCase))
			{
				item.Fordnf = available;
			}
			else
			{
				item.Forhtf = available;
			}
		}

		public static int[] GetInstockItemStatus()
		{
			return new int[]
			{
				(int) ItemStatus.Imported, 
                (int) ItemStatus.AdmitTemporarily,
				(int) ItemStatus.Moved, //--> changerequest 21/7: chuyen kho tru ton
				(int) ItemStatus.Redundant
			};
		}

		public static bool IsInstock(ItemStatus status)
		{
			switch (status)
			{
				case ItemStatus.Imported:
				case ItemStatus.AdmitTemporarily:
				case ItemStatus.Redundant:
				case ItemStatus.Moved:
					return true;
				default:
					return false;
			}
		}
		public static bool IsInstock(int status)
		{
			return IsInstock((ItemStatus)status);
		}
		// 19/11/2007
		public static string GetNativeItemStatusName(ItemStatus status)
		{
			switch (status)
			{
				case ItemStatus.Imported: return Constants.ItemStatus_Imported;
				case ItemStatus.AdmitTemporarily: return Constants.ItemStatus_AdmitTemporarily;
				case ItemStatus.Lacked: return Constants.ItemStatus_Lacked;
				case ItemStatus.Moved: return Constants.ItemStatus_Moved;
				case ItemStatus.Redundant: return Constants.ItemStatus_Redundant;
				case ItemStatus.Return: return Constants.ItemStatus_Return;
				case ItemStatus.Sold: return Constants.ItemStatus_Sold;
				default: return "";
			}
		}
		public static string GetNativeItemStatusName(int status)
		{
			return GetNativeItemStatusName((ItemStatus)status);
		}

		public static string GetNativeReturnStatusName(ReturnItemStatus status)
		{
			switch (status)
			{
				case ReturnItemStatus.DealerCanceled: return Constants.ReturnStatus_DealerCanceled;
				case ReturnItemStatus.Allowed: return Constants.ReturnStatus_Accept;
				case ReturnItemStatus.NotAllow: return Constants.ReturnStatus_NotAccept;
				case ReturnItemStatus.Proposed: return Constants.ReturnStatus_Proposed;
				case ReturnItemStatus.Returned: return Constants.ReturnStatus_Return;
				default: return "";
			}
		}
		public static string GetNativeReturnStatusName(int status)
		{
			return GetNativeReturnStatusName((ReturnItemStatus)status);
		}
		public static string GetVMEPReturnStatusName(ReturnItemStatus status)
		{
			switch (status)
			{
				case ReturnItemStatus.DealerCanceled: return GetNativeReturnStatusName(ReturnItemStatus.DealerCanceled);
				case ReturnItemStatus.NotAllow: return GetNativeReturnStatusName(ReturnItemStatus.NotAllow);
				case ReturnItemStatus.Allowed:
				case ReturnItemStatus.Returned: return GetNativeReturnStatusName(ReturnItemStatus.Allowed);
				case ReturnItemStatus.Proposed: return Constants.ReturnStatus_Verifying;
				default: return "";
			}
		}
		public static string GetVMEPReturnStatusName(int status)
		{
			return GetVMEPReturnStatusName((ReturnItemStatus)status);
		}

		#endregion

		#region Itemhelper
		/* tntung 
         * 30/07/2007
         */
		public TransHis SaveTranHis(Iteminstance ItemIns, DateTime txtime, ItemStatus TransType, int cusPayType, decimal ActualCost, string Modifiefby, string OlderEngine,string from, string to)
		{
			TransHis tshx = new TransHis();

			tshx.Actualcost = ActualCost;
			tshx.Referenceorderid = 0;
            tshx.Frombranch = from;
            tshx.Tobranch = to;
			tshx.Modifieddate = DateTime.Now;
			tshx.Iteminstance = ItemIns;
			tshx.Transactiondate = txtime;
			tshx.Transactiontype = (int)TransType;
			tshx.Modifiedby = Modifiefby;
			tshx.Oldengineno = (OlderEngine == null) ? null : OlderEngine;
			return tshx;
		}
		#endregion
	}
}