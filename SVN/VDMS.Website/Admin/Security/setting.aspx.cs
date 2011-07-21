using System;
using System.Linq;
using System.Web.UI;
using VDMS;
using VDMS.Web;
using VDMS.Common.Utils;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class Admin_Security_setting : BasePage
{
    [WebMethod]
    public static bool IsAddingParts()
    {
        return VDMSHttpApplication.syncPartSchedulerI.OnSyncNew;
    }

    [WebMethod]
    public static bool IsUpdatingPartsPrice()
    {
        return VDMSHttpApplication.syncPartSchedulerI.OnSyncPrice;
    }

    bool bindingBanks = false;

    void BindBanks()
    {
        bindingBanks = true;
        gvBanks.DataSource = VDMSSetting.CurrentSetting.BankPaymentSettings;
        gvBanks.DataBind();
        bindingBanks = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblSaveError.Visible = false;
        lblSaveOk.Visible = false;

        //#warning remove on VDMSI-golive
        //UpdatePanel1.Visible = false;

        if (!Page.IsPostBack)
        {
            var obj = VDMSSetting.CurrentSetting;

            BindBanks();

            chkCheckEngineNoForPartsChange.Checked = obj.CheckEngineNoForPartsChange;
            chkCheckEngineNoForService.Checked = obj.CheckEngineNoForService;
            chkCheckWarrantyInfoDatabase.Checked = obj.CheckWarrantyInfoDatabase;

            txtOESR.Text = obj.OrderExcelUploadSetting.StartRow.ToString();
            txtOEPCC.Text = obj.OrderExcelUploadSetting.PartCodeColumn.ToString();
            txtOEQC.Text = obj.OrderExcelUploadSetting.QuantityColumn.ToString();
            txtOEMC.Text = obj.OrderExcelUploadSetting.ModelColumn.ToString();

            txtREStart.Text = obj.PartReplaceExcelUploadSetting.StartRow.ToString();
            txtREOldPart.Text = obj.PartReplaceExcelUploadSetting.OldPartCodeColumn.ToString();
            txtRENewPart.Text = obj.PartReplaceExcelUploadSetting.NewPartCodeColumn.ToString();
            txtREStatus.Text = obj.PartReplaceExcelUploadSetting.StatusColumn.ToString();

            txtPSStart.Text = obj.PartSpecExcelUploadSetting.StartRow.ToString();
            txtPSPartCode.Text = obj.PartSpecExcelUploadSetting.PartCodeColumn.ToString();
            txtPSPack.Text = obj.PartSpecExcelUploadSetting.PackingColumn.ToString();
            txtPSUnit.Text = obj.PartSpecExcelUploadSetting.UnitColumn.ToString();
            txtPSQuantity.Text = obj.PartSpecExcelUploadSetting.QuantityColumn.ToString();

            txtSESR.Text = obj.SalesExcelUploadSetting.StartRow.ToString();
            txtSEPCC.Text = obj.SalesExcelUploadSetting.PartCodeColumn.ToString();
            txtSEQC.Text = obj.SalesExcelUploadSetting.QuantityColumn.ToString();
            txtSEPTC.Text = obj.SalesExcelUploadSetting.PartTypeColumn.ToString();

            txtVDateFormat.Text = obj.SaleVehicleExcelUploadSetting.DateFormat;
            txtVAddress.Text = obj.SaleVehicleExcelUploadSetting.Address.ToString();
            txtVBillNo.Text = obj.SaleVehicleExcelUploadSetting.BillNumber.ToString();
            txtVCommentItem.Text = obj.SaleVehicleExcelUploadSetting.CommentSellItem.ToString();
            txtVCusDesc.Text = obj.SaleVehicleExcelUploadSetting.CustomerDescription.ToString();
            txtVCusId.Text = obj.SaleVehicleExcelUploadSetting.Id.ToString();
            txtVCusName.Text = obj.SaleVehicleExcelUploadSetting.CustomerName.ToString();
            txtVDistrict.Text = obj.SaleVehicleExcelUploadSetting.District.ToString();
            txtVDOB.Text = obj.SaleVehicleExcelUploadSetting.DOB.ToString();
            txtVEmail.Text = obj.SaleVehicleExcelUploadSetting.Email.ToString();
            txtVEngineNo.Text = obj.SaleVehicleExcelUploadSetting.EngineNumber.ToString();
            txtVGender.Text = obj.SaleVehicleExcelUploadSetting.Gender.ToString();
            txtVJobType.Text = obj.SaleVehicleExcelUploadSetting.JobType.ToString();
            txtVMobile.Text = obj.SaleVehicleExcelUploadSetting.Mobile.ToString();
            txtVNumberPlate.Text = obj.SaleVehicleExcelUploadSetting.NumberPlate.ToString();
            txtVNumberPlateDate.Text = obj.SaleVehicleExcelUploadSetting.NumberPlateRecDate.ToString();
            txtVPaymentDate.Text = obj.SaleVehicleExcelUploadSetting.PaymentDate.ToString();
            txtVPaymentType.Text = obj.SaleVehicleExcelUploadSetting.PaymentType.ToString();
            txtVPrecint.Text = obj.SaleVehicleExcelUploadSetting.Precint.ToString();
            txtVPrice.Text = obj.SaleVehicleExcelUploadSetting.Price.ToString();
            txtVPriority.Text = obj.SaleVehicleExcelUploadSetting.Priority.ToString();
            txtVProvince.Text = obj.SaleVehicleExcelUploadSetting.Province.ToString();
            txtVSellDate.Text = obj.SaleVehicleExcelUploadSetting.SellDate.ToString();
            txtVSellType.Text = obj.SaleVehicleExcelUploadSetting.SellType.ToString();
            txtVStartRow.Text = obj.SaleVehicleExcelUploadSetting.StartRow.ToString();
            txtVTel.Text = obj.SaleVehicleExcelUploadSetting.Tel.ToString();
            txtVInstalmentTimes.Text = obj.SaleVehicleExcelUploadSetting.InstalmentTimes.ToString();
            txtVFirstInstalmentAmount.Text = obj.SaleVehicleExcelUploadSetting.FirstInstalmentAmount.ToString();
            txtVDaysEachInstalment.Text = obj.SaleVehicleExcelUploadSetting.DaysEachInstalment.ToString();
            txtVPayingDate1.Text = obj.SaleVehicleExcelUploadSetting.PayingDate1.ToString();
            txtVPayingDate2.Text = obj.SaleVehicleExcelUploadSetting.PayingDate2.ToString();
            txtVPayingDate3.Text = obj.SaleVehicleExcelUploadSetting.PayingDate3.ToString();
            txtVPayingDate4.Text = obj.SaleVehicleExcelUploadSetting.PayingDate4.ToString();
            txtVPayingDate5.Text = obj.SaleVehicleExcelUploadSetting.PayingDate5.ToString();
            txtVAmount1.Text = obj.SaleVehicleExcelUploadSetting.Amount1.ToString();
            txtVAmount2.Text = obj.SaleVehicleExcelUploadSetting.Amount2.ToString();
            txtVAmount3.Text = obj.SaleVehicleExcelUploadSetting.Amount3.ToString();
            txtVAmount4.Text = obj.SaleVehicleExcelUploadSetting.Amount4.ToString();
            txtVAmount5.Text = obj.SaleVehicleExcelUploadSetting.Amount5.ToString();

            txtBAmount.Text = obj.BonusSetting.Amount.ToString();
            txtBDate.Text = obj.BonusSetting.BonusDate.ToString();
            txtBDateFormat.Text = obj.BonusSetting.DateFormat;
            txtBDesc.Text = obj.BonusSetting.Description.ToString();
            txtBPlan.Text = obj.BonusSetting.BonusPlan.ToString();
            txtBSource.Text = obj.BonusSetting.BonusSource.ToString();
            txtBStartRow.Text = obj.BonusSetting.StartRow.ToString();
            txtBStatus.Text = obj.BonusSetting.PlanMonth.ToString();
            txtBDealerCode.Text = obj.BonusSetting.DealerCode.ToString();

            txtWDateFormat.Text = obj.WarrantyPartSetting.DateFormat;
            txtWMotorCode.Text = obj.WarrantyPartSetting.MotorCode.ToString();
            txtWPartCode.Text = obj.WarrantyPartSetting.PartCode.ToString();
            txtWPartNameEN.Text = obj.WarrantyPartSetting.EnglishName.ToString();
            txtWPartNameVN.Text = obj.WarrantyPartSetting.VietnameseName.ToString();
            txtWStartDate.Text = obj.WarrantyPartSetting.StartDate.ToString();
            txtWStartRow.Text = obj.WarrantyPartSetting.StartRow.ToString();
            txtWStopDate.Text = obj.WarrantyPartSetting.EndDate.ToString();
            txtWWarrantyLength.Text = obj.WarrantyPartSetting.WarrantyLength.ToString();
            txtWWarrantyTime.Text = obj.WarrantyPartSetting.WarrantyTime.ToString();

            txtCCPC.Text = obj.CycleCountExcelUploadSetting.PartCodeColumn.ToString();
            txtCCQty.Text = obj.CycleCountExcelUploadSetting.QuantityColumn.ToString();
            txtCCSR.Text = obj.CycleCountExcelUploadSetting.StartRow.ToString();
            txtCCComment.Text = obj.CycleCountExcelUploadSetting.CommentColumn.ToString();
            //txtCCST.Text = obj.CycleCountExcelUploadSetting.SafetyStockColumn.ToString();
            //txtCCPT.Text = obj.CycleCountExcelUploadSetting.PartTypeColumn.ToString();

            tbPS.Text = obj.PaymentSpan.ToString();
            tbQS.Text = obj.QuotationSpan.ToString();
            tbSS.Text = obj.ShippingSpan.ToString();
            tbAIS.Text = obj.AutoInstockSpan.ToString();
            tbEMail.Text = obj.OverShippingEmail;
            txtMaxMonthAllowReopen.Text = obj.MaxMonthAllowReopen.ToString();
            txtMaxMonthAllowReopen1.Text = obj.MaxMonthAllowReopen1.ToString();

            chkDNFOrderDateControl.Checked = obj.OrderDateControl.Contains("DNF");
            chkHTFOrderDateControl.Checked = obj.OrderDateControl.Contains("HTF");

            chkAllowChangeOrderDate.Checked = obj.AllowChangeOrderDate;

            chbAutoCloseInvI.Checked = obj.AllowAutoCloseInvI;
            txtAutoCloseInvDayI.Text = obj.AutoCloseInvDayI.ToString();
            txtAutoCloseInvTimeI.Text = obj.AutoCloseInvTimeI.ToString();

            txtDefLabourI.Text = obj.DefaultLabourOnInsertPartI.ToString();
            chbAutoSyncPart.Checked = obj.AllowAutoSyncPartI;
            txtSyncPdays.Text = obj.AutoSyncPartDaysI.ToString();
            txtSyncPHour.Text = obj.AutoSyncPartHourI.ToString();
            if (obj.AutoSyncPartFromDateI != null) txtSyncPfromDate.Text = obj.AutoSyncPartFromDateI.ToShortDateString();

            chkCheckOrderPartNotDuplicateBeforeConfirmWhenSend.Checked = obj.CheckOrderPartNotDuplicateBeforeConfirmWhenSend;
            chkApplySubOrder.Checked = obj.ApplyCheckPartInSubOrder;
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        btnRefresh_Click(null, null);
    }

    protected void cmdSave_Click(object sender, EventArgs e)
    {
        var obj = VDMSSetting.CurrentSetting;
        obj.CheckEngineNoForPartsChange = chkCheckEngineNoForPartsChange.Checked;
        obj.CheckEngineNoForService = chkCheckEngineNoForService.Checked;
        obj.CheckWarrantyInfoDatabase = chkCheckWarrantyInfoDatabase.Checked;

        obj.OrderExcelUploadSetting.StartRow = int.Parse(txtOESR.Text);
        obj.OrderExcelUploadSetting.PartCodeColumn = int.Parse(txtOEPCC.Text);
        obj.OrderExcelUploadSetting.QuantityColumn = int.Parse(txtOEQC.Text);
        obj.OrderExcelUploadSetting.ModelColumn = int.Parse(txtOEMC.Text);

        obj.PartReplaceExcelUploadSetting.StartRow = int.Parse(txtREStart.Text);
        obj.PartReplaceExcelUploadSetting.OldPartCodeColumn = int.Parse(txtREOldPart.Text);
        obj.PartReplaceExcelUploadSetting.NewPartCodeColumn = int.Parse(txtRENewPart.Text);
        obj.PartReplaceExcelUploadSetting.StatusColumn = int.Parse(txtREStatus.Text);

        obj.PartSpecExcelUploadSetting.StartRow = int.Parse(txtPSStart.Text);
        obj.PartSpecExcelUploadSetting.PartCodeColumn = int.Parse(txtPSPartCode.Text);
        obj.PartSpecExcelUploadSetting.PackingColumn = int.Parse(txtPSPack.Text);
        obj.PartSpecExcelUploadSetting.UnitColumn = int.Parse(txtPSUnit.Text);
        obj.PartSpecExcelUploadSetting.QuantityColumn = int.Parse(txtPSQuantity.Text);

        obj.SalesExcelUploadSetting.StartRow = int.Parse(txtSESR.Text);
        obj.SalesExcelUploadSetting.PartCodeColumn = int.Parse(txtSEPCC.Text);
        obj.SalesExcelUploadSetting.QuantityColumn = int.Parse(txtSEQC.Text);
        obj.SalesExcelUploadSetting.PartTypeColumn = int.Parse(txtSEPTC.Text);

        obj.CycleCountExcelUploadSetting.StartRow = int.Parse(txtCCSR.Text);
        obj.CycleCountExcelUploadSetting.PartCodeColumn = int.Parse(txtCCPC.Text);
        obj.CycleCountExcelUploadSetting.QuantityColumn = int.Parse(txtCCQty.Text);
        obj.CycleCountExcelUploadSetting.CommentColumn = int.Parse(txtCCComment.Text);
        //obj.CycleCountExcelUploadSetting.SafetyStockColumn = int.Parse(txtCCST.Text);
        //obj.CycleCountExcelUploadSetting.PartTypeColumn = int.Parse(txtCCPT.Text);

        obj.SaleVehicleExcelUploadSetting.DateFormat = txtVDateFormat.Text;
        obj.SaleVehicleExcelUploadSetting.Address = int.Parse(txtVAddress.Text);
        obj.SaleVehicleExcelUploadSetting.BillNumber = int.Parse(txtVBillNo.Text);
        obj.SaleVehicleExcelUploadSetting.CommentSellItem = int.Parse(txtVCommentItem.Text);
        obj.SaleVehicleExcelUploadSetting.CustomerDescription = int.Parse(txtVCusDesc.Text);
        obj.SaleVehicleExcelUploadSetting.Id = int.Parse(txtVCusId.Text);
        obj.SaleVehicleExcelUploadSetting.CustomerName = int.Parse(txtVCusName.Text);
        obj.SaleVehicleExcelUploadSetting.District = int.Parse(txtVDistrict.Text);
        obj.SaleVehicleExcelUploadSetting.DOB = int.Parse(txtVDOB.Text);
        obj.SaleVehicleExcelUploadSetting.Email = int.Parse(txtVEmail.Text);
        obj.SaleVehicleExcelUploadSetting.EngineNumber = int.Parse(txtVEngineNo.Text);
        obj.SaleVehicleExcelUploadSetting.Gender = int.Parse(txtVGender.Text);
        obj.SaleVehicleExcelUploadSetting.JobType = int.Parse(txtVJobType.Text);
        obj.SaleVehicleExcelUploadSetting.Mobile = int.Parse(txtVMobile.Text);
        obj.SaleVehicleExcelUploadSetting.NumberPlate = int.Parse(txtVNumberPlate.Text);
        obj.SaleVehicleExcelUploadSetting.NumberPlateRecDate = int.Parse(txtVNumberPlateDate.Text);
        obj.SaleVehicleExcelUploadSetting.PaymentDate = int.Parse(txtVPaymentDate.Text);
        obj.SaleVehicleExcelUploadSetting.PaymentType = int.Parse(txtVPaymentType.Text);
        obj.SaleVehicleExcelUploadSetting.Precint = int.Parse(txtVPrecint.Text);
        obj.SaleVehicleExcelUploadSetting.Price = int.Parse(txtVPrice.Text);
        obj.SaleVehicleExcelUploadSetting.Priority = int.Parse(txtVPriority.Text);
        obj.SaleVehicleExcelUploadSetting.Province = int.Parse(txtVProvince.Text);
        obj.SaleVehicleExcelUploadSetting.SellDate = int.Parse(txtVSellDate.Text);
        obj.SaleVehicleExcelUploadSetting.SellType = int.Parse(txtVSellType.Text);
        obj.SaleVehicleExcelUploadSetting.StartRow = int.Parse(txtVStartRow.Text);
        obj.SaleVehicleExcelUploadSetting.Tel = int.Parse(txtVTel.Text);
        obj.SaleVehicleExcelUploadSetting.InstalmentTimes = int.Parse(txtVInstalmentTimes.Text);
        obj.SaleVehicleExcelUploadSetting.FirstInstalmentAmount = int.Parse(txtVFirstInstalmentAmount.Text);
        obj.SaleVehicleExcelUploadSetting.DaysEachInstalment = int.Parse(txtVDaysEachInstalment.Text);
        obj.SaleVehicleExcelUploadSetting.PayingDate1 = int.Parse(txtVPayingDate1.Text);
        obj.SaleVehicleExcelUploadSetting.PayingDate2 = int.Parse(txtVPayingDate2.Text);
        obj.SaleVehicleExcelUploadSetting.PayingDate3 = int.Parse(txtVPayingDate3.Text);
        obj.SaleVehicleExcelUploadSetting.PayingDate4 = int.Parse(txtVPayingDate4.Text);
        obj.SaleVehicleExcelUploadSetting.PayingDate5 = int.Parse(txtVPayingDate5.Text);
        obj.SaleVehicleExcelUploadSetting.Amount1 = int.Parse(txtVAmount1.Text);
        obj.SaleVehicleExcelUploadSetting.Amount2 = int.Parse(txtVAmount2.Text);
        obj.SaleVehicleExcelUploadSetting.Amount3 = int.Parse(txtVAmount3.Text);
        obj.SaleVehicleExcelUploadSetting.Amount4 = int.Parse(txtVAmount4.Text);
        obj.SaleVehicleExcelUploadSetting.Amount5 = int.Parse(txtVAmount5.Text);

        obj.BonusSetting.Amount = int.Parse(txtBAmount.Text);
        obj.BonusSetting.BonusDate = int.Parse(txtBDate.Text);
        obj.BonusSetting.DateFormat = txtBDateFormat.Text;
        obj.BonusSetting.Description = int.Parse(txtBDesc.Text);
        obj.BonusSetting.BonusPlan = int.Parse(txtBPlan.Text);
        obj.BonusSetting.BonusSource = int.Parse(txtBSource.Text);
        obj.BonusSetting.StartRow = int.Parse(txtBStartRow.Text);
        obj.BonusSetting.PlanMonth = int.Parse(txtBStatus.Text);
        obj.BonusSetting.DealerCode = int.Parse(txtBDealerCode.Text);

        obj.WarrantyPartSetting.DateFormat = txtWDateFormat.Text;
        obj.WarrantyPartSetting.MotorCode = int.Parse(txtWMotorCode.Text);
        obj.WarrantyPartSetting.PartCode = int.Parse(txtWPartCode.Text);
        obj.WarrantyPartSetting.EnglishName = int.Parse(txtWPartNameEN.Text);
        obj.WarrantyPartSetting.VietnameseName = int.Parse(txtWPartNameVN.Text);
        obj.WarrantyPartSetting.StartDate = int.Parse(txtWStartDate.Text);
        obj.WarrantyPartSetting.StartRow = int.Parse(txtWStartRow.Text);
        obj.WarrantyPartSetting.EndDate = int.Parse(txtWStopDate.Text);
        obj.WarrantyPartSetting.WarrantyLength = int.Parse(txtWWarrantyLength.Text);
        obj.WarrantyPartSetting.WarrantyTime = int.Parse(txtWWarrantyTime.Text);

        obj.PaymentSpan = int.Parse(tbPS.Text);
        obj.QuotationSpan = int.Parse(tbQS.Text);
        obj.AutoInstockSpan = int.Parse(tbAIS.Text);
        obj.ShippingSpan = int.Parse(tbSS.Text);
        obj.OverShippingEmail = tbEMail.Text;
        obj.MaxMonthAllowReopen = int.Parse(txtMaxMonthAllowReopen.Text);
        obj.MaxMonthAllowReopen1 = int.Parse(txtMaxMonthAllowReopen1.Text);

        obj.OrderDateControl = string.Empty;
        if (chkDNFOrderDateControl.Checked) obj.OrderDateControl += "DNF";
        if (chkHTFOrderDateControl.Checked) obj.OrderDateControl += "HTF";

        obj.AllowChangeOrderDate = chkAllowChangeOrderDate.Checked;

        obj.CheckOrderPartNotDuplicateBeforeConfirmWhenSend = chkCheckOrderPartNotDuplicateBeforeConfirmWhenSend.Checked;
        obj.ApplyCheckPartInSubOrder = chkApplySubOrder.Checked;

        obj.AllowAutoCloseInvI = chbAutoCloseInvI.Checked;
        obj.AutoCloseInvDayI = string.IsNullOrEmpty(txtAutoCloseInvDayI.Text.Trim()) ? 0 : int.Parse(txtAutoCloseInvDayI.Text);
        obj.AutoCloseInvTimeI = string.IsNullOrEmpty(txtAutoCloseInvTimeI.Text.Trim()) ? 0 : int.Parse(txtAutoCloseInvTimeI.Text);

        obj.DefaultLabourOnInsertPartI = string.IsNullOrEmpty(txtDefLabourI.Text.Trim()) ? 0 : int.Parse(txtDefLabourI.Text);
        obj.AllowAutoSyncPartI = chbAutoSyncPart.Checked;
        obj.AutoSyncPartDaysI = string.IsNullOrEmpty(txtSyncPdays.Text.Trim()) ? 0 : int.Parse(txtSyncPdays.Text);
        obj.AutoSyncPartHourI = string.IsNullOrEmpty(txtSyncPHour.Text.Trim()) ? 0 : int.Parse(txtSyncPHour.Text);
        obj.AutoSyncPartFromDateI = DataFormat.DateFromString(txtSyncPfromDate.Text);

        if (VDMSSetting.CurrentInstance.Save()) lblSaveOk.Visible = true;
        else lblSaveError.Visible = true;
    }

    protected void btnCloseInvINow_Click(object sender, EventArgs e)
    {
        VDMSHttpApplication.closeSchedulerI.ForceClose = true;
    }

    protected void btnAbortProcess_Click(object sender, EventArgs e)
    {
        VDMSHttpApplication.closeSchedulerI.Abort();
    }

    protected void btnAddNewParts_Click(object sender, EventArgs e)
    {
        VDMSHttpApplication.syncPartSchedulerI.ForceSyncNew = true;
    }
    protected void btnUpdatePrice_Click(object sender, EventArgs e)
    {
        VDMSHttpApplication.syncPartSchedulerI.ForceSyncPrice = true;
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        btnCloseInvINow.Visible = !(VDMSHttpApplication.closeSchedulerI.ForceClose || VDMSHttpApplication.closeSchedulerI.Closing);
        btnAbortProcess.Visible = (VDMSHttpApplication.closeSchedulerI.ForceClose || VDMSHttpApplication.closeSchedulerI.Closing);

        btnAddNewParts.Visible = !(VDMSHttpApplication.syncPartSchedulerI.ForceSyncNew || VDMSHttpApplication.syncPartSchedulerI.OnSyncNew);
        litAddingParts.Visible = (VDMSHttpApplication.syncPartSchedulerI.ForceSyncNew || VDMSHttpApplication.syncPartSchedulerI.OnSyncNew);
        btnUpdatePrice.Visible = !(VDMSHttpApplication.syncPartSchedulerI.ForceSyncPrice || VDMSHttpApplication.syncPartSchedulerI.OnSyncPrice);
        litUpdatingPrice.Visible = (VDMSHttpApplication.syncPartSchedulerI.ForceSyncPrice || VDMSHttpApplication.syncPartSchedulerI.OnSyncPrice);
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {

    }

    //update: add new bank info
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        string df = ((TextBox)gvBanks.FootRow.FindControl("txtNewDateFormat")).Text.Trim();
        string bc = ((TextBox)gvBanks.FootRow.FindControl("txtNewBankCode")).Text.Trim().ToUpper();
        string bn = ((TextBox)gvBanks.FootRow.FindControl("txtNewBankName")).Text.Trim();
        string o = ((TextBox)gvBanks.FootRow.FindControl("txtNewOrderNumber")).Text;
        string a = ((TextBox)gvBanks.FootRow.FindControl("txtNewAmount")).Text;
        string p = ((TextBox)gvBanks.FootRow.FindControl("txtNewPaymentDate")).Text;
        string c = ((TextBox)gvBanks.FootRow.FindControl("txtNewComment")).Text;
        string t = ((TextBox)gvBanks.FootRow.FindControl("txtNewTransNumber")).Text;
        string d = ((TextBox)gvBanks.FootRow.FindControl("txtNewDealerCode")).Text;
        string n = ((TextBox)gvBanks.FootRow.FindControl("txtNewDealerName")).Text;
        string s = ((TextBox)gvBanks.FootRow.FindControl("txtNewStartRow")).Text;

        int sr = string.IsNullOrEmpty(s) ? 0 : int.Parse(s);
        int on = string.IsNullOrEmpty(o) ? 0 : int.Parse(o);
        int am = string.IsNullOrEmpty(a) ? 0 : int.Parse(a);
        int pd = string.IsNullOrEmpty(p) ? 0 : int.Parse(p);
        int cm = string.IsNullOrEmpty(c) ? 0 : int.Parse(c);
        int tn = string.IsNullOrEmpty(t) ? 0 : int.Parse(t);
        int dc = string.IsNullOrEmpty(d) ? 0 : int.Parse(d);
        int dn = string.IsNullOrEmpty(n) ? 0 : int.Parse(n);

        VDMSSetting.SettingData.BankPaymentExcelSetting b = null; // VDMSSetting.CurrentSetting.BankPaymentSettings.SingleOrDefault(bk => bk.BankCode == bank);
        if (b == null)
        {
            b = new VDMSSetting.SettingData.BankPaymentExcelSetting();
            VDMSSetting.CurrentSetting.BankPaymentSettings.Add(b);
        }

        b.DateFormat = df;
        b.BankCode = bc;
        b.BankName = bn;
        b.Amount = am;
        b.Comment = cm;
        b.OrderId = on;
        b.PaymentDate = pd;
        b.TransactionNumber = tn;
        b.DealerName = dn;
        b.DealerCode = dc;
        b.StartRow = sr;

        BindBanks();
    }
    protected void imbDelBank_Click(object sender, ImageClickEventArgs e)
    {
        Guid bankId = new Guid(((ImageButton)sender).CommandArgument);
        VDMSSetting.CurrentSetting.BankPaymentSettings.RemoveAll(b => b.BankId == bankId);

        BindBanks();
    }
    protected void UpdateBank(object sender, EventArgs e)
    {
        if (bindingBanks) return;

        var tb = (TextBox)sender;
        var cn = tb.NamingContainer;

        Guid bankId = new Guid(tb.ToolTip);
        var b = VDMSSetting.CurrentSetting.BankPaymentSettings.SingleOrDefault(bk => bk.BankId == bankId);

        if (b != null)
        {
            string df = ((TextBox)cn.FindControl("txtDateFormat")).Text.Trim();
            string bc = ((TextBox)cn.FindControl("txtBankCode")).Text.Trim().ToUpper();
            string bn = ((TextBox)cn.FindControl("txtBankName")).Text.Trim();
            string o = ((TextBox)cn.FindControl("txtOrderNumber")).Text;
            string a = ((TextBox)cn.FindControl("txtAmount")).Text;
            string p = ((TextBox)cn.FindControl("txtPaymentDate")).Text;
            string c = ((TextBox)cn.FindControl("txtComment")).Text;
            string t = ((TextBox)cn.FindControl("txtTransNumber")).Text;
            string d = ((TextBox)cn.FindControl("txtDealerCode")).Text;
            string n = ((TextBox)cn.FindControl("txtDealerName")).Text;
            string s = ((TextBox)cn.FindControl("txtStartRow")).Text;

            int on = string.IsNullOrEmpty(o) ? 0 : int.Parse(o);
            int am = string.IsNullOrEmpty(a) ? 0 : int.Parse(a);
            int pd = string.IsNullOrEmpty(p) ? 0 : int.Parse(p);
            int cm = string.IsNullOrEmpty(c) ? 0 : int.Parse(c);
            int tn = string.IsNullOrEmpty(t) ? 0 : int.Parse(t);
            int dc = string.IsNullOrEmpty(d) ? 0 : int.Parse(d);
            int dn = string.IsNullOrEmpty(n) ? 0 : int.Parse(n);
            int sr = string.IsNullOrEmpty(t) ? 0 : int.Parse(s);

            b.DateFormat = df;
            b.BankCode = bc;
            b.BankName = bn;
            b.Amount = am;
            b.Comment = cm;
            b.OrderId = on;
            b.PaymentDate = pd;
            b.TransactionNumber = tn;
            b.DealerName = dn;
            b.DealerCode = dc;
            b.StartRow = sr;
        }
    }
}
