function koEntryModel(value) {
   var self = this;
   self.idEntry = ko.observable(value.idEntry);
   self.Text = ko.observable(value.Text);
   self.Type = ko.observable(value.Type);
   self.idCategory = ko.observable(value.idCategory);
   self.idCategoryText = ko.observable(value.idCategoryText);
   self.pureEntryValue = ko.observable(Math.abs(value.EntryValue));
   self.EntryValue = ko.observable(decimalFormated(self.pureEntryValue()));
   self.EntryValuePositive = ko.observable(value.EntryValue>=0)
   self.Balance = ko.observable(decimalFormated(value.Balance));

   // ACCOUNT
   var idAccountFirstLoad = true;
   self.idAccount = ko.observable(value.idAccount);
   self.idAccountText = ko.observable(value.idAccountText);
   self.idAccountRelatedData = ko.observable();
   self.idAccountRelatedData.subscribe(function (val) {
      if (idAccountFirstLoad) { idAccountFirstLoad = false; if (value.idAccount != 0) { return; } }
      var relatedData = self.idAccountRelatedData();
      if (relatedData == null) { return; }
      var jsonValue = JSON.parse(relatedData.jsonValue);
      if (jsonValue.Type != accountTypeCreditCard) { return; }
      if (jsonValue.ClosingDay == null || jsonValue.ClosingDay == 0) { return; }
      if (jsonValue.DueDay == null || jsonValue.DueDay == 0) { return; }
      var nowDay = moment(); 
      var closingDay = moment().date(jsonValue.ClosingDay); 
      var dueDay = moment().date(jsonValue.DueDay); 
      if (closingDay < nowDay) { dueDay.add(1, 'M'); } 
      if (dueDay < nowDay) { dueDay.add(1, 'M'); } 
      self.DueDate(dueDay.format('L'));
   }, self);

   // STATE
   self.State = ko.observable(value.State);
   self.StateIcon = ko.computed(function () {
      if (self.State() == 1) { return ""; }
      else if (self.State() == 2) { return "warning"; }
      else { return ""; }
   }, self);

   self.pureDueDate = ko.observable(moment(value.DueDate));
   self.DueDate = ko.observable(self.pureDueDate().format('L'));
   self.purePayDate = ko.observable((value.PayDate == null ? null : moment(value.PayDate)));
   self.PayDate = ko.observable((self.purePayDate() == null ? '' : self.purePayDate().format('L')));

   self.SearchDate = ko.observable(value.SearchDate);
   self.SearchDateDay = ko.computed(function () { return dateFromJson(self.SearchDate()).getDate() }, self);
   self.SearchDateFormated = ko.computed(function () { return dateFormated(dateFromJson(self.SearchDate())) }, self);

   self.Paid = ko.observable(value.Paid);
   self.Paid.subscribe(function (val) {
      if (!val) { self.PayDate(''); }
      else if (val && self.PayDate() == '') {
         var nowDay = moment();
         var dueDay = moment(self.DueDate(), dateFormat());
         if (nowDay < dueDay) { self.PayDate(nowDay.format('L')); }
         else { self.PayDate(dueDay.format('L')); }
      }
   }, self);

   self.idTransfer = ko.observable(value.idTransfer);
   self.idEntryIncome = ko.observable(value.idEntryIncome);
   self.idAccountIncome = ko.observable(value.idAccountIncome);
   self.idAccountIncomeText = ko.observable(value.idAccountIncomeText);
   self.idEntryExpense = ko.observable(value.idEntryExpense);
   self.idAccountExpense = ko.observable(value.idAccountExpense);
   self.idAccountExpenseText = ko.observable(value.idAccountExpenseText);

   self.idPattern = ko.observable(value.idPattern);

   self.idRecurrency = ko.observable(value.idRecurrency);
   self.idRecurrencyView = ko.observable(new koRecurrencyModel(value.idRecurrencyView));

};

function koRecurrencyModel(value) {
   var self = this;
   if (value == null) { value = { idRecurrency: 0, Type: 2, Fixed: true, Quantity: '', Update: 0, hasRecurrency: false } }
   self.idRecurrency = ko.observable(value.idRecurrency);
   self.Type = ko.observable(value.Type);
   self.Fixed = ko.observable(value.Fixed);
   self.Fixed.subscribe(function (val) {
      if (!val) { self.Quantity(''); }
   }, self);
   self.Quantity = ko.observable(value.Quantity);
   self.Update = ko.observable(value.Update);
   self.hasRecurrency = ko.observable(value.hasRecurrency);
};

function koGroupModel(day, date, searchDateFuture) {
   var self = this;
   self.Day = ko.observable(day);
   self.Date = ko.observable(date);
   self.FormatedDate = ko.observable(day + ', ' + moment(date).format('dddd'));
   self.searchDateFuture = ko.observable(searchDateFuture);

   // BALANCE
   self.pureBalance = ko.observable(0);
   self.positiveBalance = ko.computed(function () { return self.pureBalance() >= 0 }, self);
   self.Balance = ko.computed(function () { return decimalFormated(self.pureBalance()); }, self);

   // PENDING
   self.purePending = ko.observable(0);
   self.positivePending = ko.computed(function () { return self.purePending() >= 0 }, self);
   self.Pending = ko.computed(function () { return decimalFormated(self.purePending()); }, self);

   // STATE
   self.State = ko.observable(0);
   self.StateIcon = ko.computed(function () {
      if (self.State() == 1) { return ""; }
      else if (self.State() == 2) { return "warning"; }
      else { return ""; }
   }, self);

   self.Entries = ko.observableArray([]);
};