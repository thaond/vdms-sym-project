using System;
using System.Web.UI.WebControls;
using VDMS;
using VDMS.Common.Utils;
using VDMS.Helper;
using VDMS.I.Vehicle;
using VDMS.II.Report;
using VDMS.Provider;

public partial class Part_Inventory_MonthlyClose : BasePage
{
	protected string EvalPrevMonth(object clMonth, object clYear)
	{
		if ((clMonth == null) || (clYear == null) || (clYear.ToString() == "0")) return "";
		int month = (int)clMonth, year = (int)clYear;
		DateTime dtCrMonth = DataFormat.DateOfFirstDayInMonth(DateTime.Now);
		DateTime dtClMonth = new DateTime(year, month, 1);
		if (dtClMonth.AddMonths(VDMSSetting.CurrentSetting.MaxMonthAllowReopen1) < dtCrMonth) return "";

		month--;
		if (month == 0)
		{
			month = 12;
			year--;
		}

		return string.Format("{0}/{1}", month, year);
	}

	protected string EvalNextMonth(object cMonth, object cYear)
	{
		if ((cMonth == null) || (cYear == null) || (cYear.ToString() == "0")) return string.Format("{0}/{1}", DateTime.Now.Month, DateTime.Now.Year); ;
		int month = int.Parse(cMonth.ToString()) + 1,
			year = int.Parse(cYear.ToString());
		if (month == 13)
		{
			month = 1;
			year++;
		}

		return string.Format("{0}/{1}", month, year);
	}

	protected int EvalCloseForm(object cYear)
	{
		return ((cYear == null) || (cYear.ToString() == "0")) ? 1 : 0;
	}
	//1
	protected void Page_Load(object sender, EventArgs e)
	{
		InitErrMsgControl(msg);
		InitInfoMsgControl(msg);
		if (!IsPostBack)
		{
			odsWH.SelectParameters["DealerCode"].DefaultValue = UserHelper.DealerCode;
			phResetInv.Visible = UserHelper.IsSysAdmin;// && VDMSProvider.IsDebugEnabled;
		}
	}

	protected void btnSummary_Click(object sender, EventArgs e)
	{

	}
	//1
	protected void gvWh_DataBound(object sender, EventArgs e)
	{
		GridView gv = (GridView)sender;
		if (gv.FooterRow != null)
		{
			gv.FooterRow.CssClass = "group";
			gv.FooterRow.Cells[0].Text = UserHelper.DealerCode;
			gv.FooterRow.Cells[1].Text = UserHelper.DealerName;

			MultiView mv = (MultiView)gv.FooterRow.FindControl("mvCloseForm");
			Literal litLast = (Literal)((GridView)sender).FooterRow.FindControl("litLastClosed");
			LinkButton lnkOpen = (LinkButton)((GridView)sender).FooterRow.FindControl("lnkOpenDealer");
			LinkButton lnkClose = (LinkButton)mv.Views[0].FindControl("lnkCloseDealer");

			DateTime lckDate = InventoryHelper.GetLockedDate(UserHelper.DealerCode, false);
			if (lckDate > DateTime.MinValue)
			{
				lnkClose.Text = EvalNextMonth(lckDate.Month, lckDate.Year);
				lnkOpen.Text = EvalPrevMonth(lckDate.Month, lckDate.Year);
				litLast.Text = string.Format("{0}/{1}", lckDate.Month, lckDate.Year);
				mv.ActiveViewIndex = 0;// InventoryDAO.CanCloseDealer(UserHelper.DealerCode, iv.Year, iv.Month) ? 0 : -1;
			}
			else
			{
				mv.ActiveViewIndex = 1;
			}
		}
	}
	//1
	protected void imbDoFirstDealerClose_OnClick(object sender, EventArgs e)
	{
		TextBox txt = (TextBox)((WebControl)sender).NamingContainer.FindControl("txtCloseMonth");
		try
		{
			int month, year;
			if (int.TryParse(txt.Text.Split('/')[0], out month) && (int.TryParse(txt.Text.Split('/')[1], out year)))
			{
				InventoryHelper.DoCloseD(UserHelper.DealerCode, month, year);
				AddInfoMsg(Resources.Message.ActionSucessful);
			}
			else throw new Exception("Invalid closing month!");
		}
		catch (Exception ex) { AddErrorMsg(ex.Message); }
		gvWh.DataBind();
	}
	//1
	protected void imbDoFirstClose_OnClick(object sender, EventArgs e)
	{
		TextBox txt = (TextBox)((WebControl)sender).NamingContainer.FindControl("txtCloseMonth");
		try
		{
			int month, year;
			if (int.TryParse(txt.Text.Split('/')[0], out month) && (int.TryParse(txt.Text.Split('/')[1], out year)))
			{
				InventoryHelper.DoCloseW(txt.Attributes["WC"], UserHelper.DealerCode, month, year);
				AddInfoMsg(Resources.Message.ActionSucessful);
			}
			else throw new Exception("Invalid closing month!");
		}
		catch (Exception ex) { AddErrorMsg(ex.Message); }
		gvWh.DataBind();
	}
	//1
	protected void lnkOpen_Click(object sender, EventArgs e)
	{
		LinkButton lnk = (LinkButton)sender;
		try
		{
			InventoryHelper.DoOpenW(lnk.Attributes["WC"], UserHelper.DealerCode);
			AddInfoMsg(Resources.Message.ActionSucessful);
		}
		catch (Exception ex) { AddErrorMsg(ex.Message); }
		gvWh.DataBind();
	}
	//1
	protected void lnkOpenDealer_Click(object sender, EventArgs e)
	{
		try
		{
			InventoryHelper.DoOpenD(UserHelper.DealerCode);
			AddInfoMsg(Resources.Message.ActionSucessful);
		}
		catch (Exception ex) { AddErrorMsg(ex.Message); }
		gvWh.DataBind();
	}
	//1
	protected void lnkClose_Click(object sender, EventArgs e)
	{
		LinkButton lnk = (LinkButton)sender;
		try
		{
			InventoryHelper.DoCloseW(lnk.Attributes["WC"], UserHelper.DealerCode);
			AddInfoMsg(Resources.Message.ActionSucessful);
		}
		catch (Exception ex) { AddErrorMsg(ex.Message); }
		gvWh.DataBind();
	}
	//1
	protected void lnkCloseDealer_Click(object sender, EventArgs e)
	{
		try
		{
			InventoryHelper.DoCloseD(UserHelper.DealerCode);
			AddInfoMsg(Resources.Message.ActionSucessful);
		}
		catch (Exception ex)
		{
			//throw;
			AddErrorMsg(ex.Message);
		}
		gvWh.DataBind();
	}

	protected void Button1_Click(object sender, EventArgs e)
	{
		PartMonthlyReport rpt = new PartMonthlyReport(null, "NKP001A", 4, 2009);
		//rpt.CreateExcelReport();
		rpt.DoReport();
	}

	protected void gvWh_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			HyperLink lnk = (HyperLink)e.Row.FindControl("lnkFiles");
			if (lnk != null)
			{
				lnk.Attributes["onclick"] = string.Format("javascript:showSearch(this, '{0}', '{1}')", lnk.Attributes["wCode"], lnk.Attributes["dCode"]);
			}
		}
		if (e.Row.RowType == DataControlRowType.Footer)
		{
			HyperLink lnk = (HyperLink)e.Row.FindControl("lnkFiles");
			if (lnk != null)
			{
				lnk.Attributes["onclick"] = string.Format("javascript:showSearch(this, '', '{0}')", UserHelper.DealerCode);
			}
		}
	}
	protected void btnResetInvent_Click(object sender, EventArgs e)
	{
		try
		{
			InventoryHelper.ResetInventory(UserHelper.DealerCode, DataFormat.DateFromString(txtCloseTo.Text), DateTime.MinValue);
		}
		catch (Exception ex) { AddErrorMsg(ex.Message); }
		gvWh.DataBind();
	}
}
