<%@ Page Language="C#" MasterPageFile="~/MP/Mobile.master" AutoEventWireup="true"
    CodeFile="InventoryDetail.aspx.cs" Inherits="MSales_Inventory_Detail" Title="Kiểm tra chi tiết tồn kho"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" Theme="Mobile" %>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Check"
            meta:resourcekey="ValidationSummary1Resource1" />
        <table style="width: 100%;">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" meta:resourcekey="LabelENResource1" Text="Mã chủng loại xe:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEnginenumber" runat="server" meta:resourcekey="txtTypeResource1"
                        Style="text-transform: uppercase" ></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" meta:resourcekey="Label2Resource1" Text="Mã chủng loại xe:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtModelCode" runat="server" meta:resourcekey="txtTypeResource1"
                        Style="text-transform: uppercase"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" meta:resourcekey="Label3Resource1" Text="Mã màu sắc:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtColorCode" runat="server" meta:resourcekey="txtColourResource1"
                        Style="text-transform: uppercase"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="Label6" runat="server" meta:resourcekey="Label6Resource1" Text="Chứng từ:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlVouchers" runat="server" Width="126px" meta:resourcekey="ddlVoucherResource1">
                        <asp:ListItem Text="To&#224;n bộ" Value="2" meta:resourcekey="ListItemResource1"></asp:ListItem>
                        <asp:ListItem Text="C&#243; chứng từ" Value="1" meta:resourcekey="ListItemResource2"></asp:ListItem>
                        <asp:ListItem Text="Kh&#244;ng chứng từ" Value="0" meta:resourcekey="ListItemResource3"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" meta:resourcekey="Label4Resource1" Text="Vị trí kho:"></asp:Label>
                </td>
                <td colspan="3">
                    <%--<asp:DropDownList ID="ddlStorePlace" runat="server" Width="100%" meta:resourcekey="ddlStorePositionResource1"
                        DataSourceID="AddressDataSource1" DataTextField="Address" DataValueField="Code"
                        OnDataBound="ddlStorePlace_DataBound">
                    </asp:DropDownList>--%>
                    <vdms:WarehouseList ID="ddlStorePlace" Type="V" runat="server" Width="100%" ShowSelectAllItem="true"></vdms:WarehouseList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" meta:resourcekey="btnCheckResource1" Text="Kiểm tra"
                        OnClick="btnSearch_Click" />
                    <%--<asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Button" meta:resourcekey="btnExcelResource1" />--%>
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <vdms:PageGridView ID="gvMain" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            OnRowDataBound="gvMain_RowDataBound" OnDataBound="gvMain_DataBound"
            DataKeyNames="ItemInstanceId" meta:resourcekey="gvMainResource1" PageSize="20">
            
            <EmptyDataTemplate>
                <b><asp:Localize ID="litEmpty" runat="server" Text="Kh&#244;ng t&#236;m thấy dữ liệu n&#224;o!"
                        meta:resourcekey="litEmptyResource1"></asp:Localize></b>
            </EmptyDataTemplate>
            <Columns>
                <asp:BoundField HeaderText="STT" meta:resourcekey="BoundFieldResource1" />
                <asp:BoundField DataField="Enginenumber" HeaderText="Số m&#225;y" SortExpression="Enginenumber"
                    meta:resourcekey="BoundFieldResource2" />
                <asp:BoundField DataField="Itemtype" HeaderText="M&#227; chủng loại xe" SortExpression="Itemtype"
                    meta:resourcekey="BoundFieldResource3" />
                <asp:BoundField DataField="Color" HeaderText="M&#227; m&#224;u sắc" SortExpression="Color"
                    meta:resourcekey="BoundFieldResource4" />
                <asp:TemplateField HeaderText="Vị tr&#237; kho" meta:resourcekey="TemplateFieldResource1">
                    <ItemTemplate>
                        <asp:Literal ID="litBranchcode" runat="server" Text='<%# EvalBranchcode(Eval("Branchcode")) %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Importeddate" HeaderText="Ng&#224;y nhập xe" SortExpression="Importeddate"
                    HtmlEncode="False" DataFormatString="{0:d}" meta:resourcekey="BoundFieldResource5" />
                <asp:TemplateField HeaderText="Ng&#224;y điều chuyển" meta:resourcekey="TemplateFieldResource2">
                    <ItemTemplate>
                        <asp:Literal ID="litTransDate" runat="server" Text='<%# EvalTransDate(Eval("ItemInstanceId")) %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ng&#224;y xuất xưởng" meta:resourcekey="TemplateFieldResource7">
                    <ItemTemplate>
                        <asp:Literal ID="litMadedate" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
<%--                <asp:TemplateField HeaderText="Chứng từ" meta:resourcekey="TemplateFieldResource3">
                    <ItemTemplate>
                        <asp:CheckBox ID="ckbVoucher" runat="server" Checked='<%#Eval("HasVoucher") %>' Enabled="False" meta:resourcekey="ckbVoucherResource1" />
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Chứng từ">
                    <ItemTemplate>
                        <asp:Literal ID="litVoucher" runat="server" Text='<%#EvalVoucherState(Eval("HasVoucher")) %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField> 
            </Columns>
        </vdms:PageGridView>
        <asp:ObjectDataSource ID="IteminstanceDataSource1" runat="server" EnablePaging="True"
            SelectCountMethod="CountInStock" SelectMethod="SelectInStock" TypeName="VDMS.I.ObjectDataSource.ItemInstanceDataSource">
            <SelectParameters>
                <asp:Parameter Name="maximumRows" Type="Int32" />
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:ControlParameter ControlID="txtEnginenumber" Name="engineNumber" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtModelCode" Name="ModelCode" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtColorCode" Name="ColorCode" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="ddlStorePlace" Name="StorePlace" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="ddlVouchers" Name="VouchersStatus" PropertyName="SelectedValue"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
