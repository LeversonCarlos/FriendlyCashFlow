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

var koEntryCrud = function (year, month) {
   var self = this;
   self.isLoading = ko.observable(false);
   self.refresh = null;


   /* ADD */
   self.addIncome = function (paramDate) { self.addData(entryTypeIncome, paramDate); };
   self.addExpense = function (paramDate) { self.addData(entryTypeExpense, paramDate); };
   self.addTransfer = function (paramDate) { self.addData(entryTypeTransfer, paramDate); };
   self.addData = function (entryType, paramDate) {

      // DEFAULT DATE
      var todayDate = moment();
      // var paramDate = moment(self.filter().DateGet());
      if (paramDate == undefined || paramDate == null) { paramDate = todayDate; }
      else {
         if (paramDate.year() == todayDate.year() && paramDate.month() == todayDate.month()) { paramDate = todayDate; }
      }
      var defaultDate = '/Date(' + paramDate.toDate().getTime() + ')/';

      // MODEL
      var newModel = {
         'idEntry': 0,
         'Text': '', 'Type': entryType,
         'EntryValue': 0,
         'idCategory': 0, 'idAccount': 0,
         'DueDate': defaultDate, 'SearchDate': defaultDate
      };

      // TRANSFER
      if (entryType == entryTypeTransfer) {
         newModel.idTransfer = '';
         newModel.Text = msgTextEntryTypeTransfer;
         newModel.idEntryIncome = 0;
         newModel.idAccountIncome = 0;
         newModel.idEntryExpense = 0;
         newModel.idAccountExpense = 0;
      }

      // SELECT DATA
      self.selectData(new koEntryModel(newModel));
   };


   /* OPEN */
   self.openData = function (rowData) {

      // CAST TO NEW INSTANCE [SO UNDO SHOULD WORK]
      var jsonData = ko.toJS(rowData);
      jsonData.EntryValue = jsonData.pureEntryValue;
      jsonData.DueDate = jsonData.pureDueDate;
      jsonData.PayDate = jsonData.purePayDate;
      var modelData = new koEntryModel(jsonData);

      // SELECT DATA
      self.selectData(modelData);
   };


   /* SELECT */
   self.selectData = function (val) {
      if (val.idEntry() < 0) { return; }
      self.selectedData(val);
      self.editForm
         .modal({
            startingTop: '0.5vh',
            endingTop: '3vh',
            ready: function () {
               self.editFormLoad();
            },
            show: true
         });
   };


   /* SAVE */
   self.saveDataCurrent = function () {
      self.saveData();
   };
   self.saveDataFutures = function () {
      self.selectedData().idRecurrencyView().Update(1); /*Futures*/
      self.saveData();
   };
   self.saveData = function () {
      self.isLoading(true);
      PostDataJson({
         url: GetActionUrl("Entries", "SaveData"),
         data: self.selectedDataJson(),
         success: function (actionBUNDLE) {
            if (self.refresh != null) { self.refresh(); }
            self.editForm.modal('close');
         },
         form: self.editForm,
         done: function () { self.isLoading(false); }
      });
   };


   /* REMOVE */
   self.removeData = function () {
      self.isLoading(true);
      PostDataJson({
         url: GetActionUrl("Entries", "RemoveData"),
         data: self.selectedDataJson(),
         success: function (actionBUNDLE) {
            if (self.refresh != null) { self.refresh(); }
            self.editForm.modal('close');
         },
         form: self.editForm,
         done: function () { self.isLoading(false); }
      });
   };


   /* SELECTED DATA */
   self.selectedData = ko.observable();
   self.selectedDataJson = function () {
      var jsonData = ko.toJS(self.selectedData);
      jsonData.DueDate = moment(jsonData.DueDate, dateFormat()).toDate();
      jsonData.PayDate = moment(jsonData.PayDate, dateFormat()).toDate();
      jsonData.SearchDate = dateFromJson(jsonData.SearchDate);
      jsonData.EntryValue = decimalWithouThousandSeparator(jsonData.EntryValue);
      jsonData = JSON.parse(JSON.stringify(jsonData));
      return jsonData;
   };


   /* FORM */
   self.editForm = $('#entryModal');
   self.editFormLoad = function () {
      var val = self.selectedData();

      // CONTROLS
      $('select').material_select();
      $("#entryModal .dropdown-button").dropdown({ constrainWidth: false, belowOrigin: true, alignment: 'right' });
      $.each($(".apply-date-picker"), function (k, v) { $(v).datepicker({}) });

      // TYPE
      var relatedType = "\"\"";
      if (val.Type() == entryTypeIncome) { relatedType = "\"Income\""; }
      else if (val.Type() == entryTypeExpense) { relatedType = "\"Expense\""; }

      // APPLY TYPE TO CATEGORY 
      var categoryElement = document.getElementById("idCategory"); 
      var categoryContext = ko.dataFor(categoryElement);
      categoryContext.relatedFilter(relatedType);

      // APPLY TYPE TO PATTERN 
      var patternElement = document.getElementById("idPattern");
      var patternContext = ko.dataFor(patternElement);
      patternContext.inputTextDelayed.subscribe(function (inputText) { val.idPattern(0); val.Text(inputText); });
      patternContext.inputValue.subscribe(function (inputVal) {
         var inputData = patternContext.inputData();
         if (inputData == undefined || inputData == null) { return; }
         var inputModel = JSON.parse(inputData.jsonValue);

         val.idCategory(inputModel.idCategory);
         categoryContext.relatedRefresh();
      });
      patternContext.relatedFilter(relatedType);
      // patternElement.focus(); AUTO-FOCUS DOENST ACTIVATE RELATED EDITOR. ONLY THE CLICK DOESIT

   };

   // ACTIVE USER
   self.IsActiveUser = ko.computed(function () { return authIsActiveUser(); }, self);

};

var koEntryFilterContext = function (initialYear, initialMonth, initialAccount) {
   var self = this;


   /* DATA */
   self.data = ko.observable();
   self.loadData = function (year, month, account) {
      PostDataJson({
         url: GetActionUrl("Entries", "GetInterval"),
         data: { Year: year, Month: month, Account: account },
         success: function (param) {
            if (param.Result) {
               self.data(new koEntryFilterModel(param.Data));
               $(".dropdown-button").dropdown({ constrainWidth: false });
               self.notifyRefresh();
            }
         },
         done: function () { }
      });
   }


   /* CURRENT FILTER */
   self.currentFilterYear = function () { return self.data().FilterCurrentMonth().Year(); }
   self.currentFilterMonth = function () { return self.data().FilterCurrentMonth().Month(); }
   self.currentFilterAccount = function () { return self.data().FilterCurrentAccount().ID(); }
   self.currentFilter = function () {
      return {
         year: self.currentFilterYear(),
         month: self.currentFilterMonth(),
         account: self.currentFilterAccount()
      };
   };


   /* REFRESH NOTIFY */
   self.notifyControl = ko.observable(0);
   self.notifyRefresh = function () {
      self.notifyControl(self.notifyControl() + 1);
   };
   self.notifyDelayed = ko
      .pureComputed(self.notifyControl)
      .extend({ rateLimit: { method: "notifyWhenChangesStop", timeout: 1000 } });
   self.notifyDelayed
      .subscribe(function (val) {
         self.refreshData();
      }, self);


   /* REFRESH DATA */
   self.refreshData = function () {
      var refreshContext = ko.dataFor(document.getElementById("koEntry"));
      if (refreshContext == null) {
         var timer = setInterval(function () {
            clearInterval(timer);
            self.refreshData();
         }, 500);
      }
      else { refreshContext.loadData(self.currentFilter()); }
   };


   /* CLICK */
   self.monthClick = function (pData) {
      self.loadData(pData.Year, pData.Month, self.currentFilterAccount());
   }
   self.accountClick = function (pData) {
      self.loadData(self.currentFilterYear(), self.currentFilterMonth(), pData.ID);
   }


   /* STARTUP EVENT */
   self.loadData(initialYear, initialMonth, initialAccount);
};

function koEntryFilterModel(value, filterContext) {
   var self = this;
   self.FilterPreviousMonth = ko.observable(new koEntryFilterMonthModel(value.PreviousMonth, filterContext));
   self.FilterCurrentMonth = ko.observable(new koEntryFilterMonthModel(value.CurrentMonth, filterContext));
   self.FilterNextMonth = ko.observable(new koEntryFilterMonthModel(value.NextMonth, filterContext));

   self.FilterCurrentAccount = ko.observable(new koEntryFilterAccountModel(value.CurrentAccount));
   self.FilterAccountList = ko.observableArray();
   $.each(value.AccountList, function (k, v) { self.FilterAccountList.push(new koEntryFilterAccountModel(v)); });
};

function koEntryFilterMonthModel(value, filterContext) {
   var self = this;
   self.Text = ko.observable(value.Text);
   self.Year = ko.observable(value.Year);
   self.Month = ko.observable(value.Month);

   self.monthClick = function (pData) {
      filterContext.monthClick(pData);
   }

};

function koEntryFilterAccountModel(value) {
   var self = this;
   self.ID = ko.observable(value.ID);
   self.Text = ko.observable(value.textValue);
};

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