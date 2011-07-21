<%@ Page Title="Monthly summarization" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="MonthlyClose.aspx.cs" Inherits="Part_Inventory_MonthlyClose"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
	<script type="text/javascript">
		function updated() {
			//  close the popup
			tb_remove();
		}

		function showSearch(link, wCode, dCode) {
			var s = "../Report/MonthlyReportDownload.aspx?";
			s = s + "wCode=" + wCode;
			s = s + "&dCode=" + dCode;
			s = s + "&TB_iframe=true&height=320&width=420";
			link.href = s;
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="help" style="float: right; width: 35%;">
		<ul>
			<li>
				<asp:Literal ID="Literal1" runat="server" Text="Close sequence is from bottom to up, that is from sub-store to comp-dealer, and
                from comp-dealer to main dealer." meta:resourcekey="Literal1Resource1"></asp:Literal></li>
		</ul>
	</div>
	<div runat="server" id="msg" style="float: left; width: 50%;">
		<div style="clear: both;">
		</div>
	</div>
	<div style="clear: both">
	</div>
	<div class="grid">
		<vdms:PageGridView ID="gvWh" runat="server" AllowPaging="True" AutoGenerateColumns="False"
			DataSourceID="odsWH" OnDataBound="gvWh_DataBound" ShowFooter="True" OnRowDataBound="gvWh_RowDataBound"
			meta:resourcekey="gvWhResource1">
			<Columns>
				<asp:BoundField DataField="Code" HeaderText="Warehouse Code" SortExpression="Code"
					meta:resourcekey="BoundFieldResource1" />
				<asp:BoundField DataField="Address" HeaderText="Name" SortExpression="Address" meta:resourcekey="BoundFieldResource2" />
				<asp:TemplateField HeaderText="Report files" meta:resourcekey="TemplateFieldResource1">
					<ItemTemplate>
						<asp:HyperLink CssClass="thickbox" ID="lnkFiles" runat="server" NavigateUrl="#" Text="..."
							meta:resourcekey="lnkFilesResource1" wCode='<%# Eval("Code") %>' dCode='<%# Eval("DealerCode") %>'></asp:HyperLink>
					</ItemTemplate>
					<FooterTemplate>
						<asp:HyperLink CssClass="thickbox" ID="lnkFiles" runat="server" NavigateUrl="#" Text="..."
							meta:resourcekey="lnkFilesResource2" wCode='<%# Eval("Code") %>' dCode='<%# Eval("DealerCode") %>'></asp:HyperLink>
					</FooterTemplate>
					<ItemStyle CssClass="center" />
					<FooterStyle CssClass="center" />
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Do open to" ItemStyle-CssClass="center" meta:resourcekey="TemplateFieldResource2">
					<ItemTemplate>
						<asp:LinkButton ID="lnkOpen" runat="server" CausesValidation="False" CommandName="Open"
						    WID='<%# Eval("WarehouseId") %>' 
							Text='<%# EvalPrevMonth(Eval("LastMonth"), Eval("LastYear")) %>' OnClick="lnkOpen_Click"
							meta:resourcekey="lnkOpenResource1"></asp:LinkButton>
					</ItemTemplate>
					<ItemStyle CssClass="center"></ItemStyle>
					<FooterStyle CssClass="center"></FooterStyle>
					<FooterTemplate>
						<asp:LinkButton ID="lnkOpenDealer" runat="server" CausesValidation="False" CommandName="Open"
							OnClick="lnkOpenDealer_Click" meta:resourcekey="lnkOpenDealerResource1"></asp:LinkButton>
					</FooterTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Last closed month" SortExpression="LastClosed" ItemStyle-CssClass="center"
					meta:resourcekey="TemplateFieldResource3">
					<ItemTemplate>
						<%# Eval("LastMonth") %>/<%# Eval("LastYear") %>
					</ItemTemplate>
					<ItemStyle CssClass="center"></ItemStyle>
					<FooterStyle CssClass="center"></FooterStyle>
					<FooterTemplate>
						<asp:Literal ID="litLastClosed" runat="server" meta:resourcekey="litLastClosedResource1"></asp:Literal>
					</FooterTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Do close to" ItemStyle-CssClass="center" meta:resourcekey="TemplateFieldResource4">
					<ItemTemplate>
						<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex='<%# EvalCloseForm(Eval("LastYear")) %>'>
							<asp:View ID="View1" runat="server">
								<asp:LinkButton ID="lnkClose" runat="server" CausesValidation="False" CommandName="Close"
								    WID='<%# Eval("WarehouseId") %>' 
									Text='<%# EvalNextMonth(Eval("LastMonth"), Eval("LastYear")) %>' OnClick="lnkClose_Click"
									meta:resourcekey="lnkCloseResource1"></asp:LinkButton>
							</asp:View>
							<asp:View ID="View2" runat="server">
								<asp:TextBox ID="txtCloseMonth" runat="server" WID='<%# Eval("WarehouseId") %>' meta:resourcekey="txtCloseMonthResource1"></asp:TextBox>
								<asp:ImageButton OnClick="imbDoFirstClose_OnClick" ID="imbDoFirstClose" ImageUrl="~/Images/update.gif"
									runat="server" meta:resourcekey="imbDoFirstCloseResource1" />
							</asp:View>
						</asp:MultiView>
					</ItemTemplate>
					<FooterTemplate>
						<asp:MultiView ID="mvCloseForm" runat="server">
							<asp:View ID="View3" runat="server">
								<asp:LinkButton ID="lnkCloseDealer" runat="server" CausesValidation="False" CommandName="Close"
									OnClick="lnkCloseDealer_Click" meta:resourcekey="lnkCloseDealerResource1"></asp:LinkButton>
							</asp:View>
							<asp:View ID="View4" runat="server">
								<asp:TextBox ID="txtCloseMonth" runat="server" meta:resourcekey="txtCloseMonthResource2"></asp:TextBox>
								<asp:ImageButton OnClick="imbDoFirstDealerClose_OnClick" ID="imbDoFirstDealerClose"
									ImageUrl="~/Images/update.gif" runat="server" meta:resourcekey="imbDoFirstDealerCloseResource1" />
							</asp:View>
						</asp:MultiView>
					</FooterTemplate>
					<ItemStyle CssClass="center"></ItemStyle>
					<FooterStyle CssClass="center"></FooterStyle>
				</asp:TemplateField>
			</Columns>
		</vdms:PageGridView>
		<%--<asp:GridView ID="GridView1" runat="server" AllowPaging="false" AutoGenerateColumns="False"
            Visible="false">
            <Columns>
                <asp:BoundField HeaderText="Part code" />
                <asp:BoundField HeaderText="English name" />
                <asp:BoundField HeaderText="Vietnamese name" />
                <asp:BoundField HeaderText="Begin quantity" />
                <asp:BoundField HeaderText="In quantity" />
                <asp:BoundField HeaderText="In amount" Visible="False" />
                <asp:BoundField HeaderText="Out quantity" />
                <asp:BoundField HeaderText="Out amount" Visible="False" />
                <asp:BoundField HeaderText="Balance" />
            </Columns>
        </asp:GridView>--%>
		<asp:ObjectDataSource ID="odsWH" EnablePaging="True" runat="server" SelectMethod="FindWithLock"
			TypeName="VDMS.II.BasicData.WarehouseDAO" SelectCountMethod="CountWithLock">
			<SelectParameters>
				<asp:Parameter DefaultValue="_None_" Name="DealerCode" Type="String" />
			</SelectParameters>
		</asp:ObjectDataSource>
	</div>
	<%--<asp:ListView ID="lvExcel" runat="server">
        <LayoutTemplate>
            <table cellpadding="2" cellspacing="0" border="1">
                <thead>
                    <tr>
                        <th colspan="4" style="background-color: #222222; color: White">
                            Part
                        </th>
                        <th colspan="2" style="background-color: #222222; color: White">
                            Place
                        </th>
                        <th rowspan="2" style="background-color: #222222; color: White">
                            Begin Quantity
                        </th>
                        <th colspan="2" style="background-color: #222222; color: White">
                            In
                        </th>
                        <th colspan="2" style="background-color: #222222; color: White">
                            Out
                        </th>
                        <th rowspan="2" style="background-color: #222222; color: White">
                            Balance
                        </th>
                    </tr>
                    <tr>
                        <th style="background-color: #222222; color: White">
                            No
                        </th>
                        <th style="background-color: #222222; color: White">
                            Code
                        </th>
                        <th style="background-color: #222222; color: White">
                            English name
                        </th>
                        <th style="background-color: #222222; color: White">
                            Vietnamese name
                        </th>
                        <th style="background-color: #222222; color: White">
                            Code
                        </th>
                        <th style="background-color: #222222; color: White">
                            Name
                        </th>
                        <th style="background-color: #222222; color: White">
                            Quantity
                        </th>
                        <th style="background-color: #222222; color: White">
                            Amount
                        </th>
                        <th style="background-color: #222222; color: White">
                            Quantity
                        </th>
                        <th style="background-color: #222222; color: White">
                            Amount
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </tbody>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td align="center" style="background-color: #BBBBBB">
                    <%#Container.DisplayIndex + 1%>
                </td>
                <td align="left" style="background-color: #BBBBBB">
                    <%#Eval("PartCode")%>
                </td>
                <td style="background-color: #BBBBBB">
                    <%#Eval("EnglishName")%>
                </td>
                <td style="background-color: #BBBBBB">
                    <%#Eval("EnglishName")%>
                </td>
                <td style="background-color: #BBBBBB">
                </td>
                <td style="background-color: #BBBBBB">
                </td>
                <td style="background-color: #BBBBBB">
                    <%#Eval("BeginQuantity")%>
                </td>
                <td style="background-color: #BBBBBB">
                    <%#Eval("InQuantity")%>
                </td>
                <td style="background-color: #BBBBBB">
                    <%#Eval("InAmount")%>
                </td>
                <td style="background-color: #BBBBBB">
                    <%#Eval("OutQuantity")%>
                </td>
                <td style="background-color: #BBBBBB">
                    <%#Eval("OutAmount")%>
                </td>
                <td style="background-color: #BBBBBB">
                    <%#Eval("Balance")%>
                </td>
            </tr>
            <asp:ListView ID="lvWarehouses" runat="server" DataSource='<%# Eval("ByWarehouses") %>'>
                <LayoutTemplate>
                    <tr runat="server" id="itemPlaceholder" />
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <%#Eval("PlaceCode")%>
                        </td>
                        <td>
                            <%#Eval("PlaceName")%>
                        </td>
                        <td>
                            <%#Eval("BeginQuantity")%>
                        </td>
                        <td>
                            <%#Eval("InQuantity")%>
                        </td>
                        <td>
                            <%#Eval("InAmount")%>
                        </td>
                        <td>
                            <%#Eval("OutQuantity")%>
                        </td>
                        <td>
                            <%#Eval("OutAmount")%>
                        </td>
                        <td>
                            <%#Eval("Balance")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
            <asp:ListView ID="lvDealers" runat="server" DataSource='<%# Eval("ByDealers") %>'>
                <LayoutTemplate>
                    <tr runat="server" id="itemPlaceholder" />
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <%#Eval("PlaceCode")%>
                        </td>
                        <td>
                            <%#Eval("PlaceName")%>
                        </td>
                        <td>
                            <%#Eval("BeginQuantity")%>
                        </td>
                        <td>
                            <%#Eval("InQuantity")%>
                        </td>
                        <td>
                            <%#Eval("InAmount")%>
                        </td>
                        <td>
                            <%#Eval("OutQuantity")%>
                        </td>
                        <td>
                            <%#Eval("OutAmount")%>
                        </td>
                        <td>
                            <%#Eval("Balance")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </ItemTemplate>
    </asp:ListView>--%>
	<asp:Button Visible="False" ID="Button1" runat="server" Text="Button" OnClick="Button1_Click"
		meta:resourcekey="Button1Resource1" />
</asp:Content>
