using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.I.Entity;
using VDMS.Bonus.Linq;
using VDMS.Bonus.Entity;
using VDMS.II.Linq;
using VDMS.I.Linq;

namespace VDMS.I.Vehicle
{
    [DataObject]
    public class OrderBonusDAO
    {
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<SaleOrderPayment> GetCRPayment(string dealerCode, int maximumRows, int startRowIndex)
        {
            var dealer = string.IsNullOrEmpty(dealerCode) ? UserHelper.DealerCode : dealerCode;
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            var query = dc.SaleOrderPayments.Where(p => p.OrderHeader.DealerCode == dealer &&
                                                        p.PaymentType == OrderPaymentType.ConsignRemain).ToList();
            foreach (var p in query)
            {
                p.Amount = p.SaleOrderPaymentTransHistories.Count() == 0 ? p.Amount : p.Amount + p.SaleOrderPaymentTransHistories.Sum(t => t.Amount);
            }
            query = query.Where(p => p.Amount > 0).ToList();
            _crCount = query.Count();
            if (maximumRows > 0) query = query.Skip(startRowIndex).Take(maximumRows).ToList();
            return query.AsEnumerable();
        }

        int _crCount;
        public int CountCRPayment(string dealerCode, int maximumRows, int startRowIndex)
        {
            return _crCount;
        }

        public static SaleOrderPayment GetOrderPayment(long id)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            return dc.SaleOrderPayments.SingleOrDefault(p => p.OrderPaymentId == id);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public object GetBonusMoney(string dealerCode, int maximumRows, int startRowIndex)
        {
            var dealer = string.IsNullOrEmpty(dealerCode) ? UserHelper.DealerCode : dealerCode;
            var dc = DCFactory.GetDataContext<BonusDataContext>();

            var query = dc.BonusPlanDetails.Where(d => d.DealerCode == dealer &&
                                                       d.Status == BonusStatus.Confirmed).ToList();
            foreach (var p in query)
            {
                p.Amount = p.BonusTransactions.Where(t => t.TransactionType == BonusTransactionType.OrderSubstract).Count() == 0 ? p.Amount : p.Amount + (long)p.BonusTransactions.Where(t => t.TransactionType == BonusTransactionType.OrderSubstract).Sum(t => t.Amount);
            }
            query = query.Where(d => d.Amount > 0).ToList();
            _bmCount = query.Count;
            if (maximumRows > 0) query = query.Skip(startRowIndex).Take(maximumRows).ToList();
            return query.Select(d => new { d.BonusPlanDetailId, 
                                           d.Amount,
                                           d.Balance, 
                                           d.BonusDate, 
                                           d.BonusSource.BonusSourceName, 
                                           d.DealerCode, 
                                           d.Description });
        }

        int _bmCount;
        public int CountBonusMoney(string dealerCode, int maximumRows, int startRowIndex)
        {
            return _bmCount;
        }
    }
}