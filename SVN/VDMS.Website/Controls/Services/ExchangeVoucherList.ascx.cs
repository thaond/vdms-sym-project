using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.I.Entity;
using VDMS.I.Linq;
using VDMS.I.Service;
using System.Web.UI.HtmlControls;
using Resources;
using VDMS.Helper;

public partial class Controls_Services_ExchangeVoucherList : BaseUserControl
{
    const bool ALLOW_VERIFY_MULTI_TIME = true;
    const bool ALLOW_OVERWRITE_BATCHVERIFY = true && ALLOW_VERIFY_MULTI_TIME; // must combine with ALLOW_VERIFY_MULTI_TIME

    #region public properties

    public string VoucherNumberFrom { get; set; }
    public string VoucherNumberTo { get; set; }
    public string ProposalNumber { get; set; }
    public string EngineNumber { get; set; }
    public string DealerCode { get; set; }

    public string RepairDateFrom { get; set; }
    public string RepairDateTo { get; set; }
    public string ConfirmedFrom { get; set; }
    public string ConfirmedTo { get; set; }
    public int? Status { get; set; }

    // behavior
    public bool OnVerify { get; set; }
    public bool ShowEditButton { get; set; }
    public bool ShowVerifyButton { get; set; }
    public bool ShowFooter { get; set; }
    public bool ShowAllPageSummary { get; set; }

    // info
    public List<string> ErrorList = new List<string>();
    public bool IsEditing { get; set; }

    #endregion

    public void BindData()
    {
        //lvEv.EditIndex = 1;
        lvEv.DataBind();
    }

    protected int GetApproveedStatus() { return (int)ExchangeVoucherStatus.Approved; }
    protected int GetRejectStatus() { return (int)ExchangeVoucherStatus.Reject; }  // check duplicate
    protected bool IsNotSame(object val1, object val2)
    {
        bool result = false;
        result = (val1.ToString() == val2.ToString()) ? false : true;
        return result;
    }
    // khong cho phep duyet nhieu lan (ko thi lay cai tren)
    protected bool IsNotSame2(object val1, object val2)
    {
        return (ALLOW_VERIFY_MULTI_TIME) ?
                    IsNotSame(val1, val2) :
                    ((((int)ExchangeVoucherStatus.Approved).ToString() == val2.ToString()) ||
                     (((int)ExchangeVoucherStatus.Reject).ToString() == val2.ToString())) ? false : true;
    }
    public string EvalStatus(object status)
    {
        return ServiceTools.GetNativeVMEPExchangeStatusName((int)status);
    }
    protected double GetDouble(string txt, Control findIn)
    {
        double val;
        TextBox tb = (TextBox)findIn.FindControl(txt);
        if (tb == null) return 0;
        double.TryParse(tb.Text, out val);
        return val;
    }

    protected bool UpdateDetails(int index)
    {
        var ok = true;
        ListView lv = (ListView)lvEv.Items[index].FindControl("lvParts");
        foreach (var item in lv.Items)
        {
            var p = ExchangeVoucherBO.DC.ExchangePartDetails.SingleOrDefault(i => i.ExchangePartDetailId == (long)lv.DataKeys[item.DataItemIndex].Value);
            if (p != null)
            {
                var tb = (TextBox)item.FindControl("txtPartCodeM");
                p.PartCodeM = tb.Text;
                tb = (TextBox)item.FindControl("txtPartQtyM");
                p.PartQtyM = (string.IsNullOrEmpty(tb.Text)) ? 0 : int.Parse(tb.Text);
                tb = (TextBox)item.FindControl("txtVMEPComment");
                p.VMEPComment = tb.Text;
                tb = (TextBox)item.FindControl("txtUnitPriceM");
                p.UnitPriceM = (string.IsNullOrEmpty(tb.Text)) ? 0 : decimal.Parse(tb.Text);

                if (!p.LoadWarrCond(ExchangeVoucherBO.DC))
                {
                    ErrorList.Add(string.Format("{0} : {1}", p.PartCodeM, Errors.NotExistPartCode));
                    ok = false;
                    continue;
                }

                double manP = GetDouble("txtManPowerM", item);
                p.TotalFeeM = (decimal)(manP * (double)p.LabourM);
                p.ExchangePartHeader.LastProcessedDate = DateTime.Now;
            }
        }
        if (ok) ExchangeVoucherBO.DC.SubmitChanges();
        return ok;
    }

    // overWrite == false : donot over Write status if it verified
    private void BatchVerify(bool overWrite, ExchangeVoucherStatus status)
    {
        bool hasErr = false;
        using (var tran = new VDMS.Data.DAL.NHibernateDAL.TransactionBlock())
        {
            for (int i = 0; i < lvEv.Items.Count; i++)
            {
                var res = ExchangeVoucherBO.ChangeExchangeStatus((long)lvEv.DataKeys[i].Value, status, overWrite);
                if (res != VerifyExchangeErrorCode.OK) { AddError(res); hasErr = true; break; }
            }
            // need rollback ????
            if (!hasErr) ExchangeVoucherBO.DC.SubmitChanges();
            //if (!hasErr) Response.Redirect(Request.Url.ToString(), true);
        }
    }
    public void ApproveAllRemain()
    {
        //BatchVerify(true, ExchangeVoucherStatus.Approved);
        BindData();
    }
    public void RejectAllRemain()
    {
        //BatchVerify(true, ExchangeVoucherStatus.Reject);
        BindData();
    }
    public void RejectAllPageRemain()
    {
        BatchVerify(ALLOW_OVERWRITE_BATCHVERIFY, ExchangeVoucherStatus.Reject);
        BindData();
    }
    public void ApproveAllPageRemain()
    {
        BatchVerify(ALLOW_OVERWRITE_BATCHVERIFY, ExchangeVoucherStatus.Approved);
        BindData();
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // init datasource
            odsH.SelectParameters.Add(new ControlParameter("vnFrom", System.Data.DbType.String, this.ID, "VoucherNumberFrom"));
            odsH.SelectParameters.Add(new ControlParameter("vnTo", System.Data.DbType.String, this.ID, "VoucherNumberTo"));
            odsH.SelectParameters.Add(new ControlParameter("propNo", System.Data.DbType.String, this.ID, "ProposalNumber"));
            odsH.SelectParameters.Add(new ControlParameter("enNo", System.Data.DbType.String, this.ID, "EngineNumber"));
            odsH.SelectParameters.Add(new ControlParameter("dCode", System.Data.DbType.String, this.ID, "DealerCode"));
            odsH.SelectParameters.Add(new ControlParameter("repairFromDt", System.Data.DbType.String, this.ID, "RepairDateFrom"));
            odsH.SelectParameters.Add(new ControlParameter("repairToDt", System.Data.DbType.String, this.ID, "RepairDateTo"));
            odsH.SelectParameters.Add(new ControlParameter("confirmFromDt", System.Data.DbType.String, this.ID, "ConfirmedFrom"));
            odsH.SelectParameters.Add(new ControlParameter("confirmToDt", System.Data.DbType.String, this.ID, "ConfirmedTo"));
            odsH.SelectParameters.Add(new ControlParameter("status", System.Data.DbType.Object, this.ID, "Status"));
            odsH.SelectParameters.Add(new Parameter("pageSum", System.Data.DbType.Object));

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        bllErrorMsg.Items.Clear();
        if (!IsPostBack)
        {
            // setup select spare control
            selectSpare.OnClientSelected = "selectSpare";
            selectSpare.OnCancelSelect = "cancelSelect";
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        DataPager pg = (DataPager)lvEv.FindControl("pager");
        Control total = lvEv.FindControl("allPageTotal");
        if ((total == null) || (pg == null)) return;

        total.Visible = ShowAllPageSummary
                        && pg.PageSize < pg.TotalRowCount
                        ;
        if (total.Visible)
        {
            var sum = ExchangeVoucherBO.SummaryExchangeHeaders(VoucherNumberFrom, VoucherNumberTo, ProposalNumber, EngineNumber, DealerCode, RepairDateFrom, RepairDateTo, ConfirmedFrom, ConfirmedTo, Status);
            (lvEv.FindControl("litQuantityA") as ITextControl).Text = sum.TotalQuantityO.ToString("N0");
            (lvEv.FindControl("litPartsCostA") as ITextControl).Text = sum.TotalPartCostO.ToString("N0");
            (lvEv.FindControl("litFeeA") as ITextControl).Text = sum.ProposeFeeAmount.ToString("N0");
            (lvEv.FindControl("litAmountA") as ITextControl).Text = sum.TotalAmountO.ToString("N0");

            (lvEv.FindControl("lbQuantityA") as ITextControl).Text = sum.TotalQuantityM.ToString("N0");
            (lvEv.FindControl("lbPartsCostA") as ITextControl).Text = sum.TotalPartCostM.ToString("N0");
            (lvEv.FindControl("lbFeeA") as ITextControl).Text = sum.TotalFeeM.ToString("N0");
            (lvEv.FindControl("lbAmountA") as ITextControl).Text = sum.TotalAmountM.ToString("N0");
        }
    }
    protected void AddError(VerifyExchangeErrorCode err)
    {
        switch (err)
        {
            case VerifyExchangeErrorCode.WrongFormat: bllErrorMsg.Items.Add(Message.UpdateDataFailed_WrongData); break;
            case VerifyExchangeErrorCode.CommentIsBlank: bllErrorMsg.Items.Add(litErrMsgCommentBlank.Text); break;
            default: break;
        }
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        int hid;
        int.TryParse(((IButtonControl)sender).CommandArgument, out hid);
        AddError(ExchangeVoucherBO.ChangeExchangeStatus(hid, ExchangeVoucherStatus.Approved));
        //Response.Redirect(Request.Url.ToString(), true);
        BindData();
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        int hid;
        int.TryParse(((IButtonControl)sender).CommandArgument, out hid);
        AddError(ExchangeVoucherBO.ChangeExchangeStatus(hid, ExchangeVoucherStatus.Reject));
        //Response.Redirect(Request.Url.ToString(), true);
        BindData();
    }


    protected void lvEv_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        IsEditing = true;
    }
    protected void lvEv_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {
        e.Cancel = !UpdateDetails(e.ItemIndex);
        IsEditing = e.Cancel;
    }

    protected void selSpare_Load(object sender, EventArgs e)
    {
        HtmlInputButton ctrl = (HtmlInputButton)sender;
        TextBox tb = (TextBox)ctrl.NamingContainer.FindControl("txtPartCodeM");
        TextBox tb1 = (TextBox)ctrl.NamingContainer.FindControl("txtUnitPriceM");
        TextBox tb2 = (TextBox)ctrl.NamingContainer.FindControl("txtManPowerM");
        TextBox tb3 = (TextBox)ctrl.NamingContainer.FindControl("txtPartCodeM");
        ctrl.Attributes["onclick"] = string.Format("javascript:doSelSpareCode('{0}', '{1}', '{2}','{3}')", tb.ClientID, tb1.ClientID, tb2.ClientID,tb3.ClientID);
    }

    protected void odsH_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ExchangePartHeader sum = (ExchangePartHeader)e.OutputParameters["pageSum"];
        if (sum != null)
        {
            (lvEv.FindControl("litQuantity") as ITextControl).Text = sum.TotalQuantityO.ToString("N0");
            (lvEv.FindControl("litPartsCost") as ITextControl).Text = sum.TotalPartCostO.ToString("N0");
            (lvEv.FindControl("litFee") as ITextControl).Text = sum.ProposeFeeAmount.ToString("N0");
            (lvEv.FindControl("litAmount") as ITextControl).Text = sum.TotalAmountO.ToString("N0");

            (lvEv.FindControl("lbQuantity") as ITextControl).Text = sum.TotalQuantityM.ToString("N0");
            (lvEv.FindControl("lbPartsCost") as ITextControl).Text = sum.TotalPartCostM.ToString("N0");
            (lvEv.FindControl("lbFee") as ITextControl).Text = sum.TotalFeeM.ToString("N0");
            (lvEv.FindControl("lbAmount") as ITextControl).Text = sum.TotalAmountM.ToString("N0");

            //dealer info
            pnDealer.Visible = OnVerify;
            if (OnVerify)
            {
                litDealerName.Text = litDealerCode.Text = sum.DealerCode;
                //VDMS.II.Entity.Dealer d = VDMS.II.BasicData.DealerDAO.GetDealerByCode(sum.DealerCode);
                //if (d != null) litDealerName.Text = d.DealerName;
                litDealerName.Text = DealerHelper.GetNameI(sum.DealerCode);
            }
        }
    }

    protected void rovldComment_DataBinding(object sender, EventArgs e)
    {
        //(sender as CustomValidator).be
    }
}
