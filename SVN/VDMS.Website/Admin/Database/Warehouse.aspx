<%@ Page Title="Warehouse List" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="Warehouse.aspx.cs" Inherits="Admin_Database_Warehouse" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<%@ Register TagPrefix="cc1" Namespace="VDMS.II.WebControls" %>
<%@ Register Src="../Controls/Warehouse.ascx" TagName="Warehouse" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form" style="width: 48%">
        <table width="100%">
            <tr>
                <td style="width: 25%">
                    <asp:Localize ID="litDealer" runat="server" Text="Dealer:" 
                        meta:resourcekey="litDealerResource1"></asp:Localize>
                </td>
                <td>
                    <cc1:DealerList ID="ddlDealer" runat="server" AutoPostBack="True" 
                        EnabledSaperateByArea="False" EnabledSaperateByDB="False" MergeCode="False" 
                        meta:resourcekey="ddlDealerResource1" RemoveRootItem="False" 
                        ShowEmptyItem="False" ShowSelectAllItem="False">
                    </cc1:DealerList>
                </td>
            </tr>
        </table>
    </div>
    <asp:ObjectDataSource ID="odsWarehouseList" runat="server" EnablePaging="True" TypeName="VDMS.II.BasicData.WarehouseDAO"
        SelectMethod="FindAll" SelectCountMethod="GetCount" DeleteMethod="Delete" 
        UpdateMethod="Update">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlDealer" Name="DealerCode" PropertyName="SelectedValue"
                Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <div id="Msg" runat="server">
        <asp:ValidationSummary CssClass="error" ID="ValidationSummary1" runat="server" 
            ValidationGroup="Save" meta:resourcekey="ValidationSummary1Resource1" />
    </div>
    <div style="padding: 3px; width: 100%; float: left">
        <div class="grid">
            <vdms:PageGridView ID="gv" runat="server" DataSourceID="odsWarehouseList" AllowPaging="True"
                AutoGenerateColumns="False" DataKeyNames="WarehouseId" 
                OnRowDataBound="gv_RowDataBound" meta:resourcekey="gvResource1">
                <columns>
                    <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource1">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                Text="Edit" meta:resourcekey="LinkButton1Resource1"></asp:LinkButton>&nbsp;
                            <asp:LinkButton ID="lnkbDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                Text="Delete" OnClientClick="if(!confirm(SysMsg[0])) return false;" 
                                meta:resourcekey="lnkbDeleteResource1" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ValidationGroup="Save" ID="LinkButton1" runat="server"
                                CommandName="Update" Text="Update" meta:resourcekey="LinkButton1Resource2"></asp:LinkButton>&nbsp;
                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                                Text="Cancel" meta:resourcekey="LinkButton2Resource1"></asp:LinkButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Warehouse Code" 
                        meta:resourcekey="TemplateFieldResource2">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Code") %>' 
                                meta:resourcekey="Label1Resource1"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtWhCode" MaxLength="30" runat="server" 
                                Text='<%# Bind("Code") %>' meta:resourcekey="txtWhCodeResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ValidationGroup="Save" Text="*" ControlToValidate="txtWhCode"
                                ID="RequiredFieldValidator1" runat="server" 
                                ErrorMessage="Warehouse code cannot be empty!" 
                                meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Warehouse Name">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" MaxLength="255" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Address" 
                        meta:resourcekey="TemplateFieldResource3">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("Address") %>' 
                                meta:resourcekey="Label3Resource1"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" MaxLength="255" runat="server" 
                                Text='<%# Bind("Address") %>' meta:resourcekey="TextBox2Resource1"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </columns>
                <emptydatatemplate>
                    <b>
                        <asp:Localize ID="litNotFound" runat="server" 
                        Text="There are not any warehouses." meta:resourcekey="litNotFoundResource1"></asp:Localize></b>
                </emptydatatemplate>
            </vdms:PageGridView>
        </div>
        <asp:Button ID="cmdCreateNew" runat="server" OnClick="cmdCreateNew_Click" 
            Text="Create new Warehouse" meta:resourcekey="cmdCreateNewResource1" />
    </div>
    <div style="width: 400px; float: right">
        <div id="divRight" runat="server" visible="false" class="normalBox">
            <uc1:Warehouse ID="wh1" runat="server" />
            <table style="width: 100%;" cellpadding="2" cellspacing="2" border="0">
                <tr>
                    <td>
                    </td>
                    <td align="center">
                        <asp:Button ID="cmdSave" runat="server" Text="Save" ValidationGroup="Save" 
                            OnClick="cmdSave_Click" meta:resourcekey="cmdSaveResource1" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
