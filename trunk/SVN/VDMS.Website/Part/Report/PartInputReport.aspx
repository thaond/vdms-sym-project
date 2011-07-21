<%@ Page Title="Part input report" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="PartInputReport.aspx.cs" Inherits="Part_Report_PartInputReport"
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
                    <asp:Literal ID="Literal2" runat="server" Text="Received date:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ValidationGroup="Report" ID="RequiredFieldValidator1"
                        runat="server" SetFocusOnError="True" ControlToValidate="txtFromDate" ErrorMessage="Report date cannot be blank!"
                        meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                    <asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                        Mask="99/99/9999" MaskType="Date" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtFromDate"
                        PopupButtonID="ibFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    ~
                    <asp:TextBox ID="txtToDate" runat="server" meta:resourcekey="txtToDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibToDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtToDate"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="meeFromDate" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="ibToDate" BehaviorID="ceFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <vdms:DealerPlaceHolder runat="server" VisibleBy="VMEP" AdminOnly="False">
            </vdms:DealerPlaceHolder>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="Dealer:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:DealerList AutoPostBack="True" runat="server" ID="ddlDealer" OnSelectedIndexChanged="Unnamed3_SelectedIndexChanged"
                        EnabledSaperateByArea="False" EnabledSaperateByDB="False" MergeCode="False" meta:resourcekey="ddlDealerResource1"
                        RemoveRootItem="False" ShowEmptyItem="False" ShowSelectAllItem="False">
                    </vdms:DealerList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="Warehouse" meta:resourcekey="Literal4Resource1"></asp:Literal>
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
    <div class="grid">
        <vdms:PageGridView ID="GridView1" runat="server" AutoGenerateColumns="False" meta:resourcekey="GridView1Resource1"
            DataSourceID="ods" AllowPaging="true" PageSize="30">
            <Columns>
                <asp:BoundField DataField="InputDate" HeaderText="InputDate" SortExpression="InputDate"
                    DataFormatString="{0:d}" meta:resourcekey="BoundFieldResource1" />
                <asp:BoundField DataField="PartCode" HeaderText="Part Code" SortExpression="PartCode"
                    meta:resourcekey="BoundFieldResource2" />
                <asp:BoundField DataField="PartName" HeaderText="Part Name" SortExpression="PartName"
                    meta:resourcekey="BoundFieldResource3" />
                <asp:BoundField DataField="OrderQuantity" HeaderText="OrderQuantity" SortExpression="OrderQuantity"
                    meta:resourcekey="BoundFieldResource4" />
                <asp:BoundField DataField="Quotation" HeaderText="Quotation" SortExpression="Quotation"
                    meta:resourcekey="BoundFieldResource5" />
                <asp:BoundField DataField="Good" HeaderText="Good" SortExpression="Good" meta:resourcekey="BoundFieldResource6" />
                <asp:BoundField DataField="Lack" HeaderText="Lack" SortExpression="Lack" meta:resourcekey="BoundFieldResource7" />
                <asp:BoundField DataField="Wrong" HeaderText="Wrong" SortExpression="Wrong" meta:resourcekey="BoundFieldResource8" />
                <asp:BoundField DataField="Broken" HeaderText="Broken" SortExpression="Broken" meta:resourcekey="BoundFieldResource9" />
                <asp:BoundField DataField="UnitPrice" HeaderText="UnitPrice" SortExpression="UnitPrice"
                    meta:resourcekey="BoundFieldResource10" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" meta:resourcekey="BoundFieldResource11"
                    ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="Comment"
                    meta:resourcekey="BoundFieldResource12" />
            </Columns>
        </vdms:PageGridView>
    </div>
    <asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetReportSource" TypeName="VDMS.II.Report.PartInputReport">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtFromDate" Name="dtFrom" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="txtToDate" Name="dtTo" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="ddlWarehouse" Name="warehouseId" PropertyName="SelectedValue"
                Type="Int64" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
