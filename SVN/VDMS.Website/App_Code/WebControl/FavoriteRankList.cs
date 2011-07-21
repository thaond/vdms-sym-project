using System;
using System.Web.UI.WebControls;

namespace VDMS.II.WebControls
{
	public partial class FavoriteRankList : DropDownList
	{
		public string BindingSelectedValue { get; set; }
		public bool ShowEmptyItem { get; set; }

		protected override void OnLoad(EventArgs e)
		{
			DataTextField = "Name";
			DataValueField = "Code";
			if (this.Items.Count == 0)
			{
				for (int i = 1; i <= 10; i++)
				{
                    ListItem item = new ListItem(i.ToString(), i.ToString());
                    item.Selected = (item.Value == this.BindingSelectedValue);
                    this.Items.Add(item);
				}
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