using System.Collections.Generic;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;

/// <summary>
/// Summary description for ItemInstanceHelper
/// </summary>
namespace VDMS.I.Vehicle
{
    public class ItemInstanceHelper
    {
        public ItemInstanceHelper()
        {
        }
        // BO: ItemIstance
        public static List<Iteminstance> GetIteminstanceByEngNo(string strEngNo)
        {
            List<Iteminstance> res;
            IDao<Iteminstance, long> dao = DaoFactory.GetDao<Iteminstance, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Enginenumber", strEngNo) });
            res = dao.GetAll();
            return res;
        }
        public static List<Iteminstance> GetIteminstanceByEngNo(string strEngNo, string DealerCode)
        {
            List<Iteminstance> res;
            IDao<Iteminstance, long> dao = DaoFactory.GetDao<Iteminstance, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Enginenumber", strEngNo), Expression.Eq("Dealercode", DealerCode) });
            res = dao.GetAll();
            return res;
        }

        public static Invoice GetInvoiceByItemInstance(Iteminstance ii)
        {
            Invoice res;
            IDao<Invoice, long> dao = DaoFactory.GetDao<Invoice, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Iteminstance", ii) });
            List<Invoice> listInvoice = dao.GetAll();
            if (listInvoice.Count > 0)
            {
                res = listInvoice[0];
                return res;
            }
            return null;
        }
        public static Shippingdetail GetShippingdetailByItemInstance(Iteminstance ii)
        {
            Shippingdetail res;
            Shippingdetail sh = new Shippingdetail();
            IDao<Shippingdetail, long> dao = DaoFactory.GetDao<Shippingdetail, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("PRODUCTINSTANCE", ii) });
            List<Shippingdetail> listShippingdetail = dao.GetAll();
            if (listShippingdetail.Count > 0)
            {
                res = new Shippingdetail();
                res = listShippingdetail[0];
                return res;
            }
            return null;
        }
        public static Shippingdetail GetShippingdetailByItemInstance(long Id)
        {
            Shippingdetail res;
            Shippingdetail sh = new Shippingdetail();
            IDao<Shippingdetail, long> dao = DaoFactory.GetDao<Shippingdetail, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("PRODUCTINSTANCE.Id", Id) });
            List<Shippingdetail> listShippingdetail = dao.GetAll();
            if (listShippingdetail.Count > 0)
            {
                res = new Shippingdetail();
                res = listShippingdetail[0];
                return res;
            }
            return null;
        }

        public static bool EngineNumberExist(string enginenumber)
        {
            List<Iteminstance> res;
            IDao<Iteminstance, long> dao = DaoFactory.GetDao<Iteminstance, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Enginenumber", enginenumber) });
            res = dao.GetAll();
            if (res.Count > 0)
                return true;
            else
                return false;
        }

        public static TransHis GetTransHisByTransactiontypeAndItemInstance(long IdItemIns, int transactionType)
        {
            TransHis res;
            IDao<TransHis, long> dao = DaoFactory.GetDao<TransHis, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Iteminstance.Id", IdItemIns), Expression.Eq("Transactiontype", transactionType) });
            List<TransHis> listTransHis = dao.GetAll();
            if (listTransHis.Count > 0)
            {
                res = listTransHis[0];
                return res;
            }
            return null;
        }
    }
}