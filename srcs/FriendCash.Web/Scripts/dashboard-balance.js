var koBalanceContext = function () {
   var self = this;
   self.isLoading = ko.observable(false);

   // DATA
   self.data = ko.observableArray([]);
   self.loadData = function () {
      self.data.removeAll();
      self.isLoading(true);
      GetDataJson({
         url: GetActionUrl("Dashboard", "GetBalance"),
         success: function (actionBUNDLE) {
            if (actionBUNDLE.Result) {
               $.each(actionBUNDLE.Data, function (key, value) { self.data.push(new koBalanceGroupModel(value)); });
            }
         },
         done: function () { self.isLoading(false); }
      });
   };

   // STARTUP EVENT
   self.loadData();
};

function koBalanceGroupModel(value) {
   var self = this;
   self.Type = ko.observable(value.Type);
   self.Text = ko.observable(value.Text);

   self.CurrentBalance = ko.observable(decimalFormated(value.CurrentBalance));
   self.positiveCurrentBalance = ko.observable(value.CurrentBalance >= 0)

   self.PendingBalance = ko.observable(decimalFormated(value.PendingBalance));
   self.positivePendingBalance = ko.observable(value.PendingBalance >= 0)

   // ACCOUNTS
   self.Accounts = ko.observableArray();
   $.each(value.Accounts, function (k, v) { self.Accounts.push(new koBalanceModel(v)); });

};

function koBalanceModel(value) {
   var self = this;
   self.idAccount = ko.observable(value.idAccount);
   self.Text = ko.observable(value.Text);
   self.Type = ko.observable(value.Type);
   self.ClosingDay = ko.observable(value.ClosingDay);
   self.DueDay = ko.observable(value.DueDay);
   self.DueDate = ko.observable(moment(value.DueDateText).format("DD/MMM"));

   self.itemClick = function (e) {
      window.location = GetActionUrl("Entries", "Index") + "/" + self.idAccount() + "/";
   }

   self.InitialBalance = ko.observable(value.InitialBalance);

   self.CurrentBalance = ko.observable(decimalFormated(value.CurrentBalance));
   self.positiveCurrentBalance = ko.observable(value.CurrentBalance >= 0)

   self.PendingBalance = ko.observable(decimalFormated(value.PendingBalance));
   self.positivePendingBalance = ko.observable(value.PendingBalance >= 0)
   self.hasPendingBalance = ko.computed(function () { return value.PendingBalance != 0; }, self);
};

