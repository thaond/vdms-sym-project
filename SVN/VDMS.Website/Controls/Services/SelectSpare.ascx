<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SelectSpare.ascx.cs" Inherits="Controls_Services_SelectSpare" %>
<%@ Register Src="~/Controls/UpdateProgress.ascx" TagName="UpdateProgress" TagPrefix="uc" %>
<asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="form">
            <table>
                <tr>
                    <td>
                        <asp:Literal ID="Literal1" runat="server" Text="Part Code:" 
                            meta:resourcekey="Literal1Resource1"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPC" runat="server" meta:resourcekey="txtPCResource1"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Literal ID="Literal3" runat="server" Text="Moto:" 
                            meta:resourcekey="Literal3Resource1"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMD" runat="server" meta:resourcekey="txtMDResource1"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="Literal2" runat="server" Text="Part Name:" 
                            meta:resourcekey="Literal2Resource1"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPN" runat="server" meta:resourcekey="txtPNResource1"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnSearch_Click" 
                            meta:resourcekey="LinkButton1Resource1">Search</asp:LinkButton>&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkCancel" runat="server" 
                            meta:resourcekey="lnkCancelResource1" >Cancel</asp:LinkButton>
                    </td>
                    <td>
                        <uc:UpdateProgress ID="up1" runat="server"></uc:UpdateProgress>
                    </td>
                </tr>
            </table>
        </div>
        <div class="grid">
            <vdms:PageGridView ID="GridView1" PageSize="18" runat="server" AutoGenerateColumns="False"
                AllowPaging="True"
                OnSelectedIndexChanging="GridView1_SelectedIndexChanging"                 
                meta:resourcekey="GridView1Resource1">
                <Columns>
                    <asp:TemplateField ShowHeader="False" meta:resourcekey="TemplateFieldResource1">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkSelect" 
                                ToolTip='<%# EvalTip(Eval("PartCode"),Eval("UnitPrice"),Eval("ManPower")) %>' 
                                runat="server" OnDataBinding="lnkSelect_DataBinding" 
                                meta:resourcekey="lnkSelectResource1">Select</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PartCode" HeaderText="Part Code" 
                        SortExpression="PartCode" meta:resourcekey="BoundFieldResource1" />
                    <asp:TemplateField HeaderText="Part Name" SortExpression="PartNameVN" 
                        meta:resourcekey="TemplateFieldResource2">
                        <ItemTemplate>
                            <asp:Literal ID="TextBox1" runat="server" Text='<%# SelectLang(Eval("PartNameVN"),Eval("PartNameEN")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="MotorCode" HeaderText="Motor Code" 
                        SortExpression="MotorCode" meta:resourcekey="BoundFieldResource2" />
                    <asp:BoundField DataField="WarrantyTime" HeaderText="Warr.Time"
                        SortExpression="WarrantyTime" meta:resourcekey="BoundFieldResource3">
                        <ItemStyle CssClass="number" />
                    </asp:BoundField>
                    <asp:BoundField DataField="WarrantyLength" HeaderText="Warr.Length"
                        SortExpression="WarrantyLength" meta:resourcekey="BoundFieldResource4">
                        <ItemStyle CssClass="number" />
                    </asp:BoundField>
                    <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price"
                        SortExpression="UnitPrice" meta:resourcekey="BoundFieldResource5">
                        <ItemStyle CssClass="number" />
                    </asp:BoundField>
                </Columns>
            </vdms:PageGridView>
            <asp:ObjectDataSource ID="odsP" SelectMethod="FindPart" TypeName="VDMS.I.Service.WarrantyConditionDAO"
                EnablePaging="True" runat="server" SelectCountMethod="CountPart">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtPC" Name="partCode" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txtPN" Name="partName" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txtMD" Name="model" PropertyName="Text" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="LinkButton1" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>
