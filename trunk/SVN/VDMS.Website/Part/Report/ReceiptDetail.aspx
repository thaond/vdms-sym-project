<%@ Page Title="Abnormal receive report" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="ReceiptDetail.aspx.cs" Inherits="Part_Report_ReceiptDetail"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="form" style="width: 450px">
		<asp:ValidationSummary ID="ValidationSummary1" CssClass="error" ValidationGroup="Report"
			runat="server" meta:resourcekey="ValidationSummary1Resource1" />
		<table>
			<tr>
				<td>
					<asp:Literal ID="Literal2" runat="server" Text="Order date:" meta:resourcekey="Literal2Resource1"></asp:Literal>
				</td>
				<td>
					<asp:TextBox ID="txtFromDate" runat="server" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
					<asp:ImageButton ID="i1" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
						meta:resourcekey="i1Resource1" />
					<asp:RequiredFieldValidator ID="r1" runat="server" ErrorMessage="Report date cannot be blank!"
						SetFocusOnError="True" ValidationGroup="Report" ControlToValidate="txtFromDate"
						meta:resourcekey="r1Resource1">*</asp:RequiredFieldValidator>
					<ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
						Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
						CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
						CultureTimePlaceholder="" Enabled="True">
					</ajaxToolkit:MaskedEditExtender>
					<ajaxToolkit:CalendarExtender ID="ce1" runat="server" TargetControlID="txtFromDate"
						PopupButtonID="i1" Enabled="True">
					</ajaxToolkit:CalendarExtender>
					~
					<asp:TextBox ID="txtToDate" runat="server" meta:resourcekey="txtToDateResource1"></asp:TextBox>
					<asp:ImageButton ID="i2" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
						meta:resourcekey="i2Resource1" />
					<%--<asp:RequiredFieldValidator ID="r2" runat="server" Text="'Sale date to' cannot be empty!" SetFocusOnError="True"
                        ValidationGroup="Report" ControlToValidate="txtToDate" ></asp:RequiredFieldValidator>--%>
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
			<%--<tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="To Order date:"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                </td>
            </tr>--%>
			<tr>
				<td>
					<asp:Literal ID="Literal4" runat="server" Text="Order No:" meta:resourcekey="Literal4Resource1"></asp:Literal>
				</td>
				<td>
					<asp:TextBox ID="txtFromOrder" runat="server" meta:resourcekey="txtFromOrderResource1"></asp:TextBox>
				</td>
			</tr>
			<asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="False">
				<tr>
					<td>
						<asp:Literal ID="Literal5" runat="server" Text="To Order No:" meta:resourcekey="Literal5Resource1"></asp:Literal>
					</td>
					<td>
						<asp:TextBox ID="txtToOrder" runat="server" meta:resourcekey="txtToOrderResource1"></asp:TextBox>
					</td>
				</tr>
			</asp:PlaceHolder>
			<tr>
				<td>
					<asp:Literal ID="Literal6" runat="server" Text="Area:" meta:resourcekey="Literal6Resource1"></asp:Literal>
				</td>
				<td>
					<vdms:DatabaseList AutoPostBack="True" AllowDealerSelect="False" ShowSelectAllItem="True"
						ID="ddlArea" runat="server" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged"
						meta:resourcekey="ddlAreaResource1">
					</vdms:DatabaseList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Literal ID="Literal1" runat="server" Text="Dealer:" meta:resourcekey="Literal1Resource1"></asp:Literal>
				</td>
				<td>
					<vdms:DealerList EnabledSaperateByDB="False" ShowSelectAllItem="True" runat="server"
						ID="ddlDealers" EnabledSaperateByArea="False" MergeCode="False" meta:resourcekey="ddlDealersResource1"
						RemoveRootItem="False" ShowEmptyItem="False">
					</vdms:DealerList>
				</td>
			</tr>
			<tr>
				<td>
					&nbsp;
				</td>
				<td>
					<asp:Button ID="btnShowReport" ValidationGroup="Report" runat="server" Text="View"
						OnClick="btnShowReport_Click" meta:resourcekey="btnShowReportResource1" />
					<asp:Button ID="btnExcel" runat="server" Text="Export to Excel" OnClick="btnExcel_Click"
						meta:resourcekey="btnExcelResource1" />
				</td>
			</tr>
		</table>
	</div>
	<asp:ListView ID="lv" runat="server">
		<LayoutTemplate>
			<div id="grid" class="grid">
				<table class="datatable" cellpadding="0" cellspacing="0">
					<thead>
						<tr>
							<th>
								<asp:Literal ID="Literal2" runat="server" Text="NG number" meta:resourcekey="Literal2Resource2"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal3" runat="server" Text="Part code" meta:resourcekey="Literal3Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal7" runat="server" Text="Enlish name" meta:resourcekey="Literal7Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal8" runat="server" Text="Vietnamese name" meta:resourcekey="Literal8Resource1"></asp:Literal>
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
						</tr>
					</thead>
					<tbody>
						<tr id="itemPlaceholder" runat="server">
						</tr>
					</tbody>
					<tfoot>
						<tr class="sumLine">
							<td colspan="4" class="right">
								<asp:Literal ID="Literal6" runat="server" Text="Total" meta:resourcekey="Literal6Resource2"></asp:Literal>
							</td>
							<td class="number">
								<asp:Literal ID="litTotalBroken" runat="server" meta:resourcekey="litTotalBrokenResource1"></asp:Literal>
							</td>
							<td class="number">
								<asp:Literal ID="litTotalWrong" runat="server" meta:resourcekey="litTotalWrongResource1"></asp:Literal>
							</td>
							<td class="number">
								<asp:Literal ID="litTotalLack" runat="server" meta:resourcekey="litTotalLackResource1"></asp:Literal>
							</td>
						</tr>
						<tr>
							<td colspan="7" class="pager">
								<vdms:DataPager ID="DataPager1" runat="server" PagedControlID="lv" />
							</td>
						</tr>
					</tfoot>
				</table>
			</div>
		</LayoutTemplate>
		<ItemTemplate>
			<tr class="group">
				<td align="left">
					<asp:Literal ID="Literal4" runat="server" Text="Order No:" meta:resourcekey="Literal4Resource1"></asp:Literal>
					<%#Eval("TipTopNumber")%>
				</td>
				<td>
					&nbsp;
				</td>
				<td>
					&nbsp;
				</td>
				<td>
					&nbsp;
				</td>
				<td class="number">
					<%#Eval("Broken")%>
				</td>
				<td class="number">
					<%#Eval("Wrong")%>
				</td>
				<td class="number">
					<%#Eval("Lack")%>
				</td>
			</tr>
			<asp:ListView ID="lv1" runat="server" DataSource='<%# Eval("ReceiveDetails") %>'>
				<LayoutTemplate>
					<tr runat="server" id="itemPlaceholder" />
				</LayoutTemplate>
				<ItemTemplate>
					<tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
						<td>
							<%#Eval("NotGoodNumber")%>
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
						<td class="number">
							<%#Eval("BrokenQuantity")%>
						</td>
						<td class="number">
							<%#Eval("WrongQuantity")%>
						</td>
						<td class="number">
							<%#Eval("LackQuantity")%>
						</td>
					</tr>
				</ItemTemplate>
			</asp:ListView>
		</ItemTemplate>
	</asp:ListView>
	<asp:ObjectDataSource ID="odsAbnormalOrder" runat="server" SelectMethod="FindOrderHasAbnormalReceive"
		SelectCountMethod="CountOrderHasAbnormalReceive" TypeName="VDMS.II.Report.AbnormalReceiveDAO"
		OnSelected="odsAbnormalOrder_Selected">
		<SelectParameters>
			<asp:ControlParameter ControlID="ddlArea" Name="dbCode" PropertyName="SelectedValue"
				Type="String" />
			<asp:ControlParameter ControlID="ddlDealers" Name="dealerCode" PropertyName="SelectedValue"
				Type="String" />
			<asp:ControlParameter ControlID="txtFromOrder" Name="tiptopNoFrom" PropertyName="Text"
				Type="String" />
			<asp:ControlParameter ControlID="txtToOrder" Name="tiptopNoTo" PropertyName="Text"
				Type="String" />
			<asp:ControlParameter ControlID="txtFromDate" Name="dateFrom" PropertyName="Text"
				Type="String" />
			<asp:ControlParameter ControlID="txtToDate" Name="dateTo" PropertyName="Text" Type="String" />
			<asp:Parameter Name="approveLevel" Type="Int32" DefaultValue="2" />
		</SelectParameters>
	</asp:ObjectDataSource>
	<asp:ListView ID="lvExcel" runat="server">
		<LayoutTemplate>
			<div id="grid" class="grid">
				<table class="datatable" cellpadding="0" cellspacing="0">
					<thead>
						<tr>
							<th>
								<asp:Literal ID="Literal2" runat="server" Text="Order number" meta:resourcekey="Literal2Resource3"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal12" runat="server" Text="Part code" meta:resourcekey="Literal12Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal13" runat="server" Text="Enlish name" meta:resourcekey="Literal13Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal14" runat="server" Text="Vietnamese name" meta:resourcekey="Literal14Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal15" runat="server" Text="Broken" meta:resourcekey="Literal15Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal16" runat="server" Text="Wrong" meta:resourcekey="Literal16Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal17" runat="server" Text="Lack" meta:resourcekey="Literal17Resource1"></asp:Literal>
							</th>
						</tr>
					</thead>
					<tbody>
						<tr id="itemPlaceholder" runat="server">
						</tr>
					</tbody>
					<tfoot>
						<tr class="sumLine">
							<td colspan="4" class="right">
								<asp:Literal ID="Literal6" runat="server" Text="Total" meta:resourcekey="Literal6Resource3"></asp:Literal>
							</td>
							<td class="number">
								<asp:Literal ID="litTotalBroken" runat="server" meta:resourcekey="litTotalBrokenResource2"></asp:Literal>
							</td>
							<td class="number">
								<asp:Literal ID="litTotalWrong" runat="server" meta:resourcekey="litTotalWrongResource2"></asp:Literal>
							</td>
							<td class="number">
								<asp:Literal ID="litTotalLack" runat="server" meta:resourcekey="litTotalLackResource2"></asp:Literal>
							</td>
						</tr>
					</tfoot>
				</table>
			</div>
		</LayoutTemplate>
		<ItemTemplate>
			<tr class="group">
				<td align="left">
					<%#Eval("TipTopNumber")%>
				</td>
				<td>
					&nbsp;
				</td>
				<td>
					&nbsp;
				</td>
				<td>
					&nbsp;
				</td>
				<td class="number">
					<%#Eval("Broken")%>
				</td>
				<td class="number">
					<%#Eval("Wrong")%>
				</td>
				<td class="number">
					<%#Eval("Lack")%>
				</td>
			</tr>
			<asp:ListView ID="lv1" runat="server" DataSource='<%# Eval("ReceiveDetails") %>'>
				<LayoutTemplate>
					<tr runat="server" id="itemPlaceholder" />
				</LayoutTemplate>
				<ItemTemplate>
					<tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
						<td>
							&nbsp;
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
						<td class="number">
							<%#Eval("BrokenQuantity")%>
						</td>
						<td class="number">
							<%#Eval("WrongQuantity")%>
						</td>
						<td class="number">
							<%#Eval("LackQuantity")%>
						</td>
					</tr>
				</ItemTemplate>
			</asp:ListView>
		</ItemTemplate>
	</asp:ListView>
</asp:Content>
