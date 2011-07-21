<%@ Page Title="Parts Sale" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="Sale.aspx.cs" Inherits="Part_Sale" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register TagPrefix="cc1" TagName="UpdateProgress" Src="~/Controls/UpdateProgress.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Info" Src="~/Controls/ContactInfo.ascx" %>
<%@ Register TagPrefix="cc1" TagName="SearchPartUI" Src="~/Part/Inventory/SearchPartUI.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="VDMS.II.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">

    <script type="text/javascript">
        function updated() {
            tb_remove();
            $('#<%= this.bt1.ClientID %>').click();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <ajaxToolkit:TabContainer ID="t" runat="server" CssClass="ajax__tab_technorati-theme"
        ActiveTabIndex="0" meta:resourcekey="tResource2">
        <ajaxToolkit:TabPanel ID="t1" runat="server" HeaderText="Sales Data" meta:resourcekey="t1Resource3">
            <HeaderTemplate>
                <asp:Literal ID="Literal6" runat="server" Text="Sales Data" meta:resourcekey="Literal6Resource1"></asp:Literal>
            </HeaderTemplate>
            <ContentTemplate>
                <div class="form" style="width: 50%; float: left;">
                    <asp:ValidationSummary ValidationGroup="Save" CssClass="error" ID="ValidationSummary1"
                        runat="server" meta:resourcekey="ValidationSummary1Resource1" />
                    <table>
                        <tr>
                            <td style="width: 120px;">
                                <asp:Literal ID="Literal1" runat="server" Text="Discount(%):" meta:resourcekey="Literal1Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="tbDiscount" runat="server" Text="0" CssClass="number" meta:resourcekey="tbDiscountResource1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="tbDiscount"
                                    SetFocusOnError="True" ValidationGroup="ApplyDiscount" Text="*" meta:resourcekey="rfv1Resource1"></asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rv1" runat="server" ControlToValidate="tbDiscount" MinimumValue="0"
                                    MaximumValue="100" SetFocusOnError="True" Type="Integer" ValidationGroup="ApplyDiscount"
                                    Text="*" meta:resourcekey="rv1Resource1"></asp:RangeValidator>
                                <asp:Button ID="bAD" runat="server" Text="Apply to All" OnClick="bAD_Click" ValidationGroup="ApplyDiscount"
                                    meta:resourcekey="bADResource1" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal2" runat="server" Text="Sale voucher No:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="tbSVN" runat="server" ReadOnly="True" meta:resourcekey="tbSVNResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal3" runat="server" Text="Temporary Sale voucher No:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="tbTSVN" runat="server" meta:resourcekey="tbTSVNResource1"></asp:TextBox>
                                <asp:CustomValidator ID="cvTSVN" runat="server" ControlToValidate="tbTSVN" ErrorMessage="Temporary Sale Voucher No cannot duplicate"
                                    OnServerValidate="cvTSVN_ServerValidate" SetFocusOnError="True" ValidationGroup="Save"
                                    meta:resourcekey="cvTSVNResource1" Text="*"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px;">
                                <asp:Literal ID="Literal4" runat="server" Text="Select Customer:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl1" runat="server" OnSelectedIndexChanged="ddl1_SelectedIndexChanged"
                                    AutoPostBack="True" OnDataBound="ddl1_SelectedIndexChanged" AppendDataBoundItems="True"
                                    DataTextField="Name" DataValueField="CustomerId" meta:resourcekey="ddl1Resource1">
                                    <asp:ListItem Text="Retail Customer" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal5" runat="server" Text="Customer Name:" meta:resourcekey="Literal5Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="up2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="cn" runat="server" meta:resourcekey="cnResource1"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Save" ControlToValidate="cn"
											runat="server" ErrorMessage="Customer name cannot be empty!" meta:resourcekey="RequiredFieldValidator1Resource1"
											Text="*"></asp:RequiredFieldValidator>--%>
                                        <asp:Button ID="vd" runat="server" Text="View Detail" Visible="False" OnClick="vd_Click"
                                            meta:resourcekey="vdResource1" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddl1" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="litOrderDate" runat="server" Text="Order Date:" meta:resourcekey="litOrderDateResource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="tbOrderDate" runat="server" meta:resourcekey="tbOrderDateResource1"></asp:TextBox>
                                <asp:ImageButton ID="ibOrderDate" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
                                    meta:resourcekey="ibOrderDateResource1" />
                                <ajaxToolkit:MaskedEditExtender ID="meeOrderDate" runat="server" TargetControlID="tbOrderDate"
                                    Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder=""
                                    CultureDateFormat="MDY" CultureDatePlaceholder="/" CultureDecimalPlaceholder="."
                                    CultureThousandsPlaceholder="," CultureTimePlaceholder=":" Enabled="True">
                                </ajaxToolkit:MaskedEditExtender>
                                <ajaxToolkit:CalendarExtender ID="ceOrderDate" runat="server" TargetControlID="tbOrderDate"
                                    PopupButtonID="ibOrderDate" Enabled="True">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="litSalesDate" runat="server" Text="Sales Date:" meta:resourcekey="litSalesDateResource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="tbSalesDate" runat="server" meta:resourcekey="tbSalesDateResource1"></asp:TextBox>
                                <asp:ImageButton ID="ibSalesDate" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
                                    meta:resourcekey="ibSalesDateResource1" />
                                <ajaxToolkit:MaskedEditExtender ID="meeSalesDate" runat="server" TargetControlID="tbSalesDate"
                                    Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder=""
                                    CultureDateFormat="MDY" CultureDatePlaceholder="/" CultureDecimalPlaceholder="."
                                    CultureThousandsPlaceholder="," CultureTimePlaceholder=":" Enabled="True">
                                </ajaxToolkit:MaskedEditExtender>
                                <ajaxToolkit:CalendarExtender ID="ceSalesDate" runat="server" TargetControlID="tbSalesDate"
                                    PopupButtonID="ibSalesDate" Enabled="True">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize ID="litComment" runat="server" Text="Comment:" meta:resourcekey="litCommentResource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="tbComment" runat="server" MaxLength="256" meta:resourcekey="tbCommentResource1"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:PlaceHolder ID="ph" runat="server" Visible="False">
                                <div class="form" style="width: 75%">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 25%;">
                                                <asp:Image ID="image1" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image1Resource1" />
                                                <asp:Localize ID="l1" runat="server" Text="VAT code:" meta:resourcekey="l1Resource2"></asp:Localize>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbVAT" runat="server" Font-Bold="True" meta:resourcekey="lbVATResource1"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <cc1:Info ID="_info1" runat="server"></cc1:Info>
                                </div>
                            </asp:PlaceHolder>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="vd" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="help" style="float: right; width: 35%;">
                    <asp:Localize ID="lh1" runat="server" Text="Sell item to end-user:" meta:resourcekey="lh1Resource1"></asp:Localize>
                    <ul>
                        <li>
                            <asp:Localize ID="lh2" runat="server" Text="You only can sales out in the open date."
                                meta:resourcekey="lh2Resource1"></asp:Localize>
                        </li>
                        <li>
                            <asp:Localize ID="lh3" runat="server" Text="If the sales out date is greater than current date, it will be reset to current date."
                                meta:resourcekey="lh3Resource1"></asp:Localize>
                        </li>
                    </ul>
                </div>
                <div style="clear: both;">
                </div>
                <cc1:UpdateProgress ID="upg1" runat="server" />
                <div class="form" style="width: 450px">
                    <asp:HyperLink ID="cmdFavourite" runat="server" Text="Favourite" class="thickbox"
                        title="Favourite" href="Inventory/Favourite.aspx?target=PS&ftype=S&TB_iframe=true&height=220&width=410"
                        meta:resourcekey="cmdFavouriteResource1"></asp:HyperLink>
                    <br />
                    <cc1:SearchPartUI ID="sp" runat="server" SearchOption="&target=SL&at=SL" />
                    <table>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal7" runat="server" Text="Page mode:" meta:resourcekey="Literal7Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:LinkButton ID="cmdAddRow" runat="server" Text="Add" OnClick="cmdAddRow_Click"
                                    meta:resourcekey="cmdAddRowResource1"></asp:LinkButton>
                                <asp:DropDownList ID="ddlRowCount" runat="server" meta:resourcekey="ddlRowCountResource1">
                                    <asp:ListItem Text="5" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                    <asp:ListItem Text="10" meta:resourcekey="ListItemResource3"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:Literal ID="Literal8" runat="server" Text="row." meta:resourcekey="Literal8Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="Literal9" runat="server" Text="Rows/table:" meta:resourcekey="Literal9Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlRows" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRows_SelectedIndexChanged"
                                    meta:resourcekey="ddlRowsResource1">
                                    <asp:ListItem Text="5" meta:resourcekey="ListItemResource4"></asp:ListItem>
                                    <asp:ListItem Text="10" Selected="True" meta:resourcekey="ListItemResource5"></asp:ListItem>
                                    <asp:ListItem Text="20" meta:resourcekey="ListItemResource6"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
                <asp:Label ID="lblSaveOk" runat="server" SkinID="MessageOk" Visible="False" Text="The order has been created/updated successful."
                    meta:resourcekey="lblSaveOkResource2"></asp:Label>
                <asp:Label ID="lblOverStock" runat="server" SkinID="MessageError" Visible="False"
                    Text="Cannot sales. Some items over stock." meta:resourcekey="lblOverStockResource2"></asp:Label>
                <asp:Label ID="lblInventoryClose" runat="server" SkinID="MessageError" Visible="False"
                    Text="Cannot sales. The inventory is closed." meta:resourcekey="lblInventoryCloseResource2"></asp:Label>
                <asp:ObjectDataSource ID="odsPartList" runat="server" TypeName="VDMS.II.PartManagement.Sales.PartSalesDAO"
                    SelectMethod="FindAll" />
                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="bt1" runat="server" Style="display: none" OnClick="Refresh_Click"
                            meta:resourcekey="bt1Resource2" />
                        <div class="grid">
                            <vdms:PageGridView ID="gv1" runat="server" ShowFooter="True" DataSourceID="odsPartList"
                                OnDataBound="gv1_DataBound" AutoGenerateColumns="False" OnPreRender="gv1_PreRender"
                                AllowPaging="True" OnPageIndexChanging="gv1_PageIndexChanging" OnRowDataBound="gv1_RowDataBound"
                                meta:resourcekey="gv1Resource2">
                                <Columns>
                                    <asp:TemplateField HeaderText="Part Code" meta:resourcekey="TemplateFieldResource5">
                                        <ItemTemplate>
                                            <asp:TextBox ID="t1" runat="server" Text='<%# Eval("PartCode") %>' meta:resourcekey="t1Resource4"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type" meta:resourcekey="TemplateFieldResource6">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlT" runat="server" meta:resourcekey="ddlTResource2">
                                                <asp:ListItem Text="Part" Value="P" meta:resourcekey="ListItemResource9"></asp:ListItem>
                                                <asp:ListItem Text="Accessory" Value="A" meta:resourcekey="ListItemResource10"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Part Name" DataField="PartName" meta:resourcekey="BoundFieldResource5" />
                                    <asp:BoundField HeaderText="Current Stock" DataField="Stock" meta:resourcekey="BoundFieldResource6">
                                        <ItemStyle CssClass="number" Font-Bold="True" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Available Stock" DataField="AvailableStock" meta:resourcekey="BoundFieldResource9">
                                        <ItemStyle CssClass="number" Font-Bold="True" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Quantity" meta:resourcekey="TemplateFieldResource7">
                                        <ItemTemplate>
                                            <asp:TextBox ID="t2" runat="server" Text='<%# Eval("Quantity") %>' SkinID="InGrid"
                                                CssClass="number" meta:resourcekey="t2Resource3"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="ftb2" runat="server" TargetControlID="t2"
                                                FilterType="Numbers" Enabled="True">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="UnitPrice" DataField="UnitPrice" DataFormatString="{0:C0}"
                                        meta:resourcekey="BoundFieldResource7">
                                        <ItemStyle CssClass="number" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Discount(%)" meta:resourcekey="TemplateFieldResource8">
                                        <ItemTemplate>
                                            <asp:TextBox ID="t3" runat="server" Text='<%# Eval("Discount") %>' SkinID="InGrid"
                                                CssClass="number" meta:resourcekey="t3Resource2"></asp:TextBox>
                                            <asp:RangeValidator ID="rv3" runat="server" ControlToValidate="t3" MinimumValue="0"
                                                MaximumValue="100" ErrorMessage="*" SetFocusOnError="True" Type="Integer" meta:resourcekey="rv3Resource2"></asp:RangeValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Amount" DataField="Amount" DataFormatString="{0:C0}"
                                        meta:resourcekey="BoundFieldResource8">
                                        <ItemStyle CssClass="number" />
                                    </asp:BoundField>
                                </Columns>
                                <EmptyDataTemplate>
                                    There isn't any rows.
                                </EmptyDataTemplate>
                            </vdms:PageGridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmdAddRow" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlRows" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                <div class="button">
                    <asp:Button ID="cmdSave" ValidationGroup="Save" runat="server" Text="Save" OnClick="cmdSave_Click"
                        meta:resourcekey="cmdSaveResource2" />
                    <asp:Button ID="cmdSale" ValidationGroup="Save" runat="server" Text="Sale out" OnClick="cmdSale_Click"
                        meta:resourcekey="cmdSaleResource2" />
                    <asp:Button ID="cmdPrint" ValidationGroup="Save" runat="server" Text="Print" OnClick="cmdPrint_Click"
                        meta:resourcekey="cmdPrintResource2" />
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="t2" runat="server" HeaderText="Sales Data by upload Excel file"
            meta:resourcekey="t2Resource4">
            <ContentTemplate>
                <div class="form" style="width: 450px">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Localize ID="litFileName" runat="server" Text="Filename:" meta:resourcekey="litFileNameResource2"></asp:Localize>
                            </td>
                            <td>
                                <asp:FileUpload ID="fu" runat="server" meta:resourcekey="fuResource2" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="b1" runat="server" Text="Upload" OnClick="b1_Click" meta:resourcekey="b1Resource2" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lblExcelError" runat="server" SkinID="MessageError" Visible="False"
                        Text="Cannot create excel order. The excel file structure is invalid. Please contact with VMEP IT for detail."
                        meta:resourcekey="lblExcelErrorResource2"></asp:Label>
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
</asp:Content>
