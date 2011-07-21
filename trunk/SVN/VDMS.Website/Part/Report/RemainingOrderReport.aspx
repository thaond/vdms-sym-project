<%@ Page Title="Remaining Order Report" Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="RemainingOrderReport.aspx.cs" Inherits="Part_Report_RemainingOrderReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
   </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <table>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" Text="Order number:" runat="server"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtOrderNo" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="Literal3" Text="Order date:" runat="server"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtOrderFrom" runat="server" meta:resourcekey="txtOrderFromResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ValidationGroup="Report" ID="RequiredFieldValidator1"
                        runat="server" SetFocusOnError="True" ControlToValidate="txtOrderFrom" ErrorMessage="Order date cannot be blank!"
                        Enabled="false" meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                    <asp:ImageButton ID="ibOrderFrom" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="meeOrderFrom" runat="server" TargetControlID="txtOrderFrom"
                        Mask="99/99/9999" MaskType="Date" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceOrderFrom" runat="server" TargetControlID="txtOrderFrom"
                        PopupButtonID="ibOrderFrom" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    ~
                    <asp:TextBox ID="txtOrderTo" runat="server" meta:resourcekey="txtOrderToResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibtxtOrderTo" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtOrderTo"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="meetxtOrderTo" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtOrderTo"
                        PopupButtonID="ibtxtOrderTo" BehaviorID="cetxtOrderTo" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal6" Text="Issue number:" runat="server"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtIssueNo" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="Literal4" Text="Issue date:" runat="server"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtIssueFrom" runat="server" meta:resourcekey="txtIssueFromResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ValidationGroup="Report" ID="RequiredFieldValidator2"
                        runat="server" SetFocusOnError="True" ControlToValidate="txtIssueFrom" Enabled="false"
                        ErrorMessage="Issue date cannot be blank!" meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                    <asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtIssueFrom"
                        Mask="99/99/9999" MaskType="Date" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtIssueFrom"
                        PopupButtonID="ibFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    ~
                    <asp:TextBox ID="txtIssueTo" runat="server" meta:resourcekey="txtIssueToResource1"></asp:TextBox>
                    <asp:ImageButton ID="ibToDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
                        meta:resourcekey="ibFromDateResource1" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtIssueTo"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="meeFromDate" Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtIssueTo"
                        PopupButtonID="ibToDate" BehaviorID="ceFromDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal1" Text="Dealer code:" runat="server"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtDealerCode" runat="server"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="2">
                    <asp:Button ID="btnDoReport" runat="server" Text="Find" 
                        onclick="btnDoReport_Click" />
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div class="form" id="pnOrderInfo" runat="server">
        <table style="text-align:left">
            <tr>
                <th>
                    <asp:Literal ID="Literal5" runat="server" Text="Address:"></asp:Literal></th>
                <td colspan="4"><asp:Literal ID="litAddress" runat="server" Text="" /></td>
            </tr>
            <tr>
                <th>
                    <asp:Literal ID="Literal7" runat="server" Text="Order No:"></asp:Literal></th>
                <td><asp:Literal ID="litOrder" runat="server" Text="" /></td>
                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                <th>
                    <asp:Literal ID="Literal8" runat="server" Text="Date:"></asp:Literal></th>
                <td><asp:Literal ID="litODate" runat="server" Text="" /></td>
            </tr>
            <tr>
                <th>
                    <asp:Literal ID="Literal9" runat="server" Text="Delivery:"></asp:Literal></th>
                <td colspan="4"><asp:Literal ID="litDelivery" runat="server" Text="" /></td>
            </tr>
            
        </table>
    </div>
    <div class="grid">
        <vdms:PageGridView ID="gv" runat="server" AutoGenerateColumns="False" 
            ShowFooter="True" ondatabound="gv_DataBound">
            <Columns>
                <asp:BoundField DataField="LineNumber" HeaderText="Line" 
                    SortExpression="LineNumber" >
                <ItemStyle CssClass="center" />
                </asp:BoundField>
                <asp:BoundField DataField="PartCode" HeaderText="Part Code" 
                    SortExpression="PartCode" />
                <asp:BoundField DataField="PartName" HeaderText="Part Name" ReadOnly="True" 
                    SortExpression="PartName" />
                <asp:BoundField DataField="PackUnit" HeaderText="Unit" ReadOnly="True" 
                    SortExpression="PackUnit" />
                <asp:BoundField DataField="OrderQuantity" HeaderText="Order" 
                    SortExpression="OrderQuantity" DataFormatString="{0:N0}" >
                <ItemStyle CssClass="number" />
                </asp:BoundField>
                <asp:BoundField DataField="QuotationQuantity" HeaderText="Quotation" 
                    SortExpression="QuotationQuantity" DataFormatString="{0:N0}" >
                <ItemStyle CssClass="number" />
                </asp:BoundField>
                <asp:BoundField DataField="DeliveryQuantity" HeaderText="Delivery" 
                    ReadOnly="True" SortExpression="DeliveryQuantity" 
                    DataFormatString="{0:N0}" >
                <ItemStyle CssClass="number" />
                </asp:BoundField>
                <asp:BoundField DataField="RemainQuantity" HeaderText="Remain" 
                    ReadOnly="True" SortExpression="RemainQuantity" DataFormatString="{0:N0}" >
                <ItemStyle CssClass="number" />
                </asp:BoundField>
                <asp:BoundField DataField="UnitPrice" HeaderText="Price" 
                    SortExpression="UnitPrice" DataFormatString="{0:N0}" >
                <ItemStyle CssClass="number" />
                </asp:BoundField>
                <asp:BoundField DataField="RemainAmount" HeaderText="Remain Amount" 
                    ReadOnly="True" SortExpression="RemainAmount" DataFormatString="{0:N0}" >
                <ItemStyle CssClass="number" />
                </asp:BoundField>
                <asp:BoundField DataField="Note" HeaderText="Note" 
                    ReadOnly="True" SortExpression="Note" />
            </Columns>
            <FooterStyle CssClass="sumLine" />
        </vdms:PageGridView>
        <asp:ObjectDataSource ID="odsOrderRemain" runat="server" 
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetRemainingOrder" 
            TypeName="VDMS.II.PartManagement.Order.RemainingOrderReport">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtDealerCode" Name="dCode" 
                    PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="txtIssueNo" Name="issueNo" PropertyName="Text" 
                    Type="String" />
                <asp:ControlParameter ControlID="txtOrderNo" Name="orderNo" PropertyName="Text" 
                    Type="String" />
                <asp:ControlParameter ControlID="txtOrderFrom" Name="oFrom" PropertyName="Text" 
                    Type="DateTime" />
                <asp:ControlParameter ControlID="txtOrderTo" Name="oTo" PropertyName="Text" 
                    Type="DateTime" />
                <asp:ControlParameter ControlID="txtIssueFrom" Name="iFrom" PropertyName="Text" 
                    Type="DateTime" />
                <asp:ControlParameter ControlID="txtIssueTo" Name="iTo" PropertyName="Text" 
                    Type="DateTime" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
