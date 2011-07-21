using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Helper;
using Resources;
using VDMS.Data.DAL2;

public partial class Sales_Sale_Receipt : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			txtReceiptDate.Text = DateTime.Now.ToString();
		}
		lbErr.Visible = false;
		rvReceiptDate = DateValid(rvReceiptDate);
	}
	private RangeValidator DateValid(RangeValidator rvDate)
	{
		rvDate.MinimumValue = DateTime.MinValue.ToShortDateString();
		rvDate.MaximumValue = DateTime.MaxValue.ToShortDateString();
		return rvDate;
	}
	protected void btnTest_Click(object sender, EventArgs e)
	{
		string Identify = txtIdentity.Text.Trim();
		DateTime ReceiptDate;
		DateTime.TryParse(txtReceiptDate.Text.Trim(), new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out ReceiptDate);
		if (ReceiptDate.Equals(DateTime.MinValue))
		{
			ShowMessage(Constants.DateInvalid, true);
			DisplayControl(false);
			return;
		}
		else
			bindData(Identify, ReceiptDate);
	}

	protected string Convert2Currency(string ObConvert)
	{
		decimal ICurency = decimal.Parse(ObConvert);
		NumberFormatInfo nfi = Thread.CurrentThread.CurrentCulture.NumberFormat;
		nfi.NumberDecimalDigits = 0;
		return ICurency.ToString("N", nfi);
	}
	protected void gvItems_DataBound(object sender, EventArgs e)
	{
		if (gvItems.TopPagerRow == null) return;
		Literal litPageInfo = gvItems.TopPagerRow.FindControl("litPageInfo") as Literal;
		if (litPageInfo != null) litPageInfo.Text = string.Format(Resources.Message.PagingInfo, gvItems.PageIndex + 1, gvItems.PageCount, HttpContext.Current.Items["PaymentCount"]);

		DateTime PaymentDate, PrevousDate;
		Literal liPaymentDate, liPrevousDate, liRestOfMoney, liRecOfMoney, liPriceBeforeTax, liHirePurchase;
		foreach (GridViewRow gvr in gvItems.Rows)
		{
			liPaymentDate = (Literal)gvr.FindControl("liPaymentdate");
			liPrevousDate = (Literal)gvr.FindControl("liPrevousDate");
			liPriceBeforeTax = (Literal)gvr.FindControl("liPriceBeforeTax"); liPriceBeforeTax.Text = (liPriceBeforeTax.Text.Trim().Length == 0) ? "" : Convert2Currency(liPriceBeforeTax.Text);
			liRecOfMoney = (Literal)gvr.FindControl("liRecOfMoney"); liRecOfMoney.Text = (liRecOfMoney.Text.Trim().Length == 0) ? "0" : Convert2Currency(liRecOfMoney.Text);
			liRestOfMoney = (Literal)gvr.FindControl("liRestOfMoney"); liRestOfMoney.Text = (liRestOfMoney.Text.Trim().Length == 0) ? liRestOfMoney.Text : Convert2Currency(liRestOfMoney.Text);
			liHirePurchase = (Literal)gvr.FindControl("liHirePurchase");
			DateTime.TryParse(liPaymentDate.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out PaymentDate);
			DateTime.TryParse(liPrevousDate.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out PrevousDate);
			liPaymentDate.Text = (PaymentDate.Equals(DateTime.MinValue)) ? "" : PaymentDate.ToShortDateString();
			liPrevousDate.Text = (PrevousDate.Equals(DateTime.MinValue)) ? "" : PrevousDate.ToShortDateString();
			//liPrevousDate.Text = PrevousDate.ToShortDateString();
			liHirePurchase.Text = (liHirePurchase.Text == "1") ? Resources.Constants.HirePurchase : "";
		}
	}
	protected void cmdNext_Click(object sender, EventArgs e)
	{
		if (gvItems.PageIndex < gvItems.PageCount)
		{
			gvItems.PageIndex += 1; LoadOldGrid();
		}
	}
	protected void cmdPrevious_Click(object sender, EventArgs e)
	{
		if (gvItems.PageIndex > 0)
		{
			gvItems.PageIndex -= 1; LoadOldGrid();
		}
	}
	protected void cmdFirst_Click(object sender, EventArgs e)
	{
		gvItems.PageIndex = 0; LoadOldGrid();
	}
	protected void cmdLast_Click(object sender, EventArgs e)
	{
		gvItems.PageIndex = gvItems.PageCount - 1; LoadOldGrid();
	}
	private void LoadOldGrid()
	{
		try
		{
			DataTable oldPaymentTable = (DataTable)Session["sessPayment"];
			HttpContext.Current.Items["PaymentCount"] = oldPaymentTable.Rows.Count;
			gvItems.DataSource = oldPaymentTable;
			gvItems.DataBind();
		}
		catch (Exception)
		{
			DisplayControl(false);
		}
	}
	private void bindData(string Identifynumber, DateTime ReceiptDate)
	{
		DataSet ds = InventoryDao.RestOfMoney(Identifynumber, ReceiptDate, UserHelper.DealerCode);
		if (ds.Tables[0].Rows.Count > 0)
		{
			Session["sessPayment"] = ds.Tables[0];
			HttpContext.Current.Items["PaymentCount"] = ds.Tables[0].Rows.Count;
			gvItems.DataSource = ds.Tables[0];
			gvItems.DataBind();
			DisplayControl(true);
		}
		else
		{
			DisplayControl(false);
			ShowMessage(Resources.Constants.DataEmpty, true);
		}
	}
	private void ShowMessage(string mesg, bool isError)
	{
		lbErr.Visible = true;
		lbErr.ControlStyle.ForeColor = (isError) ? Color.Red : Color.RoyalBlue;
		lbErr.Text = mesg;
	}
	private void DisplayControl(bool flag)
	{
		if (flag)
		{
			lbNote1.Visible = true; gvItems.Visible = true;
		}
		else
		{
			lbNote1.Visible = false; gvItems.Visible = false;
		}
	}
}
