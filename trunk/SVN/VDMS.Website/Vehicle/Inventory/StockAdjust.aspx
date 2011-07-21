<%@ Page Title="" Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="StockAdjust.aspx.cs" Inherits="Vehicle_Inventory_StockAdjust" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">

    <script type="text/javascript">
        function update(str) {
            tb_remove();
            __doPostBack('Updt', '');
        }

        function showCustomerInput() {
            window.showModalDialog("CusInfInput.aspx", null, "status:false;dialogWidth:750px;dialogHeight:500px")
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <table>
            <tr>
                <td colspan="2">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Adjust"
                        DisplayMode="List" meta:resourcekey="ValidationSummary1Resource1" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblMessage" runat="server" Text="Label" Visible="False" 
                        meta:resourcekey="lblMessageResource1"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="From branch" 
                        meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:WarehouseList runat="server" ID="ddlFromBranch" 
                        DontAutoUseCurrentSealer="False" MergeCode="True" 
                        meta:resourcekey="ddlFromBranchResource1" ShowEmptyItem="False" 
                        ShowSelectAllItem="False" UseVIdAsValue="False" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="To branch" 
                        meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <vdms:WarehouseList runat="server" ID="ddlToBranch" 
                        DontAutoUseCurrentSealer="False" MergeCode="True" 
                        meta:resourcekey="ddlToBranchResource1" ShowEmptyItem="False" 
                        ShowSelectAllItem="False" UseVIdAsValue="False" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="Adjusted date" 
                        meta:resourcekey="Literal3Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtAdjustDate" runat="server" 
                        meta:resourcekey="txtAdjustDateResource1"></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99/99/9999"
                        MaskType="Date" TargetControlID="txtAdjustDate"
                        Enabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                        CultureDateFormat="" CultureDatePlaceholder="" 
                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                        CultureTimePlaceholder="">
                    </ajaxToolkit:MaskedEditExtender>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="ImageButton5"
                        TargetControlID="txtAdjustDate" Enabled="True">
                    </ajaxToolkit:CalendarExtender>
                    <asp:ImageButton ID="ImageButton5" runat="server" OnClientClick="return false;" 
                        SkinID="CalendarImageButton" meta:resourcekey="ImageButton5Resource1" />
                    <asp:RangeValidator ID="rvAdjustDate" runat="server" ControlToValidate="txtAdjustDate"
                        ErrorMessage='Dữ liệu "Ngày cần thu" không đúng với định dạng ngày!' SetFocusOnError="True"
                        Type="Date" ValidationGroup="Adjust" 
                        meta:resourcekey="rvAdjustDateResource1">*</asp:RangeValidator>
                    <asp:RequiredFieldValidator ID="rfvAdjustDate" runat="server" ErrorMessage="Adjusted date required"
                        ControlToValidate="txtAdjustDate" ValidationGroup="Adjust" 
                        meta:resourcekey="rfvAdjustDateResource1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </div>
    <div class="form">
        <asp:Literal ID="Literal4" runat="server" Text="Engine number" 
            meta:resourcekey="Literal4Resource1"></asp:Literal>
        <asp:TextBox ID="txtEngineNo" runat="server" 
            meta:resourcekey="txtEngineNoResource1"></asp:TextBox>
        <asp:Button ID="btnAddVehicle" runat="server" Text="&nbsp;+&nbsp;" 
            OnClick="btnAddVehicle_Click" meta:resourcekey="btnAddVehicleResource1" />
        <asp:HyperLink ID="lnkSearchVehicles" runat="server" class="thickbox" 
            title="Search Vehicle" meta:resourcekey="lnkSearchVehiclesResource1">Search vehicles</asp:HyperLink>
        <div>
            &nbsp;</div>
        <div class="grid">
            <vdms:PageGridView ID="gv" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                EmptyDataText="No vehicle." CssClass="GridView" 
                DataSourceID="odsSessionVehicles" Width="100%" 
                OnSelectedIndexChanging="gv_SelectedIndexChanging" 
                meta:resourcekey="gvResource1">
                <Columns>
                    <asp:TemplateField HeaderText="No" meta:resourcekey="TemplateFieldResource1">
                        <ItemTemplate>
                            <%#Container.DisplayIndex + 1%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="EngineNumber" HeaderText="Engine no" 
                        meta:resourcekey="BoundFieldResource1" />
                    <asp:BoundField DataField="ItemName" HeaderText="Name" 
                        meta:resourcekey="BoundFieldResource2" />
                    <asp:BoundField DataField="ColorName" HeaderText="Color" 
                        meta:resourcekey="BoundFieldResource3" />
                    <asp:BoundField DataField="Branch" HeaderText="Branch" 
                        meta:resourcekey="BoundFieldResource4" />
                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/Images/Delete.gif" 
                        ShowSelectButton="true" meta:resourcekey="CommandFieldResource1" />
                </Columns>
            </vdms:PageGridView>
        </div>
        <div>
            &nbsp;</div>
        <asp:Button ID="btnCommit" runat="server" Text="Adjust" 
            OnClick="btnCommit_Click" meta:resourcekey="btnCommitResource1" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" 
            meta:resourcekey="btnClearResource1" />
    </div>
    <asp:ObjectDataSource ID="odsSessionVehicles" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="ListSaleSessionalVehicles" TypeName="VDMS.I.Vehicle.VehicleDAO"
        EnablePaging="True" SelectCountMethod="CountSaleSessionalVehicles">
        <SelectParameters>
            <asp:Parameter DefaultValue="stockadjust_key" Name="key" Type="String" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
