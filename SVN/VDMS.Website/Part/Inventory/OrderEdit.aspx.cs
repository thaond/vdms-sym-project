using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using VDMS;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.Linq;
using VDMS.II.PartManagement;
using VDMS.II.PartManagement.Order;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class Part_Inventory_OrderEdit : BasePage
{
    enum CommandName
    {
        Select,
        Create,
        Edit
    }
    CommandName _commandName = CommandName.Select;

    int OrderId
    {
        get
        {
            int id = 0;
            int.TryParse(Request.QueryString["id"], out id);
            return id;
        }
    }

    protected void BindWarehouse()
    {
        ddlWH.DealerCode = ddlDealer.SelectedValue;
        ddlWH.DataBind();
    }

    protected void ddlDealer_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindWarehouse();
    }

    protected void ddlDealer_DataBound(object sender, EventArgs e)
    {
        BindWarehouse();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            _commandName = (CommandName)Enum.Parse(typeof(CommandName), Request.QueryString["action"], true);
        }
        catch
        {
            Response.Redirect("order.aspx");
        }
        if (!Page.IsPostBack)
        {
            PartOrderDAO.Clear();
            txtOrderDate.Text = DateTime.Now.ToShortDateString();

            if (OrderId != 0)
            {
                var id = OrderId;
                if (id == 0) Response.Redirect("Order.aspx");
                var db = DCFactory.GetDataContext<PartDataContext>();
                var oh = db.OrderHeaders.SingleOrDefault(p => p.OrderHeaderId == id);
                if (oh == null) Response.Redirect("Order.aspx");
                ddlDealer.SelectedValue = oh.Dealer.DealerName;
                ddlWH.SelectedValue = oh.ToLocation.ToString();
                txtOrderDate.Text = oh.OrderDate.ToShortDateString();
                PartOrderDAO.LoadFromDB(oh.OrderHeaderId);
                gv1.DataBind();
                if (oh.TipTopProcessed == "Y")
                {
                    gv1.Enabled = false;
                    DisableButton();
                }
                b1.Enabled = false;
            }

            if (!VDMSSetting.CurrentSetting.AllowChangeOrderDate)
            {
                txtOrderDate.ReadOnly = true;
                phEditOrderDate.Visible = false;
            }
        }
    }

    protected void b1_Click(object sender, EventArgs e)
    {
        if (fu.PostedFile == null) return;

        bool r = PartOrderDAO.LoadExcelData(fu.PostedFile.InputStream, VDMSSetting.CurrentSetting.OrderExcelUploadSetting);
        if (r)
        {
            gv1.DataBind();
            t.ActiveTabIndex = 0;
        }
        else lblExcelError.Visible = true;

    }

    protected void Refresh_Click(object sender, EventArgs args)
    {
        //  update the grids contents
        UpdateOrderData();
        gv1.DataBind();
    }

    protected void cmdSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        UpdateOrderData();
        if (!CheckPartNo(true)) return;

        var db = DCFactory.GetDataContext<PartDataContext>();
        db.SubmitChanges();
        if (_commandName == CommandName.Create)
        {
            if (PartOrderDAO.Parts.Count(p => p.Quantity > 0) == 0) return;
            PartOrderDAO.CleanUp();
            PartOrderDAO.Merge();
            PartOrderDAO.InitPrice();
            PartOrderDAO.GetPrice();
            CheckQuantityForPacking();

            DateTime d = UserHelper.ParseDate(txtOrderDate.Text, true);
            var h = new OrderHeader
            {
                CreatedDate = DateTime.Now,
                CreatedBy = UserHelper.Username,
                Status = OrderStatus.OrderOpen,
                OrderType = "N",
                OrderSource = "V",
                ToDealer = ddlDealer.SelectedValue,
                DealerCode = UserHelper.DealerCode,
                ToLocation = int.Parse(ddlWH.SelectedValue),
                OrderDate = d,
                CanUndoAutoReceive = true,
                ChangeRemark = "N",
                TipTopProcessed = "N",
                Amount = 0,
                SentWarningOverQuotation = false,
                SentWarningOverShipping = false,
                AlreadyInStock = false
            };
            int line = 1;
            foreach (var item in PartOrderDAO.Parts)
            {
                var obj = new OrderDetail
                {
                    LineNumber = line++,
                    PartCode = item.PartCode,
                    OrderQuantity = item.Quantity,
                    QuotationQuantity = 0,
                    UnitPrice = item.UnitPrice,
                    ModifyFlag = "N",
                    Note = DateTime.Now.ToString(),
                    OrderHeader = h,
                    OriginalQty = item.Quantity,
                    Quo_Status = PartOrderQuoStatus.OrderNew
                };
            }
            db.OrderHeaders.InsertOnSubmit(h);
            db.SubmitChanges();
            PartOrderDAO.LoadFromDB(h.OrderHeaderId);
        }
        else
        {
            // check if tip-top has process
            var oh = db.OrderHeaders.Single(p => p.OrderHeaderId == OrderId);
            if (oh.TipTopProcessed == "Y")
            {
                lblTipTopProcessed.Visible = true;
                PartOrderDAO.LoadFromDB(OrderId);
                gv1.DataBind();
                DisableButton();
                return;
            }

            oh.OrderDate = UserHelper.ParseDate(txtOrderDate.Text, true);
            oh.ToDealer = ddlDealer.SelectedValue;
            oh.ToLocation = int.Parse(ddlWH.SelectedValue);
            db.SubmitChanges();

            PartOrderDAO.CleanUp();
            PartOrderDAO.Merge();
            PartOrderDAO.InitPrice();
            PartOrderDAO.GetPrice();

            int line = 1;
            var od = db.OrderDetails.Where(d => d.OrderHeaderId == OrderId).ToList();
            db.OrderDetails.DeleteAllOnSubmit(db.OrderDetails.Where(d => d.OrderHeaderId == OrderId));
            foreach (var item in PartOrderDAO.Parts)
            {
                //if (item.OrderDetailId != 0)
                //{
                //    var od = db.OrderDetails.SingleOrDefault(p => p.OrderDetailId == item.OrderDetailId);
                //    if (string.IsNullOrEmpty(item.PartCode) || item.Quantity == 0)
                //        db.OrderDetails.DeleteOnSubmit(od);
                //    else
                //    {
                //        od.LineNumber = line++;
                //        od.PartCode = item.PartCode;
                //        od.OrderQuantity = item.Quantity;
                //        od.Note = od.Note + ";" + DateTime.Now.ToString();
                //    };
                //}
                //else
                {

                    OrderDetail tod = new OrderDetail
                    {
                        LineNumber = line++,
                        PartCode = item.PartCode,
                        OrderQuantity = item.Quantity,
                        QuotationQuantity = 0,
                        UnitPrice = item.UnitPrice,
                        ModifyFlag = "N",
                        Note = DateTime.Now.ToString(),
                        OrderHeaderId = OrderId
                    };
                    if (oh.Status == OrderStatus.OrderOpen)
                    {
                        tod.OriginalQty = item.Quantity;
                        tod.Quo_Status = PartOrderQuoStatus.OrderNew;
                    }
                    else
                    {
                        OrderDetail odd = od.SingleOrDefault(d => d.LineNumber == item.Line && d.PartCode == item.PartCode);
                        if (odd != null)
                        {
                            if (odd.OrderQuantity == item.Quantity)
                            {
                                tod.OriginalQty = odd.OriginalQty;
                                tod.Quo_Status = odd.Quo_Status;
                            }
                            else
                            {
                                tod.OriginalQty = odd.OriginalQty;
                                tod.Quo_Status = PartOrderQuoStatus.OrderEditByDealer;
                            }
                        }
                        else
                        {
                            tod.OriginalQty = item.Quantity;
                            tod.Quo_Status = PartOrderQuoStatus.OrderNew;
                        }
                    }
                    db.OrderDetails.InsertOnSubmit(tod);
                }
                db.SubmitChanges();
            }

            if (oh.Status == OrderStatus.OrderSent)
                OrderDAO.SendOrder(OrderId, true);
            PartOrderDAO.LoadFromDB(oh.OrderHeaderId);
        }
        gv1.DataBind();
        PartOrderDAO.Clear();
        lblSaveOk.Visible = true;
        DisableButton();
    }

    private void CheckQuantityForPacking()
    {
        foreach (var p in PartOrderDAO.Parts)
        {
            var ps = PartSpecDAO.GetPartSpec(p.PartCode);
            if (ps != null && ps.PackQuantity.HasValue)
            {
                var x = (p.Quantity % ps.PackQuantity.Value);
                if (x > 0) p.Quantity += ps.PackQuantity.Value - x;
                p.ChangedForPacking = x > 0;
            }
        }
    }

    protected void cmdAddRow_Click(object sender, EventArgs e)
    {
        PartOrderDAO.Append(int.Parse(ddlRowCount.Text));
        this.gv1.DataBind();
    }

    protected void ddlRows_SelectedIndexChanged(object sender, EventArgs e)
    {
        gv1.PageSize = int.Parse(ddlRows.Text);
        gv1.DataBind();
    }

    bool CheckPartNo(bool CheckAll)
    {
        var r = true;
        if (!CheckAll)
        {
            foreach (GridViewRow row in gv1.Rows)
            {
                var s = ((TextBox)row.Cells[1].Controls[1]).Text;
                if (!string.IsNullOrEmpty(s) && !PartDAO.IsPartCodeValid(s, true))
                {
                    row.CssClass = "error";
                    r = false;
                }
                else if (!string.IsNullOrEmpty(s) && duplicateParts != null && duplicateParts.Contains(s))
                {
                    row.CssClass = "duplicate";
                }
                else row.CssClass = row.RowIndex % 2 == 0 ? "even" : "odd";
            }
        }
        else
        {
            int index = 0;
            foreach (var item in PartOrderDAO.Parts)
            {
                if (!string.IsNullOrEmpty(item.PartCode) && !PartDAO.IsPartCodeValid(item.PartCode, true))
                {
                    gv1.PageIndex = index / gv1.PageSize;
                    gv1.DataBind();
                    return false;
                }
                index++;
            }
        }
        return r;
    }

    protected void gv1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        UpdateOrderData();
    }

    private void UpdateOrderData()
    {
        foreach (GridViewRow row in gv1.Rows)
        {
            string s = ((TextBox)row.Cells[1].Controls[1]).Text;
            string q = ((TextBox)row.Cells[3].Controls[1]).Text;
            if (q == string.Empty) q = "0";
            int quantity = 0;
            int.TryParse(q, out quantity);
            int i = row.RowIndex + gv1.PageSize * gv1.PageIndex;
            PartOrderDAO.Change(i, p =>
            {
                p.PartCode = s;
                p.Quantity = quantity;
            });
        }
    }

    protected void gv1_PreRender(object sender, EventArgs e)
    {
        CheckPartNo(false);
    }

    List<string> duplicateParts;
    protected void gv1_DataBinding(object sender, EventArgs e)
    {
        if (Request.QueryString["checkduplicate"] == "true")
            duplicateParts = OrderDAO.GetOrderDuplicatePartCode(OrderId);
    }

    protected void ddlWH_DataBound(object sender, EventArgs e)
    {
        ddlWH.SelectedValue = UserHelper.WarehouseId.ToString();
        cmdSave.Enabled = UserHelper.IsAdmin || (ddlWH.Items.FindByValue(UserHelper.WarehouseId.ToString()) != null);
        ddlWH.Enabled = UserHelper.IsAdmin;
    }

    protected void lnkRep_databinding(object sender, EventArgs e)
    {
        var link = (HtmlInputImage)sender;
        link.Attributes["class"] = "hidden";

        var text = (TextBox)(link.Parent.FindControl("t1"));
        if (text != null)
        {
            ((HtmlInputImage)sender).Attributes["onclick"] = "repPart('" + text.ClientID + "', " + text.ToolTip + "); return false;";
            if (PartReplaceDAO.PartCodeCanReplaced(text.Text)) link.Attributes.Remove("class");
        }
    }
}
