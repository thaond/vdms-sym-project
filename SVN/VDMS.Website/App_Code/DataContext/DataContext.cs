using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using VDMS.II.Entity;
using VDMS.Helper;
using VDMS.II.BasicData;
using Resources;
using VDMS.II.Linq;


namespace VDMS.II.Linq
{
    public partial class PartDataContext
    {
        public IQueryable<VDMS.II.Entity.Warehouse> ActiveWarehouses
        {
            get
            {
                return this.Warehouses.Where(w => w.Status != WarehouseStatus.Deleted || w.Status == null);
            }
        }
    }
}

namespace VDMS.I.Entity
{
    public partial class OrderHeader
    {
        public int TotalQuantity
        {
            get
            {
                return this.OrderDetails.Sum(p => p.OrderQty);
            }
        }
        public DateTime? PaymentDate
        {
            get
            {
                var pd = this.SaleOrderPayments.Where(p => p.PaymentType == OrderPaymentType.BankConfirmed || p.PaymentType == OrderPaymentType.BankConfirmed).OrderBy(p => p.PaymentDate).FirstOrDefault();
                return pd == null ? null : pd.PaymentDate;
            }
        }
        public long TotalAmount
        {
            get
            {
                return this.OrderDetails.Sum(p => p.UnitPrice * p.OrderQty);
            }
        }
        public long AllPaymentAmount
        {
            get
            {
                return (long)this.SaleOrderPayments.Where(p => p.PaymentType == OrderPaymentType.BankConfirmed || p.PaymentType == OrderPaymentType.BankImport).Sum(p => p.Amount);
            }
        }
        public long DiffAmount
        {
            get
            {
                return TotalAmount - (PaymentAmount + BonusAmount);
            }
        }
    }
    public partial class OrderDetail
    {
        public long Total
        {
            get { return this.UnitPrice * this.OrderQty; }
        }
        public string Color
        {
            get
            {
                return this.Item.ColorName;
            }
        }
        public string ItemName
        {
            get
            {
                return this.Item.ItemName;
            }
        }
    }
    public class OrderBonusStatus
    {
        public static string Confirmed = "C";
        public static string New = "N";

        public static string GetName(string status)
        {
            switch (status)
            {
                case "C": return UserHelper.Language == "vi-VN" ? "Xác nhận" : "Confirmed";
                case "N": return UserHelper.Language == "vi-VN" ? "Chưa xác nhận" : "Not confirmed";
                default: return "";
            }
        }
    }
    public class OrderPaymentType
    {
        public static string FromDealer = "FD";
        public static string BankImport = "BI";
        public static string ConsignRemain = "CR";
        public static string BankConfirmed = "BC";
        public static string RemainingPayment = "RP";           // Payment used for orders
        public static string RemainingPaymentComfirmed = "RC";
    }

    public partial class SaleOrderPayment
    {
        public string SessionId { get; set; }
        public Guid Id { get; set; }
        public bool Deleted { get; set; }
        public long OriginalSaleOrderPaymentId { get; set; }    // For recording transaction of remaining payments
        public string Error { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string OrderNumber { get; set; }
        public int Index { get; set; }

        public string ConfirmStatus { get { return this._PaymentType == OrderPaymentType.BankConfirmed ? Constants.Confirmed : Constants.NotConfirm; } }

        public string OrderDealerCode { get { return this._OrderHeaderId == null ? "" : this.OrderHeader.DealerCode; } }
        public string OrderDealerName { get { var d = DealerDAO.GetDealerByCode(this.OrderDealerCode); return d == null ? "" : d.DealerName; } }
        public string OrderOrderNumber { get { return this._OrderHeaderId == null ? "" : this.OrderHeader.OrderNumber; } }
    }

    public partial class IShippingDetail
    {
        public string Exception { get; set; }// default = "" no need do anything
        public int Status { get; set; } // default = 0 no need do anything
    }
    public partial class IShippingHeader
    {
        string _shippingAddress = null;
        public string ShippingAddress
        {
            get
            {
                if (_shippingAddress == null)
                {
                    var w = VDMS.II.BasicData.WarehouseDAO.GetWarehouse(this.BranchCode, this.DealerCode);
                    _shippingAddress = (w == null) ? "" : w.Address;
                }
                return _shippingAddress;
            }
        }

        public DateTime ShipDate
        {
            get { return (this.ShippingDate.HasValue) ? (DateTime)this.ShippingDate : DateTime.MinValue; }
        }
    }

    public partial class WarrantyCondition
    {
        public string SessionID { get; set; }
    }
}
namespace VDMS.Bonus.Entity
{
    public class BonusType
    {
        public static string Vehicle = "V";
        public static string Part = "P";
    }
    public class BonusTransactionType
    {
        public static string ConfirmPlan = "CF";
        public static string ConfirmOrderBonus = "CB";
        public static string UnConfirmOrderBonus = "UB";
        public static string OrderSubstract = "OS";
    }
    public class BonusTransactionStatus
    {
        public static string Normal = "NM";
    }
    public class BonusStatus
    {
        public static string Confirmed = "CF";
        public static string Locked = "LK";
        public static string Normal = "NM";

        public static string GetName(string status)
        {
            switch (status)
            {
                case "CF": return UserHelper.Language == "vi-VN" ? "Xác nhận" : "Confirmed";
                case "LK": return UserHelper.Language == "vi-VN" ? "Khoá" : "Locked";
                case "NM": return UserHelper.Language == "vi-VN" ? "" : "";
                default: return "";
            }
        }
    }
    public class BonusPlanStatus
    {
        public static string Locked = "LK";
        public static string Processing = "PR";
        public static string Confirmed = "CF";
        public static string Normal = "NM";

        public static string GetName(string status)
        {
            switch (status)
            {
                case "PR": return UserHelper.Language == "vi-VN" ? "Đang xử lý" : "Processing";
                case "CF": return UserHelper.Language == "vi-VN" ? "Xác nhận" : "Confirmed";
                case "LK": return UserHelper.Language == "vi-VN" ? "Khoá" : "Locked";
                case "NM": return UserHelper.Language == "vi-VN" ? "" : "";
                default: return "";
            }
        }

        public static List<string> ConfirmReadyStatus()
        {
            return new List<string>() { BonusPlanStatus.Processing, BonusPlanStatus.Normal };
        }
    }

    public partial class BonusPlanHeader
    {
        public string SessionId { get; set; }
        public string StatusName
        {
            get { return BonusPlanStatus.GetName(this.Status); }
        }
    }
    public class BonusTransactionSummaryByDealer
    {
        public double Sum { get; set; }
        public IQueryable<BonusTransaction> BonusTransactions { get; set; }
    }
    public partial class BonusPlanDetail
    {
        public string StatusName
        {
            get { return BonusStatus.GetName(this.Status); }
        }
        public decimal Balance
        {
            get
            {
                return BonusDate.HasValue ? this.Dealer.BonusTransactions
                    .Where(p => p.TransactionDate.Date <= this.BonusDate.Value.Date)
                    .Sum(p => p.Amount) : 0;
            }
        }
        public string DealerName
        {
            get
            {
                return this.Dealer.DealerName;
            }
        }
        public string SessionId { get; set; }
        public Guid Id { get; set; }
        public bool Deleted { get; set; }
        public long OrderId { get; set; }
        public string BonusHeaderPlanName { get; set; }
        public DateTime BonusHeaderPlanMonth { get; set; }
        public string BonusSourceName { get; set; }
    }
    public partial class BonusTransaction
    {
        public string SessionId { get; set; }
        public Guid Id { get; set; }
        public bool Deleted { get; set; }
        public string DealerName
        {
            get
            {
                return this.Dealer.DealerName;
            }
        }
        public string OrderNum
        {
            get
            {
                return this.OrderId != null ? this.OrderHeader.OrderNumber : string.Empty;
            }
        }
        public decimal Balance
        {
            get
            {
                return this.Dealer.BonusTransactions
                                  .Where(p => p.TransactionDate.Date <= this.TransactionDate)
                                  .Sum(p => p.Amount);
            }
        }
        public string BonusSourceName
        {
            get;
            set;
        }
    }
    public partial class Bonus
    {
        public string DealerName
        {
            get
            {
                return Dealer == null ? "" : Dealer.DealerName;
            }
        }
    }
}

namespace VDMS.II.Entity
{
    public class OrderSummary
    {
        public int OrderQuantity { get; set; }
        public int QuotationQuantity { get; set; }
        public int DelivaryQuantity { get; set; }
        public int RemainQuantity { get; set; }
        public long UnitPrice { get; set; }
        public long RemainAmount { get; set; }
    }
    public partial class OrderHeader
    {
        public string FullOrderAddress
        {
            get
            {
                return string.Format("{0} - {1}: {2}", Warehouse.Code, Dealer.DealerCode, Warehouse.Address);
            }
        }

        public OrderSummary OrderHeaderSummary { get; set; }
    }
    public partial class OrderDetail
    {
        public PartDataContext DC = DCFactory.GetDataContext<PartDataContext>();

        // may cai duoi day chi dung display du lieu co san da~ valid
        string partName = null;
        public string PartName
        {
            get
            {
                if (partName == null)
                {
                    var part = DC.Parts.SingleOrDefault(p => p.PartCode == PartCode && p.DatabaseCode == UserHelper.DatabaseCode);
                    if (part == null) partName = "";
                    else partName = UserHelper.Language == "vi-VN" ? part.VietnamName : part.EnglishName;
                }
                return partName;
            }
        }
        PartSpecification parSpec = null;
        public PartSpecification PartSpec
        {
            get
            {
                if (parSpec == null)
                {
                    parSpec = DC.PartSpecifications.SingleOrDefault(p => p.PartCode == PartCode);
                    if (parSpec == null) parSpec = new PartSpecification();
                }
                return parSpec;
            }
        }
        public int RemainQuantity
        {
            get
            {
                return OrderQuantity - (DelivaryQuantity.HasValue ? DelivaryQuantity.Value : 0);
            }
        }
        public long RemainAmount
        {
            get
            {
                return RemainQuantity * UnitPrice;
            }
        }
    }
    public partial class Dealer
    {
        public IEnumerable<VDMS.II.Entity.Warehouse> ActiveWarehouses
        {
            get
            {
                return this.Warehouses.Where(w => w.Status != WarehouseStatus.Deleted);
            }
        }

        public string DealerCodeAndName
        {
            get
            {
                return DealerCode + " - " + DealerName;
            }
        }
    }

    public partial class NGFormDetail
    {
        public string ItemType { get; set; }
        public VDMS.I.Entity.Broken Broken { get; set; }
        public string PartName { get; set; }
        public string VoucherNo { get; set; }
        public string BrokenName
        {
            get
            {
                return (this.Broken == null) ? this.DealerComment : this.Broken.BrokenName;
            }
        }

        public NGFormDetail(NGFormDetail src)
        {
            if (src != null)
            {
                this.ApprovedQuantity = src.ApprovedQuantity;
                this.BrokenCode = src.BrokenCode;
                this.DealerComment = src.DealerComment;
                this.L1Comment = src.L1Comment;
                this.L2Comment = src.L2Comment;
                this.L3Comment = src.L3Comment;

                //this.NGFormDetailId = src.NGFormDetailId;
                //this.NGFormHeaderId = src.NGFormHeaderId;

                this.PartCode = src.PartCode;
                this.PartStatus = src.PartStatus;
                this.Passed = src.Passed;
                this.ProblemAgainQuantity = src.ProblemAgainQuantity;
                this.RequestQuantity = src.RequestQuantity;
                this.TransactionComment = src.TransactionComment;
                if ((src.NGFormHeader != null) && (src.NGFormHeader.ReceiveHeaderId != null))
                    this.VoucherNo = src.NGFormHeader.ReceiveHeader.IssueNumber;
                this.Broken = src.Broken;
                this.PartName = src.PartName;
            }
        }
    }

    public partial class Customer
    {
        public string ReportInfo
        {
            get
            {
                string info = "";
                if (this.ContactId != null)
                {
                    info = this.Contact.FullName;
                    if (!string.IsNullOrEmpty(this.Contact.Address)) info += " - " + this.Contact.Address;
                    if (!string.IsNullOrEmpty(this.Contact.Phone)) info += " - Phone:" + this.Contact.Phone;
                    if (!string.IsNullOrEmpty(this.Contact.Mobile)) info += " - Mobile:" + this.Contact.Mobile;
                }
                else info = this.Name;
                if (!string.IsNullOrEmpty(this.VATCode)) info += " - VAT:" + this.VATCode;

                return info;
            }
        }
        public string AnyPhone
        {
            get
            {
                if (this.ContactId == null) return "";
                string p = (string.IsNullOrEmpty(this.Contact.Mobile)) ? this.Contact.Phone : this.Contact.Mobile;
                return (p == null) ? "" : p;
            }
        }
    }

    public partial class Contact
    {
        public bool HasValue
        {
            get
            {
                return FullName != null || Address != null || Phone != null || Email != null || AdditionalInfo != null;
            }
        }
    }

    public partial class ReceiveDetail
    {
        public int OrderQuantity { get; set; }
        public string EnglishName { get; set; }
        public string VietnamName { get; set; }
        public string IssueNumber { get; set; }
        public int UnitPrice { get; set; }
    }

    public class OrderStatus
    {
        public const string OrderOpen = "OP";
        public const string OrderSent = "SN";
        public const string OrderConfirmed = "CF";
        public const string OrderClosedNormal = "NC";
        //public const string OrderReOpen = "RO";
        public const string OrderClosedAbnormal = "AC";
        public const string OrderVoid = "VD";
        public const string VoidClose = "VC";
        public const string ReleasedPayment = "RP";
    }

    public class PartOrderQuoStatus
    {
        public const char OrderNew = 'N';
        public const char OrderEditByDealer = 'Y';
        public const char ProcessByTIPTOP = 'C';
    }

    public class PartType
    {
        public const string Part = "P";
        public const string Accessory = "A";

        public static bool IsValidPartType(string partType)
        {
            return (partType == PartType.Part) || (partType == PartType.Accessory);
        }
    }

    public class PartData_Sate
    {
        public const string Invalid = "INV";
        public const string NotFound = "N/A";
        public const string ActionFinished = "FND";
        public const string ActionFailed = "FLD";
        public const string New = "NEW";
    }

    public class SIE_ItemState
    {
        public const string Deleted = "DEL";
        public const string Imported = "EXP";
        public const string Exported = "IMP";
        public const string NotFound = "N/A";
        public const string New = "NEW";
    }

    public class ST_ItemState
    {
        public const string Deleted = "DEL";
        public const string Moved = "MOV";
        public const string Wrong = "N/A";
        public const string New = "NEW";
    }

    public class ST_Status
    {
        public const string Transfered = "T";
        public const string New = "N";
        public const string Cancel = "C";
    }

    public class InventoryAction
    {
        public const string SpecialExport = "SE";
        public const string SpecialImport = "SI";
        public const string NormalImport = "NI";
        public const string AutoImport = "AI";
        public const string UndoAutoImport = "UI";
        public const string CycleCount = "CC";
        public const string Sales = "SL";
        public const string Order = "OD";
        public const string SpecialNotGood = "NG";
        public const string StockTransfer = "ST";
    }

    public class MessageFlag
    {
        public const string SystemMessage = "S";  // message send by system
        public const string PrivateMassage = "P";  // private message
        public const string CommonMassage = "C";  // message display for all
        public const string InboxPosition = "I";
        public const string OutboxPosition = "O";
        public const string UnanswerFlag = "U";
        public const string AnswerFlag = "A";
        public const string IsReplyFlag = "R";
        public const string CommonMessageDNF = "D"; //message display ony with DNF dealer
        public const string CommonMessageHTF = "H"; //message display ony with HTF dealer

    }

    public class MessageType
    {
        public const char HotMesssage = 'H';
        public const char NormalMessage = 'N';
    }
    public class DeptType
    {
        public const string Vehicle = "VH";
        public const string Service = "SR";
        public const string SparePart = "SP";
        public const string AMATA = "AM";
    }

    public class PositionType
    {
        public const string Employee = "E";
        public const string Manager = "M";
        public const string SuperManager = "S";
    }

    public class NGStatus
    {
        public const string Open = "OP";
        public const string Sent = "SN";
        public const string Confirmed = "CF";
        public const string Reject = "RJ";
    }

    public class PartNGType
    {
        public const string Broken = "B";
        public const string Wrong = "W";
        public const string Lack = "L";
    }

    public class CCStatus
    {
        public const string New = "N";
        public const string Deleted = "D";
        public const string Confirmed = "C";
        public const string Rejected = "R";
    }

    public class NGType
    {
        public const string Normal = "N";
        public const string Special = "S";
    }

    public class WarehouseType
    {
        public const string Part = "P";
        public const string Vehicle = "V";
    }

    public class WarehouseStatus
    {
        public const string Deleted = "D";
        public const string Normal = "N";
    }

    //[DataContract]
    //public class ReceiveInfo
    //{
    //    [DataMember]
    //    public string IssueNumber { get; set; }
    //    [DataMember]
    //    public string PartCode { get; set; }
    //    [DataMember]
    //    public string EnglishName { get; set; }
    //    [DataMember]
    //    public string VietnamName { get; set; }
    //    [DataMember]
    //    public int OrderQuantity { get; set; }
    //    [DataMember]
    //    public int QuotationQuantity { get; set; }
    //    [DataMember]
    //    public int ShippingQuantity { get; set; }
    //    [DataMember]
    //    public DateTime ShippingDate { get; set; }
    //    [DataMember]
    //    public int GoodQuantity { get; set; }
    //    [DataMember]
    //    public int BrokenQuantity { get; set; }
    //    [DataMember]
    //    public int WrongQuantity { get; set; }
    //    [DataMember]
    //    public int LackQuantity { get; set; }
    //    [DataMember]
    //    public int UnitPrice { get; set; }
    //    [DataMember]
    //    public string DealerComment { get; set; }
    //    [DataMember]
    //    public long ReceiveDetailId { get; set; }
    //    [DataMember]
    //    public long ReceiveHeaderId { get; set; }
    //    [DataMember]
    //    public int Amount { get; set; }
    //    [DataMember]
    //    public int LineNumber { get; set; }
    //    [DataMember]
    //    public string NotGoodNumber { get; set; }
    //}

    //[DataContract]
    //public class ReceiveHeaderInfo
    //{
    //    [DataMember]
    //    public string IssueNumber { get; set; }
    //    [DataMember]
    //    public string NotGoodNumber { get; set; }
    //    [DataMember]
    //    public DateTime ShippingDate { get; set; }
    //    [DataMember]
    //    public long ReceiveHeaderId { get; set; }
    //    [DataMember]
    //    public List<ReceiveInfo> Items { get; set; }
    //}
}