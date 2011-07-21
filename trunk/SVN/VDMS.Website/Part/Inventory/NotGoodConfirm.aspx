<%@ Page Title="" Language="C#" MasterPageFile="~/MP/Popup.master" AutoEventWireup="true"
	CodeFile="NotGoodConfirm.aspx.cs" Inherits="Part_Inventory_NotGoodConfirm" Theme="Thickbox"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<asp:Label ID="lblSaveOk" runat="server" SkinID="MessageOk" Visible="False" Text="The order has been sent to VMEP."
		meta:resourcekey="lblSaveOkResource1"></asp:Label>
	<div class="grid">
		<vdms:PageGridView ID="gv1" runat="server" DataSourceID="ods1" meta:resourcekey="gv1Resource1">
			<Columns>
				<asp:BoundField HeaderText="Part Code" DataField="PartCode" meta:resourcekey="BoundFieldResource1" />
				<asp:BoundField HeaderText="English Name" DataField="EnglishName" meta:resourcekey="BoundFieldResource2" />
				<asp:BoundField HeaderText="Vietnam Name" DataField="VietnamName" meta:resourcekey="BoundFieldResource3" />
				<asp:BoundField HeaderText="Quantity" DataField="RequestQuantity" ItemStyle-CssClass="number"
					meta:resourcekey="BoundFieldResource4">
					<ItemStyle CssClass="number"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField HeaderText="Status" DataField="PartStatus" meta:resourcekey="BoundFieldResource5" />
				<asp:BoundField HeaderText="Broken Code" DataField="BrokenCode" meta:resourcekey="BoundFieldResource6" />
			</Columns>
			<EmptyDataTemplate>
				<asp:Literal ID="Literal1" runat="server" Text="There isn't any rows." meta:resourcekey="Literal1Resource1"></asp:Literal>
			</EmptyDataTemplate>
		</vdms:PageGridView>
	</div>
	<div class="button">
		<asp:Button ID="bSend" runat="server" Text="Confirm and Send" OnClick="bSend_Click"
			meta:resourcekey="bSendResource1" />
		<asp:Button ID="bBack" runat="server" Text="Back" CommandName="Cancel" OnClientClick="javascript:self.parent.updated(false);"
			meta:resourcekey="bBackResource1" />
	</div>
	<asp:ObjectDataSource ID="ods1" runat="server" TypeName="VDMS.II.PartManagement.NotGoodDAO"
		SelectMethod="FindAllDetail" SelectCountMethod="GetDetailCount" EnablePaging="True">
		<SelectParameters>
			<asp:QueryStringParameter Name="NGFormHeaderId" QueryStringField="id" Type="Int64" />
		</SelectParameters>
	</asp:ObjectDataSource>
</asp:Content>
