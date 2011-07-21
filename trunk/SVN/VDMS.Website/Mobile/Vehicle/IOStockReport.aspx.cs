using System;
using System.Collections.Generic;
using System.Linq;
using VDMS.Common.Utils;
using VDMS.Common.Web;
using VDMS.Helper;
using VDMS.I.Vehicle;

public partial class MVehicle_Inventory_IOStockReport : BasePage
{
    List<IOStockVehicle> _IOdata;
    List<IOStockVehicle> _ItemType;
    List<IOStockVehicle> _BranchSummary;
    public List<IOStockVehicle> IOData
    {
        get
        {
            if (_IOdata == null) _IOdata = InventoryHelper.GetIOReportData(UserHelper.DealerCode, ddlWh.SelectedValue, txtModel.Text.Trim().ToUpper(), txtType.Text.Trim().ToUpper(), txtFromDate.Text, txtToDate.Text);
            return _IOdata;
        }
    }
    public List<IOStockVehicle> ItemType
    {
        get
        {
            if (_ItemType == null)
            {
                _ItemType = IOData.GroupBy(i => new { ItemType = i.ItemType, BranchCode = i.BranchCode }, (g, result) => new IOStockVehicle
                {
                    BranchCode = g.BranchCode,
                    ItemType = g.ItemType,
                    BeginActs = result.Sum(d => d.BeginActs),
                    BeginInv = new VDMS.I.Entity.SaleInventory() { Quantity = result.Sum(d => d.BeginMonth) },
                    In = result.Sum(d => d.In),
                    Out = result.Sum(d => d.Out),
                    Order = result.Sum(d => d.Order),
                }).ToList();
            }
            return _ItemType;
        }
    }
    public List<IOStockVehicle> BranchSummary
    {
        get
        {
            if (_BranchSummary == null)
            {
                _BranchSummary = ItemType.GroupBy(i => new { BranchCode = i.BranchCode }, (g, result) => new IOStockVehicle
                {
                    BranchCode = g.BranchCode,
                    BeginActs = result.Sum(d => d.BeginActs),
                    BeginInv = new VDMS.I.Entity.SaleInventory() { Quantity = result.Sum(d => d.BeginMonth) },
                    //BeginMonth = result.Sum(d => d.BeginMonth),
                    In = result.Sum(d => d.In),
                    Out = result.Sum(d => d.Out),
                    Order = result.Sum(d => d.Order),
                }).ToList();
            }
            return _BranchSummary;
        }
    }
    //public bool ExportingExcel { get; set; }

    //protected int EvalBorder()
    //{
    //    return ExportingExcel ? 1 : 0;
    //}

    protected object EvalTypes(string wCode)
    {
        return ItemType.Where(i => i.BranchCode == wCode).OrderBy(p => p.ItemCode);
    }
    protected object EvalItems(string itype, string branchCode)
    {
        return IOData.Where(d => d.BranchCode == branchCode && d.ItemType == itype).OrderBy(p => p.ItemCode);
    }
    protected object EvalBranchSum(string wCode, string val)
    {
        //var x = BranchSummary.ToList();
        var obj = BranchSummary.SingleOrDefault(b => b.BranchCode == wCode);
        var obj2 = BranchSummary.SingleOrDefault(b => b.BranchCode == "ONLY_ORDER"); // Những xe mới chỉ có trong order
        int orderOnly = (obj2 == null ? 0 : obj2.Order);
        if (obj != null)
        {
            switch (val)
            {
                case "BG": return obj.Begin;
                case "OR":
                    return obj.Order + orderOnly;
                case "IN": return obj.In;
                case "OT": return obj.Out;
                case "ED": return obj.Balance;
            }
        }

        return null;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFromDate.Text = DataFormat.DateOfFirstDayInMonth(DateTime.Now).ToShortDateString();
            txtToDate.Text = DateTime.Now.ToShortDateString();

            odsWH.SelectParameters["DealerCode"].DefaultValue = UserHelper.DealerCode;
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (_IOdata != null) _IOdata.Clear();
        if (_ItemType != null) _ItemType.Clear();
        _IOdata = _ItemType = null;

        if (string.IsNullOrEmpty(lv.DataSourceID)) lv.DataSourceID = odsWH.ID;
        else lv.DataBind();
    }
    protected void tbnExcel_Click(object sender, EventArgs e)
    {
        //ExportingExcel = true;

        bool paged = odsWH.EnablePaging;
        var p = (VDMS.II.WebControls.DataPager)lv.FindControl("partPager");
        p.DisablePaging = true;
        odsWH.EnablePaging = false;

        //string dl = string.IsNullOrEmpty(ddlWarehouse.SelectedValue) ? "All" : ddlWarehouse.SelectedValue;
        string fileName = string.Format("VIO.{0}.{1}.xls", UserHelper.DealerCode, UserHelper.BranchCode);

        btnQuery_Click(sender, e);
        if (lv.Items.Count > 0)
        {
            GridView2Excel.Export(lv, fileName);
        }

        odsWH.EnablePaging = paged;

        //ExportingExcel = false;
    }
    protected void odsWH_Selecting(object sender, System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs e)
    {

    }
}
