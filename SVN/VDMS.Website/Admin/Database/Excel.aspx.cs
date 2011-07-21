using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using VDMS.WebService.Service;
using VDMS.Common.Web;



public partial class Admin_Database_Excel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var sv = new Common();
            ddlTables.DataTextField = ddlTables.DataValueField = "TNAME";
            ddlViews.DataTextField = ddlViews.DataValueField = "TNAME";

            ddlTables.DataSource = sv.RunQCommand("select * from tab where TABTYPE = 'TABLE'");
            ddlViews.DataSource = sv.RunQCommand("select * from tab where TABTYPE = 'VIEW'");

            ddlTables.DataBind();
            ddlViews.DataBind();

            ddlTables.Items.Insert(0, "");
            ddlViews.Items.Insert(0, "");
        }
    }
    public void BindData()
    {
        if (string.IsNullOrEmpty(GridView1.DataSourceID)) GridView1.DataSourceID = ObjectDataSource1.ID;
        else GridView1.DataBind();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        string name = !string.IsNullOrEmpty(ddlTables.Text) ? ddlTables.Text : (string.IsNullOrEmpty(ddlViews.Text) ? "query" : ddlViews.Text);
        BindData();
        GridView2Excel.Export(GridView1, name);
    }
}
