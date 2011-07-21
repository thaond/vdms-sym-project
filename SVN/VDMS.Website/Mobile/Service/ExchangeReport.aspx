<%@ Page Language="C#" MasterPageFile="~/MP/Mobile.master" AutoEventWireup="true"
    Theme="Mobile" CodeFile="ExchangeReport.aspx.cs" Inherits="MService_ExchangeSummary"
    Title="Exchange vouchers report" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<%@ Register Src="~/Controls/Services/ExchangeVoucherList.ascx" TagName="ExchangeVoucherReport"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
    </asp:Literal><asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Check"
        meta:resourcekey="ValidationSummary1Resource1" />
    <div class="form">
        <table>
            <tbody>
                <tr>
                    <td>
                        <asp:Literal ID="Literal3" runat="server" Text="Proposal exchange No:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtProposalNo" runat="server" MaxLength="30" meta:resourcekey="txtProposalNoResource1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="litConfirmedDate" runat="server" Text="Confirmed from:" meta:resourcekey="litConfirmedDateResource1"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtConfirmedFrom" runat="server" Width="40%" meta:resourcekey="txtConfirmedFromResource1"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="meeConfirmedFromDate" runat="server" Mask="99/99/9999"
                            MaskType="Date" TargetControlID="txtConfirmedFrom" Enabled="True">
                        </ajaxToolkit:MaskedEditExtender>
                        <asp:RequiredFieldValidator ID="rfvConfirmedFrom" runat="server" ValidationGroup="Check"
                            Enabled="false" ControlToValidate="txtConfirmedFrom" CssClass="Validator" SetFocusOnError="True"
                            ErrorMessage='First "Confirmed date" cannot be blank!' meta:resourcekey="rfvConfirmedFromResource1">*</asp:RequiredFieldValidator>
                        <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="txtConfirmedFrom"
                            Enabled="true" InvalidValueMessage="*" ControlExtender="meeConfirmedFromDate"></ajaxToolkit:MaskedEditValidator>
                        ~
                        <asp:TextBox ID="txtComfirmedTo" runat="server" Width="40%" meta:resourcekey="txtComfirmedToResource1"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="meeComfirmedToDate" runat="server" Mask="99/99/9999"
                            MaskType="Date" TargetControlID="txtComfirmedTo" Enabled="True" CultureAMPMPlaceholder=""
                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="">
                        </ajaxToolkit:MaskedEditExtender>
                        <asp:RequiredFieldValidator ID="rfvConTo" runat="server" ControlToValidate="txtToDate"
                            ValidationGroup="Check" Enabled="false" CssClass="Validator" SetFocusOnError="True"
                            ErrorMessage='Last "Comfirmed date" cannot be blank!' meta:resourcekey="rfvConToResource1">*</asp:RequiredFieldValidator>
                        <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlToValidate="txtComfirmedTo"
                            Enabled="true" InvalidValueMessage="*" ControlExtender="meeComfirmedToDate"></ajaxToolkit:MaskedEditValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="Literal7" runat="server" Text="Engine number:" meta:resourcekey="Literal7Resource1"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEngineNumber" runat="server" MaxLength="30" meta:resourcekey="txtEngineNumberResource1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Image ID="image" Visible="false" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" /><asp:Literal
                            ID="Literal2" runat="server" Text="Repair date:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" Width="40%" meta:resourcekey="txtFromDateResource1"></asp:TextBox><ajaxToolkit:MaskedEditExtender
                            ID="meeFromDate" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFromDate"
                            CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY"
                            CultureDatePlaceholder="/" CultureDecimalPlaceholder="." CultureThousandsPlaceholder=","
                            CultureTimePlaceholder=":" Enabled="True">
                        </ajaxToolkit:MaskedEditExtender>
                        <asp:RequiredFieldValidator ID="rfvFromDate" Enabled="false" runat="server" ControlToValidate="txtFromDate"
                            CssClass="Validator" SetFocusOnError="True" ValidationGroup="Check" ErrorMessage='First "Repair date" cannot be blank!'
                            meta:resourcekey="rfvFromDateResource1" Text="*"></asp:RequiredFieldValidator>
                            <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator3" runat="server" ControlToValidate="txtFromDate"
                            Enabled="true" InvalidValueMessage="*" ControlExtender="meeFromDate"></ajaxToolkit:MaskedEditValidator>
                        ~
                        <asp:TextBox ID="txtToDate" runat="server" Width="40%" meta:resourcekey="txtToDateResource1"></asp:TextBox><ajaxToolkit:MaskedEditExtender
                            ID="meeToDate" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtToDate"
                            CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY"
                            CultureDatePlaceholder="/" CultureDecimalPlaceholder="." CultureThousandsPlaceholder=","
                            CultureTimePlaceholder=":" Enabled="True">
                        </ajaxToolkit:MaskedEditExtender>
                        <asp:RequiredFieldValidator ID="rfvToDate" Enabled="false" runat="server" ControlToValidate="txtToDate"
                            CssClass="Validator" SetFocusOnError="True" ValidationGroup="Check" ErrorMessage='Last "Repair date" cannot be blank!'
                            meta:resourcekey="rfvToDateResource1" Text="*"></asp:RequiredFieldValidator>
                            <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator4" runat="server" ControlToValidate="txtToDate"
                            Enabled="true" InvalidValueMessage="*" ControlExtender="meeToDate"></ajaxToolkit:MaskedEditValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="Literal4" runat="server" Text="Status:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" meta:resourcekey="ddlStatusResource1">
                            <asp:ListItem Text="Not Validate" Value="0" meta:resourcekey="ListItemResource1"></asp:ListItem>
                            <asp:ListItem Text="Validated" Value="1" meta:resourcekey="ListItemResource2"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="All" Value="-1" meta:resourcekey="ListItemResource3"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="Literal1" runat="server" Text="Exchange voucher:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtVoucherFrom" runat="server" Width="113px" MaxLength="30" meta:resourcekey="txtVoucherFromResource1"></asp:TextBox>&nbsp;
                        ~
                        <asp:TextBox ID="txtVoucherTo" runat="server" Width="113px" MaxLength="30" meta:resourcekey="txtVoucherToResource1"></asp:TextBox>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;<asp:Button ID="cmdSubmit" runat="server" SkinID="SubmitButton" Text="Search"
                            ValidationGroup="Check" Width="40%" OnClick="cmdSubmit_Click" meta:resourcekey="cmdSubmitResource1" />&nbsp;
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <uc1:ExchangeVoucherReport ID="evReport" runat="server" />
</asp:Content>
