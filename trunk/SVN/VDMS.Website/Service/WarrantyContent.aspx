<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="WarrantyContent.aspx.cs" Inherits="Service_Warranty_Content" Title="Service record sheet"
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<%@ Register Assembly="VDMS.Common" Namespace="VDMS.Common.Web.CustomControl" TagPrefix="cc2" %>
<%@ Register Src="../Controls/Services/AddExchange.ascx" TagName="AddExchange" TagPrefix="uc2" %>
<%@ Register Assembly="VDMS.Common" Namespace="VDMS.Common.Web" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">

    <script type="text/javascript">

        function engineSelected(engNo) {
            tb_remove();
            $('#<%= btnCheckEngineNo.ClientID %>').click();
            var lnk = document.getElementById('_lnkFindCust');
            lnk.href = 'Popup/SelectCustomer.aspx?key=<%= this.Info.PageKey %>&engno=' + engNo + '&TB_iframe=true';
            //alert(lnk.href);
        }
        function modelSelected() {
            tb_remove();
            $('#<%= btnCheckModel.ClientID %>').click();
        }
        function cusSelected() {
            tb_remove();
            $('#<%= btnSelectCust.ClientID %>').click();
        }
        function sparesSelected() {
            tb_remove();
            $('#<%= btnSelectSpares.ClientID %>').click();
        }

        function pageLoad(sender, args) {
            if (args.get_isPartialLoad()) {
            }
        }
        function showPopUp(linkId) { //_lnkFindCust _lnkCheckModel _lnkSelectSpare
            $('#' + linkId).click();
        }
        function showEngPopUp(linkId) {
            var s = 'Popup/SelectEngineNo.aspx?key=<%= this.Info.PageKey %>';
            var dl = document.getElementById('<%= this.ddlDealer.ClientID %>');
            if (dl != null) {
                s = s + '&dl=' + dl.value;
            }
            s = s + '&TB_iframe=true&width=500';
            document.getElementById(linkId).href = s;
            showPopUp(linkId);
        }
    </script>

    <script src="../js/FloatObj.js" type="text/javascript"></script>

    <script type="text/javascript">
        function SubmitConfirm(obj, msg) {
            return confirm(msg)
            var ok = confirm(msg)
            if (ok) { obj.disabled = 'disabled' }
            return ok
        } 
    </script>

    <a id='_lnkSelSpare' class="thickbox" href="Popup/SelectSpares.aspx?key=<%= this.Info.PageKey %>&TB_iframe=true&width=700">
    </a><a id='_lnkFindCust' class="thickbox" href="Popup/SelectCustomer.aspx?key=<%= this.Info.PageKey %>&engno=<%= this.Info.ServiceHeader.Enginenumber %>&TB_iframe=true">
    </a><a id="_lnkCheckModel" class="thickbox" href="Popup/SelectModel.aspx?key=<%= this.Info.PageKey %>&TB_iframe=true&width=400">
    </a><a id='_lnkSearchEngineNo' class="thickbox" href="Popup/SelectEngineNo.aspx?key=<%= this.Info.PageKey %>&TB_iframe=true&width=500">
    </a>
    <%--<input id="_btnSearchEngineNo" onclick="showPopUp('_lnkSearchEngineNo');" type="button" value="..." />
    <input id="_btnSelSpare" onclick="showPopUp('_lnkSelSpare');" type="button" value="..." />
    <input id="_btnFindCust" onclick="showPopUp('_lnkFindCust');" type="button" value="..." />
    <input id="_btnSearchkModel" onclick="showPopUp('_lnkCheckModel');" type="button" value="..." />--%>
    <table class="forceNowrap" style="width: 100%">
        <tr>
            <td class="forceNowrap" style="width: 1%">
            </td>
            <td style="width: 1%">
            </td>
            <td>
                <asp:HyperLink ID="lnkNewSheet" runat="server" NavigateUrl="~/Service/WarrantyContent.aspx"
                    Target="_blank" Text="Add new sheet" meta:resourcekey="lnkNewSheetResource1"></asp:HyperLink>
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" meta:resourcekey="vsSaveResource1" />
    <asp:ValidationSummary ID="vsSaveTemp" ValidationGroup="SaveTemp" runat="server"
        meta:resourcekey="vsSaveResource1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:BulletedList ID="bllErrorMsg" runat="server" CssClass="errorMsg" meta:resourcekey="bllErrorMsgResource1">
            </asp:BulletedList>
            <input id="hdSellDealer" runat="server" enableviewstate="true" type="hidden" value="" />
        </ContentTemplate>
    </asp:UpdatePanel>
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
            var objSellD = document.getElementById('<%= hdSellDealer.ClientID %>');
            var objCustomerID = document.getElementById(CustomerObj);
            var objEngineNo = document.getElementById('<%= txtEngineNo.ClientID %>');
            var objMotorType = document.getElementById('<%= txtModel.ClientID %>');
            var objCusId = document.getElementById('<%= _CustomerID.ClientID %>');

            if ((objEngineNo.value.trim() == "") || (objMotorType.value.trim() == "")) {
                alert('<%= LoadCustomerErr %>');
                return false;
            }

            //            hrefURL += "cusid=" + objCustomerID.value;
            hrefURL += "cusid=" + objCusId.value;
            hrefURL += "&IsWarranty=true";
            hrefURL += "&engineno=" + objEngineNo.value.trim();
            hrefURL += "&motortype=" + objMotorType.value.trim();
            hrefURL += "&action=";
            //            if (objCustomerID.value == "") {
            if ((objCusId.value == "") || (objCusId.value == "-1")) {
                hrefURL += "insert";
            }
            else {
                hrefURL += "edit";
            }
            hrefURL += "&SellD=" + objSellD.value;

            var arrParams = new Array();
            arrParams[0] = window;
            arrParams[1] = '<%= _CustomerID.ClientID %>';
            arrParams[2] = '<%= txtCustId.ClientID %>';
            arrParams[3] = '<%= _CustomerAction.ClientID %>';
            arrParams[4] = '<%= ddlSex.ClientID %>';
            arrParams[5] = '<%= txtBirthDate.ClientID %>';
            arrParams[6] = '<%= txtAddress.ClientID %>';
            arrParams[7] = '<%= ddlProvince.ClientID %>';
            arrParams[8] = '<%= txtDistrict.ClientID %>';
            arrParams[9] = '<%= tblCus_JobType.ClientID %>';
            arrParams[10] = '<%= txtCEmail.ClientID %>';
            arrParams[11] = '<%= txtCPhone.ClientID %>';
            arrParams[12] = '<%= txtCMobile.ClientID %>';
            arrParams[13] = '<%= ddlCus_SetType.ClientID %>';
            arrParams[14] = '<%= ddlCusType.ClientID %>';
            arrParams[15] = '<%= txtCus_Desc.ClientID %>';
            arrParams[16] = '<%= txtPrecinct.ClientID %>';
            arrParams[17] = '<%= lbCustomerFullName.ClientID %>';
            arrParams[18] = '<%= _CustomerFullName.ClientID %>';

            var objCustomerAction = document.getElementById('<%= _CustomerAction.ClientID %>');
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

    <%--<asp:UpdatePanel ID="udpPage" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
        <ContentTemplate>--%>
    <asp:Panel ID="udpPage" runat="server">
        <ajaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" Width="100%"
            meta:resourcekey="TabsResource1">
            <ajaxToolkit:TabPanel runat="server" ID="tpnSRS" HeaderText="Service record sheet"
                meta:resourcekey="tpnSRSResource1">
                <HeaderTemplate>
                    <asp:Literal Text="Service record sheet" ID="litTabSRSHeader" runat="server" meta:resourcekey="litTabSRSHeaderResource1"></asp:Literal>
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:UpdatePanel ID="udpSRS" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form">
                                <asp:UpdateProgress ID="udpr" runat="server" AssociatedUpdatePanelID="udpSRS" DisplayAfter="0"
                                    DynamicLayout="False">
                                    <ProgressTemplate>
                                        <div style="display: table-cell; width: 100%; height: 100%">
                                            <div>
                                                <img src="../../Images/Spinner.gif" alt="" />
                                                <asp:Literal ID="Literal31" runat="server" Text="Updating..." meta:resourcekey="Literal31Resource1"></asp:Literal>
                                            </div>
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table border="0" cellpadding="2" cellspacing="2">
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
                                            &nbsp;<asp:Literal ID="Literal38" runat="server" Text="Dealer code:" meta:resourcekey="Literal38Resource1"></asp:Literal>
                                        </td>
                                        <td colspan="6">
                                            <asp:DropDownList ID="ddlDealer" runat="server" Width="100%" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlDealer_SelectedIndexChanged" OnDataBound="ddlDealer_DataBound"
                                                meta:resourcekey="ddlDealerResource1">
                                            </asp:DropDownList>
                                            <%--<vdms:DealerList ID="ddlDealer" RootDealer="/" RemoveRootItem="true" EnabledSaperateByDB="true"
                                                runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDealer_SelectedIndexChanged"
                                                OnDataBound="ddlDealer_DataBound" ShowEmptyItem="true">
                                            </vdms:DealerList>--%>
                                            <asp:TextBox ID="txtDealerCode" runat="server" Visible="False" meta:resourcekey="txtDealerCodeResource1"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="nameField">
                                        </td>
                                        <td class="nameField">
                                            <asp:Literal ID="Literal36" runat="server" meta:resourcekey="Literal36Resource1"
                                                Text="Branch code:"></asp:Literal>
                                        </td>
                                        <td colspan="6">
                                            <asp:DropDownList ID="ddlBranchCode" runat="server" Width="190px" meta:resourcekey="ddlBranchCodeResource1">
                                            </asp:DropDownList>
                                            <%--<vdms:WarehouseList ID="ddlBranchCode" runat="server" Type="V">
                                            </vdms:WarehouseList>--%>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rqvBranchCode" runat="server" ControlToValidate="ddlBranchCode"
                                                ErrorMessage='"Engine number" cannot be blank!' SetFocusOnError="True" Text="*"
                                                ValidationGroup="Save" Visible="False" meta:resourcekey="rqvBranchCodeResource1"></asp:RequiredFieldValidator>
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
                                                        <asp:TextBox ID="txtEngineNo" AutoPostBack="True" ReadOnly="True" runat="server"
                                                            Width="159px" MaxLength="20" meta:resourcekey="txtEngineNoResource1"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="rqvEngineNumber" runat="server" SetFocusOnError="True"
                                                            ValidationGroup="Save" ControlToValidate="txtEngineNo" ErrorMessage='"Engine number" cannot be blank!'
                                                            Text="*" meta:resourcekey="rqvEngineNumberResource1"></asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="rqvEngineNumber2" runat="server" SetFocusOnError="True"
                                                            ValidationGroup="SaveTemp" ControlToValidate="txtEngineNo" ErrorMessage='"Engine number" cannot be blank!'
                                                            Text="*" meta:resourcekey="rqvEngineNumberResource1"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <input id="_btnSearchEngineNo" onclick="showEngPopUp('_lnkSearchEngineNo');" type="button"
                                                            value="..." />
                                                        <asp:Button ID="btnCheckEngineNo" runat="server" Text="Check engine" Width="0px"
                                                            CssClass="hidden" OnClick="btnCheckEngineNo_Click" meta:resourcekey="btnCheckEngineNoResource1" />
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
                                            <asp:Literal ID="Literal3" runat="server" Text="Sheet number:" meta:resourcekey="Literal3Resource1"></asp:Literal>
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
                                                        <asp:Button ID="btnFindCust" UseSubmitBehavior="false" OnClientClick="showPopUp('_lnkFindCust'); return false;"
                                                            Text="..." runat="server" />
                                                        <%--<input id="_btnFindCust" runat="server" onclick="showSelectPart('_lnkFindCust');" type="button" value="..." />--%>
                                                        <asp:Button ID="btnSelectCust" OnClick="btnSelectCust_Click" CssClass="hidden" runat="server"
                                                            meta:resourcekey="btnSelectCustResource1" />
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
                                            <asp:TextBox ID="txtBuyDate" AutoPostBack="True" OnTextChanged="HeaderInfo_Changed"
                                                runat="server" Width="161px" meta:resourcekey="txtBuyDateResource1"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtBuyDate"
                                                PopupButtonID="ibtnCalendar" Enabled="True">
                                            </ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtBuyDate"
                                                Mask="99/99/9999" MaskType="Date" Enabled="True" CultureAMPMPlaceholder="AM;PM"
                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                                                CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnCalendar" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
                                                meta:resourcekey="ibtnCalendarResource1" />
                                        </td>
                                        <td>
                                            <asp:RangeValidator meta:resourcekey="rvBuydateResource1" ValidationGroup="Save"
                                                Type="Date" ID="rvBuyDate" Text="*" ErrorMessage='&quot;By date&quot; cannot cannot greater than &quot;Now&quot;!'
                                                ControlToValidate="txtBuyDate" runat="server"></asp:RangeValidator>
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
                                                meta:resourcekey="txtCustAddressResource1"></asp:TextBox>
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
                                                        <asp:TextBox AutoPostBack="True" ID="txtModel" runat="server" Width="159px" MaxLength="20"
                                                            meta:resourcekey="txtModelResource1"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtModel"
                                                            ErrorMessage='&quot;Model&quot; cannot be blank!' ValidationGroup="Save" Text="*"
                                                            meta:resourcekey="RequiredFieldValidator8Resource1"></asp:RequiredFieldValidator>
                                                        <asp:Button ID="btnSearchkModel" UseSubmitBehavior="false" OnClientClick="showPopUp('_lnkCheckModel'); return false;"
                                                            Text="..." runat="server" />
                                                        <%--<input id="_btnSearchkModel" onclick="showSelectPart('_lnkCheckModel');" type="button" value="..." />--%>
                                                        <asp:Button ID="btnCheckModel" OnClick="btnCheckModel_Click" CssClass="hidden" runat="server"
                                                            Text="..." Width="27px" meta:resourcekey="btnCheckModelResource1" />
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
                                                ErrorMessage="Color cannot be blank!" ValidationGroup="Save" meta:resourcekey="rqvColorResource1"
                                                Text="*"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="nameField">
                                        </td>
                                        <td class="nameField">
                                            <asp:Literal ID="Literal11" runat="server" Text="Plate number:" meta:resourcekey="Literal11Resource1"></asp:Literal>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtNumberPlate" runat="server" Width="159px" MaxLength="12" meta:resourcekey="txtNumberPlateResource1"
                                                OnTextChanged="txtNumberPlate_TextChanged"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="nameField">
                                            <asp:Literal ID="Literal9" runat="server" Text="Frame number:" meta:resourcekey="Literal9Resource1"></asp:Literal>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtFrameNumber" runat="server" Width="186px" MaxLength="30" meta:resourcekey="txtFrameNumberResource1"
                                                OnTextChanged="txtFrameNumber_TextChanged"></asp:TextBox>
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
                                            <asp:TextBox AutoPostBack="True" ID="txtKm" runat="server" Width="159px" MaxLength="15"
                                                CausesValidation="True" meta:resourcekey="txtKmResource1" OnTextChanged="HeaderInfo_Changed"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="txtKm_FilteredTextBoxExtender" runat="server"
                                                FilterType="Numbers" TargetControlID="txtKm" Enabled="True">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                            <asp:RegularExpressionValidator ID="rgvKm" runat="server" SetFocusOnError="True"
                                                ValidationGroup="Save" ValidationExpression="\s*\d{1,15}\s*" ControlToValidate="txtKm"
                                                ErrorMessage='Invalid "Current kilometers" value!' Text="*" meta:resourcekey="rgvKmResource1"></asp:RegularExpressionValidator>
                                            <asp:CompareValidator ID="cvKmCount" runat="server" ControlToCompare="txtLastKm"
                                                ControlToValidate="txtKm" ErrorMessage="Current km less than previous value!"
                                                SetFocusOnError="True" Type="Double" ValidationGroup="None" Operator="GreaterThan"
                                                meta:resourcekey="CompareValidator1Resource1" Text="*"></asp:CompareValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtKm"
                                                ErrorMessage='"Current kilometers" cannot be blank!' SetFocusOnError="True" ValidationGroup="Save"
                                                Text="*" meta:resourcekey="RequiredFieldValidator5Resource1"></asp:RequiredFieldValidator>
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
                                            <asp:CheckBoxList OnLoad="chblSerList_OnLoad" OnSelectedIndexChanged="chblSerList_Changed"
                                                ID="chblSerList" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                                meta:resourcekey="chblSerListResource1">
                                                <asp:ListItem Value="5" Text="Maintain" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Repair" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Warranty" meta:resourcekey="ListItemResource3"></asp:ListItem>
                                            </asp:CheckBoxList>
                                            <cc2:RequiredOneItemValidator ValidationGroup="Save" ControlToValidate="chblSerList"
                                                ErrorMessage="You must select at least one service!" Text="*" ID="rovServiceList"
                                                runat="server" meta:resourcekey="rovServiceListResource1" ValidateEmptyList="False"
                                                ValidateEmptyText="True"></cc2:RequiredOneItemValidator>
                                        </td>
                                        <td class="nameField">
                                            <asp:Image ID="Image17" runat="server" SkinID="RequireField" meta:resourcekey="Image17Resource1" />
                                            <asp:Literal ID="Literal8" runat="server" Text="Repair date:" meta:resourcekey="Literal8Resource1"></asp:Literal>
                                        </td>
                                        <td style="width: 1px">
                                            <asp:TextBox ID="txtRepairDate" runat="server" Width="161px" meta:resourcekey="txtRepairDateResource1"
                                                AutoPostBack="True" OnTextChanged="HeaderInfo_Changed"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True"
                                                PopupButtonID="ibtnCalendarR" TargetControlID="txtRepairDate">
                                            </ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Enabled="True"
                                                Mask="99/99/9999" MaskType="Date" TargetControlID="txtRepairDate" CultureAMPMPlaceholder="AM;PM"
                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                                                CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnCalendarR" runat="server" OnClientClick="return false;"
                                                SkinID="CalendarImageButton" meta:resourcekey="ibtnCalendarRResource1" />
                                        </td>
                                        <td>
                                            <asp:RangeValidator meta:resourcekey="rvRepairDateResource1" ValidationGroup="Save"
                                                Type="Date" ID="rvRepairDate" Text="*" ErrorMessage="&quot;Repair date&quot; cannot cannot greater than &quot;Now&quot;!"
                                                ControlToValidate="txtRepairDate" runat="server"></asp:RangeValidator>
                                            <asp:RequiredFieldValidator ID="rqvRepairDate" runat="server" ControlToValidate="txtRepairDate"
                                                ErrorMessage='"Repair date" cannot be blank!' SetFocusOnError="True" Text="*"
                                                ValidationGroup="Save" meta:resourcekey="rqvRepairDateResource1"></asp:RequiredFieldValidator>
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
                                                ValidationGroup="Save" ControlToValidate="txtErrorStatus" ErrorMessage='"Damaged status" cannot be blank!'
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
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Button ID="btnSelSpare" OnClientClick="showPopUp('_lnkSelSpare'); return false;"
                        UseSubmitBehavior="False" Text="Add spares" runat="server" meta:resourcekey="btnPCVCallSelSpareResource1" />
                    <asp:UpdatePanel ID="udpSRSItems" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:ValidationSummary ID="vsAddNewSpare" runat="server" ValidationGroup="AddNewSpare"
                                            meta:resourcekey="vsAddNewSpareResource1" />
                                        <asp:Button ID="btnReloadSRSItems" UseSubmitBehavior="False" CssClass="hidden" Text="Refresh"
                                            runat="server" OnClick="btnReloadSRSItems_Click" meta:resourcekey="btnReloadSRSItemsResource1" />
                                    </td>
                                    <td>
                                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="udpSRSItems"
                                            DisplayAfter="0" DynamicLayout="False">
                                            <ProgressTemplate>
                                                <div style="display: inline;">
                                                    <div>
                                                        <img src="../../Images/Spinner.gif" alt="" />
                                                        <asp:Literal ID="Literal31x" runat="server" Text="Updating..." meta:resourcekey="Literal31xResource1"></asp:Literal>
                                                    </div>
                                                </div>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <div class="grid">
                                <vdms:PageGridView ID="gvSpareList" runat="server" AutoGenerateColumns="False" CssClass="GridView"
                                    DataKeyNames="Partcode" ShowFooter="True" Width="100%" OnRowDataBound="gvSpareList_RowDataBound"
                                    EmptyDataText="There are no spares in list!" OnDataBound="gvSpareList_DataBound"
                                    meta:resourcekey="gvSpareListResource1">
                                    <EmptyDataRowStyle CssClass="emptyGVrow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="No" meta:resourcekey="TemplateFieldResource23">
                                            <ItemStyle CssClass="centerObj" Wrap="False" />
                                            <ItemTemplate>
                                                <asp:Label ID="lbItemNo" runat="server" meta:resourcekey="lbItemNoResource1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Spare number" meta:resourcekey="TemplateFieldResource24">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <ItemStyle Wrap="False" />
                                            <ItemTemplate>
                                                <asp:Literal ID="litSpareNumber" runat="server" Text='<%# Eval("Partcode") %>'></asp:Literal>
                                                <asp:Label ID="lbInvalidSpareNumber" runat="server" CssClass="errorItem" Text="*"
                                                    meta:resourcekey="lbInvalidSpareNumberResource1"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtNewSpareNumber" ValidationGroup="AddNewSpare" runat="server"
                                                                meta:resourcekey="txtNewSpareNumberResource1"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="rfvNewSpareNumber" runat="server" ControlToValidate="txtNewSpareNumber"
                                                                SetFocusOnError="True" ErrorMessage="Spare number can not be blank!" ValidationGroup="AddNewSpare"
                                                                meta:resourcekey="rfvNewSpareNumberResource1" Text="*"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>
                                                            <asp:RegularExpressionValidator runat="server" ErrorMessage="&quot;Spare number&quot; must input at least 6 non-white-space character (maximum is 35)!"
                                                                SetFocusOnError="True" ControlToValidate="txtNewSpareNumber" Text="*" ValidationGroup="AddNewSpare"
                                                                ID="RegularExpressionValidator3" ValidationExpression="\s*(\S\s*){6,35}\s*" meta:resourcekey="RegularExpressionValidator3Resource1"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Spare name" meta:resourcekey="TemplateFieldResource25">
                                            <FooterTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtNewSpareName" ValidationGroup="AddNewSpare" runat="server" meta:resourcekey="txtNewSpareNameResource1"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="rfvNewSpareName" runat="server" ControlToValidate="txtNewSpareName"
                                                                SetFocusOnError="True" ErrorMessage="Spare name can not be blank!" ValidationGroup="AddNewSpare"
                                                                meta:resourcekey="rfvNewSpareNameResource1" Text="*"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Literal ID="litSpareName" runat="server" Text='<%# Eval("Partname") %>'></asp:Literal>
                                                <asp:TextBox ID="txtSpareName" AutoPostBack="True" runat="server" OnTextChanged="gvSpareList_UpdateRow"
                                                    Text='<%# Bind("Partname") %>' meta:resourcekey="txtSpareNameResource1"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity" meta:resourcekey="TemplateFieldResource26">
                                            <FooterStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtNewSpareQuantity" runat="server" ValidationGroup="AddNewSpare"
                                                                Width="35px" Text="1" meta:resourcekey="txtNewSpareQuantityResource1"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender TargetControlID="txtNewSpareQuantity" FilterType="Numbers"
                                                                ID="FilteredTextBoxExtender1" runat="server" Enabled="True" />
                                                        </td>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="rfvNewSpareQuantity" runat="server" ControlToValidate="txtNewSpareQuantity"
                                                                SetFocusOnError="True" ErrorMessage="Spare quantity can not be blank!" ValidationGroup="AddNewSpare"
                                                                meta:resourcekey="rfvNewSpareQuantityResource1" Text="*"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtQuantity" runat="server" MaxLength="4" ValidationGroup="EditSpare"
                                                                AutoPostBack="True" AutoCompleteType="Disabled" OnTextChanged="gvSpareList_UpdateRow"
                                                                CausesValidation="True" Text='<%# Bind("Partqty") %>' Width="35px" meta:resourcekey="txtQuantityResource1"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="txtQuantity_FilteredTextBoxExtender" runat="server"
                                                                FilterType="Numbers" TargetControlID="txtQuantity" Enabled="True">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
                                                        </td>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="rfvSpareQuantity" runat="server" ControlToValidate="txtQuantity"
                                                                ErrorMessage="&quot;Quantity&quot; cannot be blank!" SetFocusOnError="True" Text="*"
                                                                ValidationGroup="EditSpare" meta:resourcekey="rfvSpareQuantityResource1"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit price(VND)" meta:resourcekey="TemplateFieldResource27">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <ItemStyle CssClass="rightObj" />
                                            <FooterTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtNewSparePrice" Width="112px" ValidationGroup="AddNewSpare" runat="server"
                                                                meta:resourcekey="txtNewSparePriceResource1"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender TargetControlID="txtNewSparePrice" FilterType="Numbers"
                                                                ID="FilteredTextBoxExtender1x" runat="server" Enabled="True" />
                                                        </td>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="rfvNewSparePrice" runat="server" ControlToValidate="txtNewSparePrice"
                                                                SetFocusOnError="True" ErrorMessage="Unit price can not be blank!" ValidationGroup="AddNewSpare"
                                                                meta:resourcekey="rfvNewSparePriceResource1" Text="*"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtSpareCost" runat="server" MaxLength="30" AutoPostBack="True"
                                                                AutoCompleteType="Disabled" OnTextChanged="gvSpareList_UpdateRow" CausesValidation="True"
                                                                Text='<%# Bind("Unitprice") %>' Width="112px" ValidationGroup="EditSpare" meta:resourcekey="txtSpareCostResource1"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="txtSpareCost_FilteredTextBoxExtender" runat="server"
                                                                FilterType="Numbers" TargetControlID="txtSpareCost" Enabled="True">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
                                                        </td>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="rfvSparePrice" runat="server" ControlToValidate="txtSpareCost"
                                                                ErrorMessage="&quot;Unit price&quot; cannot be blank!" SetFocusOnError="True"
                                                                Text="*" ValidationGroup="EditSpare" meta:resourcekey="rfvSparePriceResource1"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Exchange number" Visible="False" meta:resourcekey="TemplateFieldResource28">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <ItemStyle CssClass="centerObj" />
                                            <ItemTemplate>
                                                <asp:Literal ID="Literal21" runat="server" Text='<%# Eval("ExchangeNumber") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Spare amount(VND)" meta:resourcekey="TemplateFieldResource29">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <ItemStyle CssClass="rightObj" />
                                            <ItemTemplate>
                                                <asp:Literal ID="Literal27" runat="server" Text='<%# Eval("SpareAmount") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fee amount(VND)" meta:resourcekey="TemplateFieldResource30">
                                            <ItemStyle HorizontalAlign="Right" />
                                            <ItemTemplate>
                                                <asp:Literal ID="litFeeAmount" runat="server" Text='<%# Eval("FeeAmount") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource31">
                                            <FooterTemplate>
                                                <asp:ImageButton ID="imbAddNewSpare" OnClick="imbAddNewSpare_Click" runat="server"
                                                    ValidationGroup="AddNewSpare" ImageUrl="~/Images/update.gif" meta:resourcekey="imbAddNewSpareResource1" />
                                            </FooterTemplate>
                                            <ItemStyle CssClass="centerObj" Wrap="False" />
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imbDelete" runat="server" CommandArgument='<%# Eval("Partcode") %>'
                                                    CommandName="Delete" ImageUrl="~/Images/Delete.gif" ToolTip="Delete" OnClientClick="onDeleteSRSItem()"
                                                    Visible='<%# EvalCommandVisible(Eval("ExchangeNumber")) %>' OnClick="imbDelete_Click"
                                                    meta:resourcekey="imbDeleteResource1" />
                                            </ItemTemplate>
                                            <FooterStyle Wrap="False" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle CssClass="centerObj" />
                                </vdms:PageGridView>
                            </div>
                            <!-- Total summary -->
                            <table style="width: 100%;" border="0">
                                <tr>
                                    <td style="width: 99%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 1%">
                                        <table style="width: 400px" border="0">
                                            <tr>
                                                <td class="summaryName">
                                                    <asp:Literal ID="Literal52" runat="server" Text="Spares amount(VND)" meta:resourcekey="Literal52Resource1"></asp:Literal>
                                                </td>
                                                <td class="summaryValue">
                                                    <asp:Literal ID="litSparesAmount" runat="server" meta:resourcekey="litSparesAmountResource1"></asp:Literal>
                                                </td>
                                                <td class="summaryValue">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="summaryName">
                                                    <asp:Literal ID="Literal53" runat="server" Text="Fee amount(VND)" meta:resourcekey="Literal53Resource1"></asp:Literal>
                                                </td>
                                                <td class="summaryValue">
                                                    <asp:TextBox ID="txtFee" runat="server" MaxLength="30" Width="200px" meta:resourcekey="txtFeeResource1"></asp:TextBox>
                                                    <ajaxToolkit:FilteredTextBoxExtender TargetControlID="txtFee" FilterType="Numbers"
                                                        ID="FilteredTextBoxExtender3" runat="server" Enabled="True">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                                </td>
                                                <td class="summaryValue">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorx16" runat="server" ControlToValidate="txtFee"
                                                        Enabled="False" EnableTheming="True" ErrorMessage="&quot;Fee amount&quot; cannot be blank!"
                                                        Text="*" ValidationGroup="Save" Visible="False" meta:resourcekey="RequiredFieldValidatorx16Resource1"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtFee"
                                                        ErrorMessage="&quot;Fee amount&quot; must be numeric!" Text="*" ValidationExpression="\s*\d*\s*"
                                                        ValidationGroup="Save" meta:resourcekey="RegularExpressionValidator1Resource1"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="summaryName">
                                                    <asp:Literal ID="Literal54" runat="server" Text="Total amount(VND)" meta:resourcekey="Literal54Resource1"></asp:Literal>
                                                </td>
                                                <td class="summaryValue">
                                                    <asp:Literal ID="litTotalAmount" runat="server" meta:resourcekey="litTotalAmountResource1"></asp:Literal>
                                                </td>
                                                <td class="summaryValue">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnReloadSRSItems" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel runat="server" ID="tpnPCV" HeaderText="Part change voucher"
                meta:resourcekey="tpnPCVResource1">
                <HeaderTemplate>
                    <asp:Literal Text="Part change voucher" ID="Literal64" runat="server" meta:resourcekey="litTabPCVHeaderResource1"></asp:Literal>
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:UpdatePanel ID="udpPCV" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form">
                                <table border="0" cellpadding="2" cellspacing="2" style="width: 750px">
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td style="width: 1%">
                                        </td>
                                        <td style="width: 1%">
                                        </td>
                                        <td style="width: 30%">
                                        </td>
                                        <td class="nameField" align="right" colspan="2">
                                            <asp:Literal ID="Literal34" runat="server" Text="Receipt:" meta:resourcekey="Literal34Resource1"></asp:Literal>
                                            <asp:TextBox ID="txtexReceipt" runat="server" CssClass="readOnlyInputField" MaxLength="30"
                                                ReadOnly="True" Width="186px" meta:resourcekey="txtexReceiptResource1"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            <div class="grid">
                                                <table class="datatable" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <caption>
                                                        <asp:Literal ID="Literal18" runat="server" Text="Vehicle and customer" meta:resourcekey="Literal18Resource1"></asp:Literal></caption>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="nameField" style="width: 25%">
                                            <asp:Literal ID="Literal32" runat="server" Text="Engine number:" meta:resourcekey="Literal32Resource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtexEngineNumber" runat="server" CssClass="readOnlyInputField"
                                                MaxLength="20" ReadOnly="True" meta:resourcekey="txtexEngineNumberResource1"></asp:TextBox>
                                        </td>
                                        <td style="width: 1%">
                                        </td>
                                        <td style="width: 1%">
                                        </td>
                                        <td style="width: 30%">
                                        </td>
                                        <td class="nameField" style="width: 25%">
                                            <asp:Literal ID="Literal40" runat="server" Text="Frame number:" meta:resourcekey="Literal40Resource1"></asp:Literal>
                                        </td>
                                        <td style="width: 1%">
                                            <asp:TextBox ID="txtexFrameNum" runat="server" CssClass="readOnlyInputField" MaxLength="30"
                                                ReadOnly="True" meta:resourcekey="txtexFrameNumResource1"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="nameField" style="width: 25%">
                                            <asp:Literal ID="Literal44" runat="server" Text="Model:" meta:resourcekey="Literal44Resource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtexModel" runat="server" CssClass="readOnlyInputField" MaxLength="20"
                                                ReadOnly="True" meta:resourcekey="txtexModelResource1"></asp:TextBox>
                                        </td>
                                        <td style="width: 1%">
                                        </td>
                                        <td style="width: 1%">
                                        </td>
                                        <td style="width: 30%">
                                        </td>
                                        <td class="nameField" style="width: 25%">
                                            <asp:Literal ID="Literal45" runat="server" Text="Kilometers:" meta:resourcekey="Literal45Resource1"></asp:Literal>
                                        </td>
                                        <td style="width: 1%">
                                            <asp:TextBox ID="txtexKm" runat="server" CssClass="readOnlyInputField" MaxLength="15"
                                                ReadOnly="True" meta:resourcekey="txtexKmResource1"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="nameField" style="width: 25%">
                                            <asp:Literal ID="Literal39" runat="server" Text="Customer name:" meta:resourcekey="Literal39Resource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtexCustName" runat="server" CssClass="readOnlyInputField" ReadOnly="True"
                                                meta:resourcekey="txtexCustNameResource1"></asp:TextBox>
                                        </td>
                                        <td style="width: 1%">
                                        </td>
                                        <td style="width: 1%">
                                        </td>
                                        <td style="width: 30%">
                                        </td>
                                        <td class="nameField" style="width: 25%">
                                            <asp:Literal ID="Literal42" runat="server" Text="Phone:" meta:resourcekey="Literal42Resource1"></asp:Literal>
                                        </td>
                                        <td style="width: 1%">
                                            <asp:TextBox ID="txtexPhone" runat="server" CssClass="readOnlyInputField" ReadOnly="True"
                                                meta:resourcekey="txtexPhoneResource1"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="nameField" style="width: 25%">
                                            <asp:Literal ID="Literal41" runat="server" Text="Address:" meta:resourcekey="Literal41Resource1"></asp:Literal>
                                        </td>
                                        <td colspan="6">
                                            <asp:TextBox ID="txtexAddress" runat="server" Width="99%" CssClass="readOnlyInputField"
                                                ReadOnly="True" meta:resourcekey="txtexAddressResource1"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%">
                                            <asp:Literal ID="Literal43" runat="server" Text="Dealer:" meta:resourcekey="Literal43Resource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtexDealer" runat="server" CssClass="readOnlyInputField" ReadOnly="True"
                                                meta:resourcekey="txtexDealerResource1"></asp:TextBox>
                                        </td>
                                        <td style="width: 1%">
                                        </td>
                                        <td style="width: 1%">
                                        </td>
                                        <td style="width: 30%">
                                        </td>
                                        <td style="width: 25%">
                                            <asp:Literal ID="Literal47" runat="server" Text="Area code:" meta:resourcekey="Literal47Resource1"></asp:Literal>
                                        </td>
                                        <td style="width: 1%">
                                            <asp:TextBox ID="txtexAreaCode" runat="server" CssClass="readOnlyInputField" ReadOnly="True"
                                                meta:resourcekey="txtexAreaCodeResource1"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%">
                                            <asp:Literal ID="Literal46" runat="server" Text="Buy date:" meta:resourcekey="Literal46Resource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtexBuyDate" runat="server" CssClass="readOnlyInputField" ReadOnly="True"
                                                meta:resourcekey="txtexBuyDateResource1"></asp:TextBox>
                                        </td>
                                        <td style="width: 1%">
                                        </td>
                                        <td style="width: 1%">
                                        </td>
                                        <td style="width: 30%">
                                        </td>
                                        <td style="width: 25%">
                                            <asp:Literal ID="Literal50" runat="server" Text="Repair date:" meta:resourcekey="Literal50Resource1"></asp:Literal>
                                        </td>
                                        <td style="width: 1%">
                                            <asp:TextBox ID="txtexRepairDate" runat="server" CssClass="readOnlyInputField" ReadOnly="True"
                                                meta:resourcekey="txtexRepairDateResource1"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%">
                                            <asp:Image ID="Image2a" runat="server" SkinID="RequireField" meta:resourcekey="Image2aResource1" />
                                            <asp:Literal ID="Literal49" runat="server" Text="Damaged date:" meta:resourcekey="Literal49Resource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtexDamagedDate" runat="server" Width="119px" meta:resourcekey="txtexDamagedDateResource1"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnDamagedDateCalendar" runat="server" OnClientClick="return false;"
                                                SkinID="CalendarImageButton" meta:resourcekey="ibtnDamagedDateCalendarResource1" />
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                                PopupButtonID="ibtnDamagedDateCalendar" TargetControlID="txtexDamagedDate">
                                            </ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" runat="server" Enabled="True"
                                                Mask="99/99/9999" MaskType="Date" TargetControlID="txtexDamagedDate" CultureAMPMPlaceholder=""
                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>
                                        <td style="width: 1%">
                                        </td>
                                        <td style="width: 1%">
                                        </td>
                                        <td style="width: 30%">
                                        </td>
                                        <td style="width: 25%">
                                            <asp:Literal ID="Literal48" runat="server" Text="Made date:" meta:resourcekey="Literal48Resource1"></asp:Literal>
                                        </td>
                                        <td style="width: 1%">
                                            <asp:TextBox ID="txtexExportDate" runat="server" CssClass="readOnlyInputField" ReadOnly="True"
                                                meta:resourcekey="txtexExportDateResource1"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%">
                                            <asp:Literal ID="Literal33" runat="server" Text="Using condition" meta:resourcekey="Literal33Resource1"></asp:Literal>
                                        </td>
                                        <td class="forceNowrap" colspan="6">
                                            <div class="grid">
                                                <table border="0" cellpadding="0" cellspacing="0" class="datatable">
                                                    <tr>
                                                        <th>
                                                            <asp:Literal ID="Literal37" runat="server" Text="Road" meta:resourcekey="Literal37Resource1"></asp:Literal>
                                                        </th>
                                                        <th>
                                                            <asp:Literal ID="Literal19" runat="server" Text="Weather" meta:resourcekey="Literal19Resource1"></asp:Literal>
                                                        </th>
                                                        <th>
                                                            <asp:Literal ID="Literal20" runat="server" Text="Speed" meta:resourcekey="Literal20Resource1"></asp:Literal>
                                                        </th>
                                                        <th>
                                                            <asp:Literal ID="Literal21" runat="server" Text="Transport" meta:resourcekey="Literal21Resource1"></asp:Literal>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: #f6f6f6">
                                                            <asp:RadioButtonList ID="rblRoad" CssClass="nCellInDataTable" runat="server" RepeatDirection="Horizontal"
                                                                meta:resourcekey="rblRoadResource1">
                                                                <asp:ListItem Value="0" Selected="True" Text="Good" meta:resourcekey="ListItemResource4"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Bad" meta:resourcekey="ListItemResource5"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="Mountain" meta:resourcekey="ListItemResource6"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td style="background-color: #f6f6f6">
                                                            <asp:RadioButtonList ID="rblWeather" CssClass="nCellInDataTable" runat="server" RepeatDirection="Horizontal"
                                                                meta:resourcekey="rblWeatherResource1">
                                                                <asp:ListItem Value="0" Selected="True" Text="Sunny" meta:resourcekey="ListItemResource7"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Rainning" meta:resourcekey="ListItemResource8"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td style="background-color: #f6f6f6">
                                                            <asp:RadioButtonList ID="rblSpeed" CssClass="nCellInDataTable" runat="server" RepeatDirection="Horizontal"
                                                                meta:resourcekey="rblSpeedResource1">
                                                                <asp:ListItem Value="0" Text="Galanti" meta:resourcekey="ListItemResource9"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Slow" meta:resourcekey="ListItemResource10"></asp:ListItem>
                                                                <asp:ListItem Value="2" Selected="True" Text="Normal" meta:resourcekey="ListItemResource11"></asp:ListItem>
                                                                <asp:ListItem Value="3" Text="High" meta:resourcekey="ListItemResource12"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td style="background-color: #f6f6f6">
                                                            <asp:RadioButtonList ID="rblTransport" CssClass="nCellInDataTable" runat="server"
                                                                RepeatDirection="Horizontal" meta:resourcekey="rblTransportResource1">
                                                                <asp:ListItem Value="0" Text="Goods" meta:resourcekey="ListItemResource13"></asp:ListItem>
                                                                <asp:ListItem Value="1" Selected="True" Text="Human" meta:resourcekey="ListItemResource14"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            <div class="grid">
                                                <table class="datatable" border="0" cellpadding="0" cellspacing="0" style="width: 100%"
                                                    width="100%">
                                                    <caption>
                                                        <asp:Literal ID="Literal22" runat="server" Text="Damage informations" meta:resourcekey="Literal22Resource1"></asp:Literal></caption>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="nameField" style="width: 25%">
                                            <asp:Literal ID="Literal23" runat="server" Text="Engine:" meta:resourcekey="Literal23Resource1"></asp:Literal>
                                        </td>
                                        <td colspan="6">
                                            <asp:TextBox ID="txtexEngineDmg" runat="server" Width="98%" MaxLength="256" meta:resourcekey="txtexEngineDmgResource1"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="nameField" style="width: 25%">
                                            <asp:Literal ID="Literal24" runat="server" Text="Frame:" meta:resourcekey="Literal24Resource1"></asp:Literal>
                                        </td>
                                        <td colspan="6">
                                            <asp:TextBox ID="txtexFrameDmg" runat="server" Width="98%" MaxLength="256" meta:resourcekey="txtexFrameDmgResource1"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="nameField" style="width: 25%">
                                            <asp:Literal ID="Literal25" runat="server" Text="Electrical:" meta:resourcekey="Literal25Resource1"></asp:Literal>
                                        </td>
                                        <td colspan="6">
                                            <asp:TextBox ID="txtexElectricalDmg" runat="server" Width="98%" MaxLength="256" meta:resourcekey="txtexElectricalDmgResource1"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="nameField" style="width: 25%">
                                            <asp:Image ID="Image4" runat="server" SkinID="RequireField" meta:resourcekey="Image4Resource1" /><asp:Literal
                                                ID="Literal26" runat="server" Text="Damage:" meta:resourcekey="Literal26Resource1"></asp:Literal>
                                        </td>
                                        <td colspan="6">
                                            <asp:TextBox ID="txtexDamage" runat="server" Rows="3" TextMode="MultiLine" Width="98%"
                                                MaxLength="512" meta:resourcekey="txtexDamageResource1"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvexDamage" runat="server" ControlToValidate="txtexDamage"
                                                ErrorMessage="Damage cannot be blank!" ValidationGroup="none" Text="*" meta:resourcekey="rfvexDamageResource1"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="nameField" style="width: 25%">
                                            <asp:Image ID="Image2" runat="server" SkinID="RequireField" meta:resourcekey="Image2Resource1" /><asp:Literal
                                                ID="Literal27" runat="server" Text="Reason:" meta:resourcekey="Literal27Resource1"></asp:Literal>
                                        </td>
                                        <td colspan="6">
                                            <asp:TextBox ID="txtexReason" runat="server" Rows="3" TextMode="MultiLine" Width="98%"
                                                MaxLength="512" meta:resourcekey="txtexReasonResource1"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvexReason" runat="server" ControlToValidate="txtexReason"
                                                ErrorMessage="Reason cannot be blank!" ValidationGroup="none" Text="*" meta:resourcekey="rfvexReasonResource1"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="nameField" style="width: 25%">
                                            <asp:Literal ID="Literal28" runat="server" Text="Note:" meta:resourcekey="Literal28Resource1"></asp:Literal>
                                        </td>
                                        <td colspan="6">
                                            <asp:TextBox ID="txtexNote" runat="server" Width="98%" MaxLength="1000" Rows="3"
                                                TextMode="MultiLine" meta:resourcekey="txtexNoteResource1"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="udpPCVItems" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                        <ContentTemplate>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnReloadPCVItems" UseSubmitBehavior="False" CssClass="hidden" Text="Refresh"
                                                        runat="server" OnClick="btnReloadPCVItems_Click" meta:resourcekey="btnReloadPCVItemsResource1" />
                                                </td>
                                                <td>
                                                    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="udpPCVItems"
                                                        DisplayAfter="0" DynamicLayout="False">
                                                        <ProgressTemplate>
                                                            <div style="display: inline;">
                                                                <img src="../../Images/Spinner.gif" alt="" />
                                                                <asp:Literal ID="Literal31" runat="server" Text="Updating..." meta:resourcekey="Literal31Resource2"></asp:Literal>
                                                            </div>
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rightObj">
                                        <%--<a id='_lnkSelectSpare' class="thickbox" href="Popup/SelectSpares.aspx?key=<%= this.Info.PageKey %>&TB_iframe=true&width=800"></a>--%>
                                        <asp:Button ID="btnPCVCallSelSpare" UseSubmitBehavior="false" OnClientClick="showPopUp('_lnkSelSpare'); return false;"
                                            Text="Add spares" runat="server" meta:resourcekey="btnPCVCallSelSpareResource1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ObjectDataSource ID="odsBroken" runat="server" SelectMethod="Select" TypeName="VDMS.I.ObjectDataSource.BrokenDatasource">
                                        </asp:ObjectDataSource>
                                        <asp:ValidationSummary runat="server" ID="vsPCVList" ValidationGroup="InsertPCVItem"
                                            meta:resourcekey="vsPCVListResource1" />
                                        <div class="grid">
                                            <vdms:PageGridView ID="gvexSpareList" runat="server" AllowInsertEmptyRow="False"
                                                AutoGenerateColumns="False" CssClass="GridView" ShowFooter="True" OnDataBound="gvexSpareList_DataBound"
                                                OnRowDataBound="gvexSpareList_RowDataBound" EmptyDataText="There are no spares in list!"
                                                meta:resourcekey="gvexSpareListResource1">
                                                <EmptyDataRowStyle CssClass="emptyGVrow" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource32">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgWarrantyWarn" runat="server" Visible='<%# Eval("WarrantyWarn") %>'
                                                                ToolTip="May be avoid warranty!" ImageUrl="~/Images/s_mustinput.gif" meta:resourcekey="imgWarrantyWarnResource1" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Spare number" meta:resourcekey="TemplateFieldResource33">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litexSpareNumber" runat="server" Text='<%# Eval("Partcodem") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <table cellspacing="0" cellpadding="0" border="0">
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtNewPCVSpareNumber" runat="server" MaxLength="35" meta:resourcekey="txtNewPCVSpareNumberResource1"
                                                                                Width="90px"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <input id="_btnPCVCallSelSpare" name="_btnPCVCallSelSpare" onclick="onCallSelectSpare('PCV')"
                                                                                type="button" value="..." />
                                                                        </td>
                                                                        <td>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtNewPCVSpareNumber"
                                                                                ErrorMessage="Spare number cannot be blank!" meta:resourcekey="RequiredFieldValidator6Resource1"
                                                                                Text="*" ValidationGroup="InsertPCVItem"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        <td>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtNewPCVSpareNumber"
                                                                                ErrorMessage="&quot;Spare number&quot; must input at least 6 non-white-space character (maximum is 35)!"
                                                                                meta:resourcekey="RegularExpressionValidator3Resource1" Text="*" ValidationExpression="\s*[\S|\s]{6,35}\s*"
                                                                                ValidationGroup="InsertPCVItem"></asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </FooterTemplate>
                                                        <HeaderStyle Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Spare name" meta:resourcekey="TemplateFieldResource34">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("PartName") %>' meta:resourcekey="Label1Resource1"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtNewPCVSpareName" ReadOnly="True" runat="server" meta:resourcekey="txtNewPCVSpareNameResource1"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <HeaderStyle Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quantity" meta:resourcekey="TemplateFieldResource35">
                                                        <ItemTemplate>
                                                            <table cellspacing="0" cellpadding="0" border="0">
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtexQuantity" runat="server" AutoPostBack="True" MaxLength="4"
                                                                                OnTextChanged="PCVItems_UpdateRow" Text='<%# Bind("Partqtyo") %>' Width="30px"
                                                                                meta:resourcekey="txtexQuantityResource1"></asp:TextBox>
                                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                                                Enabled="True" FilterType="Numbers" TargetControlID="txtexQuantity">
                                                                            </ajaxToolkit:FilteredTextBoxExtender>
                                                                        </td>
                                                                        <td>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtexQuantity"
                                                                                ErrorMessage="Quantity cannot be blank!" Text="*" ValidationGroup="InsertPCVItem"
                                                                                meta:resourcekey="RequiredFieldValidator3Resource1"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <table cellspacing="0" cellpadding="0" border="0">
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtNewPCVQuantity" runat="server" MaxLength="4" meta:resourcekey="txtNewPCVQuantityResource1"
                                                                                Text="1" Width="30px"></asp:TextBox>
                                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                                                Enabled="True" FilterType="Numbers" TargetControlID="txtNewPCVQuantity">
                                                                            </ajaxToolkit:FilteredTextBoxExtender>
                                                                        </td>
                                                                        <td>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNewPCVQuantity"
                                                                                ErrorMessage="Quantity cannot be blank!" meta:resourcekey="RequiredFieldValidator3Resource2"
                                                                                Text="*" ValidationGroup="InsertPCVItem"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </FooterTemplate>
                                                        <HeaderStyle Wrap="False" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Broken code" meta:resourcekey="TemplateFieldResource36">
                                                        <ItemTemplate>
                                                            <table cellspacing="0" cellpadding="0" border="0">
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlBroken" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                                                                DataSourceID="odsBroken" DataTextField="Brokenname" DataValueField="Brokencode"
                                                                                OnDataBound="ddlBroken_OnDataBound" OnSelectedIndexChanged="PCVItems_UpdateRow"
                                                                                ToolTip='<%# Eval("Broken.Brokencode") %>' Width="150px">
                                                                                <asp:ListItem meta:resourcekey="ListItemResource15" Selected="True"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlBroken"
                                                                                ErrorMessage="BrokenCode cannot be blank!" meta:resourceKey="RequiredFieldValidator4Resource1"
                                                                                Text="*" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <table cellspacing="0" cellpadding="0" border="0">
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlNewPCVBrokenCode" runat="server" AppendDataBoundItems="True"
                                                                                DataSourceID="odsBroken" DataTextField="Brokenname" DataValueField="Brokencode"
                                                                                meta:resourcekey="ddlNewPCVBrokenCodeResource1" Width="150px">
                                                                                <asp:ListItem meta:resourcekey="ListItemResource16" Selected="True"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlNewPCVBrokenCode"
                                                                                ErrorMessage="BrokenCode cannot be blank!" meta:resourceKey="RequiredFieldValidator4Resource1"
                                                                                Text="*" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </FooterTemplate>
                                                        <HeaderStyle Wrap="False" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unit price(VND)" meta:resourcekey="TemplateFieldResource37">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# Bind("Unitpriceo") %>' ID="Label6" meta:resourcekey="Label6Resource2"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtNewPCVSparePrice" ReadOnly="True" runat="server" meta:resourcekey="txtNewPCVSparePriceResource1"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <HeaderStyle Wrap="False" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Spare amount(VND)" meta:resourcekey="TemplateFieldResource38">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# Bind("SpareAmountO") %>' ID="Label5" meta:resourcekey="Label5Resource2"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Wrap="False" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Serial number" meta:resourcekey="TemplateFieldResource39">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtexSerialNumber" AutoPostBack="True" OnTextChanged="PCVItems_UpdateRow"
                                                                runat="server" Text='<%# Bind("Serialnumber") %>' MaxLength="30" Width="110px"
                                                                meta:resourcekey="txtexSerialNumberResource1"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtexSerialNumber" runat="server" Text='<%# Bind("Serialnumber") %>'
                                                                MaxLength="30" Width="110px" meta:resourcekey="txtexSerialNumberResource2"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <HeaderStyle Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ManPower" meta:resourcekey="TemplateFieldResource40">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# Bind("SManPower") %>' ID="Label2" meta:resourcekey="Label2Resource3"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Label runat="server" Text='<%# Eval("SManPower") %>' ID="Label2" meta:resourcekey="Label2Resource4"></asp:Label>
                                                        </EditItemTemplate>
                                                        <HeaderStyle Wrap="False" />
                                                        <ItemStyle CssClass="valueField" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Labour" meta:resourcekey="TemplateFieldResource41">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# Bind("Labour") %>' ID="Label3" meta:resourcekey="Label3Resource1"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Label runat="server" Text='<%# Eval("Labour") %>' ID="Label3" meta:resourcekey="Label3Resource2"></asp:Label>
                                                        </EditItemTemplate>
                                                        <HeaderStyle Wrap="False" />
                                                        <ItemStyle CssClass="valueField" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fee amount(VND)" meta:resourcekey="TemplateFieldResource42">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# Bind("FeeAmount") %>' ID="Label4" meta:resourcekey="Label4Resource1"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Label runat="server" Text='<%# Eval("FeeAmount") %>' ID="Label4" meta:resourcekey="Label4Resource2"></asp:Label>
                                                        </EditItemTemplate>
                                                        <HeaderStyle Wrap="False" />
                                                        <ItemStyle CssClass="valueField" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource43">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imbDeletePCVItem" runat="server" Text="Delete" ImageUrl="~/Images/Delete.gif"
                                                                CommandArgument='<%# Eval("Partcodeo") %>' OnClick="imbDeletePCVItem_Click" CausesValidation="False"
                                                                OnDataBinding="ImageButton1_DataBinding" meta:resourcekey="imbDeletePCVItemResource1">
                                                            </asp:ImageButton>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:ImageButton ID="imbAddPCVItem" ValidationGroup="InsertPCVItem" runat="server"
                                                                ImageUrl="~/Images/update.gif" OnClick="btnAddExchangeSpare_Click" meta:resourcekey="imbAddPCVItemResource1">
                                                            </asp:ImageButton>
                                                        </FooterTemplate>
                                                        <HeaderStyle Wrap="False" />
                                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </vdms:PageGridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rightObj">
                                        &nbsp;<asp:Literal ID="Literal56" runat="server" Text="Total fee amount:" meta:resourcekey="Literal56Resource1"></asp:Literal>
                                        <asp:TextBox ID="txtexFeeOffer" runat="server" MaxLength="15" meta:resourcekey="txtexFeeOfferResource1"></asp:TextBox>
                                        <ajaxToolkit:FilteredTextBoxExtender FilterType="Numbers" TargetControlID="txtexFeeOffer"
                                            ID="FilteredTextBoxExtender4" runat="server" Enabled="True">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                        <asp:RegularExpressionValidator ID="revexTotalFee" runat="server" ControlToValidate="txtexFeeOffer"
                                            ErrorMessage="Total fee must be numeric!" Text="*" ValidationExpression="\s*\d*\s*"
                                            ValidationGroup="Save" meta:resourcekey="revexTotalFeeResource1"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table class="hidden" border="0" cellpadding="0" cellspacing="0" runat="server" id="tblWarrantyHint">
                                            <tr id="Tr1" runat="server">
                                                <td id="Td1" runat="server">
                                                    &nbsp;<asp:Image ID="Image6" runat="server" ImageUrl="~/Images/s_mustinput.gif" />&nbsp;
                                                </td>
                                                <td id="Td2" runat="server">
                                                    :
                                                    <asp:Literal meta:resourcekey="litWarrantyWarnResource1" ID="Literal57" runat="server"
                                                        Text="May be avoid warranty!"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr id="Tr2" runat="server">
                                                <td id="Td3" runat="server">
                                                </td>
                                                <td id="Td4" runat="server">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="centerObj">
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>
        <asp:UpdatePanel ID="udpCommand" runat="server">
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td>
                        </td>
                        <td class="rightObj">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:ValidationSummary ID="vsSave2" Visible="False" ValidationGroup="Save" runat="server"
                                meta:resourcekey="vsSave2Resource1" />
                        </td>
                    </tr>
                    <tr>
                        <td class="rightObj" style="height: 24px">
                            <asp:Button ID="btnSelectSpares" OnClick="btnSelectSpares_Click" runat="server" CssClass="hidden"
                                meta:resourcekey="btnSelectSparesResource1" />
                            <asp:Button ID="btnSaveTemp" runat="server" Text="Temporary save" ValidationGroup="SaveTemp"
                                CommandName="SaveTemp" OnClick="btnSaveTemp_Click" meta:resourcekey="btnSaveTempResource1" />
                            <asp:Button ID="btnSave" runat="server" Text="Save " Width="100px" OnClick="btnSave_Click"
                                ValidationGroup="Save" CommandName="Save" meta:resourcekey="btnSaveResource1" />&nbsp;
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" OnClick="btnEdit_Click" meta:resourcekey="btnEditResource1" />&nbsp;
                            <asp:Button ID="btnCheckDataInput" runat="server" Text="Finish" ValidationGroup="Save"
                                OnClick="btnCheckDataInput_Click" meta:resourcekey="btnCheckDataInputResource1" />&nbsp;
                            <asp:Button PostBackUrl="~/Service/Report/PrintSRS.aspx" ID="btnPrint" runat="server"
                                Text="Print service sheet" Width="163px" Enabled="False" meta:resourcekey="btnPrintResource1"
                                OnClick="btnPrint_Click" />
                            <asp:Button ID="btnPrintPcv" runat="server" Enabled="False" Text="Print parts change"
                                Width="159px" ValidationGroup="Save" meta:resourcekey="btnPrintPcvResource1" />
                        </td>
                        <td class="rightObj" style="height: 24px">
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <%--</ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>--%>

    <script language="javascript" type="text/javascript">
        function switchTab() {
            var hasExchange = $get('<%=chblSerList.ClientID%>_2').checked;
            $find('<%=Tabs.ClientID%>').get_tabs()[1].set_enabled(hasExchange);
            __doPostBack('<%=chblSerList.UniqueID%>', '');
        } 
        
    </script>

    <br />
</asp:Content>
