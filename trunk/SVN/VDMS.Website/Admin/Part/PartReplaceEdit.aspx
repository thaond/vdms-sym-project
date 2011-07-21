<%@ Page Title="" Language="C#" MasterPageFile="~/MP/Popup.master" AutoEventWireup="true"
    CodeFile="PartReplaceEdit.aspx.cs" Inherits="Admin_Part_PartReplaceEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form" style="width:270px">
        <asp:FormView ID="form" runat="server" DataSourceID="ods" 
            DataKeyNames="PartReplaceId" oniteminserted="form_ItemInserted" 
            onitemupdated="form_ItemUpdated">
            <EditItemTemplate>
                <table cellpadding="2" cellspacing="2">
                    <caption>
                        <asp:Literal ID="Literal1" runat="server" Text="Edit replacing part"></asp:Literal>
                    </caption>
                    <tr>
                        <th>
                            <asp:Literal ID="Literal2" runat="server" Text="Part code:"></asp:Literal>
                        </th>
                        <td>
                            <asp:TextBox Width="100%" ID="PartCodeTextBox" MaxLength="30" runat="server" Text='<%# Bind("PartCode") %>' />
                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="PartCodeTextBox" ID="RequiredFieldValidator3"
                                runat="server" ErrorMessage="Part code cannot be blank!"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Literal ID="Literal4" runat="server" Text="Replacing code:"></asp:Literal>
                        </th>
                        <td>
                            <asp:TextBox Width="100%" ID="ReplacePartCodeTextBox" MaxLength="30" runat="server" Text='<%# Bind("ReplacePartCode") %>' />
                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="ReplacePartCodeTextBox" ID="RequiredFieldValidator4"
                                runat="server" ErrorMessage="Replacing code cannot be blank!"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Literal ID="Literal5" runat="server" Text="Status:"></asp:Literal>
                        </th>
                        <td>
                            <asp:TextBox Width="100%" ID="StatusTextBox" MaxLength="1" runat="server" Text='<%# Bind("Status") %>' />
                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="StatusTextBox" ID="RequiredFieldValidator5"
                                runat="server" ErrorMessage="Status cannot be blank!"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                                Text="Update" />
                            <%--<asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                Text="Cancel" />--%>
                        </td>
                    </tr>
                </table>
            </EditItemTemplate>
            <InsertItemTemplate>
                <table cellpadding="2" cellspacing="2">
                    <caption>
                        <asp:Literal ID="Literal1" runat="server" Text="Insert replacing part"></asp:Literal>
                    </caption>
                    <tr>
                        <th>
                            <asp:Literal ID="Literal2" runat="server" Text="Part code:"></asp:Literal>
                        </th>
                        <td>
                            <asp:TextBox Width="100%" ID="PartCodeTextBox" MaxLength="30" runat="server" Text='<%# Bind("PartCode") %>' />
                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="PartCodeTextBox" ID="RequiredFieldValidator3"
                                runat="server" ErrorMessage="Part code cannot be blank!"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Literal ID="Literal4" runat="server" Text="Replacing code:"></asp:Literal>
                        </th>
                        <td>
                            <asp:TextBox Width="100%" ID="ReplacePartCodeTextBox" MaxLength="30" runat="server" Text='<%# Bind("ReplacePartCode") %>' />
                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="ReplacePartCodeTextBox" ID="RequiredFieldValidator4"
                                runat="server" ErrorMessage="Replacing code cannot be blank!"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Literal ID="Literal5" runat="server" Text="Status:"></asp:Literal>
                        </th>
                        <td>
                            <asp:TextBox Width="100%" ID="StatusTextBox" MaxLength="1" runat="server" Text='<%# Bind("Status") %>' />
                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="StatusTextBox" ID="RequiredFieldValidator5"
                                runat="server" ErrorMessage="Status cannot be blank!"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                                Text="Insert" />
                            <%-- <asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                Text="Cancel" />--%>
                        </td>
                    </tr>
                </table>
            </InsertItemTemplate>
            <ItemTemplate>
                <%--PartSpecId:
                <asp:Label ID="PartSpecIdLabel" runat="server" Text='<%# Bind("PartReplaceId") %>' />
                <br />--%>
                Part code:
                <asp:Label ID="PartCodeLabel" runat="server" Text='<%# Bind("PartCode") %>' />
                <br />
                Replacing code:
                <asp:Label ID="ReplacePartCodeLabel" runat="server" Text='<%# Bind("ReplacePartCode") %>' />
                <br />
                Status:
                <asp:Label ID="StatusLabel" runat="server" Text='<%# Bind("Status") %>' />
                <br />
                Database code:
                <asp:Label ID="DatabaseCodeLabel" runat="server" Text='<%# Bind("DatabaseCode") %>' />
                <br />
               <%-- Status:
                <asp:Label ID="StatusLabel" runat="server" Text='<%# Bind("Status") %>' />
                <br />--%>
            </ItemTemplate>
        </asp:FormView>
        <asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetById" TypeName="VDMS.II.PartManagement.PartReplaceDAO"
            DataObjectTypeName="VDMS.II.Entity.V2PPartReplacement" DeleteMethod="Delete"
            InsertMethod="Insert" UpdateMethod="Update">
            <DeleteParameters>
                <asp:Parameter Name="PartReplaceId" Type="Int64" />
            </DeleteParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="Id" QueryStringField="id" Type="Int64" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
