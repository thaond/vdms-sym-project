<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="Store.aspx.cs" Inherits="Vehicle_Sale_Store" Title="Bán hàng theo lô cho cửa hàng phụ"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <asp:Localize ID="loPageDesc" runat="server" Text="Sell vehicle to Sub-store" meta:resourcekey="loPageDescResource1"></asp:Localize><br />
    <br />
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
        <tr>
            <td>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td align="left" colspan="4">
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List"
                                ValidationGroup="PlusEngineNO" Width="100%" meta:resourcekey="ValidationSummary2Resource1" />
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                                ValidationGroup="InsertOrUpdate" Width="100%" meta:resourcekey="ValidationSummary2Resource1" />
                            <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="List"
                                ValidationGroup="CheckInsertOrUpdate" Width="100%" meta:resourcekey="ValidationSummary2Resource1" />
                            <asp:Label ID="lbErr" runat="server" meta:resourcekey="lbErrResource1"></asp:Label><span
                                style="color: #ff0000"></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25%">
                            <asp:Literal ID="liMotorTypeCode" runat="server" Text="Model code:" meta:resourcekey="liMotorTypeCodeResource1"></asp:Literal>
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtMotorCode" runat="server" MaxLength="10" meta:resourcekey="txtMotorCodeResource1"
                                Style="text-transform: uppercase"></asp:TextBox>
                        </td>
                        <td class="style1">
                            <asp:Literal ID="liStore" runat="server" Text="Store:" meta:resourcekey="liStoreResource1"></asp:Literal>
                        </td>
                        <td>
                            <vdms:WarehouseList ID="ddlStoreCode" Type="V" runat="server">
                            </vdms:WarehouseList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25%">
                            <asp:Literal ID="liColorCode" runat="server" Text="Color code:" meta:resourcekey="liColorCodeResource1"></asp:Literal>
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtColorCode" runat="server" MaxLength="10" meta:resourcekey="txtColorCodeResource1"
                                Style="text-transform: uppercase"></asp:TextBox>
                        </td>
                        <td class="style1">
                            <asp:Button ID="btnTest" runat="server" Text="Search" OnClick="btnTest_Click" meta:resourcekey="btnTestResource1" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25%">
                            <asp:Literal ID="LiEngineNotitle" runat="server" Text="Engine No:" meta:resourcekey="LiEngineNotitleResource1"></asp:Literal>
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtEngineNo" runat="server" MaxLength="20" meta:resourcekey="txtEngineNoResource1"
                                Style="text-transform: uppercase"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator4"
                                    runat="server" ErrorMessage="Engine No to be plused cannot blank!" ValidationGroup="PlusEngineNO"
                                    ControlToValidate="txtEngineNo" meta:resourcekey="RequiredFieldValidator4Resource1">*</asp:RequiredFieldValidator>
                        </td>
                        <td class="style1">
                            <asp:Button ID="btnPlusEngine" runat="server" Text="Input" OnClick="btnPlusEngine_Click"
                                ValidationGroup="PlusEngineNO" meta:resourcekey="btnPlusEngineResource1" />
                        </td>
                        <td align="center">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="liSelectItemIns" runat="server" Visible="False" meta:resourcekey="liSelectItemInsResource1"></asp:Literal>
                <div class="grid">
                
                    <vdms:PageGridView ID="gvItems" runat="server" AutoGenerateColumns="False" Width="100%"
                        AllowPaging="True" OnDataBound="gvItems_DataBound" OnPageIndexChanging="gvItems_PageIndexChanging"
                        CssClass="GridView" meta:resourcekey="gvItemsResource1">
                        <Columns>
                            <asp:TemplateField HeaderText="Select" meta:resourcekey="TemplateFieldResource1">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbSelectItem" runat="server" meta:resourcekey="cbSelectItemResource1" />
                                    <asp:Label ID="lbItemInstanceID" runat="server" Text='<%# Bind("Id") %>' Visible="False"
                                        meta:resourcekey="lbItemInstanceIDResource1"></asp:Label>
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="AllCheckBox" runat="server" AutoPostBack="True" OnCheckedChanged="AllCheckBox_CheckedChanged"
                                        meta:resourcekey="AllCheckBoxResource1" />
                                </HeaderTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="No" meta:resourcekey="TemplateFieldResource2">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# ReturnIndex(DataBinder.Eval(Container, "RowIndex").ToString()) %>'
                                        meta:resourcekey="Label1Resource1"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Enginenumber" HeaderText="Engine No" meta:resourcekey="BoundFieldResource1" />
                            <asp:BoundField DataField="Itemtype" HeaderText="Motor Type" meta:resourcekey="BoundFieldResource2" />
                            <asp:TemplateField HeaderText="Color" meta:resourcekey="TemplateFieldResource3">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" meta:resourcekey="TextBox1Resource1" Text='<%# Bind("[Item.Colorcode]") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" meta:resourcekey="Label2Resource1" Text='<%# Eval("Item.Colorcode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Store" DataField="Dealercode" meta:resourcekey="BoundFieldResource4" />
                        </Columns>
                        <%--<PagerSettings Mode="NextPreviousFirstLast" Position="Top" />
                        <PagerTemplate>
                            <div style="float: left">
                                <asp:Literal ID="litPageInfo" runat="server" meta:resourcekey="litPageInfoResource1"></asp:Literal></div>
                            <div style="float: right; text-align: right">
                                <asp:Button ID="cmdFirst" runat="server" CommandArgument="First" CommandName="Page"
                                    meta:resourcekey="cmdFirstResource1" OnClick="cmdFirst_Click" Text="First" />
                                <asp:Button ID="cmdPrevious" runat="server" CommandArgument="Prev" CommandName="Page"
                                    meta:resourcekey="cmdPreviousResource1" OnClick="cmdPrevious_Click" Text="Previous" />
                                <asp:Button ID="cmdNext" runat="server" CommandArgument="Next" CommandName="Page"
                                    meta:resourcekey="cmdNextResource1" OnClick="cmdNext_Click" Text="Next" />
                                <asp:Button ID="cmdLast" runat="server" CommandArgument="Last" CommandName="Page"
                                    meta:resourcekey="cmdLastResource1" OnClick="cmdLast_Click" Text="Last" />
                            </div>
                        </PagerTemplate>--%>
                    </vdms:PageGridView>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td align="center" colspan="4" style="height: 5px">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4" style="font-weight: bold; font-size: 120%;">
                            <asp:Literal ID="liSellingData" runat="server" Text="Selling data" meta:resourcekey="liSellingDataResource1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25%">
                            <asp:Image ID="Image2" runat="server" SkinID="RequireField" meta:resourcekey="Image2Resource1" />
                            <asp:Literal ID="li" runat="server" Text="Selling date:" meta:resourcekey="liResource1"></asp:Literal>
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtSellingDate" runat="server" meta:resourcekey="txtSellingDateResource1"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="txtSellingDate_MaskedEditExtender" runat="server"
                                Mask="99/99/9999" MaskType="Date" TargetControlID="txtSellingDate">
                            </ajaxToolkit:MaskedEditExtender>
                            <ajaxToolkit:CalendarExtender ID="txtSellingDate_CalendarExtender" runat="server"
                                TargetControlID="txtSellingDate" PopupButtonID="imgbSellDate">
                            </ajaxToolkit:CalendarExtender>
                            <asp:RequiredFieldValidator ControlToValidate="txtSellingDate" ID="RequiredFieldValidator5"
                                runat="server" ErrorMessage="Selling date can not be blank!" Text="*" ValidationGroup="CheckInsertOrUpdate"
                                meta:resourcekey="rfvSellingDateResource1"></asp:RequiredFieldValidator>
                            <asp:RangeValidator Type="Date" ValidationGroup="CheckInsertOrUpdate" ID="rvSellingDate"
                                meta:resourcekey="rvSellingDateResource1" Text="*" ControlToValidate="txtSellingDate"
                                runat="server" ErrorMessage="RangeValidator"></asp:RangeValidator>
                            <asp:ImageButton ID="imgbSellDate" runat="server" SkinID="CalendarImageButton" />
                        </td>
                        <td style="width: 25%">
                            <asp:Image ID="Image1" runat="server" SkinID="RequireField" meta:resourcekey="Image1Resource1" />
                            <asp:Literal ID="liSellingkind" runat="server" Text="Selling object:" meta:resourcekey="liSellingkindResource1"></asp:Literal>
                        </td>
                        <td style="width: 25%">
                            <asp:DropDownList ID="ddlAgency" runat="server" Width="115px" meta:resourcekey="ddlAgencyResource1">
                                <asp:ListItem meta:resourcekey="ListItemResource2" Text="1"></asp:ListItem>
                                <asp:ListItem meta:resourcekey="ListItemResource3" Text="2"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RqvPriceTax0" meta:resourcekey="RequiredFieldValidator2Resource1x"
                                runat="server" ControlToValidate="ddlAgency" ErrorMessage="RequiredFieldValidator"
                                ValidationGroup="CheckInsertOrUpdate">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25%">
                            <%--<asp:Image ID="Image5" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image1Resource1" />--%>
                            <asp:Literal ID="Literal9" runat="server" Text="Order Number:" meta:resourcekey="Literal9Resource1"></asp:Literal>
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtBillNo" runat="server" MaxLength="30" meta:resourcekey="txtBillNoResource1"
                                Style="text-transform: uppercase"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBillNo"
                                ErrorMessage='Input "Bill No" can not be blank!' SetFocusOnError="True" ValidationGroup="InsertOrUpdate"
                                meta:resourcekey="RequiredFieldValidator1Resource1" Text="*"></asp:RequiredFieldValidator>--%>
                        </td>
                        <td style="width: 25%">
                            <asp:Literal ID="Literal13" runat="server" Text="Selling type:" meta:resourcekey="Literal13Resource1"></asp:Literal>
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtSellingType" runat="server" MaxLength="255" meta:resourcekey="txtSellingTypeResource1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25%">
                            <%--<asp:Image ID="Image4" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image1Resource1" />--%>
                            <asp:Literal ID="LiPriceinCludeTax" runat="server" Text="Price (include tax):" meta:resourcekey="LiPriceinCludeTaxResource1"></asp:Literal>
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtPriceTax" runat="server" MaxLength="28" meta:resourcekey="txtPriceTaxResource1"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="txtPriceTax_FilteredTextBoxExtender" runat="server"
                                Enabled="True" TargetControlID="txtPriceTax" FilterType="Numbers">
                            </ajaxToolkit:FilteredTextBoxExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtPriceTax"
                                ErrorMessage="Price (include tax) is not right format of currency!" ValidationExpression="\s*[1-9][0-9]*([.,]?\d*[1-9]+\d*)?\s*"
                                ValidationGroup="InsertOrUpdate" meta:resourcekey="RangeValidator1Resource1"
                                Text="*"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RqvPriceTax" meta:resourcekey="RequiredFieldValidator2Resource1"
                                runat="server" ControlToValidate="txtPriceTax" ErrorMessage="RequiredFieldValidator"
                                ValidationGroup="CheckInsertOrUpdate">*</asp:RequiredFieldValidator>
                        </td>
                        <td style="width: 25%">
                            <asp:Literal ID="Literal12" runat="server" Text="Payment method:" meta:resourcekey="Literal12Resource1"></asp:Literal>
                        </td>
                        <td style="width: 25%">
                            <asp:DropDownList ID="ddlPaymentMethod" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPaymentMethod_SelectedIndexChanged"
                                Width="132px" meta:resourcekey="ddlPaymentMethodResource1">
                                <asp:ListItem Selected="True" Value="0" meta:resourcekey="ListItemResource4" Text="Complete paying"></asp:ListItem>
                                <asp:ListItem Value="1" meta:resourcekey="ListItemResource5" Text="Fix hire-purchase"></asp:ListItem>
                                <asp:ListItem Value="2" meta:resourcekey="ListItemResource6" Text="UnFix hire-purchase"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25%" nowrap="nowrap">
                            <%--<asp:Image ID="Image3" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image1Resource1" />--%>
                            <asp:Literal ID="liRecDate" runat="server" Text="Receive Money Date:" meta:resourcekey="liRecDateResource1"></asp:Literal>
                        </td>
                        <td style="width: 25%">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRecCDate" runat="server" meta:resourcekey="txtRecCDateResource1"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="rfvRecCDate" runat="server" ControlToValidate="txtRecCDate"
                                            ErrorMessage='Input "Receive Money Date" can not be blank!' SetFocusOnError="True"
                                            ValidationGroup="InsertOrUpdate" meta:resourcekey="rfvRecCDateResource1" Text="*"></asp:RequiredFieldValidator>--%>
                                        <asp:RangeValidator ID="rvRecDate1" runat="server" ControlToValidate="txtRecCDate"
                                            ErrorMessage='Dữ liệu "Ngày cần thu" không đúng với định dạng ngày!' meta:resourcekey="rfvRecDate"
                                            SetFocusOnError="True" Type="Date" ValidationGroup="InsertOrUpdate">*</asp:RangeValidator>
                                        <asp:RangeValidator ID="rvRecDate" runat="server" ControlToValidate="txtRecCDate"
                                            ErrorMessage='Dữ liệu "Ngày cần thu" không đúng với định dạng ngày!' meta:resourcekey="rfvRecDate"
                                            SetFocusOnError="True" Type="Date" ValidationGroup="CheckInsertOrUpdate">*</asp:RangeValidator>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtRecCDate"
                                            ErrorMessage='Input "Receive Money Date" can not be blank!' SetFocusOnError="True"
                                            ValidationGroup="CheckInsertOrUpdate" meta:resourcekey="RequiredFieldValidator3Resource1"
                                            Text="*"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td style="color: #000000">
                                        <asp:ImageButton ID="ImageButton5" runat="server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                            meta:resourcekey="ImageButton5Resource1" />
                                    </td>
                                </tr>
                            </table>
                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender5" runat="server" Mask="99/99/9999"
                                MaskType="Date" TargetControlID="txtRecCDate" BehaviorID="MaskedEditExtender5"
                                CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY"
                                CultureDatePlaceholder="/" CultureDecimalPlaceholder="." CultureThousandsPlaceholder=","
                                CultureTimePlaceholder=":" Enabled="True">
                            </ajaxToolkit:MaskedEditExtender>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender5" runat="server" PopupButtonID="ImageButton5"
                                TargetControlID="txtRecCDate" BehaviorID="CalendarExtender5" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                        <td style="width: 25%">
                        </td>
                        <td style="width: 25%">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:PlaceHolder ID="FixedHP" runat="server" Visible="False">
                    <!-- Begin Fixed Hire-Purchase -->
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td align="left" colspan="4" style="font-weight: bold; font-size: 120%;">
                                <asp:Literal ID="liINSERTFixedHPurchase" runat="server" Text="Fixed hire-purchase"
                                    meta:resourcekey="liINSERTFixedHPurchaseResource2"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 25%" nowrap="nowrap">
                                <asp:Image ID="Image6" runat="server" SkinID="RequireField" meta:resourcekey="Image1Resource1" />
                                <asp:Literal ID="Literal6" runat="server" Text="Hire-purchase times:" meta:resourcekey="Literal6Resource2"></asp:Literal>
                            </td>
                            <td style="width: 25%" nowrap="nowrap">
                                <asp:TextBox ID="txtFHPTimes" runat="server" MaxLength="2" meta:resourcekey="txtFHPTimesResource2"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFHPTimes" runat="server" ControlToValidate="txtFHPTimes"
                                    ErrorMessage="Times of hire-purchase can not be blank!" ValidationGroup="CheckInsertOrUpdate"
                                    meta:resourcekey="rfvFHPTimesResource2">*</asp:RequiredFieldValidator><asp:RangeValidator
                                        ID="rvFHPPaindMoneyDate" runat="server" ControlToValidate="txtFHPTimes" ErrorMessage="Number of fixed installment times must be range 1-5!"
                                        MaximumValue="5" meta:resourcekey="rvFHPTimes" MinimumValue="1" SetFocusOnError="True"
                                        Type="Integer" ValidationGroup="CheckInsertOrUpdate">*</asp:RangeValidator>
                            </td>
                            <td align="left" style="width: 25%" nowrap="nowrap">
                                <asp:Literal ID="Literal3" runat="server" Text="Money of pay in instalment:" meta:resourcekey="Literal3Resource2"></asp:Literal>
                            </td>
                            <td nowrap="nowrap">
                                <asp:TextBox ID="lbMoneyOfTimes" runat="server" MaxLength="28" CssClass="readOnlyInputField"
                                    ReadOnly="True" meta:resourcekey="lbMoneyOfTimesResource2"></asp:TextBox>(VND)
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 25%" nowrap="nowrap">
                                <asp:Image ID="Image7" runat="server" SkinID="RequireField" meta:resourcekey="Image1Resource1" />
                                <asp:Literal ID="Literal4" runat="server" Text="First liquidate:" meta:resourcekey="Literal4Resource2"></asp:Literal>
                            </td>
                            <td style="width: 25%" nowrap="nowrap">
                                <asp:TextBox ID="txtFHPFirstMoney" runat="server" MaxLength="15" meta:resourcekey="txtFHPFirstMoneyResource2"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="FirstLiquidateFormat" runat="server" ControlToValidate="txtFHPFirstMoney"
                                    ErrorMessage="Khoản trả kỳ đầu phải được gõ bằng số hoặc lớn hơn 0!" meta:resourcekey="FirstLiquidateFormat"
                                    Text="*" ValidationExpression="\s*[1-9]\d*\s*" ValidationGroup="CheckInsertOrUpdate"></asp:RegularExpressionValidator><asp:RequiredFieldValidator
                                        ID="rfvFHPFirstliquidate" runat="server" ControlToValidate="txtFHPFirstMoney"
                                        ErrorMessage="First liquidate can not be blank!" ValidationGroup="CheckInsertOrUpdate"
                                        meta:resourcekey="rfvFHPFirstliquidateResource2">*</asp:RequiredFieldValidator>(VND)
                            </td>
                            <td align="left" style="width: 25%" nowrap="nowrap">
                                <asp:Literal ID="Literal5" runat="server" Text="Money of all hire-purchase:" meta:resourcekey="Literal5Resource2"></asp:Literal>
                            </td>
                            <td nowrap="nowrap">
                                <asp:TextBox ID="txtFHPAllMoney" runat="server" MaxLength="28" CssClass="readOnlyInputField"
                                    ReadOnly="True" meta:resourcekey="txtFHPAllMoneyResource2"></asp:TextBox>(VND)
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 25%" nowrap="nowrap">
                                <asp:Image ID="Image8" runat="server" SkinID="RequireField" meta:resourcekey="Image1Resource1" />
                                <asp:Literal ID="Literal2" runat="server" Text="Number of fixed hire-purchase's:"
                                    meta:resourcekey="Literal2Resource2"></asp:Literal>
                            </td>
                            <td style="width: 25%" nowrap="nowrap">
                                <asp:TextBox ID="txtFHPPaidMoneyDate" runat="server" MaxLength="5" meta:resourcekey="txtFHPPaidMoneyDateResource2"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="FHPDaysRangeFormat" runat="server" ControlToValidate="txtFHPPaidMoneyDate"
                                    ErrorMessage="Số ngày mỗi kỳ phải được gõ bằng số hoặc lớn hơn 0!" meta:resourcekey="FHPDaysRangeFormat"
                                    Text="*" ValidationExpression="\s*[1-9]\d*\s*" ValidationGroup="CheckInsertOrUpdate"></asp:RegularExpressionValidator><asp:RequiredFieldValidator
                                        ID="rfvFHPPaidMoneyDate" runat="server" ControlToValidate="txtFHPPaidMoneyDate"
                                        ErrorMessage="Number of fixed hire-purchase's can not be blank!" ValidationGroup="CheckInsertOrUpdate"
                                        meta:resourcekey="rfvFHPPaidMoneyDateResource2">*</asp:RequiredFieldValidator><asp:Literal
                                            ID="Literal1" runat="server" Text="(Days)" meta:resourcekey="Literal1Resource2"></asp:Literal>
                            </td>
                            <td align="left" style="width: 25%" nowrap="nowrap">
                                <asp:Literal ID="liFHPLastHPdate" runat="server" Text="Last hire-purchase date:"
                                    meta:resourcekey="liFHPLastHPdateResource2"></asp:Literal>
                            </td>
                            <td nowrap="nowrap">
                                <asp:TextBox ID="FHPPaidDateLast" runat="server" CssClass="readOnlyInputField" ReadOnly="True"
                                    meta:resourcekey="FHPPaidDateLastResource2"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <!-- End Fixed hire-purchase -->
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td>
                <asp:PlaceHolder ID="UnfixedHP" runat="server" Visible="False">
                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                        <tr>
                            <td align="left" colspan="5" style="font-weight: bold; font-size: 120%;">
                                <asp:Literal ID="Literal7" runat="server" Text="Unfixed hire-purchase" meta:resourcekey="Literal7Resource1"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="field_name" colspan="5" style="text-align: left">
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                (1)
                            </td>
                            <td align="center">
                                (2)
                            </td>
                            <td align="center">
                                (3)
                            </td>
                            <td align="center">
                                (4)
                            </td>
                            <td align="center">
                                (5)
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="5" style="text-align: left">
                                <asp:Literal ID="Literal14" runat="server" Text="Intended date for pay:" meta:resourcekey="Literal14Resource1"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtIntentDatePay1" runat="server" Width="75px" meta:resourcekey="txtIntentDatePay1Resource1"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RangeValidator ID="rvUHP1" runat="server" ControlToValidate="txtIntentDatePay1"
                                                ErrorMessage='Dữ liệu "Ngày dự tính" không đúng với định dạng ngày!' meta:resourcekey="rvUHP1"
                                                SetFocusOnError="True" Type="Date" ValidationGroup="CheckInsertOrUpdate">*</asp:RangeValidator>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgIntend1" runat="server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                                meta:resourcekey="imgIntend1Resource1" /><a href="#"></a>
                                        </td>
                                    </tr>
                                </table>
                                <ajaxToolkit:MaskedEditExtender ID="MEUHP1" runat="server" BehaviorID="MEUHP1" CultureAMPMPlaceholder="AM;PM"
                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                                    CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                                    Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtIntentDatePay1">
                                </ajaxToolkit:MaskedEditExtender>
                                <ajaxToolkit:CalendarExtender ID="CEUHP1" runat="server" BehaviorID="CEUHP1" Enabled="True"
                                    PopupButtonID="imgIntend1" TargetControlID="txtIntentDatePay1">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtIntentDatePay2" runat="server" Width="75px" meta:resourcekey="txtIntentDatePay2Resource1"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RangeValidator ID="rvUHP2" runat="server" ControlToValidate="txtIntentDatePay2"
                                                ErrorMessage='Dữ liệu "Ngày dự tính" không đúng với định dạng ngày!' meta:resourcekey="rvUHP2"
                                                SetFocusOnError="True" Type="Date" ValidationGroup="CheckInsertOrUpdate">*</asp:RangeValidator>
                                        </td>
                                        <td style="width: 29px">
                                            <asp:ImageButton ID="imgIntend2" runat="server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                                meta:resourcekey="imgIntend2Resource1" /><a href="#"></a>
                                        </td>
                                    </tr>
                                </table>
                                <ajaxToolkit:MaskedEditExtender ID="MEUHP2" runat="server" BehaviorID="MEUHP2" CultureAMPMPlaceholder="AM;PM"
                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                                    CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                                    Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtIntentDatePay2">
                                </ajaxToolkit:MaskedEditExtender>
                                <ajaxToolkit:CalendarExtender ID="CEUHP2" runat="server" BehaviorID="CEUHP2" Enabled="True"
                                    PopupButtonID="imgIntend2" TargetControlID="txtIntentDatePay2">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtIntentDatePay3" runat="server" CssClass="input_word" Width="75px"
                                                meta:resourcekey="txtIntentDatePay3Resource1"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RangeValidator ID="rvUHP3" runat="server" ControlToValidate="txtIntentDatePay3"
                                                ErrorMessage='Dữ liệu "Ngày dự tính" không đúng với định dạng ngày!' meta:resourcekey="rvUHP3"
                                                SetFocusOnError="True" Type="Date" ValidationGroup="CheckInsertOrUpdate">*</asp:RangeValidator>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgIntend3" runat="server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                                meta:resourcekey="imgIntend3Resource1" /><a href="#"></a>
                                        </td>
                                    </tr>
                                </table>
                                <ajaxToolkit:MaskedEditExtender ID="MEUHP3" runat="server" BehaviorID="MEUHP3" CultureAMPMPlaceholder="AM;PM"
                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                                    CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                                    Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtIntentDatePay3">
                                </ajaxToolkit:MaskedEditExtender>
                                <ajaxToolkit:CalendarExtender ID="CEUHP3" runat="server" BehaviorID="CEUHP3" Enabled="True"
                                    PopupButtonID="imgIntend3" TargetControlID="txtIntentDatePay3">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtIntentDatePay4" runat="server" Width="75px" meta:resourcekey="txtIntentDatePay4Resource1"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RangeValidator ID="rvUHP4" runat="server" ControlToValidate="txtIntentDatePay4"
                                                ErrorMessage='Dữ liệu "Ngày dự tính" không đúng với định dạng ngày!' meta:resourcekey="rvUHP4"
                                                SetFocusOnError="True" Type="Date" ValidationGroup="CheckInsertOrUpdate">*</asp:RangeValidator>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgIntend4" runat="server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                                meta:resourcekey="imgIntend4Resource1" /><a href="#"></a>
                                        </td>
                                    </tr>
                                </table>
                                <ajaxToolkit:MaskedEditExtender ID="MEUHP4" runat="server" BehaviorID="MEUHP4" CultureAMPMPlaceholder="AM;PM"
                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                                    CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                                    Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtIntentDatePay4">
                                </ajaxToolkit:MaskedEditExtender>
                                <ajaxToolkit:CalendarExtender ID="CEUHP4" runat="server" BehaviorID="CEUHP4" Enabled="True"
                                    PopupButtonID="imgIntend4" TargetControlID="txtIntentDatePay4">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtIntentDatePay5" runat="server" Width="75px" meta:resourcekey="txtIntentDatePay5Resource1"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RangeValidator ID="rvUHP5" runat="server" ControlToValidate="txtIntentDatePay5"
                                                ErrorMessage='Dữ liệu "Ngày dự tính" không đúng với định dạng ngày!' meta:resourcekey="rvUHP5"
                                                SetFocusOnError="True" Type="Date" ValidationGroup="CheckInsertOrUpdate">*</asp:RangeValidator>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgIntend5" runat="server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                                meta:resourcekey="imgIntend5Resource1" /><a href="#"></a>
                                        </td>
                                    </tr>
                                </table>
                                <ajaxToolkit:MaskedEditExtender ID="MEUHP5" runat="server" BehaviorID="MEUHP5" CultureAMPMPlaceholder="AM;PM"
                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                                    CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                                    Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtIntentDatePay5">
                                </ajaxToolkit:MaskedEditExtender>
                                <ajaxToolkit:CalendarExtender ID="CEUHP5" runat="server" BehaviorID="CEUHP5" Enabled="True"
                                    PopupButtonID="imgIntend5" TargetControlID="txtIntentDatePay5">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="5" style="text-align: left">
                                <asp:Literal ID="Literal8" runat="server" Text="Số tiền dự t&#237;nh sẽ nộp:" meta:resourcekey="Literal8Resource1"></asp:Literal>(VND)
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:TextBox ID="UHPtxtMoney1" runat="server" Width="85px" meta:resourcekey="UHPtxtMoney1Resource1"
                                    MaxLength="15"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="UHPtxtMoneyFormat1" runat="server" ControlToValidate="UHPtxtMoney1"
                                    ErrorMessage="Số tiền dự tính thứ nhất phải được gõ bằng số hoặc lớn hơn 0!"
                                    meta:resourcekey="txtIntentDatePay1" Text="*" ValidationExpression="\s*[1-9]\d*\s*"
                                    ValidationGroup="CheckInsertOrUpdate"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="UHPtxtMoney2" runat="server" Width="85px" meta:resourcekey="UHPtxtMoney2Resource1"
                                    MaxLength="15"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="UHPtxtMoneyFormat2" runat="server" ControlToValidate="UHPtxtMoney2"
                                    ErrorMessage="Số tiền dự tính thứ hai phải được gõ bằng số hoặc lớn hơn 0!" meta:resourcekey="txtIntentDatePay2"
                                    Text="*" ValidationExpression="\s*[1-9]\d*\s*" ValidationGroup="CheckInsertOrUpdate"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="UHPtxtMoney3" runat="server" Width="85px" meta:resourcekey="UHPtxtMoney3Resource1"
                                    MaxLength="15"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="UHPtxtMoneyFormat3" runat="server" ControlToValidate="UHPtxtMoney3"
                                    ErrorMessage="Số tiền dự tính thứ ba phải được gõ bằng số hoặc lớn hơn 0!" meta:resourcekey="txtIntentDatePay3"
                                    Text="*" ValidationExpression="\s*[1-9]\d*\s*" ValidationGroup="CheckInsertOrUpdate"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="UHPtxtMoney4" runat="server" Width="85px" meta:resourcekey="UHPtxtMoney4Resource1"
                                    MaxLength="15"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="UHPtxtMoneyFormat4" runat="server" ControlToValidate="UHPtxtMoney4"
                                    ErrorMessage="Số tiền dự tính thứ tư phải được gõ bằng số hoặc lớn hơn 0!" meta:resourcekey="txtIntentDatePay4"
                                    Text="*" ValidationExpression="\s*[1-9]\d*\s*" ValidationGroup="CheckInsertOrUpdate"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="UHPtxtMoney5" runat="server" Width="85px" meta:resourcekey="UHPtxtMoney5Resource1"
                                    MaxLength="15"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="UHPtxtMoneyFormat5" runat="server" ControlToValidate="UHPtxtMoney5"
                                    ErrorMessage="Số tiền dự tính thứ năm phải được gõ bằng số hoặc lớn hơn 0!" meta:resourcekey="txtIntentDatePay5"
                                    Text="*" ValidationExpression="\s*[1-9]\d*\s*" ValidationGroup="CheckInsertOrUpdate"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="5">
                                <br />
                            </td>
                        </tr>
                    </table>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnCheckValid" runat="server" OnClick="btnCheckValid_Click" Text="Check"
                    ValidationGroup="CheckInsertOrUpdate" Enabled="False" meta:resourcekey="btnCheckValidResource1" />
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" Enabled="False"
                    ValidationGroup="InsertOrUpdate" Visible="False" meta:resourcekey="btnSaveResource1" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="h">

    <style type="text/css">
        .style1
        {
            width: 111px;
        }
    </style>

</asp:Content>

