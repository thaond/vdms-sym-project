<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
	CodeFile="PrintNGForm.aspx.cs" Inherits="Part_PrintNGForm" Title="Not Good sheet"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<asp:HyperLink ID="lnkBack" runat="server" meta:resourcekey="lnkBackResource1">Back</asp:HyperLink>
	<br />
	<CR:CrystalReportViewer ID="crptViewer" runat="server" AutoDataBind="True" Height="50px"
		meta:resourcekey="crptViewerResource1" Width="350px" />
</asp:Content>
