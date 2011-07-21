using System;
using System.Web.UI.WebControls;
using VDMS.Bonus.Entity;
using VDMS.Helper;

namespace VDMS.II.WebControls
{
    public partial class BankList : DropDownList
    {
        public string BindingSelectedValue { get; set; }
        public string DealerCode { get; set; }
        public bool ByCurrentDealer { get; set; }
        public bool ShowEmptyItem { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!this.Page.IsPostBack)
            {
                this.DataBind();
            }
        }
        public override void DataBind()
        {
            DataTextField = "FULL_BANK_NAME";
            DataValueField = "BANK_CODE";
            this.DataSource = VDMS.Data.TipTop.Bank.GetBankList(ByCurrentDealer && UserHelper.IsDealer ? UserHelper.DealerCode : this.DealerCode);
            base.DataBind();
            if (this.ShowEmptyItem) this.Items.Insert(0, new ListItem("", ""));
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            if (this.Items.FindByValue(this.BindingSelectedValue) != null)
            {
                base.SelectedValue = this.BindingSelectedValue;
            }
        }
    }

}