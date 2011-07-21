<%@ Page Title="" Language="C#" MasterPageFile="~/MP/Popup.master" AutoEventWireup="true"
    CodeFile="BonusMoney.aspx.cs" Inherits="Bonus_Dealer_BonusMoney" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <div class="help">
            <asp:Literal ID="Literal1" runat="server" Text="Get bonus money to use in this order"
                meta:resourcekey="Literal1Resource1"></asp:Literal>
        </div>
        <div class="grid">
            <vdms:PageGridView ID="gv" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                CssClass="GridView" ShowFooter="True" DataSourceID="odsBonusMoney" EmptyDataText="No bonus found."
                OnDataBound="gv_DataBound" meta:resourcekey="gvResource1" PageSize="15">
                <Columns>
                    <asp:BoundField DataField="BonusPlanDetailId" HeaderText="Id" meta:resourcekey="BoundFieldResource1" />
                    <asp:BoundField DataField="BonusSourceName" HeaderText="BonusSource" SortExpression="BonusSourceId"
                        meta:resourcekey="BoundFieldResource2" />
                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"
                        meta:resourcekey="BoundFieldResource3" />
                    <asp:BoundField DataField="BonusDate" HeaderText="BonusDate" SortExpression="BonusDate"
                        meta:resourcekey="BoundFieldResource4" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" ReadOnly="True" SortExpression="Amount"
                        meta:resourcekey="BoundFieldResource5" />
                    <asp:TemplateField HeaderText="Using Amount" meta:resourcekey="TemplateFieldResource1">
                        <ItemTemplate>
                            <asp:TextBox ID="txtUsingAmount" runat="server" meta:resourcekey="txtUsingAmountResource1"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender TargetControlID="txtUsingAmount" FilterType="Numbers"
                                ID="FilteredTextBoxExtender1" runat="server" Enabled="True">
                            </ajaxToolkit:FilteredTextBoxExtender>
                            <asp:RangeValidator ID="rvAmount" runat="server" ErrorMessage="*" ControlToValidate="txtUsingAmount"
                                Type="Double"></asp:RangeValidator>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle CssClass="group" />
            </vdms:PageGridView>
            <asp:ObjectDataSource ID="odsBonusMoney" runat="server" OldValuesParameterFormatString="original_{0}"
                SelectMethod="GetBonusMoney" TypeName="VDMS.I.Vehicle.OrderBonusDAO" EnablePaging="True"
                SelectCountMethod="CountBonusMoney">
                <SelectParameters>
                    <asp:Parameter Name="dealerCode" Type="String" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" meta:resourcekey="btnSaveResource1" />
    </div>
</asp:Content>
