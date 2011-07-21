<%@ Page Language="C#" MasterPageFile="~/MP/Mobile.master" AutoEventWireup="true" Theme="Mobile"
    CodeFile="DailySaleReport.aspx.cs" Inherits="MVehicle_Report_DailySaleReport"
    Title="Daily Sales Report" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="c" runat="Server">
    <asp:BulletedList ID="bllError" runat="server" CssClass="errorMsg">
    </asp:BulletedList>
    <div class="form">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
                    <asp:Localize ID="litReportDate" Text="Report Date:" runat="server" meta:resourcekey="litReportDateResource1"></asp:Localize>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtFromDate" runat="server" Width="80%" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" Text="*" SetFocusOnError="True"
                        ValidationGroup="Save" ControlToValidate="txtFromDate" meta:resourcekey="rfvFromDateResource1"></asp:RequiredFieldValidator>
                    <ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                        Mask="99/99/9999" MaskType="Date" BehaviorID="meeFromDate" CultureAMPMPlaceholder="AM;PM"
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="MDY" CultureDatePlaceholder="/"
                        CultureDecimalPlaceholder="." CultureThousandsPlaceholder="," CultureTimePlaceholder=":"
                        Enabled="True">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="txtFromDate"
                            Enabled="true" InvalidValueMessage="*" ControlExtender="meeFromDate"></ajaxToolkit:MaskedEditValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="image1" runat="server" SkinID="RequireFieldNon" />
                    <asp:Literal ID="litItem" runat="server" Text="Item:" meta:resourcekey="litItemResource1"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlItem" runat="server" DataValueField="ItemType" DataTextField="ItemType"
                        DataSourceID="ItemDataSource1" OnDataBound="ddlItem_DataBound" AppendDataBoundItems="True"
                        meta:resourcekey="ddlItemResource1">
                        <asp:ListItem Text="-+-" meta:resourcekey="ListItemResource1"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ObjectDataSource ID="ItemDataSource1" runat="server" SelectMethod="GetListItemType"
                        TypeName="VDMS.I.ObjectDataSource.ItemDataSource"></asp:ObjectDataSource>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="image3" runat="server" SkinID="RequireFieldNon" />
                    <asp:Literal ID="Literal1" runat="server" Text="Area:" meta:resourcekey="litDealerResource1x"></asp:Literal>
                </td>
                <td>
                    <vdms:AreaList ID="ddlArea" ShowSelectAllItem="True" runat="server" Width="80%" >
                    </vdms:AreaList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="image2" runat="server" SkinID="RequireFieldNon" />
                    <asp:Literal ID="litDealer" runat="server" Text="Dealer:" meta:resourcekey="litDealerResource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtDealer" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="cmdCreate" runat="server" Text="Create" OnClick="cmdCreate_Click" Width="40%"
                        meta:resourcekey="cmdCreateResource1" />
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <vdms:PageGridView ID="gvReport" runat="server" AutoGenerateColumns="False" CssClass="datatable"
            meta:resourcekey="gvReportResource1" OnDataBound="gvReport_DataBound" OnRowDataBound="gvReport_RowDataBound"
            FooterStyle-CssClass="sumLine">
            <Columns>
                <asp:BoundField HeaderText="No." meta:resourcekey="BoundFieldResource6" />
                <asp:BoundField DataField="DealerCode" HeaderText="Dealer Code" meta:resourcekey="BoundFieldResource1">
                    <ItemStyle Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="DealerName" HeaderText="Dealer Name" meta:resourcekey="BoundFieldResource8">
                    <ItemStyle Width="200px" />
                </asp:BoundField>
                <asp:BoundField DataField="TonDau" DataFormatString="{0:N0}" HeaderText="Begin In stock"
                    meta:resourcekey="BoundFieldResource2" />
                <%--<asp:BoundField DataField="Dat" DataFormatString="{0:N0}" HeaderText="Order" meta:resourcekey="BoundFieldResource3" />--%>
                <asp:BoundField DataField="Nhap" DataFormatString="{0:N0}" HeaderText="Import" meta:resourcekey="BoundFieldResource4" />
                <asp:BoundField DataField="Xuat" DataFormatString="{0:N0}" HeaderText="Export" meta:resourcekey="BoundFieldResource5" />
                <asp:BoundField DataField="TonCuoi" HeaderText="End In stock" meta:resourcekey="BoundFieldResource7" />
                <asp:BoundField HeaderText="Comment" meta:resourcekey="ButtonFieldResource1">
                    <ItemStyle Width="250px" />
                </asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <b>
                    <asp:Localize ID="litEmpty" runat="server" Text="No data report." meta:resourcekey="litEmptyResource1"></asp:Localize></b>
            </EmptyDataTemplate>
        </vdms:PageGridView>
    </div>
    <%--<asp:Button ID="cmdExcel" runat="server" Text="Export to Excel" Enabled="False" OnClick="cmdExcel_Click"
        meta:resourcekey="cmdExcelResource1" />--%>
</asp:Content>
