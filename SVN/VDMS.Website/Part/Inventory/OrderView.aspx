<%@ Page Title="" Language="C#" MasterPageFile="~/MP/Popup.master" AutoEventWireup="true"
	CodeFile="OrderView.aspx.cs" Inherits="Part_Inventory_OrderView" Theme="Thickbox"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Src="OrderInfo.ascx" TagName="OrderInfo" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="form" style="width: 300px">
		<uc1:OrderInfo ID="OrderInfo1" runat="server" OrderQueryString="id" RedirectURLWhenOrderNull="Order.aspx" />
	</div>
	<div class="grid">
		<vdms:PageGridView ID="gv1" runat="server" AllowPaging="True" DataSourceID="ods1"
			meta:resourcekey="gv1Resource1">
			<Columns>
				<asp:BoundField HeaderText="Number" DataField="LineNumber" meta:resourcekey="BoundFieldResource1" />
				<asp:BoundField HeaderText="Part Code" DataField="PartCode" meta:resourcekey="BoundFieldResource2" />
				<%--<asp:BoundField HeaderText="English Name" DataField="EnglishName" />
				<asp:BoundField HeaderText="Vietnam Name" DataField="VietnamName" />--%>
				<asp:BoundField HeaderText="Quantity" DataField="OrderQuantity" ItemStyle-CssClass="number"
					meta:resourcekey="BoundFieldResource3">
					<ItemStyle CssClass="number"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField HeaderText="Status" DataField="Status" meta:resourcekey="BoundFieldResource4" />
			</Columns>
			<EmptyDataTemplate>
				<asp:Localize ID="Localize1" runat="server" Text="There isn't any rows." meta:resourcekey="Localize1Resource1"></asp:Localize>
			</EmptyDataTemplate>
		</vdms:PageGridView>
	</div>
	<asp:ObjectDataSource ID="ods1" runat="server" TypeName="VDMS.II.PartManagement.Order.OrderDAO"
		SelectMethod="GetAllDetail" SelectCountMethod="GetDetailCount" EnablePaging="True">
		<SelectParameters>
			<asp:QueryStringParameter Name="OrderHeaderId" QueryStringField="id" Type="Int64" />
		</SelectParameters>
	</asp:ObjectDataSource>
</asp:Content>
