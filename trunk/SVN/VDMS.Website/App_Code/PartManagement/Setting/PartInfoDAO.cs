using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.Linq;
using System.Transactions;

namespace VDMS.II.PartManagement
{
    public class PartSettingInfo
    {
        public string PartType { get; set; }
        public long? AccessoryId { get; set; }
        public long PartInfoId { get; set; }
        public int? UnitPrice { get; set; }
        public string DealerCode { get; set; }
        public string PartCode { get; set; }
        public string EName { get; set; }
        public string VName { get; set; }
        public Favorite SaleFav { get; set; }
        public Favorite OrderFav { get; set; }
        public AccessoryType AccType { get; set; }
        public Category Category { get; set; }
        public string TipTopCategory { get; set; }
    }

    public class PartSafetySetting : PartSafety
    {
        public string PartCode { get; set; }
        public string VietNameName { get; set; }
        public string EnglishName { get; set; }
        public string WarehouseAddress { get; set; }
        public new PartInfo PartInfo { get; set; }
        public new Warehouse Warehouse { get; set; }

        public PartSafetySetting()
        {
        }
        public PartSafetySetting(PartSafety ps, string vName, string eName)
            : this(ps)
        {
            this.EnglishName = eName;
            this.VietNameName = vName;
        }

        public PartSafetySetting(PartSafety ps)
        {
            this.PartCode = ps.PartInfo.PartCode;
            this.PartInfo = ps.PartInfo;
            this.PartInfoId = ps.PartInfoId;
            this.SafetyQuantity = ps.SafetyQuantity;
            this.CurrentStock = ps.CurrentStock;
            this.Warehouse = ps.Warehouse;
            this.WarehouseId = ps.WarehouseId;
        }
    }

    public class PartInfoDAO
    {
        public static PartDataContext PartDC
        {
            get
            {
                return DCFactory.GetDataContext<PartDataContext>();
            }
        }

        #region setting part

        int itemCountForSetting;
        public int CountForSetting(string partCode, string partName, string categoryId, string type, string favStatus)
        {
            return itemCountForSetting;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<PartSettingInfo> FindForSetting(string partCode, string partName, string categoryId, string type, string favStatus, int maximumRows, int startRowIndex)
        {
            partCode = (partCode == null) ? string.Empty : partCode.Trim().ToUpper();
            partName = (partName == null) ? string.Empty : partName.Trim();

            var query = PartDC.PartInfos.Where(pi => pi.PartType == type
                            && pi.PartCode.Contains(partCode)
                            && pi.DealerCode == UserHelper.DealerCode);
            if (favStatus == "Y")
            {
                query = query.Where(pi => PartDC.Favorites.Where(f => f.PartCode == pi.PartCode).Count() > 0);
            }
            else if (favStatus == "N")
            {
                query = query.Where(pi => PartDC.Favorites.Where(f => f.PartCode == pi.PartCode).Count() == 0);
            }
            IEnumerable<PartSettingInfo> list;

            if (type == PartType.Accessory)
            {
                #region type == "A"

                long catId;
                long.TryParse(categoryId, out catId);

                query = query.Where(pi => pi.AccessoryId != null)
                             .Where(pi => (pi.Accessory.EnglishName.Contains(partName) || pi.Accessory.VietnamName.Contains(partName)));
                if (catId > 0)
                {
                    query = query.Where(pi => pi.CategoryId == catId);
                }
                // count total items
                itemCountForSetting = query.Count();
                // paging and select to new form
                query = query.OrderBy(p => p.PartCode).Skip(startRowIndex).Take(maximumRows);
                list = query.Select(pi => new PartSettingInfo()
                                            {
                                                PartType = PartType.Accessory,
                                                Category = pi.Category,
                                                AccType = pi.Accessory.AccessoryType,
                                                PartInfoId = pi.PartInfoId,
                                                AccessoryId = pi.AccessoryId,
                                                DealerCode = pi.DealerCode,
                                                PartCode = pi.PartCode,
                                                OrderFav = PartDC.Favorites.SingleOrDefault(f => f.PartCode == pi.PartCode && f.PartType == pi.PartType && f.Type == "O"),
                                                SaleFav = PartDC.Favorites.SingleOrDefault(f => f.PartCode == pi.PartCode && f.PartType == pi.PartType && f.Type == "S"),
                                                EName = pi.Accessory.EnglishName,
                                                VName = pi.Accessory.VietnamName,
                                                UnitPrice = pi.Price,
                                            });

                #endregion
            }
            else
            {
                #region type == "P"

                var parts = PartDC.Parts.Where(p => p.VietnamName.Contains(partName) || p.EnglishName.Contains(partName))
                                        .Where(p => p.DatabaseCode == UserHelper.DatabaseCode);

                if (!string.IsNullOrEmpty(categoryId))
                {
                    parts = parts.Where(p => p.Category.Contains(categoryId));
                }

                list = query.Join(parts, pi => pi.PartCode, p => p.PartCode, (pi, p) => new PartSettingInfo()
                                        {
                                            PartType = PartType.Part,
                                            TipTopCategory = p.Category,
                                            PartInfoId = pi.PartInfoId,
                                            DealerCode = pi.DealerCode,
                                            PartCode = pi.PartCode,
                                            OrderFav = PartDC.Favorites.SingleOrDefault(f => f.PartCode == pi.PartCode && f.PartType == pi.PartType && f.Type == "O"),
                                            SaleFav = PartDC.Favorites.SingleOrDefault(f => f.PartCode == pi.PartCode && f.PartType == pi.PartType && f.Type == "S"),
                                            EName = p.EnglishName,
                                            VName = p.VietnamName,
                                        }
                                  );
                itemCountForSetting = list.Count();
                list = list.OrderBy(p => p.PartCode).Skip(startRowIndex).Take(maximumRows);

                #endregion
            }

            return list;
        }

        #endregion

        #region part safeties adjust

        int safetiesCountForAdjust;
        public int CountSafetiesForAdjust(string partCode, long warehouseId, string categoryId, string type)
        {
            return safetiesCountForAdjust;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable FindSafetiesForAdjust(string partCode, long warehouseId, string categoryId, string type, int maximumRows, int startRowIndex)
        {
            if (partCode == null) partCode = "";
            if (categoryId == null) categoryId = "";

            IQueryable<PartSafetySetting> res = null;
            var list = PartDC.PartSafeties
                            .Where(p => p.PartInfo.PartCode.Contains(partCode))
                            .Where(p => p.WarehouseId == warehouseId)
                            .Where(p => p.PartInfo.PartType == type)
                            ;

            if (type == "A")
            {
                #region type == "A"

                long catId;
                long.TryParse(categoryId, out catId);
                if (catId > 0)
                {
                    list = list.Where(p => p.PartInfo.CategoryId == catId);
                }
                res = list.Select(ps => new PartSafetySetting(ps, ps.PartInfo.Accessory.VietnamName, ps.PartInfo.Accessory.EnglishName));

                #endregion
            }
            else
            {
                #region type == "P"

                var parts = PartDC.Parts.Where(p => p.DatabaseCode == UserHelper.DatabaseCode);
                if (!string.IsNullOrEmpty(categoryId)) parts = parts.Where(p => p.Category.Contains(categoryId));

                res = list.Join(parts, ps => ps.PartInfo.PartCode, p => p.PartCode,
                                (ps, p) => new PartSafetySetting(ps, p.VietnamName, p.EnglishName));

                #endregion
            }

            safetiesCountForAdjust = res.Count();
            return res.Skip(startRowIndex).Take(maximumRows);
        }

        #endregion

        #region setting safeties parts

        //int safetiesCountForSetting;
        public int CountSafetiesForSetting(string dealerCode)
        {
            return PartDC.ActiveWarehouses.Where(w => w.DealerCode == dealerCode && w.Type == WarehouseType.Part).Count();
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<PartSafetySetting> FindSafetiesForSetting(long partInfoId, string dealerCode)
        {
            var res = GetPartSafeties(partInfoId, dealerCode).Select(ps => new PartSafetySetting(ps)).ToList();
            foreach (var item in PartDC.ActiveWarehouses.Where(w => w.DealerCode == dealerCode && w.Type == WarehouseType.Part).ToList())
            {
                var safety = res.Find(p => p.WarehouseId == item.WarehouseId);
                if (safety == null)
                {
                    var newP = new PartSafetySetting { PartInfoId = partInfoId, WarehouseId = item.WarehouseId, Warehouse = item, WarehouseAddress = item.Address, CurrentStock = 0, SafetyQuantity = 0 };
                    res.Add(newP);
                }
                else
                {
                    safety.WarehouseAddress = safety.Warehouse.Address;
                }
            }
            return res;
        }

        #endregion

        #region part safeties

        //int safetiesCount;
        //public int CountSafeties(string partCode, string dealerCode)
        //{
        //    return safetiesCount;
        //}

        //[DataObjectMethod(DataObjectMethodType.Select)]
        //public List<PartSafety> FindSafeties(string partCode, string dealerCode)
        //{
        //    safetiesCount = PartInfoDAO.GetPartSafetiesCount(partCode, dealerCode);
        //    return PartInfoDAO.GetPartSafeties(partCode, dealerCode);
        //}

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region static methods

        public static int GetPrice(string partCode, string partType)
        {
            if (partType == PartType.Part) return (int)VDMS.Data.TipTop.Part.GetPartPrice(partCode);
            else
            {
                var p = PartInfoDAO.GetPartInfo(partCode, partType);
                if (p != null) return (p.Price.HasValue) ? (int)p.Price : 0;
                else return 0;
            }
        }

        public static IQueryable<PartInfo> SearchAccessories(string partCode, string partName, string dealerCode)
        {
            if (string.IsNullOrEmpty(partCode)) partCode = "";
            if (string.IsNullOrEmpty(partName)) partName = "";

            var query = PartDC.PartInfos.Where(pi => pi.Accessory.EnglishName.Contains(partName) || pi.Accessory.VietnamName.Contains(partName));
            if (!string.IsNullOrEmpty(dealerCode))
            {
                query = query.Where(pi => pi.DealerCode == dealerCode);
            }

            return query;
        }

        /// <summary>
        /// If exist return existing pi
        /// </summary>
        /// <param name="PartCode"></param>
        /// <param name="PartType"></param>
        /// <param name="DealerCode"></param>
        /// <returns></returns>
        public static PartInfo AddPartToDealer(string PartCode, string PartType, string DealerCode)
        {
            //using (var db = new PartDataContext())
            //{
            
            var db = new PartDataContext(); // must create another data context
            var pi = db.PartInfos.SingleOrDefault(p => p.DealerCode == DealerCode && p.PartCode == PartCode && p.PartType == PartType);
            if (pi != null) return pi;

            // cancel check dealer, assume dealer is valid
            //Dealer dl = DealerDAO.GetDealerByCode(DealerCode);
            //if (dl == null) return null;

            pi = new PartInfo
            {
                DealerCode = DealerCode,
                PartCode = PartCode,
                PartType = PartType
            };

            // cancel check part code, assume part code is valid
            //if (!PartDAO.IsPartCodeValid(PartCode, "P", DealerCode)) return null;

            db.PartInfos.InsertOnSubmit(pi);
            db.SubmitChanges();
//            db.Dispose();
            return pi;
        }

        /// <summary>
        /// Inc Current Stock n Create "part safety" if needed
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="Quantity"></param>
        /// <param name="warehouseId"></param>
        public static void IncCurrentStock(PartInfo pi, int Quantity, long warehouseId)
        {
            var ps = PartInfoDAO.GetPartSafety(pi.PartInfoId, warehouseId);
            if (ps == null)
            {
                var db = new PartDataContext(); // must create another data context
                ps = new PartSafety
                {
                    WarehouseId = warehouseId,
                    PartInfoId = pi.PartInfoId,
                    CurrentStock = 0,
                    SafetyQuantity = 0
                };
                db.PartSafeties.InsertOnSubmit(ps);
                db.SubmitChanges();
                //db.Dispose();

            }

            ps = PartInfoDAO.GetPartSafety(pi.PartInfoId, warehouseId);
            if (ps != null)
            {
                ps.CurrentStock += Quantity;
                if (ps.CurrentStock < 0)
                {
                    throw new Exception(string.Format("On hand quantity of \"{0}\" will be negative!", pi.PartCode));
                }
            }
            else throw new Exception(string.Format("Cannot create safatyStock to inc current stock for {0}!", pi.PartCode));
        }

        public static PartInfo SaveOrUpdate(long? catId, string dealerCode, string partCode, string type, int? price)
        {
            PartInfo pi = PartInfoDAO.GetPartInfo(partCode, dealerCode, type);
            if (pi == null) return PartInfoDAO.AddPartToDealer(catId, dealerCode, partCode, type, price);
            pi.CategoryId = catId;
            pi.PartType = type;
            pi.Price = price;
            PartDC.SubmitChanges();

            return pi;
        }

        public static PartInfo AddPartToDealer(long? catId, string dealerCode, string partCode, string type, int? price)
        {
            PartInfo pi = null;
            if (!PartExist(partCode, dealerCode))
            {
                pi = new PartInfo() { CategoryId = catId, DealerCode = dealerCode, PartCode = partCode, PartType = type, Price = price };
                PartDC.PartInfos.InsertOnSubmit(pi);
                PartDC.SubmitChanges();
            }
            return pi;
        }

        public static bool PartExist(string partCode, string dealerCode)
        {
            return GetPartInfo(partCode, dealerCode) != null;
        }

        public static PartInfo GetPartInfo(long partInfoId)
        {
            return PartDC.PartInfos.SingleOrDefault(p => p.PartInfoId == partInfoId);
        }
        public static PartInfo GetPartInfo(string partCode, string type)
        {
            return GetPartInfo(partCode, UserHelper.DealerCode, type);
        }

        public static PartInfo GetPartInfo(string partCode, string dealerCode, string type)
        {
            return PartDC.PartInfos.SingleOrDefault(pi => pi.PartCode == partCode && pi.DealerCode == dealerCode && pi.PartType == type);
        }


        public static PartInfo CreatePartInfo(string partCode, string dealerCode, string partType)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            var pi = db.PartInfos.SingleOrDefault(p => p.DealerCode == dealerCode && p.PartCode == partCode && p.PartType == partType);
            if (pi != null) return pi;
            pi = new PartInfo
            {
                DealerCode = dealerCode,
                PartCode = partCode,
                PartType = partType
            };
            db.PartInfos.InsertOnSubmit(pi);
            return pi;
        }

        public static PartSafety CreatePartSafety(PartInfo pInfo, long warehouseId, int quantity, int safeQty)
        {
            //Warehouse wh = PartDC.ActiveWarehouses.SingleOrDefault(w => w.WarehouseId == warehouseId);
            //if (wh == null) return null;

            //PartInfo pi = PartDC.PartInfos.SingleOrDefault(p => p.PartCode == partCode && p.DealerCode == wh.DealerCode);
            //if (pi == null) return null;

            var db = DCFactory.GetDataContext<PartDataContext>();
            PartSafety ps = null;
            if (pInfo.PartInfoId != 0) ps = db.PartSafeties.SingleOrDefault(p => p.PartInfoId == pInfo.PartInfoId);
            if (ps != null)
                ps.CurrentStock += quantity;
            else
            {
                ps = new PartSafety
                {
                    CurrentStock = quantity,
                    SafetyQuantity = safeQty,
                    WarehouseId = warehouseId
                };
                db.PartSafeties.InsertOnSubmit(ps);
            }
            return ps;
        }

        public static List<PartSafety> GetPartSafeties(long wId)
        {
            return PartDC.PartSafeties.Where(ps => ps.WarehouseId == wId).ToList();
        }

        public static List<PartSafety> GetPartSafeties(string dealerCode)
        {
            return PartDC.PartSafeties.Where(ps => ps.Warehouse.DealerCode == dealerCode).ToList();
        }

        public static PartSafety GetPartSafety(string partCode, long wareHouseId)
        {
            var v = PartDC.PartSafeties.Where(p => p.PartInfo.PartCode == partCode && p.WarehouseId == wareHouseId);
            if (v.Count() > 0)
                return v.First();
            else
                return null;
            //			return PartDC.PartSafeties.SingleOrDefault(p => p.PartInfo.PartCode == partCode && p.WarehouseId == wareHouseId);
        }

        public static PartSafety GetPartSafety(string partCode, long wareHouseId, string partType)
        {
            var v = PartDC.PartSafeties.Where(p => p.PartInfo.PartType == partType && p.PartInfo.PartCode == partCode && p.WarehouseId == wareHouseId);
            if (v.Count() > 0)
                return v.First();
            else
                return null;
            //			return PartDC.PartSafeties.SingleOrDefault(p => p.PartInfo.PartType == partType && p.PartInfo.PartCode == partCode && p.WarehouseId == wareHouseId);
        }

        public static PartSafety GetPartSafety(long partInfoId, long wareHouseId)
        {
            return PartDC.PartSafeties.SingleOrDefault(p => p.PartInfo.PartInfoId == partInfoId && p.WarehouseId == wareHouseId);
        }

        // like cond
        //public static int SearchPartSafetiesCount(string partCode, string dealerCode)
        //{
        //    var count = from p in PartDC.PartSafeties
        //                join w in PartDC.ActiveWarehouses on p.WarehouseId equals w.WarehouseId
        //                where
        //                  p.PartInfo.PartCode.Contains(partCode) &&
        //                  w.DealerCode == dealerCode
        //                group p by new
        //                {
        //                    p.PartInfo.PartCode
        //                } into g
        //                select new
        //                {
        //                    g.Key.PartCode
        //                };
        //    return count.Count();
        //}


        // exactly 

        public static int GetPartSafetiesCount(long partInfoId, string dealerCode)
        {
            return PartDC.PartSafeties.Where(p => p.PartInfoId == partInfoId && p.Warehouse.DealerCode == dealerCode).Count();
        }

        public static List<PartSafety> GetPartSafeties(long partInfoId, string dealerCode)
        {
            return PartDC.PartSafeties.Where(p => p.PartInfoId == partInfoId && p.Warehouse.DealerCode == dealerCode).ToList();
        }

        //        public static List<PartSafety> GetPartSafeties(string partCode, long warehouseId, string catCode, string type, int maximumRows, int startRowIndex)
        //        {
        //#warning tiptopcat
        //            if (type.Equals("P", StringComparison.OrdinalIgnoreCase))
        //            {
        //                return PartDC.PartSafeties.Where(p => p.PartInfo.PartCode == partCode 
        //                            //&& p.Part.TipTopCategoryCode == catCode 
        //                            && p.WarehouseId == warehouseId).Skip(startRowIndex).Take(maximumRows).ToList();
        //            }
        //            else
        //            {
        //                return PartDC.PartSafeties.Where(p => p.PartInfo.PartCode == partCode 
        //                            && p.Part.PartInfos p.WarehouseId == warehouseId).Skip(startRowIndex).Take(maximumRows).ToList();
        //            }
        //        }

        public static void SaveChanged()
        {
            PartDC.SubmitChanges();
        }

        #region pending actions

        //public static PartSafety PendingUpatePartSafety()
        //{ 
        //}

        #endregion

        #endregion

    }
}