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
    /// Summary description for VehicleDAO
    /// </summary>
    /// 
    [DataObject]
    public class VehicleDAO
    {
        static VehicleDataContext DC = DCFactory.GetDataContext<VehicleDataContext>();

        public VehicleDAO()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public object ListSaleSessionalVehicles(string key, int maximumRows, int startRowIndex)
        {
            SaleVehicleHelper.InitSale(key);
            var q = SaleVehicleHelper.Vehicles;
            _countVehicles = q.Count();
            if (maximumRows > 0) q = q.Skip(startRowIndex).Take(maximumRows).ToList();
            return q.ToList();
        }


        int _countVehicles;
        public int CountSaleSessionalVehicles(string key, int maximumRows, int startRowIndex)
        {
            return _countVehicles;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public object ListVehicleTypes(string branch)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            var q = dc.ItemInstances.Where(i => i.DealerCode == UserHelper.DealerCode && i.Status == (int)ItemStatus.Imported);
            if (!string.IsNullOrEmpty(branch)) q = q.Where(i => i.BranchCode == branch);
            var rs = q.Select(i => new { i.Item.ItemType, i.Item.ItemName }).Distinct().ToList();
            rs.Insert(0, new { ItemType = "", ItemName = Resources.Constants.All });
            return rs;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public object ListColors(string branch, string type)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            var q = dc.ItemInstances.Where(i => i.DealerCode == UserHelper.DealerCode && i.Status == (int)ItemStatus.Imported);
            if (!string.IsNullOrEmpty(branch)) q = q.Where(i => i.BranchCode == branch);
            if (!string.IsNullOrEmpty(type)) q = q.Where(i => i.Item.ItemType == type);
            var rs = q.Select(i => new { i.Item.ColorCode, i.Item.ColorName }).Distinct().ToList();
            rs.Insert(0, new { ColorCode = "", ColorName = Resources.Constants.All });
            return rs;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public object FindVehicles(string type, string color, string engineNumber, string warehouseId, int maximumRows, int startRowIndex)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            var query = dc.ItemInstances.Where(i => i.DealerCode == UserHelper.DealerCode &&
                                                    ItemHepler.GetInstockItemStatus().Contains(i.Status))
                                        .Select(i => new
                                                        {
                                                            i.EngineNumber,
                                                            i.BranchCode,
                                                            i.Item.ItemType,
                                                            i.Item.ColorCode,
                                                            i.Item.ItemName,
                                                            i.Item.ColorName
                                                        }).ToList();
            if (!String.IsNullOrEmpty(warehouseId)) query = query.Where(i => i.BranchCode == warehouseId).ToList();
            if (!String.IsNullOrEmpty(type)) query = query.Where(i => i.ItemType == type).ToList();
            if (!string.IsNullOrEmpty(color)) query = query.Where(i => i.ColorCode == color).ToList();
            if (!String.IsNullOrEmpty(engineNumber)) query = query.Where(i => i.EngineNumber.Contains(engineNumber.Trim().ToUpper())).ToList();
            vehiclesCount = query.Count();
            return query.Skip(startRowIndex).Take(maximumRows);
        }

        int vehiclesCount = 0;
        public int CountFoundVehicles(string type, string color, string engineNumber, string warehouseId, int maximumRows, int startRowIndex)
        {
            return vehiclesCount;
        }

        public static ItemInstance GetVehicle(VehicleDataContext dc, string engineNo)
        {
            return dc.ItemInstances.SingleOrDefault(i => i.EngineNumber == engineNo);
        }

        public static bool IsVehicleExisted(string engineNo, string branchCode, ItemStatus status)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            var query = dc.ItemInstances.SingleOrDefault(i => i.EngineNumber == engineNo && i.Status == (int)status);
            if (query != null && (branchCode == null || query.BranchCode == branchCode)) return true;
            return false;
        }
    }
}