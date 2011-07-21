using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.I.Entity;
using VDMS.II.BonusSystem;
using VDMS.Helper;
using VDMS.Bonus.Entity;

public partial class Bonus_Dealer_OrderPayment : BasePopup
{
    public Guid PaymentId
    {
        get
        {
            return Request.QueryString["pid"] == null ? Guid.Empty : new Guid(Request.QueryString["pid"]);
        }
    }
    public long OrderId
    {
        get
        {
            return Request.QueryString["oid"] == null ? 0 : long.Parse(Request.QueryString["oid"]);
        }
    }
    SaleOrderPayment _PaymentInfo;
    public SaleOrderPayment PaymentInfo
    {
        get
        {
            if (_PaymentInfo == null) _PaymentInfo = PaymentId == Guid.Empty ? null : OrderBonus.GetEditingPayment(PaymentId);
            return _PaymentInfo;
        }
        set { _PaymentInfo = value; }
    }

    public void Close()
    {
        //  register the script to close the popup
        //Page.ClientScript.RegisterStartupScript(typeof(Bonus_Dealer_OrderPayment), "closeThickBox", "self.parent.updated();", true);
        ClosePopup("updated()");
    }
    public void LoadInfo(Guid id)
    {
        if (PaymentInfo != null)
        {
            dlFromBank.SelectedValue = PaymentInfo.FromBank;
            dlToBank.SelectedValue = PaymentInfo.ToBank;
            dlFromAcc.Text = PaymentInfo.FromAccount;
            dlToAcc.Text = PaymentInfo.ToAccount;
            txtVoucher.Text = PaymentInfo.VoucherNumber;
            txtAmount.Text = PaymentInfo.Amount.ToString();
            txtComment.Text = PaymentInfo.Description;
        }
    }
    public void SaveInfo()
    {
        bool newPay = false;
        if (PaymentInfo == null)
        {
            PaymentInfo = new SaleOrderPayment()
              {
                  CreatedDate = DateTime.Now,
                  PaymentDate = DateTime.Now,
                  UserName = UserHelper.Username,
                  PaymentType = OrderPaymentType.FromDealer,
              };
            newPay = true;
        }
        PaymentInfo.Amount = long.Parse(txtAmount.Text);
        PaymentInfo.Description = txtComment.Text.Trim();
        PaymentInfo.LastEditDate = DateTime.Now;
        PaymentInfo.VoucherNumber = txtVoucher.Text.Trim().ToUpper();

        PaymentInfo.FromBank = dlFromBank.SelectedValue;
        PaymentInfo.FromAccount = dlFromAcc.Text;
        PaymentInfo.FromAccountHolder = txtFromAcc.Text;
        PaymentInfo.ToBank = dlToBank.SelectedValue;
        PaymentInfo.ToAccount = dlToAcc.Text;
        PaymentInfo.ToAccountHolder = txtToAcc.Text;

        if (newPay) OrderBonus.AddPayment(PaymentInfo, OrderId);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (PaymentId != Guid.Empty) LoadInfo(PaymentId);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveInfo();
        Close();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Close();
    }

    protected void dlFromBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        dlFromAcc.BankCode = dlFromBank.SelectedValue;
        dlFromAcc.DataBind();
    }
    protected void dlToBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        dlToAcc.BankCode = dlToBank.SelectedValue;
        dlToAcc.DataBind();
    }
    protected void dlFromAcc_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFromAcc.Text = dlFromAcc.SelectedValue;
    }
    protected void dlToAcc_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtToAcc.Text = dlToAcc.SelectedValue;
    }
}
