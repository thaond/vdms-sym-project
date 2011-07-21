<%@ Page Title="" Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="LookupWarrantyParts.aspx.cs" Inherits="Service_LookupWarrantyParts"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <table>
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Part Code" meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPartCode" runat="server" meta:resourcekey="txtPartCodeResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="Part Name" meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPartName" runat="server" meta:resourcekey="txtPartNameResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="Motor code" meta:resourcekey="Literal3Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtMotorCode" runat="server" meta:resourcekey="txtMotorCodeResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="Affected date" meta:resourcekey="Literal4Resource1"></asp:Literal>
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
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                        meta:resourcekey="btnSearchResource1" />
                    <asp:Button ID="btnExport" runat="server" Text="Export to Excel" OnClick="btnExport_Click"
                        meta:resourcekey="btnExportResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div class="form">
        <div class="grid">
            <vdms:PageGridView ID="gv" AllowPaging="True" AutoGenerateColumns="False" runat="server"
                CssClass="GridView" PageSize="30" DataSourceID="odsWarrantyParts" EmptyDataText="No result found."
                meta:resourcekey="gvResource1">
                <Columns>
                    <asp:BoundField DataField="PartCode" HeaderText="PartCode" SortExpression="PartCode"
                        meta:resourcekey="BoundFieldResource1" />
                    <asp:BoundField DataField="PartNameVN" HeaderText="PartNameVN" SortExpression="PartNameVN"
                        meta:resourcekey="BoundFieldResource2" />
                    <asp:BoundField DataField="PartNameEN" HeaderText="PartNameEN" SortExpression="PartNameEN"
                        meta:resourcekey="BoundFieldResource3" />
                    <asp:BoundField DataField="MotorCode" HeaderText="MotorCode" SortExpression="MotorCode"
                        meta:resourcekey="BoundFieldResource4" />
                    <asp:BoundField DataField="WarrantyTime" HeaderText="WarrantyTime" SortExpression="WarrantyTime"
                        meta:resourcekey="BoundFieldResource5" />
                    <asp:BoundField DataField="WarrantyLength" HeaderText="WarrantyLength" SortExpression="WarrantyLength"
                        meta:resourcekey="BoundFieldResource6" />
                    <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate"
                        meta:resourcekey="BoundFieldResource7" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="StopDate" HeaderText="StopDate" SortExpression="StopDate"
                        meta:resourcekey="BoundFieldResource8" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="Remark" HeaderText="Remark" meta:resourcekey="BoundFieldResource10" />
                </Columns>
            </vdms:PageGridView>
            <asp:ObjectDataSource ID="odsWarrantyParts" runat="server" OldValuesParameterFormatString="original_{0}"
                SelectMethod="FindPart" TypeName="VDMS.I.Service.WarrantyConditionDAO" EnablePaging="True"
                SelectCountMethod="CountPart">
                <SelectParameters>
                    <asp:Parameter Name="byDealerCode" DefaultValue="false" />
                    <asp:ControlParameter ControlID="txtPartCode" Name="partCode" PropertyName="Text"
                        Type="String" />
                    <asp:ControlParameter ControlID="txtPartName" Name="partName" PropertyName="Text"
                        Type="String" />
                    <asp:ControlParameter ControlID="txtMotorCode" Name="model" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txtFromDate" Name="startDate" PropertyName="Text"
                        Type="DateTime" />
                    <asp:ControlParameter ControlID="txtToDate" Name="stopDate" PropertyName="Text" Type="DateTime" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
