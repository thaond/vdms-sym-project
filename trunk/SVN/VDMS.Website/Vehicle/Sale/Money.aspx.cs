using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Helper;
using NHibernate;
using NHibernate.Expression;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.I.Vehicle;

public partial class Sales_Sale_Money : BasePage
{
	string _UpdateDATA = "UPDATEDATA";
	decimal PriceIncludeTax;
	protected string DeleteAlert;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			DateRange.MinimumValue = DateTime.MinValue.ToShortDateString();
			DateRange.MaximumValue = DateTime.MaxValue.ToShortDateString();
		}
		DeleteAlert = Resources.Question.DeleteData;
		lbErr.Visible = false;
	}

	protected void gvMoneyHistory_DataBound(object sender, EventArgs e)
	{
		try
		{
			decimal PriceSum = PriceIncludeTax;
			foreach (GridViewRow grv in gvMoneyHistory.Rows)
			{
				Literal liIndexLocal = (Literal)grv.FindControl("liIndex");
				liIndexLocal.Text = (grv.RowIndex + 1).ToString();
				Literal liPayDateLocal = (Literal)grv.FindControl("liPayDate");
				DateTime dt = DateTime.Parse(liPayDateLocal.Text); liPayDateLocal.Text = dt.ToShortDateString();

				Literal liPayMethodLocal = (Literal)grv.FindControl("liPayMethod");
				liPayMethodLocal.Text = returnPayMethod(liPayMethodLocal.Text);
				Literal liPaidMoneyLocal = (Literal)grv.FindControl("liPaidMoney");

				Literal liRestMoneyLocal = (Literal)grv.FindControl("liRestMoney");
				decimal PaidMoneyDecimal = decimal.Parse(liPaidMoneyLocal.Text);
				liRestMoneyLocal.Text = Convert2Currency((PriceSum - PaidMoneyDecimal).ToString());
				PriceSum = PriceSum - PaidMoneyDecimal;
				liPaidMoneyLocal.Text = Convert2Currency(liPaidMoneyLocal.Text);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	protected void btnDel_Click(object sender, EventArgs e)
	{
		ISession sess = NHibernateSessionManager.Instance.GetSession();
		ITransaction tx = sess.BeginTransaction();
		try
		{
			/* Using Customer to Del
			Customer cus = sess.CreateCriteria(typeof(Customer))
				.Add(Expression.Eq("Dealercode",UserHelper.DealerCode))
				.Add(Expression.Eq("Identifynumber", _CurrentCustomer.Value)).List()[0] as Customer;
			Invoice invIns = sess.CreateCriteria(typeof(Invoice))
				.Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
				.Add(Expression.Eq("Customer", cus))
				.AddOrder(Order.Desc("Createddate")).List()[0] as Invoice;
			*/

			Invoice invIns = sess.CreateCriteria(typeof(Invoice))
				.Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
				.Add(Expression.Eq("Enginenumber", _EngineNo.Value))
				.List()[0] as Invoice;

			IList lstPayment = sess.CreateCriteria(typeof(Payment)).Add(Expression.Eq("Sellitem", invIns.Sellitem)).List();
			foreach (Payment pm in lstPayment)
			{
				pm.Status = (int)CusPaymentStatus.NonPay;
				sess.Update(pm);
			}
			BindData(ref sess);
			btnDel.Visible = false;
			tx.Commit();
			CloseNextTransaction(false);
		}
		catch (Exception ex)
		{
			tx.Rollback();
			lbErr.Visible = true;
			lbErr.Text = ex.Message;
			//lbErr.Text = "Database Error!!!";
		}
	}

	protected void btnApply_Click(object sender, EventArgs e)
	{
		CloseNextTransaction(false);
		ISession sess = NHibernateSessionManager.Instance.GetSession();
		//Get current payment
		Payment pmCurr = sess.CreateCriteria(typeof(Payment)).Add(Expression.Eq("Id", decimal.Parse(ddlDateMoney.Items[0].Value))).List()[0] as Payment;

		if (tbtnMTransfer.Checked)
		{
			DateTime oDate;
			DateTime.TryParse(txtTransferDate.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out oDate);
			if (!DateTime.MinValue.Equals(oDate)) pmCurr.Transferdate = oDate;
			pmCurr.Status = (int)CusPaymentStatus.PayTransfer;
			pmCurr.Bankaccount = txtBankAcc.Text.Trim();
		}
		else pmCurr.Status = (int)CusPaymentStatus.PayCash;
		pmCurr.Commentpayment = txtComment.Text.Trim();
		txtComment.Text = "";
		BindData(ref sess);
	}

	protected void btnTest_Click(object sender, EventArgs e)
	{
		CloseNextTransaction(false);
		ddlDateMoney.Items.Clear();
		_EngineNo.Value = txtEngineNo.Text.Trim().ToUpper();
		_CurrentCustomer.Value = txtCustomerID.Text.Trim();
		ISession sess = NHibernateSessionManager.Instance.GetSession();
		BindData(ref sess);
	}

	private void BindData(ref ISession sess)
	{
		ddlDateMoney.Enabled = true;
		string _CusID = "-1"; IList lstInv;
		sess = NHibernateSessionManager.Instance.GetSession();

		if (_EngineNo.Value != "")
		{
			lstInv = sess.CreateCriteria(typeof(Invoice))
				.Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
				.Add(Expression.Eq("Enginenumber", _EngineNo.Value))
				.AddOrder(NHibernate.Expression.Order.Desc("Createddate"))
				.List();
		}
		else
		{
			if (txtCustomerID.Text.Trim() == "")
			{
				ShowMessage(Resources.Customers.Money_EmptyField, true);
				return;
			}
			_CusID = ((_CurrentCustomer.Value.Trim() != "") && (_CurrentCustomer.Value != "-1")) ? _CurrentCustomer.Value : txtCustomerID.Text.Trim();
			IList lstCus = sess.CreateCriteria(typeof(Customer))
				.Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
				.Add(Expression.Eq("Identifynumber", _CusID)).List();
			Customer cus = new Customer(); cus.Id = -1;
			if (lstCus.Count > 0)
			{
				cus = lstCus[0] as Customer;
			}
			lstInv = sess.CreateCriteria(typeof(Invoice)).Add(Expression.Eq("Customer", cus))
				.AddOrder(NHibernate.Expression.Order.Desc("Createddate")).List();
		}
		if (lstInv.Count > 0)
		{
			Invoice invIns = lstInv[0] as Invoice;
			txtCustomerID.Text = invIns.Customer.Identifynumber;
			txtEngineNo.Text = invIns.Enginenumber;
			lblBillNo.Text = invIns.Invoicenumber;
			lblCustomer.Text = invIns.Customer.Fullname;
			lblAgentName.Text = DealerHelper.GetName(invIns.Dealercode);
			lblMoneySale.Text = Convert2Currency(invIns.Sellitem.Pricebeforetax.ToString());
			PriceIncludeTax = invIns.Sellitem.Pricebeforetax;
			decimal SurplusMoney = 0;
			//lblSurplusMoney.Text = invIns.Sellitem.Pricebeforetax -

			if (invIns.Sellitem.Paymenttype > 0)
			{
				//Bind paid item
				IList lstPayment = sess.CreateCriteria(typeof(Payment))
					.Add(Expression.Eq("Sellitem", invIns.Sellitem))
					.Add(Expression.Gt("Status", (int)CusPaymentStatus.NonPay))
					.AddOrder(NHibernate.Expression.Order.Asc("Paymentdate"))
					.List();
				btnDel.Visible = (lstPayment.Count > 0) ? true : false;
				gvMoneyHistory.DataSource = lstPayment;
				gvMoneyHistory.DataBind();

				lstPayment = sess.CreateCriteria(typeof(Payment))
					.Add(Expression.Eq("Sellitem", invIns.Sellitem))
					.Add(Expression.Eq("Status", (int)CusPaymentStatus.NonPay))
					.AddOrder(NHibernate.Expression.Order.Asc("Paymentdate"))
					.List();
				if (lstPayment.Count > 0)
				{
					ddlDateMoney.Items.Clear();
					//Bind not-paid item
					foreach (Payment pm in lstPayment)
					{
						ListItem li = new ListItem();
						li.Text = pm.Paymentdate.ToShortDateString();
						li.Value = pm.Id.ToString();
						ddlDateMoney.Items.Add(li);
						SurplusMoney += pm.Amount;
					}
					//Bind first item
					Payment pm1 = lstPayment[0] as Payment;
					txtCurrentMoney.Text = Convert2Currency(pm1.Amount.ToString());
				}
				else
				{
					CloseNextTransaction(true);
				}
			}
			else
			{
				lbErr.Visible = true;
				lbErr.Text = Resources.Message.Payment_NotFound;
				Panel1.Visible = false;
				return;
			}
			lblSurplusMoney.Text = Convert2Currency(SurplusMoney.ToString());
			_CurrentCustomer.Value = txtCustomerID.Text.Trim();
			_EngineNo.Value = invIns.Enginenumber;
			Panel1.Visible = true;
		}
		else
		{
			lbErr.Visible = true;
			if (_EngineNo.Value != "")
			{
				IList lstBatchInvoice = sess.CreateCriteria(typeof(Batchinvoicedetail)).Add(Expression.Eq("Enginenumber", _EngineNo.Value)).List();
				if (lstBatchInvoice.Count > 0)
				{
					lbErr.Text = Resources.Message.EngineNoBatchInvoiceFound;
				}
				else
					lbErr.Text = Resources.Message.NotFoundInDB;
			}
			else lbErr.Text = Resources.Message.Cus_HaveNotBuyItem;
			Panel1.Visible = false;
		}
	}
	protected string returnPayMethod(string paymethod)
	{
		string PayMethodString;
		if ((int.Parse(paymethod) == 1))
		{
			PayMethodString = Resources.Constants.PaymentMethod_Cash;
		}
		else
		{
			PayMethodString = Resources.Constants.PaymentMethod_Transfer;
		}
		return PayMethodString;
	}
	private void CloseNextTransaction(bool isState)
	{
		if (isState)
		{
			ddlDateMoney.Enabled = false;
			tbtMCash.Enabled = false;
			tbtnMTransfer.Enabled = false;
			txtTransferDate.Enabled = false; txtTransferDate.Text = "";
			CE.Enabled = false;
			txtBankAcc.Enabled = false; txtBankAcc.Text = "";
			txtComment.Enabled = false; txtComment.Text = "";
			btnApply.Enabled = false;
			txtCurrentMoney.Text = "";
		}
		else
		{
			ddlDateMoney.Enabled = true;
			tbtMCash.Enabled = true;
			tbtnMTransfer.Enabled = true;
			txtTransferDate.Enabled = true;
			CE.Enabled = true;
			txtBankAcc.Enabled = true;
			txtComment.Enabled = true;
			btnApply.Enabled = true;
		}
	}
	private void ShowMessage(string ErrMsg, bool isError)
	{
		lbErr.Visible = true;
		lbErr.ControlStyle.ForeColor = (isError) ? Color.Red : Color.RoyalBlue;
		lbErr.Text = ErrMsg;
	}
	protected string Convert2Currency(string ObConvert)
	{
		decimal ICurency = decimal.Parse(ObConvert);
		NumberFormatInfo nfi = Thread.CurrentThread.CurrentCulture.NumberFormat;
		nfi.NumberDecimalDigits = 0;
		return ICurency.ToString("N", nfi);
	}
	protected void gvMoneyHistory_RowEditing(object sender, GridViewEditEventArgs e)
	{
		CloseNextTransaction(false);
		_PageStatus.Value = _UpdateDATA;
		//Bind Data
		Literal liID = (Literal)gvMoneyHistory.Rows[e.NewEditIndex].FindControl("liPaymentID");
		ISession sess = NHibernateSessionManager.Instance.GetSession();
		Payment pm = sess.CreateCriteria(typeof(Payment)).Add(Expression.Eq("Id", decimal.Parse(liID.Text))).List()[0] as Payment;

		ListItem li = new ListItem();
		li.Text = pm.Paymentdate.ToShortDateString();
		li.Value = pm.Id.ToString();

		ddlDateMoney.Items.Clear(); ddlDateMoney.Items.Add(li); ddlDateMoney.Enabled = false;
		txtCurrentMoney.Text = Convert2Currency(pm.Amount.ToString());
		if (pm.Status.Equals((int)CusPaymentStatus.PayTransfer))
		{
			tbtnMTransfer.Checked = true;
			txtTransferDate.Text = (pm.Transferdate.Equals(DateTime.MinValue)) ? "" : pm.Transferdate.ToShortDateString();
			txtBankAcc.Text = (pm.Bankaccount != null) ? pm.Bankaccount : "";
		}
		else tbtMCash.Checked = true;

		txtComment.Text = pm.Commentpayment;
	}
}