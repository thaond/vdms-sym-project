using System;
using System.Linq;
using System.Collections.Generic;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;
using VDMS.Helper;
using VDMS.II.Linq;
using VDMS.I.Linq;
using VDMS.Common.Utils;
using VDMS.I.Entity;

/// <summary>
/// Summary description for ExchangeVoucherBO
/// </summary>
namespace VDMS.I.Service
{
    public class ExchangeVoucherBO
    {
        public ExchangeVoucherBO()
        {
        }
        public static string GenExchangeNumber(string dealerCode)
        {
            DateTime dt = DateTime.Now;
            IDao<Exchangevoucherheader, string> dao = DaoFactory.GetDao<Exchangevoucherheader, string>();
            string num = "D" + UserHelper.DatabaseCode + "-" + dealerCode + dt.ToString("yy") + dt.ToString("MM");

            dao.SetCriteria(new ICriterion[] { Expression.Like("Id", num + "%") });
            int count = dao.GetAll().Count + 1;
            num += count.ToString().PadLeft(4, '0');
            return num;
        }
        public static bool CheckExistExchangeVoucher()
        {
            bool res = false;
            IDao<Exchangevoucherheader, string> dao = DaoFactory.GetDao<Exchangevoucherheader, string>();
            res = dao.GetAll().Count > 0;
            return res;
        }
        public static DateTime GetFromDate(string dealerCode)
        {
            DateTime res = DateTime.Now.AddDays(-3);

            IDao<Exchangevoucherheader, string> dao = DaoFactory.GetDao<Exchangevoucherheader, string>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Dealercode", dealerCode) });
            dao.SetOrder(new Order[] { Order.Desc("Lastprocesseddate") });
            List<Exchangevoucherheader> list = dao.GetPaged(0, 1);
            if (list.Count > 0)
            {
                res = list[0].Lastprocesseddate;
            }
            return res;
        }
        public static List<Exchangepartdetail> GetExPartDetailByExPartHeader(long idPartHeader)
        {
            List<Exchangepartdetail> list = new List<Exchangepartdetail>();

            IDao<Exchangepartheader, long> daoEph = DaoFactory.GetDao<Exchangepartheader, long>();
            Exchangepartheader eph = daoEph.GetById(idPartHeader, false); //true -> false

            IDao<Exchangepartdetail, long> daoEpd = DaoFactory.GetDao<Exchangepartdetail, long>();
            daoEpd.SetCriteria(new ICriterion[] { Expression.Eq("Exchangepartheader", eph) });
            list = daoEpd.GetAll();
            return list;
        }
        public static List<Exchangepartdetail> GetExPartDetailByExPartHeader(Exchangepartheader eph)
        {
            List<Exchangepartdetail> list = new List<Exchangepartdetail>();

            IDao<Exchangepartdetail, long> daoEpd = DaoFactory.GetDao<Exchangepartdetail, long>();
            daoEpd.SetCriteria(new ICriterion[] { Expression.Eq("Exchangepartheader", eph) });
            list = daoEpd.GetAll();
            return list;
        }
        public static Exchangepartheader GetExPartHeader(long idPartHeader)
        {
            IDao<Exchangepartheader, long> daoEph = DaoFactory.GetDao<Exchangepartheader, long>();
            Exchangepartheader eph = daoEph.GetById(idPartHeader, true); //true -> false
            return eph;
        }


        #region Devart

        public static ServiceDataContext DC
        {
            get { return DCFactory.GetDataContext<ServiceDataContext>(); }
        }

        int _exchangeHeadersCount;
        public int CountExchangeHeaders(string vnFrom, string vnTo, string propNo, string enNo, string dCode,
            string repairFromDt, string repairToDt, string confirmFromDt, string confirmToDt, int? status, ExchangePartHeader pageSum)
        {
            return _exchangeHeadersCount;
        }
        public List<ExchangePartHeader> FindExchangeHeaders(string vnFrom, string vnTo, string propNo, string enNo, string dCode,
            string repairFromDt, string repairToDt, string confirmFromDt, string confirmToDt, int? status, out ExchangePartHeader pageSum, int maximumRows, int startRowIndex)
        {
            DateTime dtRF = DataFormat.DateFromString(repairFromDt);
            DateTime dtRT = DataFormat.DateFromString(repairToDt); //if (dtRT == DateTime.MinValue) dtRT = DateTime.Now.Date;
            DateTime dtCF = DataFormat.DateFromString(confirmFromDt);
            DateTime dtCT = DataFormat.DateFromString(confirmToDt); //if (dtCT == DateTime.MinValue) dtCT = DateTime.Now.Date;

            var dc = ExchangeVoucherBO.DC;
            var query = dc.ExchangePartHeaders.AsQueryable();
            if (!string.IsNullOrEmpty(vnFrom)) query = query.Where(h => h.VoucherNumber.CompareTo(vnFrom) >= 0);
            if (!string.IsNullOrEmpty(vnTo)) query = query.Where(h => h.VoucherNumber.CompareTo(vnTo) <= 0);
            if (!string.IsNullOrEmpty(propNo)) query = query.Where(h => h.FinalVoucherNumber.Contains(propNo));
            if (!string.IsNullOrEmpty(enNo)) query = query.Where(h => h.EngineNumber.Contains(enNo));
            if (!string.IsNullOrEmpty(dCode)) query = query.Where(h => h.DealerCode == dCode);
            if (dtCF > DateTime.MinValue) query = query.Where(h => h.LastProcessedDate >= dtCF);
            if (dtCT > DateTime.MinValue) query = query.Where(h => h.LastProcessedDate < dtCT.AddDays(1));
            if (dtRF > DateTime.MinValue) query = query.Where(h => h.ServiceHeader.ServiceDate >= dtRF);
            if (dtRT > DateTime.MinValue) query = query.Where(h => h.ServiceHeader.ServiceDate < dtRT.AddDays(1));
            if ((status.HasValue) && (status.Value >= 0)) query = query.Where(h => h.Status == (int)status);

            _exchangeHeadersCount = query.Count();

            if ((startRowIndex >= 0) && (maximumRows > 0)) query = query.Skip(startRowIndex).Take(maximumRows);
            var res = query.ToList();

            //summarization
            pageSum = new ExchangePartHeader();

            foreach (var item in res)
            {
                foreach (var d in item.ExchangePartDetails)
                {
                    d.LoadWarrCond(dc);
                }
                item.DoSummary();
                if (pageSum.DealerCode == null)
                    pageSum.DealerCode = item.DealerCode;
                else
                    if (!pageSum.DealerCode.Contains(item.DealerCode)) pageSum.DealerCode = string.Concat(pageSum.DealerCode, "@", item.DealerCode);
            }
            pageSum.TotalFeeM = res.Sum(p => p.TotalFeeM);
            pageSum.TotalFeeO = res.Sum(p => p.TotalFeeO);
            pageSum.TotalPartCostM = res.Sum(p => p.TotalPartCostM);
            pageSum.TotalPartCostO = res.Sum(p => p.TotalPartCostO);
            pageSum.TotalQuantityM = res.Sum(p => p.TotalQuantityM);
            pageSum.TotalQuantityO = res.Sum(p => p.TotalQuantityO);
            pageSum.ProposeFeeAmount = res.Sum(p => p.ProposeFeeAmount);

            return res;
        }

        public void UpdateVoid(long ExchangePartHeaderId)
        {
            //DC.ExchangePartDetails.First().
        }

        public static VerifyExchangeErrorCode ChangeExchangeStatus(long hid, ExchangeVoucherStatus status)
        {
            return ChangeExchangeStatus(hid, status, true);
        }
        public static VerifyExchangeErrorCode ChangeExchangeStatus(long hid, ExchangeVoucherStatus status, bool overWrite)
        {
            var eh = DC.ExchangePartHeaders.SingleOrDefault(h => h.ExchangePartHeaderId == hid);
            if (eh != null)
            {
                if (!overWrite && eh.Status != (int)ExchangeVoucherStatus.Sent) return VerifyExchangeErrorCode.OK;
                //var res = VDMS.I.ObjectDataSource.ExchangeVoucherHeaderDataSource.ChangeExchangeVoucherStatus(eh.VoucherNumber, status);
                var res = VDMS.I.ObjectDataSource.ExchangeVoucherHeaderDataSource.ChangeExchangeVoucherStatusD(eh, status);
                return res;
            }
            else return VerifyExchangeErrorCode.ExchangePartHeaderNotFound;
        }

        public static ExchangePartHeader SummaryExchangeHeaders(string vnFrom, string vnTo, string propNo, string enNo, string dCode,
            string repairFromDt, string repairToDt, string confirmFromDt, string confirmToDt, int? status)
        {
            DateTime dtRF = DataFormat.DateFromString(repairFromDt);
            DateTime dtRT = DataFormat.DateFromString(repairToDt); //if (dtRT == DateTime.MinValue) dtRT = DateTime.Now.Date;
            DateTime dtCF = DataFormat.DateFromString(confirmFromDt);
            DateTime dtCT = DataFormat.DateFromString(confirmToDt); //if (dtCT == DateTime.MinValue) dtCT = DateTime.Now.Date;

            var dc = ExchangeVoucherBO.DC;
            var query = dc.ExchangePartDetails.AsQueryable();
            if (!string.IsNullOrEmpty(vnFrom)) query = query.Where(p => p.ExchangePartHeader.VoucherNumber.CompareTo(vnFrom) >= 0);
            if (!string.IsNullOrEmpty(vnTo)) query = query.Where(p => p.ExchangePartHeader.VoucherNumber.CompareTo(vnTo) <= 0);
            if (!string.IsNullOrEmpty(propNo)) query = query.Where(p => p.ExchangePartHeader.FinalVoucherNumber.Contains(propNo));
            if (!string.IsNullOrEmpty(enNo)) query = query.Where(p => p.ExchangePartHeader.EngineNumber.Contains(enNo));
            if (!string.IsNullOrEmpty(dCode)) query = query.Where(p => p.ExchangePartHeader.DealerCode == dCode);
            if (dtCF > DateTime.MinValue) query = query.Where(p => p.ExchangePartHeader.LastProcessedDate >= dtCF);
            if (dtCT > DateTime.MinValue) query = query.Where(p => p.ExchangePartHeader.LastProcessedDate < dtCT.AddDays(1));
            if (dtRF > DateTime.MinValue) query = query.Where(p => p.ExchangePartHeader.ServiceHeader.ServiceDate >= dtRF);
            if (dtRT > DateTime.MinValue) query = query.Where(p => p.ExchangePartHeader.ServiceHeader.ServiceDate < dtRT.AddDays(1));
            if (status != null) query = query.Where(p => p.ExchangePartHeader.Status == (int)status);
            var cQuery = query.Where(p => p.ExchangePartHeader.Status == (int)ExchangeVoucherStatus.Approved);

            ExchangePartHeader res = new ExchangePartHeader();
            res.TotalFeeM = cQuery.Sum(p => p.TotalFeeM);
            res.TotalQuantityM = cQuery.Sum(p => p.PartQtyM);
            res.TotalPartCostM = cQuery.Sum(p => p.UnitPriceM * p.PartQtyM);

            res.TotalFeeO = query.Sum(p => p.TotalFeeO);
            res.TotalQuantityO = query.Sum(p => p.PartQtyO);
            res.TotalPartCostO = query.Sum(p => p.UnitPriceO * p.PartQtyO);
            res.ProposeFeeAmount = query.GroupBy(p => new { HId = p.ExchangePartHeaderId, ProposeFeeAmount = p.ExchangePartHeader.ProposeFeeAmount }, (g, result) => new { ProposeFeeAmount = g.ProposeFeeAmount }).Sum(p => p.ProposeFeeAmount);
            return res;
        }

        #endregion
    }
}