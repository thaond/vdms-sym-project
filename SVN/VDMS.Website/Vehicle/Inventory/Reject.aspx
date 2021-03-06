<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="Reject.aspx.cs" Inherits="Vehicle_Inventory_Reject" Title="Untitled Page"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Check"
            meta:resourcekey="ValidationSummary2Resource1" />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="save"
            meta:resourcekey="ValidationSummary2Resource1" />
        <asp:BulletedList ID="bllErrorMsg" runat="server" CssClass="errorMsg">
        </asp:BulletedList>
        <br />
        <table border="0" cellpadding="2" cellspacing="0" style="width: 100%">
            <tr>
                <td style="width: 20%;">
                    <asp:Image ID="image" runat="server" SkinID="RequireField" />
                    <asp:Literal ID="Literal13" runat="server" Text="Engine number:" meta:resourcekey="Literal13Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtMachineNo" runat="server" meta:resourcekey="txtMachineNoResource1"
                        MaxLength="20" CssClass="inputKeyField"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMachineNo"
                        ErrorMessage="Chưa nhập số máy" ValidationGroup="Check" meta:resourcekey="RequiredFieldValidator2Resource1">*</asp:RequiredFieldValidator>
                    <asp:Button ID="btnCheck" runat="server" Text="Check" ValidationGroup="Check" OnClick="btnCheck_Click"
                        meta:resourcekey="btnCheckResource1" />&nbsp;
                    <%--<asp:Label ID="lblMsg" runat="server" CssClass="errorMsg" meta:resourcekey="lblMsgResource1"></asp:Label>--%>
                    <asp:HiddenField ID="hdMachineNo" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <asp:Panel CssClass="form" ID="pnDetail" runat="server">
        <table border="0" cellpadding="2" cellspacing="2" style="width: 100%">
            <tr>
                <td style="width: 20%; white-space: nowrap; text-align: right">
                    <asp:Literal ID="Literal2" runat="server" Text="Model:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label SkinID="TextField" ID="lblModel" runat="server" meta:resourcekey="lblModelResource1"></asp:Label>
                </td>
                <td>
                    &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                </td>
                <td class="impNotice" style="text-align: right">
                    <asp:Literal ID="Literal8" runat="server" Text="VMEP Confirm" meta:resourcekey="Literal8Resource1"></asp:Literal>
                </td>
                <td style="width: 20%">
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 20%; white-space: nowrap; text-align: right">
                    <asp:Literal ID="Literal3" runat="server" Text="Color:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label SkinID="TextField" ID="LblColor" runat="server" meta:resourcekey="LblColorResource1"></asp:Label>
                </td>
                <td>
                </td>
                <td align="right" style="width: 20%; white-space: nowrap; text-align: right">
                    <asp:Literal ID="Literal9" runat="server" Text="Status" meta:resourcekey="Literal9Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label SkinID="TextField" ID="lblReturnStatus" runat="server" meta:resourcekey="Label1Resource1"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 20%; white-space: nowrap; text-align: right">
                    <asp:Literal ID="Literal4" runat="server" Text="Made date:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label SkinID="TextField" ID="lblDate" runat="server" meta:resourcekey="lblDateResource1"></asp:Label>
                </td>
                <td>
                </td>
                <td align="right" style="width: 20%; white-space: nowrap; text-align: right">
                    <asp:Literal ID="Literal10" runat="server" Text="Shift number" meta:resourcekey="Literal10Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label SkinID="TextField" ID="lblReturnNumber" runat="server" meta:resourcekey="Label2Resource1"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 20%; white-space: nowrap; text-align: right">
                    <asp:Literal ID="Literal5" runat="server" Text="Current stock:" meta:resourcekey="Literal5Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label SkinID="TextField" ID="lblCurStore" runat="server" meta:resourcekey="lblCurStoreResource1"></asp:Label>
                </td>
                <td>
                </td>
                <td align="right" style="width: 20%; white-space: nowrap; text-align: right">
                    <asp:Literal ID="Literal11" runat="server" Text="VMEP's note" meta:resourcekey="Literal11Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label SkinID="TextField" ID="lblReturnNote" runat="server" meta:resourcekey="Label3Resource1"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 20%; white-space: nowrap; text-align: right">
                    <asp:Literal ID="Literal6" runat="server" Text="Status:" meta:resourcekey="Literal6Resource1"></asp:Literal>
                </td>
                <td class="valueField">
                    <asp:Label ID="lblCurStatus" runat="server" meta:resourcekey="lblCurStatusResource1"></asp:Label>
                </td>
                <td>
                </td>
                <td style="width: 20%; white-space: nowrap; text-align: right">
                </td>
                <td style="width: 20%">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 20%; white-space: nowrap; text-align: right">
                    <asp:Literal ID="Literal15" runat="server" Text="Status:" meta:resourcekey="Literal15Resource1"></asp:Literal>
                </td>
                <td class="valueField">
                    <asp:Label ID="lbimportdate" runat="server"></asp:Label>
                </td>
                <td>
                </td>
                <td style="width: 20%; white-space: nowrap; text-align: right">
                </td>
                <td style="width: 20%">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 20%; white-space: nowrap; text-align: right">
                    <asp:Literal ID="Literal14" runat="server" Text="Reason:" meta:resourcekey="Literal14Resource1"></asp:Literal>
                </td>
                <td class="valueField">
                    <asp:TextBox ID="txtReleasedate" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ValidationGroup="Save" ID="RequiredFieldValidator3" runat="server"
                        SetFocusOnError="True" ControlToValidate="txtReleasedate" ErrorMessage="Releasedate date cannot be blank!"
                        meta:resourcekey="RequiredFieldValidator1Releasedate">*</asp:RequiredFieldValidator>
                    <asp:ImageButton ID="ibReleasedate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibReleasedateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="meeReleasedate" runat="server" TargetControlID="txtReleasedate"
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                        CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceReleasedate" runat="server" TargetControlID="txtReleasedate"
                        PopupButtonID="ibReleasedate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 20%; white-space: nowrap; text-align: right">
                    <asp:Literal ID="Literal7" runat="server" Text="Vouchers:" meta:resourcekey="Literal7Resource1"></asp:Literal>
                </td>
                <td class="valueField">
                    <asp:Label ID="lblVoucher" runat="server" meta:resourcekey="lblVoucherResource1"></asp:Label>
                </td>
                <td>
                </td>
                <td style="width: 20%; white-space: nowrap; text-align: right">
                </td>
                <td style="width: 20%">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 20%; white-space: nowrap; text-align: right">
                    <asp:Literal ID="Literal12" runat="server" Text="Reason:" meta:resourcekey="Literal12Resource1"></asp:Literal>
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtReason" runat="server" Width="500px" meta:resourcekey="txtReasonResource1"
                        Rows="3" ValidationGroup="save" MaxLength="2000"></asp:TextBox>
                </td>
                <td colspan="1">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtReason"
                        CssClass="Validator" ErrorMessage="Chưa nhập l&#253; do trả xe" SetFocusOnError="True"
                        ValidationGroup="save" meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="5">
                    <asp:HiddenField ID="riID" runat="server" />
                    &nbsp; &nbsp;<asp:Button ID="btnSend" runat="server" Text="Send informations to VMEP"
                        meta:resourcekey="btnSendResource1" OnClick="btnSend_Click" ValidationGroup="Save" />
                    <asp:Button ID="btnReSend" runat="server" Text="Save and ReSend informations to VMEP"
                        meta:resourcekey="btnReSendResource1" OnClick="btnSend_Click" ValidationGroup="Save"
                        Visible="false" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel request" OnClick="btnCancel_Click"
                        meta:resourcekey="btnCancelResource1" Visible="false" />
                    <asp:Button ID="btdelete" runat="server" Text="Delete" meta:resourcekey="btnDeleteResource1"
                        OnClick="btndelete_Click" Visible="false" />
                    <asp:Button ID="btnReject" runat="server" Text="Dealer confirm return" meta:resourcekey="btnRejectResource1"
                        ValidationGroup="save" OnClick="btnReject_Click" />
                </td>
                <td align="right" colspan="1">
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
