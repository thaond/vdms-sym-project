<%@ Page Title="Dealer Edit" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="DealerEdit.aspx.cs" Inherits="Admin_Security_DealerEdit"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register TagPrefix="cc1" Namespace="VDMS.II.WebControls" %>
<%@ Register Src="~/Controls/ContactInfo.ascx" TagName="ContactInfo" TagPrefix="cc1" %>
<%@ Register Src="../Controls/Warehouse.ascx" TagName="Warehouse" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <ajaxToolkit:TabContainer ID="t" runat="server" CssClass="ajax__tab_technorati-theme"
        ActiveTabIndex="0" meta:resourcekey="TabContainer1Resource1">
        <ajaxToolkit:TabPanel ID="t1" runat="server" HeaderText="Basic Data" meta:resourcekey="t1Resource1">
            <ContentTemplate>
                <div class="form" style="width: 550px" runat="server" id="inputForm">
                    <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Save" runat="server"
                        CssClass="error" meta:resourcekey="ValidationSummary1Resource1" />
                    <div runat="server" id="_msg">
                    </div>
                    <table width="100%">
                        <tr>
                            <td style="width: 35%;">
                                <asp:Image ID="image1" runat="server" SkinID="RequireField" meta:resourcekey="image1Resource1" />
                                <asp:Localize ID="l1" runat="server" Text="Dealer Code:" meta:resourcekey="l1Resource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="tb1" runat="server" MaxLength="30" Width="180px" meta:resourcekey="tb1Resource1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv1" runat="server" ErrorMessage="Dealer Code cannot be empty!"
                                    SetFocusOnError="True" ValidationGroup="Save" Text="*" ControlToValidate="tb1"
                                    meta:resourcekey="rfv1Resource1"></asp:RequiredFieldValidator>
                                <asp:Button ID="btCheck" runat="server" Text="Check from TipTop" CausesValidation="False"
                                    OnClick="btCheck_Click" meta:resourcekey="btCheckResource1" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="image2" runat="server" SkinID="RequireField" meta:resourcekey="image2Resource1" />
                                <asp:Localize ID="l2" runat="server" Text="Dealer Name:" meta:resourcekey="l2Resource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="tb2" runat="server" MaxLength="256" Width="180px" ReadOnly="True"
                                    meta:resourcekey="tb2Resource1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv2" runat="server" ErrorMessage="Dealer Name cannot be empty!"
                                    ValidationGroup="Save" SetFocusOnError="True" ControlToValidate="tb2" meta:resourcekey="rfv2Resource1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="image7" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image7Resource1" />
                                <asp:Localize ID="litDealerType" runat="server" Text="Dealer Type:" meta:resourcekey="litDealerTypeResource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="tbDT" runat="server" MaxLength="256" Width="180px" ReadOnly="True"
                                    meta:resourcekey="tbDTResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="image8" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image8Resource1" />
                                <asp:Localize ID="litOCD" runat="server" Text="Order Control Date:" meta:resourcekey="litOCDResource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:CheckBoxList ID="cblOCD" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                    meta:resourcekey="cblOCDResource1">
                                    <asp:ListItem Text="Sunday" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                    <asp:ListItem Text="Monday" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                    <asp:ListItem Text="Tuesday" meta:resourcekey="ListItemResource3"></asp:ListItem>
                                    <asp:ListItem Text="Wednesday" meta:resourcekey="ListItemResource4"></asp:ListItem>
                                    <asp:ListItem Text="Thursday" meta:resourcekey="ListItemResource5"></asp:ListItem>
                                    <asp:ListItem Text="Friday" meta:resourcekey="ListItemResource6"></asp:ListItem>
                                    <asp:ListItem Text="Saturday" meta:resourcekey="ListItemResource7"></asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <%--<tr>
                            <td>
                                <asp:Image ID="image6" runat="server" SkinID="RequireField" meta:resourcekey="image6Resource1" />
                                <asp:Localize ID="litReceiveSpan" runat="server" Text="Receive span:" meta:resourcekey="litReceiveSpanResource1"></asp:Localize>
                            </td>
                             <td>
                                <asp:TextBox ID="tbRS" runat="server" MaxLength="3" Width="180px" Text="10" CssClass="number"
                                    meta:resourcekey="tbRSResource1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv3" runat="server" ControlToValidate="tbRS" ValidationGroup="Save"
                                    ErrorMessage="Receive span cannot be empty!" SetFocusOnError="True" Text="*"
                                    meta:resourcekey="rfv3Resource1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>--%>
                        <tr>
                            <td>
                                <asp:Image ID="image3" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image3Resource1" />
                                <asp:Localize ID="l3" runat="server" Text="Parent:" meta:resourcekey="l3Resource1"></asp:Localize>
                            </td>
                            <td>
                                <cc1:DealerList ID="ddlParent" runat="server" Width="180px" AppendDataBoundItems="True"
                                    RootDealer="/" EnabledSaperateByDB="False" ShowSelectAllItem="False" MergeCode="False"
                                    RemoveRootItem="False" EnabledSaperateByArea="False" meta:resourcekey="ddlParentResource1"
                                    ShowEmptyItem="False">
                                </cc1:DealerList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="image5" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image5Resource1" />
                                <asp:Localize runat="server" ID="litDatabaseCode" Text="Database Code:" meta:resourcekey="litDatabaseCodeResource1" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDatabase" runat="server" Enabled="False" meta:resourcekey="ddlDatabaseResource1">
                                    <asp:ListItem Value="DNF" Text="Dongnai" meta:resourcekey="ListItemResource8"></asp:ListItem>
                                    <asp:ListItem Value="HTF" Text="Hatay" meta:resourcekey="ListItemResource9"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="image4" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image4Resource1" />
                                <asp:Localize runat="server" ID="litAreaCode" Text="Area Code:" meta:resourcekey="litAreaCodeResource1" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtAreaCode" runat="server" ReadOnly="True" meta:resourcekey="txtAreaCodeResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="image9" runat="server" SkinID="RequireFieldNon" />
                                <asp:Localize runat="server" ID="litQuoCFStatus" Text="Quotation Confirm Status:"
                                    meta:resourcekey="litQuoCFStatusResource1" />
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="cbQuoCFStatus" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="image10" runat="server" SkinID="RequireField" />
                                <asp:Localize runat="server" ID="litAutoInstockPartSpan" Text="Auto Instock part span:"
                                    meta:resourcekey="litAutoInstockPartSpanResource1" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbAIPS_d" runat="server" MaxLength="3" Width="50px" Text="10" CssClass="number"></asp:TextBox>
                                <asp:Literal runat="server" ID="ltAIPS_d" Text="days" meta:resourcekey="ltAIPS_dResource1"></asp:Literal>
                                &nbsp;
                                <asp:TextBox ID="tbAIPS_h" runat="server" MaxLength="6" Width="50px" Text="10" CssClass="number"></asp:TextBox>
                                <asp:Literal runat="server" ID="ltAIPS_h" Text="hours" meta:resourcekey="ltAIPS_hResource1"></asp:Literal>
                                <asp:RequiredFieldValidator ID="rfv4" runat="server" ControlToValidate="tbAIPS_d"
                                    ValidationGroup="Save" ErrorMessage="Auto instock part span cannot be empty!"
                                    SetFocusOnError="True" Text="*" meta:resourcekey="rfv4Resource1"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfv4a" runat="server" ControlToValidate="tbAIPS_h"
                                    ValidationGroup="Save" ErrorMessage="Auto instock part span cannot be empty!"
                                    SetFocusOnError="True" Text="*" meta:resourcekey="rfv4Resource1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="image11" runat="server" SkinID="RequireFieldNon" />
                                <asp:Localize runat="server" ID="litAutoInstockPartStatus" Text="Auto Instock part status:"
                                    meta:resourcekey="litAutoInstockPartStatusResource1" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbAIPS" runat="server" />
                            &nbsp;(ON/OFF)
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="image12" runat="server" SkinID="RequireField" />
                                <asp:Localize runat="server" ID="litAutoInstockVehicleSpan" Text="Auto Instock vehicle span:"
                                    meta:resourcekey="litAutoInstockVehicleSpanResource1" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbAIVS_d" runat="server" MaxLength="3" Width="50px" Text="10" CssClass="number"></asp:TextBox>
                                <asp:Literal runat="server" ID="ltAIVS_d" Text="days" meta:resourcekey="ltAIPS_dResource1"></asp:Literal>&nbsp;
                                <asp:TextBox ID="tbAIVS_h" runat="server" MaxLength="6" Width="50px" Text="10" CssClass="number"></asp:TextBox>
                                <asp:Literal runat="server" ID="ltAIVS_h" Text="hours" meta:resourcekey="ltAIPS_hResource1"></asp:Literal>
                                <asp:RequiredFieldValidator ID="rfv5" runat="server" ControlToValidate="tbAIVS_d"
                                    ValidationGroup="Save" ErrorMessage="Auto instock vehicle span cannot be empty!"
                                    SetFocusOnError="True" Text="*" meta:resourcekey="rfv5Resource1"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfv5a" runat="server" ControlToValidate="tbAIVS_h"
                                    ValidationGroup="Save" ErrorMessage="Auto instock vehicle span cannot be empty!"
                                    SetFocusOnError="True" Text="*" meta:resourcekey="rfv5Resource1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="image13" runat="server" SkinID="RequireFieldNon" />
                                <asp:Localize runat="server" ID="litAutoInstockVehicleStatus" Text="Auto Instock vehicle status:"
                                    meta:resourcekey="litAutoInstockVehicleStatusResource1" />
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="cbAIVS" />&nbsp;(ON/OFF)
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal runat="server" ID="ltformdate" Text="auto instock vehice startdate:" meta:resourcekey="ltfromDatedResource1"></asp:Literal>&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromDate" runat="server" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Save" ID="RequiredFieldValidator1"
                                    runat="server" SetFocusOnError="True" ControlToValidate="txtFromDate" ErrorMessage="Report date cannot be blank!"
                                    meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                                <asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                                    meta:resourcekey="ibFromDateResource1" />
                                <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                                    Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                    CultureTimePlaceholder="" Enabled="True">
                                </ajaxToolkit:MaskedEditExtender>
                                <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtFromDate"
                                    PopupButtonID="ibFromDate" Enabled="True">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                </div>
                <table cellpadding="2" cellspacing="4" border="0">
                    <tr>
                        <td style="vertical-align: top">
                            <div class="grid" style="width: 450px; float: left;">
                                <vdms:PageGridView ID="gvVBo" runat="server" Caption="Vehicle Branches" meta:resourcekey="gvVBoResource1">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Active" meta:resourcekey="TemplateFieldResource3x">
                                            <ItemTemplate>
                                                <asp:CheckBox Checked='<%# IsActive(Eval("Status"), "V") %>' ID="chbActive" Code='<%#Eval("Code") %>'
                                                    Address='<%#Eval("Address") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Code" HeaderText="Warehouse Code" meta:resourcekey="BoundFieldResource1">
                                            <HeaderStyle Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Address" HeaderText="Address" HtmlEncode="False" meta:resourcekey="BoundFieldResource2" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <b>
                                            <asp:Localize ID="litNotFound" runat="server" Text="There are not any warehouses."
                                                meta:resourcekey="litNotFoundResource1"></asp:Localize></b>
                                    </EmptyDataTemplate>
                                </vdms:PageGridView>
                                <vdms:PageGridView ID="gvVBn" runat="server" Caption="TipTop Vehicle Branches" meta:resourcekey="gvVBnResource1">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select" meta:resourcekey="TemplateFieldResource1">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chbSelected" Checked='<%# IsNeedAddNew(Eval("Code"), "V") %>' Code='<%#Eval("Code") %>'
                                                    Address='<%#Eval("Address") %>' runat="server" meta:resourcekey="chbSelectedResource1" />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Update for" Visible="False" meta:resourcekey="TemplateFieldResource2">
                                            <ItemTemplate>
                                                <vdms:WarehouseList ID="ddlWarehouse" Type="V" DealerCode='<%# CurrentDealerCode %>'
                                                    ShowEmptyItem="True" runat="server" DontAutoUseCurrentSealer="False" meta:resourcekey="ddlWarehouseResource1"
                                                    ShowSelectAllItem="False" UseVIdAsValue="False">
                                                </vdms:WarehouseList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Code" HeaderText="Warehouse Code" meta:resourcekey="BoundFieldResource3">
                                            <HeaderStyle Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Address" HeaderText="Address" HtmlEncode="False" meta:resourcekey="BoundFieldResource4" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <b>
                                            <asp:Localize ID="litNotFound0" runat="server" Text="There are not any warehouses."
                                                meta:resourcekey="litNotFound0Resource1"></asp:Localize>
                                        </b>
                                    </EmptyDataTemplate>
                                </vdms:PageGridView>
                            </div>
                        </td>
                        <td style="vertical-align: top">
                            <div class="grid" style="width: 450px; float: left;">
                                <vdms:PageGridView ID="gvPBo" runat="server" Caption="Part Branches" meta:resourcekey="gvPBoResource1">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Active" meta:resourcekey="TemplateFieldResource3x">
                                            <ItemTemplate>
                                                <asp:CheckBox Checked='<%# IsActive(Eval("Status"), "P") %>' ID="chbActive" Code='<%#Eval("Code") %>'
                                                    Address='<%#Eval("Address") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Code" HeaderText="Warehouse Code" meta:resourcekey="BoundFieldResource5">
                                            <HeaderStyle Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Address" HeaderText="Address" HtmlEncode="False" meta:resourcekey="BoundFieldResource6" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <b>
                                            <asp:Localize ID="litNotFound" runat="server" Text="There are not any warehouses."
                                                meta:resourcekey="litNotFoundResource2"></asp:Localize></b>
                                    </EmptyDataTemplate>
                                </vdms:PageGridView>
                                <vdms:PageGridView ID="gvPBn" runat="server" Caption="TipTop Part Branches" meta:resourcekey="gvPBnResource1">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select" meta:resourcekey="TemplateFieldResource3">
                                            <ItemTemplate>
                                                <asp:CheckBox Checked='<%# IsNeedAddNew(Eval("Code"), "P") %>' ID="chbSelected" Code='<%#Eval("Code") %>'
                                                    Address='<%#Eval("Address") %>' runat="server" meta:resourcekey="chbSelectedResource2" />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Update for" Visible="False" meta:resourcekey="TemplateFieldResource4">
                                            <ItemTemplate>
                                                <vdms:WarehouseList ID="ddlWarehouse" Type="P" DealerCode='<%# CurrentDealerCode %>'
                                                    ShowEmptyItem="True" runat="server" DontAutoUseCurrentSealer="False" meta:resourcekey="ddlWarehouseResource2"
                                                    ShowSelectAllItem="False" UseVIdAsValue="False">
                                                </vdms:WarehouseList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Code" HeaderText="Warehouse Code" meta:resourcekey="BoundFieldResource7">
                                            <HeaderStyle Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Address" HeaderText="Address" HtmlEncode="False" meta:resourcekey="BoundFieldResource8" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <b>
                                            <asp:Localize ID="litNotFound0" runat="server" Text="There are not any warehouses."
                                                meta:resourcekey="litNotFound0Resource2"></asp:Localize>
                                        </b>
                                    </EmptyDataTemplate>
                                </vdms:PageGridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="t3" runat="server" HeaderText="Additional Data" meta:resourcekey="t3Resource1">
            <ContentTemplate>
                <div class="form" style="width: 450px">
                    <cc1:ContactInfo ID="ci" runat="server" />
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
    <br />
    <asp:Button ID="b1" runat="server" Text="Save" Enabled="False" ValidationGroup="Save"
        OnClick="b1_Click" meta:resourcekey="b1Resource2" />
    <asp:Button ID="b2" runat="server" Text="Back" OnClientClick="javascript:location.href='Dealer.aspx'; return false;"
        meta:resourcekey="b2Resource2" />
    <br />
    <asp:Label ID="lblSaveOk" runat="server" SkinID="MessageOk" Visible="False" Text="The dealer has been created."
        meta:resourcekey="lblSaveOkResource1"></asp:Label>
    <asp:Label ID="lblWarehouseNotFound" runat="server" SkinID="MessageError" Visible="False"
        Text="Cannot create dealer. Warehouse not found." meta:resourcekey="lblWarehouseNotFoundResource1"></asp:Label>
</asp:Content>
