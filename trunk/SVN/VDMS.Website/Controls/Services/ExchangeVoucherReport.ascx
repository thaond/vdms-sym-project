<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ExchangeVoucherReport.ascx.cs"
    Inherits="UC_ExchangeVoucherReport" %>
<%--<%@ Register Assembly="VDMS.Common" Namespace="VDMS.Common.Web" TagPrefix="vdms" %>--%>

<script language="javascript" type="text/javascript">
<!--
    function CopyValueToPostBack(src, dest) {
        var oDest = document.getElementById(dest);
        var oSrc = document.getElementById(src);

        if ((oDest != null) && (oSrc != null)) oDest.value = oSrc.value;
        return true;
    }

-->
</script>

<asp:HiddenField ID="hdPartCode" runat="server" />
<asp:HiddenField ID="hdPartCodeO" runat="server" />
<asp:HiddenField ID="hdQuantity" runat="server" />
<asp:HiddenField ID="hdPrice" runat="server" />
<asp:HiddenField ID="hdFee" runat="server" />
<asp:HiddenField ID="hdNote" runat="server" />
<asp:HiddenField ID="hdVoucherNo" runat="server" />
<asp:HiddenField ID="hdManPower" runat="server" />
<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
    <asp:View ID="vwMain" runat="server">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Edit"
            meta:resourcekey="ValidationSummary1Resource1" />
        &nbsp;
        <asp:Literal Visible="false" runat="server" ID="litErrMsgCommentBlank" meta:resourcekey="litErrMsgCommentBlank"></asp:Literal>
        <asp:BulletedList ID="bllErrorMsg" runat="server" meta:resourcekey="bllErrorMsgResource1"
            CssClass="errorMsg">
        </asp:BulletedList>
        <br />
        <table border="0" cellpadding="2" cellspacing="0" style="width: 99%">
            <tr>
                <td colspan="2">
                    <asp:Panel ID="pnDealer" runat="server" Height="100%" Width="100%" meta:resourcekey="pnDealerResource1">
                        <table border="0" cellpadding="2" cellspacing="0" style="width: 100%">
                            <tr>
                                <td style="width: 84px">
                                    <asp:Literal ID="Literal1" runat="server" Text="Dealer code:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="litDealerCode" runat="server" meta:resourcekey="litDealerCodeResource1"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 84px">
                                    <asp:Literal ID="Literal2" runat="server" Text="Dealer name:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="litDealerName" runat="server" meta:resourcekey="litDealerNameResource1"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="pnPager" runat="server" Height="27px" Width="100%" meta:resourcekey="pnPagerResource1">
                        <div style="float: right; text-align: right">
                            <asp:Button ID="cmdFirst" runat="server" CommandArgument="First" CommandName="Page"
                                OnClick="cmdFirst_Click" Text="First" meta:resourcekey="cmdFirstResource1" />
                            <asp:Button ID="cmdPrevious" runat="server" CommandArgument="Prev" CommandName="Page"
                                OnClick="cmdFirst_Click" Text="Previous" meta:resourcekey="cmdPreviousResource1" />
                            <asp:Button ID="cmdNext" runat="server" CommandArgument="Next" CommandName="Page"
                                OnClick="cmdFirst_Click" Text="Next" meta:resourcekey="cmdNextResource1" />
                            <asp:Button ID="cmdLast" runat="server" CommandArgument="Last" CommandName="Page"
                                OnClick="cmdFirst_Click" Text="Last" meta:resourcekey="cmdLastResource1" />
                        </div>
                        <div style="float: left">
                            <asp:Literal ID="litPageInfo" runat="server" meta:resourcekey="litPageInfoResource1"></asp:Literal></div>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="grid">
                        <vdms:EmptyGridViewEx GridLines="Both" ID="EmptyGridViewEx1" runat="server" AllowInsertEmptyRow="False"
                            AutoGenerateColumns="False" GennerateSpanDataTable="False" IncludeChildsListInLevel="False"
                            RealPageSize="10" ShowEmptyTable="True" OnRowDataBound="EmptyGridViewEx1_RowDataBound"
                            OnRowCancelingEdit="EmptyGridViewEx1_RowCancelingEdit" OnRowEditing="EmptyGridViewEx1_RowEditing"
                            OnRowUpdating="EmptyGridViewEx1_RowUpdating" DataKeyNames="ExchangeVoucherNumber"
                            ShowFooter="True" meta:resourcekey="EmptyGridViewEx1Resource1" ShowEmptyFooter="True"
                            EnableViewState="False">
                            <Columns>
                                <asp:TemplateField HeaderText="No" meta:resourcekey="TemplateFieldResource1">
                                    <ItemTemplate>
                                        <asp:Literal ID="Literal28" runat="server" Text='<%# Eval("No") %>' __designer:wfdid="w13"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Buy date" meta:resourcekey="TemplateFieldResource2">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="Literal5" Text='<%# FormatDate(Eval("BuyDate")) %>'
                                            __designer:wfdid="w265"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Repair date" meta:resourcekey="TemplateFieldResource3">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="Literal6" Text='<%# FormatDate(Eval("RepairDate")) %>'
                                            __designer:wfdid="w207"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Confirmed date" meta:resourcekey="TemplateFieldResource3x">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="Literal6f" Text='<%# FormatDate(Eval("ConfirmedDate")) %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Engine number" meta:resourcekey="TemplateFieldResource4">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="Literal7" Text='<%# Eval("EngineNumber") %>' __designer:wfdid="w116"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Model" meta:resourcekey="TemplateFieldResource5">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="Literal8" Text='<%# Eval("Model") %>' __designer:wfdid="w679"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Kilometers" meta:resourcekey="TemplateFieldResource6">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="Literal9" Text='<%# Eval("Km") %>' __designer:wfdid="w210"
                                            OnDataBinding="Literal18_DataBinding"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer" meta:resourcekey="TemplateFieldResource7">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="Literal10" Text='<%# Eval("CustomerName") %>' __designer:wfdid="w211"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Service record sheet" meta:resourcekey="TemplateFieldResource8">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="HyperLink1" runat="server" CssClass="showDetail" Text='<%# Eval("ServiceSheetNumber") %>'
                                            Target="_blank" NavigateUrl='<%# "~/service/WarrantyContent.aspx?srsn=" + Eval("ServiceSheetNumber") %>'
                                            __designer:wfdid="w13"></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exchange voucher number" meta:resourcekey="TemplateFieldResource9">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="HyperLink2" runat="server" meta:resourcekey="HyperLink2Resource1"
                                            CssClass="showDetail" Text='<%# Eval("ExchangeVoucherNumber") %>' Target="_blank"
                                            NavigateUrl='<%# "~/service/WarrantyContent.aspx?pcvn=" + Eval("ExchangeVoucherNumber") %>'
                                            __designer:wfdid="w14"></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Spare number" meta:resourcekey="TemplateFieldResource10">
                                    <EditItemTemplate>
                                        <asp:Literal runat="server" ID="Literal13x" Text='<%# Eval("SpareNumberO") %>' __designer:wfdid="w62"></asp:Literal>
                                        <br />
                                        <table cellspacing="0" cellpadding="0" width="1" border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; width: 1px;
                                                        padding-top: 0px">
                                                        <asp:TextBox runat="server" Width="86px" MaxLength="30" Text='<%# Bind("SpareNumberM") %>'
                                                            ID="txtSpareNumberM" meta:resourceKey="txtSpareNumberMResource1" __designer:wfdid="w63"></asp:TextBox>
                                                    </td>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; width: 1px;
                                                        padding-top: 0px">
                                                        <asp:RequiredFieldValidator runat="server" ErrorMessage="&quot;Spare Number&quot; cannot be blank!"
                                                            ControlToValidate="txtSpareNumberM" Text="*" ValidationGroup="Edit" ID="RequiredFieldValidator1"
                                                            meta:resourceKey="RequiredFieldValidator1Resource1" __designer:wfdid="w64"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; width: 1px;
                                                        padding-top: 0px">
                                                        <asp:Button runat="server" ID="btnFindSpare" Width="20px" Text="..." CommandName="oooooo"
                                                            meta:resourceKey="btnFindSpareResource1" __designer:wfdid="w65" OnClick="btnFindSpare_Click">
                                                        </asp:Button>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <asp:TextBox runat="server" Width="51px" CssClass="hidden" Text='<%# Bind("SpareNumberO") %>'
                                            ID="txtSpareNumberO" meta:resourceKey="txtSpareNumberOResource1" __designer:wfdid="w66"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="Literal13" Text='<%# Eval("SpareNumberO") %>' __designer:wfdid="w60"></asp:Literal>
                                        <br />
                                        <asp:Label runat="server" Text='<%# Eval("SpareNumberM") %>' CssClass="changedValue"
                                            ID="Label12" Visible='<%# IsNotSame(Eval("SpareNumberO"), Eval("SpareNumberM")) %>'
                                            meta:resourceKey="Label12Resource1" __designer:wfdid="w61"></asp:Label>
                                    </ItemTemplate>
                                    <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                    <ItemStyle Width="10px" Wrap="False" CssClass="forceNowrap" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity" meta:resourcekey="TemplateFieldResource11">
                                    <EditItemTemplate>
                                        <asp:Literal runat="server" ID="Literal14x" Text='<%# Eval("QuantityO") %>' __designer:wfdid="w148"></asp:Literal>
                                        <br />
                                        <table cellspacing="0" cellpadding="0" border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; width: 1px;
                                                        padding-top: 0px">
                                                        <asp:TextBox runat="server" Width="40px" MaxLength="5" Text='<%# Bind("QuantityM") %>'
                                                            ID="txtQuantityM" meta:resourceKey="txtQuantityMResource1" __designer:wfdid="w149"></asp:TextBox>
                                                    </td>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; width: 1px;
                                                        padding-top: 0px">
                                                        <asp:RequiredFieldValidator runat="server" ErrorMessage="&quot;Quantity&quot; cannot be blank!"
                                                            ControlToValidate="txtQuantityM" Width="1px" Text="*" ValidationGroup="Edit"
                                                            ID="RequiredFieldValidator2" meta:resourceKey="RequiredFieldValidator2Resource1"
                                                            __designer:wfdid="w150"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; width: 1px;
                                                        padding-top: 0px">
                                                        <asp:RegularExpressionValidator runat="server" ErrorMessage="&quot;Quantity&quot; must be numeric!"
                                                            ControlToValidate="txtQuantityM" Width="1px" Text="*" ValidationGroup="Edit"
                                                            ID="RegularExpressionValidator1" ValidationExpression="\s*\d*\s*" meta:resourceKey="RegularExpressionValidator1Resource1"
                                                            __designer:wfdid="w151"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <table style="width: 100%" cellspacing="0" cellpadding="0" border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;
                                                        text-align: right">
                                                        <asp:Literal runat="server" ID="Literal14" Text='<%# Eval("QuantityO") %>' __designer:wfdid="w146"></asp:Literal>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;
                                                        text-align: right">
                                                        <asp:Label runat="server" Text='<%# Eval("QuantityM") %>' CssClass="changedValue"
                                                            ID="Label11" Visible='<%# IsNotSame(Eval("QuantityO"), Eval("QuantityM")) %>'
                                                            meta:resourceKey="Label11Resource1" __designer:wfdid="w147"></asp:Label>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ItemTemplate>
                                    <FooterStyle Font-Bold="True" HorizontalAlign="Center" />
                                    <HeaderStyle Width="10px" />
                                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit price" meta:resourcekey="TemplateFieldResource12">
                                    <EditItemTemplate>
                                        <asp:Literal ID="Literal15x" runat="server" Text='<%# Eval("UnitPriceO") %>' OnDataBinding="Literal18_DataBinding"
                                            __designer:wfdid="w3"></asp:Literal>
                                        <br />
                                        <table cellspacing="0" cellpadding="0" border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; width: 1px;
                                                        padding-top: 0px">
                                                        <asp:TextBox ID="txtUnitPriceM" runat="server" Width="93px" Text='<%# Bind("UnitPriceM") %>'
                                                            __designer:wfdid="w4" meta:resourceKey="txtUnitPriceMResource1" MaxLength="20"></asp:TextBox>
                                                    </td>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; width: 1px;
                                                        padding-top: 0px">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="Edit"
                                                            Text="*" ErrorMessage='"Quantity" cannot be blank!' ControlToValidate="txtUnitPriceM"
                                                            __designer:wfdid="w5" meta:resourceKey="RequiredFieldValidator3Resource1"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; width: 1px;
                                                        padding-top: 0px">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationGroup="Edit"
                                                            Text="*" ErrorMessage='"Unit price" must be numeric!' ControlToValidate="txtUnitPriceM"
                                                            ValidationExpression="\s*\d*\s*" __designer:wfdid="w6" meta:resourceKey="RegularExpressionValidator2Resource1"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <asp:TextBox ID="txtVoucherNo" runat="server" Width="26px" CssClass="hidden" Text='<%# Bind("ExchangeVoucherNumber") %>'
                                            __designer:wfdid="w7" meta:resourceKey="txtVoucherNoResource1"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <table style="width: 100%" cellspacing="0" cellpadding="0" border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;
                                                        text-align: right">
                                                        <asp:Literal ID="Literal15" runat="server" Text='<%# Eval("UnitPriceO") %>' OnDataBinding="Literal18_DataBinding"
                                                            __designer:wfdid="w1"></asp:Literal>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;
                                                        text-align: right">
                                                        <asp:Label ID="Label10" runat="server" CssClass="changedValue" Visible='<%# IsNotSame(Eval("UnitPriceO"), Eval("UnitPriceM")) %>'
                                                            Text='<%# Eval("UnitPriceM") %>' OnDataBinding="Literal18_DataBinding" __designer:wfdid="w2"
                                                            meta:resourceKey="Label10Resource1"></asp:Label>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ItemTemplate>
                                    <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                    <HeaderStyle Width="10px" />
                                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Warranty spares amount" meta:resourcekey="TemplateFieldResource13">
                                    <EditItemTemplate>
                                        <asp:Literal ID="Literal16" runat="server" Text='<%# Eval("WarrantySpareAmountO") %>'
                                            OnDataBinding="Literal18_DataBinding" __designer:wfdid="w24"></asp:Literal>
                                        <br />
                                        <asp:Label ID="Label9" runat="server" CssClass="changedValue" Text='<%# Eval("WarrantySpareAmountM") %>'
                                            OnDataBinding="Literal18_DataBinding" meta:resourceKey="Label9Resource1" Visible='<%# IsNotSame(Eval("WarrantySpareAmountO"), Eval("WarrantySpareAmountM")) %>'
                                            __designer:wfdid="w25"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <table style="width: 100%" cellspacing="0" cellpadding="0" border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;
                                                        text-align: right">
                                                        <asp:Literal ID="Literal16" runat="server" Text='<%# Eval("WarrantySpareAmountO") %>'
                                                            OnDataBinding="Literal18_DataBinding" __designer:wfdid="w22"></asp:Literal>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;
                                                        text-align: right">
                                                        <asp:Label ID="Label9" runat="server" CssClass="changedValue" Text='<%# Eval("WarrantySpareAmountM") %>'
                                                            OnDataBinding="Literal18_DataBinding" meta:resourceKey="Label9Resource2" Visible='<%# IsNotSame(Eval("WarrantySpareAmountO"), Eval("WarrantySpareAmountM")) %>'
                                                            __designer:wfdid="w23"></asp:Label>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ItemTemplate>
                                    <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                    <HeaderStyle Height="10px" />
                                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Man power" meta:resourcekey="TemplateFieldResourceManP">
                                    <EditItemTemplate>
                                        <table cellspacing="0" cellpadding="0" border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px"
                                                        colspan="3">
                                                        <asp:Literal ID="litOmanpower" runat="server" Text='<%# Eval("ManPowerO") %>' OnDataBinding="Literal18_DataBinding"></asp:Literal>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; width: 1px;
                                                        padding-top: 0px">
                                                        <asp:TextBox ID="txtManPowerM" runat="server" Width="114px" Text='<%# Bind("ManPowerM") %>'
                                                            MaxLength="20"></asp:TextBox>
                                                    </td>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; width: 1px;
                                                        padding-top: 0px">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorMP" runat="server" ValidationGroup="Edit"
                                                            Text="*" meta:resourceKey="RequiredFieldValidator5Resource1" ErrorMessage='"Fee amount" cannot be blank!'
                                                            ControlToValidate="txtManPowerM"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; width: 1px;
                                                        padding-top: 0px">
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <table style="width: 100%" cellspacing="0" cellpadding="0" border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;
                                                        text-align: right">
                                                        <asp:Literal ID="litManO" runat="server" Text='<%# Eval("ManPowerO") %>' OnDataBinding="Literal18_DataBinding"></asp:Literal>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;
                                                        text-align: right">
                                                        <asp:Label ID="lbManM" runat="server" CssClass="changedValue" Text='<%# Eval("ManPowerM") %>'
                                                            OnDataBinding="Literal18_DataBinding" meta:resourceKey="Label8Resource1" Visible='<%# IsNotSame(Eval("WarrantyFeeAmountO"), Eval("WarrantyFeeAmountM")) %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Proposed fee" meta:resourcekey="TemplateFieldResourcePF">
                                    <ItemTemplate>
                                        <table style="width: 100%" cellspacing="0" cellpadding="0" border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;
                                                        text-align: right">
                                                        <asp:Literal ID="litPFee" runat="server" Text='<%# Eval("ProposeFee") %>' OnDataBinding="Literal18_DataBinding"
                                                            __designer:wfdid="w17"></asp:Literal>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ItemTemplate>
                                    <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Warranty fee amount" meta:resourcekey="TemplateFieldResource14">
                                    <ItemTemplate>
                                        <table style="width: 100%" cellspacing="0" cellpadding="0" border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;
                                                        text-align: right">
                                                        <asp:Label ID="Label8" runat="server" CssClass="changedValue" Text='<%# Eval("WarrantyFeeAmountM") %>'
                                                            OnDataBinding="Literal18_DataBinding" meta:resourceKey="Label8Resource1" __designer:wfdid="w1"></asp:Label>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ItemTemplate>
                                    <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                    <HeaderStyle Height="10px" />
                                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" meta:resourcekey="TemplateFieldResource15">
                                    <EditItemTemplate>
                                        <asp:Literal runat="server" ID="Literal18" Text='<%# Eval("TotalO") %>' __designer:wfdid="w123"
                                            OnDataBinding="Literal18_DataBinding"></asp:Literal>
                                        <br />
                                        <asp:Label runat="server" Text='<%# Eval("TotalM") %>' CssClass="changedValue" ID="Label7"
                                            Visible='<%# IsNotSame(Eval("TotalO"), Eval("TotalM")) %>' meta:resourceKey="Label7Resource1"
                                            __designer:wfdid="w124" OnDataBinding="Literal18_DataBinding"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <table style="width: 100%" cellspacing="0" cellpadding="0" border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;
                                                        text-align: right">
                                                        <asp:Literal runat="server" ID="Literal18" Text='<%# Eval("TotalO") %>' __designer:wfdid="w121"
                                                            OnDataBinding="Literal18_DataBinding"></asp:Literal>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;
                                                        text-align: right">
                                                        <asp:Label runat="server" Text='<%# Eval("TotalM") %>' CssClass="changedValue" ID="Label7"
                                                            Visible='<%# IsNotSame(Eval("TotalO"), Eval("TotalM")) %>' meta:resourceKey="Label7Resource2"
                                                            __designer:wfdid="w122" OnDataBinding="Literal18_DataBinding"></asp:Label>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ItemTemplate>
                                    <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                    <HeaderStyle Height="10px" />
                                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="VMEP comment" meta:resourcekey="TemplateFieldResource16">
                                    <EditItemTemplate>
                                        <br />
                                        <asp:TextBox ID="txtComments" runat="server" Text='<%# Bind("Comments") %>' meta:resourceKey="txtCommentsResource1"
                                            MaxLength="512" __designer:wfdid="w17"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Literal ID="Literal19" runat="server" Text='<%# Eval("Comments") %>' __designer:wfdid="w16"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Wrap="True" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource19">
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="ImageButtonUpdate" runat="server" ValidationGroup="Edit" Text="Update"
                                            meta:resourceKey="ImageButtonUpdateResource1" CommandName="Update" __designer:wfdid="w10"
                                            ImageUrl="~/Images/update.gif"></asp:ImageButton>
                                        &nbsp;<asp:ImageButton ID="ImageButtonCancel" runat="server" Text="Cancel" meta:resourceKey="ImageButtonCancelResource1"
                                            CommandName="Cancel" __designer:wfdid="w11" ImageUrl="~/Images/cancel.gif" CausesValidation="False">
                                        </asp:ImageButton>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButtonEdit" runat="server" Text="Edit" OnDataBinding="ImageButton1_DataBinding"
                                            meta:resourceKey="ImageButtonEditResource1" Visible='<%# IsNotSame2(-1, Eval("Status")) %>'
                                            CommandName="Edit" CommandArgument='<%# Eval("No") %>' __designer:wfdid="w9"
                                            PostBackUrl="~/Service/verifyExchangeVoucher.aspx" ImageUrl="~/Images/Edit.gif"
                                            CausesValidation="False"></asp:ImageButton>
                                    </ItemTemplate>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" meta:resourcekey="TemplateFieldResource17">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="Literal20" Text='<%# Eval("StatusString") %>' __designer:wfdid="w5"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField meta:resourcekey="TemplateFieldResource18">
                                    <ItemTemplate>
                                        <asp:Button ID="btnApprove" OnClick="btnApprove_Click" runat="server" Width="70px"
                                            Text="Approve" meta:resourceKey="btnApproveResource1" Visible='<%# IsNotSame2(GetApproveedStatus(),Eval("Status")) %>'
                                            CommandArgument='<%# Eval("ExchangeVoucherNumber") %>' __designer:wfdid="w14"
                                            PostBackUrl="~/Default.aspx"></asp:Button>
                                        <asp:Button ID="btnReject" OnClick="btnReject_Click" runat="server" Width="70px"
                                            Text="Reject" meta:resourceKey="btnRejectResource1" Visible='<%# IsNotSame2(GetRejectStatus(), Eval("Status")) %>'
                                            CommandArgument='<%# Eval("ExchangeVoucherNumber") %>' __designer:wfdid="w15"
                                            UseSubmitBehavior="False"></asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </vdms:EmptyGridViewEx>
                    </div>
                    <br />
                    <asp:Panel ID="pnAllPageSummary" runat="server">
                        <table class="GridView" style="border-collapse: collapse; border-style: solid; border-width: 1px;">
                            <caption>
                                <asp:Literal ID="litAllPageSummary" Text="Summary of all pages" runat="server"></asp:Literal></caption>
                            <thead class="summaryName">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal3" meta:resourcekey="Resource11" runat="server"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal11" meta:resourcekey="Resource13" runat="server"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal12" meta:resourcekey="ResourcePF" runat="server"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal17" meta:resourcekey="Resource14" runat="server"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal22" meta:resourcekey="Resource15" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </thead>
                            <tr>
                                <td class="summaryName">
                                    <asp:Literal ID="litDealer" Text="Dealer" runat="server"></asp:Literal>
                                </td>
                                <td class="summaryValue">
                                    <asp:Literal ID="litDealerQty" runat="server" />
                                </td>
                                <td class="summaryValue">
                                    <asp:Literal ID="litDealerSparesAmount" runat="server" />
                                </td>
                                <td class="summaryValue">
                                    <asp:Literal ID="litDealerFee" runat="server" />
                                </td>
                                <td class="summaryValue">
                                    <asp:Literal ID="Literal4" runat="server" />
                                </td>
                                <td class="summaryValue">
                                    <asp:Literal ID="litDealerTotalAll" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="summaryName">
                                    <asp:Literal ID="litVMEP" Text="VMEP" runat="server"></asp:Literal>
                                </td>
                                <td class="changedValue">
                                    <asp:Literal ID="litVMEPQty" runat="server" />
                                </td>
                                <td class="changedValue">
                                    <asp:Literal ID="litVMEPSparesAmount" runat="server" />
                                </td>
                                <td class="changedValue">
                                    <asp:Literal ID="Literal21" runat="server" />
                                </td>
                                <td class="changedValue">
                                    <asp:Literal ID="litVMEPFee" runat="server" />
                                </td>
                                <td class="changedValue">
                                    <asp:Literal ID="litVMEPTotalAll" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" class="hidden">
                        <tr>
                            <td colspan="3" style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px;
                                padding-top: 0px">
                                <asp:Literal ID="Literal17x" runat="server" OnDataBinding="Literal18_DataBinding"
                                    Text='<%# Eval("WarrantyFeeAmountO") %>'></asp:Literal>edit warranty fee
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; width: 1px;
                                padding-top: 0px">
                                <asp:TextBox ID="txtWarrantyFeeAmountM" runat="server" MaxLength="20" meta:resourceKey="txtWarrantyFeeAmountMResource1"
                                    Text='<%# Bind("WarrantyFeeAmountM") %>' Width="114px"></asp:TextBox>
                            </td>
                            <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; width: 1px;
                                padding-top: 0px">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtWarrantyFeeAmountM"
                                    ErrorMessage='"Fee amount" cannot be blank!' meta:resourceKey="RequiredFieldValidator5Resource1"
                                    Text="*" ValidationGroup="Edit" Enabled="False" Visible="False"></asp:RequiredFieldValidator>
                            </td>
                            <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; width: 1px;
                                padding-top: 0px">
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtWarrantyFeeAmountM"
                                    ErrorMessage='"Fee amount" must be numeric!' meta:resourceKey="RegularExpressionValidator4Resource1"
                                    Text="*" ValidationExpression="\s*\d*\s*" ValidationGroup="Edit" Enabled="False"
                                    Visible="False"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 82px">
                </td>
                <td>
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel1" runat="server" Height="50px" Visible="False" Width="583px"
            meta:resourcekey="Panel1Resource1">
            <asp:TextBox ID="txtWarrantySpareAmountM" runat="server" Text='<%# Bind("WarrantySpareAmountM") %>'
                Width="114px" meta:resourcekey="txtWarrantySpareAmountMResource1"></asp:TextBox><asp:RequiredFieldValidator
                    ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtWarrantySpareAmountM"
                    ErrorMessage='&quot;Spare amount&quot; cannot be blank!' ValidationGroup="Edit"
                    meta:resourcekey="RequiredFieldValidator4Resource1" Text="*"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                        ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtWarrantySpareAmountM"
                        ErrorMessage='&quot;Spare amount&quot; must be numeric!' ValidationExpression="\s*\d*\s*"
                        ValidationGroup="Edit" meta:resourcekey="RegularExpressionValidator3Resource1"
                        Text="*"></asp:RegularExpressionValidator>
            <asp:Label ID="Label1" runat="server" CssClass="gridItemSep" Text="/" Visible='<%# IsNotSame(Eval("SpareNumberO"), Eval("SpareNumberM")) %>'
                meta:resourcekey="Label1Resource1"></asp:Label>
            <asp:Label ID="Label3" runat="server" CssClass="gridItemSep" Text="/" Visible='<%# IsNotSame(Eval("UnitPriceO"), Eval("UnitPriceM")) %>'
                meta:resourcekey="Label3Resource1"></asp:Label>
            <asp:Label ID="Label4" runat="server" CssClass="gridItemSep" Text="/" Visible='<%# IsNotSame(Eval("WarrantySpareAmountO"), Eval("WarrantySpareAmountM")) %>'
                meta:resourcekey="Label4Resource1"></asp:Label>
            <asp:Label ID="Label4x" runat="server" CssClass="gridItemSep" Text="/" Visible='<%# IsNotSame(Eval("WarrantySpareAmountO"), Eval("WarrantySpareAmountM")) %>'
                meta:resourcekey="Label4xResource1"></asp:Label>
            <asp:TextBox ID="txtTotalM" runat="server" Text='<%# Bind("TotalM") %>' Width="114px"
                meta:resourcekey="txtTotalMResource1"></asp:TextBox>
            <asp:Label ID="Label6x" runat="server" CssClass="gridItemSep" Text="/" Visible='<%# IsNotSame(Eval("TotalO"), Eval("TotalM")) %>'
                meta:resourcekey="Label6xResource1"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtTotalM"
                ErrorMessage='&quot;Total&quot; cannot be blank!' ValidationGroup="Edit" meta:resourcekey="RequiredFieldValidator6Resource1"
                Text="*"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator5"
                    runat="server" ControlToValidate="txtTotalM" ErrorMessage='&quot;Total&quot; must be numeric!'
                    ValidationExpression="\s*\d*\s*" ValidationGroup="Edit" meta:resourcekey="RegularExpressionValidator5Resource1"
                    Text="*"></asp:RegularExpressionValidator>
            <asp:Label ID="Label5" runat="server" CssClass="gridItemSep" Text="/" Visible='<%# IsNotSame(Eval("WarrantyFeeAmountO"), Eval("WarrantyFeeAmountM")) %>'
                meta:resourcekey="Label5Resource1"></asp:Label>
            <asp:Label ID="Label2" runat="server" CssClass="gridItemSep" Text="/" Visible='<%# IsNotSame(Eval("QuantityO"), Eval("QuantityM")) %>'
                meta:resourcekey="Label2Resource1"></asp:Label>
            <asp:Label ID="Label6" runat="server" CssClass="gridItemSep" Text="/" Visible='<%# IsNotSame(Eval("TotalO"), Eval("TotalM")) %>'
                meta:resourcekey="Label6Resource1"></asp:Label>
            &nbsp; &nbsp; &nbsp;</asp:Panel>
    </asp:View>
    <asp:View ID="vwSelectSpare" runat="server">
        <asp:Literal ID="Literal25" runat="server" meta:resourcekey="Literal25Resource1"
            Text="&lt;H3&gt;Select spare&lt;/H3&gt;"></asp:Literal><asp:ValidationSummary ID="ValidationSummary3"
                runat="server" meta:resourcekey="ValidationSummary3Resource1" ValidationGroup="SelectSpare" />
        <br />
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:GridView ID="gvSelectSpare" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CssClass="GridView" DataSourceID="ObjectDataSource1" OnPreRender="gvSelectxxx_PreRender"
                        OnRowCommand="gvSelectxxx_page" OnSelectedIndexChanging="gvSelectSpare_SelectedIndexChanging"
                        PageSize="5" Width="492px" meta:resourcekey="gvSelectSpareResource1">
                        <Columns>
                            <asp:TemplateField HeaderText="Spare number" meta:resourcekey="TemplateFieldResource20">
                                <EditItemTemplate>
                                    <asp:Literal ID="Literal34" runat="server" Text='<%# Bind("Partcode") %>'></asp:Literal>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Literal ID="litSelectedSpareNumber" runat="server" Text='<%# Bind("Partcode") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Spare name" meta:resourcekey="TemplateFieldResource21">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Partnamevn") %>' meta:resourcekey="TextBox2Resource1"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("Partnamevn") %>' meta:resourcekey="Label2Resource2"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Spare name" meta:resourcekey="TemplateFieldResource22">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Partnameen") %>' meta:resourcekey="TextBox3Resource1"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("Partnameen") %>' meta:resourcekey="Label3Resource2"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unitprice" meta:resourcekey="TemplateFieldResource23">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Unitprice") %>' meta:resourcekey="TextBox4Resource1"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("Unitprice") %>' meta:resourcekey="Label4Resource2"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Warranty length" meta:resourcekey="TemplateFieldResource24">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("Warrantylength") %>' meta:resourcekey="TextBox5Resource1"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("Warrantylength") %>' meta:resourcekey="Label5Resource2"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Warranty time" SortExpression="Warrantytime" meta:resourcekey="TemplateFieldResource25">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("Warrantytime") %>' meta:resourcekey="TextBox6Resource1"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("Warrantytime") %>' meta:resourcekey="Label6Resource2"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Button" ShowSelectButton="True" meta:resourcekey="CommandFieldResource1">
                                <ItemStyle Width="10px" />
                            </asp:CommandField>
                        </Columns>
                        <PagerSettings Mode="NextPreviousFirstLast" Position="Top" />
                        <PagerTemplate>
                            <div style="float: left;">
                                <asp:Literal ID="litgvSelectSparePageInfo" runat="server" meta:resourcekey="litgvSelectSparePageInfoResource1"></asp:Literal></div>
                            <div style="text-align: right; float: right;">
                                <asp:Button ID="cmdFirst" runat="server" CommandArgument="First" CommandName="Page"
                                    Text="First" meta:resourcekey="cmdFirstResource2" />
                                <asp:Button ID="cmdPrevious" runat="server" CommandArgument="Prev" CommandName="Page"
                                    Text="Previous" meta:resourcekey="cmdPreviousResource2" />
                                <asp:Button ID="cmdNext" runat="server" CommandArgument="Next" CommandName="Page"
                                    Text="Next" meta:resourcekey="cmdNextResource2" />
                                <asp:Button ID="cmdLast" runat="server" CommandArgument="Last" CommandName="Page"
                                    Text="Last" meta:resourcekey="cmdLastResource2" />
                            </div>
                        </PagerTemplate>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True" SelectCountMethod="SelectCount"
                        SelectMethod="Select" TypeName="VDMS.I.ObjectDataSource.SparesDataSource">
                        <SelectParameters>
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="spareNumberLike" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:Button ID="Button1" runat="server" meta:resourcekey="Button1Resource1" OnClick="btnCancel_Click"
                        Text="Cancel" />
                </td>
            </tr>
        </table>
    </asp:View>
</asp:MultiView>
