<%@ Page Title="Homepage" Language="C#" MasterPageFile="~/MP/Mobile.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Mdefault" Culture="auto" meta:resourcekey="PageResource1"
    Theme="Mobile" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <!-- Begin page wrapper -->
    <h2 class="landing">
        <asp:Label runat="server" meta:resourcekey="VehicleLabelResource1" Text="Vehicle:"></asp:Label></h2>
    <section class="tableView grouped">
    <ul class="ul-mobile">
            <li>
            <a href="/Mobile/Bonus/Query.aspx"><asp:Label runat="server" meta:resourcekey="BonnusLabelResource1"
                            Text="Bonus"></asp:Label></a></li>
        
            <li>
            <a href="/Mobile/Vehicle/CheckOrder.aspx"><asp:Label runat="server" meta:resourcekey="CheckOrderResource1"
                            Text="CheckOrder"></asp:Label></a></li>
            <li>
            <a href="/Mobile/Vehicle/InventoryDetail.aspx"><asp:Label runat="server" meta:resourcekey="InventoryDetailLabelResource1"
                            Text="InventoryDetail"></asp:Label></a></li>
            <li>
            <a href="/Mobile/Vehicle/DataCheck.aspx"><asp:Label runat="server" meta:resourcekey="CustomerDataLabelResource1"
                            Text="CustomerData"></asp:Label></a></li>
        
            <li>
            <a href="/Mobile/Vehicle/IOStockReport.aspx"><asp:Label runat="server" meta:resourcekey="StockReportLabelResource1"
                            Text="StockReport"></asp:Label></a></li>
            <li>
            <a href="/Mobile/Vehicle/SaleDetail.aspx"><asp:Label runat="server" meta:resourcekey="SaleDetailLabelResource1"
                            Text="SaleDetail"></asp:Label></a></li>
                            <li>
            <a href="/Mobile/Vehicle/DailySaleReport.aspx"><asp:Label runat="server" meta:resourcekey="DailySaleReportLabelResource1"
                            Text="DailySaleReport"></asp:Label></a></li>
                            <li>
            <a href="/Mobile/Service/ExchangeReport.aspx"><asp:Label ID="Label1" runat="server" meta:resourcekey="ExchangeReportLabelResource1"
                            Text="ExchangeReport"></asp:Label></a></li>
                            <li>
            <a href="/Mobile/Vehicle/OrderStatistic.aspx"><asp:Label ID="Label2" runat="server" meta:resourcekey="OrderStatisticLabelResource1"
                            Text="OrderStatistic"></asp:Label></a></li>
        </ul>
    </section>
    <h2 class="landing">
        <asp:Label ID="Part" runat="server" meta:resourcekey="PartLabelResource1" Text="Part:"></asp:Label></h2>
    <section class="tableView grouped">
    <ul class="ul-mobile">
            <li>
            <a href="/Mobile/Part/InShort.aspx"><asp:Label ID="Label3" runat="server" meta:resourcekey="InShortResource"
                            Text="SaleDetail"></asp:Label></a></li>
                             <li>
            <a href="/Mobile/Part/RemainingOrderDetailReport.aspx"><asp:Label ID="Label4" runat="server" meta:resourcekey="RemainingOrderDetailReportResource"
                            Text="SaleDetail"></asp:Label></a></li>
                             <li>
            <a href="/Mobile/Part/inventory.aspx"><asp:Label ID="Label5" runat="server" meta:resourcekey="inventoryResource"
                            Text="SaleDetail"></asp:Label></a></li>
                             <li>
            <a href="/Mobile/Part/IOSReport.aspx"><asp:Label ID="Label6" runat="server" meta:resourcekey="IOSReportResource"
                            Text="SaleDetail"></asp:Label></a></li>
                             <li>
            <a href="/Mobile/Part/SaleDetailReport.aspx"><asp:Label ID="Label7" runat="server" meta:resourcekey="SaleDetailReportResource"
                            Text="SaleDetail"></asp:Label></a></li>
                             <li>
            <a href="/Mobile/Part/CustomerReport.aspx"><asp:Label ID="Label8" runat="server" meta:resourcekey="CustomerReportResource"
                            Text="SaleDetail"></asp:Label></a></li>
    </ul>
    </section>
    <!-- End page wrapper -->
</asp:Content>
