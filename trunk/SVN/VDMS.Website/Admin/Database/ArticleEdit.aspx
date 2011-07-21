<%@ Page Title="Create new message" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    ValidateRequest="false" AutoEventWireup="true" CodeFile="ArticleEdit.aspx.cs"
    Inherits="Admin_Database_ArticleEdit" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">

    <script type="text/javascript" src="/js/jHtmlArea-0.7.0.min.js"></script>

    <script type="text/javascript" src="/js/jHtmlArea.ColorPickerMenu-0.7.0.min.js"></script>

    <script type="text/javascript">
        $(function() {
            $("#<%= txtBody.ClientID %>").htmlarea();
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form" style="width: 90%">
        <table cellpadding="2" cellspacing="2" border="0" style="width: 100%;">
            <tr>
                <td align="right" style="width: 20%;">
                    <asp:Localize ID="litContent" runat="server" Text="Content:" meta:resourcekey="litContentResource1"></asp:Localize>
                </td>
                <td>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Save"
                        CssClass="error" meta:resourcekey="ValidationSummary1Resource1" />
                    <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Width="100%" meta:resourcekey="txtBodyResource1"
                        Rows="10" />
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBody"
                        ValidationGroup="Save" SetFocusOnError="true" ErrorMessage="Content cannot be empty!"
                        EnableClientScript="false" Text="*" meta:resourcekey="RequiredFieldValidator1Resource1" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Localize ID="litHotNews" runat="server" Text="Hot News:" meta:resourcekey="litHotNewsResource1"></asp:Localize>
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbhotnews" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Localize ID="litDisplay" runat="server" Text="Display for:" meta:resourcekey="litDisplayResource1"></asp:Localize>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDatabase" runat="server">
                        <asp:ListItem Value="C" Text="Tất cả" meta:resourcekey="ListItemResource10"></asp:ListItem>
                        <asp:ListItem Value="D" Text="Nam" meta:resourcekey="ListItemResource8"></asp:ListItem>
                        <asp:ListItem Value="H" Text="Bắc" meta:resourcekey="ListItemResource9"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <asp:PlaceHolder ID="phFileAttachment" runat="server">
                <tr>
                    <td align="right">
                        <asp:Localize ID="litAttachFile" runat="server" Text="Attach file:" meta:resourcekey="litAttachFileResource1"></asp:Localize>
                    </td>
                    <td>
                        <table cellpadding="1" cellspacing="2" border="0" width="100%">
                            <tr>
                                <td>
                                    <asp:FileUpload ID="fu1" runat="server" Width="300px" meta:resourcekey="fu1Resource1" /><br />
                                    <asp:FileUpload ID="fu2" runat="server" Width="300px" meta:resourcekey="fu2Resource1" /><br />
                                    <asp:FileUpload ID="fu3" runat="server" Width="300px" meta:resourcekey="fu3Resource1" />
                                </td>
                                <td class="help">
                                    <asp:Label ID="lblNote" runat="server" Text="Note that max file size is 5MB. If you attach the bigger, it will be automatically removed."
                                        meta:resourcekey="lblNoteResource1"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                    </td>
                </tr>
            </asp:PlaceHolder>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"
                        ValidationGroup="Save" meta:resourcekey="btnUpdateResource1" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClientClick="javascript:location.href='Article.aspx; return false;"
                        CausesValidation="False" meta:resourcekey="btnCancelResource1" />
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
        <asp:Label ID="lblSaveOk" runat="server" SkinID="MessageOk" Visible="False" Text="The message has been created."
            meta:resourcekey="lblSaveOkResource1"></asp:Label>
    </div>
</asp:Content>
