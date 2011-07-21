using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using VDMS.Helper;
using NHibernate;
using NHibernate.Expression;
using VDMS.Core.Domain;
using VDMS.Data.DAL;
using VDMS.Data.TipTop;
using VDMS.Data.DAL.NHibernateDAL;
using Resources;
using System.Globalization;
using VDMS.I.Service;
using VDMS.I.Vehicle;

public partial class Vehicle_Sale_CusInfInput : BasePage
{
    string Cus_NumberId = "";
    protected string EmailEmpty;
    protected string EmailFormatErr;
    protected string CusIdNumEmpty;
    protected string CusFullnameEmpty;
    protected string CusMobileEmpty;
    protected string CusMobileNumbeFormatInvalid;
    protected string CusPhoneNumberFormatInvalid;
    protected string NeedOnePhoneNumber;
    protected string AddressEmpty;

    public bool RequireSex
    {
        get
        {
            return imgSex.Visible;
        }
        set
        {
            imgSex.Visible = value;
            imgSexNon.Visible = !value;
        }
    }
    public bool RequireAddress
    {
        get
        {
            return imgAddress.Visible;
        }
        set
        {
            imgAddress.Visible = value;
            imgAddressNon.Visible = !value;
        }
    }

    private string Sessionkey { get { return Request.QueryString["sessionkey"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (string.IsNullOrEmpty(Sessionkey))
            {
                btnSave.OnClientClick = "send_back_data(); return false;";
                btnSave.UseSubmitBehavior = false;
            }

            InitErrMsgControl(errMsg);
            CusIdNumEmpty = Resources.Customers.CusIdNumEmpty;
            EmailEmpty = Resources.Customers.EmailEmpty;
            EmailFormatErr = Resources.Customers.EmailFormatErr;
            CusFullnameEmpty = Resources.Customers.CusFullnameEmpty;
            CusMobileEmpty = Resources.Customers.CusMobileEmpty;
            CusMobileNumbeFormatInvalid = Resources.Customers.CusMobileNumbeFormatInvalid;
            CusPhoneNumberFormatInvalid = Resources.Customers.CusPhoneNumberFormatInvalid;
            NeedOnePhoneNumber = Resources.Customers.NeedOnePhoneNumber;
            AddressEmpty = Resources.Customers.AddressEmpty;

            // change required fields
            bool onWarr = (Page.Request.QueryString["IsWarranty"] != null) && (Page.Request.QueryString["IsWarranty"] == "true");
            RequireSex = RequireAddress = !onWarr;

            //Check Call Edit Message
            if (ExistsEditMessage())
            {
                Cus_NumberId = Page.Request.QueryString["action"];
            }
            LoadAllProvince();
            try
            {
                txtEngineNo.Text = Page.Request.QueryString["engineno"];
                txtModel.Text = Page.Request.QueryString["motortype"];
                //txtCustomerID.Text = Page.Request.QueryString["cusid"].Trim();
                if (onWarr)
                {
                    ddlCusType.SelectedIndex = 1;
                }
            }
            catch (NullReferenceException)
            {
                Page.Response.Redirect("Customer.aspx");
            }
            ShowWarning();
            BindData();
        }
    }

    private void ShowWarning()
    {
        string SellDealer = Request.QueryString["SellD"];
        if (!string.IsNullOrEmpty(SellDealer) && (SellDealer != UserHelper.DealerCode))
            AddErrorMsg(Message.WarrantyContent_ItemSoldByOtherDealer +
                        ((SrsSetting.showSourceDealerWhenAlarm && !UserHelper.IsDealer) ?
                        "  (" +
                            ((string.IsNullOrEmpty(SellDealer)) ? Message.DataNotFound :
                            SellDealer + " : " + DealerHelper.GetName(SellDealer)) + ")" :
                        ""));
    }

    private void BindData()
    {
        if (Page.Request.QueryString["action"].Trim().ToUpper().Equals("edit".ToUpper()))
        {
            if (!string.IsNullOrEmpty(Sessionkey))
            {
                txtCustomerID.Text = SaleVehicleHelper.CommonSaleData.IdentifyNumber.ToString();
                txtFullName.Text = SaleVehicleHelper.CommonSaleData.CustomerName;
                ddlCus_SetType.SelectedValue = SaleVehicleHelper.CommonSaleData.Priority.ToString();
                ddlCusType.SelectedValue = SaleVehicleHelper.CommonSaleData.CustomerType.ToString();
                txtBirthDate.Text = SaleVehicleHelper.CommonSaleData.DOB == DateTime.MinValue ? "" : SaleVehicleHelper.CommonSaleData.DOB.ToShortDateString();
                ddlSex.SelectedValue = SaleVehicleHelper.CommonSaleData.Gender.ToString();
                txtCPhone.Text = SaleVehicleHelper.CommonSaleData.Tel;
                txtCMobile.Text = SaleVehicleHelper.CommonSaleData.Mobile;
                txtAddress.Text = SaleVehicleHelper.CommonSaleData.Address;
                txtPrecinct.Text = SaleVehicleHelper.CommonSaleData.Precint;
                txtDistrict.Text = SaleVehicleHelper.CommonSaleData.District;
                ddlProvince.SelectedValue = SaleVehicleHelper.CommonSaleData.Province;
                txtCEmail.Text = SaleVehicleHelper.CommonSaleData.Email;
                tblCus_JobType.SelectedValue = SaleVehicleHelper.CommonSaleData.JobType.ToString();
                txtCus_Desc.Text = SaleVehicleHelper.CommonSaleData.CustomerDescription;
            }

            long id;
            long.TryParse(Page.Request.QueryString["cusid"], out id);
            ISession sess = NHibernateSessionManager.Instance.GetSession();
            ITransaction tran = sess.BeginTransaction();

            IList lstCus = sess.CreateCriteria(typeof(Customer))
                .Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
                //.Add(Expression.Eq("Identifynumber", Page.Request.QueryString["cusid"].Trim())).List();
                .Add(Expression.Eq("Id", id)).List();
            if (lstCus.Count > 0)
            {
                PresentControl();
                Customer cusIns = lstCus[0] as Customer;
                txtCustomerID.Text = cusIns.Identifynumber;
                txtFullName.Text = cusIns.Fullname;
                ddlCus_SetType.SelectedValue = cusIns.Priority.ToString();
                ddlCusType.SelectedValue = cusIns.Customertype.ToString();
                txtBirthDate.Text = (cusIns.Birthdate.Equals(DateTime.MinValue)) ? "" : cusIns.Birthdate.ToShortDateString();
                ddlSex.SelectedValue = ((cusIns.Gender) ? 1 : 0).ToString();
                txtCPhone.Text = cusIns.Tel;
                txtCMobile.Text = cusIns.Mobile;
                txtAddress.Text = cusIns.Address;
                txtPrecinct.Text = cusIns.Precinct;
                txtDistrict.Text = cusIns.Districtid;
                ddlProvince.SelectedValue = cusIns.Provinceid;
                txtCEmail.Text = cusIns.Email;
                tblCus_JobType.SelectedValue = cusIns.Jobtypeid.ToString();
                txtCus_Desc.Text = cusIns.Customerdescription;
            }
            else
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["engineno"]))
                {
                    IList lstInvoiceIns = sess.CreateCriteria(typeof(Invoice))
                    .Add(Expression.Eq("Enginenumber", Page.Request.QueryString["engineno"].Trim())).List();
                    if (lstInvoiceIns.Count > 0)
                    {
                        PresentControl();
                        Invoice invoiceIns = lstInvoiceIns[0] as Invoice;

                        txtCustomerID.Text = invoiceIns.Customer.Identifynumber;
                        txtFullName.Text = invoiceIns.Customer.Fullname;
                        ddlCus_SetType.SelectedValue = invoiceIns.Customer.Priority.ToString();
                        ddlCusType.SelectedValue = invoiceIns.Customer.Customertype.ToString();
                        txtBirthDate.Text = invoiceIns.Customer.Birthdate.ToShortDateString();
                        ddlSex.SelectedValue = ((invoiceIns.Customer.Gender) ? 1 : 0).ToString();
                        txtCPhone.Text = invoiceIns.Customer.Tel;
                        txtCMobile.Text = invoiceIns.Customer.Mobile;
                        txtAddress.Text = invoiceIns.Customer.Address;
                        txtPrecinct.Text = invoiceIns.Customer.Precinct;
                        txtDistrict.Text = invoiceIns.Customer.Districtid;
                        ddlProvince.SelectedValue = invoiceIns.Customer.Provinceid;
                        txtCEmail.Text = invoiceIns.Customer.Email;
                        tblCus_JobType.SelectedValue = invoiceIns.Customer.Jobtypeid.ToString();
                        txtCus_Desc.Text = invoiceIns.Customer.Customerdescription;
                    }
                }
            }

            tran.Commit();
            //NHibernateHelper.CloseSession();
            NHibernateSessionManager.Instance.CloseSession();
        }
    }

    private void PresentControl()
    {
        //txtCustomerID.Enabled = false;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

    }

    private void LoadAllProvince()
    {
        DataSet ds = Area.GetListProvince();

        ddlProvince.DataSource = ds.Tables[0];
        ddlProvince.DataValueField = "ProviceCode";
        ddlProvince.DataTextField = "ProviceName";
        ddlProvince.DataBind();
    }

    private bool ExistsEditMessage()
    {
        bool IsExistsEditMessage = false;
        //Check Exists
        if (Page.Request.QueryString["action"] != null)
        {
            //Check Value Valid
            if ((Page.Request.QueryString["action"] != ""))
            {
                try
                {
                    if (Page.Request.QueryString["action"].Equals("insert"))
                    {
                        IsExistsEditMessage = false;
                    }
                    else
                    {
                        IsExistsEditMessage = true;
                    }
                }
                catch (Exception)
                {
                    //Write down that Cus_NumberID not true
                }
            }
        }
        return IsExistsEditMessage;
    }

    private void DisplayDonePage()
    {
        //Display page when insert data done
        lblErr.Visible = false;
        liDone.Visible = true;
        btnDispose.Visible = true;
    }
    protected void btnSave_Click1(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Sessionkey))
        {
            DateTime dob;

            SaleVehicleHelper.InitSale(Sessionkey);
            SaleVehicleHelper.CommonSaleData.CustomerId = 0;
            SaleVehicleHelper.CommonSaleData.CustomerName = txtFullName.Text;
            SaleVehicleHelper.CommonSaleData.Address = txtAddress.Text;
            SaleVehicleHelper.CommonSaleData.DOB = DateTime.TryParse(txtBirthDate.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out dob) ? dob : DateTime.MinValue;
            SaleVehicleHelper.CommonSaleData.CustomerDescription = txtCus_Desc.Text;
            SaleVehicleHelper.CommonSaleData.District = txtDistrict.Text;
            SaleVehicleHelper.CommonSaleData.Email = txtCEmail.Text;
            SaleVehicleHelper.CommonSaleData.Gender = int.Parse(ddlSex.SelectedValue);
            SaleVehicleHelper.CommonSaleData.IdentifyNumber = txtCustomerID.Text;
            SaleVehicleHelper.CommonSaleData.CustomerType = int.Parse(ddlCusType.SelectedValue);
            SaleVehicleHelper.CommonSaleData.JobType= int.Parse(tblCus_JobType.SelectedValue);
            SaleVehicleHelper.CommonSaleData.Mobile = txtCMobile.Text;
            SaleVehicleHelper.CommonSaleData.Precint= txtPrecinct.Text;
            SaleVehicleHelper.CommonSaleData.Priority = int.Parse(ddlCus_SetType.SelectedValue);
            SaleVehicleHelper.CommonSaleData.Province = ddlProvince.SelectedValue;
            SaleVehicleHelper.CommonSaleData.Tel = txtCPhone.Text;

            SaleVehicleHelper.SaveCommonSaleInfo();
            if (!ClientScript.IsStartupScriptRegistered("close"))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "close", "close_dialog();", true);
            }
        }
    }
}