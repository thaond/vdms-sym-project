<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
	CodeFile="ForAgency.aspx.cs" EnableEventValidation="false" Inherits="Vehicle_Report_ForAgency"
	Title="Statistical Report for Dealer" Culture="auto" meta:resourcekey="PageResource1"
	UICulture="auto" %>

<asp:Content ID="ReportContent" ContentPlaceHolderID="c" runat="Server">
	<asp:BulletedList ID="bllError" runat="server" CssClass="errorMsg">
	</asp:BulletedList>
	<asp:PlaceHolder ID="plDateFromTo" runat="server">
		<div class="form" style="width: 450px">
			<asp:ValidationSummary ID="ValidationSummary1" runat="server" meta:resourcekey="ValidationSummary1Resource1"
				ValidationGroup="Save" Width="100%" DisplayMode="List" />
			<asp:Label ID="lblErr" runat="server" Text="Label" ForeColor="Red" Visible="False"
				meta:resourcekey="lblErrResource1"></asp:Label>
			<table cellspacing="0" border="0" width="100%">
				<tr>
					<td valign="top" style="width:20%">
						<asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
						<asp:Localize ID="litFromDate" Text="From date:" runat="server" meta:resourcekey="litFromDateResource1"></asp:Localize>
					</td>
					<td valign="top">
						<asp:TextBox ID="txtFromDate" runat="server" Width="88px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
						<asp:ImageButton ID="ibFromDate" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
							meta:resourcekey="ibFromDateResource1" />
						<asp:RequiredFieldValidator ID="rfvFromDate" runat="server" SetFocusOnError="True"
							ValidationGroup="Save" ControlToValidate="txtFromDate" meta:resourcekey="rfvFromDateResource1"
							Text="*"></asp:RequiredFieldValidator>
						<asp:RangeValidator ID="rvFromDate" runat="server" ControlToValidate="txtFromDate"
							Display="Dynamic" ErrorMessage="Định dạng ngày không đúng!" meta:resourcekey="rvFromDateResource1"
							SetFocusOnError="True" Type="Date" ValidationGroup="Save">*</asp:RangeValidator>
						<ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
							Mask="99/99/9999" MaskType="Date" BehaviorID="meeFromDate" CultureAMPMPlaceholder="AM;PM"
							CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
							CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
							Enabled="True" />
						<ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtFromDate"
							PopupButtonID="ibFromDate" BehaviorID="ceFromDate" Enabled="True">
						</ajaxToolkit:CalendarExtender>
					</td>
				</tr>
				<tr>
					<td valign="top">
						<asp:Image ID="image1" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
						<asp:Localize ID="litTodate" runat="server" meta:resourcekey="litTodateResource1"
							Text="Đến ngày"></asp:Localize>
					</td>
					<td valign="top">
						<asp:TextBox ID="txtToDate" runat="server" Width="88px" meta:resourcekey="txtToDateResource1"></asp:TextBox>
						<asp:ImageButton ID="ibToDate" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
							meta:resourcekey="ibToDateResource1" />
						<asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="Validator" SetFocusOnError="True"
							ValidationGroup="Save" ControlToValidate="txtToDate" meta:resourcekey="rfvToDateResource1"
							Text="*"></asp:RequiredFieldValidator>
						<asp:RangeValidator ID="rvToDate" runat="server" ControlToValidate="txtToDate" Display="Dynamic"
							ErrorMessage="Định dạng ngày không đúng!" meta:resourcekey="rvToDateResource1"
							SetFocusOnError="True" Type="Date" ValidationGroup="Save">*</asp:RangeValidator>
						<ajaxToolkit:MaskedEditExtender ID="meeToDate" runat="server" TargetControlID="txtToDate"
							Mask="99/99/9999" MaskType="Date" BehaviorID="meeToDate" CultureAMPMPlaceholder="AM;PM"
							CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
							CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
							Enabled="True" />
						<ajaxToolkit:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtToDate"
							PopupButtonID="ibToDate" BehaviorID="ceToDate" Enabled="True">
						</ajaxToolkit:CalendarExtender>
					</td>
				</tr>
				<tr>
					<td>
					</td>
					<td>
						<asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="View Report"
							ValidationGroup="Save" meta:resourcekey="btnSearchResource1" />
					</td>
				</tr>
			</table>
		</div>
	</asp:PlaceHolder>
	<asp:Literal ID="liDateTitle" runat="server" meta:resourcekey="liDateTitleResource1"></asp:Literal><br />
	<br />
	<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
		Height="50px" meta:resourcekey="CrystalReportViewer1Resource1" Width="350px" />
</asp:Content>
