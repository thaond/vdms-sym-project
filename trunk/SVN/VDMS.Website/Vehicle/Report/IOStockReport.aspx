<%@ Page Title="Inventory IO report" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="IOStockReport.aspx.cs" Inherits="Vehicle_Inventory_IOStockReport"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <table>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="Report date:" meta:resourcekey="Literal2Resource1"></asp:Literal>
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
                    <asp:Literal ID="Literal1" runat="server" Text="Warehouse:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:WarehouseList Type="V" ShowSelectAllItem="True" ID="ddlWh" runat="server" DontAutoUseCurrentSealer="False"
                        meta:resourcekey="ddlWhResource1" ShowEmptyItem="False" UseVIdAsValue="False">
                    </vdms:WarehouseList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="Item type:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtType" runat="server" meta:resourcekey="txtTypeResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="Model:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtModel" runat="server" meta:resourcekey="txtModelResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnQuery" runat="server" Text="Query" OnClick="btnQuery_Click" meta:resourcekey="btnQueryResource1" />
                    <asp:Button ID="tbnExcel" runat="server" Text="Export to Excel" OnClick="tbnExcel_Click"
                        meta:resourcekey="tbnExcelResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <asp:ListView ID="lv" runat="server">
            <LayoutTemplate>
                <table class="datatable" cellpadding="0" cellspacing="0">
                    <thead>
                        <tr>
                            <th>
                                <asp:Literal ID="Literal12" runat="server" Text="No" meta:resourcekey="Literal12Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal1" runat="server" Text="Item type" meta:resourcekey="Literal1Resource2"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal5" runat="server" Text="Color" meta:resourcekey="Literal5Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal6" runat="server" Text="Item Name" meta:resourcekey="Literal6Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal7" runat="server" Text="Begin" meta:resourcekey="Literal7Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal8" runat="server" Text="Order" meta:resourcekey="Literal8Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal9" runat="server" Text="In" meta:resourcekey="Literal9Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal10" runat="server" Text="Out" meta:resourcekey="Literal10Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal11" runat="server" Text="Balance" meta:resourcekey="Literal11Resource1"></asp:Literal>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td class="pager" colspan="9">
                                <vdms:DataPager runat="server" ID="partPager" PagedControlID="lv">
                                </vdms:DataPager>
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class="group">
                    <td colspan="4" align="left">
                        <%#Eval("Code")%>
                    </td>
                    <td class="number">
                        <%# EvalBranchSum((string)Eval("Code"), "BG")%>
                    </td>
                    <td class="number">
                        <%#EvalBranchSum((string)Eval("Code"), "OR")%>
                    </td>
                    <td class="number">
                        <%#EvalBranchSum((string)Eval("Code"), "IN")%>
                    </td>
                    <td class="number">
                        <%#EvalBranchSum((string)Eval("Code"), "OT")%>
                    </td>
                    <td class="number">
                        <%#EvalBranchSum((string)Eval("Code"), "ED")%>
                    </td>
                </tr>
                <asp:ListView ID="lvType" runat="server" DataSource='<%# EvalTypes((string)Eval("Code")) %>'>
                    <LayoutTemplate>
                        <tr runat="server" id="itemPlaceholder" />
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class="selected">
                            <td class="center">
                                <%# Container.DisplayIndex + 1 %>
                            </td>
                            <td>
                                <%#Eval("ItemType")%>
                            </td>
                            <td>
                                <%#Eval("Color")%>
                            </td>
                            <td>
                                <%#Eval("ItemName")%>
                            </td>
                            <td class="number">
                                <%#Eval("Begin")%>
                            </td>
                            <td class="number">
                                <%#Eval("Order")%>
                            </td>
                            <td class="number">
                                <%#Eval("In")%>
                            </td>
                            <td class="number">
                                <%#Eval("Out")%>
                            </td>
                            <td class="number">
                                <%#Eval("Balance")%>
                            </td>
                        </tr>
                        <asp:ListView ID="lvItems" runat="server" DataSource='<%# EvalItems((string)Eval("ItemType"),(string)Eval("BranchCode")) %>'>
                            <LayoutTemplate>
                                <tr runat="server" id="itemPlaceholder" />
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
                                    <td class="center">
                                        <%# Container.DisplayIndex + 1 %>
                                    </td>
                                    <td>
                                        <%#Eval("ItemCode")%>
                                    </td>
                                    <td>
                                        <%#Eval("Color")%>
                                    </td>
                                    <td>
                                        <%#Eval("ItemName")%>
                                    </td>
                                    <td class="number">
                                        <%#Eval("Begin")%>
                                    </td>
                                    <td class="number">
                                        <%#Eval("Order")%>
                                    </td>
                                    <td class="number">
                                        <%#Eval("In")%>
                                    </td>
                                    <td class="number">
                                        <%#Eval("Out")%>
                                    </td>
                                    <td class="number">
                                        <%#Eval("Balance")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsWH" runat="server" SelectMethod="SearchWithCodeForReport" EnablePaging="True"
            SelectCountMethod="CountWithCode" TypeName="VDMS.II.BasicData.WarehouseDAO" 
                    onselecting="odsWH_Selecting">
            <SelectParameters>
                <asp:Parameter Name="DealerCode" Type="String" />
                <asp:ControlParameter ControlID="ddlWh" Name="code" PropertyName="SelectedValue"
                    Type="String" />
                <asp:Parameter DefaultValue="V" Name="type" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
