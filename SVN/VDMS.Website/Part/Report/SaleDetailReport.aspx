<%@ Page Title="Sale detail report" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="SaleDetailReport.aspx.cs" Inherits="Part_Report_SaleDetailReport"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form" style="width: 500px">
        <asp:ValidationSummary ID="ValidationSummary1" CssClass="error" ValidationGroup="Report"
            runat="server" meta:resourcekey="ValidationSummary1Resource1" />
        <table>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="Sale date:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ValidationGroup="Report" ID="RequiredFieldValidator1"
                        runat="server" SetFocusOnError="True" ControlToValidate="txtFromDate" ErrorMessage="Report date cannot be blank!"
                        meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                    <asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                        CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtFromDate"
                        PopupButtonID="ibFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    ~
                    <asp:TextBox ID="txtToDate" runat="server" meta:resourcekey="txtToDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibToDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtToDate"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="meeFromDate" CultureAMPMPlaceholder=""
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                        Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="ibToDate" BehaviorID="ceFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <vdms:DealerPlaceHolder ID="DealerPlaceHolder1" runat="server" VisibleBy="VMEP" AdminOnly="False">
                <tr>
                    <td>
                        <asp:Literal ID="Literal3" runat="server" Text="Dealer:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                    </td>
                    <td>
                        <vdms:DealerList AutoPostBack="true" runat="server" ID="ddlDealer" OnSelectedIndexChanged="Unnamed3_SelectedIndexChanged"
                            meta:resourcekey="ddlDealerResource1">
                        </vdms:DealerList>
                    </td>
                </tr>
            </vdms:DealerPlaceHolder>
            <tr>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="Warehouse:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:WarehouseList ID="ddlWarehouse" runat="server" DontAutoUseCurrentSealer="False"
                        meta:resourcekey="ddlWarehouseResource1" ShowEmptyItem="False" ShowSelectAllItem="False"
                        UseVIdAsValue="False">
                    </vdms:WarehouseList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Customer:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:CustomerList ID="ddlCustomer" ShowSelectAllItem="True" runat="server" meta:resourcekey="ddlCustomerResource1"
                        ShowNullItemIfSelectFailed="False">
                    </vdms:CustomerList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnShowReport" ValidationGroup="Report" runat="server" Text="View"
                        OnClick="btnShowReport_Click" meta:resourcekey="btnShowReportResource1" />
                    &nbsp;<asp:Button ID="btnExcel" runat="server" Text="Export to Excel" OnClick="btnExcel_Click"
                        meta:resourcekey="btnExcelResource1" />
                </td>
            </tr>
        </table>
    </div>
    <%--<CR:CrystalReportViewer ID="crViewer" runat="server" AutoDataBind="True" 
        Height="50px" meta:resourcekey="crViewerResource1" Width="350px" />--%>
    <br />
    <div class="grid">
        <vdms:PageGridView ID="GridView1" runat="server" AutoGenerateColumns="False" meta:resourcekey="GridView1Resource1"
            AllowPaging="true" PageSize="30" DataSourceID="ObjectDataSource1" 
            ShowFooter="true" ondatabound="GridView1_DataBound">
            <Columns>
                <asp:BoundField DataField="PartCode" HeaderText="Part Code" SortExpression="PartCode"
                    meta:resourcekey="BoundFieldResource1" />
                <asp:BoundField DataField="PartName" HeaderText="Part Name" SortExpression="PartName"
                    meta:resourcekey="BoundFieldResource2" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity"
                    meta:resourcekey="BoundFieldResource3" />
                <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" SortExpression="UnitPrice"
                    meta:resourcekey="BoundFieldResource4" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="Discount" HeaderText="Discount" SortExpression="Discount"
                    meta:resourcekey="BoundFieldResource5" DataFormatString="{0}%" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="SubAmount" HeaderText="Sub Amount" SortExpression="SubAmount"
                    meta:resourcekey="BoundFieldResource6" ItemStyle-HorizontalAlign="Right" />
            </Columns>
            <FooterStyle CssClass="group" HorizontalAlign="Right" />
        </vdms:PageGridView>
    </div>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetReportSource"
        TypeName="SaleDetailReport" OldValuesParameterFormatString="original_{0}" OnSelecting="ObjectDataSource1_Selecting">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtFromDate" Name="dtFrom" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="txtToDate" Name="dtTo" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="ddlWarehouse" Name="warehouseId" PropertyName="SelectedValue"
                Type="Int64" />
            <asp:ControlParameter ControlID="ddlCustomer" Name="cusId" PropertyName="SelectedValue"
                Type="Int64" />
            <asp:Parameter Name="dealerCode" Type="String" />
            <asp:Parameter Name="dbCode" Type="String" />
            <asp:Parameter Direction="Output" Name="total" Type="Decimal" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
