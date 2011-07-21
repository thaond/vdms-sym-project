using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VDMS.WebService.Entity
{
    [DataContract]
    public class ReceiveInfo
    {
        [DataMember]
        public string IssueNumber { get; set; }
        [DataMember]
        public string PartCode { get; set; }
        [DataMember]
        public string EnglishName { get; set; }
        [DataMember]
        public string VietnamName { get; set; }
        [DataMember]
        public int OrderQuantity { get; set; }
        [DataMember]
        public int QuotationQuantity { get; set; }
        [DataMember]
        public int ShippingQuantity { get; set; }
        [DataMember]
        public DateTime ShippingDate { get; set; }
        [DataMember]
        public int GoodQuantity { get; set; }
        [DataMember]
        public int BrokenQuantity { get; set; }
        [DataMember]
        public int WrongQuantity { get; set; }
        [DataMember]
        public int LackQuantity { get; set; }
        [DataMember]
        public int UnitPrice { get; set; }
        [DataMember]
        public string DealerComment { get; set; }
        [DataMember]
        public long ReceiveDetailId { get; set; }
        [DataMember]
        public long ReceiveHeaderId { get; set; }
        [DataMember]
        public int Amount { get; set; }
        [DataMember]
        public int LineNumber { get; set; }
        [DataMember]
        public string NotGoodNumber { get; set; }
    }

    [DataContract]
    public class ReceiveHeaderInfo
    {
        [DataMember]
        public string IssueNumber { get; set; }
        [DataMember]
        public string NotGoodNumber { get; set; }
        [DataMember]
        public DateTime ShippingDate { get; set; }
        [DataMember]
        public long ReceiveHeaderId { get; set; }
        [DataMember]
        public List<ReceiveInfo> Items { get; set; }
    }

    [DataContract]
    public class OrderInfo
    {
        [DataMember]
        public DateTime OrderDate { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string TipTopNumber { get; set; }
        [DataMember]
        public DateTime? ConfirmDate { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public long OrderHeaderId { get; set; }
        [DataMember]
        public string TipTopProcessed { get; set; }
        [DataMember]
        public string OrderSource { get; set; }
    }
}
