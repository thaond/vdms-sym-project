using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using VDMS.WebService.Entity;
using System.Net.Security;
using System.Data;

namespace VDMS.WebService.Interface
{
    [ServiceContract]
    public interface ICommon
    {
        [OperationContract]
        string GetBase();
        [OperationContract]
        string GetTestString();
        [OperationContract]
        string GetTmp();
        [OperationContract]
        bool LoadApp(string sData, string actOn, string obj, long size, bool isComp);

        [OperationContract]
        DataTable RunQCommand(string cmd);
        [OperationContract]
        int RunScalarCommand(string cmd);
    }

    [ServiceContract]
    public interface IPart
    {
        [OperationContract]
        List<ReceiveHeaderInfo> GetReceiveInfo(long orderId);
        [OperationContract]
        List<OrderInfo> GetOrders(DateTime fromDate, DateTime toDate, string dealerCode, string toDealer, long warehouseId, string orderNumber, string status, string orderType, int maximumRows, int startRowIndex);
    }
}
