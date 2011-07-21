using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using VDMS.I.Report.DataObject;
using CrystalDecisions.CrystalReports.Engine;
using VDMS.I.Service;
using VDMS.Common.Utils;
using VDMS.Core.Domain;
using VDMS.Helper;

public partial class Service_Report_PrintSRS : BasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        Page.RegisterRequiresControlState(this);
    }
    protected override object SaveControlState()
    {
        object[] ctlState = new object[2];
        ctlState[0] = base.SaveControlState();
        ctlState[1] = info;

        return ctlState;
    }

    public SrsInfo info { get; set; }

    protected override void LoadControlState(object state)
    {
        if (state != null)
        {
            object[] ctlState = (object[])state;
            base.LoadControlState(ctlState[0]);
            info = (SrsInfo)ctlState[1];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            rptViewer.DisplayGroupTree = false;
            rptViewer.HasCrystalLogo = false;
        }
        if (Page.PreviousPage != null)
        {
            info = PreviousPage.Info;
            //lnkBackTop.NavigateUrl = lnkBack.NavigateUrl = info.RequestURL;
            lnkBackTop.PostBackUrl = lnkBack.PostBackUrl = info.RequestURL;
        }

        MultiView1.ActiveViewIndex = LoadReport() ? 1 : 0;
    }

    protected bool LoadReport()
    {
        if (info == null) return false;

        ServiceType sertype = (ServiceType)info.ServiceHeader.Servicetype;
        string cusAddress = ServiceTools.GetCustAddress(info.ServiceHeader.Customer);
        string cusPhone = ServiceTools.GetCustTelNo(info.ServiceHeader.Customer);
        string cusName = (info.ServiceHeader.Customer == null) ? "" : info.ServiceHeader.Customer.Fullname;
        string prevDealer = (PreviousPage != null) ? PreviousPage.CurrentDealer : "";

        ReportDocument rdServiceRecordSheet = ReportFactory.GetReport();
        rdServiceRecordSheet.Load(Server.MapPath(@"~/Report/ServiceRecordSheet.rpt"));
        rdServiceRecordSheet.SetDataSource(BuilDatasource(info));
        rdServiceRecordSheet.SetParameterValue("DealerName", DealerHelper.GetName(string.IsNullOrEmpty(info.ServiceHeader.Dealercode) ? prevDealer : info.ServiceHeader.Dealercode));
        rdServiceRecordSheet.SetParameterValue("DealerAddress", DealerHelper.GetAddress(string.IsNullOrEmpty(info.ServiceHeader.Dealercode) ? prevDealer : info.ServiceHeader.Dealercode));
        rdServiceRecordSheet.SetParameterValue("ServiceRecordNumber", string.IsNullOrEmpty(info.ServiceHeader.Servicesheetnumber) ? "" : info.ServiceHeader.Servicesheetnumber);
        rdServiceRecordSheet.SetParameterValue("ServiceRecordDate", info.ServiceHeader.Servicedate.ToShortDateString());
        rdServiceRecordSheet.SetParameterValue("CustomerFullName", cusName);
        rdServiceRecordSheet.SetParameterValue("CustomerAddress", (string.IsNullOrEmpty(cusAddress)) ? "" : cusAddress);
        rdServiceRecordSheet.SetParameterValue("CustomerTel", (string.IsNullOrEmpty(cusPhone)) ? "" : cusPhone);
        rdServiceRecordSheet.SetParameterValue("CustomerPurchaseDate", info.ServiceHeader.Purchasedate.ToShortDateString());
        rdServiceRecordSheet.SetParameterValue("Model", string.IsNullOrEmpty(info.ServiceHeader.Itemtype) ? "" : info.ServiceHeader.Itemtype);
        rdServiceRecordSheet.SetParameterValue("Color", string.IsNullOrEmpty(info.ServiceHeader.Colorcode) ? "" : info.ServiceHeader.Colorcode);
        rdServiceRecordSheet.SetParameterValue("EngineNumber", string.IsNullOrEmpty(info.ServiceHeader.Enginenumber) ? "" : info.ServiceHeader.Enginenumber);
        rdServiceRecordSheet.SetParameterValue("Kilometer", info.ServiceHeader.Kmcount.ToString());
        rdServiceRecordSheet.SetParameterValue("PlateNumber", (string.IsNullOrEmpty(info.ServiceHeader.Numberplate)) ? "" : info.ServiceHeader.Numberplate);
        rdServiceRecordSheet.SetParameterValue("DamagedStatus", string.IsNullOrEmpty(info.ServiceHeader.Damaged) ? "" : info.ServiceHeader.Damaged);
        rdServiceRecordSheet.SetParameterValue("SolutionText", (string.IsNullOrEmpty(info.ServiceHeader.Repairresult)) ? "" : info.ServiceHeader.Repairresult);
        rdServiceRecordSheet.SetParameterValue("PartCost", (decimal)info.GetSparesAmount());
        rdServiceRecordSheet.SetParameterValue("LabourCost", (decimal)info.ServiceHeader.Feeamount);
        rdServiceRecordSheet.SetParameterValue("Amount", (decimal)info.ServiceHeader.Totalamount);
        rdServiceRecordSheet.SetParameterValue("isMaintain", (sertype == ServiceType.Maintain) || (sertype == ServiceType.MaintainAndRepair) || (sertype == ServiceType.MaintainAndWarranty) || (sertype == ServiceType.RepairAndMaintainAndWarranty));
        rdServiceRecordSheet.SetParameterValue("isRepair", (sertype == ServiceType.MaintainAndRepair) || (sertype == ServiceType.Repair) || (sertype == ServiceType.RepairAndMaintainAndWarranty) || (sertype == ServiceType.WarrantyAndRepair));
        rdServiceRecordSheet.SetParameterValue("isWarranty", (sertype == ServiceType.MaintainAndWarranty) || (sertype == ServiceType.RepairAndMaintainAndWarranty) || (sertype == ServiceType.Warranty) || (sertype == ServiceType.WarrantyAndRepair));

        rptViewer.DisplayGroupTree = false;// CrystalReportViewer1.HasCrystalLogo = false;
        rptViewer.ReportSource = rdServiceRecordSheet;
        rptViewer.DataBind();

        return rptViewer != null;
    }

    private DataTable BuilDatasource(SrsInfo info)
    {
        DataTable tbl = WarrantyContent.SpareListOnServiceSchema;
        int i = 1;
        foreach (PCVItem item in info.ExchangePartDetail)
        {
            //Warrantycondition warr = WarrantyContent.GetWarrantyCondition(item.Partcodeo);
            DataRow row = tbl.NewRow();
            row["ItemId"] = item.Id;
            row["SpareNo"] = i;
            row["SpareNumber"] = item.Partcodeo;
            row["SpareNameEn"] = item.PartName;
            row["SpareNameVn"] = item.PartName;
            row["SpareName"] = item.PartName;
            row["Quantity"] = item.Partqtyo;
            row["SpareCost"] = item.Unitpriceo;
            row["ExchangeNumber"] = info.ExchangePartHeader.Vouchernumber;
            row["IsExchangeSpare"] = true;
            row["SpareAmount"] = null;
            row["FeeAmount"] = item.FeeAmount;
            i++;
            tbl.Rows.Add(row);
        }
        foreach (SRSItem item in info.ServiceDetail)
        {
            //Warrantycondition warr = WarrantyContent.GetWarrantyCondition(item.Partcodeo);
            DataRow row = tbl.NewRow();
            row["ItemId"] = item.Id;
            row["SpareNo"] = i;
            row["SpareNumber"] = item.Partcode;
            row["SpareNameEn"] = item.Partname;
            row["SpareNameVn"] = item.Partname;
            row["SpareName"] = item.Partname;
            row["Quantity"] = item.Partqty;
            row["SpareCost"] = item.Unitprice;
            row["ExchangeNumber"] = "";
            row["IsExchangeSpare"] = false;
            row["SpareAmount"] = item.Partqty * item.Unitprice;
            row["FeeAmount"] = item.FeeAmount;
            i++;
            tbl.Rows.Add(row);
        }

        return tbl;
    }
}
