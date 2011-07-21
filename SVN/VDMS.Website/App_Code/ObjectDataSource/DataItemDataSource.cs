using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.IDAL.Interface;
using VDMS.Helper;
using VDMS.I.Vehicle;

namespace VDMS.I.ObjectDataSource
{
    public class DataItemDataSource
    {
        private int count = 0;

        public int SelectCount(int maximumRows, int startRowIndex, string itemCodeLike, string colorCodeLike, int status)
        {
            return count;
        }

        public int SelectCount(int maximumRows, int startRowIndex, string itemTypeLike)
        {
            return count;
        }

        public IList<Item> Select(int maximumRows, int startRowIndex, string itemCodeLike, string colorCodeLike, int status)
        {
            IDao<Item, string> dao = DaoFactory.GetDao<Item, string>();
            List<Item> list = null;
            string ccode = "", icode = "";
            if (colorCodeLike != null) ccode = colorCodeLike.Trim() + "%";
            if (itemCodeLike != null) icode = itemCodeLike.Trim() + "%";
            if (icode == "") icode = "%";
            if (ccode == "") ccode = "%";

            ArrayList crit = new ArrayList();
            try
            {
                //dao.SetCriteria(new ICriterion[] { Expression.And(Expression.And(Expression.InsensitiveLike("Itemtype", icode), Expression.InsensitiveLike("Colorcode", ccode)) 
                crit.Add(Expression.InsensitiveLike("Id", icode));
                crit.Add(Expression.InsensitiveLike("Colorcode", ccode));
                crit.Add(Expression.Like("DatabaseCode", UserHelper.DatabaseCode, MatchMode.Anywhere));
                if (status != -1)
                {
                    crit.Add(ItemHepler.GetAvailableItemExpression(Convert.ToBoolean(status)));
                }

                dao.SetCriteria((ICriterion[])crit.ToArray(typeof(ICriterion)));

				dao.SetOrder(new NHibernate.Expression.Order[] { NHibernate.Expression.Order.Asc("Id") });
                list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
                count = dao.ItemCount;

                // if user change pageindex after change "select conditions" without press "check" button
                // so we select with pageindex=0
                if (startRowIndex >= count)
                {
                    list = dao.GetPaged(0, maximumRows);
                    count = dao.ItemCount;
                }
                HttpContext.Current.Items["listRowCount"] = count;
            }
            catch { };
            return list;
        }

        public ArrayList SelectTypes(string itemTypeLike)
        {
            string itemType = (string.IsNullOrEmpty(itemTypeLike)) ? "%" : itemTypeLike.Trim().ToUpper() + "%";
            IDao<Item, string> dao = DaoFactory.GetDao<Item, string>();
            ArrayList list;
            try
            {
				dao.SetOrder(new NHibernate.Expression.Order[] { NHibernate.Expression.Order.Asc("Id") });
                //dao.SetCriteria(new ICriterion[] { Expression.Like("Id", itemType) });
                //list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
                list = (ArrayList)dao.GetByQuery("select distinct item.Id from VDMS.Core.Domain.Item item where UPPER(item.Id) like '" + itemType + "'", null);
                count = list.Count;
                HttpContext.Current.Items["listgvSelectModelRowCount"] = count;
            }
            catch { return null; }
            if (list.Count <= 0) return null;
            return list;
        }

        public List<Item> Select(int maximumRows, int startRowIndex, string itemTypeLike)
        {
            string itemType = (string.IsNullOrEmpty(itemTypeLike)) ? "%" : "%" + itemTypeLike.Trim().ToUpper() + "%";
            List<Item> list = new List<Item>();

            NHibernate.ISession session = NHibernateSessionManager.Instance.GetSession();

            // searching conditions
            ArrayList listType = SelectTypes(itemTypeLike); if (listType == null) return null;
            listType.Sort();

            startRowIndex = (listType.Count <= startRowIndex) ? 0 : startRowIndex;
            int last = (maximumRows + startRowIndex);
            last = (last > listType.Count) ? listType.Count : last;

            for (int i = startRowIndex; i < last; i++)
            {
                Item item = new Item();
                item.Itemtype = listType[i].ToString();
                item.Id = item.Itemtype + i.ToString();
                list.Add(item);
            }

            if ((list.Count <= 0)) return null;
            return list;
        }

        public static bool HasChildRow(string Id)
        {
            if (Id.Trim() == "") return false;
            IDao<Iteminstance, long> daoIIS = DaoFactory.GetDao<Iteminstance, long>();
            IDao<Itemfavorite, long> daoIF = DaoFactory.GetDao<Itemfavorite, long>();
            IDao<Orderdetail, long> daoOD = DaoFactory.GetDao<Orderdetail, long>();
            IDao<Serviceheader, long> daoSH = DaoFactory.GetDao<Serviceheader, long>();
            IDao<Warrantyinfo, string> daoWI = DaoFactory.GetDao<Warrantyinfo, string>();

            daoIIS.SetCriteria(new ICriterion[] { Expression.Eq("Itemcode", Id) });
            daoIF.SetCriteria(new ICriterion[] { Expression.Eq("Item.Id", Id) });
            daoOD.SetCriteria(new ICriterion[] { Expression.Eq("Item.Id", Id) });
            daoSH.SetCriteria(new ICriterion[] { Expression.Eq("Itemtype", Id) });
            daoWI.SetCriteria(new ICriterion[] { Expression.Eq("Itemcode", Id) });

            return (daoIIS.GetAll().Count > 0)
                || (daoIF.GetAll().Count > 0)
                || (daoSH.GetAll().Count > 0)
                || (daoWI.GetAll().Count > 0)
                || (daoOD.GetAll().Count > 0);
        }

        public void Delete(string Id)
        {
            if (HasChildRow(Id)) return;
            IDao<Item, string> dao = DaoFactory.GetDao<Item, string>();
            Item item;
            try
            {
                item = dao.GetById(Id, true);
                dao.Delete(item);
            }
            catch { }
        }

        public bool Update(string Id, object Forhtf, object Fordnf, string Colorcode, string Colorname, string Itemname, string DatabaseCode, string price)
        {
            IDao<Item, string> dao = DaoFactory.GetDao<Item, string>();
            Item item = dao.GetById(Id, true);
            long Price;
            if (item == null) return false;
            long.TryParse(price, out Price);

            //item.Price = Price;
			if (UserHelper.DatabaseCode == "DNF") item.Dnfprice = Price;
			else item.Htfprice = Price;

            //ItemHepler.MakeItemAavailable(item, Available);
            if (Forhtf != null) item.Forhtf = Convert.ToBoolean(Forhtf);
            if (Fordnf != null) item.Fordnf = Convert.ToBoolean(Fordnf);

            if (Colorcode != null) item.Colorcode = Colorcode.Trim();
            if (Colorname != null) item.Colorname = Colorname.Trim();
            if (Itemname != null) item.Itemname = Itemname.Trim();
            if (DatabaseCode != null) item.DatabaseCode = DatabaseCode.Trim();
            try
            {
                return (dao.SaveOrUpdate(item) != null);
            }
            catch { return false; }
        }

        public bool Update(string Id, bool Forhtf, bool Fordnf, string Itemname, string Price)
        {
            return Update(Id, Forhtf, Fordnf, null, null, Itemname, null, Price);
        }

        // use different type for set available (two method has diff param name only)
        public bool Update(string Id, object Forhtf, string Itemname, string Price)
        {
            return Update(Id, Forhtf, null, null, null, Itemname, null, Price);
        }
        public bool Update(string Id, bool Fordnf, string Itemname, string Price)
        {
            return Update(Id, null, Fordnf, null, null, Itemname, null, Price);
        }

        public bool Update(IOrderedDictionary keys, IOrderedDictionary oldValues, IOrderedDictionary newValues)
        {
            IDao<Item, string> dao = DaoFactory.GetDao<Item, string>();
            Item item = dao.GetById(keys[0].ToString(), true);
            if (item == null) return false;
            if (newValues.Contains("Forhtf")) item.Forhtf = Convert.ToBoolean(newValues["Forhtf"].ToString());
            if (newValues.Contains("Fordnf")) item.Fordnf = Convert.ToBoolean(newValues["Fordnf"].ToString());
            if (newValues.Contains("Colorcode")) item.Colorcode = (newValues["Colorcode"].ToString());
            if (newValues.Contains("Colorname")) item.Colorcode = (newValues["Colorname"].ToString());
            if (newValues.Contains("Id")) item.Colorcode = (newValues["Id"].ToString());
            if (newValues.Contains("Itemname")) item.Itemname = (newValues["Itemname"].ToString());
            //if (newValues.Contains("Areacode")) item.Areacode = (newValues["Areacode"].ToString());
            try
            {
                return (dao.SaveOrUpdate(item) != null);
            }
            catch { return false; }
        }
    }
}
