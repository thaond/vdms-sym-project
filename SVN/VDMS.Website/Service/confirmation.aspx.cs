using System;
using System.Collections.ObjectModel;
using System.Web;
using System.Web.UI.WebControls;
using Resources;
using VDMS.I.Service;
using VDMS.Helper;

public partial class Service_confirmation : BasePage
{
    private Collection<ConfirmExchangeErrorCode> errorCode = new Collection<ConfirmExchangeErrorCode>();
    private void ShowError()
    {
        bllErrorMsg.Visible = errorCode.Count > 0;
        bllErrorMsg.Items.Clear();
        foreach (ConfirmExchangeErrorCode error in errorCode)
        {
            switch (error)
            {
                case ConfirmExchangeErrorCode.InvalidDateTime: bllErrorMsg.Items.Add(Message.WarrantyContent_InvalidDateTimeValue); break;
            }
        }
    }
    protected void AddError(ConfirmExchangeErrorCode error)
    {
        if ((errorCode.Contains(error)) || (error == ConfirmExchangeErrorCode.OK)) return;
        errorCode.Add(error);
    }

    private void ListExchangeStatus(DropDownList drop)
    {
        drop.Items.Clear();
        drop.Items.Add(new ListItem(ExchangeVoucherStatusString.All, "-1"));
        drop.Items.Add(new ListItem(ExchangeVoucherStatusString.NotVerify, ((int)ExchangeVoucherStatus.Sent).ToString()));
        drop.Items.Add(new ListItem(ExchangeVoucherStatusString.Verifing, ((int)ExchangeVoucherStatus.Verifing).ToString()));
        drop.Items.Add(new ListItem(ExchangeVoucherStatusString.Verified, ((int)ExchangeVoucherStatus.Verified).ToString()));
        drop.SelectedIndex = 1;
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        ShowError();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFromDate.Text = DateTimeHelper.FirstDayInMonth.ToShortDateString();
            txtToDate.Text = DateTime.Now.ToShortDateString();
            ListExchangeStatus(ddlStatus);
        }
        errorCode.Clear();
        //EmptyGridView1.EmptyTableRowText = Message.DataNotFound;
    }
    protected void EmptyGridView1_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        if (gv.TopPagerRow == null) return;
        Literal litPageInfo = gv.TopPagerRow.FindControl("litPageInfo") as Literal;
        if (litPageInfo != null) litPageInfo.Text = string.Format(Message.PagingInfo, gv.PageIndex + 1, gv.PageCount, HttpContext.Current.Items["ConfirmationExchangeRowCount"]);
    }

    protected void EmptyGridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView gv = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = ((int)((gv.PageIndex * gv.PageSize) + e.Row.RowIndex + 1)).ToString();
        }
    }

    // date time
    protected void Literal8_DataBinding(object sender, EventArgs e)
    {
        DateTime dt;
        Literal lit = (sender as Literal);
        if (DateTime.TryParse(lit.Text, out dt)) lit.Text = dt.ToShortDateString();
    }
    // status
    protected void Literal9_DataBinding(object sender, EventArgs e)
    {
        int status;
        Literal lit = (sender as Literal);
        if (int.TryParse(lit.Text, out status)) lit.Text = ServiceTools.GetNativeVMEPExchangeStatusName(status);
    }
    // dealer name
    protected void Literal6_DataBinding(object sender, EventArgs e)
    {
        Literal lit = (sender as Literal);
        lit.Text = DealerHelper.GetName(lit.Text);
    }
    // verify command
    protected void Button1_DataBinding(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        //if (btn.CommandArgument == ((int)ExchangeVoucherStatus.Sent).ToString())
        btn.OnClientClick = "window.open('verifyExchangeVoucher.aspx?pcvn=" + btn.OnClientClick + "','VerifyExhange',''); return false;";
        //else btn.Visible = false;
    }
    protected void cmdSubmit_Click(object sender, EventArgs e)
    {
        bool ok = true;
        DateTime dtFrom, dtTo;
        if (!DateTime.TryParse(txtFromDate.Text, out dtFrom)) { AddError(ConfirmExchangeErrorCode.InvalidDateTime); ok = false; }
        if (!DateTime.TryParse(txtToDate.Text, out dtTo)) { AddError(ConfirmExchangeErrorCode.InvalidDateTime); ok = false; }

        // bindding data
        if (ok)
        {
            EmptyGridView1.DataSourceID = "odsExchangeHeader";
            EmptyGridView1.DataBind();
        }
    }
    protected void EmptyGridView1_DataBinding(object sender, EventArgs e)
    {

    }
}
