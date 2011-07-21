using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using VDMS;
using VDMS.Common.Utils;
using VDMS.Common.Web;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.Linq;
using VDMS.II.PartManagement.Order;

public partial class Part_Inventory_Receive : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtFromDate.Text = DateTime.Now.AddDays(-21).ToShortDateString();
            txtToDate.Text = DateTime.Now.ToShortDateString();
            lv.Enabled = false;
        }
        else
            lv.Enabled = true;
    }

    public string EvalRemainStyle(int qQty, int? dQty, string status, DateTime? payDate, DateTime? deliveryDate)
    {
        return EvalRemain(qQty, dQty) > 0 ? (OrderDAO.OrderCanHasRemain(status, deliveryDate, payDate) ? " errorText" : " hlText") : "";
    }
    public int EvalRemain(int qQty, int? dQty)
    {
        return dQty.HasValue ? qQty - dQty.Value : qQty;
    }
    public string EvalComment(long oId, string comment)
    {
        var os = VDMS.II.PartManagement.Order.OrderDAO.FindSubOrders(oId);
        if (os.Count() > 0) return os.ToList()[0].TipTopNumber;
        return string.IsNullOrEmpty(comment) ? "" : string.Format(@"<a href=""javascript:window.alert('{0}')"">Y</a>", comment);
    }

    void BindData(bool bindOnly)
    {
        var db = DCFactory.GetDataContext<PartDataContext>();
        var query = from h in db.OrderHeaders
                    orderby h.OrderDate
                    select new
                    {
                        Key = h.TipTopNumber,
                        h.OrderHeaderId,
                        h.OrderDate,
                        h.QuotationDate,
                        h.PaymentDate,
                        h.ShippingDate,
                        h.DeliveryDate,
                        h.Amount,
                        h.Status,
                        h.DealerCode,
                        h.ToLocation,
                        Items = from d in db.OrderDetails
                                join p in db.Parts on d.PartCode equals p.PartCode
                                where d.OrderHeaderId == h.OrderHeaderId && p.DatabaseCode == UserHelper.DatabaseCode
                                orderby d.LineNumber
                                select new
                                {
                                    ReferenceId = d.OrderHeader.ReferenceId,
                                    DeliveryDate = d.OrderHeader.DeliveryDate,
                                    PaymentDate = d.OrderHeader.PaymentDate,
                                    HStatus = d.OrderHeader.Status,
                                    d.OrderHeaderId,
                                    d.PartCode,
                                    d.OrderQuantity,
                                    d.OriginalQty,
                                    d.QuotationQuantity,
                                    d.DelivaryQuantity,
                                    //d.OOSQuantity , //reserved: Out Of Service
                                    d.UnitPrice,
                                    p.EnglishName,
                                    p.VietnamName,
                                    Amount = d.QuotationQuantity * d.UnitPrice,
                                    Status = d.Status,
                                    ModifyFlag = d.ModifyFlag,
                                    DealerComment = ""//db.ReceiveDetails.SingleOrDefault(rd => rd.PartCode == d.PartCode && rd.OrderHeaderId == d.OrderHeaderId).DealerComment
                                },
                        ReceiveDate = db.ReceiveHeaders.Where(rh => rh.OrderHeaderId == h.OrderHeaderId).Max(p => p.ReceiveDate)
                    };
        var fromDate = UserHelper.ParseDate(txtFromDate.Text);
        var toDate = UserHelper.ParseDate(txtToDate.Text);
        if (fromDate != DateTime.MinValue) query = query.Where(p => p.OrderDate >= fromDate);
        if (toDate != DateTime.MinValue) query = query.Where(p => p.OrderDate <= toDate.AddDays(1));

        if (!string.IsNullOrEmpty(txtOrderNumber.Text)) query = query.Where(p => p.Key.Contains(txtOrderNumber.Text));
        switch (ddlStatus.SelectedIndex)
        {
            case 0:
                query = query.Where(p => p.Status == OrderStatus.OrderClosedAbnormal || p.Status == OrderStatus.OrderClosedNormal || p.Status == OrderStatus.OrderConfirmed);
                break;
            case 1:
                query = query.Where(p => p.Status == OrderStatus.OrderClosedAbnormal || p.Status == OrderStatus.OrderClosedNormal);
                break;
            case 2:
                query = query.Where(p => p.Status == OrderStatus.OrderConfirmed);
                break;
            default:
                break;
        }
        if (UserHelper.IsDealer) query = query.Where(p => p.DealerCode == UserHelper.DealerCode && p.ToLocation == UserHelper.WarehouseId);
        else query = query.Where(p => p.DealerCode == dlDealer.SelectedValue);
        lv.DataSource = query;
        lv.DataBind();

        if (bindOnly) return;
        StringBuilder builder = new StringBuilder();
        query.ToList().ForEach(u => builder.AppendFormat("'{0}',", u.ShippingDate.HasValue && (u.Status == OrderStatus.OrderConfirmed || u.Status == OrderStatus.OrderClosedNormal || u.Status == OrderStatus.OrderClosedAbnormal) ? u.OrderHeaderId : 0));
        if (builder.Length > 0)
            Page.ClientScript.RegisterArrayDeclaration("OrderNo", builder.Remove(builder.Length - 1, 1).ToString());
    }

    protected void cmdQuery_Click(object sender, EventArgs e)
    {
        BindData(false);
    }

    protected string WritePaymentDate(DateTime? QuotationDate, DateTime? PaymentDate)
    {
        if (QuotationDate == null) return "*";
        if (QuotationDate != null && PaymentDate == null)
        {
            if (QuotationDate < DateTime.Now.AddDays(-VDMSSetting.CurrentSetting.PaymentSpan))
            {
                return @"<span style=""color:Red;"">*</span>";
            }
            else return "*";
        }
        return PaymentDate.Value.ToShortDateString();
    }

    protected string WriteDeliveryDate(DateTime? PaymentDate, DateTime? ShippingDate)
    {
        if (PaymentDate == null) return "-";
        if (PaymentDate != null && ShippingDate == null)
        {
            if (PaymentDate < DateTime.Now.AddDays(-VDMSSetting.CurrentSetting.ShippingSpan))
            {
                return @"<span style=""color:Red;"">*</span>";
            }
            else return "*";
        }
        return ShippingDate.Value.ToShortDateString();
    }

    protected string WriteReceiveDate(DateTime? ShippingDate, DateTime? ReceiveDate)
    {
        if (ShippingDate == null) return "-";
        if (ShippingDate != null && (ReceiveDate == null || ReceiveDate == DateTime.MinValue))
        {
            if (ShippingDate < DateTime.Now.AddDays(-UserHelper.Dealer.ReceiveSpan))
            {
                return @"<span style=""color:Red;"">*</span>";
            }
            else return "*";
        }
        return VDMS.Helper.DateTimeHelper.To24h(ReceiveDate);
    }

    protected void cmd2Excel_Click(object sender, EventArgs e)
    {
        BindData(true);
        GridView2Excel.Export(lv, "receive.xls");
    }
}
