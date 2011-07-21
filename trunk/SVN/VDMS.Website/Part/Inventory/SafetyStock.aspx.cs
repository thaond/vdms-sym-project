using System;
using System.Web.UI.WebControls;
using VDMS.II.PartManagement.Order;

public partial class Part_Inventory_SafetyStock : BasePage
{

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

	}

	protected void BtnSave_Click(object sender, EventArgs args)
	{
		if (this.Page.IsValid)
		{
			//  move the data back to the data object
			foreach (GridViewRow row in gv1.Rows)
			{
				if ((row.Cells[0].Controls[1] as CheckBox).Checked)
                    switch (this.Target)
                    {
                        case "SI":
                            //SpecialIEDAO.Append(row.Cells[1].Text, SpecialIEType.Import, this.TargetPageKey);
                            break;
                        case "SE":
                            //SpecialIEDAO.Append(row.Cells[1].Text, SpecialIEType.Export, this.TargetPageKey);
                            break;
                        default:
                            //  move the data back to the data object
							//PartOrderDAO.Append(row.Cells[1].Text, row.Cells[2].Text, "", 0, 0, "P");
							PartOrderDAO.Append(row.Cells[1].Text, p =>
							{
								p.PartName = row.Cells[2].Text;
							});
                            break;
                    }
			}

			//  register the script to close the popup
			this.Page.ClientScript.RegisterStartupScript(typeof(Part_Inventory_SafetyStock), "closeThickBox", "self.parent.updated();", true);
		}
	}
}
