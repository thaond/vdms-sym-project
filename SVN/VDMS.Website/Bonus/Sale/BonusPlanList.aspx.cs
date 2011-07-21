using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Bonus.Entity;

public partial class Bonus_Sale_BonusPlanList : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
        }
    }
    protected void ImageButton1_DataBinding(object sender, EventArgs e)
    {
        ImageButton imb = (ImageButton)sender;
        imb.Visible = imb.CommandArgument == BonusPlanStatus.Normal;
    }
    protected void btnFind_Click(object sender, EventArgs e)
    {
        gvPlans.DataBind();
    }
}
