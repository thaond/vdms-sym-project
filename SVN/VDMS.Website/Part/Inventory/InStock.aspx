<%@ Page Title="In stock Order" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="InStock.aspx.cs" Inherits="Part_Inventory_InStock"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Src="OrderInfo.ascx" TagName="OrderInfo" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="form" style="width: 40%; float: left;">
		<asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="error" ValidationGroup="Save"
			meta:resourcekey="ValidationSummary1Resource1" />
		<uc1:OrderInfo ID="OrderInfo1" runat="server" RedirectURLWhenOrderNull="Receive.aspx" />
	</div>
	<div class="help" style="float: right; width: 40%;">
		<ul>
			<%--<li>
				<asp:Localize ID="lh1" runat="server" Text="Do instock receive parts." meta:resourcekey="lh1Resource1"></asp:Localize>
			</li>--%>
			<li>
				<asp:Localize ID="lh2" runat="server" Text="Record the Abnormal receive case: Broken, Wrong, Lack parts. in this case, comments is required."
					meta:resourcekey="lh2Resource1"></asp:Localize></li>
			<li>
				<asp:Localize ID="lh3" runat="server" Text="Using <NG> Form to send Abnormal case to VMEP."
					meta:resourcekey="lh3Resource1"></asp:Localize>
			</li>
			<li>
				<asp:Localize ID="Localize1" runat="server" Text="In stock quantity as Good parts quantity."
					meta:resourcekey="Localize1Resource1"></asp:Localize></li>
			<li>
				<asp:Localize ID="Localize2" runat="server" Text="Data valid = Good parts + Broken parts + Wrong parts + Lack parts."
					meta:resourcekey="Localize2Resource1"></asp:Localize></li>
		</ul>
	</div>
	<div style="clear: both;">
	</div>
	<asp:Label ID="lblSaveOk" runat="server" SkinID="MessageOk" Visible="False" Text="The receive has been import to stock successful."
		meta:resourcekey="lblSaveOkResource1"></asp:Label>
	<asp:Label ID="lblNGNotConfirm" runat="server" SkinID="MessageError" Visible="False"
		Text="The NG From has not been confirmed by VMEP" meta:resourcekey="lblNGNotConfirmResource1"></asp:Label>
	<asp:Label ID="lblNGNotCreate" runat="server" SkinID="MessageError" Visible="False"
		Text="The NG From has not been created." meta:resourcekey="lblNGNotCreateResource1"></asp:Label>
	<asp:Label ID="lblInventoryClose" runat="server" SkinID="MessageError" Visible="False"
		Text="Cannot sales. The inventory is closed." meta:resourcekey="lblInventoryCloseResource2"></asp:Label>
	<asp:ListView ID="lv" runat="server" DataSourceID="ods1" DataKeyNames="ReceiveHeaderId"
		OnDataBound="lv_DataBound">
		<LayoutTemplate>
			<div id="grid" class="grid">
				<div class="title">
					<asp:Literal ID="Literal1" runat="server" Text="Order List and Receive" meta:resourcekey="Literal1Resource1"></asp:Literal>
				</div>
				<table class="datatable" cellpadding="0" cellspacing="0">
					<thead>
						<tr class="header">
							<th>
								<asp:Literal ID="Literal2" runat="server" Text="Part No" meta:resourcekey="Literal2Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal3" runat="server" Text="Part Name" meta:resourcekey="Literal3Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal4" runat="server" Text="Order/Quotation" meta:resourcekey="Literal4Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal5" runat="server" Text="Shipping" meta:resourcekey="Literal5Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal6" runat="server" Text="Unit Price" meta:resourcekey="Literal6Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal7" runat="server" Text="Amount" meta:resourcekey="Literal7Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal8" runat="server" Text="Good" meta:resourcekey="Literal8Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal9" runat="server" Text="Broken" meta:resourcekey="Literal9Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal10" runat="server" Text="Wrong" meta:resourcekey="Literal10Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal11" runat="server" Text="Lack" meta:resourcekey="Literal11Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal12" runat="server" Text="Comment" meta:resourcekey="Literal12Resource1"></asp:Literal>
							</th>
						</tr>
					</thead>
					<tbody>
						<tr id="itemPlaceholder" runat="server">
						</tr>
						<tr class="gorup end">
							<td colspan="5">
								<asp:Literal ID="Literal13" runat="server" Text="Sub Total:" meta:resourcekey="Literal13Resource1"></asp:Literal>
							</td>
							<td>
								<asp:Literal ID="litTotal" runat="server" meta:resourcekey="litTotalResource1"></asp:Literal>
							</td>
							<td colspan="5">
							</td>
						</tr>
					</tbody>
				</table>
			</div>
		</LayoutTemplate>
		<ItemTemplate>
			<tr class="group">
				<td colspan="11" align="left">
					<asp:Label ID="litIssue" runat="server" Text='<%# Eval("IssueNumber") %>' ForeColor='<%# (long)Eval("ReceiveHeaderId") == 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black %>'
						meta:resourcekey="litIssueResource1"></asp:Label>,
					<%#VDMS.Helper.DateTimeHelper.To24h((DateTime)Eval("ShippingDate"))%>
					<asp:Label ID="litNGNumber" runat="server" Text='<%# Eval("NotGoodNumber") %>' ForeColor="Red"
						meta:resourcekey="litNGNumberResource1"></asp:Label>
				</td>
			</tr>
			<asp:ListView ID="lvItems" runat="server" DataSource='<%# Eval("Items") %>' DataKeyNames="ReceiveDetailId"
				Enabled='<%# CanEdit((long)Eval("ReceiveHeaderId")) %>'>
				<LayoutTemplate>
					<tr runat="server" id="itemPlaceholder" />
				</LayoutTemplate>
				<ItemTemplate>
					<tr id="tr" runat="server">
						<td runat="server">
							<%#Eval("PartCode")%>
						</td>
						<td runat="server">
							<%#Eval("EnglishName")%>
						</td>
						<td runat="server">
							<asp:Literal ID="litVN" runat="server" Visible="False" Text='<%# Eval("VietnamName") %>'></asp:Literal>
							<b>
								<asp:Literal ID="litOQ" runat="server" Text='<%# Eval("OrderQuantity") %>'></asp:Literal>
								/
								<asp:Literal ID="litQQ" runat="server" Text='<%# Eval("QuotationQuantity") %>'></asp:Literal>
							</b>
						</td>
						<td runat="server">
							<asp:Literal ID="litSQ" runat="server" Text='<%# Eval("ShippingQuantity") %>'></asp:Literal>
						</td>
						<td class="number" runat="server">
							<%#Eval("UnitPrice")%>
						</td>
						<td class="number" runat="server">
							<%#Eval("Amount")%>
						</td>
						<td runat="server">
							<asp:TextBox ID="t1" runat="server" Text='<%# Eval("GoodQuantity") %>' Columns="4"
								CssClass="number" OnTextChanged="t_TextChanged"></asp:TextBox>
						</td>
						<td runat="server">
							<asp:TextBox ID="t2" runat="server" Text='<%# Eval("BrokenQuantity") %>' Columns="4"
								CssClass="number" OnTextChanged="t_TextChanged"></asp:TextBox>
						</td>
						<td runat="server">
							<asp:TextBox ID="t3" runat="server" Text='<%# Eval("WrongQuantity") %>' Columns="4"
								CssClass="number" OnTextChanged="t_TextChanged"></asp:TextBox>
						</td>
						<td runat="server">
							<asp:TextBox ID="t4" runat="server" Text='<%# Eval("LackQuantity") %>' Columns="4"
								CssClass="number" OnTextChanged="t_TextChanged"></asp:TextBox>
						</td>
						<td style="width: 100px;" runat="server">
							<asp:TextBox ID="t5" runat="server" Text='<%# Eval("DealerComment") %>' Columns="10"
								OnTextChanged="t_TextChanged"></asp:TextBox>
						</td>
					</tr>
				</ItemTemplate>
			</asp:ListView>
		</ItemTemplate>
	</asp:ListView>
	<div class="button">
		<asp:Button ID="bSave" runat="server" ValidationGroup="Save" Text="Save" OnClick="bSave_Click"
			meta:resourcekey="bSaveResource1" />
		<asp:Button ID="bBack" runat="server" Text="Back" CommandName="Cancel" OnClientClick="javascript:location.href='Receive.aspx'; return false;"
			meta:resourcekey="bBackResource1" />
	</div>
	<asp:ObjectDataSource ID="ods1" runat="server" TypeName="VDMS.II.PartManagement.Order.OrderDAO"
		SelectMethod="GetShipping">
		<SelectParameters>
			<asp:QueryStringParameter Name="OrderHeaderId" QueryStringField="id" Type="Int64" />
		</SelectParameters>
	</asp:ObjectDataSource>
	<asp:Literal ID="litPopupJS" runat="server" meta:resourcekey="litPopupJSResource1"></asp:Literal>
</asp:Content>
