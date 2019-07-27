<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Mobile/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<FriendCash.Model.Planning>>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
   <style type="text/css">
      .TableCellDescr {width:98%; }
   </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="DrawerContent" runat="server">
   <div class="DrawerLeft">
      <%= Html.ActionLink("Back", "Index", "Home", new { }, new { @class = "command" })%>
      <%= Html.ActionLink("New Expense", "NewExpense", "Planning", new { }, new { @class = "command" })%>
      <%= Html.ActionLink("New Income", "NewIncome", "Planning", new { }, new { @class = "command" })%>
   </div>
   <div class="DrawerRight">
      <% Html.RenderPartial("SEARCH", new FriendCash.Web.Code.MyModels.Search()); %>
   </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div class="main-form">

      <div class="Table">
         <div class="TableHeader">
            <span class="TableCell TableCellDescr">Description</span>
         </div>
         <% Html.RenderPartial("List", Model); %>
      </div>

   </div>
</asp:Content>
