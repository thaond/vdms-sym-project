<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="PrintPartChange.aspx.cs" Inherits="Service_Report_PrintPartChange"
	Title="Print part change voucher" Culture="auto" meta:resourcekey="PageResource1"
	UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
	<asp:MultiView ID="MultiView1" runat="server">
		<asp:View ID="View1" runat="server">
			<asp:Label ID="lbError" runat="server" CssClass="errorMsg" meta:resourceKey="lbErrorResource1"
				Text="No order to print or order not found!"></asp:Label>
		</asp:View>
		<asp:View ID="View2" runat="server">
			<CR:CrystalReportViewer ID="rptViewer" runat="server" AutoDataBind="True" HasCrystalLogo="False"
				DisplayGroupTree="False" meta:resourcekey="rptViewerResource1" />
		</asp:View>
	</asp:MultiView>
</asp:Content>
