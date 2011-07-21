<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="SaleReject.aspx.cs" Inherits="Vehicle_Inventory_SaleReject" Title="VMEP Sale confirm vehicle return"
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
    <div class="form" style="width: 450px">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" meta:resourcekey="ValidationSummary1Resource2" />
        <br />
        <asp:BulletedList ID="bllMsg" runat="server" ForeColor="Red" meta:resourcekey="bllMsgResource2">
        </asp:BulletedList>
        <table width="100%">
            <tr>
                <td valign="top">
                    <asp:Localize ID="litDealer" runat="server" Text="Dealer Code:" meta:resourcekey="litDealerResource1"></asp:Localize>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtDealerCode" runat="server" meta:resourcekey="txtDealerCodeResource2"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Localize ID="litEnginenumber" runat="server" Text="Engine Number:" meta:resourcekey="litEnginenumberResource1"></asp:Localize>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtEnginenumber" runat="server" meta:resourcekey="txtEnginenumberResource2"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Localize ID="litReturndate" runat="server" Text="Return date:" meta:resourcekey="litReturndateResource1"></asp:Localize>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtfrom" runat="server" meta:resourcekey="txtfromResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibfrom" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="meefrom" runat="server" TargetControlID="txtfrom"
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                        CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="cefrom" runat="server" TargetControlID="txtfrom"
                        PopupButtonID="ibfrom" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    ~
                    <asp:TextBox ID="txtto" runat="server" meta:resourcekey="txttoResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibto" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibtoResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="meeto" runat="server" TargetControlID="txtto"
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                        CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceto" runat="server" TargetControlID="txtto" PopupButtonID="ibto"
                        Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*" ControlToCompare="txtto"
                        EnableClientScript="true" ValidationGroup="search" ControlToValidate="txtfrom"
                        Type="Date" Operator="LessThanEqual"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Localize ID="litStatus" runat="server" Text="Status:" meta:resourcekey="litStatusResource1"></asp:Localize>
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddlStatus" runat="server" meta:resourcekey="ddlStatusResource1">
                        <asp:ListItem Text="All" Value="0;1;2;4;5" meta:resourcekey="ListItemResource1"></asp:ListItem>
                        <asp:ListItem Value="1" meta:resourcekey="ListItemResource2">Not Confirmed</asp:ListItem>
                        <asp:ListItem Value="2" meta:resourcekey="ListItemResource3">Confirmed</asp:ListItem>
                        <asp:ListItem Value="0" meta:resourcekey="ListItemResource4">Reject</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td valign="top">
                    <asp:Button ID="btnTest" runat="server" Text="View" OnClick="btnTest_Click" meta:resourcekey="btnTestResource2"
                        ValidationGroup="search" />
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <vdms:PageGridView ID="grdSaleReject" runat="server" AutoGenerateColumns="False"
            PageSize="10" AllowPaging="True" OnDataBound="grdSaleReject_DataBound" OnRowDataBound="grdSaleReject_RowDataBound"
            DataSourceID="ReturnItemDataSource1" DataKeyNames="RETURNITEMID" meta:resourcekey="grdSaleRejectResource1"
            OnRowCommand="grdSaleReject_RowCommand">
            <Columns>
                <asp:BoundField ReadOnly="True" HeaderText="No." meta:resourcekey="BoundFieldResource2" />
                <asp:TemplateField HeaderText="Status" meta:resourcekey="TemplateFieldStatusResource01">
                    <ItemTemplate>
                        <img src='<%# EvalStatus(Eval("Status")) %>' alt='' width="30px" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField ReadOnly="True" HeaderText="Engine No." DataField="ENGINENUMBER"
                    meta:resourcekey="BoundFieldResource3" />
                <asp:BoundField ReadOnly="True" HeaderText="Model Code" DataField="ITEMTYPE" meta:resourcekey="BoundFieldResource4" />
                <asp:BoundField ReadOnly="True" HeaderText="Color Code" DataField="COLOR" meta:resourcekey="BoundFieldResource5" />
                <asp:BoundField ReadOnly="True" HeaderText="DealerCode" DataField="DEALERCODE" meta:resourcekey="BoundFieldResource10" />
                <asp:BoundField ReadOnly="True" HeaderText="Branch" DataField="BRANCHCODE" meta:resourcekey="BoundFieldResource6" />
                <asp:BoundField ReadOnly="True" HeaderText="Import Date" DataField="IMPORTEDDATE"
                    HtmlEncode="False" DataFormatString="{0:d}" meta:resourcekey="BoundFieldResource7" />
                <asp:BoundField ReadOnly="True" HeaderText="Release Date" DataField="RELEASEDATE"
                    HtmlEncode="False" DataFormatString="{0:d}" meta:resourcekey="BoundFieldResource9" />
                <asp:BoundField ReadOnly="True" HeaderText="Return Reason" DataField="RETURNREASON"
                    meta:resourcekey="BoundFieldResource8" />
                <asp:TemplateField HeaderText="VMEP Note" meta:resourcekey="TemplateFieldResource10">
                    <ItemTemplate>
                        <asp:TextBox ID="txtVMEPComment" runat="server" Text='<%# Bind("Vmepcomment") %>'
                            Width="100" ReadOnly='<%# EvalReadOnlyVMEPComment(Eval("Status")) %>' MaxLength="1900"
                            meta:resourcekey="txtVMEPCommentResource2"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Transfer Voucher Number" meta:resourcekey="TemplateFieldResource8">
                    <ItemTemplate>
                        <asp:TextBox ID="txtReturnNumber" runat="server" Text='<%# Bind("Returnnumber") %>'
                            ReadOnly='<%# EvalReadOnlyReturnNumber(Eval("Status")) %>' MaxLength="30" meta:resourcekey="txtReturnNumberResource2"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField meta:resourcekey="ButtonFieldResource1">
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btconfirm" Text="..." CommandName="Confirm" meta:resourcekey="ButtonFieldResource1"
                            CommandArgument='<%# Container.DataItemIndex %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField meta:resourcekey="ButtonFieldResource2">
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btreject" Text="..." CommandName="Reject" meta:resourcekey="ButtonFieldResource2"
                            CommandArgument='<%# Container.DataItemIndex %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:ButtonField ButtonType="Button" HeaderText="Confirm" Text="..." CommandName="Confirm"
                    meta:resourcekey="ButtonFieldResource1" />
                <asp:ButtonField ButtonType="Button" HeaderText="Reject" Text="..." CommandName="Reject"
                    meta:resourcekey="ButtonFieldResource2" />--%>
            </Columns>
            <EmptyDataTemplate>
                <b>
                    <asp:Localize ID="litDataNotFound" runat="server" Text="Data not found! Please change the search condition."
                        meta:resourcekey="litDataNotFoundResource1"></asp:Localize></b>
            </EmptyDataTemplate>
            <%--<PagerSettings Mode="NextPreviousFirstLast" Position="Top" />
            <PagerTemplate>
                <div style="float: left">
                    <asp:Literal ID="litPageInfo" runat="server" meta:resourcekey="litPageInfoResource1"></asp:Literal></div>
                <div style="float: right; text-align: right">
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
        <asp:ObjectDataSource ID="ReturnItemDataSource1" runat="server" EnablePaging="True"
            SelectCountMethod="SelectCount" SelectMethod="Select" TypeName="VDMS.I.ObjectDataSource.ReturnItemDataSource">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtDealerCode" Name="DealerCode" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtEnginenumber" Name="EngineNumber" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtfrom" Name="from" PropertyName="Text" Type="DateTime" />
                <asp:ControlParameter ControlID="txtto" Name="to" PropertyName="Text" Type="DateTime" />
                <asp:ControlParameter ControlID="ddlStatus" Name="status" PropertyName="SelectedValue"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
