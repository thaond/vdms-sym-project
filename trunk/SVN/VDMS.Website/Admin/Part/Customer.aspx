<%@ Page Title="Customer List" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="Customer.aspx.cs" Inherits="Admin_Part_Customer"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Src="~/Controls/ContactInfo.ascx" TagName="ContactInfo" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <asp:ObjectDataSource ID="odsCustomerList" runat="server" EnablePaging="True" TypeName="VDMS.II.BasicData.CustomerDAO"
        SelectMethod="FindAll" SelectCountMethod="GetCount" DeleteMethod="Delete" />
    <div style="padding: 3px; width: 100%">
        <div class="grid">
            <vdms:PageGridView ID="gv" runat="server" DataSourceID="odsCustomerList" AllowPaging="True"
                DataKeyNames="CustomerId" AutoGenerateColumns="False" OnSelectedIndexChanged="gv_SelectedIndexChanged"
                meta:resourcekey="gvResource1" OnRowDeleted="gv_RowDeleted">
                <Columns>
                    <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource1">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select"
                                Text="Edit" meta:resourcekey="LinkButton1Resource1"></asp:LinkButton>
                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" Visible='<%# EvalCanDelete(Eval("SalesHeaders")) %>'
                                CommandName="Delete" Text="Delete" OnClientClick="if(!confirm(SysMsg[0])) return false;"
                                meta:resourcekey="LinkButton2Resource1" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Code" HeaderText="Code" meta:resourcekey="BoundFieldResource1" />
                    <asp:BoundField DataField="VATCode" HeaderText="VAT Code" meta:resourcekey="BoundFieldResource1x" />
                    <asp:BoundField DataField="Name" HeaderText="Name" meta:resourcekey="BoundFieldResource2" />
                    <asp:TemplateField HeaderText="Full Name" meta:resourcekey="TemplateFieldResource2">
                        <ItemTemplate>
                            <asp:Label ID="Label1a" runat="server" Text='<%# Eval("Contact.FullName") %>' meta:resourcekey="Label1aResource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phone" meta:resourcekey="TemplateFieldResource3">
                        <ItemTemplate>
                            <asp:Label ID="Label1x" runat="server" Text='<%# Eval("Contact.Phone") %>' meta:resourcekey="Label1Resource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mobile" 
                        meta:resourcekey="TemplateFieldResource7">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Contact.Mobile") %>' 
                                meta:resourcekey="Label1Resource2"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fax" meta:resourcekey="TemplateFieldResource8">
                        <ItemTemplate>
                            <asp:Label ID="Label13" runat="server" Text='<%# Eval("Contact.Fax") %>' 
                                meta:resourcekey="Label13Resource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mail" meta:resourcekey="TemplateFieldResource4">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("Contact.Email") %>' meta:resourcekey="Label2Resource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Address" meta:resourcekey="TemplateFieldResource5">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("Contact.Address") %>' meta:resourcekey="Label3Resource2"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Info" meta:resourcekey="TemplateFieldResource6">
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("Contact.AdditionalInfo") %>'
                                meta:resourcekey="Label4Resource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <b>
                        <asp:Localize ID="litNotFound" runat="server" Text="There are not any customers."
                            meta:resourcekey="litNotFoundResource1"></asp:Localize></b>
                </EmptyDataTemplate>
            </vdms:PageGridView>
        </div>
        <asp:Button ID="cmdCreateNew" runat="server" Text="Add new Customer" OnClick="cmdCreateNew_Click"
            meta:resourcekey="cmdCreateNewResource1" />
    </div>
    <div style="padding: 3px; width: 49%">
        <div id="divRight" runat="server" visible="false" class="normalBox">
            <div class="boxHeader">
                <asp:Localize ID="litBasicInfo" runat="server" Text="Customer Basic Information"
                    meta:resourcekey="litBasicInfoResource2"></asp:Localize>
                <br />
            </div>
            <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Save" runat="server"
                CssClass="error" meta:resourcekey="ValidationSummary1Resource2" />
            <table width="100%">
                <tr>
                    <td style="width: 25%;">
                        <asp:Image ID="image2" runat="server" SkinID="RequireField" meta:resourcekey="image2Resource2" />
                        <asp:Localize ID="litCode" runat="server" Text="Code:" meta:resourcekey="litCodeResource2"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCode" MaxLength="30" runat="server" meta:resourcekey="txtCodeResource2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv1" runat="server" ErrorMessage="Customer code can not be blank!"
                            ControlToValidate="txtCode" SetFocusOnError="True" ValidationGroup="Save" Text="*"
                            meta:resourcekey="rfv1Resource1"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Image ID="image1" runat="server" SkinID="RequireField" meta:resourcekey="image1Resource2" />
                        <asp:Localize ID="litName" runat="server" Text="Name:" meta:resourcekey="litNameResource2"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" MaxLength="255" runat="server" meta:resourcekey="txtNameResource2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv2" runat="server" ErrorMessage="Customer name can not be blank!"
                            ControlToValidate="txtName" SetFocusOnError="True" ValidationGroup="Save" Text="*"
                            meta:resourcekey="rfv2Resource1"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Image ID="image3" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image1Resource2" />
                        <asp:Localize ID="Localize1" runat="server" Text="VAT code:" meta:resourcekey="litVATResource2"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox ID="txtVAT" MaxLength="255" runat="server" 
                            meta:resourcekey="txtVATResource1"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
            <div class="boxHeader">
                <asp:Localize ID="litAddInfo" runat="server" Text="Customer Addition Information"
                    meta:resourcekey="litAddInfoResource1"></asp:Localize>
            </div>
            <cc1:ContactInfo ID="ci" runat="server" />
            <br />
            <asp:Button ID="cmdSave" runat="server" Text="Save" OnClick="cmdSave_Click" ValidationGroup="Save"
                meta:resourcekey="cmdSaveResource2" />
        </div>
    </div>
</asp:Content>
