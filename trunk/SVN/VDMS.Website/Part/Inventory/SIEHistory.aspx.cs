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
using VDMS.Common.Utils;
using VDMS.II.Entity;
using VDMS.II.BasicData;

public partial class Part_Inventory_SIEHistory : BasePage
{
    protected void BindWarehouse()
    {
        if (ddlDealer.Items.Count == 0) ddlDealer.DataBind();

        ddlWarehouse.DealerCode = ddlDealer.SelectedValue;
        ddlWarehouse.DataBind();
    }
    protected string EvalWarehouse(object wh)
    {
        if (wh == null) return "";
        Warehouse w = WarehouseDAO.GetWarehouse((long)wh);
        return (w == null) ? "" : w.Address;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindWarehouse();
            txtFromDate.Text = DataFormat.DateOfFirstDayInMonth(DateTime.Now).ToShortDateString();
            txtToDate.Text = DateTime.Now.ToShortDateString();
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(gv.DataSourceID))
            gv.DataSourceID = "odsTrans";
        else
            gv.DataBind();
    }

    protected void Unnamed3_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindWarehouse();
    }

}
