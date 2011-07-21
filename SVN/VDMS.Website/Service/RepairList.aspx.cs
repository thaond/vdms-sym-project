using System;
using System.Collections.ObjectModel;
using System.Web.UI.WebControls;
using Resources;
using VDMS.Helper;
using VDMS.I.ObjectDataSource;
using VDMS.I.Service;

public partial class Service_RepairList : BasePage
{
	protected string CurrentDealerCode
	{
		get { return !UserHelper.IsDealer ? ddlDealer.SelectedValue : UserHelper.DealerCode; }
	}

	private Collection<ServiceListErrorCode> errorCode = new Collection<ServiceListErrorCode>();

	protected void AddError(ServiceListErrorCode error)
	{
		if (errorCode.Contains(error) || (error == ServiceListErrorCode.OK)) return;
		errorCode.Add(error);
	}

	private void ShowError()
	{
		bllErrorMsg.Visible = errorCode.Count > 0;
		bllErrorMsg.Items.Clear();
		foreach (ServiceListErrorCode err in errorCode)
		{
			switch (err)
			{
				case ServiceListErrorCode.InvalidDateTime: bllErrorMsg.Items.Add(Message.WarrantyContent_InvalidDateTimeValue); break;
				//case ServiceListErrorCode.BrokenInUse: bllErrorMsg.Items.Add(Message.BrokenCodeInUse); break;
			}
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		errorCode.Clear();
		if (!IsPostBack)
		{
			if (!UserHelper.IsDealer)
			{
				ddlDealer.Visible = true;
				Literal12.Visible = true;
			}
			else
			{
				ddlDealer.Visible = false;
				ddlDealer.SelectedValue = UserHelper.DealerCode;
				Literal12.Visible = false;
			}
			txtRepairDateFrom.Text = DateTime.Now.AddDays(-3).ToShortDateString();
		}
	}

	protected void Page_PreRender(object sender, EventArgs e)
	{
		ShowError();
	}

	protected void btnCheck_Click(object sender, EventArgs e)
	{
		if (CheckInput())
		{
			if (string.IsNullOrEmpty(lv.DataSourceID)) lv.DataSourceID = odsSV.ID;
			else lv.DataBind();
		}
	}

	private bool CheckInput()
	{
		bool ok = true;
		DateTime repFrom, repTo, buyFrom, buyTo;
		if ((!string.IsNullOrEmpty(txtBuyDateFrom.Text)) && (!DateTime.TryParse(txtBuyDateFrom.Text, out buyFrom))) { AddError(ServiceListErrorCode.InvalidDateTime); ok = false; }
		if ((!string.IsNullOrEmpty(txtBuyDateTo.Text)) && (!DateTime.TryParse(txtBuyDateTo.Text, out buyTo))) { AddError(ServiceListErrorCode.InvalidDateTime); ok = false; }
		if ((!string.IsNullOrEmpty(txtRepairDateFrom.Text)) && (!DateTime.TryParse(txtRepairDateFrom.Text, out repFrom))) { AddError(ServiceListErrorCode.InvalidDateTime); ok = false; }
		if ((!string.IsNullOrEmpty(txtRepairDateTo.Text)) && (!DateTime.TryParse(txtRepairDateTo.Text, out repTo))) { AddError(ServiceListErrorCode.InvalidDateTime); ok = false; }
		return ok;
	}

	protected void ddlDealer_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!ddlDealer.Visible) return;
		ddlBranchCode.DealerCode = ddlDealer.SelectedValue;
		ddlBranchCode.DataBind();
	}

	protected void ddlDealer_DataBound(object sender, EventArgs e)
	{
		if (!IsPostBack) ddlDealer_SelectedIndexChanged(sender, e);
	}

	protected void lv_DataBound(object sender, EventArgs e)
	{
		var total = ServiceHeaderDataSource.SumSVList(ddlDealer.SelectedValue, ddlBranchCode.SelectedValue, txtSheetNoFrom.Text, txtSheetNoTo.Text, txtEngineNumber.Text, txtCustName.Text, txtBuyDateFrom.Text, txtBuyDateTo.Text, txtRepairDateFrom.Text, txtRepairDateTo.Text);
		Label lb = (Label)lv.FindControl("lbAllWA"); if (lb != null) lb.Text = EvalNumber(total.WarrantyPartAmount);
		lb = (Label)lv.FindControl("lbAllWF"); if (lb != null) lb.Text = EvalNumber(total.WarrantyFee);
		lb = (Label)lv.FindControl("lbAllSA"); if (lb != null) lb.Text = EvalNumber(total.ServicePartAmount);
		lb = (Label)lv.FindControl("lbAllSF"); if (lb != null) lb.Text = EvalNumber(total.ServiceFee);
	}
}
