<%@ Page Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="reformation.aspx.cs" Inherits="Sales_Inventory_reformation" Title="Thao tác sửa đổi xe nhập"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div class="form">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Save"
            Width="100%" DisplayMode="List" meta:resourcekey="ValidationSummary1Resource1" />
        <span style="color: #ff0000">
            <asp:Label ID="liMes" runat="server" meta:resourcekey="liMesResource1"></asp:Label>
            <br />
            <asp:Label ID="lbMes" runat="server" ForeColor="RoyalBlue" meta:resourcekey="lbMesResource1"></asp:Label>
        </span>
        <table border="0" cellpadding="2" cellspacing="2">
            <tr>
                <td>
                    <asp:Image ID="image" runat="server" SkinID="RequireField" meta:resourcekey="imageResource1" />
                    <asp:Literal ID="lEngineNo" runat="server" Text="EngineNo:" meta:resourcekey="lEngineNoResource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtEngineNo" runat="server" MaxLength="20" meta:resourcekey="txtEngineNoResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True"
                        ValidationGroup="Save" ControlToValidate="txtEngineNo" ErrorMessage='EngineNo can not be blank!'
                        CssClass="lblClass" meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="Image1" runat="server" SkinID="RequireField" meta:resourcekey="Image1Resource1" />
                    <asp:Literal ID="lDropEngineNo" runat="server" Text="Drop engineNo:" meta:resourcekey="lDropEngineNoResource1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtEngineNoCancel" runat="server" MaxLength="20" meta:resourcekey="txtEngineNoCancelResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True"
                        ValidationGroup="Save" ControlToValidate="txtEngineNoCancel" ErrorMessage='Drop EngineNo can not be blank!'
                        CssClass="lblClass" meta:resourcekey="RequiredFieldValidator2Resource1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnTest" runat="server" Text="Test" ValidationGroup="Save" Css OnClick="btnTest_Click"
                        meta:resourcekey="btnTestResource1" />
                </td>
            </tr>
        </table>
    </div>
    <asp:PlaceHolder ID="plInf" runat="server" Visible="False">
        <table border="0" cellpadding="0" width="100%" cellspacing="0">
            <tr>
                <!--
        <td width="15%" align="right" ><img src="imgs/s_mustinput.gif" width="15" height="15">&nbsp;Kho</td>
        <td colspan="3" class="form_word"><input name="textfield" type="text" class="input_word" value="6" size="3">
          (6=Bỏ mã số động cơ  7=Điều chỉnh kho)</td>
          (2=Điều chuyển 　3=Kiểm kê dư 　4=Kiểm kê hụt 　5=Bù chứng từ 　6=Sửa đổi nhập xe(Phòng xuất xe))</td>  
-->
            </tr>
            <tr id="item2">
                <td align="left" style="width: 20%">
                    <asp:Literal ID="Literal1" runat="server" Text="Motor type:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td style="width: 30%">
                    <asp:Literal ID="lMotorType" runat="server" meta:resourcekey="lMotorTypeResource1"></asp:Literal>
                </td>
                <td align="left" style="width: 20%">
                    <asp:Literal ID="li1" runat="server" Text="Motor type:" meta:resourcekey="li1Resource1"></asp:Literal>
                </td>
                <td style="width: 30%">
                    <asp:Literal ID="lDMotorType" runat="server" meta:resourcekey="lDMotorTypeResource1"></asp:Literal>
                </td>
            </tr>
            <tr id="item3">
                <td align="left" style="width: 20%">
                    <asp:Literal ID="Literal2" runat="server" Text="Color:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td style="width: 30%">
                    <asp:Literal ID="lColor" runat="server" meta:resourcekey="lColorResource1"></asp:Literal>
                </td>
                <td align="left" style="width: 20%">
                    <asp:Literal ID="li2" runat="server" Text="Color:" meta:resourcekey="li2Resource1"></asp:Literal>
                </td>
                <td style="width: 30%">
                    <asp:Literal ID="lDColor" runat="server" meta:resourcekey="lDColorResource1"></asp:Literal>
                </td>
            </tr>
            <!-- 
      <tr id="item4" style="display:none">
       <td align="right" >Số hóa đơn</td>
        <td colspan="3"><table width="129" border="0" cellpadding="0" cellspacing="0" class="label_text">
            <tr>
              <td width="129">&nbsp;</td>
            </tr>
        </table></td>
      </tr>
-->
            <tr id="item4">
                <td align="left" style="width: 20%">
                    <asp:Literal ID="Literal3" runat="server" Text="Produce date:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                </td>
                <td style="width: 30%">
                    <asp:Literal ID="lProduceDate" runat="server" meta:resourcekey="lProduceDateResource1"></asp:Literal>
                </td>
                <td align="left" style="width: 20%">
                    <asp:Literal ID="li3" runat="server" Text="Produce date:" meta:resourcekey="li3Resource1"></asp:Literal>
                </td>
                <td style="width: 30%">
                    <asp:Literal ID="lDProduceDate" runat="server" meta:resourcekey="lDProduceDateResource1"></asp:Literal>
                </td>
            </tr>
            <tr id="item5">
                <td align="left" style="width: 20%">
                    <asp:Literal ID="Literal4" runat="server" Text="Current store:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                </td>
                <td style="width: 30%">
                    <asp:Literal ID="lCurrStore" runat="server" meta:resourcekey="lCurrStoreResource1"></asp:Literal>
                </td>
                <td align="left" style="width: 20%">
                    <asp:Literal ID="li4" runat="server" Text="Current store:" meta:resourcekey="li4Resource1"></asp:Literal>
                </td>
                <td style="width: 30%">
                    <asp:Literal ID="lDCurrStore" runat="server" meta:resourcekey="lDCurrStoreResource1"></asp:Literal>
                </td>
            </tr>
            <tr id="item6">
                <td align="left" style="width: 20%">
                    <asp:Literal ID="Literal5" runat="server" Text="Current status:" meta:resourcekey="Literal5Resource1"></asp:Literal>
                </td>
                <td style="width: 30%">
                    <asp:Literal ID="lCurrStatus" runat="server" meta:resourcekey="lCurrStatusResource1"></asp:Literal>
                </td>
                <td align="left" style="width: 20%">
                    <asp:Literal ID="li5" runat="server" Text="Current status:" meta:resourcekey="li5Resource1"></asp:Literal>
                </td>
                <td style="width: 30%">
                    <asp:Literal ID="lDCurrStatus" runat="server" meta:resourcekey="lDCurrStatusResource1"></asp:Literal>
                </td>
            </tr>
            <tr id="item7">
                <td align="left" style="width: 20%">
                    <asp:Literal ID="Literal6" runat="server" Text="Voucher:" meta:resourcekey="Literal6Resource1"></asp:Literal>
                </td>
                <td style="width: 30%">
                    <asp:Literal ID="lVoucher" runat="server" meta:resourcekey="lVoucherResource1"></asp:Literal>
                </td>
                <td align="left" style="width: 20%">
                    <asp:Literal ID="Literal12" runat="server" Text="Voucher:" meta:resourcekey="Literal12Resource1"></asp:Literal>
                </td>
                <td style="width: 30%">
                    <asp:Literal ID="lDVoucher" runat="server" meta:resourcekey="lDVoucherResource1"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 20%">
                </td>
                <td style="width: 30%">
                </td>
                <td align="left" style="width: 20%">
                </td>
                <td align="center" style="width: 30%">
                    <asp:Button ID="btnApply" runat="server" ValidationGroup="Save" Text="Apply" OnClick="btnApply_Click"
                        meta:resourcekey="btnApplyResource1" />
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel"
                        meta:resourcekey="btnCancelResource1" />
                </td>
            </tr>
            <tr id="item8">
                <!--
        <td align="right" ><img src="imgs/s_mustinput.gif" width="15" height="15">Ngày nhập kho</td>
        <td><table border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td><input name="textfield" type="text" class="input_word" size="10"></td>
            <td width="5">&nbsp;</td>
            <td><a href="#"><img src="imgs/calender.gif" width="20" height="20" border="0"></a></td>
          </tr>
        </table></td>

        <td align="right" ><img src="imgs/s_mustinput.gif" width="15" height="15">Đại lý xuất kho</td>
        <td class="form_word"><select name="select">
          <option>TT001A(Đại lý Miền Nam)</option>
          <option selected>TT001B(Miền Nam-Cửa hàngB)</option>
          <option>TT001C(Miền Nam-Cửa hàngC)</option>
                                </select></td>

        <td><div align="right"><span ><img src="imgs/s_mustinput.gif" width="15" height="15">Đại lý nhập kho</span></div></td>
        <td class="form_word"><select name="select">
          <option selected>TT001A(Đại lý Miền Nam)</option>
          <option>TT001B(Miền Nam-Cửa hàngB)</option>
          <option>TT001C(Miền Nam-Cửa hàngC)</option>
                                        </select></td>
      </tr>
-->
                <!--
      <tr id="item6" style="display:none">
        <td align="right" ><img src="imgs/s_mustinput.gif" width="15" height="15">Ngày xuất kho</td>
        <td><table border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td><input name="textfield" type="text" class="input_word" size="10"></td>
            <td width="5">&nbsp;</td>
            <td><a href="#"><img src="imgs/calender.gif" width="20" height="20" border="0"></a></td>
          </tr>
        </table></td>
        <td align="right" ><img src="imgs/s_mustinput.gif" width="15" height="15">Đại lý xuất kho</td>
        <td class="form_word"><select name="select">
          <option>TT001A(Đại lý Miền Nam)</option>
          <option selected>TT001B(Miền Nam-Cửa hàngB)</option>
          <option>TT001C(Miền Nam-Cửa hàngC)</option>
                                </select></td>
      </tr>

      <tr id="item9" style="display:none">
        <td align="right" >Ngày điều chuyển</td>
        <td><table width="80" border="0" cellpadding="0" cellspacing="0" class="label_text">
            <tr>
              <td>2006/01/05</td>
            </tr>
        </table></td>
        <td align="right" >Người điều chuyển</td>
        <td><table width="80" border="0" cellpadding="0" cellspacing="0" class="label_text">
            <tr>
              <td>Vương Đại Minh</td>
            </tr>
        </table></td>
        </tr>
-->
            </tr>
        </table>
    </asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="h">
</asp:Content>
