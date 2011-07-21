<%@ Page Title="Search replacing parts" Language="C#" MasterPageFile="~/MP/Popup.master"
    AutoEventWireup="true" CodeFile="ReplacePart.aspx.cs" Inherits="Part_Inventory_ReplacePart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <table>
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Replaced part code:"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtOldPart" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="New part code:"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtNewPart" runat="server"></asp:TextBox>
                    <asp:Button ID="btnFind" runat="server" Text="Find" onclick="btnFind_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div class="grid" style="margin-left:7px; width:500px">
        <vdms:PageGridView ID="gv" runat="server" AutoGenerateColumns="False" 
            DataSourceID="ods" onselectedindexchanged="gv_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="PartCode" HeaderText="Part code" 
                    ReadOnly="True" SortExpression="PartCode" />
                <asp:BoundField DataField="ReplacePartCode" HeaderText="Replace with" 
                    ReadOnly="True" SortExpression="ReplacePartCode" />
                <asp:BoundField DataField="PartName" HeaderText="Part name" ReadOnly="True" 
                    SortExpression="PartName" />
                <asp:CommandField ButtonType="Image" SelectImageUrl="~/Images/check.PNG" 
                    ShowSelectButton="True">
                <ControlStyle Height="16px" Width="16px" />
                </asp:CommandField>
            </Columns>
        </vdms:PageGridView>
        <asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetPartReplace" EnablePaging="true"
            TypeName="VDMS.II.PartManagement.PartReplaceDAO">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtOldPart" Name="partCode" 
                    PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="txtNewPart" Name="repPartCode" 
                    PropertyName="Text" Type="String" />
                <asp:Parameter Name="partName" Type="String" />
                <asp:Parameter DefaultValue="Y" Name="status" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
