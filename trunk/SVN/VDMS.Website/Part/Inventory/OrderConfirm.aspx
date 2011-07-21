<%@ Page Title="Confirm and Send" Language="C#" MasterPageFile="~/MP/Popup.master"
	Theme="Thickbox" AutoEventWireup="true" CodeFile="OrderConfirm.aspx.cs" Inherits="Part_Inventory_ConfirmOrder"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Src="OrderInfo.ascx" TagName="OrderInfo" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="form" style="width: 300px">
		<uc1:OrderInfo ID="OrderInfo1" runat="server" OrderQueryString="id" RedirectURLWhenOrderNull="Order.aspx" />
	</div>
	<asp:Label ID="lblSaveOk" runat="server" SkinID="MessageOk" Visible="False" Text="The order has been sent to VMEP."
		meta:resourcekey="lblSaveOkResource1"></asp:Label>
	<asp:Label ID="lblOrderDateSet" runat="server" SkinID="MessageError" Visible="False"
		Text="Your dealer not in Order Date now. The order cannot be sent. Please try again later"
		meta:resourcekey="lblOrderDateSetResource1"></asp:Label>
	<asp:Label ID="lblOrderDuplicate" runat="server" SkinID="MessageError" Visible="False"
		Text="Some part in this order already exist in previous order that has been sent but not comfirm. The order cannot be sent"
		meta:resourcekey="lblOrderDuplicateResource1"></asp:Label>
	<asp:HyperLink ID="hlEdit" runat="server" Text="Modify order" Visible="false" meta:resourcekey="hlEditResource1"></asp:HyperLink>
	<div class="grid">
		<vdms:PageGridView ID="gv1" runat="server" AllowPaging="True" DataSourceID="ods1"
			meta:resourcekey="gv1Resource1">
			<Columns>
				<asp:BoundField HeaderText="Number" DataField="LineNumber" meta:resourcekey="BoundFieldResource1" />
				<asp:BoundField HeaderText="Part No" DataField="PartCode" meta:resourcekey="BoundFieldResource2" />
				<asp:BoundField HeaderText="Description" meta:resourcekey="BoundFieldResource3" />
				<asp:BoundField HeaderText="Quantity" DataField="OrderQuantity" ItemStyle-CssClass="number"
					meta:resourcekey="BoundFieldResource4">
					<ItemStyle CssClass="number"></ItemStyle>
				</asp:BoundField>
			</Columns>
			<EmptyDataTemplate>
				<asp:Localize ID="Localize1" runat="server" Text="There isn't any rows." meta:resourcekey="Localize1Resource1"></asp:Localize>
			</EmptyDataTemplate>
		</vdms:PageGridView>
	</div>
	<div class="button">
		<asp:Button ID="bSend" runat="server" Text="Confirm and Send" OnClick="bSend_Click"
			meta:resourcekey="bSendResource1" />
		<asp:Button ID="bBack" runat="server" Text="Back" CommandName="Cancel" OnClientClick="javascript:self.parent.updated(false);"
			meta:resourcekey="bBackResource1" />
	</div>
	<asp:ObjectDataSource ID="ods1" runat="server" TypeName="VDMS.II.PartManagement.Order.OrderDAO"
		SelectMethod="GetAllDetail" SelectCountMethod="GetDetailCount" EnablePaging="True">
		<SelectParameters>
			<asp:QueryStringParameter Name="OrderHeaderId" QueryStringField="id" Type="Int64" />
		</SelectParameters>
	</asp:ObjectDataSource>
</asp:Content>
