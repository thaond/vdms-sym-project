<%@ Page Title="Bin Card Query" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="BinCard.aspx.cs" Inherits="Part_Inventory_BinCard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="form" style="width: 50%; float: left;">
		<asp:ValidationSummary ValidationGroup="Query" CssClass="error" ID="ValidationSummary1"
			runat="server" />
		<table width="100%">
			<tr>
				<td style="width: 25%;">
					<asp:Localize ID="litType" runat="server" Text="Type:"></asp:Localize>
				</td>
				<td>
					<asp:RadioButtonList ID="rblType" runat="server" AutoPostBack="false" OnSelectedIndexChanged="rblType_SelectedIndexChanged"
						RepeatDirection="Horizontal">
						<asp:ListItem Selected="True" Text="<%$ Resources:TextMsg, Part %>" Value="P"></asp:ListItem>
						<asp:ListItem Text="<%$ Resources:TextMsg, Accessory %>" Value="A"></asp:ListItem>
					</asp:RadioButtonList>
				</td>
			</tr>
			<tr>
				<td>
					Dealer:
				</td>
				<td>
					<vdms:DealerList runat="server" ID="ddlDealer" OnDataBound="ddlDealer_DataBound"
						AutoPostBack="true" OnSelectedIndexChanged="ddlDealer_SelectedIndexChanged">
					</vdms:DealerList>
				</td>
			</tr>
			<tr>
				<td>
					Warehouse:
				</td>
				<td>
					<vdms:WarehouseList ShowSelectAllItem="true" runat="server" ID="ddlWarehouse">
					</vdms:WarehouseList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litOrderDate" runat="server" Text="Date:"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="txtFromDate" runat="server" Width="88px"></asp:TextBox>
					<asp:ImageButton ID="i1" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;" />
					<asp:RequiredFieldValidator ID="r1" runat="server" Text="*" SetFocusOnError="True"
						ValidationGroup="Query" ControlToValidate="txtFromDate"></asp:RequiredFieldValidator>
					<ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
						Mask="99/99/9999" MaskType="Date" BehaviorID="meeFromDate">
					</ajaxToolkit:MaskedEditExtender>
					<ajaxToolkit:CalendarExtender ID="ce1" runat="server" TargetControlID="txtFromDate"
						PopupButtonID="i1" BehaviorID="ce1" Enabled="True">
					</ajaxToolkit:CalendarExtender>
					~
					<asp:TextBox ID="txtToDate" runat="server" Width="88px" meta:resourcekey="txtToDateResource1"></asp:TextBox>
					<asp:ImageButton ID="i2" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
						meta:resourcekey="ibToDateResource1" />
					<%--<asp:RequiredFieldValidator ID="r2" runat="server" Text="*" SetFocusOnError="True"
                        ValidationGroup="Query" ControlToValidate="txtToDate" meta:resourcekey="rfvToDateResource1"></asp:RequiredFieldValidator>--%>
					<ajaxToolkit:MaskedEditExtender ID="meeToDate" runat="server" TargetControlID="txtToDate"
						Mask="99/99/9999" MaskType="Date" BehaviorID="meeToDate">
					</ajaxToolkit:MaskedEditExtender>
					<ajaxToolkit:CalendarExtender ID="ce2" runat="server" TargetControlID="txtToDate"
						PopupButtonID="i2" BehaviorID="ce2" Enabled="True">
					</ajaxToolkit:CalendarExtender>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litPartNo" runat="server" Text="Part No:"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="txtPartCode" runat="server"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
				</td>
				<td>
					<asp:Button ID="btnQuery" ValidationGroup="Query" runat="server" Text="Query" OnClick="btnQuery_Click" />
					<asp:Button ID="cmd2Excel" ValidationGroup="Query" runat="server" Text="Export to Excel"
						OnClick="cmd2Excel_Click" />
				</td>
			</tr>
		</table>
	</div>
	<div class="help" style="float: right; width: 35%;">
		<ul>
			<li>Transaction code:</li>
			<li>SE: Special Export</li>
			<li>SI: Special Import</li>
			<li>NI: Normal Import</li>
			<li>CC: Cycle count</li>
			<li>ST: Stock transfer</li>
			<li>AI: Auto Import</li>
			<li>UI: Undo Auto Import</li>
			<li>SL: Sale Transaction</li>
		</ul>
	</div>
	<br />
	<div style="clear: both">
	</div>
	<div id="grid" class="grid">
		<asp:ListView ID="lv" runat="server">
			<LayoutTemplate>
				<div class="title">
					Bin Card Query Result</div>
				<table class="datatable" cellpadding="0" cellspacing="0">
					<thead>
						<tr>
							<th rowspan="2" class="double">
								Seq
							</th>
							<th colspan="2">
								Voucher
							</th>
							<th rowspan="2" class="double">
								Warehouse
							</th>
							<th rowspan="2" class="double">
								Begin Quantity
							</th>
							<th colspan="2">
								In
							</th>
							<th colspan="2">
								Out
							</th>
							<th rowspan="2" class="double">
								Balance
							</th>
							<th rowspan="2" class="double">
								Code
							</th>
							<th rowspan="2" class="double">
								Comment
							</th>
						</tr>
						<tr>
							<th>
								No
							</th>
							<th>
								Date
							</th>
							<th>
								Quantity
							</th>
							<th>
								Amount
							</th>
							<th>
								Quantity
							</th>
							<th>
								Amount
							</th>
						</tr>
					</thead>
					<tbody>
						<tr id="itemPlaceholder" runat="server">
						</tr>
					</tbody>
					<tfoot>
						<tr>
							<td class="pager" colspan="12">
								<vdms:DataPager runat="server" ID="partPager">
								</vdms:DataPager>
							</td>
						</tr>
					</tfoot>
				</table>
			</LayoutTemplate>
			<ItemTemplate>
				<tr class="group">
					<td colspan="4" align="left">
						<%#Eval("PartCode")%>
					</td>
					<td class="number">
						<%#Eval("Begin.Quantity")%>
					</td>
					<td colspan="7" class="number">
					</td>
				</tr>
				<asp:ListView ID="lvItems" runat="server" DataSource='<%# EvalActions(Eval("Items"), Eval("Begin")) %>'>
					<LayoutTemplate>
						<tr runat="server" id="itemPlaceholder" />
					</LayoutTemplate>
					<ItemTemplate>
						<tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
							<td class="center">
								<%#Container.DisplayIndex + 1%>
							</td>
							<td>
								<%#Eval("VoucherNo")%>
							</td>
							<td>
								<%#Eval("ActDateString")%>
							</td>
							<td class="number">
								<%#Eval("ToWH")%>
							</td>
							<td class="number">
								<%#Eval("BeginQuantity")%>
							</td>
							<td class="number">
								<%#Eval("InQuantity")%>
							</td>
							<td class="number">
								<%#Eval("InAmount")%>
							</td>
							<td class="number">
								<%#Eval("OutQuantity")%>
							</td>
							<td class="number">
								<%#Eval("OutAmount")%>
							</td>
							<td class="number">
								<%#Eval("Balance")%>
							</td>
							<td class="center">
								<%#Eval("TransactionCode")%>
							</td>
							<td>
								<%#Eval("TransactionComment")%>
							</td>
						</tr>
					</ItemTemplate>
				</asp:ListView>
				<%--<tr class="group end">
				<td colspan="5" align="right">
					Total:
				</td>
				<td>
					<%# Eval("Total")%>
				</td>
				<td colspan="2">
				</td>
			</tr>--%>
			</ItemTemplate>
		</asp:ListView>
	</div>
	<asp:ObjectDataSource TypeName="VDMS.II.PartManagement.BinCardDAO" EnablePaging="true"
		ID="odsParts" runat="server" SelectMethod="FindBindCardPart" SelectCountMethod="CountBindCardPart">
		<SelectParameters>
			<asp:ControlParameter ControlID="rblType" Name="partType" PropertyName="SelectedValue"
				Type="String" />
			<asp:ControlParameter ControlID="txtPartCode" Name="partCode" PropertyName="Text"
				Type="String" />
			<asp:ControlParameter ControlID="txtFromDate" Name="fromDate" PropertyName="Text"
				Type="String" />
			<asp:ControlParameter ControlID="txtToDate" Name="toDate" PropertyName="Text" Type="String" />
			<asp:ControlParameter ControlID="ddlDealer" Name="dealerCode" PropertyName="SelectedValue"
				Type="String" />
			<asp:ControlParameter ControlID="ddlWarehouse" Name="warehouseId" PropertyName="SelectedValue"
				Type="Int64" />
		</SelectParameters>
	</asp:ObjectDataSource>
</asp:Content>
