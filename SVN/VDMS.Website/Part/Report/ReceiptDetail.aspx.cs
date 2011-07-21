using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Provider;
using VDMS.II.Report;
using VDMS.Helper;
using VDMS.II.PartManagement.Order;
using System.Collections;
using VDMS.Common.Utils;
using VDMS.Common.Web;

public partial class Part_Report_ReceiptDetail : BasePage
{
    void BindData()
    {
        if (string.IsNullOrEmpty(lv.DataSourceID)) lv.DataSourceID = odsAbnormalOrder.ID;
        else lv.DataBind();
    }

    void LoadDealers()
    {
        ddlDealers.DatabaseCode = ddlArea.SelectedValue;
        ddlDealers.DataBind();
    }

    protected string EvalPartName(object part)
    {
        string res = "";
        if (part != null)
        {
            res = (UserHelper.Language == "vi-VN") ? (part as VDMS.II.Entity.Part).VietnamName : (part as VDMS.II.Entity.Part).EnglishName;
        }
        return res;
    }
    new protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
        ddlDealers.RootDealer = UserHelper.DealerCode;
        ddlDealers.RemoveRootItem = UserHelper.DealerCode == "/";
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
    protected void odsAbnormalOrder_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try   // => select list, by pass select count
        {
            IList<AbnormalOrder> list = (IList<AbnormalOrder>)e.ReturnValue;
            if ((list != null) && (list.Count > 0))
            {
                Literal litB = (Literal)lv.FindControl("litTotalBroken");
                Literal litW = (Literal)lv.FindControl("litTotalWrong");
                Literal litL = (Literal)lv.FindControl("litTotalLack");

                litB.Text = list.Sum(o => o.Broken).ToString();
                litW.Text = list.Sum(o => o.Wrong).ToString();
                litL.Text = list.Sum(o => o.Lack).ToString();
            }
        }
        catch { }
    }
    protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDealers();
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        bool paged = odsAbnormalOrder.EnablePaging;
        odsAbnormalOrder.EnablePaging = false;

        string dtFrom = DataFormat.DateFromString(txtFromDate.Text).ToString("yyyy_MM_dd");
        string dtTo = DataFormat.DateFromString(txtToDate.Text).ToString("yyyy_MM_dd");
        string dl = string.IsNullOrEmpty(ddlDealers.SelectedValue) ? "All" : ddlDealers.SelectedValue;
        string fileName = string.Format("RecvDetail.{0}.{1}-{2}.xls", dl, dtFrom, dtTo);

        lvExcel.DataSource = odsAbnormalOrder.ID;
        if (lvExcel.Items.Count > 0)
        {
            GridView2Excel.Export(lvExcel, fileName);
        }

        odsAbnormalOrder.EnablePaging = paged;

    }
}
