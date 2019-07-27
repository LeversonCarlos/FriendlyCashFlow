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