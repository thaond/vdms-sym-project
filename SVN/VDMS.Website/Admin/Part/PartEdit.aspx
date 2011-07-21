<%@ Page Title="Accessory Edit" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="PartEdit.aspx.cs" Inherits="Admin_Part_PartEdit" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<%@ Register TagPrefix="cc1" Namespace="VDMS.II.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="form" style="width: 500px;">
		<div id="msg" runat="server">
		</div>
		<asp:ValidationSummary ID="ValidationSummary1" CssClass="error"
			runat="server" ValidationGroup="Submit" 
            meta:resourcekey="ValidationSummary1Resource1" />
		<asp:Label ID="lbExistInTipTop" SkinID="MessageError" runat="server" Text="Entered code has been exist in TipTop!"
			Visible="False" meta:resourcekey="lbExistInTipTopResource1"></asp:Label>
		<table width="100%">
			<tr>
				<td style="width: 30%;">
					<asp:Image ID="image6" runat="server" SkinID="RequireFieldNon" 
                        meta:resourcekey="image6Resource1" />
					<asp:Localize runat="server" ID="litSpec" Text="Accessory/Non-SYM type:" 
                        meta:resourcekey="litSpecResource1"></asp:Localize>
				</td>
				<td>
					<vdms:AccessoryTypeList DefaultValue="SYM" ID="ddlAccType" runat="server" 
                        meta:resourcekey="ddlAccTypeResource1" ShowSelectAllItem="False">
					</vdms:AccessoryTypeList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Image ID="image3" runat="server" SkinID="RequireFieldNon" 
                        meta:resourcekey="image3Resource1" />
					<asp:Localize ID="l3" runat="server" Text="Category:" 
                        meta:resourcekey="l3Resource1"></asp:Localize>
				</td>
				<td>
					<cc1:CategoryList ID="ddlCategory" CategoryType="A" runat="server" 
                        meta:resourcekey="ddlCategoryResource1" ShowNullItemIfSelectFailed="False" 
                        ShowSelectAllItem="False">
					</cc1:CategoryList>
					<asp:RequiredFieldValidator ValidationGroup="Submit" ID="RequiredFieldValidator4"
						ControlToValidate="ddlCategory" runat="server" ErrorMessage="Category cannot be blank!" 
                        meta:resourcekey="RequiredFieldValidator4Resource1">*</asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Image ID="image1" runat="server" SkinID="RequireField" 
                        meta:resourcekey="image1Resource1" />
					<asp:Localize ID="l1" runat="server" Text="Accessory/Non-SYM Code:" 
                        meta:resourcekey="l1Resource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="tbPartCode" runat="server" MaxLength="30" Width="180px" 
                        meta:resourcekey="tbPartCodeResource1"></asp:TextBox>
					<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbPartCode"
						ErrorMessage="Part Code Cannot be blank!" ValidationGroup="Submit" 
                        meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Image ID="image2" runat="server" SkinID="RequireField" 
                        meta:resourcekey="image2Resource1" />
					<asp:Localize ID="l2" runat="server" Text="English Name:" 
                        meta:resourcekey="l2Resource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="tbEnName" runat="server" MaxLength="256" Width="180px" 
                        meta:resourcekey="tbEnNameResource1"></asp:TextBox>
					<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbEnName"
						ErrorMessage="English Name cannot be blank!" ValidationGroup="Submit" 
                        meta:resourcekey="RequiredFieldValidator2Resource1">*</asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Image ID="image4" runat="server" SkinID="RequireField" 
                        meta:resourcekey="image4Resource1" />
					<asp:Localize ID="Localize1" runat="server" Text="Vietnam Name:" 
                        meta:resourcekey="Localize1Resource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="tbVnName" runat="server" MaxLength="256" Width="180px" 
                        meta:resourcekey="tbVnNameResource1"></asp:TextBox>
					<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbVnName"
						ErrorMessage="Vietnamese Name cannot be blank!" ValidationGroup="Submit" 
                        meta:resourcekey="RequiredFieldValidator3Resource1">*</asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Image ID="image7" runat="server" SkinID="RequireFieldNon" 
                        meta:resourcekey="image7Resource1" />
					<asp:Localize runat="server" ID="litPrice" Text="Price:" 
                        meta:resourcekey="litPriceResource1"></asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="tbPrice" runat="server" MaxLength="7" Width="180px" 
                        CssClass="number" meta:resourcekey="tbPriceResource1"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Image ID="image8" runat="server" SkinID="RequireFieldNon" 
                        meta:resourcekey="image8Resource1" />
					<asp:Localize runat="server" ID="litPrice0" Text="Order fav rank:" 
                        meta:resourcekey="litPrice0Resource1"></asp:Localize>
				</td>
				<td>
					<vdms:FavoriteRankList ShowEmptyItem="True" ID="ddlOrderRank" runat="server" 
                        meta:resourcekey="ddlOrderRankResource1">
					</vdms:FavoriteRankList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Image ID="image9" runat="server" SkinID="RequireFieldNon" 
                        meta:resourcekey="image9Resource1" />
					<asp:Localize runat="server" ID="litPrice1" Text="Sale fav rank:" 
                        meta:resourcekey="litPrice1Resource1"></asp:Localize>
				</td>
				<td>
					<vdms:FavoriteRankList ShowEmptyItem="True" ID="ddlSaleRank" runat="server" 
                        meta:resourcekey="ddlSaleRankResource1">
					</vdms:FavoriteRankList>
				</td>
			</tr>
		</table>
		<div class="grid">
			<vdms:PageGridView ID="gvSafeties" runat="server" DataSourceID="odsPartSafeties"
				AutoGenerateColumns="False" meta:resourcekey="gvSafetiesResource1">
				<Columns>
					<asp:BoundField HeaderText="Warehouse" DataField="WarehouseAddress" 
                        meta:resourcekey="BoundFieldResource1" />
					<asp:TemplateField HeaderText="SafetyQuantity" SortExpression="SafetyQuantity" 
                        meta:resourcekey="TemplateFieldResource1">
						<ItemTemplate>
							<asp:TextBox CssClass="number" ID="txtSafetyQuantity" runat="server" 
                                Text='<%# Bind("SafetyQuantity") %>' MaxLength="5" WHID='<%# Eval("WarehouseId") %>'
                                meta:resourcekey="txtSafetyQuantityResource1"></asp:TextBox>
						</ItemTemplate>
						<ItemStyle Width="15%" />
					</asp:TemplateField>
					<asp:BoundField DataField="CurrentStock" HeaderText="CurrentStock" 
                        SortExpression="CurrentStock" meta:resourcekey="BoundFieldResource2">
						<ItemStyle CssClass="number" Width="15%" />
					</asp:BoundField>
				</Columns>
			</vdms:PageGridView>
			<asp:ObjectDataSource ID="odsPartSafeties" runat="server" SelectMethod="FindSafetiesForSetting"
				SelectCountMethod="CountSafetiesForSetting" TypeName="VDMS.II.PartManagement.PartInfoDAO">
				<SelectParameters>
					<asp:Parameter Name="partInfoId" Type="Int64" />
					<asp:Parameter Name="dealerCode" Type="String" />
				</SelectParameters>
			</asp:ObjectDataSource>
		</div>
		<div class="button">
			<asp:Button ID="bSave" runat="server" Text="Confirm" OnClick="bSave_Click" 
                ValidationGroup="Submit" meta:resourcekey="bSaveResource1" />
			<asp:Button ID="bCancel" runat="server" Text="Back" 
                OnClientClick="javascript:location.href='Part.aspx'; return false;" 
                meta:resourcekey="bCancelResource1" />
		</div>
	</div>
</asp:Content>
