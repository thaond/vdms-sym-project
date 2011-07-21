<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddExchange.ascx.cs" Inherits="UC_AddExchange" %>
<%@ Register Assembly="VDMS.Common" Namespace="VDMS.Common.Web" TagPrefix="cc1" %>
<asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Insert"
	meta:resourcekey="ValidationSummary1Resource1" />
<asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Save"
	meta:resourcekey="ValidationSummary2Resource1" />
<asp:BulletedList ID="bllErrorMsg" runat="server" CssClass="errorMsg" meta:resourcekey="bllErrorMsgResource1">
</asp:BulletedList>
<br />
<table border="0" cellpadding="2" cellspacing="0" class="InputTable" style="width: 300px">
	<tr>
		<td>
		</td>
		<td>
		</td>
		<td style="width: 1%">
		</td>
		<td style="width: 1%">
		</td>
		<td style="width: 30%">
		</td>
		<td class="nameField" align="right" colspan="2">
			<asp:Literal ID="Literal34" runat="server" Text="Receipt:" meta:resourcekey="Literal34Resource1"></asp:Literal>
			<asp:TextBox ID="txtexReceipt" runat="server" CssClass="readOnlyInputField" MaxLength="30"
				meta:resourcekey="txtexReceiptResource1" ReadOnly="True" Width="186px"></asp:TextBox>
		</td>
		<td>
		</td>
	</tr>
	<tr>
		<td colspan="8">
			&nbsp;
		</td>
	</tr>
	<tr>
		<td colspan="8">
			<table class="GridView" border="0" cellpadding="0" cellspacing="0" width="100%">
				<caption>
					<asp:Literal ID="Literal16" runat="server" Text="Vehicle and customer" meta:resourcekey="Literal16Resource1"></asp:Literal></caption>
				<tr>
					<td>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td class="nameField" style="width: 25%">
			<asp:Literal ID="Literal32" runat="server" Text="Engine number:" meta:resourcekey="Literal32Resource1"></asp:Literal>
		</td>
		<td>
			<asp:TextBox ID="txtexEngineNumber" runat="server" CssClass="readOnlyInputField"
				MaxLength="20" meta:resourcekey="txtexEngineNumberResource1" ReadOnly="True"></asp:TextBox>
			<asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage='"Engine number" cannot be blank!'
				ControlToValidate="txtexEngineNumber" ValidationGroup="Save" Text="*" meta:resourcekey="RequiredFieldValidator11Resource1"></asp:RequiredFieldValidator>
		</td>
		<td style="width: 1%">
		</td>
		<td style="width: 1%">
		</td>
		<td style="width: 30%">
		</td>
		<td class="nameField" style="width: 25%">
			<asp:Literal ID="Literal40" runat="server" Text="Frame number:" meta:resourcekey="Literal40Resource1"></asp:Literal>
		</td>
		<td style="width: 1%">
			<asp:TextBox ID="txtexFrameNum" runat="server" CssClass="readOnlyInputField" MaxLength="30"
				meta:resourcekey="txtexFrameNumResource1" ReadOnly="True"></asp:TextBox>
		</td>
		<td>
		</td>
	</tr>
	<tr>
		<td class="nameField" style="width: 25%">
			<asp:Literal ID="Literal44" runat="server" Text="Model:" meta:resourcekey="Literal44Resource1"></asp:Literal>
		</td>
		<td>
			<asp:TextBox ID="txtexModel" runat="server" CssClass="readOnlyInputField" MaxLength="20"
				meta:resourcekey="txtexModelResource1" ReadOnly="True"></asp:TextBox>
		</td>
		<td style="width: 1%">
		</td>
		<td style="width: 1%">
		</td>
		<td style="width: 30%">
		</td>
		<td class="nameField" style="width: 25%">
			<asp:Literal ID="Literal45" runat="server" Text="Kilometers:" meta:resourcekey="Literal45Resource1"></asp:Literal>
		</td>
		<td style="width: 1%">
			<asp:TextBox ID="txtexKm" runat="server" CssClass="readOnlyInputField" MaxLength="15"
				meta:resourcekey="txtexKmResource1" ReadOnly="True"></asp:TextBox>
		</td>
		<td>
		</td>
	</tr>
	<tr>
		<td class="nameField" style="width: 25%">
			<asp:Literal ID="Literal39" runat="server" Text="Customer name:" meta:resourcekey="Literal39Resource1"></asp:Literal>
		</td>
		<td>
			<asp:TextBox ID="txtexCustName" runat="server" CssClass="readOnlyInputField" meta:resourcekey="txtexCustNameResource1"
				ReadOnly="True"></asp:TextBox>
		</td>
		<td style="width: 1%">
		</td>
		<td style="width: 1%">
		</td>
		<td style="width: 30%">
		</td>
		<td class="nameField" style="width: 25%">
			<asp:Literal ID="Literal42" runat="server" Text="Phone:" meta:resourcekey="Literal42Resource1"></asp:Literal>
		</td>
		<td style="width: 1%">
			<asp:TextBox ID="txtexPhone" runat="server" CssClass="readOnlyInputField" meta:resourcekey="txtexPhoneResource1"
				ReadOnly="True"></asp:TextBox>
		</td>
		<td>
		</td>
	</tr>
	<tr>
		<td class="nameField" style="width: 25%">
			<asp:Literal ID="Literal41" runat="server" Text="Address:" meta:resourcekey="Literal41Resource1"></asp:Literal>
		</td>
		<td colspan="6">
			<asp:TextBox ID="txtexAddress" runat="server" Width="99%" CssClass="readOnlyInputField"
				meta:resourcekey="txtexAddressResource1" ReadOnly="True"></asp:TextBox>
		</td>
		<td>
		</td>
	</tr>
	<tr>
		<td colspan="8">
			&nbsp;
		</td>
	</tr>
	<tr>
		<td class="nameField" style="width: 25%">
			<asp:Literal ID="Literal43" runat="server" Text="Dealer:" meta:resourcekey="Literal43Resource1"></asp:Literal>
		</td>
		<td>
			<asp:TextBox ID="txtexDealer" runat="server" CssClass="readOnlyInputField" meta:resourcekey="txtexDealerResource1"
				ReadOnly="True"></asp:TextBox>
		</td>
		<td style="width: 1%">
		</td>
		<td style="width: 1%">
		</td>
		<td style="width: 30%">
		</td>
		<td class="nameField" style="width: 25%">
			<asp:Literal ID="Literal47" runat="server" Text="Area code:" meta:resourcekey="Literal47Resource1"></asp:Literal>
		</td>
		<td style="width: 1%">
			<asp:TextBox ID="txtexAreaCode" runat="server" CssClass="readOnlyInputField" meta:resourcekey="txtexAreaCodeResource1"
				ReadOnly="True"></asp:TextBox>
		</td>
		<td>
		</td>
	</tr>
	<tr>
		<td class="nameField" style="width: 25%">
			<asp:Literal ID="Literal46" runat="server" Text="Buy date:" meta:resourcekey="Literal46Resource1"></asp:Literal>
		</td>
		<td>
			<asp:TextBox ID="txtexBuyDate" runat="server" CssClass="readOnlyInputField" meta:resourcekey="txtexBuyDateResource1"
				ReadOnly="True"></asp:TextBox>
		</td>
		<td style="width: 1%">
		</td>
		<td style="width: 1%">
		</td>
		<td style="width: 30%">
		</td>
		<td class="nameField" style="width: 25%">
			<asp:Literal ID="Literal50" runat="server" Text="Repair date:" meta:resourcekey="Literal50Resource1"></asp:Literal>
		</td>
		<td style="width: 1%">
			<asp:TextBox ID="txtexRepairDate" runat="server" CssClass="readOnlyInputField" meta:resourcekey="txtexRepairDateResource1"
				ReadOnly="True"></asp:TextBox>
		</td>
		<td>
		</td>
	</tr>
	<tr>
		<td class="nameField" style="width: 25%">
			<asp:Image ID="Image2" runat="server" SkinID="RequireField" meta:resourcekey="Image2Resource1" /><asp:Literal
				ID="Literal49" runat="server" Text="Damaged date:" meta:resourcekey="Literal49Resource1"></asp:Literal>
		</td>
		<td>
			<asp:TextBox ID="txtexDamagedDate" runat="server" Width="119px" meta:resourcekey="txtexDamagedDateResource1"></asp:TextBox>
			<asp:ImageButton ID="ibtnCalendar" runat="Server" OnClientClick="return false;" SkinID="CalendarImageButton"
				meta:resourcekey="ibtnCalendarResource1" />
			<asp:RequiredFieldValidator ID="rqvBuydate" runat="server" ControlToValidate="txtexDamagedDate"
				ErrorMessage='"Buy date" cannot be blank!' ValidationGroup="Save" Width="1%"
				Text="*" meta:resourcekey="rqvBuydateResource1" EnableViewState="False"></asp:RequiredFieldValidator>
			<ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" BehaviorID="CalendarExtender3"
				Enabled="True" PopupButtonID="ibtnCalendar" TargetControlID="txtexDamagedDate">
			</ajaxToolkit:CalendarExtender>
			<ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" BehaviorID="ctl00_MaskedEditExtender2"
				Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtexDamagedDate">
			</ajaxToolkit:MaskedEditExtender>
		</td>
		<td style="width: 1%">
		</td>
		<td style="width: 1%">
		</td>
		<td style="width: 30%">
		</td>
		<td class="nameField" style="width: 25%">
			<asp:Literal ID="Literal48" runat="server" Text="Made date:" meta:resourcekey="Literal48Resource1"></asp:Literal>
		</td>
		<td style="width: 1%">
			<asp:TextBox ID="txtexExportDate" runat="server" CssClass="readOnlyInputField" meta:resourcekey="txtexExportDateResource1"
				ReadOnly="True"></asp:TextBox>
		</td>
		<td>
		</td>
	</tr>
	<tr>
		<td colspan="8">
		</td>
	</tr>
	<tr>
		<td class="nameField" style="width: 25%">
			<asp:Literal ID="Literal33" runat="server" Text="Using condition" meta:resourcekey="Literal33Resource1"></asp:Literal>
		</td>
		<td class="forceNowrap" colspan="6">
			<table border="0" cellpadding="2" cellspacing="1" class="GridView">
				<tr>
					<th>
						<asp:Literal ID="Literal37" runat="server" Text="Road" meta:resourcekey="Literal37Resource1"></asp:Literal>
					</th>
					<th>
						<asp:Literal ID="Literal2" runat="server" Text="Weather" meta:resourcekey="Literal2Resource1"></asp:Literal>
					</th>
					<th>
						&nbsp;<asp:Literal ID="Literal3" runat="server" Text="Speed" meta:resourcekey="Literal3Resource1"></asp:Literal>
					</th>
					<th>
						&nbsp;<asp:Literal ID="Literal38" runat="server" Text="Transport" meta:resourcekey="Literal38Resource1"></asp:Literal>
					</th>
				</tr>
				<tr>
					<td style="background-color: #f6f6f6">
						<asp:RadioButtonList ID="rblRoad" runat="server" RepeatDirection="Horizontal" meta:resourcekey="rblRoadResource1">
							<asp:ListItem Value="0" Selected="True" Text="Good" meta:resourcekey="ListItemResource1"></asp:ListItem>
							<asp:ListItem Value="1" Text="Bad" meta:resourcekey="ListItemResource2"></asp:ListItem>
							<asp:ListItem Value="2" Text="Mountain" meta:resourcekey="ListItemResource3"></asp:ListItem>
						</asp:RadioButtonList>
					</td>
					<td style="background-color: #f6f6f6">
						<asp:RadioButtonList ID="rblWeather" runat="server" RepeatDirection="Horizontal"
							meta:resourcekey="rblWeatherResource1">
							<asp:ListItem Value="0" Selected="True" Text="Sunny" meta:resourcekey="ListItemResource4"></asp:ListItem>
							<asp:ListItem Value="1" Text="Rainning" meta:resourcekey="ListItemResource5"></asp:ListItem>
						</asp:RadioButtonList>
					</td>
					<td style="background-color: #f6f6f6">
						<asp:RadioButtonList ID="rblSpeed" runat="server" RepeatDirection="Horizontal" meta:resourcekey="rblSpeedResource1">
							<asp:ListItem Value="0" Text="Galanti" meta:resourcekey="ListItemResource6"></asp:ListItem>
							<asp:ListItem Value="1" Text="Slow" meta:resourcekey="ListItemResource7"></asp:ListItem>
							<asp:ListItem Value="2" Selected="True" Text="Normal" meta:resourcekey="ListItemResource8"></asp:ListItem>
							<asp:ListItem Value="3" Text="High" meta:resourcekey="ListItemResource9"></asp:ListItem>
						</asp:RadioButtonList>
					</td>
					<td style="background-color: #f6f6f6">
						<asp:RadioButtonList ID="rblTransport" runat="server" RepeatDirection="Horizontal"
							meta:resourcekey="rblTransportResource1">
							<asp:ListItem Value="0" Text="Goods" meta:resourcekey="ListItemResource10"></asp:ListItem>
							<asp:ListItem Value="1" Selected="True" Text="Human" meta:resourcekey="ListItemResource11"></asp:ListItem>
						</asp:RadioButtonList>
					</td>
				</tr>
			</table>
		</td>
		<td>
		</td>
	</tr>
	<tr>
		<td colspan="8">
			&nbsp;
		</td>
	</tr>
	<tr>
		<td colspan="8">
			<table class="GridView" border="0" cellpadding="0" cellspacing="0" style="width: 100%"
				width="100%">
				<caption>
					<asp:Literal ID="Literal15" runat="server" Text="Damage informations" meta:resourcekey="Literal15Resource1"></asp:Literal></caption>
				<tr>
					<td>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td class="nameField" style="width: 25%">
			<asp:Literal ID="Literal10" runat="server" Text="Engine:" meta:resourcekey="Literal10Resource1"></asp:Literal>
		</td>
		<td colspan="6">
			<asp:TextBox ID="txtexEngineDmg" runat="server" Width="98%" MaxLength="256" meta:resourcekey="txtexEngineDmgResource1"></asp:TextBox>
		</td>
		<td>
		</td>
	</tr>
	<tr>
		<td class="nameField" style="width: 25%">
			<asp:Literal ID="Literal6" runat="server" Text="Frame:" meta:resourcekey="Literal6Resource1"></asp:Literal>
		</td>
		<td colspan="6">
			<asp:TextBox ID="txtexFrameDmg" runat="server" Width="98%" MaxLength="256" meta:resourcekey="txtexFrameDmgResource1"></asp:TextBox>
		</td>
		<td>
		</td>
	</tr>
	<tr>
		<td class="nameField" style="width: 25%">
			<asp:Literal ID="Literal7" runat="server" Text="Electrical:" meta:resourcekey="Literal7Resource1"></asp:Literal>
		</td>
		<td colspan="6">
			<asp:TextBox ID="txtexElectricalDmg" runat="server" Width="98%" MaxLength="256" meta:resourcekey="txtexElectricalDmgResource1"></asp:TextBox>
		</td>
		<td>
		</td>
	</tr>
	<tr>
		<td class="nameField" style="width: 25%">
			<asp:Image ID="Image4" runat="server" SkinID="RequireField" meta:resourcekey="Image4Resource1" /><asp:Literal
				ID="Literal8" runat="server" Text="Damage:" meta:resourcekey="Literal8Resource1"></asp:Literal>
		</td>
		<td colspan="6">
			<asp:TextBox ID="txtexDamage" runat="server" Rows="3" TextMode="MultiLine" Width="98%"
				MaxLength="512" meta:resourcekey="txtexDamageResource1"></asp:TextBox>
		</td>
		<td>
			<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtexDamage"
				ErrorMessage="Damage cannot be blank!" ValidationGroup="Save" Text="*" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr>
		<td class="nameField" style="width: 25%">
			<asp:Image ID="Image5" runat="server" SkinID="RequireField" meta:resourcekey="Image5Resource1" /><asp:Literal
				ID="Literal9" runat="server" Text="Reason:" meta:resourcekey="Literal9Resource1"></asp:Literal>
		</td>
		<td colspan="6">
			<asp:TextBox ID="txtexReason" runat="server" Rows="3" TextMode="MultiLine" Width="98%"
				MaxLength="512" meta:resourcekey="txtexReasonResource1"></asp:TextBox>
		</td>
		<td>
			<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtexReason"
				ErrorMessage="Reason cannot be blank!" ValidationGroup="Save" Text="*" meta:resourcekey="RequiredFieldValidator2Resource1"></asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr>
		<td class="nameField" style="width: 25%">
			<asp:Literal ID="Literal5" runat="server" Text="Note:" meta:resourcekey="Literal5Resource1"></asp:Literal>
		</td>
		<td colspan="6">
			<asp:TextBox ID="txtexNote" runat="server" Width="98%" MaxLength="1000" Rows="3"
				TextMode="MultiLine" meta:resourcekey="txtexNoteResource1"></asp:TextBox>
		</td>
		<td>
		</td>
	</tr>
</table>
<br />
<table border="0" cellpadding="0" cellspacing="0">
	<tr>
		<td>
			<cc1:EmptyGridViewEx ID="gvexSpareList" runat="server" AllowInsertEmptyRow="False"
				AutoGenerateColumns="False" CssClass="GridView" ShowEmptyTable="True" SubmitControlIdPrefix="txtex"
				OnRowCancelingEdit="gvexSpareList_RowCancelingEdit" OnRowCommand="gvexSpareList_RowCommand"
				OnRowEditing="gvexSpareList_RowEditing" ShowFooter="True" OnDataBound="gvexSpareList_DataBound"
				OnRowDeleting="gvexSpareList_RowDeleting" OnRowUpdating="gvexSpareList_RowUpdating"
				OnRowDataBound="gvexSpareList_RowDataBound" meta:resourcekey="gvexSpareListResource1">
				<Columns>
					<asp:ImageField DataImageUrlField="NoWarranty" NullImageUrl="~/Images/s_mustinput.gif"
						ReadOnly="True" meta:resourcekey="ImageFieldResource1">
					</asp:ImageField>
					<asp:TemplateField HeaderText="Spare number" meta:resourcekey="TemplateFieldResource1">
						<EditItemTemplate>
							<table cellspacing="0" cellpadding="0" border="0">
								<tbody>
									<tr>
										<td>
											<asp:TextBox ID="txtexSpareNumber" runat="server" Text='<%# Bind("SpareNumber") %>'
												Width="90px" MaxLength="35" meta:resourceKey="txtexSpareNumberResource1" __designer:wfdid="w22"></asp:TextBox>
										</td>
										<td>
											<asp:Button ID="btnexFindSpare" OnClick="btnexFindSpare_Click" meta:resourcekey="btnexFindSpareResource1"
												runat="server" Text="..." __designer:wfdid="w23"></asp:Button>
										</td>
										<td>
											<asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="Insert"
												runat="server" Text="*" ControlToValidate="txtexSpareNumber" ErrorMessage="Spare number cannot be blank!"
												meta:resourceKey="RequiredFieldValidator6Resource1" __designer:wfdid="w24"></asp:RequiredFieldValidator>
										</td>
										<td>
											<asp:RegularExpressionValidator ID="RegularExpressionValidator3" ValidationGroup="Insert"
												runat="server" Text="*" ControlToValidate="txtexSpareNumber" ErrorMessage='"Spare number" must input at least 6 non-white-space character (maximum is 35)'
												ValidationExpression="\s*[\S|\s]{6,35}\s*" meta:resourceKey="RegularExpressionValidator3Resource1"
												__designer:wfdid="w25"></asp:RegularExpressionValidator>
										</td>
									</tr>
								</tbody>
							</table>
							<asp:TextBox ID="txtexItemId" runat="server" CssClass="hidden" Text='<%# Eval("ItemId") %>'
								__designer:wfdid="w30"></asp:TextBox>
						</EditItemTemplate>
						<FooterTemplate>
							<table cellspacing="0" cellpadding="0" border="0">
								<tbody>
									<tr>
										<td>
											<asp:TextBox ID="txtexSpareNumber" runat="server" Width="90px" MaxLength="35" meta:resourceKey="txtexSpareNumberResource2"
												__designer:wfdid="w26"></asp:TextBox>
										</td>
										<td>
											<asp:Button ID="btnexFindSpare" OnClick="btnexFindSpare_Click" meta:resourcekey="btnexFindSpareResource2"
												runat="server" Text="..." __designer:wfdid="w27"></asp:Button>
										</td>
										<td>
											<asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="Insert"
												runat="server" Text="*" ControlToValidate="txtexSpareNumber" ErrorMessage="Spare number cannot be blank!"
												meta:resourceKey="RequiredFieldValidator6Resource2" __designer:wfdid="w28"></asp:RequiredFieldValidator>
										</td>
										<td>
											<asp:RegularExpressionValidator ID="RegularExpressionValidator3" ValidationGroup="Insert"
												runat="server" Text="*" ControlToValidate="txtexSpareNumber" ErrorMessage='"Spare number" must input at least 6 non-white-space character (maximum is 35)'
												ValidationExpression="\s*[\S|\s]{6,35}\s*" meta:resourceKey="RegularExpressionValidator3Resource2"
												__designer:wfdid="w29"></asp:RegularExpressionValidator>
										</td>
									</tr>
								</tbody>
							</table>
						</FooterTemplate>
						<HeaderStyle Wrap="False" />
						<ItemTemplate>
							<asp:Literal ID="Literal11" runat="server" Text='<%# Eval("SpareNumber") %>' __designer:wfdid="w21"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Spare name" meta:resourcekey="TemplateFieldResource2">
						<EditItemTemplate>
							<asp:Label ID="Label1" meta:resourcekey="Label1Resource1" runat="server" Text='<%# Eval("SpareName") %>'
								__designer:wfdid="w11"></asp:Label>
						</EditItemTemplate>
						<HeaderStyle Wrap="False" />
						<ItemTemplate>
							<asp:Label ID="Label1" meta:resourcekey="Label1Resource2" runat="server" Text='<%# Bind("SpareName") %>'
								__designer:wfdid="w10"></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Quantity" meta:resourcekey="TemplateFieldResource3">
						<EditItemTemplate>
							<table cellspacing="0" cellpadding="0" border="0">
								<tbody>
									<tr>
										<td>
											<asp:TextBox ID="txtexQuantity" runat="server" Text='<%# Bind("Quantity") %>' Width="63px"
												MaxLength="5" __designer:wfdid="w13" OnDataBinding="txtexQuantity_DataBinding"></asp:TextBox>
										</td>
										<td>
											<asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="Insert"
												runat="server" Text="*" ControlToValidate="txtexQuantity" ErrorMessage="Quantity cannot be blank!"
												meta:resourceKey="RequiredFieldValidator3Resource1" __designer:wfdid="w14"></asp:RequiredFieldValidator>
										</td>
										<td>
											<asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationGroup="Insert"
												runat="server" Text="*" ControlToValidate="txtexQuantity" ErrorMessage="Quantity must be numeric!"
												ValidationExpression="\s*\d*\s*" meta:resourceKey="RegularExpressionValidator1Resource1"
												__designer:wfdid="w15"></asp:RegularExpressionValidator>
										</td>
										<td>
											<asp:RangeValidator ID="RangeValidator1" meta:resourcekey="RangeValidator1Resource1"
												ValidationGroup="Insert" runat="server" __designer:dtid="8725724278030434" ControlToValidate="txtexQuantity"
												ErrorMessage='"Quantity" must between 1 and 999!' __designer:wfdid="w16" Type="Integer"
												MinimumValue="1" MaximumValue="999">*</asp:RangeValidator>
										</td>
									</tr>
								</tbody>
							</table>
						</EditItemTemplate>
						<FooterTemplate>
							<table cellspacing="0" cellpadding="0" border="0">
								<tbody>
									<tr>
										<td>
											<asp:TextBox ID="txtexQuantity" runat="server" Width="63px" MaxLength="5" meta:resourceKey="txtexQuantityResource2"
												__designer:wfdid="w17"></asp:TextBox>
										</td>
										<td>
											<asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="Insert"
												runat="server" Text="*" ControlToValidate="txtexQuantity" ErrorMessage="Quantity cannot be blank!"
												meta:resourceKey="RequiredFieldValidator3Resource2" __designer:wfdid="w18"></asp:RequiredFieldValidator>
										</td>
										<td>
											<asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationGroup="Insert"
												runat="server" Text="*" ControlToValidate="txtexQuantity" ErrorMessage="Quantity must be numeric!"
												ValidationExpression="\s*\d*\s*" meta:resourceKey="RegularExpressionValidator1Resource2"
												__designer:wfdid="w19"></asp:RegularExpressionValidator>
										</td>
										<td>
											<asp:RangeValidator ID="RangeValidator1" meta:resourcekey="RangeValidator1Resource1"
												ValidationGroup="Insert" runat="server" __designer:dtid="8725724278030434" ControlToValidate="txtexQuantity"
												ErrorMessage='"Quantity" must between 1 and 999!' __designer:wfdid="w20" Type="Integer"
												MinimumValue="1" MaximumValue="999">*</asp:RangeValidator>
										</td>
									</tr>
								</tbody>
							</table>
						</FooterTemplate>
						<ItemStyle HorizontalAlign="Center" />
						<HeaderStyle Wrap="False" />
						<ItemTemplate>
							<asp:Literal ID="Literal12" runat="server" Text='<%# Eval("Quantity") %>' __designer:wfdid="w12"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Broken code" meta:resourcekey="TemplateFieldResource4">
						<EditItemTemplate>
							<table cellspacing="0" cellpadding="0" border="0">
								<tbody>
									<tr>
										<td>
											<asp:TextBox runat="server" Width="66px" MaxLength="10" Text='<%# Bind("BrokenCode") %>'
												ID="txtexBrokenCode" meta:resourceKey="txtexBrokenCodeResource1" __designer:wfdid="w30"></asp:TextBox>
										</td>
										<td>
											<asp:Button runat="server" ID="btnexFindBroken" Text="..." __designer:wfdid="w31"
												meta:resourcekey="btnexFindBrokenResource1" OnClick="btnexFindBroken_Click">
											</asp:Button>
										</td>
										<td>
											<asp:RegularExpressionValidator runat="server" ErrorMessage="BrokenCode must be numeric!"
												ControlToValidate="txtexBrokenCode" Text="*" ValidationGroup="Insert" ID="RegularExpressionValidator2"
												ValidationExpression="\s*\d*\s*" meta:resourceKey="RegularExpressionValidator2Resource1"
												__designer:wfdid="w32"></asp:RegularExpressionValidator>
										</td>
										<td>
											<asp:RequiredFieldValidator runat="server" ErrorMessage="BrokenCode cannot be blank!"
												ControlToValidate="txtexBrokenCode" Text="*" ValidationGroup="Insert" ID="RequiredFieldValidator4"
												meta:resourceKey="RequiredFieldValidator4Resource1" __designer:wfdid="w33"></asp:RequiredFieldValidator>
										</td>
									</tr>
								</tbody>
							</table>
						</EditItemTemplate>
						<FooterTemplate>
							<table cellspacing="0" cellpadding="0" border="0">
								<tbody>
									<tr>
										<td>
											<asp:TextBox runat="server" Width="66px" MaxLength="10" Text='<%# Bind("BrokenCode") %>'
												ID="txtexBrokenCode" meta:resourceKey="txtexBrokenCodeResource2" __designer:wfdid="w34"></asp:TextBox>
										</td>
										<td>
											<asp:Button runat="server" ID="btnexFindBroken" Text="..." __designer:wfdid="w35"
												meta:resourcekey="btnexFindBrokenResource2" OnClick="btnexFindBroken_Click">
											</asp:Button>
										</td>
										<td>
											<asp:RegularExpressionValidator runat="server" ErrorMessage="BrokenCode must be numeric!"
												ControlToValidate="txtexBrokenCode" Text="*" ValidationGroup="Insert" ID="RegularExpressionValidator2"
												ValidationExpression="\s*\d*\s*" meta:resourceKey="RegularExpressionValidator2Resource2"
												__designer:wfdid="w36"></asp:RegularExpressionValidator>
										</td>
										<td>
											<asp:RequiredFieldValidator runat="server" ErrorMessage="BrokenCode cannot be blank!"
												ControlToValidate="txtexBrokenCode" Text="*" ValidationGroup="Insert" ID="RequiredFieldValidator4"
												meta:resourceKey="RequiredFieldValidator4Resource2" __designer:wfdid="w37"></asp:RequiredFieldValidator>
										</td>
									</tr>
								</tbody>
							</table>
						</FooterTemplate>
						<ItemStyle HorizontalAlign="Center" />
						<HeaderStyle Wrap="False" />
						<ItemTemplate>
							<asp:Literal runat="server" ID="Literal13" Text='<%# Eval("BrokenCode") %>' __designer:wfdid="w29"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Unit price" meta:resourcekey="TemplateFieldResource5">
						<EditItemTemplate>
							<asp:Label runat="server" Text='<%# Eval("SpareCost") %>' ID="Label6" meta:resourcekey="Label6Resource1"></asp:Label>
						</EditItemTemplate>
						<ItemStyle HorizontalAlign="Right" />
						<HeaderStyle Wrap="False" />
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# Bind("SpareCost") %>' ID="Label6" meta:resourcekey="Label6Resource2"></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Spare amount" meta:resourcekey="TemplateFieldResource6">
						<EditItemTemplate>
							<asp:Label runat="server" Text='<%# Eval("SpareAmount") %>' ID="Label5" meta:resourcekey="Label5Resource1"></asp:Label>
						</EditItemTemplate>
						<ItemStyle HorizontalAlign="Right" />
						<HeaderStyle Wrap="False" />
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# Bind("SpareAmount") %>' ID="Label5" meta:resourcekey="Label5Resource2"></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Serial number" meta:resourcekey="TemplateFieldResource7">
						<EditItemTemplate>
							<table cellspacing="0" cellpadding="0" border="0">
								<tbody>
									<tr>
										<td>
											<asp:TextBox ID="txtexSerialNumber" runat="server" Text='<%# Bind("SerialNumber") %>'
												MaxLength="30" Width="110px" __designer:wfdid="w35" meta:resourceKey="txtexSerialNumberResource1"></asp:TextBox>
										</td>
										<td style="width: 5px">
										</td>
									</tr>
								</tbody>
							</table>
						</EditItemTemplate>
						<FooterTemplate>
							<table cellspacing="0" cellpadding="0" border="0">
								<tbody>
									<tr>
										<td>
											<asp:TextBox ID="txtexSerialNumber" runat="server" Text='<%# Bind("SerialNumber") %>'
												MaxLength="30" Width="110px" __designer:wfdid="w37" meta:resourceKey="txtexSerialNumberResource2"></asp:TextBox>
										</td>
										<td>
										</td>
									</tr>
								</tbody>
							</table>
						</FooterTemplate>
						<HeaderStyle Wrap="False" />
						<ItemTemplate>
							<asp:Literal ID="Literal14" runat="server" Text='<%# Eval("SerialNumber") %>' __designer:wfdid="w34"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="ManPower" meta:resourcekey="TemplateFieldResource8">
						<EditItemTemplate>
							<asp:Label runat="server" Text='<%# Eval("ManPower") %>' ID="Label2" meta:resourcekey="Label2Resource1"></asp:Label>
						</EditItemTemplate>
						<ItemStyle CssClass="valueField" HorizontalAlign="Right" />
						<HeaderStyle Wrap="False" />
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# Bind("ManPower") %>' ID="Label2" meta:resourcekey="Label2Resource2"></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Labour" meta:resourcekey="TemplateFieldResource9">
						<EditItemTemplate>
							<asp:Label runat="server" Text='<%# Eval("Labour") %>' ID="Label3" __designer:wfdid="w266"
								meta:resourcekey="Label3Resource1"></asp:Label>
						</EditItemTemplate>
						<ItemStyle CssClass="valueField" HorizontalAlign="Right" />
						<HeaderStyle Wrap="False" />
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# Bind("Labour") %>' ID="Label3" __designer:wfdid="w265"
								meta:resourcekey="Label3Resource2"></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Fee amount" meta:resourcekey="TemplateFieldResource10">
						<EditItemTemplate>
							<asp:Label runat="server" Text='<%# Eval("FeeAmount") %>' ID="Label4" meta:resourcekey="Label4Resource1"></asp:Label>
						</EditItemTemplate>
						<ItemStyle CssClass="valueField" HorizontalAlign="Right" />
						<HeaderStyle Wrap="False" />
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# Bind("FeeAmount") %>' ID="Label4" meta:resourcekey="Label4Resource2"></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource11">
						<EditItemTemplate>
							<table cellspacing="0" cellpadding="0" border="0">
								<tbody>
									<tr>
										<td>
											<asp:ImageButton ID="ImageButton1" ValidationGroup="Insert" runat="server" Text="Update"
												ImageUrl="~/Images/update.gif" __designer:wfdid="w126" meta:resourceKey="ImageButton1Resource1"
												CommandName="Update" OnDataBinding="ImageButton1_DataBinding"></asp:ImageButton>
										</td>
										<td>
											<asp:ImageButton ID="ImageButton2" runat="server" Text="Cancel" ImageUrl="~/Images/cancel.gif"
												__designer:wfdid="w127" meta:resourceKey="ImageButton2Resource1" CommandName="Cancel"
												CausesValidation="False" OnDataBinding="ImageButton1_DataBinding"></asp:ImageButton>
										</td>
									</tr>
								</tbody>
							</table>
						</EditItemTemplate>
						<FooterTemplate>
							<table cellspacing="0" cellpadding="0" border="0">
								<tbody>
									<tr>
										<td>
											<asp:ImageButton ID="ImageButton3" ValidationGroup="Insert" runat="server" ImageUrl="~/Images/update.gif"
												__designer:wfdid="w128" meta:resourceKey="ImageButton3Resource1" CommandName="InsertSimpleRow">
											</asp:ImageButton>
										</td>
										<td>
											&nbsp;
										</td>
									</tr>
								</tbody>
							</table>
						</FooterTemplate>
						<ItemStyle HorizontalAlign="Center" Wrap="False" />
						<HeaderStyle Wrap="False" />
						<ItemTemplate>
							<table cellspacing="0" cellpadding="0" border="0">
								<tbody>
									<tr>
										<td>
											<asp:ImageButton ID="ImageButton1" runat="server" Text="Edit" ImageUrl="~/Images/Edit.gif"
												__designer:wfdid="w124" meta:resourceKey="ImageButton1Resource2" CommandName="Edit"
												CausesValidation="False" OnDataBinding="ImageButton1_DataBinding"></asp:ImageButton>
										</td>
										<td>
											<asp:ImageButton ID="ImageButton2" runat="server" Text="Delete" ImageUrl="~/Images/Delete.gif"
												__designer:wfdid="w125" meta:resourceKey="ImageButton2Resource2" CommandName="Delete"
												CausesValidation="False" OnDataBinding="ImageButton1_DataBinding"></asp:ImageButton>
										</td>
									</tr>
								</tbody>
							</table>
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
			</cc1:EmptyGridViewEx>
		</td>
		<td>
			&nbsp;&nbsp; &nbsp;
		</td>
	</tr>
	<tr>
		<td class="rightObj">
			&nbsp;<asp:Literal ID="Literal4" runat="server" Text="Total fee amount:" meta:resourcekey="Literal4Resource1"></asp:Literal>
			<asp:TextBox ID="txtexFeeOffer" runat="server" MaxLength="15" meta:resourcekey="txtexFeeOfferResource1"></asp:TextBox>
			<asp:RegularExpressionValidator ID="ssRegularExpressionValidator1" runat="server"
				ControlToValidate="txtexFeeOffer" ErrorMessage="Quantity must be numeric!" meta:resourceKey="RegularExpressionValidator1Resource1F"
				Text="*" ValidationExpression="\s*\d*\s*" ValidationGroup="Save"></asp:RegularExpressionValidator>
		</td>
		<td>
		</td>
	</tr>
	<tr>
		<td colspan="2">
			<table border="0" cellpadding="0" cellspacing="0" runat="server" id="tblWarrantyHint">
				<tr>
					<td>
						&nbsp;<asp:Image ID="Image6" runat="server" ImageUrl="~/Images/s_mustinput.gif" meta:resourcekey="Image6Resource1" />&nbsp;
					</td>
					<td>
						<asp:Literal ID="Literal1" runat="server" Text="May be avoid warranty!" meta:resourcekey="Literal1Resource1"></asp:Literal>
					</td>
				</tr>
				<tr>
					<td>
					</td>
					<td>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td class="centerObj">
			<asp:Button ID="btnSave" runat="server" OnClick="Button2_Click" Text="Save" ValidationGroup="Save"
				Width="67px" meta:resourcekey="Button2Resource3" />&nbsp;
			<asp:Button ID="btnCancel" runat="server" OnClick="Button3_Click" Text="Cancel" Width="72px"
				meta:resourcekey="Button3Resource2" />
		</td>
		<td>
		</td>
	</tr>
</table>
