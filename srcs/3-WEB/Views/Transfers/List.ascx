<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<FriendCash.Model.Transfer>>" %>

<% foreach (var item in Model) { %>
   <div class="TableRow <%: MyHelper.IndexForm.GetCrossHatch(TempData) %>" >
      <a href="<%: Url.Action("Edit", "Transfers", new {id=item.idTransfer}) %>" >
         <span class="TableCell TableCellDueDate"><%: string.Format("{0:dd/MM/yyyy}", item.DueDate) %></span>
         <span class="TableCell TableCellValue"><%: string.Format("{0:0.00}", item.Value) %></span>
         <span class="TableCell TableCellSettled"><%: Html.CheckBox("Settled", item.Settled, new { @disabled = "disabled" })%></span>
         <span class="TableCell TableCellAccountIncome"><%: (item.Settled==true? item.AccountIncomeDetails.Description:"") %></span>
         <span class="TableCell TableCellAccountExpense"><%: (item.Settled == true ? item.AccountExpenseDetails.Description : "")%></span>
      </a>
   </div>
<% } %>

<% if (MyHelper.IndexForm.HasMorePages(ViewData) == true) {%>
   <div id="<%: MyHelper.IndexForm.GetPaginationTarget(ViewData) %>">
   <% using (Ajax.BeginForm("IndexMore", "Transfers", MyHelper.IndexForm.GetPaginationParams(ViewData),
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
