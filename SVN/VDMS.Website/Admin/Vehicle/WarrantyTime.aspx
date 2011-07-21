<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="WarrantyTime.aspx.cs" Inherits="Admin_Database_WarrantyTime" Title="Sửa đổi thời hạn bảo hành linh kiện"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">

    <script language="javascript" type="text/javascript">
        function DelMessage() {
            return confirm('<%= DeleteAlert %>');
        }
    </script>

    <asp:Label ID="lbErr" runat="server"></asp:Label>
    <asp:Panel CssClass="form" ID="placeSearchControl" runat="server">
        <table border="0" cellspacing="2" cellpadding="2">
            <tr>
                <td valign="middle" colspan="3">
                    <asp:ValidationSummary ID="validSearchSum" runat="server" ValidationGroup="SearchCondition"
                        Width="100%" meta:resourcekey="validSearchSumResource1" />
                </td>
            </tr>
            <tr>
                <td valign="middle" style="width: 145px">
                    &nbsp;<asp:Localize ID="litOrderNumber" runat="server" meta:resourcekey="litOrderNumberResource1"
                        Text="Part code from:"></asp:Localize>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtPartCodeNo1" runat="server" meta:resourcekey="txtOrderNo1Resource1"
                        Width="130px" MaxLength="35" Style="text-transform: uppercase"></asp:TextBox>
                </td>
                <td valign="middle">
                    <asp:Localize ID="litToNo" runat="server" meta:resourcekey="litToNoResource1" Text="to:"></asp:Localize>&nbsp;<asp:TextBox
                        ID="txtPartCodeNo2" runat="server" MaxLength="35" meta:resourcekey="txtOrderNo2Resource1"
                        Style="text-transform: uppercase" Visible="False" Width="130px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="middle" style="width: 145px">
                    &nbsp;<asp:Localize ID="litPartWarrantyDate" runat="server" meta:resourcekey="litOrderDateResource1"
                        Text="Warranty duration:"></asp:Localize>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtWarrantyFrom" runat="server" meta:resourcekey="txtFromDateResource1"
                        Width="130px" MaxLength="3"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator5" runat="server" ControlToValidate="txtWarrantyFrom"
                        ErrorMessage="Number of warranty month must be input a numberic format!" MaximumValue="9"
                        meta:resourcekey="rvCus_PhoneNumberResource1" MinimumValue="0" Text="*" ValidationGroup="SearchCondition"></asp:RangeValidator>
                </td>
                <td valign="middle">
                    <asp:Localize ID="litTodate" runat="server" meta:resourcekey="litTodateResource1"
                        Text="to:"></asp:Localize>&nbsp;
                    <asp:TextBox ID="txtWarrantyTo" runat="server" MaxLength="3" meta:resourcekey="txtToDateResource1"
                        Width="130px"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator6" runat="server" ControlToValidate="txtWarrantyTo"
                        ErrorMessage="Number of warranty month must be input a numberic format!" MaximumValue="9"
                        meta:resourcekey="rvCus_PhoneNumberResource1" MinimumValue="0" Text="*" ValidationGroup="SearchCondition"></asp:RangeValidator>
                    <asp:Localize ID="loWarrantyMonthUnitPrice" runat="server" meta:resourcekey="MonthUnit"
                        Text="(Month)"></asp:Localize>
                </td>
            </tr>
            <tr>
                <td valign="middle" style="width: 145px">
                    &nbsp;<asp:Localize ID="loWarranty1" runat="server" meta:resourcekey="litKMResource1"
                        Text="Km warranty from:"></asp:Localize>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtWarrantyKm1" runat="server" meta:resourcekey="txtOrderNo1Resource1"
                        Width="130px" MaxLength="15"></asp:TextBox>
                    <asp:RangeValidator ID="rvWarrantyKM1" runat="server" ControlToValidate="txtWarrantyKm1"
                        ErrorMessage="Number of warranty KM must be input a numberic format!" MaximumValue="9"
                        meta:resourcekey="rvCus_PhoneNumberResource1" MinimumValue="0" Text="*" ValidationGroup="SearchCondition"></asp:RangeValidator>
                </td>
                <td valign="middle">
                    <asp:Localize ID="loKmWarranty2" runat="server" meta:resourcekey="litTodateResource1"
                        Text="to:"></asp:Localize>&nbsp;
                    <asp:TextBox ID="txtWarrantyKm2" runat="server" MaxLength="15" meta:resourcekey="txtOrderNo2Resource1"
                        Width="130px"></asp:TextBox>
                    <asp:RangeValidator ID="rvWarrantyKM2" runat="server" ControlToValidate="txtWarrantyKm2"
                        ErrorMessage="Number of warranty KM must be input a numberic format!" MaximumValue="9"
                        meta:resourcekey="rvCus_PhoneNumberResource1" MinimumValue="0" Text="*" ValidationGroup="SearchCondition"></asp:RangeValidator>
                    (km)
                </td>
            </tr>
            <tr>
                <td valign="middle" style="width: 145px">
                    &nbsp;<asp:Localize ID="loPartNameEN" runat="server" meta:resourcekey="litDealerResource1"
                        Text="Part name(English):"></asp:Localize>
                </td>
                <td colspan="2" valign="top">
                    <asp:TextBox ID="txtPartNameEng" runat="server" meta:resourcekey="txtDealerResource1"
                        Width="130px" MaxLength="255"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="middle" style="width: 145px">
                    &nbsp;<asp:Localize ID="loPartNameVN" runat="server" meta:resourcekey="litAreaResource1"
                        Text="Part name(Vietnamese):"></asp:Localize>
                </td>
                <td colspan="2" valign="top">
                    <asp:TextBox ID="txtPartNameVie" runat="server" meta:resourcekey="txtDealerResource1"
                        Width="130px" MaxLength="255"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="middle" style="width: 145px">
                </td>
                <td colspan="2" valign="top">
                    <asp:Button ID="btnAddNew" runat="server" Text="Add New" OnClick="btnAddNew_Click"
                        meta:resourcekey="btnAddNewResource1" />
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                        meta:resourcekey="btnCheckResource1" ValidationGroup="SearchCondition" />
                    
                </td>
            </tr>
        </table>
    </asp:Panel>

    <script type="text/javascript" language="javascript">
        var KmWar = "";
        function CheckNum(sName) {
            if (IsNumeric(sName.value)) {
                //alert(sName.name); 
                KmWar = sName.value;
            } else sName.value = KmWar;

        }
        function IsNumeric(sText) {
            var ValidChars = "0123456789.,";
            var IsNumber = true;
            var Char;

            for (i = 0; (i < sText.length) && (IsNumber == true); i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) < 0) {
                    IsNumber = false;
                }
            }
            return IsNumber;
        }
        function DeleteMsg() {
            return confirm('<%=DeleteAlert %>');
        }
    </script>

    <asp:MultiView ID="ControlView" runat="server">
        <asp:View ID="vgridSearch" runat="server">
            <div class="grid">
                <vdms:PageGridView ID="gvItems" runat="server" AutoGenerateColumns="False" meta:resourcekey="GridView1Resource1"
                    OnRowEditing="gvItems_RowEditing" Width="100%" AllowPaging="True" Caption="List part"
                    OnPageIndexChanging="gvItems_PageIndexChanging" OnRowDeleting="gvItems_RowDeleting"
                    CssClass="GridView" OnDataBound="gvItems_DataBound" PageSize="50" OnRowDataBound="gvItems_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="STT">
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("ID") %>' Visible="False"></asp:Label>
                                <asp:Literal ID="Literal2" runat="server" Text='<%# ReturnIndex(DataBinder.Eval(Container, "RowIndex").ToString()) %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Part code" meta:resourcekey="TemplateFieldResource1">
                            <ItemTemplate>
                                <asp:Label ID="lblWARRANTYCONTIONID" runat="server" Text='<%# Bind("PARTCODE") %>'
                                    ToolTip='<%# Bind("ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Part name (English)" meta:resourcekey="BoundFieldResource2">
                            <ItemTemplate>
                                <asp:Literal ID="Literal3" runat="server" Text='<%# Bind("PARTNAMEEN") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Part name (Vietnamese)" meta:resourceKey="BoundFieldResource3">
                            <ItemTemplate>
                                <asp:Literal ID="Literal4" runat="server" Text='<%# Bind("PARTNAMEVN") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Model" meta:resourceKey="BoundFieldResource4" DataField="MOTORCODE" />
                        <asp:TemplateField HeaderText="Warranty (Month)" meta:resourcekey="TemplateFieldResource2">
                            <ItemTemplate>
                                <asp:Literal ID="Literal1" runat="server" Text='<%# Bind("WARRANTYTIME") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Km Warranty" meta:resourceKey="BoundFieldResource6">
                            <ItemTemplate>
                                <asp:Literal ID="Literal5" runat="server" Text='<%# Convert2Currency(DataBinder.Eval(Container, "DataItem.WARRANTYLENGTH").ToString()) %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit price" meta:resourceKey="BoundFieldResource7">
                            <ItemTemplate>
                                <%--<asp:Label ID="Label1" runat="server" Text='<%# Convert2Currency(Bind("UNITPRICE").ToString()) %>'></asp:Label>--%>
                                <asp:Literal ID="Literal6" runat="server" Text='<%# Convert2Currency(DataBinder.Eval(Container, "DataItem.UNITPRICE").ToString()) %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Standard hours (H)" meta:resourceKey="BoundFieldResource8">
                            <ItemTemplate>
                                <asp:Literal ID="Literal7" runat="server" Text='<%# Bind("MANPOWER") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Warranty cost" meta:resourceKey="BoundFieldResource9">
                            <ItemTemplate>
                                <asp:Literal ID="Literal8" runat="server" Text='<%# Convert2Currency(DataBinder.Eval(Container, "DataItem.LABOUR").ToString()) %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" meta:resourceKey="TempFieldAction">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                    ImageUrl="~/Images/Edit.gif" Text="Edit" meta:resourceKey="ImageEdit" />&nbsp;<asp:ImageButton
                                        ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete"
                                        ImageUrl="~/Images/Delete.gif" OnClientClick="return DelMessage();" Text="Delete"
                                        meta:resourceKey="ImageDel" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </vdms:PageGridView>
            </div>
        </asp:View>
        <asp:View ID="vAddnew" runat="server">
            <div class="form">
                <table border="0" cellpadding="2" cellspacing="2" style="width: 100%">
                    <tr>
                        <td colspan="4">
                            <asp:ValidationSummary ID="validADDnewOrInsert" CssClass="error" runat="server" DisplayMode="List"
                                ValidationGroup="NewOrUpdate" meta:resourcekey="validADDnewOrInsertResource1" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%">
                            <asp:Image ID="image2" runat="server" SkinID="RequireField" meta:resourcekey="image2Resource1" />
                            <asp:Localize ID="loPartcode" runat="server" Text="Part code:" meta:resourcekey="loPartcodeResource1"></asp:Localize>
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtPartcode" runat="server" meta:resourcekey="txtPartcodeResource1"
                                MaxLength="35" Style="text-transform: uppercase"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPartCode" runat="server" ControlToValidate="txtPartcode"
                                ErrorMessage="Part code can not be blank!" ValidationGroup="NewOrUpdate" meta:resourcekey="rfvPartCodeResource1">*</asp:RequiredFieldValidator>
                        </td>
                        <td style="width: 25%">
                            <asp:Image ID="Image1" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image1Resource1" />
                            <asp:Localize ID="loModel" runat="server" Text="Model:" meta:resourcekey="loModelResource1"></asp:Localize>
                        </td>
                        <td>
                            <asp:TextBox ID="txtModel" runat="server" meta:resourcekey="txtModelResource1" MaxLength="12"
                                Style="text-transform: uppercase"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%" nowrap="nowrap">
                            <asp:Image ID="Image5" runat="server" SkinID="RequireField" meta:resourcekey="Image5Resource1" />
                            <asp:Localize ID="loAddPartNameEN" runat="server" Text="Part name (English):" meta:resourcekey="loAddPartNameENResource1"></asp:Localize>
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtPartNameEN" runat="server" meta:resourcekey="txtPartNameENResource1"
                                MaxLength="255"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPartNameEn" runat="server" ControlToValidate="txtPartNameEN"
                                ErrorMessage="Part name english can not be blank!" ValidationGroup="NewOrUpdate"
                                meta:resourcekey="rfvPartNameEnResource1">*</asp:RequiredFieldValidator>
                        </td>
                        <td style="width: 25%" nowrap="nowrap">
                            <asp:Image ID="Image3" runat="server" SkinID="RequireField" meta:resourcekey="Image3Resource1" />
                            <asp:Localize ID="loAddPartNameVN" runat="server" Text="Part name (Vietnamese):"
                                meta:resourcekey="loAddPartNameVNResource1"></asp:Localize>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPartNameVN" runat="server" meta:resourcekey="txtPartNameVNResource1"
                                MaxLength="255"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPartNameVn" runat="server" ControlToValidate="txtPartNameVN"
                                ErrorMessage="Part name vietnamese can not be blank!" ValidationGroup="NewOrUpdate"
                                meta:resourcekey="rfvPartNameVnResource1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%" nowrap="nowrap">
                            <asp:Image ID="Image6" runat="server" SkinID="RequireField" meta:resourcekey="Image6Resource1" />
                            <asp:Localize ID="loWarrantyMonth" runat="server" Text="Warranty (Month):" meta:resourcekey="loWarrantyMonthResource1"></asp:Localize>
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtWarrantyMonth" runat="server" meta:resourcekey="txtWarrantyMonthResource1"
                                MaxLength="3"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvWarrantyMonth" runat="server" ControlToValidate="txtWarrantyMonth"
                                ErrorMessage="Warranty duration can not be blank!" ValidationGroup="NewOrUpdate"
                                meta:resourcekey="rfvWarrantyMonthResource1">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="reWarrantyMonthNumber" runat="server" ControlToValidate="txtWarrantyMonth"
                                ErrorMessage="Thời hạn bảo hành (tháng) phải được gõ bằng số lớn hơn 0!" meta:resourcekey="rvCus_PhoneNumberResource1"
                                Text="*" ValidationExpression="\s*\d*[1-9]\d*\s*" ValidationGroup="NewOrUpdate"></asp:RegularExpressionValidator>
                        </td>
                        <td style="width: 25%">
                            <asp:Image ID="Image4" runat="server" SkinID="RequireField" meta:resourcekey="Image4Resource1" />
                            <asp:Localize ID="loKMWarranty" runat="server" Text="Km Warranty:" meta:resourcekey="loKMWarrantyResource1"></asp:Localize>
                        </td>
                        <td nowrap="nowrap">
                            <asp:TextBox ID="txtKMWarranty" runat="server" meta:resourcekey="txtKMWarrantyResource1"
                                MaxLength="6"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvkmWarrantyAN" runat="server" ControlToValidate="txtKMWarranty"
                                ErrorMessage="Km warranty can not be blank!" ValidationGroup="NewOrUpdate" meta:resourcekey="rfvkmWarrantyANResource1"
                                Text="*"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="reKMWarranty"
                                    runat="server" ControlToValidate="txtKMWarranty" ErrorMessage="Number of warranty Km must be input a number greater than 0!"
                                    meta:resourcekey="RangeValidator1" Text="*" ValidationExpression="\s*\d*[1-9]\d*\s*"
                                    ValidationGroup="NewOrUpdate"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%">
                            <asp:Image ID="Image7" runat="server" SkinID="RequireField" meta:resourcekey="Image7Resource1" />
                            <asp:Localize ID="Localize5" runat="server" Text="Unit price:" meta:resourcekey="Localize5Resource1"></asp:Localize>
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtUnitPrice" runat="server" meta:resourcekey="txtUnitPriceResource1"
                                MaxLength="15"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvUnitPrice" runat="server" ControlToValidate="txtUnitPrice"
                                ErrorMessage="Unit price can not be blank!" meta:resourcekey="rfvUnitPrice" Text="*"
                                ValidationGroup="NewOrUpdate"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                    ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtUnitPrice"
                                    ErrorMessage="Unit price must be input a number greater than 0!" meta:resourcekey="RangeValidator3"
                                    Text="*" ValidationExpression="\s*\d*[1-9]\d*\s*" ValidationGroup="NewOrUpdate"></asp:RegularExpressionValidator><asp:Localize
                                        ID="Localize2" runat="server" meta:resourcekey="VNDCurrency" Text="(VND)"></asp:Localize>
                        </td>
                        <td style="width: 25%">
                            <asp:Image ID="Image8" runat="server" SkinID="RequireField" meta:resourcekey="Image8Resource1" />
                            <asp:Localize ID="loStandardHours" runat="server" Text="Standard hours (H):" meta:resourcekey="loStandardHoursResource1"></asp:Localize>
                        </td>
                        <td>
                            <asp:TextBox ID="txtStandartHours" runat="server" meta:resourcekey="txtStandartHoursResource1"
                                onpropertychange="javascript:CheckNum(this);" MaxLength="10"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvStandartHours" runat="server" ControlToValidate="txtStandartHours"
                                ErrorMessage="Standard Hours duration can not be blank!" meta:resourcekey="rfvStandartHours"
                                Text="*" ValidationGroup="NewOrUpdate"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ControlToValidate="txtStandartHours" meta:resourcekey="RangeValidator2"
                                ID="rgvStdHours" runat="server" ErrorMessage="RegularExpressionValidator" ValidationExpression=""
                                ValidationGroup="NewOrUpdate">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%">
                            <asp:Image ID="Image9" runat="server" SkinID="RequireField" meta:resourcekey="Image9Resource1" />
                            <asp:Localize ID="loWarrantyCost" runat="server" Text="Warranty cost:" meta:resourcekey="loWarrantyCostResource1"></asp:Localize>
                        </td>
                        <td style="width: 25%" nowrap="nowrap">
                            <asp:TextBox ID="txtWarrantyCost" runat="server" meta:resourcekey="txtWarrantyCostResource1"
                                MaxLength="15"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvWarrantyCost" runat="server" ControlToValidate="txtWarrantyCost"
                                ErrorMessage="Warranty cost can not be blank!" meta:resourcekey="rfvWarrantyCost"
                                ValidationGroup="NewOrUpdate" Text="*"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                    ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtWarrantyCost"
                                    ErrorMessage="Number of warranty Km must be input a number greater than 0!" meta:resourcekey="RangeValidator4"
                                    Text="*" ValidationExpression="\s*\d*[1-9]\d*\s*" ValidationGroup="NewOrUpdate"></asp:RegularExpressionValidator><asp:Localize
                                        ID="Localize3" runat="server" meta:resourcekey="VNDCurrency" Text="(VND)"></asp:Localize>
                        </td>
                        <td style="width: 25%">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%">
                        </td>
                        <td style="width: 25%">
                        </td>
                        <td style="width: 25%">
                        </td>
                        <td>
                            <asp:Button ID="btnInsertNew" runat="server" Text="Add new" OnClick="btnInsertNew_Click"
                                ValidationGroup="NewOrUpdate" meta:resourcekey="btnInsertNewResource1" /><asp:Button
                                    ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" ValidationGroup="NewOrUpdate"
                                    meta:resourcekey="btnUpdateResource1" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                OnClientClick="history.go(-1)" meta:resourcekey="btnCancelResource1" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
