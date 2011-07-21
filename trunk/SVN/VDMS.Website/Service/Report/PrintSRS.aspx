<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
	CodeFile="PrintSRS.aspx.cs" Inherits="Service_Report_PrintSRS" Title="Print service record sheet"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ PreviousPageType VirtualPath="~/service/WarrantyContent.aspx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
	<asp:LinkButton ID="lnkBackTop" runat="server" meta:resourcekey="lnkBackTopResource1">Go back</asp:LinkButton>
	<asp:MultiView ID="MultiView1" runat="server">
		<asp:View ID="View1" runat="server">
			<asp:Label ID="lbError" runat="server" CssClass="errorMsg" meta:resourceKey="lbErrorResource1"
				Text="No sheet to print or sheet not found!"></asp:Label>
		</asp:View>
		<asp:View ID="View2" runat="server">
			<CR:CrystalReportViewer ID="rptViewer" runat="server" AutoDataBind="True" HasCrystalLogo="False"
				DisplayGroupTree="False" meta:resourcekey="rptViewerResource1" />
			<br />
			<asp:LinkButton ID="lnkBack" runat="server" meta:resourcekey="lnkBackTopResource1">Go back</asp:LinkButton>
		</asp:View>
	</asp:MultiView>
</asp:Content>
