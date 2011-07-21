using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.PartManagement.Order;

public partial class Part_Inventory_ReplacePart : BasePopup 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtOldPart.Text = Request.QueryString["code"];
            txtNewPart.Text = "";
        }
    }
    protected void btnFind_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(gv.DataSourceID))
            gv.DataSourceID = ods.ID;
        else
            gv.DataBind();
    }
    protected void gv_SelectedIndexChanged(object sender, EventArgs e)
    {
        var part = PartOrderDAO.Parts.SingleOrDefault(p => p.Line.ToString() == Request.QueryString["line"]);
        if (part != null)
        {
            //part.PartCode = gv.SelectedRow.Cells[1].Text;
        }
        ClosePopup(string.Format("replacePart('{0}','{1}')", gv.SelectedRow.Cells[1].Text, Request.QueryString["caller"]));
    }
}
