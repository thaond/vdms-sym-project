using System;
using System.Collections;
using NHibernate.Expression;
using VDMS.I.Vehicle;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.IDAL.Interface;
using VDMS.Helper;

namespace VDMS.I.Vehicle
{
    public enum AdjustmentErrorCode : int
    {
        OK = 0,
        InvalidEngineNumber = 1,
        InvalidDealer = 2,
        InvalidStatus = 3,
        SaveDataFailed = 4,
        InvalidBranch = 5,
        EmptyBranchList = 6,
        AlreadyHasVoucher = 7,
        InventLocked = 8,
    }
    public enum AdjustmentTask : int
    {
        None = 0,
        Move = 2,
        CheckLacked = 3,
        CheckRedundant = 5,
        AddVoucher = 4
    }

    public partial class Adjustment
    {
        public const bool CheckDealerCode = true;

        public static AdjustmentErrorCode UpdateVerhicle(string enginNumber, DateTime moveDate, string from, string to, string dealerCode, string dbCode, bool voucher, AdjustmentTask task)
        {
            using (TransactionBlock trs = new TransactionBlock())
            {
                Shippingdetail shd;
                AdjustmentErrorCode error = GetItemInfos(enginNumber, dealerCode, dbCode, out shd);
                if (error != AdjustmentErrorCode.OK) return error;

                // change item status 
                IDao<Iteminstance, long> dao = DaoFactory.GetDao<Iteminstance, long>();
                IDao<Shippingdetail, long> sdao = DaoFactory.GetDao<Shippingdetail, long>();
                Iteminstance item = dao.GetById(shd.PRODUCTINSTANCE.Id, false); //true -> false
                if (item == null) return AdjustmentErrorCode.InvalidEngineNumber;
                switch (task)
                {
                    case AdjustmentTask.Move: 
                        //item.Status = (int)ItemStatus.Moved; 
                        item.Branchcode = to; 
                        break;
                    case AdjustmentTask.CheckLacked: item.Status = (int)ItemStatus.Lacked; item.Releaseddate = moveDate; break;
                    case AdjustmentTask.CheckRedundant: item.Status = (int)ItemStatus.Redundant; break;
                    case AdjustmentTask.AddVoucher: shd.Voucherstatus = voucher; break;
                }
                try
                {
                    switch (task)
                    {
                        case AdjustmentTask.Move:
                        case AdjustmentTask.CheckLacked:
                        case AdjustmentTask.CheckRedundant: dao.SaveOrUpdate(item); break;
                        case AdjustmentTask.AddVoucher: sdao.SaveOrUpdate(shd); break;
                    }
                }
                catch { trs.IsValid = false; return AdjustmentErrorCode.SaveDataFailed; }

                // write transHistory
                IDao<TransHis, long> trhdao = DaoFactory.GetDao<TransHis, long>();
                TransHis trans = new TransHis();
                TransHis transTo = new TransHis();
                trans.Actualcost = 0;
                if (from != null) trans.Frombranch = UserHelper.BuildFullBranch(dealerCode, from);
                if (to != null) trans.Tobranch = UserHelper.BuildFullBranch(dealerCode, to);
                trans.Transactiondate = moveDate;
                switch (task)
                {
                    case AdjustmentTask.Move: 
                        trans.Transactiontype = (int)ItemStatus.Moved;
                        transTo.Actualcost = trans.Actualcost;
                        transTo.Frombranch = trans.Tobranch;
                        transTo.Tobranch = trans.Frombranch;
                        transTo.Transactiondate = trans.Transactiondate;
                        transTo.Transactiontype = (int)ItemStatus.ReceivedFromMoving;
                        transTo.Modifieddate = DateTime.Now;
                        transTo.Iteminstance = item;
                        transTo.Modifiedby = UserHelper.Username;
                        break;
                    case AdjustmentTask.CheckLacked: trans.Transactiontype = (int)ItemStatus.Lacked; break;
                    case AdjustmentTask.CheckRedundant: trans.Transactiontype = (int)ItemStatus.Redundant; break;
                    case AdjustmentTask.AddVoucher: trans.Transactiontype = (int)ItemStatus.VoucherCompensated; break;
                }
                trans.Modifieddate = DateTime.Now;
                trans.Iteminstance = item;
                trans.Modifiedby = UserHelper.Username;
                try 
                { 
                    trhdao.SaveOrUpdate(trans);
                    if (task == AdjustmentTask.Move)
                        trhdao.SaveOrUpdate(transTo);
                }
                catch { trs.IsValid = false; return AdjustmentErrorCode.SaveDataFailed; }

                // save xxx info
                Inventoryday invDay;
                try
                {
                    switch (task)
                    {
                        case AdjustmentTask.Move:
                            invDay = InventoryHelper.SaveInventoryDay(item.Item.Id, moveDate, -1, (int)ItemStatus.Moved, dealerCode, from);
                            if (invDay == null) { trs.IsValid = false; return AdjustmentErrorCode.SaveDataFailed; }
                            invDay = InventoryHelper.SaveInventoryDay(item.Item.Id, moveDate, 1, (int)ItemStatus.Moved, dealerCode, to);
                            if (invDay == null) { trs.IsValid = false; return AdjustmentErrorCode.SaveDataFailed; }
                            break;
                        case AdjustmentTask.CheckLacked:
                            invDay = InventoryHelper.SaveInventoryDay(item.Item.Id, moveDate, -1, (int)ItemStatus.Lacked, dealerCode, item.Branchcode);
                            if (invDay == null) { trs.IsValid = false; return AdjustmentErrorCode.SaveDataFailed; }
                            break;
                        case AdjustmentTask.CheckRedundant:
                            //invDay = InventoryHelper.SaveInventoryDay(item.Item.Id, moveDate, 1, (int)ItemStatus.Redundant, dealerCode, item.Branchcode);
                            //if (invDay == null) { trs.IsValid = false; return AdjustmentErrorCode.SaveDataFailed; }
                            break;
                    }
                }
                catch { trs.IsValid = false; return AdjustmentErrorCode.SaveDataFailed; }

                // after all
                trs.IsValid = true;
            }
            return AdjustmentErrorCode.OK;
        }
        public static AdjustmentErrorCode CheckLacked(string enginNumber, string dealerCode, string dbCode, string bCode)
        {
            return UpdateVerhicle(enginNumber, DateTime.Now, bCode, "", dealerCode, dbCode, true, AdjustmentTask.CheckLacked);
        }
        public static AdjustmentErrorCode CheckRedundant(string enginNumber, string dealerCode, string dbCode, string bCode)
        {
            return UpdateVerhicle(enginNumber, DateTime.Now, bCode, "", dealerCode, dbCode, true, AdjustmentTask.CheckRedundant);
        }
        public static AdjustmentErrorCode MoveVerhicle(string enginNumber, DateTime moveDate, string from, string to, string dealerCode, string dbCode)
        {
            return UpdateVerhicle(enginNumber, moveDate, from, to, dealerCode, dbCode, true, AdjustmentTask.Move);
        }
        public static AdjustmentErrorCode AddVoucher(string enginNumber, string dealerCode, string dbCode, string bCode)
        {
            return UpdateVerhicle(enginNumber, DateTime.Now, bCode, "", dealerCode, dbCode, true, AdjustmentTask.AddVoucher);
        }

        public static AdjustmentErrorCode GetItemInfos(string engineNumber, string dealerCode, string dbCode, out Shippingdetail item)
        {
            IDao<Shippingdetail, long> dao = DaoFactory.GetDao<Shippingdetail, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Enginenumber", engineNumber) });
            item = null;
            IList list = dao.GetAll();

            if (list.Count != 1) return AdjustmentErrorCode.InvalidEngineNumber;
            Shippingdetail shd = (Shippingdetail)list[0];
            // .PRODUCTINSTANCE == null => not exist in stock; 
            if (shd.PRODUCTINSTANCE == null) return AdjustmentErrorCode.InvalidEngineNumber;
            // <> dealerCode => wrong dealer
            if ((CheckDealerCode) && (shd.PRODUCTINSTANCE.Dealercode.ToLower()) != dealerCode.ToLower()) return AdjustmentErrorCode.InvalidDealer;
            // region (database Code)
            if (shd.PRODUCTINSTANCE.Databasecode.ToLower() != dbCode.ToLower()) return AdjustmentErrorCode.InvalidDealer;
            // wrong item status
            if (!ItemHepler.IsInstock(shd.PRODUCTINSTANCE.Status)) return AdjustmentErrorCode.InvalidStatus;
            item = shd;
            return (AdjustmentErrorCode.OK);
        }
        public static Iteminstance GetItemInfos(string engineNumber, string dCode)
        {
            return GetItemInfos(engineNumber, dCode, null);
        }
        public static Iteminstance GetItemInfos(string engineNumber, string dCode, string wCode)
        {
            IDao<Iteminstance, long> dao = DaoFactory.GetDao<Iteminstance, long>();
            if (string.IsNullOrEmpty(wCode))
                dao.SetCriteria(new ICriterion[] { 
                    Expression.Eq("Enginenumber", engineNumber.ToUpper()), 
                    Expression.Eq("Dealercode", dCode) 
                });
            else
                dao.SetCriteria(new ICriterion[] { 
                    Expression.Eq("Enginenumber", engineNumber.ToUpper()), 
                    Expression.Eq("Branchcode", wCode), 
                    Expression.Eq("Dealercode", dCode) 
                });

            IList lst = dao.GetAll();
            //if (lst.Count > 1) throw new Exception("Two many result!");
            if (lst.Count <= 0) return null;

            return (Iteminstance)lst[0];
        }
        public static Shippingdetail GetShippingDetail(string engineNumber)
        {
            IDao<Shippingdetail, long> dao = DaoFactory.GetDao<Shippingdetail, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Enginenumber", engineNumber.ToUpper()) });
            dao.SetOrder(new NHibernate.Expression.Order[] { NHibernate.Expression.Order.Desc("Id") });
            IList lst = dao.GetPaged(0, 1);
            if (lst.Count <= 0) return null;
            return (Shippingdetail)lst[0];
        }
    }


}