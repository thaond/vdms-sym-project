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
    public partial class CategoryList : DropDownList
    {
        //[
        //Category("Misc"),
        //DefaultValue("P"),
        //Bindable(true),
        //DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        //]
        //public string CategoryType
        //{
        //    get { return this.ViewState["CategoryType"].ToString(); }
        //    set
        //    {
        //        if ((this.ViewState["CategoryType"] == null) || (!this.ViewState["CategoryType"].ToString().Equals(value, StringComparison.OrdinalIgnoreCase)))
        //        {
        //            this.ViewState["CategoryType"] = value.Trim().ToUpper();
        //            this.DataBind();
        //        }
        //    }
        //}

        public string BindingSelectedValue { get; set; }
        public bool ShowSelectAllItem { get; set; }
        public bool ShowNullItemIfSelectFailed { get; set; }
        public string CategoryType { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (string.IsNullOrEmpty(CategoryType)) throw new ArgumentNullException("CategoryType", "CategoryType cannot be null.");
                this.DataBind();
            }
        }

        public override void DataBind()
        {

            if (CategoryType == "A")  //Accessory
            {
                DataTextField = "Name";
                DataValueField = "CategoryId";
                PartDataContext db = DCFactory.GetDataContext<PartDataContext>();
                DataSource = db.Categories.Where(p => p.DealerCode == UserHelper.DealerCode).ToList();
            }
            else //Part
            {
                DataTextField = "Name";
                DataValueField = "Code";
                DataSource = TipTopCategoryDAO.GetAll();
            }

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