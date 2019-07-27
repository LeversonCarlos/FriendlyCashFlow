<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Mobile/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<FriendCash.Model.Transfer>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="DrawerContent" runat="server">
   <%= Html.ActionLink("Back", "Index", "Transfers", new { }, new { @class = "command" })%>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div class="main-form">

      <% using (Html.BeginForm("Edit", "Transfers", FormMethod.Post)) {%>
      <div class="editor-form">

         <div class="editor-row editor-row0">
            <div class="editor-label"><%: Html.LabelFor(model => model.DueDate) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.DueDate, "*") %></div>
            <div class="editor-field"><%: Html.TextBoxFor(model => model.DueDate) %></div>
         </div>
           
         <div class="editor-row editor-row1">
            <div class="editor-label"><%: Html.LabelFor(model => model.Value) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.Value, "*") %></div>
            <div class="editor-field"><%: Html.TextBoxFor(model => model.Value) %></div>
         </div>
           
         <div class="editor-row editor-row0">
            <div class="editor-label"><%: Html.LabelFor(model => model.Settled) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.Settled, "*")%></div>
            <div class="editor-field"><%: Html.CheckBoxFor(model => model.Settled)%></div>
         </div>
           
         <div class="editor-row editor-row1">
            <div class="editor-label"><%: Html.LabelFor(model => model.PayDate) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.PayDate, "*")%></div>
            <div class="editor-field"><%: Html.TextBoxFor(model => model.PayDate)%></div>
         </div>
           
         <div class="editor-row editor-row0">
            <div class="editor-label"><%: Html.LabelFor(model => model.idAccountIncome) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.idAccountIncome, "*") %></div>
            <div class="editor-field">
               <% Html.RenderPartial("AutoComplete", 
                  new AutoComplete() 
                  {
                     Controller = "Accounts",
                     ControlValue = "idAccountIncome",
                     ContentValue = Model.idAccountIncome.ToString(),
                     ContentDescr = (Model.AccountIncomeDetails == null ? "" : Model.AccountIncomeDetails.Description),
                     RelatedValue = "idAccount",
                     RelatedDescr = "Description"
                   }); %>
            </div>
         </div>

         <div class="editor-row editor-row1">
            <div class="editor-label"><%: Html.LabelFor(model => model.idAccountExpense) %></div>
            <div class="editor-validation"><%: Html.ValidationMessageFor(model => model.idAccountExpense, "*")%></div>
            <div class="editor-field">
               <% Html.RenderPartial("AutoComplete", 
                  new AutoComplete() 
                  {
                     Controller = "Accounts",
                     ControlValue = "idAccountExpense",
                     ContentValue = Model.idAccountExpense.ToString(),
                     ContentDescr = (Model.AccountExpenseDetails == null ? "" : Model.AccountExpenseDetails.Description),
                     RelatedValue = "idAccount",
                     RelatedDescr = "Description"
                   }); %>
            </div>
         </div>

         <div class="editor-row">
            <%: Html.HiddenFor(model => model.idRowIncome) %>
            <%: Html.HiddenFor(model => model.idRowExpense) %>
            <%: Html.HiddenFor(model => model.idTransfer) %>
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

