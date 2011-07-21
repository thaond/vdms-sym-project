<%@ Page Title="Bonus plan list" Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true" CodeFile="BonusPlanList.aspx.cs" Inherits="Bonus_Sale_BonusPlanList" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" Runat="Server">
<div class="form">
        <table>
            <tr>
                <td>
                    <asp:Image ID="Image1" runat="server" SkinID="RequireFieldNon" 
                        meta:resourcekey="Image1Resource1" />
                    <asp:Literal ID="Literal1" runat="server" Text="Plan name:" 
                        meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox Width="100%" ID="txtPlanName" runat="server" 
                        meta:resourcekey="txtPlanNameResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="Image3" runat="server" SkinID="RequireFieldNon" 
                        meta:resourcekey="Image3Resource1" />
                    <asp:Literal ID="Literal3" runat="server" Text="Dealer:" 
                        meta:resourcekey="Literal3Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="dlDealer" runat="server" meta:resourcekey="txtFromResource1"></asp:TextBox>
                    <%--<vdms:DealerList Width="100%" runat ="server" ID="dlDealer" 
                        ShowEmptyItem="True" EnabledSaperateByArea="False" EnabledSaperateByDB="False" 
                        MergeCode="False" meta:resourcekey="dlDealerResource1" RemoveRootItem="False" 
                        ShowSelectAllItem="False"></vdms:DealerList>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="Image2" runat="server" SkinID="RequireFieldNon" 
                        meta:resourcekey="Image2Resource1" />
                    <asp:Literal ID="Literal2" runat="server" Text="Plan date:" 
                        meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtFrom" runat="server" meta:resourcekey="txtFromResource1"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender PopupButtonID="imbC1" ID="txtFrom_CalendarExtender"
                        runat="server" Enabled="True" TargetControlID="txtFrom">
                    </ajaxToolkit:CalendarExtender>
                    <ajaxToolkit:MaskedEditExtender ID="txtFrom_MaskedEditExtender" runat="server" MaskType="Date"
                        Mask="99/99/9999" TargetControlID="txtFrom" CultureAMPMPlaceholder="" 
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <asp:ImageButton runat="server" SkinID="CalendarImageButton" ID="imbC1" 
                        meta:resourcekey="imbC1Resource1" />
                    ~
                    <asp:TextBox ID="txtTo" runat="server" meta:resourcekey="txtToResource1"></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="txtTo_MaskedEditExtender" runat="server" MaskType="Date"
                        Mask="99/99/9999" TargetControlID="txtTo" CultureAMPMPlaceholder="" 
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender PopupButtonID="imbC2" ID="txtTo_CalendarExtender" runat="server"
                        Enabled="True" TargetControlID="txtTo">
                    </ajaxToolkit:CalendarExtender>
                    <asp:ImageButton runat="server" SkinID="CalendarImageButton" ID="imbC2" 
                        meta:resourcekey="imbC2Resource1" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnFind" runat="server" Text="Find" onclick="btnFind_Click" 
                        meta:resourcekey="btnFindResource1" />
                </td>
            </tr>
        </table>
    </div>
<div class="grid">
    <vdms:PageGridView ID="gvPlans" runat="server" AutoGenerateColumns="False" 
        DataSourceID="odsPlans" meta:resourcekey="gvPlansResource1">
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="BonusPlanHeaderId" 
                DataTextField="BonusPlanName" 
                DataNavigateUrlFormatString="~/Bonus/Sale/BonusPlan.aspx?id={0}" 
                HeaderText="Plan Name" meta:resourcekey="HyperLinkFieldResource1" />
            <asp:BoundField DataField="CreatedDate" HeaderText="Created date" 
                SortExpression="CreatedDate" meta:resourcekey="BoundFieldResource1" />
            <asp:BoundField DataField="FromDate" DataFormatString="{0:MM/yyyy}" 
                HeaderText="Plan on" SortExpression="FromDate" 
                meta:resourcekey="BoundFieldResource2" />
            <asp:BoundField DataField="StatusName" HeaderText="Status" 
                SortExpression="Status" meta:resourcekey="BoundFieldResource3" />
            <asp:BoundField DataField="Description" HeaderText="Comment" 
                SortExpression="Comment" meta:resourcekey="BoundFieldResource4" />
            <asp:BoundField DataField="UserName" HeaderText="Username" 
                SortExpression="UserName" meta:resourcekey="BoundFieldResource5" />
            <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource1">
                <ItemTemplate>
                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" 
                        CommandArgument='<%# Eval("Status") %>' CommandName="Delete" 
                        ImageUrl="~/Images/Delete.gif" ondatabinding="ImageButton1_DataBinding" 
                        Text="Delete" meta:resourcekey="ImageButton1Resource1" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    
    </vdms:PageGridView>
    <asp:ObjectDataSource ID="odsPlans" runat="server" DeleteMethod="DeletePlan" 
        SelectMethod="GetPlans" TypeName="VDMS.II.BonusSystem.BonusPlans" 
        OldValuesParameterFormatString="original_{0}">
        <DeleteParameters>
            <asp:Parameter Name="BonusPlanHeaderId" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="txtPlanName" Name="name" PropertyName="Text" 
                Type="String" />
            <asp:ControlParameter ControlID="txtFrom" Name="from" PropertyName="Text" 
                Type="DateTime" />
            <asp:ControlParameter ControlID="txtTo" Name="to" PropertyName="Text" 
                Type="DateTime" />
            <asp:ControlParameter ControlID="dlDealer" Name="dealer" 
                PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    </div>
    <asp:HyperLink NavigateUrl="~/Bonus/Sale/BonusPlan.aspx" ID="lnkNew" 
        runat="server" Text = "Add new plan" meta:resourcekey="lnkNewResource1"></asp:HyperLink>
</asp:Content>

