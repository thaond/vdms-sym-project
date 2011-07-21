<%@ Page Title="" Language="C#" MasterPageFile="~/MP/Popup.master" AutoEventWireup="true"
    CodeFile="CRPayment.aspx.cs" Inherits="Bonus_Dealer_CRPayment" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form" style="width: 700px;">
        <div class="help">
            <asp:Literal ID="Literal1" runat="server" Text="Choose consign-remaining payment for order"
                meta:resourcekey="Literal1Resource1"></asp:Literal>
        </div>
        <div class="grid">
            <asp:UpdatePanel runat="server" ID="pn" UpdateMode="Conditional">
                <ContentTemplate>
                    <vdms:PageGridView ID="gv" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CssClass="GridView" DataSourceID="odsCRPayments" OnDataBound="gv_DataBound" ShowFooter="True"
                        EmptyDataText="No payment found." meta:resourcekey="gvResource1" PageSize="15" >
                        <Columns>
                            <asp:BoundField DataField="OrderPaymentId" HeaderText="OrderPaymentId" SortExpression="OrderPaymentId"
                                meta:resourcekey="BoundFieldResource1" />
                            <asp:BoundField DataField="PaymentDate" HeaderText="PaymentDate" SortExpression="PaymentDate"
                                meta:resourcekey="BoundFieldResource2" />
                            <asp:BoundField DataField="Description" HeaderText="PaymentDate" SortExpression="PaymentDate"
                                meta:resourcekey="BoundFieldResource3" />
                            <asp:BoundField DataField="Amount" HeaderText="Total Amount" SortExpression="Amount"
                                meta:resourcekey="BoundFieldResource4" />
                            <asp:TemplateField HeaderText="Using Amount" meta:resourcekey="TemplateFieldResource1">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtUsingAmount" runat="server" meta:resourcekey="txtUsingAmountResource1"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender TargetControlID="txtUsingAmount" FilterType="Numbers"
                                        ID="FilteredTextBoxExtender1" runat="server" Enabled="True">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                    <asp:RangeValidator ID="rvAmount" runat="server" ErrorMessage="*" ControlToValidate="txtUsingAmount" Type="Double"></asp:RangeValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="group" />
                    </vdms:PageGridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:ObjectDataSource ID="odsCRPayments" runat="server" OldValuesParameterFormatString="original_{0}"
                SelectMethod="GetCRPayment" TypeName="VDMS.I.Vehicle.OrderBonusDAO" EnablePaging="True"
                SelectCountMethod="CountCRPayment">
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
