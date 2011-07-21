<%@ Page Title="" Language="C#" MasterPageFile="~/MP/Popup.master" AutoEventWireup="true"
    CodeFile="PartSpecEdit.aspx.cs" Inherits="Admin_Part_PartSpecEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form" style="width:270px">
        <asp:FormView ID="form" runat="server" DataSourceID="ods" 
            DataKeyNames="PartSpecId" oniteminserted="form_ItemInserted" 
            onitemupdated="form_ItemUpdated">
            <EditItemTemplate>
                <table cellpadding="2" cellspacing="2">
                    <caption>
                        <asp:Literal ID="Literal1" runat="server" Text="Edit part specification"></asp:Literal>
                    </caption>
                    <tr>
                        <th>
                            <asp:Literal ID="Literal2" runat="server" Text="Part code:"></asp:Literal>
                        </th>
                        <td>
                            <asp:TextBox Width="100%" ID="PartCodeTextBox" MaxLength="30" Enabled="false" runat="server"
                                Text='<%# Bind("PartCode") %>' />
                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="PartCodeTextBox" ID="RequiredFieldValidator1"
                                runat="server" ErrorMessage="Part code cannot be blank!"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Literal ID="Literal4" runat="server" Text="Packing:"></asp:Literal>
                        </th>
                        <td>
                            <asp:TextBox Width="100%" ID="PackByTextBox" MaxLength="5" runat="server" Text='<%# Bind("PackBy") %>' />
                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="PackByTextBox" ID="RequiredFieldValidator2"
                                runat="server" ErrorMessage="Packing type cannot be blank!"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Literal ID="Literal5" runat="server" Text="Unit:"></asp:Literal>
                        </th>
                        <td>
                            <asp:TextBox Width="100%" ID="PackUnitTextBox" MaxLength="5" runat="server" Text='<%# Bind("PackUnit") %>' />
                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="PackUnitTextBox" ID="RequiredFieldValidator3"
                                runat="server" ErrorMessage="Unit cannot be blank!"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Literal ID="Literal6" runat="server" Text="Quantity:"></asp:Literal>
                        </th>
                        <td>
                            <asp:TextBox Width="100%" ID="PackQuantityTextBox" MaxLength="5" runat="server" Text='<%# Bind("PackQuantity") %>' />
                            <ajaxToolkit:FilteredTextBoxExtender TargetControlID="PackQuantityTextBox" FilterType="Numbers"
                                ID="FilteredTextBoxExtender1" runat="server">
                            </ajaxToolkit:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="PackQuantityTextBox" ID="RequiredFieldValidator6"
                                runat="server" ErrorMessage="Quantity type cannot be blank!"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                   <%-- <tr>
                        <th>
                            <asp:Literal ID="Literal7" runat="server" Text="Status:"></asp:Literal>
                        </th>
                        <td>
                            <asp:TextBox Width="100%" ID="StatusTextBox" MaxLength="1" runat="server" Text='<%# Bind("Status") %>' />
                        </td>
                    </tr>--%>
                    <tr>
                        <th>
                            <asp:Literal ID="Literal3" runat="server" Text="Note:"></asp:Literal>
                        </th>
                        <td>
                            <asp:TextBox Width="100%" ID="SpecNoteTextBox" TextMode="MultiLine" Rows="4" MaxLength="512"
                                runat="server" Text='<%# Bind("SpecNote") %>' />
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
                        <asp:Literal ID="Literal1" runat="server" Text="Insert part specification"></asp:Literal>
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
                            <asp:Literal ID="Literal4" runat="server" Text="Packing:"></asp:Literal>
                        </th>
                        <td>
                            <asp:TextBox Width="100%" ID="PackByTextBox" MaxLength="5" runat="server" Text='<%# Bind("PackBy") %>' />
                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="PartCodeTextBox" ID="RequiredFieldValidator4"
                                runat="server" ErrorMessage="Packing type cannot be blank!"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Literal ID="Literal5" runat="server" Text="Unit:"></asp:Literal>
                        </th>
                        <td>
                            <asp:TextBox Width="100%" ID="PackUnitTextBox" MaxLength="5" runat="server" Text='<%# Bind("PackUnit") %>' />
                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="PackUnitTextBox" ID="RequiredFieldValidator5"
                                runat="server" ErrorMessage="Unit cannot be blank!"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Literal ID="Literal6" runat="server" Text="Quantity:"></asp:Literal>
                        </th>
                        <td>
                            <asp:TextBox Width="100%" ID="PackQuantityTextBox" MaxLength="5" runat="server" Text='<%# Bind("PackQuantity") %>' />
                            <ajaxToolkit:FilteredTextBoxExtender TargetControlID="PackQuantityTextBox" FilterType="Numbers"
                                ID="FilteredTextBoxExtender1" runat="server">
                            </ajaxToolkit:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="PackQuantityTextBox" ID="RequiredFieldValidator6"
                                runat="server" ErrorMessage="Quantity type cannot be blank!"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <%--<tr>
                        <th>
                            <asp:Literal ID="Literal7" runat="server" Text="Status:"></asp:Literal>
                        </th>
                        <td>
                            <asp:TextBox Width="100%" ID="StatusTextBox" MaxLength="1" runat="server" Text='<%# Bind("Status") %>' />
                        </td>
                    </tr>--%>
                    <tr>
                        <th>
                            <asp:Literal ID="Literal3" runat="server" Text="Note:"></asp:Literal>
                        </th>
                        <td>
                            <asp:TextBox Width="100%" ID="SpecNoteTextBox" TextMode="MultiLine" Rows="4" MaxLength="512"
                                runat="server" Text='<%# Bind("SpecNote") %>' />
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
                <asp:Label ID="PartSpecIdLabel" runat="server" Text='<%# Bind("PartSpecId") %>' />
                <br />--%>
                Part code:
                <asp:Label ID="PartCodeLabel" runat="server" Text='<%# Bind("PartCode") %>' />
                <br />
                Packing:
                <asp:Label ID="PackByLabel" runat="server" Text='<%# Bind("PackBy") %>' />
                <br />
                Unit:
                <asp:Label ID="PackUnitLabel" runat="server" Text='<%# Bind("PackUnit") %>' />
                <br />
                Quantity:
                <asp:Label ID="PackQuantityLabel" runat="server" Text='<%# Bind("PackQuantity") %>' />
                <br />
                SpecNote:
                <asp:Label ID="SpecNoteLabel" runat="server" Text='<%# Bind("SpecNote") %>' />
                <br />
               <%-- Status:
                <asp:Label ID="StatusLabel" runat="server" Text='<%# Bind("Status") %>' />
                <br />--%>
            </ItemTemplate>
        </asp:FormView>
        <asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetById" TypeName="VDMS.II.PartManagement.PartSpecDAO"
            DataObjectTypeName="VDMS.II.Entity.PartSpecification" DeleteMethod="DeletePartSpec"
            InsertMethod="Insert" UpdateMethod="Update">
            <DeleteParameters>
                <asp:Parameter Name="PartSpecId" Type="Int64" />
            </DeleteParameters>
<%--            <UpdateParameters>
                <asp:Parameter Name="PartSpecId" Type="Int64" />
                <asp:Parameter Name="PackBy" Type="String" />
                <asp:Parameter Name="PackUnit" Type="String" />
                <asp:Parameter Name="PartCode" Type="String" />
                <asp:Parameter Name="PackQuantity" Type="Int32" />
                <asp:Parameter Name="SpecNote" Type="String" />
                <asp:Parameter Name="Status" Type="String" />
            </UpdateParameters>--%>
            <SelectParameters>
                <asp:QueryStringParameter Name="Id" QueryStringField="id" Type="Int64" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
