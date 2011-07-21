<%@ Page Title="Vendor List" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="Vendor.aspx.cs" Inherits="Admin_Database_Vendor" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<%@ Register Src="~/Controls/ContactInfo.ascx" TagName="ContactInfo" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <asp:ObjectDataSource ID="odsCustomerList" runat="server" EnablePaging="True" TypeName="VDMS.II.BasicData.VendorDAO"
        SelectMethod="FindAll" SelectCountMethod="GetCount" 
        DeleteMethod="Delete" />
    <div style="padding: 3px; width: 100%">
        <div class="grid">
            <vdms:PageGridView ID="gv" runat="server" DataSourceID="odsCustomerList" AllowPaging="True" 
            DataKeyNames="VendorId" OnSelectedIndexChanged="gv_SelectedIndexChanged" 
                onrowdeleted="gv_RowDeleted" meta:resourcekey="gvResource1">
                <Columns>
                    <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource1">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                CommandName="Select" Text="Edit" meta:resourcekey="LinkButton1Resource1" ></asp:LinkButton>
                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" Visible='<%# (bool)Eval("CanDelete") %>'
                                CommandName="Delete" Text="Delete" 
                                OnClientClick="if(!confirm(SysMsg[0])) return false;" 
                                meta:resourcekey="LinkButton2Resource1"  />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Code" HeaderText="Vendor Code" 
                        meta:resourcekey="BoundFieldResource1" />
                    <asp:BoundField DataField="Name" HeaderText="Vendor Name" 
                        meta:resourcekey="BoundFieldResource2" />
                    <asp:BoundField DataField="FullName" HeaderText="Full Name" 
                        meta:resourcekey="BoundFieldResource3" />
                    <asp:BoundField DataField="Phone" HeaderText="Phone" 
                        meta:resourcekey="BoundFieldResource4" />
                    <asp:BoundField DataField="Mobile" HeaderText="Mobile" 
                        meta:resourcekey="BoundFieldResource5" />
                    <asp:BoundField DataField="Fax" HeaderText="Fax" 
                        meta:resourcekey="BoundFieldResource6" />
                    <asp:BoundField DataField="Email" HeaderText="Email" 
                        meta:resourcekey="BoundFieldResource7" />
                    <asp:BoundField DataField="Address" HeaderText="Address" 
                        meta:resourcekey="BoundFieldResource8" />
                    <asp:BoundField DataField="AdditionalInfo" HeaderText="Info" 
                        meta:resourcekey="BoundFieldResource9" />
                </Columns>
                <EmptyDataTemplate>
                    <b>
                        <asp:Localize ID="litNotFound" runat="server" 
                        Text="There are not any vendors." meta:resourcekey="litNotFoundResource1"></asp:Localize></b>
                </EmptyDataTemplate>
            </vdms:PageGridView>
        </div>
        <asp:Button ID="cmdCreateNew" runat="server" Text="Add new Vendor" 
            OnClick="cmdCreateNew_Click" meta:resourcekey="cmdCreateNewResource1" />
    </div>
    <div style="padding: 3px; width: 49%">
        <div id="divRight" runat="server" visible="false" class="normalBox">
            <div class="boxHeader">
                <asp:Localize ID="litBasicInfo" runat="server" Text="Vendor Basic Information" 
                    meta:resourcekey="litBasicInfoResource1"></asp:Localize>
            </div>
            <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Save" runat="server"
                CssClass="error" meta:resourcekey="ValidationSummary1Resource1" />
            <table width="100%">
                <tr>
                    <td style="width: 25%;">
                        <asp:Image ID="image2" runat="server" SkinID="RequireField" 
                            meta:resourcekey="image2Resource1" />
                        <asp:Localize ID="litCode" runat="server" Text="Code:" 
                            meta:resourcekey="litCodeResource1"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox MaxLength="30" ID="txtCode" runat="server" 
                            meta:resourcekey="txtCodeResource1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv1" runat="server" ErrorMessage="Vendor code cannot be empty!"
                            ControlToValidate="txtCode" SetFocusOnError="True" ValidationGroup="Save" 
                            Text="*" meta:resourcekey="rfv1Resource1"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Image ID="image1" runat="server" SkinID="RequireField" 
                            meta:resourcekey="image1Resource1" />
                        <asp:Localize ID="litName" runat="server" Text="Name:" 
                            meta:resourcekey="litNameResource1"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox MaxLength="250" ID="txtName" runat="server" 
                            meta:resourcekey="txtNameResource1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv2" runat="server" ErrorMessage="Vendor name cannot be empty!"
                            ControlToValidate="txtName" SetFocusOnError="True" ValidationGroup="Save" 
                            Text="*" meta:resourcekey="rfv2Resource1"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <br />
            <div class="boxHeader">
                <asp:Localize ID="litAddInfo" runat="server" Text="Vendor Addition Information" 
                    meta:resourcekey="litAddInfoResource1"></asp:Localize>
            </div>
            <cc1:ContactInfo ID="ci" runat="server" />
            <br />
            <asp:Button ID="cmdSave" runat="server" ValidationGroup="Save" Text="Save" 
                OnClick="cmdSave_Click" meta:resourcekey="cmdSaveResource1" />
        </div>
    </div>
</asp:Content>
