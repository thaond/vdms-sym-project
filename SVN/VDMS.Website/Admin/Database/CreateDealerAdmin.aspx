<%@ Page Title="" Language="C#" MasterPageFile="~/MP/Popup.master" AutoEventWireup="true"
	CodeFile="CreateDealerAdmin.aspx.cs" Inherits="Admin_Database_CreateDealerAdmin"
	Theme="Thickbox" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="form">
		<table cellpadding="2" cellspacing="2" border="0" width="100%">
			<tr>
				<td style="width: 35%">
					<asp:Localize ID="litDealerCode" runat="server" Text="Dealer Code:" meta:resourcekey="litUsernameResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox runat="server" ID="txtDealer" ReadOnly="True" 
                        meta:resourcekey="txtDealerResource1"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td style="width: 25%">
					<asp:Localize ID="litUsername" runat="server" Text="UserName:" 
                        meta:resourcekey="litUsernameResource2"></asp:Localize>
				</td>
				<td>
					<asp:TextBox runat="server" ID="txtUserName" Text="admin" ReadOnly="True" 
                        meta:resourcekey="txtUserNameResource1"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litPassword" runat="server" Text="Password:" 
                        meta:resourcekey="litPasswordResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox runat="server" ID="txtPassword" 
                        meta:resourcekey="txtPasswordResource1"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
				</td>
				<td>
					<asp:Button ID="cmdCreate" runat="server" Text="Create" 
                        OnClick="cmdCreate_Click" meta:resourcekey="cmdCreateResource1" />
					<asp:Button ID="cmdCancel" runat="server" Text="Cancel" 
                        OnClientClick="javascript:self.parent.updated(false);" 
                        meta:resourcekey="cmdCancelResource1" />
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<asp:Label ID="ActionOk" runat="server" Text="Action completed. New user has been created."
						Visible="False" meta:resourcekey="ActionOkResource1"></asp:Label>
					<asp:Label ID="ActionFaild" runat="server" ForeColor="Red" Text="Acion faild. No user created."
						Visible="False" meta:resourcekey="ActionFaildResource1"></asp:Label>
				</td>
			</tr>
		</table>
	</div>
</asp:Content>
