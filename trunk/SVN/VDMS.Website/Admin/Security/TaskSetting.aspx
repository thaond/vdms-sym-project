<%@ Page Title="" Language="C#" MasterPageFile="~/MP/Popup.master" AutoEventWireup="true"
    CodeFile="TaskSetting.aspx.cs" Inherits="Admin_Security_TaskSetting" Theme="Thickbox" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<%@ Register Src="~/Controls/UpdateProgress.ascx" TagName="UpdateProgress" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <asp:ValidationSummary ID="ValidationSummary1" CssClass="error" ValidationGroup="AddTask"
        runat="server" meta:resourcekey="ValidationSummary1Resource1" />
    <div class="grid">
        <vdms:PageGridView ID="gv1" runat="server" OnRowDataBound="gv1_RowDataBound" AutoGenerateColumns="False"
            DataSourceID="ods1" meta:resourcekey="gv1Resource1" DataKeyNames="TaskId">
            <Columns>
                <asp:BoundField HeaderText="Task name" DataField="TaskName" 
                    meta:resourcekey="BoundFieldResource1" />
                <asp:BoundField HeaderText="Command name" DataField="CommandName" 
                    meta:resourcekey="BoundFieldResource2" />
                <asp:TemplateField meta:resourcekey="TemplateFieldResource1">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkAllow" runat="server" ToolTip='<%# Eval("CommandName") %>' 
                            meta:resourcekey="chkAllowResource1" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="false" 
                            CommandName="Delete" ImageUrl="~/Images/Delete.gif" Text="" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </vdms:PageGridView>
    </div>
    <asp:Button ID="cmdSaveTask" runat="server" Text="Save task setting" 
        OnClick="cmdSaveTask_Click" meta:resourcekey="cmdSaveTaskResource1" />
    <br />
    <div id="divNewTask" class="form">
        <table id="table1" runat="server" width="100%" cellpadding="2" cellspacing="2" border="0">
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Text:" 
                        meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="tb1" runat="server" meta:resourcekey="tb1Resource1"></asp:TextBox><asp:RequiredFieldValidator ID="rfv1"
                        runat="server" ControlToValidate="tb1" 
                        ErrorMessage="Text cannot be blank!" ValidationGroup="AddTask" 
                        meta:resourcekey="rfv1Resource1">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    
                    <asp:Literal ID="Literal2" runat="server" Text="Command:" 
                        meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="tb2" runat="server" meta:resourcekey="tb2Resource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv2"
                        runat="server" ControlToValidate="tb2" ErrorMessage="Command cannot be blank!"
                        ValidationGroup="AddTask" meta:resourcekey="rfv2Resource1">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:ImageButton ID="ibAddTask" runat="server" ImageUrl="~/Images/update.gif" OnClick="ibAddTask_Click"
                        ValidationGroup="AddTask" meta:resourcekey="ibAddTaskResource1" />
                </td>
            </tr>
        </table>
    </div>
    <asp:ObjectDataSource ID="ods1" runat="server" TypeName="VDMS.II.Security.PermissionDAO"
        SelectMethod="GetTasks" DeleteMethod="DeleteTask" >
        <DeleteParameters>
            <asp:Parameter Name="TaskId" Type="Int64" />
        </DeleteParameters>
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="url" Name="url" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
