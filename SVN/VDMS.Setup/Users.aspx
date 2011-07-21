<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
	CodeFile="Users.aspx.cs" Inherits="Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div style="padding: 10px 0;">
		<div>
			<p>
				This sample demonstrates using of Membership, Roles, and Profile Providers.</p>
		</div>
		<hr />
		<div>
			<asp:MultiView ID="multiView" runat="server" ActiveViewIndex="0">
				<asp:View ID="viewMain" runat="server">
					<div class="paragraph">
						<b>Existing users:</b>
					</div>
					<div class="paragraph">
						<asp:GridView ID="gridUsers" runat="server" AutoGenerateColumns="false" CellPadding="3"
							CellSpacing="0" OnRowCommand="gridUsers_RowCommand">
							<EmptyDataTemplate>
								No users.
							</EmptyDataTemplate>
							<HeaderStyle BackColor="DarkGray" />
							<Columns>
								<asp:BoundField DataField="UserName" HeaderText="User Name" ItemStyle-HorizontalAlign="Center" />
								<asp:TemplateField>
									<ItemTemplate>
										<asp:LinkButton ID="lnkEditProfile" runat="server" CommandName="EditProfile" CommandArgument='<%# Eval("UserName") %>'
											CausesValidation="false">Edit profile</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField>
									<ItemTemplate>
										<asp:LinkButton ID="lnkDeleteUser" runat="server" CommandName="DeleteUser" CommandArgument='<%# Eval("UserName") %>'
											CausesValidation="false">Delete user</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField>
									<ItemTemplate>
										<asp:LinkButton ID="lnkEditRoles" runat="server" CommandName="EditRoles" CommandArgument='<%# Eval("UserName") %>'
											CausesValidation="false">Edit roles</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateField>
							</Columns>
						</asp:GridView>
					</div>
					<div class="paragraph">
						<asp:LinkButton ID="lnkCreateDefaultUsers" runat="server" OnClick="lnkCreateDefaultUsers_Click"
							CausesValidation="false">Create default users and roles</asp:LinkButton>
						<asp:Label ID="lbCreationStatus" runat="server" ForeColor="Gray"></asp:Label>
					</div>
					<div class="paragraph">
						<asp:LinkButton ID="lnkAddUser" runat="server" OnClick="lnkAddUser_Click" CausesValidation="false">Add new user</asp:LinkButton>
					</div>
					<hr />
					<div class="paragraph">
						<b>Existing roles:</b>
					</div>
					<div class="paragraph">
						<asp:ListBox ID="lbAllRoles" runat="server" Height="200px" Width="270px"></asp:ListBox>
					</div>
					<div class="paragraph">
						<b>Add role:</b>
						<asp:TextBox ID="tbNewRoleName" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
						<asp:RequiredFieldValidator ID="reqNewRoleName" runat="server" ControlToValidate="tbNewRoleName">*</asp:RequiredFieldValidator>
						<asp:LinkButton ID="lnkAddNewRole" runat="server" OnClick="lnkAddNewRole_Click">Add </asp:LinkButton>
					</div>
				</asp:View>
				<asp:View ID="viewEditProfile" runat="server">
					<div class="paragraph">
						<asp:Label ID="lblProfileHeader" runat="server" Font-Bold="true"></asp:Label>
					</div>
					<div class="paragraph">
						<table cellspacing="5" width="100%">
							<tr>
								<td align="right">
									First Name:
								</td>
								<td align="left">
									<asp:TextBox ID="tbFirstName" runat="server" MaxLength="50"></asp:TextBox>
									<asp:RegularExpressionValidator ID="regFirstName" runat="server" ControlToValidate="tbFirstName"
										ErrorMessage="Invalid First Name!" Text="Invalid First Name!" ValidationExpression="^[^<>]{0,50}$"></asp:RegularExpressionValidator>
								</td>
							</tr>
							<tr>
								<td align="right">
									Last Name:
								</td>
								<td align="left">
									<asp:TextBox ID="tbLastName" runat="server" MaxLength="50"></asp:TextBox>
									<asp:RegularExpressionValidator ID="regLastName" runat="server" ControlToValidate="tbLastName"
										ErrorMessage="Invalid Last Name!" Text="Invalid Last Name!" ValidationExpression="^[^<>]{0,50}$"></asp:RegularExpressionValidator>
								</td>
							</tr>
							<tr>
								<td align="right">
									Home Page:
								</td>
								<td align="left">
									<asp:TextBox ID="tbHomePage" runat="server" MaxLength="200"></asp:TextBox>
									<asp:RegularExpressionValidator ID="regHomePage" runat="server" ControlToValidate="tbHomePage"
										ErrorMessage="Invalid Home Page!" Text="Invalid Home Page!" ValidationExpression="^[^<>]{0,200}$"></asp:RegularExpressionValidator>
								</td>
							</tr>
							<tr>
								<td style="width: 91px;">
								</td>
								<td align="left">
									<asp:Button ID="btSave" runat="server" OnClick="btSave_Click" Text="Save" CssClass="button" />
									&nbsp;
									<asp:Button ID="btCancel" runat="server" OnClick="btCancel_Click" Text="Cancel" CssClass="button"
										CausesValidation="false" />
								</td>
							</tr>
						</table>
					</div>
					<asp:HiddenField ID="hfName" runat="server" />
				</asp:View>
				<asp:View ID="viewEditRoles" runat="server">
					<div class="paragraph">
						<asp:Label ID="lblRolesHeader" runat="server" Font-Bold="true"></asp:Label>
					</div>
					<div class="paragraph">
						<asp:CheckBoxList ID="cblRoles" runat="server">
						</asp:CheckBoxList>
						<br />
						<asp:Button ID="btSaveUserRoles" runat="server" OnClick="btSaveUserRoles_Click" Text="Save"
							CssClass="button" />
						&nbsp;
						<asp:Button ID="btCancelRoles" runat="server" OnClick="btCancel_Click" Text="Cancel"
							CausesValidation="false" CssClass="button" />
					</div>
				</asp:View>
				<asp:View ID="viewAddUser" runat="server">
					<div class="paragraph">
						<b>Add new user:</b>
					</div>
					<div class="paragraph">
						<table cellspacing="5" width="100%">
							<tr>
								<td align="right" style="width: 130px;">
									Login:
								</td>
								<td align="left">
									<asp:TextBox ID="tbLogin" runat="server" MaxLength="50"></asp:TextBox>
									<asp:RequiredFieldValidator ID="reqLogin" runat="server" ControlToValidate="tbLogin">*</asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator ID="regLogin" runat="server" ControlToValidate="tbLogin"
										ErrorMessage="Invalid Login!" Text="Invalid Login!" ValidationExpression="^[^<>]{0,50}$"></asp:RegularExpressionValidator>
								</td>
							</tr>
							<tr>
								<td align="right">
									Password:
								</td>
								<td align="left">
									<asp:TextBox ID="tbPassword" runat="server" MaxLength="50"></asp:TextBox>
									<asp:RequiredFieldValidator ID="reqPassword" runat="server" ControlToValidate="tbPassword">*</asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator ID="regPassword" runat="server" ControlToValidate="tbPassword"
										ErrorMessage="Invalid Password!" Text="Invalid Password!" ValidationExpression="^[^<>]{0,50}$"></asp:RegularExpressionValidator>
								</td>
							</tr>
							<tr>
								<td align="right">
									Confirm Password:
								</td>
								<td align="left">
									<asp:TextBox ID="tbConfirmPassword" runat="server" MaxLength="50"></asp:TextBox>
									<asp:RequiredFieldValidator ID="reqConfirmPassword" runat="server" ControlToValidate="tbConfirmPassword">*</asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator ID="regConfirmPassword" runat="server" ControlToValidate="tbConfirmPassword"
										ErrorMessage="Invalid Password!" Text="Invalid Password!" ValidationExpression="^[^<>]{0,50}$"></asp:RegularExpressionValidator>
									<asp:CompareValidator ID="comparePasswords" runat="server" ControlToValidate="tbConfirmPassword"
										ControlToCompare="tbPassword" ErrorMessage="Passwords must match!" Text="Passwords must match!"></asp:CompareValidator>
								</td>
							</tr>
							<tr>
								<td align="right">
									Email:
								</td>
								<td align="left">
									<asp:TextBox ID="tbEmail" runat="server" MaxLength="200"></asp:TextBox>
									<asp:RequiredFieldValidator ID="reqEmail" runat="server" ControlToValidate="tbEmail">*</asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator ID="regEmail" runat="server" ControlToValidate="tbEmail"
										ErrorMessage="Invalid Email!" Text="Invalid Email!" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
								</td>
							</tr>
							<tr>
								<td>
								</td>
								<td align="left">
									<asp:Button ID="btAddUser" runat="server" OnClick="btAddUser_Click" Text="Add" CssClass="button" />
									&nbsp;
									<asp:Button ID="btCancelAdd" runat="server" OnClick="btCancel_Click" Text="Cancel"
										CausesValidation="false" CssClass="button" />
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<asp:Label ID="lblMessage" runat="server"></asp:Label>
								</td>
							</tr>
						</table>
					</div>
				</asp:View>
			</asp:MultiView>
		</div>
	</div>
</asp:Content>
