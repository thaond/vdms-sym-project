using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using VDMS.Common.Utils;
using VDMS.Common.Web;
using VDMS.Core.Domain;
using VDMS.Helper;
using VDMS.I.Entity;
using VDMS.I.ObjectDataSource;
using VDMS.I.Service;

public partial class Service_Popup_SelectEngineNo : BasePopup
{

    public string EvalDate(object date)
    {
        DateTime dt;
        if ((date == null) ||
            !DateTime.TryParse(date.ToString(), new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out dt))
        {
            return "";
        }
        return dt.ToShortDateString();
    }
    protected void InitForm()
    {
        ddlDealer.Items[1].Value = string.IsNullOrEmpty(UserHelper.DealerCode) || (UserHelper.DealerCode == "/") ? Request.QueryString["dl"] : UserHelper.DealerCode;
        gvSelectEngine.PageSize = SrsSetting.selectGridViewPageSize;
    }
    protected string SelectAnEngineOnGrid(GridViewRow row)
    {

        Label lbImpDate = WebTools.FindControlById("lbImpDate", row) as Label;
        Literal litColorName = WebTools.FindControlById("litItemColorName", row) as Literal;
        Literal litModel = WebTools.FindControlById("litItemModel", row) as Literal;
        Literal litEngineNo = WebTools.FindControlById("litItemEngineNo", row) as Literal;
        HiddenField hdColorCode = WebTools.FindControlById("hdItemColorCode", row) as HiddenField;
        HiddenField hdDealerCode = WebTools.FindControlById("hdItemDealerCode", row) as HiddenField;
        HiddenField hdBranchCode = WebTools.FindControlById("hdItemBranchCode", row) as HiddenField;
        HiddenField hdDatabaseCode = WebTools.FindControlById("hdItemDatabaseCode", row) as HiddenField;
        HiddenField hdMadeDate = WebTools.FindControlById("hdItemMadeDate", row) as HiddenField;
        HiddenField hdID = WebTools.FindControlById("hdItemID", row) as HiddenField;

        return SelectAnEngineNo(litEngineNo.Text, hdID.Value, litModel.Text, hdColorCode.Value, hdDealerCode.Value, hdBranchCode.Value, hdDatabaseCode.Value, hdMadeDate.Value, lbImpDate.Text);
    }
    protected void SelectAnEngine(ItemInstance item)
    {
        PopupHelper.SetSelectEngineSession(this.Key, item);
    }
    protected string SelectAnEngineNo(string litEngineNo, string hdID, string litModel, string hdColorCode, string hdDealerCode, string hdBranchCode, string hdDatabaseCode, string hdMadeDate, string lbImpDate)
    {
        DateTime impDate = DataFormat.DateFromString(lbImpDate);
        DateTime madeDate = DataFormat.DateFromString(hdMadeDate);
        long id;
        long.TryParse(hdID, out id);

        //Iteminstance item = new Iteminstance(hdDealerCode, litEngineNo, litModel, hdColorCode, impDate, -1, madeDate,
        //    string.Empty, string.Empty, hdBranchCode, DateTime.MinValue, DateTime.MinValue, hdDatabaseCode,
        //    null);
        ItemInstance item = new ItemInstance
                                {
                                    ItemInstanceId = id,
                                    DealerCode = hdDealerCode,
                                    EngineNumber = litEngineNo,
                                    ItemType = litModel,
                                    Color = hdColorCode,
                                    ImportedDate = impDate,
                                    Status = -1,
                                    MadeDate = madeDate,
                                    VMEPInvoice = string.Empty,
                                    Comments = string.Empty,
                                    BranchCode = hdBranchCode,
                                    DatabaseCode = hdDatabaseCode
                                };
        
        SelectAnEngine(item);

        return item.EngineNumber;
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
        if (e.CommandName == "Page")
        {
            if ((((string)e.CommandArgument == "Next") && ((gv.PageIndex + 1) == gv.PageCount)) || (((string)e.CommandArgument == "Prev") && (gv.PageIndex == 0)))
            {
                gv.DataBind();
            }
            udpSelectEngineNo.Update();
        }
    }
    protected void gvSelectxxx_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        if (gv.TopPagerRow == null) return;
        Literal litPageInfo = gv.TopPagerRow.FindControl("lit" + gv.ID + "PageInfo") as Literal;
        if (litPageInfo != null) litPageInfo.Text = string.Format(Message.PagingInfo, gv.PageIndex + 1, gv.PageCount, HttpContext.Current.Items["list" + gv.ID + "RowCount"]);
    }

    protected void btnSearchEngine_Click(object sender, EventArgs e)
    {
        HttpContext.Current.Session["TestSearchEngine"] = string.Format("\nStart at session {0} \n", Session.SessionID);

        string engineNo = (chbUsePrefix.Checked) ? ddlPrefix.SelectedValue + txtSearchEngineNo.Text.Trim() + "%" : txtSearchEngineNo.Text;
        //string engineNo = ddlPrefix.SelectedValue + txtSearchEngineNo.Text.Trim();
        odsSelectEngine.SelectParameters["engineNumberLike"] = new Parameter("engineNumberLike", System.Data.DbType.AnsiString, engineNo);
        gvSelectEngine.DataSourceID = "odsSelectEngine";
        gvSelectEngine.DataBind();

        //HttpContext.Current.Session["TestSearchEngine"] = HttpContext.Current.Session["TestSearchEngine"].ToString() + string.Format("\nEnd at session {0} \n", Session.SessionID);
        //hdTestEngine.Value = HttpContext.Current.Session["TestSearchEngine"].ToString();

        if (gvSelectEngine.Rows.Count > 0)
        {
            HiddenField hdDatabaseCode = WebTools.FindControlById("hdItemDatabaseCode", gvSelectEngine.Rows[0]) as HiddenField;
            HiddenField hdID = WebTools.FindControlById("hdItemID", gvSelectEngine.Rows[0]) as HiddenField;

            lbDataSource.Text = (hdID.Value == "-1") ? "None" :
                ((hdID.Value == "0") ? hdDatabaseCode.Value : "VDMS");
        }
    }

    protected void btnSelectEngine_Click(object sender, EventArgs e)
    {
        GridViewRow row = (sender as Control).NamingContainer as GridViewRow;
        ClosePopup(string.Format("engineSelected('{0}')", SelectAnEngineOnGrid(row)));
    }

    protected void btnSelectEngine_DataBinding(object sender, EventArgs e)
    {
        //Button btn = sender as Button;
        //GridViewRow row = btn.NamingContainer as GridViewRow;

        //btn.OnClientClick = string.Format("SelectEngine({0}); return false;", row.RowIndex);
        //btn.OnClientClick = string.Format("onServerSelectedEngine('{0}');", btn.CommandArgument);
    }

    protected void btnUseEngineNumber_Click(object sender, EventArgs e)
    {
        string enNo = ddlPrefix.SelectedValue + txtSearchEngineNo.Text.Trim() + "%";
        ItemInstanceDataSource ds = new ItemInstanceDataSource();
        IList<ItemInstance> items = ds.SelectForSRS(1, 0, enNo, ddlDealer.SelectedValue);

        if (items.Count > 0)
        {
            SelectAnEngine(items[0]);
        }
        else
        {
            SelectAnEngineNo(enNo, "-1", "", "", "", "", "", "", "");
        }

        ClosePopup(string.Format("engineSelected('{0}')", enNo));
    }

    protected void btnSearchPrefix_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtModel.Text.Trim()))
        {
            ddlPrefix.DataSourceID = null;
            ddlPrefix.Items.Clear();
        }
        else
        {
            ddlPrefix.DataSourceID = odsEnginePrefix.ID;
            ddlPrefix.DataBind();
        }
    }
}
