var koPendingContext = function () {
   var self = this;
   self.isLoading = ko.observable(false);

   // CRUD
   self.CRUD = function () {
      var crudContext = ko.dataFor(document.getElementById("entryModal"));
      crudContext.refresh = function () { self.loadData(); }
      return crudContext;
   }

   // DATA
   self.data = ko.observableArray();
   self.loadData = function () {
      self.data.removeAll();
      self.isLoading(true);
      GetDataJson({
         url: GetActionUrl("Dashboard", "GetPending"),
         success: function (actionBUNDLE) {
            if (actionBUNDLE.Result) {
               var dataEntries = [];
               var dataOverdue = [];
               $.each(actionBUNDLE.Data, function (key, value) {
                  if (value.State == 2) { dataOverdue.push(new koEntryModel(value)); }
                  else { dataEntries.push(new koEntryModel(value)); }
               });
               if (dataOverdue.length != 0) { self.data.push(new koPendingGroupModel(2, dataOverdue)); }
               if (dataEntries.length != 0) { self.data.push(new koPendingGroupModel(1, dataEntries)); }
            }
         },
         done: function () { self.isLoading(false); }
      });
   };

   /* OPEN */
   self.openData = function () {
      var rowData = $(this)[0];
      self.CRUD().openData(rowData);
   };

   // STARTUP EVENT
   self.loadData();
};

function koPendingGroupModel(state, entries) {
   var self = this;
   self.State = ko.observable(state);
   self.Entries = ko.observableArray(entries);
};

