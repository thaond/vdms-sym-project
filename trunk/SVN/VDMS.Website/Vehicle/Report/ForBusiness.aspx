<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="ForBusiness.aspx.cs" Inherits="Sales_Report_Default4" Title="Biểu thống kê đặt nhập bán tồn dùng cho kinh doanh"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
    <div class="form" style="width: 100%">
        <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Report" CssClass="error"
            runat="server" />
        <table cellspacing="0" border="0">
            <tr>
                <td>
                    <h5>
                        <asp:Literal ID="Literal2" runat="server" Text="Ngày đổi"></asp:Literal></h5>
                </td>
                <td style="width: 10px;">
                </td>
                <td>
                    <asp:Localize ID="litDateRepair" Text="Từ ngày:" runat="server" meta:resourcekey="litDateRepairResource1"></asp:Localize>
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" Width="88px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="Validator"
                        SetFocusOnError="True" ValidationGroup="Report" ControlToValidate="txtFromDate"
                        meta:resourcekey="rfvFromDateResource1" Text="*"></asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="meeFromDate">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtFromDate"
                        PopupButtonID="ibFromDate" BehaviorID="ceFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td style="width: 10px;">
                </td>
                <td>
                    <asp:Localize ID="litTodate" runat="server" Text="Đến ngày" meta:resourcekey="litTodateResource1"></asp:Localize>&nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" Width="88px" meta:resourcekey="txtToDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibToDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibToDateResource1" />
                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="Validator" SetFocusOnError="True"
                        ValidationGroup="Report" ControlToValidate="txtToDate" meta:resourcekey="rfvToDateResource1"
                        Text="*"></asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="meeToDate" runat="server" TargetControlID="txtToDate"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="meeToDate">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="ibToDate" BehaviorID="ceToDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    <h5>
                        <asp:Literal ID="Literal3" runat="server" Text="Thẩm tra"></asp:Literal></h5>
                </td>
                <td style="width: 10px;">
                </td>
                <td>
                    <asp:Localize ID="Localize1" Text="Từ ngày:" runat="server"></asp:Localize>
                </td>
                <td>
                    <asp:TextBox ID="txtProcessFrom" runat="server" Width="88px"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton1" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtProcessFrom"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="meeFromDate">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtProcessFrom"
                        PopupButtonID="ImageButton1" BehaviorID="ceFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td style="width: 10px;">
                </td>
                <td>
                    <asp:Localize ID="Localize2" runat="server" Text="Đến ngày" meta:resourcekey="litTodateResource1"></asp:Localize>&nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtProcessTo" runat="server" Width="88px"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton2" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtProcessTo"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="meeToDate">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtProcessTo"
                        PopupButtonID="ImageButton2" BehaviorID="ceToDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
                <td valign="top">
                    <asp:Literal ID="Literal1" runat="server" Text="Status" meta:resourcekey="litStatus"></asp:Literal>
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddlStatus" runat="server" DataSourceID="odsExchangeStatus"
                        DataTextField="ValueString" DataValueField="Value">
                    </asp:DropDownList>
                    <asp:ObjectDataSource ID="odsExchangeStatus" runat="server" SelectMethod="SelectForServiceRpt"
                        TypeName="VDMS.I.ObjectDataSource.ExchangePartHeaderStatusDataSource"></asp:ObjectDataSource>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                </td>
                <td>
                    <asp:Button ID="btnSubmit" ValidationGroup="Report" runat="server" Text="Tìm kiếm"
                        SkinID="SubmitButton" OnClick="btnSubmit_Click" meta:resourcekey="btnSubmitResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <vdms:PageGridView ID="gvMain" runat="server" AllowPaging="True" OnDataBound="gvMain_DataBound"
            AutoGenerateColumns="False" OnRowDataBound="gvMain_RowDataBound" DataKeyNames="Id"
            meta:resourcekey="gvMainResource1">
            <EmptyDataTemplate>
                <b>
                    <asp:Localize ID="litEmpty" runat="server" Text="Kh&#244;ng t&#236;m thấy dữ liệu n&#224;o! Bạn c&#243; thể thay đổi điều kiện t&#236;m kiếm. Cảm ơn."
                        meta:resourcekey="litEmptyResource1"></asp:Localize></b>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="STT" meta:resourcekey="TemplateFieldResource1"></asp:TemplateField>
                <asp:TemplateField HeaderText="Số Parts change voucher" meta:resourcekey="TemplateFieldResource2">
                    <ItemTemplate>
                        <asp:Literal ID="litVouchernumber" runat="server" Text='<%# Eval("Exchangepartheader.Vouchernumber") %>'></asp:Literal>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Literal ID="litTotal" runat="server" Text="Total" meta:resourcekey="litTotalResource1"></asp:Literal>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" meta:resourcekey="TemplateFieldResourceStatus">
                    <ItemTemplate>
                        <asp:Literal ID="litStatus" runat="server" Text='<%# EvalExchangeStatus(Eval("Exchangepartheader.Status")) %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="V&#249;ng" meta:resourcekey="TemplateFieldResource3">
                    <ItemTemplate>
                        <asp:Literal ID="litAreaCode" runat="server" Text='<%# EvalAreaCode(Eval("Exchangepartheader.Areacode")) %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="T&#234;n đại l&#253;" meta:resourcekey="TemplateFieldResource4">
                    <ItemTemplate>
                        <asp:Literal ID="litDealercode" runat="server" Text='<%# EvalDealerName(Eval("Exchangepartheader.Dealercode")) %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="T&#234;n kh&#225;ch h&#224;ng" meta:resourcekey="TemplateFieldResource5">
                    <ItemTemplate>
                        <asp:Literal ID="litCustomerFullname" runat="server" Text='<%# Eval("Exchangepartheader.Customer.Fullname") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Địa chỉ kh&#225;ch h&#224;ng" meta:resourcekey="TemplateFieldResource6">
                    <ItemTemplate>
                        <asp:Literal ID="litAdrs" runat="server" meta:resourcekey="litAdrsResource1"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Giới t&#237;nh" meta:resourcekey="TemplateFieldResource7">
                    <ItemTemplate>
                        <asp:Literal ID="litGender" runat="server" Text='<%# EvalGender(Eval("Exchangepartheader.Customer.Gender")) %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Số điện thoại" meta:resourcekey="TemplateFieldResource8">
                    <ItemTemplate>
                        <asp:Literal ID="litMobile" runat="server" Text='<%# Eval("Exchangepartheader.Customer.Mobile") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Pur Date" meta:resourcekey="TemplateFieldResource12">
                    <ItemTemplate>
                        <asp:Literal ID="litPurDate" runat="server" Text='<%# EvalDate(Eval("Exchangepartheader.Purchasedate")) %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Damage date" meta:resourcekey="TemplateFieldResource22">
                    <ItemTemplate>
                        <asp:Literal ID="litDamageDate" runat="server" Text='<%# Eval("Exchangepartheader.Damageddate") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Change Date" meta:resourcekey="TemplateFieldResource14">
                    <ItemTemplate>
                        <asp:Literal ID="litChangeDate" runat="server" Text='<%# EvalDate(Eval("Exchangepartheader.Exchangeddate")) %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Processed Date" meta:resourcekey="TemplateFieldResource24">
                    <ItemTemplate>
                        <asp:Literal ID="litProcessDate" runat="server" Text='<%# EvalDate(Eval("Exchangepartheader.Exchangevoucherheader.Lastprocesseddate")) %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Eng No" meta:resourcekey="TemplateFieldResource9">
                    <ItemTemplate>
                        <asp:Literal ID="litEngNo" runat="server" Text='<%# Eval("Exchangepartheader.Enginenumber") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fr.No" meta:resourcekey="TemplateFieldResource10">
                    <ItemTemplate>
                        <asp:Literal ID="litFrNo" runat="server" Text='<%# Eval("Exchangepartheader.Framenumber") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Model" meta:resourcekey="TemplateFieldResource11">
                    <ItemTemplate>
                        <asp:Literal ID="litModel" runat="server" Text='<%# Eval("Exchangepartheader.Model") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="KM" meta:resourcekey="TemplateFieldResource13">
                    <ItemTemplate>
                        <asp:Literal ID="litKM" runat="server" Text='<%# Eval("Exchangepartheader.Kmcount") %>'></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Broken No" meta:resourcekey="TemplateFieldResource15">
                    <ItemTemplate>
                        <asp:Literal ID="litBrokenNo" runat="server" Text='<%# Eval("Broken.Brokencode") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Broken Name" meta:resourcekey="TemplateFieldResource16">
                    <ItemTemplate>
                        <asp:Literal ID="litBrokenName" runat="server" Text='<%# Eval("Broken.Brokenname") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Partcodem" HeaderText="Part No" meta:resourcekey="BoundFieldResource1" />
                <asp:TemplateField HeaderText="Part Name" meta:resourcekey="TemplateFieldResource17">
                    <ItemTemplate>
                        <asp:Literal ID="litPartName" runat="server" meta:resourcekey="litPartNameResource1"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Serial number" meta:resourcekey="TemplateFieldResource17S">
                    <ItemTemplate>
                        <asp:Literal ID="litSerial" runat="server" Text='<%# Eval("Serialnumber") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Partqtym" HeaderText="Qua" meta:resourcekey="BoundFieldResource2">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Unitpricem" HeaderText="U/PRICE (VND)" DataFormatString="{0:n0}"
                    HtmlEncode="False" meta:resourcekey="BoundFieldResource3">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Part Cost (VND)" meta:resourcekey="TemplateFieldResource19">
                    <ItemTemplate>
                        <asp:Literal ID="litPartCost" runat="server" meta:resourcekey="litPartCostResource1"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="FRT" meta:resourcekey="TemplateFieldResource18">
                    <ItemTemplate>
                        <asp:Literal ID="litFrt" runat="server" meta:resourcekey="litFrtResource1"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Manpower verified" meta:resourcekey="TemplateFieldResource23">
                    <ItemTemplate>
                        <asp:Literal ID="litManM" runat="server" Text='<%# Eval("Totalfeem") %>'></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Labour Cost (VND)" meta:resourcekey="TemplateFieldResource20">
                    <ItemTemplate>
                        <asp:Literal ID="litLabourCost" runat="server" EnableViewState="False" Text='<%# Eval("Totalfeem") %>'></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total (VND)" meta:resourcekey="TemplateFieldResource21">
                    <ItemTemplate>
                        <asp:Literal ID="litTotal" runat="server" meta:resourcekey="litTotalResource2"></asp:Literal>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Literal ID="litTotalTotal" runat="server" meta:resourcekey="litTotalTotalResource1"></asp:Literal>
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:BoundField DataField="Vmepcomment" HeaderText="Ghi ch&#250;" meta:resourcekey="BoundFieldResource4" />
            </Columns>
        </vdms:PageGridView>
    </div>
    <asp:Button ID="btnExport2Exel" runat="server" OnClick="btnExport2Exel_Click" Text="Export to Exel"
        meta:resourcekey="btnExport2ExelResource1" /><br />
    <asp:ObjectDataSource ID="ExchangePartDetailDataSource1" runat="server" EnablePaging="True"
        SelectCountMethod="SelectCount" SelectMethod="Select" 
        TypeName="VDMS.I.ObjectDataSource.ExchangePartDetailDataSource" 
        OldValuesParameterFormatString="original_{0}">
        <SelectParameters>
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:ControlParameter ControlID="txtFromDate" Name="fromDate" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="txtToDate" Name="toDate" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtProcessFrom" Name="processFromDate" 
                PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtProcessTo" Name="processToDate" 
                PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="ddlStatus" Name="status" PropertyName="SelectedValue"
                Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
