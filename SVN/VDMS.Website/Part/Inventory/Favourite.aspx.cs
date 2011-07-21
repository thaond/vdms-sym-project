using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.PartManagement.Order;
using VDMS.II.PartManagement.Sales;

public partial class Part_Inventory_Favourite : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
		if (!Page.IsPostBack)
		{
			if (Request.QueryString["target"] == "OD") rblType.Visible = false;
		}
    }

    public string Target
    {
        get { return Request.QueryString["target"]; }
    }

	public string TargetPageKey
    {
        get { return Request.QueryString["tgKey"]; }
    }

    protected void BtnSave_Click(object sender, EventArgs args)
    {
    	if (!this.Page.IsValid) return;
    	foreach (GridViewRow row in gv1.Rows)
    	{
    		if ((row.Cells[1].Controls[1] as CheckBox).Checked)
    		{
    			switch (Target)
    			{
    				case "PS":
						PartSalesDAO.Append(row.Cells[2].Text, p =>
						{
							p.PartName = row.Cells[3].Text;
							p.PartType = rblType.SelectedValue;
						});
						PartSalesDAO.GetInfoId();
						PartSalesDAO.GetPrice();
    					break;
    				default:
						PartOrderDAO.Append(row.Cells[2].Text, p =>
						{
							p.PartName = row.Cells[3].Text;
						});
    					break;
    			}
    		}
    	}

    	//  register the script to close the popup
    	Page.ClientScript.RegisterStartupScript(typeof(Part_Inventory_Favourite), "closeThickBox", "self.parent.updated();", true);
    }

	protected void cmd1_Click(object sender, EventArgs e)
	{
		gv1.DataBind();
	}
}
