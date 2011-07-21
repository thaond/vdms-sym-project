using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;
using VDMS.II.Entity;
using VDMS.II.PartManagement;
using VDMS.II.PartManagement.Order;
using VDMS.II.PartManagement.Sales;
using VDMS.Helper;

public partial class Part_Inventory_SearchPart : BasePage
{
	public static string PartType = VDMS.II.Entity.PartType.Part;
	public static string FavQueryType = "O";

	int PartCodeIndex = 2;

	public string Target
	{
		get { return Request.QueryString["target"]; }
	}
	public string TargetPageKey
	{
		get { return Request.QueryString["tgKey"]; }
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (Request.QueryString["acc"] == "N") phA.Visible = false;
		PartType = rblType.SelectedValue;
		((BoundField)gv1.Columns[3]).DataField = UserHelper.IsVietnamLanguage ? "VietnamName" : "EnglishName";
		switch (this.Target)
		{
			case InventoryAction.SpecialImport:
			case InventoryAction.SpecialExport:
            case InventoryAction.SpecialNotGood:
            case InventoryAction.CycleCount:
				gv1.Columns[0].Visible = false;
				break;
			case InventoryAction.StockTransfer:
			case InventoryAction.Sales:
				FavQueryType = "S";
				gv1.Columns[4].Visible = false;
				break;
			default:
				FavQueryType = "O";
				gv1.Columns[5].Visible = false;
				break;
		}

		if (!Page.IsPostBack)
		{
			tb1.Text = Request.QueryString["code"];
			tb2.Text = Request.QueryString["name"];
			tb3.Text = Request.QueryString["engno"];
            ddl3.DataBind();
			ddl3.SelectedValue = Request.QueryString["model"];
		}
	}

	protected void BtnSave_Click(object sender, EventArgs args)
	{
		if (!Page.IsValid) return;
		//  move the data back to the data object
		GetData();
		PartSelected = (List<PartData>)LoadState("PartSelected" + rblType.SelectedValue);
		PartSelected.ForEach(p =>
			{
				switch (this.Target)
				{
					case InventoryAction.StockTransfer:
						PartTransferDAO.Append(p.PartCode, PartType, this.TargetPageKey, p.Stock, p.PartInfoId);
						break;
					case InventoryAction.SpecialImport:
						SpecialIEDAO.Append(p.PartCode, SpecialIEType.Import, PartType, this.TargetPageKey);
						break;
					case InventoryAction.SpecialExport:
						SpecialIEDAO.Append(p.PartCode, SpecialIEType.Export, PartType, this.TargetPageKey);
						break;
					case InventoryAction.Sales:
						PartSalesDAO.Append(p.PartCode, q =>
						{
							q.PartName = p.PartName;
							q.Stock = p.Stock;
							q.PartInfoId = p.PartInfoId;
							q.PartType = rblType.SelectedValue;
                            q.AvailableStock = p.AvailableStock;
						});
						PartSalesDAO.GetPrice();
						break;
                    case InventoryAction.SpecialNotGood:
                        NotGoodManualDAO.Append(p.PartCode, q =>
                        {
                            q.PartName = p.PartName;
                        });
                        break;
                    case InventoryAction.CycleCount:
                        CycleCountDAO.Append(p.PartCode, q =>
                        {
                            q.PartName = p.PartName;
                            q.Quantity = q.CycleQuantity = p.Stock;
                            q.PartType = rblType.SelectedValue;
                        });
                        break;
					default:
						PartOrderDAO.Append(p.PartCode, q =>
						{
							q.PartName = p.PartName;
						});
						break;
				}
			});
		//foreach (GridViewRow row in gv1.Rows)
		//{
		//    if ((row.Cells[PartCodeIndex - 1].Controls[1] as CheckBox).Checked)
		//    {
		//        string partInfoId = ((HiddenField)row.FindControl("hdPartInfoId")).Value;

		//        switch (this.Target)
		//        {
		//            case InventoryAction.StockTransfer:
		//                PartTransferDAO.Append(row.Cells[PartCodeIndex].Text, PartType, this.TargetPageKey, row.Cells[PartCodeIndex + 3].Text, partInfoId);
		//                break;
		//            case InventoryAction.SpecialImport:
		//                SpecialIEDAO.Append(row.Cells[PartCodeIndex].Text, SpecialIEType.Import, PartType, this.TargetPageKey);
		//                break;
		//            case InventoryAction.SpecialExport:
		//                SpecialIEDAO.Append(row.Cells[PartCodeIndex].Text, SpecialIEType.Export, PartType, this.TargetPageKey);
		//                break;
		//            case InventoryAction.Sales:
		//                PartSalesDAO.Append(row.Cells[PartCodeIndex].Text, p =>
		//                {
		//                    p.PartName = row.Cells[PartCodeIndex + 1].Text;
		//                    p.Stock = int.Parse(row.Cells[PartCodeIndex + 3].Text);
		//                    p.PartInfoId = (long)gv1.DataKeys[row.RowIndex].Value;
		//                    p.PartType = rblType.SelectedValue;
		//                });
		//                PartSalesDAO.GetPrice();
		//                break;
		//            case InventoryAction.SpecialNotGood:
		//                NotGoodManualDAO.Append(row.Cells[PartCodeIndex].Text, p =>
		//                {
		//                    p.PartName = row.Cells[PartCodeIndex + 1].Text;
		//                });
		//                break;
		//            default:
		//                //  move the data back to the data object
		//                PartOrderDAO.Append(row.Cells[PartCodeIndex].Text, p =>
		//                {
		//                    p.PartName = row.Cells[PartCodeIndex + 1].Text;
		//                });
		//                break;
		//        }
		//    }
		//}

		//  register the script to close the popup
		Page.ClientScript.RegisterStartupScript(typeof(Part_Inventory_SearchPart), "closeThickBox", "self.parent.updated();", true);
	}

	[WebMethod]
	public static string MarkPart(string PartNo, bool Marked)
	{
		if (Marked) PartDAO.InsertFavorite(PartNo, 5, FavQueryType, PartType);
		else PartDAO.DeleteFavorite(FavQueryType, PartNo);
		return "#" + PartNo;
	}

	protected void cmdFilter_Click(object sender, EventArgs e)
	{
		gv1.PageIndex = 0;
	}

	protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
	{
		phForPart.Visible = rblType.SelectedValue == VDMS.II.Entity.PartType.Part;
	}

	protected void ddl3_DataBound(object sender, EventArgs e)
	{
		foreach (ListItem item in ddl3.Items)
		{
			if (!string.IsNullOrEmpty(item.Text))
			{
				item.Text = string.Format("{0}   ({1})", item.Value, item.Text);
			}
		}
        //if (!Page.IsPostBack)
        //{
        //    ddl3.SelectedValue = Request.QueryString["model"];
        //}
	}

	#region Keep selected item when page index change

	[Serializable]
	class PartData
	{
		public string PartCode { get; set; }
        public string PartName { get; set; }
		public long PartInfoId { get; set; }
		public int Stock { get; set; }
        public int AvailableStock { get; set; }
	}
	List<PartData> PartSelected;
	void GetData()
	{
		PartSelected = (List<PartData>)LoadState("PartSelected" + rblType.SelectedValue);
		if (PartSelected == null) PartSelected = new List<PartData>();

		foreach (GridViewRow row in gv1.Rows)
		{
			var code = row.Cells[PartCodeIndex].Text;
            var chb = (row.Cells[PartCodeIndex - 1].Controls[1] as CheckBox);
			if (chb.Checked && PartSelected.SingleOrDefault(p => p.PartCode == code) == null)
			{
				int stock = 0, availStock = 0;
				int.TryParse(row.Cells[PartCodeIndex + 3].Text, out stock);
                int.TryParse(row.Cells[PartCodeIndex + 4].Text, out availStock);
				PartSelected.Add(new PartData
				{
					PartCode = code,
					PartInfoId = long.Parse(gv1.DataKeys[row.RowIndex].Value.ToString()),
					Stock = stock,
                    AvailableStock = availStock,
					PartName = row.Cells[PartCodeIndex + 1].Text,
				});
			}
			else
			{
				var part = PartSelected.SingleOrDefault(p => p.PartCode == code);
				if (part != null) PartSelected.Remove(part);
			}
		}

		SaveState("PartSelected" + rblType.SelectedValue, PartSelected);
	}

	protected void gv1_PageIndexChanging(object sender, GridViewPageEventArgs e)
	{
		GetData();
	}

	protected void gv1_PreRender(object sender, EventArgs e)
	{
		foreach (GridViewRow row in gv1.Rows)
		{
			var code = row.Cells[PartCodeIndex].Text;
			if (PartSelected != null && PartSelected.SingleOrDefault(p => p.PartCode == code) != null) (row.Cells[PartCodeIndex - 1].Controls[1] as CheckBox).Checked = true;
		}
	}
	#endregion
}
