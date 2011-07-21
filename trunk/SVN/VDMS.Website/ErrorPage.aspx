<%@ Page Title="Exception occur" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="ErrorPage" Culture="auto"
	meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<h2>
		<asp:Localize ID="litTitle" runat="server" Text="An exception occur" meta:resourcekey="litTitleResource1"></asp:Localize>
	</h2>
	<h3>
		<asp:Literal ID="litMessage" runat="server" meta:resourcekey="litMessageResource1"></asp:Literal></h3>
	<asp:Literal ID="litDetail" runat="server" meta:resourcekey="litDetailResource1"></asp:Literal>
	<br />
	<asp:Button ID="bBack" runat="server" Text="Go back" OnClick="bBack_Click" meta:resourcekey="bBackResource1" />
</asp:Content>
