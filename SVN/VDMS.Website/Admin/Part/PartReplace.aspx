<%@ Page Title="Part replacing" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="PartReplace.aspx.cs" Inherits="Admin_Part_PartReplace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">

    <script type="text/javascript">
        function updated() {
            //  close the popup
            tb_remove();
            //  refresh the update panel so we can view the changes
            $('#<%= this.btnFind.ClientID %>').click();
        }

        function showLink(id) {
            var s = "PartReplaceEdit.aspx?";
            s = s + "id=" + id;
            s = s + "&TB_iframe=true&height=200&width=280";
            var link = document.getElementById("popupLink");
            link.href = s;
            $('#popupLink').click();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <a id="popupLink" href="#" class="thickbox"></a>
    <ajaxToolkit:TabContainer ID="TabContainer1" runat="server">
        <ajaxToolkit:TabPanel ID="tabDbData" runat="server">
            <HeaderTemplate>
                <asp:Literal ID="Literal2" runat="server" Text="Part replacing data"></asp:Literal>
            </HeaderTemplate>
            <ContentTemplate>
                <div class="form">
                    <table>
                        <tr>
                            <td>
                                <asp:Literal ID="lit1" runat="server" Text="Part code:"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPartCode" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Literal ID="lit3" runat="server" Text="Replacing code:"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRepPart" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lit2" runat="server" Text="Part name:"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPartName" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Literal ID="lit4" runat="server" Text="Status:"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtStatus" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" />
                                <asp:Button ID="btnExcel" runat="server" Text="Export excel" 
                                    onclick="btnExcel_Click" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:HyperLink ID="lnkAdd" NavigateUrl="PartReplaceEdit.aspx?TB_iframe=true&height=200&width=280"
                                    CssClass="thickbox" runat="server">Add new</asp:HyperLink>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="grid">
                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <vdms:PageGridView AllowPaging="True" PageSize="30" ID="gv" runat="server" AutoGenerateColumns="False"
                                DataKeyNames="PartReplaceId" DataSourceID="ods" OnRowDataBound="gv_RowDataBound">
                                <Columns>
                                    <asp:TemplateField ItemStyle-CssClass="center" HeaderText="No">
                                        <ItemStyle CssClass="center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PartCode" HeaderText="Part code" SortExpression="PartCode" />
                                    <asp:BoundField DataField="ReplacePartCode" HeaderText="Replacing code" SortExpression="ReplacePartCode" />
                                    <asp:BoundField DataField="PartName" HeaderText="Part name" ReadOnly="True" SortExpression="PartName" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <input type="image" style="border-width: 0px;" onclick='showLink(<%#  Eval("PartReplaceId") %>);'
                                                alt="Edit" src="../../Images/Edit.gif">
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                OnClientClick="if (!confirm(SysMsg[0])) return false;" ImageUrl="~/Images/Delete.gif"
                                                Text="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </vdms:PageGridView>
                            <asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetPartReplace" SelectCountMethod="CountPartReplace"
                                TypeName="VDMS.II.PartManagement.PartReplaceDAO" EnablePaging="True" DeleteMethod="Delete">
                                <DeleteParameters>
                                    <asp:Parameter Name="PartReplaceId" Type="Int64" />
                                </DeleteParameters>
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="txtPartCode" Name="partCode" PropertyName="Text"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="txtRepPart" Name="repPartCode" PropertyName="Text"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="txtPartName" Name="partName" PropertyName="Text"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="txtStatus" Name="status" PropertyName="Text" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnFind" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="tabImpData" runat="server">
            <HeaderTemplate>
                <asp:Literal ID="Literal3" runat="server" Text="Import excel file"></asp:Literal>
            </HeaderTemplate>
            <ContentTemplate>
                <div class="form">
                    <table>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal1" runat="server" Text="Import excel file:"></asp:Literal>
                            </td>
                            <td>
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div runat="server" id="Msg">
                </div>
                <div class="grid">
                    <vdms:PageGridView AllowPaging="True" PageSize="30" ID="gvImp" runat="server" AutoGenerateColumns="False"
                        DataKeyNames="Line" DataSourceID="odsImp" OnRowDataBound="gv_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="Line" HeaderText="Line" SortExpression="Line" ItemStyle-CssClass="center" />
                            <asp:BoundField DataField="OldPartCode" HeaderText="Old Part Code" SortExpression="OldPartCode" />
                            <asp:BoundField DataField="NewPartCode" HeaderText="New Part Code" SortExpression="NewPartCode" />
                            <%--<asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPartCode" MaxLength="30" runat="server" Text='<%# Eval("PartCode") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRepPartCode" MaxLength="30" runat="server" Text='<%# Eval("ReplacePartCode") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                            <%--<asp:BoundField DataField="PartName" HeaderText="Part name" ReadOnly="True" SortExpression="PartName" />--%>
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                            <asp:BoundField DataField="Error" HeaderText="Error" SortExpression="Error" />
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                        OnClientClick="if (!confirm(SysMsg[0])) return false;" ImageUrl="~/Images/Delete.gif"
                                        Text="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </vdms:PageGridView>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                    <asp:ObjectDataSource ID="odsImp" runat="server" SelectMethod="GetAllError" SelectCountMethod="CountAllError"
                        TypeName="VDMS.II.PartManagement.PartReplaceDAO" EnablePaging="True" DeleteMethod="Delete">
                        <DeleteParameters>
                            <asp:Parameter Name="Line" Type="Int32" />
                        </DeleteParameters>
                    </asp:ObjectDataSource>
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
</asp:Content>
