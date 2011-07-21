using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;
using VDMS.I.Linq;
using VDMS.II.Linq;

namespace VDMS.I.ObjectDataSource
{
    public class SparesDataSource
    {

        private int count = 0;

        public int SelectCount(int maximumRows, int startRowIndex, string spareNumberLike)
        {
            return count;
        }

        public IList<Warrantycondition> Select(int maximumRows, int startRowIndex, string spareNumberLike)
        {
            //IDao<Warrantycondition, Decimal> dao = DaoFactory.GetDao<Warrantycondition, Decimal>();
            //List<Warrantycondition> list = null;
            //string enNum = (string.IsNullOrEmpty(spareNumberLike)) ? "" : spareNumberLike.Trim().ToUpper();

            //try
            //{
            //    //dao.SetCriteria(new ICriterion[] { Expression.Like("Partcode", enNum) });
            //    dao.SetCriteria(new ICriterion[] { Expression.InsensitiveLike("Partcode", enNum, MatchMode.Start) });
            //    dao.SetOrder(new Order[] { Order.Asc("Partcode") });
            //    list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
            //    count = dao.ItemCount;

            //    // if user change pageindex after change "select conditions" without press "check" button
            //    // so we select with pageindex=0
            //    if (startRowIndex >= count)
            //    {
            //        list = dao.GetPaged(0, maximumRows);
            //        count = dao.ItemCount;
            //    }
            //    HttpContext.Current.Items["listgvSelectSpareRowCount"] = count;
            //}
            //catch { };
            //return list;
            string enNum = (string.IsNullOrEmpty(spareNumberLike)) ? "" : spareNumberLike.Trim().ToUpper();
            var db = DCFactory.GetDataContext<ServiceDataContext>();
            var query = (from w in db.WarrantyConditions
                         where w.PartCode.Contains(enNum)
                         select new Warrantycondition
                                    {
                                        Id = w.WarrantyConditionId,
                                        Labour = (decimal) w.Labour,
                                        Manpower = w.ManPower,
                                        Motorcode = w.MotorCode,
                                        Partcode = w.PartCode,
                                        Partnameen = w.PartNameEN,
                                        Partnamevn = w.PartNameVN,
                                        Unitprice = (decimal) w.UnitPrice,
                                        Warrantylength = w.WarrantyLength,
                                        Warrantytime = w.WarrantyTime
                                    }).OrderBy(tttt => tttt.Partcode);
            count = query.Count();
            var query2 = query.Skip(startRowIndex).Take(maximumRows).ToList();
            return query2;


        }

        public SparesDataSource()
        {
        }
    }
}