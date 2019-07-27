<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<FriendCash.Model.Document>>" %>

<% string sController = this.Request.RequestContext.RouteData.Values["controller"].ToString(); %>

<% foreach (var item in Model) { %>
   <div class="TableRow <%: MyHelper.IndexForm.GetCrossHatch(TempData) %>" >
      <a href="<%: Url.Action("History", sController, new { id = item.idDocument, page = new Int16?() }) %>" >
         <span class="TableCell TableCellItems">ITEMS</span>
      </a>
      <a href="<%: Url.Action("Edit", sController, new {id=item.idDocument}) %>" >
         <span class="TableCell TableCellDescr"><%: item.Description %></span>
         <span class="TableCell TableCellSupplier"><%: item.SupplierDetails.Description %></span>
         <span class="TableCell TableCellPlanning"><%: item.PlanningDetails.Description %></span>
      </a>
   </div>
<% } %>

<% if (MyHelper.IndexForm.HasMorePages(ViewData) == true) {%>
   <div id="<%: MyHelper.IndexForm.GetPaginationTarget(ViewData) %>">
   <% using (Ajax.BeginForm("IndexMore", sController, MyHelper.IndexForm.GetPaginationParams(ViewData),
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
