using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.IDAL.Interface;

namespace VDMS.I.ObjectDataSource
{
    public class ShippingDetailDataSource
    {
        private int count = 0;
        private IDao<Shippingdetail, Int64> dao;

        // return maximum record
        public int SelectCount(int maximumRows, int startRowIndex)
        {
            return count;
        }

        public IList<Shippingdetail> Select(int maximumRows, int startRowIndex, Int64 shipID)
        {
            List<Shippingdetail> list = null;

            try
            {
                int page = (maximumRows == 0) ? 0 : startRowIndex / maximumRows;
                dao.SetCriteria(new ICriterion[] { Expression.Eq("Shippingheader.Id", shipID) });
                dao.SetOrder(new Order[] { Order.Asc("Id") });
                list = dao.GetAll();
                count = dao.ItemCount;
            }
            catch { };
            return list;
        }

        public IList<Shippingdetail> SelectNotOder(int maximumRows, int startRowIndex, Int64 shipID)
        {
            List<Shippingdetail> list = null;

            try
            {
                int page = (maximumRows == 0) ? 0 : startRowIndex / maximumRows;
                dao.SetCriteria(new ICriterion[] { Expression.Eq("Shippingheader.Id", shipID) });
                dao.SetOrder(new Order[] { Order.Asc("Id") });
                list = dao.GetAll();
                count = dao.ItemCount;
            }
            catch { };
            return list;
        }
        public void Delete(Int64 Id)
        {
            using (TransactionBlock tran = new TransactionBlock())
            {
                Shippingdetail item = new Shippingdetail();
                item.Id = Id;

                dao.Delete(item);
                tran.IsValid = true;
            }
        }

        public bool Update(Int64 Id, string Itemtype, string Color, string Enginenumber, string Ordernumber, Int32 Status, string Exception)
        {
            Shippingdetail item = dao.GetById(Id, false); //true -> false
            if (item == null) return false;
            item.Itemtype = Itemtype;
            item.Color = Color;
            item.Enginenumber = Enginenumber;
            item.Ordernumber = Ordernumber;
            item.Status = Status;
            item.Exception = Exception;

            try
            {
                return (dao.SaveOrUpdate(item) != null);
            }
            catch { return false; }
        }

        public bool Update(IOrderedDictionary keys, IOrderedDictionary oldValues, IOrderedDictionary newValues)
        {
            Shippingdetail item = dao.GetById(Convert.ToInt64(keys[0].ToString()), false); //true -> false
            try
            {
                return (dao.SaveOrUpdate(item) != null);
            }
            catch { return false; }
        }

        public ShippingDetailDataSource()
        {
            dao = DaoFactory.GetDao<Shippingdetail, Int64>();
        }

        #region linQ

        public static List<VDMS.I.Entity.IShippingDetail> GetShippingDetail(string shipNo, string orderNo)
        {
            var dc = VDMS.II.Linq.DCFactory.GetDataContext<VDMS.I.Linq.VehicleDataContext>();
            return dc.IShippingDetails.Where(d => d.TipTopOrderNumber == orderNo && d.IssueNumber == shipNo).ToList();
        }

        #endregion
    }
}