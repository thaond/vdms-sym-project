using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using NHibernate;
using NHibernate.Expression;
using Resources;
using VDMS.Common.Utils;
using VDMS.Common.Web;
using VDMS.Common.Web.Validator;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.DAL2;
using VDMS.Data.IDAL.Interface;
using VDMS.Data.TipTop;
using VDMS.Helper;
using VDMS.I.Entity;
using VDMS.I.Linq;
using VDMS.I.ObjectDataSource;
using VDMS.I.Vehicle;
using VDMS.II.Linq;
using Item = VDMS.I.Entity.Item;
using ShippingDetail = Resources.ShippingDetail;
using ShippingHeader = VDMS.Core.Domain.ShippingHeader;

public partial class Sales_Inventory_Import : BasePage
{
    public bool IsTesting
    {
        get
        {
            return
                false;
            //true  || 
            //Request.Url.Host.Equals("tiger", StringComparison.OrdinalIgnoreCase) || (Request.IsLocal && (Request.Url.Port > 0));
        }
    }

    protected string QueryShipNo { get { return Request.QueryString["sn"]; } }
    protected string QueryOrderNo { get { return Request.QueryString["on"]; } }

    private string ShippingAddress = "";
    // store imported date for vehicles imported successfuly
    private Hashtable importedDate;

    private string logedAreaCode, clientScript,
        //corect client control state => valid require exception behaviour
        clientFunc;

    private const string strUnknown = "<UnKnown>";

    private const string VS_ShippingNumber = "ShippingNumber";  // viewstate vars
    private const string VS_ShipID = "ShipID";
    private const string VS_BranchCode = "BranchCode";
    private const string VS_BaseShipDate = "ShipDate";              // base ship date
    private const string VS_ExportDate = "ExportDate";

    // variables store information during process
    private Collection<ImportErrorCode> errorCode = new Collection<ImportErrorCode>();

    private bool onTipTop = false;
    //private const int ERC_INVALID_ISSUED_NUMBER = 1;
    //private const int ERC_UPDATE_FAILED = 2;
    //private const int ERC_ITEM_NOT_EXIST = 3;
    //private const int ERC_OK = 0;


    //private void ShowError()
    //{
    //    bllError.Items.Clear();
    //    switch (errorCode)
    //    {
    //        case ERC_INVALID_ISSUED_NUMBER: bllError.Items.Add(Message.Shipping_InvalidExportInvoiceNumber); break;
    //        case ERC_UPDATE_FAILED: bllError.Items.Add(Message.Shipping_UpdateFailed); break;
    //        case ERC_ITEM_NOT_EXIST: bllError.Items.Add(Message.Shipping_ItemNotExit); break;
    //    }
    //}

    private void ShowError()
    {
        bllError.Visible = errorCode.Count > 0;
        bllError.Items.Clear();
        foreach (ImportErrorCode error in errorCode)
        {
            switch (error)
            {
                case ImportErrorCode.ImportDateLessThanBaseDate: bllError.Items.Add(string.Format(ShippingDetail.ImportDateMustGreaterThanBaseDate, ((DateTime)ViewState[VS_BaseShipDate]).ToShortDateString())); break;
                case ImportErrorCode.ImportDateLocked: bllError.Items.Add(ShippingDetail.ImportDateLocked); break;
                case ImportErrorCode.ImportDateTooLate: bllError.Items.Add(ShippingDetail.ImportDateTooLate); break;
                case ImportErrorCode.InvalidImportDate: bllError.Items.Add(ShippingDetail.InvalidImportDate); break;
                case ImportErrorCode.InvalidIssuedNumber: bllError.Items.Add(Message.Shipping_InvalidExportInvoiceNumber); break;
                case ImportErrorCode.ItemNotExist: bllError.Items.Add(Message.Shipping_ItemNotExit); break;
                case ImportErrorCode.UpdateFailed: bllError.Items.Add(Message.Shipping_UpdateFailed); break;
                case ImportErrorCode.OrdersDoesNotConfirmed: bllError.Items.Add(Message.Shipping_OrdersDoesNotConfirmed); break;
            }
        }
    }
    protected void AddError(ImportErrorCode error)
    {
        if ((error != ImportErrorCode.Ok) && (!errorCode.Contains(error)))
        {
            errorCode.Add(error);
        }
    }
    private DateTime CalculateInitImportDate(DateTime baseImpDate)
    {
        DateTime result;
        int month = baseImpDate.Month, year = baseImpDate.Year;

        if ((month == DateTime.Now.Month) && (year == DateTime.Now.Year))
        {
            result = DateTime.Now;
        }
        else
        {
            if (month == 12)
            {
                month = 0; year++;
            }
            result = new DateTime(year, month + 1, 1).AddDays(-1);
        }

        return result;
    }

    // get data to display
    private ShippingHeader VDMS_ShippingHeader(string shipNumber)
    {
        IDao<ShippingHeader, long> dao = DaoFactory.GetDao<ShippingHeader, long>();
        dao.SetCriteria(new ICriterion[] { 
            Expression.Eq("Shippingnumber", shipNumber), 
            //Expression.In("Areacode", AreaHelper.Area), 
            Expression.Eq("Dealercode", UserHelper.DealerCode) });
        IList list = dao.GetAll();
        return ((list != null) && (list.Count > 0)) ? (ShippingHeader)list[0] : null;
    }

    private List<VDMS.I.Entity.IShippingDetail> TipTop_ShippingDetail(string shipNumber)
    {
        //string oNumber = txtOrderNumber.Text.Trim().ToUpper();
        //return (string.IsNullOrEmpty(oNumber)) ? Shipping.GetShippingDetail(shipNumber).Tables[0] : Shipping.GetShippingDetail(shipNumber, oNumber).Tables[0];

        var dc = DCFactory.GetDataContext<VehicleDataContext>();
        List<VDMS.I.Entity.IShippingDetail> res = dc.IShippingDetails.Where(d => d.IssueNumber == shipNumber).ToList();
        return res;
    }

    private VDMS.I.Entity.IShippingHeader TipTop_ShippingHeader(string shipNumber)
    {
        //return Shipping.GetShippingHeader(shipNumber, UserHelper.DealerCode);
        var dc = DCFactory.GetDataContext<VehicleDataContext>();
        return dc.IShippingHeaders.SingleOrDefault(h => h.IssueNumber == shipNumber && h.DealerCode == UserHelper.DealerCode);
    }

    /// <summary>
    /// Leo-mvbinh: this function allow change brandcode(warehouse)
    /// </summary>
    /// <param name="shipNumber"></param>
    /// <param name="branchcode"></param>
    private void UpdateBrandCode(string shipNumber, string branchcode)
    {
        if (!string.IsNullOrEmpty(shipNumber))
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            foreach (IShippingDetail i in dc.IShippingDetails.Where(h => h.IssueNumber == shipNumber))
            {
                i.BranchCode = branchcode;
            }
            dc.IShippingHeaders.SingleOrDefault(h => h.IssueNumber == shipNumber).BranchCode = branchcode;
            dc.SubmitChanges();
            IDao<ShippingHeader, long> dao = DaoFactory.GetDao<ShippingHeader, long>();
            dao.SetCriteria(new ICriterion[] { 
            Expression.Eq("Shippingnumber", shipNumber), 
            //Expression.In("Areacode", AreaHelper.Area), 
            Expression.Eq("Dealercode", UserHelper.DealerCode) });
            IList list = dao.GetAll();
            if (list != null && list.Count > 0)
            {
                ShippingHeader ShippingHeaderUpdated = (ShippingHeader)list[0];
                ShippingHeaderUpdated.Shippingto = branchcode;
                dao.SaveOrUpdate(ShippingHeaderUpdated);
            }
        }
    }

    private ShippingHeader GetShippingHeader(string shipNum, out bool onTipTop)
    {
        string shipNumber = shipNum.ToUpper().Trim();

        // get shipheader from VDMS
        ShippingHeader SH = VDMS_ShippingHeader(shipNumber);
        VDMS.I.Entity.IShippingHeader drSH = TipTop_ShippingHeader(shipNumber);

        if (SH == null)  // not found in VDMS
        {
            onTipTop = drSH != null;
            if (!onTipTop) return null;

            // copy drSH to SH ....
            SH = new ShippingHeader();
            SH.Shippingnumber = drSH.IssueNumber;   //drSH["ShippingNumber"].ToString();
            SH.Shippingdate = drSH.ShipDate;        // Convert.ToDateTime(drSH["ShippingDate"].ToString());
            SH.Itemcount = 0;  // Convert.ToInt32(drSH["ItemCount"].ToString());
            SH.Shippingto = drSH.BranchCode;// drSH["ShippingTo"].ToString();
            ShippingAddress = drSH.ShippingAddress; //["ShippingAddress"].ToString();
            SH.Dealercode = drSH.DealerCode;        // drSH["DealerCode"].ToString();
            ViewState[VS_BranchCode] = drSH.BranchCode;// drSH["BranchCode"].ToString();
        }
        else    // already exist in VDMS
        {
            onTipTop = false;
            if (drSH != null)
            {
                ViewState[VS_BranchCode] = drSH.BranchCode; // drSH["BranchCode"].ToString();
            }
        }

        // after all: save infos and return
        ViewState[VS_ShippingNumber] = SH.Shippingnumber;
        ViewState[VS_ShipID] = SH.Id;
        ViewState[VS_ExportDate] = drSH.ShipDate;   // drSH["ShippingDate"];
        ViewState[VS_BaseShipDate] = drSH.ShipDate; //drSH["ShippingDate"];

        if ((DateTime)ViewState[VS_BaseShipDate] > DateTime.Now)
            Validator.SetDateRange(rvImportDate, DateTime.Now, DateTime.Now, true);
        else
            Validator.SetDateRange(rvImportDate, (DateTime)ViewState[VS_BaseShipDate], DateTime.Now, true);

        return SH;
    }

    private bool SaveShippingDetail2()
    {
        string exception, shipNumber, itemCode, engineNumber, itemType, color, orderNumber, shipTo, branchCode;
        Int32 status;
        bool hasVoucher;
        ImportItemStatus IisStatus;
        Iteminstance IInst = null;
        ItemStatus hisItemStatus;
        DateTime madeDate, impDate, itemImpDate;
        Shippingdetail SD;
        long Price;

        VDMS.Core.Domain.Item item = null;
        long shipID;
        ISession sess = NHibernateSessionManager.Instance.GetSession();

        using (TransactionBlock trans = new TransactionBlock())
        {
            try
            {
                if ((!DateTime.TryParse(txtImportDate.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out impDate)) || (impDate > DateTime.Now))
                {
                    //impDate = DateTime.Now;
                    AddError(ImportErrorCode.InvalidImportDate);
                    return false;
                }

                DateTime baseImportDate = (DateTime)ViewState[VS_BaseShipDate];
                if (impDate < baseImportDate)
                {
                    AddError(ImportErrorCode.ImportDateLessThanBaseDate);
                    return false;
                }

                shipNumber = ViewState[VS_ShippingNumber].ToString();
                branchCode = ViewState[VS_BranchCode].ToString();
                shipTo = hdAddress.Value;

                // save Shipping header
                ShippingHeader SH = CommonDAO.SaveOrUpdateShippingHeader(logedAreaCode, shipNumber, shipTo, impDate, UserHelper.DealerCode, (GridView1.Rows.Count + GridView3.Rows.Count), UserHelper.Username);
                if (SH == null) { trans.IsValid = false; return false; }
                else shipID = SH.Id;

                #region save shipping
                foreach (GridViewRow row in GridView3.Rows)
                {
                    if (row.Enabled) // skip error vehicle
                    {
                        exception = ((TextBox)row.FindControl("txtException")).Text.Trim();
                        Int32.TryParse(((RadioButtonList)row.FindControl("rblStatus")).SelectedValue, out status);
                        hasVoucher = ((CheckBox)row.FindControl("chbVoucherStatus")).Checked;
                        itemCode = ((Label)row.FindControl("lblItemCode")).Text.Trim();
                        engineNumber = ((Label)row.FindControl("lblEngineNumber")).Text;
                        itemType = ((Label)row.FindControl("lblItemType")).Text;
                        color = ((Label)row.FindControl("lblColor")).Text + " (" + ((Label)row.FindControl("lblColorName")).Text + ")";
                        orderNumber = ((Label)row.FindControl("lblOrderNumber")).Text;
                        DateTime.TryParse(((Label)row.FindControl("lblMadeDate")).Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out madeDate);
                        long.TryParse(((Label)row.FindControl("lblPrice")).Text, out Price);

                        // custom imported date for item
                        string impDateString = ((TextBox)row.FindControl("txtItemImportDate")).Text.Trim();
                        if (string.IsNullOrEmpty(impDateString)) impDateString = txtImportDate.Text;

                        if (!DateTime.TryParse(impDateString, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out itemImpDate))
                        {
                            AddError(ImportErrorCode.InvalidImportDate);
                        }

                        if (itemImpDate < baseImportDate) AddError(ImportErrorCode.ImportDateLessThanBaseDate);
                        if (itemImpDate > DateTime.Now) AddError(ImportErrorCode.ImportDateTooLate);
                        if (InventoryHelper.IsInventoryLock(itemImpDate, UserHelper.DealerCode, UserHelper.BranchCode)) AddError(ImportErrorCode.ImportDateLocked);
                        if (errorCode.Count > 0)
                        {
                            trans.IsValid = false; return false;
                        }

                        //branchCode = ((Label)row.FindControl("lblBranchCode")).Text;

                        // day du cac thu roi thi khong chap nhan exception
                        if ((hasVoucher) && (status == 1)) exception = string.Empty;

                        IInst = null;
                        // get n' check item in table DATA_ITEM
                        item = CommonDAO.GetItemByCode(itemCode);
                        if (item == null) { AddError(ImportErrorCode.ItemNotExist); return false; }

                        if (status > 0) // nhap xe hoac tam nhap => save iteminstance n' transHistory
                        {
                            // clear instance for old shipping info
                            foreach (var sd in CommonDAO.GetShippingDetails(engineNumber))
                            {
                                sd.PRODUCTINSTANCE = null;
                            }
                            // save ItemInstance of shipping
                            switch (status)
                            {
                                case 0: IisStatus = ImportItemStatus.NotArrived; break;
                                case 1: IisStatus = ImportItemStatus.Imported; break;
                                case 2: IisStatus = ImportItemStatus.AdmitTemporarily; break;
                                default: IisStatus = ImportItemStatus.NotArrived; break;
                            }
                            IInst = CommonDAO.SaveOrUpdateItemInstance(UserHelper.DealerCode, branchCode, engineNumber, shipNumber, orderNumber, itemType, item, itemImpDate, color, (int)IisStatus, madeDate, UserHelper.DatabaseCode);
                            if (IInst == null) { trans.IsValid = false; return false; }

                            // save transaction history. <actualCost> is temporary equal to "zero"
                            switch (status)
                            {
                                case 0: hisItemStatus = ItemStatus.Lacked; break;
                                case 1: hisItemStatus = ItemStatus.Imported; break;
                                case 2: hisItemStatus = ItemStatus.AdmitTemporarily; break;
                                default: hisItemStatus = ItemStatus.AdmitTemporarily; break;
                            }
                            if (CommonDAO.SaveTransHist(IInst, itemImpDate, hisItemStatus, Price, UserHelper.DealerCode, UserHelper.BranchCode) == null) { trans.IsValid = false; return false; };

                            // save to Inventory of Day
                            if (InventoryHelper.SaveInventoryDay(itemCode, itemImpDate, 1, (int)IisStatus, UserHelper.DealerCode, branchCode) == null) { trans.IsValid = false; return false; };
                        }

                        // save shipping detail info
                        SD = CommonDAO.SaveOrUpdateShippingDetail(shipID, item, engineNumber, status, hasVoucher, exception, IInst, itemType, color, UserHelper.DealerCode, orderNumber);
                        if (SD == null) { trans.IsValid = false; return false; }
                    }
                }
                #endregion

                #region Update order delivered status

                foreach (GridViewRow row in GridView3.Rows)
                {
                    orderNumber = ((Label)row.FindControl("lblOrderNumber")).Text;
                    /* tntung
                     * 14/01/2008
                     * Update order delivered status
                     */
                    IDao<Orderheader, long> oDao = DaoFactory.GetDao<Orderheader, long>();
                    oDao.SetCriteria(new ICriterion[] { Expression.Eq("Ordernumber", orderNumber) });
                    IList list = oDao.GetAll();
                    if (list.Count > 0)
                    {
                        Orderheader OrderHeaderUpdated = (Orderheader)list[0];
                        DataSet ds = InventoryDao.CheckOrderDetail(OrderHeaderUpdated.Id);
                        int Orderstatus = (int)DeliveredOrderStatus.DeliveredAll;
                        int Orderqty, OrderShipped;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            item = CommonDAO.GetItemByCode(dr["itemcode"].ToString());
                            Orderqty = int.Parse(dr["orderqty"].ToString());
                            IDao<Shippingdetail, long> sDao = DaoFactory.GetDao<Shippingdetail, long>();
                            sDao.SetCriteria(new ICriterion[] {
                                Expression.Eq("Ordernumber", orderNumber),
                                Expression.Eq("Item", item),
                                Expression.Not(Expression.Eq("Status", (int)ItemStatus.NotArrived))
                            });
                            OrderShipped = sDao.GetAll().Count;
                            if ((Orderqty - OrderShipped) != 0)
                            {
                                Orderstatus = (int)DeliveredOrderStatus.NotDeliveredAll;
                            }
                        }
                        OrderHeaderUpdated.Deliveredstatus = Orderstatus;   //  (Orderstatus.Equals((int)DeliveredOrderStatus.NotDeliveredAll)) ? (int)DeliveredOrderStatus.NotDeliveredAll : (int)DeliveredOrderStatus.DeliveredAll;
                        oDao.SaveOrUpdate(OrderHeaderUpdated);
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                trans.IsValid = false;
                return false;
            }
            trans.IsValid = true;

        }  // transaction
        return true;
    }

    private bool SaveShippingDetail()
    {
        string exception, shipNumber, itemCode, engineNumber, itemType, color, orderNumber, shipTo, branchCode;
        Int32 status;
        bool hasVoucher;
        ImportItemStatus IisStatus;
        ItemInstance IInst = null;
        ItemStatus hisItemStatus;
        DateTime madeDate, impDate, itemImpDate;
        VDMS.I.Entity.ShippingDetail SD;
        long Price;

        Item item = null;
        long shipID;

        using (var db = new VehicleDataContext())
        {
            //try
            //{
            System.Data.Common.DbTransaction transaction;
            db.Connection.Open();
            transaction = db.Connection.BeginTransaction();
            db.Transaction = transaction;
            try
            {
                if (
                   (!DateTime.TryParse(txtImportDate.Text, new CultureInfo(UserHelper.Language),
                                       DateTimeStyles.AllowWhiteSpaces, out impDate)) || (impDate > DateTime.Now))
                {
                    //impDate = DateTime.Now;
                    AddError(ImportErrorCode.InvalidImportDate);
                    return false;
                }

                DateTime baseImportDate = (DateTime)ViewState[VS_BaseShipDate];
                if (impDate < baseImportDate)
                {
                    AddError(ImportErrorCode.ImportDateLessThanBaseDate);
                    return false;
                }
                foreach (GridViewRow row in GridView3.Rows)
                {
                    if (row.Enabled) // skip error vehicle
                    {
                        exception = ((TextBox)row.FindControl("txtException")).Text.Trim();
                        Int32.TryParse(((RadioButtonList)row.FindControl("rblStatus")).SelectedValue, out status);
                        hasVoucher = ((CheckBox)row.FindControl("chbVoucherStatus")).Checked;
                        itemCode = ((Label)row.FindControl("lblItemCode")).Text.Trim();
                        engineNumber = ((Label)row.FindControl("lblEngineNumber")).Text;
                        itemType = ((Label)row.FindControl("lblItemType")).Text;
                        color = ((Label)row.FindControl("lblColor")).Text + " (" +
                                ((Label)row.FindControl("lblColorName")).Text + ")";
                        orderNumber = ((Label)row.FindControl("lblOrderNumber")).Text;
                        DateTime.TryParse(((Label)row.FindControl("lblMadeDate")).Text,
                                          Thread.CurrentThread.CurrentCulture,
                                          DateTimeStyles.AllowWhiteSpaces, out madeDate);
                        long.TryParse(((Label)row.FindControl("lblPrice")).Text, out Price);

                        // custom imported date for item
                        string impDateString = ((TextBox)row.FindControl("txtItemImportDate")).Text.Trim();
                        if (string.IsNullOrEmpty(impDateString)) impDateString = txtImportDate.Text;

                        if (
                            !DateTime.TryParse(impDateString, Thread.CurrentThread.CurrentCulture,
                                               DateTimeStyles.AllowWhiteSpaces, out itemImpDate))
                        {
                            AddError(ImportErrorCode.InvalidImportDate);
                        }

                        if (itemImpDate < baseImportDate) AddError(ImportErrorCode.ImportDateLessThanBaseDate);
                        if (itemImpDate > DateTime.Now) AddError(ImportErrorCode.ImportDateTooLate);
                        if (InventoryHelper.IsInventoryLock(itemImpDate, UserHelper.DealerCode, UserHelper.BranchCode))
                            AddError(ImportErrorCode.ImportDateLocked);
                        if (errorCode.Count > 0)
                        {
                            return false;
                        }

                        //branchCode = ((Label)row.FindControl("lblBranchCode")).Text;

                        // day du cac thu roi thi khong chap nhan exception
                        if ((hasVoucher) && (status == 1)) exception = string.Empty;

                        IInst = null;
                        // get n' check item in table DATA_ITEM
                        item = CommonDAO.GetItemByCode(db, itemCode);
                        if (item == null)
                        {
                            AddError(ImportErrorCode.ItemNotExist);
                            return false;
                        }
                    }
                }
                shipNumber = ViewState[VS_ShippingNumber].ToString();
                branchCode = ViewState[VS_BranchCode].ToString();
                shipTo = hdAddress.Value;

                // save Shipping header
                VDMS.I.Entity.ShippingHeader SH = CommonDAO.SaveOrUpdateShippingHeader(db, logedAreaCode, shipNumber,
                                                                                       shipTo,
                                                                                       impDate, UserHelper.DealerCode,
                                                                                       (GridView1.Rows.Count +
                                                                                        GridView3.Rows.Count),
                                                                                       UserHelper.Username);
                db.SubmitChanges();

                if (SH == null)
                {
                    return false;
                }
                shipID = db.ShippingHeaders.SingleOrDefault(p => p.ShippingNumber == shipNumber).ShippingId;

                #region save shipping

                foreach (GridViewRow row in GridView3.Rows)
                {
                    if (row.Enabled) // skip error vehicle
                    {
                        exception = ((TextBox)row.FindControl("txtException")).Text.Trim();
                        Int32.TryParse(((RadioButtonList)row.FindControl("rblStatus")).SelectedValue, out status);
                        hasVoucher = ((CheckBox)row.FindControl("chbVoucherStatus")).Checked;
                        itemCode = ((Label)row.FindControl("lblItemCode")).Text.Trim();
                        engineNumber = ((Label)row.FindControl("lblEngineNumber")).Text;
                        itemType = ((Label)row.FindControl("lblItemType")).Text;
                        color = ((Label)row.FindControl("lblColor")).Text + " (" +
                                ((Label)row.FindControl("lblColorName")).Text + ")";
                        orderNumber = ((Label)row.FindControl("lblOrderNumber")).Text;
                        DateTime.TryParse(((Label)row.FindControl("lblMadeDate")).Text,
                                          Thread.CurrentThread.CurrentCulture,
                                          DateTimeStyles.AllowWhiteSpaces, out madeDate);
                        long.TryParse(((Label)row.FindControl("lblPrice")).Text, out Price);

                        // custom imported date for item
                        string impDateString = ((TextBox)row.FindControl("txtItemImportDate")).Text.Trim();
                        if (string.IsNullOrEmpty(impDateString)) impDateString = txtImportDate.Text;

                        if (
                            !DateTime.TryParse(impDateString, Thread.CurrentThread.CurrentCulture,
                                               DateTimeStyles.AllowWhiteSpaces, out itemImpDate))
                        {
                            AddError(ImportErrorCode.InvalidImportDate);
                        }

                        if (itemImpDate < baseImportDate) AddError(ImportErrorCode.ImportDateLessThanBaseDate);
                        if (itemImpDate > DateTime.Now) AddError(ImportErrorCode.ImportDateTooLate);
                        if (errorCode.Count > 0)
                        {
                            return false;
                        }

                        //branchCode = ((Label)row.FindControl("lblBranchCode")).Text;

                        // day du cac thu roi thi khong chap nhan exception
                        if ((hasVoucher) && (status == 1)) exception = string.Empty;
                        item = CommonDAO.GetItemByCode(db, itemCode);

                        if (status > 0) // nhap xe hoac tam nhap => save iteminstance n' transHistory
                        {
                            // clear instance for old shipping info
                            //foreach (var sd in CommonDAO.GetShippingDetails(db, engineNumber))
                            //{
                            //    sd.ProductInstanceId = null;
                            //}
                            // save ItemInstance of shipping
                            switch (status)
                            {
                                case 0:
                                    IisStatus = ImportItemStatus.NotArrived;
                                    break;
                                case 1:
                                    IisStatus = ImportItemStatus.Imported;
                                    break;
                                case 2:
                                    IisStatus = ImportItemStatus.AdmitTemporarily;
                                    break;
                                default:
                                    IisStatus = ImportItemStatus.NotArrived;
                                    break;
                            }
                            IInst = CommonDAO.SaveOrUpdateItemInstance(db, UserHelper.DealerCode, branchCode,
                                                                       engineNumber,
                                                                       shipNumber, orderNumber, itemType, item,
                                                                       itemImpDate,
                                                                       color, (int)IisStatus, madeDate,
                                                                       UserHelper.DatabaseCode);
                            db.SubmitChanges();

                            if (IInst == null)
                            {
                                return false;
                            }

                            // save transaction history. <actualCost> is temporary equal to "zero"
                            switch (status)
                            {
                                case 0:
                                    hisItemStatus = ItemStatus.Lacked;
                                    break;
                                case 1:
                                    hisItemStatus = ItemStatus.Imported;
                                    break;
                                case 2:
                                    hisItemStatus = ItemStatus.AdmitTemporarily;
                                    break;
                                default:
                                    hisItemStatus = ItemStatus.AdmitTemporarily;
                                    break;
                            }
                            var t = CommonDAO.SaveTransHist(db, IInst, itemImpDate, hisItemStatus, Price,
                                                            UserHelper.DealerCode, UserHelper.BranchCode);

                            // save to Inventory of Day
                            var tt = InventoryHelper.SaveInventoryDay(db, itemCode, itemImpDate, 1, (int)IisStatus,
                                                                      UserHelper.DealerCode, branchCode);

                        }

                        // save shipping detail info
                        SD = CommonDAO.SaveOrUpdateShippingDetail(db, shipID, item, engineNumber, status,
                                                                  hasVoucher,
                                                                  exception, IInst, itemType, color,
                                                                  UserHelper.DealerCode,
                                                                  orderNumber);
                        //db.SubmitChanges();
                    }
                }

                #endregion
                #region Update order delivered status
                List<String> listOrderNumber = new List<string>();
                foreach (GridViewRow row in GridView3.Rows)
                {
                    listOrderNumber.Add(((Label)row.FindControl("lblOrderNumber")).Text);
                }

                List<VDMS.I.Entity.ShippingDetail> loh = db.ShippingDetails.Where(p => listOrderNumber.Contains(p.OrderNumber)).ToList();

                foreach (GridViewRow row in GridView3.Rows)
                {
                    orderNumber = ((Label)row.FindControl("lblOrderNumber")).Text;
                    /* tntung
                     * 14/01/2008
                     * Update order delivered status
                     */
                    //IDao<Orderheader, long> oDao = DaoFactory.GetDao<Orderheader, long>();
                    //oDao.SetCriteria(new ICriterion[] {Expression.Eq("Ordernumber", orderNumber)});
                    var list = db.OrderHeaders.FirstOrDefault(p => p.OrderNumber == orderNumber); //oDao.GetAll();)

                    if (list != null)
                    {
                        DataSet ds = InventoryDao.CheckOrderDetail(list.OrderHeaderId);
                        int Orderstatus = (int)DeliveredOrderStatus.DeliveredAll;
                        int Orderqty, OrderShipped;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            item = CommonDAO.GetItemByCode(db, dr["itemcode"].ToString());
                            Orderqty = int.Parse(dr["orderqty"].ToString());

                            OrderShipped =
                                loh.Count(
                                    p =>
                                    p.OrderNumber == orderNumber && p.ItemCode == item.ItemCode &&
                                    p.Status == (int)ItemStatus.Imported);

                            if ((Orderqty - OrderShipped) != 0)
                            {
                                Orderstatus = (int)DeliveredOrderStatus.NotDeliveredAll;
                            }
                        }
                        list.DeliveredStatus = Orderstatus;
                        //  (Orderstatus.Equals((int)DeliveredOrderStatus.NotDeliveredAll)) ? (int)DeliveredOrderStatus.NotDeliveredAll : (int)DeliveredOrderStatus.DeliveredAll;
                    }
                }
                db.SubmitChanges();
                transaction.Commit();
                //trans.Complete();
                #endregion

            }
            catch
            {
                transaction.Rollback();
                return false;
            }
            finally
            {
                if (db.Connection != null)
                {
                    db.Connection.Close();
                }
            }


            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.Message);
            //    return false;
            //}

        } // transaction
        return true;
    }

    // event section
    protected void Page_Load(object sender, EventArgs e)
    {
        errorCode.Clear();
        phImported.Visible = false;
        phNotImported.Visible = false;

        litTxtList.Text = "";
        //logedDealerCode = (UserHelper.DealerCode != "") ? UserHelper.DealerCode : strUnknown;
        logedAreaCode = (UserHelper.AreaCode != "") ? UserHelper.AreaCode : strUnknown;

        clientScript = "<script language=\"javascript\" type=\"text/javascript\">\n <!--\n var txtList=new Array( ";
        clientFunc = "var checkFuncList=new Array( ";
        txtExportInvoice.Attributes.Add("onkeydown", "Import_SetFocus(event);");

        //txtExportInvoice.Attributes.Add("btnTest", btnTest.ClientID);

        //if ((!IsPostBack) || IsChangeLanguage) txtImportDate.Text = DateTime.Now.ToShortDateString();// ("dd/MM/yyyy");

        if (!IsPostBack)
        {

            ///////////////// for testing ///////////////////
            GridView2.Visible = this.IsTesting;
            if (GridView2.Visible)
            {
                DataSet ds = Shipping.GetShippingHeader(UserHelper.DealerCode);
                GridView2.DataSource = ds;
                int a = ds.Tables[0].Rows.Count;
                GridView2.DataBind();
            }
            ////////////////////////////////////
            txtImportDate.Text = DateTime.Now.ToShortDateString();
            txtExportInvoice.Text = QueryShipNo;
            txtOrderNumber.Text = QueryOrderNo;
            if (!string.IsNullOrEmpty(QueryShipNo)) btnTest_Click(null, null);
            Selectddlwarehouselist();
        }
        else
        {
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ShowError();
        //if (IsChangeLanguage && (MultiView2.ActiveViewIndex > -1)) btnTest_Click(sender, e);
    }

    protected void btnTest_Click(object sender, EventArgs e)
    {
        // default disable (enabled by change vehicle status event)
        // cannot set (validators don't work): btnAccept.Enable = false; 
        btnAccept.Attributes["disabled"] = "disabled";
        btnAccept.Visible = true;
        GridView3.Enabled = true;

        txtExportInvoice.Text = txtExportInvoice.Text.ToUpper();
        string shippingNumber = txtExportInvoice.Text.Trim();

        lblAddress.Text = "";
        lblQuantityImport.Text = "";
        txtImportDate.Enabled = false;
        imgbCarlendar.Visible = false;

        //IFShipping.RefreshShipping(txtExportInvoice.Text, UserHelper.DealerCode);

        ShippingHeader SH = GetShippingHeader(shippingNumber, out onTipTop);

        // khong ton tai phieu xuat tai bat cu dau hoac danh cho dai ly khac
        // check theo Branch hoac theo Dealer
        if ((SH == null) ||
            ((ViewState[VS_BranchCode] != null) && (ViewState[VS_BranchCode].ToString().ToLower() != UserHelper.BranchCode.ToLower()) && !(UserHelper.IsAdmin || UserHelper.IsSysAdmin)) ||
            (SH.Dealercode.ToLower() != UserHelper.DealerCode.ToLower())
           )
        {
            var tth = Shipping.GetShippingHeader(txtExportInvoice.Text, UserHelper.DealerCode);
            if (tth == null) { AddError(ImportErrorCode.InvalidIssuedNumber); return; }
            else
            {
                AddError(ImportErrorCode.OrdersDoesNotConfirmed);
                GridView3.DataSource = Shipping.GetShippingDetail(txtExportInvoice.Text);
                GridView3.DataBind();
                phNotImported.Visible = true;
                GridView3.Enabled = false;
                btnAccept.Visible = false;
                return;
            }
        }


        if (onTipTop)
        {
            #region On tiptop
            //txtImportDate.Text = DateTime.Now.ToShortDateString();
            GridView3.DataSource = TipTop_ShippingDetail(SH.Shippingnumber);
            GridView3.DataBind();
            phNotImported.Visible = true;
            btnAccept.Enabled = true;
            txtImportDate.Enabled = true;
            imgbCarlendar.Visible = true;

            if (!IsAllOrderConfirmed && !this.IsTesting)
            {
                GridView3.Enabled = false;
                lblOrderNotConfirm.Visible = true;
                btnAccept.Enabled = false;
            }
            #endregion
            lblAddress.Text = ShippingAddress;
            ddlwarehouselist.Visible = true;

        }
        else
        {
            #region Not On tiptop

            string oNumber = txtOrderNumber.Text.Trim().ToUpper();

            List<VDMS.I.Entity.IShippingDetail> notArrivedVehicles = TipTop_ShippingDetail(SH.Shippingnumber);
            ShippingDetailDataSource dsShip = new ShippingDetailDataSource();
            List<VDMS.I.Entity.ShippingDetail> items = Select(SH.Id);
            //if (!string.IsNullOrEmpty(oNumber)) items = items.Where(i => i.OrderNumber == oNumber).ToList();
            if (!string.IsNullOrEmpty(oNumber)) items = items.ToList();
            // chua nhap kho
            IList<VDMS.I.Entity.ShippingDetail> failedItems = items.Where(i => i.Status == (int)ImportItemStatus.NotArrived).ToList();
            // da nhap kho
            items = items.Where(i => i.Status == (int)ImportItemStatus.Imported || i.Status == (int)ImportItemStatus.AdmitTemporarily).ToList();

            // remove n' logs danh sach cac xe da nhap
            int index = 0;

            while (index < notArrivedVehicles.Count)
            {
                if (items.Where(v => v.EngineNumber == notArrivedVehicles[index].EngineNumber).Count() > 0)
                {
                    notArrivedVehicles.RemoveAt(index);
                }
                else
                {
                    VDMS.I.Entity.ShippingDetail item = failedItems.SingleOrDefault(i => i.OrderNumber == notArrivedVehicles[index].EngineNumber);
                    if (item != null)
                    {
                        notArrivedVehicles[index].Exception = item.Exception;
                    }
                    index++;
                }
            }

            // get and store imported date of imported vehicles for later use
            importedDate = ItemHepler.GetImportedDate(items);

            //ObjectDataSource1.SelectParameters.Remove(ObjectDataSource1.SelectParameters["shipID"]);
            //ObjectDataSource1.SelectParameters.Add("shipID", TypeCode.Int64, SH.Id.ToString());
            //GridView1.DataSourceID = "ObjectDataSource1";

            // xe da nhap - tam nhap
            GridView1.DataSource = items;
            GridView1.DataBind();
            if (items.Count > 0)
            {
                phImported.Visible = true;
                ddlwarehouselist.Visible = false;
            }
            else
            {
                phImported.Visible = false;
            }

            // xe chua nhap
            GridView3.DataSource = notArrivedVehicles;
            GridView3.DataBind();
            if (notArrivedVehicles.Count > 0)
            {
                ddlwarehouselist.Visible = true;
                phNotImported.Visible = true;
                btnAccept.Enabled = true;
                txtImportDate.Enabled = true;
                imgbCarlendar.Visible = true;
            }
            else
            {
                phNotImported.Visible = false;
                ddlwarehouselist.Visible = false;
            }
            #endregion
            lblAddress.Text = Shipping.GetShippingAddress(shippingNumber, UserHelper.DealerCode);
        }

        // show infos
        hdAddress.Value = SH.Shippingto;
        txtImportDate.Text = CalculateInitImportDate((DateTime)ViewState[VS_ExportDate]).ToShortDateString();//SH.Shippingdate.ToShortDateString();
        lblQuantityImport.Text = (GridView3.Rows.Count + GridView1.Rows.Count).ToString();

    }

    public List<VDMS.I.Entity.ShippingDetail> Select(Int64 shipID)
    {
        using (var db = new VehicleDataContext())
        {
            return db.ShippingDetails.Where(p => p.ShippingId == shipID).ToList();
        }
    }
    protected bool ToBool(int i)
    {
        return i > 0;
    }
    protected void btnAccept_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        if (!Requiredfieldvalidator9.IsValid) return;
        if (!SaveShippingDetail()) AddError(ImportErrorCode.UpdateFailed);
    }

    bool IsAllOrderConfirmed = true;
    protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridView gv = (GridView)sender;

            // set Item No
            //Literal lit = (Literal)e.Row.FindControl("litNo");
            //if (lit == null) return;
            int no = gv.PageIndex * gv.PageSize + e.Row.RowIndex + 1;
            //lit.Text = no.ToString();
            e.Row.Cells[0].Text = no.ToString();

            // setup data
            string branch = e.Row.Cells[9].Text;

            string eng = ((Label)e.Row.FindControl("lblEngineNumber")).Text;
            TextBox txt = (TextBox)e.Row.FindControl("txtException");
            TextBox txtItemDate = (TextBox)e.Row.FindControl("txtItemImportDate");
            RadioButtonList rbl = (RadioButtonList)e.Row.FindControl("rblStatus");
            CheckBox chb = (CheckBox)e.Row.FindControl("chbVoucherStatus");
            CheckBox chbConfirmed = (CheckBox)e.Row.FindControl("chbOrderComfirmed");
            ImageButton imgDt = (ImageButton)e.Row.FindControl("imgbItemCarlendar");
            if (UserHelper.BranchCode != branch && !errorCode.Contains(ImportErrorCode.OrdersDoesNotConfirmed))
            {
                txt.Text = UserHelper.BranchCode;
                txt.Attributes["disabled"] = "disabled";
                e.Row.Enabled = false;
            }

            //check trang thai hien thoi cua xe
            using (var dc = new VehicleDataContext())
            {
                if (dc.ItemInstances.FirstOrDefault(i => i.EngineNumber == eng && ItemHepler.GetInstockItemStatus().Contains(i.Status)) != null)
                {
                    txt.Text = "Vehicle already instock!";
                    txt.Attributes["disabled"] = "disabled";
                    e.Row.Enabled = false;
                }
                if (dc.ItemInstances.FirstOrDefault(i => i.EngineNumber == eng && i.Status == (int)ItemStatus.Sold) != null)
                {
                    txt.Text = "Vehicle already sold out!";
                    txt.Attributes["disabled"] = "disabled";
                    e.Row.Enabled = false;
                }
            }

            //if ((txt == null) || (rbl == null) || (chb == null)) return;
            txt.Enabled = ((!chb.Checked) || (rbl.SelectedValue != "1"));
            string js = "Import_StatusChanged(document.getElementById('" + btnAccept.ClientID +
                                          "'),document.getElementById('" + txt.ClientID +
                                          "'),document.getElementById('" + rbl.ClientID +
                                        "_1'),document.getElementById('" + chb.ClientID +
                                          "'),document.getElementById('" + rbl.ClientID +
                                        "_0'),document.getElementById('" + txtItemDate.ClientID +
                                          "'),document.getElementById('" + txtImportDate.ClientID +
                                          "'),document.getElementById('" + imgDt.ClientID + "'))";
            rbl.Attributes.Add("OnClick", js);
            chb.Attributes.Add("OnClick", js);
            txt.Attributes.Add("onkeyup", js);
            txt.Attributes.Add("onblur", js);
            //txtItemDate.Attributes.Add("disabled", "disabled");

            //clientScript += " txtList[" + e.Row.RowIndex + "] = document.getElementById('" + txt.ClientID + "'); \n";
            clientScript += " document.getElementById('" + txt.ClientID + "'), ";
            clientFunc += "function(){" + js + ";}, ";

            //string OrderNumber = (string)((e.Row.DataItem as DataRowView)["OrderNumber"]);
            string OrderNumber = "";
            if (e.Row.DataItem is VDMS.I.Entity.IShippingDetail)
                OrderNumber = (e.Row.DataItem as VDMS.I.Entity.IShippingDetail).TipTopOrderNumber;
            else
                OrderNumber = (string)((e.Row.DataItem as DataRowView)["TipTopOrderNumber"]);

            IsAllOrderConfirmed &= chbConfirmed.Checked;
            // set import date range
            RangeValidator rv = (RangeValidator)e.Row.FindControl("rvItemImportDate");
            if (rv != null)
            {
                if ((ViewState[VS_BaseShipDate] == null) || (DateTime)ViewState[VS_BaseShipDate] > DateTime.Now)
                    Validator.SetDateRange(rv, DateTime.Now, DateTime.Now, true, e.Row.RowIndex + 1);
                else
                    Validator.SetDateRange(rv, (DateTime)ViewState[VS_BaseShipDate], DateTime.Now, true, e.Row.RowIndex + 1);
            }
        }
    }

    public bool IsOrderConfirmed(string OrderNumber)
    {
        IDao<Orderheader, long> dao = DaoFactory.GetDao<Orderheader, long>();
        dao.SetCriteria(new ICriterion[] { Expression.Eq("Ordernumber", OrderNumber), Expression.Eq("Status", (int)VDMS.I.Vehicle.OrderStatus.Confirmed) });
        return dao.GetCount() == 1;
    }

    protected void GridView3_DataBound(object sender, EventArgs e)
    {
        //clientScript += " btnTest = document.getElementById('" + btnTest.ClientID + "'); \n";
        clientScript += "document.getElementById('" + btnTest.ClientID + "') );\n";
        clientFunc += "null );\n";
        if ((onTipTop) && (errorCode.Count == 0)) clientFunc += "CheckStatusList(" + GridView3.Rows.Count + ");\n";
        litTxtList.Text = clientScript + clientFunc
                        + "\n function Import_ExceptionChecker()\n{\n   return Import_ExceptionCheck(txtList," + GridView3.Rows.Count + ",'" + Message.Shipping_EnterAllException + "','" + Question.SaveData + "');\n}\n"
                        + "-->\n</script>";
        //btnAccept.Attributes.Add("Onclick", "return Import_ExceptionCheck(txtList," + GridView3.Rows.Count + ",'" + Message.Shipping_EnterAllException + "','" + Question.SaveData + "'); ");
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Literal lit = (Literal)e.Row.FindControl("litNo");
            GridView gv = ((GridView)sender);
            //if (lit == null) return;
            int no = gv.PageIndex * gv.PageSize + e.Row.RowIndex + 1;
            //lit.Text = no.ToString();
            e.Row.Cells[0].Text = no.ToString();
        }
    }

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            IDao<ShippingHeader, long> dao = DaoFactory.GetDao<ShippingHeader, long>();
            dao.SetCriteria(new ICriterion[] { Expression.Eq("Shippingnumber", e.Row.Cells[0].Text) });
            IList list = dao.GetAll();
            e.Row.Cells[1].Text = ((bool)(list.Count > 0)).ToString();
        }
    }

    protected void litImportedDate_DataBinding(object sender, EventArgs e)
    {
        Literal lit = (Literal)sender;
        lit.Text = importedDate != null && importedDate[lit.Text] != null ? DataFormat.DateString(importedDate[lit.Text].ToString()) : "";
    }



    /// <summary>
    /// Leo - mvbinh: Automatic select item when you load form
    /// </summary>
    protected void Selectddlwarehouselist()
    {
        ddlwarehouselist.DataBind();
        //object o = ddlwarehouselist.DataSource;
        //ddlwarehouselist.SelectedIndex = 2;
        int i = 0;
        foreach (ListItem lt in ddlwarehouselist.Items)
        {
            lt.Selected = false;
            if (hdAddress.Value != null)
            {
                if (lt.Value == hdAddress.Value)
                {
                    lt.Selected = true;
                    break;
                }
            }
            i++;
        }
        ddlwarehouselist.SelectedIndex = i;
    }

    /// <summary>
    /// Update ShippingAddress direct after ddlwarehouselist selectedindex
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlwarehouselist_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateBrandCode(QueryShipNo, ddlwarehouselist.SelectedValue);
        btnTest_Click(null, null);

    }

    #region fix BehaviorID to make Extender run on dataBoundControl like GridView
    protected void mexItemImportedDate_Load(object sender, EventArgs e)
    {
        MaskedEditExtender mask = (MaskedEditExtender)sender;
        mask.BehaviorID = mask.ClientID;
    }
    protected void carxItemImportedDate_Load(object sender, EventArgs e)
    {
        CalendarExtender ce = (CalendarExtender)sender;
        ce.BehaviorID = ce.ClientID;
    }
    #endregion
}
