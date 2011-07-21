<%@ Page Title="Over shipping span report" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="ShippingSpan.aspx.cs" Inherits="Part_Report_ShippingSpan" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form" style="width: 500px;">
         <asp:ValidationSummary ID="ValidationSummary1" CssClass="error" 
             ValidationGroup="Report" runat="server" 
             meta:resourcekey="ValidationSummary1Resource1" />
       <table width="100%"> 
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Shipping date:" 
                        meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtDateFrom" runat="server" 
                        meta:resourcekey="txtDateFromResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ValidationGroup="Report" 
                        ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" ControlToValidate="txtDateFrom"
                        ErrorMessage="Report date cannot be blank!" 
                        meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                    <asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtDateFrom"
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" 
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtDateFrom"
                        PopupButtonID="ibFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td>
                    ~
                </td>
                <td>
                    <asp:TextBox ID="txtDateTo" runat="server" 
                        meta:resourcekey="txtDateToResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibToDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDateTo"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="meeFromDate" 
                        CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateTo"
                        PopupButtonID="ibToDate" BehaviorID="ceFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="Region:" 
                        meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:DatabaseList ID="ddlDBCode" runat="server" AllowDealerSelect="False" 
                        meta:resourcekey="ddlDBCodeResource1" ShowSelectAllItem="False"/>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="Status:" 
                        meta:resourcekey="Literal3Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" 
                        meta:resourcekey="ddlStatusResource1">
                        <asp:ListItem Value="" meta:resourcekey="ListItemResource1">All</asp:ListItem>
                        <asp:ListItem Value="N" meta:resourcekey="ListItemResource2">Not receive</asp:ListItem>
                        <asp:ListItem Value="R" meta:resourcekey="ListItemResource3">Received</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnSearch" ValidationGroup="Report" runat="server" Text="Search" 
                        onclick="btnSearch_Click" meta:resourcekey="btnSearchResource1" />
                    <asp:Button ID="btnExcel" runat="server" Text="Export to excel" 
                        onclick="btnExcel_Click" meta:resourcekey="btnExcelResource1" />
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div class="grid">
        <%--<asp:ListView ID="lvRpt" runat="server" DataSourceID="odsShipping">
            <LayoutTemplate>
                <table class="datatable" cellpadding="0" cellspacing="0">
                    <thead>
                        <tr>
                            <th>
                                Part No
                            </th>
                            <th>
                                Part Name
                            </th>
                            <th>
                                Description
                            </th>
                            <th>
                                Quantity
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </tbody>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class="group">
                    <td colspan="4">
                        Issue No:
                        <%# Eval("IssueNo")%>
                        | Shipping date:
                        <%# ((DateTime)Eval("ShippingDate")).ToShortDateString()%>
                        | To dealer:
                        <%# Eval("DealerCode") %>
                    </td>
                </tr>
                <asp:ListView ID="lvRptDetail" runat="server" DataSource='<%# Eval("Items") %>'>
                    <LayoutTemplate>
                        <tr runat="server" id="itemPlaceholder" />
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
                            <td>
                                <%# Eval("PartNo")%>
                            </td>
                            <td>
                                <%# Eval("PartName")%>
                            </td>
                            <td>
                                <%# Eval("Description")%>
                            </td>
                            <td class="number">
                                <%# Eval("Quantity")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:ListView>--%>
        <vdms:PageGridView ID="GridView1" runat="server" AllowPaging="True" 
            AutoGenerateColumns="False" ShowFooter="True" 
            meta:resourcekey="GridView1Resource1">
            <Columns>
                <asp:BoundField DataField="DealerCode" HeaderText="Dealer Code" 
                    meta:resourcekey="BoundFieldResource1" />
                <asp:BoundField DataField="IssueNo" HeaderText="Issue No" 
                    meta:resourcekey="BoundFieldResource2" />
                <asp:BoundField DataField="OrderNumber" HeaderText="Order Number" 
                    meta:resourcekey="BoundFieldResource3" />
                <asp:TemplateField HeaderText="Payment Date" 
                    meta:resourcekey="TemplateFieldResource1">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" 
                            Text='<%# EvalDate(Eval("PaymentDate")) %>' meta:resourcekey="Label1Resource1"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Shipping Date" 
                    meta:resourcekey="TemplateFieldResource2">
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" 
                            Text='<%# EvalDate(Eval("ShippingDate")) %>' meta:resourcekey="Label2Resource1"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ShippingSpan" HeaderText="Shipping Span" 
                    meta:resourcekey="BoundFieldResource4" />
                <%--<asp:BoundField DataField="OverDays" HeaderText="Over days" />--%>
                <asp:TemplateField HeaderText="Over days" 
                    meta:resourcekey="TemplateFieldResource3">
                    <ItemTemplate>
                        <asp:Literal ID="litOverdays" runat="server" 
                            Text='<%# EvalOverDays(Eval("ShippingSpan"), Eval("OrderDate"), Eval("ShippingDate"), Eval("PaymentDate") ) %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" 
                    meta:resourcekey="BoundFieldResource5" />
                
            </Columns>
        </vdms:PageGridView>
        <asp:ObjectDataSource ID="odsShipping" runat="server" SelectMethod="FindOverSpanOrder" 
            TypeName="VDMS.II.Report.ShippingSpanDAO" EnablePaging="True"
            onselected="odsShipping_Selected" SelectCountMethod="CountOverSpanOrder">
            <SelectParameters>
                <asp:Parameter Name="dealerCode" Type="String" />
                <asp:ControlParameter ControlID="ddlDBCode" Name="dbCode" 
                    PropertyName="SelectedValue" Type="String" />
                <asp:ControlParameter ControlID="ddlStatus" Name="status" 
                    PropertyName="SelectedValue" Type="String" />
                <asp:ControlParameter ControlID="txtDateFrom" Name="dateFrom" 
                    PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="txtDateTo" Name="dateTo" PropertyName="Text" 
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
