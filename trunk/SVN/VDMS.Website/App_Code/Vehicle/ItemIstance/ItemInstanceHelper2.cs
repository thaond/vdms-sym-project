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
    /// Summary description for ItemInstanceHelper2
    /// </summary>
    public class ItemInstanceHelper2
    {
        private static VehicleDataContext DC { get { return DCFactory.GetDataContext<VehicleDataContext>(); } }

        public ItemInstanceHelper2()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static void SaveTransHis(VehicleDataContext dc, ItemInstance ItemIns, DateTime txtime, ItemStatus TransType, int cusPayType, decimal ActualCost, string Modifiefby, string OlderEngine,string from, string to)
        {
            var th = new SaleTransHistory();

            th.ActualCost = ActualCost;
            th.ReferenceOrderId = 0;
            th.FromBranch = from;
            th.ToBranch = to;
            th.ModifiedDate = DateTime.Now;
            th.ItemInstance = ItemIns;
            th.TransactionDate = txtime;
            th.TransactionType = (int)TransType;
            th.ModifiedBy = Modifiefby;
            th.OldEngineNo = (OlderEngine == null) ? null : OlderEngine;

            dc.SaleTransHistories.InsertOnSubmit(th);
        }
    }
}