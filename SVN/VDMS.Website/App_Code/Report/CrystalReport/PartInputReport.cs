using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using VDMS.Common.Utils;
using VDMS.II.Linq;
using VDMS.II.Entity;
using VDMS.II.BasicData;
using VDMS.Helper;

namespace VDMS.II.Report
{
    public class PartInputItem
    {
        public DateTime InputDate { get; set; }
        public string Comment { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }

        public int OrderQuantity { get; set; }
        public int Quotation { get; set; }
        public int Good { get; set; }
        public int Lack { get; set; }
        public int Wrong { get; set; }
        public int Broken { get; set; }

        public int UnitPrice { get; set; }
        public decimal Amount { get; set; }

        public PartInputItem()
        {
        }

        public PartInputItem(ReceiveDetail rd)
        {
            this.PartCode = rd.PartCode;
            this.Comment = rd.DealerComment;
            this.InputDate = rd.ReceiveHeader.ReceiveDate;

            this.OrderQuantity = rd.OrderQuantity;
#warning change from Quotation to ShippingQuantity
            this.Quotation = rd.ShippingQuantity;
            this.Good = rd.GoodQuantity;
            this.Broken = rd.BrokenQuantity;
            this.Lack = rd.LackQuantity;
            this.Wrong = rd.WrongQuantity;

            this.Amount = this.UnitPrice * (this.Good + this.Broken + this.Wrong + this.Lack);
        }
    }

    public class PartInputReport
    {
        public static ReportDocument CreateReportDoc(string fromDate, string toDate, string warehouseId)
        {
            string dealer = "", warehouse = "", saleman = "";
            long wId;
            long.TryParse(warehouseId, out wId);
            DateTime dtFrom = DataFormat.DateFromString(fromDate);
            DateTime dtTo = DataFormat.DateFromString(toDate);
            if (dtTo == DateTime.MinValue) dtTo = DateTime.Now;

            ReportDocument rptDoc = ReportFactory.GetReport();
            rptDoc.Load(HttpContext.Current.Server.MapPath(@"~/Part/Report/Crystal/PartInputReport.rpt"));

            IList<PartInputItem> data = GetReportSource(dtFrom, dtTo.AddDays(1).AddSeconds(-1), wId);
            rptDoc.SetDataSource(data);

            Warehouse wh = WarehouseDAO.GetWarehouse(wId);
            if (wh != null)
            {
                warehouse = wh.Address;
                dealer = wh.Dealer.DealerName;
            }
            saleman = string.IsNullOrEmpty(UserHelper.Fullname) ? UserHelper.Username : UserHelper.Fullname;

            rptDoc.SetParameterValue("FromDate", dtFrom.ToShortDateString());
            rptDoc.SetParameterValue("ToDate", dtTo.ToShortDateString());
            rptDoc.SetParameterValue("Dealer", dealer);
            rptDoc.SetParameterValue("Warehouse", warehouse);
            rptDoc.SetParameterValue("SaleMan", saleman);

            return rptDoc;
        }

        public static IList<PartInputItem> GetReportSource(DateTime dtFrom, DateTime dtTo, long warehouseId)
        {
            Warehouse wh = WarehouseDAO.GetWarehouse(warehouseId);
            if (wh == null) return new List<PartInputItem>();

            PartDataContext dc = DCFactory.GetDataContext<PartDataContext>();
            var query = from rd in dc.ReceiveDetails
                        where rd.ReceiveHeader.ReceiveDate >= dtFrom && rd.ReceiveHeader.ReceiveDate <= dtTo
                              && rd.ReceiveHeader.WarehouseId == warehouseId
                        join od in dc.OrderDetails
                        on rd.PartCode equals od.PartCode //into piJoin
                        //from pi in piJoin.DefaultIfEmpty()
                        where rd.OrderHeaderId == od.OrderHeaderId
                        join p in dc.Parts.Where(p => p.DatabaseCode == wh.Dealer.DatabaseCode)
                        on rd.PartCode equals p.PartCode
                        orderby rd.PartCode
                        select new PartInputItem()
                        {
                            PartCode = rd.PartCode,
                            PartName = p.VietnamName,
                            Comment = rd.DealerComment,
                            InputDate = rd.ReceiveHeader.ReceiveDate,

                            OrderQuantity = od.OrderQuantity,
#warning change from Quotation to ShippingQuantity
                            Quotation = rd.ShippingQuantity,
                            Good = rd.GoodQuantity,
                            Broken = rd.BrokenQuantity,
                            Lack = rd.LackQuantity,
                            Wrong = rd.WrongQuantity,

                            UnitPrice = od.UnitPrice,
                            Amount = od.UnitPrice * (rd.GoodQuantity + rd.BrokenQuantity + rd.WrongQuantity + rd.LackQuantity),
                        };

            return query.ToList();
        }
    }
}