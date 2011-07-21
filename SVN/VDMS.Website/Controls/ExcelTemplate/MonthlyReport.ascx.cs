using System;

public partial class Controls_MonthlyReportTemplate : System.Web.UI.UserControl
{
    //public string WarehouseId { get; set; }
    //public string DealerCode { get; set; }
    //public int Month { get; set; }
    //public int Year { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        //WarehouseId = UserHelper.WarehouseId > 0 ? UserHelper.WarehouseId.ToString() : null;
        //DealerCode = UserHelper.DealerCode;
    }

    public void BindData()
    {
        //InventoryDAO.CreateExcelReport(lvExcel, WarehouseId, DealerCode, Month, Year);
        //lvExcel.DataBind();
    }
}
