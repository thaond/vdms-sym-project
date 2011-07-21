using System;
using System.Collections.Generic;
using System.Linq;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using VDMS.II.Linq;
using VDMS.II.Report;

namespace VDMS.II.PartManagement
{
    public class InventoryDAO
    {
        /// <summary>
        /// Check ready state to close dealer
        /// </summary>
        /// <param name="dealerCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static bool CanCloseDealer(string dealerCode, int year, int month)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();

            // check all warehouse closed
            int whCount = dc.ActiveWarehouses.Where(wh => wh.DealerCode == dealerCode && wh.Type == WarehouseType.Part).Count();
            int closedWhCount = dc.InventoryLocks.Where(i => i.WarehouseId != null && i.Warehouse.Type == WarehouseType.Part && i.DealerCode == dealerCode && ((i.Month >= month && i.Year == year) || (i.Year > year))).Count();

            // check all sub dealer closed
            bool childsOk = true;
            List<Dealer> dealers = DealerDAO.GetAllChildDealer(dealerCode);
            dealers.ForEach(d =>
            {
                InventoryLock ilck = InventoryDAO.GetInventoryLock(d.DealerCode);
                if ((ilck == null) || (ilck.Month < month && ilck.Year == year) || (ilck.Year < year)) childsOk = false;
            });

            return childsOk && (whCount == closedWhCount);
        }

        /// <summary>
        /// Check ready state to open dealer
        /// </summary>
        /// <param name="dealerCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static bool CanOpen(string dealerCode, int year, int month)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();

            // there are no inventory action, so can close
            Inventory iv = InventoryDAO.GetFirstInventory(dealerCode);
            if (iv == null) return true;

            // month to be open need to greater than the golive month
            // and parent dealer must be opened at checking month
            DateTime chckDate = new DateTime(year, month, 1);
            DateTime frstDate = new DateTime(iv.Year, iv.Month, 1);
            Dealer d = DealerDAO.GetDealerByCode(dealerCode);
            return (chckDate >= frstDate) && !IsInventoryLock(d.ParentCode, year, month);
        }
        /// <summary>
        /// Check ready state to open warehouse
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static bool CanOpen(long warehouseId, int year, int month)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();

            // there are no inventory action, so can close
            Inventory iv = InventoryDAO.GetFirstInventory(warehouseId);
            if (iv == null) return true;

            // month to be open need to greater than the golive month
            // and dealer must be opened at checking month
            DateTime chckDate = new DateTime(year, month, 1);
            DateTime frstDate = new DateTime(iv.Year, iv.Month, 1);
            Warehouse wh = WarehouseDAO.GetWarehouse(warehouseId);
            return (chckDate >= frstDate) && !IsInventoryLock(wh.DealerCode, year, month);
        }

        /// <summary>
        /// Check for locking state of warehouse inventory
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static bool IsInventoryLock(long warehouseId, int year, int month)
        {
            InventoryLock ilck = InventoryDAO.GetInventoryLock(warehouseId);
            if (ilck == null) return false;

            DateTime chckDate = new DateTime(year, month, 1);
            DateTime lckDate = new DateTime(ilck.Year, ilck.Month, 1);

            return chckDate <= lckDate;
        }
        /// <summary>
        /// Check for locking state of all warehouses inventory of a dealer
        /// </summary>
        /// <param name="dealerCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static bool IsInventoryLock(string dealerCode, int year, int month)
        {
            InventoryLock ilck = InventoryDAO.GetInventoryLock(dealerCode);
            if (ilck == null) return false;

            DateTime chckDate = new DateTime(year, month, 1);
            DateTime lckDate = new DateTime(ilck.Year, ilck.Month, 1);

            return chckDate <= lckDate;
        }

        /// <summary>
        /// Get locking entity of warehouse
        /// </summary>
        /// <param name="wId"></param>
        /// <returns></returns>
        public static InventoryLock GetInventoryLock(long wId)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();
            return dc.InventoryLocks.SingleOrDefault(p => p.WarehouseId == wId);
        }
        /// <summary>
        /// Get locking entity of dealer
        /// </summary>
        /// <param name="dealerCode"></param>
        /// <returns></returns>
        public static InventoryLock GetInventoryLock(string dealerCode)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();
            return dc.InventoryLocks.SingleOrDefault(p => p.DealerCode == dealerCode && p.WarehouseId == null);
        }

        /// <summary>
        /// Close warehouse step by step (month after last closed month)
        /// </summary>
        /// <param name="wid"></param>
        public static void DoClose(long wid)
        {
            InventoryDAO.DoClose(wid, 1, 0, false);// now: no need to force resum
        }
        /// <summary>
        /// Close dealer step by step (month after last closed month)
        /// </summary>
        /// <param name="dealerCode"></param>
        public static void DoClose(string dealerCode)
        {
            InventoryDAO.DoClose(dealerCode, 1, 0, true);
        }

        /// <summary>
        /// Close warehouse at specified month.
        /// </summary>
        /// <param name="wId"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        public static void DoClose(long wId, int month, int year)
        {
            InventoryDAO.DoClose(wId, month, year, false);// now: no need to force resum
        }
        /// <summary>
        /// Close dealer at specified month.
        /// </summary>
        /// <param name="dealerCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        public static void DoClose(string dealerCode, int month, int year)
        {
            InventoryDAO.DoClose(dealerCode, month, year, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wId"></param>
        /// <param name="month">New Closed month when no record found</param>
        /// <param name="year">New Closed year when no record found></param>
        public static void DoClose(long wId, int month, int year, bool forceReSum)
        {
            if ((year > DateTime.Now.Year) //|| (year < 2000)
                 || ((year == DateTime.Now.Year) && (month > DateTime.Now.Month))
                 || (month > 12) || (month < 1)
               )
            {
                throw new Exception("Invalid closing month!");
            }

            var dc = DCFactory.GetDataContext<PartDataContext>();

            // check warehouse
            Warehouse wh = WarehouseDAO.GetWarehouse(wId);
            if (wh == null) throw new Exception("Invalid closing warehouse!");

            // lock closed month
            InventoryLock ilck = InventoryDAO.GetInventoryLock(wId);
            if (ilck == null)
            {
                if (year < 2008) throw new Exception("Invalid closing month at first time!");
                // lock month valid to first inventory action?
                Inventory frstIv = InventoryDAO.GetFirstInventory(wId);
                if ((frstIv != null) && ((frstIv.Year < year) || ((frstIv.Year == year) && (frstIv.Month < month))))
                {
                    throw new Exception("At first time, closing month cannot be greater than the first month that changing inventory action happen!");
                }
                ilck = new InventoryLock() { WarehouseId = wId, DealerCode = wh.DealerCode, Month = month, Year = year };
                dc.InventoryLocks.InsertOnSubmit(ilck);
            }
            else
            {
                // change locked month to new Closed month
                if (ilck.Month == 12) { ilck.Month = 1; ilck.Year++; }
                else ilck.Month++;
                if ((ilck.Year > DateTime.Now.Year) || ((ilck.Year == DateTime.Now.Year) && (ilck.Month > DateTime.Now.Month))) throw new Exception("Invalid closing month!");
            }

            // summarization
            List<PartSafety> pses = PartInfoDAO.GetPartSafeties(wId);
            foreach (PartSafety ps in pses)
            {
                Inventory currIv = InventoryDAO.GetPartInventory(ps.PartInfoId, wh.WarehouseId, ilck.Year, ilck.Month);
                if (currIv == null)
                {
                    Inventory prevIv = InventoryDAO.GetPrevInventory(ps.PartInfoId, wh.WarehouseId, ilck.Year, ilck.Month);
                    currIv = new Inventory() { PartInfoId = ps.PartInfoId, Quantity = 0, DealerCode = wh.DealerCode, WarehouseId = wh.WarehouseId, Month = ilck.Month, Year = ilck.Year };
                    dc.Inventories.InsertOnSubmit(currIv);
                    if (prevIv != null) currIv.Quantity = prevIv.Quantity;
                    currIv.Quantity += TransactionDAO.Summarization(ps.PartInfoId, ilck.Month, ilck.Year);
                }
                else if (forceReSum)
                {
                    Inventory prevIv = InventoryDAO.GetPrevInventory(ps.PartInfoId, wh.WarehouseId, ilck.Year, ilck.Month);
                    currIv.Quantity = (prevIv != null) ? prevIv.Quantity : 0;
                    currIv.Quantity += TransactionDAO.Summarization(ps.PartInfoId, ilck.Month, ilck.Year);
                }
            }

            dc.SubmitChanges();

            // gen excel file
            new PartMonthlyReport(wId.ToString(), wh.DealerCode, ilck.Month, ilck.Year).DoReport();
        }
        /// <summary>
        /// Do close dealer, after all warehouses closed.
        /// </summary>
        /// <param name="dealerCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="forceReSum"></param>
        public static void DoClose(string dealerCode, int month, int year, bool forceReSum)
        {
            if ((year > DateTime.Now.Year) //|| (year < 2000)
                 || ((year == DateTime.Now.Year) && (month > DateTime.Now.Month))
                 || (month > 12) || (month < 1)
               )
            {
                throw new Exception("Invalid closing month!");
            }

            // check Dealer
            Dealer d = DealerDAO.GetDealerByCode(dealerCode);
            if (d == null) throw new Exception("Invalid closing dealer!");
            // check all warehouse and sub dealer closed
            if (!InventoryDAO.CanCloseDealer(dealerCode, year, month)) throw new Exception(string.Format("Cannot close {0}! All sub components must be closed before.", dealerCode));

            var dc = DCFactory.GetDataContext<PartDataContext>();

            // lock closed month
            InventoryLock ilck = InventoryDAO.GetInventoryLock(d.DealerCode);
            if (ilck == null)
            {
                if (year < 2008) throw new Exception("Invalid closing month at first time!");
                // lock month valid to first inventory action?
                Inventory frstIv = InventoryDAO.GetFirstInventory(dealerCode);
                if ((frstIv != null) && ((frstIv.Year < year) || ((frstIv.Year == year) && (frstIv.Month < month))))
                {
                    throw new Exception("At first time, closing month cannot be greater than the first month that changing inventory action happen!");
                }
                ilck = new InventoryLock() { WarehouseId = null, DealerCode = d.DealerCode, Month = month, Year = year };
                dc.InventoryLocks.InsertOnSubmit(ilck);
            }
            else
            {
                // change locked month to new Closed month
                if (ilck.Month == 12) { ilck.Month = 1; ilck.Year++; }
                else ilck.Month++;
                if ((ilck.Year > DateTime.Now.Year) || ((ilck.Year == DateTime.Now.Year) && (ilck.Month > DateTime.Now.Month))) throw new Exception("Invalid closing month!");
            }
            // a
            // summarization
            var parts = dc.PartInfos.Where(p => p.DealerCode == dealerCode && p.PartSafeties.Count > 0);

            foreach (var part in parts)
            {
                Inventory currIv = InventoryDAO.GetPartInventory(part.PartInfoId, d.DealerCode, ilck.Year, ilck.Month);
                if (currIv == null)
                {
                    currIv = new Inventory() { DealerCode = d.DealerCode, Month = ilck.Month, PartInfoId = part.PartInfoId, WarehouseId = null, Year = ilck.Year, };
                    dc.Inventories.InsertOnSubmit(currIv);
                }

                if (forceReSum)
                {
                    var childsIv = InventoryDAO.GetWPartInventories(part.PartInfoId, d.DealerCode, ilck.Year, ilck.Month);
                    if (childsIv != null) currIv.Quantity = (childsIv.Count() > 0) ? childsIv.Sum(iv => iv.Quantity) : 0;
                }
            }

            dc.SubmitChanges();

            // gen excel file
            new PartMonthlyReport(null, dealerCode, ilck.Month, ilck.Year).DoReport();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wId"></param>
        public static void DoOpen(long wId)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();

            InventoryLock ilck = InventoryDAO.GetInventoryLock(wId);
            if (ilck == null)
            {
                throw new Exception("This warehouse never closed before!");
            }
            else
            {
                if (!InventoryDAO.CanOpen(wId, ilck.Year, ilck.Month)) throw new Exception("May try to open months before 'first month' or parent component has not been opened!");
                // change locked month to new Closed month
                if (ilck.Month == 1) { ilck.Month = 12; ilck.Year--; }
                else ilck.Month--;
            }

            dc.SubmitChanges();
        }
        /// <summary>
        /// Open dealer part warehouse to previous month
        /// </summary>
        /// <param name="dealerCode"></param>
        public static void DoOpen(string dealerCode)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();

            InventoryLock ilck = InventoryDAO.GetInventoryLock(dealerCode);
            if (ilck == null)
            {
                throw new Exception("This dealer never closed before!");
            }
            else
            {
                if (!InventoryDAO.CanOpen(dealerCode, ilck.Year, ilck.Month)) throw new Exception("May try to open months before 'first month' or parent component has not been opened!");
                // change locked month to new Closed month
                if (ilck.Month == 1) { ilck.Month = 12; ilck.Year--; }
                else ilck.Month--;
            }

            dc.SubmitChanges();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partInfoId"></param>
        /// <param name="warehouseId"></param>
        /// <param name="dtFrom"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public static bool UpdateInventory(long partInfoId, long warehouseId, DateTime dtFrom, int quantity)
        {
            Warehouse wh = WarehouseDAO.GetWarehouse(warehouseId);
            if (wh == null) return false;
            return InventoryDAO.UpdateInventory(partInfoId, wh.DealerCode, warehouseId, dtFrom, quantity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partInfoId"></param>
        /// <param name="dealerCode"></param>
        /// <param name="dtFrom"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public static bool UpdateInventory(long partInfoId, string dealerCode, DateTime dtFrom, int quantity)
        {
            Dealer d = DealerDAO.GetDealerByCode(dealerCode);
            if (d == null) return false;

            return InventoryDAO.UpdateInventory(partInfoId, dealerCode, null, dtFrom, quantity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partInfoId"></param>
        /// <param name="dealerCode"></param>
        /// <param name="warehouseId"></param>
        /// <param name="dtFrom"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public static bool UpdateInventory(long partInfoId, string dealerCode, long? warehouseId, DateTime dtFrom, int quantity)
        {

            // fill inventory data between last month has transaction to transaction date
            Inventory lastIv = null;
            if (warehouseId == null)
                lastIv = InventoryDAO.GetLastPartInventory(partInfoId, dealerCode);
            else
                lastIv = InventoryDAO.GetLastPartInventory(partInfoId, (long)warehouseId);

            int beginQty = 0;
            if (lastIv != null)
            {
                int year = lastIv.Year, month = lastIv.Month;
                month++;
                if (month == 13) { month = 1; year++; }
                for (; (year < dtFrom.Year) || ((month < dtFrom.Month) && (year == dtFrom.Year)); )
                {
                    Inventory iv = InventoryDAO.CreatePartInventory(partInfoId, dealerCode, month, year, lastIv.Quantity, warehouseId);
                    if (iv == null) return false;
                    month++;
                    if (month == 13) { month = 1; year++; }
                }
                beginQty = lastIv.Quantity;
            }
            else
            {
                PartSafety ps = null;
                if (warehouseId == null)
                    beginQty = PartInfoDAO.GetPartSafeties(partInfoId, dealerCode).Sum(p => p.CurrentStock);
                else
                {
                    ps = PartInfoDAO.GetPartSafety(partInfoId, (long)warehouseId);
                    beginQty = (ps != null) ? ps.CurrentStock : 0;
                }
            }

            // update inventory from transaction date to now
            beginQty += quantity;
            for (int year = dtFrom.Year, month = dtFrom.Month; (year < DateTime.Now.Year) || ((month <= DateTime.Now.Month) && (year == DateTime.Now.Year)); )
            {
                Inventory iv = null;
                if (warehouseId == null)
                    iv = InventoryDAO.GetPartInventory(partInfoId, dealerCode, year, month);
                else
                    iv = InventoryDAO.GetPartInventory(partInfoId, (long)warehouseId, year, month);
                if (iv == null)
                {
                    iv = InventoryDAO.CreatePartInventory(partInfoId, dealerCode, month, year, beginQty, warehouseId);
                    if (iv == null) return false;
                }
                else
                {
                    iv.Quantity += quantity;
                    if (iv.Quantity < 0)
                    {
                        PartInfo pi = PartInfoDAO.GetPartInfo(partInfoId);
                        throw new Exception(string.Format("Quantity of \"{0}\" will be negative at {1}/{2}!", pi.PartCode, month, year));
                    }
                }
                month++;
                if (month == 13) { month = 1; year++; }
            }

            return true;
        }

        /// <summary>
        /// part inventory of one warehouse in dealer at previous month from incomming month 
        /// </summary>
        /// <param name="partInfoId"></param>
        /// <param name="wareHouseId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static Inventory GetPrevInventory(long partInfoId, long wareHouseId, int year, int month)
        {
            month--;
            if (month == 0) { month = 12; year--; }
            return InventoryDAO.GetPartInventory(partInfoId, wareHouseId, year, month);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wareHouseId"></param>
        /// <returns></returns>
        public static Inventory GetFirstInventory(long wareHouseId)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();
            Inventory iv = dc.Inventories.Where(i => i.WarehouseId == wareHouseId).OrderBy(i => i.Year).OrderBy(i => i.Month).FirstOrDefault();
            return iv;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dealerCode"></param>
        /// <returns></returns>
        public static Inventory GetFirstInventory(string dealerCode)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();
            Inventory iv = dc.Inventories.Where(i => i.DealerCode == dealerCode).OrderBy(i => i.Year).OrderBy(i => i.Month).FirstOrDefault();
            return iv;
        }

        /// <summary>
        /// part inventory of dealer
        /// </summary>
        /// <param name="partInfoId"></param>
        /// <param name="dealerCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static Inventory GetPartInventory(long partInfoId, string dealerCode, int year, int month)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();
            return dc.Inventories.SingleOrDefault(p => p.PartInfoId == partInfoId && p.WarehouseId == null && p.DealerCode == dealerCode && p.Year == year && p.Month == month);
        }

        /// <summary>
        /// part inventory of one warehouse in dealer
        /// </summary>
        /// <param name="partInfoId"></param>
        /// <param name="wareHouseId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static Inventory GetPartInventory(long partInfoId, long wareHouseId, int year, int month)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();
            return dc.Inventories.SingleOrDefault(p => p.PartInfoId == partInfoId && p.WarehouseId == wareHouseId && p.Year == year && p.Month == month);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partCode"></param>
        /// <param name="wareHouseId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static Inventory GetPartInventory(string partCode, long wareHouseId, int year, int month)
        {
            partCode = partCode.Trim();
            var dc = DCFactory.GetDataContext<PartDataContext>();
            return dc.Inventories.Single(p => p.PartInfo.PartCode == partCode && p.WarehouseId == wareHouseId && p.Year == year && p.Month == month);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partInfoId"></param>
        /// <param name="wareHouseId"></param>
        /// <returns></returns>
        public static Inventory GetLastPartInventory(long partInfoId, long wareHouseId)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();
            return dc.Inventories.Where(p => p.PartInfoId == partInfoId && p.WarehouseId == wareHouseId)
                                 .OrderByDescending(p => p.Month).OrderByDescending(p => p.Year).FirstOrDefault();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partInfoId"></param>
        /// <param name="dealerCode"></param>
        /// <returns></returns>
        public static Inventory GetLastPartInventory(long partInfoId, string dealerCode)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();
            return dc.Inventories.Where(p => p.PartInfoId == partInfoId && p.WarehouseId == null && p.DealerCode == dealerCode)
                                 .OrderByDescending(p => p.Month).OrderByDescending(p => p.Year).FirstOrDefault();
        }

        /// <summary>
        /// Get part inventtories for each warehouses in dealer
        /// </summary>
        /// <param name="partInfoId"></param>
        /// <param name="dealerCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private static IQueryable<Inventory> GetWPartInventories(long partInfoId, string dealerCode, int year, int month)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();
            return dc.Inventories.Where(p => p.PartInfoId == partInfoId && p.WarehouseId != null && p.DealerCode == dealerCode && p.Year == year && p.Month == month);
        }

        /// <summary>
        /// Get all inventory entities for warehouse
        /// </summary>
        /// <param name="partInfoId"></param>
        /// <param name="wareHouseId"></param>
        /// <param name="fromYear"></param>
        /// <param name="fromMonth"></param>
        /// <param name="toYear"></param>
        /// <param name="toMonth"></param>
        /// <returns></returns>
        public static IQueryable<Inventory> GetWPartInventories(long partInfoId, long wareHouseId, int fromYear, int fromMonth, int toYear, int toMonth)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();
            return dc.Inventories.Where(p => p.PartInfoId == partInfoId && p.WarehouseId == wareHouseId
               && p.Year >= fromYear && p.Month >= fromMonth && p.Year <= toYear && p.Month <= toMonth);
        }

        /// <summary>
        /// Get all inventory entities for dealer
        /// </summary>
        /// <param name="partInfoId"></param>
        /// <param name="dealerCode"></param>
        /// <param name="fromYear"></param>
        /// <param name="fromMonth"></param>
        /// <param name="toYear"></param>
        /// <param name="toMonth"></param>
        /// <returns></returns>
        public static IQueryable<Inventory> GetDPartInventories(long partInfoId, string dealerCode, int fromYear, int fromMonth, int toYear, int toMonth)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();
            return dc.Inventories.Where(p => p.PartInfoId == partInfoId && p.WarehouseId == null && p.DealerCode == dealerCode
               && p.Year >= fromYear && p.Month >= fromMonth && p.Year <= toYear && p.Month <= toMonth);
        }

        /// <summary>
        /// Create new PartInventory entity but doesn't submit
        /// </summary>
        /// <param name="partInfoId"></param>
        /// <param name="dealerCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="qty"></param>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public static Inventory CreatePartInventory(long partInfoId, string dealerCode, int month, int year, int qty, long? warehouseId)
        {
            if (qty < 0)
            {
                PartInfo p = PartInfoDAO.GetPartInfo(partInfoId);
                throw new Exception(string.Format("Quantity of \"{0}\" will be negative at {1}/{2}!", (p != null) ? p.PartCode : partInfoId.ToString(), month, year));
            }

            PartDataContext dc = DCFactory.GetDataContext<PartDataContext>();
            var pi = new Inventory { DealerCode = dealerCode, Month = month, Year = year, PartInfoId = partInfoId, Quantity = qty, WarehouseId = warehouseId };
            dc.Inventories.InsertOnSubmit(pi);
            //dc.SubmitChanges();
            return pi;
        }

    }
}