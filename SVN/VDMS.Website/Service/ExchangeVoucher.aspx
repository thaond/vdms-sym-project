<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="ExchangeVoucher.aspx.cs" Inherits="Service_ExchangeVoucher" Title="Untitled Page"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <asp:BulletedList ID="bllMessage" runat="server" ForeColor="Red" meta:resourcekey="bllMessageResource1">
        </asp:BulletedList>
        <table cellspacing="2" cellpadding="2"  border="0">
            <tr>
                <td align="left">
                    <asp:Literal ID="Literal2" runat="server" Text="Số phiếu đề nghị:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td class="valueField">
                    <asp:Label ID="lblVoucherNumber" runat="server" meta:resourcekey="lblVoucherNumberResource1"></asp:Label>
                </td>
                <td colspan="5">
                </td>
            </tr>
            <tr>
                <td valign="middle">
                    <asp:Literal ID="Literal38" runat="server" meta:resourcekey="Literal3x2Resource"
                        Text="Dealer code:"></asp:Literal>
                </td>
                <td colspan="5" valign="top">
                    <asp:DropDownList ID="ddlDealer" runat="server" OnDataBound="ddlDealer_DataBound"
                        OnSelectedIndexChanged="ddlDealer_SelectedIndexChanged" Width="100%" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td valign="top">
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
                    <asp:Localize ID="litDateRepair" Text="Ngày sửa chữa:" runat="server" meta:resourcekey="litDateRepairResource1"></asp:Localize>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtFromDate" runat="server" Width="88px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="Validator"
                        SetFocusOnError="True" ValidationGroup="Save" ControlToValidate="txtFromDate"
                        meta:resourcekey="rfvFromDateResource1" Text="*"></asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="meeFromDate" Enabled="True" />
                    <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtFromDate"
                        PopupButtonID="ibFromDate" BehaviorID="ceFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td class="field_name" valign="middle">
                    <asp:Localize ID="litTodate" runat="server" Text="~" meta:resourcekey="litTodateResource1"></asp:Localize>&nbsp;
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtToDate" runat="server" Width="88px" meta:resourcekey="txtToDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibToDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibToDateResource1" />
                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="Validator" SetFocusOnError="True"
                        ValidationGroup="Save" ControlToValidate="txtToDate" meta:resourcekey="rfvToDateResource1"
                        Text="*"></asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="meeToDate" runat="server" TargetControlID="txtToDate"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="meeToDate" Enabled="True" />
                    <ajaxToolkit:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="ibToDate" BehaviorID="ceToDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td valign="top">
                    &nbsp;</td>
                <td valign="top">
                    <%--<asp:DropDownList ID="ddlStatus" runat="server">
                    <asp:ListItem Selected="True" Text="Not Process" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Confirmed" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Approved" Value="4"></asp:ListItem>
                </asp:DropDownList>--%>
                </td>
                <td valign="top">
                    &nbsp;</td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Localize ID="litStatus" runat="server" Text="Trạng thái:" meta:resourcekey="litStatusResource1"></asp:Localize>
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddlStatus" runat="server" DataSourceID="ExchangePartHeaderStatusDataSource1"
                        DataTextField="ValueString" DataValueField="Value" AutoPostBack="True" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                        meta:resourcekey="ddlStatusResource1">
                    </asp:DropDownList>
                </td>
                <td class="field_name" valign="middle">
                    &nbsp;</td>
                <td valign="top">
                    &nbsp;</td>
                <td valign="top">
                    &nbsp;</td>
                <td valign="top">
                    &nbsp;</td>
                <td valign="top">
                    &nbsp;</td>
            </tr>
            <tr>
                <td valign="top">
                    &nbsp;</td>
                <td valign="top">
                    <asp:Button ID="btnSubmit" runat="server" Text="Tìm kiếm" ValidationGroup="Save"
                        SkinID="SubmitButton" OnClick="btnSubmit_Click" meta:resourcekey="btnSubmitResource1" />
                </td>
                <td class="field_name" valign="middle">
                    &nbsp;</td>
                <td valign="top">
                    &nbsp;</td>
                <td valign="top">
                    &nbsp;</td>
                <td valign="top">
                    &nbsp;</td>
                <td valign="top">
                    &nbsp;</td>
            </tr>
        </table>
    </div>
    <br />
    <table>
        <tr>
            <td>
                <div class="grid">
                    <vdms:PageGridView ID="gvMain" runat="server" AllowPaging="True" OnDataBound="gvMain_DataBound"
                        AutoGenerateColumns="False" OnRowDataBound="gvMain_RowDataBound" DataKeyNames="Id"
                        ShowFooter="True" meta:resourcekey="gvMainResource1" DataSourceID="ExchangePartHeaderDataSource1">
                        <%--<PagerSettings Position="Top" Mode="NextPreviousFirstLast" />
					<PagerTemplate>
						<div style="float: left;">
							<asp:Literal ID="litPageInfo" runat="server" meta:resourcekey="litPageInfoResource1"></asp:Literal></div>
						<div style="text-align: right; float: right;">
							<asp:Button ID="cmdFirst" runat="server" Text="Trang 1" CommandName="Page" CommandArgument="First"
								meta:resourcekey="cmdFirstResource1" />
							<asp:Button ID="cmdPrevious" runat="server" Text="Trang trước" CommandName="Page"
								CommandArgument="Prev" meta:resourcekey="cmdPreviousResource1" />
							<asp:Button ID="cmdNext" runat="server" Text="Trang sau" CommandName="Page" CommandArgument="Next"
								meta:resourcekey="cmdNextResource1" />
							<asp:Button ID="cmdLast" runat="server" Text="Trang cuối" CommandName="Page" CommandArgument="Last"
								meta:resourcekey="cmdLastResource1" />
						</div>
					</PagerTemplate>--%>
                        <EmptyDataTemplate>
                            <b><asp:Localize ID="litEmpty" runat="server" Text="Kh&#244;ng t&#236;m thấy dữ liệu n&#224;o!"
                                    meta:resourcekey="litEmptyResource1"></asp:Localize></b>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:BoundField HeaderText="STT" meta:resourcekey="BoundFieldResource2" />
                            <asp:TemplateField HeaderText="Ng&#224;y sửa" meta:resourcekey="TemplateFieldResource2">
                                <ItemTemplate>
                                    <asp:Literal ID="litExchangeddate" runat="server" Text='<%# EvalDate(Eval("Exchangeddate")) %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Enginenumber" HeaderText="Số m&#225;y" meta:resourcekey="BoundFieldResource1" />
                            <asp:TemplateField HeaderText="T&#234;n kh&#225;ch h&#224;ng" meta:resourcekey="TemplateFieldResource3">
                                <ItemTemplate>
                                    <asp:Literal ID="litCustomerFullname" runat="server" Text='<%# Eval("Customer.Fullname") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Số phiếu sửa chữa" meta:resourcekey="TemplateFieldResource4">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkServiceheaderServicesheetnumber" runat="server" Text='<%# Eval("Serviceheader.Servicesheetnumber") %>'
                                        NavigateUrl='<%# EvalLink("srsn",Eval("Serviceheader.Servicesheetnumber")) %>'
                                        meta:resourcekey="lnkServiceheaderServicesheetnumberResource1"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Số phiếu thay thế phụ t&#249;ng" meta:resourcekey="TemplateFieldResource5">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkVouchernumber" runat="server" NavigateUrl='<%# EvalLink("pcvn",Eval("Vouchernumber")) %>'
                                        Text='<%# Eval("Vouchernumber") %>' meta:resourcekey="lnkVouchernumberResource1"></asp:HyperLink>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <asp:Literal ID="litTotal" runat="server" Text="Th&#224;nh tiền" meta:resourcekey="litTotalResource1"></asp:Literal></b>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Số lượng" meta:resourcekey="TemplateFieldResource6">
                                <ItemTemplate>
                                    <asp:Literal ID="litQuantity" runat="server" meta:resourcekey="litQuantityResource1"></asp:Literal>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Literal ID="litTotalQuantity" runat="server" meta:resourcekey="litTotalQuantityResource1"></asp:Literal>
                                </FooterTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tiền phụ t&#249;ng bảo h&#224;nh (VND)" meta:resourcekey="TemplateFieldResource7">
                                <ItemTemplate>
                                    <asp:Literal ID="litPartCost" runat="server" meta:resourcekey="litPartCostResource1"></asp:Literal>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Literal ID="litTotalPartCost" runat="server" meta:resourcekey="litTotalPartCostResource1"></asp:Literal>
                                </FooterTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tổng tiền c&#244;ng bảo h&#224;nh (VND)" meta:resourcekey="TemplateFieldResource8">
                                <ItemTemplate>
                                    <asp:Literal ID="litHireCost" runat="server" meta:resourcekey="litHireCostResource1"></asp:Literal>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Literal ID="litTotalHireCost" runat="server" meta:resourcekey="litTotalHireCostResource1"></asp:Literal>
                                </FooterTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tổng tiền (VND)" meta:resourcekey="TemplateFieldResource9">
                                <ItemTemplate>
                                    <asp:Literal ID="litTotal" runat="server" meta:resourcekey="litTotalResource2"></asp:Literal>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Literal ID="litTotalTotal" runat="server" meta:resourcekey="litTotalTotalResource1"></asp:Literal>
                                </FooterTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnCancel" runat="server" CommandArgument='<%# Eval("Status") %>'
                                        Text="Cancel" Width="61px" OnClick="btnCancel_Click" OnDataBinding="btnCancel_Load"
                                        ToolTip='<%# Eval("Vouchernumber") %>' CommandName="CancelExchangeVoucher" /><asp:Button
                                            ID="btnRecover" runat="server" CommandArgument='<%# Eval("Status") %>' Text="Recover"
                                            Width="61px" OnClick="btnRecover_Click" OnDataBinding="btnRecover_Load" ToolTip='<%# Eval("Vouchernumber") %>'
                                            CommandName="RecoverExchangeVoucher" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </vdms:PageGridView>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnSend" runat="server" Text="Gửi cho VMEP" Enabled="False" OnClick="btnSend_Click"
                    meta:resourcekey="btnSendResource1" />
            </td>
        </tr>
    </table>
    <asp:ObjectDataSource ID="ExchangePartHeaderDataSource1" runat="server" EnablePaging="True"
        SelectCountMethod="SelectCount" SelectMethod="Select" TypeName="VDMS.I.ObjectDataSource.ExchangePartHeaderDataSource">
        <SelectParameters>
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="fromDate" Type="String" />
            <asp:Parameter Name="toDate" Type="String" />
            <asp:Parameter Name="status" Type="String" />
            <asp:ControlParameter Name="dealerCode" Type="string" ControlID="ddlDealer" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ExchangePartHeaderStatusDataSource1" runat="server" SelectMethod="Select"
        TypeName="VDMS.I.ObjectDataSource.ExchangePartHeaderStatusDataSource"></asp:ObjectDataSource>
</asp:Content>
