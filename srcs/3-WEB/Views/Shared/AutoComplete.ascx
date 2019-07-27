<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FriendCash.Web.Code.MyModels.AutoComplete>" %>
<input id="<%= Model.ControlValue %>" name="<%= Model.ControlValue %>" value="<%= Model.ContentValue %>" type="hidden" />
<input id="<%= Model.ControlDescr %>" name="<%= Model.ControlDescr %>" value="<%= Model.ContentDescr %>" type="text" />

<script type="text/javascript">
   $(document).ready(function () {
      $('<%= "#" + Model.ControlDescr %>').autocomplete({
         source: function (request, response) {
            $.ajax({
               url: '<%=Url.Action("AutoComplete", Model.Controller) %>',
               type: "POST",
               dataType: "json",
               data: { term: request.term },
               success: <%= Model.TagSuccess %>
            })
         },
         select: function (e, ui) {
            $('<%= "#" + Model.ControlValue %>').val(ui.item.id);
         },
         messages:
         {
            noResults: "", results: ""
         }
      });
   })
</script>