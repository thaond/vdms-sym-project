<%@ Page Title="Warehouse Stock Transfer" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="StockTransfer.aspx.cs" Inherits="Part_Inventory_StockTransfer"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register TagName="up" TagPrefix="cc1" Src="~/Controls/UpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">

	<script type="text/javascript">
		function updated() {
			//  close the popup
			tb_remove();

			//  refresh the update panel so we can view the changes
			$('#<%= this.btnPartInserted.ClientID %>').click();
		}

		function showSearch(link) {
			var s = "SearchPart.aspx?";
			s = s + "code=" + $('#<%= this.txtPartCode.ClientID %>').val();
			s = s + "&name=" + $('#<%= this.txtPartName.ClientID %>').val();
			s = s + "&engno=" + $('#<%= this.txtEngineNo.ClientID %>').val();
			s = s + "&target=ST";
			s = s + "&at=ST";
			s = s + "&wh=" + $('#<%= this.ddlFromWh.ClientID %>').val(); ;
			s = s + "&tgKey=" + '<%= this.PageKey %>';
			s = s + "&model=" + $('#<%= this.ddl3.ClientID %>').val();
			s = s + "&TB_iframe=true&height=320&width=420";
			link.href = s;
		}
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<asp:UpdatePanel runat="server" ID="udpMsg">
		<ContentTemplate>
			<div runat="server" id="msg">
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
	<cc1:up runat="server" ID="upAll" />
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<asp:Button ID="btnPartInserted" runat="server" Text="Button" CssClass="hidden" OnClick="btnPartInserted_Click"
				meta:resourcekey="btnPartInsertedResource1" />
		</ContentTemplate>
	</asp:UpdatePanel>
	<div class="form" style="width: 50%; float: left;">
		<asp:ValidationSummary ID="ValidationSummary1" CssClass="error" runat="server" ValidationGroup="Save"
			meta:resourcekey="ValidationSummary1Resource1" />
		<table width="100%">
			<tr>
				<td>
					<asp:Localize ID="litOrderDate" runat="server" Text="Transfer Date:" meta:resourcekey="litOrderDateResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="txtTranferDate" runat="server" Width="150px" meta:resourcekey="txtTranferDateResource1"></asp:TextBox>
					<asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
						meta:resourcekey="ibFromDateResource1" />
					<ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtTranferDate"
						Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
						CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
						CultureTimePlaceholder="" Enabled="True">
					</ajaxToolkit:MaskedEditExtender>
					<ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtTranferDate"
						PopupButtonID="ibFromDate" Enabled="True">
					</ajaxToolkit:CalendarExtender>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="Localize1" runat="server" Text="From dealer:" meta:resourcekey="Localize1Resource1"></asp:Localize>
				</td>
				<td>
					<vdms:DealerList ID="ddlFromDl" RemoveRootItem="False" runat="server" AutoPostBack="True"
						OnSelectedIndexChanged="ddlFromDl_SelectedIndexChanged" OnDataBound="ddlFromDl_SelectedIndexChanged"
						EnabledSaperateByArea="False" EnabledSaperateByDB="False" MergeCode="False" meta:resourcekey="ddlFromDlResource1"
						ShowEmptyItem="False" ShowSelectAllItem="False" />
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litDeliveredPlace" runat="server" Text="From warehouse:" meta:resourcekey="litDeliveredPlaceResource1"></asp:Localize>
				</td>
				<td>
					<vdms:WarehouseList ID="ddlFromWh" runat="server" OnSelectedIndexChanged="ddlFromWh_SelectedIndexChanged"
						DontAutoUseCurrentSealer="False" meta:resourcekey="ddlFromWhResource1" ShowEmptyItem="False"
						ShowSelectAllItem="False" UseVIdAsValue="False" />
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="Localize2" runat="server" Text="To dealer:" meta:resourcekey="Localize2Resource1"></asp:Localize>
				</td>
				<td>
					<vdms:DealerList ID="ddlToDl" RemoveRootItem="False" runat="server" AutoPostBack="True"
						OnSelectedIndexChanged="ddlToDl_SelectedIndexChanged" OnDataBound="ddlToDl_SelectedIndexChanged"
						EnabledSaperateByArea="False" EnabledSaperateByDB="False" MergeCode="False" meta:resourcekey="ddlToDlResource1"
						ShowEmptyItem="False" ShowSelectAllItem="False" />
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litOrderNumber" runat="server" Text="To warehouse:" meta:resourcekey="litOrderNumberResource1"></asp:Localize>
				</td>
				<td>
					<vdms:WarehouseList ID="ddlToWh" runat="server" DontAutoUseCurrentSealer="False"
						meta:resourcekey="ddlToWhResource1" ShowEmptyItem="False" ShowSelectAllItem="False"
						UseVIdAsValue="False" />
					<asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="ddlFromWh"
						ControlToValidate="ddlToWh" ValidationGroup="Save" ErrorMessage="Destination warehouse cannot be same with source warehouse!"
						Operator="NotEqual" meta:resourcekey="CompareValidator1Resource1">*</asp:CompareValidator>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litStatus" runat="server" Text="Comment:" meta:resourcekey="litStatusResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="txtComment" runat="server" Columns="30" MaxLength="512" meta:resourcekey="txtCommentResource1"></asp:TextBox>
				</td>
			</tr>
		</table>
	</div>
	<div class="help" style="float: right; width: 35%;">
		<ul>
			<li>
				<asp:Literal ID="Literal4" runat="server" Text="Transfer parts between dealers and stores."
					meta:resourcekey="Literal4Resource1"></asp:Literal>
			</li>
		</ul>
	</div>
	<div style="clear: both;">
	</div>
	<br />
	<div class="form">
		<table>
			<tr>
				<td>
					<asp:Literal ID="Literal5" runat="server" Text="Part Code:" meta:resourcekey="Literal5Resource1"></asp:Literal>
				</td>
				<td>
					<asp:TextBox ID="txtPartCode" runat="server" Columns="15" meta:resourcekey="txtPartCodeResource1"></asp:TextBox>
				</td>
				<td>
					<asp:Literal ID="Literal6" runat="server" Text="Part Name:" meta:resourcekey="Literal6Resource1"></asp:Literal>
				</td>
				<td>
					<asp:TextBox ID="txtPartName" runat="server" Columns="15" meta:resourcekey="txtPartNameResource1"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Literal ID="Literal7" runat="server" Text="Engine No:" meta:resourcekey="Literal7Resource1"></asp:Literal>
				</td>
				<td>
					<asp:TextBox ID="txtEngineNo" runat="server" Columns="15" meta:resourcekey="txtEngineNoResource1"></asp:TextBox>
				</td>
				<td>
					<asp:Literal ID="Literal8" runat="server" Text="Model:" meta:resourcekey="Literal8Resource1"></asp:Literal>
				</td>
				<td>
					<asp:DropDownList ID="ddl3" runat="server" Width="115px" DataSourceID="odsModel"
						AppendDataBoundItems="True" DataTextField="model" meta:resourcekey="ddl3Resource1">
						<asp:ListItem Text="-+-" Value="" meta:resourcekey="ListItemResource1"></asp:ListItem>
					</asp:DropDownList>
					<asp:ObjectDataSource ID="odsModel" runat="server" TypeName="VDMS.II.PartManagement.PartDAO"
						SelectMethod="GetModelList"></asp:ObjectDataSource>
				</td>
			</tr>
			<tr>
				<td>
				</td>
				<td colspan="3">
					<asp:HyperLink ID="cmdSearch" runat="server" Text="Search Part" class="thickbox"
						title="Search Part" onclick="javascript:showSearch(this)" href="#" meta:resourcekey="cmdSearchResource1"></asp:HyperLink>
				</td>
			</tr>
		</table>
		<asp:UpdatePanel ID="udpParts" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<table cellpadding="0" cellspacing="4" border="0">
					<tr>
						<td>
							<asp:Literal ID="Literal1" runat="server" Text="Excel mode:" meta:resourcekey="Literal1Resource1"></asp:Literal>
						</td>
						<td>
							<asp:LinkButton ID="cmdAddRow" runat="server" OnClick="cmdAddRow_Click" Text="Add"
								meta:resourcekey="cmdAddRowResource1"></asp:LinkButton>
							<asp:DropDownList ID="ddlRowCount" runat="server" meta:resourcekey="ddlRowCountResource1">
								<asp:ListItem Text="5" meta:resourcekey="ListItemResource2"></asp:ListItem>
								<asp:ListItem Text="10" meta:resourcekey="ListItemResource3"></asp:ListItem>
							</asp:DropDownList>
							<asp:Literal ID="Literal2" runat="server" Text="Rows. " meta:resourcekey="Literal2Resource1"></asp:Literal>
						</td>
						<td>
							<asp:Literal ID="Literal3" runat="server" Text="Rows/table:" meta:resourcekey="Literal3Resource1"></asp:Literal>
						</td>
						<td>
							<asp:DropDownList ID="ddlRows" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRows_SelectedIndexChanged"
								meta:resourcekey="ddlRowsResource1">
								<asp:ListItem Text="5" meta:resourcekey="ListItemResource4"></asp:ListItem>
								<asp:ListItem Selected="True" Text="10" meta:resourcekey="ListItemResource5"></asp:ListItem>
								<asp:ListItem Text="20" meta:resourcekey="ListItemResource6"></asp:ListItem>
							</asp:DropDownList>
						</td>
					</tr>
				</table>
				<div class="grid">
					<vdms:PageGridView ID="gv1" AllowPaging="True" runat="server" AutoGenerateColumns="False"
						DataSourceID="odsSTParts" OnRowDataBound="gv1_RowDataBound" meta:resourcekey="gv1Resource1">
						<Columns>
							<asp:TemplateField HeaderText="Part No" meta:resourcekey="TemplateFieldResource1">
								<ItemTemplate>
									<asp:TextBox ID="txtPartNo" runat="server" PartKey='<%#Eval("PartKey") %>' Text='<%# Eval("PartNo") %>'
										OnTextChanged="UpdatePartInfo" meta:resourcekey="txtPartNoResource1"></asp:TextBox>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:BoundField HeaderText="Description" DataField="Part Name" Visible="False" meta:resourcekey="BoundFieldResource1" />
							<asp:TemplateField HeaderText="Type" meta:resourcekey="TemplateFieldResource2">
								<ItemTemplate>
									<%#Eval("PartType") %>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:BoundField HeaderText="On hand quantity" DataField="CurrentQuantity" meta:resourcekey="BoundFieldResource2" />
							<asp:TemplateField HeaderText="Transfer Quantity" meta:resourcekey="TemplateFieldResource3">
								<ItemTemplate>
									<asp:TextBox ID="txtTransferQuantity" runat="server" OnTextChanged="UpdatePartInfo"
										Text='<%# Eval("TransferQuantity") %>' meta:resourcekey="txtTransferQuantityResource1"></asp:TextBox>
									<ajaxToolkit:FilteredTextBoxExtender ID="txtTransferQuantity_FilteredTextBoxExtender"
										runat="server" TargetControlID="txtTransferQuantity" FilterType="Numbers" Enabled="True">
									</ajaxToolkit:FilteredTextBoxExtender>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Confirm quantity" Visible="False" meta:resourcekey="TemplateFieldResource4">
								<ItemTemplate>
									<asp:TextBox ID="txtConfirmQuantity" runat="server" OnTextChanged="UpdatePartInfo"
										meta:resourcekey="txtConfirmQuantityResource1"></asp:TextBox>
									<ajaxToolkit:FilteredTextBoxExtender ID="txtConfirmQuantity_FilteredTextBoxExtender"
										runat="server" TargetControlID="txtConfirmQuantity" FilterType="Numbers" Enabled="True">
									</ajaxToolkit:FilteredTextBoxExtender>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Remark" Visible="False" meta:resourcekey="TemplateFieldResource5">
								<ItemTemplate>
									<asp:TextBox Width="100%" ID="txtRemark" runat="server" Text='<%# Eval("Remark") %>'
										OnTextChanged="UpdatePartInfo" meta:resourcekey="txtRemarkResource1"></asp:TextBox>
								</ItemTemplate>
								<ItemStyle Width="250px" />
							</asp:TemplateField>
						</Columns>
						<EmptyDataTemplate>
							<asp:Literal ID="Literal5" runat="server" Text="There isn't any rows." meta:resourcekey="Literal5Resource2"></asp:Literal>
						</EmptyDataTemplate>
					</vdms:PageGridView>
					<asp:ObjectDataSource ID="odsSTParts" runat="server" SelectMethod="FindAll" EnablePaging="True"
						SelectCountMethod="CountParts" TypeName="VDMS.II.PartManagement.PartTransferDAO">
						<DeleteParameters>
							<asp:Parameter Name="PartKey" Type="String" />
						</DeleteParameters>
						<SelectParameters>
							<asp:Parameter Name="key" Type="String" />
						</SelectParameters>
					</asp:ObjectDataSource>
				</div>
				<div class="button">
					<asp:Button ID="cmdTransfer" runat="server" Text="Confirm" ValidationGroup="Save"
						OnClick="cmdTransfer_Click" meta:resourcekey="cmdTransferResource1" />
					<asp:Button ID="cmdPrint" runat="server" Text="Print" ValidationGroup="Save" OnClick="cmdPrint_Click"
						meta:resourcekey="cmdPrintResource1" />
				</div>
			</ContentTemplate>
			<Triggers>
				<asp:AsyncPostBackTrigger ControlID="btnPartInserted" EventName="Click" />
			</Triggers>
		</asp:UpdatePanel>
	</div>
</asp:Content>
