var koDeleteUserData = function () {
   var self = this;
   self.isLoading = ko.observable(false);

   // MODAL
   self.showModal = function () {
      $("#koDeleteUserDataModal").modal('open');
   };

   // EXECUTE
   self.execute = function () {
      self.isLoading(true);
      PostDataJson({
         url: GetActionUrl("Auth", "DeleteUserData"),
         data: null, 
         success: function (response) {
            if (response.Result) { window.location = response.Data; }
         },
         done: function () { self.isLoading(false) }
      });
   };

};
var koPasswordContext = function () {
   var self = this;
   self.isLoading = ko.observable(false);

   // DATA
   self.data = ko.observable();

   // LOAD 
   self.loadData = function () {
      self.data(new koPasswordModel());
   };

   // LOGIN
   self.execPassword = function () {
      self.isLoading(true);
      PostDataJson({
         url: GetActionUrl("Auth", "Password"),
         data: self.data(),
         success: function (response) {
            if (response.Result) { window.location = response.Data; }
         },
         form: $('#koPassword'),
         done: function () { self.isLoading(false) }
      });
   };

   // STARTUP 
   self.loadData();
};

function koPasswordModel() {
   this.OldPassword = ko.observable('');
   this.NewPassword = ko.observable('');
   this.ConfirmPassword = ko.observable('');
};