<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Mobile/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<FriendCash.Model.History>>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
   <style type="text/css">
      .TableCellDueDate {width:20%; text-align:center; }
      .TableCellValue {width:23%; text-align:right; }
      .TableCellSettled {width:5%; text-align:center; }
      .TableCellPayDate {width:20%; text-align:center; }
      .TableCellAccount {width:30%; }
   </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="DrawerContent" runat="server">
   <div class="DrawerLeft">
      <% string sController = this.Request.RequestContext.RouteData.Values["controller"].ToString(); %>
      <% long iID = long.Parse(this.Request.RequestContext.RouteData.Values["id"].ToString()); %>
      <%= Html.ActionLink("Back", "Index", sController, new { }, new { @class = "command" })%>
      <%= Html.ActionLink("New", "NewHistory", sController, new { id = iID }, new { @class = "command" })%>
   </div>
   <div class="DrawerRight">
   </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div class="main-form">

      <div class="Table">
         <div class="TableHeader">
            <span class="TableCell TableCellDueDate">Due Date</span>
            <span class="TableCell TableCellValue">Value</span>
            <span class="TableCell TableCellSettled"></span>
            <span class="TableCell TableCellPayDate">Pay Date</span>
            <span class="TableCell TableCellAccount">Account</span>
         </div>
         <% Html.RenderPartial("~/Views/History/List.ascx", Model); %>
      </div>

   </div>
</asp:Content>
