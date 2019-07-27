<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Mobile/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<FriendCash.Model.History>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="DrawerContent" runat="server">
   <% string sController = this.Request.RequestContext.RouteData.Values["controller"].ToString(); %>
   <%= Html.ActionLink("Back", "Index", sController, new { }, new { @class = "command" })%>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div class="main-form">

      <% string sController = this.Request.RequestContext.RouteData.Values["controller"].ToString(); %>
      <% using (Html.BeginForm("EditHistory", sController, FormMethod.Post)) {%>
      <div class="editor-form">

         <div class="editor-row editor-row0">
            <div class="editor-label"><%: Html.LabelFor(model => model.idDocument)%></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.idDocument, "*")%></div>
            <div class="editor-field">
               <% Html.RenderPartial("AutoComplete", 
                  new AutoComplete() 
                  {
                     Controller = sController,
                     ControlValue = "idDocument",
                     ContentValue = Model.idDocument.ToString(),
                     ContentDescr = (Model.DocumentDetails == null ? "" : Model.DocumentDetails.Description),
                     RelatedValue = "idDocument",
                     RelatedDescr = "Description"
                   }); %>
            </div>
         </div>

         <div class="editor-row editor-row1">
            <div class="editor-label"><%: Html.LabelFor(model => model.DueDate) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.DueDate, "*") %></div>
            <div class="editor-field"><%: Html.TextBoxFor(model => model.DueDate) %></div>
         </div>
           
         <div class="editor-row editor-row0">
            <div class="editor-label"><%: Html.LabelFor(model => model.Value) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.Value, "*") %></div>
            <div class="editor-field"><%: Html.TextBoxFor(model => model.Value) %></div>
         </div>
           
         <div class="editor-row editor-row1">
            <div class="editor-label"><%: Html.LabelFor(model => model.Settled) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.Settled, "*")%></div>
            <div class="editor-field"><%: Html.CheckBoxFor(model => model.Settled)%></div>
         </div>
           
         <div class="editor-row editor-row0">
            <div class="editor-label"><%: Html.LabelFor(model => model.PayDate) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.PayDate, "*")%></div>
            <div class="editor-field"><%: Html.TextBoxFor(model => model.PayDate)%></div>
         </div>
           
         <div class="editor-row editor-row1">
            <div class="editor-label"><%: Html.LabelFor(model => model.idAccount) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.idAccount, "*") %></div>
            <div class="editor-field">
               <% Html.RenderPartial("AutoComplete", 
                  new AutoComplete() 
                  {
                     Controller = "Accounts",
                     ControlValue = "idAccount",
                     ContentValue = Model.idAccount.ToString(),
                     ContentDescr = (Model.AccountDetails == null ? "" : Model.AccountDetails.Description),
                     RelatedValue = "idAccount",
                     RelatedDescr = "Description"
                   }); %>
            </div>
         </div>

         <div class="editor-row">
            <%: Html.HiddenFor(model => model.idRow) %>
            <%: Html.HiddenFor(model => model.idHistory) %>
            <%: Html.HiddenFor(model => model.Type) %>
            <div class="confirm">
               <input type="submit" value="Save" class="confirm" />
            </div>
         </div>   

      </div>
      <% } %>

      <script type="text/javascript">
         $(document).ready(function () {
            $("#DueDate").mask("00/00/0000");
            $("#Value").mask("000000000,00", { reverse: true });
            $("#PayDate").mask("00/00/0000");
         });
      </script>

    </div>
</asp:Content>

