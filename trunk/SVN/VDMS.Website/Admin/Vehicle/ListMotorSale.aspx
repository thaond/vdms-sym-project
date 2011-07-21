<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="ListMotorSale.aspx.cs" Inherits="Admin_Database_ListMotorSale" Title="Danh mục bán xe"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="VDMS.Common" Namespace="VDMS.Common.Web" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
    <asp:ValidationSummary CssClass="error" ID="ValidationSummary1" runat="server" ValidationGroup="check"
        meta:resourcekey="ValidationSummary1Resource2" />
    <div class="form">
        <table border="0" cellpadding="2" cellspacing="2">
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Item code:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtModelCode" runat="server" Width="100px" meta:resourcekey="txtModelCodeResource1"
                        CssClass="inputKeyField" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="Color code:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtColorCode" runat="server" Width="127px" meta:resourcekey="txtColorCodeResource1"
                        CssClass="inputKeyField" MaxLength="20"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnTest" runat="server" meta:resourceKey="btnTestResource1" Text="Check"
                        ValidationGroup="check" OnClick="btnTest_Click" SkinID="SubmitButton" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="Status:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="DropDownList1" runat="server" Width="105px" meta:resourcekey="DropDownList1Resource1">
                        <asp:ListItem Value="-1" meta:resourcekey="ListItemResource1">All</asp:ListItem>
                        <asp:ListItem Value="1" meta:resourcekey="ListItemResource2">Selling</asp:ListItem>
                        <asp:ListItem Value="0" meta:resourcekey="ListItemResource3">Not Sell</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="4">
                    <asp:Button ID="cmdSynchronize" runat="server" SkinID="SpecialButton" OnClick="cmdSynchronize_Click"
                        Text="Synchronize" meta:resourcekey="cmdSynchronizeResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <vdms:PageGridView ID="gvItem" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            DataKeyNames="Id" CssClass="GridView" meta:resourcekey="GridView1Resource1" Visible="False"
            OnPreRender="gvItem_PreRender" OnRowDeleting="gvItem_RowDeleting" OnRowDataBound="gvItem_RowDataBound"
            Width="700px">
            <Columns>
                <asp:TemplateField HeaderText="No" meta:resourcekey="TemplateFieldResource6">
                    <EditItemTemplate>
                        <asp:Literal ID="litNo" runat="server" meta:resourcekey="litNoResource1"></asp:Literal>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="litNo" runat="server" meta:resourcekey="litNoResource2"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CheckBoxField DataField="Forhtf" HeaderText="For HTF" SortExpression="Available"
                    meta:resourcekey="CheckBoxFieldResource1">
                    <ControlStyle CssClass="centerObj" />
                    <ItemStyle HorizontalAlign="Center" CssClass="centerObj" />
                </asp:CheckBoxField>
                <asp:CheckBoxField DataField="Fordnf" HeaderText="For DNF" SortExpression="Available"
                    meta:resourcekey="CheckBoxFieldResource1">
                    <ControlStyle CssClass="centerObj" />
                    <ItemStyle HorizontalAlign="Center" CssClass="centerObj" />
                </asp:CheckBoxField>
                <asp:TemplateField HeaderText="ItemType" SortExpression="Itemtype" meta:resourcekey="TemplateFieldResource1">
                    <EditItemTemplate>
                        <asp:Literal ID="Literal6" runat="server" Text='<%# Eval("Id") %>'></asp:Literal>
                    </EditItemTemplate>
                    <ItemStyle CssClass="vCenterObj" Wrap="False" />
                    <ItemTemplate>
                        <asp:Literal ID="Literal6" runat="server" Text='<%# Eval("Id") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Color code" SortExpression="Colorcode" meta:resourcekey="TemplateFieldResource3x">
                    <EditItemTemplate>
                        <asp:Literal ID="Literal5x" runat="server" Text='<%# Eval("Colorcode") %>'></asp:Literal>
                        &nbsp;
                    </EditItemTemplate>
                    <ItemStyle CssClass="vCenterObj" Wrap="False" />
                    <ItemTemplate>
                        <asp:Literal ID="Literal5x" runat="server" Text='<%# Eval("Colorcode") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Color name" SortExpression="Colorname" meta:resourcekey="TemplateFieldResource3">
                    <EditItemTemplate>
                        <asp:Literal ID="Literal5" runat="server" Text='<%# Eval("Colorname") %>'></asp:Literal>
                        &nbsp;
                    </EditItemTemplate>
                    <ItemStyle CssClass="vCenterObj" Wrap="False" />
                    <ItemTemplate>
                        <asp:Literal ID="Literal5" runat="server" Text='<%# Eval("Colorname") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Item Name" SortExpression="Itemname" meta:resourcekey="TemplateFieldResource4">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox4" runat="server" MaxLength="50" Text='<%# Bind("Itemname") %>'
                            meta:resourcekey="TextBox4Resource1" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox4"
                            ErrorMessage='You must enter &quot;Item name&quot; ' ValidationGroup="check"
                            meta:resourcekey="RequiredFieldValidator1Resource2" Text="*"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemStyle CssClass="vCenterObj" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("Itemname") %>' meta:resourcekey="Label4Resource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Price" SortExpression="Price" meta:resourcekey="TemplateFieldResource2">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Price") %>' Width="120px"
                            meta:resourcekey="TextBox1Resource1" MaxLength="15"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TextBox1"
                            ErrorMessage="Invalid Item Price" SetFocusOnError="True" ValidationExpression="\s*\d*\s*"
                            ValidationGroup="check" meta:resourcekey="RegularExpressionValidator1Resource1"
                            Text="*"></asp:RegularExpressionValidator><asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                runat="server" ControlToValidate="TextBox1" ErrorMessage="You must enter item price"
                                SetFocusOnError="True" ValidationGroup="check" meta:resourcekey="RequiredFieldValidator2Resource2"
                                Text="*"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemStyle CssClass="rightObj" Wrap="False" />
                    <ItemTemplate>
                        &nbsp;<asp:Literal ID="litPrice" runat="server" Text='<%# Eval("Price") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ButtonType="Image" EditImageUrl="~/Images/Edit.gif" ShowEditButton="True"
                    meta:resourcekey="CommandFieldResource1" CancelImageUrl="~/Images/cancel.gif"
                    UpdateImageUrl="~/Images/update.gif" ValidationGroup="check" HeaderText="Edit">
                    <ItemStyle CssClass="centerObj" />
                </asp:CommandField>
                <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource5" HeaderText="Delete"
                    Visible="False">
                    <ItemStyle CssClass="centerObj" />
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Delete"
                            ImageUrl="~/Images/Delete.gif" OnLoad="ImageButton1_Load" Text="Delete" meta:resourcekey="ImageButton1Resource1"
                            ValidationGroup="check" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Label ID="Label5" runat="server" OnLoad="Label5_Load" Text="Label"
                    meta:resourcekey="Label5Resource1"></asp:Label>
            </EmptyDataTemplate>
        </vdms:PageGridView>
    </div>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DeleteMethod="Delete"
        EnablePaging="True" SelectCountMethod="SelectCount" SelectMethod="Select" TypeName="VDMS.I.ObjectDataSource.DataItemDataSource"
        UpdateMethod="Update">
        <SelectParameters>
            <asp:Parameter DefaultValue="10" Name="maximumRows" Type="Int32" />
            <asp:Parameter DefaultValue="0" Name="startRowIndex" Type="Int32" />
            <asp:ControlParameter Name="itemCodeLike" Type="String" ControlID="txtModelCode"
                PropertyName="Text" />
            <asp:ControlParameter Name="colorCodeLike" Type="String" ControlID="txtColorCode"
                PropertyName="Text" />
            <asp:ControlParameter ControlID="DropDownList1" DefaultValue="" Name="status" PropertyName="SelectedValue"
                Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="String" />
        </DeleteParameters>
    </asp:ObjectDataSource>
</asp:Content>
