<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Quote.ascx.cs" Inherits="Admin_Controls_Quote" %>
<div class="form" style="width: 85%">
	<asp:Localize ID="l1" runat="server" Text="Message from: " 
        meta:resourcekey="l1Resource1"></asp:Localize>
	<asp:Label ID="lb1" runat="server" Font-Bold="True" 
        meta:resourcekey="lb1Resource1"></asp:Label>
	<br />
	<asp:Literal ID="l2" runat="server" meta:resourcekey="l2Resource1"></asp:Literal>
</div>
