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