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
using VDMS;

public partial class Vehicle_Fin_ConfirmPayment : BasePage
{
    public string PageKey { get; set; }
    public string OrderNumber
    {
        get
        {
            return Request.QueryString["on"];
        }
    }
    public string Action
    {
        get
        {
            return Request.QueryString["act"];
        }
    }

    protected int EvalPayClass(long pay, long total, long bonus)
    {
        if (pay + bonus == total) return 1;
        if (pay + bonus < total && pay > 0) return 2;
        if (pay == 0) return 3;
        return 0;
    }
    protected string EvalWarnClass(long pay, long total, long bonus)
    {
        return (pay + bonus) >= total ? "" : " errorText";
    }
    protected string EvalPayInfo(long id, int pClass, int status)
    {
        return string.Format("{0},{1},{2}", id, pClass, status);
    }

    public void ShowPSSelector()
    {
        var list = PaymentManager.GetOrdersConfirm(PageKey);
        litTotalReady.Text = list
                                //.Where(o => o.Selected)
                                .Count().ToString();
        //btnConfirmSelected.Enabled = lv.Items.Count > 0;

        var fp = (int)LoadState("FullyPaid");
        var pp = (int)LoadState("PartlyPaid");
        var up = (int)LoadState("UnPaid");
        var ap = (int)LoadState("AllPaid");

        rblSelector.Items[0].Text = rblSelector.Items[0].Value + " (" + ap.ToString() + ")  ";
        rblSelector.Items[1].Text = rblSelector.Items[1].Value + " (" + fp.ToString() + ")  ";
        rblSelector.Items[2].Text = rblSelector.Items[2].Value + " (" + pp.ToString() + ")  ";
        rblSelector.Items[3].Text = rblSelector.Items[3].Value + " (" + up.ToString() + ")  ";
    }
    protected void BindData()
    {
        if (string.IsNullOrEmpty(lv.DataSourceID)) lv.DataSourceID = odsIP.ID;
        else lv.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        InitInfoMsgControl(_msg);
        InitErrMsgControl(_error);
        if (!IsPostBack)
        {
            PageKey = Guid.NewGuid().ToString();
            SaveState("PageKey", PageKey);

            txtFrom.Text = DataFormat.DateOfFirstDayInMonth(DateTime.Now).ToShortDateString();
            txtTo.Text = DateTime.Now.ToShortDateString();
            if (!string.IsNullOrEmpty(OrderNumber))
            {
                txtOrderFrom.Text = txtOrderTo.Text = OrderNumber;
                txtFrom.Text = txtTo.Text = "";
            }
            if (Action == "q")
            {
                btnFind_Click(sender, e);
            }
        }
        else
        {
            PageKey = (string)LoadState("PageKey");
        }

    }
    protected void btnFind_Click(object sender, EventArgs e)
    {
        if (rblSelector.SelectedIndex == -1) rblSelector.SelectedIndex = 1;
        PaymentManager.GetOrdersConfirm(PageKey).Clear();
        BindData();
    }
    protected void btnConfirmThis_DataBinding(object sender, EventArgs e)
    {
        var b = sender as Button;
        SetOpenForm(b, string.Format("ConfirmPayment.aspx?on={0}&act=q", b.CommandArgument));
    }
    protected void btnConfirmSelected_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            if (PaymentManager.ConfirmPayment(PageKey))
            {
                AddInfoMsg(Message.ActionSucessful);
                lv.DataSourceID = null;
                lv.DataBind();
            }
            else AddErrorMsg(Errors.ErrorsFound);
        }
    }

    protected void odsIP_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.ReturnValue is IQueryable<OrderHeader>)
        {
            // calculate values
            var ap = (int)e.OutputParameters["ap"]; SaveState("AllPaid", ap);
            var fp = (int)e.OutputParameters["fp"]; SaveState("FullyPaid", fp);
            var pp = (int)e.OutputParameters["pp"]; SaveState("PartlyPaid", pp);
            var up = (int)e.OutputParameters["up"]; SaveState("UnPaid", up);

            // setup layout
            ShowPSSelector();
        }
    }
    protected void odsIP_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        odsIP.SelectParameters["key"].DefaultValue = PageKey;
    }

    protected void UpdateOrder(object sender, EventArgs e)
    {
        var c = (WebControl)sender;
        var cb = ((CheckBox)c.NamingContainer.FindControl("chbConfirm"));
        var vc = ((TextBox)c.NamingContainer.FindControl("txtTipTopVoucher"));
        var dc = ((TextBox)c.NamingContainer.FindControl("txtDesc"));

        var val = cb.ToolTip.Split(',');
        var id = long.Parse(val[0]);

        var os = PaymentManager.GetOrdersConfirm(PageKey);
        var oi = os.SingleOrDefault(o => o.Id == id);

        // update
        if (oi == null)
        {
            oi = new ConfirmOrderInfo() { Id = id, };
            os.Add(oi);
        }
        oi.Selected = cb.Checked;
        oi.Desc = dc.Text.Trim();
        oi.Voucher = vc.Text.Trim().ToUpper();

        os = PaymentManager.GetOrdersConfirm(PageKey);
        // update GUI
        ShowPSSelector();
        if (cb.Checked && val[1] != rblSelector.SelectedIndex.ToString()) rblSelector.SelectedIndex = -1;
    }
    protected void chbConfirm_DataBinding(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        var vc = ((TextBox)cb.NamingContainer.FindControl("txtTipTopVoucher"));
        var dc = ((TextBox)cb.NamingContainer.FindControl("txtDesc"));

        var val = cb.ToolTip.Split(',');
        var list = PaymentManager.GetOrdersConfirm(PageKey);

        var k = long.Parse(val[0]);
        var oi = list.SingleOrDefault(o => o.Id == k);

        // overide select status
        if (oi != null) cb.Checked = oi.Selected;
        else
        {
            if (val[2] == "5") cb.Checked = true;
        }

        cb.Enabled = val[2] == "2" || (VDMSSetting.CurrentSetting.AllowUndoVehiclePaymentConfirm && val[2] == "5");
        vc.ReadOnly = dc.ReadOnly = !cb.Enabled;
        if (oi != null)
        {
            vc.Text = oi.Voucher;
            dc.Text = oi.Desc;
        }
        // chua dung dc, phai support truong hop undo (=> ++ chu ko phai --)
        //cb.InputAttributes["onchange"] = "javascript:updateCount(this);";
    }
    protected void rblSelector_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblSelector.SelectedIndex >= 0)
            btnFind_Click(sender, e);
    }

    protected void cvOrderInfo_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = !PaymentManager.GetOrdersConfirm(PageKey).Exists(o => o.Selected && string.IsNullOrEmpty(o.Voucher));
    }
}
