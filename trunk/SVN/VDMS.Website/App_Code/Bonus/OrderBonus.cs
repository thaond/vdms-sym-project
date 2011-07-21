using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.Bonus.Linq;
using VDMS.II.Linq;
using VDMS.I.Linq;
using VDMS.I.Entity;
using System.ComponentModel;
using VDMS.Helper;
using VDMS.Bonus.Entity;

namespace VDMS.II.BonusSystem
{
    [DataObject]
    public class OrderBonus
    {
        public static BonusDataContext BDC
        {
            get
            {
                return DCFactory.GetDataContext<BonusDataContext>();
            }
        }
        public static VehicleDataContext VDC
        {
            get
            {
                return DCFactory.GetDataContext<VehicleDataContext>();
            }
        }

        static List<SaleOrderPayment> EditingItems = new List<SaleOrderPayment>();
        static List<BonusTransaction> EditingBonuses = new List<BonusTransaction>();

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEditingItem(Guid Id)
        {
            var Item = EditingItems.SingleOrDefault(p => p.Id == Id);
            if (Item != null)
                DeleteEditingItem(Item);
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEditingItem(SaleOrderPayment Item)
        {
            if (Item.OrderHeaderId > 0)
                Item.Deleted = true;
            else
                EditingItems.RemoveAll(p => p.Id == Item.Id);
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEditingBonus(Guid Id)
        {
            var bonus = EditingBonuses.SingleOrDefault(b => b.Id == Id);
            if (bonus != null)
            {
                DeleteEditingBonus(bonus);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEditingBonus(BonusTransaction bonus)
        {
            var bonusToRemove = EditingBonuses.SingleOrDefault(b => b.Id == bonus.Id);
            if (bonusToRemove.OrderId > 0)
                bonusToRemove.Deleted = true;
            else
                EditingBonuses.RemoveAll(b => b.Id == bonus.Id);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IEnumerable<SaleOrderPayment> GetEditingPayments(long oid)
        {
            return EditingItems.Where(p => !p.Deleted && p.OrderHeaderId == oid && p.SessionId == HttpContext.Current.Session.SessionID);
        }
        public static IEnumerable<SaleOrderPayment> GetDeletingPayments(long oid)
        {
            return EditingItems.Where(p => p.Deleted && p.OrderHeaderId == oid && p.SessionId == HttpContext.Current.Session.SessionID);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IEnumerable<BonusTransaction> GetEditingBonuses(long oid)
        {
            return EditingBonuses.Where(p => !p.Deleted && p.SessionId == HttpContext.Current.Session.SessionID);
        }
        public static IEnumerable<BonusTransaction> GetDeletingBonuses(long oid)
        {
            return EditingBonuses.Where(p => p.Deleted && p.SessionId == HttpContext.Current.Session.SessionID);
        }

        public static void CleanSessionOrderPayments(string sessionId)
        {
            EditingItems.RemoveAll(p => p.SessionId == sessionId);
            EditingBonuses.RemoveAll(b => b.SessionId == sessionId);
        }
        public static void CleanSessionOrderPayments(long id)
        {
            EditingItems.RemoveAll(p => p.OrderHeaderId == id);
            EditingBonuses.RemoveAll(b => b.OrderId == id);
        }

        public static void CleanBonusData(long oid)
        {
            EditingBonuses.RemoveAll(b => b.OrderId != oid);
        }

        public static VDMS.I.Entity.OrderHeader LoadSaleOrderPayments(long oid)
        {
            var bdc = DCFactory.GetDataContext<BonusDataContext>();
            if (oid > 0) CleanSessionOrderPayments(oid);
            var o = VDC.OrderHeaders.SingleOrDefault(p => p.OrderHeaderId == oid);
            //o.SaleOrderPayments.Load();
            //VDC.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, o);
            if (o != null)
            {
                //EditingPlans.Add(plan);
                var items = o.SaleOrderPayments.Where(p => p.PaymentType == OrderPaymentType.FromDealer || p.PaymentType == OrderPaymentType.RemainingPayment).ToList();
                items.ForEach(p => { p.SessionId = HttpContext.Current.Session.SessionID; p.Id = Guid.NewGuid(); });

                EditingItems.AddRange(items);

                var bonuses = bdc.BonusTransactions.Where(t => t.OrderId == o.OrderHeaderId && t.TransactionType == BonusTransactionType.OrderSubstract).ToList();
                foreach (var b in bonuses)
                {
                    b.SessionId = HttpContext.Current.Session.SessionID;
                    b.Id = Guid.NewGuid();
                    b.Amount = -b.Amount;
                    b.BonusSourceName = b.BonusSource.BonusSourceName;
                }

                EditingBonuses.AddRange(bonuses);
            }
            return o;
        }

        public static void SaveSaleOrderPayments(long oldId, long OrderId, long bonus)
        {
            var o = VDC.OrderHeaders.SingleOrDefault(p => p.OrderHeaderId == OrderId);
            var eitems = GetEditingPayments(oldId).Where(p => p.PaymentType != OrderPaymentType.RemainingPayment);
            var bitems = GetEditingPayments(oldId).Where(p => p.PaymentType == OrderPaymentType.RemainingPayment);
            var ditems = GetDeletingPayments(oldId);

            if (eitems.Count() > 0 || ditems.Count() > 0)
            {
                VDC.SaleOrderPayments.DeleteAllOnSubmit(ditems);
                //...
                foreach (var i in eitems)
                {
                    if (i.OrderPaymentId == 0)
                    {
                        i.OrderHeaderId = o.OrderHeaderId;
                        i.CreatedDate = DateTime.Now;
                        i.LastEditDate = DateTime.Now;
                        i.UserName = UserHelper.Username;
                        VDC.SaleOrderPayments.InsertOnSubmit(i);
                    }
                    else
                    {
                        var d = VDC.SaleOrderPayments.SingleOrDefault(p => p.OrderPaymentId == i.OrderPaymentId);
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
            }
            o.PaymentAmount = (long)o.SaleOrderPayments.Sum(p => p.Amount);
            o.BonusAmount = bonus;
            o.BonusStatus = OrderBonusStatus.New;


            VDC.SubmitChanges();

            foreach (var i in bitems)
            {
                if (i.OrderPaymentId == 0)
                {
                    i.OrderHeaderId = o.OrderHeaderId;
                    i.CreatedDate = DateTime.Now;
                    i.LastEditDate = DateTime.Now;
                    i.UserName = UserHelper.Username;
                    i.VoucherNumber = string.Format("{0} for Order {1}", i.VoucherNumber, o.OrderHeaderId);
                    VDC.SaleOrderPayments.InsertOnSubmit(i);
                    VDC.SubmitChanges();

                    var trans = new SaleOrderPaymentTransHistory
                        {
                            Amount = -i.Amount,
                            Orderid = o.OrderHeaderId,
                            Description = i.Description,
                            ModifiedDate = DateTime.Now,
                            ModifiedUser = UserHelper.Username,
                            OrderPaymentId = i.OriginalSaleOrderPaymentId,
                            TransactionDate = o.CreatedDate,
                            TransactionType = OrderPaymentType.RemainingPayment,
                        };
                    VDC.SaleOrderPaymentTransHistories.InsertOnSubmit(trans);
                }
                else
                {
                    var d = VDC.SaleOrderPayments.SingleOrDefault(p => p.OrderPaymentId == i.OrderPaymentId);
                    var amountBefore = d.Amount;
                    var amountAfter = i.Amount;
                    if (d != null)
                    {
                        d.Amount = i.Amount;
                        d.LastEditDate = DateTime.Now;
                    }
                    if (amountAfter != amountBefore)
                    {
                        var trans = new SaleOrderPaymentTransHistory
                        {
                            Amount = amountBefore - amountAfter,
                            Orderid = o.OrderHeaderId,
                            Description = i.Description,
                            ModifiedDate = DateTime.Now,
                            ModifiedUser = UserHelper.Username,
                            OrderPaymentId = i.OriginalSaleOrderPaymentId,
                            TransactionDate = o.CreatedDate,
                            TransactionType = OrderPaymentType.RemainingPayment,
                        };
                        VDC.SaleOrderPaymentTransHistories.InsertOnSubmit(trans);
                    }
                }
            }

            VDC.SubmitChanges();

            EditingItems.Clear();

            SaveBonusTransaction(OrderId);

            EditingBonuses.Clear();
        }

        public static SaleOrderPayment GetOrderPayment(long PaymentId)
        {
            throw new NotImplementedException();
        }

        public static SaleOrderPayment GetEditingPayment(Guid id)
        {
            return EditingItems.SingleOrDefault(p => p.Id == id);
        }

        public static SaleOrderPayment GetEditingPayment(long paymentId)
        {
            return EditingItems.SingleOrDefault(p => p.OriginalSaleOrderPaymentId == paymentId);
        }

        public static BonusTransaction GetEditingBonus(long bonusId)
        {
            return EditingBonuses.SingleOrDefault(b => b.BonusPlanDetailId == bonusId);
        }

        public static void AddPayment(SaleOrderPayment PaymentInfo, long oid)
        {
            PaymentInfo.Id = Guid.NewGuid();
            PaymentInfo.SessionId = HttpContext.Current.Session.SessionID;
            if (oid > 0) PaymentInfo.OrderHeaderId = oid;

            var o = EditingItems.FirstOrDefault(p => p.FromBank == PaymentInfo.FromBank && p.FromAccount == PaymentInfo.FromAccount && p.VoucherNumber == PaymentInfo.VoucherNumber);
            if (o != null) EditingItems.Remove(o);
            EditingItems.Add(PaymentInfo);
        }

        public static void AddBonus(BonusTransaction bonus, long oid)
        {
            bonus.Id = Guid.NewGuid();
            bonus.SessionId = HttpContext.Current.Session.SessionID;
            if (oid > 0) bonus.OrderId = oid;
            var o = EditingBonuses.FirstOrDefault(b => b.BonusPlanDetailId == bonus.BonusPlanDetailId);
            if (o != null) EditingBonuses.Remove(o);
            EditingBonuses.Add(bonus);
        }

        public static long GetBonusAmount()
        {
            return (long)EditingBonuses.Sum(b => b.Amount);
        }

        public static void SaveBonusTransaction(long oid)
        {
            var order = VDC.OrderHeaders.SingleOrDefault(o => o.OrderHeaderId == oid);
            var eBonuses = GetEditingBonuses(oid);
            var dBonuses = BDC.BonusTransactions.Where(t => GetDeletingBonuses(oid).Select(b => b.TransactionId).Contains(t.TransactionId));
            BDC.BonusTransactions.DeleteAllOnSubmit(dBonuses);
            foreach (var b in eBonuses)
            {
                var trans = BDC.BonusTransactions.SingleOrDefault(t => t.BonusPlanDetailId == b.BonusPlanDetailId && t.OrderId == oid);
                if (trans == null)
                {
                    b.Amount = -b.Amount;
                    b.TransactionDate = order.CreatedDate;
                    b.CreatedDate = DateTime.Now;
                    b.UserName = UserHelper.Username;
                    b.OrderId = order.OrderHeaderId;
                    b.Description = "Bonus used for Order " + order.OrderHeaderId;
                    BDC.BonusTransactions.InsertOnSubmit(b);
                }
                else
                {
                    trans.Amount = -b.Amount;
                    trans.CreatedDate = DateTime.Now;
                }
            }
            BDC.SubmitChanges();
        }

        public static bool IsBonusConfirmed(long oid)
        {
            var o = VDC.OrderHeaders.SingleOrDefault(p => p.OrderHeaderId == oid);
            if (o == null) return false;
            return (o.BonusAmount == 0 || (o.BonusStatus == OrderBonusStatus.Confirmed));
        }

        public static bool IsBonusNeedConfirm(long oid)
        {
            var o = VDC.OrderHeaders.SingleOrDefault(p => p.OrderHeaderId == oid);
            if (o == null) return false;
            return (o.BonusAmount != 0 && (o.BonusStatus != OrderBonusStatus.Confirmed));
        }
    }
}