<%@ Page Title="" Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="ImportWarrantyParts.aspx.cs" Inherits="Service_ImportWarrantyParts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <asp:FileUpload ID="FileUpload" runat="server" />
    <asp:Button ID="btnUpload" runat="server" Text="Button" OnClick="btnUpload_Click" />
    <div class="grid">
        <asp:ListView ID="lv" runat="server">
            <LayoutTemplate>
                <table class="datatable" cellpadding="0" cellspacing="0">
                    <tr>
                        <th>
                            <asp:Literal ID="Literal1" runat="server" Text="PartCode"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal2" runat="server" Text="Vietnamese Name"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal3" runat="server" Text="English Name"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal4" runat="server" Text="Motor Code"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal5" runat="server" Text="Warranty Time"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal6" runat="server" Text="Warranty Length"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal7" runat="server" Text="Start Date"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal8" runat="server" Text="Stop Date"></asp:Literal>
                        </th>
                    </tr>
                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
                    <td>
                        <%#Eval("PartCode") %>
                    </td>
                    <td>
                        <%#Eval("PartNameVN") %>
                    </td>
                    <td>
                        <%#Eval("PartNameEN") %>
                    </td>
                    <td>
                        <%#Eval("MotorCode") %>
                    </td>
                    <td>
                        <%#Eval("WarrantyTime") %>
                    </td>
                    <td>
                        <%#Eval("WarrantyLength") %>
                    </td>
                    <td>
                        <%#Eval("StartDate") %>
                    </td>
                    <td>
                        <%#Eval("StopDate") %>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>
    <asp:Button ID="btnImport" runat="server" Text="Button" 
        onclick="btnImport_Click" />
</asp:Content>
