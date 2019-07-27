var koAccountContext = function () {
   var self = this;
   self.isLoading = ko.observable(false);
   self.isSaving = ko.observable(false);

   // JUST ACTIVE
   self.justActive = ko.observable(true);

   // DATA
   self.data = ko.observableArray([]);
   self.dataFiltered = ko.computed(function () {
      return $.grep(self.data(), function (e) { return e.Active() == true || !self.justActive(); })
   }, self);

   // LOAD 
   self.loadData = function () {
      self.isLoading(true);
      self.data.removeAll();
      GetDataJson({
         url: GetActionUrl("Accounts", "GetData"),
         success: function (actionBUNDLE) {
            if (actionBUNDLE.Result) {
               $.each(actionBUNDLE.Data, function (key, value) { self.data.push(new koAccountModel(value)); });
            }
         },
         done: function () { self.isLoading(false) }
      });
   };

   // ADD
   self.addData = function () {
      self.selectData(new koAccountModel({ 'idAccount': -1, 'Text': '', 'TypeValue': 0, 'Type': 0, 'ClosingDay': '', 'DueDay': '', 'Active': true }));
   };

   // OPEN
   self.openData = function () {
      var rowData = $(this)[0];
      var jsonData = ko.toJS(rowData);
      // jsonData.EntryValue = jsonData.pureEntryValue;
      var modelData = new koAccountModel(jsonData);
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
         },
         show: true
      });
   };

   // SAVE
   self.saveData = function () {
      self.isSaving(true);
      PostDataJson({
         url: GetActionUrl("Accounts", "SaveData"),
         data: self.selectedData(), 
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
         url: GetActionUrl("Accounts", "RemoveData"),
         data: self.selectedData(),
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
