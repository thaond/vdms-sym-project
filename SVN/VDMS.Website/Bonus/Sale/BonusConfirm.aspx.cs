using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.I.Vehicle;
using VDMS.Common.Utils;
using VDMS.I.Entity;
using VDMS.II.BonusSystem;
using VDMS.Bonus.Linq;
using VDMS.Bonus.Entity;
using VDMS.II.Linq;

public partial class Bonus_Sale_BonusConfirm : BasePage
{
    public string GetBonusStatus(string status)
    {
        var i = ddlStatus.Items.FindByValue(status);
        return i == null ? "" : i.Text;
    }
    public bool CanChangeBonus(int status, long amount, string dealerCode, string bStatus)
    {
        var res = VDMS.I.Vehicle.OrderStatusAct.CanChangeBonusStatus(status) && amount > 0;

        var actAmt = bStatus == OrderBonusStatus.Confirmed ? amount : -amount;
        var bAmt = BonusPlans.GetDealerBonus(dealerCode);
        res = res && (bAmt + actAmt) >= 0;

        return res;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        InitErrMsgControl(_err);
        if (!IsPostBack)
        {
            txtFrom.Text = DataFormat.DateOfFirstDayInMonth(DateTime.Now).ToShortDateString(); ;
            txtTo.Text = DateTime.Now.ToShortDateString();
        }
    }
    protected void btnFind_Click(object sender, EventArgs e)
    {
        lv.DataBind();
    }
    protected void chbBConfirmed_DataBinding(object sender, EventArgs e)
    {

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        var bt = (Button)sender;
        var nc = (ListViewDataItem)bt.NamingContainer;
        var am = (TextBox)nc.FindControl("txtBonus");
        var cb = (CheckBox)nc.FindControl("chbBConfirmed");
        var cm = (TextBox)nc.FindControl("txtVMEPComment");

        try
        {
            var newAmount = long.Parse(am.Text);
            var dc = DCFactory.GetDataContext<BonusDataContext>();


            // sua transaction dua theo so thuong moi nhap
            var transactions = dc.BonusTransactions.Where(b => b.OrderId == long.Parse(bt.CommandArgument) && b.TransactionType == BonusTransactionType.OrderSubstract).OrderBy(b => b.Amount);
            var currentTransAmount = -transactions.Sum(b => b.Amount);
            var diff = newAmount - currentTransAmount;
            foreach (var trans in transactions)
            {
                if (diff != 0)
                {
                    var oldAmount = -trans.Amount;
                    //trans.Amount = trans.Amount - diff;
                    var planAmountRemaining = trans.BonusPlanDetail.Amount + trans.BonusPlanDetail.BonusTransactions.Where(t => t.TransactionType == BonusTransactionType.OrderSubstract).Sum(t => t.Amount) + oldAmount;
                    if (-diff >= oldAmount)
                    {
                        dc.BonusTransactions.DeleteOnSubmit(trans);
                    }
                    if (planAmountRemaining > 0 && newAmount > planAmountRemaining)
                    {
                        trans.Amount = -planAmountRemaining;
                        diff = diff + oldAmount;
                        oldAmount = planAmountRemaining;
                    }
                    else
                    {
                        trans.Amount = -(diff + currentTransAmount);
                    }
                    
                    diff = diff - oldAmount;
                }
            }

            dc.SubmitChanges();

            OrderDAO.ChangeBonus(long.Parse(bt.CommandArgument),
                (long)transactions.Sum(t => t.Amount),
                cb.Checked, cm.Text.Trim());

        }
        catch (Exception ex)
        {
            AddErrorMsg(ex.Message);
        }

        lv.DataBind(); // refresh info 
    }
}
