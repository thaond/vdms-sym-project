<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OrderInfo.ascx.cs" Inherits="Part_Inventory_OrderInfo" %>
<table width="100%">
	<tr>
		<td style="width: 30%">
			<asp:Localize ID="litDealer" runat="server" Text="Delivered Dealer:" meta:resourcekey="litDealerResource1"></asp:Localize>
		</td>
		<td>
			<asp:Label ID="lblDealer" runat="server" SkinID="TextField" meta:resourcekey="lblDealerResource1"></asp:Label>
		</td>
	</tr>
	<tr>
		<td>
			<asp:Localize ID="litWarehouse" runat="server" Text="Delivered Warehouse:" meta:resourcekey="litWarehouseResource1"></asp:Localize>
		</td>
		<td>
			<asp:Label ID="lblWarehouse" runat="server" SkinID="TextField" meta:resourcekey="lblWarehouseResource1"></asp:Label>
		</td>
	</tr>
	<tr>
		<td>
			<asp:Localize ID="litOrderDate" runat="server" Text="Order Date:" meta:resourcekey="litOrderDateResource1"></asp:Localize>
		</td>
		<td>
			<asp:Label ID="lblOrderDate" runat="server" SkinID="TextField" meta:resourcekey="lblOrderDateResource1"></asp:Label>
		</td>
	</tr>
	<tr>
		<td>
			<asp:Localize ID="litOrderType" runat="server" Text="Order Type:" meta:resourcekey="litOrderTypeResource1"></asp:Localize>
		</td>
		<td>
			<asp:Label ID="lblOT" runat="server" Font-Bold="True" Text="Normal" meta:resourcekey="lblOTResource1"></asp:Label>
		</td>
	</tr>
</table>
