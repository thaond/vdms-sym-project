<%@ Page Title="Order payment" Language="C#" MasterPageFile="~/MP/Popup.master" AutoEventWireup="true"
    CodeFile="OrderPayment.aspx.cs" Inherits="Bonus_Dealer_OrderPayment" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Save"
                    meta:resourcekey="ValidationSummary1Resource1" />
                <table>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal1" runat="server" Text="From bank:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                        </td>
                        <td>
                            <vdms:BankList runat="server" ID="dlFromBank" ByCurrentDealer="true" OnSelectedIndexChanged="dlFromBank_SelectedIndexChanged"
                                meta:resourcekey="dlFromBankResource1" ShowEmptyItem="False" 
                                AutoPostBack="True" ondatabound="dlFromBank_SelectedIndexChanged">
                            </vdms:BankList>
                        </td>
                        <td>
                            <asp:Literal ID="Literal2" runat="server" Text="To bank:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                        </td>
                        <td>
                            <vdms:BankList runat="server" ID="dlToBank" DealerCode="" OnSelectedIndexChanged="dlToBank_SelectedIndexChanged"
                                meta:resourcekey="dlToBankResource1" ShowEmptyItem="False" 
                                AutoPostBack="True" ondatabound="dlToBank_SelectedIndexChanged">
                            </vdms:BankList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal3" runat="server" Text="Account:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                        </td>
                        <td>
                            <vdms:BankAccountList runat="server" ID="dlFromAcc" ByCurrentDealer="true" OnSelectedIndexChanged="dlFromAcc_SelectedIndexChanged"
                                meta:resourcekey="dlFromAccResource1" ShowEmptyItem="False" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="dlFromAcc"
                                ErrorMessage="Source account cannot be blank!" ValidationGroup="Save" meta:resourcekey="RequiredFieldValidator3Resource1">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Literal ID="Literal4" runat="server" Text="Account:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                        </td>
                        <td>
                            <vdms:BankAccountList runat="server" ID="dlToAcc" DealerCode="" OnSelectedIndexChanged="dlToAcc_SelectedIndexChanged"
                                meta:resourcekey="dlToAccResource1" ShowEmptyItem="False" />
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="dlFromAcc"
                                ControlToValidate="dlToAcc" ErrorMessage="Receiving account cannot be same as source account!"
                                Operator="NotEqual" ValidationGroup="Save" meta:resourcekey="CompareValidator1Resource1">*</asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="dlToAcc"
                                ErrorMessage="Receiving account cannot be blank!" ValidationGroup="Save" meta:resourcekey="RequiredFieldValidator4Resource1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal5" runat="server" Text="Account holder:" meta:resourcekey="Literal5Resource1"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromAcc" runat="server" ReadOnly="True" meta:resourcekey="txtFromAccResource1"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="Literal6" runat="server" Text="Account holder:" meta:resourcekey="Literal6Resource1"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToAcc" runat="server" ReadOnly="True" meta:resourcekey="txtToAccResource1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal7" runat="server" Text="Voucher number:" meta:resourcekey="Literal7Resource1"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVoucher" runat="server" meta:resourcekey="txtVoucherResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtVoucher"
                                ErrorMessage="Voucher number cannot be blank!" ValidationGroup="Save" meta:resourcekey="RequiredFieldValidator2Resource1">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Literal ID="Literal8" runat="server" Text="Amount:" meta:resourcekey="Literal8Resource1"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="number" meta:resourcekey="txtAmountResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAmount"
                                ErrorMessage="Payment amount cannot be blank!" ValidationGroup="Save" meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                            <ajaxToolkit:FilteredTextBoxExtender ID="txtAmount_FilteredTextBoxExtender" FilterType="Numbers"
                                runat="server" Enabled="True" TargetControlID="txtAmount">
                            </ajaxToolkit:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal9" runat="server" Text="Comment:" meta:resourcekey="Literal9Resource1"></asp:Literal>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtComment" Width="100%" TextMode="MultiLine" Rows="3" runat="server"
                                meta:resourcekey="txtCommentResource1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="3">
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="Save"
                                meta:resourcekey="btnSaveResource1" />
                            <asp:Button ID="btnCancel" Visible="False" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                meta:resourcekey="btnCancelResource1" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
