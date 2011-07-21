using System;
using System.Web.UI.WebControls;
using VDMS.Bonus.Entity;

namespace VDMS.II.WebControls
{
    public partial class BonusStatusList : DropDownList
	{
		public string BindingSelectedValue { get; set; }
		public bool ShowEmptyItem { get; set; }

		protected override void OnLoad(EventArgs e)
		{
			DataTextField = "Name";
			DataValueField = "Code";
			if (this.Items.Count == 0)
			{
                ListItem item = new ListItem(BonusStatus.GetName(BonusStatus.Normal), BonusStatus.Normal);
                item.Selected = (item.Value == this.BindingSelectedValue);
                this.Items.Add(item);
                
                item = new ListItem(BonusStatus.GetName(BonusStatus.Locked),BonusStatus.Locked );
                item.Selected = (item.Value == this.BindingSelectedValue);
                this.Items.Add(item);

                if (this.ShowEmptyItem) this.Items.Insert(0, new ListItem("", ""));
			}
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