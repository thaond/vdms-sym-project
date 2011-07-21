<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BinCard.ascx.cs" Inherits="Controls_ExcelTemplate_BinCard" %>
<asp:ListView ID="lvExcel" runat="server">
	<LayoutTemplate>
		<table cellpadding="2" cellspacing="0" border="1">
			<thead>
				<tr>
					<th rowspan="2" style="background-color: #555555; color: White">
						Seq
					</th>
					<th colspan="2" style="background-color: #555555; color: White">
						Voucher
					</th>
					<th rowspan="2" style="background-color: #555555; color: White">
						Begin Quantity
					</th>
					<th colspan="2" style="background-color: #555555; color: White">
						In
					</th>
					<th colspan="2" style="background-color: #555555; color: White">
						Out
					</th>
					<th rowspan="2" style="background-color: #555555; color: White">
						Balance
					</th>
					<th rowspan="2" style="background-color: #555555; color: White">
						Code
					</th>
					<th rowspan="2" style="background-color: #555555; color: White">
						Comment
					</th>
				</tr>
				<tr>
					<th style="background-color: #555555; color: White">
						No
					</th>
					<th style="background-color: #555555; color: White">
						Date
					</th>
					<th style="background-color: #555555; color: White">
						Quantity
					</th>
					<th style="background-color: #555555; color: White">
						Amount
					</th>
					<th style="background-color: #555555; color: White">
						Quantity
					</th>
					<th style="background-color: #555555; color: White">
						Amount
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
		<tr>
			<td colspan="3" align="left" style="background-color: #AAAAAA">
				<%#Eval("PartCode")%>
			</td>
			<td style="background-color: #AAAAAA">
				<%#Eval("Begin.Quantity")%>
			</td>
			<td colspan="7" style="background-color: #AAAAAA">
			</td>
		</tr>
		<asp:ListView ID="lvItems" runat="server" DataSource='<%# EvalActions(Eval("Items"), Eval("Begin")) %>'>
			<LayoutTemplate>
				<tr runat="server" id="itemPlaceholder" />
			</LayoutTemplate>
			<ItemTemplate>
				<tr>
					<td align="center">
						<%#Container.DisplayIndex + 1%>
					</td>
					<td>
						<%#Eval("VoucherNo")%>
					</td>
					<td>
						<%#Eval("ActDateString")%>
					</td>
					<td>
						<%#Eval("BeginQuantity")%>
					</td>
					<td>
						<%#Eval("InQuantity")%>
					</td>
					<td>
						<%#Eval("InAmount")%>
					</td>
					<td>
						<%#Eval("OutQuantity")%>
					</td>
					<td>
						<%#Eval("OutAmount")%>
					</td>
					<td>
						<%#Eval("Balance")%>
					</td>
					<td align="center">
						<%#Eval("TransactionCode")%>
					</td>
					<td>
						<%#Eval("TransactionComment")%>
					</td>
				</tr>
			</ItemTemplate>
		</asp:ListView>
	</ItemTemplate>
</asp:ListView>
