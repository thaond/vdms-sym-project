<%@ Page Title="Not Good Form Edit" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="NotGoodEdit.aspx.cs" Inherits="Part_Inventory_NotGoodEdit"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
	<script type="text/javascript">
		function updated() {
			//  close the popup
			tb_remove();
			//  refresh the update panel so we can view the changes
			$('#<%= this.bt1.ClientID %>').click();
		}
		function showSearch(link) {
			var s = "SearchPart.aspx?";
			s = s + "code=" + $('#<%= this.txtPartCode.ClientID %>').val();
			s = s + "&name=" + $('#<%= this.txtPartName.ClientID %>').val();
			s = s + "&engno=" + $('#<%= this.txtEngineNo.ClientID %>').val();
			s = s + "&model=" + $('#<%= this.ddl3.ClientID %>').val();
			s = s + "&at=NG&target=NG&acc=N&TB_iframe=true&height=320&width=420";
			link.href = s;
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<asp:ObjectDataSource ID="ods2" runat="server" TypeName="VDMS.II.PartManagement.NotGoodManualDAO"
		SelectMethod="FindAll"></asp:ObjectDataSource>
	<div class="form" style="width: 50%; float: left;">
		<table>
			<tr>
				<td>
					<asp:Literal ID="Literal1" runat="server" Text="Part Code:" meta:resourcekey="Literal1Resource1"></asp:Literal>
				</td>
				<td>
					<asp:TextBox ID="txtPartCode" runat="server" Columns="15" meta:resourcekey="txtPartCodeResource1"></asp:TextBox>
				</td>
				<td>
					<asp:Literal ID="Literal2" runat="server" Text="Part Name:" meta:resourcekey="Literal2Resource1"></asp:Literal>
				</td>
				<td>
					<asp:TextBox ID="txtPartName" runat="server" Columns="15" meta:resourcekey="txtPartNameResource1"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Literal ID="Literal3" runat="server" Text="Engine No:" meta:resourcekey="Literal3Resource1"></asp:Literal>
				</td>
				<td>
					<asp:TextBox ID="txtEngineNo" runat="server" Columns="15" meta:resourcekey="txtEngineNoResource1"></asp:TextBox>
				</td>
				<td>
					<asp:Literal ID="Literal4" runat="server" Text="Model:" meta:resourcekey="Literal4Resource1"></asp:Literal>
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
			<tr>
				<td>
					<asp:Literal ID="Literal5" runat="server" Text="Page mode:" meta:resourcekey="Literal5Resource1"></asp:Literal>
				</td>
				<td>
					<asp:LinkButton ID="cmdAddRow" runat="server" Text="Add" OnClick="cmdAddRow_Click"
						meta:resourcekey="cmdAddRowResource1"></asp:LinkButton>
					<asp:DropDownList ID="ddlRowCount" runat="server" meta:resourcekey="ddlRowCountResource1">
						<asp:ListItem Text="5" meta:resourcekey="ListItemResource2"></asp:ListItem>
						<asp:ListItem Text="10" meta:resourcekey="ListItemResource3"></asp:ListItem>
					</asp:DropDownList>
					<asp:Literal ID="Literal8" runat="server" Text="rows." meta:resourcekey="Literal8Resource1"></asp:Literal>
				</td>
				<td>
					<asp:Literal ID="Literal6" runat="server" Text="Rows/table:" meta:resourcekey="Literal6Resource1"></asp:Literal>
				</td>
				<td>
					<asp:DropDownList ID="ddlRows" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRows_SelectedIndexChanged"
						meta:resourcekey="ddlRowsResource1">
						<asp:ListItem Text="5" meta:resourcekey="ListItemResource4"></asp:ListItem>
						<asp:ListItem Text="10" meta:resourcekey="ListItemResource5"></asp:ListItem>
						<asp:ListItem Text="20" meta:resourcekey="ListItemResource6"></asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
		</table>
	</div>
	<div class="help" style="float: right; width: 35%;">
		<ul>
			<li>
				<asp:Literal ID="Literal7" runat="server" Text="Return part to VMEP (optional function – reserved for future use)."
					meta:resourcekey="Literal7Resource1"></asp:Literal></li>
		</ul>
	</div>
	<div style="clear: both;">
	</div>
	<asp:Label ID="lblSaveOk1" runat="server" SkinID="MessageOk" Visible="False" Text="The NGForm has been created successful."
		meta:resourcekey="lblSaveOk1Resource1"></asp:Label>
	<asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<asp:Button ID="bt1" runat="server" Style="display: none" OnClick="Refresh_Click"
				meta:resourcekey="bt1Resource1" />
			<div class="grid" style="clear: both;">
				<vdms:PageGridView ID="gv1" runat="server" DataSourceID="ods2" meta:resourcekey="gv1Resource1">
					<Columns>
						<asp:TemplateField HeaderText="Part Code" meta:resourcekey="TemplateFieldResource1">
							<ItemTemplate>
								<asp:TextBox ID="t1" runat="server" Text='<%# Eval("PartCode") %>' meta:resourcekey="t1Resource2"></asp:TextBox>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField HeaderText="Part Name" DataField="PartName" meta:resourcekey="BoundFieldResource1" />
						<asp:TemplateField HeaderText="Broken code" meta:resourcekey="TemplateFieldResource2">
							<ItemTemplate>
								<asp:DropDownList ID="dllBroken" runat="server" DataSourceID="ods3" DataTextField="BrokenName"
									DataValueField="BrokenCode" Width="100px" meta:resourcekey="dllBrokenResource1">
								</asp:DropDownList>
								<asp:ObjectDataSource ID="ods3" runat="server" TypeName="VDMS.II.PartManagement.Broken"
									SelectMethod="FindAll"></asp:ObjectDataSource>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Return Quantity" meta:resourcekey="TemplateFieldResource3">
							<ItemTemplate>
								<asp:TextBox ID="t2" runat="server" Text='<%# Eval("Quantity") %>' CssClass="number"
									meta:resourcekey="t2Resource1"></asp:TextBox>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Comment" meta:resourcekey="TemplateFieldResource4">
							<ItemTemplate>
								<asp:TextBox ID="t3" runat="server" Text='<%# Eval("Comment") %>' meta:resourcekey="t3Resource1"></asp:TextBox>
								<asp:RequiredFieldValidator ID="rfv3" runat="server" ControlToValidate="t3" SetFocusOnError="True"
									Text="*" ErrorMessage="*" meta:resourcekey="rfv3Resource1"></asp:RequiredFieldValidator>
							</ItemTemplate>
						</asp:TemplateField>
					</Columns>
					<EmptyDataTemplate>
						<asp:Literal ID="Literal1" runat="server" Text="There isn't any rows." meta:resourcekey="Literal1Resource2"></asp:Literal>
					</EmptyDataTemplate>
				</vdms:PageGridView>
			</div>
		</ContentTemplate>
		<Triggers>
			<asp:AsyncPostBackTrigger ControlID="cmdAddRow" EventName="Click" />
			<asp:AsyncPostBackTrigger ControlID="ddlRows" EventName="SelectedIndexChanged" />
		</Triggers>
	</asp:UpdatePanel>
	<div class="button">
		<asp:Button ID="cmdSave" runat="server" Text="Create" OnClick="cmdSave_Click" meta:resourcekey="cmdSaveResource1" />
		<asp:Button ID="cmdSaveAndSend" runat="server" Text="Create and Send" OnClick="cmdSaveAndSend_Click"
			meta:resourcekey="cmdSaveAndSendResource1" />
		<asp:Button ID="bBack" runat="server" Text="Back" CommandName="Cancel" OnClientClick="javascript:location.href='NotGood.aspx'; return false;"
			CausesValidation="False" meta:resourcekey="bBackResource1" />
	</div>
</asp:Content>
