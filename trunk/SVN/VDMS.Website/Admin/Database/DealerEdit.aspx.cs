using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using VDMS.II.Linq;
using VDMS.Common.Utils;

public partial class Admin_Security_DealerEdit : BasePage
{
    string DealerCode
    {
        get
        {
            return Request.QueryString["code"];
        }
    }

    /// <summary>
    /// For binding warehouse on gridview
    /// </summary>
    public string CurrentDealerCode { get; set; }

    public bool IsActive(object wCode, string type)
    {
        var res = (wCode == null) || (wCode.ToString() != WarehouseStatus.Deleted);
        return res;
    }

    public bool IsNeedAddNew(object wCode, string type)
    {
        GridView gv = (type == WarehouseType.Part) ? gvPBo : gvVBo;
        foreach (GridViewRow row in gv.Rows)
        {
            if (row.Cells[1].Text == (string)wCode) return false;
        }
        return true;
    }

    protected void BindWarehouse(GridView o, GridView n, string dealer, string type)
    {
        // must bind OLD list first
        if (o != null)
        {
            o.DataSource = WarehouseDAO.GetAllWarehouses(dealer, type);
            o.DataBind();
        }
        // then TIPTOP list
        if (n != null)
        {
            n.DataSource = VDMS.Data.TipTop.Dealer.GetListAddress(dealer, type);
            n.DataBind();
        }
    }

    private void BindAllWarehouse(string code)
    {
        BindWarehouse(gvPBo, gvPBn, code, WarehouseType.Part);
        BindWarehouse(gvVBo, gvVBn, code, WarehouseType.Vehicle);
    }

    protected void UpdateOldWarehouse(ref Dealer d, string type, GridView data)
    {
        foreach (GridViewRow row in data.Rows)
        {
            CheckBox chb = (CheckBox)row.FindControl("chbActive");
            if (chb != null)
            {
                Warehouse w = WarehouseDAO.GetWarehouseAll(chb.Attributes["Code"], d.DealerCode, type);
                if (w != null)
                {
                    w.Status = chb.Checked ? WarehouseStatus.Normal : WarehouseStatus.Deleted;
                }
            }
        }
    }
    protected int UpdateWarehouses(ref Dealer d, string type, GridView data, bool newDealer, PartDataContext dc)
    {
        int res = 0;
        foreach (GridViewRow row in data.Rows)
        {
            CheckBox chb = (CheckBox)row.FindControl("chbSelected");
            if ((chb != null) && chb.Checked)
            {
                Warehouse w = WarehouseDAO.GetWarehouseAll(chb.Attributes["Code"], d.DealerCode, type);
                if (w == null)
                {
                    w = new Warehouse
                    {
                        Code = chb.Attributes["Code"],
                        Address = chb.Attributes["Address"],
                        Type = type,
                        Status = WarehouseStatus.Normal,
                    };
                    if (!newDealer)
                    {
                        w.DealerCode = d.DealerCode;
                        dc.Warehouses.InsertOnSubmit(w);
                    }
                    else
                    {
                        w.Dealer = d;
                    }
                }
                w.Address = chb.Attributes["Address"];
                res++;
            }
        }
        return res;
    }

    protected void UpdateDealer()
    {
        var odc = 0;
        for (int i = 0; i < 7; i++)
        {
            if (cblOCD.Items[i].Selected) odc |= (2 << i);
        }

        bool newDealer = false;
        CurrentDealerCode = (!string.IsNullOrEmpty(DealerCode)) ? DealerCode : tb1.Text.Trim().ToUpper();
        var db = DCFactory.GetDataContext<PartDataContext>();
        var d = db.Dealers.SingleOrDefault(p => p.DealerCode == CurrentDealerCode);

        if (d == null)
        {
            d = new Dealer
             {
                 Contact = new Contact(),
                 DealerName = tb2.Text,
                 DealerCode = CurrentDealerCode,
                 DatabaseCode = ddlDatabase.SelectedValue,
                 AreaCode = txtAreaCode.Text,
                 DealerType = tbDT.Text == string.Empty ? null : tbDT.Text,
                 Quo_CF_Status = true
             };
            newDealer = true;
        }
        d.Quo_CF_Status = cbQuoCFStatus.Checked;
        d.ParentCode = ddlParent.SelectedValue == string.Empty ? null : ddlParent.SelectedValue;
        //d.ReceiveSpan = int.Parse(tbRS.Text);
        d.AutoInStockPartStatus = cbAIPS.Checked;
        d.AutoInStockVehicleStatus = cbAIVS.Checked;
        d.AutoInStockPartSpan = int.Parse(tbAIPS_d.Text)*24 + int.Parse(tbAIPS_h.Text);
        d.AutoInStockVehicleSpan = int.Parse(tbAIVS_d.Text)*24 + int.Parse(tbAIVS_h.Text);
        d.AutoinstockVehicleStartdate = DataFormat.DateFromString(txtFromDate.Text);
        d.OrderDateControl = odc;
        ci.GetInfo(d.Contact);
        int wCount = UpdateWarehouses(ref d, WarehouseType.Part, gvPBn, newDealer, db);
        wCount += UpdateWarehouses(ref d, WarehouseType.Vehicle, gvVBn, newDealer, db);

        UpdateOldWarehouse(ref d, WarehouseType.Part, gvPBo);
        UpdateOldWarehouse(ref d, WarehouseType.Vehicle, gvVBo);

        // check warehouses count. Must > 0
        if (true)
        {
            if (wCount == 0) wCount = gvPBo.Rows.Count + gvVBo.Rows.Count;
            if (wCount == 0)
            {
                lblWarehouseNotFound.Visible = true;
                return;
            }
        }

        CacheHelper.RemoveAll("Dealer");
        DealerHelper.Unload();

        if (newDealer)
        {
            db.Dealers.InsertOnSubmit(d);
            db.SubmitChanges();
            MembershipHelper.AddMembershipProvider(CurrentDealerCode);
            MembershipHelper.AddRoleProvider(CurrentDealerCode);

            ResetControl(this);
        }
        else
        {
            db.SubmitChanges();
        }

        // default for part
        if (d.DefaultWarehouseId == 0)
        {
            Warehouse wh = d.ActiveWarehouses.Where(w => w.Type == WarehouseType.Part).FirstOrDefault();
            d.DefaultWarehouseId = (wh == null) ? 0 : wh.WarehouseId;
            db.SubmitChanges();
        }

        // default for vehicle
        if (d.DefaultWarehouseId == 0)
        {
            Warehouse wh = d.ActiveWarehouses.Where(w => w.Type == WarehouseType.Vehicle).FirstOrDefault();
            db.SubmitChanges();
            d.DefaultVWarehouseId = (wh == null) ? 0 : wh.WarehouseId;
        }

        DealerHelper.Init();

        lblSaveOk.Visible = true;
    }

    void LoadDealer(string DealerCode)
    {
        var d = DealerDAO.GetDealerByCode(DealerCode);
        if (d != null)
        {
            CurrentDealerCode = d.DealerCode;

            ddlParent.RemoveBranch = DealerCode;
            ddlParent.DataBind();
            ddlParent.SelectedValue = d.ParentCode;

            tb1.Text = d.DealerCode;
            tb2.Text = d.DealerName;
            ddlDatabase.SelectedValue = d.DatabaseCode;
            txtAreaCode.Text = d.AreaCode;
            //stbRS.Text = d.ReceiveSpan.ToString();
            tbDT.Text = d.DealerType;
            tbAIPS_d.Text = (d.AutoInStockPartSpan / 24).ToString();
            tbAIPS_h.Text = (d.AutoInStockPartSpan - d.AutoInStockPartSpan / 24 *24).ToString();
            tbAIVS_d.Text = (d.AutoInStockVehicleSpan / 24).ToString();
            tbAIVS_h.Text = (d.AutoInStockVehicleSpan - d.AutoInStockVehicleSpan / 24 *24).ToString();
            cbAIPS.Checked = d.AutoInStockPartStatus;
            txtFromDate.Text = d.AutoinstockVehicleStartdate.ToShortDateString();
            cbAIVS.Checked = d.AutoInStockVehicleStatus;
            for (int i = 0; i < 7; i++)
            {
                cblOCD.Items[i].Selected = (d.OrderDateControl & (2 << i)) != 0;
            }

            //ci.LoadInfo(d.Contact);
            cbQuoCFStatus.Checked = d.Quo_CF_Status.HasValue ? d.Quo_CF_Status.Value : true;
            btCheck.Enabled = false;
            b1.Enabled = true;
            BindAllWarehouse(CurrentDealerCode);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        InitErrMsgControl(_msg);

        if (!Page.IsPostBack)
        {
            if (!string.IsNullOrEmpty(DealerCode))
            {
                LoadDealer(DealerCode);
            }
        }
    }

    /// <summary>
    /// save data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void b1_Click(object sender, EventArgs e)
    {
        UpdateDealer();
        BindAllWarehouse(CurrentDealerCode);
        //var db = DCFactory.GetDataContext<PartDataContext>();
        //Dealer d;
        //var odc = 0;
        //for (int i = 0; i < 7; i++)
        //{
        //    if (cblOCD.Items[i].Selected) odc |= (2 << i);
        //}
        //if (!string.IsNullOrEmpty(DealerCode))
        //{
        //    d = db.Dealers.SingleOrDefault(p => p.DealerCode == DealerCode);
        //    d.ReceiveSpan = int.Parse(tbRS.Text);
        //    d.OrderDateControl = odc;

        //    var c = db.Contacts.Single(p => p.ContactId == d.ContactId);
        //    ci.GetInfo(c);
        //    db.SubmitChanges();
        //    lblSaveOk.Visible = true;
        //    return;
        //}

        //if (gv1.Rows.Count == 0)
        //{
        //    lblWarehouseNotFound.Visible = true;
        //    return;
        //}

        //ci.GetInfo();
        //d = new Dealer
        //{
        //    DealerName = tb2.Text,
        //    DealerCode = tb1.Text,
        //    DatabaseCode = ddlDatabase.SelectedValue,
        //    AreaCode = txtAreaCode.Text,
        //    ParentCode = ddlParent.SelectedValue == string.Empty ? null : ddlParent.SelectedValue,
        //    DealerType = tbDT.Text == string.Empty ? null : tbDT.Text,
        //    ReceiveSpan = int.Parse(tbRS.Text),
        //    OrderDateControl = odc,
        //    Contact = ci.Contact
        //};

        //Warehouse dw = null;
        //foreach (GridViewRow item in gv1.Rows)
        //{
        //    var w = new Warehouse
        //    {
        //        Code = item.Cells[0].Text,
        //        Address = item.Cells[1].Text,
        //        Dealer = d
        //    };
        //    if (dw == null) dw = w;
        //}

        //db.Dealers.InsertOnSubmit(d);
        //db.SubmitChanges();

        //if (dw != null)
        //{
        //    d.DefaultWarehouseId = (int)dw.WarehouseId;
        //    db.SubmitChanges();
        //}

        //MembershipHelper.AddMembershipProvider(tb1.Text);
        //MembershipHelper.AddRoleProvider(tb1.Text);

        //CacheHelper.RemoveAll("Dealer");
        //DealerHelper.Init();
        //ResetControl(this);
        //lblSaveOk.Visible = true;
    }

    protected void btCheck_Click(object sender, EventArgs e)
    {
        string code = tb1.Text.Trim().ToUpper();

        var d = DealerDAO.GetDealerByCode(code);
        if (d != null)
        {
            AddErrorMsg(string.Format(Resources.Message.ItemAlreadyExist, code));
            return;
        }

        var ds = VDMS.Data.TipTop.Dealer.GetDealer(code);

        ResetControl(inputForm);
        tb1.Text = code;

        if (ds.Tables[0].Rows.Count == 0)
        {
        }
        else
        {
            CurrentDealerCode = code;
            tb2.Text = (string)ds.Tables[0].Rows[0]["BranchName"];
            ddlDatabase.SelectedValue = (string)ds.Tables[0].Rows[0]["DBCode"];
            txtAreaCode.Text = (string)ds.Tables[0].Rows[0]["AreaCode"];
            tbDT.Text = (string)ds.Tables[0].Rows[0]["DealerType"];
            b1.Enabled = true;
        }
        BindAllWarehouse(code);
    }

}
