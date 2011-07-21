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

public partial class Part_Inventory_InStockEdit : BasePage
{
    int OrderId
    {
        get
        {
            int id = 0;
            int.TryParse(Request.QueryString["id"], out id);
            return id;
        }
    }
    List<string> duplicateParts;
    protected void gv1_DataBinding(object sender, EventArgs e)
    {
        if (Request.QueryString["checkduplicate"] == "true")
            duplicateParts = OrderDAO.GetOrderDuplicatePartCode(OrderId);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PartOrderDAO.LoadFromDB(OrderId);
        if (!IsPostBack)
        {
            OrderInfo1.OrderId = OrderId;
        }
        var db = DCFactory.GetDataContext<PartDataContext>();
        var oh = db.OrderHeaders.Single(p => p.OrderHeaderId == OrderId);
        if (oh.DealerCode != UserHelper.DealerCode)
            Response.Redirect("/Part/Inventory/Receive.aspx");
        if (!DealerHelper.GetQuotationCFStatus(UserHelper.DealerCode) || (oh.Status != OrderStatus.OrderConfirmed || oh.PaymentDate != null || oh.QuotationDate == null || oh.ShippingDate != null || oh.DeliveryDate != null))
        {
            gv1.Enabled = false;
            cmdSave.Enabled = false;
        }
    }
    protected void cmdSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        //if (!CheckPartNo(true)) return;

        var db = DCFactory.GetDataContext<PartDataContext>();
        var oh = db.OrderHeaders.Single(p => p.OrderHeaderId == OrderId);
        if (!(oh.Status != OrderStatus.OrderConfirmed || oh.PaymentDate != null || oh.QuotationDate == null || oh.ShippingDate != null || oh.DeliveryDate != null))
        {



            PartOrderDAO.CleanUp();
            PartOrderDAO.Merge();
            PartOrderDAO.InitPrice();
            PartOrderDAO.GetPrice();
            PartOrderDAO.LoadFromDB(OrderId);
            UpdateOrderData();
            foreach (var item in PartOrderDAO.Parts)
            {
                OrderDAO.UpdateOrder(OrderId, item.Line, item.PartCode, item.Quantity);
            }
            PartOrderDAO.LoadFromDB(oh.OrderHeaderId);
            gv1.DataBind();
            lblSaveOk.Visible = true;
            cmdSave.Enabled = false;
            gv1.Enabled = false;
        }
        else
        {
            lblSaveError.Visible = true;
            cmdSave.Enabled = false;
            gv1.Enabled = false;
        }
    }

    bool CheckPartNo(bool CheckAll)
    {
        var r = true;
        if (!CheckAll)
        {
            foreach (GridViewRow row in gv1.Rows)
            {
                var s = row.Cells[1].Text;
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

    private void UpdateOrderData()
    {
        foreach (GridViewRow row in gv1.Rows)
        {
            string s = row.Cells[1].Text;
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
}
