using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_Services_SelectSpare : BaseUserControl
{
    public string SelectedCode { get; set; }
    public object OnClientSelected { get { return ViewState["OnClientSelected"].ToString(); } set { ViewState["OnClientSelected"] = value; } }
    public string OnCancelSelect { get { return ViewState["OnCancelSelect"].ToString(); } set { ViewState["OnCancelSelect"] = value; } }

    public string txtPCID
    {
        get { return txtPC.ClientID;  }
    }
    public object EvalTip(object o1, object o2, object o3)
    {
        return string.Format("{0}@{1}@{2}", o1, o2, o3);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lnkCancel.Visible = !string.IsNullOrEmpty(OnCancelSelect);
        lnkCancel.OnClientClick = string.Format("{0}(); return false;", OnCancelSelect);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(GridView1.DataSourceID)) GridView1.DataSourceID = odsP.ID;
        //else
            GridView1.DataBind();
    }
    protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //SelectedCode = GridView1.DataKeys[e.NewSelectedIndex].Value.ToString();
    }

    protected void lnkSelect_DataBinding(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        string[] arg = lnk.ToolTip.Split('@');
        lnk.OnClientClick = string.Format("{0}('{1}','{2}','{3}'); return false;", OnClientSelected, arg[0], arg[1], arg[2]);
    }
}
