<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
	CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="paragraph">
		<p>
			Please select the task below.</p>
	</div>
	<table class="homeTable" border="1">
		<tr>
			<td class="sampleHeader">
				Task
			</td>
			<td class="descriptionHeader">
				Description
			</td>
		</tr>
		<tr>
			<td class="sample">
				<a href="Users.aspx">Setting Membership, Roles, and Profile</a>
			</td>
			<td class="description">
				This task allow you add and delete users, add new roles and assign them to users.
			</td>
		</tr>
		<tr>
			<td class="sample">
				<a href="WebParts.aspx">Setting SiteMap</a>
			</td>
			<td class="description">
				This task allow you init the site map of VDMS
			</td>
		</tr>
		<tr>
			<td class="sample">
				<asp:Literal ID="Literal1" runat="server" Text="Create VMEP acount"></asp:Literal>
			</td>
			<td class="description">
				<asp:Literal ID="Literal2" runat="server" Text="Username: "></asp:Literal>
				<asp:TextBox ID="txtUserN" runat="server"></asp:TextBox>
				<asp:Button ID="btnCreate" runat="server" Text="Create" 
                    onclick="btnCreate_Click" />
			</td>
		</tr>
		<tr>
			<td class="sample">
				<asp:Literal ID="Literal4" runat="server" Text="Reset VMEP acount"></asp:Literal>
			</td>
			<td class="description">
				<asp:Literal ID="Literal5" runat="server" Text="Username: "></asp:Literal>
				<asp:TextBox ID="txtUserO" runat="server"></asp:TextBox>
				<asp:Button ID="btnResetAdminAcc" runat="server" OnClick="btnResetAdminAcc_Click" Text="Reset" />
			</td>
		</tr>
		<tr>
			<td class="sample">
				<asp:Literal ID="Literal3" runat="server" Text="Reset db'sequence"></asp:Literal>
			</td>
			<td class="description">
				<asp:Button ID="cmdResetSequenceI" runat="server" Text="Reset VDMS-I" 
                    OnClick="cmdResetSequenceI_Click" />
			&nbsp;<asp:Button ID="cmdResetSequenceII" runat="server" Text="Reset VDMS-II" 
                    OnClick="cmdResetSequence_Click" />
			</td>
		</tr>
	</table>
</asp:Content>
