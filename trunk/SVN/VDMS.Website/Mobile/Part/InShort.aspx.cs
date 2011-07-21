using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Web.UI.WebControls;
using VDMS.Common.Utils;
using VDMS.Common.Web;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.Report;

public partial class MPart_Inventory_InShort : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFromDate.Text = DataFormat.DateOfFirstDayInMonth(DateTime.Now).ToShortDateString();
            txtToDate.Text = DateTime.Now.ToShortDateString();
        }
    }
    protected object EvalShort(object orderDetail)
    {
        return InShortOrderDAO.GetInshortPartList((EntitySet<OrderDetail>)orderDetail);
    }
    protected void cmdQuery_Click(object sender, EventArgs e)
    {
        lv.DataSourceID = odsShortOrder.ID;
        lv.DataBind();
    }
    protected void cmd2Excel_Click(object sender, EventArgs e)
    {
        IList<OrderHeader> data = new InShortOrderDAO().FindInShortOrder(txtOrderNumber.Text.Trim(), txtFromDate.Text, txtToDate.Text);
        if (data.Count > 0)
        {
            string dtFrom = DataFormat.DateFromString(txtFromDate.Text).ToString("yyyy_MM_dd");
            string dtTo = DataFormat.DateFromString(txtToDate.Text).ToString("yyyy_MM_dd");
            string fileName = string.Format("InShort.{0}.{1}-{2}.[{3}].xls", UserHelper.DealerCode, dtFrom, dtTo, txtOrderNumber.Text);

            ListView lvExcel = (ListView)Page.LoadControl("~/Controls/ExcelTemplate/InShortOrder.ascx").Controls[0];
            lvExcel.DataSource = data;
            lvExcel.DataBind();

            GridView2Excel.Export(lvExcel, fileName);
        }
    }
}
