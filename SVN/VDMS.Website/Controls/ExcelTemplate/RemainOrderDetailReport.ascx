<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RemainOrderDetailReport.ascx.cs"
    Inherits="Controls_ExcelTemplate_RemainOrderDetailReport" %>
<asp:ListView ID="lvExcel" runat="server" OnDataBound="lvExcel_DataBound">
    <LayoutTemplate>
        <table cellpadding="2" cellspacing="0" border="1">
            <tr>
                <th style="background-color: #555555; color: White">
                    Seq
                </th>
                <th style="background-color: #555555; color: White">
                    Part Code
                </th>
                <th style="background-color: #555555; color: White">
                    Part Name
                </th>
                <th style="background-color: #555555; color: White">
                    Unit
                </th>
                <th style="background-color: #555555; color: White">
                    Order Qty
                </th>
                <th style="background-color: #555555; color: White">
                    Quatation Qty
                </th>
                <th style="background-color: #555555; color: White">
                    Deliveri Qty
                </th>
                <th style="background-color: #555555; color: White">
                    Remain Qty
                </th>
                <th style="background-color: #555555; color: White">
                    Unit Price
                </th>
                <th style="background-color: #555555; color: White">
                    Remain Amount
                </th>
                <th style="background-color: #555555; color: White">
                    Remark
                </th>
            </tr>
            <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
            <tr>
                        <td colspan="11">
                            &nbsp;
                        </td>
                    </tr>
                    <tr style="background: #eee; font-weight: bold;">
                        <td colspan="4" align="right">
                            <%# Resources.Constants.GrandTotal %>
                        </td>
                        <td align="right">
                            <asp:Literal ID="Literal22" runat="server" ></asp:Literal>
                        </td>
                        <td align="right">
                            <asp:Literal ID="Literal23" runat="server" ></asp:Literal>
                        </td>
                        <td align="right">
                            <asp:Literal ID="Literal24" runat="server" ></asp:Literal>
                        </td>
                        <td align="right">
                            <asp:Literal ID="Literal25" runat="server" ></asp:Literal>
                        </td>
                        <td align="right">
                            <asp:Literal ID="Literal26" runat="server" ></asp:Literal>
                        </td>
                        <td align="right">
                            <asp:Literal ID="Literal27" runat="server" ></asp:Literal>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
        </table>
    </LayoutTemplate>
    <ItemTemplate>
        <tr style="background-color: #AAAAAA">
            <td colspan="11">
                <asp:Literal ID="Literal10" runat="server" Text='<%# Eval("DealerCodeAndName") %>'></asp:Literal>
            </td>
        </tr>
        <asp:ListView ID="lvOrder" runat="server" DataSource='<%# Eval("OrderHeaders") %>'>
            <LayoutTemplate>
                <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
            </LayoutTemplate>
            <ItemTemplate>
                <tr style="background-color: #DDDDDD">
                    <td colspan="11">
                        <table style="text-align: left; width: 100%">
                            <tr>
                                <th>
                                    <asp:Literal ID="Literal5" runat="server" Text="Address:"></asp:Literal>
                                </th>
                                <td colspan="10">
                                    <asp:Literal ID="litAddress" runat="server" Text='<%#Eval("FullOrderAddress") %>' />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Literal ID="Literals5" runat="server" Text="Order No:"></asp:Literal>
                                </th>
                                <td>
                                    <asp:Literal ID="litOrder" runat="server" Text='<%# Eval("TipTopNumber") %>' />
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <th>
                                    <asp:Literal ID="Literal8" runat="server" Text="Order date:"></asp:Literal>
                                </th>
                                <td>
                                    <asp:Literal ID="litODate" runat="server" Text='<%# Eval("OrderDate") %>' />
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <th>
                                    <asp:Literal ID="Literal9" runat="server" Text="Delivery:"></asp:Literal>
                                </th>
                                <td>
                                    <asp:Literal ID="litDelivery" runat="server" Text='<%# EvalDelivery(Eval("ReceiveHeaders")) %>' />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <asp:ListView ID="lvOrderDetail" runat="server" DataSource='<%# Eval("OrderDetails") %>'>
                    <LayoutTemplate>
                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="center">
                                <%# Container.DisplayIndex + 1%>
                            </td>
                            <td>
                                <%#Eval("PartCode") %>
                            </td>
                            <td>
                                <%#Eval("PartName")%>
                            </td>
                            <td>
                                <%#Eval("PartSpec.PackUnit")%>
                            </td>
                            <td align="right" class="text">
                                <%#Eval("OrderQuantity", "{0:N0}")%>
                            </td>
                            <td align="right" class="text">
                                <%#Eval("QuotationQuantity", "{0:N0}")%>
                            </td>
                            <td align="right" class="text">
                                <%#Eval("DelivaryQuantity", "{0:N0}")%>
                            </td>
                            <td align="right" class="text">
                                <%#Eval("RemainQuantity", "{0:N0}")%>
                            </td>
                            <td align="right" class="text">
                                <%#Eval("UnitPrice", "{0:N0}")%>
                            </td>
                            <td align="right" class="text">
                                <%#Eval("RemainAmount", "{0:N0}")%>
                            </td>
                            <td>
                                <%#Eval("Note") %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <tr style="background: #eee; font-weight: bold;">
                    <td colspan="4" align="right">
                        <%# Resources.Constants.Total %>
                    </td>
                    <td align="right" class="text">
                        <%#Eval("OrderHeaderSummary.OrderQuantity", "{0:N0}") %>
                    </td>
                    <td align="right" class="text">
                        <%#Eval("OrderHeaderSummary.QuotationQuantity", "{0:N0}")%>
                    </td>
                    <td align="right" class="text">
                        <%#Eval("OrderHeaderSummary.DelivaryQuantity", "{0:N0}")%>
                    </td>
                    <td align="right" class="text">
                        <%#Eval("OrderHeaderSummary.RemainQuantity", "{0:N0}")%>
                    </td>
                    <td align="right" class="text">
                        <%#Eval("OrderHeaderSummary.UnitPrice", "{0:N0}")%>
                    </td>
                    <td align="right" class="text">
                        <%#Eval("OrderHeaderSummary.RemainAmount", "{0:N0}")%>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <tr style="background: #eee; font-weight: bold;">
            <td colspan="4" align="right">
                <%# Resources.Constants.DealerAllTotal %>
            </td>
            <td align="right" class="text">
                <%#Eval("DealerSummary.OrderQuantity", "{0:N0}")%>
            </td>
            <td align="right" class="text">
                <%#Eval("DealerSummary.QuotationQuantity", "{0:N0}")%>
            </td>
            <td align="right" class="text">
                <%#Eval("DealerSummary.DelivaryQuantity", "{0:N0}")%>
            </td>
            <td align="right" class="text">
                <%#Eval("DealerSummary.RemainQuantity", "{0:N0}")%>
            </td>
            <td align="right" class="text">
                <%#Eval("DealerSummary.UnitPrice", "{0:N0}")%>
            </td>
            <td align="right" class="text">
                <%#Eval("DealerSummary.RemainAmount", "{0:N0}")%>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </ItemTemplate>
</asp:ListView>
