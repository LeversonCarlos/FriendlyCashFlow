<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Mobile/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<FriendCash.Model.Supplier>>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
   <style type="text/css">
      .TableCellCode {width:20%; }
      .TableCellDescr {}
   </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="DrawerContent" runat="server">
   <div class="DrawerLeft">
      <%= Html.ActionLink("Back", "Index", "Home", new { }, new { @class = "command" })%>
      <%= Html.ActionLink("New", "New", "Suppliers", new { }, new { @class = "command" }) %>
   </div>
   <div class="DrawerRight">
      <% Html.RenderPartial("SEARCH", new FriendCash.Web.Code.MyModels.Search()); %>
   </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div class="main-form">

      <div class="Table">
         <div class="TableHeader">
            <span class="TableCell TableCellCode">Code</span>
            <span class="TableCell TableCellDescr">Description</span>
         </div>
         <% Html.RenderPartial("List", Model); %>
      </div>

   </div>
</asp:Content>
