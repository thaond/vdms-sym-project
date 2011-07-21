<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
	CodeFile="RepairList.aspx.cs" Inherits="Service_RepairList" Title="Service list"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Src="~/Controls/UpdateProgress.ascx" TagName="UpdateProgress" TagPrefix="uc1" %>
<%@ Register Assembly="VDMS.Common" Namespace="VDMS.Common.Web" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
	<div class="form">
		<table border="0" cellpadding="2" cellspacing="2" class="InputTable" style="width: 99%">
			<tr>
				<td colspan="5">
					<asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Check"
						meta:resourcekey="ValidationSummary1Resource1" />
					<asp:BulletedList ID="bllErrorMsg" runat="server" CssClass="errorMsg" meta:resourcekey="bllErrorMsgResource1">
					</asp:BulletedList>
				</td>
				<td>
				</td>
			</tr>
			<tr>
				<td class="nameField">
					<asp:Image ID="Image1" runat="server" meta:resourcekey="Image4Resource1" SkinID="RequireFieldNon" />
					<asp:Literal ID="Literal12" runat="server" meta:resourcekey="Literal2Resource1x"
						Text="Dealer code:"></asp:Literal>
				</td>
				<td colspan="4">
					<vdms:DealerList ID="ddlDealer" EnabledSaperateByDB="True" RootDealer="/" RemoveRootItem="True"
						runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDealer_SelectedIndexChanged"
						OnDataBound="ddlDealer_DataBound" EnabledSaperateByArea="False" MergeCode="False"
						meta:resourcekey="ddlDealerResource1" ShowEmptyItem="False" ShowSelectAllItem="False">
					</vdms:DealerList>
				</td>
				<td>
				</td>
			</tr>
			<tr>
				<td class="nameField">
					<asp:Image ID="Image4" runat="server" meta:resourcekey="Image4Resource1" SkinID="RequireFieldNon" />
					<asp:Literal ID="Literal2" runat="server" Text="Branch code:" meta:resourcekey="Literal2Resource1"></asp:Literal>
				</td>
				<td>
					<asp:UpdatePanel ID="UpdatePanel1" runat="server">
						<ContentTemplate>
							<vdms:WarehouseList ID="ddlBranchCode" ShowSelectAllItem="True" Type="V" runat="server"
								meta:resourcekey="ddlBranchCodeResource1" DontAutoUseCurrentSealer="False" ShowEmptyItem="False"
								UseVIdAsValue="False">
							</vdms:WarehouseList>
						</ContentTemplate>
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="ddlDealer" EventName="SelectedIndexChanged" />
						</Triggers>
					</asp:UpdatePanel>
				</td>
				<td>
					<uc1:UpdateProgress ID="uc1UpdateProgress" runat="server" />
				</td>
				<td class="nameField">
					<asp:Literal ID="Literal6" runat="server" Text="Service Sheet No:" meta:resourcekey="Literal6Resource1"></asp:Literal>
				</td>
				<td>
					<table border="0" cellpadding="0" cellspacing="0">
						<tr>
							<td>
								<asp:TextBox ID="txtSheetNoFrom" runat="server" Width="140px" meta:resourcekey="txtSheetNoFromResource1"></asp:TextBox>
							</td>
							<td>
								&nbsp;
							</td>
							<td>
								~
							</td>
							<td>
								&nbsp;
							</td>
							<td>
								<asp:TextBox ID="txtSheetNoTo" runat="server" Width="140px" meta:resourcekey="txtSheetNoToResource1"></asp:TextBox>
							</td>
						</tr>
					</table>
				</td>
				<td>
				</td>
			</tr>
			<tr>
				<td class="nameField">
					<asp:Image ID="Image2" runat="server" meta:resourcekey="Image4Resource1" SkinID="RequireFieldNon" />
					<asp:Literal ID="Literal4" runat="server" Text="Buy date:" meta:resourcekey="Literal4Resource1"></asp:Literal>
				</td>
				<td>
					<table border="0" cellpadding="0" cellspacing="0">
						<tr>
							<td>
								<asp:TextBox ID="txtBuyDateFrom" runat="server" Width="140px" meta:resourcekey="txtBuyDateFromResource1"></asp:TextBox>
								<ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
									PopupButtonID="ImageButton1" TargetControlID="txtBuyDateFrom">
								</ajaxToolkit:CalendarExtender>
								<ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" CultureAMPMPlaceholder="AM;PM"
									CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
									CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
									Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtBuyDateFrom">
								</ajaxToolkit:MaskedEditExtender>
							</td>
							<td>
								<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtRepairDateFrom"
									ErrorMessage='"By date" cannot be blank!' meta:resourcekey="rqvBuydateResource1"
									SetFocusOnError="True" Text="*" ValidationGroup="Check" Enabled="False"></asp:RequiredFieldValidator>
							</td>
							<td>
								<asp:ImageButton ID="ImageButton1" runat="server" meta:resourcekey="ibtnCalendarResource1"
									OnClientClick="return false;" SkinID="CalendarImageButton" />
							</td>
							<td>
								&nbsp;
							</td>
							<td>
								~
							</td>
							<td>
								&nbsp;
							</td>
							<td>
								<asp:TextBox ID="txtBuyDateTo" runat="server" Width="140px" meta:resourcekey="txtBuyDateToResource1"></asp:TextBox>
								<ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True"
									PopupButtonID="ImageButton3" TargetControlID="txtBuyDateTo">
								</ajaxToolkit:CalendarExtender>
								<ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender4" runat="server" CultureAMPMPlaceholder="AM;PM"
									CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
									CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
									Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtBuyDateTo">
								</ajaxToolkit:MaskedEditExtender>
							</td>
							<td>
								<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRepairDateFrom"
									ErrorMessage='"By date" cannot be blank!' meta:resourcekey="rqvBuydateResource1"
									SetFocusOnError="True" Text="*" ValidationGroup="Check" Enabled="False"></asp:RequiredFieldValidator>
							</td>
							<td>
								<asp:ImageButton ID="ImageButton3" runat="server" meta:resourcekey="ibtnCalendarResource1"
									OnClientClick="return false;" SkinID="CalendarImageButton" />
							</td>
						</tr>
					</table>
				</td>
				<td class="nameField">
					&nbsp;
				</td>
				<td class="nameField">
					<asp:Literal ID="Literal7" runat="server" Text="Customer name:" meta:resourcekey="Literal7Resource1"></asp:Literal>
				</td>
				<td>
					<asp:TextBox ID="txtCustName" runat="server" Width="302px" meta:resourcekey="txtCustNameResource1"></asp:TextBox>
				</td>
				<td>
				</td>
			</tr>
			<tr>
				<td class="nameField">
					<asp:Image ID="Image3" runat="server" meta:resourcekey="Image4Resource1" SkinID="RequireField" />
					<asp:Literal ID="Literal3" runat="server" Text="Repair date:" meta:resourcekey="Literal3Resource1"></asp:Literal>
				</td>
				<td>
					<table border="0" cellpadding="0" cellspacing="0">
						<tr>
							<td>
								<asp:TextBox ID="txtRepairDateFrom" runat="server" Width="140px" meta:resourcekey="txtRepairDateFromResource1"></asp:TextBox>
								<ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True"
									PopupButtonID="ibtnCalendar" TargetControlID="txtRepairDateFrom">
								</ajaxToolkit:CalendarExtender>
								<ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" CultureAMPMPlaceholder="AM;PM"
									CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
									CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
									Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtRepairDateFrom">
								</ajaxToolkit:MaskedEditExtender>
							</td>
							<td>
								<asp:RequiredFieldValidator ID="rqvBuydate" runat="server" ControlToValidate="txtRepairDateFrom"
									ErrorMessage='"By date" cannot be blank!' meta:resourcekey="rqvBuydateResource1"
									SetFocusOnError="True" Text="*" ValidationGroup="Check"></asp:RequiredFieldValidator>
							</td>
							<td>
								<asp:ImageButton ID="ibtnCalendar" runat="Server" meta:resourcekey="ibtnCalendarResource1"
									OnClientClick="return false;" SkinID="CalendarImageButton" />
							</td>
							<td>
								&nbsp;
							</td>
							<td>
								~
							</td>
							<td>
								&nbsp;
							</td>
							<td>
								<asp:TextBox ID="txtRepairDateTo" runat="server" Width="140px" meta:resourcekey="txtRepairDateToResource1"></asp:TextBox>
								<ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True"
									PopupButtonID="ImageButton2" TargetControlID="txtRepairDateTo">
								</ajaxToolkit:CalendarExtender>
								<ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" runat="server" CultureAMPMPlaceholder="AM;PM"
									CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
									CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
									Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtRepairDateTo">
								</ajaxToolkit:MaskedEditExtender>
							</td>
							<td>
								<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtRepairDateFrom"
									ErrorMessage='"By date" cannot be blank!' meta:resourcekey="rqvBuydateResource1"
									SetFocusOnError="True" Text="*" Enabled="False"></asp:RequiredFieldValidator>
							</td>
							<td>
								<asp:ImageButton ID="ImageButton2" runat="server" meta:resourcekey="ibtnCalendarResource1"
									OnClientClick="return false;" SkinID="CalendarImageButton" />
							</td>
						</tr>
					</table>
				</td>
				<td>
				</td>
				<td class="nameField">
					<asp:Literal ID="Literal8" runat="server" Text="Engine number:" meta:resourcekey="Literal8Resource1"></asp:Literal>
				</td>
				<td>
					<asp:TextBox ID="txtEngineNumber" runat="server" Width="140px" meta:resourcekey="txtEngineNumberResource1"></asp:TextBox>
				</td>
				<td>
				</td>
			</tr>
			<tr>
				<td>
				</td>
				<td>
				</td>
				<td>
				</td>
				<td>
				</td>
				<td align="right">
					<asp:Button ID="btnCheck" runat="server" Text="Check" ValidationGroup="Check" Width="140px"
						OnClick="btnCheck_Click" meta:resourcekey="btnCheckResource1" />
				</td>
				<td>
				</td>
			</tr>
		</table>
	</div>
	<div class="grid">
		<asp:ListView ID="lv" runat="server" OnDataBound="lv_DataBound">
			<LayoutTemplate>
				<table class="datatable" cellpadding="0" cellspacing="0">
					<thead>
						<tr>
							<th>
								<asp:Literal ID="Literal12" runat="server" Text="No" meta:resourcekey="Literal12Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal1" runat="server" Text="Part code" meta:resourcekey="Literal1Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal5" runat="server" Text="Unit price" meta:resourcekey="Literal5Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal6" runat="server" Text="Quantity" meta:resourcekey="Literal6Resource2"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal7" runat="server" Text="Warranty amount" meta:resourcekey="Literal7Resource2"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal8" runat="server" Text="Warranty fee" meta:resourcekey="Literal8Resource2"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal9" runat="server" Text="Service amount" meta:resourcekey="Literal9Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal10" runat="server" Text="Service fee" meta:resourcekey="Literal10Resource1"></asp:Literal>
							</th>
						</tr>
					</thead>
					<tbody>
						<tr id="itemPlaceholder" runat="server">
						</tr>
					</tbody>
					<tfoot>
						<tr>
							<td class="pager" colspan="8">
								<vdms:DataPager runat="server" ID="partPager" PagedControlID="lv">
								</vdms:DataPager>
							</td>
						</tr>
						<tr class="group">
							<td colspan="4" class="right">
								<asp:Literal ID="Literal17" runat="server" Text='<%$ Resources:Constants, Total %>'></asp:Literal>:
							</td>
							<td class="number">
								<asp:Label CssClass="altCol1" ID="lbAllWA" runat="server" meta:resourcekey="lbAllWAResource1"></asp:Label>
							</td>
							<td class="number">
								<asp:Label CssClass="altCol1" ID="lbAllWF" runat="server" meta:resourcekey="lbAllWFResource1"></asp:Label>
							</td>
							</td>
							<td class="number">
								<asp:Label CssClass="altCol2" ID="lbAllSA" runat="server" meta:resourcekey="lbAllSAResource1"></asp:Label>
							</td>
							</td>
							<td class="number">
								<asp:Label CssClass="altCol2" ID="lbAllSF" runat="server" meta:resourcekey="lbAllSFResource1"></asp:Label>
							</td>
							</td>
						</tr>
					</tfoot>
				</table>
			</LayoutTemplate>
			<ItemTemplate>
				<tr class="group">
					<td class="center">
						<%# Container.DisplayIndex + 1 %>
					</td>
					<td colspan="7">
						<table>
							<tr>
								<td class="right">
									<asp:Literal ID="Literal11" runat="server" Text="Customer:" meta:resourcekey="Literal11Resource1"></asp:Literal>
								</td>
								<td class="normal">
									<%#Eval("CusName")%><asp:Image ID="Image4" runat="server" meta:resourcekey="Image4Resource1"
										SkinID="RequireFieldNon" />
								</td>
								<td class="right">
									<asp:Literal ID="Literal13" runat="server" Text="Engine number:" meta:resourcekey="Literal13Resource1"></asp:Literal>
								</td>
								<td class="normal">
									<%#Eval("EngineNumber")%><asp:Image ID="Image5" runat="server" meta:resourcekey="Image4Resource1"
										SkinID="RequireFieldNon" />
								</td>
								<td class="right">
									<asp:Literal ID="Literal14" runat="server" Text="Buy date:" meta:resourcekey="Literal14Resource1"></asp:Literal>
								</td>
								<td class="normal">
									<%#EvalDate(Eval("BuyDate"))%><asp:Image ID="Image6" runat="server" meta:resourcekey="Image4Resource1"
										SkinID="RequireFieldNon" />
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr class="selected">
					<td>
					</td>
					<td colspan="5">
						<asp:Literal ID="Literal15" runat="server" Text="Service sheet:" meta:resourcekey="Literal15Resource1"></asp:Literal>&nbsp;&nbsp;
						<asp:HyperLink runat="server" Target="_blank" Text='<%# Eval("ServiceSheetNumber") %>'
							CssClass="showDetail" ID="HyperLink1" NavigateUrl='<%# "WarrantyContent.aspx?srsn=" + Eval("ServiceSheetNumber") %>'
							meta:resourcekey="HyperLink1Resource1"></asp:HyperLink>
					</td>
					<td class="number">
						<span class="altCol2">
							<%# EvalNumber(Eval("AllServiceSpareAmountM"))%></span>
					</td>
					<td class="number">
						<span class="altCol2">
							<%# EvalNumber(Eval("ServiceFee"))%></span>
					</td>
				</tr>
				<asp:ListView ID="lvS" runat="server" DataSource='<%# Eval("ServiceList") %>'>
					<LayoutTemplate>
						<tr runat="server" id="itemPlaceholder" />
					</LayoutTemplate>
					<ItemTemplate>
						<tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
							<td class="center">
								<%# Container.DisplayIndex + 1 %>
							</td>
							<td>
								<%#Eval("PartCode")%>
							</td>
							<td class="number">
								<%#EvalNumber(Eval("UnitPrice"))%>
							</td>
							<td class="number">
								<%#EvalNumber(Eval("PartQty"))%>
							</td>
							<td>
							</td>
							<td>
							</td>
							<td class="number">
								<span class="altCol2">
									<%#EvalNumber((decimal)Eval("UnitPrice") * (int)Eval("PartQty"))%></span>
							</td>
							<td class="number">
							</td>
						</tr>
					</ItemTemplate>
				</asp:ListView>
				<tr class="selected" runat="server" visible='<%# Eval("ExchangeVoucherNumber") != null %>'>
					<td runat="server">
					</td>
					<td colspan="3" runat="server">
						<asp:Literal ID="Literal16" runat="server" Text="Exchange voucher:"></asp:Literal>&nbsp;&nbsp;
						<asp:HyperLink runat="server" Target="_blank" Text='<%# Eval("ExchangeVoucherNumber") %>'
							CssClass="showDetail" ID="HyperLink2" NavigateUrl='<%# "WarrantyContent.aspx?pcvn=" + Eval("ExchangeVoucherNumber") %>'
							meta:resourcekey="HyperLink2Resource1"></asp:HyperLink>
					</td>
					<td class="number" runat="server">
						<span class="altCol1">
							<%# EvalNumber(Eval("AllWarrantySpareAmountM"))%></span>
					</td>
					<td class="number" runat="server">
						<span class="altCol1">
							<%# EvalNumber(Eval("AllWarrantyFeeAmountM"))%></span>
					</td>
					<td runat="server">
					</td>
					<td runat="server">
					</td>
				</tr>
				<asp:ListView ID="lvE" runat="server" DataSource='<%# Eval("ExchangeList") %>'>
					<LayoutTemplate>
						<tr runat="server" id="itemPlaceholder" />
					</LayoutTemplate>
					<ItemTemplate>
						<tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
							<td class="center">
								<%# Container.DisplayIndex + 1 %>
							</td>
							<td>
								<%#Eval("PartCodeM")%>
							</td>
							<td class="number">
								<%#EvalNumber(Eval("UnitPriceM"))%>
							</td>
							<td class="number">
								<%#EvalNumber(Eval("PartQtyM"))%>
							</td>
							<td class="number">
								<span class="altCol1">
									<%#EvalNumber(Eval("WarrantySpareAmountM"))%></span>
							</td>
							<td class="number">
								<span class="altCol1">
									<%#EvalNumber(Eval("TotalFeeM"))%></span>
							</td>
							<td>
							</td>
							<td>
							</td>
						</tr>
					</ItemTemplate>
				</asp:ListView>
			</ItemTemplate>
		</asp:ListView>
		<asp:ObjectDataSource ID="odsSV" runat="server" SelectMethod="SelectSVList" SelectCountMethod="CountSVList"
			EnablePaging="True" TypeName="VDMS.I.ObjectDataSource.ServiceHeaderDataSource">
			<SelectParameters>
				<asp:ControlParameter ControlID="ddlDealer" Name="dCode" PropertyName="SelectedValue"
					Type="String" />
				<asp:ControlParameter ControlID="ddlBranchCode" Name="wCode" PropertyName="SelectedValue"
					Type="String" />
				<asp:ControlParameter ControlID="txtSheetNoFrom" Name="SNoFrom" PropertyName="Text"
					Type="String" />
				<asp:ControlParameter ControlID="txtSheetNoTo" Name="SNoTo" PropertyName="Text" Type="String" />
				<asp:ControlParameter ControlID="txtEngineNumber" Name="EngineNo" PropertyName="Text"
					Type="String" />
				<asp:ControlParameter ControlID="txtCustName" Name="cusName" PropertyName="Text"
					Type="String" />
				<asp:ControlParameter ControlID="txtBuyDateFrom" Name="buyFrom" PropertyName="Text"
					Type="String" />
				<asp:ControlParameter ControlID="txtBuyDateTo" Name="buyTo" PropertyName="Text" Type="String" />
				<asp:ControlParameter ControlID="txtRepairDateFrom" Name="repairFrom" PropertyName="Text"
					Type="String" />
				<asp:ControlParameter ControlID="txtRepairDateTo" Name="repairTo" PropertyName="Text"
					Type="String" />
			</SelectParameters>
		</asp:ObjectDataSource>
	</div>
	<p align="center">
		<asp:Button ID="btnPrint" runat="server" UseSubmitBehavior="False" OnClientClick="javascript:window.print(); return false;"
			Text="Print form" CausesValidation="False" meta:resourcekey="btnPrintResource1" />
	</p>
</asp:Content>
