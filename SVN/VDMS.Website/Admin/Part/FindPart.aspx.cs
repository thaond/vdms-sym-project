using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using VDMS.II.PartManagement;

public partial class Part_Inventory_FindPart : BasePage
{
	protected bool EvalExist(object partCode)
	{
		if (!UserHelper.IsDealer) return false;
		return PartInfoDAO.PartExist(partCode.ToString(), UserHelper.DealerCode);
	}

	protected void Refresh_Click(object sender, EventArgs args)
	{
		//  update the grids contents
		this.gvParts.DataSourceID = odsParts.ID;// = VDMS.Data.TipTop.Part.GetPartList("", "", "", "", DateTime.MinValue, DateTime.MaxValue, gvParts.PageIndex * gvParts.PageSize, gvParts.PageSize);
		this.gvParts.DataBind();
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			txtFromDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
			//gv1.DataSource = Fetch();
			//gv1.DataBind();
		}
	}

	protected void btnFind_Click(object sender, EventArgs e)
	{
		Refresh_Click(sender, e);
	}

	protected void btnAdd_Click(object sender, EventArgs e)
	{
		foreach (GridViewRow row in gvParts.Rows)
		{
			PartInfoDAO.AddPartToDealer(null, UserHelper.DealerCode, row.Cells[1].Text, "P", null);
		}
	}

	protected void btnAddAcc_Click(object sender, EventArgs e)
	{
		AccessoryTypeDAO.Init();
		foreach (GridViewRow row in gvParts.Rows)
		{
			if (row.RowIndex % 2 == 0)
			{
				Category cat = CategoryDAO.GetOne();
				if (cat != null)
					AccessoryDAO.CreateNew(row.Cells[1].Text, row.Cells[2].Text, null, UserHelper.DealerCode, "SY", cat.CategoryId);
			}
			else
			{
				AccessoryDAO.CreateNew(row.Cells[1].Text, row.Cells[2].Text, null, UserHelper.DealerCode, "OT", CategoryDAO.GetOne().CategoryId);
			}
		}
	}
}
