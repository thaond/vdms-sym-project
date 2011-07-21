<%@ Page Language="C#" MasterPageFile="~/MP/Popup.master" AutoEventWireup="true"
	CodeFile="SearchPart.aspx.cs" Inherits="Part_Inventory_SearchPart" Theme="Thickbox"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register TagName="UpdateProgress" TagPrefix="cc1" Src="~/Controls/UpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
	<script type="text/javascript">
		$(document).ready(function() {
			$('img.starring').click(function() {
				$.ajax({
					type: "POST",
					url: "SearchPart.aspx/MarkPart",
					data: "{'PartNo':'" + this.id + "', 'Marked':'" + ($(this).hasClass("gmnostar")) + "'}",
					contentType: "application/json; charset=utf-8",
					dataType: "json",
					success: function(data) {
						var star = $(data.d);
						star.toggleClass("gmstar");
						star.toggleClass("gmnostar");
					}
				});
			});
		});
		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<div class="grid" style="width: 420px; border-width: 0px;">
		<table>
			<tr>
				<td>
					<asp:Literal ID="Literal1" runat="server" Text="Part Code:" meta:resourcekey="Literal1Resource1"></asp:Literal>
				</td>
				<td>
					<asp:TextBox ID="tb1" runat="server" meta:resourcekey="tb1Resource1"></asp:TextBox>
				</td>
				<td>
					<asp:Literal ID="Literal2" runat="server" Text="Part Name:" meta:resourcekey="Literal2Resource1"></asp:Literal>
				</td>
				<td>
					<asp:TextBox ID="tb2" runat="server" meta:resourcekey="tb2Resource1"></asp:TextBox>
				</td>
			</tr>
			<asp:PlaceHolder ID="phForPart" runat="server">
				<tr>
					<td>
						<asp:Literal ID="Literal3" runat="server" Text="Engine No:" meta:resourcekey="Literal3Resource1"></asp:Literal>
					</td>
					<td>
						<asp:TextBox ID="tb3" runat="server" meta:resourcekey="tb3Resource1"></asp:TextBox>
					</td>
					<td>
						<asp:Literal ID="Literal4" runat="server" Text="Model:" meta:resourcekey="Literal4Resource1"></asp:Literal>
					</td>
					<td>
						<asp:DropDownList ID="ddl3" runat="server" Width="115px" DataSourceID="odsModel"
							AppendDataBoundItems="true" DataTextField="descript" DataValueField="model" OnDataBound="ddl3_DataBound"
							meta:resourcekey="ddl3Resource1">
							<asp:ListItem Text="" Value="" meta:resourcekey="ListItemResource1"></asp:ListItem>
						</asp:DropDownList>
						<asp:ObjectDataSource ID="odsModel" runat="server" TypeName="VDMS.II.PartManagement.PartDAO"
							SelectMethod="GetModelList">
							<SelectParameters>
								<asp:ControlParameter ControlID="tb1" Name="partCode" PropertyName="Text" />
								<asp:ControlParameter ControlID="tb3" Name="engineNo" PropertyName="Text" />
							</SelectParameters>
						</asp:ObjectDataSource>
                        <asp:TextBox ID="txtModel" runat="server"></asp:TextBox>
					</td>
				</tr>
			</asp:PlaceHolder>
			<tr>
				<td>
				</td>
				<td>
					<asp:PlaceHolder ID="phA" runat="server">
						<asp:RadioButtonList ID="rblType" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
							OnSelectedIndexChanged="rblType_SelectedIndexChanged" meta:resourcekey="rblTypeResource1">
							<asp:ListItem Selected="True" Text="<%$ Resources:TextMsg, Part %>" Value="P" meta:resourcekey="ListItemResource2"></asp:ListItem>
							<asp:ListItem Text="<%$ Resources:TextMsg, Accessory %>" Value="A" meta:resourcekey="ListItemResource3"></asp:ListItem>
						</asp:RadioButtonList>
					</asp:PlaceHolder>
				</td>
				<td class="right">
					<asp:Button ID="cmdFilter" runat="server" Text="Filter" OnClick="cmdFilter_Click"
						meta:resourcekey="cmdFilterResource1" />
				</td>
				<td>
					<cc1:UpdateProgress ID="upg1" runat="server" />
				</td>
			</tr>
		</table>
	</div>
	<div class="grid" style="width: 420px;">
		<div class="title">
			<asp:Literal ID="Literal5" runat="server" Text="Search Part" meta:resourcekey="Literal5Resource1"></asp:Literal>
		</div>
		<%--<asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
			<ContentTemplate>--%>
				<vdms:PageGridView ID="gv1" runat="server" DataSourceID="odsPartList" DataKeyNames="PartInfoId"
					OnPageIndexChanging="gv1_PageIndexChanging" OnPreRender="gv1_PreRender" meta:resourcekey="gv1Resource1">
					<Columns>
						<asp:TemplateField meta:resourcekey="TemplateFieldResource1">
							<ItemTemplate>
								<img id="<%#Eval("PartCode") %>" src="/Images/spacer.gif" class='<%# VDMS.II.PartManagement.PartDAO.IsFavoriteMarked((string)Eval("PartCode"),rblType.SelectedValue, FavQueryType)?"gmstar":"gmnostar" %> starring'	alt="star" />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField meta:resourcekey="TemplateFieldResource2">
							<ItemTemplate>
								<asp:CheckBox ID="CheckBox1" runat="server" meta:resourcekey="CheckBox1Resource1" />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField HeaderText="Part Code" DataField="PartCode" meta:resourcekey="BoundFieldResource1" />
						<asp:BoundField HeaderText="Part Name" DataField="EnglishName" HtmlEncode="False"
							meta:resourcekey="BoundFieldResource2" />
						<asp:BoundField HeaderText="Model" DataField="Model" meta:resourcekey="BoundFieldResource3" />
						<asp:BoundField HeaderText="Stock" DataField="CurrentStock" meta:resourcekey="BoundFieldResource4" />
						<asp:BoundField HeaderText="AvailableStock" DataField="AvailableStock" meta:resourcekey="BoundFieldResource5" />						
					</Columns>
				</vdms:PageGridView>
			<%--</ContentTemplate>
		</asp:UpdatePanel>--%>
		<div class="footer">
			<asp:LinkButton ID="btnSave" runat="server" Text="Save" OnClick="BtnSave_Click" meta:resourcekey="btnSaveResource1" />
		</div>
	</div>
	<asp:ObjectDataSource ID="odsPartList" runat="server" TypeName="VDMS.II.PartManagement.PartDAO"
		SelectMethod="FindAllPart" EnablePaging="True" SelectCountMethod="SelectAllCount">
		<SelectParameters>
			<asp:ControlParameter ControlID="tb1" Type="String" PropertyName="Text" Name="partCode" />
			<asp:ControlParameter ControlID="tb2" Type="String" PropertyName="Text" Name="partName" />
			<asp:ControlParameter ControlID="tb3" Type="String" PropertyName="Text" Name="engineNo" />
			<asp:ControlParameter ControlID="ddl3" Type="String" PropertyName="SelectedValue" Name="model" />
			<asp:ControlParameter ControlID="txtModel" Type="String" PropertyName="Text" Name="manualModel" />
			<asp:ControlParameter ControlID="rblType" Type="String" PropertyName="SelectedValue"
				Name="partType" />
			<asp:QueryStringParameter DefaultValue="" Name="actionType" QueryStringField="at" />
			<asp:QueryStringParameter DefaultValue="" Name="warehouseId" QueryStringField="wh" />
		</SelectParameters>
	</asp:ObjectDataSource>
</asp:Content>
