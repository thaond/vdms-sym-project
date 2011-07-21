using System;
using System.Threading;
using System.Web.UI.WebControls;
using Resources;
using VDMS.I.Service;
using VDMS.Helper;

public partial class Service_LookupExchange : BasePage
{
    private void ListExchangeStatus(DropDownList drop)
    {
        drop.Items.Clear();
        drop.Items.Add(new ListItem(ExchangeVoucherStatusString.All, "-1"));
        drop.Items.Add(new ListItem(ExchangeVoucherStatusString.New, ((int)ExchangeVoucherStatus.New).ToString()));
        drop.Items.Add(new ListItem(ExchangeVoucherStatusString.Sent, ((int)ExchangeVoucherStatus.Sent).ToString()));
        drop.Items.Add(new ListItem(ExchangeVoucherStatusString.Approved, ((int)ExchangeVoucherStatus.Approved).ToString()));
        drop.Items.Add(new ListItem(ExchangeVoucherStatusString.Reject, ((int)ExchangeVoucherStatus.Reject).ToString()));
        //drop.Items.Add(new ListItem(ExchangeVoucherStatusString.UnKnown, ((int)ExchangeVoucherStatus.UnKnown).ToString()));
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ListExchangeStatus(ddlStatus);
            txtToDate.Text = DateTime.Now.ToShortDateString();
            txtFromDate.Text = DateTimeHelper.FirstDayInMonth.ToShortDateString();
        }

        //DateTime dtFrom, dtTo, dtConFrom, dtConto;
        //DateTime.TryParse(txtFromDate.Text, Thread.CurrentThread.CurrentCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out dtFrom); evReport.RepairDateFrom = dtFrom;
        //DateTime.TryParse(txtToDate.Text, Thread.CurrentThread.CurrentCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out dtTo); evReport.RepairDateTo = dtTo;
        //DateTime.TryParse(txtComfirmedTo.Text, Thread.CurrentThread.CurrentCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out dtConto); evReport.ConfirmedTo = dtConto;
        //DateTime.TryParse(txtConfirmedFrom.Text, Thread.CurrentThread.CurrentCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out dtConFrom); evReport.ConfirmedFrom = dtConFrom;

        //evReport.DealerCode = txtDealerCode.Text.Trim();
        //evReport.EngineNumber = "%" + txtEngineNumber.Text.Trim() + "%";
        //evReport.VoucherNumberFrom = txtVoucherFrom.Text.Trim();
        //evReport.VoucherNumberTo = txtVoucherTo.Text.Trim();
        //evReport.Status = Convert.ToInt32(ddlStatus.SelectedValue);
        //evReport.OnVerify = false;
        //evReport.ShowAllPageSummary = false;
        //evReport.ShowEditButton = false;
        //evReport.ShowVerifyButton = false;
        //evReport.ShowFooter = false;

        evReport.RepairDateFrom = txtFromDate.Text;
        evReport.RepairDateTo = txtToDate.Text;
        evReport.ConfirmedTo = txtComfirmedTo.Text;
        evReport.ConfirmedFrom = txtConfirmedFrom.Text;

        evReport.DealerCode = txtDealerCode.Text.Trim().ToUpper();
        evReport.EngineNumber = txtEngineNumber.Text.Trim().ToUpper();
        evReport.VoucherNumberFrom = txtVoucherFrom.Text.Trim().ToUpper();
        evReport.VoucherNumberTo = txtVoucherTo.Text.Trim().ToUpper();
        evReport.Status = string.IsNullOrEmpty(ddlStatus.SelectedValue) ? null : (int?)int.Parse(ddlStatus.SelectedValue);
        evReport.OnVerify = false;
        evReport.ShowAllPageSummary = false;
        evReport.ShowEditButton = false;
        evReport.ShowVerifyButton = false;
        evReport.ShowFooter = false;
    }
    protected void cmdSubmit_Click(object sender, EventArgs e)
    {
        evReport.BindData();
    }
}
