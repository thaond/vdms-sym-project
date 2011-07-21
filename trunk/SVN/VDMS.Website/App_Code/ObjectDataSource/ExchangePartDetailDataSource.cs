using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using NHibernate;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.IDAL.Interface;
using VDMS.Helper;
using VDMS.I.Service;
using VDMS.Common.Utils;

/// <summary>
/// Summary description for ExchangePartDetailDataSource
/// </summary>
namespace VDMS.I.ObjectDataSource
{
    public class ExchangePartDetailDataSource
    {
        public ExchangePartDetailDataSource()
        {
        }
        private int count = 0;
        public int SelectCount(int maximumRows, int startRowIndex, string fromDate, string toDate, string processFromDate, string processToDate, int status)
        {
            return count;
        }

        public IList<Exchangepartdetail> Select(int maximumRows, int startRowIndex, string fromDate, string toDate, string processFromDate, string processToDate, int status)
        {
            // variables
            DateTime dtfromDate = DataFormat.DateFromString(fromDate),
                     dttoDate = DataFormat.DateFromString(toDate),
                     dtProcessFromDate = DataFormat.DateFromString(processFromDate),
                     dtProcessToDate = DataFormat.DateFromString(processToDate);
            if (dttoDate == DateTime.MinValue) dttoDate = DateTime.Now;

            IList<Exchangepartdetail> list;

            ISession session = NHibernateSessionManager.Instance.GetSession();
            ICriteria crit = session.CreateCriteria(typeof(Exchangepartdetail)).CreateCriteria("Exchangepartheader", "h").CreateCriteria("Exchangevoucherheader", "v");
            ICriteria critCount = session.CreateCriteria(typeof(Exchangepartdetail)).CreateCriteria("Exchangepartheader", "h").CreateCriteria("Exchangevoucherheader", "v");

            // area of exchange part header
            List<string> areas = AreaHelper.Area;
            if ((areas != null) && (areas.Count > 0))
            {
                crit.Add(Expression.In("h.Areacode", areas));
                critCount.Add(Expression.In("h.Areacode", areas));
            }

            // dealer code of exchange part header
            if (UserHelper.IsDealer)
            {
                crit.Add(Expression.Eq("h.Dealercode", UserHelper.DealerCode));
                critCount.Add(Expression.Eq("h.Dealercode", UserHelper.DealerCode));
            }

            // exchange date
            crit.Add(Expression.Between("h.Exchangeddate", dtfromDate, dttoDate));
            critCount.Add(Expression.Between("h.Exchangeddate", dtfromDate, dttoDate));

            if (dtProcessFromDate != DateTime.MinValue)
            {
                crit.Add(Expression.Gt("v.Lastprocesseddate", dtProcessFromDate));
                critCount.Add(Expression.Gt("v.Lastprocesseddate", dtProcessFromDate));
            }
            if (dtProcessToDate != DateTime.MinValue)
            {
                crit.Add(Expression.Lt("v.Lastprocesseddate", dtProcessToDate));
                critCount.Add(Expression.Lt("v.Lastprocesseddate", dtProcessToDate));
            }

            // status of exchange part header
            if ((int)ExchangeVoucherStatus.All != status)
            {
                crit.Add(Expression.Eq("h.Status", status));
                critCount.Add(Expression.Eq("h.Status", status));
            }
            else
            {
                crit.Add(Expression.Ge("h.Status", 0));   // ko lay cac temp voucher
                critCount.Add(Expression.Ge("h.Status", 0));
            }

            // get paged result
            if (maximumRows > 0)
            {
                // get items count 
                count = (int)critCount.SetProjection(Projections.RowCount()).UniqueResult();
                // set page info
                crit.SetFirstResult((startRowIndex >= count) ? 0 : startRowIndex);
                crit.SetMaxResults(maximumRows);
            }

            // output result
            HttpContext.Current.Items["rowCount"] = count;
            list = crit.List<Exchangepartdetail>();
            return list;
        }

        // phuong an nay neu listEph.Count > 1000 ==> die (max parameter limitted)
        //public List<Exchangepartdetail> _old_Select(int maximumRows, int startRowIndex, string fromDate, string toDate, int status)
        //{
        //    Exchangepartdetail epd = new Exchangepartdetail();
        //    List<Exchangepartdetail> list = new List<Exchangepartdetail>();
        //    IDao<Exchangepartdetail, long> dao = DaoFactory.GetDao<Exchangepartdetail, long>();

        //    DateTime dtfromDate, dttoDate;
        //    DateTime.TryParse(fromDate, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dtfromDate);
        //    DateTime.TryParse(toDate, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dttoDate);
        //    List<Exchangepartheader> listEph = new List<Exchangepartheader>();
        //    // nmchi fix: area
        //    //listEph = ExchangePartHeaderDataSource.Select(dtfromDate, dttoDate);
        //    listEph = ExchangePartHeaderDataSource.Select(dtfromDate, dttoDate, status, true);
        //    // end: area

        //    if (listEph.Count > 0)
        //    {
        //        dao.SetCriteria(new ICriterion[] { Expression.In("Exchangepartheader", listEph) });
        //        if (maximumRows != 0)
        //        {
        //            list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
        //            count = dao.ItemCount;
        //            if (startRowIndex >= count)
        //            {
        //                list = dao.GetPaged(0, maximumRows);
        //                count = dao.ItemCount;
        //            }
        //        }
        //        else
        //        {
        //            list = dao.GetAll();
        //        }
        //    }
        //    HttpContext.Current.Items["rowCount"] = count;
        //    return list;
        //}

        public static IList<Exchangepartdetail> GetByExchangeHeader(long headerId)
        {
            IList<Exchangepartdetail> list;
            IDao<Exchangepartdetail, long> dao = DaoFactory.GetDao<Exchangepartdetail, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Exchangepartheader.Id", headerId) });
            list = dao.GetAll();
            return list;
        }
    }
}
