<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserProfile.ascx.cs" Inherits="Admin_Controls_UserProfile" %>
<table cellpadding="2" cellspacing="0" border="0" width="100%">
    <tr>
        <td style="width: 30%">
            <asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
            <asp:Localize runat="server" ID="litFullname" Text="Fullname:" meta:resourcekey="litFullnameResource1" />
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtFullname" meta:resourcekey="txtFullnameResource1"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFullname"
                SetFocusOnError="True" Display="Dynamic" ErrorMessage="Fullname is required."
                ToolTip="Fullname is required." ValidationGroup="SaveUser" 
                meta:resourcekey="RequiredFieldValidator1Resource1" Text="*"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <asp:MultiView ID="mvP" runat="server" ActiveViewIndex="0">
        <asp:View ID="v1" runat="server">
            <tr>
                <td>
                    <asp:Image ID="image3" runat="server" SkinID="RequireFieldNon" meta:resourcekey="imageResource1" />
                    <asp:Localize runat="server" ID="litAreaCode" Text="Area Code:" meta:resourcekey="litAreaCodeResource1" />
                </td>
                <td>
                    <asp:UpdatePanel ID="udpArea" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlArea" runat="server" DataValueField="AreaCode" DataTextField="AreaName"
                                meta:resourcekey="ddlAreaResource1">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlDatabase" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="image4" runat="server" SkinID="RequireFieldNon" meta:resourcekey="imageResource1" />
                    <asp:Localize runat="server" ID="litDatabaseCode" Text="Database Code:" meta:resourcekey="litDatabaseCodeResource1" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlDatabase" runat="server" meta:resourcekey="ddlDatabaseResource1"
                        OnSelectedIndexChanged="ddlDatabase_SelectedIndexChanged" 
                        AutoPostBack="True">
                        <asp:ListItem Value="DNF" Text="Dongnai" meta:resourcekey="ListItemResource3"></asp:ListItem>
                        <asp:ListItem Value="HTF" Text="Hatay" meta:resourcekey="ListItemResource4"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                        AssociatedUpdatePanelID="udpArea">
                        <ProgressTemplate>
                            <img src="../../Images/Spinner.gif" alt="Process" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="image5" runat="server" SkinID="RequireFieldNon" meta:resourcekey="imageResource1" />
                    <asp:Localize runat="server" ID="litDept" Text="Department:" 
                        meta:resourcekey="litDeptResource2"></asp:Localize>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDept" runat="server" 
                        meta:resourcekey="ddlDeptResource1">
                        <asp:ListItem Value="VH" Text="Vehicle" meta:resourcekey="ListItemResource5"></asp:ListItem>
                        <asp:ListItem Value="SR" Text="Service" meta:resourcekey="ListItemResource6"></asp:ListItem>
                        <asp:ListItem Value="SP" Text="Spare Part" meta:resourcekey="ListItemResource7"></asp:ListItem>
                        <asp:ListItem Value="AM" Text="AMATA" meta:resourcekey="ListItemResource8"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="image1" runat="server" SkinID="RequireFieldNon" meta:resourcekey="imageResource1" />
                    <asp:Localize runat="server" ID="litPosition" Text="Position:" 
                        meta:resourcekey="litPositionResource1"></asp:Localize>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPosition" runat="server" 
                        meta:resourcekey="ddlPositionResource1">
                        <asp:ListItem Value="E" Text="Employee" meta:resourcekey="ListItemResource9"></asp:ListItem>
                        <asp:ListItem Value="M" Text="Manager" meta:resourcekey="ListItemResource10"></asp:ListItem>
                        <asp:ListItem Value="S" Text="Super Manager" 
                            meta:resourcekey="ListItemResource11"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="image6" runat="server" SkinID="RequireFieldNon" meta:resourcekey="imageResource1" />
                    <asp:Localize runat="server" ID="litNGApproveLevel" Text="NG Approve Level:" 
                        meta:resourcekey="litNGApproveLevelResource1"></asp:Localize>
                </td>
                <td>
                    <asp:DropDownList ID="ddlNGAL" runat="server" 
                        meta:resourcekey="ddlNGALResource1">
                        <asp:ListItem Selected="True" Text="No Right" Value="0" 
                            meta:resourcekey="ListItemResource12"></asp:ListItem>
                        <asp:ListItem Text="Level 1" Value="1" meta:resourcekey="ListItemResource13"></asp:ListItem>
                        <asp:ListItem Text="Level 2" Value="2" meta:resourcekey="ListItemResource14"></asp:ListItem>
                        <asp:ListItem Text="Level 3" Value="3" meta:resourcekey="ListItemResource15"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </asp:View>
        <asp:View ID="v2" runat="server">
            <tr>
                <td>
                    <asp:Image ID="image2" runat="server" SkinID="RequireFieldNon" meta:resourcekey="imageResource1" />
                    <asp:Localize runat="server" ID="litWarehouse" Text="Part Warehouse:" 
                        meta:resourcekey="litWarehouseResource1"></asp:Localize>
                </td>
                <td>
                    <vdms:WarehouseList ID="ddlWH" runat="server" MergeCode="true" DontAutoUseCurrentSealer="False" 
                        meta:resourcekey="ddlWHResource1" ShowEmptyItem="true" 
                        ShowSelectAllItem="False" UseVIdAsValue="False">
                    </vdms:WarehouseList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="image7" runat="server" SkinID="RequireFieldNon" meta:resourcekey="imageResource1" />
                    <asp:Localize runat="server" ID="Localize1" Text="Vehicle Warehouse:" 
                        meta:resourcekey="Localize1Resource1"></asp:Localize>
                </td>
                <td>
                    <vdms:WarehouseList ID="ddlVWh" runat="server" MergeCode="true" DontAutoUseCurrentSealer="False" 
                        meta:resourcekey="ddlVWhResource1" ShowEmptyItem="true" Type="V" UseVIdAsValue="true"
                        ShowSelectAllItem="False">
                    </vdms:WarehouseList>
                </td>
            </tr>
        </asp:View>
    </asp:MultiView>
</table>
