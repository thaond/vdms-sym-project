using System;
using System.Linq;
using System.Web.UI.WebControls;
using VDMS.Common.Utils;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using VDMS.II.PartManagement;

public partial class Part_Inventory_StockTransfer : BasePage
{
	int QueryId
	{
		get
		{
			int id = 0;
			int.TryParse(Request.QueryString["id"], out id);
			return id;
		}
	}

	public string PageKey;
	long transferHeaderId = 0;

	public long FromWarehouseId
	{
		get
		{
			long wh;
			long.TryParse(ddlFromWh.SelectedValue, out wh);
			return wh;
		}
	}

	public long ToWarehouseId
	{
		get
		{
			long wh;
			long.TryParse(ddlToWh.SelectedValue, out wh);
			return wh;
		}
	}

	//protected void ChangeFWH(long wid)
	//{
	//    cmdSearch.Attributes["onclick"] = string.Format("javascript:showSearch(this,{0})", wid);
	//}

	protected void DoPrint(long id)
	{
		Response.Redirect(string.Format("PrintTransferForm.aspx?id={0}", id));
	}

	protected void DoTransfer(string status, DateTime tDate)
	{
		try
		{
			if ((FromWarehouseId > 0) && (ToWarehouseId > 0))
			{
				transferHeaderId = PartTransferDAO.TransferParts(this.PageKey, QueryId, FromWarehouseId, ToWarehouseId, tDate, status, txtComment.Text.Trim());
				if (transferHeaderId > 0)
				{
					AddInfoMsg(Resources.Message.ActionSucessful);
				}
				else
				{
					AddErrorMsg(Resources.Message.SomeItemHasError);
				}
			}
		}
		catch (Exception Exception) { AddErrorMsg(Exception.Message); }
		PartTransferDAO.Clear(this.PageKey, ST_ItemState.Moved);
		gv1.DataBind();
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		Dealer d = DealerDAO.GetTopDealer(UserHelper.DealerCode);
		string root = (d != null) ? d.DealerCode : "/";
		ddlToDl.RootDealer = root;
		ddlFromDl.RootDealer = root;
		InitInfoMsgControl(msg);
		InitErrMsgControl(msg);

		if (!IsPostBack)
		{
			PageKey = Guid.NewGuid().ToString();
			SaveState("PageKey", PageKey);

			gv1.PageSize = int.Parse(ddlRows.Text);
			odsSTParts.SelectParameters["key"].DefaultValue = this.PageKey;

			txtTranferDate.Text = DateTime.Now.ToShortDateString();
			try
			{
				ddlFromDl.SelectedValue = UserHelper.DealerCode;
				ddlFromWh.SelectedValue = UserHelper.WarehouseId.ToString();
			}
			catch { }

			if (QueryId > 0)
			{
				LoadData(QueryId);
			}
		}
		else
		{
			PageKey = (string)LoadState("PageKey");
		}

	}

	private void LoadData(int Id)
	{
		TransferHeader th = PartTransferDAO.GetTransferHeader(Id);
		if ((th == null) || (th.Status != ST_Status.New))
		{
			transferHeaderId = 0;
			Response.Redirect("PrintTransferForm.aspx");    //??
		}
		else
		{
            //ddlFromDl.DataBind();
            //ddlToDl.DataBind();
            //ddlFromWh.DataBind();
            //ddlToWh.DataBind();
			ddlFromDl.SelectedValue = th.Warehouse.DealerCode;
			ddlToDl.SelectedValue = th.Warehouse1.DealerCode;
			ddlFromWh.SelectedValue = th.FromWarehouseId.ToString();
			ddlToWh.SelectedValue = th.ToWarehouseId.ToString();
			txtTranferDate.Text = th.TransferDate == null ? "" : ((DateTime)th.TransferDate).ToShortDateString();
			txtComment.Text = th.TransferComment;
			//ChangeFWH(th.FromWarehouseId);

			th.TransferDetails.ToList().ForEach(td =>
				{
					PartSafety ps = PartInfoDAO.GetPartSafety(td.PartInfoId, td.TransferHeader.FromWarehouseId);
					PartTransferDAO.Append(td.PartCode, td.PartInfo.PartType, this.PageKey, ps.CurrentStock, td.PartInfoId, td.Quantity);
				});
			gv1.DataBind();
		}
	}

	protected void UpdatePartInfo(object sender, EventArgs e)
	{
		TextBox tbPartNo = (TextBox)(sender as WebControl).Parent.FindControl("txtPartNo");
		TextBox tbCQty = (TextBox)(sender as WebControl).Parent.FindControl("txtConfirmQuantity");
		TextBox tbTQty = (TextBox)(sender as WebControl).Parent.FindControl("txtTransferQuantity");
		TextBox tbPartRemark = (TextBox)(sender as WebControl).Parent.FindControl("txtRemark");
		DropDownList ddlPartType = (DropDownList)(sender as WebControl).Parent.FindControl("ddlPartType");
		string partKey = tbPartNo.Attributes["PartKey"];

		PartTransferDAO.UpdatePart(FromWarehouseId, this.PageKey, new Guid(partKey), tbPartNo.Text.Trim(), /*ddlPartType.SelectedValue*/ null, (tbTQty.Text.Trim() != "") ? int.Parse(tbTQty.Text) : 0, (tbCQty.Text.Trim() != "") ? int.Parse(tbCQty.Text) : 0, tbPartRemark.Text.Trim());
	}

	protected void cmdAddRow_Click(object sender, EventArgs e)
	{
		PartTransferDAO.Append(int.Parse(ddlRowCount.SelectedValue), this.PageKey);
		gv1.DataBind();
	}

	protected void ddlRows_SelectedIndexChanged(object sender, EventArgs e)
	{
		gv1.PageSize = int.Parse(ddlRows.SelectedValue);
		gv1.DataBind();
	}

	protected void btnPartInserted_Click(object sender, EventArgs e)
	{
		gv1.DataBind();
		//udpParts.Update();
	}

	protected void cmdSave_Click(object sender, EventArgs e)
	{
		DateTime tDate = DataFormat.DateFromString(txtTranferDate.Text);
		if (tDate == DateTime.MinValue) tDate = DateTime.Now;

		DoTransfer(ST_Status.New, tDate);
		if (sender != null) Response.Redirect(string.Format("StockTransfer.aspx?id={0}", transferHeaderId));
	}

	protected void cmdTransfer_Click(object sender, EventArgs e)
	{
		DateTime tDate = DataFormat.DateFromString(txtTranferDate.Text);
		if (tDate == DateTime.MinValue) tDate = DateTime.Now;

		DoTransfer(ST_Status.Transfered, tDate);
		if (transferHeaderId > 0)
		{
			DoPrint(transferHeaderId);
		}
	}

	protected void cmdPrint_Click(object sender, EventArgs e)
	{
		cmdSave_Click(null, null);
		if (transferHeaderId > 0)
		{
			DoPrint(transferHeaderId);
		}
	}

	protected void gv1_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			try
			{
				e.Row.CssClass = ((e.Row.DataItem as PartTransfer).State == ST_ItemState.Wrong) ? "errLine" : "";
			}
			catch { }
		}
	}

	protected void ddlFromDl_SelectedIndexChanged(object sender, EventArgs e)
	{
		ddlFromWh.DealerCode = ddlFromDl.SelectedValue;
		ddlFromWh.DataBind();
	}

	protected void ddlToDl_SelectedIndexChanged(object sender, EventArgs e)
	{
		ddlToWh.DealerCode = ddlToDl.SelectedValue;
		ddlToWh.DataBind();
	}

	protected void ddlFromWh_SelectedIndexChanged(object sender, EventArgs e)
	{
		// ChangeFWH(FromWarehouseId);
	}
}
