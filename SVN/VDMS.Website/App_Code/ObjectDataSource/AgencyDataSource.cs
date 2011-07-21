using System;
using System.Collections.Generic;
using System.Web;
using NHibernate;
using NHibernate.Expression;
using VDMS.Common.Utils;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.IDAL.Interface;
using VDMS.Helper;

/// <summary>
/// Summary description for AgencyDataSource
/// </summary>
namespace VDMS.I.ObjectDataSource
{
    public class AgencyDataSource
    {
        private int count = 0;

        // return maximum record
        public int SelectCount(int maximumRows, int startRowIndex, string fromDate, string toDate, string status)
        {
            return count;
        }

        public ICriteria CreateQuery(string fromDate, string toDate, string status)
        {
            DateTime dtFrom = DataFormat.DateFromString(fromDate),
                     dtTo = DataFormat.DateFromString(toDate);

            ISession sess = NHibernateSessionManager.Instance.GetSession();
            ICriteria crit = sess.CreateCriteria(typeof(Shippingdetail));
            switch (int.Parse(status))
            {
                case 1:
                    crit.Add(Expression.Sql("Trim(EXCEPTION) is not null"));
                    break;
                case 2:
                    crit.Add(Expression.Sql("Trim(EXCEPTION) is not null and VMEPRESPONSEDATE < '1-Jan-1900'"));
                    break;
                case 3:
                    crit.Add(Expression.Sql("Trim(EXCEPTION) is not null and VMEPRESPONSEDATE > '1-Jan-1900'"));
                    break;
            }
            
            crit = crit.CreateCriteria("Shippingheader").Add(Expression.In("Areacode", AreaHelper.Area));
            if (dtFrom != DateTime.MinValue) crit.Add(Expression.Ge("Createddate", dtFrom));
            if (dtTo != DateTime.MinValue) crit.Add(Expression.Le("Createddate", dtTo));
         
            return crit;
        }
        public IList<Shippingdetail> Select(int maximumRows, int startRowIndex, string fromDate, string toDate, string status)
        {
            ICriteria crit = CreateQuery(fromDate, toDate, status);
            ICriteria critCount = CreateQuery(fromDate, toDate, status);

            critCount.SetProjection(Projections.RowCount());
            count = (int)critCount.UniqueResult();
            HttpContext.Current.Items["rowCount"] = count;

            if ((startRowIndex >= 0) && (maximumRows > 0))
            {
                crit.SetFirstResult(startRowIndex);
                crit.SetMaxResults(maximumRows);
            }

            return crit.List<Shippingdetail>();
        }
        //public List<Shippingdetail> Select_o(int maximumRows, int startRowIndex, string fromDate, string toDate, string status)
        //{
        //    List<Shippingdetail> list = new List<Shippingdetail>();
        //    IDao<Shippingdetail, long> dao = DaoFactory.GetDao<Shippingdetail, long>();
        //    List<ShippingHeader> listsh = GetShippingHeaderBy(fromDate, toDate);
        //    dao.SetCriteria(new ICriterion[] { Expression.In("Shippingheader", listsh) });
        //    switch (int.Parse(status))
        //    {
        //        case 1:
        //            dao.SetCriteria(new ICriterion[] { Expression.In("Shippingheader", listsh), Expression.Sql("Trim(EXCEPTION) is not null") });
        //            break;
        //        case 2:
        //            dao.SetCriteria(new ICriterion[] { Expression.In("Shippingheader", listsh), Expression.Sql("Trim(EXCEPTION) is not null and VMEPRESPONSEDATE < '1-Jan-1900'") });
        //            break;
        //        case 3:
        //            dao.SetCriteria(new ICriterion[] { Expression.In("Shippingheader", listsh), Expression.Sql("Trim(EXCEPTION) is not null and VMEPRESPONSEDATE > '1-Jan-1900'") });
        //            break;
        //        default:
        //            break;
        //    }

        //    list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
        //    count = dao.ItemCount;
        //    if (startRowIndex >= count)
        //    {
        //        list = dao.GetPaged(0, maximumRows);
        //        count = dao.ItemCount;
        //    }
        //    HttpContext.Current.Items["rowCount"] = count;
        //    return list;
        //}
        //public List<ShippingHeader> GetShippingHeaderBy(string fromDate, string toDate)
        //{
        //    List<ShippingHeader> listsh = new List<ShippingHeader>();
        //    IDao<ShippingHeader, long> daosh = DaoFactory.GetDao<ShippingHeader, long>();
        //    DateTime dtfromDate, dttoDate;
        //    DateTime.TryParse(fromDate, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dtfromDate);
        //    DateTime.TryParse(toDate, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dttoDate);

        //    //bool Con = false;
        //    if ((fromDate != null && fromDate != string.Empty) && (toDate != null && toDate != string.Empty))
        //    {
        //        //Con = true;
        //        daosh.SetCriteria(new ICriterion[] { Expression.Between("Createddate", dtfromDate.Date, dttoDate.Date.AddDays(1)), Expression.In("Areacode", AreaHelper.Area) });
        //    }
        //    else
        //    {
        //        if ((fromDate != null && fromDate != string.Empty))
        //        {
        //            //Con = true;
        //            daosh.SetCriteria(new ICriterion[] { Expression.Between("Createddate", dtfromDate.Date, DateTime.MaxValue), Expression.In("Areacode", AreaHelper.Area) });
        //        }
        //        if ((toDate != null && toDate != string.Empty))
        //        {
        //            //Con = true;
        //            daosh.SetCriteria(new ICriterion[] { Expression.Between("Createddate", DateTime.MinValue, dttoDate.Date.AddDays(1)), Expression.In("Areacode", AreaHelper.Area) });
        //        }
        //    }
        //    listsh = daosh.GetAll();
        //    return listsh;
        //}

        public void Delete(long Id)
        {
            IDao<Shippingdetail, long> dao = DaoFactory.GetDao<Shippingdetail, long>();
            Shippingdetail sd = dao.GetById(Id, false);
            if (!string.IsNullOrEmpty(sd.Vmepresponse))
            {
                sd.Vmepresponse = string.Empty;
                sd.Vmepresponsedate = DateTime.MinValue;
                dao.SaveOrUpdate(sd);
            }
        }

        public void Update(string Vmepresponse, long Id)
        {
            IDao<Shippingdetail, long> dao = DaoFactory.GetDao<Shippingdetail, long>();
            Shippingdetail sd = dao.GetById(Id, false);
            sd.Vmepresponse = Vmepresponse;
            sd.Vmepresponsedate = DateTime.Now;
            dao.SaveOrUpdate(sd);
        }
    }
}