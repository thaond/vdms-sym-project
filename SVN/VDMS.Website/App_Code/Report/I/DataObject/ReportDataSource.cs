using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using VDMS.Common.Utils;
using VDMS.Core.Domain;
using VDMS.Helper;
using VDMS.I.ObjectDataSource;
using VDMS.I.ObjectDataSource.RepairList;
using VDMS.I.Service;
using VDMS.I.Vehicle;
using System.Data;
using System.Linq;

/// <summary>
/// Summary description for ReportDataSource
/// </summary>
namespace VDMS.I.Report.DataObject
{
    public class ReportDataSource
    {
        public ReportDataSource()
        {
        }

        #region In tem thu khach hang

        public static IList<CustomerEx> ProcessInvoiceListToCustomerExList(IList<Invoice> listInvoice)
        {
            List<CustomerEx> listCusEx = new List<CustomerEx>();
            List<Customer> listCus = new List<Customer>();

            foreach (Invoice inv in listInvoice)
            {
                if (!listCus.Contains(inv.Customer))
                    listCus.Add(inv.Customer);
            }

            Customer Cus;
            CustomerEx CusEx;
            int sizeListCus = listCus.Count;
            for (int count = 0; count < sizeListCus; count++)
            {
                Cus = listCus[count];
                CusEx = new CustomerEx();
                CusEx.F1 = Cus.Fullname;

                #region Fix lacked address (nmChi)
                //CusEx.A1 = Cus.Address + ", " + Cus.Precinct + ", " + Cus.Districtid + ", " + GetProvinceNameByProvinceCode(Cus.Provinceid);
                CusEx.A1 = ServiceTools.GetCustAddress(Cus);
                #endregion

                count++;
                if (count < sizeListCus)
                {
                    Cus = listCus[count];
                    CusEx.F2 = Cus.Fullname;
                    #region Fix lacked address (ntdung)
                    //CusEx.A2 = Cus.Address + ", " + Cus.Precinct + ", " + Cus.Districtid + ", " + GetProvinceNameByProvinceCode(Cus.Provinceid);
                    CusEx.A2 = ServiceTools.GetCustAddress(Cus);
                    #endregion
                }
                listCusEx.Add(CusEx);
            }
            return listCusEx;
        }
        public static IList<CustomerEx> ConvertToCustomerExList(object cusList)
        {
            List<CustomerEx> listCusEx = new List<CustomerEx>();
            CustomerEx CusEx;
            if (cusList is DataSet)
            {
                var data = ((DataSet)cusList).Tables[0];
                for (int count = 0; count < data.Rows.Count; count++)
                {
                    DataRow row = data.Rows[count];
                    CusEx = new CustomerEx();
                    CusEx.F1 = (string)row["Fullname"];
                    CusEx.A1 = CustomerDAO.BuildAddress((string)row["Address"], (string)row["Provinceid"], (string)row["Districtid"], (string)row["Precinct"]);

                    count++;
                    if (count < data.Rows.Count)
                    {
                        row = data.Rows[count];
                        CusEx.F2 = (string)row["Fullname"];
                        CusEx.A2 = CustomerDAO.BuildAddress((string)row["Address"], (string)row["Provinceid"], (string)row["Districtid"], (string)row["Precinct"]);
                    }
                    listCusEx.Add(CusEx);
                }
            }

            if (cusList is IEnumerable<VDMS.I.Entity.CustomerInfo>)
            {
                var data = (IEnumerable<VDMS.I.Entity.CustomerInfo>)cusList;
                int sizeListCus = data.Count();
                for (int count = 0; count < sizeListCus; count++)
                {
                    VDMS.I.Entity.CustomerInfo Cus = data.ElementAt(count);
                    CusEx = new CustomerEx();
                    CusEx.F1 = Cus.FullName;
                    CusEx.A1 = CustomerDAO.BuildAddress(Cus.Address, Cus.ProvinceId, Cus.Precinct, Cus.DistrictId);

                    count++;
                    if (count < sizeListCus)
                    {
                        Cus = data.ElementAt(count);
                        CusEx.F2 = Cus.FullName;
                        CusEx.A2 = CustomerDAO.BuildAddress(Cus.Address, Cus.ProvinceId, Cus.Precinct, Cus.DistrictId);
                    }
                    listCusEx.Add(CusEx);
                }
            }

            return listCusEx;
        }
        public static ReportDocument GetCustomerExReport(string url, string cusClass, string EngineNumber, string NumberPlate, string InvoiceNumber, string fromDate, string toDate, string AreaCode, string DealerCode, string IdentifyNumber, string Fullname, string Address, string Precinct, string DistrictId, string ProvinceId, string Model)
        {
            //InvoiceDataSource ivDs = new InvoiceDataSource();
            //// set first two params to negative to specify no paging
            //IList<Invoice> listInvoice = ivDs.Select(-1, -1, EngineNumber, NumberPlate, InvoiceNumber, fromDate, toDate, AreaCode, DealerCode, IdentifyNumber, Fullname, Address, Precinct, DistrictId, ProvinceId, Model);
            //IList<CustomerEx> listCustomerEx = ProcessInvoiceListToCustomerExList(listInvoice);
            var data = new CustomerDAO().FindCustomers(cusClass, EngineNumber, NumberPlate, InvoiceNumber, fromDate, toDate, AreaCode, DealerCode, IdentifyNumber, Fullname, Address, Precinct, DistrictId, ProvinceId, Model);
            return GetCustomerExReport(url, ConvertToCustomerExList(data));
        }
        public static ReportDocument GetCustomerExReport(string url, IList<CustomerEx> listCustomerEx)
        {
            ReportDocument rdCustomerEx = ReportFactory.GetReport();
            rdCustomerEx.Load(url);
            rdCustomerEx.SetDataSource(listCustomerEx);
            return rdCustomerEx;
        }
        public static string GetProvinceNameByProvinceCode(string ProvinceCode)
        {
            return VDMS.Data.TipTop.Area.GetProvinceNameByProvinceCode(ProvinceCode);
        }
        #endregion

        #region PrintPartChangeVoucher
        public static List<RptExchangePart> GetExchangePartList(long exchangeId)
        {
            List<RptExchangePart> list = new List<RptExchangePart>();
            ArrayList spares = RepairListDataSource.GetExchangeSpares(exchangeId);
            foreach (Exchangepartdetail spare in spares)
            {
                RptExchangePart rptSpare = new RptExchangePart(spare);
                list.Add(rptSpare);
            }
            return (list.Count == 0) ? null : list;
        }

        public static ReportDocument BuildReportDocumentForPartChange(string exchangeNumber)
        {
            ReportDocument rdPartChange = null;
            Exchangepartheader exH = RepairListDataSource.GetExchangeHeader(exchangeNumber);
            if (exH != null)
            {
                rdPartChange = ReportFactory.GetReport();
                rdPartChange.Load(HttpContext.Current.Server.MapPath(@"~/Report/partChangeVoucher.rpt"));

                string dmg = exH.Damaged, reason = exH.Reason, engine = exH.Engine, frame = exH.Frame, electric = exH.Electric, area = exH.Areacode, custName = exH.Customer.Fullname, address = ServiceTools.GetCustAddress(exH.Customer), dealer = exH.Dealercode, tel = ServiceTools.GetCustTelNo(exH.Customer), model = exH.Model, engNo = exH.Enginenumber, frmNo = exH.Framenumber;
                int road = exH.Road, usage = exH.Usage, weather = exH.Weather, speed = exH.Speed;
                long km = exH.Kmcount;
                DateTime buyDate = exH.Purchasedate, dmgDate = exH.Damageddate, disbDate = exH.Exportdate, reprDate = exH.Exchangeddate;

                rdPartChange.SetDataSource(GetExchangePartList(exH.Id));
                rdPartChange.SetParameterValue("CustName", custName);
                rdPartChange.SetParameterValue("Address", address);
                rdPartChange.SetParameterValue("Tel", string.IsNullOrEmpty(tel) ? "" : tel);
                rdPartChange.SetParameterValue("BuyDate", buyDate);
                rdPartChange.SetParameterValue("BrokenDate", dmgDate);
                rdPartChange.SetParameterValue("Dealer", dealer);
                rdPartChange.SetParameterValue("Km", km);
                rdPartChange.SetParameterValue("RepairDate", reprDate);
                rdPartChange.SetParameterValue("Model", model);
                rdPartChange.SetParameterValue("EngineNumber", engNo);
                rdPartChange.SetParameterValue("FrameNumber", (frmNo == null) ? "" : frmNo);
                rdPartChange.SetParameterValue("ExpDate", disbDate);
                rdPartChange.SetParameterValue("AreaNo", area);
                rdPartChange.SetParameterValue("Damage", dmg);
                rdPartChange.SetParameterValue("Reason", reason);
                rdPartChange.SetParameterValue("Road", exH.Road);
                rdPartChange.SetParameterValue("Usage", exH.Usage);
                rdPartChange.SetParameterValue("Weather", exH.Weather);
                rdPartChange.SetParameterValue("Speed", exH.Speed);
                rdPartChange.SetParameterValue("Engine", (engine == null) ? "" : engine);
                rdPartChange.SetParameterValue("Frame", (frame == null) ? "" : frame);
                rdPartChange.SetParameterValue("Electric", (electric == null) ? "" : electric);
                rdPartChange.SetParameterValue("VoucherNo", (exH.Vouchernumber == null) ? "" : exH.Vouchernumber);
            }
            return rdPartChange;
        }
        #endregion

        #region PrintOrder
        const bool printSignAll = true;

        public static List<RptOrderItem> ParseListOrderItem(List<Orderdetail> list, out long totalPrice, out int totalQuantity, out int status)
        {
            totalPrice = 0;
            totalQuantity = 0;
            status = (list.Count > 0) ? list[0].Orderpriority : -1;
            bool multiPriority = false;

            List<RptOrderItem> items = new List<RptOrderItem>();
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    items.Add(new RptOrderItem(list[i], i + 1));
                    totalQuantity += list[i].Orderqty;
                    totalPrice += list[i].Orderqty * list[i].Unitprice;
                    if ((status > -1) && (list[i].Orderpriority != status))
                    {
                        multiPriority = true;
                    }
                    status = list[i].Orderpriority;
                }
            }
            if (multiPriority) status = -1;

            return items;
        }

        private static string SoundTheMoney(long val)
        {
            return VDMS.Common.Utils.Monetary.Sound(val.ToString(), ",");
        }

        protected static string EvalAddress(string add1, string add2)
        {
            if (!string.IsNullOrEmpty(add2)) return add2;
            add1 = VDMS.Data.TipTop.Dealer.GetAddressByBranchCode(add1);
            return (add1 == null) ? "" : add1;
        }

        public static ReportDocument BuildReportDocumentForOrder(string orderId)
        {
            long totalAmount;
            int itemsCount, status;
            string dealerName;

            ReportDocument rdOrder = null;
            var oH = OrderHeaderDataSource.GetOrderHeaderById((long.Parse(orderId)));

            if (oH != null)
            {
                // note: all (*) section are replace between Dealercode and ShippingTo
                string reportNo = ConfigurationManager.AppSettings["OrderPrintNo"];
                reportNo = (string.IsNullOrEmpty(reportNo)) ? "4" : reportNo.Trim();

                rdOrder = ReportFactory.GetReport(); //new ReportDocument();
                rdOrder.Load(HttpContext.Current.Server.MapPath(string.Format(@"~/Report/{0}PrintOrder.rpt", reportNo)));

                dealerName = DealerHelper.GetName(oH.DealerCode);   //(*)
                string isoNo = ConfigurationManager.AppSettings["IsoOrderPrintNo"];
                string isoDate = ConfigurationManager.AppSettings["IsoOrderPrintDate"];

                string topContractNo = ConfigurationManager.AppSettings["TopContractNo"];
                string contractNo = ConfigurationManager.AppSettings["ContractNo"];
                if (string.IsNullOrEmpty(topContractNo)) topContractNo = "{0}/HĐĐL-08";
                if (string.IsNullOrEmpty(contractNo)) contractNo = "{0}/HĐĐL(HĐMB)-08";

                string shipTo = EvalAddress(oH.ShippingTo, oH.SecondaryShippingTo);
                string dComment = oH.DealerComment == null ? "" : oH.DealerComment;
                string bonusComment = (oH.BonusAmount > 0) ? string.Format(Resources.Constants.PrintOrder_UsingBonus, oH.BonusAmount.ToString("N0")) : "";

                //bonus/payment
                VDMS.I.Entity.SaleOrderPayment dp = null;
                VDMS.I.Entity.SaleOrderPayment fp = null;
                long cr;
                var o = OrderDAO.GetOrder(oH.OrderHeaderId);
                if (o != null)
                {
                    dp = o.SaleOrderPayments.Where(p => p.PaymentType == VDMS.I.Entity.OrderPaymentType.FromDealer).OrderBy(p => p.PaymentDate).FirstOrDefault();
                    fp = o.SaleOrderPayments.Where(p => p.PaymentType == VDMS.I.Entity.OrderPaymentType.BankConfirmed).OrderBy(p => p.PaymentDate).FirstOrDefault();
                    cr = (long)o.SaleOrderPayments.Where(p => p.PaymentType == VDMS.I.Entity.OrderPaymentType.RemainingPayment).Sum(r => r.Amount);
                    if (cr > 0)
                        bonusComment += "\n" + string.Format(Resources.Constants.PrintOrder_UsingCR, cr.ToString("N0"));
                }
                string fromAcc = dp == null || dp.FromAccount == null ? "" : dp.FromAccount;
                string toAcc = dp == null || dp.ToAccount == null ? "" : dp.ToAccount;
                string fromBank = dp == null || dp.FromBank == null ? "" : dp.FromBank;
                string toBank = dp == null || dp.ToBank == null ? "" : dp.ToBank;
                string dPdate = dp == null || !dp.PaymentDate.HasValue ? "" : dp.PaymentDate.Value.ToString("dd/MM/yyyy");
                string fPdate = fp == null || !fp.PaymentDate.HasValue ? "" : fp.PaymentDate.Value.ToString("dd/MM/yyyy");
                decimal dPAmount = dp == null ? 0 : dp.Amount;
                decimal fPAmount = fp == null ? 0 : fp.Amount;

                rdOrder.SetDataSource(ParseListOrderItem(OrderDetailDataSource.GetItemsByHeaderId(orderId), out totalAmount, out itemsCount, out status));
                string moneySounded = SoundTheMoney(totalAmount) + ".";
                rdOrder.SetParameterValue("DealerCode", oH.DealerCode); //(*)
                rdOrder.SetParameterValue("ShippingTo", shipTo);
                rdOrder.SetParameterValue("DealerName", dealerName);
                rdOrder.SetParameterValue("OrderDate", oH.OrderDate);
                rdOrder.SetParameterValue("OrderNumber", oH.OrderNumber == null ? "" : oH.OrderNumber);
                rdOrder.SetParameterValue("TotalQuantity", itemsCount);
                rdOrder.SetParameterValue("TotalAmount", totalAmount);
                rdOrder.SetParameterValue("Priority", status);
                rdOrder.SetParameterValue("TotalAmountString", moneySounded);
                rdOrder.SetParameterValue("TopContractNo", string.Format(topContractNo, oH.DealerCode));//(*)
                rdOrder.SetParameterValue("ContractNo", string.Format(contractNo, oH.DealerCode));//(*)
                rdOrder.SetParameterValue("DealerNameAndCode", dealerName + "/" + oH.DealerCode);//(*)
                rdOrder.SetParameterValue("IsoOrderPrintDate", string.IsNullOrEmpty(isoDate) ? "Lần sửa đổi: 05 (02/01/10)" : isoDate);
                rdOrder.SetParameterValue("IsoOrderPrintNo", string.IsNullOrEmpty(isoNo) ? "M-006/QT02-SA" : isoNo);
                rdOrder.SetParameterValue("IsConfirmed", ((OrderStatus)oH.Status == OrderStatus.Confirmed) || ((OrderStatus)oH.Status == OrderStatus.Approved) || ((OrderStatus)oH.Status == OrderStatus.Sent) || ((OrderStatus)oH.Status == OrderStatus.PaymentConfirmed) || printSignAll);
                rdOrder.SetParameterValue("AConfirmed", ((OrderStatus)oH.Status == OrderStatus.Confirmed) || ((OrderStatus)oH.Status == OrderStatus.PaymentConfirmed));
                rdOrder.SetParameterValue("SaleConfirmed", ((OrderStatus)oH.Status == OrderStatus.Confirmed) || ((OrderStatus)oH.Status == OrderStatus.PaymentConfirmed));
                rdOrder.SetParameterValue("FinConfirmed", ((OrderStatus)oH.Status == OrderStatus.PaymentConfirmed));
                rdOrder.SetParameterValue("FromBank", fromBank);
                rdOrder.SetParameterValue("ToBank", toBank);
                rdOrder.SetParameterValue("FromAcc", fromAcc);
                rdOrder.SetParameterValue("ToAcc", toAcc);
                rdOrder.SetParameterValue("DPaymentDate", dPdate);
                rdOrder.SetParameterValue("FPaymentDate", fPdate);
                rdOrder.SetParameterValue("DPayAmount", dPAmount);
                rdOrder.SetParameterValue("FPayAmount", fPAmount);
                rdOrder.SetParameterValue("DealerComment", dComment);
                rdOrder.SetParameterValue("BonusComment", bonusComment);
                rdOrder.SetParameterValue("ShowBonusComment", !string.IsNullOrEmpty(bonusComment));

                //rdOrder.Subreports[0].SetParameterValue("TotalAmountString", moneySounded);
                //rdOrder.Subreports[0].SetParameterValue("TotalQuantity", itemsCount);
                //rdOrder.Subreports[0].SetParameterValue("TotalAmount", totalAmount);
            }

            return rdOrder;
        }


        #endregion
    }
}