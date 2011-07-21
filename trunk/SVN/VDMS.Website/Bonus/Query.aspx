<%@ Page Title="Query bonus" Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="Query.aspx.cs" Inherits="Bonus_Query" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">

    <div class="form">
        <table>
            <tr runat="server" id="trPlanName">
                <td>
                    <asp:Image ID="Image1" runat="server" SkinID="RequireFieldNon" 
                        meta:resourcekey="Image1Resource1" />
                    <asp:Literal ID="Literal1" runat="server" Text="Plan Name:" 
                        meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPlanName" Width="100%" runat="server" 
                        meta:resourcekey="txtPlanNameResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="Image2" runat="server" SkinID="RequireFieldNon" 
                        meta:resourcekey="Image2Resource1" />
                    <asp:Literal ID="Literal2" runat="server" Text="Date:" 
                        meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtFrom" runat="server" meta:resourcekey="txtFromResource1"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender PopupButtonID="imbC1" ID="txtFrom_CalendarExtender"
                        runat="server" Enabled="True" TargetControlID="txtFrom">
                    </ajaxToolkit:CalendarExtender>
                    <ajaxToolkit:MaskedEditExtender ID="txtFrom_MaskedEditExtender" runat="server" MaskType="Date"
                        Mask="99/99/9999" TargetControlID="txtFrom" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <asp:ImageButton runat="server" SkinID="CalendarImageButton" ID="imbC1" 
                        meta:resourcekey="imbC1Resource1" />
                    ~
                    <asp:TextBox ID="txtTo" runat="server" meta:resourcekey="txtToResource1"></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="txtTo_MaskedEditExtender" runat="server" MaskType="Date"
                        Mask="99/99/9999" TargetControlID="txtTo" Enabled="True">
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
                    <asp:Image ID="Image3" runat="server" SkinID="RequireFieldNon" 
                        meta:resourcekey="Image3Resource1" />
                    <asp:Literal ID="Literal3" runat="server" Text="Dealer:" 
                        meta:resourcekey="Literal3Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtDealer" runat="server" 
                        meta:resourcekey="txtDealerResource1"></asp:TextBox>
                    <%--<vdms:DealerList ID="dlDealer" runat="server" ShowEmptyItem="true">
                    </vdms:DealerList>--%>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnFind" runat="server" Text="Find" 
                        meta:resourcekey="btnFindResource1" onclick="btnFind_Click" />
                </td>
            </tr>
        </table> 
    </div>

    <div class="grid">
        <vdms:PageGridView PageSize="30" ID="gvPlans" runat="server" AutoGenerateColumns="False" 
            DataSourceID="odsPlans" meta:resourcekey="gvPlansResource1">
            <Columns>
                <asp:BoundField DataField="DealerCode" HeaderText="DealerCode" 
                    SortExpression="DealerCode" meta:resourcekey="BoundFieldResource1" />
                <asp:BoundField DataField="DealerName" HeaderText="DealerName" ReadOnly="True" 
                    SortExpression="DealerName" meta:resourcekey="BoundFieldResource2" />
                <asp:BoundField DataField="TransactionDate" DataFormatString="{0:d}" 
                    HeaderText="BonusDate" SortExpression="BonusDate" 
                    meta:resourcekey="BoundFieldResource3" />
                <asp:BoundField DataField="Description" HeaderText="Description" 
                    SortExpression="Description" meta:resourcekey="BoundFieldResource4" />
                <asp:BoundField DataField="Amount" DataFormatString="{0:N0}" 
                    HeaderText="Amount" SortExpression="Amount" 
                    meta:resourcekey="BoundFieldResource5">
                <ItemStyle CssClass="right" />
                </asp:BoundField>
                <asp:BoundField DataField="Balance" DataFormatString="{0:N0}" 
                    HeaderText="Balance" ReadOnly="True" SortExpression="Balance" 
                    meta:resourcekey="BoundFieldResource6">
                <ItemStyle CssClass="right" />
                </asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <div>không có gì</div>
            </EmptyDataTemplate>
        </vdms:PageGridView >
        </div>
        <asp:ObjectDataSource EnablePaging="true" ID="odsPlans" runat="server"  SelectMethod="GetBonusTransactions" 
            TypeName="VDMS.II.BonusSystem.BonusPlans" SelectCountMethod="CountBonusTransactions">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtPlanName" Name="planName" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="txtFrom" Name="from" PropertyName="Text" Type="DateTime" />
                <asp:ControlParameter ControlID="txtTo" Name="to" PropertyName="Text" Type="DateTime" />
                <asp:ControlParameter ControlID="txtDealer" Name="dealer" PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />        
        <div class="grid">
        <asp:ListView ID="ListView1" runat="server" DataSourceID="ObjectDataSource1"> 
        <ItemTemplate>
        <asp:ListView ID="ListView2" runat="server" DataSource='<%# Eval("BonusTransactions") %>'>        
            <ItemTemplate>
                <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
                    <td>
                        <%# Eval("DealerCode")%>
                    </td>
                    <td>
                        <%# Eval("DealerName")%>
                    </td>
                    <td>
                        <%#Eval("TransactionDate", "{0:d}")%>
                    </td>
                    <td>
                        <%# Eval("Description")%>
                    </td>
                    <td class="number">
                        <%#Eval("Amount", "{0:N0}")%>
                    </td>
                    <td class="number">
                        <%#Eval("Balance", "{0:N0}")%>
                    </td>
                </tr>                 
        </tr> 
            </ItemTemplate>
            <LayoutTemplate>
                <asp:PlaceHolder ID="itemPlaceholder" runat="server"/>   
            </LayoutTemplate>
        </asp:ListView>
        <tr class="summary">
                            <td colspan="4" class="right">
                                <%# Resources.Constants.Total %>
                            </td>
                            <td class ="number"> <%#Eval("Sum", "{0:N0}")%> </td>
                            <td></td>
                            </tr>
           </ItemTemplate>
           <LayoutTemplate>
            <table class="datatable" cellpadding="0" cellspacing="0">
                                <tr>
                                    <th>
                                        <asp:Literal ID="Literal11" runat="server" Text="DealerCode" meta:resourcekey="Literal11Resource1"></asp:Literal>
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal4" runat="server" Text="DealerName" meta:resourcekey="Literal4Resource1"></asp:Literal>
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal5" runat="server" Text="BonusDate" meta:resourcekey="Literal5Resource1"></asp:Literal>
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal6" runat="server" Text="Description" meta:resourcekey="Literal6Resource1"></asp:Literal>
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal7" runat="server" Text="Amount" meta:resourcekey="Literal7Resource1"></asp:Literal>
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal8" runat="server" Text="Balance" meta:resourcekey="Literal8Resource1"></asp:Literal>
                                    </th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>                                
                            </table>                   
            </LayoutTemplate>
           </asp:ListView>               
        </div>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server"  SelectMethod="GetBonusTransactions2" 
            TypeName="VDMS.II.BonusSystem.BonusPlans" 
        OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtPlanName" Name="planName" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="txtFrom" Name="from" PropertyName="Text" Type="DateTime" />
                <asp:ControlParameter ControlID="txtTo" Name="to" PropertyName="Text" Type="DateTime" />
                <asp:ControlParameter ControlID="txtDealer" Name="dealer" PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    
</asp:Content>
