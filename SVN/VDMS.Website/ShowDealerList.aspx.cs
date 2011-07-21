using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.BasicData;

public partial class ShowDealerList : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    List<DealerData> DealerSelected;
    void GetData()
    {
        DealerSelected = HttpContext.Current.Session["DealerSelected"] as List<DealerData>;
        if(DealerSelected == null) DealerSelected = new List<DealerData>();
       foreach (GridViewRow row in gv.Rows)
		{
			var code = row.Cells[1].Text;
            var chb = (row.Cells[0].Controls[1] as CheckBox);
			if (chb.Checked && DealerSelected.SingleOrDefault(p => p.DealerCode == code) == null)
			{	
                DealerSelected.Add(new DealerData
                {
                    DealerCode = code,
                    DealerName = DealerDAO.GetDealerByCode(code).DealerName
                });
			}
			else
			{
                if (!chb.Checked)
                {
                    var dealer = DealerSelected.SingleOrDefault(p => p.DealerCode == code);
                    if (dealer != null) DealerSelected.Remove(dealer);
                }
			}
		}
       HttpContext.Current.Session["DealerSelected"] = DealerSelected;
    }

    protected void gv_PreRender(object sender, EventArgs e)
    {
        DealerSelected = HttpContext.Current.Session["DealerSelected"] as List<DealerData>;
        foreach (GridViewRow row in gv.Rows)
        {   
            var code = row.Cells[1].Text;
            if (DealerSelected != null && DealerSelected.SingleOrDefault(p => p.DealerCode == code) != null) (row.Cells[0].Controls[1] as CheckBox).Checked = true;
        }
    }
    protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //GetData();
    }

    protected void btnFind_Click(object sender, EventArgs e)
    {
        gv.DataBind();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        GetData();
        DealerSelected = HttpContext.Current.Session["DealerSelected"] as List<DealerData>;
        Page.ClientScript.RegisterStartupScript(typeof(ShowDealerList), "closeThickBox", "self.parent.updated();", true);
    }
}
