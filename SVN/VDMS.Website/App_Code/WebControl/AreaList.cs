using System;
using System.Web.UI.WebControls;
using VDMS.Data.TipTop;
using Resources;
using VDMS.Helper;

namespace VDMS.II.WebControls
{
    public class AreaList : DropDownList
    {
        public bool ShowSelectAllItem { get; set; }
        public bool SeparateByDB { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            this.DataTextField = "AreaName";
            this.DataValueField = "AreaCode";
            if (!Page.IsPostBack)
            {
                this.DataBind();
            }
        }

        protected override void OnDataBound(EventArgs e)
        {
            if (this.ShowSelectAllItem) this.Items.Insert(0, new ListItem(Constants.All, ""));
            base.OnDataBound(e);
        }

        public override void DataBind()
        {
            if (SeparateByDB)
                this.DataSource = Area.GetListArea(UserHelper.DatabaseCode);
            else
                this.DataSource = Area.GetListArea();
            base.DataBind();
        }
    }
}