using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.UI.WebControls;
using NHibernate.Expression;
using Resources;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;
using VDMS.Helper;
using VDMS.I.Vehicle;

public partial class Sales_Inventory_adjustment : BasePage
{
	// dealercode: BN002B(5) AS002A(2) AS001A(2) CL001A(3) MISC(2)PM001A(3) PN001A(1) QG001A(1)

	private Collection<AdjustmentErrorCode> errorCode = new Collection<AdjustmentErrorCode>();

	protected void AddError(AdjustmentErrorCode error)
	{
		if (errorCode.Contains(error) || (error == AdjustmentErrorCode.OK)) return;
		errorCode.Add(error);
	}
	private void ShowError()
	{
		bllErrorMsg.Visible = errorCode.Count > 0;
		bllErrorMsg.Items.Clear();
		foreach (AdjustmentErrorCode err in errorCode)
		{
			switch (err)
			{
				case AdjustmentErrorCode.EmptyBranchList: bllErrorMsg.Items.Add(Message.Adjustment_EmptyBranchList); break;
				case AdjustmentErrorCode.InvalidBranch: bllErrorMsg.Items.Add(Message.Adjustment_InvalidBranch); break;
				case AdjustmentErrorCode.InvalidDealer: bllErrorMsg.Items.Add(Message.Adjustment_InvalidDealer); break;
				case AdjustmentErrorCode.InvalidEngineNumber: bllErrorMsg.Items.Add(Message.Adjustment_InvalidEngineNumber); break;
				case AdjustmentErrorCode.InvalidStatus: bllErrorMsg.Items.Add(Message.Adjustment_InvalidStatus); break;
				case AdjustmentErrorCode.SaveDataFailed: bllErrorMsg.Items.Add(Message.Adjustment_SaveDataFailed); break;
				case AdjustmentErrorCode.AlreadyHasVoucher: bllErrorMsg.Items.Add(Message.VehicleAlreadyHasVoucher); break;
				case AdjustmentErrorCode.InventLocked: bllErrorMsg.Items.Add(Reports.InventoryIsLocked); break;
			}
		}
	}

	protected string QuestionSaveData { get { return Question.SaveData; } }
	private DataTable GetTaskList()
	{
		DataTable tbl = new DataTable("TaskList");
		DataRow row;

		tbl.Columns.Add("TaskName");
		tbl.Columns.Add("TaskID");
		row = tbl.NewRow(); row["TaskName"] = (int)AdjustmentTask.Move + " - " + Constants.AdjustTask_Move; row["TaskID"] = (int)AdjustmentTask.Move; tbl.Rows.Add(row);
		row = tbl.NewRow(); row["TaskName"] = (int)AdjustmentTask.CheckLacked + " - " + Constants.AdjustTask_CheckLacked; row["TaskID"] = (int)AdjustmentTask.CheckLacked; tbl.Rows.Add(row);
		row = tbl.NewRow(); row["TaskName"] = (int)AdjustmentTask.AddVoucher + " - " + Constants.AdjustTask_AddVoucher; row["TaskID"] = (int)AdjustmentTask.AddVoucher; tbl.Rows.Add(row);
		row = tbl.NewRow(); row["TaskName"] = (int)AdjustmentTask.CheckRedundant + " - " + Constants.AdjustTask_CheckRedudant; row["TaskID"] = (int)AdjustmentTask.CheckRedundant; tbl.Rows.Add(row);
		return tbl;
	}

	protected void Page_PreRender(object sender, EventArgs e)
	{
		ShowError();
		//if (IsChangeLanguage && (pnDetail.Visible == true)) { btnTest_Click(sender, e); }
		//else if (IsChangeLanguage)
		//{
		//    drlTask.DataSource = GetTaskList();
		//    drlTask.DataBind();
		//    MaskedEditExtender2.CultureName = Page.Culture;
		//}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			//drlTask.Attributes["onchange"] = "this.disabled='disabled';"; notwork
			pnDetail.Visible = false;
			drlTask.DataSource = GetTaskList();
			drlTask.DataTextField = "TaskName";
			drlTask.DataValueField = "TaskID";
			drlTask.DataBind();
			//btnApply.OnClientClick = "return confirm('" + Question.SaveData + "');";
		}

		errorCode.Clear();
	}

	//private bool ValidBranch(string branchCode, DataSet list)
	//{
	//    string code = branchCode.ToLower();
	//    foreach (DataRow row in list.Tables[0].Rows)
	//    {
	//        if (code == row["BranchCode"].ToString().ToLower()) return true;
	//    }
	//    return false;
	//}

	private void KeepOneItem(DropDownList drop, string value)
	{
		string code = value.ToLower();
		while (drop.Items.Count > 1)
		{
			foreach (ListItem item in drop.Items)
			{
				if (item.Value.ToLower() != code) { drop.Items.Remove(item); break; }
			}
		}
	}

	private decimal GetItemIssuePrice(Iteminstance iis)
	{
		decimal price = 0;
		IDao<TransHis, long> dao = DaoFactory.GetDao<TransHis, long>();

		dao.SetCriteria(new ICriterion[] { Expression.Eq("Iteminstance", iis), Expression.In("Transactiontype", new int[] { (int)ItemStatus.Imported, (int)ItemStatus.AdmitTemporarily }) });
		dao.SetOrder(new NHibernate.Expression.Order[] { NHibernate.Expression.Order.Asc("Transactiondate") });
		List<TransHis> list = dao.GetAll();

		if (list.Count > 0) price = list[0].Actualcost;
		return price;
	}

	protected void btnTest_Click(object sender, EventArgs e)
	{
		pnDetail.Visible = false;
		btnApply.Enabled = false;
		hdEnNum.Value = txtEngineNo.Text.Trim().ToUpper();
		//drlBranch_Load(drlToBranch);
		//drlBranch_Load(drlFromBranch);

		if (((AdjustmentTask)Convert.ToInt32(drlTask.SelectedValue) == AdjustmentTask.Move) && (drlToBranch.Items.Count <= 1)) { AddError(AdjustmentErrorCode.EmptyBranchList); return; }

		// query information from database
		Iteminstance item = Adjustment.GetItemInfos(hdEnNum.Value, UserHelper.DealerCode);
		Shippingdetail shd = Adjustment.GetShippingDetail(hdEnNum.Value);
		if (item == null) { AddError(AdjustmentErrorCode.InvalidEngineNumber); return; }

		// valid branch?
		if (item.Branchcode != UserHelper.BranchCode) { AddError(AdjustmentErrorCode.InvalidBranch); return; }

		litColor.Text = item.Color;
		litModel.Text = item.Itemtype;
		litStatus.Text = ItemHepler.GetNativeItemStatusName(item.Status);
		litVoucher.Text = (shd == null) ? Message.DataNotFound : (shd.Voucherstatus ? Constants.Yes : Constants.Lacked);
		litPrize.Text = GetItemIssuePrice(item).ToString("N0");
		litInvoiceNum.Text = item.Vmepinvoice;
		txtMoveDate.Text = DateTime.Now.ToString();

		// da co chung tu ko cho bu` nua
		if ((shd != null) && shd.Voucherstatus && (drlTask.SelectedValue == ((int)AdjustmentTask.AddVoucher).ToString()))
		{
			AddError(AdjustmentErrorCode.AlreadyHasVoucher);
			return;
		}

		// dieu chinh dia diem cho thao tac MOVE
		if (drlTask.SelectedValue == ((int)AdjustmentTask.Move).ToString())
		{
			//drlFromBranch.DataSource = branchList;
			//drlToBranch.DataSource = branchList;
			//drlFromBranch.DataBind();
			//drlToBranch.DataBind();
			//drlFromBranch.SelectedValue = item.Branchcode;
			KeepOneItem(drlFromBranch, item.Branchcode);
		}

		drlToBranch.Enabled = drlTask.SelectedValue == ((int)AdjustmentTask.Move).ToString();
		drlFromBranch.Enabled = drlToBranch.Enabled;
		txtMoveDate.Enabled = drlToBranch.Enabled;
		imgCalendar.Visible = drlToBranch.Enabled;

		pnDetail.Visible = true;
		btnApply.Enabled = true;
		// da kiem ke du roi thi thoi
		if ((item.Status == (int)ItemStatus.Redundant) && (drlTask.SelectedValue == ((int)AdjustmentTask.CheckRedundant).ToString()))
			btnApply.Enabled = false;
	}

	protected void btnApply_Click(object sender, EventArgs e)
	{
		DateTime moveDate = DateTime.MinValue;
		AdjustmentTask task = (AdjustmentTask)Convert.ToInt32(drlTask.SelectedValue);
		CultureInfo ci = Thread.CurrentThread.CurrentCulture;
		DateTime.TryParse(txtMoveDate.Text, ci, DateTimeStyles.AllowWhiteSpaces, out moveDate);
		bool inventLock = false;

		switch (task)
		{
			case AdjustmentTask.Move:
				AddError(Adjustment.MoveVerhicle(hdEnNum.Value, moveDate, drlFromBranch.SelectedValue, drlToBranch.SelectedValue, UserHelper.DealerCode, UserHelper.DatabaseCode));
				inventLock = InventoryHelper.IsInventoryLock(moveDate, UserHelper.DealerCode, drlFromBranch.SelectedValue)
						  || InventoryHelper.IsInventoryLock(moveDate, UserHelper.DealerCode, drlToBranch.SelectedValue);
				break;
			case AdjustmentTask.AddVoucher:
				AddError(Adjustment.AddVoucher(hdEnNum.Value, UserHelper.DealerCode, UserHelper.DatabaseCode, UserHelper.BranchCode));
				break;
			case AdjustmentTask.CheckLacked:
				inventLock = InventoryHelper.IsInventoryLock(moveDate, UserHelper.DealerCode, drlFromBranch.SelectedValue);
				AddError(Adjustment.CheckLacked(hdEnNum.Value, UserHelper.DealerCode, UserHelper.DatabaseCode, UserHelper.BranchCode));
				break;
			case AdjustmentTask.CheckRedundant:
				AddError(Adjustment.CheckRedundant(hdEnNum.Value, UserHelper.DealerCode, UserHelper.DatabaseCode, UserHelper.BranchCode));
				break;
			default:
				break;
		}

		if (inventLock && ((task == AdjustmentTask.CheckLacked) || (task == AdjustmentTask.Move)))
		{
			AddError(AdjustmentErrorCode.InventLocked);
		}

		pnDetail.Visible = (errorCode.Count == 0);

		if (errorCode.Count == 0)
		{
			if (task != AdjustmentTask.CheckLacked)
				btnTest_Click(sender, e);
			else
				pnDetail.Visible = false;
		}
	}

	protected void drlBranch_DataBinding(object sender, EventArgs e)
	{
		//DropDownList drop = (DropDownList)sender;
		//foreach (ListItem item in drop.Items)
		//{
		//    item.Text = item.Value + " (" + item.Text + ")";
		//}
	}

	protected void drlBranch_Load(object sender)
	{
		//DropDownList drop = (DropDownList)sender;
		//ListItem item = new ListItem(Constants.NotUsed, drop.ID);
		//drop.Items.Clear();
		//drop.Items.Add(item);
	}

	protected void drlTask_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (pnDetail.Visible) btnTest_Click(sender, e);
	}
}
