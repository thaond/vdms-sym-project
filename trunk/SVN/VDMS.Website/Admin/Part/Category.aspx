<%@ Page Title="Category Setting" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="Category.aspx.cs" Inherits="Admin_Part_Category" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<%@ Register TagPrefix="cc1" Namespace="VDMS.II.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div style="padding: 3px; width: 60%; float: left">
		<div class="grid">
			<asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Update"
				CssClass="error" meta:resourcekey="ValidationSummary2Resource1" />
			<vdms:PageGridView ID="gv" runat="server" DataSourceID="CategoryDataSource" DataKeyNames="CategoryId"
				AllowPaging="True" AutoGenerateColumns="False" meta:resourcekey="gvResource1">
				<Columns>
					<asp:TemplateField InsertVisible="False" ShowHeader="False" 
                        meta:resourcekey="TemplateFieldResource1">
						<ItemTemplate>
							<asp:LinkButton ID="lnkbEdit" runat="server" CausesValidation="False" CommandName="Edit"
								Text="Edit" meta:resourcekey="lnkbEditResource1"></asp:LinkButton>
							<asp:LinkButton ID="lkbDelete" runat="server" CausesValidation="False" Visible='<%# EvalCanDelete(Eval("PartInfos")) %>'
								CommandName="Delete" Text="Delete" OnClientClick="if(!confirm(SysMsg[0])) return false;" 
                                meta:resourcekey="lkbDeleteResource1"></asp:LinkButton>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:LinkButton ID="lnkbUpdate" runat="server" ValidationGroup="Update"
								CommandName="Update" Text="Update" meta:resourcekey="lnkbUpdateResource1"></asp:LinkButton>
							<asp:LinkButton ID="lkbCancel" runat="server" CausesValidation="False" CommandName="Cancel"
								Text="Cancel" meta:resourcekey="lkbCancelResource1"></asp:LinkButton>
						</EditItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Code" meta:resourcekey="TemplateFieldResource2">
						<ItemTemplate>
							<asp:Label ID="Label1" runat="server" Text='<%# Bind("Code") %>' 
                                meta:resourcekey="Label1Resource1"></asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox ID="TextBox1" MaxLength="5" runat="server" 
                                Text='<%# Bind("Code") %>' meta:resourcekey="TextBox1Resource1"></asp:TextBox>
							<asp:RequiredFieldValidator ID="rfv1" runat="server" ErrorMessage="Category code cannot be empty!"
								ControlToValidate="TextBox1" SetFocusOnError="True" ValidationGroup="Update"
								Text="*" meta:resourcekey="rfv1Resource1"></asp:RequiredFieldValidator>
						</EditItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Name" meta:resourcekey="TemplateFieldResource3">
						<ItemTemplate>
							<asp:Label ID="Label2" runat="server" Text='<%# Bind("Name") %>' 
                                meta:resourcekey="Label2Resource1"></asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox ID="TextBox2" MaxLength="256" runat="server" 
                                Text='<%# Bind("Name") %>' meta:resourcekey="TextBox2Resource1"></asp:TextBox>
							<asp:RequiredFieldValidator ID="rfv2" runat="server" ErrorMessage="Category name cannot be empty"
								ControlToValidate="TextBox2" SetFocusOnError="True" ValidationGroup="Update"
								Text="*" meta:resourcekey="rfv2Resource1"></asp:RequiredFieldValidator>
						</EditItemTemplate>
					</asp:TemplateField>
				</Columns>
				<EmptyDataTemplate>
					There is not any category.
				</EmptyDataTemplate>
			</vdms:PageGridView>
		</div>
		<asp:ObjectDataSource ID="CategoryDataSource" runat="server" EnablePaging="True"
			TypeName="VDMS.II.BasicData.CategoryDAO" SelectMethod="FindAll" SelectCountMethod="GetCount"
			DeleteMethod="Delete" UpdateMethod="Update">
			<DeleteParameters>
				<asp:Parameter Name="CategoryId" Type="Int64" />
			</DeleteParameters>
			<UpdateParameters>
				<asp:Parameter Name="CategoryId" Type="Int64" />
				<asp:Parameter Name="Code" Type="String" />
				<asp:Parameter Name="Name" Type="String" />
			</UpdateParameters>
			<SelectParameters>
			</SelectParameters>
		</asp:ObjectDataSource>
	</div>
	<div style="padding: 3px; width: 35%; float: right;">
		<div id="divRight" class="normalBox" runat="server">
			<asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Save"
				CssClass="error" meta:resourcekey="ValidationSummary1Resource1" />
			<table cellpadding="2" cellspacing="2" border="0">
				<tr>
					<td style="width: 45%;">
						<asp:Image ID="image2" runat="server" SkinID="RequireField" 
                            meta:resourcekey="image2Resource1" />
						<asp:Localize ID="litCode" runat="server" Text="Category Code:" 
                            meta:resourcekey="litCodeResource1"></asp:Localize>
					</td>
					<td>
						<asp:TextBox ID="txtCode" MaxLength="5" runat="server" 
                            meta:resourcekey="txtCodeResource1"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" runat="server" ErrorMessage="Category code cannot be empty!"
							ControlToValidate="txtCode" SetFocusOnError="True" ValidationGroup="Save" Text="*" 
                            meta:resourcekey="rfv1Resource2"></asp:RequiredFieldValidator>
					</td>
				</tr>
				<tr>
					<td style="width: 45%;">
						<asp:Image ID="image1" runat="server" SkinID="RequireField" 
                            meta:resourcekey="image1Resource1" />
						<asp:Localize ID="litName" runat="server" Text="Category Name:" 
                            meta:resourcekey="litNameResource1"></asp:Localize>
					</td>
					<td>
						<asp:TextBox ID="txtName" MaxLength="256" runat="server" 
                            meta:resourcekey="txtNameResource1"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv2" runat="server" ErrorMessage="Category name cannot be empty"
							ControlToValidate="txtName" SetFocusOnError="True" ValidationGroup="Save" Text="*" 
                            meta:resourcekey="rfv2Resource2"></asp:RequiredFieldValidator>
					</td>
				</tr>
				<tr>
					<td>
					</td>
					<td>
						<asp:Button ID="cmdSave" runat="server" ValidationGroup="Save" Text="Create" 
                            OnClick="cmdSave_Click" meta:resourcekey="cmdSaveResource1" />
					</td>
				</tr>
			</table>
		</div>
	</div>
</asp:Content>
