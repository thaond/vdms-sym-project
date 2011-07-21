<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MonthlyReport.ascx.cs"
	Inherits="Controls_MonthlyReportTemplate" %>
<asp:ListView ID="lvExcel" runat="server">
	<LayoutTemplate>
		<table cellpadding="2" cellspacing="0" border="1">
			<thead>
				<tr>
					<th colspan="5" style="background-color: #444444; color: White">
						Part
					</th>
					<th colspan="3" style="background-color: #444444; color: White">
						Place
					</th>
					<th rowspan="2" style="background-color: #444444; color: White">
						Begin Quantity
					</th>
					<th colspan="2" style="background-color: #444444; color: White">
						In
					</th>
					<th colspan="2" style="background-color: #444444; color: White">
						Out
					</th>
					<th rowspan="2" style="background-color: #444444; color: White">
						Balance
					</th>
				</tr>
				<tr>
					<th style="background-color: #444444; color: White">
						No
					</th>
					<th style="background-color: #444444; color: White">
						Type
					</th>
					<th style="background-color: #444444; color: White">
						Code
					</th>
					<th style="background-color: #444444; color: White">
						English name
					</th>
					<th style="background-color: #444444; color: White">
						Vietnamese name
					</th>
					<th style="background-color: #444444; color: White">
						Type
					</th>
					<th style="background-color: #444444; color: White">
						Code
					</th>
					<th style="background-color: #444444; color: White">
						Name
					</th>
					<th style="background-color: #444444; color: White">
						Quantity
					</th>
					<th style="background-color: #444444; color: White">
						Amount
					</th>
					<th style="background-color: #444444; color: White">
						Quantity
					</th>
					<th style="background-color: #444444; color: White">
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
			<td align="center" style="background-color: #BBBBBB">
				<%#Container.DisplayIndex + 1%>
			</td>
			<td align="center" style="background-color: #BBBBBB">
				<%#Eval("PartType")%>
			</td>
			<td align="left" style="background-color: #BBBBBB">
				<%#Eval("PartCode")%>
			</td>
			<td style="background-color: #BBBBBB">
				<%#Eval("EnglishName")%>
			</td>
			<td style="background-color: #BBBBBB">
				<%#Eval("EnglishName")%>
			</td>
			<td style="background-color: #BBBBBB">
			</td>
			<td style="background-color: #BBBBBB">
			</td>
			<td style="background-color: #BBBBBB">
			</td>
			<td style="background-color: #BBBBBB">
				<%#Eval("BeginQuantity")%>
			</td>
			<td style="background-color: #BBBBBB">
				<%#Eval("InQuantity")%>
			</td>
			<td style="background-color: #BBBBBB">
				<%#Eval("InAmount")%>
			</td>
			<td style="background-color: #BBBBBB">
				<%#Eval("OutQuantity")%>
			</td>
			<td style="background-color: #BBBBBB">
				<%#Eval("OutAmount")%>
			</td>
			<td style="background-color: #BBBBBB">
				<%#Eval("Balance")%>
			</td>
		</tr>
		<asp:ListView ID="lvWarehouses" runat="server" DataSource='<%# Eval("ByWarehouses") %>'>
			<LayoutTemplate>
				<tr runat="server" id="itemPlaceholder" />
			</LayoutTemplate>
			<ItemTemplate>
				<tr>
					<td>
					</td>
					<td>
					</td>
					<td>
					</td>
					<td>
					</td>
					<td>
					</td>
					<td align="center">
						<%#Eval("PlaceType")%>
					</td>
					<td align="left">
						<%#Eval("PlaceCode")%>
					</td>
					<td>
						<%#Eval("PlaceName")%>
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
				</tr>
			</ItemTemplate>
		</asp:ListView>
		<asp:ListView ID="lvDealers" runat="server" DataSource='<%# Eval("ByDealers") %>'>
			<LayoutTemplate>
				<tr runat="server" id="itemPlaceholder" />
			</LayoutTemplate>
			<ItemTemplate>
				<tr>
					<td>
					</td>
					<td>
					</td>
					<td>
					</td>
					<td>
					</td>
					<td>
					</td>
					<td align="center">
						<%#Eval("PlaceType")%>
					</td>
					<td align="left">
						<%#Eval("PlaceCode")%>
					</td>
					<td>
						<%#Eval("PlaceName")%>
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
				</tr>
			</ItemTemplate>
		</asp:ListView>
	</ItemTemplate>
</asp:ListView>
