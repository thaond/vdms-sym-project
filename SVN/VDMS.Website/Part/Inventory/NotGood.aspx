<%@ Page Title="Not Good Form" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="NotGood.aspx.cs" Inherits="Part_Inventory_NotGood"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register TagPrefix="cc1" TagName="UpdateProgress" Src="~/Controls/UpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">

	<script type="text/javascript">
		function updated(refresh) {
			//  close the popup
			tb_remove();
			//  refresh the update panel so we can view the changes
			if (refresh) window.location.reload(true);
		}
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="form" style="width: 50%; float: left;">
		<table width="100%">
			<tr>
				<td>
					<asp:Localize ID="litOrderDate" runat="server" Text="Import Date:" meta:resourcekey="litOrderDateResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="t1" runat="server" Width="88px" meta:resourcekey="t1Resource1"></asp:TextBox>
					<asp:ImageButton ID="i1" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
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
					<asp:TextBox ID="t2" runat="server" Width="88px" meta:resourcekey="txtToDateResource1"></asp:TextBox>
					<asp:ImageButton ID="i2" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
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
					<asp:Localize ID="litPartNo" runat="server" Text="Part No:" meta:resourcekey="litPartNoResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="t" runat="server" meta:resourcekey="tResource1"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td style="width: 100px;">
					<asp:Localize ID="litIssueNumber" runat="server" Text="Issue Number:"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="t3" runat="server" MaxLength="30" Columns="10"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litType" runat="server" Text="NG Type:" meta:resourcekey="litTypeResource1"></asp:Localize>
				</td>
				<td>
					<asp:DropDownList ID="ddlT" runat="server" Width="180px" meta:resourcekey="ddlTResource1">
						<asp:ListItem Selected="True" Text="All" Value="" meta:resourcekey="ListItemResource1"></asp:ListItem>
						<asp:ListItem Text="N: Normal NG Form" Value="N" meta:resourcekey="ListItemResource2"></asp:ListItem>
						<asp:ListItem Text="S: Special NG Form" Value="S" meta:resourcekey="ListItemResource3"></asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litStatus" runat="server" Text="Status:" meta:resourcekey="litStatusResource1"></asp:Localize>
				</td>
				<td>
					<asp:DropDownList ID="ddlS" runat="server" Width="180px" meta:resourcekey="ddlSResource1">
						<asp:ListItem Text="All" Value="" meta:resourcekey="ListItemResource4"></asp:ListItem>
						<asp:ListItem Selected="True" Text="OP: Open, not sent" Value="OP" meta:resourcekey="ListItemResource5"></asp:ListItem>
						<asp:ListItem Text="SN: Sent, sent to VMEP" Value="SN" meta:resourcekey="ListItemResource6"></asp:ListItem>
						<asp:ListItem Text="CF: Confirm by VMEP" Value="CF" meta:resourcekey="ListItemResource7"></asp:ListItem>
						<asp:ListItem Text="RJ: Reject by VMEP" Value="RJ" meta:resourcekey="ListItemResource8"></asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="litDS" runat="server" Text="Detail Status:" meta:resourcekey="litDSResource1"></asp:Localize>
				</td>
				<td>
					<asp:DropDownList ID="ddlDS" runat="server" Width="180px" meta:resourcekey="ddlDSResource1">
						<asp:ListItem Text="All" Value="" meta:resourcekey="ListItemResource9"></asp:ListItem>
						<asp:ListItem Selected="true" Text="P: Passed" Value="true" meta:resourcekey="ListItemResource10"></asp:ListItem>
						<asp:ListItem Text="N: Not Pass" Value="false" meta:resourcekey="ListItemResource11"></asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
				</td>
				<td>
					<asp:Button ID="b" runat="server" Text="Query" OnClick="b_Click" meta:resourcekey="bResource1" />
					<asp:Button ID="cmdAddNew" runat="server" Text="Create new <NG> form" OnClientClick="javascript:location.href='NotGoodEdit.aspx'; return false;"
						meta:resourcekey="cmdAddNewResource1" />
				</td>
			</tr>
		</table>
	</div>
	<div class="help" style="float: right; width: 35%;">
		<asp:Localize ID="lh1" runat="server" Text="Send the issues to VMEP in cases:" meta:resourcekey="lh1Resource1"></asp:Localize>
		<ul>
			<li>
				<asp:Localize ID="lh2" runat="server" Text="Abnormal case (broken, wrong, lack part). You can use Excel mode to manual add parts to grid."
					meta:resourcekey="lh2Resource1"></asp:Localize></li>
			<li>
				<asp:Localize ID="lh3" runat="server" Text="Return part to VMEP (optional function – reserved for future use)."
					meta:resourcekey="lh3Resource1"></asp:Localize></li>
			<li>
				<asp:Localize ID="lh4" runat="server" Text="Type: N (Abnormal receive); S: Return parts."
					meta:resourcekey="lh4Resource1"></asp:Localize>
			</li>
		</ul>
	</div>
	<div style="clear: both;">
	</div>
	<cc1:UpdateProgress ID="upg1" runat="server" />
	<div class="grid">
		<asp:UpdatePanel ID="up" runat="server">
			<ContentTemplate>
				<vdms:PageGridView ID="gv" runat="server" OnRowDataBound="gv_RowDataBound" DataSourceID="ods1"
					AllowPaging="True" DataKeyNames="NGFormHeaderId" meta:resourcekey="gvResource1">
					<Columns>
						<asp:TemplateField meta:resourcekey="TemplateFieldResource1">
							<ItemTemplate>
								<asp:HyperLink ID="h1" runat="server" Text="Edit" meta:resourcekey="h1Resource2"></asp:HyperLink>
								<asp:LinkButton ID="h2" runat="server" Text="Delete" CommandName="Delete" meta:resourcekey="h2Resource2"></asp:LinkButton>
								<asp:HyperLink ID="h3" runat="server" Text="View" CssClass="thickbox" meta:resourcekey="h3Resource2"></asp:HyperLink>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:HyperLinkField DataNavigateUrlFields="NGFormHeaderId" Target="_blank" DataNavigateUrlFormatString="~/Part/Inventory/PrintNGForm.aspx?id={0}"
							ShowHeader="False" Text="Print" meta:resourcekey="HyperLinkFieldResource1" />
						<asp:BoundField HeaderText="Created Date" DataField="CreatedDate" meta:resourcekey="BoundFieldResource1" />
						<asp:BoundField HeaderText="Not Good Number" DataField="NotGoodNumber" meta:resourcekey="BoundFieldResource2" />
						<asp:BoundField HeaderText="Confirm Date" DataField="ApproveDate" DataFormatString="{0:d}"
							meta:resourcekey="BoundFieldResource3" />
						<asp:BoundField HeaderText="Status" DataField="Status" meta:resourcekey="BoundFieldResource4" />
						<asp:BoundField HeaderText="Type" DataField="NGType" meta:resourcekey="BoundFieldResource5" />
						<asp:HyperLinkField Text="Confirm and Send" DataNavigateUrlFields="NGFormHeaderId"
							DataNavigateUrlFormatString="NotGoodConfirm.aspx?id={0}&TB_iframe=true&height=320&width=420"
							meta:resourcekey="HyperLinkFieldResource2">
							<ControlStyle CssClass="thickbox" />
						</asp:HyperLinkField>
					</Columns>
					<EmptyDataTemplate>
						<asp:Literal ID="Literal1" runat="server" Text="There isn't any rows." meta:resourcekey="Literal1Resource1"></asp:Literal>
					</EmptyDataTemplate>
				</vdms:PageGridView>
			</ContentTemplate>
		</asp:UpdatePanel>
		<asp:ObjectDataSource ID="ods1" runat="server" EnablePaging="True" TypeName="VDMS.II.PartManagement.NotGoodDAO"
			SelectMethod="FindAll" SelectCountMethod="GetCountAll" DeleteMethod="Delete">
			<SelectParameters>
				<asp:ControlParameter ControlID="t1" Name="fromDate" PropertyName="Text" />
				<asp:ControlParameter ControlID="t2" Name="toDate" PropertyName="Text" />
				<asp:ControlParameter ControlID="t" Name="partCode" PropertyName="Text" />
				<asp:ControlParameter ControlID="t3" Name="issueNumber" PropertyName="Text" />
				<asp:ControlParameter ControlID="ddlS" PropertyName="SelectedValue" Name="status" />
				<asp:ControlParameter ControlID="ddlT" PropertyName="SelectedValue" Name="NGtype" />
				<asp:ControlParameter ControlID="ddlDS" PropertyName="SelectedValue" Name="IsPassed" />
			</SelectParameters>
		</asp:ObjectDataSource>
	</div>
</asp:Content>
