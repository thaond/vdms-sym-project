<%@ Page Title="Part Setting" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="Part.aspx.cs" Inherits="Admin_Part_Part" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register TagPrefix="cc1" Namespace="VDMS.II.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <%--    <ajaxToolkit:TabContainer ID="t" runat="server" CssClass="ajax__tab_technorati-theme"
        ActiveTabIndex="0">
        <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Parts setting">
            <ContentTemplate>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="msg" runat="server">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="form" style="width: 450px">
        <table width="100%">
            <tr>
                <td style="width: 25%;">
                    <asp:Localize ID="Localize3" runat="server" Text="Mode:" meta:resourcekey="Localize3Resource1"></asp:Localize>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblMode" runat="server" RepeatDirection="Horizontal" meta:resourcekey="rblModeResource1">
                        <asp:ListItem Selected="True" Text="Querry" Value="Q" meta:resourcekey="ListItemResource1"></asp:ListItem>
                        <asp:ListItem Text="Edit" Value="E" meta:resourcekey="ListItemResource2"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td style="width: 25%;">
                    <asp:Localize ID="litType" runat="server" Text="Type:" meta:resourcekey="litTypeResource1"></asp:Localize>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rbl1_SelectedIndexChanged"
                        RepeatDirection="Horizontal" meta:resourcekey="rblTypeResource1">
                        <asp:ListItem Selected="True" Text="<%$ Resources:TextMsg, Part %>" Value="P" meta:resourcekey="ListItemResource3"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:TextMsg, Accessory %>" Value="A" meta:resourcekey="ListItemResource4"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Localize ID="litCategory" runat="server" Text="Category:" meta:resourcekey="litCategoryResource1"></asp:Localize>
                </td>
                <td>
                    <cc1:CategoryList ShowSelectAllItem="True" ID="ddlCategory" runat="server" meta:resourcekey="ddlCategoryResource1"
                        ShowNullItemIfSelectFailed="False">
                    </cc1:CategoryList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Localize ID="Localize2" runat="server" Text="Favorite status:" meta:resourcekey="Localize2Resource1"></asp:Localize>
                </td>
                <td>
                    <asp:DropDownList ShowSelectAllItem="true" ID="ddlFavStatus" runat="server" meta:resourcekey="ddlFavStatusResource1">
                        <asp:ListItem Value="" Text="All" meta:resourcekey="ListItemResource5"></asp:ListItem>
                        <asp:ListItem Value="Y" Text="Set" meta:resourcekey="ListItemResource6"></asp:ListItem>
                        <asp:ListItem Value="N" Text="Not set" meta:resourcekey="ListItemResource7"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Localize ID="litName" runat="server" Text="Part/Accessory No:" meta:resourcekey="litNameResource1"></asp:Localize>
                </td>
                <td>
                    <asp:TextBox ID="txtPartNo" runat="server" meta:resourcekey="txtPartNoResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Localize ID="Localize1" runat="server" Text="Part/Accessory Name:" meta:resourcekey="Localize1Resource1"></asp:Localize>
                </td>
                <td>
                    <asp:TextBox ID="txtPartName" runat="server" meta:resourcekey="txtPartNameResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <asp:Button ID="bQuery" runat="server" Text="Query" OnClick="bQuery_Click" meta:resourcekey="bQueryResource1" />
        <asp:Button ID="bAddNew" runat="server" Text="Add New Item" OnClientClick="javascript:location.href='PartEdit.aspx?type=C'; return false;"
            Enabled="False" meta:resourcekey="bAddNewResource1" />
    </div>
    <br />
    <div class="grid">
        <asp:ListView ID="lvParts" runat="server">
            <LayoutTemplate>
                <div id="grid" class="grid">
                    <div class="title">
                        <asp:Literal ID="litTittle" runat="server" Text="Parts list" meta:resourcekey="litTittleResource1"></asp:Literal></div>
                    <table class="datatable" cellpadding="0" cellspacing="0">
                        <thead>
                            <tr>
                                <th rowspan="2" class="double">
                                    <asp:Literal ID="Literal1" runat="server" Text="Part code" meta:resourcekey="Literal1Resource1"></asp:Literal>
                                </th>
                                <th colspan="2">
                                    <asp:Literal ID="Literal2" runat="server" Text="Part Name" meta:resourcekey="Literal2Resource1"></asp:Literal>
                                </th>
                                <th colspan="2">
                                    <asp:Literal ID="Literal3" runat="server" Text="Favorite rank" meta:resourcekey="Literal3Resource1"></asp:Literal>
                                </th>
                                <th rowspan="2" class="double">
                                    <asp:Literal ID="Literal4" runat="server" Text="Category" meta:resourcekey="Literal4Resource1"></asp:Literal>
                                </th>
                                <th rowspan="2" class="double">
                                    <asp:Literal ID="Literal5" runat="server" Text="Part type" meta:resourcekey="Literal5Resource1"></asp:Literal>
                                </th>
                                <th rowspan="2" class="double">
                                    <asp:Literal ID="Literal6" runat="server" Text="Unit price" meta:resourcekey="Literal6Resource1"></asp:Literal>
                                </th>
                                <th colspan="3">
                                    <asp:Literal ID="Literal7" runat="server" Text="Detail" meta:resourcekey="Literal7Resource1"></asp:Literal>
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Literal ID="Literal8" runat="server" Text="English" meta:resourcekey="Literal8Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal9" runat="server" Text="Vietnamese" meta:resourcekey="Literal9Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal10" runat="server" Text="Sale" meta:resourcekey="Literal10Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal11" runat="server" Text="Order" meta:resourcekey="Literal11Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal12" runat="server" Text="Warehouse" meta:resourcekey="Literal12Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal13" runat="server" Text="Instock" meta:resourcekey="Literal13Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal14" runat="server" Text="Safety" meta:resourcekey="Literal14Resource1"></asp:Literal>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr class="pager">
                                <td colspan="10">
                                    <vdms:DataPager ID="DataPager1" runat="server" PagedControlID="lvParts">
                                    </vdms:DataPager>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="10" class="center">
                                    <asp:Button OnLoad="btnSave_OnLoad" ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"
                                        meta:resourcekey="btnSaveResource1" />
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
                    <td align="left" rowspan='<%# GetSafetyRows((long)Eval("PartInfoId")) %>'>
                        <%#Eval("PartCode")%>
                    </td>
                    <td align="left" rowspan='<%# lastRowsCount %>'>
                        <asp:MultiView ID="MultiView1" runat="server" OnLoad="SelectModeAndA">
                            <asp:View ID="vwEdit" runat="server">
                                <asp:TextBox ID="txtEName" Target="E" OnTextChanged="txtPartName_OnTextChanged" runat="server"
                                    AID='<%#Eval("AccessoryId") %>' Text='<%# Eval("EName") %>' meta:resourcekey="txtENameResource1"></asp:TextBox>
                            </asp:View>
                            <asp:View ID="vwQuery" runat="server">
                                <%#Eval("EName")%>
                            </asp:View>
                        </asp:MultiView>
                    </td>
                    <td align="left" rowspan='<%# lastRowsCount %>'>
                        <asp:MultiView ID="MultiView2" runat="server" OnLoad="SelectModeAndA">
                            <asp:View ID="vwEdit2" runat="server">
                                <asp:TextBox ID="txtVName" Target="V" OnTextChanged="txtPartName_OnTextChanged" runat="server"
                                    AID='<%#Eval("AccessoryId") %>' Text='<%# Eval("VName") %>' meta:resourcekey="txtVNameResource1"></asp:TextBox>
                            </asp:View>
                            <asp:View ID="vwQuery2" runat="server">
                                <%#Eval("VName")%>
                            </asp:View>
                        </asp:MultiView>
                    </td>
                    <td class="center" rowspan='<%# lastRowsCount %>'>
                        <asp:MultiView ID="MultiView3" runat="server" OnLoad="SelectMode">
                            <asp:View ID="View1" runat="server">
                                <vdms:FavoriteRankList OnDataBound="EnableEditFav" ShowEmptyItem="True" ID="ddlSaleFavRank"
                                    Type="S" runat="server" PC='<%#Eval("PartCode")%>' PT='<%#Eval("PartType")%>'
                                    FavID='<%#Eval("SaleFav.FavoriteId")%>' BindingSelectedValue='<%# Eval("SaleFav.Rank") %>'
                                    OnSelectedIndexChanged="FavoriteIndexChanged" meta:resourcekey="ddlSaleFavRankResource1">
                                </vdms:FavoriteRankList>
                            </asp:View>
                            <asp:View ID="View2" runat="server">
                                <%#EvalFav(Eval("SaleFav.Rank"))%>
                            </asp:View>
                        </asp:MultiView>
                    </td>
                    <td class="center" rowspan='<%# lastRowsCount %>'>
                        <asp:MultiView ID="MultiView4" runat="server" OnLoad="SelectModeAndP">
                            <asp:View ID="View3" runat="server">
                                <vdms:FavoriteRankList OnDataBound="EnableEditFav" ShowEmptyItem="True" ID="ddlOrderFav"
                                    PC='<%#Eval("PartCode")%>' PT='<%#Eval("PartType")%>' FavID='<%#Eval("SaleFav.FavoriteId")%>'
                                    runat="server" OnSelectedIndexChanged="FavoriteIndexChanged" BindingSelectedValue='<%# Eval("OrderFav.Rank") %>'
                                    Type="O" meta:resourcekey="ddlOrderFavResource1">
                                </vdms:FavoriteRankList>
                            </asp:View>
                            <asp:View ID="View4" runat="server">
                                <%#EvalFav(Eval("OrderFav.Rank"))%>
                            </asp:View>
                        </asp:MultiView>
                    </td>
                    <td rowspan='<%# lastRowsCount %>'>
                        <asp:MultiView ID="MultiView6" runat="server" OnLoad="SelectModeAndA">
                            <asp:View ID="View7" runat="server">
                                <vdms:CategoryList ID="ddlCat" runat="server" CategoryType="A" ShowNullItemIfSelectFailed="True"
                                    AID='<%#Eval("AccessoryId") %>' OnSelectedIndexChanged="ddlCat_SelectedIndexChanged"
                                    BindingSelectedValue='<%# EvalCatID(Eval("Category")) %>' meta:resourcekey="ddlCatResource1"
                                    ShowSelectAllItem="False">
                                </vdms:CategoryList>
                            </asp:View>
                            <asp:View ID="View8" runat="server">
                                <%#EvalCatName(Eval("Category"), Eval("TipTopCategory"))%>
                            </asp:View>
                        </asp:MultiView>
                    </td>
                    <asp:PlaceHolder ID="phAcc2" runat="server" Visible='<%# IsForAccessory() %>'>
                        <td align="left" rowspan='<%# lastRowsCount %>'>
                            <asp:MultiView ID="MultiView5" runat="server" OnLoad="SelectModeAndA">
                                <asp:View ID="View5" runat="server">
                                    <vdms:AccessoryTypeList ID="ddlAccType" runat="server" OnSelectedIndexChanged="ddlAccType_SelectedIndexChanged"
                                        AID='<%#Eval("AccessoryId") %>' DefaultValue="SYM" BindingSelectedValue='<%# Eval("AccType.AccessoryTypeCode") %>'
                                        meta:resourcekey="ddlAccTypeResource1" ShowSelectAllItem="False">
                                    </vdms:AccessoryTypeList>
                                </asp:View>
                                <asp:View ID="View6" runat="server">
                                    <%#Eval("AccType.AccessoryTypeName")%>
                                </asp:View>
                            </asp:MultiView>
                        </td>
                        <td align="left" rowspan='<%# lastRowsCount %>'>
                            <asp:MultiView ID="MultiView7" runat="server" OnLoad="SelectModeAndA">
                                <asp:View ID="View9" runat="server">
                                    <asp:TextBox OnTextChanged="txtUnitPrice_OnTextChanged" CssClass="number" Width="80px"
                                        PIID='<%#Eval("PartInfoId")%>' ID="txtUnitPrice" runat="server" 
                                        Text='<%# Eval("UnitPrice") %>' meta:resourcekey="txtUnitPriceResource1"></asp:TextBox>
                                </asp:View>
                                <asp:View ID="View10" runat="server">
                                    <%#Eval("UnitPrice")%>
                                </asp:View>
                            </asp:MultiView>
                        </td>
                    </asp:PlaceHolder>
                    <asp:ListView ID="lvItems" runat="server" DataSource='<%# GetPartSafetyList((long)Eval("PartInfoId")) %>'>
                        <LayoutTemplate>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <asp:PlaceHolder ID="phDetailRow" runat="server" Visible='<%# Container.DisplayIndex > 0 %>'>
                                <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
                            </asp:PlaceHolder>
                            <td>
                                <%#Eval("Warehouse.Code")%>
                            </td>
                            <td class="number">
                                <asp:Label CssClass='<%# ((int)Eval("SafetyQuantity") > (int)Eval("CurrentStock"))? "impNotice" : "" %>'
                                    ID="lbCurrentStock" runat="server" Text='<%# Eval("CurrentStock") %>' meta:resourcekey="lbCurrentStockResource1"></asp:Label>
                            </td>
                            <td class="number">
                                <asp:MultiView ID="MultiView2" runat="server" OnLoad="SelectMode">
                                    <asp:View ID="vwEdit2" runat="server">
                                        <asp:TextBox OnTextChanged="txtSafetyQuantity_OnTextChanged" CssClass="number" Width="50px"
                                            WHID='<%#Eval("Warehouse.WarehouseId")%>' PIID='<%#Eval("PartInfoId")%>' ID="txtSafetyQuantity"
                                            runat="server" Text='<%# Eval("SafetyQuantity") %>' meta:resourcekey="txtSafetyQuantityResource1"></asp:TextBox>
                                    </asp:View>
                                    <asp:View ID="vwQuery2" runat="server">
                                        <%#Eval("SafetyQuantity")%>
                                    </asp:View>
                                </asp:MultiView>
                            </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsParts" runat="server" TypeName="VDMS.II.PartManagement.PartInfoDAO"
            EnablePaging="True" SelectCountMethod="CountForSetting" SelectMethod="FindForSetting">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtPartNo" Name="partCode" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="txtPartName" Name="partName" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="ddlCategory" Name="categoryId" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="rblType" Name="type" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="ddlFavStatus" Name="favStatus" PropertyName="SelectedValue"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    <%--            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Upload Excel file">
            <ContentTemplate>
                <div class="form" style="width: 450px">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Localize ID="litFileName" runat="server" Text="Filename:"></asp:Localize>
                            </td>
                            <td>
                                <asp:FileUpload ID="fu" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="b1" runat="server" Text="Upload" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>--%>
</asp:Content>
