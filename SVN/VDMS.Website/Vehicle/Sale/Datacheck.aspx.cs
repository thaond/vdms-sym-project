using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using VDMS.Common.Web;
using VDMS.Core.Domain;
using VDMS.Helper;
using VDMS.I.Report.DataObject;
using VDMS.I.Service;

public partial class Sales_Sale_Datacheck : BasePage
{
	bool creatingExcel;

	protected void Page_Load(object sender, EventArgs e)
	{
		IsDealer = CheckIsDealer();
		grvList.Columns[13].Visible = false;
		grvList.PageSize = 10;

		bllMessage.Items.Clear();
		if (!Page.IsPostBack)
		{
			if (!UserHelper.IsDealer) RefreshDropDownList();
		}
		if ((Page.IsPostBack) && (mtvMain.ActiveViewIndex == 1)) btnShowReport_Click(sender, null);
	}

	#region Fields
	private bool _IsDealer = false;
	public bool IsDealer
	{
		set
		{
			_IsDealer = value;

			if (_IsDealer)
			{
				// Refresh lại màn hình
				VMEPView.Visible = false;
				txtDealer.Visible = false;
			}
			else
			{
				VMEPView.Visible = true;
				txtDealer.Visible = false;
			}
		}
		get
		{
			return _IsDealer;
		}
	}
	#endregion

	#region Common Method

	private bool CheckIsDealer()
	{
		return UserHelper.IsDealer;
	}

	private void SwitchView(int vIndex)
	{
		if (mtvMain.ActiveViewIndex != vIndex)
		{
			mtvMain.ActiveViewIndex = vIndex;
		}
	}

	private bool CheckValidate()
	{
		bool res = true;

		DateTime fromDate;
		if (!DateTime.TryParse(txtFromDate.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out fromDate))
		{
			if (!string.IsNullOrEmpty(txtFromDate.Text.Trim()))
			{
				bllMessage.Items.Add(Resources.Constants.DateInvalid);
				txtFromDate.Focus();
				res = false;
				return res;
			}
		}
		DateTime toDate;
		if (!DateTime.TryParse(txtToDate.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out toDate))
		{
			if (!string.IsNullOrEmpty(txtToDate.Text.Trim()))
			{
				bllMessage.Items.Add(Resources.Constants.DateInvalid);
				txtToDate.Focus();
				res = false;
				return res;
			}
		}

		int lenFromDate = txtFromDate.Text.Trim().Length;
		int lenToDate = txtToDate.Text.Trim().Length;
		int lenTotal = lenFromDate + lenToDate;
		if (lenTotal != 0)
		{
			if (lenTotal == lenFromDate || lenTotal == lenToDate)
			{
				bllMessage.Items.Add(Resources.Constants.DateInvalid);
				res = false;
				return res;
			}
		}
		return res;
	}

	void BindData()
	{
		if (CheckValidate())
		{
			InvoiceDataSource1.SelectParameters["fromDate"] = new Parameter("fromDate", TypeCode.String, txtFromDate.Text.Trim());
			InvoiceDataSource1.SelectParameters["toDate"] = new Parameter("toDate", TypeCode.String, txtToDate.Text.Trim());
			txtDealer.Text = "";
			if (ddlDealerCode.SelectedIndex != 0)
				txtDealer.Text = ddlDealerCode.SelectedValue.Trim().ToUpper();
			if (IsDealer)
				txtDealer.Text = UserHelper.DealerCode;

			InvoiceDataSource1.SelectParameters["DealerCode"] = new Parameter("DealerCode", TypeCode.String, txtDealer.Text.Trim().ToUpper());
			grvList.DataSourceID = "InvoiceDataSource1";
			grvList.DataBind();
		}
	}

	private void RefreshDropDownList()
	{
		ddlDealerCode.AreaCode = ddlArea.SelectedValue;
		ddlDealerCode.DataBind();
	}

	private void ShowReport()
	{
		if (CheckValidate())
		{
			crvMain.DisplayGroupTree = false;
			crvMain.HasCrystalLogo = false;
			txtDealer.Text = "";
			if (ddlDealerCode.SelectedIndex != 0)
				txtDealer.Text = ddlDealerCode.SelectedValue.Trim().ToUpper();
			if (IsDealer)
				txtDealer.Text = UserHelper.DealerCode;
			crvMain.ReportSource = ReportDataSource.GetCustomerExReport(Server.MapPath(@"~/Report/Stamps.rpt"), ddlCusClass.SelectedValue, txtEngineNo.Text.Trim(), txtNumberPlate.Text, txtInvoiceNo.Text.Trim(), txtFromDate.Text.Trim(), txtToDate.Text.Trim(), ddlArea.SelectedValue, txtDealer.Text.Trim().ToUpper(), txtIdentityNo.Text.Trim(), txtFullname.Text.Trim(), txtAds.Text.Trim(), txtPrecinct.Text.Trim(), txtDistrictId.Text.Trim(), ddlProvince.SelectedValue.Trim(), ddlItem.SelectedValue);
			crvMain.DataBind();
		}
	}

	#endregion

	#region Eval Method

	protected string EvalAddress(object Address, object Districtid, object Precinct, object Provinceid)
	{
		if (!creatingExcel) return (string)Address;

		Customer cus = new Customer();
		if (Address != null) cus.Address = Address.ToString();
		if (Districtid != null) cus.Districtid = Districtid.ToString();
		if (Precinct != null) cus.Precinct = Precinct.ToString();
		if (Provinceid != null) cus.Provinceid = Provinceid.ToString();
		return ServiceTools.GetCustAddress(cus);
	}

	protected string EvalGender(object male)
	{
		if (male == null) return "";
		if (male is bool) return ((bool)male) ? Gender.Male : Gender.Female;
		if (male is int) return ((int)male == 1) ? Gender.Male : Gender.Female;
		return "";
	}

	protected string EvalDealerName(object dealerCode)
	{
		return (dealerCode != null) ? DealerHelper.GetName(dealerCode.ToString()) : "";
	}

	protected string EvalCusType(string cusType)
	{
		return ddlCusClass.Items.FindByValue(cusType).Text;
	}

	#endregion

	#region Event Controls

	protected void btnTest_Click(object sender, EventArgs e)
	{
		SwitchView(0);
		BindData();
	}

	protected void btnShowReport_Click(object sender, EventArgs e)
	{
		SwitchView(1);
		ShowReport();
		//onShowingReport = true;
	}

	protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
	{
		RefreshDropDownList();
	}

	protected void grvList_DataBound(object sender, EventArgs e)
	{
		try
		{
			Literal litPageInfo = grvList.TopPagerRow.FindControl("litPageInfo") as Literal;
			if (litPageInfo != null) litPageInfo.Text = string.Format(Resources.Message.PagingInfo, grvList.PageIndex + 1, grvList.PageCount, HttpContext.Current.Items["rowCount"]);
		}
		catch { }
	}

	protected void grvList_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			e.Row.Cells[0].Text = (grvList.PageSize * grvList.PageIndex + e.Row.RowIndex + 1).ToString();
		}
	}
	#endregion

	protected void InitPostbackData(object sender, EventArgs e)
	{
		((TextBox)sender).Text = ((TextBox)sender).Text.Trim();
	}

	protected void btnExcel_Click(object sender, EventArgs e)
	{
		creatingExcel = true;
		//grvList.PageSize = 65535;
		grvList.AllowPaging = false;
		InvoiceDataSource1.EnablePaging = false;
		grvList.Columns[13].Visible = true;
		grvList.GridLines = GridLines.Both;
		BindData();
		GridView2Excel.Export(grvList, Page, "CustomerData.xls");
		creatingExcel = false;
	}

	protected void ddlItem_DataBound(object sender, EventArgs e)
	{
		DropDownList drop = (DropDownList)sender;
		if ((drop != null) && (drop.Items.Count > 0))
		{
			drop.Items[0].Value = "";
		}
	}
}
