<%@ Page Title="Query payments" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="QueryPayment.aspx.cs" Inherits="Vehicle_Fin_QueryPayment" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div id="_msg" runat="server" />
    <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Update" runat="server"
        CssClass="error" meta:resourcekey="ValidationSummary1Resource1" />
    <div class="form">
        <table>
            <tr>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="Order number:" 
                        meta:resourcekey="Literal4Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtOrderNumber" runat="server" 
                        meta:resourcekey="txtOrderNumberResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal5" runat="server" Text="Dealer code:" 
                        meta:resourcekey="Literal5Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtDealerCode" runat="server" 
                        meta:resourcekey="txtDealerCodeResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Payment date:" 
                        meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtFrom" runat="server" meta:resourcekey="txtFromResource1"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender PopupButtonID="imbC1" ID="txtFrom_CalendarExtender"
                        runat="server" Enabled="True" TargetControlID="txtFrom">
                    </ajaxToolkit:CalendarExtender>
                    <ajaxToolkit:MaskedEditExtender ID="txtFrom_MaskedEditExtender" runat="server" MaskType="Date"
                        Mask="99/99/9999" TargetControlID="txtFrom" CultureAMPMPlaceholder="" 
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <asp:ImageButton runat="server" SkinID="CalendarImageButton" ID="imbC1" 
                        meta:resourcekey="imbC1Resource1" />
                    ~
                    <asp:TextBox ID="txtTo" runat="server" meta:resourcekey="txtToResource1"></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="txtTo_MaskedEditExtender" runat="server" MaskType="Date"
                        Mask="99/99/9999" TargetControlID="txtTo" CultureAMPMPlaceholder="" 
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender PopupButtonID="imbC2" ID="txtTo_CalendarExtender" runat="server"
                        Enabled="True" TargetControlID="txtTo">
                    </ajaxToolkit:CalendarExtender>
                    <asp:ImageButton runat="server" SkinID="CalendarImageButton" ID="imbC2" 
                        meta:resourcekey="imbC2Resource1" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="Status:" 
                        meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" 
                        meta:resourcekey="ddlStatusResource1">
                        <asp:ListItem Value="" meta:resourcekey="ListItemResource1"></asp:ListItem>
                        <asp:ListItem Value="BC" meta:resourcekey="ListItemResource2">Confirmed</asp:ListItem>
                        <asp:ListItem Value="BI" meta:resourcekey="ListItemResource3">Not confirm</asp:ListItem>
                        <asp:ListItem Value="CR" meta:resourcekey="ListItemResource4">Consign remain</asp:ListItem>
                        <asp:ListItem Value="BP" meta:resourcekey="ListItemResource7">Remaining payment</asp:ListItem>
                        <asp:ListItem Value="RC" meta:resourcekey="ListItemResource8">Remaining payment confirmed</asp:ListItem>
                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" 
                        meta:resourcekey="btnFindResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div id="_error" runat="server" />
    <br />
    <div class="grid">
        <vdms:PageGridView ID="gvPayment" runat="server" AutoGenerateColumns="False" OnDataBound="gvPayment_DataBound"
            OnRowUpdating="gvPayment_RowUpdating" DataKeyNames="OrderPaymentId" 
            meta:resourcekey="gvPaymentResource1">
            <Columns>
                <asp:TemplateField HeaderText="DealerCode" SortExpression="DealerCode" 
                    meta:resourcekey="TemplateFieldResource1">
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" ToolTip='<%# Eval("OrderDealerCode") %>' Text='<%# Eval("OrderDealerCode") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="Label4" runat="server" ToolTip='<%# Eval("OrderDealerCode") %>' Text='<%# Eval("OrderDealerCode") %>'></asp:Label>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PaymentDate" SortExpression="PaymentDate" 
                    meta:resourcekey="TemplateFieldResource2">
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" 
                            Text='<%# Bind("PaymentDate", "{0:d}") %>' meta:resourcekey="Label2Resource1"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPaymentDate" Width="100px" runat="server" 
                            Text='<%# Bind("PaymentDate") %>' meta:resourcekey="txtPaymentDateResource1"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender PopupButtonID="imbC" ID="txtPaymentDate_CalendarExtender"
                            runat="server" Enabled="True" TargetControlID="txtPaymentDate">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="txtPaymentDate_MaskedEditExtender" runat="server"
                            MaskType="Date" Mask="99/99/9999" TargetControlID="txtPaymentDate" 
                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                            CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                        </ajaxToolkit:MaskedEditExtender>
                        <asp:ImageButton runat="server" SkinID="CalendarImageButton" ID="imbC" 
                            meta:resourcekey="imbCResource1" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtPaymentDate"
                            ErrorMessage="Payment date cannot be blank!" ValidationGroup="Update" 
                            meta:resourcekey="RequiredFieldValidator4Resource1">*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rvDate" runat="server" ControlToValidate="txtPaymentDate"
                            ErrorMessage="Invalid Payment date!" Type="Date" ValidationGroup="Update" 
                            OnLoad="rvDate_Load" meta:resourcekey="rvDateResource1">*</asp:RangeValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="VDMS No">
                    <ItemTemplate>
                        <asp:Label ID="Label12" runat="server" ToolTip='<%# Eval("OrderHeaderId") %>' Text='<%# Eval("OrderHeaderId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="OrderNumber" SortExpression="OrderNumber" 
                    meta:resourcekey="TemplateFieldResource3">
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("OrderOrderNumber") %>' 
                            meta:resourcekey="Label5Resource1"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtOrderNo" runat="server" 
                            Text='<%# Bind("OrderOrderNumber") %>' meta:resourcekey="txtOrderNoResource1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtOrderNo"
                            ErrorMessage="Order number cannot be blank!" ValidationGroup="Update" 
                            meta:resourcekey="RequiredFieldValidator2Resource1">*</asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cvOrderNum" runat="server" ControlToValidate="txtOrderNo"
                            ErrorMessage="Invalid Order number!" OnServerValidate="cvOrderNum_ServerValidate"
                            ValidationGroup="Update" meta:resourcekey="cvOrderNumResource1">*</asp:CustomValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Description" HeaderText="Description" 
                    SortExpression="Description" meta:resourcekey="BoundFieldResource1" />
                <asp:TemplateField HeaderText="Bank" SortExpression="ToBank" 
                    meta:resourcekey="TemplateFieldResource4">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("ToBank") %>' 
                            meta:resourcekey="Label1Resource1"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <vdms:BankList ID="ddlBank" runat="server" 
                            SelectedValue='<%# Bind("ToBank") %>' meta:resourcekey="ddlBankResource1" 
                            ShowEmptyItem="False">
                        </vdms:BankList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="VoucherNumber" SortExpression="VoucherNumber" 
                    meta:resourcekey="TemplateFieldResource5">
                    <ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("VoucherNumber") %>' 
                            meta:resourcekey="Label6Resource1"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtTrans" runat="server" Text='<%# Bind("VoucherNumber") %>' 
                            meta:resourcekey="txtTransResource1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtTrans"
                            ErrorMessage="Transaction code cannot be blank!" ValidationGroup="Update" 
                            meta:resourcekey="RequiredFieldValidator5Resource1">*</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" DataFormatString="{0:N0}"
                    ReadOnly="True" meta:resourcekey="BoundFieldResource2">
                    <ItemStyle CssClass="number" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Status" SortExpression="PaymentType" 
                    meta:resourcekey="TemplateFieldResource6">
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" 
                            Text='<%# EvalStatus(Eval("PaymentType")) %>' 
                            meta:resourcekey="Label3Resource1"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlPaymentStatus" runat="server" 
                            SelectedValue='<%# Bind("PaymentType") %>' 
                            meta:resourcekey="ddlPaymentStatusResource1">
                            <asp:ListItem Value="BI" meta:resourcekey="ListItemResource5" Selected="True">Not confirm</asp:ListItem>
                            <asp:ListItem Value="CR" meta:resourcekey="ListItemResource6">Consign remain</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource7">
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" 
                            CommandName="Edit" ImageUrl="~/Images/Edit.gif" Text="Edit" 
                            Visible='<%# "CRBI".Contains((string)Eval("PaymentType")) %>' 
                            meta:resourcekey="ImageButton1Resource1" Height="16px" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" 
                            CommandName="Update" ImageUrl="~/Images/update.gif" Text="Update" 
                            meta:resourcekey="ImageButton1Resource2" />
                        &nbsp;<asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" 
                            CommandName="Cancel" ImageUrl="~/Images/cancel.gif" Text="Cancel" 
                            meta:resourcekey="ImageButton2Resource1" />
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
        </vdms:PageGridView>
        <asp:ObjectDataSource ID="odsIP" EnablePaging="True" runat="server"
            SelectCountMethod="CountBankPayments" SelectMethod="QueryBankPayments" 
            TypeName="VDMS.I.Vehicle.PaymentManager"   UpdateMethod="UpdatePayment">
            <UpdateParameters>
                <asp:Parameter Name="OrderPaymentId" Type="Int64" />
                <asp:Parameter Name="PaymentDate" Type="DateTime" />
                <asp:Parameter Name="OrderOrderNumber" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
                <asp:Parameter Name="ToBank" Type="String" />
                <asp:Parameter Name="VoucherNumber" Type="String" />
                <asp:Parameter Name="PaymentType" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="txtOrderNumber" Name="orderNum" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtDealerCode" Name="dCode" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtFrom" Name="from" PropertyName="Text" Type="DateTime" />
                <asp:ControlParameter ControlID="txtTo" Name="to" PropertyName="Text" Type="DateTime" />
                <asp:ControlParameter ControlID="ddlStatus" Name="status" PropertyName="SelectedValue"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    <br />
    <div class="form">
        <asp:Button ID="btnExport" runat="server" Text="Export excel" 
            OnClick="btnExport_Click" meta:resourcekey="btnExportResource1" />
    </div>
</asp:Content>
