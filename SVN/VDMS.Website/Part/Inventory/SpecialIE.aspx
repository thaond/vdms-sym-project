<%@ Page Title="Special Import/Export" Language="C#" MasterPageFile="~/MP/MasterPage.master"
	AutoEventWireup="true" CodeFile="SpecialIE.aspx.cs" Inherits="Part_Inventory_SpecialIE"
	Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register TagName="up" TagPrefix="cc1" Src="~/Controls/UpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
	<script type="text/javascript">
		function updated() {
			//  close the popup
			tb_remove();

			//  refresh the update panel so we can view the changes
			$('#<%= this.btnPartInserted.ClientID %>').click();
		}
		function showSearch(link, wh) {
			var s = "SearchPart.aspx?";
			s = s + "code=" + $('#<%= this.txtPartCode.ClientID %>').val();
			s = s + "&name=" + $('#<%= this.txtPartName.ClientID %>').val();
			s = s + "&engno=" + $('#<%= this.txtEngineNo.ClientID %>').val();
			s = s + "&target=SI";
			s = s + "&at=SI";
			s = s + "&wh=" + wh;
			s = s + "&tgKey=" + $('#<%= this._PageKey.ClientID %>').val();
			s = s + "&model=" + $('#<%= this.ddl3.ClientID %>').val();
			s = s + "&TB_iframe=true&height=500&width=420";
			link.href = s;
		}
		function showSearch2(link, wh) {
			var s = "SearchPart.aspx?";
			s = s + "code=" + $('#<%= this.txtPartCode2.ClientID %>').val();
			s = s + "&name=" + $('#<%= this.txtPartName2.ClientID %>').val();
			s = s + "&engno=" + $('#<%= this.txtEngineNo2.ClientID %>').val();
			s = s + "&target=SE";
			s = s + "&at=SE";
			s = s + "&wh=" + $('#<%= this.ddlWarehouseE.ClientID %>').val();
			s = s + "&tgKey=" + $('#<%= this._PageKey.ClientID %>').val();
			s = s + "&model=" + $('#<%= this.ddl4.ClientID %>').val();
			s = s + "&TB_iframe=true&height=320&width=420";
			link.href = s;
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
	<asp:UpdatePanel runat="server" ID="udpMsg">
		<ContentTemplate>
			<asp:HiddenField ID="_PageKey" runat="server" />
			<div runat="server" id="msg">
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
	<cc1:up runat="server" ID="upAll" />
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<asp:Button ID="btnPartInserted" runat="server" Text="Button" CssClass="hidden" OnClick="btnPartInserted_Click"
				meta:resourcekey="btnPartInsertedResource1" />
		</ContentTemplate>
	</asp:UpdatePanel>
	<ajaxToolkit:TabContainer ID="tabCont" runat="server" CssClass="ajax__tab_technorati-theme"
		ActiveTabIndex="0" meta:resourcekey="tabContResource1">
		<ajaxToolkit:TabPanel ID="tabImp" runat="server" HeaderText="Special Import" meta:resourcekey="tabImpResource1">
			<HeaderTemplate>
				<asp:Literal ID="Literal8" runat="server" Text="Special Import" meta:resourcekey="Literal8Resource1"></asp:Literal>
			</HeaderTemplate>
			<ContentTemplate>
				<div class="form" style="width: 50%; float: left;">
					<asp:UpdatePanel ID="up1" runat="server">
						<ContentTemplate>
							<table>
								<tr>
									<td>
										<asp:Literal ID="lit1" runat="server" Text="Warehouse:" meta:resourcekey="lit1Resource1"></asp:Literal>
									</td>
									<td>
										<vdms:WarehouseList ID="ddlWarehouse" runat="server" AutoPostBack="True" ShowSelectAllItem="False"
											OnSelectedIndexChanged="ddlWarehouse_OnSelectedIndexChanged" OnDataBound="ddlWarehouse_OnSelectedIndexChanged"
											DontAutoUseCurrentSealer="False" meta:resourcekey="ddlWarehouseResource1" ShowEmptyItem="False"
											UseVIdAsValue="False">
										</vdms:WarehouseList>
									</td>
								</tr>
								<tr>
									<td>
										<asp:Localize ID="lit2" runat="server" Text="Part Return type:" meta:resourcekey="lit2Resource1"></asp:Localize>
									</td>
									<td>
										<asp:DropDownList ID="ddlPRT" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPRT_SelectedIndexChanged"
											meta:resourcekey="ddlPRTResource1">
											<asp:ListItem Text="Normal" Selected="True" meta:resourcekey="ListItemResource1"></asp:ListItem>
											<asp:ListItem Text="Part Return" meta:resourcekey="ListItemResource2"></asp:ListItem>
										</asp:DropDownList>
									</td>
								</tr>
								<asp:PlaceHolder ID="phNG" runat="server" Visible="False">
									<tr>
										<td>
											<asp:Localize ID="lit3" runat="server" Text="Not Good No:" meta:resourcekey="lit3Resource1"></asp:Localize>
										</td>
										<td>
											<asp:TextBox ID="txtNG" runat="server" meta:resourcekey="txtNGResource1"></asp:TextBox>
											<asp:RequiredFieldValidator ID="rfvNg" runat="server" ErrorMessage="*" SetFocusOnError="True"
												ControlToValidate="txtNG" ValidationGroup="Import" meta:resourcekey="rfvNgResource1"></asp:RequiredFieldValidator>
											<asp:CustomValidator ID="cvNG" runat="server" ErrorMessage="*" SetFocusOnError="True"
												ControlToValidate="txtNG" ValidationGroup="Import" OnServerValidate="cvNG_ServerValidate"
												meta:resourcekey="cvNGResource1"></asp:CustomValidator>
										</td>
									</tr>
								</asp:PlaceHolder>
							</table>
						</ContentTemplate>
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="ddlPRT" EventName="SelectedIndexChanged" />
						</Triggers>
					</asp:UpdatePanel>
				</div>
				<div class="help" style="float: right; width: 35%;">
					<ul>
						<li>
							<asp:Localize ID="lh1" runat="server" Text="Return part from VMEP." meta:resourcekey="lh1Resource1"></asp:Localize>
						</li>
						<li>
							<asp:Localize ID="lh2" runat="server" Text="Import part from other vendor." meta:resourcekey="lh2Resource1"></asp:Localize>
						</li>
						<li>
							<asp:Localize ID="Localize2" runat="server" Text="Leave PART CODE or QUANTITY blank to bypass unused row."
								meta:resourcekey="Localize2Resource1"></asp:Localize>
						</li>
						<li>
							<asp:Localize ID="Localize3" runat="server" Text="If TYPE is &amp;quot;Accessory&amp;quot;, importing parts must be defined in &amp;quot;Part setting&amp;quot; function."
								meta:resourcekey="Localize3Resource1"></asp:Localize>
						</li>
					</ul>
				</div>
				<div style="clear: both;">
				</div>
				<br />
				<asp:ObjectDataSource EnablePaging="True" ID="odsPartList" runat="server" TypeName="SpecialIEDAO"
					SelectMethod="FindImport" SelectCountMethod="CountImportParts" OldValuesParameterFormatString="original_{0}"
					DeleteMethod="DeleteImportPart">
					<DeleteParameters>
						<asp:Parameter Name="key" Type="String" />
					</DeleteParameters>
					<SelectParameters>
						<asp:Parameter Name="key" Type="String" />
						<asp:Parameter Name="maximumRows" Type="Int32" />
						<asp:Parameter Name="startRowIndex" Type="Int32" />
					</SelectParameters>
				</asp:ObjectDataSource>
				<div class="form">
					<table>
						<tr>
							<td>
								<asp:Literal ID="Literal9" runat="server" Text="Part Code:" meta:resourcekey="Literal9Resource1"></asp:Literal>
							</td>
							<td>
								<asp:TextBox ID="txtPartCode" runat="server" Columns="15" meta:resourcekey="txtPartCodeResource1"></asp:TextBox>
							</td>
							<td>
								<asp:Literal ID="Literal10" runat="server" Text="Part Name:" meta:resourcekey="Literal10Resource1"></asp:Literal>
							</td>
							<td>
								<asp:TextBox ID="txtPartName" runat="server" Columns="15" meta:resourcekey="txtPartNameResource1"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td>
								<asp:Literal ID="Literal11" runat="server" Text="Engine No:" meta:resourcekey="Literal11Resource1"></asp:Literal>
							</td>
							<td>
								<asp:TextBox ID="txtEngineNo" runat="server" Columns="15" meta:resourcekey="txtEngineNoResource1"></asp:TextBox>
							</td>
							<td>
								<asp:Literal ID="Literal12" runat="server" Text="Model:" meta:resourcekey="Literal12Resource1"></asp:Literal>
							</td>
							<td>
								<asp:DropDownList ID="ddl3" runat="server" Width="115px" DataSourceID="odsModel"
									AppendDataBoundItems="True" DataTextField="model" meta:resourcekey="ddl3Resource1">
									<asp:ListItem meta:resourcekey="ListItemResource3"></asp:ListItem>
								</asp:DropDownList>
								<asp:ObjectDataSource ID="odsModel" runat="server" TypeName="VDMS.II.PartManagement.PartDAO"
									SelectMethod="GetModelList"></asp:ObjectDataSource>
							</td>
						</tr>
						<tr>
							<td>
							</td>
							<td colspan="3">
								<asp:HyperLink ID="cmdSearch" runat="server" class="thickbox" href="#" onclick="javascript:showSearch(this, 'SI')"
									Text="Search Part" title="Search Part" meta:resourcekey="cmdSearchResource1"></asp:HyperLink>
							</td>
						</tr>
					</table>
					<br />
					<asp:UpdatePanel ID="udpImport" runat="server" UpdateMode="Conditional">
						<ContentTemplate>
							<table cellpadding="0" cellspacing="4" border="0">
								<tr>
									<td>
										<asp:Literal ID="Literal1" runat="server" Text="Excel mode:" meta:resourcekey="Literal1Resource1"></asp:Literal>
									</td>
									<td>
										<asp:LinkButton ID="cmdAddRow" runat="server" OnClick="cmdAddRow_Click" Text="Add"
											meta:resourcekey="cmdAddRowResource1"></asp:LinkButton>
										<asp:DropDownList ID="ddlRowCount" runat="server" meta:resourcekey="ddlRowCountResource1">
											<asp:ListItem Text="5" meta:resourcekey="ListItemResource4"></asp:ListItem>
											<asp:ListItem Text="10" meta:resourcekey="ListItemResource5"></asp:ListItem>
										</asp:DropDownList>
										<asp:Literal ID="Literal2" runat="server" Text="Rows. " meta:resourcekey="Literal2Resource1"></asp:Literal>
									</td>
									<td>
										<asp:Literal ID="Literal3" runat="server" Text="Rows/table:" meta:resourcekey="Literal3Resource1"></asp:Literal>
									</td>
									<td>
										<asp:DropDownList ID="ddlRows" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRows_SelectedIndexChanged"
											meta:resourcekey="ddlRowsResource1">
											<asp:ListItem Text="5" meta:resourcekey="ListItemResource6"></asp:ListItem>
											<asp:ListItem Selected="True" Text="10" meta:resourcekey="ListItemResource7"></asp:ListItem>
											<asp:ListItem Text="20" meta:resourcekey="ListItemResource8"></asp:ListItem>
										</asp:DropDownList>
									</td>
								</tr>
							</table>
							<div class="grid">
								<vdms:PageGridView DataSourceID="odsPartList" ID="gvImp" runat="server" AutoGenerateColumns="False"
									AllowPaging="True" DataKeyNames="PartKey" OnRowDataBound="gv1_RowDataBound" meta:resourcekey="gvImpResource1">
									<Columns>
										<asp:TemplateField HeaderText="Part Code" meta:resourcekey="TemplateFieldResource1">
											<ItemTemplate>
												<asp:TextBox ID="txtPartNo" runat="server" Text='<%# Eval("PartNo") %>' OnTextChanged="UpdatePartInfo"
													meta:resourcekey="txtPartNoResource1"></asp:TextBox>
												<asp:HiddenField ID="hdPartKey" runat="server" Value='<%# Eval("PartKey") %>' />
											</ItemTemplate>
											<ItemStyle Width="150px" />
										</asp:TemplateField>
										<asp:TemplateField HeaderText="Type" meta:resourcekey="TemplateFieldResource2">
											<ItemTemplate>
												<%# Eval("PartType")%>
											</ItemTemplate>
										</asp:TemplateField>
										<asp:TemplateField HeaderText="Quantity" meta:resourcekey="TemplateFieldResource3">
											<ItemTemplate>
												<asp:TextBox ID="txtQty" runat="server" Text='<%# Eval("Quantity") %>' SkinID="InGrid"
													OnTextChanged="UpdatePartInfo" meta:resourcekey="txtQtyResource1"></asp:TextBox>
												<ajaxToolkit:FilteredTextBoxExtender ID="txtQty_FilteredTextBoxExtender" runat="server"
													TargetControlID="txtQty" FilterType="Numbers" Enabled="True">
												</ajaxToolkit:FilteredTextBoxExtender>
											</ItemTemplate>
										</asp:TemplateField>
										<asp:TemplateField HeaderText="Unit price" meta:resourcekey="TemplateFieldResource4">
											<ItemTemplate>
												<asp:TextBox ID="txtUnitPrice" runat="server" Text='<%# Eval("UnitPrice") %>' SkinID="InGrid"
													OnTextChanged="UpdatePartInfo" meta:resourcekey="txtUnitPriceResource1"></asp:TextBox>
												<ajaxToolkit:FilteredTextBoxExtender ID="txtUnitPrice_FilteredTextBoxExtender" runat="server"
													TargetControlID="txtUnitPrice" FilterType="Numbers" Enabled="True">
												</ajaxToolkit:FilteredTextBoxExtender>
											</ItemTemplate>
										</asp:TemplateField>
										<asp:TemplateField HeaderText="Vendor" meta:resourcekey="TemplateFieldResource5">
											<ItemTemplate>
												<vdms:VendorList ID="ddlVendor" runat="server" OnSelectedIndexChanged="UpdatePartInfo"
													meta:resourcekey="ddlVendorResource1">
												</vdms:VendorList>
											</ItemTemplate>
										</asp:TemplateField>
										<asp:TemplateField HeaderText="Remark" meta:resourcekey="TemplateFieldResource6">
											<ItemTemplate>
												<asp:TextBox Width="100%" ID="txtRemark" runat="server" Text='<%# Eval("Remark") %>'
													OnTextChanged="UpdatePartInfo" meta:resourcekey="txtRemarkResource1"></asp:TextBox>
											</ItemTemplate>
											<ItemStyle Width="250px" />
										</asp:TemplateField>
									</Columns>
									<EmptyDataTemplate>
										<asp:Literal ID="Literal8" runat="server" Text="There isn't any rows." meta:resourcekey="Literal8Resource2"></asp:Literal>
									</EmptyDataTemplate>
								</vdms:PageGridView>
								<br />
							</div>
						</ContentTemplate>
					</asp:UpdatePanel>
					<asp:Button ID="btnImport" runat="server" Text="Import" OnClick="btnImport_Click"
						ValidationGroup="Import" meta:resourcekey="btnImportResource1" />
					<asp:Literal ID="litPopupJS" runat="server" meta:resourcekey="litPopupJSResource1"></asp:Literal>
				</div>
			</ContentTemplate>
		</ajaxToolkit:TabPanel>
		<ajaxToolkit:TabPanel ID="tabExp" runat="server" HeaderText="Special Export" meta:resourcekey="tabExpResource1">
			<ContentTemplate>
				<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
					<ContentTemplate>
						<div class="form" style="width: 50%; float: left;">
							<table>
								<tr>
									<td>
										<asp:Literal ID="Literal4" runat="server" Text="Warehouse:" meta:resourcekey="Literal4Resource1"></asp:Literal>
									</td>
									<td>
										<vdms:WarehouseList ID="ddlWarehouseE" runat="server" AutoPostBack="True" ShowSelectAllItem="False"
											OnSelectedIndexChanged="ddlWarehouse_OnSelectedIndexChanged" OnDataBound="ddlWarehouse_OnSelectedIndexChanged"
											DontAutoUseCurrentSealer="False" meta:resourcekey="ddlWarehouseEResource1" ShowEmptyItem="False"
											UseVIdAsValue="False">
										</vdms:WarehouseList>
									</td>
								</tr>
							</table>
						</div>
					</ContentTemplate>
				</asp:UpdatePanel>
				<div class="help" style="float: right; width: 35%;">
					<ul>
						<li>
							<asp:Localize ID="Localize1" runat="server" Text="User for Accessory and the warranty part with customer."
								meta:resourcekey="Localize1Resource1"></asp:Localize>
						</li>
					</ul>
				</div>
				<div style="clear: both;">
				</div>
				<br />
				<asp:ObjectDataSource EnablePaging="True" ID="odsExportPart" runat="server" TypeName="SpecialIEDAO"
					SelectMethod="FindExport" SelectCountMethod="CountExportParts" OldValuesParameterFormatString="original_{0}"
					DeleteMethod="DeleteExportPart">
					<DeleteParameters>
						<asp:Parameter Name="key" Type="String" />
					</DeleteParameters>
					<SelectParameters>
						<asp:Parameter Name="key" Type="String" />
						<asp:Parameter Name="maximumRows" Type="Int32" />
						<asp:Parameter Name="startRowIndex" Type="Int32" />
					</SelectParameters>
				</asp:ObjectDataSource>
				<div class="form">
					<table>
						<tr>
							<td>
								<asp:Literal ID="Literal13" runat="server" Text="Part Code:" meta:resourcekey="Literal13Resource1"></asp:Literal>
							</td>
							<td>
								<asp:TextBox ID="txtPartCode2" runat="server" Columns="15" meta:resourcekey="txtPartCode2Resource1"></asp:TextBox>
							</td>
							<td>
								<asp:Literal ID="Literal14" runat="server" Text="Part Name:" meta:resourcekey="Literal14Resource1"></asp:Literal>
							</td>
							<td>
								<asp:TextBox ID="txtPartName2" runat="server" Columns="15" meta:resourcekey="txtPartName2Resource1"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td>
								<asp:Literal ID="Literal15" runat="server" Text="Engine No:" meta:resourcekey="Literal15Resource1"></asp:Literal>
							</td>
							<td>
								<asp:TextBox ID="txtEngineNo2" runat="server" Columns="15" meta:resourcekey="txtEngineNo2Resource1"></asp:TextBox>
							</td>
							<td>
								<asp:Literal ID="Literal16" runat="server" Text="Model:" meta:resourcekey="Literal16Resource1"></asp:Literal>
							</td>
							<td>
								<asp:DropDownList ID="ddl4" runat="server" Width="115px" DataSourceID="odsModel2"
									AppendDataBoundItems="True" DataTextField="model" meta:resourcekey="ddl4Resource1">
									<asp:ListItem Text="-+-" meta:resourcekey="ListItemResource9"></asp:ListItem>
								</asp:DropDownList>
								<asp:ObjectDataSource ID="odsModel2" runat="server" TypeName="VDMS.II.PartManagement.PartDAO"
									SelectMethod="GetModelList"></asp:ObjectDataSource>
							</td>
						</tr>
						<tr>
							<td>
							</td>
							<td colspan="3">
								<asp:HyperLink ID="cmdSearch2" runat="server" Text="Search Part" class="thickbox"
									title="Search Part" onclick="javascript:showSearch2(this, 'SE')" href="#" meta:resourcekey="cmdSearch2Resource1"></asp:HyperLink>
							</td>
						</tr>
					</table>
					<br />
					<asp:UpdatePanel ID="udpExport" runat="server" UpdateMode="Conditional">
						<ContentTemplate>
							<table cellpadding="0" cellspacing="4" border="0">
								<tr>
									<td>
										<asp:Literal ID="Literal5" runat="server" Text="Excel mode:" meta:resourcekey="Literal5Resource1"></asp:Literal>
									</td>
									<td>
										<asp:LinkButton ID="cmdAddRow2" runat="server" OnClick="cmdAddRow_Click" Text="Add"
											meta:resourcekey="cmdAddRow2Resource1"></asp:LinkButton>
										<asp:DropDownList ID="ddlRowCount2" runat="server" meta:resourcekey="ddlRowCount2Resource1">
											<asp:ListItem Text="5" meta:resourcekey="ListItemResource10"></asp:ListItem>
											<asp:ListItem Text="10" meta:resourcekey="ListItemResource11"></asp:ListItem>
										</asp:DropDownList>
										<asp:Literal ID="Literal6" runat="server" Text="Rows. " meta:resourcekey="Literal6Resource1"></asp:Literal>
									</td>
									<td>
										<asp:Literal ID="Literal7" runat="server" Text="Rows/table:" meta:resourcekey="Literal7Resource1"></asp:Literal>
									</td>
									<td>
										<asp:DropDownList ID="ddlRows2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRows_SelectedIndexChanged"
											meta:resourcekey="ddlRows2Resource1">
											<asp:ListItem Text="5" meta:resourcekey="ListItemResource12"></asp:ListItem>
											<asp:ListItem Selected="True" Text="10" meta:resourcekey="ListItemResource13"></asp:ListItem>
											<asp:ListItem Text="20" meta:resourcekey="ListItemResource14"></asp:ListItem>
										</asp:DropDownList>
									</td>
								</tr>
							</table>
							<div class="grid">
								<vdms:PageGridView DataSourceID="odsExportPart" ID="gvExp" runat="server" AutoGenerateColumns="False"
									AllowPaging="True" DataKeyNames="PartKey" OnRowDataBound="gv1_RowDataBound" meta:resourcekey="gvExpResource1">
									<Columns>
										<asp:TemplateField HeaderText="Part No" meta:resourcekey="TemplateFieldResource7">
											<ItemTemplate>
												<asp:TextBox ID="txtPartNo" runat="server" Text='<%# Eval("PartNo") %>' OnTextChanged="UpdatePartInfo"
													meta:resourcekey="txtPartNoResource2"></asp:TextBox>
												<asp:HiddenField ID="hdPartKey" runat="server" Value='<%# Eval("PartKey") %>' />
											</ItemTemplate>
											<ItemStyle Width="150px" />
										</asp:TemplateField>
										<asp:TemplateField HeaderText="Type" meta:resourcekey="TemplateFieldResource8">
											<ItemTemplate>
												<%# Eval("PartType")%>
											</ItemTemplate>
										</asp:TemplateField>
										<asp:TemplateField HeaderText="Quantity" meta:resourcekey="TemplateFieldResource9">
											<ItemTemplate>
												<asp:TextBox ID="txtQty" runat="server" Text='<%# Eval("Quantity") %>' SkinID="InGrid"
													OnTextChanged="UpdatePartInfo" meta:resourcekey="txtQtyResource2"></asp:TextBox>
												<ajaxToolkit:FilteredTextBoxExtender ID="txtQty_FilteredTextBoxExtender" runat="server"
													TargetControlID="txtQty" FilterType="Numbers" Enabled="True">
												</ajaxToolkit:FilteredTextBoxExtender>
											</ItemTemplate>
										</asp:TemplateField>
										<asp:TemplateField HeaderText="Unit price" meta:resourcekey="TemplateFieldResource10">
											<ItemTemplate>
												<asp:TextBox ID="txtUnitPrice" runat="server" Text='<%# Eval("UnitPrice") %>' SkinID="InGrid"
													OnTextChanged="UpdatePartInfo" meta:resourcekey="txtUnitPriceResource2"></asp:TextBox>
												<ajaxToolkit:FilteredTextBoxExtender ID="txtUnitPrice_FilteredTextBoxExtender" runat="server"
													TargetControlID="txtUnitPrice" FilterType="Numbers" Enabled="True">
												</ajaxToolkit:FilteredTextBoxExtender>
											</ItemTemplate>
										</asp:TemplateField>
										<asp:TemplateField HeaderText="Remark" meta:resourcekey="TemplateFieldResource11">
											<ItemTemplate>
												<asp:TextBox Width="100%" ID="txtRemark" runat="server" Text='<%# Eval("Remark") %>'
													OnTextChanged="UpdatePartInfo" meta:resourcekey="txtRemarkResource2"></asp:TextBox>
											</ItemTemplate>
											<ItemStyle Width="250px" />
										</asp:TemplateField>
									</Columns>
									<EmptyDataTemplate>
										<asp:Literal ID="Literal8" runat="server" Text="There isn't any rows." meta:resourcekey="Literal8Resource3"></asp:Literal>
									</EmptyDataTemplate>
								</vdms:PageGridView>
								<br />
							</div>
						</ContentTemplate>
					</asp:UpdatePanel>
					<asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click"
						meta:resourcekey="btnExportResource1" />
				</div>
			</ContentTemplate>
				<HeaderTemplate>
				<asp:Literal ID="Literal17x" runat="server" Text="Special Export" meta:resourcekey="Literal8Resource1x"></asp:Literal>
			</HeaderTemplate>
		</ajaxToolkit:TabPanel>
	</ajaxToolkit:TabContainer>
</asp:Content>
