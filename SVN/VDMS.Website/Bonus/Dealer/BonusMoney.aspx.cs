using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Bonus.Entity;
using VDMS.Bonus.Linq;
using VDMS.II.Linq;
using VDMS.I.Linq;
using VDMS.I.Entity;
using VDMS.II.BonusSystem;

public partial class Bonus_Dealer_BonusMoney : BasePopup
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
        var dc = DCFactory.GetDataContext<VehicleDataContext>();
        var order = dc.OrderHeaders.SingleOrDefault(o => o.OrderHeaderId == OrderId);
        if (order != null)
        {
            odsBonusMoney.SelectParameters["dealerCode"].DefaultValue = order.DealerCode;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        long tmp;
        var dc = DCFactory.GetDataContext<BonusDataContext>();
        foreach (GridViewRow r in gv.Rows)
        {
            var usingAmount = long.TryParse(((TextBox)r.Cells[4].FindControl("txtUsingAmount")).Text, out tmp) ? tmp : 0;
            if (usingAmount > 0)
            {
                var newBonus = new BonusTransaction();
                var bonusDetail = dc.BonusPlanDetails.SingleOrDefault(d => d.BonusPlanDetailId == (long.TryParse(r.Cells[0].Text, out tmp) ? tmp : 0));
                newBonus.BonusPlanDetailId = bonusDetail.BonusPlanDetailId;
                newBonus.Amount = long.TryParse(((TextBox)r.Cells[5].FindControl("txtUsingAmount")).Text, out tmp) ? tmp : 0;
                newBonus.OrderId = OrderId;
                newBonus.BonusSourceId = bonusDetail.BonusSourceId;
                newBonus.DealerCode = bonusDetail.DealerCode;
                newBonus.TransactionType = BonusTransactionType.OrderSubstract;
                newBonus.Description = "Bonus used";
                newBonus.BonusSourceName = bonusDetail.BonusSource.BonusSourceName;
                OrderBonus.AddBonus(newBonus, OrderId);
            }
        }
        ClosePopup("updated()");
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
                rv.MaximumValue = r.Cells[4].Text;
                var b = OrderBonus.GetEditingBonus(long.Parse(r.Cells[0].Text));
                if (b != null && !b.Deleted)
                {
                    ((TextBox)r.FindControl("txtUsingAmount")).Text = b.Amount.ToString();
                }
                sumTotalAmount += long.TryParse(r.Cells[4].Text, out amnt) ? amnt : 0;
                sumUsingAmount += long.TryParse(((TextBox)r.FindControl("txtUsingAmount")).Text, out amnt) ? amnt : 0;

                r.Cells[4].Text = long.Parse(r.Cells[4].Text).ToString("C0");
            }
            gv.FooterRow.Cells[4].Text = sumTotalAmount.ToString("C0");
            gv.FooterRow.Cells[5].Text = sumUsingAmount.ToString("C0");
        }
    }
}
