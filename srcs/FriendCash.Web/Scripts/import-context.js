var koImportContext = function () {
   var self = this;
   self.isLoading = ko.observable(false);

   // DATA
   self.data = ko.observableArray();
   self.loadData = function () {
      self.isLoading(true);
      self.data.removeAll();
      GetDataJson({
         url: GetActionUrl("Configs", "GetImportData"),
         success: function (actionBUNDLE) {
            if (actionBUNDLE.Result) {
               $.each(actionBUNDLE.Data, function (key, value) { self.data.push(new koImportModel(value)); });
            }
         },
         done: function () { self.isLoading(false) }
      });
   };

   // IMPORT
   self.importClick = function () {
      var importFile = document.getElementById("importFile");
      var actionData = new FormData();
      actionData.append("image", importFile.files[0]);

      $.ajax({
         type: 'POST',
         url: GetActionUrl("Configs", "PostImportData"),
         data: actionData,
         dataType: 'json',
         processData: false,
         contentType: false,
         statusCode: {
            200: function (response) {
               var waitTimer = setInterval(function () {
                  clearInterval(waitTimer);
                  self.loadData();
               }, 1000);
            }
         }
      });

   };

   // STARTUP EVENT
   self.loadData();
}