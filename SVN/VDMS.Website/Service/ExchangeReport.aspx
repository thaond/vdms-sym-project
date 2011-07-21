<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="ExchangeReport.aspx.cs" Inherits="Service_ExchangeSummary" Title="Exchange vouchers report"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Src="~/Controls/Services/ExchangeVoucherList.ascx" TagName="ExchangeVoucherReport"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
    </asp:Literal><asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Check"
        meta:resourcekey="ValidationSummary1Resource1" />
    <div class="form">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 98%">
            <tr>
                <td class="InputTable">
                    <table border="0" cellpadding="2" cellspacing="0">
                        <tr>
                            <td class="nameField">
                                <asp:Literal ID="Literal3" runat="server" Text="Proposal exchange No:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtProposalNo" runat="server" MaxLength="30" Width="168px" meta:resourcekey="txtProposalNoResource1"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td class="nameField">
                                <asp:Literal ID="litConfirmedDate" runat="server" Text="Confirmed from:" meta:resourcekey="litConfirmedDateResource1"></asp:Literal>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtConfirmedFrom" runat="server" Width="88px" meta:resourcekey="txtConfirmedFromResource1"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="meeConfirmedFromDate" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtConfirmedFrom" Enabled="True">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:CalendarExtender ID="ceConfirmedFromDate" runat="server" PopupButtonID="imgConFrom"
                                                TargetControlID="txtConfirmedFrom" BehaviorID="ceComfirmedFromDate" Enabled="True">
                                            </ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvConfirmedFrom" runat="server" ValidationGroup="Check"
                                                Enabled="false" ControlToValidate="txtConfirmedFrom" CssClass="Validator" SetFocusOnError="True"
                                                ErrorMessage='First "Confirmed date" cannot be blank!' meta:resourcekey="rfvConfirmedFromResource1">*</asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgConFrom" runat="Server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                                meta:resourcekey="imgConFromResource1" />
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;
                                        </td>
                                        <td>
                                            ~
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtComfirmedTo" runat="server" Width="88px" meta:resourcekey="txtComfirmedToResource1"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="meeComfirmedToDate" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtComfirmedTo" Enabled="True" CultureAMPMPlaceholder=""
                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:CalendarExtender ID="ceComfirmedToDate" runat="server" PopupButtonID="imbConTo"
                                                TargetControlID="txtComfirmedTo" Enabled="True">
                                            </ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvConTo" runat="server" ControlToValidate="txtToDate"
                                                ValidationGroup="Check" Enabled="false" CssClass="Validator" SetFocusOnError="True"
                                                ErrorMessage='Last "Comfirmed date" cannot be blank!' meta:resourcekey="rfvConToResource1">*</asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imbConTo" runat="Server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                                meta:resourcekey="imbConToResource1" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="nameField">
                                <asp:Literal ID="Literal7" runat="server" Text="Engine number:" meta:resourcekey="Literal7Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEngineNumber" runat="server" MaxLength="30" Width="168px" meta:resourcekey="txtEngineNumberResource1"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td class="nameField">
                                <asp:Image ID="image" Visible="false" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" /><asp:Literal
                                    ID="Literal2" runat="server" Text="Repair date:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFromDate" runat="server" Width="88px" meta:resourcekey="txtFromDateResource1"></asp:TextBox><ajaxToolkit:MaskedEditExtender
                                                ID="meeFromDate" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFromDate"
                                                CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY"
                                                CultureDatePlaceholder="/" CultureDecimalPlaceholder="." CultureThousandsPlaceholder=","
                                                CultureTimePlaceholder=":" Enabled="True">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" PopupButtonID="ibFromDate"
                                                TargetControlID="txtFromDate" Enabled="True">
                                            </ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" Enabled="false" runat="server" ControlToValidate="txtFromDate"
                                                CssClass="Validator" SetFocusOnError="True" ValidationGroup="Check" ErrorMessage='First "Repair date" cannot be blank!'
                                                meta:resourcekey="rfvFromDateResource1" Text="*"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibFromDate" runat="Server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                                meta:resourcekey="ibFromDateResource1" />
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;
                                        </td>
                                        <td>
                                            ~
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToDate" runat="server" Width="88px" meta:resourcekey="txtToDateResource1"></asp:TextBox><ajaxToolkit:MaskedEditExtender
                                                ID="meeToDate" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtToDate"
                                                CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY"
                                                CultureDatePlaceholder="/" CultureDecimalPlaceholder="." CultureThousandsPlaceholder=","
                                                CultureTimePlaceholder=":" Enabled="True">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:CalendarExtender ID="ceToDate" runat="server" PopupButtonID="ibToDate"
                                                TargetControlID="txtToDate" Enabled="True">
                                            </ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvToDate" Enabled="false" runat="server" ControlToValidate="txtToDate"
                                                CssClass="Validator" SetFocusOnError="True" ValidationGroup="Check" ErrorMessage='Last "Repair date" cannot be blank!'
                                                meta:resourcekey="rfvToDateResource1" Text="*"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibToDate" runat="Server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                                meta:resourcekey="ibToDateResource1" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="nameField">
                                <asp:Literal ID="Literal4" runat="server" Text="Status:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="173px" meta:resourcekey="ddlStatusResource1">
                                    <asp:ListItem Text="Not Validate" Value="0" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                    <asp:ListItem Text="Validated" Value="1" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                    <asp:ListItem Selected="True" Text="All" Value="-1" meta:resourcekey="ListItemResource3"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Literal ID="Literal1" runat="server" Text="Exchange voucher:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtVoucherFrom" runat="server" Width="113px" MaxLength="30" meta:resourcekey="txtVoucherFromResource1"></asp:TextBox>&nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            ~
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVoucherTo" runat="server" Width="113px" MaxLength="30" meta:resourcekey="txtVoucherToResource1"></asp:TextBox>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="nameField">
                            </td>
                            <td align="right">
                            </td>
                            <td align="right" class="forceNowrap">
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                            </td>
                            <td align="right" class="nameField">
                            </td>
                            <td align="right">
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="right">
                            </td>
                            <td align="right">
                            </td>
                            <td align="right">
                            </td>
                            <td align="right">
                                &nbsp;<asp:Button ID="cmdSubmit" runat="server" SkinID="SubmitButton" Text="Search"
                                    ValidationGroup="Check" Width="113px" OnClick="cmdSubmit_Click" meta:resourcekey="cmdSubmitResource1" />&nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <uc1:ExchangeVoucherReport ID="evReport" runat="server" />
</asp:Content>
