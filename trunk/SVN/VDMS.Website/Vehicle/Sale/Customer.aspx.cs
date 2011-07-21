using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using VDMS.Helper;
using NHibernate;
using NHibernate.Expression;
using Resources;
using VDMS.I.Entity;
using VDMS.I.Vehicle;
using VDMS.Common.Web;
using VDMS.Core.Domain;
using VDMS.Data.DAL;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.I.Service;
using VDMS.Common.Utils;
using VDMS;
using Customer = VDMS.Core.Domain.Customer;
using Invoice = VDMS.Core.Domain.Invoice;

public partial class Sales_Sale_Customer : BasePage
{
    private const int vwMainIndex = 0;
    private const int vwSelectItemIndex = 1;
    private static readonly string _DEFAULTPAGE = "DEFAULTPAGE";

    private static readonly string _INSERTNEWDATA = "INSERTNEWDATA";
    private static readonly string _UPDATEDATA = "UPDATEDATA";
    private bool isCustomerEditted = false;
    private static bool ShowPriceBox = true;

    protected string LoadCustomerErr = Message.LoadCustomerErr;
    protected string LoadEngineNoErr = Message.EngineNumberNotFound;
    protected string SumMoneyInvalid = Message.Cus_SumMoneyInvalid;

    public string EnteredEngineNumber
    {
        get { return txtEngineNo.Text.Trim().ToUpper(); }
    }
    public string CurrentDealerCode
    {
        get { return UserHelper.DealerCode; }
    }

    public string PageKey;
    public void NewPageKey()
    {
        PageKey = Guid.NewGuid().ToString();
        SaveState("PageKey", PageKey);
    }
    public void LoadPageKey()
    {
        PageKey = (string)LoadState("PageKey");
    }

    void InitLayout()
    {
        if (!IsPostBack)
        {
            LiPriceinCludeTaxUnit.Visible = LiPriceinCludeTax.Visible = txtPriceTax.Visible = ShowPriceBox;
            txtPriceTax.Text = "0";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        InitLayout();

        rvRecDate = DateValid(rvRecDate);
        rvSellingDate = DateValid(rvSellingDate, DateTime.Now);
        rvTakingPlateDate = DateValid(rvTakingPlateDate);
        rvUHP1 = DateValid(rvUHP1);
        rvUHP2 = DateValid(rvUHP2);
        rvUHP3 = DateValid(rvUHP3);
        rvUHP4 = DateValid(rvUHP4);
        rvUHP5 = DateValid(rvUHP5);

        if (!Page.IsPostBack)
        {
            _PageStatus.Value = _DEFAULTPAGE;
            NewPageKey();
            _lnkFindCust.NavigateUrl = string.Format("../../Service/Popup/SelectCustomer.aspx?key={0}&ct=SL&dc={1}&TB_iframe=true", PageKey, UserHelper.DealerCode);
        }
        else
        {
            LoadPageKey();
        }

        lbMes.Visible = false;
        LoadCustomerErr = Message.LoadCustomerErr;
        SumMoneyInvalid = Message.Cus_SumMoneyInvalid;
    }

    private bool InsertNewLoad(ref ISession sess)
    {
        //bool _returnFLG = false; 
        ICriteria crit = null;
        string txtEngineNoStr = txtEngineNo.Text.Trim().ToUpper();
        ICriteria critInvOfSubShop = sess.CreateCriteria(typeof(Batchinvoicedetail))
            .Add(Expression.InsensitiveLike("Enginenumber", txtEngineNoStr));
        if (UserHelper.IsDealer)
        {
            critInvOfSubShop.CreateCriteria("Batchinvoiceheader")
                .Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
                .Add(Expression.Eq("Branchcode", UserHelper.BranchCode));
        }

        IList lstInvoiceOfSubShop = critInvOfSubShop.List();
        if (lstInvoiceOfSubShop.Count > 0)
        {
            IList lstInvoice = sess.CreateCriteria(typeof(Invoice))
                .Add(Expression.InsensitiveLike("Enginenumber", txtEngineNoStr))
                .List();
            if (lstInvoice.Count > 0)
            {
                ShowMessage(Message.EngineNumberNotFoundInDB, true);
                return false;
            }
            else
            {
                Batchinvoicedetail binvd = lstInvoiceOfSubShop[0] as Batchinvoicedetail;
                txtSubshop.Text = binvd.Batchinvoiceheader.Subshop.Name;
                cbSubStore.Checked = true;
                crit = sess.CreateCriteria(typeof(Iteminstance))
                    .Add(Expression.InsensitiveLike("Enginenumber", txtEngineNoStr))
                    .Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
                    .Add(Expression.Eq("Branchcode", UserHelper.BranchCode))
                    .Add(Expression.InsensitiveLike("Enginenumber", txtEngineNoStr))
                    .Add(Expression.In("Status", ItemHepler.GetInstockItemStatus()));
            }
        }
        else
        {
            crit = sess.CreateCriteria(typeof(Iteminstance))
                .Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
                .Add(Expression.Eq("Branchcode", UserHelper.BranchCode))
                .Add(Expression.InsensitiveLike("Enginenumber", txtEngineNoStr))
                .Add(Expression.In("Status", ItemHepler.GetInstockItemStatus()));
        }

        IList lstItemIns = crit.List();
        if (lstItemIns.Count > 0)
        {
            Iteminstance itemIns = lstItemIns[0] as Iteminstance;
            //Set IteminstanceID for Insert or Update;
            _ItemInstanceID.Value = itemIns.Id.ToString();
            liDealerCode.Text = itemIns.Dealercode;
            liMotorType.Text = itemIns.Itemtype;
            liColor.Text = itemIns.Color;
            liTax.Text = GetTax() + "%";
            txtWarehouse.Text = itemIns.Branchcode;
            GetCustomerID.Enabled = UserHelper.BranchCode == itemIns.Branchcode;
            return GetCustomerID.Enabled;
        }
        else
        {
            txtWarehouse.Text = "";
            liDealerCode.Text = "";
            liMotorType.Text = "";
            liColor.Text = "";
            liTax.Text = "";
            txtEngineNo.Text = "";
            GetCustomerID.Enabled = false;
            ShowMessage(Message.EngineNumberNotFoundInDB, true);
            return false;
        }
    }

    private bool TestLoad(ref ISession sess, bool isSelectChange)
    {
        string txtEngineNoStr = txtEngineNo.Text.Trim().ToUpper();
        //Load by subshop
        IList lstInvoiceOfSubShop = sess.CreateCriteria(typeof(Batchinvoicedetail))
            .Add(Expression.InsensitiveLike("Enginenumber", txtEngineNoStr))
            .List();
        if (lstInvoiceOfSubShop.Count > 0)
        {
            Batchinvoicedetail binvd = lstInvoiceOfSubShop[0] as Batchinvoicedetail;
            txtSubshop.Text = binvd.Batchinvoiceheader.Subshop.Name;
            cbSubStore.Checked = true;
        }

        ICriteria icrit = sess.CreateCriteria(typeof(Invoice))
            .Add(Expression.InsensitiveLike("Enginenumber", txtEngineNoStr));
        if (UserHelper.IsDealer)
        {
            icrit.Add(Expression.Eq("Dealercode", UserHelper.DealerCode));
            icrit.Add(Expression.Eq("Branchcode", UserHelper.BranchCode));
        }

        IList lstInvoiceIns = icrit.List();
        if (lstInvoiceIns.Count > 0)
        {
            Invoice InvoiceIns = (Invoice)lstInvoiceIns[0];

            #region Get ItemInstance Inf

            Iteminstance itemIns = InvoiceIns.Iteminstance;

            //Set IteminstanceID for Insert or Update;
            _ItemInstanceID.Value = itemIns.Id.ToString();

            //liDealerCode.Text = DealerHelper.GetName(itemIns.Dealercode);
            liDealerCode.Text = itemIns.Dealercode;
            liMotorType.Text = itemIns.Itemtype;
            liColor.Text = itemIns.Item.Colorname;
            liTax.Text = GetTax() + "%";

            #endregion

            txtSellingDate.Text = InvoiceIns.Selldate.ToShortDateString();
            txtBillNo.Text = InvoiceIns.Invoicenumber;
            //Payment Method ?
            if (!isSelectChange)
            {
                ddlPaymentMethod.SelectedValue = InvoiceIns.Sellitem.Paymenttype.ToString();
            }
            if (int.Parse(ddlPaymentMethod.SelectedValue) == (int)CusPaymentType.FixedHire_purchase)
            {
                try
                {
                    if (InvoiceIns.Sellitem.Paymenttype.Equals((int)CusPaymentType.FixedHire_purchase))
                    {
                        IList lstPayment = sess.CreateCriteria(typeof(Payment))
                            .Add(Expression.Eq("Sellitem", InvoiceIns.Sellitem))
                            //.Add(Expression.Eq("Status", (int)CusPaymentType.FixedHire_purchase))
                            .AddOrder(NHibernate.Expression.Order.Asc("Paymentdate"))
                            .List();
                        Payment fstPayment = lstPayment[0] as Payment;
                        Payment secPayment = lstPayment[1] as Payment;
                        Payment lastPayment = lstPayment[lstPayment.Count - 1] as Payment;
                        txtFHPFirstMoney.Text = (_PageStatus.Value.Equals(_DEFAULTPAGE))
                                                    ? Convert2Currency(fstPayment.Amount.ToString())
                                                    : fstPayment.Amount.ToString();
                        txtFHPTimes.Text = lstPayment.Count.ToString();
                        TimeSpan ts = secPayment.Paymentdate - fstPayment.Paymentdate;
                        txtFHPPaidMoneyDate.Text = ts.TotalDays.ToString();

                        txtFHPAllMoney.Text = (_PageStatus.Value.Equals(_DEFAULTPAGE))
                                                  ? Convert2Currency(InvoiceIns.Sellitem.Pricebeforetax.ToString())
                                                  : InvoiceIns.Sellitem.Pricebeforetax.ToString();
                        lbMoneyOfTimes.Text = (_PageStatus.Value.Equals(_DEFAULTPAGE))
                                                  ? Convert2Currency(lastPayment.Amount.ToString())
                                                  : lastPayment.Amount.ToString();
                        FHPPaidDateLast.Text = lastPayment.Paymentdate.ToShortDateString();
                    }
                    if (!_PageStatus.Value.Equals(_DEFAULTPAGE))
                    {
                        btnCheckValid.Visible = true;
                        btnSave.Visible = false;
                    }
                    if (_PageStatus.Value.Equals(_DEFAULTPAGE))
                    {
                        txtFHPTimes.ReadOnly = true;
                        txtFHPTimes.CssClass = "readOnlyInputField";
                        txtFHPFirstMoney.ReadOnly = true;
                        txtFHPFirstMoney.CssClass = "readOnlyInputField";
                        txtFHPPaidMoneyDate.ReadOnly = true;
                        txtFHPPaidMoneyDate.CssClass = "readOnlyInputField";
                    }
                    FixedHP.Visible = true;
                    UnfixedHP.Visible = false;
                }
                catch (Exception)
                {
                    txtFHPAllMoney.Text = (_PageStatus.Value.Equals(_DEFAULTPAGE))
                                              ? Convert2Currency(InvoiceIns.Sellitem.Pricebeforetax.ToString())
                                              : InvoiceIns.Sellitem.Pricebeforetax.ToString();
                    if (_PageStatus.Value.Equals(_UPDATEDATA))
                    {
                        btnCheckValid.Visible = true;
                        btnSave.Visible = false;
                    }
                }
            }
            else if (int.Parse(ddlPaymentMethod.SelectedValue) == (int)CusPaymentType.UnFixedHire_purchase)
            {
                BindUFHP(ref sess, InvoiceIns.Sellitem);
                if (_PageStatus.Value.Equals(_UPDATEDATA))
                {
                    btnCheckValid.Visible = true;
                    btnSave.Visible = false;
                }
                else
                {
                    CloseSessionForHPUpdate((int)CusPaymentType.UnFixedHire_purchase);
                }
                FixedHP.Visible = false;
                UnfixedHP.Visible = true;
            }
            else
            {
                FixedHP.Visible = false;
                UnfixedHP.Visible = false;
            }
            //Bind to FixHP

            txtPlateNo.Text = InvoiceIns.Sellitem.Numberplate;
            txtPriceTax.Text = (_PageStatus.Value.Equals(_DEFAULTPAGE))
                                   ? Convert2Currency(InvoiceIns.Sellitem.Pricebeforetax.ToString())
                                   : InvoiceIns.Sellitem.Pricebeforetax.ToString();
            txtSellingType.Text = InvoiceIns.Sellitem.Selltype;
            txtRecCDate.Text = InvoiceIns.Sellitem.Paymentdate.ToShortDateString();
            if (!InvoiceIns.Sellitem.Numplaterecdate.Equals(DateTime.MinValue))
            {
                txtTakPlateNoDate.Text = InvoiceIns.Sellitem.Numplaterecdate.ToShortDateString();
            }

            txtComment.Text = InvoiceIns.Sellitem.Commentsellitem;

            //Customer
            lbCustomerFullName.Text = InvoiceIns.Customer.Fullname;
            _CustomerID.Value = InvoiceIns.Customer.Id.ToString();
            txtCustomerIdentifyNumber.Text = InvoiceIns.Customer.Identifynumber;
            //Disable control for test
            //tx.Commit();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateLoad(ref ISession sess)
    {
    }

    private void BindUFHP(ref ISession sess, Sellitem SellItemIns)
    {
        if (SellItemIns.Paymenttype.Equals((int)CusPaymentType.UnFixedHire_purchase))
        {
            TextBox txtIntentDatePay, UHPtxtMoney;
            CalendarExtender CEUHPCandelar;
            //GetSellItem
            Payment pm;
            IList lstPayment = sess.CreateCriteria(typeof(Payment))
                .Add(Expression.Eq("Sellitem", SellItemIns))
                //.Add(Expression.Eq("Status", (int)CusPaymentType.UnFixedHire_purchase))
                .List();
            for (int i = 1; i <= lstPayment.Count; i++)
            {
                try
                {
                    pm = (Payment)lstPayment[i - 1];
                    txtIntentDatePay = (TextBox)UnfixedHP.FindControl("txtIntentDatePay" + i);
                    UHPtxtMoney = (TextBox)UnfixedHP.FindControl("UHPtxtMoney" + i);
                    CEUHPCandelar = (CalendarExtender)UnfixedHP.FindControl("CEUHP" + i);
                    txtIntentDatePay.Text = pm.Paymentdate.ToShortDateString();
                    UHPtxtMoney.Text = (_PageStatus.Value.Equals(_DEFAULTPAGE))
                                           ? Convert2Currency(pm.Amount.ToString())
                                           : pm.Amount.ToString();
                }
                catch (Exception)
                {
                    throw new Exception("Error");
                }
            }
        }
    }

    private void DisEnControlForTest(bool flagDisEn)
    {
        if (flagDisEn)
        {
            txtCustomerIdentifyNumber.ReadOnly = true;
            txtCustomerIdentifyNumber.CssClass = "readOnlyInputField";
            txtBillNo.ReadOnly = true;
            txtBillNo.CssClass = "readOnlyInputField";
            txtPriceTax.ReadOnly = true;
            txtPriceTax.CssClass = "readOnlyInputField";
            txtPlateNo.ReadOnly = true;
            txtPlateNo.CssClass = "readOnlyInputField";
            txtSellingType.ReadOnly = true;
            txtSellingType.CssClass = "readOnlyInputField";
            txtRecCDate.ReadOnly = true;
            txtRecCDate.CssClass = "readOnlyInputField";
            txtTakPlateNoDate.ReadOnly = true;
            txtTakPlateNoDate.CssClass = "readOnlyInputField";
            txtComment.ReadOnly = true;
            txtComment.CssClass = "readOnlyInputField";
            ddlPaymentMethod.Enabled = false;
            CalendarExtender2.Enabled = false;
            CalendarExtender6.Enabled = false;
            GetCustomerID.Enabled = false;
        }
        else
        {
            lbCustomerFullName.Text = "";
            //txtEngineNo.Text = "";
            txtCustomerIdentifyNumber.Text = "";
            txtCustomerIdentifyNumber.ReadOnly = false;
            txtCustomerIdentifyNumber.CssClass = null;
            txtSellingDate.Text = "";
            liColor.Text = "";
            liDealerCode.Text = "";
            liMotorType.Text = "";
            liTax.Text = "";
            txtBillNo.ReadOnly = false;
            txtBillNo.Text = "";
            txtBillNo.CssClass = null;
            txtPriceTax.ReadOnly = false;
            txtPriceTax.Text = "";
            txtPriceTax.CssClass = null;
            txtPlateNo.ReadOnly = false;
            txtPlateNo.Text = "";
            txtPlateNo.CssClass = null;
            txtSellingType.ReadOnly = false;
            txtSellingType.Text = "";
            txtSellingType.CssClass = null;
            txtRecCDate.ReadOnly = false;
            txtRecCDate.Text = "";
            txtRecCDate.CssClass = null;
            txtTakPlateNoDate.ReadOnly = false;
            txtTakPlateNoDate.Text = "";
            txtTakPlateNoDate.CssClass = null;
            txtComment.ReadOnly = false;
            txtComment.Text = "";
            txtComment.CssClass = null;
            ddlPaymentMethod.Enabled = true;
            ddlPaymentMethod.SelectedValue = "0";
            CalendarExtender2.Enabled = true;
            CalendarExtender6.Enabled = true;
            txtFHPTimes.ReadOnly = false;
            txtFHPTimes.CssClass = null;
            txtFHPTimes.Text = "";
            txtFHPFirstMoney.ReadOnly = false;
            txtFHPFirstMoney.CssClass = null;
            txtFHPFirstMoney.Text = "";
            txtFHPPaidMoneyDate.ReadOnly = false;
            txtFHPPaidMoneyDate.CssClass = null;
            txtFHPPaidMoneyDate.Text = "";
            FHPPaidDateLast.Text = "";
            lbMoneyOfTimes.Text = "";
        }
    }

    protected void ddlPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtPriceTax.Text = txtPriceTax.Text.Trim().Replace(",", "");
        txtPriceTax.Text = txtPriceTax.Text.Trim().Replace(".", "");
        if (int.Parse(ddlPaymentMethod.SelectedValue) == (int)CusPaymentType.FixedHire_purchase)
        {
            if (txtFHPAllMoney.Text.Trim() != "")
            {
                txtFHPAllMoney.Text = txtPriceTax.Text.Trim();
            }
            btnCheckValid.Visible = true;
            btnSave.Visible = false;
            FixedHP.Visible = true;
            UnfixedHP.Visible = false;
        }
        else if (int.Parse(ddlPaymentMethod.SelectedValue) == (int)CusPaymentType.UnFixedHire_purchase)
        {
            btnCheckValid.Visible = true;
            btnSave.Visible = false;
            FixedHP.Visible = false;
            UnfixedHP.Visible = true;
            if (_PageStatus.Value.Equals(_INSERTNEWDATA))
            {
                TextBox txtIntentDatePay, UHPtxtMoney;
                CalendarExtender CEUHPCandelar;
                for (int i = 1; i < 6; i++)
                {
                    try
                    {
                        txtIntentDatePay = (TextBox)UnfixedHP.FindControl("txtIntentDatePay" + i);
                        UHPtxtMoney = (TextBox)UnfixedHP.FindControl("UHPtxtMoney" + i);
                        CEUHPCandelar = (CalendarExtender)UnfixedHP.FindControl("CEUHP" + i);
                        txtIntentDatePay.ReadOnly = false;
                        txtIntentDatePay.CssClass = null;
                        txtIntentDatePay.Text = "";
                        UHPtxtMoney.ReadOnly = false;
                        UHPtxtMoney.CssClass = null;
                        UHPtxtMoney.Text = "";
                        CEUHPCandelar.Enabled = true;
                    }
                    catch (Exception)
                    {
                        throw new Exception("Error");
                    }
                }
            }
        }
        else
        {
            btnCheckValid.Visible = false;
            btnSave.Visible = true;
            FixedHP.Visible = false;
            UnfixedHP.Visible = false;
        }
        //ISession sess = NHibernateSessionManager.Instance.GetSession();// NHibernateHelper.GetCurrentSession();
        //NHibernateSessionManager.Instance.CloseSession(); //NHibernateHelper.CloseSession();
    }

    protected void SetReadonly(bool readOnly, object ctrl)
    {
        if (ctrl is TextBox)
        {
            ((TextBox)ctrl).ReadOnly = readOnly;
            ((TextBox)ctrl).CssClass = readOnly ? "readOnlyInputField" : "";
        }
        if (ctrl is ImageButton)
        {
            ((ImageButton)ctrl).Visible = !readOnly;
        }
    }

    protected void btnTest_Click(object sender, EventArgs e)
    {
        //ItemInstance itemIns;
        //IDao<ItemInstance, long> itemInsDAO = DaoFactory.GetDao<ItemInstance, long>();
        //itemInsDAO.SetCriteria(new ICriterion[] {Expression.Eq("Enginenumber", txtEngineNo.Text.Trim()), Expression.In("Status", new object[] {1,2,3}) });
        //ISession sess = VDMS.Data.DAL.NHibernateHelper.GetCurrentSession();
        ISession sess = NHibernateSessionManager.Instance.GetSession();
        if (_PageStatus.Value == _INSERTNEWDATA)
        {
            if (InventoryHelper.IsInventoryLock(DateTime.Now, UserHelper.DealerCode, UserHelper.BranchCode))
            {
                ShowMessage(Reports.InventoryIsLocked, true);
                return;
            }
            else InsertNewLoad(ref sess);
        }
        else if (_PageStatus.Value == _UPDATEDATA)
        {
            UpdateLoad(ref sess);
        }
        else
        {
            if (!TestLoad(ref sess, false))
            {
                ShowMessage(Message.EngineNumberNotFound, true);
                return;
            }
            plMainInput.Visible = true;
            DisEnControlForTest(true);
        }
        //NHibernateSessionManager.Instance.CloseSession();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        _PageStatus.Value = _INSERTNEWDATA;
        DisEnControlForTest(false);
        btnEdit.Visible = false;
        btnAdd.Visible = false;
        btnCheckValid.Visible = true;
        btnSave.Visible = false;

        plMainInput.Visible = true;
        plActionAccessData.Visible = true;
        FixedHP.Visible = false;
        UnfixedHP.Visible = false;

        txtSellingDate.Text = DateTime.Now.ToShortDateString();
        bool disControl = VDMSSetting.CurrentSetting.AllowAutoCloseInvI;//InventoryHelper.IsInventoryLock(DateTime.Now, UserHelper.DealerCode, UserHelper.BranchCode);
        SetReadonly(disControl, txtSellingDate);
        SetReadonly(disControl, imgbSellDate);
        txtRecCDate.Text = DateTime.Now.ToShortDateString();

        string js = "if(retrieve_lookup_data('ctl00_c_txtCustomerIdentifyNumber'," +
                    "'CusInfInput.aspx?" +
                    "')==false) return false;";
        //+ "return false;";
        GetCustomerID.OnClientClick = js;
        GetCustomerID.UseSubmitBehavior = false;
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        _PageStatus.Value = _UPDATEDATA;

        ISession sess = NHibernateSessionManager.Instance.GetSession();
        if (!TestLoad(ref sess, false))
        {
            ShowMessage(Message.EngineNumberNotFound, true);
            return;
        }
        //NHibernateSessionManager.Instance.CloseSession();

        //Prepare control to edit
        txtBillNo.ReadOnly = false;
        txtBillNo.CssClass = ""; //txtBillNo.CssClass = "readOnlyInputField";
        txtPriceTax.Enabled = true;
        txtPriceTax.ReadOnly = true;
        txtPriceTax.CssClass = "readOnlyInputField";
        txtPlateNo.Enabled = true;
        txtPlateNo.ReadOnly = false;
        txtPlateNo.CssClass = null;
        txtSellingType.ReadOnly = false;
        txtSellingType.CssClass = null;
        txtRecCDate.ReadOnly = false;
        txtRecCDate.CssClass = ""; //txtRecCDate.CssClass = "readOnlyInputField";
        txtTakPlateNoDate.ReadOnly = false;
        txtTakPlateNoDate.CssClass = null;
        txtComment.ReadOnly = false;
        txtComment.CssClass = null;
        ddlPaymentMethod.Enabled = false;
        CalendarExtender2.Enabled = false;
        CalendarExtender6.Enabled = true;
        txtCustomerIdentifyNumber.ReadOnly = true;
        txtCustomerIdentifyNumber.CssClass = "readOnlyInputField";
        btnEdit.Visible = false;
        btnAdd.Visible = false;
        btnTest.Visible = false;
        txtEngineNo.ReadOnly = true;
        txtFHPTimes.ReadOnly = false;
        txtFHPTimes.CssClass = null;
        txtFHPFirstMoney.ReadOnly = false;
        txtFHPFirstMoney.CssClass = null;
        txtFHPPaidMoneyDate.ReadOnly = false;
        txtFHPPaidMoneyDate.CssClass = null;

        plMainInput.Visible = true;
        plActionAccessData.Visible = true;

        GetCustomerID.Enabled = true;
        string js = "if(retrieve_lookup_data('ctl00_c_txtCustomerIdentifyNumber'," +
                    "'CusInfInput.aspx?" +
                    "')==false) return false;";
        //"return false;";
        GetCustomerID.OnClientClick = js;
    }

    protected bool IsValidSellDate(string engNo, DateTime sellDate)
    {
        bool result = true;
        ISession sess = NHibernateSessionManager.Instance.GetSession();
        Iteminstance item = sess.CreateCriteria(typeof(Iteminstance)).Add(Expression.Eq("Enginenumber", engNo.Trim())).UniqueResult<Iteminstance>();
        if (item != null)
        {
            result = (item.Importeddate <= sellDate) && (sellDate <= DateTime.Now);
        }

        return result;
    }

    protected void btnCheckValid_Click(object sender, EventArgs e)
    {
        DateTime sellDate;
        if ((!DateTime.TryParse(txtSellingDate.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces,
                          out sellDate)) || !IsValidSellDate(txtEngineNo.Text, sellDate))
        {
            ShowMessage(Customers.WrongSellingDate, true);
            return;
        }

        if (InventoryHelper.IsInventoryLock(sellDate, UserHelper.DealerCode, UserHelper.BranchCode))
        {
            ShowMessage(Message.InventoryLocked, true);
            return;
        }

        if (ddlPaymentMethod.SelectedIndex != 0)
        {
            try
            {
                if (!((int.Parse(txtPriceTax.Text.Trim())) > 0))
                {
                    ShowMessage(Customers.PriceEmpty, true);
                    return;
                }
                if (txtRecCDate.Text.Trim().Equals("") && ddlPaymentMethod.SelectedIndex == 1)
                {
                    ShowMessage(Customers.txtDateRecEmpty, true);
                    return;
                }
            }
            catch (Exception)
            {
                ShowMessage(Customers.PriceEmpty, true);
                return;
            }
        }

        if (liDealerCode.Text.Trim().Equals(""))
        {
            ShowMessage(Customers.MotorInformationNull, true);
            return;
        }
        if (!checkDateRange(txtSellingDate.Text.Trim(), txtRecCDate.Text.Trim()) &&
            !txtSellingDate.Text.Trim().Equals(""))
        {
            ShowMessage(Customers.RecDateSmallerThanSelDate, true);
            return;
        }
        if (!ValidDateFormat(txtRecCDate.Text.Trim()))
        {
            ShowMessage(Customers.RecDateInvalid, true);
            return;
        }

        #region UnFixedHire-Purchase

        if (ddlPaymentMethod.SelectedValue.Equals(((int)CusPaymentType.UnFixedHire_purchase).ToString()))
        {
            decimal SumCheck = 0;
            TextBox txtIntentDatePay, UHPtxtMoney, subtxtIntentDatePay;
            DateTime recDate, paidDate, subIntentDate;
            recDate = DateTime.MinValue;
            if (!txtRecCDate.Text.Trim().Equals(""))
                DateTime.TryParse(txtRecCDate.Text.Trim(), new CultureInfo(UserHelper.Language),
                                  DateTimeStyles.AllowWhiteSpaces, out recDate);

            for (int i = 1; i < 6; i++)
            {
                try
                {
                    txtIntentDatePay = (TextBox)UnfixedHP.FindControl("txtIntentDatePay" + i);
                    UHPtxtMoney = (TextBox)UnfixedHP.FindControl("UHPtxtMoney" + i);
                    if (txtIntentDatePay.Text.Trim() != "" && UHPtxtMoney.Text.Trim() != "")
                    {
                        SumCheck += decimal.Parse(UHPtxtMoney.Text.Trim());
                        DateTime.TryParse(txtIntentDatePay.Text.Trim(), new CultureInfo(UserHelper.Language),
                                          DateTimeStyles.AllowWhiteSpaces, out paidDate);
                        if (!paidDate.Equals(DateTime.MinValue) && paidDate < recDate)
                        {
                            ShowMessage(Customers.DateUFHPValid, true);
                            return;
                        }
                        for (int k = 1; k < 6; k++)
                        {
                            if (k == i) break;
                            else
                            {
                                subtxtIntentDatePay = (TextBox)UnfixedHP.FindControl("txtIntentDatePay" + k);
                                DateTime.TryParse(subtxtIntentDatePay.Text.Trim(), new CultureInfo(UserHelper.Language),
                                                  DateTimeStyles.AllowWhiteSpaces, out subIntentDate);
                                if (paidDate.Equals(subIntentDate))
                                {
                                    ShowMessage(Customers.DateUFHPValid, true);
                                    return;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Error");
                }
            }
            if (!SumCheck.Equals(decimal.Parse(txtPriceTax.Text.Trim())))
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
                ShowMessage(Customers.MoneyValid, true);
                return;
            }
            if ((decimal.Parse(txtFHPTimes.Text.Trim()) - 1) > 0)
            {
                BindEvalFHP();
                CloseSessionForHPUpdate((int)CusPaymentType.FixedHire_purchase);
            }
            else
            {
                ShowMessage(SumMoneyInvalid, true);
                return;
            }
            if (!txtFHPFirstMoney.Text.Equals(""))
            {
                txtFHPFirstMoney.Text = Convert2Currency(txtFHPFirstMoney.Text.Trim());
            }
        }

        #endregion

        #region Customer check - no check anymore

        ////if (txtCustomerIdentifyNumber.Text.Trim() != "")
        //if (_CustomerID.Value != "")
        //{
        //    long cId;
        //    long.TryParse(_CustomerID.Value, out cId);
        //    ISession sess = NHibernateSessionManager.Instance.GetSession();
        //    IList lstCus = sess.CreateCriteria(typeof(Customer))
        //        .Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
        //        .Add(Expression.Eq("Id", cId)).List();
        //    if (lstCus.Count > 0)
        //    {
        //        Customer cusIns = lstCus[0] as Customer;
        //        lbCustomerFullName.Text = cusIns.Fullname;
        //        _CustomerID.Value = cusIns.Id.ToString();
        //    }
        //    else
        //    {
        //        lbCustomerFullName.Text = txtCustomerIdentifyNumber.Text = "";
        //        ShowMessage(Customers.CustomerEmpty, true);
        //        return;
        //    }
        //}
        //else
        //{
        //    ShowMessage(Customers.CustomerEmpty, true);
        //    return;
        //}

        #endregion

        //Convert money
        txtPriceTax.Text = Convert2Currency(txtPriceTax.Text.Trim());
        txtPriceTax.CssClass = "readOnlyInputField";
        txtPriceTax.ReadOnly = true;
        if (txtPriceTax.Text.Trim().Equals("") || txtPriceTax.Text.Trim().Equals("0"))
        {
            ddlPaymentMethod.Enabled = false;
            ddlPaymentMethod.SelectedIndex = 0;
        }
        txtRecCDate.ReadOnly = true;
        txtRecCDate.CssClass = "readOnlyInputField";
        lbCustomerFullName.Text = _CustomerFullName.Value;
        txtCustomerIdentifyNumber.ReadOnly = true;
        txtCustomerIdentifyNumber.CssClass = "readOnlyInputField";
        txtSellingDate.ReadOnly = true;
        txtSellingDate.CssClass = "readOnlyInputField";
        txtTakPlateNoDate.ReadOnly = true;
        txtTakPlateNoDate.CssClass = "readOnlyInputField";
        txtEngineNo.ReadOnly = true;
        txtEngineNo.CssClass = "readOnlyInputField";

        CalendarExtender2.Enabled = false;

        btnAdd.Enabled = true;
        btnCheckValid.Visible = false;
        btnSave.Visible = true;
    }

    /*
    public bool CheckValid(DateTime sellDate, int paymentMethod, int priceTax, string dealerCode, DateTime recCDate, DateTime recDate,
                           DateTime[] UHPDate, int[] UHPAmount)
    {
        if ((!DateTime.TryParse(txtSellingDate.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces,
                          out sellDate)) || !IsValidSellDate(txtEngineNo.Text, sellDate))
        {
            ShowMessage(Customers.WrongSellingDate, true);
            return false;
        }

        if (InventoryHelper.IsInventoryLock(sellDate, UserHelper.DealerCode, UserHelper.BranchCode))
        {
            ShowMessage(Message.InventoryLocked, true);
            return false;
        }

        if (paymentMethod != 0)
        {
            if (priceTax <= 0)
            {
                ShowMessage(Customers.PriceEmpty, true);
                return false;
            }
            if (recCDate.Equals(DateTime.MinValue) && paymentMethod == 1)
            {
                ShowMessage(Customers.txtDateRecEmpty, true);
                return false;
            }
        }

        if (dealerCode.Equals(""))
        {
            ShowMessage(Customers.MotorInformationNull, true);
            return false;
        }
        if (sellDate < recCDate && !sellDate.Equals(DateTime.MinValue))
        {
            ShowMessage(Customers.RecDateSmallerThanSelDate, true);
            return false;
        }
        if (recCDate.Equals(DateTime.MinValue))
        {
            ShowMessage(Customers.RecDateInvalid, true);
            return false;
        }

        #region UnFixedHire-Purchase

        if (ddlPaymentMethod.SelectedValue.Equals(((int)CusPaymentType.UnFixedHire_purchase).ToString()))
        {
            decimal SumCheck = 0;
            TextBox txtIntentDatePay, UHPtxtMoney, subtxtIntentDatePay;
            recDate = DateTime.MinValue;
            if (!txtRecCDate.Text.Trim().Equals(""))
                DateTime.TryParse(txtRecCDate.Text.Trim(), new CultureInfo(UserHelper.Language),
                                  DateTimeStyles.AllowWhiteSpaces, out recDate);

            for (int i = 1; i < 6; i++)
            {
                try
                {
                    if (UHPDate[i] != null && UHPAmount[i] > 0)
                    {
                        SumCheck += UHPAmount[i];

                        if (!UHPDate[i].Equals(DateTime.MinValue) && UHPDate[i] < recDate)
                        {
                            ShowMessage(Customers.DateUFHPValid, true);
                            return false;
                        }
                        for (int k = 1; k < i; k++)
                        {
                            if (UHPDate[i].Equals(UHPDate[k]))
                            {
                                ShowMessage(Customers.DateUFHPValid, true);
                                return false;
                            }

                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Error");
                }
            }
            if (!SumCheck.Equals(priceTax))
            {
                ShowMessage(SumMoneyInvalid, true);
                return false;
            }
            else CloseSessionForHPUpdate((int)CusPaymentType.UnFixedHire_purchase);
        }

        #endregion

        #region Fixed Hire-purchase

        if (ddlPaymentMethod.SelectedValue.Equals(((int)CusPaymentType.FixedHire_purchase).ToString()))
        {
            if (decimal.Parse(txtFHPFirstMoney.Text) >= decimal.Parse(txtPriceTax.Text))
            {
                ShowMessage(Customers.MoneyValid, true);
                return;
            }
            if ((decimal.Parse(txtFHPTimes.Text.Trim()) - 1) > 0)
            {
                BindEvalFHP();
                CloseSessionForHPUpdate((int)CusPaymentType.FixedHire_purchase);
            }
            else
            {
                ShowMessage(SumMoneyInvalid, true);
                return;
            }
            if (!txtFHPFirstMoney.Text.Equals(""))
            {
                txtFHPFirstMoney.Text = Convert2Currency(txtFHPFirstMoney.Text.Trim());
            }
        }

        #endregion

        #region Customer check - no check anymore

        ////if (txtCustomerIdentifyNumber.Text.Trim() != "")
        //if (_CustomerID.Value != "")
        //{
        //    long cId;
        //    long.TryParse(_CustomerID.Value, out cId);
        //    ISession sess = NHibernateSessionManager.Instance.GetSession();
        //    IList lstCus = sess.CreateCriteria(typeof(Customer))
        //        .Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
        //        .Add(Expression.Eq("Id", cId)).List();
        //    if (lstCus.Count > 0)
        //    {
        //        Customer cusIns = lstCus[0] as Customer;
        //        lbCustomerFullName.Text = cusIns.Fullname;
        //        _CustomerID.Value = cusIns.Id.ToString();
        //    }
        //    else
        //    {
        //        lbCustomerFullName.Text = txtCustomerIdentifyNumber.Text = "";
        //        ShowMessage(Customers.CustomerEmpty, true);
        //        return;
        //    }
        //}
        //else
        //{
        //    ShowMessage(Customers.CustomerEmpty, true);
        //    return;
        //}

        #endregion

        //Convert money
        txtPriceTax.Text = Convert2Currency(txtPriceTax.Text.Trim());
        txtPriceTax.CssClass = "readOnlyInputField";
        txtPriceTax.ReadOnly = true;
        if (txtPriceTax.Text.Trim().Equals("") || txtPriceTax.Text.Trim().Equals("0"))
        {
            ddlPaymentMethod.Enabled = false;
            ddlPaymentMethod.SelectedIndex = 0;
        }
        txtRecCDate.ReadOnly = true;
        txtRecCDate.CssClass = "readOnlyInputField";
        txtCustomerIdentifyNumber.ReadOnly = true;
        txtCustomerIdentifyNumber.CssClass = "readOnlyInputField";
        txtSellingDate.ReadOnly = true;
        txtSellingDate.CssClass = "readOnlyInputField";
        txtTakPlateNoDate.ReadOnly = true;
        txtTakPlateNoDate.CssClass = "readOnlyInputField";
        txtEngineNo.ReadOnly = true;
        txtEngineNo.CssClass = "readOnlyInputField";

        CalendarExtender2.Enabled = false;

        btnAdd.Enabled = true;
        btnCheckValid.Visible = false;
        btnSave.Visible = true;
    }
    */
    protected void GetCustomer_OnClick(object sender, EventArgs e)
    {
        isCustomerEditted = true;
        txtCustomerIdentifyNumber.Text = txtCustomerIdentifyNumber.Text.Trim();
        lbCustomerFullName.Text = _CustomerFullName.Value;
    }

    protected void SaveOrUpdateCustomer()
    {
        ISession sess = NHibernateSessionManager.Instance.GetSession();
        var tx = sess.BeginTransaction();
        try
        {
            long id;
            long.TryParse(_CustomerID.Value, out id);
            string idnum = txtCustomerIdentifyNumber.Text.Trim();

            string ActionString = "DEFAULT";

            // check for existing Identifynumber
            IList lstCus = sess.CreateCriteria(typeof(Customer))
                            .Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
                            .Add(Expression.Not(Expression.Eq("Id", id)))
                            .Add(Expression.Eq("Identifynumber", idnum)).List();
            if (lstCus.Count > 0)
            {
                lbCustomerFullName.Text = "";
                throw new Exception(string.Format(Customers.IdentifyNumberExist, idnum));
            }

            // check for edit or update cus
            lstCus = sess.CreateCriteria(typeof(Customer))
                   .Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
                   .Add(Expression.Eq("Id", id)).List();
            if (lstCus.Count > 0)
            {
                ActionString = "UPDATECUSTOMER";
            }

            string fullname = _CustomerFullName.Value;

            if (String.IsNullOrEmpty(fullname))
            {
                throw new Exception(Customers.CustomerEmpty);
            }

            bool gender;
            if (ddlSex.Value.Trim().Equals("1"))
            {
                gender = true;
            }
            else gender = false;

            DateTime dt;
            CultureInfo cultInfo = Thread.CurrentThread.CurrentCulture;
            DateTime.TryParse(txtBirthDate.Value, cultInfo, DateTimeStyles.AllowWhiteSpaces, out dt);
            if (dt.Equals(DateTime.MinValue) && !txtBirthDate.Value.Trim().Equals(""))
            {
                throw new Exception(Customers.BirthDateInvalid);
            }
            DateTime birthdate = dt;

            string address = txtAddress.Value.Trim();
            string provinceid = ddlProvince.Value.ToString().Trim();
            string districtid = txtDistrict.Value.Trim();
            int jobtype = int.Parse(tblCus_JobType.Value.ToString());
            //string email = (txtCEmail.Value.Trim().Length == 0) ? "" : txtCEmail.Value.Trim();
            string email = txtCEmail.Value.Trim();
            string tel = txtCPhone.Value.Trim();
            //string mobile = (txtCMobile.Value.Trim().Length == 0) ? "" : txtCMobile.Value.Trim();
            string mobile = txtCMobile.Value.Trim();
            int priority = int.Parse(ddlCus_SetType.Value);
            string customertype = ddlCusType.Value;
            string cusdesc;
            if (txtCus_Desc.Value.Length > 1024)
            {
                cusdesc = txtCus_Desc.Value.Substring(0, 1024).Trim();
            }
            else cusdesc = txtCus_Desc.Value.Trim();
            string precinct = txtPrecinct.Value.Trim();

            //Save Customer
            if (!ActionString.Equals("UPDATECUSTOMER"))
            {
                _CustomerID.Value =
                    CustomerHelper.SaveCustomer(ref sess, idnum, fullname, gender, birthdate, address, provinceid,
                                                districtid, jobtype, email, tel, mobile, priority, customertype, cusdesc,
                                                precinct, UserHelper.DealerCode, false).ToString();
            }
            else
            {
                if (isCustomerEditted)
                    _CustomerID.Value =
                        CustomerHelper.UpdateCustomer(ref sess, id, idnum, fullname, gender, birthdate, address, provinceid,
                                                      districtid, jobtype, email, tel, mobile, priority, customertype, cusdesc,
                                                      precinct, UserHelper.DealerCode, false).ToString();
            }
            tx.Commit();
            isCustomerEditted = false;
        }
        catch
        {
            tx.Rollback();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveOrUpdateCustomer();
        ISession sess = NHibernateSessionManager.Instance.GetSession();
        if (_PageStatus.Value.Equals(_INSERTNEWDATA))
        {
            InsertNewData(ref sess);
        }
        if (_PageStatus.Value.Equals(_UPDATEDATA))
        {
            UpdateData(ref sess);
        }
        // clear data
        _CustomerID.Value = "";
        txtCustomerIdentifyNumber.Text = "";

        //RefreshAll();
        //_PageStatus.Value = _DEFAULTPAGE;
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        Page.Response.Redirect("Customer.aspx");
    }

    private void BindEvalFHP()
    {
        int md = int.Parse(txtFHPPaidMoneyDate.Text.Trim());
        string DateRecStr = txtRecCDate.Text.Trim();
        txtFHPAllMoney.Text = Convert2Currency(txtPriceTax.Text.Trim());
        decimal AllMoney = decimal.Parse(txtPriceTax.Text.Trim());
        decimal RestOfMoneyFHP = AllMoney - decimal.Parse(txtFHPFirstMoney.Text.Trim());
        decimal MoneyOfInstalment = RestOfMoneyFHP / (decimal.Parse(txtFHPTimes.Text.Trim()) - 1);
        lbMoneyOfTimes.Text = Convert2Currency(MoneyOfInstalment.ToString());
        DateTime firstDate;
        DateTime lastDate = new DateTime();
        if (DateRecStr.Equals(""))
        {
            DateRecStr = DateTime.Now.ToShortDateString();
        }
        DateTime.TryParse(DateRecStr, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces,
                          out firstDate);
        for (int i = 1; i <= (decimal.Parse(txtFHPTimes.Text.Trim()) - 1); i++)
        {
            lastDate = firstDate.AddDays(md * i);
        }
        FHPPaidDateLast.Text = lastDate.ToShortDateString();
    }

    private void BindAndSaveFHPPayment(ref ISession sess, Sellitem sellitemIns)
    {
        string PriceTax = ValidCurrency(txtPriceTax.Text.Trim());
        string DateRecStr = txtRecCDate.Text.Trim();
        if (DateRecStr.Equals(""))
        {
            DateRecStr = DateTime.Now.ToShortDateString();
        }
        decimal AllMoney = decimal.Parse(PriceTax);
        decimal RestOfMoneyFHP = AllMoney - decimal.Parse(txtFHPFirstMoney.Text.Trim());
        decimal MoneyOfInstalment = RestOfMoneyFHP / (decimal.Parse(txtFHPTimes.Text.Trim()) - 1);
        int DateCount = int.Parse(txtFHPPaidMoneyDate.Text.Trim());
        DateTime firstDate;
        DateTime.TryParse(DateRecStr, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces,
                          out firstDate);

        Payment paymentIns;
        for (int i = 0; i <= (decimal.Parse(txtFHPTimes.Text.Trim()) - 1); i++)
        {
            paymentIns = new Payment();
            //Save the first instalment
            if (i == 0)
            {
                paymentIns.Amount = decimal.Parse(txtFHPFirstMoney.Text.Trim());
                paymentIns.Paymentdate = firstDate;
                paymentIns.Sellitem = sellitemIns;
            }
            else
            {
                DateTime lastDate = firstDate.AddDays(DateCount * i);
                paymentIns.Amount = MoneyOfInstalment;
                paymentIns.Paymentdate = lastDate;
                paymentIns.Sellitem = sellitemIns;
            }
            paymentIns.Status = 0;
            sess.SaveOrUpdate(paymentIns);
        }
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
                            txtIntentDatePay.ReadOnly = true;
                            txtIntentDatePay.CssClass = "readOnlyInputField";
                            UHPtxtMoney.ReadOnly = true;
                            UHPtxtMoney.CssClass = "readOnlyInputField";
                            CEUHPCandelar.Enabled = false;
                        }
                        catch (Exception)
                        {
                            throw new Exception("Error");
                        }
                    }
                    break;
                }
            case (int)CusPaymentType.FixedHire_purchase:
                {
                    ddlPaymentMethod.Enabled = false;
                    txtPriceTax.ReadOnly = true;
                    txtFHPTimes.ReadOnly = true;
                    txtFHPTimes.CssClass = "readOnlyInputField";
                    txtFHPPaidMoneyDate.ReadOnly = true;
                    txtFHPPaidMoneyDate.CssClass = "readOnlyInputField";
                    txtFHPFirstMoney.ReadOnly = true;
                    txtFHPFirstMoney.CssClass = "readOnlyInputField";
                    break;
                }
            default:
                break;
        }
    }

    private void RefreshAll()
    {
        _PageStatus.Value = _DEFAULTPAGE;
        txtEngineNo.ReadOnly = false;
        txtEngineNo.Text = "";
        plActionAccessData.Visible = false;
        plMainInput.Visible = false;
        FixedHP.Visible = false;
        UnfixedHP.Visible = false;
        btnAdd.Visible = true;
        btnEdit.Visible = true;
        btnTest.Visible = true;
    }

    private void UpdateData(ref ISession sess)
    {
        //if (checkDateRange(txtSellingDate.Text.Trim(), txtRecCDate.Text.Trim())) { ShowMessage(Resources.Customers.RecDateSmallerThanSelDate, true); return; }
        DateTime dtRecDate = DataFormat.DateFromString(txtRecCDate.Text), dtSellDate = DataFormat.DateFromString(txtSellingDate.Text);
        //if (InventoryHelper.IsInventoryLock(dtSellDate, UserHelper.DealerCode, UserHelper.BranchCode))
        //{
        //    ShowMessage(Message.InventoryLocked, true);
        //    return;
        //}
        string PriceTax;
        PriceTax = txtPriceTax.Text.Trim().Replace(",", "");
        PriceTax = txtPriceTax.Text.Trim().Replace(".", "");
        ITransaction tx = sess.BeginTransaction();
        try
        {
            Invoice InvoiceIns = sess.CreateCriteria(typeof(Invoice))
                                     .Add(Expression.InsensitiveLike("Enginenumber", txtEngineNo.Text.Trim().ToUpper()))
                                     .List()[0] as Invoice;
            //IList lstInvBIll = sess.CreateCriteria(typeof(Invoice)).Add(Expression.Eq("Invoicenumber", txtBillNo.Text.Trim())).List();
            //if (lstInvBIll.Count == 0)
            //{
            //    ShowMessage(Resources.Message.Invoice_BillDuplicated);
            //    return;
            //}
            InvoiceIns.Invoicenumber = txtBillNo.Text.Trim();

            Sellitem sellitemIns = InvoiceIns.Sellitem;
            InvoiceIns.Sellitem.Numberplate = txtPlateNo.Text.Trim();
            InvoiceIns.Sellitem.Selltype = txtSellingType.Text.Trim();
            InvoiceIns.Sellitem.Pricebeforetax = (!PriceTax.Equals("")) ? decimal.Parse(PriceTax) : 0;
            InvoiceIns.Sellitem.Paymenttype = int.Parse(ddlPaymentMethod.SelectedValue);

            //Begin: Save Payment Method

            #region Update UnfixedHire-purchase

            if (ddlPaymentMethod.SelectedValue.Equals(((int)CusPaymentType.UnFixedHire_purchase).ToString()))
            {
                TextBox txtIntentDatePay, UHPtxtMoney;
                //LstPayment Deleted
                IList lstPaymentDeleted = sess.CreateCriteria(typeof(Payment))
                    .Add(Expression.Eq("Sellitem", InvoiceIns.Sellitem))
                    //.Add(Expression.Eq("Status", (int)CusPaymentType.UnFixedHire_purchase))
                    .List();
                foreach (Payment pmDel in lstPaymentDeleted)
                {
                    sess.Delete(pmDel);
                }

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
                            pmIns.Sellitem = InvoiceIns.Sellitem;
                            pmIns.Paymentdate = DateTime.Parse(txtIntentDatePay.Text.Trim());
                            pmIns.Amount = decimal.Parse(UHPtxtMoney.Text.Trim());
                            pmIns.Status = 0;
                            sess.Save(pmIns);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            #endregion

            #region Update FixedHire-purchase

            if (ddlPaymentMethod.SelectedValue.Equals(((int)CusPaymentType.FixedHire_purchase).ToString()))
            {
                //LstPayment Deleted
                IList lstPaymentDeleted = sess.CreateCriteria(typeof(Payment))
                    .Add(Expression.Eq("Sellitem", InvoiceIns.Sellitem))
                    //.Add(Expression.Eq("Status", (int)CusPaymentType.FixedHire_purchase))
                    .List();
                foreach (Payment pmDel in lstPaymentDeleted)
                {
                    sess.Delete(pmDel);
                }
                BindAndSaveFHPPayment(ref sess, InvoiceIns.Sellitem);
            }

            #endregion

            //End: Save Payment Method

            if (txtTakPlateNoDate.Text.Trim() != "")
            {
                InvoiceIns.Sellitem.Numplaterecdate = DataFormat.DateFromString(txtTakPlateNoDate.Text);
            }
            else InvoiceIns.Sellitem.Numplaterecdate = DateTime.MinValue;
            InvoiceIns.Sellitem.Paymentdate = dtRecDate;
            InvoiceIns.Sellitem.Commentsellitem = txtComment.Text.Trim();
            sess.SaveOrUpdate(InvoiceIns);

            tx.Commit();
            RefreshAll();
            _PageStatus.Value = _DEFAULTPAGE;
            ShowMessage(Message.ActionSucessful, false);
        }
        catch (Exception e)
        {
            tx.Rollback();
            ShowMessage(e.Message, true);
        }
    }

    private void InsertNewData(ref ISession sess)
    {
        //if (checkDateRange(txtSellingDate.Text.Trim(), txtRecCDate.Text.Trim())) { ShowMessage(Resources.Customers.RecDateSmallerThanSelDate, true); return; }
        //Datetime
        DateTime dtRecDate = DataFormat.DateFromString(txtRecCDate.Text), dtSellDate = DataFormat.DateFromString(txtSellingDate.Text);

        string PriceTax;
        PriceTax = txtPriceTax.Text.Trim().Replace(",", "");
        PriceTax = txtPriceTax.Text.Trim().Replace(".", "");

        ITransaction tx = sess.BeginTransaction();
        try
        {
            string Billno = txtBillNo.Text.Trim();
            //string idnum = _CustomerID.Value.Trim();
            if (_CustomerID.Value.Trim() == "-1")
            {
                ShowMessage(Message.Cus_NoCustomer, true);
                return;
            }
            Invoice InvoiceIns = new Invoice();
            // so hoa don giua 2 dai ly co the trung
            IList lstNonDulpInvoiceIns = sess.CreateCriteria(typeof(Invoice))
                .Add(Expression.Eq("Invoicenumber", Billno))
                .Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
                .List();
            if (lstNonDulpInvoiceIns.Count > 0)
            {
                ShowMessage(Customers.Invoice_BillDuplicated, true);
                return;
            }
            else
            {
                lstNonDulpInvoiceIns = sess.CreateCriteria(typeof(Invoice))
                    .Add(Expression.InsensitiveLike("Enginenumber", txtEngineNo.Text.Trim().ToUpper()))
                    //.Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
                    .List();
                if (lstNonDulpInvoiceIns.Count > 0)
                {
                    ShowMessage(Customers.Invoice_BillDuplicated, true);
                    return;
                }
                else InvoiceIns.Invoicenumber = Billno;
            }

            //Begin: Update SellItem
            Sellitem SellItemIns = new Sellitem();
            SellItemIns.Numberplate = txtPlateNo.Text.Trim();
            SellItemIns.Selltype = txtSellingType.Text.Trim();
            SellItemIns.Pricebeforetax = (!PriceTax.Equals("")) ? decimal.Parse(PriceTax) : 0;
            SellItemIns.Paymenttype = int.Parse(ddlPaymentMethod.SelectedValue);


            if (txtTakPlateNoDate.Text.Trim() != "")
            {
                SellItemIns.Numplaterecdate = DataFormat.DateFromString(txtTakPlateNoDate.Text);
            }
            else SellItemIns.Numplaterecdate = DateTime.MinValue;

            SellItemIns.Paymentdate = dtRecDate;
            SellItemIns.Commentsellitem = txtComment.Text.Trim();
            sess.SaveOrUpdate(SellItemIns);
            InvoiceIns.Sellitem = SellItemIns;
            //End: Update SellItem
            //Begin: Save Payment Method
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
                            pmIns.Sellitem = SellItemIns;
                            pmIns.Paymentdate = DateTime.Parse(txtIntentDatePay.Text.Trim());
                            pmIns.Amount = decimal.Parse(UHPtxtMoney.Text.Trim());
                            pmIns.Status = 0;
                            sess.Save(pmIns);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            if (ddlPaymentMethod.SelectedValue.Equals(((int)CusPaymentType.FixedHire_purchase).ToString()))
            {
                BindAndSaveFHPPayment(ref sess, SellItemIns);
            }
            //End: Save Payment Method
            //Begin: Update ItemInstance
            Iteminstance ItmIns = sess.CreateCriteria(typeof(Iteminstance))
                                      .Add(Expression.Eq("Id", Int64.Parse(_ItemInstanceID.Value))).List()[0] as
                                  Iteminstance;
            ItmIns.Status = (int)ItemStatus.Sold;
            ItmIns.Releaseddate = dtSellDate;
            sess.SaveOrUpdate(ItmIns);
            if (PriceTax.Equals("")) PriceTax = "0";
            TransHis th =
                (new ItemHepler()).SaveTranHis(ItmIns, dtSellDate, ItemStatus.Sold,
                                               int.Parse(ddlPaymentMethod.SelectedValue), decimal.Parse(PriceTax),
                                               UserHelper.Username, null, UserHelper.FullBranchCode, null);
            sess.Save(th);

            // save to Inventory of Day
            InventoryHelper.SaveInventoryDay(ItmIns.Item.Id, dtSellDate, -1, (int)ItemStatus.Sold,
                                             UserHelper.DealerCode, UserHelper.BranchCode);


            InvoiceIns.Iteminstance = ItmIns;
            //End: Update ItemInstance
            //Begin: Customer Type
            Customer cusIns =
                sess.CreateCriteria(typeof(Customer)).Add(Expression.Eq("Id", Int64.Parse(_CustomerID.Value)))
                .List()[0] as Customer;
            InvoiceIns.Customer = cusIns;

            InvoiceIns.Enginenumber = ItmIns.Enginenumber;
            InvoiceIns.Dealercode = ItmIns.Dealercode;
            InvoiceIns.Branchcode = ItmIns.Branchcode;
            InvoiceIns.Createddate = DateTime.Now;
            InvoiceIns.Selldate = dtSellDate;
            InvoiceIns.Createdby = UserHelper.Username;
            sess.SaveOrUpdate(InvoiceIns);

            #region warrantyInfo
            //********** Save warranty info ***************
            WarrantyInfo warr = ServiceTools.GetWarrantyInfo(ItmIns.Enginenumber);
            if (warr == null)   // co roi thi thoi: yeu cau ngay 16/7/2008
            {
                //Clone new customer
                Customer csnew = new Customer(cusIns.Identifynumber, cusIns.Fullname, cusIns.Gender, cusIns.Birthdate, cusIns.Address, cusIns.Provinceid, cusIns.Districtid, cusIns.Jobtypeid, cusIns.Email, cusIns.Tel, cusIns.Mobile, (int)CusType.WarrantyInfo, cusIns.Customerdescription, cusIns.Precinct, cusIns.Priority, cusIns.Dealercode, false);
                sess.Save(csnew);

                if (ServiceTools.SaveWarrantyInfo(ItmIns.Enginenumber, 0, dtSellDate,
                ItmIns.Databasecode, ItmIns.Item.Id, ItmIns.Color, UserHelper.DealerCode, csnew.Id) == false)
                {
                    throw new Exception(Message.WarrantyContent_UpdateDataFailed);
                }
            }
            #endregion

            tx.Commit();
            ShowMessage(Message.ActionSucessful, false);

            RefreshAll();
            _PageStatus.Value = _DEFAULTPAGE;
        }
        catch (Exception ex)
        {
            if (!tx.WasRolledBack)
                tx.Rollback();
            ShowMessage(ex.Message, true);
        }
    }

    private string GetTax()
    {
        return "10";
    }

    private static string ValidCurrency(string InvalidString)
    {
        InvalidString = InvalidString.Replace(",", "");
        InvalidString = InvalidString.Replace(".", "");
        return InvalidString;
    }

    private static bool ValidDateFormat(string oDate)
    {
        DateTime returnDate;
        DateTime.TryParse(oDate, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out returnDate);
        return (returnDate.Equals(DateTime.MinValue)) ? false : true;
    }

    protected static string Convert2Currency(string ObConvert)
    {
        NumberFormatInfo nfi = Thread.CurrentThread.CurrentCulture.NumberFormat;

        if (ObConvert.Trim().Equals("")) return "0";
        else
        {
            decimal ICurency = decimal.Parse(ObConvert);
            nfi.NumberDecimalDigits = 0;
            return ICurency.ToString("N", nfi);
        }
    }

    private void ShowMessage(string mesg, bool isError)
    {
        //if (t.ActiveTabIndex == 0)
        //{
            lbMes.Visible = true;
            lbMes.ControlStyle.ForeColor = (isError) ? Color.Red : Color.RoyalBlue;
            lbMes.Text = mesg;
        //}
        //else
        //{
        //    lbMes2.Visible = true;
        //    lbMes2.ControlStyle.ForeColor = (isError) ? Color.Red : Color.RoyalBlue;
        //    lbMes2.Text = mesg;
        //}
    }

    private RangeValidator DateValid(RangeValidator rvDate)
    {
        return DateValid(rvDate, DateTime.MaxValue);
    }

    private RangeValidator DateValid(RangeValidator rvDate, DateTime maxVal)
    {
        rvDate.MinimumValue = DateTime.MinValue.ToShortDateString();
        rvDate.MaximumValue = maxVal.ToShortDateString();
        return rvDate;
    }

    private static bool checkDateRange(string d1, string d2)
    {
        DateTime dt1, dt2;
        DateTime.TryParse(d1, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out dt1);
        DateTime.TryParse(d2, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out dt2);
        return (dt1 > dt2) ? false : true;
    }

    protected void gvSelectxxx_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        if (gv.TopPagerRow == null) return;
        Literal litPageInfo = gv.TopPagerRow.FindControl("lit" + gv.ID + "PageInfo") as Literal;
        if (litPageInfo != null)
            litPageInfo.Text =
                string.Format(Message.PagingInfo, gv.PageIndex + 1, gv.PageCount,
                              HttpContext.Current.Items["list" + gv.ID + "RowCount"]);
    }

    protected void gvSelectxxx_page(object sender, GridViewCommandEventArgs e)
    {
        GridView gv = (GridView)sender;
        if (((string)e.CommandName == "Page") &&
            ((((string)e.CommandArgument == "Next") && ((gv.PageIndex + 1) == gv.PageCount)) ||
             (((string)e.CommandArgument == "Prev") && (gv.PageIndex == 0))))
            gv.DataBind();
    }

    // user select an item. set it to engineNumber textbox 
    protected void gvSelectItem_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridViewRow row = gvSelectItem.Rows[e.NewSelectedIndex];
        if (row == null) return;
        Literal lit = (Literal)WebTools.FindControlById("litSelectedSoldItem", row);
        if (lit == null) return;

        txtEngineNo.Text = lit.Text;
        btnTest_Click(sender, e);

        SetAvtiveView(vwMainIndex);
    }

    private void SetAvtiveView(int Index)
    {
        MultiView1.ActiveViewIndex = Index;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        SetAvtiveView(vwMainIndex);
    }

    protected void btnSelectItem_Click(object sender, EventArgs e)
    {
        odsSelectItem.SelectParameters["engineNumberLike"] =
            new Parameter("engineNumberLike", TypeCode.String, txtEngineNo.Text.Trim());
        odsSelectItem.SelectMethod = (_PageStatus.Value == _INSERTNEWDATA) ? "SelectForSale" : "SelectAll";
        gvSelectItem.PageIndex = 0;
        gvSelectItem.DataBind();

        if ((gvSelectItem.Rows.Count == 1))
        {
            Literal lit = (Literal)WebTools.FindControlById("litSelectedSoldItem", gvSelectItem.Rows[0]);
            txtEngineNo.Text = lit.Text;
        }
        else if (gvSelectItem.Rows.Count > 1)
        {
            SetAvtiveView(vwSelectItemIndex);
        }
        else // item not found
        {
        }

        btnTest_Click(sender, e);
    }

    //protected void Button1_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (FileUpload1.HasFile)
    //        {
    //            SaleVehicleDAO.Init();
    //            SaleVehicleDAO.ImportExcelData(FileUpload1.PostedFile.InputStream, VDMSSetting.CurrentSetting.SaleVehicleExcelUploadSetting);
    //        }
    //        lvE.DataSource = SaleVehicleDAO.Vehicles;
    //        lvE.DataBind();
    //        //ShowMessage(Resources.Message.ActionSucessful, false);
    //    }
    //    catch (Exception ex)
    //    {
    //        //ShowMessage(ex.Message, true);
    //    }
    //}

    //protected void Button2_OnClick(Object sender, EventArgs e)
    //{
    //    BindRowToControls(SaleVehicleDAO.Vehicles[0]);
    //    btnSave_Click(null, null);
    //}

    //protected void Button3_OnClick(object sender, EventArgs e)
    //{
    //    SaleVehicleDAO.ClearSession();
    //    lvE.DataBind();
    //}
}
