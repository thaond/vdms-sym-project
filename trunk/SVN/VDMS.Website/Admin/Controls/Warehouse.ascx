<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Warehouse.ascx.cs" Inherits="Admin_Controls_Warehouse" %>
<table cellpadding="1" cellspacing="1" border="0" style="width: 100%;" >
	<tr>
		<td style="width: 30%;">
			<asp:Image ID="i1" runat="server" SkinID="RequireField" 
                meta:resourcekey="i1Resource1" />
			<asp:Localize ID="litCode" runat="server" Text="Code:" 
                meta:resourcekey="litCodeResource1"></asp:Localize>
		</td>
		<td>
			<asp:TextBox ID="tb1" runat="server" style="width: 90%;" 
                meta:resourcekey="tb1Resource1"></asp:TextBox>
			<asp:RequiredFieldValidator ID="rfv1" runat="server" 
                ErrorMessage="Warehouse code cannot be empty!" ControlToValidate="tb1"
				ValidationGroup="Save" SetFocusOnError="True" meta:resourcekey="rfv1Resource1">*</asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr>
		<td>
			<asp:Image ID="image3" runat="server" SkinID="RequireField" 
                meta:resourcekey="image3Resource1" />
			<asp:Localize ID="litAddress" runat="server" Text="Address:" 
                meta:resourcekey="litAddressResource1"></asp:Localize>
		</td>
		<td>
			<asp:TextBox ID="tb3" runat="server" style="width: 90%;" 
                meta:resourcekey="tb3Resource1"></asp:TextBox>
			<asp:RequiredFieldValidator ID="rfv3" runat="server" 
                ErrorMessage="Warehouse address cannot be empty!" ControlToValidate="tb3"
				ValidationGroup="Save" SetFocusOnError="True" meta:resourcekey="rfv3Resource1">*</asp:RequiredFieldValidator>
		</td>
	</tr>
</table>
