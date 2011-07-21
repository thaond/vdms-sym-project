<%@ Page Title="" Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="LotSale.aspx.cs" Inherits="Vehicle_Sale_LotSale" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">

    <script type="text/javascript">
        function update(str) {
            tb_remove();
            __doPostBack('Updt', '');
        }

        function showCustomerInput() {
            window.showModalDialog("CusInfInput.aspx", null, "status:false;dialogWidth:750px;dialogHeight:500px")
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <input type="hidden" id="_key" runat="server" enableviewstate="true" value="salelot_list" />
    <ajaxToolkit:TabContainer ID="t" runat="server" CssClass="ajax__tab_technorati-theme"
        ActiveTabIndex="0" meta:resourcekey="tResource1">
        <ajaxToolkit:TabPanel ID="t1" runat="server" HeaderText="Lot sale to customer" meta:resourcekey="t1Resource1">
            <HeaderTemplate>
                <asp:Literal ID="Literal6" runat="server" Text="Lot sale to customer" meta:resourcekey="Literal6Resource1"></asp:Literal>
            </HeaderTemplate>
            <ContentTemplate>
                <div class="form">
                    <table>
                        <tr>
                            <td colspan="5">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="CheckInsertOrUpdate"
                                    DisplayMode="List" meta:resourcekey="ValidationSummary1Resource1" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="txtMessage" runat="server" Visible="False" meta:resourcekey="txtMessageResource1"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Image ID="Image5" runat="server" SkinID="RequireField" meta:resourcekey="Image2Resource1" />
                                <asp:Literal ID="Literal2" runat="server" Text="Sell date" meta:resourcekey="Literal2Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSellingDate" runat="server" CssClass="readOnlyInputField" meta:resourcekey="txtSellingDateResource1"></asp:TextBox>
                                <asp:RangeValidator ID="rvSellingDate" runat="server" ControlToValidate="txtSellingDate"
                                    SetFocusOnError="True" Type="Date" ValidationGroup="CheckInsertOrUpdate" meta:resourcekey="rvSellingDateResource1"></asp:RangeValidator>
                                <asp:ImageButton ID="imgbSellDate" runat="server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                    meta:resourcekey="imgbSellDateResource1" />
                                <asp:RequiredFieldValidator ID="rfvSellingDate" runat="server" ControlToValidate="txtSellingDate"
                                    ErrorMessage='Input &quot;Selling date&quot; can not be blank!' SetFocusOnError="True"
                                    ValidationGroup="CheckInsertOrUpdate" meta:resourcekey="rfvSellingDateResource1">*</asp:RequiredFieldValidator>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                    MaskType="Date" TargetControlID="txtSellingDate" Enabled="True" CultureAMPMPlaceholder=""
                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="">
                                </ajaxToolkit:MaskedEditExtender>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgbSellDate"
                                    TargetControlID="txtSellingDate" Enabled="True">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                            <td class="space">
                            </td>
                            <td align="right">
                                <asp:Literal ID="Literal3" runat="server" Text="Payment date" meta:resourcekey="Literal3Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRecCDate" runat="server" meta:resourcekey="txtRecCDateResource1"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99/99/9999"
                                    MaskType="Date" TargetControlID="txtRecCDate" Enabled="True" CultureAMPMPlaceholder=""
                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="">
                                </ajaxToolkit:MaskedEditExtender>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="ImageButton5"
                                    TargetControlID="txtRecCDate" Enabled="True">
                                </ajaxToolkit:CalendarExtender>
                                <asp:ImageButton ID="ImageButton5" runat="server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                    meta:resourcekey="ImageButton5Resource1" />
                                <asp:RangeValidator ID="rvRecDate" runat="server" ControlToValidate="txtRecCDate"
                                    ErrorMessage='Dữ liệu "Ngày cần thu" không đúng với định dạng ngày!' SetFocusOnError="True"
                                    Type="Date" ValidationGroup="CheckInsertOrUpdate" meta:resourcekey="rvRecDateResource1">*</asp:RangeValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Literal ID="Literal9" runat="server" Text="Bill No" meta:resourcekey="Literal9Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBillNo" runat="server" MaxLength="30" Style="text-transform: uppercase"
                                    meta:resourcekey="txtBillNoResource1"></asp:TextBox>
                            </td>
                            <td class="space">
                            </td>
                            <td align="right">
                                <asp:Literal ID="Literal12" runat="server" Text="Payment method" meta:resourcekey="Literal12Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPaymentMethod" runat="server" Width="132px" AutoPostBack="True"
                                    meta:resourcekey="ddlPaymentMethodResource1">
                                    <asp:ListItem Selected="True" Value="0" meta:resourcekey="ListItemResource1">Complete paying</asp:ListItem>
                                    <asp:ListItem Value="1" meta:resourcekey="ListItemResource2">Fix hire-purchase</asp:ListItem>
                                    <asp:ListItem Value="2" meta:resourcekey="ListItemResource3">UnFix hire-purchase</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Literal ID="Literal13a" runat="server" Text="Selling type" meta:resourcekey="Literal13aResource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSellingType" runat="server" MaxLength="255" meta:resourcekey="txtSellingTypeResource1"></asp:TextBox>
                            </td>
                            <td class="space">
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Literal ID="Literal15" runat="server" Text="Description" meta:resourcekey="Literal15Resource1"></asp:Literal>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtComment" runat="server" MaxLength="255" Width="90%" meta:resourcekey="txtCommentResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Image ID="Image1" runat="server" SkinID="RequireField" meta:resourcekey="Image2Resource1" />
                                <asp:Literal ID="Literal4" runat="server" Text="Customer" meta:resourcekey="Literal4Resource1"></asp:Literal>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtCustomerName" runat="server" Width="99%" Enabled="False" meta:resourcekey="txtCustomerNameResource1"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <asp:Button ID="btnCreateCustomer" runat="server" Text="&nbsp;+&nbsp;" meta:resourcekey="btnCreateCustomerResource1" />
                                <asp:HyperLink ID="lnkFindCustomer" runat="server" class="thickbox" NavigateUrl="#"
                                    Title="Find customer" meta:resourcekey="lnkFindCustomerResource1">Find customer</asp:HyperLink>
                                <asp:RequiredFieldValidator ID="rvCustomer" runat="server" ControlToValidate="txtCustomerName"
                                    ErrorMessage="Customer required." Text="*" ValidationGroup="CheckInsertOrUpdate"
                                    meta:resourcekey="rvCustomerResource1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
    <div class="form">
        <asp:Literal ID="Literal1" runat="server" meta:resourcekey="Literal1Resource1" Text="Engine number: "></asp:Literal>
        <asp:TextBox ID="txtEngineNo" runat="server" meta:resourcekey="txtEngineNoResource1"></asp:TextBox>
        <asp:Button ID="btnAddVehicle" runat="server" Text="&nbsp;+&nbsp;" OnClick="btnAddVehicle_Click"
            meta:resourcekey="btnAddVehicleResource1" />
        <asp:HyperLink ID="lnkSearchVehicles" runat="server" class="thickbox" title="Search Vehicle"
            meta:resourcekey="lnkSearchVehiclesResource1">Search vehicles</asp:HyperLink>
        <div>
            &nbsp;</div>
        <div class="grid">
            <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="udtPanel">
                <ContentTemplate>
                    <vdms:PageGridView ID="gv" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        EmptyDataText="No vehicle." CssClass="GridView" DataSourceID="odsSessionVehicles"
                        Width="100%" OnSelectedIndexChanging="gv_SelectedIndexChanging" OnDataBound="gv_DataBound"
                        ShowFooter="True" OnDataBinding="gv_DataBinding" meta:resourcekey="gvResource1">
                        <Columns>
                            <asp:BoundField DataField="EngineNumber" HeaderText="Engine no" meta:resourcekey="BoundFieldResource1" />
                            <asp:BoundField DataField="ItemName" HeaderText="Name" meta:resourcekey="BoundFieldResource2" />
                            <asp:BoundField DataField="ColorName" HeaderText="Color" meta:resourcekey="BoundFieldResource3" />
                            <asp:BoundField DataField="Branch" HeaderText="Branch" meta:resourcekey="BoundFieldResource4" />
                            <asp:TemplateField HeaderText="Price (VND)" meta:resourcekey="TemplateFieldResource1">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPriceTax" runat="server" MaxLength="20" AutoPostBack="True" meta:resourcekey="txtPriceTaxResource1"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtPriceTax"
                                        ErrorMessage="Giá tiền phải được gõ bằng số!" ValidationExpression="\s*[0-9]*([.,]?\d*[1-9]+\d*)?\s*"
                                        ValidationGroup="CheckInsertOrUpdate" Text="*" meta:resourcekey="RegularExpressionValidator2Resource1"></asp:RegularExpressionValidator>
                                </ItemTemplate>
                                <FooterStyle Font-Bold="True" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Number Plate" meta:resourcekey="TemplateFieldResource2">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPlateNo" runat="server" AutoPostBack="True" MaxLength="10" Style="text-transform: uppercase"
                                        meta:resourcekey="txtPlateNoResource1"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Plate date" meta:resourcekey="TemplateFieldResource3">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTakPlateNoDate" runat="server" AutoPostBack="True" meta:resourcekey="txtTakPlateNoDateResource1"></asp:TextBox>
                                    <asp:ImageButton ID="ImageButton6" runat="server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                        meta:resourcekey="ImageButton6Resource1" /><a href="#"></a>
                                    <asp:RangeValidator ID="rvTakingPlateDate" runat="server" ControlToValidate="txtTakPlateNoDate"
                                        ErrorMessage='Dữ liệu "Ngày lấy biển" không đúng với định dạng ngày!' SetFocusOnError="True"
                                        Type="Date" ValidationGroup="CheckInsertOrUpdate" MaximumValue='<%# DateTime.MaxValue.ToShortDateString() %>'
                                        MinimumValue='<%# DateTime.MinValue.ToShortDateString() %>' meta:resourcekey="rvTakingPlateDateResource1">*</asp:RangeValidator>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender6" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtTakPlateNoDate" Enabled="True" CultureAMPMPlaceholder=""
                                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="">
                                    </ajaxToolkit:MaskedEditExtender>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender6" runat="server" PopupButtonID="ImageButton6"
                                        TargetControlID="txtTakPlateNoDate" Enabled="True">
                                    </ajaxToolkit:CalendarExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Image" SelectImageUrl="~/Images/Delete.gif" ShowSelectButton="True"
                                meta:resourcekey="CommandFieldResource1" />
                        </Columns>
                    </vdms:PageGridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--<asp:ListView ID="lv" runat="server" DataSourceID="odsSessionVehicles">
                <LayoutTemplate>
                    <table class="datatable">
                        <tr>
                            <th>
                                <asp:Literal ID="Literal1" runat="server" Text="Engine no"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal2" runat="server" Text="Type"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal3" runat="server" Text="Color"></asp:Literal>
                            </th>
                        </tr>
                        <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                        <tr>
                            <td colspan="3">
                                <vdms:DataPager runat="server" ID="pager" PagedControlID="lv" DisablePaging="False">
                                </vdms:DataPager>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>' >
                        <td>
                            <asp:Literal ID="Literal4" runat="server" Text='<%#Eval("EngineNumber") %>' ></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="Literal5" runat="server" Text='<%#Eval("ItemName") %>' ></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="Literal7" runat="server" Text='<%#Eval("ColorName") %>' ></asp:Literal>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>--%>
        </div>
        <div>
            &nbsp;</div>
        <asp:Button ID="btnSell" runat="server" Text="Sell" OnClick="btnSell_Click" CausesValidation="False"
            ValidationGroup="CheckInsertOrUpdate" meta:resourcekey="btnSellResource1" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" meta:resourcekey="btnClearResource1" />
        <asp:ObjectDataSource ID="odsSessionVehicles" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="ListSaleSessionalVehicles" TypeName="VDMS.I.Vehicle.VehicleDAO"
            EnablePaging="True" SelectCountMethod="CountSaleSessionalVehicles">
            <SelectParameters>
                <asp:Parameter Name="key" Type="String" DefaultValue="salelot_list" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
                <asp:Parameter Name="startRowIndex" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
