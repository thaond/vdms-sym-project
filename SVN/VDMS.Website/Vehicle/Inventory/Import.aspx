<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="Import.aspx.cs" Inherits="Sales_Inventory_Import" Title="Thao tác nhập xe"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" meta:resourceKey="ValidationSummary1Resource1"
        ValidationGroup="Check" />
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" meta:resourceKey="ValidationSummary1Resource1"
        ValidationGroup="Save" />
    <asp:BulletedList ID="bllError" runat="server" CssClass="errorMsg" meta:resourcekey="bllErrorResource1">
    </asp:BulletedList>
    <br />
    <div class="form" style="width: 50%; float: left;">
        <table border="0" cellpadding="0" cellspacing="2">
            <tr>
                <td>
                    <asp:Image ID="image1" runat="server" meta:resourceKey="image1Resource1" SkinID="RequireField" />
                </td>
                <td>
                    <asp:Localize ID="Localize2" runat="server" meta:resourceKey="Localize2Resource1"
                        Text="Ngày nhập xe:"></asp:Localize>
                </td>
                <td>
                    <asp:TextBox ID="txtImportDate" runat="server" meta:resourceKey="txtImportDateResource1"
                        Width="100px"></asp:TextBox>
                    <asp:ImageButton ID="imgbCarlendar" runat="server" meta:resourceKey="ImageButton1Resource1"
                        OnClientClick="return false;" SkinID="CalendarImageButton" />
                    <asp:RangeValidator ControlToValidate="txtImportDate" ID="rvImportDate" SetFocusOnError="true"
                        meta:resourceKey="rvImportDateResource1" ValidationGroup="Save" runat="server"
                        ErrorMessage="RangeValidator">*</asp:RangeValidator>
                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator9" runat="server" ControlToValidate="txtImportDate"
                        CssClass="Validator" ErrorMessage='Dữ liệu "Ngày nhập xe" không được để trống'
                        meta:resourceKey="Requiredfieldvalidator9Resource1" SetFocusOnError="True" Text="*"
                        ValidationGroup="Save"></asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" BehaviorID="MaskedEditExtender2"
                        Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtImportDate"
                        CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY"
                        CultureDatePlaceholder="/" CultureDecimalPlaceholder="." CultureThousandsPlaceholder=","
                        CultureTimePlaceholder=":">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" BehaviorID="CalendarExtender3"
                        Enabled="True" PopupButtonID="imgbCarlendar" TargetControlID="txtImportDate">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="image" runat="server" meta:resourceKey="imageResource1" SkinID="RequireField" />
                </td>
                <td>
                    <asp:Localize ID="Localize1" runat="server" meta:resourceKey="Localize1Resource1"
                        Text="Số&nbsp;đơn xuất&nbsp;hàng:"></asp:Localize>
                </td>
                <td>
                    <asp:TextBox ID="txtExportInvoice" runat="server" meta:resourceKey="txtExportInvoiceResource1"
                        Width="120px" MaxLength="30" CssClass="inputKeyField"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtExportInvoice"
                        CssClass="Validator" ErrorMessage='Dữ liệu "Số đơn xuất hàng" không được để trống'
                        meta:resourceKey="RequiredFieldValidator1Resource1" SetFocusOnError="True" Text="*"
                        ValidationGroup="Check"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Localize ID="Localize3" runat="server" Text="Order number:" meta:resourceKey="Localize3Resource1"></asp:Localize>
                </td>
                <td>
                    <asp:TextBox ID="txtOrderNumber" runat="server" Width="120px" MaxLength="30" CssClass="inputKeyField"></asp:TextBox>
                </td>
            </tr>
            <%-- Leo mvbinh - edit --%>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="Địa điểm giao xe:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:WarehouseList runat="server" ID="ddlwarehouselist" DontAutoUseCurrentSealer="False"
                        MergeCode="True" meta:resourcekey="ddlFromBranchResource1" ShowEmptyItem="False"
                        ShowSelectAllItem="False" UseVIdAsValue="false" AutoPostBack="true" Type="V"
                        OnSelectedIndexChanged="ddlwarehouselist_SelectedIndexChanged" />
                    <asp:Label ID="lblAddress" runat="server" meta:resourceKey="lblAddressResource1"
                        Visible="false"></asp:Label>
                    <asp:HiddenField ID="hdAddress" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Literal ID="liQuantityImport" runat="server" meta:resourceKey="liQuantityImportResource1"
                        Text="Số xe nhập:"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblQuantityImport" runat="server" meta:resourceKey="lblQuantityImportResource1"></asp:Label>
                    <asp:Localize ID="litVerhicle" runat="server" meta:resourcekey="litVerhicleResource1"
                        Text="(Xe) "></asp:Localize>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnTest" runat="server" meta:resourceKey="btnTestResource1" OnClick="btnTest_Click"
                        Text="Check" ValidationGroup="Check" />
                </td>
            </tr>
        </table>
    </div>
    <div class="help" style="float: right; width: 35%;">
        <ul>
            <li>
                <asp:Literal ID="Literal1" runat="server" Text="Close sequence is from bottom to up, that is from sub-store to comp-dealer, and
                from comp-dealer to main dealer." meta:resourcekey="Literal1Resource1"></asp:Literal></li>
        </ul>
    </div>
    <div style="clear: both">
    </div>
    <br />
    <asp:Label ID="lblOrderNotConfirm" runat="server" ForeColor="Red" Visible="False"
        Text="Some orders are not confirmed or not existed." meta:resourcekey="lblOrderNotConfirmResource1"></asp:Label>
    <br />
    <div class="grid">
        <asp:PlaceHolder ID="phImported" runat="server">
            <vdms:PageGridView ID="GridView1" runat="server" AutoGenerateColumns="False" meta:resourceKey="GridView2Resource1"
                DataKeyNames="ShippingId" Width="100%" CssClass="GridView" OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="No" meta:resourcekey="TemplateFieldResourceNo">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Itemtype" DataField="Itemtype" meta:resourcekey="TemplateFieldResource4">
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Color" DataField="Color" meta:resourcekey="TemplateFieldResource5">
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Engine Number" DataField="Enginenumber" meta:resourcekey="TemplateFieldResource6">
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Order Number" DataField="Ordernumber" meta:resourcekey="TemplateFieldResource7">
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Imported date" meta:resourcekey="TemplateFieldResourceImportedDate">
                        <ItemTemplate>
                            <asp:Literal ID="litImportedDate" runat="server" OnDataBinding="litImportedDate_DataBinding"
                                Text='<%# Eval("Enginenumber") %>'></asp:Literal>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Trạng th&#225;i" SortExpression="Status" meta:resourcekey="TemplateFieldResource8">
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemTemplate>
                            <asp:RadioButtonList ID="rblStatus" CssClass="nCellInDataTable" runat="server" meta:resourceKey="RadioButtonList1Resource1"
                                RepeatDirection="Horizontal" SelectedValue='<%# Bind("Status") %>' Enabled="False">
                                <asp:ListItem meta:resourceKey="ListItemResource1" Text="&lt;nobr&gt;Chưa đến&lt;/nobr&gt;"
                                    Value="0"></asp:ListItem>
                                <asp:ListItem meta:resourceKey="ListItemResource2" Text="&lt;nobr&gt;Nhập xe&lt;/nobr&gt;"
                                    Value="1"></asp:ListItem>
                                <asp:ListItem meta:resourceKey="ListItemResource3" Text="&lt;nobr&gt;Tạm nhập&lt;/nobr&gt;"
                                    Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Chứng từ" SortExpression="Voucherstatus" meta:resourcekey="TemplateFieldResource9">
                        <ItemStyle CssClass="center" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chbVoucherstatus" runat="server" Checked='<%# ToBool((int)Eval("VoucherStatus")) %>'
                                meta:resourcekey="chbVoucherstatusResource1" Enabled="False" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Diễn giải" SortExpression="Exception" meta:resourcekey="TemplateFieldResource10">
                        <ItemStyle CssClass="center" />
                        <ItemTemplate>
                            <asp:TextBox ID="txtException" runat="server" MaxLength="2000" Text='<%# Bind("Exception") %>'
                                meta:resourcekey="txtExceptionResource2" ReadOnly="True"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </vdms:PageGridView>
            <br />
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phNotImported" runat="server">
            <vdms:PageGridView ID="GridView3" runat="server" AutoGenerateColumns="False" meta:resourceKey="GridView1Resource1"
                DataKeyNames="EngineNumber" Width="100%" OnRowDataBound="GridView3_RowDataBound"
                OnDataBound="GridView3_DataBound" CssClass="GridView">
                <Columns>
                    <asp:BoundField HeaderText="No" meta:resourcekey="TemplateFieldResourceNo">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <%--OrderNo--%>
                    <asp:BoundField DataField="TipTopOrderNumber" HeaderText="Order No" Visible="False"
                        meta:resourcekey="BoundFieldResource9" />
                    <asp:TemplateField HeaderText="M&#227; xe" Visible="False" meta:resourcekey="TemplateFieldResource12">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("ItemCode") %>' meta:resourcekey="TextBox2Resource2"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle Wrap="False" />
                        <ItemTemplate>
                            <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("ItemCode") %>' meta:resourcekey="lblItemCodeResource2"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--ItemType ItemModel MadeDate--%>
                    <asp:TemplateField HeaderText="Loại xe" SortExpression="Itemtype" meta:resourcekey="TemplateFieldResource4">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server" meta:resourcekey="TextBox3Resource1" Text='<%# Bind("ItemCode") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblItemType" runat="server" meta:resourcekey="lblItemTypeResource1"
                                Text='<%# Bind("Model") %>'></asp:Label><asp:Label ID="lblMadeDate" runat="server"
                                    CssClass="hidden" Text='<%# Bind("OutStockDate") %>' meta:resourcekey="lblMadeDateResource1"></asp:Label>
                            <asp:Label ID="lblPrice" runat="server" CssClass="hidden" Text='<%# Bind("Price") %>'
                                meta:resourcekey="lblMadeDateResource1"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Wrap="False" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="ItemName" HeaderText="ItemName" Visible="False" meta:resourcekey="BoundFieldResource11" />
                    <%--Color--%>
                    <asp:TemplateField HeaderText="M&#224;u sắc" SortExpression="Color" meta:resourcekey="TemplateFieldResource5">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox4" runat="server" meta:resourcekey="TextBox4Resource1" Text='<%# Bind("ColorCode") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblColor" runat="server" meta:resourcekey="lblColorResource1" Text='<%# Bind("ColorCode") %>'></asp:Label>
                            (<asp:Label ID="lblColorName" runat="server" Text='<%# Bind("ColorName") %>' meta:resourcekey="lblColorNameResource1"></asp:Label>)
                            &nbsp; &nbsp;
                        </ItemTemplate>
                        <ItemStyle Wrap="False" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Số m&#225;y" SortExpression="Enginenumber" meta:resourcekey="TemplateFieldResource6">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox5" runat="server" meta:resourcekey="TextBox5Resource1" Text='<%# Bind("EngineNumber") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblEngineNumber" runat="server" meta:resourcekey="lblEngineNumberResource1"
                                Text='<%# Bind("EngineNumber") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Wrap="False" />
                    </asp:TemplateField>
                    <%--Ordernumber--%>
                    <asp:TemplateField HeaderText="Số đơn h&#224;ng" SortExpression="Ordernumber" meta:resourcekey="TemplateFieldResource7">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox6" runat="server" meta:resourcekey="TextBox6Resource1" Text='<%# Bind("TipTopOrderNumber") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblOrderNumber" runat="server" meta:resourcekey="lblOrderNumberResource1"
                                Text='<%# Bind("TipTopOrderNumber") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Wrap="False" />
                    </asp:TemplateField>
                    <%--Comfirmed--%>
                    <asp:TemplateField HeaderText="Comfirmed" SortExpression="Voucherstatus" meta:resourcekey="ComfirmedResource1">
                        <ItemStyle CssClass="center" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chbOrderComfirmed" Enabled="false" runat="server" Checked='<%# IsOrderConfirmed(Eval("TipTopOrderNumber").ToString()) %>'
                                meta:resourcekey="chbVoucherstatusResource1" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--Warehouse--%>
                    <asp:BoundField DataField="BranchCode" HeaderText="Warehouse" Visible="true" meta:resourcekey="BoundFieldResource9z" />
                    <asp:TemplateField HeaderText="Imported date" meta:resourcekey="TemplateFieldResourceImportedDate2">
                        <ItemTemplate>
                            <table cellpadding="0" cellspacing="0" border="0" class="nCellInDataTable">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtItemImportDate" runat="server" meta:resourceKey="txtImportDateResource1"
                                            Width="100px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RangeValidator ControlToValidate="txtItemImportDate" ID="rvItemImportDate" SetFocusOnError="true"
                                            meta:resourceKey="rvItemImportDateResource1" ValidationGroup="Save" runat="server"
                                            ErrorMessage="RangeValidator">*</asp:RangeValidator>
                                    </td>
                                    <td>
                                        <asp:ImageButton CssClass="hidden" ID="imgbItemCarlendar" runat="server" OnClientClick="return false;"
                                            SkinID="CalendarImageButton" />
                                    </td>
                                </tr>
                            </table>
                            <ajaxToolkit:MaskedEditExtender ID="mexItemImportedDate" runat="server" BehaviorID="mexItemImportedDate"
                                Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtItemImportDate"
                                OnLoad="mexItemImportedDate_Load">
                            </ajaxToolkit:MaskedEditExtender>
                            <ajaxToolkit:CalendarExtender ID="carxItemImportedDate" runat="server" BehaviorID="carxItemImportedDate"
                                Enabled="True" PopupButtonID="imgbItemCarlendar" TargetControlID="txtItemImportDate"
                                OnLoad="carxItemImportedDate_Load">
                            </ajaxToolkit:CalendarExtender>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Trạng th&#225;i" SortExpression="Status" meta:resourcekey="TemplateFieldResource8">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" meta:resourcekey="Label1Resource1" Text='<%# Eval("Status") %>'
                                Visible="False"></asp:Label><asp:RadioButtonList ID="rblStatus" runat="server" meta:resourceKey="RadioButtonList1Resource1"
                                    RepeatDirection="Horizontal" CssClass="nCellInDataTable" SelectedValue='<%# Bind("Status") %>'>
                                    <asp:ListItem meta:resourceKey="ListItemResource1" Text="&lt;nobr&gt;Chưa đến&lt;/nobr&gt;"
                                        Value="0"></asp:ListItem>
                                    <asp:ListItem meta:resourceKey="ListItemResource2" Text="&lt;nobr&gt;Nhập xe&lt;/nobr&gt;"
                                        Value="1"></asp:ListItem>
                                    <asp:ListItem meta:resourceKey="ListItemResource3" Text="&lt;nobr&gt;Tạm nhập&lt;/nobr&gt;"
                                        Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Chứng từ" SortExpression="Voucherstatus" meta:resourcekey="TemplateFieldResource9">
                        <EditItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked="True" meta:resourcekey="CheckBox1Resource1" />
                        </EditItemTemplate>
                        <ItemStyle CssClass="center" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chbVoucherstatus" runat="server" Checked="True" meta:resourcekey="chbVoucherstatusResource1" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Diễn giải" SortExpression="Exception" meta:resourcekey="TemplateFieldResource10">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtException" runat="server" MaxLength="2000" meta:resourcekey="txtExceptionResource1"
                                Text='<%# Bind("Exception") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="txtException" runat="server" MaxLength="2000" meta:resourcekey="txtExceptionResource2"
                                Text='<%# Bind("Exception") %>'></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </vdms:PageGridView>
            <asp:Button ID="btnAccept" runat="server" meta:resourcekey="btnAcceptResource2" OnClick="btnAccept_Click"
                Text="X&#225;c nhận" ValidationGroup="Save" /><br />
        </asp:PlaceHolder>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectCountMethod="SelectCount"
            SelectMethod="Select" TypeName="VDMS.I.ObjectDataSource.ShippingDetailDataSource"
            UpdateMethod="Update">
            <UpdateParameters>
                <asp:Parameter Name="ShippingId" Type="Int64" />
                <asp:Parameter Name="Itemtype" Type="String" />
                <asp:Parameter Name="Color" Type="String" />
                <asp:Parameter Name="Enginenumber" Type="String" />
                <asp:Parameter Name="Ordernumber" Type="String" />
                <asp:Parameter Name="Status" Type="Int32" />
                <asp:Parameter Name="Exception" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter Name="maximumRows" Type="Int32" />
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="shipID" Type="Int64" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    <br />

    <script language="javascript" type="text/javascript" src="../../js/ClientValidate.js"></script>

    <asp:Literal ID="litTxtList" runat="server" meta:resourcekey="litTxtListResource1"></asp:Literal>

    <script language="javascript" type="text/javascript">
    <!--
        var btnTest = document.getElementById('<%=btnTest.ClientID%>');
        var btnAccept = document.getElementById('<%=btnAccept.ClientID%>');
        Import_ExceptionChecker();
    -->
    </script>

    <vdms:PageGridView ID="GridView2" Visible="true" runat="server" AllowPaging="True"
        AutoGenerateColumns="False" PageSize="70" OnRowDataBound="GridView2_RowDataBound">
        <Columns>
            <asp:BoundField DataField="ShippingNumber" HeaderText="Phieu xuat xe" meta:resourcekey="BoundFieldResource8"
                SortExpression="OGA01" />
            <asp:BoundField DataField="ShippingNumber" HeaderText="Imported" meta:resourcekey="BoundFieldResource2" />
        </Columns>
    </vdms:PageGridView>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="h">
</asp:Content>
