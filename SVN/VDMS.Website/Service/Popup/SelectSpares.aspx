<%@ Page Title="" Language="C#" MasterPageFile="~/MP/popup.master" AutoEventWireup="true"
    CodeFile="SelectSpares.aspx.cs" Inherits="Service_Popup_SelectSpares" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">

    <script language="javascript" type="text/javascript">
    <!--
        function HilightObj(objID, css) {
            document.getElementById(objID).className = css;
        }

        function HilightRow(hid, src, other) {
            var txtSrc = $get(src);
            var txtOther = $get(other);
            txtSrc.value = txtSrc.value.replace(/^0(0)*/, "");
            txtOther.value = txtOther.value.replace(/^0(0)*/, "");

            if (txtSrc.value == "") txtSrc.value = "0";
            if (txtOther.value == "") txtOther.value = "0";

            if ((txtSrc.value == "0") && (txtOther.value == "0"))
                HilightObj(hid, "");
            else
                HilightObj(hid, "readOnlyRow");

        }
        function clearSRSSpareQuantity() {
            var txt = $get('<%= gvSelectSpare.ClientID %>').getElementsByTagName("input");
            for (var i = 0; i < txt.length; i++) {
                if (txt[i].name.match(/txtAddSRSSpareQuantity/)) {
                    txt[i].value = "0";
                    HilightRow(txt[i].id.replace(/_txtAddSRSSpareQuantity/, ""), txt[i].id, txt[i].id.replace(/_txtAddSRSSpareQuantity/, "_txtAddPCVSpareQuantity"));
                }
            }
        }
        function clearPCVSpareQuantity() {
            var txt = $get('<%= gvSelectSpare.ClientID %>').getElementsByTagName("input");
            for (var i = 0; i < txt.length; i++) {
                if (txt[i].name.match(/txtAddPCVSpareQuantity/)) {
                    txt[i].value = "0";
                    HilightRow(txt[i].id.replace(/_txtAddPCVSpareQuantity/, ""), txt[i].id, txt[i].id.replace(/_txtAddPCVSpareQuantity/, "_txtAddSRSSpareQuantity"));
                }
            }
        }

    -->
    </script>

    <asp:Panel ID="pnSelectSpare" runat="server" meta:resourcekey="pnSelectSpareResource1">
        <asp:UpdatePanel ID="udpSelectSpare" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
            <ContentTemplate>
                <asp:Panel CssClass="popupHeader" ID="pnSelectSpareHeader" runat="server" meta:resourcekey="pnSelectSpareHeaderResource1">
                    <asp:Literal ID="Literal35" runat="server" Text="Select spare" meta:resourcekey="Literal35Resource1"></asp:Literal>
                </asp:Panel>
                <div class="popupBodyTop">
                    <table cellpadding="3" cellspacing="0" style="width: 100%">
                        <tr>
                            <td style="white-space: nowrap; width: 1%">
                                <asp:Literal ID="Literal51" Text="Spare number:" runat="server" meta:resourcekey="Literal51Resource1"></asp:Literal>
                                <asp:TextBox ID="txtSelectSpareNumber" runat="server" meta:resourcekey="txtSelectSpareNumberResource1"></asp:TextBox>
                                <asp:Button ID="btnSearchSpare" runat="server" Text="Search" OnClick="btnSearchSpare_Click"
                                    meta:resourcekey="btnSearchSpareResource1" />
                            </td>
                            <td class="leftObj">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="udpSelectSpare"
                                    DisplayAfter="0" DynamicLayout="False">
                                    <ProgressTemplate>
                                        <img src="../../Images/Spinner.gif" alt="" /><asp:Literal ID="Literal55" Text="Updating..."
                                            runat="server" meta:resourcekey="Literal55Resource1"></asp:Literal>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                            <td class="rightObj">
                                <asp:UpdatePanel ID="udpSelectSparesComamnds" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="lnkSelectingSpares" Text="Selected spares:" meta:resourceKey="litSelectingSparesResource1"
                                            OnClick="btnUpdateSelectingSpares_Click" runat="server"></asp:LinkButton>&nbsp;&nbsp;
                                        <asp:Literal ID="litSelectingSRSSparesCount" runat="server" meta:resourceKey="litSelectingSparesCountResource1"></asp:Literal>
                                        (SRS)<asp:ImageButton ID="imbClearSelectingSRSSpares" OnClientClick="clearSRSSpareQuantity();"
                                            OnClick="btnClearSelectingSRSSpares_Click" runat="server" ImageUrl="~/Images/Delete.gif" />&nbsp;&nbsp;
                                        <asp:Literal ID="litSelectingPCVSparesCount" runat="server" meta:resourceKey="litSelectingSparesCountResource1"></asp:Literal>
                                        (PCV)<asp:ImageButton ID="imbClearSelectingPCVSpares" OnClientClick="clearPCVSpareQuantity();"
                                            OnClick="btnClearSelectingPCVSpares_Click" runat="server" ImageUrl="~/Images/Delete.gif" />&nbsp;&nbsp;&nbsp;
                                        <%--<asp:LinkButton ID="btnClearSelectingSRSSpares" OnClientClick="clearSRSSpareQuantity();"
                                            runat="server" Text="Clear (SRS)" OnClick="btnClearSelectingSRSSpares_Click"
                                            meta:resourceKey="btnClearSelectingSparesResource1" />
                                        <asp:LinkButton ID="btnClearSelectingPCVSpares" OnClientClick="clearPCVSpareQuantity();"
                                            runat="server" Text="Clear (PCV)" OnClick="btnClearSelectingPCVSpares_Click"
                                            meta:resourceKey="btnClearSelectingSparesResource1x" />--%>
                                        <asp:Button ID="btnAddSRSSpares" Visible="False" runat="server" Text="Add to SRS"
                                            OnClick="btnAddSRSSpares_Click" meta:resourceKey="btnAddSRSSparesResource1" />
                                        <asp:Button ID="btnAddPCVSpares" Visible="False" runat="server" Text="Add to PCV"
                                            OnClick="btnAddPCVSpares_Click" meta:resourceKey="btnAddPCVSparesResource1" />
                                        <asp:Button ID="btnUpdateSelectingSpares" Visible="False" runat="server" Text="Update"
                                            OnClick="btnUpdateSelectingSpares_Click" meta:resourceKey="btnUpdateSelectingSparesResource1" />
                                        <asp:Button ID="btnAddSpares" runat="server" Text="Add" OnClick="btnAddSpares_Click"
                                            meta:resourceKey="btnAddSparesResource1" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="popupBody" style="width:700px">
                    <div class="grid">
                        <vdms:PageGridView ID="gvSelectSpare" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            PageSize="5" CssClass="GridView" EmptyDataText="No spare has been found!" Width="100%"
                            OnPreRender="gvSelectxxx_PreRender" OnRowCommand="gvSelectxxx_OnRowCommand" OnRowDataBound="gvSelectSpare_RowDataBound"
                            meta:resourcekey="gvSelectSpareResource1">
                            <Columns>
                                <asp:TemplateField HeaderText="Spare number" meta:resourcekey="TemplateFieldResource5">
                                    <ItemTemplate>
                                        <asp:Literal ID="litSelectedSpareNumber" runat="server" Text='<%# Bind("Partcode") %>'></asp:Literal>
                                        <asp:HiddenField ID="hdLabour" Value='<%# Bind("Labour") %>' runat="server" />
                                        <asp:HiddenField ID="hdManPower" Value='<%# Bind("Manpower") %>' runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Literal ID="Literal34" runat="server" Text='<%# Bind("Partcode") %>'></asp:Literal>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Spare name" meta:resourcekey="TemplateFieldResource6">
                                    <ItemTemplate>
                                        <asp:Label ID="lbSpareNameVN" runat="server" Text='<%# Bind("Partnamevn") %>' meta:resourcekey="lbSpareNameVNResource1"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" meta:resourcekey="TextBox2Resource2" Text='<%# Bind("Partnamevn") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Spare name" meta:resourcekey="TemplateFieldResource7">
                                    <ItemTemplate>
                                        <asp:Label ID="lbSpareNameEN" runat="server" Text='<%# Bind("Partnameen") %>' meta:resourcekey="lbSpareNameENResource1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Model" meta:resourcekey="TemplateFieldResource8">
                                    <ItemTemplate>
                                        <asp:Literal ID="litModel" runat="server" Text='<%# Eval("Motorcode") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Warranty length" meta:resourcekey="TemplateFieldResource9">
                                    <ItemTemplate>
                                        <asp:Label ID="lbWarrantyLength" runat="server" Text='<%# Bind("Warrantylength") %>'
                                            meta:resourcekey="Label5Resource1"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox5" runat="server" meta:resourcekey="TextBox5Resource1" Text='<%# Bind("Warrantylength") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Warranty time" SortExpression="Warrantytime" meta:resourcekey="TemplateFieldResource10">
                                    <ItemTemplate>
                                        <asp:Label ID="lbWarrantyTime" runat="server" Text='<%# Bind("Warrantytime") %>'
                                            meta:resourcekey="Label6Resource1"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox6" runat="server" meta:resourcekey="TextBox6Resource1" Text='<%# Bind("Warrantytime") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SRS" meta:resourcekey="TemplateFieldResource11">
                                    <ItemTemplate>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="txtAddSRSSpareQuantity_FilteredTextBoxExtender"
                                            runat="server" FilterType="Numbers" TargetControlID="txtAddSRSSpareQuantity"
                                            Enabled="True">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ToolTip='<%# Bind("Partcode") %>' Text="0" AutoCompleteType="Disabled"
                                                        ID="txtAddSRSSpareQuantity" runat="server" Width="30px" meta:resourcekey="txtAddSpareQuantityResource1"
                                                        OnTextChanged="UpdateSpareOnSelecting"></asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PCV" meta:resourcekey="TemplateFieldResource11x">
                                    <ItemTemplate>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="txtAddPCVSpareQuantity_FilteredTextBoxExtender"
                                            runat="server" FilterType="Numbers" TargetControlID="txtAddPCVSpareQuantity"
                                            Enabled="True">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ToolTip='<%# Bind("Partcode") %>' Text="0" AutoCompleteType="Disabled"
                                                        ID="txtAddPCVSpareQuantity" runat="server" Width="30px" meta:resourcekey="txtAddSpareQuantityResource1"
                                                        OnTextChanged="UpdateSpareOnSelecting"></asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unitprice" meta:resourcekey="TemplateFieldResource12">
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr>
                                                <td>
                                                    <asp:TextBox Text='<%# Bind("Unitprice") %>' AutoCompleteType="Disabled" OnTextChanged="UpdateSpareOnSelecting"
                                                        ID="txtAddSpareUnitPrice" runat="server" Width="60px" ToolTip='<%# Eval("Partcode") %>'></asp:TextBox>
                                                    <asp:Literal Text='<%# Bind("Unitprice") %>' ID="litOriginalSpareUnitPrice" runat="server"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:RegularExpressionValidator ID="rvAddSpareUnitPrice" runat="server" ControlToValidate="txtAddSpareUnitPrice"
                                                        ErrorMessage="Unit price must be valid number!" ValidationExpression="\s*\d+([.,]?\d*[1-9]+\d*)?\s*"
                                                        ValidationGroup="AddSpare" meta:resourcekey="rvAddSpareUnitPriceResource1" Text="*"></asp:RegularExpressionValidator>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ValidationGroup="AddSpare" ControlToValidate="txtAddSpareUnitPrice"
                                                        Text="*" ID="rfvAddSpareUnitPrice" runat="server" ErrorMessage="Unit price can not be blank!"
                                                        meta:resourcekey="rfvAddSpareUnitPriceResource1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox4" runat="server" meta:resourcekey="TextBox4Resource1" Text='<%# Bind("Unitprice") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ButtonType="Button" ShowSelectButton="True" Visible="False" meta:resourcekey="CommandFieldResource1">
                                    <ItemStyle Width="10px" />
                                </asp:CommandField>
                                <asp:TemplateField ShowHeader="False" Visible="False" meta:resourcekey="TemplateFieldResource13">
                                    <ItemTemplate>
                                        <asp:Button ID="btnAddSpare" runat="server" Text="Update" ValidationGroup="AddSpare"
                                            OnClick="btnAddSpare_Click" meta:resourcekey="btnAddSpareResource1" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="False" ShowHeader="False" meta:resourcekey="TemplateFieldResource14">
                                    <ItemTemplate>
                                        <input id="_btnSelectSpare" type="button" value="OK" onclick='onSelectedSpare(&#039;<%# Eval("PartCode") %>&#039;, &#039;<%# EvalPartName(Eval("Partnameen"), Eval("Partnamevn")) %>&#039;, &#039;<%# Eval("Unitprice") %>&#039;);' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <%--<PagerSettings Position="Top" Mode="NextPreviousFirstLast" />
                            <PagerTemplate>
                                <div style="float: left;">
                                    <asp:Literal ID="litgvSelectSparePageInfo" runat="server" meta:resourcekey="litgvSelectSparePageInfoResource2"></asp:Literal></div>
                                <div style="text-align: right; float: right;">
                                    <asp:Button ID="cmdFirst" runat="server" Text="First" CommandName="Page" CommandArgument="First"
                                        meta:resourcekey="cmdFirstResource3" />
                                    <asp:Button ID="cmdPrevious" runat="server" Text="Previous" CommandName="Page" CommandArgument="Prev"
                                        meta:resourcekey="cmdPreviousResource3" />
                                    <asp:Button ID="cmdNext" runat="server" Text="Next" CommandName="Page" CommandArgument="Next"
                                        meta:resourcekey="cmdNextResource3" />
                                    <asp:Button ID="cmdLast" runat="server" Text="Last" CommandName="Page" CommandArgument="Last"
                                        meta:resourcekey="cmdLastResource3" />
                                </div>
                            </PagerTemplate>--%>
                        </vdms:PageGridView>
                    </div>
                    <asp:ObjectDataSource SelectMethod="Select" ID="odsSelectSpare" runat="server" TypeName="VDMS.I.ObjectDataSource.SparesDataSource"
                        EnablePaging="True" SelectCountMethod="SelectCount">
                        <SelectParameters>
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:ControlParameter ControlID="txtSelectSpareNumber" Name="spareNumberLike" PropertyName="Text" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearchSpare" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
