using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using Resources;
using VDMS.Common.Utils;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.IDAL.Interface;
using VDMS.Data.TipTop;
using VDMS.Helper;
using VDMS.I.ObjectDataSource;
using VDMS.I.Service;

public partial class Service_ExchangeVoucher : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bllMessage.Items.Clear();
        if (!IsPostBack)
        {

            ddlDealer.DataTextField = "BranchCode";
            ddlDealer.DataValueField = "BranchCode";
            ddlDealer.DataSource = Dealer.GetListDealer(AreaHelper.Area);
            ddlDealer.DataBind();
            if (UserHelper.IsDealer) ddlDealer.SelectedValue = UserHelper.DealerCode;
            ddlDealer.Enabled = !UserHelper.IsDealer;
            fromDate = ExchangeVoucherBO.GetFromDate(ddlDealer.SelectedValue);

            BindData();
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        ConfigForm();
    }

    #region Controls State
    protected void Page_Init(object sender, EventArgs e)
    {
        this.Page.RegisterRequiresControlState(this);
    }
    protected override void LoadControlState(object savedState)
    {
        object[] ctlState = (object[])savedState;
        base.LoadControlState(ctlState[0]);
        this.fromDate = (DateTime)ctlState[1];
        this.toDate = (DateTime)ctlState[2];
        this.status = (int)ctlState[3];
    }
    protected override object SaveControlState()
    {
        object[] ctlState = new object[4];
        ctlState[0] = base.SaveControlState();
        ctlState[1] = this.fromDate;
        ctlState[2] = this.toDate;
        ctlState[3] = this.status;
        return ctlState;
    }
    #endregion

    #region Fields
    DateTime fromDate;// = ExchangeVoucherBO.GetFromDate((!UserHelper.IsDealer) ? ddlDealer.SelectedValue : UserHelper.DealerCode);
    DateTime toDate = DateTime.Now;
    int status;
    string dealercode;
    #endregion

    #region Common Method
    private int LoadParamFromUI()
    {
        int res = 0;
        if (!DateTime.TryParse(txtFromDate.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out fromDate))
        {
            res = 1;
        }
        if (!DateTime.TryParse(txtToDate.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out toDate))
        {
            res = 2;
        }
        status = int.Parse(ddlStatus.SelectedValue);
        dealercode = ddlDealer.SelectedValue;
        return res;
    }
    private void SaveParamToUI()
    {
        txtFromDate.Text = fromDate.ToShortDateString();
        txtToDate.Text = toDate.ToShortDateString();
        ddlStatus.DataBind();
        ddlStatus.SelectedValue = status.ToString();
        //ddlDealer.SelectedValue = dealercode;
    }
    private void ConfigForm()
    {
        switch (ddlStatus.SelectedValue)
        {
            case "0":

                if (ExchangeVoucherBO.CheckExistExchangeVoucher())
                {
                    string dealerCode = (!UserHelper.IsDealer) ? ddlDealer.SelectedValue : UserHelper.DealerCode;
                    txtFromDate.Text = ExchangeVoucherBO.GetFromDate(dealerCode).ToShortDateString();
                    //txtFromDate.ReadOnly = true;
                    //ibFromDate.Enabled = false;
                }
                else
                {
                    //if (fromDate != null)
                    //    //txtFromDate.Text = fromDate.ToShortDateString();
                    //else
                    //    //txtFromDate.Text = DateTime.Now.ToShortDateString();
                    //txtFromDate.ReadOnly = false;
                    //ibFromDate.Enabled = true;
                }

                break;
            case "1":
                txtFromDate.Text = fromDate.ToShortDateString();
                //txtFromDate.ReadOnly = false;
                //ibFromDate.Enabled = true;

                break;
            default:
                break;
        }

        switch (status)
        {
            case 0:
                // Neu gui cho VMEP VoucherNumber = Gen()
                lblVoucherNumber.Text = ExchangeVoucherBO.GenExchangeNumber(ddlDealer.SelectedValue);
                btnSend.Enabled = gvMain.Rows.Count > 0;
                break;

            case 1:
                // Khong gui cho VMEP VoucherNumber = ""
                lblVoucherNumber.Text = string.Empty;
                btnSend.Enabled = false;

                break;
            default:
                btnSend.Enabled = false;
                break;
        }
    }
    private void BindData()
    {
        SaveParamToUI();

        ExchangePartHeaderDataSource1.SelectParameters["fromDate"] = new Parameter("fromDate", TypeCode.String, fromDate.ToShortDateString());
        ExchangePartHeaderDataSource1.SelectParameters["toDate"] = new Parameter("toDate", TypeCode.String, toDate.ToShortDateString());
        ExchangePartHeaderDataSource1.SelectParameters["status"] = new Parameter("status", TypeCode.String, status.ToString());

        //gvMain.DataSourceID = "ExchangePartHeaderDataSource1";
        gvMain.DataBind();

        //ConfigForm();
    }
    private void SendToVMEP()
    {
        if (status == 0)
        {
            // Lay cac PartHeaderDataSource can gui
            // ExchangePartHeaderDataSource dsEph = new ExchangePartHeaderDataSource();
            List<Exchangepartheader> listEph = (new ExchangePartHeaderDataSource()).Select(fromDate, toDate, status, ddlDealer.SelectedValue);
            // Neu so luong lon hon 0 thi bat dau gui
            if (listEph.Count > 0)
            {
                using (TransactionBlock tran = new TransactionBlock())
                {
                    try
                    {
                        // Create new Exchangevoucherheader
                        IDao<Exchangevoucherheader, string> daoEvh = DaoFactory.GetDao<Exchangevoucherheader, string>();
                        Exchangevoucherheader evh = new Exchangevoucherheader();
                        evh.Createddate = DateTime.Now;
                        evh.Dealercode = ddlDealer.SelectedValue;
                        evh.Lastprocesseddate = toDate;
                        evh.Id = ExchangeVoucherBO.GenExchangeNumber(ddlDealer.SelectedValue);
                        evh.Status = (int)ExchangeVoucherStatus.Sent;
                        evh = daoEvh.Save(evh);

                        // Sent Exchangepartheader;
                        IDao<Exchangepartheader, long> daoEph = DaoFactory.GetDao<Exchangepartheader, long>();
                        foreach (Exchangepartheader eph in listEph)
                        {
                            if (eph.Status == (int)ExchangeVoucherStatus.New)
                            {
                                eph.Status = (int)ExchangeVoucherStatus.Sent;
                                eph.Exchangevoucherheader = evh;
                                daoEph.SaveOrUpdate(eph);
                            }
                        }
                        tran.IsValid = true;
                    }
                    catch
                    {
                        tran.IsValid = false;
                    }
                }
            }
            BindData();
        }
    }
    #endregion

    #region Eval Method
	//protected string EvalDate(object objDate)
	//{
	//    string res = string.Empty;
	//    try
	//    {
	//        DateTime dtDate = (DateTime)objDate;
	//        res = dtDate.ToShortDateString();
	//    }
	//    catch { }
	//    return res;
	//}
    //EvalLinkServiceRecordSheetNo(Eval("Serviceheader.Servicesheetnumber"))
    protected string EvalLink(string sParamNanme, object oParam)
    {
        string res = "#";
        try
        {
            res = string.Format("WarrantyContent.aspx?{0}={1}", sParamNanme, oParam.ToString());
        }
        catch { }
        return res;
    }
    #endregion

    #region Event Method
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int err = LoadParamFromUI();
        switch (err)
        {
            case 0:
                BindData();
                break;
            case 1:
                bllMessage.Items.Add(Resources.Constants.DateInvalid);
                txtFromDate.Focus();
                break;
            case 2:

                bllMessage.Items.Add(Resources.Constants.DateInvalid);
                txtToDate.Focus();
                break;
        }
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        SendToVMEP();
    }
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        ConfigForm();
        // force update grid
        btnSubmit_Click(sender, e);
    }
    #endregion

    #region Event Method Gridview
    protected void gvMain_DataBound(object sender, EventArgs e)
    {
        try
        {
            //Literal litPageInfo = gvMain.TopPagerRow.FindControl("litPageInfo") as Literal;
            //if (litPageInfo != null) litPageInfo.Text = string.Format(Resources.Message.PagingInfo, gvMain.PageIndex + 1, gvMain.PageCount, HttpContext.Current.Items["rowCount"]);

            List<Exchangepartheader> listEph = (new ExchangePartHeaderDataSource()).Select(fromDate, toDate, status, ddlDealer.SelectedValue);
            if (listEph.Count > 0)
            {
                long TotalQuantity = 0;
                long TotalPartCost = 0;
                long TotalHireCost = 0;
                long TotalTotal = 0;

                foreach (Exchangepartheader eph in listEph)
                {
                    List<Exchangepartdetail> listEpd = ExchangeVoucherBO.GetExPartDetailByExPartHeader(eph);
                    long Total = 0;
                    if (listEpd.Count > 0)
                    {
                        long Quantity = 0;
                        long PartCost = 0;
                        Total = 0;
                        foreach (Exchangepartdetail epd in listEpd)
                        {
                            Quantity = Quantity + epd.Partqtym;
                            PartCost = PartCost + (long)epd.Partqtym * epd.Unitpricem;
                        }
                        TotalQuantity = TotalQuantity + Quantity;
                        TotalPartCost = TotalPartCost + PartCost;
                        Total = PartCost + eph.Feeamount;
                    }
                    TotalHireCost = TotalHireCost + eph.Feeamount;
                    TotalTotal = TotalTotal + Total;
                }

                Literal litTotalQuantity = gvMain.FooterRow.FindControl("litTotalQuantity") as Literal;
                if (litTotalQuantity != null) litTotalQuantity.Text = NumberFormatHelper.NumberToCurentMoneyFormatString(TotalQuantity);

                Literal litTotalPartCost = gvMain.FooterRow.FindControl("litTotalPartCost") as Literal;
                if (litTotalPartCost != null) litTotalPartCost.Text = NumberFormatHelper.NumberToCurentMoneyFormatString(TotalPartCost);

                Literal litTotalHireCost = gvMain.FooterRow.FindControl("litTotalHireCost") as Literal;
                if (litTotalHireCost != null) litTotalHireCost.Text = NumberFormatHelper.NumberToCurentMoneyFormatString(TotalHireCost);

                Literal litTotalTotal = gvMain.FooterRow.FindControl("litTotalTotal") as Literal;
                if (litTotalTotal != null) litTotalTotal.Text = NumberFormatHelper.NumberToCurentMoneyFormatString(TotalTotal);
            }
        }
        catch
        {
            bllMessage.Items.Add(e.ToString());
        }
    }

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = (gvMain.PageSize * gvMain.PageIndex + e.Row.RowIndex + 1).ToString();

            long Quantity = 0;
            long PartCost = 0;
            long HireCost = 0;
            long Total = 0;

            Literal litQuantity = e.Row.FindControl("litQuantity") as Literal;
            Literal litPartCost = e.Row.FindControl("litPartCost") as Literal;
            Literal litHireCost = e.Row.FindControl("litHireCost") as Literal;
            Literal litTotal = e.Row.FindControl("litTotal") as Literal;

            Exchangepartheader eph = ExchangeVoucherBO.GetExPartHeader(long.Parse(gvMain.DataKeys[e.Row.RowIndex][0].ToString()));
            List<Exchangepartdetail> listEpd = ExchangeVoucherBO.GetExPartDetailByExPartHeader(eph);
            if (listEpd.Count > 0)
            {
                foreach (Exchangepartdetail epd in listEpd)
                {
                    Quantity = Quantity + epd.Partqtym;
                    PartCost = PartCost + (long)epd.Partqtym * epd.Unitpricem;
                }
            }
            HireCost = eph.Feeamount;
            Total = PartCost + eph.Feeamount;
            if (litQuantity != null) litQuantity.Text = NumberFormatHelper.NumberToCurentMoneyFormatString(Quantity);
            if (litPartCost != null) litPartCost.Text = NumberFormatHelper.NumberToCurentMoneyFormatString(PartCost);
            if (litHireCost != null) litHireCost.Text = NumberFormatHelper.NumberToCurentMoneyFormatString(HireCost);
            if (litTotal != null) litTotal.Text = NumberFormatHelper.NumberToCurentMoneyFormatString(Total);
        }
    }
    #endregion
    protected void btnCancel_Load(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        btn.Text = Constants.Cancel;
        //btn.CssClass = (btn.CommandArgument == ((int)ExchangeVoucherStatus.New).ToString()) ? "__visible" : "hidden";
        btn.Visible = (btn.CommandArgument == ((int)ExchangeVoucherStatus.New).ToString()) ? true : false;
    }
    protected void btnRecover_Load(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        btn.Text = Constants.Recover;
        //btn.CssClass = (btn.CommandArgument == ((int)ExchangeVoucherStatus.Canceled).ToString()) ? "__visible" : "hidden";
        btn.Visible = (btn.CommandArgument == ((int)ExchangeVoucherStatus.Canceled).ToString()) ? true : false;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ExchangeVoucherHeaderDataSource.ChangeExchangeVoucherStatus(((Button)sender).ToolTip, ExchangeVoucherStatus.Canceled);
        BindData();
    }
    protected void btnRecover_Click(object sender, EventArgs e)
    {
        ExchangeVoucherHeaderDataSource.ChangeExchangeVoucherStatus(((Button)sender).ToolTip, ExchangeVoucherStatus.New);
        BindData();
    }
    protected void ddlDealer_DataBound(object sender, EventArgs e)
    {
        DropDownList drop = (DropDownList)sender;
        foreach (ListItem item in drop.Items)
        {
            item.Text += "      " + DealerHelper.GetNameI(item.Text);
        }
    }
    protected void ddlDealer_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnSubmit_Click(sender, e);
    }
}
