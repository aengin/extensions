﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Signum.Web" %>
<%@ Import Namespace="Signum.Entities.Extensions.Basics" %>

<% using (var s = Html.TypeContext<DateSpanDN>()) { %>
    <table id='datespan'>
        <tr>
            <td><%Html.ValueLine(s, e => e.Years, vl => vl.ValueHtmlProps.Add("size", 3)); %></td>
            <td><%Html.ValueLine(s, e => e.Months, vl => vl.ValueHtmlProps.Add("size", 3)); %></td>
            <td><%Html.ValueLine(s, e => e.Days, vl => vl.ValueHtmlProps.Add("size", 3)); %></td>
        </tr>
    </table> 
<% } %>