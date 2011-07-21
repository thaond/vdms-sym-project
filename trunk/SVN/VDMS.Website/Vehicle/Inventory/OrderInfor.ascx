<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OrderInfor.ascx.cs" Inherits="Vehicle_Inventory_OrderInfor" %>
<a href="ProcessOrder.aspx?OrderId=<%=_OrderId %>&oqDF=<%=OQ_DateFrom %>&oqON=<%=OQ_OrderNumber %>&oqDT=<%=OQ_DateTo %>&oqDL=<%=OQ_Dealer %>&oqAI=<%=OQ_AreaIndex %>&oqSI=<%=OQ_StatusIndex %>">
    <span style="color: Red;"><%=_DealerName %>(<%=_DealerCode %>)</span> | 
    <span style="color: Red;"><%=((DateTime)_OrderDate).ToShortDateString() %></span> | 
    <span style="color: Red;"><%=_OrderTimes %></span> (<%=_Status %>)</a> | 
<span style="color:Maroon; font-weight:bold;"><%=_OrderNumber %></span> |
<a href="../Report/PrintOrder.aspx?oid=<%=_OrderId %>" target="_blank"><img alt="<%=_print %>" src="../../Images/Print.gif" title="<%=_print %>" border="0" style="vertical-align: middle;" /></a>
<br /><span style="color:Green;"><%= !string.IsNullOrEmpty(_SecondaryAddress) ? _SecondaryAddress : _ShippingTo%></span>
<br /><span style="color:Blue;"><%=_Comment %></span>
