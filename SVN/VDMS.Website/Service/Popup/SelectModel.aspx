<%@ Page Title="" Language="C#" MasterPageFile="~/MP/popup.master" AutoEventWireup="true"
    CodeFile="SelectModel.aspx.cs" Inherits="Service_Popup_SelectModel" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <asp:UpdatePanel ID="udpSelectModel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <%--<asp:Literal ID="Literal62" runat="server" Text="Select model" meta:resourcekey="Literal62Resource1"></asp:Literal>--%>
            <table cellpadding="5" cellspacing="3">
                <tr>
                    <td style="white-space: nowrap; width: 1%">
                        <asp:Literal ID="Literal63" Text="Model:" runat="server" meta:resourcekey="Literal63Resource1"></asp:Literal>
                        <asp:TextBox ID="txtSearchModel" runat="server" meta:resourcekey="txtSearchModelResource1"></asp:TextBox>
                        <asp:Button ID="btnSearchModel" runat="server" Text="Search" meta:resourcekey="btnSearchModelResource1" />
                    </td>
                    <td class="leftObj">
                        <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="udpSelectModel"
                            DisplayAfter="0" DynamicLayout="False">
                            <ProgressTemplate>
                                <img src="../../Images/Spinner.gif" alt="" /><asp:Literal ID="Literal55p4" Text="Updating..."
                                    runat="server" meta:resourcekey="Literal55p4Resource1"></asp:Literal>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
            <div class="grid" style="width: 400px">
                <vdms:PageGridView ID="gvSelectModel" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    DataSourceID="odsSelectModel" EmptyDataText="No model found!" OnRowCommand="gvSelectxxx_OnRowCommand"
                    CssClass="GridView" Width="400px" OnPreRender="gvSelectxxx_PreRender" meta:resourcekey="gvSelectModelResource1">
                    <Columns>
                        <asp:TemplateField HeaderText="Itemtype" SortExpression="Itemtype" meta:resourcekey="TemplateFieldResource1">
                            <ItemTemplate>
                                <asp:Literal ID="litSelectedModel" runat="server" Text='<%# Bind("Itemtype") %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle Width="100px" Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Itemname" SortExpression="Itemname" meta:resourcekey="TemplateFieldResource2">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Itemname") %>' meta:resourcekey="Label2Resource1"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" meta:resourcekey="TextBox2Resource1" Text='<%# Bind("Itemname") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource3" ItemStyle-Width="50px">
                            <ItemTemplate>
                                <asp:Button ID="btnSelectModel" CommandArgument='<%# Eval("Itemtype") %>' runat="server"
                                    Text="OK" OnClick="btnSelectModel_Click" meta:resourcekey="btnSelectModelResource1" />
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                        </asp:TemplateField>
                    </Columns>
                    <%--<PagerSettings Position="Top" Mode="NextPreviousFirstLast" />
                            <PagerTemplate>
                                <div style="float: left;">
                                    <asp:Literal ID="litgvSelectModelPageInfo" runat="server" meta:resourcekey="litgvSelectModelPageInfoResource1"></asp:Literal></div>
                                <div style="text-align: right; float: right;">
                                    <asp:Button ID="cmdFirst" runat="server" Text="First" CommandName="Page" CommandArgument="First"
                                        meta:resourcekey="cmdFirstResource1" />
                                    <asp:Button ID="cmdPrevious" runat="server" Text="Previous" CommandName="Page" CommandArgument="Prev"
                                        meta:resourcekey="cmdPreviousResource1" />
                                    <asp:Button ID="cmdNext" runat="server" Text="Next" CommandName="Page" CommandArgument="Next"
                                        meta:resourcekey="cmdNextResource1" />
                                    <asp:Button ID="cmdLast" runat="server" Text="Last" CommandName="Page" CommandArgument="Last"
                                        meta:resourcekey="cmdLastResource1" />
                                </div>
                            </PagerTemplate>--%>
                </vdms:PageGridView>
            </div>
            <asp:ObjectDataSource SelectMethod="Select" ID="odsSelectModel" runat="server" TypeName="VDMS.I.ObjectDataSource.DataItemDataSource"
                EnablePaging="True" SelectCountMethod="SelectCount">
                <SelectParameters>
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:ControlParameter ControlID="txtSearchModel" PropertyName="Text" Name="itemTypeLike"
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearchModel" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
