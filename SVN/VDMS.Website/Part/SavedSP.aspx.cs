using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.Linq;

public partial class Part_SavedSP : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtFromDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
			txtToDate.Text = DateTime.Now.AddDays(1).ToShortDateString();
        }
    }

	protected void btnFind_Click(object sender, EventArgs e)
    {
		var db = DCFactory.GetDataContext<PartDataContext>();
		var d1 = UserHelper.ParseDate(txtFromDate.Text);
		var d2 = UserHelper.ParseDate(txtToDate.Text);
		if (d2 == DateTime.MinValue) d2 = DateTime.MaxValue;
		else d2 = d2.AddDays(1);

		var query = db.SalesHeaders.Where(p => p.DealerCode == UserHelper.DealerCode && p.WarehouseId == UserHelper.WarehouseId && p.OrderDate < d2 && p.OrderDate > d1);

		if (!string.IsNullOrEmpty(tbSalesFromDate.Text))
		{
			var d = UserHelper.ParseDate(tbSalesFromDate.Text);
			if (d != DateTime.MinValue) query = query.Where(p => p.SalesDate > d);
		}
		if (!string.IsNullOrEmpty(tbSalesToDate.Text))
		{
			var d = UserHelper.ParseDate(tbSalesToDate.Text);
			if (d != DateTime.MinValue) query = query.Where(p => p.SalesDate < d.AddDays(1));
		}

		if (ddlS.SelectedIndex != 0) query = query.Where(p => p.Status == ddlS.SelectedValue);
		if (!string.IsNullOrEmpty(txtVN.Text)) query = query.Where(p => p.SalesOrderNumber.Contains(txtVN.Text));
		if (!string.IsNullOrEmpty(txtTVN.Text)) query = query.Where(p => p.ManualVoucherNumber.Contains(txtTVN.Text));
		if (!string.IsNullOrEmpty(txtPC.Text))
			query = from p in query
					join d in db.SalesDetails on p.SalesHeaderId equals d.SalesHeaderId
					where d.PartCode.Contains(txtPC.Text)
					select p;
		gv1.DataSource = query.OrderByDescending(p => p.SalesHeaderId);
		gv1.DataBind();
    }

    protected void lnkbDel_OnClick(object sender, EventArgs e)
    {
        long id = long.Parse((sender as WebControl).Attributes["shid"]);
        var db = DCFactory.GetDataContext<PartDataContext>();
        SalesHeader sh = db.SalesHeaders.SingleOrDefault(h => h.SalesHeaderId == id);
        if ((sh == null) || (sh.Status != OrderStatus.OrderOpen)) return;

        db.SalesDetails.DeleteAllOnSubmit(sh.SalesDetails);
        db.SalesHeaders.DeleteOnSubmit(sh);
        db.SubmitChanges();
        gv1.DataBind();
    }
}
