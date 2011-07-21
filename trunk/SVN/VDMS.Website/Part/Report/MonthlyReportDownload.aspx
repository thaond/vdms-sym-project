<%@ Page Language="C#" MasterPageFile="~/MP/Popup.master" AutoEventWireup="true"
    CodeFile="MonthlyReportDownload.aspx.cs" Inherits="Part_Report_MonthlyReportDownload"
    Title="Untitled Page" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="grid" style="width: 420px; border-width: 0px;">
        <vdms:PageGridView AllowPaging="True" PageSize="15" ID="GridView1" 
            runat="server" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" 
            meta:resourcekey="GridView1Resource1">
            <Columns>
                <asp:BoundField DataField="Moth" HeaderText="Moth" SortExpression="Moth" 
                    ItemStyle-CssClass="center" meta:resourcekey="BoundFieldResource1" >
<ItemStyle CssClass="center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Year" HeaderText="Year" SortExpression="Year" 
                    ItemStyle-CssClass="center" meta:resourcekey="BoundFieldResource2" >
<ItemStyle CssClass="center"></ItemStyle>
                </asp:BoundField>
                <asp:HyperLinkField ItemStyle-CssClass="center" 
                    DataNavigateUrlFields="FilePathName" DataTextFormatString="..." 
                    DataTextField="FilePathName" HeaderText="By part" 
                    meta:resourcekey="HyperLinkFieldResource1">
<ItemStyle CssClass="center"></ItemStyle>
                </asp:HyperLinkField>
                <asp:HyperLinkField ItemStyle-CssClass="center" 
                    DataNavigateUrlFields="FilePathName2" DataTextFormatString="..." 
                    DataTextField="FilePathName2" HeaderText="By component" 
                    meta:resourcekey="HyperLinkFieldResource2" >
<ItemStyle CssClass="center"></ItemStyle>
                </asp:HyperLinkField>
            </Columns>
        </vdms:PageGridView>
    </div>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetReportFiles"
        TypeName="VDMS.II.Report.PartMonthlyReportFileDAO" EnablePaging="True" 
        SelectCountMethod="CountReportFiles">
        <SelectParameters>
            <asp:QueryStringParameter Name="wCode" QueryStringField="wcode" Type="String" />
            <asp:QueryStringParameter Name="dealerCode" QueryStringField="dcode" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
