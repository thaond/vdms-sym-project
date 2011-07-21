<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="LookupExchange.aspx.cs" Inherits="Service_LookupExchange" Title="Exchange vouchers report"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Src="~/Controls/Services/ExchangeVoucherList.ascx" TagName="ExchangeVoucherReport"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Check"
        meta:resourcekey="ValidationSummary1Resource1" />
    <div class="form">
        <table>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="Proposal exchange No:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtDealerCode" runat="server" MaxLength="30" Width="168px" meta:resourcekey="txtProposalNoResource1"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Literal ID="litConfirmedDate" runat="server" Text="Confirmed from:" meta:resourcekey="litConfirmedDateResource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtConfirmedFrom" runat="server" Width="88px" meta:resourcekey="txtConfirmedFromResource1"></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="meeConfirmedFromDate" runat="server" Mask="99/99/9999"
                        MaskType="Date" TargetControlID="txtConfirmedFrom" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceConfirmedFromDate" runat="server" PopupButtonID="imgConFrom"
                        TargetControlID="txtConfirmedFrom" BehaviorID="ceComfirmedFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    <asp:ImageButton ID="imgConFrom" runat="Server" OnClientClick="return false;" SkinID="CalendarImageButton"
                        meta:resourcekey="imgConFromResource1" />
                    <asp:RequiredFieldValidator ID="rfvConfirmedFrom" runat="server" ControlToValidate="txtConfirmedFrom"
                        CssClass="Validator" SetFocusOnError="True" ValidationGroup="Check" Enabled="false"
                        ErrorMessage='First "Confirmed date" cannot be blank!' meta:resourcekey="rfvConfirmedFromResource1">*</asp:RequiredFieldValidator>
                    ~ <asp:TextBox ID="txtComfirmedTo" runat="server" Width="88px" meta:resourcekey="txtComfirmedToResource1"></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="meeComfirmedToDate" runat="server" Mask="99/99/9999"
                        MaskType="Date" TargetControlID="txtComfirmedTo" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceComfirmedToDate" runat="server" PopupButtonID="imbConTo"
                        TargetControlID="txtComfirmedTo" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    <asp:ImageButton ID="imbConTo" runat="Server" OnClientClick="return false;" SkinID="CalendarImageButton"
                        meta:resourcekey="imbConToResource1" />
                    <asp:RequiredFieldValidator ID="rfvConTo" runat="server" ControlToValidate="txtToDate"
                        CssClass="Validator" SetFocusOnError="True" ValidationGroup="Check" Enabled="false"
                        ErrorMessage='Last "Comfirmed date" cannot be blank!' meta:resourcekey="rfvConToResource1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal7" runat="server" Text="Engine number:" meta:resourcekey="Literal7Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtEngineNumber" runat="server" MaxLength="30" Width="168px" meta:resourcekey="txtEngineNumberResource1"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Image ID="image" Visible="false" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" /><asp:Literal
                        ID="Literal2" runat="server" Text="Repair date:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" Width="88px" meta:resourcekey="txtFromDateResource1"></asp:TextBox><ajaxToolkit:MaskedEditExtender
                        ID="meeFromDate" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFromDate"
                        BehaviorID="meeFromDate" CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="MDY" CultureDatePlaceholder="/" CultureDecimalPlaceholder="."
                        CultureThousandsPlaceholder="," CultureTimePlaceholder=":" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" PopupButtonID="ibFromDate"
                        TargetControlID="txtFromDate" BehaviorID="ceFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    <asp:ImageButton ID="ibFromDate" runat="Server" OnClientClick="return false;" SkinID="CalendarImageButton"
                        meta:resourcekey="ibFromDateResource1" />
                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtFromDate"
                        CssClass="Validator" SetFocusOnError="True" Enabled="false" ValidationGroup="Check"
                        ErrorMessage='First "Repair date" cannot be blank!' meta:resourcekey="rfvFromDateResource1">*</asp:RequiredFieldValidator>
                    ~ <asp:TextBox ID="txtToDate" runat="server" Width="88px" meta:resourcekey="txtToDateResource1"></asp:TextBox><ajaxToolkit:MaskedEditExtender
                        ID="meeToDate" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtToDate"
                        BehaviorID="meeToDate" CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="MDY" CultureDatePlaceholder="/" CultureDecimalPlaceholder="."
                        CultureThousandsPlaceholder="," CultureTimePlaceholder=":" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceToDate" runat="server" PopupButtonID="ibToDate"
                        TargetControlID="txtToDate" BehaviorID="ceToDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    <asp:ImageButton ID="ibToDate" runat="Server" OnClientClick="return false;" SkinID="CalendarImageButton"
                        meta:resourcekey="ibToDateResource1" />
                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                        CssClass="Validator" SetFocusOnError="True" Enabled="false" ValidationGroup="Check"
                        ErrorMessage='Last "Repair date" cannot be blank!' meta:resourcekey="rfvToDateResource1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="Status:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" Width="173px" meta:resourcekey="ddlStatusResource1">
                        <asp:ListItem Text="Not Validate" Value="0" meta:resourcekey="ListItemResource1"></asp:ListItem>
                        <asp:ListItem Text="Validated" Value="1" meta:resourcekey="ListItemResource2"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="All" Value="" meta:resourcekey="ListItemResource3"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Exchange voucher:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtVoucherFrom" runat="server" Width="113px" MaxLength="30" meta:resourcekey="txtVoucherFromResource1"></asp:TextBox>
                    ~ <asp:TextBox ID="txtVoucherTo" runat="server" Width="113px" MaxLength="30" meta:resourcekey="txtVoucherToResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
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
                <td>
                    <asp:Button ID="cmdSubmit" runat="server" SkinID="SubmitButton" Text="Search" ValidationGroup="Check"
                        Width="113px" OnClick="cmdSubmit_Click" meta:resourcekey="cmdSubmitResource1" />
                </td>
            </tr>
        </table>
    </div>
    <uc1:ExchangeVoucherReport ID="evReport" runat="server" />
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="h">
</asp:Content>
