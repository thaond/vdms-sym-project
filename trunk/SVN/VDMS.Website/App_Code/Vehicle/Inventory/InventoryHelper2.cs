using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.I.Entity;
using VDMS.II.Linq;
using VDMS.I.Linq;

namespace VDMS.I.Vehicle
{
    /// <summary>
    /// Summary description for InventoryHelper2
    /// </summary>
    public class InventoryHelper2
    {
        private static VehicleDataContext DC { get { return DCFactory.GetDataContext<VehicleDataContext>(); } }

        public InventoryHelper2()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static void SaveInventoryDay(VehicleDataContext dc, string itemCode, DateTime actionTime, int quantity, int actionType, string dealerCode, string branchCode)
        {
            long actionDay = long.Parse(actionTime.ToString("yyyyMMdd"));
            var inv = dc.SaleInventoryDays.SingleOrDefault(iv => iv.ItemCode == itemCode &&
                                                                 iv.ActionDay == actionDay &&
                                                                 iv.ActionType== actionType &&
                                                                 iv.BranchCode == branchCode &&
                                                                 iv.DealerCode == dealerCode);
            if (inv == null)
            {
                inv = new SaleInventoryDay
                      {
                          ActionDay = actionDay,
                          ActionType = actionType,
                          BranchCode = branchCode,
                          DealerCode = dealerCode,
                          ItemCode = itemCode,
                          Quantity = 0,
                      };
                dc.SaleInventoryDays.InsertOnSubmit(inv);
            }

            inv.Quantity += quantity;
        }
    }
}