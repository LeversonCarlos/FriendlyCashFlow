<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Mobile/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="DrawerContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   
<% if (MyHelper.User.IsAuthenticated() == true) %>
<% { %>
   <div class="home-command">
      <%= Html.ActionLink("Suppliers", "Index", "Suppliers", new { page = new Int16?(), search = string.Empty }, new { @class = "command" })%>
      <%= Html.ActionLink("Accounts", "Index", "Accounts", new { page = new Int16?(), search = string.Empty }, new { @class = "command" })%>
      <%= Html.ActionLink("Planning", "Index", "Planning", new { page = new Int16?(), search = string.Empty }, new { @class = "command" })%>
   </div>
   <div class="home-command">
      <%= Html.ActionLink("Incomes", "Index", "Incomes", new { page = new Int16?(), search = string.Empty }, new { @class = "command" })%>
      <%= Html.ActionLink("New", "NewHistory", "Incomes", new { }, new { @class = "command" })%>
   </div>
   <div class="home-command">
      <%= Html.ActionLink("Expenses", "Index", "Expenses", new { page = new Int16?(), search = string.Empty }, new { @class = "command" })%>
      <%= Html.ActionLink("New", "NewHistory", "Expenses", new { }, new { @class = "command" })%>
   </div>
   <div class="home-command">
      <%= Html.ActionLink("Transfers", "Index", "Transfers", new { page = new Int16?(), search = string.Empty }, new { @class = "command" })%>
   </div>
<% } %>

</asp:Content>
