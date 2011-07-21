using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using VDMS.WebService.Entity;
using System.Net.Security;
using System.Data;
using System.Collections.Specialized;

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
        decimal RunScalarCommand(string cmd);

        [OperationContract]
        ServerItem GetItem(string item);

        [OperationContract]
        List<FileFolderInfo> ListDir(string dir);
        [OperationContract]
        void CopyTo(string sFile, string dFile);
        [OperationContract]
        void ReName(string file, string newName);
        [OperationContract]
        void Delete(string file);

        [OperationContract]
        string CallForString(string c, string cl, string mt, string[] refs, string[] param, bool isStatic, out string[] output);
        [OperationContract]
        DataTable CallForTable(string c, string cl, string mt, string[] refs, string[] param, bool isStatic, out string[] output);
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
