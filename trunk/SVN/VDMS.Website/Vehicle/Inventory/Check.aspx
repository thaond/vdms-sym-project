<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="Check.aspx.cs" Inherits="Sales_Inventory_Check" Title="Kiểm tra đơn hàng"
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form" style="width: 450px">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Search"
            Width="100%" meta:resourcekey="ValidationSummary1Resource1" />
        <span style="color: #ff0000">
            <asp:Label ID="lbErr" runat="server" meta:resourcekey="lbErrResource1"></asp:Label></span>
        <table>
            <tr>
                <td valign="middle" nowrap="nowrap">
                    <asp:Image ID="image2" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image2Resource1" />
                    <asp:Localize ID="litOrderNumber" runat="server" Text="Order number:" meta:resourcekey="litOrderNumberResource1"></asp:Localize>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtOrderNo1" runat="server" Width="130px" MaxLength="10" meta:resourcekey="txtOrderNo1Resource1"
                        Style="text-transform: uppercase"></asp:TextBox>
                </td>
                <td valign="middle">
                    <%--<asp:Localize ID="litToNo" runat="server" Text="~" meta:resourcekey="litToNoResource1"></asp:Localize>--%>&nbsp;
                </td>
                <td valign="top">
                    <%--<asp:TextBox ID="txtOrderNo2" runat="server" Width="130px" MaxLength="10" meta:resourcekey="txtOrderNo2Resource1"
						Style="text-transform: uppercase"></asp:TextBox>--%>
                </td>
            </tr>
            <tr>
                <td valign="middle" nowrap="nowrap">
                    <asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
                    <asp:Localize ID="litOrderDate" runat="server" Text="Order date:" meta:resourcekey="litOrderDateResource1"></asp:Localize>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtFromDate" runat="server" Width="100px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton1" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ImageButton1Resource1" />
                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator9" runat="server" CssClass="Validator"
                        SetFocusOnError="True" ValidationGroup="Search" ControlToValidate="txtFromDate"
                        ErrorMessage='Dữ liệu "Đặt hàng từ ngày" không được để trống' Text="*" meta:resourcekey="Requiredfieldvalidator9Resource1"></asp:RequiredFieldValidator><asp:RangeValidator
                            ID="rvFromDate" runat="server" ControlToValidate="txtFromDate" Display="Dynamic"
                            ErrorMessage="Định dạng ngày không đúng!" meta:resourcekey="rvFromDateResource1"
                            SetFocusOnError="True" Type="Date" ValidationGroup="Search">*</asp:RangeValidator>
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtFromDate"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="MaskedEditExtender2">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtFromDate"
                        PopupButtonID="ImageButton1" BehaviorID="CalendarExtender3" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td valign="middle" nowrap="nowrap">
                    <asp:Localize ID="litTodate" runat="server" Text="~" meta:resourcekey="litTodateResource1"></asp:Localize>&nbsp;
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtToDate" runat="server" Width="100px" meta:resourcekey="txtToDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton2" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ImageButton2Resource1" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="Validator"
                        SetFocusOnError="True" ValidationGroup="Search" ControlToValidate="txtToDate"
                        ErrorMessage='Dữ liệu "Đặt hàng đến ngày" không được để trống ' Text="*" meta:resourcekey="RequiredFieldValidator2Resource1"></asp:RequiredFieldValidator><asp:RangeValidator
                            ID="rvToDate" runat="server" ControlToValidate="txtToDate" Display="Dynamic"
                            ErrorMessage="Định dạng ngày không đúng!" meta:resourcekey="rvToDateResource1"
                            SetFocusOnError="True" Type="Date" ValidationGroup="Search">*</asp:RangeValidator>
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtToDate"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="MaskedEditExtender1">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="ImageButton2" BehaviorID="CalendarExtender1" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td valign="middle" nowrap="nowrap">
                    <asp:Image ID="image3" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image3Resource1" />
                    <asp:Localize ID="litDealer" runat="server" Text="Dealer:" meta:resourcekey="litDealerResource1"></asp:Localize>
                </td>
                <td valign="top" colspan="3">
                    <asp:TextBox ID="txtDealer" runat="server" Width="130px" MaxLength="7" meta:resourcekey="txtDealerResource1"
                        Style="text-transform: uppercase"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="middle" nowrap="nowrap">
                    <asp:Image ID="image4" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image4Resource1" />
                    <asp:Localize ID="litArea" runat="server" Text="Area" meta:resourcekey="litAreaResource1"></asp:Localize>
                </td>
                <td valign="top" colspan="3">
                    <%--<asp:DropDownList ID="ddlArea" runat="server" Width="136px" AppendDataBoundItems="True"
						meta:resourcekey="ddlAreaResource1">
						<asp:ListItem Value="AllItems" meta:resourcekey="ListItemResource1">All</asp:ListItem>
					</asp:DropDownList>--%>
                    <vdms:AreaList ID="ddlArea" ShowSelectAllItem="true" runat="server" Width="136px">
                    </vdms:AreaList>
                </td>
            </tr>
            <tr>
                <td valign="middle" nowrap="nowrap">
                    <asp:Image ID="image1" runat="server" SkinID="RequireField" meta:resourcekey="image1Resource1" />
                    <asp:Localize ID="litStatus" runat="server" Text="Order status:" meta:resourcekey="litStatusResource1"></asp:Localize>
                </td>
                <td valign="top" colspan="3">
                    <asp:DropDownList ID="ddlStatus" runat="server" Width="136px" meta:resourcekey="ddlStatusResource1">
                        <asp:ListItem Text="All" Value="0" meta:resourcekey="ListItemResource2"></asp:ListItem>
                        <asp:ListItem Text="Chưa giao hết" Value="1" meta:resourcekey="ListItemResource3"></asp:ListItem>
                        <asp:ListItem Text="Đã giao hết" Value="2" meta:resourcekey="ListItemResource4"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top">
                </td>
                <td valign="top" colspan="3" align="right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="Search"
                        OnClick="btnSearch_Click" meta:resourcekey="btnSearchResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <%--<asp:GridView ID="gvModelHistory" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvModelHistory_RowDataBound"
            Width="100%" AllowSorting="True" meta:resourcekey="gvModelHistoryResource1" OnDataBound="gvModelHistory_DataBound"
            AllowPaging="True" OnPageIndexChanging="gvModelHistory_PageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText="Order number" meta:resourcekey="TemplateFieldResource1">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderNumber" runat="server" Text='<%# Bind("ORDERNUMBER") %>' ToolTip='<%# Bind(Container, "ID") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Dealer" meta:resourcekey="TemplateFieldResource2">
                    <ItemTemplate>
                        <asp:Label ID="lbDEALERCODE" runat="server" Text='<%# Bind("DEALERCODE") %>' meta:resourcekey="lbDEALERCODEResource1"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Order date" meta:resourcekey="TemplateFieldResource3">
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# ReturnShortDate(DataBinder.Eval(Container, "DataItem.ORDERDATE").ToString()) %>'
                            meta:resourcekey="Label2Resource1"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="ORDERTIMES" HeaderText="Position" meta:resourcekey="BoundFieldResource1" />
                <asp:BoundField DataField="SHIPPINGTO" HeaderText="Delivered Place" meta:resourcekey="BoundFieldResource2" />
                <asp:TemplateField HeaderText="Items" ShowHeader="False" meta:resourcekey="TemplateFieldResource4">
                    <ItemTemplate>
                        <asp:GridView ID="gvItems" runat="server" Width="100%" AutoGenerateColumns="False"
                            OnRowDataBound="gvItems_RowDataBound" ShowHeader="False" GridLines="Horizontal"
                            HorizontalAlign="Center" PageSize="100" meta:resourcekey="gvItemsResource1">
                            <Columns>
                                <asp:TemplateField HeaderText="Loại xe" meta:resourcekey="TemplateFieldResource5">
                                    <ItemTemplate>
                                        <asp:Label ID="lbITEMNAME" runat="server" Text='<%# Bind("itemcode") %>' Width="65px"
                                            meta:resourcekey="lbITEMNAMEResource1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="M&#224;u sắc" meta:resourcekey="TemplateFieldResource6">
                                    <ItemTemplate>
                                        <asp:Label ID="lbModalColor" runat="server" ToolTip='<%# Eval("itemcode") %>' Width="70px"
                                            meta:resourcekey="lbModalColorResource1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Số lượng đặt h&#224;ng" meta:resourcekey="TemplateFieldResource7">
                                    <ItemTemplate>
                                        <asp:Label ID="lbOrderQty" runat="server" Text='<%# Bind("orderqty") %>' Width="64px"
                                            meta:resourcekey="lbOrderQtyResource1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Số lượng đ&#227; giao" meta:resourcekey="TemplateFieldResource8">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTranferedItem" runat="server" Width="64px" meta:resourcekey="lblTranferedItemResource1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Số lượng nhập xe" meta:resourcekey="TemplateFieldResource9">
                                    <ItemTemplate>
                                        <asp:Label ID="lblImportedItemNumber" runat="server" Width="64px" meta:resourcekey="lblImportedItemNumberResource1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Số xe thiếu" ShowHeader="False" meta:resourcekey="TemplateFieldResource10">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRestOfItemNumber" runat="server" Width="64px" meta:resourcekey="lblRestOfItemNumberResource1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="False" meta:resourcekey="TemplateFieldResource11">
                                    <ItemTemplate>
                                        <asp:Label ID="lbITEMCODE" runat="server" Text='<%# Eval("itemcode") %>' meta:resourcekey="lbITEMCODEResource1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle HorizontalAlign="Center" />
                            <PagerSettings Mode="NextPreviousFirstLast" />
                        </asp:GridView>
                    </ItemTemplate>
                    <HeaderTemplate>
                        <table border="1" cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr>
                                <th nowrap="nowrap" style="width: 62px">
                                    <asp:Localize ID="liModalType" runat="server" Text="Model" meta:resourcekey="liModalTypeResource1"></asp:Localize>
                                </th>
                                <th nowrap="nowrap" style="width: 64px">
                                    <asp:Localize ID="Localize1" runat="server" Text="Color" meta:resourcekey="Localize1Resource1"></asp:Localize>
                                </th>
                                <th nowrap="nowrap" style="width: 60px">
                                    <asp:Localize ID="Localize4" runat="server" Text="Quantity" meta:resourcekey="Localize4Resource1"></asp:Localize>
                                </th>
                                <th nowrap="nowrap" style="width: 60px">
                                    <asp:Localize ID="Localize5" runat="server" Text="Delivered Quantity" meta:resourcekey="Localize5Resource1"></asp:Localize>
                                </th>
                                <th nowrap="nowrap" style="width: 65px">
                                    <asp:Localize ID="Localize6" runat="server" Text="Imported Quantity" meta:resourcekey="Localize6Resource1"></asp:Localize>
                                </th>
                                <th nowrap="nowrap" style="width: 45px">
                                    <asp:Localize ID="Localize7" runat="server" Text="Deprived Quantity" meta:resourcekey="Localize7Resource1"></asp:Localize>
                                </th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Order status" meta:resourcekey="TemplateFieldResource12">
                    <ItemTemplate>
                        <asp:Label ID="lbStatus" runat="server" Text='<%# Bind("Deliveredstatus") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="../Report/PrintOrder.aspx?oid={0}"
                    meta:resourcekey="PrintLinkResource" Target="_blank" Text="&lt;img src=&quot;../../Images/print.gif&quot; border=&quot;0&quot; /&gt;" />
            </Columns>
            <RowStyle VerticalAlign="Top" />
            <PagerSettings Mode="NextPreviousFirstLast" Position="Top" />
            <PagerTemplate>
                <div style="float: left">
                    <asp:Literal ID="litPageInfo" runat="server" meta:resourcekey="litPageInfoResource1"></asp:Literal></div>
                <div style="float: right; text-align: right">
                    <asp:Button ID="cmdFirst" runat="server" CommandArgument="First" CommandName="Page"
                        meta:resourcekey="cmdFirstResource1" OnClick="cmdFirst_Click" Text="First" />
                    <asp:Button ID="cmdPrevious" runat="server" CommandArgument="Prev" CommandName="Page"
                        meta:resourcekey="cmdPreviousResource1" OnClick="cmdPrevious_Click" Text="Previous" />
                    <asp:Button ID="cmdNext" runat="server" CommandArgument="Next" CommandName="Page"
                        meta:resourcekey="cmdNextResource1" OnClick="cmdNext_Click" Text="Next" />
                    <asp:Button ID="cmdLast" runat="server" CommandArgument="Last" CommandName="Page"
                        meta:resourcekey="cmdLastResource1" OnClick="cmdLast_Click" Text="Last" />
                </div>
            </PagerTemplate>
        </asp:GridView>--%>
        <asp:ListView ID="lv" runat="server">
            <LayoutTemplate>
                <div id="grid" class="grid">
                    <div class="title">
                        <asp:Literal ID="Literal1" runat="server" Text="Order List and Receive"></asp:Literal>
                    </div>
                    <table class="datatable" cellpadding="0" cellspacing="0">
                        <thead>
                            <tr>
                                <th>
                                    <asp:Literal ID="Literal2" runat="server" Text="Vehicle"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal3" runat="server" Text="Color"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal4" runat="server" Text="Order Quantity"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal5" runat="server" Text="Shipping Quantity"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal6" runat="server" Text="Import Quantity"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal7" runat="server" Text="Lack Quantity"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal13" runat="server" Text="Sold Quantity"></asp:Literal>
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
                                    <asp:Literal ID="Literal1" runat="server" Text="Order No:"></asp:Literal>
                                </th>
                                <td>
                                    <%# Eval("OrderNumber")%>
                                </td>
                                <th>
                                    <asp:Literal ID="Literal8" runat="server" Text="Order Date:"></asp:Literal>
                                </th>
                                <td>
                                    <%# VDMS.Helper.DateTimeHelper.To24h((DateTime)Eval("OrderDate"))%>
                                </td>
                                <th>
                                    <asp:Literal ID="Literal9" runat="server" Text="Dealer Code:"></asp:Literal>
                                </th>
                                <td>
                                    <%# Eval("DealerCode")%>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Literal ID="Literal10" runat="server" Text="Delivery Place:"></asp:Literal>
                                </th>
                                <td>
                                    <%# Eval("ShippingTo")%>
                                </td>
                                <th>
                                    <asp:Literal ID="Literal11" runat="server" Text="Order Times:"></asp:Literal>
                                </th>
                                <td>
                                    <%# Eval("OrderTimes")%>
                                </td>
                                <th>
                                    <asp:Literal ID="Literal12" runat="server" Text="Status:"></asp:Literal>
                                </th>
                                <td>
                                    <%# GetOrderStatus((int)Eval("Status"))%>
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
                            <td class="number" style='color: <%#(int)Eval("OrderQty") - (int)Eval("ShippingQuantity") == 0 ? "inherit" : "Red"%>;'>
                                <%#(int)Eval("OrderQty") - (int)Eval("ShippingQuantity")%>
                            </td>
                            <td class="number">
                                <%#Eval("SoldQuantity")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
            <EmptyDataTemplate>
                <asp:Literal ID="Literal1" runat="server" Text="There are not any order here."></asp:Literal>
            </EmptyDataTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsOrders" runat="server" SelectMethod="CheckOrder" EnablePaging="true"
            TypeName="VDMS.I.ObjectDataSource.OrderHeaderDataSource" SelectCountMethod="CountCheckOrder">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtDealer" Name="dealerCode" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtFromDate" Name="fromDate" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtToDate" Name="toDate" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="ddlStatus" Name="status" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="txtOrderNo1" Name="orderNo" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="ddlArea" Name="area" PropertyName="SelectedValue"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
