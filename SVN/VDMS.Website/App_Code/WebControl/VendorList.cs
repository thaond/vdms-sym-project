using System;
using System.Linq;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.II.Linq;

namespace VDMS.II.WebControls
{
	public class VendorList : DropDownList
	{
		public string DealerCode { get; set; }

		protected override void OnLoad(EventArgs e)
		{
			this.DataTextField = "Name";
			this.DataValueField = "VendorId";
			if (!Page.IsPostBack)
			{
				this.DataBind();
			}
		}

		public override void DataBind()
		{
			PartDataContext db = DCFactory.GetDataContext<PartDataContext>();
			if (string.IsNullOrEmpty(DealerCode)) DealerCode = UserHelper.DealerCode;
			this.DataSource = db.Vendors.Where(p => p.DealerCode == DealerCode).OrderBy(p => p.Code);
			base.DataBind();
		}
	}
}