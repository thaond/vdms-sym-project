<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="Permission.aspx.cs" Inherits="Admin_Security_Permission" Title="Permission"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Src="~/Controls/UpdateProgress.ascx" TagName="UpdateProgress" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">

    <script language="javascript">
        function updated() {
            //  close the popup
            tb_remove();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <div id="msg" runat="server">
        </div>
        <asp:Literal ID="Literal1" runat="server" Text="Setting for:" meta:resourcekey="Literal1Resource1"></asp:Literal>
        <asp:TextBox ID="txtTargetCode" runat="server" meta:resourcekey="txtTargetCodeResource1"></asp:TextBox>
        <asp:Localize ID="Localize1" runat="server" Text="Role list:" meta:resourcekey="Localize1Resource1"></asp:Localize>
        <asp:DropDownList ID="ddlRoles"  runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRoles_SelectedIndexChanged"
            meta:resourcekey="ddlRolesResource1" DataSourceID="odsRole" DataTextField="RoleName"
            DataValueField="RoleName" OnDataBound="ddlRoles_DataBound">
        </asp:DropDownList>
        <asp:ObjectDataSource ID="odsRole" runat="server" SelectMethod="GetRoles" TypeName="VDMS.II.Security.RoleDAO">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtTargetCode" Name="appName" PropertyName="Text"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    <div style="float: left;" class="normalBox">
        <asp:TreeView ID="tvRole" runat="server" ShowLines="True" ShowCheckBoxes="All" DataSourceID="siteMapDS"
            OnTreeNodeDataBound="tvRole_TreeNodeDataBound">
            <SelectedNodeStyle BackColor="#FFFF66" />
            <LeafNodeStyle CssClass="thickbox" />
            <LevelStyles>
                <asp:TreeNodeStyle CssClass="thickbox" />
                <asp:TreeNodeStyle />
                <asp:TreeNodeStyle />
                <asp:TreeNodeStyle CssClass="thickbox" />
            </LevelStyles>
        </asp:TreeView>
        <asp:SiteMapDataSource ID="siteMapDS" runat="server" />
        <br />
        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" meta:resourcekey="btnSaveResource1" />
        <asp:Button ID="btnSavePath" runat="server" OnClick="btnSavePath_Click" Text="Update site map"
            meta:resourcekey="btnSavePathResource1" />
    </div>
</asp:Content>
