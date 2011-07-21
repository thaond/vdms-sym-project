using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Provider;
using VDMS.II.PartManagement;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.BasicData;
using VDMS.II.Report;
using VDMS.Common.Web;
using VDMS.Common.Utils;

public partial class MPartReport_inventory : BasePage
{
    void BindPart()
    {
        if (string.IsNullOrEmpty(gvPart.DataSourceID)) gvPart.DataSourceID = odsPartList.ID;
        gvPart.DataBind();
    }
    void BindParts()
    {
        if (string.IsNullOrEmpty(gvParts.DataSourceID)) gvParts.DataSourceID = odsPartsList.ID;
        gvParts.DataBind();
    }

    #region Eval methods

    //private int lastRowsCount = 1;
    //private string lastDealerCode = string.Empty;
    //public int GetSafetyRows(object dealerCode)
    //{
    //    //if (!lastDealerCode.Equals(dealerCode))
    //    //{
    //    //    int rows = PartInfoDAO.SearchPartSafetiesCount(txtPartNo.Text.Trim(), dealerCode.ToString());
    //    //    lastRowsCount = (rows == 0) ? 1 : rows;
    //    //}
    //    return lastRowsCount;
    //}

    ////DataSource='<%# GetPartSafetyList(Eval("DealerCode")) %>'>
    //public object GetPartSafetyList(object dealerCode)
    //{
    //    object res = new InventoryReportDAO().SearchPartInstock(txtPartNo.Text.Trim(), dealerCode.ToString(), rblType.SelectedValue, int.MaxValue, 0);
    //    return res;
    //}

    //public string EvalSource(string dealerCode)
    //{
    //    //Dealer dealer = DealerDAO.GetDealerByCode(dealerCode.ToString());
    //    //lvItems.DataSourceID = odsPartList.ID;
    //    //lvItems.DataBind();
    //    return dealerCode;
    //}

    #endregion

    new protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
        ddlDealers.RootDealer = ddlDealers2.RootDealer = UserHelper.DealerCode;
        ddlDealers.RemoveRootItem = ddlDealers2.RemoveRootItem = UserHelper.DealerCode == "/";
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnViewPartFromDealers_Click(object sender, EventArgs e)
    {
        BindPart();
    }
    protected void btnViewPartsInDealer_Click(object sender, EventArgs e)
    {
        BindParts();
    }

    protected void ddlDealers_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList drop = (DropDownList)sender;
        if (drop.ID == ddlDealers.ID)
        {
            ddlWarehouse.DealerCode = drop.SelectedValue;
            ddlWarehouse.DataBind();
            //TabContainer1.ActiveTabIndex = tabPart.TabIndex;
        }
        else
        {
            ddlWarehouses2.DealerCode = drop.SelectedValue;
            ddlWarehouses2.DataBind();
            TabContainer1.ActiveTabIndex = 1;
        }
    }

    protected void btnExcelViewPartsInDealer_Click(object sender, EventArgs e)
    {
        bool paged = gvParts.AllowPaging;
        GridLines gl = gvParts.GridLines;
        gvParts.AllowPaging = false;
        gvParts.GridLines = GridLines.Both;
        odsPartsList.EnablePaging = false;

        string dl = string.IsNullOrEmpty(ddlWarehouses2.SelectedValue) ? "All" : ddlWarehouses2.SelectedValue;
        string fileName = string.Format("inv2.{0}.{1}.{2}.xls", ddlDealers2.SelectedValue, dl, DateTime.Now.ToShortDateString());

        BindParts();
        if (gvParts.Rows.Count > 0)
        {
            GridView2Excel.Export(gvParts, fileName);
        }

        gvParts.GridLines = gl;
        gvParts.AllowPaging = paged;
        odsPartsList.EnablePaging = paged;
    }
    protected void btnExcelViewPartFromDealers_Click(object sender, EventArgs e)
    {
        bool paged = gvPart.AllowPaging;
        GridLines gl = gvPart.GridLines;
        gvPart.AllowPaging = false;
        gvPart.GridLines = GridLines.Both;
        odsPartList.EnablePaging = false;

        //string dl = string.IsNullOrEmpty(ddlWarehouse.SelectedValue) ? "All" : ddlWarehouse.SelectedValue;
        string fileName = string.Format("inv.{0}.{1}.xls", ddlDealers.SelectedValue, DateTime.Now.ToShortDateString());

        BindPart();
        if (gvPart.Rows.Count > 0)
        {
            GridView2Excel.Export(gvPart, fileName);
        }

        gvPart.GridLines = gl;
        gvPart.AllowPaging = paged;
        odsPartList.EnablePaging = paged;
    }

    protected void gvParts_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        TabContainer1.ActiveTabIndex = 1;
    }
    protected void gvPart_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        TabContainer1.ActiveTabIndex = 0;
    }
}
