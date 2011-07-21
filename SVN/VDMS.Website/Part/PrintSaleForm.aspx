<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true" CodeFile="PrintSaleForm.aspx.cs" Inherits="Part_PrintSaleForm" Title="Print Order sheet" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" Runat="Server">
    <asp:HyperLink ID="lnkBack" runat="server" meta:resourcekey="lnkBackResource1">Back</asp:HyperLink>
    <br />
    <CR:CrystalReportViewer ID="crptViewer" runat="server" AutoDataBind="True" 
        Height="50px" meta:resourcekey="crptViewerResource1" Width="350px" />
</asp:Content>

