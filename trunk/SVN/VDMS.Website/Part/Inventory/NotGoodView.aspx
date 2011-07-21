<%@ Page Title="" Language="C#" MasterPageFile="~/MP/Popup.master" AutoEventWireup="true"
	CodeFile="NotGoodView.aspx.cs" Inherits="Part_Inventory_NotGoodView" Theme="Thickbox"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<table width="100%">
		<tr>
			<td style="width: 30%">
				<asp:Localize ID="litNG" runat="server" Text="NG Number:" meta:resourcekey="litNGResource1"></asp:Localize>
			</td>
			<td>
				<asp:Label ID="lblNG" runat="server" SkinID="TextField" meta:resourcekey="lblNGResource1"></asp:Label>
			</td>
		</tr>
	</table>
	<asp:Label ID="lblSaveOk" runat="server" SkinID="MessageOk" Visible="False" Text="The data has been saved."
		meta:resourcekey="lblSaveOkResource1"></asp:Label>
	<div class="grid">
		<vdms:PageGridView ID="gv1" runat="server" DataSourceID="ods1" DataKeyNames="NGFormDetailId"
			meta:resourcekey="gv1Resource1">
			<Columns>
				<asp:BoundField HeaderText="Part Code" DataField="PartCode" meta:resourcekey="BoundFieldResource1" />
				<asp:BoundField HeaderText="English Name" DataField="EnglishName" meta:resourcekey="BoundFieldResource2" />
				<asp:BoundField HeaderText="Vietnam Name" DataField="VietnamName" meta:resourcekey="BoundFieldResource3" />
				<asp:BoundField HeaderText="Request" DataField="RequestQuantity" ItemStyle-CssClass="number"
					meta:resourcekey="BoundFieldResource4">
					<ItemStyle CssClass="number"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField HeaderText="Status" DataField="PartStatus" meta:resourcekey="BoundFieldResource5" />
				<asp:BoundField HeaderText="Broken Code" DataField="BrokenCode" meta:resourcekey="BoundFieldResource6" />
				<asp:BoundField HeaderText="Approved" DataField="ApprovedQuantity" ItemStyle-CssClass="number"
					meta:resourcekey="BoundFieldResource7">
					<ItemStyle CssClass="number"></ItemStyle>
				</asp:BoundField>
				<asp:TemplateField HeaderText="Problem Again" meta:resourcekey="TemplateFieldResource1">
					<ItemTemplate>
						<asp:TextBox ID="t1" runat="server" Text='<%# Eval("ProblemAgainQuantity") %>' CssClass="number"
							Columns="4" MaxLength="5" meta:resourcekey="t1Resource1"></asp:TextBox>
						<asp:RangeValidator ID="rv1" runat="server" ControlToValidate="t1" ErrorMessage="*"
							MinimumValue="0" MaximumValue='<%# Eval("RequestQuantity") %>' SetFocusOnError="True"
							meta:resourcekey="rv1Resource1"></asp:RangeValidator>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Passed" meta:resourcekey="TemplateFieldResource2">
					<ItemTemplate>
						<asp:CheckBox ID="c1" runat="server" Checked='<%# (bool)Eval("Passed") %>' meta:resourcekey="c1Resource1" />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Remark" meta:resourcekey="TemplateFieldResource3">
					<ItemTemplate>
						<asp:TextBox ID="t2" runat="server" Text='<%# Eval("TransactionComment") %>' Columns="10"
							meta:resourcekey="t2Resource1"></asp:TextBox>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
			<EmptyDataTemplate>
				<asp:Literal ID="Literal1" runat="server" Text="There isn't any rows." meta:resourcekey="Literal1Resource1"></asp:Literal>
			</EmptyDataTemplate>
		</vdms:PageGridView>
	</div>
	<div class="button">
		<asp:Button ID="bSend" runat="server" Text="Save" OnClick="bSend_Click" meta:resourcekey="bSendResource1" />
		<asp:Button ID="bBack" runat="server" Text="Back" CommandName="Cancel" OnClientClick="javascript:self.parent.tb_remove();"
			meta:resourcekey="bBackResource1" />
	</div>
	<asp:ObjectDataSource ID="ods1" runat="server" TypeName="VDMS.II.PartManagement.NotGoodDAO"
		SelectMethod="FindAllDetail" SelectCountMethod="GetDetailCount" EnablePaging="True">
		<SelectParameters>
			<asp:QueryStringParameter Name="NGFormHeaderId" QueryStringField="id" Type="Int64" />
		</SelectParameters>
	</asp:ObjectDataSource>
</asp:Content>
