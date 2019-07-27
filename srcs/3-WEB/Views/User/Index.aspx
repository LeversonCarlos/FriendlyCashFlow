<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Mobile/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="DrawerContent" runat="server">
   <%= Html.ActionLink("Back", "Index", "Home", new { }, new { @class = "command" })%>
</asp:Content>


<asp:Content ContentPlaceHolderID="MainContent" runat="server">

   <div class="home-command">
      <%= Html.ActionLink("Import", "Import", "User", new { }, new { @class = "command" })%>
   </div>

</asp:Content>
