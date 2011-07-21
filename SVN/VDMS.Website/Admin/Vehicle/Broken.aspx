<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="Broken.aspx.cs" Inherits="Admin_Database_Spare" Title="Sửa đổi mã số hiện tượng NG linh kiện"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="VDMS.Common" Namespace="VDMS.Common.Web" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Check"
        meta:resourcekey="ValidationSummary1Resource1" />
    <asp:BulletedList ID="bllErrorMsg" runat="server" CssClass="errorMsg" meta:resourcekey="bllErrorMsgResource1">
    </asp:BulletedList>
    <br />
    <div class="form">
        <table border="0" cellpadding="2" cellspacing="2">
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Broken code:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                    <asp:TextBox ID="txtFromCode" runat="server" ValidationGroup="Check" Width="120px"
                        meta:resourcekey="txtFromCodeResource1" MaxLength="30"></asp:TextBox><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtFromCode"
                            ErrorMessage='"From Broken code" must input numeric only!' ValidationExpression="\s*\d*\s*"
                            ValidationGroup="Check" meta:resourcekey="RegularExpressionValidator1Resource1"
                            Text="*"></asp:RegularExpressionValidator>
                    <asp:Literal ID="Literal3" runat="server" Text="~" meta:resourcekey="Literal3Resource1"></asp:Literal>&nbsp;
                    <asp:TextBox ID="txtToCode" runat="server" ValidationGroup="Check" Width="120px"
                        meta:resourcekey="txtToCodeResource1" MaxLength="30"></asp:TextBox>
                </td>
                <td>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtToCode"
                        ErrorMessage='"To Broken code" must input numeric only!' ValidationExpression="\s*\d*\s*"
                        ValidationGroup="Check" meta:resourcekey="RegularExpressionValidator2Resource1"
                        Text="*"></asp:RegularExpressionValidator>
                </td>
                <td>
                    &nbsp;<asp:Button ID="btnTest" runat="server" Text="Check" Width="69px" ValidationGroup="Check"
                        OnClick="btnTest_Click" meta:resourcekey="btnTestResource1" />
                    <asp:Button ID="btnAdd" runat="server" Text="Add new" OnClick="btnAdd_Click" meta:resourcekey="btnAddResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <vdms:PageGridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            DataSourceID="ObjectDataSource2" DataKeyNames="Id" OnRowDataBound="GridView1_RowDataBound"
            Width="100%" OnRowUpdated="GridView1_RowUpdated" OnRowUpdating="GridView1_RowUpdating"
            meta:resourcekey="GridView1Resource1" Visible="False" OnRowDeleting="GridView1_RowDeleting"
            PageSize="50">
            <Columns>
                <asp:TemplateField HeaderText="No" meta:resourcekey="TemplateFieldResource6">
                    <EditItemTemplate>
                        <asp:Literal ID="litNo" runat="server" meta:resourcekey="litNoResource1"></asp:Literal>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="litNo" runat="server" meta:resourcekey="litNoResource2"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle CssClass="center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Broken code" SortExpression="Brokencode" meta:resourcekey="TemplateFieldResource1">
                    <EditItemTemplate>
                        <asp:Label ID="Label1z" runat="server" meta:resourcekey="Label1Resource1" Text='<%# Bind("Brokencode") %>'></asp:Label><asp:TextBox
                            ID="TextBox1" runat="server" Text='<%# Bind("Brokencode") %>' Width="80px" meta:resourcekey="TextBox1Resource1"
                            Visible="False"></asp:TextBox><asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                runat="server" ControlToValidate="TextBox1" ErrorMessage='Updated &quot;Broken code&quot; must input numeric only!'
                                ValidationExpression="\s*\d*\s*" ValidationGroup="Check" SetFocusOnError="True"
                                meta:resourcekey="RegularExpressionValidator1Resource2" Text="*" Visible="False"></asp:RegularExpressionValidator><asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1" ErrorMessage='You must enter &quot;Broken code&quot;'
                                    meta:resourcekey="RequiredFieldValidator1Resource1" SetFocusOnError="True" ValidationGroup="Check"
                                    Text="*" Visible="False"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Brokencode") %>' meta:resourcekey="Label1Resource1"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Broken name" SortExpression="Brokenname" meta:resourcekey="TemplateFieldResource2">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Brokenname") %>' Width="250px"
                            meta:resourcekey="TextBox2Resource1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="TextBox2"
                            ErrorMessage="You must enter &quot;Broken name&quot;" meta:resourcekey="RequiredFieldValidator9Resource1"
                            SetFocusOnError="True" ValidationGroup="Check" Text="*"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("Brokenname") %>' meta:resourcekey="Label2Resource1"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Last update" SortExpression="Lastupdate" meta:resourcekey="TemplateFieldResource3">
                    <EditItemTemplate>
                        <asp:Literal ID="lblLastUpdate" runat="server" Text='<%# Bind("Lastupdate") %>'></asp:Literal>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="lblLastUpdate" runat="server" Text='<%# Bind("Lastupdate") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Edit by" SortExpression="Editby" meta:resourcekey="TemplateFieldResource4">
                    <EditItemTemplate>
                        <asp:Literal ID="lblEditBy" runat="server" Text='<%# Bind("Editby") %>'></asp:Literal>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="lblEditBy" runat="server" Text='<%# Bind("Editby") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Edit" ShowHeader="False" meta:resourcekey="TemplateFieldResource7">
                    <EditItemTemplate>
                        <asp:ImageButton ID="ImageUpdate" runat="server" CommandName="Update" ImageUrl="~/Images/update.gif"
                            Text="Update" meta:resourcekey="ImageUpdateResource1" ToolTip="Update" />&nbsp;
                        <asp:ImageButton ID="ImageCancel" meta:resourcekey="ImageCancelResource1" runat="server"
                            CausesValidation="False" CommandName="Cancel" ImageUrl="~/Images/cancel.gif"
                            Text="Cancel" ToolTip="Cancel" />
                    </EditItemTemplate>
                    <ItemStyle CssClass="centerObj" />
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageEdit" meta:resourcekey="ImageEditResource1" runat="server"
                            CausesValidation="False" CommandName="Edit" ImageUrl="~/Images/Edit.gif" Text="Edit"
                            ToolTip="Edit" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource5" HeaderText="Delete">
                    <ItemStyle CssClass="centerObj" />
                    <ItemTemplate>
                        <asp:ImageButton ID="imgbDelete" runat="server" CausesValidation="False" CommandName="Delete"
                            ImageUrl="~/Images/Delete.gif" meta:resourcekey="imgbDeleteResource1" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataRowStyle CssClass="errorMsg" ForeColor="Red" Height="40px" />
        </vdms:PageGridView>
    </div>
    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" TypeName="VDMS.I.ObjectDataSource.BrokenDatasource"
        EnablePaging="True" DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="Select"
        SelectCountMethod="SelectCount" UpdateMethod="Update">
        <SelectParameters>
            <asp:ControlParameter Name="fromCode" ControlID="txtFromCode" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter Name="toCode" ControlID="txtToCode" PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
