<%@ Page Title="In Short Query" Language="C#" MasterPageFile="~/MP/Mobile.master" Theme="Mobile"
    AutoEventWireup="true" CodeFile="InShort.aspx.cs" Inherits="MPart_Inventory_InShort"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <asp:ValidationSummary ValidationGroup="Query" CssClass="error" ID="ValidationSummary1"
            runat="server" meta:resourcekey="ValidationSummary1Resource1" />
        <table width="100%">
            <tr>
                <td>
                    <asp:Localize ID="litOrderDate" runat="server" Text="Order Date:" meta:resourcekey="litOrderDateResource1"></asp:Localize>
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" Width="40%" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="Validator"
                        Text="*" SetFocusOnError="True" ValidationGroup="Query" ControlToValidate="txtFromDate"
                        meta:resourcekey="rfvFromDateResource1"></asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                        CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="txtFromDate" Enabled="true" InvalidValueMessage="*" ControlExtender="meeFromDate"></ajaxToolkit:MaskedEditValidator>
                    ~
                    <asp:TextBox ID="txtToDate" runat="server" Width="40%" meta:resourcekey="txtToDateResource1"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="Validator" Text="*"
                        SetFocusOnError="True" ValidationGroup="Query" ControlToValidate="txtToDate" meta:resourcekey="rfvToDateResource1"></asp:RequiredFieldValidator>--%>
                    <ajaxToolkit:MaskedEditExtender ID="meeToDate" runat="server" TargetControlID="txtToDate"
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                        CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlToValidate="txtToDate" Enabled="true" InvalidValueMessage="*" ControlExtender="meeToDate"></ajaxToolkit:MaskedEditValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Localize ID="litOrderNumber" runat="server" Text="Order Number:" meta:resourcekey="litOrderNumberResource1"></asp:Localize>
                </td>
                <td>
                    <asp:TextBox ID="txtOrderNumber" runat="server" MaxLength="30" Width="80%" meta:resourcekey="txtOrderNumberResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="cmdQuery" ValidationGroup="Query" runat="server" Text="Query" OnClick="cmdQuery_Click" Width="40%"
                        meta:resourcekey="cmdQueryResource1" />
                    <%--<asp:Button ID="cmd2Excel" ValidationGroup="Query" runat="server" Text="Export to Excel"
                        OnClick="cmd2Excel_Click" meta:resourcekey="cmd2ExcelResource1" />--%>
                </td>
            </tr>
        </table>
    </div>
    <div style="clear: both;">
    </div>
    <br />
    <asp:ListView ID="lv" runat="server">
        <LayoutTemplate>
            <div id="grid" class="grid">
                <div class="title">
                    <asp:Literal ID="Literal1" runat="server" Text="In Short Supply Query Result" meta:resourcekey="Literal1Resource2"></asp:Literal>
                </div>
                <table class="datatable" cellpadding="0" cellspacing="0">
                    <thead>
                        <tr>
                            <th>
                                <asp:Literal ID="Literal2" runat="server" Text="Seq" meta:resourcekey="Literal2Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal3" runat="server" Text="Part No" meta:resourcekey="Literal3Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal4" runat="server" Text="English Name" meta:resourcekey="Literal4Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal5" runat="server" Text="Vietnamese Name" meta:resourcekey="Literal5Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal6" runat="server" Text="Order Quantity" meta:resourcekey="Literal6Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal7" runat="server" Text="Quotation Quantity" meta:resourcekey="Literal7Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal8" runat="server" Text="Short Quantity" meta:resourcekey="Literal8Resource1"></asp:Literal>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="7">
                                <vdms:DataPager PagedControlID="lv" ID="DataPager1" runat="server" />
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <tr class="group">
                <td colspan="7" align="left">
                    <asp:Literal ID="Literal1" runat="server" Text="Order Number:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                    <%#Eval("TipTopNumber")%>
                    |
                    <asp:Literal ID="Literal111" runat="server" Text="Order Date:" meta:resourcekey="Literal111Resource1"></asp:Literal>
                    <%#Eval("OrderDate")%>
                </td>
            </tr>
            <asp:ListView ID="lvItems" runat="server" DataSource='<%# EvalShort(Eval("OrderDetails")) %>'>
                <LayoutTemplate>
                    <tr runat="server" id="itemPlaceholder" />
                </LayoutTemplate>
                <ItemTemplate>
                    <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
                        <td class="center">
                            <%#Container.DisplayIndex + 1%>
                        </td>
                        <td>
                            <%#Eval("PartCode")%>
                        </td>
                        <td>
                            <%#Eval("EnglishName")%>
                        </td>
                        <td>
                            <%#Eval("VietnamName")%>
                        </td>
                        <td class="number">
                            <%#Eval("OrderQuantity")%>
                        </td>
                        <td class="number">
                            <%#Eval("QuotationQuantity")%>
                        </td>
                        <td class="number">
                            <%#Eval("ShortQuantity")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </ItemTemplate>
    </asp:ListView>
    <asp:ObjectDataSource ID="odsShortOrder" runat="server" SelectCountMethod="CountInShortOrder"
        EnablePaging="True" SelectMethod="FindInShortOrder" TypeName="VDMS.II.Report.InShortOrderDAO">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtOrderNumber" Name="orderNumber" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="txtFromDate" Name="dateFrom" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="txtToDate" Name="dateTo" PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
