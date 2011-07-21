<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true" CodeFile="WarrantyContento.aspx.cs"
    Inherits="Service_WarrantyContent" Title="Biểu ghi chép nội dung bảo hành" Culture="auto"
    UICulture="auto" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/Controls/Services/AddExchange.ascx" TagName="AddExchange" TagPrefix="uc2" %>
<%@ Register Assembly="VDMS.Common" Namespace="VDMS.Common.Web" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <table class="forceNowrap" style="width: 100%">
        <tr>
            <td class="forceNowrap" style="width: 1%">
                <asp:Literal ID="Literal1" runat="server" Text="<H2>Service Record Sheet</H2>" meta:resourcekey="Literal1Resource1"></asp:Literal>
            </td>
            <td style="width: 1%">
                &nbsp;&nbsp; -&nbsp;
            </td>
            <td>
                <asp:HyperLink ID="lnkNewSheet" runat="server" NavigateUrl="~/Service/WarrantyContent.aspx"
                    meta:resourcekey="lnkNewSheetResource1" Target="_blank" Text="Add new sheet"></asp:HyperLink>
            </td>
        </tr>
    </table>
    <asp:BulletedList ID="bllErrorMsg" runat="server" CssClass="errorMsg" meta:resourcekey="bllErrorMsgResource1">
    </asp:BulletedList>
    <asp:Label runat="server" ID="lbCustomerFullName" CssClass="hidden" meta:resourcekey="lbCustomerFullNameResource1"></asp:Label>
    <input id="_PageStatus" runat="server" enableviewstate="true" type="hidden" value="Insertnew" />
    <input id="_InvoiceID" runat="server" enableviewstate="true" type="hidden" value="-1" />
    <input id="_CustomerID" runat="server" enableviewstate="true" type="hidden" value="-1" />
    <input id="_ItemInstanceID" runat="server" enableviewstate="true" type="hidden" value="-1" />
    <input id="_CustomerAction" runat="server" enableviewstate="true" type="hidden" value="Reset" />
    <input id="ddlSex" runat="server" enableviewstate="true" type="hidden" value="0" />
    <input id="txtBirthDate" runat="server" enableviewstate="true" type="hidden" value="-1" />
    <input id="txtAddress" runat="server" enableviewstate="true" type="hidden" value="-1" />
    <input id="ddlProvince" runat="server" enableviewstate="true" type="hidden" value="0" />
    <input id="txtDistrict" runat="server" enableviewstate="true" type="hidden" value="-1" />
    <input id="tblCus_JobType" runat="server" enableviewstate="true" type="hidden" value="0" />
    <input id="txtCEmail" runat="server" enableviewstate="true" type="hidden" value="-1" />
    <input id="txtCPhone" runat="server" enableviewstate="true" type="hidden" value="-1" />
    <input id="txtCMobile" runat="server" enableviewstate="true" type="hidden" value="-1" />
    <input id="ddlCus_SetType" runat="server" enableviewstate="true" type="hidden" value="0" />
    <input id="ddlCusType" runat="server" enableviewstate="true" type="hidden" value="0" />
    <input id="txtCus_Desc" runat="server" enableviewstate="true" type="hidden" value="-1" />
    <input id="txtPrecinct" runat="server" enableviewstate="true" type="hidden" value="-1" />
    <input id="_CustomerFullName" runat="server" enableviewstate="true" type="hidden" />

    <script language="javascript" type="text/javascript">
		function retrieve_lookup_data(CustomerObj, hrefURL) {
			var objCustomerID = document.getElementById(CustomerObj);
			var objEngineNo = document.getElementById('<%= hdEngineNumber.ClientID %>');
			var objMotorType = document.getElementById('<%= txtModel.ClientID %>');
			var objCusId = document.getElementById('<%= _CustomerID.ClientID %>');

			if ((objEngineNo.value.trim() == "") || (objMotorType.value.trim() == "")) {
				alert('<%= LoadCustomerErr %>');
				return false;
			}

			hrefURL += "cusid=" + objCusId.value;
			hrefURL += "&IsWarranty=true";
			hrefURL += "&engineno=" + objEngineNo.value.trim();
			hrefURL += "&motortype=" + objMotorType.value.trim();
			hrefURL += "&action=";
			if (objCusId.value == "") {
				hrefURL += "insert";
			}
			else {
				hrefURL += "edit";
			}

			var arrParams = new Array();
			arrParams[0] = window;
			arrParams[1] = 'ctl00_cphMain__CustomerID';
			arrParams[2] = 'ctl00_cphMain_txtCustId';
			arrParams[3] = 'ctl00_cphMain__CustomerAction';
			arrParams[4] = 'ctl00_cphMain_ddlSex';
			arrParams[5] = 'ctl00_cphMain_txtBirthDate';
			arrParams[6] = 'ctl00_cphMain_txtAddress';
			arrParams[7] = 'ctl00_cphMain_ddlProvince';
			arrParams[8] = 'ctl00_cphMain_txtDistrict';
			arrParams[9] = 'ctl00_cphMain_tblCus_JobType';
			arrParams[10] = 'ctl00_cphMain_txtCEmail';
			arrParams[11] = 'ctl00_cphMain_txtCPhone';
			arrParams[12] = 'ctl00_cphMain_txtCMobile';
			arrParams[13] = 'ctl00_cphMain_ddlCus_SetType';
			arrParams[14] = 'ctl00_cphMain_ddlCusType';
			arrParams[15] = 'ctl00_cphMain_txtCus_Desc';
			arrParams[16] = 'ctl00_cphMain_txtPrecinct';
			arrParams[17] = 'ctl00_cphMain_lbCustomerFullName';
			arrParams[18] = 'ctl00_cphMain__CustomerFullName';

			var objCustomerAction = document.getElementById('ctl00_cphMain__CustomerAction');
			objCustomerAction.value = "Reset";

			var objDataArray = window.showModalDialog(hrefURL, arrParams, "status:false;dialogWidth:750px;dialogHeight:500px");

			if (objCustomerAction.value == 'Reset') {
				return false;
			}
			else {
				return true;
			}
		}
        
    </script>

    <script language="javascript" type="text/javascript">
<!--
		function SubmitConfirm(obj, msg) {
			return confirm(msg)
			var ok = confirm(msg)
			if (ok) { obj.disabled = 'disabled' }
			return ok
		}

-->
    </script>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="vwMain" runat="server">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Save"
                meta:resourcekey="ValidationSummary1Resource1" />
            <asp:ValidationSummary ID="ValidationSummary5" runat="server" ValidationGroup="EditSpare"
                meta:resourcekey="ValidationSummary5Resource1" />
            <asp:ValidationSummary ID="ValidationSummary7" runat="server" ValidationGroup="SaveTemp"
                meta:resourcekey="ValidationSummary5Resource1" />
            <asp:ValidationSummary ID="ValidationSummary8" runat="server" ValidationGroup="None"
                meta:resourcekey="ValidationSummary5Resource1" />
            <table border="0" cellpadding="2" cellspacing="1" style="width: 600px" class="InputTable">
                <tr>
                    <td class="nameField">
                    </td>
                    <td class="nameField">
                    </td>
                    <td colspan="4">
                    </td>
                    <td class="rightObj" colspan="2">
                        <asp:TextBox ID="txtCustId" runat="server" Width="186px" CssClass="hidden" meta:resourcekey="txtCustIdResource1"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="nameField">
                    </td>
                    <td class="nameField">
                    </td>
                    <td colspan="4">
                        <asp:Button ID="btnHistory" runat="server" Text="Repair history" Width="127px" meta:resourcekey="btnHistoryResource1" />
                        <asp:Button ID="btnList" runat="server" Text="Repair list" Width="141px" meta:resourcekey="btnListResource1" />
                    </td>
                    <td colspan="2">
                        <asp:Button ID="btnNewCus" runat="server" Text="Add new customer" Width="189px" OnClick="btnNewCus_Click"
                            meta:resourcekey="btnNewCusResource1" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="nameField">
                    </td>
                    <td class="nameField">
                        &nbsp;<asp:Literal ID="Literal38" runat="server" meta:resourcekey="Literal3x2Resource"
                            Text="Dealer code:"></asp:Literal>
                    </td>
                    <td colspan="6">
                        <asp:DropDownList EnableViewState="true" ID="ddlDealer" runat="server" Width="100%"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlDealer_SelectedIndexChanged" OnDataBound="ddlDealer_DataBound"
                            meta:resourcekey="ddlDealerResource1">
                        </asp:DropDownList>
                        <asp:TextBox ID="txtDealerCode" runat="server" Visible="False" meta:resourcekey="txtDealerCodeResource1"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="nameField">
                    </td>
                    <td class="nameField">
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td class="nameField">
                        <asp:Literal ID="Literal36" runat="server" Text="Branch code:" meta:resourcekey="Literal3xResource1"></asp:Literal>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlBranchCode" runat="server" Width="190px" meta:resourcekey="ddlBranchCodeResource1">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rqvBranchCode" runat="server" ControlToValidate="ddlBranchCode"
                            meta:resourcekey="rqvBranch1" ErrorMessage='Dữ liệu &quot;Số m&#225;y&quot; kh&#244;ng được để trống'
                            SetFocusOnError="True" Text="*" ValidationGroup="Save" Visible="False"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="nameField">
                    </td>
                    <td class="nameField">
                        <asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
                        <asp:Literal ID="Literal2" runat="server" Text="Engine number:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                    </td>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="inputKeyField">
                                    <asp:TextBox ID="txtEngineNo" runat="server" Width="159px" MaxLength="20" meta:resourcekey="txtEngineNoResource1"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rqvEngineNumber" runat="server" SetFocusOnError="True"
                                        ValidationGroup="Save" ControlToValidate="txtEngineNo" ErrorMessage='Dữ liệu &quot;Số m&#225;y&quot; kh&#244;ng được để trống'
                                        Text="*" meta:resourcekey="rqvEngineNumberResource1"></asp:RequiredFieldValidator>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btnCheck" runat="server" Text="..." OnClick="btnCheck_Click" Width="27px"
                                        meta:resourcekey="btnCheckResource1" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td class="nameField">
                        <asp:Literal ID="Literal3" runat="server" meta:resourcekey="Literal3Resource1" Text="Sheet number:"></asp:Literal>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtSheetNo" runat="server" Width="186px" ReadOnly="True" CssClass="readOnlyInputField"
                            meta:resourcekey="txtSheetNoResource1"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="nameField">
                    </td>
                    <td class="nameField">
                        <asp:Image ID="Image7" runat="server" SkinID="RequireField" meta:resourcekey="Image7Resource1" />
                        <asp:Literal ID="Literal4" runat="server" Text="Customer name:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtCusName" runat="server" Width="250px" ReadOnly="True" CssClass="readOnlyInputField"
                            meta:resourcekey="txtCusNameResource1"></asp:TextBox>
                    </td>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr>
                                <td>
                                    <asp:RequiredFieldValidator ID="rqvCustName" runat="server" ControlToValidate="txtCusName"
                                        ErrorMessage='&quot;Customer name&quot; cannot be blank!' ValidationGroup="Save"
                                        Text="*" meta:resourcekey="rqvCustNameResource1"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Button ID="Button4" runat="server" OnClick="Button4_Click" Text="..." Width="25px"
                                        meta:resourcekey="Button4Resource2" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="nameField">
                        <asp:Literal ID="Literal5" runat="server" Text="Phone:" meta:resourcekey="Literal5Resource1"></asp:Literal>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtPhoneNo" runat="server" Width="186px" ReadOnly="True" CssClass="readOnlyInputField"
                            meta:resourcekey="txtPhoneNoResource1"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="nameField">
                    </td>
                    <td class="nameField">
                        <asp:Literal ID="Literal6" runat="server" Text="Email:" meta:resourcekey="Literal6Resource1"></asp:Literal>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtEmail" runat="server" Width="250px" ReadOnly="True" CssClass="readOnlyInputField"
                            meta:resourcekey="txtEmailResource1"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td class="nameField">
                        <asp:Image ID="Image1" runat="server" SkinID="RequireField" meta:resourcekey="Image1Resource1" />
                        <asp:Literal ID="Literal7" runat="server" Text="Buy date:" meta:resourcekey="Literal7Resource1"></asp:Literal>
                    </td>
                    <td style="width: 1px">
                        <asp:TextBox ID="txtBuyDate" runat="server" Width="161px" meta:resourcekey="txtBuyDateResource1"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtBuyDate"
                            PopupButtonID="ibtnCalendar" BehaviorID="CalendarExtender3" Enabled="True">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtBuyDate"
                            Mask="99/99/9999" MaskType="Date" BehaviorID="MaskedEditExtender2" Enabled="True"
                            CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY"
                            CultureDatePlaceholder="/" CultureDecimalPlaceholder="." CultureThousandsPlaceholder=","
                            CultureTimePlaceholder=":" />
                    </td>
                    <td>
                        <asp:ImageButton ID="ibtnCalendar" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
                            meta:resourcekey="ibtnCalendarResource1" />
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rqvBuydate" runat="server" ControlToValidate="txtBuyDate"
                            ErrorMessage='&quot;By date&quot; cannot be blank!' ValidationGroup="Save" Text="*"
                            meta:resourcekey="rqvBuydateResource1"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="nameField">
                    </td>
                    <td class="nameField">
                        <asp:Literal ID="Literal10" runat="server" Text="Address:" meta:resourcekey="Literal10Resource1"></asp:Literal>
                    </td>
                    <td colspan="6">
                        <asp:TextBox ID="txtCustAddress" runat="server" Width="99%" ReadOnly="True" CssClass="readOnlyInputField"
                            meta:resourcekey="txtAddressResource1"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="nameField">
                    </td>
                    <td class="nameField">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td class="nameField">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="nameField">
                    </td>
                    <td class="nameField">
                        <asp:Image ID="Image8" runat="server" SkinID="RequireField" meta:resourcekey="Image8Resource1" />
                        <asp:Literal ID="Literal12" runat="server" Text="Model:" meta:resourcekey="Literal12Resource1"></asp:Literal>
                    </td>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="inputKeyField">
                                    <asp:TextBox ID="txtModel" runat="server" Width="159px" MaxLength="20" meta:resourcekey="txtModelResource1"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtModel"
                                        ErrorMessage='&quot;Model&quot; cannot be blank!' ValidationGroup="Save" Text="*"
                                        meta:resourcekey="RequiredFieldValidator8Resource1"></asp:RequiredFieldValidator>
                                    <asp:Button ID="btnCheckModel" runat="server" Text="..." OnClick="btnCheckModel_Click"
                                        Width="27px" meta:resourcekey="btnCheckModelResource1" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td class="nameField">
                        <asp:Literal ID="Literal13" runat="server" Text="Color:" meta:resourcekey="Literal13Resource1"></asp:Literal>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlColour" runat="server" Width="191px" DataTextField="ColorName"
                            DataValueField="ColorCode" OnDataBound="ddlColour_DataBound" meta:resourcekey="ddlColourResource1">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rqvColor" runat="server" ControlToValidate="ddlColour"
                            ErrorMessage="Color cannot be blank!" meta:resourcekey="rqvColorResource1" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="nameField">
                    </td>
                    <td class="nameField">
                        <asp:Literal ID="Literal11" runat="server" Text="Plate number:" meta:resourcekey="Literal11Resource1"></asp:Literal>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtNumberPlate" runat="server" Width="159px" MaxLength="12" meta:resourcekey="txtNumberPlateResource1"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td class="nameField">
                        <asp:Literal ID="Literal9" runat="server" Text="Frame number:" meta:resourcekey="Literal9Resource1"></asp:Literal>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtFrameNumber" runat="server" Width="186px" MaxLength="30" meta:resourcekey="txtFrameNumberResource1"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="nameField">
                    </td>
                    <td class="nameField">
                        <asp:Image ID="image14" runat="server" SkinID="RequireField" meta:resourcekey="image14Resource1" />
                        <asp:Literal ID="Literal14" runat="server" Text="Current kilometers:" meta:resourcekey="Literal14Resource1"></asp:Literal>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtKm" runat="server" Width="159px" MaxLength="15" meta:resourcekey="txtKmResource1"
                            CausesValidation="True"></asp:TextBox><asp:RegularExpressionValidator ID="rgvKm"
                                runat="server" SetFocusOnError="True" ValidationGroup="Save" ValidationExpression="\s*\d{1,15}\s*"
                                ControlToValidate="txtKm" ErrorMessage='Dữ liệu &quot;Số Km&quot; kh&#244;ng hợp lệ (y&#234;u cầu chỉ nhập chữ số 0-9 )'
                                Text="*" meta:resourcekey="rgvKmResource1"></asp:RegularExpressionValidator><asp:CompareValidator
                                    ID="CompareValidator1" runat="server" ControlToCompare="txtLastKm" ControlToValidate="txtKm"
                                    ErrorMessage="Current km less than previous value" meta:resourcekey="CompareValidator1Resource1"
                                    SetFocusOnError="True" Type="Double" ValidationGroup="None" Operator="GreaterThan"></asp:CompareValidator><asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtKm" ErrorMessage='Dữ liệu &quot;Km&quot; kh&#244;ng được để trống'
                                        SetFocusOnError="True" ValidationGroup="Save" Text="*" meta:resourcekey="RequiredFieldValidator5Resource1"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                    <td class="nameField">
                        <asp:Literal ID="Literal30" runat="server" Text="Last kilometers:" meta:resourcekey="Literal30Resource1"></asp:Literal>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtLastKm" runat="server" Width="186px" ReadOnly="True" CssClass="readOnlyInputField"
                            meta:resourcekey="txtLastKmResource1"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="nameField">
                    </td>
                    <td class="nameField">
                        <asp:Image ID="Image18" runat="server" SkinID="RequireField" meta:resourcekey="Image18Resource1" />
                        <asp:Literal ID="Literal15" runat="server" Text="Service type:" meta:resourcekey="Literal15Resource1"></asp:Literal>
                    </td>
                    <td colspan="3">
                        <asp:CheckBoxList ID="chblSerList" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                            meta:resourcekey="chblSerListResource1">
                            <asp:ListItem Value="5" Text="Maintain" meta:resourcekey="ListItemResource1"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Repair" meta:resourcekey="ListItemResource2"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Warranty" meta:resourcekey="ListItemResource3"></asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                    <td class="nameField">
                        <asp:Image ID="Image17" runat="server" SkinID="RequireField" meta:resourcekey="Image17Resource1" />
                        <asp:Literal ID="Literal8" runat="server" Text="Repair date:" meta:resourcekey="Literal8Resource1"></asp:Literal>
                    </td>
                    <td style="width: 1px">
                        <asp:TextBox ID="txtRepairDate" runat="server" Width="161px" meta:resourcekey="txtRepairDateResource1"></asp:TextBox><ajaxToolkit:CalendarExtender
                            ID="CalendarExtender2" runat="server" BehaviorID="CalendarExtender2" Enabled="True"
                            PopupButtonID="ibtnCalendarR" TargetControlID="txtRepairDate">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" BehaviorID="MaskedEditExtender1"
                            Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtRepairDate"
                            CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY"
                            CultureDatePlaceholder="/" CultureDecimalPlaceholder="." CultureThousandsPlaceholder=","
                            CultureTimePlaceholder=":">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                    <td>
                        <asp:ImageButton ID="ibtnCalendarR" runat="server" OnClientClick="return false;"
                            SkinID="CalendarImageButton" meta:resourcekey="ibtnCalendarRResource1" />
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rqvRepairDate" runat="server" ControlToValidate="txtRepairDate"
                            ErrorMessage='Dữ liệu &quot;Ng&#224;y sua&quot; kh&#244;ng được để trống' SetFocusOnError="True"
                            Text="*" ValidationGroup="Save" meta:resourcekey="rqvRepairDateResource1"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="nameField">
                    </td>
                    <td class="nameField">
                        <asp:Image ID="image5" runat="server" SkinID="RequireField" meta:resourcekey="image5Resource1" />
                        <asp:Literal ID="Literal16" runat="server" Text="Damaged status:" meta:resourcekey="Literal16Resource1"></asp:Literal>
                    </td>
                    <td colspan="6">
                        <asp:TextBox ID="txtErrorStatus" runat="server" TextMode="MultiLine" Width="98%"
                            Rows="3" MaxLength="1000" meta:resourcekey="txtErrorStatusResource1"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True"
                            ValidationGroup="Save" ControlToValidate="txtErrorStatus" ErrorMessage='Dữ liệu &quot;T&#236;nh trạng hư hỏng&quot; kh&#244;ng được để trống'
                            Text="*" meta:resourcekey="RequiredFieldValidator2Resource1"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="nameField">
                        &nbsp;&nbsp;
                    </td>
                    <td class="nameField">
                        <asp:Literal ID="Literal17" runat="server" Text="Solution:" meta:resourcekey="Literal17Resource1"></asp:Literal>
                    </td>
                    <td colspan="6">
                        <asp:TextBox ID="txtRepair" runat="server" TextMode="MultiLine" Width="98%" Rows="4"
                            MaxLength="1000" meta:resourcekey="txtRepairResource1"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="nameField">
                    </td>
                    <td class="nameField">
                        &nbsp;
                    </td>
                    <td colspan="6">
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <br />
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                <tr>
                    <td class="rightObj">
                        <asp:Button ID="btnAdd" runat="server" Text="Add spare" Width="157px" OnClick="btnAdd_Click"
                            meta:resourcekey="btnAddResource1" />&nbsp;
                        <asp:Button ID="btnAddExchange" runat="server" Text="Add exchange spares" Width="170px"
                            OnClick="btnAddExchange_Click" meta:resourcekey="btnAddExchangeResource1" Enabled="False" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <cc1:EmptyGridViewEx ID="gvSpareList" runat="server" AllowInsertEmptyRow="False"
                            AutoGenerateColumns="False" CssClass="GridView" DataKeyNames="SpareNumber" OnRowCancelingEdit="gvSpareList_RowCancelingEdit"
                            OnRowCommand="gvSpareList_RowCommand" OnRowEditing="gvSpareList_RowEditing" OnRowUpdating="gvSpareList_RowUpdating"
                            ShowEmptyTable="True" ShowFooter="True" SubmitControlIdPrefix="txt" Width="100%"
                            OnDataBinding="gvSpareList_DataBinding" OnDataBound="gvSpareList_DataBound" OnLoad="gvSpareList_Load"
                            OnRowDataBound="gvSpareList_RowDataBound" OnRowDeleting="gvSpareList_RowDeleting"
                            meta:resourcekey="gvSpareListResource1" IncludeChildsListInLevel="False" RealPageSize="10"
                            GennerateSpanDataTable="False" ShowEmptyFooter="True">
                            <Columns>
                                <asp:TemplateField HeaderText="No" meta:resourcekey="TemplateFieldResource1">
                                    <EditItemTemplate>
                                        <asp:Label runat="server" ID="Label1" meta:resourceKey="Label1Resource1"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemStyle CssClass="centerObj" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="Label1x" meta:resourceKey="Label1Resource2"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Spare number" meta:resourcekey="TemplateFieldResource2">
                                    <EditItemTemplate>
                                        <asp:Literal runat="server" ID="Literal37x" Text='<%# Eval("SpareNumber") %>' __designer:wfdid="w33"></asp:Literal>
                                        <asp:Label runat="server" Text="*" CssClass="errorMsg" ID="lbInvalidSpareNumber"
                                            meta:resourceKey="lbInvalidSpareNumberResource1" __designer:wfdid="w34"></asp:Label>
                                        <asp:TextBox runat="server" Width="111px" MaxLength="35" CssClass="hidden" Text='<%# Bind("SpareNumber") %>'
                                            ID="txtSpareNumber" meta:resourceKey="txtSpareNumberResource1" __designer:wfdid="w35"></asp:TextBox>
                                        <asp:TextBox runat="server" CssClass="hidden" Text='<%# Eval("ItemId") %>' ID="txtItemId"
                                            meta:resourceKey="txtItemIdResource1" __designer:wfdid="w44"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:MultiView runat="server" ID="MultiView2" __designer:wfdid="w36" OnLoad="MultiView2_Load">
                                            <asp:View runat="server" ID="View1" __designer:wfdid="w37">
                                                &nbsp;<asp:Literal runat="server" ID="Literal22" Text="Spares amount(VND)" meta:resourceKey="Literal22Resource1"
                                                    __designer:wfdid="w38"></asp:Literal>
                                            </asp:View>
                                            <asp:View runat="server" ID="View2" __designer:wfdid="w39">
                                                <asp:TextBox runat="server" Width="111px" MaxLength="35" Text='<%# Bind("SpareNumber") %>'
                                                    ID="txtSpareNumber" meta:resourceKey="txtSpareNumberResource2" __designer:wfdid="w40"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ErrorMessage="&quot;Spare number&quot; cannot be blank!"
                                                    ControlToValidate="txtSpareNumber" Text="*" ValidationGroup="EditSpare" ID="RequiredFieldValidator10"
                                                    meta:resourceKey="RequiredFieldValidator10Resource1" __designer:wfdid="w41"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator runat="server" ErrorMessage="&quot;Spare number&quot; must input at least 6 non-white-space character (maximum is 35)"
                                                    ControlToValidate="txtSpareNumber" Text="*" ValidationGroup="EditSpare" ID="RegularExpressionValidator3"
                                                    ValidationExpression="\s*[\S|\s]{6,35}\s*" meta:resourceKey="RegularExpressionValidator3Resource2"
                                                    __designer:wfdid="w42"></asp:RegularExpressionValidator>
                                                <asp:Button runat="server" ID="btnFindSpare" Text="..." meta:resourceKey="btnFindSpareResource2"
                                                    __designer:wfdid="w43" OnClick="btnFindSpare_Click"></asp:Button>
                                            </asp:View>
                                        </asp:MultiView>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                    </FooterTemplate>
                                    <ItemStyle Wrap="False" CssClass="vCenterObj" />
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="Literal19" Text='<%# Eval("SpareNumber") %>' __designer:wfdid="w31"></asp:Literal>
                                        <asp:Label runat="server" Text="*" CssClass="errorMsg" ID="lbInvalidSpareNumber"
                                            meta:resourceKey="lbInvalidSpareNumberResource2" __designer:wfdid="w32"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Spare name" meta:resourcekey="TemplateFieldResource3">
                                    <EditItemTemplate>
                                        <asp:Literal runat="server" ID="litSpareName" Text='<%# Bind("SpareName") %>' __designer:wfdid="w127"></asp:Literal>
                                        <asp:TextBox runat="server" Text='<%# Bind("SpareName") %>' ID="txtSpareName" meta:resourceKey="txtSpareNameResource1"
                                            __designer:wfdid="w134"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:MultiView runat="server" ID="MultiViewx" __designer:wfdid="w128" OnLoad="MultiView2_Load">
                                            <asp:View runat="server" ID="View0" __designer:wfdid="w129">
                                                <asp:Literal runat="server" ID="litSparesAmount" Text='<%# Bind("SpareName") %>'
                                                    __designer:wfdid="w130"></asp:Literal>
                                            </asp:View>
                                            <asp:View runat="server" ID="Viewx" __designer:wfdid="w131">
                                                <asp:Literal runat="server" ID="litSpareName" meta:resourceKey="litSpareNameResource1"
                                                    __designer:wfdid="w132"></asp:Literal>
                                                <asp:TextBox runat="server" ID="txtSpareName" meta:resourceKey="txtSpareNameResource2"
                                                    __designer:wfdid="w133"></asp:TextBox>
                                            </asp:View>
                                        </asp:MultiView>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="Literal26" Text='<%# Eval("SpareName") %>' __designer:wfdid="w126"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity" meta:resourcekey="TemplateFieldResource4">
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" Width="35px" MaxLength="5" Text='<%# Bind("Quantity") %>'
                                            ID="txtQuantity" meta:resourceKey="txtQuantityResource1" __designer:wfdid="w150"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ErrorMessage="&quot;Quantity&quot; cannot be blank!"
                                            ControlToValidate="txtQuantity" Text="*" ValidationGroup="EditSpare" ID="RequiredFieldValidatorx110"
                                            meta:resourceKey="RequiredFieldValidatorx110Resource1" __designer:wfdid="w151"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator runat="server" ErrorMessage="&quot;Quantity&quot; must be numeric!"
                                            ControlToValidate="txtQuantity" Text="*" ValidationGroup="EditSpare" ID="Regular1ExpressionValidator2"
                                            ValidationExpression="\s*\d*\s*" meta:resourceKey="Regular1ExpressionValidator2Resource1"
                                            __designer:wfdid="w152"></asp:RegularExpressionValidator>
                                        <asp:RangeValidator runat="server" MinimumValue="1" ErrorMessage="&quot;Quantity&quot; must between 1 and 999!"
                                            ControlToValidate="txtQuantity" Text="*" Type="Integer" ValidationGroup="EditSpare"
                                            MaximumValue="999" ID="RangeValidator1" meta:resourceKey="RangeValidator1Resource1"
                                            __designer:dtid="8725724278030434" __designer:wfdid="w153"></asp:RangeValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:MultiView runat="server" ID="MultiView6" __designer:wfdid="w154" OnLoad="MultiView2_Load">
                                            <asp:View runat="server" ID="View3" __designer:wfdid="w155">
                                                <asp:Literal runat="server" ID="Literal23" Text="Fee amount" meta:resourceKey="Literal23Resource1"
                                                    __designer:wfdid="w156"></asp:Literal>
                                            </asp:View>
                                            <asp:View runat="server" ID="View4" __designer:wfdid="w157">
                                                <asp:TextBox runat="server" Width="35px" MaxLength="5" Text="1" ID="txtQuantity"
                                                    meta:resourceKey="txtQuantityResource2" __designer:wfdid="w158"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ErrorMessage="&quot;Quantity&quot; cannot be blank!"
                                                    ControlToValidate="txtQuantity" Text="*" ValidationGroup="EditSpare" ID="RequiredFieldValidatorx1210"
                                                    meta:resourceKey="RequiredFieldValidatorx1210Resource1" __designer:wfdid="w159"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator runat="server" ErrorMessage="&quot;Quantity&quot; must be numeric!"
                                                    ControlToValidate="txtQuantity" Text="*" ValidationGroup="EditSpare" ID="Regular2ExpressionValidator2"
                                                    ValidationExpression="\s*\d*\s*" meta:resourceKey="Regular2ExpressionValidator2Resource1"
                                                    __designer:wfdid="w160"></asp:RegularExpressionValidator>
                                                <asp:RangeValidator runat="server" MinimumValue="1" ErrorMessage="&quot;Quantity&quot; must between 1 and 999!"
                                                    ControlToValidate="txtQuantity" Text="*" Type="Integer" ValidationGroup="EditSpare"
                                                    MaximumValue="999" ID="RangeValidatoer1" meta:resourceKey="RangeValidator1Resource1"
                                                    __designer:dtid="8725724278030434" __designer:wfdid="w161"></asp:RangeValidator>
                                            </asp:View>
                                        </asp:MultiView>
                                    </FooterTemplate>
                                    <ItemStyle CssClass="centerObj" />
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="Literal20" Text='<%# Eval("Quantity") %>' __designer:wfdid="w149"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit price(VND)" meta:resourcekey="TemplateFieldResource5">
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" Width="112px" MaxLength="30" Text='<%# Bind("SpareCost") %>'
                                            ID="txtSpareCost" meta:resourceKey="txtSpareCostResource1" __designer:wfdid="w26"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ErrorMessage="&quot;Unit price&quot; cannot be blank!"
                                            ControlToValidate="txtSpareCost" Text="*" ValidationGroup="EditSpare" ID="RequiredFieldValidatorx6"
                                            meta:resourceKey="RequiredFieldValidatorx6Resource1" __designer:wfdid="w27"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator runat="server" ErrorMessage="&quot;Unit price&quot; must be numeric!"
                                            ControlToValidate="txtSpareCost" Text="*" ValidationGroup="EditSpare" ID="RegularExpressionValidatorx1"
                                            ValidationExpression="\s*\d*\s*" meta:resourceKey="RegularExpressionValidatorx1Resource1"
                                            __designer:wfdid="w28"></asp:RegularExpressionValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:MultiView runat="server" ID="MultiView5" __designer:wfdid="w29" OnLoad="MultiView2_Load">
                                            <asp:View runat="server" ID="View9" __designer:wfdid="w30">
                                                <asp:TextBox runat="server" Width="95px" MaxLength="30" ID="txtFee" meta:resourceKey="txtFeeResource1"
                                                    __designer:wfdid="w31" OnLoad="txtFee_Load"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ErrorMessage="&quot;Fee amount&quot; cannot be blank!"
                                                    Enabled="False" ControlToValidate="txtFee" Text="*" ValidationGroup="Save" ID="RequiredFieldValidatorx16"
                                                    Visible="False" EnableTheming="True" meta:resourceKey="RequiredFieldValidatorx16Resource1"
                                                    __designer:wfdid="w32"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator runat="server" ErrorMessage="&quot;Fee amount&quot; must be numeric!"
                                                    ControlToValidate="txtFee" Text="*" ValidationGroup="Save" ID="RegularExpressionValidator1"
                                                    ValidationExpression="\s*\d*\s*" meta:resourceKey="RegularExpressionValidator1Resource1"
                                                    __designer:wfdid="w33"></asp:RegularExpressionValidator>
                                            </asp:View>
                                            <asp:View runat="server" ID="View10" __designer:wfdid="w34">
                                                <asp:Literal runat="server" ID="litSpareCost" Text='<%# Bind("SpareCost") %>' __designer:wfdid="w35"></asp:Literal>
                                                <asp:TextBox runat="server" Width="109px" ID="txtSpareCost" meta:resourceKey="txtSpareCostResource2"
                                                    __designer:wfdid="w36"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ErrorMessage="&quot;Unit price&quot; cannot be blank!"
                                                    ControlToValidate="txtSpareCost" Text="*" ValidationGroup="EditSpare" ID="RequiredFieldValidatorx6"
                                                    meta:resourceKey="RequiredFieldValidatorx6Resource1" __designer:wfdid="w37"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator runat="server" ErrorMessage="&quot;Unit price&quot; must be numeric!"
                                                    ControlToValidate="txtSpareCost" Text="*" ValidationGroup="EditSpare" ID="RegularExpressionValidatorx1"
                                                    ValidationExpression="\s*\d*\s*" meta:resourceKey="RegularExpressionValidatorx1Resource1"
                                                    __designer:wfdid="w38"></asp:RegularExpressionValidator>
                                            </asp:View>
                                        </asp:MultiView>
                                    </FooterTemplate>
                                    <ItemStyle CssClass="rightObj" />
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="Literal29" Text='<%# Eval("SpareCost") %>' __designer:wfdid="w25"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exchange number" meta:resourcekey="TemplateFieldResource6">
                                    <EditItemTemplate>
                                        <asp:Literal runat="server" ID="Literal21x" Text='<%# Eval("ExchangeNumber") %>'
                                            __designer:wfdid="w97"></asp:Literal>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:MultiView runat="server" ID="MultiView7" __designer:wfdid="w99" OnLoad="MultiView2_Load">
                                            <asp:View runat="server" ID="View5" __designer:wfdid="w100">
                                                <asp:Literal runat="server" ID="Literal24" Text="Total amount" meta:resourceKey="Literal24Resource1"
                                                    __designer:wfdid="w101"></asp:Literal>
                                            </asp:View>
                                            <asp:View runat="server" ID="View6" __designer:wfdid="w102">
                                            </asp:View>
                                        </asp:MultiView>
                                    </FooterTemplate>
                                    <ItemStyle CssClass="centerObj" />
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="Literal21" Text='<%# Eval("ExchangeNumber") %>' __designer:wfdid="w96"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Spare amount(VND)" meta:resourcekey="TemplateFieldResource7">
                                    <EditItemTemplate>
                                        <asp:Literal runat="server" ID="Literal28" Text='<%# Bind("SpareAmount") %>' __designer:wfdid="w98"></asp:Literal>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:MultiView runat="server" ID="MultiViewx1" __designer:wfdid="w99" OnLoad="MultiView2_Load">
                                            <asp:View runat="server" ID="View01" __designer:wfdid="w100">
                                                <asp:Literal runat="server" ID="litTotalAmount" Text='<%# Bind("SpareName") %>' __designer:wfdid="w101"></asp:Literal>
                                            </asp:View>
                                            <asp:View runat="server" ID="Viewx1" EnableTheming="False" __designer:wfdid="w102">
                                            </asp:View>
                                        </asp:MultiView>
                                    </FooterTemplate>
                                    <ItemStyle CssClass="rightObj" />
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="Literal27" Text='<%# Eval("SpareAmount") %>' __designer:wfdid="w97"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fee amount" meta:resourcekey="TemplateFieldResource7F">
                                    <EditItemTemplate>
                                        <table cellspacing="0" cellpadding="0" border="0">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" Width="115px" Text='<%# Bind("FeeAmount") %>' ID="txtFeeAmount"
                                                            meta:resourceKey="txtFeeAmountResource1" __designer:wfdid="w113"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:RegularExpressionValidator runat="server" ErrorMessage="&quot;Spare number&quot; must input at least 6 non-white-space character (maximum is 35)"
                                                            ControlToValidate="txtFeeAmount" Text="*" ValidationGroup="EditSpare" ID="xRegularExpressionValidator3"
                                                            ValidationExpression="\s*\d*\s*" meta:resourceKey="RegularExpressionValidator3Resource2"
                                                            __designer:wfdid="w114"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        &nbsp;
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:MultiView runat="server" ID="Mult_View2" __designer:wfdid="w115" OnLoad="MultiView2_Load">
                                            <asp:View runat="server" ID="Viewq1" __designer:wfdid="w116">
                                                &nbsp;<asp:Literal runat="server" ID="litTotalFeeAmount" Visible="False" Text="Spares amount(VND)"
                                                    meta:resourceKey="Literal22Resource1" __designer:wfdid="w85"></asp:Literal>
                                            </asp:View>
                                            <asp:View runat="server" ID="Viewq2" __designer:wfdid="w118">
                                                <asp:TextBox runat="server" Width="111px" MaxLength="35" Text='<%# Bind("SpareNumber") %>'
                                                    ID="txtFeeAmount" meta:resourceKey="txtSpareNumberResource2" __designer:wfdid="w119"></asp:TextBox>
                                                <asp:RegularExpressionValidator runat="server" ErrorMessage="&quot;Spare number&quot; must input at least 6 non-white-space character (maximum is 35)"
                                                    ControlToValidate="txtFeeAmount" Text="*" ValidationGroup="EditSpare" ID="xRegularExpressionValidator3"
                                                    ValidationExpression="\s*\d*\s*" meta:resourceKey="RegularExpressionValidator3Resource2"
                                                    __designer:wfdid="w120"></asp:RegularExpressionValidator>
                                            </asp:View>
                                        </asp:MultiView>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="litFeeAmount" Text='<%# Eval("FeeAmount") %>' __designer:wfdid="w112"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource8">
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" ID="ImageButton2" ToolTip="Update" CommandName="Update"
                                            ImageUrl="~/Images/update.gif" meta:resourceKey="ImageButton2Resource1" __designer:wfdid="w319">
                                        </asp:ImageButton>
                                        &nbsp;&nbsp;<asp:ImageButton runat="server" ID="ImageButton3" ToolTip="Cancel" CommandName="Cancel"
                                            ImageUrl="~/Images/cancel.gif" meta:resourceKey="ImageButton3Resource1" __designer:wfdid="w320">
                                        </asp:ImageButton>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:MultiView runat="server" ID="MultiView8" __designer:wfdid="w321" OnLoad="MultiView2_Load">
                                            <asp:View runat="server" ID="View7" __designer:wfdid="w322">
                                            </asp:View>
                                            <asp:View runat="server" ID="View8" __designer:wfdid="w323">
                                                <asp:ImageButton runat="server" ID="ImageButton4" ToolTip="Update" CommandName="InsertSimpleRow"
                                                    ValidationGroup="EditSpare" ImageUrl="~/Images/update.gif" meta:resourceKey="ImageButton4Resource1"
                                                    __designer:wfdid="w324"></asp:ImageButton>
                                                &nbsp;&nbsp;<asp:ImageButton runat="server" ID="imgCancelInsert" ToolTip="Cancel"
                                                    CommandName="CancelInsertSimpleRow" ImageUrl="~/Images/cancel.gif" meta:resourceKey="imgCancelInsertResource1"
                                                    __designer:wfdid="w325"></asp:ImageButton>
                                            </asp:View>
                                        </asp:MultiView>
                                    </FooterTemplate>
                                    <ItemStyle CssClass="centerObj" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="ImageButtonx2" CommandArgument='<%# Eval("ExchangeNumber") %>'
                                            ToolTip="Edit" CommandName="Edit" ImageUrl="~/Images/Edit.gif" meta:resourceKey="ImageButtonx2Resource1"
                                            __designer:wfdid="w317" OnDataBinding="ImageButtonx2_DataBinding"></asp:ImageButton>
                                        &nbsp;&nbsp;<asp:ImageButton runat="server" ID="ImageButton3" CommandArgument='<%# Eval("ExchangeNumber") %>'
                                            ToolTip="Delete" CommandName="Delete" ImageUrl="~/Images/Delete.gif" meta:resourceKey="ImageButton3Resource2"
                                            __designer:wfdid="w318" OnDataBinding="ImageButtonx2_DataBinding"></asp:ImageButton>
                                    </ItemTemplate>
                                    <FooterStyle Wrap="False" />
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="centerObj" />
                        </cc1:EmptyGridViewEx>
                        &nbsp;&nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorx10" runat="server" ControlToValidate="txtSpareNumber"
                            ErrorMessage='&quot;Spare number&quot; cannot be blank!' meta:resourceKey="RequiredFieldValidatorx10Resource1"
                            Text="*" ValidationGroup="EditSpare" Visible="False"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtSpareNumber"
                                ErrorMessage='&quot;Spare number&quot; must input at least 6 non-white-space character (maximum is 35)'
                                meta:resourceKey="RegularExpressionValidator3Resource1" Text="*" ValidationExpression="\s*\S{6,35}\s*"
                                ValidationGroup="EditSpare" Visible="False"></asp:RegularExpressionValidator><asp:Button
                                    ID="btnFindSpare" runat="server" meta:resourceKey="btnFindSpareResource1" OnClick="btnFindSpare_Click"
                                    Text="..." Visible="False" />
                    </td>
                    <td>
                        &nbsp; &nbsp; &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="rightObj">
                        &nbsp;
                    </td>
                    <td class="rightObj">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="rightObj" style="height: 24px">
                        <asp:Button ID="btnMore" runat="server" CssClass="hidden" ValidationGroup="None"
                            meta:resourcekey="btnMoreResource1" />
                        <asp:Button ID="btnSaveTemp" runat="server" meta:resourcekey="btnSaveResource2" OnClick="btnSaveTemp_Click"
                            Text="Temporary save" ValidationGroup="SaveTemp" CommandName="SaveTemp" />
                        <asp:Button ID="btnSave" runat="server" Text=" Save " Width="100px" OnClick="btnSave_Click"
                            Enabled="False" meta:resourcekey="btnSaveResource1" ValidationGroup="Save" CommandName="Save" />&nbsp;
                        <asp:Button ID="btnCalculate" runat="server" Text="Finish" OnClick="btnCalculate_Click1"
                            ValidationGroup="Save" meta:resourcekey="btnCalculateResource1" Enabled="False" />&nbsp;
                        <asp:Button ID="btnPrint" runat="server" Text="Print service sheet" Width="163px"
                            OnClick="btnPrint_Click" Enabled="False" meta:resourcekey="btnPrintResource1" />
                        <asp:Button ID="btnPrintPcv" runat="server" Enabled="False" OnClick="btnPrintPcv_Click"
                            meta:resourcekey="btnPrintResource2" Text="Print parts change" Width="159px"
                            ValidationGroup="Save" />
                    </td>
                    <td class="rightObj" style="height: 24px">
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vwSelectItem" runat="server">
            <asp:Literal ID="Literal18" runat="server" Text="&lt;H3&gt; Select item&lt;/H3&gt;"
                meta:resourcekey="Literal18Resource1"></asp:Literal><asp:ValidationSummary ID="ValidationSummary2"
                    runat="server" ValidationGroup="Select" meta:resourcekey="ValidationSummary2Resource1" />
            <br />
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:GridView ID="gvSelectItem" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            DataKeyNames="Id" DataSourceID="odsSelectItem" PageSize="15" OnSelectedIndexChanging="gvSelectItem_SelectedIndexChanging"
                            CssClass="GridView" Width="492px" OnPreRender="gvSelectxxx_PreRender" meta:resourcekey="gvSelectItemResource1"
                            OnRowCommand="gvSelectxxx_page">
                            <Columns>
                                <asp:TemplateField HeaderText="Engine number" SortExpression="Enginenumber" meta:resourcekey="TemplateFieldResource9">
                                    <EditItemTemplate>
                                        <asp:Literal ID="Literal35" runat="server" Text='<%# Eval("Enginenumber") %>'></asp:Literal>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Literal ID="litSelectedSoldItem" runat="server" Text='<%# Eval("Enginenumber") %>'></asp:Literal><asp:HiddenField
                                            ID="hdMadedate" runat="server" Value='<%# Eval("Madedate") %>' />
                                        <asp:HiddenField ID="hdTiptop" runat="server" Value='<%# Eval("Id") %>' />
                                        <asp:HiddenField ID="hdDealerCode" runat="server" Value='<%# Eval("Dealercode") %>' />
                                        <asp:HiddenField ID="hdDBcode" runat="server" Value='<%# Eval("Databasecode") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Model" SortExpression="Itemtype" meta:resourcekey="TemplateFieldResource10">
                                    <EditItemTemplate>
                                        <asp:Literal ID="litSelectedSoldItemModel" runat="server" Text='<%# Eval("Itemtype") %>'></asp:Literal>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Literal ID="litSelectedSoldItemModel" runat="server" Text='<%# Eval("Itemtype") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Color" SortExpression="Color" meta:resourcekey="TemplateFieldResource11">
                                    <ItemTemplate>
                                        <asp:Literal ID="litSelectedSoldItemColor" runat="server" Text='<%# Eval("Item.Colorname") %>'></asp:Literal>
                                        <asp:HiddenField ID="hdItemColorCode" runat="server" Value='<%# Eval("Item.Colorcode") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Imported date" SortExpression="Importeddate" Visible="False"
                                    meta:resourcekey="TemplateFieldResource12">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Importeddate") %>' meta:resourcekey="TextBox4Resource1"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("Importeddate") %>' meta:resourcekey="Label4Resource1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ButtonType="Button" ShowSelectButton="True" meta:resourcekey="CommandFieldResource1">
                                    <ItemStyle Width="10px" />
                                </asp:CommandField>
                            </Columns>
                            <PagerSettings Position="Top" Mode="NextPreviousFirstLast" />
                            <PagerTemplate>
                                <div style="float: left;">
                                    <asp:Literal ID="litgvSelectItemPageInfo" runat="server" meta:resourcekey="litgvSelectItemPageInfoResource1"></asp:Literal></div>
                                <div style="text-align: right; float: right;">
                                    <asp:Button ID="cmdFirst" runat="server" Text="First" CommandName="Page" CommandArgument="First"
                                        meta:resourcekey="cmdFirstResource1" />
                                    <asp:Button ID="cmdPrevious" runat="server" Text="Previous" CommandName="Page" CommandArgument="Prev"
                                        meta:resourcekey="cmdPreviousResource1" />
                                    <asp:Button ID="cmdNext" runat="server" Text="Next" CommandName="Page" CommandArgument="Next"
                                        meta:resourcekey="cmdNextResource1" />
                                    <asp:Button ID="cmdLast" runat="server" Text="Last" CommandName="Page" CommandArgument="Last"
                                        meta:resourcekey="cmdLastResource1" />
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsSelectItem" runat="server" EnablePaging="True" SelectMethod="Select"
                            TypeName="VDMS.I.ObjectDataSource.ItemInstanceDataSource" SelectCountMethod="SelectCount">
                            <SelectParameters>
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                <asp:Parameter Name="engineNumberLike" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                            meta:resourcekey="btnCancelResource1" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vwSelectSpare" runat="server">
            <asp:Literal ID="Literal25" runat="server" Text="&lt;H3&gt;Select spare&lt;/H3&gt;"
                meta:resourcekey="Literal25Resource1"></asp:Literal><asp:ValidationSummary ID="ValidationSummary3"
                    runat="server" ValidationGroup="SelectSpare" meta:resourcekey="ValidationSummary3Resource1" />
            <br />
            <table border="0" cellpadding="0" cellspacing="0" style="width: 98%">
                <tr>
                    <td>
                        <asp:GridView ID="gvSelectSpare" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            DataSourceID="ObjectDataSource1" PageSize="5" OnSelectedIndexChanging="gvSelectSpare_SelectedIndexChanging"
                            CssClass="GridView" Width="100%" OnPreRender="gvSelectxxx_PreRender" meta:resourcekey="gvSelectSpareResource1"
                            OnRowCommand="gvSelectxxx_page">
                            <Columns>
                                <asp:TemplateField HeaderText="Spare number" meta:resourcekey="TemplateFieldResource13">
                                    <EditItemTemplate>
                                        <asp:Literal ID="Literal34" runat="server" Text='<%# Bind("Partcode") %>'></asp:Literal>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Literal ID="litSelectedSpareNumber" runat="server" Text='<%# Bind("Partcode") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Spare name" meta:resourcekey="TemplateFieldResource14">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Partnamevn") %>' meta:resourcekey="TextBox2Resource1"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("Partnamevn") %>' meta:resourcekey="Label2Resource1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Spare name" meta:resourcekey="TemplateFieldResource15">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Partnameen") %>' meta:resourcekey="TextBox3Resource1"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("Partnameen") %>' meta:resourcekey="Label3Resource1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unitprice" meta:resourcekey="TemplateFieldResource16">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Unitprice") %>' meta:resourcekey="TextBox4Resource2"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("Unitprice") %>' meta:resourcekey="Label4Resource2"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Model" meta:resourcekey="TemplateFieldResource16z">
                                    <ItemTemplate>
                                        <asp:Literal ID="Literal39" runat="server" Text='<%# Eval("Motorcode") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Warranty length" meta:resourcekey="TemplateFieldResource17">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("Warrantylength") %>' meta:resourcekey="TextBox5Resource1"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("Warrantylength") %>' meta:resourcekey="Label5Resource1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Warranty time" SortExpression="Warrantytime" meta:resourcekey="TemplateFieldResource18">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("Warrantytime") %>' meta:resourcekey="TextBox6Resource1"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("Warrantytime") %>' meta:resourcekey="Label6Resource1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ButtonType="Button" ShowSelectButton="True" meta:resourcekey="CommandFieldResource2">
                                    <ItemStyle Width="10px" />
                                </asp:CommandField>
                            </Columns>
                            <PagerSettings Position="Top" Mode="NextPreviousFirstLast" />
                            <PagerTemplate>
                                <div style="float: left;">
                                    <asp:Literal ID="litgvSelectSparePageInfo" runat="server" meta:resourcekey="litgvSelectSparePageInfoResource1"></asp:Literal></div>
                                <div style="text-align: right; float: right;">
                                    <asp:Button ID="cmdFirst" runat="server" Text="First" CommandName="Page" CommandArgument="First"
                                        meta:resourcekey="cmdFirstResource2" />
                                    <asp:Button ID="cmdPrevious" runat="server" Text="Previous" CommandName="Page" CommandArgument="Prev"
                                        meta:resourcekey="cmdPreviousResource2" />
                                    <asp:Button ID="cmdNext" runat="server" Text="Next" CommandName="Page" CommandArgument="Next"
                                        meta:resourcekey="cmdNextResource2" />
                                    <asp:Button ID="cmdLast" runat="server" Text="Last" CommandName="Page" CommandArgument="Last"
                                        meta:resourcekey="cmdLastResource2" />
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                        <asp:ObjectDataSource SelectMethod="Select" ID="ObjectDataSource1" runat="server"
                            TypeName="VDMS.I.ObjectDataSource.SparesDataSource" EnablePaging="True" SelectCountMethod="SelectCount">
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
                        &nbsp;<asp:Button ID="Button1" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                            meta:resourcekey="Button1Resource1" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vwAddCust" runat="server" EnableTheming="True">
            <asp:ObjectDataSource ID="odsCustomer" runat="server" EnablePaging="True" SelectCountMethod="SelectCount"
                SelectMethod="Select" TypeName="VDMS.I.ObjectDataSource.CustomerDataSource">
                <SelectParameters>
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:ControlParameter ControlID="hdEngineNumber" Name="engineNumber" PropertyName="Value"
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:GridView ID="gvSelectCust" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            CssClass="GridView" DataSourceID="odsCustomer" meta:resourcekey="gvSelectCustResource1"
                            OnPreRender="gvSelectxxx_PreRender" OnRowDataBound="gvSelectCust_RowDataBound">
                            <PagerSettings Mode="NextPreviousFirstLast" Position="Top" />
                            <Columns>
                                <asp:BoundField DataField="Identifynumber" HeaderText="Identifynumber" meta:resourcekey="BoundFieldResource1"
                                    SortExpression="Identifynumber" />
                                <asp:BoundField DataField="Fullname" HeaderText="Fullname" meta:resourcekey="BoundFieldResource2"
                                    SortExpression="Fullname" />
                                <asp:BoundField DataField="Address" HeaderText="Address" meta:resourcekey="BoundFieldResource3"
                                    SortExpression="Address" />
                                <asp:BoundField DataField="Email" HeaderText="Email" meta:resourcekey="BoundFieldResource4"
                                    SortExpression="Email" />
                                <asp:BoundField DataField="Birthdate" HeaderText="Birthdate" meta:resourcekey="BoundFieldResource5"
                                    SortExpression="Birthdate" />
                                <asp:BoundField DataField="Mobile" HeaderText="Mobile" meta:resourcekey="BoundFieldResource6"
                                    SortExpression="Mobile" />
                                <asp:BoundField DataField="Tel" HeaderText="Tel" meta:resourcekey="BoundFieldResource7"
                                    SortExpression="Tel" />
                                <asp:BoundField DataField="Customerdescription" HeaderText="Description" meta:resourcekey="BoundFieldResource8"
                                    SortExpression="Customerdescription" />
                                <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource23">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelectCust" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id") %>'
                                            meta:resourcekey="btnSelectCustResource1" OnClick="Button1_Click" Text="Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerTemplate>
                                <div style="float: left;">
                                    <asp:Literal ID="litgvSelectSparePageInfo" runat="server" meta:resourceKey="litgvSelectSparePageInfoResource1"></asp:Literal>
                                </div>
                                <div style="text-align: right; float: right;">
                                    <asp:Button ID="cmdFirst" runat="server" CommandArgument="First" CommandName="Page"
                                        meta:resourceKey="cmdFirstResource2" Text="First" />
                                    <asp:Button ID="cmdPrevious" runat="server" CommandArgument="Prev" CommandName="Page"
                                        meta:resourceKey="cmdPreviousResource2" Text="Previous" />
                                    <asp:Button ID="cmdNext" runat="server" CommandArgument="Next" CommandName="Page"
                                        meta:resourceKey="cmdNextResource2" Text="Next" />
                                    <asp:Button ID="cmdLast" runat="server" CommandArgument="Last" CommandName="Page"
                                        meta:resourceKey="cmdLastResource2" Text="Last" />
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="Button3" runat="server" Text="Cancel" OnClick="btnCancel_Click" meta:resourcekey="Button3Resource3" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vwSelectModel" runat="server">
            <asp:Literal ID="Literal31" runat="server" Text="&lt;H3&gt;Select model&lt;/H3&gt;"
                meta:resourcekey="Literal31Resource1"></asp:Literal><asp:ValidationSummary ID="ValidationSummary4"
                    runat="server" ValidationGroup="SelectSpare" meta:resourcekey="ValidationSummary4Resource1" />
            <br />
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:GridView ID="gvSelectModel" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            DataSourceID="odsSelectModel" PageSize="15" OnSelectedIndexChanging="gvSelectModel_SelectedIndexChanging"
                            CssClass="GridView" Width="492px" OnPreRender="gvSelectxxx_PreRender" meta:resourcekey="gvSelectModelResource1"
                            OnRowCommand="gvSelectxxx_page">
                            <Columns>
                                <asp:TemplateField HeaderText="Itemtype" SortExpression="Itemtype" meta:resourcekey="TemplateFieldResource19">
                                    <ItemTemplate>
                                        <asp:Literal ID="litSelectedModel" runat="server" Text='<%# Bind("Itemtype") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Itemname" SortExpression="Itemname" meta:resourcekey="TemplateFieldResource20">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Itemname") %>' meta:resourcekey="TextBox2Resource2"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("Itemname") %>' meta:resourcekey="Label2Resource2"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowSelectButton="True" meta:resourcekey="CommandFieldResource3"
                                    ButtonType="Button">
                                    <ItemStyle Width="10px" />
                                </asp:CommandField>
                            </Columns>
                            <PagerSettings Position="Top" Mode="NextPreviousFirstLast" />
                            <PagerTemplate>
                                <div style="float: left;">
                                    <asp:Literal ID="litgvSelectModelPageInfo" runat="server" meta:resourcekey="litgvSelectModelPageInfoResource1"></asp:Literal></div>
                                <div style="text-align: right; float: right;">
                                    <asp:Button ID="cmdFirst" runat="server" Text="First" CommandName="Page" CommandArgument="First"
                                        meta:resourcekey="cmdFirstResource3" />
                                    <asp:Button ID="cmdPrevious" runat="server" Text="Previous" CommandName="Page" CommandArgument="Prev"
                                        meta:resourcekey="cmdPreviousResource3" />
                                    <asp:Button ID="cmdNext" runat="server" Text="Next" CommandName="Page" CommandArgument="Next"
                                        meta:resourcekey="cmdNextResource3" />
                                    <asp:Button ID="cmdLast" runat="server" Text="Last" CommandName="Page" CommandArgument="Last"
                                        meta:resourcekey="cmdLastResource3" />
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                        <asp:ObjectDataSource SelectMethod="Select" ID="odsSelectModel" runat="server" TypeName="VDMS.I.ObjectDataSource.DataItemDataSource"
                            EnablePaging="True" SelectCountMethod="SelectCount">
                            <SelectParameters>
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                <asp:Parameter Name="itemTypeLike" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        &nbsp;<asp:Button ID="Button2" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                            meta:resourcekey="Button2Resource2" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vwAddExchange" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                <tr>
                    <td>
                        <asp:Literal ID="Literal32" runat="server" Text="&lt;H3&gt;Add Exchange spare&lt;/H3&gt;"
                            meta:resourcekey="Literal32Resource1"></asp:Literal><uc2:AddExchange ID="AddExchange1"
                                runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vwAddBroken" runat="server">
            <asp:Literal ID="Literal33" runat="server" Text="&lt;H3&gt;Select broken code&lt;/H3&gt;"
                meta:resourcekey="Literal33Resource1"></asp:Literal><asp:ValidationSummary ID="ValidationSummary6"
                    runat="server" ValidationGroup="SelectBroken" meta:resourcekey="ValidationSummary6Resource1" />
            <br />
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:GridView ID="gvSelectBroken" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            DataSourceID="odsSelectBroken" PageSize="5" OnSelectedIndexChanging="gvSelectBroken_SelectedIndexChanging"
                            CssClass="GridView" Width="100%" OnPreRender="gvSelectxxx_PreRender" meta:resourcekey="gvSelectBrokenResource1"
                            OnRowCommand="gvSelectxxx_page">
                            <PagerSettings Position="Top" Mode="NextPreviousFirstLast" />
                            <Columns>
                                <asp:TemplateField HeaderText="Broken code" SortExpression="Brokencode" meta:resourcekey="TemplateFieldResource21">
                                    <ItemTemplate>
                                        <asp:Literal ID="litSelectedBrokenCode" runat="server" Text='<%# Eval("Brokencode") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Broken name" SortExpression="Brokenname" meta:resourcekey="TemplateFieldResource22">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Brokenname") %>' meta:resourcekey="TextBox2Resource3"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("Brokenname") %>' meta:resourcekey="Label2Resource3"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ButtonType="Button" ShowSelectButton="True" meta:resourcekey="CommandFieldResource4">
                                    <ItemStyle Width="10px" />
                                </asp:CommandField>
                            </Columns>
                            <PagerTemplate>
                                <div style="float: left;">
                                    <asp:Literal ID="litgvSelectBrokenPageInfo" runat="server" meta:resourcekey="litgvSelectBrokenPageInfoResource1"></asp:Literal></div>
                                <div style="text-align: right; float: right;">
                                    <asp:Button ID="cmdFirst" runat="server" Text="First" CommandName="Page" CommandArgument="First"
                                        meta:resourcekey="cmdFirstResource4" />
                                    <asp:Button ID="cmdPrevious" runat="server" Text="Previous" CommandName="Page" CommandArgument="Prev"
                                        meta:resourcekey="cmdPreviousResource4" />
                                    <asp:Button ID="cmdNext" runat="server" Text="Next" CommandName="Page" CommandArgument="Next"
                                        meta:resourcekey="cmdNextResource4" />
                                    <asp:Button ID="cmdLast" runat="server" Text="Last" CommandName="Page" CommandArgument="Last"
                                        meta:resourcekey="cmdLastResource4" />
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                        <asp:ObjectDataSource SelectMethod="Select" ID="odsSelectBroken" runat="server" TypeName="VDMS.I.ObjectDataSource.BrokenDatasource"
                            EnablePaging="True" SelectCountMethod="SelectCount">
                            <SelectParameters>
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                <asp:Parameter Name="fromCode" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        &nbsp;<asp:Button ID="Button5" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                            meta:resourcekey="Button5Resource1" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vwNoSheetToView" runat="server">
        </asp:View>
        <asp:View ID="vwPrint" runat="server">
            <table style="width: 100%">
                <tr>
                    <td style="width: 100px">
                        <CR:CrystalReportViewer ID="CRVServiceRecordSheet" runat="server" AutoDataBind="True"
                            DisplayGroupTree="False" meta:resourcekey="CRVServiceRecordSheetResource1"></CR:CrystalReportViewer>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 100px; text-align: center">
                        <asp:Button ID="btnBackFromPrint" runat="server" Text="Back" meta:resourcekey="btnBackFromPrintResource1"
                            OnClick="btnBackFromPrint_Click" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView><asp:HiddenField ID="hdEngineNumber" runat="server" />
    &nbsp;&nbsp;
</asp:Content>
