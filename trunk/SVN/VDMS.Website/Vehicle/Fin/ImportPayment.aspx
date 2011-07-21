<%@ Page Title="Import bank payments from excel" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="ImportPayment.aspx.cs" Inherits="Vehicle_Fin_ImportPayment"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div id="_msg" runat="server" />
    <div class="form" style="width: 320px; float: left; margin-right: 4px;">
        <table>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="Bank:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:BankList runat="server" ID="ddlBank" meta:resourcekey="ddlBankResource1" ShowEmptyItem="False">
                    </vdms:BankList>
                    <asp:Button ID="btnClear" runat="server" Text="Clear data" OnClick="btnClear_Click"
                        meta:resourcekey="btnClearResource1" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="Select file(*.xls):" meta:resourcekey="Literal3Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:FileUpload Width="100%" ID="file" runat="server" meta:resourcekey="fileResource1" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click"
                        meta:resourcekey="btnUploadResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div class="form" style="float: left">
        <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="AddNew" runat="server"
            meta:resourcekey="ValidationSummary1Resource1" />
        <table>
            <tr>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="Dealer code:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtDealer" MaxLength="30" runat="server" meta:resourcekey="txtDealerResource1"></asp:TextBox>
                    <asp:CustomValidator ID="cvDealer" runat="server" ControlToValidate="txtDealer" ErrorMessage="Invalid Dealer code!"
                        OnServerValidate="cvDealer_ServerValidate" ValidationGroup="AddNew" meta:resourcekey="cvDealerResource1">*</asp:CustomValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDealer"
                        ErrorMessage="Dealer code cannot be blank!" ValidationGroup="AddNew" meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:Literal ID="Literal5" runat="server" Text="Bank:" meta:resourcekey="Literal5Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:BankList runat="server" ID="ddlNewBank" meta:resourcekey="ddlNewBankResource1"
                        ShowEmptyItem="False">
                    </vdms:BankList>
                </td>
                <td>
                    <asp:Literal ID="Literal10" runat="server" Text="Date:" meta:resourcekey="Literal10Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtDate" runat="server" meta:resourcekey="txtDateResource1"></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="txtDate_MaskedEditExtender" runat="server" MaskType="Date"
                        Mask="99/99/9999" TargetControlID="txtDate" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                        CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender PopupButtonID="imbC2" ID="txtDate_CalendarExtender"
                        runat="server" Enabled="True" TargetControlID="txtDate">
                    </ajaxToolkit:CalendarExtender>
                    <asp:ImageButton runat="server" SkinID="CalendarImageButton" ID="imbC2" meta:resourcekey="imbC2Resource1" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDate"
                        ErrorMessage="Payment date cannot be blank!" ValidationGroup="AddNew" meta:resourcekey="RequiredFieldValidator4Resource1">*</asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rvDate" runat="server" ControlToValidate="txtDate" ErrorMessage="Invalid Payment date!"
                        Type="Date" ValidationGroup="AddNew" meta:resourcekey="rvDateResource1">*</asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal6" runat="server" Text="Order number:" meta:resourcekey="Literal6Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtOrderNum" MaxLength="30" runat="server" meta:resourcekey="txtOrderNumResource1"></asp:TextBox>
                    <asp:CustomValidator ID="cvOrderNum" runat="server" ControlToValidate="txtOrderNum"
                        ErrorMessage="Invalid Order number!" OnServerValidate="cvOrderNum_ServerValidate"
                        ValidationGroup="AddNew" meta:resourcekey="cvOrderNumResource1">*</asp:CustomValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtOrderNum"
                        ErrorMessage="Order number cannot be blank!" ValidationGroup="AddNew" meta:resourcekey="RequiredFieldValidator2Resource1">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:Literal ID="Literal7" runat="server" Text="Trans code:" meta:resourcekey="Literal7Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtTrans" MaxLength="70" runat="server" meta:resourcekey="txtTransResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtTrans"
                        ErrorMessage="Transaction code cannot be blank!" ValidationGroup="AddNew" meta:resourcekey="RequiredFieldValidator5Resource1">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:Literal ID="Literal11" runat="server" Text="Amount:" meta:resourcekey="Literal11Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtAmount" runat="server" meta:resourcekey="txtAmountResource1"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="TextBox3_FilteredTextBoxExtender" FilterType="Numbers"
                        runat="server" Enabled="True" TargetControlID="txtAmount">
                    </ajaxToolkit:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAmount"
                        ErrorMessage="Payment amount cannot be blank!" ValidationGroup="AddNew" meta:resourcekey="RequiredFieldValidator3Resource1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal9" runat="server" Text="Description:" meta:resourcekey="Literal9Resource1"></asp:Literal>
                </td>
                <td colspan="5">
                    <asp:TextBox ID="txtDesc" MaxLength="512" Width="100%" runat="server" meta:resourcekey="txtDescResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="5">
                    <asp:Button ID="btnAdd" runat="server" Text="Add new payment" OnClick="btnAdd_Click"
                        ValidationGroup="AddNew" meta:resourcekey="btnAddResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div id="_error" runat="server" style="clear: both" />
    <br />
    <div class="grid">
        <vdms:PageGridView PageSize="50" ID="gvPayment" runat="server" AutoGenerateColumns="False"
            DataSourceID="odsIP" DataKeyNames="Id" OnRowDataBound="gvPayment_RowDataBound"
            OnDataBound="gvPayment_DataBound" meta:resourcekey="gvPaymentResource1">
            <Columns>
                <asp:BoundField DataField="Index" HeaderText="Row" meta:resourcekey="BoundFieldResource1">
                    <ItemStyle CssClass="center" />
                </asp:BoundField>
                <asp:BoundField DataField="DealerCode" HeaderText="DealerCode" SortExpression="DealerCode"
                    meta:resourcekey="BoundFieldResource2" />
                <asp:BoundField DataField="DealerName" HeaderText="DealerName" SortExpression="DealerName"
                    meta:resourcekey="BoundFieldResource3" />
                <asp:BoundField DataField="PaymentDate" HeaderText="PaymentDate" DataFormatString="{0:d}"
                    SortExpression="PaymentDate" meta:resourcekey="BoundFieldResource4" />
                <asp:BoundField DataField="OrderNumber" HeaderText="OrderNumber" SortExpression="OrderNumber"
                    meta:resourcekey="BoundFieldResource5" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"
                    meta:resourcekey="BoundFieldResource6" />
                <asp:BoundField DataField="ToBank" HeaderText="Bank code" SortExpression="ToBank"
                    meta:resourcekey="BoundFieldResource7" />
                <asp:BoundField DataField="OrderHeaderId" HeaderText="VMDS Order Number" SortExpression="ToBank" />
                <asp:BoundField DataField="VoucherNumber" HeaderText="Transaction" SortExpression="VoucherNumber"
                    meta:resourcekey="BoundFieldResource8" />
                <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:N0}"
                    SortExpression="Amount" ItemStyle-CssClass="number" meta:resourcekey="BoundFieldResource9">
                    <ItemStyle CssClass="number"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Error" HeaderText="Error" SortExpression="Error" meta:resourcekey="BoundFieldResource10" />
                <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Images/Delete.gif" ShowDeleteButton="True"
                    meta:resourcekey="CommandFieldResource1" />
            </Columns>
        </vdms:PageGridView>
        <asp:ObjectDataSource ID="odsIP" runat="server" DataObjectTypeName="VDMS.I.Entity.SaleOrderPayment"
            DeleteMethod="DeleteImportingPayment" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetImportingItems" TypeName="VDMS.I.Vehicle.PaymentManager"></asp:ObjectDataSource>
    </div>
    <br />
    <div class="form">
        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" meta:resourcekey="btnSaveResource1" />
        <asp:Button ID="btnClearAll" runat="server" Text="Clear all data" OnClick="btnClearAll_Click"
            meta:resourcekey="btnClearAllResource1" />
    </div>
</asp:Content>
