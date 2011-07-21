<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true" CodeFile="CheckOrder.aspx.cs"
    Inherits="Sales_Inventory_CheckOrder" Title="Kiểm tra đơn hàng" Culture="auto"
    UICulture="auto" meta:resourcekey="PageResource1" Theme="Default" %>

<%@ Register Src="~/Controls/UpdateProgress.ascx" TagName="UpdateProgress" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <asp:UpdatePanel ID="udpForm" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form" style="width: 450px">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Search"
                    Width="100%" meta:resourcekey="ValidationSummary1Resource1" />
                <uc:UpdateProgress ID="UpdateProgress" runat="server" />
                <table>
                    <tr>
                        <td valign="middle" nowrap="nowrap">
                            <asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
                            <asp:Localize ID="litOrderDate" runat="server" Text="Order date:" meta:resourcekey="litOrderDateResource1"></asp:Localize>
                        </td>
                        <td valign="top">
                            <asp:TextBox ID="txtFromDate" runat="server" Width="100px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                            <asp:ImageButton ID="ImageButton1" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
                                meta:resourcekey="ImageButton1Resource1" />
                            <asp:RequiredFieldValidator ID="Requiredfieldvalidator9" runat="server" CssClass="Validator"
                                SetFocusOnError="True" ValidationGroup="Search" ControlToValidate="txtFromDate"
                                ErrorMessage='Dữ liệu "Đặt hàng từ ngày" không được để trống' Text="*" meta:resourcekey="Requiredfieldvalidator9Resource1"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvFromDate" runat="server" ControlToValidate="txtFromDate"
                                Display="Dynamic" ErrorMessage="Định dạng ngày không đúng!" SetFocusOnError="True"
                                Type="Date" ValidationGroup="Search" meta:resourcekey="rvFromDateResource1">*</asp:RangeValidator>
                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtFromDate"
                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                CultureTimePlaceholder="" Enabled="True">
                            </ajaxToolkit:MaskedEditExtender>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtFromDate"
                                PopupButtonID="ImageButton1" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                            <asp:Localize ID="litTodate" runat="server" meta:resourcekey="litTodateResource1"
                                Text="~"></asp:Localize>
                            <asp:TextBox ID="txtToDate" runat="server" Width="100px" meta:resourcekey="txtToDateResource1">
                            </asp:TextBox>
                            <asp:ImageButton ID="ImageButton2" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
                                meta:resourcekey="ImageButton2Resource1" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="Validator"
                                SetFocusOnError="True" ValidationGroup="Search" ControlToValidate="txtToDate"
                                ErrorMessage='Dữ liệu "Đặt hàng đến ngày" không được để trống ' Text="*" meta:resourcekey="RequiredFieldValidator2Resource1"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvToDate" runat="server" ControlToValidate="txtToDate" Display="Dynamic"
                                ErrorMessage="Định dạng ngày không đúng!" SetFocusOnError="True" Type="Date"
                                ValidationGroup="Search" meta:resourcekey="rvToDateResource1">*</asp:RangeValidator>
                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtToDate"
                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                CultureTimePlaceholder="" Enabled="True">
                            </ajaxToolkit:MaskedEditExtender>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                                PopupButtonID="ImageButton2" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" nowrap="nowrap">
                            <asp:Image ID="image1" runat="server" SkinID="RequireField" meta:resourcekey="image1Resource1" />
                            <asp:Localize ID="litStatus" runat="server" Text="Order status:" meta:resourcekey="litStatusResource1"></asp:Localize>
                        </td>
                        <td valign="top">
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="136px" meta:resourcekey="ddlStatusResource1">
                                <asp:ListItem Text="All" Value="0" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                <asp:ListItem Text="Chưa giao hết" Value="1" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                <asp:ListItem Text="Đã giao hết" Value="2" meta:resourcekey="ListItemResource3"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" nowrap="nowrap">
                            <asp:Image ID="image2" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image2Resource1" />
                            <asp:Localize ID="litOrderNumber" runat="server" Text="Order number:" meta:resourcekey="litOrderNumberResource1"></asp:Localize>
                        </td>
                        <td valign="top">
                            <asp:TextBox ID="txtOrderNoFrom" runat="server" Width="130px" MaxLength="10" Style="text-transform: uppercase"
                                meta:resourcekey="txtOrderNo1Resource1"></asp:TextBox>
                            ~
                            <asp:TextBox ID="txtOrderNoTo" runat="server" Width="130px" MaxLength="10" Style="text-transform: uppercase"
                                meta:resourcekey="txtOrderNo1Resource1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" nowrap="nowrap">
                            <asp:Image ID="image3" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image3Resource1" />
                            <asp:Localize ID="litArea" runat="server" Text="Area:" meta:resourcekey="litAreaResource1"></asp:Localize>
                        </td>
                        <td valign="top">
                            <vdms:AreaList ID="ddlArea" ShowSelectAllItem="True" runat="server" Width="136px"
                                meta:resourcekey="ddlAreaResource1">
                            </vdms:AreaList>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" nowrap="nowrap">
                            <asp:Image ID="image4" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image4Resource1" />
                            <asp:Localize ID="litDealer" runat="server" Text="Dealer:" meta:resourcekey="litDealerResource1"></asp:Localize>
                        </td>
                        <td valign="top">
                            <%--<vdms:DealerList AutoPostBack="True" ID="ddlDealer" RootDealer="/" RemoveRootItem="True"
                                runat="server" ShowSelectAllItem="True" OnSelectedIndexChanged="ddlDealer_SelectedIndexChanged"
                                EnabledSaperateByArea="False" EnabledSaperateByDB="False" MergeCode="False" meta:resourcekey="ddlDealerResource1"
                                ShowEmptyItem="False">
                            </vdms:DealerList>--%>
                            <asp:TextBox ID="txtDealerCode" AutoPostBack="false" OnTextChanged="ddlDealer_SelectedIndexChanged"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" nowrap="nowrap">
                            <asp:Image ID="image5" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image5Resource1" />
                            <asp:Localize ID="litDealer0" runat="server" Text="Warehouse:" meta:resourcekey="litDealer0Resource1"></asp:Localize>
                        </td>
                        <td valign="top">
                            <vdms:WarehouseList ID="ddlWarehouse" Type="V" runat="server" ShowSelectAllItem="True"
                                DontAutoUseCurrentSealer="False" meta:resourcekey="ddlWarehouseResource1" ShowEmptyItem="False"
                                UseVIdAsValue="False">
                            </vdms:WarehouseList>
                            <asp:LinkButton ID="lnkUpdateW" runat="server" Text="Refresh warehouses" meta:resourcekey="lnkUpdateWResource1"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="Search"
        OnClick="btnSearch_Click" meta:resourcekey="btnSearchResource1" />
    <div class="grid">
        <asp:ListView ID="lv" runat="server">
            <LayoutTemplate>
                <div id="grid" class="grid">
                    <div class="title">
                        <asp:Literal ID="Literal1" runat="server" Text="Order List and Receive" meta:resourcekey="Literal1Resource3"></asp:Literal>
                    </div>
                    <table class="datatable" cellpadding="0" cellspacing="0">
                        <thead>
                            <tr>
                                <th>
                                    <asp:Literal ID="Literal2" runat="server" Text="Vehicle" meta:resourcekey="Literal2Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal3" runat="server" Text="Color" meta:resourcekey="Literal3Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal4" runat="server" Text="Order Quantity" meta:resourcekey="Literal4Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal5" runat="server" Text="Shipping Quantity" meta:resourcekey="Literal5Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal6" runat="server" Text="Import Quantity" meta:resourcekey="Literal6Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal7" runat="server" Text="Lack Quantity" meta:resourcekey="Literal7Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal13" runat="server" Text="Sold Quantity" meta:resourcekey="Literal13Resource1"></asp:Literal>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="6">
                                    <vdms:DataPager runat="server" ID="DataPager" PagedControlID="lv">
                                    </vdms:DataPager>
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class="group">
                    <td colspan="9" align="left">
                        <table style="width: 700px" class="info">
                            <tr>
                                <th>
                                    <asp:Literal ID="Literal1" runat="server" Text="Order No:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%--<asp:LinkButton OnDataBinding="lnkViewShipping_DataBinding" ID="lnkViewShipping"
                                        Text='<%# Eval("OrderNumber") %>' runat="server" meta:resourcekey="lnkViewShippingResource1"></asp:LinkButton>--%>
                                    <a href="ShippingIssues.aspx?on=<%#Eval("OrderNumber")%>">
                                        <%#Eval("OrderNumber")%></a> [
                                    <%# VDMS.I.Vehicle.OrderStatusAct.GetName((int)Eval("Status"))%>
                                    ]
                                </td>
                                <th>
                                    <asp:Literal ID="Literal8" runat="server" Text="Order Date:" meta:resourcekey="Literal8Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%# VDMS.Helper.DateTimeHelper.To24h((DateTime)Eval("OrderDate"))%>
                                </td>
                                <th>
                                    <asp:Literal ID="Literal9" runat="server" Text="Dealer Code:" meta:resourcekey="Literal9Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%# Eval("DealerCode")%>
                                </td>
                                <td rowspan="2">
                                    <a href='../Report/PrintOrder.aspx?oid=<%# Eval("OrderHeaderId") %>' target="_blank">
                                        <img src="../../Images/print.gif" style="border: 0" />
                                    </a>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Literal ID="Literal10" runat="server" Text="Delivery Place:" meta:resourcekey="Literal10Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%# Eval("ShippingTo")%>
                                    -
                                    <%# Eval("SecondaryShippingCode")%>
                                </td>
                                <th>
                                    <asp:Literal ID="Literal11" runat="server" Text="Order Times:" meta:resourcekey="Literal11Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%# Eval("OrderTimes")%>
                                </td>
                                <th>
                                    <asp:Literal ID="Literal12" runat="server" Text="Status:" meta:resourcekey="Literal12Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%# GetOrderStatus((int)Eval("DeliveredStatus"))%>
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
                                <%#Eval("ItemCode")%>
                            </td>
                            <td>
                            </td>
                            <td class="number">
                                <%#Eval("OrderQty")%>
                            </td>
                            <td class="number">
                                <%#Eval("ShippingQuantity")%>
                            </td>
                            <td class="number">
                                <%#Eval("ImportQuantity")%>
                            </td>
                            <td class="number" style='color: <%# EvalLackColor(Eval("OrderQty"), Eval("ShippingQuantity")) %>;'>
                                <%# EvalLack(Eval("OrderQty"), Eval("ImportQuantity"))%>
                            </td>
                            <td class="number">
                                <%#Eval("SoldQuantity")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
            <EmptyDataTemplate>
                <asp:Literal ID="Literal1" runat="server" Text="There are not any order here." meta:resourcekey="Literal1Resource2"></asp:Literal>
            </EmptyDataTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsOrders" runat="server" SelectMethod="CheckOrder" EnablePaging="True"
            TypeName="VDMS.I.ObjectDataSource.OrderHeaderDataSource" SelectCountMethod="CountCheckOrder">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtDealerCode" Name="dealerCode" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="ddlWarehouse" Name="wCode" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="txtFromDate" Name="fromDate" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtToDate" Name="toDate" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="ddlStatus" Name="status" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="txtOrderNoFrom" Name="orderNoFrom" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtOrderNoTo" Name="orderNoTo" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="ddlArea" Name="area" PropertyName="SelectedValue"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
