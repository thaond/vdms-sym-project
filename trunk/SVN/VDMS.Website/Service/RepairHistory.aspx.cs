using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using VDMS.I.ObjectDataSource.RepairHistory;
using VDMS.Common.Web;
using VDMS.Helper;

public partial class Service_RepairHistory : BasePage
{
	private string engineNumber = "44xxx11";

	protected void Page_Load(object sender, EventArgs e)
	{
		//if (!IsPostBack)
		{
			// get param: engineNumber
			if (!string.IsNullOrEmpty(Request.QueryString["engnum"]))
			{
				engineNumber = Request.QueryString["engnum"].Trim().ToUpper();
			}
			RepairHistoryDataSource ds = new RepairHistoryDataSource(engineNumber);

			EmptyGridViewEx1.EmptyTableRowText = Message.DataNotFound;
			EmptyGridViewEx1.DataObjectTableSchema = RepairHistoryDataSource.RepairListTableSchema;
			EmptyGridViewEx1.IncludeChildsListInLevel = true;
			//EmptyGridViewEx1._debugGridOnObject = true;
			EmptyGridViewEx1.ColumnsLevelList = new ArrayList[] { new ArrayList(new int[] { 0, 1, 2, 3, 4, 5, 8 }), new ArrayList(new int[] { 6, 7 }), new ArrayList(new int[] { 7 }) };
			EmptyGridViewEx1.DataSourceObject = ds.Select(int.MaxValue, 0);
			EmptyGridViewEx1.DataBind();
		}
	}

	protected string FormatLong(object num, int digits)
	{
		long val;
		NumberFormatInfo ni = Thread.CurrentThread.CurrentCulture.NumberFormat;
		ni.NumberDecimalDigits = digits;
		if (long.TryParse(num.ToString(), out val))
			return val.ToString("N", ni);
		else return "";
	}

	protected void EmptyGridViewEx1_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			// dealer n fee: 
			Literal lit = (Literal)WebTools.FindControlById("litDealer", e.Row);
			if (lit != null)
			{
				if ((UserHelper.IsDealer) && (lit.Text != UserHelper.DealerCode))
				{
					// dai ly khong duoc xem cua dai ly khac
					lit.Text = "";
					lit = (Literal)WebTools.FindControlById("litFee", e.Row); if (lit != null) lit.Text = "";
					lit = (Literal)WebTools.FindControlById("litDealerName", e.Row); if (lit != null) lit.Text = "";
					ImageButton ibtn = (ImageButton)WebTools.FindControlById("imgbDelete", e.Row); if (ibtn != null) ibtn.Visible = false;
				}
			}
			// warrany??
			CheckBox chb = (CheckBox)WebTools.FindControlById("cbbWarr", e.Row);
			if (chb != null)
			{
				if (string.IsNullOrEmpty(chb.Text))
					chb.Visible = false;
				else chb.Checked = Convert.ToBoolean(chb.Text); chb.Text = "";
			}

			// date time
			DateTime dt;
			lit = (Literal)WebTools.FindControlById("litRepairDate", e.Row);
			if (lit != null)
			{
				if (DateTime.TryParse(lit.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dt)) e.Row.Cells[0].Text = dt.ToShortDateString();
			}
		}
	}
	// eval method for visible property of ibtnDelete
	protected bool EvalDeleteVisibility(object sheetId)
	{
		long id;
		long.TryParse(sheetId.ToString(), out id);
		return RepairHistoryDataSource.CanDeleteSRS(id);
	}

	protected void imgbDelete_DataBinding(object sender, EventArgs e)
	{
		ImageButton btn = (ImageButton)sender;
		btn.OnClientClick = string.Format("return confirm('{0}');", Resources.Question.DeleteData);
	}
	protected void imgbDelete_Click(object sender, ImageClickEventArgs e)
	{
		long id;
		ImageButton btn = (ImageButton)sender;
		if (btn != null)
		{
			long.TryParse(btn.CommandArgument, out id);
			RepairHistoryDataSource.DeleteSRS(id);
			Response.Redirect(Request.UrlReferrer.ToString());
		}
	}
	protected void EmptyGridViewEx1_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{

	}
}
