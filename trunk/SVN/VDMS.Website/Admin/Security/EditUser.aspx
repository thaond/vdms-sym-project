<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
	CodeFile="EditUser.aspx.cs" Inherits="Admin_Security_EditUser" Title="Edit user's information"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Src="../Controls/UserProfile.ascx" TagName="UserProfile" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<asp:MultiView runat="server" ID="mvMain">
		<asp:View ID="vwMain" runat="server">
			<div style="float: left; width: 50%" class="normalBox">
				<div class="boxHeader">
					<asp:Localize ID="litTitle" runat="server" Text="General user information" meta:resourcekey="litTitleResource1"></asp:Localize>
				</div>
				<asp:UpdatePanel ID="udpUserInfo" runat="server">
					<ContentTemplate>
						<asp:UpdateProgress ID="UpdateProgress2" DisplayAfter="0" DynamicLayout="false" AssociatedUpdatePanelID="udpUserInfo"
							runat="server">
							<ProgressTemplate>
								<asp:ImageButton ID="ImageButton1" SkinID="UpdateProgress" runat="server" /><asp:Literal
									ID="Literal1x" meta:resourcekey="Literal1Resource1" runat="server" Text="Updating..."></asp:Literal>
							</ProgressTemplate>
						</asp:UpdateProgress>
						<asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="SaveUser"
							CssClass="error" meta:resourcekey="ValidationSummary1Resource1" />
						<table cellpadding="2" cellspacing="2" border="0" width="100%">
							<tr>
								<td style="width: 30%">
									<asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
									<asp:Localize ID="litUsername" runat="server" Text="UserName:" meta:resourcekey="litUsernameResource1"></asp:Localize>
								</td>
								<td>
									<asp:TextBox runat="server" ID="txtUserName" meta:resourcekey="txtUserNameResource1"></asp:TextBox>
									<asp:RequiredFieldValidator ID="valRequireUserName" runat="server" ControlToValidate="txtUserName"
										SetFocusOnError="True" ErrorMessage="Username is required." ValidationGroup="SaveUser"
										meta:resourcekey="valRequireUserNameResource1" Text="*"></asp:RequiredFieldValidator>
								</td>
							</tr>
							<tr>
								<td>
									<asp:Image ID="image1" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
									<asp:Localize ID="litEmail" runat="server" Text="E-mail:" meta:resourcekey="litEmailResource1"></asp:Localize>
								</td>
								<td>
									<asp:TextBox runat="server" ID="txtEmail" meta:resourcekey="txtEmailResource1"></asp:TextBox>
									<asp:RequiredFieldValidator ID="valRequireEmail" runat="server" ControlToValidate="txtEmail"
										SetFocusOnError="True" ErrorMessage="E-mail is required." ValidationGroup="SaveUser"
										meta:resourcekey="valRequireEmailResource1" Text="*"></asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator runat="server" ID="valEmailPattern" SetFocusOnError="True"
										ValidationGroup="SaveUser" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
										ErrorMessage="The e-mail address you specified is not well-formed." meta:resourcekey="valEmailPatternResource1"
										Text="*"></asp:RegularExpressionValidator>
								</td>
							</tr>
							<asp:MultiView ID="mvProperties" runat="server" ActiveViewIndex="0">
								<asp:View ID="view1" runat="server">
									<tr>
										<td>
											<asp:Image ID="image2" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
											<asp:Localize runat="server" ID="litPassword" Text="Password:" meta:resourcekey="litPasswordResource1"></asp:Localize>
										</td>
										<td>
											<asp:TextBox runat="server" ID="txtPassword" TextMode="Password" meta:resourcekey="txtPasswordResource1"></asp:TextBox>
											<asp:RequiredFieldValidator ID="valRequirePassword" runat="server" ControlToValidate="txtPassword"
												SetFocusOnError="True" ErrorMessage="Password is required." ValidationGroup="SaveUser"
												meta:resourcekey="valRequirePasswordResource1">*</asp:RequiredFieldValidator>
										</td>
									</tr>
									<tr>
										<td>
											<asp:Image ID="image3" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
											<asp:Localize runat="server" ID="litConfirmPassword" Text="Confirm Password:" meta:resourcekey="litConfirmPasswordResource1"></asp:Localize>
										</td>
										<td>
											<asp:TextBox runat="server" ID="txtConfirmPassword" TextMode="Password" meta:resourcekey="txtConfirmPasswordResource1"></asp:TextBox>
											<asp:RequiredFieldValidator ID="valRequireConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword"
												SetFocusOnError="True" ErrorMessage="Confirm password is required." ValidationGroup="SaveUser"
												meta:resourcekey="valRequireConfirmPasswordResource1">*</asp:RequiredFieldValidator>
											<asp:CompareValidator ID="valComparePasswords" runat="server" ControlToCompare="txtPassword"
												SetFocusOnError="True" ControlToValidate="txtConfirmPassword" ErrorMessage="Password is invalid."
												ValidationGroup="SaveUser" meta:resourcekey="valComparePasswordsResource1">*</asp:CompareValidator>
										</td>
									</tr>
								</asp:View>
								<asp:View ID="view2" runat="server">
									<tr>
										<td>
											<asp:Image ID="image5" runat="server" SkinID="RequireFieldNon" />
											<asp:Localize ID="litRegistered" runat="server" Text="Registered:" meta:resourcekey="litRegisteredResource1"></asp:Localize>
										</td>
										<td>
											<asp:Literal runat="server" ID="lblRegistered" meta:resourcekey="lblRegisteredResource1"></asp:Literal>
										</td>
									</tr>
									<tr>
										<td>
											<asp:Image ID="image6" runat="server" SkinID="RequireFieldNon" />
											<asp:Localize ID="litLastLogin" runat="server" Text="Last Login:" meta:resourcekey="litLastLoginResource1"></asp:Localize>
										</td>
										<td>
											<asp:Literal runat="server" ID="lblLastLogin" meta:resourcekey="lblLastLoginResource1"></asp:Literal>
										</td>
									</tr>
									<tr>
										<td>
											<asp:Image ID="image7" runat="server" SkinID="RequireFieldNon" />
											<asp:Localize ID="litLastActivity" runat="server" Text="Last Activity:" meta:resourcekey="litLastActivityResource1"></asp:Localize>
										</td>
										<td>
											<asp:Literal runat="server" ID="lblLastActivity" meta:resourcekey="lblLastActivityResource1"></asp:Literal>
										</td>
									</tr>
									<tr>
										<td>
											<asp:Image ID="image8" runat="server" SkinID="RequireFieldNon" />
											<asp:Label runat="server" ID="lblOnlineNow" AssociatedControlID="chkOnlineNow" Text="Online Now:"
												meta:resourcekey="lblOnlineNowResource1" />
										</td>
										<td>
											<asp:CheckBox runat="server" ID="chkOnlineNow" Enabled="False" meta:resourcekey="chkOnlineNowResource1" />
										</td>
									</tr>
									<tr>
										<td>
											<asp:Image ID="image9" runat="server" SkinID="RequireFieldNon" />
											<asp:Label runat="server" ID="lblApproved" AssociatedControlID="chkApproved" Text="Approved:"
												meta:resourcekey="lblApprovedResource1"></asp:Label>
										</td>
										<td>
											<asp:CheckBox runat="server" ID="chkApproved" meta:resourcekey="chkApprovedResource1" />
										</td>
									</tr>
									<tr>
										<td>
											<asp:Image ID="image10" runat="server" SkinID="RequireFieldNon" />
											<asp:Label runat="server" ID="lblLockedOut" AssociatedControlID="chkLockedOut" Text="Locked Out:"
												meta:resourcekey="lblLockedOutResource1"></asp:Label>
										</td>
										<td>
											<asp:CheckBox runat="server" ID="chkLockedOut" meta:resourcekey="chkLockedOutResource1" />
										</td>
									</tr>
								</asp:View>
							</asp:MultiView>
						</table>
						<uc1:UserProfile ID="_UserProfile" runat="server" />
						<br />
						<br />
						<div class="boxHeader">
							<asp:Localize ID="litEditRole" runat="server" Text="Edit user's roles" meta:resourcekey="litEditRoleResource1"></asp:Localize>
						</div>
						<br />
						<asp:CheckBoxList runat="server" ID="chklRoles" RepeatColumns="5" CellSpacing="4"
							DataTextField="RoleName" meta:resourcekey="chklRolesResource1">
						</asp:CheckBoxList>
						<br />
						<asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"
							SkinID="SubmitButton" ValidationGroup="SaveUser" meta:resourcekey="btnUpdateResource1" />
						<asp:Button ID="btnBack" runat="server" Text="Back" OnClientClick="javascript:location.href='ManageUsers.aspx'; return false;"
							meta:resourcekey="btnBackResource1" />
						<br />
						<asp:Label ID="lblSaveOk" runat="server" SkinID="MessageOk" Visible="false" Text="The setting has been saved."></asp:Label>
					</ContentTemplate>
				</asp:UpdatePanel>
			</div>
			<asp:Panel Style="float: left" ID="pnResetPass" runat="server">
				<asp:UpdatePanel ID="udpResetPassword" runat="server">
					<ContentTemplate>
						<div class="normalBox">
							<div class="boxHeader">
								<asp:Literal meta:resourcekey="litHeadResetPassResource1" ID="litHeadResetPass" runat="server"></asp:Literal></div>
							<asp:UpdateProgress ID="UpdateProgress1" DisplayAfter="0" DynamicLayout="false" AssociatedUpdatePanelID="udpResetPassword"
								runat="server">
								<ProgressTemplate>
									<asp:ImageButton ID="ImageButton2" SkinID="UpdateProgress" runat="server" /><asp:Literal
										ID="Literal1" meta:resourcekey="Literal1Resource1" runat="server" Text="Updating..."></asp:Literal>
								</ProgressTemplate>
							</asp:UpdateProgress>
							<asp:Label ID="lbResetPassResult" ForeColor="Red" runat="server" EnableViewState="False"></asp:Label>
							<table cellpadding="0" cellspacing="0" border="0">
								<tr>
									<td style="white-space: nowrap">
										<asp:Literal ID="litNewPassword" meta:resourcekey="litNewPasswordResource1" runat="server"
											Text="New Password:"></asp:Literal>
									</td>
									<td>
										<asp:TextBox ID="txtNewPassword" runat="server"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
											ValidationGroup="NewPassword" ControlToValidate="txtNewPassword" SetFocusOnError="true"></asp:RequiredFieldValidator>
										<asp:Button ID="btnResetPassword" runat="server" Text="Reset user's password" OnClick="btnResetPassword_Click"
											ValidationGroup="NewPassword" meta:resourcekey="btnResetPasswordResource1" />
									</td>
								</tr>
							</table>
						</div>
					</ContentTemplate>
				</asp:UpdatePanel>
			</asp:Panel>
		</asp:View>
		<asp:View ID="vwCrError" runat="server">
		</asp:View>
	</asp:MultiView>
</asp:Content>
