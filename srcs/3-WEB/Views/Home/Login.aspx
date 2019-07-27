<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Mobile/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="DrawerContent" runat="server">
   <%= Html.ActionLink("Back", "Index", "Home", new { }, new { @class = "command" })%>
   <%= Html.ActionLink("Register", "Register", "Home", new { }, new { @class = "command" })%>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div class="main-form">

      <% FriendCash. Model.User oModel = new FriendCash.Model.User(); %>
      <% using (Html.BeginForm()) {%>
      <div class="editor-form">

         <div class="editor-row editor-row0">
            <div class="editor-label"><%: Html.LabelFor(model => oModel.Code) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => oModel.Code, "*") %></div>
            <div class="editor-field"><%: Html.TextBoxFor(model => oModel.Code) %></div>
         </div>
         
         <div class="editor-row editor-row1">
            <div class="editor-label"><%: Html.LabelFor(model => oModel.Password) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => oModel.Password, "*") %></div>
            <div class="editor-field"><%: Html.PasswordFor(model => oModel.Password) %></div>
         </div>   
         
         <div class="editor-row">
            <%: Html.HiddenFor(model => oModel.idRow) %>
            <div class="confirm">
               <input type="submit" value="Login" class="confirm" />
            </div>
         </div>   

      </div>
      <% } %>

    </div>
</asp:Content>

