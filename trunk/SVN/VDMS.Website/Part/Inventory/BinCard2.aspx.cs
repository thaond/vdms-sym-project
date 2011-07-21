using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Common.Utils;
using VDMS.Common.Web;
using VDMS.II.Entity;
using VDMS.II.PartManagement;
using VDMS.II.BasicData;

public partial class Part_Inventory_BinCard2 : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFromDate.Text = DataFormat.DateOfFirstDayInMonth(DateTime.Now.Date).ToShortDateString();
            txtToDate.Text = DateTime.Now.ToShortDateString();
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
    }

    protected List<BindCardItem> EvalAct(object wid)
    {
        return EvalActions(new BinCardDAO().FindBindcardAct(rblType.SelectedValue, txtPartCode.Text.Trim(), txtFromDate.Text, txtToDate.Text, ddlDealer.SelectedValue, (long)wid, -1, -1));
    }
    public int GetBeginQty(long pid, long wid, DateTime date)
    {
        int beginQty = 0;
        Inventory invt = InventoryDAO.GetPrevInventory(pid, wid, date.Year, date.Month);
        if (invt != null) beginQty = invt.Quantity;
        beginQty += TransactionDAO.Summarization(pid, DataFormat.DateOfFirstDayInMonth(date), date);
        return beginQty;
    }
    protected List<BindCardItem> EvalActions(IQueryable<BindCardItem> items)
    {
        DateTime fromdate = DataFormat.DateFromString(txtFromDate.Text);
        List<BindCardItem> list;

        if (items != null) list = items.ToList();
        else list = new List<BindCardItem>();

        if (list.Count > 0)
        {
            int beginQty = 0;
            int no = 0;
            string curPart = "";


            for (int i = 0; i < list.Count; i++)
            {
                if (curPart != list[i].PartCode)
                {
                    curPart = list[i].PartCode;
                    beginQty = GetBeginQty(list[i].PartInfoId, list[i].WarehouseId, fromdate);
                    no = 0;
                }

                list[i].No = ++no;
                list[i].BeginQuantity = beginQty.ToString();
                list[i].ActDateString = DataFormat.ToDateString(list[i].ActDate);

                if (list[i].Quantity < 0)
                {
                    list[i].OutAmount = list[i].Amount.ToString();
                    list[i].OutQuantity = list[i].Quantity.ToString();
                }
                else if (list[i].Quantity > 0)
                {
                    list[i].InAmount = list[i].Amount.ToString();
                    list[i].InQuantity = list[i].Quantity.ToString();
                }
                beginQty += list[i].Quantity;
                list[i].Balance = beginQty.ToString();
            }
        }
        return list;
    }

    protected void LoadWarehouse()
    {
        ddlWarehouse.DealerCode = ddlDealer.SelectedValue;
        ddlWarehouse.DataBind();
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        lv.DataSourceID = odsParts.ID;
        lv.DataBind();
    }
    protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlDealer_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadWarehouse();
    }
    protected void ddlDealer_DataBound(object sender, EventArgs e)
    {
        LoadWarehouse();
    }
    protected void cmd2Excel_Click(object sender, EventArgs e)
    {
        string dtFrom = DataFormat.DateFromString(txtFromDate.Text).ToString("yyyy_MM_dd");
        string dtTo = DataFormat.DateFromString(txtToDate.Text).ToString("yyyy_MM_dd");
        string wh = string.IsNullOrEmpty(ddlWarehouse.SelectedValue) ? "All" : ddlWarehouse.SelectedValue;
        string fileName = string.Format("BinCard.{0}.{1}.{2}-{3}.[{4}].xls", ddlDealer.SelectedValue, wh, dtFrom, dtTo, txtPartCode.Text);
        long wid = string.IsNullOrEmpty(ddlWarehouse.SelectedValue) ? 0 : long.Parse(ddlWarehouse.SelectedValue);

        ListView lvExcel = (ListView)Page.LoadControl("~/Controls/ExcelTemplate/BinCard2.ascx").Controls[0];
        lvExcel.Attributes["txtFromDate"] = txtFromDate.Text;
        lvExcel.Attributes["rblType"] = rblType.Text;
        lvExcel.Attributes["txtToDate"] = txtToDate.Text;
        lvExcel.Attributes["txtPartCode"] = txtPartCode.Text.Trim();
        lvExcel.Attributes["ddlDealer"] = ddlDealer.SelectedValue;

        BinCardDAO dao = new BinCardDAO();
        lvExcel.DataSource = dao.FindBindCardWH(ddlDealer.SelectedValue, wid, -1, -1);
        if (dao.CountBindCardWH("", 0) > 0)
        {
            lvExcel.DataBind();
            GridView2Excel.Export(lvExcel, fileName);
        }
    }
    protected void odsParts_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //e.
    }
}
