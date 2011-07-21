<%@ Page Title="" Language="C#" MasterPageFile="~/MP/popup.master" AutoEventWireup="true"
    CodeFile="SelectBroken.aspx.cs" Inherits="Service_Popup_SelectBroken" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <asp:Panel ID="pnSelectBrokenCode" runat="server" meta:resourcekey="pnSelectBrokenCodeResource1">
        <asp:UpdatePanel ID="udpSelectBrokenCode" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel CssClass="popupHeader" ID="pnSelBrokenHeader" runat="server" meta:resourcekey="pnSelBrokenHeaderResource1">
                    <asp:Literal ID="Literal29" runat="server" Text="Select Broken code" meta:resourcekey="Literal29Resource1"></asp:Literal>
                </asp:Panel>
                <div class="popupBodyTop">
                    <table cellpadding="3" cellspacing="0" style="width: 100%">
                        <tr>
                            <td style="white-space: nowrap; width: 1%">
                                <asp:Literal ID="Literal58" Text="Broken code:" runat="server" meta:resourcekey="Literal58Resource1"></asp:Literal>
                                <asp:TextBox ID="txtSearchBrokenCode" runat="server" meta:resourcekey="txtSearchBrokenCodeResource1"></asp:TextBox>
                                <asp:Button ID="btnSearchBroken" runat="server" Text="Search" OnClick="btnSearchBroken_Click"
                                    meta:resourcekey="btnSearchBrokenResource1" />
                            </td>
                            <td class="leftObj">
                                <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="udpSelectBrokenCode"
                                    DisplayAfter="0" DynamicLayout="False">
                                    <ProgressTemplate>
                                        <img src="../../Images/Spinner.gif" alt="" /><asp:Literal ID="Literal55p" Text="Updating..."
                                            runat="server" meta:resourcekey="Literal55pResource1"></asp:Literal>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                            <td class="rightObj">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="popupBody">
                    <asp:DropDownList ID="DropDownList1" runat="server" DataTextField="Brokenname" DataValueField="Brokencode"
                        DataSourceID="odsBroken" meta:resourcekey="DropDownList1Resource1">
                    </asp:DropDownList>
                    <asp:ObjectDataSource ID="odsBroken" runat="server" SelectMethod="Select" TypeName="VDMS.I.ObjectDataSource.BrokenDatasource">
                    </asp:ObjectDataSource>
                    <div class="grid">
                        <vdms:PageGridView ID="gvSelectBroken" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            PageSize="5" CssClass="GridView" EmptyDataText="No broken code found!" Width="100%"
                            OnPreRender="gvSelectxxx_PreRender" meta:resourcekey="gvSelectBrokenResource1"
                            DataSourceID="odsSelectBroken">
                            <Columns>
                                <asp:TemplateField HeaderText="Broken code" SortExpression="Brokencode" meta:resourcekey="TemplateFieldResource15">
                                    <ItemTemplate>
                                        <asp:Literal ID="litSelectedBrokenCode" runat="server" Text='<%# Eval("Brokencode") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Broken name" SortExpression="Brokenname" meta:resourcekey="TemplateFieldResource16">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("Brokenname") %>' meta:resourcekey="Label2Resource2"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" meta:resourcekey="TextBox2Resource3" Text='<%# Bind("Brokenname") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ButtonType="Button" ShowSelectButton="True" Visible="False" meta:resourcekey="CommandFieldResource2">
                                    <ItemStyle Width="10px" />
                                </asp:CommandField>
                                <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource17">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelectBroken" runat="server" Text="OK" OnClick="btnSelectBroken_Click"
                                            CommandArgument='<%# Eval("Brokencode") %>' meta:resourcekey="btnSelectBrokenResource1" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <%--<PagerSettings Position="Top" Mode="NextPreviousFirstLast" />
						<PagerTemplate>
							<div style="float: left;">
								<asp:Literal ID="litgvSelectBrokenPageInfo" runat="server" meta:resourcekey="litgvSelectBrokenPageInfoResource1"></asp:Literal></div>
							<div style="text-align: right; float: right;">
								<asp:Button ID="cmdFirst" runat="server" Text="First" CommandName="Page" CommandArgument="First"
									meta:resourcekey="cmdFirstResource4" />
								<asp:Button ID="cmdPrevious" runat="server" Text="Previous" CommandName="Page" CommandArgument="Prev"
									meta:resourcekey="cmdPreviousResource4" />
								<asp:Button ID="cmdNext" runat="server" Text="Next" CommandName="Page" CommandArgument="Next"
									meta:resourcekey="cmdNextResource4" />
								<asp:Button ID="cmdLast" runat="server" Text="Last" CommandName="Page" CommandArgument="Last"
									meta:resourcekey="cmdLastResource4" />
							</div>
						</PagerTemplate>--%>
                        </vdms:PageGridView>
                    </div>
                    <asp:ObjectDataSource SelectMethod="Select" ID="odsSelectBroken" runat="server" TypeName="VDMS.I.ObjectDataSource.BrokenDatasource"
                        EnablePaging="True" SelectCountMethod="SelectCount">
                        <SelectParameters>
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:ControlParameter Name="fromCode" ControlID="txtSearchBrokenCode" PropertyName="Text"
                                Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
