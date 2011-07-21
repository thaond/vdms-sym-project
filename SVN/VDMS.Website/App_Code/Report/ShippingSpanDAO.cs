using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using VDMS.Common.Utils;
using VDMS.II.Linq;

namespace VDMS.II.Report
{
    public class OverShippingSpan
    {
        public string DealerCode { get; set; }
        public DateTime? ShippingDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int OverDays { get; set; }
        public int ShippingSpan { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderNumber { get; set; }
        public string IssueNo { get; set; }
        public DateTime OrderDate { get; set; }

    }

    public class ShippingSpanDAO
    {
        int overSpanOrderCount = 0;
        public int CountOverSpanOrder(string dealerCode, string dbCode, string status, string dateFrom, string dateTo)
        {
            return overSpanOrderCount;
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<OverShippingSpan> FindOverSpanOrder(string dealerCode, string dbCode, string status, string dateFrom, string dateTo)
        {
            return FindOverSpanOrder(dealerCode, dbCode, status, dateFrom, dateTo, -1, -1);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<OverShippingSpan> FindOverSpanOrder(string dealerCode, string dbCode, string status, string dateFrom, string dateTo, int startRowIndex, int maximumRows)
        {
            DateTime dtFrom = DataFormat.DateFromString(dateFrom);
            DateTime dtTo = DataFormat.DateFromString(dateTo);
            var ShippingSpan = VDMSSetting.CurrentSetting.ShippingSpan;
            if (dtTo == DateTime.MinValue) dtTo = DateTime.Now;
            if (dbCode == null) dbCode = "";
            if (dealerCode == null) dealerCode = "";

            var db = DCFactory.GetDataContext<PartDataContext>();
            var query = db.OrderHeaders.Where(oh =>
                            oh.DealerCode.Contains(dealerCode)
                            && oh.Dealer.DatabaseCode.Contains(dbCode)
                                // da co $ va` confirmed
                            && oh.PaymentDate != oh.OrderDate
                                //date rank, co the chua xuat hang` nua
                            && oh.ShippingDate >= dtFrom && (oh.ShippingDate <= dtTo || oh.ShippingDate == oh.OrderDate)
                            && (
                                ((oh.ShippingDate != oh.OrderDate) && oh.PaymentDate.Value.AddDays(ShippingSpan) < oh.ShippingDate)
                                ||
                                ((oh.ShippingDate == oh.OrderDate) && oh.PaymentDate.Value.AddDays(ShippingSpan) < DateTime.Now)
                               )
                            );

            overSpanOrderCount = query.Count();
            switch (status)
            {

                case "N": query = query.Where(oh => oh.ReceiveHeaders.Count() == 0);
                    break;
                case "R": query = query.Where(oh => oh.ReceiveHeaders.Count() > 0);
                    break;
                default:
                    break;
            }

            if ((startRowIndex >= 0) && (maximumRows >= 0)) query = query.Skip(startRowIndex).Take(maximumRows);

            return query.Select(oh => new OverShippingSpan()
                {
                    DealerCode = oh.DealerCode,
                    IssueNo = db.ReceiveHeaders.SingleOrDefault(h => h.OrderHeaderId == oh.OrderHeaderId).IssueNumber,
                    OrderNumber = oh.TipTopNumber,
                    PaymentDate = oh.PaymentDate,
                    OrderDate = oh.OrderDate,
                    ShippingDate = oh.ShippingDate,
                    ShippingSpan = ShippingSpan,
                    //OverDays = oh.Dealer.ShippingSpan + ((oh.ShippingDate == oh.OrderDate) ? DateTime.Now.Subtract(oh.PaymentDate.Value).Days : oh.ShippingDate.Value.Subtract(oh.PaymentDate.Value).Days),
                    TotalAmount = oh.OrderDetails.Sum(od => od.UnitPrice * od.QuotationQuantity)
                }
                ).ToList();
        }
    }
}