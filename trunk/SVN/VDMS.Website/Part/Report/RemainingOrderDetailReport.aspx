<%@ Page Title="Order detail report" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="RemainingOrderDetailReport.aspx.cs" Inherits="Part_Report_RemainingOrderDetailReport"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <table>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" Text="Order number:" runat="server" meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtOrderNo" runat="server" meta:resourcekey="txtOrderNoResource1"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="Literal3" Text="Order date:" runat="server" meta:resourcekey="Literal3Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtOrderFrom" runat="server" meta:resourcekey="txtOrderFromResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ValidationGroup="Report" ID="RequiredFieldValidator1"
                        runat="server" SetFocusOnError="True" ControlToValidate="txtOrderFrom" ErrorMessage="Order date cannot be blank!"
                        Enabled="False" meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                    <asp:ImageButton ID="ibOrderFrom" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="meeOrderFrom" runat="server" TargetControlID="txtOrderFrom"
                        Mask="99/99/9999" MaskType="Date" Enabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                        CultureTimePlaceholder="">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceOrderFrom" runat="server" TargetControlID="txtOrderFrom"
                        PopupButtonID="ibOrderFrom" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    ~
                    <asp:TextBox ID="txtOrderTo" runat="server" meta:resourcekey="txtOrderToResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibtxtOrderTo" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtOrderTo"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="meetxtOrderTo" Enabled="True" CultureAMPMPlaceholder=""
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtOrderTo"
                        PopupButtonID="ibtxtOrderTo" BehaviorID="cetxtOrderTo" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal6" Text="Issue number:" runat="server" meta:resourcekey="Literal6Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtIssueNo" runat="server" meta:resourcekey="txtIssueNoResource1"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="Literal4" Text="Issue date:" runat="server" meta:resourcekey="Literal4Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtIssueFrom" runat="server" meta:resourcekey="txtIssueFromResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ValidationGroup="Report" ID="RequiredFieldValidator2"
                        runat="server" SetFocusOnError="True" ControlToValidate="txtIssueFrom" Enabled="False"
                        ErrorMessage="Issue date cannot be blank!" meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                    <asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtIssueFrom"
                        Mask="99/99/9999" MaskType="Date" Enabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                        CultureTimePlaceholder="">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtIssueFrom"
                        PopupButtonID="ibFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    ~
                    <asp:TextBox ID="txtIssueTo" runat="server" meta:resourcekey="txtIssueToResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibToDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtIssueTo"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="meeFromDate" Enabled="True" CultureAMPMPlaceholder=""
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtIssueTo"
                        PopupButtonID="ibToDate" BehaviorID="ceFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal1" Text="Dealer code:" runat="server" meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtDealerCode" runat="server" meta:resourcekey="txtDealerCodeResource1"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="Literal7" Text="Area:" runat="server" meta:resourcekey="Literal7Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:AreaList SeparateByDB="true" ShowSelectAllItem="True" runat="server" ID="ddlArea"
                        meta:resourcekey="ddlAreaResource1">
                    </vdms:AreaList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="2">
                    <asp:Button ID="btnDoReport" runat="server" Text="Find" OnClick="btnDoReport_Click"
                        meta:resourcekey="btnDoReportResource1" />
                    <asp:Button ID="btnExcel" runat="server" Text="Export excel" OnClick="btnExcel_Click"
                        meta:resourcekey="btnExcelResource1" />
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <br />
        <%# Resources.Constants.GrandTotal %>
        <asp:ListView ID="lv" runat="server" OnDataBound="lv_DataBound">
            <LayoutTemplate>
                <table class="datatable" cellpadding="0" cellspacing="0">
                    <tr>
                        <th>
                            <asp:Literal ID="Literal11" runat="server" Text="Seq" meta:resourcekey="Literal11Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal12" runat="server" Text="Part Code" meta:resourcekey="Literal12Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal13" runat="server" Text="Part Name" meta:resourcekey="Literal13Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal14" runat="server" Text="Unit" meta:resourcekey="Literal14Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal15" runat="server" Text="Order Qty" meta:resourcekey="Literal15Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal16" runat="server" Text="Quatation Qty" meta:resourcekey="Literal16Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal17" runat="server" Text="Deliveri Qty" meta:resourcekey="Literal17Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal18" runat="server" Text="Remain Qty" meta:resourcekey="Literal18Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal19" runat="server" Text="Unit Price" meta:resourcekey="Literal19Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal20" runat="server" Text="Remain Amount" meta:resourcekey="Literal20Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal21" runat="server" Text="Remark" meta:resourcekey="Literal21Resource1"></asp:Literal>
                        </th>
                    </tr>
                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                    <tr>
                        <td colspan="11">
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="summary">
                        <td colspan="4" class="right">
                            <%# Resources.Constants.GrandTotal %>
                        </td>
                        <td class="number">
                            <asp:Literal ID="Literal22" runat="server" ></asp:Literal>
                        </td>
                        <td class="number">
                            <asp:Literal ID="Literal23" runat="server" ></asp:Literal>
                        </td>
                        <td class="number">
                            <asp:Literal ID="Literal24" runat="server" ></asp:Literal>
                        </td>
                        <td class="number">
                            <asp:Literal ID="Literal25" runat="server" ></asp:Literal>
                        </td>
                        <td class="number">
                            <asp:Literal ID="Literal26" runat="server" ></asp:Literal>
                        </td>
                        <td class="number">
                            <asp:Literal ID="Literal27" runat="server" ></asp:Literal>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="11">
                            <vdms:DataPager runat="server" ID="pager" PagedControlID="lv" DisablePaging="False">
                            </vdms:DataPager>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class="group">
                    <td colspan="11">
                        <asp:Literal ID="Literal10" runat="server" Text='<%# Eval("DealerCodeAndName") %>'></asp:Literal>
                    </td>
                </tr>
                <asp:ListView ID="lvOrder" runat="server" DataSource='<%# Eval("OrderHeaders") %>'>
                    <LayoutTemplate>
                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class="subGroup">
                            <td colspan="11">
                                <table class="info" style="text-align: left;">
                                    <tr>
                                        <th>
                                            <asp:Literal ID="Literal5" runat="server" Text="Address:" meta:resourcekey="Literal5Resource1"></asp:Literal>
                                        </th>
                                        <td colspan="7">
                                            <asp:Literal ID="litAddress" runat="server" Text='<%# Eval("FullOrderAddress") %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            <asp:Literal ID="Literals5" runat="server" Text="Order No:" meta:resourcekey="Literals5Resource1"></asp:Literal>
                                        </th>
                                        <td>
                                            <asp:Literal ID="litOrder" runat="server" Text='<%# Eval("TipTopNumber") %>' />
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <th>
                                            <asp:Literal ID="Literal8" runat="server" Text="Order date:" meta:resourcekey="Literal8Resource1"></asp:Literal>
                                        </th>
                                        <td>
                                            <asp:Literal ID="litODate" runat="server" Text='<%# Eval("OrderDate","{0:d}") %>' />
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <th>
                                            <asp:Literal ID="Literal9" runat="server" Text="Delivery:" meta:resourcekey="Literal9Resource1"></asp:Literal>
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
                                <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
                                    <td class="center">
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
                                    <td class="number">
                                        <%#Eval("OrderQuantity", "{0:N0}")%>
                                    </td>
                                    <td class="number">
                                        <%#Eval("QuotationQuantity", "{0:N0}")%>
                                    </td>
                                    <td class="number">
                                        <%#Eval("DelivaryQuantity", "{0:N0}")%>
                                    </td>
                                    <td class="number">
                                        <%#Eval("RemainQuantity", "{0:N0}")%>
                                    </td>
                                    <td class="number">
                                        <%#Eval("UnitPrice", "{0:N0}")%>
                                    </td>
                                    <td class="number">
                                        <%#Eval("RemainAmount", "{0:N0}")%>
                                    </td>
                                    <td >
                                        <%# StringToShortDateTime((string)Eval("Note"))%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                        <tr class="summary">
                            <td colspan="4" class="right">
                                <%# Resources.Constants.Total %>
                            </td>
                            <td class="number">
                                <%#Eval("OrderHeaderSummary.OrderQuantity", "{0:N0}") %>
                            </td>
                            <td class="number">
                                <%#Eval("OrderHeaderSummary.QuotationQuantity", "{0:N0}")%>
                            </td>
                            <td class="number">
                                <%#Eval("OrderHeaderSummary.DelivaryQuantity", "{0:N0}")%>
                            </td>
                            <td class="number">
                                <%#Eval("OrderHeaderSummary.RemainQuantity", "{0:N0}")%>
                            </td>
                            <td class="number">
                                <%#Eval("OrderHeaderSummary.UnitPrice", "{0:N0}")%>
                            </td>
                            <td class="number">
                                <%#Eval("OrderHeaderSummary.RemainAmount", "{0:N0}")%>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <tr class="summary">
                    <td colspan="4" class="right">
                        <%# Resources.Constants.DealerAllTotal %>
                    </td>
                    <td class="number">
                        <%#Eval("DealerSummary.OrderQuantity", "{0:N0}")%>
                    </td>
                    <td class="number">
                        <%#Eval("DealerSummary.QuotationQuantity", "{0:N0}")%>
                    </td>
                    <td class="number">
                        <%#Eval("DealerSummary.DelivaryQuantity", "{0:N0}")%>
                    </td>
                    <td class="number">
                        <%#Eval("DealerSummary.RemainQuantity", "{0:N0}")%>
                    </td>
                    <td class="number">
                        <%#Eval("DealerSummary.UnitPrice", "{0:N0}")%>
                    </td>
                    <td class="number">
                        <%#Eval("DealerSummary.RemainAmount", "{0:N0}")%>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="ods" runat="server" EnablePaging="True" SelectMethod="GetDealerHasOrderRemain"
            SelectCountMethod="CountDealerHasOrderRemain" TypeName="VDMS.II.PartManagement.Order.OrderDAO">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlArea" Name="aCode" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="txtOrderNo" Name="orderNo" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="txtOrderFrom" Name="oFrom" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="txtOrderTo" Name="oTo" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="txtIssueNo" Name="issueNo" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="txtIssueFrom" Name="iFrom" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="txtIssueTo" Name="iTo" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="txtDealerCode" Name="dCode" PropertyName="Text"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:Label ID="lblNoResult" runat="server" Text="No result" Visible="False" meta:resourcekey="lblNoResult"></asp:Label>
    </div>
</asp:Content>
