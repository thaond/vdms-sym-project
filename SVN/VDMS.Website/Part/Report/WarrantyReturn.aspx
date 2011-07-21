<%@ Page Title="Warranty Return report" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="WarrantyReturn.aspx.cs" Inherits="Part_Report_WarrantyReturn" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form" style="width: 450px">
         <asp:ValidationSummary ID="ValidationSummary1" CssClass="error" 
             ValidationGroup="Report" runat="server" 
             meta:resourcekey="ValidationSummary1Resource1" />
       <table>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="NG create date:" 
                        meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" 
                        meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" 
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtFromDate"
                        PopupButtonID="ibFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                     <asp:RequiredFieldValidator ID="r1" runat="server" ErrorMessage="NG create date cannot be blank!"
                        SetFocusOnError="True" ValidationGroup="Report" 
                        ControlToValidate="txtFromDate" meta:resourcekey="r1Resource1">*</asp:RequiredFieldValidator>
                    ~
                    <asp:TextBox ID="txtToDate" runat="server" 
                        meta:resourcekey="txtToDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibToDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtToDate"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="meeFromDate" 
                        CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="ibToDate" BehaviorID="ceFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="Part No:" 
                        meta:resourcekey="Literal4Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPartNo" runat="server" 
                        meta:resourcekey="txtPartNoResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal5" runat="server" Text="Area:" 
                        meta:resourcekey="Literal5Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlArea" runat="server" 
                        meta:resourcekey="ddlAreaResource1">
                        <asp:ListItem Selected="True" Value="" meta:resourcekey="ListItemResource1">All</asp:ListItem>
                        <asp:ListItem Value="HTF" meta:resourcekey="ListItemResource2">North</asp:ListItem>
                        <asp:ListItem Value="DNF" meta:resourcekey="ListItemResource3">South</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal6" runat="server" Text="Status:" 
                        meta:resourcekey="Literal6Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" 
                        meta:resourcekey="ddlStatusResource1">
                        <asp:ListItem Selected="True" Value="" meta:resourcekey="ListItemResource4">All</asp:ListItem>
                        <asp:ListItem Value="OP" meta:resourcekey="ListItemResource5">Open</asp:ListItem>
                        <asp:ListItem Value="SN" meta:resourcekey="ListItemResource6">Sent</asp:ListItem>
                        <asp:ListItem Value="CF" meta:resourcekey="ListItemResource7">Confirmed</asp:ListItem>
                        <asp:ListItem Value="RJ" meta:resourcekey="ListItemResource8">Rejected</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Dealer:" 
                        meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:DealerList RemoveRootItem="True" ShowSelectAllItem="True" runat="server" 
                        ID="ddlDealers" EnabledSaperateByArea="False" EnabledSaperateByDB="False" 
                        MergeCode="False" meta:resourcekey="ddlDealersResource1" ShowEmptyItem="False">
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
        <asp:ListView ID="lvParts" runat="server">
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
                                    <asp:Literal ID="Literal8" runat="server" Text="Request Quantity" 
                                        meta:resourcekey="Literal8Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal9" runat="server" Text="Approved Quantity" 
                                        meta:resourcekey="Literal9Resource1"></asp:Literal>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr class="sumLine">
                                <td class="number">
                                    <asp:Literal ID="Literal7" runat="server" Text="Total:" 
                                        meta:resourcekey="Literal7Resource1"></asp:Literal>
                                </td>
                                <td class="number">
                                    <asp:Literal ID="litTotalPartR" runat="server" 
                                        meta:resourcekey="litTotalPartRResource1"></asp:Literal>
                                </td>
                                <td class="number">
                                    <asp:Literal ID="litTotalPartA" runat="server" 
                                        meta:resourcekey="litTotalPartAResource1"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <vdms:DataPager ID="DataPager1" runat="server" PagedControlID="lvParts">
                                    </vdms:DataPager>
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
                <asp:ListView ID="lvItems" runat="server" DataSource='<%# GetPartReturnList(Eval("DealerCode")) %>'>
                    <LayoutTemplate>
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
                            <td>
                                <%#Eval("PartCode")%>
                            </td>
                            <td class="number">
                                <%#Eval("RequestQuantity")%>
                            </td>
                            <td class="number">
                                <%#Eval("ApprovedQuantity")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsDealerList" runat="server" EnablePaging="True" SelectMethod="FindByCode"
            SelectCountMethod="CountByCode" TypeName="VDMS.II.BasicData.DealerDAO" 
            OnSelected="odsDealerList_Selected">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlDealers" Name="dealerCode" PropertyName="SelectedValue"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    <%--<div class="grid">
        <vdms:PageGridView ID="gvWarranty" runat="server" Width="500">
            <Columns>
                <asp:BoundField DataField="Dealer" HeaderText="Dealer" />
                <asp:BoundField DataField="PartNo" HeaderText="Part No" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
            </Columns>
        </vdms:PageGridView>
    </div>--%>
</asp:Content>
