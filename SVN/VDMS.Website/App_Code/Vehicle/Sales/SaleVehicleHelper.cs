using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using VDMS.Helper;
using VDMS.Common.Utils;
using VDMS.II.BasicData;
using VDMS.I.Entity;
using VDMS.II.Linq;
using VDMS.I.Linq;
using VDMS.II.Common.ExcelReader;
using Resources;
using Excel;

namespace VDMS.I.Vehicle
{
    public class SaleVehicleInfo : VehicleInfo
    {
        public bool IsValid { get; set; }
        public string Error { get; set; }
        // Invoice
        public string BillNumber { get; set; }
        public DateTime SellDate { get; set; }
        // SellItem
        public int Price { get; set; }
        public int PaymentType { get; set; }
        public string NumberPlate { get; set; }
        public string SellType { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime NumberPlateRecDate { get; set; }
        public string CommentSellItem { get; set; }
        // Payment - fixed instalments
        public int InstalmentTimes { get; set; }
        public int FirstInstalmentAmount { get; set; }
        public int DaysEachInstalment { get; set; }
        public List<DateTime> FHPPayingDate { get; set; }
        public List<int> FHPAmount { get; set; }
        // Payment - unfixed instalments
        public List<DateTime> UHPPayingDate { get; set; }
        public List<int> UHPAmount { get; set; }
        // Customer
        public long CustomerId { get; set; }
        public string IdentifyNumber { get; set; }
        public string CustomerName { get; set; }
        public int Gender { get; set; }
        public DateTime DOB { get; set; }
        public string Tel { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public int JobType { get; set; }
        public string Email { get; set; }
        public string Precint { get; set; }
        public string CustomerDescription { get; set; }
        public int Priority { get; set; }
        public int CustomerType { get; set; }
        // Adjust
        public string FromBranch { get; set; }
        public string ToBranch { get; set; }
        public DateTime AdjustDate { get; set; }
    }

    public enum SaveSaleAction
    {
        ImportVehicles = 0,
        LotSale = 1,
    }

    public class SaleVehicleHelper : SessionVehicleDAO<SaleVehicleInfo>
    {
        public static SaleVehicleInfo CommonSaleData  // Common data used for selling vehicles
        {
            get
            {
                var commonSaleData = HttpContext.Current.Session[key + "_common"] as SaleVehicleInfo;
                if (commonSaleData == null)
                {
                    commonSaleData = new SaleVehicleInfo();
                    HttpContext.Current.Session[key + "_common"] = commonSaleData;
                }
                return commonSaleData;
            }
        }

        public static void SaveCommonSaleInfo()
        {
            HttpContext.Current.Session[key + "_common"] = CommonSaleData;
        }

        public static void ClearCommonSaleInfo()
        {
            HttpContext.Current.Session.Remove(key + "_common");
        }

        public static void InitSale(string keyy)
        {
            Init(keyy);
        }

        public static void ImportExcelData(IExcelDataReader spreadsheet, VDMS.VDMSSetting.SettingData.VehicleSaleExcelSetting setting)
        {


            var startRow = setting.StartRow;
            var addressColumn = setting.Address;
            var billNoColumn = setting.BillNumber;
            var commentColumn = setting.CommentSellItem;
            var cusDescColumn = setting.CustomerDescription;
            var cusNameColumn = setting.CustomerName;
            var districtColumn = setting.District;
            var dobColumn = setting.DOB;
            var emailColumn = setting.Email;
            var engineNoColumn = setting.EngineNumber;
            var genderColumn = setting.Gender;
            var idColumn = setting.Id;
            var jobTypeColumn = setting.JobType;
            var mobileColumn = setting.Mobile;
            var numberPlateColumn = setting.NumberPlate;
            var numberPlateRecDateColumn = setting.NumberPlateRecDate;
            var paymentDateColumn = setting.PaymentDate;
            var paymentTypeColumn = setting.PaymentType;
            var precintColumn = setting.Precint;
            var priceColumn = setting.Price;
            var priorityColumn = setting.Priority;
            var provinceColumn = setting.Province;
            var sellDateColumn = setting.SellDate;
            var sellTypeColumn = setting.SellType;
            var telColumn = setting.Tel;
            var instalmentTimesColumn = setting.InstalmentTimes;
            var firstInstalmentAmountColumn = setting.FirstInstalmentAmount;
            var daysEachInstalmentColumn = setting.DaysEachInstalment;
            var payingDate1Column = setting.PayingDate1;
            var payingDate2Column = setting.PayingDate2;
            var payingDate3Column = setting.PayingDate3;
            var payingDate4Column = setting.PayingDate4;
            var payingDate5Column = setting.PayingDate5;
            var amount1Column = setting.Amount1;
            var amount2Column = setting.Amount2;
            var amount3Column = setting.Amount3;
            var amount4Column = setting.Amount4;
            var amount5Column = setting.Amount5;

            var rows = spreadsheet.AsDataSet().Tables[0].AsEnumerable();

            var query = from r in rows
                        select new
                        {
                            Address = addressColumn == 0 || addressColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(addressColumn - 1),
                            BillNumber = billNoColumn == 0 || billNoColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(billNoColumn - 1), 
                            Comment = commentColumn == 0 || commentColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(commentColumn - 1),
                            CustomerDescription = cusDescColumn == 0 || cusDescColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(cusDescColumn - 1),
                            District = districtColumn == 0 || districtColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(districtColumn - 1),
                            DOB = dobColumn == 0 || dobColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(dobColumn - 1),
                            Email = emailColumn == 0 || emailColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(emailColumn - 1),
                            CustomerName = cusNameColumn == 0 || cusNameColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(cusNameColumn - 1),
                            EngineNumber = engineNoColumn == 0 || engineNoColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(engineNoColumn - 1),
                            Gender = genderColumn == 0 || genderColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(genderColumn - 1),
                            Id = idColumn == 0 || idColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(idColumn - 1),
                            JobType = jobTypeColumn == 0 || jobTypeColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(jobTypeColumn - 1),
                            Mobile = mobileColumn == 0 || mobileColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(mobileColumn - 1),
                            NumberPlate = numberPlateColumn == 0 || numberPlateColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(numberPlateColumn - 1),
                            NumberPlateRecDate = numberPlateRecDateColumn == 0 || numberPlateRecDateColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(numberPlateRecDateColumn - 1),
                            PaymentDate = paymentDateColumn == 0 || paymentDateColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(paymentDateColumn - 1),
                            PaymentType = paymentTypeColumn == 0 || paymentTypeColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(paymentTypeColumn - 1),
                            Precint = precintColumn == 0 || precintColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(precintColumn - 1),
                            Price = priceColumn == 0 || priceColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(priceColumn - 1),
                            Priority = priorityColumn == 0 || priorityColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(priorityColumn - 1),
                            Province = provinceColumn == 0 || provinceColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(provinceColumn - 1),
                            SellDate = sellDateColumn == 0 || sellDateColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(sellDateColumn - 1),
                            SellType = sellTypeColumn == 0 || sellTypeColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(sellTypeColumn - 1),
                            Tel = telColumn == 0 || telColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(telColumn - 1),
                            InstalmentTimes = instalmentTimesColumn == 0 || instalmentTimesColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(instalmentTimesColumn - 1),
                            FirstInstalmentAmount = firstInstalmentAmountColumn == 0 || firstInstalmentAmountColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(firstInstalmentAmountColumn - 1),
                            DaysEachInstalment = daysEachInstalmentColumn == 0 || daysEachInstalmentColumn > r.ItemArray.Count() ? string.Empty : r.Field<string>(daysEachInstalmentColumn - 1),
                            PayingDate1 = payingDate1Column == 0 || payingDate1Column > r.ItemArray.Count() ? string.Empty : r.Field<string>(payingDate1Column - 1),
                            PayingDate2 = payingDate2Column == 0 || payingDate2Column > r.ItemArray.Count() ? string.Empty : r.Field<string>(payingDate2Column - 1),
                            PayingDate3 = payingDate3Column == 0 || payingDate3Column > r.ItemArray.Count() ? string.Empty : r.Field<string>(payingDate3Column - 1),
                            PayingDate4 = payingDate4Column == 0 || payingDate4Column > r.ItemArray.Count() ? string.Empty : r.Field<string>(payingDate4Column - 1),
                            PayingDate5 = payingDate5Column == 0 || payingDate5Column > r.ItemArray.Count() ? string.Empty : r.Field<string>(payingDate5Column - 1),
                            Amount1 = amount1Column == 0 || amount1Column > r.ItemArray.Count() ? string.Empty : r.Field<string>(amount1Column - 1),
                            Amount2 = amount2Column == 0 || amount2Column > r.ItemArray.Count() ? string.Empty : r.Field<string>(amount2Column - 1),
                            Amount3 = amount3Column == 0 || amount3Column > r.ItemArray.Count() ? string.Empty : r.Field<string>(amount3Column - 1),
                            Amount4 = amount4Column == 0 || amount4Column > r.ItemArray.Count() ? string.Empty : r.Field<string>(amount4Column - 1),
                            Amount5 = amount5Column == 0 || amount5Column > r.ItemArray.Count() ? string.Empty : r.Field<string>(amount5Column - 1),
                        };
            var data = query.Skip(startRow - 1).TakeWhile(v => !string.IsNullOrEmpty(v.EngineNumber) ||
                                                               !string.IsNullOrEmpty(v.CustomerName));

            foreach (var item in data)
            {
                var v = new SaleVehicleInfo();
                int gender, price, priority,
                    instalmentTimes, firstInstalmentAmount, daysEachInstalment,
                    amount1, amount2, amount3, amount4, amount5;


                v.UHPPayingDate = new List<DateTime>();
                v.UHPAmount = new List<int>();
                v.FHPAmount = new List<int>();
                v.FHPPayingDate = new List<DateTime>();

                v.ImportDate = GetImportDateVehice(item.EngineNumber.Trim());

                v.Address = item.Address;
                v.BillNumber = string.IsNullOrEmpty(item.BillNumber) ? string.Empty : item.BillNumber.Trim().ToUpper();
                v.CommentSellItem = item.Comment;
                v.CustomerDescription = item.CustomerDescription;
                v.CustomerName = item.CustomerName;
                v.District = item.District.Trim();
                v.DOB = DataFormat.DateFromExcel(item.DOB, setting.DateFormat);
                v.Email = item.Email;
                v.EngineNumber = item.EngineNumber.Trim().ToUpper();
                v.Gender = int.TryParse(item.Gender, out gender) ? gender : 0;
                v.IdentifyNumber = item.Id;
                v.JobType = VehicleHelper.EvalJobType(item.JobType);
                v.Mobile = item.Mobile;
                v.NumberPlate = item.NumberPlate;
                v.NumberPlateRecDate = DataFormat.DateFromExcel(item.NumberPlateRecDate, setting.DateFormat);
                v.PaymentDate = DataFormat.DateFromExcel(item.PaymentDate, setting.DateFormat);
                v.PaymentType = VehicleHelper.EvalPaymentMethod(item.PaymentType);
                v.Precint = item.Precint;
                v.Price = int.TryParse(item.Price, out price) ? price : 0;
                v.Priority = int.TryParse(item.Priority, out priority) ? priority : 0;
                v.Province = VehicleHelper.EvalProvince(item.Province);
                v.SellDate = DataFormat.DateFromExcel(item.SellDate, setting.DateFormat);
                v.SellType = item.SellType;
                v.Tel = item.Tel;
                v.InstalmentTimes = int.TryParse(item.InstalmentTimes, out instalmentTimes) ? instalmentTimes : 0;
                v.FirstInstalmentAmount = int.TryParse(item.FirstInstalmentAmount, out firstInstalmentAmount) ? firstInstalmentAmount : 0;
                v.DaysEachInstalment = int.TryParse(item.DaysEachInstalment, out daysEachInstalment) ? daysEachInstalment : 0;
                v.UHPPayingDate.Add(DataFormat.DateFromExcel(item.PayingDate1, setting.DateFormat));
                v.UHPPayingDate.Add(DataFormat.DateFromExcel(item.PayingDate2, setting.DateFormat));
                v.UHPPayingDate.Add(DataFormat.DateFromExcel(item.PayingDate3, setting.DateFormat));
                v.UHPPayingDate.Add(DataFormat.DateFromExcel(item.PayingDate4, setting.DateFormat));
                v.UHPPayingDate.Add(DataFormat.DateFromExcel(item.PayingDate5, setting.DateFormat));
                v.UHPAmount.Add(int.TryParse(item.Amount1, out amount1) ? amount1 : 0);
                v.UHPAmount.Add(int.TryParse(item.Amount2, out amount2) ? amount2 : 0);
                v.UHPAmount.Add(int.TryParse(item.Amount3, out amount3) ? amount3 : 0);
                v.UHPAmount.Add(int.TryParse(item.Amount4, out amount4) ? amount4 : 0);
                v.UHPAmount.Add(int.TryParse(item.Amount5, out amount5) ? amount5 : 0);

                AddVehicle(v);
            }

            SaveSession();
            spreadsheet.Close();
        }


        /// <summary>
        /// mvbinh
        /// Check File Excel
        /// </summary>
        /// <param name="excel"></param>
        /// <returns></returns>
        public static bool ValidExcelData(Stream excel, out IExcelDataReader exceldatareader)
        {
            IExcelDataReader spreadsheet = ExcelReaderFactory.CreateBinaryReader(excel);

            var rows = spreadsheet.AsDataSet().Tables[0].AsEnumerable();
            //bool v =  rows.First().ItemArray.Contains(new object[10] { "EngineNumber", "Sell Date", "Customer name", "Gender", "Telephone", "Mobile", "Address", "Payment Date", "Price", "Invoice" });
            var row = rows.First();
            exceldatareader = spreadsheet;
            //bool t =
            //row.ItemArray.Contains("Engine Number") &&
            //row.ItemArray.Contains("Sell Date") &&
            //row.ItemArray.Contains("Customer name") &&
            //row.ItemArray.Contains("Gender") &&
            //row.ItemArray.Contains("Mobile") &&
            //row.ItemArray.Contains("Address") &&
            //row.ItemArray.Contains("Payment Date") &&
            //row.ItemArray.Contains("Price") &&
            //row.ItemArray.Contains("Invoice");
            //bool tt =
            //row.ItemArray.Contains("So may") &&
            //row.ItemArray.Contains("Ngay ban") &&
            //row.ItemArray.Contains("Ho ten khach hang") &&
            //row.ItemArray.Contains("Gioi tinh") &&
            //row.ItemArray.Contains("Dien thoai") &&
            //row.ItemArray.Contains("Di dong") &&
            //row.ItemArray.Contains("Ngay can thu") &&
            //row.ItemArray.Contains("Gia ban") &&
            //row.ItemArray.Contains("So hoa don");

            //if (t == tt && t == false)
            //    return false;
            ////foreach (var r in rows.Skip(1).Take(rows.Count()))
            ////{   
            ////    if (r.ItemArray[0].ToString().Length > 20 || r.ItemArray[0].ToString() == string.Empty)
            ////        return false;
            ////    if (r.ItemArray[2].ToString().ToCharArray().Length > 50 || r.ItemArray[2].ToString() == string.Empty)
            ////        return false;
            ////}
            return true;
        }

        public static Customer SaveCustomer(ServiceDataContext dc, SaleVehicleInfo v)
        {
            var newCus = MakeCustomer(v);
            Customer existedCus = null;
            if (!string.IsNullOrEmpty(newCus.IdentifyNumber)) existedCus = CustomerDAO.FindSingleCustomer(newCus.IdentifyNumber);
            if (existedCus == null)
            {
                newCus.CustomerId = dc.Customers.Max(c => c.CustomerId) + 1;
                dc.Customers.InsertOnSubmit(newCus);
                return newCus;
            }
            return existedCus;
        }

        private static Customer MakeCustomer(SaleVehicleInfo vehicle)
        {
            Customer newCus = new Customer();

            newCus.Address = vehicle.Address;
            newCus.BirthDate = vehicle.DOB;
            newCus.CustomerDescription = vehicle.CustomerDescription;
            newCus.IdentifyNumber = vehicle.IdentifyNumber;
            newCus.CustomerType = (int)CusType.Sale;
            newCus.DealerCode = UserHelper.DealerCode;
            newCus.DistrictId = vehicle.District;
            newCus.FullName = vehicle.CustomerName;
            newCus.JobTypeId = vehicle.JobType;
            newCus.Mobile = vehicle.Mobile;
            newCus.Precinct = vehicle.Precint;
            newCus.Priority = vehicle.Priority;
            newCus.ProvinceId = vehicle.Province;
            newCus.Tel = vehicle.Tel;

            return newCus;
        }

        public static Customer CloneWarrantyCustomer(ServiceDataContext dc, SaleVehicleInfo v, Customer c)
        {
            // Clone new customer
            var newCus = new Customer();
            newCus.Address = c.Address;
            newCus.BirthDate = c.BirthDate;
            newCus.CustomerType = (int)CusType.WarrantyInfo;
            newCus.DealerCode = c.DealerCode;
            newCus.DistrictId = c.DistrictId;
            newCus.Email = c.Email;
            newCus.ForService = false;
            newCus.FullName = c.FullName;
            newCus.Gender = c.Gender;
            newCus.IdentifyNumber = c.IdentifyNumber;
            newCus.JobTypeId = c.JobTypeId;
            newCus.Mobile = c.Mobile;
            newCus.Precinct = c.Precinct;
            newCus.Priority = c.Priority;
            newCus.ProvinceId = c.ProvinceId;
            newCus.Tel = c.Tel;
            dc.Customers.InsertOnSubmit(newCus);
            return newCus;
        }

        public static SaleSellItem SaveSellItem(VehicleDataContext dc, SaleVehicleInfo vehicleInfo)
        {
            var newItem = new SaleSellItem();
            newItem.Commentsellitem = vehicleInfo.CommentSellItem;
            newItem.NumberPlate = vehicleInfo.NumberPlate;
            newItem.Numplaterecdate = vehicleInfo.NumberPlateRecDate;
            newItem.PaymentDate = vehicleInfo.PaymentDate;
            newItem.PaymentType = vehicleInfo.PaymentType;
            newItem.PriceBeforeTax = vehicleInfo.Price;
            newItem.SellType = vehicleInfo.SellType;
            newItem.TaxAmt = 0;
            dc.SaleSellItems.InsertOnSubmit(newItem);
            return newItem;
        }
        /// <summary>
        /// Save payment to DB
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="v"></param>
        /// <param name="item"></param>
        public static void SavePayments(VehicleDataContext dc, SaleVehicleInfo v, SaleSellItem item)
        {
            if (v.PaymentType == (int)CusPaymentType.FixedHire_purchase)
            {
                if (v.FHPPayingDate.Count == 0)
                    BindFHPPayments(ref v);
                // Save the first payment
                var firstPayment = new SalePayment();
                firstPayment.Amount = v.FirstInstalmentAmount;
                firstPayment.Paymentdate = v.PaymentDate;
                firstPayment.SaleSellitem = item;
                firstPayment.Status = 0;

                dc.SalePayments.InsertOnSubmit(firstPayment);

                for (int i = 0; i < v.FHPPayingDate.Count; i++)
                {
                    var newPayment = new SalePayment();
                    newPayment.Amount = v.FHPAmount[i];
                    newPayment.Paymentdate = v.FHPPayingDate[i];
                    newPayment.SaleSellitem = item;
                    newPayment.Status = 0;

                    dc.SalePayments.InsertOnSubmit(newPayment);
                }
            }
            if (v.PaymentType == (int)CusPaymentType.UnFixedHire_purchase)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (v.UHPPayingDate[i] == DateTime.MinValue || v.UHPAmount[i] <= 0) continue;
                    var newPayment = new SalePayment();
                    newPayment.Amount = v.UHPAmount[i];
                    newPayment.Paymentdate = v.UHPPayingDate[i];
                    newPayment.Status = 0;
                    newPayment.SaleSellitem = item;
                    dc.SalePayments.InsertOnSubmit(newPayment);
                }
            }
        }
        /// <summary>
        /// Bind fixed instalments
        /// </summary>
        /// <param name="v"></param>
        public static void BindFHPPayments(ref SaleVehicleInfo v)
        {
            int remainingMoney = v.Price - v.FirstInstalmentAmount;
            for (int i = 0; i < v.InstalmentTimes - 1; i++)
            {
                v.FHPAmount.Add(remainingMoney / (v.InstalmentTimes - 1));
                v.FHPPayingDate.Add(v.PaymentDate.AddDays(i * v.DaysEachInstalment));
            }
        }





        /// <summary>
        /// Check that vehicle is legal or not
        /// </summary>
        /// <param name="v">Vehicle to check</param>
        /// <returns>Checking result</returns>
        public static string CheckVehicleInfo(SaleVehicleInfo v, SaveSaleAction a)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            var instance = VehicleDAO.GetVehicle(dc, v.EngineNumber);
            List<string> ee = new List<string>();
            if (string.IsNullOrEmpty(v.BillNumber))
                ee.Add(FExcel.Invoice);
            if (string.IsNullOrEmpty(v.CustomerName))
                ee.Add(FExcel.CustomerName);
            if (!(v.SellDate > DateTime.MinValue))
                ee.Add(FExcel.SellDate);
            if (string.IsNullOrEmpty(v.EngineNumber))
                ee.Add(FExcel.EngineNumber);
            if (ee.Count > 0)
            {
                string str = Message.BankFied;
                foreach (string s in ee)
                {
                    str += s + ", ";
                }
                str = str.Substring(0, str.Length - 2);
                return str;
            }
            if (instance == null)
            {
                return Message.NoVehicle;
            }
            if (instance.DealerCode != UserHelper.DealerCode)
                return Message.WrongVehiceNumber;
            if (!UserHelper.IsAdmin)
            {
                if (instance.DealerCode != UserHelper.DealerCode || instance.BranchCode != UserHelper.BranchCode)
                    return Message.YouMustBeAdmin;
            }
            if (instance.Status == (int)ItemStatus.NotArrived)
                return Message.VehicleNotArrived;
            if (instance.Status != (int)ItemStatus.Imported)
                return Customers.MotorInformationNull;

            if (v.SellDate > DateTime.Now || v.SellDate < instance.ImportedDate)
                return Customers.WrongSellingDate;

            if (InventoryHelper.IsInventoryLock(v.SellDate, UserHelper.DealerCode, instance.BranchCode))
                return Message.InventoryLocked;

            if (v.PaymentDate != DateTime.MinValue && v.PaymentDate < v.SellDate)
                return Customers.RecDateSmallerThanSelDate;

            if (a == SaveSaleAction.ImportVehicles)
            {
                var str = CheckPayments(v);
                if (str != Message.ActionSucessful)
                    return str;
            }

            if (string.IsNullOrEmpty(v.CustomerName))
                return Customers.CustomerEmpty;

            return Message.ActionSucessful;
        }

        public static string CheckPayments(SaleVehicleInfo v)
        {
            if (v.PaymentType != 0)
            {
                if (v.Price < 0)
                    return Customers.PriceEmpty;
            }

            if (v.PaymentType == 1 && v.PaymentDate == DateTime.MinValue)
                return Customers.txtDateRecEmpty;

            if (v.PaymentType == (int)CusPaymentType.UnFixedHire_purchase)
            {
                int sumUHPMoney = 0;
                for (int i = 0; i < 5; i++)
                {
                    if (v.UHPPayingDate[i] != DateTime.MinValue && v.UHPAmount[i] > 0)
                    {
                        if (v.UHPPayingDate[i] < v.PaymentDate)
                            return Customers.DateUFHPValid;
                        sumUHPMoney += v.UHPAmount[i];
                        for (int j = 0; j < i; j++)
                        {
                            if (v.UHPPayingDate[i] == v.UHPPayingDate[j])
                                return Customers.DateUFHPValid;
                        }
                    }
                }
                if (sumUHPMoney != v.Price)
                    return Message.MoneyInvalid;
            }

            if (v.PaymentType == (int)CusPaymentType.FixedHire_purchase)
            {
                if (v.FirstInstalmentAmount > v.Price)
                    return Customers.MoneyValid;
                if (v.InstalmentTimes <= 1)
                    return Message.Cus_SumMoneyInvalid;
            }

            return Message.ActionSucessful;
        }

        /// <summary>
        /// Save a single vehicle into DB
        /// </summary>
        /// <param name="v">Vehicle to save</param>
        public static void SaveSaleVehicleInfo(SaleVehicleInfo v, SaveSaleAction action)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            var serviceDc = DCFactory.GetDataContext<ServiceDataContext>();
            // Save customer and his clone
            Customer customer;
            if (v.CustomerId > 0)
            {
                customer = serviceDc.Customers.SingleOrDefault(c => c.CustomerId == v.CustomerId);
            }
            else
                customer = SaveCustomer(serviceDc, v);
            var warrantyCustomer = CloneWarrantyCustomer(serviceDc, v, customer);
            serviceDc.SubmitChanges();

            var sellItem = SaveSellItem(dc, v);
            try
            {
                // Save sell item
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                // Error while saving sell item, delete customer
                if (v.CustomerId == 0) // if it's new customer, delete it
                    serviceDc.Customers.DeleteOnSubmit(customer);
                serviceDc.Customers.DeleteOnSubmit(warrantyCustomer);
                serviceDc.SubmitChanges();
                throw ex;
            }

            var instance = VehicleHelper.UpdateItemInstance(dc, v.EngineNumber, ItemStatus.Sold, v.SellDate, v.Price, v.PaymentType, null);
            if (action == SaveSaleAction.ImportVehicles || CommonSaleData.PaymentType == (int)CusPaymentType.PayAll)
                SavePayments(dc, v, sellItem);

            var invoice = new SaleInvoice();
            invoice.SellItemId = sellItem.SellItemId;
            invoice.ItemInstanceId = instance.ItemInstanceId;
            invoice.CreatedBy = UserHelper.Username;
            invoice.CreatedDate = DateTime.Now;
            invoice.DealerCode = UserHelper.DealerCode;
            invoice.EngineNumber = v.EngineNumber;
            invoice.InvoiceNumber = v.BillNumber;
            invoice.SellDate = v.SellDate;
            invoice.BranchCode = instance.BranchCode;
            invoice.CustomerId = customer.CustomerId;
            dc.SaleInvoices.InsertOnSubmit(invoice);
            try
            {
                // Update item instance, insert payments, warranty info and invoice
                dc.SubmitChanges();
                VDMS.I.Service.ServiceTools.SaveWarrantyInfo(v.EngineNumber, 0, v.SellDate, instance.DatabaseCode, instance.Item.ItemCode, instance.Color, UserHelper.DealerCode, warrantyCustomer.CustomerId);
            }
            catch (Exception ex)
            {
                // Roll back
                if (v.CustomerId == 0)
                    serviceDc.Customers.DeleteOnSubmit(customer);
                serviceDc.Customers.DeleteOnSubmit(warrantyCustomer);
                serviceDc.SubmitChanges();
                dc.SaleSellItems.DeleteOnSubmit(sellItem);
                dc.SubmitChanges();
                throw ex;
            }
        }
        /// <summary>
        /// Save all vehicles into DB
        /// </summary>
        /// <returns>Result string</returns>
        public static string SaveAllVehicleInfoes(SaveSaleAction a)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            int row = VDMSSetting.CurrentSetting.SaleVehicleExcelUploadSetting.StartRow;
            List<string> errorRows = new List<string>();
            int importedRow = 0;

            foreach (var v in Vehicles)
            {
                try
                {
                    if (v.IsValid)
                    {
                        SaveSaleVehicleInfo(v, a);
                        importedRow += 1;
                    }
                    else
                        errorRows.Add(row.ToString());
                }
                catch
                {
                    errorRows.Add(row.ToString());
                }
                finally
                {
                    row++;
                }
            }

            if (errorRows.Count() > 0)
            {
                var x = string.Join(", ", errorRows.ToArray());
                return string.Format(Message.ImportExcelSuccessful, importedRow) + string.Format(Message.ImportExcelError, string.Join(", ", errorRows.ToArray()));
            }
            return Message.ActionSucessful;
        }

        public static void SaveBatchInvoice()
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();

            var sellItem = SaveSellItem(dc, CommonSaleData);

            var batchInvoice = new SaleBatchinvoiceheader();

            batchInvoice.Dealercode = UserHelper.DealerCode;
        }


        /// <summary>
        /// Check all vehicles for errors
        /// </summary>
        /// <returns>String that indicates that action is successful or not</returns>
        public static string CheckAllVehicleInfoes(SaveSaleAction a)
        {
            string resText = null;
            int row = 1;

            if (Vehicles.Count == 0)
                return Message.NoVehicle;

            foreach (var v in SaleVehicleHelper.Vehicles)
            {
                v.IsValid = true;
                resText = SaleVehicleHelper.CheckVehicleInfo(v, a);
                if (resText != Resources.Message.ActionSucessful)
                {
                    v.IsValid = false;
                    v.Error = resText;
                }
                row++;
            }
            if (resText != Message.ActionSucessful)
                return string.Format(Message.ImportExcelError, row, resText);
            else return Message.ActionSucessful;
        }

        public static void SumLotSalePrice()
        {
            CommonSaleData.Price = Vehicles.Sum(v => v.Price);

            if (CommonSaleData.PaymentType == (int)CusPaymentType.FixedHire_purchase)
            {
                var commonSaleData = new SaleVehicleInfo();
                commonSaleData.FirstInstalmentAmount = CommonSaleData.FirstInstalmentAmount;
                commonSaleData.InstalmentTimes = CommonSaleData.InstalmentTimes;
                commonSaleData.DaysEachInstalment = CommonSaleData.DaysEachInstalment;
                BindFHPPayments(ref commonSaleData);
                //CommonSaleData.FHPAmount = commonSaleData.FHPAmount.AsEnumerable().ToList();
                //CommonSaleData.FHPPayingDate = commonSaleData.FHPPayingDate.AsEnumerable().ToList();
            }
        }

        public static string CheckLotSale()
        {
            var rs = CheckAllVehicleInfoes(SaveSaleAction.LotSale);
            if (rs == Message.ActionSucessful)
            {
                SumLotSalePrice();
                rs = CheckPayments(CommonSaleData);
            }
            return rs;
        }

        /// <summary>
        /// Bind the common data : customer, invoice... to all vehicles
        /// </summary>
        public static void BindCommonData()
        {
            foreach (var v in Vehicles)
            {
                v.Address = CommonSaleData.Address;
                v.BillNumber = CommonSaleData.BillNumber;
                v.CommentSellItem = CommonSaleData.CommentSellItem;
                v.CustomerDescription = CommonSaleData.CustomerDescription;
                v.CustomerId = CommonSaleData.CustomerId;
                v.CustomerName = CommonSaleData.CustomerName;
                v.CustomerType = CommonSaleData.CustomerType;
                v.DaysEachInstalment = CommonSaleData.DaysEachInstalment;
                v.District = CommonSaleData.District;
                v.DOB = CommonSaleData.DOB;
                v.Email = CommonSaleData.Email;
                v.FirstInstalmentAmount = CommonSaleData.FirstInstalmentAmount;
                v.IdentifyNumber = CommonSaleData.IdentifyNumber;
                v.InstalmentTimes = CommonSaleData.InstalmentTimes;
                v.JobType = CommonSaleData.JobType;
                v.Mobile = CommonSaleData.Mobile;
                v.NumberPlate = CommonSaleData.NumberPlate;
                v.NumberPlateRecDate = CommonSaleData.NumberPlateRecDate;
                v.PaymentDate = CommonSaleData.PaymentDate;
                v.PaymentType = CommonSaleData.PaymentType;
                v.Precint = CommonSaleData.Precint;
                v.Price = CommonSaleData.Price;
                v.Priority = CommonSaleData.Priority;
                v.Province = CommonSaleData.Province;
                v.SellDate = CommonSaleData.SellDate;
                v.SellType = CommonSaleData.SellType;
                v.Tel = CommonSaleData.Tel;
            }
        }

        /// <summary>
        /// Get ImportDate of Vehice
        /// Return Default DateTime(01/01/0001) when not avalliable Enginenumber
        /// </summary>
        /// <param name="enginenumber"></param>
        /// <returns></returns>
        public static DateTime GetImportDateVehice(string enginenumber)
        {

            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            var instance = VehicleDAO.GetVehicle(dc, enginenumber);
            if (instance != null)
                return instance.ImportedDate;
            else
                return default(DateTime);
        }
    }
}