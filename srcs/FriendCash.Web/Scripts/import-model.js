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