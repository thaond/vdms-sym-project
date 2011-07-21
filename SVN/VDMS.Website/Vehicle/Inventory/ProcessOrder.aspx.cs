using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Expression;
using VDMS.Common.Web.GridViewHepler;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.DAL2;
using VDMS.Data.IDAL.Interface;
using VDMS.Data.TipTop;
using VDMS.Helper;
using VDMS.I.ObjectDataSource;
using VDMS.I.Vehicle;
using VDMS.II.BonusSystem;

public partial class Vehicle_Inventory_ProcessOrder : BasePage, IOrderRequestPage
{
    const bool OnDevTesting = false;

    // change request (27/Oct/2008) 
    const bool NEED_ITEM_AVAILABLE_ON_CONFIRM = false;
    #region For requery order at Business.aspx

    OrderQueryInformation _OrderQueryInfo = new OrderQueryInformation();
    public OrderQueryInformation OrderQueryInfo
    {
        get
        {
            return _OrderQueryInfo;
        }
        set
        {
            _OrderQueryInfo = value;
        }
    }

    // store query info for requery when back to Business.aspx
    protected void LoadRequeryParams()
    {
        OrderQueryInfo.OQ_DateFrom = Server.UrlDecode(Request.QueryString["oqDF"]);
        OrderQueryInfo.OQ_DateTo = Server.UrlDecode(Request.QueryString["oqDT"]);
        OrderQueryInfo.OQ_Dealer = Server.UrlDecode(Request.QueryString["oqDL"]);
        OrderQueryInfo.OQ_OrderNumber = Server.UrlDecode(Request.QueryString["oqON"]);
        OrderQueryInfo.OQ_Area = Server.UrlDecode(Request.QueryString["oqAI"]);
        OrderQueryInfo.OQ_Status = Server.UrlDecode(Request.QueryString["oqSI"]);
    }

    #endregion

    #region bonus

    private void LoadBonus()
    {
        txtBonusAmount.Text = "";
        chBConfirmed.Visible = false;

        var o = OrderDAO.GetOrder(OrderId);
        if (o != null)
        {
            chBConfirmed.Visible = true;
            txtBonusAmount.Text = o.BonusAmount.ToString();
            chBConfirmed.Checked = o.BonusStatus == VDMS.I.Entity.OrderBonusStatus.Confirmed;
        }
    }

    protected void cvBonus_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;    // no need to confirm bonus first - change request 21/7/2010
        //OrderBonus.IsBonusConfirmed(OrderId);
    }

    #endregion

    private long OrderId
    {
        get
        {
            long id = 0;
            long.TryParse(Request.QueryString["OrderId"], out id);
            return id;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        GridViewHelper helper = new GridViewHelper(this.gvMain);
        helper.RegisterSummary("ORDERQTY", SummaryOperation.Sum);
        helper.RegisterSummary("ITEMTOTALPRICE", SummaryOperation.Sum);
        helper.GeneralSummary += new FooterEvent(helper_GeneralSummary);

        // print order
        btnPrint.OnClientClick = @"window.open('../report/printorder.aspx?oid=" + OrderId + "','printOrder_at_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + "',''); return false;";

        if (!Page.IsPostBack)
        {
            LoadRequeryParams();

            btnPrint.Text = Resources.OrderForm.PrintOrder;

            if (OrderId != 0)
            {
                BindData();
                LoadBonus();

                cmdDoSplit.OnClientClick = CreateConfirmScript(Resources.Question.OrderSplit);
                cmdRefreshShipping.Enabled = (CurrentOrderHeader != null) && (!string.IsNullOrEmpty(CurrentOrderHeader.Ordernumber));
            }
            else Response.Redirect("Business.aspx");
        }
    }

    private void BindData()
    {
        IDao<Orderheader, long> ohDao = DaoFactory.GetDao<Orderheader, long>();
        _currentOrderHeader = ohDao.GetById(OrderId, false); //true -> false

        DataSet ds = OrderDao.GetOrderDetail(OrderId);
        gvMain.DataSource = ds;
        gvMain.DataBind();
        gvSplit.DataSource = ds;
        gvSplit.DataBind();

        int status = int.Parse(ds.Tables[0].Rows[0]["Status"].ToString());
        if (status == (int)OrderStatus.Deleted) Response.Redirect("~/default.aspx");
        if (status != (int)OrderStatus.Confirmed)
        {
            this.cmdEdit.OnClientClick = string.Format("javascript:location.href='EditOrder.aspx?OrderId={0}&Dealer=0'; return false;", OrderId);
            this.cmdSplit.CommandName = "SplitBefore";
        }
        else
        {
            //this.cmdEdit.OnClientClick = string.Format("javascript:alert('{0}'); return false;", Message.OrderForm_DeleteConfirmedOrderError);
            //this.cmdApprove.OnClientClick = string.Format("javascript:alert('{0}'); return false;", Message.OrderForm_ApprovedConfirmedOrderError);
            this.cmdEdit.Enabled = this.cmdApprove.Enabled = false;
            this.cmdSplit.CommandName = "SplitAfter";
        }
        if (!string.IsNullOrEmpty(_currentOrderHeader.Ordernumber)
            && !IsTipTopOrderValid(_currentOrderHeader.Ordernumber, _currentOrderHeader.Shippingto, false)
            ) this.cmdSplit.Enabled = false;
        if (status != (int)OrderStatus.Sent) cmdApprove.Enabled = false;
        lblStatus.Text = VDMS.I.Vehicle.Order.GetOrderStatusString(status);
        lblOrderDate.Text = ((DateTime)ds.Tables[0].Rows[0]["ORDERDATE"]).ToShortDateString();
        lblOrderTimes.Text = ds.Tables[0].Rows[0]["ORDERTIMES"].ToString();
        //lblShipingTo.Text = ds.Tables[0].Rows[0]["SHIPPINGTO"].ToString() + " - " + Dealer.GetAddressByBranchCode(ds.Tables[0].Rows[0]["SHIPPINGTO"].ToString());
        lblDealerComment.Text = ds.Tables[0].Rows[0]["DEALERCOMMENT"].ToString();
        lblSecondaryAddress.Text = ds.Tables[0].Rows[0]["SECONDARYSHIPPINGCODE"].ToString() + " - " + ds.Tables[0].Rows[0]["SECONDARYSHIPPINGTO"].ToString();

        txtComment.Text = ds.Tables[0].Rows[0]["VMEPCOMMENT"] as string;
        txtOrder1Date.Text = ((DateTime)ds.Tables[0].Rows[0]["ORDERDATE"]).ToShortDateString();
        txtOrder2Date.Text = DateTime.Now.ToShortDateString();

        txtTipTopOrderNumber.Text = ds.Tables[0].Rows[0]["ORDERNUMBER"] as string;
        if (!string.IsNullOrEmpty(txtTipTopOrderNumber.Text)) txtTipTopOrderNumber.ReadOnly = true;
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        this.Page.RegisterRequiresControlState(this);
    }

    protected override void LoadControlState(object savedState)
    {
        object[] ctlState = (object[])savedState;
        base.LoadControlState(ctlState[0]);
        this._currentOrderHeader = (Orderheader)ctlState[1];
        this._OrderQueryInfo = (OrderQueryInformation)ctlState[2];
    }

    protected override object SaveControlState()
    {
        object[] ctlState = new object[3];
        ctlState[0] = base.SaveControlState();
        ctlState[1] = this._currentOrderHeader;
        ctlState[2] = this._OrderQueryInfo;
        return ctlState;
    }

    void helper_GeneralSummary(GridViewRow row)
    {
        row.Cells[0].Text = Resources.Message.Total;
        row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
        row.CssClass = "Summary";
    }

    Orderheader _currentOrderHeader = null;
    Orderheader CurrentOrderHeader
    {
        get
        {
            return _currentOrderHeader;
        }
        set
        {
            _currentOrderHeader = value;
        }
    }

    protected void cmdConfirm_Click(object sender, EventArgs e)
    {
        if ((!Page.IsValid)
            && !OnDevTesting
            ) return;
        IDao<Orderheader, long> ohDao = DaoFactory.GetDao<Orderheader, long>();
        IDao<Orderdetail, long> odDao = DaoFactory.GetDao<Orderdetail, long>();

        using (TransactionBlock tran = new TransactionBlock())
        {
            Orderheader oh = CurrentOrderHeader;
            oh.Status = (int)OrderStatus.Confirmed;
            oh.Ordernumber = txtTipTopOrderNumber.Text;
            ////oh.Createddate = 
            //    oh.Orderdate = VDMS.Data.TipTop.Order.GetOrderDate(oh.Ordernumber);
            ohDao.SaveOrUpdate(oh);

            //if (!OnDevTesting)
            {
                // ======> need SynchronizeOrderDetail (du lieu Interface chi co y nghia sau lan confirm dau tien)
                int index = 0;

                // first, delete all in VDMS
                foreach (DataKey key in gvMain.DataKeys)
                {
                    long OrderDetailId = long.Parse(key.Value.ToString());
                    odDao.Delete(OrderDetailId);
                    index++;
                }

                // second, copy from Tip-Top to VDSM
                if (!SynchronizeOrderDetail(txtTipTopOrderNumber.Text, ohDao, odDao, oh))
                {
                    tran.IsValid = false;
                    lblError.Text = (string)GetLocalResourceObject("_ItemDefferent");
                }
                else tran.IsValid = true;

                // update header
                oh.Orderdate = VDMS.Data.TipTop.Order.GetOrderDate(oh.Ordernumber);
            }
            //else 
            tran.IsValid = true;
        }

        IFShipping.RefreshShipping(txtTipTopOrderNumber.Text.ToUpper(), CurrentOrderHeader.Id);
        //RefreshShipping("DOA-550075", CurrentOrderHeader.Id);
        cmdRefreshShipping.Enabled = (CurrentOrderHeader != null) && (!string.IsNullOrEmpty(CurrentOrderHeader.Ordernumber));

        phGoback.Visible = true;
        DisableButton();
    }

    delegate void ModifyOrderHeader(Orderheader oh);
    void ProcessOrderHeader(ModifyOrderHeader func)
    {
        Orderheader oh = CurrentOrderHeader;
        func(oh);
        IDao<Orderheader, long> ohDao = DaoFactory.GetDao<Orderheader, long>();
        ohDao.SaveOrUpdate(oh);
        phGoback.Visible = true;
    }

    protected void cmdCommentAndSave_Click(object sender, EventArgs e)
    {
        ProcessOrderHeader(delegate(Orderheader oh) { oh.Vmepcomment = txtComment.Text; });
        DisableButton();
    }

    protected void cmdApprove_Click(object sender, EventArgs e)
    {
        ProcessOrderHeader(delegate(Orderheader oh) { oh.Status = (int)OrderStatus.Approved; });
        cmdApprove.Enabled = false;
    }

    protected void cmdSplit_Click(object sender, EventArgs e)
    {
        if (CurrentOrderHeader.Status != (int)OrderStatus.Confirmed) mvMain.ActiveViewIndex = 1;
        else
        {
            mvMain.ActiveViewIndex = 2;
            txtOldOrderNumber.Text = CurrentOrderHeader.Ordernumber;
            txtOldOrderNumber.ReadOnly = true;
        }
    }

    // tach truoc khi confirmed
    protected void cmdDoSplit_Click(object sender, EventArgs e)
    {
        int count1 = 0, count2 = 0;
        foreach (GridViewRow row in gvSplit.Rows)
        {
            count1 += int.Parse((row.Cells[2].Controls[1] as TextBox).Text);
            count2 += int.Parse((row.Cells[3].Controls[1] as TextBox).Text);
        }

        if (count1 == count2 || count2 == 0)
        {
            lblResult.Text = (string)GetLocalResourceObject("_EmptyOrderError");
            return;
        }

        IDao<Orderheader, long> ohDao = DaoFactory.GetDao<Orderheader, long>();
        IDao<Orderdetail, long> odDao = DaoFactory.GetDao<Orderdetail, long>();
        using (TransactionBlock tran = new TransactionBlock())
        {
            //get current order
            Orderheader oh = CurrentOrderHeader;

            // and mark current order to deleted
            oh.Status = (int)VDMS.I.Vehicle.OrderStatus.Deleted;
            oh.Lasteditedby = UserHelper.Username;
            oh.Lastediteddate = DateTime.Now;
            ohDao.SaveOrUpdate(oh);

            // get detail of this order
            odDao.SetCriteria(new ICriterion[] { Expression.Eq("Orderheader", oh) });
            List<Orderdetail> listOd = odDao.GetAll();

            // create two new orders
            Orderheader oh1 = new Orderheader(), oh2 = new Orderheader();
            oh1.Createddate = oh2.Createddate = DateTime.Now;
            oh1.Createdby = oh2.Createdby = UserHelper.Username;
            oh1.Lastediteddate = oh2.Lastediteddate = DateTime.Now;
            oh1.Lasteditedby = oh2.Lasteditedby = UserHelper.Username;
            oh1.Ordernumber = oh2.Ordernumber = oh.Ordernumber;

            // set order date of two orders header
            DateTime oh1Date, oh2Date;
            DateTime.TryParse(txtOrder1Date.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out oh1Date);
            DateTime.TryParse(txtOrder2Date.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out oh2Date);
            oh1.Orderdate = oh1Date;
            oh2.Orderdate = oh2Date;

            oh1.Ordertimes = oh2.Ordertimes = 1; // TODO
            oh1.Shippingdate = oh2.Shippingdate = oh.Shippingdate;
            oh1.Shippingto = oh2.Shippingto = oh.Shippingto;
            oh1.Status = oh2.Status = (int)VDMS.I.Vehicle.OrderStatus.Sent;
            oh1.Dealercode = oh2.Dealercode = oh.Dealercode;
            oh1.Subtotal = oh2.Subtotal = 0; // TODO
            oh1.Taxamt = oh2.Taxamt = oh.Taxamt;
            oh1.Freight = oh2.Freight = oh.Freight;
            oh1.Dealercomment = oh2.Dealercomment = oh.Dealercomment;
            oh1.Vmepcomment = oh2.Vmepcomment = oh.Vmepcomment;
            oh1.Referenceorderid = oh2.Referenceorderid = (int)oh.Id;
            oh1.Databasecode = oh2.Databasecode = oh.Databasecode;
            oh1.Areacode = oh2.Areacode = oh.Areacode;
            // new on substore mgr 
            oh1.Secondaryshippingcode = oh2.Secondaryshippingcode = oh.Secondaryshippingcode;
            oh1.Secondaryshippingto = oh2.Secondaryshippingto = oh.Secondaryshippingto;

            // save three orders
            ohDao.SaveOrUpdate(oh1);
            ohDao.SaveOrUpdate(oh2);

            int index = 0;
            foreach (GridViewRow row in gvSplit.Rows)
            {
                int total = int.Parse((row.Cells[2].Controls[1] as TextBox).Text);
                int q2 = int.Parse((row.Cells[3].Controls[1] as TextBox).Text);
                int q1 = total - q2;
                Orderdetail od1 = new Orderdetail(), od2 = new Orderdetail();
                od1.Createddate = od2.Createddate = DateTime.Now;
                od1.Createdby = od2.Createdby = UserHelper.Username;
                od1.Lastediteddate = od2.Lastediteddate = DateTime.Now;
                od1.Lasteditedby = od2.Lasteditedby = UserHelper.Username;
                //od1.Unitprice = od2.Unitprice = listOd[index].Unitprice;
                //od1.Unitpricediscount = od2.Unitpricediscount = listOd[index].Unitpricediscount;
                //od1.Orderpriority = od2.Orderpriority = listOd[index].Orderpriority;
                //Orderdetail od = listOd.Find(delegate(Orderdetail d) { return d.Item.Id == row.Cells[0].Text; });
                Orderdetail od = listOd.Find(d => d.Item.Id == row.Cells[0].Text);
                od1.Unitprice = od2.Unitprice = od.Unitprice;
                od1.Unitpricediscount = od2.Unitpricediscount = od.Unitpricediscount;
                od1.Orderpriority = od2.Orderpriority = od.Orderpriority;
                od1.Item = od2.Item = od.Item;
                od1.Orderheader = oh1;
                od2.Orderheader = oh2;

                if (q1 > 0)
                {
                    od1.Orderqty = q1;
                    odDao.SaveOrUpdate(od1);
                }
                if (q2 > 0)
                {
                    od2.Orderqty = q2;
                    odDao.SaveOrUpdate(od2);
                }
                index++;
            }
            tran.IsValid = true;
        }
        lblResult.Text = (string)GetLocalResourceObject("_ProcessComplete");
        phGoback.Visible = true;
        DisableButton();
    }
    // tach sau khi confirmed
    protected void cmdDoConfirmSplit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        string oldOder = txtOldOrderNumber.Text;
        string[] newOrders = txtNewOrderNumber.Text.Split(';');
        IDao<Orderheader, long> ohDao = DaoFactory.GetDao<Orderheader, long>();
        IDao<Orderdetail, long> odDao = DaoFactory.GetDao<Orderdetail, long>();

        using (TransactionBlock tran = new TransactionBlock())
        {
            //get current order
            Orderheader oh = CurrentOrderHeader;

            if (chkDeleted.Checked) oh.Status = (int)VDMS.I.Vehicle.OrderStatus.Deleted;
            oh.Lasteditedby = UserHelper.Username;
            oh.Lastediteddate = DateTime.Now;

            ohDao.SaveOrUpdate(oh);

            if (!chkDeleted.Checked) // mean sale modify old order
            {
                // first delete the order detail
                odDao.SetCriteria(new ICriterion[] { Expression.Eq("Orderheader", oh) });
                foreach (Orderdetail obj in odDao.GetAll())
                    odDao.Delete(obj);

                // second, add new contain, do in later step
            }

            // create a new list order header
            List<Orderheader> newOh = new List<Orderheader>();
            foreach (var newOrder in newOrders)
            {
                Orderheader noh = new Orderheader()
                {
                    Createddate = DateTime.Now,
                    Createdby = UserHelper.Username,
                    Lastediteddate = DateTime.Now,
                    Lasteditedby = UserHelper.Username,
                    Ordernumber = newOrder,
                    Orderdate = oh.Orderdate,
                    Ordertimes = 1,
                    Shippingdate = oh.Shippingdate,
                    Shippingto = oh.Shippingto,
                    Status = (int)OrderStatus.Confirmed,
                    Dealercode = oh.Dealercode,
                    Subtotal = 0, // TODO
                    Taxamt = oh.Taxamt,
                    Freight = oh.Freight,
                    Dealercomment = oh.Dealercomment,
                    Vmepcomment = oh.Vmepcomment,
                    Referenceorderid = (int)oh.Id,
                    Databasecode = oh.Databasecode,
                    Areacode = oh.Areacode,
                    // new on substore mgr 
                    Secondaryshippingcode = oh.Secondaryshippingcode,
                    Secondaryshippingto = oh.Secondaryshippingto,
                };

                newOh.Add(noh);
                ohDao.SaveOrUpdate(noh);
            }

            // create order detail, include old order if necessary
            int index = 0;
            bool b = true;
            foreach (var newOrder in newOrders)
            {
                if (!SynchronizeOrderDetail(newOrder, ohDao, odDao, newOh[index]))
                {
                    tran.IsValid = false;
                    b = false;
                    lblError.Text = (string)GetLocalResourceObject("_ItemDefferent");
                }
                index++;
            }

            if (!chkDeleted.Checked && b)
            {
                // ======> no need SynchronizeOrderDetail, order on interface auto sync
                //if (!SynchronizeOrderDetail(oh.Ordernumber, odDao, oh))
                //{
                //    tran.IsValid = false;
                //    lblError.Text = (string)GetLocalResourceObject("_ItemDefferent");
                //}
            }

            if (b) tran.IsValid = true;
        }

        // if bonus hasnt confirmed yet, do it now - CR 21/7/2010
        if (!chBConfirmed.Checked)
            OrderDAO.ChangeBonus(CurrentOrderHeader.Id, long.Parse(txtBonusAmount.Text), true, string.Empty);

        phGoback.Visible = true;
        DisableButton();
    }

    bool SynchronizeOrderDetail(string OrderNumber, IDao<Orderheader, long> ohDao, IDao<Orderdetail, long> odDao, Orderheader oh)
    {
        DataSet ds = VDMS.Data.TipTop.Order.GetOrderDetail(OrderNumber);
        long total = 0;
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            if (!ItemDataSource.IsExistItem((string)row["ItemCode"], NEED_ITEM_AVAILABLE_ON_CONFIRM)) return false;
            Orderdetail od = new Orderdetail()
            {
                Createddate = DateTime.Now,
                Createdby = UserHelper.Username,
                Lastediteddate = DateTime.Now,
                Lasteditedby = UserHelper.Username,
                Unitprice = long.Parse(row["Price"].ToString()),
                Unitpricediscount = 0,
                Orderpriority = int.Parse((string)row["Priority"]),
                Orderqty = (int)((decimal)row["Quantity"])
            };
            total += (od.Unitprice * od.Orderqty);

            Item item = new Item() { Id = (string)row["ItemCode"] };
            od.Item = item;
            od.Orderheader = oh;

            odDao.SaveOrUpdate(od);
        }

        oh.Subtotal = total;
        ohDao.SaveOrUpdate(oh);
        return true;
    }

    protected void cmdReturn_Click(object sender, EventArgs e)
    {
        mvMain.ActiveViewIndex = 0;
    }

    protected void cvNewOrderNumber_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        foreach (string s in args.Value.Split(';'))
            if (!IsTipTopOrderValid(s, _currentOrderHeader.Shippingto, true)) // check issue
            {
                args.IsValid = false;
                return;
            }
    }

    protected void cvTipTopOrderNumber_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = IsTipTopOrderValid(args.Value, _currentOrderHeader.Shippingto, false); // not check issue
    }

    bool IsTipTopOrderValid(string OrderNumber, string DealerCode, bool CheckIssue)
    {
        bool result = VDMS.Data.TipTop.Order.IsConfirmedOrderExist(OrderNumber, DealerCode, CheckIssue);

        IDao<Orderheader, long> dao = DaoFactory.GetDao<Orderheader, long>();
        dao.SetCriteria(new ICriterion[] { Expression.Eq("Ordernumber", OrderNumber), Expression.Eq("Status", (int)OrderStatus.Confirmed), Expression.Sql(string.Format("OrderId <> {0}", Request.QueryString["OrderId"])) });
        int count = dao.GetCount();

        result &= count == 0;
        return result
            || OnDevTesting
            ;
    }



    protected void cmdRefreshShipping_Click(object sender, EventArgs e)
    {
        //try
        //{
        IFShipping.RefreshShipping(txtTipTopOrderNumber.Text, CurrentOrderHeader.Id);
        //IFShipping.RefreshShipping("DOA-550075", CurrentOrderHeader.Id);
        phGoback.Visible = true;
        //}
        //catch { }
    }

}
