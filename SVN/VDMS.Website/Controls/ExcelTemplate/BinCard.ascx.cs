using System;
using System.Collections.Generic;
using System.Linq;
using VDMS.Common.Utils;
using VDMS.II.Entity;
using VDMS.II.PartManagement;

public partial class Controls_ExcelTemplate_BinCard : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected List<BindCardItem> EvalActions(object items, object begin)
    {
        List<BindCardItem> list;

        if (items != null) list = ((IEnumerable<BindCardItem>)items).ToList();
        else list = new List<BindCardItem>();

        //if (list.Count == 0) list.Add(new BindCardItem());

        if (list.Count > 0)
        {
            int beginQty = 0;
            if (begin != null) beginQty = ((Inventory)begin).Quantity;

            list[0].BeginQuantity = beginQty.ToString();

            for (int i = 0; i < list.Count; i++)
            {
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
