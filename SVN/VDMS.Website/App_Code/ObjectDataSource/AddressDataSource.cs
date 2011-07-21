using System.Collections.Generic;
using System.Data;
using VDMS.Data.TipTop;
using VDMS.Helper;

namespace VDMS.I.ObjectDataSource
{
    public class AddressDataSource
    {
        //public static DataSet GetListAddress()
        public static IList<VDMS.II.Entity.Warehouse> GetListBranches()
        {
            return GetListBranches(Helper.UserHelper.DealerCode);
        }
        public static IList<VDMS.II.Entity.Warehouse> GetListBranches(string DealerCode)
        {
            //return Dealer.GetListBranchOfDealer(DealerCode);
            return VDMS.II.BasicData.WarehouseDAO.GetWarehouses(DealerCode, VDMS.II.Entity.WarehouseType.Vehicle);
        }

        public static DataSet GetListSecondaryAddress()
        {
            return Dealer.GetListSecondaryAddressOfDealer(UserHelper.DealerCode);
        }

        //public static DataSet GetListSecondaryAddress(string DealerCode)
        //{
        //    return Dealer.GetListSecondaryAddressOfDealer(DealerCode);
        //}

        public static string GetAddressByBranchCode(string strBranchCode)
        {
            VDMS.II.Entity.Warehouse w = VDMS.II.BasicData.WarehouseDAO.GetWarehouse(strBranchCode, UserHelper.DealerCode, VDMS.II.Entity.WarehouseType.Vehicle);
            return w == null ? "" : w.Address;
        }
    }
}
