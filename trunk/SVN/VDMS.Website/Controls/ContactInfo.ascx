<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ContactInfo.ascx.cs" Inherits="Controls_ContactInfo" %>
<table width="100%">
	<tr>
		<td style="width: 25%;">
			<asp:Image ID="image1" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image1Resource1" />
			<asp:Localize ID="l1" runat="server" Text="Fullname:" meta:resourcekey="l1Resource1"></asp:Localize>
		</td>
		<td>
			<asp:MultiView ID="mv1" runat="server" ActiveViewIndex="0">
				<asp:View ID="view1" runat="server">
					<asp:TextBox ID="tb1" runat="server" MaxLength="50" Width="180px" AutoCompleteType="DisplayName"
						meta:resourcekey="tb1Resource1"></asp:TextBox>
				</asp:View>
				<asp:View ID="view2" runat="server">
					<asp:Label ID="lb1" runat="server" Font-Bold="true"></asp:Label>
				</asp:View>
			</asp:MultiView>
		</td>
	</tr>
	<tr>
		<td>
			<asp:Image ID="image2" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image2Resource1" />
			<asp:Localize ID="l2" runat="server" Text="Address:" meta:resourcekey="l2Resource1"></asp:Localize>
		</td>
		<td>
			<asp:MultiView ID="mv2" runat="server" ActiveViewIndex="0">
				<asp:View ID="view3" runat="server">
					<asp:TextBox ID="tb2" runat="server" MaxLength="255" Width="180px" meta:resourcekey="tb2Resource1"></asp:TextBox>
				</asp:View>
				<asp:View ID="view4" runat="server">
					<asp:Label ID="lb2" runat="server" Font-Bold="true"></asp:Label>
				</asp:View>
			</asp:MultiView>
		</td>
	</tr>
	<tr>
		<td>
			<asp:Image ID="image3" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image3Resource1" />
			<asp:Localize ID="l3" runat="server" Text="Phone:" meta:resourcekey="l3Resource1"></asp:Localize>
		</td>
		<td>
			<asp:MultiView ID="mv3" runat="server" ActiveViewIndex="0">
				<asp:View ID="view5" runat="server">
					<asp:TextBox ID="tb3" runat="server" MaxLength="20" Width="180px" AutoCompleteType="HomePhone"
						meta:resourcekey="tb3Resource1"></asp:TextBox>
				</asp:View>
				<asp:View ID="view6" runat="server">
					<asp:Label ID="lb3" runat="server" Font-Bold="true"></asp:Label>
				</asp:View>
			</asp:MultiView>
		</td>
	</tr>
	<tr>
		<td>
			<asp:Image ID="image6" runat="server" SkinID="RequireFieldNon" />
			<asp:Localize ID="Localize1" runat="server" Text="Fax:" ></asp:Localize>
		</td>
		<td>
			<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
				<asp:View ID="view11" runat="server">
					<asp:TextBox ID="txtFax" runat="server" MaxLength="20" Width="180px" AutoCompleteType="BusinessFax"></asp:TextBox>
				</asp:View>
				<asp:View ID="view12" runat="server">
					<asp:Label ID="lbFax" runat="server" Font-Bold="true"></asp:Label>
				</asp:View>
			</asp:MultiView>
		</td>
	</tr>
	<tr>
		<td>
			<asp:Image ID="image7" runat="server" SkinID="RequireFieldNon" />
			<asp:Localize ID="Localize2" runat="server" Text="Mobile:"></asp:Localize>
		</td>
		<td>
			<asp:MultiView ID="MultiView2" runat="server" ActiveViewIndex="0">
				<asp:View ID="view13" runat="server">
					<asp:TextBox ID="txtMobile" runat="server" MaxLength="20" Width="180px" ></asp:TextBox>
				</asp:View>
				<asp:View ID="view14" runat="server">
					<asp:Label ID="lbMobile" runat="server" Font-Bold="true"></asp:Label>
				</asp:View>
			</asp:MultiView>
		</td>
	</tr>
	<tr>
		<td>
			<asp:Image ID="image4" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image4Resource1" />
			<asp:Localize ID="l4" runat="server" Text="Email:" meta:resourcekey="l4Resource1"></asp:Localize>
		</td>
		<td>
			<asp:MultiView ID="mv4" runat="server" ActiveViewIndex="0">
				<asp:View ID="view7" runat="server">
					<asp:TextBox ID="tb4" runat="server" MaxLength="255" Width="180px" AutoCompleteType="Email"
						meta:resourcekey="tb4Resource1"></asp:TextBox>
					<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="Save"
						ErrorMessage="Email is in invalid format!" ControlToValidate="tb4" meta:resourcekey="RegularExpressionValidator1Resource1"
						ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
				</asp:View>
				<asp:View ID="view8" runat="server">
					<asp:Label ID="lb4" runat="server" Font-Bold="true"></asp:Label>
				</asp:View>
			</asp:MultiView>
		</td>
	</tr>
	<tr>
		<td>
			<asp:Image ID="image5" runat="server" SkinID="RequireFieldNon" meta:resourcekey="image5Resource1" />
			<asp:Localize ID="l5" runat="server" Text="Additional Info:" meta:resourcekey="l5Resource1"></asp:Localize>
		</td>
		<td>
			<asp:MultiView ID="mv5" runat="server" ActiveViewIndex="0">
				<asp:View ID="view9" runat="server">
					<asp:TextBox ID="tb5" runat="server" MaxLength="255" Width="180px" meta:resourcekey="tb5Resource1"></asp:TextBox>
				</asp:View>
				<asp:View ID="view10" runat="server">
					<asp:Label ID="lb5" runat="server" Font-Bold="true"></asp:Label>
				</asp:View>
			</asp:MultiView>
		</td>
	</tr>
</table>
