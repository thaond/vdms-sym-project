<%@ Page Title="Find new part" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="FindPart.aspx.cs" Inherits="Part_Inventory_FindPart" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="form" style="width: 450px">
		<asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Search"
			CssClass="error" meta:resourcekey="ValidationSummary1Resource1" />
		<table width="100%">
			<tr>
				<td>
					<asp:Localize ID="litDate" runat="server" Text="From Date:" 
                        meta:resourcekey="litDateResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="txtFromDate" runat="server" Width="100px" 
                        meta:resourcekey="txtFromDateResource1"></asp:TextBox>
					<asp:ImageButton ID="ibFromDate" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;"
						meta:resourcekey="ibFromDateResource1" />
					<ajaxToolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
						Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" 
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True">
					</ajaxToolkit:MaskedEditExtender>
					<ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtFromDate"
						PopupButtonID="ibFromDate" Enabled="True">
					</ajaxToolkit:CalendarExtender>
					<asp:RequiredFieldValidator ValidationGroup="Search" ID="RequiredFieldValidator1"
						runat="server" SetFocusOnError="True" ControlToValidate="txtFromDate" 
                        ErrorMessage="Created date cannot be blank!" 
                        meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="Localize1" runat="server" Text="Area:" 
                        meta:resourcekey="Localize1Resource1"></asp:Localize>
				</td>
				<td>
					<vdms:DatabaseList runat="server" ID="ddlDatabase" ShowSelectAllItem="False" 
                        AllowDealerSelect="False" meta:resourcekey="ddlDatabaseResource1">
					</vdms:DatabaseList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="Localize2" runat="server" Text="Category:" 
                        meta:resourcekey="Localize2Resource1"></asp:Localize>
				</td>
				<td>
					<vdms:CategoryList runat="server" ID="ddlCategory" ShowSelectAllItem="True" 
                        CategoryType="P" meta:resourcekey="ddlCategoryResource1" 
                        ShowNullItemIfSelectFailed="False">
					</vdms:CategoryList>
				</td>
			</tr>
			<tr>
				<td>
				</td>
				<td>
					<asp:Button ID="btnFind" runat="server" ValidationGroup="Search" Text="Find" 
                        OnClick="btnFind_Click" meta:resourcekey="btnFindResource1" />
				</td>
			</tr>
		</table>
	</div>
	<br />
	<%--DataSourceID="odsParts"--%>
	<div class="grid">
		<vdms:PageGridView ID="gvParts" runat="server" AutoGenerateColumns="False" 
            AllowPaging="True" meta:resourcekey="gvPartsResource1">
			<Columns>
				<asp:TemplateField HeaderText="Used" meta:resourcekey="TemplateFieldResource1">
					<ItemTemplate>
						<asp:CheckBox ID="chbUsed" runat="server" 
                            Checked='<%# EvalExist(Eval("PartCode")) %>' 
                            meta:resourcekey="chbUsedResource1" />
					</ItemTemplate>
					<ItemStyle CssClass="center" />
				</asp:TemplateField>
				<asp:BoundField HeaderText="Part Code" DataField="PartCode" 
                    meta:resourcekey="BoundFieldResource1" />
				<asp:BoundField HeaderText="English Name" DataField="EnglishName" 
                    meta:resourcekey="BoundFieldResource2" />
				<asp:BoundField HeaderText="VietNam Name" DataField="VietnamName" 
                    meta:resourcekey="BoundFieldResource3" />
				<%--<asp:BoundField HeaderText="Model" DataField="Model" />--%>
				<asp:BoundField HeaderText="Created date" DataField="PartCode" 
                    meta:resourcekey="BoundFieldResource4" />
			</Columns>
			<EmptyDataTemplate>
				There isn't any rows.
			</EmptyDataTemplate>
		</vdms:PageGridView>
		<asp:ObjectDataSource ID="odsParts" runat="server" SelectMethod="FindNewPart" TypeName="VDMS.II.PartManagement.PartDAO"
			SelectCountMethod="CountNewPart" EnablePaging="True">
			<SelectParameters>
				<asp:ControlParameter ControlID="txtFromDate" Name="createFrom" PropertyName="Text"
					Type="DateTime" />
				<asp:Parameter Name="createTo" Type="DateTime" />
				<asp:ControlParameter ControlID="ddlCategory" Name="category" PropertyName="SelectedValue"
					Type="String" />
				<asp:ControlParameter ControlID="ddlDatabase" Name="databaseCode" PropertyName="SelectedValue"
					Type="String" />
			</SelectParameters>
		</asp:ObjectDataSource>
	</div>
	<asp:Button ID="btnAdd" runat="server" Text="test: Add to my part list" OnClick="btnAdd_Click"
		Visible="False" meta:resourcekey="btnAddResource1" />
	<asp:Button ID="btnAddAcc" runat="server" Text="test: Add to my accessory" OnClick="btnAddAcc_Click"
		Visible="False" meta:resourcekey="btnAddAccResource1" />
</asp:Content>
