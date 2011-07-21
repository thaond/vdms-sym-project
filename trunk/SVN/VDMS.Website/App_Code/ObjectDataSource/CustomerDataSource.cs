using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;

namespace VDMS.I.ObjectDataSource
{
    class CustomerComparer : IEqualityComparer<Customer>
    {
        public bool Equals(Customer c1, Customer c2)
        { 
            if (object.ReferenceEquals(c1, c2)) return true;
            if (object.ReferenceEquals(c1, null) || object.ReferenceEquals(c2, null)) return false;

            return c1.Fullname == c2.Fullname &&
                   c1.Identifynumber == c2.Identifynumber &&
                   c1.Address == c2.Address &&
                   c1.Birthdate == c2.Birthdate;
        }

        public int GetHashCode(Customer c)
        {
            if (object.ReferenceEquals(c, null)) return 0;
            return c.Fullname.GetHashCode() ^ (c.Identifynumber == null ? 0 : c.Identifynumber.GetHashCode());
        }
    }

    public class CustomerDataSource
    {
        private int count;
        public CustomerDataSource()
        {
        }
        public int SelectCount(int maximumRows, int startRowIndex, string engineNumber, string dCode, string cType, string id, string name, string address, string phone)
        {
            return count;
        }
        public object SelectCustomers(int maximumRows, int startRowIndex, string engineNumber, string dCode, string cType, string id, string name, string address, string phone)
        {
            var dc = VDMS.II.Linq.DCFactory.GetDataContext<VDMS.I.Linq.ServiceDataContext>();
            IQueryable<long> cList = null;

            if (cType == "SV")
            {
                var ser = dc.ServiceHeaders.Where(s => s.CustomerId != null);
                if (!string.IsNullOrEmpty(engineNumber)) ser = ser.Where(s => s.EngineNumber == engineNumber);
                if (!string.IsNullOrEmpty(dCode)) ser = ser.Where(s => s.DealerCode == dCode);
                cList = ser.GroupBy(s => s.CustomerId).Select(g => (long)g.Key);
            }
            else if(cType == "SL")
            {
                var inv = dc.Invoices.AsQueryable();
                if (!string.IsNullOrEmpty(engineNumber)) inv = inv.Where(s => s.EngineNumber == engineNumber);
                if (!string.IsNullOrEmpty(dCode)) inv = inv.Where(s => s.DealerCode == dCode);
                cList = inv.GroupBy(s => s.CustomerId).Select(g => g.Key);
            }

            var cus = dc.Customers.AsQueryable();
            if (!string.IsNullOrEmpty(dCode)) cus = cus.Where(s => s.DealerCode == dCode);
            if (!string.IsNullOrEmpty(id)) cus = cus.Where(c => c.IdentifyNumber == id);
            if (!string.IsNullOrEmpty(name)) cus = cus.Where(c => c.FullName.ToLower().Contains(name.ToLower()));
            if (!string.IsNullOrEmpty(address)) cus = cus.Where(c => c.Address.ToLower().Contains(address.ToLower()));
            if (!string.IsNullOrEmpty(phone)) cus = cus.Where(c => c.Mobile == phone || c.Tel == phone);
            var comparer = new CustomerComparer();
            var query = cus.Join(cList, c => c.CustomerId, s => s, (c, s) => c).ToList();
            var distinctCustomers = query.Select(c => new { c.BirthDate, c.FullName, c.IdentifyNumber }).Distinct();
            var rs = distinctCustomers.GroupJoin(query,
                                                 d => d,
                                                 c => new { c.BirthDate, c.FullName, c.IdentifyNumber },
                                                 (d, c) => c.First());
            count = rs.Count();

            if ((maximumRows > 0) && (startRowIndex >= 0)) rs = rs.AsEnumerable().Skip(startRowIndex).Take(maximumRows);

            return rs;
        }

        public ArrayList SelectCusts(string engineNumber)
        {

            IDao<Serviceheader, long> dao = DaoFactory.GetDao<Serviceheader, long>();
            ArrayList list = null;
            try
            {
                list = (ArrayList)dao.GetByQuery("select distinct ser.Customer.Id from VDMS.Core.Domain.Serviceheader ser where ser.Status >= 0 AND UPPER(ser.Enginenumber) = UPPER('" + engineNumber + "')", null);
                count = list.Count;
                HttpContext.Current.Items["listgvSelectCustomerRowCount"] = count;
            }
            catch { }
            if (list == null) list = new ArrayList();

            return list;
        }
        public List<Customer> Select_o(int maximumRows, int startRowIndex, string engineNumber, string dCode)
        {
            List<Customer> list = null;
            IDao<Customer, long> dao = DaoFactory.GetDao<Customer, long>();

            try
            {
                dao.SetCriteria(new ICriterion[] { Expression.In("Id", SelectCusts(engineNumber)) });
                dao.SetOrder(new Order[] { Order.Asc("Fullname") });

                list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
                count = dao.ItemCount;

                // if user change pageindex after change "select conditions" without press "check" button
                // so we select with pageindex=0
                if (startRowIndex >= count)
                {
                    list = dao.GetPaged(0, maximumRows);
                    count = dao.ItemCount;
                }
                HttpContext.Current.Items["listgvSelectCustRowCount"] = count;
            }
            catch { };
            return list;
        }


        public static Customer GetCustomer(string custId)
        {
            long id;
            long.TryParse(custId, out id);
            IDao<Customer, long> dao = DaoFactory.GetDao<Customer, long>();
            return dao.GetById(id, false); //true -> false
        }

    }
}