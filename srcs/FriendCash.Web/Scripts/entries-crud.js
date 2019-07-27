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
