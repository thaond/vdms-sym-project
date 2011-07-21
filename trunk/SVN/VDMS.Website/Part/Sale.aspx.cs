using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.Linq;
using VDMS.II.PartManagement;
using VDMS.II.PartManagement.Sales;
using VDMS.II.BasicData;

public partial class Part_Sale : BasePage
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

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			PartSalesDAO.Clear();
			var db = DCFactory.GetDataContext<PartDataContext>();
			ddl1.DataSource = db.Customers.Where(p => p.DealerCode == UserHelper.DealerCode);
			ddl1.DataBind();
			tbOrderDate.Text = DateTime.Now.ToShortDateString();

			if (OrderId != 0)
			{
				var id = OrderId;
				var oh = db.SalesHeaders.SingleOrDefault(p => p.SalesHeaderId == id);
				if (oh == null) Response.Redirect("Sale.aspx");
				if (oh.Status == OrderStatus.OrderClosedNormal)
				{
					DisableButton();
					gv1.Enabled = false;
					ddl1.Enabled = false;
				}

				if (oh.CustomerId != null)
				{
					ddl1.SelectedValue = oh.CustomerId.ToString();
					ddl1_SelectedIndexChanged(sender, e);
				}
				else cn.Text = oh.CustomerName;
				tbOrderDate.Text = oh.OrderDate.ToShortDateString();
				if ((oh.SalesDate != null) && (oh.SalesDate > DateTime.MinValue))
					tbSalesDate.Text = ((DateTime)oh.SalesDate).ToShortDateString();
				tbSVN.Text = oh.SalesOrderNumber;
				tbTSVN.Text = oh.ManualVoucherNumber;
				tbComment.Text = oh.SalesComment;
				//tbDiscount.Text = oh.Discount.ToString();
				//cmdSave.Enabled = false;
				if (oh.SalesDate.HasValue) tbSalesDate.Text = oh.SalesDate.Value.ToShortDateString();

				PartSalesDAO.LoadFromDB(oh.SalesHeaderId);
				gv1.DataBind();

				if (Request.QueryString["save"] == "true")
				{
					lblSaveOk.Visible = true;
					tbSVN.Text = Request.QueryString["svn"];
				}
			}
		}
	}

	protected void b1_Click(object sender, EventArgs e)
	{
		if (fu.PostedFile == null) return;

		bool r = PartSalesDAO.LoadExcelData(fu.PostedFile.InputStream, VDMSSetting.CurrentSetting.SalesExcelUploadSetting);
		PartSalesDAO.GetInfoId();
		PartSalesDAO.GetPrice();
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
		this.gv1.DataBind();
	}

	long SalesOrderId = 0;
	protected void cmdSave_Click(object sender, EventArgs e)
	{
		if (!Page.IsValid) return;
		UpdateOrderData();
		if (!CheckPartNo(true)) return;

		PartSalesDAO.CleanUp();
		var db = DCFactory.GetDataContext<PartDataContext>();
		try
		{
			db.SubmitChanges();
		}
		catch { }

		if (PartSalesDAO.Parts.Count == 0) return;

		string svn = string.Empty;
		if (OrderId == 0)
		{
			var h = new SalesHeader
			{
				DealerCode = UserHelper.DealerCode,
				OrderDate = UserHelper.ParseDate(tbOrderDate.Text, false),
				CustomerName = cn.Text,
				Status = OrderStatus.OrderOpen,
				SalesPerson = UserHelper.Username,
				SubTotal = PartSalesDAO.Parts.Sum(i => i.Amount),
				TaxAmount = 0,
				Discount = PartSalesDAO.Parts.Sum(i => i.DiscountAmount),
				ModifiedDate = DateTime.Now,
				WarehouseId = UserHelper.WarehouseId,
				SalesComment = string.IsNullOrEmpty(tbComment.Text) ? null : tbComment.Text,
				SalesDate = GetSalesDate()
			};
			h.SalesOrderNumber = PartSalesDAO.GenSaleNumber(h);
			tbTSVN.Text = tbTSVN.Text.Trim();
			if (!string.IsNullOrEmpty(tbTSVN.Text)) h.ManualVoucherNumber = tbTSVN.Text;
			if (ddl1.SelectedIndex > 0) h.CustomerId = long.Parse(ddl1.SelectedValue);

			foreach (var item in PartSalesDAO.Parts)
			{
				var obj = new SalesDetail
				{
					PartCode = item.PartCode,
					PartInfoId = item.PartInfoId,
					PartName = item.PartName,
					OrderQuantity = item.Quantity,
					UnitPrice = item.UnitPrice,
					PercentDiscount = item.Discount,
					LineTotal = item.Amount,
					ModifiedDate = DateTime.Now,
					PartType = item.PartType,
					SalesHeader = h
				};
			}
			db.SalesHeaders.InsertOnSubmit(h);
			db.SubmitChanges();
			SalesOrderId = h.SalesHeaderId;
			svn = h.SalesOrderNumber;
		}
		else
		{
			foreach (var item in PartSalesDAO.Parts)
			{
				if (item.SalesDetailId != 0)
				{
					var od = db.SalesDetails.SingleOrDefault(p => p.SalesDetailId == item.SalesDetailId);
					if (string.IsNullOrEmpty(item.PartCode) || item.Quantity == 0)
						db.SalesDetails.DeleteOnSubmit(od);
					else
					{
						od.PartCode = item.PartCode;
						od.PartName = item.PartName;
						od.OrderQuantity = item.Quantity;
						od.PercentDiscount = item.Discount;
						od.ModifiedDate = DateTime.Now;
						od.LineTotal = item.Amount;
					};
				}
				else
				{
					db.SalesDetails.InsertOnSubmit(new SalesDetail
					{
						PartCode = item.PartCode,
						PartName = item.PartName,
						OrderQuantity = item.Quantity,
						UnitPrice = item.UnitPrice,
						SalesHeaderId = OrderId,
						PercentDiscount = item.Discount,
						LineTotal = item.Amount,
						ModifiedDate = DateTime.Now,
						PartInfoId = item.PartInfoId,
						PartType = item.PartType
					});
				}
				db.SubmitChanges();
			}

			// update amount
			var oh = db.SalesHeaders.Single(p => p.SalesHeaderId == OrderId);
			oh.CustomerName = cn.Text;
			if (ddl1.SelectedIndex > 0) oh.CustomerId = long.Parse(ddl1.SelectedValue);

			oh.SubTotal = PartSalesDAO.Parts.Sum(i => i.Amount);
			oh.Discount = PartSalesDAO.Parts.Sum(i => i.DiscountAmount);
			db.SubmitChanges();

			SalesOrderId = OrderId;
			svn = tbSVN.Text;
		}

		lblSaveOk.Visible = true;
		tbSVN.Text = svn;
		if (sender != null)
		{
			PartSalesDAO.Clear();
			Response.Redirect(string.Format("Sale.aspx?id={0}&save=true&svn={1}", SalesOrderId, svn));
		}
		else
		{
			db.Dispose();
			DCFactory.RemoveDataContext<PartDataContext>();
		}
		//DisableButton();
	}

	DateTime GetSalesDate()
	{
		var d = DateTime.Now.Date;
		if (!string.IsNullOrEmpty(tbSalesDate.Text))
		{
			d = UserHelper.ParseDate(tbSalesDate.Text, false);
			if (d > DateTime.Now) d = DateTime.Now.Date;
		}
		return d;
	}

	protected void cmdSale_Click(object sender, EventArgs e)
	{
		// first, click to Save
		cmdSave_Click(null, null);

		// second, sales out
		if (PartSalesDAO.CheckSaleStockQuantity(SalesOrderId, UserHelper.WarehouseId))
		{
			lblOverStock.Visible = true;
			return;
		}

		var d = GetSalesDate();

		// check inventory close
		if (InventoryDAO.IsInventoryLock(UserHelper.WarehouseId, d.Year, d.Month))
		{
			lblInventoryClose.Visible = true;
			return;
		}

		PartSalesDAO.SalesOut(SalesOrderId, d);
		PartSalesDAO.Clear();
		Response.Redirect("Sale.aspx");
	}

	protected void cmdAddRow_Click(object sender, EventArgs e)
	{
		PartSalesDAO.Append(int.Parse(ddlRowCount.Text));
		this.gv1.DataBind();
	}

	bool CheckPartNo(bool CheckAll)
	{
		var r = true;
		if (!CheckAll)
		{
			foreach (GridViewRow row in gv1.Rows)
			{
				var name = row.Cells[2].Text.Trim();
				var s = ((TextBox)row.Cells[0].Controls[1]).Text;
				var t = ((DropDownList)row.Cells[1].Controls[1]).SelectedValue;
                if (!string.IsNullOrEmpty(s) && (!PartDAO.IsPartCodeValid(s, t, false) || string.IsNullOrEmpty(name)))
				{
					row.CssClass = "error";
					r = false;
				}
				else row.CssClass = row.RowIndex % 2 == 0 ? "event" : "old";
			}
		}
		else
		{
			int index = 0;
			foreach (var item in PartSalesDAO.Parts)
			{
                if (!string.IsNullOrEmpty(item.PartCode) && (!PartDAO.IsPartCodeValid(item.PartCode, item.PartType, false) || string.IsNullOrEmpty(item.PartName)))
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

	protected void ddlRows_SelectedIndexChanged(object sender, EventArgs e)
	{
		gv1.PageSize = int.Parse(ddlRows.Text);
		gv1.DataBind();
	}

	protected void ddl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (ddl1.SelectedIndex == 0)
		{
			cn.Text = string.Empty;
			cn.Enabled = true;
			vd.Visible = false;
			lbVAT.Text = string.Empty;
		}
		else
		{
			Customer cus = CustomerDAO.GetCustomer(long.Parse(ddl1.SelectedValue));
			cn.Text = ddl1.SelectedItem.Text;
			if (cus != null) lbVAT.Text = cus.VATCode;
			cn.Enabled = false;
			vd.Visible = true;
		}
		ph.Visible = false;
		if (gv1.Rows.Count > 0) Refresh_Click(sender, e);
	}

	protected void vd_Click(object sender, EventArgs e)
	{
		var db = DCFactory.GetDataContext<PartDataContext>();
		_info1.InViewMode = true;
		_info1.LoadInfo(db.Contacts.SingleOrDefault(p => p.ContactId == db.Customers.Single(c => c.CustomerId == long.Parse(ddl1.SelectedValue)).ContactId));
		ph.Visible = true;
	}

	protected void bAD_Click(object sender, EventArgs e)
	{
		UpdateOrderData();
		PartSalesDAO.Parts.ForEach(p => p.Discount = int.Parse(tbDiscount.Text));
		gv1.DataBind();
	}

	protected void gv1_DataBound(object sender, EventArgs e)
	{
		GridView gv = (GridView)sender;
		if (gv.FooterRow != null)
		{
			gv.FooterRow.Cells[1].Text = "Total";
			gv.FooterRow.CssClass = "sumLine";
			if (PartSalesDAO.Parts != null)
			{
				gv.FooterRow.Cells[5].Text = string.Format("{0}", PartSalesDAO.Parts.Sum(p => p.Quantity));
				gv.FooterRow.Cells[5].CssClass = "number";
				gv.FooterRow.Cells[8].Text = string.Format("{0:C0}", PartSalesDAO.Parts.Sum(p => p.Amount));
				gv.FooterRow.Cells[8].CssClass = "number";
			}
		}
	}

	protected void gv1_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			var ddlT = (DropDownList)e.Row.Cells[1].Controls[1];
			ddlT.SelectedValue = ((PartSales)e.Row.DataItem).PartType;
		}
	}

	protected void gv1_PageIndexChanging(object sender, GridViewPageEventArgs e)
	{
		UpdateOrderData();
	}

	private void UpdateOrderData()
	{
		foreach (GridViewRow row in gv1.Rows)
		{
			string s = ((TextBox)row.Cells[0].Controls[1]).Text;
			string q = ((TextBox)row.Cells[5].Controls[1]).Text;
			string r = ((TextBox)row.Cells[7].Controls[1]).Text;
			string t = ((DropDownList)row.Cells[1].Controls[1]).SelectedValue;
			int i = row.RowIndex + gv1.PageSize * gv1.PageIndex;
			PartSalesDAO.Change(i, p =>
			{
				string temp = p.PartCode;
				p.PartCode = s;
				p.Quantity = int.Parse(q);
				p.Discount = int.Parse(r);
				p.PartType = t;
				if (temp != s) PartSalesDAO.GetInfoId(p);
			});
		}
	}

	protected void gv1_PreRender(object sender, EventArgs e)
	{
		CheckPartNo(false);
	}

    protected void cmdPrint_Click(object sender, EventArgs e)
    {
        // first, click to Save
        cmdSave_Click(null, null);

        if (SalesOrderId > 0)
        {
            // second, print out
            Response.Redirect(string.Format("PrintSaleForm.aspx?id={0}", SalesOrderId));
        }
    }

	protected void cvTSVN_ServerValidate(object source, ServerValidateEventArgs args)
	{
		var db = DCFactory.GetDataContext<PartDataContext>();
		if (OrderId != 0) return;
		args.IsValid = db.SalesHeaders.FirstOrDefault(p => p.DealerCode == UserHelper.DealerCode && p.ManualVoucherNumber == args.Value.Trim()) == null;
	}
}
