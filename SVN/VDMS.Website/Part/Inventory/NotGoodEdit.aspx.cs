using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.Linq;
using VDMS.II.PartManagement;
using VDMS.II.BasicData;

public partial class Part_Inventory_NotGoodEdit : BasePage
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

	string NotGoodNumber { get; set; }

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			NotGoodManualDAO.Clear();

			if (OrderId != 0)
			{
				var id = OrderId;
				var oh = db.NGFormHeaders.SingleOrDefault(p => p.NGFormHeaderId == id);
				if (oh == null || oh.Status != NGStatus.Open || oh.NGType != "S") Response.Redirect("Sale.aspx");
				NotGoodManualDAO.LoadFromDB(oh.NGFormHeaderId);
				gv1.DataBind();
			}
		}
	}

	protected void cmdAddRow_Click(object sender, EventArgs e)
	{
		NotGoodManualDAO.Append(int.Parse(ddlRowCount.Text));
		this.gv1.DataBind();
	}

	protected void ddlRows_SelectedIndexChanged(object sender, EventArgs e)
	{
		gv1.PageSize = int.Parse(ddlRows.Text);
		gv1.DataBind();
	}

	protected void Refresh_Click(object sender, EventArgs args)
	{
		//  update the grids contents
		gv1.DataBind();
	}

	void CreateNGForm(string Status)
	{
		if (!Page.IsValid) return;
		UpdateOrderData();
		if (!CheckPartNo(true)) return;
		var db = DCFactory.GetDataContext<PartDataContext>();
		if (OrderId == 0)
		{
			var h = new NGFormHeader
			{
				CreatedDate = DateTime.Now,
				Status = Status,
				NGType = NGType.Special,
				DealerCode = UserHelper.DealerCode,
				ApproveLevel = 0
			};
			foreach (var item in NotGoodManualDAO.Parts)
			{
				if (!string.IsNullOrEmpty(item.PartCode))
				{
					var obj = new NGFormDetail
					{
						PartCode = item.PartCode,
						RequestQuantity = item.Quantity,
						BrokenCode = item.BrokenCode,
						DealerComment = item.Comment,
						NGFormHeader = h
					};
				}
			}
			if (Status == OrderStatus.OrderSent)
			{
				var count = db.NGFormHeaders.Count(q => q.NotGoodNumber.Contains("NG-M-" + DateTime.Now.ToString("yyyyMM")));
				NotGoodNumber = h.NotGoodNumber = "NG-M-" + DateTime.Now.ToString("yyyyMM") + (count + 1).ToString();
			}
			db.NGFormHeaders.InsertOnSubmit(h);
			db.SubmitChanges();
		}
		else
		{
			foreach (var item in NotGoodManualDAO.Parts)
			{
				if (item.NGFormDetailId != 0)
				{
					var od = db.NGFormDetails.SingleOrDefault(p => p.NGFormDetailId == item.NGFormDetailId);
					if (string.IsNullOrEmpty(item.PartCode) || item.Quantity == 0)
						db.NGFormDetails.DeleteOnSubmit(od);
					else
					{
						od.PartCode = item.PartCode;
						od.RequestQuantity = item.Quantity;
						od.BrokenCode = item.BrokenCode;
						od.DealerComment = item.Comment;
					};
				}
				else
				{
					db.NGFormDetails.InsertOnSubmit(new NGFormDetail
					{
						PartCode = item.PartCode,
						RequestQuantity = item.Quantity,
						BrokenCode = item.BrokenCode,
						DealerComment = item.Comment
					});
				}
				db.SubmitChanges();
			}
		}
		NotGoodManualDAO.Clear();
		lblSaveOk1.Visible = true;
		DisableButton();
	}

	protected void cmdSave_Click(object sender, EventArgs e)
	{
		CreateNGForm(NGStatus.Open);
	}

	protected void cmdSaveAndSend_Click(object sender, EventArgs e)
	{
		CreateNGForm(NGStatus.Sent);
		MessageDAO.SendNGAlert(NotGoodNumber, UserHelper.DealerCode);
	}

	bool CheckPartNo(bool CheckAll)
	{
		var r = true;
		if (!CheckAll)
		{
			foreach (GridViewRow row in gv1.Rows)
			{
				var s = ((TextBox)row.Cells[1].Controls[1]).Text;
                if (!string.IsNullOrEmpty(s) && !PartDAO.IsPartCodeValid(s, false))
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
			foreach (var item in NotGoodManualDAO.Parts)
			{
                if (!string.IsNullOrEmpty(item.PartCode) && !PartDAO.IsPartCodeValid(item.PartCode, false))
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
			string s = ((TextBox)row.Cells[0].Controls[1]).Text;
			string q = ((TextBox)row.Cells[3].Controls[1]).Text;
			string bc = ((DropDownList)row.Cells[2].Controls[1]).SelectedValue;
			string c = ((TextBox)row.Cells[4].Controls[1]).Text;
			int i = row.RowIndex + gv1.PageSize * gv1.PageIndex;
			NotGoodManualDAO.Change(i, p =>
			{
				p.PartCode = s;
				p.Quantity = int.Parse(q);
				p.BrokenCode = bc;
				p.Comment = c;
			});
		}
	}

	protected void gv1_PreRender(object sender, EventArgs e)
	{
		CheckPartNo(false);
	}

	//protected void b1_Click(object sender, EventArgs e)
	//{
	//    NotGoodDAO.CreateNGFrom(long.Parse((sender as Button).CommandArgument));
	//    lblSaveOk.Visible = true;
	//    lv.DataBind();
	//}
}
