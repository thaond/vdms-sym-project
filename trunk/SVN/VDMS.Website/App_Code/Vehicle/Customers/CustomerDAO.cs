using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.I.Entity;
using VDMS.II.Linq;
using VDMS.I.Linq;
using VDMS.Common.Utils;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using VDMS.Helper;

namespace VDMS.I.Vehicle
{
    public class CustomerDAO
    {
        int _custCount;
        public object CountCustomers(string cusClass, string engineNumber, string NumberPlate,
                string InvoiceNumber, string fromDate, string toDate, string AreaCode, string DealerCode,
                string IdentifyNumber, string Fullname, string Address, string Precinct, string DistrictId,
                string ProvinceId, string Model)
        {
            return _custCount;
        }

        public static string BuildFindCustomersQuery(bool getIdOnly, string engineNumber, string NumberPlate,
                string InvoiceNumber, DateTime saleFrom, DateTime saleTo, string AreaCode, string DealerCode,
                string IdentifyNumber, string Fullname, string Address, string Precinct, string DistrictId,
                string ProvinceId, string Model)
        {
            #region build sql string

            StringBuilder cusAll = new StringBuilder(string.Format("select {0} from SYM_CUSTOMER where 1=1", getIdOnly ? "CUSTOMERID, CUSTOMERTYPE" : "SYM_CUSTOMER.*, NVL(TEL, MOBILE) as PHONE, NVL2(TEL, NVL2(MOBILE, TEL || ' | ' || MOBILE, TEL), MOBILE) as ALLPHONE"));
            if (!string.IsNullOrEmpty(IdentifyNumber)) cusAll.Append(string.Format(" and IDENTIFYNUMBER = '{0}'", IdentifyNumber));
            if (!string.IsNullOrEmpty(Fullname)) cusAll.Append(string.Format(" and Lower(FULLNAME) like '%{0}%'", Fullname.ToLower()));
            if (!string.IsNullOrEmpty(Address)) cusAll.Append(string.Format(" and Lower(ADDRESS) like '%{0}%'", Address.ToLower()));
            if (!string.IsNullOrEmpty(Precinct)) cusAll.Append(string.Format(" and Lower(PRECINCT) like '%{0}%'", Precinct.ToLower()));
            if (!string.IsNullOrEmpty(DistrictId)) cusAll.Append(string.Format(" and DistrictId = '{0}'", DistrictId));
            if (!string.IsNullOrEmpty(ProvinceId)) cusAll.Append(string.Format(" and ProvinceId = '{0}'", ProvinceId));

            StringBuilder invoices = new StringBuilder("select SALE_INVOICE.ENGINENUMBER, SALE_INVOICE.SELLDATE as BuyDate, SALE_INVOICE.CUSTOMERID, DATA_ITEMINSTANCE.ITEMTYPE, DATA_ITEM.COLORCODE as ItemColor from SALE_INVOICE");
            //if (!string.IsNullOrEmpty(NumberPlate)) invoices.Append(" join SALE_SELLITEM on SALE_INVOICE.SELLITEMID = SALE_SELLITEM.SELLITEMID");
            if (!string.IsNullOrEmpty(AreaCode)) invoices.Append(" join V2_P_DEALER on V2_P_DEALER.DEALER_CODE = SALE_INVOICE.DEALERCODE");
            if (!string.IsNullOrEmpty(NumberPlate)) invoices.Append(" join SALE_SELLITEM on SALE_SELLITEM.SELLITEMID = SALE_INVOICE.SELLITEMID");
            invoices.Append(" join DATA_ITEMINSTANCE on DATA_ITEMINSTANCE.ITEMINSTANCEID = SALE_INVOICE.ITEMINSTANCEID");
            invoices.Append(" join DATA_ITEM on DATA_ITEMINSTANCE.ITEMCODE = DATA_ITEM.ITEMCODE");
            invoices.Append(" where 1=1");
            if (!string.IsNullOrEmpty(engineNumber)) invoices.Append(string.Format(" and SALE_INVOICE.engineNumber like '%{0}%'", engineNumber));
            if (!string.IsNullOrEmpty(NumberPlate)) invoices.Append(string.Format(" and SALE_SELLITEM.NUMBERPLATE = '{0}'", NumberPlate));
            if (!string.IsNullOrEmpty(InvoiceNumber)) invoices.Append(string.Format(" and SALE_INVOICE.INVOICENUMBER = '{0}'", InvoiceNumber));
            if (saleFrom > DateTime.MinValue) invoices.Append(string.Format(" and SALE_INVOICE.SELLDATE >= to_date('{0}', 'yyyy/mm/dd')", saleFrom.ToString("yyyy/MM/dd")));
            if (saleTo > DateTime.MinValue) invoices.Append(string.Format(" and SALE_INVOICE.SELLDATE <= to_date('{0}', 'yyyy/mm/dd')", saleTo.AddDays(1).AddSeconds(-1).ToString("yyyy/MM/dd")));
            if (!string.IsNullOrEmpty(DealerCode)) invoices.Append(string.Format(" and SALE_INVOICE.DEALERCODE = '{0}'", DealerCode));
            if (!string.IsNullOrEmpty(AreaCode)) invoices.Append(string.Format(" and V2_P_DEALER.AREA_CODE = '{0}'", AreaCode));
            if (!string.IsNullOrEmpty(Model)) invoices.Append(string.Format(" and DATA_ITEMINSTANCE.ITEMTYPE like '{0}%'", Model));

            StringBuilder warranties = new StringBuilder("select SER_WARRANTYINFO.ENGINENUMBER, SER_WARRANTYINFO.PURCHASEDATE as BuyDate, SER_WARRANTYINFO.CUSTOMERID, NVL(DATA_ITEM.ITEMTYPE, SER_WARRANTYINFO.ITEMCODE) as ITEMTYPE, DATA_ITEM.COLORCODE as ItemColor from SER_WARRANTYINFO");
            //StringBuilder warranties = new StringBuilder("select SER_WARRANTYINFO.ENGINENUMBER, SER_WARRANTYINFO.PURCHASEDATE as BuyDate, SER_WARRANTYINFO.CUSTOMERID, SER_WARRANTYINFO.ITEMCODE as ITEMTYPE, SER_WARRANTYINFO.COLOR as ItemColor from SER_WARRANTYINFO");
            if (!string.IsNullOrEmpty(AreaCode)) warranties.Append(" join V2_P_DEALER on V2_P_DEALER.DEALER_CODE = SER_WARRANTYINFO.SELLDEALERCODE");
            warranties.Append(" left join DATA_ITEM on DATA_ITEM.ITEMCODE = SER_WARRANTYINFO.ITEMCODE");
            warranties.Append(" where 1=1");
            if (!string.IsNullOrEmpty(engineNumber)) warranties.Append(string.Format(" and SER_WARRANTYINFO.ENGINENUMBER like '%{0}%'", engineNumber));
            if (saleFrom > DateTime.MinValue) warranties.Append(string.Format(" and SER_WARRANTYINFO.PURCHASEDATE >= to_date('{0}', 'yyyy/mm/dd')", saleFrom.ToString("yyyy/MM/dd")));
            if (saleTo > DateTime.MinValue) warranties.Append(string.Format(" and SER_WARRANTYINFO.PURCHASEDATE <= to_date('{0}', 'yyyy/mm/dd')", saleTo.AddDays(1).AddSeconds(-1).ToString("yyyy/MM/dd")));
            if (!string.IsNullOrEmpty(DealerCode)) warranties.Append(string.Format(" and SER_WARRANTYINFO.SELLDEALERCODE = '{0}'", DealerCode));
            if (!string.IsNullOrEmpty(AreaCode)) warranties.Append(string.Format(" and V2_P_DEALER.AREA_CODE = '{0}'", AreaCode));
            if (!string.IsNullOrEmpty(Model)) warranties.Append(string.Format(" and (DATA_ITEM.ITEMTYPE = '{0}' or (DATA_ITEM.ITEMTYPE IS NULL and SER_WARRANTYINFO.ITEMCODE like '{0}%'))", Model));
            //if (!string.IsNullOrEmpty(Model)) warranties.Append(string.Format(" and SER_WARRANTYINFO.ITEMCODE like '{0}%'", Model));

            StringBuilder services = new StringBuilder("select SER_SERVICEHEADER.ENGINENUMBER, SER_SERVICEHEADER.PURCHASEDATE as BuyDate, SER_SERVICEHEADER.CUSTOMERID, ITEMTYPE, COLORCODE as ItemColor from SER_SERVICEHEADER");
            if (!string.IsNullOrEmpty(AreaCode)) services.Append(" join V2_P_DEALER on V2_P_DEALER.DEALER_CODE = SER_SERVICEHEADER.DEALERCODE");
            services.Append(" where 1=1");
            if (!string.IsNullOrEmpty(engineNumber)) services.Append(string.Format(" and SER_SERVICEHEADER.ENGINENUMBER like '%{0}%'", engineNumber));
            if (saleFrom > DateTime.MinValue) services.Append(string.Format(" and SER_SERVICEHEADER.PURCHASEDATE >= to_date('{0}', 'yyyy/mm/dd')", saleFrom.ToString("yyyy/MM/dd")));
            if (saleTo > DateTime.MinValue) services.Append(string.Format(" and SER_SERVICEHEADER.PURCHASEDATE <= to_date('{0}', 'yyyy/mm/dd')", saleTo.AddDays(1).AddSeconds(-1).ToString("yyyy/MM/dd")));
            if (!string.IsNullOrEmpty(DealerCode)) services.Append(string.Format(" and SER_SERVICEHEADER.DEALERCODE = '{0}'", DealerCode));
            if (!string.IsNullOrEmpty(AreaCode)) services.Append(string.Format(" and V2_P_DEALER.AREA_CODE = '{0}'", AreaCode));
            if (!string.IsNullOrEmpty(Model)) services.Append(string.Format(" and ITEMTYPE = '{0}'", Model));

            string sql = string.Format(@"select * from (
                    select IV.ENGINENUMBER,IV.BuyDate,IV.ITEMTYPE,IV.ItemColor, T1.*, 'SL' as CusClass from ({1}) IV 
                    join ({0}) T1 on IV.CUSTOMERID = T1.CUSTOMERID
                    where CUSTOMERTYPE = {4}
                    UNION
                    select SV.ENGINENUMBER ,SV.BuyDate ,SV.ITEMTYPE ,SV.ItemColor , T2.*, 'SV' as CusClass from ({2}) SV 
                    join ({0}) T2 on SV.CUSTOMERID = T2.CUSTOMERID
                    where CUSTOMERTYPE = {5}
                    UNION
                    select WI.ENGINENUMBER ,WI.BuyDate ,WI.ITEMTYPE ,WI.ItemColor , T3.*, 'WI' as CusClass from ({3}) WI 
                    join ({0}) T3 on WI.CUSTOMERID = T3.CUSTOMERID
                    where CUSTOMERTYPE = {6}
                )",
                cusAll,
                invoices,
                services,
                warranties,
                (int)CusType.Sale,
                (int)CusType.Service,
                (int)CusType.WarrantyInfo
                );
            return sql;

            #endregion
        }
        public object FindCustomers(string cusClass, string engineNumber, string NumberPlate,
                string InvoiceNumber, string fromDate, string toDate, string AreaCode, string DealerCode,
                string IdentifyNumber, string Fullname, string Address, string Precinct, string DistrictId,
                string ProvinceId, string Model)
        {
            return FindCustomers(-1, -1, cusClass, engineNumber, NumberPlate, InvoiceNumber, fromDate, toDate, AreaCode, DealerCode, IdentifyNumber, Fullname, Address, Precinct, DistrictId, ProvinceId, Model);
        }
        public object FindCustomers(int maximumRows, int startRowIndex, string cusClass, string engineNumber, string NumberPlate,
                string InvoiceNumber, string fromDate, string toDate, string AreaCode, string DealerCode,
                string IdentifyNumber, string Fullname, string Address, string Precinct, string DistrictId,
                string ProvinceId, string Model)
        {
            DateTime saleFrom = DataFormat.DateFromString(fromDate);
            DateTime saleTo = DataFormat.DateFromString(toDate);
            //saleTo =  saleTo.AddDays(1).AddSeconds(-1);
            bool pagingEnabled = (startRowIndex >= 0) && (maximumRows > 0);
            if (!string.IsNullOrEmpty(engineNumber)) engineNumber = engineNumber.Trim();

            if (string.IsNullOrEmpty(cusClass)) // all class
            {
                // count customers
                string sqlSel = BuildFindCustomersQuery(true, engineNumber, NumberPlate, InvoiceNumber, saleFrom, saleTo, AreaCode, DealerCode, IdentifyNumber, Fullname, Address, Precinct, DistrictId, ProvinceId, Model);
                string sqlCount = string.Format("select count(*) from ({0})", sqlSel);
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(sqlCount);
                _custCount = (int)(decimal)db.ExecuteScalar(dbCommand);

                if (pagingEnabled)
                {
                    //sqlSel = BuildFindCustomersQuery(true, engineNumber, NumberPlate, InvoiceNumber, saleFrom, saleTo, AreaCode, DealerCode, IdentifyNumber, Fullname, Address, Precinct, DistrictId, ProvinceId, Model);
                    sqlSel = string.Format("select * from (select T.*, rownum as rowindex from ({0}) T) where rowindex between {1} and {2} order by ENGINENUMBER", sqlSel, startRowIndex + 1, startRowIndex + maximumRows);
                }
                else if (_custCount > 999) // cannot separate build data into 2 step
                {
                    sqlSel = BuildFindCustomersQuery(false, engineNumber, NumberPlate, InvoiceNumber, saleFrom, saleTo, AreaCode, DealerCode, IdentifyNumber, Fullname, Address, Precinct, DistrictId, ProvinceId, Model);
                    sqlSel = string.Format("{0} order by ENGINENUMBER", sqlSel);

                    dbCommand = db.GetSqlStringCommand(sqlSel);
                    DataSet data = db.ExecuteDataSet(dbCommand);
                    return data;
                }
                else
                {
                    sqlSel = string.Format("{0} order by ENGINENUMBER", sqlSel);
                }

                // data with CUSTOMERID only
                dbCommand = db.GetSqlStringCommand(sqlSel);
                DataSet preData = db.ExecuteDataSet(dbCommand);

                // get CUSTOMER data
                var dc = DCFactory.GetDataContext<ServiceDataContext>();
                List<long> cusIds = new List<long>();
                foreach (DataRow row in preData.Tables[0].Rows)
                {
                    cusIds.Add((long)(decimal)row["CUSTOMERID"]);
                }
                List<Customer> cuses = dc.Customers.Where(c => cusIds.Contains(c.CustomerId)).ToList();

                // build result data
                List<CustomerInfo> res = new List<CustomerInfo>();
                foreach (DataRow row in preData.Tables[0].Rows)
                {
                    object cusid = row["CUSTOMERID"];
                    var cus = (cusid == null) ? new Customer() : cuses.SingleOrDefault(c => c.CustomerId == (long)(decimal)cusid);

                    res.Add(new CustomerInfo(cus)
                            {
                                BuyDate = row["BuyDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["BuyDate"],
                                EngineNumber = row["EngineNumber"] == DBNull.Value ? "" : (string)row["EngineNumber"],
                                ItemColor = row["ItemColor"] == DBNull.Value ? "" : (string)row["ItemColor"],
                                ItemType = row["ItemType"] == DBNull.Value ? "" : (string)row["ItemType"],
                                CusClass = row["CusClass"] == DBNull.Value ? "" : (string)row["CusClass"],
                            });
                }
                return res;
            }
            else
            {
                var dc = DCFactory.GetDataContext<ServiceDataContext>();
                switch (cusClass)
                {
                    case "SL":
                        #region // sale

                        var query = dc.Invoices.OrderBy(i => i.EngineNumber).Where(i => i.CustomerId > 0 && i.Customer.CustomerType == (int)CusType.Sale);
                        if (!string.IsNullOrEmpty(engineNumber)) query = query.Where(i => i.EngineNumber.Contains(engineNumber.ToUpper()));
                        if (!string.IsNullOrEmpty(NumberPlate)) query = query.Where(i => i.SaleSellitem.NumberPlate.ToUpper() == NumberPlate.ToUpper());
                        if (!string.IsNullOrEmpty(InvoiceNumber)) query = query.Where(i => i.InvoiceNumber == InvoiceNumber.ToUpper());
                        if (saleFrom > DateTime.MinValue) query = query.Where(i => i.SellDate >= saleFrom);
                        if (saleTo > DateTime.MinValue) query = query.Where(i => i.SellDate < saleTo.AddDays(1));
                        if (!string.IsNullOrEmpty(DealerCode))
                        {
                            query = query.Where(i => i.DealerCode == DealerCode);
                        }
                        else if (!string.IsNullOrEmpty(AreaCode))
                        {
                            var pdc = DCFactory.GetDataContext<PartDataContext>();
                            IList<string> dealers = pdc.Dealers.Where(d => d.AreaCode == AreaCode).Select(d => d.DealerCode).ToList();
                            query = query.Where(i => dealers.Contains(i.DealerCode));
                        }
                        if (!string.IsNullOrEmpty(IdentifyNumber)) query = query.Where(i => i.Customer.IdentifyNumber == IdentifyNumber);
                        if (!string.IsNullOrEmpty(Fullname)) query = query.Where(i => i.Customer.FullName.ToLower().Contains(Fullname.ToLower()));
                        if (!string.IsNullOrEmpty(Address)) query = query.Where(i => i.Customer.Address.ToLower().Contains(Address.ToLower()));
                        if (!string.IsNullOrEmpty(Precinct)) query = query.Where(i => i.Customer.Precinct.Contains(Precinct));
                        if (!string.IsNullOrEmpty(DistrictId)) query = query.Where(i => i.Customer.DistrictId.Contains(DistrictId));
                        if (!string.IsNullOrEmpty(ProvinceId)) query = query.Where(i => i.Customer.ProvinceId.Contains(ProvinceId));
                        if (!string.IsNullOrEmpty(Model)) query = query.Where(i => i.DataIteminstance.ItemType == Model);

                        _custCount = query.Count();
                        if (pagingEnabled) query = query.Skip(startRowIndex).Take(maximumRows);
                        return query.Select(i => new CustomerInfo(i));

                        #endregion
                    case "SV":
                        #region // service

                        var ser = dc.ServiceHeaders.Where(i => i.CustomerId > 0 && i.Customer.CustomerType == (int)CusType.Service)
                                    .GroupBy(i => new { i.CustomerId, i.EngineNumber },
                                            (g, rs) => new { ServiceHeaderId = rs.Max(s => s.ServiceHeaderId) });
                        var query2 = ser.Join(dc.ServiceHeaders, s1 => s1.ServiceHeaderId, s2 => s2.ServiceHeaderId, (s1, s2) => s2)
                                        .OrderBy(i => i.EngineNumber).AsQueryable();

                        if (!string.IsNullOrEmpty(engineNumber)) query2 = query2.Where(i => i.EngineNumber.Contains(engineNumber.ToUpper()));
                        if (!string.IsNullOrEmpty(NumberPlate)) query2 = query2.Where(i => i.NumberPlate.ToUpper() == NumberPlate.ToUpper());
                        if (saleFrom > DateTime.MinValue) query2 = query2.Where(i => i.PurchaseDate >= saleFrom);
                        if (saleTo > DateTime.MinValue) query2 = query2.Where(i => i.PurchaseDate < saleTo.AddDays(1));
                        if (!string.IsNullOrEmpty(DealerCode))
                        {
                            query2 = query2.Where(i => i.DealerCode == DealerCode);
                        }
                        else if (!string.IsNullOrEmpty(AreaCode))
                        {
                            var pdc = DCFactory.GetDataContext<PartDataContext>();
                            IList<string> dealers = pdc.Dealers.Where(d => d.AreaCode == AreaCode).Select(d => d.DealerCode).ToList();
                            query2 = query2.Where(i => dealers.Contains(i.DealerCode));
                        }
                        if (!string.IsNullOrEmpty(IdentifyNumber)) query2 = query2.Where(i => i.Customer.IdentifyNumber == IdentifyNumber);
                        if (!string.IsNullOrEmpty(Fullname)) query2 = query2.Where(i => i.Customer.FullName.ToLower().Contains(Fullname.ToLower()));
                        if (!string.IsNullOrEmpty(Address)) query2 = query2.Where(i => i.Customer.Address.ToLower().Contains(Address.ToLower()));
                        if (!string.IsNullOrEmpty(Precinct)) query2 = query2.Where(i => i.Customer.Precinct == Precinct);
                        if (!string.IsNullOrEmpty(DistrictId)) query2 = query2.Where(i => i.Customer.DistrictId == DistrictId);
                        if (!string.IsNullOrEmpty(ProvinceId)) query2 = query2.Where(i => i.Customer.ProvinceId == ProvinceId);
                        if (!string.IsNullOrEmpty(Model)) query2 = query2.Where(i => i.ItemType == Model);

                        _custCount = query2.Count();
                        if (pagingEnabled) query2 = query2.Skip(startRowIndex).Take(maximumRows);
                        return query2.Select(i => new CustomerInfo(i));

                        #endregion
                    case "WI":
                        #region // warranty Info

                        var query3 = dc.WarrantyInfos.OrderBy(i => i.EngineNumber).Where(i => i.CustomerId != null && i.Customer.CustomerType == (int)CusType.WarrantyInfo);
                        if (!string.IsNullOrEmpty(engineNumber)) query3 = query3.Where(i => i.EngineNumber.Contains(engineNumber.ToUpper()));
                        if (saleFrom > DateTime.MinValue) query3 = query3.Where(i => i.PurchaseDate >= saleFrom);
                        if (saleTo > DateTime.MinValue) query3 = query3.Where(i => i.PurchaseDate < saleTo.AddDays(1));
                        if (!string.IsNullOrEmpty(DealerCode))
                        {
                            query3 = query3.Where(i => i.SellDealerCode == DealerCode);
                        }
                        else if (!string.IsNullOrEmpty(AreaCode))
                        {
                            var pdc = DCFactory.GetDataContext<PartDataContext>();
                            IList<string> dealers = pdc.Dealers.Where(d => d.AreaCode == AreaCode).Select(d => d.DealerCode).ToList();
                            query3 = query3.Where(i => dealers.Contains(i.SellDealerCode));
                        }
                        if (!string.IsNullOrEmpty(IdentifyNumber)) query3 = query3.Where(i => i.Customer.IdentifyNumber == IdentifyNumber);
                        if (!string.IsNullOrEmpty(Fullname)) query3 = query3.Where(i => i.Customer.FullName.ToLower().Contains(Fullname.ToLower()));
                        if (!string.IsNullOrEmpty(Address)) query3 = query3.Where(i => i.Customer.Address.ToLower().Contains(Address.ToLower()));
                        if (!string.IsNullOrEmpty(Precinct)) query3 = query3.Where(i => i.Customer.Precinct == Precinct);
                        if (!string.IsNullOrEmpty(DistrictId)) query3 = query3.Where(i => i.Customer.DistrictId == DistrictId);
                        if (!string.IsNullOrEmpty(ProvinceId)) query3 = query3.Where(i => i.Customer.ProvinceId == ProvinceId);

                        var items = (!string.IsNullOrEmpty(Model)) ? dc.DataItems.Where(i => i.ItemType == Model) : dc.DataItems.AsQueryable();
                        if (!string.IsNullOrEmpty(Model))
                        {
                            query3 = query3.Where(i => i.ItemCode.Contains(Model));
                        }

                        _custCount = query3.Count();
                        if (pagingEnabled) query3 = query3.Skip(startRowIndex).Take(maximumRows);
                        var res = from w in query3
                                  join i in items on w.ItemCode equals i.ItemCode into cusList
                                  from c in cusList.DefaultIfEmpty()
                                  select new CustomerInfo(w, c.ColorCode) { ItemType = c.ItemType };
                        //var res = query3.Join(items.DefaultIfEmpty(), w => w.ItemCode, i => i.ItemCode, (w, i) => new CustomerInfo(w, i.ColorCode) { ItemType = i.ItemType });
                        //var res = query3.Select(i => new CustomerInfo(i) { ItemType = i.ItemCode });
                        //if (pagingEnabled) res = res.Skip(startRowIndex).Take(maximumRows);
                        return res;

                        #endregion
                }
                return null;
            }
        }

        public object CountCustomers(string engineNumber, string cusClass)
        {
            return _custCount;
        }
        public IQueryable<CustomerInfo> FindCustomers(int maximumRows, int startRowIndex, string engineNumber, string cusClass)
        {
            var dc = DCFactory.GetDataContext<ServiceDataContext>();
            if (cusClass == "SL")
            {
                var query = dc.Invoices.OrderBy(i => i.EngineNumber).Where(i => i.CustomerId > 0 && i.Customer.CustomerType == (int)CusType.Sale);
                if (UserHelper.IsDealer) query = query.Where(i => i.DealerCode == UserHelper.DealerCode);
                if (!string.IsNullOrEmpty(engineNumber)) query = query.Where(i => i.EngineNumber.Contains(engineNumber.ToUpper()));
                _custCount = query.Count();
                if ((startRowIndex >= 0) && (maximumRows > 0)) query = query.Skip(startRowIndex).Take(maximumRows);
                return query.Select(i => new CustomerInfo(i));
            }
            else if (cusClass == "SV")
            {
                var query = dc.ServiceHeaders.OrderBy(i => i.EngineNumber).Where(i => i.CustomerId > 0 && i.Customer.CustomerType == (int)CusType.Service);
                if (UserHelper.IsDealer) query = query.Where(i => i.DealerCode == UserHelper.DealerCode);
                if (!string.IsNullOrEmpty(engineNumber)) query = query.Where(i => i.EngineNumber.Contains(engineNumber.ToUpper()));
                _custCount = query.Count();
                if ((startRowIndex >= 0) && (maximumRows > 0)) query = query.Skip(startRowIndex).Take(maximumRows);
                return query.Select(i => new CustomerInfo(i));
            }
            _custCount = 0;
            return null;
        }

        public static string BuildAddress(string Address, string Provinceid, string Precinct, string Districtid)
        {
            string address = Address, province = ProvinceHelper.GetProvinceName(Provinceid);
            address += (string.IsNullOrEmpty(Precinct) ? "" : ((string.IsNullOrEmpty(address)) ? "" : " - ") + Precinct);
            address += (string.IsNullOrEmpty(Districtid) ? "" : ((string.IsNullOrEmpty(address)) ? "" : " - ") + Districtid);
            address += (string.IsNullOrEmpty(province) ? "" : ((string.IsNullOrEmpty(address)) ? "" : " - ") + province);
            return address;
        }

        public static Customer FindSingleCustomer(string identity)
        {
            var dc = DCFactory.GetDataContext<ServiceDataContext>();
            return dc.Customers.FirstOrDefault(c => c.IdentifyNumber == identity);
        }
    }
}