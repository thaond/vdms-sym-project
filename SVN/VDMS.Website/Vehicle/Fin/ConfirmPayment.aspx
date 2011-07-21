<%@ Page Title="Confirm Payment" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="ConfirmPayment.aspx.cs" Inherits="Vehicle_Fin_ConfirmPayment" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
<%--<script language="javascript">
    function updateCount(cb) {
        var curr = $('#<%= litTotalReady.ClientID %>').val();
        var nv = cb.checked ? ++curr : --curr;
        $('#<%= litTotalReady.ClientID %>').val(nv);
    }
</script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div id="_msg" runat="server" />
    <div class="form">
        <table>
            <tr>
                <td>
                    <asp:Literal ID="Literal10" runat="server" Text="Area:" 
                        meta:resourcekey="Literal10Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:AreaList ID="ddlArea" ShowSelectAllItem="True" runat="server" 
                        meta:resourcekey="ddlAreaResource1">
                    </vdms:AreaList>
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
                    <asp:Literal ID="Literal1" runat="server" Text="Order date:" 
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
                    <asp:Literal ID="Literal12" runat="server" Text="Order number:" 
                        meta:resourcekey="Literal12Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtOrderFrom" runat="server" 
                        meta:resourcekey="txtOrderFromResource1"></asp:TextBox>
                    ~
                    <asp:TextBox ID="txtOrderTo" runat="server" 
                        meta:resourcekey="txtOrderToResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal11" runat="server" Text="Status:" 
                        meta:resourcekey="Literal11Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" 
                        meta:resourcekey="ddlStatusResource1">
                        <asp:ListItem Value="" meta:resourcekey="ListItemResource1"></asp:ListItem>
                        <asp:ListItem Value="5" meta:resourcekey="ListItemResource2">Confirmed</asp:ListItem>
                        <asp:ListItem Value="2" meta:resourcekey="ListItemResource3">Not confirm</asp:ListItem>
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
    <asp:ValidationSummary CssClass="error" ID="ValidationSummary1" runat="server" 
            ValidationGroup="Confirm" meta:resourcekey="ValidationSummary1Resource1" 
            EnableViewState="False" />
    <div class="form">
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Literal ID="Literal9" runat="server" Text="Orders selected: " 
                        meta:resourcekey="Literal9Resource1"></asp:Literal>
                    <asp:TextBox ID="litTotalReady" runat="server" Width="50px" ReadOnly="True" 
                        meta:resourcekey="litTotalReadyResource1"></asp:TextBox>
                    <%--<asp:Literal ID="litTotalReady" runat="server" Text=""></asp:Literal>--%>
                    <asp:RangeValidator ID="cpvOrders" runat="server" MaximumValue="1000" 
                        MinimumValue="1" ValidationGroup="Confirm"
                        Type="Integer" ControlToValidate="litTotalReady" ErrorMessage="Invalid Orders selected count!"
                        Text="*" EnableClientScript="False" meta:resourcekey="cpvOrdersResource1"></asp:RangeValidator>
                    <asp:CustomValidator ID="cvOrderInfo" runat="server" ControlToValidate="rblSelector"
                        EnableClientScript="False" ErrorMessage="Some orders require more information!"
                        OnServerValidate="cvOrderInfo_ServerValidate" ValidationGroup="Confirm" 
                        meta:resourcekey="cvOrderInfoResource1">*</asp:CustomValidator>
                </td>
                <td style="text-align: right">
                    <asp:RadioButtonList ID="rblSelector" runat="server" RepeatDirection="Horizontal"
                        AutoPostBack="True" OnSelectedIndexChanged="rblSelector_SelectedIndexChanged"
                        CellPadding="2" RepeatLayout="Flow" 
                        meta:resourcekey="rblSelectorResource1">
                        <asp:ListItem Value="All" meta:resourcekey="ListItemResource4">All</asp:ListItem>
                        <asp:ListItem Value="Fully paid" Selected="True" 
                            meta:resourcekey="ListItemResource5">Fully paid</asp:ListItem>
                        <asp:ListItem Value="Partly paid" meta:resourcekey="ListItemResource6">Partly paid</asp:ListItem>
                        <asp:ListItem Value="Unpaid" meta:resourcekey="ListItemResource7">Unpaid</asp:ListItem>
                        <asp:ListItem Value="None" meta:resourcekey="ListItemResource8">None</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td style="text-align: right">
                    <asp:Button ID="btnConfirmSelected" runat="server" Text="Confirm selected" OnClick="btnConfirmSelected_Click"
                        ValidationGroup="Confirm" meta:resourcekey="btnConfirmSelectedResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <asp:ListView ID="lv" runat="server">
            <LayoutTemplate>
                <table class="datatable">
                    <tr>
                        <th>
                            Item code
                        </th>
                        <th>
                            Item name
                        </th>
                        <th>
                            Color
                        </th>
                        <th>
                            Quanity
                        </th>
                        <th>
                            Unit price
                        </th>
                        <th>
                            Amount
                        </th>
                    </tr>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                    <tr>
                        <td colspan="6">
                            <vdms:DataPager PageSize="30" PageValidationGroup="Confirm" PagedControlID="lv" ID="DataPager1"
                                runat="server">
                            </vdms:DataPager>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class='group<%# EvalWarnClass((long)Eval("PaymentAmount"), (long)Eval("SubTotal"), (long)Eval("BonusAmount")) %>'>
                    <td colspan="6" class="groupHeader">
                        <table style="width: 100%" class="info">
                            <tr>
                                <th class="fieldName">
                                    <asp:Literal ID="Literal4" runat="server" Text="Dealer code:" 
                                        meta:resourcekey="Literal4Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%#Eval("DealerCode")%>
                                </td>
                                <th class="fieldName">
                                    <asp:Literal ID="Literal2" runat="server" Text="Order number:" 
                                        meta:resourcekey="Literal2Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%#Eval("OrderNumber")%>
                                </td>
                                <th class="fieldName">
                                    <asp:Literal ID="Literal8" runat="server" Text="Payment:" 
                                        meta:resourcekey="Literal8Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%#Eval("PaymentAmount", "{0:N0}")%>
                                </td>
                                <th class="fieldName">
                                    <asp:Literal ID="Literal13" runat="server" Text="Total:" 
                                        meta:resourcekey="Literal13Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%#Eval("TotalAmount", "{0:N0}")%>
                                </td>
                                <th class="fieldName">
                                    <asp:Literal ID="Literal3" runat="server" Text="Tiptop voucher:" 
                                        meta:resourcekey="Literal3Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtTipTopVoucher" runat="server" OnTextChanged="UpdateOrder" 
                                        Text='<%# Eval("FinVoucher") %>' meta:resourcekey="txtTipTopVoucherResource1"></asp:TextBox>
                                    <vdms:RequiredOneItemValidator Text="*" ControlToValidate="txtTipTopVoucher"
                                        runat="server" ID="rovConfirm" ValidationGroup="Confirm" ErrorMessage="Tiptop voucher cannot be blank!"
                                        ListControlsToValidate="txtTipTopVoucher" 
                                        ListControlsToRevertValidate="chbConfirm" 
                                        meta:resourcekey="rovConfirmResource1" ValidateEmptyList="False" 
                                        ValidateEmptyText="True"></vdms:RequiredOneItemValidator>
                                </td>
                                <td>
                                    <asp:CheckBox ID="CheckBox1" Enabled="False" Checked='<%# (int)Eval("Status") == 5 %>'
                                        runat="server" meta:resourcekey="CheckBox1Resource1" />
                                    <asp:CheckBox ID="chbConfirm" ToolTip='<%# EvalPayInfo((long)Eval("OrderHeaderId") , EvalPayClass((long)Eval("PaymentAmount"), (long)Eval("SubTotal"), (long)Eval("BonusAmount")), (int)Eval("Status")) %>'
                                        OnCheckedChanged="UpdateOrder" OnDataBinding="chbConfirm_DataBinding" 
                                        Text="Confirm" runat="server" meta:resourcekey="chbConfirmResource1" />
                                </td>
                            </tr>
                            <tr>
                                <th class="fieldName">
                                    <asp:Literal ID="Literal6" runat="server" Text="Order date:" 
                                        meta:resourcekey="Literal6Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%#Eval("OrderDate", "{0:d}")%>
                                </td>
                                <th class="fieldName">
                                    <asp:Literal ID="Literal6x" runat="server" Text="Payment date:" 
                                        meta:resourcekey="Literal6xResource1"></asp:Literal>
                                </th>
                                <td>
                                    <%#Eval("PaymentDate", "{0:d}")%>
                                </td>
                                <th class="fieldName">
                                    <asp:Literal ID="Literal7" runat="server" Text="Bonus:" 
                                        meta:resourcekey="Literal7Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%#Eval("BonusAmount", "{0:N0}")%>
                                </td>
                                <th class="fieldName">
                                    <asp:Literal ID="Literal14" runat="server" Text="Difference:" 
                                        meta:resourcekey="Literal14Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <%#Eval("DiffAmount", "{0:N0}")%>
                                </td>
                                <th class="fieldName">
                                    <asp:Literal ID="Literal15" runat="server" Text="Description:" 
                                        meta:resourcekey="Literal15Resource1"></asp:Literal>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtDesc" runat="server" OnTextChanged="UpdateOrder" 
                                        Text='<%# Eval("FinComment") %>' meta:resourcekey="txtDescResource1"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnConfirmThis" CommandArgument='<%# Eval("OrderNumber") %>' OnDataBinding="btnConfirmThis_DataBinding"
                                        runat="server" Text="Confirm this" 
                                        meta:resourcekey="btnConfirmThisResource1" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <asp:ListView ID="ListView1" runat="server" 
                        DataSource='<%# Eval("OrderDetails") %>'>
                        <LayoutTemplate>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
                                <td>
                                    <%#Eval("ItemCode")%>
                                </td>
                                <td>
                                    <%#Eval("ItemName")%>
                                </td>
                                <td>
                                    <%#Eval("Color")%>
                                </td>
                                <td class="number">
                                    <%#Eval("OrderQty", "{0:N0}")%>
                                </td>
                                <td class="number">
                                    <%# Eval("UnitPrice", "{0:N0}")%>
                                </td>
                                <td class="number">
                                    <%#Eval("Total", "{0:N0}")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsIP" EnablePaging="True" runat="server" SelectCountMethod="CountConfirmOrders"
            SelectMethod="QueryConfirmOrders" TypeName="VDMS.I.Vehicle.PaymentManager" OnSelected="odsIP_Selected"
            OnSelecting="odsIP_Selecting">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtOrderFrom" PropertyName="Text" Name="oFrom" Type="String">
                </asp:ControlParameter>
                <asp:ControlParameter ControlID="txtOrderTo" Name="oTo" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="txtDealerCode" Name="dCode" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="ddlArea" Name="aCode" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="txtFrom" Name="from" PropertyName="Text" Type="DateTime" />
                <asp:ControlParameter ControlID="txtTo" Name="to" PropertyName="Text" Type="DateTime"
                    DefaultValue="" />
                <asp:ControlParameter ControlID="ddlStatus" Name="status" PropertyName="SelectedValue"
                    Type="Int32" />
                <asp:ControlParameter ControlID="rblSelector" Name="pClass" PropertyName="SelectedIndex"
                    Type="Int32" />
                <asp:Parameter Name="key" Type="String" />
                <asp:Parameter Direction="Output" Name="fp" Type="Int32" />
                <asp:Parameter Direction="Output" Name="pp" Type="Int32" />
                <asp:Parameter Direction="Output" Name="up" Type="Int32" />
                <asp:Parameter Direction="Output" Name="ap" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
