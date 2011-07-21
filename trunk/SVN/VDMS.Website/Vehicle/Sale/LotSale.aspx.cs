 using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.I.Vehicle;
using VDMS.Helper;
using System.Drawing;

public partial class Vehicle_Sale_LotSale : BasePage
{
    private RangeValidator DateValid(RangeValidator rvDate, DateTime maxVal)
    {
        rvDate.MinimumValue = DateTime.MinValue.ToShortDateString();
        rvDate.MaximumValue = maxVal.ToShortDateString();
        return rvDate;
    }

    string lotkey = "salelot_list";
    public string PageKey;
    public void NewPageKey()
    {
        PageKey = Guid.NewGuid().ToString();
        SaveState("PageKey", PageKey);
    }
    public void LoadPageKey()
    {
        PageKey = (string)LoadState("PageKey");
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        // Update form when postback
        txtCustomerName.Text = SaleVehicleHelper.CommonSaleData.CustomerName;

        if (!IsPostBack)
        {
            SaleVehicleHelper.InitSale(lotkey);

            NewPageKey();
            _key.Value = lotkey;
            lnkSearchVehicles.NavigateUrl = string.Format("../Popup/SearchVehicle.aspx?key={0}&sessionkey={1}&TB_iframe=true&height=480&width=600", PageKey, lotkey);
            lnkFindCustomer.NavigateUrl = string.Format("../../Service/Popup/SelectCustomer.aspx?key={0}&sessionkey={1}&ct=SL&dc={2}&TB_iframe=true", PageKey, lotkey, UserHelper.DealerCode);

            var custAction = string.IsNullOrEmpty(SaleVehicleHelper.CommonSaleData.CustomerName) ? "insert" : "edit";
            string js = string.Format("window.showModalDialog(\"CusInfInput.aspx?action={0}&sessionkey={1}\", null, \"status:false;dialogWidth:750px;dialogHeight:500px\")", custAction, lotkey);
            btnCreateCustomer.OnClientClick = js;
            btnCreateCustomer.UseSubmitBehavior = false;

            rvSellingDate = DateValid(rvSellingDate, DateTime.Now);
            rvRecDate = DateValid(rvRecDate, DateTime.MaxValue);

            txtSellingDate.Text = DateTime.Now.ToShortDateString();
            txtRecCDate.Text = DateTime.Now.ToShortDateString();
        }
        else
        {
            LoadPageKey();
        }

        foreach (GridViewRow row in gv.Rows)
        {
            int p;
            DateTime d;
            var price = int.TryParse(((TextBox)row.FindControl("txtPriceTax")).Text, out p) ? p : 0;
            var numberPlate = ((TextBox)row.FindControl("txtPlateNo")).Text;
            var numberPlateDate = DateTime.TryParse(((TextBox)row.FindControl("txtTakPlateNoDate")).Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out d) ? d : DateTime.MinValue;

            SaleVehicleHelper.UpdateVehicle(row.Cells[0].Text, v =>
            {
                v.Price = price;
                v.NumberPlate = numberPlate;
                v.NumberPlateRecDate = numberPlateDate;

            });

        }
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        gv.DataBind();
    }

    protected void btnAddVehicle_Click(object sender, EventArgs e)
    {
        var engineNo = txtEngineNo.Text.Trim().ToUpper();
        if (VehicleDAO.IsVehicleExisted(engineNo, UserHelper.BranchCode, ItemStatus.Imported))
        {
            SaleVehicleHelper.AddVehicle(engineNo);
            SaleVehicleHelper.SaveSession();
            gv.DataBind();
        }
        else
        {
            ShowMessage(Resources.Message.VehicleError, true);
        }
    }

    protected void gv_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        SaleVehicleHelper.RemoveVehicle(e.NewSelectedIndex);
        SaleVehicleHelper.SaveSession();
        e.Cancel = true;
        gv.DataBind();
    }
    protected void btnSell_Click(object sender, EventArgs e)
    {
        Page.Validate();
        if (!Page.IsValid) return;
        string msg;
        if (!BindFieldsToVehicleData(true))
        {
            return;
        }

        msg = SaleVehicleHelper.CheckLotSale();
        if (msg != Resources.Message.ActionSucessful)
        {
            ShowMessage(msg, true);
            return;
        }
        ShowMessage(SaleVehicleHelper.SaveAllVehicleInfoes(SaveSaleAction.LotSale), false);

        SaleVehicleHelper.ClearSession();
        SaleVehicleHelper.ClearCommonSaleInfo();
        txtBillNo.Text = "";
        txtComment.Text = "";
        txtCustomerName.Text = "";
        txtEngineNo.Text = "";
        txtRecCDate.Text = "";
        txtSellingDate.Text = DateTime.Now.ToShortDateString();
        txtSellingType.Text = "";
        ddlPaymentMethod.SelectedValue = "0";

    }

    protected void ShowMessage(string message, bool isError)
    {
        txtMessage.Visible = true;
        txtMessage.Text = message;
        txtMessage.ForeColor = isError ? Color.Red : Color.RoyalBlue;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        SaleVehicleHelper.ClearSession();
        gv.DataBind();
    }

    private bool BindFieldsToVehicleData(bool validate)
    {
        DateTime d;

        SaleVehicleHelper.CommonSaleData.SellDate = DateTime.TryParse(txtSellingDate.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out d) ? d : DateTime.MinValue;
        SaleVehicleHelper.CommonSaleData.PaymentDate = DateTime.TryParse(txtRecCDate.Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out d) ? d : DateTime.MinValue;
        SaleVehicleHelper.CommonSaleData.BillNumber = txtBillNo.Text;
        SaleVehicleHelper.CommonSaleData.PaymentType = int.Parse(ddlPaymentMethod.SelectedValue);
        SaleVehicleHelper.CommonSaleData.SellType = txtSellingType.Text;
        SaleVehicleHelper.CommonSaleData.CommentSellItem = txtComment.Text;

        SaleVehicleHelper.BindCommonData();

        foreach (GridViewRow row in gv.Rows)
        {
            int p;
            var price = int.TryParse(((TextBox)row.FindControl("txtPriceTax")).Text, out p) ? p : 0;
            if (price == 0 && validate)
            {
                ShowMessage(Resources.Message.NoPrice, true);
                return false;
            }
            var numberPlate = ((TextBox)row.FindControl("txtPlateNo")).Text;
            var numberPlateDate = DateTime.TryParse(((TextBox)row.FindControl("txtTakPlateNoDate")).Text, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out d) ? d : DateTime.MinValue;
            SaleVehicleHelper.UpdateVehicle(row.Cells[0].Text, v =>
            {
                v.Price = price;
                v.NumberPlate = numberPlate;
                v.NumberPlateRecDate = numberPlateDate;
            });

        }
        return true;
    }
    protected void gv_DataBound(object sender, EventArgs e)
    {
        if (gv.Rows.Count > 0)
        {
            int p, allPrice = 0;

            foreach (GridViewRow row in gv.Rows)
            {

                var tb = (TextBox)row.FindControl("txtPriceTax");
                var v = SaleVehicleHelper.GetVehicle(row.Cells[0].Text);
                p = v.Price;
                tb.Text = p.ToString();
                ((TextBox)row.FindControl("txtTakPlateNoDate")).Text = v.NumberPlateRecDate == DateTime.MinValue ? "" : v.NumberPlateRecDate.ToShortDateString();
                ((TextBox)row.FindControl("txtPlateNo")).Text = v.NumberPlate;
                allPrice += p;
            }
            gv.FooterRow.Cells[4].Text = allPrice.ToString("C0");
        }
    }
    protected void gv_DataBinding(object sender, EventArgs e)
    {
        //foreach (GridViewRow row in gv.Rows)
        //{
        //    int p;
        //    var tb = (TextBox)row.FindControl("txtPriceTax");
        //    var price = int.TryParse(tb.Text, out p) ? p : 0;

        //    SaleVehicleHelper.UpdateVehicle(row.Cells[0].Text, v =>
        //    {
        //        v.Price = price;
        //    });

        //}
    }
}
