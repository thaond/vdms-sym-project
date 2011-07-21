using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;
using VDMS.Helper;
using VDMS.I.Entity;
using VDMS.I.Linq;
using VDMS.II.Linq;
using Item = VDMS.Core.Domain.Item;
using ShippingHeader = VDMS.Core.Domain.ShippingHeader;

namespace VDMS.I.Vehicle
{
    public partial class CommonDAO
    {
        public static IList<Shippingdetail> GetShippingDetailByShippingNumber(decimal shippingID)
        {
            IDao<Shippingdetail, long> dao;
            List<Shippingdetail> list;
            try
            {
                dao = DaoFactory.GetDao<Shippingdetail, long>();
                dao.SetCriteria(new ICriterion[] { Expression.Eq("Shippingheader.Id", shippingID) });
            }
            catch (System.Exception e)
            {
                throw e;
            }
            list = dao.GetAll();
            return ((list != null) && (list.Count > 0)) ? list : null;
        }
        public static IList<Shippingdetail> GetShippingDetails(string engNo)
        {
            IDao<Shippingdetail, long> dao;
            List<Shippingdetail> list;
            dao = DaoFactory.GetDao<Shippingdetail, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Enginenumber", engNo) });
            list = dao.GetAll();
            return list;
           
        }
        public static IQueryable<ShippingDetail> GetShippingDetails(VehicleDataContext db, string engNo)
        {
            return db.ShippingDetails.Where(p => p.EngineNumber.Equals(engNo));
        }

        public static ShippingHeader GetShippingHeaderByShippingNumber(string shipNum)
        {
            IDao<ShippingHeader, long> dao;
            List<ShippingHeader> list;
            dao = DaoFactory.GetDao<ShippingHeader, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Shippingnumber", shipNum.Trim()) });
            list = dao.GetAll();
            return ((list != null) && (list.Count > 0)) ? list[0] : null;
        }

        public static Iteminstance GetItemInstance(string engineNumber)
        {
            IDao<Iteminstance, long> dao;
            List<Iteminstance> list;
            dao = DaoFactory.GetDao<Iteminstance, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Enginenumber", engineNumber.Trim()) });
            list = dao.GetAll();
            return ((list != null) && (list.Count > 0)) ? list[0] : null;
        }

        public static ShippingHeader SaveOrUpdateShippingHeader(string areaCode, string shipNumber, string shipTo, DateTime shipDate, string dealerCode, int itemCount, string createBy)
        {
            
            IDao<ShippingHeader, long> dao;
            List<ShippingHeader> list;
            ShippingHeader SH;

            dao = DaoFactory.GetDao<ShippingHeader, long>();
           dao.SetCriteria(new ICriterion[] { Expression.Eq("Shippingnumber", shipNumber.Trim()) });

            // check shippingNumber for existing;
            list = dao.GetAll();

            var db =DCFactory.GetDataContext<VehicleDataContext>();
            
                var query = (from sh in db.ShippingHeaders
                            where sh.ShippingNumber.Equals(shipNumber.Trim())
                            select new ShippingHeader
                                       {
                                           Areacode = sh.AreaCode,
                                           Createdby = sh.CreatedBy,
                                           Createddate = sh.CreatedDate,
                                           Dealercode = sh.DealerCode,
                                           Id = sh.ShippingId,
                                           Itemcount = sh.ItemCount,
                                           Shippingdate = sh.ShippingDate,
                                           Shippingnumber = sh.ShippingTo
                                       }).ToList();
                //if (query.Count() > 0) SH = (ShippingHeader)list[0];
                if (query.Count() > 0) SH = (ShippingHeader)query[0];
            else
            {
                SH = new ShippingHeader();
                SH.Createddate = DateTime.Now;
                SH.Createdby = createBy;
            }

            SH.Shippingnumber = shipNumber;
            SH.Shippingto = shipTo;
            SH.Shippingdate = shipDate;
            SH.Dealercode = dealerCode;
            SH.Areacode = areaCode;
            if (SH.Itemcount < itemCount) SH.Itemcount = itemCount;

            try
            {
                return dao.SaveOrUpdate(SH);
            }
            catch
            {
                return null;
            }


        }
        public static Entity.ShippingHeader SaveOrUpdateShippingHeader(VehicleDataContext db, string areaCode, string shipNumber, string shipTo, DateTime shipDate, string dealerCode, int itemCount, string createBy)
        {
                var query = db.ShippingHeaders.FirstOrDefault(p => p.ShippingNumber.Equals(shipNumber.Trim()));
                if (query == null)
                {
                    query = new Entity.ShippingHeader { CreatedDate = DateTime.Now, CreatedBy = createBy };
                    db.ShippingHeaders.InsertOnSubmit(query);
                }
                query.ShippingNumber = shipNumber;
                query.ShippingTo = shipTo;
                query.ShippingDate = shipDate;
                query.DealerCode = dealerCode;
                query.AreaCode = areaCode;
                if (query.ItemCount < itemCount) query.ItemCount = itemCount;
            return query;
        }

        public static Iteminstance SaveOrUpdateItemInstance(string dealerCode, string branchCode, string engineNumber, string shipNumber, string invoice, string itemType, Item item, DateTime impDate, string color, int status, DateTime madeDate, string DatabaseCode)
        {
            IDao<Iteminstance, long> dao;
            //List<Iteminstance> list;
            Iteminstance IIS = null;

            dao = DaoFactory.GetDao<Iteminstance, long>();

            dao.SetCriteria(new ICriterion[] { Expression.Eq("Enginenumber", engineNumber.Trim()) });
            //// khong cho phep de` du lieu xe cu
            //if (dao.GetCount() > 0)
            //{
            //    Exception e = new Exception("Enginenumber exist: " + engineNumber);
            //    throw e;
            //}

            List<Iteminstance> list = dao.GetAll();
            if (list.Count > 0)
                IIS = (Iteminstance)list[0];
            else
            {
                IIS = new Iteminstance();
                IIS.Createddate = DateTime.Now;
            }

            // get shipping header to take some data
            ShippingHeader SH = GetShippingHeaderByShippingNumber(shipNumber);
            if (SH == null)
            {
                Exception e = new Exception("Shipping number does not exist: " + shipNumber);
                throw e;
            }
            else
            {
                // save to database
                //IIS.Dealercode = SH.Dealercode;
                IIS.Enginenumber = engineNumber;
                IIS.Itemtype = itemType;
                IIS.Item = item;
                IIS.Importeddate = impDate;
                IIS.Color = color;
                IIS.Status = status;
                IIS.Dealercode = dealerCode;
                IIS.Vmepinvoice = invoice;
                IIS.Databasecode = DatabaseCode;
                if (madeDate > DateTime.MinValue) IIS.Madedate = madeDate;
                IIS.Branchcode = branchCode;

                IIS.Releaseddate = DateTime.MaxValue;

                dao.SaveOrUpdate(IIS);
            }
            return IIS;
        }

        public static ItemInstance SaveOrUpdateItemInstance(VehicleDataContext db, string dealerCode, string branchCode, string engineNumber, string shipNumber, string invoice, string itemType, Entity.Item item, DateTime impDate, string color, int status, DateTime madeDate, string DatabaseCode)
        {
            var IIS = db.ItemInstances.FirstOrDefault(p => p.EngineNumber.Equals(engineNumber.Trim()));

            if (IIS == null)
            {
                IIS = new ItemInstance { CreatedDate = DateTime.Now };
                IIS.EngineNumber = engineNumber;
                db.ItemInstances.InsertOnSubmit(IIS);
            }

            // get shipping header to take some data
            //var SH = db.ShippingHeaders.FirstOrDefault(p => p.ShippingNumber == (shipNumber.Trim()));
            //if (SH == null)
            //{
            //    Exception e = new Exception("Shipping number does not exist: " + shipNumber);
            //    throw e;
            //}
            //else
            //{
                // save to database
                //IIS.Dealercode = SH.Dealercode;
                
                IIS.ItemType = itemType;
                IIS.ItemCode = item.ItemCode;
                IIS.ImportedDate = impDate;
                IIS.Color = color;
                IIS.Status = status;
                IIS.DealerCode = dealerCode;
                IIS.VMEPInvoice = invoice;
                IIS.DatabaseCode = DatabaseCode;
                if (madeDate > DateTime.MinValue) IIS.MadeDate = madeDate;
                IIS.BranchCode = branchCode;

                IIS.ReleasedDate = DateTime.Parse("9999/12/31");
           // }
            return IIS;
        }

        public static Shippingdetail SaveOrUpdateShippingDetail(long shipId, Item item, string engineNumber, int status, bool voucher, string ex, Iteminstance itemInstance, string itemType, string color, string branchCode, string orderNumber)
        {
            IDao<Shippingdetail, long> dao;
            IDao<ShippingHeader, long> shdao;
            List<Shippingdetail> list;
            Shippingdetail SD = null;

            dao = DaoFactory.GetDao<Shippingdetail, long>();
            dao.SetCriteria(new ICriterion[] { Expression.And(Expression.Eq("Shippingheader.Id", shipId), Expression.Eq("Enginenumber", engineNumber.Trim())) });
            list = dao.GetAll();

            if (list.Count > 0) SD = (Shippingdetail)list[0];
            if (SD == null)
            {
                shdao = DaoFactory.GetDao<ShippingHeader, long>();
                SD = new Shippingdetail();
                SD.Shippingheader = shdao.GetById(shipId, false); //true -> false
                SD.Enginenumber = engineNumber;
            }

            SD.Itemtype = itemType;
            SD.Color = color;
            SD.Item = item;
            SD.Branchcode = branchCode;
            SD.Ordernumber = orderNumber;
            SD.Status = status;
            SD.Voucherstatus = voucher;
            if (itemInstance != null) SD.PRODUCTINSTANCE = itemInstance;
            //if (!string.IsNullOrEmpty(ex)) 
            SD.Exception = ex;

            dao.SaveOrUpdate(SD);
            return SD;
        }
        
        public static ShippingDetail SaveOrUpdateShippingDetail(VehicleDataContext db,long shipId, Entity.Item item, string engineNumber, int status, bool voucher, string ex, ItemInstance itemInstance, string itemType, string color, string branchCode, string orderNumber)
        {
            //IDao<Shippingdetail, long> dao;
            //IDao<ShippingHeader, long> shdao;
            //List<Shippingdetail> list;
            ShippingDetail SD = null;

            //dao = DaoFactory.GetDao<Shippingdetail, long>();
            //dao.SetCriteria(new ICriterion[] { Expression.And(Expression.Eq("Shippingheader.Id", shipId), Expression.Eq("Enginenumber", engineNumber.Trim())) });
            //list = dao.GetAll();
            SD =
                db.ShippingDetails.FirstOrDefault(
                    p => p.ShippingId == shipId && p.EngineNumber == engineNumber.Trim());
            
            if (SD == null)
            {
                
                SD = new ShippingDetail();
                SD.ShippingId = shipId;
                SD.EngineNumber = engineNumber;
                db.ShippingDetails.InsertOnSubmit(SD);
            }

            SD.ItemType = itemType;
            SD.Color = color;
            SD.Item = item;
            SD.BranchCode = branchCode;
            SD.OrderNumber = orderNumber;
            SD.Status = status;
            SD.VoucherStatus = voucher? 1:0;
            SD.ItemCode = item.ItemCode;
            SD.VMEPResponseDate = DateTime.MinValue;
            if (itemInstance != null) SD.ProductInstanceId = itemInstance.ItemInstanceId;
            //if (!string.IsNullOrEmpty(ex)) 
            SD.Exception = ex;

            return SD;
        }

        public static Item GetItemByCode(String itemCode)
        {
            IDao<Item, string> dao = DaoFactory.GetDao<Item, string>();
            return dao.GetById(itemCode, false); //true -> false
        }

        public static Entity.Item GetItemByCode(VehicleDataContext db, String itemCode)
        {
            return db.Items.FirstOrDefault(p => p.ItemCode.Equals(itemCode));
        }

        public static TransHis SaveTransHist(Iteminstance IInst, DateTime tranDate, ItemStatus status, long ActualCost, string dCode, string bCode)
        {
            IDao<TransHis, long> dao = DaoFactory.GetDao<TransHis, long>();
            TransHis transHis = new TransHis();
            transHis.Actualcost = ActualCost;
            transHis.Frombranch = "";
            transHis.Tobranch = string.Format("{0}-{1}", dCode, bCode);
            transHis.Modifieddate = DateTime.Now;
            transHis.Iteminstance = IInst;
            transHis.Transactiondate = tranDate;
            transHis.Transactiontype = (int)status;
            transHis.Modifiedby = UserHelper.Username;
            try
            {
                return dao.Save(transHis);
            }
            catch { return null; }
        }
        public static Entity.SaleTransHistory SaveTransHist(VehicleDataContext db, ItemInstance IInst, DateTime tranDate, ItemStatus status, long ActualCost, string dCode, string bCode)
        {
            
            var transHis = new SaleTransHistory
                               {
                                   ActualCost = ActualCost,
                                   FromBranch = "",
                                   ToBranch = string.Format("{0}-{1}", dCode, bCode),
                                   ModifiedDate = DateTime.Now,
                                   ItemInstance = IInst,
                                   TransactionDate = tranDate,
                                   TransactionType = (int) status,
                                   ModifiedBy = UserHelper.Username
                               };
            db.SaleTransHistories.InsertOnSubmit(transHis);
            return transHis;
        }
    }
}