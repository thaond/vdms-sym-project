using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web;
using NHibernate;
using NHibernate.Expression;
using VDMS.Common.Data;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.IDAL.Interface;
using VDMS.Helper;
using VDMS.I.Service;

namespace VDMS.I.ObjectDataSource
{
    #region declare objects
    public class ExchangeListHeader : TreeListObject
    {
        private int no, status;
        long km, proposeFee;
        private string engineNumber, model, custName, sheetNumber, voucherNumber, statusString;
        private DateTime repairDate, buyDate, confirmedDate;
        private ExchangeSparesList exchangeSpares;

        public long feeO, feeM;

        public int No
        {
            get { return no; }
            set { no = value; }
        }
        public long ProposeFee
        {
            get { return proposeFee; }
            set { proposeFee = value; }
        }
        public long Km
        {
            get { return km; }
            set { km = value; }
        }
        public string EngineNumber
        {
            get { return engineNumber; }
            set { engineNumber = value; }
        }
        public string Model
        {
            get { return model; }
            set { model = value; }
        }
        public string CustomerName
        {
            get { return custName; }
            set { custName = value; }
        }
        public string ServiceSheetNumber
        {
            get { return sheetNumber; }
            set { sheetNumber = value; }
        }
        public string ExchangeVoucherNumber
        {
            get { return voucherNumber; }
            set { voucherNumber = value; }
        }
        public string StatusString
        {
            get { return statusString; }
            set { statusString = value; }
        }
        public int Status
        {
            get { return status; }
            set { status = value; }
        }
        public DateTime RepairDate
        {
            get { return repairDate; }
            set { repairDate = value; }
        }
        public DateTime BuyDate
        {
            get { return buyDate; }
            set { buyDate = value; }
        }
        public DateTime ConfirmedDate
        {
            get { return confirmedDate; }
            set { confirmedDate = value; }
        }

        public ExchangeSparesList ExchangeSpares
        {
            get { return exchangeSpares; }
            set { exchangeSpares = value; }
        }

        public ExchangeListHeader()
        {
            ExchangeSpares = new ExchangeSparesList();
        }

        public ExchangeListHeader(string engineNum, string model, long km, string custName, DateTime repairDate, DateTime buyDate)
            : this()
        {
            this.EngineNumber = engineNum;
            this.CustomerName = custName;
            this.RepairDate = repairDate;
            this.BuyDate = buyDate;
            this.Model = model;
            this.Km = km;
        }
    }

    public class ExchangeSpare : TreeListObject
    {
        private string spareNumberO, spareNumberM, comment;
        private int quantityO, quantityM;
        private long labourO, labourM, unitPriceO, spareAmountO, feeAmountO, unitPriceM, spareAmountM, feeAmountM;
        double manPowerO, manPowerM;

        public string Comments
        {
            get { return comment; }
            set { comment = value; }
        }
        public string SpareNumberO
        {
            get { return spareNumberO; }
            set { spareNumberO = value; }
        }
        public string SpareNumberM
        {
            get { return spareNumberM; }
            set { spareNumberM = value; }
        }
        public int QuantityO
        {
            get { return quantityO; }
            set { quantityO = value; spareAmountO = UnitPriceO * value; }
        }
        public int QuantityM
        {
            get { return quantityM; }
            set { quantityM = value; spareAmountM = UnitPriceM * value; }
        }
        public long TotalO
        {
            get { return WarrantySpareAmountO + WarrantyFeeAmountO; }
        }
        public long TotalM
        {
            get { return WarrantySpareAmountM + WarrantyFeeAmountM; }
        }
        public long UnitPriceO
        {
            get { return unitPriceO; }
            set { unitPriceO = value; spareAmountO = QuantityO * value; }
        }
        public long UnitPriceM
        {
            get { return unitPriceM; }
            set { unitPriceM = value; spareAmountM = QuantityM * value; }
        }
        public double ManPowerO
        {
            get { return manPowerO; }
            set { manPowerO = value; WarrantyFeeAmountO = (long)(labourO * ManPowerO); }
        }
        public double ManPowerM
        {
            get { return manPowerM; }
            set { manPowerM = value; WarrantyFeeAmountM = (long)(labourM * ManPowerM); }
        }
        public long LabourO
        {
            get { return labourO; }
            set { labourO = value; }
        }
        public long LabourM
        {
            get { return labourM; }
            set { labourM = value; }
        }

        public long WarrantySpareAmountO
        {
            get { return spareAmountO; }
            //set { spareAmountO = value; }
        }
        public long WarrantySpareAmountM
        {
            get { return spareAmountM; }
            //set { spareAmountM = value; }
        }
        public long WarrantyFeeAmountO
        {
            get { return feeAmountO; }
            set { feeAmountO = value; }
        }
        public long WarrantyFeeAmountM
        {
            get { return feeAmountM; }
            set { feeAmountM = value; }
        }

        public ExchangeSpare() { }
    }

    public class ExchangeSparesList : TreeListObjects
    {
        public ExchangeSparesList()
            : base()
        {
        }
        public ExchangeSparesList(string voucherNumber)
            : this()
        {

        }
    }
    #endregion

    public class ExchangeVoucherHeaderDataSource
    {
        #region nmChi
        public ExchangeVoucherHeaderDataSource()
        {
            byAreas = new List<string>();
        }
        public static DataTable ExchangeVoucherTableSchema
        {
            get
            {
                DataTable tbl = new DataTable();
                tbl.Columns.Add("No");
                tbl.Columns.Add("BuyDate");
                tbl.Columns.Add("RepairDate");
                tbl.Columns.Add("ConfirmedDate");
                tbl.Columns.Add("EngineNumber");
                tbl.Columns.Add("Model");
                tbl.Columns.Add("Km");
                tbl.Columns.Add("CustomerName");
                tbl.Columns.Add("ServiceSheetNumber");
                tbl.Columns.Add("ExchangeVoucherNumber");
                tbl.Columns.Add("SpareNumberO");
                tbl.Columns.Add("SpareNumberM");
                tbl.Columns.Add("QuantityO");
                tbl.Columns.Add("QuantityM");
                tbl.Columns.Add("UnitPriceO");
                tbl.Columns.Add("UnitPriceM");
                tbl.Columns.Add("ManPowerM");
                tbl.Columns.Add("ManPowerO");
                tbl.Columns.Add("WarrantySpareAmountO");
                tbl.Columns.Add("WarrantySpareAmountM");
                tbl.Columns.Add("WarrantyFeeAmountO");
                tbl.Columns.Add("WarrantyFeeAmountM");
                tbl.Columns.Add("TotalO");
                tbl.Columns.Add("TotalM");
                tbl.Columns.Add("Status");
                tbl.Columns.Add("StatusString");
                tbl.Columns.Add("Comments");
                tbl.Columns.Add("ProposeFee");

                return tbl;
            }
        }

        private int count = 0, pageSize, firstItemIndex, status = -1;
        private DateTime repairFrom, repairTo;
        private string exchageFrom, exchageTo, engineNum, voucherNo, byDealer, byArea;
        private List<string> byAreas;
        private bool forVerify = false; public bool CalculateAllPageSummary = false;
        public long TotalFeeO, TotalAmountO, TotalSparesO, AllTotalO, TotalFeeM, TotalAmountM, TotalSparesM, AllTotalM;
        public long AllPage_TotalAmountO, AllPage_TotalSparesO, AllPage_AllTotalO, AllPage_TotalFeeM, AllPage_TotalAmountM, AllPage_TotalSparesM, AllPage_AllTotalM;
        public decimal AllPage_TotalFeeO;

        public string DealerCode;
        public string DealerName;
        public DateTime RepairFromDate
        {
            get { return repairFrom; }
            set { repairFrom = value; }
        }
        public DateTime RepairToDate
        {
            get { return repairTo; }
            set { repairTo = value; }
        }
        public string ExchageNoFrom
        {
            get { return exchageFrom; }
            set { exchageFrom = (value != null) ? value.Trim().ToUpper() : null; }
        }
        public DateTime ConfirmedDateFrom { get; set; }
        public DateTime ConfirmedDateTo { get; set; }
        public string ExchageNoTo
        {
            get { return exchageTo; }
            set { exchageTo = (value != null) ? value.Trim().ToUpper() : null; }
        }
        public string EngineNum
        {
            get { return engineNum; }
            set { engineNum = (value != null) ? value.Trim().ToUpper() : null; }
        }
        public string VoucherNo
        {
            get { return voucherNo; }
            set { voucherNo = (value != null) ? value.Trim().ToUpper() : null; }
        }
        public string ByDealer
        {
            get { return byDealer; }
            set { byDealer = (value != null) ? value.Trim().ToUpper() : null; }
        }
        public string ByArea
        {
            get { return byArea; }
            set { byArea = (value != null) ? value.Trim().ToUpper() : null; }
        }
        public List<string> ByAreas
        {
            get { return byAreas; }
            set { byAreas = value; }
        }

        public bool ForVerify
        {
            get { return forVerify; }
            set { forVerify = value; }
        }
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        public int ItemCount
        {
            get { return count; }
            set { count = value; }
        }
        public int FirstItemIndex
        {
            get { return firstItemIndex; }
            set { firstItemIndex = value; }
        }
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
        public int PageCount
        {
            get { return (pageSize == 0) ? 0 : (ItemCount / PageSize) + (((ItemCount % PageSize) > 0) ? 1 : 0); }
        }
        public int PageIndex
        {
            get { return (pageSize == 0) ? 0 : FirstItemIndex / PageSize; }
        }

        #region for exchange confirmation (search at start)

        public int SelectCount(int maximumRows, int startRowIndex, string fromDate, string toDate, int status, string dealerCode)
        {
            return count;
        }
        public int SelectCount(int maximumRows, int startRowIndex, string model, string fromDate, string toDate, int status, string dealerCode)
        {
            return count;
        }
        public IList<Exchangevoucherheader> Select(int maximumRows, int startRowIndex, string fromDate, string toDate, int status, string dealerCode)
        {
            DateTime dtfromDate, dttoDate;
            ArrayList crit = new ArrayList();
            IList<Exchangevoucherheader> list;
            IDao<Exchangevoucherheader, string> dao = DaoFactory.GetDao<Exchangevoucherheader, string>();

            crit.Add(Expression.Like("Id", "D" + UserHelper.DatabaseCode + "%"));
            if (DateTime.TryParse(fromDate, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dtfromDate))
                crit.Add(Expression.Ge("Createddate", dtfromDate));
            if (DateTime.TryParse(toDate, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dttoDate))
                crit.Add(Expression.Le("Createddate", dttoDate.AddDays(1)));

            if (status == (int)ExchangeVoucherStatus.AfterSent)
                crit.Add(Expression.In("Status", ServiceTools.GetAfterSentVoucherStatus()));
            else if (status > 0)
                crit.Add(Expression.In("Status", ServiceTools.GetListVoucherStatusForVerify(status)));

            if (!string.IsNullOrEmpty(dealerCode)) crit.Add(Expression.InsensitiveLike("Dealercode", "%" + dealerCode.Trim().ToUpper() + "%"));

            try
            {
                dao.SetCriteria((ICriterion[])crit.ToArray(typeof(ICriterion)));
                list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
                count = dao.ItemCount;
                HttpContext.Current.Items["ConfirmationExchangeRowCount"] = count;
            }
            catch { return null; }
            return list;
        }
        public IList<Exchangepartheader> Select(int maximumRows, int startRowIndex, string model, string fromDate, string toDate, int status, string dealerCode)
        {
            DateTime dtfromDate, dttoDate;
            ArrayList crit = new ArrayList();
            IList<Exchangepartheader> list;
            IDao<Exchangepartheader, long> dao = DaoFactory.GetDao<Exchangepartheader, long>();

            crit.Add(Expression.In("Areacode", AreaHelper.Area));
            if (DateTime.TryParse(fromDate, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dtfromDate))
                crit.Add(Expression.Ge("Exchangeddate", dtfromDate));
            if (DateTime.TryParse(toDate, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dttoDate))
                crit.Add(Expression.Le("Exchangeddate", dttoDate));
            if (status > 0) crit.Add(Expression.Eq("Status", status));
            else crit.Add(Expression.Ge("Status", 0));

            if (!string.IsNullOrEmpty(dealerCode)) crit.Add(Expression.Eq("Dealercode", dealerCode.Trim().ToUpper()));
            if (!string.IsNullOrEmpty(model)) crit.Add(Expression.Eq("Model", model.Trim().ToUpper()));

            try
            {
                dao.SetCriteria((ICriterion[])crit.ToArray(typeof(ICriterion)));
                list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
                count = dao.ItemCount;
                HttpContext.Current.Items["ConfirmationExchangeRowCount"] = count;
            }
            catch { return null; }
            return list;
        }
        #endregion

        #region for verify exchange

        public static void UpdateSpare(string voucherNo, string spareNumO, string spareNumM, int quantity, long price, long fee, string comment)
        {
            IDao<Exchangepartdetail, long> dao = DaoFactory.GetDao<Exchangepartdetail, long>();
            IDao<Exchangepartheader, long> daoh = DaoFactory.GetDao<Exchangepartheader, long>();

            Exchangepartheader exH = ExchangeVoucherHeaderDataSource.GetExchPartHeader(voucherNo);
            if (exH == null) return;
            Exchangepartdetail exD = GetExchPartDetail(exH.Id, spareNumO);
            if (exD == null) return;

            exD.Partcodem = spareNumM;
            exD.Partqtym = quantity;
            exD.Unitpricem = price;
            exD.Totalfeem = fee;
            exD.Vmepcomment = comment;
            try
            {
                dao.SaveOrUpdate(exD);
                dao.SetCriteria(new ICriterion[] { Expression.Eq("Exchangepartheader.Id", exH.Id) });
                List<Exchangepartdetail> list = dao.GetAll();
                exH.Feeamount = 0;
                foreach (Exchangepartdetail item in list)
                {
                    exH.Feeamount += item.Totalfeem;
                }
                daoh.SaveOrUpdate(exH);
            }
            catch { }
        }
        // for one page in report
        private void CalculateSummary(ExchangeListHeader obj)
        {
            foreach (ExchangeSpare spare in obj.ExchangeSpares.Items)
            {
                TotalSparesO += spare.QuantityO;
                TotalAmountO += spare.WarrantySpareAmountO;
                if (obj.Status == (int)ExchangeVoucherStatus.Approved)
                {
                    TotalSparesM += spare.QuantityM;
                    TotalAmountM += spare.WarrantySpareAmountM;
                    TotalFeeM += spare.WarrantyFeeAmountM;
                }
            }

            TotalFeeO += obj.ProposeFee;
            AllTotalM = TotalFeeM + TotalAmountM;
            AllTotalO = TotalFeeO + TotalAmountO;
        }
        // for all page in report
        private void CalculateTotalSummary()
        {
            ISession session = NHibernateSessionManager.Instance.GetSession();
            List<ICriterion> listCrit = CreateExchangeHeadersQueryCriterions();
            // original
            ICriteria crit = session.CreateCriteria(typeof(Exchangepartdetail)).CreateCriteria("Exchangepartheader");
            foreach (ICriterion item in listCrit)
            {
                crit.Add(item);
            }
            object[] resO = (object[])crit.SetProjection(Projections.ProjectionList()
                                .Add(Projections.Sum("Partqtyo"))
                                .Add(Projections.Sum("Totalfeeo"))
                                .Add(Projections.Sum("PartAmountO"))
                                .Add(Projections.Sum("SubTotalO"))).List()[0];

            // get other version of AllPage_TotalFeeO
            crit = session.CreateCriteria(typeof(Exchangepartheader));
            foreach (ICriterion item in listCrit)
            {
                crit.Add(item);
            }
            resO[1] = crit.SetProjection(Projections.Sum("Proposefeeamount")).UniqueResult();

            // confirmed
            crit = session.CreateCriteria(typeof(Exchangepartdetail)).CreateCriteria("Exchangepartheader");
            foreach (ICriterion item in listCrit)
            {
                crit.Add(item);
            }
            crit.Add(Expression.Eq("Status", (int)ExchangeVoucherStatus.Approved));
            object[] resM = (object[])crit.SetProjection(Projections.ProjectionList()
                                .Add(Projections.Sum("Partqtym"))
                                .Add(Projections.Sum("Totalfeem"))
                                .Add(Projections.Sum("PartAmountM"))
                                .Add(Projections.Sum("SubTotalM"))).List()[0];
            AllPage_TotalFeeO = (resO[1] != null) ? (decimal)resO[1] : 0;
            AllPage_TotalAmountO = (resO[2] != null) ? (long)resO[2] : 0;
            AllPage_TotalSparesO = (resO[0] != null) ? (int)resO[0] : 0;
            AllPage_AllTotalO = (resO[3] != null) ? (long)resO[3] : 0;
            AllPage_AllTotalO = (long)AllPage_TotalFeeO + AllPage_TotalAmountO;

            AllPage_TotalFeeM = (resM[1] != null) ? (long)resM[1] : 0;
            AllPage_TotalAmountM = (resM[2] != null) ? (long)resM[2] : 0;
            AllPage_TotalSparesM = (resM[0] != null) ? (int)resM[0] : 0;
            AllPage_AllTotalM = (resM[3] != null) ? (long)resM[3] : 0;
        }

        public TreeListObjects Select(int maximumRows, int startRowIndex)
        {
            TreeListObjects data = new TreeListObjects();
            int items;
            //IList<Exchangepartheader> listEH = GetExchangeHeaders();
            IList<Exchangepartheader> listEH = GetExchangeHeaders(maximumRows, startRowIndex, out items);

            // dealer info when verify Proposal exchanges
            if (ForVerify)
            {
                if (!string.IsNullOrEmpty(VoucherNo))
                {
                    Exchangevoucherheader exchH = GetExchVoucherHeader(VoucherNo);
                    if (exchH != null)
                    {
                        DealerCode = exchH.Dealercode;
                        DealerName = DealerHelper.GetName(DealerCode);
                    }
                }
                else if ((this.ExchageNoFrom == this.ExchageNoTo) && (listEH.Count > 0))
                {
                    DealerCode = listEH[0].Dealercode;
                    DealerName = DealerHelper.GetName(DealerCode);
                }
            }

            //ItemCount = (listEH == null) ? 0 : listEH.Count;  // paging roi thi thoi
            ItemCount = items;

            PageSize = maximumRows;
            FirstItemIndex = startRowIndex;
            int lastItem = FirstItemIndex + PageSize;
            if (lastItem > ItemCount) lastItem = ItemCount;

            if ((listEH == null) || (listEH.Count == 0)) return null;
            for (int i = 0; i < listEH.Count; i++)
            {
                Exchangepartheader exch = listEH[i];

                #region header section
                ExchangeListHeader header = new ExchangeListHeader();
                header.No = i + 1;
                header.CustomerName = exch.Customer.Fullname;
                header.EngineNumber = exch.Enginenumber;
                header.RepairDate = exch.Exchangeddate;
                header.BuyDate = exch.Purchasedate;
                header.ConfirmedDate = (exch.Status == (int)ExchangeVoucherStatus.Approved) ? exch.Lastprocesseddate : DateTime.MinValue;
                header.ServiceSheetNumber = exch.Serviceheader.Servicesheetnumber;
                header.ExchangeVoucherNumber = exch.Vouchernumber;
                header.Status = exch.Status;
                header.StatusString = ServiceTools.GetNativeVMEPExchangeStatusName(exch.Status);
                header.Km = exch.Kmcount;
                header.Model = exch.Serviceheader.Itemtype;
                header.ProposeFee = (long)exch.Proposefeeamount;
                header.feeO = (long)exch.Proposefeeamount;
                header.feeM = exch.Feeamount;

                #endregion

                #region exchange section
                ArrayList listED = GetExchangeDetails(exch.Id);
                foreach (Exchangepartdetail exchD in listED)
                {
                    Warrantycondition warr;
                    ExchangeSpare exSpare = new ExchangeSpare();

                    warr = WarrantyContent.GetWarrantyCondition(exchD.Partcodeo);
                    if (warr != null) exSpare.LabourO = (long)warr.Labour;
                    warr = WarrantyContent.GetWarrantyCondition(exchD.Partcodem);
                    if (warr != null) exSpare.LabourM = (long)warr.Labour;

                    exSpare.SpareNumberO = exchD.Partcodeo;
                    exSpare.SpareNumberM = exchD.Partcodem;
                    exSpare.UnitPriceO = exchD.Unitpriceo;
                    exSpare.UnitPriceM = exchD.Unitpricem;
                    exSpare.QuantityO = exchD.Partqtyo;
                    exSpare.QuantityM = exchD.Partqtym;
                    exSpare.Comments = exchD.Vmepcomment;
                    exSpare.WarrantyFeeAmountO = exchD.Totalfeeo;
                    exSpare.WarrantyFeeAmountM = exchD.Totalfeem;

                    exSpare.ManPowerM = (exSpare.LabourM == 0) ? 0 : (double)((double)exSpare.WarrantyFeeAmountM / (double)exSpare.LabourM);
                    exSpare.ManPowerO = (exSpare.LabourO == 0) ? 0 : (double)((double)exSpare.WarrantyFeeAmountO / (double)exSpare.LabourO);

                    header.ExchangeSpares.Items.Add(exSpare);
                }
                #endregion

                CalculateSummary(header);

                // add to top list 
                //if ((i >= FirstItemIndex) && (i < lastItem))   // paging roi thi thoi
                {
                    data.Items.Add(header);
                }
            }

            if (CalculateAllPageSummary) CalculateTotalSummary();
            return data;
        }

        private List<ICriterion> CreateExchangeHeadersQueryCriterions()
        {
            List<ICriterion> crit = new List<ICriterion>();
            if (!string.IsNullOrEmpty(VoucherNo)) crit.Add(Expression.Eq("Exchangevoucherheader.Id", VoucherNo));

            if (!string.IsNullOrEmpty(ByDealer)) { crit.Add(Expression.InsensitiveLike("Dealercode", ByDealer)); }
            //else if (!string.IsNullOrEmpty(ByArea)) { crit.Add(Expression.Eq("Areacode", ByArea)); }
            else if (ByAreas.Count > 0) { crit.Add(Expression.In("Areacode", ByAreas)); }

            if (!string.IsNullOrEmpty(ExchageNoFrom)) crit.Add(Expression.Ge("Vouchernumber", ExchageNoFrom));
            if (!string.IsNullOrEmpty(ExchageNoTo)) crit.Add(Expression.Le("Vouchernumber", ExchageNoTo));
            if (!ForVerify)
            {
                if ((RepairFromDate != null) && (RepairFromDate > DateTime.MinValue)) crit.Add(Expression.Ge("Exchangeddate", RepairFromDate));
                if ((RepairToDate != null) && (RepairToDate > DateTime.MinValue)) crit.Add(Expression.Le("Exchangeddate", RepairToDate));
                if (!string.IsNullOrEmpty(EngineNum)) crit.Add(Expression.InsensitiveLike("Enginenumber", EngineNum));
                if (Status >= 0) crit.Add(Expression.Eq("Status", Status));
                else
                {
                    crit.Add(Expression.Ge("Status", 0));
                    //crit.Add(Expression.Not(Expression.Eq("Status", (int)ExchangeVoucherStatus.Canceled)));
                }

                bool conFrom = (ConfirmedDateFrom != null) && (ConfirmedDateFrom > DateTime.MinValue);
                bool conTo = (ConfirmedDateTo != null) && (ConfirmedDateTo > DateTime.MinValue);
                if (conFrom || conTo)
                {
                    if (conTo) crit.Add(Expression.Le("Lastprocesseddate", ConfirmedDateTo));
                    if (conFrom) crit.Add(Expression.Ge("Lastprocesseddate", ConfirmedDateFrom));
                }
            }

            return crit;
        }
        private IList<Exchangepartheader> GetExchangeHeaders(int maximumRows, int startRowIndex, out int itemsCount)
        {
            int page;

            IDao<Exchangepartheader, long> dao = DaoFactory.GetDao<Exchangepartheader, long>();

            dao.SetCriteria(CreateExchangeHeadersQueryCriterions().ToArray());
            itemsCount = dao.GetCount();

            page = (startRowIndex >= itemsCount) ? 0 : (startRowIndex / maximumRows);
            return dao.GetPaged(page, maximumRows);
        }

        public ArrayList GetExchangeDetails(long exchangeId)
        {
            NHibernate.ISession session = NHibernateSessionManager.Instance.GetSession();
            return (ArrayList)session.CreateCriteria(typeof(Exchangepartdetail)).Add(Expression.Eq("Exchangepartheader.Id", exchangeId)).List();
        }
        public Exchangevoucherheader GetExchVoucherHeader(string voucherNo)
        {
            IDao<Exchangevoucherheader, string> dao = DaoFactory.GetDao<Exchangevoucherheader, string>();
            Exchangevoucherheader exchH = dao.GetById(voucherNo, true);
            return exchH;
        }

        public static Exchangepartheader GetExchPartHeader(string vNum)
        {
            NHibernate.ISession session = NHibernateSessionManager.Instance.GetSession();
            return session.CreateCriteria(typeof(Exchangepartheader))
                .Add(Expression.Ge("Status", 0))
                .Add(Expression.Eq("Vouchernumber", vNum.Trim().ToUpper()))
                .UniqueResult<Exchangepartheader>();
        }
        public static IList<Exchangepartheader> GetProposalExchPartHeaders(string vNum)
        {
            NHibernate.ISession session = NHibernateSessionManager.Instance.GetSession();
            return session.CreateCriteria(typeof(Exchangepartheader))
                .Add(Expression.Ge("Status", 0))
                .Add(Expression.Eq("Exchangevoucherheader.Id", vNum.Trim().ToUpper()))
                .List<Exchangepartheader>();
        }
        public static Exchangepartdetail GetExchPartDetail(long vId, string code)
        {
            NHibernate.ISession session = NHibernateSessionManager.Instance.GetSession();
            return session.CreateCriteria(typeof(Exchangepartdetail))
                .Add(Expression.Eq("Exchangepartheader.Id", vId))
                .Add(Expression.Eq("Partcodeo", code.Trim().ToUpper()))
                .UniqueResult<Exchangepartdetail>();
        }
        public static VerifyExchangeErrorCode ChangeExchangeVoucherStatus(string voucherNo, ExchangeVoucherStatus status)
        {
            VerifyExchangeErrorCode result = VerifyExchangeErrorCode.OK;

            NHibernate.ISession session = NHibernateSessionManager.Instance.GetSession();
            Exchangepartheader exch = session.CreateCriteria(typeof(Exchangepartheader))
                                        .Add(Expression.Ge("Status", 0))
                                        .Add(Expression.Eq("Vouchernumber", voucherNo.Trim().ToUpper()))
                                        .UniqueResult<Exchangepartheader>();
            if (exch == null)
            {
                result = VerifyExchangeErrorCode.ExchangePartHeaderNotFound;
            }
            else
            {
                result = ServiceTools.CanVerifyExchangePartHeader(exch, status);
            }

            if (result == VerifyExchangeErrorCode.OK)
            {
                // calculate status of "Phieu xin doi"
                IList<Exchangepartheader> exchs = null;
                bool verifyAll = true, approvedAll = true, rejectAll = true;
                if (exch.Exchangevoucherheader != null)
                {
                    exchs = session.CreateCriteria(typeof(Exchangepartheader))
                                        .Add(Expression.Eq("Exchangevoucherheader.Id", exch.Exchangevoucherheader.Id))
                                        .Add(Expression.Ge("Status", 0))
                                        .List<Exchangepartheader>();
                    foreach (Exchangepartheader item in exchs)
                    {
                        if ((item.Id != exch.Id) && ((item.Status != (int)ExchangeVoucherStatus.Approved) && (item.Status != (int)ExchangeVoucherStatus.Reject))) verifyAll = false;
                        if ((item.Id != exch.Id) && (item.Status != (int)ExchangeVoucherStatus.Approved)) approvedAll = false;
                        if ((item.Id != exch.Id) && (item.Status != (int)ExchangeVoucherStatus.Reject)) rejectAll = false;
                    }
                }
                verifyAll = verifyAll && ((status == ExchangeVoucherStatus.Approved) || (status == ExchangeVoucherStatus.Reject));
                approvedAll = approvedAll && (status == ExchangeVoucherStatus.Approved);
                rejectAll = rejectAll && (status == ExchangeVoucherStatus.Reject);

                try
                {
                    exch.Status = (int)status;
                    if (exch.Exchangevoucherheader != null)
                    {
                        if (approvedAll)
                            exch.Exchangevoucherheader.Status = (int)ExchangeVoucherStatus.Approved;
                        else if (rejectAll)
                            exch.Exchangevoucherheader.Status = (int)ExchangeVoucherStatus.Reject;
                        else if (verifyAll)
                            exch.Exchangevoucherheader.Status = (int)ExchangeVoucherStatus.Verified;
                        else
                            exch.Exchangevoucherheader.Status = (int)ExchangeVoucherStatus.Verifing;
                    }
                    exch.Lastprocesseddate = DateTime.Now;
                    exch.Exchangevoucherheader.Lastprocesseddate = DateTime.Now;
                    session.SaveOrUpdate(exch);
                }
                catch { }
                result = VerifyExchangeErrorCode.OK;
            }
            return result;
        }
        public static VerifyExchangeErrorCode ChangeExchangeVoucherStatusD(VDMS.I.Entity.ExchangePartHeader exch, ExchangeVoucherStatus status)
        {
            VerifyExchangeErrorCode result = VerifyExchangeErrorCode.OK;
            var dc = VDMS.II.Linq.DCFactory.GetDataContext<VDMS.I.Linq.ServiceDataContext>();

            if (exch == null)
            {
                result = VerifyExchangeErrorCode.ExchangePartHeaderNotFound;
            }
            else
            {
                result = ServiceTools.CanVerifyExchangePartHeaderD(exch, status);
            }

            if (result == VerifyExchangeErrorCode.OK)
            {
                exch.Status = (int)status;
                exch.LastProcessedDate = DateTime.Now;

                // calculate status of "Phieu xin doi"
                var pxd = dc.ExchangeVoucherHeaders.SingleOrDefault(h => h.FinalVoucherNumber == exch.FinalVoucherNumber);
                if (pxd != null)
                {
                    var list = dc.ExchangePartHeaders.Where(e => e.FinalVoucherNumber == exch.FinalVoucherNumber);
                    int allCount = list.Count();
                    int appCount = list.Where(e => e.Status == (int)ExchangeVoucherStatus.Approved).Count();
                    int rejCount = list.Where(e => e.Status == (int)ExchangeVoucherStatus.Reject).Count();

                    if (allCount == appCount)
                        pxd.Status = (int)ExchangeVoucherStatus.Approved;
                    else if (allCount == rejCount)
                        pxd.Status = (int)ExchangeVoucherStatus.Reject;
                    else if (allCount == rejCount + appCount)
                        pxd.Status = (int)ExchangeVoucherStatus.Verified;
                    else
                        pxd.Status = (int)ExchangeVoucherStatus.Verifing;

                    pxd.LastProcessedDate = DateTime.Now;
                }

                result = VerifyExchangeErrorCode.OK;
            }
            return result;
        }

        #endregion

        #endregion
    }
}
