<%@ Page Title="" Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="sql.aspx.cs" Inherits="sql" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <br />
    <asp:Button ID="btnUpdateReceive" runat="server" Text="UpdatePartReceive" 
        onclick="btnUpdateReceive_Click" Visible="False" />
    <asp:TextBox ID="txtSql" runat="server" TextMode="MultiLine" Width="100%" Rows = "10"></asp:TextBox>
    <asp:Button ID="cmdRun" runat="server" Text="Run" OnClick="cmdRun_Click" />
    <div class="grid">
        <asp:GridView CssClass="datatable" ID="gv" runat="server" AutoGenerateColumns="true">
        </asp:GridView>
    </div>
</asp:Content>
