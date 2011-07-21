using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using VDMS.Common.Utils;
using VDMS.Common.Web;
using VDMS.Core.Domain;
using VDMS.Data.TipTop;
using VDMS.Helper;
using VDMS.I.ObjectDataSource;
using VDMS.I.Service;

public partial class UC_AddExchange : UserControl
{
	private Warrantycondition footWarr;
	private Broken footBroken;
	private bool updateFee = false;
	private string brokenCode, spareNumber, quantity, dealerCode;
	private Collection<WarrantyContentErrorCode> errors = new Collection<WarrantyContentErrorCode>();

	//private int quantity;

	public string EngineNumber, FrameNumber;
	public VDMS.Core.Domain.Customer Cust = null;
	public int Km;
	public string Model, BuyDate;
	public Exchangepartheader ExchangeHeader;
	public DataTable RawSpareList;
	public bool readOnlyMode;

	public DataTable SpareList
	{
		get { return gvexSpareList.DataSourceTable; }
	}

	// user input amount
	public long TotalFee
	{
		get
		{
			long fee;
			return (long.TryParse(txtexFeeOffer.Text, out fee)) ? fee : 0;
		}
	}

	public delegate void FindSpare_Click(object sender, EventArgs e);
	public FindSpare_Click OnFindSpareClick;
	public delegate void FindBroken_Click(object sender, EventArgs e);
	public FindBroken_Click OnFindBrokenClick;
	public delegate void SaveList_Click(object sender, EventArgs e);
	public SaveList_Click OnSaveListClick;
	public delegate void CancelSaveList_Click(object sender, EventArgs e);
	public CancelSaveList_Click OnCancelSaveListClick;
	public delegate void AddErrorFunc(WarrantyContentErrorCode error);
	public AddErrorFunc AddNewError; // add error to parent

	public void AddSpareTofoot(Warrantycondition spare)
	{
		footWarr = spare;
		spareNumber = (spare != null) ? spare.Partcode : "";
		ViewState["Exchange_SpareNumber"] = spareNumber;
	}

	public void AddBrokenTofoot(Broken broken)
	{
		footBroken = broken;
		brokenCode = (broken != null) ? broken.Brokencode : "";
		ViewState["Exchange_BrokenCode"] = brokenCode;
	}

	public void ClearForm()
	{
		//txtexAddress.Text = "";
		//txtexAreaCode.Text = "";
		//txtexBuyDate.Text = "";
		//txtexCustName.Text = "";
		//txtexDamage.Text = "";
		//txtexDamagedDate.Text = "";
		//txtexDealer.Text = "";
		//txtexElectricalDmg.Text = "";
		//txtexEngineDmg.Text = "";
		//txtexEngineNumber.Text = "";
		//txtexExportDate.Text = "";
		//txtexFrameDmg.Text = "";
		//txtexFrameNum.Text = "";
		//txtexKm.Text = "";
		//txtexModel.Text = "";
		//txtexNote.Text = "";
		//txtexPhone.Text = "";
		//txtexReason.Text = "";
		//txtexReceipt.Text = "";
		//txtexRepairDate.Text = "";
		FormHelper.ResetControl(this);
		if (gvexSpareList.DataSourceTable != null) gvexSpareList.DataSourceTable.Rows.Clear();
	}

	public void InitUC()
	{
		//ClearForm();
		InitForm();

		if (ExchangeHeader != null)
		{
			if (this.SpareList == null) Page_Load(null, null);
			//Iteminstance item = AddExchange.GetItemInstance(ExchangeHeader.Enginenumber);
			//if (item != null) ExchangeHeader.Exportdate = item.Madedate;

			txtexEngineNumber.Text = ExchangeHeader.Enginenumber;
			txtexFrameNum.Text = ExchangeHeader.Framenumber;
			if (ExchangeHeader.Purchasedate != null) txtexBuyDate.Text = ExchangeHeader.Purchasedate.ToShortDateString();
			if (ExchangeHeader.Damageddate != null) txtexDamagedDate.Text = ExchangeHeader.Damageddate.ToShortDateString();
			if (ExchangeHeader.Exchangeddate != null) txtexRepairDate.Text = ExchangeHeader.Exchangeddate.ToShortDateString();
			if ((ExchangeHeader.Exportdate != null) && (ExchangeHeader.Exportdate > DateTime.MinValue)) txtexExportDate.Text = ExchangeHeader.Exportdate.ToShortDateString();
			else txtexExportDate.Text = "";
			txtexKm.Text = ExchangeHeader.Kmcount.ToString();
			txtexModel.Text = ExchangeHeader.Model;
			txtexNote.Text = ExchangeHeader.Comments;
			txtexReason.Text = ExchangeHeader.Reason;
			txtexEngineDmg.Text = ExchangeHeader.Engine;
			txtexElectricalDmg.Text = ExchangeHeader.Electric;
			txtexFrameDmg.Text = ExchangeHeader.Frame;
			txtexDamage.Text = ExchangeHeader.Damaged;
			txtexFeeOffer.Text = ExchangeHeader.Feeamount.ToString();
			rblRoad.SelectedIndex = ExchangeHeader.Road;
			rblSpeed.SelectedIndex = ExchangeHeader.Speed;
			rblTransport.SelectedIndex = ExchangeHeader.Usage;
			rblWeather.SelectedIndex = ExchangeHeader.Weather;
			if (ExchangeHeader.Customer != null)
			{
				txtexAddress.Text = ServiceTools.GetCustAddress(ExchangeHeader.Customer);
				txtexCustName.Text = ExchangeHeader.Customer.Fullname;
				txtexPhone.Text = ServiceTools.GetCustTelNo(ExchangeHeader.Customer);
			}
		}
	}

	public string GetLookingSpareNumber()
	{
		AddSpareTofoot(null);
		TextBox tb = (TextBox)WebTools.FindControlById("txtexSpareNumber", gvexSpareList);
		if (tb == null) return "";
		return tb.Text;
	}

	public string GetLookingBrokenNumber()
	{
		TextBox tb = (TextBox)WebTools.FindControlById("txtexBrokenCode", gvexSpareList);
		if (tb == null) return "";
		return tb.Text;
	}

	public void CollectData()
	{
		DateTime dt;
		long km;
		int road, weather, usage, speed;
		if (ExchangeHeader == null) ExchangeHeader = new Exchangepartheader();
		ExchangeHeader.Areacode = UserHelper.AreaCode;
		ExchangeHeader.Comments = txtexNote.Text.Trim();
		if ((ExchangeHeader.Customer == null) && (Cust != null)) ExchangeHeader.Customer = Cust;
		ExchangeHeader.Damaged = txtexDamage.Text.Trim();
		DateTime.TryParse(txtexDamagedDate.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dt); ExchangeHeader.Damageddate = dt;
		ExchangeHeader.Electric = txtexElectricalDmg.Text.Trim();
		ExchangeHeader.Engine = txtexEngineDmg.Text.Trim();
		ExchangeHeader.Enginenumber = txtexEngineNumber.Text.Trim().ToUpper();
		DateTime.TryParse(txtexRepairDate.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dt); ExchangeHeader.Exchangeddate = dt;
		DateTime.TryParse(txtexExportDate.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dt); ExchangeHeader.Exportdate = dt;
		ExchangeHeader.Feeamount = gvexSpareList.SumBy("FeeAmount");
		ExchangeHeader.Frame = txtexFrameDmg.Text.Trim();
		ExchangeHeader.Framenumber = txtexFrameNum.Text.Trim().ToUpper();
		long.TryParse(txtexKm.Text, out km); ExchangeHeader.Kmcount = km;
		ExchangeHeader.Model = txtexModel.Text.Trim().ToUpper();
		DateTime.TryParse(txtexBuyDate.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dt); ExchangeHeader.Purchasedate = dt;
		ExchangeHeader.Reason = txtexReason.Text.Trim();
		int.TryParse(rblRoad.SelectedValue, out road); ExchangeHeader.Road = road;
		int.TryParse(rblSpeed.SelectedValue, out speed); ExchangeHeader.Speed = speed;
		int.TryParse(rblWeather.SelectedValue, out weather); ExchangeHeader.Weather = weather;
		int.TryParse(rblTransport.SelectedValue, out usage); ExchangeHeader.Usage = usage;
		//ExchangeHeader.Vouchernumber = AddExchange.GenExchangeNumber(txtexDealer.Text);
	}

	public void ChangeLanguage()
	{
		gvexSpareList.EmptyTableRowText = Message.WarrantyContent_EmptySpareList;
		SetupSpareString();
	}

	public void UpdateSpareList()
	{
		gvexSpareList.DataBind();
	}

	public void CalculateAmount(DataRow newRow, DataRow oldRow)
	{
		long cost, quantity, addedquantity, amount;
		double totalfee, manPower, labour;
		if (newRow == null) return;

		if ((oldRow != null)) // inc quantity
		{
			long.TryParse(newRow["Quantity"].ToString(), out addedquantity);
			long.TryParse(oldRow["Quantity"].ToString(), out quantity);
			long.TryParse(oldRow["SpareCost"].ToString(), out cost);
			double.TryParse(oldRow["ManPower"].ToString(), out manPower);
			double.TryParse(oldRow["Labour"].ToString(), out labour);

			addedquantity += quantity;
			totalfee = manPower * labour * addedquantity;
			amount = addedquantity * cost;
			oldRow["Quantity"] = (addedquantity > 999) ? 999 : addedquantity;
			oldRow["SpareAmount"] = amount;
			oldRow["FeeAmount"] = totalfee;
			oldRow["SerialNumber"] = newRow["SerialNumber"];
		}
		else  //update one
		{
			long.TryParse(newRow["Quantity"].ToString(), out quantity);
			long.TryParse(newRow["SpareCost"].ToString(), out cost);
			double.TryParse(newRow["ManPower"].ToString(), out manPower);
			double.TryParse(newRow["Labour"].ToString(), out labour);

			amount = quantity * cost;
			totalfee = manPower * labour * quantity;
			newRow["SpareAmount"] = amount;
			newRow["Quantity"] = quantity;
			newRow["SpareCost"] = cost;
			newRow["FeeAmount"] = totalfee;
		}

		if (!readOnlyMode) UpdateTotalFee();
	}

	public bool GetSpareInfo(string spareNumber, DataRow row)
	{
		double manPower;
		Warrantycondition warr = WarrantyContent.GetWarrantyCondition(spareNumber);
		if ((warr == null) && (AddNewError != null)) { return false; }

		string manP = NumberFormatHelper.StrDoubleToStr(warr.Manpower, "en-US");

		if (string.IsNullOrEmpty(row["SpareCost"].ToString())) row["SpareCost"] = warr.Unitprice.ToString();
		double.TryParse(manP, out manPower);
		row["SpareNumber"] = warr.Partcode;
		row["SpareNameEn"] = warr.Partnameen;
		row["SpareNameVn"] = warr.Partnamevn;
		row["ManPower"] = manP; // h tieu chuan
		row["Labour"] = warr.Labour;
		row["FeeAmount"] = ((double)((double)warr.Labour * manPower));//.ToString(); ;
		row["WarrantyTime"] = warr.Warrantytime;
		row["WarrantyLength"] = warr.Warrantylength;
		//row["NoWarranty"] = repairDt.Month - buyDt.Month;
		return true;
	}

	public WarrantyContentErrorCode AddNewItem(DataRow newRow)
	{
		// check new values
		Broken brk = BrokenDatasource.GetByCode(newRow["BrokenCode"].ToString());
		WarrantyContentErrorCode result = (brk != null) ? WarrantyContentErrorCode.OK : WarrantyContentErrorCode.BrokenCodeNotFound;

		if (result == WarrantyContentErrorCode.OK)
		{
			DataRow oldRow = gvexSpareList.GetSimpleRowByKey("SpareNumber", newRow["SpareNumber"].ToString(), true);
			if ( // add fresh new 
				 (oldRow == null) ||
				// add old spare but new broken code
				 ((oldRow != null) && (newRow["BrokenCode"].ToString() != oldRow["BrokenCode"].ToString()))
			   )
			{
				if (GetSpareInfo(newRow["SpareNumber"].ToString(), newRow))
				{
					CalculateAmount(newRow, null);
					gvexSpareList.NewSimpleRow = newRow;
					updateFee = true;
				}
				else
				{
					result = WarrantyContentErrorCode.InvalidSpareCode;
					gvexSpareList.NewSimpleRow = null;
				}
			}
			else                // inc quantity
			{
				CalculateAmount(newRow, oldRow);
				gvexSpareList.NewSimpleRow = null;    // cancel insert
			}
		}

		return result;
	}

	private void UpdateTotalFee()
	{
		// update total fee when add or update spare
		txtexFeeOffer.Text = gvexSpareList.SumBy("FeeAmount").ToString();
	}
	private void ShowError()
	{
		bllErrorMsg.Visible = errors.Count > 0;
		bllErrorMsg.Items.Clear();
		foreach (WarrantyContentErrorCode error in errors)
		{
			switch (error)
			{
				case WarrantyContentErrorCode.BrokenCodeNotFound: bllErrorMsg.Items.Add(Message.WarrantyContent_BrokenCodeNotFound); break;
				case WarrantyContentErrorCode.EngineNumberNotFound: bllErrorMsg.Items.Add(Message.WarrantyContent_EngineNumberNotFound); break;
				case WarrantyContentErrorCode.ExchangeNumberNotFound: bllErrorMsg.Items.Add(Message.WarrantyContent_ExchangeNumberNotFound); break;
				case WarrantyContentErrorCode.InvalidDateTimeValue: bllErrorMsg.Items.Add(Message.WarrantyContent_InvalidDateTimeValue); break;
				case WarrantyContentErrorCode.InvalidExchangeSparesList: bllErrorMsg.Items.Add(Message.WarrantyContent_InvalidExchangeSparesList); break;
				case WarrantyContentErrorCode.InvalidServiceType: bllErrorMsg.Items.Add(Message.WarrantyContent_InvalidServiceType); break;
				case WarrantyContentErrorCode.InvalidSpareCode: bllErrorMsg.Items.Add(Message.WarrantyContent_InvalidSpareCode); break;
				case WarrantyContentErrorCode.ItemTypeNotFound: bllErrorMsg.Items.Add(Message.WarrantyContent_ItemTypeNotFound); break;
				case WarrantyContentErrorCode.NoItemSold: bllErrorMsg.Items.Add(Message.WarrantyContent_NoItemSold); break;
				case WarrantyContentErrorCode.SaveDetailFailed: bllErrorMsg.Items.Add(Message.WarrantyContent_SaveDetailFailed); break;
				case WarrantyContentErrorCode.SaveExchDetailFailed: bllErrorMsg.Items.Add(Message.WarrantyContent_SaveExchDetailFailed); break;
				case WarrantyContentErrorCode.SaveExchHeaderFailed: bllErrorMsg.Items.Add(Message.WarrantyContent_SaveExchHeaderFailed); break;
				case WarrantyContentErrorCode.SaveHeaderFailed: bllErrorMsg.Items.Add(Message.WarrantyContent_SaveHeaderFailed); break;
				case WarrantyContentErrorCode.ServiceSheetNumberNotFound: bllErrorMsg.Items.Add(Message.WarrantyContent_ServiceSheetNumberNotFound); break;
				case WarrantyContentErrorCode.SpareNumberNotFound: bllErrorMsg.Items.Add(Message.WarrantyContent_SpareNumberNotFound); break;
				case WarrantyContentErrorCode.UpdateDataFailed: bllErrorMsg.Items.Add(Message.WarrantyContent_UpdateDataFailed); break;
				case WarrantyContentErrorCode.StringTooLong: bllErrorMsg.Items.Add(Message.WarrantyContent_StringTooLong); break;
			}
		}
	}
	protected void AddError(WarrantyContentErrorCode error)
	{
		if (errors.Contains(error)) return;
		errors.Add(error);
	}
	private void ClearFootState()
	{
		footBroken = null; if (ViewState["Exchange_BrokenCode"] != null) ViewState.Remove("Exchange_BrokenCode");
		footWarr = null; if (ViewState["Exchange_SpareNumber"] != null) ViewState.Remove("Exchange_SpareNumber");
		quantity = "1";
	}
	private void SetReadOnly(WebControl obj, bool readOnly)
	{
		if (obj == null) return;
		obj.CssClass = ((readOnly) && ((obj is TextBox) || (obj is Panel) || (obj is DropDownList))) ? "readOnlyInputField" : "";
		if (obj is TextBox) ((TextBox)obj).ReadOnly = readOnly;
		if (obj is ImageButton) ((ImageButton)obj).Enabled = !readOnly;
		if (obj is Button) ((Button)obj).Enabled = !readOnly;
		if (obj is CheckBoxList) ((CheckBoxList)obj).Enabled = !readOnly;
		if (obj is DropDownList) ((DropDownList)obj).Enabled = !readOnly;
		if (obj is Panel) ((Panel)obj).Enabled = !readOnly;
	}
	private void InitForm()
	{
		dealerCode = (ExchangeHeader != null) ? ExchangeHeader.Dealercode : "";
		txtexDealer.Text = dealerCode;
		txtexAreaCode.Text = VDMS.II.BasicData.DealerDAO.GetDealerByCode(dealerCode).AreaCode;
		txtexDamagedDate.Text = DateTime.Now.ToShortDateString();
		txtexRepairDate.Text = DateTime.Now.ToShortDateString();
		if (ExchangeHeader != null) txtexReceipt.Text = ExchangeHeader.Vouchernumber;  // AddExchange.GenExchangeNumber(dealerCode);
	}

	// CollectData on form and store to use later
	protected void SetupSpareString()
	{
		if (gvexSpareList.DataSourceTable == null) return;
		string name = (Thread.CurrentThread.CurrentCulture.Name == "vi-VN") ? "SpareNameVn" : "SpareNameEn";
		foreach (DataRow row in gvexSpareList.DataSourceTable.Rows)
		{
			row["SpareName"] = row[name].ToString();
		}
	}
	protected void CheckReadOnlyMode()
	{
		gvexSpareList.ShowFooter = (!readOnlyMode) && (gvexSpareList.EditIndex < 0);
		btnCancel.Visible = !readOnlyMode;
		btnSave.Visible = !readOnlyMode;
		txtexDamage.ReadOnly = readOnlyMode;
		txtexElectricalDmg.ReadOnly = readOnlyMode;
		txtexEngineDmg.ReadOnly = readOnlyMode;
		txtexFrameDmg.ReadOnly = readOnlyMode;
		txtexNote.ReadOnly = readOnlyMode;
		txtexReason.ReadOnly = readOnlyMode;
		txtexDamagedDate.ReadOnly = readOnlyMode;
		ibtnCalendar.Enabled = !readOnlyMode;
		rblRoad.Enabled = !readOnlyMode;
		rblSpeed.Enabled = !readOnlyMode;
		rblTransport.Enabled = !readOnlyMode;
		rblWeather.Enabled = !readOnlyMode;
		SetReadOnly(txtexFeeOffer, readOnlyMode);
		if ((readOnlyMode) && (ExchangeHeader != null))
			txtexReceipt.Text = ExchangeHeader.Vouchernumber;
	}
	protected void CheckWarrantyCondition()
	{
		DateTime buyDt, repairDt;
		long km;
		DateTime.TryParse(txtexBuyDate.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out buyDt);
		DateTime.TryParse(txtexRepairDate.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out repairDt);
		long.TryParse(txtexKm.Text, out km);

		foreach (DataRow row in gvexSpareList.DataSourceTable.Rows)
		{
			Warrantycondition warr = WarrantyContent.GetWarrantyCondition(row["SpareNumber"].ToString());
			if (warr == null) continue;
			if (((((TimeSpan)repairDt.Subtract(buyDt)).Days / 30) <= warr.Warrantytime) || ((km <= warr.Warrantylength)))
				row["NoWarranty"] = "N";
			else
				row["NoWarranty"] = null; // show warnning
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		errors.Clear();

		if (!IsPostBack)
		{
			if (sender != null) InitForm(); // manual call to pageLoad
			if (gvexSpareList.DataSourceTable == null)
				gvexSpareList.DataSourceTable = AddExchange.SpareListOnServiceSchema;

			gvexSpareList.EmptyTableRowText = Message.WarrantyContent_EmptySpareList;

			if (RawSpareList != null) gvexSpareList.DataSourceTable = RawSpareList;
		}
		if (ViewState["Exchange_BrokenCode"] != null)
		{
			brokenCode = ViewState["Exchange_BrokenCode"].ToString();
			footBroken = WarrantyContent.GetBroken(brokenCode);
		}
		if (ViewState["Exchange_SpareNumber"] != null)
		{
			spareNumber = ViewState["Exchange_SpareNumber"].ToString();
			footWarr = WarrantyContent.GetWarrantyCondition(spareNumber);
		}

		// save new spare quantity
		if (gvexSpareList.EditIndex < 0)
		{
			TextBox tb = (TextBox)WebTools.FindControlById("txtexQuantity", gvexSpareList);
			if (tb != null) quantity = tb.Text;
		}
	}
	protected void Page_PreRender(object sender, EventArgs e)
	{
		CheckReadOnlyMode();
		if (VDMS.VDMSSetting.CurrentSetting.CheckWarrantyCondition) CheckWarrantyCondition();
		tblWarrantyHint.Visible = VDMS.VDMSSetting.CurrentSetting.CheckWarrantyCondition;

		//ShowError();
		gvexSpareList.DataBind();

		// total fee
		//NumberFormatInfo ni = Thread.CurrentThread.CurrentCulture.NumberFormat;
		//ni.NumberDecimalDigits = 0;
		//txtexFeeOffer.Text = gvexSpareList.SumBy("FeeAmount").ToString("N", ni);
		if (updateFee) UpdateTotalFee();

		bool showWarrantyFee = string.IsNullOrEmpty(dealerCode);
		gvexSpareList.Columns[8].Visible = showWarrantyFee;
		gvexSpareList.Columns[9].Visible = showWarrantyFee;
		gvexSpareList.Columns[10].Visible = showWarrantyFee;
		ShowError();
	}

	protected void gvexSpareList_RowEditing(object sender, GridViewEditEventArgs e)
	{
		gvexSpareList.ShowFooter = false;
	}
	protected void gvexSpareList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		gvexSpareList.ShowFooter = true;
		ClearFootState();
	}
	protected void gvexSpareList_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		if (e.CommandName == "InsertSimpleRow")
		{
			DataTable tbl = gvexSpareList.DataSourceTable;
			DataRow newRow = gvexSpareList.NewSimpleRow;
			if (newRow == null) return;

			WarrantyContentErrorCode err = AddNewItem(newRow);
			if (err == WarrantyContentErrorCode.OK)
			{
				// clear foot state
				ClearFootState();
			}
			else
			{
				gvexSpareList.NewSimpleRow = null;         // cancel insert
				AddError(err);
			}
		}
		//else if (e.CommandName == "AddNew")
		//{
		//    ShowSumary = false;
		//}
		//else if (e.CommandName == "CancelInsertSimpleRow")
		//{
		//    ShowSumary = true;
		//}
	}
	protected void gvexSpareList_DataBound(object sender, EventArgs e)
	{
		TextBox tb;
		GridViewRow row = gvexSpareList.FootRow;
		// warranty spare number
		if (footWarr != null)
		{
			if (row != null)
			{
				NumberFormatInfo ni = Thread.CurrentThread.CurrentCulture.NumberFormat; ni.NumberDecimalDigits = 0;
				row.Cells[2].Text = (Thread.CurrentThread.CurrentCulture.Name == "vi-VN") ? footWarr.Partnamevn : footWarr.Partnameen;
				row.Cells[5].Text = footWarr.Unitprice.ToString("N", ni);
			}
			tb = (TextBox)WebTools.FindControlById("txtexSpareNumber", gvexSpareList);
			if (tb != null) tb.Text = footWarr.Partcode;
		}

		// broken code
		if (footBroken != null)
		{
			tb = (TextBox)WebTools.FindControlById("txtexBrokenCode", gvexSpareList);
			if (tb != null) tb.Text = footBroken.Brokencode;
		}

		// restore new spare quantity
		if (gvexSpareList.EditIndex < 0)
		{
			tb = (TextBox)WebTools.FindControlById("txtexQuantity", gvexSpareList);
			if ((tb != null) && (!string.IsNullOrEmpty(quantity))) tb.Text = quantity;
		}
	}
	protected void gvexSpareList_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		DataRow newRow = gvexSpareList.NewSimpleRow;
		if ((newRow == null)) { AddNewError(WarrantyContentErrorCode.UpdateDataFailed); return; }

		if (!GetSpareInfo(newRow["SpareNumber"].ToString(), newRow))
		{ e.Cancel = true; return; }

		CalculateAmount(gvexSpareList.NewSimpleRow, null);
		gvexSpareList.ShowFooter = true;
		updateFee = true;

		ClearFootState();
	}
	protected void gvexSpareList_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{

	}
	protected void gvexSpareList_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		EmptyGridViewEx gv = (EmptyGridViewEx)sender;
		double val;

		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			if (gv.DataSourceTable == null) return;
			DataRow datarow = gv.DataSourceTable.Rows[e.Row.DataItemIndex];

			// no warranty warning
			DataRowView row = (DataRowView)e.Row.DataItem;
			if (!string.IsNullOrEmpty(row["NoWarranty"].ToString())) e.Row.Cells[0].Text = "";

			// format number
			NumberFormatInfo ni = Thread.CurrentThread.CurrentCulture.NumberFormat;
			ni.NumberDecimalDigits = 1;
			if (double.TryParse(datarow["ManPower"].ToString(), out val)) e.Row.Cells[8].Text = val.ToString("N", ni);
			ni.NumberDecimalDigits = 0;
			if (double.TryParse(datarow["SpareCost"].ToString(), out val)) e.Row.Cells[5].Text = val.ToString("N", ni);
			if (double.TryParse(datarow["SpareAmount"].ToString(), out val)) e.Row.Cells[6].Text = val.ToString("N", ni);
			if (double.TryParse(datarow["Labour"].ToString(), out val)) e.Row.Cells[9].Text = val.ToString("N", ni);
			if (double.TryParse(datarow["FeeAmount"].ToString(), out val)) e.Row.Cells[10].Text = val.ToString("N", ni);
		}
	}

	protected void btnexFindSpare_Click(object sender, EventArgs e)
	{
		if (OnFindSpareClick != null) OnFindSpareClick(sender, e);
	}
	protected void btnexFindBroken_Click(object sender, EventArgs e)
	{
		if (OnFindBrokenClick != null) OnFindBrokenClick(sender, e);
	}
	// save list
	protected void Button2_Click(object sender, EventArgs e)
	{
		DateTime dtDamage, dtRepair, dtBuy;
		CollectData();

		dtDamage = ExchangeHeader.Damageddate;
		dtRepair = ExchangeHeader.Exchangeddate;
		dtBuy = ExchangeHeader.Purchasedate;
		if (txtexDamage.Text.Length >= txtexDamage.MaxLength) { AddError(WarrantyContentErrorCode.StringTooLong); }
		if (txtexReason.Text.Length >= txtexReason.MaxLength) { AddError(WarrantyContentErrorCode.StringTooLong); }
		if (txtexNote.Text.Length >= txtexNote.MaxLength) { AddError(WarrantyContentErrorCode.StringTooLong); }
		if (dtDamage > DateTime.Now) { AddError(WarrantyContentErrorCode.InvalidDateTimeValue); }
		if ((dtBuy != null) && (dtBuy > dtDamage)) { AddError(WarrantyContentErrorCode.InvalidDateTimeValue); }
		if ((dtRepair != null) && (dtRepair < dtDamage)) { AddError(WarrantyContentErrorCode.InvalidDateTimeValue); }

		if (errors.Count == 0)
		{
			if (OnSaveListClick != null) OnSaveListClick(sender, e);
		}
	}
	protected void Button3_Click(object sender, EventArgs e)
	{
		UpdateTotalFee();
		if (OnCancelSaveListClick != null) OnCancelSaveListClick(sender, e);
	}

	protected void ImageButton1_DataBinding(object sender, EventArgs e)
	{
		ImageButton imgbtn = (ImageButton)sender;
		imgbtn.Visible = !readOnlyMode;
	}
	protected void txtexQuantity_DataBinding(object sender, EventArgs e)
	{
		string a = ((TextBox)sender).Text;
	}
}
