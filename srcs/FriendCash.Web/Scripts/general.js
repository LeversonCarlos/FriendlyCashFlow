function GetActionUrl(controller, action)
{
   return baseActionURL.replace("BaseController", controller).replace("BaseAction", action);
}

/*
var xmlHttp = new XMLHttpRequest();
xmlHttp.onerror = function (e) { console.log("error", e); };
xmlHttp.onreadystatechange = function (e) {
   if (xmlHttp.readyState == 4) {
      console.log({ status: xmlHttp.status, response: xmlHttp.response, e: e });
      if (xmlHttp.status == 200) {
         var actionResponse = JSON.parse(xmlHttp.response);
         options.success(actionResponse);
         options.done();
      }
      else { GetDataJson_Execute(options, tries, response); }
      xmlHttp = null;
   }
};
xmlHttp.open('GET', options.url, options.async);
xmlHttp.send(null);
*/

function GetDataJson(options) {
   var defaultOptions = {
      url: '',
      checkUserToken: true,
      success: function () { },
      done: function () { },
      async: true
   };
   options = $.extend(defaultOptions, options);
   GetDataJson_Execute(options, 0); 
};
function GetDataJson_Execute(options, tries) {
   if (options.checkUserToken && authIsTokenExpired()) {
      refreshAuthToken(function () {
         GetDataJson_Execute(options, tries);
      });
   }
   else {
      $.ajax({
         type: 'GET',
         url: options.url,
         dataType: 'json',
         async: options.async,
         statusCode: {
            200: function (response) { GetDataJson_Result(options, tries, 200, response); },
            400: function (response) { GetDataJson_Result(options, tries, 400, response); },
            401: function (response) { GetDataJson_Result(options, tries, 401, response); },
            403: function (response) { GetDataJson_Result(options, tries, 403, response); },
            404: function (response) { GetDataJson_Result(options, tries, 404, response); },
            422: function (response) { GetDataJson_Result(options, tries, 422, response); },
            500: function (response) { GetDataJson_Result(options, tries, 500, response); }
         }
      });
   }
};
function GetDataJson_Result(options, tries, statusCode, response) {
   if (statusCode == 200) {
      if (response.Result != undefined) { options.success(response); options.done(); return; }
   }

   tries++;
   console.log(options.url, { statusCode, tries, response });
   if (tries >= 5) { options.done(); return; }
   else {
      GetDataJson_Execute(options, tries);
   }
}

function PostDataJson(options) {
   var defaultOptions = {
      url: '',
      data: {},
      checkUserToken: true,
      success: function () { },
      form: null,
      done: function () { },
      async: true
   };
   options = $.extend(defaultOptions, options);
   PostDataJson_Execute(options, 0, null);   
};
function PostDataJson_Execute(options, tries, responseParam) {
   tries++;
   if (tries >= 3) { console.log(responseParam); options.done(); }
   else {
      if (options.checkUserToken && authIsTokenExpired()) {
         refreshAuthToken(function () {
            PostDataJson_Execute(options, tries, responseParam);
         });
      }
      else {
         $.ajax({
            type: 'POST',
            url: options.url,
            data: options.data,
            dataType: 'json',
            async: options.async,
            statusCode: {
               200: function (response) { PostResponse(options, response); options.done(); },
               400: function (response) { PostResponse(options, response); options.done(); },
               401: function (response) { PostDataJson_Execute(options, tries, response); },
               403: function (response) { PostResponse(options, response); options.done(); },
               404: function (response) { PostResponse(options, response); options.done(); },
               422: function (response) { PostResponse(options, response); options.done(); },
               500: function (response) { PostResponse(options, response); options.done(); }
            }
         });
      }
   }
};

function PostResponse(options, response) {
   if (options.form != undefined) {
      options.form.find(".validate").removeClass("invalid").removeClass("valid");
   }
   if (response.Result == undefined && response.Messages == undefined) {
      if (response.responseText != '') {
         ShowMessageResponse(response.responseText);
      }
   }
   else if (response.Result) { options.success(response); options.done(); }
   else {
      $.each(response.Messages, function (key, val) {
         try {
            var jsonMessage = JSON.parse(val.Text);
            if (jsonMessage.ModelState == null) { ShowMessage(jsonMessage.Message, val.Type); }
            else {
               $.each(jsonMessage.ModelState, function (keyState, valState) {
                  PostResponse_State(options, keyState, valState, val.Type);
               });
            }
         }
         catch (e) { ShowMessage(val.Text, val.Type); }
      });
   }
};
function PostResponse_State(options, keyState, valState, msgType) {
   if (!isNaN(parseInt(keyState))) { PostResponse_State(options, valState.Key, valState.Value, msgType); }
   else {
      if (options.form == null) { ShowMessage(keyState + ": " + valState[0], msgType); }
      else {
         if (options.form.find("#" + keyState).length == 0) { 
            var aKeys = keyState.split("."); 
            if (aKeys != undefined && aKeys.length == 2) { keyState = aKeys[1]; }
         }
         options.form.find("label[for='" + keyState + "']").attr("data-error", valState[0]);
         options.form.find("#" + keyState).addClass("invalid");
      }
   }
};
function GetTranslation(key) {
   var result = key;
   GetDataJson({
      url: GetActionUrl("Home", "Translation") + "/" + key + "",
      checkUserToken: false,
      success: function (response) { result = response; },
      async: false
   })
   return result;
};

function dateFromJson(value)
{ return new Date(parseInt(value.substr(6))); }

function dateFormated(value) {
   var dd = value.getDate()
   if (dd < 10) dd = '0' + dd
   var mm = value.getMonth() + 1
   if (mm < 10) mm = '0' + mm
   var yy = value.getFullYear()
   // var yy = value.getFullYear() % 100
   // if (yy < 10) yy = '0' + yy
   return dd + '/' + mm + '/' + yy
}

function dateFormat() {
   return moment("3333-11-22").format("L")
      .replace("3333", "YYYY")
      .replace("11", "MM")
      .replace("22", "DD");
}

function decimalFormated(value) {
   // return Number(value).toFixed(2).replace(".", baseMaskDecimalSeparator);
   return Number(value).toLocaleString(navigator.language, { minimumFractionDigits: 2 });
}

function decimalSeparator() {
   var formatedValue = decimalFormated(1.1);
   return formatedValue.substring(1, 2);
}

function decimalWithouThousandSeparator(formatedValue) {
   if (decimalSeparator() == ',') {
      formatedValue = formatedValue.replace(/[.]/g, '');
   }
   else {
      formatedValue = formatedValue.replace(/[,]/g, '');
   }
   return formatedValue;
}

function decimalParse(formatedValue) {
   if (decimalSeparator() == ',') {
      formatedValue = formatedValue.replace(/[.]/g, '');
      formatedValue = formatedValue.replace(',', '.');
   }
   else {
      formatedValue = formatedValue.replace(/[,]/g, '');
   }
   return parseFloat(formatedValue);
}

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


function refreshAuthToken(doneCallback) {
   GetDataJson({
      url: GetActionUrl("Auth", "CheckToken"),
      checkUserToken: false,
      success: function (responseBundle) {
         if (responseBundle.Result) {
            authUserTicketData = getUserTicketData(responseBundle.Data.UserTicket);
            console.log("refreshAuthToken", authUserTicketData);
            authUserTicketData.refreshTimer = setInterval(function () {
               authUserTicketData.IsTokenExpired = true;
               clearInterval(authUserTicketData.refreshTimer);
               refreshAuthToken();
            }, authUserTicketData.refreshSeconds * 1000);
         }
         else { console.log("refreshAuthToken-error", responseBundle) }
      },
      done: function () {
         if (doneCallback != null) { doneCallback(); }
      }
   });
};

function getUserTicketData(userTicket) {
   if (userTicket == null) { userTicket = { UserName: '', IsActiveUser: false, AccessExpirationSeconds: 1 }; }

   var userTicketData = {
      UserName: userTicket.UserName,
      IsActiveUser: userTicket.IsActiveUser,
      refreshSeconds: userTicket.AccessExpirationSeconds
   };
   userTicketData.IsTokenExpired = (userTicketData.refreshSeconds <= 1);
   userTicketData.refreshTime = moment().add(userTicketData.refreshSeconds, 'seconds').format('L LTS');

   return userTicketData;
};

function authIsTokenExpired() {
   if (authUserTicketData == null) { return true; }
   if (authUserTicketData.IsTokenExpired == null) { return true; }
   return authUserTicketData.IsTokenExpired;
};

function authIsActiveUser() {
   if (authUserTicketData == null) { return false; }
   if (authUserTicketData.IsActiveUser == null) { return false; }
   return authUserTicketData.IsActiveUser;
};