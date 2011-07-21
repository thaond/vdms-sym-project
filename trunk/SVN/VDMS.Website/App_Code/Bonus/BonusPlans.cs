using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.Bonus.Linq;
using System.ComponentModel;
using VDMS.Helper;
using VDMS.Bonus.Entity;
using VDMS.II.Linq;
using VDMS.Common.Utils;
using VDMS.I.Vehicle;
using System.IO;
using Excel;

namespace VDMS.II.BonusSystem
{
    [DataObject]
    public class BonusPlans
    {
        static List<BonusPlanDetail> EditingItems = new List<BonusPlanDetail>();
        static List<BonusPlanHeader> EditingPlans = new List<BonusPlanHeader>();

        public BonusPlans()
        {
        }

        public static BonusDataContext DC
        {
            get
            {
                return DCFactory.GetDataContext<BonusDataContext>();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IQueryable<BonusPlanHeader> GetPlans(string name, DateTime from, DateTime to, string dealer)
        {
            if (name == null) name = "";
            string dbCode = UserHelper.DatabaseCode ?? "";

            var query = DC.BonusPlanHeaders.Where(p => p.BonusPlanName.Contains(name) && p.DatabaseCode.Contains(dbCode));
            if (!string.IsNullOrEmpty(dealer))
            {
                query = query.Where(p => p.BonusPlanDetails.Any(d => d.DealerCode == dealer));
            }
            if (from > DateTime.MinValue)
            {
                query = query.Where(p => p.FromDate >= from.Date);
            }
            if (to > DateTime.MinValue)
            {
                query = query.Where(p => p.ToDate <= to.Date);
            }

            return query;
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeletePlan(int BonusPlanHeaderId)
        {
            var p = DC.BonusPlanHeaders.SingleOrDefault(h => h.BonusPlanHeaderId == BonusPlanHeaderId);
            if (p != null)
            {
                DC.BonusPlanHeaders.DeleteOnSubmit(p);
                DC.SubmitChanges();
            }
        }

        public static List<BonusPlanDetail> GetAllEditingItems()
        {
            return EditingItems.Where(p => !p.Deleted && p.SessionId == HttpContext.Current.Session.SessionID).ToList();
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static List<BonusPlanDetail> GetEditingItems(long PlanId)
        {
            return EditingItems.Where(p => !p.Deleted && p.SessionId == HttpContext.Current.Session.SessionID && p.BonusPlanHeaderId == PlanId).ToList();
        }
        public static List<BonusPlanDetail> GetDeletingItems(long PlanId)
        {
            return EditingItems.Where(p => p.Deleted && p.SessionId == HttpContext.Current.Session.SessionID && p.BonusPlanHeaderId == PlanId).ToList();
        }

        #region Query bonus

        int _ConfirmedPlansDetailCount;
        public int CountConfirmedPlansDetail(int maximumRows, int startRowIndex, string planName, DateTime from, DateTime to, string dealer)
        {
            return _ConfirmedPlansDetailCount;
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable<BonusPlanDetail> GetConfirmedPlansDetail(int maximumRows, int startRowIndex, string planName, DateTime from, DateTime to, string dealer)
        {
            var q = DC.BonusPlanDetails.Where(p => p.Status == BonusStatus.Confirmed);
            if (!string.IsNullOrEmpty(dealer)) q = q.Where(p => p.DealerCode == dealer);
            if (from > DateTime.MinValue) q = q.Where(p => p.BonusDate >= from.Date);
            if (to > DateTime.MinValue) q = q.Where(p => p.BonusDate <= to.Date);
            if (!string.IsNullOrEmpty(planName)) q = q.Where(p => p.BonusPlanHeader.BonusPlanName.Contains(planName));

            q = q.OrderBy(p => p.BonusDate).OrderBy(p => p.DealerCode);
            _ConfirmedPlansDetailCount = q.Count();
            if (maximumRows > 0) q = q.Skip(startRowIndex).Take(maximumRows);

            return q;
        }

        int _TransCount;
        public int CountBonusTransactions(int maximumRows, int startRowIndex, string planName, DateTime from, DateTime to, string dealer)
        {
            return _TransCount;
        }
        //public int CountBonusTransactions(string planName, DateTime from, DateTime to, string dealer)
        //{
        //    return _TransCount;
        //}
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable<BonusTransaction> GetBonusTransactions(int maximumRows, int startRowIndex, string planName, DateTime from, DateTime to, string dealer)
        {
            var q = DC.BonusTransactions.Where(p => p.Dealer.DatabaseCode == UserHelper.DatabaseCode);

            // sale man chi quan ly vung cua minh(admin - ADM: theo DB)
            if (UserHelper.Profile.Position == VDMS.II.Entity.PositionType.Employee && !UserHelper.IsSysAdmin)
                q = q.Where(p => p.Dealer.AreaCode == UserHelper.AreaCode);

            if (!string.IsNullOrEmpty(dealer)) q = q.Where(p => p.DealerCode == dealer);
            if (from > DateTime.MinValue) q = q.Where(p => p.TransactionDate >= from.Date);
            if (to > DateTime.MinValue) q = q.Where(p => p.TransactionDate < to.AddDays(1).Date);
            if (!string.IsNullOrEmpty(planName)) q = q.Where(p => p.BonusPlanDetail.BonusPlanHeader.BonusPlanName.Contains(planName));

            q = q.OrderBy(p => p.DealerCode).ThenBy(p => p.TransactionDate);
            _TransCount = q.Count();
            if (maximumRows > 0) q = q.Skip(startRowIndex).Take(maximumRows);

            return q;
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable<BonusTransaction> GetBonusTransactions(string planName, DateTime from, DateTime to, string dealer)
        {
            var q = DC.BonusTransactions.Where(p => p.Dealer.DatabaseCode == UserHelper.DatabaseCode);

            // sale man chi quan ly vung cua minh(admin - ADM: theo DB)
            if (UserHelper.Profile.Position == VDMS.II.Entity.PositionType.Employee && !UserHelper.IsSysAdmin)
                q = q.Where(p => p.Dealer.AreaCode == UserHelper.AreaCode);

            if (!string.IsNullOrEmpty(dealer)) q = q.Where(p => p.DealerCode == dealer);
            if (from > DateTime.MinValue) q = q.Where(p => p.TransactionDate >= from.Date);
            if (to > DateTime.MinValue) q = q.Where(p => p.TransactionDate < to.AddDays(1).Date);
            if (!string.IsNullOrEmpty(planName)) q = q.Where(p => p.BonusPlanDetail.BonusPlanHeader.BonusPlanName.Contains(planName));

            q = q.OrderBy(p => p.DealerCode).ThenBy(p => p.TransactionDate);
            _SumBonus = (double) q.Sum(p => p.Amount);
            return q;
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable<BonusTransactionSummaryByDealer> GetBonusTransactions2(string planName, DateTime from, DateTime to, string dealer)
        {
            var q = DC.BonusTransactions.Where(p => p.Dealer.DatabaseCode == UserHelper.DatabaseCode);

            // sale man chi quan ly vung cua minh(admin - ADM: theo DB)
            if (UserHelper.Profile.Position == VDMS.II.Entity.PositionType.Employee && !UserHelper.IsSysAdmin)
                q = q.Where(p => p.Dealer.AreaCode == UserHelper.AreaCode);

            if (!string.IsNullOrEmpty(dealer)) q = q.Where(p => p.DealerCode == dealer);
            if (from > DateTime.MinValue) q = q.Where(p => p.TransactionDate >= from.Date);
            if (to > DateTime.MinValue) q = q.Where(p => p.TransactionDate < to.AddDays(1).Date);
            if (!string.IsNullOrEmpty(planName)) q = q.Where(p => p.BonusPlanDetail.BonusPlanHeader.BonusPlanName.Contains(planName));
            var query = from qq in q
                        group qq by qq.DealerCode
                        into qqq
                            select new BonusTransactionSummaryByDealer
                                   {
                                       BonusTransactions = (IQueryable<BonusTransaction>) (from qqqq in qqq
                                                                         select qqqq),
                                       Sum = (double)qqq.Sum(p => p.Amount)
                                   };

            //q = q.OrderBy(p => p.DealerCode).ThenBy(p => p.TransactionDate);

            return query;
        }

        private double _SumBonus;
        public double GetSumBonus()
        {
            return _SumBonus;
        }
        #endregion

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEditingItem(Guid Id)
        {
            var Item = EditingItems.SingleOrDefault(p => p.Id == Id);
            if (Item != null)
                DeleteEditingItem(Item);
        }
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEditingItem(BonusPlanDetail Item)
        {
            var it = EditingItems.SingleOrDefault(t => t.Id == Item.Id);
            if (it != null && it.BonusPlanDetailId > 0)
                it.Deleted = true;
            else
                EditingItems.RemoveAll(i => i.Id == Item.Id);
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateEditingItem(Guid Id, long Amount, DateTime BonusDate, string DealerCode, string Description, string Status, string PlanType)
        {
            var o = EditingItems.SingleOrDefault(p => p.Id == Id);
            if (o != null)
            {
                o.Amount = Amount;
                o.BonusDate = BonusDate;
                o.Description = Description;
                o.Status = Status;
                o.PlanType = PlanType;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateEditingItem(BonusPlanDetail Item)
        {
            var o = EditingItems.SingleOrDefault(p => p.Id == Item.Id);
            if (o != null)
            {
                o.Amount = Item.Amount;
                o.BonusDate = Item.BonusDate;
                o.Description = Item.Description;
                o.Status = Item.Status;
                o.PlanType = Item.PlanType;
                o.DealerCode = Item.DealerCode;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static BonusPlanDetail AddEditingItem(long PlanId, string type)
        {
            var o = new BonusPlanDetail()
            {
                Id = Guid.NewGuid(),
                BonusPlanHeaderId = PlanId,
                SessionId = HttpContext.Current.Session.SessionID,
                UserName = UserHelper.Username,
                CreatedDate = DateTime.Now,
                PlanType = type,
            };
            EditingItems.Add(o);
            return o;
        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void AddEditingItem(int count, long PlanId, string type)
        {
            for (int i = 0; i < count; i++)
            {
                AddEditingItem(PlanId, type);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void AddEditingItem(long PlanId, long source, DateTime BonusDate, string DealerCode, string Status, string PlanType, long Amount, string Description)
        {
            var o = AddEditingItem(PlanId, PlanType);
            o.BonusSourceId = source == 0 ? null : (long?)source;
            o.DealerCode = DealerCode;
            o.Amount = Amount;
            if (BonusDate > DateTime.MinValue) o.BonusDate = BonusDate;
            o.Description = Description;
            o.Status = Status;
        }

        public static void AddEditingItem(BonusPlanDetail item)
        {
            item.Id = Guid.NewGuid();
            item.SessionId = HttpContext.Current.Session.SessionID;
            EditingItems.Add(item);
        }

        public static void CleanSessionItems(string sessionId)
        {
            EditingItems.RemoveAll(p => p.SessionId == sessionId);
        }
        public static void CleanSessionPlans(string sessionId)
        {
            EditingPlans.RemoveAll(p => p.SessionId == sessionId);
        }
        public static void CleanSessionPlan(long id)
        {
            EditingPlans.RemoveAll(p => p.BonusPlanHeaderId == id);
            EditingItems.RemoveAll(p => p.BonusPlanHeaderId == id);
        }

        public static BonusPlanHeader SaveEditingPlan(long PId, string planName, string type, string status, string desc, DateTime fromDate, DateTime toDate)
        {
            var plan = DC.BonusPlanHeaders.SingleOrDefault(p => p.BonusPlanHeaderId == PId);
            var eitems = GetEditingItems(PId);
            var ditems = GetDeletingItems(PId);
            if (eitems.Count() > 0 || ditems.Count() > 0)
            {
                DC.BonusPlanDetails.AttachAll(ditems, false);
                DC.BonusPlanDetails.DeleteAllOnSubmit(ditems);
                if (plan == null)
                {
                    plan = new BonusPlanHeader();
                    plan.UserName = UserHelper.Username;
                    plan.CreatedDate = DateTime.Now;
                    plan.DatabaseCode = UserHelper.DatabaseCode == null ? "HTFDNF" : UserHelper.DatabaseCode;
                    DC.BonusPlanHeaders.InsertOnSubmit(plan);
                }
                plan.BonusPlanName = planName;
                plan.Description = desc;
                plan.FromDate = fromDate;
                if (toDate > DateTime.MinValue) plan.ToDate = toDate;
                plan.PlanType = type;
                if (PId == 0) plan.Status = status;

                foreach (var i in eitems)
                {
                    if (i.BonusPlanDetailId == 0)
                    {
                        i.BonusPlanHeader = plan;
                        DC.BonusPlanDetails.InsertOnSubmit(i);
                    }
                    else
                    {
                        var d = DC.BonusPlanDetails.SingleOrDefault(p => p.BonusPlanDetailId == i.BonusPlanDetailId);
                        if (d != null)
                        {
                            d.Amount = i.Amount;
                            d.BonusDate = i.BonusDate;
                            d.BonusPlanHeaderId = i.BonusPlanHeaderId;
                            d.BonusSourceId = i.BonusSourceId;
                            d.DealerCode = i.DealerCode;
                            d.Description = i.Description;
                            d.PlanType = i.PlanType;
                            d.Status = i.Status;
                            d.UserName = i.UserName;
                        }
                    }
                }

                DC.SubmitChanges();
                return plan;
            }
            else return null;
        }
        public static BonusPlanHeader LoadPlan(long id)
        {
            if (id > 0) CleanSessionPlan(id);
            var plan = DC.BonusPlanHeaders.SingleOrDefault(p => p.BonusPlanHeaderId == id);
            if (plan != null)
            {
                EditingPlans.Add(plan);
                var items = plan.BonusPlanDetails.ToList();
                items.ForEach(p => { p.SessionId = HttpContext.Current.Session.SessionID; p.Id = Guid.NewGuid(); });
                EditingItems.AddRange(items);
                return plan;
            }
            return null;
        }
        public static BonusPlanHeader GetEditingPlan(long id)
        {
            return EditingPlans.SingleOrDefault(p => p.BonusPlanHeaderId == id);
        }

        public static void ConfirmPlan(long PlanId)
        {
            var plan = DC.BonusPlanHeaders.SingleOrDefault(p => p.BonusPlanHeaderId == PlanId);
            if (plan != null)
            {
                var items = plan.BonusPlanDetails.Where(p => p.Status == BonusStatus.Normal && p.BonusDate.HasValue 
                    //&& p.BonusDate.Value.Date <= DateTime.Now.Date
                    );
                foreach (var item in items)
                {
                    ConfirmItem(item);
                }
                plan.Status = (plan.BonusPlanDetails.Any(p => p.Status == BonusStatus.Normal || p.Status == BonusStatus.Locked)) ? BonusPlanStatus.Processing : BonusPlanStatus.Confirmed;
            }
        }
        public static bool IsValidBonusDate(BonusPlanDetail item)
        {
            return item.BonusDate.HasValue && (
                !BonusCloser.IsLock(item.DealerCode, item.BonusDate.Value) //&&
                //item.BonusDate.Value.Date <= DateTime.Now.Date ||
                //item.Status == BonusStatus.Confirmed || item.Status == BonusStatus.Locked
                );
        }
        public static void MakeBonusTrans(BonusDataContext dc, string type, DateTime TransDate, string dCode, string oNum, long Amount, long? BonusPlanDetailId, long? BonusSourceId, string comment)
        {
            var db = dc.Bonus.SingleOrDefault(d => d.DealerCode == dCode);
            if (db == null)
            {
                db = new VDMS.Bonus.Entity.Bonus() { Amount = 0, DealerCode = dCode, };
                DC.Bonus.InsertOnSubmit(db);
            }

            //var oAm = db.Amount;
            db.Amount += Amount;
            if (//oAm >= 0 && 
                db.Amount < 0) throw new Exception("Bonus balance less than zero!");

            // order header via ordernumber
            long? oid = null;
            if (!string.IsNullOrEmpty(oNum))
            {
                var o = OrderDAO.GetOrder(oNum);
                if (o != null) oid = o.OrderHeaderId;
            }

            var th = new BonusTransaction()
            {
                Amount = Amount,
                BonusPlanDetailId = BonusPlanDetailId,
                BonusSourceId = BonusSourceId,
                CreatedDate = DateTime.Now,
                DealerCode = dCode,
                OrderId = oid,
                Description = comment,
                TransactionDate = TransDate.Date,
                UserName = UserHelper.Username,
                TransactionType = type,
            };
            dc.BonusTransactions.InsertOnSubmit(th);
        }
        public static void ConfirmItem(BonusPlanDetail item)
        {
            if (item.Status == BonusStatus.Normal && IsValidBonusDate(item))
            {
                item.Status = BonusStatus.Confirmed;
                MakeBonusTrans(DC, BonusTransactionType.ConfirmPlan, item.BonusDate.Value, item.DealerCode,
                    null, item.Amount, item.BonusPlanDetailId, item.BonusSourceId,
                    string.IsNullOrEmpty(item.Description) ?
                        (string.IsNullOrEmpty(item.BonusPlanHeader.Description) ? item.BonusPlanHeader.BonusPlanName : item.BonusPlanHeader.Description) :
                        item.Description);
            }
        }

        public static bool IsOrderBonusOK(string dCode, long bonus)
        {
            return bonus <= GetDealerBonus(dCode);
        }

        public static long GetDealerBonus(string dCode)
        {
            return (long)DC.BonusTransactions.Where(t => t.DealerCode == dCode && (t.TransactionType == BonusTransactionType.ConfirmPlan || t.TransactionType == BonusTransactionType.OrderSubstract)).Sum(t => t.Amount);
        }

        public static void CleanEditingItems(long PlanId)
        {
            EditingItems.RemoveAll(i => i.BonusPlanHeaderId == PlanId && string.IsNullOrEmpty(i.DealerCode));
            EditingItems.ForEach(i =>
            {
                if (i.BonusPlanHeaderId == PlanId && i.BonusDate.HasValue && i.BonusDate.Value == DateTime.MinValue)
                    i.BonusDate = DateTime.MaxValue;
            });
        }

        public static void CleanEditingItems()
        {
            EditingItems.RemoveAll(i => i.SessionId == HttpContext.Current.Session.SessionID);
        }

        public static void ImportExcel(Stream excel, VDMS.VDMSSetting.SettingData.BonusExcelSetting setting)
        {
            IExcelDataReader spreadsheet = ExcelReaderFactory.CreateBinaryReader(excel);
            var amountCol = setting.Amount;
            var dateCol = setting.BonusDate;
            var planCol = setting.BonusPlan;
            var sourceCol = setting.BonusSource;
            var dateFormat = setting.DateFormat;
            var dealerCol = setting.DealerCode;
            var descCol = setting.Description;
            var startRow = setting.StartRow;
            var planMonthCol = setting.PlanMonth;

            var rows = spreadsheet.AsDataSet().Tables[0].AsEnumerable();
            var query = from r in rows
                        select new
                        {
                            Amount = amountCol == 0 || amountCol > r.ItemArray.Count() ? string.Empty : r.Field<string>(amountCol - 1),
                            Date = dateCol == 0 || dateCol > r.ItemArray.Count() ? string.Empty : r.Field<string>(dateCol - 1),
                            Plan = planCol == 0 || planCol > r.ItemArray.Count() ? string.Empty : r.Field<string>(planCol - 1),
                            Source = sourceCol == 0 || sourceCol > r.ItemArray.Count() ? string.Empty : r.Field<string>(sourceCol - 1),
                            DealerCode = dealerCol == 0 || dealerCol > r.ItemArray.Count() ? string.Empty : r.Field<string>(dealerCol - 1),
                            Description = descCol == 0 || descCol > r.ItemArray.Count() ? string.Empty : r.Field<string>(descCol - 1),
                            PlanMonth = planMonthCol == 0 || planMonthCol > r.ItemArray.Count() ? string.Empty : r.Field<string>(planMonthCol - 1),
                        };
            var data = query.Skip(startRow - 1).TakeWhile(v => !string.IsNullOrEmpty(v.Plan) ||
                                                               !string.IsNullOrEmpty(v.Amount) ||
                                                               !string.IsNullOrEmpty(v.DealerCode));

            foreach (var item in data)
            {
                //var planHeader = DC.BonusPlanHeaders.FirstOrDefault(h => h.BonusPlanName.ToUpper() == item.Plan.ToUpper());
                var bonusSource = DC.BonusSources.FirstOrDefault(s => s.BonusSourceName.ToUpper() == item.Source.ToUpper());
                var dealer = DC.Dealers.FirstOrDefault(s => s.DealerCode.ToUpper() == item.DealerCode.ToUpper());
                var planExisted = DC.BonusPlanDetails.Where(p => p.DealerCode.ToUpper() == item.DealerCode.ToUpper() && p.BonusPlanHeader.BonusPlanName.ToUpper() == item.Plan.ToUpper()).FirstOrDefault();
                if (bonusSource != null && dealer != null && (planExisted == null || planExisted.Status != BonusStatus.Confirmed))
                {
                    if (planExisted != null)
                    {
                        var o = AddEditingItem(planExisted.BonusPlanHeaderId, planExisted.PlanType);
                        o.BonusHeaderPlanName = item.Plan;
                        o.Amount = int.Parse(item.Amount);
                        o.BonusDate = DataFormat.DateFromExcel(item.Date, dateFormat);
                        o.BonusPlanDetailId = planExisted.BonusPlanDetailId;
                        o.BonusSourceId = bonusSource.BonusSourceId;
                        o.CreatedDate = DateTime.Now;
                        o.DealerCode = dealer.DealerCode;
                        o.Description = item.Description;
                        o.BonusSourceName = bonusSource.BonusSourceName;
                        o.BonusHeaderPlanMonth = DataFormat.DateFromExcel(item.PlanMonth, dateFormat);
                    }
                    else
                    {
                        var planDetail = new BonusPlanDetail();

                        planDetail.BonusHeaderPlanName = item.Plan;
                        planDetail.Amount = int.Parse(item.Amount);
                        planDetail.BonusDate = DataFormat.DateFromExcel(item.Date, dateFormat);
                        //planDetail.BonusPlanHeaderId = planHeader.BonusPlanHeaderId;
                        planDetail.BonusSourceId = bonusSource.BonusSourceId;
                        planDetail.CreatedDate = DateTime.Now;
                        planDetail.DealerCode = dealer.DealerCode;
                        planDetail.Description = item.Description;
                        planDetail.BonusSourceName = bonusSource.BonusSourceName;
                        planDetail.BonusHeaderPlanMonth = DataFormat.DateFromExcel(item.PlanMonth, dateFormat);
                        //AddEditingItem(planDetail.BonusPlanHeaderId, planDetail.BonusSourceId.Value, planDetail.BonusDate.Value, planDetail.DealerCode, BonusStatus.Normal, BonusType.Vehicle, planDetail.Amount, planDetail.Description);
                        AddEditingItem(planDetail);
                    }
                }
            }

        }

        public static void SaveImportingBonus()
        {
            foreach (var item in EditingItems)
            {
                var planHeader = DC.BonusPlanHeaders.FirstOrDefault(h => h.BonusPlanName.ToUpper() == item.BonusHeaderPlanName.ToUpper());
                if (planHeader == null)
                {
                    planHeader = new BonusPlanHeader
                    {
                        BonusPlanName = item.BonusHeaderPlanName,
                        CreatedDate = DateTime.Now,
                        DatabaseCode = UserHelper.DatabaseCode == null ? "HTFDNF" : UserHelper.DatabaseCode,
                        FromDate = item.BonusHeaderPlanMonth == DateTime.MinValue ? DateTime.Now : item.BonusHeaderPlanMonth,
                        PlanType = BonusType.Vehicle,
                        Status = BonusStatus.Normal,
                        UserName = UserHelper.Username,
                    };

                    DC.BonusPlanHeaders.InsertOnSubmit(planHeader);
                }

                var planDetail = DC.BonusPlanDetails.Where(p => p.DealerCode.ToUpper() == item.DealerCode.ToUpper() && p.BonusPlanHeader.BonusPlanName.ToUpper() == planHeader.BonusPlanName.ToUpper()).FirstOrDefault();
                if (planDetail == null)
                {
                    planDetail = new BonusPlanDetail
                    {
                        BonusPlanHeader = planHeader,
                        CreatedDate = DateTime.Now,
                        Status = BonusStatus.Normal,
                        PlanType = BonusType.Vehicle,
                    };
                    DC.BonusPlanDetails.InsertOnSubmit(planDetail);
                }

                planDetail.Amount = item.Amount;
                planDetail.BonusDate = item.BonusDate;
                planDetail.DealerCode = item.DealerCode;
                planDetail.Description = item.Description;
                planDetail.UserName = UserHelper.Username;
                planDetail.BonusSourceId = item.BonusSourceId;
            }
            DC.SubmitChanges();
        }
    }
}