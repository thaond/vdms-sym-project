using System;
using System.Collections.Generic;
using System.Linq;
using VDMS.Common.Utils;
using VDMS.II.Entity;
using VDMS.II.PartManagement;

public partial class Controls_ExcelTemplate_BinCard2 : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
     
    protected List<BindCardItem> EvalAct(object wid)
    {
        return EvalActions(new BinCardDAO().FindBindcardAct(lvExcel.Attributes["rblType"], lvExcel.Attributes["txtPartCode"], lvExcel.Attributes["txtFromDate"], lvExcel.Attributes["txtToDate"], lvExcel.Attributes["ddlDealer"], (long)wid, -1, -1));
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
        DateTime fromdate = DataFormat.DateFromString(lvExcel.Attributes["txtFromDate"]);
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

}
