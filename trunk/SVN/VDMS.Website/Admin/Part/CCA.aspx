<%@ Page Title="Cycle Counting Adjust" Language="C#" MasterPageFile="~/MP/MasterPage.master"
    AutoEventWireup="true" CodeFile="CCA.aspx.cs" Inherits="Admin_Part_CCA" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register TagPrefix="cc1" Namespace="VDMS.II.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">

    <script type="text/javascript">
        function updated() {
            //  close the popup
            tb_remove();

            //  refresh the update panel so we can view the changes
            $('#<%= this.btnPartInserted.ClientID %>').click();
        }
        function showSearch(link, wh) {
            var s = "../../part/inventory/SearchPart.aspx?";
            s = s + "code=" + $('#<%= this.txtPartCode.ClientID %>').val();
            s = s + "&name=" + $('#<%= this.txtPartName.ClientID %>').val();
            s = s + "&engno=" + $('#<%= this.txtEngineNo.ClientID %>').val();
            s = s + "&target=CC";
            s = s + "&at=CC";
            s = s + "&wh=" + $('#<%= this.ddlWarehouse.ClientID %>').val(); ;
            s = s + "&model=" + $('#<%= this.ddl3.ClientID %>').val();
            s = s + "&TB_iframe=true&height=320&width=420";
            link.href = s;
        }
       
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <asp:UpdatePanel runat="server" ID="udpMsg">
        <ContentTemplate>
            <asp:Literal ID="litExcelError" runat="server" Visible="False" Text="Cannot do cycle count automatically. The excel file structure may be invalid. Please contact with VMEP IT for detail."
                meta:resourcekey="litExcelErrorResource1"></asp:Literal>
            <div runat="server" id="msg">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="form" style="width: 500px">
        <table width="100%">
            <tr>
                <td>
                    <asp:Localize ID="litCategory0" runat="server" Text="Warehouse:" meta:resourcekey="litCategory0Resource1"></asp:Localize>
                </td>
                <td>
                    <vdms:WarehouseList ID="ddlWarehouse" runat="server" DontAutoUseCurrentSealer="False"
                        meta:resourcekey="ddlWarehouseResource1" ShowEmptyItem="False" ShowSelectAllItem="False"
                        UseVIdAsValue="False">
                    </vdms:WarehouseList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Localize ID="Localize1" runat="server" Text="Comment:" meta:resourcekey="Localize1Resource1"></asp:Localize>
                </td>
                <td>
                    <asp:TextBox MaxLength="200" ID="txtSessionComment" runat="server" Width="100%" meta:resourcekey="txtSessionCommentResource1"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <ajaxToolkit:TabContainer ID="t" runat="server" CssClass="ajax__tab_technorati-theme"
        ActiveTabIndex="0" meta:resourcekey="tResource2">
        <ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Manual Cycle counting"
            meta:resourcekey="TabPanel3Resource1">
            <HeaderTemplate>
                <asp:Literal ID="Literal10" runat="server" Text="Cycle count data" meta:resourcekey="Literal10Resource1"></asp:Literal>
            </HeaderTemplate>
            <ContentTemplate>
                <div class="form">
                    <table>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal8" runat="server" Text="Status:" meta:resourcekey="Literal8Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" meta:resourcekey="ddlStatusResource1">
                                    <asp:ListItem meta:resourcekey="ListItemResource1" Text="All"></asp:ListItem>
                                    <asp:ListItem Value="N" meta:resourcekey="ListItemResource2" Text="New"></asp:ListItem>
                                    <asp:ListItem Value="C" meta:resourcekey="ListItemResource3" Text="Confirmed"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="txtLoadHeader" runat="server" Text="View" OnClick="txtLoadHeader_Click"
                                    meta:resourcekey="txtLoadHeaderResource1" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div class="grid">
                    <table width="100%" class="datatable">
                        <tr>
                            <td class="right">
                                <asp:Literal ID="Literal9" runat="server" Text="Confirm Cycle count on month:" meta:resourcekey="Literal9Resource1"></asp:Literal>
                                <asp:DropDownList ID="ddlMonth" runat="server" meta:resourcekey="ddlMonthResource1">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <vdms:PageGridView ID="gvH" runat="server" DataKeyNames="CycleCountHeaderId" AutoGenerateColumns="False"
                        DataSourceID="odsH" OnRowEditing="gvH_RowEditing" OnSelectedIndexChanging="gvH_SelectedIndexChanging"
                        OnRowDeleting="gvH_RowDeleting" meta:resourcekey="gvHResource1">
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" SelectText="View" meta:resourcekey="CommandFieldResource1" />
                            <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource1">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" Visible='<%# (string)Eval("Status") == "N" %>'
                                        CommandName="Edit" Text="Edit" meta:resourcekey="LinkButton1Resource1"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource2">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" Visible='<%# ((string)Eval("Status") != "C") && ((string)Eval("Status") != "D") %>'
                                        CommandName="Delete" Text="Delete" meta:resourcekey="LinkButton2Resource1"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CreatedTime" HeaderText="Created time" SortExpression="CreatedTime"
                                meta:resourcekey="BoundFieldResource1" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="Created by" SortExpression="CreatedBy"
                                meta:resourcekey="BoundFieldResource2" />
                            <asp:BoundField DataField="LastEditedDate" HeaderText="LastEdited date" SortExpression="LastEditedDate"
                                meta:resourcekey="BoundFieldResource3" />
                            <asp:TemplateField HeaderText="Confirmed date" SortExpression="CycleDate" meta:resourcekey="TemplateFieldResource3">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# EvalDateTime(Eval("CycleDate")) %>' meta:resourcekey="Label1Resource1"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" meta:resourcekey="BoundFieldResource4">
                                <ItemStyle CssClass="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TransactionComment" HeaderText="Comment" SortExpression="TransactionComment"
                                meta:resourcekey="BoundFieldResource5" />
                            <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource4">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbConfirm" runat="server" CausesValidation="False" Visible='<%# (string)Eval("Status") == "N" %>'
                                        CommandName="Confirm" HID='<%# Eval("CycleCountHeaderId") %>' Text="Confirm"
                                        OnClick="lnkbConfirm_Click" meta:resourcekey="lnkbConfirmResource1"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </vdms:PageGridView>
                    <asp:ObjectDataSource ID="odsH" runat="server" EnablePaging="True" SelectMethod="FindCycleCountHeaders"
                        TypeName="VDMS.II.PartManagement.CycleCountDAO" SelectCountMethod="CountCycleCountHeaders"
                        DeleteMethod="DeleteHeader">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlStatus" Name="status" PropertyName="SelectedValue"
                                Type="String" />
                            <asp:ControlParameter ControlID="ddlWarehouse" Name="wid" PropertyName="SelectedValue"
                                Type="Int64" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <vdms:PageGridView ID="gvD" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        DataSourceID="odsD" PageSize="30" OnDataBound="gvD_DataBound" meta:resourcekey="gvDResource1">
                        <Columns>
                            <asp:BoundField DataField="PartCode" HeaderText="Part code" SortExpression="PartCode"
                                meta:resourcekey="BoundFieldResource6" />
                            <asp:BoundField DataField="PartType" HeaderText="Part type" SortExpression="PartType"
                                meta:resourcekey="BoundFieldResource7" />
                            <asp:TemplateField HeaderText="Current stock quantity" SortExpression="Quantity"
                                meta:resourcekey="TemplateFieldResource5">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# EvalStockQty(Eval("OriginalObj")) %>'
                                        meta:resourcekey="Label1Resource2"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="number" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Before stock quantity" SortExpression="Quantity" meta:resourcekey="TemplateFieldResource6">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# EvalBeforeStockQty(Eval("OriginalObj"), (int)Eval("CycleQuantity")) %>'
                                        meta:resourcekey="Label1Resource3"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="number" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="CycleQuantity" HeaderText="Cycle count quantity" SortExpression="Quantity"
                                meta:resourcekey="BoundFieldResource8">
                                <ItemStyle CssClass="number" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="Comment"
                                meta:resourcekey="BoundFieldResource9" />
                        </Columns>
                    </vdms:PageGridView>
                    <asp:ObjectDataSource ID="odsD" runat="server" SelectCountMethod="CountCycleCountDetails"
                        SelectMethod="FindCycleCountDetails" EnablePaging="True" TypeName="VDMS.II.PartManagement.CycleCountDAO">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="gvH" Name="hid" PropertyName="SelectedValue" Type="Int64" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Manual Cycle counting"
            meta:resourcekey="TabPanel1Resource1">
            <HeaderTemplate>
                <asp:Literal ID="Literal11" runat="server" Text="Manual Cycle counting" meta:resourcekey="Literal11Resource1"></asp:Literal>
            </HeaderTemplate>
            <ContentTemplate>
                <div class="form">
                    <table>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal12" runat="server" Text="Part Code:" meta:resourcekey="Literal12Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPartCode" runat="server" Columns="15" meta:resourcekey="txtPartCodeResource1"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="Literal13" runat="server" Text="Part Name:" meta:resourcekey="Literal13Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPartName" runat="server" Columns="15" meta:resourcekey="txtPartNameResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal14" runat="server" Text="Engine No:" meta:resourcekey="Literal14Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEngineNo" runat="server" Columns="15" meta:resourcekey="txtEngineNoResource1"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="Literal15" runat="server" Text="Model:" meta:resourcekey="Literal15Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl3" runat="server" Width="115px" DataSourceID="odsModel"
                                    AppendDataBoundItems="True" DataTextField="model" meta:resourcekey="ddl3Resource1">
                                    <asp:ListItem meta:resourcekey="ListItemResource4"></asp:ListItem>
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
                    <asp:UpdatePanel ID="udpPart" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="4" border="0">
                                <tr>
                                    <td>
                                        <asp:Literal ID="Literal4" runat="server" Text="Excel mode:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="cmdAddRow" runat="server" OnClick="cmdAddRow_Click" Text="Add"
                                            meta:resourcekey="cmdAddRowResource1"></asp:LinkButton>
                                        <asp:DropDownList ID="ddlRowCount" runat="server" meta:resourcekey="ddlRowCountResource1">
                                            <asp:ListItem Text="5" meta:resourcekey="ListItemResource5"></asp:ListItem>
                                            <asp:ListItem Text="10" meta:resourcekey="ListItemResource6"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Literal ID="Literal5" runat="server" Text="Rows. " meta:resourcekey="Literal5Resource1"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal7" runat="server" Text="Rows/table:" meta:resourcekey="Literal7Resource1"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRows" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRows_SelectedIndexChanged"
                                            meta:resourcekey="ddlRowsResource1">
                                            <asp:ListItem Text="5" meta:resourcekey="ListItemResource7"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="10" meta:resourcekey="ListItemResource8"></asp:ListItem>
                                            <asp:ListItem Text="20" meta:resourcekey="ListItemResource9"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div class="grid">
                                <vdms:PageGridView PageSize="25" ID="gv" DataSourceID="odsPartList" runat="server"
                                    OnRowDataBound="gv_RowDataBound" AutoGenerateColumns="False" AllowPaging="True"
                                    DataKeyNames="Line" OnRowDeleting="gv_RowDeleting" meta:resourcekey="gvResource1">
                                    <Columns>
                                        <asp:BoundField HeaderText="Line" DataField="Line" meta:resourcekey="BoundFieldResource10">
                                            <ItemStyle CssClass="center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Part No" meta:resourcekey="TemplateFieldResource7">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPartCode" runat="server" Text='<%# Bind("PartCode") %>' OnTextChanged="UpdateRow"
                                                    meta:resourcekey="txtPartCodeResource2"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part Name" meta:resourcekey="TemplateFieldResource8">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("PartName") %>' meta:resourcekey="Label1Resource4"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Part type" DataField="PartType" meta:resourcekey="BoundFieldResource11">
                                            <ItemStyle CssClass="center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Safety quantity" meta:resourcekey="TemplateFieldResource9">
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("SafetyQuantity") %>' meta:resourcekey="Label3Resource1"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="number" />
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="On Hand" DataField="Quantity" meta:resourcekey="BoundFieldResource12">
                                            <ItemStyle CssClass="number" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Count Quantity" meta:resourcekey="TemplateFieldResource10">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCountQty" runat="server" SkinID="InGrid" Text='<%# Eval("CycleQuantity") %>'
                                                    OnTextChanged="UpdateRow" CssClass="number" meta:resourcekey="txtCountQtyResource1"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="txtCountQty_FilteredTextBoxExtender" runat="server"
                                                    TargetControlID="txtCountQty" FilterType="Numbers" Enabled="True">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="number" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Difference" meta:resourcekey="TemplateFieldResource11">
                                            <ItemTemplate>
                                                <asp:Label ID="Label6" runat="server" CssClass='<%# Eval("WarnClass") %>' Text='<%# Eval("Difference") %>'
                                                    meta:resourcekey="Label6Resource1"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="number" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Comment" meta:resourcekey="TemplateFieldResource12">
                                            <ItemTemplate>
                                                <asp:TextBox OnTextChanged="UpdateRow" Text='<%# Eval("Comment") %>' Width="150px"
                                                    ID="txtComment" runat="server" SkinID="InGrid" meta:resourcekey="txtCommentResource1"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField ShowDeleteButton="True" meta:resourcekey="CommandFieldResource2" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <asp:Literal ID="Literal12" runat="server" Text="There aren't any rows." meta:resourcekey="Literal12Resource2"></asp:Literal>
                                    </EmptyDataTemplate>
                                </vdms:PageGridView>
                            </div>
                            <asp:Button ID="btnPartInserted" runat="server" Text="Refresh" OnClick="btnPartInserted_Click"
                                meta:resourcekey="btnPartInsertedResource1" />
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" meta:resourcekey="btnSaveResource1" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:ObjectDataSource ID="odsPartList" DeleteMethod="Delete" runat="server" EnablePaging="True"
                    SelectMethod="FindAll" SelectCountMethod="CountAll" TypeName="VDMS.II.PartManagement.CycleCountDAO">
                </asp:ObjectDataSource>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Upload Excel file"
            meta:resourcekey="TabPanel2Resource1">
            <ContentTemplate>
                <div class="form" style="width: 450px">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Localize ID="litFileName" runat="server" Text="Filename:" meta:resourcekey="litFileNameResource1"></asp:Localize>
                            </td>
                            <td>
                                <asp:FileUpload ID="fu" runat="server" meta:resourcekey="fuResource1" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="txtUpload" OnClick="txtUpload_OnClick" runat="server" Text="Upload"
                                    meta:resourcekey="txtUploadResource1" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="grid" runat="server" id="divResult" style="width: 100%" visible="False">
                    <table class="datatable" cellpadding="2" cellspacing="1">
                        <caption>
                            <asp:Literal ID="Literal2" runat="server" Text="Result" meta:resourcekey="Literal2Resource1"></asp:Literal>
                        </caption>
                        <tr class="group">
                            <td style="width: 140px">
                                <asp:Literal ID="Literal6" runat="server" Text="Total lines:" meta:resourcekey="Literal6Resource1"></asp:Literal>
                            </td>
                            <td style="width: 40px">
                                <asp:Literal ID="litTotalParts" runat="server" meta:resourcekey="litTotalPartsResource1"></asp:Literal>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr class="group">
                            <td>
                                <asp:Literal ID="Literal3" runat="server" Text="Valid lines:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litUpdated" runat="server" meta:resourcekey="litUpdatedResource1"></asp:Literal>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr class="group">
                            <td>
                                <asp:Literal ID="Literal1" runat="server" Text="Invalid lines:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litInvalid" runat="server" meta:resourcekey="litInvalidResource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litInvalidList" runat="server" meta:resourcekey="litInvalidListResource1"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <vdms:PageGridView runat="server" ID="gvInvalid" AutoGenerateColumns="False" meta:resourcekey="gvInvalidResource1">
                                    <Columns>
                                        <asp:BoundField DataField="Line" HeaderText="Line" meta:resourcekey="BoundFieldResource13" />
                                        <asp:TemplateField HeaderText="Part Code" meta:resourcekey="TemplateFieldResource13">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("OriginalObj.PartCode") %>'
                                                    meta:resourcekey="Label1Resource5"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity" meta:resourcekey="TemplateFieldResource14">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("OriginalObj.Quantity") %>'
                                                    meta:resourcekey="Label2Resource1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Safety Quantity" meta:resourcekey="TemplateFieldResource15">
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("OriginalObj.SafetyStock") %>'
                                                    meta:resourcekey="Label3Resource2"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part Type" meta:resourcekey="TemplateFieldResource16">
                                            <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("OriginalObj.PartType") %>'
                                                    meta:resourcekey="Label4Resource1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Error" meta:resourcekey="TemplateFieldResource17">
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("Error") %>' meta:resourcekey="Label5Resource1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </vdms:PageGridView>
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
    <asp:ObjectDataSource ID="odsInvalidParts" TypeName="VDMS.II.PartManagement.AutoCycleCount"
        runat="server" SelectCountMethod="CountInvalidParts" SelectMethod="FindInvalidParts"
        EnablePaging="True"></asp:ObjectDataSource>
</asp:Content>
