<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="EditOrder.aspx.cs" Inherits="Vehicle_Inventory_EditOrder" Title="Modify the order"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="VDMS.Common" Namespace="VDMS.Common.Web.CustomControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">

    <script type="text/javascript">
        function updated() {
            //  close the popup
            tb_remove();
            //  refresh the update panel so we can view the changes
            $('#<%= this.btnLoadPayment.ClientID %>').click();
        }

        function showBox(id) {
            var s = "../../Bonus/Dealer/OrderPayment.aspx?";
            s = s + "oid=" + '<%= this.OrderId %>';
            if (id != '0') s = s + "&pid=" + id;
            s = s + "&TB_iframe=true&height=220&width=510";
            document.getElementById('_lnkBox').href = s;
            $('#_lnkBox').click();
        }

        function showCRBox() {
            var s = "../../Bonus/Dealer/CRPayment.aspx?";
            s = s + "oid=" + '<%= this.OrderId %>';
            s = s + "&TB_iframe=true&height=400&width=700";
            document.getElementById('_lnkCRBox').href = s;
            $('#_lnkCRBox').click();
        }

        function showBMBox() {
            var s = "../../Bonus/Dealer/BonusMoney.aspx?";
            s = s + "oid=" + '<%= this.OrderId %>';
            s = s + "&TB_iframe=true&height=400&width=830";
            document.getElementById('_lnkBMBox').href = s;
            $('#_lnkBMBox').click();
        }
    </script>

    <a id='_lnkBox' class="thickbox" href="#"></a><a id='_lnkCRBox' class="thickbox"
        href="#"></a><a id='_lnkBMBox' class="thickbox" href="#"></a>
    <asp:UpdatePanel ID="upPage" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:UpdateProgress ID="uprPage" runat="server" AssociatedUpdatePanelID="upPage"
                DisplayAfter="0" DynamicLayout="False">
                <ProgressTemplate>
                    Loading...
                    <img src="../../Images/Spinner.gif" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:PlaceHolder ID="phInventoryLock" runat="server" Visible="False">
                <asp:Label ID="lblInventoryLock" runat="server" ForeColor="Red" meta:resourcekey="lblInventoryLockResource1"
                    Text="The inventory is locked. You cannot create order."></asp:Label>
                <br />
            </asp:PlaceHolder>
            <asp:ValidationSummary ID="vsAddNewRow" ValidationGroup="AddNewRow" runat="server"
                meta:resourcekey="vsAddNewRowResource1" />
            <div class="form">
                <table cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td align="left" style="width: 120px">
                            <asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
                            <asp:Localize ID="litOrderDate" Text="Order date:" runat="server" meta:resourcekey="litOrderDateResource1"></asp:Localize>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrderDate" runat="server" Width="88px" meta:resourcekey="txtOrderDateResource1"
                                OnTextChanged="txtOrderDate_TextChanged"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvOrderDate" runat="server" ControlToValidate="txtOrderDate"
                                meta:resourcekey="rfvOrderDateResource1" SetFocusOnError="True" Text="*" ValidationGroup="Finish"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvOrderDate" ControlToValidate="txtOrderDate" Text="*" ValidationGroup="Finish"
                                runat="server" ErrorMessage="Order date must be from {0} to {1} !" meta:resourcekey="rvOrderDateResource1"></asp:RangeValidator>
                            <asp:ImageButton ID="ibOrderDate" runat="server" meta:resourcekey="ibFromDateResource1"
                                OnClientClick="return false;" SkinID="CalendarImageButton" />
                            <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtOrderDate"
                                Mask="99/99/9999" MaskType="Date" Enabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                CultureTimePlaceholder="" />
                            <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtOrderDate"
                                PopupButtonID="ibOrderDate" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Image ID="image1" runat="server" meta:resourceKey="imageResource1" SkinID="RequireField" />
                            <asp:Localize ID="litOrderTimes" Text="Times:" runat="server" meta:resourcekey="litOrderTimesResource1"></asp:Localize>
                        </td>
                        <td>
                            <asp:Label ID="lblOrderTimes" runat="server" Width="88px" CssClass="valueField" meta:resourcekey="lblOrderTimesResource1"
                                Height="19px"></asp:Label>
                            <asp:LinkButton ID="lnkUpdateOrderTime" runat="server" Text="Update" ToolTip="Update order times"
                                meta:resourcekey="ImageButton1Resource1" OnClick="lnkUpdateOrderTime_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Image ID="image2" runat="server" meta:resourceKey="imageResource1" SkinID="RequireField" />
                            <asp:Localize ID="litAddress" Text="Delivered Place:" runat="server" meta:resourcekey="litAddressResource1"></asp:Localize>
                        </td>
                        <td>
                            <vdms:DealerList ID="ddltoAddress" runat="server" Width="228px" DataTextField="DealerName"
                                DataValueField="DealerCode" meta:resourcekey="ddltoAddressResource1" OnDataBound="ddltoAddress_DataBound"
                                AutoPostBack="True" OnSelectedIndexChanged="ddltoAddress_SelectedIndexChanged"
                                EnabledSaperateByArea="False" EnabledSaperateByDB="False" MergeCode="False" RemoveRootItem="False"
                                ShowEmptyItem="False" ShowSelectAllItem="False">
                            </vdms:DealerList>
                            &nbsp;<asp:Literal ID="Literal3" runat="server" Text="Bonus available:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                            <asp:Literal ID="litCurBonus" runat="server" meta:resourcekey="litCurBonusResource1"></asp:Literal>
                            <asp:TextBox ID="txtCurBonus" runat="server" CssClass="hidden" ReadOnly="True" meta:resourcekey="txtCurBonusResource1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Image ID="image3" runat="server" SkinID="RequireField" meta:resourcekey="image3Resource1" />
                            <asp:Localize ID="litSecondaryAddress" Text="Secondary Place:" runat="server" meta:resourcekey="litSecondaryAddressResource1"></asp:Localize>
                        </td>
                        <td>
                            <vdms:WarehouseList ID="ddlSecondaryAddress" OnDataBound="ddlSecondaryAddress_DataBound"
                                runat="server" Width="228px" DataTextField="Address" DataValueField="Code" Type="V"
                                DontAutoUseCurrentSealer="False" MergeCode="True" meta:resourcekey="ddlSecondaryAddressResource2"
                                ShowEmptyItem="False" ShowSelectAllItem="False" UseVIdAsValue="False">
                            </vdms:WarehouseList>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Finish"
                meta:resourcekey="ValidationSummary1Resource1" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Save"
                meta:resourcekey="ValidationSummary2Resource1" />
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="Update"
                meta:resourcekey="ValidationSummary2Resource1" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <p />
    <p />
    <div class="grid">
        <asp:UpdatePanel ID="udpMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:UpdateProgress ID="upg1" runat="server" AssociatedUpdatePanelID="udpMain" DisplayAfter="0"
                    DynamicLayout="False">
                    <ProgressTemplate>
                        Loading...
                        <img src="../../Images/Spinner.gif" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:PlaceHolder ID="phOldItemNotOnSale" runat="server" Visible="False">
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="Some items has been ordered not available now: "
                        meta:resourcekey="Label1Resource1" />
                    <asp:Label ID="lblOldItemNotOnSale" runat="server" ForeColor="Red" meta:resourcekey="lblOldItemNotOnSaleResource1" />
                    <br />
                </asp:PlaceHolder>
                <vdms:PageGridView ID="gvMain" runat="server" AutoGenerateColumns="False" CssClass="GridView"
                    ShowFooter="True" AllowPaging="True" OnDataBound="gvMain_DataBound" OnPageIndexChanging="gvMain_PageIndexChanging"
                    meta:resourcekey="gvMainResource1" OnRowDataBound="gvMain_RowDataBound">
                    <Columns>
                        <asp:BoundField HeaderText="No" ReadOnly="True" DataField="Index" meta:resourcekey="BoundFieldResource6">
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Model" meta:resourcekey="TemplateFieldResource5">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlItem" runat="server" DataValueField="Id" DataTextField="Itemname"
                                    OnDataBound="ddlItem_DataBound" DataSourceID="ItemDataSource1" ToolTip='<%# Eval("Item.Id") %>'
                                    OnSelectedIndexChanged="ddlItem_SelectedIndexChanged" AppendDataBoundItems="True"
                                    Visible='<%# Eval("OnSale") %>' OnDataBinding="ddlItem_DataBinding">
                                    <asp:ListItem Text="--+--" meta:resourcekey="ListItemResource8"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:Label ID="lbModel" runat="server" ToolTip='<%# Eval("Item.Id") %>' Text='<%# Eval("ItemDescription") %>'
                                    Visible='<%# Eval("NotOnSale") %>' OnDataBinding="litModel_DataBinding"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Button ID="btnAddRows" runat="server" Text="Add" OnClick="btnAddRows_Click"
                                    ValidationGroup="AddNewRow" meta:resourcekey="btnAddRowsResource1" />
                                &nbsp;<asp:TextBox ID="txtAddedRows" runat="server" Width="30px" MaxLength="2" meta:resourcekey="txtAddedRowsResource1"
                                    Text="5"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender TargetControlID="txtAddedRows" FilterType="Numbers"
                                    ID="FilteredTextBoxExtender1" runat="server" Enabled="True">
                                </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RangeValidator ID="rvAddRows" Text="*" ValidationGroup="AddNewRow" ControlToValidate="txtAddedRows"
                                    MinimumValue="1" MaximumValue="10" runat="server" ErrorMessage="Each time you can add maximum 10 rows!"
                                    meta:resourcekey="rvAddRowsResource1"></asp:RangeValidator>
                                &nbsp;<asp:Literal ID="litAddedItem" runat="server" Text="item(s)" meta:resourcekey="litAddedItemResource1"></asp:Literal>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:BoundField SortExpression="Color" HeaderText="Color" DataField="ItemColor" meta:resourcekey="BoundFieldResource2">
                            <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                        </asp:BoundField>
                        <asp:TemplateField SortExpression="Quantity" HeaderText="Quantity" meta:resourcekey="TemplateFieldResource2">
                            <ItemTemplate>
                                <asp:TextBox ID="txtOrderQty" runat="server" MaxLength="4" Text='<%# EvalQuantity(Eval("OrderQty")) %>'
                                    OnTextChanged="txtOrderQty_TextChanged" ValidationGroup="CheckOrder" meta:resourcekey="txtOrderQtyResource1"
                                    Width="40px"></asp:TextBox>
                                <asp:RangeValidator ID="rvOrderQty" runat="server" ErrorMessage="Quantity must be between 1 and 9999"
                                    ControlToValidate="txtOrderQty" SetFocusOnError="True" MinimumValue="1" MaximumValue="9999"
                                    ValidationGroup="Update" Type="Integer" Text="*" meta:resourcekey="rvOrderQtyResource1"></asp:RangeValidator>
                                <ajaxToolkit:FilteredTextBoxExtender ID="ftbe" runat="server" TargetControlID="txtOrderQty"
                                    FilterType="Numbers" Enabled="True">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Price (VND)" DataField="UnitPrice" DataFormatString="{0:n0}"
                            HtmlEncode="False" ReadOnly="True" meta:resourcekey="BoundFieldResource4">
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Sub total (VND)" meta:resourcekey="BoundFieldResource5">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="litPrice" Text='<%# Eval("Price") %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="Orderpriority" HeaderText="Orderpriority" meta:resourcekey="TemplateFieldResource3">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlOrderPriority" runat="server" DataValueField="Orderpriority"
                                    OnSelectedIndexChanged="txtOrderQty_TextChanged" SelectedValue='<%# Eval("Orderpriority") %>'
                                    meta:resourcekey="ddlOrderPriorityResource1">
                                    <asp:ListItem Text="Urgent" Value="1" meta:resourcekey="ListItemResource1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="General" Value="2" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                    <asp:ListItem Text="Stock" Value="3" meta:resourcekey="ListItemResource3"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False" Visible="False" meta:resourcekey="TemplateFieldResource6">
                            <ItemTemplate>
                                <asp:ImageButton ID="imbDelete" runat="server" CausesValidation="False" ImageUrl="~/Images/Delete.gif"
                                    Text="Delete" Visible='<%# Eval("NotOnSale") %>' CommandArgument='<%# Eval("Index") %>'
                                    OnClick="imbDelete_Click" meta:resourcekey="imbDeleteResource1" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </vdms:PageGridView>
                <asp:TextBox ID="txtTotalOrderQuantity" runat="server" CssClass="hidden" meta:resourcekey="txtTotalOrderQuantityResource1"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
                <%--<asp:AsyncPostBackTrigger ControlID="cmdEdit" EventName="Click" />--%>
                <asp:AsyncPostBackTrigger ControlID="cmdComputePrice" EventName="Click" />
                <%--<asp:AsyncPostBackTrigger ControlID="" EventName="Click" />--%>
            </Triggers>
        </asp:UpdatePanel>
        <vdms:PageGridView ID="gvPreview" runat="server" AutoGenerateColumns="False" ShowFooter="True"
            AllowPaging="True" OnDataBound="gvMain_DataBound" OnPageIndexChanging="gvMain_PageIndexChanging"
            meta:resourcekey="gvPreviewResource1" OnRowDataBound="gvPreview_RowDataBound">
            <Columns>
                <asp:BoundField HeaderText="No" ReadOnly="True" DataField="Index" meta:resourcekey="BoundFieldResource7">
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Model" DataField="ItemDescription" meta:resourcekey="BoundFieldResource8" />
                <asp:BoundField HeaderText="Color" DataField="ItemColor" meta:resourcekey="BoundFieldResource2" />
                <asp:BoundField HeaderText="Quantity" DataField="OrderQty" meta:resourcekey="TemplateFieldResource2">
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle CssClass="right" HorizontalAlign="Right" Font-Bold="True" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Price (VND)" DataField="UnitPrice" DataFormatString="{0:n0}"
                    HtmlEncode="False" ReadOnly="True" meta:resourcekey="BoundFieldResource4">
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle CssClass="right" HorizontalAlign="Right" Font-Bold="True" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Sub total (VND)" meta:resourcekey="BoundFieldResource5">
                    <ItemTemplate>
                        <%--# ((int)Eval("OrderQty") * (long)Eval("UnitPrice")).ToString("n0")--%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Orderpriority" HeaderText="Orderpriority" meta:resourcekey="TemplateFieldResource3">
                    <ItemTemplate>
                        <%# EvalPriorityName(Eval("Orderpriority"))%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </vdms:PageGridView>
    </div>
    <asp:RangeValidator MinimumValue="1" ControlToValidate="txtTotalOrderQuantity" ID="rvTotalQuantity"
        meta:resourcekey="rvTotalQuantityResource1" runat="server" MaximumValue="42000000"
        ErrorMessage="Total order quantity must be greater than zero!" Text="*" ValidationGroup="Finish"
        SetFocusOnError="True" Type="Integer"></asp:RangeValidator>
    <p />
    <p>
        <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Text="Priority:<br/> - Issue if vehicle exsist<br/> - Normal: 2-3 days (arrangement)<br/> - Storage: Not need to issue<br/> - Not issued can be changed"
            meta:resourcekey="lblMsgResource1"></asp:Label>
    </p>
    <asp:UpdatePanel ID="udpPayment" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form">
                <table style="width: 100%">
                    <tr>
                        <td>
                            <%--  <asp:Literal ID="Literal2" runat="server" Text="Payments:" meta:resourcekey="Literal2Resource2"></asp:Literal>--%>
                            <asp:HyperLink ID="lnkAddPayment" runat="server" Text="Add payment" title="Add payment"
                                onclick="javascript:showBox('0'); return false;" href="#" meta:resourcekey="lnkAddPaymentResource1"></asp:HyperLink>
                            <br />
                            <asp:HyperLink ID="HyperLink2" runat="server" Text="Add CR payment" title="Add CR payment"
                                onclick="javascript:showCRBox(); return false;" href="#" meta:resourcekey="HyperLink2Resource1"></asp:HyperLink>
                        </td>
                        <td>
                            <div class="grid">
                                <vdms:PageGridView runat="server" ID="gvPayment" AutoGenerateColumns="False" DataSourceID="odsPays"
                                    DataKeyNames="Id" OnDataBound="gvPayment_DataBound" ShowFooter="True" OnRowCommand="gvPayment_RowCommand"
                                    OnRowDeleted="gvPayment_RowDeleted" meta:resourcekey="gvPaymentResource1">
                                    <Columns>
                                        <asp:TemplateField HeaderText="VoucherNumber" meta:resourcekey="TemplateFieldResource7">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Eval("VoucherNumber") %>'
                                                    href="#" OnDataBinding="HyperLink1_DataBinding" ToolTip='<%# Eval("Id") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PaymentDate" HeaderText="PaymentDate" SortExpression="PaymentDate"
                                            DataFormatString="{0:d}" meta:resourcekey="BoundFieldResource9" />
                                        <asp:BoundField DataField="FromBank" HeaderText="FromBank" SortExpression="FromBank"
                                            meta:resourcekey="BoundFieldResource10" />
                                        <asp:BoundField DataField="FromAccount" HeaderText="FromAccount" SortExpression="FromAccount"
                                            meta:resourcekey="BoundFieldResource11" />
                                        <asp:BoundField DataField="ToBank" HeaderText="ToBank" SortExpression="ToBank" meta:resourcekey="BoundFieldResource12" />
                                        <asp:BoundField DataField="ToAccount" HeaderText="ToAccount" SortExpression="ToAccount"
                                            meta:resourcekey="BoundFieldResource13" />
                                        <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" DataFormatString="{0:C0}"
                                            meta:resourcekey="BoundFieldResource14">
                                            <ItemStyle CssClass="right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Description" HeaderText="Comment" SortExpression="Comment"
                                            meta:resourcekey="BoundFieldResource15" />
                                        <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Images/Delete.gif" ShowDeleteButton="True"
                                            meta:resourcekey="CommandFieldResource1" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <asp:Literal ID="Literal1" runat="server" Text="There's no payments." meta:resourcekey="EmptyDataPaymentResource"></asp:Literal>
                                    </EmptyDataTemplate>
                                    <FooterStyle CssClass="group" />
                                </vdms:PageGridView>
                                <asp:ObjectDataSource ID="odsPays" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="GetEditingPayments" TypeName="VDMS.II.BonusSystem.OrderBonus" DataObjectTypeName="VDMS.I.Entity.SaleOrderPayment"
                                    DeleteMethod="DeleteEditingItem">
                                    <SelectParameters>
                                        <asp:Parameter Name="oid" Type="Int64" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:Button ID="btnLoadPayment" runat="server" Text="Load payment" OnClick="btnLoadPayment_Click"
                                    CssClass="hidden" meta:resourcekey="btnLoadPaymentResource1" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--<asp:Literal ID="Literal1" runat="server" Text="Bonus:" meta:resourcekey="Literal1Resource1"></asp:Literal>--%>
                            <asp:HyperLink ID="HyperLink3" runat="server" Text="Add Bonus" title="Add Bonus"
                                onclick="javascript:showBMBox(); return false;" href="#" meta:resourcekey="HyperLink3Resource1"></asp:HyperLink>
                        </td>
                        <td>
                            <div class="grid">
                                <vdms:PageGridView runat="server" ID="gvBonus" AutoGenerateColumns="False" DataSourceID="odsBonus"
                                    DataKeyNames="Id" ShowFooter="True" OnDataBound="gvBonus_DataBound" OnRowDeleted="gvBonus_RowDeleted">
                                    <Columns>
                                        <asp:BoundField DataField="BonusPlanDetailId" HeaderText="Id" SortExpression="BonusPlanDetailId"
                                            meta:resourcekey="BoundFieldBonusId" />
                                        <asp:BoundField DataField="BonusSourceName" HeaderText="BonusSource" SortExpression="BonusSourceId"
                                            meta:resourcekey="BoundFieldBonusSource" />
                                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"
                                            meta:resourcekey="BoundFieldBonusDesc" />
                                        <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" DataFormatString="{0:C0}" meta:resourcekey="BoundFieldBonusAmount" />
                                        <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Images/Delete.gif" ShowDeleteButton="True"
                                            meta:resourcekey="CommandFieldResource1" />
                                    </Columns>
                                    <FooterStyle CssClass="group" />
                                    <EmptyDataTemplate>
                                        <asp:Literal ID="Literal2" runat="server" Text="There's no bonuses." meta:resourcekey="EmptyDataBonusResource"></asp:Literal>
                                    </EmptyDataTemplate>
                                </vdms:PageGridView>
                            </div>
                            <asp:ObjectDataSource ID="odsBonus" runat="server" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="GetEditingBonuses" TypeName="VDMS.II.BonusSystem.OrderBonus" DeleteMethod="DeleteEditingBonus"
                                DataObjectTypeName="VDMS.Bonus.Entity.BonusTransaction">
                                <SelectParameters>
                                    <asp:Parameter Name="oid" Type="Int64" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="form">
        <table cellpadding="2" cellspacing="2" border="0">
            <tr>
                <td align="left">
                    <asp:Localize ID="litOrderStaus" Text="Order status:" runat="server" meta:resourcekey="litOrderStausResource1"></asp:Localize>
                </td>
                <td>
                    <asp:TextBox ID="txtOrderStaus" runat="server" Width="300px" ReadOnly="True" meta:resourcekey="txtOrderStausResource1"></asp:TextBox>
                </td>
            </tr>
            <asp:MultiView ID="mvComment" runat="server" ActiveViewIndex="0">
                <asp:View ID="view1" runat="server">
                    <tr>
                        <td align="left">
                            <asp:Localize ID="litDealerComment" Text="Dealer comment:" runat="server" meta:resourcekey="litDealerCommentResource1"></asp:Localize>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDealerComment" runat="server" Width="222px" Height="82px" TextMode="MultiLine"
                                meta:resourcekey="txtDealerCommentResource1" MaxLength="255"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="rvCommentLength1" runat="server" ControlToValidate="txtDealerComment"
                                ErrorMessage="RegularExpressionValidator" ValidationExpression="^[\s\S]{0,255}$"
                                ValidationGroup="Finish" meta:resourcekey="rvCommentLength1Resource1">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </asp:View>
                <asp:View ID="view2" runat="server">
                    <tr>
                        <td align="left">
                            <asp:Localize ID="litVmepComment" Text="VMEP comment:" runat="server" meta:resourcekey="litVmepCommentResource1"></asp:Localize>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVmepComment" runat="server" Width="222px" Height="82px" TextMode="MultiLine"
                                meta:resourcekey="txtVmepCommentResource1"></asp:TextBox>
                        </td>
                    </tr>
                </asp:View>
            </asp:MultiView>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="cmdComputePrice" OnClick="cmdComputePrice_Click" runat="server" Text="Compute Price"
                        ValidationGroup="CheckOrder" meta:resourcekey="cmdComputePriceResource1" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <%--<asp:AsyncPostBackTrigger ControlID="" EventName="Click" />--%>
                    <asp:Button ID="cmdFinish" runat="server" Text="Finish" ValidationGroup="Finish"
                        OnClick="cmdFinish_Click" meta:resourcekey="cmdFinishResource1" />
                    <asp:Button ID="cmdEdit" runat="server" Text="Edit" OnClick="cmdEdit_Click" meta:resourcekey="cmdEditResource1" />
                    <asp:Button ID="cmdSave" runat="server" Text="Save" ValidationGroup="Save" OnClick="cmdSave_Click"
                        meta:resourcekey="cmdSaveResource1" />
                    <asp:Button ID="cmdSend" runat="server" Text="Send" ValidationGroup="Save" OnClick="cmdSend_Click"
                        meta:resourcekey="cmdSendResource1" />
                    <asp:Button ID="cmdDelete" runat="server" Text="Delete" CausesValidation="False"
                        OnClientClick="if (confirm(DeleteData)==false) return false;" OnClick="cmdDelete_Click"
                        meta:resourcekey="cmdDeleteResource1" />
                    <asp:Button ID="cmdClose" runat="server" Text="Back" CausesValidation="False" OnClick="cmdClose_Click"
                        CommandName="Cancel" meta:resourcekey="cmdCloseResource1" />
                    <asp:Button ID="btnPrint" runat="server" Text="Print" UseSubmitBehavior="False" CommandName="Cancel"
                        meta:resourcekey="cmdPrintResource" />
                    <%--# ((int)Eval("OrderQty") * (long)Eval("UnitPrice")).ToString("n0")--%>
                </td>
            </tr>
        </table>
    </div>
    <asp:ObjectDataSource ID="ItemDataSource1" runat="server" SelectMethod="GetListItem"
        TypeName="VDMS.I.ObjectDataSource.ItemDataSource"></asp:ObjectDataSource>
</asp:Content>
