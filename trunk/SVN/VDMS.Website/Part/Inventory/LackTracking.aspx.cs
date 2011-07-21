using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.Linq;

public partial class Part_Inventory_LackTracking : BasePage
{
    public static Random rand = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void b_Click(object sender, EventArgs e)
    {
        lv.DataBind();
    }

    protected void cmdSave_Click(object sender, EventArgs e)
    {
		var lv1 = (ListView)((ListViewDataItem)((Button)sender).NamingContainer).FindControl("lv1");
        var db = DCFactory.GetDataContext<PartDataContext>();
        var first = true;
        foreach (var item in lv1.Items)
        {
            var key = (long)lv1.DataKeys[item.DataItemIndex].Value;

            var passed = ((CheckBox)item.FindControl("chkProcess")).Checked;
            var transactionComment = ((TextBox)item.FindControl("t")).Text;

            var d = db.NGFormDetails.Single(p => p.NGFormDetailId == key);
            d.Passed = passed;
            d.TransactionComment = transactionComment;
            //if (first && string.IsNullOrEmpty(d.NGFormHeader.RewardNumber))
            if (first)
            {
                first = false;
                //var count = db.NGFormHeaders.Count(p => p.RewardNumber.Contains("RW-" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString()));
                //d.NGFormHeader.RewardNumber = "RW-" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + "-" + (count + 1).ToString();
                d.NGFormHeader.RewardNumber = "RW-" + d.NGFormHeader.NotGoodNumber.Substring(3);
            }
            db.SubmitChanges();
        }
        lv.DataBind();
        lblSaveOk.Visible = true;
    }

    protected void lnkPrint_DataBinding(object sender, EventArgs e)
    {
        Button lnk = (Button)sender;
        lnk.OnClientClick = string.Format("window.open('{0}&s={1}', 'PrintRW{2}'); return false;", lnk.OnClientClick, ddlS.SelectedValue, rand.Next());
    }
}
