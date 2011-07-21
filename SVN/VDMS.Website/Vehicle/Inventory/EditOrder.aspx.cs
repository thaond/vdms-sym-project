using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Common.Utils;
using VDMS.Common.Web.Validator;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.IDAL.Interface;
using VDMS.Helper;
using VDMS.I.ObjectDataSource;
using VDMS.I.Vehicle;
using VDMS.II.BonusSystem;
using VDMS.II.Linq;
using VDMS.Bonus.Entity;

public partial class Vehicle_Inventory_EditOrder : BasePage
{
    int _totalItemsCount = 10;

    enum PageModes
    {
        Input,
        PreCalcul
    }
    PageModes PageMode;
    EditOrderActions CurAction = EditOrderActions.Undefined;

    public string GetFullDateString(string date, string split)
    {
        string[] d = date.Split('/');
        for (int i = 0; i < d.Length; i++)
        {
            if (d[i].Length % 2 == 1) d[i] = "0" + d[i];
        }
        return string.Format("{0}/{1}/{2}", d);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        EditOrderActions curAction = EditOrderActions.ViewOnly;
        LoadBonus(ddltoAddress.SelectedValue);
        OrderBonus.CleanBonusData(OrderId);
        //btnPrint.OnClientClick = @"window.open('../report/printorder.aspx?oid=" + OrderId + "','printOrder_at_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + "',''); return false;";
        phInventoryLock.Visible = false;
        phOldItemNotOnSale.Visible = false;

        if (!Page.IsPostBack)
        {
            //EnablePrint(false);
            PageMode = PageModes.Input;
            txtOrderDate.Text = GetFullDateString(DateTime.Now.ToShortDateString(), "/");
            //txtOrderDate.Text = DateTime.Now.ToShortDateString();

            if (OrderId != 0)
            {
                LoadOrderPayments(OrderId);
                //LoadOrderBonus(OrderId);

                IDao<Orderheader, long> dao = DaoFactory.GetDao<Orderheader, long>();
                Value = dao.GetById(OrderId, false);

                if (Value != null)
                {
                    // load order detail
                    LoadOrderItems();
                    ReCheckItems(_items);

                    if (
                        (!IsDealer || (Value.Status == (int)OrderStatus.Draft) || (Value.Status == (int)OrderStatus.Sent))
                        && OrderDAO.CanWriteToInterface(Value.Id)
                        )
                    {
                        curAction = EditOrderActions.EditOldOrder;
                    }
                    else
                    {
                        curAction = EditOrderActions.ViewOnly;
                    }
                }
            }
            else
            {
                //if (InventoryHelper.IsInventoryLock(DateTime.Now, UserHelper.DealerCode)) GoHome();
                curAction = EditOrderActions.EditNewOrder;
            }

            if (!IsDealer)
            {
                mvComment.ActiveViewIndex = 1;
                //ddltoAddress.Enabled = false;
            }

            // check for inventory lock
            Validator.SetDateRange(rvOrderDate, InventoryHelper.GetLockedDate(ddltoAddress.SelectedValue, ddlSecondaryAddress.SelectedValue).AddMonths(1), DateTime.MaxValue, true);
            if (InventoryHelper.IsInventoryLock(DateTime.Now, ddltoAddress.SelectedValue, ddlSecondaryAddress.SelectedValue))
            {
                curAction = EditOrderActions.ViewOnly;
                phInventoryLock.Visible = true;
            }
            else
            {
                phInventoryLock.Visible = false;
            }

            SetAction(curAction);

            // page mode here
            BindData();
        }
    }

    protected void cmdSave_Click(object sender, EventArgs e)
    {
        if (ValidOrder() && SaveOrder(-1)) SetAction(EditOrderActions.ViewWithCommands);
        else SetAction(EditOrderActions.FinishEdit);
    }

    protected void cmdFinish_Click(object sender, EventArgs e)
    {
        if (_items.Count(i => i.Orderqty > 0) <= 0) return;

        if (PageMode == PageModes.Input)
        {
            PageMode = PageModes.PreCalcul;
            SetAction(EditOrderActions.FinishEdit);

            BindData();
        }
    }

    protected void cmdEdit_Click(object sender, EventArgs e)
    {
        if (PageMode == PageModes.PreCalcul)
        {
            PageMode = PageModes.Input;
            if (Value == null) SetAction(EditOrderActions.EditNewOrder);
            else if ((Value != null) && (Value.Status != (int)OrderStatus.Confirmed) && (Value.Status != (int)OrderStatus.Approved)) SetAction(EditOrderActions.EditOldOrder);
            else SetAction(EditOrderActions.ViewOnly);

            BindData();
        }
    }

    protected void cmdClose_Click(object sender, EventArgs e)
    {
        GoBack(true);
    }

    protected void cmdSend_Click(object sender, EventArgs e)
    {
        if (ValidOrder() && SaveOrder((int)OrderStatus.Sent)) SetAction(EditOrderActions.ViewWithCommands);
        else SetAction(EditOrderActions.FinishEdit);
    }

    protected void cmdDelete_Click(object sender, EventArgs e)
    {
        Order.DelOrderHeader(Value);
        GoBack(true);
    }

    protected void ddltoAddress_DataBound(object sender, EventArgs e)
    {
        if (UserHelper.IsDealer && UserHelper.BranchCode != UserHelper.DealerCode)
        {
            //ddltoAddress.SelectedValue = UserHelper.BranchCode;
            ddltoAddress.SelectedValue = UserHelper.DealerCode;
            //ddltoAddress.Enabled = false;
        }
        LoadBonus(ddltoAddress.SelectedValue);
    }

    protected void ddlItem_DataBound(object sender, EventArgs e)
    {
        DropDownList drop = sender as DropDownList;
        foreach (ListItem item in (drop.Items))
        {
            item.Text = string.Format("{1}({0})", item.Text, item.Value);
        }
    }

    protected void cmdComputePrice_Click(object sender, EventArgs e)
    {
        CalculateTotal(gvMain);
    }

    #region Check Methods

    protected bool ValidOrder()
    {
        bool ok = true;

        DateTime orderDate = DataFormat.DateFromString(txtOrderDate.Text);
        if (orderDate == DateTime.MinValue
            || InventoryHelper.IsInventoryLock(orderDate, ddltoAddress.SelectedValue, ddlSecondaryAddress.SelectedValue)
            //|| (Order.FixOrderDate(orderDate) > DateTime.Now)
           )
        {
            phInventoryLock.Visible = true;
            ok = false;
        }
        else
        {
            phInventoryLock.Visible = false;
        }
        if (!IsBonusOK())
        {
            LoadBonus(ddltoAddress.SelectedValue);
            ok = false;
        }
        return ok;
    }

    #endregion

    #region Controls State

    protected void Page_Init(object sender, EventArgs e)
    {
        this.Page.RegisterRequiresControlState(this);
    }
    protected override void LoadControlState(object savedState)
    {
        object[] ctlState = (object[])savedState;
        base.LoadControlState(ctlState[0]);
        this._value = (Orderheader)ctlState[1];
        this._items = (List<ItemOrder>)ctlState[2];
        this.PageMode = (PageModes)ctlState[3];
        this._totalItemsCount = (int)ctlState[4];
    }
    protected override object SaveControlState()
    {
        object[] ctlState = new object[5];
        ctlState[0] = base.SaveControlState();
        ctlState[1] = this._value;
        ctlState[2] = this._items;
        ctlState[3] = this.PageMode;
        ctlState[4] = this._totalItemsCount;
        return ctlState;
    }

    #endregion

    #region Fields

    long _orderId = -1;
    public long OrderId
    {
        get
        {
            if (_orderId < 0)
            {
                long.TryParse(Request.QueryString["OrderId"], out _orderId);
            }
            return _orderId;
        }
        set { _orderId = value; }
    }

    List<ItemOrder> _items = new List<ItemOrder>();
    Orderheader _value = null;
    Orderheader Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;

            if (!IsPostBack || value != null)
            {
                ddltoAddress.RootDealer = (_value != null) ? _value.Dealercode : UserHelper.DealerCode;
                ddltoAddress.DataBind();
                //ddlSecondaryAddress.DataBind();
            }

            RefreshFromLocal();
        }
    }

    public bool IsDealer
    {
        get { return (Request.QueryString["Dealer"] == "1") || UserHelper.IsDealer; }
    }

    #endregion

    #region Evals Method

    public string EvalQuantity(object iValue)
    {       //|| ((int)iValue == 0)
        return ((iValue == null)) ? string.Empty : iValue.ToString();
    }

    public string EvalPriorityName(object iValue)
    {
        switch ((int)iValue)
        {
            case 1:
                return Resources.OrderPriority.Urgent;
            case 2:
                return Resources.OrderPriority.General;
            default:
                return Resources.OrderPriority.Stock;
        }
    }

    protected string EvalItemId(object o)
    {
        string res = string.Empty;
        try
        {
            if (ItemDataSource.IsExistItem(o.ToString()))
                res = o.ToString();
            else
                res = new ItemDataSource().GetOneItem().Id;
        }
        catch { }
        return res;
    }

    protected string GetModifyQuestion(object o)
    {
        if (o == null) return string.Empty;
        if (!ItemDataSource.IsExistItem(o.ToString()))
            return CreateConfirmScript(Resources.Question.NotExistItem);
        return string.Empty;
    }


    #endregion

    #region Operator Method

    public bool SaveOrder(int OrderStatus)
    {
        bool res = true;
        long oldId = OrderId; // for save payment

        var dto = UserHelper.ParseDate(txtOrderDate.Text, true);

        var daooh = DaoFactory.GetDao<Orderheader, long>();
        var daood = DaoFactory.GetDao<Orderdetail, long>();
        Orderheader oh;

        //dto = Order.FixOrderDate(dto);

        if (Value != null)
        {
            oh = daooh.GetById(Value.Id, false);
            if (oh.Status == (int)VDMS.I.Vehicle.OrderStatus.Confirmed) return false;
            if (oh.Status == (int)VDMS.I.Vehicle.OrderStatus.Approved && UserHelper.IsDealer) return false;
        }
        else oh = Order.CreateNewOrderHeaderDefault(dto);

        // Update OrderHeader
        oh.Shippingto = ddltoAddress.SelectedValue;
        //if (ddlSecondaryAddress.SelectedIndex != 0)
        {
            oh.Secondaryshippingcode = ddlSecondaryAddress.SelectedValue;
            oh.Secondaryshippingto = ddlSecondaryAddress.SelectedItem.Text;
        }

        if (IsDealer) oh.Dealercomment = txtDealerComment.Text;
        else oh.Vmepcomment = txtVmepComment.Text;

        if (OrderStatus != -1) oh.Status = OrderStatus;

        // To Database
        using (TransactionBlock tran = new TransactionBlock())
        {
            try
            {
                oh = daooh.SaveOrUpdate(oh);

                // delete all old items
                if (Value != null)
                {
                    Order.DeleteOrderItems(Value.Id, ref daood);
                }

                // add edited items
                foreach (ItemOrder item in _items)
                {
                    item.Orderheader = oh;
                    daood.SaveOrUpdate(item.Base);
                }

                oh.Subtotal = _items.Sum(d => d.Unitprice * d.Orderqty);
                tran.IsValid = true;
                OrderId = oh.Id;
            }
            catch
            {
                tran.IsValid = false;
                res = false;
            }
        }
        this.Value = oh;

        // payment
        if (res) res = SavePayment(oldId, oh.Id);

        VDMS.II.Linq.DCFactory.RemoveDataContext<VDMS.I.Linq.VehicleDataContext>();
        VDMS.II.Linq.DCFactory.RemoveDataContext<VDMS.Bonus.Linq.BonusDataContext>();
        LoadOrderPayments(OrderId);

        return res;
    }

    protected void CalculateTotal(GridView activeGv)
    {
        txtTotalOrderQuantity.Text = "";

        ShowNotOnSaleItem(_items.Where(i => i.NotOnSale == true).ToList());

        int total = _items.Sum(s => s.Orderqty);
        activeGv.FooterRow.Cells[3].Text = total.ToString("N0");
        txtTotalOrderQuantity.Text = total.ToString();

        activeGv.FooterRow.Cells[4].Text = _items.Sum(s => s.Price).ToString("N0");
    }

    private void BindData()
    {
        GridView activeGv = null;
        switch (PageMode)
        {
            case PageModes.Input:
                RepackItemsList();
                activeGv = gvMain;
                break;
            case PageModes.PreCalcul:
                activeGv = gvPreview;
                CalculateItem();
                break;
        }
        if ((activeGv != null) && (_items.Count > 0))
        {
            activeGv.DataSource = _items;
            activeGv.DataBind();
            CalculateTotal(activeGv);
        }
    }

    private void ShowNotOnSaleItem(List<ItemOrder> old)
    {
        if (old.Count > 0)
        {
            phOldItemNotOnSale.Visible = true;
            lblOldItemNotOnSale.Text = "";
            foreach (ItemOrder item in old)
            {
                lblOldItemNotOnSale.Text += item.Item.Id + ",  ";
            }
        }
    }

    private List<ItemOrder> ReCheckItems(List<ItemOrder> _items)
    {
        List<Item> sellList = new ItemDataSource().GetListItem();
        List<ItemOrder> old = new List<ItemOrder>();

        for (int i = 0; i < _items.Count; ) //(ItemOrder item in _items)
        {
            if (!string.IsNullOrEmpty(_items[i].Item.Id) && !ItemInList(_items[i].Item, sellList))
            {
                _items[i].NotOnSale = true;
                old.Add(_items[i]);
                _items.RemoveAt(i);
            }
            else i++;
            //_items[i].NotOnSale = !string.IsNullOrEmpty(_items[i].Item.Id) && !ItemInList(_items[i].Item, sellList);
            //i++;
        }

        _items.InsertRange(0, old);
        return old;
    }

    private bool ItemInList(Item item, List<Item> sellList)
    {
        return sellList.SingleOrDefault(i => i.Id.Equals(item.Id, StringComparison.OrdinalIgnoreCase)) != null;
    }

    private void UpdateItem(GridViewRow gvr)
    {
        if (gvr != null)
        {
            string model = string.Empty;
            TextBox txtOrderQty = gvr.Cells[3].FindControl("txtOrderQty") as TextBox;
            DropDownList dropPriority = gvr.Cells[3].FindControl("ddlOrderPriority") as DropDownList;

            Label lbModel = gvr.Cells[1].FindControl("lbModel") as Label;
            if (lbModel.Visible)    // not on sale model
                model = lbModel.ToolTip;
            else    //if (string.IsNullOrEmpty(model))
            {
                DropDownList ddlItem = gvr.Cells[3].FindControl("ddlItem") as DropDownList;
                model = ddlItem.SelectedValue;
            }

            int index = int.Parse(gvr.Cells[0].Text) - 1;
            VDMS.Core.Domain.Item it = Order.GetItemById(model);

            if (it == null)
            {
                _items[index].Item = new Item() { Id = string.Empty };
                gvr.Cells[2].Text = string.Empty;
                gvr.Cells[4].Text = "0";
                _items[index].Orderqty = 0;
                _items[index].Orderpriority = 2;
                _items[index].Unitprice = 0;
            }
            else
            {
                Literal litPrice = (Literal)gvr.FindControl("litPrice");
                _items[index].Item = it;
                gvr.Cells[2].Text = it.Colorname;
                gvr.Cells[4].Text = it.GetUnitPrice(UserHelper.DatabaseCode).ToString("N0");
                _items[index].Orderqty = int.Parse(txtOrderQty.Text);
                _items[index].Orderpriority = int.Parse(dropPriority.SelectedValue);
                _items[index].Unitprice = it.GetUnitPrice(UserHelper.DatabaseCode);
                litPrice.Text = _items[index].Price.ToString("N0");
            }

            CalculateTotal(gvMain);
            //BindData();
        }
    }

    private void SetAction(EditOrderActions action)
    {
        CurAction = action;
        // default
        bool showPrintBtn = (Value != null) && (Value.Id > 0);
        cmdDelete.Enabled = cmdSave.Enabled = cmdSend.Enabled = false;
        cmdFinish.Visible = cmdEdit.Visible = false;
        gvMain.Visible = gvPreview.Visible = false;
        cmdComputePrice.Enabled = false;
        EnablePaymentEdit(false);

        switch (action)
        {
            case EditOrderActions.FinishEdit:
                cmdEdit.Visible = true;
                cmdSave.Enabled = true;
                cmdSend.Enabled = (Value == null) || (Value.Status == (int)OrderStatus.Draft);
                gvPreview.Visible = true;
                break;
            case EditOrderActions.ViewWithCommands:
                cmdEdit.Visible = true;
                cmdSend.Enabled = (Value == null) || (Value.Status == (int)OrderStatus.Draft);
                cmdDelete.Enabled = (Value != null) && (Value.Status != (int)OrderStatus.Confirmed);
                gvPreview.Visible = true;
                break;
            case EditOrderActions.ViewOnly:
                gvPreview.Visible = true;
                PageMode = PageModes.PreCalcul;
                break;
            case EditOrderActions.EditOldOrder:
                cmdDelete.Enabled = (Value != null) && (Value.Status != (int)OrderStatus.Confirmed);
                cmdFinish.Visible = true;
                cmdEdit.Visible = false;
                gvMain.Visible = true;
                cmdComputePrice.Enabled = true;
                EnablePaymentEdit(true);
                break;
            case EditOrderActions.EditNewOrder:
                Value = null;   // force bind dealer info
                showPrintBtn = false;
                cmdFinish.Visible = true;
                gvMain.Visible = true;
                cmdComputePrice.Enabled = true;
                EnablePaymentEdit(true);
                break;
        }

        EnablePrint(showPrintBtn);
        gvPayment.DataBind();
        gvBonus.DataBind();
    }

    private void EnablePrint(bool enable)
    {
        btnPrint.Enabled = enable;
        if (enable)
        {
            btnPrint.OnClientClick = @"window.open('../report/printorder.aspx?oid=" + OrderId + "','printOrder_at_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + "',''); return false;";
        }
    }

    private void GoBack(bool IsGoBack)
    {
        if (IsDealer)
        {
            if (IsGoBack) Response.Redirect("Orderform.aspx");
            else
            {
                //this.LockEditMode();
                //DisableButton();
                //EnablePrint(true);
            }
        }
        else Response.Redirect("Business.aspx");
    }

    private void LoadOrderItems()
    {
        _items = Order.GetOrderItemsByOrderID(OrderId);
    }

    private void RepackItemsList()
    {
        for (int i = _items.Count; i < _totalItemsCount; i++)
        {
            _items.Add(new ItemOrder() { Index = i + 1, Item = new Item() { Id = string.Empty }, Orderpriority = 2 });
        }
        int index = 1;
        foreach (var u in _items) u.Index = index++;
    }

    private void CalculateItem()
    {
        var r = from m in _items
                where m.Orderqty > 0
                group m by m.ItemCode into g
                select new ItemOrder
                {
                    Orderqty = g.Sum(m => m.Orderqty),
                    Item = g.ElementAt(0).Item,
                    Orderpriority = g.ElementAt(0).Orderpriority,
                    Unitprice = g.ElementAt(0).Item.GetUnitPrice(UserHelper.DatabaseCode),
                    NotOnSale = g.ElementAt(0).NotOnSale,
                    Lastediteddate = DateTime.Now,
                    Createddate = DateTime.Now,
                    Createdby = UserHelper.Username,
                    Lasteditedby = UserHelper.Username
                };

        var t = new List<ItemOrder>();
        t.AddRange(r);
        _items = t;

        int index = 1;
        foreach (var u in t) u.Index = index++;
    }

    public void RefreshFromLocal()
    {
        // Edit Order
        if (Value != null)
        {
            txtOrderDate.Text = Value.Orderdate.ToShortDateString();
            txtOrderDate.ReadOnly = true;
            txtOrderDate.CssClass = "readOnlyInputField";
            ibOrderDate.Enabled = false;

            lblOrderTimes.Text = Value.Ordertimes.ToString();
            try
            {
                ddltoAddress.SelectedValue = Value.Shippingto;
                ddltoAddress_SelectedIndexChanged(null, null);
                ddlSecondaryAddress.SelectedValue = Value.Secondaryshippingcode;
            }
            catch { }
            txtOrderStaus.Text = Order.GetOrderStatusString(Value.Status);
            txtDealerComment.Text = Value.Dealercomment;
            txtVmepComment.Text = Value.Vmepcomment;
        }
        //Add Order
        else
        {
            DateTime dto = DataFormat.DateFromString(txtOrderDate.Text);
            lblOrderTimes.Text = Order.GetOrderNumberByDate(dto).ToString();
            txtDealerComment.Text = string.Empty;
            txtVmepComment.Text = string.Empty;
        }

        // Bind to Gridview grdOrder

        //Hien Gridview Caption
        //if (IsDataChange)
        //{
        //    gvMain.Caption = string.Format(Resources.OrderForm.CaptionOrderDetail, "(*)");
        //}
        //else
        //{
        //    gvMain.Caption = string.Format(Resources.OrderForm.CaptionOrderDetail, string.Empty);
        //}
    }

    #endregion

    // for page mode

    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((Control)sender).NamingContainer;
        UpdateItem(gvr);
        //if (gvr != null)
        //{
        //    VDMS.Core.Domain.Item it = Order.GetItemById(ddlItem.SelectedValue);
        //    gvr.Cells[2].Text = it.Colorname;
        //    gvr.Cells[4].Text = it.GetUnitPrice(UserHelper.DatabaseCode).ToString("N0");
        //}
    }

    protected void txtOrderQty_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((Control)sender).NamingContainer;
        UpdateItem(gvr);
    }

    protected void gvMain_DataBound(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        if (gv.FooterRow != null)
        {
            gv.FooterRow.Cells[1].ColumnSpan = 2;
            //gv.FooterRow.Cells[2].Text = "Total:";
            gv.FooterRow.Cells[0].Visible = false;
            gv.FooterRow.Cells[4].ColumnSpan = 2;
            gv.FooterRow.Cells[5].Visible = false;
        }
    }

    protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvMain.PageIndex = e.NewPageIndex;
        BindData();
    }
    protected void btnAddRows_Click(object sender, EventArgs e)
    {
        TextBox tb = gvMain.FooterRow.FindControl("txtAddedRows") as TextBox;
        if (tb != null)
        {
            _totalItemsCount += int.Parse(tb.Text);
            BindData();
        }
    }
    protected void txtOrderDate_TextChanged(object sender, EventArgs e)
    {
        DateTime dto = DataFormat.DateFromString(txtOrderDate.Text);
        //dto = Order.FixOrderDate(dto);
        lblOrderTimes.Text = Order.GetOrderNumberByDate(dto).ToString();
    }
    protected void ddlItem_DataBinding(object sender, EventArgs e)
    {
        DropDownList listModel = sender as DropDownList;
        GridViewRow row = listModel.NamingContainer as GridViewRow;
        if (listModel.Visible) listModel.SelectedValue = listModel.ToolTip;

        row.CssClass = (listModel.Visible) ? "" : "readOnlyRow";
    }
    protected void litModel_DataBinding(object sender, EventArgs e)
    {

    }
    protected void gvPreview_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.CssClass = ((e.Row.DataItem as ItemOrder).NotOnSale) ? "readOnlyRow" : "";
        }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.CssClass = ((e.Row.DataItem as ItemOrder).NotOnSale) ? "readOnlyRow" : "";
        }
    }
    protected void imbDelete_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton imb = (ImageButton)sender;
        _items.RemoveAt(int.Parse(imb.CommandArgument) - 1);
        BindData();
    }

    protected void ddltoAddress_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtOrderDate_TextChanged(null, null);
        // reload warehouse
        ddlSecondaryAddress.DealerCode = ddltoAddress.SelectedValue;
        ddlSecondaryAddress.DataBind();
        LoadBonus(ddltoAddress.SelectedValue);
    }
    protected void ddlSecondaryAddress_DataBound(object sender, EventArgs e)
    {
        ddlSecondaryAddress.SelectedValue = UserHelper.BranchCode;
        ddlSecondaryAddress.Enabled = UserHelper.IsAdmin;
        // khong cho phep dat hang sang kho khac
        cmdFinish.Enabled = UserHelper.IsAdmin || (ddlSecondaryAddress.Items.FindByValue(UserHelper.BranchCode) != null);
    }
    protected void lnkUpdateOrderTime_Click(object sender, EventArgs e)
    {
        txtOrderDate_TextChanged(sender, e);
    }

    #region Payment

    bool IsBonusOK()
    {
        //long bonus = string.IsNullOrEmpty(txtBonus.Text) ? 0 : long.Parse(txtBonus.Text);
        //return BonusPlans.IsOrderBonusOK(ddltoAddress.SelectedValue, bonus);
        return true;
    }

    protected void btnLoadPayment_Click(object sender, EventArgs e)
    {
        gvPayment.DataBind();
        gvBonus.DataBind();
    }

    void LoadBonus(string dCode)
    {
        long curBonus = BonusPlans.GetDealerBonus(dCode);
        txtCurBonus.Text = curBonus.ToString();
        litCurBonus.Text = curBonus.ToString("N0");
    }

    private void LoadOrderBonus(long OrderId)
    {
        var o = OrderBonus.LoadSaleOrderPayments(OrderId);
        gvBonus.DataBind();
        //if (o != null)
        //    txtBonus.Text = o.BonusAmount.ToString();
        //else
        //    txtBonus.Text = "";
    }
    private void LoadOrderPayments(long OrderId)
    {
        var o = OrderBonus.LoadSaleOrderPayments(OrderId);
        odsPays.SelectParameters["oid"].DefaultValue = OrderId.ToString();
        gvPayment.DataBind();
        gvBonus.DataBind();
    }

    private bool SavePayment(long oldId, long OrderId)
    {
        //try
        {
            long bonus = OrderBonus.GetBonusAmount();
            //string.IsNullOrEmpty(txtBonus.Text) ? 0 : long.Parse(txtBonus.Text);
            OrderBonus.SaveSaleOrderPayments(oldId, OrderId, bonus);
            //OrderBonus.SaveBonusTransaction(OrderId);
            OrderBonus.CleanSessionOrderPayments(System.Web.HttpContext.Current.Session.SessionID);
            return true;
        }
        //catch { return false; }
    }

    //private void SaveBonus(

    protected void HyperLink1_DataBinding(object sender, EventArgs e)
    {
        HyperLink lnk = (HyperLink)sender;
        if (CurAction == EditOrderActions.EditNewOrder || CurAction == EditOrderActions.EditOldOrder)
            lnk.Attributes["onclick"] = string.Format("javascript:showBox('{0}'); return false;", lnk.ToolTip);
        else
            lnk.Attributes["onclick"] = "return false;";
    }

    protected void gvPayment_DataBound(object sender, EventArgs e)
    {
        if (gvPayment.FooterRow != null)
        {
            gvPayment.FooterRow.Cells[6].Text = OrderBonus.GetEditingPayments(OrderId).Sum(p => p.Amount).ToString("C0");
        }
    }

    protected void gvPayment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }

    protected void gvPayment_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        gvPayment.DataBind();
    }

    protected void gvBonus_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        gvBonus.DataBind();
    }

    void EnablePaymentEdit(bool enabled)
    {
        gvPayment.Columns[gvPayment.Columns.Count - 1].Visible = enabled;
        gvBonus.Columns[gvBonus.Columns.Count - 1].Visible = enabled;
        lnkAddPayment.Visible = enabled;
        HyperLink2.Visible = enabled;
        HyperLink3.Visible = enabled;
    }
    protected void gvBonus_DataBound(object sender, EventArgs e)
    {
        if (gvBonus.Rows.Count > 0)
        {
            gvBonus.FooterRow.Cells[3].Text = OrderBonus.GetEditingBonuses(OrderId).Sum(b => b.Amount).ToString("C0");
        }
    }
    #endregion

}
