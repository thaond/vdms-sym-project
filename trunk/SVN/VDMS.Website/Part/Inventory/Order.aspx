<%@ Page Title="Order Parts" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="Order.aspx.cs" Inherits="Part_Inventory_Order"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register TagPrefix="cc1" TagName="UpdateProgress" Src="~/Controls/UpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
	<script type="text/javascript">
		function updated(refresh) {
			//  close the popup
			tb_remove();
			//  refresh the update panel so we can view the changes
			if (refresh) window.location.reload(true);
		}
		function checkduplicate(id) {
			//  close the popup
			tb_remove();
			//  refresh the update panel so we can view the changes
			window.location = "OrderEdit.aspx?action=edit&checkduplicate=true&id=" + id;
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="form" style="width: 50%; float: left;">
		<table width="100%">
			<tr>
				<td>
					<asp:Localize ID="litOrderDate" runat="server" Text="Order Date:" meta:resourcekey="litOrderDateResource1"></asp:Localize>
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
					<asp:Localize ID="litDealer" runat="server" Text="Delivered Dealer:" meta:resourcekey="litDealerResource1"></asp:Localize>
				</td>
				<td>
					<vdms:DealerList ID="ddlDealer" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
						OnDataBound="ddlDealer_DataBound" OnSelectedIndexChanged="ddlDealer_SelectedIndexChanged"
						EnabledSaperateByArea="False" EnabledSaperateByDB="False" MergeCode="False" meta:resourcekey="ddlDealerResource1"
						RemoveRootItem="False" ShowEmptyItem="False" ShowSelectAllItem="False">
					</vdms:DealerList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litWarehouse" runat="server" Text="Delivered Warehouse:" meta:resourcekey="litWarehouseResource1"></asp:Localize>
				</td>
				<td>
					<asp:UpdatePanel ID="up1" runat="server">
						<ContentTemplate>
							<vdms:WarehouseList ID="ddlWH" runat="server" ShowSelectAllItem="False" DontAutoUseCurrentSealer="False"
								meta:resourcekey="ddlWHResource1" ShowEmptyItem="False" UseVIdAsValue="False">
							</vdms:WarehouseList>
							<cc1:UpdateProgress ID="upg1" runat="server" />
						</ContentTemplate>
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="ddlDealer" EventName="SelectedIndexChanged" />
						</Triggers>
					</asp:UpdatePanel>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litOrderNumber" runat="server" Text="Order Number:" meta:resourcekey="litOrderNumberResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="tbON" runat="server" MaxLength="30" Width="180px" meta:resourcekey="tbONResource1"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litStatus" runat="server" Text="Status:" meta:resourcekey="litStatusResource1"></asp:Localize>
				</td>
				<td>
					<asp:DropDownList ID="ddlS" runat="server" Width="180px" meta:resourcekey="ddlSResource1">
						<asp:ListItem Selected="True" Text="All" Value="" meta:resourcekey="ListItemResource1"></asp:ListItem>
						<asp:ListItem Text="OP: Open, not sent" Value="OP" meta:resourcekey="ListItemResource2"></asp:ListItem>
						<asp:ListItem Text="SN: Sent, sent to VMEP" Value="SN" meta:resourcekey="ListItemResource3"></asp:ListItem>
						<asp:ListItem Text="CF: Confirm by VMEP" Value="CF" meta:resourcekey="ListItemResource4"></asp:ListItem>
						<asp:ListItem Text="NC: Normal Closed" Value="NC" meta:resourcekey="ListItemResource5"></asp:ListItem>
						<asp:ListItem Text="RO: Order re-open" Value="RO" meta:resourcekey="ListItemResource6"></asp:ListItem>
						<asp:ListItem Text="AC: Order abnormal closed" Value="AC" meta:resourcekey="ListItemResource7"></asp:ListItem>
						<asp:ListItem Text="VD: Order void by VMEP" Value="VD" meta:resourcekey="ListItemResource8"></asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litOrderType" runat="server" Text="Order Type:" meta:resourcekey="litOrderTypeResource1"></asp:Localize>
				</td>
				<td>
					<asp:DropDownList ID="ddlOT" runat="server" Width="160px" meta:resourcekey="ddlOTResource1">
						<asp:ListItem Selected="True" Text="All" Value="" meta:resourcekey="ListItemResource9"></asp:ListItem>
						<asp:ListItem Text="Normal Order" Value="N" meta:resourcekey="ListItemResource10"></asp:ListItem>
						<asp:ListItem Text="Sub Order" Value="S" meta:resourcekey="ListItemResource11"></asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
				</td>
				<td>
					<asp:Button ID="cmdQuery" runat="server" Text="Query" OnClick="cmdQuery_Click" meta:resourcekey="cmdQueryResource1" />
					<asp:Button ID="cmdAddNew" runat="server" Text="Add new Order" OnClientClick="javascript:location.href='OrderEdit.aspx?action=create'; return false;"
						meta:resourcekey="cmdAddNewResource1" />
				</td>
			</tr>
		</table>
	</div>
	<div class="help" style="float: right; width: 35%;">
		<ul>
			<li>
				<asp:Localize ID="lh1" runat="server" Text="You can filter the result by combine the input condition."
					meta:resourcekey="lh1Resource1"></asp:Localize>
			</li>
			<li>
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
			</li>
		</ul>
	</div>
	<br />
	<div class="grid" style="clear: both;">
		<vdms:PageGridView ID="gv" runat="server" OnRowDataBound="gv_RowDataBound" DataSourceID="odsOrderList"
			DataKeyNames="OrderHeaderId" AllowPaging="True" meta:resourcekey="gvResource1">
			<Columns>
				<asp:TemplateField meta:resourcekey="TemplateFieldResource1">
					<ItemTemplate>
						<asp:HyperLink ID="h1" runat="server" Text="Edit" meta:resourcekey="h1Resource2"></asp:HyperLink>
						<asp:LinkButton ID="h2" runat="server" Text="Delete" CommandName="Delete" OnClientClick="if(!confirm(SysMsg[0])) return false;"
							meta:resourcekey="h2Resource2"></asp:LinkButton>
						<asp:HyperLink ID="h3" runat="server" Text="View" class="thickbox" meta:resourcekey="h3Resource2"></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:BoundField HeaderText="Order Date" DataField="OrderDate" meta:resourcekey="BoundFieldResource1" />
				<asp:BoundField HeaderText="Delivered Place" DataField="Address" meta:resourcekey="BoundFieldResource2" />
				<asp:BoundField HeaderText="Order Number" DataField="TipTopNumber" meta:resourcekey="BoundFieldResource3" />
				<asp:BoundField HeaderText="Confirm Date" DataField="ConfirmDate" DataFormatString="{0:d}"
					meta:resourcekey="BoundFieldResource4" />
				<asp:BoundField HeaderText="Status" DataField="Status" meta:resourcekey="BoundFieldResource5" />
				<asp:BoundField HeaderText="Created by" DataField="CreatedBy" meta:resourcekey="BoundFieldResource6" />
				<asp:HyperLinkField Text="Confirm and Send" ControlStyle-CssClass="thickbox" DataNavigateUrlFields="OrderHeaderId"
					DataNavigateUrlFormatString="OrderConfirm.aspx?id={0}&TB_iframe=true&height=320&width=420"
					meta:resourcekey="HyperLinkFieldResource1">
					<ControlStyle CssClass="thickbox"></ControlStyle>
				</asp:HyperLinkField>
				<asp:HyperLinkField DataNavigateUrlFields="OrderHeaderId" Target="_blank" DataNavigateUrlFormatString="~/Part/Inventory/PrintOrderForm.aspx?id={0}"
					ShowHeader="False" Text="Print" meta:resourcekey="HyperLinkFieldResource2" />
			</Columns>
			<EmptyDataTemplate>
				<asp:Localize ID="Localize1" runat="server" meta:resourcekey="Localize1Resource1"
					Text="There isn't any rows."></asp:Localize>
			</EmptyDataTemplate>
		</vdms:PageGridView>
		<asp:ObjectDataSource ID="odsOrderList" runat="server" EnablePaging="True" TypeName="VDMS.II.PartManagement.Order.OrderDAO"
			SelectMethod="FindAll" SelectCountMethod="GetCount" DeleteMethod="Delete">
			<SelectParameters>
				<asp:ControlParameter ControlID="txtFromDate" Type="String" PropertyName="Text" Name="fromDate" />
				<asp:ControlParameter ControlID="txtToDate" Type="String" PropertyName="Text" Name="toDate" />
				<asp:ControlParameter ControlID="ddlDealer" Type="String" PropertyName="SelectedValue"
					Name="dealerCode" />
				<asp:ControlParameter ControlID="ddlWH" Type="String" PropertyName="SelectedValue"
					Name="warehouseCode" />
				<asp:ControlParameter ControlID="tbON" Type="String" PropertyName="Text" Name="orderNumber" />
				<asp:ControlParameter ControlID="ddlS" Type="String" PropertyName="SelectedValue"
					Name="status" />
				<asp:ControlParameter ControlID="ddlOT" Type="String" PropertyName="SelectedValue"
					Name="orderType" />
			</SelectParameters>
		</asp:ObjectDataSource>
	</div>
</asp:Content>
