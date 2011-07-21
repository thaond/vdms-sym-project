<%@ Page Language="C#" MasterPageFile="~/MP/popup.master" AutoEventWireup="true"
    CodeFile="SelectEngineNo.aspx.cs" Inherits="Service_Popup_SelectEngineNo" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">

    <script type="text/javascript">
<!--
        function switchPrefixPn(chb) {
            $get('<%= phFindPrefix.ClientID %>').className = (chb.checked) ? "" : "hidden";
            $get('<%= ddlPrefix.ClientID %>').className = (chb.checked) ? "" : "hidden";
        }
        function pageLoad(sender, args) {
            //switchPrefixPn($get('<%= chbUsePrefix.ClientID %>'));
        }
-->
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <asp:UpdatePanel ID="udpSelectEngineNo" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
        <ContentTemplate>
            <div class="popupHeader">
                <asp:Literal ID="Literal59" runat="server" Text="Select Engine number" meta:resourcekey="Literal59Resource1"></asp:Literal>
            </div>
            <asp:HiddenField ID="hdTestEngine" runat="server" />
            <asp:ObjectDataSource ID="odsEnginePrefix" runat="server" SelectMethod="GetPrefixByModel"
                TypeName="VDMS.I.ObjectDataSource.EnginePrefixDataSource">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtModel" PropertyName="Text" Name="model" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <table cellpadding="3" cellspacing="1">
                <tr>
                    <td style="white-space: nowrap; width: 1%">
                        <asp:UpdatePanel ID="udpQueryCond" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                            <ContentTemplate>
                                <table cellpadding="3" cellspacing="1">
                                    <tr>
                                        <td colspan="2">
                                            <asp:ValidationSummary ID="vsPrefix" ValidationGroup="SearchPrefix" runat="server"
                                                meta:resourceKey="vsPrefixResource1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chbUsePrefix" onclick="switchPrefixPn(this)" runat="server" Text="Use prefix"
                                                Checked="true" CssClass="hidden" meta:resourceKey="chbUsePrefixResource1" /><br />
                                            <asp:Panel ID="phFindPrefix" runat="server" meta:resourceKey="phFindPrefixResource1">
                                                <asp:Literal ID="Literal1" runat="server" Text="Model:" meta:resourceKey="Literal1Resource1"></asp:Literal>
                                                <asp:TextBox ID="txtModel" CssClass="inputKeyField" runat="server" meta:resourceKey="txtModelResource1"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="SearchPrefix" ID="rfvModel" ControlToValidate="txtModel"
                                                    Text="*" runat="server" Visible="false" ErrorMessage='"Model" cannot be blank!'
                                                    meta:resourceKey="rfvModelResource1"></asp:RequiredFieldValidator>
                                                <asp:Button ValidationGroup="SearchPrefix" ID="btnSearchPrefix" runat="server" Text="Find prefix"
                                                    OnClick="btnSearchPrefix_Click" meta:resourceKey="btnSearchPrefixResource1" />
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="udpSelectEngineNo"
                                                DisplayAfter="0" DynamicLayout="true">
                                                <ProgressTemplate>
                                                    <img alt="" src="../../Images/Spinner.gif" /><asp:Literal ID="Literal55p2" runat="server"
                                                        meta:resourcekey="Literal55p2Resource1" Text="Updating..."></asp:Literal>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Literal ID="Literal60" Text="Engine number:" runat="server" meta:resourcekey="Literal60Resource1"></asp:Literal>
                                                <asp:DropDownList ID="ddlPrefix" runat="server" DataTextField="Prefix" DataValueField="Prefix"
                                                    meta:resourceKey="ddlPrefixResource1">
                                                </asp:DropDownList>
                                                <asp:TextBox CssClass="inputKeyField" ID="txtSearchEngineNo" runat="server" meta:resourcekey="txtSearchEngineNoResource1"></asp:TextBox>
                                                <asp:DropDownList ID="ddlDealer" runat="server" meta:resourcekey="ddlDealerResource1">
                                                    <asp:ListItem meta:resourcekey="ListItemResource1" Value="">All</asp:ListItem>
                                                    <asp:ListItem meta:resourcekey="ListItemResource2" Value="">My Dealer</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Button ID="btnSearchEngine" runat="server" Text="Search" OnClick="btnSearchEngine_Click"
                                                    meta:resourcekey="btnSearchEngineResource1" />
                                                <asp:Button ID="btnUseEngineNumber" OnClick="btnUseEngineNumber_Click" runat="server"
                                                    Text="Use this number" meta:resourcekey="btnUseEngineNumberResource1" />
                                            </td>
                                        </tr>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="chbUsePrefix" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnSearchPrefix" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="text-align:right">
                        &nbsp;
                        <asp:Label ID="lbDataSource" runat="server" CssClass="hide"></asp:Label>
                    </td>
                </tr>
            </table>
            <div class="grid" style="width: 500px">
                <vdms:PageGridView ID="gvSelectEngine" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    OnRowCommand="gvSelectxxx_OnRowCommand" DataKeyNames="ItemInstanceId" EmptyDataText="No engine number found!"
                    meta:resourcekey="gvSelectEngineResource1">
                    <Columns>
                        <asp:TemplateField HeaderText="Engine number" SortExpression="Enginenumber" meta:resourcekey="TemplateFieldResource18">
                            <ItemTemplate>
                                <asp:Literal ID="litItemEngineNo" runat="server" Text='<%# Eval("EngineNumber") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Model" SortExpression="Itemtype" meta:resourcekey="TemplateFieldResource19">
                            <ItemTemplate>
                                <asp:Literal ID="litItemModel" runat="server" Text='<%# Eval("ItemType") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Color" SortExpression="Color" meta:resourcekey="TemplateFieldResource20">
                            <ItemTemplate>
                                <asp:Literal ID="litItemColorName" runat="server" Text='<%# Eval("Item.ColorName") %>'></asp:Literal>
                                <asp:HiddenField ID="hdItemColorCode" runat="server" Value='<%# Eval("Item.ColorCode") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Imported date" SortExpression="Importeddate" Visible="False"
                            meta:resourcekey="TemplateFieldResource21">
                            <ItemTemplate>
                                <asp:Label ID="lbImpDate" runat="server" Text='<%# Eval("ImportedDate") %>' meta:resourcekey="lbImpDateResource1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource22">
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hdItemMadeDate" Value='<%# Eval("MadeDate") %>' />
                                <asp:HiddenField runat="server" ID="hdItemDealerCode" Value='<%# Eval("DealerCode") %>' />
                                <asp:HiddenField runat="server" ID="hdItemBranchCode" Value='<%# Eval("BranchCode") %>' />
                                <asp:HiddenField runat="server" ID="hdItemDatabaseCode" Value='<%# Eval("DatabaseCode") %>' />
                                <asp:HiddenField runat="server" ID="hdItemID" Value='<%# Eval("ItemInstanceId") %>' />
                                <asp:Button ID="btnSelectAnEngine" runat="server" Text="OK" OnClick="btnSelectEngine_Click"
                                    CommandArgument='<%# Eval("EngineNumber") %>' OnDataBinding="btnSelectEngine_DataBinding"
                                    meta:resourcekey="btnSelectAnEngineResource1" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </vdms:PageGridView>
            </div>
            <asp:ObjectDataSource ID="odsSelectEngine" runat="server" EnablePaging="True" SelectMethod="Select"
                TypeName="VDMS.I.ObjectDataSource.ItemInstanceDataSource" SelectCountMethod="SelectCount">
                <SelectParameters>
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="engineNumberLike" Type="String" />
                    <asp:ControlParameter Name="dealerCode" ControlID="ddlDealer" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearchEngine" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
