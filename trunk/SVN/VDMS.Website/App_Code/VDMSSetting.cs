using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml.Serialization;
using System;

namespace VDMS
{
    [XmlRoot("VDMSSetting", Namespace = "", IsNullable = true)]
    public class VDMSSetting
    {
        public class SettingData
        {
            public class ExcelSetting
            {
                public int StartRow { get; set; }
                public int PartCodeColumn { get; set; }
                public int QuantityColumn { get; set; }
                public int ModelColumn { get; set; }
                public int PartTypeColumn { get; set; }
                public int SafetyStockColumn { get; set; }
                public int CommentColumn { get; set; }
                public int OldPartCodeColumn { get; set; }
                public int NewPartCodeColumn { get; set; }
                public int StatusColumn { get; set; }
                public int PackingColumn { get; set; }
                public int UnitColumn { get; set; }
            }

            public class BankPaymentExcelSetting
            {
                // excel setting
                public int StartRow { get; set; }
                public int PaymentDate { get; set; }
                public int OrderId { get; set; }
                public int Comment { get; set; }
                public int TransactionNumber { get; set; }
                public int Amount { get; set; }
                public int DealerCode { get; set; }
                public int DealerName { get; set; }
                public string DateFormat { get; set; }

                // detail data
                public Guid BankId { get; set; }
                public string BankCode { get; set; }
                public string BankName { get; set; }

                public BankPaymentExcelSetting()
                {
                    BankId = Guid.NewGuid();
                }
            }

            public class VehicleSaleExcelSetting
            {
                public int StartRow { get; set; }
                public string DateFormat { get; set; }
                // Invoice
                public int EngineNumber { get; set; }
                public int BillNumber { get; set; }
                public int SellDate { get; set; }
                // SellItem
                public int Price { get; set; }
                public int PaymentType { get; set; }
                public int NumberPlate { get; set; }
                public int SellType { get; set; }
                public int PaymentDate { get; set; }
                public int NumberPlateRecDate { get; set; }
                public int CommentSellItem { get; set; }
                // Payment - fixed instalments
                public int InstalmentTimes { get; set; }
                public int FirstInstalmentAmount { get; set; }
                public int DaysEachInstalment { get; set; }
                // Payment - unfixed instalments
                public int PayingDate1 { get; set; }
                public int Amount1 { get; set; }
                public int PayingDate2 { get; set; }
                public int Amount2 { get; set; }
                public int PayingDate3 { get; set; }
                public int Amount3 { get; set; }
                public int PayingDate4 { get; set; }
                public int Amount4 { get; set; }
                public int PayingDate5 { get; set; }
                public int Amount5 { get; set; }
                // Customer
                public int Id { get; set; }
                public int CustomerName { get; set; }
                public int Gender { get; set; }
                public int DOB { get; set; }
                public int Tel { get; set; }
                public int Mobile { get; set; }
                public int Address { get; set; }
                public int Province { get; set; }
                public int District { get; set; }
                public int JobType { get; set; }
                public int Email { get; set; }
                public int Precint { get; set; }
                public int CustomerDescription { get; set; }
                public int Priority { get; set; }
            }

            public class BonusExcelSetting
            {
                public int StartRow { get; set; }
                public string DateFormat { get; set; }
                public int BonusPlan { get; set; }
                public int DealerCode { get; set; }
                public int BonusDate { get; set; }
                public int BonusSource { get; set; }
                public int Amount { get; set; }
                public int PlanMonth { get; set; }
                public int Description{ get; set; }
            }

            public class WarrantySetting
            {
                public int StartRow { get; set; }
                public string DateFormat { get; set; }
                public int PartCode { get; set; }
                public int VietnameseName { get; set; }
                public int EnglishName { get; set; }
                public int MotorCode { get; set; }
                public int WarrantyTime { get; set; }
                public int WarrantyLength { get; set; }
                public int StartDate { get; set; }
                public int EndDate { get; set; }
            }

            public List<BankPaymentExcelSetting> BankPaymentSettings { get; set; }

            public bool AllowUndoVehiclePaymentConfirm { get; set; }
            public bool AllowServiceAddNewWarrantyInfoForAllRegion { get; set; }
            public bool CheckEngineNoForService { get; set; }
            public bool CheckEngineNoForPartsChange { get; set; }
            public bool CheckWarrantyCondition { get; set; }
            public bool CheckWarrantyInfoDatabase { get; set; }
            public string OrderDateControl { get; set; }
            public ExcelSetting OrderExcelUploadSetting { get; set; }
            public ExcelSetting SalesExcelUploadSetting { get; set; }
            public ExcelSetting CycleCountExcelUploadSetting { get; set; }
            public ExcelSetting PartReplaceExcelUploadSetting { get; set; }
            public ExcelSetting PartSpecExcelUploadSetting { get; set; }
            public VehicleSaleExcelSetting SaleVehicleExcelUploadSetting { get; set; }
            public BonusExcelSetting BonusSetting { get; set; }
            public WarrantySetting WarrantyPartSetting { get; set; }
            public int PaymentSpan { get; set; }
            public int QuotationSpan { get; set; }
            public int ShippingSpan { get; set; }
            public int AutoInstockSpan { get; set; }
            public int MaxMonthAllowReopen1 { get; set; }
            public int MaxMonthAllowReopen { get; set; }
            public string OverShippingEmail { get; set; }
            public bool AllowChangeOrderDate { get; set; }

            public bool AllowAutoCloseInvI { get; set; }
            public int AutoCloseInvDayI { get; set; }
            public int AutoCloseInvTimeI { get; set; }

            public bool AllowAutoSyncPartI { get; set; }
            public int AutoSyncPartDaysI { get; set; }
            public int AutoSyncPartHourI { get; set; }
            public int DefaultLabourOnInsertPartI { get; set; }
            public DateTime AutoSyncPartFromDateI { get; set; }

            public bool NotAllowTransactionsBeforeCC { get; set; }
            public bool CheckOrderPartNotDuplicateBeforeConfirmWhenSend { get; set; }
            public bool ApplyCheckPartInSubOrder { get; set; }

            public SettingData()
            {
                PartReplaceExcelUploadSetting = new ExcelSetting();
                PartSpecExcelUploadSetting = new ExcelSetting();
                OrderExcelUploadSetting = new ExcelSetting();
                SalesExcelUploadSetting = new ExcelSetting();
                CycleCountExcelUploadSetting = new ExcelSetting();
                SaleVehicleExcelUploadSetting = new VehicleSaleExcelSetting();
                BonusSetting = new BonusExcelSetting();
                WarrantyPartSetting = new WarrantySetting();
                OrderDateControl = string.Empty;
                BankPaymentSettings = new List<BankPaymentExcelSetting>();
            }
        }

        private static VDMSSetting instance = new VDMSSetting();
        private SettingData data;

        private VDMSSetting()
        {
            string path = Path.Combine(HttpRuntime.AppDomainAppPath, "VDMS.II.Config.xml");
            if (File.Exists(path))
            {
                using (Stream s = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    try
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(SettingData));
                        data = (SettingData)xmlSerializer.Deserialize(s);
                    }
                    catch
                    {
                        data = new SettingData();
                    }
                }
            }
            else data = new SettingData();
        }

        public static SettingData CurrentSetting
        {
            get { return instance.data; }
        }

        public static VDMSSetting CurrentInstance
        {
            get { return instance; }
        }

        public bool Save()
        {
            string path = Path.Combine(HttpRuntime.AppDomainAppPath, "VDMS.II.Config.xml");
            try
            {
                using (Stream s = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(SettingData));
                    xmlSerializer.Serialize(s, this.data);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
