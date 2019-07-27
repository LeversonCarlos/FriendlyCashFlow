var koCategoryContext = function () {
   var self = this;
   self.isLoading = ko.observable(false);
   self.isSaving = ko.observable(false);

   // CATEGORY TYPE
   self.categoryType = function () {
      var selectedTab = $("li.tab a.active").attr("href");
      if (selectedTab == "#tabExpense") { return categoryTypeExpense; }
      else { return categoryTypeIncome; }
   };

   // DATA
   self.dataIncome = ko.observableArray([]);
   self.dataExpense = ko.observableArray([]);

   // LOAD 
   self.loadData = function () {
      self.isLoading(true);
      self.dataIncome.removeAll();
      self.dataExpense.removeAll();
      GetDataJson({
         url: GetActionUrl("Categories", "GetData"),
         success: function (actionBUNDLE) {
            if (actionBUNDLE.Result) {
               $.each(actionBUNDLE.Data.Income, function (key, value) { self.dataIncome.push(new koCategoryModel(value)); });
               $.each(actionBUNDLE.Data.Expense, function (key, value) { self.dataExpense.push(new koCategoryModel(value)); });
            }
         },
         done: function () { self.isLoading(false) }
      });
   };

   // ADD
   self.addData = function () {
      self.selectData(new koCategoryModel({ 'idCategory': -1, 'Text': '', 'Type': self.categoryType(), 'idParentRow': 0 }));
   };

   // OPEN
   self.openData = function () {
      var rowData = $(this)[0];
      var jsonData = ko.toJS(rowData);
      // jsonData.EntryValue = jsonData.pureEntryValue;
      var modelData = new koCategoryModel(jsonData);
      self.selectData(modelData);
      // self.selectData($(this)[0]);
   };

   // SELECT
   self.selectedData = ko.observable();
   self.selectData = function (val) {
      self.selectedData(val);
      $('#editModal').modal({
         ready: function () {
            $('select').material_select();

            // APPLY TYPE TO CATEGORY RELATED
            var elParentRow = document.getElementById("idParentRow");
            var koParentRow = ko.dataFor(document.getElementById("idParentRow"));
            if (val.Type() == categoryTypeIncome) { koParentRow.relatedFilter("\"Income\""); }
            else if (val.Type() == categoryTypeExpense) { koParentRow.relatedFilter("\"Expense\""); }
            else { koParentRow.relatedFilter("\"\""); }

         },
         show: true
      });
   };
   self.selectedDataJson = function () {
      return JSON.parse(JSON.stringify(ko.toJS(self.selectedData)));
   };

   // SAVE
   self.saveData = function () {
      self.isSaving(true);
      PostDataJson({
         url: GetActionUrl("Categories", "SaveData"),
         data: self.selectedDataJson(),
         success: function (actionBUNDLE) {
            // self.selectedData().TypeValue(self.selectedData().Type());            
            self.loadData();
            $('#editModal').modal('close');
         },
         form: $('#editModal'),
         done: function () { self.isSaving(false) }
      });
   };

   // REMOVE
   self.removeData = function () {
      self.isSaving(true);
      PostDataJson({
         url: GetActionUrl("Categories", "RemoveData"),
         data: self.selectedDataJson(),
         success: function (actionBUNDLE) {
            self.loadData();
            $('#editModal').modal('close');
         },
         form: $('#editModal'),
         done: function () { self.isSaving(false) }
      });
   };

   // ACTIVE USER
   self.IsActiveUser = ko.computed(function () { return authIsActiveUser(); }, self);

   // STARTUP EVENT
   self.loadData();
};
