<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
	CodeFile="profile.aspx.cs" Inherits="profile" Title="User Profile" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<ajaxToolkit:TabContainer ID="t" runat="server" CssClass="ajax__tab_technorati-theme"
		ActiveTabIndex="0" meta:resourcekey="tResource2">
		<ajaxToolkit:TabPanel ID="t1" runat="server" HeaderText="General Information" 
            meta:resourcekey="t1Resource1">
			<ContentTemplate>
				<div class="form" style="width: 50%;">
					<table width="100%">
						<tr>
							<td style="width: 30%">
								<asp:Localize ID="litUsername" runat="server" Text="Username:" 
                                    meta:resourcekey="litUsernameResource1"></asp:Localize>
							</td>
							<td>
								<asp:TextBox ID="tb1" runat="server" ReadOnly="True" 
                                    meta:resourcekey="tb1Resource1"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td>
								<asp:Localize ID="litFullname" runat="server" Text="Fullname:" 
                                    meta:resourcekey="litFullnameResource1"></asp:Localize>
							</td>
							<td>
								<asp:TextBox ID="tb2" runat="server" meta:resourcekey="tb2Resource1"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td>
							</td>
							<td>
								<asp:Button ID="b1" runat="server" Text="Update" OnClick="b1_Click" 
                                    meta:resourcekey="b1Resource2" />
							</td>
						</tr>
					</table>
					<asp:Label ID="lblSaveOk" runat="server" SkinID="MessageOk" Visible="False" 
                        Text="Your profile has been created/updated successful." 
                        meta:resourcekey="lblSaveOkResource1"></asp:Label>
				</div></ContentTemplate></ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="t2" runat="server" HeaderText="Change Password" 
            meta:resourcekey="t2Resource1">
			<ContentTemplate>
				<div class="form" style="width: 50%;">
					<asp:ChangePassword ID="ChangePassword1" runat="server" 
                        meta:resourcekey="ChangePassword1Resource1">
						<ChangePasswordTemplate>
							<table border="0" cellpadding="1" cellspacing="0" style="border-collapse: collapse;">
								<tr>
									<td>
										<table border="0" cellpadding="0">
											<tr>
												<td align="center" colspan="2">
                <asp:Literal ID="Literal1" runat="server" Text="Change Your Password" meta:resourcekey="Literal1Resource1"></asp:Literal>
													
												</td>
											</tr>
											<tr>
												<td align="right">
													<asp:Label ID="CurrentPasswordLabel" runat="server" 
                                                        AssociatedControlID="CurrentPassword" 
                                                        meta:resourcekey="CurrentPasswordLabelResource1">Password:</asp:Label></td><td>
													<asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password" 
                                                        meta:resourcekey="CurrentPasswordResource1"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
														ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="ChangePassword1" 
                                                        meta:resourcekey="CurrentPasswordRequiredResource1">*</asp:RequiredFieldValidator></td></tr><tr>
												<td align="right">
													<asp:Label ID="NewPasswordLabel" runat="server" 
                                                        AssociatedControlID="NewPassword" meta:resourcekey="NewPasswordLabelResource1">New Password:</asp:Label></td><td>
													<asp:TextBox ID="NewPassword" runat="server" TextMode="Password" 
                                                        meta:resourcekey="NewPasswordResource1"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
														ErrorMessage="New Password is required." ToolTip="New Password is required."
														ValidationGroup="ChangePassword1" meta:resourcekey="NewPasswordRequiredResource1">*</asp:RequiredFieldValidator></td></tr><tr>
												<td align="right">
													<asp:Label ID="ConfirmNewPasswordLabel" runat="server" 
                                                        AssociatedControlID="ConfirmNewPassword" 
                                                        meta:resourcekey="ConfirmNewPasswordLabelResource1">Confirm New Password:</asp:Label></td><td>
													<asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password" 
                                                        meta:resourcekey="ConfirmNewPasswordResource1"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
														ErrorMessage="Confirm New Password is required." ToolTip="Confirm New Password is required."
														ValidationGroup="ChangePassword1" meta:resourcekey="ConfirmNewPasswordRequiredResource1">*</asp:RequiredFieldValidator></td></tr><tr>
												<td align="center" colspan="2">
													<asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
														ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="The Confirm New Password must match the New Password entry."
														ValidationGroup="ChangePassword1" meta:resourcekey="NewPasswordCompareResource1"></asp:CompareValidator></td></tr><tr>
												<td align="center" colspan="2" style="color: Red;">
													<asp:Literal ID="FailureText" runat="server" EnableViewState="False" 
                                                        meta:resourcekey="FailureTextResource1"></asp:Literal></td></tr><tr>
												<td align="right">
													<asp:Button ID="ChangePasswordPushButton" runat="server" CommandName="ChangePassword"
														Text="Change Password" ValidationGroup="ChangePassword1" meta:resourcekey="ChangePasswordPushButtonResource1" />
												</td>
												<td>
													<asp:Button ID="CancelPushButton" runat="server" CausesValidation="False" CommandName="Cancel"
														Text="Cancel" meta:resourcekey="CancelPushButtonResource1" />
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</ChangePasswordTemplate>
					</asp:ChangePassword>
				</div>
			</ContentTemplate>
		</ajaxToolkit:TabPanel>
	</ajaxToolkit:TabContainer>
</asp:Content>
