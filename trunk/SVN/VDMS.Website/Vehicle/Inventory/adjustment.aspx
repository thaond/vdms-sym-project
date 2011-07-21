<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="adjustment.aspx.cs" Inherits="Sales_Inventory_adjustment" Title="Thao tác điều chỉnh xuất nhập kho xe nguyên chiếc"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Save"
        meta:resourcekey="ValidationSummary1Resource1" CssClass="errorMsg"/>
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Check"
        meta:resourcekey="ValidationSummary2Resource1" />
    <asp:BulletedList ID="bllErrorMsg" runat="server" CssClass="errorMsg">
    </asp:BulletedList>
    <div class="form">
        <table border="0" cellpadding="2" cellspacing="0" width="100%">
            <tr>
                <td style="width: 20%; text-align: right">
                    <asp:Literal ID="Literal2" runat="server" Text="Task:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="drlTask" runat="server" Width="150px" meta:resourcekey="drlTaskResource1"
                        AutoPostBack="True" OnSelectedIndexChanged="drlTask_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 20%; text-align: right">
                    <asp:Image ID="image" runat="server" SkinID="RequireField" />
                    &nbsp;<asp:Literal ID="Literal3" runat="server" Text="Engine number:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtEngineNo" runat="server" CssClass="inputKeyField" Width="144px"
                        meta:resourcekey="txtEngineNoResource1" MaxLength="50"></asp:TextBox>&nbsp;
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True"
                        ValidationGroup="Check" ControlToValidate="txtEngineNo" ErrorMessage='Dữ liệu "Số máy" không được để trống'
                        CssClass="lblClass" meta:resourcekey="RequiredFieldValidator2Resource1">*</asp:RequiredFieldValidator>
                    <asp:Button ID="btnTest" runat="server" Text="View" ValidationGroup="Check" OnClick="btnTest_Click"
                        meta:resourcekey="btnTestResource1" />
                    <asp:HiddenField ID="hdEnNum" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <asp:Panel CssClass="form" ID="pnDetail" runat="server">
        <table border="0" cellpadding="2" cellspacing="2" style="width: 100%">
            <tr>
                <td align="right" style="width: 20%; text-align: right">
                    <asp:Literal ID="Literal4" runat="server" Text="Model:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                </td>
                <td class="valueField" style="width: 18%">
                    <asp:Literal ID="litModel" runat="server" meta:resourcekey="litModelResource1"></asp:Literal>
                </td>
                <td style="width: 20%; text-align: right">
                    <asp:Literal ID="Literal16" runat="server" Text="Status:" meta:resourcekey="Literal16Resource1"></asp:Literal>
                </td>
                <td class="valueField">
                    <asp:Literal ID="litStatus" runat="server" meta:resourcekey="litStatusResource1"></asp:Literal>
                </td>
                <td style="width: 4%">
                    &nbsp; &nbsp; &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 20%; text-align: right">
                    <asp:Literal ID="Literal5" runat="server" Text="Color:" meta:resourcekey="Literal5Resource1"></asp:Literal>
                </td>
                <td class="valueField" style="width: 18%">
                    <asp:Literal ID="litColor" runat="server" meta:resourcekey="litColorResource1"></asp:Literal>
                </td>
                <td style="width: 20%; text-align: right">
                    <asp:Literal ID="Literal19" runat="server" Text="Voucher:" meta:resourcekey="Literal19Resource1"></asp:Literal>
                </td>
                <td class="valueField">
                    <asp:Literal ID="litVoucher" runat="server" meta:resourcekey="litVoucherResource1"></asp:Literal>
                </td>
                <td style="width: 4%">
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 20%; text-align: right">
                    <asp:Literal ID="Literal6" runat="server" Text="Made date:" meta:resourcekey="Literal6Resource1"></asp:Literal>
                </td>
                <td class="valueField" style="width: 18%">
                    <asp:Literal ID="litMadeDate" runat="server" meta:resourcekey="litMadeDateResource1"></asp:Literal>
                </td>
                <td style="width: 20%; text-align: right">
                    <asp:Literal ID="Literal9" runat="server" Text="Gi&#225; xuất xưởng:" meta:resourcekey="Literal9Resource1"></asp:Literal>
                </td>
                <td class="valueField">
                    <asp:Literal ID="litPrize" runat="server" meta:resourcekey="litPrizeResource1"></asp:Literal>
                    (VND)
                </td>
                <td style="width: 4%">
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 20%; text-align: right">
                    <asp:Literal ID="Literal10" runat="server" Text="Invoice number:" meta:resourcekey="Literal10Resource1"></asp:Literal>
                </td>
                <td class="valueField" style="width: 18%">
                    <asp:Literal ID="litInvoiceNum" runat="server" meta:resourcekey="litInvoiceNumResource1"></asp:Literal>
                </td>
                <td style="width: 20%; text-align: right">
                    <asp:Literal ID="Literal20" runat="server" Text="Import stock: " meta:resourcekey="Literal20Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:WarehouseList Type="V" ID="drlToBranch" runat="server" Width="100%" ShowEmptyItem="true"
                        OnDataBound="drlBranch_DataBinding">
                    </vdms:WarehouseList>
                </td>
                <td style="width: 4%">
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="drlFromBranch"
                        ControlToValidate="drlToBranch" ErrorMessage="Nơi nhập xe v&#224; nơi xuất xe phải kh&#225;c nhau"
                        Operator="NotEqual" SetFocusOnError="True" ValidationGroup="Save" meta:resourcekey="CompareValidator1Resource1">*</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 20%; text-align: right">
                    <asp:Literal ID="Literal7" runat="server" Text="Shift date:" meta:resourcekey="Literal7Resource1"></asp:Literal>
                </td>
                <td style="width: 18%">
                    <asp:TextBox ID="txtMoveDate" runat="server" Width="100px" meta:resourcekey="txtMoveDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="imgCalendar" runat="server" OnClientClick="return false;" SkinID="CalendarImageButton"
                        meta:resourcekey="imgCalendarResource1" />
                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator9" runat="server" ControlToValidate="txtMoveDate"
                        ErrorMessage='Dữ liệu "Ngày nhập xe" không được để trống' meta:resourceKey="Requiredfieldvalidator9Resource1"
                        SetFocusOnError="True" Text="*" ValidationGroup="Save">
                    </asp:RequiredFieldValidator><ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2"
                        runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtMoveDate"
                        BehaviorID="MaskedEditExtender2" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgCalendar"
                        TargetControlID="txtMoveDate" BehaviorID="CalendarExtender2" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td style="width: 20%; text-align: right">
                    <asp:Literal ID="Literal21" runat="server" Text="Export stock:  " meta:resourcekey="Literal21Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:WarehouseList Type="V" ShowEmptyItem="true" ID="drlFromBranch" runat="server" Width="100%"
                        OnDataBound="drlBranch_DataBinding">
                    </vdms:WarehouseList>
                </td>
                <td style="width: 4%">
                </td>
            </tr>
            <tr>
                <td align="right" colspan="4">
                    &nbsp;<asp:Button ID="btnApply" runat="server" Text="Save" OnClick="btnApply_Click"
                        ValidationGroup="Save" meta:resourcekey="btnApplyResource1" />
                </td>
                <td align="right" colspan="1" style="width: 4%">
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
