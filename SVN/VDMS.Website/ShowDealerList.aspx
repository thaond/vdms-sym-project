<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowDealerList.aspx.cs" Inherits="ShowDealerList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding: 10px;">
        <asp:ObjectDataSource ID="odsDealerList" runat="server" EnablePaging="True" TypeName="VDMS.II.BasicData.DealerDAO"
            SelectMethod="FindByCode" SelectCountMethod="CountByCode" DeleteMethod="Delete"
            OldValuesParameterFormatString="original_{0}">
            <DeleteParameters>
                <asp:Parameter Name="DealerCode" Type="String" />
            </DeleteParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="txtDealerCode" Name="dealerCode" PropertyName="Text"
                    Type="String" />
                <asp:Parameter Name="autoinstockpart" Type="String" />
                <asp:Parameter Name="autoinstockvehicle" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <table>
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Dealer code:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtDealerCode" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnFind" runat="server" Text="Search" meta:resourcekey="btnFindResource1"
                        OnClick="btnFind_Click" />
                </td>
            </tr>
        </table>
        <div class="grid">
            <vdms:PageGridView ID="gv" runat="server" DataSourceID="odsDealerList" AllowPaging="True"
                DataKeyNames="DealerCode" meta:resourcekey="gvResource1" OnPageIndexChanging="gv_PageIndexChanging"
                OnPreRender="gv_PreRender">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DealerCode" HeaderText="Dealer Code" meta:resourcekey="BoundFieldResource1" />
                    <asp:BoundField DataField="DealerName" HeaderText="Dealer Name" meta:resourcekey="BoundFieldResource3" />
                </Columns>
                <EmptyDataTemplate>
                    <b>
                        <asp:Localize ID="litNotFound" runat="server" Text="There are not any dealers." meta:resourcekey="litNotFoundResource1"></asp:Localize></b>
                </EmptyDataTemplate>
            </vdms:PageGridView>
        </div>
        <br />
        <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" meta:resourcekey="btnSaveResource1" />
    </div>
    </form>
</body>
</html>
