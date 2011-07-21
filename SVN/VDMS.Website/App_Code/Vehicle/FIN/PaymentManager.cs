using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.I.Linq;
using VDMS.II.Linq;
using VDMS.I.Entity;
using System.ComponentModel;
using VDMS.Helper;
using Resources;
using VDMS.II.Common.ExcelReader;
using System.IO;
using VDMS.Common.Utils;

namespace VDMS.I.Vehicle
{
    public class ConsignOrderPayment
    {
        public OrderHeader OrderHeader { get; set; }
        public long TotalPaymentAmount { get; set; }
        public IQueryable<SaleOrderPayment> SaleOrderPayments { get; set; }
    }
    public class ConfirmOrderInfo
    {
        public bool Selected { get; set; }
        public long Id { get; set; }
        public string Desc { get; set; }
        public string Voucher { get; set; }

        public ConfirmOrderInfo(OrderHeader o)
        {
            this.Id = o.OrderHeaderId;
            this.Selected = true; // o.Status == (int)OrderStatus.PaymentConfirmed;
            this.Desc = o.FinComment;
            this.Voucher = o.FinVoucher;
        }
        public ConfirmOrderInfo()
        {
        }
    }

    [DataObject]
    public class PaymentManager
    {
        public PaymentManager()
        {
        }

        public static VehicleDataContext DC
        {
            get
            {
                return DCFactory.GetDataContext<VehicleDataContext>();
            }
        }

        #region Import

        static List<SaleOrderPayment> ImportingItems = new List<SaleOrderPayment>();

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IEnumerable<SaleOrderPayment> GetImportingItems()
        {
            return ImportingItems.Where(p => !p.Deleted && p.SessionId == HttpContext.Current.Session.SessionID
                && p.PaymentType == OrderPaymentType.BankImport)
                                    .OrderBy(p => p.Index)
                                    .OrderByDescending(p => p.Error)
                                    ;
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IEnumerable<SaleOrderPayment> GetDeletingItems()
        {
            return ImportingItems.Where(p => p.Deleted && p.SessionId == HttpContext.Current.Session.SessionID
                && p.PaymentType == OrderPaymentType.BankImport);
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteImportingPayment(Guid id)
        {
            var p = ImportingItems.SingleOrDefault(i => i.Id == id);
            DeleteImportingPayment(p);
        }
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteImportingPayment(SaleOrderPayment item)
        {
            if (item != null)
            {
                if (item.OrderPaymentId > 0) item.Deleted = true;
                else ImportingItems.RemoveAll(p => p.Id == item.Id);
            }
        }

        public static void CleanSession(string sessionId)
        {
            ImportingItems.RemoveAll(p => p.SessionId == sessionId);
        }
        public static void CleanSessionBank(string bCode)
        {
            ImportingItems.RemoveAll(p => p.ToBank == bCode);
        }

        public static SaleOrderPayment ImportPayment(string orderno, string bankCode, string dCode, string dName, string trans, string content, DateTime pDate, long amount)
        {
            SaleOrderPayment p = null;
            long oid;
            OrderHeader o;
            if (long.TryParse(orderno, out oid))
                o = OrderDAO.GetOrder(oid);
            else
                o = OrderDAO.GetOrder(orderno);

            p = new SaleOrderPayment()
            {
                Id = Guid.NewGuid(),
                Amount = amount,
                OrderNumber = o.OrderNumber,
                CreatedDate = DateTime.Now,
                DealerCode = dCode,
                DealerName = dName,
                Description = content,
                LastEditDate = DateTime.Now,
                OrderHeaderId = o.OrderHeaderId,
                PaymentDate = pDate,
                PaymentType = OrderPaymentType.BankImport,
                SessionId = HttpContext.Current.Session.SessionID,
                ToBank = bankCode,
                UserName = UserHelper.Username,
                VoucherNumber = trans,
            };
            // check for existing data
            if (DC.SaleOrderPayments.Any(op => op.ToBank == bankCode && op.VoucherNumber == trans))
            {
                p.Error = p.Error + " " + string.Format(Errors.DataExistInDatabase, bankCode, trans);
            }

            ImportingItems.Add(p);

            if (o == null) p.Error = p.Error + " " + Errors.NotExistVDMSOrderNumber;
            else if (o.DealerCode != dCode) p.Error = p.Error + " " + string.Format(Errors.WrongActualDealerCode, o.DealerCode);
            else if (o.DatabaseCode != UserHelper.DatabaseCode || (o.AreaCode != UserHelper.AreaCode && UserHelper.Profile.Position == VDMS.II.Entity.PositionType.Employee && !UserHelper.IsSysAdmin)) p.Error = p.Error + " " + string.Format(Errors.NotExistDealerCode, o.DealerCode);

            return p;
        }
        public static void ImportPayment(SaleOrderPayment p)
        {
            p.Id = Guid.NewGuid();
            p.SessionId = HttpContext.Current.Session.SessionID;

            ImportingItems.Add(p);
        }

        public static bool LoadExcelData(Stream excel, string bCode)
        {
            var result = false;
            var setting = VDMS.VDMSSetting.CurrentSetting.BankPaymentSettings.SingleOrDefault(s => s.BankCode == bCode);
            if (setting == null) return false;

            //try
            {
                ExcelDataReader spreadsheet = new ExcelDataReader(excel);

                var am = setting.Amount;
                var sr = setting.StartRow;
                var cm = setting.Comment;
                var vn = setting.OrderId;
                var pd = setting.PaymentDate;
                var tn = setting.TransactionNumber;
                var dc = setting.DealerCode;
                var dn = setting.DealerName;
                if (am < 1 || sr < 1 || cm < 1 || vn < 1 || pd < 1 || tn < 1 || dc < 1 || dn < 1)
                    throw new Exception(string.Format("Wrong excel format for: {0}", bCode));

                var rows = spreadsheet.WorkbookData.Tables[0].AsEnumerable();
                int i = 0;
                var query = from r in rows
                            select new
                            {
                                Amount = r.Field<string>(am - 1),
                                Comment = r.Field<string>(cm - 1),
                                OrderNumber = r.Field<string>(vn - 1),
                                PaymentDate = r.Field<string>(pd - 1),
                                TransactionNumber = r.Field<string>(tn - 1),
                                DealerCode = r.Field<string>(dc - 1),
                                DealerName = r.Field<string>(dn - 1),
                                Index = ++i,
                            };
                var data = query.Skip(sr - 1).TakeWhile(p => !string.IsNullOrEmpty(p.TransactionNumber));

                List<SaleOrderPayment> items = new List<SaleOrderPayment>();

                foreach (var item in data)
                {

                    try
                    {
                        DateTime d = DataFormat.DateFromExcel(item.PaymentDate, setting.DateFormat);
                        ImportPayment(item.OrderNumber.Trim(), bCode, item.DealerCode, item.DealerName, item.TransactionNumber, item.Comment, d, long.Parse(item.Amount)).Index = item.Index;
                    }
                    catch
                    {
                        result = false;
                        var p = new SaleOrderPayment() { Error = Resources.Message.DataFormatWrong };
                        ImportPayment(p);
                    };
                }

                result = true;
            }
            //catch
            //{
            //    result = false;
            //}

            return result;
        }

        public static void SavePayments()
        {
            var eitems = GetImportingItems();
            var ditems = GetDeletingItems();
            var cd = DateTime.Now;
            if (eitems.Count() > 0 || ditems.Count() > 0)
            {
                DC.SaleOrderPayments.DeleteAllOnSubmit(ditems);
                //...
                foreach (var i in eitems)
                {
                    if (i.OrderPaymentId == 0)
                    {
                        i.CreatedDate = cd;
                        i.LastEditDate = cd;
                        i.UserName = UserHelper.Username;
                        DC.SaleOrderPayments.InsertOnSubmit(i);
                    }
                    else
                    {
                        var d = DC.SaleOrderPayments.SingleOrDefault(p => p.OrderPaymentId == i.OrderPaymentId);
                        if (d != null)
                        {
                            d.Amount = i.Amount;
                            d.Description = i.Description;
                            d.FromAccount = i.FromAccount;
                            d.FromAccountHolder = i.FromAccountHolder;
                            d.FromBank = i.FromBank;
                            d.LastEditDate = i.LastEditDate;
                            d.PaymentDate = i.PaymentDate;
                            d.ToAccount = i.ToAccount;
                            d.ToAccountHolder = i.ToAccountHolder;
                            d.ToBank = i.ToBank;
                            d.PaymentType = i.PaymentType;
                            d.VoucherNumber = i.VoucherNumber;
                        }
                    }
                }
                DC.SubmitChanges();
                CleanSession(HttpContext.Current.Session.SessionID);
            }
        }

        #endregion

        #region query

        int pCount;
        public int CountBankPayments(string orderNum, string dCode, DateTime from, DateTime to, string status)
        {
            return pCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable<SaleOrderPayment> QueryBankPayments(int maximumRows, int startRowIndex, string orderNum, string dCode, DateTime from, DateTime to, string status)
        {
            var q = DC.SaleOrderPayments.Where(p => p.PaymentType != OrderPaymentType.FromDealer && p.OrderHeader.DatabaseCode == UserHelper.DatabaseCode);
            // giong nhu sale man, FIN man chi quan ly vung cua minh(admin - FIN_ADM: theo DB)
            if (UserHelper.Profile.Position == VDMS.II.Entity.PositionType.Employee && !UserHelper.IsSysAdmin)
                q = q.Where(p => p.OrderHeader.AreaCode == UserHelper.AreaCode);

            if (!string.IsNullOrEmpty(status)) q = q.Where(p => status == p.PaymentType);
            if (from > DateTime.MinValue) q = q.Where(p => p.PaymentDate >= from.Date);
            if (to > DateTime.MinValue) q = q.Where(p => p.PaymentDate < to.Date.AddDays(1));
            if (!string.IsNullOrEmpty(orderNum)) q = q.Where(p => p.OrderHeader.OrderNumber == orderNum);
            if (!string.IsNullOrEmpty(dCode)) q = q.Where(p => p.OrderHeader.DealerCode == dCode);

            pCount = q.Count();
            if (maximumRows > 0) q = q.Skip(startRowIndex).Take(maximumRows);

            return q;
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<ConsignOrderPayment> QueryOrderPayments(int maximumRows, int startRowIndex, string orderNum, string dCode, DateTime from, DateTime to, string status)
        {
            var q = QueryBankPayments(-1, -1, orderNum, dCode, from, to, status).ToList();
            q.AddRange(QueryBankPayments(-1, -1, orderNum, dCode, from, to, OrderPaymentType.RemainingPayment).ToList());

            var query = q.GroupBy(p => p.OrderHeader).Select(g => new ConsignOrderPayment()
                {
                    OrderHeader = g.Key,
                });
            pCount = query.Count();
            if (maximumRows > 0) query = query.Skip(startRowIndex).Take(maximumRows);

            var res = query.ToList();
            res.ForEach(p =>
                {
                    p.SaleOrderPayments = q.Where(pm => pm.OrderHeaderId == p.OrderHeader.OrderHeaderId).AsQueryable();
                    p.TotalPaymentAmount = (long)p.SaleOrderPayments.Sum(pm => pm.Amount);
                });

            return res;
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public void UpdatePayment(long OrderPaymentId, DateTime PaymentDate, string OrderOrderNumber, string Description, string ToBank, string VoucherNumber, string PaymentType)
        {
            var pm = DC.SaleOrderPayments.SingleOrDefault(p => p.OrderPaymentId == OrderPaymentId);
            if (pm != null)
            {
                var o = OrderDAO.GetOrder(OrderOrderNumber);
                pm.OrderHeaderId = o.OrderHeaderId;
                pm.PaymentType = PaymentType;
                pm.PaymentDate = PaymentDate;
                pm.Description = Description;
                pm.ToBank = ToBank;
                pm.VoucherNumber = VoucherNumber;
                DC.SubmitChanges();
            }
        }

        #endregion

        #region confirm

        int _ConfirmOrdersCount;
        public int CountConfirmOrders(string oFrom, string oTo, string dCode, string aCode, DateTime from, DateTime to, int status, int pClass, string key, out int fp, out int pp, out int up, out int ap)
        {
            pp = fp = up = ap = 0;
            return _ConfirmOrdersCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable<OrderHeader> QueryConfirmOrders(int maximumRows, int startRowIndex, string oFrom, string oTo, string dCode, string aCode, DateTime from, DateTime to, int status, int pClass, string key, out int fp, out int pp, out int up, out int ap)
        {
            var q = DC.OrderHeaders.Where(o => o.DatabaseCode == UserHelper.DatabaseCode)
                        .Where(o => o.Status == (int)OrderStatus.PaymentConfirmed || o.Status == (int)OrderStatus.Confirmed);
            // giong nhu sale man, FIN man chi quan ly vung cua minh(admin - FIN_ADM: theo DB)
            if (UserHelper.Profile.Position == VDMS.II.Entity.PositionType.Employee && !UserHelper.IsSysAdmin)
                q = q.Where(p => p.AreaCode == UserHelper.AreaCode);

            if (status > 0) q = q.Where(o => o.Status == status);
            if (!string.IsNullOrEmpty(dCode)) q = q.Where(o => o.DealerCode == dCode);
            if (!string.IsNullOrEmpty(aCode)) q = q.Where(o => o.AreaCode == aCode);
            if (!string.IsNullOrEmpty(oFrom)) q = q.Where(p => p.OrderNumber.CompareTo(oFrom.Trim().ToUpper()) >= 0);
            if (!string.IsNullOrEmpty(oTo)) q = q.Where(p => p.OrderNumber.CompareTo(oTo.Trim().ToUpper()) <= 0);
            if (from > DateTime.MinValue) q = q.Where(o => o.OrderDate >= from);
            if (to > DateTime.MinValue) q = q.Where(o => o.OrderDate < to.Date.AddDays(1));

            _ConfirmOrdersCount = q.Count();

            var r = q.ToList();

            // return values
            fp = q.Where(o => o.PaymentAmount + o.BonusAmount == o.SubTotal).Count();
            pp = q.Where(o => o.PaymentAmount + o.BonusAmount < o.SubTotal && o.PaymentAmount > 0).Count();
            up = q.Where(o => o.PaymentAmount == 0).Count();
            ap = _ConfirmOrdersCount;

            // mark selected
            if (!string.IsNullOrEmpty(key) && pClass >= 0)
            {
                var list = GetOrdersConfirm(key);
                List<ConfirmOrderInfo> tmp;
                switch (pClass) // select orders: all,fully paid,partly paid,unpaid,none
                {
                    case 0: tmp = (q.Select(o => new ConfirmOrderInfo(o)).ToList()); break;
                    case 1: tmp = (q.Where(o => o.PaymentAmount + o.BonusAmount == o.SubTotal).Select(o => new ConfirmOrderInfo(o)).ToList()); break;
                    case 2: tmp = (q.Where(o => o.PaymentAmount + o.BonusAmount < o.SubTotal && o.PaymentAmount > 0).Select(o => new ConfirmOrderInfo(o)).ToList()); break;
                    case 3: tmp = (q.Where(o => o.PaymentAmount == 0).Select(o => new ConfirmOrderInfo(o)).ToList()); break;
                    default: tmp = new List<ConfirmOrderInfo>(); break;
                }
                // check/add new item
                tmp.ForEach(o => { if (!list.Exists(p => p.Id == o.Id)) list.Add(o); });
            }

            if (maximumRows > 0) q = q.Skip(startRowIndex).Take(maximumRows);

            return q;
        }
        public IQueryable<OrderHeader> QueryConfirmOrders(string oFrom, string oTo, string dCode, string aCode, DateTime from, DateTime to, int status)
        {
            int fp, pp, up, ap;
            return QueryConfirmOrders(-1, -1, oFrom, oTo, dCode, aCode, from, to, status, 0, null, out fp, out pp, out up, out ap);
        }

        public static List<ConfirmOrderInfo> GetOrdersConfirm(string key)
        {
            var s = (List<ConfirmOrderInfo>)HttpContext.Current.Session[key];
            if (s == null)
            {
                s = new List<ConfirmOrderInfo>();
                HttpContext.Current.Session[key] = s;
            }
            return s;
        }
        public static List<ConfirmOrderInfo> GetOrdersConfirmResult(string key)
        {
            var s = (List<ConfirmOrderInfo>)HttpContext.Current.Session[key + "res"];
            if (s == null)
            {
                s = new List<ConfirmOrderInfo>();
                HttpContext.Current.Session[key + "res"] = s;
            }
            return s;
        }

        #endregion

        public static bool ReConsign(DateTime from, DateTime to, string dCode)
        {
            var found = false;
            var q = new PaymentManager().QueryOrderPayments(-1, -1, null, dCode, from, to, OrderPaymentType.BankImport);
            foreach (var item in q)
            {
                found = true;

                var total = item.OrderHeader.TotalAmount;
                item.OrderHeader.SubTotal = total;
                foreach (var pm in item.SaleOrderPayments)
                {
                    if (pm.PaymentType == OrderPaymentType.BankImport)
                        pm.PaymentType = OrderPaymentType.BankConfirmed;
                    if (pm.PaymentType == OrderPaymentType.RemainingPayment)
                        pm.PaymentType = OrderPaymentType.RemainingPaymentComfirmed;
                    item.OrderHeader.PaymentAmount += (long)pm.Amount;
                }
                var remain = item.OrderHeader.PaymentAmount + item.OrderHeader.BonusAmount - total;

                if (remain > 0)
                {
                    item.OrderHeader.PaymentAmount = total - item.OrderHeader.BonusAmount > 0 ? total - item.OrderHeader.BonusAmount : 0;
                    var lastPm = item.SaleOrderPayments.OrderByDescending(p => p.PaymentDate).FirstOrDefault();
                    var pm = new SaleOrderPayment()
                    {
                        Amount = remain,
                        CreatedDate = DateTime.Now,
                        Description = "Auto Re-consign",
                        LastEditDate = DateTime.Now,
                        OrderHeaderId = item.OrderHeader.OrderHeaderId,
                        PaymentDate = DateTime.Now,
                        PaymentType = OrderPaymentType.ConsignRemain,
                        ToAccount = lastPm.ToAccount,
                        ToAccountHolder = lastPm.ToAccountHolder,
                        ToBank = lastPm.ToBank,
                        UserName = UserHelper.Username,
                        VoucherNumber = string.Format("OrderNo: {0}", item.OrderHeader.OrderNumber),
                    };
                    DC.SaleOrderPayments.InsertOnSubmit(pm);
                }
            }
            DC.SubmitChanges();
            return found;
        }
        public static bool ConfirmPayment(string key)
        {
            var list = GetOrdersConfirm(key);
            foreach (var item in list)
            {
                var oh = DC.OrderHeaders.SingleOrDefault(o => o.OrderHeaderId == item.Id);
                if (oh.Status == (int)OrderStatus.PaymentConfirmed && !VDMSSetting.CurrentSetting.AllowUndoVehiclePaymentConfirm) continue;

                int newStatus = item.Selected ? (int)OrderStatus.PaymentConfirmed : (int)OrderStatus.Confirmed;
                if (oh.Status != newStatus)
                {
                    oh.FinComment = item.Desc;
                    oh.FinVoucher = item.Voucher;
                    oh.Status = newStatus;
                }
            }
            DC.SubmitChanges();
            list.Clear();

            return true;
        }
    }
}