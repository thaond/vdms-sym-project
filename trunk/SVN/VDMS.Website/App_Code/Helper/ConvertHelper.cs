using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.Core.Domain;
using VDMS.I.Entity;
using Customer = VDMS.Core.Domain.Customer;

/// <summary>
/// Summary description for ConvertHelper
/// </summary>
public static class ConvertHelper
{
    public static Customer ConvertCustomer(this VDMS.I.Entity.Customer cus)
    {
        return new Customer
                   {
                       Address = cus.Address,
                       Birthdate = cus.BirthDate ?? DateTime.Parse("1900/01/01"),
                       Customerdescription = cus.CustomerDescription,
                       Customertype = cus.CustomerType ?? 0,
                       Dealercode = cus.DealerCode,
                       Districtid = cus.DistrictId,
                       Email = cus.Email,
                       Forservice = cus.ForService,
                       Fullname = cus.FullName,
                       Gender = cus.Gender ?? false,
                       Provinceid = cus.ProvinceId,
                       Identifynumber = cus.IdentifyNumber,
                       Jobtypeid = cus.JobTypeId ?? 0,
                       Mobile = cus.Mobile,
                       Precinct = cus.Precinct,
                       Id = cus.CustomerId,
                       Priority = cus.Priority ?? 0,
                       Tel = cus.Tel
                   };
    }
    public static Serviceheader ConvertServiceHeader(this ServiceHeader sh)
    {
        return new Serviceheader(sh.EngineNumber, sh.ServiceDate, sh.ServiceType??0, sh.Damaged, sh.RepairResult,
                                 (long)sh.KmCount, sh.Comments, sh.ServiceSheetNumber, (long)sh.FeeAmount, (long)sh.TotalAmount,
                                 sh.NumberPlate, sh.FrameNumber, sh.PurchaseDate, sh.ItemType, sh.ColorCode,
                                 sh.DealerCode, sh.BranchCode, sh.CreateBy, sh.Customer.ConvertCustomer(), (int)sh.Status);
    }
    public static Warrantyinfo ConvertWarrantyInfo( this WarrantyInfo wi)
    {
        return new Warrantyinfo(wi.PurchaseDate, wi.KmCount, wi.ItemCode, wi.Color, wi.SellDealerCode, wi.DatabaseCode,
                                (DateTime)wi.CreatedDate, wi.Customer.ConvertCustomer(), "", "");
    }
}
