using System;
using System.Web;
using System.Web.UI.WebControls;
using Resources;
using VDMS.Core.Domain;
using VDMS.Helper;
using VDMS.I.ObjectDataSource;
using VDMS.I.Service;
using VDMS.I.Vehicle;

public partial class Service_Popup_SelectCustomer : BasePopup
{
    public string CusType
    {
        get { return Request.QueryString["ct"]; }
    }

    private string Sessionkey { get { return Request.QueryString["sessionkey"]; } }

    protected void InitForm()
    {
        gvSelectCust.PageSize = SrsSetting.selectGridViewPageSize;
        if ((CusType == "SV") || string.IsNullOrEmpty(CusType)) btnSearch_Click(null, null);
        pnSelectCusHeader.Visible = CusType != "SL";
        divSearch.Visible = CusType == "SL";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitForm();
        }
    }

    protected void gvSelectxxx_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView gv = (GridView)sender;
        if (((string)e.CommandName == "Page") && ((((string)e.CommandArgument == "Next") && ((gv.PageIndex + 1) == gv.PageCount)) || (((string)e.CommandArgument == "Prev") && (gv.PageIndex == 0))))
        {
            gv.DataBind();
        }
    }

    protected void gvSelectxxx_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        if (gv.TopPagerRow == null) return;
        Literal litPageInfo = gv.TopPagerRow.FindControl("lit" + gv.ID + "PageInfo") as Literal;
        if (litPageInfo != null) litPageInfo.Text = string.Format(Message.PagingInfo, gv.PageIndex + 1, gv.PageCount, HttpContext.Current.Items["list" + gv.ID + "RowCount"]);
    }

    protected void gvSelectCust_RowDataBound(object sender, GridViewRowEventArgs e)
    {//e
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    Customer cust = (Customer)e.Row.DataItem;
        //    e.Row.Cells[3].Text = ServiceTools.GetCustAddress(cust);
        //    //if (e.Row.Cells[].Text.Trim() == ".") e.Row.Cells[3].Text = ""; // email
        //    e.Row.Cells[4].Text = (cust.Birthdate > DateTime.MinValue) ? cust.Birthdate.ToShortDateString() : "";
        //    //if (e.Row.Cells[].Text.Trim() == ".") e.Row.Cells[5].Text = ""; // mobile
        //}
    }

    protected void btnSelectCust_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        Customer cust = CustomerDataSource.GetCustomer(btn.CommandArgument);
        if (!string.IsNullOrEmpty(Sessionkey))
        {
            SaleVehicleHelper.Init(Sessionkey);
            SaleVehicleHelper.CommonSaleData.CustomerId = cust.Id;
            SaleVehicleHelper.CommonSaleData.Address = cust.Address;
            SaleVehicleHelper.CommonSaleData.DOB = cust.Birthdate;
            SaleVehicleHelper.CommonSaleData.CustomerDescription = cust.Customerdescription;
            SaleVehicleHelper.CommonSaleData.District = cust.Districtid;
            SaleVehicleHelper.CommonSaleData.Email = cust.Email;
            SaleVehicleHelper.CommonSaleData.CustomerName = cust.Fullname;
            SaleVehicleHelper.CommonSaleData.Gender = cust.Gender ? 1 : 0;
            SaleVehicleHelper.CommonSaleData.IdentifyNumber = cust.Identifynumber;
            SaleVehicleHelper.CommonSaleData.CustomerType = cust.Customertype;
            SaleVehicleHelper.CommonSaleData.JobType = cust.Jobtypeid;
            SaleVehicleHelper.CommonSaleData.Mobile = cust.Mobile;
            SaleVehicleHelper.CommonSaleData.Precint = cust.Precinct;
            SaleVehicleHelper.CommonSaleData.Priority = cust.Priority;
            SaleVehicleHelper.CommonSaleData.Province = cust.Provinceid;
            SaleVehicleHelper.CommonSaleData.Tel = cust.Tel;

            SaleVehicleHelper.SaveCommonSaleInfo();
            ClosePopup("update()");
        }
        else if (CusType == "SL")
            ClosePopup(string.Format("cusSelected({0},'{1}','{2}')", cust.Id, cust.Fullname, cust.Identifynumber));
        else
        {
            PopupHelper.SetSelectCusSession(this.Key, cust);
            ClosePopup("cusSelected()");
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(gvSelectCust.DataSourceID)) gvSelectCust.DataSourceID = odsCustomer.ID;
        else gvSelectCust.DataBind();
    }
}
