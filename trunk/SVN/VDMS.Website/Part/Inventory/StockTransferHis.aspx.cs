using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using VDMS.II.Entity;
using VDMS.II.BasicData;
using VDMS.II.Linq;
using VDMS.Common.Utils;

public partial class Part_Inventory_StockTransferHis : BasePage
{
    public long FromWarehouseId
    {
        get
        {
            long wh;
            long.TryParse(ddlFromWh.SelectedValue, out wh);
            return wh;
        }
    }
    public long ToWarehouseId
    {
        get
        {
            long wh;
            long.TryParse(ddlToWh.SelectedValue, out wh);
            return wh;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFromDate.Text = DataFormat.DateOfFirstDayInMonth(DateTime.Now).ToShortDateString();
            txtToDate.Text = DateTime.Now.ToShortDateString();
        }
    }

    Hashtable _address = new Hashtable();
    protected object EvalAddress(object wid)
    {
        if (wid == null) return "";

        if (_address.ContainsKey(wid)) return _address[wid];
        else
        {
            //var dc = DCFactory.GetDataContext<PartDataContext>();
            //dc.Dealers.de
            Warehouse w = WarehouseDAO.GetWarehouse(long.Parse(wid.ToString()));
            _address[wid] = w.DealerCode + " -- " + w.Address;
            return _address[wid];
        }
    }



    protected void ddlFromDl_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFromWh.DealerCode = ddlFromDl.SelectedValue;
        ddlFromWh.DataBind();
    }
    protected void ddlToDl_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlToWh.DealerCode = ddlToDl.SelectedValue;
        ddlToWh.DataBind();
    }
    protected void ddlFromWh_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }


    protected void btnQuery_Click(object sender, EventArgs e)
    {
        //if (string.IsNullOrEmpty(gvD.DataSourceID))
            gvH.DataSourceID = "odsTrans";
        gvH.DataBind();
        gvD.DataBind();
        //UpdatePanel1.Update();
    }

    protected void gvH_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
