using System;
using System.Web;
using System.Web.UI.WebControls;
using VDMS.Core.Domain;
using VDMS.Helper;
using VDMS.I.Vehicle;
using VDMS.Common.Web;

public partial class Sales_Inventory_Detail : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			if (!string.IsNullOrEmpty(UserHelper.BranchCode))//&& !UserHelper.IsAdmin)
			{
				ddlStorePlace.SelectedValue = UserHelper.BranchCode;
			}
			ddlStorePlace.Enabled = UserHelper.IsAdmin;
		}
	}
	protected void btnSearch_Click(object sender, EventArgs e)
	{
		BindData();
	}

	private void BindData()
	{
		if (string.IsNullOrEmpty(gvMain.DataSourceID)) gvMain.DataSourceID = IteminstanceDataSource1.ID;
		else gvMain.DataBind();
	}
	protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			e.Row.Cells[0].Text = (gvMain.PageSize * gvMain.PageIndex + e.Row.RowIndex + 1).ToString();

			//CheckBox ckbVoucher = e.Row.FindControl("ckbVoucher") as CheckBox;
			//if (ckbVoucher != null)
			//{
			//    //long Id = long.Parse(gvMain.DataKeys[e.Row.RowIndex][0].ToString());
			//    Iteminstance ii = e.Row.DataItem as Iteminstance;
			//    long Id = ii.Id;
			//    Shippingdetail sh = ItemInstanceHelper.GetShippingdetailByItemInstance(Id);
			//    if (sh != null)
			//    {
			//        ckbVoucher.Checked = sh.Voucherstatus;
			//    }
			//}
		}
	}
	protected void gvMain_DataBound(object sender, EventArgs e)
	{
		try
		{
			Literal litPageInfo = gvMain.TopPagerRow.FindControl("litPageInfo") as Literal;
			if (litPageInfo != null) litPageInfo.Text = string.Format(Resources.Message.PagingInfo, gvMain.PageIndex + 1, gvMain.PageCount, HttpContext.Current.Items["rowCount"]);
		}
		catch { }
	}
	protected void ddlStorePlace_DataBound(object sender, EventArgs e)
	{
		ListItem Item = new ListItem(Resources.Constants.All, "0");
		ddlStorePlace.Items.Add(Item);
	}

	#region Eval Method()
	//EvalBranchcode(Eval("Branchcode"))
	//protected string EvalDate(object oDate)
	//{
	//    string res = string.Empty;
	//    try
	//    {
	//        DateTime dRes = (DateTime)oDate;
	//        if (dRes != DateTime.MinValue)
	//        {
	//            res = dRes.ToShortDateString();
	//        }
	//    }
	//    catch { }
	//    return res;
	//}
	protected string EvalBranchcode(object oBranchcode)
	{
		if (oBranchcode == null) return "";
		string res = string.Empty;
		try
		{
			//string b = UserHelper.ExtractBranch((string)oBranchcode);
			string b = (string)oBranchcode;
			res = VDMS.II.BasicData.WarehouseDAO.GetWarehouse(b, UserHelper.DealerCode, VDMS.II.Entity.WarehouseType.Vehicle).Address;
		}
		catch { }
		return res;
	}
	//EvalTransDate(Eval("Id"))
	protected string EvalTransDate(object oId)
	{
		string res = string.Empty;
		try
		{
			TransHis th = ItemInstanceHelper.GetTransHisByTransactiontypeAndItemInstance(long.Parse(oId.ToString()), (int)ItemStatus.Moved);
			if (th != null)
			{
				res = th.Transactiondate.ToShortDateString();
			}
		}
		catch { }
		return res;
	}
	#endregion
    protected void Button1_Click(object sender, EventArgs e)
    {
        gvMain.AllowPaging = false;
        BindData();
        GridView2Excel.Export(gvMain, "qwe.xls");
        gvMain.AllowPaging = true;
    }
}
