<%@ Page Title="Input/Sale/Stock report" Language="C#" MasterPageFile="~/MP/Mobile.master" Theme="Mobile"
    AutoEventWireup="true" CodeFile="IOSReport.aspx.cs" Inherits="MPart_Report_IOSReport"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <asp:ValidationSummary ID="ValidationSummary1" CssClass="error" ValidationGroup="Report"
            runat="server" meta:resourcekey="ValidationSummary1Resource1" />
        <table>
            <tr>
                <td style="width: 25%;">
                    <asp:Localize ID="litType" runat="server" Text="Type:" meta:resourcekey="litTypeResource1"></asp:Localize>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" meta:resourcekey="rblTypeResource1">
                        <asp:ListItem Selected="True" Text="<%$ Resources:TextMsg, Part %>" Value="P" meta:resourcekey="ListItemResource1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:TextMsg, Accessory %>" Value="A" meta:resourcekey="ListItemResource2"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="Transaction date:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" Width="40%" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ValidationGroup="Report" ID="RequiredFieldValidator1"
                        runat="server" SetFocusOnError="True" ControlToValidate="txtFromDate" ErrorMessage="Report date cannot be blank!"
                        meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                    
                    <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                        Mask="99/99/9999" MaskType="Date">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="txtFromDate" Enabled="true" InvalidValueMessage="*" ControlExtender="meeFromDate"></ajaxToolkit:MaskedEditValidator>
                    ~
                    <asp:TextBox ID="txtToDate" runat="server" Width="40%" meta:resourcekey="txtToDateResource1"></asp:TextBox>
                    
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1"  runat="server" TargetControlID="txtToDate"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="meeFromDate" CultureAMPMPlaceholder=""
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                        Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlToValidate="txtToDate" Enabled="true" InvalidValueMessage="*" ControlExtender="MaskedEditExtender1"></ajaxToolkit:MaskedEditValidator>
                </td>
            </tr>
            <vdms:DealerPlaceHolder runat="server" VisibleBy="VMEP" AdminOnly="False">
            </vdms:DealerPlaceHolder>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="Dealer:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:DealerList runat="server" AppendDataBoundItems="True" AutoPostBack="True" ID="ddlDealer" Width="80%"
                        OnSelectedIndexChanged="Unnamed3_SelectedIndexChanged" EnabledSaperateByArea="False"
                        EnabledSaperateByDB="False" MergeCode="False" meta:resourcekey="ddlDealerResource1"
                        RemoveRootItem="False" ShowEmptyItem="False" ShowSelectAllItem="False">
                    </vdms:DealerList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="Warehouse" meta:resourcekey="Literal4Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:WarehouseList ShowSelectAllItem="True" ID="ddlWarehouse" runat="server" DontAutoUseCurrentSealer="False"
                        meta:resourcekey="ddlWarehouseResource1" ShowEmptyItem="False" UseVIdAsValue="False">
                    </vdms:WarehouseList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnShowReport" runat="server" ValidationGroup="Report" Text="View" Width="40%"
                        OnClick="btnShowReport_Click" meta:resourcekey="btnShowReportResource1" />
                    <%--<asp:Button ID="btnShowReport2" runat="server" Text="View old" OnClick="btnShowReport2_Click" />--%>
                    <%--<asp:Button ID="btnExcel" runat="server" Text="Export to excel" OnClick="btnExcel_Click"
                        meta:resourcekey="btnExcelResource1" />--%>
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <vdms:PageGridView ID="GridView1" runat="server" AutoGenerateColumns="False"
            meta:resourcekey="GridView1Resource1" AllowPaging="true" DataSourceID="ods" PageSize="30">
            <Columns>
                <asp:BoundField DataField="No" HeaderText="No" SortExpression="No" meta:resourcekey="BoundFieldResource1" />
                <asp:BoundField DataField="PartCode" HeaderText="Part Code" SortExpression="PartCode"
                    meta:resourcekey="BoundFieldResource2" />
                <asp:BoundField DataField="PartName" HeaderText="Part Name" SortExpression="PartName"
                    meta:resourcekey="BoundFieldResource3" />
                <asp:BoundField DataField="Begin" HeaderText="Begin" SortExpression="Begin" meta:resourcekey="BoundFieldResource4" />
                <asp:BoundField DataField="In" HeaderText="In" SortExpression="In" meta:resourcekey="BoundFieldResource5" />
                <asp:BoundField DataField="InAmount" HeaderText="In Amount" SortExpression="InAmount"
                    meta:resourcekey="BoundFieldResource6" />
                <asp:BoundField DataField="Out" HeaderText="Out" SortExpression="Out" meta:resourcekey="BoundFieldResource7" />
                <asp:BoundField DataField="OutAmount" HeaderText="Out Amount" SortExpression="OutAmount"
                    meta:resourcekey="BoundFieldResource8" />
                <asp:BoundField DataField="Stock" HeaderText="Stock" SortExpression="Stock" meta:resourcekey="BoundFieldResource9" />
            </Columns>
        </vdms:PageGridView>
    </div>
    <asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetReportSource" TypeName="VDMS.II.Report.InOutStockReport">
        <SelectParameters>
            <asp:ControlParameter ControlID="rblType" Name="partType" PropertyName="SelectedValue"
                Type="String" />
            <asp:ControlParameter ControlID="txtFromDate" Name="dtFrom" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="txtToDate" Name="dtTo" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="ddlWarehouse" Name="warehouseId" PropertyName="SelectedValue"
                Type="Int64" />
            <asp:ControlParameter ControlID="ddlDealer" Name="dealerCode" PropertyName="SelectedValue"
                Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
