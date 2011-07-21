<%@ Page Title="Part order service management summary" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="PartOrderServiceSummary.aspx.cs" Inherits="Part_Report_PartOrderServiceSummary"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form" style="float: left; width: 40%;">
        <asp:ValidationSummary ID="ValidationSummary1" CssClass="error" ValidationGroup="Report"
            runat="server" meta:resourcekey="ValidationSummary1Resource1" />
        <table width="500px">
            <tr>
                <td>
                    <asp:Localize ID="litOrderDate" runat="server" Text="Order Date:" meta:resourcekey="litOrderDateResource1"></asp:Localize>
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" Width="88px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ValidationGroup="Report" ID="RequiredFieldValidator1"
                        runat="server" SetFocusOnError="True" ControlToValidate="txtFromDate" ErrorMessage="Report date cannot be blank!"
                        meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                    <asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="Validator"
                        Text="*" SetFocusOnError="True" ValidationGroup="Save" ControlToValidate="txtFromDate"
                        meta:resourcekey="rfvFromDateResource1"></asp:RequiredFieldValidator>
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
            <%--<tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Dealer code:"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtDealerCode" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="Region:"></asp:Literal>
                </td>
                <td>
                    <vdms:DatabaseList ShowSelectAllItem="true" ID="ddlRegion" runat="server">
                    </vdms:DatabaseList>
                </td>
            </tr>--%>
            <tr>
                <td>
                    <asp:Literal ID="Literal6" runat="server" Text="Area:" meta:resourcekey="Literal6Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:DatabaseList AutoPostBack="True" AllowDealerSelect="False" ShowSelectAllItem="True"
                        ID="ddlRegion" runat="server" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged"
                        meta:resourcekey="ddlRegionResource1">
                    </vdms:DatabaseList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Dealer:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:DealerList EnabledSaperateByDB="False" ShowSelectAllItem="True" runat="server"
                        ID="ddlDealers" EnabledSaperateByArea="False" MergeCode="False" meta:resourcekey="ddlDealersResource1"
                        RemoveRootItem="False" ShowEmptyItem="False">
                    </vdms:DealerList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="cmdQuery" ValidationGroup="Report" runat="server" Text="Query" OnClick="cmdQuery_Click"
                        meta:resourcekey="cmdQueryResource1" />
                    <asp:Button ID="btnExcel" runat="server" Text="Export to excel" OnClick="btnExcel_Click"
                        meta:resourcekey="btnExcelResource1" />
                </td>
            </tr>
        </table>
    </div>
    	<div class="help" style="float: right; width: 45%;">
		<ul>
			<li>
				<asp:Localize ID="lh1" runat="server" Text="You can filter the result by combine the input condition."
					meta:resourcekey="lh1Resource1"></asp:Localize>
			</li>
			<%--<li>
				<asp:Localize ID="lh2" runat="server" Text="If you want to make new order, click to 'Add new Order'."
					meta:resourcekey="lh2Resource1"></asp:Localize>
			</li>
			<li>
				<asp:Localize ID="lh3" runat="server" Text="Order status, refer to Status ComboBox."
					meta:resourcekey="lh3Resource1"></asp:Localize>
			</li>
			<li>
				<asp:Localize ID="th4" runat="server" Text="Sub-Order: System auto create when QUOTED qty LESS than ORDER qty"
					meta:resourcekey="th4Resource1"></asp:Localize>
			</li>--%>
		</ul>
	</div>
	<br />
	<div class="grid" style="clear: both;" />
    <div class="grid">
        <vdms:PageGridView ID="GridView1" AllowPaging="True" runat="server" ShowFooter="True"
            OnDataBound="GridView1_DataBound" AutoGenerateColumns="False" meta:resourcekey="GridView1Resource1">
            <FooterStyle CssClass="sumLine" />
            <Columns>
                <asp:BoundField DataField="DealerCode" HeaderText="Dealer" FooterStyle-CssClass="number"
                    meta:resourcekey="BoundFieldResource1">
                    <FooterStyle CssClass="number"></FooterStyle>
                </asp:BoundField>
                <asp:BoundField DataField="QuotationParts" HeaderText="Quotation parts" ItemStyle-CssClass="number"
                    FooterStyle-CssClass="number" meta:resourcekey="BoundFieldResource2">
                    <FooterStyle CssClass="number"></FooterStyle>
                    <ItemStyle CssClass="number"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="OrderedParts" HeaderText="Ordered parts" ItemStyle-CssClass="number"
                    FooterStyle-CssClass="number" meta:resourcekey="BoundFieldResource3">
                    <FooterStyle CssClass="number"></FooterStyle>
                    <ItemStyle CssClass="number"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Supply rate" ItemStyle-CssClass="number" FooterStyle-CssClass="number"
                    meta:resourcekey="TemplateFieldResource1">
                    <ItemTemplate>
                        <%# EvalRate(Eval("QuotationParts"), Eval("OrderedParts"))%>
                    </ItemTemplate>
                    <FooterStyle CssClass="number"></FooterStyle>
                    <ItemStyle CssClass="number"></ItemStyle>
                </asp:TemplateField>
                <asp:BoundField DataField="QuotationPieces" HeaderText="Quotation pieces" ItemStyle-CssClass="number"
                    FooterStyle-CssClass="number" meta:resourcekey="BoundFieldResource4">
                    <FooterStyle CssClass="number"></FooterStyle>
                    <ItemStyle CssClass="number"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="OrderedPieces" HeaderText="Ordered pieces" ItemStyle-CssClass="number"
                    FooterStyle-CssClass="number" meta:resourcekey="BoundFieldResource5">
                    <FooterStyle CssClass="number"></FooterStyle>
                    <ItemStyle CssClass="number"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Service rate" ItemStyle-CssClass="number" FooterStyle-CssClass="number"
                    meta:resourcekey="TemplateFieldResource2">
                    <ItemTemplate>
                        <%# EvalRate(Eval("QuotationPieces"), Eval("OrderedPieces"))%>
                    </ItemTemplate>
                    <FooterStyle CssClass="number"></FooterStyle>
                    <ItemStyle CssClass="number"></ItemStyle>
                </asp:TemplateField>
            </Columns>
        </vdms:PageGridView>
        <asp:ObjectDataSource ID="odsRateRpt" runat="server" SelectMethod="CalcPartServiceRate"
            EnablePaging="True" TypeName="VDMS.II.Report.PartOrderServiceDAO" SelectCountMethod="CountInShortOrder">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlDealers" Name="dealerCode" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="ddlRegion" Name="dbCode" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="txtFromDate" Name="dateFrom" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtToDate" Name="dateTo" PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
