using System;
using System.Collections;
using System.Web;
using NHibernate;
using NHibernate.Expression;
using VDMS.Core.Domain;
using VDMS.Core.Data;
using VDMS.Data.IDAL.Interface;
/// <summary>
/// Author: tntung
/// Date: 19/11/2007
/// Last Update: 19/11/2007
/// </summary>


namespace VDMS.I.Vehicle
{
    public enum CusType : int
    {
        Sale = 0,
        Service = 1,
        WarrantyInfo = 2,
    }
	public enum CusJobType : int
	{
		Student = 1,
		Bussiness = 2,
		Free = 3,
		Army = 4,
		Other = 5
	}
	public enum CusPaymentType : int
	{
		PayAll = 0,
		FixedHire_purchase = 1,
		UnFixedHire_purchase = 2
	}
	public enum CusPaymentStatus : int
	{
		NonPay = 0,
		PayCash = 1,
		PayTransfer = 2
	}
	public sealed class CustomerHelper
	{
		public static Customer LoadCustomerByIdentifyNumber(ref ISession sess, string IdentifyNumber)
		{
			IList lstCustomer = sess.CreateCriteria(typeof(Customer)).Add(Expression.Eq("Identifynumber", IdentifyNumber)).List();
			if (lstCustomer.Count > 0)
			{
				return (Customer)lstCustomer[0];
			}
			else return null;
		}
		public static Customer SaveCustomer(
			string idnum,
			string fullname,
			bool gender,
			DateTime birthdate,
			string address,
			string provinceid,
			string districtid,
			int jobtype,
			string email,
			string tel,
			string mobile,
			int priority,
			int customertype,
			string cusdesc,
			string precinct,
			string dealercode,
			bool forservices
			)
		{
            IDao<Customer, long> dao = DaoFactory.GetDao<Customer, long>();
			Customer cusIns = new Customer();
			cusIns.Identifynumber = idnum;
			cusIns.Fullname = fullname;
			cusIns.Gender = gender;
			if (birthdate != DateTime.MinValue)
			{
				cusIns.Birthdate = birthdate;
			}
			cusIns.Address = address;
			cusIns.Provinceid = provinceid; 
            cusIns.Districtid = districtid; 
            cusIns.Jobtypeid = jobtype; 
            cusIns.Email = email;
			cusIns.Tel = tel; 
            cusIns.Mobile = mobile; 
            cusIns.Priority = priority; 
            cusIns.Customertype = customertype;
			cusIns.Customerdescription = cusdesc; 
            cusIns.Precinct = precinct; 
            cusIns.Dealercode = dealercode;
			cusIns.Forservice = forservices;
            dao.SaveOrUpdate(cusIns);
			return cusIns;
		}
        public static long SaveCustomer(ref ISession sess,
            string idnum,
            string fullname,
            bool gender,
            DateTime birthdate,
            string address,
            string provinceid,
            string districtid,
            int jobtype,
            string email,
            string tel,
            string mobile,
            int priority,
            string customertype,
            string cusdesc,
            string precinct,
            string dealercode,
            bool forservices
            )
        {
            Customer cusIns = new Customer();
            cusIns.Identifynumber = idnum;
            cusIns.Fullname = fullname;
            cusIns.Gender = gender;
            if (birthdate != DateTime.MinValue)
            {
                cusIns.Birthdate = birthdate;
            }
            cusIns.Address = address;
            cusIns.Provinceid = provinceid;
            cusIns.Districtid = districtid;
            cusIns.Jobtypeid = jobtype;
            cusIns.Email = email;
            cusIns.Tel = tel;
            cusIns.Mobile = mobile;
            cusIns.Priority = priority;
            cusIns.Customertype = int.Parse(customertype);
            cusIns.Customerdescription = cusdesc;
            cusIns.Precinct = precinct;
            cusIns.Dealercode = dealercode;
            cusIns.Forservice = forservices;
            sess.SaveOrUpdate(cusIns);
            return cusIns.Id;
        }
		public static long UpdateCustomer(ref ISession sess,
            long id,
			string idnum,
			string fullname,
			bool gender,
			DateTime birthdate,
			string address,
			string provinceid,
			string districtid,
			int jobtype,
			string email,
			string tel,
			string mobile,
			int priority,
			string customertype,
			string cusdesc,
			string precinct,
			string dealercode,
			Boolean forservices
			)
		{
			Customer cusIns = sess.CreateCriteria(typeof(Customer))
				.Add(Expression.Eq("Dealercode", dealercode))
                //.Add(Expression.Eq("Identifynumber", idnum)).List()[0] as Customer;
				.Add(Expression.Eq("Id", id)).List()[0] as Customer;
			cusIns.Identifynumber = idnum;
			cusIns.Fullname = fullname;
			cusIns.Gender = gender;
			if (birthdate != DateTime.MinValue)
			{
				cusIns.Birthdate = birthdate;
			}
			cusIns.Address = address;
			cusIns.Provinceid = provinceid; cusIns.Districtid = districtid; cusIns.Jobtypeid = jobtype; cusIns.Email = email;
			cusIns.Tel = tel; cusIns.Mobile = mobile; cusIns.Priority = priority; cusIns.Customertype = int.Parse(customertype);
			cusIns.Customerdescription = cusdesc; cusIns.Precinct = precinct; cusIns.Dealercode = dealercode;
			cusIns.Forservice = forservices;
			sess.SaveOrUpdate(cusIns);
			return cusIns.Id;
		}
	}
}

//namespace VDMS.Data.DAL
//{
//    public sealed class NHibernateHelper
//    {
//        private const string CurrentSessionKey = "nhibernate.current_session";
//        private static readonly ISessionFactory sessionFactory;
//        static NHibernateHelper()
//        {
//            sessionFactory = new NHibernate.Cfg.Configuration().Configure().BuildSessionFactory();
//        }
//        public static ISession GetCurrentSession()
//        {
//            HttpContext context = HttpContext.Current;
//            ISession currentSession = context.Items[CurrentSessionKey] as ISession;
//            if (currentSession == null)
//            {
//                currentSession = sessionFactory.OpenSession();
//                context.Items[CurrentSessionKey] = currentSession;
//            }
//            return currentSession;
//        }
//        public static void CloseSession()
//        {
//            HttpContext context = HttpContext.Current;
//            ISession currentSession = context.Items[CurrentSessionKey] as ISession;
//            if (currentSession == null)
//            {
//                //Nocurrentsession
//                return;
//            }
//            currentSession.Close();
//            context.Items.Remove(CurrentSessionKey);
//        }
//        public static void CloseSessionFactory()
//        {
//            if (sessionFactory != null)
//            {
//                sessionFactory.Close();
//            }
//        }
//    }
//}
