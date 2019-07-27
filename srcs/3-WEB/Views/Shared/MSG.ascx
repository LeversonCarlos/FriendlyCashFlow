<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% if (ViewData["MSG"] != null) { %>

   <% List<string> oMSGs = ((List<string>)ViewData["MSG"]); %>
   <% foreach (var oMSG in oMSGs)  %>
    <% { %> <span><%: oMSG + ". " %></span> <% } %>

<% } %>
