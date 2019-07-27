<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Mobile/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<FriendCash.Model.Transfer>>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
   <style type="text/css">
      .TableCellDueDate {width:20%; text-align:center; }
      .TableCellValue {width:23%; text-align:right; }
      .TableCellSettled {width:5%; text-align:center; }
      .TableCellAccountIncome {width:25%; }
      .TableCellAccountExpense {width:25%; }
   </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="DrawerContent" runat="server">
   <div class="DrawerLeft">
      <%= Html.ActionLink("Back", "Index", "Home", new { }, new { @class = "command" })%>
      <%= Html.ActionLink("New", "New", "Transfers", new { }, new { @class = "command" })%>
   </div>
   <div class="DrawerRight">
      <!-- <% Html.RenderPartial("SEARCH", new FriendCash.Web.Code.MyModels.Search() { UseSettled = true }); %> -->
   </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div class="main-form">

      <div class="Table">
         <div class="TableHeader">
            <span class="TableCell TableCellDueDate">Due Date</span>
            <span class="TableCell TableCellValue">Value</span>
            <span class="TableCell TableCellSettled"></span>
            <span class="TableCell TableCellAccount">Income</span>
            <span class="TableCell TableCellAccount">Expense</span>
         </div>
         <% Html.RenderPartial("List", Model); %>
      </div>

   </div>
</asp:Content>
