using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using VDMS.Common.Utils;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using VDMS.II.Linq;

namespace VDMS.II.Report
{
    #region Object

    public class AbnormalReceiveDetail
    {
        public string EnglishName { get; set; }
        public string VietNamName { get; set; }
        public string PartCode { get; set; }
        public string NotGoodNumber { get; set; }
        public long ReceiveDetailId { get; set; }
        public int BrokenQuantity { get; set; }
        public int GoodQuantity { get; set; }
        public int LackQuantity { get; set; }
        public int OrderQuantity { get; set; }
        public int WrongQuantity { get; set; }
        public int Quotation { get; set; }

        public AbnormalReceiveDetail(ReceiveDetail rd, string eng, string vn)
        {
            this.BrokenQuantity = rd.BrokenQuantity;
            this.EnglishName = eng;
            this.GoodQuantity = rd.GoodQuantity;
            this.LackQuantity = rd.LackQuantity;
            this.OrderQuantity = rd.OrderQuantity;
            this.PartCode = rd.PartCode;
#warning change from Quotation to ShippingQuantity
            this.Quotation = rd.ShippingQuantity;
            this.ReceiveDetailId = rd.ReceiveDetailId;
            this.VietNamName = vn;
            this.WrongQuantity = rd.WrongQuantity;
            if (rd.ReceiveHeaderId > 0) NotGoodNumber = rd.ReceiveHeader.NotGoodNumber;
        }
    }
    public class AbnormalOrder
    {
        public decimal OrderId { get; set; }
        public string TipTopNumber { get; set; }
        public decimal Broken { get; set; }
        public decimal Wrong { get; set; }
        public decimal Lack { get; set; }
        public IEnumerable<AbnormalReceiveDetail> ReceiveDetails { get; set; }

        public AbnormalOrder()
        {
        }
    }

    #endregion

    public class AbnormalReceiveDAO
    {

        int orderHasAbnormalReceiveCount = 0;
        public int CountOrderHasAbnormalReceive(string dealerCode, string tiptopNoFrom, string tiptopNoTo, string dateFrom, string dateTo, int approveLevel)
        {
            return orderHasAbnormalReceiveCount;
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<AbnormalOrder> FindOrderHasAbnormalReceive(string dbCode, string dealerCode, string tiptopNoFrom, string tiptopNoTo, string dateFrom, string dateTo, int approveLevel)
        {
            return FindOrderHasAbnormalReceive(-1, -1, dbCode, dealerCode, tiptopNoFrom, tiptopNoTo, dateFrom, dateTo, approveLevel);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<AbnormalOrder> FindOrderHasAbnormalReceive(int maximumRows, int startRowIndex, string dbCode, string dealerCode, string tiptopNoFrom, string tiptopNoTo, string dateFrom, string dateTo, int approveLevel)
        {
            DateTime dtFrom = DataFormat.DateFromString(dateFrom);
            DateTime dtTo = DataFormat.DateFromString(dateTo);
            if (dtTo == DateTime.MinValue) dtTo = DateTime.MaxValue;
            if (tiptopNoFrom == null) tiptopNoFrom = "";
            if (tiptopNoTo == null) tiptopNoTo = "";

            var db = DCFactory.GetDataContext<PartDataContext>();
            var query = db.OrderHeaders
                // base condition
                .Where(oh => oh.TipTopNumber.Contains(tiptopNoFrom)) //&& oh.TipTopNumber.Contains(tiptopNoTo))
                .Where(oh => oh.OrderDate >= dtFrom && oh.OrderDate < dtTo.AddDays(1))
                // xac dinh order da nhan hang, va co do` lom, gioi han theo ApproveLevel
                .Where(oh => oh.ReceiveDetails.Count > 0)
                .Where(oh => oh.ReceiveDetails.Where(rd => rd.BrokenQuantity > 0 || rd.LackQuantity > 0 || rd.WrongQuantity > 0).Count() > 0)
                .Where(oh => oh.ReceiveDetails.Where(rd => rd.ReceiveHeader.NGFormHeaders.Where(ng => ng.ApproveLevel >= approveLevel).Count() > 0).Count() > 0)
                ;

            if (!string.IsNullOrEmpty(dealerCode))
            {
                query = query.Where(oh => oh.DealerCode == dealerCode);
            }
            else
            {
                if (!string.IsNullOrEmpty(dbCode)) query = query.Where(oh => oh.Dealer.DatabaseCode == dbCode);
            }

            orderHasAbnormalReceiveCount = query.Count();
            if ((startRowIndex >= 0) && (maximumRows >= 0)) query = query.Skip(startRowIndex).Take(maximumRows);

            var res = query.Select(o =>
                    new AbnormalOrder()
                    {
                        OrderId = o.OrderHeaderId,
                        TipTopNumber = o.TipTopNumber,
                        Broken = o.ReceiveDetails
                                          .Where(rd => rd.BrokenQuantity > 0 || rd.LackQuantity > 0 || rd.WrongQuantity > 0)
                                          .Where(rd => rd.ReceiveHeader.NGFormHeaders.Where(ng => ng.ApproveLevel >= approveLevel).Count() > 0)
                                          .Sum(rd => rd.BrokenQuantity),
                        Wrong = o.ReceiveDetails
                                          .Where(rd => rd.BrokenQuantity > 0 || rd.LackQuantity > 0 || rd.WrongQuantity > 0)
                                          .Where(rd => rd.ReceiveHeader.NGFormHeaders.Where(ng => ng.ApproveLevel >= approveLevel).Count() > 0)
                                          .Sum(rd => rd.WrongQuantity),
                        Lack = o.ReceiveDetails
                                          .Where(rd => rd.BrokenQuantity > 0 || rd.LackQuantity > 0 || rd.WrongQuantity > 0)
                                          .Where(rd => rd.ReceiveHeader.NGFormHeaders.Where(ng => ng.ApproveLevel >= approveLevel).Count() > 0)
                                          .Sum(rd => rd.LackQuantity),
                        ReceiveDetails = o.ReceiveDetails
                                          .Where(rd => rd.BrokenQuantity > 0 || rd.LackQuantity > 0 || rd.WrongQuantity > 0)
                                          .Where(rd => rd.ReceiveHeader.NGFormHeaders.Where(ng => ng.ApproveLevel >= approveLevel).Count() > 0)
                                          .Join(db.Parts.Where(p => p.DatabaseCode == o.Dealer.DatabaseCode), rd => rd.PartCode, p => p.PartCode,
                                                (rd, p) => new AbnormalReceiveDetail(rd, p.EnglishName, p.VietnamName)),
                    }
                );

            //var test = new List<AbnormalOrder>();
            //var ran = new Random(DateTime.Now.Second);
            //var im = ran.Next(12);

            //for (int i = 0; i < im; i++)
            //{
            //    test.Add(new AbnormalOrder());
            //}
            //orderHasAbnormalReceiveCount = test.Count;
            //return test;
            return res.ToList();
        }
        //[DataObjectMethod(DataObjectMethodType.Select)]
        //public IList<AbnormalOrder> FindOrderHasAbnormalReceive(string dealerCode, string tiptopNoFrom, string tiptopNoTo, string dateFrom, string dateTo)
        //{
        //    DateTime dtFrom = DataFormat.DateFromString(dateFrom);
        //    DateTime dtTo = DataFormat.DateFromString(dateTo);
        //    if (dtTo == DateTime.MinValue) dtTo = DateTime.MaxValue;
        //    if (tiptopNoFrom == null) tiptopNoFrom = "";
        //    if (tiptopNoTo == null) tiptopNoTo = "";

        //    var db = DCFactory.GetDataContext<PartDataContext>();
        //    var query = db.OrderHeaders
        //        // base condition
        //        .Where(p => p.DealerCode == dealerCode
        //         && p.TipTopNumber.Contains(tiptopNoFrom) //&& p.TipTopNumber.Contains(tiptopNoTo)
        //         && p.OrderDate >= dtFrom && p.OrderDate <= dtTo
        //            // xac dinh order da nhan hang, va co do` lom
        //         && p.ReceiveDetails.Count > 0
        //         && p.ReceiveDetails.Where(rd => rd.BrokenQuantity > 0 || rd.LackQuantity > 0 || rd.WrongQuantity > 0).Count() > 0
        //        );
        //    orderHasAbnormalReceiveCount = query.Count();
        //    query.Sum(p => p.ReceiveDetails.Sum(rd => rd.BrokenQuantity));
        //    var res = query.Select(o =>
        //                new AbnormalOrder()
        //                {
        //                    OrderId = o.OrderHeaderId,
        //                    TipTopNumber = o.TipTopNumber,
        //                    Broken = o.ReceiveDetails.Sum(rd => rd.BrokenQuantity),
        //                    Wrong = o.ReceiveDetails.Sum(rd => rd.WrongQuantity),
        //                    Lack = o.ReceiveDetails.Sum(rd => rd.LackQuantity),
        //                    ReceiveDetails = o.ReceiveDetails
        //                }
        //            );
        //    return res.ToList();
        //}

    }
}