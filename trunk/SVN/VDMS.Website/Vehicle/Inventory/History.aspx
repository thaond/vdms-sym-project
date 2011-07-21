<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="History.aspx.cs" Inherits="Sales_Inventory_History" Title="Kiểm tra lịch sử xuất nhập kho"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form" style="width: 650px">
        <asp:BulletedList ID="bllMessage" runat="server" ForeColor="Red" meta:resourcekey="bllMessageResource1">
        </asp:BulletedList>
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Save"
            Width="100%" meta:resourcekey="ValidationSummary2Resource1" />
        <table cellpadding="4" cellspacing="2" style="width: 100%;">
            <tr>
                <td>
                    <asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
                    <asp:Literal ID="EngNo" Text="Số máy:" runat="server" meta:resourcekey="EngNoResource1"></asp:Literal>
                </td>
                <td colspan="5">
                    <asp:TextBox ID="txtEngineNo" runat="server" meta:resourcekey="txtEngineNoResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True"
                        ValidationGroup="Save" ControlToValidate="txtEngineNo" ErrorMessage='Dữ liệu "Số máy" không được để trống'
                        CssClass="lblClass" meta:resourcekey="RequiredFieldValidator2Resource1">*</asp:RequiredFieldValidator>
                    <asp:Button ID="btnTest" runat="server" Text="Kiểm tra" OnClick="btnTest_Click" ValidationGroup="Save"
                        meta:resourcekey="btnTestResource1" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="liAddress" runat="server" Text="Địa điểm kho:" meta:resourcekey="liAddressResource1"></asp:Literal>
                </td>
                <td colspan="5">
                    <asp:Label ID="lblAddress" runat="server" SkinID="TextField" meta:resourcekey="lblAddressResource1"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="liType" runat="server" Text="Chủng loại xe:" meta:resourcekey="liTypeResource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblType" SkinID="TextField" runat="server" meta:resourcekey="lblTypeResource1"></asp:Label>
                </td>
                <td>
                    <asp:Literal ID="liImportDate" runat="server" Text="Ngày nhập xe:" meta:resourcekey="liImportDateResource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label SkinID="TextField" ID="lblImportDate" runat="server" meta:resourcekey="lblImportDateResource1"></asp:Label>
                </td>
                <td>
                    <asp:Literal ID="liColour" runat="server" Text="Mã màu:" meta:resourcekey="liColourResource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label SkinID="TextField" ID="lblColour" runat="server" meta:resourcekey="lblColourResource1"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="liMoneyImport" runat="server" Text="Số tiền nhập xe (VND):" meta:resourcekey="liMoneyImportResource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label SkinID="TextField" ID="lblMoneyImport" runat="server" meta:resourcekey="lblMoneyImportResource1"></asp:Label>
                </td>
                <td>
                    <asp:Literal ID="liExportDate" runat="server" Text="Ngày xuất xưởng VMEP:" meta:resourcekey="liExportDateResource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label SkinID="TextField" ID="lblExportDate" runat="server" meta:resourcekey="lblExportDateResource1"></asp:Label>
                </td>
                <td>
                    <asp:Literal ID="liReceiptStatus" runat="server" Text="Tình trạng chứng từ:" meta:resourcekey="liReceiptStatusResource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label SkinID="TextField" ID="lblReceiptStatus" runat="server" meta:resourcekey="lblReceiptStatusResource1"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="liInvoice" runat="server" Text="Hoá đơn VMEP:" meta:resourcekey="liInvoiceResource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label SkinID="TextField" ID="lblInvoice" runat="server" meta:resourcekey="lblInvoiceResource1"></asp:Label>
                </td>
                <td>
                    <asp:Literal ID="liImportExportDate" runat="server" Text="Ngày xuất nhập kho:" meta:resourcekey="liImportExportDateResource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label SkinID="TextField" ID="lblImportExportDate" runat="server" meta:resourcekey="lblImportExportDateResource1"></asp:Label>
                </td>
                <td>
                    <asp:Literal ID="liInventoryStatus" runat="server" Text="Tình trạng tồn kho:" meta:resourcekey="liInventoryStatusResource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label SkinID="TextField" ID="lblInventoryStatus" runat="server" meta:resourcekey="lblInventoryStatusResource1"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="liMoneySale" runat="server" Text="Số tiền bán xe (VND):" meta:resourcekey="liMoneySaleResource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label SkinID="TextField" ID="lblMoneySale" runat="server" meta:resourcekey="lblMoneySaleResource1"></asp:Label>
                </td>
                <td>
                    <asp:Literal ID="liAgencyNo" runat="server" Text="Mã đại lý bán:" meta:resourcekey="liAgencyNoResource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label SkinID="TextField" ID="lblAgencyNo" runat="server" meta:resourcekey="lblAgencyNoResource1"></asp:Label>
                </td>
                <td>
                    <asp:Literal ID="liIdentityNo" runat="server" Text="Số CMND:" meta:resourcekey="liIdentityNoResource1"></asp:Literal>
                </td>
                <td>
                    <asp:Label SkinID="TextField" ID="lblIdentityNo" runat="server" meta:resourcekey="lblIdentityNoResource1"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <vdms:PageGridView ID="grdHistory" runat="server" DataKeyNames="Id" AutoGenerateColumns="False"
            Width="100%" CssClass="GridView" OnRowDataBound="grdHistory_RowDataBound" meta:resourcekey="grdHistoryResource1">
            <Columns>
                <asp:BoundField HeaderText="STT" meta:resourcekey="BoundFieldResource1" />
                <asp:BoundField DataField="Transactiondate" HeaderText="Ng&#224;y xuất kho" HtmlEncode="False"
                    DataFormatString="{0:d}" meta:resourcekey="BoundFieldResource2" />
                <asp:TemplateField HeaderText="Ph&#226;n loại" meta:resourcekey="TemplateFieldResource1">
                    <ItemTemplate>
                        <asp:Literal ID="litTransactionType" runat="server" Text='<%# EvalType(Eval("Transactiontype")) %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Frombranch" HeaderText="Cửa h&#224;ng xuất kho" meta:resourcekey="BoundFieldResource3" />
                <asp:BoundField DataField="Tobranch" HeaderText="Cửa h&#224;ng nhập kho" meta:resourcekey="BoundFieldResource4" />
                <asp:BoundField DataField="Oldengineno" HeaderText="Bỏ số động cơ" meta:resourcekey="BoundFieldResource5" />
                <asp:BoundField DataField="Modifieddate" HeaderText="Ng&#224;y đăng nhập" HtmlEncode="False"
                    DataFormatString="{0:d}" meta:resourcekey="BoundFieldResource6" />
                <asp:BoundField DataField="Modifiedby" HeaderText="Người đăng nhập" meta:resourcekey="BoundFieldResource7" />
            </Columns>
            <HeaderStyle CssClass="form_content_t12" />
        </vdms:PageGridView>
    </div>
</asp:Content>
