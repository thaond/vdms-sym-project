<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register TagPrefix="cc1" Namespace="VDMS.II.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>VDMS Login page</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:PlaceHolder runat="server" ID="pl1">
        <table style="width: 100%; height: 100%; background-color: #ffffff">
            <tr>
                <td valign="top">
                    <div id="mainBody">
                        <div style="text-align: right">
                            <%--<font size="0.5">Power by </font>--%>
                            <a href="http://www.thanglongsoftware.net" title="Power by Thang Long Software"><font
                                size="0.5" color="Blue">
                                <img src="Images/power.png" style="border: 0" alt="Thang Long Software" /></font>
                            </a>
                        </div>
                        <div align="center">
                            <div class="loginUI">
                                <asp:Login ID="Login1" runat="server" meta:resourcekey="Login1Resource1" TitleText=""
                                    OnLoggedIn="Login1_LoggedIn" DestinationPageUrl="~/Default.aspx" CssClass="loginCom"
                                    MembershipProvider="VDMSMembershipProvider" OnLoggingIn="Login1_LoggingIn">
                                    <LayoutTemplate>
                                        <table border="0" cellpadding="0" width="500">
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lbLoginAs" runat="server" AssociatedControlID="ddl" CssClass="loginCom"
                                                        Text="Login as:" meta:resourcekey="lbLoginAsResource1"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDealerCode" runat="server"></asp:TextBox>
                                                    <asp:CustomValidator ID="cvDealerCode" runat="server" ControlToValidate="txtDealerCode"
                                                        SetFocusOnError="true" ValidateEmptyText="false" Text="*" ErrorMessage="Dealer code not found!"
                                                        ValidationGroup="Login1" OnServerValidate="cvDealerCode_ServerValidate" meta:resourcekey="cvDealerCodeResource1"></asp:CustomValidator>
                                                    <cc1:DealerList ID="ddl" runat="server" AppendDataBoundItems="true">
                                                    </cc1:DealerList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" style="width: 200px;">
                                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" meta:resourcekey="UserNameLabelResource1"
                                                        CssClass="loginCom" Text="User Name:"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="UserName" runat="server" meta:resourcekey="UserNameResource1" CssClass="loginComNormal"
                                                        Width="220px" OnTextChanged="UserName_TextChanged"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                        ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1"
                                                        meta:resourcekey="UserNameRequiredResource1">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" meta:resourcekey="PasswordLabelResource1"
                                                        CssClass="loginCom" Text="Password:"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="Password" runat="server" TextMode="Password" meta:resourcekey="PasswordResource1"
                                                        CssClass="loginComNormal" Width="220px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                        ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1"
                                                        meta:resourcekey="PasswordRequiredResource1">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="LanguageLabel" runat="server" AssociatedControlID="Language" meta:resourcekey="LanguageLabelResource1"
                                                        CssClass="loginCom" Text="Language:"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="Language" runat="server" Width="221px" meta:resourcekey="LanguageResource1"
                                                        AutoPostBack="True" OnSelectedIndexChanged="Language_SelectedIndexChanged" CssClass="loginComNormal"
                                                        DataSourceID="LangObjectDataSource" DataTextField="NativeName" DataValueField="Name">
                                                        <asp:ListItem Text="English" Value="en-US" meta:resourcekey="ListItemResource1" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Việt Nam" Value="vi-VN" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="LangObjectDataSource" runat="server" SelectMethod="SelectAllLang"
                                                        TypeName="VDMS.BLL.ObjectDataSource.LanguageDataSource"></asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." meta:resourcekey="RememberMeResource1"
                                                        CssClass="loginComNormal" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2" style="color: red">
                                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False" meta:resourcekey="FailureTextResource1"></asp:Literal>
                                                    <asp:ValidationSummary ID="vs" runat="server" ValidationGroup="Login1" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td align="left">
                                                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="Login1"
                                                        meta:resourcekey="LoginButtonResource1" CssClass="loginCom" />
                                                </td>
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                </asp:Login>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="pl2">
        <div id="wrapper">
            <div id="top_bar">
                <div class="inner">
                    <a href="/Mobile/">
                        <img alt="" src="/images/logo.png">
                    </a>
                </div>
            </div>
            <div class="content_wrapper">
                <asp:Login ID="Login2" runat="server" meta:resourcekey="Login1Resource1" TitleText=""
                    Width="99%" OnLoggedIn="Login1_LoggedIn" DestinationPageUrl="~/Mobile/Default.aspx"
                    MembershipProvider="VDMSMembershipProvider" OnLoggingIn="Login1_LoggingIn">
                    <LayoutTemplate>
                        <p>
                            <asp:Label ID="lbLoginAs" runat="server" AssociatedControlID="ddl" Text="Login as:"
                                meta:resourcekey="lbLoginAsResource1"></asp:Label>
                            <asp:TextBox ID="txtDealerCode" runat="server" Width="90%"></asp:TextBox>
                            <asp:CustomValidator ID="cvDealerCode" runat="server" ControlToValidate="txtDealerCode"
                                SetFocusOnError="true" ValidateEmptyText="false" Text="*" ErrorMessage="Dealer code not found!"
                                ValidationGroup="Login1" OnServerValidate="cvDealerCode_ServerValidate" meta:resourcekey="cvDealerCodeResource1"></asp:CustomValidator>
                            <cc1:DealerList ID="ddl" runat="server" AppendDataBoundItems="true" Width="90%">
                            </cc1:DealerList>
                        </p>
                        <p>
                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" meta:resourcekey="UserNameLabelResource1"
                                Text="User Name:"></asp:Label>
                            <asp:TextBox ID="UserName" runat="server" meta:resourcekey="UserNameResource1" Width="90%"
                                OnTextChanged="UserName_TextChanged"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1"
                                meta:resourcekey="UserNameRequiredResource1">*</asp:RequiredFieldValidator>
                        </p>
                        <p>
                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" meta:resourcekey="PasswordLabelResource1"
                                Text="Password:"></asp:Label>
                            <asp:TextBox ID="Password" runat="server" TextMode="Password" meta:resourcekey="PasswordResource1"
                                CssClass="loginComNormal" Width="90%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1"
                                meta:resourcekey="PasswordRequiredResource1">*</asp:RequiredFieldValidator>
                        </p>
                        <p>
                            <asp:Label ID="LanguageLabel" runat="server" AssociatedControlID="Language" meta:resourcekey="LanguageLabelResource1"
                                Text="Language:"></asp:Label>
                            <asp:DropDownList ID="Language" runat="server" Width="90%" meta:resourcekey="LanguageResource1"
                                AutoPostBack="True" OnSelectedIndexChanged="Language_SelectedIndexChanged" CssClass="loginComNormal"
                                DataSourceID="LangObjectDataSource" DataTextField="NativeName" DataValueField="Name">
                                <asp:ListItem Text="English" Value="en-US" meta:resourcekey="ListItemResource1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Việt Nam" Value="vi-VN" meta:resourcekey="ListItemResource2"></asp:ListItem>
                            </asp:DropDownList>
                        </p>
                        <asp:ObjectDataSource ID="LangObjectDataSource" runat="server" SelectMethod="SelectAllLang"
                            TypeName="VDMS.BLL.ObjectDataSource.LanguageDataSource"></asp:ObjectDataSource>
                        <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." meta:resourcekey="RememberMeResource1"
                            Width="99%" />
                        <asp:Label ID="FailureText" runat="server" EnableViewState="False" meta:resourcekey="FailureTextResource1"
                            ForeColor="Red"></asp:Label>
                        <br />
                        <asp:ValidationSummary ID="vs" runat="server" ValidationGroup="Login1" />
                        <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="Login1"
                            Width="91%" meta:resourcekey="LoginButtonResource1" />
                    </LayoutTemplate>
                </asp:Login>
            </div>
        </div>
    </asp:PlaceHolder>
    </form>
</body>
</html>
