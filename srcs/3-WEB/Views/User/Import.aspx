<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Mobile/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="DrawerContent" runat="server">
   <%= Html.ActionLink("Back", "Index", "User", new { }, new { @class = "command" })%>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div class="main-form">

      <% using (Html.BeginForm("Import", "User", new { },  FormMethod.Post,
            new { id = "ImportFormName", name = "ImportFormName", target = "ImportFormTarget", enctype = "multipart/form-data" })) %>
      <% { %>

         <ul class="steps-list">
            <li>Generate an excel file with the data to be imported, following this template.</li>
            <li><label><input type="checkbox" id="ImportClear" name="ImportClear" />Clear current data before importing (isnt working)</label></li>
            <li>
               <span class="upload-wrapper" >
                  <input class="upload-file" type="file" name="FileData" onchange='<%= "import_send(this)" %>' />
                  <label>Select the file to be imported clicking here</label>
               </span>
               <iframe id="<%= "ImportFormTarget" %>" style="display:none;"></iframe>
            </li>
         </ul>

      <%  } %>

      <script type="text/javascript">
         function import_send(sender) {
            sender.form.submit();
         }
      </script>

   </div>
</asp:Content>
