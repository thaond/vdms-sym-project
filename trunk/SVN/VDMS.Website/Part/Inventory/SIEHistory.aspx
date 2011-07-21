<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
	CodeFile="SIEHistory.aspx.cs" Inherits="Part_Inventory_SIEHistory" Title="Special import/export history"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="form">
		<table class="style1">
			<tr>
				<td style="width: 150px">
					<asp:Literal ID="Literal1" runat="server" Text="Transaction date:" meta:resourcekey="Literal1Resource1"></asp:Literal>
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
					<asp:Literal ID="Literal3" runat="server" Text="Dealer:" meta:resourcekey="Literal3Resource1"></asp:Literal>
				</td>
				<td>
					<vdms:DealerList runat="server" AppendDataBoundItems="True" AutoPostBack="True" ID="ddlDealer"
						OnSelectedIndexChanged="Unnamed3_SelectedIndexChanged" EnabledSaperateByArea="False"
						EnabledSaperateByDB="False" MergeCode="False" meta:resourcekey="ddlDealerResource1"
						RemoveRootItem="False" ShowEmptyItem="False" ShowSelectAllItem="False">
					</vdms:DealerList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Literal ID="Literal4" runat="server" Text="Warehouse" meta:resourcekey="Literal4Resource1"></asp:Literal>
				</td>
				<td>
					<vdms:WarehouseList ShowSelectAllItem="True" ID="ddlWarehouse" runat="server" DontAutoUseCurrentSealer="False"
						meta:resourcekey="ddlWarehouseResource1" ShowEmptyItem="False" UseVIdAsValue="False">
					</vdms:WarehouseList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Literal ID="Literal2" runat="server" Text="Action:" meta:resourcekey="Literal2Resource1"></asp:Literal>
				</td>
				<td>
					<asp:DropDownList runat="server" ID="ddlAction" meta:resourcekey="ddlActionResource1">
						<asp:ListItem Text="" Value="SI,SE" meta:resourcekey="ListItemResource1"></asp:ListItem>
						<asp:ListItem Text="SI" Value="SI" meta:resourcekey="ListItemResource2"></asp:ListItem>
						<asp:ListItem Text="SE" Value="SE" meta:resourcekey="ListItemResource3"></asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Literal ID="Literal5" runat="server" Text="Session No:" meta:resourcekey="Literal5Resource1"></asp:Literal>
				</td>
				<td>
					<asp:TextBox ID="txtSessionNo" runat="server" meta:resourcekey="txtSessionNoResource1"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					&nbsp;
				</td>
				<td>
					<asp:Button ID="btnQuery" runat="server" Text="Query" OnClick="btnQuery_Click" meta:resourcekey="btnQueryResource1" />
				</td>
			</tr>
			<tr>
				<td>
					&nbsp;
				</td>
				<td>
					&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<div class="grid">
		<vdms:PageGridView ID="gv" runat="server" AutoGenerateColumns="False" meta:resourcekey="gvResource1">
			<Columns>
				<asp:BoundField DataField="InvoiceNumber" HeaderText="Session No" SortExpression="InvoiceNumber"
					Visible="true" meta:resourcekey="BoundFieldResource1" />
				<asp:BoundField DataField="DealerCode" HeaderText="DealerCode" SortExpression="DealerCode"
					Visible="False" meta:resourcekey="BoundFieldResource2" />
				<asp:TemplateField HeaderText="Part code" SortExpression="PartInfoId" meta:resourcekey="TemplateFieldResource1">
					<ItemTemplate>
						<asp:Label ID="Label1" runat="server" Text='<%# Eval("PartInfo.PartCode") %>' meta:resourcekey="Label1Resource1"></asp:Label>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Warehouse" SortExpression="WarehouseId" meta:resourcekey="TemplateFieldResource2">
					<ItemTemplate>
						<asp:Label ID="Label2" runat="server" Text='<%# EvalWarehouse(Eval("WarehouseId")) %>'
							meta:resourcekey="Label2Resource1"></asp:Label>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:BoundField DataField="TransactionDate" HeaderText="TransactionDate" DataFormatString="{0:d}"
					SortExpression="TransactionDate" meta:resourcekey="BoundFieldResource3" />
				<asp:BoundField DataField="TransactionCode" HeaderText="TransactionCode" SortExpression="TransactionCode"
					meta:resourcekey="BoundFieldResource4">
					<ItemStyle CssClass="center" />
				</asp:BoundField>
				<asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity"
					meta:resourcekey="BoundFieldResource5">
					<ItemStyle CssClass="number" />
				</asp:BoundField>
				<asp:BoundField DataField="ActualCost" HeaderText="ActualCost" DataFormatString="{0:N0}"
					SortExpression="ActualCost" meta:resourcekey="BoundFieldResource6">
					<ItemStyle CssClass="number" />
				</asp:BoundField>
				<asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy"
					meta:resourcekey="BoundFieldResource7" />
				<asp:BoundField DataField="TransactionComment" HeaderText="TransactionComment" SortExpression="TransactionComment"
					meta:resourcekey="BoundFieldResource8" />
			</Columns>
		</vdms:PageGridView>
		<asp:ObjectDataSource ID="odsTrans" runat="server" EnablePaging="True" SelectMethod="FindTransactions"
			TypeName="VDMS.II.PartManagement.TransactionDAO">
			<SelectParameters>
				<asp:ControlParameter ControlID="ddlDealer" Name="dealerCode" PropertyName="SelectedValue"
					Type="String" />
				<asp:ControlParameter ControlID="ddlWarehouse" Name="wId" PropertyName="SelectedValue"
					Type="Int64" />
				<asp:ControlParameter ControlID="ddlAction" Name="transCode" PropertyName="SelectedValue"
					Type="String" />
				<asp:ControlParameter ControlID="txtFromDate" Name="fromDate" PropertyName="Text"
					Type="String" />
				<asp:ControlParameter ControlID="txtToDate" Name="toDate" PropertyName="Text" Type="String" />
				<asp:ControlParameter ControlID="txtSessionNo" Name="invNo" PropertyName="Text" Type="String" />
			</SelectParameters>
		</asp:ObjectDataSource>
	</div>
</asp:Content>
