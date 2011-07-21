using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.II.Linq;
using VDMS.I.Linq;
using VDMS.I.Service;
using VDMS.Helper;

namespace VDMS.I.Entity
{
    public partial class ExchangePartDetail
    {
        public double ManPowerM { get { return (this.LabourM == 0) ? 0 : (double)((double)this.TotalFeeM / (double)this.LabourM); } }
        public double ManPowerO { get { return (this.LabourO == 0) ? 0 : (double)((double)this.TotalFeeO / (double)this.LabourO); } }
        public long LabourM { get; set; }
        public long LabourO { get; set; }
        public decimal WarrantySpareAmountO { get { return this.PartQtyO * this.UnitPriceO; } }
        public decimal WarrantySpareAmountM { get { return this.PartQtyM * this.UnitPriceM; } }

        public decimal TotalO
        {
            get { return WarrantySpareAmountO + TotalFeeO; }
        }
        public decimal TotalM
        {
            get { return WarrantySpareAmountM + TotalFeeM; }
        }

        public bool LoadWarrCond(ServiceDataContext dc)
        {
            //var dc = DCFactory.GetDataContext<ServiceDataContext>();
            var wc = dc.WarrantyConditions.FirstOrDefault(p => p.PartCode == this.PartCodeM);
            if (wc == null) return false;
            if (wc.Labour != null) this.LabourM = (long)wc.Labour;
            wc = dc.WarrantyConditions.FirstOrDefault(p => p.PartCode == this.PartCodeO);
            if (wc == null) return false;
            if (wc.Labour != null) this.LabourO = (long)wc.Labour;

            return true;
        }
    }
    public partial class ExchangePartHeader
    {
        public string CustomerName { get { return (this.CustomerId > 0) ? this.Customer.FullName : ""; } }
        public string ServiceSheetNumber { get { return (this.ServiceHeaderId > 0) ? this.ServiceHeader.ServiceSheetNumber : ""; } }

        public int TotalQuantityO { get; set; }
        public decimal TotalPartCostO { get; set; }
        public decimal TotalFeeO { get; set; }
        public decimal TotalAmountO { get { return this.TotalFeeO + this.TotalPartCostO; } }

        public decimal TotalFeeM { get; set; }
        public decimal TotalPartCostM { get; set; }
        public int TotalQuantityM { get; set; }
        public decimal TotalAmountM { get { return this.TotalFeeM + this.TotalPartCostM; } }

        public void DoSummary()
        {
            if (this.Status == (int)ExchangeVoucherStatus.Approved)
            {
                //this.TotalAmountM = this.ExchangePartDetails.Sum(p => p.TotalM);
                this.TotalFeeM = this.ExchangePartDetails.Sum(p => p.TotalFeeM);
                this.TotalPartCostM = this.ExchangePartDetails.Sum(p => p.WarrantySpareAmountM);
                this.TotalQuantityM = this.ExchangePartDetails.Sum(p => p.PartQtyM);
            }
            this.TotalPartCostO = this.ExchangePartDetails.Sum(p => p.WarrantySpareAmountO);
            this.TotalFeeO = this.ExchangePartDetails.Sum(p => p.TotalFeeO);
            //this.TotalAmountO = this.ExchangePartDetails.Sum(p => p.TotalO);
            this.TotalQuantityO = this.ExchangePartDetails.Sum(p => p.PartQtyO);
        }
    }
    public partial class ItemInstanceEx : ItemInstance
    {
        public bool HasVoucher { get; set; }
        public int VoucherStatus { get; set; }
        public static bool CheckVoucherStatus(long iteminstanceid, string dealercode, string brancecode, string enginenumber, out int voucherstatus)
        {   
            var dc = VDMS.II.Linq.DCFactory.GetDataContext<VDMS.I.Linq.VehicleDataContext>();
            var m = dc.ShippingDetails.Where(s => s.EngineNumber == enginenumber);
            if(m.Count() > 0)
            {
                var r = m.FirstOrDefault();
                if (r.ShippingHeader.DealerCode != dealercode)
                {
                    voucherstatus = 0;
                    return true;
                }
            }
            voucherstatus = 1;
            return false;
        }
        public ItemInstanceEx(ItemInstance i)
        {
            this.BranchCode = i.BranchCode;
            this.Color = i.Color;
            this.Comments = i.Comments;
            this.CreatedDate = i.CreatedDate;
            this.DatabaseCode = i.DatabaseCode;
            this.DealerCode = i.DealerCode;
            this.EngineNumber = i.EngineNumber;
            this.ImportedDate = i.ImportedDate;
            //this.Item = i.Item;
            this.ItemCode = i.ItemCode;
            this.ItemInstanceId = i.ItemInstanceId;
            this.ItemType = i.ItemType;
            this.MadeDate = i.MadeDate;
            this.ReleasedDate = i.ReleasedDate;
            this.Status = i.Status;
            this.VMEPInvoice = i.VMEPInvoice;

            //var sd = i.ShippingDetails.Where(d => d.ShippingHeader.DealerCode == UserHelper.DealerCode).OrderByDescending(s => s.ShippingDetailId).FirstOrDefault();
            int voucherstatus = 1;
            var sd = CheckVoucherStatus(i.ItemInstanceId, i.DealerCode, i.BranchCode, i.EngineNumber, out voucherstatus);
            this.HasVoucher = sd ? false : 1 == voucherstatus;
            this.VoucherStatus = sd ? 0 : voucherstatus;
            //var sd = s.OrderByDescending(v => v.ShippingDetailId).FirstOrDefault();
            //this.HasVoucher = sd == null ? false : sd.VoucherStatus == 1;
            //this.VoucherStatus = sd == null ? 0 : sd.VoucherStatus;
        }
    }
    public class CustomerInfo : Customer
    {
        public string CusClass { get; set; }
        public string NumberPlate { get; set; }
        public string EngineNumber { get; set; }
        public string ItemType { get; set; }
        public string ItemColor { get; set; }
        public DateTime BuyDate { get; set; }
        public string Phone { get { return string.IsNullOrEmpty(this.Mobile) ? this.Tel : this.Mobile; } }
        public string AllPhone { get { return string.IsNullOrEmpty(this.Mobile) ? this.Tel : (string.IsNullOrEmpty(this.Tel) ? this.Mobile : string.Format("{0} | {1}", this.Tel, this.Mobile)); } }

        public CustomerInfo(Invoice inv)
        {
            if (inv.Customer != null) GetCustomerInfo(inv.Customer);
            if (inv.DataIteminstance != null)
            {
                this.ItemColor = inv.DataIteminstance.Color;
                this.ItemType = inv.DataIteminstance.ItemType;
            }
            if (inv.SaleSellitem != null) this.NumberPlate = inv.SaleSellitem.NumberPlate;

            this.CusClass = "SL";
            this.EngineNumber = inv.EngineNumber;
            this.BuyDate = inv.SellDate;
        }
        public CustomerInfo(ServiceHeader src)
        {
            if (src.Customer != null) GetCustomerInfo(src.Customer);

            this.CusClass = "SV";
            this.NumberPlate = src.NumberPlate;
            this.ItemType = src.ItemType;
            this.EngineNumber = src.EngineNumber;
            this.BuyDate = src.PurchaseDate;
            this.ItemColor = src.ColorCode;
        }
        public CustomerInfo(WarrantyInfo src, string color)
        {
            if (src.Customer != null) GetCustomerInfo(src.Customer);

            this.CusClass = "WI";
            this.EngineNumber = src.EngineNumber;
            this.BuyDate = src.PurchaseDate;
            this.ItemColor = (string.IsNullOrEmpty(color)) ? src.Color : color;
        }
        public CustomerInfo(Customer cus)
        {
            GetCustomerInfo(cus);
        }

        public void GetCustomerInfo(Customer cus)
        {
            if (cus != null)
            {
                this.Address = cus.Address;
                this.BirthDate = cus.BirthDate;
                this.CustomerDescription = cus.CustomerDescription;
                this.CustomerId = cus.CustomerId;
                this.CustomerType = cus.CustomerType;
                this.DealerCode = cus.DealerCode;
                this.DistrictId = cus.DistrictId;
                this.Email = cus.Email;
                this.ForService = cus.ForService;
                this.FullName = cus.FullName;
                this.Gender = cus.Gender;
                this.IdentifyNumber = cus.IdentifyNumber;
                this.JobTypeId = cus.JobTypeId;
                this.Mobile = cus.Mobile;
                this.Precinct = cus.Precinct;
                this.Priority = cus.Priority;
                this.ProvinceId = cus.ProvinceId;
                this.Tel = cus.Tel;
            }
        }
    }
}