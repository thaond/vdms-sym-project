<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchPartUI.ascx.cs"
	Inherits="Part_Inventory_SearchPartUI" %>
<script language="javascript" type="text/javascript">
    function showSearch(link, option) {
        var s = "/part/Inventory/SearchPart.aspx?";
        s = s + "code=" + $('#<%= this.tb1.ClientID %>').val();
        s = s + "&name=" + $('#<%= this.tb2.ClientID %>').val();
        s = s + "&engno=" + $('#<%= this.tb3.ClientID %>').val();
        s = s + "&model=" + $('#<%= this.ddl1.ClientID %>').val();
        s = s + option + "&TB_iframe=true&height=500&width=420";
        link.href = s;
    }
</script>
<table>
	<tr>
		<td>
			<asp:Literal ID="Literal1" runat="server" Text="Part Code:" meta:resourcekey="Literal1Resource1"></asp:Literal>
		</td>
		<td>
			<asp:TextBox ID="tb1" runat="server" Columns="15" meta:resourcekey="tb1Resource1"></asp:TextBox>
		</td>
		<td>
			<asp:Literal ID="Literal2" runat="server" Text="Part Name:" meta:resourcekey="Literal2Resource1"></asp:Literal>
		</td>
		<td>
			<asp:TextBox ID="tb2" runat="server" Columns="15" meta:resourcekey="tb2Resource1"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td>
			<asp:Literal ID="Literal3" runat="server" Text="Engine No:" meta:resourcekey="Literal3Resource1"></asp:Literal>
		</td>
		<td>
			<asp:TextBox ID="tb3" runat="server" Columns="15" meta:resourcekey="tb3Resource1"></asp:TextBox>
		</td>
		<td>
			<asp:Literal ID="Literal4" runat="server" Text="Model:" meta:resourcekey="Literal4Resource1"></asp:Literal>
		</td>
		<td>
			<asp:UpdatePanel ID="up3" runat="server">
				<ContentTemplate>
					<asp:DropDownList ID="ddl1" runat="server" Width="115px" DataSourceID="odsModel"
						AppendDataBoundItems="True" DataTextField="descript" DataValueField="model" OnDataBound="ddl3_DataBound"
						meta:resourcekey="ddl1Resource1">
						<asp:ListItem meta:resourcekey="ListItemResource1"></asp:ListItem>
					</asp:DropDownList>
					<asp:Button ID="cmdUpdateModel" runat="server" Text="Get Model" OnClick="cmdUpdateModel_Click"
						meta:resourcekey="cmdUpdateModelResource1" />
				</ContentTemplate>
			</asp:UpdatePanel>
			<asp:ObjectDataSource ID="odsModel" runat="server" TypeName="VDMS.II.PartManagement.PartDAO"
				SelectMethod="GetModelList">
				<SelectParameters>
					<asp:ControlParameter ControlID="tb1" Name="partCode" PropertyName="Text" />
					<asp:ControlParameter ControlID="tb3" Name="engineNo" PropertyName="Text" />
				</SelectParameters>
			</asp:ObjectDataSource>
		</td>
	</tr>
	<tr>
		<td>
		</td>
		<td colspan="3">
			<asp:HyperLink ID="h1" runat="server" Text="Search Part" class="thickbox" title="Search Part"
				NavigateUrl="#" meta:resourcekey="h1Resource1"></asp:HyperLink>
		</td>
	</tr>
</table>
