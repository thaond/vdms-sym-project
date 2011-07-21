using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using VDMS.Common.Utils;
using VDMS.Common.Web;
using VDMS.Core.Domain;
using VDMS.Helper;
using VDMS.I.ObjectDataSource;
using VDMS.I.Service;

public partial class UC_ExchangeVoucherReport : UserControl
{
	const string SELECTING_SPARE_KEY = "Selecting_Spare";
	const bool ALLOW_VERIFY_MULTI_TIME = true;
	const bool ALLOW_OVERWRITE_BATCHVERIFY = false && ALLOW_VERIFY_MULTI_TIME; // must combine with ALLOW_VERIFY_MULTI_TIME
	const string SESSIONKEY_POSTEDERR = "VERIFY_EXCHANGE_POSTED_ERR";

	bool isManualPost = false;
	string sEditItem;
	private int pageSize = 10, pageIndex = 1, totalFootColSpan = 10;
	private Warrantycondition selWarranty;
	int editIndex = -1;
	VerifyExchangeAction verifyAction;

	private Collection<VerifyExchangeErrorCode> errorCode = new Collection<VerifyExchangeErrorCode>();
	private void ShowError()
	{
		if (Session["VERIFY_EXCHANGE_POSTED_ERR"] != null)
		{
			string errList = Session["VERIFY_EXCHANGE_POSTED_ERR"].ToString();
			string[] errs = errList.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string err in errs)
			{
				try
				{
					AddError((VerifyExchangeErrorCode)Convert.ToInt32(err));
				}
				catch { }
			}
			Session["VERIFY_EXCHANGE_POSTED_ERR"] = null;
		}

		foreach (VerifyExchangeErrorCode error in errorCode)
		{
			switch (error)
			{
				case VerifyExchangeErrorCode.WrongFormat: bllErrorMsg.Items.Add(Message.UpdateDataFailed_WrongData); break;
				case VerifyExchangeErrorCode.CommentIsBlank: bllErrorMsg.Items.Add(litErrMsgCommentBlank.Text); break;
				default: break;
			}
		}
	}
	protected void AddError(VerifyExchangeErrorCode error)
	{
		if ((errorCode.Contains(error)) || (error == VerifyExchangeErrorCode.OK)) return;
		errorCode.Add(error);
	}

	protected void ProcessManualPost()
	{
		switch (verifyAction)
		{
			case VerifyExchangeAction.Approve:
				AddError(ExchangeVoucherHeaderDataSource.ChangeExchangeVoucherStatus(sEditItem, ExchangeVoucherStatus.Approved));
				verifyAction = VerifyExchangeAction.None;
				//if (errorCode.Count == 0)
				Response.Redirect(BuildMiniPostBackUrl("-1", "", "")); // remove if needed
				break;
			case VerifyExchangeAction.Reject:
				AddError(ExchangeVoucherHeaderDataSource.ChangeExchangeVoucherStatus(sEditItem, ExchangeVoucherStatus.Reject));
				verifyAction = VerifyExchangeAction.None;
				//if (errorCode.Count == 0)
				Response.Redirect(BuildMiniPostBackUrl("-1", "", "")); // remove if needed
				break;
			case VerifyExchangeAction.Update: EmptyGridViewEx1_RowUpdating(EmptyGridViewEx1, null); break;
			case VerifyExchangeAction.CancelEdit: EmptyGridViewEx1_RowCancelingEdit(EmptyGridViewEx1, null); break;
			case VerifyExchangeAction.SelectSpare:
				if (!((ViewState[SELECTING_SPARE_KEY] != null) && Convert.ToBoolean(ViewState[SELECTING_SPARE_KEY].ToString())))
					btnFindSpare_Click(null, null);
				break;
			default: break;
		}
		EmptyGridViewEx1.EditIndex = editIndex;
		if (editIndex >= 0) EmptyGridViewEx1_RowEditing(EmptyGridViewEx1, null);
	}
	protected void Page_Init(object sender, EventArgs e)
	{
		isManualPost = Convert.ToBoolean(Request.QueryString["pb"]);
		string sEditIndex = Request.QueryString["ei"];
		string sPage = Request.QueryString["pg"];
		sEditItem = Request.QueryString["epcv"];
		string sEditAct = Request.QueryString["eact"];
		string sPcvn = Request.QueryString["pcvn"];
		string sPcvf = Request.QueryString["pcvf"];
		string sPcvt = Request.QueryString["pcvt"];
		string sEngine = Request.QueryString["en"];
		string sStatus = Request.QueryString["pcvn"];
		string sReparef = Request.QueryString["rpf"];
		string sReparet = Request.QueryString["rpt"];

		int tsT, tsF, iact;
		DateTimeFormatInfo dti = Thread.CurrentThread.CurrentCulture.DateTimeFormat;

		if ((!string.IsNullOrEmpty(sReparef)) && (int.TryParse(sReparef, out tsF))) this.RepairDateFrom = DateTime.Now.Add(new TimeSpan(tsF, 0, 0, 0));
		if ((!string.IsNullOrEmpty(sReparet)) && (int.TryParse(sReparet, out tsT))) this.RepairDateTo = DateTime.Now.Add(new TimeSpan(tsT, 0, 0, 0));
		if (!string.IsNullOrEmpty(sStatus)) int.TryParse(sStatus, out this.Status);
		if (!string.IsNullOrEmpty(sEngine)) this.EngineNumber = sEngine;
		if (!string.IsNullOrEmpty(sPcvn)) this.ProposalNumber = sPcvn;
		if (!string.IsNullOrEmpty(sPcvf)) this.VoucherNumberFrom = sPcvf;
		if (!string.IsNullOrEmpty(sPcvt)) this.VoucherNumberTo = sPcvt;
		if (!string.IsNullOrEmpty(sPage)) pageIndex = Convert.ToInt32(sPage);
		if (!string.IsNullOrEmpty(sEditIndex)) editIndex = Convert.ToInt32(sEditIndex); if (editIndex < 0) editIndex = -1;

		// process request
		if (int.TryParse(sEditAct, out iact)) { verifyAction = (VerifyExchangeAction)iact; }


	}
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			//PageInit(sender, e);
			EmptyGridViewEx1.IncludeChildsListInLevel = true;
			EmptyGridViewEx1.ColumnsLevelList = new ArrayList[] { new ArrayList(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 15, 20, 21 }), new ArrayList(new int[] { 10, 11, 12, 13, 14, 16, 17, 18, 19 }) };
			EmptyGridViewEx1.DataObjectTableSchema = ExchangeVoucherHeaderDataSource.ExchangeVoucherTableSchema;

			pnPager.Visible = false;
			pnDealer.Visible = false;
			litDealer.Text = Constants.Dealer;
			litAllPageSummary.Text = Constants.AllPageTotalAmount;
			pnAllPageSummary.Visible = false;
		}
		EmptyGridViewEx1.ShowEmptyFooter = false;
		EmptyGridViewEx1.EmptyTableRowText = Message.DataNotFound;
		EmptyGridViewEx1.ShowFooter = ShowFooter;

		bllErrorMsg.Items.Clear();
		errorCode.Clear();
		if (isManualPost) ProcessManualPost();
	}
	protected void Page_PreRender(object sender, EventArgs e)
	{
		if (IsPostBack || OnVerify)
			BindData(pageIndex);
		ShowError();
		//EmptyGridViewEx1.ShowFooter = (EmptyGridViewEx1.Rows.Count > 0);
	}


	public EmptyGridViewEx Grid
	{
		get { return EmptyGridViewEx1; }
	}
	public object DataSourceObject
	{
		get { return EmptyGridViewEx1.DataSourceObject; }
		set { EmptyGridViewEx1.DataSourceObject = value; }
	}
	public string VoucherNumberFrom, ProposalNumber, EngineNumber, VoucherNumberTo, DealerCode = "";
	public bool OnVerify, ShowEditButton = true, ShowVerifyButton = true, ShowFooter = true, ShowAllPageSummary;
	public DateTime RepairDateFrom, RepairDateTo, ConfirmedFrom, ConfirmedTo;
	public int Status;

	private ExchangeVoucherHeaderDataSource InitDataSource()
	{
		ExchangeVoucherHeaderDataSource ds = new ExchangeVoucherHeaderDataSource();
		ds.VoucherNo = ProposalNumber;
		ds.ForVerify = OnVerify;
		ds.RepairFromDate = RepairDateFrom;
		ds.RepairToDate = RepairDateTo;
		ds.ConfirmedDateFrom = ConfirmedFrom;
		ds.ConfirmedDateTo = ConfirmedTo;
		ds.ExchageNoFrom = VoucherNumberFrom;
		ds.ExchageNoTo = VoucherNumberTo;
		ds.EngineNum = EngineNumber;
		ds.Status = Status;
		ds.ByDealer = (string.IsNullOrEmpty(DealerCode)) ? UserHelper.DealerCode : DealerCode;
		ds.ByArea = UserHelper.AreaCode;
		ds.ByAreas = AreaHelper.Area;
		ds.CalculateAllPageSummary = ShowAllPageSummary;

		return ds;
	}

	private void ShowAllPageTotal(ExchangeVoucherHeaderDataSource ds)
	{
		litDealerFee.Text = ds.AllPage_TotalFeeO.ToString("N0");
		litDealerQty.Text = ds.AllPage_TotalSparesO.ToString("N0");
		litDealerSparesAmount.Text = ds.AllPage_TotalAmountO.ToString("N0");
		litDealerTotalAll.Text = ds.AllPage_AllTotalO.ToString("N0");

		litVMEPFee.Text = ds.AllPage_TotalFeeM.ToString("N0");
		litVMEPQty.Text = ds.AllPage_TotalSparesM.ToString("N0");
		litVMEPSparesAmount.Text = ds.AllPage_TotalAmountM.ToString("N0");
		litVMEPTotalAll.Text = ds.AllPage_AllTotalM.ToString("N0");
	}

	// pageIndex start at 1 
	public void BindData(int pageIndex)
	{
		ExchangeVoucherHeaderDataSource ds = InitDataSource();
		EmptyGridViewEx1.DataSourceObject = ds.Select(pageSize, (pageIndex - 1) * pageSize);
		EmptyGridViewEx1.DataBind();

		// setting paging
		int page;
		cmdFirst.CommandArgument = "1";
		cmdLast.CommandArgument = ds.PageCount.ToString();
		page = (ds.PageIndex > 0) ? ds.PageIndex : 1; // ? PageIndex + 1 - 1 : 1
		cmdPrevious.CommandArgument = page.ToString();
		page = ((ds.PageIndex + 1) < ds.PageCount) ? ds.PageIndex + 2 : ds.PageCount;
		cmdNext.CommandArgument = page.ToString();
		litPageInfo.Text = string.Format(Message.PagingInfo, ds.PageIndex + 1, ds.PageCount, ds.ItemCount);
		pnPager.Visible = ds.PageCount > 1;
		pnAllPageSummary.Visible = (ds.PageCount > 0) && this.ShowAllPageSummary;

		if (EmptyGridViewEx1.Rows.Count > 0)
		{
			// show total infos
			NumberFormatInfo ni = (NumberFormatInfo)Thread.CurrentThread.CurrentCulture.NumberFormat.Clone();
			ni.NumberDecimalDigits = 0;

			#region total of all
			if (this.ShowFooter)       //(ds.PageCount > 1)
			{
				GridViewRow frow = EmptyGridViewEx1.FootRow;    // clone footer
				if (frow != null)
				{
					for (int i = 0; i < totalFootColSpan; i++) { frow.Cells.RemoveAt(0); }
					frow.Cells[0].ColumnSpan = totalFootColSpan + 1; frow.Cells[0].CssClass = "summaryName";
					frow.Cells[0].Text = Constants.DealerAllTotal;
					frow.Cells[1].Text = ds.TotalSparesO.ToString("N", ni); frow.Cells[1].HorizontalAlign = HorizontalAlign.Right;
					frow.Cells[3].Text = ds.TotalAmountO.ToString("N", ni);
					frow.Cells[5].Text = ds.TotalFeeO.ToString("N", ni);
					frow.Cells[7].Text = ds.AllTotalO.ToString("N", ni);
				}

				//create a new foot row
				GridViewRow row = EmptyGridViewEx1._CreateRow(0, -1, DataControlRowType.Footer, DataControlRowState.Normal);
				DataControlField[] fields = new DataControlField[EmptyGridViewEx1.Columns.Count];
				EmptyGridViewEx1.Columns.CopyTo(fields, 0);
				EmptyGridViewEx1._InitializeRow(row, fields);
				for (int i = 0; i < totalFootColSpan; i++) { row.Cells.RemoveAt(0); }
				for (int i = 1; i < row.Cells.Count; i++) { row.Cells[i].CssClass = "changedValue"; }
				row.Cells[0].ColumnSpan = totalFootColSpan + 1; row.Cells[0].CssClass = "summaryName";

				row.Cells[0].Text = Constants.VMEPAllTotal;
				row.Cells[1].Text = ds.TotalSparesM.ToString("N", ni); frow.Cells[1].HorizontalAlign = HorizontalAlign.Right;
				row.Cells[3].Text = ds.TotalAmountM.ToString("N", ni);
				row.Cells[6].Text = ds.TotalFeeM.ToString("N", ni);
				row.Cells[7].Text = ds.AllTotalM.ToString("N", ni);
				EmptyGridViewEx1.Controls[0].Controls.Add(row);

				if (ShowAllPageSummary) ShowAllPageTotal(ds);
			}
			#endregion
		}
		pnDealer.Visible = OnVerify;
		//dealer info
		if (OnVerify)
		{
			litDealerCode.Text = ds.DealerCode;
			litDealerName.Text = ds.DealerName;
		}

		// command visibility
		EmptyGridViewEx1.Columns[EmptyGridViewEx1.Columns.Count - 1].Visible = ShowVerifyButton;
		EmptyGridViewEx1.Columns[EmptyGridViewEx1.Columns.Count - 3].Visible = ShowEditButton;
	}

	public void InitUC()
	{

	}

	// overWrite == false : donot over Write status if it verified
	private void BatchVerify(bool allPage, ExchangeVoucherStatus status)
	{
		int max = (allPage) ? int.MaxValue : pageSize,
			start = (allPage) ? 0 : (pageIndex - 1) * pageSize;

		ExchangeVoucherHeaderDataSource ds = InitDataSource();
		ArrayList list = ds.Select(max, start).Items;
		foreach (ExchangeListHeader item in list)
		{
			if ((item.Status == (int)ExchangeVoucherStatus.Sent) || ALLOW_OVERWRITE_BATCHVERIFY)
				AddError(ExchangeVoucherHeaderDataSource.ChangeExchangeVoucherStatus(item.ExchangeVoucherNumber, status));
		}
	}

	public void ApproveAllRemain()
	{
		BatchVerify(true, ExchangeVoucherStatus.Approved);
	}

	public void RejectAllRemain()
	{
		BatchVerify(true, ExchangeVoucherStatus.Reject);
	}

	public void RejectAllPageRemain()
	{
		BatchVerify(false, ExchangeVoucherStatus.Reject);
	}

	public void ApproveAllPageRemain()
	{
		BatchVerify(false, ExchangeVoucherStatus.Approved);
	}

	protected void EmptyGridViewEx1_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType != DataControlRowType.DataRow) return;
		ImageButton img;
		GridView gv = (GridView)sender;
		if ((e.Row.RowIndex == gv.EditIndex) && (selWarranty != null))
		{
			#region chon spare moi khi edit row
			int quantity = 0;
			double manpower;
			string manP = NumberFormatHelper.StrDoubleToStr(selWarranty.Manpower, "en-US");

			TextBox tb = (TextBox)WebTools.FindControlById("txtSpareNumberM", e.Row);
			if (tb != null) { tb.Text = selWarranty.Partcode; }

			tb = (TextBox)WebTools.FindControlById("txtQuantityM", e.Row);
			if (tb != null) { int.TryParse(tb.Text, out quantity); }

			tb = (TextBox)WebTools.FindControlById("txtUnitPriceM", e.Row);
			if (tb != null) { tb.Text = selWarranty.Unitprice.ToString(); }

			double.TryParse(manP, out manpower);
			tb = (TextBox)WebTools.FindControlById("txtWarrantyFeeAmountM", e.Row);
			if (tb != null)
			{
				tb.Text = ((double)((double)selWarranty.Labour * manpower * quantity)).ToString();
			}

			tb = (TextBox)WebTools.FindControlById("txtManPowerM", e.Row);
			if (tb != null)
			{
				tb.Text = manP;
			}
			#endregion
		}

		if (e.Row.RowIndex == gv.EditIndex)
		{
			#region edit row

			Button btn = (Button)WebTools.FindControlById("btnFindSpare", e.Row);
			if (btn != null) btn.PostBackUrl = BuildPostBackUrl(e.Row.RowIndex.ToString(), "", ((int)VerifyExchangeAction.SelectSpare).ToString());

			TextBox tb = (TextBox)WebTools.FindControlById("txtSpareNumberM", e.Row);
			if (tb != null)
			{
				tb.Attributes.Add("onkeyup", "CopyValueToPostBack('" + tb.ClientID + "','" + hdPartCode.ClientID + "')");
				hdPartCode.Value = tb.Text;
			}

			tb = (TextBox)WebTools.FindControlById("txtSpareNumberO", e.Row);
			if (tb != null)
			{
				tb.Attributes.Add("onkeyup", "CopyValueToPostBack('" + tb.ClientID + "','" + hdPartCodeO.ClientID + "')");
				hdPartCodeO.Value = tb.Text;
			}

			tb = (TextBox)WebTools.FindControlById("txtVoucherNo", e.Row);
			if (tb != null)
			{
				//tb.Attributes.Add("onkeyup", "CopyValueToPostBack('" + tb.ClientID + "','" + hdPartCodeO.ClientID + "')");
				hdVoucherNo.Value = tb.Text;
			}

			tb = (TextBox)WebTools.FindControlById("txtQuantityM", e.Row);
			if (tb != null)
			{
				tb.Attributes.Add("onkeyup", "CopyValueToPostBack('" + tb.ClientID + "','" + hdQuantity.ClientID + "')");
				hdQuantity.Value = tb.Text;
			}

			tb = (TextBox)WebTools.FindControlById("txtUnitPriceM", e.Row);
			if (tb != null)
			{
				tb.Attributes.Add("onkeyup", "CopyValueToPostBack('" + tb.ClientID + "','" + hdPrice.ClientID + "')");
				hdPrice.Value = tb.Text;
			}

			tb = (TextBox)WebTools.FindControlById("txtWarrantyFeeAmountM", e.Row);
			if (tb != null)
			{
				tb.Attributes.Add("onkeyup", "CopyValueToPostBack('" + tb.ClientID + "','" + hdFee.ClientID + "')");
				hdFee.Value = tb.Text;
			}

			tb = (TextBox)WebTools.FindControlById("txtManPowerM", e.Row);
			if (tb != null)
			{
				tb.Attributes.Add("onkeyup", "CopyValueToPostBack('" + tb.ClientID + "','" + hdManPower.ClientID + "')");
				hdManPower.Value = tb.Text;
			}

			tb = (TextBox)WebTools.FindControlById("txtComments", e.Row);
			if (tb != null)
			{
				tb.Attributes.Add("onkeyup", "CopyValueToPostBack('" + tb.ClientID + "','" + hdNote.ClientID + "')");
				hdNote.Value = tb.Text;
			}

			// update image button
			img = (ImageButton)WebTools.FindControlById("ImageButtonUpdate", e.Row);
			if (img != null)
				img.PostBackUrl = BuildPostBackUrl("-1", "", ((int)VerifyExchangeAction.Update).ToString());

			// cancel image button
			img = (ImageButton)WebTools.FindControlById("ImageButtonCancel", e.Row);
			if (img != null)
				img.PostBackUrl = BuildMiniPostBackUrl("-1", "", ((int)VerifyExchangeAction.CancelEdit).ToString());

			#endregion
		}

		//
		// image post back url
		img = (ImageButton)WebTools.FindControlById("ImageButtonEdit", e.Row);
		if (img != null) { img.PostBackUrl = BuildPostBackUrl(e.Row.RowIndex.ToString(), "", ((int)VerifyExchangeAction.Edit).ToString()); }

		Button btnAct = (Button)WebTools.FindControlById("btnApprove", e.Row);
		if (btnAct != null) btnAct.PostBackUrl = BuildPostBackUrl("-1", btnAct.CommandArgument, ((int)VerifyExchangeAction.Approve).ToString());

		btnAct = (Button)WebTools.FindControlById("btnReject", e.Row);
		if (btnAct != null) btnAct.PostBackUrl = BuildPostBackUrl("-1", btnAct.CommandArgument, ((int)VerifyExchangeAction.Reject).ToString());
	}

	protected void EmptyGridViewEx1_RowEditing(object sender, GridViewEditEventArgs e)
	{
		GridView gv = ((GridView)sender);
		gv.Columns[gv.Columns.Count - 1].Visible = false;
	}

	protected void EmptyGridViewEx1_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		long fee = 0, price = 0;
		double manPower = 0;
		int quantity = 0;
		string comment = "", spareNumO = "", spareNumM = "", voucherNo = "";
		try
		{
			if (e != null) // automatic by gridview
			{
				GridViewRow row = ((GridView)sender).Rows[e.RowIndex];
				price = GetNumber("txtUnitPriceM", row);
				quantity = (int)GetNumber("txtQuantityM", row);
				//fee = GetNumber("txtWarrantyFeeAmountM", row);
				manPower = GetDouble("txtManPowerM", row);
				comment = GetString("txtComments", row);
				spareNumO = GetString("txtSpareNumberO", row);
				spareNumM = GetString("txtSpareNumberM", row);
				voucherNo = GetString("txtVoucherNo", row);
			}
			else
			{
				price = Convert.ToInt64(hdPrice.Value);
				quantity = Convert.ToInt32(hdQuantity.Value);
				//fee = Convert.ToInt64(hdFee.Value);
				manPower = Convert.ToDouble(hdManPower.Value);

				if ((price < 0) || (quantity < 0) || (fee < 0)) throw new Exception("Invalid number values");
				comment = hdNote.Value;
				spareNumM = hdPartCode.Value;
				spareNumO = hdPartCodeO.Value;
				voucherNo = hdVoucherNo.Value;
				//fee = manPower 
			}

			Warrantycondition warr = WarrantyContent.GetWarrantyCondition(spareNumM);
			if (warr == null)
			{
				AddError(VerifyExchangeErrorCode.InvalidSpareCode);
			}
			else
			{
				// tinh fee theo manpower (bo 2 cai fee o tren di)
				fee = (long)(manPower * (double)warr.Labour);
				ExchangeVoucherHeaderDataSource.UpdateSpare(voucherNo, spareNumO, spareNumM, quantity, price, fee, comment);
				((GridView)sender).EditIndex = -1;
				editIndex = -1;
				((GridView)sender).Columns[((GridView)sender).Columns.Count - 1].Visible = true;
			}
		}
		catch { AddError(VerifyExchangeErrorCode.WrongFormat); }
	}

	protected void EmptyGridViewEx1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		editIndex = -1;
		((GridView)sender).Columns[((GridView)sender).Columns.Count - 1].Visible = true;
	}

	// number
	protected void Literal18_DataBinding(object sender, EventArgs e)
	{
		NumberFormatInfo ni = (NumberFormatInfo)Thread.CurrentThread.CurrentCulture.NumberFormat.Clone();
		ni.NumberDecimalDigits = 0;
		long val;
		if (sender is Literal)
		{
			Literal lit = (Literal)sender;
			if (long.TryParse(lit.Text, out val)) lit.Text = val.ToString("N", ni);
		}
		else if (sender is Label)
		{
			Label lbl = (Label)sender;
			if (long.TryParse(lbl.Text, out val)) lbl.Text = val.ToString("N", ni);
		}
	}
	// check duplicate
	protected bool IsNotSame(object val1, object val2)
	{
		bool result = false;
		result = (val1.ToString() == val2.ToString()) ? false : true;
		return result;
	}
	// khong cho phep duyet nhieu lan (ko thi lay cai tren)
	protected bool IsNotSame2(object val1, object val2)
	{
		return (ALLOW_VERIFY_MULTI_TIME) ?
					IsNotSame(val1, val2) :
					((((int)ExchangeVoucherStatus.Approved).ToString() == val2.ToString()) ||
					 (((int)ExchangeVoucherStatus.Reject).ToString() == val2.ToString())) ? false : true;
	}

	protected int GetApproveedStatus() { return (int)ExchangeVoucherStatus.Approved; }
	protected int GetRejectStatus() { return (int)ExchangeVoucherStatus.Reject; }
	// datetime
	protected object FormatDate(object val)
	{
		DateTime dt;
		object ret = val;
		if (DateTime.TryParse(val.ToString(), out dt))
		{
			ret = (dt > DateTime.MinValue) ? dt.ToShortDateString() : "";
		}

		return ret;
	}
	protected long GetNumber(string txt, object findIn)
	{
		long val;
		TextBox tb = (TextBox)WebTools.FindControlById(txt, (Control)findIn);
		if (tb == null) return 0;
		long.TryParse(tb.Text, out val);
		return val;
	}
	protected double GetDouble(string txt, object findIn)
	{
		double val;
		TextBox tb = (TextBox)WebTools.FindControlById(txt, (Control)findIn);
		if (tb == null) return 0;
		double.TryParse(tb.Text, out val);
		return val;
	}
	protected string GetString(string txt, object findIn)
	{
		TextBox tb = (TextBox)WebTools.FindControlById(txt, (Control)findIn);
		if (tb == null) return "";
		return tb.Text.Trim();
	}


	protected void gvSelectxxx_PreRender(object sender, EventArgs e)
	{
		GridView gv = (GridView)sender;
		if (gv.TopPagerRow == null) return;
		Literal litPageInfo = gv.TopPagerRow.FindControl("lit" + gv.ID + "PageInfo") as Literal;
		if (litPageInfo != null) litPageInfo.Text = string.Format(Message.PagingInfo, gv.PageIndex + 1, gv.PageCount, HttpContext.Current.Items["list" + gv.ID + "RowCount"]);
	}
	protected void gvSelectxxx_page(object sender, GridViewCommandEventArgs e)
	{
		GridView gv = (GridView)sender;
		if ((e.CommandName == "Page") && (((e.CommandArgument == "Next") && ((gv.PageIndex + 1) == gv.PageCount)) || ((e.CommandArgument == "Prev") && (gv.PageIndex == 0))))
			gv.DataBind();
	}
	protected void gvSelectSpare_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		GridViewRow gvr = ((GridView)sender).Rows[e.NewSelectedIndex]; if (gvr == null) return;
		Literal lit = (Literal)WebTools.FindControlById("litSelectedSpareNumber", gvr);
		if (lit != null) selWarranty = WarrantyContent.GetWarrantyCondition(lit.Text);
		MultiView1.ActiveViewIndex = 0;
		ViewState[SELECTING_SPARE_KEY] = "false";
		ObjectDataSource1.SelectParameters["spareNumberLike"] = new Parameter("spareNumberLike", TypeCode.String, "");
		// trick on me
		EmptyGridViewEx1._debugGridOnObject = false;
	}
	// user cancel select an item
	protected void btnCancel_Click(object sender, EventArgs e)
	{
		MultiView1.ActiveViewIndex = 0;
		ViewState[SELECTING_SPARE_KEY] = "false";
	}

	protected void btnFindSpare_Click(object sender, EventArgs e)
	{
		string spareNum;

		TextBox tb = (TextBox)WebTools.FindControlById("txtSpareNumberM", EmptyGridViewEx1); if ((tb == null) && (sender != null)) return;

		spareNum = (tb != null) ? tb.Text : hdPartCode.Value;

		ObjectDataSource1.SelectParameters["spareNumberLike"] = new Parameter("spareNumberLike", TypeCode.String, spareNum);
		gvSelectSpare.PageIndex = 0;
		gvSelectSpare.DataBind();

		if (gvSelectSpare.Rows.Count == 1)
		{

			GridViewRow gvr = gvSelectSpare.Rows[0]; if (gvr == null) return;
			Literal lit = (Literal)WebTools.FindControlById("litSelectedSpareNumber", gvr);
			if (lit != null) selWarranty = WarrantyContent.GetWarrantyCondition(lit.Text);
			ObjectDataSource1.SelectParameters["spareNumberLike"] = new Parameter("spareNumberLike", TypeCode.String, "");
		}
		else if (gvSelectSpare.Rows.Count > 1)
		{
			MultiView1.ActiveViewIndex = 1;
			// trick on me
			EmptyGridViewEx1._debugGridOnObject = true;
			ViewState[SELECTING_SPARE_KEY] = "true";
		}
		else
		{
			MultiView1.ActiveViewIndex = 0;
			AddError(VerifyExchangeErrorCode.SpareNumberNotFound);
		}
	}

	// for all paging btn
	protected void cmdFirst_Click(object sender, EventArgs e)
	{
		Button btn = (Button)sender;
		int page;
		if (int.TryParse(btn.CommandArgument, out page))
		{
			//if (page != pageIndex) 
			editIndex = -1;
			pageIndex = page;
		}
	}
	protected void btnApprove_Click(object sender, EventArgs e)
	{
		ExchangeVoucherHeaderDataSource.ChangeExchangeVoucherStatus(((Button)sender).CommandArgument, ExchangeVoucherStatus.Approved);
	}
	protected void btnReject_Click(object sender, EventArgs e)
	{
		ExchangeVoucherHeaderDataSource.ChangeExchangeVoucherStatus(((Button)sender).CommandArgument, ExchangeVoucherStatus.Reject);
	}

	private string BuildPostBackUrl(string editIndex, string editpart, string action)
	{
		string url = Request.Url.AbsolutePath + "?";
		url += "pb=true";
		url += "&ei=" + editIndex;
		if (!string.IsNullOrEmpty(this.ProposalNumber)) url += "&pcvn=" + this.ProposalNumber;
		if (!string.IsNullOrEmpty(this.VoucherNumberFrom)) url += "&pcvf=" + this.VoucherNumberFrom;
		if (!string.IsNullOrEmpty(this.VoucherNumberTo)) url += "&pcvt=" + this.VoucherNumberTo;
		if (!string.IsNullOrEmpty(editpart)) url += "&epcv=" + editpart;
		if (!string.IsNullOrEmpty(action)) url += "&eact=" + action;
		if (this.RepairDateFrom != null) url += "&rpf=" + ((TimeSpan)this.RepairDateFrom.Subtract(DateTime.Now)).Days.ToString();
		if (this.RepairDateTo != null) url += "&rpt=" + ((TimeSpan)this.RepairDateTo.Subtract(DateTime.Now)).Days.ToString();
		if (!string.IsNullOrEmpty(this.EngineNumber)) url += "&en=" + this.EngineNumber;
		if (this.Status != null) url += "&status=" + this.Status.ToString();
		url += "&pg=" + pageIndex.ToString();

		return url;
	}
	private string BuildMiniPostBackUrl(string editIndex, string editpart, string action)
	{
		string url = Request.Url.AbsolutePath + "?";
		url += "pb=true";
		if (!string.IsNullOrEmpty(this.ProposalNumber)) url += "&pcvn=" + this.ProposalNumber;
		if (!string.IsNullOrEmpty(this.VoucherNumberFrom)) url += "&pcvf=" + this.VoucherNumberFrom;
		if (!string.IsNullOrEmpty(this.VoucherNumberTo)) url += "&pcvt=" + this.VoucherNumberTo;
		if (errorCode.Count > 0)
		{
			string sPostedErr = "";
			foreach (VerifyExchangeErrorCode err in errorCode)
			{
				sPostedErr += ((int)err).ToString() + "-";
			}
			Session["VERIFY_EXCHANGE_POSTED_ERR"] = sPostedErr;
		}
		url += "&pg=" + pageIndex.ToString();
		return url;
	}    // edit image
	protected void ImageButton1_DataBinding(object sender, EventArgs e)
	{

	}

}
