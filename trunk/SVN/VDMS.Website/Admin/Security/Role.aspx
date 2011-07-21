<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
	CodeFile="Role.aspx.cs" Inherits="Admin_Security_Role" Title="Manage role is VDMS system"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Src="~/Controls/UpdateProgress.ascx" TagName="UpdateProgress" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
		<ContentTemplate>
			<div id="msg" runat="server">
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
	<asp:UpdatePanel ID="udpRole" UpdateMode="Conditional" runat="server">
		<ContentTemplate>
		    
			<uc1:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="udpRole" runat="server" />
			<div style="float: left;" class="normalBox">
			    <asp:Localize ID="litDealerCode" runat="server" Text="Dealer code:" meta:resourcekey="litDealerCodeResource1"></asp:Localize> <asp:TextBox runat="server" ID="txtdealercode"></asp:TextBox> 
                <asp:Button runat="server" ID="btDealerCode" Text="Role List" 
                    meta:resourcekey="btDealerCodeResource1" onclick="btDealerCode_Click" />
			    <br />
			    <br />
				<div class="boxHeader">
				
					<asp:Literal ID="Literal3" runat="server" Text="Dealers list" meta:resourcekey="Literal3Resource1"></asp:Literal>
				</div>
				<asp:TreeView ID="tvDealer" runat="server" DataSourceID="xmlDataSource" OnTreeNodeDataBound="tvDealer_TreeNodeDataBound"
					OnSelectedNodeChanged="tvDealer_SelectedNodeChanged" meta:resourcekey="tvDealerResource1">
					<SelectedNodeStyle BackColor="#FFFF66" />
					<DataBindings>
						<asp:TreeNodeBinding ValueField="DealerCode" DataMember="MenuItem" TextField="Title"
							meta:resourcekey="TreeNodeBindingResource1" />
					</DataBindings>
				</asp:TreeView>
				<asp:XmlDataSource ID="xmlDataSource" TransformFile="~\xslt\Dealer.xsl" XPath="MenuItems/MenuItem"
					runat="server" />
			</div>
			<asp:Panel ID="pnRoles" runat="server" Style="float: left;" class="normalBox">
				<div class="boxHeader">
					<asp:Literal meta:resourcekey="litboxTitleResource1" ID="Literal1" runat="server"
						Text="Roles in VDMS system"></asp:Literal>
				</div>
				<asp:Label ID="lblError" runat="server" meta:resourcekey="lblErrorResource1"></asp:Label>
				<br />
				<asp:TreeView ID="tvRoles" runat="server" OnSelectedNodeChanged="tvRoles_SelectedNodeChanged"
					DataSourceID="xdsRoles" OnDataBound="tvRoles_DataBound" meta:resourcekey="tvRolesResource1">
					<SelectedNodeStyle BackColor="#FFFF66" />
					<DataBindings>
						<asp:TreeNodeBinding ValueField="RoleIndex" DataMember="MenuItem" TextField="Title"
							meta:resourcekey="TreeNodeBindingResource2" />
					</DataBindings>
				</asp:TreeView>
				<asp:XmlDataSource ID="xdsRoles" TransformFile="~\xslt\Role.xsl" XPath="MenuItems/MenuItem"
					runat="server" />
				<br />
				<asp:Button ID="btnDeleteRole" runat="server" OnClick="btnDeleteRole_Click" Text="Delete current role"
					meta:resourcekey="btnDeleteRoleResource1" />
				<asp:Button ID="btnSetPermission" runat="server" Text="Set permission" />
				<asp:Button ID="btnCreateAdmin" runat="server" OnClick="btnCreateAdmin_Click" Text="Create Administrators" />
			</asp:Panel>
			<asp:PlaceHolder ID="phUserInRole" runat="server" Visible="False">
				<div style="float: left;" class="normalBox">
					<div class="boxHeader">
						<asp:Localize ID="litUserInRole" runat="server" Text="Users in role:" meta:resourcekey="litUserInRoleResource1"></asp:Localize>
					</div>
					<asp:CheckBoxList ID="cblUserInRole" runat="server" RepeatColumns="5" CellSpacing="4"
						meta:resourcekey="cblUserInRoleResource1">
					</asp:CheckBoxList>
					<br />
					<asp:Button runat="server" ID="cmdUpdate" Text="Update" CausesValidation="False"
						SkinID="SubmitButton" OnClick="cmdUpdate_Click" meta:resourcekey="cmdUpdateResource1" />
				</div>
			</asp:PlaceHolder>
			<asp:Panel ID="pnCreateRole" runat="server" Style="float: left;" CssClass="normalBox"
				meta:resourcekey="pnCreateRoleResource1">
				<div class="boxHeader">
					<asp:Literal meta:resourcekey="litCreateNewRoleResource1" ID="Literal2" runat="server"
						Text="Roles in VDMS system"></asp:Literal>
					<asp:Literal meta:resourcekey="txtCurrentRoleResource1" ID="txtCurrentRole" runat="server"
						Text="Roles in VDMS system"></asp:Literal>
				</div>
				<br />
				<asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="CreateRole"
					meta:resourcekey="ValidationSummary1Resource1" />
				<br />
				<table cellpadding="2">
					<tr>
						<td>
							<asp:Localize ID="litNewRole" runat="server" Text="New role:" meta:resourcekey="litNewRoleResource1"></asp:Localize>
						</td>
						<td>
							<asp:TextBox runat="server" ID="txtRole" MaxLength="128" meta:resourcekey="txtRoleResource1"></asp:TextBox>
							<asp:CustomValidator ID="cvRole" runat="server" ControlToValidate="txtRole" ErrorMessage="Role is exist. Please select another."
								OnServerValidate="cvRole_ServerValidate" ValidationGroup="CreateRole" SetFocusOnError="True"
								meta:resourcekey="cvRoleResource1" Text="*"></asp:CustomValidator>
							<asp:RequiredFieldValidator ID="valRequireNewRole" runat="server" ControlToValidate="txtRole"
								SetFocusOnError="True" ErrorMessage="Role name is required." ToolTip="Role name is required."
								ValidationGroup="CreateRole" meta:resourcekey="valRequireNewRoleResource1" Text="*"></asp:RequiredFieldValidator>
						</td>
					</tr>
					<tr>
						<td>
						</td>
						<td>
							<asp:Button runat="server" ID="cmdCreateRole" Text="Create" ValidationGroup="CreateRole"
								SkinID="SubmitButton" OnClick="cmdCreateRole_Click" meta:resourcekey="cmdCreateRoleResource1" />
						</td>
					</tr>
				</table>
			</asp:Panel>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
