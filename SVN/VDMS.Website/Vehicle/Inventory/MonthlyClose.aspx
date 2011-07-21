<%@ Page Title="Monthly summarization" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="MonthlyClose.aspx.cs" Inherits="Part_Inventory_MonthlyClose" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">

    <script type="text/javascript">
        function updated() {
            //  close the popup
            tb_remove();
        }

        function showSearch(link, wCode, dCode) {
            var s = "../Report/MonthlyReportDownload.aspx?";
            s = s + "wCode=" + wCode;
            s = s + "&dCode=" + dCode;
            s = s + "&TB_iframe=true&height=320&width=420";
            link.href = s;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="help" style="float: right; width: 35%;">
        <ul>
            <li>Close sequence is from bottom to up, that is from sub-store to comp-dealer, and
                from comp-dealer to main dealer.</li>
        </ul>
    </div>
    <div runat="server" id="msg" style="float: left; width: 50%;">
        <div style="clear: both;">
        </div>
    </div>
    <div style="clear: both">
    </div>
    <div class="grid">
        <vdms:PageGridView ID="gvWh" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            DataSourceID="odsWH" OnDataBound="gvWh_DataBound" ShowFooter="True" OnRowDataBound="gvWh_RowDataBound">
            <Columns>
                <asp:BoundField DataField="Code" HeaderText="Warehouse Code" SortExpression="Code" />
                <asp:BoundField DataField="Address" HeaderText="Name" SortExpression="Address" />
                <%--<asp:TemplateField HeaderText="Report files">
                    <ItemTemplate>
                        <asp:HyperLink wcode='<%#Eval("Code") %>' dcode='<%#Eval("DealerCode") %>' CssClass="thickbox" ID="lnkFiles" runat="server" NavigateUrl="#" Text="..."></asp:HyperLink>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:HyperLink CssClass="thickbox" ID="lnkFiles" runat="server" NavigateUrl="#" Text="..."></asp:HyperLink>
                    </FooterTemplate>
                    <ItemStyle CssClass="center" />
                    <FooterStyle CssClass="center" />
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Do open to" ItemStyle-CssClass="center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkOpen" runat="server" CausesValidation="False" CommandName="Open"
                            WC='<%# Eval("Code") %>' Text='<%# EvalPrevMonth(Eval("LastMonth"), Eval("LastYear")) %>'
                            OnClick="lnkOpen_Click"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle CssClass="center"></ItemStyle>
                    <FooterStyle CssClass="center"></FooterStyle>
                    <FooterTemplate>
                        <asp:LinkButton ID="lnkOpenDealer" runat="server" CausesValidation="False" CommandName="Open"
                            OnClick="lnkOpenDealer_Click"></asp:LinkButton>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Last closed month" SortExpression="LastClosed" ItemStyle-CssClass="center">
                    <ItemTemplate>
                        <%# Eval("LastMonth") %>/<%# Eval("LastYear") %>
                    </ItemTemplate>
                    <ItemStyle CssClass="center"></ItemStyle>
                    <FooterStyle CssClass="center"></FooterStyle>
                    <FooterTemplate>
                        <asp:Literal ID="litLastClosed" runat="server"></asp:Literal>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Do close to" ItemStyle-CssClass="center">
                    <ItemTemplate>
                        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex='<%#EvalCloseForm(Eval("LastYear")) %>'>
                            <asp:View ID="View1" runat="server">
                                <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="False" CommandName="Close"
                                    WC='<%# Eval("Code") %>' Text='<%# EvalNextMonth(Eval("LastMonth"), Eval("LastYear")) %>'
                                    OnClick="lnkClose_Click"></asp:LinkButton>
                            </asp:View>
                            <asp:View ID="View2" runat="server">
                                <asp:TextBox ID="txtCloseMonth" runat="server" WC='<%# Eval("Code") %>'></asp:TextBox>
                                <asp:ImageButton OnClick="imbDoFirstClose_OnClick" ID="imbDoFirstClose" ImageUrl="~/Images/update.gif"
                                    runat="server" />
                            </asp:View>
                        </asp:MultiView>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:MultiView ID="mvCloseForm" runat="server">
                            <asp:View ID="View3" runat="server">
                                <asp:LinkButton ID="lnkCloseDealer" runat="server" CausesValidation="False" CommandName="Close"
                                    OnClick="lnkCloseDealer_Click"></asp:LinkButton>
                            </asp:View>
                            <asp:View ID="View4" runat="server">
                                <asp:TextBox ID="txtCloseMonth" runat="server"></asp:TextBox>
                                <asp:ImageButton OnClick="imbDoFirstDealerClose_OnClick" ID="imbDoFirstDealerClose"
                                    ImageUrl="~/Images/update.gif" runat="server" />
                            </asp:View>
                        </asp:MultiView>
                    </FooterTemplate>
                    <ItemStyle CssClass="center"></ItemStyle>
                    <FooterStyle CssClass="center"></FooterStyle>
                </asp:TemplateField>
            </Columns>
        </vdms:PageGridView>
        <asp:ObjectDataSource ID="odsWH" EnablePaging="true" runat="server" SelectMethod="FindWithLock1"
            TypeName="VDMS.II.BasicData.WarehouseDAO" SelectCountMethod="CountWithLock">
            <SelectParameters>
                <asp:Parameter DefaultValue="_None_" Name="DealerCode" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    <asp:Button Visible="false" ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
    <asp:PlaceHolder ID="phResetInv" runat="server">
        <asp:TextBox ID="txtCloseTo" runat="server" meta:resourceKey="txtImportDateResource1"
            Width="100px"></asp:TextBox>
        <asp:ImageButton ID="imgbCarlendar" runat="server" meta:resourceKey="ImageButton1Resource1"
            OnClientClick="return false;" SkinID="CalendarImageButton" />
        <%-- <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" BehaviorID="MaskedEditExtender2"
        Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtCloseTo">
    </ajaxToolkit:MaskedEditExtender>--%>
        <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" BehaviorID="CalendarExtender3"
            Enabled="True" PopupButtonID="imgbCarlendar" TargetControlID="txtCloseTo">
        </ajaxToolkit:CalendarExtender>
        <asp:Button ID="btnResetInvent" runat="server" Text="Reset Inventory" OnClick="btnResetInvent_Click" />
    </asp:PlaceHolder>
</asp:Content>
