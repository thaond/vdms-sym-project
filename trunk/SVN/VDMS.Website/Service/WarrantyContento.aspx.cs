using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using NHibernate;
using NHibernate.Expression;
using Resources;
using VDMS.Common.Utils;
using VDMS.Common.Web;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.IDAL.Interface;
using VDMS.Data.TipTop;
using VDMS.Helper;
using VDMS.I.Entity;
using VDMS.I.ObjectDataSource;
using VDMS.I.ObjectDataSource.RepairList;
using VDMS.I.Service;
using VDMS.I.Vehicle;
using Broken = VDMS.Core.Domain.Broken;
using Customer = VDMS.Core.Domain.Customer;
using Dealer = VDMS.Data.TipTop.Dealer;
using Invoice = VDMS.Core.Domain.Invoice;

public partial class Service_WarrantyContent : BasePage
{
	// setting section
	private const bool showSourceDealerWhenAlarm = true;    // dai ly sua <> dai ly nhap xe thi alarm ==> co kem ten dai ly nhap ko?
	private const bool allowInvalidSpare = true;
	private const bool allowInputSRSFeeAfterComplete = true;// cho phep nhap fee cua SRS sau khi da nhan nut "Hoan tat nhap lieu"
	private const bool showFeeAmountColumn = false;         // show col FeeAmount len thi fee dc tinh tong tu dong va khong cho nhap
	private const bool requireCheckWarrantyToAddPCV = true; // user must check warranty check box to add partChangeVoucher
	private const bool allWayPrint = true;                  // In SRS moi luc moi noi :)
	private const bool showTempSheetNo = false;             // hien so SRS/PVC khi chua luu?
	private const string imageFolderBase = "~/Images/";
	private const string imageAddNew = "new.jpg";
	private const int selectGridViewPageSize = 10;
	private const int selectSparePageSize = 50;
	private const int selectBrokenCodePageSize = 50;

	// constant values
	private const int vwMainIndex = 0;
	private const int vwSelectItemIndex = 1;
	private const int vwSelectSpareIndex = 2;
	private const int vwAddCustIndex = 3;
	private const int vwSelectModelIndex = 4;
	private const int vwAddExchangeIndex = 5;
	private const int vwAddBrokenIndex = 6;
	private const int vwNoSheetToViewIndex = 7;
	private const int vwPrintIndex = 8;
	protected string LoadCustomerErr = Resources.Message.LoadCustomerErr;
	protected string SumMoneyInvalid = Resources.Message.Cus_SumMoneyInvalid;
	protected string LoadEngineNoErr = Resources.Message.EngineNumberNotFound;

	// variables store information during process
	private Collection<WarrantyContentErrorCode> errorCode = new Collection<WarrantyContentErrorCode>();
	private Warrantycondition footWarranty;  // Warranty information for a spare  when add in footer
	private Broken footBroken;
	private Exchangepartheader _exchangeHeader;
	private bool _canChangeBuyDate, _showSumary, _finish, _saved, _saveTemp, _exchSaved, _addExchange, readOnlyForm = false, _finishChecking = false;
	private string _feeAmount, viewmodeDealer = "", _exchangeNumber = "", showServiceSheetNumber, showExchangeSparesNumber, _repairSpare;
	private Customer _custInfo;
	private DateTime _dtVehicleMadedate = DateTime.MinValue;
	private bool _isVehicleOnTiptop = false, _itemExist = false, itemSelecting = false;         // dang chon 1 xe => giu lai thong tin vua lay dc
	private string _sVehicleDB = string.Empty, _sVehicleDealer = string.Empty, _sVehicleType = string.Empty, _sVehileColor = string.Empty;
	private long _editSheetId, changedLastKm;


	protected bool ShowSumary
	{
		get { return _showSumary; }
		set
		{
			_showSumary = value;
			if (_showSumary)
			{
				gvSpareList.EditIndex = -1;
				//btnAdd.Enabled = true;
			}
		}
	}
	// cho phep show in setting section va ko phai la dealer
	protected bool ShowSourceDealerWhenAlarm
	{
		get { return showSourceDealerWhenAlarm && (!UserHelper.IsDealer); }
	}

	protected string CurrentDealer
	{
		get { return (!UserHelper.IsDealer) ? ddlDealer.SelectedValue : UserHelper.DealerCode; }
	}

	protected string CurrentBranch
	{
		get
		{
			string branch = (string.IsNullOrEmpty(UserHelper.BranchCode)) ? ddlBranchCode.SelectedValue : UserHelper.BranchCode;
			return (string.IsNullOrEmpty(branch)) ? CurrentDealer : branch;
		}
	}

	private void SetAvtiveView(int vwIndex)
	{
		if (MultiView1.ActiveViewIndex != vwPrintIndex)
		{
			MultiView1.ActiveViewIndex = vwIndex;
		}
	}
	protected void ShowDealerForm()
	{
		bool isNotDealer = !UserHelper.IsDealer && (!readOnlyForm);
		Literal38.Visible = isNotDealer;
		Literal36.Visible = isNotDealer;
		ddlBranchCode.Visible = isNotDealer;
		ddlDealer.Visible = isNotDealer;
	}
	protected override object SaveControlState()
	{
		object[] ctlState = new object[20];
		ctlState[0] = base.SaveControlState();
		ctlState[1] = _showSumary;
		ctlState[2] = _feeAmount;
		ctlState[3] = _custInfo;
		ctlState[4] = _exchangeNumber;
		ctlState[5] = _finish;
		ctlState[6] = _saved;
		ctlState[7] = _addExchange;
		ctlState[8] = _exchangeHeader;
		ctlState[9] = _isVehicleOnTiptop;
		ctlState[10] = _sVehicleType;
		ctlState[11] = _sVehileColor;
		ctlState[12] = _sVehicleDealer;
		ctlState[13] = _dtVehicleMadedate;
		ctlState[14] = _editSheetId;
		ctlState[15] = _saveTemp;
		ctlState[16] = _exchSaved;
		ctlState[17] = _canChangeBuyDate;
		ctlState[18] = _itemExist;
		ctlState[19] = _sVehicleDB;
		return ctlState;
	}
	protected override void LoadControlState(object state)
	{
		if (state != null)
		{
			object[] ctlState = (object[])state;
			base.LoadControlState(ctlState[0]);
			_showSumary = (bool)ctlState[1];
			_feeAmount = (string)ctlState[2];
			_custInfo = (Customer)ctlState[3];
			_exchangeNumber = (string)ctlState[4];
			_finish = (bool)ctlState[5];
			_saved = (bool)ctlState[6];
			_addExchange = (bool)ctlState[7];
			_exchangeHeader = (Exchangepartheader)ctlState[8];
			_isVehicleOnTiptop = (bool)ctlState[9];
			_sVehicleType = (string)ctlState[10];
			_sVehileColor = (string)ctlState[11];
			_sVehicleDealer = (string)ctlState[12];
			_dtVehicleMadedate = (DateTime)ctlState[13];
			_editSheetId = (long)ctlState[14];
			_saveTemp = (bool)ctlState[15];
			_exchSaved = (bool)ctlState[16];
			_canChangeBuyDate = (bool)ctlState[17];
			_itemExist = (bool)ctlState[18];
			_sVehicleDB = (string)ctlState[19];
		}
	}

	private void ShowError()
	{
		bllErrorMsg.Visible = errorCode.Count > 0;
		bllErrorMsg.Items.Clear();
		foreach (WarrantyContentErrorCode error in errorCode)
		{
			switch (error)
			{
				case WarrantyContentErrorCode.BrokenCodeNotFound: bllErrorMsg.Items.Add(Message.WarrantyContent_BrokenCodeNotFound); break;
				case WarrantyContentErrorCode.EngineNumberNotFound: bllErrorMsg.Items.Add(VDMS.VDMSSetting.CurrentSetting.CheckEngineNoForService ? Message.WarrantyContent_InvalidEngineNumber : Message.WarrantyContent_EngineNumberNotFound); break;
				case WarrantyContentErrorCode.ExchangeNumberNotFound: bllErrorMsg.Items.Add(Message.WarrantyContent_ExchangeNumberNotFound); break;
				case WarrantyContentErrorCode.InvalidDateTimeValue: bllErrorMsg.Items.Add(Message.WarrantyContent_InvalidDateTimeValue); break;
				case WarrantyContentErrorCode.InvalidExchangeSparesList: bllErrorMsg.Items.Add(Message.WarrantyContent_InvalidExchangeSparesList); break;
				case WarrantyContentErrorCode.InvalidServiceType: bllErrorMsg.Items.Add(Message.WarrantyContent_InvalidServiceType); break;
				case WarrantyContentErrorCode.InvalidSpareCode: bllErrorMsg.Items.Add(Message.WarrantyContent_InvalidSpareCode); break;
				case WarrantyContentErrorCode.ItemNotSold: bllErrorMsg.Items.Add(Message.WarrantyContent_ItemNotSold); break;
				case WarrantyContentErrorCode.ItemSoldByOtherDealer: bllErrorMsg.Items.Add(Message.WarrantyContent_ItemSoldByOtherDealer + ((ShowSourceDealerWhenAlarm) ? "  (" + _sVehicleDealer + " : " + DealerHelper.GetName(_sVehicleDealer) + ")" : "")); break;
				case WarrantyContentErrorCode.ItemTypeNotFound: bllErrorMsg.Items.Add(Message.WarrantyContent_ItemTypeNotFound); break;
				case WarrantyContentErrorCode.LastKmChanged: bllErrorMsg.Items.Add(string.Format(Message.WarrantyContent_LastKmChanged, changedLastKm)); break;
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
		if (errorCode.Contains(error)) return;
		errorCode.Add(error);
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
	protected void SetupSpareString()
	{
		if (gvSpareList.DataSourceTable == null) return;
		string name = (Thread.CurrentThread.CurrentCulture.Name == "vi-VN") ? "SpareNameVn" : "SpareNameEn";
		foreach (DataRow row in gvSpareList.DataSourceTable.Rows)
		{
			row["SpareName"] = row[name].ToString();
			if (!string.IsNullOrEmpty(row["ExchangeNumber"].ToString()))
				row["SpareAmount"] = Constants.FreeWarranty;
		}
	}
	protected void CalculateAmount(DataRow newRow, DataRow oldRow)
	{
		long cost, quantity, addedquantity, amount;
		if (newRow == null) return;

		if (oldRow != null) // inc quantity
		{
			long.TryParse(oldRow["Quantity"].ToString(), out quantity);
			long.TryParse(newRow["Quantity"].ToString(), out addedquantity);
			long.TryParse(oldRow["SpareCost"].ToString(), out cost);
			addedquantity += quantity;
			amount = addedquantity * cost;
			addedquantity = (addedquantity > 999) ? 999 : addedquantity;
			oldRow["Quantity"] = addedquantity.ToString();
			oldRow["SpareAmount"] = amount;
		}
		else  //update one
		{
			if (string.IsNullOrEmpty(newRow["ExchangeNumber"].ToString()))
			{
				long.TryParse(newRow["Quantity"].ToString(), out quantity);
				long.TryParse(newRow["SpareCost"].ToString(), out cost);
				amount = quantity * cost;
				newRow["SpareAmount"] = amount;
			}
		}
	}
	protected long CalculateTotalAmount()
	{
		//for (int i = 0; i < i + 1; i++) ; 
		long fee;
		long spareAmount = gvSpareList.SumBy("SpareAmount");
		if (showFeeAmountColumn)
			fee = gvSpareList.SumBy("FeeAmount");
		else
			long.TryParse(_feeAmount, out fee);
		return fee + spareAmount;
	}
	protected string GetTotalAmount()
	{
		NumberFormatInfo ni = Thread.CurrentThread.CurrentCulture.NumberFormat;
		ni.NumberDecimalDigits = 0;
		return CalculateTotalAmount().ToString("N", ni);
	}
	protected bool GetSpareInfo(string number, DataRow row)
	{
		Warrantycondition warr = WarrantyContent.GetWarrantyCondition(number);
		bool ok = false;
		if (warr == null)
		{
			AddError(WarrantyContentErrorCode.InvalidSpareCode); //return false; 
			if (allowInvalidSpare)
			{
				row["SpareNameEn"] = row["SpareName"];
				row["SpareNameVn"] = row["SpareName"];
				ok = true;
			}
		}
		else
		{
			if (string.IsNullOrEmpty(row["SpareCost"].ToString())) row["SpareCost"] = warr.Unitprice;
			row["SpareNameEn"] = warr.Partnameen;
			row["SpareNameVn"] = warr.Partnamevn;
			ok = true;
		}
		return ok;
	}
	protected void ShowCustInfo()
	{
		if (_custInfo != null)
		{
			txtCustAddress.Text = ServiceTools.GetCustAddress(_custInfo);
			txtCusName.Text = _custInfo.Fullname;
			//txtEmail.Text = (_custInfo.Email != ".") ? _custInfo.Email : "";
			txtEmail.Text = _custInfo.Email;
			//txtPhoneNo.Text = (_custInfo.Mobile != ".") ? _custInfo.Mobile : "";
			txtPhoneNo.Text = _custInfo.Mobile;
		}

		txtCustId.Text = (_custInfo != null) ? _custInfo.Identifynumber : "";
	}

	private bool ItemsHasValue(ListItemCollection items, string val)
	{
		foreach (ListItem item in items)
		{
			if (item.Value.ToLower() == val.ToLower()) return true;
		}
		return false;
	}

	private int GetItemsIndex(ListItemCollection items, string val)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].Value.ToLower() == val.ToLower()) return i;
		}
		return -1;
	}

	protected void LoadAllColor()
	{
		if (ddlColour.SelectedIndex <= 0) // dang chon roi thoi ko load lai nua)
		{
			ddlColour.DataSource = Motorbike.GetAllColor(); ddlColour.DataBind();
			ddlColour.Items.Insert(0, new ListItem("", ""));
		}
	}

	// get item's info and fill to mainView.
	// rowIndex: index cua xe duoc chon tren selectItemGridview
	// ret true if vehicle exist in VDMS
	private bool GetItemInfo(string engineNum)
	{
		bool itemExist = false;
		bool tipTopHasThisItem = false;
		//string txt, txt2;
		//int i;

		ItemInstance iis = ItemInstanceDataSource.GetByEngineNumber(engineNum);
		// warranty info
		WarrantyInfo warrInfo = ServiceTools.GetWarrantyInfo(engineNum);

		// tim thong tin tu cac lan sua chua truoc
		Serviceheader serh = (_editSheetId > 0) ? WarrantyContent.GetServiceSheet(_editSheetId).ConvertServiceHeader() : WarrantyContent.FindServiceSheet(hdEngineNumber.Value).ConvertServiceHeader();

		// model and color ----------------------------------------------------------
		//if ((iis != null) || (serh != null))
		{
			#region Model - Color
			if ((iis == null) && (!_isVehicleOnTiptop) && (serh != null))
			{
				_sVehileColor = serh.Colorcode;
				_sVehicleType = serh.Itemtype;
			}

			// khong lay thong tin xe dc (rat kho, nhung da co :))
			if (iis != null)
			{
				if (string.IsNullOrEmpty(_sVehicleType)) _sVehicleType = iis.ItemType;
				if (string.IsNullOrEmpty(_sVehileColor)) _sVehileColor = iis.Color;
			}

			if (!_finishChecking)
			{
				txtModel.Text = _sVehicleType;

				if (string.IsNullOrEmpty(_sVehileColor))
				{
					if (!_finishChecking) LoadAllColor();
					ddlColour.Enabled = true;
					ddlColour.SelectedIndex = 0;
				}
				else
				{
					int i = GetItemsIndex(ddlColour.Items, _sVehileColor);
					if (i >= 0)
					{
						ddlColour.SelectedIndex = i;
						ddlColour.Enabled = false;
					}
					else if (!string.IsNullOrEmpty(_sVehileColor))
					{
						ddlColour.Items.Clear();
						ddlColour.Items.Add(new ListItem(_sVehileColor, _sVehileColor));
					}
				}
			}
			#endregion
		}

		// repair history----------------------------------------------------------
		#region repair history
		if ((serh != null) /*|| (iis != null)*/)
		{
			btnHistory.Enabled = true;
			btnHistory.OnClientClick = "window.open('repairhistory.aspx?engnum=" + engineNum + "','repairhis',''); return false;";
		}
		else
		{
			btnHistory.Enabled = false;
			btnHistory.OnClientClick = "";
		}

		// cust invoice info /*****/
		var cusInv = WarrantyContent.GetCustInvoiceInfos(engineNum);
		bool custInvNotExist = cusInv == null;
		#endregion

		// thong tin khach hang----------------------------------------------------------
		#region Customer
		if (_custInfo == null)
		{
			if (serh != null)
			{
				_custInfo = serh.Customer;
			}
			else if ((warrInfo != null) && (warrInfo.Customer != null))
			{
				_custInfo = warrInfo.Customer.ConvertCustomer();
			}
			else if (!custInvNotExist)
			{
				_custInfo = cusInv.Customer.ConvertCustomer();
			}
			else if (!_finishChecking) // lan dau tien nhap them KH (tru` luc chuan bi ket thuc: vua nhap KH xong => khong xoa)
			{
				txtCusName.Text = ""; txtCustAddress.Text = "";
				txtEmail.Text = ""; txtPhoneNo.Text = "";
			}
		}
		ShowCustInfo();
		#endregion

		// so khung, bien so ----------------------------------------------------------
		if ((serh != null) && (!_finishChecking))
		{
			if (!string.IsNullOrEmpty(serh.Framenumber)) txtFrameNumber.Text = serh.Framenumber;
			if (!string.IsNullOrEmpty(serh.Numberplate)) txtNumberPlate.Text = serh.Numberplate;
			SetReadOnly(txtNumberPlate, !string.IsNullOrEmpty(txtNumberPlate.Text));
			SetReadOnly(txtFrameNumber, !string.IsNullOrEmpty(txtFrameNumber.Text));
		}

		// get KmCount, buy date----------------------------------------------------------
		#region KM-Buy date
		if (serh != null)
		{
			if ((readOnlyForm || (_editSheetId > 0)) && (!_finishChecking))
			{
				txtKm.Text = serh.Kmcount.ToString();
			}
			txtLastKm.Text = (warrInfo != null) ? warrInfo.KmCount.ToString() : "";
		}
		else if (warrInfo != null)
		{
			txtLastKm.Text = warrInfo.KmCount.ToString();
		}
		else
		{
			txtLastKm.Text = "0";
		}

		DateTime buyDate = DateTime.MinValue;
		if (string.IsNullOrEmpty(txtBuyDate.Text))
		{
			if (warrInfo != null)
			{
				buyDate = warrInfo.PurchaseDate;
			}
			else if (!custInvNotExist)
			{
				buyDate = cusInv.SellDate;
			}
			else if (serh != null)
			{
				buyDate = serh.Purchasedate;
			}
		}
		if (buyDate > DateTime.MinValue) txtBuyDate.Text = buyDate.ToShortDateString();
		#endregion

		// lay duoc thong tin xe tren tiptop => thoi ko alarm nua
		itemExist = (iis != null) || tipTopHasThisItem || _isVehicleOnTiptop;
		_canChangeBuyDate = (!itemExist) || (buyDate == DateTime.MinValue);

		//// canh bao xe chua duoc ban
		//if ((iis != null) && (iis.Status != (int)ItemStatus.Sold))
		//{
		//    AddError(WarrantyContentErrorCode.ItemNotSold);
		//}
		//else 

		// canh bao dai ly ban xe va dai ly sua la khac nhau
		if ((_sVehicleDealer != CurrentDealer) && (!readOnlyForm) && (itemExist))
		{
			AddError(WarrantyContentErrorCode.ItemSoldByOtherDealer);
		}

		_itemExist = itemExist;
		return itemExist;
	}

	private void CollectItemInfos(int index)
	{
		HiddenField hdMadeDate = (HiddenField)WebTools.FindControlById("hdMadedate", gvSelectItem.Rows[index]);
		if (hdMadeDate != null) { DateTime.TryParse(hdMadeDate.Value, out _dtVehicleMadedate); };

		Literal litItemType = (Literal)WebTools.FindControlById("litSelectedSoldItemModel", gvSelectItem.Rows[index]);
		if (litItemType != null) { _sVehicleType = litItemType.Text; };

		HiddenField hdItemDealerCode = (HiddenField)WebTools.FindControlById("hdDealerCode", gvSelectItem.Rows[index]);
		if (hdItemDealerCode != null) { _sVehicleDealer = hdItemDealerCode.Value; };

		HiddenField litItemColor = (HiddenField)WebTools.FindControlById("hdItemColorCode", gvSelectItem.Rows[index]);
		if (litItemColor != null) { _sVehileColor = litItemColor.Value; };

		HiddenField hdDBCode = (HiddenField)WebTools.FindControlById("hdDBcode", gvSelectItem.Rows[index]);
		if (hdDBCode != null) { _sVehicleDB = hdDBCode.Value; };
	}

	// lookup incomplete engineNumber and show list to select
	// if 1 item return, set it to engineNumber textbox 
	// if no item found => return error;
	private bool CheckEngineNumber(string engineNumber, bool mustSelect) // mustSelect: select from many
	{
		odsSelectItem.SelectParameters["engineNumberLike"] = new Parameter("engineNumberLike", TypeCode.String, engineNumber);
		gvSelectItem.PageIndex = 0;
		gvSelectItem.DataBind();
		bool exist = false;
		int id = -1;

		if ((gvSelectItem.Rows.Count == 1) || ((gvSelectItem.Rows.Count > 1) && (!mustSelect)))
		{
			if (gvSelectItem.Rows.Count == 1)
			{
				Literal lit = (Literal)WebTools.FindControlById("litSelectedSoldItem", gvSelectItem.Rows[0]);
				if (lit != null) engineNumber = lit.Text;
				txtEngineNo.Text = engineNumber;

				HiddenField hdTiptop = (HiddenField)WebTools.FindControlById("hdTiptop", gvSelectItem.Rows[0]);
				if (hdTiptop != null)
				{
					_isVehicleOnTiptop = int.TryParse(hdTiptop.Value, out id) && (id == 0);
				}

				CollectItemInfos(0);
			}

			exist = GetItemInfo(engineNumber);
		}
		else if (gvSelectItem.Rows.Count > 1)
		{
			SetAvtiveView(vwSelectItemIndex);
			//btnCalculate.Enabled = false;     // ??????
			//btnAddExchange.Enabled = false;   // ??????
		}
		else    // item not found
		{
			exist = GetItemInfo(engineNumber);
			if (!exist)
			{
				SetAvtiveView(vwMainIndex);
				if (string.IsNullOrEmpty(hdEngineNumber.Value))
					AddError(WarrantyContentErrorCode.NoItemSold);
				else
					AddError(WarrantyContentErrorCode.EngineNumberNotFound);
			}
		}

		//btnHistory.Enabled = true;
		return exist; // found atleast 1 item
	}
	private void CheckFinish() // enable/disable base on finish state
	{
		_finishChecking = true;
		if (_finish)
		{
			if (!readOnlyForm) btnCheck_Click(null, null);
			ShowSumary = true;
			gvSpareList.ShowFooter = true;
		}
		//SetReadOnly(Panel1, _finish);
		SetReadOnly(ddlBranchCode, _finish);
		SetReadOnly(ddlDealer, _finish);
		SetReadOnly(btnCheck, _finish);
		SetReadOnly(btnAddExchange, _finish);
		SetReadOnly(btnPrint, !(_finish || allWayPrint));  // print SRS
		SetReadOnly(btnCheckModel, _finish);
		SetReadOnly(Button4, _finish); // select cust
		SetReadOnly(txtEngineNo, _finish);
		SetReadOnly(ddlColour, _finish);
		SetReadOnly(txtKm, _finish);
		SetReadOnly(txtModel, _finish);
		SetReadOnly(txtNumberPlate, _finish);
		SetReadOnly(txtFrameNumber, _finish);
		if (_canChangeBuyDate || _finish)
		{
			SetReadOnly(txtBuyDate, _finish);
			SetReadOnly(ibtnCalendar, _finish);
		}
		SetReadOnly(txtRepairDate, _finish);
		SetReadOnly(ibtnCalendarR, _finish);

		SetReadOnly(chblSerList, _finish);
		SetReadOnly(txtErrorStatus, _finish);
		SetReadOnly(txtRepair, _finish);
		//TextBox tb = (TextBox)WebTools.FindControlById("txtFee", gvSpareList);
		//SetReadOnly((WebControl)tb, _finish);

		btnSave.Enabled = _finish;
		//btnSaveTemp.Enabled = _finish;

		_finishChecking = false;
	}
	protected int GetSelectedServices()
	{
		int sers = 0;
		foreach (ListItem item in chblSerList.Items) { if (item.Selected) sers += Convert.ToInt32(item.Value); }
		return sers;
	}
	private bool SaveExchangeSpares(ServiceStatus status, Exchangepartheader exchH, Serviceheader serH, DataTable spareList, long totalFee)
	{
		WarrantyContentErrorCode err = WarrantyContentErrorCode.OK;
		_exchSaved = false;

		// check for any exchange spares exist
		// ret true if no one to save
		if ((exchH == null) || (spareList == null) || (spareList.Rows.Count < 1)) return true;

		// save header
		for (int i = 0; i < 5; i++) // try 5 time to ensure
		{
			err = AddExchange.SaveExchHeader(status, UserHelper.AreaCode, CurrentDealer, exchH, serH, totalFee);
			if (err == WarrantyContentErrorCode.OK) break;
		}
		if (err != WarrantyContentErrorCode.OK) { AddError(err); return false; }

		// save detail
		err = AddExchange.SaveExchDetails(spareList, exchH);
		if (err != WarrantyContentErrorCode.OK) { AddError(err); return false; }

		_exchSaved = true;
		return true;
	}
	private bool SaveSheet(ServiceStatus status)
	{
		// check service type
		//if (chblSerList.SelectedIndex < 0) { AddError(WarrantyContentErrorCode.InvalidServiceType); return false; }

		////////////////////////////////////////
		int serviceType = 0;
		long km, fee, total;
		DateTime serDate, buyDate;
		DateTimeFormatInfo dti = Thread.CurrentThread.CurrentCulture.DateTimeFormat;
		WarrantyContentErrorCode error = WarrantyContentErrorCode.OK;
		string model, plateNumber, frameNumber, errStatus, solution, color;

		// colect header infos
		model = txtModel.Text;
		plateNumber = txtNumberPlate.Text;
		frameNumber = txtFrameNumber.Text;
		errStatus = txtErrorStatus.Text;
		solution = txtRepair.Text;
		total = CalculateTotalAmount();
		color = ddlColour.SelectedValue;

		DateTime.TryParse(txtRepairDate.Text, dti, DateTimeStyles.AllowWhiteSpaces, out serDate);
		DateTime.TryParse(txtBuyDate.Text, dti, DateTimeStyles.AllowWhiteSpaces, out buyDate);

		long.TryParse(txtKm.Text, out km);
		long.TryParse(_feeAmount, out fee);
		serviceType = GetSelectedServices();

		using (TransactionBlock trans = new TransactionBlock())
		{
			// check last km count before save
			WarrantyInfo warrInfo = ServiceTools.GetWarrantyInfo(hdEngineNumber.Value);
			long.TryParse(txtLastKm.Text, out changedLastKm);
			if ((warrInfo != null) && (changedLastKm != warrInfo.KmCount))
			{
				AddError(WarrantyContentErrorCode.LastKmChanged);
				_finish = false;
				CheckFinish();
				txtLastKm.Text = warrInfo.KmCount.ToString();
				return false;
			}

			// save service header                                                  // ddlBranchCode.SelectedValue  
			Serviceheader serH = WarrantyContent.SaveSerHeader(_editSheetId, status, CurrentDealer, CurrentBranch, hdEngineNumber.Value, plateNumber, frameNumber, model, color, errStatus, solution, _exchangeNumber, serviceType, _custInfo, km, fee, total, serDate, buyDate, out error);
			if ((serH == null) || (error != WarrantyContentErrorCode.OK)) { AddError(error); trans.IsValid = false; return false; }

			// save service details
			error = WarrantyContent.SaveSerDetails(gvSpareList.DataSourceTable, serH);
			if (error != WarrantyContentErrorCode.OK) { AddError(error); trans.IsValid = false; return false; }
			_editSheetId = serH.Id;

			// save exchange spares (also addErr)
			trans.IsValid = SaveExchangeSpares(status, _exchangeHeader, serH, AddExchange1.SpareList, AddExchange1.TotalFee);

			// save warranty info 
			if ((_itemExist) && (status != ServiceStatus.Temp))
			{
				// cho nay ko update customer
				if (!ServiceTools.SaveWarrantyInfo(hdEngineNumber.Value, Convert.ToInt32(km), buyDate, _sVehicleDB, model, color, _sVehicleDealer, 0))
				{
					AddError(WarrantyContentErrorCode.UpdateDataFailed);
					trans.IsValid = false; return false;
				}
			}

			// show real sheet no
			txtSheetNo.Text = serH.Servicesheetnumber;
			// refresh exchange items with saved VoucherNumber
			if ((_exchangeHeader != null) && (AddExchange1.SpareList != null) && (AddExchange1.SpareList.Rows.Count > 0)) CopyExchangeList();
		}

		return true;
	}

	private DataRow GetExchangeRow()
	{
		if (gvSpareList.DataSourceTable == null) return null;
		foreach (DataRow row in gvSpareList.DataSourceTable.Rows)
		{
			if (!string.IsNullOrEmpty(row["IsExchangeSpare"].ToString())) return row;
		}
		return null;
	}
	private void CopyExchangeList()
	{
		DataRow row;
		if ((gvSpareList.DataSourceTable == null) || (_exchangeHeader == null)) { AddError(WarrantyContentErrorCode.InvalidExchangeSparesList); return; }

		// remove all current exchange row
		row = GetExchangeRow();
		while (row != null)
		{
			gvSpareList.DataSourceTable.Rows.Remove(row);
			row = GetExchangeRow();
		}

		// copy new rows from Add exchange form
		if (AddExchange1.SpareList == null) return;
		foreach (DataRow xRow in AddExchange1.SpareList.Rows)
		{
			row = gvSpareList.DataSourceTable.NewRow();
			row["SpareNumber"] = xRow["SpareNumber"];
			row["SpareNameEn"] = xRow["SpareNameEn"];
			row["SpareNameVn"] = xRow["SpareNameVn"];
			row["SpareName"] = xRow["SpareName"];
			row["Quantity"] = xRow["Quantity"];
			row["SpareCost"] = xRow["SpareCost"];
			row["ExchangeNumber"] = _exchangeHeader.Vouchernumber;
			row["SpareAmount"] = Constants.FreeWarranty;
			row["IsExchangeSpare"] = "true";
			gvSpareList.DataSourceTable.Rows.InsertAt(row, 0);
		}
	}
	private void ClearForm()
	{
		txtModel.Text = ""; txtFrameNumber.Text = "";
		txtKm.Text = ""; txtNumberPlate.Text = "";
		txtCusName.Text = ""; txtEmail.Text = "";
		txtCustAddress.Text = ""; txtPhoneNo.Text = "";
		txtLastKm.Text = "0"; txtBuyDate.Text = "";
		txtErrorStatus.Text = ""; txtRepair.Text = "";
		txtCustId.Text = "";
		txtSheetNo.Text = (showTempSheetNo) ? WarrantyContent.GenSheetNumber(CurrentDealer) : "";

		if (!itemSelecting)
		{
			_dtVehicleMadedate = DateTime.MinValue;
			_sVehicleType = string.Empty;
			_sVehileColor = string.Empty;
			_sVehicleDealer = string.Empty;
			_sVehicleDB = string.Empty;
			_isVehicleOnTiptop = false;
		}

		_feeAmount = "0";

		LoadAllColor();
		if (gvSpareList.DataSourceTable != null) gvSpareList.DataSourceTable.Rows.Clear();
		AddExchange1.ClearForm();
	}

	protected void GetBranchs()
	{
		// list branch
		ddlBranchCode.DataSource = Dealer.GetListBranchOfDealer(CurrentDealer);
		ddlBranchCode.DataBind();
	}
	protected void Page_Init(object sender, EventArgs e)
	{
		Page.RegisterRequiresControlState(this);
	}
	protected void Page_PreRender(object sender, EventArgs e)
	{
		ShowError();
		SetupSpareString();
		btnAdd.Enabled = ShowSumary && (!_finish);

		if (MultiView1.ActiveViewIndex == vwMainIndex) gvSpareList.DataBind();
		if (MultiView1.ActiveViewIndex == vwAddExchangeIndex) AddExchange1.ChangeLanguage();

		// print parts change button
		btnPrintPcv.OnClientClick = string.Format("window.open('Report/PrintPartChange.aspx?pcvn={0}','printExchangeList',''); return false;", (_exchangeHeader != null) ? _exchangeHeader.Vouchernumber : "");

		//btnCalculate.Enabled = !_saved;

		// da save roi thi khong dc change enginnumber 
		if (_editSheetId > 0)
		{
			//SetReadOnly(btnCheck, true); //bo cai nay di de cho phep check(trong truong hop chua check da luu)
			SetReadOnly(txtEngineNo, true);
		}

		btnCalculate.Text = (_finish) ? Constants.EditSheet : Constants.FinishInput;
		btnSave.Text = (_saved) ? Constants.AddNewSheet : Constants.Save;
		btnSave.OnClientClick = (_saved) ? "" : "return SubmitConfirm(" + btnSave.ClientID + ",'" + Resources.Question.SaveData + "');";
		btnSaveTemp.OnClientClick = btnSave.OnClientClick;

		txtCustId.Text = ""; // force always add new cust
		CheckReadOnlyForm();
		ShowDealerForm();
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		errorCode.Clear();
		bllErrorMsg.Items.Clear();

		if (!IsPostBack)
		{
			btnHistory.OnClientClick = "window.open('repairhistory.aspx?engnum=','repairhis',''); return false;";
			btnList.OnClientClick = "window.open('repairlist.aspx','repairlist',''); return false;";
			//ddlBranchCode.DataTextField = "BranchName";
			ddlBranchCode.DataTextField = "BranchCode";
			ddlBranchCode.DataValueField = "BranchCode";
			//ddlBranchCode.Items.Insert(0, new ListItem("", ""));

			ddlDealer.DataTextField = "BranchCode";
			ddlDealer.DataValueField = "BranchCode";
			ddlDealer.DataSource = Dealer.GetListDealerByDatabase(UserHelper.DatabaseCode);

			ddlDealer.DataBind();
			GetBranchs();

			txtBuyDate.Text = DateTime.Now.ToShortDateString();
			txtRepairDate.Text = DateTime.Now.ToShortDateString();
			SetAvtiveView(vwMainIndex);
			gvSpareList.DataSourceTable = WarrantyContent.SpareListOnServiceSchema;
			gvSpareList.EmptyTableRowText = Message.WarrantyContent_EmptySpareList;
			gvSelectCust.EmptyDataText = Message.DataNotFound;

			gvSpareList.Columns[7].Visible = showFeeAmountColumn;
			ShowSumary = false;
			_finish = false;
			_saved = false;
			_saveTemp = false;
			_exchSaved = false;
			_addExchange = false;
			_canChangeBuyDate = false;

			gvSelectItem.PageSize = selectGridViewPageSize;
			gvSelectSpare.PageSize = selectSparePageSize;
			gvSelectModel.PageSize = selectGridViewPageSize;
			gvSelectBroken.PageSize = selectBrokenCodePageSize;
			gvSelectCust.PageSize = selectGridViewPageSize;


			ClearForm();

			// add new customer /////////////////
			string js = "if(retrieve_lookup_data('" + txtCustId.ClientID + "','../vehicle/sale/CusInfInput2.aspx?')==false) return false;";
			btnNewCus.OnClientClick = js;
			btnNewCus.UseSubmitBehavior = false;

			/////////////////////////////////////
			// SRS id to be edit
			string srsId = Request.QueryString["sid"];
			long.TryParse(srsId, out _editSheetId);

			// load temp SRS
			if (!string.IsNullOrEmpty(srsId))
			{
				ShowServiceSheet(false, _editSheetId);
			}

			// check km count for dealer
			CompareValidator1.ValidationGroup = (!UserHelper.IsDealer) ? "None" : "Save";
		}
		else
		{
			// bind data for frint form
			if (MultiView1.ActiveViewIndex == vwPrintIndex) btnPrint_Click(null, null);

			// keep tracking PVC
			if ((AddExchange1.ExchangeHeader == null) && (_exchangeHeader != null))
			{
				AddExchange1.ExchangeHeader = _exchangeHeader;
			}
		}

		_repairSpare = "";
		if (AddExchange1.OnFindSpareClick == null) AddExchange1.OnFindSpareClick += btnFindSpare_Click;
		if (AddExchange1.OnFindBrokenClick == null) AddExchange1.OnFindBrokenClick += btnFindBroken_Click;
		if (AddExchange1.OnCancelSaveListClick == null) AddExchange1.OnCancelSaveListClick += btnCancel_Click;
		if (AddExchange1.OnSaveListClick == null) AddExchange1.OnSaveListClick += btnSaveExchange_Click;
		if (AddExchange1.AddNewError == null) AddExchange1.AddNewError += AddError;

		// check and store sparenumber
		TextBox tb = (TextBox)WebTools.FindControlById("txtSpareNumber", gvSpareList);
		if (tb != null) _repairSpare = tb.Text;

		// check and store fee
		if (showFeeAmountColumn)
			_feeAmount = gvSpareList.SumBy("FeeAmount").ToString();
		else
		{
			tb = (TextBox)WebTools.FindControlById("txtFee", gvSpareList);
			if (tb != null)
			{
				if ((tb.Text.Trim() != _feeAmount) && (!string.IsNullOrEmpty(tb.Text))) _feeAmount = tb.Text.Trim();
				else if (string.IsNullOrEmpty(_feeAmount)) _feeAmount = "0";
				//if (string.IsNullOrEmpty(tb.Text)) tb.Text = _feeAmount;
			}
		}

		//CheckFinish();
		footWarranty = null;

		// show data from other page's query
		showServiceSheetNumber = Request.QueryString["srsn"];
		showExchangeSparesNumber = Request.QueryString["pcvn"];
		if (!string.IsNullOrEmpty(showServiceSheetNumber)) ShowServiceSheet(true, showServiceSheetNumber);
		else if (!string.IsNullOrEmpty(showExchangeSparesNumber)) ShowExchangeVoucher();

		//print Mes4lang
		LoadCustomerErr = Resources.Message.LoadCustomerErr;
	}

	private bool ListContainsValue(ListItemCollection items, string value)
	{
		foreach (ListItem item in items)
		{
			if (item.Value == value) return true;
		}
		return false;
	}
	private void CheckReadOnlyForm()
	{
		btnPrint.Enabled = (_saved || _saveTemp || readOnlyForm || allWayPrint);  // print SRS
		//btnBackFromPrint.Enabled = !readOnlyForm;
		btnPrintPcv.Enabled = (_saved || (_saveTemp && _exchSaved) || readOnlyForm) &&
			(_exchangeHeader != null);
		btnCheck.Visible = !readOnlyForm;
		btnSave.Visible = !readOnlyForm;
		btnSaveTemp.Visible = !readOnlyForm;
		btnAddExchange.Visible = !readOnlyForm;
		btnAdd.Visible = !readOnlyForm;
		btnCheckModel.Visible = !readOnlyForm;
		btnCalculate.Visible = !readOnlyForm;
		AddExchange1.readOnlyMode = readOnlyForm;
	}

	private void ShowServiceSheet(bool readOnly, string SheetNumber)
	{
		Serviceheader serH = RepairListDataSource.GetServiceHeader(SheetNumber);

		if (
			(serH == null) ||
			(UserHelper.IsDealer && (serH.Dealercode != UserHelper.DealerCode))
		   )
		{
			AddError(WarrantyContentErrorCode.ServiceSheetNumberNotFound);
			SetAvtiveView(vwNoSheetToViewIndex);
		}
		else
		{
			ShowServiceSheet(readOnly, serH);
			SetAvtiveView(vwMainIndex);
		}
	}
	private void ShowServiceSheet(bool readOnly, long SheetId)
	{
		IDao<Serviceheader, long> dao = DaoFactory.GetDao<Serviceheader, long>();
		Serviceheader serH = dao.GetById(SheetId, false);

		if (
			(serH == null) ||
			(serH.Status != (int)ServiceStatus.Temp) ||
			(UserHelper.IsDealer && (serH.Dealercode != UserHelper.DealerCode))
		   )
		{
			AddError(WarrantyContentErrorCode.ServiceSheetNumberNotFound);
			SetAvtiveView(vwNoSheetToViewIndex);
		}
		else
		{
			ShowServiceSheet(readOnly, serH);
			SetAvtiveView(vwMainIndex);
		}
	}
	private void ShowServiceSheet(bool readOnly, Serviceheader serH)
	{
		_finish = readOnly;
		readOnlyForm = readOnly;
		AddExchange1.readOnlyMode = readOnly;

		if (serH != null)
		{
			//_exchangeHeader = RepairListDataSource.GetExchangeHeaderFromServiceSheet(serH.Id);

			#region header section
			// warranty info
			// WarrantyInfo warrInfo = ServiceTools.GetWarrantyInfo(serH.Enginenumber);

			_sVehileColor = serH.Colorcode;
			_sVehicleDealer = serH.Dealercode;
			_sVehicleType = serH.Itemtype;
			_feeAmount = serH.Feeamount.ToString();

			txtSheetNo.Text = serH.Servicesheetnumber;
			txtEngineNo.Text = serH.Enginenumber;

			txtErrorStatus.Text = serH.Damaged;
			txtRepair.Text = serH.Repairresult;

			viewmodeDealer = serH.Dealercode;
			if (ListContainsValue(ddlDealer.Items, viewmodeDealer)) ddlDealer.SelectedValue = viewmodeDealer;
			else ddlDealer.Items.Clear();
			GetBranchs();

			btnCheck_Click(null, null); // donot clear
			EnableButtonAfterCheck(true, true);

			// service type
			bool maint = false, rep = false, warr = false;
			switch ((ServiceType)serH.Servicetype)
			{
				case ServiceType.Repair: rep = true; break;
				case ServiceType.Warranty: warr = true; break;
				case ServiceType.Maintain: maint = true; break;
				case ServiceType.MaintainAndRepair: maint = true; rep = true; break;
				case ServiceType.MaintainAndWarranty: maint = true; warr = true; break;
				case ServiceType.WarrantyAndRepair: warr = true; rep = true; break;
				case ServiceType.RepairAndMaintainAndWarranty: maint = true; rep = true; warr = true; break;
			}
			chblSerList.Items[0].Selected = maint; chblSerList.Items[1].Selected = rep; chblSerList.Items[2].Selected = warr;
			#endregion

			gvSpareList.DataSourceTable.Rows.Clear();

			#region repair section
			_feeAmount = serH.Feeamount.ToString();

			// service detail: spares
			RepairListDataSource ds = new RepairListDataSource();
			ArrayList serDs = ds.GetServiceDetails(serH.Id);
			for (int i = 0; i < serDs.Count; i++)
			{
				Servicedetail serD = (Servicedetail)serDs[i];
				DataRow row = gvSpareList.DataSourceTable.NewRow();
				Warrantycondition spareInfo = WarrantyContent.GetWarrantyCondition(serD.Partcode);

				row["SpareNo"] = i + 1;
				row["SpareNumber"] = serD.Partcode;
				row["Quantity"] = serD.Partqty;
				row["SpareCost"] = serD.Unitprice;
				row["IsExchangeSpare"] = string.Empty;
				row["ItemId"] = serD.Id;

				if (spareInfo != null)
				{
					row["SpareNameEn"] = spareInfo.Partnameen;
					row["SpareNameVn"] = spareInfo.Partnamevn;
					row["SpareName"] = (Thread.CurrentThread.CurrentCulture.Name == "vi-VN") ? spareInfo.Partnamevn : spareInfo.Partnameen;
				}
				else
				{
					row["SpareNameEn"] = serD.Partname;
					row["SpareNameVn"] = serD.Partname;
					row["SpareName"] = serD.Partname;
				}

				CalculateAmount(row, null);
				gvSpareList.DataSourceTable.Rows.Add(row);
			}
			#endregion

			#region exchangeSection
			if (ds == null) ds = new RepairListDataSource();
			Exchangepartheader exch = ds.GetExchangeHeader(serH.Id);
			_exchangeHeader = exch;
			if (exch != null)
			{
				AddExchange1.ExchangeHeader = _exchangeHeader;
				AddExchange1.InitUC();
				AddExchange1.SpareList.Rows.Clear();

				ArrayList exchDs = ds.GetExchangeDetails(exch.Id);
				AddExchange1.RawSpareList = AddExchange.SpareListOnServiceSchema;

				for (int i = 0; i < exchDs.Count; i++)
				{
					Exchangepartdetail exchD = (Exchangepartdetail)exchDs[i];
					Warrantycondition spareInfo = WarrantyContent.GetWarrantyCondition(exchD.Partcodem);

					#region Display on SRS
					DataRow row = gvSpareList.DataSourceTable.NewRow();
					row["SpareNo"] = i + 1;
					row["SpareNumber"] = exchD.Partcodem;
					row["Quantity"] = exchD.Partqtym;
					row["SpareCost"] = exchD.Unitpricem;
					row["ExchangeNumber"] = exch.Vouchernumber;
					row["IsExchangeSpare"] = "true";
					row["ItemId"] = exchD.Id;
					if (spareInfo != null)
					{
						row["SpareNameEn"] = spareInfo.Partnameen;
						row["SpareNameVn"] = spareInfo.Partnamevn;
						row["SpareName"] = (Thread.CurrentThread.CurrentCulture.Name == "vi-VN") ? spareInfo.Partnamevn : spareInfo.Partnameen;
					}
					else
					{
						row["SpareNameEn"] = Message.DataLost;
						row["SpareNameVn"] = Message.DataLost;
						row["SpareName"] = Message.DataLost;
					}

					//_feeAmount = exch.Feeamount.ToString();
					//CalculateAmount(row, null);
					gvSpareList.DataSourceTable.Rows.InsertAt(row, 0);
					#endregion

					#region display on PCV
					row = AddExchange1.RawSpareList.NewRow();

					row["ItemId"] = exchD.Id;
					row["SpareNo"] = i + 1;
					row["SpareNumber"] = exchD.Partcodem;
					row["Quantity"] = exchD.Partqtym;
					row["BrokenCode"] = exchD.Broken.Brokencode;
					row["SerialNumber"] = exchD.Serialnumber;

					AddExchange1.AddNewItem(row);
					AddExchange1.RawSpareList.Rows.Add(row);
					#endregion
				}
			}
			#endregion

			CalculateTotalAmount();
			gvSpareList.DataBind();
			CheckFinish();

			// cust info n' buy date
			_custInfo = serH.Customer;
			ShowCustInfo();
			txtRepairDate.Text = (serH.Servicedate > DateTime.MinValue) ? serH.Servicedate.ToShortDateString() : "";
		}
	}

	private void ShowExchangeVoucher()
	{
		readOnlyForm = true;
		AddExchange1.readOnlyMode = readOnlyForm;

		Exchangepartheader exH = RepairListDataSource.GetExchangeHeader(showExchangeSparesNumber);
		_exchangeHeader = exH;
		if (
			(exH == null) ||
			(UserHelper.IsDealer && (exH.Dealercode != UserHelper.DealerCode))
		   )
		{
			AddError(WarrantyContentErrorCode.ExchangeNumberNotFound);
			SetAvtiveView(vwNoSheetToViewIndex);
		}
		else
		{
			viewmodeDealer = exH.Dealercode;
			AddExchange1.ExchangeHeader = exH;
			AddExchange1.InitUC();
			RepairListDataSource ds = new RepairListDataSource();
			ArrayList exDs = ds.GetExchangeDetails(exH.Id);
			if ((exDs != null) && (exDs.Count > 0))
			{
				DataTable tbl = AddExchange.SpareListOnServiceSchema;
				foreach (Exchangepartdetail exD in exDs)
				{
					DataRow row = tbl.NewRow();
					AddExchange1.GetSpareInfo(exD.Partcodem, row);
					row["Quantity"] = exD.Partqtym;
					row["SpareCost"] = exD.Unitpricem;
					row["BrokenCode"] = exD.Broken.Brokencode;
					row["SerialNumber"] = exD.Serialnumber;
					AddExchange1.CalculateAmount(row, null);
					tbl.Rows.Add(row);
				}
				AddExchange1.RawSpareList = tbl;
			}
			SetAvtiveView(vwAddExchangeIndex);
		}
	}
	protected void EnableButtonAfterCheck(bool itemExist, bool all)
	{
		SetReadOnly(txtModel, itemExist);
		btnCheckModel.Enabled = !itemExist;
		btnAddExchange.Enabled = !VDMS.VDMSSetting.CurrentSetting.CheckEngineNoForPartsChange || itemExist;
		btnCalculate.Enabled = !VDMS.VDMSSetting.CurrentSetting.CheckEngineNoForService || itemExist;

		// set input status only when user click
		if (all)
		{
			SetReadOnly(txtBuyDate, !_canChangeBuyDate);
			SetReadOnly(ibtnCalendar, !_canChangeBuyDate);
		}
	}
	protected void btnCheck_Click(object sender, EventArgs e)
	{
		string newEngine = txtEngineNo.Text.Trim().ToUpper();
		if (hdEngineNumber.Value != newEngine) _custInfo = null;

		hdEngineNumber.Value = newEngine;
		SetReadOnly(txtNumberPlate, false);
		SetReadOnly(txtFrameNumber, false);
		SetReadOnly(txtBuyDate, false);
		SetReadOnly(ibtnCalendar, false);
		btnNewCus.Enabled = true;

		// clear form when check a enginenumber
		// (sender == null) => manual check from check finish => donot clear
		if (sender != null) { ClearForm(); }

		bool itemExist = CheckEngineNumber(hdEngineNumber.Value, (sender != null) && (e != null));

		EnableButtonAfterCheck(itemExist, sender != null);

		itemSelecting = false;
	}
	// user cancel select an item
	protected void btnCancel_Click(object sender, EventArgs e)
	{
		if ((_addExchange) && (MultiView1.ActiveViewIndex == vwAddExchangeIndex))
		{
			SetAvtiveView(vwMainIndex);
			_addExchange = false;
		}
		else if (_addExchange)
		{
			SetAvtiveView(vwAddExchangeIndex);
			if (AddExchange1.ExchangeHeader != null) _exchangeHeader = AddExchange1.ExchangeHeader;
		}
		else { SetAvtiveView(vwMainIndex); }
	}

	// user select an item. set it to engineNumber textbox 
	protected void gvSelectItem_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		itemSelecting = true;
		GridViewRow row = gvSelectItem.Rows[e.NewSelectedIndex]; if (row == null) return;
		Literal lit = (Literal)WebTools.FindControlById("litSelectedSoldItem", row); if (lit == null) return;

		CollectItemInfos(e.NewSelectedIndex);
		txtEngineNo.Text = lit.Text;
		btnCheck_Click(btnCheck, null);

		SetAvtiveView(vwMainIndex);
	}
	// select spare
	protected void gvSelectSpare_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		GridViewRow gvr = ((GridView)sender).Rows[e.NewSelectedIndex]; if (gvr == null) return;
		Literal lit = (Literal)WebTools.FindControlById("litSelectedSpareNumber", gvr);
		if (lit != null) footWarranty = WarrantyContent.GetWarrantyCondition(lit.Text);
		if (_addExchange) SetAvtiveView(vwAddExchangeIndex); else SetAvtiveView(vwMainIndex);
		if (_addExchange) AddExchange1.AddSpareTofoot(footWarranty);
	}
	protected void gvSelectBroken_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		GridViewRow gvr = ((GridView)sender).Rows[e.NewSelectedIndex]; if (gvr == null) return;
		Literal lit = (Literal)WebTools.FindControlById("litSelectedBrokenCode", gvr); if (lit == null) return;
		footBroken = WarrantyContent.GetBroken(lit.Text);
		if (_addExchange) SetAvtiveView(vwAddExchangeIndex); else SetAvtiveView(vwMainIndex);
		if (_addExchange) AddExchange1.AddBrokenTofoot(footBroken);
	}
	protected void gvSelectModel_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		GridViewRow gvr = ((GridView)sender).Rows[e.NewSelectedIndex];
		Literal lit = (Literal)WebTools.FindControlById("litSelectedModel", gvr); if (lit == null) return;
		txtModel.Text = lit.Text;
		SetAvtiveView(vwMainIndex);
	}

	protected void gvSpareList_RowEditing(object sender, GridViewEditEventArgs e)
	{
		gvSpareList.ShowFooter = false;
	}
	protected void gvSpareList_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		DataRow newRow = gvSpareList.NewSimpleRow;
		if ((newRow == null)) { AddError(WarrantyContentErrorCode.UpdateDataFailed); return; }

		//newRow["ExchangeNumber"] = newRow["ExchangeNumber"].ToString().Trim().ToUpper();
		GetSpareInfo(newRow["SpareNumber"].ToString(), newRow);
		if (errorCode.Contains(WarrantyContentErrorCode.InvalidSpareCode) && (errorCode.Count == 1) && allowInvalidSpare)
		{ }
		else if (errorCode.Count > 0) { e.Cancel = true; return; }

		CalculateAmount(gvSpareList.NewSimpleRow, null);
		gvSpareList.ShowFooter = true;
		_repairSpare = ""; // update ok, so clear saved number
	}
	protected void gvSpareList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		gvSpareList.ShowFooter = true;
		_repairSpare = "";
	}
	protected void gvSpareList_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		if (e.CommandName == "InsertSimpleRow")
		{
			DataTable tbl = gvSpareList.DataSourceTable;
			DataRow newRow = gvSpareList.NewSimpleRow; if (newRow == null) return;
			DataRow oldRow = gvSpareList.GetSimpleRowByKey("SpareNumber", newRow["SpareNumber"].ToString(), true);

			// warranty spare - no warranty spare are difference
			if ((oldRow != null) && (!string.IsNullOrEmpty(oldRow["ExchangeNumber"].ToString())))
				oldRow = null;

			if (oldRow == null) // addnew
			{
				newRow["ExchangeNumber"] = newRow["ExchangeNumber"].ToString().Trim().ToUpper();
				if (GetSpareInfo(newRow["SpareNumber"].ToString(), newRow))
				{
					CalculateAmount(newRow, null);
					gvSpareList.NewSimpleRow = newRow;
				}
				else gvSpareList.NewSimpleRow = null; // invalid spare -> cancel insert
			}
			else                // inc quantity
			{
				CalculateAmount(newRow, oldRow);
				if (!string.IsNullOrEmpty(newRow["ExchangeNumber"].ToString()))
				{
					oldRow["ExchangeNumber"] = newRow["ExchangeNumber"].ToString().Trim().ToUpper();
				}
				gvSpareList.NewSimpleRow = null;    // cancel insert
			}
			//ShowSumary = true;
			_repairSpare = ""; // added ok, so clear text box
		}
		else if (e.CommandName == "AddNew")
		{
			ShowSumary = false;
		}
		else if (e.CommandName == "CancelInsertSimpleRow")
		{
			ShowSumary = true;
		}

	}
	protected void gvSpareList_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{

	}
	protected void gvSpareList_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			long val;
			NumberFormatInfo ni = Thread.CurrentThread.CurrentCulture.NumberFormat;
			ni.NumberDecimalDigits = 0;

			DataRowView row = (DataRowView)e.Row.DataItem; if (row == null) return;

			// check for valid spare number 
			Warrantycondition warr = WarrantyContent.GetWarrantyCondition(row["SpareNumber"].ToString());
			Literal lit = (Literal)WebTools.FindControlById("litSpareName", e.Row); if (lit != null) lit.Visible = (warr != null);
			TextBox tb = (TextBox)WebTools.FindControlById("txtSpareName", e.Row); if (tb != null) tb.Visible = (warr == null);
			Label lb = (Label)WebTools.FindControlById("lbInvalidSpareNumber", e.Row); if (lb != null) lb.Visible = (warr == null);

			// STT
			e.Row.Cells[0].Text = ((int)(e.Row.DataItemIndex + 1)).ToString();
			row["SpareNo"] = e.Row.Cells[0].Text;

			// change color to indicate Free Warranty 
			if (!string.IsNullOrEmpty(row["IsExchangeSpare"].ToString())) e.Row.CssClass = "readOnlyRow";

			// format number
			if (!((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit))
			{
				// quantity
				if (long.TryParse(row["Quantity"].ToString(), out val)) e.Row.Cells[3].Text = val.ToString("N", ni);
				// unit price
				if (long.TryParse(row["SpareCost"].ToString(), out val)) e.Row.Cells[4].Text = val.ToString("N", ni);
				// fee amount
				if ((showFeeAmountColumn) && (long.TryParse(row["FeeAmount"].ToString(), out val))) e.Row.Cells[7].Text = val.ToString("N", ni);
			}
			// spare amount
			if (long.TryParse(row["SpareAmount"].ToString(), out val)) e.Row.Cells[6].Text = val.ToString("N", ni);
		}
	}
	protected void gvSpareList_DataBound(object sender, EventArgs e)
	{
		long fee;
		TextBox tb;
		NumberFormatInfo ni = null;
		if (gvSpareList.ShowFooter)
		{
			// bind data for footer when select a spare
			if ((footWarranty != null) && gvSpareList.ShowFooter)// && (e.Row.RowType == DataControlRowType.Footer))
			{
				tb = (TextBox)WebTools.FindControlById("txtSpareName", gvSpareList); if (tb != null) { tb.Visible = false; }
				tb = (TextBox)WebTools.FindControlById("txtSpareCost", gvSpareList); if (tb != null) { tb.Text = footWarranty.Unitprice.ToString(); }
				ni = Thread.CurrentThread.CurrentCulture.NumberFormat; ni.NumberDecimalDigits = 0;
				tb = (TextBox)WebTools.FindControlById("txtSpareNumber", gvSpareList); if (tb != null) tb.Text = footWarranty.Partcode;
				Literal lit = (Literal)WebTools.FindControlById("litSpareName", gvSpareList); if (lit != null) lit.Text = (Thread.CurrentThread.CurrentCulture.Name == "vi-VN") ? footWarranty.Partnamevn : footWarranty.Partnameen;
				//lit = (Literal)WebTools.FindControlById("litSpareCost", gvSpareList); if (lit != null) lit.Text = footWarranty.Unitprice.ToString("N", ni);
			}
			else
			{
				tb = (TextBox)WebTools.FindControlById("txtSpareName", gvSpareList); if (tb != null) tb.Visible = allowInvalidSpare;
				//tb = (TextBox)WebTools.FindControlById("txtSpareCost", gvSpareList); if (tb != null) { tb.Visible = allowInvalidSpare; }
				tb = (TextBox)WebTools.FindControlById("txtSpareNumber", gvSpareList); if (tb != null) tb.Text = _repairSpare;
			}

			// calculate sumary
			if (ni == null) ni = Thread.CurrentThread.CurrentCulture.NumberFormat;
			ni.NumberDecimalDigits = 0;
			long.TryParse(_feeAmount, out fee);
			// if (gvSpareList.ShowFooter)
			{
				// total fee amount
				if ((!_finish) || allowInputSRSFeeAfterComplete)
				{
					tb = (TextBox)WebTools.FindControlById("txtFee", gvSpareList);
					if (tb != null)
					{
						tb.Enabled = !showFeeAmountColumn;
						tb.Text = (showFeeAmountColumn) ? fee.ToString("N", ni) : _feeAmount;
					}
				}

				Literal lit = (Literal)WebTools.FindControlById("litSparesAmount", gvSpareList);
				if (lit != null) lit.Text = gvSpareList.SumBy("SpareAmount").ToString("N", ni);

				lit = (Literal)WebTools.FindControlById("litTotalAmount", gvSpareList);
				if (lit != null) lit.Text = GetTotalAmount();

				////////// footer style
				if (!ShowSumary) return;
				GridViewRow gvr = gvSpareList.FootRow; if (gvr == null) return;
				gvr.Cells[0].CssClass = "summaryName";
				gvr.Cells[1].CssClass = "summaryName";
				gvr.Cells[3].CssClass = "summaryName";
				gvr.Cells[5].CssClass = "summaryName";
				gvr.Cells[2].CssClass = "summaryValue";
				gvr.Cells[4].CssClass = "summaryValue"; if (((_finish) && (!allowInputSRSFeeAfterComplete)) || readOnlyForm || _saved) { gvr.Cells[4].Text = fee.ToString("N", ni); }
				gvr.Cells[6].CssClass = "summaryValue";
			}
		}
	}
	protected void gvSpareList_DataBinding(object sender, EventArgs e)
	{

	}
	protected void gvSpareList_Load(object sender, EventArgs e)
	{
	}

	protected void MultiView2_Load(object sender, EventArgs e)
	{
		((MultiView)sender).ActiveViewIndex = (ShowSumary) ? 0 : 1;
	}
	protected void btnFindSpare_Click(object sender, EventArgs e)
	{
		string spareNum;
		if (_addExchange)
		{
			spareNum = AddExchange1.GetLookingSpareNumber();
		}
		else
		{
			TextBox tb = (TextBox)WebTools.FindControlById("txtSpareNumber", gvSpareList); if (tb == null) return;
			spareNum = tb.Text;
		}

		ObjectDataSource1.SelectParameters["spareNumberLike"] = new Parameter("spareNumberLike", TypeCode.String, spareNum);
		gvSelectSpare.PageIndex = 0;
		gvSelectSpare.DataBind();

		if (gvSelectSpare.Rows.Count == 1)
		{

			GridViewRow gvr = gvSelectSpare.Rows[0]; if (gvr == null) return;
			Literal lit = (Literal)WebTools.FindControlById("litSelectedSpareNumber", gvr);
			if (lit != null) footWarranty = WarrantyContent.GetWarrantyCondition(lit.Text);
			if (_addExchange) AddExchange1.AddSpareTofoot(footWarranty);
		}
		else if (gvSelectSpare.Rows.Count > 1) SetAvtiveView(vwSelectSpareIndex);
		else
		{
			if (_addExchange) SetAvtiveView(vwAddExchangeIndex); else SetAvtiveView(vwMainIndex);
			AddError(WarrantyContentErrorCode.SpareNumberNotFound);
		}
	}
	protected void btnFindBroken_Click(object sender, EventArgs e)
	{
		string brokenNum;
		if (_addExchange)
		{
			brokenNum = AddExchange1.GetLookingBrokenNumber();
		}
		else { brokenNum = ""; }

		odsSelectBroken.SelectParameters["fromCode"] = new Parameter("fromCode", TypeCode.String, brokenNum);
		gvSelectBroken.PageIndex = 0;
		gvSelectBroken.DataBind();

		if (gvSelectBroken.Rows.Count == 1)
		{
			GridViewRow gvr = gvSelectBroken.Rows[0]; if (gvr == null) return;
			Literal lit = (Literal)WebTools.FindControlById("litSelectedBrokenCode", gvr);
			if (lit != null) footBroken = WarrantyContent.GetBroken(lit.Text);
			if (_addExchange) AddExchange1.AddBrokenTofoot(footBroken);
		}
		else if (gvSelectBroken.Rows.Count > 1) SetAvtiveView(vwAddBrokenIndex);
		else
		{
			if (_addExchange) SetAvtiveView(vwAddExchangeIndex); else SetAvtiveView(vwMainIndex);
			AddError(WarrantyContentErrorCode.BrokenCodeNotFound);
		}
	}

	protected bool CheckServiceTypeForPVC(int type)
	{
		return CheckServiceTypeForPVC((ServiceType)type);
	}
	protected bool CheckServiceTypeForPVC(ServiceType type)
	{
		return ((!requireCheckWarrantyToAddPCV) ||
				(type == ServiceType.MaintainAndWarranty) ||
				(type == ServiceType.RepairAndMaintainAndWarranty) ||
				(type == ServiceType.Warranty) ||
				(type == ServiceType.WarrantyAndRepair));
	}

	protected void btnCalculate_Click1(object sender, EventArgs e)
	{
		long km, kmo;
		bool invalidInput = false;
		CultureInfo ci = Thread.CurrentThread.CurrentCulture;
		DateTime buyDate, repairDate;

		// check text
		if (txtErrorStatus.Text.Length >= txtErrorStatus.MaxLength) { AddError(WarrantyContentErrorCode.StringTooLong); invalidInput = true; }
		if (txtRepair.Text.Length >= txtRepair.MaxLength) { AddError(WarrantyContentErrorCode.StringTooLong); invalidInput = true; }

		// check service type
		ServiceType stype = (ServiceType)GetSelectedServices();
		if (chblSerList.SelectedIndex < 0) { AddError(WarrantyContentErrorCode.InvalidServiceType); invalidInput = true; }
		if ((AddExchange1.SpareList.Rows.Count > 0) &&
			(!(requireCheckWarrantyToAddPCV && CheckServiceTypeForPVC(stype))))
		{
			AddError(WarrantyContentErrorCode.InvalidServiceType);
			invalidInput = true;
		}

		// check datetime
		if (!DateTime.TryParse(txtBuyDate.Text, ci, DateTimeStyles.AllowWhiteSpaces, out buyDate)) { AddError(WarrantyContentErrorCode.InvalidDateTimeValue); invalidInput = true; }
		if (!DateTime.TryParse(txtRepairDate.Text, ci, DateTimeStyles.AllowWhiteSpaces, out repairDate)) { AddError(WarrantyContentErrorCode.InvalidDateTimeValue); invalidInput = true; }
		if (
			(buyDate > DateTime.Now)
			|| (repairDate > DateTime.Now)
			|| (buyDate > repairDate)
			|| (buyDate == DateTime.MinValue)
			|| (buyDate == DateTime.MinValue)
			|| ((_exchangeHeader != null) && (AddExchange1.SpareList.Rows.Count > 0) && ((_exchangeHeader.Damageddate < buyDate) || (_exchangeHeader.Damageddate > repairDate)))
			)
		{
			AddError(WarrantyContentErrorCode.InvalidDateTimeValue); invalidInput = true;
		}
		if ((_exchangeHeader != null) && (_exchangeHeader.Damageddate != null) && ((_exchangeHeader.Damageddate > repairDate) || (_exchangeHeader.Damageddate < buyDate))) { AddError(WarrantyContentErrorCode.InvalidDateTimeValue); invalidInput = true; }

		// check current kmCount
		if (!long.TryParse(txtKm.Text, out km)) { rgvKm.IsValid = false; invalidInput = true; }
		if (//UserHelper.IsDealer && 
			(long.TryParse(txtLastKm.Text, out kmo))
		   )
		{
			CompareValidator1.IsValid = (km > kmo);
			if (((km <= kmo)) && UserHelper.IsDealer) invalidInput = true;
		}

		// input data ok?
		if (invalidInput) _finish = false;
		else _finish = !_finish;
		CheckFinish();
	}
	protected void btnSave_Click(object sender, EventArgs e)
	{
		if (_saved) // add new sheet
		{
			_finish = false;
			_saveTemp = false;
			_exchSaved = false;
			_canChangeBuyDate = true;
			_editSheetId = 0;
			_exchangeHeader = null;
			_feeAmount = "0";
			txtEngineNo.Text = "";
			btnSaveTemp.Enabled = true;

			CheckFinish();
			ClearForm();
		}
		else // save this sheet
		{
			if (!SaveSheet(ServiceStatus.Done)) return;
			btnCalculate.Enabled = false;
			btnSaveTemp.Enabled = false;
		}

		_saved = !_saved;
	}
	/* tntung
	 * Begin: Service Record Sheet
	 */
	protected void btnPrint_Click(object sender, EventArgs e)
	{
		//Parameters
		//string CustomerFullName = "";
		//GetReportDocument
		//MessageBox.Show(txtSheetNo.Text);
		ReportDocument rdServiceRecordSheet = ReportFactory.GetReport();
		rdServiceRecordSheet.Load(Server.MapPath(@"~/Report/ServiceRecordSheet.rpt"));
		rdServiceRecordSheet.SetDataSource(gvSpareList.DataSourceTable);
		rdServiceRecordSheet.SetParameterValue("DealerName", DealerHelper.GetName(string.IsNullOrEmpty(viewmodeDealer) ? CurrentDealer : viewmodeDealer));
		rdServiceRecordSheet.SetParameterValue("DealerAddress", DealerHelper.GetAddress(string.IsNullOrEmpty(viewmodeDealer) ? CurrentDealer : viewmodeDealer));
		rdServiceRecordSheet.SetParameterValue("ServiceRecordNumber", txtSheetNo.Text.Trim());
		rdServiceRecordSheet.SetParameterValue("ServiceRecordDate", txtRepairDate.Text.Trim());
		rdServiceRecordSheet.SetParameterValue("CustomerFullName", txtCusName.Text.Trim());
		rdServiceRecordSheet.SetParameterValue("CustomerAddress", txtCustAddress.Text.Trim());
		rdServiceRecordSheet.SetParameterValue("CustomerTel", txtPhoneNo.Text.Trim());
		rdServiceRecordSheet.SetParameterValue("CustomerPurchaseDate", txtBuyDate.Text.Trim());
		rdServiceRecordSheet.SetParameterValue("Model", txtModel.Text.Trim());
		rdServiceRecordSheet.SetParameterValue("Color", ddlColour.SelectedItem.Text);
		rdServiceRecordSheet.SetParameterValue("EngineNumber", hdEngineNumber.Value.Trim());
		rdServiceRecordSheet.SetParameterValue("Kilometer", (string.IsNullOrEmpty(txtKm.Text.Trim())) ? 0 : decimal.Parse(txtKm.Text.Trim()));
		rdServiceRecordSheet.SetParameterValue("PlateNumber", txtNumberPlate.Text.Trim());
		rdServiceRecordSheet.SetParameterValue("DamagedStatus", txtErrorStatus.Text.Trim());
		rdServiceRecordSheet.SetParameterValue("SolutionText", txtRepair.Text.Trim());
		rdServiceRecordSheet.SetParameterValue("PartCost", (decimal)gvSpareList.SumBy("SpareAmount"));
		rdServiceRecordSheet.SetParameterValue("LabourCost", (string.IsNullOrEmpty(_feeAmount)) ? 0 : decimal.Parse(_feeAmount));
		rdServiceRecordSheet.SetParameterValue("Amount", (decimal)(CalculateTotalAmount()));
		rdServiceRecordSheet.SetParameterValue("isMaintain", (bool)(chblSerList.Items[0].Selected));
		rdServiceRecordSheet.SetParameterValue("isRepair", (bool)(chblSerList.Items[1].Selected));
		rdServiceRecordSheet.SetParameterValue("isWarranty", (bool)(chblSerList.Items[2].Selected));

		CRVServiceRecordSheet.DisplayGroupTree = false;// CrystalReportViewer1.HasCrystalLogo = false;
		CRVServiceRecordSheet.ReportSource = rdServiceRecordSheet;
		CRVServiceRecordSheet.DataBind();

		SetAvtiveView(vwPrintIndex);

		//gvSpareList.DataSourceTable;
	}
	/* tntung
	 * End: Service Record Sheet
	 */
	protected void btnNewCus_Click(object sender, EventArgs e)
	{
		#region copy from tnTung

		long lngCustId;
		long.TryParse(_CustomerID.Value, out lngCustId);
		string ActionString = "DEFAULT";

		ISession sess = NHibernateSessionManager.Instance.GetSession();
		IList lstCus = sess.CreateCriteria(typeof(Customer))
			.Add(Expression.Eq("Id", lngCustId))
			.Add(Expression.Eq("Dealercode", CurrentDealer)).List();
		if (lstCus.Count > 0)
		{
			ActionString = "UPDATECUSTOMER";
		}
		string idnum = txtCustId.Text.Trim();
		string fullname = _CustomerFullName.Value;
		bool gender;
		if (ddlSex.Value.Trim().Equals("1"))
		{
			gender = true;
		}
		else gender = false;

		DateTime dt;
		CultureInfo cultInfo = Thread.CurrentThread.CurrentCulture;
		DateTime.TryParse(txtBirthDate.Value, cultInfo, DateTimeStyles.AllowWhiteSpaces, out dt);
		DateTime birthdate = dt;

		string address = txtAddress.Value.Trim();
		string provinceid = ddlProvince.Value.ToString().Trim();
		string districtid = txtDistrict.Value.Trim();
		int jobtype = int.Parse(tblCus_JobType.Value.ToString());
		string email = txtCEmail.Value.Trim();
		//if (string.IsNullOrEmpty(email)) email = ".";
		string tel = txtCPhone.Value.Trim();
		string mobile = txtCMobile.Value.Trim();
		//if (string.IsNullOrEmpty(mobile)) mobile = ".";
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
		ITransaction tx = sess.BeginTransaction();
		if (ActionString.Equals("UPDATECUSTOMER"))
		{
			lngCustId = CustomerHelper.UpdateCustomer(ref sess, lngCustId, idnum, fullname, gender, birthdate, address, provinceid, districtid, jobtype, email, tel, mobile, priority, customertype, cusdesc, precinct, CurrentDealer, true);
			_CustomerID.Value = lngCustId.ToString();
			txtCustId.Text = idnum;
			//lbCustomerFullName.Text = fullname;
		}
		else
		{
			lngCustId = CustomerHelper.SaveCustomer(ref sess, idnum, fullname, gender, birthdate, address, provinceid, districtid, jobtype, email, tel, mobile, priority, customertype, cusdesc, precinct, CurrentDealer, true);
			_CustomerID.Value = lngCustId.ToString();
			txtCustId.Text = idnum;
			//lbCustomerFullName.Text = fullname;
		}
		//btnSave.Enabled = true;

		#endregion

		// load cust info
		_custInfo = sess.Get<Customer>(lngCustId);
		//_custInfo = WarrantyContent.GetCustInfos(lngCustId);
		ShowCustInfo();
		tx.Commit();
	}
	protected void btnCheckModel_Click(object sender, EventArgs e)
	{
		odsSelectModel.SelectParameters["itemTypeLike"] = new Parameter("itemTypeLike", TypeCode.String, txtModel.Text);
		gvSelectModel.PageIndex = 0;
		gvSelectModel.DataBind();
		if (gvSelectModel.Rows.Count == 1)
		{
			GridViewRow gvr = gvSelectModel.Rows[0];
			Literal lit = (Literal)WebTools.FindControlById("litSelectedModel", gvr); if (lit == null) return;
			txtModel.Text = lit.Text;
		}
		else if (gvSelectModel.Rows.Count > 1) { SetAvtiveView(vwSelectModelIndex); }
		else { AddError(WarrantyContentErrorCode.ItemTypeNotFound); }
	}
	protected void btnAdd_Click(object sender, EventArgs e)
	{
		ShowSumary = false;
	}
	protected void ImageButtonx2_DataBinding(object sender, EventArgs e)
	{
		ImageButton ibtn = (ImageButton)sender;
		ibtn.Visible = (string.IsNullOrEmpty(ibtn.CommandArgument) && (!_finish));
	}
	protected void gvSelectxxx_PreRender(object sender, EventArgs e)
	{
		GridView gv = (GridView)sender;
		if (gv.TopPagerRow == null) return;
		Literal litPageInfo = gv.TopPagerRow.FindControl("lit" + gv.ID + "PageInfo") as Literal;
		if (litPageInfo != null) litPageInfo.Text = string.Format(Message.PagingInfo, gv.PageIndex + 1, gv.PageCount, HttpContext.Current.Items["list" + gv.ID + "RowCount"]);
	}
	protected void ddlColour_DataBound(object sender, EventArgs e)
	{
		DropDownList drop = (DropDownList)sender;
		foreach (ListItem item in drop.Items)
		{
			item.Text = item.Value + " (" + item.Text + ")";
		}
	}
	protected void btnAddExchange_Click(object sender, EventArgs e)
	{
		bool initDataOk = true;
		DateTime dt;
		long km;

		// validate data
		//hdEngineNumber.Value = txtEngineNo.Text.Trim();
		if (_custInfo == null) { rqvCustName.IsValid = false; initDataOk = false; }
		if (string.IsNullOrEmpty(hdEngineNumber.Value)) { rqvEngineNumber.IsValid = false; initDataOk = false; }
		if (string.IsNullOrEmpty(txtRepairDate.Text)) { rqvRepairDate.IsValid = false; initDataOk = false; }
		if (string.IsNullOrEmpty(txtBuyDate.Text)) { rqvBuydate.IsValid = false; initDataOk = false; }
		if (!long.TryParse(txtKm.Text, out km)) { rgvKm.IsValid = false; initDataOk = false; }
		if (!(requireCheckWarrantyToAddPCV && CheckServiceTypeForPVC(GetSelectedServices())))
		{
			AddError(WarrantyContentErrorCode.InvalidServiceType);
			initDataOk = false;
		}

		if (!initDataOk) return;

		string strEngNo = hdEngineNumber.Value.Trim().ToUpper();

		// input data ok => show addExchange form
		SetAvtiveView(vwAddExchangeIndex);
		_addExchange = true;
		if (_exchangeHeader == null)
		{
			_exchangeHeader = new Exchangepartheader();
			_exchangeHeader.Damageddate = DateTime.Now;

			if (_dtVehicleMadedate > DateTime.MinValue)
			{
				_exchangeHeader.Exportdate = _dtVehicleMadedate;
			}
			else
			{
				Iteminstance item = AddExchange.GetItemInstance(strEngNo);
				if (item != null) _exchangeHeader.Exportdate = item.Madedate;
			}

			AddExchange1.ClearForm();
		}

		_exchangeHeader.Customer = _custInfo;
		if (DateTime.TryParse(txtBuyDate.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dt)) _exchangeHeader.Purchasedate = dt;
		if (DateTime.TryParse(txtRepairDate.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dt)) _exchangeHeader.Exchangeddate = dt;
		if (showTempSheetNo) _exchangeHeader.Vouchernumber = AddExchange.GenExchangeNumber(CurrentDealer);
		_exchangeHeader.Enginenumber = strEngNo;
		_exchangeHeader.Framenumber = txtFrameNumber.Text.Trim().ToUpper();
		_exchangeHeader.Kmcount = km;
		_exchangeHeader.Model = txtModel.Text.Trim().ToUpper();
		_exchangeHeader.Dealercode = CurrentDealer;

		AddExchange1.ExchangeHeader = _exchangeHeader;
		AddExchange1.InitUC();
	}
	protected void txtFee_Load(object sender, EventArgs e)
	{
		//if (!string.IsNullOrEmpty(_feeAmount)) ((TextBox)sender).Text = _feeAmount;
	}
	protected void btnSaveExchange_Click(object sender, EventArgs e)
	{
		_exchangeHeader = AddExchange1.ExchangeHeader;
		_addExchange = false;
		SetAvtiveView(vwMainIndex);
		CopyExchangeList();
		gvSpareList.DataBind();
	}
	protected void gvSelectxxx_page(object sender, GridViewCommandEventArgs e)
	{
		GridView gv = (GridView)sender;
		if (((string)e.CommandName == "Page") && ((((string)e.CommandArgument == "Next") && ((gv.PageIndex + 1) == gv.PageCount)) || (((string)e.CommandArgument == "Prev") && (gv.PageIndex == 0))))
			gv.DataBind();
	}
	protected void btnPrintPcv_Click(object sender, EventArgs e)
	{

	}
	protected void ddlDealer_SelectedIndexChanged(object sender, EventArgs e)
	{
		GetBranchs();
		btnCheck_Click(null, null); // FORCE to check sell dealer (donot clear)
	}

	protected void ddlDealer_DataBound(object sender, EventArgs e)
	{
		DropDownList drop = (DropDownList)sender;
		foreach (ListItem item in drop.Items)
		{
			item.Text += "      " + DealerHelper.GetName(item.Text);
		}
	}
	// show select customer
	protected void Button4_Click(object sender, EventArgs e)
	{
		gvSelectCust.DataBind();
		SetAvtiveView(vwAddCustIndex);
	}
	protected void gvSelectCust_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			Customer cust = (Customer)e.Row.DataItem;
			e.Row.Cells[2].Text = ServiceTools.GetCustAddress(cust);
			//if (e.Row.Cells[3].Text.Trim() == ".") e.Row.Cells[3].Text = ""; // email
			e.Row.Cells[4].Text = (cust.Birthdate > DateTime.MinValue) ? cust.Birthdate.ToShortDateString() : "";
			//if (e.Row.Cells[5].Text.Trim() == ".") e.Row.Cells[5].Text = ""; // mobile
		}
	}
	// select a customer
	protected void Button1_Click(object sender, EventArgs e)
	{
		Button btn = (Button)sender;
		Customer cust = CustomerDataSource.GetCustomer(btn.CommandArgument);
		if (cust != null)
		{
			_custInfo = cust;
			ShowCustInfo();
		}
		SetAvtiveView(vwMainIndex);
	}
	protected void btnBackFromPrint_Click(object sender, EventArgs e)
	{
		MultiView1.ActiveViewIndex = vwMainIndex;
	}
	protected void btnSaveTemp_Click(object sender, EventArgs e)
	{
		if (!_saved)
		{
			if (string.IsNullOrEmpty(txtEngineNo.Text))
			{
				rqvEngineNumber.IsValid = false;
			}
			else
			{
				// fix: chua click check da save temp luon
				if (string.IsNullOrEmpty(hdEngineNumber.Value))
				{
					hdEngineNumber.Value = txtEngineNo.Text.Trim().ToUpper();
				}

				if (!SaveSheet(ServiceStatus.Temp)) return;
				_saveTemp = true;

				// direct luon cho no clear
				Response.Redirect(string.Format("WarrantyContent.aspx?sid={0}", _editSheetId));
			}
		}
	}
}
