<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Mobile/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<FriendCash.Model.Document>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="DrawerContent" runat="server">
   <% string sController = this.Request.RequestContext.RouteData.Values["controller"].ToString(); %>
   <%= Html.ActionLink("Back", "Index", sController, new { }, new { @class = "command" })%>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div class="main-form">

      <% string sController = this.Request.RequestContext.RouteData.Values["controller"].ToString(); %>
      <% using (Html.BeginForm("Edit", sController, FormMethod.Post)) {%>
      <div class="editor-form">

         <div class="editor-row editor-row0">
            <div class="editor-label"><%: Html.LabelFor(model => model.Description) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.Description, "*") %></div>
            <div class="editor-field"><%: Html.TextBoxFor(model => model.Description) %></div>
         </div>
           
         <div class="editor-row editor-row1">
            <div class="editor-label"><%: Html.LabelFor(model => model.idSupplier) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.idSupplier, "*") %></div>
            <div class="editor-field">
               <% Html.RenderPartial("AutoComplete", 
                  new AutoComplete() 
                  {
                     Controller = "Suppliers", ControlValue = "idSupplier",
                     ContentValue = Model.idSupplier.ToString(), ContentDescr = (Model.SupplierDetails == null ? "" : Model.SupplierDetails.Description), 
                     RelatedValue="idSupplier", RelatedDescr="Description"
                   }); %>
            </div>
         </div>

         <div class="editor-row editor-row0">
            <div class="editor-label"><%: Html.LabelFor(model => model.idPlanning) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.idPlanning, "*")%></div>
            <div class="editor-field"><%= MyHelper.Planning.PlanningDropDown("idPlanning", Model.idPlanning, ViewData["PlanningTree"])%></div>
         </div>
           
         <div class="editor-row">
            <%: Html.HiddenFor(model => model.idRow) %>
            <%: Html.HiddenFor(model => model.idDocument) %>
            <div class="confirm">
               <input type="submit" value="Save" class="confirm" />
            </div>
         </div>   

      </div>
      <% } %>

    </div>
</asp:Content>

