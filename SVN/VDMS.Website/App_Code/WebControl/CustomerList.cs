using System;
using System.ComponentModel;
using System.Linq;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.Linq;
using Resources;
using VDMS.II.BasicData;

namespace VDMS.II.WebControls
{
    public partial class CustomerList : DropDownList
    {

        public string BindingSelectedValue { get; set; }
        public bool ShowSelectAllItem { get; set; }
        public bool ShowNullItemIfSelectFailed { get; set; }
        public string DealerCode { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.DataBind();
            }
        }

        public override void DataBind()
        {
            this.DataTextField = "Name";
            this.DataValueField = "CustomerId";

            PartDataContext db = DCFactory.GetDataContext<PartDataContext>();
            if (string.IsNullOrEmpty(this.DealerCode))
                this.DataSource = db.Customers.Where(c => c.DealerCode == UserHelper.DealerCode);
            else
                this.DataSource = db.Customers.Where(c => c.DealerCode == this.DealerCode);
            base.DataBind();
        }

        protected override void OnDataBound(EventArgs e)
        {
            if (this.ShowSelectAllItem) this.Items.Insert(0, new ListItem(Constants.All, ""));
            base.OnDataBound(e);
        }

        protected override void OnDataBinding(EventArgs e)
        {

            base.OnDataBinding(e);
            if (this.Items.FindByValue(this.BindingSelectedValue) == null)
            {
                if (this.ShowNullItemIfSelectFailed)
                {
                    this.Items.Insert(0, new ListItem("", ""));
                    this.SelectedIndex = 0;
                }
            }
            else base.SelectedValue = this.BindingSelectedValue;
        }
    }

}