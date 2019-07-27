<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<FriendCash.Model.Planning>>" %>

<% foreach (var item in Model) { %>
   <li class="treeview" >
      <a class="TableRow <%: MyHelper.IndexForm.GetCrossHatch(TempData) %>" href="<%: Url.Action("Edit", "Planning", new {id=item.idPlanning}) %>" >
         <span class="TableCell TableCellDescr treeview <%: MyHelper.Planning.GetTypeClass(item.Type) %>"><%: item.Description %></span>
      </a>
      <% IEnumerable<FriendCash.Model.Planning> oChilds = item.Childs; %>
      <% if (oChilds != null && oChilds.Count() != 0) {%>
         <ol class="treeview">
            <% Html.RenderPartial("List", oChilds); %>
         </ol>
      <%} %>
   </li>
<% } %>
