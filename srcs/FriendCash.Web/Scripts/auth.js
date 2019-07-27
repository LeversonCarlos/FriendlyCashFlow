var koLoginContext = function (userName) {
   var self = this;
   self.isLoading = ko.observable(false);

   // DATA
   self.data = ko.observable();

   // LOAD 
   self.loadData = function () {
      self.data({ username: userName, password: '', isPersistent: true });
   };

   // LOGIN
   self.execLogin = function () {
      self.isLoading(true);
      PostDataJson({
         url: GetActionUrl("Auth", "Login") + "?returnUrl=" + returnUrl + "",
         data: self.data(),
         checkUserToken: false,
         success: function (response) {
            if (response.Result) { window.location = response.Data; }
         },
         form: $('#koLogin'),
         done: function () { self.isLoading(false) }
      });
   };

   // STARTUP 
   self.loadData();
};
var koRegisterContext = function () {
   var self = this;
   self.isLoading = ko.observable(false);

   // DATA
   self.data = ko.observable();

   // LOAD 
   self.loadData = function () {
      self.data(new koRegisterModel());
   };

   // LOGIN
   self.execRegister = function () {
      self.isLoading(true);
      PostDataJson({
         url: GetActionUrl("Auth", "Register"),
         data: self.data(),
         checkUserToken: false,
         success: function (response) {
            if (response.Result) { window.location = response.Data; }
         },
         form: $('#koRegister'),
         done: function () { self.isLoading(false) }
      });
   };

   // STARTUP 
   self.loadData();
};

function koRegisterModel() {
   this.Email = ko.observable('');
   this.Username = ko.observable('');
   this.Fullname = ko.observable('');
   this.Password = ko.observable('');
   this.ConfirmPassword = ko.observable('');
};