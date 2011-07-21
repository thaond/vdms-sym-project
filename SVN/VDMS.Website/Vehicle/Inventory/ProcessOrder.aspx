<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="ProcessOrder.aspx.cs" Inherits="Vehicle_Inventory_ProcessOrder" Title="VDMS - Processing order..."
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <table cellpadding="2" cellspacing="0" width="100%" border="0">
            <tr>
                <td style="width: 10%;">
                    <asp:Localize ID="litStatus" runat="server" Text="Order status:" meta:resourcekey="litStatusResource1"></asp:Localize>
                </td>
                <td style="width: 40%">
                    <asp:Label ID="lblStatus" runat="server" CssClass="valueField" meta:resourcekey="lblStatusResource1"></asp:Label>
                </td>
                <td style="width: 10%;">
                    <asp:Localize ID="litOrderTimes" runat="server" Text="Times:" meta:resourcekey="litOrderTimesResource1"></asp:Localize>
                </td>
                <td style="width: 40%">
                    <asp:Label ID="lblOrderTimes" runat="server" CssClass="valueField" meta:resourcekey="lblOrderTimesResource1"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 10%;">
                    <asp:Localize ID="litOrderDate" runat="server" Text="Order date:" meta:resourcekey="litOrderDateResource1"></asp:Localize>
                </td>
                <td style="width: 40%">
                    <asp:Label ID="lblOrderDate" runat="server" CssClass="valueField" meta:resourcekey="lblOrderDateResource1"></asp:Label>
                </td>
                <td style="width: 10%;">
                    <asp:Localize ID="litShippingTo" runat="server" Text="Address:" meta:resourcekey="litShippingToResource1"></asp:Localize>
                </td>
                <td style="width: 40%">
                    <asp:Label ID="lblShipingTo" runat="server" CssClass="valueField" meta:resourcekey="lblShipingToResource1"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 10%;">
                    <asp:Localize ID="litDealerComment" runat="server" Text="Dealer comment:" meta:resourcekey="litDealerCommentResource1"></asp:Localize>
                </td>
                <td style="width: 40%">
                    <asp:Label ID="lblDealerComment" runat="server" CssClass="valueField" meta:resourcekey="lblDealerCommentResource1"></asp:Label>
                </td>
                <td style="width: 10%;">
                    <asp:Localize ID="litSecondaryAddress" runat="server" Text="Secondary Address:" meta:resourcekey="litSecondaryAddressResource1"></asp:Localize>
                </td>
                <td style="width: 40%">
                    <asp:Label ID="lblSecondaryAddress" runat="server" CssClass="valueField" meta:resourcekey="lblSecondaryAddressResource1"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <asp:ValidationSummary ID="vsMain" runat="server" meta:resourcekey="vsMainResource1" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="TipTopOrderNumber"
        meta:resourcekey="ValidationSummary1Resource1" />
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="SendComment"
        meta:resourcekey="ValidationSummary2Resource1" />
    <asp:Label ID="lblError" runat="server" ForeColor="Red" 
        meta:resourcekey="lblErrorResource1"></asp:Label>
    <asp:MultiView ID="mvMain" runat="server" ActiveViewIndex="0">
        <asp:View ID="view1" runat="server">
            <div class="form" style="float: left">
                <p>
                    <asp:Localize ID="litTipTopOrderNumber" runat="server" Text="Tip-Top Order number:"
                        meta:resourcekey="litTipTopOrderNumberResource1"></asp:Localize>
                    <asp:TextBox ID="txtTipTopOrderNumber" runat="server" MaxLength="2000" meta:resourcekey="txtTipTopOrderNumberResource1"></asp:TextBox><asp:RequiredFieldValidator
                        ID="rfvTipTop" runat="server" Text="*" ControlToValidate="txtTipTopOrderNumber"
                        ValidationGroup="TipTopOrderNumber" SetFocusOnError="True" meta:resourcekey="rfvTipTopResource1"></asp:RequiredFieldValidator><asp:CustomValidator
                            ID="cvTipTopOrderNumber" runat="server" ControlToValidate="txtTipTopOrderNumber"
                            ErrorMessage="Order number in Tip-Top is invalid." OnServerValidate="cvTipTopOrderNumber_ServerValidate"
                            ValidationGroup="TipTopOrderNumber" SetFocusOnError="True" meta:resourcekey="cvTipTopOrderNumberResource1"
                            Text="*"></asp:CustomValidator>
                    <asp:Button ID="cmdConfirm" runat="server" SkinID="SpecialButton" Text="Confirm"
                        OnClick="cmdConfirm_Click" ValidationGroup="TipTopOrderNumber" meta:resourcekey="cmdConfirmResource1"
                        CommandName="Confirm" />
                    &nbsp;<asp:Button ID="cmdRefreshShipping" runat="server" CommandName="RefreshShipping"
                        meta:resourcekey="cmdRefreshShippingResource1" SkinID="SpecialButton" Text="Refresh shipping"
                        OnClick="cmdRefreshShipping_Click" />
                </p>
                <p>
                    <asp:Localize ID="litComment" runat="server" Text="Comment to dealer:" meta:resourcekey="litCommentResource1"></asp:Localize>
                    <asp:TextBox ID="txtComment" runat="server" MaxLength="1000" meta:resourcekey="txtCommentResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtComment"
                        ErrorMessage="Comment is required." ValidationGroup="SendComment" SetFocusOnError="True"
                        meta:resourcekey="RequiredFieldValidator1Resource1" Text="*"></asp:RequiredFieldValidator>
                    <asp:Button ID="cmdCommentAndSave" runat="server" Text="Send comment" SkinID="SubmitButton"
                        OnClick="cmdCommentAndSave_Click" ValidationGroup="SendComment" meta:resourcekey="cmdCommentAndSaveResource1"
                        CommandName="Comment" />
                </p>
                <asp:Button ID="cmdApprove" runat="server" SkinID="submitButton" Text="Approve" OnClick="cmdApprove_Click"
                    meta:resourcekey="cmdApproveResource1" CommandName="Approve" />
                <asp:Button ID="cmdSplit" runat="server" SkinID="SubmitButton" Text="Split" OnClick="cmdSplit_Click"
                    meta:resourcekey="cmdSplitResource1" />
                <asp:Button ID="cmdEdit" runat="server" SkinID="SubmitButton" Text="Edit" meta:resourcekey="cmdEditResource1"
                    CommandName="Edit" />
                <asp:Button ID="btnPrint" runat="server" Text="Print" UseSubmitBehavior="False" 
                    meta:resourcekey="btnPrintResource1" />
                <asp:Button ID="cmdBack" runat="server" SkinID="SubmitButton" Text="Back" PostBackUrl="Business.aspx"
                    meta:resourcekey="cmdBackResource1" />
            </div>
            <div class="form" style="float: left; margin-left:3px; width: 200px">
                <table>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal1" runat="server" Text="Bonus:" 
                                meta:resourcekey="Literal1Resource1"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBonusAmount" ReadOnly="True" runat="server" 
                                meta:resourcekey="txtBonusAmountResource1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal2" runat="server" Text="Confirmed:" 
                                meta:resourcekey="Literal2Resource1"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBox ID="chBConfirmed" runat="server" Enabled="False" 
                                meta:resourcekey="chBConfirmedResource1" />
                            <asp:CustomValidator ID="cvBonus" runat="server" 
                                ErrorMessage="Bonus has not been confirmed!" ControlToValidate="txtBonusAmount"
                                OnServerValidate="cvBonus_ServerValidate" 
                                ValidationGroup="TipTopOrderNumber" meta:resourcekey="cvBonusResource1">*</asp:CustomValidator>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="clear: both">
            </div>
            <div class="grid">
                <vdms:PageGridView ID="gvMain" runat="server" AutoGenerateColumns="False" Caption="List items in order"
                    DataKeyNames="ORDERDETAILID" meta:resourcekey="gvMainResource1">
                    <Columns>
                        <asp:BoundField DataField="ITEMCODE" HeaderText="Item code" meta:resourcekey="BoundFieldResource1" />
                        <asp:BoundField DataField="COLORNAME" HeaderText="Color name" meta:resourcekey="BoundFieldResource2" />
                        <asp:BoundField DataField="ORDERQTY" HeaderText="Quantity" meta:resourcekey="BoundFieldResource3">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UNITPRICE" HeaderText="Price" DataFormatString="{0:n0}"
                            HtmlEncode="False" meta:resourcekey="BoundFieldResource4">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ITEMTOTALPRICE" HeaderText="Total" DataFormatString="{0:n0}"
                            HtmlEncode="False" meta:resourcekey="BoundFieldResource5">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
                </vdms:PageGridView>
            </div>
        </asp:View>
        <asp:View ID="view2" runat="server">
            <div class="grid">
                <vdms:PageGridView ID="gvSplit" runat="server" AutoGenerateColumns="False" Caption="List items in order"
                    DataKeyNames="ORDERDETAILID" meta:resourcekey="gvSplitResource1">
                    <Columns>
                        <asp:BoundField DataField="ITEMCODE" HeaderText="Item code" meta:resourcekey="BoundFieldResource6" />
                        <asp:BoundField DataField="COLORNAME" HeaderText="Color name" meta:resourcekey="BoundFieldResource7" />
                        <asp:TemplateField HeaderText="Total Quantity" meta:resourcekey="TemplateFieldResource1">
                            <ItemTemplate>
                                <asp:TextBox ID="txtTotal" runat="server" Text='<%# Bind("ORDERQTY") %>' ReadOnly="True"
                                    meta:resourcekey="txtTotalResource1"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Order 2 Quantity" meta:resourcekey="TemplateFieldResource2">
                            <ItemTemplate>
                                <asp:TextBox ID="txtSubQty" runat="server" Text="0" meta:resourcekey="txtSubQtyResource1"></asp:TextBox>
                                <asp:CompareValidator ID="cvQuantity" runat="server" ControlToValidate="txtSubQty"
                                    ControlToCompare="txtTotal" ErrorMessage="New quantity must be equal or less than old quantity"
                                    Text="*" Type="Integer" Operator="LessThanEqual" meta:resourcekey="cvQuantityResource1"></asp:CompareValidator>
                                <asp:RangeValidator ID="rvQuantity" runat="server" ControlToValidate="txtSubQty"
                                    ErrorMessage="Quantity must not be negative" Text="*" Type="Integer" MinimumValue="0"
                                    MaximumValue="999" meta:resourcekey="rvQuantityResource1"></asp:RangeValidator>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </vdms:PageGridView>
            </div>
            <br />
            <div class="form">
                <table cellpadding="2" cellspacing="0" border="0" class="InputTable">
                    <tr>
                        <td style="width: 150px;" align="right">
                            <asp:Localize ID="litOrder1Date" Text="Order 1 date:" runat="server" meta:resourcekey="litOrder1DateResource1"></asp:Localize>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrder1Date" runat="server" Width="88px" meta:resourcekey="txtOrder1DateResource1"></asp:TextBox>
                            <asp:ImageButton ID="ibFromDate" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
                                meta:resourcekey="ibFromDateResource1" />
                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="Validator"
                                SetFocusOnError="True" ControlToValidate="txtOrder1Date" meta:resourcekey="rfvFromDateResource1"
                                Text="*"></asp:RequiredFieldValidator>
                            <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtOrder1Date"
                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="AM;PM"
                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                                CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                                Enabled="True">
                            </ajaxToolkit:MaskedEditExtender>
                            <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtOrder1Date"
                                PopupButtonID="ibFromDate" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Localize ID="litOrder2Date" Text="Order 2 date:" runat="server" meta:resourcekey="litOrder2DateResource1"></asp:Localize>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrder2Date" runat="server" Width="88px" meta:resourcekey="txtOrder2DateResource1"></asp:TextBox>
                            <asp:ImageButton ID="ibToDate" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
                                meta:resourcekey="ibToDateResource1" />
                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="Validator" SetFocusOnError="True"
                                ControlToValidate="txtOrder2Date" meta:resourcekey="rfvToDateResource1" Text="*"></asp:RequiredFieldValidator>
                            <ajaxToolkit:MaskedEditExtender ID="meeToDate" runat="server" TargetControlID="txtOrder2Date"
                                Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="AM;PM"
                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                                CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                                Enabled="True">
                            </ajaxToolkit:MaskedEditExtender>
                            <ajaxToolkit:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtOrder2Date"
                                PopupButtonID="ibToDate" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="cmdDoSplit" runat="server" Text="Split" SkinID="SubmitButton" OnClick="cmdDoSplit_Click"
                                meta:resourcekey="cmdDoSplitResource1" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:View>
        <asp:View ID="view3" runat="server">
            <div class="form">
                <table cellpadding="2" cellspacing="0" border="0" class="InputTable">
                    <tr>
                        <td style="width: 150px;">
                            <asp:Image ID="image1" runat="server" SkinID="RequireField" meta:resourcekey="image1Resource1" /><asp:Localize
                                ID="litOldOrderNumber" runat="server" Text="Old order number:" meta:resourcekey="litOldOrderNumberResource1"></asp:Localize>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOldOrderNumber" runat="server" MaxLength="30" meta:resourcekey="txtOldOrderNumberResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvOldOrderNumber" runat="server" ControlToValidate="txtOldOrderNumber"
                                SetFocusOnError="True" ErrorMessage="Old order number is required." Text="*"
                                meta:resourcekey="rfvOldOrderNumberResource1"></asp:RequiredFieldValidator><br />
                            <asp:CheckBox ID="chkDeleted" runat="server" Text="Deleted" meta:resourcekey="chkDeletedResource1" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px;">
                            <asp:Image ID="image2" runat="server" SkinID="RequireField" meta:resourcekey="image2Resource1" /><asp:Localize
                                ID="litNewOrderNumber" runat="server" Text="New order number:" meta:resourcekey="litNewOrderNumberResource1"></asp:Localize>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewOrderNumber" runat="server" MaxLength="30" meta:resourcekey="txtNewOrderNumberResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvNewOrderNumber" runat="server" ControlToValidate="txtNewOrderNumber"
                                SetFocusOnError="True" ErrorMessage="Tip-Top order number is required." Text="*"
                                meta:resourcekey="rfvNewOrderNumberResource1"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvNewOrderNumber" runat="server" ControlToValidate="txtNewOrderNumber"
                                ErrorMessage="Order number in Tip-Top is invalid." OnServerValidate="cvNewOrderNumber_ServerValidate"
                                meta:resourcekey="cvNewOrderNumberResource1" Text="*"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="cmdDoConfirmSplit" runat="server" Text="Split" OnClick="cmdDoConfirmSplit_Click"
                                meta:resourcekey="cmdDoConfirmSplitResource1" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div class="PageNote">
                <ul>
                    <li>
                        <asp:Localize ID="litHelp" runat="server" Text="Input the old order number in VDMS, and a list of new order number in Tip-Top, each saperate with semicolon."
                            meta:resourcekey="litHelpResource1"></asp:Localize>
                    </li>
                    <li>
                        <asp:Localize ID="litPageNote" runat="server" Text="You must ensure all the new order in Tip-Top MUST NOT ship to dealer before synchronize data"
                            meta:resourcekey="litPageNoteResource1"></asp:Localize>
                    </li>
                </ul>
            </div>
        </asp:View>
    </asp:MultiView>
    <p>
        <asp:Label ID="lblResult" runat="server" ForeColor="Red" meta:resourcekey="lblResultResource1"></asp:Label>
        <asp:PlaceHolder ID="phGoback" runat="server" Visible="False">
            <asp:Literal ID="litProcessComplete" runat="server" Text="Process completed without error. To go back, please "
                meta:resourcekey="litProcessCompleteResource1"></asp:Literal>
            <asp:LinkButton meta:resourcekey="lnkbBackResource1" ID="lnkbBack" PostBackUrl="Business.aspx"
                runat="server">click here.</asp:LinkButton>
        </asp:PlaceHolder>
    </p>
</asp:Content>
