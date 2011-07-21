using System.ComponentModel;
using System.Linq;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.Linq;

namespace VDMS.II.BasicData
{
	public class VendorDAO
	{
		public int GetCount()
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			return db.Vendors.Where(p => p.DealerCode == UserHelper.DealerCode).Count();
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IQueryable FindAll(int maximumRows, int startRowIndex)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			return from v in db.Vendors
				   join c in db.Contacts on v.ContactId equals c.ContactId
				   where v.DealerCode == UserHelper.DealerCode
				   orderby v.Code
				   select new
				   {
					   v.VendorId,
					   v.Code,
					   v.Name,
					   c.FullName,
					   c.Phone,
					   c.Mobile,
					   c.Fax,
					   c.Email,
					   c.Address,
					   c.AdditionalInfo,
					   CanDelete = db.TransactionHistories.Where(p => p.VendorId == v.VendorId).Count() == 0
				   };
		}

		[DataObjectMethod(DataObjectMethodType.Delete)]
		public void Delete(long VendorId)
		{
			PartDataContext db = DCFactory.GetDataContext<PartDataContext>();
			Vendor d = db.Vendors.SingleOrDefault(p => p.VendorId == VendorId);
			if (d != null)
			{
				try
				{
					db.Vendors.DeleteOnSubmit(d);
					if (d.Contact != null) DC.Contacts.DeleteOnSubmit(d.Contact);
					db.SubmitChanges();
				}
				catch { }
			}
		}

		#region statics

		public static bool IsVendorExist(string code, string dealerCode)
		{
			return VendorDAO.GetVendor(code, dealerCode) != null;
		}

		public static PartDataContext DC
		{
			get
			{
				return DCFactory.GetDataContext<PartDataContext>();
			}
		}

		public static Vendor GetVendor(long id)
		{
			return DC.Vendors.SingleOrDefault(v => v.VendorId == id);
		}

		public static Vendor GetVendor(string code, string dealerCode)
		{
			return DC.Vendors.SingleOrDefault(c => c.Code == code && c.DealerCode == dealerCode);
		}

		public static Vendor CreateOrUpdate(long vdId, string code, string name, string dealerCode, Contact contact)
		{
			Vendor vd = VendorDAO.GetVendor(vdId);
			if (vd == null) vd = VendorDAO.GetVendor(code, dealerCode);
			if (contact == null) contact = new Contact();

			if (vd == null)
			{
				vd = new Vendor();
				vd.Contact = new Contact();
				vd.DealerCode = dealerCode;
				DC.Vendors.InsertOnSubmit(vd);
			}

			vd.Code = code;
			vd.Name = name;

			vd.Contact.AdditionalInfo = contact.AdditionalInfo;
			vd.Contact.Address = contact.Address;
			vd.Contact.Email = contact.Email;
			vd.Contact.FullName = contact.FullName;
			vd.Contact.Phone = contact.Phone;
			vd.Contact.Mobile = contact.Mobile;
			vd.Contact.Fax = contact.Fax;

			DC.SubmitChanges();

			return vd;
		}

		#endregion
	}
}