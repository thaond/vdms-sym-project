using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.BonusSystem;

public partial class Admin_Bonus_BonusSource : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        BonusSourceDAO.Insert(txtName.Text.Trim(), txtDesc.Text.Trim());
        gv.DataBind();
    }
}
