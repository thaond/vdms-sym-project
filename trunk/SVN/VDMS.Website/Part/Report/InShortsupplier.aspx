<%@ Page Title="In Short supplier report" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="InShortsupplier.aspx.cs" Inherits="Part_Inventory_InShortReason"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form" style="width: 450px">
        <asp:ValidationSummary ID="ValidationSummary1" CssClass="error" ValidationGroup="Report"
            runat="server" meta:resourcekey="ValidationSummary1Resource1" />
        <table width="100%">
            <tr>
                <td>
                    <asp:Localize ID="litOrderDate" runat="server" Text="Order Date:" meta:resourcekey="litOrderDateResource1"></asp:Localize>
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" Width="88px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <asp:RequiredFieldValidator ValidationGroup="Report" ID="RequiredFieldValidator1"
                        runat="server" SetFocusOnError="True" ControlToValidate="txtFromDate" ErrorMessage="Report date cannot be blank!"
                        meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                        CultureTimePlaceholder="" Enabled="True">
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
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                        CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="ibToDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="litRegion" runat="server" Text="Region:" meta:resourcekey="litRegionResource1"></asp:Literal>
                </td>
                <td>
                    <vdms:DatabaseList ID="ddlRegion" runat="server" ShowSelectAllItem="False" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged"
                        AutoPostBack="True" AllowDealerSelect="False" meta:resourcekey="ddlRegionResource1">
                    </vdms:DatabaseList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="litDealer" runat="server" Text="Dealer code:" meta:resourcekey="litDealerResource1"></asp:Literal>
                </td>
                <td>
                    <vdms:DealerList EnabledSaperateByDB="False" RemoveRootItem="True" ShowSelectAllItem="True"
                        runat="server" ID="ddlDealers" EnabledSaperateByArea="False" MergeCode="False"
                        meta:resourcekey="ddlDealersResource1" ShowEmptyItem="False">
                    </vdms:DealerList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Localize ID="litCriterion" runat="server" Text="Criterion:" meta:resourcekey="litCriterionResource1"></asp:Localize>
                </td>
                <td>
                    <asp:DropDownList ID="ddlOrderBy" runat="server" meta:resourcekey="ddlOrderByResource1">
                        <asp:ListItem Value="O" Selected="True" Text="Most Order Quantity" meta:resourcekey="ListItemResource1"></asp:ListItem>
                        <asp:ListItem Value="S" Text="Most Short Quantity" meta:resourcekey="ListItemResource2"></asp:ListItem>
                        <asp:ListItem Value="R" Text="Most Short Rate" meta:resourcekey="ListItemResource3"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="cmdQuery" ValidationGroup="Report" runat="server" Text="Query" OnClick="cmdQuery_Click"
                        meta:resourcekey="cmdQueryResource1" />
                    <asp:Button ID="cmd2Excel" ValidationGroup="Report" runat="server" Text="Export to Excel"
                        OnClick="cmd2Excel_Click" meta:resourcekey="cmd2ExcelResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <vdms:PageGridView ID="gv" runat="server" AutoGenerateColumns="False" AllowPaging="True"
            DataSourceID="odsInshortPart" meta:resourcekey="gvResource1" PageSize="20">
            <Columns>
                <asp:BoundField HeaderText="Part Code" DataField="PartCode" meta:resourcekey="BoundFieldResource1" />
                <asp:BoundField HeaderText="English Name" DataField="EnName" meta:resourcekey="BoundFieldResource2" />
                <asp:BoundField HeaderText="Vietnamese Name" DataField="VnName" meta:resourcekey="BoundFieldResource3" />
                <asp:BoundField HeaderText="Total Order" DataField="TotalOrder" ItemStyle-CssClass="number"
                    meta:resourcekey="BoundFieldResource4">
                    <ItemStyle CssClass="number"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="Total Quotation" DataField="TotalQuotation" ItemStyle-CssClass="number"
                    meta:resourcekey="BoundFieldResource5">
                    <ItemStyle CssClass="number"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="Total Short" DataField="TotalShort" ItemStyle-CssClass="number"
                    meta:resourcekey="BoundFieldResource6">
                    <ItemStyle CssClass="number"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Rate" ItemStyle-CssClass="number" meta:resourcekey="TemplateFieldResource1">
                    <ItemTemplate>
                        <%#Eval("StringShortRate")%>
                    </ItemTemplate>
                    <ItemStyle CssClass="number"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="Literal2" runat="server" Text="There isn't any rows." meta:resourcekey="Literal2Resource1"></asp:Literal>
            </EmptyDataTemplate>
        </vdms:PageGridView>
        <asp:ObjectDataSource ID="odsInshortPart" runat="server" EnablePaging="True" SelectMethod="FindInShortSupplier"
            TypeName="VDMS.II.Report.InShortSupplyDAO" SelectCountMethod="CountInShortSupplier">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlDealers" Name="dealerCode" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="ddlRegion" Name="dbCode" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="txtFromDate" Name="dateFrom" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtToDate" Name="dateTo" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="ddlOrderBy" Name="orderBy" PropertyName="SelectedValue"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
