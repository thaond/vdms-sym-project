using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Resources;
using VDMS.I.Entity;
using VDMS.I.Linq;
using VDMS.I.ObjectDataSource;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;
using VDMS.Data.TipTop;
using VDMS.II.Linq;
using Customer = VDMS.Core.Domain.Customer;

namespace VDMS.I.Service
{
    // NMChi: 
    // 26/11/2007
    public enum ConfirmExchangeErrorCode
    {
        OK,
        InvalidDateTime,
    }
    public enum ServiceListErrorCode
    {
        OK,
        InvalidDateTime,
    }
    public enum ServiceType : int
    {
        UnKnown = 0,
        Repair = 1,
        Warranty = 2,
        Maintain = 5,
        MaintainAndRepair = 6,
        MaintainAndWarranty = 7,
        WarrantyAndRepair = 3,
        RepairAndMaintainAndWarranty = 8
    }
    public enum ServiceStatus : int
    {
        Temp = -1,
        Done = 1,
        UnKnown = 99,
    }
    public class WarrantyInfoStatus
    {
        public static string New = "NEW";
        public static string Approved = "APR";
        public static string OldData = "OLD";
    }
    public enum ExchangeVoucherStatus : int
    {
        AfterSent = -3,         // khong co trong db
        All = -2,               // khong co trong db
        Temp = -1,
        New = 0,                // moi tao chua lam gi
        Sent = 1,               // moi gui len chua xem xet cai nao
        Approved = 2,           // ok (het)
        Reject = 3,             // tra ve (het)
        Verified = 4,           // da tham tra xong (co cai ok co cai tra tra ve - ko co trong db)
        Verifing = 5,           // dang tham tra (dc vai cai, con vai cai chua xong - ko co trong db)
        Canceled = 6,           // dai ly huy ko gui len VMEP nua
        UnKnown = 1000
    }
    public enum UsageRoadType : int
    {
        Good = 0,
        Bad = 1,
        Mountain = 2
    }
    public enum UsageTransportType : int
    {
        Goods = 0,
        Human = 1
    }
    public enum UsageWeatherType : int
    {
        Sunny = 0,
        Rainning = 1
    }
    public enum UsageSpeedType : int
    {
        Galanti = 0,
        Slow = 1,
        Normal = 2,
        High = 3
    }
    public enum VerifyExchangeAction : int
    {
        Approve = 1,
        Reject = 2,
        Update = 3,
        CancelEdit = 4,
        SelectSpare = 5,
        Edit = 6,
        None = 7,
    }

    public class WarrantyInfoDAO
    {
        public static bool ItemInfoExist(string EngineNo)
        {
            return GetWarrantyinfo(EngineNo) != null;
        }

        public static Warrantyinfo GetWarrantyinfo(string engineNo)
        {
            IDao<Warrantyinfo, string> dao = DaoFactory.GetDao<Warrantyinfo, string>();
            Warrantyinfo item = dao.GetById(engineNo, false);
            return item;
        }
    }

    public class ServiceTools
    {
        public static string GetNativeExchangeStatusName(int status)
        {
            switch ((ExchangeVoucherStatus)status)
            {
                case ExchangeVoucherStatus.All: return Constants.All;
                case ExchangeVoucherStatus.New: return ExchangeVoucherStatusString.New;
                case ExchangeVoucherStatus.Sent: return ExchangeVoucherStatusString.Sent;
                case ExchangeVoucherStatus.Verified: return ExchangeVoucherStatusString.Verified;
                case ExchangeVoucherStatus.Approved: return ExchangeVoucherStatusString.Approved;
                case ExchangeVoucherStatus.Reject: return ExchangeVoucherStatusString.Reject;
                case ExchangeVoucherStatus.UnKnown: return ExchangeVoucherStatusString.UnKnown;
                case ExchangeVoucherStatus.Canceled: return ExchangeVoucherStatusString.Canceled;
                default: return "";
            }
        }
        public static string GetNativeVMEPExchangeStatusName(int status)
        {
            switch ((ExchangeVoucherStatus)status)
            {
                case ExchangeVoucherStatus.All: return Constants.All;
                case ExchangeVoucherStatus.New: return ExchangeVoucherStatusString.New;
                case ExchangeVoucherStatus.Sent: return ExchangeVoucherStatusString.NotVerify;
                case ExchangeVoucherStatus.Verified: return ExchangeVoucherStatusString.Verified;
                case ExchangeVoucherStatus.Approved: return ExchangeVoucherStatusString.Approved;
                case ExchangeVoucherStatus.Reject: return ExchangeVoucherStatusString.Reject;
                case ExchangeVoucherStatus.Verifing: return ExchangeVoucherStatusString.Verifing;
                case ExchangeVoucherStatus.Canceled: return ExchangeVoucherStatusString.Canceled;
                case ExchangeVoucherStatus.UnKnown: return ExchangeVoucherStatusString.UnKnown;
                default: return "";
            }
        }
        public static string GetCustAddress(Customer cst)
        {
            if (cst == null) return "";
            string address = cst.Address, province = ProvinceHelper.GetProvinceName(cst.Provinceid);
            address += (string.IsNullOrEmpty(cst.Precinct) ? "" : ((string.IsNullOrEmpty(address)) ? "" : " - ") + cst.Precinct);
            address += (string.IsNullOrEmpty(cst.Districtid) ? "" : ((string.IsNullOrEmpty(address)) ? "" : " - ") + cst.Districtid);
            address += (string.IsNullOrEmpty(province) ? "" : ((string.IsNullOrEmpty(address)) ? "" : " - ") + province);
            return address;
        }
        public static string GetCustTelNo(Customer cst)
        {
            if (cst == null) return "";
            return (string.IsNullOrEmpty(cst.Mobile)) ? cst.Tel : cst.Mobile;
        }

        public static WarrantyInfo GetWarrantyInfo(string engineNumber)
        {
            var db = DCFactory.GetDataContext<ServiceDataContext>();
            return db.WarrantyInfos.FirstOrDefault(p => p.EngineNumber.Equals(engineNumber.Trim()));
        }

        public static bool SaveWarrantyInfo(string engine, int Km, DateTime buyDate, string database, string itemCode, string color, string sellDealer, long custID)
        {
            return SaveWarrantyInfo(engine, Km, buyDate, database, itemCode, color, sellDealer, custID, null);
        }
        public static bool SaveWarrantyInfo(string engine, int Km, DateTime buyDate, string database, string itemCode, string color, string sellDealer, long custID, Customer initCus)
        {
            bool result = false;
            Warrantyinfo warrInfo;
            IDao<Warrantyinfo, string> dao = DaoFactory.GetDao<Warrantyinfo, string>();
            IDao<Customer, long> daoCust;

            try
            {
                warrInfo = dao.GetById(engine, false); //true -> false
                bool newItem = warrInfo == null;
                if (newItem)
                {
                    warrInfo = new Warrantyinfo();
                    warrInfo.Id = engine;
                    warrInfo.Createdate = DateTime.Now;
                    warrInfo.CreateByDealer = VDMS.Helper.UserHelper.DealerCode;
                    warrInfo.Status = WarrantyInfoStatus.New;
                }
                if (Km >= 0) warrInfo.Kmcount = Km;
                if (!DateTime.MinValue.Equals(buyDate)) warrInfo.Purchasedate = buyDate;
                if (!string.IsNullOrEmpty(sellDealer)) warrInfo.Selldealercode = sellDealer;

                // for first time init warrInfo by SRS: copy customer
                if ((warrInfo.Customer == null) && (initCus != null)) warrInfo.Customer = initCus;
                // main customer
                if (custID > 0)
                {
                    daoCust = DaoFactory.GetDao<Customer, long>();
                    Customer cust = daoCust.GetById(custID, false);
                    if (cust != null) warrInfo.Customer = cust;
                }

                if (!string.IsNullOrEmpty(database)) warrInfo.Databasecode = database;
                if (!string.IsNullOrEmpty(color)) warrInfo.Color = color;
                if (!string.IsNullOrEmpty(itemCode)) warrInfo.Itemcode = itemCode;

                if (newItem)
                    dao.Save(warrInfo);
                else
                    dao.SaveOrUpdate(warrInfo);
                result = true;
            }
            catch
            {
            }

            return result;
        }

        public static string GetPartName(Warrantycondition warr)
        {
            return (Thread.CurrentThread.CurrentCulture.Name == "vi-VN") ? warr.Partnamevn : warr.Partnameen;
        }

        public static List<int> GetAfterSentVoucherStatus()
        {
            List<int> result = new List<int>();
            result.Add((int)ExchangeVoucherStatus.Verified);
            result.Add((int)ExchangeVoucherStatus.Reject);
            result.Add((int)ExchangeVoucherStatus.Approved);
            result.Add((int)ExchangeVoucherStatus.Sent);
            return result;
        }
        public static List<int> GetListVoucherStatusForVerify(int status)
        {
            List<int> result = new List<int>();
            ExchangeVoucherStatus voucherStatus = (ExchangeVoucherStatus)status;
            switch (voucherStatus)
            {
                case ExchangeVoucherStatus.Verified:
                    result.Add((int)ExchangeVoucherStatus.Verified);
                    result.Add((int)ExchangeVoucherStatus.Reject);
                    result.Add((int)ExchangeVoucherStatus.Approved);
                    break;
                case ExchangeVoucherStatus.New:
                case ExchangeVoucherStatus.Sent:
                case ExchangeVoucherStatus.Approved:
                case ExchangeVoucherStatus.Reject:
                case ExchangeVoucherStatus.Verifing:
                case ExchangeVoucherStatus.Canceled:
                case ExchangeVoucherStatus.UnKnown:
                    result.Add(status);
                    break;
            }
            return result;
        }

        public static VerifyExchangeErrorCode CanVerifyExchangePartHeaderD(VDMS.I.Entity.ExchangePartHeader exch, ExchangeVoucherStatus status)
        {
            VerifyExchangeErrorCode result = VerifyExchangeErrorCode.OK;
            switch (status)
            {
                case ExchangeVoucherStatus.Reject:
                    result = VerifyExchangeErrorCode.CommentIsBlank; // chang may khong co part nao coi nhu ko dc verify
                    IList<VDMS.I.Entity.ExchangePartDetail> items = exch.ExchangePartDetails.ToList();
                    foreach (VDMS.I.Entity.ExchangePartDetail item in items)
                    {
                        if (!string.IsNullOrEmpty(item.VMEPComment))
                        {
                            result = VerifyExchangeErrorCode.OK;
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
        public static VerifyExchangeErrorCode CanVerifyExchangePartHeader(Exchangepartheader exch, ExchangeVoucherStatus status)
        {
            VerifyExchangeErrorCode result = VerifyExchangeErrorCode.OK;
            switch (status)
            {
                case ExchangeVoucherStatus.Reject:
                    result = VerifyExchangeErrorCode.CommentIsBlank; // chang may khong co part nao coi nhu ko dc verify
                    IList<Exchangepartdetail> items = ExchangePartDetailDataSource.GetByExchangeHeader(exch.Id);
                    foreach (Exchangepartdetail item in items)
                    {
                        if (!string.IsNullOrEmpty(item.Vmepcomment))
                        {
                            result = VerifyExchangeErrorCode.OK;
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}