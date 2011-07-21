using System;
using System.Linq;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.Bonus.Linq;
using VDMS.II.Linq;

namespace VDMS.II.WebControls
{
    public class BonusSourceList : DropDownList
    {
        public bool ShowEmptyItem { get; set; }
        public long? BindingValue { get; set; }

        public BonusSourceList()
        {
            BindingValue = null;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.DataBind();
            }
        }
        public class SourceInfo
        {
            public string Name { get; set; }
            public long? Id { get; set; }
        }
        public override void DataBind()
        {
            this.DataTextField = "Name";
            this.DataValueField = "Id";
            var db = DCFactory.GetDataContext<BonusDataContext>();
            var d = db.BonusSources.OrderBy(p => p.BonusSourceName).ToList().Select(p => new SourceInfo() { Name = p.BonusSourceName, Id = p.BonusSourceId }).ToList();

            if (ShowEmptyItem) d.Insert(0, new SourceInfo() { Name = "", Id = null });

            this.DataSource = d;
            base.DataBind();

            //if (BindingValue == null) SelectedIndex = -1;
            //else SelectedValue = BindingValue.ToString();
        }
    }
}