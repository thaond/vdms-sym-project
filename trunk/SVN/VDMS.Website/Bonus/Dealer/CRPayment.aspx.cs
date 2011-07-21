using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.BonusSystem;
using VDMS.I.Vehicle;
using VDMS.I.Entity;

public partial class Bonus_Dealer_CRPayment : BasePopup
{
    private long OrderId
    {
        get
        {
            return Request.QueryString["oid"] == null ? 0 : long.Parse(Request.QueryString["oid"]);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    protected void gv_DataBound(object sender, EventArgs e)
    {
        if (gv.Rows.Count > 0)
        {
            long sumTotalAmount = 0, sumUsingAmount = 0, amnt;
            foreach (GridViewRow r in gv.Rows)
            {
                var rv = (RangeValidator)r.FindControl("rvAmount");
                ((TextBox)r.FindControl("txtUsingAmount")).Text = "0";
                rv.MinimumValue = "0";
                rv.MaximumValue = r.Cells[3].Text;
                var p = OrderBonus.GetEditingPayment(long.Parse(r.Cells[0].Text));
                if (p != null && !p.Deleted)
                {
                    ((TextBox)r.FindControl("txtUsingAmount")).Text = p.Amount.ToString();       
                }
                sumTotalAmount += long.TryParse(r.Cells[3].Text, out amnt) ? amnt : 0;
                sumUsingAmount += long.TryParse(((TextBox)r.FindControl("txtUsingAmount")).Text, out amnt) ? amnt : 0;

                r.Cells[3].Text = long.Parse(r.Cells[3].Text).ToString("C0");
            }
            gv.FooterRow.Cells[3].Text = sumTotalAmount.ToString("C0");
            gv.FooterRow.Cells[4].Text = sumUsingAmount.ToString("C0");
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!IsValid)
            return;
        long tmp;
        int count = 0;
        foreach (GridViewRow r in gv.Rows)
        {
            count++;
            var usingAmount = long.TryParse(((TextBox)r.Cells[4].FindControl("txtUsingAmount")).Text, out tmp) ? tmp : 0;
            if (usingAmount > 0)
            {
                var paymentId = long.TryParse(r.Cells[0].Text, out tmp) ? tmp : 0;
                var orgPayment = OrderBonusDAO.GetOrderPayment(paymentId);
                var newPayment = new SaleOrderPayment
                {
                    Amount = (decimal)usingAmount,
                    CreatedDate = DateTime.Now,
                    DealerCode = orgPayment.DealerCode,
                    PaymentDate = orgPayment.PaymentDate,
                    PaymentType = OrderPaymentType.RemainingPayment,
                    ToBank = orgPayment.ToBank,
                    UserName = VDMS.Helper.UserHelper.Username,
                    VoucherNumber = orgPayment.VoucherNumber + " used",
                    Description = "Use remaining money",
                    OriginalSaleOrderPaymentId = orgPayment.OrderPaymentId,
                };
                OrderBonus.AddPayment(newPayment, OrderId);
            }
        }
        ClosePopup("updated()");
    }
}
