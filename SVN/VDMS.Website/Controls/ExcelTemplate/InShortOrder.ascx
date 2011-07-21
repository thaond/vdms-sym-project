<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InShortOrder.ascx.cs"
	Inherits="Controls_ExcelTemplate_InShortOrder" %>
<asp:ListView ID="lv" runat="server">
	<LayoutTemplate>
		<table border="1" cellpadding="0" cellspacing="0">
			<thead>
				<tr>
					<th style="background-color: #555555; color: White">
						Seq
					</th>
					<th style="background-color: #555555; color: White">
						Part No
					</th>
					<th style="background-color: #555555; color: White">
						English Name
					</th>
					<th style="background-color: #555555; color: White">
						Vietnamese Name
					</th>
					<th style="background-color: #555555; color: White">
						Order Quantity
					</th>
					<th style="background-color: #555555; color: White">
						Quotation Quantity
					</th>
					<th style="background-color: #555555; color: White">
						Short Quantity
					</th>
				</tr>
			</thead>
			<tbody>
				<tr id="itemPlaceholder" runat="server">
				</tr>
			</tbody>
		</table>
	</LayoutTemplate>
	<ItemTemplate>
		<tr style="background-color: #BBBBBB; font-weight: bold">
			<td colspan="7" align="left">
				Order Number:
				<%#Eval("TipTopNumber")%>
				| Order Date:
				<%#Eval("OrderDate")%>
			</td>
		</tr>
		<asp:ListView ID="lvItems" runat="server" DataSource='<%# EvalShort(Eval("OrderDetails")) %>'>
			<LayoutTemplate>
				<tr runat="server" id="itemPlaceholder" />
			</LayoutTemplate>
			<ItemTemplate>
				<tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
					<td align="center">
						<%#Container.DisplayIndex + 1%>
					</td>
					<td>
						<%#Eval("PartCode")%>
					</td>
					<td>
						<%#Eval("EnglishName")%>
					</td>
					<td>
						<%#Eval("VietnamName")%>
					</td>
					<td>
						<%#Eval("OrderQuantity")%>
					</td>
					<td>
						<%#Eval("QuotationQuantity")%>
					</td>
					<td>
						<%#Eval("ShortQuantity")%>
					</td>
				</tr>
			</ItemTemplate>
		</asp:ListView>
	</ItemTemplate>
</asp:ListView>
