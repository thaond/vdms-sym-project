using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Provider;
using VDMS.I.Service;
using VDMS.II.Entity;
using VDMS.II.Report;
using VDMS.Common.Utils;
using VDMS.Common.Web;

public partial class Part_Report_WarrantyReturn : BasePage
{
    long totalRQuantity, totalAQuantity;
    void BindData()
    {
        totalRQuantity = 0;
        totalAQuantity = 0;
        if (string.IsNullOrEmpty(lvParts.DataSourceID))
        {
            lvParts.DataSourceID = odsDealerList.ID;
        }
        else
        {
            lvParts.DataBind();
        }

        Literal litTotalR = (Literal)WebTools.FindControlById("litTotalPartR", lvParts);
        if (litTotalR != null)
            litTotalR.Text = totalRQuantity.ToString();
        Literal litTotalA = (Literal)WebTools.FindControlById("litTotalPartA", lvParts);
        if (litTotalA != null)
            litTotalA.Text = totalAQuantity.ToString();
    }

    public object GetPartReturnList(object dealerCode)
    {
        // co' [warranty part from SV] ben VDMS.I.Service
        IList<NGFormDetail> list = new WarrantyReturnReport().GetWarrantyParts(dealerCode.ToString(), txtFromDate.Text, txtToDate.Text, ddlStatus.SelectedValue, ddlArea.SelectedValue, txtPartNo.Text);
        //list.Add(new NGFormDetail() { RequestQuantity = 5, ApprovedQuantity = 2, PartCode = "ASDASD" });
        //list.Add(new NGFormDetail() { RequestQuantity = 6, ApprovedQuantity = 1, PartCode = "FASFSD" });
        totalRQuantity += list.Sum(p => p.RequestQuantity);
        totalAQuantity += list.Sum(p => p.ApprovedQuantity);
        return list;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFromDate.Text = DataFormat.DateOfFirstDayInMonth(DateTime.Now).ToShortDateString();
        }
    }


    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void odsDealerList_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {

        }
        catch
        {
        }
    }
}
