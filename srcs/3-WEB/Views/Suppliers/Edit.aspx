<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Mobile/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<FriendCash.Model.Supplier>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="DrawerContent" runat="server">
   <%= Html.ActionLink("Back", "Index", "Suppliers", new { }, new { @class = "command" })%>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div class="main-form">

      <% using (Html.BeginForm("Edit", "Suppliers", FormMethod.Post)) {%>
      <div class="editor-form">

         <div class="editor-row editor-row0">
            <div class="editor-label"><%: Html.LabelFor(model => model.Code) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.Code, "*") %></div>
            <div class="editor-field"><%: Html.TextBoxFor(model => model.Code) %></div>
         </div>

         <div class="editor-row editor-row1">
            <div class="editor-label"><%: Html.LabelFor(model => model.Description) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.Description, "*")%></div>
            <div class="editor-field"><%: Html.TextBoxFor(model => model.Description) %></div>
         </div>
           
         <div class="editor-row">
            <%: Html.HiddenFor(model => model.idRow) %>
            <%: Html.HiddenFor(model => model.idSupplier) %>
            <div class="confirm">
               <input type="submit" value="Save" class="confirm" />
            </div>
         </div>   

      </div>
      <% } %>

    </div>
</asp:Content>

