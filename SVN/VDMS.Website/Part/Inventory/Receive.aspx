<%@ Page Title="Order List and Receive" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="Receive.aspx.cs" Inherits="Part_Inventory_Receive"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register TagPrefix="cc1" Namespace="VDMS.II.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">

    <script language="javascript" type="text/javascript">
        function go(o) {
            if (OrderNo[o] != 0)
                window.location = "InStock.aspx?id=" + OrderNo[o];
        }
        function goEdit(o) {
            window.location = "InStockEdit.aspx?id=" + o;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form" style="width: 50%; float: left;">
        <table width="100%">
            <tr>
                <td>
                    <asp:Localize ID="litOrderDate" runat="server" Text="Order Date:" meta:resourcekey="litOrderDateResource1"></asp:Localize>
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" Width="88px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="Validator"
                        Text="*" SetFocusOnError="True" ValidationGroup="Save" ControlToValidate="txtFromDate"
                        meta:resourcekey="rfvFromDateResource1"></asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="MDY" CultureDatePlaceholder="/" CultureDecimalPlaceholder="."
                        CultureThousandsPlaceholder="," CultureTimePlaceholder=":" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtFromDate"
                        PopupButtonID="ibFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    ~
                    <asp:TextBox ID="txtToDate" runat="server" Width="88px" meta:resourcekey="txtToDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibToDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibToDateResource1" />
                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="Validator" Text="*"
                        SetFocusOnError="True" ValidationGroup="Save" ControlToValidate="txtToDate" meta:resourcekey="rfvToDateResource1"></asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="meeToDate" runat="server" TargetControlID="txtToDate"
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="MDY" CultureDatePlaceholder="/" CultureDecimalPlaceholder="."
                        CultureThousandsPlaceholder="," CultureTimePlaceholder=":" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="ibToDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <cc1:DealerPlaceHolder ID="dph1" runat="server" VisibleBy="VMEP" AdminOnly="False">
                <tr>
                    <td>
                        <asp:Localize ID="litDealer" runat="server" Text="Dealer:" meta:resourcekey="litDealerResource1"></asp:Localize>
                    </td>
                    <td>
                        <cc1:DealerList ID="dlDealer" runat="server" RemoveRootItem="true" meta:resourcekey="dlDealerResource1">
                        </cc1:DealerList>
                    </td>
                </tr>
            </cc1:DealerPlaceHolder>
            <tr>
                <td>
                    <asp:Localize ID="litOrderNo" runat="server" Text="Order Number:" meta:resourcekey="litOrderNoResource1"></asp:Localize>
                </td>
                <td>
                    <asp:TextBox ID="txtOrderNumber" runat="server" meta:resourcekey="txtOrderNumberResource1"></asp:TextBox>
                </td>
            </tr>
            <tr runat="server" visible="false">
                <td>
                    <asp:Localize ID="litReceiveNo" runat="server" Text="Receive Number:" meta:resourcekey="litReceiveNoResource1"></asp:Localize>
                </td>
                <td>
                    <asp:TextBox ID="txtReceiveNumber" runat="server" meta:resourcekey="txtReceiveNumberResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Localize ID="litStatus" runat="server" Text="Status:" meta:resourcekey="litStatusResource1"></asp:Localize>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" meta:resourcekey="ddlStatusResource1">
                        <asp:ListItem Text="All" Value="" meta:resourcekey="ListItemResource1"></asp:ListItem>
                        <asp:ListItem Text="Closed" Value="1" meta:resourcekey="ListItemResource2"></asp:ListItem>
                        <asp:ListItem Text="Not Closed" Value="0" meta:resourcekey="ListItemResource3"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="cmdQuery" runat="server" Text="Query" OnClick="cmdQuery_Click" meta:resourcekey="cmdQueryResource1" />
                    <asp:Button ID="cmd2Excel" runat="server" Text="Export to Excel" OnClick="cmd2Excel_Click"
                        meta:resourcekey="cmd2ExcelResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div class="help" style="float: right; width: 35%;">
        <ul>
            <li>
                <asp:Localize ID="lh1" runat="server" Text="Query & Display all type of Order." meta:resourcekey="lh1Resource1"></asp:Localize>
            </li>
            <li>
                <asp:Localize ID="lh2" runat="server" Text="Do instock receive parts." meta:resourcekey="lh2Resource1"></asp:Localize>
            </li>
            <li>
                <asp:Localize ID="lh3" runat="server" Text="Record the Abnormal receive case: Broken, Wrong, Lack parts."
                    meta:resourcekey="lh3Resource1"></asp:Localize>
            </li>
        </ul>
    </div>
    <div style="clear: both;">
    </div>
    <br />
    <asp:ListView ID="lv" runat="server">
        <LayoutTemplate>
            <div id="grid" class="grid">
                <div class="title">
                    <asp:Localize ID="Localize1" runat="server" Text="Order List and Receive" meta:resourcekey="Localize1Resource3"></asp:Localize>
                </div>
                <table class="datatable" cellpadding="0" cellspacing="0">
                    <thead>
                        <tr>
                            <th rowspan="2" class="double">
                                <asp:Localize ID="Localize2" runat="server" Text="Part Code" meta:resourcekey="Localize2Resource1"></asp:Localize>
                            </th>
                            <th colspan="2">
                                <asp:Localize ID="Localize3" runat="server" Text="Part Name" meta:resourcekey="Localize3Resource1"></asp:Localize>
                            </th>
                            <th colspan="5">
                                <asp:Localize ID="Localize4" runat="server" Text="Quantity" meta:resourcekey="Localize4Resource1"></asp:Localize>
                            </th>
                            <th rowspan="2" class="double">
                                <asp:Localize ID="Localize5" runat="server" Text="Unit Price" meta:resourcekey="Localize5Resource1"></asp:Localize>
                            </th>
                            <th rowspan="2" class="double">
                                <asp:Localize ID="Localize6" runat="server" Text="Amount" meta:resourcekey="Localize6Resource1"></asp:Localize>
                            </th>
                            <th rowspan="2" class="double">
                                <asp:Localize ID="Localize7" runat="server" Text="Status" meta:resourcekey="Localize7Resource1"></asp:Localize>
                            </th>
                            <th rowspan="2" class="double">
                                <asp:Localize ID="Localize8" runat="server" Text="Rmk" meta:resourcekey="Localize8Resource1"></asp:Localize>
                            </th>
                        </tr>
                        <tr>
                            <th>
                                <asp:Localize ID="Localize9" runat="server" Text="English" meta:resourcekey="Localize9Resource1"></asp:Localize>
                            </th>
                            <th>
                                <asp:Localize ID="Localize10" runat="server" Text="Vietnam" meta:resourcekey="Localize10Resource1"></asp:Localize>
                            </th>
                            <th>
                                <asp:Localize ID="Localize11" runat="server" Text="Order" meta:resourcekey="Localize11Resource1"></asp:Localize>
                            </th>
                            <th>
                                <asp:Localize ID="Localize11a" runat="server" Text="OriginalQty" meta:resourcekey="Localize11aResource1"></asp:Localize>
                            </th>
                            <th>
                                <asp:Localize ID="Localize12" runat="server" Text="Quotation" meta:resourcekey="Localize12Resource1"></asp:Localize>
                            </th>
                            <th>
                                <asp:Localize ID="Localize19" runat="server" Text="Delivery" meta:resourcekey="Localize12Resource1a"></asp:Localize>
                            </th>
                            <th>
                                <asp:Localize ID="Localize20" runat="server" Text="Remain" meta:resourcekey="Localize12Resource1b"></asp:Localize>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </tbody>
                </table>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <tr class="group">
                <td colspan="11" align="left">
                    <table style="width: 700px" class="info">
                        <tr>
                            <th>
                                <asp:Localize ID="Localize1" runat="server" Text="Order No:" meta:resourcekey="Localize1Resource1"></asp:Localize>
                            </th>
                            <td>
                                <%# Eval("Key")%>
                            </td>
                            <th>
                                <asp:Localize ID="Localize13" runat="server" Text="Order Date:" meta:resourcekey="Localize13Resource1"></asp:Localize>
                            </th>
                            <td>
                                <%# VDMS.Helper.DateTimeHelper.To24h((DateTime)Eval("OrderDate"))%>
                            </td>
                            <th>
                                <asp:Localize ID="Localize14" runat="server" Text="Quotation Date:" meta:resourcekey="Localize14Resource1"></asp:Localize>
                            </th>
                            <td>
                                <%#((DateTime?)Eval("QuotationDate")).HasValue ? ((DateTime?)Eval("QuotationDate")).Value.ToShortDateString() : string.Empty%>
                            </td>
                            <td rowspan="2" valign="bottom" runat="server" visible='<%# VDMS.Helper.UserHelper.IsDealer && ((DateTime?)Eval("ShippingDate")).HasValue %>'>
                                <input type="button" value="In Stock" onclick="go(<%# Container.DisplayIndex %>)" />
                            </td>
                            <%--<td rowspan="2" valign="bottom" runat="server" visible='<%# VDMS.Helper.UserHelper.IsDealer && VDMS.Helper.DealerHelper.GetQuotationCFStatus(VDMS.Helper.UserHelper.DealerCode) %>'>--%>
                            <td  rowspan="2" valign="bottom" runat="server" visible='<%# VDMS.Helper.UserHelper.IsDealer %>'>
                                <input type="button" value="Xác nhận báo giá" onclick="goEdit(<%# Eval("OrderHeaderId") %>)" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <asp:Localize ID="Localize15" runat="server" Text="Payment Date:" meta:resourcekey="Localize15Resource1"></asp:Localize>
                            </th>
                            <td>
                                <%#WritePaymentDate((DateTime?)Eval("QuotationDate"), (DateTime?)Eval("PaymentDate"))%>
                            </td>
                            <th>
                                <asp:Localize ID="Localize16" runat="server" Text="Delivery Date:" meta:resourcekey="Localize16Resource1"></asp:Localize>
                            </th>
                            <td>
                                <%#WriteDeliveryDate((DateTime?)Eval("PaymentDate"), (DateTime?)Eval("DeliveryDate"))%>
                            </td>
                            <th>
                                <asp:Localize ID="Localize17" runat="server" Text="In-Stock Date:" meta:resourcekey="Localize17Resource1"></asp:Localize>
                            </th>
                            <td>
                                <%#WriteReceiveDate((DateTime?)Eval("ShippingDate"), (DateTime?)Eval("ReceiveDate"))%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <asp:ListView ID="lvItems" runat="server" DataSource='<%# Eval("Items") %>'>
                <LayoutTemplate>
                    <tr runat="server" id="itemPlaceholder" />
                </LayoutTemplate>
                <ItemTemplate>
                    <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
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
                            <%# Eval("OriginalQty")%>
                        </td>
                        <td class="number">
                            <%#Eval("QuotationQuantity")%>
                        </td>
                        <td class="number">
                            <%#Eval("DelivaryQuantity")%>
                        </td>
                        <td class='number<%#EvalRemainStyle((int)Eval("QuotationQuantity"), (int?)Eval("DelivaryQuantity"), (string)Eval("HStatus"),(DateTime?)Eval("PaymentDate"),(DateTime?)Eval("DeliveryDate"))%>'>
                            <%# EvalRemain((int)Eval("QuotationQuantity"), (int?)Eval("DelivaryQuantity"))%>
                        </td>
                        <td class="number">
                            <%#string.Format("{0:C0}", Eval("UnitPrice"))%>
                        </td>
                        <td class="number">
                            <%#string.Format("{0:C0}", Eval("Amount"))%>
                        </td>
                        <td>
                            <%#Eval("Status")%>
                        </td>
                        <td>
                            <%# EvalComment((long)Eval("OrderHeaderId"), (string)Eval("DealerComment"))%>
                            <%--# string.IsNullOrEmpty((string)Eval("DealerComment")) ? "" : string.Format(@"<a href=""javascript:window.alert('{0}')"">Y</a>", Eval("DealerComment"))--%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
            <tr class="group end">
                <td colspan="8" align="right">
                    Total:
                    <asp:Localize ID="Localize18" runat="server" Text="Order List and Receive" meta:resourcekey="Localize18Resource1"></asp:Localize>
                </td>
                <td>
                    <%# Eval("Amount", "{0:C0}")%>
                </td>
                <td colspan="2">
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <asp:Localize ID="Localize1" runat="server" Text="There are not any order here."
                meta:resourcekey="Localize1Resource2"></asp:Localize>
        </EmptyDataTemplate>
    </asp:ListView>
</asp:Content>
