<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
	CodeFile="verifyExchangeVoucher.aspx.cs" Inherits="Service_verifyExchangeVoucher"
	Title="Service VMEP verify" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Src="~/Controls/Services/ExchangeVoucherReport.ascx" TagName="ExchangeVoucherReport" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Services/ExchangeVoucherList.ascx" TagName="ExchangeVoucherReport2" TagPrefix="uc1" %>
	
<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
	
	<table border="0" cellpadding="2" cellspacing="2" style="width: 100%">
		<tr>
			<td>
				<%--<uc1:ExchangeVoucherReport ID="evReport2" runat="server"></uc1:ExchangeVoucherReport>--%>
				<uc1:ExchangeVoucherReport2 ID="evReport" runat="server"></uc1:ExchangeVoucherReport2>
			</td>
		</tr>
		<tr>
			<td align="center">&nbsp;
			</td>
		</tr>
		<tr runat="server" id="pnBatchAct">
			<td align="center">
				<asp:Button ID="btnApproveAllRemain" Visible="false" runat="server" meta:resourcekey="btnCloseResource1Aall"
					OnClick="btnApproveAllRemain_Click" Text="Approve all" />
				<asp:Button ID="btnRejectAll" Visible="false" runat="server" meta:resourcekey="btnCloseResource1Rall"
					OnClick="btnRejectAll_Click" Text="Reject all" />
				<asp:Button ID="btnApprovePage" runat="server" meta:resourcekey="btnCloseResource1Ap"
					OnClick="btnApprovePage_Click" Text="Approve this page" />
				<asp:Button ID="btnRejectPage" runat="server" meta:resourcekey="btnCloseResource1Rp"
					OnClick="btnRejectPage_Click" Text="Reject this page" />
				<asp:Button ID="btnClose" runat="server" meta:resourcekey="btnCloseResource1" OnClientClick="window.close(); return false;"
					Text="Close" Width="105px" />
			</td>
		</tr>
	</table>
	<br />
</asp:Content>
