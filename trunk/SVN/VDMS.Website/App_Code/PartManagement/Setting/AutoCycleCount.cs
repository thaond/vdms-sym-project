using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using VDMS.II.Linq;
using VDMS.Helper;
using Resources;
using System.Transactions;
using System.Data.Common;

namespace VDMS.II.PartManagement
{
    public class CycleResult
    {
        public int Total { get; set; }
        public int Invalid { get; set; }
        public int Finished { get; set; }
        public int Failed { get; set; }
        public int New { get; set; }
        public string InvalidList { get; set; }
        public string FailedList { get; set; }
        public List<PartData> FailedItems { get; set; }
        public List<PartData> InvalidItems { get; set; }


        public CycleResult()
        {
        }
    }
    public class CyclePart : PartData
    {
        public int CycleQuantity { get; set; }
        public int Difference { get { return CycleQuantity - Quantity; } }
        public string WarnClass { get { return (Difference == 0) ? "" : "impNotice"; } }
        public CyclePart()
        {
        }
    }

    public class AutoCycleCount : SessionPartDAO<PartData>
    {
        static AutoCycleCount()
        {
            AutoCycleCount.key = "AutoCycleCountParts";
        }
        static bool HasInvalid = false;

        public static bool IsValidPart(PartData part)
        {
            return PartType.IsValidPartType(part.PartType) && (part.Quantity >= 0);
        }

        public static bool DoCycleCount(Stream file, long wId, out CycleResult res)
        {
            AutoCycleCount.Clear();
            res = new CycleResult();
            StringBuilder invalidList = new StringBuilder();
            StringBuilder failedList = new StringBuilder();
            List<PartData> failedItems = new List<PartData>();
            List<PartData> invalidItems = new List<PartData>();

            bool r = AutoCycleCount.LoadExcelData(file, VDMSSetting.CurrentSetting.CycleCountExcelUploadSetting);
            if (!r) return r;

            Warehouse wh = WarehouseDAO.GetWarehouse(wId);
            if (wh == null) throw new Exception(string.Format("Ware house: {0} not found!", wId));

            res.Total = AutoCycleCount.Parts.Count;

            AutoCycleCount.Parts.Where(p => p.State == PartData_Sate.New).ToList().ForEach(p =>
                {
                    if (!AutoCycleCount.IsValidPart(p))
                    {
                        p.State = PartData_Sate.Invalid;
                        p.Error = Resources.Message.DataValueWrong;
                        invalidList.Append(p.PartCode).Append(", ");
                        //invalidItems.Add(p);
                        r = false;
                    }
                });
            invalidItems.AddRange(AutoCycleCount.Parts.Where(p => p.State == PartData_Sate.Invalid));
            res.Invalid = invalidItems.Count;
            AutoCycleCount.HasInvalid = res.Invalid > 0;

            if (r)
            {
                foreach (var p in AutoCycleCount.Parts.Where(p => p.State != PartData_Sate.Invalid))
                {
                    int qty = p.Quantity;

                    PartSafety ps = PartInfoDAO.GetPartSafety(p.PartCode, wId, p.PartType);
                    if (ps != null)
                        qty -= ps.CurrentStock;
                    else
                    {
                        p.State = PartData_Sate.NotFound;
                        res.New++;
                    }
                    // update current stock
                    if (qty != 0)
                    {
                        bool adjustOk = false;
                        string failMsg = "";
                        try
                        {
                            adjustOk = PartDAO.StockAdjust(p.PartCode, p.PartType, wh.DealerCode, wId, null, DateTime.Now, InventoryAction.CycleCount, 0, qty, "Automatically cycle count", "", null);
                        }
                        catch (Exception ex) { failMsg = ex.Message; }

                        if (adjustOk) p.State = PartData_Sate.ActionFinished;
                        else
                        {
                            p.State = PartData_Sate.ActionFailed;
                            p.Error = failMsg;
                            failedList.Append(p.PartCode).Append(", ");
                            failedItems.Add(p);
                        }
                    }
                    // update safety stock
                    if (ps == null) ps = PartInfoDAO.GetPartSafety(p.PartCode, wId, p.PartType);
                    if ((ps != null) && (p.SafetyQuantity >= 0)) ps.SafetyQuantity = p.SafetyQuantity;
                }

                res.Failed = AutoCycleCount.Parts.Where(p => p.State == PartData_Sate.ActionFailed).Count();
                res.Finished = AutoCycleCount.Parts.Where(p => p.State == PartData_Sate.ActionFinished).Count();
                res.New = AutoCycleCount.Parts.Where(p => p.State == PartData_Sate.New).Count();

                if ((res.Failed == 0) && (res.Invalid == 0))
                    PartDAO.PartDC.SubmitChanges();
                else
                    r = false;
            }

            res.InvalidList = invalidList.ToString();
            res.FailedList = failedList.ToString();
            res.FailedItems = failedItems;
            res.InvalidItems = invalidItems;
            return r;
        }

        public int CountInvalidParts()
        {
            return AutoCycleCount.Parts.Count;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<PartData> FindInvalidParts(int startRowIndex, int maximumRows)
        {
            return AutoCycleCount.Parts.Skip(startRowIndex).Take(maximumRows);
        }
    }

    public class CycleCountDAO : SessionPartDAO<CyclePart>
    {
        static CycleCountDAO()
        {
            key = "CycleCountPartList";
        }
        /// <summary>
        /// update part infomation (to be cycle count)
        /// </summary>
        /// <param name="p"></param>
        /// <param name="partCode"></param>
        /// <param name="countQty"></param>
        /// <param name="comment"></param>
        /// <param name="wid"></param>
        public static void UpdatePart(CyclePart p, string partCode, string countQty, string comment, long wid)
        {
            UpdatePart(p, partCode, countQty, comment, wid, false);
        }

        public static void UpdatePart(CyclePart p, string partCode, string countQty, string comment, long wid, bool forceReload)
        {
            if ((p != null) && (wid > 0))
            {
                Warehouse wh = WarehouseDAO.GetWarehouse(wid);
                var dc = DCFactory.GetDataContext<PartDataContext>();

                var ps = PartInfoDAO.GetPartSafety(partCode, wid);
                var part = dc.Parts.FirstOrDefault(i => i.PartCode == partCode);
                if (part == null)
                    throw new Exception(string.Format("Part Code: {0} not found!", partCode));

                // update base infomation
                if ((p.PartCode != partCode) || forceReload)
                {
                    if (ps != null)     // part exist in VDMS
                    {
                        p.PartType = ps.PartInfo.PartType;
                        if (p.PartType == PartType.Part)
                        {
                            p.PartName = (UserHelper.Language == "vi-VN") ? part.VietnamName : part.EnglishName;
                        }
                        else
                        {
                            p.PartName = (UserHelper.Language == "vi-VN") ? ps.PartInfo.Accessory.VietnamName : ps.PartInfo.Accessory.EnglishName;
                        }
                        p.Quantity = ps.CurrentStock;
                        p.SafetyQuantity = ps.SafetyQuantity;
                    }
                    else if (part != null)   // part does not exist in VDMS, get form TipTop
                    {
                        p.PartType = PartType.Part;
                        p.Quantity = p.SafetyQuantity = 0;
                        p.PartName = (UserHelper.Language == "vi-VN") ? part.VietnamName : part.EnglishName;
                    }
                }
                // update quantity and comment
                p.CycleQuantity = string.IsNullOrEmpty(countQty) ? p.Quantity : int.Parse(countQty);
                p.Comment = comment;
                p.PartCode = partCode;
            }
        }

        /// <summary>
        /// Remove Duplicate parts in cycle count data
        /// </summary>
        public static void RemoveDuplicate()
        {
            var t = from i in Parts select i;
            Parts.Clear();
            int index = 1;
            foreach (var item in t)
            {
                if (Parts.SingleOrDefault(x => x.PartCode == item.PartCode) == null)
                {
                    item.Line = index;
                    Parts.Add(item);
                    index++;
                }
            }
        }

        public static void SubmitChanged()
        {
            DCFactory.GetDataContext<PartDataContext>().SubmitChanges();
        }

        public static void SaveSession(long hId, long wid, string commnent)
        {
            bool newSess = false;
            var dc = DCFactory.GetDataContext<PartDataContext>();
            var h = CycleCountDAO.GetCycleCountHeader(hId);
            if (h == null)
            {
                h = new CycleCountHeader
                   {
                       CreatedBy = UserHelper.Username,
                       CreatedTime = DateTime.Now,
                       CycleDate = DateTime.MinValue,
                       DealerCode = UserHelper.DealerCode,
                       LastEditedDate = DateTime.Now,
                       Status = CCStatus.New,
                       WarehouseId = wid,
                       TransactionComment = commnent,
                   };
                newSess = true;
            }
            else
            {
                if (h.Status == CCStatus.Confirmed) throw new Exception(Errors.CCSessionExecuted);
                h.LastEditedDate = DateTime.Now;
                dc.CycleCountDetails.DeleteAllOnSubmit(dc.CycleCountDetails.Where(p => p.CycleCountHeaderId == hId));
            }

            CycleCountDAO.CleanByPartCode();
            if (Parts.Count == 0) throw new Exception(Errors.PartListEmpty);

            foreach (var item in Parts)
            {
                var d = new CycleCountDetail
                {
                    ItemComment = item.Comment,
                    PartCode = item.PartCode,
                    Quantity = item.CycleQuantity,
                    PartType = item.PartType,
                };
                if (!newSess)
                {
                    d.CycleCountHeaderId = hId;
                    dc.CycleCountDetails.InsertOnSubmit(d);
                }
                else d.CycleCountHeader = h;
            }

            if (newSess) dc.CycleCountHeaders.InsertOnSubmit(h);
            dc.SubmitChanges();
        }
        /// <summary>
        /// Do cycle count, based on saved session
        /// </summary>
        /// <param name="hId"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool DoConfirm(long hId, int month, int year)
        {

            //var h = GetCycleCountHeader(hId);

            var dc = DCFactory.GetDataContext<PartDataContext>();
            DbTransaction transaction;
            dc.Connection.Open();
            transaction = dc.Connection.BeginTransaction();
            dc.Transaction = transaction;
            try
            {
                

                var h = dc.CycleCountHeaders.SingleOrDefault(v => v.CycleCountHeaderId == hId);
                if (h == null) return false;

                // calculate actions date
                DateTime cycleDate = new DateTime(year, month, 1);
                DateTime transDate = cycleDate.AddDays(-1);

                // check condition
                if (!InventoryDAO.IsInventoryLock(h.WarehouseId, transDate.Year, transDate.Month))
                {
                    throw new Exception(Errors.NeedInventoryLockBFCC);
                }
                if (VDMSSetting.CurrentSetting.NotAllowTransactionsBeforeCC)
                {
                    if (new TransactionDAO().FindTransactions(h.DealerCode, h.WarehouseId, null, cycleDate.ToShortDateString(), DateTime.MaxValue.ToShortDateString(), null, -1, -1).Count > 0)
                    {
                        throw new Exception(Errors.HasTransactionsBFCC);
                    }
                }

                // do cycle
                if (h.Status == CCStatus.New)
                {
                    //using (var transaction = new TransactionScope())
                    //{
                    // loop for each part need to cycle count
                    CycleCountDAO.SubmitChanged();
                    foreach (var p in h.CycleCountDetails)
                    {
                        int qty = p.Quantity;
                        // get inventory data
                        //PartSafety ps = PartInfoDAO.GetPartSafety(p.PartCode, h.WarehouseId, p.PartType);
                        Inventory iv = InventoryDAO.GetPartInventory(p.PartCode, h.WarehouseId, transDate.Year, transDate.Month);
                        //if (ps != null)
                        //{
                        //if (iv == null) throw new Exception(string.Format("Cannot find inventory data of {0} in {1}!", p.PartCode, transDate.ToString("MM/yyyy")));
                        if (iv != null)
                            qty -= iv.Quantity;
                        //}
                        //else
                        //{
                        //    throw new Exception(string.Format("Cannot find Part Safety data with partcode {0}, warehouseid {1} and parttype {2} ",p.PartCode, h.WarehouseId, p.PartType));
                        //    //return false;

                        //}

                        // update inventory data
                        //if (qty != 0)

                        PartDAO part = new PartDAO();
                        part.FindAllPart(p.PartCode, string.Empty, string.Empty, string.Empty, string.Empty, p.PartType, InventoryAction.CycleCount, h.WarehouseId.ToString(), 0, 0);
                        int count = part.SelectAllCount(p.PartCode, string.Empty, string.Empty, string.Empty, string.Empty, p.PartType, InventoryAction.CycleCount, h.WarehouseId.ToString());
                        if (count > 0)
                        {

                            if (!PartDAO.StockAdjust(p.PartCode, p.PartType, h.DealerCode, h.WarehouseId, null, transDate, InventoryAction.CycleCount, 0, qty, p.ItemComment, string.Format("{0}-{1}", h.CycleCountHeaderId, p.CycleCountDetailId), null, true, false))
                                return false;
                        }
                        else
                        {
                            throw new Exception(string.Format("This {0} is not allow in system", p.PartCode));
                        }
                    }
                    h.Status = CCStatus.Confirmed;
                    h.CycleDate = DateTime.Now;
                    //   CycleCountDAO.SubmitChanged();
                    //    transaction.Complete();
                    //}
                    dc.SubmitChanges();
                    transaction.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get cycle count session by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CycleCountHeader GetCycleCountHeader(long id)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();
            return dc.CycleCountHeaders.SingleOrDefault(h => h.CycleCountHeaderId == id);
        }
        /// <summary>
        /// Load Cycle count parts list to memory, prepare to do cycle count
        /// </summary>
        /// <param name="id"></param>
        public static void LoadCycleCountDetails(long id)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();
            CycleCountDAO.Clear();
            var h = CycleCountDAO.GetCycleCountHeader(id);
            if (h != null)
            {
                var list = dc.CycleCountDetails.Where(d => d.CycleCountHeaderId == id).OrderBy(p => p.CycleCountDetailId);//.ToList();
                //var list1 = new CycleCountDAO().FindCycleCountDetails(id, -1, -1);
                foreach (var d in list)
                {
                    CycleCountDAO.Append(d.PartCode, p => CycleCountDAO.UpdatePart(p, d.PartCode, d.Quantity.ToString(), d.ItemComment, h.WarehouseId, true));
                }
            }
        }

        /// <summary>
        /// Query list of cycle count session
        /// </summary>
        /// <param name="status"></param>
        /// <param name="wid"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable<CycleCountHeader> FindCycleCountHeaders(string status, long wid, int maximumRows, int startRowIndex)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();
            var query = dc.CycleCountHeaders.Where(h => h.WarehouseId == wid).OrderBy(h => h.Status).OrderBy(h => h.CycleDate).AsQueryable();
            if (!string.IsNullOrEmpty(status)) query = query.Where(h => h.Status == status);
            else query = query.Where(h => h.Status != CCStatus.Deleted);

            _cycleCountHeadersCount = query.Count();
            if ((startRowIndex >= 0) && (maximumRows > 0)) query = query.Skip(startRowIndex).Take(maximumRows);

            return query;
        }

        int _cycleCountHeadersCount;
        public int CountCycleCountHeaders(string status, long wid)
        {
            return _cycleCountHeadersCount;
        }

        /// <summary>
        /// Query cycle count parts list and their informations
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable<CyclePart> FindCycleCountDetails(long hid, int maximumRows, int startRowIndex)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();
            var h = GetCycleCountHeader(hid);
            if (h == null) return null;

            var query = dc.CycleCountDetails.Where(d => d.CycleCountHeaderId == hid);
            _cycleCountDetailsCount = query.Count();
            if ((startRowIndex >= 0) && (maximumRows > 0)) query = query.Skip(startRowIndex).Take(maximumRows);

            if (h.Status == CCStatus.New)
            {
                return query.Select(d => new CyclePart
                {
                    Comment = d.ItemComment,
                    CycleQuantity = d.Quantity,
                    PartCode = d.PartCode,
                    PartType = d.PartType,
                    OriginalObj = dc.PartSafeties.SingleOrDefault(p => p.PartInfo.PartCode == d.PartCode
                                        && p.PartInfo.PartType == d.PartType
                                        && p.PartInfo.DealerCode == d.CycleCountHeader.DealerCode
                                        && p.WarehouseId == d.CycleCountHeader.WarehouseId),
                });
            }
            else
            {
                return query.Select(d => new CyclePart
                {
                    Comment = d.ItemComment,
                    CycleQuantity = d.Quantity,
                    PartCode = d.PartCode,
                    PartType = d.PartType,
                    OriginalObj = dc.TransactionHistories.SingleOrDefault(t => t.TransactionCode == InventoryAction.CycleCount
                                        && t.InvoiceNumber == d.CycleCountHeaderId + "-" + d.CycleCountDetailId),
                });
            }
        }
        int _cycleCountDetailsCount;
        public int CountCycleCountDetails(long hid)
        {
            return _cycleCountDetailsCount;
        }

        /// <summary>
        /// Mark one cycle count session as deleted
        /// </summary>
        /// <param name="CycleCountHeaderId"></param>
        public void DeleteHeader(long CycleCountHeaderId)
        {
            var dc = DCFactory.GetDataContext<PartDataContext>();
            CycleCountHeader ch = dc.CycleCountHeaders.SingleOrDefault(h => h.CycleCountHeaderId == CycleCountHeaderId);
            if (ch != null)
            {
                ch.Status = CCStatus.Deleted;
                dc.SubmitChanges();
            }
        }
    }

}
