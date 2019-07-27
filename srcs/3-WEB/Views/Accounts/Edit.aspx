<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Mobile/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<FriendCash.Model.Account>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="DrawerContent" runat="server">
   <%= Html.ActionLink("Back", "Index", "Accounts", new { }, new { @class = "command" })%>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div class="main-form">

      <% using (Html.BeginForm("Edit", "Accounts", FormMethod.Post)) {%>
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
           
         <div class="editor-row editor-row0">
            <div class="editor-label"><%: Html.LabelFor(model => model.Type) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.Type, "*")%></div>
            <div class="editor-field"><%: Html.DropDownListFor(model => model.Type, new SelectList(Enum.GetValues(typeof(FriendCash.Model.Account.enType)))) %></div>
         </div>
           
         <div class="editor-row editor-row1">
            <div class="editor-label"><%: Html.LabelFor(model => model.DueDay) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.DueDay, "*")%></div>
            <div class="editor-field"><%: Html.TextBoxFor(model => model.DueDay)%></div>
         </div>
           
         <div class="editor-row editor-row0">
            <div class="editor-label"><%: Html.LabelFor(model => model.Status) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.Status, "*")%></div>
            <div class="editor-field"><%: Html.DropDownListFor(model => model.Status, new SelectList(Enum.GetValues(typeof(FriendCash.Model.Account.enStatus))))%></div>
         </div>
           
         <div class="editor-row">
            <%: Html.HiddenFor(model => model.idRow) %>
            <%: Html.HiddenFor(model => model.idAccount) %>
            <div class="confirm">
               <input type="submit" value="Save" class="confirm" />
            </div>
         </div>   

      </div>
      <% } %>

    </div>
</asp:Content>
