<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="ReceiptDetail.aspx.cs" Inherits="Sales_Report_Default3" Title="Biểu chi tiết các khoản cần thu"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="ReportContent" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
            <tr>
                <td colspan="3" nowrap="nowrap">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Search"
                        Width="100%" meta:resourcekey="ValidationSummary1Resource1" />
                    <asp:Label ID="lblErr" runat="server" meta:resourcekey="lblErrResource1"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" nowrap="nowrap">
                    <asp:Literal ID="Literal3" runat="server" Text="Cửa hàng:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                    <%--<asp:DropDownList ID="ddlBranch" runat="server" meta:resourcekey="ddlBranchResource1">
                        </asp:DropDownList>--%>
                    <vdms:WarehouseList ID="ddlBranch" runat="server" Type="V" 
                        ShowSelectAllItem="True" DontAutoUseCurrentSealer="False" MergeCode="True" 
                        meta:resourcekey="ddlBranchResource2" ShowEmptyItem="False" 
                        UseVIdAsValue="False">
                    </vdms:WarehouseList>
                </td>
                <td nowrap="nowrap" valign="middle">
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap" style="width: 46px">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Image ID="Image2" runat="server" meta:resourcekey="Image2Resource1" SkinID="RequireField" />
                            </td>
                            <td nowrap="nowrap" valign="middle">
                                &nbsp;<asp:Literal ID="Literal1" runat="server" Text="Từ ngày:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                                <asp:TextBox ID="txtFromDate" runat="server" meta:resourcekey="txtRecCDateResource1"></asp:TextBox><asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFromDate" ErrorMessage='Dữ liệu "Từ ngày" không được bỏ trống!'
                                    meta:resourcekey="rfvFromdateResource1" SetFocusOnError="True" ValidationGroup="Search"
                                    Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td nowrap="nowrap" valign="middle">
                                <asp:RangeValidator ID="rvFromDate" runat="server" ControlToValidate="txtFromDate"
                                    Display="Dynamic" ErrorMessage="Định dạng ngày không đúng!" SetFocusOnError="True"
                                    Type="Date" ValidationGroup="Search" 
                                    meta:resourcekey="rvFromDateResource1" Text="*"></asp:RangeValidator>
                            </td>
                            <td style="color: #000000">
                                <asp:ImageButton ID="ImageButton1" runat="server" meta:resourcekey="ImageButton5Resource1"
                                    OnClientClick="return false;" SkinID="CalendarImageButton" />
                            </td>
                        </tr>
                    </table>
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server"
                        CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY"
                        CultureDatePlaceholder="/" CultureDecimalPlaceholder="." CultureThousandsPlaceholder=","
                        CultureTimePlaceholder=":" Enabled="True" Mask="99/99/9999" 
                        MaskType="Date" TargetControlID="txtFromDate">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                        Enabled="True" PopupButtonID="ImageButton1" TargetControlID="txtFromDate">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td nowrap="nowrap" style="width: 50px">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Image ID="Image1" runat="server" meta:resourcekey="Image2Resource1" SkinID="RequireField" />
                            </td>
                            <td nowrap="nowrap" valign="middle">
                                &nbsp;<asp:Literal ID="Literal2" runat="server" Text="Đến ngày:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                                <asp:TextBox ID="txtToDate" runat="server" meta:resourcekey="txtRecCDateResource1"></asp:TextBox><asp:RequiredFieldValidator
                                    ID="rfvRecCDate" runat="server" ControlToValidate="txtToDate" ErrorMessage='Dữ liệu "đến ngày" không được bỏ trống!'
                                    meta:resourcekey="rfvRecCDateResource1" SetFocusOnError="True" ValidationGroup="Search"
                                    Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td nowrap="nowrap" valign="middle">
                                <asp:RangeValidator ID="rvToDate" runat="server" ControlToValidate="txtToDate" Display="Dynamic"
                                    ErrorMessage="Định dạng ngày không đúng!" SetFocusOnError="True" Type="Date"
                                    ValidationGroup="Search" meta:resourcekey="rvToDateResource1" Text="*"></asp:RangeValidator>
                            </td>
                            <td style="color: #000000">
                                <asp:ImageButton ID="ImageButton5" runat="server" meta:resourcekey="ImageButton5Resource1"
                                    OnClientClick="return false;" SkinID="CalendarImageButton" />
                            </td>
                        </tr>
                    </table>
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender5" runat="server"
                        CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY"
                        CultureDatePlaceholder="/" CultureDecimalPlaceholder="." CultureThousandsPlaceholder=","
                        CultureTimePlaceholder=":" Enabled="True" Mask="99/99/9999" 
                        MaskType="Date" TargetControlID="txtToDate">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender5" runat="server"
                        Enabled="True" PopupButtonID="ImageButton5" TargetControlID="txtToDate">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td nowrap="nowrap" valign="middle">
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Kiểm tra"
                        ValidationGroup="Search" meta:resourcekey="btnSearchResource1" />
                    <asp:Button ID="btnExcel" runat="server" Text="Export to Excel" 
                        meta:resourcekey="btnExcelResource1" onclick="btnExcel_Click" />
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap" style="width: 46px">
                </td>
                <td nowrap="nowrap" style="width: 50px">
                </td>
                <td nowrap="nowrap">
                </td>
            </tr>
        </table>
        <br />
        <div class="grid">
            <vdms:PageGridView AllowPaging="True" ID="gv" runat="server" AutoGenerateColumns="False"
                PageSize="30" EmptyDataText="No result found." meta:resourcekey="gvResource1"
                DataSourceID="ods">
                <Columns>
                    <asp:TemplateField HeaderText="FullName" meta:resourcekey="TemplateFieldResource1">
                        <ItemTemplate>
                            <asp:Literal ID="Literal4" runat="server" Text='<%# Eval("FullName") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Address" meta:resourcekey="TemplateFieldResource2">
                        <ItemTemplate>
                            <asp:Literal ID="Literal5" runat="server" Text='<%# Eval("Address") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Model" meta:resourcekey="TemplateFieldResource3">
                        <ItemTemplate>
                            <asp:Literal ID="Literal6" runat="server" Text='<%# Eval("Model") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="EngineNo" meta:resourcekey="TemplateFieldResource4">
                        <ItemTemplate>
                            <asp:Literal ID="Literal7" runat="server" Text='<%# Eval("EngineNo") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Color" meta:resourcekey="TemplateFieldResource5">
                        <ItemTemplate>
                            <asp:Literal ID="Literal8" runat="server" Text='<%# Eval("Color") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Price" meta:resourcekey="TemplateFieldResource7">
                        <ItemTemplate>
                            <asp:Literal ID="Literal10" runat="server" Text='<%# Eval("Price") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TraTruoc" meta:resourcekey="TemplateFieldResource8">
                        <ItemTemplate>
                            <asp:Literal ID="Literal11" runat="server" Text='<%# Eval("TraTruoc") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dept" meta:resourcekey="TemplateFieldResource9">
                        <ItemTemplate>
                            <asp:Literal ID="Literal12" runat="server" Text='<%# Eval("Dept") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </vdms:PageGridView>
            <asp:ObjectDataSource ID="ods" runat="server" OldValuesParameterFormatString="original_{0}"
                SelectMethod="ReportSellingDailyDebtOnly" 
                TypeName="VDMS.Data.DAL2.InventoryDao" onselecting="ods_Selecting">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtFromDate" Name="FromDate" PropertyName="Text"
                        Type="DateTime" />
                    <asp:ControlParameter ControlID="txtToDate" Name="ToDate" PropertyName="Text" Type="DateTime" />
                    <asp:Parameter Name="DealerCode" Type="String" />
                    <asp:ControlParameter ControlID="ddlBranch" Name="BranchCode" PropertyName="SelectedValue"
                        Type="String" />
                    <asp:Parameter Name="DatabaseCode" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
