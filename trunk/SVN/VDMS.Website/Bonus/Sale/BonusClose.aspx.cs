using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Bonus_Sale_BonusClose : BasePage
{
    DateTime? ActDate { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        InitErrMsgControl(_err);
    }
    protected DateTime? GetLockDate(object sender)
    {
        DateTime? res = null;
        var tb = (TextBox)((sender as WebControl).NamingContainer.FindControl("txtCloseMonth"));
        if (tb != null && !tb.ReadOnly && !string.IsNullOrEmpty(tb.Text))
        {
            var val = tb.Text.Split('/');
            if (val.Length == 2)
                try
                {
                    res = new DateTime(int.Parse(val[1]), int.Parse(val[0]), 1);
                }
                catch { return null; }
        }

        return res;
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Close":
                if (!BonusCloser.Close(e.CommandArgument.ToString(), GetLockDate(e.CommandSource)))
                    AddErrorMsg(errInvalidLockMonth.Text);
                else
                    GridView1.DataBind();
                break;
            case "Open":
                if (!BonusCloser.Open(e.CommandArgument.ToString()))
                    AddErrorMsg(errBonusNeverClosed.Text);
                else
                    GridView1.DataBind();
                break;
            default:
                break;
        }
    }
    protected void btnFind_Click(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }
}
