using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Data.TipTop;
using VDMS.II.Entity;
using VDMS.II.Linq;
using VDMS.Provider;

public partial class Admin_Controls_UserProfile : System.Web.UI.UserControl
{
    public string Username { get; set; }
    public string DealerCode { get; set; }

    protected void Page_Init(object sender, EventArgs e)
    {
        this.Page.RegisterRequiresControlState(this);
    }

    protected override void LoadControlState(object savedState)
    {
        object[] ctlState = (object[])savedState;
        base.LoadControlState(ctlState[0]);
        this.Username = (string)ctlState[1];
        this.DealerCode = (string)ctlState[2];
    }

    protected override object SaveControlState()
    {
        object[] ctlState = new object[3];
        ctlState[0] = base.SaveControlState();
        ctlState[1] = this.Username;
        ctlState[2] = this.DealerCode;
        return ctlState;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ddlArea.DataSource = Area.GetListArea(ddlDatabase.SelectedValue);
            ddlArea.DataBind();
            ddlWH.DealerCode = ddlVWh.DealerCode = DealerCode;

            if (DealerCode != "/") mvP.ActiveViewIndex = 1;
        }
    }
    public void LoadProfile()
    {
        if (Username != string.Empty)
        {
            ddlArea.DataSource = Area.GetListArea(ddlDatabase.SelectedValue);
            ddlArea.DataBind();
            var db = DCFactory.GetDataContext<SecurityDataContext>();
            var obj = db.UserProfiles.FirstOrDefault(p => p.UserName.ToUpper() == Username.ToUpper() && p.DealerCode.ToUpper() == DealerCode.ToUpper());
            if (obj == null) return;

            txtFullname.Text = obj.FullName;
            if (DealerCode == "/")
            {
                ddlDatabase.SelectedValue = obj.DatabaseCode;
                ddlDatabase_SelectedIndexChanged(null, null);
                ddlArea.SelectedValue = obj.AreaCode;
                ddlDept.SelectedValue = obj.Dept;
                ddlPosition.SelectedValue = obj.Position;
                ddlNGAL.SelectedValue = obj.NGLevel.ToString();
            }
            else
            {
                if (obj.VWarehouseId.HasValue) ddlVWh.SelectedValue = obj.VWarehouseId.ToString();
                if (obj.WarehouseId.HasValue) ddlWH.SelectedValue = obj.WarehouseId.ToString();
            }
        }
    }

    public void SaveProfile(string Username)
    {
        var db = DCFactory.GetDataContext<SecurityDataContext>();
        var obj = db.UserProfiles.FirstOrDefault(p => p.UserName.ToUpper() == Username.ToUpper() && p.DealerCode.ToUpper() == DealerCode.ToUpper());
        if (obj == null)
        {
            obj = new UserProfile
            {
                UserName = Username.ToUpper(),
                DealerCode = DealerCode.ToUpper()
            };
            db.UserProfiles.InsertOnSubmit(obj);
        }
        obj.FullName = txtFullname.Text.Trim();
        if (DealerCode == "/")
        {
            obj.DatabaseCode = ddlDatabase.SelectedValue;
            obj.AreaCode = ddlArea.SelectedValue;
            obj.Dept = ddlDept.SelectedValue;
            obj.Position = ddlPosition.SelectedValue;
            obj.NGLevel = int.Parse(ddlNGAL.SelectedValue);
        }
        else
        {
            obj.WarehouseId = (!string.IsNullOrEmpty(ddlWH.SelectedValue)) ? (long?)long.Parse(ddlWH.SelectedValue) : null;
            obj.VWarehouseId = (!string.IsNullOrEmpty(ddlVWh.SelectedValue)) ? (long?)long.Parse(ddlVWh.SelectedValue) : null;
        }
        db.SubmitChanges();
        HttpContext.Current.Session["CurrentUser.Profile"] = null;
        HttpContext.Current.Session["CurrentUser.DatabaseCode"] = obj.DatabaseCode;
    }

    protected void ddlDatabase_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlArea.DataSource = Area.GetListArea(ddlDatabase.SelectedValue);
        ddlArea.DataBind();
    }
}
