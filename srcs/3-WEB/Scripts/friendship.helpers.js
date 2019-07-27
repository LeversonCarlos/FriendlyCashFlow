$(document).ready(function () {

   // HOOK SAVE BUTTON CLICK EVENT
   $('body').on('click', '.SaveButtonHooked', function (e) {
      var formName = $('.SaveButtonHooked').attr("data-form-name");
      var formObject = $("#" + formName);
      if (formName == undefined) { formObject = $('.SaveButtonHooked').closest("form"); };
      var formAction = formObject.attr('action');
      var formData = formObject.serialize();
      FormPost(formAction, formData);
      return false;
   });

   // HOOK MODAL HIDE
   $("body").on('hidden.bs.modal', '.modal', function () {
      $(".modal").remove();
      WaitHide();
   });

   // HOOK POPUP EDIT
   $(".modal-edit").click(function () {
      WaitShow();
      var formURL = $(this).attr("data-action");
      $.get(formURL, null, function (formResponse) {
         WaitHide();
         $('body').append(formResponse);
         $("#MyPopup").modal({
            show: true,
            backdrop: true,
            keyboard: true
         });
      }).fail(function (e) { WaitHide(); ShowMessageException("Error", e.responseText, "lg"); });
   });

});

function FormPost(formAction, formData) {
   WaitShow();
   $.post(formAction, formData, function (formResponse) {
      var formResult = JSON.parse(JSON.stringify(formResponse));
      if (formResult.OK == false) { WaitHide(); ShowMessage(formResult); }
      if (formResult.OK == true) { window.location = formResult.Redirect; }
   }).fail(function (e) { WaitHide(); ShowMessageException("Error", e.responseText, "lg"); });
}

function WaitShow() {
   $("html").css("cursor", "progress");
   $("body").append("<div class='modal-backdrop fade in'></div>");
}
function WaitHide() {
   $(".modal-backdrop").remove();
   $("html").css("cursor", "default");
}


function ShowMessage(formResult) {
   if (formResult.Exceptions != null && formResult.Exceptions.length != 0) { ShowMessageException('Error', formResult.Exceptions); }
   if (formResult.Warnings != null && formResult.Warnings.length != 0) { ShowMessageWarning('Warning', formResult.Warnings); }
   if (formResult.Informations != null && formResult.Informations.length != 0) { ShowMessageInformation('Information', formResult.Informations); }
}

function ShowMessageWarning(title, messages) {
   ShowMessageAlert(title, messages, "warning");
}

function ShowMessageInformation(title, messages) {
   ShowMessageAlert(title, messages, "info");
}

function ShowMessageAlert(title, messages, type) {
   var sHTML = "";
   sHTML += "<div id='My" + type + "' class='alert alert-" + type + " alert-dismissable affix-top col-sm-6 col-xs-10'>";
   sHTML += " <button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>";
   $.each(messages, function () {
      sHTML += "  <p class='text-" + type + "'>" + this + "</p>";
   })
   sHTML += "</div>";
   $('body').append(sHTML);
   $('#My' + type).alert();
   $('#My' + type).on('closed.bs.alert', function () {
      $('#My' + type).remove();
   });
}

function ShowMessageException(title, messages, modalSize) {
   ShowModal("MyException", title, messages, modalSize);
}

function ShowModal(name, title, messages, modalSize, confirmButton) {
   var sHTML = "";
   sHTML += "<div id='" + name + "' class='modal-signature modal fade' tabindex='-1' role='dialog' aria-labelledby='" + name + "Title' aria-hidden='false'>";
   sHTML += " <div class='modal-dialog" + (modalSize != "" && modalSize != undefined ? " modal-" + modalSize : "") + "'>";
   sHTML += "  <div class='modal-content'>";

   sHTML += "   <div class='modal-header'>";
   sHTML += "    <button type='button' class='close' data-dismiss='modal' aria-hidden='true'>&times;</button>";
   sHTML += "    <h4 class='modal-title' id='" + name + "Title'>" + title + "</h4>";
   sHTML += "   </div>";

   sHTML += "   <div class='modal-body'>";
   if (typeof (messages) == "string") {
      sHTML += "    <p>" + messages + "</p>";
   }
   else {
      $.each(messages, function () {
         sHTML += "    <p class='text-danger'>" + this + "</p>";
      })
   }
   sHTML += "   </div>";

   if (confirmButton != null) {
      sHTML += "   <div class='modal-footer'>";
      sHTML += "    <button id='" + confirmButton.Name + "' type='button' class='btn btn-" + confirmButton.Color + "' >";
      sHTML += "     <span class='glyphicon glyphicon-" + confirmButton.Icon + "'></span>"
      sHTML += "     <span class='hidden-xs'> " + confirmButton.Text + "</span>";
      sHTML += "    </button>";
      sHTML += "   </div>";
   }

   sHTML += "  </div>";
   sHTML += " </div>";
   sHTML += "</div>";

   $('body').append(sHTML);
   $("#" + name).modal({
      show: true,
      backdrop: true,
      keyboard: true
   });
   return true;
}