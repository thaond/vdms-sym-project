using System;
using System.Data.Linq;
using VDMS.II.Entity;
using VDMS.II.Report;

public partial class Controls_ExcelTemplate_InShortOrder : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected object EvalShort(object orderDetail)
    {
        return InShortOrderDAO.GetInshortPartList((EntitySet<OrderDetail>)orderDetail);
    }

}
