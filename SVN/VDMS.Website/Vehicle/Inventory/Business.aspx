<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true" CodeFile="Business.aspx.cs"
    Inherits="Sales_Inventory_Business" Title="Confirm order by sales" Culture="auto"
    UICulture="auto" meta:resourcekey="PageResource1" Theme="Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
    <link id="livebookmark" rel="alternate" href="/OrderRss.ashx" title="Last Order"
        type="application/rss+xml" />

    <script language="javascript" type="text/javascript">
		function print(o) {
			window.location = "/Vehicle/Report/PrintOrder.aspx?oid=" + o;
		}
		function process(o) {
			var s = "/Vehicle/Inventory/ProcessOrder.aspx?OrderId=" + o;
			s = s + "&oqDF=" + $('#<%= this.txtFromDate.ClientID %>').val();
			s = s + "&oqDT=" + $('#<%= this.txtToDate.ClientID %>').val();
			s = s + "&oqDL=" + $('#<%= this.txtDealer.ClientID %>').val();
			s = s + "&oqON=" + $('#<%= this.txtOrderNumber.ClientID %>').val();
			s = s + "&oqAI=" + $('#<%= this.ddlArea.ClientID %>').val();
			s = s + "&oqSI=" + $('#<%= this.ddlStatus.ClientID %>').val();
			window.location = s;
		}
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form" style="width: 450px">
        <table>
            <tr>
                <td valign="top">
                    <asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
                    <asp:Localize ID="litOrderDate" Text="Order date:" runat="server" meta:resourcekey="litOrderDateResource1"></asp:Localize>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtFromDate" runat="server" Width="88px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="Validator"
                        SetFocusOnError="True" ValidationGroup="Save" ControlToValidate="txtFromDate"
                        meta:resourcekey="rfvFromDateResource1" Text="*"></asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                        CultureTimePlaceholder="" Enabled="True" />
                    <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtFromDate"
                        PopupButtonID="ibFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtToDate" runat="server" Width="88px" meta:resourcekey="txtToDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibToDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibToDateResource1" />
                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="Validator" SetFocusOnError="True"
                        ValidationGroup="Save" ControlToValidate="txtToDate" meta:resourcekey="rfvToDateResource1"
                        Text="*"></asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="meeToDate" runat="server" TargetControlID="txtToDate"
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                        CultureTimePlaceholder="" Enabled="True" />
                    <ajaxToolkit:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="ibToDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Image ID="image1" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image1Resource1" />
                    <asp:Localize ID="litDealer" runat="server" Text="Dealer:" meta:resourcekey="litDealerResource1"></asp:Localize>
                </td>
                <td valign="top" colspan="2">
                    <asp:TextBox ID="txtDealer" runat="server" MaxLength="30" meta:resourcekey="txtDealerResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Image ID="image2" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image2Resource1" />
                    <asp:Localize ID="litArea" runat="server" Text="Area:" meta:resourcekey="litAreaResource1"></asp:Localize>
                </td>
                <td valign="top" colspan="2">
                    <asp:DropDownList ID="ddlArea" runat="server" DataSourceID="odsArea" DataTextField="AreaName"
                        DataValueField="AreaCode" AppendDataBoundItems="True" meta:resourcekey="ddlAreaResource1"
                        OnDataBound="ddlArea_DataBound">
                        <asp:ListItem Selected="True" Text="All" meta:resourcekey="ListItemResource1"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ObjectDataSource ID="odsArea" runat="server" TypeName="VDMS.Data.TipTop.Area"
                        SelectMethod="GetListArea"></asp:ObjectDataSource>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Image ID="image4" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image2Resource1" />
                    <asp:Localize ID="litOrderNumber" runat="server" Text="Order number:" meta:resourcekey="litOrderNumberResource1"></asp:Localize>
                </td>
                <td valign="top" colspan="2">
                    <asp:TextBox ID="txtOrderNumber" runat="server" meta:resourcekey="txtOrderNumberResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Image ID="image3" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image3Resource1" />
                    <asp:Localize ID="litStatus" runat="server" Text="Status:" meta:resourcekey="litStatusResource1"></asp:Localize>
                </td>
                <td valign="top" colspan="2">
                    <asp:DropDownList ID="ddlStatus" runat="server" meta:resourcekey="ddlStatusResource1">
                        <asp:ListItem Text="All" Value="1;2;4" meta:resourcekey="ListItemResource5"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="Sent, but not processed by VMEP" Value="1" meta:resourcekey="ListItemResource2"></asp:ListItem>
                        <asp:ListItem Text="Confirmed by VMEP" Value="2" meta:resourcekey="ListItemResource3"></asp:ListItem>
                        <asp:ListItem Text="Sent, being processed by VMEP" Value="4" meta:resourcekey="ListItemResource4"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top">
                </td>
                <td valign="top" colspan="2">
                    <asp:Button ID="btnSubmit" runat="server" Text="<%$ Resources:CommandName, Search %>"
                        ValidationGroup="Save" SkinID="SubmitButton" OnClick="btnSubmit_Click" meta:resourcekey="btnSubmitResource1" />
                    <br />
                    <div style="float: right; padding-top: 2px;">
                        <asp:HyperLink ID="RssHyperLink1" runat="server" Text="RSS" NavigateUrl="~/OrderRss.ashx"
                            meta:resourcekey="RssHyperLink1Resource1"></asp:HyperLink>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <%--<vdms:PageGridView ID="gvMain" runat="server" AutoGenerateColumns="False" AllowPaging="true"
			OnPageIndexChanging="gvMain_PageIndexChanging" meta:resourcekey="gvMainResource1">
			<Columns>
				<asp:BoundField DataField="ITEMCODE" HeaderText="Item code" meta:resourcekey="BoundFieldResource1" />
				<asp:BoundField DataField="COLORNAME" HeaderText="Color name" meta:resourcekey="BoundFieldResource2" />
				<asp:BoundField DataField="ORDERQTY" HeaderText="Quantity" meta:resourcekey="BoundFieldResource3"
					ItemStyle-HorizontalAlign="Right" />
				<asp:BoundField DataField="UNITPRICE" HeaderText="Unit price" DataFormatString="{0:n0}"
					HtmlEncode="False" meta:resourcekey="BoundFieldResource4">
					<ItemStyle HorizontalAlign="Right" />
				</asp:BoundField>
				<asp:BoundField DataField="ITEMTOTALPRICE" HeaderText="Total" DataFormatString="{0:n0}"
					HtmlEncode="False" meta:resourcekey="BoundFieldResource5">
					<ItemStyle HorizontalAlign="Right" />
				</asp:BoundField>
			</Columns>
			<EmptyDataTemplate>
				<b>
					<asp:Localize ID="litEmpty" runat="server" Text="No order found! Please change the condition search."
						meta:resourcekey="litEmptyResource1"></asp:Localize></b>
			</EmptyDataTemplate>
		</vdms:PageGridView>--%>
        <asp:ListView ID="lv" runat="server" DataSourceID="ods1">
            <LayoutTemplate>
                <div id="grid" class="grid">
                    <div class="title">
                        <asp:Literal ID="Literal14" Text="VDMS's Business Confirm Order" runat="server" meta:resourcekey="Literal14Resource1"></asp:Literal>
                    </div>
                    <table class="datatable" cellpadding="0" cellspacing="0">
                        <thead>
                            <tr>
                                <th>
                                    <asp:Literal ID="Literal1" Text="Item Code" runat="server" meta:resourcekey="Literal1Resource3"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal2" Text="Color Name" runat="server" meta:resourcekey="Literal2Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal3" Text="Quantity" runat="server" meta:resourcekey="Literal3Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal4" Text="Unit Price" runat="server" meta:resourcekey="Literal4Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal5" Text="Total" runat="server" meta:resourcekey="Literal5Resource1"></asp:Literal>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </tbody>
                    </table>
                    <div class="pager">
                        <vdms:DataPager ID="dp" runat="server" PagedControlID="lv">
                        </vdms:DataPager>
                    </div>
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <%-- class="group"--%>
                    <td colspan="5" align="left">
                        <table style="width: 100%" class="info">
                            <%--<tr>
                                <th>
                                    <asp:Literal ID="Literal1" Text="Dealer Code:" runat="server" meta:resourcekey="Literal1Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%# Eval("DealerCode")%>
                                </td>
                                <th>
                                    <asp:Literal ID="Literal6" Text="Order Date:" runat="server" meta:resourcekey="Literal6Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%#Eval("OrderDate", "{0:d}")%>
                                </td>
                                <th>
                                    <asp:Literal ID="Literal7" Text="Status:" runat="server" meta:resourcekey="Literal7Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%# GetStatus((int)Eval("Status"))%>
                                </td>
                                <th>
                                </th>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Literal ID="Literal8" Text="Dealer Name:" runat="server" meta:resourcekey="Literal8Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%# VDMS.Helper.DealerHelper.GetNameI((string)Eval("DealerCode"))%>
                                </td>
                                <th>
                                    <asp:Literal ID="Literal9" Text="Order Times:" runat="server" meta:resourcekey="Literal9Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%#Eval("OrderTimes")%>
                                </td>
                                <th>
                                    <asp:Literal ID="Literal10" Text="Order Number:" runat="server" meta:resourcekey="Literal10Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%#Eval("OrderNumber")%>
                                </td>
                                <th>
                                </th>
                                <td>
                                    <input type="button" value='<asp:Localize runat="server" Text="Process" ID="Localize1" meta:resourcekey="Localize1Resource1" />'
                                        onclick='process(<%# Eval("OrderHeaderId") %>)' />
                                    <input type="button" value='<asp:Localize runat="server" Text="Print" ID="Localize2" meta:resourcekey="Localize2Resource1" />'
                                        onclick='print(<%# Eval("OrderHeaderId") %>)' />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Literal ID="Literal11" Text="Shipping Address:" runat="server" meta:resourcekey="Literal11Resource1"></asp:Literal>
                                </th>
                                <td colspan="7">
                                    <%# string.IsNullOrEmpty((string)Eval("SecondaryShippingTo")) ? Eval("ShippingTo") : Eval("SecondaryShippingTo")%>
                                </td>
                            </tr>
                           <tr>
                                <th>
                                    <asp:Literal ID="Literal12" Text="Dealer Comment:" runat="server" meta:resourcekey="Literal12Resource1"></asp:Literal>
                                </th>
                                <td colspan="7">
                                    <%#Eval("DealerComment")%>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                    <a href="javascript:process(<%#Eval("OrderHeaderId")%>)"><span style="color: Maroon;
                                        font-weight: bold;">
                                        <%#Eval("OrderHeaderId")%></span> | <span style="color: Red;">
                                            <%#VDMS.Helper.DealerHelper.GetNameI((string)Eval("ShippingTo"))%>(<%#Eval("ShippingTo")%>)</span>
                                        | <span style="color: Red;">
                                            <%#Eval("OrderDate", "{0:d}")%></span> | <span style="color: Red;">
                                                <%#Eval("OrderTimes")%></span> (<%# GetStatus((int)Eval("Status"))%>)</a>
                                    | <span style="color: Maroon; font-weight: bold;">
                                        <%#Eval("OrderNumber")%></span> | <a href="../Report/PrintOrder.aspx?oid=<%#Eval("OrderHeaderId") %>"
                                            target="_blank">
                                            <img alt="print" src="../../Images/Print.gif" border="0" style="vertical-align: middle;" /></a>
                                    <br />
                                    <span style="color: Green;">
                                        <%# string.IsNullOrEmpty((string)Eval("SecondaryShippingTo")) ? Eval("ShippingTo") : Eval("SecondaryShippingTo")%></span>
                                    <br />
                                    <span style="color: Blue;">
                                        <%#Eval("DealerComment")%></span>
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
                        <tr class='<%# Container.DisplayIndex % 2 == 0 ? "odd" : "even" %>'>
                            <td>
                                <%#Eval("ItemCode")%>
                            </td>
                            <td>
                                <%#Eval("ColorName")%>
                            </td>
                            <td class="number">
                                <%#Eval("OrderQty")%>
                            </td>
                            <td class="number">
                                <%#Eval("UnitPrice")%>
                            </td>
                            <td class="number">
                                <%#Eval("Amount")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <tr class="group end">
                    <td colspan="2" align="right">
                        <asp:Literal ID="Literal13" Text="Total:" runat="server" meta:resourcekey="Literal13Resource1"></asp:Literal>
                    </td>
                    <td>
                        <%# Eval("TotalQuantity", "{0:N0}")%>
                    </td>
                    <td>
                    </td>
                    <td>
                        <%# Eval("SubTotal", "{0:C0}")%>
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                <asp:Literal ID="Literal1" Text="There are not any order here." runat="server" meta:resourcekey="Literal1Resource2"></asp:Literal>
            </EmptyDataTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="Select" TypeName="VDMS.I.Vehicle.OrderDAO"
            EnablePaging="True" SelectCountMethod="SelectCount">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtFromDate" Name="sFromDate" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtToDate" Name="sToDate" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="txtDealer" Name="DealerCode" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="ddlArea" Name="AreaCode" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="ddlStatus" Name="StatusCode" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="txtOrderNumber" Name="orderNumber" PropertyName="Text"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
