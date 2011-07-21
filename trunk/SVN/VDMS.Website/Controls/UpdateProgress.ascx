<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UpdateProgress.ascx.cs"
	Inherits="Controls_UpdateProgress" %>
<asp:UpdateProgress DisplayAfter="0" DynamicLayout="False" ID="UpdateProgress1" runat="server">
	<ProgressTemplate>
		<asp:ImageButton ID="ImageButton1" SkinID="UpdateProgress" runat="server" meta:resourcekey="ImageButton1Resource1" /><asp:Literal
			ID="Literal1" Text="Updating..." runat="server" meta:resourcekey="Literal1Resource1"></asp:Literal></ProgressTemplate>
</asp:UpdateProgress>
