<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ExchangeVoucherList.ascx.cs"
    Inherits="Controls_Services_ExchangeVoucherList" %>
<%@ Register Src="~/Controls/Services/SelectSpare.ascx" TagName="SelectSpare" TagPrefix="uc" %>

<script language="javascript" type="text/javascript">
    var copyTo1, copyTo2, copyTo3;
    function doSelSpareCode(obj1, obj2, obj3, obj4) {
        copyTo1 = obj1;
        copyTo2 = obj2;
        copyTo3 = obj3;
        document.getElementById('mainView').className = "hidden";
        document.getElementById('_selectSpare').className = "";
        document.getElementById('<%= selectSpare.txtPCID %>').value = document.getElementById(obj4).value;
        
    }
    function cancelSelect() {
        copyTo1 = null;
        copyTo2 = null;
        copyTo3 = null;
        document.getElementById('mainView').className = "grid";
        document.getElementById('_selectSpare').className = "hidden";
    }
    function selectSpare(part, unitPrice, manP) {
        //tb_remove();
        document.getElementById('mainView').className = "grid";
        document.getElementById('_selectSpare').className = "hidden";
        var tb1 = document.getElementById(copyTo1);
        var tb2 = document.getElementById(copyTo2);
        var tb3 = document.getElementById(copyTo3);
        tb1.value = part;
        tb2.value = unitPrice;
        tb3.value = manP;
    }
</script>

<asp:Panel ID="pnDealer" runat="server" Height="100%" Width="100%" meta:resourcekey="pnDealerResource1">
    <div class="form">
    
        <table border="0" cellpadding="2" cellspacing="0" style="width: 100%">
            <tr>
                <td style="width: 84px">
                    <asp:Literal ID="Literal1" runat="server" Text="Dealer code:" meta:resourcekey="Literal1Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:Literal ID="litDealerCode" runat="server" meta:resourcekey="litDealerCodeResource1"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 84px">
                    <asp:Literal ID="Literal2" runat="server" Text="Dealer name:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                </td>
                <td>
                    <asp:Literal ID="litDealerName" runat="server" meta:resourcekey="litDealerNameResource1"></asp:Literal>
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>
<div id="mainView" class="grid">
    <asp:BulletedList ID="bllErrorMsg" runat="server" meta:resourcekey="bllErrorMsgResource1"
        CssClass="errorMsg" />
    <asp:Literal Visible="false" runat="server" ID="litErrMsgCommentBlank" meta:resourcekey="litErrMsgCommentBlank"></asp:Literal>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="error" ValidationGroup="UpdateVoucher"
        meta:resourcekey="ValidationSummary1Resource1" />
    <asp:ListView ID="lvEv" DataKeyNames="ExchangePartHeaderId" runat="server" DataSourceID="odsH"
        OnItemEditing="lvEv_ItemEditing" OnItemUpdating="lvEv_ItemUpdating">
        <LayoutTemplate>
            <table class="datatable" cellpadding="1" cellspacing="2">
                <tr>
                    <th>
                        <asp:Literal ID="Literal1" runat="server" Text="Part Code" meta:resourcekey="Literal1Resource3"></asp:Literal>
                    </th>
                    <th>
                        <asp:Literal ID="Literal16" runat="server" Text="VMEP comment" meta:resourcekey="Literal16Resource1"></asp:Literal>
                    </th>
                    <th>
                        <asp:Literal ID="Literal12" runat="server" Text="Unit price(VND)" meta:resourcekey="Literal12Resource1"></asp:Literal>
                    </th>
                    <th>
                        <asp:Literal ID="Literal13" runat="server" Text="Man power" meta:resourcekey="Literal13Resource1"></asp:Literal>
                    </th>
                    <th>
                        <asp:Literal ID="Literal8" runat="server" Text="Quantity" meta:resourcekey="Literal8Resource1"></asp:Literal>
                    </th>
                    <th>
                        <asp:Literal ID="Literal17" runat="server" Text="Parts cost(VND)" meta:resourcekey="Literal17Resource1"></asp:Literal>
                    </th>
                    <th>
                        <asp:Literal ID="Literal14" runat="server" Text="Warranty fee(VND)" meta:resourcekey="Literal14Resource1"></asp:Literal>
                    </th>
                    <th>
                        <asp:Literal ID="Literal15" runat="server" Text="Amount(VND)" meta:resourcekey="Literal15Resource1"></asp:Literal>
                    </th>
                </tr>
                <tr id="itemPlaceHolder" runat="server" />
                <tfoot>
                    <tr class="end">
                        <td colspan="4">
                            <asp:Literal ID="Literal18" runat="server" Text="Page total:" meta:resourcekey="Literal18Resource1"></asp:Literal>
                        </td>
                        <td class="number">
                            <asp:Literal ID="litQuantity" runat="server" meta:resourcekey="litQuantityResource1"></asp:Literal><br />
                            <asp:Label CssClass="modValue" ID="lbQuantity" runat="server" meta:resourcekey="lbQuantityResource1"></asp:Label>
                        </td>
                        <td class="number">
                            <asp:Literal ID="litPartsCost" runat="server" meta:resourcekey="litPartsCostResource1"></asp:Literal><br />
                            <asp:Label CssClass="modValue" ID="lbPartsCost" runat="server" meta:resourcekey="lbPartsCostResource1"></asp:Label>
                        </td>
                        <td class="number">
                            <asp:Literal ID="litFee" runat="server" meta:resourcekey="litFeeResource1"></asp:Literal><br />
                            <asp:Label CssClass="modValue" ID="lbFee" runat="server" meta:resourcekey="lbFeeResource1"></asp:Label>
                        </td>
                        <td class="number">
                            <asp:Literal ID="litAmount" runat="server" meta:resourcekey="litAmountResource1"></asp:Literal><br />
                            <asp:Label CssClass="modValue" ID="lbAmount" runat="server" meta:resourcekey="lbAmountResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <vdms:DataPager ID="pager" PageSize="10" PagedControlID="lvEv" runat="server">
                            </vdms:DataPager>
                        </td>
                    </tr>
                    <tr class="end" runat="server" id="allPageTotal">
                        <td colspan="4" runat="server">
                            <asp:Literal ID="Literal20" meta:resourcekey="litLiteral20Resource1" runat="server"
                                Text="All total:"></asp:Literal>
                        </td>
                        <td class="number" runat="server">
                            <asp:Literal ID="litQuantityA" runat="server"></asp:Literal><br />
                            <asp:Label CssClass="modValue" ID="lbQuantityA" runat="server"></asp:Label>
                        </td>
                        <td class="number" runat="server">
                            <asp:Literal ID="litPartsCostA" runat="server"></asp:Literal><br />
                            <asp:Label CssClass="modValue" ID="lbPartsCostA" runat="server"></asp:Label>
                        </td>
                        <td class="number" runat="server">
                            <asp:Literal ID="litFeeA" runat="server"></asp:Literal><br />
                            <asp:Label CssClass="modValue" ID="lbFeeA" runat="server"></asp:Label>
                        </td>
                        <td class="number" runat="server">
                            <asp:Literal ID="litAmountA" runat="server"></asp:Literal><br />
                            <asp:Label CssClass="modValue" ID="lbAmountA" runat="server"></asp:Label>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </LayoutTemplate>
        <EditItemTemplate>
            <tr class="group">
                <td colspan="4">
                    <table style="width: 100%">
                        <tr class="group2">
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal1" runat="server" Text="Customer:" meta:resourcekey="Literal1Resource4"></asp:Literal>
                            </td>
                            <td class="normal">
                                <%#Eval("CustomerName")%>
                            </td>
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal2" runat="server" Text="Engine:" meta:resourcekey="Literal2Resource3"></asp:Literal>
                            </td>
                            <td class="normal">
                                <%#Eval("EngineNumber")%>
                            </td>
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal3" runat="server" Text="Model:" meta:resourcekey="Literal3Resource2"></asp:Literal>
                            </td>
                            <td class="normal">
                                <%#Eval("Model")%>
                            </td>
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal4" runat="server" Text="Km:" meta:resourcekey="Literal4Resource2"></asp:Literal>
                            </td>
                            <td class="normal">
                                <%#Eval("KmCount")%>
                            </td>
                        </tr>
                        <tr class="group2">
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal5" runat="server" Text="Purchased:" meta:resourcekey="Literal5Resource2"></asp:Literal>
                            </td>
                            <td class="normal">
                                <%#EvalDate(Eval("PurchaseDate"))%>
                            </td>
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal6" runat="server" Text="Repair:" meta:resourcekey="Literal6Resource2"></asp:Literal>
                            </td>
                            <td class="normal">
                                <%#EvalDate(Eval("ExchangedDate"))%>
                            </td>
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal7" runat="server" Text="Processed:" meta:resourcekey="Literal7Resource2"></asp:Literal>
                            </td>
                            <td class="normal">
                                <%#EvalDate(Eval("LastProcessedDate"))%>
                            </td>
                            <td style="white-space: nowrap">
                                <asp:LinkButton ID="btnUpdate" ValidationGroup="UpdateVoucher" runat="server" CommandName="Update"
                                    Text="Update" meta:resourcekey="btnUpdateResource1" />
                            </td>
                            <td style="white-space: nowrap">
                                <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel"
                                    meta:resourcekey="btnCancelResource1" />
                            </td>
                        </tr>
                        <tr class="group2">
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal9" runat="server" Text="Service:" meta:resourcekey="Literal9Resource2"></asp:Literal>
                            </td>
                            <td class="normal">
                                <asp:HyperLink ID="HyperLink1" runat="server" CssClass="showDetail" Text='<%# Eval("ServiceSheetNumber") %>'
                                    Target="_blank" NavigateUrl='<%# "~/service/WarrantyContent.aspx?srsn=" + Eval("ServiceSheetNumber") %>'
                                    meta:resourcekey="HyperLink1Resource2"></asp:HyperLink>
                            </td>
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal10" runat="server" Text="Exchange:" meta:resourcekey="Literal10Resource2"></asp:Literal>
                            </td>
                            <td class="normal">
                                <asp:HyperLink ID="HyperLink2" runat="server" CssClass="showDetail" Text='<%# Eval("VoucherNumber") %>'
                                    Target="_blank" NavigateUrl='<%# "~/service/WarrantyContent.aspx?pcvn=" + Eval("VoucherNumber") %>'
                                    meta:resourcekey="HyperLink2Resource2"></asp:HyperLink>
                            </td>
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal11" runat="server" Text="Status:" meta:resourcekey="Literal11Resource2"></asp:Literal>
                            </td>
                            <td class="normal">
                                <%#EvalStatus(Eval("Status"))%>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="number">
                    <%# EvalNumber(Eval("TotalQuantityO"))%>
                    <br />
                    <asp:Label CssClass="modValue" ID="lbs0" runat="server" Text='<%# EvalNumber(Eval("TotalQuantityM")) %>'
                        meta:resourcekey="lbs0Resource2"></asp:Label>
                </td>
                <td class="number">
                    <%# EvalNumber(Eval("TotalPartCostO"))%>
                    <br />
                    <asp:Label CssClass="modValue" ID="Label3" runat="server" Text='<%# EvalNumber(Eval("TotalPartCostM")) %>'
                        meta:resourcekey="Label3Resource2"></asp:Label>
                </td>
                <td class="number">
                    <%# EvalNumber(Eval("ProposeFeeAmount"))%>
                    <br />
                    <asp:Label CssClass="modValue" ID="Label4" runat="server" Text='<%# EvalNumber(Eval("TotalFeeM")) %>'
                        meta:resourcekey="Label4Resource2"></asp:Label>
                </td>
                <td class="number">
                    <%# EvalNumber(Eval("TotalAmountO"))%>
                    <br />
                    <asp:Label CssClass="modValue" ID="Label5" runat="server" Text='<%# EvalNumber(Eval("TotalAmountM")) %>'
                        meta:resourcekey="Label5Resource2"></asp:Label>
                </td>
            </tr>
            <asp:ListView DataKeyNames="ExchangePartDetailId" ID="lvParts" runat="server" DataSource='<%# Eval("ExchangePartDetails") %>'>
                <LayoutTemplate>
                    <tr id="itemPlaceHolder" runat="server" />
                </LayoutTemplate>
                <ItemTemplate>
                    <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
                        <td>
                            <%# Eval("PartCodeO") %><br />
                            <asp:TextBox ID="txtPartCodeM" runat="server" Text='<%# Eval("PartCodeM") %>' meta:resourcekey="txtPartCodeMResource1" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Part Code cannot be blank!"
                                ValidationGroup="UpdateVoucher" ControlToValidate="txtPartCodeM" Text="*" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                            <input id="selSpare" runat="server" alt="#TB_inline?height=500&width=700&inlineId=selectSpare"
                                title="Select Spare" type="button" value="..." onload="selSpare_Load" />
                        </td>
                        <td>
                            <asp:TextBox Width="100%" ID="txtVMEPComment" runat="server" Text='<%# Eval("VMEPComment") %>'
                                meta:resourcekey="txtVMEPCommentResource1" />
                        </td>
                        <td class="number">
                            <asp:TextBox Width="90px" ID="txtUnitPriceM" runat="server" Text='<%# Eval("UnitPriceM") %>'
                                meta:resourcekey="txtUnitPriceMResource1" />
                            <ajaxToolkit:FilteredTextBoxExtender TargetControlID="txtUnitPriceM" FilterType="Numbers"
                                ID="FilteredTextBoxExtender2" runat="server" Enabled="True">
                            </ajaxToolkit:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Unit Price cannot be blank!"
                                ValidationGroup="UpdateVoucher" ControlToValidate="txtUnitPriceM" Text="*" meta:resourcekey="RequiredFieldValidator3Resource1"></asp:RequiredFieldValidator>
                        </td>
                        <td class="number">
                            <%# EvalNumber(Eval("ManPowerO"), 1) %>
                            <asp:TextBox Width="30px" ID="txtManPowerM" runat="server" Text='<%# Eval("ManPowerM") %>'
                                meta:resourcekey="txtManPowerMResource1" />
                            <ajaxToolkit:FilteredTextBoxExtender TargetControlID="txtManPowerM" ValidChars="1234567890,."
                                ID="FilteredTextBoxExtender3" runat="server" Enabled="True">
                            </ajaxToolkit:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Man Power cannot be blank!"
                                ValidationGroup="UpdateVoucher" ControlToValidate="txtManPowerM" Text="*" meta:resourcekey="RequiredFieldValidator4Resource1"></asp:RequiredFieldValidator>
                        </td>
                        <td class="number">
                            <%# EvalNumber(Eval("PartQtyO"))%>
                            <asp:TextBox Width="30px" ID="txtPartQtyM" runat="server" Text='<%# Eval("PartQtyM") %>'
                                meta:resourcekey="txtPartQtyMResource1" />
                            <ajaxToolkit:FilteredTextBoxExtender TargetControlID="txtPartQtyM" FilterType="Numbers"
                                ID="FilteredTextBoxExtender1" runat="server" Enabled="True">
                            </ajaxToolkit:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Quantity cannot be blank!"
                                ValidationGroup="UpdateVoucher" ControlToValidate="txtPartQtyM" Text="*" meta:resourcekey="RequiredFieldValidator2Resource1"></asp:RequiredFieldValidator>
                        </td>
                        <td class="number">
                            <%# EvalNumber(Eval("WarrantySpareAmountO"))%>
                            <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# IsNotSame(Eval("WarrantySpareAmountO"), Eval("WarrantySpareAmountM")) %>'>
                                <br />
                                <asp:Label CssClass="modValue" ID="Label1" runat="server" Text='<%# EvalNumber(Eval("WarrantySpareAmountM")) %>'
                                    meta:resourcekey="Label1Resource2"></asp:Label>
                            </asp:PlaceHolder>
                        </td>
                        <td class="number">
                            <asp:Label CssClass="modValue" ID="Label6" runat="server" Text='<%# EvalNumber(Eval("TotalFeeM")) %>'
                                meta:resourcekey="Label6Resource2"></asp:Label>
                        </td>
                        <td class="number">
                            <%# EvalNumber(Eval("TotalO"))%>
                            <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# IsNotSame(Eval("TotalO"), Eval("TotalM")) %>'>
                                <br />
                                <asp:Label CssClass="modValue" ID="Label2" runat="server" Text='<%# EvalNumber(Eval("TotalM")) %>'
                                    meta:resourcekey="Label2Resource2"></asp:Label>
                            </asp:PlaceHolder>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </EditItemTemplate>
        <ItemTemplate>
            <tr class="group">
                <td colspan="4">
                    <table style="width: 100%">
                        <tr class="group2">
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal1" runat="server" Text="Customer:" meta:resourcekey="Literal1Resource2"></asp:Literal>
                            </td>
                            <td class="normal">
                                <%#Eval("CustomerName")%>
                            </td>
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal2" runat="server" Text="Engine:" meta:resourcekey="Literal2Resource2"></asp:Literal>
                            </td>
                            <td class="normal">
                                <%#Eval("EngineNumber")%>
                            </td>
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal3" runat="server" Text="Model:" meta:resourcekey="Literal3Resource1"></asp:Literal>
                            </td>
                            <td class="normal">
                                <%#Eval("Model")%>
                            </td>
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal4" runat="server" Text="Km:" meta:resourcekey="Literal4Resource1"></asp:Literal>
                            </td>
                            <td class="normal">
                                <%#Eval("KmCount")%>
                            </td>
                        </tr>
                        <tr class="group2">
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal5" runat="server" Text="Purchased:" meta:resourcekey="Literal5Resource1"></asp:Literal>
                            </td>
                            <td class="normal">
                                <%#EvalDate(Eval("PurchaseDate"))%>
                            </td>
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal6" runat="server" Text="Repair:" meta:resourcekey="Literal6Resource1"></asp:Literal>
                            </td>
                            <td class="normal">
                                <%#EvalDate(Eval("ExchangedDate"))%>
                            </td>
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal7" runat="server" Text="Processed:" meta:resourcekey="Literal7Resource1"></asp:Literal>
                            </td>
                            <td class="normal">
                                <%#EvalDate(Eval("LastProcessedDate"))%>
                            </td>
                            <td style="white-space: nowrap">
                                <asp:LinkButton ID="btnEdit" Visible='<%# this.ShowEditButton %>' runat="server"
                                    CommandName="Edit" Text="Edit" meta:resourcekey="btnEditResource1" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr class="group2">
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal9" runat="server" Text="Service:" meta:resourcekey="Literal9Resource1"></asp:Literal>
                            </td>
                            <td class="normal">
                                <asp:HyperLink ID="HyperLink1" runat="server" CssClass="showDetail" Text='<%# Eval("ServiceSheetNumber") %>'
                                    Target="_blank" NavigateUrl='<%# "~/service/WarrantyContent.aspx?srsn=" + Eval("ServiceSheetNumber") %>'
                                    meta:resourcekey="HyperLink1Resource1"></asp:HyperLink>
                            </td>
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal10" runat="server" Text="Exchange:" meta:resourcekey="Literal10Resource1"></asp:Literal>
                            </td>
                            <td class="normal">
                                <asp:HyperLink ID="HyperLink2" runat="server" CssClass="showDetail" Text='<%# Eval("VoucherNumber") %>'
                                    Target="_blank" NavigateUrl='<%# "~/service/WarrantyContent.aspx?pcvn=" + Eval("VoucherNumber") %>'
                                    meta:resourcekey="HyperLink2Resource1"></asp:HyperLink>
                            </td>
                            <td class="right" style="white-space: nowrap">
                                <asp:Literal ID="Literal11" runat="server" Text="Status:" meta:resourcekey="Literal11Resource1"></asp:Literal>
                            </td>
                            <td class="normal">
                                <%#EvalStatus(Eval("Status"))%>
                            </td>
                            <td style="white-space: nowrap">
                                <asp:LinkButton ID="btnApprove" OnClick="btnApprove_Click" runat="server" Text="Approve"
                                    Visible='<%# IsNotSame2(GetApproveedStatus(),Eval("Status")) && this.ShowVerifyButton %>'
                                    CommandArgument='<%# Eval("ExchangePartHeaderId") %>' meta:resourcekey="btnApproveResource1"></asp:LinkButton>
                            </td>
                            <td style="white-space: nowrap">
                                <asp:LinkButton ID="btnReject" OnClick="btnReject_Click" runat="server" Text="Reject"
                                    CausesValidation="true" Visible='<%# IsNotSame2(GetRejectStatus(), Eval("Status")) && this.ShowVerifyButton %>'
                                    CommandArgument='<%# Eval("ExchangePartHeaderId") %>' meta:resourcekey="btnRejectResource1"></asp:LinkButton>
                                <%--<vdms:RequiredOneItemValidator OnDataBinding="rovldComment_DataBinding" ControlToValidate="lvParts" ChildControlToValidate="Literal21"
                                runat="server" ID="rovldComment" ErrorMessage="" Text="*" ValidationGroup="RejectVoucher"></vdms:RequiredOneItemValidator>--%>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="number">
                    <%# EvalNumber(Eval("TotalQuantityO"))%>
                    <br />
                    <asp:Label CssClass="modValue" ID="lbs0" runat="server" Text='<%# EvalNumber(Eval("TotalQuantityM")) %>'
                        meta:resourcekey="lbs0Resource1"></asp:Label>
                </td>
                <td class="number">
                    <%# EvalNumber(Eval("TotalPartCostO"))%>
                    <br />
                    <asp:Label CssClass="modValue" ID="Label3" runat="server" Text='<%# EvalNumber(Eval("TotalPartCostM")) %>'
                        meta:resourcekey="Label3Resource1"></asp:Label>
                </td>
                <td class="number">
                    <%# EvalNumber(Eval("ProposeFeeAmount"))%>
                    <br />
                    <asp:Label CssClass="modValue" ID="Label4" runat="server" Text='<%# EvalNumber(Eval("TotalFeeM")) %>'
                        meta:resourcekey="Label4Resource1"></asp:Label>
                </td>
                <td class="number">
                    <%# EvalNumber(Eval("TotalAmountO"))%>
                    <br />
                    <asp:Label CssClass="modValue" ID="Label5" runat="server" Text='<%# EvalNumber(Eval("TotalAmountM")) %>'
                        meta:resourcekey="Label5Resource1"></asp:Label>
                </td>
            </tr>
            <asp:ListView ID="lvParts" runat="server" DataSource='<%# Eval("ExchangePartDetails") %>'>
                <LayoutTemplate>
                    <tr id="itemPlaceHolder" runat="server" />
                </LayoutTemplate>
                <ItemTemplate>
                    <tr class='<%# Container.DisplayIndex % 2 == 0 ? "even" : "odd" %>'>
                        <td>
                            <%# Eval("PartCodeO") %>
                            <asp:PlaceHolder ID="br0" runat="server" Visible='<%# IsNotSame(Eval("PartCodeO"), Eval("PartCodeM")) %>'>
                                <br />
                                <asp:Label CssClass="modValue" ID="lb0" runat="server" Text='<%# Eval("PartCodeM") %>'
                                    meta:resourcekey="lb0Resource1"></asp:Label>
                            </asp:PlaceHolder>
                        </td>
                        <td>
                            <asp:Label ID="Literal21" runat="server" Text='<%# Eval("VMEPComment")%>'></asp:Label>
                        </td>
                        <td class="number">
                            <%# EvalNumber(Eval("UnitPriceO"))%>
                            <asp:PlaceHolder ID="br2" runat="server" Visible='<%# IsNotSame(Eval("UnitPriceO"), Eval("UnitPriceM")) %>'>
                                <br />
                                <asp:Label CssClass="modValue" ID="lb2" runat="server" Text='<%# EvalNumber(Eval("UnitPriceM")) %>'
                                    meta:resourcekey="lb2Resource1"></asp:Label>
                            </asp:PlaceHolder>
                        </td>
                        <td class="number">
                            <%# EvalNumber(Eval("ManPowerO"),1)%>
                            <asp:PlaceHolder ID="br3" runat="server" Visible='<%# IsNotSame(Eval("ManPowerO"), Eval("ManPowerM")) %>'>
                                <br />
                                <asp:Label CssClass="modValue" ID="lb3" runat="server" Text='<%# EvalNumber(Eval("ManPowerM"),1) %>'
                                    meta:resourcekey="lb3Resource1"></asp:Label>
                            </asp:PlaceHolder>
                        </td>
                        <td class="number">
                            <%# EvalNumber(Eval("PartQtyO"))%>
                            <asp:PlaceHolder ID="br1" runat="server" Visible='<%# IsNotSame(Eval("PartQtyO"), Eval("PartQtyM")) %>'>
                                <br />
                                <asp:Label CssClass="modValue" ID="lb1" runat="server" Text='<%# EvalNumber(Eval("PartQtyM")) %>'
                                    meta:resourcekey="lb1Resource1"></asp:Label>
                            </asp:PlaceHolder>
                        </td>
                        <td class="number">
                            <%# EvalNumber(Eval("WarrantySpareAmountO"))%>
                            <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# IsNotSame(Eval("WarrantySpareAmountO"), Eval("WarrantySpareAmountM")) %>'>
                                <br />
                                <asp:Label CssClass="modValue" ID="Label1" runat="server" Text='<%# EvalNumber(Eval("WarrantySpareAmountM")) %>'
                                    meta:resourcekey="Label1Resource1"></asp:Label>
                            </asp:PlaceHolder>
                        </td>
                        <td class="number">
                            <asp:Label CssClass="modValue" ID="Label6" runat="server" Text='<%# EvalNumber(Eval("TotalFeeM")) %>'
                                meta:resourcekey="Label6Resource1"></asp:Label>
                        </td>
                        <td class="number">
                            <%# EvalNumber(Eval("TotalO"))%>
                            <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# IsNotSame(Eval("TotalO"), Eval("TotalM")) %>'>
                                <br />
                                <asp:Label CssClass="modValue" ID="Label2" runat="server" Text='<%# EvalNumber(Eval("TotalM")) %>'
                                    meta:resourcekey="Label2Resource1"></asp:Label>
                            </asp:PlaceHolder>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </ItemTemplate>
        <EmptyDataTemplate>
            <asp:Literal ID="Literal19" runat="server" Text="No data found!" meta:resourcekey="Literal19Resource1"></asp:Literal>
        </EmptyDataTemplate>
    </asp:ListView>
    <asp:ObjectDataSource ID="odsH" runat="server" SelectMethod="FindExchangeHeaders"
        SelectCountMethod="CountExchangeHeaders" EnablePaging="True" TypeName="VDMS.I.Service.ExchangeVoucherBO"
        UpdateMethod="UpdateVoid" OnSelected="odsH_Selected"></asp:ObjectDataSource>
</div>
<div id="_selectSpare" style="width: 600px" class="hidden">
    <uc:SelectSpare runat="server" ID="selectSpare" />
</div>
