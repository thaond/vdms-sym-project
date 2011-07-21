<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="PrintOrder.aspx.cs" Inherits="Vehicle_Report_PrintOrder"
	Title="Print part change voucher" Culture="auto" meta:resourcekey="PageResource1"
	UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
	<asp:MultiView ID="MultiView1" runat="server">
		<asp:View ID="View1" runat="server">
			<asp:Label ID="lbError" runat="server" CssClass="errorMsg" meta:resourcekey="lbErrorResource1"
				Text="No order to print or order not found!"></asp:Label>
		</asp:View>
		<asp:View ID="View2" runat="server">
			<CR:CrystalReportViewer ID="rptViewer" runat="server" AutoDataBind="True" HasCrystalLogo="False"
				DisplayGroupTree="False" Height="50px" meta:resourcekey="rptViewerResource1"
				Width="350px" />
		</asp:View>
	</asp:MultiView>
</asp:Content>
