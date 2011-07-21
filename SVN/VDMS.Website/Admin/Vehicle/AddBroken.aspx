<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="AddBroken.aspx.cs" Inherits="Admin_Database_AddBroken" Title="Untitled Page"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Save"
        meta:resourcekey="ValidationSummary1Resource1" />
    <asp:Label ID="lblError" runat="server" CssClass="errorMsg" meta:resourcekey="lblErrorResource1"></asp:Label>
    <br />
    <div class="form">
        <table border="0" cellpadding="2" cellspacing="0" style="width: 100%">
            <tr>
                <td align="right">
                    <asp:Image ID="image" runat="server" SkinID="RequireField" />
                    <asp:Literal ID="Literal2" runat="server" Text="Broken code : " meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtBrokenCode" runat="server" Text='<%# Bind("Id") %>' Width="94px"
                        meta:resourcekey="txtBrokenCodeResource1" ValidationGroup="Save" MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBrokenCode"
                        ErrorMessage="You must enter Broken code" ValidationGroup="Save" meta:resourcekey="RequiredFieldValidator1Resource1"
                        Text="*"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                            runat="server" ControlToValidate="txtBrokenCode" ErrorMessage='"Broken code" must input numeric only!'
                            ValidationExpression="\s*\d*\s*" ValidationGroup="Save" meta:resourcekey="RegularExpressionValidator1Resource1">*</asp:RegularExpressionValidator>
                </td>
                <td align="right">
                    <asp:Image ID="Image1" runat="server" SkinID="RequireField" />
                    <asp:Literal ID="Literal3" runat="server" Text="Broken name : " meta:resourcekey="Literal3Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtBrokenName" runat="server" Text='<%# Bind("Brokenname") %>' meta:resourcekey="txtBrokenNameResource1"
                        Width="250px" MaxLength="256"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBrokenName"
                        ErrorMessage="You must enter Broken name" ValidationGroup="Save" meta:resourcekey="RequiredFieldValidator2Resource1"
                        Text="*"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Literal ID="Literal5" runat="server" Text="Last update :" meta:resourcekey="Literal5Resource1"></asp:Literal>
                </td>
                <td class="valueField">
                    <asp:Literal ID="lblLastUpdate" runat="server" Text="Label"></asp:Literal>
                </td>
                <td align="right">
                    <asp:Literal ID="Literal4" runat="server" Text="Editby :" meta:resourcekey="Literal4Resource1"></asp:Literal>
                </td>
                <td class="valueField">
                    <asp:Literal ID="lblEditBy" runat="server" Text="Label"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td colspan="2">
                    <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="Save" meta:resourcekey="btnSaveResource1"
                        OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel"
                        meta:resourcekey="btnCancelResource1" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
