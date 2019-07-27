/* { Message = 0, Information = 1, Warning = 2, Alert = 3 } */
function ShowMessage(text, type) {
   var toastBody = text;
   var toastIcon = "";
   if (type == 1) { toastIcon = "info"; }
   else if (type == 2) { toastIcon = "warning"; }
   else if (type == 3) { toastIcon = "cancel"; }
   if (toastIcon != "") {
      toastBody = "<span class='valign-wrapper'>" + "<i class='material-icons valign'>" + toastIcon + "</i>" + text + "</span>"      
   }
   Materialize.toast(toastBody, 5000);
};

function ShowMessageResponse(responseText) {
   var alertID = 'alert-' + Math.floor(Math.random() * 10000);
   var alertHTML = "";
   alertHTML += "<div id='" + alertID + "' class='modal'>";
   alertHTML += " <div class='modal-content'>" + responseText + "</div>";
   alertHTML += " <div class='modal-footer'>"
   alertHTML += "  <a href='#!' class=' modal-action modal-close waves-effect waves-green btn-flat'>X</a>"
   alertHTML += " </div>"
   alertHTML += "</div>"
   $('body').append(alertHTML);
   $('#' + alertID).modal('open');
};

var fullScreenHeight = -1; var fullScreenWidth = -1;
function getFullScreenHeight() {
   getFullScreenSize();
   return fullScreenHeight;
};
function getFullScreenWidth() {
   getFullScreenSize();
   return fullScreenWidth;
};
function getFullScreenSize() {
   if (fullScreenHeight == -1 || fullScreenWidth == -1) {
      $("body").addClass("hide");
      $("html").css("height", "100%");
      $("html").css("width", "100%");

      fullScreenHeight = $("html").height();
      fullScreenWidth = $("html").width();

      $("html").css("height", "");
      $("html").css("width", "");
      $("body").removeClass("hide");
   }
   return fullScreenHeight;
};

