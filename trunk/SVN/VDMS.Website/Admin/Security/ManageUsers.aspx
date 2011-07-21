<%@ Page Title="User Management" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="ManageUsers.aspx.cs" Inherits="Admin_Security_ManageUser"
	Culture="auto" UICulture="auto" meta:resourcekey="PageResource3" %>

<%@ Register TagPrefix="cc1" Namespace="VDMS.II.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="form" style="width: 450px">
		<table width="100%">
			<tr>
				<td>
					<asp:Localize ID="litUsername" runat="server" Text="Username:" meta:resourcekey="litUsernameResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="txtUsername" runat="server" MaxLength="256" Width="180px" meta:resourcekey="txtUsernameResource1"></asp:TextBox>
				</td>
			</tr>
			<%-- Leo mvbinh --%>
			<tr>
			    <td>
			        <asp:Localize ID="litDealerCode" runat="server" Text="Dealer code:" meta:resourcekey="litDealerCodeResource1"></asp:Localize>
			    </td>
			    <td>
			        <asp:TextBox ID="txtDealerCode" runat="server" MaxLength="256" Width="180px" meta:resourcekey="txtDealerCodeResource1"></asp:TextBox>
			    </td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litDealer" runat="server" Text="Dealer:" meta:resourcekey="litDealerResource1"></asp:Localize>
				</td>
				<td>
					<cc1:DealerList ID="ddlDealer" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
						OnSelectedIndexChanged="ddlDealer_SelectedIndexChanged" OnDataBound="ddlDealer_DataBound"
						EnabledSaperateByArea="False" EnabledSaperateByDB="False" MergeCode="False" meta:resourcekey="ddlDealerResource1"
						RemoveRootItem="False" ShowEmptyItem="False" ShowSelectAllItem="False">
					</cc1:DealerList>
				</td>
			</tr>
			<tr>
				<td>
				</td>
				<td>
					<asp:Button ID="cmdQuery" runat="server" Text="Query" OnClick="cmdQuery_Click" meta:resourcekey="cmdQueryResource1" />
					<asp:Button ID="cmdAddNew" runat="server" Text="Create new User" OnClientClick="javascript:location.href='EditUser.aspx'; return false;"
						meta:resourcekey="cmdAddNewResource1" />
				</td>
			</tr>
		</table>
	</div>
	 <div id="msg" runat="server">
            </div>
            <br />
	<div class="grid">
		<vdms:PageGridView ID="gvwUsers" PageSize="10" runat="server" AutoGenerateColumns="False" DataKeyNames="UserName"
			AllowPaging="True" DataSourceID="odsMembership" OnRowDataBound="gvwUsers_RowDataBound"
			meta:resourcekey="gvwUsersResource2">
			<Columns>
				<asp:BoundField HeaderText="UserName" DataField="UserName" meta:resourcekey="BoundFieldResource1" />
				<asp:HyperLinkField HeaderText="E-mail" DataTextField="Email" DataNavigateUrlFormatString="mailto:{0}"
					DataNavigateUrlFields="Email" meta:resourcekey="HyperLinkFieldResource1" />
				<asp:BoundField HeaderText="Creation Date" DataField="CreationDate" DataFormatString="{0:d}"
					HtmlEncode="False" meta:resourcekey="BoundFieldResource5" />
				<asp:BoundField HeaderText="Last Activity Date" DataField="LastActivityDate" DataFormatString="{0:d}"
					HtmlEncode="False" meta:resourcekey="BoundFieldResource6" />
				<asp:CheckBoxField HeaderText="Is Approved" DataField="IsApproved" meta:resourcekey="CheckBoxFieldResource1"
					ItemStyle-CssClass="center">
					<ItemStyle CssClass="center"></ItemStyle>
				</asp:CheckBoxField>
				<asp:HyperLinkField Text="..." DataNavigateUrlFormatString="EditUser.aspx?UserName={0}"
					DataNavigateUrlFields="UserName" HeaderImageUrl="~/Images/Edit.gif" ItemStyle-CssClass="center"
					meta:resourcekey="HyperLinkFieldResource4">
					<ItemStyle CssClass="center"></ItemStyle>
				</asp:HyperLinkField>
				<asp:ButtonField CommandName="Delete" HeaderImageUrl="~/Images/Delete.gif" Text="..."
					ItemStyle-CssClass="center" meta:resourcekey="ButtonFieldResource2">
					<ItemStyle CssClass="center"></ItemStyle>
				</asp:ButtonField>
			</Columns>
			<EmptyDataTemplate>
				<b>
					<asp:Localize ID="litNotFound" runat="server" Text="No users found for the specified criteria"
						meta:resourcekey="litNotFoundResource1"></asp:Localize></b></EmptyDataTemplate>
		</vdms:PageGridView>
	</div>
	<asp:ObjectDataSource ID="odsMembership" runat="server" EnablePaging="True" TypeName="VDMS.II.Security.MembershipDAO"
		SelectMethod="FindAll" SelectCountMethod="GetCount" DeleteMethod="Delete">
		<SelectParameters>
			<asp:ControlParameter ControlID="txtDealerCode" Name="app" PropertyName="Text" />
			<asp:ControlParameter ControlID="txtUsername" Name="filter" PropertyName="Text" />
		</SelectParameters>
		<DeleteParameters>
			<asp:ControlParameter ControlID="ddlDealer" Name="app" PropertyName="SelectedValue" />
		</DeleteParameters>
	</asp:ObjectDataSource>
</asp:Content>
