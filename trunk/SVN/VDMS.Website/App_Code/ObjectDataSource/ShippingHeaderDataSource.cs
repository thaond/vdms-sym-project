using System.Collections.Generic;
using System.Linq;
using VDMS.I.Linq;
using VDMS.II.Linq;

namespace VDMS.I.ObjectDataSource
{
    public class ShippingHeaderDataSource
    {
        int _count = 0;
        public int SelectCount(string orderNumber)
        {
            return _count;
            //string dealerCode = UserHelper.DealerCode == "/" ? "%" : UserHelper.DealerCode;
            //return Shipping.CountShippingHeaders(orderNumber, dealerCode);
        }

        //public DataTable Select(string orderNumber, int maximumRows, int startRowIndex)
        //{
        //    string dealerCode = UserHelper.DealerCode == "/" ? "%" : UserHelper.DealerCode;
        //    return Shipping.GetShippingHeaders(orderNumber, dealerCode, startRowIndex, maximumRows);
        //}

        public List<VDMS.I.Entity.IShippingHeader> Select(string orderNumber, int maximumRows, int startRowIndex)
        {
            //string dealerCode = UserHelper.DealerCode == "/" ? "%" : UserHelper.DealerCode;
            //return Shipping.GetShippingHeaders(orderNumber, dealerCode, startRowIndex, maximumRows);
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            var list = dc.IShippingDetails.Where(h => h.TipTopOrderNumber == orderNumber)
                         .GroupBy(d => d.IssueNumber).Join(dc.IShippingHeaders, d => d.Key, h => h.IssueNumber, (d, h) => h);

            _count = list.Count();
            if ((maximumRows > 0) && (startRowIndex >= 0))
                list = list.Skip(startRowIndex).Take(maximumRows);

            return list.ToList();
        }

    }
}