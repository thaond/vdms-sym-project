using System;
using System.Linq;
using System.Web.UI.WebControls;
using Resources;
using VDMS.II.Linq;

namespace VDMS.II.WebControls
{
    public partial class AccessoryTypeList : DropDownList
    {
        public bool ShowSelectAllItem { get; set; }

        public string BindingSelectedValue { get; set; }

        public string DefaultValue { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            DataTextField = "AccessoryTypeName";
            DataValueField = "AccessoryTypeCode";
            if (!Page.IsPostBack)
            {
                this.DataBind();
            }
        }

        public override void DataBind()
        {
            PartDataContext db = DCFactory.GetDataContext<PartDataContext>();
            this.DataSource = db.AccessoryTypes.ToList();
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

            if (this.Items.FindByValue(this.BindingSelectedValue) != null)
            {
                base.SelectedValue = this.BindingSelectedValue;
            }
            else
            {
                if (this.Items.FindByValue(this.DefaultValue) != null)
                {
                    base.SelectedValue = this.DefaultValue;
                }
            }
        }
    }
}