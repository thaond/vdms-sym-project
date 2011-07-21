using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Common.Web;

public partial class Service_LookupWarrantyParts : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        gv.DataBind();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        gv.PageIndex = 0;
        gv.AllowPaging = false;
        gv.DataBind();
        GridView2Excel.Export(gv, "WarrantyParts.xls");
        gv.AllowPaging = true;
    }
}
