<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="Receipt.aspx.cs" Inherits="Sales_Sale_Receipt" Title="Kiểm tra khoản cần thu"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
            <tr>
                <td nowrap="nowrap">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Search"
                        Width="100%" meta:resourcekey="ValidationSummary1Resource1" />
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap">
                    <asp:Label ID="lbErr" runat="server" meta:resourcekey="lbErrResource1"></asp:Label>
                </td>
            </tr>
        </table>
        <table cellspacing="0" border="0">
            <tr>
                <td valign="top" nowrap="nowrap">
                    <asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
                    <asp:Literal ID="Literal1" runat="server" meta:resourcekey="Literal1Resource1" Text="Ngày cần thu:"></asp:Literal>
                </td>
                <td valign="top" nowrap="nowrap">
                    <asp:TextBox ID="txtReceiptDate" runat="server" Width="88px" meta:resourcekey="txtReceiptDateResource1"></asp:TextBox><asp:RequiredFieldValidator
                        ID="Requiredfieldvalidator9" runat="server" SetFocusOnError="True" ValidationGroup="Search"
                        ControlToValidate="txtReceiptDate" ErrorMessage='Dữ liệu "Ngày cần thu" không được để trống!'
                        meta:resourcekey="Requiredfieldvalidator9Resource1" Text="*"></asp:RequiredFieldValidator><asp:RangeValidator
                            ID="rvReceiptDate" runat="server" ControlToValidate="txtReceiptDate" Display="Dynamic"
                            ErrorMessage="Định dạng ngày không đúng!" SetFocusOnError="True" Type="Date"
                            ValidationGroup="Search">*</asp:RangeValidator>
                    <asp:ImageButton ID="ImageButton1" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ImageButton1Resource1" /><%--<asp:RegularExpressionValidator ID="Regularexpressionvalidator1" runat="server" EnableClientScript="true"
					SetFocusOnError="true" ValidationGroup="Search" ValidationExpression="^((0[1-9])|(1[0-9])|(2[0-9])|(3[0-1]))/(([1-9])|(0[1-9])|(1[0-2]))/((\d{4}))$"
					ControlToValidate="txtReceiptDate" ErrorMessage='Dữ liệu "Ngày cần thu" không hợp lệ (yêu cầu nhập theo định dạng dd/MM/yyyy)'>*</asp:RegularExpressionValidator>--%><ajaxToolkit:MaskedEditExtender
                        ID="MaskedEditExtender2" runat="server" TargetControlID="txtReceiptDate" Mask="99/99/9999"
                        MaskType="Date" BehaviorID="MaskedEditExtender2" CultureAMPMPlaceholder="AM;PM"
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                        CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                        Enabled="True" />
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtReceiptDate"
                        Format="dd/MM/yyyy" PopupButtonID="ImageButton1" BehaviorID="CalendarExtender3"
                        Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td valign="top" nowrap="nowrap">
                    <asp:Image ID="image1" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image1Resource1" />
                    <asp:Literal ID="Literal2" runat="server" meta:resourcekey="Literal2Resource1" Text="Số CMND:"></asp:Literal>
                </td>
                <td valign="top" nowrap="nowrap">
                    <asp:TextBox ID="txtIdentity" runat="server" Width="188px" meta:resourcekey="txtIdentityResource1"
                        MaxLength="10"></asp:TextBox>
                    <%--<asp:RegularExpressionValidator
                                    ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtIdentity"
                                    ErrorMessage="Giá tiền phải được gõ bằng số!" ValidationExpression="\s*[1-9]\d*\s*"
                                    ValidationGroup="CheckInsertOrUpdate" meta:resourcekey="RegularExpressionValidator2Resource1">*</asp:RegularExpressionValidator>--%>
                </td>
            </tr>
            <tr>
                <td valign="top" nowrap="nowrap">
                    <asp:Image ID="image2" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image2Resource1" />
                    <asp:Literal ID="Literal3" runat="server" meta:resourcekey="Literal3Resource1" Text="Phương thức thanh toán: "></asp:Literal>
                </td>
                <td valign="top" nowrap="nowrap">
                    <asp:DropDownList ID="ddlPayMethod" runat="server" Width="192px" Enabled="False"
                        meta:resourcekey="ddlPayMethodResource1">
                        <asp:ListItem Value="0" meta:resourcekey="ListItemResource1" Text="ALL"></asp:ListItem>
                        <asp:ListItem Value="1" Selected="True" meta:resourcekey="ListItemResource2" Text="Trả g&#243;p"></asp:ListItem>
                        <asp:ListItem Value="2" meta:resourcekey="ListItemResource3" Text="Trả hết một lần"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" nowrap="nowrap">
                </td>
                <td valign="top" nowrap="nowrap" colspan="2">
                    <asp:Button ID="btnTest" runat="server" Text="Kiểm tra lũy kế khoản phải thu" ValidationGroup="Search"
                        OnClick="btnTest_Click" meta:resourcekey="btnTestResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
            <tr>
                <td nowrap="nowrap">
                    <vdms:PageGridView ID="gvItems" runat="server" AutoGenerateColumns="False" CssClass="GridView"
                        Width="100%" meta:resourcekey="gvItemsResource1" OnDataBound="gvItems_DataBound"
                        AllowPaging="True">
                        <Columns>
                            <asp:BoundField HeaderText="STT" meta:resourcekey="BoundFieldResource1" DataField="indexrow">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Số động cơ" meta:resourcekey="BoundFieldResource2" DataField="enginenumber">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="fullname" HeaderText="Đối tượng thu" meta:resourcekey="BoundFieldResource10">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Số CMND" meta:resourcekey="BoundFieldResource3" DataField="identifynumber">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Ng&#224;y cần thu" meta:resourcekey="TemplateFieldResource1">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Literal ID="liPaymentdate" runat="server" Text='<%# EvalDate(Eval("paymentdate")) %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ng&#224;y khoản thu lần trước" meta:resourcekey="TemplateFieldResource2">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Literal ID="liPrevousDate" runat="server" Text='<%# Bind("prevousdate") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Số tiền b&#225;n(VND)" meta:resourcekey="TemplateFieldResource3">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Literal ID="liPriceBeforeTax" runat="server" Text='<%# Bind("pricebeforetax") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Khoản thu lũy kế(VND)" meta:resourcekey="TemplateFieldResource4">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Literal ID="liRecOfMoney" runat="server" Text='<%# Bind("recofmoney") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Khoản thặng dư chưa thu(VND)" meta:resourcekey="TemplateFieldResource5">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Literal ID="liRestOfMoney" runat="server" Text='<%# Bind("restofmoney") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phương thức thanh to&#225;n" meta:resourcekey="TemplateFieldResource6">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Literal ID="liHirePurchase" runat="server" Text='<%# Bind("TRAGOP") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <%--<PagerSettings Mode="NextPreviousFirstLast" Position="Top" />
                    <PagerTemplate>
                        <div style="float: left;">
                            <asp:Literal ID="litPageInfo" runat="server"></asp:Literal></div>
                        <div style="text-align: right; float: right;">
                            <asp:Button ID="cmdFirst" runat="server" Text="First" CommandName="Page" CommandArgument="First"
                                meta:resourcekey="cmdFirstResource1" OnClick="cmdFirst_Click" />
                            <asp:Button ID="cmdPrevious" runat="server" Text="Previous" CommandName="Page" CommandArgument="Prev"
                                meta:resourcekey="cmdPreviousResource1" OnClick="cmdPrevious_Click" />
                            <asp:Button ID="cmdNext" runat="server" Text="Next" CommandName="Page" CommandArgument="Next"
                                meta:resourcekey="cmdNextResource1" OnClick="cmdNext_Click" />
                            <asp:Button ID="cmdLast" runat="server" Text="Last" CommandName="Page" CommandArgument="Last"
                                meta:resourcekey="cmdLastResource1" OnClick="cmdLast_Click" />
                        </div>
                    </PagerTemplate>--%>
                    </vdms:PageGridView>
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap">
                    <asp:Label ID="lbNote1" Visible="false" runat="server" ForeColor="Red" Text="＊Liệt kê ngày cần thu (gồm) khoản thặng dư cần thu trước đây chưa có dữ liệu (>0)"
                        meta:resourcekey="lbNote1Resource1"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" nowrap="nowrap">
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap">
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
