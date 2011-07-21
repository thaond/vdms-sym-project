<%@ Page Title="Inventory report" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="inventory.aspx.cs" Inherits="PartReport_inventory" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
        meta:resourcekey="TabContainer1Resource1">
        <ajaxToolkit:TabPanel runat="server" ID="tabPart" 
            meta:resourcekey="tabPartResource1">
            <HeaderTemplate>
                
                    <asp:Literal ID="Literal7" runat="server" Text="Part from all dealers" 
                        meta:resourcekey="Literal7Resource1"></asp:Literal>
            </HeaderTemplate>
            <ContentTemplate>
                <div class="form" style="width: 50%;">
                    <table>
                        <asp:PlaceHolder ID="phPartType" runat="server" Visible="False">
                            <tr>
                                <td>
                                    <asp:Localize ID="litType" runat="server" Text="Type:" 
                                        meta:resourcekey="litTypeResource1"></asp:Localize>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" 
                                        meta:resourcekey="rblTypeResource1">
                                        <asp:ListItem Selected="True" Value="P" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                        <asp:ListItem Value="A" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal3" runat="server" Text="Dealer:" 
                                    meta:resourcekey="Literal3Resource1"></asp:Literal>
                            </td>
                            <td>
                                <vdms:DealerList AppendDataBoundItems="True" runat="server" ID="ddlDealers" ShowSelectAllItem="True"
                                    EnabledSaperateByDB="False" MergeCode="False" RemoveRootItem="False" 
                                    ShowEmptyItem="False" EnabledSaperateByArea="False" 
                                    meta:resourcekey="ddlDealersResource1">
                                </vdms:DealerList>
                            </td>
                        </tr>
                        <asp:PlaceHolder ID="phWarehouse" runat="server" Visible="False">
                            <tr>
                                <td>
                                    <asp:Literal ID="Literal4" runat="server" Text="Warehouse" 
                                        meta:resourcekey="Literal4Resource1"></asp:Literal>
                                </td>
                                <td>
                                    <vdms:WarehouseList ShowSelectAllItem="True" ID="ddlWarehouse" runat="server" 
                                        DontAutoUseCurrentSealer="False" ShowEmptyItem="False" 
                                        UseVIdAsValue="False" meta:resourcekey="ddlWarehouseResource1">
                                    </vdms:WarehouseList>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal2" runat="server" Text="Part No:" 
                                    meta:resourcekey="Literal2Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPartNo" runat="server" 
                                    meta:resourcekey="txtPartNoResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnViewPartFromDealers" runat="server" Text="View" 
                                    OnClick="btnViewPartFromDealers_Click" 
                                    meta:resourcekey="btnViewPartFromDealersResource1" />
                                <asp:Button ID="btnExcelViewPartFromDealers" runat="server" 
                                    Text="Export to excel" OnClick="btnExcelViewPartFromDealers_Click" 
                                    meta:resourcekey="btnExcelViewPartFromDealersResource1" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="grid">
                    <vdms:PageGridView runat="server" ID="gvPart" AllowPaging="True" 
                        onselectedindexchanging="gvPart_SelectedIndexChanging" 
                        meta:resourcekey="gvPartResource1">
                        <Columns>
                            <asp:BoundField DataField="DealerCode" HeaderText="Dealer code" 
                                meta:resourcekey="BoundFieldResource1" />
                            <asp:BoundField DataField="DealerName" HeaderText="Dealer name" 
                                meta:resourcekey="BoundFieldResource2" />
                            <asp:BoundField DataField="CurrentStock" HeaderText="Quantity" 
                                meta:resourcekey="BoundFieldResource3" />
                        </Columns>
                    </vdms:PageGridView>
                </div>
                <asp:ObjectDataSource ID="odsPartList" runat="server" EnablePaging="True" SelectMethod="SearchInstockByPart"
                    SelectCountMethod="CountInstockByPart" 
                        TypeName="VDMS.II.Report.InventoryReportDAO">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtPartNo" Name="partCode" Type="String" PropertyName="Text" />
                        <asp:ControlParameter ControlID="ddlDealers" Name="dealer" Type="String" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="rblType" Name="type" Type="String" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel runat="server" ID="tabParts" 
            meta:resourcekey="tabPartsResource1">
            <HeaderTemplate>
                
                    <asp:Literal ID="Literal8" runat="server" Text="Parts in a dealer" 
                        meta:resourcekey="Literal8Resource1"></asp:Literal>
            </HeaderTemplate>
            <ContentTemplate>
                <div class="form" style="width: 50%;">
                    <table>
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="False">
                            <tr>
                                <td>
                                    <asp:Localize ID="Localize1" runat="server" Text="Type:" 
                                        meta:resourcekey="Localize1Resource1"></asp:Localize>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblType2" runat="server" RepeatDirection="Horizontal" 
                                        meta:resourcekey="rblType2Resource1">
                                        <asp:ListItem Selected="True" Value="P" meta:resourcekey="ListItemResource3"></asp:ListItem>
                                        <asp:ListItem Value="A" meta:resourcekey="ListItemResource4"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal1" runat="server" Text="Dealer:" 
                                    meta:resourcekey="Literal1Resource1"></asp:Literal>
                            </td>
                            <td>
                                <vdms:DealerList AppendDataBoundItems="True" runat="server" ID="ddlDealers2" ShowSelectAllItem="False"
                                    AutoPostBack="True" OnDataBound="ddlDealers_SelectedIndexChanged" 
                                    OnSelectedIndexChanged="ddlDealers_SelectedIndexChanged" 
                                    EnabledSaperateByArea="False" EnabledSaperateByDB="False" MergeCode="False" 
                                    RemoveRootItem="False" ShowEmptyItem="False" 
                                    meta:resourcekey="ddlDealers2Resource1">
                                </vdms:DealerList>
                            </td>
                        </tr>
                        <asp:PlaceHolder ID="PlaceHolder2" runat="server">
                            <tr>
                                <td>
                                    <asp:Literal ID="Literal5" runat="server" Text="Warehouse" 
                                        meta:resourcekey="Literal5Resource1"></asp:Literal>
                                </td>
                                <td>
                                    <vdms:WarehouseList ShowSelectAllItem="True" ID="ddlWarehouses2" runat="server" 
                                        DontAutoUseCurrentSealer="False" ShowEmptyItem="False" 
                                        UseVIdAsValue="False" meta:resourcekey="ddlWarehouses2Resource1">
                                    </vdms:WarehouseList>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal6" runat="server" Text="Part No:" 
                                    meta:resourcekey="Literal6Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPartNo2" runat="server" 
                                    meta:resourcekey="txtPartNo2Resource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnViewPartsInDealer" runat="server" Text="View" 
                                    OnClick="btnViewPartsInDealer_Click" 
                                    meta:resourcekey="btnViewPartsInDealerResource1" />
                                <asp:Button ID="btnExcelViewPartsInDealer" runat="server" 
                                    Text="Export to excel" OnClick="btnExcelViewPartsInDealer_Click" 
                                    meta:resourcekey="btnExcelViewPartsInDealerResource1" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="grid">
                    <vdms:PageGridView runat="server" ID="gvParts" AllowPaging="True" 
                        onpageindexchanging="gvParts_PageIndexChanging" 
                        meta:resourcekey="gvPartsResource1">
                        <Columns>
                            <asp:BoundField DataField="PartCode" HeaderText="Part code" 
                                meta:resourcekey="BoundFieldResource4" />
                            <asp:BoundField DataField="EnglishName" HeaderText="English name" 
                                meta:resourcekey="BoundFieldResource5" />
                            <asp:BoundField DataField="VietNamName" HeaderText="Vietnamese Name" 
                                meta:resourcekey="BoundFieldResource6" />
                            <asp:BoundField DataField="CurrentStock" HeaderText="Quantity" 
                                meta:resourcekey="BoundFieldResource7" />
                        </Columns>
                    </vdms:PageGridView>
                    <asp:ObjectDataSource ID="odsPartsList" EnablePaging="True" runat="server" TypeName="VDMS.II.Report.InventoryReportDAO"
                        SelectCountMethod="CountPartInstock" SelectMethod="SearchPartInstock">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtPartNo2" Name="partCode" Type="String" PropertyName="Text" />
                            <asp:ControlParameter ControlID="ddlDealers2" Name="dealerCode" Type="String" PropertyName="SelectedValue" />
                            <asp:ControlParameter ControlID="ddlWarehouses2" Name="wId" Type="Int64" PropertyName="SelectedValue" />
                            <asp:ControlParameter ControlID="rblType2" Name="type" Type="String" PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
</asp:Content>
