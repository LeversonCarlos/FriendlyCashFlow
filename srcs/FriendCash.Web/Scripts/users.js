var koUsersContext = function () {
   var self = this;
   self.isLoading = ko.observable(false);
   self.isSaving = ko.observable(false);

   // DATA
   self.data = ko.observableArray([]);
   self.dataFiltered = ko.computed(function () {
      return $.grep(self.data(), function (e) { return true; })
   }, self);

   // LOAD 
   self.loadData = function () {
      self.isLoading(true);
      self.data.removeAll();
      GetDataJson({
         url: GetActionUrl("Users", "GetData"),
         success: function (actionBUNDLE) {
            if (actionBUNDLE.Result) {
               $.each(actionBUNDLE.Data, function (key, value) { self.data.push(new koUserModel(value)); });
            }
         },
         done: function () { self.isLoading(false) }
      });
   };

   // OPEN
   self.openData = function () {
      var rowData = $(this)[0];
      var jsonData = ko.toJS(rowData);
      var modelData = new koUserModel(jsonData);
      console.log({ jsonData, modelData: ko.toJS(modelData) });
      self.selectData(modelData);
   };

   // SELECT
   self.selectedData = ko.observable();
   self.selectData = function (val) {
      self.selectedData(val);
      $('#editModal').modal('open');
      $('select').material_select();
   };

   // STARTUP EVENT
   self.loadData();
};

var koUserContext = function () {
   var self = this;
   self.isLoading = ko.observable(false);
   self.isSaving = ko.observable(false);

   /* DATA */
   self.data = ko.observable();
   self.dataSignatures = ko.observableArray();

   /* LOAD */
   self.loadData = function () {
      self.isLoading(true);
      GetDataJson({
         url: GetActionUrl("Users", "GetData") + "?id=" + userID,
         success: function (bundleMain) {
            if (bundleMain.Result) {
               self.data(new koUserModel(bundleMain.Data));
               self.loadSignatures();
            }
         },
         done: function () { self.isLoading(false) }
      });

   };

   /* LOAD SIGNATURES */
   self.loadSignatures = function () {
      self.dataSignatures.removeAll();
      GetDataJson({
         url: GetActionUrl("Users", "GetSignatures") + "?id=" + userID,
         success: function (bundleSignatures) {
            if (bundleSignatures.Result) {
               $.each(bundleSignatures.Data, function (k, v) { self.dataSignatures.push(v); });
            }
         }
      });
   };

   /* ADD SIGNATURES */
   self.addYearSignature = function () { self.addSignature(365); };
   self.addMonthSignature = function () { self.addSignature(30); };
   self.addSignature = function (days) {
      self.isSaving = ko.observable(false);
      PostDataJson({
         url: GetActionUrl("Users", "AddSignature"), 
         data: { idUser: userID, days: days },
         success: function (bundleSignatures) {
            if (bundleSignatures.Result) {
               self.loadSignatures();
            }
         },
         done: function () { self.isSaving(false) }
      });
   };

   /* REMOVE SIGNATURES */
   self.removeSignature = function (item) {
      self.isSaving = ko.observable(false);
      PostDataJson({
         url: GetActionUrl("Users", "RemoveSignature"),
         data: { idUser: userID, idSignature: item.idSignature },
         success: function (bundleSignatures) {
            if (bundleSignatures.Result) {
               self.loadSignatures();
            }
         },
         done: function () { self.isSaving(false) }
      });
   };

   /* STARTUP */
   self.loadData();
};
function koUserModel(value) {
   var self = this;
   self.ID = ko.observable(value.ID);
   self.UserName = ko.observable(value.UserName);
   self.FullName = ko.observable(value.FullName);
   self.Email = ko.observable(value.Email);
   self.EmailConfirmed = ko.observable(value.EmailConfirmed);

   // DATE
   self.pureJoinDate = ko.observable(moment(value.JoinDate));
   self.JoinDate = ko.observable(self.pureJoinDate().format('L'));
   self.pureExpirationDate = ko.observable(moment(value.ExpirationDate));
   self.ExpirationDate = ko.observable(self.pureExpirationDate().format('L'));

   // ROLES
   self.UserRoles = ko.observable(new koUserRolesModel(value.UserRoles, self));

};

function koUserRolesModel(value, parent) {
   var self = this;
   self.Admin = ko.observable(value.Admin);
   self.User = ko.observable(value.User);
   self.Viewer = ko.observable(value.Viewer);
}