<%@ Page Language="C#" MasterPageFile="~/MP/Popup.master" AutoEventWireup="true"
	CodeFile="SafetyStock.aspx.cs" Inherits="Part_Inventory_SafetyStock" Theme="Thickbox"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register TagName="UpdateProgress" TagPrefix="cc1" Src="~/Controls/UpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="grid" style="width: 520px;">
		<div class="title">
			<asp:Literal ID="Literal2" runat="server" Text="Safety Stock" meta:resourcekey="Literal2Resource1"></asp:Literal>
		</div>
		<asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<cc1:UpdateProgress ID="upg1" runat="server" />
				<vdms:PageGridView ID="gv1" runat="server" PageSize="10" DataSourceID="odsPartList"
					DataKeyNames="PartCode" meta:resourcekey="gv1Resource1">
					<Columns>
						<asp:TemplateField meta:resourcekey="TemplateFieldResource1">
							<ItemTemplate>
								<asp:CheckBox ID="CheckBox1" runat="server" meta:resourcekey="CheckBox1Resource1" />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField HeaderText="Part Code" DataField="PartCode" meta:resourcekey="BoundFieldResource1" />
						<asp:BoundField HeaderText="English Name" DataField="EnglishName" meta:resourcekey="BoundFieldResource2" />
						<asp:BoundField HeaderText="Vietnam Name" DataField="VietnamName" meta:resourcekey="BoundFieldResource3" />
						<asp:BoundField HeaderText="Safety" DataField="SafetyQuantity" meta:resourcekey="BoundFieldResource4" />
						<asp:BoundField HeaderText="On hand" DataField="CurrentStock" meta:resourcekey="BoundFieldResource5" />
					</Columns>
					<EmptyDataTemplate>
						<asp:Literal ID="Literal2" runat="server" Text="There is not any safety stock data."
							meta:resourcekey="Literal2Resource2"></asp:Literal>
					</EmptyDataTemplate>
				</vdms:PageGridView>
			</ContentTemplate>
		</asp:UpdatePanel>
		<div class="footer">
			<asp:LinkButton ID="btnSave" runat="server" Text="Save" OnClick="BtnSave_Click" meta:resourcekey="btnSaveResource1" />
		</div>
	</div>
	<asp:ObjectDataSource ID="odsPartList" runat="server" TypeName="VDMS.II.PartManagement.PartDAO"
		SelectMethod="FindAllSafety" SelectCountMethod="GetSafetyCount" EnablePaging="True">
		<SelectParameters>
			<asp:QueryStringParameter Name="WarehouseId" QueryStringField="wid" Type="Int32"
				ConvertEmptyStringToNull="true" DefaultValue="0" />
		</SelectParameters>
	</asp:ObjectDataSource>
</asp:Content>
