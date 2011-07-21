<%@ Page Title="Common message" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="Article.aspx.cs" Inherits="Admin_Database_Article"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form" style="width: 450px">
        <table cellspacing="2" border="0" width="100%">
            <tr>
                <td valign="top">
                    <asp:Localize ID="litDateTime" Text="Date:" runat="server" meta:resourcekey="litDateTimeResource1"></asp:Localize>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtFromDate" runat="server" Width="88px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibFromDate" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="Validator"
                        SetFocusOnError="True" ValidationGroup="Save" ControlToValidate="txtFromDate"
                        meta:resourcekey="rfvFromDateResource1">*</asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                        CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtFromDate"
                        PopupButtonID="ibFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    ~&nbsp;
                    <asp:TextBox ID="txtToDate" runat="server" Width="88px" meta:resourcekey="txtToDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibToDate" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibToDateResource1" />
                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="Validator" SetFocusOnError="True"
                        ValidationGroup="Save" ControlToValidate="txtToDate" meta:resourcekey="rfvToDateResource1">*</asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="meeToDate" runat="server" TargetControlID="txtToDate"
                        Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                        CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="ibToDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td valign="top">
                </td>
                <td valign="top">
                    <asp:Button ID="btnSubmit" runat="server" Text="Search" ValidationGroup="Save" SkinID="SubmitButton"
                        OnClick="btnSubmit_Click" meta:resourcekey="btnSubmitResource1" />
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div class="grid">
        <vdms:PageGridView ID="grdArticles" runat="server" AutoGenerateColumns="False" AllowPaging="True"
            DataKeyNames="MessageId" DataSourceID="ObjectDataSource1" meta:resourcekey="grdArticlesResource1">
            <Columns>
                <asp:BoundField DataField="CreatedDate" HeaderText="Publish date" HtmlEncode="False"
                    DataFormatString="{0:d}" meta:resourcekey="BoundFieldResource1" />
                <asp:TemplateField HeaderText="Content" meta:resourcekey="BoundFieldResource2">
                    <ItemStyle Width="60%" />
                    <ItemTemplate>
                        <%# ((Eval("BodyNonHTML").ToString()).Length > 103) ? Eval("BodyNonHTML").ToString().Substring(0, 100) + "..." : Eval("BodyNonHTML")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderImageUrl="~/Images/paperclip.gif" meta:resourcekey="TemplateFieldResource1">
                    <ItemTemplate>
                        <asp:Repeater ID="repAttachment" runat="server" DataSource='<%# Eval("Files") %>'>
                            <ItemTemplate>
                                <asp:HyperLink ID="hlAttachment" runat="server" Text='<%# Left((string)Eval("Filename")) %>'
                                    ToolTip='<%# Eval("Filename") %>' NavigateUrl='<%# string.Concat("~/download.ashx?id=", Eval("FileId")) %>'></asp:HyperLink>
                            </ItemTemplate>
                            <SeparatorTemplate>
                                ,</SeparatorTemplate>
                        </asp:Repeater>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Created by" meta:resourcekey="TemplateFieldResource2">
                    <ItemTemplate>
                        <%# Dept((string)Eval("Createdby")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource3">
					<ItemTemplate>
						<asp:ImageButton ID="imgbEdit" runat="server" CausesValidation="False" CommandName="Edit"
							ToolTip="Edit" ImageUrl="~/Images/Edit.gif" meta:resourcekey="imgbEditResource1" />
						<asp:HyperLink ID="e" runat="server" ImageUrl="~/Images/Edit.gif" ToolTip="Edit"
							NavigateUrl="~/Admin/Database/ArticleEdit.aspx?id="></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>--%>
                <asp:HyperLinkField HeaderImageUrl="~/Images/Edit.gif" Text="..." DataNavigateUrlFields="MessageId"
                    DataNavigateUrlFormatString="~/Admin/Database/ArticleEdit.aspx?id={0}" ItemStyle-HorizontalAlign="Center" />
                <asp:ButtonField HeaderImageUrl="~/Images/Delete.gif" CommandName="Delete" CausesValidation="false"
                    Text="..." ItemStyle-HorizontalAlign="Center" />
                <%--<asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource4">
					<ItemTemplate>
						<asp:ImageButton ID="imgbDelete" runat="server" CausesValidation="False" CommandName="Delete"
							ToolTip="Delete" ImageUrl="~/Images/Delete.gif" meta:resourcekey="imgbDeleteResource1" />
					</ItemTemplate>
				</asp:TemplateField>--%>
            </Columns>
            <EmptyDataTemplate>
                <b>
                    <asp:Localize ID="litArticleNotFound" runat="server" Text="Article in system not found!"
                        meta:resourcekey="litArticleNotFoundResource1"></asp:Localize>
                </b>
            </EmptyDataTemplate>
        </vdms:PageGridView>
    </div>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="FindCommonMessage"
        DeleteMethod="Delete" TypeName="VDMS.II.BasicData.MessageDAO" EnablePaging="True"
        SelectCountMethod="GetCommonMessageCount">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtFromDate" Name="fromDate" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="txtToDate" Name="toDate" PropertyName="Text" Type="String" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="MessageId" Type="Int32" />
        </DeleteParameters>
    </asp:ObjectDataSource>
    <asp:Button ID="cmdAddNew" runat="server" SkinID="SubmitButton" Text="Add new Message"
        OnClientClick="javascript:location.href='ArticleEdit.aspx'; return false;" meta:resourcekey="cmdAddNewResource1" />
</asp:Content>
