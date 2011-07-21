using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using VDMS.Common.Utils;
using VDMS.Common.Web;
using VDMS.Core.Domain;
using VDMS.Helper;
using VDMS.I.Service;

/////////////////////////////////////////////////////////////////////////////////////////////////
// luu y khi them/bot column trong grid => dieu chinh lai "text columns" list khi export excel //
/////////////////////////////////////////////////////////////////////////////////////////////////

public partial class Sales_Report_Default4 : BasePage
{
	// flags here
	private bool exporttingToExcel = false;

	protected void Page_Load(object sender, EventArgs e)
	{
		exporttingToExcel = false;
		//bllMessage.Items.Clear();
		if (!IsPostBack)
		{
			txtFromDate.Text = DateTimeHelper.FirstDayInMonth.ToShortDateString();
			txtToDate.Text = DateTime.Now.ToShortDateString();
			//BindData();
		}
	}

	private void BindData()
	{
		//SaveParamToUI();

		if (string.IsNullOrEmpty(gvMain.DataSourceID))
			gvMain.DataSourceID = ExchangePartDetailDataSource1.ID;
		else
			gvMain.DataBind();

	}

	#region Eval Method
	//protected string EvalDate(object objDate)
	//{
	//    string res = string.Empty;
	//    try
	//    {
	//        DateTime dtDate = (DateTime)objDate;
	//        res = dtDate.ToShortDateString();
	//    }
	//    catch { }
	//    return res;
	//}
	protected string EvalDealerName(object oAreaCode)
	{
		string res = string.Empty;
		try
		{
			res = oAreaCode.ToString();
			res += " (" + DealerHelper.GetName(oAreaCode.ToString()) + ")";
		}
		catch { }
		return res;
	}
	//EvalGender(Eval("Exchangepartheader.Customer.Gender"))
	protected string EvalGender(object oGender)
	{
		string res = string.Empty;
		try
		{
			if ((bool)oGender)
			{
				res = Resources.Gender.Male;
			}
			else
			{
				res = Resources.Gender.Female;
			}
		}
		catch { }
		return res;
	}
	//EvalAreaCode(Eval("Exchangepartheader.Areacode"))
	protected string EvalAreaCode(object oAreaCode)
	{
		string res = string.Empty;
		try
		{
			res = AreaHelper.GetAreaNameByAreaCode(oAreaCode.ToString());
		}
		catch { }
		return res;
	}
	// EvalExchangeStatus(Eval("Exchangepartheader.Status"))
	protected string EvalExchangeStatus(object status)
	{
		string res = string.Empty;
		try
		{
			res = ServiceTools.GetNativeExchangeStatusName((int)status);
		}
		catch { }
		return res;
	}
	#endregion

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		//if (CheckValidate())
		//{
		//    LoadParamFromUI();
		//    BindData();
		//}
		BindData();
	}

	protected void gvMain_DataBound(object sender, EventArgs e)
	{
		try
		{
			Literal litPageInfo = gvMain.TopPagerRow.FindControl("litPageInfo") as Literal;
			if (litPageInfo != null) litPageInfo.Text = string.Format(Resources.Message.PagingInfo, gvMain.PageIndex + 1, gvMain.PageCount, HttpContext.Current.Items["rowCount"]);
		}
		catch
		{
		}
	}
	protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			e.Row.Cells[0].Text = (gvMain.PageSize * gvMain.PageIndex + e.Row.RowIndex + 1).ToString();

			Literal lit;
			Exchangepartdetail epd = (Exchangepartdetail)e.Row.DataItem;
			Warrantycondition wrc = WarrantyContent.GetWarrantyCondition(epd.Partcodem);
			NumberFormatInfo ni = Thread.CurrentThread.CurrentCulture.NumberFormat;
			ni.NumberDecimalDigits = 2;

			if (wrc != null)
			{
				lit = e.Row.FindControl("litPartName") as Literal;
				if (lit != null)
				{
					lit.Text = ServiceTools.GetPartName(wrc);
				}
				lit = e.Row.FindControl("litFrt") as Literal;
				if (lit != null)
				{
					lit.Text = NumberFormatHelper.StrDoubleToStr(wrc.Manpower, "en-US", Thread.CurrentThread.CurrentCulture.Name);
				}
				lit = e.Row.FindControl("litManM") as Literal;
				if (lit != null)
				{
					lit.Text = (wrc.Labour == 0) ? "" : ((double)(epd.Totalfeem / wrc.Labour)).ToString(ni);
				}

				if (!exporttingToExcel)
				{
					lit = e.Row.FindControl("litLabourCost") as Literal;
					if (lit != null)
					{
						long fee;
						long.TryParse(lit.Text, out fee);
						lit.Text = NumberFormatHelper.NumberToCurentMoneyFormatString(fee);
					}
				}
			}

			if (epd != null)
			{
				lit = e.Row.FindControl("litDamageDate") as Literal;
				if (lit != null)
				{
					lit.Text = epd.Exchangepartheader.Damageddate.ToShortDateString();
				}
				lit = e.Row.FindControl("litPartCost") as Literal;
				if (lit != null)
				{
					long partCost = (long)epd.Partqtym * epd.Unitpricem;
					lit.Text = (exporttingToExcel) ? partCost.ToString() : NumberFormatHelper.NumberToCurentMoneyFormatString(partCost);
				}
				lit = e.Row.FindControl("litAdrs") as Literal;
				if ((lit != null) && (epd.Exchangepartheader.Customer != null))
				{
					lit.Text = ServiceTools.GetCustAddress(epd.Exchangepartheader.Customer);
				}
				lit = e.Row.FindControl("litTotal") as Literal;
				if (lit != null)
				{
					decimal Total = (decimal)epd.Partqtym * (decimal)epd.Unitpricem + epd.Totalfeem;
					lit.Text = (exporttingToExcel) ? Total.ToString() : NumberFormatHelper.NumberToCurentMoneyFormatString(Total);
				}
			}

			if (!exporttingToExcel)
			{
				//litBrokenNo
			}
		}
	}

	protected void btnExport2Exel_Click(object sender, EventArgs e)
	{
		if (gvMain.Rows.Count > 0)
		{
			//exporttingToExcel = true; // no needed from now :)

			// truoc khi save thi khong paging
			gvMain.PageIndex = 0;
			gvMain.AllowPaging = false;
			gvMain.GridLines = GridLines.Both;
			BindData();

			GridView2Excel.Export(gvMain,
					this.Page,
					string.Format("SVrpt.{0}-{1}.xls", txtFromDate.Text.Replace("/", string.Empty), txtToDate.Text.Replace("/", string.Empty)),
					true, // treat some column as text
					new int[] { 8, 16 });

			// sau khi save Bind voi Paging
			gvMain.AllowPaging = true;
			BindData();
		}
	}
}
