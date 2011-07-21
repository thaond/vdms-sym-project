using System;
using System.Collections;
using System.Data;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;

namespace VDMS.I.Service
{
    public enum AddExchangeErrorCode : int
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
        InvalidServiceType = 9
    }

    public partial class AddExchange
    {
        public static DataTable SpareListOnServiceSchema
        {
            get
            {
                DataTable tbl = new DataTable();
                tbl.Columns.Add("ItemId");
                tbl.Columns.Add("NoWarranty");
                tbl.Columns.Add("SpareNo");
                tbl.Columns.Add("SpareNumber");
                tbl.Columns.Add("SpareNameEn");
                tbl.Columns.Add("SpareNameVn");
                tbl.Columns.Add("SpareName");
                tbl.Columns.Add("Quantity");
                tbl.Columns.Add("SpareCost");
                tbl.Columns.Add("BrokenCode");
                tbl.Columns.Add("SpareAmount");
                tbl.Columns.Add("SerialNumber");
                tbl.Columns.Add("ManPower");
                tbl.Columns.Add("Labour");
                tbl.Columns.Add("FeeAmount");
                tbl.Columns.Add("WarrantyTime");
                tbl.Columns.Add("WarrantyLength");
                return tbl;
            }
        }
        private static bool ExchangeNumberExist(string number)
        {
            IDao<Exchangepartheader, long> dao = DaoFactory.GetDao<Exchangepartheader, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Vouchernumber", number.Trim().ToUpper()) });
            IList list = dao.GetAll();
            if ((list != null) && (list.Count > 0)) return true;
            else return false;
        }
        public static string GenExchangeNumber(string DealerCode)
        {
            return GenExchangeNumber(false, DealerCode);
        }
        public static string GenExchangeNumber(bool temp, string DealerCode)
        {
            DateTime dt = DateTime.Now;
            IDao<Exchangepartheader, long> dao = DaoFactory.GetDao<Exchangepartheader, long>();
            string num = (temp ? "T_" : "") + "P" + DealerCode + dt.ToString("yyMMdd");

            dao.SetCriteria(new ICriterion[] { Expression.Like("Vouchernumber", num + "%") });
            int count = dao.GetAll().Count + 1;

            while (ExchangeNumberExist(num + count.ToString().PadLeft(4, '0'))) { count++; }

            num += count.ToString().PadLeft(4, '0');
            return num;
        }

        public static Iteminstance GetItemInstance(string engineNumber)
        {
            IDao<Iteminstance, long> dao = DaoFactory.GetDao<Iteminstance, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Enginenumber", engineNumber.Trim().ToUpper()) });
            IList list = dao.GetAll();
            if (list.Count != 1) return null;
            return (Iteminstance)list[0];
        }

        public static WarrantyContentErrorCode SaveExchHeader(string area, string dealer, Exchangepartheader exchH, Serviceheader serH, long totalFee)
        {
            return SaveExchHeader(ServiceStatus.Done, area, dealer, exchH, serH, totalFee);
        }
        public static WarrantyContentErrorCode SaveExchHeader(Exchangepartheader exchH, Serviceheader serH)
        {
            return SaveExchHeader((ServiceStatus)exchH.Status, null, null, exchH, serH, -1);
        }
        public static WarrantyContentErrorCode SaveExchHeader(ServiceStatus status, string area, string dealer, Exchangepartheader exchH, Serviceheader serH, long totalFee)
        {
            if (exchH == null) return WarrantyContentErrorCode.SaveExchHeaderFailed;

            IDao<Exchangepartheader, long> dao = DaoFactory.GetDao<Exchangepartheader, long>();
            //Exchangepartheader exchH = dao.GetById(eH.Id, true);
            //if (exchH == null) exchH = eH;

            try
            {
                if (serH != null) exchH.Serviceheader = serH;
                exchH.Customer = serH.Customer;
                exchH.Enginenumber = serH.Enginenumber;
                exchH.Framenumber = serH.Framenumber;
                if (!string.IsNullOrEmpty(area)) exchH.Areacode = area;
                if (!string.IsNullOrEmpty(dealer)) exchH.Dealercode = dealer;
                exchH.Status = (status != ServiceStatus.Temp) ? (int)ExchangeVoucherStatus.New : (int)ExchangeVoucherStatus.Temp;
                exchH.Vouchernumber = GenExchangeNumber(status == ServiceStatus.Temp, exchH.Dealercode);
                if (totalFee >= 0)
                {
                    exchH.Feeamount = totalFee;
                }
                exchH.Proposefeeamount = exchH.Feeamount;
                dao.SaveOrUpdateCopy(exchH);
            }
            catch { return WarrantyContentErrorCode.SaveExchHeaderFailed; }
            //exchH.Vouchernumber = exchH.Vouchernumber;

            return WarrantyContentErrorCode.OK;
        }

        public static WarrantyContentErrorCode SaveExchDetail(DataRow item, Exchangepartheader exchH)
        {
            IDao<Exchangepartdetail, long> dao = DaoFactory.GetDao<Exchangepartdetail, long>();
            IDao<Broken, long> daoB = DaoFactory.GetDao<Broken, long>();
            return SaveExchDetail(item, exchH, dao, daoB);
        }
        public static WarrantyContentErrorCode SaveExchDetail(DataRow item, Exchangepartheader exchH, IDao<Exchangepartdetail, long> dao, IDao<Broken, long> daoB)
        {
            IList list;
            long eid;
            long.TryParse(item["ItemId"].ToString(), out eid);

            Exchangepartdetail exchD = dao.GetById(eid, true);
            if (exchD == null) exchD = new Exchangepartdetail();

            if (exchH == null) return WarrantyContentErrorCode.SaveExchDetailFailed;
            if (string.IsNullOrEmpty(item["BrokenCode"].ToString())) return WarrantyContentErrorCode.SaveExchDetailFailed;
            if (string.IsNullOrEmpty(item["SpareNumber"].ToString())) return WarrantyContentErrorCode.SaveExchDetailFailed;
            if (string.IsNullOrEmpty(item["Quantity"].ToString())) return WarrantyContentErrorCode.SaveExchDetailFailed;
            //if (string.IsNullOrEmpty(item["SerialNumber"].ToString())) return WarrantyContentErrorCode.SaveExchDetailFailed;
            if (string.IsNullOrEmpty(item["SpareCost"].ToString())) return WarrantyContentErrorCode.SaveExchDetailFailed;

            // get broken object
            daoB.SetCriteria(new ICriterion[] { Expression.Eq("Brokencode", item["BrokenCode"].ToString()) });
            list = daoB.GetAll();
            if (list.Count != 1) return WarrantyContentErrorCode.SaveExchDetailFailed;
            Broken broken = (Broken)list[0];
            long price; long.TryParse(item["SpareCost"].ToString(), out price);

            exchD.Broken = broken;
            exchD.Exchangepartheader = exchH;
            exchD.Partcodem = item["SpareNumber"].ToString();
            exchD.Partcodeo = item["SpareNumber"].ToString();
            exchD.Partqtym = Convert.ToInt32(item["Quantity"].ToString());
            exchD.Partqtyo = exchD.Partqtym;
            //exchD.Serialnumber = ((item["SerialNumber"] == null) || (item["SerialNumber"].ToString().Trim() == "")) ? " " : item["SerialNumber"].ToString();
            exchD.Serialnumber = item["SerialNumber"].ToString();
            exchD.Unitpricem = price;
            exchD.Unitpriceo = price;
            long.TryParse(item["FeeAmount"].ToString(), out price);
            exchD.Totalfeem = price;
            exchD.Totalfeeo = price;

            try { dao.SaveOrUpdate(exchD); }
            catch { return WarrantyContentErrorCode.SaveDetailFailed; }
            return WarrantyContentErrorCode.OK;
        }
        public static WarrantyContentErrorCode SaveExchDetails(DataTable exchDetail, Exchangepartheader exchH)
        {
            IDao<Exchangepartdetail, long> dao = DaoFactory.GetDao<Exchangepartdetail, long>();
            IDao<Broken, long> daoB = DaoFactory.GetDao<Broken, long>();

            foreach (DataRow row in exchDetail.Rows)
            {
                if (SaveExchDetail(row, exchH, dao, daoB) != WarrantyContentErrorCode.OK) return WarrantyContentErrorCode.SaveDetailFailed;
            }
            return WarrantyContentErrorCode.OK;
        }
        public static WarrantyContentErrorCode SaveExchDetails(IList exDetail, Exchangepartheader eH)
        {
            IDao<Exchangepartdetail, long> dao = DaoFactory.GetDao<Exchangepartdetail, long>();
            try
            {
                foreach (PCVItem item in exDetail)
                {
                    if ((item.State == ServiceItemState.Transient) || item.HasModified || (item.Exchangepartheader == null) || (item.Exchangepartheader.Id != eH.Id))
                    {
                        Exchangepartdetail ed = item.Base();
                        dao.SaveOrUpdateCopy(ed);
                    }
                }
            }
            catch { return WarrantyContentErrorCode.SaveDetailFailed; }
            return WarrantyContentErrorCode.OK;
        }
    }

}