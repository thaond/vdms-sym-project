<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CusInfInput.aspx.cs" Inherits="Vehicle_Sale_CusInfInput"
    Title="Edit customer information - Add new" Culture="auto" meta:resourcekey="PageResource2"
    UICulture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Pragma" content="no-cache" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

    <script language="javascript" type="text/javascript">
        //var returnrefCustomerAction = "Reset";
        function close_dialog() {
            window.close();
        }
        function close_edit_dialog() {
            var currentCusID = document.getElementById('txtCustomerID');
            var currentCustomerName = document.getElementById('txtFullName');
            if (currentCusID.value.trim() != "" && currentCustomerName.disable == true) {
                var objWindow = dialogArguments[0]; //Window object 
                var refCusFullName = dialogArguments[17];
                var returnrefCusFullName = objWindow.document.getElementById(refCusFullName);
                returnrefCusFullName.value = currentCustomerName.value;
            }
            window.close();
        }
        function send_back_data() {
            //Get dialog arguments
            var objWindow = dialogArguments[0]; //Window object
            var refCustomerID = dialogArguments[1]; //CustomerID
            var refCustomerIdentifyNumber = dialogArguments[2]; //CustomerName
            var refCustomerAction = dialogArguments[3]; //CustomerAction
            var refCusSex = dialogArguments[4]; //CustomerSex
            var refCusBirthdate = dialogArguments[5]; //CustomerBirthdate
            var refCusAddress = dialogArguments[6]; //CustomerAddress
            var refCusProvince = dialogArguments[7]; //CustomerAction
            var refCusDistrict = dialogArguments[8]; //CustomerAction
            var refCusJobType = dialogArguments[9]; //CustomerAction
            var refCusEmail = dialogArguments[10]; //CustomerAction
            var refCusPhone = dialogArguments[11]; //CustomerAction
            var refCusMobile = dialogArguments[12]; //CustomerAction
            var refCusSetType = dialogArguments[13]; //CustomerAction
            var refCusType = dialogArguments[14]; //CustomerAction
            var refCusDesc = dialogArguments[15]; //CustomerAction
            var refCusPrecinct = dialogArguments[16]; //CustomerAction
            var refCusFullName = dialogArguments[17]; var refHiddenCusFullName = dialogArguments[18];

            var currCustomerIDnum = document.getElementById('<%= txtCustomerID.ClientID %>');
            //			if (currCustomerIDnum.value == "") {
            //				//var objCusIdNumEmpty = document.getElementById('CusIdNumEmpty');
            //				//alert(objCusIdNumEmpty.value);
            //				alert('<%= CusIdNumEmpty %>');
            //				return false;
            //			}

            var currentCustomerName = document.getElementById('<%= txtFullName.ClientID %>');
            if (currentCustomerName.value == "") {
                //	        var objCusFullnameEmpty = document.getElementById('CusFullnameEmpty');
                //	        alert(objCusFullnameEmpty.value);
                alert('<%= CusFullnameEmpty %>');
                return false;
            }

            var currPhone = document.getElementById('<%= txtCPhone.ClientID %>');
            if (currPhone.value != "") {
                if (!IsNumeric(currPhone.value.trim())) {
                    //var objCusPhoneNumberic = document.getElementById('CusPhoneNumberic');
                    //alert(objCusPhoneNumberic.value);
                    alert('<%= CusPhoneNumberFormatInvalid %>');
                    return false;
                }
            }

            var currMobile = document.getElementById('<%= txtCMobile.ClientID %>');
            if (!IsNumeric(currMobile.value.trim())) {
                //var objCusMobileNumberic = document.getElementById('CusMobileNumberic');
                //alert(objCusMobileNumberic.value);
                alert('<%= CusMobileNumbeFormatInvalid %>');
                return false;
            }

            if ((currMobile.value == "") && (currPhone.value == "")) {
                alert('<%= NeedOnePhoneNumber %>');
                return false;
            }

            var currEmail = document.getElementById('<%= txtCEmail.ClientID %>');
            //	    if (currEmail.value == ""){
            //	        var objEmailEmpty = document.getElementById('EmailEmpty');
            //	        alert(objEmailEmpty.value);
            //	        alert('<%= EmailEmpty %>');
            //	        return false;
            //	    }

            if (currEmail.value != "") {
                if (emailCheck(currEmail.value) == false) {
                    return false;
                }
            }

            //alert(returnrefCustomerIdentifyNumber);
            var returnrefCustomerIdentifyNumber = objWindow.document.getElementById(refCustomerIdentifyNumber);
            returnrefCustomerIdentifyNumber.value = currCustomerIDnum.value;

            var returnrefrefHiddenCusFullName = objWindow.document.getElementById(refHiddenCusFullName);
            var returnrefCusFullName = objWindow.document.getElementById(refCusFullName);
            returnrefrefHiddenCusFullName.value = currentCustomerName.value;
            if (returnrefCusFullName != null)
                returnrefCusFullName.value = currentCustomerName.value;

            var returnrefCusSex = objWindow.document.getElementById(refCusSex);
            var currObj = document.getElementById('ddlSex'); returnrefCusSex.value = currObj.value;

            var returnCusBirthdate = objWindow.document.getElementById(refCusBirthdate);
            var currObj = document.getElementById('txtBirthDate'); returnCusBirthdate.value = currObj.value;
            //alert(currObj.value);

            var returnAddress = objWindow.document.getElementById(refCusAddress);
            var currObj = document.getElementById('txtAddress'); returnAddress.value = currObj.value;
            if (currObj.value == "" && '<%= RequireAddress.ToString().ToLower()%>' == 'true') {
                alert('<%= AddressEmpty %>');
                return false;
            }

            var returnrefCusProvince = objWindow.document.getElementById(refCusProvince);
            var currObj = document.getElementById('ddlProvince'); returnrefCusProvince.value = currObj.value;

            var returnDistrict = objWindow.document.getElementById(refCusDistrict);
            var currObj = document.getElementById('txtDistrict'); returnDistrict.value = currObj.value;

            var returnrefCusJobType = objWindow.document.getElementById(refCusJobType);
            var currObj = document.getElementsByName('tblCus_JobType');
            for (i = 1; i < currObj.length; i++) {
                if (currObj[i].checked) {
                    returnrefCusJobType.value = currObj[i].value;
                    //alert(currObj[i].value);
                }
            }

            var returnPhone = objWindow.document.getElementById(refCusPhone);
            returnPhone.value = currPhone.value;

            var returnMobile = objWindow.document.getElementById(refCusMobile);
            returnMobile.value = currMobile.value;

            var returnEmail = objWindow.document.getElementById(refCusEmail);
            returnEmail.value = currEmail.value;

            var returnrefCusSetType = objWindow.document.getElementById(refCusSetType);
            var currObj = document.getElementById('ddlCus_SetType'); returnrefCusSetType.value = currObj.value;

            var returnrefCusType = objWindow.document.getElementById(refCusType);
            var currObj = document.getElementById('ddlCusType'); returnrefCusType.value = currObj.value;

            var returnDesc = objWindow.document.getElementById(refCusDesc);
            var currObj = document.getElementById('txtCus_Desc'); returnDesc.value = currObj.value;

            var returnPrecinct = objWindow.document.getElementById(refCusPrecinct);
            var currObj = document.getElementById('txtPrecinct'); returnPrecinct.value = currObj.value;

            returnrefCustomerAction = objWindow.document.getElementById(refCustomerAction);
            //alert(returnrefCustomerAction.value);
            returnrefCustomerAction.value = "Save";

            close_dialog();
        }
        //<!-- Begin
        function IsNumeric(sText) {
            var ValidChars = "0123456789";
            var IsNumber = true;
            var Char;

            for (i = 0; i < sText.length && IsNumber == true; i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    IsNumber = false;
                }
            }
            return IsNumber;
        }
        function emailCheck(emailStr) {
            var emailPat = /^(.+)@(.+)$/
            var specialChars = "\\(\\)<>@,;:\\\\\\\"\\.\\[\\]"
            var validChars = "\[^\\s" + specialChars + "\]"
            var quotedUser = "(\"[^\"]*\")"
            var ipDomainPat = /^\[(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})\]$/
            var atom = validChars + '+'
            var word = "(" + atom + "|" + quotedUser + ")"
            var userPat = new RegExp("^" + word + "(\\." + word + ")*$")
            var domainPat = new RegExp("^" + atom + "(\\." + atom + ")*$")

            var matchArray = emailStr.match(emailPat)
            if (matchArray == null) {
                //alert("Email address seems incorrect (check @ and .'s)")
                //alert("Địa chỉ email sai! Hãy nhập đúng định dạng (ví dụ: nguoidung@viettel.local)")
                alert('<%= EmailFormatErr %>');
                return false
            }
            var user = matchArray[1]
            var domain = matchArray[2]

            if (user.match(userPat) == null) {
                // user is not valid
                //alert("Ký tự trong tên của email không đúng!")
                alert('<%= EmailFormatErr %>');
                return false
            }

            var IPArray = domain.match(ipDomainPat)
            if (IPArray != null) {
                // this is an IP address
                for (var i = 1; i <= 4; i++) {
                    if (IPArray[i] > 255) {
                        //alert("Địa chỉ ip mail không đúng!")
                        alert('<%= EmailFormatErr %>');
                        return false
                    }
                }
                //return true
            }

            // Domain is symbolic name
            var domainArray = domain.match(domainPat)
            if (domainArray == null) {
                //alert("Domain name email không đúng!")
                alert('<%= EmailFormatErr %>');
                return false
            }
            var atomPat = new RegExp(atom, "g")
            var domArr = domain.match(atomPat)
            var len = domArr.length
            if (domArr[domArr.length - 1].length < 2 ||
			domArr[domArr.length - 1].length > 5) {
                //alert("Domain name email không đúng!!")
                alert('<%= EmailFormatErr %>');
                return false
            }

            if (len < 2) {
                //var errStr="Địa chỉ mail thiếu tên Domain!"
                //alert(errStr)
                alert('<%= EmailFormatErr %>');
                return false
            }
            //return true;
        }
        //  End -->
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <!-- Begin: Add New Customer Infomation  
        <link href="imgs/content_style.css" rel="stylesheet" type="text/css"> -->
        <asp:Panel ID="Panel1" runat="server" Width="580px" meta:resourcekey="Panel1Resource1">
            <table width="100%" border="0" align="center" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <div runat="server" id="errMsg">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="page_title">
                        <asp:Literal ID="liTitlePage" runat="server" Text="Sửa đổi dữ liệu kh&#225;ch h&#224;ng-Th&#234;m mới"
                            meta:resourcekey="liTitlePageResource1"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:ValidationSummary ID="vsAddNewCustomer" runat="server" DisplayMode="List" ValidationGroup="AddNewCustomer"
                            Width="100%" meta:resourcekey="vsAddNewCustomerResource1" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblErr" runat="server" ForeColor="Red" meta:resourcekey="lblErrResource1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0" cellspacing="4" cellpadding="0">
                            <tr>
                                <td align="left" class="field_name" style="width: 20%" valign="middle" nowrap="nowrap">
                                    <asp:Image ID="Image3" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image3Resource1" />
                                    <asp:Literal ID="liCustomerID" runat="server" Text="Số CMND/M&#227; số c&#244;ng ty:"
                                        meta:resourcekey="liCustomerIDResource1"></asp:Literal>&nbsp;
                                </td>
                                <td style="width: 30%" valign="middle">
                                    <asp:TextBox ID="txtCustomerID" runat="server" CssClass="lblClass" MaxLength="10"
                                        meta:resourcekey="txtCustomerIDResource1"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqvCustomerID" runat="server" ControlToValidate="txtCustomerID"
                                        Enabled="false" ErrorMessage="Số CMND/M&#227; số c&#244;ng ty kh&#244;ng được để trống!"
                                        ValidationGroup="AddNewCustomer" CssClass="lblClass" meta:resourcekey="rqvCustomerIDResource1"
                                        Text="*"></asp:RequiredFieldValidator>
                                </td>
                                <td align="left" style="width: 20%" valign="middle" nowrap="nowrap">
                                    <asp:Image ID="Image4" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image4Resource1" />
                                    <asp:Literal ID="liCus_Type" runat="server" Text="Ph&#226;n loại kh&#225;ch h&#224;ng :"
                                        meta:resourcekey="liCus_TypeResource1"></asp:Literal>
                                </td>
                                <td valign="middle">
                                    <asp:DropDownList ID="ddlCus_SetType" runat="server" CssClass="lblClass" meta:resourcekey="ddlCus_SetTypeResource1">
                                        <asp:ListItem meta:resourcekey="ListItemResource1" Text="Th&#244;ng thường" Value="0"></asp:ListItem>
                                        <asp:ListItem meta:resourcekey="ListItemResource2" Text="Quan trọng" Value="1"></asp:ListItem>
                                        <asp:ListItem meta:resourcekey="ListItemResource3" Text="Đặt biệt quan trọng" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="item1">
                                <td align="left" style="width: 20%" valign="middle">
                                    <asp:Image ID="Image1" runat="server" SkinID="RequireField" meta:resourcekey="Image1Resource1" />
                                    <asp:Literal ID="liFullName" runat="server" Text="Họ t&#234;n:" meta:resourcekey="liFullNameResource1"></asp:Literal>
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtFullName" runat="server" CssClass="lblClass" MaxLength="50" meta:resourcekey="txtFullNameResource1"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFullName"
                                        ErrorMessage="Họ v&#224; t&#234;n kh&#225;ch h&#224;ng kh&#244;ng được bỏ trống!"
                                        ValidationGroup="AddNewCustomer" CssClass="lblClass" meta:resourcekey="RequiredFieldValidator2Resource1"
                                        Text="*"></asp:RequiredFieldValidator>
                                </td>
                                <td align="left" style="width: 20%" valign="middle">
                                    <asp:Image ID="Image5" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image5Resource1" />
                                    <asp:Literal ID="liCusType" runat="server" Text="Loại kh&#225;ch h&#224;ng :" meta:resourcekey="liCusTypeResource1"></asp:Literal>
                                </td>
                                <td valign="middle">
                                    <asp:DropDownList ID="ddlCusType" runat="server" CssClass="lblClass" meta:resourcekey="ddlCusTypeResource1"
                                        Enabled="False">
                                        <asp:ListItem meta:resourcekey="ListItemResource4" Text="Kh&#225;ch h&#224;ng mua xe"
                                            Value="0"></asp:ListItem>
                                        <asp:ListItem meta:resourcekey="ListItemResource5" Text="Kh&#225;ch h&#224;ng sửa xe"
                                            Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="Tr1">
                                <td align="left" style="width: 20%" valign="middle">
                                    <asp:Image ID="Image2" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image2Resource1" />
                                    <asp:Literal ID="liBirthDate" runat="server" Text="Ng&#224;y sinh:" meta:resourcekey="liBirthDateResource1"></asp:Literal>
                                </td>
                                <td valign="middle">
                                    <table border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtBirthDate" runat="server" meta:resourcekey="txtBirthDateResource1"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="ImageButton1" runat="server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                                    CssClass="lblClass" meta:resourcekey="ImageButton1Resource1" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtBirthDate"
                                        ErrorMessage="Kh&#244;ng c&#243; ng&#224;y n&#224;y!" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"
                                        ValidationGroup="AddNewCustomer" meta:resourcekey="RegularExpressionValidator1Resource1">*</asp:RegularExpressionValidator>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" CultureName="vi-VN"
                                        Mask="99/99/9999" MaskType="Date" TargetControlID="txtBirthDate" BehaviorID="MaskedEditExtender2"
                                        CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY"
                                        CultureDatePlaceholder="/" CultureDecimalPlaceholder="." CultureThousandsPlaceholder=","
                                        CultureTimePlaceholder=":" Enabled="True">
                                    </ajaxToolkit:MaskedEditExtender>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy"
                                        PopupButtonID="ImageButton1" TargetControlID="txtBirthDate" BehaviorID="CalendarExtender3"
                                        Enabled="True">
                                    </ajaxToolkit:CalendarExtender>
                                </td>
                                <td align="left" style="width: 20%" valign="middle">
                                    <asp:Image ID="imgSexNon" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image6Resource1" />
                                    <asp:Image ID="imgSex" runat="server" SkinID="RequireField" meta:resourcekey="Image6Resource1" />
                                    <asp:Literal ID="liSex" runat="server" Text="Giới t&#237;nh:" meta:resourcekey="liSexResource1"></asp:Literal>
                                </td>
                                <td valign="middle">
                                    <asp:DropDownList ID="ddlSex" runat="server" CssClass="lblClass" meta:resourcekey="ddlSexResource1">
                                        <asp:ListItem Value="1" meta:resourcekey="ListItemResource6" Text="M"></asp:ListItem>
                                        <asp:ListItem Value="0" meta:resourcekey="ListItemResource7" Text="F"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="item3"">
                                <td align="left" style="width: 20%" valign="middle">
                                    <asp:Image ID="Image8" runat="server" SkinID="RequireField" meta:resourcekey="Image8Resource1" />
                                    <asp:Literal ID="liCPhone" runat="server" Text="Số điện thoại:" meta:resourcekey="liCPhoneResource1"></asp:Literal>
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtCPhone" runat="server" CssClass="lblClass" MaxLength="20" meta:resourcekey="txtCPhoneResource1"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtCPhone"
                                        ErrorMessage="Số điện thoại của khách hàng phải được gõ bằng số!" meta:resourcekey="RegularExpressionValidator2Resource1"
                                        Text="*" ValidationExpression="\s*[0-9]\d*\s*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                                </td>
                                <td align="left" style="width: 20%" valign="middle">
                                    <asp:Image ID="Image7" runat="server" SkinID="RequireField" meta:resourcekey="Image7Resource1" />
                                    <asp:Literal ID="liCMobile" runat="server" Text="Số ĐTDĐ:" meta:resourcekey="liCMobileResource1"></asp:Literal>
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtCMobile" runat="server" CssClass="lblClass" MaxLength="20" meta:resourcekey="txtCMobileResource1"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtCMobile"
                                        ErrorMessage="Số điện thoại di động của khách hàng phải được gõ bằng số!" meta:resourcekey="RegularExpressionValidator1Resource1"
                                        Text="*" ValidationExpression="\s*[0-9]\d*\s*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                                    <%--<asp:RequiredFieldValidator ID="rfvCus_Mobile" runat="server" ControlToValidate="txtCMobile"
                                            ErrorMessage="Số điện thoại của kh&#225;ch h&#224;ng kh&#244;ng được bỏ trống!"
                                            ValidationGroup="AddNewCustomer" CssClass="lblClass" meta:resourcekey="rfvCus_MobileResource1"
                                            Text="*"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <!-- None End Here-->
                            <tr id="item4">
                                <td align="Left" style="width: 20%" valign="middle">
                                    <asp:Image ID="Image9" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image9Resource1" />
                                    <asp:Literal ID="Literal1" runat="server" Text="Địa chỉ hộ khẩu:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                                </td>
                                <td align="center" valign="middle">
                                </td>
                                <td align="left" style="width: 20%" valign="middle">
                                </td>
                                <td align="center" valign="middle">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 20%" valign="middle">
                                    <asp:Image ID="imgAddressNon" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image10Resource1" />
                                    <asp:Image ID="imgAddress" runat="server" SkinID="RequireField" meta:resourcekey="Image10Resource1" />
                                    <asp:Literal ID="liHouseNo" runat="server" Text="Số nh&#224;, phố:" meta:resourcekey="liHouseNoResource1"></asp:Literal>
                                </td>
                                <td align="left" valign="middle">
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="lblClass" MaxLength="256" meta:resourcekey="txtAddressResource1"></asp:TextBox>
                                </td>
                                <td align="left" style="width: 20%" valign="middle">
                                    <asp:Image ID="Image18" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image18Resource1" />
                                    <asp:Literal ID="liPrecinct" runat="server" Text="Phường, x&#227;:" meta:resourcekey="liPrecinctResource1"></asp:Literal>
                                </td>
                                <td align="left" valign="middle">
                                    <asp:TextBox ID="txtPrecinct" runat="server" CssClass="lblClass" MaxLength="256"
                                        meta:resourcekey="txtPrecinctResource1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="item6">
                                <td align="left" style="width: 20%" valign="middle">
                                    <asp:Image ID="Image11" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image11Resource1" />
                                    <asp:Literal ID="liDistrict" runat="server" Text="Quận, huyện:" meta:resourcekey="liDistrictResource1"></asp:Literal>
                                </td>
                                <td align="left" valign="middle">
                                    <asp:TextBox ID="txtDistrict" runat="server" CssClass="lblClass" MaxLength="256"
                                        meta:resourcekey="txtDistrictResource1"></asp:TextBox>
                                </td>
                                <td align="Left" style="width: 20%" valign="middle">
                                    <asp:Image ID="Image17" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image17Resource1" />
                                    <asp:Literal ID="liCity" runat="server" Text="Tỉnh, th&#224;nh phố:" meta:resourcekey="liCityResource1"></asp:Literal>
                                </td>
                                <td align="left" valign="middle">
                                    <asp:DropDownList ID="ddlProvince" runat="server" CssClass="lblClass" meta:resourcekey="ddlProvinceResource1">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="item8">
                                <td align="left" style="width: 20%">
                                    <asp:Image ID="Image12" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image12Resource1" />
                                    <asp:Literal ID="liCEmail" runat="server" Text="E-MAIL:" meta:resourcekey="liCEmailResource1"></asp:Literal>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtCEmail" runat="server" CssClass="lblClass" MaxLength="256" meta:resourcekey="txtCEmailResource1"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revCus_Email" runat="server" ControlToValidate="txtCEmail"
                                        ErrorMessage="Định dạng mail của kh&#225;ch h&#224;ng chưa ch&#237;nh x&#225;c!"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="AddNewCustomer"
                                        meta:resourcekey="revCus_EmailResource1" Text="*"></asp:RegularExpressionValidator>
                                    <%--<asp:RequiredFieldValidator ID="rfvCus_Email" runat="server" ControlToValidate="txtCEmail"
                                            ErrorMessage="Email của kh&#225;ch h&#224;ng kh&#244;ng được bỏ trống!" ValidationGroup="AddNewCustomer"
                                            meta:resourcekey="rfvCus_EmailResource1" Text="*"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 20%">
                                    <asp:Image ID="Image13" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image13Resource1" />
                                    <asp:Literal ID="liJobType" runat="server" Text="Nghề nghiệp:" meta:resourcekey="liJobTypeResource1"></asp:Literal>
                                </td>
                                <td colspan="3">
                                    <span class="form_word">
                                        <asp:RadioButtonList ID="tblCus_JobType" runat="server" RepeatDirection="Horizontal"
                                            CssClass="lblClass" meta:resourcekey="tblCus_JobTypeResource1">
                                            <asp:ListItem Value="1" meta:resourcekey="ListItemResource8" Text="Học sinh"></asp:ListItem>
                                            <asp:ListItem Value="2" meta:resourcekey="ListItemResource9" Text="C&#244;ng sở"></asp:ListItem>
                                            <asp:ListItem Value="3" meta:resourcekey="ListItemResource10" Text="Tự do"></asp:ListItem>
                                            <asp:ListItem Value="4" meta:resourcekey="ListItemResource11" Text="Qu&#226;n nh&#226;n"></asp:ListItem>
                                            <asp:ListItem Selected="True" Value="5" meta:resourcekey="ListItemResource12" Text="Kh&#225;c"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </span>
                                </td>
                            </tr>
                            <tr valign="middle">
                                <td align="left" style="width: 20%">
                                    <asp:Image ID="Image14" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image14Resource1" />
                                    <asp:Literal ID="liModel" runat="server" Text="Chủng loại xe:" meta:resourcekey="liModelResource1"></asp:Literal>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtModel" runat="server" CssClass="lblClass" meta:resourcekey="txtModelResource1"
                                        ReadOnly="True"></asp:TextBox>
                                </td>
                                <td align="left" style="width: 20%">
                                    <asp:Image ID="Image16" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image16Resource1" />
                                    <asp:Literal ID="liEngineNo" runat="server" Text="Số động cơ:" meta:resourcekey="liEngineNoResource1"></asp:Literal>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtEngineNo" runat="server" CssClass="lblClass" MaxLength="256"
                                        meta:resourcekey="txtEngineNoResource1" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr valign="middle">
                                <td align="left" valign="top" style="width: 20%">
                                    <asp:Image ID="Image15" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image15Resource1" />
                                    <asp:Literal ID="liDescription" runat="server" Text="Ch&#250; th&#237;ch:" meta:resourcekey="liDescriptionResource1"></asp:Literal>
                                </td>
                                <td align="left" colspan="3">
                                    <asp:TextBox ID="txtCus_Desc" runat="server" CssClass="lblClass" Height="70px" TextMode="MultiLine"
                                        Width="100%" MaxLength="1024" meta:resourcekey="txtCus_DescResource1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr valign="middle">
                                <td colspan="4" align="right">
                                    <asp:Button ID="btnSave" runat="server" Text="Lưu" ValidationGroup="AddNewCustomer" CssClass="field_name"
                                        meta:resourcekey="btnSaveResource1" onclick="btnSave_Click1" />
                                    <asp:Button ID="btnExit" runat="server" Text="Tho&#225;t" CssClass="field_name" meta:resourcekey="btnExitResource1"
                                        OnClientClick="close_edit_dialog();return false;" UseSubmitBehavior="False" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Literal ID="liDone" runat="server" Visible="False" meta:resourcekey="liDoneResource1"></asp:Literal><br />
                                    <asp:Button ID="btnDispose" runat="server" OnClientClick="send_back_data();return false;"
                                        Text="Chọn &amp; đ&#243;ng cửa sổ" UseSubmitBehavior="False" Visible="False"
                                        meta:resourcekey="btnDisposeResource1" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
