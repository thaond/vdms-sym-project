using System;
using System.Linq;
using System.Web.UI.WebControls;
using Resources;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.Linq;

namespace VDMS.II.WebControls
{
    public class WarehouseList : DropDownList
    {
        public string DealerCode { get; set; }
        public bool ShowSelectAllItem { get; set; }
        public bool ShowEmptyItem { get; set; }
        public bool UseVIdAsValue { get; set; }
        public string Type { get; set; }
        public bool MergeCode { get; set; }

        public WarehouseList()
        {
            MergeCode = true;
            this.EnableViewState = true;
        }

        public bool DontAutoUseCurrentSealer { get; set; }

        public long SelectedWarehouseId
        {
            get
            {
                if (SelectedIndex < 0 || !Visible) return UserHelper.WarehouseId;
                return long.Parse(SelectedValue);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.DataBind();
                this.SelectedValue = UserHelper.WarehouseId.ToString();
            }
        }

        public string GetCode(string val, IQueryable<Warehouse> list)
        {
            if (UseVIdAsValue || (this.Type == WarehouseType.Part))
            {
                long id; long.TryParse(val, out id);
                var wh = list.SingleOrDefault(w => w.WarehouseId == id);
                return (wh == null) ? "" : wh.Code;
            }
            else
            {
                return val;
            }
        }

        public override void DataBind()
        {
            this.DataTextField = "Address";
            this.DataValueField = ((this.Type == WarehouseType.Vehicle) && !UseVIdAsValue) ? "Code" : "WarehouseId";

            var db = DCFactory.GetDataContext<PartDataContext>();
            if (string.IsNullOrEmpty(DealerCode) && !DontAutoUseCurrentSealer) DealerCode = UserHelper.DealerCode;
            if (string.IsNullOrEmpty(Type)) Type = "P";
            var query = db.ActiveWarehouses.Where(p => p.DealerCode == DealerCode && p.Type == Type).OrderBy(p => p.Code);
            try
            {
                this.DataSource = query;
                base.DataBind();
                if (MergeCode)
                {
                    foreach (ListItem item in this.Items)
                    {
                        if (!string.IsNullOrEmpty(item.Value)) item.Text = string.Format("{0}\t\t- {1}", GetCode(item.Value, query), item.Text);
                    }
                }
            }
            catch { }
        }

        protected override void OnDataBound(EventArgs e)
        {
            if (this.ShowSelectAllItem) this.Items.Insert(0, new ListItem(Constants.All, ""));
            if (this.ShowEmptyItem) this.Items.Insert(0, new ListItem("", ""));

            base.OnDataBound(e);
        }
    }
}