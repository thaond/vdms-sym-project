<%@ Page Title="" Language="C#" MasterPageFile="~/MP/Popup.master" AutoEventWireup="true"
	CodeFile="QuickReply.aspx.cs" Inherits="QuickReply" Theme="Thickbox" %>

<%@ Register Src="Admin/Controls/Quote.ascx" TagName="Quote" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<uc1:Quote ID="Quote1" runat="server" />
	<asp:TextBox ID="tb1" runat="server" TextMode="MultiLine" Rows="5" Width="400px"></asp:TextBox>
	<div style="text-align: right">
		<asp:Button ID="b1" runat="server" Text="Send" OnClick="b1_Click" />
	</div>
</asp:Content>
