<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
	CodeFile="RepairHistory.aspx.cs" Inherits="Service_RepairHistory" Title="Repair and warranty history"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="VDMS.Common" Namespace="VDMS.Common.Web" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
	<div class="grid">
		<cc1:EmptyGridViewEx GridLines="Both" ID="EmptyGridViewEx1" runat="server" Caption="Repair and warranty history"
			CssClass="GridView" AllowInsertEmptyRow="False" AutoGenerateColumns="False" EnableViewState="False"
			IncludeChildsListInLevel="True" OnRowDataBound="EmptyGridViewEx1_RowDataBound"
			RealPageSize="10" ShowEmptyTable="True" EmptyTableRowText="No infomations found!"
			GennerateSpanDataTable="False" meta:resourcekey="EmptyGridViewEx1Resource1" ShowEmptyFooter="True"
			Width="100%" OnRowDeleting="EmptyGridViewEx1_RowDeleting">
			<Columns>
				<asp:TemplateField HeaderText="Repair date" meta:resourcekey="TemplateFieldResource1">
					<ItemStyle HorizontalAlign="Center" Wrap="False" />
					<ItemTemplate>
						<asp:Literal runat="server" ID="litRepairDate" Text='<%# Eval("RepairDate") %>'></asp:Literal>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Dealer" meta:resourcekey="TemplateFieldResource6">
					<ItemTemplate>
						<asp:Literal runat="server" ID="litDealer" Text='<%# Eval("Dealer") %>'></asp:Literal>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Dealer" meta:resourcekey="TemplateFieldResource6Name">
					<ItemTemplate>
						<asp:Literal runat="server" ID="litDealerName" Text='<%# Eval("DealerName") %>'></asp:Literal>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Damaged" meta:resourcekey="TemplateFieldResource2">
					<ItemTemplate>
						<asp:Literal runat="server" ID="Literal2" Text='<%# Eval("Damaged") %>'></asp:Literal>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Repair" meta:resourcekey="TemplateFieldResource3">
					<ItemTemplate>
						<asp:Literal runat="server" ID="Literal3" Text='<%# Eval("Repair") %>'></asp:Literal>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Fee amount" meta:resourcekey="TemplateFieldResource7">
					<ItemStyle HorizontalAlign="Right" />
					<ItemTemplate>
						<asp:Literal runat="server" ID="litFee" Text='<%# FormatLong(Eval("RepairFee"), 0) %>'></asp:Literal>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Spare number" meta:resourcekey="TemplateFieldResource4">
					<ItemStyle Wrap="False" />
					<ItemTemplate>
						<asp:Literal runat="server" ID="Literal4" Text='<%# Eval("SpareNumber") %>'></asp:Literal>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Warranty" meta:resourcekey="TemplateFieldResource5">
					<ItemStyle HorizontalAlign="Center" Wrap="True" />
					<HeaderStyle Wrap="False" />
					<ItemTemplate>
						<asp:CheckBox runat="server" Enabled="False" Text='<%# Bind("IsWarranty") %>' ID="cbbWarr"
							meta:resourceKey="cbbWarrResource1"></asp:CheckBox>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemStyle HorizontalAlign="Center" Width="1px" />
					<ItemTemplate>
						<asp:ImageButton ID="imgbDelete" OnClick="imgbDelete_Click" runat="server" meta:resourcekey="imgbDeleteResource1"
							CausesValidation="False" CommandName="Delete" ImageUrl="~/Images/Delete.gif"
							OnDataBinding="imgbDelete_DataBinding" CommandArgument='<%# Eval("ServiceHeaderId") %>'
							Visible='<%# EvalDeleteVisibility(Eval("ServiceHeaderId")) %>'></asp:ImageButton>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</cc1:EmptyGridViewEx>
		<p align="center">
			<asp:Button ID="Button1" runat="server" OnClientClick="window.close(); return false;"
				Text="OK" Width="54px" meta:resourcekey="Button1Resource1" />
		</p>
	</div>
</asp:Content>
