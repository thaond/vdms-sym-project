using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.PartManagement.Sales;
using VDMS.Helper;
using VDMS.Common.Utils;

public partial class Part_Report_saleReport : BasePage
{
    //public string QDealer { get { return Request.QueryString["d"]; } }
    void LoadDealers()
    {
        ddlDealers.DatabaseCode = ddlRegion.SelectedValue;
        ddlDealers.DataBind();
    }
    void BindData()
    {
        if (string.IsNullOrEmpty(lvParts.DataSourceID)) lvParts.DataSourceID = odsPartCurrent.ID;
        else lvParts.DataBind();
    }

    #region Eval methods

    public object GetPartSaleList(object dealerCode)
    {
        object res = new PartSalesDAO().GetPartSaleDetailReport(dealerCode.ToString(), rblType.SelectedValue, txtFromDate.Text, txtToDate.Text);
        return res;
    }

    #endregion

    new protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
        ddlDealers.RootDealer = UserHelper.DealerCode;
        ddlDealers.RemoveRootItem = UserHelper.DealerCode == "/";
        //if (!string.IsNullOrEmpty(QDealer)) ddlDealers.SelectedValue = QDealer;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDealers();
            txtFromDate.Text = DataFormat.DateOfFirstDayInMonth(DateTime.Now).ToShortDateString();
            txtToDate.Text = DateTime.Now.ToShortDateString();
        }
    }


    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void ddlDealers_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Response.Redirect(string.Format("~/Part/Report/SaleReport.aspx?d={0}", ddlDealers.SelectedValue));
    }
    protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDealers();
    }
}
