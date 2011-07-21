using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Expression;
using VDMS.Common.Utils;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;
using VDMS.I.Entity;
using VDMS.I.Linq;
using VDMS.II.BasicData;
using VDMS.II.Linq;
using VDMS.Helper;

namespace VDMS.I.Vehicle
{
    public class IOReportItem
    {
        public string ItemCode { get; set; }
        public string DealerCode { get; set; }
        public string BranchCode { get; set; }
    }
    public class IOStockVehicle
    {
        public string BranchCode { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public string Color { get; set; }
        public int Order { get; set; }
        public int In { get; set; }
        public int Out { get; set; }
        public int Balance { get { return Begin + In + Out; } }
        //public int BeginMonth { get; set; }
        public int BeginActs { get; set; }
        public int Begin { get { return BeginActs + BeginMonth; } }
        public int BeginInvs { get; set; } // dung khi lay dc begin luon 
        public int BeginMonth { get { return BeginInv == null ? BeginInvs : BeginInv.Quantity; } }
        public SaleInventory BeginInv { get; set; }
    }

    public class InventoryHelper
    {
        /// <summary>
        /// Comnine wCode and dCode into single value to use in sale_inventoryLock
        /// </summary>
        /// <param name="dcode"></param>
        /// <param name="wcode"></param>
        /// <returns></returns>
        public static string GetWLockCode(string dcode, string wcode)
        {
            return string.Format("{0}-{1}", dcode, wcode);
        }

        public static bool CanOpenD(string dealerCode, int year, int month)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();

            //Inventory iv = InventoryDAO.GetFirstInventory(dealerCode);
            //if (iv == null) return true;

            //DateTime chckDate = new DateTime(year, month, 1);
            //DateTime frstDate = new DateTime(iv.Year, iv.Month, 1);

            var d = DealerDAO.GetDealerByCode(dealerCode);
            return
                //(chckDate >= frstDate) && 
                !InventoryHelper.IsInventoryLock(month, year, d.ParentCode, 0);
        }
        /// <summary>
        /// Check ready state to open warehouse
        /// </summary>
        /// <param name="wCode"></param>
        /// <param name="dealerCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static bool CanOpenW(string wCode, string dealerCode, int year, int month)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();

            // there are no inventory action, so can open
            var iv = InventoryHelper.GetFirstInventory(wCode, dealerCode);
            if (iv == null) return true;


            // month to be open need to greater than the golive month
            // and parent dealer must be opened at checking month
            var chckDate = new DateTime(year, month, 1);
            var frstDate = new DateTime(iv.Year, iv.Month, 1);

            var wh = WarehouseDAO.GetWarehouse(wCode, dealerCode, "V");
            var Dlock = InventoryHelper.IsInventoryLock(month, year, wh.DealerCode, 0);
            var DlockA = InventoryHelper.IsInventoryLockAny(dealerCode, 0);
            return (chckDate > frstDate) && (!Dlock || !DlockA);
        }
        public static bool CanCloseDealer(string dealerCode, int year, int month)
        {
            var dc1 = DCFactory.GetDataContext<VehicleDataContext>();
            return CanCloseDealer(dealerCode, year, month, dc1);
        }
        /// <summary>
        /// Check ready state to close dealer
        /// </summary>
        /// <param name="dealerCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="dc1"></param>
        /// <returns></returns>
        public static bool CanCloseDealer(string dealerCode, int year, int month, VehicleDataContext dc1)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();
            //var dc1 = DCFactory.GetDataContext<VehicleDataContext>();

            // check all warehouse closed
            int whCount = dc.ActiveWarehouses.Where(wh => wh.DealerCode == dealerCode && wh.Type == VDMS.II.Entity.WarehouseType.Vehicle).Count();
            int closedWhCount = dc1.SaleInventoryLocks.Where(i => i.IsLocked == 1 && i.DealerCode.Contains(dealerCode) && ((i.Month >= month && i.Year == year) || (i.Year > year))).Count();

            // check all sub dealer closed
            bool childsOk = true;
            var dealers = DealerDAO.GetAllChildDealer(dealerCode);
            dealers.ForEach(d =>
            {
                var ilck = dc1.SaleInventoryLocks.SingleOrDefault(i => i.DealerCode == dealerCode && i.IsLocked == 0);
                if ((ilck == null) || (ilck.Month < month && ilck.Year == year) || (ilck.Year < year)) childsOk = false;
            });

            return childsOk && (whCount == closedWhCount);
        }

        // get inventoryLock record 
        public static SaleInventoryLock GetInventoryLock(string dcode, string wcode)
        {
            return GetInventoryLock(GetWLockCode(dcode, wcode), 1);
        }
        /// <summary>
        /// get inventoryLock record 
        /// </summary>
        /// <param name="code">WCode or DCode in full mode</param>
        /// <param name="isWarehouse"></param>
        /// <returns></returns>
        public static SaleInventoryLock GetInventoryLock(string code, int isWarehouse)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            return dc.SaleInventoryLocks.SingleOrDefault(i => i.DealerCode == code && i.IsLocked == isWarehouse);
        }

        /// <summary>
        /// get oldest sale_inventory record
        /// </summary>
        /// <param name="wCode"></param>
        /// <param name="dCode"></param>
        /// <returns></returns>
        public static SaleInventory GetFirstInventory(string wCode, string dCode)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            var iv = (from i in dc.SaleInventories
                      where i.BranchCode == wCode && i.DealerCode == dCode
                      orderby i.Year, i.Month
                      select i);
            return iv.FirstOrDefault();
        }

        /// <summary>
        /// Do close inventory
        /// </summary>
        /// <param name="code"></param>
        /// <param name="dealerCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="dc"></param>
        public static void DoInventory(string code, string dealerCode, int month, int year, VehicleDataContext dc)
        {
            //var dc = DCFactory.GetDataContext<VehicleDataContext>();
            int nextM = month + 1, nextY = year;
            int prevM = month - 1, prevY = year;

            if (nextM == 13) { nextM = 1; nextY++; }
            if (prevM == 0) { prevM = 12; prevY--; }

            var prevInv = dc.SaleInventories.Where(i => i.Year == prevY && i.Month == prevM && i.BranchCode == code && i.DealerCode == dealerCode).ToList();

            // delete all current data
            dc.SaleInventories.DeleteAllOnSubmit(dc.SaleInventories.Where(iv => iv.BranchCode == code && iv.DealerCode == dealerCode && iv.Year == year && iv.Month == month).ToList());

            // get all actions happend in closing month(group by item code)
            var invs = dc.SaleInventoryDays.Where(id => id.ActionDay >= DataFormat.DateToCompareNumber(1, month, year)
                    && id.ActionDay < DataFormat.DateToCompareNumber(1, nextM, nextY)
                    && id.BranchCode == code && id.DealerCode == dealerCode)
                    .GroupBy(id => id.ItemCode, (key, g) => new
                    {
                        Itemcode = key,
                        Quantity = (int)g.Sum(i => i.Quantity),
                    })
                .ToList();
            // all items already exist in sale_inventory and has transactions
            var udInvs = from pi in prevInv
                         join id in invs on pi.ItemCode equals id.Itemcode
                         select new SaleInventory
                         {
                             ItemCode = pi.ItemCode,
                             BranchCode = code,
                             DealerCode = dealerCode,
                             Month = month,
                             Year = year,
                             Quantity = id.Quantity + pi.Quantity,
                         };
            // new items has beem imported in closing month
            var nwInvs = invs.Where(i => prevInv.Where(p => p.ItemCode == i.Itemcode).Count() == 0).Select(i => new SaleInventory
            {
                ItemCode = i.Itemcode,
                BranchCode = code,
                DealerCode = dealerCode,
                Month = month,
                Year = year,
                Quantity = i.Quantity,
            });

            // all items already exist in sale_inventory and has no transactions
            var mvInvs = prevInv.Where(i => invs.Where(c => c.Itemcode == i.ItemCode).Count() == 0).Select(i => new SaleInventory
            {
                ItemCode = i.ItemCode,
                BranchCode = code,
                DealerCode = dealerCode,
                Month = month,
                Year = year,
                Quantity = i.Quantity,
            });

            // insert all to sale_inventory
            if (udInvs.Count() > 0)
                dc.SaleInventories.InsertAllOnSubmit(udInvs.Distinct());
            if (nwInvs.Count() > 0)
                dc.SaleInventories.InsertAllOnSubmit(nwInvs);
            if (mvInvs.Count() > 0)
                dc.SaleInventories.InsertAllOnSubmit(mvInvs);
            dc.SubmitChanges();
        }
        /// <summary>
        /// DoInventory calulate follow dataiteminstance
        /// </summary>
        public static void DoInventory2(string code,string dealerCode, int month, int year,VehicleDataContext dc)
        {
            int nextM = month + 1, nextY = year;
            int prevM = month - 1, prevY = year;

            if (nextM == 13) { nextM = 1; nextY++; }
            if (prevM == 0) { prevM = 12; prevY--; }
            dc.SaleInventories.DeleteAllOnSubmit(dc.SaleInventories.Where(iv => iv.BranchCode == code && iv.DealerCode == dealerCode && iv.Year == year && iv.Month == month).ToList());
            var t = GetIOReportData(dealerCode,code, string.Empty, string.Empty,new DateTime(year, month, 1).ToShortDateString(), new DateTime(nextY, nextM, 1).AddTicks(-1).ToShortDateString());
            var result = from i in t
                         select new SaleInventory
                         {
                             ItemCode = i.ItemCode,
                             BranchCode = code,
                             DealerCode = dealerCode,
                             Month = month,
                             Year = year,
                             Quantity = i.Begin +  i.In + i.Out
                         };
            if (result.Count() > 0)
                dc.SaleInventories.InsertAllOnSubmit(result);
            dc.SubmitChanges();
        }

        public static void DoCloseW(string code, string dealerCode)
        {
            DoCloseW(code, dealerCode, 1, 0);
        }
        public static void DoCloseW(string code, string dealerCode, int month, int year)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            DoCloseW(code, dealerCode, month, year, dc);
        }
        /// <summary>
        /// Close warehouse vehicle inventory at specified month.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="dealerCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="dc"></param>
        public static void DoCloseW(string code, string dealerCode, int month, int year, VehicleDataContext dc)
        {
            if ((year > DateTime.Now.Year) //|| (year < 2000)
                 || ((year == DateTime.Now.Year) && (month > DateTime.Now.Month))
                 || (month > 12) || (month < 1)
               )
            {
                throw new Exception("Invalid closing month!");
            }

            // check for valid warehouse
            var wh = VDMS.II.BasicData.WarehouseDAO.GetWarehouse(code, dealerCode, VDMS.II.Entity.WarehouseType.Vehicle);
            if (wh == null) throw new Exception("Invalid closing warehouse!");

            // get lock record for current closed month
            var ilck = dc.SaleInventoryLocks.SingleOrDefault(i => i.DealerCode == GetWLockCode(dealerCode, code) && i.IsLocked == 1);
            if (ilck == null)// never closed before
            {
                if (year < 2007) throw new Exception("Invalid closing month at first time!");
                // lock month valid to first inventory action?
                SaleInventory frstIv = dc.SaleInventories.Where(i => i.BranchCode == code && i.DealerCode == dealerCode).OrderBy(i => i.Year).OrderBy(i => i.Month).FirstOrDefault();
                if (frstIv != null)
                {
                    DateTime fDt = new DateTime(frstIv.Year, frstIv.Month, 1);
                    DateTime cDt = new DateTime(year, month, 1);
                    if (fDt.AddMonths(1) < cDt)
                        throw new Exception("At first time, closing month cannot be greater than the first month that changing inventory action happen!");
                }
                ilck = new SaleInventoryLock
                {
                    DealerCode = GetWLockCode(wh.DealerCode, wh.Code),
                    Month = month,
                    Year = year,
                    IsLocked = 1
                };
                dc.SaleInventoryLocks.InsertOnSubmit(ilck);
            }
            else
            {
                // change locked month to new Closed month
                if (ilck.Month == 12) { ilck.Month = 1; ilck.Year++; }
                else ilck.Month++;
                if ((ilck.Year > DateTime.Now.Year) || ((ilck.Year == DateTime.Now.Year) && (ilck.Month > DateTime.Now.Month))) throw new Exception("Invalid closing month!");
            }

            dc.SubmitChanges();

            // summarization
            InventoryHelper.DoInventory2(code, dealerCode, (int)ilck.Month, (int)ilck.Year, dc);

            // gen excel file
            //new VDMS.II.Report.PartMonthlyReport(wId.ToString(), wh.DealerCode, ilck.Month, ilck.Year).DoReport();
        }

        public static void DoCloseD(string dealerCode)
        {
            DoCloseD(dealerCode, 1, 0);
        }
        public static void DoCloseD(string dealerCode, int month, int year)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            DoCloseD(dealerCode, month, year, dc);
        }
        /// <summary>
        /// Close dealer vehicle inventory at specified month.
        /// </summary>
        /// <param name="dealerCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="dc"></param>
        public static void DoCloseD(string dealerCode, int month, int year, VehicleDataContext dc)
        {
            if ((year > DateTime.Now.Year) //|| (year < 2000)
                 || ((year == DateTime.Now.Year) && (month > DateTime.Now.Month))
                 || (month > 12) || (month < 1)
               )
            {
                throw new Exception("Invalid closing month!");
            }

            // check Dealer exist
            var d = DealerDAO.GetDealerByCode(dealerCode);
            if (d == null) throw new Exception("Invalid closing dealer!");

            // check all warehouse and sub dealer are closed
            if (!InventoryHelper.CanCloseDealer(dealerCode, year, month, dc))
                throw new Exception(string.Format("Cannot close {0}! All sub components must be closed before.", dealerCode));

            // get lock record for current closed month
            var ilck = dc.SaleInventoryLocks.SingleOrDefault(i => i.DealerCode == dealerCode && i.IsLocked == 0);
            if (ilck == null)   // never closed before
            {
                if (year < 2007) throw new Exception("Invalid closing month at first time!");
                // create new lock record
                ilck = new SaleInventoryLock
                {
                    IsLocked = 0,
                    DealerCode = d.DealerCode,
                    Month = month,
                    Year = year
                };
                dc.SaleInventoryLocks.InsertOnSubmit(ilck);
            }
            else // update locked record to new closing month
            {
                if (ilck.Month == 12) { ilck.Month = 1; ilck.Year++; }
                else ilck.Month++;
                if ((ilck.Year > DateTime.Now.Year) || ((ilck.Year == DateTime.Now.Year) && (ilck.Month > DateTime.Now.Month))) throw new Exception("Invalid closing month!");
            }

            dc.SubmitChanges();
        }

        // open warehouse to previous month
        public static void DoOpenW(string wCode, string dCode)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            DoOpenW(wCode, dCode, dc);
        }
        public static void DoOpenW(string wCode, string dCode, VehicleDataContext dc)
        {
            var ilck = InventoryHelper.GetInventoryLock(dCode, wCode);
            if (ilck == null)
            {
                throw new Exception("This warehouse never closed before!");
            }
            else
            {
                if (!CanOpenW(wCode, dCode, (int)ilck.Year, (int)ilck.Month)) throw new Exception("May try to open months before 'first month' or parent component has not been opened!");
                // change locked month to new Closed month
                if (ilck.Month == 1) { ilck.Month = 12; ilck.Year--; }
                else ilck.Month--;
            }

            dc.SubmitChanges();
        }
        // open dealer
        public static void DoOpenD(string dealerCode)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            DoOpenD(dealerCode, dc);
        }
        public static void DoOpenD(string dealerCode, VehicleDataContext dc)
        {
            var ilck = InventoryHelper.GetInventoryLock(dealerCode, 0);
            if (ilck == null)
            {
                throw new Exception("This dealer never closed before!");
            }
            else
            {
                if (!CanOpenD(dealerCode, (int)ilck.Year, (int)ilck.Month)) throw new Exception("May try to open months before 'first month' or parent component has not been opened!");
                // change locked month to new Closed month
                if (ilck.Month == 1) { ilck.Month = 12; ilck.Year--; }
                else ilck.Month--;
            }

            dc.SubmitChanges();
        }

        //public static bool LockInventory(int month, int year, string Code, bool isWareHouse)
        //{
        //    bool result = false;
        //    try
        //    {
        //        DateTime lckd = new DateTime(year, month, 1);
        //        if ((lckd <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)) && (lckd > DateTime.MinValue))
        //        {
        //            IDao<Inventorylock, long> dao = DaoFactory.GetDao<Inventorylock, long>();
        //            dao.SetCriteria(new ICriterion[] { Expression.Eq("Dealercode", Code), Expression.Eq("Islocked", isWareHouse) });
        //            List<Inventorylock> list = dao.GetAll();
        //            if (list != null)
        //            {
        //                list[0].Month = month;
        //                list[0].Year = year;
        //                list[0].Islocked = isWareHouse;
        //                dao.Save(list[0]);
        //                result = true;
        //            }
        //        }
        //    }
        //    catch { }

        //    return result;
        //}

        public static DateTime GetLockedDate(string dealerCode, string wCode)
        {
            return GetLockedDate(GetWLockCode(dealerCode, wCode), true);
        }
        public static DateTime GetLockedDate(string dealerCode, bool isWareHouse)
        {
            var dao = DaoFactory.GetDao<Inventorylock, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Dealercode", dealerCode), Expression.Eq("Islocked", isWareHouse) });
            dao.SetOrder(new NHibernate.Expression.Order[] { NHibernate.Expression.Order.Desc("Year"), NHibernate.Expression.Order.Desc("Month") });
            List<Inventorylock> list = dao.GetAll();

            return (list.Count > 0) ? new DateTime(list[0].Year, list[0].Month, 1) : DateTime.MinValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Day"></param>
        /// <param name="DealerCode">WCode or DCode in full mode</param>
        /// <param name="isWareHouse"></param>
        /// <returns></returns>
        public static InventoryLockStatus GetInventoryLockStatus(DateTime Day, string DealerCode, int isWareHouse)
        {
            if (!IsInventoryLockAny(DealerCode, isWareHouse)) return InventoryLockStatus.NoFound;
            if (IsInventoryLock(Day, DealerCode, isWareHouse)) return InventoryLockStatus.Locked;
            return InventoryLockStatus.Unlocked;
        }

        //public static bool IsFirstLockMonth(int month, int year, string dealerCode)
        //{
        //    var dao = DaoFactory.GetDao<Saleinventory, long>();
        //    dao.SetCriteria(new ICriterion[] { 
        //                Expression.Eq("Dealercode", dealerCode) 
        //                //Expression.Eq("Islock", dealerCode) ,
        //    });
        //    dao.SetOrder(new NHibernate.Expression.Order[] { 
        //                NHibernate.Expression.Order.Asc("Year"), 
        //                NHibernate.Expression.Order.Asc("Month") 
        //    });
        //    List<Saleinventory> list = dao.GetPaged(0, 1);
        //    if (list.Count > 0)
        //    {
        //        if ((list[0].Month == month) && (list[0].Year == year)) return true;
        //    }
        //    return false;
        //}

        /// <summary>
        /// Check for any lock record.
        /// </summary>
        /// <param name="dealerCode">WCode or DCode in full mode</param>
        /// <param name="isWareHouse"></param>
        /// <returns></returns>
        public static bool IsInventoryLockAny(string dealerCode, int isWareHouse)
        {
            return IsInventoryLock(1, 1, dealerCode, isWareHouse);
        }

        public static bool IsInventoryLock(DateTime Day, string DealerCode, string wCode)
        {
            return IsInventoryLock(Day, GetWLockCode(DealerCode, wCode), 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Day"></param>
        /// <param name="DealerCode">WCode or DCode in full mode</param>
        /// <param name="isWareHouse"></param>
        /// <returns></returns>
        public static bool IsInventoryLock(DateTime Day, string DealerCode, int isWareHouse)
        {
            return IsInventoryLock(Day.Month, Day.Year, DealerCode, isWareHouse);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="DealerCode">WCode or DCode in full mode</param>
        /// <param name="isWareHouse"></param>
        /// <returns></returns>
        public static bool IsInventoryLock(int month, int year, string DealerCode, int isWareHouse)
        {
            // khong check lock invent cho sale/admin
            if (string.IsNullOrEmpty(DealerCode)) return false;
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            var iLock = dc.SaleInventoryLocks
                .Where(il => il.DealerCode == DealerCode && il.IsLocked == isWareHouse)
                .SingleOrDefault();
            if (iLock == null) return false;
            //lock year: 2009, lock month: 8 , year 2011 , month = 05
            return !(iLock.Year < year) && ((iLock.Year > year) || (iLock.Year == year && iLock.Month >= month));
            // false && ( false || ( false && true))
        }

        /// <summary>
        /// Save to inventory actions of day for each item code
        /// If data for item in same day exist, quantity will be update
        /// </summary>
        /// <param name="ItemCode"></param>
        /// <param name="ActionTime"></param>
        /// <param name="Quantity"></param>
        /// <param name="ActionType"></param>
        /// <param name="DealerCode"></param>
        /// <param name="BranchCode"></param>
        /// <returns></returns>
        public static SaleInventoryDay SaveInventoryDay(VehicleDataContext db, string ItemCode, DateTime ActionTime, int Quantity, int ActionType, string DealerCode, string BranchCode)
        {
            //var daoItem = DaoFactory.GetDao<VDMS.Core.Domain.Item, string>();
            //var item = daoItem.GetById(ItemCode, false); //true -> false

            //var daoInven = DaoFactory.GetDao<Inventoryday, long>();

            long ActionDay = long.Parse(ActionTime.ToString("yyyyMMdd"));
            //daoInven.SetCriteria(new ICriterion[] { Expression.Eq("Item", item), Expression.Eq("Actionday", ActionDay)
            //        , Expression.Eq("Actiontype", ActionType), Expression.Eq("Dealercode", DealerCode), Expression.Eq("Branchcode", BranchCode)});

            
            //var list = daoInven.GetAll();
            var ivd =
                db.SaleInventoryDays.FirstOrDefault(
                    p =>
                    p.ItemCode == ItemCode && p.ActionDay == ActionDay && p.ActionType == ActionType &&
                    p.DealerCode == DealerCode && p.BranchCode == BranchCode);

            if (ivd == null)
            {
                ivd = new SaleInventoryDay();
                ivd.ItemCode = ItemCode;
                ivd.ActionDay = ActionDay;
                ivd.ActionType = ActionType;
                ivd.DealerCode = DealerCode;
                ivd.BranchCode = BranchCode;
                ivd.Quantity = 0;
                db.SaleInventoryDays.InsertOnSubmit(ivd);
            }
            ivd.Quantity += Quantity;
            return ivd;
        }

        public static Inventoryday SaveInventoryDay(string ItemCode, DateTime ActionTime, int Quantity, int ActionType, string DealerCode, string BranchCode)
        {
            var daoItem = DaoFactory.GetDao<VDMS.Core.Domain.Item, string>();
            var item = daoItem.GetById(ItemCode, false); //true -> false

            var daoInven = DaoFactory.GetDao<Inventoryday, long>();

            long ActionDay = long.Parse(ActionTime.ToString("yyyyMMdd"));
            daoInven.SetCriteria(new ICriterion[] { Expression.Eq("Item", item), Expression.Eq("Actionday", ActionDay)
					, Expression.Eq("Actiontype", ActionType), Expression.Eq("Dealercode", DealerCode), Expression.Eq("Branchcode", BranchCode)});

            Inventoryday ivd = null;
            var list = daoInven.GetAll();
            if (list.Count == 0)
            {
                ivd = new Inventoryday();
                ivd.Item = item;
                ivd.Actionday = ActionDay;
                ivd.Actiontype = ActionType;
                ivd.Dealercode = DealerCode;
                ivd.Branchcode = BranchCode;
                ivd.Quantity = 0;
            }
            else ivd = list[0];
            ivd.Quantity += Quantity;

            try
            {
                return daoInven.SaveOrUpdate(ivd);
            }
            catch
            {
                return null;
            }
        }

        public static List<IOStockVehicle> GetIOReportData(string DealerCode, string wCode, string itemCode, string itemType, string fromDt, string toDt)
        {
            if (string.IsNullOrEmpty(wCode))
            {
                return GetIOReportData(DealerCode, itemCode, itemType, fromDt, toDt);
            }

            var dealer = DealerDAO.GetDealerByCode(DealerCode);
            DateTime dtFrom = DataFormat.DateFromString(fromDt),
                     dtTo = DataFormat.DateFromString(toDt),
                     dtStartOfBegin = dtFrom.AddMonths(-1);
            var dc = DCFactory.GetDataContext<VehicleDataContext>();

            var query = dc.ItemInstances.Where(i => i.DealerCode == DealerCode && i.BranchCode == wCode);
            //&& i.BranchCode == wCode && (
            //                                   (i.ImportedDate < dtFrom && i.ReleasedDate >= dtFrom) ||
            //                                   (i.ImportedDate >= dtFrom && i.ImportedDate <= dtTo) ||
            //                                   (i.ReleasedDate >= dtFrom && i.ReleasedDate <= dtTo)
            //    // || (i.ImportedDate < dtTo && i.ReleasedDate >= dtTo)
            //                                   ));

            if (!string.IsNullOrEmpty(itemType))
                query = query.Where(i => i.ItemType == itemType);
            if (!string.IsNullOrEmpty(itemCode))
                query = query.Where(i => i.ItemCode == itemCode);

            var items = query.ToList();

            var importedItems = items.Where(i => i.ImportedDate >= dtFrom && i.ImportedDate <= dtTo);
            var beginActImportedItems = items.Where(i => i.ImportedDate < dtFrom && i.ReleasedDate >= dtFrom);
            var releasedItems = items.Where(i => i.ReleasedDate >= dtFrom && i.ReleasedDate <= dtTo);
            //var stockEndItems = items.Where(i => i.ImportedDate < dtTo && i.ReleasedDate >= dtTo);
            var orders = dc.OrderDetails.Where(od => od.OrderHeader.DealerCode == DealerCode)
                                  .Where(od => od.OrderHeader.OrderDate >= dtFrom && od.OrderHeader.OrderDate <= dtTo)
                                  .Where(od => od.OrderHeader.Status == (int)OrderStatus.Confirmed ).ToList();
           var dcitems = dc.Items.ToList();
            var qqq = from i in items
                      group i by new { i.ItemCode, i.BranchCode }
                          into gi
                          let it = dcitems.FirstOrDefault(p => p.ItemCode == gi.Key.ItemCode)
                          select new IOStockVehicle
                          {
                              ItemName = it.ItemName,
                              ItemCode = gi.Key.ItemCode,
                              ItemType = it.ItemType,
                              Color = it.ColorName,
                              BeginActs = beginActImportedItems.Where(i => i.ItemCode == gi.Key.ItemCode && i.BranchCode == gi.Key.BranchCode).Count(),
                              BranchCode = gi.Key.BranchCode,
                              In = importedItems.Where(i => i.ItemCode == gi.Key.ItemCode && i.BranchCode == gi.Key.BranchCode).Count(),
                              Out = -releasedItems.Where(i => i.ItemCode == gi.Key.ItemCode && i.BranchCode == gi.Key.BranchCode).Count(),
                              Order = orders.Where(o => o.ItemCode == gi.Key.ItemCode).Sum(o => o.OrderQty)
                          };

            var itemExited = (from ii in items
                              select ii.ItemCode).Distinct();
            var itemOrder = (from o in orders
                             select new { o.ItemCode, o.Item.ItemType }).Distinct();
            var itemNot = itemOrder.Where(p => !itemExited.Contains(p.ItemCode));
            var queryItemNotExitInInstance = from it in itemNot
                                             where (string.IsNullOrEmpty(itemCode) || it.ItemCode == itemCode) &&
                                             (string.IsNullOrEmpty(itemType) || it.ItemType == itemType)
                                             let i = dcitems.FirstOrDefault(p => p.ItemCode == it.ItemCode)
                                             select new IOStockVehicle
                                             {
                                                 ItemName = i.ItemName,
                                                 ItemCode = i.ItemCode,
                                                 ItemType = i.ItemType,
                                                 Color = i.ColorName,
                                                 BeginActs = 0,
                                                 BranchCode = "ONLY_ORDER",
                                                 In = 0,
                                                 Out = 0,
                                                 Order = orders.Where(o => o.ItemCode == it.ItemCode).Sum(o => o.OrderQty)
                                             };

            return qqq.Union(queryItemNotExitInInstance).Where(p => p.Begin != 0 || p.In != 0 || p.Out != 0 || p.Order != 0).ToList();
        }

        public static List<IOStockVehicle> GetIOReportData(string DealerCode, string itemCode, string itemType, string fromDt, string toDt)
        {
            var dealer = DealerDAO.GetDealerByCode(DealerCode);
            DateTime dtFrom = DataFormat.DateFromString(fromDt),
                     dtTo = DataFormat.DateFromString(toDt),
                     dtStartOfBegin = dtFrom.AddMonths(-1);
            var dc = DCFactory.GetDataContext<VehicleDataContext>();

            var query = dc.ItemInstances.Where(i => i.DealerCode == DealerCode);
                
                //.Where(i => i.DealerCode == DealerCode && (
                //                               (i.ImportedDate < DataFormat.DateOfFirstDayInMonth(dtFrom) && i.ReleasedDate >= dtFrom) ||
                //                               (i.ImportedDate >= DataFormat.DateOfFirstDayInMonth(dtFrom) && i.ImportedDate <= dtTo) ||
                //                               (i.ReleasedDate >= DataFormat.DateOfFirstDayInMonth(dtFrom) && i.ReleasedDate <= dtTo)
                //// || (i.ImportedDate < dtTo && i.ReleasedDate >= dtTo)
                //                               ));

            if (!string.IsNullOrEmpty(itemType))
                query = query.Where(i => i.ItemType == itemType);
            if (!string.IsNullOrEmpty(itemCode))
                query = query.Where(i => i.ItemCode == itemCode);

            var items = query.ToList();

            var beginActImportedItems = items.Where(i =>i.ImportedDate < dtFrom && i.ReleasedDate >= dtFrom);
            //var beginActReleasedItems = items.Where(i => i.ReleasedDate >= DataFormat.DateOfFirstDayInMonth(dtFrom) && i.ReleasedDate < dtFrom);
            var importedItems = items.Where(i => i.ImportedDate >= dtFrom && i.ImportedDate <= dtTo);
            var releasedItems = items.Where(i => i.ReleasedDate >= dtFrom && i.ReleasedDate <= dtTo);
            //var stockEndItems = items.Where(i => i.ImportedDate < dtTo && i.ReleasedDate >= dtTo);
            var orders = dc.OrderDetails.Where(od => od.OrderHeader.DealerCode == DealerCode)
                                  .Where(od => od.OrderHeader.OrderDate >= dtFrom && od.OrderHeader.OrderDate <= dtTo)
                                  .Where(od => od.OrderHeader.Status == (int)OrderStatus.Confirmed).ToList();
            var dcitems = dc.Items.ToList();
            var qqq = from i in items
                      group i by new { i.ItemCode }
                          into gi
                          let it = dcitems.FirstOrDefault(p => p.ItemCode == gi.Key.ItemCode)
                          select new IOStockVehicle
                                     {
                                         ItemName = it.ItemName,
                                         ItemCode = gi.Key.ItemCode,
                                         ItemType = it.ItemType,
                                         Color = it.ColorName,
                                         BeginActs = beginActImportedItems.Where(i => i.ItemCode == gi.Key.ItemCode).Count(),
                                         BranchCode = "",
                                         In = importedItems.Where(i => i.ItemCode == gi.Key.ItemCode).Count(),
                                         Out = -releasedItems.Where(i => i.ItemCode == gi.Key.ItemCode).Count(),
                                         Order = orders.Where(o => o.ItemCode == gi.Key.ItemCode).Sum(o => o.OrderQty)
                                     };
            // 
            var itemExited = (from ii in items
                             select ii.ItemCode).Distinct();
            var itemOrder = (from o in orders
                             select new {o.ItemCode, o.Item.ItemType}).Distinct();
            var itemNot = itemOrder.Where(p => !itemExited.Contains(p.ItemCode));
            var queryItemNotExitInInstance = from it in itemNot
                                             where (string.IsNullOrEmpty(itemCode) || it.ItemCode == itemCode) &&
                                             (string.IsNullOrEmpty(itemType) || it.ItemType == itemType)
                                             let i = dcitems.FirstOrDefault(p => p.ItemCode == it.ItemCode)
                                             select new IOStockVehicle
                                             {
                                                 ItemName = i.ItemName,
                                                 ItemCode = i.ItemCode,
                                                 ItemType = i.ItemType,
                                                 Color = i.ColorName,
                                                 BeginActs = 0,
                                                 BranchCode = "",
                                                 In = 0,
                                                 Out = 0,
                                                 Order = orders.Where(o => o.ItemCode == it.ItemCode).Sum(o => o.OrderQty)
                                             };

            var t = qqq.Union(queryItemNotExitInInstance).Where(
                p => p.Begin != 0 || p.In != 0 || p.Out != 0 || p.Order != 0).OrderBy(p => p.ItemCode).ToList();
            return t;
        }


        private static Boolean IsImportedItem(int status)
        {
            switch (status)
            {
                case (int)ItemStatus.Imported:
                case (int)ItemStatus.ReceivedFromMoving: return true;
                default: return false;
            };
        }

        //public static List<IOStockVehicle> GetIOReportData(string DealerCode, string itemCode, string itemType, string fromDt, string toDt)
        //{
        //    var d = DealerDAO.GetDealerByCode(DealerCode);
        //    DateTime dtFrom = DataFormat.DateFromString(fromDt),
        //             dtTo = DataFormat.DateFromString(toDt),
        //             dtStartOfBegin = dtFrom.AddMonths(-1);

        //    //parsing date to number for searching in sale_inventoryday table
        //    int startDay = DataFormat.DateToCompareNumber(dtFrom);
        //    int startMonth = DataFormat.DateToCompareNumber(DataFormat.DateOfFirstDayInMonth(dtFrom));
        //    int endDay = DataFormat.DateToCompareNumber(dtTo);

        //    var dc = DCFactory.GetDataContext<VehicleDataContext>();
        //    List<IOReportItem> rptItems = new List<IOReportItem>();

        //    var tranHis = dc.SaleTransHistories.Where(id => id.ItemInstance.DealerCode == DealerCode &&
        //                                                    id.TransactionDate <= dtTo &&
        //                                                    id.TransactionDate >= DataFormat.DateOfFirstDayInMonth(dtFrom) &&
        //                                                    (id.TransactionType == (int)ItemStatus.Imported ||
        //        //id.TransactionType == (int)ItemStatus.Moved ||
        //                                                    id.TransactionType == (int)ItemStatus.Sold ||
        //                                                    id.TransactionType == (int)ItemStatus.Return))
        //                                        .Join(dc.ItemInstances,
        //                                                       t => t.ItemInstanceId,
        //                                                       i => i.ItemInstanceId,
        //                                                       (t, i) => new
        //                                                       {
        //                                                           i.ItemInstanceId,
        //                                                           ItemType = i.ItemType,
        //                                                           ItemCode = i.ItemCode,
        //                                                           DealerCode = i.DealerCode,
        //                                                           TransactionType = t.TransactionType,
        //                                                           TransactionDate = t.TransactionDate,
        //                                                       }).Distinct().ToList();
        //    var orders = dc.OrderDetails.Where(od => od.OrderHeader.DealerCode == DealerCode)
        //                                      .Where(od => od.OrderHeader.OrderDate >= dtFrom && od.OrderHeader.OrderDate <= dtTo)
        //                                      .Where(od => od.OrderHeader.Status != (int)OrderStatus.Deleted && od.OrderHeader.Status != (int)OrderStatus.Draft).ToList();
        //    var inventories = dc.SaleInventories.Where(ivb => ivb.DealerCode == DealerCode &&
        //                                                    ivb.Month == dtStartOfBegin.Month &&
        //                                                    ivb.Year == dtStartOfBegin.Year).ToList();

        //    //var saleInvoices = dc.SaleInvoices.Where(id => id.DealerCode == DealerCode && id.CreatedDate <= dtTo && id.CreatedDate >= DataFormat.DateOfFirstDayInMonth(dtFrom)).ToList();

        //    // get all items that already exist in sale_inventory table
        //    var inv = dc.SaleInventories.Where(iv => (iv.DealerCode == DealerCode)
        //        //&& (iv.Month <= dtFrom.Month) && (iv.Year == dtFrom.Year)
        //        //&& (((iv.Month <= dtTo.Month) && (iv.Year == dtTo.Year)) || (iv.Year < dtTo.Year))
        //        //&& (((iv.Month >= dtFrom.Month) && (iv.Year == dtFrom.Year)) || (iv.Year > dtFrom.Year))
        //    ).ToList();
        //    rptItems = inv.GroupBy(iv => new { iv.ItemCode, iv.DealerCode })
        //                   .Select(g => new IOReportItem() { ItemCode = g.Key.ItemCode, DealerCode = g.Key.DealerCode, BranchCode = "" }).ToList();

        //    // get items come from inventory_day (new items)
        //    //var invd = dc.SaleInventoryDays.Where(iv => (iv.DealerCode == DealerCode)
        //    //                                         && (iv.ActionDay <= DataFormat.DateToCompareNumber(dtTo))
        //    //                                         && (iv.ActionDay >= DataFormat.DateToCompareNumber(dtFrom))
        //    //                                         );


        //    var invd = tranHis.Where(i => i.TransactionDate >= dtFrom).Select(i => new { ItemCode = i.ItemCode, DealerCode = i.DealerCode }).ToList();
        //    //var invd2 = saleInvoices.Where(i => i.CreatedDate >= dtFrom).Select(i => new { ItemCode = i.ItemInstance.ItemCode, DealerCode = i.DealerCode }).ToList();
        //    //invd.AddRange(invd2);

        //    rptItems.AddRange(invd.GroupBy(iv => new { iv.ItemCode, iv.DealerCode })
        //                   .Select(g => new IOReportItem() { ItemCode = g.Key.ItemCode, DealerCode = g.Key.DealerCode, BranchCode = "" }));
        //    rptItems.AddRange(orders.Where(o => !inv.Select(i => i.ItemCode).Contains(o.ItemCode) && !invd.Select(id => id.ItemCode).Contains(o.ItemCode))
        //                            .GroupBy(o => new { o.ItemCode, o.OrderHeader.DealerCode })
        //                            .Select(g => new IOReportItem { ItemCode = g.Key.ItemCode, DealerCode = g.Key.DealerCode, BranchCode = "" }));
        //    // get items detail information
        //    var items = dc.Items.Where(i => i.DatabaseCode.Contains(d.DatabaseCode));
        //    if (!string.IsNullOrEmpty(itemCode)) items = items.Where(i => i.ItemCode == itemCode);
        //    if (!string.IsNullOrEmpty(itemType)) items = items.Where(i => i.ItemType == itemType);

        //    var items2 = items.GroupJoin(dc.ItemInstances,
        //                            i => i.ItemCode,
        //                            t => t.ItemCode,
        //                            (i, t) => new
        //                            {
        //                                ItemName = i.ItemName,
        //                                ItemType = i.ItemType,
        //                                ItemCode = i.ItemCode,
        //                                Color = i.ColorName,
        //                            });


        //    var query = rptItems.GroupBy(iv => new { iv.ItemCode, iv.DealerCode })
        //                   .Select(g => new { g.Key.ItemCode, g.Key.DealerCode })
        //                   .Join(items, iv => iv.ItemCode, i => i.ItemCode,
        //                        (iv, i) => new IOStockVehicle
        //                        {
        //                            ItemName = i.ItemName,
        //                            ItemType = i.ItemType,
        //                            ItemCode = i.ItemCode,
        //                            BranchCode = "",
        //                            Color = i.ColorName,
        //                            BeginInvs = inventories.Where(ivb => ivb.ItemCode == iv.ItemCode).Sum(p => p.Quantity),
        //                            // actions happened between the report day and the first day of report month
        //                            //BeginActs = (int)dc.SaleInventoryDays
        //                            //              .Where(id => id.DealerCode == DealerCode)
        //                            //              .Where(id => id.ItemCode == i.ItemCode)
        //                            //              .Where(id => id.ActionDay >= startMonth && id.ActionDay < startDay)
        //                            //              .Sum(id => id.Quantity),
        //                            BeginActs = (int)tranHis.Where(id => id.ItemCode == i.ItemCode && id.TransactionDate < dtFrom && IsImportedItem(id.TransactionType))
        //                                                    .Count()
        //                                        - (int)tranHis.Where(id => id.ItemCode == i.ItemCode && id.TransactionDate < dtFrom && !IsImportedItem(id.TransactionType))
        //                                                      .Count(),
        //                            // order quantity
        //                            Order = orders.Where(od => od.ItemCode == i.ItemCode)
        //                                      .Sum(od => od.OrderQty),
        //                            // import quantity
        //                            //In = (int)dc.SaleInventoryDays.Where(id => id.DealerCode == DealerCode)
        //                            //            .Where(id => id.ItemCode == i.ItemCode)
        //                            //            .Where(id => id.ActionDay >= startDay && id.ActionDay <= endDay && id.Quantity > 0)
        //                            //            .Sum(id => id.Quantity),
        //                            In = (int)tranHis.Where(id => IsImportedItem(id.TransactionType) &&
        //                                                              id.ItemCode == i.ItemCode &&
        //                                                              id.TransactionDate >= dtFrom)
        //                                                 .Count(),

        //                            // export quantity (sold, ...)
        //                            //Out = (int)dc.SaleInventoryDays.Where(id => id.DealerCode == DealerCode)
        //                            //            .Where(id => id.ItemCode == i.ItemCode)
        //                            //            .Where(id => id.ActionDay >= startDay && id.ActionDay <= endDay && id.Quantity < 0)
        //                            //            .Sum(id => id.Quantity),
        //                            Out = -(int)tranHis.Where(id => id.ItemCode == i.ItemCode && id.TransactionDate >= dtFrom && !IsImportedItem(id.TransactionType))
        //                                                    .Count(),
        //                        })
        //        // remove zero items (all properties)
        //                .Where(i => i.BeginInvs != 0 || i.BeginInv != null || i.BeginActs != 0 || i.In != 0 || i.Order != 0 || i.Out != 0)
        //                ;

        //    return query.ToList();
        //}

        /// <summary>
        /// Recalculate vehicle inventory data from dtInit to dtCloseTo.
        /// All old inventory data will be detele.
        /// If dtInit == Datetime.MinValue system will uses first month that dealer golive as dtInit.
        /// </summary>
        /// <param name="dealerCode"></param>
        /// <param name="dtCloseTo"></param>
        /// <param name="dtInit"></param>
        public static void ResetInventory(string dealerCode, DateTime dtCloseTo, DateTime dtInit)
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            VDMS.II.Entity.Dealer d = VDMS.II.BasicData.DealerDAO.GetDealerByCode(dealerCode);
            // loop for all warehouses in dealer
            foreach (var w in d.ActiveWarehouses.Where(wh => wh.Type == "V"))
            {
                // first month to reset inventory
                var fmonth = new { Month = dtInit.Month, Year = dtInit.Year };
                // get first month from database
                if (dtInit == DateTime.MinValue)
                {
                    var fI = (from iv in dc.SaleInventories
                              where iv.DealerCode == dealerCode && iv.BranchCode == w.Code
                              orderby iv.Year, iv.Month
                              select iv).FirstOrDefault();

                    if (fI == null) continue;   //there are no vehicle data => next warehouse
                    fmonth = new { Month = fI.Month, Year = fI.Year };
                }
                // determine that first month's data is ok?
                var inv = dc.SaleInventories.Where(i => i.DealerCode == dealerCode
                                                        && i.BranchCode == w.Code
                                                        && i.Month == fmonth.Month
                                                        && i.Year == fmonth.Year)
                    .GroupBy(i => new { ItemCode = i.ItemCode, DealerCode = i.DealerCode, BranchCode = i.BranchCode },
                    (g, result) => new
                    {
                        ItemCode = g.ItemCode,
                        MinCount = result.Count(r => r.ItemCode == r.ItemCode),
                    }).ToList();
                inv.ForEach(i =>
                {
                    if (i.MinCount > 1) throw new Exception(string.Format("Multi Begin quantity record for {0} in warehouse {1} in {2}/{3}", i.ItemCode, w.Code, fmonth.Month, fmonth.Year));
                });

                // delete all old data
                var ilck = dc.SaleInventoryLocks.Where(i => i.DealerCode.Contains(dealerCode) && i.IsLocked == 1);
                if (ilck != null) dc.SaleInventoryLocks.DeleteAllOnSubmit(ilck);
                dc.SubmitChanges();
                // do calculate inventory 
                DateTime dtFrom = new DateTime(fmonth.Year, fmonth.Month, 1);
                dtCloseTo = DataFormat.DateOfFirstDayInMonth(dtCloseTo);
                while (dtFrom < dtCloseTo)
                {
                    dtFrom = dtFrom.AddMonths(1);
                    InventoryHelper.DoCloseW(w.Code, dealerCode, dtFrom.Month, dtFrom.Year);
                }
            }
            //       
        }
    }

}