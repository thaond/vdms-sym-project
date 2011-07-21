<%@ Page Title="NotGoodAuto" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="NotGoodAuto.aspx.cs" Inherits="Part_Inventory_NotGoodAuto"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="help">
		<ul>
			<li>
				<asp:Localize ID="lh1" runat="server" Text="This form is used only when the receive data has abnormal case (broken, wrong, lack part) and automaticly call by system (user cannot call it)."
					meta:resourcekey="lh1Resource1"></asp:Localize></li>
			<li>
				<asp:Localize ID="lh2" runat="server" Text="It display the abnormal case to user, so that user can verify the input. Ater verify, all the transaction (receive and NG form) will be saved and cannot modify."
					meta:resourcekey="lh2Resource1"></asp:Localize>
			</li>
		</ul>
	</div>
	<div style="clear: both;">
	</div>
	<br />
	<asp:Label ID="lblSaveOk" runat="server" SkinID="MessageOk" Visible="False" Text="The receive has been import to stock successful."
		meta:resourcekey="lblSaveOkResource1"></asp:Label>
	<asp:ListView ID="lv" runat="server">
		<LayoutTemplate>
			<div id="grid" class="grid">
				<div class="title">
					<asp:Literal ID="Literal1" runat="server" Text="Not Good Issue" meta:resourcekey="Literal1Resource1"></asp:Literal>
				</div>
				<table class="datatable" cellpadding="0" cellspacing="0">
					<thead>
						<tr>
							<th rowspan="2" class="double">
								<asp:Literal ID="Literal2" runat="server" Text="Part Code" meta:resourcekey="Literal2Resource1"></asp:Literal>
							</th>
							<th colspan="2">
								<asp:Literal ID="Literal3" runat="server" Text="Part Name" meta:resourcekey="Literal3Resource1"></asp:Literal>
							</th>
							<th colspan="3">
								<asp:Literal ID="Literal4" runat="server" Text="Quantity" meta:resourcekey="Literal4Resource1"></asp:Literal>
							</th>
							<th rowspan="2" class="double">
								<asp:Literal ID="Literal5" runat="server" Text="Comment" meta:resourcekey="Literal5Resource1"></asp:Literal>
							</th>
						</tr>
						<tr>
							<th>
								<asp:Literal ID="Literal6" runat="server" Text="English" meta:resourcekey="Literal6Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal7" runat="server" Text="Vietnam" meta:resourcekey="Literal7Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal8" runat="server" Text="Broken" meta:resourcekey="Literal8Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal9" runat="server" Text="Wrong" meta:resourcekey="Literal9Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal10" runat="server" Text="Lack" meta:resourcekey="Literal10Resource1"></asp:Literal>
							</th>
						</tr>
					</thead>
					<tbody>
						<tr id="itemPlaceholder" runat="server">
						</tr>
					</tbody>
				</table>
			</div>
		</LayoutTemplate>
		<ItemTemplate>
			<tr class="group">
				<td colspan="7" align="left">
					<asp:Localize ID="l1" runat="server" Text="Issue Number:" meta:resourcekey="l1Resource1"></asp:Localize>
					<%#Eval("IssueNumber")%>
				</td>
			</tr>
			<asp:ListView ID="lv1" runat="server" DataSource='<%# Eval("Items") %>'>
				<LayoutTemplate>
					<tr runat="server" id="itemPlaceholder">
					</tr>
				</LayoutTemplate>
				<ItemTemplate>
					<tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
						<td>
							<%#Eval("PartCode")%>
						</td>
						<td>
							<%#Eval("EnglishName")%>
						</td>
						<td>
							<%#Eval("VietnamName")%>
						</td>
						<td class="number">
							<%#Eval("BrokenQuantity")%>
						</td>
						<td class="number">
							<%#Eval("WrongQuantity")%>
						</td>
						<td class="number">
							<%#Eval("LackQuantity")%>
						</td>
						<td>
							<%#Eval("DealerComment")%>
						</td>
					</tr>
				</ItemTemplate>
			</asp:ListView>
		</ItemTemplate>
	</asp:ListView>
	<div class="button">
		<asp:Button ID="b1" runat="server" Text="Save all to System" OnClick="b1_Click" meta:resourcekey="b1Resource1" />
		<asp:Button ID="b2" runat="server" Text="Back to modify" CausesValidation="False"
			OnClientClick="javascript:history.go(-1); return false;" meta:resourcekey="b2Resource1" />
	</div>
</asp:Content>
