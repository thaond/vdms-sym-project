<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="Datacheck.aspx.cs" Inherits="Sales_Sale_Datacheck" Title="Kiểm tra dữ liệu khách hàng"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <asp:BulletedList ID="bllMessage" runat="server" ForeColor="Red" meta:resourcekey="bllMessageResource1">
        </asp:BulletedList>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Save"
            Width="100%" meta:resourcekey="ValidationSummary1Resource1" />
        <table cellpadding="2" border="0" cellspacing="2">
            <tr>
                <td style="width: 100px;" align="right" valign="top">
                    <asp:Literal ID="Literal1" runat="server" Text="Số động cơ:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td style="width: 100px;" valign="top">
                    <asp:TextBox ID="txtEngineNo" runat="server" Width="180px" meta:resourcekey="txtEngineNoResource1"
                        OnLoad="InitPostbackData"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                            runat="server" SetFocusOnError="True" ValidationGroup="Save" ControlToValidate="txtEngineNo"
                            ErrorMessage='Dữ liệu "Số động cơ" không được để trống' meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                </td>
                <td style="width: 100px;" align="right" valign="top">
                    <asp:Literal Visible="false" ID="Literal3" runat="server" Text="Biển số xe:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                </td>
                <td style="width: 100px;" valign="top">
                    <asp:TextBox Visible="false" ID="txtNumberPlate" runat="server" Width="100px" meta:resourcekey="txtNumberPlateResource1"
                        OnLoad="InitPostbackData"></asp:TextBox>
                </td>
                <td style="width: 86px;" align="right" valign="top">
                    <asp:Literal ID="Literal2" runat="server" Text="Model:" meta:resourcekey="Literal4Resource1xxx"></asp:Literal>
                </td>
                <td style="width: 100px;" valign="top">
                    <asp:DropDownList ID="ddlItem" runat="server" AppendDataBoundItems="True" DataSourceID="ItemDataSource1"
                        DataTextField="ItemType" DataValueField="ItemType" meta:resourcekey="ddlItemResource1"
                        OnDataBound="ddlItem_DataBound">
                        <asp:ListItem Text="-+-"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ObjectDataSource ID="ItemDataSource1" runat="server" SelectMethod="GetListItemType"
                        TypeName="VDMS.I.ObjectDataSource.ItemDataSource"></asp:ObjectDataSource>
                </td>
            </tr>
            <tr>
                <td style="width: 100px;" align="right" valign="top">
                    <asp:Literal ID="Literal5" runat="server" Text="Ngày bán:" meta:resourcekey="Literal5Resource1"></asp:Literal>
                </td>
                <td style="width: 100px;" valign="top" colspan="">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 100px">
                                <asp:TextBox ID="txtFromDate" runat="server" Width="100px" meta:resourcekey="txtFromDateResource1"></asp:TextBox><ajaxToolkit:MaskedEditExtender
                                    ID="MaskedEditExtender2" runat="server" TargetControlID="txtFromDate" Mask="99/99/9999"
                                    MaskType="Date" CultureName="vi-VN" BehaviorID="MaskedEditExtender2" CultureAMPMPlaceholder="AM;PM"
                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                                    CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                                    Enabled="True" />
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtFromDate"
                                    PopupButtonID="ImageButton1" BehaviorID="CalendarExtender3" Enabled="True">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                            <td align="right" style="width: 100px">
                                <asp:ImageButton ID="ImageButton1" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                                    meta:resourcekey="ImageButton1Resource1" />
                            </td>
                            <td style="width: 100px">
                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator9" runat="server" CssClass="Validator"
                                    SetFocusOnError="True" ValidationGroup="Save" ControlToValidate="txtFromDate"
                                    ErrorMessage='Dữ liệu "Bán hàng từ ngày" không được để trống' meta:resourcekey="Requiredfieldvalidator9Resource1">*</asp:RequiredFieldValidator>
                            </td>
                            <td style="width: 100px">
                                <asp:RegularExpressionValidator ID="Regularexpressionvalidator1" runat="server" SetFocusOnError="True"
                                    ValidationGroup="Save" ValidationExpression="^((0[1-9])|(1[0-9])|(2[0-9])|(3[0-1]))/(([1-9])|(0[1-9])|(1[0-2]))/((\d{4}))$"
                                    ControlToValidate="txtFromDate" ErrorMessage='Dữ liệu "Bán hàng từ ngày" không hợp lệ (yêu cầu nhập theo định dạng dd/MM/yyyy)'
                                    meta:resourcekey="Regularexpressionvalidator1Resource1">*</asp:RegularExpressionValidator>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 100px;" align="left" valign="top">
                    <asp:Literal ID="Literal14" runat="server" Text="~" meta:resourcekey="Literal14Resource1"></asp:Literal>
                </td>
                <td style="width: 100px;" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 100px">
                                <asp:TextBox ID="txtToDate" runat="server" Width="100px" meta:resourcekey="txtToDateResource1"></asp:TextBox><ajaxToolkit:MaskedEditExtender
                                    ID="MaskedEditExtender1" runat="server" CultureName="vi-VN" Mask="99/99/9999"
                                    MaskType="Date" TargetControlID="txtToDate" BehaviorID="MaskedEditExtender1"
                                    CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY"
                                    CultureDatePlaceholder="/" CultureDecimalPlaceholder="." CultureThousandsPlaceholder=","
                                    CultureTimePlaceholder=":" Enabled="True">
                                </ajaxToolkit:MaskedEditExtender>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="ImageButton2"
                                    TargetControlID="txtToDate" BehaviorID="CalendarExtender1" Enabled="True">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                            <td style="width: 100px">
                                <asp:ImageButton ID="ImageButton2" runat="Server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                    meta:resourcekey="ImageButton2Resource1" />
                            </td>
                            <td style="width: 100px">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtToDate"
                                    CssClass="Validator" ErrorMessage='Dữ liệu "Bán hàng đến ngày" không được để trống '
                                    SetFocusOnError="True" ValidationGroup="Save" meta:resourcekey="RequiredFieldValidator2Resource1">*</asp:RequiredFieldValidator>
                            </td>
                            <td style="width: 100px">
                                <asp:RegularExpressionValidator ID="Regularexpressionvalidator3" runat="server" ControlToValidate="txtToDate"
                                    ErrorMessage='Dữ liệu "Bán hàng đến ngày" không hợp lệ (yêu cầu nhập theo định dạng dd/MM/yyyy)'
                                    SetFocusOnError="True" ValidationExpression="^((0[1-9])|(1[0-9])|(2[0-9])|(3[0-1]))/(([1-9])|(0[1-9])|(1[0-2]))/((\d{4}))$"
                                    ValidationGroup="Save" meta:resourcekey="Regularexpressionvalidator3Resource1">*</asp:RegularExpressionValidator>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 86px;" align="right" valign="top">
                    <asp:Literal ID="Literal4" runat="server" meta:resourcekey="Literal4Resource1" Text="Số hoá đơn:"></asp:Literal>
                </td>
                <td style="width: 100px;" valign="top">
                    <asp:TextBox ID="txtInvoiceNo" runat="server" Width="100px" meta:resourcekey="txtInvoiceNoResource1"
                        OnLoad="InitPostbackData"></asp:TextBox>
                </td>
            </tr>
            <tr runat="server" id="VMEPView">
                <td style="width: 100px;" align="right" valign="top">
                    <asp:Literal ID="litArea" runat="server" Text="Khu vực:" meta:resourcekey="litAreaResource1"></asp:Literal>
                </td>
                <td style="width: 100px;" valign="top">
                    <asp:DropDownList ID="ddlArea" runat="server" Width="100px" DataSourceID="ObjectDataSourceArea"
                        DataTextField="Display" DataValueField="Value" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged"
                        meta:resourcekey="ddlAreaResource1" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td style="width: 100px;" align="right" valign="top">
                    <asp:Literal ID="Literal7" runat="server" Text="Đại lý:" meta:resourcekey="Literal7Resource1"></asp:Literal>
                </td>
                <td style="width: 100px;" colspan="3" valign="top">
                    <%--<asp:DropDownList ID="ddlDealerCode" runat="server" Width="200px" DataSourceID="ObjectDataSourceBranch"
                        DataTextField="Display" DataValueField="Value" meta:resourcekey="ddlDealerCodeResource1">
                    </asp:DropDownList>--%>
                    <vdms:DealerList ShowSelectAllItem="true" EnabledSaperateByDB="true" RemoveRootItem="true"
                        ID="ddlDealerCode" runat="server" Width="200px" />
                    <asp:TextBox ID="txtDealer" runat="server" Visible="False" meta:resourcekey="txtDealerResource1"
                        Width="100px" OnLoad="InitPostbackData"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 100px;" align="right" valign="top">
                    <asp:Literal ID="Literal8" runat="server" Text="Số CMND:" meta:resourcekey="Literal8Resource1"></asp:Literal>
                </td>
                <td style="width: 100px;" valign="top">
                    <asp:TextBox ID="txtIdentityNo" runat="server" Width="100px" meta:resourcekey="txtIdentityNoResource1"
                        OnLoad="InitPostbackData"></asp:TextBox>
                </td>
                <td style="width: 100px;" align="right" valign="top">
                    <asp:Literal ID="Literal9" runat="server" Text="Họ tên công ty: " meta:resourcekey="Literal9Resource1"></asp:Literal>
                </td>
                <td style="width: 100px;" valign="top">
                    <asp:TextBox ID="txtFullname" runat="server" Width="100px" meta:resourcekey="txtFullnameResource1"
                        OnLoad="InitPostbackData"></asp:TextBox>
                </td>
                <td style="width: 86px;" align="right" valign="top">
                    <asp:Literal ID="Literal15" runat="server" Text="Customer type:" meta:resourcekey="Literal3Resource1a"></asp:Literal>
                </td>
                <td style="width: 100px;" valign="top">
                    <asp:DropDownList ID="ddlCusClass" runat="server" meta:resourcekey="ddlCusClassResource1">
                        <asp:ListItem Value="" meta:resourcekey="ListItemResource0">All</asp:ListItem>
                        <asp:ListItem Value="SL" meta:resourcekey="ListItemResource1">Sale</asp:ListItem>
                        <asp:ListItem Value="SV" meta:resourcekey="ListItemResource2">Service</asp:ListItem>
                        <asp:ListItem Value="WI" meta:resourcekey="ListItemResource3">Warranty infomation</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 100px;" align="right" valign="top">
                    <asp:Literal ID="Literal10" runat="server" Text="Đường:" meta:resourcekey="Literal10Resource1"></asp:Literal>
                </td>
                <td style="width: 100px;" valign="top">
                    <asp:TextBox ID="txtAds" runat="server" meta:resourcekey="txtAdsResource1" Width="100px"
                        OnLoad="InitPostbackData"></asp:TextBox>
                </td>
                <td style="width: 100px;" align="right" valign="top">
                    <asp:Literal ID="Literal11" runat="server" Text="Phường:" meta:resourcekey="Literal11Resource1"></asp:Literal>
                </td>
                <td style="width: 100px;" valign="top">
                    <asp:TextBox ID="txtPrecinct" runat="server" meta:resourcekey="txtPrecinctResource1"
                        Width="100px" OnLoad="InitPostbackData"></asp:TextBox>
                </td>
                <td style="width: 86px;" align="right" valign="top">
                    <asp:Literal ID="Literal12" runat="server" Text="Quận huyện:" meta:resourcekey="Literal12Resource1"></asp:Literal>
                </td>
                <td style="width: 100px;" valign="top">
                    <asp:TextBox ID="txtDistrictId" runat="server" meta:resourcekey="txtDistrictIdResource1"
                        Width="100px" OnLoad="InitPostbackData"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 100px;" align="right" valign="top">
                    <asp:Literal ID="Literal13" runat="server" Text="Tỉnh, thành phố:" meta:resourcekey="Literal13Resource1"></asp:Literal>
                </td>
                <td style="width: 100px;" valign="top" colspan="2">
                    <asp:DropDownList ID="ddlProvince" runat="server" Width="100%" DataSourceID="ObjectDataSourceProvince"
                        DataTextField="Display" DataValueField="Value" meta:resourcekey="ddlProvinceResource1">
                    </asp:DropDownList>
                </td>
                <td style="width: 100px;" valign="top">
                </td>
                <td style="width: 86px;" align="right" valign="top">
                </td>
                <td style="width: 100px;" valign="top">
                </td>
            </tr>
            <tr>
                <td align="right" colspan="1" style="width: 100px" valign="top">
                </td>
                <td align="right" colspan="5" style="width: 100px" valign="top">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" colspan="1" style="width: 100px" valign="top">
                </td>
                <td style="width: 100px;" colspan="5" align="right" valign="top">
                    &nbsp;<asp:Button ID="btnExcel" runat="server" Text="Export to excel" OnClick="btnExcel_Click"
                        Width="140px" meta:resourcekey="btnTestResource1E" />
                    <asp:Button ID="btnTest" runat="server" Text="Kiểm tra lịch sử xe" OnClick="btnTest_Click"
                        Width="140px" meta:resourcekey="btnTestResource1" />
                    <asp:Button ID="btnShowReport" runat="server" OnClick="btnShowReport_Click" Text="In tem thư khách hàng"
                        Width="140px" meta:resourcekey="btnShowReportResource1" />
                </td>
            </tr>
            <tr>
                <td align="right" colspan="1" style="width: 100px" valign="top">
                </td>
                <td align="right" colspan="5" style="width: 100px" valign="top">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <asp:MultiView ID="mtvMain" runat="server" ActiveViewIndex="0">
        <asp:View ID="vGridView" runat="server">
            <div class="grid">
                <vdms:PageGridView ID="grvList" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                    OnDataBound="grvList_DataBound" CssClass="GridView" OnRowDataBound="grvList_RowDataBound"
                    meta:resourcekey="grvListResource1">
                    <Columns>
                        <asp:BoundField HeaderText="STT" meta:resourcekey="BoundFieldResource1" />
                        <asp:BoundField HeaderText="Số động cơ" DataField="Enginenumber" meta:resourcekey="BoundFieldResource2" />
                        <asp:TemplateField HeaderText="Chủng loại xe" meta:resourcekey="TemplateFieldResource1">
                            <ItemTemplate>
                                <asp:Literal ID="ltrItemCode" runat="server" Text='<%# Eval("ItemType") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="M&#224;u" meta:resourcekey="TemplateFieldResource2">
                            <ItemTemplate>
                                <asp:Literal ID="ltrColorCode" runat="server" Text='<%# Eval("ItemColor") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField Visible="false" HeaderText="Biển số" meta:resourcekey="TemplateFieldResource3">
                            <ItemTemplate>
                                <asp:Literal ID="ltrNumberPlate" runat="server" Text='<%# Eval("NumberPlate") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Số CMND" meta:resourcekey="TemplateFieldResource4">
                            <ItemTemplate>
                                <asp:Literal ID="ltrCusId" runat="server" Text='<%# Eval("Identifynumber") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cus type" meta:resourcekey="TemplateFieldResource4a">
                            <ItemTemplate>
                                <asp:Literal ID="ltrCusId" runat="server" Text='<%# EvalCusType((string)Eval("CusClass")) %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Họ t&#234;n" meta:resourcekey="TemplateFieldResource5">
                            <ItemTemplate>
                                <asp:Label ID="ltrFullname" runat="server" Text='<%# Eval("Fullname") %>' ToolTip='<%# Eval("CusClass") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phone" meta:resourcekey="BoundFieldResource2a">
                            <ItemTemplate>
                                <asp:Literal ID="litPhone" runat="server" Text='<%# Eval("AllPhone") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" HeaderText="Mobile" meta:resourcekey="BoundFieldResource2b">
                            <ItemTemplate>
                                <asp:Literal ID="litMobile" runat="server" Text='<%# Eval("Mobile") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gender" meta:resourcekey="TemplateFieldResource5C">
                            <ItemTemplate>
                                <asp:Literal ID="litAddress" runat="server" Text='<%# EvalGender(Eval("Gender")) %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Birthday" meta:resourcekey="TemplateFieldResource5B">
                            <ItemTemplate>
                                <asp:Literal ID="litAddress" runat="server" Text='<%# EvalDate(Eval("Birthdate")) %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Address" meta:resourcekey="TemplateFieldResource5A">
                            <ItemTemplate>
                                <asp:Literal ID="litAddress" runat="server" Text='<%# EvalAddress(Eval("Address"), Eval("Districtid"), Eval("Precinct"), Eval("Provinceid")) %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="M&#227; số đại l&#253;" DataField="Dealercode" meta:resourcekey="BoundFieldResource3" />
                        <asp:TemplateField HeaderText="Dealer name" meta:resourcekey="TemplateFieldResource5D">
                            <ItemTemplate>
                                <asp:Literal ID="litDealerName" runat="server" Text='<%# EvalDealerName(Eval("Dealercode")) %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ng&#224;y b&#225;n" meta:resourcekey="TemplateFieldResource6">
                            <ItemTemplate>
                                <asp:Literal ID="litDateBuy" runat="server" Text='<%# EvalDate(Eval("BuyDate")) %>'></asp:Literal>
                            </ItemTemplate>
                            <FooterTemplate>
                                &nbsp;
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <b>
                            <asp:Localize ID="litEmpty" runat="server" Text="Kh&#244;ng t&#236;m thấy dữ liệu n&#224;o! Bạn c&#243; thể thay đổi điều kiện t&#236;m kiếm. Cảm ơn."
                                meta:resourcekey="litEmptyResource1"></asp:Localize></b>
                    </EmptyDataTemplate>
                </vdms:PageGridView>
            </div>
        </asp:View>
        <asp:View ID="vReportView" runat="server">
            <CR:CrystalReportViewer ID="crvMain" runat="server" AutoDataBind="True" meta:resourcekey="crvMainResource1" />
        </asp:View>
    </asp:MultiView>
    <table cellspacing="0" border="0">
        <tr>
            <td colspan="3">
                <asp:ObjectDataSource ID="InvoiceDataSource1" runat="server" SelectMethod="FindCustomers"
                    TypeName="VDMS.I.Vehicle.CustomerDAO" EnablePaging="True" SelectCountMethod="CountCustomers">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlCusClass" DefaultValue="" Name="cusClass" PropertyName="SelectedValue"
                            Type="String" />
                        <asp:ControlParameter ControlID="txtEngineNo" DefaultValue="" Name="engineNumber"
                            PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="txtNumberPlate" Name="NumberPlate" PropertyName="Text"
                            Type="String" />
                        <asp:ControlParameter ControlID="txtInvoiceNo" Name="InvoiceNumber" PropertyName="Text"
                            Type="String" />
                        <asp:Parameter Name="fromDate" Type="DateTime" />
                        <asp:Parameter Name="toDate" Type="DateTime" />
                        <asp:ControlParameter ControlID="ddlArea" Name="AreaCode" PropertyName="SelectedValue"
                            Type="String" />
                        <asp:ControlParameter ControlID="ddlDealerCode" Name="DealerCode" PropertyName="SelectedValue"
                            Type="String" />
                        <asp:ControlParameter ControlID="txtIdentityNo" Name="IdentifyNumber" PropertyName="Text"
                            Type="String" />
                        <asp:ControlParameter ControlID="txtFullname" Name="Fullname" PropertyName="Text"
                            Type="String" />
                        <asp:ControlParameter ControlID="txtAds" Name="Address" PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="txtPrecinct" Name="Precinct" PropertyName="Text"
                            Type="String" />
                        <asp:ControlParameter ControlID="txtDistrictId" Name="DistrictId" PropertyName="Text"
                            Type="String" />
                        <asp:ControlParameter ControlID="ddlProvince" Name="ProvinceId" PropertyName="SelectedValue"
                            Type="String" />
                        <asp:ControlParameter ControlID="ddlItem" Name="Model" PropertyName="SelectedValue"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="ObjectDataSourceArea" runat="server" SelectMethod="ListArea"
                    TypeName="VDMS.I.ObjectDataSource.AreaHelperDataSource"></asp:ObjectDataSource>
                <asp:ObjectDataSource ID="ObjectDataSourceProvince" runat="server" SelectMethod="ListProvice"
                    TypeName="VDMS.I.ObjectDataSource.AreaHelperDataSource"></asp:ObjectDataSource>
            </td>
        </tr>
    </table>
</asp:Content>
