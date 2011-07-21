using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using VDMS.I.ObjectDataSource.RepairHistory;

public partial class Service_tempSRS : BasePage
{
    protected string FormatDate(object val)
    {
        if (val == null) return "";
        DateTime dt = (DateTime)val;

        return (dt > DateTime.MinValue) ? (dt.ToShortDateString()) : "";

    }
    protected string FormatNumber(object val)
    {
        string result = "";
        NumberFormatInfo ni = Thread.CurrentThread.CurrentCulture.NumberFormat;
        ni.NumberDecimalDigits = 0;
        if (val is double) result = ((double)val).ToString("N", ni);
        else if (val is long) result = ((long)val).ToString("N", ni);
        else if (val is int) result = ((int)val).ToString("N", ni);
        else if (val is float) result = ((float)val).ToString("N", ni);

        return result;
    }


    protected void InitData()
    {
        txtFrom.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToShortDateString();
        txtTo.Text = DateTime.Now.ToShortDateString();
        GridView1.EmptyDataText = Message.DataNotFound;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitData();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GridView1.DataSourceID = SRSObjectDataSource.ID;
        GridView1.DataBind();
    }
    protected void imgbDelete_Click(object sender, ImageClickEventArgs e)
    {
        long id;
        ImageButton btn = (ImageButton)sender;
        if (btn != null)
        {
            long.TryParse(btn.CommandArgument, out id);
            RepairHistoryDataSource.DeleteSRS(id);
            //Response.Redirect(Request.UrlReferrer.ToString());
            btnSearch_Click(null, null);
        }
    }
    protected void imgbDelete_DataBinding(object sender, EventArgs e)
    {
        ImageButton btn = (ImageButton)sender;
        btn.OnClientClick = string.Format("return confirm('{0}');", Resources.Question.DeleteData);
    }
    protected void GridView1_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        if (gv.TopPagerRow == null) return;
        Literal litPageInfo = gv.TopPagerRow.FindControl("litPageInfo") as Literal;
        if (litPageInfo != null) litPageInfo.Text = string.Format(Message.PagingInfo, gv.PageIndex + 1, gv.PageCount, HttpContext.Current.Items["TempRowCount"]);
    }
    protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {

    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton ibtn = (ImageButton)sender;
        Response.Redirect(string.Format("WarrantyContent.aspx?sid={0}", ibtn.CommandArgument));
    }
}
