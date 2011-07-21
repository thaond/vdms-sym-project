using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;
using VDMS.Helper;
using VDMS.I.Service;
using VDMS.II.Linq;
using VDMS.I.Linq;
using VDMS.Common.Utils;

namespace VDMS.I.ObjectDataSource
{
    public class SVListTotal
    {
        public decimal ServicePartAmount { get; set; }
        public double ServiceFee { get; set; }
        public decimal WarrantyPartAmount { get; set; }
        public decimal WarrantyFee { get; set; }
    }

    public class ServiceHeaderDataSource
    {
        private int count = 0;
        public int SelectTempCount(int maximumRows, int startRowIndex, string EngineNo, string fromDate, string toDate)
        {
            return count;
        }
        public IList<Serviceheader> SelectTemp(int maximumRows, int startRowIndex, string EngineNo, string fromDate, string toDate)
        {
            DateTime from, to;
            IDao<Serviceheader, long> dao = DaoFactory.GetDao<Serviceheader, long>();
            List<ICriterion> crit = new List<ICriterion>();

            DateTime.TryParse(fromDate, out from);
            DateTime.TryParse(toDate, out to);

            crit.Add(Expression.InsensitiveLike("Enginenumber", EngineNo, MatchMode.Anywhere));
            if (from > DateTime.MinValue) crit.Add(Expression.Ge("Servicedate", from));
            if (to > DateTime.MinValue) crit.Add(Expression.Le("Servicedate", to));
            if (UserHelper.IsDealer) crit.Add(Expression.Eq("Dealercode", UserHelper.DealerCode));
            crit.Add(Expression.Eq("Status", (int)ServiceStatus.Temp));

            if (crit.Count > 0) dao.SetCriteria(crit.ToArray());
            dao.SetOrder(new Order[] { Order.Desc("Servicedate"), Order.Desc("Dealercode") });

            List<Serviceheader> list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
            count = dao.ItemCount;

            HttpContext.Current.Items["TempRowCount"] = count;
            return list;
        }

        public int CountSVList(string dCode, string wCode, string SNoFrom, string SNoTo, string EngineNo, string cusName, string buyFrom, string buyTo, string repairFrom, string repairTo)
        {
            return count;
        }
        public static IQueryable<VDMS.I.Entity.ServiceHeader> BuildSVListQuery(string dCode, string wCode, string SNoFrom, string SNoTo, string EngineNo, string cusName, string buyFrom, string buyTo, string repairFrom, string repairTo)
        {
            DateTime dtBF = DataFormat.DateFromString(buyFrom);
            DateTime dtBT = DataFormat.DateFromString(buyTo);
            DateTime dtRF = DataFormat.DateFromString(repairFrom);
            DateTime dtRT = DataFormat.DateFromString(repairTo);

            var sdc = DCFactory.GetDataContext<ServiceDataContext>();
            var query = sdc.ServiceHeaders
                            .OrderBy(s => s.ServiceSheetNumber).OrderBy(s => s.ServiceDate)
                            .Where(s => s.DealerCode == dCode && s.Status >= 0);

            if (!string.IsNullOrEmpty(SNoFrom)) query = query.Where(s => s.BranchCode == wCode);
            if (!string.IsNullOrEmpty(SNoFrom)) query = query.Where(s => s.ServiceSheetNumber.CompareTo(SNoFrom) >= 0);
            if (!string.IsNullOrEmpty(SNoTo)) query = query.Where(s => s.ServiceSheetNumber.CompareTo(SNoTo) <= 0);
            if (!string.IsNullOrEmpty(EngineNo)) query = query.Where(s => s.EngineNumber.Contains(EngineNo));
            if (!string.IsNullOrEmpty(cusName)) query = query.Where(s => s.Customer.FullName.ToLower().Contains(cusName.ToLower()));
            if (dtBF != DateTime.MinValue) query = query.Where(s => s.PurchaseDate >= dtBF);
            if (dtBT != DateTime.MinValue) query = query.Where(s => s.PurchaseDate <= dtBT);
            if (dtRF != DateTime.MinValue) query = query.Where(s => s.ServiceDate >= dtRF);
            if (dtRT != DateTime.MinValue) query = query.Where(s => s.ServiceDate <= dtRT);

            return query;
        }

        public object SelectSVList(int maximumRows, int startRowIndex, string dCode, string wCode, string SNoFrom, string SNoTo, string EngineNo, string cusName, string buyFrom, string buyTo, string repairFrom, string repairTo)
        {
            var sdc = DCFactory.GetDataContext<ServiceDataContext>();
            var query = BuildSVListQuery(dCode, wCode, SNoFrom, SNoTo, EngineNo, cusName, buyFrom, buyTo, repairFrom, repairTo);

            count = query.Count();
            if ((startRowIndex >= 0) && (maximumRows > 0)) query = query.Skip(startRowIndex).Take(maximumRows);

            //VDMS.I.Entity.ExchangePartDetail d;d.WarrantySpareAmountM
            var res = query.Select(s => new
            {
                ExchangeVoucherNumber = sdc.ExchangePartHeaders.SingleOrDefault(e => e.ServiceHeaderId == s.ServiceHeaderId).VoucherNumber,
                AllServiceSpareAmountM = s.ServiceDetails.Sum(sd => sd.UnitPrice * sd.PartQty),
                AllWarrantySpareAmountM = sdc.ExchangePartHeaders.SingleOrDefault(e => e.ServiceHeaderId == s.ServiceHeaderId).ExchangePartDetails.Sum(ed => ed.UnitPriceM * ed.PartQtyM),
                AllWarrantyFeeAmountM = sdc.ExchangePartHeaders.SingleOrDefault(e => e.ServiceHeaderId == s.ServiceHeaderId).ExchangePartDetails.Sum(ed => ed.TotalFeeM),
                ServiceSheetNumber = s.ServiceSheetNumber,
                ServiceFee = s.FeeAmount,
                CusName = s.Customer.FullName,
                BuyDate = s.PurchaseDate,
                EngineNumber = s.EngineNumber,
                ServiceList = s.ServiceDetails,
                ExchangeList = sdc.ExchangePartHeaders.SingleOrDefault(e => e.ServiceHeaderId == s.ServiceHeaderId).ExchangePartDetails
            });
            return res;
        }

        public static SVListTotal SumSVList(string dCode, string wCode, string SNoFrom, string SNoTo, string EngineNo, string cusName, string buyFrom, string buyTo, string repairFrom, string repairTo)
        {
            var sdc = DCFactory.GetDataContext<ServiceDataContext>();
            var queryS = BuildSVListQuery(dCode, wCode, SNoFrom, SNoTo, EngineNo, cusName, buyFrom, buyTo, repairFrom, repairTo);
            var queryE = queryS.Join(sdc.ExchangePartDetails, s => s.ServiceHeaderId, e => e.ExchangePartHeader.ServiceHeaderId,
                                (s, e) => e);
            var res = new SVListTotal()
            {
                ServicePartAmount = queryS.Join(sdc.ServiceDetails, h => h.ServiceHeaderId, d => d.ServiceHeaderId, (h, d) => d)
                                          .Sum(sd => sd.PartQty * sd.UnitPrice),
                ServiceFee = queryS.Sum(s => s.FeeAmount),
                WarrantyPartAmount = queryE.Sum(e => e.PartQtyM * e.UnitPriceM),
                WarrantyFee = queryE.Sum(e => e.TotalFeeM),
            };

            return res;
        }
    }
}
