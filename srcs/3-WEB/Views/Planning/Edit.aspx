<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Mobile/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<FriendCash.Model.Planning>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="DrawerContent" runat="server">
   <%= Html.ActionLink("Back", "Index", "Planning", new { }, new { @class = "command" })%>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div class="main-form">

      <% using (Html.BeginForm("Edit", "Planning", FormMethod.Post)) {%>
      <div class="editor-form">

         <div class="editor-row editor-row0">
            <div class="editor-label"><%: Html.LabelFor(model => model.Description) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.Description, "*")%></div>
            <div class="editor-field"><%: Html.TextBoxFor(model => model.Description)%></div>
         </div>

         <div class="editor-row editor-row1">
            <div class="editor-label"><%: Html.LabelFor(model => model.Type) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.Type, "*")%></div>
            <div class="editor-field"><%: Html.TextBoxFor(model => model.Type, new {@readonly="true" })%></div>
            <!--
            <div class="editor-field"><%: Html.DropDownListFor(model => model.Type, new SelectList(Enum.GetValues(typeof(FriendCash.Model.Document.enType))), new { @readonly = true })%></div>
            -->
         </div>
           
         <div class="editor-row editor-row0">
            <div class="editor-label"><%: Html.LabelFor(model => model.idParentRow) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.idParentRow, "*")%></div>
            <div class="editor-field"><%= MyHelper.Planning.PlanningDropDown("idParentRow", Model.idParentRow, ViewData["PlanningTree"])%></div>
         </div>

         <div class="editor-row">
            <%: Html.HiddenFor(model => model.idRow) %>
            <%: Html.HiddenFor(model => model.idPlanning) %>
            <div class="confirm">
               <input type="submit" value="Save" class="confirm" />
            </div>
         </div>   

      </div>
      <% } %>

    </div>
</asp:Content>
