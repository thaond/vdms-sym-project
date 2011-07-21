<%@ Page Title="" Language="C#" MasterPageFile="~/MP/Popup.master" AutoEventWireup="true"
    CodeFile="SearchVehicle.aspx.cs" Inherits="Vehicle_Sale_SearchVehicle" Theme="Thickbox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="grid" style="border: 0px">
        <table>
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server">Model</asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlModel" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Literal ID="Literal8" runat="server">Color</asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlColor" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server">Engine number: </asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtEngineNo" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="Literal3" runat="server">Warehouse: </asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlWarehouse" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button ID="Button1" runat="server" Text="Search" OnClick="Button1_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div class="grid">
        <vdms:PageGridView ID="gv" runat="server" DataSourceID="odsVehicles" OnPageIndexChanging="gv_PageIndexChanging"
            OnPreRender="gv_PreRender">
            <Columns>
                <asp:TemplateField meta:resourcekey="TemplateFieldResource2">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" meta:resourcekey="CheckBox1Resource1" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Engine No" DataField="EngineNumber" />
                <asp:BoundField HeaderText="Type" DataField="ItemType" />
                <asp:BoundField HeaderText="Color" DataField="ColorCode" />
            </Columns>
        </vdms:PageGridView>
    </div>
    <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" />
    <asp:Button ID="btnClear" runat="server" Text="Clear" 
        onclick="btnClear_Click" />
    
    <asp:ObjectDataSource ID="odsVehicles" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="FindVehicles" TypeName="VDMS.I.Vehicle.VehicleDAO" EnablePaging="true"
        SelectCountMethod="CountFoundVehicles">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlModel" Name="type" PropertyName="SelectedValue"
                Type="String" />
            <asp:ControlParameter ControlID="ddlColor" Name="color" PropertyName="SelectedValue"
                Type="String" />
            <asp:ControlParameter ControlID="txtEngineNo" Name="engineNumber" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="ddlWarehouse" Name="warehouseId" PropertyName="SelectedValue"
                Type="String" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
