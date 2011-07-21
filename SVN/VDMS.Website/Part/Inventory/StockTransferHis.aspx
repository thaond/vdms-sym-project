<%@ Page Title="Stock transfer history" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="StockTransferHis.aspx.cs" Inherits="Part_Inventory_StockTransferHis"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Src="~/Controls/UpdateProgress.ascx" TagName="UpdateProgress" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="form">
		<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
			<Triggers>
				<asp:AsyncPostBackTrigger ControlID="ddlFromDl" EventName="SelectedIndexChanged" />
				<asp:AsyncPostBackTrigger ControlID="ddlToDl" EventName="SelectedIndexChanged" />
			</Triggers>
			<ContentTemplate>
				<cc1:UpdateProgress runat="server" ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel2" />
				<asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Query"
					CssClass="error" meta:resourcekey="ValidationSummary1Resource1" />
				<table>
					<tr>
						<td style="width: 150px">
							<asp:Literal ID="Literal1" runat="server" Text="Transfer date:" meta:resourcekey="Literal1Resource1"></asp:Literal>
						</td>
						<td>
							<asp:TextBox ID="txtFromDate" runat="server" Width="88px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
							<asp:ImageButton ID="ibFromDate" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
								meta:resourcekey="ibFromDateResource1" />
							<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Query"
								runat="server" SetFocusOnError="True" ControlToValidate="txtFromDate" ErrorMessage="Report From date cannot be blank!"
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
							<asp:ImageButton ID="ibToDate" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
								meta:resourcekey="ibToDateResource1" />
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
							<asp:Localize ID="Localize1" runat="server" Text="From dealer:" meta:resourcekey="Localize1Resource1"></asp:Localize>
						</td>
						<td>
							<vdms:DealerList ID="ddlFromDl" RemoveRootItem="False" runat="server" AutoPostBack="True"
								ShowEmptyItem="True" OnSelectedIndexChanged="ddlFromDl_SelectedIndexChanged"
								OnDataBound="ddlFromDl_SelectedIndexChanged" EnabledSaperateByArea="False" EnabledSaperateByDB="False"
								MergeCode="False" meta:resourcekey="ddlFromDlResource1" ShowSelectAllItem="False" />
							<vdms:RequiredOneItemValidator runat="server" Text="*" ErrorMessage="You must select 'From Dealer' or 'To Dealer' to view report!"
								ID="rqo1" ListControlsToValidate="ddlFromDl,ddlToDl" ValidationGroup="Query"
								meta:resourcekey="rqo1Resource1" ValidateEmptyList="False" ValidateEmptyText="True"></vdms:RequiredOneItemValidator>
						</td>
					</tr>
					<tr>
						<td>
							<asp:Localize ID="litDeliveredPlace" runat="server" Text="From warehouse:" meta:resourcekey="litDeliveredPlaceResource1"></asp:Localize>
						</td>
						<td>
							<vdms:WarehouseList ID="ddlFromWh" runat="server" DontAutoUseCurrentSealer="True"
								ShowEmptyItem="True" OnSelectedIndexChanged="ddlFromWh_SelectedIndexChanged"
								meta:resourcekey="ddlFromWhResource1" ShowSelectAllItem="False" UseVIdAsValue="False" />
						</td>
					</tr>
					<tr>
						<td>
							<asp:Localize ID="Localize2" runat="server" Text="To dealer:" meta:resourcekey="Localize2Resource1"></asp:Localize>
						</td>
						<td>
							<vdms:DealerList ID="ddlToDl" RemoveRootItem="False" runat="server" AutoPostBack="True"
								ShowEmptyItem="True" OnSelectedIndexChanged="ddlToDl_SelectedIndexChanged" OnDataBound="ddlToDl_SelectedIndexChanged"
								EnabledSaperateByArea="False" EnabledSaperateByDB="False" MergeCode="False" meta:resourcekey="ddlToDlResource1"
								ShowSelectAllItem="False" />
						</td>
					</tr>
					<tr>
						<td>
							<asp:Localize ID="litOrderNumber" runat="server" Text="To warehouse:" meta:resourcekey="litOrderNumberResource1"></asp:Localize>
						</td>
						<td>
							<vdms:WarehouseList DontAutoUseCurrentSealer="True" ID="ddlToWh" runat="server" ShowEmptyItem="True"
								meta:resourcekey="ddlToWhResource1" ShowSelectAllItem="False" UseVIdAsValue="False" />
							<asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="ddlFromWh"
								ControlToValidate="ddlToWh" ValidationGroup="Query" ErrorMessage="Destination warehouse cannot be same with source warehouse!"
								Operator="NotEqual" meta:resourcekey="CompareValidator1Resource1">*</asp:CompareValidator>
						</td>
					</tr>
					<tr>
						<td>
							&nbsp;
						</td>
						<td>
							<asp:Button ID="btnQuery" runat="server" Text="Query" ValidationGroup="Query" OnClick="btnQuery_Click"
								meta:resourcekey="btnQueryResource1" />
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
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>
	<div class="grid">
		<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<vdms:PageGridView ID="gvH" runat="server" DataKeyNames="TransferHeaderId" Caption="Transfer session"
					AutoGenerateColumns="False" OnSelectedIndexChanged="gvH_SelectedIndexChanged"
					meta:resourcekey="gvHResource1">
					<Columns>
						<asp:CommandField SelectText="View detail" ShowSelectButton="True" meta:resourcekey="CommandFieldResource1" />
						<asp:HyperLinkField DataNavigateUrlFields="TransferHeaderId" Target="_blank" DataNavigateUrlFormatString="~/Part/Inventory/PrintTransferForm.aspx?id={0}"
							Text="Print" meta:resourcekey="HyperLinkFieldResource1" />
						<asp:BoundField DataField="TransferDate" HeaderText="TransferDate" DataFormatString="{0:d}"
							SortExpression="TransferDate" meta:resourcekey="BoundFieldResource1" />
						<asp:TemplateField HeaderText="From" SortExpression="FromWarehouseId" meta:resourcekey="TemplateFieldResource1">
							<ItemTemplate>
								<asp:Label ID="Label1" runat="server" Text='<%# EvalAddress(Eval("FromWarehouseId")) %>'
									meta:resourcekey="Label1Resource1"></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="To" SortExpression="ToWarehouseId" meta:resourcekey="TemplateFieldResource2">
							<ItemTemplate>
								<asp:Label ID="Label2" runat="server" Text='<%# EvalAddress(Eval("ToWarehouseId")) %>'
									meta:resourcekey="Label2Resource1"></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" Visible="False"
							meta:resourcekey="BoundFieldResource2" />
						<asp:BoundField DataField="TransferComment" HeaderText="Comment" SortExpression="TransferComment"
							meta:resourcekey="BoundFieldResource3" />
						<asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy"
							meta:resourcekey="BoundFieldResource4" />
					</Columns>
				</vdms:PageGridView>
				<cc1:UpdateProgress runat="server" ID="upg1" AssociatedUpdatePanelID="UpdatePanel1" />
				<vdms:PageGridView ID="gvD" runat="server" Caption="Parts list" DataSourceID="odsDetail"
					AutoGenerateColumns="False" meta:resourcekey="gvDResource1">
					<Columns>
						<asp:BoundField DataField="PartCode" HeaderText="Part Code" SortExpression="PartCode"
							meta:resourcekey="BoundFieldResource5" />
						<asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity"
							meta:resourcekey="BoundFieldResource6" />
						<asp:BoundField DataField="PartComment" HeaderText="Comment" SortExpression="PartComment"
							meta:resourcekey="BoundFieldResource7" />
					</Columns>
				</vdms:PageGridView>
				<asp:ObjectDataSource ID="odsDetail" runat="server" SelectMethod="ViewTransferDetail"
					TypeName="VDMS.II.PartManagement.PartTransferDAO">
					<SelectParameters>
						<asp:ControlParameter ControlID="gvH" Name="thId" PropertyName="SelectedValue" Type="Int64" />
						<asp:Parameter Name="maximumRows" Type="Int32" />
						<asp:Parameter Name="startRowIndex" Type="Int32" />
					</SelectParameters>
				</asp:ObjectDataSource>
				<asp:ObjectDataSource ID="odsTrans" runat="server" EnablePaging="True" SelectMethod="SearchTransferHeaders"
					TypeName="VDMS.II.PartManagement.PartTransferDAO">
					<SelectParameters>
						<asp:ControlParameter ControlID="ddlFromDl" Name="fromD" PropertyName="SelectedValue"
							Type="String" />
						<asp:ControlParameter ControlID="ddlToDl" Name="toD" PropertyName="SelectedValue"
							Type="String" />
						<asp:ControlParameter ControlID="ddlFromWh" Name="fromWH" PropertyName="SelectedValue"
							Type="Int64" />
						<asp:ControlParameter ControlID="ddlToWh" Name="toWH" PropertyName="SelectedValue"
							Type="Int64" />
						<asp:ControlParameter ControlID="txtFromDate" Name="fromDate" PropertyName="Text"
							Type="String" />
						<asp:ControlParameter ControlID="txtToDate" Name="toDate" PropertyName="Text" Type="String" />
						<asp:Parameter Name="status" Type="String" DefaultValue="" />
					</SelectParameters>
				</asp:ObjectDataSource>
			</ContentTemplate>
			<Triggers>
				<asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
			</Triggers>
		</asp:UpdatePanel>
	</div>
</asp:Content>
