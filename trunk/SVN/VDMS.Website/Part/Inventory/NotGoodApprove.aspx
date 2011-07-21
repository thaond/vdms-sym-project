<%@ Page Title="Not Good Confirm" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="NotGoodApprove.aspx.cs" Inherits="Part_Inventory_NotGoodApprove"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register TagPrefix="cc1" Namespace="VDMS.II.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="form" style="width: 50%; float: left;">
		<table width="100%">
			<tr>
				<td>
					<asp:Localize ID="litOrderDate" runat="server" Text="Order Date:" meta:resourcekey="litOrderDateResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="t1" runat="server" MaxLength="10" Columns="10" meta:resourcekey="t1Resource1"></asp:TextBox>
					<asp:ImageButton ID="i1" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
						meta:resourcekey="i1Resource1" />
					<asp:RequiredFieldValidator ID="r1" runat="server" Text="*" SetFocusOnError="True"
						ValidationGroup="Save" ControlToValidate="t1" meta:resourcekey="r1Resource1"></asp:RequiredFieldValidator>
					<ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="t1"
						Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder=""
						CultureDateFormat="MDY" CultureDatePlaceholder="/" CultureDecimalPlaceholder="."
						CultureThousandsPlaceholder="," CultureTimePlaceholder=":" Enabled="True">
					</ajaxToolkit:MaskedEditExtender>
					<ajaxToolkit:CalendarExtender ID="ce1" runat="server" TargetControlID="t1" PopupButtonID="i1"
						Enabled="True">
					</ajaxToolkit:CalendarExtender>
					~
					<asp:TextBox ID="t2" runat="server" MaxLength="10" Columns="10" meta:resourcekey="t2Resource1"></asp:TextBox>
					<asp:ImageButton ID="i2" runat="server" SkinID="CalendarImageButton" OnClientClick="return false;"
						meta:resourcekey="ibToDateResource1" />
					<asp:RequiredFieldValidator ID="r2" runat="server" Text="*" SetFocusOnError="True"
						ValidationGroup="Save" ControlToValidate="t2" meta:resourcekey="rfvToDateResource1"></asp:RequiredFieldValidator>
					<ajaxToolkit:MaskedEditExtender ID="meeToDate" runat="server" TargetControlID="t2"
						Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="AM;PM" CultureCurrencySymbolPlaceholder=""
						CultureDateFormat="MDY" CultureDatePlaceholder="/" CultureDecimalPlaceholder="."
						CultureThousandsPlaceholder="," CultureTimePlaceholder=":" Enabled="True">
					</ajaxToolkit:MaskedEditExtender>
					<ajaxToolkit:CalendarExtender ID="ce2" runat="server" TargetControlID="t2" PopupButtonID="i2"
						Enabled="True">
					</ajaxToolkit:CalendarExtender>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litIssueNo" runat="server" Text="Issue Number:" meta:resourcekey="litIssueNoResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="t3" runat="server" MaxLength="30" Columns="10" meta:resourcekey="t3Resource1"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litDealer" runat="server" Text="Dealer:" meta:resourcekey="litDealerResource1"></asp:Localize>
				</td>
				<td>
					<cc1:DealerList ID="ddlD" runat="server" RemoveRootItem="True" EnabledSaperateByDB="True"
						MergeCode="True" ShowSelectAllItem="True" EnabledSaperateByArea="False" meta:resourcekey="ddlDResource1"
						ShowEmptyItem="False">
					</cc1:DealerList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Literal ID="Literal3" runat="server" Text="Level:" meta:resourcekey="Literal3Resource1"></asp:Literal>
				</td>
				<td>
					<asp:DropDownList ID="ddlL" runat="server" meta:resourcekey="ddlLResource1">
						<asp:ListItem Text="All" Value="0" meta:resourcekey="ListItemResource1"></asp:ListItem>
						<asp:ListItem Text="Level 1" Value="1" meta:resourcekey="ListItemResource2"></asp:ListItem>
						<asp:ListItem Text="Level 2" Value="2" meta:resourcekey="ListItemResource3"></asp:ListItem>
						<asp:ListItem Text="Level 3" Value="3" meta:resourcekey="ListItemResource4"></asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Literal ID="Literal2" runat="server" Text="Status:" meta:resourcekey="Literal2Resource1"></asp:Literal>
				</td>
				<td>
					<asp:DropDownList ID="ddlS" runat="server" meta:resourcekey="ddlSResource1">
						<asp:ListItem Text="All" Value="" meta:resourcekey="ListItemResource5"></asp:ListItem>
						<asp:ListItem Text="New" Value="SN" meta:resourcekey="ListItemResource6"></asp:ListItem>
						<asp:ListItem Text="Confirmed" Value="CF" meta:resourcekey="ListItemResource7"></asp:ListItem>
						<asp:ListItem Text="Rejected" Value="RJ" meta:resourcekey="ListItemResource8"></asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Literal ID="Literal1" runat="server" Text="Type:" meta:resourcekey="Literal1Resource1"></asp:Literal>
				</td>
				<td>
					<asp:DropDownList ID="ddlT" runat="server" meta:resourcekey="ddlTResource1">
						<asp:ListItem Selected="True" Text="All" Value="" meta:resourcekey="ListItemResource9"></asp:ListItem>
						<asp:ListItem Text="Abnormal Receive" Value="N" meta:resourcekey="ListItemResource10"></asp:ListItem>
						<asp:ListItem Text="Part return" Value="S" meta:resourcekey="ListItemResource11"></asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
				</td>
				<td>
					<asp:Button ID="b" runat="server" Text="Search for Issue" OnClick="b_Click" meta:resourcekey="bResource1" />
				</td>
			</tr>
		</table>
	</div>
	<div class="help" style="float: right; width: 30%;">
		<asp:Literal ID="Literal4" runat="server" Text="Part Status:" meta:resourcekey="Literal4Resource1"></asp:Literal>
		<ul>
			<li>
				<asp:Localize ID="lh1" runat="server" Text="B: Broken." meta:resourcekey="lh1Resource1"></asp:Localize>
			</li>
			<li>
				<asp:Localize ID="lh2" runat="server" Text="W: Wrong." meta:resourcekey="lh2Resource1"></asp:Localize>
			</li>
			<li>
				<asp:Localize ID="lh3" runat="server" Text="L: Lack." meta:resourcekey="lh3Resource1"></asp:Localize>
			</li>
			<li>
				<asp:Localize ID="lh4" runat="server" Text="Other: Broken code." meta:resourcekey="lh4Resource1"></asp:Localize>
			</li>
		</ul>
	</div>
	<br />
	<asp:ListView ID="lv" runat="server" DataSourceID="ods1">
		<LayoutTemplate>
			<div class="grid" style="clear: both;">
				<div class="title">
					<asp:Literal ID="Literal4" runat="server" Text="Not Good Issue" meta:resourcekey="Literal4Resource2"></asp:Literal>
				</div>
				<table class="datatable" cellpadding="0" cellspacing="0">
					<thead>
						<tr>
							<th colspan="2">
								<asp:Literal ID="Literal5" runat="server" Text="Part" meta:resourcekey="Literal5Resource1"></asp:Literal>
							</th>
							<th rowspan="2" class="double">
								<asp:Literal ID="Literal6" runat="server" Text="Status" meta:resourcekey="Literal6Resource1"></asp:Literal>
							</th>
							<th colspan="2">
								<asp:Literal ID="Literal7" runat="server" Text="Quantity" meta:resourcekey="Literal7Resource1"></asp:Literal>
							</th>
							<th colspan="4">
								<asp:Literal ID="Literal8" runat="server" Text="Comment" meta:resourcekey="Literal8Resource1"></asp:Literal>
							</th>
						</tr>
						<tr>
							<th>
								<asp:Literal ID="Literal9" runat="server" Text="Code" meta:resourcekey="Literal9Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal10" runat="server" Text="Name" meta:resourcekey="Literal10Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal11" runat="server" Text="Req." meta:resourcekey="Literal11Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal12" runat="server" Text="App." meta:resourcekey="Literal12Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal13" runat="server" Text="Dealer" meta:resourcekey="Literal13Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal14" runat="server" Text="L1" meta:resourcekey="Literal14Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal15" runat="server" Text="L2" meta:resourcekey="Literal15Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal16" runat="server" Text="L3" meta:resourcekey="Literal16Resource2"></asp:Literal>
							</th>
						</tr>
					</thead>
					<tbody>
						<tr id="itemPlaceholder" runat="server">
						</tr>
					</tbody>
				</table>
				<div class="pager">
					<vdms:DataPager ID="dp" runat="server" PageSize="5" PagedControlID="lv">
					</vdms:DataPager>
				</div>
			</div>
		</LayoutTemplate>
		<ItemTemplate>
			<tr class="group">
				<td colspan="9" align="left">
					<table style="width: 550px" class="info">
						<tr>
							<th>
								<asp:Literal ID="Literal16" runat="server" Text="NG Form No:" meta:resourcekey="Literal16Resource1"></asp:Literal>
							</th>
							<td>
							<span style="color: <%#Eval("NGType") == "S" ? "Yellow":"inherit" %> "></span>
								<%# Eval("NotGoodNumber")%>
							</td>
							<th>
								<asp:Literal ID="Literal17" runat="server" Text="Created Date:" meta:resourcekey="Literal17Resource1"></asp:Literal>
							</th>
							<td>
								<%# Eval("CreatedDate")%>
							</td>
							<td>
								<asp:Button ID="b1" runat="server" Text="Confirm" CommandArgument='<%# Eval("NGFormHeaderId") %>'
									OnClick="b1_Click" Enabled='<%# (int)Eval("ApproveLevel")==VDMS.Helper.UserHelper.Profile.NGLevel - 1 %>'
									meta:resourcekey="b1Resource1" />
							</td>
						</tr>
						<tr>
							<th>
								<asp:Literal ID="Literal18" runat="server" Text="Dealer:" meta:resourcekey="Literal18Resource1"></asp:Literal>
							</th>
							<td>
								<%# Eval("DealerCode")%>
							</td>
							<th>
								<asp:Literal ID="Literal19" runat="server" Text="Status:" meta:resourcekey="Literal19Resource1"></asp:Literal>
							</th>
							<td>
								<img src='../../Images/level<%# Eval("ApproveLevel") %>.gif' alt='' />
							</td>
							<td>
								<asp:Button ID="b2" runat="server" Text="Reject" CommandArgument='<%# Eval("NGFormHeaderId") %>'
									OnClick="b2_Click" Enabled='<%# (int)Eval("ApproveLevel")==VDMS.Helper.UserHelper.Profile.NGLevel - 1 %>'
									meta:resourcekey="b2Resource1" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<asp:ListView ID="lv1" runat="server" DataSource='<%# Eval("Items") %>' DataKeyNames="NGFormDetailId">
				<LayoutTemplate>
					<tr runat="server" id="itemPlaceholder" />
				</LayoutTemplate>
				<ItemTemplate>
					<tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
						<td>
							<%#Eval("PartCode")%>
						</td>
						<td>
							<%#Eval("EnglishName")%>
						</td>
						<td>
							<%#Eval("PartStatus")%><%# Eval("BrokenCode")%>
						</td>
						<td class="number">
							<%#Eval("RequestQuantity")%>
						</td>
						<td>
							<asp:TextBox ID="t1" runat="server" Columns="5" Text='<%# Eval("ApprovedQuantity") %>'
								meta:resourcekey="t1Resource2"></asp:TextBox>
						</td>
						<td>
							<%#Eval("DealerComment")%>
						</td>
						<td>
							<asp:TextBox ID="t2" runat="server" Columns="15" Text='<%# Eval("L1Comment") %>'
								Enabled="<%# (VDMS.Helper.UserHelper.Profile.NGLevel==1) %>" meta:resourcekey="t2Resource2"></asp:TextBox>
						</td>
						<td>
							<asp:TextBox ID="t3" runat="server" Columns="15" Text='<%# Eval("L2Comment") %>'
								Enabled="<%# (VDMS.Helper.UserHelper.Profile.NGLevel==2) %>" meta:resourcekey="t3Resource2"></asp:TextBox>
						</td>
						<td>
							<asp:TextBox ID="t4" runat="server" Columns="15" Text='<%# Eval("L3Comment") %>'
								Enabled="<%# (VDMS.Helper.UserHelper.Profile.NGLevel==3) %>" meta:resourcekey="t4Resource2"></asp:TextBox>
						</td>
					</tr>
				</ItemTemplate>
			</asp:ListView>
		</ItemTemplate>
	</asp:ListView>
	<div class="button">
		<asp:Button ID="b1" runat="server" Text="Save" Visible="False" meta:resourcekey="b1Resource2" />
	</div>
	<asp:ObjectDataSource ID="ods1" runat="server" TypeName="VDMS.II.PartManagement.NotGoodDAO"
		EnablePaging="True" SelectMethod="FindAllDetails" SelectCountMethod="GetCountDetails">
		<SelectParameters>
			<asp:ControlParameter ControlID="t1" Name="fromDate" PropertyName="Text" />
			<asp:ControlParameter ControlID="t2" Name="toDate" PropertyName="Text" />
			<asp:ControlParameter ControlID="t3" Name="issueNumber" PropertyName="Text" />
			<asp:ControlParameter ControlID="ddlD" Name="dealer" PropertyName="SelectedValue" />
			<asp:ControlParameter ControlID="ddlS" Name="status" PropertyName="SelectedValue" />
			<asp:ControlParameter ControlID="ddlT" Name="_type" PropertyName="SelectedValue" />
			<asp:ControlParameter ControlID="ddlL" Name="level" PropertyName="SelectedValue" />
		</SelectParameters>
	</asp:ObjectDataSource>
</asp:Content>
