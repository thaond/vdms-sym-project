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
    public partial class DatabaseList : DropDownList
    {
        object list = new object[]
        {
            new  { Name = Constants.North, Code = "HTF" },
            new  { Name = Constants.South, Code = "DNF" }
        };

        public bool AllowDealerSelect { get; set; }
        public string BindingSelectedValue { get; set; }
        public bool ShowSelectAllItem { get; set; }
        public string CategoryType { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            DataTextField = "Name";
            DataValueField = "Code";
            if (!Page.IsPostBack)
            {
                this.DataBind();
            }
        }

        public override void DataBind()
        {
            this.DataSource = list;
            base.DataBind();
        }

        protected override void OnDataBound(EventArgs e)
        {
            if (this.ShowSelectAllItem) this.Items.Insert(0, new ListItem(Constants.All, ""));
            base.OnDataBound(e);
            
            if (UserHelper.IsDealer)
            {
                this.SelectedValue = UserHelper.DatabaseCode;
                this.Enabled = this.AllowDealerSelect;
            }
        }

        protected override void OnDataBinding(EventArgs e)
        {

            base.OnDataBinding(e);
            if (this.Items.FindByValue(this.BindingSelectedValue) == null)
            {
                this.SelectedIndex = -1;
            }
            else base.SelectedValue = this.BindingSelectedValue;
        }
    }

}