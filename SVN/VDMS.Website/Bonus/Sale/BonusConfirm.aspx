<%@ Page Title="Bonus confirm" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="BonusConfirm.aspx.cs" Inherits="Bonus_Sale_BonusConfirm" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <table cellpadding="2" cellspacing="2" border="0">
            <tr>
                <td>
                    <asp:Localize ID="litStatus" runat="server" Text="Order Date:" 
                        meta:resourcekey="litStatusResource1"></asp:Localize>
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
                    <asp:Localize ID="litOrderDate" runat="server" Text="Bonus status:" 
                        meta:resourcekey="litOrderDateResource1"></asp:Localize>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" 
                        meta:resourcekey="ddlStatusResource1">
                        <asp:ListItem meta:resourcekey="ListItemResource1"></asp:ListItem>
                        <asp:ListItem Value="C" meta:resourcekey="ListItemResource2">Confirmed</asp:ListItem>
                        <asp:ListItem Value="N" meta:resourcekey="ListItemResource3">Not confirm</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Localize ID="litDealerComment" runat="server" Text="Dealer code:" 
                        meta:resourcekey="litDealerCommentResource1"></asp:Localize>
                </td>
                <td>
                    <asp:TextBox ID="txtDealerCode" runat="server" 
                        meta:resourcekey="txtDealerCodeResource1"></asp:TextBox>
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
    <div id="_err" runat="server"></div>
    <div class="grid">
        <asp:ListView ID="lv" runat="server" DataSourceID="odsOrders" DataKeyNames="OrderHeaderId">
            <LayoutTemplate>
                <div id="grid" class="grid">
                    <div class="title">
                        <asp:Literal ID="Literal1" runat="server" Text="Order List" meta:resourcekey="Literal1Resource3"></asp:Literal>
                    </div>
                    <table class="datatable" cellpadding="0" cellspacing="0">
                        <thead>
                            <tr>
                                <th>
                                    <asp:Literal ID="Literal2" runat="server" Text="Item code" meta:resourcekey="Literal2Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal4" runat="server" Text="Item name" meta:resourcekey="Literal4Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal3" runat="server" Text="Color" meta:resourcekey="Literal3Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal5" runat="server" Text="Quantity" meta:resourcekey="Literal5Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal6" runat="server" Text="Unit price" meta:resourcekey="Literal6Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal7" runat="server" Text="Total" meta:resourcekey="Literal7Resource1"></asp:Literal>
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
                        <table style="width: 100%" class="info" cellpadding="0" cellspacing="0">
                            <tr>
                                <th>
                                    <asp:Literal ID="Literal1" runat="server" Text="Order No:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%# Eval("OrderNumber") %>
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
                                    <%# Eval("ShippingTo")%>
                                </td>
                                <th>
                                    <asp:Literal ID="Literal16" runat="server" Text="Bonus:" 
                                        meta:resourcekey="Literal16Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <asp:TextBox Width="100%" ID="txtBonus" Text='<%# Eval("BonusAmount") %>' ReadOnly='<%# !VDMS.I.Vehicle.OrderStatusAct.CanChangeBonusStatus((int)Eval("Status")) %>'
                                        runat="server" meta:resourcekey="txtBonusResource1"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender TargetControlID="txtBonus" FilterType="Numbers"
                                        ID="FilteredTextBoxExtender1" runat="server" Enabled="True">
                                    </ajaxToolkit:FilteredTextBoxExtender>
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
                                    <asp:CheckBox Checked='<%# (string)Eval("BonusStatus") == "C" %>' ID="chbBConfirmed"
                                        Enabled='<%# CanChangeBonus((int)Eval("Status"), (long)Eval("BonusAmount"), (string)Eval("ShippingTo"), (string)Eval("BonusStatus")) %>'
                                        OnDataBinding="chbBConfirmed_DataBinding" Text=" Confirmed" runat="server" 
                                        meta:resourcekey="chbBConfirmedResource1" />
                                    <%--<%# GetBonusStatus((string)Eval("BonusStatus"))%>--%>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" CommandArgument='<%# Eval("OrderHeaderId") %>' Enabled='<%# VDMS.I.Vehicle.OrderStatusAct.CanChangeBonusStatus((int)Eval("Status")) %>'
                                        OnClick="btnSave_Click" runat="server" Text="Save" 
                                        meta:resourcekey="btnSaveResource1" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Literal ID="Literal13" runat="server" Text="Comment:" 
                                        meta:resourcekey="Literal13Resource1"></asp:Literal>
                                </th>
                                <td colspan="3">
                                    <%# Eval("DealerComment")%>
                                </td>
                                <th>
                                    <asp:Literal ID="Literal14" runat="server" Text="VMEP comment:" 
                                        meta:resourcekey="Literal14Resource1"></asp:Literal>
                                </th>
                                <td colspan="3">
                                    <asp:TextBox Width="100%" ReadOnly='<%# !VDMS.I.Vehicle.OrderStatusAct.CanChangeBonusStatus((int)Eval("Status")) %>'
                                        ID="txtVMEPComment" Text='<%# Eval("VMEPComment") %>' runat="server" 
                                        meta:resourcekey="txtVMEPCommentResource1"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <asp:ListView ID="lvItems" runat="server" DataSource='<%# Eval("OrderDetails") %>'>
                    <LayoutTemplate>
                        <tr runat="server" id="itemPlaceholder" />
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
                            <td>
                                <%#Eval("ItemCode")%>
                            </td>
                            <td>
                                <%#Eval("ItemName")%>
                            </td>
                            <td class="number">
                                <%#Eval("Color")%>
                            </td>
                            <td class="number">
                                <%#Eval("OrderQty")%>
                            </td>
                            <td class="number">
                                <%#Eval("UnitPrice", "{0:C0}")%>
                            </td>
                            <td class="number">
                                <%#Eval("Total", "{0:C0}")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <tr class="normalSum">
                    <td colspan="3" class="number">
                        <asp:Literal ID="Literal15" runat="server" Text="Total:" 
                            meta:resourcekey="Literal15Resource1"></asp:Literal>
                    </td>
                    <td class="number">
                        <%# Eval("TotalQuantity")%>
                    </td>
                    <td class="number">
                    </td>
                    <td class="number">
                        <%# Eval("TotalAmount", "{0:C0}")%>
                    </td>
                </tr>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                <asp:Literal ID="Literal1" runat="server" Text="There are not any order here." meta:resourcekey="Literal1Resource2"></asp:Literal>
            </EmptyDataTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsOrders" runat="server" EnablePaging="True" SelectCountMethod="CountOrderBonus"
            OldValuesParameterFormatString="original_{0}" SelectMethod="SelectOrderBonus"
            TypeName="VDMS.I.Vehicle.OrderDAO">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtFrom" Name="from" PropertyName="Text" Type="DateTime" />
                <asp:ControlParameter ControlID="txtTo" Name="to" PropertyName="Text" Type="DateTime" />
                <asp:ControlParameter ControlID="ddlStatus" Name="bonusStatus" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="txtDealerCode" Name="dealer" PropertyName="Text"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
