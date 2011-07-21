<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="Money.aspx.cs" Inherits="Sales_Sale_Money" Title="Sửa đổi dữ liệu thu tiền khách hàng"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">

    <script language="javascript" type="text/javascript">
        function DelMessage() {
            return confirm('<%= DeleteAlert %>');
        }
    </script>

    <input type="hidden" id="_CurrentCustomer" value="-1" runat="server" enableviewstate="true" />
    <input type="hidden" id="_EngineNo" value="-1" runat="server" enableviewstate="true" />
    <input type="hidden" id="_PageStatus" value="-1" runat="server" enableviewstate="true" />
    <asp:Localize ID="loPageDesc" runat="server" Text="Sửa đổi dữ liệu thu tiền khách hàng"
        meta:resourcekey="loPageDescResource1"></asp:Localize>
    <div class="form">
        <table cellspacing="2" border="0" style="width: 100%" cellpadding="2">
            <tr>
                <td colspan="4" nowrap="nowrap" valign="top">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Apply"
                        Width="100%" DisplayMode="List" meta:resourcekey="ValidationSummary1Resource1" />
                    <asp:Label ID="lbErr" runat="server" ForeColor="Red" meta:resourcekey="lbErrResource1"></asp:Label><span
                        style="color: #ff0000"></span>
                </td>
            </tr>
            <tr>
                <td nowrap>
                </td>
                <td nowrap>
                </td>
                <td nowrap="nowrap">
                </td>
                <td nowrap="nowrap">
                </td>
            </tr>
            <tr>
                <td nowrap style="width: 23%">
                    <asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
                    <asp:Literal ID="Literal2" runat="server" Text="Số CMND:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td nowrap style="width: 27%">
                    <asp:TextBox ID="txtCustomerID" runat="server" MaxLength="20" meta:resourcekey="txtEngineNoResource1"></asp:TextBox>
                    <%--<asp:RegularExpressionValidator
                        ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCustomerID"
                        ErrorMessage="Số CMND của khách hàng phải gõ bằng số!" meta:resourcekey="RangeValidator1Resource1"
                        Text="*" ValidationExpression="\s*[0-9]\d*\s*" ValidationGroup="Apply"></asp:RegularExpressionValidator>--%>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;<span></span>
                    <asp:Literal ID="Literal3" runat="server" Text="Hoặc" meta:resourcekey="Literal3Resource1"></asp:Literal>
                </td>
                <td nowrap="nowrap" style="width: 25%">
                    <asp:Image ID="image2" runat="server" SkinID="RequireField" meta:resourcekey="image2Resource1" />
                    <asp:Literal ID="Literal4" runat="server" Text="Số động cơ:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                </td>
                <td nowrap="nowrap">
                    <asp:TextBox ID="txtEngineNo" runat="server" MaxLength="20" meta:resourcekey="txtEngineNoResource1"
                        Style="text-transform: uppercase"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap">
                </td>
                <td class="lblClass" nowrap="nowrap">
                </td>
                <td class="lblClass" nowrap="nowrap">
                </td>
                <td class="lblClass" nowrap="nowrap">
                </td>
            </tr>
            <tr>
                <td nowrap>
                </td>
                <td nowrap>
                </td>
                <td nowrap="nowrap">
                </td>
                <td nowrap="nowrap">
                </td>
            </tr>
            <tr>
                <td nowrap>
                </td>
                <td nowrap align="right">
                    &nbsp;
                </td>
                <td align="right" nowrap="nowrap">
                </td>
                <td align="center" nowrap="nowrap">
                    <asp:Button ID="btnTest" runat="server" Text="Kiểm tra" OnClick="btnTest_Click" meta:resourcekey="btnTestResource1" />
                </td>
            </tr>
        </table>
    </div>
    <asp:Panel ID="Panel1" runat="server" CssClass="form" Visible="False"
        meta:resourcekey="Panel1Resource1">
        <table style="width: 90%" border="0" cellpadding="2" cellspacing="2">
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Ng&#224;y thu tiền:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDateMoney" runat="server" meta:resourcekey="ddlDateMoneyResource1">
                    </asp:DropDownList>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal5" runat="server" Text="T&#234;n đại l&#253;:" meta:resourcekey="Literal5Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="lblAgentName" runat="server" CssClass="readOnlyInputField" meta:resourcekey="txtFHPAllMoneyResource1"
                        ReadOnly="True"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="Literal11" runat="server" Text="Số h&#243;a đơn:" meta:resourcekey="Literal11Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="lblBillNo" runat="server" CssClass="readOnlyInputField" meta:resourcekey="txtFHPAllMoneyResource1"
                        ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal6" runat="server" Text="Kh&#225;ch h&#224;ng:" meta:resourcekey="Literal6Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="lblCustomer" runat="server" CssClass="readOnlyInputField" meta:resourcekey="txtFHPAllMoneyResource1"
                        ReadOnly="True"></asp:TextBox>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal7" runat="server" Text="Số tiền b&#225;n h&#224;ng:" meta:resourcekey="Literal7Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="lblMoneySale" runat="server" CssClass="readOnlyInputField" meta:resourcekey="txtFHPAllMoneyResource1"
                        ReadOnly="True"></asp:TextBox>
                    (VND)
                </td>
                <td>
                    <asp:Literal ID="Literal12" runat="server" Text="Số tiền thặng dư cần thu:" meta:resourcekey="Literal12Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="lblSurplusMoney" runat="server" CssClass="readOnlyInputField" meta:resourcekey="txtFHPAllMoneyResource1"
                        ReadOnly="True"></asp:TextBox>
                    (VND)
                </td>
            </tr>
            <tr>
               <td>
                    <asp:Literal ID="Literal8" runat="server" Text="Số tiền thu lần n&#224;y:" meta:resourcekey="Literal8Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtCurrentMoney" runat="server" ReadOnly="True" MaxLength="15" meta:resourcekey="txtCurrentMoneyResource1"></asp:TextBox>
                    (VND)
                </td>
                <td>
                    <asp:RadioButton ID="tbtMCash" runat="server" Text="Tiền mặt" GroupName="GroupTransferMoneyType"
                        Checked="True" meta:resourcekey="tbtMCashResource1" OnClick="PaymentMethod(false);" />
                    <asp:RadioButton ID="tbtnMTransfer" runat="server" GroupName="GroupTransferMoneyType"
                        Text="Chuyển tiền" meta:resourcekey="tbtnMTransferResource1" OnClick="PaymentMethod(true);" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal9" runat="server" Text="Ng&#224;y chuyển tiền:" meta:resourcekey="Literal9Resource1"></asp:Literal>
                </td>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                            </td>
                            <td width="5" nowrap="nowrap">
                                <asp:TextBox ID="txtTransferDate" runat="server" meta:resourcekey="txtTransferDateResource1"></asp:TextBox>
                            </td>
                            <td nowrap="nowrap" style="width: 5px">
                                <asp:RangeValidator ID="DateRange" runat="server" ControlToValidate="txtTransferDate"
                                    Display="Dynamic" ErrorMessage="Định dạng ngày không đúng!" SetFocusOnError="True"
                                    Type="Date" ValidationGroup="Apply">*</asp:RangeValidator>
                            </td>
                            <td>
                                <asp:ImageButton ID="ImageButton2" runat="server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                    meta:resourcekey="ImageButton2Resource1" />
                            </td>
                        </tr>
                    </table>
                    <ajaxToolkit:MaskedEditExtender ID="MEE1" runat="server" Mask="99/99/9999" MaskType="Date"
                        TargetControlID="txtTransferDate" BehaviorID="MEE1" CultureAMPMPlaceholder="AM;PM"
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                        CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                        Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CE" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImageButton2"
                        TargetControlID="txtTransferDate" BehaviorID="CE" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td>
                    <asp:Literal ID="Literal10" runat="server" Text="Ng&#226;n h&#224;ng/Số t&#224;i khoản:"
                        meta:resourcekey="Literal10Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtBankAcc" runat="server" MaxLength="13" meta:resourcekey="txtBankAccResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal13" runat="server" Text="Ch&#250; th&#237;ch:" meta:resourcekey="Literal13Resource1"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtComment" runat="server" Width="95%" MaxLength="511" meta:resourcekey="txtCommentResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td align="center">
                    <asp:Button ID="btnApply" runat="server" Text="X&#225;c nhận" OnClick="btnApply_Click"
                        ValidationGroup="Apply" meta:resourcekey="btnApplyResource1" />
                </td>
            </tr>
        </table>
        <br />
        <table border="0" cellpadding="2" cellspacing="2" style="width: 90%">
            <tr>
                <td colspan="4">
                <div class="grid">
                    <vdms:PageGridView AllowPaging="false" ID="gvMoneyHistory" runat="server" AutoGenerateColumns="False" Width="100%"
                        OnDataBound="gvMoneyHistory_DataBound" OnRowEditing="gvMoneyHistory_RowEditing"
                        meta:resourcekey="gvMoneyHistoryResource1">
                        <Columns>
                            <asp:TemplateField HeaderText="STT" meta:resourcekey="TemplateFieldResource1">
                                <ItemTemplate>
                                    <asp:Literal ID="liIndex" runat="server" meta:resourcekey="liIndexResource1"></asp:Literal><asp:Literal
                                        ID="liPaymentID" runat="server" Text='<%# Bind("Id") %>' Visible="False"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ng&#224;y thu tiền" meta:resourcekey="TemplateFieldResource2">
                                <ItemTemplate>
                                    <asp:Literal ID="liPayDate" runat="server" Text='<%# Bind("Paymentdate") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Khoản thu(VND)" meta:resourcekey="TemplateFieldResource3">
                                <ItemTemplate>
                                    <asp:Literal ID="liPaidMoney" runat="server" Text='<%# Bind("Amount") %>'></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phương thức thu tiền" meta:resourcekey="TemplateFieldResource4">
                                <ItemTemplate>
                                    <asp:Literal ID="liPayMethod" runat="server" Text='<%# Bind("Status") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Số tiền thặng dư phải thu lũy kế" meta:resourcekey="TemplateFieldResource5">
                                <ItemTemplate>
                                    <asp:Literal ID="liRestMoney" runat="server" Text="-1" meta:resourcekey="liRestMoneyResource1"></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Ch&#250; th&#237;ch" DataField="Commentpayment" meta:resourcekey="BoundFieldResource1" />
                            <asp:TemplateField HeaderText="C&#244;ng năng" ShowHeader="False" meta:resourcekey="Action">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                        ImageUrl="~/Images/Edit.gif" Text="Edit" meta:resourcekey="imgbDeleteResource1" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle HorizontalAlign="Center" />
                    </vdms:PageGridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td align="center">
                    <asp:Button ID="btnDel" runat="server" Text="X&#243;a" OnClick="btnDel_Click" meta:resourcekey="btnDelResource1"
                        Visible="False" OnClientClick="return DelMessage();" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <script language="javascript">
        var obj = document.getElementById('ctl00_cphMain_ImageButton2');
        var obj1 = document.getElementById('ctl00_cphMain_txtTransferDate');
        var obj2 = document.getElementById('ctl00_cphMain_txtBankAcc');
        PrepareDisplay();

        function PrepareDisplay() {
            if (obj != null && obj1 != null && obj2 != null) {
                obj.disabled = true; obj1.disabled = "disabled"; obj2.disabled = "disabled";
            } else return;
        }
        function PaymentMethod(isTransfer) {
            if (!isTransfer) {
                obj.disabled = true; obj1.disabled = "disabled"; obj2.disabled = "disabled";
            }
            else {
                obj.disabled = false; obj1.disabled = null; obj2.disabled = null;
            }
        }
    </script>

</asp:Content>
