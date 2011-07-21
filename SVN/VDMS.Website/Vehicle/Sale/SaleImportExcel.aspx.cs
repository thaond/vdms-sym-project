using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS;
using VDMS.I.Vehicle;
using Excel;

public partial class Vehicle_Sale_SaleImportExcel : BasePage
{
    string impkey = "SaleImportExcel_list";

    protected void Page_Load(object sender, EventArgs e)
    {
        InitErrMsgControl(msg);
        if (!IsPostBack)
        {
            SaleVehicleHelper.ClearSession();
        }
    }

    public string EvalPaymentMethod(object p)
    {
        switch (int.Parse(p.ToString()))
        {
            case (int)CusPaymentType.FixedHire_purchase: return Resources.PaymentMethod.FixedInstalment;
            case (int)CusPaymentType.UnFixedHire_purchase: return Resources.PaymentMethod.UnfixedInstalment;
            default: return Resources.PaymentMethod.PayAll;
        }
    }

    protected void Button1_OnClick(object sender, EventArgs e)
    {
        //try
        //{
            if (FileUpload1.HasFile)
            {
                SaleVehicleHelper.Init(impkey);
                IExcelDataReader exceldatareader;
                if (SaleVehicleHelper.ValidExcelData(FileUpload1.PostedFile.InputStream,out exceldatareader))
                    SaleVehicleHelper.ImportExcelData(exceldatareader, VDMSSetting.CurrentSetting.SaleVehicleExcelUploadSetting);
                else
                {
                    AddErrorMsg("File Excel invalid");
                }
            }
            SaleVehicleHelper.CheckAllVehicleInfoes(SaveSaleAction.ImportVehicles);
            lvE.DataSource = SaleVehicleHelper.Vehicles;
            lvE.DataBind();
        //}
        //catch (Exception ex)
        //{
         //   Literal1.Text = ex.Message;
        //}
    }
    
    protected void Button2_OnClick(object sender, EventArgs e)
    {
        //var res = SaleVehicleHelper.CheckAllVehicleInfoes(SaveSaleAction.ImportVehicles);
        //if (res == Resources.Message.ActionSucessful)
        Literal1.Text = SaleVehicleHelper.SaveAllVehicleInfoes(SaveSaleAction.ImportVehicles);
        //else Literal1.Text = res;
        SaleVehicleHelper.ClearSession();
        lvE.DataBind();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        SaleVehicleHelper.ClearSession();
        lvE.DataBind();
    }
    protected void lvE_DataBound(object sender, EventArgs e)
    {
        var lv = (ListView)sender;

        if (lv.Items.Count > 0)
        {
            Button2.Enabled = true;
            Button3.Enabled = true;
        }
        else
        {
            Button2.Enabled = false;
            Button3.Enabled = false;
        }
    }
    protected void lvE_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
    }
}
