<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="tempSRS.aspx.cs" Inherits="Service_tempSRS" Title="Untitled Page" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <table border="0" cellpadding="2" cellspacing="0">
            <tr>
                <td align="left" colspan="2">
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Check"
                        meta:resourcekey="ValidationSummary1Resource1" />
                    <asp:BulletedList ID="bllErrorMsg" runat="server" CssClass="errorMsg" meta:resourcekey="bllErrorMsgResource1">
                    </asp:BulletedList>
                </td>
            </tr>
            <tr>
                <td class="nameField">
                    <asp:Literal ID="Literal2" runat="server" meta:resourcekey="Literal2Resource1" Text="Engine number:"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtEngineNumber" runat="server" meta:resourcekey="txtEngineNumberResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="nameField">
                    <asp:Image ID="Image3" runat="server" meta:resourcekey="Image4Resource1" SkinID="RequireField" />
                    <asp:Literal ID="Literal3" runat="server" meta:resourcekey="Literal3Resource1" Text="Repair date:"></asp:Literal>
                </td>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:TextBox ID="txtFrom" runat="server" meta:resourcekey="txtFromResource1"></asp:TextBox>&nbsp;<ajaxToolkit:CalendarExtender
                                    ID="CalendarExtender1" runat="server" PopupButtonID="ibtnCalendar" TargetControlID="txtFrom"
                                    BehaviorID="CalendarExtender1" Enabled="True">
                                </ajaxToolkit:CalendarExtender>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" BehaviorID="MaskedEditExtender2"
                                    CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY"
                                    CultureDatePlaceholder="/" CultureDecimalPlaceholder="." CultureThousandsPlaceholder=","
                                    CultureTimePlaceholder=":" Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFrom">
                                </ajaxToolkit:MaskedEditExtender>
                            </td>
                            <td>
                            </td>
                            <td style="width: 9px">
                                <asp:RequiredFieldValidator ID="rqvFromdate" runat="server" ControlToValidate="txtFrom"
                                    ErrorMessage='"Search date" cannot be blank!' meta:resourcekey="rqvBuydateResource1"
                                    SetFocusOnError="True" Text="*" ValidationGroup="Check"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:ImageButton ID="ibtnCalendar" runat="Server" meta:resourcekey="ibtnCalendarResource1"
                                    OnClientClick="return false;" SkinID="CalendarImageButton" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                ~
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;<asp:TextBox ID="txtTo" runat="server" meta:resourcekey="txtToResource1"></asp:TextBox><ajaxToolkit:CalendarExtender
                                    ID="CalendarExtender2" runat="server" PopupButtonID="ImageButton2" TargetControlID="txtTo"
                                    BehaviorID="CalendarExtender2" Enabled="True">
                                </ajaxToolkit:CalendarExtender>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" BehaviorID="MaskedEditExtender2"
                                    CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY"
                                    CultureDatePlaceholder="/" CultureDecimalPlaceholder="." CultureThousandsPlaceholder=","
                                    CultureTimePlaceholder=":" Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtTo">
                                </ajaxToolkit:MaskedEditExtender>
                                &nbsp;
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTo"
                                    ErrorMessage='"Search date" cannot be blank!' meta:resourcekey="rqvBuydateResource1"
                                    SetFocusOnError="True" Text="*" ValidationGroup="Check"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:ImageButton ID="ImageButton2" runat="server" meta:resourcekey="ibtnCalendarResource1"
                                    OnClientClick="return false;" SkinID="CalendarImageButton" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                        ValidationGroup="Check" meta:resourcekey="btnSearchResource1" CommandName="Search" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                <br />
                    <div class="grid">
                        <vdms:PageGridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            meta:resourcekey="GridView1Resource1" OnPageIndexChanging="GridView1_PageIndexChanging"
                            OnPreRender="GridView1_PreRender" OnSelectedIndexChanging="GridView1_SelectedIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="Createby" HeaderText="Created by" meta:resourcekey="BoundFieldResource0"
                                    SortExpression="Createby" Visible="False" />
                                <asp:TemplateField HeaderText="Servicedate" SortExpression="Servicedate" meta:resourcekey="TemplateFieldResource1">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Servicedate") %>' meta:resourcekey="TextBox1Resource1"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# FormatDate(Eval("Servicedate")) %>'
                                            meta:resourcekey="Label1Resource1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Dealercode" HeaderText="Dealer code" meta:resourcekey="BoundFieldResource5"
                                    SortExpression="Dealercode" Visible="True" />
                                <asp:BoundField DataField="Enginenumber" HeaderText="Enginenumber" SortExpression="Enginenumber"
                                    meta:resourcekey="BoundFieldResource1" />
                                <asp:BoundField DataField="Numberplate" HeaderText="Numberplate" SortExpression="Numberplate"
                                    meta:resourcekey="BoundFieldResource2" />
                                <asp:BoundField DataField="Itemtype" HeaderText="Itemtype" SortExpression="Itemtype"
                                    meta:resourcekey="BoundFieldResource3" />
                                <asp:BoundField DataField="Colorcode" HeaderText="Colorcode" SortExpression="Colorcode"
                                    meta:resourcekey="BoundFieldResource4" />
                                <asp:TemplateField HeaderText="Totalamount" meta:resourcekey="TemplateFieldResource2"
                                    SortExpression="Totalamount">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox3" runat="server" meta:resourcekey="TextBox3Resource1" Text='<%# Bind("Totalamount") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" meta:resourcekey="Label3Resource1" Text='<%# FormatNumber(Eval("Totalamount")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Kmcount" meta:resourcekey="TemplateFieldResource3"
                                    SortExpression="Kmcount">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox4" runat="server" meta:resourcekey="TextBox4Resource1" Text='<%# Bind("Kmcount") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" meta:resourcekey="Label4Resource1" Text='<%# FormatNumber(Eval("Kmcount")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Purchasedate" SortExpression="Purchasedate" meta:resourcekey="TemplateFieldResource4">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Purchasedate") %>' meta:resourcekey="TextBox2Resource1"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# FormatDate(Eval("Purchasedate")) %>'
                                            meta:resourcekey="Label2Resource1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:HyperLinkField Target="_blank" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="WarrantyContent.aspx?sid={0}"
                                    Text="&lt;img Title=&quot;Edit&quot; src=&quot;../Images/Edit.gif&quot; Alt=&quot;Edit&quot; border=&quot;0&quot; /&gt;"
                                    meta:resourcekey="HyperLinkFieldResource1" />
                                <asp:TemplateField meta:resourcekey="TemplateFieldResource5" ShowHeader="False" Visible="False">
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Update" ImageUrl="~/Images/update.gif"
                                            meta:resourcekey="ImageButton1Resource1" Text="Update" />
                                        &nbsp;<asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False"
                                            CommandName="Cancel" ImageUrl="~/Images/cancel.gif" meta:resourcekey="ImageButton2Resource1"
                                            Text="Cancel" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id") %>'
                                            CommandName="Edit" ImageUrl="~/Images/Edit.gif" meta:resourcekey="ImageButton1Resource2"
                                            OnClick="ImageButton1_Click" Text="Edit" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField meta:resourcekey="TemplateFieldResource5">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id") %>'
                                            CommandName="Delete" ImageUrl="~/Images/Delete.gif" meta:resourcekey="imgbDeleteResource1"
                                            OnClick="imgbDelete_Click" OnDataBinding="imgbDelete_DataBinding" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <%--<PagerTemplate>
                        <div style="float: left;">
                            <asp:Literal ID="litPageInfo" runat="server" meta:resourcekey="litPageInfoResource1"></asp:Literal></div>
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
                    </PagerTemplate>
                    <PagerSettings Position="Top" />--%>
                        </vdms:PageGridView>
                    </div>
                    <asp:ObjectDataSource ID="SRSObjectDataSource" runat="server" SelectMethod="SelectTemp"
                        TypeName="VDMS.I.ObjectDataSource.ServiceHeaderDataSource" EnablePaging="True"
                        SelectCountMethod="SelectTempCount">
                        <SelectParameters>
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:ControlParameter ControlID="txtEngineNumber" Name="EngineNo" PropertyName="Text"
                                Type="String" />
                            <asp:ControlParameter ControlID="txtFrom" Name="fromDate" PropertyName="Text" Type="String" />
                            <asp:ControlParameter ControlID="txtTo" Name="toDate" PropertyName="Text" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
