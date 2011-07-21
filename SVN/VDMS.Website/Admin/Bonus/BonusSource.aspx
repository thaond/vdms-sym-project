<%@ Page Title="" Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="BonusSource.aspx.cs" Inherits="Admin_Bonus_BonusSource" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
        ValidationGroup="New" meta:resourcekey="ValidationSummary1Resource1" />
    <div class="grid">
        <vdms:PageGridView ID="gv" runat="server" AutoGenerateColumns="False" DataKeyNames="BonusSourceId"
            DataSourceID="odsBS">
            <Columns>
                <asp:CommandField ButtonType="Image" CancelImageUrl="~/Images/cancel.gif" EditImageUrl="~/Images/Edit.gif"
                    ShowEditButton="True" UpdateImageUrl="~/Images/update.gif" />
                <asp:BoundField DataField="BonusSourceName" HeaderText="Name" SortExpression="BonusSourceName" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Images/Delete.gif" ShowDeleteButton="True" />
            </Columns>
        </vdms:PageGridView>
        <asp:ObjectDataSource ID="odsBS" runat="server" DataObjectTypeName="VDMS.Bonus.Entity.BonusSource"
            DeleteMethod="Delete" OldValuesParameterFormatString="original_{0}" SelectMethod="GetAll"
            TypeName="VDMS.II.BonusSystem.BonusSourceDAO" UpdateMethod="Update">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int64" />
            </DeleteParameters>
        </asp:ObjectDataSource>
        <br />
    </div>
    <div class="form">
        <table>
            <tr>
                <td>
                    <asp:Image ID="Image1" runat="server" SkinID="RequireField" 
                        meta:resourcekey="Image1Resource1" />
                    <asp:Literal ID="Literal1" runat="server" Text="Name:" 
                        meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtName" MaxLength="256" runat="server" 
                        meta:resourcekey="txtNameResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="txtName" ErrorMessage="Bonus source name cannot be blank!" 
                        ValidationGroup="New" meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="Image2" runat="server" SkinID="RequireFieldNon" 
                        meta:resourcekey="Image2Resource1" />
                    <asp:Literal ID="Literal2" runat="server" Text="Description:" 
                        meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtDesc" MaxLength="512" runat="server" 
                        meta:resourcekey="txtDescResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnAdd" runat="server" Text="Add new" onclick="btnAdd_Click" 
                        ValidationGroup="New" meta:resourcekey="btnAddResource1" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
