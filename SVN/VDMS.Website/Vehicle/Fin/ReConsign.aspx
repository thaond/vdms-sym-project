<%@ Page Title="Do Re-Consign" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="ReConsign.aspx.cs" Inherits="Vehicle_Fin_ReConsign" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div id="_msg" runat="server" />
    <div class="form">
        <table>
            <tr>
                <td>
                    <asp:Literal ID="Literal5" runat="server" Text="Dealer code:" 
                        meta:resourcekey="Literal5Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtDealerCode" runat="server" 
                        meta:resourcekey="txtDealerCodeResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Payment date:" 
                        meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtFrom" runat="server" meta:resourcekey="txtFromResource1"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender PopupButtonID="imbC1" ID="txtFrom_CalendarExtender"
                        runat="server" Enabled="True" TargetControlID="txtFrom">
                    </ajaxToolkit:CalendarExtender>
                    <ajaxToolkit:MaskedEditExtender ID="txtFrom_MaskedEditExtender" runat="server" MaskType="Date"
                        Mask="99/99/9999" TargetControlID="txtFrom" CultureAMPMPlaceholder="" 
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <asp:ImageButton runat="server" SkinID="CalendarImageButton" ID="imbC1" 
                        meta:resourcekey="imbC1Resource1" />
                    ~
                    <asp:TextBox ID="txtTo" runat="server" meta:resourcekey="txtToResource1"></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="txtTo_MaskedEditExtender" runat="server" MaskType="Date"
                        Mask="99/99/9999" TargetControlID="txtTo" CultureAMPMPlaceholder="" 
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender PopupButtonID="imbC2" ID="txtTo_CalendarExtender" runat="server"
                        Enabled="True" TargetControlID="txtTo">
                    </ajaxToolkit:CalendarExtender>
                    <asp:ImageButton runat="server" SkinID="CalendarImageButton" ID="imbC2" 
                        meta:resourcekey="imbC2Resource1" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" 
                        meta:resourcekey="btnFindResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div id="_error" runat="server" />
    <br />
    <div class="grid">
        <asp:ListView ID="lv" runat="server" OnDataBound="gvPayment_DataBound">
            <LayoutTemplate>
                <table class="datatable">
                    <tr>
                        <th>
                            PaymentDate
                        </th>
                        <th>
                            Description
                        </th>
                        <th>
                            Bank
                        </th>
                        <th>
                            Transaction
                        </th>
                        <th>
                            Amount
                        </th>
                        <th>
                            Status
                        </th>
                    </tr>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                    <tr>
                        <td colspan="6">
                            <vdms:DataPager PagedControlID="lv" ID="DataPager1" runat="server">
                            </vdms:DataPager>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class='group<%# EvalWarnClass((long)Eval("TotalPaymentAmount"), (long)Eval("OrderHeader.TotalAmount")) %>'>
                    <td colspan="6">
                        <table style="width: 100%" class="info">
                            <tr>
                                <th class="fieldName">
                                    <asp:Literal ID="Literal4" runat="server" Text="Dealer code:" 
                                        meta:resourcekey="Literal4Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%#Eval("OrderHeader.DealerCode")%>
                                </td>
                                <th class="fieldName">
                                    <asp:Literal ID="Literal2" runat="server" Text="Order number:" 
                                        meta:resourcekey="Literal2Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%#Eval("OrderHeader.OrderNumber")%>
                                </td>
                                <th class="fieldName">
                                    <asp:Literal ID="Literal3" runat="server" Text="OrderAmount:" 
                                        meta:resourcekey="Literal3Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%#Eval("OrderHeader.TotalAmount", "{0:N0}")%>
                                </td>
                                <th class="fieldName">
                                    <asp:Literal ID="Literal7" runat="server" Text="Bonus:" 
                                        meta:resourcekey="Literal7Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%#Eval("OrderHeader.BonusAmount", "{0:N0}")%>
                                </td>
                                <th class="fieldName">
                                    <asp:Literal ID="Literal8" runat="server" Text="Consigned payment:" 
                                        meta:resourcekey="Literal8Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%#Eval("OrderHeader.PaymentAmount", "{0:N0}")%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <asp:ListView ID="ListView1" runat="server" 
                        DataSource='<%# Eval("SaleOrderPayments") %>'>
                        <LayoutTemplate>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
                                <td>
                                    <%# Eval("PaymentDate", "{0:d}")%>
                                </td>
                                <td>
                                    <%# Eval("Description")%>
                                </td>
                                <td>
                                    <%# Eval("ToBank")%>
                                </td>
                                <td>
                                    <%# Eval("VoucherNumber")%>
                                </td>
                                <td class="number">
                                    <%# Eval("Amount", "{0:N0}")%>
                                </td>
                                <td>
                                    <%# Eval("ConfirmStatus") %>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </tr>
                <tr class="normalSum">
                    <td colspan="4" class="number">
                        <asp:Literal ID="Literal9" runat="server" Text="Total:" 
                            meta:resourcekey="Literal9Resource1"></asp:Literal>
                    </td>
                    <td class="number">
                        <%#Eval("TotalPaymentAmount", "{0:N0}")%>
                    </td>
                    <td>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsIP" EnablePaging="True" runat="server" SelectCountMethod="CountBankPayments"
            SelectMethod="QueryOrderPayments" TypeName="VDMS.I.Vehicle.PaymentManager">
            <SelectParameters>
                <asp:Parameter DefaultValue="" Name="orderNum" Type="String" />
                <asp:ControlParameter ControlID="txtDealerCode" Name="dCode" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtFrom" Name="from" PropertyName="Text" Type="DateTime" />
                <asp:ControlParameter ControlID="txtTo" Name="to" PropertyName="Text" Type="DateTime"
                    DefaultValue="" />
                <asp:Parameter DefaultValue="BI" Name="status" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    <br />
    <div class="form">
        <asp:Button ID="btnReConsign" runat="server" Text="Re Consign" 
            OnClick="btnReConsign_Click" meta:resourcekey="btnReConsignResource1" />
    </div>
</asp:Content>
