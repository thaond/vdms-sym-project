<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="WarrantyInfo.aspx.cs" Inherits="Service_WarrantyInfo" Title="Modify Warranty Information" %>

<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
    <asp:HiddenField ID="_CustomerID" runat="server" Value="-1" />
    <!-- GUI: content page -->
    <asp:Label ID="lbMes" runat="server" meta:resourcekey="lbMesResource1" />
    <div runat="server" id="dvMsg">
    </div>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Input"
        DisplayMode="List" meta:resourcekey="ValidationSummary1Resource1" />
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="InputCus"
        DisplayMode="List" />
    <!-- Search:content -->
    <asp:PlaceHolder ID="plSearchContent" runat="server">
        <table border="0" cellpadding="2" cellspacing="2">
            <tr>
                <td>
                    <asp:Literal ID="litEngineNumber" runat="server" Text="Engine Number:" meta:resourcekey="litEngineNumberResource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtEngineNumber" runat="server" CssClass="inputKeyField" MaxLength="50"
                        meta:resourcekey="txtEngineNumberResource1"></asp:TextBox>
                </td>
            </tr>
            <!-- Engine:type -->
            <tr>
                <td>
                    <asp:Literal ID="liEngineType" runat="server" Text="Engine type:" meta:resourcekey="EngineTypeLi"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtEngineType" runat="server" CssClass="inputKeyField" MaxLength="7"
                        meta:resourcekey="EngineTypeTxt"></asp:TextBox>
                </td>
            </tr>
            <!-- Customer:identify number -->
            <tr>
                <td>
                    <asp:Literal ID="liCustomerId" runat="server" Text="Customer's ID number:" meta:resourcekey="CustomerIdLi"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtCusID" runat="server" CssClass="inputKeyField" MaxLength="11"
                        meta:resourcekey="CustomerIdTxt"></asp:TextBox>
                </td>
            </tr>
            <!-- Customer:name -->
            <tr>
                <td>
                    <asp:Literal ID="liCusName" runat="server" Text="Customer's name:" meta:resourcekey="CustomerNameLi"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtCusName" runat="server" CssClass="inputKeyField" MaxLength="255"
                        meta:resourcekey="CustomerNameTxt"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <!-- Button: Excel -->
                    <asp:Button ID="btlExcel" runat="server" meta:resourceKey="btnTestResource1x" Text="Export to excel"
                        SkinID="SubmitButton" OnClick="ExportToExcel" />
                    <!-- Button:search -->
                    <asp:Button ID="btnTest" runat="server" meta:resourceKey="btnTestResource1" Text="Check"
                        SkinID="SubmitButton" OnClick="Search" />
                    <!-- Button:Addnew -->
                    <asp:Button ID="btnAddWarrantyInfo" runat="server" Text="Warranty information" SkinID="SubmitButton"
                        OnClick="WarrantyInfoAddNew" meta:resourcekey="btnAddWarrantyInfoResource1" />
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <!-- Search:Input warranty information panel -->
    <asp:PlaceHolder ID="plInputWInf" runat="server" Visible="False">
        <table border="0" cellpadding="2" cellspacing="2">
            <tr>
                <td>
                    <asp:Literal ID="liEngineNumber" runat="server" Text="Engine number:" meta:resourcekey="liEngineNumberResource1" />
                </td>
                <td>
                    <asp:TextBox ID="txtEngineNumberInput" runat="server" MaxLength="50" Style="text-transform: uppercase"
                        meta:resourcekey="txtEngineNumberInputResource1" />
                    <asp:Button Text="..." runat="server" ID="btnFilterEng" OnClick="SelectEngineNo" />
                    <asp:RequiredFieldValidator ID="rfvEngineNumberInput" runat="server" CssClass="Validator"
                        Text="*" SetFocusOnError="True" ValidationGroup="Input" ErrorMessage="Engine number can not be blank!"
                        ControlToValidate="txtEngineNumberInput" meta:resourcekey="rfvEngineNumberInputResource1"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:Literal ID="liType" runat="server" Text="Model:" meta:resourcekey="liTypeRes" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtType" ReadOnly="true" CssClass="readOnlyInputField" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <div class="grid">
                        <vdms:PageGridView ID="gvItemins" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            OnSelectedIndexChanging="gvSelectItem_SelectedIndexChanging" DataKeyNames="ItemInstanceId"
                            Width="492px" DataSourceID="odsSelectItem" Visible="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Engine number" SortExpression="Enginenumber" meta:resourcekey="TemplateFieldResource9">
                                    <ItemTemplate>
                                        <asp:Literal ID="litSelectedSoldItem" runat="server" Text='<%# Eval("EngineNumber") %>'></asp:Literal>
                                        <asp:HiddenField ID="hdImporteddate" runat="server" Value='<%# Eval("ImportedDate", "{0:d}") %>' />
                                        <asp:HiddenField ID="hdItemtype" runat="server" Value='<%# Eval("ItemType") %>' />
                                        <asp:HiddenField ID="hdColor" runat="server" Value='<%# Eval("Color") %>' />
                                        <asp:HiddenField ID="hdDealerCode" runat="server" Value='<%# Eval("DealerCode") %>' />
                                        <asp:HiddenField ID="hdDatabaseCode" runat="server" Value='<%# Eval("DatabaseCode") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField HeaderText="DbCode" DataField="DatabaseCode" ReadOnly="true" />--%>
                                <asp:BoundField HeaderText="Model" DataField="Itemtype" ReadOnly="true" meta:resourcekey="ModelCol" />
                                <asp:BoundField HeaderText="Color" DataField="Color" ReadOnly="true" meta:resourcekey="ColorCol" />
                                <asp:CommandField ButtonType="Button" ShowSelectButton="True" meta:resourcekey="CommandFieldResource1">
                                    <ItemStyle Width="10px" />
                                </asp:CommandField>
                            </Columns>
                            <%--<PagerSettings Position="Top" Mode="NextPreviousFirstLast" />
						<PagerTemplate>
							<div style="float: left;">
								<asp:Literal ID="litgvIteminsPageInfo" runat="server" meta:resourcekey="litgvSelectItemPageInfoResource1"></asp:Literal></div>
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
						</PagerTemplate>--%>
                        </vdms:PageGridView>
                    </div>
                    <asp:ObjectDataSource ID="odsSelectItem" runat="server" EnablePaging="True" SelectMethod="SelectNot"
                        TypeName="VDMS.I.ObjectDataSource.ItemInstanceDataSource" SelectCountMethod="SelectCount">
                        <SelectParameters>
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="engineNumberLike" DefaultValue="" Type="String" />
                            <asp:Parameter Name="dealerCode" DefaultValue="" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal Text="Color:" ID="liColor" runat="server" meta:resourcekey="liColorRes" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtColor" ReadOnly="true" CssClass="readOnlyInputField" />
                </td>
                <td>
                    <asp:Literal ID="liPurchaseDateInput" runat="server" Text="Purchase date:" meta:resourcekey="liPurchaseDateInputResource1" />
                </td>
                <td>
                    <asp:TextBox ID="txtPurchaseDateInput" runat="server" MaxLength="12" meta:resourcekey="txtPurchaseDateInputResource1" />
                    <asp:ImageButton ID="ibPurchaseDateInput" runat="server" SkinID="CalendarImageButton"
                        OnClientClick="return false;" meta:resourcekey="ibPurchaseDateInputResource1" />
                    <asp:RangeValidator ID="rvPurchaseDateInput" runat="server" ControlToValidate="txtPurchaseDateInput"
                        ErrorMessage='Ngày bán không đúng với định dạng ngày!' SetFocusOnError="True"
                        Type="Date" ValidationGroup="Input" meta:resourcekey="rvPurchaseDateInputResource1">*</asp:RangeValidator>
                    <asp:RequiredFieldValidator ID="rfvPurchaseDateInput" runat="server" CssClass="Validator"
                        Enabled="true" Text="*" SetFocusOnError="True" ValidationGroup="Input" ErrorMessage="Purchase date can not be blank!"
                        ControlToValidate="txtPurchaseDateInput" meta:resourcekey="rfvPurchaseDateInputResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="MEEBirthday" runat="server" Mask="99/99/9999"
                        MaskType="Date" TargetControlID="txtPurchaseDateInput" BehaviorID="MEEBirthday"
                        CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY"
                        CultureDatePlaceholder="/" CultureDecimalPlaceholder="." CultureThousandsPlaceholder=","
                        CultureTimePlaceholder=":" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="cePurchaseDateInput" runat="server" TargetControlID="txtPurchaseDateInput"
                        PopupButtonID="ibPurchaseDateInput" BehaviorID="cePurchaseDateInput" Enabled="True" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="liKmCountInput" runat="server" Text="KM count:" meta:resourcekey="liKmCountInputResource1" />
                </td>
                <td>
                    <asp:TextBox ID="txtKmCountInput" runat="server" MaxLength="5" meta:resourcekey="txtKmCountInputResource1" />
                    <asp:RangeValidator ID="rvKmCount" runat="server" ErrorMessage="Km count must be between 0 and 99999"
                        ControlToValidate="txtKmCountInput" SetFocusOnError="True" MinimumValue="0" MaximumValue="99999"
                        ValidationGroup="Input" Type="Integer" Text="*" meta:resourcekey="rvKmCountResource1"></asp:RangeValidator>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr runat="server" id="trDealer">
                <td>
                    <asp:Literal ID="liDealerCodeInput" runat="server" Text="Dealer:" meta:resourcekey="liDealerCodeInputResource1" />
                </td>
                <td colspan="3">
                    <asp:DropDownList OnDataBound="ddlDc_databound" ID="ddlDealerCode" runat="server"
                        meta:resourcekey="ddlDealerCodeResource1" />
                </td>
            </tr>
            <asp:PlaceHolder ID="CusInfph" runat="server" Visible="false">
                <!-- Customer Infomtaion Panel -->
                <tr>
                    <td align="center" colspan="4">
                        <h5>
                            <asp:Literal Text="Customer's infomation" ID="CusInfPanelLabel" runat="server" meta:resourcekey="CusInfPanelLabel" />
                        </h5>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal Text="Customer's name:" ID="liCusNameInput" runat="server" meta:resourcekey="liCusNameInputRes" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCusNameInput" MaxLength="100" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvtxtCusNameInput" runat="server" CssClass="Validator"
                            Text="*" SetFocusOnError="True" ValidationGroup="InputCus" ErrorMessage="Customer's name can not be blank!"
                            ControlToValidate="txtCusNameInput" meta:resourcekey="RequiredFieldValidator2Resource1" />
                    </td>
                    <td>
                        <asp:Literal ID="liCustomerIDInput" runat="server" Text="Customer's identify number:"
                            meta:resourcekey="liCustomerIDInputResource1" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCustomerIDInput" runat="server" MaxLength="20" ReadOnly="True"
                            CssClass="readOnlyInputField" meta:resourcekey="txtCustomerIDInputResource1" />
                    </td>
                </tr>
                <tr>
                    <td nowrap="nowrap">
                        <asp:Literal ID="liCusType" runat="server" Text="Loại kh&#225;ch h&#224;ng :" meta:resourcekey="liCusTypeResource1"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCusType" runat="server" Enabled="False" Width="135px" meta:resourcekey="ddlCusTypeResource1">
                            <asp:ListItem meta:resourcekey="ListItemResource1" Text="Kh&#225;ch h&#224;ng mua xe"
                                Value="0"></asp:ListItem>
                            <asp:ListItem meta:resourcekey="ListItemResource2" Text="Kh&#225;ch h&#224;ng sửa xe"
                                Value="1"></asp:ListItem>
                            <asp:ListItem Selected="True" meta:resourcekey="ListItemResource2x" Text="Warranty info"
                                Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td nowrap="nowrap">
                        <asp:Literal ID="liCus_Type" runat="server" Text="Ph&#226;n loại kh&#225;ch h&#224;ng :"
                            meta:resourcekey="liCus_TypeResource1"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCus_SetType" runat="server" meta:resourcekey="ddlCus_SetTypeResource1">
                            <asp:ListItem meta:resourcekey="ListItemResource3" Text="Th&#244;ng thường" Value="0"></asp:ListItem>
                            <asp:ListItem meta:resourcekey="ListItemResource4" Text="Quan trọng" Value="1"></asp:ListItem>
                            <asp:ListItem meta:resourcekey="ListItemResource5" Text="Đặt biệt quan trọng" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="liBirthDate" runat="server" Text="Birthday:" meta:resourcekey="liBirthDateRes" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtBirthDateInput" runat="server" meta:resourcekey="txtBirthDateResource1" />
                        <asp:ImageButton ID="ImageButton3" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
                            meta:resourcekey="ibPurchaseDateInputResource1" />
                        <asp:RangeValidator ID="rvBirthDate" runat="server" ControlToValidate="txtBirthDateInput"
                            ErrorMessage='Dữ liệu "Ngày sinh" không đúng với định dạng ngày!' SetFocusOnError="True"
                            Type="Date" ValidationGroup="InputCus" meta:resourcekey="rvBirthDate">*</asp:RangeValidator>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtBirthDateInput"
                            Mask="99/99/9999" MaskType="Date" BehaviorID="MaskedEditExtender1" CultureAMPMPlaceholder="AM;PM"
                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                            CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                            Enabled="True" />
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBirthDateInput"
                            PopupButtonID="ImageButton3" BehaviorID="CalendarExtender1" Enabled="True" />
                    </td>
                    <td>
                        <asp:Literal ID="liSex" runat="server" Text="Giới t&#237;nh:" meta:resourcekey="liSexResource1" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSexInput" runat="server" CssClass="lblClass" meta:resourcekey="ddlSexResource1">
                            <asp:ListItem Value="1" meta:resourcekey="ListItemResource6" Text="M"></asp:ListItem>
                            <asp:ListItem Value="0" meta:resourcekey="ListItemResource7" Text="F"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="liPhone" runat="server" Text="Số điện thoại:" meta:resourcekey="liCPhoneResource1" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="20" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtPhone"
                            ErrorMessage="Số điện thoại của kh&#225;ch h&#224;ng phải được g&#245; bằng số!"
                            ValidationExpression="\s*[0-9]\d*\s*" ValidationGroup="InputCus" meta:resourcekey="RegularExpressionValidator2Resource1"
                            Text="*">
                        </asp:RegularExpressionValidator>
                    </td>
                    <td>
                        <asp:Literal ID="liMobile" runat="server" Text="Số di động:" meta:resourcekey="liCMobileResource1" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server" MaxLength="20" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtMobile"
                            ErrorMessage="Số điện thoại di động của kh&#225;ch h&#224;ng phải được g&#245; bằng số!"
                            ValidationExpression="\s*[0-9]\d*\s*" ValidationGroup="InputCus" meta:resourcekey="RegularExpressionValidator1Resource1"
                            Text="*">
                        </asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="liEmail" runat="server" Text="E-MAIL:" meta:resourcekey="liemail" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="256" />
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="liJobType" runat="server" Text="Nghề nghiệp:" meta:resourcekey="liJobTypeResource1"></asp:Literal>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="tblCus_JobType" runat="server" RepeatDirection="Horizontal"
                            meta:resourcekey="tblCus_JobTypeResource1">
                            <asp:ListItem Value="1" meta:resourcekey="ListItemResource9" Text="Học sinh"></asp:ListItem>
                            <asp:ListItem Value="2" meta:resourcekey="ListItemResource10" Text="C&#244;ng sở"></asp:ListItem>
                            <asp:ListItem Value="3" meta:resourcekey="ListItemResource11" Text="Tự do"></asp:ListItem>
                            <asp:ListItem Value="4" meta:resourcekey="ListItemResource12" Text="Qu&#226;n nh&#226;n"></asp:ListItem>
                            <asp:ListItem Selected="True" Value="5" meta:resourcekey="ListItemResource13" Text="Kh&#225;c"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="liHouseNo" runat="server" Text="Số nh&#224;, phố:" meta:resourcekey="liHouseNoResource1" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddressInput" runat="server" CssClass="lblClass" MaxLength="255"
                            meta:resourcekey="txtAddressResource1" />
                        <asp:RequiredFieldValidator ID="rfvAddress" runat="server" CssClass="Validator" Text="*"
                            SetFocusOnError="True" ValidationGroup="InputCus" ErrorMessage="Address can not be blank!"
                            Enabled="false" ControlToValidate="txtAddressInput" meta:resourcekey="txtAddRes" />
                    </td>
                    <td>
                        <asp:Literal ID="liPrecinct" runat="server" Text="Phường, x&#227;:" meta:resourcekey="liPrecinctResource1"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPrecinctInput" runat="server" CssClass="lblClass" MaxLength="256"
                            meta:resourcekey="txtPrecinctResource1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPrecinct" meta:resourcekey="rfvPrecinctRes" runat="server"
                            CssClass="Validator" Text="*" SetFocusOnError="True" ValidationGroup="InputCus"
                            ErrorMessage="Precinct can not be blank!" Enabled="false" ControlToValidate="txtPrecinctInput" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="liDistrict" runat="server" Text="Quận, huyện:" meta:resourcekey="liDistrictResource1" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtDistrictInput" runat="server" CssClass="lblClass" MaxLength="256"
                            meta:resourcekey="txtDistrictResource1" />
                        <asp:RequiredFieldValidator ID="rfvDistrict" meta:resourcekey="rfvDistrictRes" runat="server"
                            CssClass="Validator" Text="*" SetFocusOnError="True" ValidationGroup="InputCus"
                            ErrorMessage="District can not be blank!" Enabled="false" ControlToValidate="txtDistrictInput" />
                    </td>
                    <td>
                        <asp:Literal ID="liCity" runat="server" Text="Tỉnh, th&#224;nh phố:" meta:resourcekey="liCityResource1" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlProvinceInput" runat="server" CssClass="lblClass" meta:resourcekey="ddlProvinceResource1">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="liDesc" runat="server" Text="Chú thích:" meta:resourcekey="liDesc" />
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtCus_Desc" runat="server" Height="70px" TextMode="MultiLine" Width="98%"
                            meta:resourcekey="txtCus_DescResource2" MaxLength="2000"></asp:TextBox>
                    </td>
                </tr>
            </asp:PlaceHolder>
            <!-- Button panel -->
            <tr>
                <td colspan="4" align="right">
                    <asp:Button ID="btnInsertWarrantyInfo" runat="server" Text="Save warranty information"
                        ValidationGroup="Input" Enabled="false" OnClick="InputWarrantyInfo" meta:resourcekey="btnInsertWarrantyInfoResource1"
                        TabIndex="7" UseSubmitBehavior="False" />
                    <asp:Button ID="btnInsertCus" runat="server" Text="Save customer" Enabled="False"
                        OnClick="InsertData" ValidationGroup="InputCus" meta:resourcekey="btnInsertCusResource1"
                        TabIndex="8" UseSubmitBehavior="False" />
                    <!-- Button:Reset -->
                    <asp:Button ID="btnReset" runat="server" Text="Reset" SkinID="SubmitButton" OnClick="WarrantyInfoAddNew"
                        meta:resourcekey="btnResetResource1" TabIndex="9" UseSubmitBehavior="False" />
                    <!-- Button:Exit -->
                    <asp:Button ID="btnExit" Text="Exit" runat="server" UseSubmitBehavior="False" meta:resourcekey="btnExitRes"
                        TabIndex="10" />
                </td>
            </tr>
            <!-- Template row -->
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <p />
    <div class="grid">
        <vdms:PageGridView CssClass="GridView" ID="gvItem" runat="server" AllowPaging="True"
            AutoGenerateColumns="False" DataKeyNames="Id" Width="100%" DataSourceID="ObjectDataSource1"
            OnDataBound="gvItem_DataBound" meta:resourcekey="gvItemResource1" OnRowEditing="gvItem_OnRowEditing">
            <Columns>
                <asp:BoundField HeaderText="Engine Number" DataField="Id" ReadOnly="True" meta:resourcekey="BoundFieldResource1" />
                <asp:TemplateField HeaderText="Dealercode" meta:resourcekey="TemplateFieldResource3">
                    <ItemStyle CssClass="vCenterObj" Wrap="False" />
                    <ItemTemplate>
                        <asp:Literal ID="liCusIdkeys" Text='<%# Eval("Customer.Id") %>' runat="server" Visible="False"></asp:Literal>
                        <asp:Literal ID="liDealercode" Text='<%# Eval("Selldealercode") %>' runat="server"></asp:Literal>
                        <asp:Literal ID="liItemcode" Text='<%# Eval("Itemcode") %>' runat="server" Visible="False"></asp:Literal>
                        <asp:Literal ID="liColor" Text='<%# Eval("Color") %>' runat="server" Visible="False"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Purchase Date" meta:resourcekey="TemplateFieldResource1">
                    <ItemStyle CssClass="vCenterObj" Wrap="False" />
                    <ItemTemplate>
                        <asp:Literal ID="liPdate" Text='<%# ((DateTime)Eval("Purchasedate")).ToString("d") %>'
                            runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Km Count" meta:resourcekey="TemplateFieldResource2">
                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                    <ItemTemplate>
                        <asp:Literal ID="liEnNo" Text="<%# Bind('Id') %>" runat="server" Visible="False" />
                        <asp:Literal ID="likmCount" Text='<%# Eval("Kmcount") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer's full name" meta:resourcekey="TemplateFieldResource4">
                    <ItemStyle CssClass="vCenterObj" Wrap="False" />
                    <ItemTemplate>
                        <asp:Literal ID="licusfullname" Text='<%# Eval("Customer.Fullname") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer's identify number" meta:resourcekey="TemplateFieldResource5">
                    <ItemStyle CssClass="vCenterObj" Wrap="False" />
                    <ItemTemplate>
                        <asp:Literal ID="licusid" Text='<%# Eval("Customer.Identifynumber") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer's telephone" meta:resourcekey="TemplateFieldResource6">
                    <ItemStyle CssClass="vCenterObj" Wrap="False" />
                    <ItemTemplate>
                        <%# Eval("Customer.Tel") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer's address" meta:resourcekey="TemplateFieldResource7">
                    <ItemStyle CssClass="vCenterObj" Wrap="true" />
                    <ItemTemplate>
                        <%# EvalAddress(Eval("Customer")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Edit" ShowHeader="False" meta:resourcekey="TemplateFieldResource8">
                    <EditItemTemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="True" CommandName="Update"
                            ImageUrl="~/Images/update.gif" Text="Update" />&nbsp;<asp:ImageButton ID="ImageButton2"
                                runat="server" CausesValidation="False" CommandName="Cancel" ImageUrl="~/Images/cancel.gif" />
                    </EditItemTemplate>
                    <ItemStyle CssClass="centerObj" />
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Edit"
                            meta:resourcekey="EditImageButton1Resource1" ImageUrl="~/Images/Edit.gif" Visible='<%# EvalEditable(Eval("Databasecode"), Eval("Id")) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <%--<PagerSettings Position="Top" Mode="NextPreviousFirstLast" />
		<PagerTemplate>
			<div style="float: left;">
				<asp:Literal ID="litPageInfo" runat="server" meta:resourcekey="litPageInfoResource1"></asp:Literal></div>
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
		</PagerTemplate>--%>
            <EmptyDataTemplate>
                <b>
                    <asp:Localize ID="litEmpty" runat="server" Text="No information found! Please change the condition search."
                        meta:resourcekey="litEmptyResource1"></asp:Localize></b>
            </EmptyDataTemplate>
        </vdms:PageGridView>
    </div>
    <asp:PlaceHolder ID="phCusInf" runat="server"></asp:PlaceHolder>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DeleteMethod="Delete"
        EnablePaging="True" SelectCountMethod="SelectCount" SelectMethod="Select" TypeName="VDMS.I.ObjectDataSource.WarrantyInfoDataSource"
        UpdateMethod="Update">
        <SelectParameters>
            <asp:Parameter DefaultValue="10" Name="maximumRows" Type="Int32" />
            <asp:Parameter DefaultValue="0" Name="startRowIndex" Type="Int32" />
            <asp:ControlParameter Name="EngineNo" Type="String" ControlID="txtEngineNumber" PropertyName="Text" />
            <asp:ControlParameter Name="EngineType" Type="String" ControlID="txtEngineType" PropertyName="Text" />
            <asp:ControlParameter Name="CusName" Type="String" ControlID="txtCusName" PropertyName="Text" />
            <asp:ControlParameter Name="CusID" Type="String" ControlID="txtCusID" PropertyName="Text" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
