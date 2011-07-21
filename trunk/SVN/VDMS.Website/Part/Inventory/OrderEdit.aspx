<%@ Page Title="Order Form Edit" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="OrderEdit.aspx.cs" Inherits="Part_Inventory_OrderEdit"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register TagPrefix="cc1" TagName="UpdateProgress" Src="~/Controls/UpdateProgress.ascx" %>
<%@ Register TagPrefix="cc1" TagName="SearchPartUI" Src="~/Part/Inventory/SearchPartUI.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">

    <script type="text/javascript">
        function updated() {
            //  close the popup
            tb_remove();
            //  refresh the update panel so we can view the changes
            $('#<%= this.bt1.ClientID %>').click();
        }
        function replacePart(code, dest) {
            //  close the popup
            tb_remove();
            var textbox = document.getElementById(dest);
            textbox.value = code;
            $('#<%= this.bt1.ClientID %>').click();
        }
        function showSafety(link) {
            var s = "SafetyStock.aspx?";
            s = s + "wid=" + $('#<%= this.ddlWH.ClientID %>').val();
            s = s + "&TB_iframe=true&height=420&width=510";
            link.href = s;
        }
        function repPart(caller, line) {
            var s = "ReplacePart.aspx?";
            s = s + "code=" + $('#' + caller).val();
            s = s + "&caller=" + caller;
            s = s + "&line=" + line;
            s = s + "&TB_iframe=true&height=420&width=510";
            document.getElementById("lnkRepPart").href = s;
            $('#lnkRepPart').click();
        }		
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <a id="lnkRepPart" class="thickbox" href="ReplacePart.aspx?TB_iframe=true&height=420&width=510"></a>
    <div class="form" style="width: 50%; float: left;">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="error" />
        <table width="100%">
            <tr>
                <td style="width: 30%">
                    <asp:Localize ID="litDealer" runat="server" Text="Delivered Dealer:" meta:resourcekey="litDealerResource1"></asp:Localize>
                </td>
                <td>
                    <vdms:DealerList ID="ddlDealer" runat="server" AutoPostBack="True" AppendDataBoundItems="True"
                        OnDataBound="ddlDealer_DataBound" OnSelectedIndexChanged="ddlDealer_SelectedIndexChanged"
                        EnabledSaperateByArea="False" EnabledSaperateByDB="False" MergeCode="False" meta:resourcekey="ddlDealerResource1"
                        RemoveRootItem="False" ShowEmptyItem="False" ShowSelectAllItem="False">
                    </vdms:DealerList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Localize ID="litWarehouse" runat="server" Text="Delivered Warehouse:" meta:resourcekey="litWarehouseResource1"></asp:Localize>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <vdms:WarehouseList ID="ddlWH" OnDataBound="ddlWH_DataBound" runat="server" DontAutoUseCurrentSealer="False"
                                UseVIdAsValue="False">
                            </vdms:WarehouseList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlWH"
                                ErrorMessage="Warehouse cannot be blank!" Text="*" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            <cc1:UpdateProgress ID="upg1" runat="server" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlDealer" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Localize ID="litOrderDate" runat="server" Text="Order Date:" meta:resourcekey="litOrderDateResource1"></asp:Localize>
                </td>
                <td>
                    <asp:TextBox ID="txtOrderDate" runat="server" meta:resourcekey="txtOrderDateResource1"></asp:TextBox>
                    <asp:PlaceHolder ID="phEditOrderDate" runat="server">
                        <asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                            meta:resourcekey="ibFromDateResource1" />
                        <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtOrderDate"
                            Mask="99/99/9999" MaskType="Date" BehaviorID="meeFromDate" CultureAMPMPlaceholder="AM;PM"
                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                            CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                            Enabled="True">
                        </ajaxToolkit:MaskedEditExtender>
                        <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtOrderDate"
                            PopupButtonID="ibFromDate" BehaviorID="ceFromDate" Enabled="True">
                        </ajaxToolkit:CalendarExtender>
                    </asp:PlaceHolder>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Localize ID="litOrderType" runat="server" Text="Order Type:" meta:resourcekey="litOrderTypeResource1"></asp:Localize>
                </td>
                <td>
                    <asp:Label ID="lblOT" runat="server" Font-Bold="True" Text="Normal" meta:resourcekey="lblOTResource1"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div class="help" style="float: right; width: 35%;">
        <ul>
            <li>
                <asp:Localize ID="lh1" runat="server" Text="Use Favorite, Safety Stock or Search Part to look up parts and add to grid."
                    meta:resourcekey="lh1Resource1"></asp:Localize>
            </li>
            <li>
                <asp:Localize ID="lh2" runat="server" Text="Use Excel mode to manual add parts to grid."
                    meta:resourcekey="lh2Resource1"></asp:Localize>
            </li>
            <li>
                <asp:Localize ID="lh3" runat="server" Text="Use Upload Excel file to upload an exist excel order to system."
                    meta:resourcekey="lh3Resource1"></asp:Localize>
            </li>
            <li>
                <asp:Localize ID="lh4" runat="server" Text="Order type always is Normal." meta:resourcekey="lh4Resource1"></asp:Localize>
            </li>
            <li>
                <asp:Localize ID="lh5" runat="server" Text="Save order here, send Order at Query Order screen."
                    meta:resourcekey="lh5Resource1"></asp:Localize>
            </li>
        </ul>
    </div>
    <div style="clear: both;">
    </div>
    <br />
    <asp:Label ID="lblSaveOk" runat="server" SkinID="MessageOk" Visible="False" Text="The order has been created/updated successful."
        meta:resourcekey="lblSaveOkResource1"></asp:Label>
    <asp:Label ID="lblTipTopProcessed" runat="server" SkinID="MessageError" Visible="False"
        Text="Tip-Top has been processed this order, cannot update new data." meta:resourcekey="lblTipTopProcessedResource1"></asp:Label>
    <ajaxToolkit:TabContainer ID="t" runat="server" CssClass="ajax__tab_technorati-theme"
        ActiveTabIndex="0" meta:resourcekey="tResource2">
        <ajaxToolkit:TabPanel ID="t1" runat="server" HeaderText="Create new Order">
            <HeaderTemplate>
                <asp:Literal ID="Literal1" runat="server" meta:resourcekey="t1Resource3"></asp:Literal>
            </HeaderTemplate>
            <ContentTemplate>
                <asp:ObjectDataSource ID="odsPartList" runat="server" TypeName="VDMS.II.PartManagement.Order.PartOrderDAO"
                    SelectMethod="FindAll"></asp:ObjectDataSource>
                <div class="form">
                    <asp:HyperLink ID="cmdFavourite" runat="server" Text="Favourite" class="thickbox"
                        title="Favourite" href="Favourite.aspx?at=OD&TB_iframe=true&height=400&width=410"
                        meta:resourcekey="cmdFavouriteResource1"></asp:HyperLink>
                    |
                    <asp:HyperLink ID="cmdSafetyStock" runat="server" Text="Safety Stock" class="thickbox"
                        title="Safety Stock" onclick="javascript:showSafety(this)" href="#" meta:resourcekey="cmdSafetyStockResource1"></asp:HyperLink>
                    <br />
                    <cc1:SearchPartUI ID="sp" runat="server" SearchOption="&at=OD&acc=N" />
                    <table>
                        <tr>
                            <td>
                                <asp:Localize ID="Localize1" runat="server" Text="Excel mode:" meta:resourcekey="Localize1Resource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:LinkButton ID="cmdAddRow" runat="server" Text="Add" OnClick="cmdAddRow_Click"
                                    meta:resourcekey="cmdAddRowResource1"></asp:LinkButton>
                                <asp:DropDownList ID="ddlRowCount" runat="server" meta:resourcekey="ddlRowCountResource1">
                                    <asp:ListItem Text="5" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                    <asp:ListItem Text="10" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:Localize ID="Localize2" runat="server" Text="rows." meta:resourcekey="Localize2Resource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:Localize ID="Localize3" runat="server" Text="Rows/table:" meta:resourcekey="Localize3Resource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlRows" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRows_SelectedIndexChanged"
                                    meta:resourcekey="ddlRowsResource1">
                                    <asp:ListItem Text="5" meta:resourcekey="ListItemResource3"></asp:ListItem>
                                    <asp:ListItem Text="10" Selected="True" meta:resourcekey="ListItemResource4"></asp:ListItem>
                                    <asp:ListItem Text="20" meta:resourcekey="ListItemResource5"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="bt1" runat="server" Style="display: none" OnClick="Refresh_Click"
                                meta:resourcekey="bt1Resource1" />
                            <div class="grid">
                                <vdms:PageGridView DataSourceID="odsPartList" ID="gv1" runat="server" OnPageIndexChanging="gv1_PageIndexChanging"
                                    AllowPaging="True" OnPreRender="gv1_PreRender" AutoGenerateColumns="false" meta:resourcekey="gv1Resource1"
                                    OnDataBinding="gv1_DataBinding">
                                    <Columns>
                                        <asp:BoundField HeaderText="Line" DataField="Line" meta:resourcekey="BoundFieldResource1" />
                                        <asp:TemplateField HeaderText="Part Code" meta:resourcekey="TemplateFieldResource1">
                                            <ItemTemplate>
                                                <asp:TextBox ID="t1" runat="server" ToolTip='<%# Eval("Line") %>' Text='<%# Eval("PartCode") %>'></asp:TextBox>
                                                <input alt="Change" id="lnkRep" ondatabinding="lnkRep_databinding" type="image" runat="server"
                                                    src="../../Images/arrow-retweet.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Part Name" DataField="PartName" meta:resourcekey="BoundFieldResource2" />
                                        <asp:TemplateField HeaderText="Quantity" meta:resourcekey="TemplateFieldResource2">
                                            <ItemTemplate>
                                                <asp:TextBox ID="t2" runat="server" Text='<%# Eval("Quantity") %>' CssClass="number"
                                                    meta:resourcekey="t2Resource1" Width="70"></asp:TextBox>
                                                <asp:Label Visible='<%# Eval("ChangedForPacking") %>' CssClass="errorItem" ID="Literal3"
                                                    runat="server" Text=" (Changed for packing standard)" meta:resourcekey="Literal3Resource1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <asp:Localize ID="Localize1" runat="server" Text="There isn't any rows." meta:resourcekey="Localize1Resource2"></asp:Localize>
                                    </EmptyDataTemplate>
                                </vdms:PageGridView>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmdAddRow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddlRows" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="button">
                    <asp:Button ID="cmdSave" runat="server" Text="Save" CommandName="Save" OnClick="cmdSave_Click"
                        ValidationGroup="Save" meta:resourcekey="cmdSaveResource1" />
                    <asp:Button ID="bBack" runat="server" Text="Back" CommandName="Cancel" OnClientClick="javascript:location.href='Order.aspx'; return false;"
                        CausesValidation="False" meta:resourcekey="bBackResource1" />
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="t2" runat="server" HeaderText="Upload Excel file Order">
            <HeaderTemplate>
                <asp:Literal ID="Literal2" runat="server" meta:resourcekey="t2Resource2"></asp:Literal>
            </HeaderTemplate>
            <ContentTemplate>
                <div class="form" style="width: 450px">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Localize ID="litFileName" runat="server" Text="Filename:" meta:resourcekey="litFileNameResource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:FileUpload ID="fu" runat="server" meta:resourcekey="fuResource1" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="b1" runat="server" Text="Upload and Create Order" OnClick="b1_Click"
                                    meta:resourcekey="b1Resource1" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lblExcelError" runat="server" SkinID="MessageError" Visible="False"
                        Text="Cannot create excel order. The excel file structure is invalid. Please contact with VMEP IT for detail."
                        meta:resourcekey="lblExcelErrorResource1"></asp:Label>
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
</asp:Content>
