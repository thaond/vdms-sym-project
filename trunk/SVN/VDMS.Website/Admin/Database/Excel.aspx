<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Excel.aspx.cs" Inherits="Admin_Database_Excel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <b>Data items:</b> From<asp:TextBox ID="txtFromItem" Text="1" runat="server"></asp:TextBox> To<asp:TextBox ID="txtToItem" Text="1000" runat="server"></asp:TextBox><br />
        <b>Sources:</b>
        <asp:TextBox runat="server" Rows="6" TextMode="MultiLine" ID="txtSql" Width="100%"></asp:TextBox>
        <br />
        Tables<br />
        <asp:DropDownList ID="ddlTables" runat="server">
        </asp:DropDownList>
        <br />
        Views<br />
        <asp:DropDownList ID="ddlViews" runat="server">
        </asp:DropDownList>
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="Get" onclick="Button1_Click" />
        <asp:Button ID="btnExcel" runat="server" Text="Get Excel" 
            onclick="btnExcel_Click" />
        <br />
        <asp:GridView ID="GridView1" AutoGenerateColumns="true" runat="server" CellPadding="4" ForeColor="#333333" >
            <RowStyle BackColor="#E3EAEB" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#7C6F57" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" TypeName="ExcelDataSource" SelectMethod="GetData">
        <SelectParameters>
        <asp:ControlParameter ControlID="txtSql" Name="query" PropertyName="Text" Type="String" />
        <asp:ControlParameter ControlID="ddlTables" Name="tableName" PropertyName="Text" Type="String" />
        <asp:ControlParameter ControlID="ddlViews" Name="viewName" PropertyName="Text" Type="String" />
        <asp:ControlParameter ControlID="txtFromItem" Name="from" PropertyName="Text" Type="Int32" />
        <asp:ControlParameter ControlID="txtToItem" Name="to" PropertyName="Text" Type="Int32" />
        </SelectParameters>
        </asp:ObjectDataSource>
        <br />
        <br />
        <b>Sample:</b>
        <br />
        Color: select distinct tc_col010 as ColorCode, tc_col030 as ColorName from view_htf_tc_col_file
    </div>
    </form>
</body>
</html>
