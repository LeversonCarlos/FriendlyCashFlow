<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FriendCash.Web.Code.MyModels.Search>" %>

<% using (Html.BeginForm()) { %>
   <%: Html.HiddenFor(model => model.UseSettled) %>
   <% if (Model.UseSettled == true) {%>
      <%= MyHelper.IndexForm.BoxedCheckBoxFor("ValueSettled", Model.ValueSettled, "Settled")%>
   <% } %>
   <%: Html.TextBoxFor(model => model.ValueSearch, new { @class = "search" })%>
   <input type="submit" value="Search" />
<% } %>

