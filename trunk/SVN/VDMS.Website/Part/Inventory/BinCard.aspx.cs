using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Common.Utils;
using VDMS.Common.Web;
using VDMS.II.Entity;
using VDMS.II.PartManagement;

public partial class Part_Inventory_BinCard : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			txtFromDate.Text = DataFormat.DateOfFirstDayInMonth(DateTime.Now.Date).ToShortDateString();
			txtToDate.Text = DateTime.Now.ToShortDateString();
		}
	}
	protected void Page_PreRender(object sender, EventArgs e)
	{
	}

	protected List<BindCardItem> EvalActions(object items, object begin)
	{
		List<BindCardItem> list;

		if (items != null) list = ((IEnumerable<BindCardItem>)items).ToList();
		else list = new List<BindCardItem>();

		//if (list.Count == 0) list.Add(new BindCardItem());

		if (list.Count > 0)
		{
			int beginQty = 0;
			if (begin != null) beginQty = ((Inventory)begin).Quantity;

			list[0].BeginQuantity = beginQty.ToString();

			for (int i = 0; i < list.Count; i++)
			{
				list[i].ActDateString = DataFormat.ToDateString(list[i].ActDate);

				if (list[i].Quantity < 0)
				{
					list[i].OutAmount = list[i].Amount.ToString();
					list[i].OutQuantity = list[i].Quantity.ToString();
				}
				else if (list[i].Quantity > 0)
				{
					list[i].InAmount = list[i].Amount.ToString();
					list[i].InQuantity = list[i].Quantity.ToString();
				}
				beginQty += list[i].Quantity;
				list[i].Balance = beginQty.ToString();
			}
		}
		return list;
	}

	protected void LoadWarehouse()
	{
		ddlWarehouse.DealerCode = ddlDealer.SelectedValue;
		ddlWarehouse.DataBind();
	}

	protected void btnQuery_Click(object sender, EventArgs e)
	{
		lv.DataSourceID = odsParts.ID;
		lv.DataBind();
	}
	protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
	{

	}
	protected void ddlDealer_SelectedIndexChanged(object sender, EventArgs e)
	{
		LoadWarehouse();
	}
	protected void ddlDealer_DataBound(object sender, EventArgs e)
	{
		LoadWarehouse();
	}
	protected void cmd2Excel_Click(object sender, EventArgs e)
	{
		string dtFrom = DataFormat.DateFromString(txtFromDate.Text).ToString("yyyy_MM_dd");
		string dtTo = DataFormat.DateFromString(txtToDate.Text).ToString("yyyy_MM_dd");
		string wh = string.IsNullOrEmpty(ddlWarehouse.SelectedValue) ? "All" : ddlWarehouse.SelectedValue;
		string fileName = string.Format("BinCard.{0}.{1}.{2}-{3}.[{4}].xls", ddlDealer.SelectedValue, wh, dtFrom, dtTo, txtPartCode.Text);
		long? wid = string.IsNullOrEmpty(ddlWarehouse.SelectedValue) ? null : (long?)long.Parse(ddlWarehouse.SelectedValue);

		ListView lvExcel = (ListView)Page.LoadControl("~/Controls/ExcelTemplate/BinCard.ascx").Controls[0];
		BinCardDAO dao = new BinCardDAO();
		lvExcel.DataSource = dao.FindBindCardPart(rblType.SelectedValue, txtPartCode.Text.Trim(), txtFromDate.Text, txtToDate.Text, ddlDealer.SelectedValue, wid, -1, -1);
		if (dao.CountBindCardPart("", "", "", "", "", null) > 0)
		{
			lvExcel.DataBind();
			GridView2Excel.Export(lvExcel, fileName);
		}
	}
}
