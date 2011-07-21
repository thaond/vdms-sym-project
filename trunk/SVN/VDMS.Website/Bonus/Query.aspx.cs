using System;
using VDMS.Helper;
 
public partial class Bonus_Query : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gvPlans.Columns[0].Visible = !UserHelper.IsDealer;
            gvPlans.Columns[1].Visible = !UserHelper.IsDealer;
            gvPlans.Columns[5].Visible = !UserHelper.IsDealer;
            if (UserHelper.IsDealer)
            {
                txtDealer.Text = UserHelper.DealerCode;
                txtDealer.Enabled = false;
                trPlanName.Visible = false;
            }
        }
    }
    protected void btnFind_Click(object sender, EventArgs e)
    {
        gvPlans.DataBind();
    }
}
