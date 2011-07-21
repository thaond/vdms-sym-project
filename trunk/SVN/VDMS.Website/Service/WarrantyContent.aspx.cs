using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate;
using NHibernate.Expression;
using Resources;
using VDMS.Common.Web;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
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

public partial class Service_Warranty_Content : BasePage
{
    // constant values
    protected string LoadCustomerErr = Resources.Message.LoadCustomerErr;
    protected string SumMoneyInvalid = Resources.Message.Cus_SumMoneyInvalid;
    protected string LoadEngineNoErr = Resources.Message.EngineNumberNotFound;

    // variables store information during process
    private Collection<WarrantyContentErrorCode> errorCode = new Collection<WarrantyContentErrorCode>();

    private bool ListContainsValue(ListItemCollection items, string value)
    {
        foreach (ListItem item in items)
        {
            if (item.Value == value) return true;
        }
        return false;
    }
    protected void ShowEmptyGridView<T>(GridView gv) where T : new()
    {
        List<T> items = new List<T>();
        items.Add(new T());
        gv.DataSource = items;
        gv.DataBind();
        gv.Rows[0].Cells[0].ColumnSpan = gv.Columns.Count;
        gv.Rows[0].Cells[0].Text = gv.EmptyDataText;
        gv.Rows[0].Cells[0].CssClass = gv.EmptyDataRowStyle.CssClass;
        for (int i = 1; i < gv.Columns.Count; i++)
        {
            gv.Rows[0].Cells[i].Visible = false;
        }
    }

    public bool EvalCommandVisible(object exchangeNumber)
    {
        return string.IsNullOrEmpty((string)exchangeNumber);
    }
    public string EvalPartName(object nameEn, object nameVn)
    {
        return (string)((UserHelper.Language.Equals("vi-VN")) ? nameVn : nameEn);
    }
    public string EvalDate(object date)
    {
        DateTime dt;
        if ((date == null) ||
            !DateTime.TryParse(date.ToString(), new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out dt))
        {
            return "";
        }
        return dt.ToShortDateString();
    }

    #region Properties

    // current dealer/branch code used to create SRS
    string _CurrentDealer;
    public string CurrentDealer
    {
        get
        {
            if (string.IsNullOrEmpty(_CurrentDealer))
            {
                _CurrentDealer = (!UserHelper.IsDealer) ? ddlDealer.SelectedValue : UserHelper.DealerCode;
            }
            return _CurrentDealer;
        }
        set
        {
            _CurrentDealer = value;
            ddlDealer.SelectedValue = value;
            GetBranchs(value);
            //UpdateDealerInfo();
        }
    }
    string _CurrentBranch;
    public string CurrentBranch
    {
        get
        {
            if (string.IsNullOrEmpty(_CurrentBranch))
            {
                _CurrentBranch = (string.IsNullOrEmpty(UserHelper.BranchCode)) ? ddlBranchCode.SelectedValue : UserHelper.BranchCode;
                if (string.IsNullOrEmpty(_CurrentBranch)) _CurrentBranch = CurrentDealer;
            }
            return _CurrentBranch;
        }
        set
        {
            _CurrentBranch = value;
            ddlBranchCode.SelectedValue = value;
        }
    }

    public SrsInfo Info { get; set; }

    #endregion

    #region Control state

    protected override object SaveControlState()
    {
        object[] ctlState = new object[2];
        ctlState[0] = base.SaveControlState();
        ctlState[1] = Info;
        //ctlState[1] = _showSumary;
        //ctlState[2] = _feeAmount;
        //ctlState[3] = _custInfo;
        //ctlState[4] = _exchangeNumber;
        //ctlState[5] = _finish;
        //ctlState[6] = _saved;
        //ctlState[7] = _addExchange;
        //ctlState[8] = _exchangeHeader;
        //ctlState[9] = _isVehicleOnTiptop;
        //ctlState[10] = _sVehicleType;
        //ctlState[11] = _sVehileColor;
        //ctlState[12] = _sVehicleDealer;
        //ctlState[13] = _dtVehicleMadedate;
        //ctlState[14] = _editSheetId;
        //ctlState[15] = _saveTemp;
        //ctlState[16] = _exchSaved;
        //ctlState[17] = _canChangeBuyDate;
        //ctlState[18] = _itemExist;
        //ctlState[19] = _sVehicleDB;
        return ctlState;
    }
    protected override void LoadControlState(object state)
    {
        if (state != null)
        {
            object[] ctlState = (object[])state;
            base.LoadControlState(ctlState[0]);
            Info = (SrsInfo)ctlState[1];
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        Page.RegisterRequiresControlState(this);
    }

    #endregion

    #region Operations

    protected bool CheckServiceTypeForPVC(int type)
    {
        return CheckServiceTypeForPVC((ServiceType)type);
    }
    protected bool CheckServiceTypeForPVC(ServiceType type)
    {
        return ((!SrsSetting.requireCheckWarrantyToAddPCV) ||
                (type == ServiceType.MaintainAndWarranty) ||
                (type == ServiceType.RepairAndMaintainAndWarranty) ||
                (type == ServiceType.Warranty) ||
                (type == ServiceType.WarrantyAndRepair));
    }
    protected void CheckWarrantyCondition()
    {
        DateTime buyDt, repairDt;
        long km;
        DateTime.TryParse(txtBuyDate.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out buyDt);
        DateTime.TryParse(txtRepairDate.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out repairDt);
        long.TryParse(txtKm.Text, out km);

        foreach (PCVItem item in Info.ExchangePartDetail)
        {
            Warrantycondition warr = WarrantyContent.GetWarrantyCondition(item.Partcodeo);
            if (warr == null) continue;
            item.WarrantyWarn = (((((TimeSpan)repairDt.Subtract(buyDt)).Days / 30) > warr.Warrantytime)
                                   && (km > warr.Warrantylength));
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="engineNo"></param>
    /// <param name="itemId">itemId > 0: In VDMS. itemId = 0: In TipTop. itemId = -1: Not found.</param>
    /// <param name="model"></param>
    /// <param name="colorCode"></param>
    /// <param name="dealerCode"></param>
    /// <param name="branchCode"></param>
    /// <param name="dbCode"></param>
    /// <param name="exportDate"></param>
    /// <param name="impDate"></param>
    protected void CheckEnteredEngineNo(string engineNo, long itemId, string model, string colorCode, string dealerCode, string branchCode, string dbCode, DateTime exportDate, DateTime impDate)
    {
        //Iteminstance iis = ItemInstanceDataSource.GetByEngineNumber(engineNo);
        // warranty info
        var warrInfo = ServiceTools.GetWarrantyInfo(engineNo);
        // cust invoice info /*****/
        var cusInv = WarrantyContent.GetCustInvoiceInfos(engineNo);
        // tim thong tin tu cac lan sua chua truoc
        ServiceHeader serh = WarrantyContent.FindServiceSheet(engineNo);

        //// from first upgrade: Motorbike.GetItemInfo() does not return DealerCode and OutStockDate
        //DataRow saleInfo = Motorbike.GetItemSaleInfo(engineNo, dbCode);
        //if (saleInfo != null)
        //{
        //    dealerCode = saleInfo["DealerCode"].ToString();
        //    exportDate = (DateTime)saleInfo["OutStockDate"];
        //}

        //DateTime exportDate;
        //DateTime.TryParse(madeDate, out exportDate);
        long id = itemId;
        //long.TryParse(itemId, out id);

        //Info.IsPersistent = Info.IsPersistent && engineNo.Trim().Equals(Info.ServiceHeader.Enginenumber, StringComparison.OrdinalIgnoreCase);
        Info.IsItemOnTipTop = id == 0;
        Info.IsItemExist = id >= 0;// !string.IsNullOrEmpty(model) && !string.IsNullOrEmpty(colorCode) && !string.IsNullOrEmpty(dealerCode);

        // dealer and export/made date ----------------------------------------------------------
        Info.ItemSoldDealer = dealerCode;
        Info.ItemDBCode = dbCode;
        Info.ExchangePartHeader.Exportdate = exportDate;
        // engine - model and color ----------------------------------------------------------
        Info.ServiceHeader.Enginenumber = Info.ExchangePartHeader.Enginenumber = engineNo.Trim().ToUpper();
        Info.ServiceHeader.Itemtype = Info.ExchangePartHeader.Model = model;
        Info.ServiceHeader.Colorcode = colorCode;
        // repair history----------------------------------------------------------
        btnHistory.Enabled = (serh != null);
        btnHistory.OnClientClick = "window.open('repairhistory.aspx?engnum=" + engineNo + "','repairhis',''); return false;";
        // so khung, bien so ----------------------------------------------------------
        Info.ServiceHeader.Framenumber = Info.ExchangePartHeader.Framenumber = (serh != null) ? serh.FrameNumber : "";
        Info.ServiceHeader.Numberplate = (serh != null) ? serh.NumberPlate : "";
        Info.CanChangeFrameNumber = string.IsNullOrEmpty(Info.ServiceHeader.Framenumber);
        Info.CanChangePlateNumber = string.IsNullOrEmpty(Info.ServiceHeader.Numberplate);
        // buy-repair date----------------------------------------------------------
        if (Info.ServiceHeader.Servicedate == DateTime.MinValue) Info.ServiceHeader.Servicedate = DateTime.Now;
        Info.ServiceHeader.Purchasedate = (warrInfo != null) ? warrInfo.PurchaseDate :
                                                    ((cusInv != null) ? cusInv.SellDate :
                                                            ((serh != null) ? serh.PurchaseDate : DateTime.MinValue)
                                                    );
        if (Info.ExchangePartHeader != null) Info.ExchangePartHeader.Purchasedate = Info.ServiceHeader.Purchasedate;
        Info.IsItemHasBuyDate = Info.ServiceHeader.Purchasedate > DateTime.MinValue;
        // get KmCount, Customer ------------------------------------------------------------------
        Info.LastKm = (warrInfo != null) ? (long)warrInfo.KmCount : ((serh != null) ? (long)serh.KmCount : 0);

        Info.ExchangePartHeader.Customer = ((serh != null) && (serh.Customer != null)) ? serh.Customer.ConvertCustomer() :
                                                    (((cusInv != null) && (cusInv.Customer != null)) ? cusInv.Customer.ConvertCustomer() :
                                                            (((warrInfo != null) && (warrInfo.Customer != null)) ? warrInfo.Customer.ConvertCustomer() : null)
                                                    );
        Info.ServiceHeader.Customer = Info.ExchangePartHeader.Customer;

        ShowItemStatus();
    }

    protected bool CheckDataOnSave()
    {
        bool invalidInput = false;
        // check text
        if (Info.ServiceHeader.Damaged.Length >= txtErrorStatus.MaxLength) { AddError(WarrantyContentErrorCode.StringTooLong); invalidInput = true; }
        if (Info.ServiceHeader.Repairresult.Length >= txtRepair.MaxLength) { AddError(WarrantyContentErrorCode.StringTooLong); invalidInput = true; }

        // check service type
        if (Info.ServiceHeader.Servicetype < 0) { AddError(WarrantyContentErrorCode.InvalidServiceType); invalidInput = true; }
        if ((Info.ExchangePartDetail.Count > 0) && Info.HasPCV &&
            (!(SrsSetting.requireCheckWarrantyToAddPCV && CheckServiceTypeForPVC(Info.ServiceHeader.Servicetype))))
        {
            AddError(WarrantyContentErrorCode.InvalidServiceType);
            invalidInput = true;
        }

        // check datetime
        if (
            (Info.ServiceHeader.Purchasedate > DateTime.Now)
            || (Info.ServiceHeader.Servicedate > DateTime.Now)
            || (Info.ServiceHeader.Purchasedate > Info.ServiceHeader.Servicedate)
            || (Info.ServiceHeader.Purchasedate == DateTime.MinValue)
            || (Info.ServiceHeader.Purchasedate == DateTime.MinValue)
            || ((Info.ExchangePartHeader != null) && Info.HasPCV && (Info.ExchangePartDetail.Count > 0) && ((Info.ExchangePartHeader.Damageddate < Info.ServiceHeader.Purchasedate) || (Info.ExchangePartHeader.Damageddate > Info.ServiceHeader.Servicedate)))
            )
        {
            AddError(WarrantyContentErrorCode.InvalidDateTimeValue); invalidInput = true;
        }

        // check current kmCount: 
        cvKmCount.IsValid = (Info.ServiceHeader.Kmcount > Info.LastKm); //this will show warning only
        if ((Info.ServiceHeader.Kmcount < Info.LastKm) && (UserHelper.IsDealer)) invalidInput = true;
        // check spares
        invalidInput = invalidInput || !CheckSparesList();
        // input data ok?
        //if (!invalidInput) Info.CurrentSate =  SrsState.FinishEdit;
        return !invalidInput;
    }

    private bool CheckSparesList()
    {
        bool ok = true;
        if (Info.HasPCV)
        {
            ok = Info.ExchangePartDetail.Count(i => i.Broken == null) == 0;
            if (!ok) AddError(WarrantyContentErrorCode.InCompleteSpares);
        }
        return ok;
    }


    protected void AddSRSItem(string spareNumber, string spareName, int quantity, long unitPrice)
    {
        bool exist = true;
        Warrantycondition warr = WarrantyContent.GetWarrantyCondition(spareNumber);
        if (SrsSetting.allowInvalidSpare || (warr != null))
        {
            SRSItem item = Info.FindServiceItem(spareNumber);
            if (item == null)
            {
                item = new SRSItem();
                exist = false;
            }

            if (quantity > 0)   // modify or add new
            {
                item.Unitprice = unitPrice;
                item.Partqty += quantity;
                item.Partcode = spareNumber;
                item.Partname = spareName;
                item.HasModified = exist;
                if (!exist)
                {
                    item.State = ServiceItemState.Transient;
                    item.Serviceheader = Info.ServiceHeader;
                    Info.ServiceDetail.Add(item);
                }
                else if (item.State == ServiceItemState.Deleted)
                {
                    item.State = ServiceItemState.Persistent;
                    item.HasModified = true;
                    item.Partqty = quantity;    // use new quantity, not increase
                }
            }
            else        // delete existing
            {
                if (exist) item.State = ServiceItemState.Deleted;   // Info.ServiceDetail.Remove(item);
            }
        }
    }
    protected void AddPCVItem(string spareNumber, int quantity, string serial, string brokenCode)
    {
        bool exist = true;
        Warrantycondition warr = WarrantyContent.GetWarrantyCondition(spareNumber);
        Broken broken = WarrantyContent.GetBroken(brokenCode);

        if ((warr != null) && (broken != null))
        {
            PCVItem item = Info.FindExchangeItem(spareNumber);
            if (item == null)
            {
                item = new PCVItem(warr);
                exist = false;
            }

            if (broken != null) item.Broken = broken;
            if (!string.IsNullOrEmpty(serial)) item.Serialnumber = serial;

            if (quantity > 0)   // modify or add new
            {
                item.Partqtyo += quantity;
                item.Partqtym = item.Partqtyo;
                item.HasModified = exist;
                if (!exist)
                {
                    item.State = ServiceItemState.Transient;
                    item.Exchangepartheader = Info.ExchangePartHeader;
                    Info.ExchangePartDetail.Add(item);
                }
                else if (item.State == ServiceItemState.Deleted)
                {
                    item.State = ServiceItemState.Persistent;
                    item.HasModified = true;
                    item.Partqtym = item.Partqtyo = quantity;   // use new quantity, not increase
                }
            }
            else        // delete existing
            {
                if (exist) item.State = ServiceItemState.Deleted;   // Info.ServiceDetail.Remove(item);
            }

            PCVItemsUpdated();
        }
    }
    protected void PCVItemsUpdated()
    {
        CheckWarrantyCondition();
        txtexFeeOffer.Text = Info.GetExchangeTotalFee().ToString();
    }

    protected void BindSRSItems()
    {

        var items = from item in Info.ServiceDetail where item.State != ServiceItemState.Deleted select item;
        gvSpareList.DataSource = items;
        gvSpareList.DataBind();

        if (gvSpareList.Rows.Count == 0)
        {
            ShowEmptyGridView<SRSItem>(gvSpareList);
        }
    }
    protected void BindPCVItems()
    {
        var items = from item in Info.ExchangePartDetail where item.State != ServiceItemState.Deleted select item;
        gvexSpareList.DataSource = items;
        gvexSpareList.DataBind();
        if (gvexSpareList.Rows.Count == 0)
        {
            ShowEmptyGridView<PCVItem>(gvexSpareList);
        }
    }

    protected void ShowItemStatus()
    {
        if (Info.IsItemExist)
        {
            if (Info.ItemSoldDealer != CurrentDealer) AddError(WarrantyContentErrorCode.ItemSoldByOtherDealer);
        }
        else
        {
            if (string.IsNullOrEmpty(Info.ServiceHeader.Enginenumber))
            {
                //AddError(WarrantyContentErrorCode.NoItemSold);
            }
            else
                AddError(WarrantyContentErrorCode.EngineNumberNotFound);
        }
    }
    protected void ShowDealerForm()
    {
        bool isNotDealer = !UserHelper.IsDealer && !Info.ReadOnly;
        Literal38.Visible = isNotDealer;
        Literal36.Visible = isNotDealer;
        ddlBranchCode.Visible = isNotDealer;
        ddlDealer.Visible = isNotDealer;
        if (isNotDealer) GetDealerList();
    }
    protected void UpdateDealerInfo()
    {
        Info.ServiceHeader.Dealercode = CurrentDealer;
        if (Info.HasPCV) Info.ExchangePartHeader.Dealercode = CurrentDealer;

        Info.ServiceHeader.Branchcode = CurrentBranch;

        txtexDealer.Text = CurrentDealer;
        var d = VDMS.Data.TipTop.Dealer.GetDealer(CurrentDealer).Tables[0];
        txtexAreaCode.Text = (d.Rows.Count > 0) ? (string)d.Rows[0]["AreaCode"] : "";

        udpPCV.Update();
        //ShowItemStatus();
    }
    protected void ShowCustInfo(Customer cus)
    {
        txtexAddress.Text = txtCustAddress.Text = (cus == null) ? "" : ServiceTools.GetCustAddress(cus);
        txtexCustName.Text = txtCusName.Text = (cus == null) ? "" : cus.Fullname;
        txtEmail.Text = (cus == null) ? "" : cus.Email;
        txtexPhone.Text = txtPhoneNo.Text = (cus == null) ? "" : ServiceTools.GetCustTelNo(cus);

        txtCustId.Text = (cus != null) ? cus.Identifynumber : "";
    }
    protected void LoadAllColor()
    {
        if (ddlColour.SelectedIndex <= 0) // dang chon roi thoi ko load lai nua)
        {
            ddlColour.DataSource = Motorbike.GetAllColor(); ddlColour.DataBind();
            ddlColour.Items.Insert(0, new ListItem("", ""));
        }
    }

    protected void GetBranchs(string dealer)
    {
        if (IsPostBack || !string.IsNullOrEmpty(dealer))
        {
            //ddlBranchCode.DealerCode = dealer;
            //ddlBranchCode.DataBind();
            ddlBranchCode.DataTextField = "BranchCode";
            ddlBranchCode.DataValueField = "BranchCode";
            ddlBranchCode.DataSource = Dealer.GetListBranchOfDealer(dealer);
            ddlBranchCode.DataBind();
        }
    }
    protected void GetDealerList()
    {
        ddlDealer.DataTextField = "BranchCode";
        ddlDealer.DataValueField = "BranchCode";
        ddlDealer.DataSource = Dealer.GetListDealerByDatabase(UserHelper.DatabaseCode);
        ddlDealer.DataBind();

        GetBranchs(this.CurrentDealer);
    }

    protected void InitPCVForm(Exchangepartheader ExchangeHeader)
    {
        txtexDamagedDate.Text = DateTime.Now.ToShortDateString();
        txtexRepairDate.Text = DateTime.Now.ToShortDateString();
        if (ExchangeHeader != null) txtexReceipt.Text = ExchangeHeader.Vouchernumber;  // AddExchange.GenExchangeNumber(dealerCode);
    }
    protected void BindServiceType(int Servicetype)
    {
        bool maint = false, rep = false, warr = false;
        switch ((ServiceType)Servicetype)
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
    }
    protected void GetItemInfo(string engineNo)
    {
        ItemInstanceDataSource ds = new ItemInstanceDataSource();
        IList<ItemInstance> list = ds.Select(2, 0, engineNo, null);
        WarrantyInfo warrInfo = ServiceTools.GetWarrantyInfo(engineNo);

        bool exist = (list != null) && (list.Count > 0);

        Info.LastKm = (warrInfo != null) ? warrInfo.KmCount : 0;
        Info.ItemSoldDealer = (exist) ? list[0].DealerCode : "";
        Info.ItemDBCode = (exist) ? list[0].DatabaseCode : "";
        Info.IsItemOnTipTop = (exist) ? list[0].ItemInstanceId > 0 : false;
        Info.IsItemExist = exist;

        ShowItemStatus();
    }

    private void LoadPCV(string exNumber)
    {
        Exchangepartheader exH = RepairListDataSource.GetExchangeHeader(exNumber);

        if (
            (exH == null) ||
            (UserHelper.IsDealer && (exH.Dealercode != UserHelper.DealerCode))
           )
        {
            AddError(WarrantyContentErrorCode.ServiceSheetNumberNotFound);
            Info.CurrentSate = SrsState.SheetNotFound;
        }
        else
        {
            LoadSRS(exH.Serviceheader);
            Info.CurrentSate = SrsState.PlayOldPCV;
        }
    }
    private void LoadSRS(string SheetNumber)
    {
        Serviceheader serH = RepairListDataSource.GetServiceHeader(SheetNumber);

        if (
            (serH == null) ||
            (UserHelper.IsDealer && (serH.Dealercode != UserHelper.DealerCode))
           )
        {
            AddError(WarrantyContentErrorCode.ServiceSheetNumberNotFound);
            Info.CurrentSate = SrsState.SheetNotFound;
        }
        else
        {
            LoadSRS(serH);
            Info.CurrentSate = SrsState.PlayOldSRS;
        }
    }
    protected void LoadTempSRS(long id)
    {
        Serviceheader serH = WarrantyContent.GetServiceSheet(id).ConvertServiceHeader();

        if (
            (serH == null)
            //#warning test
            || (serH.Status != (int)ServiceStatus.Temp)
            || (UserHelper.IsDealer && (serH.Dealercode != UserHelper.DealerCode))
           )
        {
            AddError(WarrantyContentErrorCode.ServiceSheetNumberNotFound);
            Info.CurrentSate = SrsState.SheetNotFound;
        }
        else
        {
            LoadSRS(serH);
            Info.CurrentSate = SrsState.PlayOldSRS;
        }
    }
    protected void LoadSRS(Serviceheader serH)
    {
        Info.ServiceHeader = serH;
        if (Info.ServiceHeader != null)
        {
            GetItemInfo(Info.ServiceHeader.Enginenumber);
            Info.ServiceDetail = SrsInfo.ConvertToSRSItem(WarrantyContent.GetServiceDetail(serH.Id), ServiceItemState.Persistent);
            Info.ExchangePartHeader = WarrantyContent.GetExchangePartHeader(serH.Id);
            Info.ReadOnly = Info.ServiceHeader.Status == (int)ServiceStatus.Done;
            Info.HasPCV = Info.ExchangePartHeader != null;
            if (Info.HasPCV)
            {
                Info.ExchangePartDetail = SrsInfo.ConvertToPCVItem(WarrantyContent.GetExchangePartDetail(Info.ExchangePartHeader.Id), ServiceItemState.Persistent);
            }
            CurrentDealer = serH.Dealercode;
            CurrentBranch = serH.Branchcode;
        }
    }

    protected void UpdateCustomer(Customer cust)
    {
        Info.ServiceHeader.Customer = Info.ExchangePartHeader.Customer = cust;
        ShowCustInfo(cust);
    }

    protected void SetReadOnly(WebControl obj, bool readOnly)
    {
        if (obj == null) return;
        if (obj is TextBox)
        {
            obj.CssClass = (readOnly) ? "readOnlyInputField" : "";
            (obj as TextBox).ReadOnly = readOnly;
        }
        if ((obj is Panel) || (obj is DropDownList))
        {
            obj.CssClass = (readOnly) ? "readOnlyInputField" : "";
        }
        if (!(obj is TextBox))
        {
            obj.Enabled = !readOnly;
        }
    }
    protected void RefreshSate()
    {
        bool _saved = (Info.CurrentSate == SrsState.Saved);
        bool _finish = (Info.CurrentSate == SrsState.FinishEdit) || Info.ReadOnly;

        SetReadOnly(ddlBranchCode, _finish);
        SetReadOnly(ddlDealer, _finish);
        SetReadOnly(btnPrint, !(Info.ReadOnly || _saved || SrsSetting.allWayPrint));  // print SRS
        SetReadOnly(btnCheckModel, _finish);
        SetReadOnly(btnFindCust, _finish || string.IsNullOrEmpty(Info.ServiceHeader.Enginenumber)); // select cust
        //SetReadOnly(txtEngineNo, _finish);
        SetReadOnly(ddlColour, _finish || Info.IsItemExist);
        SetReadOnly(txtKm, _finish);
        SetReadOnly(txtModel, _finish || Info.IsItemExist);
        btnSearchkModel.Visible = !(_finish || Info.IsItemExist);

        SetReadOnly(txtNumberPlate, _finish || !Info.CanChangePlateNumber);
        SetReadOnly(txtFrameNumber, _finish || !Info.CanChangeFrameNumber);

        SetReadOnly(txtBuyDate, !Info.CanChangeBuyDate || _finish);
        SetReadOnly(ibtnCalendar, !Info.CanChangeBuyDate || _finish);

        SetReadOnly(txtRepairDate, _finish);
        SetReadOnly(ibtnCalendarR, _finish);

        SetReadOnly(chblSerList, _finish);
        SetReadOnly(txtErrorStatus, _finish);
        SetReadOnly(txtRepair, _finish);
        SetReadOnly((WebControl)txtFee, _finish && SrsSetting.allowInputSRSFeeAfterComplete);

        btnPCVCallSelSpare.Enabled = !_finish;
        btnPrintPcv.Enabled = (_saved && Info.HasPCV) || Info.ReadOnly;
        btnSaveTemp.Enabled = !_saved && !Info.ReadOnly && (Info.CurrentSate != SrsState.Saved);
        btnSave.Enabled = _finish && (Info.CurrentSate != SrsState.Saved) && !Info.ReadOnly;
        btnEdit.Visible = _finish && !Info.ReadOnly && (Info.CurrentSate != SrsState.Saved);
        btnCheckDataInput.Visible = !_finish;

        //this.EnableControl(gvSpareList, !_finish && !Info.ReadOnly);
        //this.EnableControl(gvexSpareList, !_finish && !Info.ReadOnly);
        gvSpareList.Enabled = gvexSpareList.Enabled = !_finish && !Info.ReadOnly;
        gvSpareList.ShowFooter = gvSpareList.Enabled;
        //gvexSpareList.ShowFooter = gvexSpareList.Enabled;

        SetReadOnly(txtexDamagedDate, _finish); SetReadOnly(ibtnDamagedDateCalendar, _finish);
        SetReadOnly(rblRoad, _finish);
        SetReadOnly(rblSpeed, _finish);
        SetReadOnly(rblTransport, _finish);
        SetReadOnly(rblWeather, _finish);
        SetReadOnly(txtexElectricalDmg, _finish);
        SetReadOnly(txtexEngineDmg, _finish);
        SetReadOnly(txtexFeeOffer, _finish);
        SetReadOnly(txtexFrameDmg, _finish);
        SetReadOnly(txtexDamage, _finish);
        SetReadOnly(txtexReason, _finish);
        SetReadOnly(txtexNote, _finish);

        udpPage.Visible = Info.CurrentSate != SrsState.SheetNotFound;
    }

    protected void RefreshSRSItemInfo()
    {
        Serviceheader serH = Info.ServiceHeader;
        txtEngineNo.Text = serH.Enginenumber;
        txtKm.Text = (serH.Kmcount == 0) ? "" : serH.Kmcount.ToString();
        txtNumberPlate.Text = serH.Numberplate;
        txtFrameNumber.Text = serH.Framenumber;
        txtBuyDate.Text = (serH.Purchasedate == DateTime.MinValue) ? "" : serH.Purchasedate.ToShortDateString();
        txtModel.Text = serH.Itemtype;
        txtLastKm.Text = ((Info.ReadOnly) || (Info.LastKm == 0)) ? "" : Info.LastKm.ToString();
        // item color 
        if (ListContainsValue(ddlColour.Items, serH.Colorcode)) ddlColour.SelectedValue = serH.Colorcode;
        else
        {
            if (Info.IsItemExist && !Info.ReadOnly) ddlColour.Items.Clear();    // readonly=>ko can thiet lock color
            ddlColour.Items.Add(new ListItem(serH.Colorcode, serH.Colorcode));
            ddlColour.SelectedValue = serH.Colorcode;
        }
        if (Info.HasPCV)
        {
            txtexEngineNumber.Text = Info.ExchangePartHeader.Enginenumber = serH.Enginenumber;
            txtexModel.Text = Info.ExchangePartHeader.Model = serH.Itemtype;
            txtexFrameNum.Text = Info.ExchangePartHeader.Framenumber = serH.Framenumber;
            Info.ExchangePartHeader.Purchasedate = serH.Purchasedate;
            txtexBuyDate.Text = (Info.ExchangePartHeader.Purchasedate == DateTime.MinValue) ? "" : Info.ExchangePartHeader.Purchasedate.ToShortDateString();
        }
    }
    protected void RefreshSRSHeder()
    {
        Serviceheader serH = Info.ServiceHeader;

        RefreshSRSItemInfo();

        txtSheetNo.Text = serH.Servicesheetnumber;
        txtErrorStatus.Text = serH.Damaged;
        txtRepair.Text = serH.Repairresult;
        if (serH.Servicedate > DateTime.MinValue) txtRepairDate.Text = serH.Servicedate.ToShortDateString();

        ShowCustInfo(serH.Customer);
        BindServiceType(Info.ServiceHeader.Servicetype);
    }
    protected void RefreshPCVItemInfo()
    {
        if (Info.HasPCV)
        {
            Exchangepartheader ExchangeHeader = Info.ExchangePartHeader;
            txtexEngineNumber.Text = ExchangeHeader.Enginenumber;
            txtexFrameNum.Text = ExchangeHeader.Framenumber;
            txtexKm.Text = ExchangeHeader.Kmcount.ToString();
            txtexModel.Text = ExchangeHeader.Model;
            if ((ExchangeHeader.Purchasedate != null) && (ExchangeHeader.Purchasedate > DateTime.MinValue)) txtexBuyDate.Text = ExchangeHeader.Purchasedate.ToShortDateString();
            if ((ExchangeHeader.Exportdate != null) && (ExchangeHeader.Exportdate > DateTime.MinValue)) txtexExportDate.Text = ExchangeHeader.Exportdate.ToShortDateString();
            else txtexExportDate.Text = "";
        }
    }
    protected void RefreshPCVHeder()
    {
        if (Info.HasPCV)
        {
            Exchangepartheader ExchangeHeader = Info.ExchangePartHeader;
            //if (!Info.ReadOnly) 
            this.UpdateDealerInfo();
            this.InitPCVForm(ExchangeHeader);
            this.RefreshPCVItemInfo();

            if (ExchangeHeader != null)
            {
                //Iteminstance item = AddExchange.GetItemInstance(ExchangeHeader.Enginenumber);
                //if (item != null) ExchangeHeader.Exportdate = item.Madedate;

                if ((ExchangeHeader.Damageddate != null) && (ExchangeHeader.Damageddate > DateTime.MinValue)) txtexDamagedDate.Text = ExchangeHeader.Damageddate.ToShortDateString();
                if ((ExchangeHeader.Exchangeddate != null) && (ExchangeHeader.Exchangeddate > DateTime.MinValue)) txtexRepairDate.Text = ExchangeHeader.Exchangeddate.ToShortDateString();
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
            }
        }
    }
    protected void RefreshContent()
    {
        ClearContent();

        RefreshSRSHeder();
        BindSRSItems();

        RefreshPCVHeder();
        BindPCVItems();

        RefreshSate();
        SwitchExchangeSection(Info.HasPCV);
    }
    protected void RefreshStatistic()
    {
    }
    protected void ClearContent()
    {
    }

    protected void SwitchExchangeSection(bool isTurnOn)
    {
        Info.HasPCV = isTurnOn;
        if (Info.ExchangePartHeader == null)
        {
            Info.ExchangePartHeader = new Exchangepartheader();
        }
        Tabs.Tabs[1].Enabled = isTurnOn;
        rfvexDamage.ValidationGroup = (isTurnOn) ? "Save" : "none";
        rfvexReason.ValidationGroup = (isTurnOn) ? "Save" : "none";
        revexTotalFee.ValidationGroup = (isTurnOn) ? "Save" : "none";
    }

    protected void CollectData()
    {
        //long srsFee, exFee;
        long lastKm, fee, exfee, km;
        long.TryParse(txtLastKm.Text, out lastKm);
        DateTime repairDate, damageDate, expDate, buyDate;
        DateTime.TryParse(txtexExportDate.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out expDate);
        DateTime.TryParse(txtRepairDate.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out repairDate);
        DateTime.TryParse(txtexDamagedDate.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out damageDate);
        DateTime.TryParse(txtBuyDate.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out buyDate);
        long.TryParse(txtKm.Text, out km);
        long.TryParse(txtFee.Text, out fee);
        long.TryParse(txtexFeeOffer.Text, out exfee);

        Info.LastKm = lastKm;

        Info.ServiceHeader.Servicetype = GetSelectedServices();
        Info.ServiceHeader.Dealercode = Info.ExchangePartHeader.Dealercode = CurrentDealer;
        Info.ServiceHeader.Branchcode = CurrentBranch;
        Info.ServiceHeader.Feeamount = fee;
        Info.ServiceHeader.Damaged = txtErrorStatus.Text.Trim();
        Info.ServiceHeader.Repairresult = txtRepair.Text.Trim();
        Info.ServiceHeader.Framenumber = txtFrameNumber.Text;
        Info.ServiceHeader.Numberplate = txtNumberPlate.Text;
        Info.ServiceHeader.Servicedate = repairDate;
        Info.ServiceHeader.Purchasedate = buyDate;
        Info.ServiceHeader.Itemtype = txtModel.Text;
        Info.ServiceHeader.Colorcode = ddlColour.SelectedValue;
        Info.ServiceHeader.Kmcount = km;

        Info.ExchangePartHeader.Areacode = txtexAreaCode.Text;
        Info.ExchangePartHeader.Comments = txtexNote.Text.Trim();
        Info.ExchangePartHeader.Damaged = txtexDamage.Text;
        Info.ExchangePartHeader.Damageddate = damageDate;
        Info.ExchangePartHeader.Dealercode = CurrentDealer;
        Info.ExchangePartHeader.Electric = txtexElectricalDmg.Text.Trim();
        Info.ExchangePartHeader.Engine = txtexEngineDmg.Text.Trim();
        Info.ExchangePartHeader.Exchangeddate = repairDate;
        Info.ExchangePartHeader.Exportdate = expDate;
        Info.ExchangePartHeader.Feeamount = exfee;
        Info.ExchangePartHeader.Frame = txtexFrameDmg.Text.Trim();
        Info.ExchangePartHeader.Kmcount = km;
        Info.ExchangePartHeader.Model = txtModel.Text;
        Info.ExchangePartHeader.Purchasedate = buyDate;
        Info.ExchangePartHeader.Reason = txtexReason.Text;
        Info.ExchangePartHeader.Road = int.Parse(rblRoad.SelectedValue);
        Info.ExchangePartHeader.Speed = int.Parse(rblSpeed.SelectedValue);
        Info.ExchangePartHeader.Usage = int.Parse(rblTransport.SelectedValue);
        Info.ExchangePartHeader.Weather = int.Parse(rblWeather.SelectedValue);

        Info.SyncData();
    }
    protected int GetSelectedServices()
    {
        int sers = 0;
        foreach (ListItem item in chblSerList.Items) { if (item.Selected) sers += Convert.ToInt32(item.Value); }
        return sers;
    }
    protected bool SaveExchangeSpares(ServiceStatus status)
    {
        WarrantyContentErrorCode err = WarrantyContentErrorCode.OK;

        // check for any exchange spares exist
        // ret true if no one to save
        if ((Info.ServiceHeader.Servicetype == (int)ServiceType.Maintain) ||
            (Info.ServiceHeader.Servicetype == (int)ServiceType.MaintainAndRepair) ||
            (Info.ServiceHeader.Servicetype == (int)ServiceType.Repair)) return true;
        if (!Info.HasPCV || (Info.ExchangePartHeader == null) || (Info.ExchangePartDetail == null) || (Info.ExchangePartDetail.Count < 1)) return true;

        // save header
        for (int i = 0; i < 5; i++) // try 5 time to ensure
        {
            err = AddExchange.SaveExchHeader(Info.ExchangePartHeader, Info.ServiceHeader);
            if (err == WarrantyContentErrorCode.OK) break;
        }
        if (err != WarrantyContentErrorCode.OK) { AddError(err); return false; }

        // save detail
        err = AddExchange.SaveExchDetails(Info.ExchangePartDetail, Info.ExchangePartHeader);
        if (err != WarrantyContentErrorCode.OK) { AddError(err); return false; }

        return true;
    }
    protected bool SaveSheet(ServiceStatus status)
    {
        // check service type
        //if (chblSerList.SelectedIndex < 0) { AddError(WarrantyContentErrorCode.InvalidServiceType); return false; }

        ////////////////////////////////////////
        WarrantyContentErrorCode error = WarrantyContentErrorCode.OK;

        // header info
        CollectData();
        Info.ServiceHeader.Status = Info.ExchangePartHeader.Status = (int)status;

        using (TransactionBlock trans = new TransactionBlock())
        {
            // check last km count before save
            WarrantyInfo warrInfo = ServiceTools.GetWarrantyInfo(Info.ServiceHeader.Enginenumber);
            if ((warrInfo != null) && (Info.LastKm != warrInfo.KmCount) && (warrInfo.KmCount > 0))
            {
                AddError(WarrantyContentErrorCode.LastKmChanged);
                txtLastKm.Text = warrInfo.KmCount.ToString();
                Info.OldLastKm = Info.LastKm;
                return false;
            }

            // save service header                                                  // ddlBranchCode.SelectedValue  
            Serviceheader serH = WarrantyContent.SaveSerHeader(Info.ServiceHeader, out error);//_editSheetId, status, CurrentDealer, CurrentBranch, hdEngineNumber.Value, plateNumber, frameNumber, model, color, errStatus, solution, _exchangeNumber, serviceType, _custInfo, km, fee, total, serDate, buyDate, out error);
            if ((serH == null) || (error != WarrantyContentErrorCode.OK)) { AddError(error); trans.IsValid = false; return false; }
            Info.ServiceHeader = serH;

            // save service details
            error = WarrantyContent.SaveSerDetails(Info.ServiceDetail, serH);
            Info.ServiceDetail = Info.ServiceDetail.Where(p => p.State != ServiceItemState.Deleted).ToList();

            if (error != WarrantyContentErrorCode.OK) { AddError(error); trans.IsValid = false; return false; }

            // save exchange spares (also addErr)
            trans.IsValid = SaveExchangeSpares(status);

            // save warranty info 
            if ((Info.IsItemExist) && (status != ServiceStatus.Temp))
            {
                Customer initCus = null;
                if ((warrInfo == null) || (warrInfo.Customer == null))
                {
                    //Clone new customer
                    var cus = Info.ServiceHeader.Customer;
                    initCus = CustomerHelper.SaveCustomer(cus.Identifynumber, cus.Fullname, cus.Gender, cus.Birthdate, cus.Address, cus.Provinceid, cus.Districtid, cus.Jobtypeid, cus.Email, cus.Tel, cus.Mobile, cus.Priority, (int)CusType.WarrantyInfo, cus.Customerdescription, cus.Precinct, cus.Dealercode, false);
                }
                if (!ServiceTools.SaveWarrantyInfo(Info.ServiceHeader.Enginenumber, Convert.ToInt32(Info.ServiceHeader.Kmcount), Info.ServiceHeader.Purchasedate, Info.ItemDBCode, Info.ServiceHeader.Itemtype, Info.ServiceHeader.Colorcode, Info.ItemSoldDealer, 0, initCus))
                {
                    AddError(WarrantyContentErrorCode.UpdateDataFailed);
                    trans.IsValid = false; return false;
                }
            }

            // show real sheet no
            txtSheetNo.Text = serH.Servicesheetnumber;
            // refresh exchange items with saved VoucherNumber
            //if ((_exchangeHeader != null) && (AddExchange1.SpareList != null) && (AddExchange1.SpareList.Rows.Count > 0)) CopyExchangeList();

            // reload to get id for new items
            Info.ServiceDetail = SrsInfo.ConvertToSRSItem(WarrantyContent.GetServiceDetail(serH.Id), ServiceItemState.Persistent);
            if (Info.HasPCV)
            {
                Info.ExchangePartDetail = SrsInfo.ConvertToPCVItem(WarrantyContent.GetExchangePartDetail(Info.ExchangePartHeader.Id), ServiceItemState.Persistent);
            }
        }
        return true;
    }

    protected void FirstTimeInit()
    {
        gvexSpareList.ShowFooter = false;

        Info = new SrsInfo();
        ShowDealerForm();
        Info.RequestURL = Request.Url.ToString();

        UpdateDealerInfo();
        LoadAllColor();

        //lnkCheckModel.NavigateUrl = string.Format(lnkCheckModel.NavigateUrl, this.Info.PageKey.ToString());
        txtBuyDate.Text = DateTime.Now.ToShortDateString();
        txtRepairDate.Text = txtexRepairDate.Text = DateTime.Now.ToShortDateString();

        gvSpareList.Columns[7].Visible = SrsSetting.showFeeAmountColumn;
        //bool showWarrantyFee = string.IsNullOrEmpty(CurrentDealer);
        bool showWarrantyFee = !UserHelper.IsDealer;
        gvexSpareList.Columns[8].Visible = showWarrantyFee;
        gvexSpareList.Columns[9].Visible = showWarrantyFee;
        gvexSpareList.Columns[10].Visible = showWarrantyFee;

        // add new customer /////////////////
        string js = "if(retrieve_lookup_data('" + txtCustId.ClientID + "','../vehicle/sale/CusInfInput.aspx?')==false) return false;";
        btnNewCus.OnClientClick = js;
        btnNewCus.UseSubmitBehavior = false;
        // save buttons
        btnSave.OnClientClick = "if(!SubmitConfirm(" + btnSave.ClientID + ",'" + Resources.Question.SaveData + "')) return false;";
        btnSaveTemp.OnClientClick = btnSave.OnClientClick;
        // history and repair list
        btnHistory.OnClientClick = "window.open('repairhistory.aspx?engnum=','repairhis',''); return false;";
        btnList.OnClientClick = "window.open('repairlist.aspx','repairlist',''); return false;";
        // check km count for dealer
        cvKmCount.ValidationGroup = (!UserHelper.IsDealer) ? "None" : "Save";

        ////SwitchExchangeSection();
        //if (!Page.ClientScript.IsStartupScriptRegistered(typeof(Page), "switchTab"))
        //{
        //    Page.ClientScript.RegisterStartupScript(typeof(Page), "switchTab", "setTimeOut('switchTab()', 500);", true);
        //}

        // date time checker
        rvBuyDate.MinimumValue = rvRepairDate.MinimumValue = DateTime.MinValue.ToShortDateString();
        rvRepairDate.MaximumValue = rvBuyDate.MaximumValue = DateTime.Now.ToShortDateString();
        // require one item selected
        rovServiceList.ErrorMessage = Message.WarrantyContent_InvalidServiceType;
    }

    #endregion

    #region Events

    // SRS
    protected void ddlDealer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            GetBranchs(this.CurrentDealer);
            UpdateDealerInfo();
        }
    }
    protected void ddlDealer_DataBound(object sender, EventArgs e)
    {
        DropDownList drop = (DropDownList)sender;
        foreach (ListItem item in drop.Items)
        {
            item.Text += "      " + DealerHelper.GetNameI(item.Text);
        }
        drop.Items.Insert(0, "");

        if (!IsPostBack)
        {
            drop.SelectedIndex = 0;
        }
    }
    protected void ddlColour_DataBound(object sender, EventArgs e)
    {
        DropDownList drop = (DropDownList)sender;
        foreach (ListItem item in drop.Items)
        {
            item.Text = item.Value + " (" + item.Text + ")";
        }
    }

    protected void gvSpareList_UpdateRow(object sender, EventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((Control)sender).NamingContainer;
        if (gvr != null)
        {
            Literal litSpareNum = WebTools.FindControlById("litSpareNumber", gvr) as Literal;
            TextBox txtQty = WebTools.FindControlById("txtQuantity", gvr) as TextBox;
            TextBox txtUPrice = WebTools.FindControlById("txtSpareCost", gvr) as TextBox;
            TextBox txtName = WebTools.FindControlById("txtSpareName", gvr) as TextBox;

            int qty; long price;
            int.TryParse(txtQty.Text, out qty);
            long.TryParse(txtUPrice.Text, out price);

            SRSItem item = Info.FindServiceItem(litSpareNum.Text);
            if ((item != null) && (txtQty != null) && (txtUPrice != null))
            {
                item.HasModified = true;
                if (txtName.Visible) item.Partname = txtName.Text;  // phu tung ngoai luong`
                if (!string.IsNullOrEmpty(txtQty.Text)) item.Partqty = qty;
                if (item.Partqty == 0) item.Partqty = 1;
                if (!string.IsNullOrEmpty(txtUPrice.Text)) item.Unitprice = price;
            }
            BindSRSItems();
            udpSRSItems.Update();
        }
    }
    protected void gvSpareList_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvr = gvSpareList.FooterRow; if (gvr != null)
        {
            NumberFormatInfo ni = Thread.CurrentThread.CurrentCulture.NumberFormat;
            ni.NumberDecimalDigits = 0;

            long fee;
            if (long.TryParse(txtFee.Text, out fee)) { Info.ServiceHeader.Feeamount = fee; }
            Info.SyncData();

            txtFee.Text = Info.ServiceHeader.Feeamount.ToString();
            litSparesAmount.Text = Info.GetSparesAmount().ToString("N", ni);
            litTotalAmount.Text = Info.ServiceHeader.Totalamount.ToString("N", ni);
        }
    }
    protected void gvSpareList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // STT
            e.Row.Cells[0].Text = ((int)(e.Row.DataItemIndex + 1)).ToString();

            SRSItem curItem = (SRSItem)e.Row.DataItem;
            if (curItem != null)
            {
                NumberFormatInfo ni = Thread.CurrentThread.CurrentCulture.NumberFormat;
                ni.NumberDecimalDigits = 0;

                // check for valid spare number 
                Warrantycondition warr = WarrantyContent.GetWarrantyCondition(curItem.Partcode);
                Literal lit = (Literal)WebTools.FindControlById("litSpareName", e.Row); if (lit != null) lit.Visible = (warr != null);
                TextBox txt = (TextBox)WebTools.FindControlById("txtSpareName", e.Row); if (txt != null) txt.Visible = (warr == null);
                Label lb = (Label)WebTools.FindControlById("lbInvalidSpareNumber", e.Row); if (lb != null) lb.Visible = (warr == null);

                // change color to indicate Free Warranty 
                if (!string.IsNullOrEmpty(curItem.ExchangeNumber)) e.Row.CssClass = "readOnlyRow";

                // format number
                if (!((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit))
                {
                    if (!string.IsNullOrEmpty(curItem.ExchangeNumber))
                    {
                        // quantity
                        e.Row.Cells[3].Text = curItem.Partqty.ToString("N", ni);
                        // unit price
                        e.Row.Cells[4].Text = curItem.Unitprice.ToString("N", ni);
                    }
                    //  fee amount
                    if (SrsSetting.showFeeAmountColumn) e.Row.Cells[7].Text = curItem.FeeAmount.ToString("N", ni);
                }
                // spare amount
                e.Row.Cells[6].Text = curItem.SpareAmount.ToString("N", ni);

                // update ValidationGroup (does not work!!!)
                TextBox txtQty = WebTools.FindControlById("txtQuantity", e.Row) as TextBox;
                TextBox txtUPrice = WebTools.FindControlById("txtSpareCost", e.Row) as TextBox;
                if (txtQty != null) txtQty.ValidationGroup = string.Format("EditSpare{0}", e.Row.RowIndex.ToString());
                if (txtUPrice != null) txtUPrice.ValidationGroup = string.Format("EditSpare{0}", e.Row.RowIndex.ToString());
            }

        }
    }

    protected void btnSelectCust_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        Customer cust = PopupHelper.GetSelectCusSession(Info.PageKey.ToString());
        UpdateCustomer(cust);
        udpSRS.Update();
        udpPCV.Update();
    }
    protected void btnReloadSRSItems_Click(object sender, EventArgs e)
    {
        BindSRSItems();
        udpSRSItems.Update();
    }
    protected void btnCheckModel_Click(object sender, EventArgs e)
    {
        Info.ServiceHeader.Itemtype = Info.ExchangePartHeader.Model = txtModel.Text = PopupHelper.GetSelectModelSession(Info.PageKey.ToString());
        udpSRS.Update();
    }

    //protected void txtAddSpareQuantity_DataBinding(object sender, EventArgs e)
    //{
    //    TextBox tb = sender as TextBox;
    //    SRSItem item = Info.FindServiceItem(tb.ToolTip);
    //    if (item != null) tb.Text = item.Partqty.ToString();
    //}
    //protected void txtAddSpareUnitPrice_DataBinding(object sender, EventArgs e)
    //{
    //    TextBox tb = sender as TextBox;
    //    SRSItem item = Info.FindServiceItem(tb.ToolTip);
    //    if (item != null) tb.Text = item.Unitprice.ToString();
    //}

    protected void imbDelete_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btn = sender as ImageButton;
        int index = Info.ServiceDetail.FindIndex(sd => sd.Partcode == btn.CommandArgument && sd.State != ServiceItemState.Deleted);
        if (index >= 0)
        {
            if (Info.ServiceDetail[index].State == ServiceItemState.Transient) Info.ServiceDetail.RemoveAt(index);
            else
            {
                Info.ServiceDetail[index].State = ServiceItemState.Deleted;
                Info.ServiceDetail[index].HasModified = true;
            }
            BindSRSItems();
            udpSRSItems.Update();
        }
        //SRSItem item = Info.FindServiceItem(btn.CommandArgument);
        //if (item != null)
        //{
        //    if (item.State == ServiceItemState.Transient) Info.ServiceDetail.Remove(item);
        //    else
        //    {
        //        item.State = ServiceItemState.Deleted;
        //        item.HasModified = true;
        //    }
        //    BindSRSItems();
        //    udpSRSItems.Update();
        //}
    }
    protected void imbAddNewSpare_Click(object sender, ImageClickEventArgs e)
    {
        long unitPrice = 0; int quantity = 0; string spareNumber = "", spareName = "";
        GridViewRow gvr = gvSpareList.FooterRow;

        // unit price
        TextBox tbP = WebTools.FindControlById("txtNewSparePrice", gvr) as TextBox;
        if (tbP != null) long.TryParse(tbP.Text, out unitPrice);
        // quantity
        TextBox tbQ = WebTools.FindControlById("txtNewSpareQuantity", gvr) as TextBox;
        if (tbQ != null) int.TryParse(tbQ.Text, out quantity);
        // spare name
        TextBox tbName = WebTools.FindControlById("txtNewSpareName", gvr) as TextBox;
        if (tbName != null) spareName = tbName.Text;
        // spare number
        TextBox tbNum = WebTools.FindControlById("txtNewSpareNumber", gvr) as TextBox;
        if (tbNum != null) spareNumber = tbNum.Text;
        if (!string.IsNullOrEmpty(spareNumber))
        {
            this.AddSRSItem(spareNumber, spareName, quantity, unitPrice);
            BindSRSItems();
            udpSRSItems.Update();
        }
    }

    protected void HeaderInfo_Changed(object sender, EventArgs e)
    {
        CollectData();
        RefreshSRSHeder();
        RefreshPCVHeder();
    }
    protected void chblSerList_Changed(object sender, EventArgs e)
    {
        SwitchExchangeSection(chblSerList.Items[2].Selected);
        RefreshSRSItemInfo();
        RefreshPCVHeder();
        BindPCVItems();
        udpPCV.Update();
        udpPCVItems.Update();
    }
    protected void chblSerList_OnLoad(object sender, EventArgs e)
    {
        chblSerList.Items[2].Attributes.Add("onclick", "switchTab()");
    }

    // PCV
    protected void PCVItems_UpdateRow(object sender, EventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((Control)sender).NamingContainer;
        if (gvr != null)
        {
            Literal litSpareNum = WebTools.FindControlById("litexSpareNumber", gvr) as Literal;
            TextBox txtQty = WebTools.FindControlById("txtexQuantity", gvr) as TextBox;
            TextBox txtSr = WebTools.FindControlById("txtexSerialNumber", gvr) as TextBox;
            DropDownList dropB = gvr.FindControl("ddlBroken") as DropDownList;

            int qty;
            int.TryParse(txtQty.Text, out qty);

            PCVItem item = Info.FindExchangeItem(litSpareNum.Text);
            if ((item != null) && (txtQty != null) && (dropB != null) && (txtSr != null))
            {
                Broken broken = WarrantyContent.GetBroken(dropB.SelectedValue);
                if (qty == 0) qty = 1;

                item.Serialnumber = txtSr.Text.Trim();
                item.Partqtyo = item.Partqtym = qty;
                if (broken != null) item.Broken = broken;
            }

            PCVItemsUpdated();
            BindPCVItems();
            udpPCVItems.Update();
        }
    }
    protected void gvexSpareList_DataBound(object sender, EventArgs e)
    {

    }
    protected void gvexSpareList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //EmptyGridViewEx gv = (EmptyGridViewEx)sender;
        //double val;

        if ((Info.ExchangePartDetail.Count > 0) && (e.Row.RowType == DataControlRowType.DataRow))
        {
            PCVItem item = (PCVItem)e.Row.DataItem;

            DropDownList drop = e.Row.FindControl("ddlBroken") as DropDownList;
            ListItem broken = drop.Items.FindByValue(drop.ToolTip);
            if (broken != null)
            {
                drop.SelectedIndex = drop.Items.IndexOf(broken);
            }
            else
            {
                drop.Items.Insert(1, new ListItem(drop.ToolTip, drop.ToolTip));
                drop.SelectedIndex = 1;
            }

            // format number
            NumberFormatInfo ni = Thread.CurrentThread.CurrentCulture.NumberFormat;
            ni.NumberDecimalDigits = 1;
            e.Row.Cells[8].Text = item.ManPower.ToString("N", ni);
            ni.NumberDecimalDigits = 0;
            e.Row.Cells[5].Text = item.Unitpriceo.ToString("N", ni);
            e.Row.Cells[6].Text = item.SpareAmountO.ToString("N", ni);
            e.Row.Cells[9].Text = item.Labour.ToString("N", ni);
            e.Row.Cells[10].Text = item.FeeAmount.ToString("N", ni);
        }
    }

    protected void btnReloadPCVItems_Click(object sender, EventArgs e)
    {
        BindPCVItems();
        udpPCVItems.Update();
    }
    protected void btnAddExchangeSpare_Click(object sender, EventArgs e)
    {
        int quantity = 0; string spareNumber = "", brokenCode = "", serial = "";
        GridViewRow gvr = (GridViewRow)((Control)sender).NamingContainer;
        // broken code
        DropDownList dropB = gvr.FindControl("ddlNewPCVBrokenCode") as DropDownList;
        if (dropB != null) brokenCode = dropB.SelectedValue;
        // quantity
        TextBox tbQ = gvr.FindControl("txtNewPCVQuantity") as TextBox;
        if (tbQ != null) int.TryParse(tbQ.Text, out quantity);
        // spare number
        TextBox tbN = gvr.FindControl("txtNewPCVSpareNumber") as TextBox;
        if (tbN != null) spareNumber = tbN.Text;
        // serial
        TextBox tbSr = WebTools.FindControlById("txtexSerialNumber", gvr) as TextBox;
        if (tbSr != null) serial = tbSr.Text;
        if (!string.IsNullOrEmpty(spareNumber))
        {
            this.AddPCVItem(spareNumber, quantity, serial, brokenCode);
            BindPCVItems();
            udpPCVItems.Update();
        }
    }
    protected void ImageButton1_DataBinding(object sender, EventArgs e)
    {
        ImageButton imgbtn = (ImageButton)sender;
        imgbtn.Visible = !Info.ReadOnly;
    }
    protected void imbDeletePCVItem_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btn = sender as ImageButton;
        int index = Info.ExchangePartDetail.FindIndex(ed => ed.Partcodeo == btn.CommandArgument);
        if (index >= 0)
        {
            if (Info.ExchangePartDetail[index].State == ServiceItemState.Transient) Info.ExchangePartDetail.RemoveAt(index);
            else
            {
                Info.ExchangePartDetail[index].State = ServiceItemState.Deleted;
                Info.ExchangePartDetail[index].HasModified = true;
            }
            BindPCVItems();
            udpPCVItems.Update();
        }
        //PCVItem item = Info.FindExchangeItem(btn.CommandArgument);
        //if (item != null)
        //{
        //    if (item.State == ServiceItemState.Transient) Info.ExchangePartDetail.Remove(item);
        //    else
        //    {
        //        item.State = ServiceItemState.Deleted;
        //        item.HasModified = true;
        //    }
        //    BindPCVItems();
        //    udpPCVItems.Update();
        //}
    }
    protected void ddlBroken_OnDataBound(object sender, EventArgs e)
    {
        DropDownList drop = sender as DropDownList;
        foreach (ListItem item in drop.Items)
        {
            if ((!string.IsNullOrEmpty(item.Value)) && !item.Text.Equals(item.Value))
                item.Text = string.Format("{0} - {1}", item.Value, item.Text);
        }
    }

    // page
    protected void Page_Load(object sender, EventArgs e)
    {
        errorCode.Clear();
        bllErrorMsg.Items.Clear();

        if (PreviousPage != null)
        {
            FirstTimeInit();

            Type t = PreviousPage.GetType();
            PropertyInfo[] props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == "info")
                {
                    object info = prop.GetValue(PreviousPage, null);
                    if (info != null) this.Info = (SrsInfo)info;
                    RefreshContent();
                    break;
                }
            }
        }
        else if (!IsPostBack)
        {
            FirstTimeInit();

            /////////////////////////////////////
            // SRS id to be edit
            long sid;
            if (long.TryParse(Request.QueryString["sid"], out sid))
            {
                // load temp SRS
                this.LoadTempSRS(sid);
            }
            // show data from other page's query
            string showServiceSheetNumber = Request.QueryString["srsn"];
            string showExchangeSparesNumber = Request.QueryString["pcvn"];

            if (!string.IsNullOrEmpty(showServiceSheetNumber)) LoadSRS(showServiceSheetNumber);
            else if (!string.IsNullOrEmpty(showExchangeSparesNumber)) LoadPCV(showExchangeSparesNumber);

            ////////////////////////////////////
            // reload content
            this.RefreshContent();

            if (Info.HasPCV && (Info.CurrentSate == SrsState.PlayOldPCV))
            {
                Tabs.ActiveTabIndex = 1;
            }

            // must be at end of !IsPostBack
            SwitchExchangeSection(Info.HasPCV);
        }
        else
        {
            // bind data for frint form
            //if (MultiView1.ActiveViewIndex == vwPrintIndex) btnPrint_Click(null, null);
        }

        // store fee amount
        TextBox tb = (TextBox)WebTools.FindControlById("txtFee", gvSpareList);
        if (tb != null)
        {
            long fee;
            if (long.TryParse(tb.Text, NumberStyles.Any, Thread.CurrentThread.CurrentCulture, out fee))
            {
                Info.ServiceHeader.Feeamount = fee;
            }
        }

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        // print parts change button
        btnPrintPcv.OnClientClick = string.Format("window.open('Report/PrintPartChange.aspx?pcvn={0}','printExchangeList',''); return false;", (Info.HasPCV) ? Info.ExchangePartHeader.Vouchernumber : "");

        txtCustId.Text = ""; // force always add new cust
        RefreshStatistic();
        ShowItemStatus();
        ShowError();
    }

    // command
    protected void btnSaveTemp_Click(object sender, EventArgs e)
    {
        if (SaveSheet(ServiceStatus.Temp))
        {
            Info.CurrentSate = SrsState.SavedTemp;
            RefreshContent();
            BindPCVItems();
            BindSRSItems();
            udpPCVItems.Update();
            udpSRSItems.Update();
            udpPCV.Update();
            udpSRS.Update();
        }
    }
    protected void btnCheckEngineNo_Click(object sender, EventArgs e)
    {
        ItemInstance item = PopupHelper.GetSelectEngineSession(Info.PageKey.ToString());
        if (item != null)
        {
            CheckEnteredEngineNo(item.EngineNumber, item.ItemInstanceId, item.ItemType, item.Color, item.DealerCode, item.BranchCode, item.DatabaseCode, (DateTime)item.MadeDate, item.ImportedDate);
            ShowCustInfo(Info.ServiceHeader.Customer);
            RefreshSate();
            RefreshSRSItemInfo();
            RefreshPCVItemInfo();
            udpSRS.Update();
            udpPCV.Update();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (SaveSheet(ServiceStatus.Done))
        {
            Info.CurrentSate = SrsState.Saved;
            Info.ReadOnly = true;
            RefreshContent();
            BindPCVItems();
            BindSRSItems();
            udpPCVItems.Update();
            udpSRSItems.Update();
            udpPCV.Update();
            udpSRS.Update();
        }
    }
    protected void btnCheckDataInput_Click(object sender, EventArgs e)
    {
        CollectData();
        if (CheckDataOnSave()) Info.CurrentSate = SrsState.FinishEdit;
        RefreshSate();
        BindSRSItems();
        BindPCVItems();
        udpPCVItems.Update();
        udpSRSItems.Update();
        udpPCV.Update();
        udpSRS.Update();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (Info.ServiceHeader.Id == 0) Info.CurrentSate = SrsState.AddNewSRS;
        else Info.CurrentSate = SrsState.PlayOldSRS;
        RefreshSate();
        if (Info.ServiceDetail.Count == 0) BindSRSItems();// for show footer :(
        udpPCVItems.Update();
        udpSRSItems.Update();
        udpPCV.Update();
        udpSRS.Update();
    }
    protected void btnSelectSpares_Click(object sender, EventArgs e)
    {
        List<WarrantySpare> srs = PopupHelper.GetSelectSRSSparesSession(Info.PageKey.ToString());
        if (srs.Count > 0)
        {
            foreach (WarrantySpare item in srs)
            {
                Info.UpdateSRSItem((Warrantycondition)item, item.Quantity, item.NewUnitPrice);
            }
            //srs.Clear();
            BindSRSItems();
            udpSRSItems.Update();
        }

        List<WarrantySpare> pcv = PopupHelper.GetSelectPCVSparesSession(Info.PageKey.ToString());
        if (pcv.Count > 0)
        {
            foreach (WarrantySpare item in pcv)
            {
                Info.UpdatePCVItem((Warrantycondition)item, item.Quantity);
            }
            //pcv.Clear();
            PCVItemsUpdated();
            BindPCVItems();
            udpPCVItems.Update();
        }
    }
    // print SRS
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        CollectData();
    }
    protected void btnNewCus_Click(object sender, EventArgs e)
    {
        #region copy from tnTung

        long lngCustId;
        long.TryParse(_CustomerID.Value, out lngCustId);
        string ActionString = "DEFAULT";

        ISession sess = NHibernateSessionManager.Instance.GetSession();
        IList lstCus = sess.CreateCriteria(typeof(Customer))
            //.Add(Expression.Eq("Identifynumber", txtCustId.Text.Trim()))
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
        Info.ServiceHeader.Customer = Info.ExchangePartHeader.Customer = sess.Get<Customer>(lngCustId);
        //_custInfo = WarrantyContent.GetCustInfos(lngCustId);
        ShowCustInfo(Info.ServiceHeader.Customer);
        tx.Commit();
    }

    #endregion

    private void ShowError()
    {
        hdSellDealer.Value = "";
        bllErrorMsg.Visible = errorCode.Count > 0;
        bllErrorMsg.Items.Clear();
        foreach (WarrantyContentErrorCode error in errorCode)
        {
            switch (error)
            {
                case WarrantyContentErrorCode.BrokenCodeNotFound: bllErrorMsg.Items.Add(Message.WarrantyContent_BrokenCodeNotFound); break;
                case WarrantyContentErrorCode.EngineNumberNotFound: bllErrorMsg.Items.Add(VDMS.VDMSSetting.CurrentSetting.CheckEngineNoForService ? Message.WarrantyContent_InvalidEngineNumber : Message.WarrantyContent_EngineNumberNotFound); break;
                case WarrantyContentErrorCode.ExchangeNumberNotFound: bllErrorMsg.Items.Add(Message.WarrantyContent_ExchangeNumberNotFound); break;
                case WarrantyContentErrorCode.InCompleteSpares: bllErrorMsg.Items.Add(Message.WarrantyContent_InCompleteSpares); break;
                case WarrantyContentErrorCode.InvalidDateTimeValue: bllErrorMsg.Items.Add(Message.WarrantyContent_InvalidDateTimeValue); break;
                case WarrantyContentErrorCode.InvalidExchangeSparesList: bllErrorMsg.Items.Add(Message.WarrantyContent_InvalidExchangeSparesList); break;
                case WarrantyContentErrorCode.InvalidServiceType: bllErrorMsg.Items.Add(Message.WarrantyContent_InvalidServiceType); break;
                case WarrantyContentErrorCode.InvalidSpareCode: bllErrorMsg.Items.Add(Message.WarrantyContent_InvalidSpareCode); break;
                case WarrantyContentErrorCode.ItemNotSold: bllErrorMsg.Items.Add(Message.WarrantyContent_ItemNotSold); break;
                case WarrantyContentErrorCode.ItemSoldByOtherDealer: bllErrorMsg.Items.Add(Message.WarrantyContent_ItemSoldByOtherDealer +
                        ((SrsSetting.showSourceDealerWhenAlarm && !UserHelper.IsDealer) ?
                        "  (" +
                            ((string.IsNullOrEmpty(Info.ItemSoldDealer)) ? Message.DataNotFound :
                            Info.ItemSoldDealer + " : " + DealerHelper.GetName(Info.ItemSoldDealer)) + ")" :
                        ""));
                    hdSellDealer.Value = Info.ItemSoldDealer;
                    break;
                case WarrantyContentErrorCode.ItemTypeNotFound: bllErrorMsg.Items.Add(Message.WarrantyContent_ItemTypeNotFound); break;
                case WarrantyContentErrorCode.LastKmChanged: bllErrorMsg.Items.Add(string.Format(Message.WarrantyContent_LastKmChanged, Info.OldLastKm)); break;
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

    protected void txtNumberPlate_TextChanged(object sender, EventArgs e)
    {
        Info.ServiceHeader.Numberplate = txtNumberPlate.Text;
    }
    protected void txtFrameNumber_TextChanged(object sender, EventArgs e)
    {
        Info.ServiceHeader.Framenumber = txtFrameNumber.Text;
    }
}
