var koEntryContext = function (year, month) {
   var self = this;
   self.isLoading = ko.observable(false);

   /* CRUD */
   self.CRUD = ko.dataFor(document.getElementById("entryModal"));
   self.CRUD.refresh = function () { self.refreshData(); }

   /* DATA */
   self.data = ko.observableArray();
   self.dataUrl = function () {
      var resultUrl = GetActionUrl("Entries", "GetData");
      resultUrl += "?";
      resultUrl += "year=" + self.currentFilter().year + "&";
      resultUrl += "month=" + self.currentFilter().month + "&";
      resultUrl += "account=" + self.currentFilter().account + "&";
      return resultUrl;
   };

   /* REFRESH */
   self.currentFilter = ko.observable();
   self.loadData = function (currentFilter) {
      currentFilter.date = moment(new Date(currentFilter.year, currentFilter.month - 1, 1));
      self.currentFilter(currentFilter);
      self.refreshData();
   };

   /* LOAD */
   self.refreshData = function () {
      if (self.isLoading()) { return; }
      self.isLoading(true);
      self.data.removeAll();
      GetDataJson({
         url: self.dataUrl(),
         success: function (actionBUNDLE) {
            if (actionBUNDLE.Result)
            {
               var dataGroup; var purePending = 0.0;
               $.each(actionBUNDLE.Data, function (key, value) {
                  var date = dateFromJson(value.SearchDate);
                  var day = date.getDate();
                  if (dataGroup == undefined || dataGroup == null || dataGroup.Day() != day) {
                     dataGroup = new koGroupModel(day, date, value.SearchDateFuture);
                     dataGroup.purePending(purePending);
                     self.data.push(dataGroup);
                  }
                  var dataEntry = new koEntryModel(value);
                  dataGroup.pureBalance((value.Balance - purePending));
                  if (dataEntry.State() == 2) { purePending += value.EntryValue; dataGroup.purePending(purePending); }
                  if (dataGroup.State() < dataEntry.State()) { dataGroup.State(dataEntry.State()); }
                  dataGroup.Entries.push(dataEntry);
               });
            }
         },

         done: function () { self.isLoading(false); }
      });
   };

   /* ADD */
   self.addIncome = function () { self.CRUD.addIncome(self.currentFilter().date); };
   self.addExpense = function () { self.CRUD.addExpense(self.currentFilter().date); };
   self.addTransfer = function () { self.CRUD.addTransfer(self.currentFilter().date); };

   /* OPEN */
   self.openData = function () {
      var rowData = $(this)[0];
      self.CRUD.openData(rowData);
   };

   // ACTIVE USER
   self.IsActiveUser = ko.computed(function () { return authIsActiveUser(); }, self);

   /* STARTUP */
   // self.loadData();
};
