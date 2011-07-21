using System;
using System.Linq;
using System.Web.UI.WebControls;
using Resources;
using VDMS;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.PartManagement;

public partial class Admin_Part_CCA : BasePage
{
	public long EditingID
	{
		get
		{
			long id;
			long.TryParse(Request.QueryString["id"], out id);
			return id;
		}
	}

	protected string EvalStockQty(object ps)
	{
		if ((ps != null) && (ps is PartSafety))
		{
			PartSafety p = (PartSafety)ps;
			return p.CurrentStock.ToString();
		}
		return "";
	}

	protected string EvalBeforeStockQty(object th, int CycleQuantity)
	{
		if ((th != null) && (th is TransactionHistory))
		{
			TransactionHistory t = (TransactionHistory)th;
			return (CycleQuantity - t.Quantity).ToString();
		}
		return "";
	}

	protected string EvalDate(object d)
	{
		DateTime dt = (DateTime)d;
		if (dt == DateTime.MinValue) return "";
		return dt.ToShortDateString();
	}

	public void BindData()
	{
		//gv.DataSourceID = odsPartList.ID;
		gv.DataBind();
	}
	public void LoadData(long id)
	{
		var h = CycleCountDAO.GetCycleCountHeader(id);
		if ((h == null) || (h.DealerCode != UserHelper.DealerCode))
		{
			AddErrorMsg(Resources.Message.DataNotFound);
			return;
		}
		ddlWarehouse.SelectedValue = h.WarehouseId.ToString(); ;
		txtSessionComment.Text = h.TransactionComment;
		CycleCountDAO.LoadCycleCountDetails(id);
		t.ActiveTabIndex = 1;
	}

	protected void InitCloseMonth()
	{
		ddlMonth.Items.Clear();
		ddlMonth.Items.Add(DateTime.Now.ToString("MM/yyyy"));
		ddlMonth.Items.Add(DateTime.Now.AddMonths(1).ToString("MM/yyyy"));
	}

	protected bool CheckError(CyclePart p)
	{
		if (string.IsNullOrEmpty(p.PartCode)) return true;
		long wid = long.Parse(ddlWarehouse.SelectedValue);

		var ps = PartInfoDAO.GetPartSafety(p.PartCode, wid);
		if ((ps == null) && !PartDAO.IsPartCodeValidForCC(p.PartCode, PartType.Part))
		{
			p.PartType = p.PartName = "";
			p.Error = Errors.NotExistPartCode; return false;
		}
		if (CycleCountDAO.Parts.Where(i => i.PartCode == p.PartCode).Count() > 1)
		{
			p.Error = Errors.DuplicatedPartCode; return false;
		}
		p.Error = string.Empty;
		return true;
	}

	protected bool CheckError()
	{
		bool ok = true;
		foreach (var p in CycleCountDAO.Parts)
		{
			ok = CheckError(p) && ok;
			if (!string.IsNullOrEmpty(p.Error)) AddErrorMsg(string.Format("{0} {1}: {2}", Constants.Line, p.Line, p.Error));
		}
		return ok;
	}

	protected void UpdateRow(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(ddlWarehouse.SelectedValue))
		{
			GridViewRow row = (GridViewRow)(sender as WebControl).NamingContainer;
			TextBox txtPartCode = (TextBox)row.FindControl("txtPartCode");
			TextBox txtCountQty = (TextBox)row.FindControl("txtCountQty");
			TextBox txtComment = (TextBox)row.FindControl("txtComment");

			CycleCountDAO.UpdateLine(int.Parse(row.Cells[0].Text),
										p => CycleCountDAO.UpdatePart(p,
												txtPartCode.Text.Trim().ToUpper(),
												txtCountQty.Text,
												txtComment.Text.Trim(),
												long.Parse(ddlWarehouse.SelectedValue)));
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		InitErrMsgControl(msg);
		InitInfoMsgControl(msg);
		if (!IsPostBack)
		{
			InitCloseMonth();

			if (EditingID > 0)
			{
				try
				{
					ddlWarehouse.DataBind();
					LoadData(EditingID);
					CheckError();
				}
				catch (Exception ex) { AddErrorMsg(ex.Message); }
			}
		}
	}

	//protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
	//{
	//    ddlCategory.DataBind();
	//    ddlCategory.CategoryType = rblType.SelectedValue;
	//}

	protected void txtCountQty_TextChanged(object sender, EventArgs e)
	{
		//try
		//{
		//    TextBox txt = (TextBox)sender;
		//    long piId, wId;

		//    long.TryParse(txt.Attributes["WHID"], out wId);
		//    long.TryParse(txt.Attributes["PIID"], out piId);

		//    PartSafety ps = PartInfoDAO.GetPartSafety(piId, wId);

		//    if (ps != null)
		//    {
		//        int qty;
		//        int.TryParse(txt.Text, out qty);
		//        qty = qty - ps.CurrentStock;
		//        if (qty != 0)
		//        {
		//            PartDAO.StockAdjust(ps.PartInfo.PartCode, ps.PartInfo.PartType, UserHelper.DealerCode, wId, null, DateTime.Now, InventoryAction.CycleCount, 0, qty, "", "", null);
		//        }
		//    }
		//}
		//catch (Exception ex) { AddErrorMsg(ex.Message); }
	}

	protected void txtUpload_OnClick(object sender, EventArgs e)
	{
		if (fu.PostedFile == null) return;
		long wId;
		long.TryParse(ddlWarehouse.SelectedValue, out wId);
		//try
		//{
		//    bool isInvLock = InventoryDAO.IsInventoryLock(wId, DateTime.Now.Year, DateTime.Now.Month);
		//    if (isInvLock)
		//    {
		//        AddErrorMsg(Resources.Message.InventoryLocked);
		//        return;
		//    }

		//    CycleResult cRes;
		//    if (AutoCycleCount.DoCycleCount(fu.PostedFile.InputStream, wId, out cRes))
		//        AddInfoMsg(Resources.Message.ActionSucessful);
		//    else
		//        AddErrorMsg(litExcelError.Text);

		//    divResult.Visible = true;
		//    litInvalid.Text = cRes.Invalid.ToString();
		//    litUpdated.Text = cRes.Finished.ToString();
		//    //litFailed.Text = cRes.Failed.ToString();
		//    //litUnchanged.Text = cRes.New.ToString();
		//    litTotalParts.Text = cRes.Total.ToString();

		//    //litFailedList.Text = cRes.FailedList;
		//    //litInvalidList.Text = cRes.InvalidList;

		//    gvInvalid.DataSource = cRes.InvalidItems;
		//    gvInvalid.DataBind();
		//    //gvFail.DataSource = cRes.FailedItems;
		//    //gvFail.DataBind();
		//}
		//catch (Exception ex) { AddErrorMsg(ex.Message); }
		if (wId > 0)
		{
			bool r = CycleCountDAO.LoadExcelData(fu.PostedFile.InputStream, VDMSSetting.CurrentSetting.CycleCountExcelUploadSetting);
			if (r)
			{
				var list = CycleCountDAO.Parts.Select(p => p).ToList();
				CycleCountDAO.Clear();
				list.ForEach(item =>
				{
					CycleCountDAO.Append(item.PartCode, p => CycleCountDAO.UpdatePart(p, item.PartCode, item.Quantity.ToString(), item.Comment, wId, true));
				});
				CheckError();
				BindData();
				t.ActiveTabIndex = 1;
			}
		}
	}

	//protected void SaveRow(GridViewRow row)
	//{
	//    long piId, wId;
	//    TextBox txt = (TextBox)row.FindControl("txtCountQty");
	//    long.TryParse(txt.Attributes["WHID"], out wId);
	//    long.TryParse(txt.Attributes["PIID"], out piId);

	//    if (txt.Text.Trim() == "") return;

	//    PartSafety ps = PartInfoDAO.GetPartSafety(piId, wId);

	//    if (ps != null)
	//    {
	//        int qty;
	//        int.TryParse(txt.Text, out qty);
	//        qty = qty - ps.CurrentStock;
	//        if (qty != 0)
	//        {
	//            txt = (TextBox)row.FindControl("txtComment");
	//            PartDAO.StockAdjust(ps.PartInfo.PartCode, ps.PartInfo.PartType, UserHelper.DealerCode, wId, null, DateTime.Now, InventoryAction.CycleCount, 0, qty, txt.Text.Trim(), "", null);
	//        }
	//    }

	//}
	protected void btnSave_Click(object sender, EventArgs e)
	{
		//try
		//{
		//    long wid = long.Parse(ddlWarehouse.SelectedValue);
		//    bool isInvLock = InventoryDAO.IsInventoryLock(wid, DateTime.Now.Year, DateTime.Now.Month);
		//    if (isInvLock) AddErrorMsg(Resources.Message.InventoryLocked);
		//    else
		//    {
		//        foreach (GridViewRow row in gv.Rows)
		//        {
		//            SaveRow(row);
		//        }
		//        PartDAO.PartDC.SubmitChanges();
		//        BindData();
		//    }
		//}
		//catch (Exception ex) { AddErrorMsg(ex.Message); }
		try
		{
			if (CheckError())
			{
				CycleCountDAO.SaveSession(EditingID, long.Parse(ddlWarehouse.SelectedValue), txtSessionComment.Text.Trim());
				CycleCountDAO.Clear();
				Response.Redirect("~/Admin/Part/CCA.aspx");
			}
			BindData();
		}
		catch (Exception ex) { AddErrorMsg(ex.Message); }
	}

	protected void cmdAddRow_Click(object sender, EventArgs e)
	{
		if ((sender as WebControl).ID == "cmdAddRow")
		{
			CycleCountDAO.Append(int.Parse(ddlRowCount.Text));
			this.gv.DataBind();
		}
	}
	protected void ddlRows_SelectedIndexChanged(object sender, EventArgs e)
	{
		if ((sender as WebControl).ID == "ddlRows")
		{
			gv.PageSize = int.Parse(ddlRows.Text);
			gv.DataBind();
		}
	}
	protected void btnPartInserted_Click(object sender, EventArgs e)
	{
		CheckError();
		BindData();
	}

	protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			CyclePart p = (CyclePart)e.Row.DataItem;
			if ((p != null) && !string.IsNullOrEmpty(p.Error))
				e.Row.CssClass = "error";
		}
	}
	protected void gv_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{

	}
	protected void txtLoadHeader_Click(object sender, EventArgs e)
	{
		gvH.DataBind();
	}
	protected void gvH_RowEditing(object sender, GridViewEditEventArgs e)
	{
		Response.Redirect(string.Format("~/Admin/Part/CCA.aspx?id={0}", gvH.DataKeys[e.NewEditIndex].Value));
	}

	protected void lnkbConfirm_Click(object sender, EventArgs e)
	{
        try
        {
			LinkButton b = (LinkButton)sender;
            string[] ccTime = ddlMonth.SelectedValue.Split('/');
            if (!CycleCountDAO.DoConfirm(long.Parse(b.Attributes["HID"]), int.Parse(ccTime[0]), int.Parse(ccTime[1])))
			{
				Response.Redirect(string.Format("~/Admin/Part/CCA.aspx?id={0}", b.Attributes["HID"]));
			}
			gvH.DataBind();
			gvH.EditIndex = -1;
        }
        catch (Exception ex) { AddErrorMsg(ex.Message); }
	}
	protected void gvD_DataBound(object sender, EventArgs e)
	{
	}
	protected void gvH_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		if (e.NewSelectedIndex >= 0)
		{
			gvD.Columns[2].Visible = gvH.Rows[e.NewSelectedIndex].Cells[7].Text == CCStatus.New;
			gvD.Columns[3].Visible = !gvD.Columns[2].Visible;
		}
	}

	protected void gvH_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
	}
}
