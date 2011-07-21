<%@ Page Title="Saved sale form" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="SavedSP.aspx.cs" Inherits="Part_SavedSP" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<%@ Register TagPrefix="cc1" Namespace="VDMS.II.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="form" style="width: 50%; float: left;">
		<table width="100%">
			<tr>
				<td>
					<asp:Localize ID="litOrderDate" runat="server" Text="Order Date:" 
                        meta:resourcekey="litOrderDateResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="txtFromDate" runat="server" Width="88px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
					<asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
						meta:resourcekey="ibFromDateResource1" />
					<asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="Validator"
						Text="*" SetFocusOnError="True" ValidationGroup="Save" ControlToValidate="txtFromDate"
						meta:resourcekey="rfvFromDateResource1"></asp:RequiredFieldValidator>
					<ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
						Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="AM;PM"
						CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
						CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
						Enabled="True">
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
						Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="AM;PM"
						CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
						CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
						Enabled="True">
					</ajaxToolkit:MaskedEditExtender>
					<ajaxToolkit:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtToDate"
						PopupButtonID="ibToDate" Enabled="True">
					</ajaxToolkit:CalendarExtender>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litSalesDate" runat="server" Text="Sales Date:" 
                        meta:resourcekey="litSalesDateResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="tbSalesFromDate" runat="server" Width="88px" 
                        meta:resourcekey="tbSalesFromDateResource1"></asp:TextBox>
					<asp:ImageButton ID="ibSalesFromDate" runat="Server" SkinID="CalendarImageButton"
						OnClientClick="return false;" meta:resourcekey="ibSalesFromDateResource1" />
					<ajaxToolkit:MaskedEditExtender ID="meeSalesFromDate" runat="server" TargetControlID="tbSalesFromDate"
						Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="AM;PM"
						CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
						CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
						Enabled="True">
					</ajaxToolkit:MaskedEditExtender>
					<ajaxToolkit:CalendarExtender ID="ceSalesFromDate" runat="server" TargetControlID="tbSalesFromDate"
						PopupButtonID="ibSalesFromDate" Enabled="True">
					</ajaxToolkit:CalendarExtender>
					~
					<asp:TextBox ID="tbSalesToDate" runat="server" Width="88px" 
                        meta:resourcekey="tbSalesToDateResource1"></asp:TextBox>
					<asp:ImageButton ID="ibSalesToDate" runat="Server" SkinID="CalendarImageButton" 
                        OnClientClick="return false;" meta:resourcekey="ibSalesToDateResource1" />
					<ajaxToolkit:MaskedEditExtender ID="meeSalesToDate" runat="server" TargetControlID="tbSalesToDate"
						Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="AM;PM"
						CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
						CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
						Enabled="True">
					</ajaxToolkit:MaskedEditExtender>
					<ajaxToolkit:CalendarExtender ID="ceSalesToDate" runat="server" TargetControlID="tbSalesToDate"
						PopupButtonID="ibSalesToDate" Enabled="True">
					</ajaxToolkit:CalendarExtender>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Literal ID="litStatus" runat="server" Text="Status:" 
                        meta:resourcekey="litStatusResource1"></asp:Literal>
				</td>
				<td>
					<asp:DropDownList ID="ddlS" runat="server" meta:resourcekey="ddlSResource1">
						<asp:ListItem Text="All" Value="" meta:resourcekey="ListItemResource1"></asp:ListItem>
						<asp:ListItem Selected="True" Text="Saved, not sale" Value="OP" 
                            meta:resourcekey="ListItemResource2"></asp:ListItem>
						<asp:ListItem Text="Sale out" Value="NC" meta:resourcekey="ListItemResource3"></asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Literal ID="litPartCode" runat="server" Text="Part Code:" 
                        meta:resourcekey="litPartCodeResource1"></asp:Literal>
				</td>
				<td>
					<asp:TextBox ID="txtPC" runat="server" meta:resourcekey="txtPCResource1"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Literal ID="litVN" runat="server" Text="Voucher Number:" 
                        meta:resourcekey="litVNResource1"></asp:Literal>
				</td>
				<td>
					<asp:TextBox ID="txtVN" runat="server" meta:resourcekey="txtVNResource1"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Literal ID="litTVN" runat="server" Text="Temporary Voucher Number:" 
                        meta:resourcekey="litTVNResource1"></asp:Literal>
				</td>
				<td>
					<asp:TextBox ID="txtTVN" runat="server" meta:resourcekey="txtTVNResource1"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
				</td>
				<td>
					<asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" 
                        meta:resourcekey="btnFindResource1" />
					<asp:Button ID="cmdAddNew" runat="server" Text="Add new Order" 
                        OnClientClick="javascript:location.href='Sale.aspx'; return false;" 
                        meta:resourcekey="cmdAddNewResource1" />
				</td>
			</tr>
		</table>
	</div>
	<div class="help" style="float: right; width: 35%;">
		<ul>
			<li>
				<asp:Localize ID="lh1" runat="server" Text="Query & Display all type of Order." 
                    meta:resourcekey="lh1Resource1"></asp:Localize>
			</li>
		</ul>
	</div>
	<div style="clear: both;">
	</div>
	<br />
	<div class="grid">
		<vdms:PageGridView ID="gv1" runat="server" meta:resourcekey="gv1Resource1">
			<Columns>
				<asp:TemplateField meta:resourcekey="TemplateFieldResource1">
					<ItemTemplate>
						<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("SalesHeaderId", "Sale.aspx?id={0}") %>'
							Text="Edit" meta:resourcekey="HyperLink1Resource1"></asp:HyperLink>
						<asp:HyperLink ID="lnkPrint" runat="server" NavigateUrl='<%# Eval("SalesHeaderId", "PrintSaleForm.aspx?id={0}") %>'
							Text="Print" meta:resourcekey="lnkPrintResource1"></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:BoundField HeaderText="Customer" DataField="CustomerName" 
                    meta:resourcekey="BoundFieldResource1" />
				<asp:BoundField HeaderText="Voucher Number" DataField="SalesOrderNumber" 
                    meta:resourcekey="BoundFieldResource2" />
				<asp:BoundField HeaderText="Temporary Voucher Number" 
                    DataField="ManualVoucherNumber" meta:resourcekey="BoundFieldResource3" />
				<asp:BoundField HeaderText="Order Date" DataField="OrderDate" 
                    DataFormatString="{0:d}" meta:resourcekey="BoundFieldResource4" />
				<asp:BoundField HeaderText="Sales Date" DataField="SalesDate" 
                    DataFormatString="{0:d}" meta:resourcekey="BoundFieldResource5" />
				<asp:BoundField HeaderText="Sales Person" DataField="SalesPerson" 
                    meta:resourcekey="BoundFieldResource6" />
				<asp:TemplateField meta:resourcekey="TemplateFieldResource2">
					<ItemTemplate>
						<asp:LinkButton ID="lnkbDel" OnClick="lnkbDel_OnClick"
							shid='<%# Eval("SalesHeaderId")%>' runat="server" Text="Delete" Visible='<%# (string)Eval("Status") == "OP" %>' 
                            meta:resourcekey="lnkbDelResource1"></asp:LinkButton>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
			<EmptyDataTemplate>
                <asp:Literal ID="Literal1" runat="server" Text="There isn't any rows." 
                    meta:resourcekey="Literal1Resource1"></asp:Literal>
			</EmptyDataTemplate>
		</vdms:PageGridView>
	</div>
</asp:Content>
