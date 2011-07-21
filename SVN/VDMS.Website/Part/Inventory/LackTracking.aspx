<%@ Page Title="Lack case Tracking" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="LackTracking.aspx.cs" Inherits="Part_Inventory_LackTracking"
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
					<asp:Localize ID="litRewardNo" runat="server" Text="Reward Number:" meta:resourcekey="litRewardNoResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="t4" runat="server" MaxLength="30" Columns="10" meta:resourcekey="t4Resource1"></asp:TextBox>
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
					<asp:Literal ID="Literal2" runat="server" Text="Status:" meta:resourcekey="Literal2Resource1"></asp:Literal>
				</td>
				<td>
					<asp:DropDownList ID="ddlS" runat="server" meta:resourcekey="ddlSResource1">
						<asp:ListItem Text="All" Value="" meta:resourcekey="ListItemResource1"></asp:ListItem>
						<asp:ListItem Text="Processed" Value="true" meta:resourcekey="ListItemResource2"></asp:ListItem>
						<asp:ListItem Text="Not Process" Value="false" meta:resourcekey="ListItemResource3"></asp:ListItem>
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
		<asp:Localize ID="litHelp" runat="server" Text="This form allow VMEP tracking the lack case..."
			meta:resourcekey="litHelpResource1"></asp:Localize>
	</div>
	<br />
	<asp:Label ID="lblSaveOk" runat="server" SkinID="MessageOk" Visible="False" Text="The data has been created/updated successful."
		meta:resourcekey="lblSaveOkResource1"></asp:Label>
	<br />
	<asp:ListView ID="lv" runat="server" DataSourceID="ods1">
		<LayoutTemplate>
			<div class="grid" style="clear: both;">
				<div class="title">
					<asp:Literal ID="Literal1" runat="server" Text="Not Good Issue" meta:resourcekey="Literal1Resource2"></asp:Literal>
				</div>
				<table class="datatable" cellpadding="0" cellspacing="0">
					<thead>
						<tr>
							<th>
								<asp:Literal ID="Literal3" runat="server" Text="Part Code" meta:resourcekey="Literal3Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal4" runat="server" Text="Part Name" meta:resourcekey="Literal4Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal5" runat="server" Text="Lack requested" meta:resourcekey="Literal5Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal6" runat="server" Text="Approved" meta:resourcekey="Literal6Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal7" runat="server" Text="Level 3 Comment" meta:resourcekey="Literal7Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal8" runat="server" Text="Status" meta:resourcekey="Literal8Resource1"></asp:Literal>
							</th>
							<th>
								<asp:Literal ID="Literal9" runat="server" Text="Comment" meta:resourcekey="Literal9Resource1"></asp:Literal>
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
								<asp:Literal ID="Literal1" runat="server" Text="NG Form No:" meta:resourcekey="Literal1Resource1"></asp:Literal>
							</th>
							<td>
								<%# Eval("NotGoodNumber")%>
							</td>
							<th>
								<asp:Literal ID="Literal10" runat="server" Text="Reward Number:" meta:resourcekey="Literal10Resource1"></asp:Literal>
							</th>
							<td>
								<%# Eval("RewardNumber")%>
							</td>
						</tr>
						<tr>
							<th>
								<asp:Literal ID="Literal11" runat="server" Text="Dealer Code:" meta:resourcekey="Literal11Resource1"></asp:Literal>
							</th>
							<td>
								<%# Eval("DealerCode")%>
							</td>
							<th>
								<asp:Literal ID="Literal12" runat="server" Text="Dealer Name:" meta:resourcekey="Literal12Resource1"></asp:Literal>
							</th>
							<td>
								<%# Eval("DealerName")%>
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
							<%#Eval("PartName")%>
						</td>
						<td>
							<%#Eval("RequestQuantity")%>
						</td>
						<td class="number">
							<%#Eval("ApprovedQuantity")%>
						</td>
						<td>
							<%#Eval("L3Comment")%>
						</td>
						<td>
							<asp:CheckBox ID="chkProcess" runat="server" Checked='<%# (bool)Eval("Passed") %>'
								meta:resourcekey="chkProcessResource1" />
						</td>
						<td>
							<asp:TextBox ID="t" runat="server" Columns="15" MaxLength="250" Text='<%# Eval("TransactionComment") %>'
								meta:resourcekey="tResource1"></asp:TextBox>
						</td>
					</tr>
				</ItemTemplate>
			</asp:ListView>
			<tr>
				<td colspan="7" align="right">
					<asp:Button ID="cmdSave" runat="server" Text="Save" OnClick="cmdSave_Click" meta:resourcekey="cmdSaveResource1" />
					<asp:Button ID="lnkPrint" OnDataBinding="lnkPrint_DataBinding" OnClientClick='<%# Eval("NGFormHeaderId","PrintRewardForm.aspx?h={0}") %>'
						runat="server" Text="Print" meta:resourcekey="lnkPrintResource1" />
				</td>
			</tr>
		</ItemTemplate>
	</asp:ListView>
	<div class="button">
		<asp:Button ID="b1" runat="server" Text="Save" Visible="False" meta:resourcekey="b1Resource1" />
	</div>
	<asp:ObjectDataSource ID="ods1" runat="server" TypeName="VDMS.II.PartManagement.NotGoodDAO"
		EnablePaging="True" SelectMethod="FindLackTracking" SelectCountMethod="GetLackTrakingCount">
		<SelectParameters>
			<asp:ControlParameter ControlID="t1" Name="fromDate" PropertyName="Text" />
			<asp:ControlParameter ControlID="t2" Name="toDate" PropertyName="Text" />
			<asp:ControlParameter ControlID="t3" Name="issueNumber" PropertyName="Text" />
			<asp:ControlParameter ControlID="t4" Name="rewardNumber" PropertyName="Text" />
			<asp:ControlParameter ControlID="ddlD" Name="dealer" PropertyName="SelectedValue" />
			<asp:ControlParameter ControlID="ddlS" Name="status" PropertyName="SelectedValue" />
		</SelectParameters>
	</asp:ObjectDataSource>
</asp:Content>
