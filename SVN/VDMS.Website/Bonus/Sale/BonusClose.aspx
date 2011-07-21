<%@ Page Title="Bonus Monthly closing" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="BonusClose.aspx.cs" Inherits="Bonus_Sale_BonusClose" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <table>
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Dealer code:" 
                        meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox MaxLength="30" ID="txtDealer" runat="server" 
                        meta:resourcekey="txtDealerResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnFind" runat="server" Text="Find" onclick="btnFind_Click" 
                        meta:resourcekey="btnFindResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <div id="_err" runat="server">
        </div>
        <vdms:PageGridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="odsBonus"
            DataKeyNames="DealerCode" OnRowCommand="GridView1_RowCommand" 
            meta:resourcekey="GridView1Resource1">
            <Columns>
                <asp:BoundField DataField="DealerCode" HeaderText="Dealer code" 
                    SortExpression="DealerCode" meta:resourcekey="BoundFieldResource1" />
                <asp:BoundField DataField="DealerName" HeaderText="Dealer name" 
                    SortExpression="DealerName" meta:resourcekey="BoundFieldResource2" />
                <asp:BoundField DataField="Amount" HeaderText="Current amount" SortExpression="Amount"
                    DataFormatString="{0:N0}" meta:resourcekey="BoundFieldResource3">
                    <ItemStyle CssClass="number" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Locked month" SortExpression="LockDate" 
                    meta:resourcekey="TemplateFieldResource1">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("LockDate") %>' 
                            meta:resourcekey="TextBox1Resource1"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:TextBox ReadOnly='<%# Eval("LockDate") != null %>' ID="txtCloseMonth" Width="80px"
                            runat="server" Text='<%# Eval("LockDate", "{0:MM/yyyy}") %>' 
                            meta:resourcekey="txtCloseMonthResource1"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender PopupButtonID="imbC1" ID="txtCloseMonth_CalendarExtender"
                            Enabled='<%# Eval("LockDate") == null %>' runat="server" TargetControlID="txtCloseMonth"
                            Format="MM/yyyy">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="txtCloseMonth_MaskedEditExtender" runat="server"
                            MaskType="Number" Mask="99/9999" TargetControlID="txtCloseMonth" 
                            ClearMaskOnLostFocus="False"  Enabled="True">
                        </ajaxToolkit:MaskedEditExtender>
                        <asp:ImageButton Visible='<%# Eval("LockDate") == null %>' runat="server" SkinID="CalendarImageButton"
                            ID="imbC1" meta:resourcekey="imbC1Resource1" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource2">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Close"
                            CommandArgument='<%# Eval("DealerCode") %>' Text="Close" 
                            meta:resourcekey="LinkButton1Resource1"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource3">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton2" Visible='<%# Eval("LockDate") != null %>' runat="server"
                            CausesValidation="False" CommandName="Open" CommandArgument='<%# Eval("DealerCode") %>'
                            Text="Open" meta:resourcekey="LinkButton2Resource1"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </vdms:PageGridView>
        <asp:ObjectDataSource ID="odsBonus" runat="server" EnablePaging="True"
            SelectMethod="GetCloseData" TypeName="BonusCloser" 
            SelectCountMethod="CountCloseData">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtDealer" Name="dCode" PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    <asp:Literal ID="errInvalidLockMonth" runat="server" Visible="False" 
        Text="Invalid lock month!" meta:resourcekey="errInvalidLockMonthResource1"></asp:Literal>
    <asp:Literal ID="errBonusNeverClosed" runat="server" Visible="False" 
        Text="This Dealer need closed before open!" 
        meta:resourcekey="errBonusNeverClosedResource1"></asp:Literal>
</asp:Content>
