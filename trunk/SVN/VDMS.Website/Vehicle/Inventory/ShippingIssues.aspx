<%@ Page Title="Shipping issues" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="ShippingIssues.aspx.cs" Inherits="Vehicle_Inventory_ShippingIssues" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <asp:ListView ID="lv" runat="server" DataSourceID="ObjectDataSource1">
        <LayoutTemplate>
            <div id="grid" class="grid">
                <div class="title">
                    <asp:Literal ID="litTitle" OnLoad="litTitle_Load" runat="server" 
                        Text="Shipping issues for order: {0}" meta:resourcekey="litTitleResource1"></asp:Literal>
                </div>
                <table class="datatable" cellpadding="0" cellspacing="0">
                    <thead>
                        <tr>
                            <th>
                                <asp:Literal ID="Literal2" runat="server" Text="Item Type" 
                                    meta:resourcekey="Literal2Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal6" runat="server" Text="Item Code" 
                                    meta:resourcekey="Literal6Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal7" runat="server" Text="Item Name" 
                                    meta:resourcekey="Literal7Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal3" runat="server" Text="Color" 
                                    meta:resourcekey="Literal3Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal4" runat="server" Text="EngineNumber" 
                                    meta:resourcekey="Literal4Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal5" runat="server" Text="Status" 
                                    meta:resourcekey="Literal5Resource1"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="Literal11" runat="server" Text="Import note" 
                                    meta:resourcekey="Literal11Resource1"></asp:Literal>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                        <tr>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="7">
                                <vdms:DataPager runat="server" ID="DataPager" PagedControlID="lv">
                                </vdms:DataPager>
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <tr class="group">
                <td colspan="3" align="left">
                    <table class="info">
                        <tr>
                            <th>
                                <asp:Literal ID="Literal1" runat="server" Text="Issue No:" 
                                    meta:resourcekey="Literal1Resource1"></asp:Literal>
                            </th>
                            <td>
                                <%# Eval("IssueNumber")%>
                                <%--# Eval("ShippingNumber")--%>
                            </td>
                            <th>
                                <asp:Literal ID="Literal8" runat="server" Text="Shipping Date:" 
                                    meta:resourcekey="Literal8Resource1"></asp:Literal>
                            </th>
                            <td>
                                <%# EvalDate(Eval("ShipDate"))%>
                                <%--# EvalDate(Eval("ShippingDate"))--%>
                            </td>
                            <th>
                                <asp:Literal ID="Literal9" runat="server" Text="Warehouse:" 
                                    meta:resourcekey="Literal9Resource1"></asp:Literal>
                            </th>
                            <td>
                                <%# Eval("BranchCode")%>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <asp:Literal ID="Literal10" runat="server" Text="Address:" 
                                    meta:resourcekey="Literal10Resource1"></asp:Literal>
                            </th>
                            <td colspan="4">
                                <%# Eval("ShippingAddress")%>
                            </td>
                            <td>
                                <%--<asp:HyperLink NavigateUrl='<# string.Format("Import.aspx?sn={0}&on={1}", Eval("ShippingNumber"),  OrderNumber) %>'--%>
                                <asp:HyperLink NavigateUrl='<%# string.Format("Import.aspx?sn={0}&on={1}", Eval("IssueNumber"),  OrderNumber) %>'
                                    ID="lnkImport" Text="Import" runat="server" 
                                    meta:resourcekey="lnkImportResource1" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td colspan="4" align="left">
                <table class="info">
                        <tr>
                            <th>
                                <asp:Literal ID="Literal12" runat="server" Text="Car Number:" 
                                    meta:resourcekey="Literal12Resource1"></asp:Literal>
                            </th>
                            <td>
                                <%# GetInfoIssueDetail("car_number",Eval("IssueNumber").ToString())%>
                                <%--# Eval("ShippingNumber")--%>
                            </td>
                            <th>
                                <asp:Literal ID="Literal13" runat="server" Text="Driver name:" 
                                    meta:resourcekey="Literal13Resource1"></asp:Literal>
                            </th>
                            <td>
                                <%# GetInfoIssueDetail("driver_name", Eval("IssueNumber").ToString())%>
                                <%--# EvalDate(Eval("ShippingDate"))--%>
                            </td>
                            <th>
                                <asp:Literal ID="Literal14" runat="server" Text="Driver tel:" 
                                    meta:resourcekey="Literal14Resource1"></asp:Literal>
                            </th>
                            <td>
                                <%# GetInfoIssueDetail("driver_tel", Eval("IssueNumber").ToString())%>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <asp:Literal ID="Literal15" runat="server" Text="Exec Name:" 
                                    meta:resourcekey="Literal15Resource1"></asp:Literal>
                            </th>
                            <td>
                                <%# GetInfoIssueDetail("exec_name", Eval("IssueNumber").ToString())%>
                            </td>
                            <th>
                                <asp:Literal ID="Literal16" runat="server" Text="Exec Tel:" 
                                    meta:resourcekey="Literal16Resource1"></asp:Literal>
                            </th>
                            <td >
                                <%# GetInfoIssueDetail("exec_tel", Eval("IssueNumber").ToString())%>
                            </td>
                            <td>
                                <%--<asp:HyperLink NavigateUrl='<# string.Format("Import.aspx?sn={0}&on={1}", Eval("ShippingNumber"),  OrderNumber) %>'--%>
                                <asp:HyperLink NavigateUrl='<%# string.Format("ShowShippingInfo.aspx?issue={0}", Eval("IssueNumber")) %>'
                                    ID="HyperLink1" Text="Import" runat="server" 
                                    meta:resourcekey="lnkDetailResource1" CssClass="thickbox" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--<asp:ListView ID="lvItems" runat="server" DataSource='<# GetIssueDetail((string)Eval("ShippingNumber")) %>'>--%>
            <asp:ListView ID="lvItems" runat="server" DataSource='<%# GetIssueDetail((string)Eval("IssueNumber")) %>'>
                <LayoutTemplate>
                    <tr runat="server" id="itemPlaceholder" />
                </LayoutTemplate>
                <ItemTemplate>
                    <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
                        <td>
                            <%#Eval("Model")%>
                            <%--#Eval("ItemType")--%>
                        </td>
                        <td>
                            <%#Eval("ItemCode")%>
                        </td>
                        <td>
                            <%#Eval("ItemName")%>
                        </td>
                        <td>
                            <%#Eval("ColorCode")%>
                            <%--#Eval("Color")--%>
                        </td>
                        <td>
                            <%#Eval("EngineNumber")%>
                        </td>
                        <td>
                            <%#EvalStatus((string)Eval("EngineNumber"))%>
                        </td>
                        <td>
                            <%#EvalImportNote((string)Eval("EngineNumber"))%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </ItemTemplate>
        <EmptyDataTemplate>
            <asp:Literal ID="Literal1" runat="server" 
                Text="No issues found. May be, order was not confirmed!" 
                meta:resourcekey="Literal1Resource2"></asp:Literal>
        </EmptyDataTemplate>
    </asp:ListView>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="Select"
        SelectCountMethod="SelectCount" EnablePaging="True" 
            TypeName="VDMS.I.ObjectDataSource.ShippingHeaderDataSource">
        <SelectParameters>
            <asp:QueryStringParameter Name="orderNumber" QueryStringField="on" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
