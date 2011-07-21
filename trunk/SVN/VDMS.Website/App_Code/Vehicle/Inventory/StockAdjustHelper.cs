using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.I.Entity;
using VDMS.II.Linq;
using VDMS.I.Linq;
using Resources;

namespace VDMS.I.Vehicle
{
    public class StockAdjustHelper : SessionVehicleDAO<SaleVehicleInfo>
    {
        public static void AdjustStock(VehicleDataContext dc, SaleVehicleInfo v)
        {
            var instance = dc.ItemInstances.SingleOrDefault(i => i.EngineNumber == v.EngineNumber);
            instance.BranchCode = v.ToBranch;
            //instance.ImportedDate = v.AdjustDate;

            ItemInstanceHelper2.SaveTransHis(dc, instance, v.AdjustDate, ItemStatus.Moved, 0, 0, UserHelper.Username, null, UserHelper.BuildFullBranch(UserHelper.DealerCode, v.FromBranch), UserHelper.BuildFullBranch(UserHelper.DealerCode, v.ToBranch));
            ItemInstanceHelper2.SaveTransHis(dc, instance, v.AdjustDate, ItemStatus.ReceivedFromMoving, 0, 0, UserHelper.Username, null, UserHelper.BuildFullBranch(UserHelper.DealerCode, v.FromBranch), UserHelper.BuildFullBranch(UserHelper.DealerCode, v.ToBranch));

            InventoryHelper2.SaveInventoryDay(dc, instance.Item.ItemCode, v.AdjustDate, -1, (int)ItemStatus.Moved, UserHelper.DealerCode, v.FromBranch);
            InventoryHelper2.SaveInventoryDay(dc, instance.Item.ItemCode, v.AdjustDate, 1, (int)ItemStatus.Imported, UserHelper.DealerCode, v.ToBranch);

        }

        public static string CheckAdjustingVehicle(SaleVehicleInfo v)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();

            if (InventoryHelper.IsInventoryLock(v.AdjustDate, UserHelper.DealerCode, v.FromBranch) ||
                InventoryHelper.IsInventoryLock(v.AdjustDate, UserHelper.DealerCode, v.ToBranch))
                return Message.InventoryLocked;

            if (v.AdjustDate > DateTime.Now)
                return Message.Adjustment_InvalidDate;

            if (string.IsNullOrEmpty(v.FromBranch) || string.IsNullOrEmpty(v.ToBranch))
                return Message.Adjustment_EmptyBranchList;

            if (v.FromBranch == v.ToBranch)
                return Message.Adjustment_EmptyBranchList;

            if (UserHelper.DealerCode == "/")
                return Message.Adjustment_InvalidDealer;

            var instance = dc.ItemInstances.SingleOrDefault(i => i.EngineNumber == v.EngineNumber);

            if (instance == null)
                return Message.Adjustment_InvalidEngineNumber;
            if (instance.DealerCode != UserHelper.DealerCode || instance.BranchCode != v.FromBranch)
                return Message.Adjustment_InvalidBranch;

            if (instance.Status != (int)ItemStatus.Imported)
                return Message.Adjustment_InvalidStatus;

            return Message.ActionSucessful;
        }

        public static string CheckAllAdjustingVehicles()
        {
            if (SaleVehicleHelper.Vehicles.Count == 0)
                return Message.NoVehicle;
            int i = 0;
            foreach (var v in SaleVehicleHelper.Vehicles)
            {
                i++;
                var msg = CheckAdjustingVehicle(v);
                if (msg != Message.ActionSucessful)
                    return string.Format(Message.VehicleError, i, msg);
            }
            return Message.ActionSucessful;
        }

        public static void AdjustAllVehicles()
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();

            foreach (var v in SaleVehicleHelper.Vehicles)
            {
                AdjustStock(dc, v);
            }

            dc.SubmitChanges();
        }

        public static void BindCommonData(string fromBranch, string toBranch, DateTime adjustDate)
        {
            foreach (var v in Vehicles)
            {
                v.FromBranch = fromBranch;
                v.ToBranch = toBranch;
                v.AdjustDate = adjustDate;
            }
        }
    }
}