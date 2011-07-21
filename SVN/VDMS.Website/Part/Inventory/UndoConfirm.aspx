<%@ Page Title="Undo Auto Confirm" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="UndoConfirm.aspx.cs" Inherits="Part_Inventory_UndoConfirm"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register TagPrefix="cc1" Namespace="VDMS.II.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="form" style="width: 450px">
		<table width="100%">
			<tr>
				<td>
					<asp:Localize ID="litInStockDate" runat="server" Text="Auto InStock Date:" meta:resourcekey="litInStockDateResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="txtFromDate" runat="server" Width="88px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
					<asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
						meta:resourcekey="ibFromDateResource1" />
					<asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="Validator"
						Text="*" SetFocusOnError="True" ValidationGroup="Save" ControlToValidate="txtFromDate"
						meta:resourcekey="rfvFromDateResource1"></asp:RequiredFieldValidator>
					<ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
						Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder=""
						CultureDateFormat="MDY" CultureDatePlaceholder="/" CultureDecimalPlaceholder="."
						CultureThousandsPlaceholder="," CultureTimePlaceholder=":" Enabled="True">
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
						Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder=""
						CultureDateFormat="MDY" CultureDatePlaceholder="/" CultureDecimalPlaceholder="."
						CultureThousandsPlaceholder="," CultureTimePlaceholder=":" Enabled="True">
					</ajaxToolkit:MaskedEditExtender>
					<ajaxToolkit:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtToDate"
						PopupButtonID="ibToDate" Enabled="True">
					</ajaxToolkit:CalendarExtender>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litDealer" runat="server" Text="Dealer:" meta:resourcekey="litDealerResource1"></asp:Localize>
				</td>
				<td>
					<cc1:DealerList ID="ddl" runat="server" RemoveRootItem="True" EnabledSaperateByArea="False"
						EnabledSaperateByDB="False" MergeCode="False" meta:resourcekey="ddlResource1"
						ShowEmptyItem="False" ShowSelectAllItem="False">
					</cc1:DealerList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litOrderNumber" runat="server" Text="Order Number:" meta:resourcekey="litOrderNumberResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="txtOrderNumber" runat="server" MaxLength="30" Width="180px" meta:resourcekey="txtOrderNumberResource1"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
				</td>
				<td>
					<asp:Button ID="cmdQuery" runat="server" Text="Query" OnClick="cmdQuery_Click" meta:resourcekey="cmdQueryResource1" />
				</td>
			</tr>
		</table>
	</div>
	<br />
	<div class="grid">
		<vdms:PageGridView ID="gv" runat="server" DataSourceID="ods1" AllowPaging="True"
			DataKeyNames="IssueNumber" meta:resourcekey="gvResource1">
			<Columns>
				<asp:BoundField HeaderText="Issue Number" DataField="IssueNumber" meta:resourcekey="BoundFieldResource1" />
				<asp:BoundField HeaderText="Dealer Code" DataField="DealerCode" meta:resourcekey="BoundFieldResource2" />
				<asp:BoundField HeaderText="Order No" DataField="TipTopNumber" meta:resourcekey="BoundFieldResource3" />
				<asp:BoundField HeaderText="Order Date" DataField="OrderDate" meta:resourcekey="BoundFieldResource4" />
				<asp:BoundField HeaderText="Shipping Date" DataField="ShippingDate" meta:resourcekey="BoundFieldResource5" />
				<asp:BoundField HeaderText="Auto InStock Date" DataField="ReceiveDate" meta:resourcekey="BoundFieldResource6" />
				<asp:ButtonField HeaderText="Undo" ButtonType="Button" CommandName="Delete" Text="Undo InStock"
					meta:resourcekey="ButtonFieldResource1" />
			</Columns>
			<EmptyDataTemplate>
				<asp:Literal ID="Literal1" runat="server" Text="There isn't any rows." meta:resourcekey="Literal1Resource1"></asp:Literal>
			</EmptyDataTemplate>
		</vdms:PageGridView>
	</div>
	<asp:ObjectDataSource ID="ods1" runat="server" TypeName="VDMS.II.PartManagement.Order.OrderDAO"
		EnablePaging="True" SelectMethod="FindAllAutoImport" SelectCountMethod="GetAutoImportCount"
		DeleteMethod="UndoAutoReceive">
		<SelectParameters>
			<asp:ControlParameter ControlID="txtFromDate" PropertyName="Text" Name="fromDate" />
			<asp:ControlParameter ControlID="txtToDate" PropertyName="Text" Name="toDate" />
			<asp:ControlParameter ControlID="ddl" PropertyName="SelectedValue" Name="dealerCode" />
			<asp:ControlParameter ControlID="txtOrderNumber" PropertyName="Text" Name="orderNumber" />
		</SelectParameters>
	</asp:ObjectDataSource>
</asp:Content>
