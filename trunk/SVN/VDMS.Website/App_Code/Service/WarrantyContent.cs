using System;
using System.Collections;
using System.Data;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using VDMS.Helper;
using VDMS.I.Entity;
using VDMS.I.Linq;
using VDMS.II.Linq;
using Broken = VDMS.Core.Domain.Broken;
using Customer = VDMS.Core.Domain.Customer;

namespace VDMS.I.Service
{
    public enum SrsState : int
    {
        AddNewSRS = 1,
        PlayOldSRS = 2,
        FinishEdit = 5,
        Saved = 3,
        SavedTemp = 4,
        SheetNotFound = 6,
        PlayOldPCV = 7,
    }
    public enum ServiceItemState
    {
        Persistent,
        Transient,
        Deleted,
    }

    public class SrsSetting
    {
        public const bool showSourceDealerWhenAlarm = true;    // dai ly sua <> dai ly nhap xe thi alarm ==> co kem ten dai ly nhap ko?
        public const bool allowInvalidSpare = true;
        public const bool allowInputSRSFeeAfterComplete = true;// cho phep nhap fee cua SRS sau khi da nhan nut "Hoan tat nhap lieu"
        public const bool showFeeAmountColumn = false;         // show col FeeAmount len thi fee dc tinh tong tu dong va khong cho nhap
        public const bool requireCheckWarrantyToAddPCV = true; // user must check warranty check box to add partChangeVoucher
        public const bool allWayPrint = true;                  // In SRS moi luc moi noi :)
        public const bool showTempSheetNo = false;             // hien so SRS/PVC khi chua luu?
        public const string imageFolderBase = "~/Images/";
        public const string imageAddNew = "new.jpg";
        public const int selectGridViewPageSize = 10;
        public const int selectSparePageSize = 50;
        public const int selectBrokenCodePageSize = 50;
    }

    [Serializable]
    public class SelectSparesInfo
    {
        public List<WarrantySpare> SelectingPCVSpares { get; set; }
        public List<WarrantySpare> SelectingSRSSpares { get; set; }

        public SelectSparesInfo()
        {
            SelectingPCVSpares = new List<WarrantySpare>();
            SelectingSRSSpares = new List<WarrantySpare>();
        }
    }

    [Serializable]
    public class SRSItem : Servicedetail
    {
        public string ExchangeNumber { get; set; }
        public long SpareAmount { get { return this.Unitprice * this.Partqty; } }
        public long FeeAmount { get; set; }
        public bool HasModified { get; set; }
        public ServiceItemState State { get; set; }

        public Servicedetail Base()
        {
            Servicedetail item = new Servicedetail(this.Partcode, this.Partname, this.Partqty, this.Unitprice, this.Serialnumber, this.Serviceheader);
            item.Id = this.Id;
            return item;
        }
        public SRSItem() : base() { this.State = ServiceItemState.Transient; this.HasModified = false; }
        public SRSItem(Warrantycondition spare)
            : this()
        {
            this.Partcode = spare.Partcode;
            this.Partname = (UserHelper.Language.Equals("vi-VN", StringComparison.OrdinalIgnoreCase)) ? spare.Partnamevn : spare.Partnameen;
            this.Unitprice = (long)spare.Unitprice;
        }
        public SRSItem(Servicedetail sd, ServiceItemState state)
            : this()
        {
            this.Id = sd.Id;
            this.Partcode = sd.Partcode;
            this.Partname = sd.Partname;
            this.Partqty = sd.Partqty;
            this.Serialnumber = sd.Serialnumber;
            this.Serviceheader = sd.Serviceheader;
            this.Unitprice = sd.Unitprice;
            this.State = state;
        }
    }
    [Serializable]
    public class PCVItem : Exchangepartdetail
    {
        public bool WarrantyWarn { get; set; }
        public string PartName { get; set; }
        public string SManPower { get; set; }
        public double ManPower
        {
            get
            {
                if (string.IsNullOrEmpty(this.SManPower)) return 0;
                return double.Parse(this.SManPower, new CultureInfo("en-US"));
            }
        }
        public decimal Labour { get; set; }
        public long SpareAmountO { get { return this.Unitpriceo * this.Partqtyo; } }
        public double FeeAmount
        {
            get
            {
                return ((double)((double)this.Labour * this.ManPower * this.Partqtyo));
            }
        }
        public bool HasModified { get; set; }
        public ServiceItemState State { get; set; }

        public Exchangepartdetail Base()
        {
            Exchangepartdetail item = new Exchangepartdetail(this.Partcodeo, this.Partcodem, this.Serialnumber, this.Unitpricem, (long)this.FeeAmount, (long)this.FeeAmount, this.Unitpriceo, this.Partqtyo, this.Partqtym, this.Vmepcomment, this.Broken, this.Exchangepartheader);
            item.Id = this.Id;
            return item;
        }
        public PCVItem() : base() { this.State = ServiceItemState.Transient; this.HasModified = false; }
        public PCVItem(Warrantycondition spare)
            : this()
        {
            this.Partcodem = this.Partcodeo = spare.Partcode;
            this.PartName = (UserHelper.Language.Equals("vi-VN", StringComparison.OrdinalIgnoreCase)) ? spare.Partnamevn : spare.Partnameen;
            this.Unitpricem = this.Unitpriceo = (long)spare.Unitprice;
            this.Labour = spare.Labour;
            this.SManPower = spare.Manpower;
        }
        public PCVItem(Exchangepartdetail ed, ServiceItemState state)
            : this()
        {
            this.Id = ed.Id;
            this.Broken = ed.Broken;
            this.Exchangepartheader = ed.Exchangepartheader;
            this.Partcodem = ed.Partcodem;
            this.Partcodeo = ed.Partcodeo;
            this.Partqtym = ed.Partqtym;
            this.Partqtyo = ed.Partqtyo;
            this.Serialnumber = ed.Serialnumber;
            this.Totalfeem = ed.Totalfeem;
            this.Totalfeeo = ed.Totalfeeo;
            this.Unitpricem = ed.Unitpricem;
            this.Unitpriceo = ed.Unitpriceo;
            this.Vmepcomment = ed.Vmepcomment;

            Warrantycondition warr = WarrantyContent.GetWarrantyCondition(ed.Partcodeo);
            //if (warr == null) throw new Exception(string.Format("Part Code not found: {0}", ed.Partcodeo));
            if (warr != null)
            {
                this.PartName = (UserHelper.Language.Equals("vi-VN")) ? warr.Partnamevn : warr.Partnameen;
                this.Labour = warr.Labour;
                this.SManPower = warr.Manpower;
            }
            this.State = state;
        }
    }
    [Serializable]
    public class WarrantySpare : Warrantycondition
    {
        public int Quantity { get; set; }
        public decimal NewUnitPrice { get; set; }

        public WarrantySpare(int qty, string partcode, string partnamevn, string partnameen, string motorcode, long warrantytime, decimal warrantylength, decimal labour, string manpower, decimal unitprice, decimal newUnitprice)
        {
            this.Quantity = qty;
            this.NewUnitPrice = newUnitprice;

            this._partcode = partcode;
            this._partnamevn = partnamevn;
            this._partnameen = partnameen;
            this._motorcode = motorcode;
            this._warrantytime = warrantytime;
            this._warrantylength = warrantylength;
            this._labour = labour;
            this._manpower = manpower;
            this._unitprice = unitprice;
        }
    }

    [Serializable]
    public class SrsInfo
    {
        private static Random KeyMaker = new Random(DateTime.Now.Millisecond);

        public int PageKey { get; set; }
        public string RequestURL { get; set; }
        public string ItemDBCode { get; set; }
        public string ItemSoldDealer { get; set; }
        public bool CanChangeBuyDate
        {
            get { return !this.IsItemHasBuyDate || (this.ServiceHeader == null) || (this.ServiceHeader.Purchasedate == DateTime.MinValue); }
        }
        public bool CanChangeFrameNumber { get; set; }
        public bool CanChangePlateNumber { get; set; }
        public bool ReadOnly { get; set; }

        public bool HasPCV { get; set; }
        public bool IsPersistent { get { return (this.ServiceHeader != null) && (this.ServiceHeader.Id > 0); } }
        public bool IsItemExist { get; set; }
        public bool IsItemHasBuyDate { get; set; }
        public bool IsItemOnTipTop { get; set; }
        public long LastKm { get; set; }
        public long OldLastKm { get; set; }

        public SrsState CurrentSate { get; set; }
        public Serviceheader ServiceHeader { get; set; }
        public Exchangepartheader ExchangePartHeader { get; set; }

        public SRSItem UpdateSRSItem(Warrantycondition warr, int quantity, decimal newUnitPrice)
        {
            SRSItem item = this.FindServiceItem(warr.Partcode);
            if ((item == null) && (quantity > 0))   // chua co thi add
            {
                item = new SRSItem(warr);
                item.State = ServiceItemState.Transient;
                item.Serviceheader = this.ServiceHeader;
                this.ServiceDetail.Add(item);
            }

            if (quantity > 0)  // co => update
            {
                item.HasModified = true;
                item.Partqty = quantity;
                item.Unitprice = (long)newUnitPrice;
            }
            else    // co nhung quantity==0 => remove
            {
                this.ServiceDetail.Remove(item);
                item = null;
            }

            return item;
        }
        public PCVItem UpdatePCVItem(Warrantycondition warr, int quantity)
        {
            PCVItem item = this.FindExchangeItem(warr.Partcode);
            if (item == null)   // chua co thi add
            {
                item = new PCVItem(warr);
                item.State = ServiceItemState.Transient;
                item.Partqtyo = quantity;
                item.Exchangepartheader = this.ExchangePartHeader;
                this.ExchangePartDetail.Add(item);
            }
            else if (quantity > 0)  // co => update
            {
                item.HasModified = true;
                item.Partqtyo = quantity;
                item.Unitpriceo = (long)warr.Unitprice;
            }
            else    // co nhung quantity==0 => remove
            {
                this.ExchangePartDetail.Remove(item);
                item = null;
            }

            return item;
        }

        public SRSItem FindServiceItem(string spareNumber)
        {
            return this.ServiceDetail.FirstOrDefault(item => item.Partcode.Equals(spareNumber.Trim(), StringComparison.OrdinalIgnoreCase));
        }
        public PCVItem FindExchangeItem(string spareNumber)
        {
            return this.ExchangePartDetail.FirstOrDefault(item => item.Partcodeo.Equals(spareNumber.Trim(), StringComparison.OrdinalIgnoreCase));
        }
        public List<SRSItem> ServiceDetail { get; set; }
        public List<PCVItem> ExchangePartDetail { get; set; }
        public List<WarrantySpare> SelectingSpares { get; set; }

        public static List<SRSItem> ConvertToSRSItem(List<Servicedetail> items, ServiceItemState state)
        {
            List<SRSItem> res = new List<SRSItem>();
            foreach (Servicedetail item in items)
            {
                res.Add(new SRSItem(item, state));
            }
            return res;
        }
        public static List<PCVItem> ConvertToPCVItem(List<Exchangepartdetail> items, ServiceItemState state)
        {
            List<PCVItem> res = new List<PCVItem>();
            foreach (Exchangepartdetail item in items)
            {
                res.Add(new PCVItem(item, state));
            }
            return res;
        }
        public long GetSparesAmount()
        {
            var list = from item in this.ServiceDetail where item.State != ServiceItemState.Deleted select item;
            return list.Sum(s => s.Partqty * s.Unitprice);
        }
        public double GetExchangeTotalFee()
        {
            var list = from item in this.ExchangePartDetail where item.State != ServiceItemState.Deleted select item;
            return list.Sum(x => x.FeeAmount);
        }
        public void SyncData()
        {
            this.ServiceHeader.Totalamount = this.GetSparesAmount() + this.ServiceHeader.Feeamount;
        }

        public SrsInfo()
        {
            this.PageKey = KeyMaker.Next();
            this.ServiceDetail = new List<SRSItem>();
            this.ExchangePartDetail = new List<PCVItem>();
            this.SelectingSpares = new List<WarrantySpare>();

            this.ServiceHeader = new Serviceheader();
            this.ExchangePartHeader = new Exchangepartheader();
            this.HasPCV = false;
            this.CurrentSate = SrsState.AddNewSRS;
            this.ReadOnly = false;
            this.IsItemExist = false;
            this.IsItemHasBuyDate = false;
            this.CanChangeFrameNumber = false;
            this.CanChangePlateNumber = false;
        }
    }

    // NMChi: 
    // 24/11/2007
    public enum WarrantyContentErrorCode : int
    {
        OK = 0,
        InvalidSpareCode = 1,
        EngineNumberNotFound = 2,
        SpareNumberNotFound = 3,
        SaveHeaderFailed = 4,
        SaveDetailFailed = 5,
        UpdateDataFailed = 6,
        ItemTypeNotFound = 7,
        InvalidDateTimeValue = 8,
        InvalidServiceType = 9,
        BrokenCodeNotFound = 10,
        InvalidExchangeSparesList = 11,
        SaveExchHeaderFailed = 12,
        SaveExchDetailFailed = 13,
        ServiceSheetNumberNotFound = 14,
        ExchangeNumberNotFound = 15,
        NoItemSold = 16,
        StringTooLong = 17,
        ItemSoldByOtherDealer = 18,
        LastKmChanged = 19,
        ItemNotSold = 20,
        InCompleteSpares = 21,
    }

    public partial class WarrantyContent
    {
        public static DataTable SpareListOnServiceSchema
        {
            get
            {
                DataTable tbl = new DataTable();
                tbl.Columns.Add("ItemId");
                tbl.Columns.Add("SpareNo");
                tbl.Columns.Add("SpareNumber");
                tbl.Columns.Add("SpareNameEn");
                tbl.Columns.Add("SpareNameVn");
                tbl.Columns.Add("SpareName");
                tbl.Columns.Add("Quantity");
                tbl.Columns.Add("SpareCost");
                tbl.Columns.Add("ExchangeNumber");
                tbl.Columns.Add("IsExchangeSpare");
                tbl.Columns.Add("SpareAmount");
                tbl.Columns.Add("FeeAmount");
                return tbl;
            }
        }


        private static bool SheetNumberExist(string number)
        {
            IDao<Serviceheader, long> dao = DaoFactory.GetDao<Serviceheader, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Servicesheetnumber", number.Trim().ToUpper()) });
            IList list = dao.GetAll();
            if ((list != null) && (list.Count > 0)) return true;
            else return false;
        }
        public static string GenSheetNumber(string DealerCode)
        {
            return GenSheetNumber(false, DealerCode);
        }
        public static string GenSheetNumber(bool temp, string DealerCode)
        {
            DateTime dt = DateTime.Now;
            IDao<Serviceheader, long> dao = DaoFactory.GetDao<Serviceheader, long>();
            string num = (temp ? "T_" : "") + "S" + DealerCode + dt.ToString("yyMMdd");

            dao.SetCriteria(new ICriterion[] { Expression.Ge("Status", 0), Expression.Like("Servicesheetnumber", num + "%") });
            int count = dao.GetAll().Count + 1;
            //NHibernate.ISession session = NHibernateSessionManager.Instance.GetSession();
            //object count = session.CreateQuery("select count(*) from Serviceheader {sh} where {sh}.Servicesheetnumber like ?").SetString(0, num + "%").UniqueResult();
            while (SheetNumberExist(num + count.ToString().PadLeft(4, '0'))) { count++; }

            num += count.ToString().PadLeft(4, '0');
            return num;
        }
        public static ServiceHeader FindServiceSheet(string engine)
        {
            //IDao<Serviceheader, long> dao = DaoFactory.GetDao<Serviceheader, long>();
            //dao.SetCriteria(new ICriterion[] { Expression.Ge("Status", 0), Expression.Eq("Enginenumber", engine) });
            //dao.SetOrder(new Order[] { Order.Desc("Servicedate") });
            //IList list = dao.GetAll();
            //if (list.Count < 1) return null;
            //return (Serviceheader)list[0];
            var db = DCFactory.GetDataContext<ServiceDataContext>();
            return (from sh in db.ServiceHeaders
                         where sh.Status >= 0
                               && sh.EngineNumber.Equals(engine.Trim())
                         select sh).OrderByDescending(p => p.ServiceDate).FirstOrDefault();
        }
        public static ServiceHeader GetServiceSheet(long id)
        {
            var db = DCFactory.GetDataContext<ServiceDataContext>();
            return db.ServiceHeaders.FirstOrDefault(p => p.ServiceHeaderId == id);
        }

        public static Warrantycondition GetWarrantyCondition(string partCode)
        {
            if (string.IsNullOrEmpty(partCode)) return null;

            IDao<Warrantycondition, Decimal> dao = DaoFactory.GetDao<Warrantycondition, Decimal>();
            dao.SetCriteria(new ICriterion[] { Expression.InsensitiveLike("Partcode", partCode.Trim(), MatchMode.Exact) });
            IList list = dao.GetAll();
            if (list.Count == 0) return null;
            return (Warrantycondition)list[0];
        }
        public static Broken GetBroken(string bcode)
        {
            IDao<Broken, long> dao = DaoFactory.GetDao<Broken, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Brokencode", bcode.Trim().ToUpper()) });
            IList list = dao.GetAll();
            if (list.Count != 1) return null;
            return (Broken)list[0];
        }
        public static Entity.Invoice GetCustInvoiceInfos(string engineNumber)
        {
            var db = DCFactory.GetDataContext<ServiceDataContext>();
            return db.Invoices.FirstOrDefault(p => p.EngineNumber.Equals(engineNumber.Trim()));
        }
        public static Customer GetCustInfos(long custId)
        {
            IDao<Customer, long> dao = DaoFactory.GetDao<Customer, long>();
            Customer cust = dao.GetById(custId, false);
            return cust;
        }
        public static Serviceheader GetSerHeader(string engineNumber)
        {
            IDao<Serviceheader, long> dao = DaoFactory.GetDao<Serviceheader, long>();
            // , Expression.Sql("Kmcount = MAX(Kmcount) Group by Enginenumber") 
            dao.SetCriteria(new ICriterion[] { Expression.Ge("Status", 0), Expression.Eq("Enginenumber", engineNumber.Trim().ToUpper()) });
            dao.SetOrder(new Order[] { Order.Desc("Kmcount") });
            IList list = dao.GetAll();
            if (list.Count <= 0) return null;
            return ((Serviceheader)list[0]);
        }

        public static Serviceheader SaveSerHeader(Serviceheader serH, out WarrantyContentErrorCode errCode)
        {
            errCode = WarrantyContentErrorCode.SaveHeaderFailed;

            if (string.IsNullOrEmpty(serH.Dealercode) || string.IsNullOrEmpty(serH.Branchcode)) return null;
            if (
                ((!string.IsNullOrEmpty(serH.Damaged.Trim()))
                && (serH.Customer != null)
                && (!string.IsNullOrEmpty(serH.Itemtype))
                && (!string.IsNullOrEmpty(serH.Colorcode)))
                ||
                (serH.Status < 0)
               )
            {
                IDao<Serviceheader, long> dao = DaoFactory.GetDao<Serviceheader, long>();
                serH.Servicesheetnumber = GenSheetNumber(serH.Status == (int)ServiceStatus.Temp, serH.Dealercode);
                serH.Createby = UserHelper.Username;
                try
                {
                    serH = dao.SaveOrUpdateCopy(serH);
                    errCode = WarrantyContentErrorCode.OK;
                }
                catch { serH = null; }
            }


            return serH;
        }
        public static Serviceheader SaveSerHeader(long sheetId, ServiceStatus status, string dealer, string branch, string enNum, string plateNum, string frameNum, string model, string color, string err, string solution, string exchangeNum, int serType, Customer cust, long km, long fee, long total, DateTime serDate, DateTime buyDate, out WarrantyContentErrorCode errCode)
        {
            errCode = WarrantyContentErrorCode.SaveHeaderFailed;

            if (string.IsNullOrEmpty(dealer) || string.IsNullOrEmpty(branch)) return null;

            Serviceheader serH = null;
            if (
                ((!string.IsNullOrEmpty(err.Trim()))
                && (cust != null)
                && (!string.IsNullOrEmpty(model))
                && (!string.IsNullOrEmpty(color)))
                ||
                (status < 0)
                )
            {
                IDao<Serviceheader, long> dao = DaoFactory.GetDao<Serviceheader, long>();

                serH = dao.GetById(sheetId, false); //true -> false
                if (serH == null)
                {
                    serH = new Serviceheader();
                }

                serH.Branchcode = branch;
                serH.Customer = cust;
                serH.Colorcode = color;
                serH.Damaged = err.Trim();
                serH.Dealercode = dealer;
                serH.Enginenumber = enNum.Trim().ToUpper();
                serH.Feeamount = fee;
                serH.Framenumber = frameNum;
                serH.Itemtype = model;
                serH.Kmcount = km;
                serH.Numberplate = plateNum.Trim().ToUpper();
                serH.Purchasedate = buyDate;
                serH.Repairresult = solution.Trim();
                serH.Servicedate = serDate;
                serH.Servicesheetnumber = GenSheetNumber(status == ServiceStatus.Temp, dealer);
                serH.Servicetype = serType;
                serH.Totalamount = total;
                serH.Status = (int)status;
                serH.Createby = UserHelper.Username;

                try { dao.SaveOrUpdate(serH); }
                catch { serH = null; }
            }

            errCode = WarrantyContentErrorCode.OK;
            return serH;
        }

        public static WarrantyContentErrorCode SaveSerDetail(string partCode, string partName, int quantity, long price, Serviceheader serH)
        {
            IDao<Servicedetail, long> dao = DaoFactory.GetDao<Servicedetail, long>();
            return SaveSerDetail(string.Empty, partCode, partName, quantity, price, serH, dao);
        }
        public static WarrantyContentErrorCode SaveSerDetail(string itemId, string partCode, string partName, int quantity, long price, Serviceheader serH, IDao<Servicedetail, long> dao)
        {
            long id;
            long.TryParse(itemId, out id);

            Servicedetail serD = dao.GetById(id, false); //true -> false
            if (serD == null)
            {
                serD = new Servicedetail();
            }

            serD.Partcode = partCode.Trim().ToUpper();
            serD.Partqty = quantity;
            serD.Serviceheader = serH;
            serD.Unitprice = price;
            serD.Partname = partName;
            try
            {
                dao.SaveOrUpdate(serD);
            }
            catch { return WarrantyContentErrorCode.SaveDetailFailed; }
            return WarrantyContentErrorCode.OK;
        }
        public static WarrantyContentErrorCode SaveSerDetails(DataTable serDetail, Serviceheader serH)
        {
            int quantity;
            long price;
            IDao<Servicedetail, long> dao = DaoFactory.GetDao<Servicedetail, long>();

            foreach (DataRow row in serDetail.Rows)
            {
                if (string.IsNullOrEmpty(row["IsExchangeSpare"].ToString()))
                {
                    int.TryParse(row["Quantity"].ToString(), out quantity);
                    long.TryParse(row["SpareCost"].ToString(), out price);
                    if (SaveSerDetail(row["ItemId"].ToString(), row["SpareNumber"].ToString(), row["SpareName"].ToString(), quantity, price, serH, dao) != WarrantyContentErrorCode.OK) return WarrantyContentErrorCode.SaveDetailFailed;
                }
            }
            return WarrantyContentErrorCode.OK;
        }
        public static WarrantyContentErrorCode SaveSerDetails(IList<SRSItem> serDetail, Serviceheader serH)
        {
            IDao<Servicedetail, long> dao = DaoFactory.GetDao<Servicedetail, long>();
            try
            {
                foreach (SRSItem item in serDetail)
                {
                    if ((item.State == ServiceItemState.Transient) || item.HasModified || (item.Serviceheader == null) || (item.Serviceheader.Id != serH.Id))
                    {
                        Servicedetail sd = item.Base();
                        if (item.State == ServiceItemState.Deleted)
                            dao.Delete(sd.Id);
                        else
                        {
                            var i = dao.SaveOrUpdateCopy(sd);
                            if (sd.Id == 0) sd.Id = i.Id;
                        }
                    }
                }
            }
            catch { return WarrantyContentErrorCode.SaveDetailFailed; }
            return WarrantyContentErrorCode.OK;
        }

        public static List<Servicedetail> GetServiceDetail(long sId)
        {
            IDao<Servicedetail, long> dao = DaoFactory.GetDao<Servicedetail, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Serviceheader.Id", sId) });
            return dao.GetAll();
        }
        public static Exchangepartheader GetExchangePartHeader(long hId)
        {
            IDao<Exchangepartheader, long> dao = DaoFactory.GetDao<Exchangepartheader, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Serviceheader.Id", hId) });
            List<Exchangepartheader> list = dao.GetAll();
            return (list.Count > 0) ? list[0] : null;
        }
        public static List<Exchangepartdetail> GetExchangePartDetail(long hId)
        {
            IDao<Exchangepartdetail, long> dao = DaoFactory.GetDao<Exchangepartdetail, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Exchangepartheader.Id", hId) });
            return dao.GetAll();
        }

    }
}