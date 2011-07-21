<%@ Page Title="Access deny" Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
	CodeFile="AccessDeny.aspx.cs" Inherits="AccessDeny" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<asp:Label ID="lblError" runat="server" SkinID="MessageError" Text="Sorry, you have not enought permission to access this page"></asp:Label>
	<br />
	<asp:Button ID="bBack" runat="server" Text="Go back" OnClientClick="javascript://history.go(-1)" />
</asp:Content>
