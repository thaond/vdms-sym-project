<%@ Page Title="Sale summarize" Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true" CodeFile="saleReport.aspx.cs" Inherits="Part_Report_saleReport" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" Runat="Server">
    <div class="form" style="width: 450px">
        <asp:ValidationSummary ID="ValidationSummary1" CssClass="error" 
            ValidationGroup="Report" runat="server" 
            meta:resourcekey="ValidationSummary1Resource1" />
        <table>
            <tr>
                <td>
                    <asp:Localize ID="litType" runat="server" Text="Type:" 
                        meta:resourcekey="litTypeResource1"></asp:Localize>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" 
                        meta:resourcekey="rblTypeResource1">
                        <asp:ListItem Selected="True" Text="<%$ Resources:TextMsg, Part %>" Value="P" 
                            meta:resourcekey="ListItemResource1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:TextMsg, Accessory %>" Value="A" 
                            meta:resourcekey="ListItemResource2"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="Sale date:" 
                        meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" 
                        meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="i1" runat="Server" SkinID="CalendarImageButton" 
                        OnClientClick="return false;" meta:resourcekey="i1Resource1" />
                    <asp:RequiredFieldValidator ID="r1" runat="server" ErrorMessage="Report date cannot be blank!"
                        SetFocusOnError="True" ValidationGroup="Report" 
                        ControlToValidate="txtFromDate" meta:resourcekey="r1Resource1">*</asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" 
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ce1" runat="server" TargetControlID="txtFromDate"
                        PopupButtonID="i1" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    ~
                    <asp:TextBox ID="txtToDate" runat="server" 
                        meta:resourcekey="txtToDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="i2" runat="Server" SkinID="CalendarImageButton" 
                        OnClientClick="return false;" meta:resourcekey="i2Resource1" />
                    <asp:RequiredFieldValidator ID="r2" runat="server" Text="*" 
                        ErrorMessage="'Sale date to' cannot be empty!" SetFocusOnError="True"
                        ValidationGroup="Report" ControlToValidate="txtToDate" 
                        meta:resourcekey="r2Resource1" ></asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="meeToDate" runat="server" TargetControlID="txtToDate"
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" 
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ce2" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="i2" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <%--<tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Dealer:"></asp:Literal>
                </td>
                <td>
                    <vdms:DealerList runat="server" ID="ddlDealers" ShowSelectAllItem="true" 
                        onselectedindexchanged="ddlDealers_SelectedIndexChanged">
                    </vdms:DealerList>
                </td>
            </tr>--%>
            <tr>
                <td>
                    <asp:Literal ID="Literal6" runat="server" Text="Area:" 
                        meta:resourcekey="Literal6Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:DatabaseList AutoPostBack="True" AllowDealerSelect="False" 
                        ShowSelectAllItem="True" ID="ddlRegion"
                        runat="server" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged" 
                        meta:resourcekey="ddlRegionResource1">
                    </vdms:DatabaseList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Dealer:" 
                        meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:DealerList EnabledSaperateByDB="False" ShowSelectAllItem="True" 
                        runat="server" ID="ddlDealers" EnabledSaperateByArea="False" MergeCode="False" 
                        meta:resourcekey="ddlDealersResource1" RemoveRootItem="False" 
                        ShowEmptyItem="False">
                    </vdms:DealerList>
                </td>
            </tr>             
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnShowReport" ValidationGroup="Report" runat="server" 
                        Text="View" OnClick="btnShowReport_Click" 
                        meta:resourcekey="btnShowReportResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <asp:ListView ID="lvParts" runat="server" EnableViewState="true">
            <LayoutTemplate>
                <div id="grid" class="grid">
                    <div class="title">
                        <asp:Literal ID="litTittle" runat="server" Text="Parts list" 
                            meta:resourcekey="litTittleResource1"></asp:Literal>
                    </div>
                    <table cellpadding="0" cellspacing="0" class="datatable">
                        <thead>
                            <tr>
                                <th>
                                    <asp:Literal ID="Literal10" runat="server" Text="Part No" 
                                        meta:resourcekey="Literal10Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal8" runat="server" Text="Quantity" 
                                        meta:resourcekey="Literal8Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal5" runat="server" Text="Amount" 
                                        meta:resourcekey="Literal5Resource1"></asp:Literal>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="3">
                                    <vdms:DataPager PageSize="2" ID="DataPager1" runat="server" PagedControlID="lvParts" />
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class="group">
                    <td colspan="3">
                        <%#Eval("DealerCode")%><br />
                        <%#Eval("DealerName")%>
                    </td>
                </tr>
                <asp:ListView ID="lvItems" EnableViewState="False" runat="server" 
                    DataSource='<%# GetPartSaleList(Eval("DealerCode")) %>'>
                    <LayoutTemplate>
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
                            <td>
                                <%#Eval("PartCode")%>
                            </td>
                            <td class="number">
                                <%#Eval("Quantity")%>
                            </td>
                            <td class="number">
                                <%#Eval("Price")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsPartCurrent" runat="server" EnablePaging="True" SelectMethod="SearchDealerByCodeWithDB"
            SelectCountMethod="CountByCodeWithDB" 
            TypeName="VDMS.II.BasicData.DealerDAO">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlDealers" Name="dealerCode" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="ddlRegion" Name="dbCode" PropertyName="SelectedValue"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>

