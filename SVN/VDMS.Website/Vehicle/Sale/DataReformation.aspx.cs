using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Helper;
using NHibernate;
using NHibernate.Expression;
using VDMS.I.Vehicle;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.TipTop;
using Resources;

public partial class Sales_Sale_DataReformation : BasePage
{
    protected string EvalGender(object male)
    {
        if (male == null) return "";
        return ((bool)male) ? Gender.Male : Gender.Female;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            rvBirthDate = DateValid(rvBirthDate);
            LoadAllProvince();
        }
        lblErr.Visible = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (SaveCustomer())
        {
            ShowMessage(Resources.Message.ActionSucessful, false);
            DisplayControlStatus("ADDNEW");
            Panel1.Visible = false;
            phCusList.Visible = true;
            _PageStatus.Value = "DEFAULT";
            gvCust.EditIndex = -1;
            gvCust.DataBind();
        }
        else
        {
            if (_PageStatus.Value.Equals("ADDNEW"))
                ShowMessage(Resources.Message.Cus_SaveErr, true);
            else
                ShowMessage(Resources.Constants.SaveDatabaseFail, true);
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        _PageStatus.Value = "ADDNEW";
        DisplayControlStatus("ADDNEW");
        Panel1.Visible = true;
        phCusList.Visible = false;
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        _PageStatus.Value = "UPDATE";
        DisplayControlStatus("UPDATE");
        bindDatatoComponent();
    }
    protected void btnTest_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(gvCust.DataSourceID)) gvCust.DataSourceID = odsCus.ID;
        else gvCust.DataBind();
        gvCust.SelectedIndex = -1;
        this.Panel1.Visible = false;
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        /*
        ISession sess = NHibernateSessionManager.Instance.GetSession();
        IList lstCus = sess.CreateCriteria(typeof(Customer))
            .Add(Expression.Eq("Identifynumber", txtIdentityNo.Text.Trim())).List();
        if (lstCus.Count > 0)
        {
            Customer delcus = lstCus[0] as Customer;
            sess.Delete(delcus);
        }
        */
    }
    private void DisplayControlStatus(string ActionCommand)
    {
        switch (ActionCommand)
        {
            case "ADDNEW":
                //txtIdentityNo.ReadOnly = false; txtIdentityNo.CssClass = null;//txtIdentityNo.Text = "";
                txtFullName.ReadOnly = false; txtFullName.Text = ""; txtFullName.CssClass = null;
                ddlCusType.Enabled = true; ddlCusType.SelectedIndex = 0;
                ddlCus_SetType.Enabled = true; ddlCus_SetType.SelectedIndex = 0;
                ddlProvince.Enabled = true; ddlProvince.SelectedIndex = 0;
                ddlSex.Enabled = true; ddlSex.SelectedIndex = 0;
                txtAddress.ReadOnly = false; txtAddress.Text = ""; txtAddress.CssClass = null;
                txtBirthDate.ReadOnly = false; txtBirthDate.Text = ""; txtBirthDate.CssClass = null;
                txtCEmail.ReadOnly = false; txtCEmail.Text = ""; txtCEmail.CssClass = null;
                txtCMobile.ReadOnly = false; txtCMobile.Text = ""; txtCMobile.CssClass = null;
                txtCPhone.ReadOnly = false; txtCPhone.Text = ""; txtCPhone.CssClass = null;
                txtCus_Desc.ReadOnly = false; txtCus_Desc.Text = ""; txtCus_Desc.CssClass = null;
                txtDistrict.ReadOnly = false; txtDistrict.Text = ""; txtDistrict.CssClass = null;
                txtPrecinct.ReadOnly = false; txtPrecinct.Text = ""; txtPrecinct.CssClass = null;
                tblCus_JobType.Enabled = true; tblCus_JobType.SelectedIndex = 0;
                txtModel.Text = ""; txtEngineNo.Text = "";
                btnSave.Visible = true;
                break;
            case "SEARCH":
                //txtIdentityNo.ReadOnly = false; txtIdentityNo.CssClass = null;
                txtFullName.ReadOnly = true; txtFullName.CssClass = "readOnlyInputField";
                ddlCusType.Enabled = false;
                ddlCus_SetType.Enabled = false;
                ddlProvince.Enabled = false;
                ddlSex.Enabled = false;
                txtAddress.ReadOnly = true; txtAddress.CssClass = "readOnlyInputField";
                txtBirthDate.ReadOnly = true; txtBirthDate.CssClass = "readOnlyInputField";
                txtCEmail.ReadOnly = true; txtCEmail.CssClass = "readOnlyInputField";
                txtCMobile.ReadOnly = true; txtCMobile.CssClass = "readOnlyInputField";
                txtCPhone.ReadOnly = true; txtCPhone.CssClass = "readOnlyInputField";
                txtCus_Desc.ReadOnly = true; txtCus_Desc.CssClass = "readOnlyInputField";
                txtDistrict.ReadOnly = true; txtDistrict.CssClass = "readOnlyInputField";
                txtPrecinct.ReadOnly = true; txtPrecinct.CssClass = "readOnlyInputField";
                tblCus_JobType.Enabled = false;
                btnSave.Visible = false;
                break;
            case "UPDATE":
                //txtIdentityNo.ReadOnly = true; txtIdentityNo.CssClass = "readOnlyInputField";
                txtFullName.ReadOnly = false; txtFullName.CssClass = null;
                ddlCusType.Enabled = true;
                ddlCus_SetType.Enabled = true;
                ddlProvince.Enabled = true;
                ddlSex.Enabled = true;
                txtAddress.ReadOnly = false; txtAddress.CssClass = null;
                txtBirthDate.ReadOnly = false; txtBirthDate.CssClass = null;
                txtCEmail.ReadOnly = false; txtCEmail.CssClass = null;
                txtCMobile.ReadOnly = false; txtCMobile.CssClass = null;
                txtCPhone.ReadOnly = false; txtCPhone.CssClass = null;
                txtCus_Desc.ReadOnly = false; txtCus_Desc.CssClass = null;
                txtDistrict.ReadOnly = false; txtDistrict.CssClass = null;
                txtPrecinct.ReadOnly = false; txtPrecinct.CssClass = null;
                tblCus_JobType.Enabled = true;
                btnSave.Visible = true;
                break;
            default: break;
        }
    }
    private void LoadAllProvince()
    {
        DataSet ds = Area.GetListProvince();

        ddlProvince.DataSource = ds.Tables[0];
        ddlProvince.DataValueField = "ProviceCode";
        ddlProvince.DataTextField = "ProviceName";
        ddlProvince.DataBind();
    }
    private Invoice LoadInvoiceOfCustomer(Customer cusIns)
    {
        ISession sess = NHibernateSessionManager.Instance.GetSession();
        IList lstInvoice = sess.CreateCriteria(typeof(Invoice)).Add(Expression.Eq("Customer", cusIns)).List();
        if (lstInvoice.Count > 0)
        {
            return lstInvoice[0] as Invoice;
        }
        else return null;
    }

    private void bindDatatoComponent()
    {
        ISession sess = NHibernateSessionManager.Instance.GetSession();
        var query = sess.CreateCriteria(typeof(Customer))
            .Add(Expression.Eq("Id", gvCust.SelectedValue));

        if (UserHelper.IsDealer) query.Add(Expression.Eq("Dealercode", UserHelper.DealerCode));

        IList lstCus = query.List();
        if (lstCus.Count > 0)
        {
            Customer scb = (Customer)lstCus[0];
            this.Panel1.Visible = true;
            
            _CustomerID.Value = scb.Id.ToString();

            txtFullName.Text = scb.Fullname;
            if (!scb.Birthdate.Equals(DateTime.MinValue))
            {
                txtBirthDate.Text = scb.Birthdate.ToShortDateString();
            }
            txtAddress.Text = scb.Address;
            if (scb.Email != null) txtCEmail.Text = scb.Email;
            if (scb.Mobile != null) txtCMobile.Text = scb.Mobile;
            txtCPhone.Text = scb.Tel;
            txtCus_Desc.Text = scb.Customerdescription;
            ddlCusType.SelectedValue = scb.Customertype.ToString();
            if (scb.Priority.ToString() == "")
            {
                ddlCus_SetType.SelectedValue = "1";
            }
            else
            {
                ddlCus_SetType.SelectedValue = scb.Priority.ToString();
            }

            ddlSex.SelectedValue = (scb.Gender) ? "1" : "0";
            tblCus_JobType.SelectedValue = scb.Jobtypeid.ToString();

            txtPrecinct.Text = scb.Precinct;
            txtDistrict.Text = scb.Districtid;
            try
            {
                ddlProvince.SelectedValue = scb.Provinceid;
            }
            catch (Exception) { }

            Invoice invIns = LoadInvoiceOfCustomer(scb);
            if (invIns != null)
            {
                txtModel.Text = invIns.Iteminstance.Itemtype;
                txtEngineNo.Text = invIns.Enginenumber;
            }
            else { txtModel.Text = ""; txtEngineNo.Text = ""; }
        }
        else
        {
            //txtIdentityNo.CssClass = null; txtIdentityNo.ReadOnly = false;
            lblErr.Visible = true;
            lblErr.Text = Resources.Message.Cus_NoCustomer;
            Panel1.Visible = false;
        }
    }
    private bool SaveCustomer()
    {
        long id;
        long.TryParse(_CustomerID.Value, out id);
        string ActionString = "DEFAULT";
        ISession sess = NHibernateSessionManager.Instance.GetSession();
        IList lstCus = sess.CreateCriteria(typeof(Customer))
            .Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
            .Add(Expression.Eq("Id", id)).List();
        //.Add(Expression.Eq("Identifynumber", txtIdentityNo.Text.Trim().ToUpper())).List();
        if (lstCus.Count > 0)
        {
            if (_PageStatus.Value.Equals("ADDNEW"))
            {
                return false;
            }
            ActionString = "UPDATECUSTOMER";
        }
        string idnum = "";// txtIdentityNo.Text.Trim().ToUpper();
        string fullname = txtFullName.Text.Trim();
        bool gender = (ddlSex.SelectedValue.Trim().Equals("1")) ? true : false;

        DateTime dt;
        DateTime.TryParse(txtBirthDate.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out dt);
        DateTime birthdate = dt;

        string address = txtAddress.Text.Trim();
        string provinceid = ddlProvince.Text.ToString().Trim();
        string districtid = txtDistrict.Text.Trim();
        int jobtype = int.Parse(tblCus_JobType.SelectedValue.ToString());
        string email = txtCEmail.Text.Trim();
        string tel = txtCPhone.Text.Trim();
        string mobile = txtCMobile.Text.Trim();
        int priority = int.Parse(ddlCus_SetType.SelectedValue);
        string customertype = ddlCusType.SelectedValue;
        string cusdesc;
        if (txtCus_Desc.Text.Trim().Length > 2000)
        {
            cusdesc = txtCus_Desc.Text.Trim().Substring(0, 2000).Trim();
        }
        else cusdesc = txtCus_Desc.Text.Trim();
        string precinct = txtPrecinct.Text.Trim();

        //Save Customer
        ITransaction tx = sess.BeginTransaction();
        try
        {
            if (_PageStatus.Value.Equals("ADDNEW") && !ActionString.Equals("UPDATECUSTOMER"))
            {
                id = CustomerHelper.SaveCustomer(ref sess, idnum, fullname, gender, birthdate, address, provinceid, districtid, jobtype, email, tel, mobile, priority, customertype, cusdesc, precinct, UserHelper.DealerCode, false);
                _CustomerID.Value = id.ToString();
            }
            else
                CustomerHelper.UpdateCustomer(ref sess, id, idnum, fullname, gender, birthdate, address, provinceid, districtid, jobtype, email, tel, mobile, priority, customertype, cusdesc, precinct, UserHelper.DealerCode, false);
            tx.Commit();
        }
        catch (Exception)
        {
            tx.Rollback();
        }
        return tx.WasCommitted;
    }
    private void ShowMessage(string mesg, bool isError)
    {
        lblErr.Visible = true;
        lblErr.ControlStyle.ForeColor = (isError) ? Color.Red : Color.RoyalBlue;
        lblErr.Text = mesg;
    }
    static private RangeValidator DateValid(RangeValidator rvDate)
    {
        rvDate.MinimumValue = DateTime.MinValue.ToShortDateString();
        rvDate.MaximumValue = DateTime.Now.ToShortDateString();
        return rvDate;
    }

    protected void gvCust_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DisplayControlStatus("SEARCH");
        //bindDatatoComponent();
        _PageStatus.Value = "UPDATE";
        DisplayControlStatus("UPDATE");
        bindDatatoComponent();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        gvCust.SelectedIndex = -1;
        phCusList.Visible = true;
        Panel1.Visible = false;
        _PageStatus.Value = "DEFAULT";
    }
    protected void gvCust_PageIndexChanged(object sender, EventArgs e)
    {
        Panel1.Visible = false;
        gvCust.EditIndex = -1;
    }
}
