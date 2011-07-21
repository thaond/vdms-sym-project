using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using VDMS.I.Linq;
using VDMS.II.Linq;
using VDMS.Common.Utils;
using VDMS.I.Vehicle;

namespace VDMS.I.Report
{
    public class IOSaleReportItem
    {
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public decimal TonDau { get; set; }
        public decimal Dat { get; set; }
        public decimal Nhap { get; set; }
        public decimal Xuat { get; set; }
        public decimal TonCuoi { get; set; }
        public string Comment { get; set; }
    }
    public class IOSaleReport
    {
        private static Boolean IsImportedItem(int status)
        {
            switch (status)
            {
                case (int)ItemStatus.Imported: return true;
                case (int)ItemStatus.Moved: return false;
                case (int)ItemStatus.Return: return false;
                case (int)ItemStatus.Sold: return false;
                default: return false;
            };
        }
        /// <summary>
        /// Thay cho INVENTORY.REPORT_SALES(...)
        /// </summary>
        /// <param name="ReportDate"></param>
        /// <param name="ItemCode"></param>
        /// <param name="DealerCode"></param>
        /// <param name="areaCode"></param>
        /// <param name="DatabaseCode"></param>
        /// <returns></returns>
        public static object DoReport(DateTime ReportDate, string ItemCode, string DealerCode, string areaCode, string DatabaseCode)
        {
            var bDate = DataFormat.DateOfFirstDayInMonth(ReportDate);
            var pDate = bDate.AddDays(-1);
            var bDay = Convert.ToDouble(bDate.ToString("yyyyMMdd"));
            var rDay = Convert.ToDouble(ReportDate.ToString("yyyyMMdd"));
            if (ItemCode == null) ItemCode = "";

            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            var q = dc.Dealers.Where(d => d.DatabaseCode == DatabaseCode);

            var trans = dc.SaleTransHistories.Where(id => id.TransactionDate <= ReportDate &&
                                                          id.TransactionDate >= bDate &&
                                                          (id.TransactionType == (int)ItemStatus.Imported ||
                                                          id.TransactionType == (int)ItemStatus.Moved ||
                                                          id.TransactionType == (int)ItemStatus.Sold ||
                                                          id.TransactionType == (int)ItemStatus.Return))
                                              .Join(dc.ItemInstances,
                                                    t => t.ItemInstanceId,
                                                    i => i.ItemInstanceId,
                                                    (t, i) => new
                                                    {
                                                        DealerCode = i.DealerCode,
                                                        ItemCode = i.ItemCode,
                                                        TransactionType = t.TransactionType,
                                                    });

            if (!string.IsNullOrEmpty(areaCode))
            {
                q = q.Where(d => d.AreaCode == areaCode);
            }

            if (!string.IsNullOrEmpty(DealerCode))
            {
                q = q.Where(d => d.DealerCode == DealerCode);
                trans = trans.Where(t => t.DealerCode == DealerCode);
            }

            if (!String.IsNullOrEmpty(ItemCode))
            {
                trans = trans.Where(t => t.ItemCode == ItemCode);
            }

            var tranHis = trans.ToList();

            var dealers = q.Select(d => new { d.DealerCode, d.DealerName }).ToList();
            var invs = dc.SaleInventories.Where(i => dealers.Select(d => d.DealerCode).Contains(i.DealerCode) && i.Month == pDate.Month && i.Year == pDate.Year).ToList();
            //var test = dc.SaleInventories.Where(i => (i.Item.ItemType == ItemCode || ItemCode == "" || ItemCode == null) && i.DealerCode == "NC001A" && i.Month == pDate.Month && i.Year == pDate.Year).ToList();
            var res = dealers.Select(d => new IOSaleReportItem
            {
                DealerCode = d.DealerCode,
                DealerName = d.DealerName,
                //TonDau = dc.SaleInventories.Where(i => (i.Item.ItemType == ItemCode || ItemCode == "" || ItemCode == null) && i.DealerCode == d.DealerCode && i.Month == pDate.Month && i.Year == pDate.Year).Sum(i => i.Quantity),
                TonDau = invs.Where(i =>  i.DealerCode == d.DealerCode).Sum(i => i.Quantity),
                //Dat = dc.OrderDetails.Where(od => od.OrderHeader.Status != (int)OrderStatus.Deleted && od.OrderHeader.OrderDate >= bDate && od.OrderHeader.OrderDate < ReportDate.AddDays(1).Date && od.OrderHeader.DealerCode == d.DealerCode).Sum(od => od.OrderQty),
                //Nhap = dc.SaleInventoryDays.Where(i => (i.Item.ItemType == ItemCode || ItemCode == "" || ItemCode == null) && i.Quantity > 0 && i.DealerCode == d.DealerCode && i.ActionDay >= bDay && i.ActionDay <= rDay).Sum(i => i.Quantity),
                Nhap = tranHis.Where(i => IsImportedItem(i.TransactionType) && i.DealerCode == d.DealerCode).Count(),
                Xuat = tranHis.Where(i => !IsImportedItem(i.TransactionType) && i.DealerCode == d.DealerCode).Count(),
                //Nhap = dc.SaleTransHistories.Where(i => (i.ItemInstance.ItemType == ItemCode || ItemCode == "" || ItemCode == null) && i.Quantity > 0 && i.DealerCode == d.DealerCode && i.ActionDay >= bDay && i.ActionDay <= rDay).Sum(i => i.Quantity),
                //Xuat = dc.SaleInventoryDays.Where(i => (i.Item.ItemType == ItemCode || ItemCode == "" || ItemCode == null) && i.Quantity < 0 && i.DealerCode == d.DealerCode && i.ActionDay >= bDay && i.ActionDay <= rDay).Sum(i => i.Quantity),
            })
              .ToList()
              .Select(d => new IOSaleReportItem { DealerCode = d.DealerCode, DealerName = d.DealerName, TonDau = d.TonDau, Nhap = d.Nhap, Xuat = d.Xuat, TonCuoi = d.TonDau + d.Nhap - d.Xuat })
              .Where(d =>
                d.TonDau != 0
                || d.Nhap != 0
                || d.Xuat != 0
                //|| d.Dat != 0
                )
              ;

            return res;
        }
    }
}