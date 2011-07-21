<%@ Page Title="" Language="C#" MasterPageFile="~/MP/Popup.master" AutoEventWireup="true"
    CodeFile="SearchVehicle.aspx.cs" Inherits="Vehicle_Sale_SearchVehicle" Theme="Thickbox" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <asp:Panel ID="pn" runat="server" meta:resourcekey="pnResource1">
        <asp:UpdatePanel ID="udtPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form">
                    <div class="grid" style="border: 0px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Literal ID="Literal3" runat="server" meta:resourcekey="Literal3Resource1" 
                                        Text="Warehouse: "></asp:Literal>
                                </td>
                                <td>
                                    <vdms:WarehouseList ID="ddlWarehouse" runat="server" AutoPostBack="True" DealerCode='<%# VDMS.Helper.UserHelper.DealerCode %>'
                                        ShowSelectAllItem="True" Type="V" DontAutoUseCurrentSealer="False" 
                                        MergeCode="True" meta:resourcekey="ddlWarehouseResource1" ShowEmptyItem="False" 
                                        UseVIdAsValue="False">
                                    </vdms:WarehouseList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="Literal1" runat="server" meta:resourcekey="Literal1Resource1" 
                                        Text="Model"></asp:Literal>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlModel" runat="server" DataSourceID="odsVehicleTypes" DataTextField="ItemName"
                                        DataValueField="ItemType" OnDataBound="ddlModel_DataBound" 
                                        AutoPostBack="True" meta:resourcekey="ddlModelResource1">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="Literal8" runat="server" meta:resourcekey="Literal8Resource1" 
                                        Text="Color"></asp:Literal>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlColor" runat="server" AutoPostBack="True" DataSourceID="odsColors"
                                        DataTextField="ColorName" DataValueField="ColorCode" 
                                        meta:resourcekey="ddlColorResource1">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="Literal2" runat="server" meta:resourcekey="Literal2Resource1" 
                                        Text="Engine number: "></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEngineNo" runat="server" 
                                        meta:resourcekey="txtEngineNoResource1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Button ID="Button1" runat="server" Text="Search" OnClick="Button1_Click" 
                                        meta:resourcekey="Button1Resource1" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="udtPanel"
                        DisplayAfter="0" DynamicLayout="False">
                        <ProgressTemplate>
                            <img src="../../Images/Spinner.gif" alt="" /><asp:Literal ID="Literal55p3" Text="Updating..."
                                runat="server" meta:resourcekey="Literal55p3Resource1" ></asp:Literal>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <div class="grid">
                        <vdms:PageGridView ID="gv" runat="server" DataSourceID="odsVehicles" OnPageIndexChanging="gv_PageIndexChanging"
                            AutoGenerateColumns="False" AllowPaging="True" OnPreRender="gv_PreRender" 
                            EmptyDataText="No vehicle found." meta:resourcekey="gvResource1">
                            <Columns>
                                <asp:TemplateField meta:resourcekey="TemplateFieldResource1" >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" 
                                            meta:resourcekey="CheckBox1Resource1" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Engine No" DataField="EngineNumber" 
                                    meta:resourcekey="BoundFieldResource1" />
                                <asp:BoundField HeaderText="Type" DataField="ItemName" 
                                    meta:resourcekey="BoundFieldResource2" />
                                <asp:BoundField HeaderText="Color" DataField="ColorName" 
                                    meta:resourcekey="BoundFieldResource3" />
                                <asp:BoundField HeaderText="Branch" DataField="BranchCode" 
                                    meta:resourcekey="BoundFieldResource4" />
                            </Columns>
                        </vdms:PageGridView>
                    </div>
                    <div class="footer">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" 
                            meta:resourcekey="btnSaveResource1" />
                    </div>
                </div>
                <asp:ObjectDataSource ID="odsVehicles" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="FindVehicles" TypeName="VDMS.I.Vehicle.VehicleDAO" EnablePaging="True"
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
                <asp:ObjectDataSource ID="odsVehicleTypes" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="ListVehicleTypes" TypeName="VDMS.I.Vehicle.VehicleDAO">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlWarehouse" Name="branch" PropertyName="SelectedValue"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="odsColors" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="ListColors" TypeName="VDMS.I.Vehicle.VehicleDAO">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlWarehouse" Name="branch" PropertyName="SelectedValue"
                            Type="String" />
                        <asp:ControlParameter ControlID="ddlModel" Name="type" PropertyName="SelectedValue"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
