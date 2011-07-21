<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="CustomerInfor.aspx.cs" Inherits="Service_CustomerInfor" Title="Tư liệu khách hàng " %>

<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <table cellspacing="2" cellpadding="2" border="0">
            <tr width="580">
                <td valign="top" nowrap>
                    <asp:Image ID="image1" runat="server" SkinID="RequireFieldNon" />
                    Loại khách hàng:
                </td>
                <td valign="top" nowrap colspan="4">
                    <asp:DropDownList ID="ddlCusType" runat="server" Width="398px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr width="580">
                <td valign="top" nowrap>
                    <asp:Image ID="image2" runat="server" SkinID="RequireFieldNon" />
                    Họ tên:
                </td>
                <td valign="top" nowrap colspan="4">
                    <asp:TextBox ID="txtName" runat="server" Width="394px"></asp:TextBox>
                </td>
            </tr>
            <tr width="580">
                <td valign="top" nowrap>
                    <asp:Image ID="image3" runat="server" SkinID="RequireFieldNon" />
                    Địa chỉ:
                </td>
                <td valign="top" nowrap colspan="4">
                    <asp:DropDownList ID="ddlAddress" runat="server" Width="131px">
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlAddress1" runat="server" Width="131px">
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlAddress2" runat="server" Width="131px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr width="580">
                <td valign="top" nowrap>
                    <asp:Image ID="image4" runat="server" SkinID="RequireFieldNon" />
                    Số CMND:
                </td>
                <td valign="top" nowrap colspan="23">
                    <asp:TextBox ID="txtIdentity" runat="server" Width="394px"></asp:TextBox>
                </td>
            </tr>
            <tr width="580">
                <td valign="top" nowrap>
                    <asp:Image ID="image5" runat="server" SkinID="RequireFieldNon" />
                    Giới tính:
                </td>
                <td valign="top" nowrap colspan="3">
                    <asp:DropDownList ID="ddlSex" runat="server" Width="143px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr width="580">
                <td valign="top" nowrap>
                    <asp:Image ID="image6" runat="server" SkinID="RequireFieldNon" />
                    Tuổi:
                </td>
                <td valign="top" nowrap>
                    <asp:TextBox ID="txtFromAge" runat="server" Width="88px"></asp:TextBox>
                    (Tuổi)
                </td>
                <td valign="top" nowrap colspan="3">
                    <asp:TextBox ID="txtToAge" runat="server" Width="88px"></asp:TextBox>
                    (Tuổi)
                </td>
            </tr>
            <tr width="580">
                <td valign="top" nowrap>
                    <asp:Image ID="image7" runat="server" SkinID="RequireFieldNon" />
                    Sinh tháng:
                </td>
                <td valign="top" nowrap>
                    <asp:DropDownList ID="ddlFromMonth" runat="server" Width="92px">
                    </asp:DropDownList>
                    (Tháng)
                </td>
                <td valign="top" nowrap colspan="3">
                    <asp:DropDownList ID="ddlToMonth" runat="server" Width="92px">
                    </asp:DropDownList>
                    (Tháng)
                </td>
            </tr>
            <tr width="580">
                <td valign="top" nowrap>
                    <asp:Image ID="image8" runat="server" SkinID="RequireFieldNon" />
                    Số điện thoại:
                </td>
                <td valign="top" nowrap>
                    <asp:TextBox ID="txtPhoneNo" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr width="580">
                <td valign="top" nowrap>
                    <asp:Image ID="image9" runat="server" SkinID="RequireFieldNon" />
                    Phân loại khách hàng:
                </td>
                <td valign="top" nowrap>
                    <asp:DropDownList ID="ddlCusClassify" runat="server" Width="143px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr width="580">
                <td valign="top" nowrap>
                    <asp:Image ID="image10" runat="server" SkinID="RequireFieldNon" />
                    Nghề nghiệp:
                </td>
                <td valign="top" nowrap colspan="4">
                    <asp:CheckBox ID="ckPupil" runat="server" Text="Học sinh" />
                    <asp:CheckBox ID="ckOffice" runat="server" Text="Công sở" />
                    <asp:CheckBox ID="ckFree" runat="server" Text="Tự do" />
                    <asp:CheckBox ID="ckSoldier" runat="server" Text="Quân nhân" />
                    <asp:CheckBox ID="ckOther" runat="server" Text="Khác" />
                </td>
            </tr>
            <tr width="580">
                <td valign="top" nowrap>
                    <asp:Image ID="image11" runat="server" SkinID="RequireFieldNon" />
                    Chủng loại xe:
                </td>
                <td valign="top" nowrap>
                    <asp:TextBox ID="VehicleType" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr width="580">
                <td valign="top" nowrap>
                    <asp:Image ID="image12" runat="server" SkinID="RequireFieldNon" />
                    Số máy:
                </td>
                <td valign="top" nowrap>
                    <asp:TextBox ID="txtEngineNo1" runat="server"></asp:TextBox>
                </td>
                <td valign="top" nowrap colspan="3">
                    <asp:TextBox ID="txtEngineNo2" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr width="580">
                <td valign="top" nowrap>
                    <asp:Image ID="image13" runat="server" SkinID="RequireFieldNon" />
                    Số Km:
                </td>
                <td valign="top" nowrap>
                    <asp:TextBox ID="txtKm1" runat="server"></asp:TextBox>
                </td>
                <td valign="top" nowrap colspan="3">
                    <asp:TextBox ID="txtKm2" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr width="580">
                <td valign="top" nowrap>
                    <asp:Image ID="image14" runat="server" SkinID="RequireFieldNon" />
                    Ngày mua:
                </td>
                <td valign="top" nowrap>
                    <asp:TextBox ID="txtBuyFromDate" runat="server" Width="88px"></asp:TextBox>&nbsp;
                    <asp:ImageButton ID="ImageButton1" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtBuyFromDate"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" AcceptAMPM="false" CultureName="vi-VN" />
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtBuyFromDate"
                        Format="dd/MM/yyyy" PopupButtonID="ImageButton1">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td valign="top" nowrap colspan="3">
                    <asp:TextBox ID="txtBuyToDate" runat="server" Width="88px"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton2" runat="Server" SkinID="CalendarImageButton" OnClientClick="return false;" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtBuyToDate"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" AcceptAMPM="false" CultureName="vi-VN" />
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBuyToDate"
                        Format="dd/MM/yyyy" PopupButtonID="ImageButton2">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td valign="top" nowrap>
                </td>
            </tr>
            <tr width="580">
                <td valign="top" nowrap>
                    <asp:Image ID="image15" runat="server" SkinID="RequireFieldNon" />
                    Trong vòng:
                </td>
                <td valign="top" nowrap colspan="4">
                    <asp:TextBox ID="txtRecentMonth" runat="server" Width="88px"></asp:TextBox>
                    Tháng gần đây
                </td>
            </tr>
            <tr width="580">
                <td valign="top" nowrap>
                </td>
                <td valign="top" nowrap colspan="4">
                    <asp:RadioButton ID="radYes" runat="server" Text="Có đến" />
                    <asp:RadioButton ID="radNo" runat="server" Text="Không đến cửa hàng này mua hàng" />
                </td>
            </tr>
            <tr width="580">
                <td valign="top" nowrap>
                </td>
                <td valign="top" nowrap colspan="4">
                    <asp:Button ID="btnTest" runat="server" Text="Kiểm tra" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
