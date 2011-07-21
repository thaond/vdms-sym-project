using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using NHibernate;
using NHibernate.Expression;
using Resources;
using VDMS;
using VDMS.Common.Web;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.TipTop;
using VDMS.Helper;
using VDMS.I.Entity;
using VDMS.I.ObjectDataSource;
using VDMS.I.Service;
using System.Web.UI;
using Customer = VDMS.Core.Domain.Customer;
using Dealer = VDMS.Data.TipTop.Dealer;
using Invoice = VDMS.Core.Domain.Invoice;

public partial class Service_WarrantyInfo : BasePage
{
    #region object
    protected string LoadCustomerErr = Message.LoadCustomerErr;
    string minSellDate = DateTime.MinValue.ToShortDateString();

    protected enum CurrentAction
    {
        AddNew,
        Edit,
    }
    #endregion

    #region event method
    protected void Page_Load(object sender, EventArgs e)
    {
        //lbMes.Text = String.Empty;
        InitErrMsgControl(dvMsg);
        InitInfoMsgControl(dvMsg);
        if (!Page.IsPostBack)
        {
            btnExit.OnClientClick = "javascript:window.location.replace('WarrantyInfo.aspx');return false;";
            LoadCustomerErr = Message.LoadCustomerErr;
            rvPurchaseDateInput = DateValid(rvPurchaseDateInput, minSellDate, DateTime.Now.ToShortDateString());
            rvBirthDate = DateValid(rvBirthDate, DateTime.MinValue.ToShortDateString(), DateTime.Now.ToShortDateString());
            LoadAllProvince();
            // disable enter
            ScanControl<WebControl, IEditableTextControl>(Page, RemoveDefaultEnterButton);
            // enable enter
            InitDefaultEnterButton(txtEngineNumberInput, btnFilterEng);
        }
    }

    protected void gvItem_DataBound(object sender, EventArgs e)
    {
        if (gvItem.TopPagerRow == null) return;
        Literal litPageInfo = gvItem.TopPagerRow.FindControl("litPageInfo") as Literal;
        if (litPageInfo != null) litPageInfo.Text = string.Format(Message.PagingInfo, gvItem.PageIndex + 1, gvItem.PageCount, HttpContext.Current.Items["rowCount"]);
    }
    protected void WarrantyInfoAddNew(object sender, EventArgs e)
    {
        GUIResetInputPanel();
        plInputWInf.Visible = true;
        InputGUIDisplay(false);
        getDealerCode(CurrentAction.AddNew);
    }
    protected void InputWarrantyInfo(object sender, EventArgs e)
    {
        gvItemins.Visible = false;

        //chac' cu', lam luc ko validate client dc
        if (string.IsNullOrEmpty(txtPurchaseDateInput.Text.Trim()))
        {
            rfvPurchaseDateInput.IsValid = false;
            return;
        }

        ItemInstanceDataSource ids = new ItemInstanceDataSource();
        IList<ItemInstance> lsIns = ids.Select(2, 0, txtEngineNumberInput.Text.Trim(), null);
        if (lsIns.Count != 1)
        {
            btnInsertWarrantyInfo.CommandName = "";
            ShowMessage(Message.WarrantyInfo_EngineNumberInvalid, true);
            return;
        }
        ItemInstance iIns = lsIns[0];
        //BindItemInf(iIns.Enginenumber);
        if (!btnInsertWarrantyInfo.CommandName.Equals("ActionConflict"))
        {
            _CustomerID.Value = "-1";
        }

        //ISession sess = NHibernateSessionManager.Instance.GetSession();
        //IList lstItemWan = sess.CreateCriteria(typeof(Warrantyinfo))
        //    .Add(Expression.Eq("Id", iIns.Enginenumber)).List();
        //if (lstItemWan.Count > 0 && !btnInsertWarrantyInfo.CommandName.Equals("ActionConflict"))
        //{
        //    ShowMessage(Message.WarrantyInfo_EditStatus, false);
        //    Warrantyinfo wr = lstItemWan[0] as Warrantyinfo;
        //    txtKmCountInput.Text = wr.Kmcount.ToString();
        //    txtPurchaseDateInput.Text = wr.Purchasedate.ToShortDateString();
        //    ddlDealerCode.SelectedValue = wr.Selldealercode;
        //    ddlDealerCode.Enabled = false;
        //    BindCustomer(wr.Customer);

        //    btnInsertWarrantyInfo.CommandName = "ActionConflict";
        //    rfvPurchaseDateInput.ValidationGroup = "Input";
        //    return;
        //}
        //else if (lstItemWan.Count == 0 && !btnInsertWarrantyInfo.CommandName.Equals("ActionAddnew"))
        //{
        //    btnInsertWarrantyInfo.CommandName = "ActionAddnew";
        //    rfvPurchaseDateInput.ValidationGroup = "Input";
        //    return;
        //}

        //GUI: disable three input field of warranty info
        txtEngineNumberInput.ReadOnly = true; txtEngineNumberInput.CssClass = "readOnlyInputField";
        txtEngineNumberInput.Text = txtEngineNumberInput.Text.Trim();
        txtPurchaseDateInput.Text = txtPurchaseDateInput.Text.Trim(); txtPurchaseDateInput.CssClass = "readOnlyInputField";
        txtPurchaseDateInput.ReadOnly = true;
        if (String.IsNullOrEmpty(txtKmCountInput.Text.Trim())) txtKmCountInput.Text = "0";
        txtKmCountInput.ReadOnly = true; txtKmCountInput.CssClass = "readOnlyInputField";
        txtCustomerIDInput.ReadOnly = false; txtCustomerIDInput.CssClass = "";
        ddlDealerCode.Enabled = false;
        btnInsertCus.Enabled = true;
        btnInsertWarrantyInfo.Enabled = false;

        if (ServiceTools.SaveWarrantyInfo(iIns.EngineNumber, int.Parse(txtKmCountInput.Text), DateTime.Parse(txtPurchaseDateInput.Text),
            iIns.DatabaseCode, iIns.Item.ItemCode, iIns.Color, ddlDealerCode.SelectedValue, -1) == false)
        {
            ShowMessage(Message.WarrantyContent_UpdateDataFailed, true);
        }
        else
        {
            ShowMessage(Message.ActionSucessful, false);
        }
        btnInsertWarrantyInfo.CommandName = "";
        btnFilterEng.Enabled = false;
        rfvPurchaseDateInput.ValidationGroup = "InputDraft";
        CusInfph.Visible = true;
    }
    protected void InsertData(object sender, EventArgs e)
    {
        if (!CusInfph.Visible) { CusInfph.Visible = true; return; }
        string ActionString = "DEFAULT";
        ISession sess = NHibernateSessionManager.Instance.GetSession();
        string EngNo = txtEngineNumberInput.Text.Trim();
        Customer cusIns = new Customer();
        if (long.Parse(_CustomerID.Value) > 0)
            cusIns.Id = long.Parse(_CustomerID.Value);

        DateTime pDate;
        DateTime.TryParse(txtPurchaseDateInput.Text.Trim(), new CultureInfo(UserHelper.Language),
                          DateTimeStyles.AllowWhiteSpaces, out pDate);
        if (pDate.Equals(DateTime.MinValue))
        {
            ShowMessage(Message.WarrantyInfo_PurchaseDateInvalid, true); return;
        }
        IList lstCus = sess.CreateCriteria(typeof(Customer))
            .Add(Expression.Eq("Id", long.Parse(_CustomerID.Value)))
            .Add(Expression.Eq("Forservice", true))
            .List();
        if (lstCus.Count > 0)
        {
            cusIns = lstCus[0] as Customer;
            ActionString = "UPDATECUSTOMER";
        }
        DateTime dt;
        CultureInfo cultInfo = Thread.CurrentThread.CurrentCulture;
        DateTime.TryParse(txtBirthDateInput.Text, cultInfo, DateTimeStyles.AllowWhiteSpaces, out dt);
        if (dt.Equals(DateTime.MinValue) && !txtBirthDateInput.Text.Trim().Equals(""))
        {
            ShowMessage(Customers.BirthDateInvalid, true);
        }
        //DateTime birthdate = dt;
        cusIns.Birthdate = dt;

        //string idnum = txtCustomerIDInput.Text.Trim();
        cusIns.Identifynumber = txtCustomerIDInput.Text.Trim();
        //string fullname = txtCusNameInput.Text;
        cusIns.Fullname = txtCusNameInput.Text;
        bool gender;
        if (ddlSexInput.SelectedValue.Trim() == "1")
        {
            gender = true;
        }
        else gender = false;
        cusIns.Gender = gender;
        //string address = txtAddressInput.Text.Trim();
        //string precinct = txtPrecinctInput.Text.Trim();
        //string districtid = txtDistrictInput.Text.Trim();
        //string provinceid = ddlProvinceInput.SelectedValue.Trim();
        cusIns.Address = txtAddressInput.Text.Trim();
        cusIns.Precinct = txtPrecinctInput.Text.Trim();
        cusIns.Districtid = txtDistrictInput.Text.Trim();
        cusIns.Provinceid = ddlProvinceInput.SelectedValue.Trim();

        //int jobtype = int.Parse(tblCus_JobType.SelectedValue);
        cusIns.Jobtypeid = int.Parse(tblCus_JobType.SelectedValue);
        //string email = txtEmail.Text.Trim();
        cusIns.Email = txtEmail.Text.Trim();
        //string tel = txtPhone.Text.Trim();
        cusIns.Tel = txtPhone.Text.Trim();
        //string mobile = txtMobile.Text.Trim();
        cusIns.Mobile = txtMobile.Text.Trim();
        //int priority = int.Parse(ddlCus_SetType.SelectedValue);
        //string customertype = ddlCusType.SelectedValue;
        cusIns.Priority = int.Parse(ddlCus_SetType.SelectedValue);
        cusIns.Customertype = int.Parse(ddlCusType.SelectedValue);
        if (txtCus_Desc.Text.Length > 1024)
        {
            cusIns.Customerdescription = txtCus_Desc.Text.Substring(0, 1024).Trim();
        }
        else cusIns.Customerdescription = txtCus_Desc.Text.Trim();

        //static field
        cusIns.Dealercode = ddlDealerCode.SelectedValue;
        if (ActionString != "UPDATECUSTOMER") cusIns.Forservice = true;

        //Save Customer
        using (ITransaction tx = sess.BeginTransaction())
        {
            tx.Begin();
            sess.SaveOrUpdate(cusIns);
            if (ServiceTools.SaveWarrantyInfo(EngNo, -1, DateTime.MinValue,
                    null, null, null, null, cusIns.Id) == false)
            {
                tx.Rollback();
                ShowMessage(Message.WarrantyContent_UpdateDataFailed, true);
                btnInsertCus.Enabled = true;
            }
            else
            {
                tx.Commit();
                ShowMessage(Message.ActionSucessful, false);
                btnInsertCus.Enabled = false;
            }
        }

        InputGUIDisplay(false);
        gvItem.DataBind();
    }
    protected void gvItem_OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        ISession sess = NHibernateSessionManager.Instance.GetSession();
        GUIResetInputPanel();
        Literal lien = gvItem.Rows[e.NewEditIndex].FindControl("liEnNo") as Literal;
        //Iteminstance itemins =
        //    sess.CreateCriteria(typeof(Iteminstance)).Add(Expression.Eq("Enginenumber", lien.Text)).List()[0] as
        //    Iteminstance;
        Literal liItemcode = gvItem.Rows[e.NewEditIndex].FindControl("liItemcode") as Literal;
        Literal liColor = gvItem.Rows[e.NewEditIndex].FindControl("liColor") as Literal;
        if (liItemcode != null) txtType.Text = liItemcode.Text;
        if (liColor != null) txtColor.Text = liColor.Text;
        //txtType.Text = itemins.Item.Itemtype;
        //txtColor.Text = itemins.Color;
        Literal likmCount = gvItem.Rows[e.NewEditIndex].FindControl("likmCount") as Literal;
        Literal liPdate = gvItem.Rows[e.NewEditIndex].FindControl("liPdate") as Literal;
        Literal liCusIdkeys = gvItem.Rows[e.NewEditIndex].FindControl("liCusIdkeys") as Literal;
        if (liCusIdkeys != null && !string.IsNullOrEmpty(liCusIdkeys.Text))
        {
            IList cus = sess.CreateCriteria(typeof(Customer))
                .Add(Expression.Eq("Id", long.Parse(liCusIdkeys.Text)))
                .List();
            if (cus.Count > 0)
            {
                Customer cusGet = cus[0] as Customer;
                BindCustomer(cusGet);
            }
        }
        //Literal licusid = gvItem.Rows[e.NewEditIndex].FindControl("licusid") as Literal;
        //Literal licusfullname = gvItem.Rows[e.NewEditIndex].FindControl("licusfullname") as Literal;
        if (lien != null) txtEngineNumberInput.Text = lien.Text;
        if (likmCount != null) txtKmCountInput.Text = likmCount.Text;
        if (liPdate != null) txtPurchaseDateInput.Text = liPdate.Text;

        InputGUIDisplay(false);
        getDealerCode(CurrentAction.Edit);
        //btnInsertWarrantyInfo.CommandName = "ActionConflict";

        // nmChi: edit thi cho no luu luon
        btnInsertWarrantyInfo.Enabled = true;

        SelectEngineNo(sender, e);  // luc chon edit thi check luon thong tin

        e.Cancel = true;
    }
    protected void ExportToExcel(object sender, EventArgs e)
    {
        int size = gvItem.PageSize;

        gvItem.PageSize = 65535;
        // edit column
        gvItem.Columns[gvItem.Columns.Count - 1].Visible = false;
        gvItem.DataBind();
        GridView2Excel.Export(gvItem, Page, "WarrantyInfo.xls");

        gvItem.PageSize = size;
    }
    protected void Search(object sender, EventArgs e)
    {
        gvItem.PageIndex = 0;
    }
    //protected void gvSelectxxx_PreRender(object sender, EventArgs e)
    //{
    //    GridView gv = (GridView)sender;
    //    if (gv.TopPagerRow == null) return;
    //    Literal litPageInfo = gv.TopPagerRow.FindControl("lit" + gv.ID + "PageInfo") as Literal;
    //    if (litPageInfo != null) litPageInfo.Text = string.Format(Message.PagingInfo, gv.PageIndex + 1, gv.PageCount, HttpContext.Current.Items["list" + gv.ID + "RowCount"]);
    //}
    //protected void gvSelectxxx_page(object sender, GridViewCommandEventArgs e)
    //{
    //    GridView gv = (GridView)sender;
    //    if ((e.CommandName == "Page") && (((e.CommandArgument == "Next") && ((gv.PageIndex + 1) == gv.PageCount)) || ((e.CommandArgument == "Prev") && (gv.PageIndex == 0))))
    //        gv.DataBind();
    //}

    void CheckPerm(string eng, string dealerCode, string db)
    {
        // cho nay da set sell date if invoice exist
        CheckEditStatus(eng, dealerCode, db);
        //Dbcheck(hdDbCode.Value, lit.Text); check trong CheckEditStatus roi

        // dai ly ko dc add xe cua DL khac
        btnInsertWarrantyInfo.Enabled = btnInsertWarrantyInfo.Enabled && (!UserHelper.IsDealer || (UserHelper.IsDealer && (dealerCode == UserHelper.DealerCode)));
        // and show warnning
        if (!string.IsNullOrEmpty(dealerCode) && (dealerCode != UserHelper.DealerCode) && UserHelper.IsDealer)
            AddErrorMsg(string.Format("{0}", Message.WarrantyContent_ItemSoldByOtherDealer));
        // ko cho DL sua so Km
        txtKmCountInput.Enabled = !UserHelper.IsDealer;
        txtPurchaseDateInput.Enabled = !UserHelper.IsDealer;
        ibPurchaseDateInput.Visible = !UserHelper.IsDealer;
    }
    protected void gvSelectItem_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //itemSelecting = true;
        GridViewRow row = gvItemins.Rows[e.NewSelectedIndex]; if (row == null) return;
        Literal lit = (Literal)WebTools.FindControlById("litSelectedSoldItem", row); if (lit == null) return;
        txtEngineNumberInput.Text = lit.Text;
        //SelectEngineNo(null, null);

        HiddenField hdDealerCode = (HiddenField)WebTools.FindControlById("hdDealerCode", row); if (hdDealerCode == null) return;
        HiddenField hdDbCode = (HiddenField)WebTools.FindControlById("hdDatabaseCode", row); if (hdDealerCode == null) return;

        string dealerCode = hdDealerCode.Value;
        if (string.IsNullOrEmpty(dealerCode))
        {
            DataRow saleInfo = Motorbike.GetItemSaleInfo(lit.Text, hdDbCode.Value);
            if (saleInfo != null) dealerCode = saleInfo["DEALERCODE"].ToString();
        }

        BindItemInf(e.NewSelectedIndex);

        //CheckEditStatus(lit.Text, dealerCode, hdDbCode.Value);
        //Dbcheck(hdDbCode.Value, lit.Text);
        CheckPerm(lit.Text, dealerCode, hdDbCode.Value);

        gvItemins.Visible = false;
    }
    protected void SelectEngineNo(object sender, EventArgs e)
    {
        CusInfph.Visible = false;

        // encode "%" + "..." here and then Datasource add "%" to end of string
        odsSelectItem.SelectParameters["engineNumberLike"] = new Parameter("engineNumberLike", TypeCode.String,txtEngineNumberInput.Text);
        gvItemins.PageIndex = 0;
        gvItemins.DataBind();
        if (gvItemins.Rows.Count > 1)
        {
            gvItemins.Visible = true;
        }
        else if (gvItemins.Rows.Count == 1)
        {
            GridViewRow row = gvItemins.Rows[0]; if (row == null) return;
            Literal lit = (Literal)WebTools.FindControlById("litSelectedSoldItem", row); if (lit == null) return;
            HiddenField hdDealerCode = (HiddenField)WebTools.FindControlById("hdDealerCode", row); if (hdDealerCode == null) return;
            HiddenField hdDbCode = (HiddenField)WebTools.FindControlById("hdDatabaseCode", row); if (hdDealerCode == null) return;
            HiddenField hdImporteddate = (HiddenField)WebTools.FindControlById("hdImporteddate", row);

            string dealerCode = hdDealerCode.Value;
            if (string.IsNullOrEmpty(dealerCode))
            {
                DataRow saleInfo = Motorbike.GetItemSaleInfo(lit.Text, hdDbCode.Value);
                if (saleInfo != null) dealerCode = saleInfo["DEALERCODE"].ToString();
                if (saleInfo["OUTSTOCKDATE"] != null) minSellDate = ((DateTime)saleInfo["OUTSTOCKDATE"]).ToShortDateString();
            }
            // get shipping_date/outstock_date as minSellDate
            minSellDate = hdImporteddate.Value;

            rvPurchaseDateInput = DateValid(rvPurchaseDateInput, minSellDate, DateTime.Now.ToShortDateString());

            txtEngineNumberInput.Text = lit.Text;
            BindItemInf(0);
            btnInsertWarrantyInfo.CommandName = "";
            CheckPerm(lit.Text, dealerCode, hdDbCode.Value);
        }
        else
        {
            ShowMessage(Message.WarrantyInfo_EngineNumberInvalid, true);
            gvItemins.Visible = false;
            btnInsertWarrantyInfo.Enabled = false; // yeu cau ngay 16/7/2008
        }
    }
    #endregion

    #region static/function method
    private void CheckEditStatus(string egnum, string dlcode, string dbCode)
    {
        ISession sess = NHibernateSessionManager.Instance.GetSession();
        IList<Warrantyinfo> lstItemWan = sess.CreateCriteria(typeof(Warrantyinfo))
            .Add(Expression.Eq("Id", egnum)).List<Warrantyinfo>();

        // check n compare info with invoice
        if (lstItemWan.Count > 0)
        {
            Warrantyinfo wr = lstItemWan[0] as Warrantyinfo;
            BindCustomer(wr.Customer);

            IList lstInv = sess.CreateCriteria(typeof(Invoice))
                .Add(Expression.Eq("Enginenumber", egnum))
                .List();
            if (lstInv.Count > 0)
            {
                Invoice inv = lstInv[0] as Invoice;
                if (!string.Equals(inv.Dealercode, dlcode, StringComparison.OrdinalIgnoreCase))
                {
                    ShowMessage(Message.WarrantyInfo_WrongDealercode, false);
                }
                if (!inv.Selldate.Equals(wr.Purchasedate))
                {
                    ShowMessage(Message.WarrantyInfo_WrongSellSDate, false);
                }
                txtPurchaseDateInput.Text = inv.Selldate.ToShortDateString();
                minSellDate = txtPurchaseDateInput.Text;
            }
        }

        if (lstItemWan.Count > 0 && !btnInsertWarrantyInfo.CommandName.Equals("ActionConflict"))
        {
            ShowMessage(Message.WarrantyInfo_EditStatus, false);
            Warrantyinfo wr = lstItemWan[0] as Warrantyinfo;
            txtKmCountInput.Text = wr.Kmcount.ToString();
            txtPurchaseDateInput.Text = wr.Purchasedate.ToShortDateString();

            try
            {
                ddlDealerCode.SelectedValue = wr.Selldealercode;
            }
            catch (Exception)
            {
                if (ddlDealerCode.Items.Count > 0)
                    ddlDealerCode.SelectedIndex = 0;
            }

            //ddlDealerCode.Enabled = false;    // nmChi: KH lai muon duoc sua cai dealer
            Dbcheck(dbCode, egnum);
            // dai ly khong duoc phep sua data da exist
            btnInsertWarrantyInfo.Enabled = btnInsertWarrantyInfo.Enabled && !UserHelper.IsDealer;// || lstItemWan[0].CreateByDealer == UserHelper.DealerCode;
            btnInsertWarrantyInfo.CommandName = "ActionConflict";
            //rfvPurchaseDateInput.ValidationGroup = "Input";
            return;
        }
        else if (lstItemWan.Count == 0 && !btnInsertWarrantyInfo.CommandName.Equals("ActionAddnew"))
        {
            btnInsertWarrantyInfo.CommandName = "ActionAddnew";
            btnInsertWarrantyInfo.Enabled = true;
            Dbcheck(dbCode, egnum);
            // If add new Iteminstance bind the dealer on TIPTOP
            try
            {
                ddlDealerCode.SelectedValue = dlcode;
            }
            catch (Exception) { }
            return;
        }
    }
    private void BindCustomer(Customer cusGet)
    {
        CusInfph.Visible = cusGet != null;
        if (cusGet != null)
        {
            _CustomerID.Value = cusGet.Id.ToString();
            txtCustomerIDInput.Text = cusGet.Identifynumber;
            txtCusNameInput.Text = cusGet.Fullname;
            if (!DateTime.MinValue.Equals(cusGet.Birthdate))
            {
                txtBirthDateInput.Text = cusGet.Birthdate.ToShortDateString();
            }

            txtAddressInput.Text = cusGet.Address;
            txtPrecinctInput.Text = cusGet.Precinct;
            txtDistrictInput.Text = cusGet.Districtid;
            ddlSexInput.SelectedValue = (cusGet.Gender) ? "1" : "0";
            try
            {
                if (!String.IsNullOrEmpty(cusGet.Provinceid))
                    ddlProvinceInput.SelectedValue = cusGet.Provinceid;
            }
            catch (Exception)
            {
                ddlProvinceInput.SelectedIndex = 0;
            }
        }
    }
    private void BindItemInf(int index)
    {
        HiddenField hdColor = ((HiddenField)gvItemins.Rows[index].FindControl("hdColor"));
        HiddenField hdItemtype = ((HiddenField)gvItemins.Rows[index].FindControl("hdItemtype"));
        if (hdItemtype != null) txtType.Text = hdItemtype.Value;
        if (hdColor != null) txtColor.Text = hdColor.Value;
        //else 
        //    txtColor.Text = gvItemins.Rows[index].Cells[2].Text;


        //ISession sess = NHibernateSessionManager.Instance.GetSession();
        //Iteminstance iIns =
        //    (Iteminstance)
        //    (sess.CreateCriteria(typeof(Iteminstance)).Add(Expression.Eq("Enginenumber", egnum)).List()[0]);
        //txtType.Text = iIns.Itemtype;
        //txtColor.Text = iIns.Color;
    }
    private void Dbcheck(string dbcode, string engineNo)
    {
        btnInsertWarrantyInfo.Enabled = EvalEditable(dbcode, engineNo);
        if (!btnInsertWarrantyInfo.Enabled)
        {
            //lbMes.Text = "";
            ShowMessage(Message.WarrantyInfo_DBCodePermission, true);
        }
    }
    protected static string EvalAddress(object cust)
    {
        if (cust != null)
        {
            return ServiceTools.GetCustAddress((Customer)cust);
        }
        else
        {
            return "";
        }
    }
    private void ShowMessage(string mesg, bool isError)
    {
        //lbMes.Visible = true;
        //lbMes.ControlStyle.ForeColor = (isError) ? Color.Red : Color.RoyalBlue;
        //lbMes.Text += mesg + "<br/>";
        if (isError) AddErrorMsg(mesg); else AddInfoMsg(mesg);
    }
    private void GUIResetInputPanel()
    {
        txtEngineNumberInput.ReadOnly = false; txtEngineNumberInput.CssClass = String.Empty;
        txtEngineNumberInput.Text = String.Empty;
        txtPurchaseDateInput.Text = String.Empty; txtPurchaseDateInput.CssClass = String.Empty;
        txtPurchaseDateInput.ReadOnly = false;
        txtKmCountInput.ReadOnly = false; txtKmCountInput.CssClass = String.Empty;
        txtKmCountInput.Text = "0"; txtKmCountInput.Enabled = !UserHelper.IsDealer;
        txtCustomerIDInput.ReadOnly = false; txtCustomerIDInput.CssClass = String.Empty;
        trDealer.Visible = !UserHelper.IsDealer;
        if (ddlDealerCode.Items.Count > 0) ddlDealerCode.SelectedIndex = 0;
        txtType.Text = string.Empty;
        txtColor.Text = string.Empty;
        txtCusNameInput.Text = string.Empty;
        txtBirthDateInput.Text = string.Empty;
        txtAddressInput.Text = string.Empty;
        txtPrecinctInput.Text = string.Empty;
        txtDistrictInput.Text = string.Empty;
        ddlProvinceInput.SelectedIndex = 0;
        btnInsertCus.Enabled = false;
        btnInsertWarrantyInfo.Enabled = false;
        btnFilterEng.Enabled = true;
        CusInfph.Visible = false;
    }
    private void InputGUIDisplay(bool isDisplay)
    {
        //GUI:Defaulpage
        plSearchContent.Visible = isDisplay;
        gvItem.Visible = isDisplay;
        plInputWInf.Visible = (isDisplay) ? false : true;
    }
    static private RangeValidator DateValid(RangeValidator rvDate, string from, string to)
    {
        rvDate.MinimumValue = from;
        rvDate.MaximumValue = to;
        rvDate.ErrorMessage = string.Format(Message.InvalidSoldDateRange, rvDate.MinimumValue, rvDate.MaximumValue);
        return rvDate;
    }
    protected void ddlDc_databound(object sender, EventArgs e)
    {
        foreach (ListItem li in ddlDealerCode.Items)
        {
            li.Text = li.Value + " - " + li.Text;
        }
    }
    private void getDealerCode(CurrentAction act)
    {
        if (!(ddlDealerCode.Items.Count > 0))
        {
            if (UserHelper.IsSysAdmin || (!VDMSSetting.CurrentSetting.CheckWarrantyInfoDatabase))
            {
                ddlDealerCode.DataSource = Dealer.GetListDealerALL();
            }
            else if (UserHelper.IsVMEPService && (act == CurrentAction.AddNew) && (VDMSSetting.CurrentSetting.AllowServiceAddNewWarrantyInfoForAllRegion))
            {
                ddlDealerCode.DataSource = Dealer.GetListDealerALL();
            }
            else
            {
                ddlDealerCode.DataSource = Dealer.GetListDealerByDatabase(UserHelper.DatabaseCode);
            }

            ddlDealerCode.DataTextField = "BranchName";
            ddlDealerCode.DataValueField = "BranchCode";
            ddlDealerCode.DataBind();

            // fix sell dealer as keyin dealer
            if (UserHelper.IsDealer) ddlDealerCode.SelectedValue = UserHelper.DealerCode;
        }
    }
    private void LoadAllProvince()
    {
        DataSet ds = Area.GetListProvince();

        ddlProvinceInput.DataSource = ds.Tables[0];
        ddlProvinceInput.DataValueField = "ProviceCode";
        ddlProvinceInput.DataTextField = "ProviceName";
        ddlProvinceInput.DataBind();
    }
    protected static bool EvalEditable(object databaseCode, object engineNo)
    {
        if (UserHelper.IsSysAdmin || (!VDMSSetting.CurrentSetting.CheckWarrantyInfoDatabase)) return true;
        if (UserHelper.IsVMEPService)
        {
            if (VDMSSetting.CurrentSetting.AllowServiceAddNewWarrantyInfoForAllRegion && !WarrantyInfoDAO.ItemInfoExist(engineNo.ToString())) return true;
        }
        return databaseCode.ToString() == UserHelper.DatabaseCode;
    }
    private void disableTextBox(TextBox tx)
    {
        tx.ReadOnly = true;
        tx.CssClass = "readOnlyInputField";
        tx.Text = tx.Text.Trim();
    }
    #endregion
}
