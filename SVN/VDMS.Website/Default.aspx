<%@ Page Title="Homepage" Language="C#" MasterPageFile="~/MP/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_default" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<%@ Register TagPrefix="cc1" Namespace="VDMS.II.WebControls" %>
<%@ Register Src="Admin/Controls/Quote.ascx" TagName="Quote" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="Server">
    <script type="text/javascript" src="/js/swfobject.js"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            LoadImagePreview();

            var $search = $('#<%= tbsearch.ClientID %>');
            original_val = $search.val();
            $search.focus(function() {
                if ($(this).val() === original_val) {
                    $(this).val('');
                }
            })
	           .keypress(function(event) {
	               if (event.keyCode == '13') {
	                   //$("#bts").click();
	               }
	           });
        });

        function OnClickSearch() {
            if ($("#divsearch").is(":hidden")) {
                $("#divsearch").css("display","block");
            } else {
                $("#divsearch").css("display","none");
            }
        }
        //Edit by mvBinh
        //load flash

        var cacheBuster = "?t=" + Date.parse(new Date());
        // ATTRIBUTES
        var attributes = {};
        attributes.id = 'slide';
        attributes.name = attributes.id;

        var stageW = 600; //"100%";
        var stageH = 250; //"100%";

        // PARAMS
        var params = {};
        params.bgcolor = "#ccc";
        params.wmode = 'transparent';

        /* FLASH VARS */
        var flashvars = {};

        /// if commented / delete these lines, the component will take the stage dimensions defined 
        /// above in "JAVASCRIPT SECTIONS" section or those defined in the settings xml			
        flashvars.componentWidth = stageW;
        flashvars.componentHeight = stageH;

        flashvars.pathToFiles = "flash/";
        flashvars.xmlPath = "xml/banner.xml";


        /** EMBED THE SWF**/

        swfobject.embedSWF("preview.swf" + cacheBuster, attributes.id, stageW, stageH, "9.0.124", "js/expressInstall.swf", flashvars, params);
        //end load flash

        //End mvbinhs
        function LoadImagePreview() {
            $('.imgPre').imgPreview({
                containerID: 'imgPreviewWithStyles',
                imgCSS: {
                    // Limit preview size:
                    height: 200
                },
                // When container is shown:
                onShow: function(link) {
                    // Animate link:
                    $(link).stop().animate({ opacity: 0.4 });
                    // Reset image:
                    $('img', this).stop().css({ opacity: 0 });
                },
                // When image has loaded:
                onLoad: function() {
                    // Animate image
                    $(this).animate({ opacity: 1 }, 300);
                },
                // When container hides: 
                onHide: function(link) {
                    // Animate link:
                    $(link).stop().animate({ opacity: 1 });
                }
            });
        }
        function updated() {
            //  close the popup
            tb_remove();
            //  refresh the update panel so we can view the changes
            $('#<%= this.btrefresh.ClientID %>').click();
            //window.location.reload(true);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c" runat="Server">
    <div id="divLeft" runat="server" class="left-content">
        <div class="box-content" id="slide">
            <p>
                In order to view this object you need Flash Player 9+ support!</p>
            <a href="http://www.adobe.com/go/getflashplayer">
                <img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif"
                    alt="Get Adobe Flash player" />
            </a>
        </div>
        <br />
        <br />
        <div class="grid">
            <div class="title">
                <asp:Literal ID="Literal1x" runat="server" Text="Today's Notice" meta:resourcekey="literal1xResource1"></asp:Literal>
                <%=DateTime.Now.ToShortDateString() %></div>
            <br />
            <vdms:PageGridView ID="gv3" runat="server" Caption="Payment Notice" DataSourceID="odsPayment" AllowPaging="true" PageSize="10"
                meta:resourcekey="gv3Resource1">
                <Columns>
                    <asp:HyperLinkField DataTextField="TipTopNumber" HeaderText="Order No" DataNavigateUrlFields="OrderHeaderId"
                        ItemStyle-HorizontalAlign="Center" DataNavigateUrlFormatString="/Part/Inventory/OrderEdit.aspx?id={0}&action=select"
                        meta:resourcekey="HyperLinkFieldResource1" />
                    <asp:BoundField DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:d}"
                        ItemStyle-HorizontalAlign="Center" meta:resourcekey="BoundFieldResource1" />
                    <asp:BoundField DataField="QuotationDate" HeaderText="Quotation Date" DataFormatString="{0:d}"
                        ItemStyle-HorizontalAlign="Center" meta:resourcekey="BoundFieldResource2" />
                    <%--<asp:BoundField DataField="PaymentSpan" HeaderText="Payment/Over Span" ItemStyle-CssClass="number" />--%>
                    <asp:TemplateField HeaderText="Pay Span" meta:resourcekey="TemplateFieldResource4"
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#Eval("PaymentSpan") %>
                            / <span style="color: Red;">
                                <%#GetOverSpan((DateTime)Eval("QuotationDate"), (int)Eval("PaymentSpan"))%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:C0}"
                        ItemStyle-HorizontalAlign="Center" meta:resourcekey="BoundFieldResource3"></asp:BoundField>
                </Columns>
            </vdms:PageGridView>
            <asp:ObjectDataSource ID="odsPayment" runat="server" TypeName="VDMS.II.PartManagement.Order.OrderDAO" EnablePaging="true"
                SelectMethod="PaymentLate" SelectCountMethod="GetCountPaymentLate"></asp:ObjectDataSource>
            <br />
            <vdms:PageGridView ID="gv4" runat="server" Caption="Receive Notice" DataSourceID="odsReceive" AllowPaging="true" PageSize="10"
                meta:resourcekey="gv4Resource1">
                <Columns>
                    <asp:HyperLinkField DataTextField="IssueNumber" HeaderText="Issue No" DataNavigateUrlFields="VDMSOrderId"
                        DataNavigateUrlFormatString="/Part/Inventory/InStock.aspx?id={0}" meta:resourcekey="HyperLinkFieldResource2"
                        ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="ShippingDate" HeaderText="Shipping Date" DataFormatString="{0:d}"
                        ItemStyle-HorizontalAlign="Center" meta:resourcekey="BoundFieldResource4" />
                    <asp:BoundField DataField="ShippingSpan" HeaderText="Shipping Span" ItemStyle-HorizontalAlign="Center"
                        meta:resourcekey="BoundFieldResource5"></asp:BoundField>
                    <asp:TemplateField HeaderText="Over Span" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Red"
                        meta:resourcekey="TemplateFieldResource5">
                        <ItemTemplate>
                            <%#GetOverSpan((DateTime)Eval("ShippingDate"), (int)Eval("ShippingSpan"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </vdms:PageGridView>
            <asp:ObjectDataSource ID="odsReceive" runat="server" TypeName="VDMS.II.PartManagement.Order.OrderDAO" EnablePaging="true"
                SelectMethod="ReceiveLate" SelectCountMethod="GetCountReceiveLate"></asp:ObjectDataSource>
            <br />
            <asp:UpdatePanel ID="up5" runat="server">
                <ContentTemplate>
                    <vdms:PageGridView ID="gv5" runat="server" DataSourceID="odsSafetyStock" AllowPaging="True"
                        Caption="Parts/Accessory Below Safety Stock" PageSize="5" meta:resourcekey="gv5Resource1">
                        <Columns>
                            <asp:BoundField DataField="PartCode" HeaderText="Part No" meta:resourcekey="BoundFieldResource6" />
                            <asp:BoundField DataField="EnglishName" HeaderText="Part Name" meta:resourcekey="BoundFieldResource7" />
                            <asp:BoundField DataField="SafetyQuantity" HeaderText="Safety Stock" meta:resourcekey="BoundFieldResource8">
                                <ItemStyle CssClass="number" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CurrentStock" HeaderText="On Hand Stock" meta:resourcekey="BoundFieldResource9">
                                <ItemStyle CssClass="number" ForeColor="Red" />
                            </asp:BoundField>
                        </Columns>
                    </vdms:PageGridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:ObjectDataSource ID="odsSafetyStock" runat="server" EnablePaging="True" TypeName="VDMS.II.PartManagement.PartDAO"
                SelectMethod="FindAllSafety" SelectCountMethod="GetSafetyCount"></asp:ObjectDataSource>
        </div>
        <br />
    </div>
    <div id="divRight" runat="server" class="right-content">
        <div class="grid">
            <div class="title">
                <asp:Literal ID="Literal1" runat="server" Text="Latest News" meta:resourcekey="Literal1Resource1"></asp:Literal>
                <a href="javascript:OnClickSearch()" style="float: right; display: block;" title="Search">
                    <img alt="" src="/images/search.png" height="20px" /></a>
            </div>
            <div id="divsearch" style="display: none;overflow: hidden; padding: 10px;">
                <asp:TextBox runat="server" ID="tbsearch" meta:resourcekey="tbsearchResource1"></asp:TextBox>
                <asp:Button runat="server" ID="btsearch" meta:resourcekey="btsearchResource1" />
            </div>
            <asp:ObjectDataSource ID="odsArticleList" runat="server" EnablePaging="True" TypeName="VDMS.II.BasicData.MessageDAO"
                SelectMethod="FindCommonMessage" SelectCountMethod="GetCommonMessageCount" 
                OldValuesParameterFormatString="original_{0}">
                <SelectParameters>
                    <asp:ControlParameter ControlID="tbsearch" Name="filltercontent" 
                        PropertyName="Text" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <vdms:PageGridView ID="gv1" runat="server" DataSourceID="odsArticleList" AllowPaging="True"
                CssClass="table-content" meta:resourcekey="gv1Resource1">
                <Columns>
                    <asp:TemplateField HeaderText="Notice" meta:resourcekey="TemplateFieldResource1">
                        <ItemTemplate>
                            <small style="color: Black; font-weight: bold;">
                                <%#((DateTime)Eval("CreatedDate")).ToShortDateString()%>
                                -
                                <%#Eval("CreatedBy")%>: </small><a class="news thickbox" href='/ShowMessage.aspx?id=<%# Eval("MessageId")%>&TB_iframe=true'
                                    style='<%# ((Char)Eval("Type") == VDMS.II.Entity.MessageType.HotMesssage ) ? "": "background:none;" %>'>
                                    <%# ((Eval("BodyNonHTML").ToString() + ((DateTime)Eval("CreatedDate")).ToShortDateString() + " - " + Eval("CreatedBy").ToString() + ": ").Length > 73) ? Eval("BodyNonHTML").ToString().Substring(0, 70 - (((DateTime)Eval("CreatedDate")).ToShortDateString() + " - " + Eval("CreatedBy").ToString() + ": ").Length) + "..." : Eval("BodyNonHTML")%></a>
                            <br />
                            <asp:Repeater ID="repAttachment" runat="server" DataSource='<%# GetAttachment((long)Eval("MessageId")) %>'>
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlAttachment" runat="server" Text='<%# (string)Eval("Filename") %>'
                                        CssClass='<%# isImage((string)Eval("Filename")) ? "attach-file imgPre": "attach-file" %>'
                                        ToolTip='<%# Bind("Filename") %>' NavigateUrl='<%# string.Concat("~/download.ashx?id=", Eval("FileId")) %>'>
                                    </asp:HyperLink>
                                </ItemTemplate>
                                <SeparatorTemplate>
                                    ,</SeparatorTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <small style="color: Black; font-weight: bold;">
                                <%#((DateTime)Eval("CreatedDate")).ToShortDateString()%>
                                -
                                <%#Eval("CreatedBy")%>: </small><a class="news thickbox" href='/ShowMessage.aspx?id=<%# Eval("MessageId")%>&TB_iframe=true'
                                    style='<%# ((Char)Eval("Type") == VDMS.II.Entity.MessageType.HotMesssage ) ? "": "background:none;" %>'>
                                    <%# ((Eval("BodyNonHTML").ToString() + ((DateTime)Eval("CreatedDate")).ToShortDateString() + " - " + Eval("CreatedBy").ToString() + ": ").Length > 73) ? Eval("BodyNonHTML").ToString().Substring(0, 70 - (((DateTime)Eval("CreatedDate")).ToShortDateString() + " - " + Eval("CreatedBy").ToString() + ": ").Length) + "..." : Eval("BodyNonHTML")%></a>
                            <br />
                            <asp:Repeater ID="repAttachment" runat="server" DataSource='<%# GetAttachment((long)Eval("MessageId")) %>'>
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlAttachment" runat="server" Text='<%# (string)Eval("Filename") %>'
                                        CssClass='<%# isImage((string)Eval("Filename")) ? "attach-file imgPre": "attach-file" %>'
                                        ToolTip='<%# Bind("Filename") %>' NavigateUrl='<%# string.Concat("~/download.ashx?id=", Eval("FileId")) %>'></asp:HyperLink>
                                </ItemTemplate>
                                <SeparatorTemplate>
                                    ,</SeparatorTemplate>
                            </asp:Repeater>
                        </AlternatingItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </vdms:PageGridView>
        </div>
        <br />
        <div class="grid">
            <div class="title">
                <asp:Literal ID="Literal2" runat="server" Text="Messages for:" meta:resourcekey="Literal2Resource1"></asp:Literal>
                <%=VDMS.Helper.UserHelper.DealerName %></div>
            <ajaxToolkit:TabContainer ID="t" runat="server" CssClass="ajax__tab_technorati-theme"
                ActiveTabIndex="0" meta:resourcekey="TabContainer1Resource1">
                <ajaxToolkit:TabPanel ID="t1" runat="server" HeaderText="Inbox" meta:resourcekey="t1Resource2">
                    <ContentTemplate>
                        <div class="grid">
                            <asp:UpdatePanel ID="up2" runat="server">
                                <ContentTemplate>
                                    <asp:LinkButton ID="bt1" runat="server" OnClick="bt1_Click" meta:resourcekey="bt1Resource1"></asp:LinkButton>
                                    <vdms:PageGridView ID="mb1" runat="server" DataSourceID="InboxDataSource" DataKeyNames="MessageBoxId"
                                        PageSize="5" OnRowDataBound="mb1_RowDataBound" AllowPaging="True" meta:resourcekey="mb1Resource1">
                                        <Columns>
                                            <asp:TemplateField meta:resourcekey="TemplateFieldResource2">
                                                <ItemTemplate>
                                                    <small>
                                                        <%#((DateTime)Eval("CreatedDate")).ToShortDateString()%>
                                                        |
                                                        <%#Eval("FromUser")%></small> |
                                                    <asp:Image ID="i1" runat="server" ImageUrl='<%# string.Format("~/Images/{0}", Eval("Image")) %>'
                                                        meta:resourcekey="i1Resource1" />
                                                    <br />
                                                    <%#Eval("Body") %>
                                                    <uc1:Quote ID="Quote1" runat="server" />
                                                    <br />
                                                    <asp:Repeater ID="repAttachment" runat="server" DataSource='<%# GetAttachment((long)Eval("MessageId")) %>'>
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="hlAttachment" runat="server" Text='<%# (string)Eval("Filename") %>'
                                                                ToolTip='<%# Bind("Filename") %>' NavigateUrl='<%# string.Concat("~/download.ashx?id=", Eval("FileId")) %>'></asp:HyperLink>
                                                        </ItemTemplate>
                                                        <SeparatorTemplate>
                                                            ,</SeparatorTemplate>
                                                    </asp:Repeater>
                                                    <div style="text-align: right;">
                                                        <asp:LinkButton ID="l1" runat="server" Text="Delete" CommandName="Delete" meta:resourcekey="l1Resource2"></asp:LinkButton>
                                                        |
                                                        <asp:HyperLink ID="l2" runat="server" Text="Reply" CssClass="thickbox" Visible='<%# (string)Eval("Flag") != "S" %>'
                                                            NavigateUrl='<%# string.Concat("~/QuickReply.aspx?id=", Eval("MessageBoxId"), "&TB_iframe=true&height=320&width=420") %>'
                                                            meta:resourcekey="l2Resource2"></asp:HyperLink>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </vdms:PageGridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="t2" runat="server" HeaderText="Outbox" meta:resourcekey="t2Resource1">
                    <ContentTemplate>
                        <div class="grid">
                            <asp:UpdatePanel ID="up3" runat="server">
                                <ContentTemplate>
                                    <vdms:PageGridView ID="mb2" runat="server" DataSourceID="OutboxDataSource" DataKeyNames="MessageBoxId"
                                        PageSize="5" OnRowDataBound="mb1_RowDataBound" AllowPaging="True" meta:resourcekey="mb2Resource1">
                                        <RowStyle CssClass="box-sub-title border-right" />
                                        <Columns>
                                            <asp:TemplateField meta:resourcekey="TemplateFieldResource3">
                                                <ItemTemplate>
                                                    <small>
                                                        <%#((DateTime)Eval("CreatedDate")).ToShortDateString()%>
                                                        |
                                                        <%#Eval("FromUser")%>
                                                        |
                                                        <%#Eval("ToUser")%></small> |
                                                    <asp:Image ID="i1" runat="server" ImageUrl='<%# string.Format("~/Images/{0}", Eval("Image")) %>'
                                                        meta:resourcekey="i1Resource2" />
                                                    <br />
                                                    <%#Eval("Body") %>
                                                    <uc1:Quote ID="Quote1" runat="server" />
                                                    <br />
                                                    <asp:Repeater ID="repAttachment" runat="server" DataSource='<%# GetAttachment((long)Eval("MessageId")) %>'>
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="hlAttachment" runat="server" Text='<%# (string)Eval("Filename") %>'
                                                                ToolTip='<%# Bind("Filename") %>' NavigateUrl='<%# string.Concat("~/download.ashx?id=", Eval("FileId")) %>'></asp:HyperLink>
                                                        </ItemTemplate>
                                                        <SeparatorTemplate>
                                                            ,</SeparatorTemplate>
                                                    </asp:Repeater>
                                                    <div style="text-align: right;">
                                                        <asp:LinkButton ID="l1" runat="server" Text="Delete" CommandName="Delete" meta:resourcekey="l1Resource3"></asp:LinkButton>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </vdms:PageGridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="t3" runat="server" HeaderText="Compose" meta:resourcekey="t3Resource1">
                    <ContentTemplate>
                        <div class="form">
                            <table width="100%">
                                <cc1:DealerPlaceHolder ID="dph1" runat="server" VisibleBy="VMEP" AdminOnly="False">
                                    <tr>
                                        <td>
                                            <asp:Localize ID="litToDealer" runat="server" Text="To Dealer:" meta:resourcekey="litToDealerResource1"></asp:Localize>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btrefresh" runat="server" OnClick="Refresh_Click" meta:resourcekey="bt1Resource1"
                                                        Style="display: none;" />
                                                    <vdms:PageGridView DataSourceID="odsdl" ID="dl" runat="server" AutoGenerateDeleteButton="true"
                                                        DataKeyNames="DealerCode">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Dealer Code" DataField="DealerCode" meta:resourcekey="BoundFieldDealerCodeResouce1" />
                                                            <asp:BoundField HeaderText="Dealer name" DataField="DealerName" meta:resourcekey="BoundFieldDealerNameResouce1" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <asp:Localize ID="emptysender" runat="server" Text="To Dealer:" meta:resourcekey="emptysenderResource1"></asp:Localize>
                                                        </EmptyDataTemplate>
                                                    </vdms:PageGridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <br />
                                            <asp:HyperLink runat="server" CssClass="thickbox" NavigateUrl="~/ShowDealerList.aspx?TB_iframe=true">
                                                <asp:Localize ID="litAddDealer" runat="server" Text="Add Dealer:" meta:resourcekey="litAddDealerResource1"></asp:Localize></asp:HyperLink>
                                        </td>
                                    </tr>
                                </cc1:DealerPlaceHolder>
                                <tr>
                                    <td>
                                        <asp:Localize ID="litMessage" runat="server" Text="Message:" meta:resourcekey="litMessageResource1"></asp:Localize>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Width="100%" MaxLength="250"
                                            meta:resourcekey="txtMessageResource1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtMessage"
                                            ErrorMessage="*" ValidationGroup="SendMessage" SetFocusOnError="True" meta:resourcekey="rfv1Resource1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Localize ID="litAttachFile" runat="server" Text="Attach file:" meta:resourcekey="litAttachFileResource1"></asp:Localize>
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="fuPM" runat="server" meta:resourcekey="fuPMResource1" />
                                    </td>
                                </tr>
                                <cc1:DealerPlaceHolder ID="DealerPlaceHolder1" runat="server" VisibleBy="Dealer"
                                    AdminOnly="False">
                                    <tr>
                                        <td>
                                            <asp:Localize ID="litDivision" runat="server" Text="Division:" meta:resourcekey="litDivisionResource1"></asp:Localize>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblD" runat="server" RepeatDirection="Horizontal" meta:resourcekey="rblDResource1">
                                                <asp:ListItem Value="VH" Text="Vehicle" Selected="True" meta:resourcekey="ListItemResource1" />
                                                <asp:ListItem Value="SR" Text="Service" meta:resourcekey="ListItemResource2" />
                                                <asp:ListItem Value="SP" Text="Spare Part" meta:resourcekey="ListItemResource3" />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="litPosition" runat="server" Text="Position:" meta:resourcekey="litPositionResource1"></asp:Localize>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblP" runat="server" RepeatDirection="Horizontal" meta:resourcekey="rblPResource1">
                                                <asp:ListItem Value="S" Text="Super Manager" Selected="True" meta:resourcekey="ListItemResource4" />
                                                <asp:ListItem Value="M" Text="Manager" meta:resourcekey="ListItemResource5" />
                                                <asp:ListItem Value="E" Text="Enployee" meta:resourcekey="ListItemResource6" />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </cc1:DealerPlaceHolder>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="cmdSend" runat="server" Text="Send" ValidationGroup="SendMessage"
                                            OnClick="cmdSend_Click" meta:resourcekey="cmdSendResource1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="ActionOk" runat="server" Text="Your message has been sent." Visible="False"
                                            meta:resourcekey="ActionOkResource1"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="t4" runat="server" HeaderText="View Employee's Box" Visible="false"
                    meta:resourcekey="t4Resource1">
                    <ContentTemplate>
                        <div class="form">
                            <table width="100%">
                                <tr>
                                    <td style="width: 125px">
                                        <asp:Localize ID="litViewMessage" runat="server" Text="View Message of:" meta:resourcekey="litViewMessageResource1"></asp:Localize>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlU" runat="server" meta:resourcekey="ddlUResource1">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="bGo" runat="server" Text="View" OnClick="bGo_Click" meta:resourcekey="bGoResource1" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
            </ajaxToolkit:TabContainer>
        </div>
    </div>
    <asp:ObjectDataSource ID="InboxDataSource" runat="server" EnablePaging="True" TypeName="VDMS.II.BasicData.MessageDAO"
        SelectMethod="FindAllPM" SelectCountMethod="GetPMCount" DeleteMethod="DeletePM">
        <SelectParameters>
            <asp:Parameter Name="Position" DefaultValue="I" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="OutboxDataSource" runat="server" EnablePaging="True" TypeName="VDMS.II.BasicData.MessageDAO"
        SelectMethod="FindAllPM" SelectCountMethod="GetPMCount" DeleteMethod="DeletePM">
        <SelectParameters>
            <asp:Parameter Name="Position" DefaultValue="O" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource runat="server" ID="odsdl" TypeName="VDMS.II.BasicData.DealerDAO"
        SelectMethod="FindAll" DeleteMethod="SecsionDealerDelete"></asp:ObjectDataSource>
</asp:Content>
