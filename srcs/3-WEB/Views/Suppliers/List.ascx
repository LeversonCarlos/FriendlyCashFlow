<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<FriendCash.Model.Supplier>>" %>

<% foreach (var item in Model) { %>
   <a class="TableRow <%: MyHelper.IndexForm.GetCrossHatch(TempData) %>" href="<%: Url.Action("Edit", "Suppliers", new {id=item.idSupplier}) %>" >
      <span class="TableCell TableCellCode"><%: item.Code %></span>
      <span class="TableCell TableCellDescr"><%: item.Description %></span>
   </a>
<% } %>

<% if (MyHelper.IndexForm.HasMorePages(ViewData) == true) {%>
   <div id="<%: MyHelper.IndexForm.GetPaginationTarget(ViewData) %>">
   <% using (Ajax.BeginForm("IndexMore", "Suppliers", MyHelper.IndexForm.GetPaginationParams(ViewData),
             new AjaxOptions
             {
                UpdateTargetId = MyHelper.IndexForm.GetPaginationTarget(ViewData), 
                HttpMethod = "POST", InsertionMode=InsertionMode.Replace 
              })) %>
   <% { %> 
      <div class="confirm"><input type="submit" value="Load More" class="confirm" /></div>
   <% } %>
   </div>
<% } %>

