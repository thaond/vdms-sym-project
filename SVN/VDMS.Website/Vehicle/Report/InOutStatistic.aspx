<%@ Page Title="Imported/Sold-out report" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="InOutStatistic.aspx.cs" Inherits="Vehicle_Report_InOutStatistic" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
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
            <%--<tr>
                <td>
                    <asp:Literal ID="Literal11" runat="server" Text="Dealer code:"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtDealer" runat="server"></asp:TextBox>
                </td>
            </tr>--%>
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Date:" 
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
                    &nbsp;
                </td>
                <td>
                    <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" 
                        RepeatLayout="Flow" meta:resourcekey="rblTypeResource1">
                        <asp:ListItem Selected="True" Value="Detail" 
                            meta:resourcekey="ListItemResource1">Detail  </asp:ListItem>
                        <asp:ListItem Value="Summary" meta:resourcekey="ListItemResource2">Summary  </asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" 
                        meta:resourcekey="btnFindResource1" />
                    &nbsp;<asp:Button ID="btnExcel" runat="server" Text="Export excel" 
                        OnClick="btnExcel_Click" meta:resourcekey="btnExcelResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <vdms:PageGridView ID="gv" PageSize="30" runat="server" AllowPaging="True" 
            onrowdatabound="gv_RowDataBound" meta:resourcekey="gvResource1">
            <columns>
                <asp:BoundField DataField="DealerCode" HeaderText="DealerCode" 
                    SortExpression="DealerCode" meta:resourcekey="BoundFieldResource1" />
                <asp:BoundField DataField="DealerCode" HeaderText="DealerName" 
                    SortExpression="DealerName" meta:resourcekey="BoundFieldResource2" />
                <asp:BoundField DataField="OrderNumber" HeaderText="OrderNumber" 
                    SortExpression="OrderNumber" meta:resourcekey="BoundFieldResource3" />
                <asp:BoundField DataField="OrderDate" HeaderText="OrderDate" 
                    SortExpression="OrderDate" DataFormatString="{0:d}" 
                    meta:resourcekey="BoundFieldResource4" />
                <asp:BoundField DataField="ItemType" HeaderText="ItemType" 
                    SortExpression="ItemType" meta:resourcekey="BoundFieldResource5" />
                <asp:BoundField DataField="In" HeaderText="In" SortExpression="In" 
                    DataFormatString="{0:N0}" meta:resourcekey="BoundFieldResource6">
                    <ItemStyle CssClass="number" />
                </asp:BoundField>
                <asp:BoundField DataField="Out" HeaderText="Out" SortExpression="Out" 
                    DataFormatString="{0:N0}" meta:resourcekey="BoundFieldResource7">
                    <ItemStyle CssClass="number" />
                </asp:BoundField>
            </columns>
        </vdms:PageGridView>
        <asp:ObjectDataSource ID="odsDetail" runat="server" SelectCountMethod="CountDetailInOutReport"
            EnablePaging="True" SelectMethod="GetDetailInOutReport" 
            TypeName="VDMS.I.Report.InOutStatisticReport">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlArea" Name="area" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="txtFrom" Name="from" PropertyName="Text" Type="DateTime" />
                <asp:ControlParameter ControlID="txtTo" Name="to" PropertyName="Text" Type="DateTime" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ListView ID="lv" runat="server">
            <LayoutTemplate>
                <table class="datatable" cellspacing="0">
                    <tr>
                        <th rowspan="2" class="double">
                            <asp:Literal ID="Literal2" runat="server" Text="Dealer code" 
                                meta:resourcekey="Literal2Resource1"></asp:Literal>
                        </th>
                        <th rowspan="2" class="double">
                            <asp:Literal ID="Literal3" runat="server" Text="Dealer name" 
                                meta:resourcekey="Literal3Resource1"></asp:Literal>
                        </th>
                        <asp:ListView ID="lvH" runat="server" OnLoad="lvH_Load">
                            <LayoutTemplate>
                                <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <th colspan="2">
                                    <%# Eval("ItemType") %>
                                </th>
                            </ItemTemplate>
                        </asp:ListView>
                        <th colspan="2">
                            <asp:Literal ID="Literal4" runat="server" Text="Total" 
                                meta:resourcekey="Literal4Resource1"></asp:Literal>
                        </th>
                    </tr>
                    <tr>
                        <asp:ListView ID="lvH2" runat="server" OnLoad="lvH_Load">
                            <LayoutTemplate>
                                <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <th>
                                    <asp:Literal ID="Literal5" runat="server" Text="In" 
                                        meta:resourcekey="Literal5Resource1"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="Literal6" runat="server" Text="Out" 
                                        meta:resourcekey="Literal6Resource1"></asp:Literal>
                                </th>
                            </ItemTemplate>
                        </asp:ListView>
                        <th>
                            <asp:Literal ID="Literal5" runat="server" Text="In" 
                                meta:resourcekey="Literal5Resource2"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal6" runat="server" Text="Out" 
                                meta:resourcekey="Literal6Resource2"></asp:Literal>
                        </th>
                    </tr>
                    <tr id="itemPlaceHolder" runat="server">
                    </tr>
                    <tr>
                        <td runat="server" id="tdPager" onload="tdPager_Load">
                            <vdms:DataPager PageSize="30" PagedControlID="lv" ID="DataPager1" runat="server">
                            </vdms:DataPager>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>' 
                    title='<%# Eval("DealerCode") %>'>
                    <td>
                        <%# Eval("DealerCode")%>
                    </td>
                    <td>
                        <%# Eval("DealerName")%>
                    </td>
                    <asp:ListView ID="lvD" runat="server" DataSource='<%# Eval("Items") %>'>
                        <LayoutTemplate>
                            <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <td class="number altCol1">
                                <asp:Label Text='<%# Eval("In", "{0:N0}") %>' ID="lbQty" runat="server" ToolTip='<%# Eval("ItemType") %>'></asp:Label>
                            </td>
                            <td class="number altCol2">
                                <asp:Label Text='<%# Eval("Out", "{0:N0}") %>' ID="Label1" runat="server" ToolTip='<%# Eval("ItemType") %>'></asp:Label>
                            </td>
                        </ItemTemplate>
                    </asp:ListView>
                    <td class="number">
                        <%# Eval("TotalIn", "{0:N0}")%>
                    </td>
                    <td class="number">
                        <%# Eval("TotalOut", "{0:N0}")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <asp:ListView ID="lvExcel" runat="server">
            <LayoutTemplate>
                <table cellpadding="2" style="border-collapse: collapse;" border="1" rules="all">
                    <tr>
                        <th rowspan="2"  style="background-color: #DDDDDD; font-weight: bold;">
                            <asp:Literal ID="Literal2" runat="server" Text="Dealer code" 
                                meta:resourcekey="Literal2Resource2"></asp:Literal>
                        </th>
                        <th rowspan="2"  style="background-color: #DDDDDD; font-weight: bold;">
                            <asp:Literal ID="Literal3" runat="server" Text="Dealer name" 
                                meta:resourcekey="Literal3Resource2"></asp:Literal>
                        </th>
                        <asp:ListView ID="lvH" runat="server" OnLoad="lvH_Load">
                            <LayoutTemplate>
                                <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <th colspan="2" style="background-color: #DDDDDD; font-weight: bold;">
                                    <%# Eval("ItemType")%>
                                </th>
                            </ItemTemplate>
                        </asp:ListView>
                        <th colspan="2" style="background-color: #DDDDDD; font-weight: bold;">
                            <asp:Literal ID="Literal4" runat="server" Text="Total" 
                                meta:resourcekey="Literal4Resource2"></asp:Literal>
                        </th>
                    </tr>
                    <tr>
                        <asp:ListView ID="lvH2" runat="server" OnLoad="lvH_Load">
                            <LayoutTemplate>
                                <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <th style="background-color: #DDDDDD; font-weight: bold;">
                                    <asp:Literal ID="Literal5" runat="server" Text="In" 
                                        meta:resourcekey="Literal5Resource3"></asp:Literal>
                                </th>
                                <th style="background-color: #DDDDDD; font-weight: bold;">
                                    <asp:Literal ID="Literal6" runat="server" Text="Out" 
                                        meta:resourcekey="Literal6Resource3"></asp:Literal>
                                </th>
                            </ItemTemplate>
                        </asp:ListView>
                        <th style="background-color: #DDDDDD; font-weight: bold;">
                            <asp:Literal ID="Literal5" runat="server" Text="In" 
                                meta:resourcekey="Literal5Resource4"></asp:Literal>
                        </th>
                        <th style="background-color: #DDDDDD; font-weight: bold;">
                            <asp:Literal ID="Literal6" runat="server" Text="Out" 
                                meta:resourcekey="Literal6Resource4"></asp:Literal>
                        </th>
                    </tr>
                    <tr id="itemPlaceHolder" runat="server">
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr style='background-color: <%# Container.DisplayIndex % 2 == 0 ? "#f1f5fa;" : "#ffffff;" %>' 
                    title='<%# Eval("DealerCode") %>'>
                    <td>
                        <%# Eval("DealerCode")%>
                    </td>
                    <td>
                        <%# Eval("DealerName")%>
                    </td>
                    <asp:ListView ID="lvD" runat="server" DataSource='<%# Eval("Items") %>'>
                        <LayoutTemplate>
                            <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <td class="number altCol1">
                                <asp:Label Text='<%# Eval("In") %>' ID="lbQty" runat="server" ToolTip='<%# Eval("ItemType") %>'></asp:Label>
                            </td>
                            <td class="number altCol2">
                                <asp:Label Text='<%# Eval("Out") %>' ID="Label1" runat="server" ToolTip='<%# Eval("ItemType") %>'></asp:Label>
                            </td>
                        </ItemTemplate>
                    </asp:ListView>
                    <td class="number">
                        <%# Eval("TotalIn")%>
                    </td>
                    <td class="number">
                        <%# Eval("TotalOut")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsSummary" runat="server" SelectCountMethod="CountInOutStatisticItemsData"
            SelectMethod="GetInOutStatisticItemsData" TypeName="VDMS.I.Report.InOutStatisticReport"
            EnablePaging="True">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlArea" Name="area" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="txtFrom" Name="from" PropertyName="Text" Type="DateTime" />
                <asp:ControlParameter ControlID="txtTo" Name="to" PropertyName="Text" Type="DateTime" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
