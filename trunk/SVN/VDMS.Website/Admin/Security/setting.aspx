<%@ Page Title="Setting for VDMS system" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="setting.aspx.cs" Inherits="Admin_Security_setting"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Save"
        CssClass="error" meta:resourcekey="ValidationSummary1Resource1" />
    <br />
    <ajaxToolkit:TabContainer ID="t" runat="server" CssClass="ajax__tab_technorati-theme"
        ActiveTabIndex="0" meta:resourcekey="tResource1">
        <ajaxToolkit:TabPanel ID="t1" runat="server">
            <HeaderTemplate>
                <asp:Literal ID="Literal17" Text="Parameters Setting" runat="server" meta:resourcekey="t1Resource1"></asp:Literal></HeaderTemplate>
            <ContentTemplate>
                <div class="form" style="width: 700px">
                    <asp:CheckBox ID="chkCheckEngineNoForPartsChange" runat="server" Text="Must check engine number when input parts change voucher"
                        meta:resourcekey="chkCheckEngineNoForPartsChangeResource1" /><br />
                    <asp:CheckBox ID="chkCheckEngineNoForService" runat="server" Text="Must check engine number when input service record sheet"
                        meta:resourcekey="chkCheckEngineNoForServiceResource1" /><br />
                    <asp:CheckBox ID="chkCheckWarrantyInfoDatabase" runat="server" Text="Must check database code when input warranty informations"
                        meta:resourcekey="chkCheckWarrantyInfoDatabaseResource1" /><br />
                    <asp:CheckBox ID="chkHTFOrderDateControl" runat="server" Text="Apply Order Date Control to North"
                        meta:resourcekey="chkHTFOrderDateControlResource1" /><br />
                    <asp:CheckBox ID="chkDNFOrderDateControl" runat="server" Text="Apply Order Date Control to South"
                        meta:resourcekey="chkDNFOrderDateControlResource1" /><br />
                    <asp:CheckBox ID="chkAllowChangeOrderDate" runat="server" Text="Allow Dealer to change Order Date"
                        meta:resourcekey="chkAllowChangeOrderDateResource1" /><br />
                    <asp:CheckBox ID="chkCheckOrderPartNotDuplicateBeforeConfirmWhenSend" runat="server"
                        Text="Do not allow dealer re-order duplicate part before confirm" meta:resourcekey="chkCheckOrderPartNotDuplicateBeforeConfirmWhenSendResource1" />
                    <asp:CheckBox ID="chkApplySubOrder" runat="server" Text="Also apply for sub-order"
                        meta:resourcekey="chkApplySubOrderResource1" />
                    <br />
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="hidden" meta:resourcekey="btnRefreshResource1" />
                    <asp:CheckBox ID="chbAutoCloseInvI" runat="server" Text="Allow auto close vehicle inventory at:"
                        meta:resourcekey="chbAutoCloseInvIResource1" />
                    <asp:TextBox ID="txtAutoCloseInvTimeI" MaxLength="2" runat="server" Width="20px"
                        meta:resourcekey="txtAutoCloseInvTimeIResource1"></asp:TextBox>
                    <asp:Literal ID="Literal14" runat="server" Text="o'clock in day " meta:resourcekey="Literal14Resource1"></asp:Literal>
                    <asp:TextBox ID="txtAutoCloseInvDayI" MaxLength="2" runat="server" Width="20px" meta:resourcekey="txtAutoCloseInvDayIResource1"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender FilterType="Numbers" TargetControlID="txtAutoCloseInvTimeI"
                        ID="FilteredTextBoxExtender4" runat="server" Enabled="True">
                    </ajaxToolkit:FilteredTextBoxExtender>
                    <ajaxToolkit:FilteredTextBoxExtender FilterType="Numbers" TargetControlID="txtAutoCloseInvDayI"
                        ID="FilteredTextBoxExtender5" runat="server" Enabled="True">
                    </ajaxToolkit:FilteredTextBoxExtender>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:LinkButton ID="btnCloseInvINow" runat="server" Text="Close all to previous month"
                                OnClick="btnCloseInvINow_Click" meta:resourcekey="btnCloseInvINowResource1" />&nbsp;
                            <asp:LinkButton ID="btnAbortProcess" runat="server" Text="Abort Process" OnClick="btnAbortProcess_Click"
                                meta:resourcekey="btnAbortProcessResource1" /><br />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:CheckBox ID="chbAutoSyncPart" runat="server" Text="Allow auto Synchronize VDMS-I parts list at "
                        meta:resourcekey="chbAutoSyncPartResource1" />
                    <asp:TextBox ID="txtSyncPHour" MaxLength="2" runat="server" Width="30px" meta:resourcekey="txtSyncPHourResource1"></asp:TextBox>
                    <asp:Literal ID="Literal16" runat="server" Text=" o'clock every " meta:resourcekey="Literal16Resource1"></asp:Literal>
                    <asp:TextBox ID="txtSyncPdays" MaxLength="3" runat="server" Width="30px" meta:resourcekey="txtSyncPdaysResource1"></asp:TextBox>
                    <asp:Literal ID="Literal15" runat="server" Text="days from" meta:resourcekey="Literal15Resource1"></asp:Literal>
                    <asp:TextBox ID="txtSyncPfromDate" runat="server" Width="70px" meta:resourcekey="txtSyncPfromDateResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ValidationGroup="Save" ID="RequiredFieldValidator5" runat="server"
                        SetFocusOnError="True" ControlToValidate="txtSyncPfromDate" ErrorMessage="Start date cannot be blank!"
                        meta:resourcekey="RequiredFieldValidator5Resource1" Text="*"></asp:RequiredFieldValidator>
                    <asp:ImageButton ID="ibFromDate" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtSyncPfromDate"
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                        CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtSyncPfromDate"
                        PopupButtonID="ibFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    <ajaxToolkit:FilteredTextBoxExtender FilterType="Numbers" TargetControlID="txtSyncPHour"
                        ID="FilteredTextBoxExtender8" runat="server" Enabled="True">
                    </ajaxToolkit:FilteredTextBoxExtender>
                    <ajaxToolkit:FilteredTextBoxExtender FilterType="Numbers" TargetControlID="txtSyncPdays"
                        ID="FilteredTextBoxExtender9" runat="server" Enabled="True">
                    </ajaxToolkit:FilteredTextBoxExtender>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick">
                            </asp:Timer>
                            <asp:LinkButton ID="btnAddNewParts" runat="server" Text="Add new parts" OnClick="btnAddNewParts_Click"
                                meta:resourcekey="btnAddNewPartsResource1" />
                            <asp:Literal ID="litAddingParts" runat="server" Text="Adding new parts..." Visible="False"
                                meta:resourcekey="litAddingPartsResource1"></asp:Literal>&nbsp;
                            <asp:LinkButton ID="btnUpdatePrice" runat="server" Text="Update price" OnClick="btnUpdatePrice_Click"
                                meta:resourcekey="btnUpdatePriceResource1" />
                            <asp:Literal ID="litUpdatingPrice" runat="server" Text="Updating price..." Visible="False"
                                meta:resourcekey="litUpdatingPriceResource1"></asp:Literal><br />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <table>
                        <tr>
                            <td>
                                <asp:Image ID="image8" runat="server" SkinID="RequireField" meta:resourcekey="image3Resource1" />
                                <asp:Literal ID="Literal21" runat="server" Text="Default Labour when auto sync(vehicle):"
                                    meta:resourcekey="Literal6Resource1z"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDefLabourI" Text="50000" runat="server" Width="50px" meta:resourcekey="tbPSResource1"></asp:TextBox>
                                VND
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="image3" runat="server" SkinID="RequireField" meta:resourcekey="image3Resource1" />
                                <asp:Literal ID="Literal6" runat="server" Text="Payment span:" meta:resourcekey="Literal6Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="tbPS" Text="3" runat="server" Width="50px" meta:resourcekey="tbPSResource1"></asp:TextBox>
                                <asp:Literal ID="Literal7" runat="server" Text="days" meta:resourcekey="Literal7Resource1"></asp:Literal>
                                <asp:RequiredFieldValidator ID="rfv1" runat="server" ErrorMessage="Payment span cannot be empty!"
                                    ValidationGroup="Save" SetFocusOnError="True" ControlToValidate="tbPS" meta:resourcekey="rfv1Resource1"
                                    Text="*"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="image1" runat="server" SkinID="RequireField" meta:resourcekey="image1Resource1" />
                                <asp:Literal ID="Literal9" runat="server" Text="Quotation span:" meta:resourcekey="Literal9Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="tbQS" Text="1" runat="server" Width="50px" meta:resourcekey="tbQSResource1"></asp:TextBox>
                                <asp:Literal ID="Literal8" runat="server" Text="days" meta:resourcekey="Literal8Resource1"></asp:Literal>
                                <asp:RequiredFieldValidator ID="rfvQuotation" runat="server" ErrorMessage="Quotation span cannot be empty!"
                                    ValidationGroup="Save" SetFocusOnError="True" ControlToValidate="tbQS" Text="*"
                                    meta:resourcekey="rfvQuotationResource1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="image5" runat="server" SkinID="RequireField" meta:resourcekey="image5Resource1" />
                                <asp:Literal ID="Literal10" runat="server" Text="Shipping span:" meta:resourcekey="Literal10Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="tbSS" Text="1" runat="server" Width="50px" meta:resourcekey="tbSSResource1"></asp:TextBox>
                                <asp:Literal ID="Literal11" runat="server" Text="days" meta:resourcekey="Literal11Resource1"></asp:Literal>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Shipping span cannot be empty!"
                                    ValidationGroup="Save" SetFocusOnError="True" ControlToValidate="tbSS" Text="*"
                                    meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="image2" runat="server" SkinID="RequireField" meta:resourcekey="image2Resource1" />
                                <asp:Literal ID="Literal12" runat="server" Text="Auto instock span:" meta:resourcekey="Literal12Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="tbAIS" Text="24" runat="server" Width="50px" meta:resourcekey="tbAISResource1"></asp:TextBox>
                                <asp:Literal ID="Literal13" runat="server" Text="h" meta:resourcekey="Literal13Resource1"></asp:Literal>
                                <asp:RequiredFieldValidator ID="rfvtbAIS" runat="server" ValidationGroup="Save" ControlToValidate="tbAIS"
                                    SetFocusOnError="True" ErrorMessage="Auto instock span cannot be empty!" Text="*"
                                    meta:resourcekey="rfvtbAISResource1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="image6" runat="server" SkinID="RequireField" meta:resourcekey="image6Resource1" />
                                <asp:Literal ID="Literal3" runat="server" Text="Maximum months allow reopen:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMaxMonthAllowReopen" runat="server" Width="50px" CssClass="number"
                                    meta:resourcekey="txtMaxMonthAllowReopenResource1"></asp:TextBox>
                                <asp:Literal ID="Literal1" runat="server" Text="months" meta:resourcekey="Literal1Resource1"></asp:Literal>
                                <asp:RequiredFieldValidator ValidationGroup="Save" ControlToValidate="txtMaxMonthAllowReopen"
                                    ID="RequiredFieldValidator7" runat="server" ErrorMessage="Maximum months allow reopen cannot be blank!"
                                    meta:resourcekey="RequiredFieldValidator7Resource1" Text="*"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="image7" runat="server" SkinID="RequireField" meta:resourcekey="image7Resource1" />
                                <asp:Literal ID="Literal4" runat="server" Text="Maximum months allow reopen (vehicle):"
                                    meta:resourcekey="Literal4Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMaxMonthAllowReopen1" runat="server" Width="50px" CssClass="number"
                                    meta:resourcekey="txtMaxMonthAllowReopen1Resource1"></asp:TextBox>
                                <asp:Literal ID="Literal5" runat="server" Text="months" meta:resourcekey="Literal5Resource1"></asp:Literal>
                                <ajaxToolkit:FilteredTextBoxExtender FilterType="Numbers" TargetControlID="txtMaxMonthAllowReopen1"
                                    ID="FilteredTextBoxExtender7" runat="server" Enabled="True">
                                </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ValidationGroup="Save" ControlToValidate="txtMaxMonthAllowReopen1"
                                    ID="RequiredFieldValidator3" runat="server" ErrorMessage="Maximum months allow reopen (vehicle), cannot be blank!"
                                    meta:resourcekey="RequiredFieldValidator3Resource1" Text="*"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="image4" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image4Resource1" />
                                <asp:Literal ID="Literal2" runat="server" Text="If over shipping span, send email to:"
                                    meta:resourcekey="Literal2Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="tbEMail" Text="300" runat="server" Width="150px" meta:resourcekey="tbEMailResource1"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="t2" runat="server">
            <HeaderTemplate>
                <asp:Literal ID="Literal18" runat="server" Text="Order Excel Structure" meta:resourcekey="t0Resource1"></asp:Literal></HeaderTemplate>
            <ContentTemplate>
                <div class="form" style="width: 450px">
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                                <h5>
                                    <asp:Literal ID="Literal26" runat="server" Text="Order Excel Structure" meta:resourcekey="t2Resource1"></asp:Literal></HeaderTemplate>
                                </h5>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <asp:Localize ID="l1" runat="server" Text="Order Excel Start Row:" meta:resourcekey="l1Resource2"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOESR" runat="server" meta:resourcekey="txtOESRResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="l2" runat="server" Text="Part Code Column" meta:resourcekey="l2Resource2"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOEPCC" runat="server" meta:resourcekey="txtOEPCCResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="l3" runat="server" Text="Quantity Column" meta:resourcekey="l3Resource2"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOEQC" runat="server" meta:resourcekey="txtOEQCResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="l4" runat="server" Text="Model Column" meta:resourcekey="l4Resource2"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOEMC" runat="server" meta:resourcekey="txtOEMCResource1"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="form" style="width: 450px">
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                                <h5>
                                    <asp:Literal ID="Literal19" runat="server" Text="Sales Excel Structure" meta:resourcekey="t3Resource1"></asp:Literal></HeaderTemplate>
                                </h5>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <asp:Localize ID="l5" runat="server" Text="Order Excel Start Row:" meta:resourcekey="l5Resource2"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSESR" runat="server" meta:resourcekey="txtSESRResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="l6" runat="server" Text="Part Code Column" meta:resourcekey="l6Resource2"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSEPCC" runat="server" meta:resourcekey="txtSEPCCResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="l7" runat="server" Text="Quantity Column" meta:resourcekey="l7Resource2"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSEQC" runat="server" meta:resourcekey="txtSEQCResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="l8" runat="server" Text="Part Type" meta:resourcekey="l8Resource2"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSEPTC" runat="server" meta:resourcekey="txtSEPTCResource1"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <%--kiem ke--%>
                <div class="form" style="width: 450px">
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                                <h5>
                                    <asp:Literal ID="Literal20" runat="server" Text="Cycle count Excel Structure" meta:resourcekey="TabPanel1Resource1"></asp:Literal></HeaderTemplate>
                                </h5>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <asp:Localize ID="Localize1" runat="server" Text="Start Row:" meta:resourcekey="Localize1Resource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCCSR" runat="server" CssClass="number" meta:resourcekey="txtCCSRResource1"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender TargetControlID="txtCCSR" FilterType="Numbers"
                                    ID="FilteredTextBoxExtender1" runat="server" Enabled="True">
                                </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCCSR"
                                    ErrorMessage="Start Row cannot be empty!" ValidationGroup="Save" meta:resourcekey="RequiredFieldValidator2Resource1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize2" runat="server" Text="Part Code Column" meta:resourcekey="Localize2Resource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCCPC" runat="server" CssClass="number" meta:resourcekey="txtCCPCResource1"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender TargetControlID="txtCCPC" FilterType="Numbers"
                                    ID="FilteredTextBoxExtender2" runat="server" Enabled="True">
                                </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="rfvCCPC" runat="server" ControlToValidate="txtCCPC"
                                    ErrorMessage="Part Code Column cannot be empty!" ValidationGroup="Save" meta:resourcekey="rfvCCPCResource1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize3" runat="server" Text="Quantity Column" meta:resourcekey="Localize3Resource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCCQty" runat="server" CssClass="number" meta:resourcekey="txtCCQtyResource1"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender TargetControlID="txtCCQty" FilterType="Numbers"
                                    ID="FilteredTextBoxExtender3" runat="server" Enabled="True">
                                </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="rfvCCQty" runat="server" ControlToValidate="txtCCQty"
                                    ErrorMessage="Quantity Column cannot be empty!" ValidationGroup="Save" meta:resourcekey="rfvCCQtyResource1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize6" runat="server" Text="Commnent column" meta:resourcekey="Localize6Resource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCCComment" runat="server" CssClass="number" meta:resourcekey="txtCCCommentResource1"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender TargetControlID="txtCCComment" FilterType="Numbers"
                                    ID="FilteredTextBoxExtender6" runat="server" Enabled="True">
                                </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtCCComment"
                                    ErrorMessage="Commnent column cannot be empty!" ValidationGroup="Save" meta:resourcekey="RequiredFieldValidator4Resource1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </div>
                <%--thong tin linh kien--%>
                <div class="form" style="width: 450px">
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                                <h5>
                                    <asp:Literal ID="Literal24" runat="server" Text="Part spec Excel file" meta:resourcekey="Literal23Resource1"></asp:Literal></HeaderTemplate>
                                </h5>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <asp:Localize ID="Localize9" runat="server" Text="Excel Start Row:" meta:resourcekey="Localize9Resource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPSStart" runat="server" meta:resourcekey="txtOESRResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize10" runat="server" Text="Part Code Column" meta:resourcekey="Localize10Resource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPSPartCode" runat="server" meta:resourcekey="txtOEPCCResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize11" runat="server" Text="Packing Column" meta:resourcekey="Localize11Resource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPSPack" runat="server" meta:resourcekey="txtOEQCResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize12" runat="server" Text="Unit Column" meta:resourcekey="Localize12Resource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPSUnit" runat="server" meta:resourcekey="txtOEMCResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize13" runat="server" Text="Quantity Column" meta:resourcekey="Localize13Resource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPSQuantity" runat="server" meta:resourcekey="txtOEMCResource1"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <%--thay the linh kien--%>
                <div class="form" style="width: 450px">
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                                <h5>
                                    <asp:Literal ID="Literal23" runat="server" Text="Part replace Excel file" meta:resourcekey="Literal24Resource1"></asp:Literal></HeaderTemplate>
                                </h5>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <asp:Localize ID="Localize4" runat="server" Text="Excel Start Row:" meta:resourcekey="Localize4Resource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtREStart" runat="server" meta:resourcekey="txtOESRResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize5" runat="server" Text="Old Part Code Column" meta:resourcekey="Localize5Resource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtREOldPart" runat="server" meta:resourcekey="txtOEPCCResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize7" runat="server" Text="New Part Code Column" meta:resourcekey="Localize7Resource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRENewPart" runat="server" meta:resourcekey="txtOEQCResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize8" runat="server" Text="Status Column" meta:resourcekey="Localize8Resource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtREStatus" runat="server" meta:resourcekey="txtOEMCResource1"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="banks" runat="server">
            <HeaderTemplate>
                <asp:Literal ID="Literal22" runat="server" Text="Payment Excel Structure" meta:resourcekey="t2Resource1xxx"></asp:Literal></HeaderTemplate>
            <ContentTemplate>
                <asp:ValidationSummary ID="ValidationSummary2" ValidationGroup="NewBank" CssClass="error"
                    runat="server" />
                <br />
                <div class="grid">
                    <vdms:EmptyGridViewEx ID="gvBanks" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                        AllowInsertEmptyRow="False" GennerateSpanDataTable="False" IncludeChildsListInLevel="False"
                        RealPageSize="10" ShowEmptyFooter="True" ShowEmptyTable="True">
                        <Columns>
                            <asp:TemplateField HeaderText="Bank Code">
                                <FooterTemplate>
                                    <asp:TextBox Width="60px" ID="txtNewBankCode" runat="server"></asp:TextBox>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="60px" ID="txtBankCode" runat="server" Text='<%# Bind("BankCode") %>'
                                        OnTextChanged="UpdateBank" ToolTip='<%# Bind("BankId") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Bank name">
                                <FooterTemplate>
                                    <asp:TextBox Width="130px" ID="txtNewBankName" runat="server"></asp:TextBox>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="130px" ID="txtBankName" runat="server" Text='<%# Bind("BankName") %>'
                                        OnTextChanged="UpdateBank" ToolTip='<%# Bind("BankId") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date format">
                                <FooterTemplate>
                                    <asp:TextBox Width="100px" ID="txtNewDateFormat" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator Text="*" ValidationGroup="NewBank" ControlToValidate="txtNewDateFormat"
                                        ID="RequiredFieldValidator6" runat="server" ErrorMessage="Paymment date format cannot be blank!"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="100px" ID="txtDateFormat" runat="server" Text='<%# Bind("DateFormat") %>'
                                        OnTextChanged="UpdateBank" ToolTip='<%# Bind("BankId") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator Text="*" ValidationGroup="Save" ControlToValidate="txtDateFormat"
                                        ID="RequiredFieldValidator6" runat="server" ErrorMessage="Paymment date format cannot be blank!"></asp:RequiredFieldValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Start row">
                                <FooterTemplate>
                                    <asp:TextBox Width="30px" ID="txtNewStartRow" runat="server"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="txtNewStartRow_FilteredTextBoxExtender"
                                        FilterType="Numbers" runat="server" Enabled="True" TargetControlID="txtNewStartRow">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="30px" ID="txtStartRow" runat="server" OnTextChanged="UpdateBank"
                                        ToolTip='<%# Bind("BankId") %>' Text='<%# Bind("StartRow") %>'></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="txtStartRow_FilteredTextBoxExtender" FilterType="Numbers"
                                        runat="server" Enabled="True" TargetControlID="txtStartRow">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dealer code">
                                <FooterTemplate>
                                    <asp:TextBox Width="30px" ID="txtNewDealerCode" runat="server"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="txtNewDealerCode_FilteredTextBoxExtender"
                                        FilterType="Numbers" runat="server" Enabled="True" TargetControlID="txtNewDealerCode">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="30px" ID="txtDealerCode" runat="server" OnTextChanged="UpdateBank"
                                        ToolTip='<%# Bind("BankId") %>' Text='<%# Bind("DealerCode") %>'></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="txtDealerCode_FilteredTextBoxExtender" FilterType="Numbers"
                                        runat="server" Enabled="True" TargetControlID="txtDealerCode">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dealer name">
                                <FooterTemplate>
                                    <asp:TextBox Width="30px" ID="txtNewDealerName" runat="server"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="txtNewDealerName_FilteredTextBoxExtender"
                                        FilterType="Numbers" runat="server" Enabled="True" TargetControlID="txtNewDealerName">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="30px" ID="txtDealerName" runat="server" OnTextChanged="UpdateBank"
                                        ToolTip='<%# Bind("BankId") %>' Text='<%# Bind("DealerName") %>'></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="txtDealerName_FilteredTextBoxExtender" FilterType="Numbers"
                                        runat="server" Enabled="True" TargetControlID="txtDealerName">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order number">
                                <FooterTemplate>
                                    <asp:TextBox Width="30px" ID="txtNewOrderNumber" runat="server"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="txtNewOrderNumber_FilteredTextBoxExtender"
                                        FilterType="Numbers" runat="server" Enabled="True" TargetControlID="txtNewOrderNumber">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="30px" ID="txtOrderNumber" runat="server" OnTextChanged="UpdateBank"
                                        ToolTip='<%# Bind("BankId") %>' Text='<%# Bind("OrderId") %>'></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="txtOrderNumber_FilteredTextBoxExtender"
                                        FilterType="Numbers" runat="server" Enabled="True" TargetControlID="txtOrderNumber">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount">
                                <FooterTemplate>
                                    <asp:TextBox Width="30px" ID="txtNewAmount" runat="server"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="txtNewAmount_FilteredTextBoxExtender" FilterType="Numbers"
                                        runat="server" Enabled="True" TargetControlID="txtNewAmount">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="30px" OnTextChanged="UpdateBank" ToolTip='<%# Bind("BankId") %>'
                                        ID="txtAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="txtAmount_FilteredTextBoxExtender" FilterType="Numbers"
                                        runat="server" Enabled="True" TargetControlID="txtAmount">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Payment date">
                                <FooterTemplate>
                                    <asp:TextBox Width="30px" ID="txtNewPaymentDate" runat="server"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="txtNewPaymentDate_FilteredTextBoxExtender"
                                        FilterType="Numbers" runat="server" Enabled="True" TargetControlID="txtNewPaymentDate">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="30px" ID="txtPaymentDate" runat="server" OnTextChanged="UpdateBank"
                                        ToolTip='<%# Bind("BankId") %>' Text='<%# Bind("PaymentDate") %>'></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="txtPaymentDate_FilteredTextBoxExtender"
                                        FilterType="Numbers" runat="server" TargetControlID="txtPaymentDate">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Transaction">
                                <FooterTemplate>
                                    <asp:TextBox Width="30px" ID="txtNewTransNumber" runat="server"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="txtNewTransNumber_FilteredTextBoxExtender"
                                        FilterType="Numbers" runat="server" Enabled="True" TargetControlID="txtNewTransNumber">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="30px" ID="txtTransNumber" runat="server" OnTextChanged="UpdateBank"
                                        ToolTip='<%# Bind("BankId") %>' Text='<%# Bind("TransactionNumber") %>'></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="txtTransNumber_FilteredTextBoxExtender"
                                        FilterType="Numbers" runat="server" Enabled="True" TargetControlID="txtTransNumber">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comment">
                                <FooterTemplate>
                                    <asp:TextBox Width="30px" ID="txtNewComment" runat="server"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="txtNewComment_FilteredTextBoxExtender" FilterType="Numbers"
                                        runat="server" Enabled="True" TargetControlID="txtNewComment">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="30px" ID="txtComment" runat="server" Text='<%# Bind("Comment") %>'
                                        OnTextChanged="UpdateBank" ToolTip='<%# Bind("BankId") %>'></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="txtComment_FilteredTextBoxExtender" FilterType="Numbers"
                                        runat="server" Enabled="True" TargetControlID="txtComment">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <FooterTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="true" ValidationGroup="NewBank"
                                        CommandName="" ImageUrl="~/Images/update.gif" OnClick="ImageButton1_Click" Text="Button" />
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbDelBank" runat="server" CausesValidation="False" CommandArgument='<%# Bind("BankId") %>'
                                        ImageUrl="~/Images/Delete.gif" OnClick="imbDelBank_Click" Text="Button" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </vdms:EmptyGridViewEx>
                </div>
                <br />
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="vehicles" runat="server">
            <HeaderTemplate>
                <asp:Literal ID="Literal25" runat="server" Text="Vehicle sale" meta:resourcekey="tSaleResource"></asp:Literal></HeaderTemplate>
            <ContentTemplate>
                <div class="form" style="width: 450px">
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                                <h5>
                                    <asp:Literal ID="Literal27" runat="server" Text="Sale vehicle excel file" meta:resourcekey="Literal27Resource1"></asp:Literal></HeaderTemplate>
                                </h5>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <asp:Localize ID="Localize14" runat="server" Text="Excel Start Row:"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVStartRow" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize41" runat="server" Text="Date format"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVDateFormat" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize15" runat="server" Text="Engine Number Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVEngineNo" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize16" runat="server" Text="Bill Number Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVBillNo" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize18" runat="server" Text="Sell Date Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVSellDate" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize19" runat="server" Text="Price Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVPrice" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize21" runat="server" Text="Payment Type Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVPaymentType" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize22" runat="server" Text="Number Plate Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVNumberPlate" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize23" runat="server" Text="Sell Type Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVSellType" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize24" runat="server" Text="Payment Date Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVPaymentDate" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize25" runat="server" Text="Number Plate Receive Date Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVNumberPlateDate" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize26" runat="server" Text="Comment sell item Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVCommentItem" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize42" runat="server" Text="Instalment times"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVInstalmentTimes" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize43" runat="server" Text="First instalment amount"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVFirstInstalmentAmount" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize44" runat="server" Text="DaysEachInstalment"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVDaysEachInstalment" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize45" runat="server" Text="PayingDate1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVPayingDate1" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize50" runat="server" Text="Amount1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVAmount1" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize46" runat="server" Text="PayingDate2"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVPayingDate2" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize51" runat="server" Text="Amount2"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVAmount2" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize47" runat="server" Text="PayingDate3"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVPayingDate3" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize52" runat="server" Text="Amount3"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVAmount3" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize48" runat="server" Text="PayingDate4"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVPayingDate4" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize53" runat="server" Text="Amount4"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVAmount4" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize49" runat="server" Text="PayingDate5"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVPayingDate5" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize54" runat="server" Text="Amount5"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVAmount5" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize27" runat="server" Text="Customer Id column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVCusId" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize28" runat="server" Text="Customer name Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVCusName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize29" runat="server" Text="Gender Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVGender" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize30" runat="server" Text="DOB Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVDOB" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize31" runat="server" Text="Telephone Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVTel" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize32" runat="server" Text="Mobile Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVMobile" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize33" runat="server" Text="Address Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVAddress" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize34" runat="server" Text="Province Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVProvince" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize35" runat="server" Text="District Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVDistrict" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize36" runat="server" Text="Job type Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVJobType" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize37" runat="server" Text="Email column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVEmail" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize38" runat="server" Text="Precint Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVPrecint" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize39" runat="server" Text="Customer Description Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVCusDesc" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize40" runat="server" Text="Priority Column"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVPriority" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="bonus" runat="server">
            <HeaderTemplate>
                <asp:Literal ID="Literal28" runat="server" Text="Bonus data"></asp:Literal></HeaderTemplate>
            <ContentTemplate>
                <div class="form" style="width: 450px">
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                                <h5>
                                    <asp:Literal ID="Literal29" runat="server" Text="Bonus data excel file"></asp:Literal></HeaderTemplate>
                                </h5>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <asp:Localize ID="Localize17" runat="server" Text="Excel Start Row:"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBStartRow" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize20" runat="server" Text="Date format"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBDateFormat" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize55" runat="server" Text="Bonus plan"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBPlan" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize61" runat="server" Text="Dealer code"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBDealerCode" runat="server"></asp:TextBox>
                            </td>
                        </tr>                        
                        <tr>
                            <td>
                                <asp:Localize ID="Localize56" runat="server" Text="Bonus date"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBDate" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize57" runat="server" Text="Bonus source"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBSource" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize58" runat="server" Text="Amount"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBAmount" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize59" runat="server" Text="Plan month"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBStatus" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize60" runat="server" Text="Description"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBDesc" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="TabPanel1" runat="server">
            <HeaderTemplate>
                <asp:Literal ID="Literal30" runat="server" Text="Warranty part"></asp:Literal></HeaderTemplate>
            <ContentTemplate>
                <div class="form" style="width: 450px">
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                                <h5>
                                    <asp:Literal ID="Literal31" runat="server" Text="Warranty part excel file"></asp:Literal></HeaderTemplate>
                                </h5>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <asp:Localize ID="Localize62" runat="server" Text="Excel Start Row:"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWStartRow" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize63" runat="server" Text="Date format"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWDateFormat" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize64" runat="server" Text="Part code"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWPartCode" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize65" runat="server" Text="Vietnamese name"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWPartNameVN" runat="server"></asp:TextBox>
                            </td>
                        </tr>                        
                        <tr>
                            <td>
                                <asp:Localize ID="Localize66" runat="server" Text="English name"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWPartNameEN" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize67" runat="server" Text="Motor code"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWMotorCode" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize68" runat="server" Text="Warranty time"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWWarrantyTime" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize69" runat="server" Text="Warranty length"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWWarrantyLength" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize70" runat="server" Text="Start date"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWStartDate" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize71" runat="server" Text="Stop date"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWStopDate" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
    <div class="button">
        <asp:Button ID="cmdSave" runat="server" SkinID="SubmitButton" Text="Save setting"
            ValidationGroup="Save" OnClick="cmdSave_Click" meta:resourcekey="cmdSaveResource1" />
    </div>
    <asp:Label ID="lblSaveOk" runat="server" SkinID="MessageOk" Visible="False" Text="The system setting has been save successful."
        meta:resourcekey="lblSaveOkResource1"></asp:Label>
    <asp:Label ID="lblSaveError" runat="server" SkinID="MessageError" Visible="False"
        Text="The system setting has been failed to save." meta:resourcekey="lblSaveErrorResource1"></asp:Label>

    <script language="javascript" type="text/javascript">
        function reloadState() {
            $('#<%=btnRefresh.ClientID %>').click();
            setTimeout("reloadState()", 1000);
        }
        //setTimeout("reloadState()", 1000);
    </script>

</asp:Content>
