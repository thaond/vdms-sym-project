using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using NHibernate;
using NHibernate.Expression;
using Resources;
using VDMS.I.Vehicle;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.TipTop;
using VDMS.Helper;

public partial class Vehicle_Sale_Store : BasePage
{
    protected string SumMoneyInvalid = Resources.Message.Cus_SumMoneyInvalid;
    protected string AtLeastOneItemSelect = Resources.Message.AtleastOneItemSelected;
    protected void Page_Load(object sender, EventArgs e)
    {
        rvRecDate = DateValid(rvRecDate);
        rvRecDate1 = DateValid(rvRecDate1);
        rvUHP1 = DateValid(rvUHP1);
        rvUHP2 = DateValid(rvUHP2);
        rvUHP3 = DateValid(rvUHP3);
        rvUHP4 = DateValid(rvUHP4);
        rvUHP5 = DateValid(rvUHP5);
        if (!Page.IsPostBack)
        {
            LoadSubShop();
            Session["GrdSlotItemInstance"] = null;
            txtRecCDate.Text = DateTime.Now.ToShortDateString();
            txtSellingDate.Text = DateTime.Now.ToShortDateString();
            // sell date must before "now"
            rvSellingDate.MaximumValue = DateTime.Now.ToShortDateString();
            rvSellingDate.MinimumValue = DateTime.MinValue.ToShortDateString();
        }
        ChangeDatetimeByLang(txtRecCDate.Text);
        GetLanguage();
        lbErr.Visible = false;
        SumMoneyInvalid = Resources.Message.Cus_SumMoneyInvalid;
        AtLeastOneItemSelect = Resources.Message.AtleastOneItemSelected;
    }
    protected void btnPlusEngine_Click(object sender, EventArgs e)
    {
        try
        {
            ISession sess = NHibernateSessionManager.Instance.GetSession();
            IList lstPlusItemIns = sess.CreateCriteria(typeof(Iteminstance))
                .Add(Expression.Eq("Enginenumber", txtEngineNo.Text.Trim()))
                .Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
                .Add(Expression.Eq("Branchcode", ddlStoreCode.SelectedValue))
                .Add(Expression.In("Status", ItemHepler.GetInstockItemStatus()))
                .List();

            if (lstPlusItemIns.Count > 0)
            {
                Iteminstance PlusItemIns = lstPlusItemIns[0] as Iteminstance;
                IList lstItemIns = (IList)Session["GrdSlotItemInstance"];
                bool isHavedItem = false; bool isFirstItem = false;
                if (lstItemIns == null)
                {
                    isFirstItem = true;
                    lstItemIns = lstPlusItemIns;
                }
                else
                {
                    foreach (Iteminstance ItemIns in lstItemIns)
                    {
                        if (PlusItemIns.Id == ItemIns.Id)
                        {
                            isHavedItem = true;
                        }
                    }
                }
                if (isHavedItem)
                {
                    ShowMessage(Resources.Message.EngineNumberIsHaved, true);
                    return;
                }
                if (!isFirstItem)
                {
                    lstItemIns.Insert(0, PlusItemIns);
                }
                Session["GrdSlotItemInstance"] = lstItemIns;
                gvItems.DataSource = lstItemIns;
                gvItems.DataMember = "Id";
                gvItems.DataBind();
            }
            else
            {
                ShowMessage(Resources.Message.EngineNumberNotFoundInDB, true);
                return;
            }
        }
        catch (Exception)
        {
            gvItems.Visible = false;
        }
    }
    protected void btnTest_Click(object sender, EventArgs e)
    {
        if (InventoryHelper.IsInventoryLock(DateTime.Now, UserHelper.DealerCode, ddlStoreCode.SelectedValue))
        {
            ShowMessage(Reports.InventoryIsLocked, true);
            return;
        }
        liSelectItemIns.Text = ""; gvItems.Visible = true;
        ISession sess = NHibernateSessionManager.Instance.GetSession();
        //Begin search condition
        string MotorCode = txtMotorCode.Text.Trim().ToUpper();
        string ColorCode = txtColorCode.Text.Trim().ToUpper();
        string StoreCode = ddlStoreCode.SelectedValue.ToUpper();
        //IList lstItemIns;
        ICriteria icrit = sess.CreateCriteria(typeof(Iteminstance));
        if (ColorCode != "")
            icrit.CreateCriteria("Item", "it").Add(Expression.Like("Colorcode", "%" + ColorCode + "%"));

        if (MotorCode != "")
            icrit.Add(Expression.Like("Itemtype", "%" + MotorCode + "%"));

        if (!string.IsNullOrEmpty(StoreCode))
            icrit.Add(Expression.Eq("Branchcode", StoreCode));

        icrit.Add(Expression.Eq("Dealercode", UserHelper.DealerCode));
        icrit.Add(Expression.In("Status", ItemHepler.GetInstockItemStatus()));

        #region Not show the item had sold for sub-shop

        //IList lstBatchInvoice = sess.CreateCriteria(typeof(Batchinvoicedetail))
        //    .CreateCriteria("Batchinvoiceheader", "bvh")
        //    .Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
        //    .Add(Expression.Eq("Branchcode", UserHelper.BranchCode))
        //    //.Add(Expression.Eq())
        //    .List();
        //Batchinvoicedetail biItems;
        //List<Int64> lstIDBatchInvoice = new List<Int64>();
        //for (int i = 0; i < lstBatchInvoice.Count; i++)
        //{
        //    biItems = lstBatchInvoice[i] as Batchinvoicedetail;
        //    if (biItems != null) lstIDBatchInvoice.Add(biItems.Iteminstance.Id);
        //}
        //if (lstBatchInvoice.Count > 0) icrit.Add(Expression.Not(Expression.In("Id", lstIDBatchInvoice)));
        
        icrit.Add(Expression.Sql("not exists(select * from (select enginenumber as eng from SALE_BATCHINVOICEDETAIL) T where enginenumber = T.eng)"));
        
        #endregion

        //icrit.SetFetchMode("Item", FetchMode.Join);
        IList lstItemIns = icrit.List();
        if (lstItemIns.Count > 0)
        {
            HttpContext.Current.Items["PaymentCount"] = lstItemIns.Count;
            btnCheckValid.Enabled = true;
            btnSave.Enabled = true;
        }
        else
        {
            ShowMessage(Resources.Message.NotFoundInDB, true);
            gvItems.Visible = false;
            btnSave.Enabled = false;
            btnCheckValid.Enabled = false;
            return;
        }
        //End search condition

        Session["GrdSlotItemInstance"] = lstItemIns;

        gvItems.DataSource = lstItemIns;
        gvItems.DataMember = "Id";
        gvItems.DataBind(); gvItems.Visible = true;
    }
    protected void btnCheckValid_Click(object sender, EventArgs e)
    {
        if (!checkDateRange(txtSellingDate.Text.Trim(), txtRecCDate.Text.Trim()) && !txtSellingDate.Text.Trim().Equals("")) { ShowMessage(Resources.Customers.RecDateSmallerThanSelDate, true); return; }
        if (!ValidDateFormat(txtRecCDate.Text.Trim())) { ShowMessage(Resources.Customers.RecDateInvalid, true); return; }

        #region UnFixedHire-Purchase
        if (ddlPaymentMethod.SelectedValue.Equals(((int)CusPaymentType.UnFixedHire_purchase).ToString()))
        {
            decimal SumCheck = 0;
            TextBox txtIntentDatePay, UHPtxtMoney, subtxtIntentDatePay;
            DateTime recDate, paidDate, subIntentDate;
            recDate = DateTime.MinValue;
            if (!txtRecCDate.Text.Trim().Equals(""))
                DateTime.TryParse(txtRecCDate.Text.Trim(), new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out recDate);

            for (int i = 1; i < 6; i++)
            {
                try
                {
                    txtIntentDatePay = (TextBox)UnfixedHP.FindControl("txtIntentDatePay" + i);
                    UHPtxtMoney = (TextBox)UnfixedHP.FindControl("UHPtxtMoney" + i);
                    if (txtIntentDatePay.Text.Trim() != "" && UHPtxtMoney.Text.Trim() != "")
                    {
                        SumCheck += decimal.Parse(UHPtxtMoney.Text.Trim());
                        DateTime.TryParse(txtIntentDatePay.Text.Trim(), new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out paidDate);
                        if (!paidDate.Equals(DateTime.MinValue) && paidDate < recDate)
                        {
                            ShowMessage(Resources.Customers.DateUFHPValid, true);
                            return;
                        }
                        for (int k = 1; k < 6; k++)
                        {
                            if (k == i) break;
                            else
                            {
                                subtxtIntentDatePay = (TextBox)UnfixedHP.FindControl("txtIntentDatePay" + k);
                                DateTime.TryParse(subtxtIntentDatePay.Text.Trim(), new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out subIntentDate);
                                if (paidDate.Equals(subIntentDate))
                                {
                                    ShowMessage(Resources.Customers.DateUFHPValid, true);
                                    return;
                                }
                            }
                        }
                        UHPtxtMoney.Text = Convert2Currency(UHPtxtMoney.Text);
                    }
                }
                catch (Exception) { throw new Exception("Error"); }
            }
            if (!SumCheck.Equals(decimal.Parse(txtPriceTax.Text)))
            {
                ShowMessage(SumMoneyInvalid, true);
                return;
            }
            else CloseSessionForHPUpdate((int)CusPaymentType.UnFixedHire_purchase);
        }
        #endregion
        #region Fixed Hire-purchase
        if (ddlPaymentMethod.SelectedValue.Equals(((int)CusPaymentType.FixedHire_purchase).ToString()))
        {
            if (decimal.Parse(txtFHPFirstMoney.Text) >= decimal.Parse(txtPriceTax.Text))
            {
                ShowMessage(Resources.Customers.MoneyValid, true);
                return;
            }
            if ((decimal.Parse(txtFHPTimes.Text) - 1) > 0)
            {
                BindEvalFHP();
                CloseSessionForHPUpdate((int)CusPaymentType.FixedHire_purchase);
            }
            else
            {
                ShowMessage(SumMoneyInvalid, true); return;
            }
        }
        #endregion
        txtPriceTax.Text = Convert2Currency(txtPriceTax.Text.Trim()); txtPriceTax.CssClass = "readOnlyInputField";
        btnCheckValid.Visible = false;
        btnSave.Visible = true;
    }
    protected string Convert2Currency(string ObConvert)
    {
        decimal ICurency = decimal.Parse(ObConvert);
        NumberFormatInfo nfi = Thread.CurrentThread.CurrentCulture.NumberFormat;
        nfi.NumberDecimalDigits = 0;
        return ICurency.ToString("N", nfi);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!checkDateRange(txtSellingDate.Text.Trim(), txtRecCDate.Text.Trim()) && !txtSellingDate.Text.Trim().Equals("")) { ShowMessage(Resources.Customers.RecDateSmallerThanSelDate, true); return; }
        if (!GetItemSelected())
        {
            ShowMessage(AtLeastOneItemSelect, true); return;
        }
        ISession sess = NHibernateSessionManager.Instance.GetSession();
        InsertNewData(ref sess);
        btnSave.Visible = false;
        btnCheckValid.Visible = true;
    }
    protected void ddlPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
        // default: check require price
        RqvPriceTax.Enabled = true;

        if (int.Parse(ddlPaymentMethod.SelectedValue) == (int)CusPaymentType.FixedHire_purchase)
        {
            if (txtFHPAllMoney.Text != "")
            {
                txtFHPAllMoney.Text = txtPriceTax.Text;
            }
            btnCheckValid.Visible = true;
            btnSave.Visible = false;
            FixedHP.Visible = true; UnfixedHP.Visible = false;
        }
        else if (int.Parse(ddlPaymentMethod.SelectedValue) == (int)CusPaymentType.UnFixedHire_purchase)
        {
            btnCheckValid.Visible = true;
            btnSave.Visible = false;
            FixedHP.Visible = false; UnfixedHP.Visible = true;
            TextBox txtIntentDatePay, UHPtxtMoney;
            CalendarExtender CEUHPCandelar;
            for (int i = 1; i < 6; i++)
            {
                try
                {
                    txtIntentDatePay = (TextBox)UnfixedHP.FindControl("txtIntentDatePay" + i);
                    UHPtxtMoney = (TextBox)UnfixedHP.FindControl("UHPtxtMoney" + i);
                    CEUHPCandelar = (CalendarExtender)UnfixedHP.FindControl("CEUHP" + i);
                    txtIntentDatePay.ReadOnly = false; txtIntentDatePay.CssClass = null; txtIntentDatePay.Text = "";
                    UHPtxtMoney.ReadOnly = false; UHPtxtMoney.CssClass = null; UHPtxtMoney.Text = "";
                    CEUHPCandelar.Enabled = true;
                }
                catch (Exception) { throw new Exception("Error"); }
            }
        }
        else
        {
            // ban 1 lan ko bat nhap Price
            RqvPriceTax.Enabled = false;

            btnCheckValid.Visible = false;
            btnSave.Visible = true;
            FixedHP.Visible = false;
            UnfixedHP.Visible = false;
        }
    }

    protected void gvItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        e.Cancel = true;
        GetItemSelected();
        gvItems.PageIndex = e.NewPageIndex; LoadOldGrid();
    }
    protected void gvItems_DataBound(object sender, EventArgs e)
    {
        string[] lstItemSelected = liSelectItemIns.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries); ;
        CheckBox cb; Label lb;
        foreach (GridViewRow gvr in gvItems.Rows)
        {
            cb = (CheckBox)gvr.FindControl("cbSelectItem");
            lb = (Label)gvr.FindControl("lbItemInstanceID");
            for (int i = 0; i < lstItemSelected.Length; i++)
            {
                if (lstItemSelected[i].Equals(lb.Text))
                {
                    cb.Checked = true;
                }
            }
        }
    }
    protected void AllCheckBox_CheckedChanged(object sender, EventArgs e)
    {

        CheckBox Controlcb = (CheckBox)sender;
        foreach (GridViewRow grv in gvItems.Rows)
        {
            CheckBox cb = (CheckBox)grv.FindControl("cbSelectItem");
            if (cb != null) cb.Checked = Controlcb.Checked;
        }
    }
    //protected void cmdNext_Click(object sender, EventArgs e)
    //{

    //    GetItemSelected();
    //    if (gvItems.PageIndex < gvItems.PageCount)
    //    {
    //        gvItems.PageIndex += 1; LoadOldGrid();
    //    }
    //}
    //protected void cmdPrevious_Click(object sender, EventArgs e)
    //{
    //    GetItemSelected();
    //    if (gvItems.PageIndex > 0)
    //    {
    //        gvItems.PageIndex -= 1; LoadOldGrid();
    //    }
    //}
    //protected void cmdFirst_Click(object sender, EventArgs e)
    //{
    //    GetItemSelected();
    //    gvItems.PageIndex = 0; LoadOldGrid();
    //}
    //protected void cmdLast_Click(object sender, EventArgs e)
    //{
    //    GetItemSelected();
    //    gvItems.PageIndex = gvItems.PageCount - 1; LoadOldGrid();
    //}

    private void LoadSubShop()
    {
        ISession sess = NHibernateSessionManager.Instance.GetSession();
        IList lstSubShop = sess.CreateCriteria(typeof(Subshop))
            .Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
            .Add(Expression.Eq("Status", true))
            .List();
        ddlAgency.DataSource = lstSubShop;
        ddlAgency.DataTextField = "Name";
        ddlAgency.DataValueField = "Id";
        ddlAgency.DataBind();
    }
    private void UpdateData(ref ISession sess) { }
    private string ValidCurrency(string InvalidString)
    {
        InvalidString = InvalidString.Replace(",", "");
        InvalidString = InvalidString.Replace(".", "");
        return InvalidString;
    }
    private void InsertNewData(ref ISession sess)
    {
        string PriceTax = ValidCurrency(txtPriceTax.Text.Trim());
        ITransaction tx = sess.BeginTransaction();
        try
        {
            Batchinvoiceheader bih = new Batchinvoiceheader();
            bih.Dealercode = UserHelper.DealerCode;
            bih.Branchcode = ddlStoreCode.SelectedValue;
            bih.Createdby = UserHelper.Username;
            bih.Createddate = DateTime.Now;
            IList lstbih = sess.CreateCriteria(typeof(Batchinvoiceheader)).Add(Expression.Eq("Batchinvoicenumber", txtBillNo.Text.Trim())).List();
            if (lstbih.Count > 0)
            {
                ShowMessage(Message.Invoice_BillDuplicated, true);
                return;
            }
            else
                bih.Batchinvoicenumber = txtBillNo.Text.Trim();

            //Create sellitem
            Sellitem sellIns = new Sellitem();

            if (!PriceTax.Equals("")) sellIns.Pricebeforetax = decimal.Parse(PriceTax);
            DateTime dtRecDate;
            DateTime.TryParse(txtRecCDate.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out dtRecDate);
            sellIns.Paymentdate = dtRecDate;
            sellIns.Selltype = txtSellingType.Text;

            sellIns.Paymenttype = int.Parse(ddlPaymentMethod.SelectedValue);
            sess.Save(sellIns);
            bih.Sellitem = sellIns;

            #region Save hire-purchase
            if (ddlPaymentMethod.SelectedValue.Equals(((int)CusPaymentType.UnFixedHire_purchase).ToString()))
            {
                TextBox txtIntentDatePay, UHPtxtMoney;
                Payment pmIns;
                for (int i = 1; i < 6; i++)
                {
                    try
                    {
                        txtIntentDatePay = (TextBox)UnfixedHP.FindControl("txtIntentDatePay" + i);
                        UHPtxtMoney = (TextBox)UnfixedHP.FindControl("UHPtxtMoney" + i);
                        pmIns = new Payment();
                        if (txtIntentDatePay.Text.Trim() != "" && UHPtxtMoney.Text.Trim() != "")
                        {
                            pmIns.Sellitem = sellIns;
                            pmIns.Paymentdate = DateTime.Parse(txtIntentDatePay.Text.Trim());
                            pmIns.Amount = decimal.Parse(ValidCurrency(UHPtxtMoney.Text.Trim()));
                            pmIns.Status = 0;
                            sess.Save(pmIns);
                        }
                    }
                    catch (Exception) { }
                }
            }
            if (ddlPaymentMethod.SelectedValue.Equals(((int)CusPaymentType.FixedHire_purchase).ToString()))
            {
                BindAndSaveFHPPayment(ref sess, sellIns);
            }
            #endregion

            //Create subshop
            if (ddlAgency.SelectedValue.Trim().Equals(""))
            {
                ShowMessage(Resources.Message.Selling_SubStoreNotFound, true);
                return;
            }
            else
            {
                Subshop ss = sess.CreateCriteria(typeof(Subshop)).Add(Expression.Eq("Id", long.Parse(ddlAgency.SelectedValue))).List()[0] as Subshop;
                bih.Subshop = ss;
                sess.SaveOrUpdate(bih);
            }

            string strItemSelected = liSelectItemIns.Text;
            string[] lstItemSelected = strItemSelected.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lstItemSelected.Length; i++)
            {   // selling date (nmChi added: 29/07/2008)
                DateTime dtSellDate;
                if (!DateTime.TryParse(txtSellingDate.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out dtSellDate))
                {
                    dtSellDate = DateTime.Now;
                }

                Iteminstance itemIns = sess.CreateCriteria(typeof(Iteminstance)).Add(Expression.Eq("Id", long.Parse(lstItemSelected[i]))).List()[0] as Iteminstance;
                itemIns.Status = (int)ItemStatus.Sold;   // Now drop inventory - change request 21/7/2010
                //itemIns.Status = (int)ItemStatus.Imported;
                itemIns.Releaseddate = dtSellDate;
                sess.SaveOrUpdate(itemIns);

                // save to Inventory of Day
                //InventoryHelper
                //    .SaveInventoryDay(itemIns.Item.Id, DateTime.Now, -1, (int)ItemStatus.Sold, UserHelper.DealerCode, UserHelper.BranchCode);

                //sess.Save((new ItemHepler()).SaveTranHis(itemIns, DateTime.Now, ItemStatus.Sold, int.Parse(ddlPaymentMethod.SelectedValue), long.Parse(PriceTax), UserHelper.CurrentUsername, null));

                sess.SaveOrUpdate(new Batchinvoicedetail(itemIns.Enginenumber, DateTime.Now, UserHelper.Username, itemIns, bih));
            }
            tx.Commit();
            PageRefresh();
            gvItems.Visible = false; btnSave.Enabled = false;
            ShowMessage(Resources.Message.ActionSucessful, false);
        }
        catch (HibernateException)
        {
            tx.Rollback();
            throw;
        }
    }
    private void BindAndSaveFHPPayment(ref ISession sess, Sellitem sellitemIns)
    {
        decimal AllMoney = decimal.Parse(ValidCurrency(txtPriceTax.Text));
        decimal RestOfMoneyFHP = AllMoney - decimal.Parse(ValidCurrency(txtFHPFirstMoney.Text));
        decimal MoneyOfInstalment = RestOfMoneyFHP / (decimal.Parse(txtFHPTimes.Text) - 1);
        int DateCount = int.Parse(txtFHPPaidMoneyDate.Text.Trim());
        DateTime firstDate; DateTime lastDate = new DateTime();
        DateTime.TryParse(txtRecCDate.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out firstDate);

        Payment paymentIns;
        for (int i = 0; i <= (decimal.Parse(txtFHPTimes.Text) - 1); i++)
        {
            paymentIns = new Payment();
            //Save the first instalment
            if (i == 0)
            {
                paymentIns.Amount = decimal.Parse(ValidCurrency(txtFHPFirstMoney.Text));
                paymentIns.Paymentdate = firstDate;
                paymentIns.Sellitem = sellitemIns;
            }
            else
            {
                lastDate = firstDate.AddDays(DateCount * i);
                paymentIns.Amount = MoneyOfInstalment;
                paymentIns.Paymentdate = lastDate;
                paymentIns.Sellitem = sellitemIns;
            }
            paymentIns.Status = 0;
            sess.SaveOrUpdate(paymentIns);
        }
    }
    private void BindEvalFHP()
    {
        txtFHPAllMoney.Text = Convert2Currency(ValidCurrency(txtPriceTax.Text));
        decimal AllMoney = decimal.Parse(ValidCurrency(txtPriceTax.Text));
        decimal RestOfMoneyFHP = AllMoney - decimal.Parse(txtFHPFirstMoney.Text);
        decimal MoneyOfInstalment = RestOfMoneyFHP / (decimal.Parse(ValidCurrency(txtFHPTimes.Text)) - 1);
        lbMoneyOfTimes.Text = Convert2Currency(MoneyOfInstalment.ToString());
        DateTime firstDate; DateTime lastDate = new DateTime();
        DateTime.TryParse(txtRecCDate.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out firstDate);
        for (int i = 1; i <= (decimal.Parse(txtFHPTimes.Text) - 1); i++)
        {
            lastDate = firstDate.AddDays(15 * i);
        }
        FHPPaidDateLast.Text = lastDate.ToShortDateString();
    }
    private void LoadOldGrid(int OldPageIndex)
    {
        try
        {
            IList lstItemIns = (IList)Session["GrdSlotItemInstance"];
            gvItems.PageIndex = OldPageIndex;
            gvItems.DataSource = lstItemIns;
            gvItems.DataBind();
        }
        catch (Exception)
        {
            gvItems.Visible = false;
        }
    }
    private void LoadOldGrid()
    {
        try
        {
            IList lstItemIns = (IList)Session["GrdSlotItemInstance"];
            HttpContext.Current.Items["PaymentCount"] = lstItemIns.Count;
            gvItems.DataSource = lstItemIns;
            gvItems.DataBind();
        }
        catch (Exception)
        {
            gvItems.Visible = false;
        }
    }
    private bool GetItemSelected()
    {
        bool AtleastOneItemSeleted = false;
        string strItemSelected = liSelectItemIns.Text;
        string strNewItemSelected = "";
        string[] lstItemSelected = strItemSelected.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        int a = lstItemSelected.Length;

        CheckBox cb; Label lb; bool isHaved = false;
        foreach (GridViewRow gvr in gvItems.Rows)
        {
            cb = (CheckBox)gvr.FindControl("cbSelectItem");
            lb = (Label)gvr.FindControl("lbItemInstanceID");
            if (cb != null)
            {
                //Check DropID
                if (!cb.Checked)
                {
                    for (int i = 0; i < lstItemSelected.Length; i++)
                    {
                        if (lstItemSelected[i].Equals(lb.Text))
                        {
                            lstItemSelected[i] = "REMOVE";
                        }
                    }
                }
                //Check new ID, haved ID
                else
                {
                    for (int i = 0; i < lstItemSelected.Length; i++)
                    {
                        if (lstItemSelected[i].Equals(lb.Text))
                        {
                            isHaved = true;
                        }
                    }
                    if (!isHaved) strNewItemSelected += lb.Text + ",";
                }
            }
        }
        //Join String
        for (int i = 0; i < lstItemSelected.Length; i++)
        {
            if (!lstItemSelected[i].Equals("REMOVE"))
            {
                strNewItemSelected += lstItemSelected[i] + ",";
            }
        }
        if (strNewItemSelected.Replace(",", "").Trim().Length > 0)
        {
            AtleastOneItemSeleted = true;
        }
        liSelectItemIns.Text = strNewItemSelected;
        //liSelectItemIns.Visible = true;
        return AtleastOneItemSeleted;
    }
    private void CloseSessionForHPUpdate(int HPstatus)
    {
        TextBox txtIntentDatePay, UHPtxtMoney;
        CalendarExtender CEUHPCandelar;
        switch (HPstatus)
        {
            case (int)CusPaymentType.UnFixedHire_purchase:
                {
                    ddlPaymentMethod.Enabled = false;
                    txtPriceTax.ReadOnly = true;
                    for (int i = 1; i < 6; i++)
                    {
                        try
                        {
                            txtIntentDatePay = (TextBox)UnfixedHP.FindControl("txtIntentDatePay" + i);
                            UHPtxtMoney = (TextBox)UnfixedHP.FindControl("UHPtxtMoney" + i);
                            CEUHPCandelar = (CalendarExtender)UnfixedHP.FindControl("CEUHP" + i);
                            txtIntentDatePay.ReadOnly = true; txtIntentDatePay.CssClass = "readOnlyInputField";
                            UHPtxtMoney.ReadOnly = true; UHPtxtMoney.CssClass = "readOnlyInputField";
                            CEUHPCandelar.Enabled = false;
                        }
                        catch (Exception) { throw new Exception("Error"); }
                    }
                    break;
                }
            case (int)CusPaymentType.FixedHire_purchase:
                {
                    ddlPaymentMethod.Enabled = false;
                    txtPriceTax.ReadOnly = true;
                    txtFHPTimes.ReadOnly = true; txtFHPTimes.CssClass = "readOnlyInputField";
                    txtFHPPaidMoneyDate.ReadOnly = true; txtFHPPaidMoneyDate.CssClass = "readOnlyInputField";
                    txtFHPFirstMoney.ReadOnly = true; txtFHPFirstMoney.CssClass = "readOnlyInputField";
                    break;
                }
            default: break;
        }
    }
    private string GetTax()
    {
        return "10";
    }
    private void ShowMessage(string ErrMsg, bool isError)
    {
        lbErr.Visible = true;
        lbErr.ControlStyle.ForeColor = (isError) ? Color.Red : Color.RoyalBlue;
        lbErr.Text = ErrMsg;
    }
    protected string ReturnIndex(String RowIndexInline)
    {
        return ((gvItems.PageSize * gvItems.PageIndex) + (int.Parse(RowIndexInline) + 1)).ToString();
    }
    protected string GetLanguage()
    {
        return UserHelper.Language;
    }
    protected string ChangeDatetimeByLang(string oDate)
    {
        if (oDate.Equals("")) return "";
        DateTime rDate;
        DateTime.TryParse(oDate, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out rDate);
        return rDate.ToShortDateString();
    }

    private bool ValidDateFormat(string oDate)
    {
        DateTime returnDate;
        DateTime.TryParse(oDate, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out returnDate);
        return (returnDate.Equals(DateTime.MinValue)) ? false : true;
    }

    private RangeValidator DateValid(RangeValidator rvDate)
    {
        rvDate.MinimumValue = DateTime.MinValue.ToShortDateString();
        rvDate.MaximumValue = DateTime.MaxValue.ToShortDateString();
        return rvDate;
    }
    private void PageRefresh()
    {
        txtSellingDate.Text = DateTime.Now.ToShortDateString();
        ddlAgency.SelectedIndex = 0;
        txtBillNo.Text = "";
        txtSellingType.Text = "";
        txtPriceTax.Text = ""; txtPriceTax.ReadOnly = false; txtPriceTax.CssClass = null;
        ddlPaymentMethod.Enabled = true; ddlPaymentMethod.SelectedIndex = 0;
        txtRecCDate.Text = "";
        FixedHP.Visible = false; UnfixedHP.Visible = false;
        txtFHPTimes.ReadOnly = false; txtFHPTimes.CssClass = null; txtFHPTimes.Text = "";
        txtFHPFirstMoney.ReadOnly = false; txtFHPFirstMoney.CssClass = null; txtFHPFirstMoney.Text = "";
        txtFHPPaidMoneyDate.ReadOnly = false; txtFHPPaidMoneyDate.CssClass = null; txtFHPPaidMoneyDate.Text = "";
    }
    private bool checkDateRange(string d1, string d2)
    {
        DateTime dt1, dt2;
        DateTime.TryParse(d1, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out dt1);
        DateTime.TryParse(d2, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out dt2);
        return (dt1 > dt2) ? false : true;
    }
}
