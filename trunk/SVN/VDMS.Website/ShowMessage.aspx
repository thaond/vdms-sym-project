<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowMessage.aspx.cs" Inherits="ShowMessage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding: 10px;">
        <span style="color:#333333; font-weight:bold;"><%= message.CreatedBy %>(<%= message.CreatedDate %>)</span>
        <hr />
        <br />
        <%= message.Body %>
        <br />
        <br />
        <span style="color:#333333;">File Attachment:</span>
        <hr />
        <asp:Repeater ID="repAttachment" runat="server">
            <ItemTemplate>
                <asp:HyperLink ID="hlAttachment" runat="server" Text='<%# (string)Eval("Filename") %>'
                    CssClass="attach-file" ToolTip='<%# Bind("Filename") %>' NavigateUrl='<%# string.Concat("~/download.ashx?id=", Eval("FileId")) %>'></asp:HyperLink>
            </ItemTemplate>
            <SeparatorTemplate>
                ,</SeparatorTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>
