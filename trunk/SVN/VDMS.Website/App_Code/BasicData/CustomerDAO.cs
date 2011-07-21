using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Resources;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.Linq;

namespace VDMS.II.BasicData
{
	public class CustomerDAO
	{
		int _CusCount;
		public int GetCount()
		{
			return _CusCount;
		}
		public int GetCount(long cusId)
		{ return _CusCount; }

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IQueryable FindAll(int maximumRows, int startRowIndex)
		{
			return FindAll(0, maximumRows, startRowIndex);
		}
		[DataObjectMethod(DataObjectMethodType.Select)]
		public IQueryable FindAll(long cusId, int maximumRows, int startRowIndex)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			var query = db.Customers.Where(p => p.DealerCode == UserHelper.DealerCode);
			if (cusId > 0) query = query.Where(c => c.CustomerId == cusId);
			_CusCount = query.Count();
			return query.OrderBy(p => p.Code).Skip(startRowIndex).Take(maximumRows);
		}
		[DataObjectMethod(DataObjectMethodType.Select)]
		public List<Customer> FindAllWithRetail(long cusId, int maximumRows, int startRowIndex)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			List<Customer> res;


			var query = db.Customers.Where(p => p.DealerCode == UserHelper.DealerCode);
			if (cusId != 0) query = query.Where(c => c.CustomerId == cusId);
			res = query.OrderBy(p => p.Code).ToList();

			if (cusId <= 0)
			{
				Customer retail = new Customer() { CustomerId = -1, Code = Constants.RetailCustomer };
				res.Insert(0, retail);
			}

			_CusCount = res.Count();
			return res.Skip(startRowIndex).Take(maximumRows).ToList();
		}

		[DataObjectMethod(DataObjectMethodType.Delete)]
		public void Delete(long CustomerId)
		{
			Customer cus = CustomerDAO.GetCustomer(CustomerId);
			if (cus != null)
			{
				DC.Customers.DeleteOnSubmit(cus);
				if (cus.Contact != null) DC.Contacts.DeleteOnSubmit(cus.Contact);
				DC.SubmitChanges();
			}
		}

		#region Static

		public static PartDataContext DC
		{
			get
			{
				return DCFactory.GetDataContext<PartDataContext>();
			}
		}
		public static bool IsCustomerExist(string code, string dealerCode)
		{
			return CustomerDAO.GetCustomer(code, dealerCode) != null;
		}
		public static Customer GetCustomer(long id)
		{
			return DC.Customers.SingleOrDefault(c => c.CustomerId == id);
		}

		public static Customer GetCustomer(string code, string dealerCode)
		{
			return DC.Customers.SingleOrDefault(c => c.Code == code && c.DealerCode == dealerCode);
		}

		public static Customer CreateOrUpdate(long cusId, string code, string name, string vat, string dealerCode, Contact contact)
		{
			Customer cus = CustomerDAO.GetCustomer(cusId);
			if (cus == null) cus = CustomerDAO.GetCustomer(code, dealerCode);
			if (contact == null) contact = new Contact();

			if (cus == null)
			{
				cus = new Customer();
				cus.Contact = new Contact();
				cus.DealerCode = dealerCode;
				DC.Customers.InsertOnSubmit(cus);
			}

			cus.Code = code.Trim().ToUpper();
			cus.Name = name.Trim();
			cus.VATCode = vat.Trim();

			cus.Contact.AdditionalInfo = contact.AdditionalInfo;
			cus.Contact.Address = contact.Address;
			cus.Contact.Email = contact.Email;
			cus.Contact.FullName = contact.FullName;
			cus.Contact.Phone = contact.Phone;
			cus.Contact.Fax = contact.Fax;
			cus.Contact.Mobile = contact.Mobile;

			DC.SubmitChanges();

			return cus;
		}

		#endregion
	}
}