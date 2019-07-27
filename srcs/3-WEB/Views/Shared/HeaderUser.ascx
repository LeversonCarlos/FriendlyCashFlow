<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% if ( MyHelper.User.IsAuthenticated() == true) %>
<% { %>
    <%= Html.ActionLink(MyHelper.User.MyLogin.UserDetails.Description, "Index", "User", new { }, new { })%>
<%  } %>
<% else %>
<% { %>
<%= Html.ActionLink("Login", "Login", "Home", new { }, new { })%>
<%  } %>
