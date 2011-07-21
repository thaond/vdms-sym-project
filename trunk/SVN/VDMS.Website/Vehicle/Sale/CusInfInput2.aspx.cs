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

public partial class Vehicle_Sale_CusInfInput2 : BasePage
{
	string Cus_NumberId = "";
	protected string EmailEmpty;
	protected string EmailFormatErr;
	protected string CusIdNumEmpty;
	protected string CusFullnameEmpty;
	protected string CusMobileEmpty;
	protected string CusMobileNumbeFormatInvalid;
	protected string CusPhoneNumberFormatInvalid;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			CusIdNumEmpty = Resources.Customers.CusIdNumEmpty;
			EmailEmpty = Resources.Customers.EmailEmpty;
			EmailFormatErr = Resources.Customers.EmailFormatErr;
			CusFullnameEmpty = Resources.Customers.CusFullnameEmpty;
			CusMobileEmpty = Resources.Customers.CusMobileEmpty;
			CusMobileNumbeFormatInvalid = Resources.Customers.CusMobileNumbeFormatInvalid;
			CusPhoneNumberFormatInvalid = Resources.Customers.CusPhoneNumberFormatInvalid;
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
				if (Page.Request.QueryString["IsWarranty"] != null)
				{
					ddlCusType.SelectedIndex = 1;
				}
			}
			catch (NullReferenceException)
			{
				Page.Response.Redirect("Customer.aspx");
			}
			BindData();
		}
	}

	private void BindData()
	{
		if (Page.Request.QueryString["action"].Trim().ToUpper().Equals("edit".ToUpper()))
		{
            long id;
            long.TryParse(Page.Request.QueryString["cusid"], out id);
            ISession sess = NHibernateSessionManager.Instance.GetSession();// NHibernateHelper.GetCurrentSession();
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

			tran.Commit();
			NHibernateSessionManager.Instance.CloseSession();
			//NHibernateHelper.CloseSession();
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
}