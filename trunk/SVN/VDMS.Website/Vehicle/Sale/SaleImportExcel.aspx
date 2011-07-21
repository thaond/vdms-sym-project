<%@ Page Title="" Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="SaleImportExcel.aspx.cs" Inherits="Vehicle_Sale_SaleImportExcel" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div>
        <asp:Literal ID="Literal1" runat="server" meta:resourcekey="Literal1Resource1"></asp:Literal>
    </div>
    <asp:FileUpload ID="FileUpload1" runat="server" meta:resourcekey="FileUpload1Resource1" />
    <asp:Button ID="Button1" runat="server" Text="Upload" OnClick="Button1_OnClick" meta:resourcekey="Button1Resource1" />
    <br />
    <div id="msg" runat="server">
    </div>
    <br />
    <div class="grid">
        <asp:ListView ID="lvE" runat="server" OnDataBound="lvE_DataBound" OnItemDataBound="lvE_ItemDataBound">
            <LayoutTemplate>
                <table class="datatable" cellpadding="0" cellspacing="0">
                    <tr>
                        <th>
                            <asp:Literal ID="Literal23" runat="server" Text="No" meta:resourcekey="Literal23Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal11" runat="server" Text="Engine Number" meta:resourcekey="Literal11Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal12" runat="server" Text="Bill Number" meta:resourcekey="Literal12Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="ltImportDate" runat="server" Text="Import Date" meta:resourcekey="ltImportDateResource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal14" runat="server" Text="Sell Date" meta:resourcekey="Literal14Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal15" runat="server" Text="Price" meta:resourcekey="Literal15Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal17" runat="server" Text="Payment Type" meta:resourcekey="Literal17Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal2" runat="server" Text="Payment Date" meta:resourcekey="Literal2Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal18" runat="server" Text="Customer Name" meta:resourcekey="Literal18Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal19" runat="server" Text="Gender" meta:resourcekey="Literal19Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal20" runat="server" Text="Tel" meta:resourcekey="Literal20Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal21" runat="server" Text="Mobile" meta:resourcekey="Literal21Resource1"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal22" runat="server" Text="Address" meta:resourcekey="Literal22Resource1"></asp:Literal>
                        </th>
                        <th>
                        </th>
                    </tr>
                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                    <%--                    <td colspan="12">
                        <vdms:DataPager runat="server" ID="pager" PagedControlID="lvE" PageSize="20" DisablePaging="false">
                        </vdms:DataPager>
                    </td>--%>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
                    <td class="center">
                        <%# Container.DisplayIndex + 1%>
                    </td>
                    <td>
                        <%#Eval("EngineNumber") %>
                    </td>
                    <td>
                        <%#Eval("BillNumber")%>
                    </td>
                    <td>
                        <%# (DateTime)Eval("ImportDate") == default(DateTime) ? string.Empty : String.Format("{0:d}", Eval("ImportDate"))%>
                    </td>
                    <td>
                        <%# (DateTime)Eval("SellDate") == default(DateTime) ? string.Empty : String.Format("{0:d}", Eval("SellDate"))%>
                    </td>
                    <td class="number">
                        <%#Eval("Price", "{0:N0}")%>
                    </td>
                    <td>
                        <%#EvalPaymentMethod(Eval("PaymentType"))%>
                    </td>
                    <td>
                        <%# (DateTime)Eval("PaymentDate") == default(DateTime) ? string.Empty : String.Format("{0:d}", Eval("PaymentDate"))%>
                    </td>
                    <td>
                        <%#Eval("CustomerName")%>
                    </td>
                    <td>
                        <%#Eval("Gender").ToString() == "0" ? Resources.Gender.Female : Resources.Gender.Male%>
                    </td>
                    <td class="number">
                        <%#Eval("Tel") %>
                    </td>
                    <td class="number">
                        <%#Eval("Mobile") %>
                    </td>
                    <td>
                        <%#Eval("Address") %>
                    </td>
                    <td class="errorText">
                        <%#!(bool)Eval("IsValid") ? Eval("Error") : "" %>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>
    <asp:Button ID="Button2" runat="server" Text="Import" OnClick="Button2_OnClick" Enabled="False"
        meta:resourcekey="Button2Resource1" />
    <asp:Button ID="Button3" runat="server" Enabled="False" OnClick="Button3_Click" Text="Clear"
        meta:resourcekey="Button3Resource1" />
</asp:Content>
