using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.I.Vehicle;
using VDMS.I.Entity;
using Resources;
using VDMS.Common.Web;
using VDMS.Common.Utils;

public partial class Vehicle_Fin_ReConsign : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnReConsign.Enabled = false;
        InitInfoMsgControl(_msg);
        if (!IsPostBack)
        {
            txtFrom.Text = DataFormat.DateOfFirstDayInMonth(DateTime.Now).ToShortDateString();
            txtTo.Text = DateTime.Now.ToShortDateString();
        }
    }
    protected string EvalWarnClass(long pay, long total)
    {
        return pay >= total ? "" : " errorText";
    }
    protected void btnFind_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(lv.DataSourceID)) lv.DataSourceID = odsIP.ID;
        else lv.DataBind();
    }
    protected void gvPayment_DataBound(object sender, EventArgs e)
    {
        btnReConsign.Enabled = lv.Items.Count > 0;
    }

    protected void btnReConsign_Click(object sender, EventArgs e)
    {
        DateTime from = DataFormat.DateFromString(txtFrom.Text);
        DateTime to = DataFormat.DateFromString(txtTo.Text);
        if (PaymentManager.ReConsign(from, to, txtDealerCode.Text.Trim()))
        {
            AddInfoMsg(Message.ActionSucessful);
        }
    }
}
