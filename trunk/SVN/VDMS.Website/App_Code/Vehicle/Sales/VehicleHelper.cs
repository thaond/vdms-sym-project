using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.I.Entity;
using VDMS.II.Linq;
using VDMS.I.Linq;

namespace VDMS.I.Vehicle
{
    /// <summary>
    /// Summary description for VehicleHelper
    /// </summary>
    public class VehicleHelper
    {
        public VehicleHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string EvalProvince(string provinceName)
        {
            if (!string.IsNullOrEmpty(provinceName))
            {
                var provinces = VDMS.Data.TipTop.Area.GetListProvince();
                for (int i = 0; i < provinces.Tables[0].Rows.Count; i++)
                {
                    if (provinces.Tables[0].Rows[i]["ProviceName"].ToString().ToLower().Contains(provinceName.Trim().ToLower()))
                    {
                        return provinces.Tables[0].Rows[i]["ProviceCode"].ToString();
                    }
                }
            }
            return "N/A (DNF)";

        }

        public static int EvalJobType(string jobType)
        {
            if (!string.IsNullOrEmpty(jobType))
            {
                if (jobType.Trim().Equals(Resources.JobType.Student, StringComparison.CurrentCultureIgnoreCase)) return 1;
                if (jobType.Trim().Equals(Resources.JobType.Office, StringComparison.CurrentCultureIgnoreCase)) return 2;
                if (jobType.Trim().Equals(Resources.JobType.Freelance, StringComparison.CurrentCultureIgnoreCase)) return 3;
                if (jobType.Trim().Equals(Resources.JobType.Soldier, StringComparison.CurrentCultureIgnoreCase)) return 4;
            }
            return 5;
        }

        public static int EvalPaymentMethod(string paymentMethod)
        {
            if (!string.IsNullOrEmpty(paymentMethod))
            {
                if (paymentMethod.Trim().Equals(Resources.PaymentMethod.FixedInstalment, StringComparison.CurrentCultureIgnoreCase)) return (int)CusPaymentType.FixedHire_purchase;
                if (paymentMethod.Trim().Equals(Resources.PaymentMethod.UnfixedInstalment, StringComparison.CurrentCultureIgnoreCase)) return (int)CusPaymentType.UnFixedHire_purchase;
            }
            return (int)CusPaymentType.PayAll;
        }

        public static ItemInstance UpdateItemInstance(VehicleDataContext dc, string engineNo, ItemStatus action, DateTime adjustDate, int price, int paymentType, string toBranch)
        {
            if (action != ItemStatus.Sold && action != ItemStatus.Moved)
                return null;

            var instance = dc.ItemInstances.SingleOrDefault(i => i.EngineNumber == engineNo);
            if (instance != null)
            {
                if (action == ItemStatus.Sold)
                    instance.Status = (int)ItemStatus.Sold;
                instance.ReleasedDate = adjustDate;
                // Save transaction
                ItemInstanceHelper2.SaveTransHis(dc, instance, adjustDate, action, paymentType, price, UserHelper.Username, null, string.Format("{0}-{1}", UserHelper.DealerCode, instance.BranchCode), toBranch);
                // Save inventory day
                InventoryHelper2.SaveInventoryDay(dc, instance.Item.ItemCode, adjustDate, -1, (int)action, UserHelper.DealerCode, instance.BranchCode);

                if (!string.IsNullOrEmpty(toBranch))
                {
                    ItemInstanceHelper2.SaveTransHis(dc, instance, adjustDate, ItemStatus.Imported, paymentType, price, UserHelper.Username, null, string.Format("{0}-{1}", UserHelper.DealerCode, instance.BranchCode), null);
                    InventoryHelper2.SaveInventoryDay(dc, instance.Item.ItemCode, adjustDate, 1, (int)ItemStatus.Imported, UserHelper.DealerCode, toBranch);
                }
            }
            return instance;
        }
    }
}