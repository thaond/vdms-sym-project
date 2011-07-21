<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="SaleDetail_old.aspx.cs" Inherits="Vehicle_Report_SaleDetail" Title="Báo cáo Sale - Chi tiết ngày"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="ReportContent" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <asp:PlaceHolder ID="plDateFromTo" runat="server" EnableViewState="true" Visible="true">
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
                        <vdms:WarehouseList ID="ddlBranch" runat="server" Type="V" ShowSelectAllItem="true"></vdms:WarehouseList>
                    </td>
                    <td nowrap="nowrap" valign="middle">
                    </td>
                </tr>
                <tr>
                    <td nowrap="nowrap" style="width: 46px">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                </td>
                                <td nowrap="nowrap" valign="middle">
                                    <asp:Image ID="Image2" runat="server" meta:resourcekey="Image2Resource1" SkinID="RequireField" />
                                    <asp:Literal ID="Literal1" runat="server" Text="Từ ngày:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                                    <asp:TextBox ID="txtFromDate" runat="server" meta:resourcekey="txtRecCDateResource1"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFromDate" ErrorMessage='Dữ liệu "Từ ngày" không được bỏ trống!'
                                        meta:resourcekey="rfvFromdateResource1" SetFocusOnError="True" ValidationGroup="Search"
                                        Text="*"></asp:RequiredFieldValidator>
                                </td>
                                <td nowrap="nowrap" valign="middle">
                                    <asp:RangeValidator ID="rvFromDate" runat="server" ControlToValidate="txtFromDate"
                                        Display="Dynamic" ErrorMessage="Định dạng ngày không đúng!" SetFocusOnError="True"
                                        Type="Date" ValidationGroup="Search" meta:resourcekey="rvFromDateResource1">*</asp:RangeValidator>
                                </td>
                                <td style="color: #000000">
                                    <asp:ImageButton ID="ImageButton1" runat="server" meta:resourcekey="ImageButton5Resource1"
                                        OnClientClick="return false;" SkinID="CalendarImageButton" />
                                </td>
                            </tr>
                        </table>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" BehaviorID="MaskedEditExtender1"
                            CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY"
                            CultureDatePlaceholder="/" CultureDecimalPlaceholder="." CultureThousandsPlaceholder=","
                            CultureTimePlaceholder=":" Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFromDate">
                        </ajaxToolkit:MaskedEditExtender>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="CalendarExtender1"
                            Enabled="True" PopupButtonID="ImageButton1" TargetControlID="txtFromDate">
                        </ajaxToolkit:CalendarExtender>
                    </td>
                    <td nowrap="nowrap" style="width: 50px">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                </td>
                                <td nowrap="nowrap" valign="middle">
                                    <asp:Image ID="Image1" runat="server" meta:resourcekey="Image2Resource1" SkinID="RequireField" />
                                    <asp:Literal ID="Literal2" runat="server" Text="Đến ngày:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                                    <asp:TextBox ID="txtToDate" runat="server" meta:resourcekey="txtRecCDateResource1"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="rfvFromdate" runat="server" ControlToValidate="txtToDate" ErrorMessage='Dữ liệu "đến ngày" không được bỏ trống!'
                                        meta:resourcekey="rfvRecCDateResource1" SetFocusOnError="True" ValidationGroup="Search"
                                        Text="*"></asp:RequiredFieldValidator>
                                </td>
                                <td nowrap="nowrap" valign="middle">
                                    <asp:RangeValidator ID="rvToDate" runat="server" ControlToValidate="txtToDate" Display="Dynamic"
                                        ErrorMessage="Định dạng ngày không đúng!" SetFocusOnError="True" Type="Date"
                                        ValidationGroup="Search" meta:resourcekey="rvToDateResource1">*</asp:RangeValidator>
                                </td>
                                <td style="color: #000000">
                                    <asp:ImageButton ID="ImageButton5" runat="server" meta:resourcekey="ImageButton5Resource1"
                                        OnClientClick="return false;" SkinID="CalendarImageButton" />
                                </td>
                            </tr>
                        </table>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender5" runat="server" BehaviorID="MaskedEditExtender5"
                            CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY"
                            CultureDatePlaceholder="/" CultureDecimalPlaceholder="." CultureThousandsPlaceholder=","
                            CultureTimePlaceholder=":" Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtToDate">
                        </ajaxToolkit:MaskedEditExtender>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender5" runat="server" BehaviorID="CalendarExtender5"
                            Enabled="True" PopupButtonID="ImageButton5" TargetControlID="txtToDate">
                        </ajaxToolkit:CalendarExtender>
                    </td>
                    <td nowrap="nowrap" valign="middle">
                        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Kiểm tra"
                            ValidationGroup="Search" meta:resourcekey="btnSearchResource1" />
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
        </asp:PlaceHolder>
    </div>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
        BorderColor="Silver" BorderStyle="Groove" HasRefreshButton="True" Height="50px"
        Width="350px" DisplayGroupTree="False" HasCrystalLogo="False" HasSearchButton="False"
        EnableDatabaseLogonPrompt="False" ReuseParameterValuesOnRefresh="True" meta:resourcekey="CrystalReportViewer1Resource1" />
</asp:Content>
