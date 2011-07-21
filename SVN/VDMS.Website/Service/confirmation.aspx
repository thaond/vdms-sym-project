<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="Confirmation.aspx.cs" Inherits="Service_confirmation" Title="Xác nhận đổi hàng bảo hành"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="VDMS.Common" Namespace="VDMS.Common.Web" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    </asp:Literal><asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Save"
        meta:resourcekey="ValidationSummary1Resource1" />
    <asp:BulletedList ID="bllErrorMsg" runat="server" CssClass="errorMsg" meta:resourcekey="bllErrorMsgResource1">
    </asp:BulletedList>
    <div class="form">
        <table border="0" cellpadding="2" cellspacing="2" style="width: 98%">
            <tr>
                <td class="InputTable">
                    <table border="0" cellpadding="2" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" /><asp:Literal
                                    ID="Literal2" runat="server" Text="Exchange date:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFromDate" runat="server" Width="88px" meta:resourcekey="txtFromDateResource1"></asp:TextBox><ajaxToolkit:MaskedEditExtender
                                                ID="meeFromDate" runat="server" TargetControlID="txtFromDate" Mask="99/99/9999"
                                                MaskType="Date" BehaviorID="meeFromDate" CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="MDY" CultureDatePlaceholder="/" CultureDecimalPlaceholder="."
                                                CultureThousandsPlaceholder="," CultureTimePlaceholder=":" Enabled="True" />
                                            <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtFromDate"
                                                PopupButtonID="ibFromDate" BehaviorID="ceFromDate" Enabled="True">
                                            </ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="Validator"
                                                SetFocusOnError="True" ValidationGroup="Save" ControlToValidate="txtFromDate"
                                                ErrorMessage='First "Exchange date" cannot be blank!' meta:resourcekey="rfvFromDateResource1"
                                                Text="*"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                                                meta:resourcekey="ibFromDateResource1" />
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;
                                        </td>
                                        <td>
                                            ~
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToDate" runat="server" Width="88px" meta:resourcekey="txtToDateResource1"></asp:TextBox><ajaxToolkit:MaskedEditExtender
                                                ID="meeToDate" runat="server" TargetControlID="txtToDate" Mask="99/99/9999" MaskType="Date"
                                                BehaviorID="meeToDate" CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="MDY" CultureDatePlaceholder="/" CultureDecimalPlaceholder="."
                                                CultureThousandsPlaceholder="," CultureTimePlaceholder=":" Enabled="True" />
                                            <ajaxToolkit:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtToDate"
                                                PopupButtonID="ibToDate" BehaviorID="ceToDate" Enabled="True">
                                            </ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="Validator" SetFocusOnError="True"
                                                ValidationGroup="Save" ControlToValidate="txtToDate" ErrorMessage='Last "Exchange date" cannot be blank!'
                                                meta:resourcekey="rfvToDateResource1" Text="*"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibToDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                                                meta:resourcekey="ibToDateResource1" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal4" runat="server" Text="Status:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="259px" meta:resourcekey="ddlStatusResource1">
                                    <asp:ListItem Text="Not Validate" Value="0" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                    <asp:ListItem Text="Validated" Value="1" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                    <asp:ListItem Selected="True" Text="All" Value="-1" meta:resourcekey="ListItemResource3"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal3" runat="server" Text="Dealer:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDealer" runat="server" MaxLength="30" Width="254px" meta:resourcekey="txtDealerResource1"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="litFieldModel" runat="server" meta:resourcekey="litFieldModelResource1"
                                    Text="Model:" Visible="False"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtModel" runat="server" MaxLength="30" meta:resourcekey="txtModelResource1"
                                    Width="254px" CssClass="hidden"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                                <asp:Button ID="cmdSubmit" runat="server" Text="Validate" ValidationGroup="Save"
                                    SkinID="SubmitButton" Width="76px" meta:resourcekey="cmdSubmitResource1" OnClick="cmdSubmit_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="right">
                            </td>
                            <td align="right">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div class="grid">
        <vdms:PageGridView ID="EmptyGridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            OnPreRender="EmptyGridView1_PreRender" OnRowDataBound="EmptyGridView1_RowDataBound"
            meta:resourcekey="EmptyGridView1Resource1" OnDataBinding="EmptyGridView1_DataBinding">
            <Columns>
                <asp:TemplateField HeaderText="No" meta:resourcekey="TemplateFieldResource1">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Dealer code" meta:resourcekey="TemplateFieldResource2">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="Literal5" Text='<%# Eval("DealerCode") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Dealer name" meta:resourcekey="TemplateFieldResource3">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="Literal6" Text='<%# Eval("DealerCode") %>' OnDataBinding="Literal6_DataBinding"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Model" meta:resourcekey="TemplateFieldResource8">
                    <ItemTemplate>
                        <asp:Literal ID="litModel" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Voucher number" meta:resourcekey="TemplateFieldResource4">
                    <ItemTemplate>
                        <asp:Literal ID="Literal7" runat="server" Text='<%# Eval("Id") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sent date" meta:resourcekey="TemplateFieldResource5">
                    <ItemTemplate>
                        <asp:Literal ID="Literal8" runat="server" Text='<%# Eval("Createddate") %>' OnDataBinding="Literal8_DataBinding"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" meta:resourcekey="TemplateFieldResource6">
                    <ItemTemplate>
                        <asp:Literal ID="Literal9" runat="server" Text='<%# Eval("Status") %>' OnDataBinding="Literal9_DataBinding"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField meta:resourcekey="TemplateFieldResource7">
                    <ItemStyle Width="10px" />
                    <ItemTemplate>
                        <asp:Button ID="Button1" runat="server" Text="Verify" OnClientClick='<%# Eval("Id") %>'
                            OnDataBinding="Button1_DataBinding" CommandArgument='<%# Eval("Status") %>' meta:resourceKey="Button1Resource1">
                        </asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <b>
                    <asp:Localize ID="litEmpty" runat="server" Text="No sheet found! Please change the condition search."
                        meta:resourcekey="litEmptyResource1"></asp:Localize></b>
            </EmptyDataTemplate>
            <%--<PagerSettings Position="Top" Mode="NextPreviousFirstLast" />
        <PagerTemplate>
            <div style="float: left;">
                <asp:Literal ID="litPageInfo" runat="server" meta:resourcekey="litPageInfoResource1"></asp:Literal></div>
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
    <asp:ObjectDataSource ID="odsExchangeHeader" runat="server" EnablePaging="True" SelectCountMethod="SelectCount"
        SelectMethod="Select" TypeName="VDMS.I.ObjectDataSource.ExchangeVoucherHeaderDataSource">
        <SelectParameters>
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:ControlParameter ControlID="txtFromDate" Name="fromDate" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="txtToDate" Name="toDate" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="ddlStatus" Name="status" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:ControlParameter ControlID="txtDealer" Name="dealerCode" PropertyName="Text"
                Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
