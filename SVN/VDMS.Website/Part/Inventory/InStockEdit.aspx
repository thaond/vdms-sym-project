<%@ Page Title="" Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="InStockEdit.aspx.cs" Inherits="Part_Inventory_InStockEdit" meta:resourcekey="PageResource1" %>

<%@ Register Src="OrderInfo.ascx" TagName="OrderInfo" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form" style="width: 40%; float: left;">
        <uc1:OrderInfo ID="OrderInfo1" runat="server" RedirectURLWhenOrderNull="Receive.aspx" />
    </div>
    <div class="help" style="float: right; width: 40%;">
        <ul>
            <%--<li>
				<asp:Localize ID="lh1" runat="server" Text="Do instock receive parts." meta:resourcekey="lh1Resource1"></asp:Localize>
			</li>--%>
            <li>
                <asp:Localize ID="lh2" runat="server" Text="Confirm the Order Quantity of Part that VMEP Sale quoted."
                    meta:resourcekey="lh2Resource1"></asp:Localize></li>
            <li>
                <asp:Localize ID="lh3" runat="server" Text="Quotation Status 'N': No change Order Quantity."
                    meta:resourcekey="lh3Resource1"></asp:Localize>
            </li>
            <li>
                <asp:Localize ID="Localize1" runat="server" Text="Quotation Status 'Y': Changed Order Quantity."
                    meta:resourcekey="Localize1Resource1"></asp:Localize></li>
        </ul>
    </div>
    <div style="clear: both;">
    </div>
    <br />
    <asp:Label ID="lblSaveOk" runat="server" SkinID="MessageOk" Visible="False" Text="The order has been updated successful."
        meta:resourcekey="lblSaveOkResource1"></asp:Label>
    <asp:Label ID="lblSaveError" runat="server" SkinID="MessageError" Visible="False" Text="Maybe,The order have been deleted by VMEP."
        meta:resourcekey="lblSaveErrorResource1"></asp:Label>
    <br />
    <div class="grid">
        <div class="title">
            <asp:Literal ID="Literal1" runat="server" Text="Confirm quotation Part" meta:resourcekey="Literal1Resource1"></asp:Literal>
        </div>
        <vdms:PageGridView DataSourceID="odsPartList" ID="gv1" runat="server" AutoGenerateColumns="false"
            meta:resourcekey="gv1Resource1" OnDataBinding="gv1_DataBinding">
            <Columns>
                <asp:BoundField HeaderText="Line" DataField="Line" meta:resourcekey="BoundFieldResource1" />
                <asp:BoundField HeaderText="PartCode" DataField="PartCode" meta:resourcekey="BoundFieldResource2" />
                <asp:BoundField HeaderText="Part Name" DataField="PartName" meta:resourcekey="BoundFieldResource3" />
                <asp:TemplateField HeaderText="Quantity" meta:resourcekey="TemplateFieldResource1">
                    <ItemTemplate>
                        <asp:TextBox ID="t2" runat="server" Text='<%# Eval("Quantity") %>' CssClass="number"
                            meta:resourcekey="t2Resource1" Width="70"></asp:TextBox>
                        <asp:CompareValidator runat="server" ControlToValidate="t2" ControlToCompare="t3"
                            ValidationGroup="Save" Operator="GreaterThan" Display="Dynamic" meta:resourcekey="compairevalidatorResource1"
                            Type="Integer"></asp:CompareValidator>
                        <asp:TextBox ID="t3" runat="server" Text='<%# Eval("DelivaryQuantity") %>' CssClass="hidden"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="DelivaryQuantity" DataField="DelivaryQuantity" meta:resourcekey="BoundFieldResource4" />
                <asp:BoundField HeaderText="Quantity Status" DataField="Quo_Status" meta:resourcekey="BoundFieldResource5" />
            </Columns>
            <EmptyDataTemplate>
                <asp:Localize ID="Localize1" runat="server" Text="There isn't any rows." meta:resourcekey="Localize1Resource2"></asp:Localize>
            </EmptyDataTemplate>
        </vdms:PageGridView>
    </div>
    <div class="button">
        <asp:Button ID="cmdSave" runat="server" Text="Save" CommandName="Save" OnClick="cmdSave_Click"
            ValidationGroup="Save" meta:resourcekey="cmdSaveResource1" />
        <asp:Button ID="bBack" runat="server" Text="Back" CommandName="Cancel" OnClientClick="javascript:location.href='Receive.aspx'; return false;"
            CausesValidation="False" meta:resourcekey="bBackResource1" />
    </div>
    <asp:ObjectDataSource ID="odsPartList" runat="server" TypeName="VDMS.II.PartManagement.Order.PartOrderDAO"
        SelectMethod="FindAll"></asp:ObjectDataSource>
</asp:Content>
