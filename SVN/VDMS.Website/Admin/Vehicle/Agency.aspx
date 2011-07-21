<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="Agency.aspx.cs" Inherits="Admin_Database_Agency" Title="Modify Import Information to Dealer"
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
    <asp:ValidationSummary ID="ValidationSummary1" CssClass="error" runat="server" ValidationGroup="Save"
        meta:resourcekey="ValidationSummary1Resource1" />
    <div class="form">
        <table border="0" cellspacing="2" width="480px">
            <tr>
                <td valign="top" align="right">
                    <asp:Literal ID="litDate" runat="server" Text="Date:" meta:resourcekey="litDateResource1"></asp:Literal>
                </td>
                <td valign="top" colspan="3">
                    <asp:TextBox ID="txtFromDate" runat="server" Width="88px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>&nbsp;
                    <asp:ImageButton ID="ImageButton1" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ImageButton1Resource1" />
                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" SetFocusOnError="True"
                        ControlToValidate="txtFromDate" ErrorMessage='From Date is required.' meta:resourcekey="rfvFromDateResource1"
                        Text="*"></asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtFromDate"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="MaskedEditExtender2" CultureAMPMPlaceholder="AM;PM"
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                        CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                        Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtFromDate"
                        PopupButtonID="ImageButton1" BehaviorID="CalendarExtender3" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    <asp:Localize ID="litTodate" runat="server" Text="~" meta:resourcekey="litTodateResource1"></asp:Localize>&nbsp;
                    <asp:TextBox ID="txtToDate" runat="server" Width="88px" meta:resourcekey="txtToDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton2" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ImageButton2Resource1" />
                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" SetFocusOnError="True"
                        ControlToValidate="txtToDate" ErrorMessage='To date is required.' meta:resourcekey="rfvToDateResource1"
                        Text="*"></asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtToDate"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="MaskedEditExtender1" CultureAMPMPlaceholder="AM;PM"
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                        CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                        Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="ImageButton2" BehaviorID="CalendarExtender1" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right">
                    <asp:Literal ID="litStatus" runat="server" Text="Status:" meta:resourcekey="litStatus"></asp:Literal>
                </td>
                <td valign="top" colspan="3">
                    <asp:DropDownList ID="ddlStatus" runat="server" Width="150px" meta:resourcekey="ddlStatusResource2">
                        <asp:ListItem Text="All" Value="1" meta:resourcekey="ListItemResource1"></asp:ListItem>
                        <asp:ListItem Text="Unreplied" Value="2" meta:resourcekey="ListItemResource2"></asp:ListItem>
                        <asp:ListItem Text="Replied" Value="3" meta:resourcekey="ListItemResource3"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top">
                </td>
                <td colspan="6" valign="top">
                    <asp:Button ID="btnTest" runat="server" Text="Search" OnClick="btnTest_Click" meta:resourcekey="btnTestResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <vdms:PageGridView ID="grvMaster" runat="server" AutoGenerateColumns="False" AllowPaging="True"
            OnDataBound="grvMaster_DataBound" OnRowDataBound="grvMaster_RowDataBound" DataSourceID="AgencyDataSource1"
            DataKeyNames="Id" meta:resourcekey="grvMasterResource1">
            <Columns>
                <asp:BoundField HeaderText="No." ReadOnly="True" meta:resourcekey="BoundFieldResource1" />
                <asp:TemplateField HeaderText="Imported Date" meta:resourcekey="TemplateFieldResource1">
                    <ItemTemplate>
                        <%# ((DateTime)(Eval("Shippingheader.Shippingdate"))).ToShortDateString() %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Dealer" meta:resourcekey="TemplateFieldResource2">
                    <ItemTemplate>
                        <%# Eval("Shippingheader.Dealercode") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Shipping Number" meta:resourcekey="TemplateFieldResource3">
                    <ItemTemplate>
                        <%# Eval("Shippingheader.Shippingnumber") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Item Code" DataField="Itemtype" ReadOnly="True" meta:resourcekey="BoundFieldResource2" />
                <asp:BoundField HeaderText="Color" DataField="Color" ReadOnly="True" meta:resourcekey="BoundFieldResource3" />
                <asp:BoundField HeaderText="Engine Number" DataField="Enginenumber" ReadOnly="True"
                    meta:resourcekey="BoundFieldResource4" />
                <asp:BoundField HeaderText="Dealer comment" DataField="Exception" ReadOnly="True"
                    meta:resourcekey="BoundFieldResource5" />
                <asp:TemplateField HeaderText="Reply content" meta:resourcekey="TemplateFieldResource4">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditVMEPResponse" runat="server" Text='<%# Bind("Vmepresponse") %>'
                            meta:resourcekey="txtEditVMEPResponseResource1"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="lblVMEPResponse" runat="server" Text='<%# Bind("Vmepresponse") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Reply date" meta:resourcekey="TemplateFieldResource5">
                    <ItemTemplate>
                        <%# (DateTime)(Eval("Vmepresponsedate")) != DateTime.MinValue ? ((DateTime)(Eval("Vmepresponsedate"))).ToShortDateString() : "" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Action" meta:resourcekey="TemplateFieldResource6">
                    <EditItemTemplate>
                        <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="Reply" meta:resourcekey="btnUpdateResource1" />
                        <asp:Button ID="btnCancel" runat="server" CommandName="Delete" Text="Delete" meta:resourcekey="btnCancelResource1" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="imgEdit" runat="server" CommandName="Edit" ImageUrl="~/Images/Edit.gif"
                            ToolTip="Edit" meta:resourcekey="imgEditResource1" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <b>
                    <asp:Localize ID="litEmpty" runat="server" Text="Data not found. Please change search condition"
                        meta:resourcekey="litEmptyResource1"></asp:Localize></b>
            </EmptyDataTemplate>
        </vdms:PageGridView>
    </div>
    <asp:ObjectDataSource ID="AgencyDataSource1" runat="server" SelectCountMethod="SelectCount"
        SelectMethod="Select" TypeName="VDMS.I.ObjectDataSource.AgencyDataSource" EnablePaging="True"
        DeleteMethod="Delete" UpdateMethod="Update">
        <SelectParameters>
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:ControlParameter ControlID="txtFromDate" Name="fromDate" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="txtToDate" Name="toDate" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="ddlStatus" Name="status" PropertyName="SelectedValue"
                Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
