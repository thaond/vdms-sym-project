<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="DataReformation.aspx.cs" Inherits="Sales_Sale_DataReformation" Title="Sửa đổi dữ liệu khách hàng"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <input type="hidden" runat="server" id="_CustomerID" value="-1" enableviewstate="true" />
    <input type="hidden" id="_PageStatus" value="DEFAULT" runat="server" enableviewstate="true" />
    <asp:Localize ID="loPageDesc" runat="server" Text="Sửa đổi dữ liệu khách hàng" meta:resourcekey="loPageDescResource1"></asp:Localize>
    <br />
    <br />
    <asp:ValidationSummary ID="ValidTest" runat="server" ValidationGroup="Search" DisplayMode="List"
        meta:resourcekey="ValidationSummary1Resource1" Width="100%" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Save"
        DisplayMode="List" meta:resourcekey="ValidationSummary1Resource1" Width="100%" />
    <asp:Label ID="lblErr" runat="server" ForeColor="Red" Visible="False" meta:resourcekey="lblErrResource1"></asp:Label>
    <asp:PlaceHolder ID="phCusList" runat="server">
        <table cellspacing="2" border="0" cellpadding="2">
            <%--<tr>
            <td nowrap>
                <asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
                <asp:Literal ID="liCustomerID" runat="server" Text="Số CMND/Mã số công ty:" meta:resourcekey="liCustomerIDResource1"></asp:Literal>
            </td>
            <td valign="top" nowrap>
                <asp:TextBox ID="txtIdentityNo" runat="server" Width="305px" meta:resourcekey="txtIdentityNoResource1"
                    MaxLength="10"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True"
                    ValidationGroup="Search" ControlToValidate="txtIdentityNo" ErrorMessage='Dữ liệu "Số CMND/Mã số công ty" không được để trống!'
                    meta:resourcekey="RequiredFieldValidator1Resource1" Text="*">
            </td>
        </tr>--%>
            <tr>
                <td valign="top" nowrap>
                    <asp:Image ID="image2" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
                </td>
                <td valign="top" nowrap>
                    <asp:Literal ID="liCustomerID0" runat="server" Text="Số máy:"></asp:Literal>
                </td>
                <td valign="top" nowrap>
                    <asp:TextBox ID="txtQueryEngNo" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True"
                        ValidationGroup="Search" ControlToValidate="txtQueryEngNo" ErrorMessage='Dữ liệu "Số máy" không được để trống!'
                        Text="*" meta:resourcekey="RequiredFieldValidator1Resource1a"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td valign="top" nowrap>
                    &nbsp;
                </td>
                <td valign="top" nowrap>
                    <asp:Literal ID="liCustomerID1" runat="server" meta:resourcekey="liCustomerID1Resource1"
                        Text="Customer type:"></asp:Literal>
                </td>
                <td valign="top" nowrap>
                    <asp:DropDownList ID="ddlCusClass" runat="server">
                        <asp:ListItem Text="Sale" Value="SL" meta:resourcekey="ListItemResource1a"></asp:ListItem>
                        <asp:ListItem Text="Service" Value="SV" meta:resourcekey="ListItemResource2a"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" nowrap>
                    &nbsp;
                </td>
                <td valign="top" nowrap>
                </td>
                <td valign="top" nowrap>
                    <asp:Button ID="btnTest" runat="server" Text="Kiểm tra" ValidationGroup="Search"
                        OnClick="btnTest_Click" meta:resourcekey="btnTestResource1" />
                    <asp:Button ID="btnAdd" runat="server" Text="Thêm mới" OnClick="btnAdd_Click" meta:resourcekey="btnAddResource1" />
                    <asp:Button ID="btnEdit" runat="server" Text="Sửa đổi" OnClick="btnEdit_Click" ValidationGroup="Search"
                        meta:resourcekey="btnEditResource1" Visible="False" />
                    <asp:Button ID="btnDelete" runat="server" Text="Xoá" Width="81px" OnClick="btnDelete_Click"
                        OnClientClick="return confirm(DeleteData);" meta:resourcekey="btnDeleteResource1"
                        Visible="False" />
                </td>
            </tr>
        </table>
        <div class="grid">
            <vdms:PageGridView ID="gvCust" runat="server" DataKeyNames="CustomerId" AutoGenerateColumns="False"
                AllowPaging="True" OnSelectedIndexChanged="gvCust_SelectedIndexChanged" meta:resourcekey="gvCustResource1"
                OnPageIndexChanging="gvCust_PageIndexChanged">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" meta:resourcekey="CommandFieldResource1" />
                    <asp:BoundField DataField="EngineNumber" HeaderText="Engine Number" SortExpression="EngineNumber"
                        meta:resourcekey="BoundFieldResource1" />
                    <asp:BoundField DataField="ItemType" HeaderText="Item Type" SortExpression="ItemType"
                        meta:resourcekey="BoundFieldResource2" />
                    <asp:BoundField DataField="ItemColor" HeaderText="Color" SortExpression="ItemColor"
                        meta:resourcekey="BoundFieldResource3" />
                    <asp:TemplateField HeaderText="Buy Date" SortExpression="BuyDate" meta:resourcekey="TemplateFieldResource1">
                        <ItemTemplate>
                            <asp:Label ToolTip='<%# Eval("CustomerId")%>' ID="Label1" runat="server" Text='<%# EvalDate(Eval("BuyDate")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="IdentifyNumber" HeaderText="Identify Number" SortExpression="IdentifyNumber"
                        meta:resourcekey="BoundFieldResource4" />
                    <asp:BoundField DataField="FullName" HeaderText="FullName" SortExpression="FullName"
                        meta:resourcekey="BoundFieldResource5" />
                    <asp:TemplateField HeaderText="Gender" SortExpression="Gender" meta:resourcekey="TemplateFieldResource2">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# EvalGender(Eval("Gender")) %>' meta:resourcekey="Label3Resource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Birthday" SortExpression="BirthDate" meta:resourcekey="TemplateFieldResource3">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# EvalDate(Eval("BirthDate")) %>' meta:resourcekey="Label2Resource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Phone" HeaderText="Phone" ReadOnly="True" SortExpression="Phone"
                        meta:resourcekey="BoundFieldResource6" />
                    <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address"
                        meta:resourcekey="BoundFieldResource7" />
                    <asp:BoundField DataField="Precinct" HeaderText="Precinct" SortExpression="Precinct"
                        meta:resourcekey="BoundFieldResource8" />
                    <asp:BoundField DataField="DistrictId" HeaderText="District" SortExpression="DistrictId"
                        meta:resourcekey="BoundFieldResource9" />
                    <asp:BoundField DataField="DealerCode" HeaderText="Dealer" SortExpression="DealerCode"
                        meta:resourcekey="BoundFieldResource10" />
                </Columns>
            </vdms:PageGridView>
            <asp:ObjectDataSource ID="odsCus" runat="server" SelectMethod="FindCustomers" SelectCountMethod="CountCustomers"
                TypeName="VDMS.I.Vehicle.CustomerDAO" EnablePaging="True">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtQueryEngNo" Name="engineNumber" PropertyName="Text"
                        Type="String" />
                    <asp:ControlParameter ControlID="ddlCusClass" Name="cusClass" PropertyName="SelectedValue"
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </asp:PlaceHolder>
    <asp:Panel ID="Panel1" runat="server" Visible="False" Width="100%" meta:resourcekey="Panel1Resource1">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 3px">
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" cellpadding="0" width="100%">
                        <tr id="item1">
                            <td align="left" style="width: 25%" valign="middle">
                                <asp:Image ID="Image1" runat="server" meta:resourcekey="imageResource1" SkinID="RequireField" />
                                <asp:Literal ID="liFullName" runat="server" meta:resourcekey="liFullNameResource1"
                                    Text="Họ tên:"></asp:Literal>
                            </td>
                            <td nowrap="nowrap" style="width: 25%" valign="middle">
                                <asp:TextBox ID="txtFullName" runat="server" MaxLength="50" meta:resourcekey="txtFullNameResource1"
                                    ReadOnly="True"></asp:TextBox><asp:RequiredFieldValidator ID="rqvCustomerID" runat="server"
                                        ControlToValidate="txtFullName" ErrorMessage="Họ và tên khách hàng không được bỏ trống!"
                                        meta:resourcekey="rqvCustomerIDResource1" Text="*" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </td>
                            <td align="left" style="width: 20%" valign="middle">
                            </td>
                            <td valign="middle">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" nowrap="nowrap" style="width: 25%" valign="middle">
                                <asp:Literal ID="liCusType" runat="server" meta:resourcekey="liCusTypeResource1"
                                    Text="Loại khách hàng :"></asp:Literal>
                            </td>
                            <td style="width: 25%" valign="middle">
                                <asp:DropDownList ID="ddlCusType" runat="server" Enabled="False" meta:resourcekey="ddlCusTypeResource1"
                                    Width="135px">
                                    <asp:ListItem meta:resourcekey="ListItemResource1" Text="Khách hàng mua xe" Value="0"></asp:ListItem>
                                    <asp:ListItem meta:resourcekey="ListItemResource2" Text="Khách hàng sửa xe" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="left" nowrap="nowrap" style="width: 20%" valign="middle">
                                <asp:Literal ID="liCus_Type" runat="server" meta:resourcekey="liCus_TypeResource1"
                                    Text="Phân loại khách hàng :"></asp:Literal>
                            </td>
                            <td valign="middle">
                                <asp:DropDownList ID="ddlCus_SetType" runat="server" Enabled="False" meta:resourcekey="ddlCus_SetTypeResource1">
                                    <asp:ListItem meta:resourcekey="ListItemResource3" Text="Thông thường" Value="0"></asp:ListItem>
                                    <asp:ListItem meta:resourcekey="ListItemResource4" Text="Quan trọng" Value="1"></asp:ListItem>
                                    <asp:ListItem meta:resourcekey="ListItemResource5" Text="Đặt biệt quan trọng" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="Tr1">
                            <td align="left" style="width: 25%" valign="middle">
                                <asp:Literal ID="liBirthDate" runat="server" meta:resourcekey="liBirthDateResource1"
                                    Text="Ngày sinh:"></asp:Literal>
                            </td>
                            <td style="width: 25%" valign="middle">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtBirthDate" runat="server" meta:resourcekey="txtBirthDateResource1"
                                                ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RangeValidator ID="rvBirthDate" runat="server" ControlToValidate="txtBirthDate"
                                                ErrorMessage="Dữ liệu &quot;Ngày sinh&quot; không đúng với định dạng ngày!" meta:resourcekey="rvBirthDate"
                                                SetFocusOnError="True" Type="Date" ValidationGroup="Save">*</asp:RangeValidator>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgBDate" runat="server" OnClientClick="return false;" SkinID="CalendarImageButton" />
                                        </td>
                                    </tr>
                                </table>
                                <ajaxToolkit:MaskedEditExtender ID="MEEBirthday" runat="server" BehaviorID="MEEBirthday"
                                    CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY"
                                    CultureDatePlaceholder="/" CultureDecimalPlaceholder="." CultureThousandsPlaceholder=","
                                    CultureTimePlaceholder=":" Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtBirthDate">
                                </ajaxToolkit:MaskedEditExtender>
                                <ajaxToolkit:CalendarExtender ID="CEBirthday" runat="server" BehaviorID="CEBirthday"
                                    Enabled="True" PopupButtonID="imgBDate" TargetControlID="txtBirthDate">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                            <td align="left" style="width: 20%" valign="middle">
                                <asp:Literal ID="liSex" runat="server" meta:resourcekey="liSexResource1" Text="Giới tính:"></asp:Literal>
                            </td>
                            <td valign="middle">
                                <asp:DropDownList ID="ddlSex" runat="server" Enabled="False" meta:resourcekey="ddlSexResource1">
                                    <asp:ListItem meta:resourcekey="ListItemResource6" Text="M" Value="1"></asp:ListItem>
                                    <asp:ListItem meta:resourcekey="ListItemResource7" Text="F" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="item3">
                            <td align="left" style="width: 25%" valign="middle">
                                <asp:Literal ID="liCPhone" runat="server" meta:resourcekey="liCPhoneResource1" Text="Số điện thoại:"></asp:Literal>
                            </td>
                            <td style="width: 25%" valign="middle">
                                <asp:TextBox ID="txtCPhone" runat="server" MaxLength="11" meta:resourcekey="txtCPhoneResource1"
                                    ReadOnly="True"></asp:TextBox><asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                                        runat="server" ControlToValidate="txtCPhone" ErrorMessage="Số điện thoại của khách hàng phải được gõ bằng số!"
                                        meta:resourcekey="RegularExpressionValidator2Resource1" Text="*" ValidationExpression="\s*[0-9]\d*\s*"
                                        ValidationGroup="Save"></asp:RegularExpressionValidator>
                            </td>
                            <td align="left" style="width: 20%" valign="middle">
                                <asp:Literal ID="liCMobile" runat="server" meta:resourcekey="liCMobileResource1"
                                    Text="Số ĐTDĐ:"></asp:Literal>
                            </td>
                            <td nowrap="nowrap" valign="middle">
                                <asp:TextBox ID="txtCMobile" runat="server" MaxLength="15" meta:resourcekey="txtCMobileResource1"
                                    ReadOnly="True"></asp:TextBox><%--<asp:RequiredFieldValidator ID="rfvCus_Mobile" runat="server" ControlToValidate="txtCMobile"
									ErrorMessage="Số điện thoại di động của kh&#225;ch h&#224;ng kh&#244;ng được bỏ trống!"
									meta:resourcekey="rfvCus_MobileResource1" Text="*" ValidationGroup="Save"></asp:RequiredFieldValidator>--%><asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCMobile"
                                        ErrorMessage="Số điện thoại di động của khách hàng phải được gõ bằng số!" meta:resourcekey="RegularExpressionValidator1Resource1"
                                        Text="*" ValidationExpression="\s*[0-9]\d*\s*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr id="item4">
                            <td align="left" style="width: 25%; font-weight: bold;" valign="middle">
                                <asp:Literal ID="Literal1" runat="server" meta:resourcekey="Literal1Resource1" Text="Địa chỉ hộ khẩu:"></asp:Literal>
                            </td>
                            <td align="center" style="width: 25%" valign="middle">
                            </td>
                            <td align="left" style="width: 20%" valign="middle">
                            </td>
                            <td align="center" valign="middle">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 25%" valign="middle">
                                <asp:Literal ID="liHouseNo" runat="server" meta:resourcekey="liHouseNoResource1"
                                    Text="Số nhà, phố:"></asp:Literal>
                            </td>
                            <td align="left" style="width: 25%" valign="middle">
                                <asp:TextBox ID="txtAddress" runat="server" MaxLength="256" meta:resourcekey="txtAddressResource1"
                                    ReadOnly="True"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 20%" valign="middle">
                                <asp:Literal ID="liPrecinct" runat="server" meta:resourcekey="liPrecinctResource1"
                                    Text="Phường, xã:"></asp:Literal>
                            </td>
                            <td align="left" valign="middle">
                                <asp:TextBox ID="txtPrecinct" runat="server" MaxLength="30" meta:resourcekey="txtPrecinctResource1"
                                    ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="item6">
                            <td align="left" style="width: 25%" valign="middle">
                                <asp:Literal ID="liDistrict" runat="server" meta:resourcekey="liDistrictResource1"
                                    Text="Quận, huyện:"></asp:Literal>
                            </td>
                            <td align="left" style="width: 25%" valign="middle">
                                <asp:TextBox ID="txtDistrict" runat="server" MaxLength="30" meta:resourcekey="txtDistrictResource1"
                                    ReadOnly="True"></asp:TextBox>
                            </td>
                            <td align="left" nowrap="nowrap" style="width: 20%" valign="middle">
                                <asp:Literal ID="liCity" runat="server" meta:resourcekey="liCityResource1" Text="Tỉnh,thành phố:"></asp:Literal>
                            </td>
                            <td align="left" valign="middle">
                                <asp:DropDownList ID="ddlProvince" runat="server" meta:resourcekey="ddlProvinceResource1">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="item8">
                            <td align="left" style="width: 25%" valign="middle">
                                <asp:Literal ID="liCEmail" runat="server" meta:resourcekey="liCEmailResource1" Text="E-MAIL:"></asp:Literal>
                            </td>
                            <td colspan="3" valign="middle">
                                <asp:TextBox ID="txtCEmail" runat="server" MaxLength="256" meta:resourcekey="txtCEmailResource1"
                                    ReadOnly="True"></asp:TextBox><asp:RegularExpressionValidator ID="revCus_Email" runat="server"
                                        ControlToValidate="txtCEmail" ErrorMessage="Định dạng mail của khách hàng chưa chính xác!"
                                        meta:resourcekey="revCus_EmailResource1" Text="*" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        ValidationGroup="Save"></asp:RegularExpressionValidator><%--<asp:RequiredFieldValidator
										ID="rfvCus_Email" runat="server" ControlToValidate="txtCEmail" ErrorMessage="Email của kh&#225;ch h&#224;ng kh&#244;ng được bỏ trống!"
										meta:resourcekey="rfvCus_EmailResource1" Text="*" ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 25%" valign="middle">
                                <asp:Literal ID="liJobType" runat="server" meta:resourcekey="liJobTypeResource1"
                                    Text="Nghề nghiệp:"></asp:Literal>
                            </td>
                            <td colspan="3" valign="middle">
                                <span class="form_word">
                                    <asp:RadioButtonList ID="tblCus_JobType" runat="server" meta:resourcekey="tblCus_JobTypeResource1"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem meta:resourcekey="ListItemResource9" Text="Học sinh" Value="1"></asp:ListItem>
                                        <asp:ListItem meta:resourcekey="ListItemResource10" Text="Công sở" Value="2"></asp:ListItem>
                                        <asp:ListItem meta:resourcekey="ListItemResource11" Text="Tự do" Value="3"></asp:ListItem>
                                        <asp:ListItem meta:resourcekey="ListItemResource12" Text="Quân nhân" Value="4"></asp:ListItem>
                                        <asp:ListItem meta:resourcekey="ListItemResource13" Selected="True" Text="Khác" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </span>
                            </td>
                        </tr>
                        <tr valign="middle">
                            <td align="left" style="width: 25%" valign="middle">
                                <asp:Literal ID="liModel" runat="server" meta:resourcekey="liModelResource1" Text="Chủng loại xe:"></asp:Literal>
                            </td>
                            <td align="left" valign="middle">
                                <asp:TextBox ID="txtModel" runat="server" CssClass="readOnlyInputField" meta:resourcekey="txtModelResource1"
                                    ReadOnly="True"></asp:TextBox>
                            </td>
                            <td align="left" valign="middle">
                                <asp:Literal ID="liEngineNo" runat="server" meta:resourcekey="liEngineNoResource1"
                                    Text="Số động cơ:"></asp:Literal>
                            </td>
                            <td align="left" valign="middle">
                                <asp:TextBox ID="txtEngineNo" runat="server" CssClass="readOnlyInputField" meta:resourcekey="txtEngineNoResource1"
                                    ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr valign="middle">
                            <td align="left" style="width: 25%" valign="middle">
                                <asp:Literal ID="liDescription" runat="server" meta:resourcekey="liDescriptionResource1"
                                    Text="Chú thích:"></asp:Literal>
                            </td>
                            <td align="left" colspan="3" style="padding-right: 15px" valign="middle">
                                <asp:TextBox ID="txtCus_Desc" runat="server" Height="70px" MaxLength="2000" meta:resourcekey="txtCus_DescResource2"
                                    ReadOnly="True" TextMode="MultiLine" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr valign="middle">
                            <td align="right" colspan="4" style="padding-right: 15px">
                                <asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSaveResource1" OnClick="btnSave_Click"
                                    Text="Lưu" ValidationGroup="Save" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                    meta:resourcekey="btnCancelResource1" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
