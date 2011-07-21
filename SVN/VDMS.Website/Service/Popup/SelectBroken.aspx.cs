using System;
using System.Web;
using System.Web.UI.WebControls;
using Resources;
using VDMS.Helper;
using VDMS.I.Service;

public partial class Service_Popup_SelectBroken : BasePopup
{
    protected void InitForm()
    {
        gvSelectBroken.PageSize = SrsSetting.selectBrokenCodePageSize;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitForm();
        }
    }

    protected void gvSelectxxx_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView gv = (GridView)sender;
        if ((e.CommandName == "Page") && (((e.CommandArgument == "Next") && ((gv.PageIndex + 1) == gv.PageCount)) || ((e.CommandArgument == "Prev") && (gv.PageIndex == 0))))
        {
            gv.DataBind();
        }
    }
    protected void gvSelectxxx_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        if (gv.TopPagerRow == null) return;
        Literal litPageInfo = gv.TopPagerRow.FindControl("lit" + gv.ID + "PageInfo") as Literal;
        if (litPageInfo != null) litPageInfo.Text = string.Format(Message.PagingInfo, gv.PageIndex + 1, gv.PageCount, HttpContext.Current.Items["list" + gv.ID + "RowCount"]);
    }

    protected void btnSearchBroken_Click(object sender, EventArgs e)
    {
        gvSelectBroken.DataSourceID = "odsSelectBroken";
        gvSelectBroken.DataBind();
    }

    protected void btnSelectBroken_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        PopupHelper.SetSelectBrokenSession(this.Key, btn.CommandArgument);
        ClosePopup("brokenSelected()");
    }
}
