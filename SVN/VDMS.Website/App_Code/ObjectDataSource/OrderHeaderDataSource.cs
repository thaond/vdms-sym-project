using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using NHibernate.Expression;
using VDMS.Common.Utils;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;
using VDMS.Helper;
using VDMS.I.Entity;
using VDMS.I.Linq;
using VDMS.II.Linq;
using System.Collections;
//using VDMS.I.Vehicle;

namespace VDMS.I.ObjectDataSource
{
    public class OrderHeaderDataSource
    {
        private static int count = 0;
        public static int SelectCount(int maximumRows, int startRowIndex, string sFromDate, string sToDate, string BranchCode, string StatusCode, string OrderNumber)
        {
            return count;
        }

        public static List<Orderheader> Select(int maximumRows, int startRowIndex, string sFromDate, string sToDate, string BranchCode, string StatusCode, string OrderNumber)
        {
            DateTime fromDate, toDate;
            DateTime.TryParse(sFromDate, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out fromDate);
            DateTime.TryParse(sToDate, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out toDate);

            IDao<Orderheader, long> dao = DaoFactory.GetDao<Orderheader, long>();

            List<ICriterion> cri = new List<ICriterion>();
            if (!string.IsNullOrEmpty(StatusCode)) cri.Add(Expression.Eq("Status", int.Parse(StatusCode)));
            else cri.Add(Expression.In("Status", new int[] { 0, 1, 2, 4 }));
            cri.Add(Expression.Between("Orderdate", fromDate.Date, toDate.Date.AddDays(1)));
            //cri.Add(Expression.Eq("Shippingto", BranchCode));
            if (!string.IsNullOrEmpty(BranchCode)) cri.Add(Expression.Eq("Secondaryshippingcode", BranchCode));
            //if (!string.IsNullOrEmpty(BranchCode)) cri.Add(Expression.Eq("Secondaryshippingcode", BranchCode));
            if (UserHelper.IsDealer) cri.Add(Expression.Eq("Dealercode", UserHelper.DealerCode));

            if (!string.IsNullOrEmpty(OrderNumber)) cri.Add(Expression.InsensitiveLike("Ordernumber", string.Format("%{0}%", OrderNumber.Trim())));
            dao.SetCriteria(cri.ToArray());
            //if (!string.IsNullOrEmpty(StatusCode)) dao.SetCriteria(new ICriterion[] { Expression.Eq("Status", int.Parse(StatusCode)), Expression.Between("Createddate", fromDate.Date, toDate.Date.AddDays(1)), Expression.Eq("Shippingto", BranchCode), Expression.InsensitiveLike("Ordernumber", OrderNumber) });
            //else dao.SetCriteria(new ICriterion[] { Expression.In("Status", new int[] { 0, 1, 2, 4 }), Expression.Between("Createddate", fromDate.Date, toDate.Date.AddDays(1)), Expression.Eq("Shippingto", BranchCode), Expression.InsensitiveLike("Ordernumber", OrderNumber) });
            dao.SetOrder(new Order[] { Order.Asc("Orderdate"), Order.Asc("Ordertimes") });
            List<Orderheader> list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
            count = dao.ItemCount;
            HttpContext.Current.Items["rowCount"] = count;
            return list;
        }

        public static Orderheader GetById(string orderId)
        {
            long id;
            long.TryParse(orderId, out id);
            IDao<Orderheader, long> dao = DaoFactory.GetDao<Orderheader, long>();
            return dao.GetById(id, false);
        }
        public static OrderHeader GetOrderHeaderById(long orderId)
        {
            var db = DCFactory.GetDataContext<VehicleDataContext>();
            return db.OrderHeaders.SingleOrDefault(p => p.OrderHeaderId == orderId);
        }
        int _orderCheckCount;
        public int CountCheckOrder(string dealerCode, string wCode, string fromDate, string toDate, string status, string orderNoFrom, string orderNoTo, string area)
        {
            return _orderCheckCount;
        }

        public IList CheckOrder(int maximumRows, int startRowIndex, string dealerCode, string wCode, string fromDate, string toDate, string status, string orderNoFrom, string orderNoTo, string area)
        {
            var db = DCFactory.GetDataContext<VehicleDataContext>();
            DateTime dtFrom = DataFormat.DateFromString(fromDate);
            DateTime dtTo = DataFormat.DateFromString(toDate);
            if (dtTo == DateTime.MinValue) dtTo = DateTime.Now;

            var query = db.OrderHeaders.OrderBy(p => p.OrderDate).Where(h => h.DatabaseCode == UserHelper.DatabaseCode)
                                       .Where(h => h.OrderDate >= dtFrom && h.OrderDate < dtTo.AddDays(1))
                                       .Where(h=>h.Status != (int)VDMS.I.Vehicle.OrderStatus.Deleted);
            if (!string.IsNullOrEmpty(area)) query = query.Where(h => h.AreaCode == area);
            if (status != "0") query = query.Where(p => p.DeliveredStatus == int.Parse(status));
            if (!string.IsNullOrEmpty(dealerCode)) query = query.Where(p => p.DealerCode.Contains(dealerCode.Trim().ToUpper()));
            if (!string.IsNullOrEmpty(orderNoFrom)) query = query.Where(p => p.OrderNumber.CompareTo(orderNoFrom.Trim().ToUpper()) >= 0);
            if (!string.IsNullOrEmpty(orderNoTo)) query = query.Where(p => p.OrderNumber.CompareTo(orderNoTo.Trim().ToUpper()) <= 0);
            if (!string.IsNullOrEmpty(wCode)) query = query.Where(p => p.SecondaryShippingCode == wCode);

            _orderCheckCount = query.Count();
            if ((startRowIndex >= 0) && (maximumRows >= 0)) query = query.Skip(startRowIndex).Take(maximumRows);
            var queryL = query.ToList();
            var shipping = (from s in db.ShippingDetails
                            where 
                            (from q in queryL select q.OrderNumber).Contains(s.OrderNumber)
                           select s).ToList();
            var info = (from sf in db.ShippingInfos
                        where 
                        (from q in queryL select q.OrderNumber).Contains(sf.OrderNumber)
                        select sf).ToList();
            var orderD = (from d in db.OrderDetails
                          where 
                          (from qu in queryL select qu.OrderHeaderId).Contains(d.OrderHeaderId)
                         select d).ToList();

            var invoice = (from si in db.SaleInvoices
                           where 
                           (from ss in shipping select ss.EngineNumber).Contains(si.EngineNumber)
                           select si).ToList();
            

            var res = queryL.Select(h => new
            {
                h.OrderHeaderId,
                h.OrderNumber,
                h.OrderDate,
                h.DeliveredStatus,
                h.DealerCode,
                h.OrderTimes,
                h.Status,
                h.ShippingTo,
                h.SecondaryShippingCode,
                Items = from d in orderD
                        where d.OrderHeaderId == h.OrderHeaderId
                        select new
                        {
                            d.ItemCode,
                            d.OrderQty,
                            ImportQuantity = shipping.Count(p => p.OrderNumber == h.OrderNumber && p.ItemCode == d.ItemCode && p.ProductInstanceId != null),
                            ShippingQuantity = info.Where(p => p.OrderNumber == h.OrderNumber && p.ItemCode == d.ItemCode).Sum(p => p.ShippingQuantity),
                            SoldQuantity = shipping.Where(p => p.OrderNumber == h.OrderNumber && p.ItemCode == d.ItemCode).Join(invoice, sd => sd.EngineNumber, iv => iv.EngineNumber, (sd, iv) => new { EngineNumber = sd.EngineNumber }).Count(),
                            //SoldQuantity = invoice.Where(p => p.OrderNumber == h.OrderNumber && p.ItemCode == d.ItemCode)
                        }
            }).ToList();
            

            return res;
        }
    }
}
