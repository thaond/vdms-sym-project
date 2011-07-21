<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="Customer.aspx.cs" Inherits="Sales_Sale_Customer" Title="Nhập dữ liệu khách hàng"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <input type="hidden" runat="server" id="_PageStatus" value="Insertnew" enableviewstate="true" />
    <input type="hidden" runat="server" id="_InvoiceID" value="-1" enableviewstate="true" />
    <input type="hidden" runat="server" id="_CustomerID" value="-1" enableviewstate="true" />
    <input type="hidden" runat="server" id="_ItemInstanceID" value="-1" enableviewstate="true" />
    <input type="hidden" runat="server" id="_CustomerAction" value="Reset" enableviewstate="true" />
    <input type="hidden" runat="server" id="ddlSex" value="0" enableviewstate="true" />
    <input type="hidden" runat="server" id="txtBirthDate" value="-1" enableviewstate="true" />
    <input type="hidden" runat="server" id="txtAddress" value="-1" enableviewstate="true" />
    <input type="hidden" runat="server" id="ddlProvince" value="0" enableviewstate="true" />
    <input type="hidden" runat="server" id="txtDistrict" value="-1" enableviewstate="true" />
    <input type="hidden" runat="server" id="tblCus_JobType" value="0" enableviewstate="true" />
    <input type="hidden" runat="server" id="txtCEmail" value="-1" enableviewstate="true" />
    <input type="hidden" runat="server" id="txtCPhone" value="-1" enableviewstate="true" />
    <input type="hidden" runat="server" id="txtCMobile" value="-1" enableviewstate="true" />
    <input type="hidden" runat="server" id="ddlCus_SetType" value="0" enableviewstate="true" />
    <input type="hidden" runat="server" id="ddlCusType" value="0" enableviewstate="true" />
    <input type="hidden" runat="server" id="txtCus_Desc" value="-1" enableviewstate="true" />
    <input type="hidden" runat="server" id="txtPrecinct" value="-1" enableviewstate="true" />
    <input type="hidden" runat="server" id="_CustomerFullName" value="" enableviewstate="true" />

    <script language="javascript" type="text/javascript">
        function retrieve_lookup_data(CustomerObj, hrefURL) {
            var objCustomerID = document.getElementById(CustomerObj);
            var objEngineNo = document.getElementById('ctl00_c_txtEngineNo');
            var objMotorType = document.getElementById('ctl00_c_liMotorType');
            var objCusId = document.getElementById('ctl00_c__CustomerID');

            if ((objEngineNo.value.trim() == "") || (objMotorType.value.trim() == "")) {
                alert('<%= LoadCustomerErr %>');
                return false;
            }

            //            hrefURL += "cusid=" + objCustomerID.value.trim();
            hrefURL += "cusid=" + objCusId.value.trim();
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

            var arrParams = new Array();
            arrParams[0] = window;
            arrParams[1] = 'ctl00_c__CustomerID';
            arrParams[2] = '<%= txtCustomerIdentifyNumber.ClientID %>';
            arrParams[3] = 'ctl00_c__CustomerAction';
            arrParams[4] = 'ctl00_c_ddlSex';
            arrParams[5] = 'ctl00_c_txtBirthDate';
            arrParams[6] = 'ctl00_c_txtAddress';
            arrParams[7] = 'ctl00_c_ddlProvince';
            arrParams[8] = 'ctl00_c_txtDistrict';
            arrParams[9] = 'ctl00_c_tblCus_JobType';
            arrParams[10] = 'ctl00_c_txtCEmail';
            arrParams[11] = 'ctl00_c_txtCPhone';
            arrParams[12] = 'ctl00_c_txtCMobile';
            arrParams[13] = 'ctl00_c_ddlCus_SetType';
            arrParams[14] = 'ctl00_c_ddlCusType';
            arrParams[15] = 'ctl00_c_txtCus_Desc';
            arrParams[16] = 'ctl00_c_txtPrecinct';
            arrParams[17] = 'ctl00_c_lbCustomerFullName';
            arrParams[18] = 'ctl00_c__CustomerFullName';

            var objCustomerAction = document.getElementById('ctl00_c__CustomerAction');
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

    <script language="javascript">

        function cusSelected(id, name, cmnd) {
            tb_remove();
//            alert(document.getElementById('<%= _CustomerID.ClientID %>').value);
            document.getElementById('<%= _CustomerID.ClientID %>').value = id;
            document.getElementById('<%= _CustomerFullName.ClientID %>').value = name;
            document.getElementById('<%= lbCustomerFullName.ClientID %>').value = name;
            document.getElementById('<%= txtCustomerIdentifyNumber.ClientID %>').value = cmnd;
//            alert(document.getElementById('<%= _CustomerID.ClientID %>').value);
        }
        
        function ApplyToNumFriendlyFormat(UnfriendlyControlName) {
            var objUnFriendly = document.getElementById(UnfriendlyControlName);
            var FriendlyValue = objUnFriendly.value;
            var FriendlyArr = new Array();
            count = 0;
            for (i = FriendlyValue.length; i > 0; i--) {
                count++;
                //bypass the first value and not mod 3
                if (((i != FriendlyValue.length) && (i != 1)) && ((count % 3) == 0)) {
                    if (true) {
                        FriendlyArr.push(FriendlyValue.charAt(i - 1));
                        FriendlyArr.push('.');
                    }
                }
                else FriendlyArr.push(FriendlyValue.charAt(i - 1));
            }
            var FriendStr = '';
            FriendlyArr.reverse();
            for (i = 0; i < FriendlyArr.length; i++) {
                FriendStr += FriendlyArr[i];
            }
            return FriendStr;
        }
        //        function GetStatusHirePurchase()
        //        {
        //            var objStatus = document.getElementById("ctl00_c_ddlPaymentMethod");
        //            if (objStatus.value == "0")
        //            {
        //                return true;
        //            }
        //            if (objStatus.value == "1")
        //            {
        //                
        //                return true;
        //            }
        //            if (objStatus.value == "2")
        //            {
        //                if(CheckValidUFHP())
        //                {
        //                return true; 
        //                }
        //                else return false;
        //            }
        //        } 
        function ApplySumPaidMoney() {
            var PaidMoney = document.getElementById("ctl00_c_txtPriceTax");  //Price include tax
            var SumOfFHP = document.getElementById("ctl00_c_txtFHPAllMoney"); //Price of all PH
            SumOfFHP.innerHTML = PaidMoney.value;
        }
        //        function AutoGenSumForFPH()
        //        {
        //            var FPHTimes =  document.getElementById("ctl00_c_txtPriceTax");  //FHP times
        //            var PaidMoney = document.getElementById("ctl00_c_txtPriceTax");  //Price include tax
        //            var NumberDate =  document.getElementById("ctl00_c_txtFHPPaidMoneyDate"); //DD
        //            var FirstMoney = document.getElementById("ctl00_c_txtFHPFirstMoney"); //First Money
        //            var LastDate = document.getElementById("ctl00_c_FHPPaidDateLast"); //Last Date
        //            var InstamentMoney = document.getElementById("ctl00_c_lbMoneyOfTimes"); //Per Money per instament
        //            
        //            if( (PaidMoney.value != "") && (NumberDate.value != "") && (FirstMoney.value != "") )
        //            {
        //                //
        //            }
        //            else return false;
        //        } 
        //       function CheckValidUFHP()
        //       {
        //            var objPriceIncludeTax = document.getElementById("ctl00_c_txtPriceTax");
        //            var SumaryMoney = objPriceIncludeTax.value;
        //            var countNumber = 0;
        //            
        //            for (int i=1;i<6;i++)
        //            {
        //                var objIntend = document.getElementById("ctl00_c_txtIntentDatePay"+i);
        //                var objtxtMoney = document.getElementById("ctl00_c_UHPtxtMoney"+i);
        //                if (objIntend.value != "" && objtxtMoney!="")
        //                {
        //                    countNumber += parseInt(objIntend.value);
        //                } 
        //            }
        //            
        //            if(countNumber != SumaryMoney)
        //            {
        //                alert('<%= SumMoneyInvalid %>');
        //                return false;    
        //            }
        //            else return true;
        //       } 
    </script>

    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <div class="form">
                <asp:Localize ID="Localize1" runat="server" Text="Input customer's information" meta:resourcekey="Localize1Resource1"></asp:Localize>
                <table cellspacing="0" border="0" style="width: 100%">
                    <tr>
                        <td colspan="2" valign="top">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Search"
                                Width="100%" DisplayMode="List" meta:resourcekey="ValidationSummary1Resource1" />
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="InsertOrUpdate"
                                Width="100%" DisplayMode="List" meta:resourcekey="ValidationSummary2Resource1" />
                            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="CheckInsertOrUpdate"
                                Width="100%" DisplayMode="List" meta:resourcekey="ValidationSummary3Resource1" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" nowrap="nowrap" valign="top">
                            <asp:Label ID="lbMes" runat="server" ForeColor="RoyalBlue" meta:resourcekey="lbMesResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" nowrap="nowrap">
                            <asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" /><asp:Literal
                                ID="LiEngineNo" runat="server" Text="Engine No:" meta:resourcekey="LiEngineNoResource1"></asp:Literal>
                        </td>
                        <td valign="top" nowrap="nowrap">
                            <asp:TextBox ID="txtEngineNo" runat="server" Width="232px" Style="text-transform: uppercase"
                                meta:resourcekey="txtEngineNoResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEngineNo" runat="server" SetFocusOnError="True"
                                ValidationGroup="Search" ControlToValidate="txtEngineNo" ErrorMessage='Dữ liệu "Số máy" không được để trống'
                                meta:resourcekey="rfvEngineNoResource1">*</asp:RequiredFieldValidator>
                            <asp:Button ID="btnSelectItem" runat="server" OnClick="btnSelectItem_Click" Text="..." />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" nowrap="nowrap">
                        </td>
                        <td valign="top" nowrap="nowrap">
                            <asp:Button ID="btnTest" runat="server" Text="Test" ValidationGroup="Search" OnClick="btnTest_Click"
                                meta:resourcekey="btnTestResource1" />
                            <asp:Button ID="btnAdd" runat="server" Text="Add new" OnClick="btnAdd_Click" meta:resourcekey="btnAddResource1" />
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" OnClick="btnEdit_Click" ValidationGroup="Search"
                                meta:resourcekey="btnEditResource1" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="form">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td>
                            <!-- Begin: Main input -->
                            <asp:PlaceHolder ID="plMainInput" runat="server" Visible="False">
                                <table border="0" cellpadding="2" cellspacing="2">
                                    <tr>
                                        <td align="center" colspan="4">
                                            <h5>
                                                <asp:Literal ID="SellingTitle" Text="Selling" runat="server" meta:resourcekey="SellingTitleLiteral"></asp:Literal></h5>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 25%">
                                            <asp:Image ID="Image2" runat="server" SkinID="RequireField" meta:resourcekey="Image2Resource1" />&nbsp;<asp:Literal
                                                ID="li" runat="server" Text="Selling date:" meta:resourcekey="liResource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSellingDate" runat="server" ReadOnly="True" CssClass="readOnlyInputField"
                                                meta:resourcekey="txtSellingDateResource1"></asp:TextBox>
                                            <asp:RangeValidator ID="rvSellingDate" runat="server" ControlToValidate="txtSellingDate"
                                                meta:resourcekey="rfvsellDate" SetFocusOnError="True" Type="Date" ValidationGroup="CheckInsertOrUpdate"></asp:RangeValidator>
                                            <asp:RequiredFieldValidator ID="rfvSellingDate" runat="server" ControlToValidate="txtSellingDate"
                                                ErrorMessage='Input &quot;Selling date&quot; can not be blank!' SetFocusOnError="True"
                                                ValidationGroup="CheckInsertOrUpdate" meta:resourcekey="rfvSellingDateResource1">*</asp:RequiredFieldValidator>
                                            <asp:ImageButton ID="imgbSellDate" runat="server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                                meta:resourcekey="ImageButton5Resource1" />
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtSellingDate" BehaviorID="MaskedEditExtender1"
                                                Enabled="True">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgbSellDate"
                                                TargetControlID="txtSellingDate" BehaviorID="CalendarExtender1" Enabled="True">
                                            </ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td align="left">
                                            <asp:Literal ID="liDealerCodeTitle" runat="server" Text="Dealer code:" meta:resourcekey="liDealerCodeTitleResource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="liDealerCode" runat="server" CssClass="readOnlyInputField" ReadOnly="True"
                                                meta:resourcekey="liDealerCodeResource1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 25%">
                                            <asp:Literal ID="Literal8" runat="server" Text="MotorType:" meta:resourcekey="Literal8Resource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="liMotorType" runat="server" CssClass="readOnlyInputField" ReadOnly="True"
                                                meta:resourcekey="liMotorTypeResource1"></asp:TextBox>
                                        </td>
                                        <td align="left" width="12%">
                                            <asp:Literal ID="Literal13" runat="server" Text="Warehouse:" meta:resourcekey="liKho"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtWarehouse" runat="server" CssClass="readOnlyInputField" ReadOnly="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 25%">
                                            <%--<asp:Image ID="Image1" runat="server" SkinID="RequireField" meta:resourcekey="Image2Resource1" />--%>
                                            <asp:Literal ID="Literal9" runat="server" Text="Bill No:" meta:resourcekey="Literal9Resource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillNo" runat="server" MaxLength="30" Style="text-transform: uppercase"
                                                meta:resourcekey="txtBillNoResource1"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator ID="rfvBillNo" runat="server" ControlToValidate="txtBillNo" 
                                            ErrorMessage='Input &quot;Bill No&quot; can not be blank!' SetFocusOnError="True"
                                            ValidationGroup="InsertOrUpdate" meta:resourcekey="rfvBillNoResource1">*</asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td align="left">
                                            <asp:Literal ID="liColorTitle" runat="server" Text="Color:" meta:resourcekey="liColorTitleResource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="liColor" runat="server" CssClass="readOnlyInputField" ReadOnly="True"
                                                meta:resourcekey="liColorResource1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 25%">
                                            <asp:Literal ID="Literal11" runat="server" Text="Store code:" meta:resourcekey="Literal11Resource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbSubStore" runat="server" Enabled="False" />
                                            <asp:TextBox ID="txtSubshop" runat="server" CssClass="readOnlyInputField" meta:resourcekey="liMotorTypeResource1"
                                                ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            <asp:Literal ID="Literal10" runat="server" Text="Tax:" meta:resourcekey="Literal10Resource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="liTax" runat="server" CssClass="readOnlyInputField" ReadOnly="True"
                                                meta:resourcekey="liTaxResource1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 25%">
                                            <%--<asp:Image ID="image8" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1"/>--%>
                                            <asp:Literal ID="LiPriceinCludeTax" runat="server" Text="Price (include tax):" meta:resourcekey="LiPriceinCludeTaxResource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPriceTax" runat="server" MaxLength="20" meta:resourcekey="txtPriceTaxResource1"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator ID="rfvPrice" runat="server" ControlToValidate="txtPriceTax" ErrorMessage='Input &quot;Price&quot; can not be blank!' SetFocusOnError="True"
                                                ValidationGroup="InsertOrUpdate" meta:resourcekey="rfvPriceResource1">*</asp:RequiredFieldValidator>--%>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPriceTax"
                                                ErrorMessage='Input &quot;Price&quot; can not be blank!' SetFocusOnError="True"
                                                ValidationGroup="CheckInsertOrUpdate" meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtPriceTax"
                                                ErrorMessage="Giá tiền phải được gõ bằng số!" ValidationExpression="\s*[0-9]*([.,]?\d*[1-9]+\d*)?\s*"
                                                ValidationGroup="CheckInsertOrUpdate" meta:resourcekey="RegularExpressionValidator2Resource1"
                                                Text="*"></asp:RegularExpressionValidator>
                                            <asp:Literal ID="LiPriceinCludeTaxUnit" runat="server" Text="(VND)"></asp:Literal>
                                        </td>
                                        <td align="left" nowrap="nowrap">
                                            <asp:Literal ID="Literal12" runat="server" Text="Payment method:" meta:resourcekey="Literal12Resource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlPaymentMethod" runat="server" Width="132px" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlPaymentMethod_SelectedIndexChanged" meta:resourcekey="ddlPaymentMethodResource1">
                                                <asp:ListItem Selected="True" Value="0" meta:resourcekey="ListItemResource5">Complete paying</asp:ListItem>
                                                <asp:ListItem Value="1" meta:resourcekey="ListItemResource6">Fix hire-purchase</asp:ListItem>
                                                <asp:ListItem Value="2" meta:resourcekey="ListItemResource7">UnFix hire-purchase</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 25%">
                                            <asp:Literal ID="Literal18aa" runat="server" Text="Plate No:" meta:resourcekey="Literal18Resource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPlateNo" runat="server" MaxLength="10" Style="text-transform: uppercase"
                                                meta:resourcekey="txtPlateNoResource1"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            <asp:Literal ID="Literal13a" runat="server" Text="Selling type:" meta:resourcekey="Literal13Resource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSellingType" runat="server" MaxLength="255" meta:resourcekey="txtSellingTypeResource1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 25%">
                                            <%--<asp:Image ID="Image4" runat="server" SkinID="RequireField" meta:resourcekey="Image2Resource1" />--%>
                                            <asp:Literal ID="liRecDate" runat="server" Text="Receive Money Date:" meta:resourcekey="liRecDateResource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRecCDate" runat="server" meta:resourcekey="txtRecCDateResource1"></asp:TextBox>
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRecCDate" 
                                                            ErrorMessage='Input &quot;Receive Money Date&quot; can not be blank!'
                                                            SetFocusOnError="True" ValidationGroup="CheckInsertOrUpdate" meta:resourcekey="RequiredFieldValidator2Resource1" Text="*">
                                                            </asp:RequiredFieldValidator>--%>
                                                        <asp:RangeValidator ID="rvRecDate" runat="server" ControlToValidate="txtRecCDate"
                                                            ErrorMessage='Dữ liệu "Ngày cần thu" không đúng với định dạng ngày!' meta:resourcekey="rfvRecDate"
                                                            SetFocusOnError="True" Type="Date" ValidationGroup="CheckInsertOrUpdate">*</asp:RangeValidator>
                                                        <%--<asp:RequiredFieldValidator
                                                            ID="rfvRecCDate" runat="server" ControlToValidate="txtRecCDate"
                                                            ErrorMessage='Input &quot;Receive Money Date&quot; can not be blank!' SetFocusOnError="True"
                                                            ValidationGroup="InsertOrUpdate" meta:resourcekey="rfvRecCDateResource1">*</asp:RequiredFieldValidator>--%>
                                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99/99/9999"
                                                            MaskType="Date" TargetControlID="txtRecCDate" BehaviorID="MaskedEditExtender2"
                                                            Enabled="True">
                                                        </ajaxToolkit:MaskedEditExtender>
                                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="ImageButton5"
                                                            TargetControlID="txtRecCDate" BehaviorID="CalendarExtender2" Enabled="True">
                                                        </ajaxToolkit:CalendarExtender>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="ImageButton5" runat="server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                                            meta:resourcekey="ImageButton5Resource1" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="left">
                                            <asp:Literal ID="Literal17" runat="server" Text="Taking plate No date:" meta:resourcekey="Literal17Resource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTakPlateNoDate" runat="server" meta:resourcekey="txtTakPlateNoDateResource1"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:RangeValidator ID="rvTakingPlateDate" runat="server" ControlToValidate="txtTakPlateNoDate"
                                                            ErrorMessage='Dữ liệu "Ngày lấy biển" không đúng với định dạng ngày!' meta:resourcekey="rvTakingPlateDate"
                                                            SetFocusOnError="True" Type="Date" ValidationGroup="CheckInsertOrUpdate">*</asp:RangeValidator>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="ImageButton6" runat="server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                                            meta:resourcekey="ImageButton6Resource1" /><a href="#"></a>
                                                    </td>
                                                </tr>
                                            </table>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender6" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtTakPlateNoDate" BehaviorID="MaskedEditExtender6"
                                                Enabled="True">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender6" runat="server" PopupButtonID="ImageButton6"
                                                TargetControlID="txtTakPlateNoDate" BehaviorID="CalendarExtender6" Enabled="True">
                                            </ajaxToolkit:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 25%">
                                            <asp:Literal ID="Literal15" runat="server" Text="Description:" meta:resourcekey="Literal15Resource1"></asp:Literal>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtComment" runat="server" Width="90%" MaxLength="255" meta:resourcekey="txtCommentResource1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 25%">
                                            <asp:Image ID="Image3" runat="server" SkinID="RequireField" meta:resourcekey="Image3Resource1" />&nbsp;<asp:Literal
                                                ID="Literal16" runat="server" Text="Customer:" meta:resourcekey="Literal16Resource1"></asp:Literal>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="lbCustomerFullName" runat="server" Width="200" CssClass="readOnlyInputField"
                                                ReadOnly="True" meta:resourcekey="lbCustomerFullNameResource1"></asp:TextBox>
                                            &nbsp;
                                            <asp:Button OnClick="GetCustomer_OnClick" ID="GetCustomerID" Style="padding: 2 2 2 2;
                                                margin: 2 2 2 2; height: 20px; width: 20px" runat="server" Text="..." Enabled="False"
                                                meta:resourcekey="GetCustomerIDResource1" />
                                            &nbsp;
                                            <asp:HyperLink meta:resourcekey="_lnkFindCustResource1" id='_lnkFindCust' runat="server" CssClass="thickbox" NavigateUrl="#">Select customer</asp:HyperLink>
                                            <asp:Panel ID="phCusIdentifyNum" CssClass="hidden" runat="server">
                                                <asp:RadioButton ID="rbtnCCustomer" runat="server" GroupName="GrSaleObj" Text="Customer's identify number:"
                                                    Checked="True" meta:resourcekey="rbtnCCustomerResource1" />
                                                <asp:TextBox ID="txtCustomerIdentifyNumber" runat="server" MaxLength="10" meta:resourcekey="txtCustomerIdentifyNumberResource1"></asp:TextBox>
                                                <asp:RequiredFieldValidator Enabled="false"
                                                    ID="rfvCustomerName" runat="server" ControlToValidate="txtCustomerIdentifyNumber"
                                                    ErrorMessage='Input &quot;Customer name&quot; can not be blank!' SetFocusOnError="True"
                                                    ValidationGroup="InsertOrUpdate" meta:resourcekey="rfvCustomerNameResource1">*</asp:RequiredFieldValidator>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:PlaceHolder>
                            <!-- End: Main input -->
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:PlaceHolder ID="FixedHP" runat="server" Visible="False">
                                <!-- Begin Fixed Hire-Purchase -->
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td align="left" colspan="4">
                                            <h5>
                                                <asp:Literal ID="liINSERTFixedHPurchase" runat="server" Text="Fixed hire-purchase"
                                                    meta:resourcekey="liINSERTFixedHPurchaseResource1"></asp:Literal></h5>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 25%">
                                            <asp:Image ID="Image5" runat="server" SkinID="RequireField" meta:resourcekey="Image2Resource1" />
                                            <asp:Literal ID="Literal6" runat="server" Text="Hire-purchase times:" meta:resourcekey="Literal6Resource1"></asp:Literal>
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="txtFHPTimes" runat="server" MaxLength="1" meta:resourcekey="txtFHPTimesResource1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFHPTimes" runat="server" ControlToValidate="txtFHPTimes"
                                                ErrorMessage="Times of hire-purchase can not be blank!" ValidationGroup="CheckInsertOrUpdate"
                                                meta:resourcekey="rfvFHPTimesResource1">*</asp:RequiredFieldValidator><asp:RangeValidator
                                                    ID="rvFHPPaindMoneyDate" runat="server" ControlToValidate="txtFHPTimes" ErrorMessage="Number of fixed installment times must be range 2-5!"
                                                    MaximumValue="5" meta:resourcekey="rvFHPTimes" MinimumValue="2" SetFocusOnError="True"
                                                    Type="Integer" ValidationGroup="CheckInsertOrUpdate">*</asp:RangeValidator><ajaxToolkit:MaskedEditExtender
                                                        ID="meDaysIns" runat="server" BehaviorID="meDaysIns" CultureAMPMPlaceholder="AM;PM"
                                                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                                                        CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                                                        Enabled="True" Mask="9" MaskType="Number" PromptCharacter="" TargetControlID="txtFHPTimes">
                                                    </ajaxToolkit:MaskedEditExtender>
                                        </td>
                                        <td align="left" style="width: 25%">
                                            <asp:Literal ID="Literal3" runat="server" Text="Money of pay in instalment:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="lbMoneyOfTimes" runat="server" CssClass="readOnlyInputField" ReadOnly="True"
                                                meta:resourcekey="lbMoneyOfTimesResource1"></asp:TextBox>
                                            (VND)
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 25%">
                                            <asp:Image ID="Image6" runat="server" SkinID="RequireField" meta:resourcekey="Image2Resource1" />
                                            <asp:Literal ID="Literal4" runat="server" Text="First liquidate:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="txtFHPFirstMoney" runat="server" MaxLength="15" meta:resourcekey="txtFHPFirstMoneyResource1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFHPFirstliquidate" runat="server" ControlToValidate="txtFHPFirstMoney"
                                                ErrorMessage="First liquidate can not be blank!" ValidationGroup="CheckInsertOrUpdate"
                                                meta:resourcekey="rfvFHPFirstliquidateResource1">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                    ID="FirstLiquidateFormat" runat="server" ControlToValidate="txtFHPFirstMoney"
                                                    ErrorMessage="Khoản trả kỳ đầu phải được gõ bằng số lớn hơn 0!" meta:resourcekey="FirstLiquidateFormat"
                                                    Text="*" ValidationExpression="\s*[1-9]\d*\s*" ValidationGroup="CheckInsertOrUpdate"></asp:RegularExpressionValidator>(VND)
                                        </td>
                                        <td align="left" style="width: 25%">
                                            <asp:Literal ID="Literal5" runat="server" Text="Money of all hire-purchase:" meta:resourcekey="Literal5Resource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFHPAllMoney" runat="server" CssClass="readOnlyInputField" ReadOnly="True"
                                                meta:resourcekey="txtFHPAllMoneyResource1"></asp:TextBox>
                                            (VND)
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 25%">
                                            <asp:Image ID="Image7" runat="server" SkinID="RequireField" meta:resourcekey="Image2Resource1" />
                                            <asp:Literal ID="Literal2" runat="server" Text="Number of fixed hire-purchase's:"
                                                meta:resourcekey="Literal2Resource1"></asp:Literal>
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="txtFHPPaidMoneyDate" runat="server" MaxLength="5" meta:resourcekey="txtFHPPaidMoneyDateResource1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFHPPaidMoneyDate" runat="server" ControlToValidate="txtFHPPaidMoneyDate"
                                                ErrorMessage="Number of fixed hire-purchase's can not be blank!" ValidationGroup="CheckInsertOrUpdate"
                                                meta:resourcekey="rfvFHPPaidMoneyDateResource1">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                    ID="FHPDaysRangeFormat" runat="server" ControlToValidate="txtFHPPaidMoneyDate"
                                                    ErrorMessage="Số ngày mỗi kỳ phải được gõ bằng số hoặc lớn hơn 0!" meta:resourcekey="FHPDaysRangeFormat"
                                                    Text="*" ValidationExpression="\s*[1-9]\d*\s*" ValidationGroup="CheckInsertOrUpdate"></asp:RegularExpressionValidator><asp:Literal
                                                        ID="Literal1" runat="server" Text="(Days)" meta:resourcekey="Literal1Resource1"></asp:Literal>
                                        </td>
                                        <td align="left" style="width: 25%">
                                            <asp:Literal ID="liFHPLastHPdate" runat="server" Text="Last hire-purchase date:"
                                                meta:resourcekey="liFHPLastHPdateResource1"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="FHPPaidDateLast" runat="server" CssClass="readOnlyInputField" ReadOnly="True"
                                                meta:resourcekey="FHPPaidDateLastResource1"></asp:TextBox>
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
                                <!-- Begin: Unfixed hire-purchase -->
                                <table style="width: 100%">
                                    <tr>
                                        <td align="left" colspan="5">
                                            <h5>
                                                <asp:Literal ID="Literal7" runat="server" Text="Unfixed hire-purchase" meta:resourcekey="Literal7Resource1"></asp:Literal></h5>
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
                                            <ajaxToolkit:MaskedEditExtender ID="MEUHP1" runat="server" Mask="99/99/9999" MaskType="Date"
                                                TargetControlID="txtIntentDatePay1" BehaviorID="MEUHP1" CultureAMPMPlaceholder="AM;PM"
                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                                                CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                                                Enabled="True">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:CalendarExtender ID="CEUHP1" runat="server" PopupButtonID="imgIntend1"
                                                TargetControlID="txtIntentDatePay1" BehaviorID="CEUHP1" Enabled="True">
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
                                            <ajaxToolkit:MaskedEditExtender ID="MEUHP2" runat="server" Mask="99/99/9999" MaskType="Date"
                                                TargetControlID="txtIntentDatePay2" BehaviorID="MEUHP2" CultureAMPMPlaceholder="AM;PM"
                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                                                CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                                                Enabled="True">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:CalendarExtender ID="CEUHP2" runat="server" PopupButtonID="imgIntend2"
                                                TargetControlID="txtIntentDatePay2" BehaviorID="CEUHP2" Enabled="True">
                                            </ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                    </td>
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
                                            <ajaxToolkit:MaskedEditExtender ID="MEUHP3" runat="server" Mask="99/99/9999" MaskType="Date"
                                                TargetControlID="txtIntentDatePay3" BehaviorID="MEUHP3" CultureAMPMPlaceholder="AM;PM"
                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                                                CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                                                Enabled="True">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:CalendarExtender ID="CEUHP3" runat="server" PopupButtonID="imgIntend3"
                                                TargetControlID="txtIntentDatePay3" BehaviorID="CEUHP3" Enabled="True">
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
                                            <ajaxToolkit:MaskedEditExtender ID="MEUHP4" runat="server" Mask="99/99/9999" MaskType="Date"
                                                TargetControlID="txtIntentDatePay4" BehaviorID="MEUHP4" CultureAMPMPlaceholder="AM;PM"
                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                                                CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                                                Enabled="True">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:CalendarExtender ID="CEUHP4" runat="server" PopupButtonID="imgIntend4"
                                                TargetControlID="txtIntentDatePay4" BehaviorID="CEUHP4" Enabled="True">
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
                                            <ajaxToolkit:MaskedEditExtender ID="MEUHP5" runat="server" Mask="99/99/9999" MaskType="Date"
                                                TargetControlID="txtIntentDatePay5" BehaviorID="MEUHP5" CultureAMPMPlaceholder="AM;PM"
                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                                                CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                                                Enabled="True">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:CalendarExtender ID="CEUHP5" runat="server" PopupButtonID="imgIntend5"
                                                TargetControlID="txtIntentDatePay5" BehaviorID="CEUHP5" Enabled="True">
                                            </ajaxToolkit:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="5" style="text-align: left">
                                            <asp:Literal ID="Literal19" runat="server" Text="Số tiền dự t&#237;nh sẽ nộp: (VND)"
                                                meta:resourcekey="Literal19Resource1"></asp:Literal>
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
                                <!-- End: Unfixed hire-purchase -->
                            </asp:PlaceHolder>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:PlaceHolder ID="plActionAccessData" runat="server" Visible="False">
                                <asp:Button ID="btnCheckValid" runat="server" Visible="False" Text="Check" OnClick="btnCheckValid_Click"
                                    ValidationGroup="CheckInsertOrUpdate" meta:resourcekey="btnCheckValidResource1" />
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="InsertOrUpdate"
                                    meta:resourcekey="btnSaveResource1" />&nbsp;
                                <asp:Button ID="btnClose" runat="server" Text="Exit" OnClick="btnClose_Click" meta:resourcekey="btnCloseResource1" />
                            </asp:PlaceHolder>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:Literal ID="Literal18" runat="server" meta:resourcekey="Literal18Resource1xxx"
                Text="<H3> Select item</H3>"></asp:Literal><asp:ValidationSummary ID="ValidationSummary4"
                    runat="server" meta:resourcekey="ValidationSummary2Resource1" ValidationGroup="Select" />
            <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                <tr>
                    <td>
                        <div class="grid">
                            <vdms:PageGridView ID="gvSelectItem" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CssClass="GridView" DataKeyNames="ItemInstanceId" DataSourceID="odsSelectItem" meta:resourcekey="gvSelectItemResource1"
                                OnPreRender="gvSelectxxx_PreRender" OnRowCommand="gvSelectxxx_page" OnSelectedIndexChanging="gvSelectItem_SelectedIndexChanging"
                                PageSize="15" Width="492px">
                                <Columns>
                                    <asp:TemplateField HeaderText="Engine number" meta:resourcekey="TemplateFieldResource9"
                                        SortExpression="Enginenumber">
                                        <EditItemTemplate>
                                            <asp:Literal ID="Literal35" runat="server" Text='<%# Eval("EngineNumber") %>'></asp:Literal>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Literal ID="litSelectedSoldItem" runat="server" Text='<%# Eval("EngineNumber") %>'></asp:Literal><asp:HiddenField
                                                ID="hdMadedate" runat="server" Value='<%# Eval("MadeDate") %>' />
                                            <asp:HiddenField ID="hdTiptop" runat="server" Value='<%# Eval("ItemInstanceId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Model" meta:resourcekey="TemplateFieldResource10"
                                        SortExpression="Itemtype">
                                        <EditItemTemplate>
                                            <asp:Literal ID="litSelectedSoldItemModel" runat="server" Text='<%# Eval("ItemType") %>'></asp:Literal>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Literal ID="litSelectedSoldItemModel" runat="server" Text='<%# Eval("ItemType") %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Color" meta:resourcekey="TemplateFieldResource11"
                                        SortExpression="Color">
                                        <ItemTemplate>
                                            <asp:Literal ID="litSelectedSoldItemColor" runat="server" Text='<%# Eval("Color") %>'></asp:Literal>
                                            <asp:HiddenField ID="hdItemColorCode" runat="server" Value='<%# Eval("Item.ColorCode") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Imported date" meta:resourcekey="TemplateFieldResource12"
                                        SortExpression="Importeddate" Visible="False">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox4" runat="server" meta:resourcekey="TextBox4Resource1" Text='<%# Bind("ImportedDate") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" meta:resourcekey="Label4Resource1" Text='<%# Bind("ImportedDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ButtonType="Button" meta:resourcekey="CommandFieldResource1" ShowSelectButton="True">
                                        <ItemStyle Width="10px" />
                                    </asp:CommandField>
                                </Columns>
                            </vdms:PageGridView>
                        </div>
                        <asp:ObjectDataSource ID="odsSelectItem" runat="server" EnablePaging="True" SelectCountMethod="SelectCount"
                            SelectMethod="SelectForSale" TypeName="VDMS.I.ObjectDataSource.ItemInstanceDataSource">
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
                        &nbsp;<asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancelResource1"
                            OnClick="btnCancel_Click" Text="Cancel" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
