using System;
using VDMS.Data.TipTop;
using VDMS.II.Linq;

public partial class sql : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void cmdRun_Click(object sender, EventArgs e)
    {
        gv.DataSource = DataObjectBase.ExecuteSql(txtSql.Text);
        gv.DataBind();
    }
    protected void btnUpdateReceive_Click(object sender, EventArgs e)
    {
        var dc = DCFactory.GetDataContext<PartDataContext>();
        foreach (var o in dc.OrderHeaders)
        {
            VDMS.II.PartManagement.Order.OrderDAO.UpdateOrderDelivery(dc, o.OrderHeaderId);
        }
        dc.SubmitChanges();
    }
}
