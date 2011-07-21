using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using VDMS.WebService.Interface;
using VDMS.II.PartManagement.Order;
using VDMS.WebService.Entity;

namespace VDMS.WebService.Service
{
    // NOTE: If you change the class name "Part" here, you must also update the reference to "Part" in Web.config.
    public class Part : IPart
    {
        OrderDAO _oDao = null;
        public OrderDAO oDao
        {
            get { if (_oDao == null) _oDao = new OrderDAO(); return _oDao; }
        }
        #region IPart Members

        List<ReceiveHeaderInfo> IPart.GetReceiveInfo(long orderId)
        {
            return oDao.GetShipping(orderId);
        }

        public List<OrderInfo> GetOrders(DateTime fromDate, DateTime toDate, string dealerCode, string toDealer, long warehouseId, string orderNumber, string status, string orderType, int maximumRows, int startRowIndex)
        {
            return oDao.FindAll(fromDate, toDate, dealerCode, toDealer, 0, orderNumber, status, orderType, maximumRows, startRowIndex);
        }

        #endregion
    }
}