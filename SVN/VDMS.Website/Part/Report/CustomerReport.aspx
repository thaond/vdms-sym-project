<%@ Page Title="Customer sale report" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="CustomerReport.aspx.cs" Inherits="Part_Report_CustomerReport"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <table class="style1">
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Sale date:" meta:resourcekey="Literal1Resource1"></asp:Literal>
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
                    <asp:Literal ID="Literal2" runat="server" Text="Warehouse:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:WarehouseList ShowSelectAllItem="True" ID="ddlWarehouse" runat="server" DontAutoUseCurrentSealer="False"
                        meta:resourcekey="ddlWarehouseResource1" ShowEmptyItem="False" UseVIdAsValue="False">
                    </vdms:WarehouseList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="Customer:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:CustomerList ID="ddlCustomer" runat="server" ShowSelectAllItem="True" meta:resourcekey="ddlCustomerResource1"
                        ShowNullItemIfSelectFailed="False" AppendDataBoundItems="true">
                        <asp:ListItem Text="Retail Customer" Value="-1"></asp:ListItem>
                    </vdms:CustomerList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="Sale voucher:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtVchFrom" runat="server" meta:resourcekey="txtVchFromResource1"></asp:TextBox>
                    ~
                    <asp:TextBox ID="txtVchTo" runat="server" meta:resourcekey="txtVchToResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal6" runat="server" Text="Manual voucher:" meta:resourcekey="Literal6Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtMVchFrom" runat="server" meta:resourcekey="txtMVchFromResource1"></asp:TextBox>
                    ~
                    <asp:TextBox ID="txtMVchTo" runat="server" meta:resourcekey="txtMVchToResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal5" runat="server" Text="Status:" meta:resourcekey="Literal5Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" meta:resourcekey="ddlStatusResource1">
                        <asp:ListItem Value="" Text="All" meta:resourcekey="ListItemResource1"></asp:ListItem>
                        <asp:ListItem Value="OP" Text="Saved" meta:resourcekey="ListItemResource2"></asp:ListItem>
                        <asp:ListItem Value="NC" Text="Sale out" meta:resourcekey="ListItemResource3"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal7" runat="server" Text="Part code:" meta:resourcekey="Literal7Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPartCode" runat="server" meta:resourcekey="txtPartCodeResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnView" runat="server" Text="View" OnClick="btnView_Click" meta:resourcekey="btnViewResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div id="grid" class="grid">
        <asp:ListView ID="lv" runat="server" OnItemDataBound="lv_ItemDataBound">
            <LayoutTemplate>
                <div class="title">
                    <asp:Literal ID="Literal2" runat="server" Text="Sale data" meta:resourcekey="Literal2Resource2"></asp:Literal>
                </div>
                <table class="datatable" cellpadding="0" cellspacing="0">
                    <thead>
                        <tr>
                            <th>
                                <asp:Literal ID="Literal8" runat="server" Text="No" meta:resourcekey="Literal8Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal9" runat="server" Text="Voucher Number" meta:resourcekey="Literal9Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal10" runat="server" Text="Manual number" meta:resourcekey="Literal10Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal11" runat="server" Text="Status" meta:resourcekey="Literal11Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal12" runat="server" Text="Part Code" meta:resourcekey="Literal12Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal13" runat="server" Text="Part Name(E)" meta:resourcekey="Literal13Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal14" runat="server" Text="Part Name(V)" meta:resourcekey="Literal14Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal15" runat="server" Text="Quantity" meta:resourcekey="Literal15Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal16" runat="server" Text="Unit Price" meta:resourcekey="Literal16Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal17" runat="server" Text="Discount (%)" meta:resourcekey="Literal17Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal18" runat="server" Text="Sub Amount" meta:resourcekey="Literal18Resource1"></asp:Literal>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td class="pager" colspan="12">
                                <vdms:DataPager runat="server" ID="partPager">
                                </vdms:DataPager>
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class="group">
                    <td colspan="7" align="left">
                        [<%#Eval("Code")%>] -
                        <%#Eval("Name")%>
                        :
                        <%#Eval("ReportInfo")%>
                    </td>
                    <td class="number">
                        <asp:Literal ID="litQuantity" runat="server" meta:resourcekey="litQuantityResource1"></asp:Literal>
                    </td>
                    <td class="number">
                    </td>
                    <td class="number">
                    </td>
                    <td class="number">
                        <asp:Literal ID="litAmount" runat="server" meta:resourcekey="litAmountResource1"></asp:Literal>
                    </td>
                </tr>
                <asp:ListView ID="lvItems" runat="server" DataSource='<%# EvalDetail(Eval("CustomerId")) %>'>
                    <LayoutTemplate>
                        <tr runat="server" id="itemPlaceholder" />
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class='<%# ((int)Eval("No") == 0)? "selected" : (Container.DisplayIndex % 2 == 0 ? "even" : "odd") %>'>
                            <td class="center">
                                <%#((int)Eval("No") == 0)? "" : Eval("No")%>
                            </td>
                            <td>
                                <%#Eval("HeadVoucher")%>
                            </td>
                            <td>
                                <%#Eval("HeadManualVoucher")%>
                            </td>
                            <td class="center">
                                <%#EvalSatus((string)Eval("HeadStatus"))%>
                            </td>
                            <td>
                                <%#Eval("PartCode")%>
                            </td>
                            <td>
                                <%#Eval("EnglishName")%>
                            </td>
                            <td>
                                <%#Eval("VietnameseName")%>
                            </td>
                            <td class="number">
                                <%#((decimal)Eval("Quantity")).ToString("N0")%>
                            </td>
                            <td class="number">
                                <%#(((int)Eval("No") == 0) && ((decimal)Eval("UnitPrice") == 0)) ? "" : ((decimal)Eval("UnitPrice")).ToString("N0")%>
                            </td>
                            <td class="number">
                                <%#(((int)Eval("No") == 0) && ((decimal)Eval("Discount") == 0)) ? "" : ((decimal)Eval("Discount")).ToString("N0")%>
                            </td>
                            <td class="number">
                                <%#((decimal)Eval("SubAmount")).ToString("N0")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsCus" runat="server" SelectMethod="FindAllWithRetail" SelectCountMethod="GetCount"
            TypeName="VDMS.II.BasicData.CustomerDAO" EnablePaging="True">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlCustomer" Name="cusId" PropertyName="SelectedValue"
                    Type="Int64" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
