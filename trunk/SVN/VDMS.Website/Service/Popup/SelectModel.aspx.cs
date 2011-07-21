using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.I.Service;
using Resources;
using VDMS.Helper;

public partial class Service_Popup_SelectModel : BasePopup
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitForm();
        }
    }
    protected void InitForm()
    {
        gvSelectModel.PageSize = SrsSetting.selectGridViewPageSize;
    }

    protected void gvSelectxxx_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView gv = (GridView)sender;
        if (e.CommandName == "Page")
        {
            if (((e.CommandArgument == "Next") && ((gv.PageIndex + 1) == gv.PageCount)) || ((e.CommandArgument == "Prev") && (gv.PageIndex == 0)))
            {
                gv.DataBind();
            }
            udpSelectModel.Update();
        }
    }
    protected void gvSelectxxx_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        if (gv.TopPagerRow == null) return;
        Literal litPageInfo = gv.TopPagerRow.FindControl("lit" + gv.ID + "PageInfo") as Literal;
        if (litPageInfo != null) litPageInfo.Text = string.Format(Message.PagingInfo, gv.PageIndex + 1, gv.PageCount, HttpContext.Current.Items["list" + gv.ID + "RowCount"]);
    }
    protected void btnSelectModel_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        PopupHelper.SetSelectModelSession(this.Key, btn.CommandArgument);
        ClosePopup("modelSelected()");
    }
}
