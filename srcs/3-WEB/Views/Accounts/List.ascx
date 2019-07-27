<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<FriendCash.Model.Account>>" %>

<% foreach (var item in Model) { %>
   <a class="TableRow <%: MyHelper.IndexForm.GetCrossHatch(TempData) %>" href="<%: Url.Action("Edit", "Accounts", new {id=item.idAccount}) %>" >
      <span class="TableCell TableCellCode"><%: item.Code %></span>
      <span class="TableCell TableCellDescr"><%: item.Description %></span>
      <span class="TableCell TableCellType"><%: item.Type %></span>
      <span class="TableCell TableCellStatus"><%: item.Status %></span>
   </a>
<% } %>

<% if (MyHelper.IndexForm.HasMorePages(ViewData) == true) {%>
   <div id="<%: MyHelper.IndexForm.GetPaginationTarget(ViewData) %>">
   <% using (Ajax.BeginForm("IndexMore", "Accounts", MyHelper.IndexForm.GetPaginationParams(ViewData),
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

