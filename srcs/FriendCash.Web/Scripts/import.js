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
function koImportModel(value) {
   var self = this;
   self.idImport = ko.observable(value.idImport);
   self.State = ko.observable(value.State);

   self.Accounts = ko.observable(value.Accounts);
   self.ImportedAccounts = ko.observable(value.ImportedAccounts);

   self.Categories = ko.observable(value.Categories);
   self.ImportedCategories = ko.observable(value.ImportedCategories);

   self.Entries = ko.observable(value.Entries);
   self.ImportedEntries = ko.observable(value.ImportedEntries);

   self.RowDate = ko.observable(value.RowDate);
   self.RowDateHumanized = ko.computed(function () { return moment.duration(moment(self.RowDate()).diff(moment())).humanize() });

   // REFRESH
   self.refreshTimer = null;
   self.refreshData = function (doneCallback) {
      GetDataJson({
         url: GetActionUrl("Configs", "GetImportData") + "\\" + self.idImport() + "",
         success: function (actionBUNDLE) {
            if (actionBUNDLE.Result) {
               var refreshModel = actionBUNDLE.Data;
               self.State(refreshModel.State);
               self.Accounts(refreshModel.Accounts);
               self.ImportedAccounts(refreshModel.ImportedAccounts);
               self.Categories(refreshModel.Categories);
               self.ImportedCategories(refreshModel.ImportedCategories);
               self.Entries(refreshModel.Entries);
               self.ImportedEntries(refreshModel.ImportedEntries);
            }
         },
         done: function () { doneCallback(); }
      });

   };
   self.refresh = function () {
      if (self.refreshTimer != null) { window.clearInterval(self.refreshTimer); self.refreshTimer = null; }
      self.refreshData(function () {
         if (self.State() == 0 || self.State() == 1) {
            self.refreshTimer = window.setInterval(function () { self.refresh(); }, 250);
         };
      });

   };
   if (self.State() == 0 || self.State() == 1) {
      self.refresh();
   };

};