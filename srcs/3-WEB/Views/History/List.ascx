<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<FriendCash.Model.History>>" %>

<% string sController = this.Request.RequestContext.RouteData.Values["controller"].ToString(); %>
<% long iID = long.Parse(this.Request.RequestContext.RouteData.Values["id"].ToString()); %>

<% foreach (var item in Model) { %>
   <div class="TableRow <%: MyHelper.IndexForm.GetCrossHatch(TempData) %>" >
      <a href="<%: Url.Action("EditHistory", sController, new {id=item.idHistory}) %>" >
         <span class="TableCell TableCellDueDate"><%: string.Format("{0:dd/MM/yyyy}", item.DueDate) %></span>
         <span class="TableCell TableCellValue"><%: string.Format("{0:0.00}", item.Value) %></span>
         <span class="TableCell TableCellSettled"><%: Html.CheckBox("Settled", item.Settled, new { @disabled = "disabled" })%></span>
         <span class="TableCell TableCellPayDate"><%: string.Format("{0:dd/MM/yyyy}", item.PayDate) %></span>
         <span class="TableCell TableCellAccount"><%: (item.Settled==true? item.AccountDetails.Description:"") %></span>
      </a>
   </div>
<% } %>

<% if (MyHelper.IndexForm.HasMorePages(ViewData) == true) {%>
   <div id="<%: MyHelper.IndexForm.GetPaginationTarget(ViewData) %>">
   <% using (Ajax.BeginForm("HistoryMore", sController, MyHelper.IndexForm.GetPaginationParams(ViewData, iID),
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
