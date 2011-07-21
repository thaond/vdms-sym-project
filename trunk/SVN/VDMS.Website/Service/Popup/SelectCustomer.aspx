<%@ Page Title="" Language="C#" MasterPageFile="~/MP/popup.master" AutoEventWireup="true"
    CodeFile="SelectCustomer.aspx.cs" Inherits="Service_Popup_SelectCustomer" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <asp:Panel ID="pnSelectCus" runat="server" meta:resourcekey="pnSelectCusResource1">
        <asp:UpdatePanel ID="udpSelectCus" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hdEngineNumber" />
                <asp:Panel CssClass="popupHeader" ID="pnSelectCusHeader" runat="server" meta:resourcekey="pnSelectCusHeaderResource1">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="Literal61" runat="server" Text="Select customer" meta:resourcekey="Literal61Resource1"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div class="popupBodyTop">
                    <table cellpadding="3" cellspacing="0" style="width: 100%">
                        <tr>
                            <td class="leftObj">
                                <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="udpSelectCus"
                                    DisplayAfter="0" DynamicLayout="False">
                                    <ProgressTemplate>
                                        <img src="../../Images/Spinner.gif" alt="" /><asp:Literal ID="Literal55p3" Text="Updating..."
                                            runat="server" meta:resourcekey="Literal55p3Resource1"></asp:Literal>
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
                    <div class="form" runat="server" id="divSearch">
                        <table>
                            <tr>
                                <td>
                                    <asp:Literal ID="Literal62" runat="server" meta:resourcekey="Literal62Resource1"
                                        Text="Identify number:"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtId" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="Literal64" runat="server" meta:resourcekey="Literal64Resource1"
                                        Text="Address:"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="Literal63" runat="server" meta:resourcekey="Literal63Resource1"
                                        Text="Name:"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="Literal65" runat="server" meta:resourcekey="Literal65Resource1"
                                        Text="Phone:"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnSearch" runat="server" meta:resourcekey="btnSearchResource1" 
                                        OnClick="btnSearch_Click" Text="Search" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="grid">
                        <vdms:PageGridView ID="gvSelectCust" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            EmptyDataText="No customer found!" OnPreRender="gvSelectxxx_PreRender" Width="100%"
                            OnRowDataBound="gvSelectCust_RowDataBound" meta:resourcekey="gvSelectCustResource1">
                            <Columns>
                                <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource4">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelectCust" runat="server" CausesValidation="False" CommandArgument='<%# Eval("CustomerId") %>'
                                            OnClick="btnSelectCust_Click" Text="Select" meta:resourcekey="btnSelectCustResource1" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Identifynumber" HeaderText="Identifynumber" SortExpression="Identifynumber"
                                    meta:resourcekey="BoundFieldResource1" />
                                <asp:BoundField DataField="Fullname" HeaderText="Fullname" SortExpression="Fullname"
                                    meta:resourcekey="BoundFieldResource2" />
                                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address"
                                    meta:resourcekey="BoundFieldResource3" />
                                <%--<asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" meta:resourcekey="BoundFieldResource4" />--%>
                                <asp:TemplateField HeaderText="Birthdate" SortExpression="Birthdate">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# EvalDate(Eval("Birthdate")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Mobile" HeaderText="Mobile" SortExpression="Mobile" meta:resourcekey="BoundFieldResource6" />
                                <asp:BoundField DataField="Tel" HeaderText="Tel" SortExpression="Tel" meta:resourcekey="BoundFieldResource7" />
                                <asp:BoundField DataField="Customerdescription" HeaderText="Description" SortExpression="Customerdescription"
                                    meta:resourcekey="BoundFieldResource8" />
                            </Columns>
                            <%--<PagerSettings Mode="NextPreviousFirstLast" Position="Top" />
                        <PagerTemplate>
                            <div style="float: left;">
                                <asp:Literal ID="litgvSelectSparePageInfo" runat="server" meta:resourcekey="litgvSelectSparePageInfoResource1"></asp:Literal>
                            </div>
                            <div style="text-align: right; float: right;">
                                <asp:Button ID="cmdFirst" runat="server" CommandArgument="First" CommandName="Page"
                                    Text="First" meta:resourcekey="cmdFirstResource2" />
                                <asp:Button ID="cmdPrevious" runat="server" CommandArgument="Prev" CommandName="Page"
                                    Text="Previous" meta:resourcekey="cmdPreviousResource2" />
                                <asp:Button ID="cmdNext" runat="server" CommandArgument="Next" CommandName="Page"
                                    Text="Next" meta:resourcekey="cmdNextResource2" />
                                <asp:Button ID="cmdLast" runat="server" CommandArgument="Last" CommandName="Page"
                                    Text="Last" meta:resourcekey="cmdLastResource2" />
                            </div>
                        </PagerTemplate>--%>
                        </vdms:PageGridView>
                    </div>
                    <asp:ObjectDataSource ID="odsCustomer" runat="server" EnablePaging="True" SelectCountMethod="SelectCount"
                        SelectMethod="SelectCustomers" TypeName="VDMS.I.ObjectDataSource.CustomerDataSource">
                        <SelectParameters>
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:QueryStringParameter Name="engineNumber" QueryStringField="engno" Type="String" />
                            <asp:QueryStringParameter Name="dCode" QueryStringField="dc" Type="String" />
                            <asp:QueryStringParameter Name="cType" QueryStringField="ct" Type="String" DefaultValue="SV" />
                            <asp:ControlParameter ControlID="txtId" PropertyName="Text" Name="id" Type="String" />
                            <asp:ControlParameter ControlID="txtName" PropertyName="Text" Name="name" Type="String" />
                            <asp:ControlParameter ControlID="txtAddress" PropertyName="Text" Name="address" Type="String" />
                            <asp:ControlParameter ControlID="txtPhone" PropertyName="Text" Name="phone" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
