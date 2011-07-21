<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="Branch.aspx.cs" Inherits="Admin_Database_Branch" Title="Sửa đổi chi nhánh"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
    <asp:ValidationSummary ID="ValidationSummary1" CssClass="error" runat="server" ValidationGroup="Save"
        meta:resourcekey="ValidationSummary1Resource1"></asp:ValidationSummary>
    <asp:MultiView ID="mvMain" runat="server" ActiveViewIndex="0">
        <asp:View ID="view1" runat="server">
            <div class="grid" style="float: left; width: 60%">
                <vdms:PageGridView ID="gvSubShop" runat="server" DataSourceID="ObjectDataSource1"
                    AutoGenerateColumns="False" AllowPaging="True" DataKeyNames="Id" OnRowDataBound="gvSubShop_RowDataBound"
                    OnRowEditing="gvSubShop_RowEditing" OnDataBound="gvSubShop_DataBound" meta:resourcekey="gvSubShopResource1">
                    <Columns>
                        <asp:BoundField ReadOnly="True" ItemStyle-CssClass="center" HeaderText="No" meta:resourcekey="BoundFieldResource1"></asp:BoundField>
                        <asp:TemplateField HeaderText="Use status" meta:resourcekey="TemplateFieldResource1">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" Enabled="false" Checked='<%#Eval("Status") %>' runat="server" />
                            </ItemTemplate>
                            <ItemStyle CssClass="center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Code" HeaderText="Code" meta:resourcekey="BoundFieldResource2">
                        </asp:BoundField>
                        <asp:BoundField DataField="Name" HeaderText="Name" meta:resourcekey="BoundFieldResource3">
                        </asp:BoundField>
                        <asp:BoundField DataField="Address" SortExpression="Address" HeaderText="Address"
                            meta:resourcekey="BoundFieldResource4"></asp:BoundField>
                        <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource2">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                    ToolTip="Edit" ImageUrl="~/Images/Edit.gif" meta:resourcekey="imgbEditResource1" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource3">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ToolTip="Delete" ImageUrl="~/Images/Delete.gif" meta:resourcekey="imgbDeleteResource1" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <b>
                            <asp:Localize ID="litNotFound" runat="server" Text="There are not any sub shop" meta:resourcekey="litNotFoundResource1"></asp:Localize></b>
                    </EmptyDataTemplate>
                </vdms:PageGridView>
            </div>
            <div class="help" style="float: right; width: 25%;">
                <ul>
                    <li>
                        <asp:Localize ID="lh1" runat="server" Text="Note that you can only delete the sub shop that is not in use."
                            meta:resourcekey="lblDeleteNoteResource1"></asp:Localize>
                    </li>
                </ul>
            </div>
            <div style="clear: both;">
            </div>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="Select"
                DeleteMethod="Delete" TypeName="VDMS.I.ObjectDataSource.SubShopDataSource" EnablePaging="True"
                SelectCountMethod="SelectCount">
                <SelectParameters>
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="Id" Type="Int32" />
                </DeleteParameters>
            </asp:ObjectDataSource>
            <br />
            <asp:Button ID="btnAdd" OnClick="btnAdd_Click" runat="server" Text="Add new" SkinID="SubmitButton"
                meta:resourcekey="btnAddResource1"></asp:Button>
        </asp:View>
        <asp:View ID="view2" runat="server">
            <asp:ValidationSummary ID="vsMain" CssClass="error" runat="server" meta:resourcekey="vsMainResource1" />
            <asp:Label ID="litError" runat="server" CssClass="errorMsg" Text="Duplicate in code field"
                Visible="False" meta:resourcekey="litErrorResource1"></asp:Label>
            <div class="form">
                <table cellpadding="2" cellspacing="2" border="0" width="100%">
                    <tr>
                        <td style="width: 120px;">
                            <asp:Image ID="image1" runat="server" SkinID="RequireField" meta:resourcekey="image1Resource1" /><asp:Localize
                                ID="litBranchCode" runat="server" Text="Branch code:" meta:resourcekey="litBranchCodeResource1"></asp:Localize>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCode" runat="server" MaxLength="30" meta:resourcekey="txtCodeResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvBranchCode" runat="server" ControlToValidate="txtCode"
                                SetFocusOnError="True" ErrorMessage="Banch code is required." Text="*" meta:resourcekey="rfvBranchCodeResource1"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revCode" runat="server" ControlToValidate="txtCode"
                                ValidationExpression="^([a-zA-Z0-9_]{3,15})$" SetFocusOnError="True" ErrorMessage="Code is invalid (only character, number, underscore, length from 3 to 15)"
                                Text="*" meta:resourcekey="revCodeResource1"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" /><asp:Localize
                                ID="litBranchName" runat="server" Text="Sub shop name:" meta:resourcekey="litBranchNameResource1"></asp:Localize>
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" MaxLength="256" meta:resourcekey="txtNameResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                                SetFocusOnError="True" ErrorMessage="Name id required." Text="*" meta:resourcekey="rfvNameResource1"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="image3" runat="server" SkinID="RequireField" meta:resourcekey="image3Resource1" /><asp:Localize
                                ID="litAddress" runat="server" Text="Address:" meta:resourcekey="litAddressResource1"></asp:Localize>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress" runat="server" MaxLength="512" meta:resourcekey="txtAddressResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress"
                                SetFocusOnError="True" ErrorMessage="Address is required." Text="*" meta:resourcekey="rfvAddressResource1"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="image2" runat="server" SkinID="RequireField" meta:resourcekey="image2Resource1" /><asp:Localize
                                ID="litStatus" runat="server" Text="Status:" meta:resourcekey="litStatusResource1"></asp:Localize>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkStatus" runat="server" Text="This sub shop is in use" Checked="true"
                                meta:resourcekey="chkStatusResource1"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_OnClick" Text="Update"
                                meta:resourcekey="btnUpdateResource1" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                                OnClick="btnCancel_Click" meta:resourcekey="btnCancelResource1" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
