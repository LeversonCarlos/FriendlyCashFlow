<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Mobile/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<FriendCash.Model.Document>>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
   <style type="text/css">
      .TableCellItems {width:5%; }
      .TableCellDescr {width:55%; }
      .TableCellSupplier {width:19%; }
      .TableCellPlanning {width:19%; }
   </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="DrawerContent" runat="server">
   <div class="DrawerLeft">
      <% string sController = this.Request.RequestContext.RouteData.Values["controller"].ToString(); %>
      <%= Html.ActionLink("Back", "Index", "Home", new { }, new { @class = "command" })%>
      <%= Html.ActionLink("New", "New", sController, new { }, new { @class = "command" })%>
   </div>
   <div class="DrawerRight">
      <% Html.RenderPartial("SEARCH", new FriendCash.Web.Code.MyModels.Search() { UseSettled = true }); %>
   </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div class="main-form">

      <div class="Table">
         <div class="TableHeader">
            <span class="TableCell TableCellItems"></span>
            <span class="TableCell TableCellDescr">Description</span>
            <span class="TableCell TableCellSupplier">Supplier</span>
            <span class="TableCell TableCellPlanning">Planning</span>
         </div>
         <% Html.RenderPartial("~/Views/Documents/List.ascx", Model); %>
      </div>

   </div>
</asp:Content>
