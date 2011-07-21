<%@ Page Title="Bonus plan" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="BonusPlan.aspx.cs" Inherits="Bonus_Sale_BonusPlan"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <ajaxToolkit:TabContainer ID="tabContainer" runat="server" CssClass="ajax__tab_technorati-theme"
        ActiveTabIndex="0" meta:resourcekey="tResource1">
        <ajaxToolkit:TabPanel ID="t1" runat="server">
            <HeaderTemplate>
                <asp:Literal ID="Literal17" Text="Key-in bonus data" runat="server"></asp:Literal></HeaderTemplate>
            <ContentTemplate>
                <div runat="server" id="error">
                </div>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" meta:resourcekey="ValidationSummary1Resource1" />
                <div class="form">
                    <table>
                        <tr>
                            <td>
                                <asp:Image ID="Image1" runat="server" SkinID="RequireField" meta:resourcekey="Image1Resource1" />
                                <asp:Literal ID="Literal1" runat="server" Text="Plan name:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPlanName" runat="server" meta:resourcekey="txtPlanNameResource1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Plan name cannot be blank!"
                                    Text="*" ValidationGroup="Save" ControlToValidate="txtPlanName" meta:resourcekey="RequiredFieldValidator3Resource1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="Image2" runat="server" SkinID="RequireField" meta:resourcekey="Image2Resource1" />
                                <asp:Literal ID="Literal2" runat="server" Text="Plan month:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPlanMonth" runat="server" meta:resourcekey="txtPlanMonthResource1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtPlanMonth"
                                    ErrorMessage="Plan month cannot be blank!" Text="*" ValidationGroup="Save" meta:resourcekey="RequiredFieldValidator4Resource1"></asp:RequiredFieldValidator>
                                <ajaxToolkit:CalendarExtender PopupButtonID="imbC1" ID="txtPlanMonth_CalendarExtender"
                                    runat="server" TargetControlID="txtPlanMonth" Format="MM/yyyy">
                                </ajaxToolkit:CalendarExtender>
                                <ajaxToolkit:MaskedEditExtender ID="txtPlanMonth_MaskedEditExtender" runat="server"
                                    MaskType="Number" Mask="99/9999" TargetControlID="txtPlanMonth" ClearMaskOnLostFocus="False"
                                    Enabled="True">
                                </ajaxToolkit:MaskedEditExtender>
                                <asp:ImageButton runat="server" SkinID="CalendarImageButton" ID="imbC1" meta:resourcekey="imbC1Resource1" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="Image3" runat="server" SkinID="RequireFieldNon" meta:resourcekey="Image3Resource1" />
                                <asp:Literal ID="Literal3" runat="server" Text="Comment:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDesc" Width="500px" runat="server" MaxLength="200" meta:resourcekey="txtDescResource1"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="grid">
                    <asp:PlaceHolder ID="phEdit" runat="server">
                        <asp:LinkButton ID="lnkAddRows" runat="server" OnClick="lnkAddRows_Click" ValidationGroup="AddItems"
                            meta:resourcekey="lnkAddRowsResource1">Add</asp:LinkButton>
                        <asp:TextBox ID="txtItems" runat="server" Width="40px" meta:resourcekey="txtItemsResource1">5</asp:TextBox>
                        <ajaxToolkit:FilteredTextBoxExtender ID="txtItems_FilteredTextBoxExtender" runat="server"
                            Enabled="True" FilterType="Numbers" TargetControlID="txtItems">
                        </ajaxToolkit:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="You must enter number of row(s) to insert!"
                            ValidationGroup="AddItems" ControlToValidate="txtItems" meta:resourcekey="RequiredFieldValidator5Resource1">*</asp:RequiredFieldValidator>
                        <asp:Literal ID="Literal4" runat="server" Text="row(s)" meta:resourcekey="Literal4Resource1"></asp:Literal>
                        <vdms:EmptyGridViewEx ID="gvItems" runat="server" AllowInsertEmptyRow="False" AutoGenerateColumns="False"
                            DataSourceID="odsItems" GennerateSpanDataTable="False" IncludeChildsListInLevel="False"
                            RealPageSize="20" ShowEmptyFooter="True" ShowEmptyTable="True" DataKeyNames="Id"
                            ShowFooter="True" OnRowCommand="gvItems_RowCommand" OnRowDataBound="gvItems_RowDataBound"
                            meta:resourcekey="gvItemsResource1">
                            <FooterStyle CssClass="footer" />
                            <Columns>                          
                                <asp:TemplateField HeaderText="DealerCode" SortExpression="DealerCode" meta:resourcekey="TemplateFieldResource1">
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNewDealerCode" runat="server" MaxLength="30" Text='<%# Bind("DealerCode") %>'
                                            meta:resourcekey="txtNewDealerCodeResource1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNewDealerCode"
                                            ErrorMessage="*" ValidationGroup="AddItem" meta:resourcekey="RequiredFieldValidator2Resource1"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDealerCode" runat="server" Text='<%# Bind("DealerCode") %>' MaxLength="30"
                                            OnTextChanged="ItemChanged" meta:resourcekey="txtDealerCodeResource1"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BonusDate" SortExpression="BonusDate" meta:resourcekey="TemplateFieldResource2">
                                    <FooterTemplate>
                                        <span class="calendar">
                                            <asp:TextBox ID="txtNewBonusDate" runat="server" Text='<%# Bind("BonusDate") %>'
                                                meta:resourcekey="txtNewBonusDateResource1"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="txtNewBonusDate_CalendarExtender" runat="server"
                                                Format="dd/MM/yyyy" Enabled="True" PopupButtonID="ImageButton1" TargetControlID="txtNewBonusDate">
                                            </ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="txtNewBonusDate_MaskedEditExtender" Mask="99/99/9999"
                                                runat="server" Enabled="True" MaskType="Date" TargetControlID="txtNewBonusDate"
                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <asp:ImageButton ID="ImageButton1" runat="server" OnClientClick="return false;" SkinID="CalendarImageButton"
                                                meta:resourcekey="ImageButton1Resource2" />
                                        </span>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <span class="calendar">
                                            <asp:TextBox ID="txtBonusDate" OnTextChanged="ItemChanged" runat="server" Text='<%# Bind("BonusDate") %>'
                                                meta:resourcekey="txtBonusDateResource1"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="txtBonusDate_CalendarExtender" runat="server" TargetControlID="txtBonusDate"
                                                PopupButtonID="ImageButton1" Enabled="True">
                                            </ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="txtBonusDate_MaskedEditExtender" Mask="99/99/9999"
                                                runat="server" Enabled="True" TargetControlID="txtBonusDate" MaskType="Date"
                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <asp:ImageButton ID="ImageButton1" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
                                                meta:resourcekey="ImageButton1Resource1" />
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" SortExpression="Amount" meta:resourcekey="TemplateFieldResource3">
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNewAmount" runat="server" MaxLength="18" Text='<%# Bind("Amount") %>'
                                            meta:resourcekey="txtNewAmountResource1"></asp:TextBox>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="txtNewAmount_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="txtNewAmount">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNewAmount"
                                            ErrorMessage="*" ValidationGroup="AddItem" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAmount" OnTextChanged="ItemChanged" runat="server" Text='<%# Bind("Amount") %>'
                                            MaxLength="18" meta:resourcekey="txtAmountResource1"></asp:TextBox>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="txtAmount_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="txtAmount">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" SortExpression="Status" meta:resourcekey="TemplateFieldResource4">
                                    <FooterTemplate>
                                        <vdms:BonusStatusList ID="dlNewStatus" runat="server" meta:resourcekey="dlNewStatusResource1"
                                            ShowEmptyItem="False">
                                        </vdms:BonusStatusList>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <vdms:BonusStatusList ID="dlStatus" runat="server" BindingSelectedValue='<%# Bind("Status") %>'
                                            meta:resourcekey="dlStatusResource1" ShowEmptyItem="False">
                                        </vdms:BonusStatusList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="PlanType" SortExpression="PlanType">
                        <FooterTemplate>
                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("PlanType") %>'></asp:TextBox>
                        </FooterTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBox5" ontextchanged="ItemChanged" runat="server" Text='<%# Bind("PlanType") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Bonus source" SortExpression="BonusSourceId" meta:resourcekey="TemplateFieldResource5">
                                    <FooterTemplate>
                                        <vdms:BonusSourceList ShowEmptyItem="True" ID="dlNewBSource" runat="server" meta:resourcekey="dlNewBSourceResource1">
                                        </vdms:BonusSourceList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="dlNewBSource"
                                            ErrorMessage="*" ValidationGroup="AddItem" meta:resourcekey="RequiredFieldValidator2Resource1x"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <vdms:BonusSourceList ShowEmptyItem="false" ID="dlBSource" SelectedIndexChanged="ItemChanged"
                                            runat="server" meta:resourcekey="dlBSourceResource1">
                                        </vdms:BonusSourceList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" SortExpression="Description" meta:resourcekey="TemplateFieldResource6">
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNewDesc" runat="server" MaxLength="512" Text='<%# Bind("Description") %>'
                                            meta:resourcekey="txtNewDescResource1"></asp:TextBox>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDesc" OnTextChanged="ItemChanged" runat="server" Text='<%# Bind("Description") %>'
                                            MaxLength="512" meta:resourcekey="txtDescResource2"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource7">
                                    <FooterTemplate>
                                        <asp:ImageButton ID="ImageButton3" runat="server" CommandName="Insert" ImageUrl="~/Images/update.gif"
                                            OnClick="ImageButton3_Click" ValidationGroup="AddItem" meta:resourcekey="ImageButton3Resource1" />
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton2" runat="server" CommandName="Delete" ImageUrl="~/Images/Delete.gif"
                                            meta:resourcekey="ImageButton2Resource1" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </vdms:EmptyGridViewEx>
                    </asp:PlaceHolder>
                    <vdms:PageGridView PageSize="20" ID="gvViewItems" runat="server" AutoGenerateColumns="False"
                        DataSourceID="odsItems" OnRowDataBound="gvViewItems_RowDataBound" meta:resourcekey="gvViewItemsResource1">
                        <Columns>
                            <asp:BoundField DataField="DealerCode" HeaderText="Dealer Code" SortExpression="DealerCode"
                                meta:resourcekey="BoundFieldResource1" />
                            <asp:BoundField DataField="BonusDate" DataFormatString="{0:d}" HeaderText="Bonus Date"
                                SortExpression="BonusDate" meta:resourcekey="BoundFieldResource2" />
                            <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" DataFormatString="{0:C0}"
                                meta:resourcekey="BoundFieldResource3">
                                <ItemStyle CssClass="right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="Status"
                                meta:resourcekey="BoundFieldResource4" />
                            <%--<asp:BoundField DataField="PlanType" HeaderText="Type" SortExpression="Type" />--%>
                            <asp:TemplateField HeaderText="Bonus source" SortExpression="BonusSourceId" meta:resourcekey="TemplateFieldResource8">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("BonusSourceId") %>' meta:resourcekey="TextBox1Resource1"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# EvalBSourceName((long)Eval("BonusSourceId")) %>'
                                        meta:resourcekey="Label1Resource1"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"
                                meta:resourcekey="BoundFieldResource5" />
                        </Columns>
                    </vdms:PageGridView>
                    <asp:Button ID="btnConfirm" runat="server" Text="Confirm" OnClick="btnConfirm_Click"
                        meta:resourcekey="btnConfirmResource1" />
                    <asp:Button ID="btnEnd" runat="server" Text="Finish" ValidationGroup="Save" OnClick="btnEnd_Click"
                        meta:resourcekey="btnEndResource1" />
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" OnClick="btnEdit_Click" meta:resourcekey="btnEditResource1" />
                    <asp:Button ID="btnSave" runat="server" Text="Save plan" ValidationGroup="Save" OnClick="btnSave_Click"
                        meta:resourcekey="btnSaveResource1" />
                    <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" meta:resourcekey="btnBackResource1" />
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="t2" runat="server">
            <HeaderTemplate>
                <asp:Literal ID="Literal5" Text="Import from excel file" runat="server" meta:resourcekey="Literal5Resource1"></asp:Literal></HeaderTemplate>
            <ContentTemplate>
                <asp:FileUpload runat="server" ID="fileUpload" />
                <asp:Button runat="server" ID="btnBUpload" Text="Upload" OnClick="btnBUpload_Click"
                    meta:resourcekey="btnBUpload" />
                <div class="grid">
                    <vdms:PageGridView runat="server" AllowPaging="true" ID="gvImport" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="BonusHeaderPlanName" HeaderText="Plan Name" meta:resourcekey="BoundField1" />
                            <asp:BoundField DataField="DealerCode" HeaderText="Dealer" meta:resourcekey="BoundField2" />
                            <asp:BoundField DataField="BonusDate" HeaderText="Bonus Date" meta:resourcekey="BoundField3" />
                            <asp:BoundField DataField="BonusSourceName" HeaderText="Source" meta:resourcekey="BoundField4" />
                            <asp:BoundField DataField="Amount" HeaderText="Amount" meta:resourcekey="BoundField5" />
                            <asp:BoundField DataField="Description" HeaderText="Description" meta:resourcekey="BoundField6" />
                        </Columns>
                    </vdms:PageGridView>
                </div>
                <asp:Button ID="btnBImport" runat="server" Text="Import" OnClick="btnBImport_Click"
                    meta:resourcekey="btnBImport" />
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
    <asp:ObjectDataSource ID="odsItems" runat="server" DataObjectTypeName="VDMS.Bonus.Entity.BonusPlanDetail"
        DeleteMethod="DeleteEditingItem" OldValuesParameterFormatString="original_{0}"
        SelectMethod="GetEditingItems" TypeName="VDMS.II.BonusSystem.BonusPlans" UpdateMethod="UpdateEditingItem"
        InsertMethod="AddEditingItem">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="0" Name="PlanId" QueryStringField="id" Type="Int32" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="BonusDate" Type="DateTime" />
            <asp:Parameter Name="DealerCode" Type="String" />
            <asp:Parameter Name="Status" Type="String" />
            <asp:Parameter Name="PlanType" Type="String" />
            <asp:Parameter Name="Amount" Type="Int64" />
            <asp:Parameter Name="Description" Type="String" />
        </InsertParameters>
    </asp:ObjectDataSource>
</asp:Content>
