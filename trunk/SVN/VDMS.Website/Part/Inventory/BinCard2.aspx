<%@ Page Title="Bin Card Query" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="BinCard2.aspx.cs" Inherits="Part_Inventory_BinCard2"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="form" style="width: 50%; float: left;">
		<asp:ValidationSummary ValidationGroup="Query" CssClass="error" ID="ValidationSummary1"
			runat="server" meta:resourcekey="ValidationSummary1Resource1" />
		<table width="100%">
			<tr>
				<td style="width: 25%;">
					<asp:Localize ID="litType" runat="server" Text="Type:" meta:resourcekey="litTypeResource1"></asp:Localize>
				</td>
				<td>
					<asp:RadioButtonList ID="rblType" runat="server" OnSelectedIndexChanged="rblType_SelectedIndexChanged"
						RepeatDirection="Horizontal" meta:resourcekey="rblTypeResource1">
						<asp:ListItem Selected="True" Text="<%$ Resources:TextMsg, Part %>" Value="P" meta:resourcekey="ListItemResource1"></asp:ListItem>
						<asp:ListItem Text="<%$ Resources:TextMsg, Accessory %>" Value="A" meta:resourcekey="ListItemResource2"></asp:ListItem>
					</asp:RadioButtonList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Literal ID="Literal1" runat="server" Text="Dealer:" meta:resourcekey="Literal1Resource1"></asp:Literal>
				</td>
				<td>
					<vdms:DealerList runat="server" ID="ddlDealer" OnDataBound="ddlDealer_DataBound"
						AutoPostBack="True" OnSelectedIndexChanged="ddlDealer_SelectedIndexChanged" EnabledSaperateByArea="False"
						EnabledSaperateByDB="False" MergeCode="False" meta:resourcekey="ddlDealerResource1"
						RemoveRootItem="False" ShowEmptyItem="False" ShowSelectAllItem="False">
					</vdms:DealerList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Literal ID="Literal2" runat="server" Text="Warehouse:" meta:resourcekey="Literal2Resource1"></asp:Literal>
				</td>
				<td>
					<vdms:WarehouseList ShowSelectAllItem="True" runat="server" ID="ddlWarehouse" DontAutoUseCurrentSealer="False"
						meta:resourcekey="ddlWarehouseResource1" ShowEmptyItem="False" UseVIdAsValue="False">
					</vdms:WarehouseList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litOrderDate" runat="server" Text="Date:" meta:resourcekey="litOrderDateResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="txtFromDate" runat="server" Width="88px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
					<asp:ImageButton ID="i1" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
						meta:resourcekey="i1Resource1" />
					<asp:RequiredFieldValidator ID="r1" runat="server" Text="*" SetFocusOnError="True"
						ValidationGroup="Query" ControlToValidate="txtFromDate" meta:resourcekey="r1Resource1"></asp:RequiredFieldValidator>
					<ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
						Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
						CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
						CultureTimePlaceholder="" Enabled="True">
					</ajaxToolkit:MaskedEditExtender>
					<ajaxToolkit:CalendarExtender ID="ce1" runat="server" TargetControlID="txtFromDate"
						PopupButtonID="i1" Enabled="True">
					</ajaxToolkit:CalendarExtender>
					~
					<asp:TextBox ID="txtToDate" runat="server" Width="88px" meta:resourcekey="txtToDateResource1"></asp:TextBox>
					<asp:ImageButton ID="i2" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
						meta:resourcekey="ibToDateResource1" />
					<%--<asp:RequiredFieldValidator ID="r2" runat="server" Text="*" SetFocusOnError="True"
                        ValidationGroup="Query" ControlToValidate="txtToDate" meta:resourcekey="rfvToDateResource1"></asp:RequiredFieldValidator>--%>
					<ajaxToolkit:MaskedEditExtender ID="meeToDate" runat="server" TargetControlID="txtToDate"
						Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
						CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
						CultureTimePlaceholder="" Enabled="True">
					</ajaxToolkit:MaskedEditExtender>
					<ajaxToolkit:CalendarExtender ID="ce2" runat="server" TargetControlID="txtToDate"
						PopupButtonID="i2" Enabled="True">
					</ajaxToolkit:CalendarExtender>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litPartNo" runat="server" Text="Part No:" meta:resourcekey="litPartNoResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="txtPartCode" runat="server" meta:resourcekey="txtPartCodeResource1"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
				</td>
				<td>
					<asp:Button ID="btnQuery" ValidationGroup="Query" runat="server" Text="Query" OnClick="btnQuery_Click"
						meta:resourcekey="btnQueryResource1" />
					<asp:Button ID="cmd2Excel" ValidationGroup="Query" runat="server" Text="Export to Excel"
						OnClick="cmd2Excel_Click" meta:resourcekey="cmd2ExcelResource1" />
				</td>
			</tr>
		</table>
	</div>
	<div class="help" style="float: right; width: 35%;">
		<asp:Literal ID="Literal3" runat="server" Text="Transaction code:" meta:resourcekey="Literal3Resource1"></asp:Literal>
		<ul>
			<li>
				<asp:Literal ID="Literal5" runat="server" Text="SE: Special Export" meta:resourcekey="Literal5Resource1"></asp:Literal></li>
			<li>
				<asp:Literal ID="Literal6" runat="server" Text="NI: Normal Import" meta:resourcekey="Literal6Resource1"></asp:Literal></li>
			<li>
				<asp:Literal ID="Literal7" runat="server" Text="SI: Special Import" meta:resourcekey="Literal7Resource1"></asp:Literal></li>
			<li>
				<asp:Literal ID="Literal8" runat="server" Text="CC: Cycle count" meta:resourcekey="Literal8Resource1"></asp:Literal></li>
			<li>
				<asp:Literal ID="Literal9" runat="server" Text="ST: Stock transfer" meta:resourcekey="Literal9Resource1"></asp:Literal></li>
			<li>
				<asp:Literal ID="Literal10" runat="server" Text="AI: Auto Import" meta:resourcekey="Literal10Resource1"></asp:Literal></li>
			<li>
				<asp:Literal ID="Literal11" runat="server" Text="UI: Undo Auto Import" meta:resourcekey="Literal11Resource1"></asp:Literal></li>
			<li>
				<asp:Literal ID="Literal12" runat="server" Text="SL: Sale Transaction" meta:resourcekey="Literal12Resource1"></asp:Literal></li>
		</ul>
	</div>
	<br />
	<div style="clear: both">
	</div>
	<div id="grid" class="grid">
		<asp:ListView ID="lv" runat="server">
			<LayoutTemplate>
				<div class="title">
					<asp:Literal ID="Literal4" runat="server" Text="Bin Card Query Result" meta:resourcekey="Literal4Resource1"></asp:Literal>
				</div>
				<table class="datatable" cellpadding="0" cellspacing="0">
					<thead>
						<tr>
							<th rowspan="2" class="double">
								<asp:Literal ID="Literal13" runat="server" Text="Seq" meta:resourcekey="Literal13Resource1"></asp:Literal>
							</th>
							<th colspan="2">
								<asp:Literal ID="Literal14" runat="server" Text="Voucher" meta:resourcekey="Literal14Resource1"></asp:Literal>
							</th>
							<th rowspan="2" class="double">
								<asp:Literal ID="Literal15" runat="server" Text="Part code" meta:resourcekey="Literal15Resource1"></asp:Literal>
							</th>
							<th rowspan="2" class="double">
								<asp:Literal ID="Literal16" runat="server" Text="Begin Quantity" meta:resourcekey="Literal16Resource1"></asp:Literal>
							</th>
							<th colspan="2">
								<asp:Literal ID="Literal17" runat="server" Text="In" meta:resourcekey="Literal17Resource1"></asp:Literal>
							</th>
							<th colspan="2">
								<asp:Literal ID="Literal18" runat="server" Text="Out" meta:resourcekey="Literal18Resource1"></asp:Literal>
							</th>
							<th rowspan="2" class="double">
								<asp:Literal ID="Literal19" runat="server" Text="Balance" meta:resourcekey="Literal19Resource1"></asp:Literal>
							</th>
							<th rowspan="2" class="double">
								<asp:Literal ID="Literal20" runat="server" Text="Code" meta:resourcekey="Literal20Resource1"></asp:Literal>
							</th>
							<th rowspan="2" class="double">
								<asp:Literal ID="Literal21" runat="server" Text="Comment" meta:resourcekey="Literal21Resource1"></asp:Literal>
							</th>
						</tr>
						<tr>
							<th>
								<asp:Literal ID="Literal22" runat="server" Text="No" meta:resourcekey="Literal22Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal23" runat="server" Text="Date" meta:resourcekey="Literal23Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal24" runat="server" Text="Quantity" meta:resourcekey="Literal24Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal25" runat="server" Text="Amount" meta:resourcekey="Literal25Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal26" runat="server" Text="Quantity" meta:resourcekey="Literal26Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal27" runat="server" Text="Amount" meta:resourcekey="Literal27Resource1"></asp:Literal>
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
				<tr class="group">
					<td colspan="12" align="left">
						<%#Eval("Code")%>
						-
						<%#Eval("Address")%><%--<asp:HiddenField runat="server" ID="hdWhid" Value='<%#Eval("WarehouseId")%>' />--%>
					</td>
				</tr>
				<asp:ListView ID="lvItems" runat="server" DataSource='<%# EvalAct(Eval("WarehouseId")) %>'>
					<LayoutTemplate>
						<tr runat="server" id="itemPlaceholder" />
					</LayoutTemplate>
					<ItemTemplate>
						<tr class='<%# ((int)Eval("No") == 1)? "selected" : (Container.DisplayIndex % 2 == 0 ? "even" : "odd") %>'>
							<td class="center">
								<%#Eval("No")%>
							</td>
							<td>
								<%#Eval("VoucherNo")%>
							</td>
							<td>
								<%#Eval("ActDateString")%>
							</td>
							<td class="number">
								<%#Eval("PartCode")%>
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
			</ItemTemplate>
		</asp:ListView>
	</div>
	<asp:ObjectDataSource TypeName="VDMS.II.PartManagement.BinCardDAO" EnablePaging="True"
		ID="odsParts" runat="server" SelectMethod="FindBindCardWH" SelectCountMethod="CountBindCardWH"
		OnSelecting="odsParts_Selecting">
		<SelectParameters>
			<asp:ControlParameter ControlID="ddlDealer" Name="dealerCode" PropertyName="SelectedValue"
				Type="String" />
			<asp:ControlParameter ControlID="ddlWarehouse" Name="wid" PropertyName="SelectedValue"
				Type="Int64" />
		</SelectParameters>
	</asp:ObjectDataSource>
</asp:Content>
