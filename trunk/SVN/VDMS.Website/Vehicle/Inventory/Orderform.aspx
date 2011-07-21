<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="Orderform.aspx.cs" Inherits="Sales_Inventory_Orderform" Title="Entire vehicle Order"
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Save"
        meta:resourcekey="ValidationSummary1Resource1" />
    <div class="form" style="width: 450px">
        <table>
            <tr>
                <td>
                    <asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
                    <asp:Localize ID="litOrderDate" Text="Order Date:" runat="server" meta:resourcekey="litOrderDateResource1"></asp:Localize>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtFromDate" runat="server" Width="88px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="Validator"
                        Text="*" SetFocusOnError="True" ValidationGroup="Save" ControlToValidate="txtFromDate"
                        meta:resourcekey="rfvFromDateResource1"></asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="meeFromDate" CultureAMPMPlaceholder="AM;PM"
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                        CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                        Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtFromDate"
                        PopupButtonID="ibFromDate" BehaviorID="ceFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    ~
                    <asp:TextBox ID="txtToDate" runat="server" Width="88px" meta:resourcekey="txtToDateResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibToDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibToDateResource1" />
                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="Validator" Text="*"
                        SetFocusOnError="True" ValidationGroup="Save" ControlToValidate="txtToDate" meta:resourcekey="rfvToDateResource1"></asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="meeToDate" runat="server" TargetControlID="txtToDate"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="meeToDate" CultureAMPMPlaceholder="AM;PM"
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                        CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                        Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="ibToDate" BehaviorID="ceToDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="image1" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image1Resource1" />
                    <asp:Localize ID="litDeliveredPlace" Text="Delivered Place:" runat="server" meta:resourcekey="litDeliveredPlaceResource1"></asp:Localize>
                </td>
                <td>
                    <vdms:WarehouseList ID="ddlAddress" Type="V" runat="server" Width="300px" ShowSelectAllItem="true">
                    </vdms:WarehouseList>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Image ID="image4" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image2Resource1" />
                    <asp:Localize ID="litOrderNumber" runat="server" Text="Order number:" meta:resourcekey="litOrderNumberResource1"></asp:Localize>
                </td>
                <td valign="top" colspan="2">
                    <asp:TextBox ID="txtOrderNumber" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="image2" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image2Resource1" />
                    <asp:Localize ID="litStatus" Text="Status:" runat="server" meta:resourcekey="litStatusResource1"></asp:Localize>
                </td>
                <td>
                    <asp:DropDownList ID="ddlOrderStatus" runat="server" meta:resourcekey="ddlOrderStatusResource1"
                        Width="300px">
                        <asp:ListItem Text="All" Selected="True" meta:resourcekey="ListItemResource1"></asp:ListItem>
                        <asp:ListItem Text="Not sent by dealer" Value="0" meta:resourcekey="ListItemResource2"></asp:ListItem>
                        <asp:ListItem Text="Sent, but not process by VMEP" Value="1" meta:resourcekey="ListItemResource3"></asp:ListItem>
                        <asp:ListItem Text="Sent, being processed by VMEP" Value="4" meta:resourcekey="ListItemResource4"></asp:ListItem>
                        <asp:ListItem Text="Confirmed by VMEP" Value="2" meta:resourcekey="ListItemResource5"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="Save" SkinID="SubmitButton"
                        CommandName="Query" OnClick="btnSearch_Click" meta:resourcekey="btnSearchResource1" />
                    <asp:Button ID="btnAdd" runat="server" Text="Add new Order" OnClientClick="javascript:location.href='EditOrder.aspx?Dealer=1'; return false;"
                        SkinID="SubmitButton" meta:resourcekey="btnAddResource1" />
                </td>
            </tr>
        </table>
    </div>
    <asp:PlaceHolder ID="phInventoryLock" runat="server" Visible="false">
        <p>
            <asp:Label ID="lblInventoryLock" runat="server" ForeColor="Red" Text="The inventory is locked. You cannot create order."
                meta:resourcekey="lblInventoryLockResource1"></asp:Label>
        </p>
    </asp:PlaceHolder>
    <div class="grid">
        <vdms:PageGridView ID="grdListOrder" runat="server" AutoGenerateColumns="False" AllowPaging="True"
            OnDataBound="grdListOrder_DataBound" DataKeyNames="Id" DataSourceID="ObjectDataSource1"
            meta:resourcekey="grdListOrderResource1">
            <Columns>
                <asp:BoundField DataField="ORDERDATE" HeaderText="Order Date" HtmlEncode="False"
                    DataFormatString="{0:d}" meta:resourcekey="BoundFieldResource1" />
                <asp:BoundField DataField="ORDERTIMES" HeaderText="Times" meta:resourcekey="BoundFieldResource2">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Delivered Place" meta:resourcekey="TemplateFieldResource1">
                    <ItemTemplate>
                        <%# EvalAddress(Eval("Shippingto"), Eval("Secondaryshippingto")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="VMEPCOMMENT" HeaderText="VMEP comments" meta:resourcekey="BoundFieldResource3">
                </asp:BoundField>
                <asp:BoundField DataField="Id" HeaderText="VDMS No" meta:resourcekey="BoundFieldResource5" />
                <asp:BoundField DataField="OrderNumber" HeaderText="Order Number" meta:resourcekey="BoundFieldResource4">
                </asp:BoundField>
                <asp:TemplateField HeaderText="Status" meta:resourcekey="TemplateFieldResource2">
                    <ItemTemplate>
                        <%# EvalOrderStatus(Eval("STATUS")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="EditOrder.aspx?OrderId={0}&amp;Dealer=1"
                    Text='&lt;img src=&quot;../../Images/Edit.gif&quot; border=&quot;0&quot; /&gt;'
                    meta:resourcekey="HyperLinkFieldResource1" />
                <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="../Report/PrintOrder.aspx?oid={0}"
                    meta:resourcekey="PrintLinkResource" Target="_blank" Text="&lt;img src=&quot;../../Images/print.gif&quot; border=&quot;0&quot; /&gt;" />
            </Columns>
            <EmptyDataTemplate>
                <b>
                    <asp:Localize ID="litEmpty" runat="server" Text="No order found! Please change the condition search."
                        meta:resourcekey="litEmptyResource1"></asp:Localize></b>
            </EmptyDataTemplate>
        </vdms:PageGridView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="Select"
            TypeName="VDMS.I.ObjectDataSource.OrderHeaderDataSource" EnablePaging="True"
            SelectCountMethod="SelectCount">
            <SelectParameters>
                <asp:Parameter Name="maximumRows" Type="Int32" />
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:ControlParameter ControlID="txtFromDate" Name="sFromDate" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtToDate" Name="sToDate" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="ddlAddress" Name="BranchCode" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="ddlOrderStatus" Name="StatusCode" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="txtOrderNumber" Name="OrderNumber" PropertyName="Text"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
